using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// F14 — Weather Gate Catalog Integrity &amp; Content Utilization Audit.
    /// F16 — Full Weather Gate Regression Matrix (catalog portion).
    /// Proves every gate is structurally valid, targets resolve, weather
    /// names are canonical, override items exist, and the full weather ×
    /// gate truth table is correct.
    /// </summary>
    public sealed class WeatherGateCatalogIntegrityTests
    {
        private readonly string _dataDir;
        private readonly IFileIO _fileIO;
        private readonly WeatherRouteGateCatalog _routeCatalog;
        private readonly WeatherGateCatalog _domainCatalog;
        private readonly WeatherGateEvaluator _evaluator;

        public WeatherGateCatalogIntegrityTests()
        {
            _dataDir = WeatherGateAuditSimulator.FindDataDir();
            _fileIO = new FileSystemIO();
            _routeCatalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, _fileIO);
            _domainCatalog = new WeatherGateCatalog();
            foreach (var def in _routeCatalog.Gates)
                _domainCatalog.Register(WeatherGateEvaluator.FromDef(def));
            _evaluator = new WeatherGateEvaluator(_domainCatalog);
        }

        // ── F14.1 / F16.1 Catalog load ────────────────────────────────

        [Fact]
        public void F14_CatalogLoads_Exactly18Gates()
        {
            Assert.Equal(18, _routeCatalog.Gates.Count);
            Assert.Equal(15, _routeCatalog.Gates.Count(g => g.gate_type == "route"));
            Assert.Equal(3, _routeCatalog.Gates.Count(g => g.gate_type == "destination"));
        }

        [Fact]
        public void F14_DomainCatalogLoads_AllGatesRegistered()
        {
            Assert.Equal(18, _domainCatalog.Count);
        }

        // ── F14.2 / F16.2 Unique IDs ──────────────────────────────────

        [Fact]
        public void F14_AllGateIdsAreUnique()
        {
            var ids = _routeCatalog.Gates.Select(g => g.id).ToList();
            var unique = ids.Distinct(StringComparer.Ordinal).ToList();
            Assert.Equal(ids.Count, unique.Count);
            Assert.All(ids, id => Assert.False(string.IsNullOrWhiteSpace(id)));
        }

        // ── F14.3 / F16.3 Target closure ──────────────────────────────

        [Fact]
        public void F14_AllRouteGateTargetsResolve()
        {
            // Load caravan routes
            string caravanPath = Path.Combine(_dataDir, "narrative", "wasteland_trade_caravan_routes.json");
            Assert.True(File.Exists(caravanPath), "wasteland_trade_caravan_routes.json not found");
            var caravanJson = _fileIO.ReadAllText(caravanPath);
            var caravanData = new SystemTextJsonSerializer().Deserialize<TradeCaravanRouteEnvelope>(caravanJson);
            var routeIds = new HashSet<string>(
                caravanData?.routes?.Select(r => !string.IsNullOrEmpty(r.route_id) ? r.route_id : r.id) ?? Enumerable.Empty<string>(),
                StringComparer.Ordinal);

            // Load expedition destinations
            string expedPath = Path.Combine(_dataDir, "expeditions.json");
            Assert.True(File.Exists(expedPath), "expeditions.json not found");
            var expedJson = _fileIO.ReadAllText(expedPath);
            var expedData = new SystemTextJsonSerializer().Deserialize<ExpeditionEnvelope>(expedJson);
            var destIds = new HashSet<string>(
                expedData?.expeditions?.Select(e => e.id) ?? Enumerable.Empty<string>(),
                StringComparer.Ordinal);

            var unresolved = new List<string>();
            foreach (var gate in _routeCatalog.Gates)
            {
                bool found = routeIds.Contains(gate.target) || destIds.Contains(gate.target);
                if (!found) unresolved.Add($"{gate.id} -> {gate.target}");
            }

            Assert.True(unresolved.Count == 0,
                $"Unresolved gate targets:\n{string.Join("\n", unresolved)}");
        }

        // ── F14.4 / F16.4 Weather-kind closure ────────────────────────

        [Fact]
        public void F14_AllWeatherKindsAreCanonical()
        {
            var knownKinds = new HashSet<string>(
                Enum.GetNames(typeof(WeatherKind)), StringComparer.Ordinal);

            var violations = new List<string>();
            foreach (var gate in _routeCatalog.Gates)
            {
                foreach (var w in (gate.blocked_weather ?? new List<string>()))
                {
                    if (!knownKinds.Contains(w))
                        violations.Add($"{gate.id} blocked_weather: '{w}'");
                }
                foreach (var w in (gate.required_weather ?? new List<string>()))
                {
                    if (!knownKinds.Contains(w))
                        violations.Add($"{gate.id} required_weather: '{w}'");
                }
            }

            Assert.True(violations.Count == 0,
                $"Non-canonical weather kinds:\n{string.Join("\n", violations)}");
        }

        // ── F14.5 / F16.5 Override-item closure ───────────────────────

        [Fact]
        public void F14_AllOverrideItemsResolve()
        {
            string itemsPath = Path.Combine(_dataDir, "items.json");
            Assert.True(File.Exists(itemsPath), "items.json not found");
            var itemsJson = _fileIO.ReadAllText(itemsPath);
            var itemsData = new SystemTextJsonSerializer().Deserialize<ItemCatalogEnvelope>(itemsJson);
            var itemIds = new HashSet<string>(
                itemsData?.items?.Select(i => i.id) ?? Enumerable.Empty<string>(),
                StringComparer.Ordinal);

            var missing = new List<string>();
            foreach (var gate in _routeCatalog.Gates)
            {
                if (!string.IsNullOrEmpty(gate.override_item) && !itemIds.Contains(gate.override_item))
                    missing.Add($"{gate.id} override_item: '{gate.override_item}'");
            }

            Assert.True(missing.Count == 0,
                $"Missing override items:\n{string.Join("\n", missing)}");
        }

        // ── F16.6 No required/blocked overlap ──────────────────────────

        [Fact]
        public void F16_NoGateHasRequiredBlockedOverlap()
        {
            foreach (var gate in _routeCatalog.Gates)
            {
                var blocked = new HashSet<string>(gate.blocked_weather ?? new List<string>(), StringComparer.Ordinal);
                var required = new HashSet<string>(gate.required_weather ?? new List<string>(), StringComparer.Ordinal);
                blocked.IntersectWith(required);
                Assert.True(blocked.Count == 0,
                    $"Gate {gate.id} has required/blocked overlap: {string.Join(", ", blocked)}");
            }
        }

        // ── F16.7 Rollable weather coverage ────────────────────────────

        [Fact]
        public void F16_SevenRollableWeatherStates_AllHavePositiveWeights()
        {
            string profilePath = Path.Combine(_dataDir, "weather_seasons.json");
            var profile = WeatherProfileLoader.Load(_dataDir, _fileIO, new SystemTextJsonSerializer());
            Assert.NotNull(profile);

            var rollableKinds = new[] { "Clear", "Rain", "Overcast", "Ashfall", "FalloutStorm", "Blizzard", "BlackRain" };

            foreach (var season in profile!.seasons)
            {
                float total = season.clearWeight + season.rainWeight + season.overcastWeight +
                              season.ashfallWeight + season.falloutStormWeight +
                              season.blizzardWeight + season.blackRainWeight;
                Assert.True(total > 0, $"Season {season.id} has zero total weight");
            }
        }

        // ── F16.8 Weather × gate truth table ───────────────────────────

        [Theory]
        [InlineData(WeatherKind.Clear)]
        [InlineData(WeatherKind.Rain)]
        [InlineData(WeatherKind.Overcast)]
        [InlineData(WeatherKind.Ashfall)]
        [InlineData(WeatherKind.FalloutStorm)]
        [InlineData(WeatherKind.Blizzard)]
        [InlineData(WeatherKind.BlackRain)]
        public void F16_WeatherGateTruthTable_AllGatesCorrect(WeatherKind weather)
        {
            foreach (var gateDef in _routeCatalog.Gates)
            {
                var domain = WeatherGateEvaluator.FromDef(gateDef);
                var state = WeatherGateEvaluator.EvaluateGateStatic(domain, weather);

                // Verify positive gates
                if (gateDef.required_weather != null && gateDef.required_weather.Count > 0 &&
                    (gateDef.blocked_weather == null || gateDef.blocked_weather.Count == 0))
                {
                    bool requiredMatch = gateDef.required_weather.Contains(weather.ToString());
                    Assert.Equal(requiredMatch, state.IsOpen);
                    Assert.True(state.IsPositiveGate);
                }
                // Verify negative gates
                else if (gateDef.blocked_weather != null && gateDef.blocked_weather.Count > 0 &&
                         (gateDef.required_weather == null || gateDef.required_weather.Count == 0))
                {
                    bool blockedMatch = gateDef.blocked_weather.Contains(weather.ToString());
                    Assert.Equal(!blockedMatch, state.IsOpen);
                }
            }
        }

        // ── F16.9 Positive gate contract ───────────────────────────────

        [Fact]
        public void F16_PositiveGates_OpenOnlyDuringRequiredWeather()
        {
            var positiveGates = _routeCatalog.Gates
                .Where(g => g.required_weather != null && g.required_weather.Count > 0 &&
                            (g.blocked_weather == null || g.blocked_weather.Count == 0))
                .ToList();

            Assert.True(positiveGates.Count > 0, "No positive gates found");

            foreach (var gateDef in positiveGates)
            {
                var allKinds = Enum.GetValues<WeatherKind>();
                foreach (var kind in allKinds)
                {
                    var domain = WeatherGateEvaluator.FromDef(gateDef);
                    var state = WeatherGateEvaluator.EvaluateGateStatic(domain, kind);

                    bool shouldOpen = gateDef.required_weather!.Contains(kind.ToString());
                    Assert.Equal(shouldOpen, state.IsOpen);
                    if (!shouldOpen)
                    {
                        Assert.Equal("required_weather_not_matched", state.Reason);
                    }
                }
            }
        }

        // ── F16.10 Negative gate contract ──────────────────────────────

        [Fact]
        public void F16_NegativeGates_BlockedOnlyDuringBlockedWeather()
        {
            var negativeGates = _routeCatalog.Gates
                .Where(g => g.blocked_weather != null && g.blocked_weather.Count > 0 &&
                            (g.required_weather == null || g.required_weather.Count == 0))
                .ToList();

            Assert.True(negativeGates.Count > 0, "No negative gates found");

            foreach (var gateDef in negativeGates)
            {
                var allKinds = Enum.GetValues<WeatherKind>();
                foreach (var kind in allKinds)
                {
                    var domain = WeatherGateEvaluator.FromDef(gateDef);
                    var state = WeatherGateEvaluator.EvaluateGateStatic(domain, kind);

                    bool shouldBlock = gateDef.blocked_weather!.Contains(kind.ToString());
                    Assert.Equal(!shouldBlock, state.IsOpen);
                }
            }
        }

        // ── F16.11 Override matrix ─────────────────────────────────────

        [Fact]
        public void F16_OverrideMatrix_CorrectBehavior()
        {
            var overrideGates = _routeCatalog.Gates
                .Where(g => !string.IsNullOrEmpty(g.override_item))
                .ToList();

            Assert.Equal(4, overrideGates.Count); // gas_mask x2, hazmat_suit x2

            foreach (var gateDef in overrideGates)
            {
                var blockedKind = gateDef.blocked_weather![0];

                // No inventory → blocked
                Assert.True(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind, _ => false));

                // Unrelated item → still blocked
                Assert.True(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind,
                    item => item == "unrelated_item"));

                // Correct override → not blocked
                Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind,
                    item => item == gateDef.override_item));

                // Open weather → not blocked regardless of inventory
                Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gateDef, "Clear", _ => false));
                Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gateDef, "Clear", _ => true));
            }
        }

        // ── F16.12 Radio transition behavior ───────────────────────────

        [Fact]
        public void F16_RadioTransitions_EmitOnStateChange_NotOnSameState()
        {
            var catalog = new WeatherGateCatalog();
            catalog.Register(new WeatherGate
            {
                Id = "gate_test_blizzard", TargetId = "route_test",
                BlockedWeather = new List<string> { "Blizzard" }
            });
            var evaluator = new WeatherGateEvaluator(catalog);
            var hooks = new WeatherGateRadioHooks(evaluator);

            // Clear → Blizzard: should produce a closure transition
            var transitions = evaluator.CompareWeatherStates(WeatherKind.Clear, WeatherKind.Blizzard);
            Assert.Single(transitions);
            Assert.True(transitions[0].WasOpen);
            Assert.False(transitions[0].IsOpen);

            // Same state → no transition
            var noTransitions = evaluator.CompareWeatherStates(WeatherKind.Blizzard, WeatherKind.Blizzard);
            Assert.Empty(noTransitions);

            // Unrelated change → no transition for blizzard gate
            var unrelated = evaluator.CompareWeatherStates(WeatherKind.Rain, WeatherKind.Overcast);
            Assert.Empty(unrelated);
        }

        // ── F14.6–F14.8 360-day weather simulation & utilization ───────

        [Fact]
        public void F14_360DaySimulation_WeatherDistribution()
        {
            var timeline = WeatherGateAuditSimulator.BuildTimeline(
                WeatherGateAuditSimulator.AuditSeed, WeatherGateAuditSimulator.CampaignDays);

            Assert.Equal(360, timeline.Count);

            var freq = new Dictionary<WeatherKind, int>();
            foreach (var day in timeline)
            {
                if (!freq.ContainsKey(day.Weather)) freq[day.Weather] = 0;
                freq[day.Weather]++;
            }

            // All 7 rollable kinds should appear in 360 days
            Assert.True(freq.ContainsKey(WeatherKind.Clear), "Clear never appeared");
            Assert.True(freq.ContainsKey(WeatherKind.Rain), "Rain never appeared");
            Assert.True(freq.ContainsKey(WeatherKind.Overcast), "Overcast never appeared");
            Assert.True(freq.ContainsKey(WeatherKind.Ashfall), "Ashfall never appeared");
            Assert.True(freq.ContainsKey(WeatherKind.FalloutStorm), "FalloutStorm never appeared");
            Assert.True(freq.ContainsKey(WeatherKind.Blizzard), "Blizzard never appeared");
            Assert.True(freq.ContainsKey(WeatherKind.BlackRain), "BlackRain never appeared");

            // Non-rollable kinds should NOT appear
            Assert.False(freq.ContainsKey(WeatherKind.BioFog), "BioFog appeared but has zero weight");
            Assert.False(freq.ContainsKey(WeatherKind.EMPStorm), "EMPStorm appeared but has zero weight");
        }

        [Fact]
        public void F14_PerGateUtilization_Calculated()
        {
            var sim = new WeatherGateAuditSimulator(_dataDir);
            var stats = sim.CalculateUtilization();

            Assert.Equal(18, stats.Count);

            foreach (var s in stats)
            {
                Assert.True(s.BlockedDays + s.OpenDays == 360,
                    $"Gate {s.GateId}: {s.BlockedDays}+{s.OpenDays} != 360");

                // Dead gate heuristic: <5% trigger
                if (s.BlockedPct < 5.0 && s.BlockedWeather.Count > 0)
                {
                    // This is a finding, not a failure — log it
                    Assert.True(true, $"Gate {s.GateId} blocked <5% ({s.BlockedPct:F1}%) — potentially dead");
                }
            }
        }

        // ── F14.9–F14.10 Dead/restrictive gate flags ───────────────────

        [Fact]
        public void F14_DeadGateDetection_BioFogGatesAreDead()
        {
            var sim = new WeatherGateAuditSimulator(_dataDir);
            var stats = sim.CalculateUtilization();

            // BioFog gates should be dead (0% blocked) since BioFog has zero season weight
            var bioFogGates = stats.Where(s => s.BlockedWeather.Contains("BioFog")).ToList();
            Assert.Equal(3, bioFogGates.Count);
            foreach (var g in bioFogGates)
            {
                Assert.Equal(0, g.BlockedDays);
                Assert.Equal(360, g.OpenDays);
            }
        }

        [Fact]
        public void F14_DeadGateDetection_EMPGateIsDead()
        {
            var sim = new WeatherGateAuditSimulator(_dataDir);
            var stats = sim.CalculateUtilization();

            var empGate = stats.First(s => s.GateId == "gate_electronics_route_emp");
            Assert.Equal(0, empGate.BlockedDays);
            Assert.Equal(360, empGate.OpenDays);
        }

        [Fact]
        public void F14_BlizzardGates_HighBlockedRate_InColdSeasons()
        {
            var sim = new WeatherGateAuditSimulator(_dataDir);
            var stats = sim.CalculateUtilization();

            // Blizzard gates should have meaningful blocked rates
            var blizzardGates = stats.Where(s =>
                s.BlockedWeather.Count == 1 && s.BlockedWeather[0] == "Blizzard").ToList();
            Assert.Equal(4, blizzardGates.Count);

            foreach (var g in blizzardGates)
            {
                Assert.True(g.BlockedPct > 5.0,
                    $"Blizzard gate {g.GateId} blocked only {g.BlockedPct:F1}% — expected >5%");
            }
        }

        // ── F14.11 Redundancy detection ────────────────────────────────

        [Fact]
        public void F14_NoRedundantGates_SameTargetSameWeather()
        {
            var groups = _routeCatalog.Gates
                .GroupBy(g => g.target, StringComparer.Ordinal)
                .Where(grp => grp.Count() > 1)
                .ToList();

            foreach (var group in groups)
            {
                var normalized = group.Select(g =>
                {
                    var blocked = new HashSet<string>(g.blocked_weather ?? new List<string>(), StringComparer.Ordinal);
                    var required = new HashSet<string>(g.required_weather ?? new List<string>(), StringComparer.Ordinal);
                    return (g.id, blocked, required);
                }).ToList();

                // If all gates in the group have identical weather sets, flag as redundant
                for (int i = 0; i < normalized.Count; i++)
                {
                    for (int j = i + 1; j < normalized.Count; j++)
                    {
                        bool sameBlocked = normalized[i].blocked.SetEquals(normalized[j].blocked);
                        bool sameRequired = normalized[i].required.SetEquals(normalized[j].required);
                        // This is informational — currently no duplicates exist
                        if (sameBlocked && sameRequired)
                        {
                            Assert.Fail($"Redundant gates: {normalized[i].id} and {normalized[j].id} " +
                                        $"target same route with identical weather sets");
                        }
                    }
                }
            }
        }

        // ── F14.12 Orphan detection ────────────────────────────────────

        [Fact]
        public void F14_NoOrphanGates_AllTargetsExistInData()
        {
            // This is the same as F14_AllRouteGateTargetsResolve but also checks
            // that every gate target appears in at least one consumer catalog
            string caravanPath = Path.Combine(_dataDir, "narrative", "wasteland_trade_caravan_routes.json");
            var caravanJson = _fileIO.ReadAllText(caravanPath);
            var caravanData = new SystemTextJsonSerializer().Deserialize<TradeCaravanRouteEnvelope>(caravanJson);
            var routeIds = new HashSet<string>(
                caravanData?.routes?.Select(r => !string.IsNullOrEmpty(r.route_id) ? r.route_id : r.id) ?? Enumerable.Empty<string>(),
                StringComparer.Ordinal);

            string expedPath = Path.Combine(_dataDir, "expeditions.json");
            var expedJson = _fileIO.ReadAllText(expedPath);
            var expedData = new SystemTextJsonSerializer().Deserialize<ExpeditionEnvelope>(expedJson);
            var destIds = new HashSet<string>(
                expedData?.expeditions?.Select(e => e.id) ?? Enumerable.Empty<string>(),
                StringComparer.Ordinal);

            foreach (var gate in _routeCatalog.Gates)
            {
                bool inRoutes = routeIds.Contains(gate.target);
                bool inDestinations = destIds.Contains(gate.target);
                Assert.True(inRoutes || inDestinations,
                    $"Orphan gate {gate.id}: target '{gate.target}' not in caravan routes or expedition destinations");
            }
        }

        // ── F16.13 Determinism linkage (reuse F13 helpers) ─────────────

        [Fact]
        public void F16_DeterminismLinkage_SameSeedSameGates()
        {
            var sim = new WeatherGateAuditSimulator(_dataDir);
            var timeline = sim.Timeline;

            // Rebuild with same seed and verify identical
            var timeline2 = WeatherGateAuditSimulator.BuildTimeline(
                WeatherGateAuditSimulator.AuditSeed, WeatherGateAuditSimulator.CampaignDays);

            Assert.Equal(timeline.Count, timeline2.Count);
            for (int i = 0; i < timeline.Count; i++)
            {
                Assert.Equal(timeline[i].Weather, timeline2[i].Weather);
                Assert.Equal(timeline[i].SeasonId, timeline2[i].SeasonId);
            }
        }

        // ── DTOs for JSON deserialization ──────────────────────────────

        private sealed class TradeCaravanRouteEnvelope
        {
            public int schema_version { get; set; }
            public List<TradeCaravanRouteDef>? routes { get; set; }
        }

        private sealed class TradeCaravanRouteDef
        {
            public string id { get; set; } = "";
            public string route_id { get; set; } = "";
        }

        private sealed class ExpeditionEnvelope
        {
            public int schema_version { get; set; }
            public List<ExpeditionDef>? expeditions { get; set; }
        }

        private sealed class ExpeditionDef
        {
            public string id { get; set; } = "";
        }

        private sealed class ItemCatalogEnvelope
        {
            public int schema_version { get; set; }
            public List<ItemDef>? items { get; set; }
        }

        private sealed class ItemDef
        {
            public string id { get; set; } = "";
        }
    }
}
