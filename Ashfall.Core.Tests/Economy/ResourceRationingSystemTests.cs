// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class ResourceRationingSystemTests
    {
        [Fact]
        public void SetRationTier_AdjustsMultiplierAndRecordsEvent()
        {
            var system = new ResourceRationingSystem();
            RationTarget? emitted = null;
            system.OnRationingTierChanged += t => emitted = t;

            var target = system.SetRationTier("food", RationingTier.Half, currentDay: 5);

            Assert.NotNull(target);
            Assert.Equal("food", target.ResourceId);
            Assert.Equal(RationingTier.Half, target.Tier);
            Assert.Equal(0.5f, target.BaseMultiplier);
            Assert.Equal(emitted, target);
        }

        [Fact]
        public void GetAllocationMultiplier_ScalesWithSurvivorPriorityTier()
        {
            var system = new ResourceRationingSystem();
            system.SetRationTier("water", RationingTier.Half, 1); // 0.50

            system.AssignSurvivorPriority("medic_1", PriorityGroupTier.Critical); // 1.30
            system.AssignSurvivorPriority("dweller_1", PriorityGroupTier.Standard); // 1.00
            system.AssignSurvivorPriority("prisoner_1", PriorityGroupTier.Low); // 0.75

            float medicMult = system.GetAllocationMultiplier("water", "medic_1");
            float dwellerMult = system.GetAllocationMultiplier("water", "dweller_1");
            float prisonerMult = system.GetAllocationMultiplier("water", "prisoner_1");

            Assert.Equal(0.65f, medicMult); // 0.5 * 1.30 = 0.65
            Assert.Equal(0.50f, dwellerMult); // 0.5 * 1.00 = 0.50
            Assert.Equal(0.38f, prisonerMult); // 0.5 * 0.75 = 0.375 -> 0.38
        }

        [Fact]
        public void DeclareCrisis_And_ResolveCrisis_TracksStatus()
        {
            var system = new ResourceRationingSystem();
            var crisis = system.DeclareCrisis(ResourceCrisisType.FoodShortage, "severe", currentDay: 10, "Crop blighted");

            Assert.NotNull(crisis);
            Assert.False(crisis.IsResolved);
            Assert.Equal(1, system.ActiveCrisesCount);

            bool resolved = system.ResolveCrisis(crisis.CrisisId, currentDay: 15);
            Assert.True(resolved);
            Assert.True(crisis.IsResolved);
            Assert.Equal(0, system.ActiveCrisesCount);
        }

        [Fact]
        public void CalculateMoraleImpact_PenalizesRationTightness()
        {
            var system = new ResourceRationingSystem();
            system.SetRationTier("food", RationingTier.Minimal, 1); // 0.12
            system.DeclareCrisis(ResourceCrisisType.FoodShortage, "critical", 1);

            float moralePenalty = system.CalculateMoraleImpact("dweller_2");
            Assert.True(moralePenalty < -5.0f);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new ResourceRationingSystem();
            system1.SetRationTier("medicine", RationingTier.Quarter, 3);
            system1.AssignSurvivorPriority("doc_1", PriorityGroupTier.Critical);
            var crisis = system1.DeclareCrisis(ResourceCrisisType.MedicalShortage, "moderate", 3);

            var state = system1.CaptureState();
            Assert.Single(state.Targets);
            Assert.Single(state.Assignments);
            Assert.Single(state.Crises);

            var system2 = new ResourceRationingSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.TargetCount);
            Assert.Equal(1, system2.ActiveCrisesCount);
            Assert.Equal(RationingTier.Quarter, system2.GetRationTier("medicine"));
            Assert.Equal(PriorityGroupTier.Critical, system2.GetSurvivorPriority("doc_1"));
        }

        [Fact]
        public void AuthorizeAllocation_IsPolicyOnlyAndBoundedByDemandAndStock()
        {
            var system = new ResourceRationingSystem();
            system.SetRationTier("food", RationingTier.Half, currentDay: 2);
            system.AssignSurvivorPriority("child_1", PriorityGroupTier.Critical);

            var decision = system.AuthorizeAllocation(new ResourceAllocationRequest
            {
                ResourceId = "food",
                ConsumerId = "child_1",
                DemandUnits = 10,
                AvailableUnits = 3,
                CurrentDay = 2
            });

            Assert.True(decision.Authorized);
            Assert.Equal(3, decision.AllocatedUnits); // floor(10 * .65) capped by stock
            Assert.Equal(3, decision.AvailableUnits);
            Assert.Equal(1, system.TargetCount); // policy owns no stock ledger
        }

        [Fact]
        public void BoundResourceValidator_RejectsUnknownPolicyAndDropsInvalidRestoredRows()
        {
            var state = new ResourceRationingState();
            state.Targets.Add(new RationTarget { ResourceId = "retired_resource", Tier = RationingTier.Half });
            var system = new ResourceRationingSystem(state);
            system.BindResourceValidator(id => id == "clean_water");

            Assert.Empty(system.CaptureState().Targets);
            Assert.Throws<ArgumentException>(() => system.SetRationTier("retired_resource", RationingTier.None, 1));
            Assert.Equal(RationingTier.Full, system.GetRationTier("clean_water"));
        }

        [Fact]
        public void BoundResourceValidator_AlsoFiltersSubsequentRestore()
        {
            var system = new ResourceRationingSystem(resourceValidator: id => id == "clean_water");
            system.RestoreState(new ResourceRationingState
            {
                Targets = new List<RationTarget>
                {
                    new RationTarget { ResourceId = "retired_resource", Tier = RationingTier.None },
                    new RationTarget { ResourceId = " clean_water ", Tier = RationingTier.Half }
                }
            });

            var restored = system.CaptureState();
            var target = Assert.Single(restored.Targets);
            Assert.Equal("clean_water", target.ResourceId);
            Assert.Equal(RationingTier.Half, target.Tier);
        }
    }
}
