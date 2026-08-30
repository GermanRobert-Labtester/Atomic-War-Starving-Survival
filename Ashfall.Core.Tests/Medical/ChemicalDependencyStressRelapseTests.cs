// SPDX-License-Identifier: MIT
// Plan 09 / 9B Core — OnStressReported subscription API on
// ChemicalDependencySystem. Until this commit, relapse was driven only by
// consumption (OnSubstanceConsumed). External stress sources (guilt spike,
// combat trauma, ration cut, etc.) had no Core entry point. This file pins
// the new contract: deterministic per-call nudges, save round-trip stable,
// no schema bump.
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class ChemicalDependencyStressRelapseTests
    {
        private const string SurvivorA = "survivor_stress_patient";
        private const string ItemOpioid = "morphine";
        private const string ItemAlcohol = "vodka";
        private const string ItemStimulant = "caffeine_pills";
        private const string ItemSedative = "sleeping_pills";

        private static ChemicalDependencySystem NewSystem()
            => new ChemicalDependencySystem();

        // ── StressRelapseRules (pure function, no system state) ────────

        [Theory]
        [InlineData(0.0f, ChemicalDependencyKind.Opioid, 0f)]           // below threshold
        [InlineData(0.04f, ChemicalDependencyKind.Opioid, 0f)]          // still below
        [InlineData(0.05f, ChemicalDependencyKind.Opioid, 0.0068f)]    // 0.05×0.9×0.15 rounded
        [InlineData(0.5f, ChemicalDependencyKind.Opioid, 0.0675f)]      // 0.5×0.9×0.15
        [InlineData(0.5f, ChemicalDependencyKind.Alcohol, 0.0525f)]
        [InlineData(0.5f, ChemicalDependencyKind.Stimulant, 0.045f)]
        [InlineData(0.5f, ChemicalDependencyKind.Sedative, 0.0375f)]
        [InlineData(1.0f, ChemicalDependencyKind.Opioid, 0.135f)]      // 1.0×0.9×0.15
        [InlineData(1.5f, ChemicalDependencyKind.Opioid, 0.135f)]      // clamp to 1.0
        [InlineData(-1.0f, ChemicalDependencyKind.Opioid, 0f)]
        [InlineData(float.NaN, ChemicalDependencyKind.Opioid, 0f)]     // NaN guard
        public void ComputeDelta_ReturnsExpectedClampedSeverityScaled(float magnitude,
            ChemicalDependencyKind kind, float expected)
        {
            float got = StressRelapseRules.ComputeDelta(magnitude, kind);
            // Float math + culture-invariant rounding tolerance.
            const float tol = 0.0005f;
            Assert.True(Math.Abs(got - expected) <= tol,
                $"magnitude={magnitude}, kind={kind}, expected={expected}, got={got}");
        }

        [Fact]
        public void ComputeDelta_IsHostStable_BothSidesSeeSameResult()
        {
            // Determinism invariant — SaveChecksum gate relies on this. Two
            // independent calls into the rules table from different system
            // instances must produce the same value.
            var a = StressRelapseRules.ComputeDelta(0.7f, ChemicalDependencyKind.Alcohol);
            var b = StressRelapseRules.ComputeDelta(0.7f, ChemicalDependencyKind.Alcohol);
            Assert.Equal(a, b);
        }

        [Fact]
        public void IsReportable_CheapestFilterForPreCallGating()
        {
            Assert.False(StressRelapseRules.IsReportable(0f));
            Assert.False(StressRelapseRules.IsReportable(-1f));
            Assert.False(StressRelapseRules.IsReportable(float.NaN));
            Assert.True(StressRelapseRules.IsReportable(0.05f));
        }

        // ── ReportStress — system-level invariants ──────────────────────

        [Fact]
        public void ReportStress_ReturnsZero_WhenNoLedgerEntry()
        {
            var sys = NewSystem();
            int bumped = sys.ReportStress(SurvivorA, "guilt_spike", 0.8f);
            Assert.Equal(0, bumped);
        }

        [Fact]
        public void ReportStress_AlwaysRaisesOnStressReported_OncePerCall()
        {
            var sys = NewSystem();
            int echoCount = 0;
            sys.OnStressReported += (survivorId, source, magnitude) =>
            {
                echoCount++;
                Assert.Equal(SurvivorA, survivorId);
                Assert.Equal("raid", source);
                Assert.Equal(0.65f, magnitude);
            };
            sys.ReportStress(SurvivorA, "raid", 0.65f);
            sys.ReportStress(SurvivorA, "raid", 0.65f);
            Assert.Equal(2, echoCount);
        }

        [Fact]
        public void ReportStress_NudgesActiveDependency_PerKindDelta()
        {
            var sys = NewSystem();
            // Establish two dependencies at mid-range.
            for (int i = 0; i < 3; i++) sys.OnSubstanceConsumed(SurvivorA, ItemOpioid, ChemicalDependencyKind.Opioid);
            for (int i = 0; i < 3; i++) sys.OnSubstanceConsumed(SurvivorA, ItemAlcohol, ChemicalDependencyKind.Alcohol);
            // Three doses: float accumulation may offset slightly; treat both
            // values as "around 0.30 + 0.15" using tolerance.
            const float tol = 0.0001f;
            Assert.True(Math.Abs(0.45f -
                sys.DependencyLevel(SurvivorA, ItemOpioid)) <= tol);
            Assert.True(Math.Abs(0.45f -
                sys.DependencyLevel(SurvivorA, ItemAlcohol)) <= tol);

            int bumped = sys.ReportStress(SurvivorA, "ration_cut", 0.5f);

            // 0.5 × 0.9(severity) × 0.15(cap) = 0.0675 for Opioid,
            // 0.5 × 0.7(severity) × 0.15(cap) = 0.0525 for Alcohol. Both
            // non-zero, both nudged. Baseline here is 0.45 (3 × 0.15), so:
            // 0.45 + 0.0675 = 0.5175, 0.45 + 0.0525 = 0.5025.
            Assert.Equal(2, bumped);
            Assert.True(Math.Abs(0.5175f -
                sys.DependencyLevel(SurvivorA, ItemOpioid)) <= 0.0001f);
            Assert.True(Math.Abs(0.5025f -
                sys.DependencyLevel(SurvivorA, ItemAlcohol)) <= 0.0001f);
        }

        [Fact]
        public void ReportStress_DoesNotTouch_DependenciesInDetoxPrograms()
        {
            var sys = NewSystem();
            for (int i = 0; i < 3; i++) sys.OnSubstanceConsumed(SurvivorA, ItemOpioid, ChemicalDependencyKind.Opioid);
            sys.BeginManagedDetox(SurvivorA, ItemOpioid);

            float before = sys.DependencyLevel(SurvivorA, ItemOpioid);

            int bumped = sys.ReportStress(SurvivorA, "guilt_spike", 0.9f);

            Assert.Equal(0, bumped);
            Assert.Equal(before, sys.DependencyLevel(SurvivorA, ItemOpioid));
        }

        [Fact]
        public void ReportStress_RespectsSaturationFloor_NeverExceedsMax()
        {
            var sys = NewSystem();
            // Saturate Opioid dependency regardless of delta math.
            for (int i = 0; i < 30; i++) sys.OnSubstanceConsumed(SurvivorA, ItemOpioid, ChemicalDependencyKind.Opioid);
            float saturated = sys.DependencyLevel(SurvivorA, ItemOpioid);
            Assert.Equal(ChemicalDependencySystem.MaxDependencyLevel, saturated);

            int bumped = sys.ReportStress(SurvivorA, "raid", 1.0f);
            Assert.Equal(0, bumped); // clamped → no movement, no change → no nudge
            Assert.Equal(ChemicalDependencySystem.MaxDependencyLevel,
                sys.DependencyLevel(SurvivorA, ItemOpioid));
        }

        [Fact]
        public void ReportStress_BelowThresholdMagnitude_ReportsButDoesNotMutate()
        {
            var sys = NewSystem();
            for (int i = 0; i < 3; i++) sys.OnSubstanceConsumed(SurvivorA, ItemOpioid, ChemicalDependencyKind.Opioid);
            float before = sys.DependencyLevel(SurvivorA, ItemOpioid);

            int bumped = sys.ReportStress(SurvivorA, "temper_issue", 0.04f);
            Assert.Equal(0, bumped);
            Assert.Equal(before, sys.DependencyLevel(SurvivorA, ItemOpioid));
        }

        [Fact]
        public void ReportStress_OnCrossUp_FiresOnDependencyReFormedByStress()
        {
            // The cross-up event is meant to mark "stress caused a sub-threshold
            // survivor to fall into dependency". Two doses naturally land at
            // exactly DependencyThreshold (0.30), which is *not* a clean
            // sub-threshold baseline; the rule correctly treats that as already
            // addicted. We hand-poke the ledger to 0.299 so the cross-up event
            // is the only legitimate cause for the threshold break.
            var sys = NewSystem();
            sys.OnSubstanceConsumed(SurvivorA, ItemOpioid, ChemicalDependencyKind.Opioid);
            Assert.Single(sys.DependenciesFor(SurvivorA));
            // Force the ledger row strictly below threshold.
            ((List<ChemicalDependencyState>)sys.Ledger[SurvivorA])[0].dependencyLevel = 0.299f;
            Assert.Equal(0.299f, sys.DependencyLevel(SurvivorA, ItemOpioid));

            (string survivorId, string itemId, ChemicalDependencyKind kind) captured = default;
            int crossUpCount = 0;
            sys.OnDependencyReFormedByStress += (s, id, k) =>
            {
                crossUpCount++;
                captured = (s, id, k);
            };

            // 1.0 × 0.9 × 0.15 = 0.135 delta. 0.299 + 0.135 = 0.434 → crosses.
            int bumped = sys.ReportStress(SurvivorA, "raid", 1.0f);

            Assert.Equal(1, bumped);
            Assert.Equal(1, crossUpCount);
            Assert.Equal(SurvivorA, captured.survivorId);
            Assert.Equal(ItemOpioid, captured.itemId);
            Assert.Equal(ChemicalDependencyKind.Opioid, captured.kind);
        }

        // ── Save / load round-trip (Plan 09 9B must not break the DTO) ──

        [Fact]
        public void DependencyLedger_Capture_Restore_StableAcrossStressCall()
        {
            // Plan 09 9B stress nudges ride on the existing dependencyLevel
            // field. The shape of CaptureState is unchanged — survivors
            // (sorted ordinal), dependencies (sorted by itemId ordinal), no
            // new fields. This test pins two invariants:
            //   (a) before-stress state round-trips via Capture/Restore
            //       with no field drift;
            //   (b) after-stress state round-trips the new dependencyLevel
            //       exactly, so a SaveChecksum hash computed pre- and post-
            //       stress changes deterministically only via the level.
            var sys = NewSystem();
            sys.OnSubstanceConsumed(SurvivorA, ItemOpioid, ChemicalDependencyKind.Opioid);
            sys.OnSubstanceConsumed(SurvivorA, ItemAlcohol, ChemicalDependencyKind.Alcohol);

            var snapBefore = sys.CaptureState();
            var rt = NewSystem();
            rt.RestoreState(snapBefore);
            Assert.Equal(sys.DependencyLevel(SurvivorA, ItemOpioid),
                rt.DependencyLevel(SurvivorA, ItemOpioid));
            Assert.Equal(sys.DependencyLevel(SurvivorA, ItemAlcohol),
                rt.DependencyLevel(SurvivorA, ItemAlcohol));
            // ledger can be re-captured without drift (stable normalised form).
            var snapBefore2 = rt.CaptureState();
            Assert.Equal(AssertLedgerSnapshotBytes(snapBefore), AssertLedgerSnapshotBytes(snapBefore2));

            // Stress once. Persist. Reload. Compare.
            sys.ReportStress(SurvivorA, "raid", 0.55f);
            var snapAfter = sys.CaptureState();
            var rt2 = NewSystem();
            rt2.RestoreState(snapAfter);
            Assert.Equal(sys.DependencyLevel(SurvivorA, ItemOpioid),
                rt2.DependencyLevel(SurvivorA, ItemOpioid));
            Assert.Equal(sys.DependencyLevel(SurvivorA, ItemAlcohol),
                rt2.DependencyLevel(SurvivorA, ItemAlcohol));
        }

        private static string AssertLedgerSnapshotBytes(ChemicalDependencyLedgerState state)
        {
            // Order-stable byte for byte-equality assertions across Capture.
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < state.survivors.Count; i++)
            {
                var s = state.survivors[i];
                for (int j = 0; j < s.dependencies.Count; j++)
                {
                    var d = s.dependencies[j];
                    sb.Append(s.survivorId).Append("|").Append(d.itemId).Append("|")
                      .Append(d.dependencyLevel.ToString("F4",
                          System.Globalization.CultureInfo.InvariantCulture))
                      .Append("|").Append(d.kind).Append("|")
                      .Append(d.inManagedDetox ? 1 : 0).Append("|")
                      .Append(d.inColdTurkey ? 1 : 0).Append("|")
                      .Append(d.detoxProgressHours.ToString("F4",
                          System.Globalization.CultureInfo.InvariantCulture))
                      .Append(";");
                }
            }
            return sb.ToString();
        }
    }
}
