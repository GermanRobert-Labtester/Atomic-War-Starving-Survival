using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Narrative;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task F14: Route affinity integration tests.
    /// Verifies category overlap logic, zero-weight filtering, universal pool floor (>= 10),
    /// and canonical route coverage across expeditions.
    /// </summary>
    public class MicroLocationRouteAffinityTests
    {
        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static string MicroLocationsPath => Path.Combine(DataDir(), "micro_locations.json");
        private static string ExpeditionsPath => Path.Combine(DataDir(), "expeditions.json");

        private static JsonDocument LoadJson(string path)
        {
            Assert.True(File.Exists(path), $"File not found: {path}");
            return JsonDocument.Parse(File.ReadAllText(path));
        }

        private static HashSet<string> LoadValidExpeditionCategories()
        {
            using var doc = LoadJson(ExpeditionsPath);
            var set = new HashSet<string>(StringComparer.Ordinal);
            foreach (var exp in doc.RootElement.GetProperty("expeditions").EnumerateArray())
            {
                if (exp.TryGetProperty("lootCategories", out var cats))
                {
                    foreach (var c in cats.EnumerateArray())
                    {
                        var s = c.GetString();
                        if (!string.IsNullOrEmpty(s)) set.Add(s);
                    }
                }
            }
            return set;
        }

        [Fact]
        public void RouteAffinity_Empty_IsEligibleEverywhere()
        {
            var def = new EncounterDefinition
            {
                id = "micro_universal",
                baseWeight = 1.0f,
                routeAffinity = new List<string>()
            };

            // Empty categories
            float w1 = def.GetEffectiveWeight("Normal", 0f, "anywhere", new List<string>());
            Assert.Equal(1.0f, w1);

            // Null categories
            float w2 = def.GetEffectiveWeight("Normal", 0f, "anywhere", null);
            Assert.Equal(1.0f, w2);

            // Arbitrary categories
            float w3 = def.GetEffectiveWeight("Normal", 0f, "anywhere", new List<string> { "fuel", "cloth" });
            Assert.Equal(1.0f, w3);
        }

        [Fact]
        public void RouteAffinity_Overlap_PreservesWeight()
        {
            var def = new EncounterDefinition
            {
                id = "micro_specialized",
                baseWeight = 0.6f,
                routeAffinity = new List<string> { "fuel", "diesel_fuel" }
            };

            var routeCats = new List<string> { "canned_food", "fuel", "scrap_metal" };
            float weight = def.GetEffectiveWeight("Normal", 0f, "rural_gas_station", routeCats);

            Assert.Equal(0.6f, weight);
        }

        [Fact]
        public void RouteAffinity_NoOverlap_ReturnsZeroWeight()
        {
            var def = new EncounterDefinition
            {
                id = "micro_specialized",
                baseWeight = 0.6f,
                routeAffinity = new List<string> { "seed_packets", "growing_manual" }
            };

            var routeCats = new List<string> { "fuel", "scrap_metal" };
            float weight = def.GetEffectiveWeight("Normal", 0f, "rural_gas_station", routeCats);

            Assert.Equal(0f, weight);
        }

        [Fact]
        public void RouteAffinity_UnknownToken_FailsIntegrityValidation()
        {
            var validCategories = LoadValidExpeditionCategories();
            using var doc = LoadJson(MicroLocationsPath);

            var encounters = doc.RootElement.GetProperty("encounters");
            for (int i = 0; i < 25; i++)
            {
                var enc = encounters[i];
                string id = enc.GetProperty("id").GetString() ?? "";
                if (enc.TryGetProperty("routeAffinity", out var aff))
                {
                    foreach (var elem in aff.EnumerateArray())
                    {
                        string token = elem.GetString() ?? "";
                        Assert.True(validCategories.Contains(token),
                            $"Micro-location '{id}' references non-canonical routeAffinity token '{token}'");
                    }
                }
            }
        }

        [Fact]
        public void RouteAffinity_SameExpeditionSameSeed_ProducesSameEligiblePool()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = "micro_a",
                baseWeight = 1.0f,
                routeAffinity = new List<string> { "fuel" }
            });
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = "micro_b",
                baseWeight = 1.0f,
                routeAffinity = new List<string> { "clean_water" }
            });

            var cats = new List<string> { "fuel" };

            var pick1 = sys.SelectEncounter("Normal", 0f, "loc", new SeededRng(42), cats);
            var pick2 = sys.SelectEncounter("Normal", 0f, "loc", new SeededRng(42), cats);

            Assert.NotNull(pick1);
            Assert.NotNull(pick2);
            Assert.Equal(pick1!.id, pick2!.id);
            Assert.Equal("micro_a", pick1.id);
        }

        [Fact]
        public void RouteAffinity_AtLeastTenUniversalEntriesExist()
        {
            using var doc = LoadJson(MicroLocationsPath);
            var encounters = doc.RootElement.GetProperty("encounters");

            int universalCount = 0;
            for (int i = 0; i < 25; i++)
            {
                var enc = encounters[i];
                if (!enc.TryGetProperty("routeAffinity", out var aff) || aff.GetArrayLength() == 0)
                {
                    universalCount++;
                }
            }

            Assert.True(universalCount >= 10,
                $"Expected at least 10 universal micro-locations, found {universalCount}");
        }

        [Fact]
        public void RouteAffinity_EveryExpeditionHasMinimumEligiblePool()
        {
            using var expDoc = LoadJson(ExpeditionsPath);
            using var microDoc = LoadJson(MicroLocationsPath);

            var microList = new List<EncounterDefinition>();
            var microArray = microDoc.RootElement.GetProperty("encounters");
            for (int i = 0; i < 25; i++)
            {
                var e = microArray[i];
                var def = new EncounterDefinition
                {
                    id = e.GetProperty("id").GetString() ?? "",
                    baseWeight = (float)e.GetProperty("baseWeight").GetDouble(),
                    minDangerLevel = e.TryGetProperty("minDangerLevel", out var d) ? (float)d.GetDouble() : 0f,
                    routeAffinity = new List<string>()
                };
                if (e.TryGetProperty("routeAffinity", out var aff))
                {
                    foreach (var a in aff.EnumerateArray())
                        def.routeAffinity.Add(a.GetString() ?? "");
                }
                microList.Add(def);
            }

            foreach (var exp in expDoc.RootElement.GetProperty("expeditions").EnumerateArray())
            {
                string expId = exp.GetProperty("id").GetString() ?? "";
                var cats = new List<string>();
                if (exp.TryGetProperty("lootCategories", out var cArray))
                {
                    foreach (var c in cArray.EnumerateArray())
                        cats.Add(c.GetString() ?? "");
                }

                int eligibleCount = 0;
                foreach (var def in microList)
                {
                    if (def.GetEffectiveWeight("Normal", 10f, expId, cats) > 0f)
                    {
                        eligibleCount++;
                    }
                }

                Assert.True(eligibleCount >= 10,
                    $"Expedition '{expId}' has only {eligibleCount} eligible micro-locations (expected >= 10)");
            }
        }

        [Fact]
        public void RuinedGreenhouse_MatchesAgriculturalRoute()
        {
            var def = new EncounterDefinition
            {
                id = "micro_ruined_greenhouse",
                baseWeight = 0.5f,
                routeAffinity = new List<string> { "seed_packets", "growing_manual", "roots" }
            };

            var shedCats = new List<string> { "seed_packets", "growing_manual" };
            Assert.True(def.GetEffectiveWeight("Normal", 0f, "family_bunker_backyard_shed", shedCats) > 0f);

            var industrialCats = new List<string> { "scrap_metal", "mechanical_parts" };
            Assert.Equal(0f, def.GetEffectiveWeight("Normal", 0f, "ruined_garage", industrialCats));
        }

        [Fact]
        public void ObservationPost_MatchesConfiguredMilitaryHighGroundRoute()
        {
            var def = new EncounterDefinition
            {
                id = "micro_observation_post",
                baseWeight = 0.3f,
                minDangerLevel = 2f,
                routeAffinity = new List<string> { "dosimeter", "geiger_counter", "handheld_radio", "military_radio" }
            };

            var bunkerCats = new List<string> { "military_mre", "military_radio", "rad_away", "anti_rad" };
            Assert.True(def.GetEffectiveWeight("Normal", 2f, "government_bunker", bunkerCats) > 0f);

            var farmCats = new List<string> { "roots", "seed_packets" };
            Assert.Equal(0f, def.GetEffectiveWeight("Normal", 2f, "loc_grange_hall", farmCats));
        }

        [Fact]
        public void RailSiding_MatchesIndustrialRoute()
        {
            var def = new EncounterDefinition
            {
                id = "micro_rail_siding",
                baseWeight = 0.5f,
                routeAffinity = new List<string> { "mechanical_parts", "scrap_metal" }
            };

            var garageCats = new List<string> { "scrap_metal", "mechanical_parts", "engine" };
            Assert.True(def.GetEffectiveWeight("Normal", 1f, "ruined_garage", garageCats) > 0f);
        }

        [Fact]
        public void FrozenBus_MatchesCompatibleUrbanEvacuationRoute()
        {
            var def = new EncounterDefinition
            {
                id = "micro_frozen_bus",
                baseWeight = 0.5f,
                routeAffinity = new List<string> { "cloth", "bandage" }
            };

            var houseCats = new List<string> { "canned_food", "cloth", "battery" };
            Assert.True(def.GetEffectiveWeight("Normal", 0f, "suburban_house", houseCats) > 0f);
        }

        [Fact]
        public void WaterSource_MatchesConfiguredWaterAdjacentRoute()
        {
            var def = new EncounterDefinition
            {
                id = "micro_water_source",
                baseWeight = 0.5f,
                routeAffinity = new List<string> { "clean_water", "water_filter" }
            };

            var stationCats = new List<string> { "clean_water", "water_filter", "mechanical_parts" };
            Assert.True(def.GetEffectiveWeight("Normal", 0f, "loc_water_station", stationCats) > 0f);
        }

        [Fact]
        public void MakeshiftClinic_MatchesAbandonedHospital()
        {
            var def = new EncounterDefinition
            {
                id = "micro_makeshift_clinic",
                baseWeight = 0.4f,
                routeAffinity = new List<string> { "medical_kit", "antibiotics", "bandage", "field_surgical_kit" }
            };

            var hospitalCats = new List<string> { "antibiotics", "bandage", "field_surgical_kit", "medical_kit" };
            Assert.True(def.GetEffectiveWeight("Normal", 1f, "abandoned_hospital", hospitalCats) > 0f);
        }

        [Fact]
        public void ReconciledTaxonomy_RuralGasStation_MatchesIndustrialAndFuelAffinities()
        {
            // rural_gas_station has [fuel, scrap_metal, mechanical_parts, canned_food]
            var stationCats = new List<string> { "fuel", "scrap_metal", "mechanical_parts", "canned_food" };

            var fuelCache = new EncounterDefinition
            {
                id = "micro_fuel_cache",
                baseWeight = 0.2f,
                routeAffinity = new List<string> { "fuel", "diesel_fuel", "fuel_canister" }
            };
            Assert.True(fuelCache.GetEffectiveWeight("Normal", 1f, "rural_gas_station", stationCats) > 0f);

            var railSiding = new EncounterDefinition
            {
                id = "micro_rail_siding",
                baseWeight = 0.5f,
                routeAffinity = new List<string> { "mechanical_parts", "scrap_metal" }
            };
            Assert.True(railSiding.GetEffectiveWeight("Normal", 1f, "rural_gas_station", stationCats) > 0f);
        }

        [Fact]
        public void ReconciledTaxonomy_ConcertHallRuins_MatchesUrbanEvacuationAffinity()
        {
            // concert_hall_ruins has [cloth, scrap_wood, book, battery]
            var concertCats = new List<string> { "cloth", "scrap_wood", "book", "battery" };

            var bus = new EncounterDefinition
            {
                id = "micro_frozen_bus",
                baseWeight = 0.5f,
                routeAffinity = new List<string> { "cloth", "bandage" }
            };
            Assert.True(bus.GetEffectiveWeight("Normal", 0f, "concert_hall_ruins", concertCats) > 0f);
        }
    }
}
