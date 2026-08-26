using System;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class RebelBranchSystemTests
    {
        private static RebelBranchCatalog LoadCatalog()
        {
            string start = System.IO.Directory.GetCurrentDirectory();
            string dir;
            if (!CatalogLocator.TryFindDataDirectory(start, out dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
            Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");
            return RebelBranchCatalog.LoadAndRegister(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static MoralChoiceSystem MakeMoralChoice(int seed = 1) =>
            new MoralChoiceSystem(new StubRng(seed));

        [Fact]
        public void Catalog_LoadsAllEightBranchesWithThreeEndingsEach()
        {
            var catalog = LoadCatalog();
            Assert.Equal(RebelBranchIds.BranchCount, catalog.Count);
            foreach (var branchId in RebelBranchIds.AllBranches)
            {
                var entry = catalog.GetById(branchId);
                Assert.NotNull(entry);
                Assert.Equal(3, entry!.endings.Count);
            }
        }

        [Fact]
        public void CommitBranch_WithinEntryBand_Succeeds()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var moral = MakeMoralChoice(); // starts at Neutral band

            string committed = system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);

            Assert.Equal(RebelBranchIds.BranchTrueRebel, committed);
            Assert.Equal(RebelBranchIds.BranchTrueRebel, system.CommittedBranchId);
        }

        [Fact]
        public void CommitBranch_OutsideEntryBand_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            // REB-4 Martyr requires a positive-or-better starting band; a fresh
            // MoralChoiceSystem starts at Neutral, which is outside that range.
            Assert.Throws<InvalidOperationException>(() =>
                system.CommitBranch(RebelBranchIds.BranchMartyr, moral));
        }

        [Fact]
        public void CommitBranch_CalledTwice_KeepsFirstCommitment()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            string second = system.CommitBranch(RebelBranchIds.BranchLoneWolf, moral);

            Assert.Equal(RebelBranchIds.BranchTrueRebel, second);
            Assert.Equal(RebelBranchIds.BranchTrueRebel, system.CommittedBranchId);
        }

        [Fact]
        public void LockPointOfNoReturn_BeforeCommit_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);

            Assert.Throws<InvalidOperationException>(() => system.LockPointOfNoReturn());
        }

        [Fact]
        public void LockPointOfNoReturn_SetsDurableAndRuntimeFlag()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            system.AdvanceDay(55);
            system.LockPointOfNoReturn();

            Assert.True(system.IsPonrLocked);
            Assert.True(flags.IsSet(RebelBranchIds.FlagPonrTrueRebel));
            Assert.Contains(RebelBranchIds.FlagPonrTrueRebel, system.State.setFlags);
            Assert.Equal(55, system.State.branch.ponrLockedDay);
        }

        [Fact]
        public void LockPointOfNoReturn_CalledTwice_IsANoOp()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            system.AdvanceDay(10);
            system.LockPointOfNoReturn();
            system.AdvanceDay(20);
            system.LockPointOfNoReturn();

            Assert.Equal(10, system.State.branch.ponrLockedDay);
        }

        [Fact]
        public void ResolveEnding_BeforePonrLocked_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);

            Assert.Throws<InvalidOperationException>(() => system.ResolveEnding(moral));
        }

        [Fact]
        public void ResolveEnding_NeutralBand_ResolvesSurvivorEnding()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var moral = MakeMoralChoice(); // Neutral band

            system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            system.LockPointOfNoReturn();
            string ending = system.ResolveEnding(moral);

            Assert.Equal(RebelBranchIds.EndingTrueRebelC, ending);
            Assert.Equal(ending, system.ResolvedEndingId);
        }

        [Fact]
        public void ResolveEnding_IsIdempotent_EvenIfMoralityDriftsAfterward()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            system.LockPointOfNoReturn();
            string first = system.ResolveEnding(moral);

            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_share_child",
                Choices = { new MoralChoiceOption { MoralDelta = 150, EmpathyDelta = 1 } }
            };
            moral.Resolve(quest, 0, "loc_test", 1);

            string second = system.ResolveEnding(moral);
            Assert.Equal(first, second);
        }

        [Fact]
        public void ShiftFactionAlignment_ClampsToRange()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);

            system.ShiftFactionAlignment(-500);
            Assert.Equal(RebelBranchSystem.MinAlignment, system.RebelAlignment);

            system.ShiftFactionAlignment(1000);
            Assert.Equal(RebelBranchSystem.MaxAlignment, system.RebelAlignment);
        }

        [Fact]
        public void IsGameOver_ZeroOrNegativeSurvivors_IsTrue()
        {
            Assert.True(RebelBranchSystem.IsGameOver(0));
            Assert.True(RebelBranchSystem.IsGameOver(-1));
            Assert.False(RebelBranchSystem.IsGameOver(1));
        }

        [Fact]
        public void SaveRoundTrip_PreservesBranchTimelineAlignmentAndFlags()
        {
            var catalog = LoadCatalog();
            var flagsA = new InMemoryFlagLedger();
            var systemA = new RebelBranchSystem(catalog, flagsA);
            var moral = MakeMoralChoice();

            systemA.AdvanceDay(25);
            systemA.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            systemA.AdvanceDay(55);
            systemA.LockPointOfNoReturn();
            systemA.ShiftFactionAlignment(30);
            systemA.ResolveEnding(moral);

            var save = RebelBranchSaveCodec.Capture(systemA);
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = RebelBranchSaveCodec.Encode(save, jsonSerializer);
            Assert.Contains(RebelBranchIds.BranchTrueRebel, jsonText);

            var loaded = RebelBranchSaveCodec.Decode(jsonText, jsonSerializer);

            var flagsB = new InMemoryFlagLedger();
            var systemB = new RebelBranchSystem(catalog, flagsB);
            RebelBranchSaveCodec.Restore(loaded, systemB);

            Assert.Equal(55, systemB.CurrentDay);
            Assert.Equal(RebelBranchIds.BranchTrueRebel, systemB.CommittedBranchId);
            Assert.True(systemB.IsPonrLocked);
            Assert.Equal(-50, systemB.RebelAlignment); // -80 default + 30 shift
            Assert.Equal(systemA.ResolvedEndingId, systemB.ResolvedEndingId);
            Assert.True(flagsB.IsSet(RebelBranchIds.FlagPonrTrueRebel));
        }

        [Fact]
        public void Decode_TamperedChecksum_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();
            system.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);

            var save = RebelBranchSaveCodec.Capture(system);
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = RebelBranchSaveCodec.Encode(save, jsonSerializer);

            string tampered = jsonText.Replace(RebelBranchIds.BranchTrueRebel, RebelBranchIds.BranchLoneWolf);

            Assert.Throws<InvalidOperationException>(() => RebelBranchSaveCodec.Decode(tampered, jsonSerializer));
        }

        [Fact]
        public void Decode_NewerSaveVersion_Throws()
        {
            var save = new RebelBranchSave { saveVersion = 99 };
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = RebelBranchSaveCodec.Encode(save, jsonSerializer);

            Assert.Throws<InvalidOperationException>(() => RebelBranchSaveCodec.Decode(jsonText, jsonSerializer));
        }

        [Fact]
        public void RestoreState_WrongSystemId_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            var badState = new RebelBranchSystemState { systemId = "not_the_right_system" };

            Assert.Throws<ArgumentException>(() => system.RestoreState(badState));
        }
    }
}
