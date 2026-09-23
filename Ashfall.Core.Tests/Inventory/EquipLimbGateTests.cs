// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class EquipLimbGateTests
    {
        private static List<LimbState> IntactLimbs() => new()
        {
            new LimbState { limb = LimbId.LeftArm, condition = LimbCondition.Intact },
            new LimbState { limb = LimbId.RightArm, condition = LimbCondition.Intact },
            new LimbState { limb = LimbId.LeftLeg, condition = LimbCondition.Intact },
            new LimbState { limb = LimbId.RightLeg, condition = LimbCondition.Intact }
        };

        private static List<LimbState> OneArmAmputated() => new()
        {
            new LimbState { limb = LimbId.LeftArm, condition = LimbCondition.Intact },
            new LimbState { limb = LimbId.RightArm, condition = LimbCondition.Amputated },
            new LimbState { limb = LimbId.LeftLeg, condition = LimbCondition.Intact },
            new LimbState { limb = LimbId.RightLeg, condition = LimbCondition.Intact }
        };

        private static List<LimbState> HookAndAmputated() => new()
        {
            new LimbState { limb = LimbId.LeftArm, condition = LimbCondition.Amputated },
            new LimbState { limb = LimbId.RightArm, condition = LimbCondition.Prosthetic, prostheticId = "item_hook_prosthetic" },
            new LimbState { limb = LimbId.LeftLeg, condition = LimbCondition.Intact },
            new LimbState { limb = LimbId.RightLeg, condition = LimbCondition.Intact }
        };

        private static List<LimbState> TwoArticulatedHands() => new()
        {
            new LimbState { limb = LimbId.LeftArm, condition = LimbCondition.Prosthetic, prostheticId = "item_articulated_hand" },
            new LimbState { limb = LimbId.RightArm, condition = LimbCondition.Prosthetic, prostheticId = "item_articulated_hand" },
            new LimbState { limb = LimbId.LeftLeg, condition = LimbCondition.Intact },
            new LimbState { limb = LimbId.RightLeg, condition = LimbCondition.Intact }
        };

        private static ItemDefinition HookProsthetic() => new()
        {
            id = "item_hook_prosthetic",
            providesLimb = new LimbProvision { hands = 1, gripClass = "simple" }
        };

        private static ItemDefinition ArticulatedHand() => new()
        {
            id = "item_articulated_hand",
            providesLimb = new LimbProvision { hands = 1, gripClass = "full" }
        };

        private static ItemDefinition TwoHandedRifle() => new()
        {
            id = "item_bolt_action_rifle",
            isEquipable = true,
            equipSlot = EquipSlot.Weapon,
            limbRequirements = new LimbRequirement { hands = 2, gripClass = "full" }
        };

        private static ItemDefinition SimpleKnife() => new()
        {
            id = "item_hunting_knife",
            isEquipable = true,
            equipSlot = EquipSlot.Weapon,
            limbRequirements = new LimbRequirement { hands = 1, gripClass = "simple" }
        };

        private static ItemDefinition FullGripPistol() => new()
        {
            id = "item_combat_pistol",
            isEquipable = true,
            equipSlot = EquipSlot.Weapon,
            limbRequirements = new LimbRequirement { hands = 1, gripClass = "full" }
        };

        private static ItemDefinition LegacyMask() => new()
        {
            id = "item_gas_mask",
            isEquipable = true,
            equipSlot = EquipSlot.Face
        };

        private static ItemDefinition? MockCatalog(string id) => id switch
        {
            "item_hook_prosthetic" => HookProsthetic(),
            "item_articulated_hand" => ArticulatedHand(),
            _ => null
        };

        [Fact]
        public void BothArmsIntact_TwoHandedFullGrip_CanEquip()
        {
            var limbs = IntactLimbs();
            var rifle = TwoHandedRifle();
            Assert.True(EquipLimbGate.CanEquip(rifle, limbs, MockCatalog));
        }

        [Fact]
        public void RightArmAmputated_TwoHandedItem_CannotEquip()
        {
            var limbs = OneArmAmputated();
            var rifle = TwoHandedRifle();
            Assert.False(EquipLimbGate.CanEquip(rifle, limbs, MockCatalog));
        }

        [Fact]
        public void RightArmHook_LeftAmputated_TwoHandedItem_CannotEquip()
        {
            var limbs = HookAndAmputated();
            var rifle = TwoHandedRifle();
            Assert.False(EquipLimbGate.CanEquip(rifle, limbs, MockCatalog));
        }

        [Fact]
        public void RightArmHook_LeftAmputated_SingleSimpleItem_CanEquip()
        {
            var limbs = HookAndAmputated();
            var knife = SimpleKnife();
            Assert.True(EquipLimbGate.CanEquip(knife, limbs, MockCatalog));
        }

        [Fact]
        public void RightArmHook_LeftAmputated_SingleFullGripItem_CannotEquip()
        {
            var limbs = HookAndAmputated();
            var pistol = FullGripPistol();
            Assert.False(EquipLimbGate.CanEquip(pistol, limbs, MockCatalog));
        }

        [Fact]
        public void BothArmsArticulated_TwoHandedFullGrip_CanEquip()
        {
            var limbs = TwoArticulatedHands();
            var rifle = TwoHandedRifle();
            Assert.True(EquipLimbGate.CanEquip(rifle, limbs, MockCatalog));
        }

        [Fact]
        public void FailedProsthetic_ZeroDurability_CannotProvideLimb()
        {
            var limbs = HookAndAmputated();
            var knife = SimpleKnife();
            // Condition provider returns 0 -> broken prosthetic
            Assert.False(EquipLimbGate.CanEquip(knife, limbs, MockCatalog, _ => 0f));
        }

        [Fact]
        public void LegacyItemWithoutRequirements_CanEquipRegardlessOfLimbs()
        {
            var limbs = new List<LimbState>
            {
                new LimbState { limb = LimbId.LeftArm, condition = LimbCondition.Amputated },
                new LimbState { limb = LimbId.RightArm, condition = LimbCondition.Amputated }
            };
            var mask = LegacyMask();
            Assert.True(EquipLimbGate.CanEquip(mask, limbs, MockCatalog));
        }
    }
}
