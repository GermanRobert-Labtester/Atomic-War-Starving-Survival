using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Nerve Damage System (#46) — radiation exposure or untreated wounds
    /// cause involuntary muscle spasms, lowering weapon accuracy and
    /// medical crafting speed unless treated with nerve stabilization drugs.
    ///
    /// Owns: Survivor.HasNerveDamage, Survivor.NerveStabilizerHours,
    /// Survivor.WeaponAccuracyModifier.
    /// </summary>
    public class NerveDamageSystem
    {
        public const float NerveDamageRadThreshold = 500f;
        public const float UntreatedWoundDaysThreshold = 5f;
        public const float NerveStabilizerDurationHours = 12f;
        public const float BaseAccuracyPenalty = 0.30f;
        public const float CraftingSpeedPenalty = 0.25f;
        public const float StabilizedAccuracyPenalty = 0.05f;
        public const float StabilizedCraftingPenalty = 0.05f;

        public event Action<Survivor> OnNerveDamageDeveloped;
        public event Action<Survivor> OnNerveStabilizerApplied;
        public event Action<Survivor> OnNerveStabilizerWore;

        public Func<Survivor, float> GetDaysSinceLastWoundTreatment;
        public Action<Survivor, float> ApplyCraftingSpeedPenalty;
        public System.Random Rng;

        public void CheckForNerveDamage(Survivor sv)
        {
            if (sv == null || !sv.IsAlive || sv.HasNerveDamage) return;

            bool fromRadiation = sv.LifetimeRadiationExposure >= NerveDamageRadThreshold;
            float untreatedDays = GetDaysSinceLastWoundTreatment?.Invoke(sv) ?? 0f;
            bool fromWounds = untreatedDays >= UntreatedWoundDaysThreshold;

            if (fromRadiation || fromWounds)
            {
                sv.HasNerveDamage = true;
                sv.WeaponAccuracyModifier = BaseAccuracyPenalty;
                ApplyCraftingSpeedPenalty?.Invoke(sv, CraftingSpeedPenalty);
                OnNerveDamageDeveloped?.Invoke(sv);
            }
        }

        public bool ApplyNerveStabilizer(Survivor sv)
        {
            if (sv == null || !sv.HasNerveDamage) return false;

            sv.NerveStabilizerHours = NerveStabilizerDurationHours;
            sv.WeaponAccuracyModifier = StabilizedAccuracyPenalty;
            ApplyCraftingSpeedPenalty?.Invoke(sv, StabilizedCraftingPenalty);
            OnNerveStabilizerApplied?.Invoke(sv);
            return true;
        }

        public void Tick(Survivor sv, float gameHours)
        {
            if (sv == null || !sv.IsAlive || !sv.HasNerveDamage) return;

            CheckForNerveDamage(sv); // also checks for new damage

            if (sv.NerveStabilizerHours > 0f)
            {
                sv.NerveStabilizerHours -= gameHours;
                if (sv.NerveStabilizerHours <= 0f)
                {
                    sv.NerveStabilizerHours = 0f;
                    sv.WeaponAccuracyModifier = BaseAccuracyPenalty;
                    ApplyCraftingSpeedPenalty?.Invoke(sv, CraftingSpeedPenalty);
                    OnNerveStabilizerWore?.Invoke(sv);
                }
            }
        }
    }
}
