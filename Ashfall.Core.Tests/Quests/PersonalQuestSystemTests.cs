// SPDX-License-Identifier: MIT
// ASHFALL survivor personal quest core test suite (Plan 83 / Task B24).

using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Quests;
using Xunit;

namespace Ashfall.Core.Tests.Quests
{
    public class PersonalQuestSystemTests
    {
        private static string FindDataDir()
        {
            string current = Directory.GetCurrentDirectory();
            while (current != null)
            {
                string probe = Path.Combine(current, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                probe = Path.Combine(current, "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                current = Directory.GetParent(current)?.FullName!;
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
        }

        private static PersonalQuestSystem CreateSystemWithAuthoredCatalog()
        {
            var sys = new PersonalQuestSystem(new SeededRng(101));
            string path = Path.Combine(FindDataDir(), "personal_quests.json");
            Assert.True(File.Exists(path), "personal_quests.json must exist at " + path);
            string json = File.ReadAllText(path);
            sys.LoadCatalog(json, new SystemTextJsonSerializer());
            return sys;
        }

        [Fact]
        public void Catalog_LoadsFromAuthoredJsonFile_ContainsTenArcs()
        {
            var sys = CreateSystemWithAuthoredCatalog();
            Assert.True(sys.Catalog.Count >= 10, $"Expected at least 10 quests, found {sys.Catalog.Count}");

            foreach (var kvp in sys.Catalog)
            {
                var q = kvp.Value;
                Assert.False(string.IsNullOrWhiteSpace(q.id));
                Assert.False(string.IsNullOrWhiteSpace(q.title));
                Assert.False(string.IsNullOrWhiteSpace(q.required_trait));
                Assert.NotEmpty(q.stages);
                foreach (var stage in q.stages)
                {
                    Assert.False(string.IsNullOrWhiteSpace(stage.title));
                    Assert.NotEmpty(stage.choices);
                }
            }
        }

        [Fact]
        public void TryTriggerQuest_ActivatesMatchingTraitQuest()
        {
            var sys = CreateSystemWithAuthoredCatalog();
            bool triggered = sys.TryTriggerQuest("survivor_01", "scout", 5);

            Assert.True(triggered);
            var active = sys.GetActiveQuest("survivor_01");
            Assert.NotNull(active);
            Assert.Equal("pq_buried_cache", active!.questId);
            Assert.Equal(0, active.currentStage);
            Assert.Equal(PersonalQuestStatus.Active, active.status);
            Assert.Equal(5, active.startedDay);
        }

        [Fact]
        public void TryTriggerQuest_RejectsSecondQuestForSameSurvivor()
        {
            var sys = CreateSystemWithAuthoredCatalog();
            Assert.True(sys.TryTriggerQuest("survivor_01", "scout", 1));

            // Attempting to trigger again for same survivor fails
            bool second = sys.TryTriggerQuest("survivor_01", "medic", 2);
            Assert.False(second);
            Assert.Single(sys.ActiveQuests);
        }

        [Fact]
        public void ProgressRequirement_AdvancesProgressCount()
        {
            var sys = CreateSystemWithAuthoredCatalog();
            sys.TryTriggerQuest("survivor_01", "scout", 1);

            bool prog1 = sys.ProgressRequirement("survivor_01", "days_elapsed", 1);
            Assert.True(prog1);
            var active = sys.GetActiveQuest("survivor_01");
            Assert.Equal(1, active!.progressCount);

            // Mismatched requirement fails
            bool progWrong = sys.ProgressRequirement("survivor_01", "deliver_item", 2);
            Assert.False(progWrong);
            Assert.Equal(1, active.progressCount);
        }

        [Fact]
        public void ChooseOption_BranchingTransitionsStage()
        {
            var sys = CreateSystemWithAuthoredCatalog();
            sys.TryTriggerQuest("survivor_01", "scout", 1);

            bool chosen = sys.ChooseOption("survivor_01", "study_thoroughly", 2, out var chosenDef);
            Assert.True(chosen);
            Assert.NotNull(chosenDef);
            Assert.Equal(5.0f, chosenDef!.morale_delta);

            var active = sys.GetActiveQuest("survivor_01");
            Assert.NotNull(active);
            Assert.Equal(1, active!.currentStage);
            Assert.Contains("study_thoroughly", active.selectedChoices);
        }

        [Fact]
        public void ChooseOption_CompletesQuestWhenTerminal()
        {
            var sys = CreateSystemWithAuthoredCatalog();
            sys.TryTriggerQuest("survivor_01", "scout", 1);
            sys.ChooseOption("survivor_01", "study_thoroughly", 2, out _);

            // Now on stage 1 (the final stage)
            bool finalChoice = sys.ChooseOption("survivor_01", "share_supplies", 4, out var termDef);
            Assert.True(finalChoice);
            Assert.NotNull(termDef);
            Assert.Equal(-1, termDef!.next_stage);

            Assert.Null(sys.GetActiveQuest("survivor_01"));
            Assert.Empty(sys.ActiveQuests);
            Assert.Single(sys.CompletedQuests);

            var finished = sys.CompletedQuests[0];
            Assert.Equal(PersonalQuestStatus.Completed, finished.status);
            Assert.Equal(4, finished.resolvedDay);
            Assert.Equal(2, finished.selectedChoices.Count);
        }

        [Fact]
        public void FailQuest_MovesToHistoricalWithReason()
        {
            var sys = CreateSystemWithAuthoredCatalog();
            sys.TryTriggerQuest("survivor_02", "medic", 1);

            bool failed = sys.FailQuest("survivor_02", "survivor_deceased", 3);
            Assert.True(failed);

            Assert.Null(sys.GetActiveQuest("survivor_02"));
            Assert.Empty(sys.ActiveQuests);
            Assert.Single(sys.CompletedQuests);

            var record = sys.CompletedQuests[0];
            Assert.Equal(PersonalQuestStatus.Failed, record.status);
            Assert.Equal("survivor_deceased", record.failureReason);
            Assert.Equal(3, record.resolvedDay);
        }

        [Fact]
        public void TickDay_AutomaticallyIncrementsDaysElapsedRequirement()
        {
            var sys = CreateSystemWithAuthoredCatalog();
            sys.TryTriggerQuest("survivor_01", "scout", 1); // stage 0 has days_elapsed

            sys.TickDay(2);
            var active = sys.GetActiveQuest("survivor_01");
            Assert.Equal(1, active!.progressCount);

            sys.TickDay(3);
            Assert.Equal(2, active.progressCount);
        }

        [Fact]
        public void SaveAndRestore_RoundTripsStateCorrectly()
        {
            var sys = CreateSystemWithAuthoredCatalog();
            sys.TryTriggerQuest("survivor_01", "scout", 1);
            sys.ChooseOption("survivor_01", "study_thoroughly", 2, out _);

            sys.TryTriggerQuest("survivor_02", "medic", 2);
            sys.FailQuest("survivor_02", "injury", 3);

            var captured = sys.CaptureState();
            Assert.Single(captured.activeQuests);
            Assert.Single(captured.completedQuests);

            var restoredSys = CreateSystemWithAuthoredCatalog();
            restoredSys.RestoreState(captured);

            var restoredActive = restoredSys.GetActiveQuest("survivor_01");
            Assert.NotNull(restoredActive);
            Assert.Equal(1, restoredActive!.currentStage);
            Assert.Single(restoredActive.selectedChoices);
            Assert.Equal("study_thoroughly", restoredActive.selectedChoices[0]);

            Assert.Single(restoredSys.CompletedQuests);
            Assert.Equal("survivor_02", restoredSys.CompletedQuests[0].survivorId);
            Assert.Equal("injury", restoredSys.CompletedQuests[0].failureReason);
        }

        [Fact]
        public void Determinism_StateMatchesAcrossIdenticalSimulations()
        {
            var sysA = CreateSystemWithAuthoredCatalog();
            var sysB = CreateSystemWithAuthoredCatalog();

            sysA.TryTriggerQuest("surv_alpha", "mechanic", 1);
            sysB.TryTriggerQuest("surv_alpha", "mechanic", 1);

            sysA.TickDay(2);
            sysB.TickDay(2);

            sysA.ChooseOption("surv_alpha", "boost_gain", 3, out _);
            sysB.ChooseOption("surv_alpha", "boost_gain", 3, out _);

            var stateA = sysA.CaptureState();
            var stateB = sysB.CaptureState();

            string hashA = SaveChecksum.Compute(stateA);
            string hashB = SaveChecksum.Compute(stateB);

            Assert.Equal(hashA, hashB);
        }
    }
}
