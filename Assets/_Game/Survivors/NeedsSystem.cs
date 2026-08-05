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

            // #275 Zen State: 80% reduced hunger/thirst/fatigue decay.
            float needsMult = 1f;
            if (_personalQuests != null)
                needsMult = _personalQuests.GetZenNeedsDecayMultiplier(survivor);
            Modify(survivor, NeedKind.Hunger, _profile.hungerPerHour * gameHours * needsMult);
            Modify(survivor, NeedKind.Thirst, _profile.thirstPerHour * gameHours * needsMult);
            Modify(survivor, NeedKind.Fatigue, _profile.fatiguePerHour * gameHours * needsMult);
            ApplyWarmth(survivor, gameHours);

            // Light / photoperiod tick — null-safe; skipped when not wired
            if (_getEffectiveDaylightHours != null && _lightProfile != null)
            {
                bool growLight = _isGrowLightActive != null && _isGrowLightActive();
                bool ignoreDark = IgnoresDarknessMorale != null && IgnoresDarknessMorale(survivor);
                LightSystemHelper.TickSurvivorLight(
                    survivor,
                    gameHours,
                    _getEffectiveDaylightHours(),
                    growLight,
                    _lightProfile,
                    ignoreDarknessMorale: ignoreDark);
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

            // #262 Hyper-Empathetic: morale drifts toward bunker average.
            if (_personalQuests != null && _personalQuests.HasHyperEmpathetic(survivor))
            {
                float avg = ComputeBunkerAverageMorale(survivor);
                _personalQuests.ApplyHyperEmpatheticMorale(survivor, avg, gameHours);
            }

            // #265 Living Saint: permanent Inspired morale floor for the bunker.
            _personalQuests?.ApplyLivingSaintMoraleFloor(survivor);

            if (_personalQuests == null) return;

            // #268 Restless: clamp fatigue to permanent cap.
            float fatCap = _personalQuests.GetMaxFatigueCap(survivor);
            if (survivor.Needs.Fatigue > fatCap)
                survivor.Needs.Fatigue = fatCap;

            // #275 Hunger strike tick (Pacifist Monk).
            _personalQuests.TickHungerStrike(survivor);

            // #269 Hypochondriac: fake illness without placebo → real morale/fatigue hit.
            if (_personalQuests.ShouldGenerateFakeAfflictionAlert(survivor))
                _personalQuests.ApplyHypochondriacPlaceboTick(survivor, givenPlacebo: false);

            // #278 Moral Compass aura (bunker-wide small morale).
            float compass = _personalQuests.GetMoralCompassBunkerMorale(
                _getSurvivors != null ? _getSurvivors() : _survivors);
            if (compass > 0f)
                survivor.Needs.Morale = UnityEngine.Mathf.Min(100f, survivor.Needs.Morale + compass * gameHours * 0.1f);

            // #282 Agoraphile: fractional bunker morale hit (day counter advances in TickDaily).
            if (_personalQuests.HasAgoraphile(survivor) && !survivor.IsOnExpedition)
            {
                float hit = _personalQuests.GetAgoraphileBunkerMoraleHitPerDay(survivor) * (gameHours / 24f);
                if (hit > 0f)
                    survivor.Needs.Morale = UnityEngine.Mathf.Max(0f, survivor.Needs.Morale - hit);
            }

            // #267 Forced chem: only reset dose clock when host actually drains stock.
            // No free withdrawal immunity — missing stock leaves HoursSinceLastDose running.
            if (_personalQuests.ShouldForceConsumeMedicalChems(survivor))
            {
                bool consumed = ForcedChemConsumeHandler != null
                    && ForcedChemConsumeHandler(survivor);
                if (consumed)
                {
                    survivor.HoursSinceLastDose = 0f;
                    _personalQuests.NotifyChemUsed(survivor);
                }
            }

            // #278 Failing Heart: decay max stamina proxy + extra fatigue pressure.
            if (_personalQuests.HasFailingHeart(survivor))
            {
                _personalQuests.TickFailingHeart(survivor, currentDay: survivor.DaysAlive);
                float stam = _personalQuests.GetFailingHeartStaminaMax(
                    survivor, survivor.DaysAlive);
                if (stam < 100f && gameHours > 0f)
                {
                    float pressure = (1f - stam / 100f) * 2f * gameHours;
                    survivor.Needs.Fatigue = UnityEngine.Mathf.Min(
                        100f, survivor.Needs.Fatigue + pressure);
                }
            }

            // #284 Black Lung: reduced max stamina → proportional fatigue pressure.
            if (_personalQuests.HasBlackLung(survivor) && gameHours > 0f)
            {
                float stamMult = _personalQuests.GetBlackLungStaminaMaxMultiplier(survivor);
                if (stamMult < 1f)
                {
                    float pressure = (1f - stamMult) * 3f * gameHours;
                    survivor.Needs.Fatigue = UnityEngine.Mathf.Min(
                        100f, survivor.Needs.Fatigue + pressure);
                }
            }

            // #284 Claustrophilic: morale gain when already deep/underground (room id hint).
            if (_personalQuests.HasClaustrophilic(survivor)
                && IsSmallUndergroundRoomId(survivor.CurrentRoomId))
            {
                _personalQuests.ApplyClaustrophilicMorale(
                    survivor, inSmallUndergroundRoom: true, gameHours: gameHours);
            }

            // #287 Neat Freak + #281 Photogenic: hygiene-driven morale pressure.
            float hygiene01 = survivor.Needs.Hygiene / 100f;
            _personalQuests.ApplyNeatFreakHygienePressure(survivor, hygiene01);
            _personalQuests.ApplyPhotogenicHygieneMorale(survivor, hygiene01);

            // #291 Frail: clamp health to cap.
            float healthCap = _personalQuests.GetMaxHealthCapForQuests(survivor);
            if (survivor.Needs.Health > healthCap)
                survivor.Needs.Health = healthCap;
        }

        private static bool IsSmallUndergroundRoomId(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            return roomId.IndexOf("bunker", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || roomId.IndexOf("cellar", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || roomId.IndexOf("tunnel", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || roomId.IndexOf("shaft", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || roomId.IndexOf("deep", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || roomId.IndexOf("mine", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || roomId.IndexOf("sublevel", System.StringComparison.OrdinalIgnoreCase) >= 0
                   || roomId.IndexOf("basement", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private float ComputeBunkerAverageMorale(Survivor exclude = null)
        {
            var all = _getSurvivors != null ? _getSurvivors() : _survivors;
            if (all == null || all.Count == 0) return 50f;
            float sum = 0f;
            int n = 0;
            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                if (s == null || !s.IsAlive || s.Needs == null) continue;
                if (exclude != null && ReferenceEquals(s, exclude)) continue;
                sum += s.Needs.Morale;
                n++;
            }
            return n > 0 ? sum / n : 50f;
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
            OnDied?.Invoke(survivor);
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
                case NeedKind.Hygiene: return needs.Hygiene;
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
