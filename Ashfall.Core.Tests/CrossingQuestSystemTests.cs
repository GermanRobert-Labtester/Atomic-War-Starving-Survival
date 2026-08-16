using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Crossing;

namespace Ashfall.Core.Tests
{
    public class CrossingQuestSystemTests
    {
        private static List<CrossingQuestDef> SampleCatalog()
        {
            return new List<CrossingQuestDef>
            {
                new CrossingQuestDef
                {
                    id = "quest_crossing_the_vouch",
                    display_name = "A Name at the Gate",
                    type = "expedition",
                    briefing = "Bram Ostrowski names the Crossing.",
                    prereq_quest_id = "",
                    min_day = 10,
                    stages = new List<CrossingQuestStage>
                    {
                        new CrossingQuestStage { id = "s0", text = "Hear Ostrowski out." },
                        new CrossingQuestStage { id = "s1", text = "Find a name." },
                        new CrossingQuestStage { id = "s2", text = "Walk the approach." }
                    },
                    choices = new List<CrossingQuestChoice>
                    {
                        new CrossingQuestChoice { id = "vouch_ostrowski", text = "Ostrowski vouches.", set_flag = "flag_vouched_clean" }
                    },
                    knowledge_key = "lore_nc_the_vouch",
                    target_location_id = "loc_crossing_viaduct_gate"
                },
                new CrossingQuestDef
                {
                    id = "quest_crossing_first_weigh",
                    display_name = "What the Scale Says",
                    type = "expedition",
                    briefing = "Osran weighs your goods.",
                    prereq_quest_id = "quest_crossing_the_vouch",
                    min_day = 20,
                    stages = new List<CrossingQuestStage>
                    {
                        new CrossingQuestStage { id = "s0", text = "Present goods." },
                        new CrossingQuestStage { id = "s1", text = "Accept weight." }
                    },
                    choices = new List<CrossingQuestChoice>
                    {
                        new CrossingQuestChoice { id = "accept_true", text = "Accept.", set_flag = "flag_honest_trader" },
                        new CrossingQuestChoice { id = "contest", text = "Contest.", set_flag = "flag_difficult" }
                    },
                    knowledge_key = "lore_nc_read_again",
                    target_location_id = "loc_crossing_scalehouse"
                }
            };
        }

        private static CrossingQuestSystem FreshSystem()
        {
            var sys = new CrossingQuestSystem();
            sys.BindCatalog(SampleCatalog());
            return sys;
        }

        [Fact]
        public void BindCatalog_Populates_Catalog()
        {
            var sys = FreshSystem();
            Assert.Equal(2, sys.Catalog.Count);
            Assert.NotNull(sys.GetDef("quest_crossing_the_vouch"));
            Assert.Null(sys.GetDef("nonexistent"));
        }

        [Fact]
        public void StartQuest_Succeeds_When_PrereqsMet()
        {
            var sys = FreshSystem();
            bool started = false;
            sys.OnQuestStarted += id => started = true;

            Assert.True(sys.StartQuest("quest_crossing_the_vouch", 10));
            Assert.True(started);
            Assert.True(sys.IsQuestStarted("quest_crossing_the_vouch"));
        }

        [Fact]
        public void StartQuest_Fails_Before_MinDay()
        {
            var sys = FreshSystem();
            Assert.False(sys.StartQuest("quest_crossing_the_vouch", 5));
            Assert.False(sys.IsQuestStarted("quest_crossing_the_vouch"));
        }

        [Fact]
        public void StartQuest_Fails_When_PrereqNotCompleted()
        {
            var sys = FreshSystem();
            Assert.False(sys.StartQuest("quest_crossing_first_weigh", 25));
        }

        [Fact]
        public void StartQuest_Fails_When_AlreadyStarted()
        {
            var sys = FreshSystem();
            Assert.True(sys.StartQuest("quest_crossing_the_vouch", 10));
            Assert.False(sys.StartQuest("quest_crossing_the_vouch", 10));
        }

        [Fact]
        public void AdvanceStage_Progresses_Through_Stages()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);

            int? reportedStage = null;
            sys.OnQuestStageChanged += (id, stage) => reportedStage = stage;

            Assert.Equal(1, sys.AdvanceStage("quest_crossing_the_vouch"));
            Assert.Equal(1, reportedStage);

            Assert.Equal(2, sys.AdvanceStage("quest_crossing_the_vouch"));
            Assert.Equal(2, reportedStage);
        }

        [Fact]
        public void AdvanceStage_Completes_When_PastLastStage()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);

            bool completed = false;
            sys.OnQuestCompleted += id => completed = true;

            sys.AdvanceStage("quest_crossing_the_vouch"); // 0→1
            sys.AdvanceStage("quest_crossing_the_vouch"); // 1→2
            var result = sys.AdvanceStage("quest_crossing_the_vouch"); // 2→complete

            Assert.Equal(-1, result);
            Assert.True(completed);
            Assert.True(sys.IsQuestCompleted("quest_crossing_the_vouch"));
        }

        [Fact]
        public void OpeningQuestCompletion_Fires_Event()
        {
            var sys = FreshSystem();
            bool openingFired = false;
            sys.OnOpeningQuestCompleted += () => openingFired = true;

            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");

            Assert.True(openingFired);
        }

        [Fact]
        public void MakeChoice_SetsFlag()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);

            string setFlag = null;
            sys.OnFlagSet += (qid, flag) => setFlag = flag;

            Assert.True(sys.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski"));
            Assert.Equal("flag_vouched_clean", setFlag);
            Assert.True(sys.HasFlag("flag_vouched_clean"));
        }

        [Fact]
        public void MakeChoice_Fails_For_InvalidChoice()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);
            Assert.False(sys.MakeChoice("quest_crossing_the_vouch", "nonexistent_choice"));
        }

        [Fact]
        public void GetAvailableQuests_FiltersByDay_And_Prereqs()
        {
            var sys = FreshSystem();

            var day5 = sys.GetAvailableQuests(5);
            Assert.Empty(day5);

            var day10 = sys.GetAvailableQuests(10);
            Assert.Single(day10);
            Assert.Equal("quest_crossing_the_vouch", day10[0].id);

            var day25 = sys.GetAvailableQuests(25);
            Assert.Single(day25);
            Assert.Equal("quest_crossing_the_vouch", day25[0].id);
        }

        [Fact]
        public void GetAvailableQuests_UnlocksAfter_PrereqCompleted()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");

            var available = sys.GetAvailableQuests(25);
            Assert.Contains(available, q => q.id == "quest_crossing_first_weigh");
        }

        [Fact]
        public void GetAvailableQuests_Excludes_Completed()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");

            var available = sys.GetAvailableQuests(10);
            Assert.DoesNotContain(available, q => q.id == "quest_crossing_the_vouch");
        }

        [Fact]
        public void SaveLoad_RoundTrip()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski");

            var saved = sys.CaptureState();
            Assert.Single(saved.quests);
            Assert.Contains("flag_vouched_clean", saved.setFlags);

            var sys2 = FreshSystem();
            sys2.RestoreState(saved);

            Assert.True(sys2.IsQuestStarted("quest_crossing_the_vouch"));
            Assert.False(sys2.IsQuestCompleted("quest_crossing_the_vouch"));
            Assert.True(sys2.HasFlag("flag_vouched_clean"));

            var progress = sys2.GetProgress("quest_crossing_the_vouch");
            Assert.NotNull(progress);
            Assert.Equal(1, progress.currentStage);
            Assert.Equal("vouch_ostrowski", progress.chosenChoiceId);
        }

        [Fact]
        public void RestoreState_Null_IsSafe()
        {
            var sys = FreshSystem();
            sys.RestoreState(null);
            Assert.Empty(sys.State.quests);
        }

        [Fact]
        public void ExpansionHubSave_Includes_CrossingQuests()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski");

            var vouch = new VouchAccessSystem();
            var waystation = new WaystationSystem();
            var greenhouse = new GreenhouseSystem(1);
            var arbitration = new CrossingArbitrationSystem();
            var ledger = new LedgerDebtSystem();
            var layouts = new LocationLayoutSystem(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var memory = new LocationMemorySystem(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var siteEncounters = new SiteEncounterSystem();

            var save = ExpansionHubSaveCodec.Capture(42,
                waystation, layouts, memory, siteEncounters, vouch, greenhouse,
                arbitration, ledger, sys);

            Assert.Single(save.crossingQuests.quests);
            Assert.Contains("flag_vouched_clean", save.crossingQuests.setFlags);

            var sys2 = FreshSystem();
            ExpansionHubSaveCodec.Restore(save,
                waystation, layouts, memory, siteEncounters, vouch, greenhouse,
                arbitration, ledger, sys2);

            Assert.True(sys2.IsQuestStarted("quest_crossing_the_vouch"));
            Assert.True(sys2.HasFlag("flag_vouched_clean"));
        }

        // ── Daily auto-start (TickDaily) ───────────────────────────────────────────

        [Fact]
        public void TickDaily_StartsEligibleQuest_Once()
        {
            var sys = FreshSystem();
            int startCount = 0;
            sys.OnQuestStarted += _ => startCount++;

            sys.TickDaily(10);

            Assert.Equal(1, startCount);
            Assert.True(sys.IsQuestStarted("quest_crossing_the_vouch"));
        }

        [Fact]
        public void TickDaily_Idempotent_SameDay()
        {
            var sys = FreshSystem();
            int startCount = 0;
            sys.OnQuestStarted += _ => startCount++;

            sys.TickDaily(10);
            sys.TickDaily(10); // repeated tick same day — must be a no-op
            sys.TickDaily(10);

            Assert.Equal(1, startCount);
        }

        [Fact]
        public void TickDaily_NoStart_BeforeMinDay()
        {
            var sys = FreshSystem();
            int startCount = 0;
            sys.OnQuestStarted += _ => startCount++;

            sys.TickDaily(5); // min_day is 10 in sample catalog

            Assert.Equal(0, startCount);
            Assert.False(sys.IsQuestStarted("quest_crossing_the_vouch"));
        }

        [Fact]
        public void TickDaily_DoesNotStart_AlreadyStarted()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);

            int startCount = 0;
            sys.OnQuestStarted += _ => startCount++;

            sys.TickDaily(11); // next day tick — quest already started
            Assert.Equal(0, startCount);
        }

        [Fact]
        public void TickDaily_DoesNotStart_AlreadyCompleted()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");
            Assert.True(sys.IsQuestCompleted("quest_crossing_the_vouch"));

            int startCount = 0;
            sys.OnQuestStarted += _ => startCount++;
            sys.TickDaily(11);
            Assert.Equal(0, startCount);
        }

        [Fact]
        public void TickDaily_SaveLoad_NoRestartAfterRestore()
        {
            var sys = FreshSystem();
            sys.TickDaily(10); // starts the vouch quest

            // Save and restore
            var saved = sys.CaptureState();
            Assert.Equal(10, saved.lastTickedDay);

            var sys2 = FreshSystem();
            sys2.RestoreState(saved);

            int startCount = 0;
            sys2.OnQuestStarted += _ => startCount++;

            sys2.TickDaily(10); // same day after restore — must not restart
            Assert.Equal(0, startCount);
            Assert.Equal(1, sys2.State.quests.Count);
        }

        [Fact]
        public void TickDaily_ManualStart_PlusTick_NoDoubleStart()
        {
            var sys = FreshSystem();
            bool manuallyCounted = false;
            sys.OnQuestStarted += _ => manuallyCounted = true;
            sys.StartQuest("quest_crossing_the_vouch", 10);
            Assert.True(manuallyCounted);

            int tickStartCount = 0;
            sys.OnQuestStarted += _ => tickStartCount++;

            // Tick same day after manual start
            sys.TickDaily(10);
            Assert.Equal(0, tickStartCount);
        }

        // ── Exactly-once stage narrative dispatch ──────────────────────────────────

        [Fact]
        public void StartQuest_EmitsStageNarrative_ExactlyOnce()
        {
            var sys = FreshSystem();
            var emitted = new List<CrossingStageNarrativeEvent>();
            sys.OnStageNarrativeEmitted += e => emitted.Add(e);

            sys.StartQuest("quest_crossing_the_vouch", 10);

            Assert.Single(emitted);
            Assert.Equal("quest_crossing_the_vouch", emitted[0].questId);
            Assert.Equal(0, emitted[0].stageIndex);
            Assert.False(emitted[0].isCompletion);
        }

        [Fact]
        public void AdvanceStage_EmitsNarrative_ExactlyOnce_PerStage()
        {
            var sys = FreshSystem();
            var emitted = new List<CrossingStageNarrativeEvent>();
            sys.OnStageNarrativeEmitted += e => emitted.Add(e);

            sys.StartQuest("quest_crossing_the_vouch", 10); // stage 0 emitted at start
            sys.AdvanceStage("quest_crossing_the_vouch");   // stage 1
            sys.AdvanceStage("quest_crossing_the_vouch");   // stage 2
            sys.AdvanceStage("quest_crossing_the_vouch");   // completion

            Assert.Equal(4, emitted.Count);
            Assert.False(emitted[0].isCompletion);
            Assert.False(emitted[1].isCompletion);
            Assert.False(emitted[2].isCompletion);
            Assert.True(emitted[3].isCompletion);
        }

        [Fact]
        public void SaveLoad_DoesNotReplayStageNarrative()
        {
            var sys = FreshSystem();
            var emitted = new List<CrossingStageNarrativeEvent>();
            sys.OnStageNarrativeEmitted += e => emitted.Add(e);

            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.AdvanceStage("quest_crossing_the_vouch");
            int countBeforeSave = emitted.Count;

            var saved = sys.CaptureState();

            var sys2 = FreshSystem();
            sys2.OnStageNarrativeEmitted += e => emitted.Add(e);
            sys2.RestoreState(saved);

            // RestoreState must not re-emit any previously dispatched stage events
            Assert.Equal(countBeforeSave, emitted.Count);
        }

        [Fact]
        public void Stage_Narrative_Key_IsUnique_PerStage()
        {
            var sys = FreshSystem();
            var keys = new HashSet<string>();
            sys.OnStageNarrativeEmitted += e => keys.Add($"{e.questId}:{e.stageIndex}:{e.isCompletion}");

            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");

            Assert.Equal(4, keys.Count);
        }

        // ── Failure handling & Post-vouch gating ───────────────────────────────────

        [Fact]
        public void FailQuest_MarksQuestFailed_And_FiresEvent()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);

            string failedQuestId = null;
            sys.OnQuestFailed += qId => failedQuestId = qId;

            Assert.True(sys.FailQuest("quest_crossing_the_vouch"));
            Assert.Equal("quest_crossing_the_vouch", failedQuestId);
            Assert.True(sys.IsQuestFailed("quest_crossing_the_vouch"));
            Assert.False(sys.IsQuestCompleted("quest_crossing_the_vouch"));
        }

        [Fact]
        public void FailQuest_PreventsAdvance_And_TickDailyRestart()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.FailQuest("quest_crossing_the_vouch");

            // Advance must fail
            Assert.Equal(-1, sys.AdvanceStage("quest_crossing_the_vouch"));

            // Daily tick must not restart a failed quest
            int startCount = 0;
            sys.OnQuestStarted += _ => startCount++;
            sys.TickDaily(11);
            Assert.Equal(0, startCount);
        }

        [Fact]
        public void SaveLoad_PreservesFailedState()
        {
            var sys = FreshSystem();
            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.FailQuest("quest_crossing_the_vouch");

            var saved = sys.CaptureState();
            Assert.Single(saved.quests);
            Assert.True(saved.quests[0].failed);

            var sys2 = FreshSystem();
            sys2.RestoreState(saved);
            Assert.True(sys2.IsQuestFailed("quest_crossing_the_vouch"));
            Assert.False(sys2.IsQuestCompleted("quest_crossing_the_vouch"));
        }

        [Fact]
        public void TickDaily_PostVouchGating_RequiresVouchOrOpeningCompletion()
        {
            var sys = FreshSystem();
            // Opening quest completed
            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");
            Assert.True(sys.IsQuestCompleted("quest_crossing_the_vouch"));

            // Next quest (min_day 20) with opening completed should auto-start on day 20
            sys.TickDaily(20, hasVouchAccess: false);
            Assert.True(sys.IsQuestStarted("quest_crossing_first_weigh"));
        }

        [Fact]
        public void TickDaily_PostVouchGating_WithVouchAccess_AutoStarts()
        {
            var sys = FreshSystem();
            // Start and complete opening quest
            sys.StartQuest("quest_crossing_the_vouch", 10);
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");
            sys.AdvanceStage("quest_crossing_the_vouch");

            sys.TickDaily(20, hasVouchAccess: true);
            Assert.True(sys.IsQuestStarted("quest_crossing_first_weigh"));
        }
    }
}
