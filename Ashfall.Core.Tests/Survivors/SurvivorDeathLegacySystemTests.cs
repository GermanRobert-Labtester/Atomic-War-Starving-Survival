// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class SurvivorDeathLegacySystemTests
    {
        [Fact]
        public void RecordDeath_StoresRecordAndEmitsEvent()
        {
            var system = new SurvivorDeathLegacySystem();
            DeathRecord? emitted = null;
            system.OnDeathRecorded += r => emitted = r;

            var record = system.RecordDeath(
                survivorId: "surv_marcus",
                survivorName: "Marcus Vance",
                cause: DeathCause.Combat,
                deathDay: 42,
                location: "loc_north_gate",
                lastWords: "Keep the beacon burning.",
                witnesses: new[] { "surv_elena" },
                circumstances: "Defending against raider assault."
            );

            Assert.NotNull(record);
            Assert.Equal("surv_marcus", record.SurvivorId);
            Assert.Equal("Marcus Vance", record.SurvivorName);
            Assert.Equal(DeathCause.Combat, record.Cause);
            Assert.Equal(42, record.DeathDay);
            Assert.Equal(record, emitted);
            Assert.Equal(1, system.DeathCount);
        }

        [Fact]
        public void CreateWill_StoresWillAndInvalidatesOlderWills()
        {
            var system = new SurvivorDeathLegacySystem();
            var will1 = system.CreateWill("surv_elena", residuaryBeneficiary: "surv_marcus", currentDay: 10);
            Assert.True(will1.IsValid);

            var will2 = system.CreateWill(
                "surv_elena",
                beneficiaries: new[] { new BeneficiaryEntry { BeneficiaryId = "surv_child", Percentage = 100f } },
                specialBequests: new[] { new SpecialBequest { ItemId = "item_watch", RecipientId = "surv_mentor" } },
                residuaryBeneficiary: "commons",
                currentDay: 20
            );

            Assert.False(will1.IsValid);
            Assert.True(will2.IsValid);
            Assert.Equal(2, system.WillCount);

            var active = system.GetActiveWill("surv_elena");
            Assert.Equal(will2, active);
        }

        [Fact]
        public void DistributeInheritance_FollowsSpecialBequestAndBeneficiary()
        {
            var system = new SurvivorDeathLegacySystem();
            system.CreateWill(
                "surv_elena",
                beneficiaries: new[] { new BeneficiaryEntry { BeneficiaryId = "surv_heir", Percentage = 100f } },
                specialBequests: new[] { new SpecialBequest { ItemId = "item_rifle", RecipientId = "surv_sniper" } }
            );

            var death = system.RecordDeath("surv_elena", "Elena", DeathCause.Disease, 15);

            var items = new[] { "item_rifle", "item_jacket" };
            var distributed = system.DistributeInheritance(death.RecordId, items);

            Assert.Equal(2, distributed.Count);

            var rifle = distributed.FirstOrDefault(i => i.ItemId == "item_rifle");
            Assert.NotNull(rifle);
            Assert.Equal("surv_sniper", rifle.RecipientId); // Special bequest

            var jacket = distributed.FirstOrDefault(i => i.ItemId == "item_jacket");
            Assert.NotNull(jacket);
            Assert.Equal("surv_heir", jacket.RecipientId); // Primary beneficiary
        }

        [Fact]
        public void RaiseAndResolveDispute_TransitionsStatus()
        {
            var system = new SurvivorDeathLegacySystem();
            var will = system.CreateWill("surv_old", residuaryBeneficiary: "surv_fav");

            var disp = system.RaiseDispute(will.WillId, "surv_sibling", "Undue influence");
            Assert.NotNull(disp);
            Assert.Equal(DisputeResolution.Pending, disp.Resolution);
            Assert.Equal(1, system.ActiveDisputeCount);

            bool resolved = system.ResolveDispute(disp.DisputeId, DisputeResolution.Mediated);
            Assert.True(resolved);
            Assert.Equal(DisputeResolution.Mediated, disp.Resolution);
            Assert.Equal(0, system.ActiveDisputeCount);
        }

        [Fact]
        public void CaptureAndRestoreState_RoundTripsAccurately()
        {
            var system1 = new SurvivorDeathLegacySystem();
            var will = system1.CreateWill("surv_a", residuaryBeneficiary: "surv_b", currentDay: 5);
            var death = system1.RecordDeath("surv_a", "Survivor A", DeathCause.Radiation, 6);
            system1.DistributeInheritance(death.RecordId, new[] { "item_boots" });

            var state = system1.CaptureState();

            var system2 = new SurvivorDeathLegacySystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.DeathCount);
            Assert.Equal(1, system2.WillCount);

            var restoredDeath = system2.GetDeathRecord("surv_a");
            Assert.NotNull(restoredDeath);
            Assert.Equal("Survivor A", restoredDeath.SurvivorName);
            Assert.Equal(DeathCause.Radiation, restoredDeath.Cause);

            var restoredWill = system2.GetActiveWill("surv_a");
            Assert.NotNull(restoredWill);
            Assert.Equal("surv_b", restoredWill.ResiduaryBeneficiary);
        }
    }
}
