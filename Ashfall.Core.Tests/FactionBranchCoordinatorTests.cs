// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FactionBranchCoordinatorTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (!CatalogLocator.TryFindDataDirectory(start, out var dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
            Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");
            return dir;
        }

        private static FactionBranchCoordinator CreateCoordinator(IFlagLedger? flags = null)
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return FactionBranchCoordinator.LoadFromData(DataDir(), fileIO, json, flags ?? new InMemoryFlagLedger());
        }

        private static MoralChoiceSystem MakeMoralChoice(int moralScore = 0, int seed = 1)
        {
            var moral = new MoralChoiceSystem(new StubRng(seed));
            moral.State.moralScore = moralScore;
            return moral;
        }

        [Fact]
        public void InitialState_AllConstituentSystemsInitializedAndUncommitted()
        {
            var coordinator = CreateCoordinator();

            Assert.Equal(FactionBranchKind.None, coordinator.ActiveFactionKind);
            Assert.Null(coordinator.ActiveBranchId);
            Assert.False(coordinator.IsCommitted);
            Assert.False(coordinator.IsPonrLocked);
            Assert.Null(coordinator.ResolvedEndingId);
            Assert.NotNull(coordinator.Military);
            Assert.NotNull(coordinator.Rebel);
            Assert.NotNull(coordinator.Independent);
            Assert.NotNull(coordinator.Prpf);
        }

        [Fact]
        public void MutualExclusivity_CommittingToMilitary_BlocksRebelAndIndependent()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice();

            var result = coordinator.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            Assert.True(result.IsSuccess);
            Assert.Equal(FactionBranchKind.Military, coordinator.ActiveFactionKind);
            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, coordinator.ActiveBranchId);
            Assert.True(coordinator.IsCommitted);

            // Attempting to commit to Rebel is blocked
            var rebResult = coordinator.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            Assert.False(rebResult.IsSuccess);
            Assert.Contains("mutually exclusive", rebResult.MessageKey);

            // Attempting to commit to Independent is blocked
            var indResult = coordinator.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            Assert.False(indResult.IsSuccess);
            Assert.Contains("mutually exclusive", indResult.MessageKey);

            // Coordinator remains committed to Military
            Assert.Equal(FactionBranchKind.Military, coordinator.ActiveFactionKind);
            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, coordinator.ActiveBranchId);
        }

        [Fact]
        public void MutualExclusivity_CommittingToRebel_BlocksMilitaryAndIndependent()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice();

            var result = coordinator.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            Assert.True(result.IsSuccess);
            Assert.Equal(FactionBranchKind.Rebel, coordinator.ActiveFactionKind);
            Assert.Equal(RebelBranchIds.BranchTrueRebel, coordinator.ActiveBranchId);

            // Blocked from Military
            var milResult = coordinator.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            Assert.False(milResult.IsSuccess);
            Assert.Contains("mutually exclusive", milResult.MessageKey);

            // Blocked from Independent
            var indResult = coordinator.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            Assert.False(indResult.IsSuccess);
            Assert.Contains("mutually exclusive", indResult.MessageKey);
        }

        [Fact]
        public void MutualExclusivity_CommittingToIndependent_BlocksMilitaryAndRebel()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice();

            var result = coordinator.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            Assert.True(result.IsSuccess);
            Assert.Equal(FactionBranchKind.Independent, coordinator.ActiveFactionKind);
            Assert.Equal(IndependentBranchIds.BranchSurvivor, coordinator.ActiveBranchId);

            // Blocked from Military
            var milResult = coordinator.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            Assert.False(milResult.IsSuccess);
            Assert.Contains("mutually exclusive", milResult.MessageKey);

            // Blocked from Rebel
            var rebResult = coordinator.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            Assert.False(rebResult.IsSuccess);
            Assert.Contains("mutually exclusive", rebResult.MessageKey);
        }

        [Fact]
        public void IdempotentCommit_SameBranch_ReturnsSuccess()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice();

            var r1 = coordinator.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            Assert.True(r1.IsSuccess);

            var r2 = coordinator.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            Assert.True(r2.IsSuccess);
            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, coordinator.ActiveBranchId);
        }

        [Fact]
        public void MoralityBandGating_OutsideRequiredBand_IsBlocked()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice(0); // starts at Neutral

            // MIL-4 Martyr requires positive or very_positive
            bool canCommit = coordinator.CanCommit(MilitaryBranchIds.BranchMartyr, moral, out var reason);
            Assert.False(canCommit);
            Assert.Contains("Requires morality band between", reason);

            var res = coordinator.CommitBranch(MilitaryBranchIds.BranchMartyr, moral);
            Assert.False(res.IsSuccess);
            Assert.False(coordinator.IsCommitted);
        }

        [Fact]
        public void IndependentGating_PrpfStandingRequirement_Enforced()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice(60); // Positive morality for IND-3 Peacekeeper

            // IND-3 Peacekeeper requires PRPF standing >= 20. Default PRPF standing is 0.
            bool canCommitDefault = coordinator.CanCommit(IndependentBranchIds.BranchPeacekeeperDiplomat, moral, out var reason);
            Assert.False(canCommitDefault);
            Assert.Contains("Requires PRPF standing >= 20", reason);

            // Modify PRPF standing
            coordinator.ModifyStanding(PrpfIds.FactionId, 25);
            Assert.Equal(25, coordinator.Prpf.Standing);

            bool canCommitAfter = coordinator.CanCommit(IndependentBranchIds.BranchPeacekeeperDiplomat, moral, out reason);
            Assert.True(canCommitAfter);

            var res = coordinator.CommitBranch(IndependentBranchIds.BranchPeacekeeperDiplomat, moral);
            Assert.True(res.IsSuccess);
            Assert.Equal(IndependentBranchIds.BranchPeacekeeperDiplomat, coordinator.ActiveBranchId);
        }

        [Fact]
        public void IndependentGating_DualHostilityRequirement_Enforced()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice(-60); // Evil band for IND-4 Exile

            // Default standings are 0 (neutral), not hostile
            bool canCommitDefault = coordinator.CanCommit(IndependentBranchIds.BranchExile, moral, out var reason);
            Assert.False(canCommitDefault);
            Assert.Contains("Requires hostile standing", reason);

            // Make hostile to military and rebel
            coordinator.ModifyStanding(MilitaryBranchIds.FactionId, -60);
            coordinator.ModifyStanding(RebelBranchIds.FactionId, -60);
            Assert.True(coordinator.Independent.IsHostileToMilitary);
            Assert.True(coordinator.Independent.IsHostileToRebel);

            bool canCommitAfter = coordinator.CanCommit(IndependentBranchIds.BranchExile, moral, out reason);
            Assert.True(canCommitAfter);

            var res = coordinator.CommitBranch(IndependentBranchIds.BranchExile, moral);
            Assert.True(res.IsSuccess);
            Assert.Equal(IndependentBranchIds.BranchExile, coordinator.ActiveBranchId);
        }

        [Fact]
        public void PrpfThirdPower_StandingAndAlignment_DurableAcrossBranches()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice();

            // Commit to Military
            coordinator.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            Assert.Equal(FactionBranchKind.Military, coordinator.ActiveFactionKind);

            // PRPF standing and alignment are still modifiable and persistent
            coordinator.ModifyStanding(PrpfIds.FactionId, 15);
            coordinator.ShiftFactionAlignment(PrpfIds.FactionId, -20);
            Assert.Equal(15, coordinator.Prpf.Standing);
            Assert.Equal(100, coordinator.Prpf.Alignment); // 120 - 20 = 100
        }

        [Fact]
        public void PrpfThirdPower_JoinAndOppose_RulesEnforced()
        {
            // Negative moral band cannot join
            var coordinatorBad = CreateCoordinator();
            var moralEvil = MakeMoralChoice(-60); // Evil
            bool joined = coordinatorBad.TryJoinPrpf(moralEvil);
            Assert.False(joined);
            Assert.False(coordinatorBad.Prpf.IsJoined);

            // Positive moral band can join
            var coordinatorGood = CreateCoordinator();
            var moralGood = MakeMoralChoice(60); // Positive
            bool joinedGood = coordinatorGood.TryJoinPrpf(moralGood);
            Assert.True(joinedGood);
            Assert.True(coordinatorGood.Prpf.IsJoined);

            // Oppose PRPF
            var coordinatorOpposed = CreateCoordinator();
            coordinatorOpposed.OpposePrpf();
            Assert.True(coordinatorOpposed.Prpf.IsOpposed);

            // Once opposed, cannot join even with VeryPositive
            var moralHero = MakeMoralChoice(120); // VeryPositive
            bool rejoin = coordinatorOpposed.TryJoinPrpf(moralHero);
            Assert.False(rejoin);
        }

        [Fact]
        public void PonrLocking_RequiresCommitment_AndLocksPonrOnActiveBranch()
        {
            var flags = new InMemoryFlagLedger();
            var coordinator = CreateCoordinator(flags);
            var moral = MakeMoralChoice();

            // Uncommitted -> cannot lock PoNR
            var lockUncommitted = coordinator.LockPonr(10);
            Assert.False(lockUncommitted.IsSuccess);

            // Commit to Rebel
            coordinator.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            var lockCommitted = coordinator.LockPonr(10);
            Assert.True(lockCommitted.IsSuccess);
            Assert.True(coordinator.IsPonrLocked);
            Assert.True(flags.IsSet(RebelBranchIds.FlagPonrTrueRebel));
        }

        [Fact]
        public void EndingResolution_RequiresPonr_AndResolvesMoralEnding()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice();

            coordinator.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);

            // Attempting to resolve ending before PoNR is blocked
            var resPrePonr = coordinator.ResolveEnding(moral);
            Assert.False(resPrePonr.IsSuccess);
            Assert.Null(coordinator.ResolvedEndingId);

            // Lock PoNR
            coordinator.LockPonr(15);

            // Resolve ending
            var resPostPonr = coordinator.ResolveEnding(moral);
            Assert.True(resPostPonr.IsSuccess);
            Assert.NotNull(coordinator.ResolvedEndingId);
            Assert.Equal(coordinator.ResolvedEndingId, coordinator.Military.ResolvedEndingId);
        }

        [Fact]
        public void RollbackSafety_FailedCommit_LeavesStateUncommitted()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice();

            // Invalid branch id
            var res = coordinator.CommitBranch("invalid_nonexistent_branch", moral);
            Assert.False(res.IsSuccess);
            Assert.False(coordinator.IsCommitted);
            Assert.Equal(FactionBranchKind.None, coordinator.ActiveFactionKind);
            Assert.Null(coordinator.ActiveBranchId);
        }

        [Fact]
        public void SaveAndReload_FullRoundTrip_PreservesAllStates()
        {
            var flagsA = new InMemoryFlagLedger();
            var coordinatorA = CreateCoordinator(flagsA);
            var moral = MakeMoralChoice();

            coordinatorA.CommitBranch(MilitaryBranchIds.BranchLoyalSoldier, moral);
            coordinatorA.ShiftFactionAlignment(MilitaryBranchIds.FactionId, 30);
            coordinatorA.ModifyStanding(PrpfIds.FactionId, 45);
            coordinatorA.ShiftFactionAlignment(PrpfIds.FactionId, 10);
            coordinatorA.LockPonr(12);

            var save = coordinatorA.CaptureState();

            // Restore into fresh coordinator
            var flagsB = new InMemoryFlagLedger();
            var coordinatorB = CreateCoordinator(flagsB);
            coordinatorB.RestoreState(save);

            Assert.Equal(FactionBranchKind.Military, coordinatorB.ActiveFactionKind);
            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, coordinatorB.ActiveBranchId);
            Assert.True(coordinatorB.IsPonrLocked);
            Assert.Equal(-50, coordinatorB.Military.MilitaryAlignment); // -80 + 30 = -50
            Assert.Equal(45, coordinatorB.Prpf.Standing);
            Assert.Equal(130, coordinatorB.Prpf.Alignment);
        }

        [Fact]
        public void SaveMigration_V1WithoutIndependent_LoadsCleanlyIntoV2()
        {
            var json = new SystemTextJsonSerializer();
            // A v1 save without independentBranch section
            var v1 = new WeightOfChoicesSaveV1
            {
                saveVersion = 1,
                militaryBranch = new MilitaryBranchSystemState
                {
                    branch = new MilitaryBranchRecord
                    {
                        branchId = MilitaryBranchIds.BranchLoyalSoldier,
                        committed = true,
                        ponrLocked = true,
                        ponrLockedDay = 5
                    },
                    militaryAlignment = new FactionAlignmentRecord
                    {
                        factionId = MilitaryBranchIds.FactionId,
                        alignment = 20
                    }
                },
                prpf = new PrpfSystemState
                {
                    standing = new PlayerFactionStandingRecord
                    {
                        factionId = PrpfIds.FactionId,
                        standing = 10
                    },
                    alignment = new FactionAlignmentRecord
                    {
                        factionId = PrpfIds.FactionId,
                        alignment = 120
                    }
                }
            };
            v1.Checksum = SaveChecksum.Compute(v1);
            string v1Json = json.Serialize(v1);

            var save = WeightOfChoicesSaveCodec.Decode(v1Json, json);
            Assert.NotNull(save);
            Assert.Equal(2, save!.saveVersion);
            Assert.NotNull(save.independentBranch);
            Assert.False(save.independentBranch.branch.committed);

            var coordinator = CreateCoordinator();
            coordinator.RestoreState(save);

            Assert.Equal(FactionBranchKind.Military, coordinator.ActiveFactionKind);
            Assert.Equal(MilitaryBranchIds.BranchLoyalSoldier, coordinator.ActiveBranchId);
            Assert.True(coordinator.IsPonrLocked);
            Assert.Equal(20, coordinator.Military.MilitaryAlignment);
            Assert.Equal(10, coordinator.Prpf.Standing);
        }

        [Fact]
        public void UIQueries_GetBranchOptionsAndFactionStandingSummaries_ReturnAccurateData()
        {
            var coordinator = CreateCoordinator();
            var moral = MakeMoralChoice();

            var options = coordinator.GetBranchOptions(moral);
            Assert.Equal(24, options.Count); // 8 Military + 8 Rebel + 8 Independent

            var standings = coordinator.GetFactionStandingSummaries();
            Assert.Equal(3, standings.Count); // Military, Rebel, PRPF
            Assert.Contains(standings, s => s.FactionId == MilitaryBranchIds.FactionId);
            Assert.Contains(standings, s => s.FactionId == RebelBranchIds.FactionId);
            Assert.Contains(standings, s => s.FactionId == PrpfIds.FactionId);
        }

        [Fact]
        public void CatalogIntegrity_AllEmittedBranchEndingAndFlagIds_FollowConventions()
        {
            var coordinator = CreateCoordinator();
            var options = coordinator.GetBranchOptions(null);

            foreach (var opt in options)
            {
                Assert.StartsWith("branch_", opt.BranchId);
                Assert.StartsWith("flag_branch_", opt.PonrFlag);
                Assert.EndsWith("_ponr", opt.PonrFlag);
                Assert.False(string.IsNullOrEmpty(opt.DisplayName));
                Assert.False(string.IsNullOrEmpty(opt.ConsequencesSummary));
                Assert.NotEmpty(opt.PossibleEndings);
            }
        }
    }
}
