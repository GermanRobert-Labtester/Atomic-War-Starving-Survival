using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BloodTypesState
    {
        public string system_id = "system_blood_types";
        // parallel lists: survivorId → blood type string
        public List<string> survivor_ids = new List<string>();
        public List<string> blood_types = new List<string>();
        // survivors whose blood type has been tested
        public List<string> tested_ids = new List<string>();
        // survivors currently in hemolytic shock
        public List<string> hemolytic_shock_ids = new List<string>();
    }

    /// <summary>
    /// Prompt #829: Blood Types.
    /// Every survivor is assigned A, B, AB, or O. Wrong BloodBag type during
    /// transfusion causes HemolyticShock — Coma with 80 % death chance.
    /// Player must use TestKits to catalog crew blood types before transfusing.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class System_BloodTypes
    {
        // ── Constants ──────────────────────────────────────────────────
        public const string TYPE_A = "A";
        public const string TYPE_B = "B";
        public const string TYPE_AB = "AB";
        public const string TYPE_O = "O";

        private const float HEMOLYTIC_DEATH_CHANCE = 0.80f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, string> OnBloodTypeDiscovered;   // survivorId, type
        public event Action<string, string> OnTransfusionStarted;    // recipientId, bagType
        public event Action<string> OnHemolyticShock;                // survivorId
        public event Action<string> OnDeath;                         // survivorId
        public event Action<string> OnSurvival;                      // survivorId

        // ── State ──────────────────────────────────────────────────────
        private readonly Dictionary<string, string> _bloodTypes = new Dictionary<string, string>();
        private readonly HashSet<string> _testedIds = new HashSet<string>();
        private readonly HashSet<string> _hemolyticShockActive = new HashSet<string>();

        private System.Random _rng = AtomicWar._Game.Utilities.SeededRandom.Create(
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed, "system_bloodtypes");

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>Inject seeded RNG (bootstrap world seed) for deterministic assign.</summary>
        public void SetRng(System.Random rng) => _rng = rng ?? AtomicWar._Game.Utilities.SeededRandom.Create(
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed, "system_bloodtypes");

        /// <summary>
        /// Assign a blood type to a survivor. Call once per survivor at
        /// creation or recruitment.
        /// </summary>
        public void AssignBloodType(string survivorId, string type)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(type)) return;
            if (!IsValidType(type))
            {
                Debug.LogWarning($"[System_BloodTypes] Invalid blood type '{type}'.");
                return;
            }

            _bloodTypes[survivorId] = type;
        }

        /// <summary>
        /// Assign a random blood type if none exists (O 44% / A 42% / B 10% / AB 4%).
        /// Returns the assigned type.
        /// </summary>
        public string EnsureBloodType(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            if (_bloodTypes.TryGetValue(survivorId, out var existing) && IsValidType(existing))
                return existing;

            double roll = _rng.NextDouble();
            string type;
            if (roll < 0.44) type = TYPE_O;
            else if (roll < 0.86) type = TYPE_A;
            else if (roll < 0.96) type = TYPE_B;
            else type = TYPE_AB;
            _bloodTypes[survivorId] = type;
            return type;
        }

        /// <summary>
        /// Bag transfusion path (Prompt #829). On incompatible bag → hemolytic shock,
        /// then immediate ResolveShock (80% death). Returns true if compatible.
        /// </summary>
        public bool TryTransfuseBag(string recipientId, string bagType, out bool died)
        {
            died = false;
            if (string.IsNullOrEmpty(recipientId) || string.IsNullOrEmpty(bagType))
                return false;

            EnsureBloodType(recipientId);
            if (!_bloodTypes.TryGetValue(recipientId, out var recipientType))
                return false;

            OnTransfusionStarted?.Invoke(recipientId, bagType);

            if (CheckCompatibility(recipientType, bagType))
                return true;

            _hemolyticShockActive.Add(recipientId);
            OnHemolyticShock?.Invoke(recipientId);

            // Resolve immediately for bag path (no separate wait UI yet).
            bool death = false;
            Action<string> onDeath = id => { if (id == recipientId) death = true; };
            OnDeath += onDeath;
            try { ResolveShock(recipientId); }
            finally { OnDeath -= onDeath; }

            died = death;
            return false;
        }

        /// <summary>
        /// Player uses a TestKit to discover a survivor's blood type.
        /// Fires OnBloodTypeDiscovered.
        /// </summary>
        public void TestBlood(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;

            if (!_bloodTypes.TryGetValue(survivorId, out var type))
            {
                Debug.LogWarning($"[System_BloodTypes] No blood type assigned for '{survivorId}'.");
                return;
            }

            _testedIds.Add(survivorId);
            OnBloodTypeDiscovered?.Invoke(survivorId, type);
        }

        /// <summary>
        /// Attempt a transfusion. If the bag type is incompatible the
        /// recipient enters hemolytic shock (80 % death chance).
        /// </summary>
        public void Transfuse(string recipientId, string bagType)
        {
            if (string.IsNullOrEmpty(recipientId) || string.IsNullOrEmpty(bagType)) return;

            if (!_bloodTypes.TryGetValue(recipientId, out var recipientType))
            {
                Debug.LogWarning($"[System_BloodTypes] No blood type for recipient '{recipientId}'.");
                return;
            }

            OnTransfusionStarted?.Invoke(recipientId, bagType);

            if (!CheckCompatibility(recipientType, bagType))
            {
                // Hemolytic shock
                _hemolyticShockActive.Add(recipientId);
                OnHemolyticShock?.Invoke(recipientId);
            }
        }

        /// <summary>
        /// Returns true if the donor type is compatible with the recipient type.
        /// Rules: O is universal donor, AB is universal recipient,
        /// A↔A, B↔B.
        /// </summary>
        public bool CheckCompatibility(string recipientType, string donorType)
        {
            if (string.IsNullOrEmpty(recipientType) || string.IsNullOrEmpty(donorType))
                return false;

            // O is universal donor
            if (donorType == TYPE_O) return true;
            // AB is universal recipient
            if (recipientType == TYPE_AB) return true;
            // Exact match
            return recipientType == donorType;
        }

        /// <summary>
        /// Resolve hemolytic shock for a survivor — 80 % death, 20 % survival.
        /// </summary>
        public void ResolveShock(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;

            if (!_hemolyticShockActive.Remove(survivorId))
            {
                Debug.LogWarning($"[System_BloodTypes] '{survivorId}' is not in hemolytic shock.");
                return;
            }

            float roll = (float)_rng.NextDouble();
            if (roll < HEMOLYTIC_DEATH_CHANCE)
            {
                OnDeath?.Invoke(survivorId);
            }
            else
            {
                OnSurvival?.Invoke(survivorId);
            }
        }

        /// <summary>Returns the assigned blood type for a survivor, or null.</summary>
        public string GetBloodType(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            return _bloodTypes.TryGetValue(survivorId, out var t) ? t : null;
        }

        /// <summary>Returns true if the survivor's blood type has been tested.</summary>
        public bool IsTested(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _testedIds.Contains(survivorId);
        }

        /// <summary>Returns true if the survivor is currently in hemolytic shock.</summary>
        public bool IsInShock(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _hemolyticShockActive.Contains(survivorId);
        }

        // ── Helpers ────────────────────────────────────────────────────

        private static bool IsValidType(string type)
        {
            return type == TYPE_A || type == TYPE_B || type == TYPE_AB || type == TYPE_O;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public BloodTypesState CaptureState()
        {
            var state = new BloodTypesState
            {
                system_id = "system_blood_types",
                survivor_ids = new List<string>(),
                blood_types = new List<string>(),
                tested_ids = new List<string>(_testedIds),
                hemolytic_shock_ids = new List<string>(_hemolyticShockActive)
            };

            foreach (var kvp in _bloodTypes)
            {
                state.survivor_ids.Add(kvp.Key);
                state.blood_types.Add(kvp.Value);
            }

            return state;
        }

        public void RestoreState(BloodTypesState saved)
        {
            _bloodTypes.Clear();
            _testedIds.Clear();
            _hemolyticShockActive.Clear();

            if (saved == null) return;

            if (saved.survivor_ids != null && saved.blood_types != null)
            {
                int count = Mathf.Min(saved.survivor_ids.Count, saved.blood_types.Count);
                for (int i = 0; i < count; i++)
                {
                    if (string.IsNullOrEmpty(saved.survivor_ids[i])) continue;
                    _bloodTypes[saved.survivor_ids[i]] = saved.blood_types[i];
                }
            }

            if (saved.tested_ids != null)
            {
                foreach (var id in saved.tested_ids)
                    if (!string.IsNullOrEmpty(id)) _testedIds.Add(id);
            }

            if (saved.hemolytic_shock_ids != null)
            {
                foreach (var id in saved.hemolytic_shock_ids)
                    if (!string.IsNullOrEmpty(id)) _hemolyticShockActive.Add(id);
            }
        }
    }
}
