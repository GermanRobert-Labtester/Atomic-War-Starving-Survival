using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Survival / wasteland-digestion milestone perks (Prompts #189–#194).
    /// Earned through cooking, sickness recovery, crops, butchery, medical
    /// crafting, and planter duty — not XP grind. Plain C#, save/load safe.
    /// </summary>
    public class SurvivalPerkSystem
    {
        // ── Perk ids ─────────────────────────────────────────────────────
        public const string RationStretcherId = "perk_ration_stretcher";
        public const string IronStomachId = "perk_iron_stomach";
        public const string WastelandBrewerId = "perk_wasteland_brewer";
        public const string ButcherId = "perk_butcher";
        public const string PharmacologistId = "perk_pharmacologist";
        public const string MycologyId = "perk_mycology";

        // ── Thresholds ───────────────────────────────────────────────────
        public const int MealsForRationStretcher = 20;
        public const int IllnessRecoveriesForIronStomach = 2;
        public const int CropsForWastelandBrewer = 30;
        public const int ButcherActionsForButcher = 1; // first successful harvest/process
        public const int MedicalCraftsForPharmacologist = 10;
        public const float PlanterHoursForMycology = 50f;

        // ── Effect constants ─────────────────────────────────────────────
        public const float RationStretcherFreeWaterChance = 0.25f;
        public const float IronStomachIllnessMultiplier = 0.10f; // 90% reduced
        public const float ButcherProcessTimeMultiplier = 0.5f;
        public const int ButcherExtraBones = 1;
        public const int ButcherExtraMeat = 1;
        public const float HighYieldTreatmentSpeedMult = 0.5f; // twice as fast
        public const float MoonshineMoraleBoost = 35f;
        public const float MoonshineFatigueHit = 40f;
        public const string BloodstainedTag = "bloodstained";
        public const string HighYieldAntibioticId = "antibiotic_high_yield";
        public const string HighYieldIodineId = "iodine_high_yield";
        public const string MoonshineId = "moonshine";
        public const string MutatedFungiId = "mutated_fungi";
        public const string DirtyWaterItemId = "dirty_water";
        public const string CleanWaterItemId = "clean_water";
        public const string SpoiledMeatId = "spoiled_meat";
        public const string BonesId = "bones";
        public const string MeatId = "meat";

        private SkillProgressionSystem _progression;
        private readonly Dictionary<string, SurvivalCounters> _bySurvivor =
            new Dictionary<string, SurvivalCounters>();

        public event Action<Survivor, string> OnSurvivalPerkEarned;
        public event Action<Survivor, string, int> OnMilestoneProgress;
        public event Action<Survivor> OnBloodstainedTagApplied;

        public void Bind(SkillProgressionSystem progression)
        {
            _progression = progression;
            _progression?.RegisterSurvivalPerks();
        }

        public void RegisterCatalog() => _progression?.RegisterSurvivalPerks();

        // ── Queries ──────────────────────────────────────────────────────

        public bool Has(string survivorId, string perkId)
        {
            if (_progression == null || string.IsNullOrEmpty(survivorId)) return false;
            return _progression.HasActivePerk(survivorId, perkId);
        }

        public bool Has(Survivor sv, string perkId) =>
            sv != null && Has(sv.Id, perkId);

        public SurvivalCounters GetCounters(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return new SurvivalCounters();
            return GetOrCreate(survivorId).Clone();
        }

        // ── #189 Ration Stretcher ────────────────────────────────────────

        public void RecordMealCooked(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.MealsCooked++;
            OnMilestoneProgress?.Invoke(sv, "meals_cooked", c.MealsCooked);
            if (c.MealsCooked >= MealsForRationStretcher)
                TryGrant(sv, RationStretcherId, currentDay);
        }

        /// <summary>
        /// When cooking, 25% chance CleanWater cost is skipped if cook has perk.
        /// </summary>
        public bool RollSkipCleanWater(Survivor cook, System.Random rng = null)
        {
            if (!Has(cook, RationStretcherId)) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.Stream("survivalperksystem");
            return rng.NextDouble() < RationStretcherFreeWaterChance;
        }

        // ── #190 Iron Stomach ────────────────────────────────────────────

        /// <summary>
        /// Record recovery from FoodPoisoning/Botulism or Dysentery.
        /// Two recoveries grant Iron Stomach.
        /// </summary>
        public void RecordIllnessRecovery(Survivor sv, string afflictionId, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive || string.IsNullOrEmpty(afflictionId)) return;
            if (!IsGastricIllness(afflictionId)) return;

            var c = GetOrCreate(sv.Id);
            c.GastricIllnessRecoveries++;
            OnMilestoneProgress?.Invoke(sv, "gastric_recoveries", c.GastricIllnessRecoveries);
            if (c.GastricIllnessRecoveries >= IllnessRecoveriesForIronStomach)
                TryGrant(sv, IronStomachId, currentDay);
        }

        public static bool IsGastricIllness(string afflictionId)
        {
            if (string.IsNullOrEmpty(afflictionId)) return false;
            return string.Equals(afflictionId, "food_poisoning", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(afflictionId, "botulism", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(afflictionId, "dysentery", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Multiplier on Phase-1 illness chance from spoiled meat / dirty water.
        /// Iron Stomach → 0.10 (90% reduced).
        /// </summary>
        public float GetContaminatedIllnessChanceMultiplier(Survivor sv)
        {
            return Has(sv, IronStomachId) ? IronStomachIllnessMultiplier : 1f;
        }

        public float ScaleIllnessChance(Survivor sv, float baseChance)
        {
            return Mathf.Clamp01(baseChance * GetContaminatedIllnessChanceMultiplier(sv));
        }

        // ── #191 Wasteland Brewer ────────────────────────────────────────

        public void RecordCropHarvested(Survivor sv, int count = 1, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive || count <= 0) return;
            var c = GetOrCreate(sv.Id);
            c.CropsHarvested += count;
            OnMilestoneProgress?.Invoke(sv, "crops_harvested", c.CropsHarvested);
            if (c.CropsHarvested >= CropsForWastelandBrewer)
                TryGrant(sv, WastelandBrewerId, currentDay);
        }

        public bool CanCraftMoonshine(Survivor sv) => Has(sv, WastelandBrewerId);

        // ── #192 The Butcher ─────────────────────────────────────────────

        /// <summary>
        /// Record harvesting animals or processing human corpses.
        /// First successful action grants The Butcher.
        /// </summary>
        public void RecordButchery(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.ButcheryActions++;
            OnMilestoneProgress?.Invoke(sv, "butchery_actions", c.ButcheryActions);
            if (c.ButcheryActions >= ButcherActionsForButcher)
            {
                if (TryGrant(sv, ButcherId, currentDay))
                    ApplyBloodstainedTag(sv);
                else if (Has(sv, ButcherId))
                    ApplyBloodstainedTag(sv);
            }
        }

        public float GetCorpseProcessTimeMultiplier(Survivor sv) =>
            Has(sv, ButcherId) ? ButcherProcessTimeMultiplier : 1f;

        public int GetExtraBonesYield(Survivor sv) =>
            Has(sv, ButcherId) ? ButcherExtraBones : 0;

        public int GetExtraMeatYield(Survivor sv) =>
            Has(sv, ButcherId) ? ButcherExtraMeat : 0;

        public bool HasBloodstained(Survivor sv) =>
            sv != null && sv.HasAestheticTag(BloodstainedTag);

        private void ApplyBloodstainedTag(Survivor sv)
        {
            if (sv == null) return;
            if (sv.AddAestheticTag(BloodstainedTag))
                OnBloodstainedTagApplied?.Invoke(sv);
        }

        // ── #193 Pharmacologist ──────────────────────────────────────────

        public void RecordMedicalCraft(Survivor sv, int count = 1, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive || count <= 0) return;
            var c = GetOrCreate(sv.Id);
            c.MedicalCrafts += count;
            OnMilestoneProgress?.Invoke(sv, "medical_crafts", c.MedicalCrafts);
            if (c.MedicalCrafts >= MedicalCraftsForPharmacologist)
                TryGrant(sv, PharmacologistId, currentDay);
        }

        public bool CanProduceHighYieldMeds(Survivor sv) => Has(sv, PharmacologistId);

        /// <summary>
        /// If Pharmacologist crafts antibiotics/iodine from chemicals, return high-yield id.
        /// Otherwise return original result id.
        /// </summary>
        public string ResolveMedicalCraftResultId(Survivor sv, string baseResultId)
        {
            if (!CanProduceHighYieldMeds(sv) || string.IsNullOrEmpty(baseResultId))
                return baseResultId;
            if (IsAntibioticId(baseResultId)) return HighYieldAntibioticId;
            if (IsIodineId(baseResultId)) return HighYieldIodineId;
            return baseResultId;
        }

        public static bool IsAntibioticId(string id) =>
            !string.IsNullOrEmpty(id)
            && (string.Equals(id, "antibiotics", StringComparison.OrdinalIgnoreCase)
                || string.Equals(id, "antibiotic", StringComparison.OrdinalIgnoreCase)
                || string.Equals(id, HighYieldAntibioticId, StringComparison.OrdinalIgnoreCase));

        public static bool IsIodineId(string id) =>
            !string.IsNullOrEmpty(id)
            && (string.Equals(id, "iodine", StringComparison.OrdinalIgnoreCase)
                || string.Equals(id, "iodine_pills", StringComparison.OrdinalIgnoreCase)
                || string.Equals(id, HighYieldIodineId, StringComparison.OrdinalIgnoreCase));

        public static bool IsHighYieldMed(string id) =>
            string.Equals(id, HighYieldAntibioticId, StringComparison.OrdinalIgnoreCase)
            || string.Equals(id, HighYieldIodineId, StringComparison.OrdinalIgnoreCase);

        /// <summary>High-yield meds act twice as fast (half treatment hours).</summary>
        public static float GetTreatmentDurationMultiplier(string itemId) =>
            IsHighYieldMed(itemId) ? HighYieldTreatmentSpeedMult : 1f;

        /// <summary>High-yield meds completely ignore antibiotic resistance.</summary>
        public static bool IgnoresAntibioticResistance(string itemId) =>
            IsHighYieldMed(itemId);

        // ── #194 Mycology ────────────────────────────────────────────────

        public void RecordPlanterHours(Survivor sv, float hours, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive || hours <= 0f) return;
            var c = GetOrCreate(sv.Id);
            c.PlanterHours += hours;
            OnMilestoneProgress?.Invoke(sv, "planter_hours", Mathf.FloorToInt(c.PlanterHours));
            if (c.PlanterHours >= PlanterHoursForMycology)
                TryGrant(sv, MycologyId, currentDay);
        }

        public bool CanIdentifyToxicFungi(Survivor sv) => Has(sv, MycologyId);

        /// <summary>
        /// True when any living survivor with Mycology is present — Toxic Spore
        /// random event is fully prevented for the hydroponics bay.
        /// </summary>
        public bool PreventsToxicSporeEvent(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv != null && sv.IsAlive && Has(sv, MycologyId))
                    return true;
            }
            return false;
        }

        // ── Grant / storage ──────────────────────────────────────────────

        private bool TryGrant(Survivor sv, string perkId, int currentDay)
        {
            if (_progression == null || sv == null) return false;
            if (_progression.HasActivePerk(sv.Id, perkId)
                || _progression.HasDormantPerk(sv.Id, perkId))
                return false;

            bool granted = _progression.TryGrantPerk(sv, perkId, currentDay);
            if (granted)
                OnSurvivalPerkEarned?.Invoke(sv, perkId);
            return granted;
        }

        private SurvivalCounters GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var c))
            {
                c = new SurvivalCounters();
                _bySurvivor[survivorId] = c;
            }
            return c;
        }

        public SurvivalPerkSave CaptureState()
        {
            var save = new SurvivalPerkSave { Entries = new List<SurvivalCounterSave>() };
            foreach (var kv in _bySurvivor)
            {
                var c = kv.Value;
                save.Entries.Add(new SurvivalCounterSave
                {
                    SurvivorId = kv.Key,
                    MealsCooked = c.MealsCooked,
                    GastricIllnessRecoveries = c.GastricIllnessRecoveries,
                    CropsHarvested = c.CropsHarvested,
                    ButcheryActions = c.ButcheryActions,
                    MedicalCrafts = c.MedicalCrafts,
                    PlanterHours = c.PlanterHours
                });
            }
            return save;
        }

        public void RestoreState(SurvivalPerkSave save)
        {
            _bySurvivor.Clear();
            if (save?.Entries == null) return;
            for (int i = 0; i < save.Entries.Count; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                _bySurvivor[e.SurvivorId] = new SurvivalCounters
                {
                    MealsCooked = e.MealsCooked,
                    GastricIllnessRecoveries = e.GastricIllnessRecoveries,
                    CropsHarvested = e.CropsHarvested,
                    ButcheryActions = e.ButcheryActions,
                    MedicalCrafts = e.MedicalCrafts,
                    PlanterHours = e.PlanterHours
                };
            }
        }

        public sealed class SurvivalCounters
        {
            public int MealsCooked;
            public int GastricIllnessRecoveries;
            public int CropsHarvested;
            public int ButcheryActions;
            public int MedicalCrafts;
            public float PlanterHours;

            public SurvivalCounters Clone() => new SurvivalCounters
            {
                MealsCooked = MealsCooked,
                GastricIllnessRecoveries = GastricIllnessRecoveries,
                CropsHarvested = CropsHarvested,
                ButcheryActions = ButcheryActions,
                MedicalCrafts = MedicalCrafts,
                PlanterHours = PlanterHours
            };
        }
    }

    [Serializable]
    public class SurvivalPerkSave
    {
        public List<SurvivalCounterSave> Entries = new List<SurvivalCounterSave>();
    }

    [Serializable]
    public class SurvivalCounterSave
    {
        public string SurvivorId;
        public int MealsCooked;
        public int GastricIllnessRecoveries;
        public int CropsHarvested;
        public int ButcheryActions;
        public int MedicalCrafts;
        public float PlanterHours;
    }
}
