using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>Scriptable eligibility for selector tests (flag answers from a set,
    /// alive answers from a dead-set, faction answers from a present-set).</summary>
    internal class ScriptedWitnessEligibility : IWitnessEligibility
    {
        public HashSet<string> Flags = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> DeadSubjects = new HashSet<string>(StringComparer.Ordinal);
        public HashSet<string> AbsentFactions = new HashSet<string>(StringComparer.Ordinal);

        public bool IsFlagSet(string flagId) => !string.IsNullOrEmpty(flagId) && Flags.Contains(flagId);
        public bool IsSubjectAlive(string subjectId) => !DeadSubjects.Contains(subjectId);
        public bool IsFactionPresent(string factionId) => !AbsentFactions.Contains(factionId);
    }

    public class WitnessSelectionTests : IDisposable
    {
        private readonly string _tempDir;

        public WitnessSelectionTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ashfall_witness_sel_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            try { Directory.Delete(_tempDir, true); } catch { /* best effort */ }
        }

        private List<WitnessDefinition> WriteAndLoad(string json)
        {
            File.WriteAllText(Path.Combine(_tempDir, WitnessCatalogLoader.FileName), json);
            return WitnessCatalogLoader.LoadWitnesses(_tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static WitnessDefinition W(string id, int dayMin = 0, int priority = 0,
            string factionId = "", string subjectId = "", params WitnessTestimony[] testimonies)
        {
            var w = new WitnessDefinition
            {
                id = id,
                witnessName = id,
                locationId = "loc_test",
                knowledgeKey = "history_test",
                dayMin = dayMin,
                priority = priority,
                factionId = factionId,
                subjectId = subjectId
            };
            w.testimonies.AddRange(testimonies.Length > 0
                ? testimonies
                : new[] { new WitnessTestimony { variantId = "account", body = id + " speaks." } });
            return w;
        }

        // ── loader ─────────────────────────────────────────────────────

        [Fact]
        public void Loader_V2EntriesCarryFactionSubjectPriorityAndVariants()
        {
            var witnesses = WriteAndLoad(@"{""schema_version"":2,""witnesses"":[
{""id"":""witness_x"",""witness_name"":""The X"",""location_id"":""loc_x"",""knowledge_key"":""history_x"",
""day_min"":200,""faction_id"":""faction_scavenger_guild"",""subject_id"":""npc_x"",""priority"":25,
""testimonies"":[
{""variant_id"":""helped"",""requires_all_flags"":[""flag_helped_x""],""body"":""You paid.""},
{""variant_id"":""failed"",""requires_any_flags"":[""flag_grief_a"",""flag_grief_b""],""forbids_flags"":[""flag_made_amends""],""body"":""You never came.""},
{""variant_id"":""absent"",""body"":""Who?""}]}]}");
            Assert.Single(witnesses);
            var w = witnesses[0];
            Assert.Equal("faction_scavenger_guild", w.factionId);
            Assert.Equal("npc_x", w.subjectId);
            Assert.Equal(25, w.priority);
            Assert.Equal(3, w.testimonies.Count);
            Assert.Equal("You paid.", w.testimonies[0].body);
            // v1-compat mirror: body = first testimony body
            Assert.Equal("You paid.", w.body);
            Assert.Equal("flag_grief_b", w.testimonies[1].requiresAnyFlags[1]);
            Assert.Equal("flag_made_amends", w.testimonies[1].forbidsFlags[0]);
        }

        [Fact]
        public void Loader_V1FlatBodyBecomesOneUnconditionalTestimony()
        {
            var witnesses = WriteAndLoad(@"{""schema_version"":1,""witnesses"":[
{""id"":""witness_old"",""witness_name"":""Old"",""location_id"":""loc_o"",""knowledge_key"":""history_o"",
""day_min"":241,""body"":""Flat account.""}]}");
            Assert.Single(witnesses);
            var w = witnesses[0];
            Assert.Single(w.testimonies);
            Assert.Equal("account", w.testimonies[0].variantId);
            Assert.Empty(w.testimonies[0].requiresAllFlags);
            Assert.Equal("Flat account.", w.body);
            Assert.Equal(string.Empty, w.factionId);
        }

        [Fact]
        public void Loader_RejectsFutureSchema()
        {
            var witnesses = WriteAndLoad(@"{""schema_version"":99,""witnesses"":[
{""id"":""witness_future"",""body"":""nope""}]}");
            Assert.Empty(witnesses);
        }

        // ── selection ──────────────────────────────────────────────────

        [Fact]
        public void Select_DayGateExcludesEarlyWitnesses()
        {
            var gate = new ScriptedWitnessEligibility();
            var list = new[] { W("early", dayMin: 200), W("late", dayMin: 300) };
            var day200 = WitnessSelector.Select(list, 200, gate);
            Assert.Equal(new[] { "early" }, new[] { day200[0].Witness.id });
            var day300 = WitnessSelector.Select(list, 300, gate);
            Assert.Equal(2, day300.Count);
        }

        [Fact]
        public void Select_DeadSubjectNeverTestifies()
        {
            var gate = new ScriptedWitnessEligibility();
            gate.DeadSubjects.Add("npc_dead");
            var list = new[] { W("haunted", subjectId: "npc_dead"), W("alive", subjectId: "npc_living") };
            var selected = WitnessSelector.Select(list, 300, gate);
            Assert.Equal(new[] { "alive" }, new[] { selected[0].Witness.id });
        }

        [Fact]
        public void Select_EmptySubjectIdSkipsTheCensusCheck()
        {
            var gate = new ScriptedWitnessEligibility();
            var list = new[] { W("institutional", factionId: "faction_hydro_barons") };
            Assert.Single(WitnessSelector.Select(list, 300, gate));
        }

        [Fact]
        public void Select_FactionPresenceGatesFactionWitnesses()
        {
            var gate = new ScriptedWitnessEligibility();
            gate.AbsentFactions.Add("faction_iron_raiders");
            var list = new[]
            {
                W("raider_rep", factionId: "faction_iron_raiders"),
                W("guild_rep", factionId: "faction_scavenger_guild"),
                W("no_faction")
            };
            var selected = WitnessSelector.Select(list, 300, gate);
            Assert.Equal(2, selected.Count);
            Assert.DoesNotContain(selected, d => d.Witness.id == "raider_rep");
        }

        [Fact]
        public void SelectTestimony_FirstAuthoredMatchWins()
        {
            var gate = new ScriptedWitnessEligibility();
            gate.Flags.Add("flag_helped_x");
            gate.Flags.Add("flag_grief_b");
            var w = W("w", testimonies: new[]
            {
                new WitnessTestimony { variantId = "helped", requiresAllFlags = { "flag_helped_x" }, body = "helped" },
                new WitnessTestimony { variantId = "failed", requiresAnyFlags = { "flag_grief_a", "flag_grief_b" }, forbidsFlags = { "flag_made_amends" }, body = "failed" },
                new WitnessTestimony { variantId = "absent", body = "absent" }
            });
            Assert.Equal("helped", WitnessSelector.SelectTestimony(w, gate).variantId);

            gate.Flags.Remove("flag_helped_x");
            Assert.Equal("failed", WitnessSelector.SelectTestimony(w, gate).variantId);

            gate.Flags.Add("flag_made_amends"); // forbids now closed on 'failed'
            Assert.Equal("absent", WitnessSelector.SelectTestimony(w, gate).variantId);
        }

        [Fact]
        public void SelectTestimony_AllConditionsUnmetYieldsNull()
        {
            var gate = new ScriptedWitnessEligibility();
            var w = W("w", testimonies: new[]
            {
                new WitnessTestimony { variantId = "helped", requiresAllFlags = { "flag_never_set" }, body = "helped" }
            });
            Assert.Null(WitnessSelector.SelectTestimony(w, gate));
        }

        [Fact]
        public void Select_OrdersByPriorityThenIdOrdinal()
        {
            var gate = PassAllWitnessEligibility.Instance;
            var list = new[]
            {
                W("b_mid", priority: 10),
                W("a_mid", priority: 10),
                W("z_high", priority: 50),
                W("a_low", priority: 0)
            };
            var selected = WitnessSelector.Select(list, 300, gate);
            Assert.Equal(new[] { "z_high", "a_mid", "b_mid", "a_low" },
                new[] { selected[0].Witness.id, selected[1].Witness.id, selected[2].Witness.id, selected[3].Witness.id });
        }

        [Fact]
        public void Select_CapPreservesPriorityAndFactionDiversity()
        {
            var gate = PassAllWitnessEligibility.Instance;
            var list = new[]
            {
                W("guild_1", priority: 30, factionId: "faction_scavenger_guild"),
                W("guild_2", priority: 20, factionId: "faction_scavenger_guild"),
                W("hydro_1", priority: 20, factionId: "faction_hydro_barons"),
                W("solo", priority: 20)
            };
            // Priority order: guild_1, then (guild_2, hydro_1, solo by id). Cap 2
            // must not take two guild witnesses: round-robin gives guild_1 + hydro_1.
            var capped = WitnessSelector.Select(list, 300, gate, maxCount: 2);
            Assert.Equal(2, capped.Count);
            Assert.Equal("guild_1", capped[0].Witness.id);
            Assert.Equal("hydro_1", capped[1].Witness.id);

            var uncapped = WitnessSelector.Select(list, 300, gate);
            Assert.Equal(4, uncapped.Count);
        }

        [Fact]
        public void Select_DuplicateIdsAreDropped()
        {
            var gate = PassAllWitnessEligibility.Instance;
            var list = new[] { W("dup"), W("dup"), W("other") };
            Assert.Equal(2, WitnessSelector.Select(list, 300, gate).Count);
        }

        [Fact]
        public void Select_DeterministicAcrossRepeatedCalls()
        {
            var gate = new ScriptedWitnessEligibility();
            gate.Flags.Add("flag_grief_a");
            var list = new[]
            {
                W("w1", priority: 5, factionId: "faction_scavenger_guild"),
                W("w2", priority: 5),
                W("w3", dayMin: 280),
                W("w4", testimonies: new[]
                {
                    new WitnessTestimony { variantId = "failed", requiresAnyFlags = { "flag_grief_a" }, body = "f" },
                    new WitnessTestimony { variantId = "absent", body = "a" }
                })
            };
            for (int round = 0; round < 3; round++)
            {
                var run = WitnessSelector.Select(list, 300, gate);
                Assert.Equal(4, run.Count);
                Assert.Equal("w1,w2,w3,w4", string.Join(",", new[]
                {
                    run[0].Witness.id, run[1].Witness.id, run[2].Witness.id, run[3].Witness.id
                }));
                Assert.Equal("failed", run[3].VariantId); // w4: flag_grief_a set → first match is the failed variant
            }
        }

        [Fact]
        public void Select_NullEligibilityDefaultsToPassAll()
        {
            var list = new[] { W("w1"), W("w2", dayMin: 400) };
            var selected = WitnessSelector.Select(list, 300, null);
            Assert.Single(selected);
        }
    }
}
