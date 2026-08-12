using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion VIII — Psychological Friction & Bureaucratic Mechanics.
    /// New survivor archetypes whose specific neuroses are either salvation or undoing.
    /// The Actuary who blocks risky expeditions, the Forger who crafts transit passes,
    /// the Foley who hears everything, the Notary who processes guilt into paperwork,
    /// the Blood Courier who sees bodies as plasma vessels.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class PsychologicalFrictionSystem
    {
        // ── Archetype ids ─────────────────────────────────────────────
        public const string Arch_Actuary = "the_actuary";
        public const string Arch_Forger = "the_forger";
        public const string Arch_Foley = "the_foley";
        public const string Arch_Notary = "the_notary";
        public const string Arch_BloodCourier = "the_blood_courier";

        // ── Trait ids ─────────────────────────────────────────────────
        public const string Trait_RiskParalysis = "trait_risk_paralysis";
        public const string Trait_BureaucraticChameleon = "trait_bureaucratic_chameleon";
        public const string Trait_Hyperacusis = "trait_hyperacusis";
        public const string Trait_TheStamp = "trait_the_stamp";
        public const string Trait_ColdChainObsession = "trait_cold_chain_obsession";

        // ── Affliction ids ────────────────────────────────────────────
        public const string Affliction_CalculatingGuilt = "affliction_calculating_guilt";
        public const string Affliction_MigraineBlindness = "affliction_migraine_blindness";
        public const string Affliction_Hypoxia = "affliction_hypoxia";
        public const string Affliction_BrainDamage = "affliction_brain_damage";

        // ── Actuary constants ─────────────────────────────────────────
        public const float Actuary_CasualtyThreshold = 0.35f; // >35% → refuses
        public const float Actuary_HoardCurrencyChance = 0.20f;

        // ── Forger constants ──────────────────────────────────────────
        public const float ForgeryDetectionChance = 0.15f;
        public const string Item_TransitPassForged = "transit_pass_forged";
        public const string Item_RationCardFake = "ration_card_fake";
        public const string Item_StampMinistry = "stamp_ministry_official";
        public const string Item_InkIndelible = "ink_indelible";

        // ── Foley constants ───────────────────────────────────────────
        public const float Foley_RaidPredictionHours = 12f;
        public const float Foley_SoundproofingMaterialReduction = 0.50f;
        public const float Foley_MigraineNoiseThreshold = 60f;

        // ── Notary constants ──────────────────────────────────────────
        public const string Item_Cigarette = "cigarette";
        public const string Item_CoffeeBean = "coffee_arabica_bean";

        // ── Blood Courier constants ───────────────────────────────────
        public const string Module_ICEBox = "ice_box";
        public const string Item_BloodBag = "blood_bag";

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnRiskParalysisTriggered;
        public event Action<string> OnForgeryDetected;
        public event Action<string> OnFoleyRaidPrediction;
        public event Action<string> OnFoleyMigraine;
        public event Action<string> OnTribunalInitiated;
        public event Action<string> OnNotarizationComplete;
        public event Action<string> OnPlasmaHarvestAttempt;
        public event Action<string> OnExpeditionBlocked;

        private readonly System.Random _rng;
        private readonly HashSet<string> _registeredArchetypes = new HashSet<string>();
        private int _forgeriesDetected;
        private int _tribunalsHeld;
        private int _plasmaHarvests;

        public int ForgeriesDetected => _forgeriesDetected;
        public int TribunalsHeld => _tribunalsHeld;

        public PsychologicalFrictionSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(8000);
        }

        // ── Archetype registration ────────────────────────────────────

        public void RegisterArchetype(string archetypeId)
        {
            _registeredArchetypes.Add(archetypeId);
        }

        public bool HasArchetype(string archetypeId) => _registeredArchetypes.Contains(archetypeId);

        // ── The Actuary: Risk Paralysis ───────────────────────────────

        /// <summary>
        /// Check if the Actuary blocks an expedition due to casualty risk.
        /// Returns true if the expedition is blocked.
        /// </summary>
        public bool CheckRiskParalysis(float casualtyChance)
        {
            if (!HasArchetype(Arch_Actuary)) return false;
            if (casualtyChance > Actuary_CasualtyThreshold)
            {
                OnRiskParalysisTriggered?.Invoke(Arch_Actuary);
                OnExpeditionBlocked?.Invoke(Arch_Actuary);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Force the Actuary to approve a risky action. Develops CalculatingGuilt.
        /// </summary>
        public void ForceActuaryApproval(string actuaryId)
        {
            OnRiskParalysisTriggered?.Invoke(actuaryId);
        }

        // ── The Forger: Transit Passes ────────────────────────────────

        /// <summary>
        /// Craft a forged transit pass. Requires stamp, ink, and blood.
        /// </summary>
        public ForgerResult CraftTransitPass(string forgerId, bool hasStamp,
            bool hasInk, bool hasBlood, float intellectSkill)
        {
            if (!HasArchetype(Arch_Forger))
                return new ForgerResult { Success = false, NoForger = true };

            if (!hasStamp || !hasInk || !hasBlood)
                return new ForgerResult { Success = false, MissingMaterials = true };

            float quality = Mathf.Clamp01(intellectSkill);
            return new ForgerResult
            {
                Success = true,
                Quality = quality,
                ItemCrafted = Item_TransitPassForged,
                Message = "The ink smells like copper. The stamp is heavy. The pass looks real to a tired guard at a dark checkpoint."
            };
        }

        /// <summary>
        /// Check if a forgery is detected during use.
        /// </summary>
        public bool CheckForgeryDetection(string itemId, float quality)
        {
            float detectionChance = ForgeryDetectionChance * (1f - quality);
            if (_rng.NextDouble() < detectionChance)
            {
                _forgeriesDetected++;
                OnForgeryDetected?.Invoke(itemId);
                return true;
            }
            return false;
        }

        // ── The Foley: Acoustic Perception ────────────────────────────

        /// <summary>
        /// The Foley predicts a night raid 12 hours in advance.
        /// </summary>
        public bool PredictRaid(float noiseLevel)
        {
            if (!HasArchetype(Arch_Foley)) return false;
            if (noiseLevel > 30f) // Significant acoustic shift
            {
                OnFoleyRaidPrediction?.Invoke(Arch_Foley);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the Foley suffers a migraine from ambient noise.
        /// </summary>
        public bool CheckFoleyMigraine(float ambientNoiseDB)
        {
            if (!HasArchetype(Arch_Foley)) return false;
            if (ambientNoiseDB >= Foley_MigraineNoiseThreshold)
            {
                OnFoleyMigraine?.Invoke(Arch_Foley);
                return true;
            }
            return false;
        }

        /// <summary>Get material reduction for soundproofing crafting.</summary>
        public float GetSoundproofingMaterialMultiplier()
        {
            return HasArchetype(Arch_Foley) ? Foley_SoundproofingMaterialReduction : 1f;
        }

        // ── The Notary: Tribunal System ───────────────────────────────

        /// <summary>
        /// Initiate a tribunal. The Notary stamps the decision, absorbing guilt.
        /// Returns true if the tribunal is valid.
        /// </summary>
        public TribunalResult InitiateTribunal(string notaryId, string disputeType,
            string decision, bool hasFilingFee)
        {
            if (!HasArchetype(Arch_Notary))
                return new TribunalResult { Success = false, NoNotary = true };

            if (!hasFilingFee)
                return new TribunalResult
                {
                    Success = false,
                    NoFilingFee = true,
                    Message = "The Notary demands a filing fee. A cigarette. A coffee bean. Something civilized."
                };

            _tribunalsHeld++;
            OnTribunalInitiated?.Invoke(notaryId);
            OnNotarizationComplete?.Invoke(notaryId);

            return new TribunalResult
            {
                Success = true,
                Decision = decision,
                GuiltAbsorbed = true,
                Message = "THWACK. The stamp hits the paper. The decision is filed. The bureaucracy absorbs the guilt."
            };
        }

        // ── The Blood Courier: Plasma Harvest ─────────────────────────

        /// <summary>
        /// The Blood Courier attempts to harvest plasma from a dying survivor.
        /// Triggers a massive moral dilemma.
        /// </summary>
        public PlasmaResult AttemptPlasmaHarvest(string courierId, string dyingSurvivorId,
            bool isDead)
        {
            if (!HasArchetype(Arch_BloodCourier))
                return new PlasmaResult { Success = false };

            _plasmaHarvests++;
            OnPlasmaHarvestAttempt?.Invoke(courierId);

            return new PlasmaResult
            {
                Success = true,
                IsEthical = isDead,
                BloodBagYield = isDead ? 2 : 1,
                MoraleImpact = isDead ? -5f : -30f,
                Message = isDead
                    ? "The plasma is extracted. The body is lighter. The bag is warm."
                    : "The survivor is still breathing. The needle goes in. The bunker watches."
            };
        }

        /// <summary>Get preservation multiplier for the ICE box.</summary>
        public float GetPreservationMultiplier(bool hasICEBox)
        {
            return (HasArchetype(Arch_BloodCourier) && hasICEBox) ? 0f : 1f; // 0 = no decay
        }

        // ── Save / Load ───────────────────────────────────────────────

        public FrictionSave CaptureState()
        {
            var archetypes = new string[_registeredArchetypes.Count];
            _registeredArchetypes.CopyTo(archetypes);
            return new FrictionSave
            {
                RegisteredArchetypes = archetypes,
                ForgeriesDetected = _forgeriesDetected,
                TribunalsHeld = _tribunalsHeld,
                PlasmaHarvests = _plasmaHarvests
            };
        }

        public void RestoreState(FrictionSave save)
        {
            _registeredArchetypes.Clear();
            _forgeriesDetected = 0;
            _tribunalsHeld = 0;
            _plasmaHarvests = 0;
            if (save == null) return;
            _forgeriesDetected = save.ForgeriesDetected;
            _tribunalsHeld = save.TribunalsHeld;
            _plasmaHarvests = save.PlasmaHarvests;
            if (save.RegisteredArchetypes != null)
                for (int i = 0; i < save.RegisteredArchetypes.Length; i++)
                    if (!string.IsNullOrEmpty(save.RegisteredArchetypes[i]))
                        _registeredArchetypes.Add(save.RegisteredArchetypes[i]);
        }
    }

    [Serializable]
    public class ForgerResult
    {
        public bool Success;
        public bool NoForger;
        public bool MissingMaterials;
        public float Quality;
        public string ItemCrafted;
        public string Message;
    }

    [Serializable]
    public class TribunalResult
    {
        public bool Success;
        public bool NoNotary;
        public bool NoFilingFee;
        public string Decision;
        public bool GuiltAbsorbed;
        public string Message;
    }

    [Serializable]
    public class PlasmaResult
    {
        public bool Success;
        public bool IsEthical;
        public int BloodBagYield;
        public float MoraleImpact;
        public string Message;
    }

    [Serializable]
    public class FrictionSave
    {
        public string[] RegisteredArchetypes;
        public int ForgeriesDetected;
        public int TribunalsHeld;
        public int PlasmaHarvests;
    }
}
