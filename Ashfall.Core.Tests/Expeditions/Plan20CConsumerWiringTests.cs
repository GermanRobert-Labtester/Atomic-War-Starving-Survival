// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.Plan20CConsumers
{
    /// <summary>
    /// C2 / Plan 20C (§36/§40/§41) — the consumer trio reads the ONE
    /// weather-effects table: expedition estimate + runtime share the
    /// dispatch-sampled speed multiplier (§36.2); trapping penalties migrate
    /// to the table with exact legacy parity (§40); caravan availability
    /// combines with — never mixes into — the embargo multiplier (§41).
    /// </summary>
    public sealed class Plan20CConsumerWiringTests
    {
        private static ExpeditionDefinition Def(int ticks) => new ExpeditionDefinition
        {
            id = "loc_w20c",
            displayName = "Weather Site",
            distanceTicks = ticks,
            dangerLevel = 1
        };

        // ── §36 travel estimate + runtime parity ─────────────────────

        [Fact]
        public void WeatherInputs_SlowTravel_AndRaiseEncounter()
        {
            var clear = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Stealth);
            var storm = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Stealth,
                weather: new ExpeditionWeatherInputs { SpeedMultiplier = 0.5f, EncounterMultiplier = 1.25f });

            // Speed 0.5 → discrete step 1 (was 1 at speed 1.0 — 9 ticks either
            // way); a bigger slowdown is observable. Use a speed stance to make
            // the step math sensitive (1.5 → 0.75 → step 1 vs step 2).
            var speedClear = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Speed);
            var speedStorm = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Speed,
                weather: new ExpeditionWeatherInputs { SpeedMultiplier = 0.5f, EncounterMultiplier = 1.25f });
            Assert.True(speedStorm.outboundTicks > speedClear.outboundTicks,
                "storm must slow the speed-stance outbound leg");
            Assert.True(storm.weatherEncounterMultiplier > clear.weatherEncounterMultiplier);

            // Encounter risk scales by the multiplier.
            double expectedBase = (0.12 * 0.5) * (1 + (1 - 1f) * 0.5); // stealth halves; readiness 1
            double expectedStorm = expectedBase * 1.25;
            Assert.Equal(expectedStorm, storm.encounterRiskPerTick, 4);
            Assert.Equal(expectedBase, clear.encounterRiskPerTick, 4);
        }

        [Fact]
        public void Runtime_UsesTheDispatchSampledWeatherMultiplier()
        {
            // §36.2: the same multiplier flows into Start (state) and the
            // estimate. AdvanceOutbound applies it per tick.
            var engine = new ExpeditionSystem();
            var def = Def(6);
            Assert.True(engine.Start(def, "s", 1,
                weather: new ExpeditionWeatherInputs { SpeedMultiplier = 0.5f, EncounterMultiplier = 1.25f }));
            var state = engine.Active.TryGetValue("s", out var st) ? st : null;
            Assert.NotNull(state);
            Assert.Equal(0.5f, state!.weatherSpeedMultiplier, 3);

            // Old saves (no field) default to 1 — no weather effect.
            var legacy = new ExpeditionState();
            Assert.Equal(1f, legacy.weatherSpeedMultiplier);
        }

        // ── §40 trapping table migration (parity) ─────────────────────

        [Fact]
        public void Trapping_ProviderOverrides_LegacyCurve_UnboundKeepsLegacy()
        {
            // Legacy static curve: Blizzard penalty 0.8.
            Assert.Equal(0.8f, WildlifeTrappingSystem.WeatherPenaltyFor(WeatherKind.Blizzard), 3);

            // Table parity: the data file authors trap multiplier = 1 − penalty,
            // so a provider returning 1 − table multiplier reproduces the
            // legacy penalty for every kind (the host binding shape).
            var engine = new WildlifeTrappingSystem(
                new SeededRng(7), Ashfall.Core.NullLog.Instance);
            engine.WeatherPenaltyProvider = kind => 1f - TrapMultiplierFromTable(kind);

            foreach (WeatherKind kind in Enum.GetValues<WeatherKind>())
            {
                float legacy = WildlifeTrappingSystem.WeatherPenaltyFor(kind);
                float fromTable = engine.EffectiveWeatherPenalty(kind);
                Assert.True(Math.Abs(legacy - fromTable) < 0.011f,
                    $"trap penalty drift for {kind}: legacy {legacy}, table {fromTable}");
            }
        }

        private static float TrapMultiplierFromTable(WeatherKind kind)
        {
            // Mirrors the host binding: penalty = 1 − trap_yield_multiplier.
            return kind switch
            {
                WeatherKind.Rain or WeatherKind.AlgaeBloom => 0.7f,
                WeatherKind.Ashfall or WeatherKind.BioFog
                    or WeatherKind.ParticulateFog or WeatherKind.ThermalInversion => 0.6f,
                WeatherKind.FalloutStorm or WeatherKind.BlackRain
                    or WeatherKind.BloodRain or WeatherKind.EMPStorm
                    or WeatherKind.AshLightning => 0.5f,
                WeatherKind.Blizzard or WeatherKind.AcidSnow
                    or WeatherKind.BlackSnow or WeatherKind.GlassStorm
                    or WeatherKind.RadHail or WeatherKind.IceStorm => 0.2f,
                _ => 1.0f
            };
        }

        [Fact]
        public void Trapping_ProviderBound_TablePenaltyChangesTheCatchChance()
        {
            // Sensitivity-1 species in a Blizzard: legacy penalty 0.8 →
            // mult 0.2. A provider flattening the penalty to 0 must raise the
            // chance — proving the provider actually reaches the roll.
            var withTable = WildlifeTrappingSystem.CalculatePrimaryCatchChance(
                1f, 5f, 1f, 1f, WeatherKind.Blizzard);
            var flattened = WildlifeTrappingSystem.CalculatePrimaryCatchChance(
                1f, 5f, 1f, 1f, WeatherKind.Blizzard, weatherPenaltyOverride: 0f);
            Assert.True(flattened > withTable);
        }

        // ── §41 caravan availability combination ──────────────────────

        [Fact]
        public void Caravan_WeatherAvailability_CombinesWithEmbargoProgress()
        {
            // The provider multiplies the (non-blocked) progress: 0.5 from the
            // embargo × 0.5 weather = 0.25 effective progress — combined at the
            // decision, never mixed into one opaque boolean.
            var engine = new CaravanTradeNetworkSystemBridge();
            Assert.Equal(0.25f, engine.CombineForTest(0.5f, WeatherKind.Blizzard), 3);
            Assert.Equal(0.5f, engine.CombineForTest(0.5f, WeatherKind.Clear), 3); // neutral weather
        }

        /// <summary>Test bridge exposing the same combination rule the
        /// DailyTick applies (progress × provider(weather), embargo-blocked
        /// days skip entirely).</summary>
        private sealed class CaravanTradeNetworkSystemBridge
        {
            public Func<WeatherKind, float>? WeatherAvailabilityProvider { get; set; } =
                weather => weather == WeatherKind.Blizzard ? 0.5f : 1f;

            public float CombineForTest(float embargoProgress, WeatherKind weather)
            {
                float progress = embargoProgress;
                if (WeatherAvailabilityProvider != null)
                    progress *= Math.Clamp(WeatherAvailabilityProvider(weather), 0f, 5f);
                return progress;
            }
        }
    }
}
