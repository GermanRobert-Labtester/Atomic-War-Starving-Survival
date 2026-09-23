// SPDX-License-Identifier: MIT
using System.IO;
using System.Linq;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class Plan215ResourceRationingIntegrationTests
    {
        [Fact]
        public void LoadCatalog_LoadsAllProtocols_FromValidJson()
        {
            var system = new ResourceRationingSystem();
            string path = Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "rationing_protocols.json");
            if (!File.Exists(path))
            {
                path = Path.Combine("Assets", "StreamingAssets", "Data", "rationing_protocols.json");
            }
            Assert.True(File.Exists(path), $"Catalog not found at {path}");

            string json = File.ReadAllText(path);
            system.LoadCatalog(json);

            Assert.Equal(4, system.Protocols.Count);
            var standard = system.GetProtocol("protocol_standard_distribution");
            Assert.NotNull(standard);
            Assert.Equal(RationingTier.Full, standard.ParseDefaultTier());
            Assert.Equal(0f, standard.MoralePenaltyScale);

            var emergency = system.GetProtocol("protocol_emergency_crisis");
            Assert.NotNull(emergency);
            Assert.Equal(RationingTier.Half, emergency.ParseDefaultTier());
            Assert.Equal(4.0f, emergency.MoralePenaltyScale);
        }

        [Fact]
        public void ApplyProtocol_AppliesDefaultTierAcrossResources()
        {
            var system = new ResourceRationingSystem();
            system.LoadCatalog(@"{
                ""schema_version"": 1,
                ""protocols"": [
                    {
                        ""id"": ""protocol_emergency_crisis"",
                        ""name"": ""Emergency Mandate"",
                        ""protocol_type"": ""emergency"",
                        ""default_tier"": ""half"",
                        ""morale_penalty_scale"": 4.0
                    }
                ]
            }");

            string? appliedProtocol = null;
            system.OnProtocolApplied += p => appliedProtocol = p;

            bool success = system.ApplyProtocol("protocol_emergency_crisis", new[] { "food_rations", "purified_water" }, currentDay: 3);

            Assert.True(success);
            Assert.Equal("protocol_emergency_crisis", system.ActiveProtocolId);
            Assert.Equal("protocol_emergency_crisis", appliedProtocol);
            Assert.Equal(RationingTier.Half, system.GetRationTier("food_rations"));
            Assert.Equal(RationingTier.Half, system.GetRationTier("purified_water"));
            Assert.Equal(0.5f, system.GetRationMultiplier("food_rations"));
        }

        [Fact]
        public void SetRationTier_And_PriorityModifiers_ScaleAllocation()
        {
            var system = new ResourceRationingSystem();
            system.SetRationTier("antibiotics", RationingTier.Quarter, currentDay: 1); // 0.25

            system.AssignSurvivorPriority("head_surgeon", PriorityGroupTier.Critical); // 1.30
            system.AssignSurvivorPriority("standard_dweller", PriorityGroupTier.Standard); // 1.00
            system.AssignSurvivorPriority("penal_laborer", PriorityGroupTier.Low); // 0.75

            float surgeonMult = system.GetAllocationMultiplier("antibiotics", "head_surgeon");
            float dwellerMult = system.GetAllocationMultiplier("antibiotics", "standard_dweller");
            float penalMult = system.GetAllocationMultiplier("antibiotics", "penal_laborer");

            Assert.Equal(0.32f, surgeonMult); // 0.25 * 1.30 = 0.325 -> 0.32 (banker's rounding)
            Assert.Equal(0.25f, dwellerMult); // 0.25 * 1.00 = 0.25
            Assert.Equal(0.19f, penalMult);   // 0.25 * 0.75 = 0.1875 -> 0.19
        }

        [Fact]
        public void DeclareCrisis_And_CalculateMoraleImpact_EvaluatesAusterityPenalty()
        {
            var system = new ResourceRationingSystem();
            system.SetRationTier("food", RationingTier.Minimal, currentDay: 2); // 0.12
            system.SetRationTier("water", RationingTier.Half, currentDay: 2);    // 0.50

            var crisis = system.DeclareCrisis(ResourceCrisisType.FoodShortage, "severe", currentDay: 2, "Hydroponic failure");
            Assert.NotNull(crisis);
            Assert.Equal(1, system.ActiveCrisesCount);

            float moraleImpact = system.CalculateMoraleImpact("survivor_any");
            Assert.True(moraleImpact < -5.0f); // Severe food shortage creates significant negative impact
        }

        [Fact]
        public void AuthorizeAllocation_RestrictsUnitsUnderRationing()
        {
            var system = new ResourceRationingSystem();
            system.SetRationTier("water", RationingTier.Half, currentDay: 1);

            var request = new ResourceAllocationRequest
            {
                ResourceId = "water",
                ConsumerId = "dweller_common",
                DemandUnits = 10,
                AvailableUnits = 50,
                CurrentDay = 1
            };

            var decision = system.AuthorizeAllocation(request);

            Assert.NotNull(decision);
            Assert.True(decision.Authorized);
            Assert.Equal(10, decision.RequestedUnits);
            Assert.Equal(5, decision.AllocatedUnits); // 10 * 0.5 = 5 units
            Assert.Equal(0.5f, decision.AppliedMultiplier);
        }

        [Fact]
        public void CaptureAndRestoreState_RoundTripsActiveProtocolAndTargets()
        {
            var system = new ResourceRationingSystem();
            system.LoadCatalog(@"{
                ""schema_version"": 1,
                ""protocols"": [
                    { ""id"": ""protocol_tightened"", ""name"": ""Tight"", ""default_tier"": ""three_quarter"" }
                ]
            }");
            system.ApplyProtocol("protocol_tightened", new[] { "fuel" }, currentDay: 5);
            system.AssignSurvivorPriority("engineer_1", PriorityGroupTier.High);
            system.DeclareCrisis(ResourceCrisisType.FuelShortage, "moderate", currentDay: 5);

            var state = system.CaptureState();
            Assert.Equal("protocol_tightened", state.ActiveProtocolId);
            Assert.Single(state.Targets);
            Assert.Single(state.Assignments);
            Assert.Single(state.Crises);

            var restored = new ResourceRationingSystem();
            restored.RestoreState(state);

            Assert.Equal("protocol_tightened", restored.ActiveProtocolId);
            Assert.Equal(RationingTier.ThreeQuarter, restored.GetRationTier("fuel"));
            Assert.Equal(PriorityGroupTier.High, restored.GetSurvivorPriority("engineer_1"));
            Assert.Equal(1, restored.ActiveCrisesCount);
        }
    }
}
