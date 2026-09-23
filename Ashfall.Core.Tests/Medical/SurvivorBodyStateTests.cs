// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class SurvivorBodyStateTests
    {
        [Fact]
        public void DefaultIntact_HasAllLimbsIntact_AndNullRehab()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();

            Assert.Equal(1, body.SchemaVersion);
            Assert.Null(body.Rehab);
            Assert.Equal(4, body.Limbs.Count);
            Assert.Equal("intact", body.GetLimb(SurvivorBodyState.LeftArmKey).Condition);
            Assert.Equal("intact", body.GetLimb(SurvivorBodyState.RightArmKey).Condition);
            Assert.Equal("intact", body.GetLimb(SurvivorBodyState.LeftLegKey).Condition);
            Assert.Equal("intact", body.GetLimb(SurvivorBodyState.RightLegKey).Condition);
        }

        [Fact]
        public void FromLimbStates_AndToLimbStates_RoundTrip()
        {
            var initialStates = new List<LimbState>
            {
                new LimbState { limb = LimbId.LeftArm, condition = LimbCondition.Prosthetic, prostheticId = "item_hook_prosthetic" },
                new LimbState { limb = LimbId.RightArm, condition = LimbCondition.Intact },
                new LimbState { limb = LimbId.LeftLeg, condition = LimbCondition.Intact },
                new LimbState { limb = LimbId.RightLeg, condition = LimbCondition.Amputated }
            };

            var rehab = new RehabRecord("hand", "adaptation", 4, 620);
            var body = SurvivorBodyState.FromLimbStates(initialStates, rehab);

            Assert.NotNull(body.Rehab);
            Assert.Equal("hand", body.Rehab!.ProstheticTypeKey);
            Assert.Equal("adaptation", body.Rehab.Phase);
            Assert.Equal(4, body.Rehab.DaysInPhase);
            Assert.Equal(620, body.Rehab.QualityRampPermille);

            Assert.Equal("prosthetized", body.GetLimb(SurvivorBodyState.LeftArmKey).Condition);
            Assert.Equal("item_hook_prosthetic", body.GetLimb(SurvivorBodyState.LeftArmKey).ProstheticItemId);
            Assert.Equal("amputated", body.GetLimb(SurvivorBodyState.RightLegKey).Condition);

            var mappedBack = new List<LimbState>(body.ToLimbStates());
            Assert.Equal(4, mappedBack.Count);

            var leftArm = mappedBack.Find(l => l.limb == LimbId.LeftArm);
            Assert.NotNull(leftArm);
            Assert.Equal(LimbCondition.Prosthetic, leftArm!.condition);
            Assert.Equal("item_hook_prosthetic", leftArm.prostheticId);

            var rightLeg = mappedBack.Find(l => l.limb == LimbId.RightLeg);
            Assert.NotNull(rightLeg);
            Assert.Equal(LimbCondition.Amputated, rightLeg!.condition);
        }

        [Fact]
        public void JsonSerialization_MatchesNormativeShape()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();
            body.SetLimbCondition(SurvivorBodyState.LeftArmKey, "prosthetized", "item_hook_prosthetic");
            body.SetLimbCondition(SurvivorBodyState.RightLegKey, "amputated");
            body.Rehab = new RehabRecord("hand", "adaptation", 4, 620);

            string json = JsonSerializer.Serialize(body);
            Assert.Contains("\"schema_version\":1", json);
            Assert.Contains("\"left_arm\"", json);
            Assert.Contains("\"item_hook_prosthetic\"", json);
            Assert.Contains("\"quality_ramp_permille\":620", json);

            var deserialized = JsonSerializer.Deserialize<SurvivorBodyState>(json);
            Assert.NotNull(deserialized);
            Assert.Equal("prosthetized", deserialized!.GetLimb(SurvivorBodyState.LeftArmKey).Condition);
            Assert.Equal("item_hook_prosthetic", deserialized.GetLimb(SurvivorBodyState.LeftArmKey).ProstheticItemId);
            Assert.Equal(620, deserialized.Rehab?.QualityRampPermille);
        }

        [Fact]
        public void MissingLimb_DefaultsToIntact()
        {
            var body = new SurvivorBodyState();
            var record = body.GetLimb("unknown_limb");
            Assert.Equal("intact", record.Condition);
            Assert.Null(record.ProstheticItemId);
        }
    }
}
