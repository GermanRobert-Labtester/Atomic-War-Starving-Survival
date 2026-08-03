using System;
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
    /// </summary>
    [Serializable]
    public class PlanterBox
    {
        public const float MinimumViableTemperatureC = 10f;
        public const float MaxUnpoweredHoursBeforeStall = 24f;

        public CropSO ActiveCrop { get; private set; }
        public CropLifecycleStage Stage { get; private set; } = CropLifecycleStage.Dead;
        public float GrowthHours { get; private set; }
        public float UnpoweredHours { get; private set; }
        public bool IsStalled { get; private set; }
        public bool HasWater { get; private set; }

        public event Action<CropLifecycleStage> OnStageChanged;
        public event Action OnCropStalled;
        public event Action OnCropDied;

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
        public bool Water(bool isCleanWater, ShelterRoom room = null)
        {
            if (ActiveCrop == null || Stage == CropLifecycleStage.Dead) return false;

            if (!isCleanWater)
            {
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
        /// Harvest a mature crop. Returns true and yields calories and contamination if mature.
        /// </summary>
        public bool Harvest(out float calories, out float contamination)
        {
            if (Stage == CropLifecycleStage.Mature && ActiveCrop != null)
            {
                calories = ActiveCrop.CalorieYield;
                contamination = ActiveCrop.ContaminationYield;

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
