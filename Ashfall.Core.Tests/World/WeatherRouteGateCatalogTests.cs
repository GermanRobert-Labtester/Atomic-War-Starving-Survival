using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// GAP-48A / GAP-49B — destination-level weather gates and micro-location
    /// bindings (Plan 48 + Plan 76 §35/§37 deferred seams).
    ///
    /// Seals: weather_route_gates.json gains a loader + pure evaluation and
    /// destination-targeted gates; micro_locations.json loads through the
    /// existing NarrativeEncounterCatalogLoader so `requiredLocationId`
    /// bindings become reachable runtime behavior.
    /// </summary>
    public sealed class WeatherRouteGateCatalogTests : IDisposable
    {
        private readonly string _dataDir;
        private readonly IFileIO _fileIO;

        public WeatherRouteGateCatalogTests()
        {
            _dataDir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            }

            _fileIO = new FileSystemIO();
        }

        public void Dispose()
        {
        }

        [Fact]
        public void Loader_ParsesFullCatalog_IncludingDestinationGates()
        {
            var catalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, _fileIO);
            Assert.Equal(18, catalog.Gates.Count); // 15 original route gates + 3 destination gates
            Assert.Equal(15, catalog.Gates.Count(g => g.gate_type == "route"));
            Assert.Equal(3, catalog.Gates.Count(g => g.gate_type == "destination"));
        }

        [Fact]
        public void DestinationGates_ResolveByTargetId()
        {
            var catalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, _fileIO);
            foreach (var target in new[]
                     {
                         "location_silent_observatory",
                         "location_flooded_subway_depot",
                         "loc_the_shallows_market"
                     })
            {
                Assert.True(catalog.TryGetGatesForTarget(target, out var gates),
                    $"no gates resolved for {target}");
                Assert.Single(gates);
            }

            Assert.False(catalog.TryGetGatesForTarget("loc_the_allotments", out _));
        }

        [Fact]
        public void Evaluation_BlockedWeatherHits_Misses_AndOverrideLifts()
        {
            var blizzardGate = new WeatherGateDef
            {
                id = "g1", gate_type = "destination", target = "loc_x",
                blocked_weather = new List<string> { "Blizzard", "IceStorm" }
            };
            // hit
            Assert.True(WeatherRouteGateCatalog.IsGateBlocking(blizzardGate, "Blizzard", null));
            Assert.True(WeatherRouteGateCatalog.IsGateBlocking(blizzardGate, "IceStorm", null));
            // miss (case-insensitive match, unrelated weather passes)
            Assert.True(WeatherRouteGateCatalog.IsGateBlocking(blizzardGate, "blizzard", null));
            Assert.False(WeatherRouteGateCatalog.IsGateBlocking(blizzardGate, "Clear", null));
            // no override configured → predicate is never consulted, block stands
            Assert.True(WeatherRouteGateCatalog.IsGateBlocking(blizzardGate, "Blizzard", item => item == "gas_mask"));
        }

        [Fact]
        public void Evaluation_OverrideItem_LiftsTheBlock_WhenConfigured()
        {
            var gated = new WeatherGateDef
            {
                id = "g3", gate_type = "destination", target = "loc_x",
                blocked_weather = new List<string> { "Blizzard" },
                override_item = "gas_mask"
            };
            Assert.True(WeatherRouteGateCatalog.IsGateBlocking(gated, "Blizzard", item => item != "gas_mask"));
            Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gated, "Blizzard", item => item == "gas_mask"));
        }

        [Fact]
        public void Evaluation_RequiredWeather_BlocksWhenAbsent()
        {
            var coldGate = new WeatherGateDef
            {
                id = "g2", gate_type = "route", target = "route_x",
                required_weather = new List<string> { "Blizzard", "IceStorm" }
            };
            // required weather present → passable
            Assert.False(WeatherRouteGateCatalog.IsGateBlocking(coldGate, "Blizzard", null));
            // required weather absent → blocked
            Assert.True(WeatherRouteGateCatalog.IsGateBlocking(coldGate, "Clear", null));
        }

        [Fact]
        public void EvaluateBlock_ReturnsDescriptionAsReason_AndNullWhenPassable()
        {
            var catalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, _fileIO);

            var block = catalog.EvaluateBlock("location_silent_observatory", "Blizzard", null);
            Assert.NotNull(block);
            Assert.Equal("gate_dest_silent_observatory_blizzard", block!.GateId);
            Assert.Contains("blizzard", block.ShortReason);

            Assert.Null(catalog.EvaluateBlock("location_silent_observatory", "Clear", null));
            Assert.Null(catalog.EvaluateBlock("loc_the_allotments", "Blizzard", null));
        }

        [Fact]
        public void Evaluation_IsPure_SameInputsSameResult()
        {
            var catalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, _fileIO);
            var a = catalog.EvaluateBlock("location_silent_observatory", "Blizzard", null);
            var b = catalog.EvaluateBlock("location_silent_observatory", "Blizzard", null);
            Assert.Equal(a!.GateId, b!.GateId);
            Assert.Equal(a.Reason, b.Reason);
            Assert.Equal(a.ShortReason, b.ShortReason);
        }

        [Fact]
        public void MicroLocations_LoadThroughNarrativeLoader_WithDestinationBindings()
        {
            var serializer = new SystemTextJsonSerializer();
            var loaded = NarrativeEncounterCatalogLoader.Load(_dataDir, _fileIO, serializer);

            foreach (var target in new[]
                     {
                         "abandoned_hospital",
                         "location_flooded_subway_depot",
                         "loc_garrison_checkpoint_gamma"
                     })
            {
                var bound = loaded.Where(e => e.requiredLocationId == target).ToList();
                Assert.True(bound.Count >= 1, $"expected micro-locations bound to {target}, found none");
            }

            // the three GAP-49B micro-location entries are present and loadable
            foreach (var id in new[]
                     {
                         "micro_hospital_chapel_ledger",
                         "micro_depot_undertow_raft_line",
                         "micro_gamma_levy_board"
                     })
            {
                var enc = loaded.Single(e => e.id == id);
                Assert.False(string.IsNullOrEmpty(enc.requiredLocationId));
            }

            var hospital = loaded.Single(e => e.id == "micro_hospital_chapel_ledger");
            // location-bound weighting: zero off-destination, positive on-destination
            Assert.Equal(0f, hospital.GetEffectiveWeight("Stealth", 6, "loc_the_allotments"));
            Assert.True(hospital.GetEffectiveWeight("Stealth", 6, "abandoned_hospital") > 0f);
            // choices resolve through the existing schema (grant items valid ids are
            // covered by the data-integrity selftest)
            Assert.NotEmpty(hospital.choices);
        }
    }
}
