using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class NanitesState
    {
        public string itemId = "item_nanites";
        public bool healsAllDamage = true;
        public float floraRejectionDamage = 60f;
        // Track injection and rejection history
        public bool hasBeenInjected = false;
        public int injectionCount = 0;
    }

    /// <summary>
    /// Nanite Injectors — microscopic machines that cure all physical damage
    /// instantly. However, nanites aggressively attack mutated fungi on contact,
    /// causing extreme internal bleeding if the survivor consumes any mutated
    /// flora while nanites are active in their bloodstream.
    /// Prompt #797: Item_Nanites
    /// </summary>
    public class Item_Nanites
    {
        // -- Constants --
        public const float FloraRejectionDamage = 60f;
        private static readonly string[] MutatedFungiPrefixes = { "food_mutated_fungi", "food_mutated_mushroom", "food_fungi" };

        // -- Events --
        public event Action<string> OnHealingApplied;         // survivorId
        public event Action<string, float> OnFloraRejection;  // survivorId, damage

        // -- State --
        private bool _hasBeenInjected = false;
        private int _injectionCount = 0;

        // -- Public API --

        /// <summary>
        /// Injects nanites into a survivor, healing all physical damage instantly.
        /// </summary>
        public void Inject(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                Debug.LogWarning("[Item_Nanites] Survivor id cannot be null or empty.");
                return;
            }
            _hasBeenInjected = true;
            _injectionCount++;
            OnHealingApplied?.Invoke(survivorId);
        }

        /// <summary>
        /// Called when a survivor with active nanites consumes food.
        /// If the food is a mutated fungi type, nanites attack it causing
        /// extreme internal bleeding. Returns the damage dealt (0 if safe).
        /// </summary>
        public float ConsumeFungi(string survivorId, string foodId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(foodId))
            {
                Debug.LogWarning("[Item_Nanites] Invalid survivor or food id.");
                return 0f;
            }

            if (!_hasBeenInjected) return 0f;

            if (IsMutatedFungi(foodId))
            {
                OnFloraRejection?.Invoke(survivorId, FloraRejectionDamage);
                return FloraRejectionDamage;
            }

            return 0f;
        }

        /// <summary>
        /// Returns true if nanites are currently active in the survivor's bloodstream.
        /// </summary>
        public bool IsActive() => _hasBeenInjected;

        /// <summary>Returns total number of injections administered.</summary>
        public int GetInjectionCount() => _injectionCount;

        // -- Helpers --

        private static bool IsMutatedFungi(string foodId)
        {
            if (string.IsNullOrEmpty(foodId)) return false;
            string lower = foodId.ToLowerInvariant();
            for (int i = 0; i < MutatedFungiPrefixes.Length; i++)
            {
                if (lower.StartsWith(MutatedFungiPrefixes[i]))
                    return true;
            }
            return false;
        }

        // -- Save / Load --

        public NanitesState CaptureState()
        {
            return new NanitesState
            {
                itemId = "item_nanites",
                healsAllDamage = true,
                floraRejectionDamage = FloraRejectionDamage,
                hasBeenInjected = _hasBeenInjected,
                injectionCount = _injectionCount
            };
        }

        public void RestoreState(NanitesState saved)
        {
            if (saved == null) return;
            _hasBeenInjected = saved.hasBeenInjected;
            _injectionCount = saved.injectionCount;
        }
    }
}
