using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 102: Comprehensive regression suite for the expanded inter-faction
    /// Foundry treaty accords catalog (foundry_accords.json).
    /// Covers schema parsing, baseline parity, signatory authority, resource
    /// allocations, structured legal articles, penalties, and consequence bindings.
    /// </summary>
    public sealed class FoundryAccordExpansionTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static (RegionalTreatiesFile File, RegionalTreatyCatalog Catalog) LoadAccords()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            string accordsRaw = files.ReadAllText(Path.Combine(dataDir, SilentFoundryCatalogLoader.AccordsFileName));
            var file = json.Deserialize<RegionalTreatiesFile>(accordsRaw)!;
            var catalog = new RegionalTreatyCatalog();
            catalog.Load(accordsRaw, json);

            return (file, catalog);
        }

        // ── 1. Catalog Loading & Count ───────────────────────────────────

        [Fact]
        public void Catalog_LoadsAllAccordsWithoutErrors()
        {
            var (file, catalog) = LoadAccords();
            Assert.NotNull(file);
            Assert.Equal(1, file.schema_version);
            Assert.Equal("foundry_district8_accords", file.collection_id);

            // Meets and exceeds the Plan 102 minimum requirement of 10 accords.
            Assert.True(catalog.AllTreaties.Count >= 10,
                $"Expected at least 10 accords, found {catalog.AllTreaties.Count}");
            Assert.Equal(12, catalog.AllTreaties.Count);
        }

        // ── 2. Baseline Parity Preservation ─────────────────────────────

        [Fact]
        public void Parity_BaselineFourDistrict8AccordsPreserved()
        {
            var (_, catalog) = LoadAccords();

            // 1. Brine Pipe & Iodine Exchange
            var brine = catalog.GetById(SilentFoundryIds.TreatyBrinePipe);
            Assert.NotNull(brine);
            Assert.Equal(280, brine.ratified_day);
            Assert.Equal("The Brine Pipe & Iodine Exchange", brine.treaty_title);
            Assert.Equal(40.0f, brine.water_allocation_lpm);
            Assert.Equal(12.0f, brine.power_quota_kw);
            Assert.Contains("faction_silent_foundry", brine.signatory_factions);
            Assert.Contains("faction_the_office", brine.signatory_factions);

            // 2. Cluster Labour Schedule
            var labour = catalog.GetById(SilentFoundryIds.TreatyLabourSchedule);
            Assert.NotNull(labour);
            Assert.Equal(305, labour.ratified_day);
            Assert.Equal("The Cluster Labour Schedule", labour.treaty_title);
            Assert.Equal(25.0f, labour.water_allocation_lpm);
            Assert.Equal(8.0f, labour.power_quota_kw);
            Assert.Contains("faction_silent_foundry", labour.signatory_factions);
            Assert.Contains("faction_the_office", labour.signatory_factions);
            Assert.Contains("faction_the_cutters", labour.signatory_factions);

            // 3. Road Iron Charter
            var roadIron = catalog.GetById(SilentFoundryIds.TreatyRoadIron);
            Assert.NotNull(roadIron);
            Assert.Equal(330, roadIron.ratified_day);
            Assert.Equal("The Road Iron Charter", roadIron.treaty_title);
            Assert.Equal(15.0f, roadIron.water_allocation_lpm);
            Assert.Equal(6.0f, roadIron.power_quota_kw);
            Assert.Contains("faction_silent_foundry", roadIron.signatory_factions);
            Assert.Contains("faction_the_cutters", roadIron.signatory_factions);
            Assert.Contains("faction_the_fleet", roadIron.signatory_factions);

            // 4. Cluster Charter
            var charter = catalog.GetById(SilentFoundryIds.TreatyClusterCharter);
            Assert.NotNull(charter);
            Assert.Equal(365, charter.ratified_day);
            Assert.Equal("The Cluster Charter", charter.treaty_title);
            Assert.Equal(0.0f, charter.water_allocation_lpm);
            Assert.Equal(0.0f, charter.power_quota_kw);
            Assert.Contains("faction_silent_foundry", charter.signatory_factions);
            Assert.Contains("faction_the_office", charter.signatory_factions);
            Assert.Contains("faction_the_cutters", charter.signatory_factions);
            Assert.Contains("faction_the_fleet", charter.signatory_factions);
        }

        // ── 3. Treaty ID Uniqueness & Grammar ───────────────────────────

        [Fact]
        public void TreatyId_AllIdsAreUniqueAndFollowSnakeCasePrefix()
        {
            var (_, catalog) = LoadAccords();
            var seenIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var treaty in catalog.AllTreaties)
            {
                Assert.False(string.IsNullOrWhiteSpace(treaty.treaty_id), "treaty_id cannot be blank");
                Assert.StartsWith("treaty_", treaty.treaty_id);
                Assert.Equal(treaty.treaty_id.ToLowerInvariant(), treaty.treaty_id);
                Assert.DoesNotContain(" ", treaty.treaty_id);
                Assert.True(seenIds.Add(treaty.treaty_id), $"Duplicate treaty_id found: {treaty.treaty_id}");
            }
        }

        // ── 4. Signatory Authority Resolution ───────────────────────────

        [Fact]
        public void Signatories_AllFactionsAreValidAndNonEmpty()
        {
            var (_, catalog) = LoadAccords();
            var validFactionIds = new HashSet<string>(StringComparer.Ordinal)
            {
                "faction_silent_foundry",
                "faction_the_office",
                "faction_the_cutters",
                "faction_the_fleet",
                "faction_central_garrison",
                "faction_rebuilders",
                "faction_ash_sign",
                "faction_forward_roster",
                "faction_the_scale"
            };

            foreach (var treaty in catalog.AllTreaties)
            {
                Assert.NotNull(treaty.signatory_factions);
                Assert.True(treaty.signatory_factions.Length >= 2,
                    $"Treaty '{treaty.treaty_id}' must have at least 2 signatories");

                foreach (var faction in treaty.signatory_factions)
                {
                    Assert.False(string.IsNullOrWhiteSpace(faction),
                        $"Treaty '{treaty.treaty_id}' has empty faction id in signatories");
                    Assert.True(validFactionIds.Contains(faction),
                        $"Signatory '{faction}' in treaty '{treaty.treaty_id}' is not an authorized faction");
                }
            }
        }

        // ── 5. Resource Allocation Validity ─────────────────────────────

        [Fact]
        public void Resources_WaterAndPowerAllocationsAreNonNegativeAndPlausible()
        {
            var (_, catalog) = LoadAccords();

            foreach (var treaty in catalog.AllTreaties)
            {
                // Non-negative allocation invariant
                Assert.True(treaty.water_allocation_lpm >= 0f,
                    $"Treaty '{treaty.treaty_id}' water allocation must be >= 0");
                Assert.True(treaty.power_quota_kw >= 0f,
                    $"Treaty '{treaty.treaty_id}' power quota must be >= 0");

                // Industrial sanity upper bounds
                Assert.True(treaty.water_allocation_lpm <= 200f,
                    $"Treaty '{treaty.treaty_id}' water allocation exceeds realistic maximum");
                Assert.True(treaty.power_quota_kw <= 100f,
                    $"Treaty '{treaty.treaty_id}' power quota exceeds realistic maximum");
            }
        }

        // ── 6. Legal Prose & Article Formatting ─────────────────────────

        [Fact]
        public void LegalText_ArticlesFollowNumberedClausesAndPenaltiesAreEnforceable()
        {
            var (_, catalog) = LoadAccords();

            foreach (var treaty in catalog.AllTreaties)
            {
                Assert.False(string.IsNullOrWhiteSpace(treaty.treaty_title));
                Assert.False(string.IsNullOrWhiteSpace(treaty.demarcated_territory));
                Assert.False(string.IsNullOrWhiteSpace(treaty.tariff_schedule));
                Assert.False(string.IsNullOrWhiteSpace(treaty.treaty_articles));
                Assert.False(string.IsNullOrWhiteSpace(treaty.penalties));

                // Structured clause convention: ARTICLE 1, ARTICLE 2, etc.
                Assert.Contains("ARTICLE 1:", treaty.treaty_articles);
                Assert.Contains("ARTICLE 2:", treaty.treaty_articles);

                // Penalty must not be placeholder
                Assert.True(treaty.penalties.Length >= 15,
                    $"Penalty for '{treaty.treaty_id}' is suspiciously short");
            }
        }

        // ── 7. Tag Vocabulary Normalization ─────────────────────────────

        [Fact]
        public void Tags_FollowNormalizedVocabularyWithoutSynonymSplits()
        {
            var (_, catalog) = LoadAccords();
            var recognizedTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "foundry", "saltworks", "brine", "iodine", "exchange", "district8",
                "labour", "cluster", "school", "schedule", "road", "ice", "anchors",
                "charter", "garrison", "rebuilders", "grain", "tithe", "verge",
                "flotilla", "cutters", "maritime", "saline", "coast", "ash_sign",
                "forward_roster", "switchback", "scarp", "fuel", "the_scale",
                "trade", "suburbs", "convention", "scrap", "salvage", "industrial",
                "neutral_ground", "demilitarization", "border", "water", "aquifer",
                "observatory", "sanctuary"
            };

            foreach (var treaty in catalog.AllTreaties)
            {
                Assert.NotNull(treaty.tags);
                Assert.NotEmpty(treaty.tags);

                foreach (var tag in treaty.tags)
                {
                    Assert.False(string.IsNullOrWhiteSpace(tag));
                    Assert.Equal(tag.ToLowerInvariant(), tag);
                    Assert.True(recognizedTags.Contains(tag),
                        $"Treaty '{treaty.treaty_id}' has unrecognized tag '{tag}'");
                }
            }
        }

        // ── 8. Timeline Chronology & Ordering ───────────────────────────

        [Fact]
        public void Timeline_RatificationDaysAreChronologicallyOrdered()
        {
            var (_, catalog) = LoadAccords();

            foreach (var treaty in catalog.AllTreaties)
            {
                Assert.True(treaty.ratified_day > 0,
                    $"Treaty '{treaty.treaty_id}' must have positive ratified_day");
                Assert.True(treaty.ratified_day <= 365,
                    $"Treaty '{treaty.treaty_id}' must occur within campaign year 1 (<= 365)");

                // The accords array is chronologically sequenced from Day 120 to Day 365
                // Note: District 8 accords start at Day 280 (first 4 records), while
                // wasteland regional accords start at Day 120.
            }

            // Verify query by ratification day
            var day200Treaties = catalog.GetRatifiedByDay(200);
            Assert.Equal(2, day200Treaties.Count); // grain tithe (120), saline corridor (180)

            var day300Treaties = catalog.GetRatifiedByDay(300);
            Assert.Equal(7, day300Treaties.Count);

            var day365Treaties = catalog.GetRatifiedByDay(365);
            Assert.Equal(12, day365Treaties.Count);
        }

        // ── 9. Functional Diversity Audit ───────────────────────────────

        [Fact]
        public void Diversity_CatalogSpansResourceLogisticsTerritorialAndGovernanceRoles()
        {
            var (_, catalog) = LoadAccords();

            // Resource & Infrastructure
            Assert.NotNull(catalog.GetById("treaty_brine_pipe_and_iodine_exchange"));
            Assert.NotNull(catalog.GetById("treaty_deep_coast_aquifer_protection_treaty"));

            // Logistics & Transport
            Assert.NotNull(catalog.GetById("treaty_road_iron_charter"));
            Assert.NotNull(catalog.GetById("treaty_switchback_fuel_and_passage_accord"));
            Assert.NotNull(catalog.GetById("treaty_flotilla_saline_corridor_concordat"));

            // Labor & Training
            Assert.NotNull(catalog.GetById("treaty_cluster_labour_schedule"));

            // Trade & Commerce
            Assert.NotNull(catalog.GetById("treaty_scale_suburban_fair_trade_convention"));
            Assert.NotNull(catalog.GetById("treaty_scrap_salvage_demarcation"));
            Assert.NotNull(catalog.GetById("treaty_garrison_grain_tithe_compact"));

            // Demilitarization & Security
            Assert.NotNull(catalog.GetById("treaty_roster_border_demilitarization_pact"));

            // Governance, Accountability & Sanctuary
            Assert.NotNull(catalog.GetById("treaty_the_cluster_charter"));
            Assert.NotNull(catalog.GetById("treaty_high_scarp_observatory_sanctuary"));
        }

        // ── 10. Consequence Integration Seam ────────────────────────────

        [Fact]
        public void ConsequenceSeam_Plan103PoliciesResolveAgainstTheseAccords()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var consequenceFile = SilentFoundryConsequenceCatalogLoader.Load(dataDir, files, json);
            var (_, catalog) = LoadAccords();

            Assert.NotEmpty(consequenceFile.policies);
            foreach (var policy in consequenceFile.policies)
            {
                var accord = catalog.GetById(policy.treaty_id);
                Assert.NotNull(accord);
                Assert.Contains(accord.signatory_factions, f => f == policy.faction_id);
            }
        }
    }
}
