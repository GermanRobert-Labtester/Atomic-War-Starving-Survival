// SPDX-License-Identifier: MIT
// World incidents (events.json): the weighted runtime picker that serves as
// the third fallback of the per-day decision stream (arc -> echo -> incident).
// These tests drive the real authored catalog through the same gates the day
// owner uses: day windows, flag/weather conditions, fail-closed unsupported
// rows, once-only resolution, scheduled follow-ups, and save round-trips.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class WorldIncidentSystemTests
    {
        private sealed class RecordingPort : IWorldIncidentConsequencePort
        {
            public readonly List<string> Log = new List<string>();
            public readonly HashSet<string> Flags = new HashSet<string>(StringComparer.Ordinal);
            public string Weather = string.Empty;

            public bool CanApplyMorale(double delta, out string reason)
            { reason = string.Empty; return true; }
            public void ApplyMorale(double delta)
            { Log.Add($"morale:{delta}"); }
            public bool CanGrantItem(string itemId, int amount, out string reason)
            { reason = string.Empty; return true; }
            public void GrantItem(string itemId, int amount)
            { Log.Add($"item:{itemId}:{amount}"); }
            public bool CanApplyNeedDelta(string needId, double delta, out string reason)
            { reason = string.Empty; return true; }
            public void ApplyNeedDelta(string needId, double delta)
            { Log.Add($"need:{needId}:{delta}"); }
            public bool CanSetWorldFlag(string flagId, bool value, out string reason)
            { reason = string.Empty; return true; }
            public void SetWorldFlag(string flagId, bool value)
            {
                if (value) Flags.Add(flagId); else Flags.Remove(flagId);
                Log.Add($"flag:{flagId}:{value}");
            }
            public bool IsWorldFlagSet(string flagId) => Flags.Contains(flagId);
            public bool WeatherIs(string weatherKindName)
                => string.Equals(Weather, weatherKindName, StringComparison.Ordinal);
            public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
            { reason = string.Empty; return true; }
            public void ApplyFactionStanding(string canonicalFactionId, int delta)
            { Log.Add($"faction:{canonicalFactionId}:{delta}"); }
        }

        private static WorldIncidentCatalogLoadResult LoadCatalog()
        {
            string path = AppContext.BaseDirectory;
            while (!string.IsNullOrEmpty(path))
            {
                string candidate = Path.Combine(path, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate))
                    return WorldIncidentCatalogLoader.LoadDetailed(
                        candidate, new FileSystemIO(), new SystemTextJsonSerializer());
                path = Directory.GetParent(path)?.FullName ?? string.Empty;
            }
            throw new InvalidOperationException("could not locate Assets/StreamingAssets/Data");
        }

        private static WorldIncidentSystem NewEngine(
            WorldIncidentCatalogLoadResult load,
            RecordingPort port,
            params string[] only)
        {
            var subset = only.Length == 0
                ? load.Incidents
                : load.Incidents.Where(i => only.Contains(i.Id)).ToList();
            return new WorldIncidentSystem(subset) { Consequences = port };
        }

        [Fact]
        public void Catalog_LoadsAllAuthoredEvents_WithZeroErrors()
        {
            var load = LoadCatalog();
            Assert.True(load.IsSuccess, string.Join("; ", load.Errors));
            Assert.Equal(1, load.SchemaVersion);
            Assert.Equal(240, load.Incidents.Count);
            Assert.All(load.Incidents, i => Assert.False(string.IsNullOrEmpty(i.Id)));
        }

        [Fact]
        public void SameSeed_ProducesIdenticalDrawSequences()
        {
            var load = LoadCatalog();
            var port = new RecordingPort();
            var first = new List<string>();
            var second = new List<string>();
            for (int day = 8; day <= 40; day++)
            {
                var a = NewEngine(load, port).SelectForDay(day, new SeededRng(450 + day));
                if (a != null) first.Add(a.Id);
                var b = NewEngine(load, port).SelectForDay(day, new SeededRng(450 + day));
                if (b != null) second.Add(b.Id);
            }
            Assert.NotEmpty(first);
            Assert.Equal(first, second);
        }

        [Fact]
        public void MinDayWindow_GatesEligibility()
        {
            var load = LoadCatalog();
            var engine = NewEngine(load, new RecordingPort());
            var dryPipes = engine.Find("water_shortage");
            Assert.NotNull(dryPipes);
            Assert.False(engine.IsEligible(dryPipes!, 3));
            Assert.True(engine.IsEligible(dryPipes!, 4));
        }

        [Fact]
        public void WeatherAndFlagConditions_FailClosedUntilSatisfied()
        {
            var load = LoadCatalog();
            var port = new RecordingPort();
            var engine = NewEngine(load, port);

            // weather gate
            var contamination = engine.Find("weather_black_rain_contamination");
            Assert.NotNull(contamination);
            Assert.False(engine.IsEligible(contamination!, 4));
            port.Weather = "BlackRain";
            Assert.True(engine.IsEligible(contamination!, 4));

            // world-flag gate
            var wakes = engine.Find("silent_knock_part2a_wakes");
            Assert.NotNull(wakes);
            Assert.False(engine.IsEligible(wakes!, 37));
            port.Flags.Add("stranger_inside");
            Assert.True(engine.IsEligible(wakes!, 37));
        }

        [Fact]
        public void UnsupportedAuthoredConditions_StayIneligible()
        {
            var load = LoadCatalog();
            var engine = NewEngine(
                load, new RecordingPort(), "apprenticeship_completion_rough_repairs");
            var apprenticeship = engine.Find("apprenticeship_completion_rough_repairs");
            Assert.NotNull(apprenticeship);
            Assert.False(engine.IsEligible(apprenticeship!, 100));
            Assert.Null(engine.SelectForDay(100, new SeededRng(21)));
        }

        [Fact]
        public void InformationalIncident_AutoResolvesAndNeverBlocksThePendingSlot()
        {
            var load = LoadCatalog();
            var engine = NewEngine(load, new RecordingPort(), "fallout_storm");
            var surfaced = engine.SelectForDay(1, new SeededRng(31));
            Assert.NotNull(surfaced);
            Assert.Equal("fallout_storm", surfaced!.Id);
            Assert.False(engine.HasPendingIncident);
            Assert.True(engine.IsResolved("fallout_storm"));
        }

        [Fact]
        public void PendingDecision_ResolvesThroughPort_ExactlyOnce()
        {
            var load = LoadCatalog();
            var port = new RecordingPort { Weather = "BlackRain" };
            var engine = NewEngine(load, port, "weather_black_rain_contamination");
            var pending = engine.SelectForDay(4, new SeededRng(41));
            Assert.NotNull(pending);
            Assert.True(engine.HasPendingIncident);

            var result = engine.Resolve(pending!.Id, "pour_out", 4);
            Assert.Equal(WorldIncidentResolutionStatus.Committed, result.Status);
            Assert.Contains("morale:-5", port.Log);
            Assert.Contains("item:dirty_water:-5", port.Log);
            Assert.False(engine.HasPendingIncident);
            Assert.True(engine.IsResolved(pending.Id));

            var again = engine.Resolve(pending.Id, "boil_it", 5);
            Assert.Equal(WorldIncidentResolutionStatus.AlreadyResolved, again.Status);
        }

        [Fact]
        public void ScheduledFollowUp_FiresOnItsDueDay_ExactlyOnce()
        {
            var load = LoadCatalog();
            var port = new RecordingPort { Weather = "FalloutStorm" };
            var engine = NewEngine(
                load, port, "silent_knock_part1", "silent_knock_part2a_wakes");

            var knock = engine.SelectForDay(35, new SeededRng(51));
            Assert.NotNull(knock);
            Assert.Equal("silent_knock_part1", knock!.Id);

            var opened = engine.Resolve(knock.Id, "open_hatch", 35);
            Assert.Equal(WorldIncidentResolutionStatus.Committed, opened.Status);
            Assert.Contains("need:radiation:12", port.Log);
            Assert.Contains("flag:stranger_inside:True", port.Log);

            // The weight-0 follow-up is schedule-only: it cannot fire early.
            Assert.Null(engine.SelectForDay(36, new SeededRng(52)));
            var followUp = engine.SelectForDay(37, new SeededRng(53));
            Assert.NotNull(followUp);
            Assert.Equal("silent_knock_part2a_wakes", followUp!.Id);
        }

        [Fact]
        public void CaptureRestore_RoundTrip_PreservesPendingResolvedAndScheduledState()
        {
            var load = LoadCatalog();
            var port = new RecordingPort { Weather = "FalloutStorm" };
            var engine = NewEngine(
                load, port, "silent_knock_part1", "silent_knock_part2a_wakes");
            var knock = engine.SelectForDay(35, new SeededRng(61));
            Assert.NotNull(knock);
            engine.Resolve(knock!.Id, "open_hatch", 35);

            var restored = NewEngine(
                load, new RecordingPort { Weather = "FalloutStorm" },
                "silent_knock_part1", "silent_knock_part2a_wakes");
            restored.RestoreState(engine.CaptureState());

            Assert.True(restored.IsResolved("silent_knock_part1"));
            Assert.Single(restored.State.Scheduled);
            Assert.Equal("silent_knock_part2a_wakes", restored.State.Scheduled[0].EventId);
            Assert.Equal(37, restored.State.Scheduled[0].DueDay);

            var replay = restored.Resolve("silent_knock_part1", "ignore_hatch", 36);
            Assert.Equal(WorldIncidentResolutionStatus.AlreadyResolved, replay.Status);
        }

        [Fact]
        public void TypedEffectWithoutBoundOwner_IsNonExecutableAtLoad()
        {
            var load = LoadCatalog();
            var trade = load.Incidents.FirstOrDefault(i => i.Id == "contaminated_water_trade");
            Assert.NotNull(trade);
            var tradeWater = trade!.Choices.FirstOrDefault(c => c.ChoiceId == "trade_water");
            Assert.NotNull(tradeWater);
            Assert.False(tradeWater!.IsExecutable);
            Assert.Contains("has no bound consequence owner", tradeWater.ValidationError);
        }

        [Fact]
        public void WeekOne_HasDecisionCapableIncidents()
        {
            var load = LoadCatalog();
            var decisions = load.Incidents
                .Where(i => i.MinDay <= 7 && i.HasExecutableChoices)
                .ToList();
            Assert.True(decisions.Count >= 5,
                $"only {decisions.Count} decision-capable incidents in days 1-7");
        }
    }
}
