using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class PowerGridCatalogTests
    {
        private readonly string _catalogPath;

        public sealed class RoomRecord
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("display_name")]
            public string DisplayName { get; set; } = string.Empty;

            [JsonPropertyName("draw_watts")]
            public float DrawWatts { get; set; }

            [JsonPropertyName("default_priority")]
            public string DefaultPriority { get; set; } = string.Empty;

            [JsonPropertyName("failure_effect_id")]
            public string FailureEffectId { get; set; } = string.Empty;
        }

        public sealed class PowerGridCatalog
        {
            [JsonPropertyName("schema_version")]
            public int SchemaVersion { get; set; }

            [JsonPropertyName("generation_watts_default")]
            public float GenerationWattsDefault { get; set; }

            [JsonPropertyName("battery_capacity_wh_default")]
            public float BatteryCapacityWhDefault { get; set; }

            [JsonPropertyName("fuel_units_default")]
            public float FuelUnitsDefault { get; set; }

            [JsonPropertyName("rooms")]
            public List<RoomRecord> Rooms { get; set; } = new List<RoomRecord>();
        }

        public PowerGridCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string candidate = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "power_grid.json");
            if (File.Exists(candidate))
            {
                _catalogPath = Path.GetFullPath(candidate);
            }
            else
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "power_grid.json");
            }
        }

        private PowerGridCatalog LoadCatalog()
        {
            Assert.True(File.Exists(_catalogPath), $"Catalog file not found at: {_catalogPath}");
            string json = File.ReadAllText(_catalogPath);
            var catalog = JsonSerializer.Deserialize<PowerGridCatalog>(json);
            Assert.NotNull(catalog);
            return catalog;
        }

        [Fact]
        public void Catalog_HasValidSchemaVersion()
        {
            var catalog = LoadCatalog();
            Assert.Equal(1, catalog.SchemaVersion);
            Assert.Equal(800f, catalog.GenerationWattsDefault);
            Assert.Equal(4000f, catalog.BatteryCapacityWhDefault);
            Assert.Equal(100f, catalog.FuelUnitsDefault);
        }

        [Fact]
        public void Catalog_HasExactly18Rooms()
        {
            var catalog = LoadCatalog();
            Assert.Equal(18, catalog.Rooms.Count);
        }

        [Fact]
        public void Catalog_PreservesBaseline6Rooms()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.Rooms.Count >= 6);

            Assert.Equal("room_air_filtration", catalog.Rooms[0].Id);
            Assert.Equal("Air Filtration", catalog.Rooms[0].DisplayName);
            Assert.Equal(180f, catalog.Rooms[0].DrawWatts);
            Assert.Equal("critical", catalog.Rooms[0].DefaultPriority);
            Assert.Equal("fx_filtration_off", catalog.Rooms[0].FailureEffectId);

            Assert.Equal("room_clinic", catalog.Rooms[1].Id);
            Assert.Equal("Clinic", catalog.Rooms[1].DisplayName);
            Assert.Equal(120f, catalog.Rooms[1].DrawWatts);
            Assert.Equal("critical", catalog.Rooms[1].DefaultPriority);
            Assert.Equal("fx_clinic_off", catalog.Rooms[1].FailureEffectId);

            Assert.Equal("room_water_pump", catalog.Rooms[2].Id);
            Assert.Equal("Water Pump", catalog.Rooms[2].DisplayName);
            Assert.Equal(100f, catalog.Rooms[2].DrawWatts);
            Assert.Equal("critical", catalog.Rooms[2].DefaultPriority);
            Assert.Equal("fx_water_pressure_drop", catalog.Rooms[2].FailureEffectId);

            Assert.Equal("room_greenhouse", catalog.Rooms[3].Id);
            Assert.Equal("Greenhouse", catalog.Rooms[3].DisplayName);
            Assert.Equal(160f, catalog.Rooms[3].DrawWatts);
            Assert.Equal("standard", catalog.Rooms[3].DefaultPriority);
            Assert.Equal("fx_grow_lights_off", catalog.Rooms[3].FailureEffectId);

            Assert.Equal("room_foundry", catalog.Rooms[4].Id);
            Assert.Equal("Silent Foundry", catalog.Rooms[4].DisplayName);
            Assert.Equal(220f, catalog.Rooms[4].DrawWatts);
            Assert.Equal("low", catalog.Rooms[4].DefaultPriority);
            Assert.Equal("fx_foundry_standstill", catalog.Rooms[4].FailureEffectId);

            Assert.Equal("room_lighting_main", catalog.Rooms[5].Id);
            Assert.Equal("Main Lighting", catalog.Rooms[5].DisplayName);
            Assert.Equal(80f, catalog.Rooms[5].DrawWatts);
            Assert.Equal("low", catalog.Rooms[5].DefaultPriority);
            Assert.Equal("fx_lighting_dim", catalog.Rooms[5].FailureEffectId);
        }

        [Fact]
        public void Catalog_New12Rooms_ResolvePlan41AndCanonicalServices()
        {
            var catalog = LoadCatalog();
            Assert.Equal(18, catalog.Rooms.Count);

            var expectedNewIds = new[]
            {
                "room_workshop",
                "room_kitchen",
                "room_radio_tuner",
                "room_laboratory_research",
                "room_armory_munitions",
                "room_storage_secure",
                "room_common_mess_hall",
                "room_bunks",
                "room_water_treatment",
                "room_surveillance",
                "room_airlock",
                "room_ward_quarantine"
            };

            for (int i = 0; i < expectedNewIds.Length; i++)
            {
                Assert.Equal(expectedNewIds[i], catalog.Rooms[6 + i].Id);
            }
        }

        [Fact]
        public void Catalog_ContainsZeroDuplicateRoomIds()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var r in catalog.Rooms)
            {
                Assert.StartsWith("room_", r.Id);
                Assert.True(seen.Add(r.Id), $"Duplicate room id detected: {r.Id}");
            }
            Assert.Equal(18, seen.Count);
        }

        [Fact]
        public void Catalog_AllWattages_ArePositiveAndWithinBounds()
        {
            var catalog = LoadCatalog();
            foreach (var r in catalog.Rooms)
            {
                Assert.InRange(r.DrawWatts, 30f, 300f);
            }
        }

        [Fact]
        public void Catalog_AllPriorities_AreValid()
        {
            var catalog = LoadCatalog();
            var validPriorities = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "critical", "standard", "low" };
            foreach (var r in catalog.Rooms)
            {
                Assert.Contains(r.DefaultPriority, validPriorities);
            }
        }

        [Fact]
        public void Catalog_AllFailureEffectIds_AreNonEmptyAndPrefixed()
        {
            var catalog = LoadCatalog();
            foreach (var r in catalog.Rooms)
            {
                Assert.False(string.IsNullOrWhiteSpace(r.FailureEffectId));
                Assert.StartsWith("fx_", r.FailureEffectId);
            }
        }

        [Fact]
        public void PowerGridSystem_TotalNominalDraw_EqualsExpected2230W()
        {
            var catalog = LoadCatalog();
            var rooms = catalog.Rooms.Select(r => new PowerGridRoom(
                r.Id,
                r.DisplayName,
                r.DrawWatts,
                r.DefaultPriority switch
                {
                    "critical" => PowerGridRoomPriority.Critical,
                    "standard" => PowerGridRoomPriority.Standard,
                    "low" => PowerGridRoomPriority.Low,
                    _ => PowerGridRoomPriority.Standard
                },
                r.FailureEffectId
            )).ToList();

            var state = new PowerGridState
            {
                GenerationWatts = catalog.GenerationWattsDefault,
                FuelUnits = catalog.FuelUnitsDefault,
                BatteryCapacityWh = catalog.BatteryCapacityWhDefault,
                BatteryReserveWh = catalog.BatteryCapacityWhDefault
            };

            var grid = new PowerGridSystem(state, rooms, new SeededRng(42));
            Assert.Equal(2230f, grid.TotalDrawWatts);
            Assert.Equal(800f, grid.GenerationWatts);
            Assert.Equal(800f - 2230f, grid.NetWatts);
            Assert.False(grid.IsBrownout); // Battery has 4000 Wh reserve, so brownout is not yet active
        }

        [Fact]
        public void PowerGridSystem_CriticalCore_FitsUnderBaselineGeneration800W()
        {
            var catalog = LoadCatalog();
            var criticalRooms = catalog.Rooms.Where(r => string.Equals(r.DefaultPriority, "critical", StringComparison.OrdinalIgnoreCase)).ToList();
            Assert.Equal(6, criticalRooms.Count);

            float totalCritical = criticalRooms.Sum(r => r.DrawWatts);
            Assert.Equal(760f, totalCritical);
            Assert.True(totalCritical <= catalog.GenerationWattsDefault,
                $"Critical core ({totalCritical} W) must fit within default generation ({catalog.GenerationWattsDefault} W)");
        }

        [Fact]
        public void PowerGridSystem_LoadShedding_ByOpeningBreakersOrSettingDisabled()
        {
            var catalog = LoadCatalog();
            var rooms = catalog.Rooms.Select(r => new PowerGridRoom(
                r.Id,
                r.DisplayName,
                r.DrawWatts,
                r.DefaultPriority switch
                {
                    "critical" => PowerGridRoomPriority.Critical,
                    "standard" => PowerGridRoomPriority.Standard,
                    "low" => PowerGridRoomPriority.Low,
                    _ => PowerGridRoomPriority.Standard
                },
                r.FailureEffectId
            )).ToList();

            var state = new PowerGridState
            {
                GenerationWatts = 800f,
                FuelUnits = 100f,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = 0f // Empty battery forces brownout if draw > generation
            };

            var grid = new PowerGridSystem(state, rooms, new SeededRng(42));
            // Demand is 2230 W > 800 W with 0 battery => IsBrownout true
            Assert.True(grid.IsBrownout);

            // Open breakers for heavy non-critical loads (workshop 200W, lab 300W, foundry 220W, greenhouse 160W, etc.)
            // until draw is <= 800 W (e.g. only critical core at 760 W is left)
            foreach (var r in rooms)
            {
                if (r.DefaultPriority != PowerGridRoomPriority.Critical)
                {
                    grid.SetBreaker(r.RoomId, false); // open breaker
                }
            }

            Assert.Equal(760f, grid.TotalDrawWatts);
            Assert.False(grid.IsBrownout); // 760 W <= 800 W => Brownout cleared!
            Assert.True(grid.IsRoomPowered("room_air_filtration"));
            Assert.True(grid.IsRoomPowered("room_clinic"));
            Assert.False(grid.IsRoomPowered("room_workshop")); // Breaker is open
        }

        [Fact]
        public void PowerGridSystem_DeterministicDayTick_IdenticalSeeds()
        {
            var catalog = LoadCatalog();
            var rooms = catalog.Rooms.Select(r => new PowerGridRoom(
                r.Id,
                r.DisplayName,
                r.DrawWatts,
                PowerGridRoomPriority.Standard,
                r.FailureEffectId
            )).ToList();

            PowerGridSystem CreateGrid()
            {
                var state = new PowerGridState
                {
                    GenerationWatts = 800f,
                    FuelUnits = 100f,
                    BatteryCapacityWh = 4000f,
                    BatteryReserveWh = 2000f
                };
                return new PowerGridSystem(state, rooms, new SeededRng(77));
            }

            var gridA = CreateGrid();
            var gridB = CreateGrid();

            for (int day = 1; day <= 10; day++)
            {
                var sumA = gridA.TickDay(day, new SeededRng(day * 100));
                var sumB = gridB.TickDay(day, new SeededRng(day * 100));

                Assert.Equal(sumA.FuelConsumed, sumB.FuelConsumed, 3);
                Assert.Equal(sumA.BatteryEndWh, sumB.BatteryEndWh, 3);
                Assert.Equal(sumA.BrownoutHours, sumB.BrownoutHours, 3);
                Assert.Equal(sumA.IsBrownout, sumB.IsBrownout);
            }
        }

        [Fact]
        public void PowerGridSystem_SaveRoundTrip_PreservesAll18RoomsAndPriorities()
        {
            var catalog = LoadCatalog();
            var rooms = catalog.Rooms.Select(r => new PowerGridRoom(
                r.Id,
                r.DisplayName,
                r.DrawWatts,
                r.DefaultPriority == "critical" ? PowerGridRoomPriority.Critical : PowerGridRoomPriority.Standard,
                r.FailureEffectId
            )).ToList();

            var state = new PowerGridState
            {
                GenerationWatts = 1200f,
                FuelUnits = 85f,
                BatteryCapacityWh = 5000f,
                BatteryReserveWh = 3200f
            };

            var grid = new PowerGridSystem(state, rooms, new SeededRng(42));
            grid.SetBreaker("room_workshop", false);
            grid.SetPriority("room_foundry", PowerGridRoomPriority.Disabled);

            var save = new PowerGridSave
            {
                simDay = 15,
                Rooms = rooms.Select(PowerGridSaveCodec.FromRoom).ToList(),
                State = grid.CaptureState()
            };

            var serializer = new SystemTextJsonSerializer();
            string encoded = PowerGridSaveCodec.EncodeToString(save, serializer);
            var decoded = PowerGridSaveCodec.Decode(encoded, serializer);

            Assert.Equal(18, decoded.Rooms.Count);
            Assert.Equal(save.Checksum, decoded.Checksum);
            Assert.Equal(1200f, decoded.State.GenerationWatts);
            Assert.False(decoded.State.IsBreakerClosed("room_workshop"));
            Assert.Equal(PowerGridRoomPriority.Disabled, decoded.State.GetRoomPriority("room_foundry"));
        }

        [Fact]
        public void PowerGridSystem_Old6RoomSave_RestoresCleanlyWithNew18Rooms()
        {
            var catalog = LoadCatalog();
            var all18Rooms = catalog.Rooms.Select(r => new PowerGridRoom(
                r.Id,
                r.DisplayName,
                r.DrawWatts,
                PowerGridRoomPriority.Standard,
                r.FailureEffectId
            )).ToList();

            // Simulate an old save state created with only 6 rooms:
            var oldState = new PowerGridState
            {
                SimDay = 5,
                GenerationWatts = 800f,
                FuelUnits = 90f,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = 3500f,
                ClosedBreakers = new List<string> { "room_foundry" } // foundry breaker open
            };

            var newSystem = new PowerGridSystem(oldState, all18Rooms, new SeededRng(42));
            // Old opened breaker preserved
            Assert.False(newSystem.State.IsBreakerClosed("room_foundry"));
            // Newly added rooms default to closed (healthy) breaker
            Assert.True(newSystem.State.IsBreakerClosed("room_workshop"));
            Assert.True(newSystem.State.IsBreakerClosed("room_kitchen"));
            Assert.True(newSystem.State.IsBreakerClosed("room_laboratory_research"));
        }
    }
}
