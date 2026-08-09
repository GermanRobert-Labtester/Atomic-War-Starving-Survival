using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GlowingDeadState
    {
        public string encounter_id = "encounter_glowing_dead";
        public float rad_transfer_amount = 50f;
    }

    /// <summary>DEMOTE-Encounter-batch — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public sealed class Encounter_GlowingDead
    {
        private GlowingDeadState _state;

        public event Action<string, float> OnRadTransferredToInventory;
        public event Action<string, string> OnItemIrradiated;

        public string EncounterId => _state.encounter_id;

        public Encounter_GlowingDead()
        {
            _state = new GlowingDeadState();
        }

        public void LootCorpse(string scavenger_id, List<string> inventory_item_ids)
        {
            if (string.IsNullOrEmpty(scavenger_id))
            {
                Debug.LogError("[Encounter_GlowingDead] scavenger_id is null or empty.");
                return;
            }

            if (inventory_item_ids == null)
            {
                Debug.LogError("[Encounter_GlowingDead] inventory_item_ids is null.");
                return;
            }

            OnRadTransferredToInventory?.Invoke(scavenger_id, _state.rad_transfer_amount);
            GameLog.Log($"[Encounter_GlowingDead] Scavenger '{scavenger_id}' absorbed {_state.rad_transfer_amount} rad from corpse.");

            foreach (string item_id in inventory_item_ids)
            {
                if (string.IsNullOrEmpty(item_id))
                    continue;

                if (IsFoodOrWater(item_id))
                {
                    OnItemIrradiated?.Invoke(scavenger_id, item_id);
                    GameLog.Log($"[Encounter_GlowingDead] Item '{item_id}' irradiated in scavenger '{scavenger_id}' inventory.");
                }
            }
        }

        private static bool IsFoodOrWater(string item_id)
        {
            if (string.IsNullOrEmpty(item_id))
                return false;

            string lower = item_id.ToLowerInvariant();
            return lower.Contains("food") || lower.Contains("water") ||
                   lower.Contains("ration") || lower.Contains("canned") ||
                   lower.Contains("meal") || lower.Contains("drink");
        }

        public GlowingDeadState CaptureState()
        {
            return new GlowingDeadState
            {
                encounter_id = _state.encounter_id,
                rad_transfer_amount = _state.rad_transfer_amount
            };
        }

        public void RestoreState(GlowingDeadState saved)
        {
            _state = saved ?? new GlowingDeadState();
        }
    }
}
