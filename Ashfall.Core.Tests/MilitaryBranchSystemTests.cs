using System;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MilitaryBranchSystemTests
    {
        private static MilitaryBranchCatalog LoadCatalog()
        {
            string start = System.IO.Directory.GetCurrentDirectory();
            string dir;
            if (!CatalogLocator.TryFindDataDirectory(start, out dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
            Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");
            return MilitaryBranchCatalog.LoadAndRegister(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static MoralChoiceSystem MakeMoralChoice(int seed = 1) =>
            new MoralChoiceSystem(new StubRng(seed));

        [Fact]
        public void Catalog_LoadsAllEightBranchesWithThreeEndingsEach()
        {
            var catalog = LoadCatalog();
            Assert.Equal(MilitaryBranchIds.BranchCount, catalog.Count);
            foreach (var branchId in MilitaryBranchIds.AllBranches)
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
            var system = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoice(); // starts at Neutral band

            string committed = system.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);

            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, committed);
            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, system.CommittedBranchId);
        }

        [Fact]
        public void CommitBranch_OutsideEntryBand_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            // MIL-4 Martyr requires a positive-or-better starting band; a fresh
            // MoralChoiceSystem starts at Neutral, which is outside that range.
            Assert.Throws<InvalidOperationException>(() =>
                system.CommitBranch(MilitaryBranchIds.BranchMartyr, moral));
        }

        [Fact]
        public void CommitBranch_CalledTwice_KeepsFirstCommitment()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            string second = system.CommitBranch(MilitaryBranchIds.BranchDeserter, moral);

            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, second);
            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, system.CommittedBranchId);
        }

        [Fact]
        public void LockPointOfNoReturn_BeforeCommit_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);

            Assert.Throws<InvalidOperationException>(() => system.LockPointOfNoReturn());
        }

        [Fact]
        public void LockPointOfNoReturn_SetsDurableAndRuntimeFlag()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            system.AdvanceDay(65);
            system.LockPointOfNoReturn();

            Assert.True(system.IsPonrLocked);
            Assert.True(flags.IsSet(MilitaryBranchIds.FlagPonrLoyalSoldier));
            Assert.Contains(MilitaryBranchIds.FlagPonrLoyalSoldier, system.State.setFlags);
            Assert.Equal(65, system.State.branch.ponrLockedDay);
        }

        [Fact]
        public void LockPointOfNoReturn_CalledTwice_IsANoOp()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            system.AdvanceDay(10);
            system.LockPointOfNoReturn();
            system.AdvanceDay(20);
            system.LockPointOfNoReturn(); // should not move the lock day forward

            Assert.Equal(10, system.State.branch.ponrLockedDay);
        }

        [Fact]
        public void ResolveEnding_BeforePonrLocked_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);

            Assert.Throws<InvalidOperationException>(() => system.ResolveEnding(moral));
        }

        [Fact]
        public void ResolveEnding_NeutralBand_ResolvesSurvivorKingEnding()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoice(); // Neutral band, never resolved a quest

            system.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            system.LockPointOfNoReturn();
            string ending = system.ResolveEnding(moral);

            Assert.Equal(MilitaryBranchIds.EndingLoyalSoldierC, ending);
            Assert.Equal(ending, system.ResolvedEndingId);
        }

        [Fact]
        public void ResolveEnding_IsIdempotent_EvenIfMoralityDriftsAfterward()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            system.LockPointOfNoReturn();
            string first = system.ResolveEnding(moral);

            // Push morality to a wildly different band via a scripted quest choice.
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
            var system = new MilitaryBranchSystem(catalog, flags);

            system.ShiftFactionAlignment(-500);
            Assert.Equal(MilitaryBranchSystem.MinAlignment, system.MilitaryAlignment);

            system.ShiftFactionAlignment(1000);
            Assert.Equal(MilitaryBranchSystem.MaxAlignment, system.MilitaryAlignment);
        }

        [Fact]
        public void IsGameOver_ZeroOrNegativeSurvivors_IsTrue()
        {
            Assert.True(MilitaryBranchSystem.IsGameOver(0));
            Assert.True(MilitaryBranchSystem.IsGameOver(-1));
            Assert.False(MilitaryBranchSystem.IsGameOver(1));
        }

        [Fact]
        public void SaveRoundTrip_PreservesBranchTimelineAlignmentAndFlags()
        {
            var catalog = LoadCatalog();
            var flagsA = new InMemoryFlagLedger();
            var systemA = new MilitaryBranchSystem(catalog, flagsA);
            var moral = MakeMoralChoice();

            systemA.AdvanceDay(30);
            systemA.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            systemA.AdvanceDay(65);
            systemA.LockPointOfNoReturn();
            systemA.ShiftFactionAlignment(40);
            systemA.ResolveEnding(moral);

            var save = MilitaryBranchSaveCodec.Capture(systemA);
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = MilitaryBranchSaveCodec.Encode(save, jsonSerializer);
            Assert.Contains(MilitaryBranchIds.BranchLoyalSoldier, jsonText);

            var loaded = MilitaryBranchSaveCodec.Decode(jsonText, jsonSerializer);

            var flagsB = new InMemoryFlagLedger();
            var systemB = new MilitaryBranchSystem(catalog, flagsB);
            MilitaryBranchSaveCodec.Restore(loaded, systemB);

            Assert.Equal(65, systemB.CurrentDay);
            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, systemB.CommittedBranchId);
            Assert.True(systemB.IsPonrLocked);
            Assert.Equal(-40, systemB.MilitaryAlignment); // -80 default + 40 shift
            Assert.Equal(systemA.ResolvedEndingId, systemB.ResolvedEndingId);
            Assert.True(flagsB.IsSet(MilitaryBranchIds.FlagPonrLoyalSoldier));
        }

        [Fact]
        public void Decode_TamperedChecksum_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();
            system.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);

            var save = MilitaryBranchSaveCodec.Capture(system);
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = MilitaryBranchSaveCodec.Encode(save, jsonSerializer);

            string tampered = jsonText.Replace(MilitaryBranchIds.BranchLoyalSoldier, MilitaryBranchIds.BranchDeserter);

            Assert.Throws<InvalidOperationException>(() => MilitaryBranchSaveCodec.Decode(tampered, jsonSerializer));
        }

        [Fact]
        public void Decode_NewerSaveVersion_Throws()
        {
            var save = new MilitaryBranchSave { saveVersion = 99 };
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = MilitaryBranchSaveCodec.Encode(save, jsonSerializer);

            Assert.Throws<InvalidOperationException>(() => MilitaryBranchSaveCodec.Decode(jsonText, jsonSerializer));
        }

        [Fact]
        public void RestoreState_WrongSystemId_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new MilitaryBranchSystem(catalog, flags);
            var badState = new MilitaryBranchSystemState { systemId = "not_the_right_system" };

            Assert.Throws<ArgumentException>(() => system.RestoreState(badState));
        }
    }
}
