using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Codex;
using Ashfall.Core.Factions;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.Research;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    /// <summary>
    /// Plans 72–75 Flagship Integration Suite:
    /// Proves the deterministic interactions across:
    /// - Plan 72: Muster Warfare (Mobilization, supplies, doctrine, engagement, aftermath)
    /// - Plan 73: Strategic Wasteland Cartography (4-tier fog, provenance, obfuscation, route parity)
    /// - Plan 74: Codex Knowledge Surface (Pure functional projection, multi-source union, deterministic sorting)
    /// - Plan 75: Daily Briefing System (Taxonomy classification, cadence suppression, deep links, save roundtrip)
    /// </summary>
    public sealed class Plans72To75CampaignIntegrationTests
    {
        [Fact]
        public void Plan72_MusterWarfare_Mobilization_Engagement_AndAftermath()
        {
            var engine = new MusterWarfareEngine();
            var soldiers = new List<MusterSoldier>
            {
                new MusterSoldier { SurvivorId = "surv_1", DisplayName = "Sgt. Miller", Role = MusterSoldierRole.Infantry, CombatStrength = 15f, Readiness = 1f },
                new MusterSoldier { SurvivorId = "surv_2", DisplayName = "Pvt. Hayes", Role = MusterSoldierRole.Scout, CombatStrength = 10f, Readiness = 1f },
                new MusterSoldier { SurvivorId = "surv_3", DisplayName = "Doc Jones", Role = MusterSoldierRole.Medic, CombatStrength = 8f, Readiness = 1f }
            };
            var supplies = new MusterSupplyReservation
            {
                FoodRations = 30,
                CleanWaterUnits = 30,
                AmmoUnits = 50,
                MedicalUnits = 20,
                FuelUnits = 10
            };

            // Mobilization
            bool mobilized = engine.TryMobilize("branch_garrison", null, soldiers, supplies, "warlord_doctrine_toll", "sector_alpha", out string? reason);
            Assert.True(mobilized, $"Mobilization failed: {reason}");
            Assert.Equal(MusterConflictPhase.Mobilizing, engine.Phase);
            Assert.Equal(3, engine.ActiveRoster.Count);

            // Progress to Ready and Deployed
            engine.TickDay(1);
            Assert.Equal(MusterConflictPhase.Ready, engine.Phase);
            engine.TickDay(2);
            Assert.Equal(MusterConflictPhase.Deployed, engine.Phase);

            // Engage
            bool engaged = engine.TryEngage(42);
            Assert.True(engaged);
            Assert.Equal(MusterConflictPhase.Engaged, engine.Phase);

            // Resolve
            var result = engine.ResolveEngagement(42);
            Assert.NotNull(result);
            Assert.Equal(MusterConflictPhase.Aftermath, engine.Phase);
            Assert.NotNull(engine.State.LastEngagementResult);
            Assert.True(engine.Supplies.AmmoConsumed > 0, "Munitions should be consumed in combat");

            // Recovery transition
            engine.TickDay(3);
            Assert.Equal(MusterConflictPhase.Recovering, engine.Phase);
            engine.TickDay(4);
            Assert.Equal(MusterConflictPhase.Idle, engine.Phase);
        }

        [Fact]
        public void Plan73_WastelandCartography_FogLifecycle_AndRoutingConstraints()
        {
            var nodes = new List<MapNode>
            {
                new MapNode { Id = "base", DisplayName = "Holdfast", PositionX = 0f, PositionY = 0f, StartingUnlocked = true },
                new MapNode { Id = "mid", DisplayName = "Relay Ridge", PositionX = 100f, PositionY = 100f, StartingUnlocked = false },
                new MapNode { Id = "dest", DisplayName = "Radio Array", PositionX = 200f, PositionY = 200f, StartingUnlocked = false }
            };
            var routes = new List<MapRoute>
            {
                new MapRoute { From = "base", To = "mid", Distance = 10f, Danger = MapNodeDanger.Low },
                new MapRoute { From = "mid", To = "dest", Distance = 10f, Danger = MapNodeDanger.Medium }
            };
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);

            // 1. Unknown initially
            Assert.Equal(MapFogState.Unknown, map.GetFogState("mid"));
            var unknownIntel = map.GetNodeIntel("mid");
            Assert.NotNull(unknownIntel);
            Assert.False(unknownIntel!.Routable);

            // 2. Discover Rumor
            bool rumored = map.DiscoverRumor("mid", "intercept_01", day: 1, InformationConfidence.Low);
            Assert.True(rumored);
            Assert.Equal(MapFogState.Rumored, map.GetFogState("mid"));

            var rumoredIntel = map.GetNodeIntel("mid");
            Assert.NotNull(rumoredIntel);
            Assert.False(rumoredIntel!.Routable);
            // Coords must be obfuscated / non-exact
            Assert.True(Math.Abs(rumoredIntel.PositionX - 100f) > 0.01f || Math.Abs(rumoredIntel.PositionY - 100f) > 0.01f);

            // Route planning through rumored intermediate node must fail
            var blockedRoute = map.PlanRoute("base", "dest");
            Assert.Empty(blockedRoute);

            // 3. Survey Node
            bool surveyed = map.DiscoverSurvey("mid", "survey_unit_alpha", day: 2, new[] { "radio_mast", "elevated" });
            Assert.True(surveyed);
            Assert.Equal(MapFogState.Surveyed, map.GetFogState("mid"));

            var surveyedIntel = map.GetNodeIntel("mid");
            Assert.NotNull(surveyedIntel);
            Assert.True(surveyedIntel!.Routable);
            Assert.Equal(100f, surveyedIntel.PositionX);
            Assert.Equal(100f, surveyedIntel.PositionY);
            Assert.Contains("radio_mast", surveyedIntel.Traits);

            // Route planning now succeeds through surveyed node once unlocked
            map.Unlock("mid");
            map.Unlock("dest");
            map.DiscoverVisited("dest", "expedition_1", 3);
            var validRoute = map.PlanRoute("base", "dest");
            Assert.Equal(3, validRoute.Count);
            Assert.Equal("base", validRoute[0]);
            Assert.Equal("mid", validRoute[1]);
            Assert.Equal("dest", validRoute[2]);

            // 4. Visit Node
            map.DiscoverVisited("mid", "expedition_1", 4);
            Assert.Equal(MapFogState.Visited, map.GetFogState("mid"));
        }

        [Fact]
        public void Plan74_CodexProjection_MultiSourceFactAggregation_AndOrdering()
        {
            var fieldGuideItems = new List<CodexFactSourceItem>
            {
                new CodexFactSourceItem
                {
                    Id = "item_rad_filter",
                    Title = "Rad-X Filter Membrane",
                    Category = CodexCategory.Technology,
                    Description = "High-density particulate filter for intake scrubbers.",
                    Provenance = new CampaignProvenanceRecord(KnowledgeSourceKind.FieldGuide, "fg_entry_01", "field_guide", 1, InformationConfidence.Authoritative, "item_rad_filter"),
                    RelatedTopicIds = new List<string> { "scrubber", "rad_shield" }
                }
            };

            var researchItems = new List<CodexFactSourceItem>
            {
                new CodexFactSourceItem
                {
                    Id = "tech_filtration",
                    Title = "Advanced Electrostatic Filtration",
                    Category = CodexCategory.Technology,
                    Description = "Ionized plate scrubbing reduces fallout ingress by 60%.",
                    Provenance = new CampaignProvenanceRecord(KnowledgeSourceKind.ResearchArchive, "res_node_02", "research_archive", 5, InformationConfidence.Authoritative, "tech_filtration"),
                    RelatedTopicIds = new List<string> { "scrubber" }
                }
            };

            var journalItems = new List<CodexFactSourceItem>
            {
                new CodexFactSourceItem
                {
                    Id = "item_rad_filter", // Duplicate ID to test fact deduplication and unioned provenance
                    Title = "Rad-X Filter Membrane (Recovered Field Notes)",
                    Category = CodexCategory.Technology,
                    Description = "Scavenged from the ruins of Depot 9.",
                    Provenance = new CampaignProvenanceRecord(KnowledgeSourceKind.PhysicalRecovery, "journal_ev_88", "journal", 6, InformationConfidence.Verified, "item_rad_filter"),
                    RelatedTopicIds = new List<string> { "depot_9" }
                }
            };

            var entries = CodexProjectionBuilder.BuildProjection(fieldGuideItems, researchItems, journalItems);
            Assert.Equal(2, entries.Count);

            var radFilter = entries.First(e => e.Id == "item_rad_filter");
            Assert.Equal(2, radFilter.ProvenanceHistory.Count);
            Assert.Equal(InformationConfidence.Authoritative, radFilter.Confidence);
            Assert.Contains("scrubber", radFilter.RelatedTopicIds);
            Assert.Contains("depot_9", radFilter.RelatedTopicIds);

            // Deterministic ordinal order
            Assert.True(string.CompareOrdinal(entries[0].Title, entries[1].Title) <= 0);
        }

        [Fact]
        public void Plan75_DailyBriefing_CadenceSuppression_AndResolution()
        {
            var filter = new DailyBriefingCadenceFilter();

            var warningFact = new BriefingFact
            {
                FactId = "warn_ammo_low",
                Class = BriefingTaxonomyClass.WARNING,
                Title = "Munitions Depleted",
                Summary = "Shelter reserve below 10 rounds.",
                SourceSystem = "inventory",
                Confidence = InformationConfidence.Authoritative,
                DeepLinkRoute = "ui_inventory"
            };

            var criticalFact = new BriefingFact
            {
                FactId = "crit_rad_breach",
                Class = BriefingTaxonomyClass.CRITICAL,
                Title = "Radiation Spike",
                Summary = "Sublevel B exposure exceeding 50 mSv.",
                SourceSystem = "radiation",
                Confidence = InformationConfidence.Authoritative,
                DeepLinkRoute = "ui_rad_scrubber"
            };

            // Day 1: Both facts are emitted
            var day1Filtered = filter.FilterDailyFacts(new[] { warningFact, criticalFact }, day: 1);
            Assert.Equal(2, day1Filtered.Count);

            // Day 2: Same warning condition unchanged -> suppressed!
            // Critical warning unchanged -> suppressed because < 3 days cadence
            var day2Filtered = filter.FilterDailyFacts(new[] { warningFact, criticalFact }, day: 2);
            Assert.Empty(day2Filtered);

            // Day 4: Critical warning hits 3-day recurrence interval -> re-emits!
            var day4Filtered = filter.FilterDailyFacts(new[] { warningFact, criticalFact }, day: 4);
            Assert.Single(day4Filtered);
            Assert.Equal("crit_rad_breach", day4Filtered[0].FactId);

            // Day 5: Critical warning resolves (no longer in active facts) -> resolution emitted!
            var day5Filtered = filter.FilterDailyFacts(new[] { warningFact }, day: 5);
            Assert.Single(day5Filtered);
            Assert.Equal("RESOLVED: Radiation Spike", day5Filtered[0].Title);
            Assert.Equal(BriefingTaxonomyClass.INTEL, day5Filtered[0].Class);
        }

        [Fact]
        public void IntegratedSevenDayCampaignPlaythrough_AndSaveRoundtrip()
        {
            // Set up 4-tier map
            var nodes = new List<MapNode>
            {
                new MapNode { Id = "loc_base", DisplayName = "Sanctuary", PositionX = 10f, PositionY = 10f, StartingUnlocked = true },
                new MapNode { Id = "loc_depot", DisplayName = "Depot 12", PositionX = 50f, PositionY = 50f, StartingUnlocked = false },
                new MapNode { Id = "loc_relay", DisplayName = "Radio Array", PositionX = 120f, PositionY = 120f, StartingUnlocked = false }
            };
            var routes = new List<MapRoute>
            {
                new MapRoute { From = "loc_base", To = "loc_depot", Distance = 15f, Danger = MapNodeDanger.Low },
                new MapRoute { From = "loc_depot", To = "loc_relay", Distance = 20f, Danger = MapNodeDanger.High }
            };
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var briefingFilter = new DailyBriefingCadenceFilter();
            var briefingState = new DailyBriefingState();
            var warfareEngine = new MusterWarfareEngine();

            // --- DAY 1 ---
            // Intercept rumor of loc_depot
            map.DiscoverRumor("loc_depot", "intercept_d1", day: 1, InformationConfidence.Medium);
            var d1Intel = map.GetNodeIntel("loc_depot");
            Assert.Equal(MapFogState.Rumored, d1Intel!.FogState);

            var d1Facts = new List<BriefingFact>
            {
                new BriefingFact
                {
                    FactId = "fact_rumor_depot",
                    Class = BriefingTaxonomyClass.INTEL,
                    Title = "Radio Intercept: Depot Rumor",
                    Summary = "Signal picked up mentioning supply depot at approximate coordinates.",
                    SourceSystem = "radio",
                    Confidence = InformationConfidence.Medium,
                    DeepLinkRoute = "ui_wasteland_map"
                },
                new BriefingFact
                {
                    FactId = "fact_warn_water",
                    Class = BriefingTaxonomyClass.WARNING,
                    Title = "Water Reserves Dwindling",
                    Summary = "Clean water reserve below 20L.",
                    SourceSystem = "water",
                    Confidence = InformationConfidence.Authoritative,
                    DeepLinkRoute = "ui_water_treatment"
                }
            };
            var d1Filtered = briefingFilter.FilterDailyFacts(d1Facts, day: 1);
            Assert.Equal(2, d1Filtered.Count);

            var report1 = DailyBriefingReportBuilder.BuildFromBriefingFacts(1, d1Filtered);
            Assert.Equal(2, report1.Entries.Count);
            Assert.Contains(report1.Entries, e => e.DeepLinkRoute == "ui_wasteland_map");

            // --- DAY 2 ---
            // Water warning persists unchanged
            var d2Facts = new List<BriefingFact>
            {
                new BriefingFact
                {
                    FactId = "fact_warn_water",
                    Class = BriefingTaxonomyClass.WARNING,
                    Title = "Water Reserves Dwindling",
                    Summary = "Clean water reserve below 20L.",
                    SourceSystem = "water",
                    Confidence = InformationConfidence.Authoritative,
                    DeepLinkRoute = "ui_water_treatment"
                }
            };
            var d2Filtered = briefingFilter.FilterDailyFacts(d2Facts, day: 2);
            Assert.Empty(d2Filtered); // Suppressed!

            // --- DAY 3 ---
            // Survey loc_depot -> upgrades to Surveyed
            map.DiscoverSurvey("loc_depot", "surveyor_squad", day: 3, new[] { "dry_storage", "secure_perimeter" });
            Assert.Equal(MapFogState.Surveyed, map.GetFogState("loc_depot"));

            // --- DAY 4 ---
            // Muster warfare patrol mobilized to secure the route to depot
            var soldiers = new List<MusterSoldier>
            {
                new MusterSoldier { SurvivorId = "surv_vance", DisplayName = "Capt. Vance", Role = MusterSoldierRole.Infantry, CombatStrength = 20f }
            };
            var supplies = new MusterSupplyReservation { AmmoUnits = 30, FoodRations = 10, CleanWaterUnits = 10 };
            bool mobilized = warfareEngine.TryMobilize("branch_scout", null, soldiers, supplies, "warlord_doctrine_procedure", "loc_depot", out _);
            Assert.True(mobilized);

            // --- DAY 5 ---
            // Battle resolution
            warfareEngine.TickDay(4);
            warfareEngine.TickDay(5);
            warfareEngine.TryEngage(1001);
            var engResult = warfareEngine.ResolveEngagement(1001);
            Assert.NotNull(engResult);

            // --- DAY 6 ---
            // Expedition visits loc_depot -> upgraded to Visited
            map.DiscoverVisited("loc_depot", "patrol_vance", day: 6);
            Assert.Equal(MapFogState.Visited, map.GetFogState("loc_depot"));

            // --- DAY 7 ---
            // Codex compiles knowledge projection
            var codexItems = new List<CodexFactSourceItem>
            {
                new CodexFactSourceItem
                {
                    Id = "loc_depot",
                    Title = "Depot 12",
                    Category = CodexCategory.Locations,
                    Description = "Secured military storage annex.",
                    Provenance = map.GetNodeKnowledge("loc_depot")?.Provenance
                }
            };
            var codexEntries = CodexProjectionBuilder.BuildProjection(codexItems, Array.Empty<CodexFactSourceItem>(), Array.Empty<CodexFactSourceItem>());
            Assert.Single(codexEntries);
            Assert.Equal("Depot 12", codexEntries[0].Title);

            // --- SAVE ROUND-TRIP VERIFICATION ---
            // 1. Capture map state
            var mapState = map.CaptureState();
            Assert.NotNull(mapState.Knowledge);
            Assert.Contains(mapState.Knowledge, k => k.NodeId == "loc_depot" && k.FogState == MapFogState.Visited);

            // 2. Capture briefing state
            briefingState.CadenceRecords = briefingFilter.CaptureRecords();
            var briefingCaptured = briefingState.CaptureState();

            // 3. Serialize and restore map
            var json = new SystemTextJsonSerializer();
            string mapJson = json.Serialize(mapState);
            var restoredMapState = json.Deserialize<WastelandMapState>(mapJson);
            Assert.NotNull(restoredMapState);
            var restoredMap = new WastelandMapSystem(restoredMapState!, nodes, routes);
            Assert.Equal(MapFogState.Visited, restoredMap.GetFogState("loc_depot"));
            Assert.Equal(MapFogState.Unknown, restoredMap.GetFogState("loc_relay"));

            // 4. Serialize and restore briefing cadence
            string briefingJson = json.Serialize(briefingCaptured);
            var restoredBriefingCaptured = json.Deserialize<DailyBriefingState>(briefingJson);
            Assert.NotNull(restoredBriefingCaptured);
            var restoredFilter = new DailyBriefingCadenceFilter();
            restoredFilter.RestoreRecords(restoredBriefingCaptured!.CadenceRecords);

            // Verify restored filter continues suppression without loss
            var d8Filtered = restoredFilter.FilterDailyFacts(d2Facts, day: 8);
            Assert.Empty(d8Filtered); // Water warning still suppressed because it was recorded on Day 1
        }
    }
}
