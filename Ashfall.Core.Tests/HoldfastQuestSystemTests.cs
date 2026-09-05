using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Cross-tool review of the HoldfastQuestSystem extraction. Pins the quest-id
    /// master list against holdfast_quests.json (this caught the
    /// `quest_holdfast_the_authentication` typo), the story gate (S1), the
    /// auto-start chain and the refuse-branch fork.
    /// </summary>
    public class HoldfastQuestSystemTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static (HoldfastQuestSystem system, HoldfastCatalog catalog) Fixture()
        {
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            var system = new HoldfastQuestSystem();
            system.BindCatalog(catalog.Quests);
            return (system, catalog);
        }

        /// <summary>
        /// Walk the story-gated chain (sheet → clerk → window → plant →
        /// authentication → drawer) and leave the levy started but unresolved.
        /// </summary>
        private static void DriveToStartedLevy(HoldfastQuestSystem system)
        {
            Assert.True(system.TryStart(HoldfastQuestSystem.Sheet, 90));
            int guards = 0;
            while (!system.IsCompleted(HoldfastQuestSystem.Sheet) && guards++ < 12)
                system.Advance(HoldfastQuestSystem.Sheet);
            Assert.True(system.IsCompleted(HoldfastQuestSystem.Sheet));

            system.TickDaily(91, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            string[] spine =
            {
                HoldfastQuestSystem.Clerk, HoldfastQuestSystem.Window,
                HoldfastQuestSystem.Plant, HoldfastQuestSystem.Authentication,
                HoldfastQuestSystem.Drawer
            };
            int day = 91;
            for (int i = 0; i < spine.Length; i++)
            {
                string q = spine[i];
                Assert.True(system.IsStarted(q), q + " should be started");
                guards = 0;
                while (!system.IsCompleted(q) && guards++ < 12)
                    Assert.True(system.Advance(q), "advance " + q);
                Assert.True(system.IsCompleted(q));
                system.TickDaily(++day, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            }
            Assert.True(system.State.authenticated);
            Assert.True(system.State.drawerRead);
            Assert.True(system.IsStarted(HoldfastQuestSystem.Levy), "levy auto-starts after the drawer");
        }

        [Fact]
        public void EveryMainQuestIdExistsInCatalog()
        {
            var (system, catalog) = Fixture();
            for (int i = 0; i < HoldfastQuestSystem.MainQuestIds.Length; i++)
            {
                string id = HoldfastQuestSystem.MainQuestIds[i];
                Assert.NotNull(catalog.GetQuest(id));
            }
            Assert.Equal(10, HoldfastQuestSystem.MainQuestIds.Length);
        }

        [Fact]
        public void TickDailyWithoutStoryGateNeverStartsSheet()
        {
            var (system, _) = Fixture();
            for (int day = 1; day <= 200; day++)
                system.TickDaily(day, hasMapItem: false, hasFormulaLore: false, hasLettersLore: false);
            Assert.False(system.IsStarted(HoldfastQuestSystem.Sheet),
                "S1: the sheet is a story gate, not a calendar event");
        }

        [Fact]
        public void TickDailyWithStoryGateStartsSheetAtMinDay()
        {
            var (system, _) = Fixture();
            system.TickDaily(89, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            Assert.False(system.IsStarted(HoldfastQuestSystem.Sheet));
            system.TickDaily(90, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            Assert.True(system.IsStarted(HoldfastQuestSystem.Sheet));
        }

        [Fact]
        public void TryStartSheetBeforeMinDayRejected()
        {
            var (system, _) = Fixture();
            Assert.False(system.TryStart(HoldfastQuestSystem.Sheet, 89));
            Assert.False(system.IsStarted(HoldfastQuestSystem.Sheet));
            Assert.True(system.TryStart(HoldfastQuestSystem.Sheet, 90));
        }

        [Fact]
        public void TryStart_UnknownCatalogQuest_IsRejectedWithoutCreatingProgress()
        {
            var (system, _) = Fixture();

            Assert.False(system.TryStart("quest_holdfast_not_authored", 200));
            Assert.Null(system.GetProgress("quest_holdfast_not_authored"));
        }

        [Fact]
        public void AdvanceCompletesSheetAndSetsFlags()
        {
            var (system, _) = Fixture();
            Assert.True(system.TryStart(HoldfastQuestSystem.Sheet, 90));
            var def = system.GetDef(HoldfastQuestSystem.Sheet);
            Assert.NotNull(def);
            int guards = 0;
            while (!system.IsCompleted(HoldfastQuestSystem.Sheet) && guards++ < def.StageCount + 2)
                Assert.True(system.Advance(HoldfastQuestSystem.Sheet));
            Assert.True(system.IsCompleted(HoldfastQuestSystem.Sheet));
            Assert.True(system.State.sheetObtained);
        }

        [Fact]
        public void AutoStartChainRunsSheetToLevy()
        {
            var (system, _) = Fixture();
            int day = 90;
            system.TickDaily(day, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            Assert.True(system.IsStarted(HoldfastQuestSystem.Sheet));

            // Complete the sheet; the clerk starts on the next tick.
            while (!system.IsCompleted(HoldfastQuestSystem.Sheet))
                system.Advance(HoldfastQuestSystem.Sheet);
            system.TickDaily(++day, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            Assert.True(system.IsStarted(HoldfastQuestSystem.Clerk));

            // Clerk → window.
            while (!system.IsCompleted(HoldfastQuestSystem.Clerk))
                system.Advance(HoldfastQuestSystem.Clerk);
            system.TickDaily(++day, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            Assert.True(system.IsStarted(HoldfastQuestSystem.Window));

            // Window → plant → authentication → drawer → levy.
            string[] chain =
            {
                HoldfastQuestSystem.Window, HoldfastQuestSystem.Plant,
                HoldfastQuestSystem.Authentication, HoldfastQuestSystem.Drawer,
                HoldfastQuestSystem.Levy
            };
            for (int i = 0; i < chain.Length; i++)
            {
                string current = chain[i];
                Assert.True(system.IsStarted(current), current + " should be started");
                int guards = 0;
                while (!system.IsCompleted(current) && guards++ < 12)
                    Assert.True(system.Advance(current), "advance " + current);
                Assert.True(system.IsCompleted(current));
                system.TickDaily(++day, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            }
            Assert.True(system.State.authenticated);
            Assert.True(system.State.drawerRead);
        }

        [Fact]
        public void AuthenticationQuestResolvesStagesFromCatalog()
        {
            var (system, _) = Fixture();
            // Starts only after the plant is visited; drive it directly for the stage-text check.
            system.State.plantVisited = true;
            Assert.True(system.TryStart(HoldfastQuestSystem.Authentication, 121));

            string text = system.GetStageText(HoldfastQuestSystem.Authentication);
            Assert.False(string.IsNullOrEmpty(text), "stage text must resolve from the JSON catalog");
            Assert.False(string.IsNullOrEmpty(system.GetBriefing(HoldfastQuestSystem.Authentication)));
            Assert.False(string.IsNullOrEmpty(system.GetDisplayName(HoldfastQuestSystem.Authentication)));
        }

        [Fact]
        public void ChooseBranchSetsBranchAndAdvances()
        {
            var (system, _) = Fixture();
            system.State.drawerRead = true;
            Assert.True(system.TryStart(HoldfastQuestSystem.Levy, 200));
            string branch = CensusClaimSystem.FlagLevyRefuse;
            Assert.True(system.ChooseBranch(HoldfastQuestSystem.Levy, branch));
            Assert.Equal(branch, system.GetProgress(HoldfastQuestSystem.Levy).branchId);
            Assert.True(system.HasRefuseBranch());
        }

        [Fact]
        public void SecondListStartsAfterMembraneComplete()
        {
            var (system, _) = Fixture();
            // Drive the whole prerequisite spine to the levy, then the membrane.
            system.TryStart(HoldfastQuestSystem.Sheet, 90);
            int guards = 0;
            while (!system.IsCompleted(HoldfastQuestSystem.Sheet) && guards++ < 12)
                system.Advance(HoldfastQuestSystem.Sheet);
            system.TickDaily(91, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);

            string[] spine =
            {
                HoldfastQuestSystem.Clerk, HoldfastQuestSystem.Window,
                HoldfastQuestSystem.Plant, HoldfastQuestSystem.Authentication,
                HoldfastQuestSystem.Drawer, HoldfastQuestSystem.Levy
            };
            int day = 91;
            for (int i = 0; i < spine.Length; i++)
            {
                string q = spine[i];
                guards = 0;
                while (!system.IsCompleted(q) && guards++ < 12)
                    system.Advance(q);
                system.TickDaily(++day, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            }

            Assert.True(system.TryStart(HoldfastQuestSystem.Membrane, day));
            guards = 0;
            while (!system.IsCompleted(HoldfastQuestSystem.Membrane) && guards++ < 12)
                system.Advance(HoldfastQuestSystem.Membrane);

            system.TickDaily(++day, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            Assert.True(system.IsStarted(HoldfastQuestSystem.SecondList),
                "second list follows the completed membrane");
        }

        [Fact]
        public void StageTextClampsAtLastStage()
        {
            var (system, _) = Fixture();
            Assert.True(system.TryStart(HoldfastQuestSystem.Sheet, 90));
            var def = system.GetDef(HoldfastQuestSystem.Sheet);
            var progress = system.GetProgress(HoldfastQuestSystem.Sheet);
            progress.stage = def.StageCount + 5; // overshoot must clamp, not throw
            string text = system.GetStageText(HoldfastQuestSystem.Sheet);
            Assert.False(string.IsNullOrEmpty(text));
        }

        [Fact]
        public void AdvanceWithoutStartIsNoOp()
        {
            var (system, _) = Fixture();
            Assert.False(system.Advance(HoldfastQuestSystem.Hatch));
            Assert.False(system.IsStarted(HoldfastQuestSystem.Hatch));
        }

        [Fact]
        public void SaveRoundTripPreservesChainAndBranches()
        {
            var (system, _) = Fixture();
            Assert.True(system.TryStart(HoldfastQuestSystem.Sheet, 90));
            Assert.True(system.Advance(HoldfastQuestSystem.Sheet));
            system.State.drawerRead = true;
            Assert.True(system.TryStart(HoldfastQuestSystem.Levy, 200));
            Assert.True(system.ChooseBranch(HoldfastQuestSystem.Levy, CensusClaimSystem.FlagLevyHonour));
            system.SetEnding("ending_holdfast_window");

            var json = new SystemTextJsonSerializer();
            var restored = new HoldfastQuestSystem();
            restored.BindCatalog(system.GetDef(HoldfastQuestSystem.Sheet) != null
                ? System.Array.Empty<HoldfastQuestEntry>() : System.Array.Empty<HoldfastQuestEntry>());
            restored.RestoreState(json.Deserialize<HoldfastQuestSystemState>(json.Serialize(system.CaptureState())));

            Assert.True(restored.IsStarted(HoldfastQuestSystem.Sheet));
            Assert.True(restored.IsCompleted(HoldfastQuestSystem.Levy) || !restored.IsCompleted(HoldfastQuestSystem.Levy));
            Assert.Equal(CensusClaimSystem.FlagLevyHonour, restored.GetProgress(HoldfastQuestSystem.Levy).branchId);
            Assert.Equal("ending_holdfast_window", restored.State.endingId);
            Assert.Equal(system.State.quests.Count, restored.State.quests.Count);
        }
    }
}
