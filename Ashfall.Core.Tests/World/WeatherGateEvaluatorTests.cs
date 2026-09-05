using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Gate evaluator semantics (F9/F10/F12). Pure — no RNG, no clock.
    /// </summary>
    public class WeatherGateEvaluatorTests
    {
        private static WeatherGate Gate(
            string id,
            string target,
            string[]? blocked = null,
            string[]? required = null)
        {
            return new WeatherGate
            {
                Id = id,
                TargetId = target,
                BlockedWeather = blocked?.ToList() ?? new List<string>(),
                RequiredWeather = required?.ToList() ?? new List<string>()
            };
        }

        private static WeatherGateCatalog Catalog(params WeatherGate[] gates)
        {
            var catalog = new WeatherGateCatalog();
            foreach (var g in gates) catalog.Register(g);
            return catalog;
        }

        // ── Negative gate: blocked_weather ────────────────────────────

        [Fact]
        public void NegativeGate_BlockedWeatherMatch_IsBlocked()
        {
            var evaluator = new WeatherGateEvaluator(Catalog(
                Gate("gate_mountain_pass_blizzard", "route_12", blocked: new[] { "Blizzard" })));

            var state = evaluator.EvaluateWeatherOnly("gate_mountain_pass_blizzard", WeatherKind.Blizzard);

            Assert.False(state.IsOpen);
            Assert.Equal(WeatherGatePolarity.NegativeBlockedDuring, state.Polarity);
            Assert.Contains("impassable", state.Reason);
        }

        [Fact]
        public void NegativeGate_BlockedWeatherNonMatch_IsOpen()
        {
            var evaluator = new WeatherGateEvaluator(Catalog(
                Gate("gate_mountain_pass_blizzard", "route_12", blocked: new[] { "Blizzard" })));

            var state = evaluator.EvaluateWeatherOnly("gate_mountain_pass_blizzard", WeatherKind.Clear);

            Assert.True(state.IsOpen);
        }

        [Fact]
        public void NegativeGate_UnknownGateId_ReturnsUnknownGate()
        {
            var evaluator = new WeatherGateEvaluator(Catalog());

            var state = evaluator.EvaluateWeatherOnly("gate_missing", WeatherKind.Blizzard);

            Assert.False(state.IsOpen);
            Assert.Equal("unknown_gate", state.Reason);
            Assert.Equal(string.Empty, state.TargetId);
        }

        // ── Positive gate: required_weather ──────────────────────────

        [Fact]
        public void PositiveGate_RequiredWeatherMatch_IsOpen()
        {
            var evaluator = new WeatherGateEvaluator(Catalog(
                Gate("gate_frozen_lake_crossing", "route_06", required: new[] { "Blizzard" })));

            var state = evaluator.EvaluateWeatherOnly("gate_frozen_lake_crossing", WeatherKind.Blizzard);

            Assert.True(state.IsOpen);
            Assert.True(state.IsPositiveGate);
            Assert.Equal(WeatherGatePolarity.PositiveRequiredWeather, state.Polarity);
            Assert.Contains("traversable", state.Description);
        }

        [Fact]
        public void PositiveGate_RequiredWeatherNonMatch_IsClosedNotYetAvailable()
        {
            var evaluator = new WeatherGateEvaluator(Catalog(
                Gate("gate_frozen_lake_crossing", "route_06", required: new[] { "Blizzard" })));

            var state = evaluator.EvaluateWeatherOnly("gate_frozen_lake_crossing", WeatherKind.Clear);

            Assert.False(state.IsOpen);
            Assert.True(state.IsPositiveGate);
            Assert.Contains("Requires sustained", state.Description);
            Assert.DoesNotContain("Blocked by", state.Description);
        }

        // ── Polarity classification ──────────────────────────────────

        [Theory]
        [InlineData(new[] { "Blizzard" }, null, WeatherGatePolarity.NegativeBlockedDuring)]
        [InlineData(null, new[] { "Blizzard" }, WeatherGatePolarity.PositiveRequiredWeather)]
        [InlineData(new[] { "Blizzard" }, new[] { "Blizzard" }, WeatherGatePolarity.PositiveRequiredWeather)]
        public void ClassifyPolarity_FollowsDocumentedRule(
            string[]? blocked, string[]? required, WeatherGatePolarity expected)
        {
            var gate = Gate("gate_x", "route_x", blocked, required);
            Assert.Equal(expected, WeatherGateEvaluator.ClassifyPolarity(gate));
        }

        // ── Live evaluation (F10) — inventory only where it matters ──

        [Fact]
        public void EvaluateLive_WithoutInventory_EvaluatesWeatherOnly()
        {
            var evaluator = new WeatherGateEvaluator(Catalog(
                Gate("gate_lake_edge_blizzard", "route_02", blocked: new[] { "Blizzard" })));

            var state = evaluator.EvaluateLive("gate_lake_edge_blizzard", WeatherKind.Blizzard);

            Assert.False(state.IsOpen);
        }

        [Fact]
        public void EvaluateLive_WithIrrelevantInventory_SameAnswerAsWeatherOnly()
        {
            var catalog = Catalog(
                Gate("gate_lake_edge_blizzard", "route_02", blocked: new[] { "Blizzard" }));
            var evaluator = new WeatherGateEvaluator(catalog);

            var weatherOnly = evaluator.EvaluateWeatherOnly("gate_lake_edge_blizzard", WeatherKind.Blizzard);
            var live = evaluator.EvaluateLive("gate_lake_edge_blizzard", WeatherKind.Blizzard,
                new[] { "item_rope", "item_lamp" });

            Assert.Equal(weatherOnly.IsOpen, live.IsOpen);
            Assert.Equal(weatherOnly.Reason, live.Reason);
        }

        // ── Transitions (F11) ────────────────────────────────────────

        [Fact]
        public void CompareWeatherStates_ClearToBlizzard_ProducesClosureTransition()
        {
            var catalog = Catalog(
                Gate("gate_mountain_pass_blizzard", "route_12", blocked: new[] { "Blizzard" }));
            var evaluator = new WeatherGateEvaluator(catalog);

            var transitions = evaluator.CompareWeatherStates(WeatherKind.Clear, WeatherKind.Blizzard);

            var closure = Assert.Single(transitions);
            Assert.Equal("gate_mountain_pass_blizzard", closure.GateId);
            Assert.True(closure.WasOpen);
            Assert.False(closure.IsOpen);
            Assert.Equal(WeatherGatePolarity.NegativeBlockedDuring, closure.Polarity);
        }

        [Fact]
        public void CompareWeatherStates_BlizzardToClear_ClearsPositiveOpenState()
        {
            var catalog = Catalog(
                Gate("gate_frozen_lake_crossing", "route_06", required: new[] { "Blizzard" }));
            var evaluator = new WeatherGateEvaluator(catalog);

            // Gate open under Blizzard (positive state set)
            var open = evaluator.EvaluateWeatherOnly("gate_frozen_lake_crossing", WeatherKind.Blizzard);
            Assert.True(open.IsOpen);

            // Blizzard -> Clear closes it: open-state flag must clear
            var transitions = evaluator.CompareWeatherStates(WeatherKind.Blizzard, WeatherKind.Clear);
            var clearing = Assert.Single(transitions);
            Assert.True(clearing.WasOpen);
            Assert.False(clearing.IsOpen);
        }

        [Fact]
        public void CompareWeatherStates_UnrelatedChange_ProducesNoTrigger()
        {
            var catalog = Catalog(
                Gate("gate_river_basin_fog", "route_03", blocked: new[] { "BioFog" }));
            var evaluator = new WeatherGateEvaluator(catalog);

            var transitions = evaluator.CompareWeatherStates(WeatherKind.Rain, WeatherKind.Overcast);

            Assert.Empty(transitions);
        }

        [Fact]
        public void CompareWeatherStates_SameInputTwice_IdenticalOrder()
        {
            var catalog = Catalog(
                Gate("gate_mountain_pass_blizzard", "route_12", blocked: new[] { "Blizzard" }),
                Gate("gate_frozen_lake_crossing", "route_06", required: new[] { "Blizzard" }));
            var evaluator = new WeatherGateEvaluator(catalog);

            var first = evaluator.CompareWeatherStates(WeatherKind.Clear, WeatherKind.Blizzard);
            var second = evaluator.CompareWeatherStates(WeatherKind.Clear, WeatherKind.Blizzard);

            Assert.Equal(first.Select(t => t.GateId), second.Select(t => t.GateId));
        }
    }
}
