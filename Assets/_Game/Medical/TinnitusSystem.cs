using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Tinnitus System (#42) — survivors exposed to heavy artillery bombardment
    /// or room explosions suffer temporary hearing loss, making them immune to
    /// verbal panic from companions but unable to hear incoming raid warnings.
    ///
    /// Owns: Survivor.HasTinnitus, Survivor.TinnitusHoursRemaining,
    /// Survivor.IsDeafToWarnings.
    /// </summary>
    public class TinnitusSystem
    {
        public const float TinnitusDurationHours = 24f;
        public const float ExplosionTriggerThreshold = 0.5f; // severity threshold
        public const float RaidWarningDeafnessChance = 0.70f;
        public const float MoraleDrainPerHourWithTinnitus = 1f;

        public event Action<Survivor> OnTinnitusStarted;
        public event Action<Survivor> OnTinnitusEnded;
        public event Action<Survivor> OnRaidWarningMissed;

        public Action<Survivor, float> ApplyMoraleDelta;
        public System.Random Rng;

        public void OnExplosionEvent(float severity, IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (sv.HasTinnitus) continue;

                float chance = severity * ExplosionTriggerThreshold;
                if ((Rng?.NextDouble() ?? 0.5) < chance)
                {
                    sv.HasTinnitus = true;
                    sv.TinnitusHoursRemaining = TinnitusDurationHours;
                    sv.IsDeafToWarnings = true;
                    OnTinnitusStarted?.Invoke(sv);
                }
            }
        }

        public void Tick(Survivor sv, float gameHours, bool isRaidWarningActive)
        {
            if (sv == null || !sv.IsAlive) return;
            if (!sv.HasTinnitus) return;

            sv.TinnitusHoursRemaining -= gameHours;
            ApplyMoraleDelta?.Invoke(sv,
                -MoraleDrainPerHourWithTinnitus * gameHours);

            if (isRaidWarningActive && sv.IsDeafToWarnings)
            {
                if ((Rng?.NextDouble() ?? 0.5) < RaidWarningDeafnessChance)
                    OnRaidWarningMissed?.Invoke(sv);
            }

            if (sv.TinnitusHoursRemaining <= 0f)
            {
                sv.HasTinnitus = false;
                sv.IsDeafToWarnings = false;
                sv.TinnitusHoursRemaining = 0f;
                OnTinnitusEnded?.Invoke(sv);
            }
        }
    }
}
