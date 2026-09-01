using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class ExcavationSitesCatalogTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            return string.Empty;
        }

        [Fact]
        public void ExcavationCatalog_LoadsAllEightAuthoredSites()
        {
            string dataDir = ResolveDataDir();
            var sites = ExcavationCatalogLoader.Load(dataDir);

            Assert.NotNull(sites);
            Assert.Equal(8, sites.Count);

            var expectedIds = new[]
            {
                "excavation_command_vault",
                "excavation_utility_tunnels",
                "excavation_metro_interchange",
                "excavation_mine_shaft",
                "excavation_archive_bunker",
                "excavation_drainage_network",
                "excavation_storage_chamber",
                "excavation_civilian_shelter"
            };

            foreach (var id in expectedIds)
            {
                var site = sites.FirstOrDefault(s => s.site_id == id);
                Assert.NotNull(site);
                Assert.False(string.IsNullOrWhiteSpace(site.display_name));
                Assert.False(string.IsNullOrWhiteSpace(site.description));
                Assert.True(site.max_depth_meters > 0f);
                Assert.True(site.required_progress > 0f);
                Assert.True(site.structural_risk > 0f && site.structural_risk <= 1.0f);
            }
        }

        [Fact]
        public void ExcavationCatalog_AllSiteIdsAreUnique()
        {
            string dataDir = ResolveDataDir();
            var sites = ExcavationCatalogLoader.Load(dataDir);
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var site in sites)
            {
                Assert.True(set.Add(site.site_id), $"Duplicate site_id detected: {site.site_id}");
            }
        }

        [Fact]
        public void ExcavationCatalog_AllLocationReferencesResolveInLocationsCatalog()
        {
            string dataDir = ResolveDataDir();
            var sites = ExcavationCatalogLoader.Load(dataDir);
            string locationsPath = Path.Combine(dataDir, "locations.json");
            Assert.True(File.Exists(locationsPath), "locations.json must exist");

            string locationsJson = File.ReadAllText(locationsPath);
            using var doc = JsonDocument.Parse(locationsJson);
            var locationIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (doc.RootElement.TryGetProperty("locations", out var locArray))
            {
                foreach (var loc in locArray.EnumerateArray())
                {
                    if (loc.TryGetProperty("id", out var idProp))
                        locationIds.Add(idProp.GetString() ?? string.Empty);
                }
            }

            foreach (var site in sites)
            {
                Assert.False(string.IsNullOrWhiteSpace(site.location_id));
                Assert.True(locationIds.Contains(site.location_id),
                    $"Excavation site '{site.site_id}' references unknown location_id '{site.location_id}'");
            }
        }

        [Fact]
        public void ExcavationCatalog_DepthBandsAreOrderedAndWithinRange()
        {
            string dataDir = ResolveDataDir();
            var sites = ExcavationCatalogLoader.Load(dataDir);

            foreach (var site in sites)
            {
                Assert.NotNull(site.depth_bands);
                Assert.InRange(site.depth_bands.Count, 3, 6);

                float prevDepth = 0f;
                foreach (var band in site.depth_bands)
                {
                    Assert.False(string.IsNullOrWhiteSpace(band.label));
                    Assert.True(band.depth_meters > prevDepth,
                        $"Depth band '{band.label}' on site '{site.site_id}' is not monotonically increasing.");
                    Assert.True(band.depth_meters <= site.max_depth_meters + 0.1f,
                        $"Depth band '{band.label}' exceeds max_depth_meters on site '{site.site_id}'");
                    Assert.InRange(band.risk, 0.05f, 1.0f);
                    prevDepth = band.depth_meters;
                }
            }
        }

        [Fact]
        public void ExcavationCatalog_DepthBandsSpanShallowMediumDeepTiers()
        {
            string dataDir = ResolveDataDir();
            var sites = ExcavationCatalogLoader.Load(dataDir);

            var shallowSites = sites.Where(s => s.max_depth_meters <= 70f).ToList();
            var mediumSites = sites.Where(s => s.max_depth_meters > 70f && s.max_depth_meters <= 110f).ToList();
            var deepSites = sites.Where(s => s.max_depth_meters > 110f).ToList();

            Assert.True(shallowSites.Count >= 3, $"Expected at least 3 shallow sites, got {shallowSites.Count}");
            Assert.True(mediumSites.Count >= 2, $"Expected at least 2 medium sites, got {mediumSites.Count}");
            Assert.True(deepSites.Count >= 2, $"Expected at least 2 deep sites, got {deepSites.Count}");
        }

        [Fact]
        public void ExcavationCatalog_HazardCoverageMatchesDesignContract()
        {
            string dataDir = ResolveDataDir();
            var sites = ExcavationCatalogLoader.Load(dataDir);

            // Spore mold required on at least 2 sites (Metro Interchange, Archive Bunker)
            var moldSites = sites.Where(s => s.hazard_type == "hazard_spore_mold").ToList();
            Assert.True(moldSites.Count >= 2, $"Expected at least 2 spore mold sites, got {moldSites.Count}");
            Assert.Contains(moldSites, s => s.site_id == "excavation_metro_interchange");
            Assert.Contains(moldSites, s => s.site_id == "excavation_archive_bunker");

            // Flood hazard on Utility Tunnels and Drainage Network
            var floodSites = sites.Where(s => s.hazard_type == "hazard_flood").ToList();
            Assert.True(floodSites.Count >= 2, $"Expected at least 2 flood sites, got {floodSites.Count}");
            Assert.Contains(floodSites, s => s.site_id == "excavation_utility_tunnels");
            Assert.Contains(floodSites, s => s.site_id == "excavation_drainage_network");

            // Gas/Methane on Mine Shaft
            Assert.Contains(sites, s => s.site_id == "excavation_mine_shaft" && s.hazard_type == "hazard_methane_pocket");

            // Radiation hotspot on Command Vault
            Assert.Contains(sites, s => s.site_id == "excavation_command_vault" && s.hazard_type == "hazard_radiation_hotspot");
        }

        [Fact]
        public void ExcavationCatalog_RelicRewardLinksAreValid()
        {
            string dataDir = ResolveDataDir();
            var sites = ExcavationCatalogLoader.Load(dataDir);

            // 3 primary relic sources: Command Vault, Archive Bunker, Storage Chamber
            var relicSites = new[] { "excavation_command_vault", "excavation_archive_bunker", "excavation_storage_chamber" };
            foreach (var id in relicSites)
            {
                var site = sites.FirstOrDefault(s => s.site_id == id);
                Assert.NotNull(site);
                Assert.False(string.IsNullOrWhiteSpace(site.relic_reward_id));
            }
        }

        [Fact]
        public void ExcavationCatalog_DefaultSitesFallbackMatchesEightSites()
        {
            var defaultSites = ExcavationCatalogLoader.GetDefaultSites();
            Assert.Equal(8, defaultSites.Count);

            var siteIds = defaultSites.Select(s => s.site_id).ToHashSet();
            Assert.Contains("excavation_command_vault", siteIds);
            Assert.Contains("excavation_utility_tunnels", siteIds);
            Assert.Contains("excavation_metro_interchange", siteIds);
            Assert.Contains("excavation_mine_shaft", siteIds);
            Assert.Contains("excavation_archive_bunker", siteIds);
            Assert.Contains("excavation_drainage_network", siteIds);
            Assert.Contains("excavation_storage_chamber", siteIds);
            Assert.Contains("excavation_civilian_shelter", siteIds);
        }

        [Fact]
        public void ExcavationSystem_MultiSiteParallelExcavation_AndDeterministicProgress()
        {
            string dataDir = ResolveDataDir();
            var defs = ExcavationCatalogLoader.Load(dataDir);

            var rng1 = new SeededRng(2026);
            var sys1 = new ExcavationSystem(rng1);

            var rng2 = new SeededRng(2026);
            var sys2 = new ExcavationSystem(rng2);

            foreach (var def in defs)
            {
                sys1.AddSite(def.site_id, "room_" + def.site_id, def.required_progress, def.structuralRisk());
                sys2.AddSite(def.site_id, "room_" + def.site_id, def.required_progress, def.structuralRisk());
            }

            Assert.Equal(8, sys1.State.sites.Count);
            Assert.Equal(8, sys2.State.sites.Count);

            // Assign workers to multiple sites
            sys1.AssignWorkers("excavation_utility_tunnels", 3);
            sys1.AssignWorkers("excavation_drainage_network", 2);
            sys1.ApplyShoring("excavation_utility_tunnels");

            sys2.AssignWorkers("excavation_utility_tunnels", 3);
            sys2.AssignWorkers("excavation_drainage_network", 2);
            sys2.ApplyShoring("excavation_utility_tunnels");

            for (int day = 0; day < 5; day++)
            {
                sys1.TickDay();
                sys2.TickDay();
            }

            for (int i = 0; i < 8; i++)
            {
                Assert.Equal(sys1.State.sites[i].progress, sys2.State.sites[i].progress);
                Assert.Equal(sys1.State.sites[i].hasCavedIn, sys2.State.sites[i].hasCavedIn);
                Assert.Equal(sys1.State.sites[i].isComplete, sys2.State.sites[i].isComplete);
            }
        }

        [Fact]
        public void ExcavationSystem_SaveAndRestore_MaintainsAllEightSitesState()
        {
            string dataDir = ResolveDataDir();
            var defs = ExcavationCatalogLoader.Load(dataDir);

            var sys1 = new ExcavationSystem(new SeededRng(100));
            foreach (var def in defs)
            {
                sys1.AddSite(def.site_id, "room_" + def.site_id, def.required_progress, def.structural_risk);
            }

            sys1.AssignWorkers("excavation_command_vault", 4);
            sys1.AssignWorkers("excavation_civilian_shelter", 2);
            sys1.ApplyShoring("excavation_command_vault");
            sys1.TickDay();

            var saved = sys1.CaptureState();
            Assert.Equal(8, saved.sites.Count);

            var sys2 = new ExcavationSystem(new SeededRng(200));
            sys2.RestoreState(saved);

            Assert.Equal(8, sys2.State.sites.Count);
            var vault2 = sys2.State.sites.First(s => s.siteId == "excavation_command_vault");
            Assert.True(vault2.shoringApplied);
            Assert.Equal(4, vault2.assignedWorkerCount);
            Assert.True(vault2.progress > 0f);

            var shelter2 = sys2.State.sites.First(s => s.siteId == "excavation_civilian_shelter");
            Assert.False(shelter2.shoringApplied);
            Assert.Equal(2, shelter2.assignedWorkerCount);
            Assert.True(shelter2.progress > 0f);
        }
    }

    internal static class ExcavationSiteDefExtensions
    {
        public static float structuralRisk(this ExcavationSiteDef def) => def.structural_risk;
    }
}
