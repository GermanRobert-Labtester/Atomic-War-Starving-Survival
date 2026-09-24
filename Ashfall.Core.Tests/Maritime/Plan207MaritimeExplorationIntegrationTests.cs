// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 207: Maritime & Underwater Exploration Expansion — Integration Tests
// Verifies maritime zones catalog loading, zone & site discovery, expedition
// depth/gear gating, deterministic hazard resolution, finite salvage progression,
// and state persistence roundtrip.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Maritime;

namespace Ashfall.Core.Tests.Maritime
{
    public sealed class Plan207MaritimeExplorationIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsAllMaritimeZones()
        {
            var system = new MaritimeExplorationSystem();
            string path = Path.Combine(DataDirectory, "maritime_zones.json");
            Assert.True(File.Exists(path), $"maritime_zones.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var zones = system.GetAllZoneDefs();
            Assert.True(zones.Count >= 6, $"Expected at least 6 zones, found {zones.Count}");

            var types = zones.Select(z => z.zone_type).ToHashSet(StringComparer.OrdinalIgnoreCase);
            Assert.Contains("coastal", types);
            Assert.Contains("estuary", types);
            Assert.Contains("open_ocean", types);
            Assert.Contains("deep_trench", types);
            Assert.Contains("contaminated_zone", types);
            Assert.Contains("underwater_ridge", types);

            var shallows = system.GetZoneDef("zone_coastal_shallows_north");
            Assert.NotNull(shallows);
            Assert.Equal("Northern Tidal Shallows", shallows.name);
            Assert.Equal(14.5f, shallows.water_temp_celsius);
            Assert.Equal("none", shallows.required_equipment_type);
            Assert.Contains("site_tidal_kelp_beds", shallows.dive_sites);

            var trench = system.GetZoneDef("zone_abyssal_rift_trench");
            Assert.NotNull(trench);
            Assert.Equal("deep_dive_suit", trench.required_equipment_type);
            Assert.Equal(75.0f, trench.min_depth_meters);
            Assert.Equal(220.0f, trench.max_depth_meters);
        }

        [Fact]
        public void DiscoverZone_UnlocksZoneAndAutoDiscoversContainedSites()
        {
            var system = new MaritimeExplorationSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "maritime_zones.json")));

            var site = system.RegisterDiveSite(
                siteId: "site_tidal_kelp_beds",
                siteName: "Tidal Kelp Beds",
                zoneId: "zone_coastal_shallows_north",
                type: DiveSiteType.CoastalShallows,
                depthMeters: 12.0f,
                hazardLevel: 10.0f
            );

            Assert.Equal(DiveSiteDiscoveryStatus.Undiscovered, site.Status);
            Assert.False(system.IsZoneDiscovered("zone_coastal_shallows_north"));

            DiveSiteRecord? discoveredSite = null;
            system.OnSiteDiscovered += s => discoveredSite = s;

            bool zoneDiscovered = system.DiscoverZone("zone_coastal_shallows_north");
            Assert.True(zoneDiscovered);
            Assert.True(system.IsZoneDiscovered("zone_coastal_shallows_north"));
            Assert.Equal(DiveSiteDiscoveryStatus.Discovered, site.Status);
            Assert.NotNull(discoveredSite);
            Assert.Equal("site_tidal_kelp_beds", discoveredSite.SiteId);
        }

        [Fact]
        public void ValidateExpedition_EnforcesDiverAssignmentDepthRatingAndGearGates()
        {
            var system = new MaritimeExplorationSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "maritime_zones.json")));

            // Deep trench site
            var deepSite = system.RegisterDiveSite(
                siteId: "site_deep_research_submersible",
                siteName: "Deep Research Sub",
                zoneId: "zone_abyssal_rift_trench",
                type: DiveSiteType.DeepOcean,
                depthMeters: 150.0f,
                hazardLevel: 60.0f
            );
            system.DiscoverSite(deepSite.SiteId);

            // 1. Missing divers
            bool validNoDivers = system.ValidateExpedition(deepSite.SiteId, Array.Empty<string>(), Array.Empty<string>(), out var msg1);
            Assert.False(validNoDivers);
            Assert.Contains("at least one assigned diver", msg1);

            // 2. Insufficient depth rating gear
            var shallowSuit = system.RegisterEquipment("eq_basic_01", "basic_dive_suit", depthRating: 40.0f);
            bool validShallowGear = system.ValidateExpedition(
                deepSite.SiteId,
                new[] { "diver_scout" },
                new[] { shallowSuit.EquipmentId },
                out var msg2);
            Assert.False(validShallowGear);
            Assert.Contains("insufficient for site depth", msg2);

            // 3. Proper gear matching depth and zone requirement
            var deepSuit = system.RegisterEquipment("eq_deep_01", "deep_dive_suit", depthRating: 250.0f);
            bool validDeepGear = system.ValidateExpedition(
                deepSite.SiteId,
                new[] { "diver_scout" },
                new[] { deepSuit.EquipmentId },
                out var msg3);
            Assert.True(validDeepGear, $"Expected valid gear, failed with: {msg3}");
        }

        [Fact]
        public void ExecuteExpedition_ResolvesLootAndEnforcesFiniteSalvage()
        {
            var system = new MaritimeExplorationSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "maritime_zones.json")));

            var site = system.RegisterDiveSite(
                siteId: "site_collapsed_coal_barge",
                siteName: "Collapsed Coal Barge",
                zoneId: "zone_brackish_estuary_mouth",
                type: DiveSiteType.SunkenVessel,
                depthMeters: 20.0f,
                hazardLevel: 15.0f,
                maxExplorations: 2,
                lootItemIds: new[] { "scrap_metal", "industrial_battery", "copper_pipe" }
            );
            system.DiscoverSite(site.SiteId);

            var suit = system.RegisterEquipment("eq_basic_suit", "basic_dive_suit", depthRating: 50.0f);

            List<string> deliveredLoot = new List<string>();
            system.InventoryLootDeliverer = (diver, items) => deliveredLoot.AddRange(items);

            // Dive 1
            var (planned1, _, exp1) = system.PlanExpedition(site.SiteId, new[] { "diver_bob" }, new[] { suit.EquipmentId }, day: 1);
            Assert.True(planned1);
            Assert.NotNull(exp1);

            var (executed1, _) = system.ExecuteExpedition(exp1.ExpeditionId, currentDay: 1, diverSkill: 75, seed: 100);
            Assert.True(executed1);
            Assert.Equal(MaritimeExpeditionStatus.Completed, exp1.Status);
            Assert.Equal(1, site.ExplorationCount);
            Assert.Equal(DiveSiteDiscoveryStatus.Explored, site.Status);
            Assert.NotEmpty(deliveredLoot);

            // Dive 2 (Reaching max explorations)
            var (planned2, _, exp2) = system.PlanExpedition(site.SiteId, new[] { "diver_bob" }, new[] { suit.EquipmentId }, day: 2);
            Assert.True(planned2);
            system.ExecuteExpedition(exp2!.ExpeditionId, currentDay: 2, diverSkill: 75, seed: 200);

            Assert.Equal(2, site.ExplorationCount);
            Assert.Equal(DiveSiteDiscoveryStatus.FullySalvaged, site.Status);
            Assert.True(site.IsFullySalvaged);

            // Dive 3 should be rejected as fully salvaged
            var (planned3, msg3, _) = system.PlanExpedition(site.SiteId, new[] { "diver_bob" }, new[] { suit.EquipmentId }, day: 3);
            Assert.False(planned3);
            Assert.Contains("fully salvaged", msg3);
        }

        [Fact]
        public void EvaluateHazards_TriggersHazardsAndAppliesConsequences()
        {
            var system = new MaritimeExplorationSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "maritime_zones.json")));

            // High hazard irradiated shoals
            var site = system.RegisterDiveSite(
                siteId: "site_breached_nuclear_reactor_core",
                siteName: "Reactor Discharge Pool",
                zoneId: "zone_reactor_runoff_shoals",
                type: DiveSiteType.ContaminatedZone,
                depthMeters: 30.0f,
                hazardLevel: 90.0f
            );
            system.DiscoverSite(site.SiteId);

            var suit = system.RegisterEquipment("eq_hazmat_suit", "hazmat_dive_suit", depthRating: 50.0f);

            float appliedRadiation = 0f;
            float appliedInjury = 0f;
            MaritimeHazardEvent? hazardEvent = null;

            system.RadiationApplier = (diver, rad) => appliedRadiation = rad;
            system.InjuryApplier = (diver, dmg) => appliedInjury = dmg;
            system.OnHazardEncountered += h => hazardEvent = h;

            var (planned, _, exp) = system.PlanExpedition(site.SiteId, new[] { "diver_novice" }, new[] { suit.EquipmentId }, day: 1);
            Assert.True(planned);

            // Low diver skill causes hazard with injury
            system.ExecuteExpedition(exp!.ExpeditionId, currentDay: 1, diverSkill: 20, seed: 42);

            Assert.NotNull(hazardEvent);
            Assert.Equal(MaritimeHazardType.RadiationHotspot, hazardEvent.HazardType);
            Assert.True(appliedRadiation > 0f, "Radiation should be applied from zone runoff");
            Assert.True(appliedInjury > 0f, "Injury should be applied to low skill diver");
        }

        [Fact]
        public void ExecuteExpedition_IntMinSeed_CompletesWithoutOverflow()
        {
            var system = new MaritimeExplorationSystem();
            var site = system.RegisterDiveSite(
                "site_extreme_seed",
                "Extreme Seed",
                "zone_test",
                DiveSiteType.SunkenVessel,
                depthMeters: 20f,
                hazardLevel: 10f,
                lootItemIds: new[] { "scrap_metal", "copper_pipe" });
            Assert.True(system.DiscoverSite(site.SiteId));
            var suit = system.RegisterEquipment("eq_extreme", "basic_dive_suit", depthRating: 50f);
            var (planned, reason, expedition) = system.PlanExpedition(
                site.SiteId,
                new[] { "diver_extreme" },
                new[] { suit.EquipmentId },
                day: 1);

            Assert.True(planned, reason);
            Assert.NotNull(expedition);
            var (executed, message) = system.ExecuteExpedition(
                expedition!.ExpeditionId,
                currentDay: 1,
                diverSkill: 90,
                seed: int.MinValue);

            Assert.True(executed, message);
            Assert.Equal(MaritimeExpeditionStatus.Completed, expedition.Status);
            Assert.NotEmpty(expedition.LootCollected);
        }

        [Fact]
        public void SaveRestoreState_PreservesZonesSitesExpeditionsAndEquipment()
        {
            var system1 = new MaritimeExplorationSystem();
            system1.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "maritime_zones.json")));
            system1.DiscoverZone("zone_coastal_shallows_north");

            var site = system1.RegisterDiveSite("site_test_cove", "Test Cove", "zone_coastal_shallows_north", DiveSiteType.CoastalShallows, 15f, 10f);
            system1.DiscoverSite(site.SiteId);

            var suit = system1.RegisterEquipment("eq_suit_01", "basic_dive_suit", 60f, 40f);

            var (planned, _, exp) = system1.PlanExpedition(site.SiteId, new[] { "diver_alice" }, new[] { suit.EquipmentId }, day: 5);
            system1.ExecuteExpedition(exp!.ExpeditionId, currentDay: 5, diverSkill: 80, seed: 777);

            var captured = system1.CaptureState();
            Assert.NotNull(captured);
            Assert.Single(captured.DiscoveredZoneIds);
            Assert.Single(captured.Sites);
            Assert.Single(captured.Expeditions);
            Assert.Single(captured.Equipment);

            var system2 = new MaritimeExplorationSystem();
            system2.RestoreState(captured);

            Assert.Equal(1, system2.DiscoveredZoneCount);
            Assert.True(system2.IsZoneDiscovered("zone_coastal_shallows_north"));

            var restoredSite = system2.GetSite("site_test_cove");
            Assert.NotNull(restoredSite);
            Assert.Equal(1, restoredSite.ExplorationCount);
            Assert.Equal(DiveSiteDiscoveryStatus.Explored, restoredSite.Status);

            var restoredEq = system2.GetEquipment("eq_suit_01");
            Assert.NotNull(restoredEq);
            Assert.True(restoredEq.Condition < 100f, "Equipment condition should be degraded after expedition");

            Assert.Single(system2.Expeditions);
            Assert.Equal(MaritimeExpeditionStatus.Completed, system2.Expeditions[0].Status);
        }
    }
}
