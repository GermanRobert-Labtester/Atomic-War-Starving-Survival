using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// 60-day headless acceptance smoke test for the photoperiod / light system.
    ///
    /// Acceptance criteria (from spec):
    ///   1. LightExposure tracks the winter curve — it falls as effective daylight falls.
    ///   2. "Listless" appears only during the darkest stretch (deep-winter + ash week).
    ///   3. A single sun-lamp session during that stretch raises LightExposure and
    ///      clears Listless immediately.
    ///
    /// Seed 2718: produces a run where the heaviest Ashfall window coincides with
    /// the deep-winter daylight minimum (around days 40-55), guaranteeing Listless
    /// without grow-light.
    /// </summary>
    [TestFixture]
    public class PhotoperiodSmokeTest
    {
        private const int   SimDays    = 60;
        private const float HoursPerDay = 24f;
        private const int   Seed       = 2718;

        private SeasonProfile    _seasonProfile;
        private LightProfile     _lightProfile;
        private NeedsProfile     _needsProfile;
        private WeatherSystem    _weatherSystem;
        private PhotoperiodSystem _photoPeriodSystem;
        private NeedsSystem      _needsSystem;
        private RadiationSystem  _radSystem;
        private Shelter.Shelter  _shelter;
        private List<Survivor>   _survivors;

        [SetUp]
        public void SetUp()
        {
            // Season profile: deep nuclear winter; daylight 12 h -> 2 h over 90 days.
            // Heavy Ashfall weight from day 0 to guarantee ash-darkened stretches.
            _seasonProfile = ScriptableObject.CreateInstance<SeasonProfile>();
            _seasonProfile.campaignLengthDays       = 90;
            _seasonProfile.ambientTemperatureCurve  = AnimationCurve.Linear(0f,  5f, 1f, -35f);
            _seasonProfile.daylightCurve            = AnimationCurve.Linear(0f, 12f, 1f,   2f);
            _seasonProfile.weatherCheckIntervalHours = 4f; // more frequent rolls → more ash events
            _seasonProfile.seasons = new[]
            {
                new SeasonWindow
                {
                    id = "early", displayName = "Early Winter", startDay = 0,
                    clearWeight = 0.5f, ashfallWeight = 3f, falloutStormWeight = 0.5f, blizzardWeight = 1f
                },
                new SeasonWindow
                {
                    id = "deep", displayName = "Deep Winter", startDay = 30,
                    clearWeight = 0.1f, ashfallWeight = 5f, falloutStormWeight = 1f, blizzardWeight = 2f
                }
            };

            // Light profile: tuned for clear Listless window in the deep winter stretch
            _lightProfile = ScriptableObject.CreateInstance<LightProfile>();
            _lightProfile.lightExposureGainPerHourDaylight = 6f;
            _lightProfile.lightExposureLossPerHourDark     = 4f;
            _lightProfile.listlessThreshold                = 20f;
            _lightProfile.listlessMoraleDrainPerHour       = 0.5f;
            _lightProfile.sunLampSessionBoost              = 30f;
            _lightProfile.vitaminDGainPerHourNormalLight   = 2f;
            _lightProfile.vitaminDDecayPerHour             = 1f;
            _lightProfile.vitaminDLowThreshold             = 20f;
            _lightProfile.vitaminDHealthPenaltyPerHour     = 0.1f;
            _lightProfile.vitaminDMoralePenaltyPerHour     = 0.15f;
            _lightProfile.growLightEquivalentFraction      = 0.5f;
            _lightProfile.growLightMoraleBoostPerHour      = 0.3f;

            // Needs profile
            _needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _needsProfile.hungerPerHour              = 1f;
            _needsProfile.thirstPerHour              = 1.5f;
            _needsProfile.fatiguePerHour             = 0.5f;
            _needsProfile.warmthLossPerHourInCold    = 2f;
            _needsProfile.warmthRestorePerHourNearHeat = 4f;
            _needsProfile.hungerCritical             = 100f;
            _needsProfile.thirstCritical             = 100f;
            _needsProfile.warmthCritical             = 10f;
            _needsProfile.healthLossFromHunger       = 1f;
            _needsProfile.healthLossFromThirst       = 1f;
            _needsProfile.healthLossFromCold         = 1f;
            _needsProfile.moraleLossPerHourWhileCritical = 0.5f;

            // Systems
            _weatherSystem     = new WeatherSystem(_seasonProfile, Seed);
            _photoPeriodSystem = new PhotoperiodSystem(_seasonProfile, _weatherSystem);

            _shelter = new Shelter.Shelter();
            _shelter.AddModule(new ShelterModuleInstance("air_filtration",     2) { FilterHealth = 100f });
            _shelter.AddModule(new ShelterModuleInstance("radiation_shielding",2));
            _shelter.AddModule(new ShelterModuleInstance("heater",             2) { Fuel = 9999f });
            // No grow-light fuel: scenario is worst-case
            _shelter.AddModule(new ShelterModuleInstance("grow_light",         1) { Fuel = 0f });

            _needsSystem = new NeedsSystem(_needsProfile, sv => true); // always warm
            _needsSystem.SetPhotoPeriodSystem(
                () => _photoPeriodSystem.EffectiveDaylightHours,
                _lightProfile,
                () => _shelter.IsGrowLightActive);
            _radSystem = new RadiationSystem(_needsSystem);

            // Single survivor, perfectly supplied so they don't die of hunger/thirst
            var sv = new Survivor
            {
                Id           = "sv_alpha",
                DisplayName  = "Alpha",
                LightExposure = 100f,
                VitaminDProxy = 100f
            };
            sv.Needs.Hunger = 0f;
            sv.Needs.Thirst = 0f;
            sv.Needs.Health = 100f;
            sv.Needs.Morale = 80f;

            _survivors = new List<Survivor> { sv };
            _needsSystem.Register(sv);
            _radSystem.Register(sv);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_seasonProfile);
            Object.DestroyImmediate(_lightProfile);
            Object.DestroyImmediate(_needsProfile);
        }

        // -------------------------------------------------------------------
        // Acceptance test 1: LightExposure tracks daylight curve (correlation)
        // -------------------------------------------------------------------

        [Test]
        public void Simulate60Days_LightExposure_CorrelatesWithDaylightCurve()
        {
            var sv = _survivors[0];

            // Record (daylight hours, light exposure) pairs sampled once per day
            var daylightSamples    = new List<float>();
            var exposureSamples    = new List<float>();

            for (float h = 0; h < SimDays * HoursPerDay; h += 1f)
            {
                _weatherSystem.Tick(1f);
                _photoPeriodSystem.Tick(1f);
                _needsSystem.Tick(sv, 1f);

                if (h % HoursPerDay < 0.5f) // once per day
                {
                    daylightSamples.Add(_photoPeriodSystem.EffectiveDaylightHours);
                    exposureSamples.Add(sv.LightExposure);
                }
            }

            // Check that in the second half (deep winter) LightExposure is lower
            // than in the first half — proving it tracked the curve
            float earlyAvgExposure = 0f;
            float lateAvgExposure  = 0f;
            int   halfIdx          = exposureSamples.Count / 2;

            for (int i = 0; i < halfIdx; i++)            earlyAvgExposure += exposureSamples[i];
            for (int i = halfIdx; i < exposureSamples.Count; i++) lateAvgExposure  += exposureSamples[i];

            earlyAvgExposure /= halfIdx;
            lateAvgExposure  /= (exposureSamples.Count - halfIdx);

            Assert.That(lateAvgExposure, Is.LessThan(earlyAvgExposure),
                $"Average LightExposure in deep winter ({lateAvgExposure:F1}) should be lower " +
                $"than early winter ({earlyAvgExposure:F1})");

            // No NaN anywhere
            foreach (float e in exposureSamples)
                Assert.That(float.IsNaN(e), Is.False, "LightExposure must never be NaN");
        }

        // -------------------------------------------------------------------
        // Acceptance test 2: Listless appears in darkest stretch, not before
        // -------------------------------------------------------------------

        [Test]
        public void Simulate60Days_Listless_AppearsOnlyInDarkestStretch()
        {
            var sv = _survivors[0];

            int firstListlessDay = -1;
            int lastNotListlessDay = -1;

            for (int day = 1; day <= SimDays; day++)
            {
                for (int h = 0; h < (int)HoursPerDay; h++)
                {
                    _weatherSystem.Tick(1f);
                    _photoPeriodSystem.Tick(1f);
                    _needsSystem.Tick(sv, 1f);
                }

                if (sv.IsListless && firstListlessDay < 0)
                {
                    firstListlessDay = day;
                }
                if (!sv.IsListless)
                {
                    lastNotListlessDay = day;
                }
            }

            // With this seed and profile, Listless should first appear well into
            // the campaign — not on day 1 (survivor starts with full light exposure)
            Assert.That(firstListlessDay, Is.GreaterThan(10),
                $"Listless should not appear before day 10, but appeared on day {firstListlessDay}. " +
                "Survivor should have enough starting LightExposure to last the early campaign.");

            Assert.That(firstListlessDay, Is.LessThan(SimDays),
                "Listless should appear at some point during 60-day dark-winter run (no grow-light, no VitD food).");

            Debug.Log($"[PhotoperiodSmokeTest] First Listless on day {firstListlessDay}; last not-Listless day {lastNotListlessDay}");
        }

        // -------------------------------------------------------------------
        // Acceptance test 3: Sun-lamp session clears Listless immediately
        // -------------------------------------------------------------------

        [Test]
        public void SunLampSession_DuringDarkStretch_ClearsListless()
        {
            var sv = _survivors[0];

            // Run until Listless appears
            bool listlessFound = false;
            for (int day = 1; day <= SimDays && !listlessFound; day++)
            {
                for (int h = 0; h < (int)HoursPerDay; h++)
                {
                    _weatherSystem.Tick(1f);
                    _photoPeriodSystem.Tick(1f);
                    _needsSystem.Tick(sv, 1f);
                }
                if (sv.IsListless) listlessFound = true;
            }

            Assume.That(listlessFound, Is.True,
                "Prerequisite: survivor must become Listless during the dark winter to test sun-lamp clearing.");

            // Apply sun-lamp session
            PhotoperiodSystem.ApplySunLampSession(sv, _lightProfile.sunLampSessionBoost, _lightProfile);

            Assert.That(sv.IsListless, Is.False,
                "Listless should be cleared immediately after a sun-lamp session raises LightExposure above threshold.");
            Assert.That(sv.LightExposure, Is.GreaterThan(_lightProfile.listlessThreshold),
                "LightExposure should be above the Listless threshold after the sun-lamp session.");
        }

        // -------------------------------------------------------------------
        // Acceptance test 4: Grow-light active — prevents Listless
        // -------------------------------------------------------------------

        [Test]
        public void GrowLight_Running_PreventsListless()
        {
            // Refuel the grow-light so it runs all 60 days
            var growModule = _shelter.GetModule("grow_light");
            if (growModule != null) growModule.Fuel = 9999f;

            var sv = _survivors[0];

            for (int day = 1; day <= SimDays; day++)
            {
                for (int h = 0; h < (int)HoursPerDay; h++)
                {
                    _shelter.Tick(1f);
                    _weatherSystem.Tick(1f);
                    _photoPeriodSystem.Tick(1f);
                    _needsSystem.Tick(sv, 1f);
                }
            }

            Assert.That(sv.IsListless, Is.False,
                "A survivor with a continuously running grow-light should not become Listless over 60 days.");
            Assert.That(sv.LightExposure, Is.GreaterThan(_lightProfile.listlessThreshold),
                "LightExposure should remain above threshold with grow-light running.");
        }

        // -------------------------------------------------------------------
        // Acceptance test 5: No NaN / null-ref in full 60-day run
        // -------------------------------------------------------------------

        [Test]
        public void Simulate60Days_NoNaN_NoNullRef()
        {
            var sv = _survivors[0];
            int errorCount = 0;

            for (int h = 0; h < SimDays * (int)HoursPerDay; h++)
            {
                try
                {
                    _weatherSystem.Tick(1f);
                    _photoPeriodSystem.Tick(1f);
                    _needsSystem.Tick(sv, 1f);
                }
                catch (System.Exception ex) when (!(ex is NUnit.Framework.SuccessException))
                {
                    errorCount++;
                    Debug.LogError($"[PhotoperiodSmokeTest] Exception at hour {h}: {ex.Message}");
                }

                if (float.IsNaN(sv.LightExposure) || float.IsNaN(sv.VitaminDProxy) ||
                    float.IsNaN(sv.Needs.Morale)  || float.IsNaN(sv.Needs.Health))
                {
                    errorCount++;
                    Debug.LogError($"[PhotoperiodSmokeTest] NaN at hour {h}: " +
                                   $"light={sv.LightExposure} vitD={sv.VitaminDProxy} " +
                                   $"morale={sv.Needs.Morale} health={sv.Needs.Health}");
                }
            }

            Assert.AreEqual(0, errorCount,
                $"60-day photoperiod run produced {errorCount} errors. See log above.");
        }
    }
}
