using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Foundry;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 103: Comprehensive regression suite for the expanded 15-policy
    /// treaty consequence catalog (foundry_treaty_consequences.json).
    /// Covers schema parsing, treaty/faction reference integrity, outcome
    /// vocabulary, market modifiers, idempotency, and determinism.
    /// </summary>
    public sealed class FoundryTreatyConsequenceExpansionTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static (FoundryTreatyConsequenceFile Raw, SilentFoundryConsequencePolicyCatalog Catalog, RegionalTreatiesFile Accords, GoodsCatalog Goods) LoadFixtures()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var rawConsequences = SilentFoundryConsequenceCatalogLoader.Load(dataDir, files, json);
            var catalog = new SilentFoundryConsequencePolicyCatalog();
            catalog.Load(rawConsequences);

            string accordsRaw = files.ReadAllText(Path.Combine(dataDir, SilentFoundryCatalogLoader.AccordsFileName));
            var accords = json.Deserialize<RegionalTreatiesFile>(accordsRaw)!;

            var goodsLoad = GoodsCatalogLoader.Load(dataDir, files, json);
            var goods = GoodsCatalogLoader.ToCatalog(goodsLoad);

            return (rawConsequences, catalog, accords, goods);
        }

        // ── Task 103AA & 103CL: Catalog Count & Parse Test ──────────────

        [Fact]
        public void Catalog_LoadsExactlyFifteenPoliciesWithoutErrors()
        {
            var (_, catalog, _, _) = LoadFixtures();
            Assert.False(catalog.HasErrors, "Catalog errors: " + string.Join("; ", catalog.Errors));
            Assert.Equal(15, catalog.PolicyCount);
            Assert.Equal(15, catalog.AllPolicies.Count);
        }

        // ── Task 103CM: Treaty Reference Integrity Test ─────────────────

        [Fact]
        public void ReferenceIntegrity_AllTreatyIdsResolveInFoundryAccords()
        {
            var (_, catalog, accords, _) = LoadFixtures();
            var treatyIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var t in accords.treaties)
                if (!string.IsNullOrEmpty(t.treaty_id))
                    treatyIds.Add(t.treaty_id);

            foreach (var policy in catalog.AllPolicies)
            {
                Assert.True(treatyIds.Contains(policy.treaty_id),
                    $"Treaty id '{policy.treaty_id}' in policy does not exist in foundry_accords.json");
            }
        }

        // ── Task 103CN & 103AN: Faction Reference & Consistency Test ─────

        [Fact]
        public void ReferenceIntegrity_AllFactionIdsAreSignatoriesOfReferencedTreaty()
        {
            var (_, catalog, accords, _) = LoadFixtures();
            var treatyMap = new Dictionary<string, RegionalTreatyEntry>(StringComparer.Ordinal);
            foreach (var t in accords.treaties)
                if (!string.IsNullOrEmpty(t.treaty_id))
                    treatyMap[t.treaty_id] = t;

            foreach (var policy in catalog.AllPolicies)
            {
                Assert.True(treatyMap.TryGetValue(policy.treaty_id, out var treaty),
                    $"Treaty '{policy.treaty_id}' not found");
                Assert.NotNull(treaty!.signatory_factions);
                Assert.Contains(treaty.signatory_factions, s => string.Equals(s, policy.faction_id, StringComparison.Ordinal));
            }
        }

        // ── Task 103CO: Outcome Validation Test ─────────────────────────

        [Fact]
        public void OutcomeValidation_AllPoliciesUseCanonicalOutcomeVocabulary()
        {
            var (_, catalog, _, _) = LoadFixtures();
            var canonicalOutcomes = new HashSet<string>(SilentFoundryConsequencePolicyCatalog.KnownOutcomes, StringComparer.Ordinal);

            foreach (var policy in catalog.AllPolicies)
            {
                Assert.True(canonicalOutcomes.Contains(policy.outcome),
                    $"Policy on '{policy.treaty_id}' has non-canonical outcome '{policy.outcome}'");
            }
        }

        // ── Task 103CP: Mechanical Effect Validation Test ───────────────

        [Fact]
        public void MechanicalEffectValidation_AllMarketGoodModifiersResolveInEconomyGoods()
        {
            var (_, catalog, _, goods) = LoadFixtures();
            foreach (var policy in catalog.AllPolicies)
            {
                Assert.NotNull(policy.market_modifiers);
                foreach (var modifier in policy.market_modifiers)
                {
                    Assert.False(string.IsNullOrWhiteSpace(modifier.good_id),
                        $"Policy on '{policy.treaty_id}' outcome '{policy.outcome}' has empty good_id");
                    Assert.NotNull(goods.Find(modifier.good_id));
                    Assert.NotEqual(0f, modifier.demand_delta);
                    Assert.False(string.IsNullOrWhiteSpace(modifier.reason),
                        $"Modifier for good '{modifier.good_id}' on treaty '{policy.treaty_id}' has empty reason");
                }
            }
        }

        // ── Task 103AM: Policy Key Uniqueness Audit ─────────────────────

        [Fact]
        public void PolicyUniqueness_NoDuplicateTreatyAndOutcomeKeys()
        {
            var (raw, catalog, _, _) = LoadFixtures();
            Assert.Empty(catalog.Errors);
            var seenKeys = new HashSet<string>(StringComparer.Ordinal);

            foreach (var policy in raw.policies)
            {
                string key = policy.treaty_id + "|" + policy.outcome;
                Assert.True(seenKeys.Add(key), $"Duplicate policy row found: {key}");
            }
        }

        // ── Task 103CQ & 103H: Coverage Matrix Test ─────────────────────

        [Fact]
        public void CoverageMatrix_EightTreatiesCoveredWithRationalDistribution()
        {
            var (_, catalog, _, _) = LoadFixtures();
            var outcomesByTreaty = new Dictionary<string, List<string>>(StringComparer.Ordinal);

            foreach (var policy in catalog.AllPolicies)
            {
                if (!outcomesByTreaty.TryGetValue(policy.treaty_id, out var list))
                {
                    list = new List<string>();
                    outcomesByTreaty[policy.treaty_id] = list;
                }
                list.Add(policy.outcome);
            }

            // Exactly 8 distinct treaties receive policies.
            Assert.Equal(8, outcomesByTreaty.Count);

            // 7 treaties have exactly 2 policies (met + missed/violated).
            int twoPolicyTreaties = 0;
            int onePolicyTreaties = 0;
            foreach (var kv in outcomesByTreaty)
            {
                if (kv.Value.Count == 2) twoPolicyTreaties++;
                else if (kv.Value.Count == 1) onePolicyTreaties++;
            }
            Assert.Equal(7, twoPolicyTreaties);
            Assert.Equal(1, onePolicyTreaties);

            // Verified 8 covered treaties:
            Assert.True(outcomesByTreaty.ContainsKey("treaty_brine_pipe_and_iodine_exchange"));
            Assert.True(outcomesByTreaty.ContainsKey("treaty_cluster_labour_schedule"));
            Assert.True(outcomesByTreaty.ContainsKey("treaty_road_iron_charter"));
            Assert.True(outcomesByTreaty.ContainsKey("treaty_flotilla_saline_corridor_concordat"));
            Assert.True(outcomesByTreaty.ContainsKey("treaty_switchback_fuel_and_passage_accord"));
            Assert.True(outcomesByTreaty.ContainsKey("treaty_deep_coast_aquifer_protection_treaty"));
            Assert.True(outcomesByTreaty.ContainsKey("treaty_garrison_grain_tithe_compact"));
            Assert.True(outcomesByTreaty.ContainsKey("treaty_scale_suburban_fair_trade_convention"));

            // Cluster Charter is intentionally exempt (0 policies by design).
            Assert.False(outcomesByTreaty.ContainsKey("treaty_the_cluster_charter"));
        }

        // ── Task 103CR, 103CS, 103CT: Representative Outcomes ───────────

        [Fact]
        public void RepresentativePolicy_SalineCorridorMetAndMissed()
        {
            var (_, catalog, _, _) = LoadFixtures();
            var met = catalog.Find("treaty_flotilla_saline_corridor_concordat", FoundryTreatyOutcome.Met);
            Assert.NotNull(met);
            Assert.Equal("faction_the_fleet", met.faction_id);
            Assert.Equal(3.0f, met.standing_delta);
            Assert.Contains(met.market_modifiers, m => m.good_id == "fuel" && m.demand_delta < 0f);
            Assert.Contains(met.market_modifiers, m => m.good_id == "clean_water" && m.demand_delta < 0f);

            var missed = catalog.Find("treaty_flotilla_saline_corridor_concordat", FoundryTreatyOutcome.Missed);
            Assert.NotNull(missed);
            Assert.Equal("faction_the_fleet", missed.faction_id);
            Assert.Equal(-5.0f, missed.standing_delta);
            Assert.Contains(missed.market_modifiers, m => m.good_id == "fuel" && m.demand_delta > 0f);
        }

        [Fact]
        public void RepresentativePolicy_SwitchbackMetAndViolated()
        {
            var (_, catalog, _, _) = LoadFixtures();
            var met = catalog.Find("treaty_switchback_fuel_and_passage_accord", FoundryTreatyOutcome.Met);
            Assert.NotNull(met);
            Assert.Equal("faction_ash_sign", met.faction_id);
            Assert.Equal(4.0f, met.standing_delta);
            Assert.Contains(met.market_modifiers, m => m.good_id == "fuel" && m.demand_delta < 0f);

            var violated = catalog.Find("treaty_switchback_fuel_and_passage_accord", FoundryTreatyOutcome.Violated);
            Assert.NotNull(violated);
            Assert.Equal("faction_ash_sign", violated.faction_id);
            Assert.Equal(-10.0f, violated.standing_delta);
            Assert.Contains(violated.market_modifiers, m => m.good_id == "fuel" && m.demand_delta > 0f);
        }

        [Fact]
        public void RepresentativePolicy_AquiferProtectionMetAndViolated()
        {
            var (_, catalog, _, _) = LoadFixtures();
            var met = catalog.Find("treaty_deep_coast_aquifer_protection_treaty", FoundryTreatyOutcome.Met);
            Assert.NotNull(met);
            Assert.Equal("faction_the_fleet", met.faction_id);
            Assert.Equal(3.0f, met.standing_delta);
            Assert.Contains(met.market_modifiers, m => m.good_id == "clean_water" && m.demand_delta < 0f);

            var violated = catalog.Find("treaty_deep_coast_aquifer_protection_treaty", FoundryTreatyOutcome.Violated);
            Assert.NotNull(violated);
            Assert.Equal("faction_the_fleet", violated.faction_id);
            Assert.Equal(-10.0f, violated.standing_delta);
            Assert.Contains(violated.market_modifiers, m => m.good_id == "clean_water" && m.demand_delta > 0f);
            Assert.Contains(violated.market_modifiers, m => m.good_id == "water_filter" && m.demand_delta > 0f);
        }

        [Fact]
        public void RepresentativePolicy_GrainTitheMetAndViolated()
        {
            var (_, catalog, _, _) = LoadFixtures();
            var met = catalog.Find("treaty_garrison_grain_tithe_compact", FoundryTreatyOutcome.Met);
            Assert.NotNull(met);
            Assert.Equal("faction_central_garrison", met.faction_id);
            Assert.Equal(4.0f, met.standing_delta);
            Assert.Contains(met.market_modifiers, m => m.good_id == "canned_food" && m.demand_delta < 0f);

            var violated = catalog.Find("treaty_garrison_grain_tithe_compact", FoundryTreatyOutcome.Violated);
            Assert.NotNull(violated);
            Assert.Equal("faction_central_garrison", violated.faction_id);
            Assert.Equal(-12.0f, violated.standing_delta);
            Assert.Contains(violated.market_modifiers, m => m.good_id == "canned_food" && m.demand_delta > 0f);
            Assert.Contains(violated.market_modifiers, m => m.good_id == "fuel" && m.demand_delta > 0f);
        }

        [Fact]
        public void RepresentativePolicy_FairTradeMet()
        {
            var (_, catalog, _, _) = LoadFixtures();
            var met = catalog.Find("treaty_scale_suburban_fair_trade_convention", FoundryTreatyOutcome.Met);
            Assert.NotNull(met);
            Assert.Equal("faction_the_scale", met.faction_id);
            Assert.Equal(3.0f, met.standing_delta);
            Assert.Contains(met.market_modifiers, m => m.good_id == "scrap_metal" && m.demand_delta < 0f);
        }

        // ── Task 103AR: Idempotency & Ledgers ────────────────────────────

        [Fact]
        public void Idempotency_RecordStateTracksAssessmentDayCycleKey()
        {
            var state = new SilentFoundryConsequenceState();
            Assert.False(state.IsApplied("treaty_flotilla_saline_corridor_concordat", 180));

            state.applied.Add(new FoundryConsequenceRecord
            {
                treatyId = "treaty_flotilla_saline_corridor_concordat",
                outcome = FoundryTreatyOutcome.Met,
                appliedDay = 180,
                cycleMarker = 180,
                standingDelta = 3.0f,
                reason = "Saline corridor concordat honored"
            });

            Assert.True(state.IsApplied("treaty_flotilla_saline_corridor_concordat", 180));
            Assert.False(state.IsApplied("treaty_flotilla_saline_corridor_concordat", 210));
            Assert.False(state.IsApplied("treaty_garrison_grain_tithe_compact", 180));
        }

        // ── Task 103AD & 103BW: Balance & Severity Bands ────────────────

        [Fact]
        public void Balance_StandingDeltasAreBoundedAndProportional()
        {
            var (_, catalog, _, _) = LoadFixtures();
            foreach (var p in catalog.AllPolicies)
            {
                if (p.outcome == "met")
                {
                    Assert.InRange(p.standing_delta, 1.0f, 5.0f);
                }
                else if (p.outcome == "missed")
                {
                    Assert.InRange(p.standing_delta, -7.0f, -4.0f);
                }
                else if (p.outcome == "violated")
                {
                    Assert.InRange(p.standing_delta, -16.0f, -7.0f);
                }
            }
        }
    }
}
