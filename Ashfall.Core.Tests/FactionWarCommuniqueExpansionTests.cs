// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 133 — Faction War Communiqués expansion (18 → 40).
    ///
    /// Contract tests for the communiqué corpus in
    /// Assets/StreamingAssets/Data/faction_war_communiques.json. These tests are the
    /// mechanical enforcement of the Plan 133 invariants:
    ///
    ///  - every communiqué references a REAL auto-firing event chain (strict FK;
    ///    the flag-gated evt_p25_* chains may never have fired, so no static public
    ///    statement may reference them — branch-safety invariant 3.3);
    ///  - every factionId resolves against the canonical known-faction list;
    ///  - chronology: a communiqué may not precede the earliest stage of the chain
    ///    it comments on (invariant 3.4);
    ///  - the Forward Roster issues no statement before its public identity exists;
    ///  - authorNote is privileged metadata: it must never appear in any
    ///    player-facing text of any faction-war surface (invariant 3.2), and no
    ///    host code may read it (leakage gate);
    ///  - the original 18 entries are compatibility anchors, preserved verbatim.
    ///
    /// This file is deliberately count-independent for the baseline contract tests;
    /// the exact-40 / per-faction allocation pins live in the same file and are
    /// updated in the expansion phase.
    /// </summary>
    public class FactionWarCommuniqueExpansionTests : CatalogTestBase
    {
        private static FactionWarContentCatalog LoadReal()
        {
            var loader = new FactionWarContentCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            return loader.Load(DataDirectory);
        }

        private static string RepoRoot()
        {
            string dir = Directory.GetCurrentDirectory();
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir, "Assets", "StreamingAssets", "Data")))
                    return dir;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            throw new DirectoryNotFoundException("Repository root not found");
        }

        /// <summary>The 18 baseline entries — compatibility anchors (ids + exact
        /// title text preserved; the plan forbids rewriting them).</summary>
        private static readonly (string id, string title)[] Baseline18 =
        {
            ("comm_d497_garrison_clean_strike", "On the Unclaimed Impact Near the Rail Span"),
            ("comm_d497_rebuilders_clean_strike", "We Don't Have Artillery. We Wish We Knew Who Does."),
            ("comm_d498_ash_sign_clean_strike", "A Ground Chosen, Not Struck"),
            ("comm_d519_garrison_almshouse", "On the Loss of the Former Almshouse"),
            ("comm_d520_rebuilders_almshouse", "Insurgent Ordnance Doesn't Overshoot From a Garrison Emplacement"),
            ("comm_d521_ash_sign_almshouse", "The Reading Was Given"),
            ("comm_d537_garrison_exchange_checkpoint", "Continuity of Distribution, Formally Secured"),
            ("comm_d538_rebuilders_exchange_checkpoint", "Call It What It Is"),
            ("comm_d549_garrison_ration_plaza", "On the Loss at Ration Plaza"),
            ("comm_d550_rebuilders_ration_plaza", "There Was No Convoy"),
            ("comm_d552_ash_sign_ration_plaza", "A Harder Reading"),
            ("comm_d573_forward_roster_checkpoint", "What We Are, Plainly, Since People Keep Asking"),
            ("comm_d581_garrison_shrine_strike", "On the Reported Impact at the Ash Sign Shrine"),
            ("comm_d582_rebuilders_shrine_strike", "We Don't Know Either, and We're Saying So"),
            ("comm_d583_ash_sign_shrine_strike", "What We No Longer Claim"),
            ("comm_d591_ash_sign_ceasefire_pause", "On the Pause, Since Pilgrims Are Asking"),
            ("comm_d593_forward_roster_ceasefire_toll", "Nobody Asked Us, So We're Saying It Anyway"),
            ("comm_d607_garrison_forward_roster_recognition", "On the Unlicensed Checkpoint, Spur Road West"),
        };

        private static readonly string[] KnownFactions =
        {
            "faction_central_garrison", "faction_rebuilders", "faction_black_ops",
            "faction_ash_sign", "faction_forward_roster", "warlords_sector_4",
            "faction_hydro_barons", "faction_railway_guild", "faction_supply_corps",
            "faction_scavenger_guild",
        };

        // ── Baseline contract (count-independent) ─────────────────────────────

        [Fact]
        public void Catalog_LoadsAtLeastTheBaselineCorpus()
        {
            var catalog = LoadReal();
            Assert.True(catalog.CommuniqueCount >= 18,
                $"expected at least the 18 baseline communiques, found {catalog.CommuniqueCount}");
        }

        [Fact]
        public void Existing18Ids_PreservedVerbatim()
        {
            var catalog = LoadReal();
            foreach (var (id, title) in Baseline18)
            {
                var c = Assert.Single(catalog.Communiques, x => x.id == id);
                Assert.Equal(title, c.title);
                Assert.False(string.IsNullOrEmpty(c.body), $"{id} body must remain present");
            }
        }

        [Fact]
        public void Existing18Ids_RemainInChronologicalFileOrder()
        {
            var catalog = LoadReal();
            // The baseline order (by day) must be preserved as the authored
            // presentation order of the merged file.
            var indexOf = catalog.Communiques
                .Select((c, i) => (c, i))
                .ToDictionary(x => x.c.id, x => x.i, StringComparer.Ordinal);
            var positions = Baseline18.Select(anchor => indexOf[anchor.id]).ToList();
            for (int i = 1; i < positions.Count; i++)
                Assert.True(positions[i - 1] < positions[i],
                    $"baseline order broken at anchor {Baseline18[i].id} (positions {positions[i - 1]} >= {positions[i]})");
        }

        [Fact]
        public void AllIds_UniqueAndNonEmpty()
        {
            var catalog = LoadReal();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var c in catalog.Communiques)
            {
                Assert.False(string.IsNullOrEmpty(c.id), "communique id must be non-empty");
                Assert.True(seen.Add(c.id), $"duplicate communique id: {c.id}");
            }
        }

        [Fact]
        public void AllEventChainIds_ResolveToRealChains()
        {
            var catalog = LoadReal();
            var chainIds = new HashSet<string>(
                catalog.EventChains.Select(ch => ch.chainId), StringComparer.Ordinal);
            foreach (var c in catalog.Communiques)
            {
                Assert.False(string.IsNullOrEmpty(c.eventChainId), $"{c.id}: eventChainId empty");
                Assert.True(chainIds.Contains(c.eventChainId),
                    $"{c.id}: eventChainId '{c.eventChainId}' does not resolve to any chain");
            }
        }

        [Fact]
        public void AllFactionIds_ResolveToKnownFactions()
        {
            var catalog = LoadReal();
            foreach (var c in catalog.Communiques)
            {
                Assert.True(KnownFactions.Contains(c.factionId),
                    $"{c.id}: factionId '{c.factionId}' is not a canonical faction");
            }
        }

        [Fact]
        public void TitlesAndBodies_NonEmpty_DayPositive()
        {
            var catalog = LoadReal();
            foreach (var c in catalog.Communiques)
            {
                Assert.False(string.IsNullOrEmpty(c.title), $"{c.id}: title empty");
                Assert.False(string.IsNullOrEmpty(c.body), $"{c.id}: body empty");
                Assert.True(c.day > 0, $"{c.id}: day must be positive, found {c.day}");
            }
        }

        [Fact]
        public void Chronology_NoCommuniquePrecedesTheEarliestStageOfItsChain()
        {
            var catalog = LoadReal();
            var chains = catalog.EventChains.ToDictionary(ch => ch.chainId, StringComparer.Ordinal);
            foreach (var c in catalog.Communiques)
            {
                var chain = chains[c.eventChainId];
                Assert.NotEmpty(chain.stages);
                var earliest = chain.stages.Min(s => s.minDay);
                Assert.True(c.day >= earliest,
                    $"{c.id}: day {c.day} precedes earliest stage {earliest} of {c.eventChainId}");
            }
        }

        [Fact]
        public void Communiques_NeverReferenceFlagGatedChains()
        {
            // Plan 133 invariant 3.3/3.5: the evt_p25_* chains fire only while a
            // campaign grievance flag is set — in campaigns where the flag never
            // fired, the events never happened. A static public statement
            // referencing them would be branch-impossible.
            var catalog = LoadReal();
            foreach (var c in catalog.Communiques)
            {
                Assert.False(c.eventChainId.StartsWith("evt_p25_", StringComparison.Ordinal),
                    $"{c.id}: references flag-gated chain {c.eventChainId}");
            }
        }

        [Fact]
        public void ForwardRoster_IssuesNoStatementBeforeItsPublicIdentity()
        {
            // The Roster's first public statement (baseline comm_d573) is its
            // self-introduction; no communiqué may precede it.
            var catalog = LoadReal();
            foreach (var c in catalog.Communiques)
            {
                if (c.factionId == "faction_forward_roster")
                    Assert.True(c.day >= 573,
                        $"{c.id}: Forward Roster statement at day {c.day} predates its identity");
            }
        }

        [Fact]
        public void CeasefireStatements_DoNotPrecedeTheCeasefire()
        {
            var catalog = LoadReal();
            foreach (var c in catalog.Communiques)
            {
                if (c.eventChainId == "evt_d588_ceasefire_by_exhaustion")
                    Assert.True(c.day >= 588,
                        $"{c.id}: ceasefire statement at day {c.day} precedes the talks");
            }
        }

        [Fact]
        public void GetCommuniquesForFaction_DayBoundary_IsExact()
        {
            var catalog = LoadReal();
            var all = catalog.Communiques;
            Assert.NotEmpty(all);
            var sample = all[0];
            var atDay = catalog.GetCommuniquesForFaction(sample.factionId, sample.day);
            Assert.Contains(sample, atDay);
            var beforeDay = catalog.GetCommuniquesForFaction(sample.factionId, sample.day - 1);
            Assert.DoesNotContain(sample, beforeDay);
        }

        [Fact]
        public void MultipleCommuniquesPerFactionPerDay_AreLegal()
        {
            var catalog = LoadReal();
            // Baseline already ships two same-faction... two same-DAY entries
            // (comm_d497_garrison_clean_strike + comm_d497_rebuilders_clean_strike);
            // the corpus also exercises multiple entries per faction per chain.
            var byFaction = catalog.Communiques
                .GroupBy(c => c.factionId)
                .ToDictionary(g => g.Key, g => g.ToList());
            Assert.True(byFaction["faction_central_garrison"].Count >= 2,
                "multiple garrison communiques expected");
        }

        // ── authorNote truth-layer protection (invariant 3.2) ─────────────────

        [Fact]
        public void AuthorNote_NeverAppearsInPlayerFacingText()
        {
            var catalog = LoadReal();
            // Player-facing text surfaces of the faction-war content layer:
            // communiqué titles/bodies, radio messages, journal bodies, dialogue
            // bodies. Location-override display strings are presentation-only
            // place descriptions but are included for completeness.
            var playerFacing = new List<string>();
            foreach (var c in catalog.Communiques)
            {
                playerFacing.Add(c.title);
                playerFacing.Add(c.body);
            }
            foreach (var b in catalog.Broadcasts) playerFacing.Add(b.message);
            foreach (var j in catalog.JournalEntries) playerFacing.Add(j.body);
            foreach (var d in catalog.DialogueSnippets) playerFacing.Add(d.body);
            foreach (var o in catalog.LocationOverrides) playerFacing.Add(o.description);

            static string Norm(string s) =>
                System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ").Trim();

            foreach (var c in catalog.Communiques)
            {
                if (string.IsNullOrEmpty(c.authorNote)) continue;
                var note = Norm(c.authorNote);
                Assert.False(c.body.Contains(note, StringComparison.Ordinal),
                    $"{c.id}: authorNote text found in body");
                Assert.False(c.title.Contains(note, StringComparison.Ordinal),
                    $"{c.id}: authorNote text found in title");
                foreach (var text in playerFacing)
                {
                    Assert.False(Norm(text).Contains(note, StringComparison.Ordinal),
                        $"authorNote of {c.id} leaked into player-facing text");
                }
            }
        }

        [Fact]
        public void AuthorNote_NoPlayerFacingConsumers_Gate()
        {
            // Source-scan gate: no host (Godot) code may read the authorNote
            // field. If a renderer appears that does, this test fails and the
            // surface must be audited before ship.
            var srcDir = Path.Combine(RepoRoot(), "src");
            Assert.True(Directory.Exists(srcDir));
            var offenders = Directory
                .EnumerateFiles(srcDir, "*.cs", SearchOption.AllDirectories)
                .Where(f => File.ReadAllText(f).Contains("authorNote", StringComparison.Ordinal))
                .Select(Path.GetFileName)
                .ToList();
            Assert.Empty(offenders);
        }

        // ── Loader failure-policy fixtures (negative fixtures, temp data) ─────

        private static string TempDataDir()
        {
            var dir = Path.Combine(Path.GetTempPath(), "plan133-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static FactionWarContentCatalog LoadSingleFile(string json)
        {
            var dir = TempDataDir();
            try
            {
                File.WriteAllText(Path.Combine(dir, FactionWarContentCatalogLoader.CommuniquesFile), json);
                var loader = new FactionWarContentCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
                return loader.Load(dir);
            }
            finally
            {
                Directory.Delete(dir, recursive: true);
            }
        }

        [Fact]
        public void Loader_DuplicateId_BothLoad_DocumentedPolicy()
        {
            // Current contract: the loader enforces only non-empty id; duplicates
            // both load in file order (documented failure policy). Uniqueness of
            // the shipped corpus is enforced by AllIds_UniqueAndNonEmpty above.
            var catalog = LoadSingleFile(@"{ ""schema_version"": 1, ""communiques"": [
                { ""id"": ""comm_dup"", ""eventChainId"": ""evt_x"", ""factionId"": ""faction_central_garrison"",
                  ""day"": 1, ""title"": ""t1"", ""body"": ""b1"" },
                { ""id"": ""comm_dup"", ""eventChainId"": ""evt_x"", ""factionId"": ""faction_central_garrison"",
                  ""day"": 2, ""title"": ""t2"", ""body"": ""b2"" } ] }");
            Assert.Equal(2, catalog.CommuniqueCount);
            Assert.Equal("t1", catalog.Communiques[0].title);
            Assert.Equal("t2", catalog.Communiques[1].title);
        }

        [Fact]
        public void Loader_UnknownEventChainAndFaction_StillLoad_DocumentedPolicy()
        {
            // No FK enforcement exists at load time (see PLAN133_BASELINE.md §0.2);
            // this pins the tolerant behavior. Integrity is enforced by tests.
            var catalog = LoadSingleFile(@"{ ""schema_version"": 1, ""communiques"": [
                { ""id"": ""comm_x"", ""eventChainId"": ""evt_does_not_exist"",
                  ""factionId"": ""faction_does_not_exist"", ""day"": 1, ""title"": ""t"", ""body"": ""b"" } ] }");
            Assert.Equal(1, catalog.CommuniqueCount);
            Assert.Equal("evt_does_not_exist", catalog.Communiques[0].eventChainId);
        }

        [Fact]
        public void Loader_NegativeDayAndEmptyBody_StillLoad_DocumentedPolicy()
        {
            // The loader has no day/body validation; the shipped corpus contract
            // (positive day, non-empty body) is enforced by
            // TitlesAndBodies_NonEmpty_DayPositive. Pin the loader's tolerance so
            // the two layers don't silently drift apart.
            var catalog = LoadSingleFile(@"{ ""schema_version"": 1, ""communiques"": [
                { ""id"": ""comm_neg"", ""eventChainId"": ""evt_x"", ""factionId"": ""f"",
                  ""day"": -1, ""title"": ""t"", ""body"": """" } ] }");
            Assert.Equal(1, catalog.CommuniqueCount);
            Assert.Equal(-1, catalog.Communiques[0].day);
            Assert.Equal(string.Empty, catalog.Communiques[0].body);
        }

        [Fact]
        public void Loader_AuthorNote_AbsentDeserializesToEmpty()
        {
            var catalog = LoadSingleFile(@"{ ""schema_version"": 1, ""communiques"": [
                { ""id"": ""comm_a"", ""eventChainId"": ""evt_x"", ""factionId"": ""f"",
                  ""day"": 1, ""title"": ""t"", ""body"": ""b"" },
                { ""id"": ""comm_b"", ""eventChainId"": ""evt_x"", ""factionId"": ""f"",
                  ""day"": 2, ""title"": ""t"", ""body"": ""b"", ""authorNote"": ""Privileged truth."" } ] }");
            Assert.Equal(string.Empty, catalog.Communiques[0].authorNote);
            Assert.Equal("Privileged truth.", catalog.Communiques[1].authorNote);
        }

        [Fact]
        public void Loader_ParseFailure_IsToleratedPerFile()
        {
            var dir = TempDataDir();
            try
            {
                File.WriteAllText(
                    Path.Combine(dir, FactionWarContentCatalogLoader.CommuniquesFile), "{ not json");
                var loader = new FactionWarContentCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
                var catalog = loader.Load(dir);
                Assert.Equal(0, catalog.CommuniqueCount);
            }
            finally
            {
                Directory.Delete(dir, recursive: true);
            }
        }

        // ── Post-expansion pins (Plan 133 final state) ─────────────────────

        /// <summary>The corpus ships exactly 40 communiqués: the 18 baseline
        /// anchors plus 22 Plan 133 additions (Garrison +7, Rebuilders +7,
        /// Ash Sign +4, Forward Roster +4).</summary>
        [Fact]
        public void Catalog_LoadsExactly40Communiques()
        {
            var catalog = LoadReal();
            Assert.Equal(40, catalog.CommuniqueCount);
        }

        [Fact]
        public void Allocation_MatchesPlan133_Distribution()
        {
            var catalog = LoadReal();
            var byFaction = catalog.Communiques.GroupBy(c => c.factionId)
                .ToDictionary(g => g.Key, g => g.Count());
            // baseline 6/5/5/2 + additions 7/7/4/4
            Assert.Equal(13, byFaction["faction_central_garrison"]);
            Assert.Equal(12, byFaction["faction_rebuilders"]);
            Assert.Equal(9, byFaction["faction_ash_sign"]);
            Assert.Equal(6, byFaction["faction_forward_roster"]);
        }

        [Fact]
        public void Coverage_AtLeast10Chains_HaveTwoCompetingPerspectives()
        {
            var catalog = LoadReal();
            var multi = catalog.Communiques
                .GroupBy(c => c.eventChainId)
                .Where(g => g.Select(x => x.factionId).Distinct().Count() >= 2)
                .Select(g => g.Key)
                .ToList();
            Assert.True(multi.Count >= 10,
                $"expected >=10 chains with competing perspectives, found {multi.Count}: {string.Join(", ", multi)}");
        }

        [Fact]
        public void Coverage_AtLeast5Chains_HaveThreePerspectives()
        {
            var catalog = LoadReal();
            var triplets = catalog.Communiques
                .GroupBy(c => c.eventChainId)
                .Where(g => g.Select(x => x.factionId).Distinct().Count() >= 3)
                .Select(g => g.Key)
                .ToList();
            Assert.True(triplets.Count >= 5,
                $"expected >=5 chains with three perspectives, found {triplets.Count}");
        }

        [Fact]
        public void Coverage_ColdWarAndOpenConflictBands_AreNoLongerUncovered()
        {
            // Baseline skew: zero communiqués before day 495. Plan 133 covers the
            // cold-war manifest holdup (d488) and the open-conflict band
            // (d503, d509, d522, d524).
            var catalog = LoadReal();
            var chains = catalog.Communiques.Select(c => c.eventChainId).ToHashSet();
            foreach (var required in new[]
                     {
                         "evt_d488_manifest_holdup", "evt_d503_conscription_lists",
                         "evt_d509_border_clash_span44", "evt_d522_switchback_toll",
                         "evt_d524_market_price_spike", "evt_d552_rebuilders_fracture",
                         "evt_d558_ln74_signal_intercept", "evt_d565_hydro_leverage_break",
                         "evt_d600_theory_surfaces",
                     })
                Assert.Contains(required, chains);
        }

        [Fact]
        public void Repetition_NoDuplicateTitles_AndOnTheSkeletonDoesNotGrow()
        {
            var catalog = LoadReal();
            var titles = catalog.Communiques.Select(c => c.title).ToList();
            Assert.Equal(titles.Count, titles.Distinct(StringComparer.Ordinal).Count());
            // Repetition audit rule: the baseline carries 6 "On the …" titles
            // (4 Garrison, 2 Ash Sign); no new entry may add another.
            var onThe = titles.Count(t => t.StartsWith("On the ", StringComparison.Ordinal));
            Assert.True(onThe <= 6, $"'On the' title skeleton grew to {onThe}");
        }

        [Fact]
        public void Repetition_ComeThroughClean_IsBoundedSignatureUse()
        {
            // Forward Roster signature closer — deliberately reused, but bounded.
            var uses = 0;
            var catalog = LoadReal();
            foreach (var c in catalog.Communiques)
                if (c.factionId == "faction_forward_roster" &&
                    c.body.Contains("Come through clean", StringComparison.Ordinal))
                    uses++;
            Assert.InRange(uses, 1, 3);
        }

        [Fact]
        public void Temporal_Distribution_IsNotClusteredInOneBand()
        {
            var catalog = LoadReal();
            var byBand = new Dictionary<string, int>
            {
                ["cold_war_to_498"] = 0,
                ["open_conflict_to_530"] = 0,
                ["offensive_to_561"] = 0,
                ["culmination_to_608"] = 0,
            };
            foreach (var c in catalog.Communiques)
            {
                if (c.day <= 498) byBand["cold_war_to_498"]++;
                else if (c.day <= 530) byBand["open_conflict_to_530"]++;
                else if (c.day <= 561) byBand["offensive_to_561"]++;
                else byBand["culmination_to_608"]++;
            }
            foreach (var (band, count) in byBand)
                Assert.True(count >= 4, $"band {band} too thin: {count}");
        }

        [Fact]
        public void NoCommunique_AssertsPossessionOfTheBranchSensitiveIntercept()
        {
            // evt_d558 branch-safety: who received the LN74 intercept is a player
            // choice. No faction may claim the intercept/pad itself (the radio burst
            // is public; the pad is not).
            var catalog = LoadReal();
            foreach (var c in catalog.Communiques)
            {
                if (c.eventChainId != "evt_d558_ln74_signal_intercept") continue;
                Assert.False(c.body.Contains("intercept", StringComparison.OrdinalIgnoreCase),
                    $"{c.id}: claims possession of the branch-sensitive intercept");
                Assert.False(c.body.Contains("pad", StringComparison.Ordinal),
                    $"{c.id}: references the branch-sensitive cipher pad");
            }
        }

        [Fact]
        public void FactionWarChainRunner_ExposesCatalogProperty()
        {
            var catalog = new FactionWarContentCatalog();
            var runner = new FactionWarChainRunner(catalog);
            Assert.Same(catalog, runner.Catalog);
        }

        [Fact]
        public void YearOfAshTimeline_ClampsPastDay360_PhaseA()
        {
            var timeline = new YearOfAshTimelineSystem();
            timeline.AdvanceDay(500);
            Assert.Equal(360, timeline.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase6_TheGreatThaw, timeline.CurrentPhase);
        }
    }
}
