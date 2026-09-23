// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan54SevenDaySliceIntegrationTests
    {
        private static SliceScenarioData LoadAuthoredScenarioData()
        {
            string catalogPath = Path.Combine(AppContext.BaseDirectory, "Data", "slice_seven_days.json");
            if (!File.Exists(catalogPath))
            {
                catalogPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "slice_seven_days.json"));
            }
            if (!File.Exists(catalogPath))
            {
                catalogPath = Path.Combine("Assets", "StreamingAssets", "Data", "slice_seven_days.json");
            }

            string json = File.ReadAllText(catalogPath);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var data = new SliceScenarioData
            {
                SchemaVersion = root.GetProperty("schema_version").GetInt32(),
                ScenarioId = root.GetProperty("scenario_id").GetString() ?? string.Empty,
                Title = root.GetProperty("title").GetString() ?? string.Empty,
                Description = root.GetProperty("description").GetString() ?? string.Empty,
                TargetDays = root.GetProperty("target_days").GetInt32(),
                DefaultSeed = root.GetProperty("default_seed").GetInt32(),
                DifficultyPreset = root.GetProperty("difficulty_preset").GetString() ?? "normal",
                InitialSurvivorCount = root.GetProperty("initial_survivor_count").GetInt32()
            };

            if (root.TryGetProperty("starting_resources", out var resElem))
            {
                foreach (var prop in resElem.EnumerateObject())
                {
                    data.StartingResources[prop.Name] = prop.Value.GetInt32();
                }
            }

            if (root.TryGetProperty("beats", out var beatsElem))
            {
                foreach (var b in beatsElem.EnumerateArray())
                {
                    data.Beats.Add(new SliceBeatDef
                    {
                        Day = b.GetProperty("day").GetInt32(),
                        BeatId = b.GetProperty("beat_id").GetString() ?? string.Empty,
                        Title = b.GetProperty("title").GetString() ?? string.Empty,
                        RequiredSystem = b.GetProperty("required_system").GetString() ?? string.Empty,
                        ActionKey = b.GetProperty("action_key").GetString() ?? string.Empty,
                        ExpectedOutcome = b.GetProperty("expected_outcome").GetString() ?? string.Empty,
                        Description = b.GetProperty("description").GetString() ?? string.Empty
                    });
                }
            }

            return data;
        }

        [Fact]
        public void CatalogData_DeclaresSevenSequentialBeatsAndInitialConditions()
        {
            var data = LoadAuthoredScenarioData();

            Assert.Equal(1, data.SchemaVersion);
            Assert.Equal("slice_seven_days_v1", data.ScenarioId);
            Assert.Equal(7, data.TargetDays);
            Assert.Equal(1337, data.DefaultSeed);
            Assert.Equal("normal", data.DifficultyPreset);
            Assert.Equal(4, data.InitialSurvivorCount);

            Assert.True(data.StartingResources.ContainsKey("food"));
            Assert.True(data.StartingResources.ContainsKey("water"));
            Assert.True(data.StartingResources.ContainsKey("chits"));

            Assert.Equal(7, data.Beats.Count);
            for (int day = 1; day <= 7; day++)
            {
                Assert.Equal(day, data.Beats[day - 1].Day);
                Assert.False(string.IsNullOrWhiteSpace(data.Beats[day - 1].BeatId));
                Assert.False(string.IsNullOrWhiteSpace(data.Beats[day - 1].RequiredSystem));
                Assert.False(string.IsNullOrWhiteSpace(data.Beats[day - 1].ActionKey));
                Assert.False(string.IsNullOrWhiteSpace(data.Beats[day - 1].ExpectedOutcome));
            }
        }

        [Fact]
        public void SliceScenario_ComputeScenarioHash_ProducesDeterministicFingerprint()
        {
            var data = LoadAuthoredScenarioData();
            var scenario = new SliceScenario(data);

            string hash1 = scenario.ComputeScenarioHash();
            string hash2 = scenario.ComputeScenarioHash();

            Assert.False(string.IsNullOrEmpty(hash1));
            Assert.Equal(hash1, hash2);
            Assert.True(scenario.IsScenarioFrozen(hash1));
            Assert.False(scenario.IsScenarioFrozen("tampered_hash_12345"));
        }

        [Fact]
        public void SliceScenario_EvaluateDayBeat_TracksSequentialProgressionAndMismatches()
        {
            var data = LoadAuthoredScenarioData();
            var scenario = new SliceScenario(data);

            // Day 1: matching action and outcome
            bool day1Success = scenario.EvaluateDayBeat(1, "ration_distribution", "NeedsEvaluated", out var r1);
            Assert.True(day1Success);
            Assert.True(r1.Success);
            Assert.Equal("beat_day1_orient_and_ration", r1.BeatId);

            // Day 2: mismatched action key
            bool day2Success = scenario.EvaluateDayBeat(2, "wrong_action", "PartyDispatched", out var r2);
            Assert.False(day2Success);
            Assert.False(r2.Success);
            Assert.Contains("Mismatch", r2.Notes);

            // Day 99: non-existent beat
            bool day99Success = scenario.EvaluateDayBeat(99, "any", "any", out var r99);
            Assert.False(day99Success);
            Assert.False(r99.Success);

            Assert.Equal(3, scenario.EvaluatedResults.Count);
        }

        [Fact]
        public void SliceScenario_CompleteSlice_GeneratesComprehensiveScorecard()
        {
            var data = LoadAuthoredScenarioData();
            var scenario = new SliceScenario(data);

            // Simulate 7 successful beats
            scenario.EvaluateDayBeat(1, "ration_distribution", "NeedsEvaluated", out _);
            scenario.EvaluateDayBeat(2, "scouting_dispatch", "PartyDispatched", out _);
            scenario.EvaluateDayBeat(3, "route_divergence", "HazardMitigated", out _);
            scenario.EvaluateDayBeat(4, "radiation_treatment", "TriageApplied", out _);
            scenario.EvaluateDayBeat(5, "memorial_ceremony", "EulogyDelivered", out _);
            scenario.EvaluateDayBeat(6, "policy_ratification", "PolicyEnacted", out _);
            scenario.EvaluateDayBeat(7, "campaign_settlement", "Week1Certified", out _);

            var scorecard = scenario.CompleteSlice(retainedSurvivors: 3);

            Assert.Equal("slice_seven_days_v1", scorecard.ScenarioId);
            Assert.Equal(7, scorecard.CompletedDays);
            Assert.Equal(7, scorecard.TotalBeats);
            Assert.Equal(7, scorecard.CompletedBeats);
            Assert.Equal(4, scorecard.InitialSurvivors);
            Assert.Equal(3, scorecard.RetainedSurvivors);
            Assert.True(scorecard.Passed);
            Assert.False(string.IsNullOrEmpty(scorecard.ScenarioContentHash));
        }

        [Fact]
        public void SliceScenario_SeamDelegates_TriggerOnEvaluationAndCompletion()
        {
            var data = LoadAuthoredScenarioData();
            var scenario = new SliceScenario(data);

            SliceBeatResult? evaluatedBeat = null;
            SliceScorecard? completedScorecard = null;
            string? failedBeatId = null;

            scenario.OnBeatEvaluatedSeam = b => evaluatedBeat = b;
            scenario.OnSliceCompletedSeam = sc => completedScorecard = sc;
            scenario.OnBeatValidationFailedSeam = (id, reason) => failedBeatId = id;

            scenario.EvaluateDayBeat(1, "wrong", "wrong", out _);
            Assert.NotNull(evaluatedBeat);
            Assert.Equal("beat_day1_orient_and_ration", failedBeatId);

            var scorecard = scenario.CompleteSlice(retainedSurvivors: 4);
            Assert.NotNull(completedScorecard);
            Assert.Equal(scorecard, completedScorecard);
        }
    }
}
