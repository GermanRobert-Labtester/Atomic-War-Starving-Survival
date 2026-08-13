#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion IV — The Forensic Economy of Corpses. A corpse is not a tragedy.
    /// It is 4kg of wet cloth, 0.5kg of rendered fat, and a pair of boots.
    /// Extends CorpseManagementSystem with forensic scavenging: stripping bodies
    /// for materials, rendering fat, extracting dental gold, and the psychological
    /// toll of wearing a dead person's skin.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class ForensicScavengingSystem
    {
        // ── Action constants ──────────────────────────────────────────
        public const float StripCorpseHours = 1.5f;
        public const int ClothYieldMin = 3;
        public const int ClothYieldMax = 5;
        public const float FatRenderedYieldKg = 0.5f;
        public const int GoldFillingTradeValue = 8;

        // ── Item ids ──────────────────────────────────────────────────
        public const string Item_FatRendered = "fat_rendered";
        public const string Item_Cloth = "cloth";
        public const string Item_GoldFilling = "gold_filling";
        public const string Item_CrematoriumAsh = "crematorium_ash";
        public const string Item_FamilyPhotograph = "family_photograph";
        public const string Item_DecontaminationSoap = "decontamination_soap_5_of_5";

        // ── Affliction ids ────────────────────────────────────────────
        public const string Affliction_CorpseThiefShame = "affliction_corpse_thief_shame";
        public const string Affliction_InfectionFromCorpse = "affliction_bacterial_infection";

        // ── Crematorium ───────────────────────────────────────────────
        public const string LocationId_Crematorium = "location_tessarat_crematorium";
        public const float CrematoriumAshFertilizerBonus = 0.25f; // +25% crop yield
        public const float CrematoriumAshMoraleHit = 15f;

        // ── Shame cure ────────────────────────────────────────────────
        public const float WashClothesHours = 1f;
        public const string ImmuneArchetype_Misanthrope = "the_misanthrope";
        public const string ImmuneArchetype_Psychopath = "the_psychopath";

        // ── Infection risk ────────────────────────────────────────────
        public const float CorpseInfectionChanceNoMask = 0.40f;
        public const float CorpseInfectionChanceWithMask = 0.05f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string, int> OnCorpseStripped;        // survivorId, clothYield
        public event Action<string> OnFatRendered;                // survivorId
        public event Action<string> OnGoldFillingExtracted;       // survivorId
        public event Action<string> OnShameApplied;               // survivorId
        public event Action<string> OnShameCured;                 // survivorId
        public event Action<string> OnCorpseInfection;            // survivorId
        public event Action<string> OnCrematoriumAshCollected;    // survivorId
        public event Action<string, float> OnCrematoriumAshUsed;  // survivorId, yieldBonus

        private readonly System.Random _rng;
        private int _corpsesProcessedTotal;
        private int _goldFillingsExtractedTotal;
        private int _clothingStrippedTotal;
        private int _crematoriumVisits;

        public int CorpsesProcessedTotal => _corpsesProcessedTotal;
        public int GoldFillingsExtractedTotal => _goldFillingsExtractedTotal;
        public int ClothingStrippedTotal => _clothingStrippedTotal;
        public int CrematoriumVisits => _crematoriumVisits;

        public ForensicScavengingSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(4000);
        }

        // ── Strip Corpse ──────────────────────────────────────────────

        /// <summary>
        /// Strip a corpse for usable materials. Returns cloth yield and fat.
        /// Applies CorpseThiefShame unless scavenger is immune.
        /// Risk of infection if corpse died of contagious disease.
        /// </summary>
        public StripResult StripCorpse(
            string scavengerId,
            string scavengerArchetypeId,
            bool hasGasMask,
            bool hasKnife,
            bool hasSurgeonOrDentist,
            string causeOfDeath = null)
        {
            var result = new StripResult();

            // Cloth yield (3-5 units)
            result.ClothYield = _rng.Next(ClothYieldMin, ClothYieldMax + 1);
            _clothingStrippedTotal += result.ClothYield;

            // Fat rendered (requires knife)
            if (hasKnife)
            {
                result.FatRenderedKg = FatRenderedYieldKg;
                result.FatRenderedUnits = 1;
            }

            // Gold fillings (requires surgeon or dentist)
            if (hasSurgeonOrDentist)
            {
                result.GoldFillingsExtracted = 1;
                result.GoldFillingTradeValue = GoldFillingTradeValue;
                _goldFillingsExtractedTotal++;
                OnGoldFillingExtracted?.Invoke(scavengerId);
            }

            // Infection risk from contagious corpse
            bool isContagious = !string.IsNullOrEmpty(causeOfDeath)
                && (causeOfDeath.Contains("spore_lung") || causeOfDeath.Contains("zoonotic"));
            if (isContagious)
            {
                float infectionChance = hasGasMask
                    ? CorpseInfectionChanceWithMask
                    : CorpseInfectionChanceNoMask;
                if (_rng.NextDouble() < infectionChance)
                {
                    result.GotInfected = true;
                    OnCorpseInfection?.Invoke(scavengerId);
                }
            }

            // CorpseThiefShame — unless immune archetype
            if (!IsImmuneToShame(scavengerArchetypeId))
            {
                result.GotShame = true;
                OnShameApplied?.Invoke(scavengerId);
            }

            _corpsesProcessedTotal++;
            OnCorpseStripped?.Invoke(scavengerId, result.ClothYield);
            return result;
        }

        // ── Shame Cure ────────────────────────────────────────────────

        /// <summary>
        /// Wash the clothes to cure CorpseThiefShame. Requires decontamination
        /// soap and boiling water.
        /// </summary>
        public bool WashClothes(string survivorId, bool hasSoap, bool hasBoilingWater)
        {
            if (!hasSoap || !hasBoilingWater) return false;
            OnShameCured?.Invoke(survivorId);
            return true;
        }

        /// <summary>Check if an archetype is immune to corpse shame.</summary>
        public static bool IsImmuneToShame(string archetypeId)
        {
            if (string.IsNullOrEmpty(archetypeId)) return false;
            return archetypeId == ImmuneArchetype_Misanthrope
                || archetypeId == ImmuneArchetype_Psychopath;
        }

        // ── Crematorium ───────────────────────────────────────────────

        /// <summary>
        /// Collect crematorium ash. High phosphorus — perfect fertilizer.
        /// But the ash is not wood ash. It is calcium and phosphorus from
        /// the dead.
        /// </summary>
        public int CollectCrematoriumAsh(string scavengerId, int visits)
        {
            _crematoriumVisits += visits;
            int ashYield = 3 * visits; // 3 bags per visit
            OnCrematoriumAshCollected?.Invoke(scavengerId);
            return ashYield;
        }

        /// <summary>
        /// Apply crematorium ash to hydroponics. +25% crop yield but morale hit.
        /// The the_priest will violently object. The the_chef will refuse to eat.
        /// The math says the tomatoes are the only source of Vitamin C.
        /// </summary>
        public float ApplyCrematoriumAsh(string survivorId)
        {
            OnCrematoriumAshUsed?.Invoke(survivorId, CrematoriumAshFertilizerBonus);
            return CrematoriumAshFertilizerBonus;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public ForensicScavengingSave CaptureState()
        {
            return new ForensicScavengingSave
            {
                CorpsesProcessedTotal = _corpsesProcessedTotal,
                GoldFillingsExtractedTotal = _goldFillingsExtractedTotal,
                ClothingStrippedTotal = _clothingStrippedTotal,
                CrematoriumVisits = _crematoriumVisits
            };
        }

        public void RestoreState(ForensicScavengingSave save)
        {
            _corpsesProcessedTotal = 0;
            _goldFillingsExtractedTotal = 0;
            _clothingStrippedTotal = 0;
            _crematoriumVisits = 0;
            if (save == null) return;
            _corpsesProcessedTotal = save.CorpsesProcessedTotal;
            _goldFillingsExtractedTotal = save.GoldFillingsExtractedTotal;
            _clothingStrippedTotal = save.ClothingStrippedTotal;
            _crematoriumVisits = save.CrematoriumVisits;
        }
    }

    [Serializable]
    public class StripResult
    {
        public int ClothYield;
        public float FatRenderedKg;
        public int FatRenderedUnits;
        public int GoldFillingsExtracted;
        public int GoldFillingTradeValue;
        public bool GotInfected;
        public bool GotShame;
    }

    [Serializable]
    public class ForensicScavengingSave
    {
        public int CorpsesProcessedTotal;
        public int GoldFillingsExtractedTotal;
        public int ClothingStrippedTotal;
        public int CrematoriumVisits;
    }
}
