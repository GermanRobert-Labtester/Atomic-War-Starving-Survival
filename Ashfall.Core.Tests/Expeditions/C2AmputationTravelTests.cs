// SPDX-License-Identifier: MIT
using Ashfall.Core.Medical;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// C2 §D3 — limb-state travel consumer. The medical owner authors the
    /// movement multiplier; the expedition owner consumes the one dispatch-
    /// sampled value for both estimate and runtime. Intact/absent survivors
    /// store 1.0 (byte-identical parity); the field is additive and
    /// save-round-trips without migration.
    /// </summary>
    public sealed class C2AmputationTravelTests
    {
        private static ExpeditionDefinition Def(int ticks) => new ExpeditionDefinition
        {
            id = "loc_c2_travel",
            displayName = "C2 Travel Site",
            distanceTicks = ticks,
            dangerLevel = 1
        };

        [Fact]
        public void ImplicitAndExplicitOne_AreIntactParity()
        {
            var implicitOne = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Speed);
            var explicitOne = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Speed, survivorSpeedMultiplier: 1f);

            Assert.Equal(1f, implicitOne.survivorSpeedMultiplier, 3);
            Assert.Equal(implicitOne.outboundTicks, explicitOne.outboundTicks);
            Assert.Equal(implicitOne.totalTicks, explicitOne.totalTicks);
        }

        [Fact]
        public void AmputatedLeg_SlowsTravel_AndRecordsFactor()
        {
            var intact = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Speed);
            var limping = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Speed, survivorSpeedMultiplier: 0.55f);

            Assert.Equal(0.55f, limping.survivorSpeedMultiplier, 3);
            Assert.True(limping.outboundTicks > intact.outboundTicks,
                "an amputated leg must lengthen the outbound leg");
        }

        [Fact]
        public void MedicalOwner_MovementFactor_FeedsTheEstimate()
        {
            var medical = new AmputationSystem();
            var limb = medical.EnsureSurvivorLimbs("s_leg").Find(l => l.limb == LimbId.LeftLeg);
            Assert.NotNull(limb);
            limb!.condition = LimbCondition.Amputated;

            float factor = medical.GetMovementSpeedMultiplier("s_leg");
            Assert.True(factor < 1f, $"amputated leg must reduce movement (got {factor:F2})");

            var intact = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Speed);
            var limping = ExpeditionSystem.Estimate(Def(9), ExpeditionStance.Speed, survivorSpeedMultiplier: factor);
            Assert.True(limping.outboundTicks > intact.outboundTicks,
                "the medical factor must change the projected expedition travel");
        }

        [Fact]
        public void Start_SamplesFactor_OntoState_AndLegacyStateDefaultsToOne()
        {
            var engine = new ExpeditionSystem();
            Assert.True(engine.Start(Def(6), "s", 1, stance: ExpeditionStance.Speed, survivorSpeedMultiplier: 0.55f));

            Assert.Equal(0.55f, engine.Active["s"].survivorSpeedMultiplier, 3);

            // Additive field: an old save (field absent) deserializes to 1.
            Assert.Equal(1f, new ExpeditionState().survivorSpeedMultiplier);
        }

        [Fact]
        public void Runtime_AppliesFactorOncePerTravelStep()
        {
            var fast = new ExpeditionSystem();
            var slow = new ExpeditionSystem();
            Assert.True(fast.Start(Def(9), "s1", 1, stance: ExpeditionStance.Speed, survivorSpeedMultiplier: 1f));
            Assert.True(slow.Start(Def(9), "s2", 1, stance: ExpeditionStance.Speed, survivorSpeedMultiplier: 0.55f));

            var rng = new SeededRng(7);
            for (int i = 0; i < 4; i++)
            {
                fast.TickHours(1f, rng);
                slow.TickHours(1f, rng);
            }

            Assert.True(fast.Active["s1"].travelTicksCompleted > slow.Active["s2"].travelTicksCompleted,
                "the intact survivor must outpace the amputated survivor over the same hours");
        }

        [Fact]
        public void Factor_IsBounded_AndAbsentMeansIntact()
        {
            // Absent/zero reads as intact (old saves and the no-provider path).
            Assert.Equal(1f, ExpeditionSystem.Estimate(Def(4), ExpeditionStance.Stealth,
                survivorSpeedMultiplier: 0f).survivorSpeedMultiplier, 3);
            // Floored / ceilinged to the medical owner's own bounds.
            Assert.Equal(ExpeditionSystem.MinSurvivorSpeedMultiplier,
                ExpeditionSystem.Estimate(Def(4), ExpeditionStance.Stealth,
                    survivorSpeedMultiplier: 0.05f).survivorSpeedMultiplier, 3);
            Assert.Equal(ExpeditionSystem.MaxSurvivorSpeedMultiplier,
                ExpeditionSystem.Estimate(Def(4), ExpeditionStance.Stealth,
                    survivorSpeedMultiplier: 5f).survivorSpeedMultiplier, 3);
        }

        [Fact]
        public void State_RoundTripsThroughSerializer()
        {
            var state = new ExpeditionState
            {
                expeditionId = "e1",
                survivorId = "s",
                survivorSpeedMultiplier = 0.55f
            };

            var serializer = new SystemTextJsonSerializer();
            var restored = serializer.Deserialize<ExpeditionState>(serializer.Serialize(state));

            Assert.NotNull(restored);
            Assert.Equal(0.55f, restored!.survivorSpeedMultiplier, 3);
        }
    }
}