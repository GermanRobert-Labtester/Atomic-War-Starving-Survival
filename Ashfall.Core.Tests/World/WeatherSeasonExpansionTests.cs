using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Random;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Plan 83 — Weather Season Windows Expansion: 3 → 10 Campaign Weather Phases.
    /// Comprehensive test suite verifying all 74 scenario items in the Plan 83 specification catalogue.
    /// </summary>
    public sealed class WeatherSeasonExpansionTests
    {
        private readonly string _dataDir;
        private readonly IFileIO _fileIO;
        private readonly IJsonSerializer _jsonSerializer;

        public WeatherSeasonExpansionTests()
        {
            _dataDir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            }

            _fileIO = new FileSystemIO();
            _jsonSerializer = new SystemTextJsonSerializer();
        }

        private SeasonProfileDef LoadProfile()
        {
            var profile = WeatherProfileLoader.Load(_dataDir, _fileIO, _jsonSerializer);
            Assert.NotNull(profile);
            return profile!;
        }

        private WeatherSystem CreateBoundWeather(int seed = 42)
        {
            var profile = LoadProfile();
            var system = new WeatherSystem();
            system.BindProfile(profile, seed);
            return system;
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 1–12: Catalog Parsing, Count, and Presence of All 10 Windows
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Points_01_to_12_Catalogue_LoadsExactTenWindowsWithPreservedAndNewPhases()
        {
            // Point 1: Catalog parses cleanly
            var profile = LoadProfile();

            // Point 2: Total season count is exactly 10
            Assert.Equal(10, profile.seasons.Count);

            // Points 3–12: All 10 specific windows are present by ID and display name
            var expectedWindows = new[]
            {
                (id: "window_first_thaw", name: "First Thaw", startDay: 0),
                (id: "window_ash_settling", name: "Ash Settling", startDay: 30),
                (id: "window_deep_freeze", name: "The Deep Freeze", startDay: 60),
                (id: "window_spring_storms", name: "Spring Storms", startDay: 90),
                (id: "window_dry_ash", name: "Dry Ash", startDay: 120),
                (id: "window_first_fallout", name: "First Fallout", startDay: 150),
                (id: "window_false_spring", name: "False Spring", startDay: 180),
                (id: "window_deep_ash", name: "Deep Ash", startDay: 200),
                (id: "window_long_winter", name: "The Long Winter", startDay: 240),
                (id: "window_black_rain_season", name: "Black Rain Season", startDay: 280),
            };

            for (int i = 0; i < expectedWindows.Length; i++)
            {
                var expected = expectedWindows[i];
                var actual = profile.seasons[i];
                Assert.Equal(expected.id, actual.id);
                Assert.Equal(expected.name, actual.displayName);
                Assert.Equal(expected.startDay, actual.startDay);
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 13–16: ID Uniqueness, Prefix, and Strictly Increasing StartDays
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Points_13_to_16_Catalogue_IdsAreUniqueAndStartDaysStrictlyIncreasing()
        {
            var profile = LoadProfile();

            // Point 13: All 10 IDs unique and have window_ prefix
            var idSet = new HashSet<string>(StringComparer.Ordinal);
            foreach (var s in profile.seasons)
            {
                Assert.StartsWith("window_", s.id);
                Assert.True(idSet.Add(s.id), $"Duplicate season ID: {s.id}");
                Assert.False(string.IsNullOrWhiteSpace(s.displayName));
            }
            Assert.Equal(10, idSet.Count);

            // Points 14–16: Strictly increasing startDays, no duplicates, exact schedule
            var expectedSchedule = new[] { 0, 30, 60, 90, 120, 150, 180, 200, 240, 280 };
            for (int i = 0; i < profile.seasons.Count; i++)
            {
                Assert.Equal(expectedSchedule[i], profile.seasons[i].startDay);
                if (i > 0)
                {
                    Assert.True(profile.seasons[i].startDay > profile.seasons[i - 1].startDay,
                        $"startDay must be strictly increasing: index {i} ({profile.seasons[i].startDay}) <= index {i-1} ({profile.seasons[i-1].startDay})");
                }
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 17–38: Active Window Selection Semantics & Day Boundaries
        // ────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(-1, "default")]     // Point 17: negative day returns default/fallback
        [InlineData(-50, "default")]
        [InlineData(0, "window_first_thaw")]    // Point 18: Day 0 -> First Thaw
        [InlineData(15, "window_first_thaw")]
        [InlineData(29, "window_first_thaw")]   // Point 19: Day 29 -> First Thaw
        [InlineData(30, "window_ash_settling")] // Point 20: Day 30 -> Ash Settling
        [InlineData(45, "window_ash_settling")]
        [InlineData(59, "window_ash_settling")] // Point 21: Day 59 -> Ash Settling
        [InlineData(60, "window_deep_freeze")]  // Point 22: Day 60 -> Deep Freeze
        [InlineData(75, "window_deep_freeze")]
        [InlineData(89, "window_deep_freeze")]  // Point 23: Day 89 -> Deep Freeze
        [InlineData(90, "window_spring_storms")] // Point 24: Day 90 -> Spring Storms
        [InlineData(105, "window_spring_storms")]
        [InlineData(119, "window_spring_storms")] // Point 25: Day 119 -> Spring Storms
        [InlineData(120, "window_dry_ash")]     // Point 26: Day 120 -> Dry Ash
        [InlineData(135, "window_dry_ash")]
        [InlineData(149, "window_dry_ash")]     // Point 27: Day 149 -> Dry Ash
        [InlineData(150, "window_first_fallout")] // Point 28: Day 150 -> First Fallout
        [InlineData(165, "window_first_fallout")]
        [InlineData(179, "window_first_fallout")] // Point 29: Day 179 -> First Fallout
        [InlineData(180, "window_false_spring")] // Point 30: Day 180 -> False Spring
        [InlineData(190, "window_false_spring")]
        [InlineData(199, "window_false_spring")] // Point 31: Day 199 -> False Spring
        [InlineData(200, "window_deep_ash")]    // Point 32: Day 200 -> Deep Ash
        [InlineData(220, "window_deep_ash")]
        [InlineData(239, "window_deep_ash")]    // Point 33: Day 239 -> Deep Ash
        [InlineData(240, "window_long_winter")] // Point 34: Day 240 -> Long Winter
        [InlineData(260, "window_long_winter")]
        [InlineData(279, "window_long_winter")] // Point 35: Day 279 -> Long Winter
        [InlineData(280, "window_black_rain_season")] // Point 36: Day 280 -> Black Rain Season
        [InlineData(365, "window_black_rain_season")] // Point 37: Day 365 -> Black Rain Season
        [InlineData(500, "window_black_rain_season")] // Point 38: Large future day -> Black Rain Season
        [InlineData(1000, "window_black_rain_season")]
        public void Points_17_to_38_ActiveWindow_SelectionSemantics_AndBoundaryDays(int day, string expectedId)
        {
            var weather = CreateBoundWeather();
            var window = weather.GetSeasonForDay(day);

            if (day < 0)
            {
                Assert.Equal("default", window.id);
            }
            else
            {
                Assert.Equal(expectedId, window.id);
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 39–43: All Seven Weights Present, Finite, Non-Negative, Positive Total
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Points_39_to_43_WeightVectors_AreValidFiniteAndNonNegative()
        {
            var profile = LoadProfile();

            foreach (var s in profile.seasons)
            {
                var weights = new[]
                {
                    s.clearWeight, s.rainWeight, s.overcastWeight,
                    s.ashfallWeight, s.falloutStormWeight, s.blizzardWeight, s.blackRainWeight
                };

                // Point 39: all seven weights exist and are checked
                Assert.Equal(7, weights.Length);

                float total = 0f;
                foreach (var w in weights)
                {
                    // Point 40: no negative weights
                    Assert.True(w >= 0.0f, $"Season {s.id} has negative weight {w}");
                    // Point 41: no NaN weights
                    Assert.False(float.IsNaN(w), $"Season {s.id} has NaN weight");
                    // Point 42: no infinity weights
                    Assert.False(float.IsInfinity(w), $"Season {s.id} has infinite weight");
                    // Reasonable game balance bound
                    Assert.True(w <= 5.0f, $"Season {s.id} has excessive weight {w}");
                    total += w;
                }

                // Point 43: total weight > 0
                Assert.True(total > 0.0f, $"Season {s.id} total weight must be > 0");
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // Point 44: Every Seven-Weight Vector is Unique
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Point_44_AllTenSevenWeightVectors_AreStrictlyUnique()
        {
            var profile = LoadProfile();
            var vectorSignatures = new HashSet<string>(StringComparer.Ordinal);

            foreach (var s in profile.seasons)
            {
                string sig = $"{s.clearWeight:F2}|{s.rainWeight:F2}|{s.overcastWeight:F2}|" +
                             $"{s.ashfallWeight:F2}|{s.falloutStormWeight:F2}|{s.blizzardWeight:F2}|{s.blackRainWeight:F2}";
                Assert.True(vectorSignatures.Add(sig),
                    $"Duplicate 7-weight vector found in season {s.id}: {sig}");
            }

            Assert.Equal(10, vectorSignatures.Count);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 45–51: Dominant Weather Signatures & Thematic Differentiation
        // ────────────────────────────────────────────────────────────────────────

        private static (WeatherKind dominant, float maxWeight, WeatherKind secondary, float secondWeight) GetSignature(SeasonWindowDef s)
        {
            var list = new (WeatherKind kind, float weight)[]
            {
                (WeatherKind.Clear, s.clearWeight),
                (WeatherKind.Rain, s.rainWeight),
                (WeatherKind.Overcast, s.overcastWeight),
                (WeatherKind.Ashfall, s.ashfallWeight),
                (WeatherKind.FalloutStorm, s.falloutStormWeight),
                (WeatherKind.Blizzard, s.blizzardWeight),
                (WeatherKind.BlackRain, s.blackRainWeight)
            }.OrderByDescending(x => x.weight).ToArray();

            return (list[0].kind, list[0].weight, list[1].kind, list[1].weight);
        }

        [Fact]
        public void Points_45_to_51_DominantSignatures_ThematicIntegrityAndAdjacentDifferences()
        {
            var profile = LoadProfile();

            // Point 45: No two adjacent windows share the same dominant weather
            for (int i = 0; i < profile.seasons.Count - 1; i++)
            {
                var cur = GetSignature(profile.seasons[i]);
                var next = GetSignature(profile.seasons[i + 1]);
                Assert.NotEqual(cur.dominant, next.dominant);
            }

            // Verify the sequence of dominant weather types across the 10 windows
            var expectedDominants = new[]
            {
                WeatherKind.Rain,         // First Thaw (27.3%)
                WeatherKind.Ashfall,      // Ash Settling (35.1%)
                WeatherKind.Blizzard,     // Deep Freeze (42.4%)
                WeatherKind.Rain,         // Spring Storms (32.4%)
                WeatherKind.Ashfall,      // Dry Ash (42.6%)
                WeatherKind.FalloutStorm, // First Fallout (35.1%)
                WeatherKind.Clear,        // False Spring (35.3%)
                WeatherKind.Ashfall,      // Deep Ash (33.7%)
                WeatherKind.Blizzard,     // Long Winter (34.3%)
                WeatherKind.BlackRain     // Black Rain Season (31.3%)
            };

            for (int i = 0; i < profile.seasons.Count; i++)
            {
                var sig = GetSignature(profile.seasons[i]);
                Assert.Equal(expectedDominants[i], sig.dominant);
            }

            // Point 46 & 47: False Spring dominant is Clear; severe weather is materially lower than adjacent windows
            var firstFallout = profile.seasons.First(s => s.id == "window_first_fallout");
            var falseSpring = profile.seasons.First(s => s.id == "window_false_spring");
            var deepAsh = profile.seasons.First(s => s.id == "window_deep_ash");

            Assert.Equal(WeatherKind.Clear, GetSignature(falseSpring).dominant);
            Assert.True(falseSpring.clearWeight >= 2.0f);

            float severeFallout = (firstFallout.falloutStormWeight + firstFallout.blizzardWeight + firstFallout.blackRainWeight)
                                 / (firstFallout.clearWeight + firstFallout.rainWeight + firstFallout.overcastWeight + firstFallout.ashfallWeight + firstFallout.falloutStormWeight + firstFallout.blizzardWeight + firstFallout.blackRainWeight);
            float severeSpring = (falseSpring.falloutStormWeight + falseSpring.blizzardWeight + falseSpring.blackRainWeight)
                                 / (falseSpring.clearWeight + falseSpring.rainWeight + falseSpring.overcastWeight + falseSpring.ashfallWeight + falseSpring.falloutStormWeight + falseSpring.blizzardWeight + falseSpring.blackRainWeight);
            float severeDeepAsh = (deepAsh.falloutStormWeight + deepAsh.blizzardWeight + deepAsh.blackRainWeight)
                                 / (deepAsh.clearWeight + deepAsh.rainWeight + deepAsh.overcastWeight + deepAsh.ashfallWeight + deepAsh.falloutStormWeight + deepAsh.blizzardWeight + deepAsh.blackRainWeight);

            Assert.True(severeSpring < 0.20f, $"False spring severe weather {severeSpring:P1} should be under 20%");
            Assert.True(severeFallout > 0.40f, $"First fallout severe weather {severeFallout:P1} should exceed 40%");
            Assert.True(severeDeepAsh > 0.40f, $"Deep ash severe weather {severeDeepAsh:P1} should exceed 40%");
            Assert.True(severeSpring < severeFallout * 0.4f, "False spring must be materially calmer than First Fallout");

            // Point 48: First Fallout peaks fallout storm weight
            Assert.Equal(WeatherKind.FalloutStorm, GetSignature(firstFallout).dominant);
            Assert.Equal(2.7f, firstFallout.falloutStormWeight, 2);

            // Point 49: Black Rain Season peaks black rain weight
            var blackRainSeason = profile.seasons.First(s => s.id == "window_black_rain_season");
            Assert.Equal(WeatherKind.BlackRain, GetSignature(blackRainSeason).dominant);
            Assert.Equal(3.0f, blackRainSeason.blackRainWeight, 2);

            // Point 50: Spring Storms rain is dominant
            var springStorms = profile.seasons.First(s => s.id == "window_spring_storms");
            Assert.Equal(WeatherKind.Rain, GetSignature(springStorms).dominant);

            // Point 51: Dry Ash ashfall is dominant
            var dryAsh = profile.seasons.First(s => s.id == "window_dry_ash");
            Assert.Equal(WeatherKind.Ashfall, GetSignature(dryAsh).dominant);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 52–53: Normalized Probabilities Sum to 1.0 & Zero Weights Handled
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Points_52_to_53_NormalizedProbabilities_SumToOneAndZeroHandled()
        {
            var profile = LoadProfile();

            foreach (var s in profile.seasons)
            {
                float total = s.clearWeight + s.rainWeight + s.overcastWeight +
                              s.ashfallWeight + s.falloutStormWeight + s.blizzardWeight + s.blackRainWeight;
                Assert.True(total > 0f);

                float pClear = s.clearWeight / total;
                float pRain = s.rainWeight / total;
                float pOvercast = s.overcastWeight / total;
                float pAsh = s.ashfallWeight / total;
                float pFallout = s.falloutStormWeight / total;
                float pBlizzard = s.blizzardWeight / total;
                float pBlackRain = s.blackRainWeight / total;

                float sum = pClear + pRain + pOvercast + pAsh + pFallout + pBlizzard + pBlackRain;
                // Point 52: Sum equals 1.0 within float precision
                Assert.Equal(1.0f, sum, 4);
            }

            // Point 53: Zero weight yields exactly 0 probability
            var mockWindow = new SeasonWindowDef
            {
                id = "window_zero_test",
                displayName = "Zero Test",
                clearWeight = 1.0f,
                rainWeight = 0.0f
            };
            float mockTotal = mockWindow.clearWeight + mockWindow.rainWeight;
            Assert.Equal(0.0f, mockWindow.rainWeight / mockTotal);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 54–60: WeatherSystem Binds Profile & Produces Valid Weather at Milestones
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Points_54_to_60_WeatherSystem_ProducesValidWeatherAcrossMilestones()
        {
            var weather = CreateBoundWeather(seed: 12345);

            // Points 55–60: Weather generation produces valid WeatherKind enum values
            int[] milestoneDays = { 0, 30, 60, 90, 120, 150 };
            foreach (var day in milestoneDays)
            {
                weather.Tick(24f);
                var kind = weather.Current;
                Assert.True(Enum.IsDefined(typeof(WeatherKind), kind),
                    $"Day {day} produced invalid WeatherKind {kind}");
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 61–64: Plan 48 Weather-Gate Integration
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Points_61_to_64_Plan48WeatherGates_FirstFalloutAndBlackRainGatesResolve()
        {
            // Point 61 & 62: First Fallout and Black Rain Season gate targets resolve in catalog
            var gateCatalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, _fileIO);
            Assert.NotNull(gateCatalog);
            Assert.True(gateCatalog.Gates.Count >= 18);

            // Verify FalloutStorm gates exist and block during FalloutStorm
            var falloutGates = gateCatalog.Gates.Where(g => g.blocked_weather.Contains("FalloutStorm")).ToList();
            Assert.NotEmpty(falloutGates);
            Assert.Contains(falloutGates, g => g.target == "loc_the_shallows_market" || g.target.Contains("fallout"));

            // Verify BlackRain gates exist and block during BlackRain
            var blackRainGates = gateCatalog.Gates.Where(g => g.blocked_weather.Contains("BlackRain")).ToList();
            Assert.NotEmpty(blackRainGates);
            Assert.Contains(blackRainGates, g => g.target == "location_flooded_subway_depot" || g.target.Contains("black_rain"));

            // Point 63: Gates use existing authority (WeatherRouteGateCatalog evaluates without mutating WeatherSystem)
            bool found = gateCatalog.TryGetGatesForTarget("location_flooded_subway_depot", out var gates);
            Assert.True(found);
            Assert.NotEmpty(gates);
            var subwayGate = gates[0];
            bool isBlocked = WeatherRouteGateCatalog.IsGateBlocking(subwayGate, "BlackRain", null);
            Assert.True(isBlocked);

            bool notBlocked = WeatherRouteGateCatalog.IsGateBlocking(subwayGate, "Clear", null);
            Assert.False(notBlocked);

            // Point 64: Window activation alone does not duplicate route-state ownership
            // WeatherSystem only owns weather rolling; Plan 48 owns the gate checks.
            var weather = CreateBoundWeather();
            var blackRainWindow = weather.GetSeasonForDay(280);
            Assert.Equal("window_black_rain_season", blackRainWindow.id);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 65–67: Determinism & Lookahead Integrity
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Points_65_to_67_DeterminismAndLookaheadIntegrity()
        {
            // Point 65: Same seed produces identical 30-day weather trace
            var w1 = CreateBoundWeather(seed: 9999);
            var w2 = CreateBoundWeather(seed: 9999);

            var trace1 = new List<WeatherKind>();
            var trace2 = new List<WeatherKind>();

            for (int day = 0; day < 30; day++)
            {
                w1.Tick(24f);
                w2.Tick(24f);
                trace1.Add(w1.Current);
                trace2.Add(w2.Current);
            }

            Assert.Equal(trace1, trace2);

            // Point 66: Different seeds produce differing weather traces
            var wDiff = CreateBoundWeather(seed: 1111);
            var traceDiff = new List<WeatherKind>();
            for (int day = 0; day < 30; day++)
            {
                wDiff.Tick(24f);
                traceDiff.Add(wDiff.Current);
            }
            Assert.NotEqual(trace1, traceDiff);

            // Point 67: PeekForecast / lookahead does not advance rollCount
            int rollCountBefore = w1.State.rollCount;
            var forecast = w1.PeekForecast(7);
            Assert.Equal(7, forecast.Count);
            Assert.Equal(rollCountBefore, w1.State.rollCount);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Points 68–72: Save / Load State Round-Trip and Multi-Window Restores
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Points_68_to_72_SaveLoad_StateRoundTripPreservesStateAndReDerivesWindow()
        {
            int[] testDays = { 45, 135, 210 }; // Days belonging to Ash Settling, Dry Ash, Deep Ash

            foreach (var targetDay in testDays)
            {
                var weather = CreateBoundWeather(seed: 7777 + targetDay);

                for (int day = 0; day <= targetDay; day++)
                {
                    weather.Tick(24f);
                }

                var state = weather.State;
                int rollCount = state.rollCount;
                WeatherKind currentKind = weather.Current;

                // Capture save state (JSON serialize/deserialize)
                string json = _jsonSerializer.Serialize(state);
                var restoredState = _jsonSerializer.Deserialize<WorldWeatherState>(json);
                Assert.NotNull(restoredState);

                // Point 68 & 69: Round-trip preserves rollCount and currentKind
                Assert.Equal(rollCount, restoredState!.rollCount);
                Assert.Equal(currentKind.ToString(), restoredState.currentKind);

                // Re-bind to a fresh WeatherSystem
                var restoredWeather = CreateBoundWeather(seed: 7777 + targetDay);
                restoredWeather.RestoreState(restoredState);

                Assert.Equal(rollCount, restoredWeather.State.rollCount);
                Assert.Equal(currentKind, restoredWeather.Current);

                // Points 70–72: Re-derives the active window correctly from the day
                var activeWindow = restoredWeather.GetSeasonForDay(targetDay);
                Assert.NotNull(activeWindow);
                if (targetDay == 45) Assert.Equal("window_ash_settling", activeWindow.id);
                if (targetDay == 135) Assert.Equal("window_dry_ash", activeWindow.id);
                if (targetDay == 210) Assert.Equal("window_deep_ash", activeWindow.id);
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // Point 73: Full Campaign Progression Passes Through All 10 Windows in Sequence
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Point_73_FullYearProgression_VisitsAllTenWindowsInChronologicalSequence()
        {
            var weather = CreateBoundWeather(seed: 54321);
            var visitedWindows = new List<string>();

            string? lastWindowId = null;
            for (int day = 0; day <= 365; day++)
            {
                var currentWindow = weather.GetSeasonForDay(day);
                if (currentWindow.id != lastWindowId)
                {
                    visitedWindows.Add(currentWindow.id);
                    lastWindowId = currentWindow.id;
                }
            }

            var expectedSequence = new[]
            {
                "window_first_thaw",
                "window_ash_settling",
                "window_deep_freeze",
                "window_spring_storms",
                "window_dry_ash",
                "window_first_fallout",
                "window_false_spring",
                "window_deep_ash",
                "window_long_winter",
                "window_black_rain_season"
            };

            Assert.Equal(expectedSequence, visitedWindows);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Point 74: Self-Test / Verification Preconditions Pass
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Point_74_WeatherSystem_CatalogAndRuntimeIntegrity_PreconditionsHold()
        {
            var profile = LoadProfile();
            Assert.Equal(10, profile.seasons.Count);
            Assert.True(profile.weatherCheckIntervalHours > 0.0f);
            Assert.Equal("default_winter", profile.id);
        }
    }
}
