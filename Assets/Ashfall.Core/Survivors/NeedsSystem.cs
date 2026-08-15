using System;

namespace Ashfall.Core.Survivors
{
    /// <summary>The seven tracked survival needs. Hunger/Thirst/Fatigue/Morale are
    /// 0..100 where HIGHER = WORSE (critical = starving); Warmth is 0..100 where
    /// LOWER = worse; Health 0..100 where lower = worse.</summary>
    public enum NeedKind
    {
        Hunger,
        Thirst,
        Fatigue,
        Warmth,
        Morale,
        Health,
        Hygiene
    }

    /// <summary>
    /// Engine-agnostic per-survivor need state, decoupled from any Unity/Godot
    /// survivor class. Hosts map this onto their own survivor objects.
    /// </summary>
    public class SurvivorNeedsState
    {
        public string Id = string.Empty;
        public float Hunger;
        public float Thirst;
        public float Fatigue;
        public float Warmth = 100f;
        public float Morale = 50f;
        public float Health = 100f;
        public float Hygiene = 100f;

        public bool WasHungerCritical;
        public bool WasThirstCritical;
        public bool WasWarmthCritical;

        public float MaxHealthCap = 100f;
        public bool IsAlive = true;
        public bool IsDead;

        /// <summary>Convenience mirror of IsDead for host mapping.</summary>
        public bool IsAliveState => !IsDead && IsAlive;
    }

    /// <summary>Decay/restore tuning values (port of Unity's NeedsProfile defaults).</summary>
    public class NeedsProfile
    {
        public float hungerPerHour = 0.8f;
        public float thirstPerHour = 1.2f;
        public float fatiguePerHour = 0.4f;
        public float warmthLossPerHourInCold = 0.5f;
        public float warmthRestorePerHourNearHeat = 3f;
        public float moraleLossPerHourWhileCritical = 1f;
        public float healthLossFromHunger = 0.4f;
        public float healthLossFromThirst = 0.6f;
        public float healthLossFromCold = 0.3f;
        public float hungerCritical = 90f;
        public float thirstCritical = 90f;
        public float warmthCritical = 20f;
    }

    /// <summary>
    /// Engine-agnostic port of Unity's NeedsSystem. Decays and restores survivor
    /// needs over game time, raises threshold/critical events, applies starvation /
    /// thirst / cold health consequences, and runs death evaluation at zero Health.
    /// Writes only via Modify/SetHealth so Health stays a single-writer value.
    /// </summary>
    public class NeedsSystem
    {
        private readonly NeedsProfile _profile;
        private readonly Func<SurvivorNeedsState, bool> _isNearHeatSource;
        private readonly System.Collections.Generic.List<SurvivorNeedsState> _survivors =
            new System.Collections.Generic.List<SurvivorNeedsState>();

        public event Action<SurvivorNeedsState, NeedKind, float> OnNeedChanged;
        public event Action<SurvivorNeedsState, NeedKind> OnNeedCritical;
        public event Action<SurvivorNeedsState> OnDied;

        /// <summary>Optional death-gate: return true to defer death at 0 Health.</summary>
        public Func<SurvivorNeedsState, bool> TryDeferDeath;

        public NeedsSystem(NeedsProfile profile = null, Func<SurvivorNeedsState, bool> isNearHeatSource = null)
        {
            _profile = profile ?? new NeedsProfile();
            _isNearHeatSource = isNearHeatSource;
        }

        public void Register(SurvivorNeedsState survivor)
        {
            if (survivor != null && !_survivors.Contains(survivor))
                _survivors.Add(survivor);
        }

        public void Unregister(SurvivorNeedsState survivor)
        {
            _survivors.Remove(survivor);
        }

        public void Tick(float gameHours)
        {
            for (int i = 0; i < _survivors.Count; i++)
                Tick(_survivors[i], gameHours);
        }

        public void Tick(SurvivorNeedsState survivor, float gameHours)
        {
            if (survivor == null || !survivor.IsAliveState || gameHours <= 0f) return;
            ApplyBaseNeedDrift(survivor, gameHours);
            ApplyCriticalNeedConsequences(survivor, gameHours);
        }

        private void ApplyBaseNeedDrift(SurvivorNeedsState survivor, float gameHours)
        {
            Modify(survivor, NeedKind.Hunger, _profile.hungerPerHour * gameHours);
            Modify(survivor, NeedKind.Thirst, _profile.thirstPerHour * gameHours);
            Modify(survivor, NeedKind.Fatigue, _profile.fatiguePerHour * gameHours);
            ApplyWarmth(survivor, gameHours);
        }

        private void ApplyCriticalNeedConsequences(SurvivorNeedsState survivor, float gameHours)
        {
            bool hungerCritical = survivor.Hunger >= _profile.hungerCritical;
            bool thirstCritical = survivor.Thirst >= _profile.thirstCritical;
            bool warmthCritical = survivor.Warmth <= _profile.warmthCritical;
            if (!hungerCritical && !thirstCritical && !warmthCritical) return;

            Modify(survivor, NeedKind.Morale, -MathfCompat.Max(0f, _profile.moraleLossPerHourWhileCritical) * gameHours);

            float healthLossPerHour = 0f;
            if (hungerCritical) healthLossPerHour += MathfCompat.Max(0f, _profile.healthLossFromHunger);
            if (thirstCritical) healthLossPerHour += MathfCompat.Max(0f, _profile.healthLossFromThirst);
            if (warmthCritical) healthLossPerHour += MathfCompat.Max(0f, _profile.healthLossFromCold);

            Modify(survivor, NeedKind.Health, -healthLossPerHour * gameHours);
        }

        private void ApplyWarmth(SurvivorNeedsState survivor, float gameHours)
        {
            bool warmed = _isNearHeatSource != null && _isNearHeatSource(survivor);
            float rate = warmed ? _profile.warmthRestorePerHourNearHeat : -_profile.warmthLossPerHourInCold;
            Modify(survivor, NeedKind.Warmth, rate * gameHours);
        }

        public void Modify(SurvivorNeedsState survivor, NeedKind need, float delta)
        {
            if (survivor == null || !survivor.IsAliveState || delta == 0f) return;
            ApplyNeedDelta(survivor, need, delta);
        }

        private void ApplyNeedDelta(SurvivorNeedsState survivor, NeedKind need, float delta)
        {
            float maxCap = need == NeedKind.Health ? survivor.MaxHealthCap : 100f;
            float newValue = MathfCompat.Clamp(GetValue(survivor, need) + delta, 0f, maxCap);
            SetValue(survivor, need, newValue);

            switch (need)
            {
                case NeedKind.Hunger:
                    NotifyCritical(survivor, need, newValue >= _profile.hungerCritical);
                    break;
                case NeedKind.Thirst:
                    NotifyCritical(survivor, need, newValue >= _profile.thirstCritical);
                    break;
                case NeedKind.Warmth:
                    NotifyCritical(survivor, need, newValue <= _profile.warmthCritical);
                    break;
                case NeedKind.Health:
                    EvaluateDeath(survivor);
                    break;
            }
        }

        private void NotifyCritical(SurvivorNeedsState survivor, NeedKind kind, bool isCritical)
        {
            bool wasCritical = kind switch
            {
                NeedKind.Hunger => survivor.WasHungerCritical,
                NeedKind.Thirst => survivor.WasThirstCritical,
                NeedKind.Warmth => survivor.WasWarmthCritical,
                _ => false
            };
            if (isCritical && !wasCritical)
                OnNeedCritical?.Invoke(survivor, kind);
            switch (kind)
            {
                case NeedKind.Hunger: survivor.WasHungerCritical = isCritical; break;
                case NeedKind.Thirst: survivor.WasThirstCritical = isCritical; break;
                case NeedKind.Warmth: survivor.WasWarmthCritical = isCritical; break;
            }
        }

        private void EvaluateDeath(SurvivorNeedsState survivor)
        {
            if (survivor.Health <= 0f && !survivor.IsDead)
            {
                if (TryDeferDeath != null && TryDeferDeath(survivor)) return;
                survivor.IsDead = true;
                survivor.IsAlive = false;
                OnDied?.Invoke(survivor);
            }
        }

        public void ForceDeath(SurvivorNeedsState survivor)
        {
            if (survivor == null || survivor.IsDead) return;
            survivor.Health = 0f;
            survivor.IsDead = true;
            survivor.IsAlive = false;
            OnDied?.Invoke(survivor);
        }

        public void SetHealth(SurvivorNeedsState survivor, float health)
        {
            if (survivor == null || !survivor.IsAliveState) return;
            survivor.Health = MathfCompat.Clamp(health, 0f, survivor.MaxHealthCap);
            OnNeedChanged?.Invoke(survivor, NeedKind.Health, survivor.Health);
            EvaluateDeath(survivor);
        }

        public void AdjustHealth(SurvivorNeedsState survivor, float delta)
        {
            if (survivor == null || !survivor.IsAliveState || delta == 0f) return;
            SetHealth(survivor, survivor.Health + delta);
        }

        public void NotifyNeedsRestored(SurvivorNeedsState survivor)
        {
            if (survivor == null || OnNeedChanged == null) return;
            OnNeedChanged.Invoke(survivor, NeedKind.Hunger, survivor.Hunger);
            OnNeedChanged.Invoke(survivor, NeedKind.Thirst, survivor.Thirst);
            OnNeedChanged.Invoke(survivor, NeedKind.Fatigue, survivor.Fatigue);
            OnNeedChanged.Invoke(survivor, NeedKind.Warmth, survivor.Warmth);
            OnNeedChanged.Invoke(survivor, NeedKind.Morale, survivor.Morale);
            OnNeedChanged.Invoke(survivor, NeedKind.Health, survivor.Health);
            OnNeedChanged.Invoke(survivor, NeedKind.Hygiene, survivor.Hygiene);
        }

        private static float GetValue(SurvivorNeedsState s, NeedKind kind) => kind switch
        {
            NeedKind.Hunger => s.Hunger,
            NeedKind.Thirst => s.Thirst,
            NeedKind.Fatigue => s.Fatigue,
            NeedKind.Warmth => s.Warmth,
            NeedKind.Morale => s.Morale,
            NeedKind.Health => s.Health,
            NeedKind.Hygiene => s.Hygiene,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };

        private void SetValue(SurvivorNeedsState s, NeedKind kind, float value)
        {
            switch (kind)
            {
                case NeedKind.Hunger: s.Hunger = value; break;
                case NeedKind.Thirst: s.Thirst = value; break;
                case NeedKind.Fatigue: s.Fatigue = value; break;
                case NeedKind.Warmth: s.Warmth = value; break;
                case NeedKind.Morale: s.Morale = value; break;
                case NeedKind.Health: s.Health = value; break;
                case NeedKind.Hygiene: s.Hygiene = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
            OnNeedChanged?.Invoke(s, kind, value);
        }
    }
}
