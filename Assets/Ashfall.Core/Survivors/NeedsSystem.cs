// SPDX-License-Identifier: MIT
using System;
#pragma warning disable CS8618

namespace Ashfall.Core.Survivors
{
    /// <summary>The nine tracked survival needs. Hunger/Thirst/Fatigue/Morale,
    /// Numbness, and RadiationAnxiety are 0..100 where HIGHER = WORSE;
    /// Warmth is 0..100 where LOWER = worse; Health 0..100 where lower = worse.</summary>
    public enum NeedKind
    {
        Hunger,
        Thirst,
        Fatigue,
        Warmth,
        Morale,
        Health,
        Hygiene,
        Numbness,
        RadiationAnxiety
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
        public float Numbness;
        public float RadiationAnxiety;

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
        // Shared defaults so host projections (e.g. HoldfastRuntimeSession's
        // fallback thresholds) derive from one source instead of re-typing 90.
        public const float DefaultHungerCritical = 90f;
        public const float DefaultThirstCritical = 90f;
        public const float DefaultWarmthCritical = 20f;
        // Presentation warn bands (no simulation consequence) as shared defaults
        // so surfaces without a bound NeedsProfile (dashboards, HUD cards) read
        // the same numbers the profile enforces instead of re-typing 70/30/15.
        public const float DefaultHungerWarn = 70f;
        public const float DefaultThirstWarn = 70f;
        public const float DefaultFatigueWarn = 70f;
        public const float DefaultFatigueCritical = 90f;
        public const float DefaultMoraleWarn = 30f;
        public const float DefaultMoraleCritical = 15f;
        public const float DefaultWarmthWarn = 40f;

        public float hungerPerHour = 0.8f;
        public float thirstPerHour = 1.2f;
        public float fatiguePerHour = 0.4f;
        public float warmthLossPerHourInCold = 0.5f;
        public float warmthRestorePerHourNearHeat = 3f;
        public float moraleLossPerHourWhileCritical = 1f;
        public float healthLossFromHunger = 0.4f;
        public float healthLossFromThirst = 0.6f;
        public float healthLossFromCold = 0.3f;
        public float hungerCritical = DefaultHungerCritical;
        public float thirstCritical = DefaultThirstCritical;
        public float warmthCritical = DefaultWarmthCritical;

        // Presentation warning bands (no simulation consequence). The HUD and
        // panels read these instead of re-typing 70 / 30 / 15 literals, so the
        // authored survival-tuning profile is the single source of the bands
        // players see.
        public float hungerWarn = DefaultHungerWarn;
        public float thirstWarn = DefaultThirstWarn;
        public float fatigueWarn = DefaultFatigueWarn;
        public float fatigueCritical = DefaultFatigueCritical;
        public float moraleWarn = DefaultMoraleWarn;
        public float moraleCritical = DefaultMoraleCritical;
        // Warmth is low-is-bad; the warn band sits above the critical floor.
        public float warmthWarn = DefaultWarmthWarn;

        // Shared defaults so presentation surfaces (afflictions/medical panels)
        // read the same health bands the needs profile enforces.
        public const float DefaultHealthWarn = 30f;
        public const float DefaultHealthCritical = 25f;

        // Health is low-is-bad; the warn band sits above the critical floor.
        public float healthWarn = DefaultHealthWarn;
        public float healthCritical = DefaultHealthCritical;

        /// <summary>
        /// Single cold predicate shared by every presentation surface: true when
        /// a warmth value is at or below the authored critical floor. Panels must
        /// call this instead of re-comparing <see cref="warmthCritical"/> so the
        /// threshold can never drift between them.
        /// </summary>
        public bool IsWarmthCritical(float warmth) => warmth <= warmthCritical;

        /// <summary>High-is-bad predicate: true when hunger is at or above critical.</summary>
        public bool IsHungerCritical(float hunger) => hunger >= hungerCritical;

        /// <summary>High-is-bad predicate: true when thirst is at or above critical.</summary>
        public bool IsThirstCritical(float thirst) => thirst >= thirstCritical;

        /// <summary>High-is-bad predicate: true when fatigue is at or above critical.</summary>
        public bool IsFatigueCritical(float fatigue) => fatigue >= fatigueCritical;

        /// <summary>Low-is-bad predicate: true when morale is at or below critical.</summary>
        public bool IsMoraleCritical(float morale) => morale <= moraleCritical;

        /// <summary>
        /// Low-is-bad predicate: true when health is at or below the authored
        /// critical floor. Symmetric with the other need predicates so every
        /// presentation surface classifies a critical survivor identically.
        /// </summary>
        public bool IsHealthCritical(float health) => health <= healthCritical;

        /// <summary>
        /// Low-is-bad predicate: true when health is at or below the authored
        /// presentation warn band (which sits above the critical floor). Panels
        /// read this instead of re-comparing <see cref="healthWarn"/>.
        /// </summary>
        public bool IsHealthWarn(float health) => health <= healthWarn;
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
        private readonly Func<SurvivorNeedsState, bool>? _isNearHeatSource;
        private readonly System.Collections.Generic.List<SurvivorNeedsState> _survivors =
            new System.Collections.Generic.List<SurvivorNeedsState>();

        /// <summary>Campaign day used only for timed external modifiers. A
        /// negative value keeps day-agnostic callers active.</summary>
        public int CurrentDay { get; set; } = -1;

        /// <summary>External, attributable per-hour need contributions.</summary>
        public NeedsModifierStack ModifierStack { get; } = new NeedsModifierStack();

        /// <summary>Optional campaign multipliers for the two authored base need drifts.</summary>
        public Func<float>? HungerRateMultiplier { get; set; }
        public Func<float>? ThirstRateMultiplier { get; set; }

        /// <summary>
        /// Plan 142 — Optional provider returning a fraction [0..0.9] of cold loss reduction from clothing gear.
        /// (survivorId -> reductionFraction).
        /// </summary>
        public Func<string, float>? ClothingWarmthReductionProvider { get; set; }

        public event Action<SurvivorNeedsState, NeedKind, float>? OnNeedChanged;
        public event Action<SurvivorNeedsState, NeedsModifierContribution>? OnAttributedContribution;
        public event Action<SurvivorNeedsState, NeedKind>? OnNeedCritical;
        public event Action<SurvivorNeedsState>? OnDied;

        /// <summary>Optional death-gate: return true to defer death at 0 Health.</summary>
        public Func<SurvivorNeedsState, bool>? TryDeferDeath;

        public NeedsSystem(NeedsProfile? profile = null, Func<SurvivorNeedsState, bool>? isNearHeatSource = null)
        {
            _profile = profile ?? new NeedsProfile();
            _isNearHeatSource = isNearHeatSource;
        }

        /// <summary>
        /// The tuning profile this system simulates with. Exposed read-only so
        /// presentation surfaces (HUD danger coloring, lethal-threshold legend)
        /// read the same critical values the simulation actually enforces
        /// instead of re-typing magic numbers that can drift.
        /// </summary>
        public NeedsProfile Profile => _profile;

        /// <summary>
        /// Register a survivor's needs state for simulation.
        ///
        /// <para><b>One state per survivor id.</b> Registering a state whose
        /// <c>Id</c> already belongs to a registered state <i>replaces</i> it in
        /// place, evicting the older object from the simulation entirely. It does
        /// not shadow it.</para>
        ///
        /// <para>This is defect D1's structural fix. The previous implementation
        /// de-duplicated with <c>List.Contains</c> — reference equality — so two
        /// distinct objects sharing one id could both be registered.
        /// <see cref="Get"/> returned the first, so a stale object won every
        /// lookup while the simulation ticked both. A host restore that rebuilt
        /// state objects without unregistering the old ones therefore left ghosts
        /// that kept decaying, and a ghost reaching 0 Health raised
        /// <see cref="OnDied"/> for a survivor who was alive in the loaded
        /// campaign.</para>
        ///
        /// <para>Replacement keeps the evicted state's slot so tick order is
        /// unchanged; reordering the roster would alter simulation results for the
        /// same seed (AGENTS.md Invariant 4). States with an empty <c>Id</c> cannot
        /// be keyed and keep the reference-only de-duplication.</para>
        /// </summary>
        public void Register(SurvivorNeedsState survivor)
        {
            if (survivor == null) return;
            if (_survivors.Contains(survivor)) return;

            if (!string.IsNullOrEmpty(survivor.Id))
            {
                for (int i = 0; i < _survivors.Count; i++)
                {
                    var existing = _survivors[i];
                    if (existing == null) continue;
                    if (!string.Equals(existing.Id, survivor.Id, StringComparison.Ordinal)) continue;

                    // Evict in place: the ghost leaves the simulation, the slot stays.
                    _survivors[i] = survivor;
                    return;
                }
            }

            _survivors.Add(survivor);
        }

        public void Unregister(SurvivorNeedsState survivor)
        {
            _survivors.Remove(survivor);
        }

        /// <summary>
        /// Remove whatever state is registered for <paramref name="id"/>, if any;
        /// returns whether something was removed. Lets a caller drop a survivor
        /// without having to still hold the original object reference.
        /// </summary>
        public bool UnregisterById(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < _survivors.Count; i++)
            {
                var existing = _survivors[i];
                if (existing == null) continue;
                if (!string.Equals(existing.Id, id, StringComparison.Ordinal)) continue;
                _survivors.RemoveAt(i);
                return true;
            }
            return false;
        }

        /// <summary>
        /// How many states are registered for simulation. Exposed so callers and
        /// tests can detect leaked registrations: a restore that forgets to
        /// unregister leaves ghosts here that keep ticking.
        /// </summary>
        public int RegisteredCount => _survivors.Count;

        /// <summary>
        /// The registered states in simulation order. Read-only view for parity
        /// comparison and determinism assertions.
        /// </summary>
        public System.Collections.Generic.IReadOnlyList<SurvivorNeedsState> Registered => _survivors;

        public SurvivorNeedsState? Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < _survivors.Count; i++)
                if (_survivors[i] != null && string.Equals(_survivors[i].Id, id, StringComparison.Ordinal))
                    return _survivors[i];
            return null;
        }

        public void Modify(string survivorId, NeedKind need, float delta)
        {
            var s = Get(survivorId);
            if (s != null) Modify(s, need, delta);
        }

        public void Tick(float gameHours)
        {
            // Recent attribution is a presentation window for the current
            // simulation pass, not durable state. Clear it once per aggregate
            // tick so the UI cannot report an effect from an arbitrarily old
            // day as if it were current.
            ModifierStack.ClearRecentAttributions();
            for (int i = 0; i < _survivors.Count; i++)
                Tick(_survivors[i], gameHours);
        }

        public void Tick(SurvivorNeedsState survivor, float gameHours)
        {
            if (survivor == null || !survivor.IsAliveState || gameHours <= 0f) return;
            ApplyBaseNeedDrift(survivor, gameHours);
            ModifierStack.ApplyTo(this, survivor, gameHours, CurrentDay);
            ApplyCriticalNeedConsequences(survivor, gameHours);
        }

        public void SetExternalModifier(string survivorId, string sourceId, NeedKind need,
            float deltaPerHour, int priority = 0, int startDay = -1, int endDay = -1)
            => ModifierStack.Set(survivorId, sourceId, need, deltaPerHour, priority, startDay, endDay);

        public bool RemoveExternalModifier(string survivorId, string sourceId, NeedKind need)
            => ModifierStack.Remove(survivorId, sourceId, need);

        public int ClearExternalModifiers(string sourceId)
            => ModifierStack.ClearSource(sourceId);

        /// <summary>
        /// Applies a one-shot effect through the same attributed seam used by
        /// persistent modifiers. The delta itself is not persisted as a rate.
        /// </summary>
        public bool ApplyAttributedDelta(string survivorId, NeedKind need,
            float delta, string sourceId)
        {
            if (string.IsNullOrWhiteSpace(sourceId) || float.IsNaN(delta)
                || float.IsInfinity(delta)) return false;
            var survivor = Get(survivorId);
            if (survivor == null || !survivor.IsAliveState || delta == 0f) return false;
            Modify(survivor, need, delta);
            var contribution = new NeedsModifierContribution(survivorId, sourceId, need, delta);
            ModifierStack.RecordApplied(contribution);
            OnAttributedContribution?.Invoke(survivor, contribution);
            return true;
        }

        internal void NotifyAttributedContribution(SurvivorNeedsState survivor,
            NeedsModifierContribution contribution)
        {
            ModifierStack.RecordApplied(contribution);
            OnAttributedContribution?.Invoke(survivor, contribution);
        }

        private void ApplyBaseNeedDrift(SurvivorNeedsState survivor, float gameHours)
        {
            Modify(survivor, NeedKind.Hunger,
                _profile.hungerPerHour * ResolveRateMultiplier(HungerRateMultiplier) * gameHours);
            Modify(survivor, NeedKind.Thirst,
                _profile.thirstPerHour * ResolveRateMultiplier(ThirstRateMultiplier) * gameHours);
            Modify(survivor, NeedKind.Fatigue, _profile.fatiguePerHour * gameHours);
            ApplyWarmth(survivor, gameHours);
        }

        private static float ResolveRateMultiplier(Func<float>? provider)
        {
            if (provider == null) return 1f;
            float value = provider();
            return float.IsNaN(value) || float.IsInfinity(value) || value < 0f ? 1f : value;
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
            float coldLoss = _profile.warmthLossPerHourInCold;
            if (!warmed && ClothingWarmthReductionProvider != null && !string.IsNullOrEmpty(survivor.Id))
            {
                float reduction = ClothingWarmthReductionProvider(survivor.Id);
                reduction = MathfCompat.Clamp(reduction, 0f, 0.9f);
                coldLoss *= (1f - reduction);
            }
            float rate = warmed ? _profile.warmthRestorePerHourNearHeat : -coldLoss;
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
            OnNeedChanged.Invoke(survivor, NeedKind.Numbness, survivor.Numbness);
            OnNeedChanged.Invoke(survivor, NeedKind.RadiationAnxiety, survivor.RadiationAnxiety);
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
            NeedKind.Numbness => s.Numbness,
            NeedKind.RadiationAnxiety => s.RadiationAnxiety,
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
                case NeedKind.Numbness: s.Numbness = value; break;
                case NeedKind.RadiationAnxiety: s.RadiationAnxiety = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
            OnNeedChanged?.Invoke(s, kind, value);
        }
    }
}
