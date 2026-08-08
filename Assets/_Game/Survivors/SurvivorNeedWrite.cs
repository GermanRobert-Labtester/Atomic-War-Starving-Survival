using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Static helpers for systems that cannot hold a <see cref="NeedsSystem"/>
    /// reference but must not leave Health at 0 while <see cref="SurvivorState"/>
    /// stays Alive (MISC-006). Prefer <see cref="NeedsSystem.AdjustHealth"/> when
    /// available — that path also fires OnNeedChanged / TryDeferDeath / OnDied.
    /// </summary>
    public static class SurvivorNeedWrite
    {
        public static void AdjustHealth(Survivor sv, float delta)
        {
            if (sv == null || sv.Needs == null || !sv.IsAlive || delta == 0f) return;
            float max = sv.MaxHealthCap > 0f ? sv.MaxHealthCap : 100f;
            SetHealth(sv, sv.Needs.Health + delta, max);
        }

        public static void SetHealth(Survivor sv, float health, float maxCap = -1f)
        {
            if (sv == null || sv.Needs == null) return;
            float max = maxCap > 0f ? maxCap : (sv.MaxHealthCap > 0f ? sv.MaxHealthCap : 100f);
            sv.Needs.Health = Mathf.Clamp(health, 0f, max);
            if (sv.Needs.Health <= 0f && sv.State != SurvivorState.Dead && sv.IsAlive)
                sv.State = SurvivorState.Dead;
        }
    }
}
