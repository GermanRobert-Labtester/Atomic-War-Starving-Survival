using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests for the audio event integration layer.
    /// Verifies that Core events fire correctly for radiation thresholds,
    /// weather hazard transitions, and radio broadcast deduplication.
    /// These events drive the Godot-side AudioManager calls.
    /// </summary>
    public class AudioEventIntegrationTests
    {
        // ── Radiation: OnStatusGained edge detection ──────────────

        [Fact]
        public void Radiation_OnStatusGained_FiresOnce_OnAcuteThresholdCrossing()
        {
            var system = new RadiationSystem(
                exposureContext: _ => new ExposureContext { ZoneRadLevel = 0 },
                applyNeed: (_, _, _) => { });

            var survivor = new TestRadState { Id = "test_survivor" };
            system.Register(survivor);

            int alertCount = 0;
            SurvivorStatus? firedStatus = null;
            system.OnStatusGained += (state, status) =>
            {
                alertCount++;
                firedStatus = status;
            };

            // Below threshold — no alert
            system.Expose(survivor, 50f, 1f); // dose = 50, below 80
            Assert.Equal(0, alertCount);

            // Cross threshold — exactly one alert
            system.Expose(survivor, 40f, 1f); // dose = 90, above 80
            Assert.Equal(1, alertCount);
            Assert.Equal(SurvivorStatus.AcuteRadiationSickness, firedStatus);

            // Continue above threshold — no additional alerts
            system.Expose(survivor, 5f, 1f); // dose = 95
            Assert.Equal(1, alertCount);
        }

        [Fact]
        public void Radiation_OnStatusGained_DoesNotFire_BelowThreshold()
        {
            var system = new RadiationSystem(
                exposureContext: _ => new ExposureContext { ZoneRadLevel = 0 },
                applyNeed: (_, _, _) => { });

            var survivor = new TestRadState { Id = "test_safe" };
            system.Register(survivor);

            int alertCount = 0;
            system.OnStatusGained += (_, _) => alertCount++;

            // Stay below threshold
            system.Expose(survivor, 30f, 1f); // dose = 30
            system.Expose(survivor, 30f, 1f); // dose = 60
            system.Expose(survivor, 10f, 1f); // dose = 70, still below 80

            Assert.Equal(0, alertCount);
        }

        [Fact]
        public void Radiation_OnStatusGained_FiresForChronicIllness()
        {
            var system = new RadiationSystem(
                exposureContext: _ => new ExposureContext { ZoneRadLevel = 0 },
                applyNeed: (_, _, _) => { });

            var survivor = new TestRadState { Id = "test_chronic" };
            system.Register(survivor);

            var firedStatuses = new List<SurvivorStatus>();
            system.OnStatusGained += (_, status) => firedStatuses.Add(status);

            // Push lifetime exposure above 400
            system.SeedLifetimeExposure(survivor, 450f);

            Assert.Contains(SurvivorStatus.ChronicIllness, firedStatuses);
        }

        [Fact]
        public void Radiation_AntiRadBelowThreshold_DoesNotTriggerAlert()
        {
            var system = new RadiationSystem(
                exposureContext: _ => new ExposureContext { ZoneRadLevel = 0 },
                applyNeed: (_, _, _) => { });

            var survivor = new TestRadState { Id = "test_antirad" };
            system.Register(survivor);

            int alertCount = 0;
            system.OnStatusGained += (_, _) => alertCount++;

            // Expose then reduce — never crosses 80
            system.Expose(survivor, 60f, 1f); // dose = 60
            system.AdministerAntiRad(survivor, 20f); // dose = 40
            system.Expose(survivor, 30f, 1f); // dose = 70

            Assert.Equal(0, alertCount);
        }

        // ── Weather: OnWeatherChanged hazard transition ───────────

        [Fact]
        public void Weather_OnWeatherChanged_FiresOnKindTransition()
        {
            var profile = new SeasonProfileDef
            {
                id = "test",
                weatherCheckIntervalHours = 6f,
                seasons = new List<SeasonWindowDef>
                {
                    new SeasonWindowDef
                    {
                        id = "s1", startDay = 0,
                        clearWeight = 0, rainWeight = 0, overcastWeight = 0,
                        ashfallWeight = 0, falloutStormWeight = 1,
                        blizzardWeight = 0, blackRainWeight = 0
                    }
                }
            };

            var system = new WeatherSystem();
            system.BindProfile(profile, seed: 42);

            var transitions = new List<WeatherKind>();
            system.OnWeatherChanged += kind => transitions.Add(kind);

            // Tick past the first check interval
            system.Tick(6f);

            Assert.NotEmpty(transitions);
            Assert.Equal(WeatherKind.FalloutStorm, transitions[0]);
        }

        [Fact]
        public void Weather_OnWeatherChanged_DoesNotFire_OnSameKind()
        {
            var profile = new SeasonProfileDef
            {
                id = "test",
                weatherCheckIntervalHours = 6f,
                seasons = new List<SeasonWindowDef>
                {
                    new SeasonWindowDef
                    {
                        id = "s1", startDay = 0,
                        clearWeight = 1, rainWeight = 0, overcastWeight = 0,
                        ashfallWeight = 0, falloutStormWeight = 0,
                        blizzardWeight = 0, blackRainWeight = 0
                    }
                }
            };

            var system = new WeatherSystem();
            system.BindProfile(profile, seed: 42);

            int changeCount = 0;
            system.OnWeatherChanged += _ => changeCount++;

            // Tick multiple intervals — should stay Clear, fire at most once
            system.Tick(6f);
            system.Tick(6f);
            system.Tick(6f);

            // First tick may fire if initial state differs; subsequent should not
            Assert.True(changeCount <= 1);
        }

        [Fact]
        public void Weather_ForceWeather_FiresOnHazardTransition()
        {
            var system = new WeatherSystem();
            var transitions = new List<WeatherKind>();
            system.OnWeatherChanged += kind => transitions.Add(kind);

            system.ForceWeather(WeatherKind.BlackRain);

            Assert.Contains(WeatherKind.BlackRain, transitions);
        }

        [Fact]
        public void Weather_Determinism_SameSeedSameHazardSequence()
        {
            WeatherKind[] RunSequence()
            {
                var profile = new SeasonProfileDef
                {
                    id = "test",
                    weatherCheckIntervalHours = 6f,
                    seasons = new List<SeasonWindowDef>
                    {
                        new SeasonWindowDef
                        {
                            id = "s1", startDay = 0,
                            clearWeight = 1, rainWeight = 1, overcastWeight = 1,
                            ashfallWeight = 1, falloutStormWeight = 1,
                            blizzardWeight = 1, blackRainWeight = 1
                        }
                    }
                };
                var system = new WeatherSystem();
                system.BindProfile(profile, seed: 99);
                var results = new List<WeatherKind>();
                system.OnWeatherChanged += kind => results.Add(kind);
                system.Tick(48f); // 8 checks
                return results.ToArray();
            }

            var seq1 = RunSequence();
            var seq2 = RunSequence();

            Assert.Equal(seq1.Length, seq2.Length);
            for (int i = 0; i < seq1.Length; i++)
                Assert.Equal(seq1[i], seq2[i]);
        }

        // ── Radio: Broadcast dedup key stability ──────────────────

        [Fact]
        public void Radio_BroadcastKey_IsStableForSameIntercept()
        {
            var a = new RadioIntercept("faction_a", "CALLSIGN", 100.0f, RadioEventKind.InterceptChatter, "test message", 7, 5);
            var b = new RadioIntercept("faction_a", "CALLSIGN", 100.0f, RadioEventKind.InterceptChatter, "test message", 7, 5);

            Assert.Equal(MakeKey(a), MakeKey(b));
        }

        [Fact]
        public void Radio_BroadcastKey_DiffersByDay()
        {
            var day5 = new RadioIntercept("faction_a", "CALL", 100.0f, RadioEventKind.InterceptChatter, "msg", 7, 5);
            var day6 = new RadioIntercept("faction_a", "CALL", 100.0f, RadioEventKind.InterceptChatter, "msg", 7, 6);

            Assert.NotEqual(MakeKey(day5), MakeKey(day6));
        }

        [Fact]
        public void Radio_BroadcastKey_DiffersByFrequency()
        {
            var freq100 = new RadioIntercept("faction_a", "CALL", 100.0f, RadioEventKind.InterceptChatter, "msg", 7, 5);
            var freq101 = new RadioIntercept("faction_a", "CALL", 101.0f, RadioEventKind.InterceptChatter, "msg", 7, 5);

            Assert.NotEqual(MakeKey(freq100), MakeKey(freq101));
        }

        [Fact]
        public void Radio_BroadcastKey_DiffersByMessage()
        {
            var msg1 = new RadioIntercept("faction_a", "CALL", 100.0f, RadioEventKind.InterceptChatter, "alpha", 7, 5);
            var msg2 = new RadioIntercept("faction_a", "CALL", 100.0f, RadioEventKind.InterceptChatter, "bravo", 7, 5);

            Assert.NotEqual(MakeKey(msg1), MakeKey(msg2));
        }

        [Fact]
        public void Radio_HashSet_DedupPreventsReplay()
        {
            var intercept = new RadioIntercept("faction_a", "CALL", 100.0f, RadioEventKind.InterceptChatter, "msg", 7, 5);
            var played = new HashSet<string>();

            // First play — added
            Assert.True(played.Add(MakeKey(intercept)));
            // Second play — duplicate, not added
            Assert.False(played.Add(MakeKey(intercept)));
            // Third play — still duplicate
            Assert.False(played.Add(MakeKey(intercept)));

            Assert.Single(played);
        }

        [Fact]
        public void Radio_DeadAir_HasEmptyFactionId()
        {
            var deadAir = new RadioIntercept("", "", 100.0f, RadioEventKind.Silence, "static", 1, 5);
            Assert.True(string.IsNullOrWhiteSpace(deadAir.FactionId));
        }

        // ── Helpers ───────────────────────────────────────────────

        private sealed class TestRadState : SurvivorRadState { }

        private static string MakeKey(RadioIntercept intercept)
        {
            return $"{intercept.Day}:{intercept.FrequencyMhz:F2}:{(intercept.Message?.GetHashCode() ?? 0)}";
        }
    }
}
