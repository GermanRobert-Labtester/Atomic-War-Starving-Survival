using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for the nuclear-winter temperature curve and the fallout
    /// weather state machine: curve monotonicity, storm blocking, and seeded
    /// weather determinism required for save/load reproducibility.
    /// </summary>
    [TestFixture]
    public class EnvironmentTests
    {
        private const float Eps = 1e-4f;

        private static SeasonProfile NewProfile(
            int campaignLengthDays = 10,
            float startC = 5f,
            float endC = -35f,
            float weatherCheckIntervalHours = 6f,
            SeasonWindow[] seasons = null)
        {
            var profile = ScriptableObject.CreateInstance<SeasonProfile>();
            profile.campaignLengthDays = campaignLengthDays;
            profile.ambientTemperatureCurve = AnimationCurve.Linear(0f, startC, 1f, endC);
            profile.weatherCheckIntervalHours = weatherCheckIntervalHours;
            profile.seasons = seasons ?? new[]
            {
                new SeasonWindow { id = "s1", displayName = "Season 1", startDay = 0, clearWeight = 1f, ashfallWeight = 1f, falloutStormWeight = 1f, blizzardWeight = 1f }
            };
            return profile;
        }

        private static Survivor NewSurvivor(string id = "sv_test") => new Survivor { Id = id, DisplayName = "Test Survivor" };

        // ---- Nuclear winter curve monotonicity ----

        [Test]
        public void AmbientCelsius_DriftsNonIncreasing_AsCampaignProgresses()
        {
            var profile = NewProfile(campaignLengthDays: 10, startC: 5f, endC: -35f);
            var weather = new WeatherSystem(profile, seed: 1);
            var temp = new TemperatureSystem(profile, weather);

            float previous = temp.AmbientCelsius;
            for (int day = 0; day < 10; day++)
            {
                temp.Tick(24f);
                Assert.That(temp.AmbientCelsius, Is.LessThanOrEqualTo(previous + Eps),
                    $"AmbientCelsius rose on day {day}: {previous} -> {temp.AmbientCelsius}");
                previous = temp.AmbientCelsius;
            }

            Assert.That(temp.AmbientCelsius, Is.LessThan(5f));
        }

        [Test]
        public void AmbientCelsius_ClampsAtCurveEnd_PastCampaignLength()
        {
            var profile = NewProfile(campaignLengthDays: 10, startC: 5f, endC: -35f);
            var weather = new WeatherSystem(profile, seed: 1);
            var temp = new TemperatureSystem(profile, weather);

            temp.Tick(24f * 10f); // exactly campaign end
            float atEnd = temp.AmbientCelsius;

            temp.Tick(24f * 50f); // far past campaign end
            Assert.That(temp.AmbientCelsius, Is.EqualTo(atEnd).Within(Eps));
            Assert.That(temp.AmbientCelsius, Is.EqualTo(-35f).Within(Eps));
        }

        [Test]
        public void OnSeasonChanged_FiresOnceWhenCrossingIntoNewWindow_AndNotAgainWithin()
        {
            var seasons = new[]
            {
                new SeasonWindow { id = "early", displayName = "Early", startDay = 0, clearWeight = 1f },
                new SeasonWindow { id = "late", displayName = "Late", startDay = 5, clearWeight = 1f }
            };
            var profile = NewProfile(campaignLengthDays: 10, seasons: seasons);
            var weather = new WeatherSystem(profile, seed: 1);
            var temp = new TemperatureSystem(profile, weather);

            int fireCount = 0;
            SeasonWindow lastSeason = null;
            temp.OnSeasonChanged += s => { fireCount++; lastSeason = s; };

            temp.Tick(24f * 4f); // day 4, still "early"
            Assert.That(fireCount, Is.EqualTo(0));

            temp.Tick(24f * 2f); // day 6, crosses into "late"
            Assert.That(fireCount, Is.EqualTo(1));
            Assert.That(lastSeason.id, Is.EqualTo("late"));

            temp.Tick(24f * 1f); // day 7, still "late"
            Assert.That(fireCount, Is.EqualTo(1));
        }

        [Test]
        public void SetElapsedHours_RestoresAmbientAndSeason_WithoutFiringOnSeasonChanged()
        {
            var seasons = new[]
            {
                new SeasonWindow { id = "early", displayName = "Early", startDay = 0, clearWeight = 1f },
                new SeasonWindow { id = "late", displayName = "Late", startDay = 5, clearWeight = 1f }
            };
            var profile = NewProfile(campaignLengthDays: 10, seasons: seasons);
            var weather = new WeatherSystem(profile, seed: 1);
            var temp = new TemperatureSystem(profile, weather);

            int fireCount = 0;
            temp.OnSeasonChanged += _ => fireCount++;

            temp.SetElapsedHours(24f * 6f); // jump straight into "late"

            Assert.That(fireCount, Is.EqualTo(0));
            Assert.That(temp.CurrentSeason.id, Is.EqualTo("late"));
            Assert.That(temp.AmbientCelsius, Is.EqualTo(profile.EvaluateAmbientCelsius(24f * 6f)).Within(Eps));
        }

        // ---- Legacy/manual construction stays intact for existing callers (ShelterModuleTests) ----

        [Test]
        public void ParameterlessConstructor_TickIsNoOp_SetAmbientStillWorks()
        {
            var temp = new TemperatureSystem();
            temp.SetAmbient(-30f);

            temp.Tick(100f); // should not throw, should not move AmbientCelsius

            Assert.That(temp.AmbientCelsius, Is.EqualTo(-30f).Within(Eps));
        }

        // ---- Perceived temperature: weather penalty when unsheltered ----

        [Test]
        public void GetPerceivedTemperature_Unsheltered_AppliesBlizzardPenalty()
        {
            var profile = NewProfile();
            var weather = new WeatherSystem(profile, seed: 1);
            var temp = new TemperatureSystem(profile, weather);
            weather.ForceWeather(WeatherKind.Blizzard);

            float perceived = temp.GetPerceivedTemperature(NewSurvivor());

            Assert.That(perceived, Is.EqualTo(temp.AmbientCelsius + WeatherSystem.BlizzardTemperaturePenaltyC).Within(Eps));
        }

        [Test]
        public void GetPerceivedTemperature_Unsheltered_NoPenaltyWhenClear()
        {
            var profile = NewProfile();
            var weather = new WeatherSystem(profile, seed: 1);
            var temp = new TemperatureSystem(profile, weather);

            float perceived = temp.GetPerceivedTemperature(NewSurvivor());

            Assert.That(perceived, Is.EqualTo(temp.AmbientCelsius).Within(Eps));
        }

        // ---- Storm blocking logic ----

        [Test]
        public void IsScavengingBlocked_TrueDuringFalloutStorm_WithoutFullSuit()
        {
            var weather = new WeatherSystem(NewProfile(), seed: 1);
            weather.ForceWeather(WeatherKind.FalloutStorm);

            Assert.That(weather.IsScavengingBlocked(hasFullSuit: false), Is.True);
        }

        [Test]
        public void IsScavengingBlocked_FalseDuringFalloutStorm_WithFullSuit()
        {
            var weather = new WeatherSystem(NewProfile(), seed: 1);
            weather.ForceWeather(WeatherKind.FalloutStorm);

            Assert.That(weather.IsScavengingBlocked(hasFullSuit: true), Is.False);
        }

        [Test]
        public void IsScavengingBlocked_FalseWhenNotFalloutStorm()
        {
            var weather = new WeatherSystem(NewProfile(), seed: 1);
            weather.ForceWeather(WeatherKind.Blizzard);

            Assert.That(weather.IsScavengingBlocked(hasFullSuit: false), Is.False);
        }

        [Test]
        public void VisibilityFactor_IsZeroDuringFalloutStorm_AndFullDuringClear()
        {
            var weather = new WeatherSystem(NewProfile(), seed: 1);

            weather.ForceWeather(WeatherKind.FalloutStorm);
            Assert.That(weather.VisibilityFactor, Is.EqualTo(0f).Within(Eps));

            weather.ForceWeather(WeatherKind.Clear);
            Assert.That(weather.VisibilityFactor, Is.EqualTo(1f).Within(Eps));
        }

        [Test]
        public void OutdoorRadModifier_IsAddedOnlyDuringFalloutStorm()
        {
            var weather = new WeatherSystem(NewProfile(), seed: 1);

            Assert.That(weather.OutdoorRadModifier, Is.EqualTo(0f).Within(Eps));

            weather.ForceWeather(WeatherKind.FalloutStorm);
            Assert.That(weather.OutdoorRadModifier, Is.EqualTo(WeatherSystem.FalloutStormOutdoorRadModifier).Within(Eps));
        }

        // ---- ForceWeather ----

        [Test]
        public void ForceWeather_RaisesEventOnlyWhenStateActuallyChanges()
        {
            var weather = new WeatherSystem(NewProfile(), seed: 1);
            int fireCount = 0;
            weather.OnWeatherChanged += _ => fireCount++;

            weather.ForceWeather(WeatherKind.Clear); // already Clear: no change
            Assert.That(fireCount, Is.EqualTo(0));

            weather.ForceWeather(WeatherKind.Blizzard);
            Assert.That(fireCount, Is.EqualTo(1));
            Assert.That(weather.Current, Is.EqualTo(WeatherKind.Blizzard));
        }

        // ---- Weighted random transitions: seeded determinism ----

        [Test]
        public void Tick_SameSeed_ProducesIdenticalWeatherSequence()
        {
            var weatherA = new WeatherSystem(NewProfile(), seed: 12345);
            var weatherB = new WeatherSystem(NewProfile(), seed: 12345);

            var sequenceA = new List<WeatherKind>();
            var sequenceB = new List<WeatherKind>();

            for (int i = 0; i < 30; i++)
            {
                weatherA.Tick(6f);
                weatherB.Tick(6f);
                sequenceA.Add(weatherA.Current);
                sequenceB.Add(weatherB.Current);
            }

            Assert.That(sequenceB, Is.EqualTo(sequenceA));
        }

        [Test]
        public void Tick_DifferentSeeds_CanProduceDifferentSequences()
        {
            var weatherA = new WeatherSystem(NewProfile(), seed: 1);
            var weatherB = new WeatherSystem(NewProfile(), seed: 2);

            var sequenceA = new List<WeatherKind>();
            var sequenceB = new List<WeatherKind>();

            for (int i = 0; i < 30; i++)
            {
                weatherA.Tick(6f);
                weatherB.Tick(6f);
                sequenceA.Add(weatherA.Current);
                sequenceB.Add(weatherB.Current);
            }

            Assert.That(sequenceB, Is.Not.EqualTo(sequenceA));
        }

        [Test]
        public void WeightedTransition_NeverPicksAZeroWeightState()
        {
            var seasons = new[]
            {
                new SeasonWindow { id = "no_blizzard", displayName = "No Blizzard", startDay = 0, clearWeight = 3f, ashfallWeight = 3f, falloutStormWeight = 3f, blizzardWeight = 0f }
            };
            var profile = NewProfile(campaignLengthDays: 100, seasons: seasons);
            var weather = new WeatherSystem(profile, seed: 42);

            for (int i = 0; i < 200; i++)
            {
                weather.Tick(6f);
                Assert.That(weather.Current, Is.Not.EqualTo(WeatherKind.Blizzard));
            }
        }

        // ---- Save/restore determinism ----

        [Test]
        public void RestoreState_ResumesSameDeterministicSequenceAsUninterruptedRun()
        {
            var baseline = new WeatherSystem(NewProfile(), seed: 777);
            var tailA = new List<WeatherKind>();
            for (int i = 0; i < 20; i++)
            {
                baseline.Tick(6f);
                if (i >= 10) tailA.Add(baseline.Current);
            }

            var interrupted = new WeatherSystem(NewProfile(), seed: 777);
            for (int i = 0; i < 10; i++)
            {
                interrupted.Tick(6f);
            }
            var savedState = interrupted.GetState();

            var resumed = new WeatherSystem(NewProfile(), seed: savedState.Seed);
            resumed.RestoreState(savedState);

            var tailB = new List<WeatherKind>();
            for (int i = 10; i < 20; i++)
            {
                resumed.Tick(6f);
                tailB.Add(resumed.Current);
            }

            Assert.That(tailB, Is.EqualTo(tailA));
        }

        // ---- SeasonProfile.GetSeasonForDay ----

        [Test]
        public void GetSeasonForDay_ReturnsLastWindowWithStartDayAtOrBeforeDay()
        {
            var seasons = new[]
            {
                new SeasonWindow { id = "s0", startDay = 0 },
                new SeasonWindow { id = "s10", startDay = 10 },
                new SeasonWindow { id = "s20", startDay = 20 }
            };
            var profile = NewProfile(seasons: seasons);

            Assert.That(profile.GetSeasonForDay(0).id, Is.EqualTo("s0"));
            Assert.That(profile.GetSeasonForDay(9).id, Is.EqualTo("s0"));
            Assert.That(profile.GetSeasonForDay(10).id, Is.EqualTo("s10"));
            Assert.That(profile.GetSeasonForDay(25).id, Is.EqualTo("s20"));
        }
    }
}
