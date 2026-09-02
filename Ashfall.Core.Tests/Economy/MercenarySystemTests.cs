// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public class MercenarySystemTests
    {
        [Fact]
        public void MercenarySystem_GenerateBoard_CreatesDeterministicContracts()
        {
            var rng = new SeededRng(188);
            var inv = new Inventory.Inventory();
            var system = new MercenarySystem(rng, inv);

            var targets = new List<string> { "target_warlord_kane", "target_marauder_voss" };
            system.GenerateBoard(1, targets);

            Assert.NotEmpty(system.ActiveContracts);
            foreach (var c in system.ActiveContracts)
            {
                Assert.Equal(BountyContractStatus.Open, c.status);
                Assert.True(c.rewardAmount > 0f);
                Assert.Contains(c.targetId, targets);
            }
        }

        [Fact]
        public void MercenarySystem_AcceptContract_RevealsTargetIntelAndAssignsPlayer()
        {
            var rng = new SeededRng(188);
            var inv = new Inventory.Inventory();
            var system = new MercenarySystem(rng, inv);

            system.GenerateBoard(1, new List<string> { "target_warlord_kane" });
            var contract = system.ActiveContracts[0];

            bool intelFired = false;
            system.OnTargetIntelUpdated += (_) => intelFired = true;

            var result = system.AcceptContract(contract.contractId, 1);

            Assert.True(result.IsSuccess);
            Assert.Equal(BountyContractStatus.Accepted, contract.status);
            Assert.True(contract.acceptedByPlayer);
            Assert.True(intelFired);
            Assert.NotEmpty(system.State.intel);
        }

        [Fact]
        public void MercenarySystem_ClaimReward_RequiresProofItem_AwardsScrapAtomically()
        {
            var rng = new SeededRng(188);
            var inv = new Inventory.Inventory();
            var system = new MercenarySystem(rng, inv);

            system.GenerateBoard(1, new List<string> { "target_warlord_kane" });
            var contract = system.ActiveContracts[0];
            system.AcceptContract(contract.contractId, 1);

            // Attempt claim without required proof item
            var failClaim = system.ClaimReward(contract.contractId);
            Assert.False(failClaim.IsSuccess);
            Assert.Equal("missing_proof", failClaim.FailureCode);

            // Add proof item
            inv.AddById(contract.requiredProofItemId, 1);
            Assert.Equal(1, inv.CountById(contract.requiredProofItemId));

            bool bountyClaimedFired = false;
            system.OnBountyClaimed += (_) => bountyClaimedFired = true;

            var successClaim = system.ClaimReward(contract.contractId);
            Assert.True(successClaim.IsSuccess);
            Assert.True(bountyClaimedFired);

            // Proof item consumed
            Assert.Equal(0, inv.CountById(contract.requiredProofItemId));

            // Scrap awarded
            Assert.Equal((int)contract.rewardAmount, inv.CountById("scrap_metal"));

            // Cannot claim a second time
            var duplicateClaim = system.ClaimReward(contract.contractId);
            Assert.False(duplicateClaim.IsSuccess);
            Assert.Equal("already_claimed", duplicateClaim.FailureCode);
        }

        [Fact]
        public void MercenarySystem_PostBounty_ConsumesPostingFeeAndDispatchesSquad()
        {
            var rng = new SeededRng(188);
            var inv = new Inventory.Inventory();
            inv.AddById("scrap_metal", 100);

            var system = new MercenarySystem(rng, inv);
            var result = system.PostBounty("bounty_template_raider_warlord", "target_raider_boss", "faction_hostile", 5);

            Assert.True(result.IsSuccess);
            Assert.True(inv.CountById("scrap_metal") < 100);

            var posted = system.ActiveContracts.FirstOrDefault(c => c.targetId == "target_raider_boss");
            Assert.NotNull(posted);
            Assert.False(posted.acceptedByPlayer);
        }
    }
}
