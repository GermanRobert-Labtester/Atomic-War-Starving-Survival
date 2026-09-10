using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class NarrativeArcEventSystemTests
    {
        [Fact]
        public void CurrentCatalog_ParsesThePlan143InventoryAndWhitelist()
        {
            var load = LoadCurrent();

            Assert.True(load.IsSuccess, string.Join(" | ", load.Errors));
            Assert.Equal(1, load.SchemaVersion);
            Assert.Equal(15, load.Events.Count);
            Assert.Equal(18, load.Events.Sum(e => e.Choices.Count));
            Assert.Equal(17, load.Events.SelectMany(e => e.Choices).SelectMany(c => c.Effects).Count());
            Assert.Equal(
                new[]
                {
                    NarrativeArcEffectKind.AdvanceNarrativeArc,
                    NarrativeArcEffectKind.NarrativeArcBranch,
                    NarrativeArcEffectKind.GainFactionIntel,
                    NarrativeArcEffectKind.StartExpedition,
                    NarrativeArcEffectKind.FactionStanding
                },
                load.Events.SelectMany(e => e.Choices).SelectMany(c => c.Effects)
                    .Select(e => e.Kind).Distinct().OrderBy(k => k));

            Assert.Equal(
                new[] { "aris_thorne", "elena_rostov", "maya_lin", "victor_vance" },
                load.Events.Where(e => e.Stage == 1).Select(e => e.RequiredSurvivorId).OrderBy(id => id));
            Assert.Contains(load.Events, e => e.Id == "narrative_garrison_defector_intel");
            Assert.Contains(load.Events, e => e.Id == "narrative_cult_prophet_rumor");
            Assert.Contains(load.Events, e => e.Id == "narrative_militia_council_invitation");
        }

        [Fact]
        public void CharacterArc_EnforcesStageOrderAndCommitsBranchOnce()
        {
            var events = LoadCurrent().Events.Where(e => e.ArcId == "aris_thorne").ToList();
            var port = new RecordingPort();
            var system = NewSystem(events, port);

            Assert.Equal("narrative_aris_thorne_stage_1", system.SelectForDay(15, new SeededRng(7))?.Id);
            Assert.Equal(NarrativeArcChoiceStatus.Committed,
                system.CommitChoice("narrative_aris_thorne_stage_1", "investigate_crack", 15).Status);
            Assert.Null(system.SelectForDay(17, new SeededRng(7)));
            Assert.Equal("narrative_aris_thorne_stage_2", system.SelectForDay(18, new SeededRng(7))?.Id);

            var branch = system.CommitChoice("narrative_aris_thorne_stage_2", "force_rest_branch_a", 18);
            Assert.Equal(NarrativeArcChoiceStatus.Committed, branch.Status);
            Assert.Equal(5, port.MoraleCalls);
            Assert.Equal("a", system.State.arcs.Single().branchId);
            Assert.True(system.State.arcs.Single().branchCommitted);
            Assert.Equal("narrative_aris_thorne_stage_3", system.SelectForDay(25, new SeededRng(7))?.Id);
            Assert.Equal(NarrativeArcChoiceStatus.Committed,
                system.AcknowledgeEvent("narrative_aris_thorne_stage_3", 25).Status);
            Assert.True(system.State.arcs.Single().complete);
            Assert.Null(system.SelectForDay(26, new SeededRng(7)));

            var duplicate = system.CommitChoice("narrative_aris_thorne_stage_2", "force_rest_branch_a", 18);
            Assert.Equal(NarrativeArcChoiceStatus.AlreadyCommitted, duplicate.Status);
            Assert.Equal(5, port.MoraleCalls);
        }

        [Fact]
        public void Selection_IsDeterministicAndCatalogOrderIndependent()
        {
            var all = LoadCurrent().Events;
            var first = NewSystem(all, new RecordingPort());
            var second = NewSystem(all.OrderByDescending(e => e.Id), new RecordingPort());

            var selectedA = first.SelectForDay(20, new SeededRng(12345));
            var selectedB = second.SelectForDay(20, new SeededRng(12345));

            Assert.Equal(selectedA?.Id, selectedB?.Id);
            Assert.Equal(selectedA?.Id, first.State.pendingEventId);
            Assert.Equal(selectedB?.Id, second.State.pendingEventId);
        }

        [Fact]
        public void DailySelectionSimulation_RemainsStableThroughDayForty()
        {
            var all = LoadCurrent().Events;
            var first = RunDailySimulation(all);
            var second = RunDailySimulation(all.OrderByDescending(e => e.Id));

            Assert.Equal(first, second);
            Assert.All(first, id => Assert.DoesNotContain(id, first.Take(first.IndexOf(id))));
        }

        [Fact]
        public void MissingResident_BlocksCharacterArcButLeavesIndependentEventEligible()
        {
            var all = LoadCurrent().Events;
            var system = NewSystem(all, new RecordingPort());
            system.SurvivorIsPresent = id => id != "aris_thorne";

            var candidates = system.GetEligibleCandidates(15).Select(x => x.Event.Id).ToList();
            Assert.DoesNotContain("narrative_aris_thorne_stage_1", candidates);
            Assert.Contains("narrative_militia_council_invitation", candidates);
        }

        [Fact]
        public void PreflightFailure_AppliesNoEarlierConsequence()
        {
            var eventDef = LoadCurrent().Events.Single(e => e.Id == "narrative_militia_council_invitation");
            var port = new RecordingPort { StandingAllowed = false };
            var system = NewSystem(new[] { eventDef }, port);

            Assert.NotNull(system.SelectForDay(15, new SeededRng(4)));
            var result = system.CommitChoice("narrative_militia_council_invitation", "attend_council", 15);

            Assert.Equal(NarrativeArcChoiceStatus.Rejected, result.Status);
            Assert.Contains("standing", result.Reason, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(0, port.MoraleCalls);
            Assert.Equal(0, port.StandingCalls);
            Assert.Empty(system.State.completedEventIds);
        }

        [Fact]
        public void ExpeditionPreflightFailure_DoesNotForceDispatchOrCompleteChoice()
        {
            var eventDef = LoadCurrent().Events.Single(e => e.Id == "narrative_cult_prophet_rumor");
            var port = new RecordingPort { ExpeditionAllowed = false };
            var system = NewSystem(new[] { eventDef }, port);

            Assert.NotNull(system.SelectForDay(20, new SeededRng(4)));
            var result = system.CommitChoice(eventDef.Id, "investigate_rumor", 20);

            Assert.Equal(NarrativeArcChoiceStatus.Rejected, result.Status);
            Assert.Contains("expedition", result.Reason, StringComparison.OrdinalIgnoreCase);
            Assert.Empty(system.State.completedEventIds);
        }

        [Fact]
        public void UnknownExpeditionLocation_FailsClosedBeforeCommit()
        {
            string source = File.ReadAllText(Path.Combine(FindDataDirectory(), NarrativeArcEventCatalogLoader.FileName));
            var load = LoadRaw(source.Replace(
                "loc_missile_silo", "loc_missing_plan143", StringComparison.Ordinal));
            Assert.True(load.IsSuccess, string.Join(" | ", load.Errors));

            var eventDef = load.Events.Single(e => e.Id == "narrative_cult_prophet_rumor");
            var port = new RecordingPort { ExpeditionAllowed = false };
            var system = NewSystem(new[] { eventDef }, port);

            Assert.NotNull(system.SelectForDay(20, new SeededRng(4)));
            var result = system.CommitChoice(eventDef.Id, "investigate_rumor", 20);

            Assert.Equal(NarrativeArcChoiceStatus.Rejected, result.Status);
            Assert.Contains("expedition", result.Reason, StringComparison.OrdinalIgnoreCase);
            Assert.Empty(system.State.completedEventIds);
        }

        [Fact]
        public void UnboundConsequenceAuthority_FailsClosedWithoutCompletingChoice()
        {
            var eventDef = LoadCurrent().Events.Single(e => e.Id == "narrative_garrison_defector_intel");
            var system = new NarrativeArcEventSystem(new[] { eventDef })
            {
                SurvivorIsPresent = _ => true
            };

            Assert.NotNull(system.SelectForDay(10, new SeededRng(2)));
            var result = system.CommitChoice(eventDef.Id, "grant_asylum_intel", 10);

            Assert.Equal(NarrativeArcChoiceStatus.Rejected, result.Status);
            Assert.Contains("authority", result.Reason, StringComparison.OrdinalIgnoreCase);
            Assert.Empty(system.State.completedEventIds);
        }

        [Fact]
        public void Restore_ReconstructsPendingAndCompletionWithoutReplayingEffects()
        {
            var eventDef = LoadCurrent().Events.Single(e => e.Id == "narrative_garrison_defector_intel");
            var originalPort = new RecordingPort();
            var original = NewSystem(new[] { eventDef }, originalPort);
            Assert.NotNull(original.SelectForDay(10, new SeededRng(2)));
            Assert.Equal(NarrativeArcChoiceStatus.Committed,
                original.CommitChoice(eventDef.Id, "grant_asylum_intel", 10).Status);
            var saved = original.CaptureState();

            var restoredPort = new RecordingPort();
            var restored = NewSystem(new[] { eventDef }, restoredPort);
            restored.RestoreState(saved);

            Assert.Contains(eventDef.Id, restored.State.completedEventIds);
            Assert.Equal(0, restoredPort.MoraleCalls);
            Assert.Equal(0, restoredPort.IntelCalls);
            Assert.Equal(NarrativeArcChoiceStatus.AlreadyCommitted,
                restored.CommitChoice(eventDef.Id, "grant_asylum_intel", 10).Status);
            Assert.Equal(0, restoredPort.IntelCalls);
        }

        [Fact]
        public void UnknownEffect_FailsClosedDuringTypedLoad()
        {
            string raw = File.ReadAllText(Path.Combine(FindDataDirectory(), NarrativeArcEventCatalogLoader.FileName));
            raw = raw.Replace("advance_narrative_arc", "unknown_plan143_effect", StringComparison.Ordinal);
            var load = LoadRaw(raw);

            Assert.False(load.IsSuccess);
            Assert.Contains(load.Errors, e => e.Contains("must advance", StringComparison.OrdinalIgnoreCase));
            Assert.Empty(NarrativeArcEventCatalogLoader.Load("data", new MemoryFileIO(raw), new SystemTextJsonSerializer()));
        }

        [Fact]
        public void CatalogValidation_RejectsDuplicateEventChoiceAndStageFixtures()
        {
            string source = File.ReadAllText(Path.Combine(FindDataDirectory(), NarrativeArcEventCatalogLoader.FileName));

            var duplicateEvent = LoadRaw(source.Replace(
                "narrative_garrison_defector_intel", "narrative_cult_prophet_rumor", StringComparison.Ordinal));
            Assert.False(duplicateEvent.IsSuccess);
            Assert.Contains(duplicateEvent.Errors, e => e.Contains("duplicate event id", StringComparison.OrdinalIgnoreCase));

            var duplicateChoice = LoadRaw(source.Replace(
                "\"choiceId\": \"decline_council\"", "\"choiceId\": \"attend_council\"", StringComparison.Ordinal));
            Assert.False(duplicateChoice.IsSuccess);
            Assert.Contains(duplicateChoice.Errors, e => e.Contains("duplicate choice id", StringComparison.OrdinalIgnoreCase));

            var invalidBranch = LoadRaw(source.Replace(
                "\"branchId\": \"a\"", "\"branchId\": \"c\"", StringComparison.Ordinal));
            Assert.False(invalidBranch.IsSuccess);
            Assert.Contains(invalidBranch.Errors, e => e.Contains("branch", StringComparison.OrdinalIgnoreCase));

            var stageGap = LoadRaw(source.Replace(
                "narrative_aris_thorne_stage_3", "narrative_aris_thorne_stage_missing", StringComparison.Ordinal));
            Assert.False(stageGap.IsSuccess);
            Assert.Contains(stageGap.Errors, e => e.Contains("required Plan 143 event is missing", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void CatalogValidation_RejectsInvalidWeightAndUnknownPayloadReference()
        {
            string source = File.ReadAllText(Path.Combine(FindDataDirectory(), NarrativeArcEventCatalogLoader.FileName));

            var negativeWeight = LoadRaw(source.Replace(
                "\"weight\": 3.0", "\"weight\": -1.0", StringComparison.Ordinal));
            Assert.False(negativeWeight.IsSuccess);
            Assert.Contains(negativeWeight.Errors, e => e.Contains("finite positive weight", StringComparison.OrdinalIgnoreCase));

            var invalidStandingDelta = LoadRaw(source.Replace(
                "\"delta\": 20", "\"delta\": 120", StringComparison.Ordinal));
            Assert.True(invalidStandingDelta.IsSuccess, string.Join(" | ", invalidStandingDelta.Errors));
            var invalidStandingEvent = invalidStandingDelta.Events.Single(e => e.Id == "narrative_militia_council_invitation");
            Assert.False(invalidStandingEvent.Choices.Single(c => c.ChoiceId == "attend_council").IsExecutable);

            var nanWeight = LoadRaw(source.Replace(
                "\"weight\": 3.0", "\"weight\": NaN", StringComparison.Ordinal));
            Assert.False(nanWeight.IsSuccess);
            Assert.Contains(nanWeight.Errors, e => e.Contains("parse failed", StringComparison.OrdinalIgnoreCase));

            var unknownSurvivor = LoadRaw(source.Replace(
                "\"survivorId\": \"aris_thorne\"", "\"survivorId\": \"survivor_missing\"", StringComparison.Ordinal));
            Assert.False(unknownSurvivor.IsSuccess);
            Assert.Contains(unknownSurvivor.Errors, e => e.Contains("must advance", StringComparison.OrdinalIgnoreCase));

            var unknownFaction = LoadRaw(source.Replace(
                "\"factionId\": \"iron_garrison\"", "\"factionId\": \"faction_missing\"", StringComparison.Ordinal));
            Assert.True(unknownFaction.IsSuccess, string.Join(" | ", unknownFaction.Errors));
            var garrison = unknownFaction.Events.Single(e => e.Id == "narrative_garrison_defector_intel");
            Assert.All(garrison.Choices, choice => Assert.False(choice.IsExecutable));
        }

        private static NarrativeArcEventSystem NewSystem(
            IEnumerable<NarrativeArcEventDefinition> events,
            RecordingPort port)
        {
            var system = new NarrativeArcEventSystem(events);
            system.SurvivorIsPresent = _ => true;
            system.Consequences = port;
            return system;
        }

        private static List<string> RunDailySimulation(IEnumerable<NarrativeArcEventDefinition> events)
        {
            var system = NewSystem(events, new RecordingPort());
            var selected = new List<string>();
            for (int day = 1; day <= 40; day++)
            {
                var picked = system.SelectForDay(day, new SeededRng(9000 + day));
                if (picked == null) continue;

                selected.Add(picked.Id);
                if (picked.Choices.Count == 0)
                {
                    Assert.Equal(NarrativeArcChoiceStatus.Committed,
                        system.AcknowledgeEvent(picked.Id, day).Status);
                    continue;
                }

                var choice = picked.Choices.First(c => c.IsExecutable);
                Assert.Equal(NarrativeArcChoiceStatus.Committed,
                    system.CommitChoice(picked.Id, choice.ChoiceId, day).Status);
            }
            return selected;
        }

        private static NarrativeArcEventCatalogLoadResult LoadCurrent()
        {
            return LoadRaw(File.ReadAllText(Path.Combine(FindDataDirectory(), NarrativeArcEventCatalogLoader.FileName)));
        }

        private static NarrativeArcEventCatalogLoadResult LoadRaw(string raw)
        {
            return NarrativeArcEventCatalogLoader.LoadDetailed(
                "data", new MemoryFileIO(raw), new SystemTextJsonSerializer());
        }

        private static string FindDataDirectory()
        {
            string configured = Environment.GetEnvironmentVariable("ASHFALL_DATA_DIR") ?? string.Empty;
            if (Directory.Exists(configured)) return configured;

            string current = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                current = Directory.GetParent(current)?.FullName ?? string.Empty;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private sealed class RecordingPort : INarrativeArcConsequencePort
        {
            public bool StandingAllowed { get; set; } = true;
            public int MoraleCalls { get; private set; }
            public int IntelCalls { get; private set; }
            public int StandingCalls { get; private set; }
            public bool ExpeditionAllowed { get; set; } = true;

            public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason)
            {
                reason = string.Empty;
                return true;
            }

            public void ApplyMorale(string survivorId, int delta, bool shelterWide) => MoraleCalls += delta;

            public bool CanGrantFactionIntel(string canonicalFactionId, out string reason)
            {
                reason = string.Empty;
                return true;
            }

            public void GrantFactionIntel(string canonicalFactionId) => IntelCalls++;

            public bool CanOfferExpedition(string locationId, out string reason)
            {
                reason = ExpeditionAllowed ? string.Empty : "expedition dispatch is unavailable";
                return ExpeditionAllowed;
            }

            public void OfferExpedition(string locationId) { }

            public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
            {
                reason = StandingAllowed ? string.Empty : "standing authority rejected the delta";
                return StandingAllowed;
            }

            public void ApplyFactionStanding(string canonicalFactionId, int delta) => StandingCalls++;
        }

        private sealed class MemoryFileIO : IFileIO
        {
            private readonly string _contents;

            public MemoryFileIO(string contents) => _contents = contents;
            public bool DirectoryExists(string path) => path == "data";
            public bool FileExists(string path) => path == "data/" + NarrativeArcEventCatalogLoader.FileName;
            public string ReadAllText(string path) => _contents;
            public void WriteAllText(string path, string contents) { }
            public string Combine(params string[] parts) => string.Join("/", parts);
        }
    }
}
