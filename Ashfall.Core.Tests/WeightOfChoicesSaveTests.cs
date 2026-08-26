using System;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests for the combined Military + Rebel + Independent + PRPF save
    /// envelope (WeightOfChoicesSaveCodec) that a host session uses instead
    /// of juggling four separate codecs.
    /// </summary>
    public class WeightOfChoicesSaveTests
    {
        private static string DataDir()
        {
            string start = System.IO.Directory.GetCurrentDirectory();
            string dir;
            if (!CatalogLocator.TryFindDataDirectory(start, out dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
            return dir;
        }

        [Fact]
        public void Capture_WithOnlyMilitaryCommitted_OtherSectionsStayAtDefaults()
        {
            var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var rebelCatalog = RebelBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var independentCatalog = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            var military = new MilitaryBranchSystem(militaryCatalog, new InMemoryFlagLedger());
            var rebel = new RebelBranchSystem(rebelCatalog, new InMemoryFlagLedger());
            var independent = new IndependentBranchSystem(independentCatalog, new InMemoryFlagLedger());
            var prpf = new PrpfStandingSystem(new InMemoryFlagLedger());
            var moral = new MoralChoiceSystem(new StubRng(1));

            military.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);

            var save = WeightOfChoicesSaveCodec.Capture(military, rebel, independent, prpf);

            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, save.militaryBranch.branch.branchId);
            Assert.True(save.militaryBranch.branch.committed);
            Assert.False(save.rebelBranch.branch.committed);
            Assert.Equal(string.Empty, save.rebelBranch.branch.branchId);
            Assert.False(save.independentBranch.branch.committed);
            Assert.Equal(string.Empty, save.independentBranch.branch.branchId);
        }

        [Fact]
        public void RoundTrip_RestoresAllFourSystemsIntoFreshInstances()
        {
            var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var rebelCatalog = RebelBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var independentCatalog = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            var militaryA = new MilitaryBranchSystem(militaryCatalog, new InMemoryFlagLedger());
            var rebelA = new RebelBranchSystem(rebelCatalog, new InMemoryFlagLedger());
            var independentA = new IndependentBranchSystem(independentCatalog, new InMemoryFlagLedger());
            var prpfA = new PrpfStandingSystem(new InMemoryFlagLedger());
            var moral = new MoralChoiceSystem(new StubRng(1));

            militaryA.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            militaryA.LockPointOfNoReturn();
            prpfA.ModifyStanding(40);
            independentA.ModifyMilitaryStanding(-10);

            var save = WeightOfChoicesSaveCodec.Capture(militaryA, rebelA, independentA, prpfA);
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = WeightOfChoicesSaveCodec.Encode(save, jsonSerializer);
            var loaded = WeightOfChoicesSaveCodec.Decode(jsonText, jsonSerializer);

            var militaryB = new MilitaryBranchSystem(militaryCatalog, new InMemoryFlagLedger());
            var rebelB = new RebelBranchSystem(rebelCatalog, new InMemoryFlagLedger());
            var independentB = new IndependentBranchSystem(independentCatalog, new InMemoryFlagLedger());
            var prpfB = new PrpfStandingSystem(new InMemoryFlagLedger());
            WeightOfChoicesSaveCodec.Restore(loaded, militaryB, rebelB, independentB, prpfB);

            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, militaryB.CommittedBranchId);
            Assert.True(militaryB.IsPonrLocked);
            Assert.Null(rebelB.CommittedBranchId);
            Assert.Null(independentB.CommittedBranchId);
            Assert.Equal(-10, independentB.MilitaryStanding);
            Assert.Equal(40, prpfB.Standing);
        }

        [Fact]
        public void HasConflictingFactionCommitment_TwoCommitted_ReturnsTrue()
        {
            var save = new WeightOfChoicesSave();
            save.militaryBranch.branch.committed = true;
            save.militaryBranch.branch.branchId = MilitaryBranchIds.BranchLoyalSoldier;
            save.rebelBranch.branch.committed = true;
            save.rebelBranch.branch.branchId = RebelBranchIds.BranchTrueRebel;

            Assert.True(WeightOfChoicesSaveCodec.HasConflictingFactionCommitment(save));
        }

        [Fact]
        public void HasConflictingFactionCommitment_AllThreeCommitted_ReturnsTrue()
        {
            var save = new WeightOfChoicesSave();
            save.militaryBranch.branch.committed = true;
            save.rebelBranch.branch.committed = true;
            save.independentBranch.branch.committed = true;

            Assert.True(WeightOfChoicesSaveCodec.HasConflictingFactionCommitment(save));
        }

        [Fact]
        public void HasConflictingFactionCommitment_OnlyOneCommitted_ReturnsFalse()
        {
            var save = new WeightOfChoicesSave();
            save.militaryBranch.branch.committed = true;
            save.militaryBranch.branch.branchId = MilitaryBranchIds.BranchLoyalSoldier;

            Assert.False(WeightOfChoicesSaveCodec.HasConflictingFactionCommitment(save));
        }

        [Fact]
        public void HasConflictingFactionCommitment_NoneCommitted_ReturnsFalse()
        {
            var save = new WeightOfChoicesSave();
            Assert.False(WeightOfChoicesSaveCodec.HasConflictingFactionCommitment(save));
        }

        [Fact]
        public void Decode_NewerSaveVersion_Throws()
        {
            var save = new WeightOfChoicesSave { saveVersion = 99 };
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = WeightOfChoicesSaveCodec.Encode(save, jsonSerializer);

            Assert.Throws<InvalidOperationException>(() => WeightOfChoicesSaveCodec.Decode(jsonText, jsonSerializer));
        }

        [Fact]
        public void Decode_TamperedChecksum_Throws()
        {
            var save = new WeightOfChoicesSave();
            save.prpf.standing.standing = 5;
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = WeightOfChoicesSaveCodec.Encode(save, jsonSerializer);
            string tampered = jsonText.Replace("\"standing\":5", "\"standing\":99");

            Assert.Throws<InvalidOperationException>(() => WeightOfChoicesSaveCodec.Decode(tampered, jsonSerializer));
        }

        [Fact]
        public void Decode_V1Envelope_MigratesWithFreshIndependentSection()
        {
            // Build a v1 envelope by hand (Military + Rebel + PRPF only, as the
            // pre-Independent-slice shape actually wrote) and confirm it upgrades
            // cleanly instead of failing the checksum check against the v2 shape.
            var v1 = new WeightOfChoicesSaveV1();
            v1.militaryBranch.branch.committed = true;
            v1.militaryBranch.branch.branchId = MilitaryBranchIds.BranchLoyalSoldier;
            v1.prpf.standing.standing = 25;
            v1.Checksum = SaveChecksum.Compute(v1);

            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = jsonSerializer.Serialize(v1);

            var migrated = WeightOfChoicesSaveCodec.Decode(jsonText, jsonSerializer);

            Assert.Equal(WeightOfChoicesSave.CurrentSaveVersion, migrated.saveVersion);
            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, migrated.militaryBranch.branch.branchId);
            Assert.Equal(25, migrated.prpf.standing.standing);
            Assert.False(migrated.independentBranch.branch.committed);
            Assert.Equal(string.Empty, migrated.independentBranch.branch.branchId);
        }
    }
}
