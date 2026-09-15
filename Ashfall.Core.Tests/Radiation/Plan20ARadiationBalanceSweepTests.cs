// SPDX-License-Identifier: MIT
using System;
using System.Globalization;
using System.Text;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.Plan20ARadiation
{
    /// <summary>
    /// C2 / Plan 20A (§17) — seeded 60-day exposure sweep over the canonical
    /// resolver + RadiationSystem, using the same data-authored values the
    /// shipped catalog carries (weather_effects.json: FalloutStorm +150;
    /// crossing_locations.json bands 18–24 rads/h). Deterministic by
    /// construction (fixed hourly ticks, no RNG). Records dose/day curves and
    /// acceptance-principle ordering; balance proposals live in
    /// docs/balance/BALANCE_SIM_radiation_exposure_20A.md, never in data.
    /// </summary>
    public sealed class Plan20ARadiationBalanceSweepTests
    {
        private const int HoursPerDay = 24;

        private static ExposureEnvironmentResolver Resolver(
            float attenuation, float weather, float locationRadRate)
        {
            var resolver = new ExposureEnvironmentResolver();
            resolver.ShelterAttenuationProvider = () => attenuation;
            resolver.WeatherRadModifierProvider = () => weather;
            resolver.LocationRadRateProvider = _ => locationRadRate;
            return resolver;
        }

        private static (float dose, float lifetime) Run(
            SurvivorExposureLocation kind, string locationId,
            float attenuation, float weather, float locationRadRate, int days)
        {
            var resolver = Resolver(attenuation, weather, locationRadRate);
            var state = new SurvivorRadState { Id = "sweep_survivor" };
            var sys = new RadiationSystem(exposureContext: _ =>
                resolver.ResolveForEnvironment(kind, locationId).ToExposureContext());
            sys.Register(state);
            for (int hour = 0; hour < days * HoursPerDay; hour++)
                sys.Tick(1f);
            return (state.RadiationDose, state.LifetimeRadiationExposure);
        }

        // ── Scenario curves (60 days unless noted) ─────────────────────

        [Fact]
        public void IntactShelter_Survivable_ZeroEffectiveDose()
        {
            var (dose, lifetime) = Run(SurvivorExposureLocation.ShelterInterior, "",
                attenuation: 1.0f, weather: 0f, locationRadRate: 0f, days: 60);
            Assert.Equal(0f, dose, 3);
            Assert.Equal(0f, lifetime, 3);
        }

        [Fact]
        public void DegradedShelter_TakesDoseButSurvivesHourlyCurve()
        {
            // zone 2.0 · shielding 0.8 → 1.2 mSv/h → 28.8/day.
            var (dose, lifetime) = Run(SurvivorExposureLocation.ShelterInterior, "",
                attenuation: 0.4f, weather: 0f, locationRadRate: 0f, days: 60);
            Assert.InRange(lifetime, 1727f, 1729f);
            Assert.Equal(100f, dose, 3); // acute scale saturates after ~4 days
        }

        [Fact]
        public void OutsideClear_AlignedWithWastelandBaseRate()
        {
            // 40 mSv/h → 960/day.
            var (dose, lifetime) = Run(SurvivorExposureLocation.WastelandOutdoors, "",
                attenuation: 0f, weather: 0f, locationRadRate: 0f, days: 60);
            Assert.InRange(lifetime, 57599f, 57601f);
            Assert.Equal(100f, dose, 3);
        }

        [Fact]
        public void OutsideFalloutStorm_AddsTheAuthoredModifier()
        {
            // (40 + 150) mSv/h → 4560/day.
            var (dose, lifetime) = Run(SurvivorExposureLocation.WastelandOutdoors, "",
                attenuation: 0f, weather: 150f, locationRadRate: 0f, days: 60);
            Assert.InRange(lifetime, 273599f, 273601f);
            Assert.Equal(100f, dose, 3);
        }

        [Fact]
        public void ExpeditionLowSector_UsesLocationCatalogRate()
        {
            // 6 mSv/h → 144/day.
            var (dose, lifetime) = Run(SurvivorExposureLocation.Expedition, "loc_low",
                attenuation: 0f, weather: 0f, locationRadRate: 6f, days: 60);
            Assert.InRange(lifetime, 8639f, 8641f);
            Assert.Equal(100f, dose, 3);
        }

        [Fact]
        public void ExpeditionHighSector_UsesLocationCatalogRate()
        {
            // 24 mSv/h (crossing_locations.json authored band) → 576/day.
            var (dose, lifetime) = Run(SurvivorExposureLocation.Expedition, "loc_hot",
                attenuation: 0f, weather: 0f, locationRadRate: 24f, days: 60);
            Assert.InRange(lifetime, 34559f, 34561f);
            Assert.Equal(100f, dose, 3);
        }

        [Fact]
        public void MultiZoneExpedition_PhaseCurve_AndReturnDoesNotHealDose()
        {
            float currentRate = 6f;
            float currentAttenuation = 0f;
            var resolver = new ExposureEnvironmentResolver
            {
                ShelterAttenuationProvider = () => currentAttenuation,
                WeatherRadModifierProvider = () => 0f,
                LocationRadRateProvider = _ => currentRate
            };
            int hour = 0;
            var state = new SurvivorRadState { Id = "sweep_multizone" };
            var sys = new RadiationSystem(exposureContext: _ =>
            {
                int day = hour / HoursPerDay;
                if (day < 20) return resolver
                    .ResolveForEnvironment(SurvivorExposureLocation.Expedition, "loc_phase").ToExposureContext();
                if (day < 40)
                {
                    currentRate = 24f;
                    return resolver
                        .ResolveForEnvironment(SurvivorExposureLocation.Expedition, "loc_phase").ToExposureContext();
                }
                currentAttenuation = 1.0f;
                return resolver
                    .ResolveForEnvironment(SurvivorExposureLocation.ShelterInterior, "").ToExposureContext();
            });
            sys.Register(state);
            for (hour = 0; hour < 60 * HoursPerDay; hour++)
                sys.Tick(1f);

            // 20d × 6 + 20d × 24 + 20d × 0 = 14,400 mSv accumulated while
            // deployed; returning indoors removes future accumulation but the
            // acute dose stays saturated (no healing).
            Assert.InRange(state.LifetimeRadiationExposure, 14399f, 14401f);
            Assert.Equal(100f, state.RadiationDose, 3);
        }

        [Fact]
        public void Ordering_ShelterLessThanExpeditionLessThanStorm()
        {
            var shelter = Run(SurvivorExposureLocation.ShelterInterior, "",
                attenuation: 0.4f, weather: 0f, locationRadRate: 0f, days: 1).lifetime;
            var lowSector = Run(SurvivorExposureLocation.Expedition, "loc_low",
                attenuation: 0f, weather: 0f, locationRadRate: 6f, days: 1).lifetime;
            var clear = Run(SurvivorExposureLocation.WastelandOutdoors, "",
                attenuation: 0f, weather: 0f, locationRadRate: 0f, days: 1).lifetime;
            var storm = Run(SurvivorExposureLocation.WastelandOutdoors, "",
                attenuation: 0f, weather: 150f, locationRadRate: 0f, days: 1).lifetime;
            Assert.True(shelter < lowSector, "degraded shelter must stay below expedition sector dose");
            Assert.True(lowSector < clear);
            Assert.True(clear < storm, "storm must be the most dangerous outdoor state");
        }

        [Fact]
        public void Sweep_IsDeterministic_PairedRunsIdentical()
        {
            string Fingerprint()
            {
                var scenarios = new (SurvivorExposureLocation kind, string id, float att, float weather, float loc)[]
                {
                    (SurvivorExposureLocation.ShelterInterior, "", 1.0f, 0f, 0f),
                    (SurvivorExposureLocation.ShelterInterior, "", 0.4f, 0f, 0f),
                    (SurvivorExposureLocation.WastelandOutdoors, "", 0f, 0f, 0f),
                    (SurvivorExposureLocation.WastelandOutdoors, "", 0f, 150f, 0f),
                    (SurvivorExposureLocation.Expedition, "loc_low", 0f, 0f, 6f),
                    (SurvivorExposureLocation.Expedition, "loc_hot", 0f, 0f, 24f)
                };
                var sb = new StringBuilder();
                foreach (var s in scenarios)
                {
                    var (dose, life) = Run(s.kind, s.id, s.att, s.weather, s.loc, 60);
                    sb.Append(CultureInfo.InvariantCulture, $"{s.kind}|{s.id}|{dose:F4}|{life:F4};");
                }
                return sb.ToString();
            }

            Assert.Equal(Fingerprint(), Fingerprint());
        }
    }
}