using System;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class HeirloomState
    {
        public string itemId = "item_heirloom";
        public float efficiencyBonus = 0.5f;
        public float moraleBuff = 0.1f;
        public string originalOwnerId = "";
        public string toolType = "";
        public string equippedById = "";
        public bool isCreated = false;
    }

    public class Item_Heirloom
    {
        public event Action<string, string, string> OnHeirloomCreated;
        public event Action<string, string> OnHeirloomEquipped;
        public event Action<string, float> OnEfficiencyBoosted;

        private readonly HeirloomState _state;

        public Item_Heirloom()
        {
            _state = new HeirloomState();
        }

        public void CreateHeirloom(string originalToolId, string ownerId, string toolType)
        {
            _state.itemId = originalToolId + "_heirloom";
            _state.originalOwnerId = ownerId;
            _state.toolType = toolType;
            _state.isCreated = true;
            OnHeirloomCreated?.Invoke(_state.itemId, ownerId, toolType);
        }

        public void Equip(string survivorId, bool isBiologicalChild)
        {
            _state.equippedById = survivorId;
            OnHeirloomEquipped?.Invoke(survivorId, _state.itemId);

            if (isBiologicalChild)
            {
                OnEfficiencyBoosted?.Invoke(survivorId, _state.efficiencyBonus);
            }
        }

        public float GetEfficiencyMultiplier(bool isBiologicalChild)
        {
            return isBiologicalChild ? 1f + _state.efficiencyBonus : 1f;
        }

        public float GetMoraleBuff(bool isBiologicalChild)
        {
            return isBiologicalChild ? _state.moraleBuff : 0f;
        }

        public HeirloomState CaptureState() => _state;

        public void RestoreState(HeirloomState state)
        {
            _state.itemId = state.itemId;
            _state.efficiencyBonus = state.efficiencyBonus;
            _state.moraleBuff = state.moraleBuff;
            _state.originalOwnerId = state.originalOwnerId;
            _state.toolType = state.toolType;
            _state.equippedById = state.equippedById;
            _state.isCreated = state.isCreated;
        }
    }
}
