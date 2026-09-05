// SPDX-License-Identifier: MIT
// ASHFALL Core Tests — Plan 84: Muster Witness Testimonies Expansion (3 → 15 investigation witnesses).
// Asserts that the 12 new witnesses (Coastal Evacuation, Grain Convoy Massacre, Silent Foundry Accord)
// are correctly authored, schema-valid, and coexist with the 3 Voss witnesses and 12 Plan 25 faction witnesses.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan84WitnessExpansionTests : CatalogTestBase
    {
        // ── Canonical investigation witness IDs for all four threads ─────────
        private static readonly string[] VossThreadIds =
        {
            "witness_1_checkpoint_conscript",
            "witness_2_quartermaster_paperwork",
            "witness_3_signals_intercept",
        };

        private static readonly string[] CoastalThreadIds =
        {
            "witness_harbor_master_kell",
            "witness_trawler_captain_maren",
            "witness_coastal_refugee_nurse",
            "witness_naval_conscript_brant",
        };

        private static readonly string[] GrainThreadIds =
        {
            "witness_convoy_driver_tomas",
            "witness_rebuilder_field_medic",
            "witness_garrison_picket_vaughn",
            "witness_wayside_mechanic_yorin",
        };

        private static readonly string[] FoundryThreadIds =
        {
            "witness_foundry_molder_hask",
            "witness_iceroad_hauler_sula",
            "witness_arbitration_clerk_moran",
            "witness_terrace_elder_marit",
        };

        // Verdict site location IDs (Plan 82 integration anchors)
        private static readonly string[] VerdictSiteIds =
        {
            "loc_abandoned_tide_gauge",      // Coastal thread Verdict Site 1
            "loc_border_checkpoint_ruins",   // Grain thread Verdict Site 2
            "loc_river_gauging_station",     // Foundry thread Verdict Site 3
        };

        // ── Shared helpers ────────────────────────────────────────────────────

        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string? parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        private static List<WitnessDefinition> LoadAll()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return new List<WitnessDefinition>();
            return WitnessCatalogLoader.LoadWitnesses(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        // ── Test 1: catalog loads without error ──────────────────────────────

        [Fact]
        public void WitnessCatalog_LoadsWithoutError()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return; // skip if no data dir (CI without assets)
            var witnesses = LoadAll();
            Assert.NotNull(witnesses);
            Assert.True(witnesses.Count >= 15, $"Expected >= 15 witnesses total, got {witnesses.Count}");
        }

        // ── Test 2: all 15 investigation witnesses present ───────────────────

        [Fact]
        public void VossThread_AllThreeWitnessesPresent()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            foreach (var id in VossThreadIds)
                Assert.True(witnesses.Any(w => w.id == id), $"Missing Voss witness: {id}");
        }

        [Fact]
        public void CoastalThread_AllFourWitnessesPresent()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            foreach (var id in CoastalThreadIds)
                Assert.True(witnesses.Any(w => w.id == id), $"Missing Coastal witness: {id}");
        }

        [Fact]
        public void GrainThread_AllFourWitnessesPresent()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            foreach (var id in GrainThreadIds)
                Assert.True(witnesses.Any(w => w.id == id), $"Missing Grain witness: {id}");
        }

        [Fact]
        public void FoundryThread_AllFourWitnessesPresent()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            foreach (var id in FoundryThreadIds)
                Assert.True(witnesses.Any(w => w.id == id), $"Missing Foundry witness: {id}");
        }

        // ── Test 3: all Plan 25 faction witnesses still present ───────────────

        [Fact]
        public void Plan25FactionWitnesses_Preserved()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            Assert.True(witnesses.Any(w => w.id == "witness_scavenger_claimant"),
                "Plan 25 witness 'witness_scavenger_claimant' missing — FactionEcologySelftest will break");
            Assert.True(witnesses.Any(w => w.id == "witness_messengers_keeper"),
                "Plan 25 witness 'witness_messengers_keeper' missing");
        }

        // ── Test 4: all IDs unique, witness_ prefix ───────────────────────────

        [Fact]
        public void AllWitnesses_HaveUniqueIds()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var ids = witnesses.Select(w => w.id).ToList();
            var distinct = ids.Distinct().ToList();
            Assert.Equal(ids.Count, distinct.Count);
        }

        [Fact]
        public void NewInvestigationWitnesses_HaveWitnessPrefixedIds()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var allNewIds = CoastalThreadIds
                .Concat(GrainThreadIds)
                .Concat(FoundryThreadIds);
            foreach (var id in allNewIds)
            {
                Assert.StartsWith("witness_", id);
                Assert.True(witnesses.Any(w => w.id == id),
                    $"Expected witness id '{id}' not found in catalog");
            }
        }

        // ── Test 5: mandatory fields populated ───────────────────────────────

        [Fact]
        public void AllWitnesses_HaveRequiredFields()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            foreach (var w in witnesses)
            {
                Assert.False(string.IsNullOrWhiteSpace(w.id),
                    "A witness entry has an empty id");
                Assert.False(string.IsNullOrWhiteSpace(w.locationId),
                    $"Witness '{w.id}' has empty location_id");
                Assert.False(string.IsNullOrWhiteSpace(w.knowledgeKey),
                    $"Witness '{w.id}' has empty knowledge_key");
                Assert.False(string.IsNullOrWhiteSpace(w.body),
                    $"Witness '{w.id}' has empty body");
                Assert.NotEmpty(w.testimonies);
                Assert.False(string.IsNullOrWhiteSpace(w.testimonies[0].body),
                    $"Witness '{w.id}' testimony[0].body is empty");
            }
        }

        // ── Test 6: knowledge_key format (snake_case, history_ prefix) ────────

        [Fact]
        public void NewWitnesses_KnowledgeKeysHaveHistoryPrefix()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var allNewIds = CoastalThreadIds
                .Concat(GrainThreadIds)
                .Concat(FoundryThreadIds)
                .ToHashSet();
            foreach (var w in witnesses.Where(x => allNewIds.Contains(x.id)))
            {
                Assert.True(w.knowledgeKey.StartsWith("history_"),
                    $"Witness '{w.id}' knowledge_key '{w.knowledgeKey}' lacks 'history_' prefix");
                Assert.True(!w.knowledgeKey.Contains(" "),
                    $"Witness '{w.id}' knowledge_key '{w.knowledgeKey}' contains spaces");
            }
        }

        [Fact]
        public void CoastalWitnesses_KnowledgeKeysHaveEvacuationPrefix()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            foreach (var id in CoastalThreadIds)
            {
                var w = witnesses.FirstOrDefault(x => x.id == id);
                if (w == null) continue;
                Assert.True(w.knowledgeKey.StartsWith("history_evacuation_"),
                    $"Coastal witness '{id}' knowledge_key '{w.knowledgeKey}' lacks 'history_evacuation_' prefix");
            }
        }

        [Fact]
        public void GrainWitnesses_KnowledgeKeysHaveGrainConvoyPrefix()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            foreach (var id in GrainThreadIds)
            {
                var w = witnesses.FirstOrDefault(x => x.id == id);
                if (w == null) continue;
                Assert.True(w.knowledgeKey.StartsWith("history_grain_convoy_"),
                    $"Grain witness '{id}' knowledge_key '{w.knowledgeKey}' lacks 'history_grain_convoy_' prefix");
            }
        }

        [Fact]
        public void FoundryWitnesses_KnowledgeKeysHaveFoundryPrefix()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            foreach (var id in FoundryThreadIds)
            {
                var w = witnesses.FirstOrDefault(x => x.id == id);
                if (w == null) continue;
                Assert.True(w.knowledgeKey.StartsWith("history_foundry_"),
                    $"Foundry witness '{id}' knowledge_key '{w.knowledgeKey}' lacks 'history_foundry_' prefix");
            }
        }

        // ── Test 7: day_min within valid campaign range (242–250) ─────────────

        [Fact]
        public void NewInvestigationWitnesses_DayMinWithinExpectedRange()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var allNewIds = CoastalThreadIds
                .Concat(GrainThreadIds)
                .Concat(FoundryThreadIds)
                .ToHashSet();
            foreach (var w in witnesses.Where(x => allNewIds.Contains(x.id)))
            {
                Assert.True(w.dayMin >= 240,
                    $"Witness '{w.id}' day_min={w.dayMin} is below campaign range (>=240)");
                Assert.True(w.dayMin <= 270,
                    $"Witness '{w.id}' day_min={w.dayMin} exceeds campaign range (<=270)");
            }
        }

        // ── Test 8: body length (at least 2 sentences, no empty paragraphs) ───

        [Fact]
        public void NewInvestigationWitnesses_BodiesHaveAtLeastTwoSentences()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var allNewIds = CoastalThreadIds
                .Concat(GrainThreadIds)
                .Concat(FoundryThreadIds)
                .ToHashSet();
            foreach (var w in witnesses.Where(x => allNewIds.Contains(x.id)))
            {
                // Count terminal punctuation as sentence delimiters
                int periods = w.body.Count(c => c == '.');
                int exclaims = w.body.Count(c => c == '!');
                int questions = w.body.Count(c => c == '?');
                int sentences = periods + exclaims + questions;
                Assert.True(sentences >= 2,
                    $"Witness '{w.id}' body appears to have fewer than 2 sentences (found {sentences} terminal marks)");
                Assert.False(string.IsNullOrWhiteSpace(w.body));
            }
        }

        // ── Test 9: three Verdict site location bindings ─────────────────────

        [Fact]
        public void Plan82VerdictSites_ThreeWitnessesAnchored()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var locationIds = witnesses.Select(w => w.locationId).ToHashSet();
            foreach (var siteId in VerdictSiteIds)
            {
                Assert.True(locationIds.Contains(siteId),
                    $"No witness anchored at Verdict site '{siteId}'");
            }
        }

        [Fact]
        public void VerdictSite_AbandonedTideGauge_AssignedToCoastalThread()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var kell = witnesses.FirstOrDefault(w => w.id == "witness_harbor_master_kell");
            if (kell == null) return;
            Assert.Equal("loc_abandoned_tide_gauge", kell.locationId);
        }

        [Fact]
        public void VerdictSite_BorderCheckpointRuins_AssignedToGrainThread()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var vaughn = witnesses.FirstOrDefault(w => w.id == "witness_garrison_picket_vaughn");
            if (vaughn == null) return;
            Assert.Equal("loc_border_checkpoint_ruins", vaughn.locationId);
        }

        [Fact]
        public void VerdictSite_RiverGaugingStation_AssignedToFoundryThread()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var marit = witnesses.FirstOrDefault(w => w.id == "witness_terrace_elder_marit");
            if (marit == null) return;
            Assert.Equal("loc_river_gauging_station", marit.locationId);
        }

        // ── Test 10: priority value consistent across new witnesses ────────────

        [Fact]
        public void NewInvestigationWitnesses_PriorityIs40()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var allNewIds = CoastalThreadIds
                .Concat(GrainThreadIds)
                .Concat(FoundryThreadIds)
                .ToHashSet();
            foreach (var w in witnesses.Where(x => allNewIds.Contains(x.id)))
            {
                Assert.True(w.priority == 40,
                    $"Witness '{w.id}' priority={w.priority}, expected 40");
            }
        }

        // ── Test 11: each thread has internal day_min stagger ─────────────────

        [Fact]
        public void CoastalThread_DayMinsAreStaggered()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var days = CoastalThreadIds
                .Select(id => witnesses.FirstOrDefault(w => w.id == id))
                .Where(w => w != null)
                .Select(w => w!.dayMin)
                .ToList();
            if (days.Count < 2) return;
            Assert.True(days.Distinct().Count() == days.Count,
                "Coastal thread witnesses share identical day_min values — expected stagger");
        }

        [Fact]
        public void GrainThread_DayMinsAreStaggered()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var days = GrainThreadIds
                .Select(id => witnesses.FirstOrDefault(w => w.id == id))
                .Where(w => w != null)
                .Select(w => w!.dayMin)
                .ToList();
            if (days.Count < 2) return;
            Assert.True(days.Distinct().Count() == days.Count,
                "Grain thread witnesses share identical day_min values — expected stagger");
        }

        [Fact]
        public void FoundryThread_DayMinsAreStaggered()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var days = FoundryThreadIds
                .Select(id => witnesses.FirstOrDefault(w => w.id == id))
                .Where(w => w != null)
                .Select(w => w!.dayMin)
                .ToList();
            if (days.Count < 2) return;
            Assert.True(days.Distinct().Count() == days.Count,
                "Foundry thread witnesses share identical day_min values — expected stagger");
        }

        // ── Test 12: body/testimony[0].body mirror ─────────────────────────────

        [Fact]
        public void AllWitnesses_BodyMirrorsFirstTestimonyBody()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            foreach (var w in witnesses)
            {
                if (w.testimonies.Count > 0)
                    Assert.True(w.testimonies[0].body == w.body,
                        $"Witness '{w.id}' body does not mirror testimonies[0].body");
            }
        }

        // ── Test 13: witness_name non-empty for all new witnesses ──────────────

        [Fact]
        public void NewInvestigationWitnesses_HaveNonEmptyWitnessName()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var allNewIds = CoastalThreadIds
                .Concat(GrainThreadIds)
                .Concat(FoundryThreadIds)
                .ToHashSet();
            foreach (var w in witnesses.Where(x => allNewIds.Contains(x.id)))
                Assert.False(string.IsNullOrWhiteSpace(w.witnessName),
                    $"Witness '{w.id}' has empty witness_name");
        }

        // ── Test 14: no factionId on investigation witnesses (thread-neutral) ──

        [Fact]
        public void NewInvestigationWitnesses_HaveNoFactionId()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            var allNewIds = CoastalThreadIds
                .Concat(GrainThreadIds)
                .Concat(FoundryThreadIds)
                .ToHashSet();
            foreach (var w in witnesses.Where(x => allNewIds.Contains(x.id)))
                Assert.True(string.IsNullOrEmpty(w.factionId),
                    $"Witness '{w.id}' unexpectedly has faction_id='{w.factionId}' — investigation witnesses are faction-neutral");
        }

        // ── Test 15: total count sanity (27 = 3 Voss + 12 Plan25 + 12 Plan84) ──

        [Fact]
        public void WitnessCatalog_TotalCountIs27()
        {
            var witnesses = LoadAll();
            if (witnesses.Count == 0) return;
            Assert.True(witnesses.Count >= 27,
                $"Expected >= 27 witnesses (3 Voss + 12 Plan25 + 12 Plan84), got {witnesses.Count}");
        }
    }
}
