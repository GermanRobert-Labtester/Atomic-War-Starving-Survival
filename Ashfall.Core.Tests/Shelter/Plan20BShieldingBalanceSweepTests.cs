// SPDX-License-Identifier: MIT
using System;
using System.Globalization;
using System.Text;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Plan20BShielding
{
    /// <summary>
    /// C2 / Plan 20B (§28/§30) — contributor breakdown semantics and the seeded
    /// 60-day shielding sweep. The breakdown must be produced by the same
    /// arithmetic path as the dose and must name the largest single mover;
    /// the sweep records dose/day curves for the shelter contributors and
    /// asserts monotonic ordering + determinism. No balance data is modified.
    /// </summary>
    public sealed class Plan20BShieldingBreakdownTests
    {
        [Fact]
        public void IntactDryShelter_WeakestContributor_IsCeilingAttenuation()
        {
            var model = new ShelterShieldingModel { StructuralAttenuationProvider = () => 0.6f };
            var b = model.GetBreakdown(2.0f);
            Assert.Equal("ceiling attenuation", b.WeakestContributor);
            Assert.Equal(0.8f, b.WeakestContribution, 3); // bleed 2 × 0.4
            Assert.Equal(0.8f, b.InteriorRad, 3);
        }

        [Fact]
        public void CloggedFilter_IsNamedWeakest_WhenItDominates()
        {
            var model = new ShelterShieldingModel
            {
                StructuralAttenuationProvider = () => 0.6f, // bleed 0.8
                FilterHealthPercentProvider = () => 0f,      // penalty 0.6
                WeatherRadModifierProvider = () => 150f      // ×2.5 → 1.5 > bleed
            };
            var b = model.GetBreakdown(2.0f);
            Assert.Equal("air filter", b.WeakestContributor);
            Assert.Equal(1.2f, b.WeakestContribution, 3); // bleed 0.8 × penalty 1.5
            Assert.Equal(2.0f, b.InteriorRad, 3);         // 0.8 × (1 + 1.5)
        }

        [Fact]
        public void Radon_IsNamedWeakest_WhenItDominates()
        {
            var model = new ShelterShieldingModel
            {
                StructuralAttenuationProvider = () => 1.0f, // bleed 0
                IndoorRadonProvider = () => 800f             // 4.0 mSv/h
            };
            var b = model.GetBreakdown(2.0f);
            Assert.Equal("radon", b.WeakestContributor);
            Assert.Equal(4.0f, b.WeakestContribution, 3);
            Assert.Equal(4.0f, b.InteriorRad, 3);
        }

        [Fact]
        public void FloodingOutweighsStructure_IsNamedWeakest()
        {
            var model = new ShelterShieldingModel
            {
                StructuralAttenuationProvider = () => 0.5f, // bleed 1.0
                FloodingContaminationProvider = () => 0.9f  // 3.6
            };
            var b = model.GetBreakdown(2.0f);
            Assert.Equal("flooding", b.WeakestContributor);
            Assert.Equal(3.6f, b.WeakestContribution, 3);
        }

        [Fact]
        public void BreakdownInterior_AlwaysMatchesComputeInterior()
        {
            var model = new ShelterShieldingModel
            {
                StructuralAttenuationProvider = () => 0.5f,
                FilterHealthPercentProvider = () => 55f,
                AirlockSealProvider = () => 0f,
                WeatherRadModifierProvider = () => 150f,
                IndoorRadonProvider = () => 200f,
                FloodingContaminationProvider = () => 0.2f,
                ShelterContaminationProvider = () => 0.05f,
                DeconActiveProvider = () => true
            };
            Assert.Equal(model.ComputeInteriorRad(2.0f), model.GetBreakdown(2.0f).InteriorRad, 4);
        }

        [Fact]
        public void DeconFlag_IsReported_AndInternalHalved()
        {
            var model = new ShelterShieldingModel
            {
                StructuralAttenuationProvider = () => 1.0f,
                ShelterContaminationProvider = () => 0.1f, // 0.8 internal
                DeconActiveProvider = () => true
            };
            var b = model.GetBreakdown(2.0f);
            Assert.True(b.DeconReduced);
            Assert.Equal(0.4f, b.InternalRad, 3);
            Assert.Equal(0.4f, b.InteriorRad, 3);
        }
    }

    public sealed class Plan20BShieldingBalanceSweepTests
    {
        private const int HoursPerDay = 24;
        private const int Days = 60;

        private static (float dose, float lifetime, ShelterShieldingBreakdown breakdown) Run(
            Action<ShelterShieldingModel> configure)
        {
            var model = new ShelterShieldingModel();
            configure(model);
            var resolver = new ExposureEnvironmentResolver
            {
                ShelterAttenuationProvider = () => model.StructuralAttenuationProvider?.Invoke() ?? 0f,
                ShelterInteriorRadQuery = zone => model.ComputeInteriorRad(zone)
            };
            var state = new SurvivorRadState { Id = "sweep_b" };
            var sys = new RadiationSystem(exposureContext: _ =>
                resolver.ResolveForEnvironment(SurvivorExposureLocation.ShelterInterior, "")
                    .ToExposureContext());
            sys.Register(state);
            for (int hour = 0; hour < Days * HoursPerDay; hour++)
                sys.Tick(1f);
            return (state.RadiationDose, state.LifetimeRadiationExposure, model.GetBreakdown(2.0f));
        }

        [Fact]
        public void IntactShelter_RadonFloorOnly()
        {
            var (_, lifetime, b) = Run(m =>
            {
                m.StructuralAttenuationProvider = () => 1.0f;
                m.IndoorRadonProvider = () => 12f; // 0.06 mSv/h
            });
            Assert.Equal(0.06f, b.InteriorRad, 3); // 86.4 over 60 days
            Assert.InRange(lifetime, 86.3f, 86.5f);
        }

        [Fact]
        public void DegradedCeiling_AddsStructuralBleed()
        {
            var (_, lifetime, _) = Run(m =>
            {
                m.StructuralAttenuationProvider = () => 0.5f;
                m.IndoorRadonProvider = () => 12f;
            });
            // (1.0 + 0.06) × 1440
            Assert.InRange(lifetime, 1526.3f, 1526.5f);
        }

        [Fact]
        public void CloggedFilter_ExceedsDegradedCeiling()
        {
            var (_, clogged, _) = Run(m =>
            {
                m.StructuralAttenuationProvider = () => 0.5f;
                m.FilterHealthPercentProvider = () => 0f; // ×1.6
                m.IndoorRadonProvider = () => 12f;
            });
            var (_, degraded, _) = Run(m =>
            {
                m.StructuralAttenuationProvider = () => 0.5f;
                m.IndoorRadonProvider = () => 12f;
            });
            Assert.True(clogged > degraded);
            Assert.InRange(clogged, 2390.3f, 2390.5f); // (1.6 + 0.06) × 1440
        }

        [Fact]
        public void StormAmplifiesOpenAirlock_ButNotIntactStructure()
        {
            var (_, stormOpen, _) = Run(m =>
            {
                m.StructuralAttenuationProvider = () => 0.5f;
                m.AirlockSealProvider = () => 0f;
                m.WeatherRadModifierProvider = () => 150f; // 0.8 × 2.5 = 2.0 → ×3.0
                m.IndoorRadonProvider = () => 12f;
            });
            var (_, stormIntact, _) = Run(m =>
            {
                m.StructuralAttenuationProvider = () => 0.5f;
                m.WeatherRadModifierProvider = () => 150f;
                m.IndoorRadonProvider = () => 12f;
            });
            Assert.InRange(stormOpen, 4406.3f, 4406.5f); // (3.0 + 0.06) × 1440
            Assert.Equal(stormIntact, 1526.4f, 1);       // weather cannot enter an intact structure
        }

        [Fact]
        public void FloodingAndDecon_MoveTheCurveAsAuthored()
        {
            var (_, flooded, _) = Run(m =>
            {
                m.StructuralAttenuationProvider = () => 0.5f;
                m.IndoorRadonProvider = () => 12f;
                m.FloodingContaminationProvider = () => 0.25f; // +1.0
            });
            var (_, deconced, _) = Run(m =>
            {
                m.StructuralAttenuationProvider = () => 0.5f;
                m.IndoorRadonProvider = () => 12f;
                m.FloodingContaminationProvider = () => 0.25f;
                m.DeconActiveProvider = () => true; // internal halved: 0.5 + 0.03
            });
            Assert.InRange(flooded, 2966.3f, 2966.5f); // (1.0 + 0.06 + 1.0) × 1440
            Assert.InRange(deconced, 2203.1f, 2203.3f); // (1.0 + 0.53) × 1440
            Assert.True(deconced < flooded);
        }

        [Fact]
        public void Sweep_IsDeterministic_PairedRunsIdentical()
        {
            string Fingerprint()
            {
                var scenarios = new Action<ShelterShieldingModel>[]
                {
                    m => { m.StructuralAttenuationProvider = () => 1.0f; m.IndoorRadonProvider = () => 12f; },
                    m => { m.StructuralAttenuationProvider = () => 0.5f; },
                    m => { m.StructuralAttenuationProvider = () => 0.5f; m.FilterHealthPercentProvider = () => 0f; },
                    m => { m.StructuralAttenuationProvider = () => 0.5f; m.AirlockSealProvider = () => 0f; m.WeatherRadModifierProvider = () => 150f; },
                    m => { m.StructuralAttenuationProvider = () => 0.5f; m.FloodingContaminationProvider = () => 0.25f; m.DeconActiveProvider = () => true; }
                };
                var sb = new StringBuilder();
                foreach (var s in scenarios)
                {
                    var (dose, life, b) = Run(s);
                    sb.Append(CultureInfo.InvariantCulture,
                        $"{dose:F4}|{life:F4}|{b.WeakestContributor}|{b.InteriorRad:F4};");
                }
                return sb.ToString();
            }

            Assert.Equal(Fingerprint(), Fingerprint());
        }
    }
}