using System;
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
        /// <summary>
        /// DEATH-006 centralized kill event. Fired whenever <see cref="SetHealth"/>
        /// (or <see cref="Kill"/>) transitions a survivor to Health=0 / State=Dead.
        /// The bootstrap wires this to the same death chain that
        /// <see cref="NeedsSystem.OnDied"/> runs, so any system that cannot hold a
        /// <see cref="NeedsSystem"/> reference still produces world-reacting deaths.
        /// </summary>
        public static event Action<Survivor> OnKilled;

        public static void AdjustHealth(Survivor sv, float delta)
        {
            if (sv == null || sv.Needs == null || !sv.IsAlive || delta == 0f) return;
            float max = sv.MaxHealthCap > 0f ? sv.MaxHealthCap : 100f;
            SetHealth(sv, sv.Needs.Health + delta, max);
        }

        /// <summary>
        /// DEATH-001/004 hardened: SetHealth now (a) refuses to write a Health
        /// value onto a survivor that is NOT alive (no zombie state — dead stays
        /// dead unless the caller passes <paramref name="forceRevive"/>), and
        /// (b) when the write drops Health to 0, invokes <paramref name="onKilled"/>
        /// so the host can chain the same death hooks <see cref="NeedsSystem"/>
        /// runs (OnDied, NotifySurvivorDied, NotifyTwinDeath). The default
        /// <paramref name="onKilled"/> is a no-op so call sites that do not
        /// care (e.g. ambient damage ticks) keep working unchanged.
        /// </summary>
        /// <remarks>
        /// The adrenaline-revive path (MedicalPerkSystem.TryAdministerAdrenaline)
        /// intentionally writes Health on a dead survivor and then flips State
        /// to Idle; it must pass <c>forceRevive: true</c> AND must set State
        /// to Idle *after* calling this method. The default <c>forceRevive: false</c>
        /// is the right behaviour for every other call site.
        /// </remarks>
        public static void SetHealth(
            Survivor sv,
            float health,
            float maxCap = -1f,
            System.Action<Survivor> onKilled = null,
            bool forceRevive = false)
        {
            if (sv == null || sv.Needs == null) return;
            // DEATH-004: no zombie state. A dead survivor must not be given
            // positive HP. The only legitimate call that writes Health on a
            // dead survivor is the adrenaline-revive path, which sets
            // forceRevive: true and immediately flips State to Idle.
            if (!forceRevive && !sv.IsAlive) return;

            float max = maxCap > 0f ? maxCap : (sv.MaxHealthCap > 0f ? sv.MaxHealthCap : 100f);
            float newHealth = Mathf.Clamp(health, 0f, max);
            bool wasAlive = sv.Needs.Health > 0f;
            sv.Needs.Health = newHealth;

            // DEATH-001: when the write drops Health to 0 on a survivor that
            // was previously above zero, fire the death chain. Most callers do
            // not need to wire onKilled (a no-op default keeps the surface
            // compatible with the pre-fix behaviour); killers like
            // BunkerSocialDirector pass NeedsSystem.OnDied so all death
            // side-effects run.
            if (!forceRevive && wasAlive && newHealth <= 0f && sv.IsAlive)
            {
                sv.State = SurvivorState.Dead;
                onKilled?.Invoke(sv);
                OnKilled?.Invoke(sv);
            }
            else if (!forceRevive && newHealth <= 0f && sv.IsAlive)
            {
                // Health was already 0 (e.g. after a previous kill), but the
                // survivor was somehow still marked alive. Transition to Dead
                // and fire the chain exactly once.
                sv.State = SurvivorState.Dead;
                onKilled?.Invoke(sv);
                OnKilled?.Invoke(sv);
            }
        }

        /// <summary>
        /// DEATH-006 centralized kill helper: set Health to 0 and State to Dead,
        /// firing the same <see cref="OnKilled"/> chain that a natural death
        /// uses. Use this instead of writing <c>State = SurvivorState.Dead</c>
        /// directly so the world reacts to the death.
        /// </summary>
        public static void Kill(Survivor sv)
        {
            SetHealth(sv, 0f);
        }
    }
}
