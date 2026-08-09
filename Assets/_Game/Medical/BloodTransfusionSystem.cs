using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Blood Types & Transfusions (Prompt #55). Survivors have a BloodType trait.
    /// Transfusions via MedicalBed can save a BloodLoss victim but incompatible
    /// blood causes AnaphylacticShock (lethal in 12h without adrenaline).
    /// BloodTestingKits (rare item) reveal blood types safely.
    /// Save/load safe. Plain C#.
    /// </summary>
    public enum BloodType
    {
        Unknown = 0,
        A = 1,
        B = 2,
        AB = 3,
        O = 4
    }

    public static class BloodTypeExtensions
    {
        /// <summary>Whether donor blood is compatible with recipient.</summary>
        /// O- is universal donor, AB+ is universal recipient (simplified: no Rh factor).
        public static bool IsCompatibleWith(this BloodType donor, BloodType recipient)
        {
            if (donor == BloodType.Unknown || recipient == BloodType.Unknown)
                return false;

            // O can donate to anyone (universal donor).
            if (donor == BloodType.O) return true;

            // AB can receive from anyone (universal recipient).
            if (recipient == BloodType.AB) return true;

            // Same type is always compatible.
            if (donor == recipient) return true;

            // A can donate to A and AB (AB already covered above).
            // B can donate to B and AB (AB already covered above).
            // Any other combination is incompatible.
            return false;
        }

        public static string DisplayName(this BloodType type)
        {
            switch (type)
            {
                case BloodType.A: return "A";
                case BloodType.B: return "B";
                case BloodType.AB: return "AB";
                case BloodType.O: return "O";
                default: return "?";
            }
        }
    }

    /// <summary>
    /// Blood transfusion system. Survivors have a hidden BloodType (default Unknown).
    /// BloodTestingKit reveals it. Transfusions heal BloodLoss but incompatible
    /// blood inflicts AnaphylacticShock.
    /// </summary>
    public class BloodTransfusionSystem
    {
        public const string BloodTestingKitItemId = "blood_testing_kit";
        public const string AdrenalineItemId = "adrenaline";
        public const string BloodDrawKitItemId = "blood_draw_kit";

        public const string AnaphylacticShockId = "anaphylactic_shock";
        public const float AnaphylacticShockLethalHours = 12f;
        public const float TransfusionBloodLossHeal = 40f; // health restored
        public const float TransfusionDonorFatigue = 20f;
        public const float TransfusionDonorHealthCost = 8f;

        /// <summary>Blood type per survivor id. Unknown until tested.</summary>
        private readonly Dictionary<string, BloodType> _bloodTypes = new Dictionary<string, BloodType>();

        /// <summary>Survivor ids whose blood type has been tested/revealed.</summary>
        private readonly HashSet<string> _testedSurvivors = new HashSet<string>();

        private readonly System.Random _rng;
        private Func<string, Survivors.Survivor> _findSurvivor;
        private Action<Survivors.Survivor, string> _inflictAffliction;

        // -- Events --
        public event Action<Survivors.Survivor, BloodType> OnBloodTypeRevealed;
        public event Action<Survivors.Survivor, Survivors.Survivor, bool> OnTransfusionPerformed; // donor, recipient, compatible

        public BloodTransfusionSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(55);
        }

        public void Bind(
            Func<string, Survivors.Survivor> findSurvivor,
            Action<Survivors.Survivor, string> inflictAffliction)
        {
            _findSurvivor = findSurvivor;
            _inflictAffliction = inflictAffliction;
        }

        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        /// <summary>Get or assign a random blood type for a survivor (distribution: O=44%, A=42%, B=10%, AB=4%).</summary>
        public BloodType GetBloodType(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return BloodType.Unknown;

            if (!_bloodTypes.TryGetValue(survivorId, out var type))
            {
                double roll = _rng.NextDouble();
                if (roll < 0.44) type = BloodType.O;
                else if (roll < 0.86) type = BloodType.A;
                else if (roll < 0.96) type = BloodType.B;
                else type = BloodType.AB;
                _bloodTypes[survivorId] = type;
            }
            return type;
        }

        /// <summary>Whether a survivor's blood type has been tested/revealed to the player.</summary>
        public bool IsTested(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _testedSurvivors.Contains(survivorId);
        }

        /// <summary>Use a BloodTestingKit to reveal a survivor's blood type. Returns the type.</summary>
        public BloodType TestBlood(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return BloodType.Unknown;
            var type = GetBloodType(survivorId);
            _testedSurvivors.Add(survivorId);

            var sv = _findSurvivor?.Invoke(survivorId);
            OnBloodTypeRevealed?.Invoke(sv, type);
            return type;
        }

        /// <summary>
        /// Perform a transfusion from donor to recipient. Heals BloodLoss on recipient.
        /// If blood types are incompatible, inflicts AnaphylacticShock on recipient.
        /// Returns true if compatible, false if incompatible.
        /// </summary>
        public bool PerformTransfusion(string donorId, string recipientId)
        {
            if (string.IsNullOrEmpty(donorId) || string.IsNullOrEmpty(recipientId))
                return false;
            if (donorId == recipientId) return false;

            var donorType = GetBloodType(donorId);
            var recipientType = GetBloodType(recipientId);
            bool compatible = donorType.IsCompatibleWith(recipientType);

            var donor = _findSurvivor?.Invoke(donorId);
            var recipient = _findSurvivor?.Invoke(recipientId);

            if (donor == null || recipient == null || !donor.IsAlive || !recipient.IsAlive)
                return false;

            // Donor cost.
            SurvivorNeedWrite.SetHealth(donor, donor.Needs.Health - TransfusionDonorHealthCost);
            if (_needsSystem != null)
                _needsSystem.Modify(donor, NeedKind.Fatigue, TransfusionDonorFatigue);
            else
                donor.Needs.Fatigue = Mathf.Clamp(donor.Needs.Fatigue + TransfusionDonorFatigue, 0f, 100f);

            if (compatible)
            {
                // Heal BloodLoss on recipient.
                SurvivorNeedWrite.AdjustHealth(recipient, TransfusionBloodLossHeal);
                if (_needsSystem != null)
                    _needsSystem.Modify(recipient, NeedKind.Morale, 5f);
                else
                    recipient.Needs.Morale = Mathf.Clamp(recipient.Needs.Morale + 5f, 0f, 100f);
            }
            else
            {
                // Anaphylactic shock — lethal in 12h without adrenaline.
                _inflictAffliction?.Invoke(recipient, AnaphylacticShockId);
                SurvivorNeedWrite.AdjustHealth(recipient, -20f);
            }

            OnTransfusionPerformed?.Invoke(donor, recipient, compatible);
            return compatible;
        }

        /// <summary>Force a blood type for tests/scripted survivors.</summary>
        public void SetBloodType(string survivorId, BloodType type)
        {
            if (!string.IsNullOrEmpty(survivorId))
                _bloodTypes[survivorId] = type;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public BloodTransfusionSave CaptureState()
        {
            var keys = new string[_bloodTypes.Count];
            var values = new int[_bloodTypes.Count];
            int i = 0;
            foreach (var kv in _bloodTypes)
            {
                keys[i] = kv.Key;
                values[i] = (int)kv.Value;
                i++;
            }
            var tested = new string[_testedSurvivors.Count];
            _testedSurvivors.CopyTo(tested);
            return new BloodTransfusionSave
            {
                BloodTypeKeys = keys,
                BloodTypeValues = values,
                TestedSurvivorIds = tested
            };
        }

        public void RestoreState(BloodTransfusionSave save)
        {
            _bloodTypes.Clear();
            _testedSurvivors.Clear();
            if (save == null) return;
            if (save.BloodTypeKeys != null)
            {
                for (int i = 0; i < save.BloodTypeKeys.Length; i++)
                {
                    if (string.IsNullOrEmpty(save.BloodTypeKeys[i])) continue;
                    int val = save.BloodTypeValues != null && i < save.BloodTypeValues.Length
                        ? save.BloodTypeValues[i] : 0;
                    _bloodTypes[save.BloodTypeKeys[i]] = (BloodType)Mathf.Clamp(val, 0, 4);
                }
            }
            if (save.TestedSurvivorIds != null)
            {
                for (int i = 0; i < save.TestedSurvivorIds.Length; i++)
                {
                    if (!string.IsNullOrEmpty(save.TestedSurvivorIds[i]))
                        _testedSurvivors.Add(save.TestedSurvivorIds[i]);
                }
            }
        }
    }

    [Serializable]
    public class BloodTransfusionSave
    {
        public string[] BloodTypeKeys;
        public int[] BloodTypeValues;
        public string[] TestedSurvivorIds;
    }
}
