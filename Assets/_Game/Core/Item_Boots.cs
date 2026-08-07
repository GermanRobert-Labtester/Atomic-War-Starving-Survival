using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BootsState
    {
        public string itemId = "item_boots";
        public float durability = 1.0f;
        public float degradationPerHour = 0.05f;
        public bool isWornOut = false;
    }

    
    [Serializable]
    public class ItemBootsSave
    {
        public List<string> keys = new List<string>();
        public List<BootsState> values = new List<BootsState>();
    }
public class Item_Boots
    {
        public event Action<string, float> OnDurabilityChanged; // survivorId, durability
        public event Action<string> OnBootsWornOut; // survivorId
        public event Action<string> OnBarefootPenalty; // survivorId

        private const float GlassRubbleMultiplier = 3f;
        private const float BarefootSpeedMultiplier = 0.5f;
        private const float NormalSpeedMultiplier = 1.0f;

        private Dictionary<string, BootsState> _survivorBoots = new Dictionary<string, BootsState>();

        public void WalkOnTerrain(string survivorId, string terrainType, float hours)
        {
            BootsState boots = GetOrCreateBoots(survivorId);
            if (boots.isWornOut)
            {
                OnBarefootPenalty?.Invoke(survivorId);
                return;
            }

            float multiplier = 1f;
            if (terrainType == "glass" || terrainType == "rubble")
            {
                multiplier = GlassRubbleMultiplier;
            }

            boots.durability -= boots.degradationPerHour * hours * multiplier;
            if (boots.durability < 0f)
                boots.durability = 0f;

            if (boots.durability <= 0f && !boots.isWornOut)
            {
                boots.isWornOut = true;
                OnBootsWornOut?.Invoke(survivorId);
            }

            OnDurabilityChanged?.Invoke(survivorId, boots.durability);
        }

        public bool IsBarefoot(string survivorId)
        {
            if (!_survivorBoots.TryGetValue(survivorId, out BootsState boots))
                return true;
            return boots.isWornOut;
        }

        public float GetSpeedMultiplier(string survivorId)
        {
            if (IsBarefoot(survivorId))
                return BarefootSpeedMultiplier;
            return NormalSpeedMultiplier;
        }

        public void AccumulateBarefootDamage(string survivorId)
        {
            if (IsBarefoot(survivorId))
            {
                OnBarefootPenalty?.Invoke(survivorId);
            }
        }

        public void Repair(string survivorId, float amount)
        {
            BootsState boots = GetOrCreateBoots(survivorId);
            boots.durability += amount;
            if (boots.durability > 1.0f)
                boots.durability = 1.0f;

            if (boots.durability > 0f)
                boots.isWornOut = false;

            OnDurabilityChanged?.Invoke(survivorId, boots.durability);
        }

        public ItemBootsSave CaptureState()
        {
            var save = new ItemBootsSave();
            foreach (var kvp in _survivorBoots)
            {
                save.keys.Add(kvp.Key);
                save.values.Add(kvp.Value);
            }
            return save;
        }

        public void RestoreState(ItemBootsSave saved)
        {
            _survivorBoots.Clear();
            if (saved == null || saved.keys == null) return;
            for (int i = 0; i < saved.keys.Count; i++)
            {
                var val = (saved.values != null && i < saved.values.Count) ? saved.values[i] : null;
                if (val != null) _survivorBoots[saved.keys[i]] = val;
            }
        }

        private BootsState GetOrCreateBoots(string survivorId)
        {
            if (!_survivorBoots.TryGetValue(survivorId, out BootsState boots))
            {
                boots = new BootsState();
                _survivorBoots[survivorId] = boots;
            }
            return boots;
        }
    }
}
