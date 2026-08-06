using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public enum AmmoType
    {
        Standard,
        ArmorPiercing,
        HollowPoint
    }

    [Serializable]
    public class AmmoTypeState
    {
        public string itemIdStandard = "item_ammo_standard";
        public string itemIdAP = "item_ammo_ap";
        public string itemIdHP = "item_ammo_hp";
    }

    public class Item_AmmoTypes
    {
        // Events
        public event Action<string, AmmoType, float> OnDamageModified;  // targetId, ammoType, multiplier

        // Internal state
        private readonly AmmoTypeState _state;

        // Target type constants (snake_case ids)
        private const string TARGET_MUTANT = "mutant";
        private const string TARGET_ANIMAL = "animal";
        private const string TARGET_WARLORD = "warlord";

        public Item_AmmoTypes()
        {
            _state = new AmmoTypeState();
        }

        /// <summary>
        /// Get the damage multiplier for a given ammo type vs a target.
        /// AP: ignores Kevlar but 0.5x vs Mutants.
        /// HP: 2.0x vs unarmored Mutants/Animals, 0x vs Armored Warlords.
        /// Standard: 1.0x always.
        /// </summary>
        public float GetDamageMultiplier(AmmoType ammo, string targetType, bool hasKevlar)
        {
            float multiplier;

            switch (ammo)
            {
                case AmmoType.ArmorPiercing:
                    // Ignores Kevlar entirely, but reduced vs Mutants
                    if (string.Equals(targetType, TARGET_MUTANT, StringComparison.OrdinalIgnoreCase))
                    {
                        multiplier = 0.5f;
                    }
                    else
                    {
                        multiplier = 1.0f; // Kevlar is ignored, full damage
                    }
                    break;

                case AmmoType.HollowPoint:
                    // 2.0x vs unarmored Mutants/Animals
                    if (!hasKevlar && (
                        string.Equals(targetType, TARGET_MUTANT, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(targetType, TARGET_ANIMAL, StringComparison.OrdinalIgnoreCase)))
                    {
                        multiplier = 2.0f;
                    }
                    // 0x vs Armored Warlords
                    else if (hasKevlar && string.Equals(targetType, TARGET_WARLORD, StringComparison.OrdinalIgnoreCase))
                    {
                        multiplier = 0.0f;
                    }
                    else
                    {
                        multiplier = 1.0f;
                    }
                    break;

                case AmmoType.Standard:
                default:
                    multiplier = 1.0f;
                    break;
            }

            return multiplier;
        }

        /// <summary>
        /// Apply damage modification and fire event.
        /// Returns the modified damage value.
        /// </summary>
        public float ApplyDamage(string targetId, AmmoType ammo, string targetType, bool hasKevlar, float baseDamage)
        {
            float multiplier = GetDamageMultiplier(ammo, targetType, hasKevlar);
            OnDamageModified?.Invoke(targetId, ammo, multiplier);
            return baseDamage * multiplier;
        }

        /// <summary>
        /// Get the snake_case item ID for an ammo type.
        /// </summary>
        public string GetAmmoId(AmmoType type)
        {
            switch (type)
            {
                case AmmoType.ArmorPiercing:
                    return _state.itemIdAP;
                case AmmoType.HollowPoint:
                    return _state.itemIdHP;
                case AmmoType.Standard:
                default:
                    return _state.itemIdStandard;
            }
        }

        public AmmoTypeState CaptureState()
        {
            return new AmmoTypeState
            {
                itemIdStandard = _state.itemIdStandard,
                itemIdAP = _state.itemIdAP,
                itemIdHP = _state.itemIdHP
            };
        }

        public void RestoreState(AmmoTypeState saved)
        {
            if (saved == null) return;
            _state.itemIdStandard = saved.itemIdStandard;
            _state.itemIdAP = saved.itemIdAP;
            _state.itemIdHP = saved.itemIdHP;
        }
    }
}
