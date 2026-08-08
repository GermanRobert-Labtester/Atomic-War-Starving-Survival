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
    public partial class NeedsSystem
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

        /// <summary>
        /// Optional death gate (Prompt #205 Death's Door). Return true to defer death
        /// while Health is at 0 (e.g. Paramedic colony). Null = always die at 0 HP.
        /// </summary>
        public Func<Survivor, bool> TryDeferDeath;

        /// <summary>
        /// Prompt #209 Night Terror — when true, skip Listless / darkness morale drain
        /// for this survivor. Null = always apply darkness morale penalties.
        /// </summary>
        public Func<Survivor, bool> IgnoresDarknessMorale;

        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        /// <summary>
        /// #267 Relapsing Addict: host supplies inventory drain for forced chem.
        /// Return true only when stock was actually consumed.
        /// </summary>
        public Func<Survivor, bool> ForcedChemConsumeHandler;

        /// <summary>
        /// Prompts #249–#266 — Selfless morale absorb, Traumatized morale cap,
        /// Pillar-of-Atlas death, Living Saint floor, Hyper-Empath drift.
        /// </summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

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

            ApplyBaseNeedDrift(survivor, gameHours);
            TickLightExposure(survivor, gameHours);
            ApplyCriticalNeedConsequences(survivor, gameHours);

            if (survivor.IsAlive)
            {
                ApplyPersonalQuestEffects(survivor, gameHours);
            }
        }

        private void ApplyBaseNeedDrift(Survivor survivor, float gameHours)
        {
            // #275 Zen State: 80% reduced hunger/thirst/fatigue decay.
            float needsMultiplier = _personalQuests != null
                ? _personalQuests.GetZenNeedsDecayMultiplier(survivor)
                : 1f;

            // #316 Synth: androids don't need food/water; Overclocked doesn't need sleep.
            if (_personalQuests == null || _personalQuests.NeedsFoodOrWater(survivor))
            {
                Modify(survivor, NeedKind.Hunger, _profile.hungerPerHour * gameHours * needsMultiplier);
                Modify(survivor, NeedKind.Thirst, _profile.thirstPerHour * gameHours * needsMultiplier);
            }

            if (_personalQuests == null || _personalQuests.NeedsSleep(survivor))
            {
                Modify(survivor, NeedKind.Fatigue, _profile.fatiguePerHour * gameHours * needsMultiplier);
            }

            ApplyWarmth(survivor, gameHours);
        }

        private void TickLightExposure(Survivor survivor, float gameHours)
        {
            if (_getEffectiveDaylightHours == null || _lightProfile == null) return;

            bool growLight = _isGrowLightActive != null && _isGrowLightActive();
            bool ignoreDarkness = IgnoresDarknessMorale != null && IgnoresDarknessMorale(survivor);
            LightSystemHelper.TickSurvivorLight(
                survivor,
                gameHours,
                _getEffectiveDaylightHours(),
                growLight,
                _lightProfile,
                ignoreDarknessMorale: ignoreDarkness);
        }

        private void ApplyCriticalNeedConsequences(Survivor survivor, float gameHours)
        {
            Needs needs = survivor.Needs;
            bool hungerCritical = needs.Hunger >= _profile.hungerCritical;
            bool thirstCritical = needs.Thirst >= _profile.thirstCritical;
            bool warmthCritical = needs.Warmth <= _profile.warmthCritical;
            if (!hungerCritical && !thirstCritical && !warmthCritical) return;

            Modify(
                survivor,
                NeedKind.Morale,
                -Mathf.Max(0f, _profile.moraleLossPerHourWhileCritical) * gameHours);

            // Profile fields are loss-per-hour while critical. Apply each active
            // condition once; in particular, cold must hurt at the exact threshold
            // instead of waiting until Warmth falls below it.
            float healthLossPerHour = 0f;
            if (hungerCritical)
                healthLossPerHour += Mathf.Max(0f, _profile.healthLossFromHunger);
            if (thirstCritical)
                healthLossPerHour += Mathf.Max(0f, _profile.healthLossFromThirst);
            if (warmthCritical)
                healthLossPerHour += Mathf.Max(0f, _profile.healthLossFromCold);

            Modify(survivor, NeedKind.Health, -healthLossPerHour * gameHours);
        }

        /// <summary>Apply a clamped delta to a single need of a survivor.</summary>
        public void Modify(Survivor survivor, NeedKind need, float delta)
        {
            if (survivor == null || !survivor.IsAlive || delta == 0f)
            {
                return;
            }

            // #249 Selfless: redistribute morale damage onto Selfless absorbers.
            if (need == NeedKind.Morale && delta < 0f && _personalQuests != null)
            {
                var all = _getSurvivors != null ? _getSurvivors() : _survivors;
                _personalQuests.ApplyMoraleDamageWithSelfless(survivor, -delta, all);
                float after = survivor.Needs.Morale;
                _personalQuests.ClampMoraleToCap(survivor);
                // #265 Living Saint Inspired: floor applies after any morale damage path.
                _personalQuests.ApplyLivingSaintMoraleFloor(survivor);
                OnNeedChanged?.Invoke(survivor, NeedKind.Morale, survivor.Needs.Morale);
                // Still fire critical if applicable (morale has no critical threshold here).
                _ = after;
                return;
            }

            float maxCap = need == NeedKind.Health ? survivor.MaxHealthCap : 100f;
            // #252 Traumatized: permanent 50% max Morale cap.
            if (need == NeedKind.Morale && _personalQuests != null)
                maxCap = Mathf.Min(maxCap, _personalQuests.GetMaxMoraleCap(survivor));
            // #268 Restless: permanent max Fatigue cap at 80%.
            if (need == NeedKind.Fatigue && _personalQuests != null)
                maxCap = Mathf.Min(maxCap, _personalQuests.GetMaxFatigueCap(survivor));
            // #291 Frail: permanent max Health cap at 60.
            if (need == NeedKind.Health && _personalQuests != null)
                maxCap = Mathf.Min(maxCap, _personalQuests.GetMaxHealthCapForQuests(survivor));
            // #249 Matriarch: room-mates gain +20 effective health cap.
            if (need == NeedKind.Health && _personalQuests != null)
            {
                var all = _getSurvivors != null ? _getSurvivors() : _survivors;
                maxCap += _personalQuests.GetMatriarchRoomHealthBonus(survivor, all);
            }

            float newValue = Mathf.Clamp(GetValue(survivor.Needs, need) + delta, 0f, maxCap);
            SetValue(survivor, need, newValue);

            if (need == NeedKind.Morale && _personalQuests != null)
            {
                _personalQuests.ClampMoraleToCap(survivor);
                // #265 Living Saint Inspired: Morale never drops below 50 bunker-wide.
                _personalQuests.ApplyLivingSaintMoraleFloor(survivor);
            }

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
                if (TryDeferDeath != null && TryDeferDeath(survivor))
                    return;
                survivor.State = SurvivorState.Dead;
                // #250 Pillar of Atlas death → permanent shelter repair debuff.
                _personalQuests?.NotifySurvivorDied(survivor);
                // #304/#305 Twin: partner suffers a symbiotic-bond death reaction.
                _personalQuests?.NotifyTwinDeath(
                    survivor, _getSurvivors != null ? _getSurvivors() : _survivors);
                OnDied?.Invoke(survivor);
            }
        }

        /// <summary>
        /// Force true death (bypasses TryDeferDeath). Used when Death's Door expires.
        /// </summary>
        public void ForceDeath(Survivor survivor)
        {
            if (survivor == null || survivor.State == SurvivorState.Dead) return;
            survivor.Needs.Health = 0f;
            survivor.State = SurvivorState.Dead;
            _personalQuests?.NotifySurvivorDied(survivor);
            _personalQuests?.NotifyTwinDeath(
                survivor, _getSurvivors != null ? _getSurvivors() : _survivors);
            OnDied?.Invoke(survivor);
        }

        /// <summary>
        /// Absolute health write that still runs death evaluation (MISC-006).
        /// Prefer this over <c>survivor.Needs.Health = …</c> from combat/events.
        /// </summary>
        public void SetHealth(Survivor survivor, float health)
        {
            if (survivor == null || !survivor.IsAlive || survivor.Needs == null) return;
            float maxCap = survivor.MaxHealthCap;
            if (_personalQuests != null)
                maxCap = Mathf.Min(maxCap, _personalQuests.GetMaxHealthCapForQuests(survivor));
            survivor.Needs.Health = Mathf.Clamp(health, 0f, maxCap);
            OnNeedChanged?.Invoke(survivor, NeedKind.Health, survivor.Needs.Health);
            EvaluateDeath(survivor);
        }

        /// <summary>Delta health write that still runs death evaluation.</summary>
        public void AdjustHealth(Survivor survivor, float delta)
        {
            if (survivor == null || !survivor.IsAlive || survivor.Needs == null || delta == 0f) return;
            SetHealth(survivor, survivor.Needs.Health + delta);
        }

        /// <summary>
        /// SAVE-1D: replay <see cref="OnNeedChanged"/> for every need after a load.
        /// <c>SaveSystem.RestoreSurvivor</c> writes the need fields directly (it must —
        /// the restored values are authoritative and must not be re-clamped or trigger
        /// death evaluation), so observers such as the HUD bars would otherwise keep
        /// rendering pre-load values until the next natural need tick. This does not
        /// mutate any state; it only re-broadcasts what was restored.
        /// </summary>
        public void NotifyNeedsRestored(Survivor survivor)
        {
            if (survivor?.Needs == null || OnNeedChanged == null) return;
            var needs = survivor.Needs;
            OnNeedChanged.Invoke(survivor, NeedKind.Hunger, needs.Hunger);
            OnNeedChanged.Invoke(survivor, NeedKind.Thirst, needs.Thirst);
            OnNeedChanged.Invoke(survivor, NeedKind.Fatigue, needs.Fatigue);
            OnNeedChanged.Invoke(survivor, NeedKind.Warmth, needs.Warmth);
            OnNeedChanged.Invoke(survivor, NeedKind.Morale, needs.Morale);
            OnNeedChanged.Invoke(survivor, NeedKind.Health, needs.Health);
            OnNeedChanged.Invoke(survivor, NeedKind.Hygiene, needs.Hygiene);
        }

        private static float GetValue(Needs needs, NeedKind kind) => kind switch
        {
            NeedKind.Hunger => needs.Hunger,
            NeedKind.Thirst => needs.Thirst,
            NeedKind.Fatigue => needs.Fatigue,
            NeedKind.Warmth => needs.Warmth,
            NeedKind.Morale => needs.Morale,
            NeedKind.Health => needs.Health,
            NeedKind.Hygiene => needs.Hygiene,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };

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
                case NeedKind.Hygiene: needs.Hygiene = value; break;
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
