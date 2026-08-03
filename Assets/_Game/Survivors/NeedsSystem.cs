using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Pure-C# system that decays and restores survivor needs over game time and
    /// raises threshold events. Reads elapsed hours from whatever drives its Tick
    /// (e.g. TimeSystem); writes only via Modify, so Health always changes through
    /// a single, event-raising path regardless of the cause (starvation, cold,
    /// radiation via RadiationSystem, etc).
    ///
    /// Warmth restoration near a heat source is a hook: pass isNearHeatSource to
    /// the constructor once a shelter/heat-source system exists. Left null (the
    /// default), every survivor is treated as exposed to the cold.
    ///
    /// Bad/good event morale hook: event-handling code can call
    /// Modify(survivor, NeedKind.Morale, delta) directly.
    /// </summary>
    public class NeedsSystem
    {
        private readonly NeedsProfile _profile;
        private readonly Func<Survivor, bool> _isNearHeatSource;
        private readonly List<Survivor> _survivors = new List<Survivor>();

        // Optional photoperiod integration — null-safe so existing callers don't break.
        // Stored as thin delegates so Survivors.asmdef has no reference to Environment.
        private Func<float>  _getEffectiveDaylightHours; // returns PhotoperiodSystem.EffectiveDaylightHours
        private Func<bool>   _isGrowLightActive;
        private LightProfile _lightProfile;

        /// <summary>Fired whenever any single need value changes, with its new value.</summary>
        public event Action<Survivor, NeedKind, float> OnNeedChanged;
        /// <summary>Fired once when a need first crosses into its critical range.</summary>
        public event Action<Survivor, NeedKind> OnNeedCritical;
        /// <summary>Fired once when a survivor's health reaches zero.</summary>
        public event Action<Survivor> OnDied;

        public NeedsSystem(NeedsProfile profile, Func<Survivor, bool> isNearHeatSource = null)
        {
            _profile = profile != null ? profile : throw new ArgumentNullException(nameof(profile));
            _isNearHeatSource = isNearHeatSource;
        }

        /// <summary>
        /// Inject photoperiod light delegates after construction so existing
        /// callsites don't need to change signature.
        /// <paramref name="getEffectiveDaylightHours"/> is called each Tick and
        /// returns the current effective daylight hours from PhotoperiodSystem.
        /// <paramref name="isGrowLightActive"/> returns true when the shelter
        /// grow-light module is fuelled and running.
        /// </summary>
        public void SetPhotoPeriodSystem(
            Func<float>  getEffectiveDaylightHours,
            LightProfile lightProfile,
            Func<bool>   isGrowLightActive = null)
        {
            _getEffectiveDaylightHours = getEffectiveDaylightHours;
            _lightProfile              = lightProfile;
            _isGrowLightActive         = isGrowLightActive;
        }

        /// <summary>Register a survivor so bulk Tick(gameHours) advances their needs.</summary>
        public void Register(Survivor survivor)
        {
            if (survivor != null && !_survivors.Contains(survivor))
            {
                _survivors.Add(survivor);
            }
        }

        /// <summary>Stop advancing a survivor's needs via bulk Tick(gameHours).</summary>
        public void Unregister(Survivor survivor)
        {
            _survivors.Remove(survivor);
        }

        /// <summary>Advance need decay/recovery for all registered survivors over elapsed game hours.</summary>
        public void Tick(float gameHours)
        {
            for (int i = 0; i < _survivors.Count; i++)
            {
                Tick(_survivors[i], gameHours);
            }
        }

        /// <summary>Advance need decay/recovery for a single survivor over elapsed game hours.</summary>
        public void Tick(Survivor survivor, float gameHours)
        {
            if (survivor == null || !survivor.IsAlive || gameHours <= 0f)
            {
                return;
            }

            Modify(survivor, NeedKind.Hunger, _profile.hungerPerHour * gameHours);
            Modify(survivor, NeedKind.Thirst, _profile.thirstPerHour * gameHours);
            Modify(survivor, NeedKind.Fatigue, _profile.fatiguePerHour * gameHours);
            ApplyWarmth(survivor, gameHours);

            // Light / photoperiod tick — null-safe; skipped when not wired
            if (_getEffectiveDaylightHours != null && _lightProfile != null)
            {
                bool growLight = _isGrowLightActive != null && _isGrowLightActive();
                LightSystemHelper.TickSurvivorLight(
                    survivor,
                    gameHours,
                    _getEffectiveDaylightHours(),
                    growLight,
                    _lightProfile);
            }

            var needs = survivor.Needs;
            bool hungerCritical = needs.Hunger >= _profile.hungerCritical;
            bool thirstCritical = needs.Thirst >= _profile.thirstCritical;
            bool warmthCritical = needs.Warmth <= _profile.warmthCritical;

            // Health is medical-domain: active Afflictions (MedicalSystem) own health
            // drain/recovery. Critical hunger/thirst/cold still punish morale and can
            // be mirrored as afflictions by content systems, but no longer write Health
            // here — so the Health bar is not a free-floating second meter.
            if (hungerCritical || thirstCritical || warmthCritical)
            {
                Modify(survivor, NeedKind.Morale, -_profile.moraleLossPerHourWhileCritical * gameHours);
            }
        }

        /// <summary>Apply a clamped delta to a single need of a survivor.</summary>
        public void Modify(Survivor survivor, NeedKind need, float delta)
        {
            if (survivor == null || !survivor.IsAlive || delta == 0f)
            {
                return;
            }

            float maxCap = need == NeedKind.Health ? survivor.MaxHealthCap : 100f;
            float newValue = Mathf.Clamp(GetValue(survivor.Needs, need) + delta, 0f, maxCap);
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

        private void ApplyWarmth(Survivor survivor, float gameHours)
        {
            bool warmed = _isNearHeatSource != null && _isNearHeatSource(survivor);
            float rate = warmed ? _profile.warmthRestorePerHourNearHeat : -_profile.warmthLossPerHourInCold;
            Modify(survivor, NeedKind.Warmth, rate * gameHours);
        }

        private void NotifyCritical(Survivor survivor, NeedKind kind, bool isCritical)
        {
            bool wasCritical = GetWasCritical(survivor.Needs, kind);
            if (isCritical && !wasCritical)
            {
                OnNeedCritical?.Invoke(survivor, kind);
            }
            SetWasCritical(survivor.Needs, kind, isCritical);
        }

        private void EvaluateDeath(Survivor survivor)
        {
            if (survivor.Needs.Health <= 0f && survivor.State != SurvivorState.Dead)
            {
                survivor.State = SurvivorState.Dead;
                OnDied?.Invoke(survivor);
            }
        }

        private static float GetValue(Needs needs, NeedKind kind)
        {
            switch (kind)
            {
                case NeedKind.Hunger: return needs.Hunger;
                case NeedKind.Thirst: return needs.Thirst;
                case NeedKind.Fatigue: return needs.Fatigue;
                case NeedKind.Warmth: return needs.Warmth;
                case NeedKind.Morale: return needs.Morale;
                case NeedKind.Health: return needs.Health;
                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private void SetValue(Survivor survivor, NeedKind kind, float value)
        {
            var needs = survivor.Needs;
            switch (kind)
            {
                case NeedKind.Hunger: needs.Hunger = value; break;
                case NeedKind.Thirst: needs.Thirst = value; break;
                case NeedKind.Fatigue: needs.Fatigue = value; break;
                case NeedKind.Warmth: needs.Warmth = value; break;
                case NeedKind.Morale: needs.Morale = value; break;
                case NeedKind.Health: needs.Health = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
            OnNeedChanged?.Invoke(survivor, kind, value);
        }

        private static bool GetWasCritical(Needs needs, NeedKind kind)
        {
            switch (kind)
            {
                case NeedKind.Hunger: return needs.WasHungerCritical;
                case NeedKind.Thirst: return needs.WasThirstCritical;
                case NeedKind.Warmth: return needs.WasWarmthCritical;
                default: return false;
            }
        }

        private static void SetWasCritical(Needs needs, NeedKind kind, bool value)
        {
            switch (kind)
            {
                case NeedKind.Hunger: needs.WasHungerCritical = value; break;
                case NeedKind.Thirst: needs.WasThirstCritical = value; break;
                case NeedKind.Warmth: needs.WasWarmthCritical = value; break;
            }
        }
    }
}
