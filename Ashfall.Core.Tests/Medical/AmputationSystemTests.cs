// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class AmputationSystemTests
    {
        [Fact]
        public void DeepWound_ProgressesTo_Infection_And_Gangrene()
        {
            var sys = new AmputationSystem(new SeededRng(42));
            sys.EnsureSurvivorLimbs("survivor_1");

            sys.InflictDeepWound("survivor_1", LimbId.LeftLeg);
            var limb = sys.GetLimb("survivor_1", LimbId.LeftLeg);
            Assert.NotNull(limb);
            Assert.Equal(LimbCondition.Wounded, limb!.condition);

            // Day 1: untreated
            sys.TickDay(1);
            Assert.Equal(LimbCondition.Wounded, limb.condition);

            // Day 2: infected
            sys.TickDay(2);
            Assert.Equal(LimbCondition.Infected, limb.condition);

            // Days 3-5: gangrene
            sys.TickDay(3);
            sys.TickDay(4);
            sys.TickDay(5);
            Assert.Equal(LimbCondition.Gangrenous, limb.condition);
        }

        [Fact]
        public void Amputation_Blocks_When_Missing_Tools_Or_Consumables()
        {
            var inv = new Inventory.Inventory();
            var sys = new AmputationSystem(new SeededRng(42), inv);
            sys.RegisterProcedure(new SurgicalProcedureDef
            {
                procedure_id = "procedure_amputation_arm_field",
                required_tool_id = "surgical_saw",
                required_items = new List<SurgicalItemCost>
                {
                    new SurgicalItemCost { item_id = "painkillers", amount = 1 },
                    new SurgicalItemCost { item_id = "cloth", amount = 2 }
                }
            });
            sys.EnsureSurvivorLimbs("survivor_1");
            sys.InflictDeepWound("survivor_1", LimbId.RightArm);

            // Missing saw
            var res = sys.PerformAmputation("survivor_1", LimbId.RightArm, "procedure_amputation_arm_field");
            Assert.False(res.Success);
            Assert.Equal("missing_surgical_tool", res.FailureCode);

            // Add saw, but missing painkillers/cloth
            inv.AddById("surgical_saw", 1);
            res = sys.PerformAmputation("survivor_1", LimbId.RightArm, "procedure_amputation_arm_field");
            Assert.False(res.Success);
            Assert.StartsWith("missing_item_", res.FailureCode);
        }

        [Fact]
        public void Amputation_Succeeds_And_Sets_Amputated_State()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("surgical_saw", 1);
            inv.AddById("painkillers", 5);
            inv.AddById("clean_water", 5);
            inv.AddById("cloth", 5);

            var sys = new AmputationSystem(new SeededRng(42), inv);
            sys.RegisterProcedure(new SurgicalProcedureDef
            {
                procedure_id = "procedure_amputation_arm_field",
                required_tool_id = "surgical_saw",
                required_items = new List<SurgicalItemCost>
                {
                    new SurgicalItemCost { item_id = "painkillers", amount = 1 },
                    new SurgicalItemCost { item_id = "cloth", amount = 2 }
                },
                base_shock_risk = 0.0f // guaranteed survival in test
            });

            sys.EnsureSurvivorLimbs("survivor_1");
            sys.InflictDeepWound("survivor_1", LimbId.RightArm);

            var res = sys.PerformAmputation("survivor_1", LimbId.RightArm, "procedure_amputation_arm_field");
            Assert.True(res.Success);
            Assert.False(res.SurvivorDied);

            var limb = sys.GetLimb("survivor_1", LimbId.RightArm);
            Assert.NotNull(limb);
            Assert.Equal(LimbCondition.Amputated, limb!.condition);
            Assert.True(limb.recoveryDaysLeft > 0);

            // Work multiplier is reduced
            float workMult = sys.GetWorkSpeedMultiplier("survivor_1");
            Assert.True(workMult < 1.0f);
        }

        [Fact]
        public void Fitting_Prosthetic_And_Upgrading_To_Bionic()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("prosthetic_wooden_leg", 1);
            inv.AddById("bionic_leg_prototype", 1);

            var sys = new AmputationSystem(new SeededRng(42), inv);
            sys.EnsureSurvivorLimbs("survivor_1");

            var limb = sys.GetLimb("survivor_1", LimbId.LeftLeg);
            limb!.condition = LimbCondition.Amputated;
            limb.recoveryDaysLeft = 0; // recovered

            // Fit wooden prosthetic
            var fitRes = sys.FitProsthetic("survivor_1", LimbId.LeftLeg, "prosthetic_wooden_leg");
            Assert.True(fitRes.IsSuccess);
            Assert.Equal(LimbCondition.Prosthetic, limb.condition);
            Assert.Equal("prosthetic_wooden_leg", limb.prostheticId);

            float speedWithPeg = sys.GetMovementSpeedMultiplier("survivor_1");

            // Upgrade to bionic
            var bionicRes = sys.UpgradeToBionic("survivor_1", LimbId.LeftLeg, "bionic_leg_prototype");
            Assert.True(bionicRes.IsSuccess);
            Assert.Equal(LimbCondition.Bionic, limb.condition);
            Assert.Equal("bionic_leg_prototype", limb.prostheticId);

            float speedWithBionic = sys.GetMovementSpeedMultiplier("survivor_1");
            Assert.True(speedWithBionic > speedWithPeg);
        }

        [Fact]
        public void State_RoundTrip_Preserves_Limb_Data()
        {
            var sys = new AmputationSystem(new SeededRng(42));
            sys.EnsureSurvivorLimbs("survivor_alpha");
            var l = sys.GetLimb("survivor_alpha", LimbId.LeftArm);
            l!.condition = LimbCondition.Bionic;
            l.prostheticId = "bionic_arm_prototype";

            var state = sys.State;
            var json = System.Text.Json.JsonSerializer.Serialize(state);

            var deserialized = System.Text.Json.JsonSerializer.Deserialize<AmputationSystemState>(json);
            var sys2 = new AmputationSystem(new SeededRng(42));
            sys2.RestoreState(deserialized!);

            var restoredLimb = sys2.GetLimb("survivor_alpha", LimbId.LeftArm);
            Assert.NotNull(restoredLimb);
            Assert.Equal(LimbCondition.Bionic, restoredLimb!.condition);
            Assert.Equal("bionic_arm_prototype", restoredLimb.prostheticId);
        }

        [Fact]
        public void PhantomPain_EpisodesFire_Periodically_AndDecay()
        {
            var needs = new NeedsSystem();
            needs.Register(new SurvivorNeedsState { Id = "survivor_pain", Health = 100f, Morale = 100f, Hunger = 100f, Thirst = 100f, Fatigue = 100f });
            var sys = new AmputationSystem(new SeededRng(42), needs: needs);
            sys.RegisterProcedure(new SurgicalProcedureDef
            {
                procedure_id = "procedure_amputation_leg",
                required_tool_id = "surgical_saw",
                phantom_pain_chance = 1.0f
            });
            sys.EnsureSurvivorLimbs("survivor_pain");

            // Amputate — phantom pain guaranteed
            var inv = new Ashfall.Core.Inventory.Inventory();
            inv.AddById("surgical_saw", 1);
            var sys2 = new AmputationSystem(new SeededRng(42), inv, needs);
            sys2.RegisterProcedure(new SurgicalProcedureDef
            {
                procedure_id = "procedure_amputation_leg",
                required_tool_id = "surgical_saw",
                phantom_pain_chance = 1.0f
            });
            sys2.EnsureSurvivorLimbs("survivor_pain");
            sys2.PerformAmputation("survivor_pain", LimbId.LeftLeg, "procedure_amputation_leg");

            int episodesFired = 0;
            sys2.OnPhantomPainEpisode += (id, stress) => { episodesFired++; };

            // Tick for 21 days — should get multiple episodes
            for (int d = 0; d < 21; d++)
                sys2.TickDay(d);

            Assert.True(episodesFired >= 2, $"Expected >=2 phantom pain episodes, got {episodesFired}");
        }

        [Fact]
        public void CombatEffectiveness_Degrades_WithAmputations()
        {
            var sys = new AmputationSystem(new SeededRng(42));
            sys.EnsureSurvivorLimbs("survivor_fighter");

            // Intact = 100%
            float intact = sys.GetCombatEffectivenessMultiplier("survivor_fighter");
            Assert.Equal(1.0f, intact);

            // Amputate right arm
            var limb = sys.GetLimb("survivor_fighter", LimbId.RightArm);
            limb!.condition = LimbCondition.Amputated;
            float oneArm = sys.GetCombatEffectivenessMultiplier("survivor_fighter");
            Assert.True(oneArm < 0.80f, $"Expected <0.80 with missing arm, got {oneArm}");

            // Amputate left leg too
            var leg = sys.GetLimb("survivor_fighter", LimbId.LeftLeg);
            leg!.condition = LimbCondition.Amputated;
            float armAndLeg = sys.GetCombatEffectivenessMultiplier("survivor_fighter");
            Assert.True(armAndLeg < oneArm, "Missing arm+leg should be worse than missing just arm");
            Assert.True(armAndLeg >= 0.10f, $"Combat effectiveness floor is 0.10, got {armAndLeg}");

            // Bionic upgrade restores
            limb.condition = LimbCondition.Bionic;
            leg.condition = LimbCondition.Bionic;
            float bionic = sys.GetCombatEffectivenessMultiplier("survivor_fighter");
            Assert.True(bionic > 1.0f, $"Bionic should exceed baseline, got {bionic}");
        }
    }
}
