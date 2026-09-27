// SPDX-License-Identifier: MIT
// F14-D / F14-E / F14-G integration: the live limb authority (AmputationSystem)
// now consumes the signed prosthetic wear, rehabilitation, and body-presentation
// engines on its daily tick and exposes a read-only slate. The engine-level
// suites already cover the pure math (ProstheticConditionWearEngineTests,
// RehabilitationProgressionEngineTests, SurvivorBodyPresentationSlateTests);
// this suite pins the missing half: limb owner -> daily outcome -> projection.
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Medical;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class PlanF14ProstheticCareIntegrationTests
    {
        private static (AmputationSystem system, LimbState limb) FittedSurvivor()
        {
            var inv = new InventoryContainer();
            inv.AddById("prosthetic_wooden_leg", 1);
            var sys = new AmputationSystem(new SeededRng(414), inv);
            sys.EnsureSurvivorLimbs("survivor_1");
            var limb = sys.GetLimb("survivor_1", LimbId.LeftLeg)!;
            limb.condition = LimbCondition.Amputated;
            limb.recoveryDaysLeft = 0;
            var fit = sys.FitProsthetic("survivor_1", LimbId.LeftLeg, "prosthetic_wooden_leg");
            Assert.True(fit.IsSuccess);
            return (sys, limb);
        }

        [Fact]
        public void Fitting_Initialises_Factory_Condition_And_Fitting_Rehab_Arc()
        {
            var (_, limb) = FittedSurvivor();
            Assert.Equal(1000, limb.prostheticConditionPermille);
            Assert.Equal("prosthetic_wooden_leg", limb.prostheticTypeKey);
            Assert.Equal("fitting", limb.rehabPhase);
            Assert.Equal(0, limb.rehabDaysInPhase);
            Assert.Equal(RehabilitationProgressionEngine.FittingQualityPermille, limb.rehabQualityPermille);
        }

        [Fact]
        public void Daily_Tick_Applies_Wear_And_Advances_Rehab()
        {
            var (sys, limb) = FittedSurvivor();
            int before = limb.prostheticConditionPermille;

            sys.TickDay(1);
            Assert.True(limb.prostheticConditionPermille < before,
                "F14-D: daily prosthetic wear must reduce device condition.");

            // F14-E: fitting lasts 4 days at defaults, then the arc advances.
            sys.TickDay(2);
            sys.TickDay(3);
            sys.TickDay(4);
            Assert.Equal("adaptation", limb.rehabPhase);

            for (int day = 5; day <= 30; day++) sys.TickDay(day);
            Assert.Equal("mastery", limb.rehabPhase);
            Assert.Equal(RehabilitationProgressionEngine.MasteryQualityPermille, limb.rehabQualityPermille);
        }

        [Fact]
        public void Service_Action_Restores_Condition_Through_The_Owner()
        {
            var (sys, limb) = FittedSurvivor();
            for (int day = 1; day <= 40; day++) sys.TickDay(day);
            int worn = limb.prostheticConditionPermille;

            var service = sys.ServiceProsthetic("survivor_1", LimbId.LeftLeg, 250);
            Assert.True(service.IsSuccess);
            Assert.True(limb.prostheticConditionPermille > worn);
            Assert.True(limb.prostheticConditionPermille <= 1000);
        }

        [Fact]
        public void Service_Action_Refuses_Non_Prosthetic_Limb()
        {
            var sys = new AmputationSystem(new SeededRng(1));
            sys.EnsureSurvivorLimbs("survivor_1");
            var refused = sys.ServiceProsthetic("survivor_1", LimbId.RightArm, 250);
            Assert.False(refused.IsSuccess);
        }

        [Fact]
        public void Body_Slate_Projects_Amputated_And_Prosthetized_Limbs()
        {
            var (sys, _) = FittedSurvivor();
            var slate = sys.BuildBodySlate("survivor_1");

            Assert.Equal("survivor_1", slate.SurvivorId);
            Assert.Contains(slate.Limbs, row => row.LimbKey == SurvivorBodyState.LeftLegKey
                && row.StatusText.StartsWith("Prosthetic"));
            Assert.Contains(slate.Limbs, row => row.LimbKey == SurvivorBodyState.RightLegKey
                && row.StatusText == "Intact");
            Assert.True(slate.OverallMobilityPermille > 0);
            Assert.Equal(2, slate.EffectiveHands);
        }

        [Fact]
        public void Body_State_Carries_Rehab_Arc_From_Limb_Authority()
        {
            var (sys, limb) = FittedSurvivor();
            sys.TickDay(1);
            var body = sys.BuildBodyState("survivor_1");
            Assert.NotNull(body.Rehab);
            Assert.Equal(limb.rehabPhase, body.Rehab!.Phase);
            Assert.Equal(
                limb.condition == LimbCondition.Prosthetic ? "prosthetized" : "other",
                body.GetLimb(SurvivorBodyState.LeftLegKey).Condition);
        }

        [Fact]
        public void Prosthetic_Care_Round_Trips_Through_Owner_State()
        {
            var (sys, limb) = FittedSurvivor();
            for (int day = 1; day <= 8; day++) sys.TickDay(day);
            int condition = limb.prostheticConditionPermille;
            string phase = limb.rehabPhase ?? string.Empty;

            var restored = new AmputationSystem(new SeededRng(99));
            restored.RestoreState(sys.CaptureState());
            var rlimb = restored.GetLimb("survivor_1", LimbId.LeftLeg)!;
            Assert.Equal(condition, rlimb.prostheticConditionPermille);
            Assert.Equal(phase, rlimb.rehabPhase);
        }
    }
}
