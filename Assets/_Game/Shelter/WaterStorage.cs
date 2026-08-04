using System;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// The bunker's water cistern: three tiers of stored volume (clean, dirty,
    /// irradiated) fed by <see cref="Modules.CatchmentSurfaceModuleSO"/> and
    /// refined by <see cref="Modules.WaterPurifierModuleSO"/>. Save/load safe:
    /// primitives only.
    /// </summary>
    [Serializable]
    public class WaterStorage
    {
        public float CleanWater;
        public float DirtyWater;
        public float IrradiatedWater;

        public void AddClean(float amount)
        {
            if (amount > 0f) CleanWater += amount;
        }

        public void AddDirty(float amount)
        {
            if (amount > 0f) DirtyWater += amount;
        }

        public void AddIrradiated(float amount)
        {
            if (amount > 0f) IrradiatedWater += amount;
        }

        /// <summary>
        /// Black Rain / catchment sabotage: move all clean and dirty volume into
        /// the irradiated tier. Idempotent when already empty of clean/dirty.
        /// </summary>
        public void RuinCleanAndDirtyToIrradiated()
        {
            float ruined = 0f;
            if (CleanWater > 0f)
            {
                ruined += CleanWater;
                CleanWater = 0f;
            }
            if (DirtyWater > 0f)
            {
                ruined += DirtyWater;
                DirtyWater = 0f;
            }
            if (ruined > 0f) IrradiatedWater += ruined;
        }

        /// <summary>Draw up to <paramref name="amount"/> from the clean pool; returns the actual amount taken.</summary>
        public float ConsumeClean(float amount) => Consume(ref CleanWater, amount);

        /// <summary>Draw up to <paramref name="amount"/> from the dirty pool; returns the actual amount taken.</summary>
        public float ConsumeDirty(float amount) => Consume(ref DirtyWater, amount);

        /// <summary>Draw up to <paramref name="amount"/> from the irradiated pool; returns the actual amount taken.</summary>
        public float ConsumeIrradiated(float amount) => Consume(ref IrradiatedWater, amount);

        private static float Consume(ref float pool, float amount)
        {
            if (amount <= 0f || pool <= 0f) return 0f;
            float taken = Mathf.Min(pool, amount);
            pool -= taken;
            return taken;
        }

        public WaterStorageSave CaptureState()
        {
            return new WaterStorageSave
            {
                CleanWater = CleanWater,
                DirtyWater = DirtyWater,
                IrradiatedWater = IrradiatedWater
            };
        }

        public void RestoreState(WaterStorageSave save)
        {
            if (save == null) return;
            CleanWater = save.CleanWater;
            DirtyWater = save.DirtyWater;
            IrradiatedWater = save.IrradiatedWater;
        }
    }

    [Serializable]
    public class WaterStorageSave
    {
        public float CleanWater;
        public float DirtyWater;
        public float IrradiatedWater;
    }
}
