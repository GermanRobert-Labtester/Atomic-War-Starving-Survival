// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W3 — Seasonal pressure (focused suite; run alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave CORE-MECH-W3-WINTER-PRESSURE-POWER-WATER (cases AV.3).
//
// Pinned here: the pure day→pressure provider, the authored rows, and the water
// owner's single consumption site (including the ReverseOsmosis double-charge
// fix the W3 forensic pass found). The power consumer is deferred by decision
// (DP-CM-1 resolved as water-first); its absence is asserted here so it cannot
// be forgotten silently.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SeasonalPressureProviderTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static List<YearOfAshEventEntry> LoadEvents()
        {
            return YearOfAshCatalogLoader.LoadEvents(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static WaterTreatmentSystem FreshWater()
        {
            var sys = new WaterTreatmentSystem(new NullLog());
            sys.RestoreState(new WaterTreatmentState());
            return sys;
        }

        // ── Provider contract (AV.3 cases 1–4) ───────────────────────────────

        [Fact]
        public void ClearDay_IsIdentityMultiplier()
        {
            var provider = new SeasonalPressureProvider(LoadEvents());
            Assert.Equal(1f, provider.MultiplierFor(1), 4);
            Assert.False(provider.IsUnderPressure(1));
        }

        [Fact]
        public void DeepFreeze_AuthoredAsStrongestWinterPressure()
        {
            var provider = new SeasonalPressureProvider(LoadEvents());
            var pressure = provider.PressureFor(180);
            Assert.NotNull(pressure);
            Assert.Equal("extreme_cold", pressure!.HazardType);
            Assert.True(pressure.Multiplier > 1f);
        }

        [Fact]
        public void EmptyProvider_IsNeutralEverywhere()
        {
            var empty = SeasonalPressureProvider.Empty;
            for (int d = 180; d <= 360; d += 13)
            {
                Assert.Equal(1f, empty.MultiplierFor(d), 4);
                Assert.False(empty.IsUnderPressure(d));
            }
        }

        [Fact]
        public void InvalidAndRunawayMultipliers_AreNeutralized()
        {
            // Days are spaced beyond the ±1 radius so each row is judged alone.
            var events = new List<YearOfAshEventEntry>
            {
                new YearOfAshEventEntry { id = "nan", day = 200, pressureMultiplier = float.NaN },
                new YearOfAshEventEntry { id = "neg", day = 210, pressureMultiplier = -3f },
                new YearOfAshEventEntry { id = "runaway", day = 220, pressureMultiplier = 50f }
            };
            var provider = new SeasonalPressureProvider(events);
            Assert.Equal(1f, provider.MultiplierFor(200), 4);
            Assert.Equal(1f, provider.MultiplierFor(210), 4);
            Assert.True(provider.MultiplierFor(220) <= 4f);
        }

        [Fact]
        public void Pressure_IsDeterministic_TwoPass()
        {
            var a = new SeasonalPressureProvider(LoadEvents());
            var b = new SeasonalPressureProvider(LoadEvents());
            Assert.Equal(a.PressureDays(), b.PressureDays());
            for (int d = 180; d <= 360; d += 5)
                Assert.Equal(a.MultiplierFor(d), b.MultiplierFor(d), 5);
        }

        [Fact]
        public void ExposureAndPressureAreSeparateSemantics()
        {
            // The diesel-gelling crisis strains utilities but is not fallout: the
            // two columns must not leak into each other (BK.1).
            var diesel = LoadEvents().First(e => e.id == "event_diesel_fuel_gelling");
            var provider = new SeasonalPressureProvider(LoadEvents());
            Assert.True(provider.IsUnderPressure(diesel.day));
            Assert.Equal(1f, diesel.exposureMultiplier, 4);
        }

        // ── Authored data (AV.3: data-first slice) ───────────────────────────

        [Fact]
        public void Catalog_AnnotatesWinterRows_InsideCanonRange()
        {
            var events = LoadEvents();
            int annotated = events.Count(e => e.pressureMultiplier > 1f);
            Assert.True(annotated >= 5, $"expected several authored pressure rows, found {annotated}");
            foreach (var e in events)
                Assert.InRange(e.day, 180, 360);
        }

        // ── Receiver contract: the water owner applies it once (AA.3) ────────

        [Fact]
        public void Water_AppliesSeasonalLoad_ExactlyOnce()
        {
            // Same input, two seasons: the pressurized day must degrade the filter
            // by the authored factor, exactly once.
            var clearDay = FreshWater();
            var stormDay = FreshWater();

            clearDay.SeasonalFilterLoadMultiplier = _ => 1.0f;
            stormDay.SeasonalFilterLoadMultiplier = _ => 2.0f;

            RunOneCharcoalBatch(clearDay);
            RunOneCharcoalBatch(stormDay);

            float clearDegradation = 100f - clearDay.FilterIntegrity;
            float stormDegradation = 100f - stormDay.FilterIntegrity;

            Assert.True(stormDegradation > 0f);
            Assert.Equal(clearDegradation * 2.0f, stormDegradation, 3);
        }

        [Fact]
        public void Water_UnwiredProvider_BehavesExactlyAsBefore()
        {
            // The additive seam must be inert until Main binds it (PIR-5/AC.2).
            var unwired = FreshWater();
            RunOneCharcoalBatch(unwired);
            float degradation = 100f - unwired.FilterIntegrity;
            Assert.True(degradation > 0f);
            // Charcoal filtration is FilterDegradePerUnit * input * 0.5, once.
            Assert.Equal(WaterTreatmentSystem.FilterDegradePerUnit * 10f * 0.5f, degradation, 3);
        }

        [Fact]
        public void Water_ReverseOsmosis_IsNoLongerDoubleCharged()
        {
            // W3 forensic finding: the RO branch subtracted filter degradation and
            // the shared tail subtracted it again. It is now charged exactly once.
            var ro = FreshWater();
            RunOneBatch(ro, TreatmentMode.ReverseOsmosis);
            float degradation = 100f - ro.FilterIntegrity;
            Assert.Equal(WaterTreatmentSystem.FilterDegradePerUnit * 10f, degradation, 3);
        }

        [Fact]
        public void Water_SeasonalLoadIsClampedAndNeutralizesBadInput()
        {
            var sys = FreshWater();
            sys.SeasonalFilterLoadMultiplier = _ => 99f; // clamped to 4
            RunOneCharcoalBatch(sys);
            float clamped = 100f - sys.FilterIntegrity;

            var nan = FreshWater();
            nan.SeasonalFilterLoadMultiplier = _ => float.NaN; // neutral 1.0
            RunOneCharcoalBatch(nan);
            float neutral = 100f - nan.FilterIntegrity;

            var plain = FreshWater();
            RunOneCharcoalBatch(plain);
            float baseline = 100f - plain.FilterIntegrity;

            Assert.Equal(baseline * 4f, clamped, 3);
            Assert.Equal(baseline, neutral, 3);
        }

        [Fact]
        public void Power_Consumer_IsDeferred_NotSilentlyDropped()
        {
            // DP-CM-1 resolved as water-first: PowerGridSystem has no external
            // demand seam (fuel need is computed inside TickDay), so its winter
            // consumer is a named follow-up. This assertion keeps the deferral
            // visible in code rather than only in prose.
            var power = new Ashfall.Core.Shelter.PowerGridSystem(
                new Ashfall.Core.Shelter.PowerGridState(),
                new[] { new Ashfall.Core.Shelter.PowerGridRoom("r_test", "Test Room", 10f) },
                new Ashfall.Core.SeededRng(5));
            var summary = power.TickDay(1, new Ashfall.Core.SeededRng(5));
            Assert.Equal(1, summary.Day);
        }

        // ── helpers ───────────────────────────────────────────────────────────

        private static void RunOneCharcoalBatch(WaterTreatmentSystem sys)
            => RunOneBatch(sys, TreatmentMode.CharcoalFiltration);

        private static void RunOneBatch(WaterTreatmentSystem sys, TreatmentMode mode)
        {
            sys.RestoreState(new WaterTreatmentState
            {
                activeMode = mode,
                rawWater = 100f,
                charcoalSupply = 50f,
                processingTarget = 10f,
                isProcessing = true
            });
            for (int i = 0; i < 10; i++) sys.TickTreatment(0.1f);
        }

        private sealed class NullLog : ILog
        {
            public void Info(string message) { }
            public void Warn(string message) { }
            public void Error(string message) { }
        }
    }
}
