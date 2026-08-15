using System.IO;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WeatherSystemTests
    {
        private static SeasonProfileDef TestProfile()
        {
            return new SeasonProfileDef
            {
                id = "test",
                weatherCheckIntervalHours = 6f,
                seasons = new System.Collections.Generic.List<SeasonWindowDef>
                {
                    new SeasonWindowDef
                    {
                        id = "s1", startDay = 0,
                        clearWeight = 1f, rainWeight = 1f, overcastWeight = 1f,
                        ashfallWeight = 1f, falloutStormWeight = 1f,
                        blizzardWeight = 1f, blackRainWeight = 1f
                    },
                    new SeasonWindowDef
                    {
                        id = "s2", startDay = 100,
                        clearWeight = 10f, rainWeight = 0f, overcastWeight = 0f,
                        ashfallWeight = 0f, falloutStormWeight = 0f,
                        blizzardWeight = 0f, blackRainWeight = 0f
                    }
                }
            };
        }

        private static WeatherSystem NewSystem(int seed = 42)
        {
            var sys = new WeatherSystem();
            sys.BindProfile(TestProfile(), seed);
            return sys;
        }

        [Fact]
        public void Tick_TransitionsWeatherOverTime()
        {
            var sys = NewSystem();
            int changes = 0;
            sys.OnWeatherChanged += k => changes++;
            // 6h interval: 48h -> up to 8 rolls.
            sys.Tick(48f);
            Assert.True(changes >= 1, "weather should change at least once over 48h");
            Assert.True(sys.State.rollCount >= 1);
        }

        [Fact]
        public void Tick_NoProfileNoAdvance()
        {
            var sys = new WeatherSystem();
            sys.Tick(48f);
            Assert.Equal(0f, sys.State.totalElapsedHours);
            Assert.Equal(0, sys.State.rollCount);
        }

        [Fact]
        public void Determinism_SameSeedSameSequence()
        {
            var a = NewSystem(7);
            var b = NewSystem(7);
            var seqA = new System.Collections.Generic.List<string>();
            var seqB = new System.Collections.Generic.List<string>();
            for (int i = 0; i < 5; i++)
            {
                a.Tick(6f);
                b.Tick(6f);
                seqA.Add(a.Current.ToString());
                seqB.Add(b.Current.ToString());
            }
            Assert.Equal(string.Join(",", seqA), string.Join(",", seqB));

            // Different seed diverges (usually).
            var c = NewSystem(8);
            var seqC = new System.Collections.Generic.List<string>();
            for (int i = 0; i < 5; i++)
            {
                c.Tick(6f);
                seqC.Add(c.Current.ToString());
            }
            Assert.NotEqual(string.Join(",", seqA), string.Join(",", seqC));
        }

        [Fact]
        public void SaveLoad_ResumesIdenticalSequence()
        {
            var sys = NewSystem(11);
            for (int i = 0; i < 3; i++) sys.Tick(6f);
            string snapshot = sys.Current.ToString();

            var restored = new WeatherSystem();
            restored.BindProfile(TestProfile(), 11);
            restored.RestoreState(sys.CaptureState());

            Assert.Equal(snapshot, restored.Current.ToString());
            // Same future rolls.
            var contA = new System.Collections.Generic.List<string>();
            var contB = new System.Collections.Generic.List<string>();
            for (int i = 0; i < 3; i++)
            {
                sys.Tick(6f);
                restored.Tick(6f);
                contA.Add(sys.Current.ToString());
                contB.Add(restored.Current.ToString());
            }
            Assert.Equal(string.Join(",", contA), string.Join(",", contB));
        }

        [Fact]
        public void RestrictToNonHazard_ExcludesStorms()
        {
            var profile = TestProfile();
            profile.seasons[0].falloutStormWeight = 100f;
            profile.seasons[0].blizzardWeight = 100f;
            profile.seasons[0].blackRainWeight = 100f;
            var sys = new WeatherSystem();
            sys.BindProfile(profile, 5);
            sys.RestrictToNonHazardWeather(true);
            for (int i = 0; i < 10; i++) sys.Tick(6f);
            Assert.NotEqual(WeatherKind.FalloutStorm, sys.Current);
            Assert.NotEqual(WeatherKind.Blizzard, sys.Current);
            Assert.NotEqual(WeatherKind.BlackRain, sys.Current);
        }

        [Fact]
        public void Modifiers_UnityParity()
        {
            var sys = NewSystem();
            sys.ForceWeather(WeatherKind.FalloutStorm);
            Assert.Equal(0f, sys.VisibilityFactor);
            Assert.Equal(WeatherSystem.FalloutStormOutdoorRadModifier, sys.OutdoorRadModifier);
            Assert.True(sys.IsScavengingBlocked(false));
            Assert.False(sys.IsScavengingBlocked(true));
            Assert.Equal(1f, sys.HazmatDegradeMultiplier);

            sys.ForceWeather(WeatherKind.BlackRain);
            Assert.Equal(WeatherSystem.BlackRainHazmatMeltMultiplier, sys.HazmatDegradeMultiplier);
            Assert.Equal(WeatherSystem.BlackRainOutdoorRadModifier, sys.OutdoorRadModifier);

            sys.ForceWeather(WeatherKind.Blizzard);
            Assert.Equal(WeatherSystem.BlizzardVisibilityFactor, sys.VisibilityFactor);
            Assert.Equal(0f, sys.OutdoorRadModifier);

            sys.ForceWeather(WeatherKind.Clear);
            Assert.Equal(1f, sys.VisibilityFactor);
            Assert.Equal(0f, sys.OutdoorRadModifier);

            Assert.Equal(-15f, WeatherSystem.TemperaturePenaltyForWeather(WeatherKind.Blizzard));
            Assert.Equal(-5f, WeatherSystem.TemperaturePenaltyForWeather(WeatherKind.FalloutStorm));
            Assert.Equal(-8f, WeatherSystem.TemperaturePenaltyForWeather(WeatherKind.BlackRain));
            Assert.Equal(0f, WeatherSystem.TemperaturePenaltyForWeather(WeatherKind.Clear));
        }

        [Fact]
        public void ForceWeather_RaisesChangedEvent()
        {
            var sys = NewSystem();
            int changed = 0;
            sys.OnWeatherChanged += k => changed++;
            sys.ForceWeather(WeatherKind.Rain);
            Assert.Equal(1, changed);
            Assert.Equal(WeatherKind.Rain, sys.Current);
            sys.ForceWeather(WeatherKind.Rain); // same kind: no event
            Assert.Equal(1, changed);
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = NewSystem();
            sys.ForceWeather(WeatherKind.Overcast);
            var snapshot = sys.CaptureState();
            snapshot.currentKind = "Blizzard";
            Assert.Equal(WeatherKind.Overcast, sys.Current);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = NewSystem(3);
            sys.Tick(18f);
            sys.ForceWeather(WeatherKind.Rain);
            string before = SaveChecksum.Compute(sys.CaptureState());
            var restored = new WeatherSystem();
            restored.BindProfile(TestProfile(), 3);
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());
            Assert.Equal(before, after);
        }

        // ── Data catalog ───────────────────────────────────────────────

        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        [Fact]
        public void Catalog_LoadsProfileWithBoundSeasons()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var profile = WeatherProfileLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(profile);
            Assert.True(profile.seasons.Count >= 2);
            foreach (var s in profile.seasons)
            {
                Assert.False(string.IsNullOrEmpty(s.id));
                Assert.True(s.clearWeight + s.rainWeight + s.overcastWeight +
                            s.ashfallWeight + s.falloutStormWeight +
                            s.blizzardWeight + s.blackRainWeight > 0f,
                    s.id + " weights unbound");
            }
        }

        [Fact]
        public void Catalog_MissingFileReturnsNull()
        {
            Assert.Null(WeatherProfileLoader.Load(
                "/nonexistent", new FileSystemIO(), new SystemTextJsonSerializer()));
        }
    }
}
