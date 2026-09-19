// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan206SurvivorDeathLegacyIntegrationTests
    {
        [Fact]
        public void DeathRecord_AndWill_DistributesInheritanceCorrectly()
        {
            var system = new SurvivorDeathLegacySystem();

            var will = system.CreateWill(
                survivorId: "surv_mechanic",
                beneficiaries: new[]
                {
                    new BeneficiaryEntry { BeneficiaryId = "surv_apprentice", Category = InheritanceCategory.Tools, Percentage = 100f }
                },
                specialBequests: new[]
                {
                    new SpecialBequest { ItemId = "wrench_golden", RecipientId = "surv_bestfriend" }
                },
                residuaryBeneficiary: "commons",
                currentDay: 1
            );

            Assert.NotNull(will);
            Assert.True(will.IsValid);

            var death = system.RecordDeath(
                survivorId: "surv_mechanic",
                survivorName: "Bob the Builder",
                cause: DeathCause.Accident,
                deathDay: 15,
                location: "Generator Bay"
            );

            Assert.NotNull(death);
            Assert.Equal(1, system.DeathCount);

            var items = system.DistributeInheritance(death.RecordId, new[] { "wrench_golden", "tool_kit", "rations" });
            Assert.Equal(3, items.Count);

            // Special bequest
            Assert.Equal("surv_bestfriend", items.First(i => i.ItemId == "wrench_golden").RecipientId);
            // Beneficiary
            Assert.Equal("surv_apprentice", items.First(i => i.ItemId == "tool_kit").RecipientId);
        }

        [Fact]
        public void InheritanceDispute_Lifecycle_CanBeRaisedAndResolved()
        {
            var system = new SurvivorDeathLegacySystem();
            var will = system.CreateWill("surv_a");

            var disp = system.RaiseDispute(will.WillId, "surv_b", "Unfair division of rations");
            Assert.NotNull(disp);
            Assert.Equal(1, system.ActiveDisputeCount);
            Assert.Equal(DisputeResolution.Pending, disp.Resolution);

            bool resolved = system.ResolveDispute(disp.DisputeId, DisputeResolution.Mediated);
            Assert.True(resolved);
            Assert.Equal(0, system.ActiveDisputeCount);
            Assert.Equal(DisputeResolution.Mediated, disp.Resolution);
        }

        [Fact]
        public void DeathLegacy_PersistenceRoundtrip_PreservesFullState()
        {
            var system = new SurvivorDeathLegacySystem();
            system.CreateWill("surv_1");
            system.RecordDeath("surv_2", "Jane Doe", DeathCause.Radiation, 5);

            var state = system.CaptureState();
            string json = JsonSerializer.Serialize(state);
            var restored = JsonSerializer.Deserialize<SurvivorDeathLegacyState>(json);
            Assert.NotNull(restored);

            var newSystem = new SurvivorDeathLegacySystem();
            newSystem.RestoreState(restored!);

            Assert.Equal(1, newSystem.WillCount);
            Assert.Equal(1, newSystem.DeathCount);
            Assert.Equal("Jane Doe", newSystem.DeathRecords[0].SurvivorName);
        }
    }
}
