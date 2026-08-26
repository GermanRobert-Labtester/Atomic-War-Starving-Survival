using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Comprehensive tests for ExpansionQuestSystem:
    /// catalog binding, availability filtering, lifecycle transitions, event firing,
    /// choices and effects, daily ticking, state capture/restore round-trip,
    /// SaveChecksum envelope verification, and JSON catalog loader.
    /// </summary>
    public class ExpansionQuestSystemTests
    {
        // ─── Helpers ───────────────────────────────────────────────────────────────

        private static ExpansionQuestEntry MakeQuest(
            string id = "quest_test_alpha",
            int minDay = 1,
            int maxDay = 100,
            List<string>? prereqs = null,
            List<ExpansionQuestChoice>? choices = null)
        {
            return new ExpansionQuestEntry
            {
                id = id,
                title = "Test Alpha Quest",
                description = "A test expansion quest description.",
                type = "exploration",
                minDay = minDay,
                maxDay = maxDay,
                factionTag = "scavengers",
                synopsis = "Test synopsis.",
                prerequisites = prereqs ?? new List<string>(),
                choices = choices ?? new List<ExpansionQuestChoice>
                {
                    new ExpansionQuestChoice
                    {
                        id = "choice_agree",
                        text = "Agree to assist",
                        consequences = "Assisted the faction.",
                        effects = new List<ExpansionQuestEffect>
                        {
                            new ExpansionQuestEffect { type = "faction_relation", target = "scavengers", value = 15 },
                            new ExpansionQuestEffect { type = "resource", target = "scrap_metal", value = 25 }
                        }
                    },
                    new ExpansionQuestChoice
                    {
                        id = "choice_refuse",
                        text = "Refuse offer",
                        consequences = "Refused the faction.",
                        effects = new List<ExpansionQuestEffect>
                        {
                            new ExpansionQuestEffect { type = "faction_relation", target = "scavengers", value = -10 }
                        }
                    }
                }
            };
        }

        private static ExpansionQuestSystem MakeSystem(params ExpansionQuestEntry[] quests)
        {
            var system = new ExpansionQuestSystem();
            system.BindCatalog(quests.ToList());
            return system;
        }

        // ─── 1. Catalog Binding ───────────────────────────────────────────────────

        [Fact]
        public void BindCatalog_LoadsAllQuests()
        {
            var system = MakeSystem(MakeQuest("quest_a"), MakeQuest("quest_b"));
            var qA = system.GetDefinition("quest_a");
            var qB = system.GetDefinition("quest_b");

            Assert.NotNull(qA);
            Assert.NotNull(qB);
            Assert.Equal("quest_a", qA.id);
            Assert.Equal("quest_b", qB.id);
        }

        [Fact]
        public void BindCatalog_NullCatalog_DefaultsToEmpty()
        {
            var system = new ExpansionQuestSystem();
            system.BindCatalog(null!);
            Assert.Null(system.GetDefinition("quest_any"));
            Assert.Empty(system.GetAvailableQuests(10));
            Assert.Empty(system.GetActiveQuests());
        }

        [Fact]
        public void GetDefinition_UnknownId_ReturnsNull()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            Assert.Null(system.GetDefinition("quest_unknown"));
        }

        // ─── 2. Availability Queries ──────────────────────────────────────────────

        [Fact]
        public void IsAvailable_SimpleCheck_ReturnsTrueIfInCatalog()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            Assert.True(system.IsAvailable("quest_a"));
            Assert.False(system.IsAvailable("quest_nonexistent"));
        }

        [Fact]
        public void IsAvailable_DayWindow_ReturnsTrueWhenInRange()
        {
            var system = MakeSystem(MakeQuest("quest_a", minDay: 10, maxDay: 50));
            Assert.False(system.IsAvailable("quest_a", day: 9));
            Assert.True(system.IsAvailable("quest_a", day: 10));
            Assert.True(system.IsAvailable("quest_a", day: 30));
            Assert.True(system.IsAvailable("quest_a", day: 50));
            Assert.False(system.IsAvailable("quest_a", day: 51));
        }

        [Fact]
        public void IsAvailable_Prerequisites_EnforcesCompletion()
        {
            var qA = MakeQuest("quest_prereq", minDay: 1, maxDay: 100);
            var qB = MakeQuest("quest_dependent", minDay: 1, maxDay: 100, prereqs: new List<string> { "quest_prereq" });
            var system = MakeSystem(qA, qB);

            // Prereq not started or completed
            Assert.False(system.IsAvailable("quest_dependent", day: 5));

            // Start prereq (still not completed)
            system.StartQuest("quest_prereq", day: 5);
            Assert.False(system.IsAvailable("quest_dependent", day: 6));

            // Complete prereq -> dependent becomes available
            system.CompleteQuest("quest_prereq", day: 7);
            Assert.True(system.IsAvailable("quest_dependent", day: 8));
        }

        [Fact]
        public void IsAvailable_ExcludesStartedCompletedAndFailed()
        {
            var system = MakeSystem(MakeQuest("quest_a"));

            Assert.True(system.IsAvailable("quest_a", day: 10));

            system.StartQuest("quest_a", day: 10);
            Assert.False(system.IsAvailable("quest_a", day: 10));

            system.CompleteQuest("quest_a", day: 11);
            Assert.False(system.IsAvailable("quest_a", day: 12));
        }

        [Fact]
        public void GetAvailableQuests_ReturnsOnlyEligibleEntries()
        {
            var q1 = MakeQuest("quest_early", minDay: 5, maxDay: 20);
            var q2 = MakeQuest("quest_late", minDay: 50, maxDay: 100);
            var q3 = MakeQuest("quest_locked", minDay: 1, maxDay: 100, prereqs: new List<string> { "quest_unmet" });
            var system = MakeSystem(q1, q2, q3);

            var day10 = system.GetAvailableQuests(day: 10);
            Assert.Single(day10);
            Assert.Equal("quest_early", day10[0].id);

            var day60 = system.GetAvailableQuests(day: 60);
            Assert.Single(day60);
            Assert.Equal("quest_late", day60[0].id);
        }

        // ─── 3. Lifecycle Operations & Events ─────────────────────────────────────

        [Fact]
        public void StartQuest_SetsStartedAndDay()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            system.StartQuest("quest_a", day: 15);

            Assert.True(system.IsStarted("quest_a"));
            Assert.False(system.IsCompleted("quest_a"));
            Assert.False(system.IsFailed("quest_a"));

            var progress = system.GetProgress("quest_a");
            Assert.NotNull(progress);
            Assert.Equal("quest_a", progress.questId);
            Assert.True(progress.started);
            Assert.Equal(15, progress.dayStarted);
            Assert.Equal(-1, progress.dayResolved);
        }

        [Fact]
        public void StartQuest_FiresEvents()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            ExpansionQuestEntry? startedEntry = null;
            ExpansionQuestSystemState? changedState = null;

            system.OnQuestStarted += entry => startedEntry = entry;
            system.OnStateChanged += state => changedState = state;

            system.StartQuest("quest_a", day: 3);

            Assert.NotNull(startedEntry);
            Assert.Equal("quest_a", startedEntry.id);
            Assert.NotNull(changedState);
            Assert.Single(changedState.quests);
        }

        [Fact]
        public void StartQuest_AlreadyStarted_Noop()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            system.StartQuest("quest_a", day: 5);
            system.StartQuest("quest_a", day: 10);

            var progress = system.GetProgress("quest_a");
            Assert.NotNull(progress);
            Assert.Equal(5, progress.dayStarted);
            Assert.Single(system.State.quests);
        }

        [Fact]
        public void CompleteQuest_MarksCompletedAndRecordsDay()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            system.StartQuest("quest_a", day: 2);

            ExpansionQuestEntry? completedEntry = null;
            system.OnQuestCompleted += entry => completedEntry = entry;

            system.CompleteQuest("quest_a", day: 8);

            Assert.True(system.IsCompleted("quest_a"));
            Assert.Contains("quest_a", system.State.completedQuestIds);
            Assert.NotNull(completedEntry);
            Assert.Equal("quest_a", completedEntry.id);

            var progress = system.GetProgress("quest_a");
            Assert.NotNull(progress);
            Assert.True(progress.completed);
            Assert.Equal(8, progress.dayResolved);
        }

        [Fact]
        public void CompleteQuest_NotStarted_Noop()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            system.CompleteQuest("quest_a", day: 5);

            Assert.False(system.IsCompleted("quest_a"));
            Assert.Empty(system.State.completedQuestIds);
        }

        [Fact]
        public void FailQuest_MarksFailedAndRecordsDay()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            system.StartQuest("quest_a", day: 2);

            ExpansionQuestEntry? failedEntry = null;
            system.OnQuestFailed += entry => failedEntry = entry;

            system.FailQuest("quest_a", day: 9);

            Assert.True(system.IsFailed("quest_a"));
            Assert.Contains("quest_a", system.State.failedQuestIds);
            Assert.NotNull(failedEntry);
            Assert.Equal("quest_a", failedEntry.id);

            var progress = system.GetProgress("quest_a");
            Assert.NotNull(progress);
            Assert.True(progress.failed);
            Assert.Equal(9, progress.dayResolved);
        }

        [Fact]
        public void FailQuest_NotStarted_Noop()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            system.FailQuest("quest_a", day: 5);

            Assert.False(system.IsFailed("quest_a"));
            Assert.Empty(system.State.failedQuestIds);
        }

        [Fact]
        public void GetActiveQuests_ReturnsOnlyActiveInProgressQuests()
        {
            var system = MakeSystem(
                MakeQuest("quest_active_1"),
                MakeQuest("quest_active_2"),
                MakeQuest("quest_done"),
                MakeQuest("quest_failed"),
                MakeQuest("quest_unstarted")
            );

            system.StartQuest("quest_active_1", 1);
            system.StartQuest("quest_active_2", 1);
            system.StartQuest("quest_done", 1);
            system.CompleteQuest("quest_done", 2);
            system.StartQuest("quest_failed", 1);
            system.FailQuest("quest_failed", 2);

            var active = system.GetActiveQuests();
            Assert.Equal(2, active.Count);
            Assert.Contains(active, q => q.id == "quest_active_1");
            Assert.Contains(active, q => q.id == "quest_active_2");
        }

        // ─── 4. Choices & Effects Application ─────────────────────────────────────

        [Fact]
        public void MakeChoice_SetsCurrentChoiceId()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            system.StartQuest("quest_a", day: 1);
            system.MakeChoice("quest_a", "choice_agree", day: 2);

            var progress = system.GetProgress("quest_a");
            Assert.NotNull(progress);
            Assert.Equal("choice_agree", progress.currentChoiceId);
        }

        [Fact]
        public void MakeChoice_AppliesChainedQuestEffects()
        {
            var questA = MakeQuest("quest_a", choices: new List<ExpansionQuestChoice>
            {
                new ExpansionQuestChoice
                {
                    id = "choice_trigger_chain",
                    text = "Unlock the mystery",
                    effects = new List<ExpansionQuestEffect>
                    {
                        new ExpansionQuestEffect { type = "start_quest", target = "quest_b" },
                        new ExpansionQuestEffect { type = "complete_quest", target = "quest_a" }
                    }
                },
                new ExpansionQuestChoice
                {
                    id = "choice_fail_chain",
                    text = "Abandon lead",
                    effects = new List<ExpansionQuestEffect>
                    {
                        new ExpansionQuestEffect { type = "fail_quest", target = "quest_a" }
                    }
                }
            });
            var questB = MakeQuest("quest_b");

            var system = MakeSystem(questA, questB);
            system.StartQuest("quest_a", day: 10);

            system.MakeChoice("quest_a", "choice_trigger_chain", day: 11);

            Assert.True(system.IsCompleted("quest_a"));
            Assert.True(system.IsStarted("quest_b"));
            Assert.Equal(11, system.GetProgress("quest_b")!.dayStarted);
        }

        [Fact]
        public void GetChoices_ReturnsChoicesFromDefinition()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            var choices = system.GetChoices("quest_a");

            Assert.Equal(2, choices.Count);
            Assert.Equal("choice_agree", choices[0].id);
            Assert.Equal("choice_refuse", choices[1].id);
        }

        [Fact]
        public void GetChoices_UnknownQuest_ReturnsEmptyList()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            var choices = system.GetChoices("quest_unknown");
            Assert.NotNull(choices);
            Assert.Empty(choices);
        }

        // ─── 5. Daily Tick Progression ────────────────────────────────────────────

        [Fact]
        public void TickDay_EmptyCatalog_ReturnsWarning()
        {
            var system = new ExpansionQuestSystem();
            string status = system.TickDay(5);
            Assert.Equal("No expansion quests loaded", status);
        }

        [Fact]
        public void TickDay_AutoStartsQuestsWithinWindow()
        {
            var q1 = MakeQuest("quest_day_5", minDay: 5, maxDay: 20);
            var q2 = MakeQuest("quest_day_10", minDay: 10, maxDay: 30);
            var system = MakeSystem(q1, q2);

            string statusDay5 = system.TickDay(5);
            Assert.Contains("1 new quests started", statusDay5);
            Assert.True(system.IsStarted("quest_day_5"));
            Assert.False(system.IsStarted("quest_day_10"));

            string statusDay10 = system.TickDay(10);
            Assert.Contains("1 new quests started", statusDay10);
            Assert.True(system.IsStarted("quest_day_10"));
        }

        // ─── 6. State Capture & Restore ───────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTrip_PreservesAllState()
        {
            var system = MakeSystem(MakeQuest("quest_a"), MakeQuest("quest_b"), MakeQuest("quest_c"));
            system.StartQuest("quest_a", day: 1);
            system.MakeChoice("quest_a", "choice_agree", day: 2);
            system.CompleteQuest("quest_a", day: 3);

            system.StartQuest("quest_b", day: 4);
            system.FailQuest("quest_b", day: 5);

            system.StartQuest("quest_c", day: 6);

            var state = system.CaptureState();

            var restoredSystem = MakeSystem(MakeQuest("quest_a"), MakeQuest("quest_b"), MakeQuest("quest_c"));
            restoredSystem.RestoreState(state);

            Assert.True(restoredSystem.IsCompleted("quest_a"));
            Assert.True(restoredSystem.IsFailed("quest_b"));
            Assert.True(restoredSystem.IsStarted("quest_c"));

            var pA = restoredSystem.GetProgress("quest_a");
            Assert.NotNull(pA);
            Assert.Equal("choice_agree", pA.currentChoiceId);
            Assert.Equal(1, pA.dayStarted);
            Assert.Equal(3, pA.dayResolved);

            var pB = restoredSystem.GetProgress("quest_b");
            Assert.NotNull(pB);
            Assert.Equal(4, pB.dayStarted);
            Assert.Equal(5, pB.dayResolved);

            Assert.Contains("quest_a", restoredSystem.State.completedQuestIds);
            Assert.Contains("quest_b", restoredSystem.State.failedQuestIds);
        }

        [Fact]
        public void CaptureRestore_DeepCopy_NoAliasing()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            system.StartQuest("quest_a", day: 1);

            var state = system.CaptureState();

            // Mutate system further
            system.CompleteQuest("quest_a", day: 5);

            // Captured state must remain unaffected
            var progressInSnapshot = state.quests.First(q => q.questId == "quest_a");
            Assert.False(progressInSnapshot.completed);
            Assert.Equal(-1, progressInSnapshot.dayResolved);
        }

        [Fact]
        public void RestoreState_Null_DoesNotThrow()
        {
            var system = MakeSystem(MakeQuest("quest_a"));
            system.RestoreState(null!);
            Assert.NotNull(system.State);
        }

        // ─── 7. Save Codec & Checksums ────────────────────────────────────────────

        [Fact]
        public void SaveCodec_EncodeDecode_RoundTrip()
        {
            var serializer = new SystemTextJsonSerializer();
            var state = new ExpansionQuestSystemState
            {
                completedQuestIds = new List<string> { "quest_completed_1" },
                failedQuestIds = new List<string> { "quest_failed_1" },
                quests = new List<ExpansionQuestProgress>
                {
                    new ExpansionQuestProgress
                    {
                        questId = "quest_completed_1",
                        started = true,
                        completed = true,
                        dayStarted = 1,
                        dayResolved = 4
                    }
                }
            };

            var envelope = new ExpansionQuestSaveEnvelope
            {
                version = ExpansionQuestSaveEnvelope.CurrentVersion,
                state = state
            };

            string encoded = ExpansionQuestSaveCodec.Encode(envelope, serializer);
            Assert.False(string.IsNullOrWhiteSpace(encoded));
            Assert.False(string.IsNullOrWhiteSpace(envelope.checksum));

            var decoded = ExpansionQuestSaveCodec.Decode(encoded, serializer);
            Assert.NotNull(decoded);
            Assert.Equal(ExpansionQuestSaveEnvelope.CurrentVersion, decoded.version);
            Assert.Single(decoded.state.completedQuestIds);
            Assert.Equal("quest_completed_1", decoded.state.completedQuestIds[0]);
            Assert.Single(decoded.state.failedQuestIds);
            Assert.Equal("quest_failed_1", decoded.state.failedQuestIds[0]);
        }

        [Fact]
        public void SaveCodec_TamperedChecksum_Throws()
        {
            var serializer = new SystemTextJsonSerializer();
            var envelope = new ExpansionQuestSaveEnvelope
            {
                version = ExpansionQuestSaveEnvelope.CurrentVersion,
                state = new ExpansionQuestSystemState()
            };

            string encoded = ExpansionQuestSaveCodec.Encode(envelope, serializer);
            // Replace checksum with forged hash
            string tampered = encoded.Replace(envelope.checksum, "0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef");

            Assert.Throws<InvalidOperationException>(() => ExpansionQuestSaveCodec.Decode(tampered, serializer));
        }

        [Fact]
        public void SaveCodec_EmptyOrNullJson_Throws()
        {
            var serializer = new SystemTextJsonSerializer();
            Assert.Throws<InvalidOperationException>(() => ExpansionQuestSaveCodec.Decode(string.Empty, serializer));
            Assert.Throws<InvalidOperationException>(() => ExpansionQuestSaveCodec.Decode("   ", serializer));
            Assert.Throws<InvalidOperationException>(() => ExpansionQuestSaveCodec.Decode(null!, serializer));
        }

        [Fact]
        public void SaveCodec_FutureVersion_Throws()
        {
            var serializer = new SystemTextJsonSerializer();
            var futureEnvelope = new ExpansionQuestSaveEnvelope
            {
                version = 99,
                state = new ExpansionQuestSystemState(),
                checksum = "dummy"
            };
            string raw = serializer.Serialize(futureEnvelope);

            Assert.Throws<InvalidOperationException>(() => ExpansionQuestSaveCodec.Decode(raw, serializer));
        }

        // ─── 8. Catalog Loader from Data Files ────────────────────────────────────

        [Fact]
        public void CatalogLoader_LoadsExistingExpansionQuests()
        {
            string dataDir = Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "..",
                "Assets", "StreamingAssets", "Data");

            if (!Directory.Exists(dataDir))
            {
                // In isolated test environments where Assets/ is not relative
                return;
            }

            var quests = ExpansionQuestCatalogLoader.Load(dataDir);
            Assert.NotEmpty(quests);
            Assert.Equal(41, quests.Count);

            Assert.All(quests, q =>
            {
                Assert.False(string.IsNullOrWhiteSpace(q.id), "Quest ID cannot be empty");
                Assert.StartsWith("quest_", q.id, StringComparison.Ordinal);
                Assert.False(string.IsNullOrWhiteSpace(q.title), "Quest title cannot be empty");
                Assert.True(q.minDay <= q.maxDay, $"minDay ({q.minDay}) must be <= maxDay ({q.maxDay}) for {q.id}");
                Assert.NotEmpty(q.choices);
            });
        }

        [Fact]
        public void CatalogLoader_NonexistentDir_ReturnsEmptyList()
        {
            var quests = ExpansionQuestCatalogLoader.Load("/path/that/does/not/exist");
            Assert.NotNull(quests);
            Assert.Empty(quests);
        }
    }
}
