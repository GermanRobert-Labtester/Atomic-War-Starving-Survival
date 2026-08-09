using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Phantom Intruders — Severe Paranoia (Prompt #6). When a survivor's
    /// RadiationAnxiety AND Fatigue both reach their maximum (1.0 and 100
    /// respectively), the system generates a fake "Hatch Breach!" alert.
    /// The paranoid survivor wakes up, fires their weapon at the door,
    /// consuming precious ammo and terrifying the bunker — only for the
    /// player to realize nothing is out there.
    ///
    /// Plain C# system. Tick monitors all survivors for the trigger condition.
    /// Side effects (ammo consumption, morale hit, fake UI alert) run through
    /// injected handlers — same pattern as MentalBreakSystem.
    /// </summary>
    public class PhantomIntruderSystem
    {
        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        /// <summary>RadiationAnxiety must be at or above this to trigger (0..1).</summary>
        public const float AnxietyTriggerThreshold = 1.0f;

        /// <summary>Fatigue must be at or above this to trigger (0..100).</summary>
        public const float FatigueTriggerThreshold = 100f;

        /// <summary>Cooldown in game-hours before the same survivor can trigger again.</summary>
        public const float CooldownHours = 72f;

        /// <summary>Ammo units consumed by the phantom "defense."</summary>
        public const int AmmoConsumed = 2;

        /// <summary>Morale hit applied bunker-wide when the phantom intruder is revealed.</summary>
        public const float BunkerMoraleHit = 10f;

        /// <summary>Morale hit applied to the paranoid survivor specifically.</summary>
        public const float ParanoidMoraleHit = 5f;

        /// <summary>Fired when a phantom intruder event triggers.
        /// Args: (paranoidSurvivor). UI listens to show the fake "Hatch Breach!" alert.</summary>
        public event Action<Survivor> OnPhantomIntruderTriggered;

        /// <summary>Fired after the realization. Args: (paranoidSurvivor).</summary>
        public event Action<Survivor> OnPhantomIntruderResolved;

        /// <summary>
        /// Host hook: consume ammo from inventory. Injected by GameBootstrap.
        /// Returns true if ammo was actually consumed (there was ammo to waste).
        /// </summary>
        public Func<int, bool> ConsumeAmmoHandler;

        /// <summary>
        /// Host hook: play gunshot audio / visual feedback. Injected by GameBootstrap.
        /// </summary>
        public Action OnWeaponFiredHandler;

        /// <summary>Per-survivor cooldown tracking (hours since last phantom event).
        /// Made public for SaveSystem serialization.</summary>
        public Dictionary<string, float> Cooldowns = new Dictionary<string, float>();

        // -----------------------------------------------------------------
        // Tick
        // -----------------------------------------------------------------

        /// <summary>
        /// Advance the system. Checks each alive survivor for the phantom
        /// intruder trigger condition (Anxiety >= 1.0 AND Fatigue >= 100),
        /// respects the per-survivor cooldown, and fires the event.
        /// </summary>
        // Reused buffer so Tick does not allocate a keys list every hour.
        private readonly List<string> _cooldownKeyBuffer = new List<string>(8);

        public void Tick(
            float gameHours,
            IReadOnlyList<Survivor> survivors,
            System.Random rng)
        {
            if (gameHours <= 0f || survivors == null) return;
            if (rng == null) rng = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("phantomintrudersystem");

            // Decay cooldowns without LINQ / per-tick Keys enumerator allocation.
            if (Cooldowns.Count > 0)
            {
                _cooldownKeyBuffer.Clear();
                foreach (var key in Cooldowns.Keys)
                    _cooldownKeyBuffer.Add(key);
                for (int k = 0; k < _cooldownKeyBuffer.Count; k++)
                {
                    string key = _cooldownKeyBuffer[k];
                    float next = Mathf.Max(0f, Cooldowns[key] - gameHours);
                    if (next <= 0f) Cooldowns.Remove(key);
                    else Cooldowns[key] = next;
                }
            }

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive || sv.Needs == null) continue;

                // Check cooldown
                if (Cooldowns.TryGetValue(sv.Id, out float cd) && cd > 0f) continue;

                // Trigger condition: RadiationAnxiety maxed AND Fatigue maxed
                if (sv.RadiationAnxiety >= AnxietyTriggerThreshold &&
                    sv.Needs.Fatigue >= FatigueTriggerThreshold)
                {
                    // Small RNG gate so it doesn't fire every tick while at max
                    if (rng.NextDouble() < 0.3f * gameHours) // ~30% per hour while at max
                    {
                        TriggerPhantomIntruder(sv, survivors);
                        Cooldowns[sv.Id] = CooldownHours;
                    }
                }
            }
        }

        private void TriggerPhantomIntruder(Survivor paranoid, IReadOnlyList<Survivor> allSurvivors)
        {
            // 1. Fire the fake alert
            OnPhantomIntruderTriggered?.Invoke(paranoid);

            // 2. Consume ammo via host hook
            bool hadAmmo = ConsumeAmmoHandler != null && ConsumeAmmoHandler(AmmoConsumed);

            // 3. Weapon fire audio
            if (hadAmmo)
            {
                OnWeaponFiredHandler?.Invoke();
            }

            // 4. Bunker-wide morale hit (the gunshot terrifies everyone)
            if (allSurvivors == null) return;
            for (int i = 0; i < allSurvivors.Count; i++)
            {
                var sv = allSurvivors[i];
                if (sv == null || !sv.IsAlive || sv.Needs == null) continue;

                float hit = sv == paranoid ? ParanoidMoraleHit : BunkerMoraleHit;
                if (_needsSystem != null)

                    _needsSystem.Modify(sv, NeedKind.Morale, -(hit));

                else

                    sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - hit, 0f, 100f);
            }

            // 5. Resolve — the realization that nothing was there
            OnPhantomIntruderResolved?.Invoke(paranoid);
        }
    }
}
