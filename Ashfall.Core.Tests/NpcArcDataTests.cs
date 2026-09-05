using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.NpcArcs;
using Ashfall.Core.Radio;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 52 — authored-data contract gates for the recurring-NPC arc
    /// layer: catalog completeness (24 arcs, 8 flagship with 3+ states),
    /// identity/quest/encounter/location reference resolution, per-arc state
    /// validity, and the four distress-signal arc integrations with their
    /// resolution quests. Run against the real data authority.
    /// </summary>
    public class NpcArcDataTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static HashSet<string> JsonIds(string fileName, string arrayKey)
        {
            using var doc = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir(), fileName)));
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var item in doc.RootElement.GetProperty(arrayKey).EnumerateArray())
                ids.Add(item.GetProperty("id").GetString()!);
            return ids;
        }

        private static NpcArcCatalog Catalog() => NpcArcCatalog.Load(DataDir());

        // ── catalog completeness ───────────────────────────────────────

        [Fact]
        public void Cast_Has84NamedNpcs()
        {
            Assert.Equal(84, JsonIds("characters.json", "items").Count);
        }

        [Fact]
        public void Arcs_24Authored_8FlagshipWithThreePlusStates()
        {
            var arcs = Catalog().Arcs;
            Assert.Equal(24, arcs.Count);
            var flagship = arcs.Where(a => a.flagship).ToList();
            Assert.Equal(8, flagship.Count);
            foreach (var arc in flagship)
                Assert.True(arc.states.Count >= 3, $"{arc.npc_id} flagship arc needs 3+ states");
        }

        [Fact]
        public void Arcs_EveryStateIdUniqueWithinArc()
        {
            foreach (var arc in Catalog().Arcs)
            {
                var stateIds = arc.states.Select(s => s.id).ToList();
                Assert.Equal(stateIds.Count, stateIds.Distinct(StringComparer.Ordinal).Count());
            }
        }

        [Fact]
        public void Arcs_RecruitableArcsCarryRosterDeathTerminalState()
        {
            // Roster death is only reachable for arcs with a recruitment
            // branch, so only those must author the terminal death state —
            // and it must outrank every other authored state in the arc.
            foreach (var arc in Catalog().Arcs.Where(a => a.recruitable))
            {
                var dead = arc.states.FirstOrDefault(s => s.when_dead);
                Assert.True(dead != null, $"{arc.npc_id} (recruitable) is missing a roster-death terminal state");
                Assert.True(dead!.terminal, $"{arc.npc_id} death state must be terminal");
                Assert.True(arc.states.Where(s => s != dead).All(s => s.precedence < dead.precedence),
                    $"{arc.npc_id} death state must have the highest precedence");
            }
            foreach (var arc in Catalog().Arcs.Where(a => a.flagship))
                Assert.Contains(arc.states, s => s.when_dead && s.terminal);
        }

        [Fact]
        public void Arcs_EveryArcNpcExistsInIdentityCatalog()
        {
            var characters = JsonIds("characters.json", "items");
            foreach (var arc in Catalog().Arcs)
                Assert.Contains(arc.npc_id, characters);
        }

        // ── quest reference resolution ─────────────────────────────────

        [Fact]
        public void Arcs_EveryReferencedQuestResolvesInCatalog()
        {
            var quests = new ExpansionQuestSystem();
            quests.BindCatalog(ExpansionQuestCatalogLoader.Load(DataDir()));
            var questIds = JsonIds("quests_npc_arcs.json", "quests");

            foreach (var arc in Catalog().Arcs)
            {
                foreach (var state in arc.states)
                {
                    if (!string.IsNullOrEmpty(state.quest_id))
                    {
                        Assert.True(quests.GetDefinition(state.quest_id) != null,
                            $"{arc.npc_id}/{state.id}: quest_id {state.quest_id} unresolved");
                        Assert.Contains(state.quest_id, questIds);
                    }
                    foreach (var q in state.requires_completed)
                        Assert.True(quests.GetDefinition(q) != null,
                            $"{arc.npc_id}/{state.id}: requires_completed {q} unresolved");
                    foreach (var q in state.excludes_completed)
                        Assert.True(quests.GetDefinition(q) != null,
                            $"{arc.npc_id}/{state.id}: excludes_completed {q} unresolved");
                    foreach (var cond in state.requires_choice)
                    {
                        var def = quests.GetDefinition(cond.quest_id);
                        Assert.True(def != null,
                            $"{arc.npc_id}/{state.id}: requires_choice quest {cond.quest_id} unresolved");
                        Assert.Contains(def!.choices, c => c.id == cond.choice_id);
                    }
                }
            }
        }

        [Fact]
        public void ArcQuests_AllRegisteredInQuestlineMaster()
        {
            var registered = JsonIds("questline_master.json", "entries");
            foreach (var questId in JsonIds("quests_npc_arcs.json", "quests"))
                Assert.Contains(questId, registered);
        }

        // ── encounter contract ─────────────────────────────────────────

        [Fact]
        public void ArcEncounters_LinkNpcAndResolveQuestAndChoice()
        {
            var characters = JsonIds("characters.json", "items");
            var quests = new ExpansionQuestSystem();
            quests.BindCatalog(ExpansionQuestCatalogLoader.Load(DataDir()));

            var engine = new NarrativeEncounterSystem();
            engine.RegisterRange(NarrativeEncounterCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            var arcEncounters = engine.Catalog.Where(e => e.npcId.StartsWith("npc_")).ToList();
            Assert.True(arcEncounters.Count >= 30, $"expected 30+ arc encounters, got {arcEncounters.Count}");

            foreach (var enc in arcEncounters)
            {
                Assert.Contains(enc.npcId, characters);
                foreach (var choice in enc.choices.Where(c => !string.IsNullOrEmpty(c.completesQuestId)))
                {
                    var def = quests.GetDefinition(choice.completesQuestId);
                    Assert.True(def != null, $"{enc.id}: completesQuestId {choice.completesQuestId} unresolved");
                    if (!string.IsNullOrEmpty(choice.completesQuestChoiceId))
                        Assert.Contains(def!.choices, c => c.id == choice.completesQuestChoiceId);
                }
            }
        }

        [Fact]
        public void ArcEncounters_EveryFlagshipDecisionQuestHasACarrier()
        {
            // Every requires_choice condition authored on an arc state must be
            // reachable: some encounter choice must record that exact decision.
            var choicesThatRecord = new HashSet<string>(StringComparer.Ordinal);
            var engine = new NarrativeEncounterSystem();
            engine.RegisterRange(NarrativeEncounterCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));
            foreach (var enc in engine.Catalog)
                foreach (var choice in enc.choices.Where(c => !string.IsNullOrEmpty(c.completesQuestId)))
                    choicesThatRecord.Add(choice.completesQuestId + "::" + choice.completesQuestChoiceId);

            foreach (var arc in Catalog().Arcs)
                foreach (var state in arc.states)
                    foreach (var cond in state.requires_choice)
                        Assert.Contains(cond.quest_id + "::" + cond.choice_id, choicesThatRecord);
        }

        // ── distress integration ───────────────────────────────────────

        [Fact]
        public void Distress_FourArcSignalsLinkNpcAndResolveQuest()
        {
            var characters = JsonIds("characters.json", "items");
            var questIds = JsonIds("quests_npc_arcs.json", "quests");

            using var doc = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir(), "radio_distress_signals_expansion.json")));
            var arcSignals = new List<System.Text.Json.JsonElement>();
            foreach (var signal in doc.RootElement.GetProperty("radio_broadcasts").EnumerateArray())
                if (signal.TryGetProperty("npc_id", out var npc) && npc.GetString()!.StartsWith("npc_"))
                    arcSignals.Add(signal);

            Assert.Equal(4, arcSignals.Count);
            var expectedNpcs = new HashSet<string>(StringComparer.Ordinal)
            {
                "npc_ilze_kaar", "npc_anete_sarn", "npc_marek_voln", "npc_liva_kern"
            };
            foreach (var signal in arcSignals)
            {
                var npcId = signal.GetProperty("npc_id").GetString()!;
                Assert.Contains(npcId, characters);
                Assert.Contains(npcId, expectedNpcs);
                var resolveQuest = signal.GetProperty("resolve_quest_id").GetString()!;
                Assert.Contains(resolveQuest, questIds);
            }
        }

        [Fact]
        public void Distress_SignalNpcArcsStartThroughRealSystem()
        {
            var distress = new RadioDistressSystem();
            distress.LoadFromJson(File.ReadAllText(
                Path.Combine(DataDir(), "radio_distress_signals_expansion.json")));

            foreach (var npcId in new[] { "npc_ilze_kaar", "npc_anete_sarn", "npc_marek_voln", "npc_liva_kern" })
            {
                var def = distress.Definitions.FirstOrDefault(d => d.NpcId == npcId);
                Assert.True(def != null, $"{npcId} has no distress signal in the loaded catalog");
                Assert.False(string.IsNullOrEmpty(def!.ResolveQuestId));
            }
        }

        [Fact]
        public void Distress_SuppressionBlocksInterceptForTerminalArcs()
        {
            var distress = new RadioDistressSystem();
            distress.LoadFromJson(File.ReadAllText(
                Path.Combine(DataDir(), "radio_distress_signals_expansion.json")));

            // A dead arc must suppress its signal at the interception gate.
            distress.NpcSignalSuppressionFilter = npcId => npcId == "npc_liva_kern";
            var def = distress.Definitions.First(d => d.NpcId == "npc_liva_kern");
            Assert.False(distress.Intercept(def.FrequencyId, 60));
        }
    }
}
