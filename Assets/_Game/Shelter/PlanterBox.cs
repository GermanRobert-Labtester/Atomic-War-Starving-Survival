using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// PlanterBox module (Prompt #37): Holds seeds, requires water and grow-light power.
    /// Multi-stage lifecycle: Seed -> Sprout -> Mature -> Dead.
    /// Dependencies:
    ///   • Temperature < 10°C -> Crop dies.
    ///   • Unpowered GrowLights > 24 hours -> Crop stalls.
    ///   • Irrigating with dirty water -> Introduces mold to room and ruins crop.
    /// Prompt #194 — Mycology: planter duty hours + Toxic Spore prevention.
    /// </summary>
    [Serializable]
    public class PlanterBox
    {
        public const float MinimumViableTemperatureC = 10f;
        public const float MaxUnpoweredHoursBeforeStall = 24f;
        /// <summary>Base chance per day that Toxic Spore ruins the bay (no Mycology).</summary>
        public const float ToxicSporeBaseChance = 0.08f;

        public CropSO ActiveCrop { get; private set; }
        public CropLifecycleStage Stage { get; private set; } = CropLifecycleStage.Dead;
        public float GrowthHours { get; private set; }
        public float UnpoweredHours { get; private set; }
        public bool IsStalled { get; private set; }
        public bool HasWater { get; private set; }
        /// <summary>Accumulated hours survivors have been assigned to this box (Prompt #194).</summary>
        public float AssignedDutyHours { get; private set; }

        public event Action<CropLifecycleStage> OnStageChanged;
        public event Action OnCropStalled;
        public event Action OnCropDied;
        public event Action OnToxicSporeEvent;
        public event Action<Survivor, float> OnDutyHoursRecorded;

        /// <summary>Plant a seed into this planter box.</summary>
        public bool PlantSeed(CropSO crop)
        {
            if (crop == null) return false;
            ActiveCrop = crop;
            GrowthHours = 0f;
            UnpoweredHours = 0f;
            IsStalled = false;
            HasWater = true; // Initial soil moisture
            SetStage(CropLifecycleStage.Seed);
            return true;
        }

        /// <summary>
        /// Irrigate the planter box.
        /// Using clean water maintains growth.
        /// Using dirty water introduces room mold (#20) and ruins the crop (changes to Dead).
        /// </summary>
        public bool Water(
            bool isCleanWater,
            ShelterRoom room = null,
            PersonalQuestSystem personalQuests = null,
            System.Collections.Generic.IReadOnlyList<Survivor> survivors = null)
        {
            if (ActiveCrop == null || Stage == CropLifecycleStage.Dead) return false;

            if (!isCleanWater)
            {
                // Prompt #230 — Gaia: crops never catch mold from dirty water.
                if (personalQuests != null && personalQuests.CropsImmuneToMold(survivors))
                {
                    HasWater = true;
                    return true;
                }
                // Dirty water ruins crop and causes room mold infestation
                if (room != null)
                {
                    room.HasMold = true;
                    room.MoldLevel = Mathf.Clamp01(room.MoldLevel + 0.5f);
                    room.AmbientContamination = Mathf.Clamp01(room.AmbientContamination + 0.3f);
                }
                KillCrop();
                return false;
            }

            HasWater = true;
            return true;
        }

        /// <summary>
        /// Advance growth over time.
        /// Checks temperature, grow-light power duration, and moisture.
        /// </summary>
        public void Tick(float gameHours, float roomTemperature, bool isLightPowered, ShelterRoom room = null)
        {
            if (ActiveCrop == null || Stage == CropLifecycleStage.Dead || gameHours <= 0f) return;

            // 1. Cold kill check (< 10°C)
            if (roomTemperature < MinimumViableTemperatureC)
            {
                KillCrop();
                return;
            }

            // 2. Power loss check (> 24 hours unpowered)
            if (!isLightPowered)
            {
                UnpoweredHours += gameHours;
                if (UnpoweredHours > MaxUnpoweredHoursBeforeStall)
                {
                    if (!IsStalled)
                    {
                        IsStalled = true;
                        OnCropStalled?.Invoke();
                    }
                }
            }
            else
            {
                UnpoweredHours = 0f;
                IsStalled = false;
            }

            // 3. Growth progression (only when not stalled and has water)
            if (!IsStalled && HasWater)
            {
                GrowthHours += gameHours;
                UpdateLifecycleStage();
            }
        }

        /// <summary>
        /// Record time a survivor spent assigned to this PlanterBox (Prompt #194 Mycology).
        /// </summary>
        public void RecordDutyHours(
            Survivor worker,
            float hours,
            SurvivalPerkSystem survivalPerks = null,
            int currentDay = 0)
        {
            if (worker == null || !worker.IsAlive || hours <= 0f) return;
            AssignedDutyHours += hours;
            survivalPerks?.RecordPlanterHours(worker, hours, currentDay);
            OnDutyHoursRecorded?.Invoke(worker, hours);
        }

        /// <summary>
        /// Prompt #194 — Toxic Spore random event. Returns true if the bay was ruined.
        /// Fully prevented when any living survivor has Mycology.
        /// </summary>
        public bool TryToxicSporeEvent(
            IReadOnlyList<Survivor> survivors,
            SurvivalPerkSystem survivalPerks,
            System.Random rng = null,
            float chance = ToxicSporeBaseChance,
            PersonalQuestSystem personalQuests = null)
        {
            if (ActiveCrop == null || Stage == CropLifecycleStage.Dead) return false;
            if (survivalPerks != null && survivalPerks.PreventsToxicSporeEvent(survivors))
                return false;
            // Prompt #230 — Gaia: crops never catch mold/spores.
            if (personalQuests != null && personalQuests.CropsImmuneToMold(survivors))
                return false;

            rng ??= AtomicWar._Game.Utilities.SeededRandom.Stream("planterbox");
            if (rng.NextDouble() >= chance) return false;

            KillCrop();
            OnToxicSporeEvent?.Invoke();
            return true;
        }

        /// <summary>
        /// Mycology holders can identify toxic mutated-fungi strains before harvest.
        /// Returns true when the active crop is mutated fungi and would be toxic.
        /// </summary>
        public bool IsActiveCropVisiblyToxic(Survivor observer, SurvivalPerkSystem survivalPerks)
        {
            if (ActiveCrop == null || survivalPerks == null || observer == null) return false;
            if (!survivalPerks.CanIdentifyToxicFungi(observer)) return false;
            if (!string.Equals(ActiveCrop.CropId, SurvivalPerkSystem.MutatedFungiId,
                    StringComparison.OrdinalIgnoreCase))
                return false;
            return ActiveCrop.IsToxicStrain;
        }

        /// <summary>
        /// Harvest a mature crop. Returns true and yields calories and contamination if mature.
        /// When harvester is provided, counts toward Wasteland Brewer crop milestone (#191).
        /// </summary>
        public bool Harvest(
            out float calories,
            out float contamination,
            Survivor harvester = null,
            SurvivalPerkSystem survivalPerks = null,
            int currentDay = 0,
            PersonalQuestSystem personalQuests = null)
        {
            if (Stage == CropLifecycleStage.Mature && ActiveCrop != null)
            {
                calories = ActiveCrop.CalorieYield;
                contamination = ActiveCrop.ContaminationYield;
                // Prompt #230 — Gaia: 3x food yield.
                if (personalQuests != null && harvester != null)
                {
                    int mult = personalQuests.GetCropYieldMultiplier(harvester);
                    if (mult > 1) calories *= mult;
                }

                // Prompt #194 — Mycology: refuse harvest of visibly toxic fungi
                if (harvester != null && survivalPerks != null
                    && IsActiveCropVisiblyToxic(harvester, survivalPerks))
                {
                    // Identified as toxic — discard without poisoning the bay inventory
                    calories = 0f;
                    contamination = 0f;
                    ActiveCrop = null;
                    SetStage(CropLifecycleStage.Dead);
                    GrowthHours = 0f;
                    IsStalled = false;
                    HasWater = false;
                    return true; // action succeeded (safe discard)
                }

                if (harvester != null && survivalPerks != null)
                    survivalPerks.RecordCropHarvested(harvester, 1, currentDay);

                // Reset box after harvest
                ActiveCrop = null;
                SetStage(CropLifecycleStage.Dead);
                GrowthHours = 0f;
                IsStalled = false;
                HasWater = false;
                return true;
            }

            calories = 0f;
            contamination = 0f;
            return false;
        }

        private void KillCrop()
        {
            if (Stage == CropLifecycleStage.Dead) return;
            SetStage(CropLifecycleStage.Dead);
            IsStalled = false;
            OnCropDied?.Invoke();
        }

        private void UpdateLifecycleStage()
        {
            if (ActiveCrop == null || Stage == CropLifecycleStage.Dead) return;

            float progress = GrowthHours / Mathf.Max(0.01f, ActiveCrop.GrowthHoursRequired);
            CropLifecycleStage nextStage;

            if (progress >= 1.0f)
            {
                nextStage = CropLifecycleStage.Mature;
            }
            else if (progress >= 0.33f)
            {
                nextStage = CropLifecycleStage.Sprout;
            }
            else
            {
                nextStage = CropLifecycleStage.Seed;
            }

            SetStage(nextStage);
        }

        private void SetStage(CropLifecycleStage nextStage)
        {
            if (Stage == nextStage) return;
            Stage = nextStage;
            OnStageChanged?.Invoke(Stage);
        }
    }
}
