using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Unit tests for the photoperiod / light system.
    /// All tests are headless EditMode; no MonoBehaviour or scene required.
    /// </summary>
    [TestFixture]
    public class PhotoperiodTests
    {
        private const float Eps = 1e-3f;

        // -------------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------------

        private static SeasonProfile MakeProfile(
            float startDaylight = 12f,
            float endDaylight   = 2f,
            int   campaignDays  = 90)
        {
            var p = ScriptableObject.CreateInstance<SeasonProfile>();
            p.campaignLengthDays = campaignDays;
            p.ambientTemperatureCurve = AnimationCurve.Linear(0f, 5f, 1f, -35f);
            p.daylightCurve           = AnimationCurve.Linear(0f, startDaylight, 1f, endDaylight);
            p.weatherCheckIntervalHours = 6f;
            p.seasons = new[]
            {
                new SeasonWindow
                {
                    id = "nuclear_winter", displayName = "Nuclear Winter", startDay = 0,
                    clearWeight = 1f, ashfallWeight = 2f, falloutStormWeight = 1f, blizzardWeight = 1f
                }
            };
            return p;
        }

        private static LightProfile MakeLightProfile()
        {
            var lp = ScriptableObject.CreateInstance<LightProfile>();
            lp.lightExposureGainPerHourDaylight = 8f;
            lp.lightExposureLossPerHourDark     = 3f;
            lp.listlessThreshold                = 20f;
            lp.listlessMoraleDrainPerHour       = 0.5f;
            lp.sunLampSessionBoost              = 30f;
            lp.vitaminDGainPerHourNormalLight   = 2f;
            lp.vitaminDDecayPerHour             = 0.8f;
            lp.vitaminDLowThreshold             = 20f;
            lp.vitaminDHealthPenaltyPerHour     = 0.15f;
            lp.vitaminDMoralePenaltyPerHour     = 0.20f;
            lp.vitaminDFoodRestoreAmount        = 30f;
            lp.growLightEquivalentFraction      = 0.5f;
            lp.growLightMoraleBoostPerHour      = 0.3f;
            return lp;
        }

        private static Survivor MakeSurvivor(string id = "sv_test") =>
            new Survivor { Id = id, DisplayName = "Test", LightExposure = 100f, VitaminDProxy = 100f };

        // -------------------------------------------------------------------
        // 1. DaylightHours tracks season curve
        // -------------------------------------------------------------------

        [Test]
        public void DaylightHours_TracksSeasonCurve_MonotonicallyDeclines()
        {
            var profile = MakeProfile(startDaylight: 12f, endDaylight: 2f, campaignDays: 90);
            var weather = new WeatherSystem(profile, seed: 1);
            var pp      = new PhotoperiodSystem(profile, weather);

            float prevDaylight = pp.DaylightHours;
            for (int day = 1; day <= 90; day++)
            {
                pp.Tick(24f); // advance one day
                Assert.That(pp.DaylightHours, Is.LessThanOrEqualTo(prevDaylight + Eps),
                    $"DaylightHours rose on day {day}: {prevDaylight} -> {pp.DaylightHours}");
                prevDaylight = pp.DaylightHours;
            }

            // Should reach near campaign end value of 2
            Assert.That(pp.DaylightHours, Is.LessThan(4f),
                "DaylightHours should be near end-curve value of 2 after 90 days");
        }

        // -------------------------------------------------------------------
        // 2. SkyClarity maps WeatherKind
        // -------------------------------------------------------------------

        [Test]
        public void SkyClarity_MapsWeatherKind_Correctly()
        {
            var profile = MakeProfile();
            var weather = new WeatherSystem(profile, seed: 1);
            var pp      = new PhotoperiodSystem(profile, weather);

            weather.ForceWeather(WeatherKind.Clear);
            pp.Tick(0.01f);
            Assert.That(pp.SkyClarity, Is.EqualTo(PhotoperiodSystem.ClarityForClear).Within(Eps),
                "Clear sky should give full clarity");

            weather.ForceWeather(WeatherKind.Ashfall);
            pp.Tick(0.01f);
            Assert.That(pp.SkyClarity, Is.EqualTo(PhotoperiodSystem.ClarityForAshfall).Within(Eps),
                "Ashfall should give reduced clarity");

            weather.ForceWeather(WeatherKind.FalloutStorm);
            pp.Tick(0.01f);
            Assert.That(pp.SkyClarity, Is.EqualTo(PhotoperiodSystem.ClarityForFalloutStorm).Within(Eps),
                "FalloutStorm should give near-zero clarity");

            weather.ForceWeather(WeatherKind.Blizzard);
            pp.Tick(0.01f);
            Assert.That(pp.SkyClarity, Is.EqualTo(PhotoperiodSystem.ClarityForBlizzard).Within(Eps),
                "Blizzard should give partial clarity");
        }

        // -------------------------------------------------------------------
        // 3. AshBlackout overrides weather clarity
        // -------------------------------------------------------------------

        [Test]
        public void AshBlackout_OverridesWeatherClarity()
        {
            var profile = MakeProfile();
            var weather = new WeatherSystem(profile, seed: 1);
            var pp      = new PhotoperiodSystem(profile, weather);

            weather.ForceWeather(WeatherKind.Clear);
            pp.Tick(0.01f);
            Assert.That(pp.SkyClarity, Is.EqualTo(PhotoperiodSystem.ClarityForClear).Within(Eps));

            pp.ForceAshBlackout();
            Assert.That(pp.IsAshBlackout, Is.True, "Should be in blackout after ForceAshBlackout");
            Assert.That(pp.SkyClarity, Is.LessThan(0.1f),
                "Clarity should be near-zero during ash blackout");

            // After draining blackout timer it should recover
            pp.Tick(PhotoperiodSystem.AshBlackoutDurationHours + 1f);
            Assert.That(pp.IsAshBlackout, Is.False, "Blackout should end after timer expires");
            Assert.That(pp.SkyClarity, Is.EqualTo(PhotoperiodSystem.ClarityForClear).Within(Eps),
                "Clarity should return to Clear after blackout ends");
        }

        // -------------------------------------------------------------------
        // 4. LightExposure falls in dark, rises in light
        // -------------------------------------------------------------------

        [Test]
        public void LightExposure_Falls_InTotalDark_Rises_InDaylight()
        {
            var lp = MakeLightProfile();
            var sv = MakeSurvivor();
            sv.LightExposure = 60f;

            // Simulate 48 h of total darkness (effectiveDaylight = 0)
            for (int h = 0; h < 48; h++)
            {
                PhotoperiodSystem.TickSurvivorLight(sv, 1f, 0f, false, lp);
            }
            float afterDark = sv.LightExposure;
            Assert.That(afterDark, Is.LessThan(60f),
                "LightExposure should fall in darkness");

            // Then 24 h of good daylight (effectiveDaylight = 12)
            float beforeRecover = sv.LightExposure;
            for (int h = 0; h < 24; h++)
            {
                PhotoperiodSystem.TickSurvivorLight(sv, 1f, 12f, false, lp);
            }
            Assert.That(sv.LightExposure, Is.GreaterThan(beforeRecover),
                "LightExposure should recover in daylight");
        }

        // -------------------------------------------------------------------
        // 5. Listless appears after dark stretch
        // -------------------------------------------------------------------

        [Test]
        public void ListlessStatus_Appears_AfterProlangedDarkness()
        {
            var lp = MakeLightProfile();
            lp.listlessThreshold = 20f;

            var sv = MakeSurvivor();
            sv.LightExposure = 100f;
            sv.IsListless    = false;

            // Drive to 0 in pure darkness; at 3/hr: 100/3 = ~34 h
            for (int h = 0; h < 80; h++)
            {
                PhotoperiodSystem.TickSurvivorLight(sv, 1f, 0f, false, lp);
            }

            Assert.That(sv.IsListless, Is.True,
                "Survivor should be Listless after 80 h of zero effective daylight");
            Assert.That(sv.LightExposure, Is.LessThanOrEqualTo(lp.listlessThreshold),
                "LightExposure should be at or below the listless threshold");
        }

        // -------------------------------------------------------------------
        // 6. Sun-lamp session partially clears Listless
        // -------------------------------------------------------------------

        [Test]
        public void SunLampSession_BoostsLightExposure_ClearsListless()
        {
            var lp = MakeLightProfile();
            lp.listlessThreshold  = 20f;
            lp.sunLampSessionBoost = 30f;

            var sv = MakeSurvivor();
            sv.LightExposure = 5f; // well below threshold
            sv.IsListless    = true;

            PhotoperiodSystem.ApplySunLampSession(sv, lp.sunLampSessionBoost, lp);

            Assert.That(sv.LightExposure, Is.EqualTo(35f).Within(Eps),
                "LightExposure should increase by sunLampSessionBoost");
            Assert.That(sv.IsListless, Is.False,
                "Listless should be cleared when LightExposure rises above threshold");
        }

        // -------------------------------------------------------------------
        // 7. VitaminD falls slowly and applies hidden health penalty
        // -------------------------------------------------------------------

        [Test]
        public void VitaminD_FallsInDark_AppliesHiddenHealthPenalty()
        {
            var lp = MakeLightProfile();
            lp.vitaminDDecayPerHour         = 1f;   // faster for test
            lp.vitaminDLowThreshold         = 80f;  // wide window for quick trigger
            lp.vitaminDHealthPenaltyPerHour = 1f;

            var sv = MakeSurvivor();
            sv.LightExposure = 100f;
            sv.VitaminDProxy = 100f;
            sv.Needs.Health  = 100f;

            // Simulate 60 h in total darkness
            for (int h = 0; h < 60; h++)
            {
                PhotoperiodSystem.TickSurvivorLight(sv, 1f, 0f, false, lp);
            }

            Assert.That(sv.VitaminDProxy, Is.LessThan(100f),
                "VitaminD should decay in darkness");
            // VitaminD should have hit the threshold, causing hidden health drain
            Assert.That(sv.Needs.Health, Is.LessThan(100f),
                "Health should be silently drained when VitaminD is low");
        }

        // -------------------------------------------------------------------
        // 8. Grow-light grants light exposure even at midnight
        // -------------------------------------------------------------------

        [Test]
        public void GrowLight_Active_ProvidesLightExposureBoost()
        {
            var lp = MakeLightProfile();
            lp.growLightEquivalentFraction = 0.5f;

            var sv = MakeSurvivor();
            sv.LightExposure = 20f; // at threshold

            // 24 h at midnight (effectiveDaylight = 0) WITH grow-light running
            float before = sv.LightExposure;
            for (int h = 0; h < 24; h++)
            {
                PhotoperiodSystem.TickSurvivorLight(sv, 1f, 0f, true, lp);
            }

            Assert.That(sv.LightExposure, Is.GreaterThan(before),
                "Grow-light should provide light exposure even when natural daylight is zero");
        }

        // -------------------------------------------------------------------
        // 9. PhotoperiodState round-trips through GetState/RestoreState
        // -------------------------------------------------------------------

        [Test]
        public void PhotoperiodState_RoundTrips_Correctly()
        {
            var profile = MakeProfile();
            var weather = new WeatherSystem(profile, seed: 42);
            var pp      = new PhotoperiodSystem(profile, weather);

            pp.Tick(24f * 45f); // advance 45 days
            pp.ForceAshBlackout();

            var state = pp.GetState();

            // Restore into a fresh system
            var pp2 = new PhotoperiodSystem(profile, weather);
            pp2.RestoreState(state);

            Assert.That(pp2.DaylightHours,  Is.EqualTo(pp.DaylightHours).Within(Eps),
                "DaylightHours should survive state round-trip");
            Assert.That(pp2.SkyClarity,     Is.EqualTo(pp.SkyClarity).Within(Eps),
                "SkyClarity should survive state round-trip");
            Assert.That(pp2.IsAshBlackout,  Is.True,
                "IsAshBlackout should survive state round-trip");
        }
    }
}
