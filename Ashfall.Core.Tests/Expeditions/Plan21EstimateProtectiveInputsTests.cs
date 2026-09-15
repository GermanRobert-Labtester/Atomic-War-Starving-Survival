// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// C2 / Plan 21C (P6) — expedition estimate protective inputs. The
    /// projection must run through the canonical dose formula
    /// (RadiationSystem.ComputeExposurePerHour — one arithmetic path), predict
    /// mid-route gear failure from the data-authored wear rate, and leave the
    /// legacy estimate byte-identical when inputs are absent (plan §3.8/§33).
    /// </summary>
    public sealed class Plan21EstimateProtectiveInputsTests
    {
        private static ExpeditionDefinition Def(int ticks = 6)
        {
            return new ExpeditionDefinition
            {
                id = "loc_plan21_site",
                displayName = "Plan 21 Site",
                distanceTicks = ticks,
                dangerLevel = 1
            };
        }

        private static ExpeditionProtectiveInputs Inputs(
            float radRate = 24f,
            float protection = 10f,
            float degradeRate = 1f,
            float durability = 40f,
            float wearMult = 1f,
            float hoursPerTick = 1f,
            int unprotected = 0)
        {
            return new ExpeditionProtectiveInputs
            {
                LocationRadRatePerHour = radRate,
                WorkingProtection = protection,
                UnprotectedCount = unprotected,
                WeakestGearDegradeRate = degradeRate,
                WeakestGearDurability = durability,
                WearMultiplier = wearMult,
                HoursPerTick = hoursPerTick
            };
        }

        [Fact]
        public void NoProtectiveInputs_LegacyEstimateUnchanged()
        {
            var est = ExpeditionSystem.Estimate(Def(), ExpeditionStance.Stealth);
            Assert.Equal(0f, est.partyProtection);
            Assert.Equal(0, est.unprotectedCount);
            Assert.Equal(0f, est.projectedDosePerHour);
            Assert.Equal(0f, est.projectedDoseTotal);
            Assert.Equal(0f, est.projectedGearWear);
            Assert.Equal(0f, est.protectiveLifeHours);
            Assert.False(est.predictsMidRouteFailure);
        }

        [Fact]
        public void ProjectedDose_UsesTheCanonicalDoseFormula()
        {
            var est = ExpeditionSystem.Estimate(Def(), ExpeditionStance.Stealth,
                protective: Inputs(radRate: 24f, protection: 10f));
            // Canonical: max(0, 24 − 10) — the exact function RadiationSystem ticks.
            Assert.Equal(RadiationSystem.ComputeExposurePerHour(24f, 10f, 0f),
                est.projectedDosePerHour);
            Assert.Equal(14f, est.projectedDosePerHour, 3);
            Assert.Equal(14f * est.totalTicks, est.projectedDoseTotal, 3);
            Assert.Equal(est.totalTicks, est.projectedTripHours, 3); // hoursPerTick 1
        }

        [Fact]
        public void ProjectedGearWear_UsesDataRate_TimesMultiplier_TimesTripHours()
        {
            var est = ExpeditionSystem.Estimate(Def(), ExpeditionStance.Stealth,
                protective: Inputs(degradeRate: 1f, durability: 40f, wearMult: 5f));
            Assert.Equal(5f * est.totalTicks, est.projectedGearWear, 3);
            Assert.Equal(8f, est.protectiveLifeHours, 3); // 40 / (1 × 5)
        }

        [Fact]
        public void MidRouteFailure_PredictedOnlyWhenLifeShorterThanTrip()
        {
            // Life 40 h — plenty for a 9-tick trip.
            var safe = ExpeditionSystem.Estimate(Def(), ExpeditionStance.Stealth,
                protective: Inputs(degradeRate: 1f, durability: 40f));
            Assert.False(safe.predictsMidRouteFailure);

            // Life 4 h — fails before the trip ends.
            var doomed = ExpeditionSystem.Estimate(Def(), ExpeditionStance.Stealth,
                protective: Inputs(degradeRate: 1f, durability: 4f));
            Assert.True(doomed.predictsMidRouteFailure);
            Assert.Equal(4f, doomed.protectiveLifeHours, 3);
        }

        [Fact]
        public void NoGear_InHotZone_CountsAsUnprotected()
        {
            var est = ExpeditionSystem.Estimate(Def(), ExpeditionStance.Stealth,
                protective: Inputs(radRate: 24f, protection: 0f, degradeRate: 0f, durability: 0f));
            Assert.Equal(1, est.unprotectedCount);
            Assert.Equal(24f, est.projectedDosePerHour, 3); // full ambient, no mitigation
            Assert.False(est.predictsMidRouteFailure); // nothing to fail mid-route
        }

        [Fact]
        public void HoursPerTick_ScalesTripProjection()
        {
            var est = ExpeditionSystem.Estimate(Def(), ExpeditionStance.Stealth,
                protective: Inputs(radRate: 24f, protection: 10f, hoursPerTick: 6f));
            Assert.Equal(14f * est.totalTicks * 6f, est.projectedDoseTotal, 2);
            Assert.Equal(est.totalTicks * 6f, est.projectedTripHours, 3);
        }

        [Fact]
        public void Estimate_IsPure_SameInputsSameOutputs()
        {
            var a = ExpeditionSystem.Estimate(Def(), ExpeditionStance.Stealth,
                protective: Inputs(durability: 4f));
            var b = ExpeditionSystem.Estimate(Def(), ExpeditionStance.Stealth,
                protective: Inputs(durability: 4f));
            Assert.Equal(a.projectedDoseTotal, b.projectedDoseTotal, 3);
            Assert.Equal(a.projectedGearWear, b.projectedGearWear, 3);
            Assert.Equal(a.protectiveLifeHours, b.protectiveLifeHours, 3);
            Assert.Equal(a.predictsMidRouteFailure, b.predictsMidRouteFailure);
        }
    }

    /// <summary>
    /// Source gates: the host assembles protective inputs from the canonical
    /// authorities and binds the hook; the panel displays (never recomputes)
    /// the projection — warn-don't-block is display-only (plan §34/§35).
    /// </summary>
    public sealed class Plan21EstimateSourceGateTests
    {
        private static string RepoRoot
        {
            get
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8; i++)
                {
                    if (File.Exists(Path.Combine(dir, "Ashfall.csproj"))) return dir;
                    dir = Path.GetDirectoryName(dir)!;
                }
                throw new InvalidOperationException("repository root not found");
            }
        }

        private static string Read(string relativePath)
            => File.ReadAllText(Path.Combine(RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));

        [Fact]
        public void HostBinds_ProtectiveInputs_FromCanonicalAuthorities()
        {
            string expeditions = Read("src/Main.Expeditions.cs");
            Assert.Contains("SetEstimateProtectiveInputs", expeditions, StringComparison.Ordinal);

            string session = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("BuildProtectiveEstimateInputs", session, StringComparison.Ordinal);
            Assert.Contains("ResolveForEnvironment", session, StringComparison.Ordinal);
            Assert.Contains("ComputeGearProtection", session, StringComparison.Ordinal);
        }

        [Fact]
        public void Panel_WarnsWithoutBlocking_NoDoseArithmetic()
        {
            string panel = Read("src/UI/ExpeditionPanel.cs");
            Assert.Contains("GEAR FAILS MID-ROUTE", panel, StringComparison.Ordinal);
            Assert.Contains("NO WORKING PROTECTION", panel, StringComparison.Ordinal);
            // No panel-side dose formula (plan §3.8).
            Assert.DoesNotContain("ComputeExposurePerHour", panel, StringComparison.Ordinal);
        }
    }
}
