using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.Phantoms;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side aggregate view of one survivor's Phase-0 effect state.
    /// The values here are DERIVED from the Core systems for presentation and
    /// save; the Core systems own the rules. Real gameplay consumers (NeedsSystem
    /// morale/health/fatigue, CraftingSystem craft time, ExpeditionSystem stamina,
    /// Journal narrative) are reached through <see cref="Phase0EffectConsumers"/>.
    /// </summary>
    public class Phase0SurvivorEffects
    {
        public string survivorId = string.Empty;
        /// <summary>Combined work-efficiency factor from phantom motivation and flashback penalty.</summary>
        public float workEfficiencyMultiplier = 1f;
        /// <summary>Hours the survivor refuses to work (phantom breakdown).</summary>
        public float workRefusalHours = 0f;
        /// <summary>Stamina multiplier (respiratory degeneration).</summary>
        public float staminaMultiplier = 1f;
        /// <summary>Guilt insomnia severity 0..1.</summary>
        public float guiltInsomniaSeverity = 0f;
        /// <summary>Combat trauma hypervigilance 0..1 (defense bonus).</summary>
        public float hypervigilance = 0f;
        /// <summary>Moral branch direction (Neutral until decided).</summary>
        public string moralBranch = "Neutral";
        /// <summary>Radiation sickness phase.</summary>
        public string radiationPhase = "Healthy";
        /// <summary>Dependency crafting penalty factor (0 = none).</summary>
        public float dependencyCraftingPenalty = 0f;
        /// <summary>Dependency combat penalty factor (0 = none).</summary>
        public float dependencyCombatPenalty = 0f;
        /// <summary>Final-wish state (empty / active / completed / failed).</summary>
        public string finalWishState = string.Empty;
    }

    /// <summary>Serialized Phase-0 effects envelope (all 10 systems).</summary>
    public class Phase0EffectsSaveState
    {
        public PhaseProgressionSaveState radiationPhase = new PhaseProgressionSaveState();
        public PhantomMemoryEngineState phantom = new PhantomMemoryEngineState();
        public GuiltInsomniaSaveState guilt = new GuiltInsomniaSaveState();
        public CombatTraumaSaveState combatTrauma = new CombatTraumaSaveState();
        public SomaticFlashbackSaveState flashbacks = new SomaticFlashbackSaveState();
        public MoralBranchingSaveState moral = new MoralBranchingSaveState();
        public TradeSpecialtySaveState tradeSpecialty = new TradeSpecialtySaveState();
        public FinalWishSaveState finalWishes = new FinalWishSaveState();
        public RespiratoryDegenerationState respiratory = new RespiratoryDegenerationState();
        public List<Phase0SurvivorEffects> effects = new List<Phase0SurvivorEffects>();
        public float permanentShelterMoraleBuff = 0f;
    }

    /// <summary>
    /// Immutable real-consumer wiring bundle. The host (Main.cs) constructs one
    /// fully-bound instance and assigns it to <see cref="Phase0HostSession.Consumers"/>
    /// so Phase-0 effects reach the authoritative gameplay consumers instead of
    /// living in a display value. There are no mutable fields to silently leave
    /// unwired.
    ///
    /// Effect classification:
    ///  - Essential (health/morale/fatigue/shelter-morale): default to named
    ///    no-op adapters when null, tracked as unbound.
    ///  - Production-required (progression/medical/narrative): stay null when
    ///    unbound so the session's null-check skips preserve headless behavior;
    ///    tracked as unbound for startup validation.
    ///  - Truly optional (ApplyWorkRefusalHours): null when unbound, not tracked.
    ///
    /// Use <see cref="NoOp"/> (with optional overrides) for isolated tests.
    /// </summary>
    public sealed class Phase0EffectConsumers
    {
        // ── Essential effects (health / morale / fatigue) — no-op when unbound ──

        /// <summary>survivorId, morale delta → NeedsSystem.</summary>
        public Action<string, float> ApplyMoraleDelta { get; }
        /// <summary>survivorId, health delta → NeedsSystem.</summary>
        public Action<string, float> ApplyHealthDelta { get; }
        /// <summary>survivorId, fatigue delta → NeedsSystem.</summary>
        public Action<string, float> ApplyFatigueDelta { get; }
        /// <summary>Shelter-wide morale delta (final wish / moral branching).</summary>
        public Action<float> ApplyShelterMoraleDelta { get; }

        // ── Production-required (progression / medical / narrative) — null when unbound ──

        /// <summary>survivorId, work-efficiency multiplier → work/task consumer.</summary>
        public Action<string, float> ApplyWorkEfficiencyMultiplier { get; }
        /// <summary>survivorId, crafting penalty factor → CraftingSystem time multiplier.</summary>
        public Action<string, float> ApplyCraftingPenaltyFactor { get; }
        /// <summary>survivorId, combat penalty factor → expedition/combat consumer.</summary>
        public Action<string, float> ApplyCombatPenaltyFactor { get; }
        /// <summary>survivorId, stamina drain multiplier → ExpeditionSystem stamina drain.</summary>
        public Action<string, float> ApplyStaminaDrainMultiplier { get; }
        /// <summary>narrativeId, survivorId → Journal / event runner.</summary>
        public Action<string, string> FireNarrativeEvent { get; }
        /// <summary>survivorId, afflictionId → medical / chronic-illness authority.</summary>
        public Action<string, string> GrantChronicIllness { get; }
        /// <summary>survivorId → RadiationSystem dose reset (Prodromal metabolized the acute dose).</summary>
        public Action<string> ResetRadiationDose { get; }

        // ── Truly optional — null when unbound, not validated ──

        /// <summary>survivorId, work-refusal hours → work/task consumer.</summary>
        public Action<string, float> ApplyWorkRefusalHours { get; }

        private readonly List<string> _unboundRequired;

        /// <summary>
        /// Names of production-required effects that were not explicitly bound
        /// (null passed → no-op/null). Empty when fully wired. Checked at startup
        /// so a production system cannot silently run with a missing health,
        /// morale, inventory, or progression effect.
        /// </summary>
        public IReadOnlyList<string> UnboundRequiredEffects => _unboundRequired;

        public Phase0EffectConsumers(
            Action<string, float>? applyMoraleDelta,
            Action<string, float>? applyHealthDelta,
            Action<string, float>? applyFatigueDelta,
            Action<float>? applyShelterMoraleDelta,
            Action<string, float>? applyWorkEfficiencyMultiplier = null,
            Action<string, float>? applyCraftingPenaltyFactor = null,
            Action<string, float>? applyCombatPenaltyFactor = null,
            Action<string, float>? applyStaminaDrainMultiplier = null,
            Action<string, string>? fireNarrativeEvent = null,
            Action<string, string>? grantChronicIllness = null,
            Action<string>? resetRadiationDose = null,
            Action<string, float>? applyWorkRefusalHours = null)
        {
            ApplyMoraleDelta = applyMoraleDelta ?? NoOpMoraleDelta;
            ApplyHealthDelta = applyHealthDelta ?? NoOpHealthDelta;
            ApplyFatigueDelta = applyFatigueDelta ?? NoOpFatigueDelta;
            ApplyShelterMoraleDelta = applyShelterMoraleDelta ?? NoOpShelterMoraleDelta;
            ApplyWorkEfficiencyMultiplier = applyWorkEfficiencyMultiplier;
            ApplyCraftingPenaltyFactor = applyCraftingPenaltyFactor;
            ApplyCombatPenaltyFactor = applyCombatPenaltyFactor;
            ApplyStaminaDrainMultiplier = applyStaminaDrainMultiplier;
            FireNarrativeEvent = fireNarrativeEvent;
            GrantChronicIllness = grantChronicIllness;
            ResetRadiationDose = resetRadiationDose;
            ApplyWorkRefusalHours = applyWorkRefusalHours;

            _unboundRequired = new List<string>();
            if (applyMoraleDelta == null) _unboundRequired.Add(nameof(ApplyMoraleDelta));
            if (applyHealthDelta == null) _unboundRequired.Add(nameof(ApplyHealthDelta));
            if (applyFatigueDelta == null) _unboundRequired.Add(nameof(ApplyFatigueDelta));
            if (applyShelterMoraleDelta == null) _unboundRequired.Add(nameof(ApplyShelterMoraleDelta));
            if (applyWorkEfficiencyMultiplier == null) _unboundRequired.Add(nameof(ApplyWorkEfficiencyMultiplier));
            if (applyCraftingPenaltyFactor == null) _unboundRequired.Add(nameof(ApplyCraftingPenaltyFactor));
            if (applyCombatPenaltyFactor == null) _unboundRequired.Add(nameof(ApplyCombatPenaltyFactor));
            if (applyStaminaDrainMultiplier == null) _unboundRequired.Add(nameof(ApplyStaminaDrainMultiplier));
            if (fireNarrativeEvent == null) _unboundRequired.Add(nameof(FireNarrativeEvent));
            if (grantChronicIllness == null) _unboundRequired.Add(nameof(GrantChronicIllness));
            if (resetRadiationDose == null) _unboundRequired.Add(nameof(ResetRadiationDose));
        }

        /// <summary>
        /// All-effects-unbound instance for isolated tests, with optional
        /// overrides. Essential effects use no-op adapters; production-required
        /// effects are null (session null-checks skip them).
        /// </summary>
        public static Phase0EffectConsumers NoOp(
            Action<string, float>? applyMoraleDelta = null,
            Action<string, float>? applyHealthDelta = null,
            Action<string, float>? applyFatigueDelta = null,
            Action<float>? applyShelterMoraleDelta = null,
            Action<string, float>? applyWorkEfficiencyMultiplier = null,
            Action<string, float>? applyCraftingPenaltyFactor = null,
            Action<string, float>? applyCombatPenaltyFactor = null,
            Action<string, float>? applyStaminaDrainMultiplier = null,
            Action<string, string>? fireNarrativeEvent = null,
            Action<string, string>? grantChronicIllness = null,
            Action<string>? resetRadiationDose = null,
            Action<string, float>? applyWorkRefusalHours = null)
            => new Phase0EffectConsumers(
                applyMoraleDelta, applyHealthDelta, applyFatigueDelta, applyShelterMoraleDelta,
                applyWorkEfficiencyMultiplier, applyCraftingPenaltyFactor, applyCombatPenaltyFactor,
                applyStaminaDrainMultiplier, fireNarrativeEvent, grantChronicIllness,
                resetRadiationDose, applyWorkRefusalHours);

        // ── Named no-op adapters for essential effects ──

        public static readonly Action<string, float> NoOpMoraleDelta = (_, __) => { };
        public static readonly Action<string, float> NoOpHealthDelta = (_, __) => { };
        public static readonly Action<string, float> NoOpFatigueDelta = (_, __) => { };
        public static readonly Action<float> NoOpShelterMoraleDelta = _ => { };
    }

    /// <summary>
    /// Thin Godot-host session for ALL Phase-0 psychological/medical effects.
    /// Owns the ten engine-agnostic Core systems and wires every effect event to
    /// the injected <see cref="Consumers"/> so effects reach real gameplay consumers.
    /// Host-derived per-survivor views are a pure function of Core state. All rules
    /// live in Ashfall.Core; this session only wires and presents.
    ///
    /// Owned systems:
    ///  1. Radiation Phase Progression
    ///  2. Phantom Memory
    ///  3. Guilt Insomnia
    ///  4. Combat Trauma
    ///  5. Somatic Flashback
    ///  6. Moral Branching
    ///  7. Chemical Dependency (shared with MedicalHostSession via <see cref="Dependency"/>)
    ///  8. Trade Specialty
    ///  9. Final Wish
    /// 10. Respiratory Degeneration
    /// </summary>
    public sealed class Phase0HostSession
    : HostSessionBase{
        public const int DefaultSeed = 808;

        public RadiationPhaseProgression RadiationPhase { get; }
        public PhantomMemoryEngine Phantom { get; }
        public GuiltInsomniaSystem Guilt { get; }
        public CombatTraumaSystem CombatTrauma { get; }
        public SomaticFlashbackSystem Flashbacks { get; }
        public MoralBranchingSystem Moral { get; }
        /// <summary>
        /// Chemical Dependency authority. Shares the MedicalHostSession's instance
        /// (single source of truth); a fresh instance is only created for
        /// self-contained headless selftests. Use the MedicalHostSession-owned
        /// instance in the running game via the <paramref name="dependency"/> ctor
        /// parameter so Phase-0 does not fork the ledger.
        /// </summary>
        public ChemicalDependencySystem Dependency { get; }

        public TradeSpecialtySystem TradeSpecialty { get; }
        public FinalWishSystem FinalWish { get; }
        public RespiratoryDegenerationSystem Respiratory { get; }

        /// <summary>Real-consumer wiring bundle. Set by the host (Main.cs) with a fully-bound instance.</summary>
        public Phase0EffectConsumers Consumers { get; set; } = Phase0EffectConsumers.NoOp();

        /// <summary>Accumulated permanent shelter-wide morale buff from completed final wishes.</summary>
        public float PermanentShelterMoraleBuff { get; private set; }

        /// <summary>Set by the host from the current expedition/zone (real ash-zone signal).</summary>
        public bool IsInAshZone { get; set; }

        /// <summary>Set by the host from the world state (real fallout-storm signal).</summary>
        public bool IsInFalloutStorm { get; set; }

        /// <summary>Set by the host from the photoperiod (real night signal for trauma false alarms).</summary>
        public bool IsNightTime { get; set; }

        /// <summary>Current sim day, injected by the host (guilt expiry, wishes, phases).</summary>
        public int CurrentDay { get; set; } = 1;

        /// <summary>Air-filtration health 0..100, injected by the shelter host.</summary>
        public Func<float> GetFilterHealth;

        public IReadOnlyList<Phase0SurvivorEffects> Effects => _effects;

        /// <summary>Public accessor for the derived host view of one survivor.</summary>
        public Phase0SurvivorEffects GetEffects(string survivorId) => GetOrCreateEffects(survivorId);

        public string LastEvent { get; private set; } = string.Empty;

        // ── Named relay handlers so UnsubscribeSystemEvents can clean up ──
        private Action _onRadiationPhaseStateChanged = null!;
        private Action<PhantomMemoryEngineState> _onPhantomStateChanged = null!;
        private Action _onGuiltStateChanged = null!;
        private Action _onCombatTraumaStateChanged = null!;
        private Action _onFlashbacksStateChanged = null!;
        private Action _onMoralStateChanged = null!;
        private Action _onDependencyStateChanged = null!;
        private Action _onTradeSpecialtyStateChanged = null!;
        private Action _onFinalWishStateChanged = null!;
        private Action _onRespiratoryStateChanged = null!;
        private readonly List<Phase0SurvivorEffects> _effects = new List<Phase0SurvivorEffects>();
        private readonly List<string> _aliveSurvivorIds = new List<string>();
        private readonly Dictionary<string, MoralBranchState> _moralStates = new Dictionary<string, MoralBranchState>();
        private readonly Dictionary<string, PhaseProgressionState> _phaseStates = new Dictionary<string, PhaseProgressionState>();
        private readonly ISeededRng _rng;

        public Phase0HostSession(int seed = DefaultSeed, ChemicalDependencySystem dependency = null!)
        {
            _rng = new CoreSeededRng(seed);

            // ── 1. Radiation Phase Progression ───────────────────────────
            RadiationPhase = new RadiationPhaseProgression(new CoreSeededRng(seed));
            RadiationPhase.OnHealthDeltaRequested += (sv, delta) => Consumers.ApplyHealthDelta?.Invoke(sv, delta);
            RadiationPhase.OnMoraleDeltaRequested += (sv, delta) => Consumers.ApplyMoraleDelta?.Invoke(sv, delta);
            RadiationPhase.OnChronicIllnessRequested += sv => Consumers.GrantChronicIllness?.Invoke(sv, "radiation_sickness");
            RadiationPhase.OnChronicFibrosisMarked += sv => Consumers.GrantChronicIllness?.Invoke(sv, "chronic_fibrosis");
            RadiationPhase.OnRadiationDoseResetRequested += sv => Consumers.ResetRadiationDose?.Invoke(sv);
            RadiationPhase.OnTerminalPrognosisDeclared += (sv, days) =>
            {
                LastEvent = $"TERMINAL PROGNOSIS: {sv} — {days:F0} days remaining. A final wish opens.";
                RaiseStateChanged();
            };
            RadiationPhase.OnPhaseChanged += (sv, oldP, newP) =>
            {
                LastEvent = $"Radiation phase: {sv} {oldP} → {newP}.";
                RecomputeSurvivorEffects(sv);
                RaiseStateChanged();
            };

            // ── 2. Phantom Memory ────────────────────────────────────────
            Phantom = new PhantomMemoryEngine();
            Phantom.OnPhantomTriggered += (sv, item, isMotivation) =>
            {
                RecomputeSurvivorEffects(sv);
                if (isMotivation)
                {
                    Consumers.ApplyMoraleDelta?.Invoke(sv, PhantomMemoryEngine.MotivationMoraleBoost);
                    Consumers.ApplyWorkEfficiencyMultiplier?.Invoke(sv, GetEffects(sv).workEfficiencyMultiplier);
                    LastEvent = $"Phantom memory: {sv} motivated by {item}. Work speed up.";
                }
                else
                {
                    Consumers.ApplyMoraleDelta?.Invoke(sv, PhantomMemoryEngine.BreakdownMoraleDrop);
                    Consumers.ApplyWorkRefusalHours?.Invoke(sv, PhantomMemoryEngine.BreakdownWorkRefusalHours);
                    LastEvent = $"Phantom memory: {sv} breaks down over {item}. Refuses work.";
                }
                RaiseStateChanged();
            };
            Phantom.OnPhantomBreakdown += (sv, item) => { /* handled above via OnPhantomTriggered */ };

            // ── 3. Guilt Insomnia ────────────────────────────────────────
            Guilt = new GuiltInsomniaSystem();
            Guilt.OnGuiltRecorded += (sv, rec) =>
            {
                Consumers.ApplyMoraleDelta?.Invoke(sv, -rec.severity * 10f);
                LastEvent = $"Guilt recorded: {sv} ({rec.sourceId}, severity {rec.severity:F2}). Sleep quality falls.";
                RecomputeSurvivorEffects(sv);
                RaiseStateChanged();
            };
            Guilt.OnGuiltInsomniaCritical += sv =>
            {
                Consumers.ApplyFatigueDelta?.Invoke(sv, 20f);
                LastEvent = $"GUILT INSOMNIA: {sv} cannot sleep.";
                RaiseStateChanged();
            };

            // ── 4. Combat Trauma ─────────────────────────────────────────
            CombatTrauma = new CombatTraumaSystem
            {
                Rng = new CoreSeededRng(seed + 1),
                ApplyMoraleDelta = (sv, delta) => Consumers.ApplyMoraleDelta?.Invoke(sv, delta)
            };
            CombatTrauma.OnFalseAlarmTriggered += sv =>
            {
                LastEvent = $"FALSE ALARM: {sv} startled the bunker at night.";
                RecomputeSurvivorEffects(sv);
                RaiseStateChanged();
            };

            // ── 5. Somatic Flashback ─────────────────────────────────────
            Flashbacks = new SomaticFlashbackSystem
            {
                Rng = new CoreSeededRng(seed + 2),
                GetAliveSurvivorIds = () => _aliveSurvivorIds,
                IsCompanionInSameRoom = (a, b) => false
            };
            Flashbacks.OnFlashbackTriggered += (sv, duration) =>
            {
                RecomputeSurvivorEffects(sv);
                Consumers.ApplyWorkEfficiencyMultiplier?.Invoke(sv, GetEffects(sv).workEfficiencyMultiplier);
                LastEvent = $"SOMATIC FLASHBACK: {sv} — {duration:F1}h of distortion. Work efficiency drops.";
                RaiseStateChanged();
            };
            Flashbacks.OnFlashbackEnded += sv =>
            {
                RecomputeSurvivorEffects(sv);
                Consumers.ApplyWorkEfficiencyMultiplier?.Invoke(sv, GetEffects(sv).workEfficiencyMultiplier);
                LastEvent = $"Flashback ended: {sv}.";
                RaiseStateChanged();
            };

            // ── 6. Moral Branching ───────────────────────────────────────
            Moral = new MoralBranchingSystem
            {
                ApplyMoraleDelta = (state, delta) =>
                {
                    if (state != null) Consumers.ApplyMoraleDelta?.Invoke(state.SurvivorId, delta);
                },
                ApplyShelterMoraleDelta = delta => Consumers.ApplyShelterMoraleDelta?.Invoke(delta)
            };
            Moral.OnBranchDecided += (state, dir) =>
            {
                LastEvent = $"Moral branch decided: {state.SurvivorId} → {dir}.";
                RecomputeSurvivorEffects(state.SurvivorId);
                RaiseStateChanged();
            };

            // ── 7. Chemical Dependency (single authority shared with MedicalHostSession) ──
            Dependency = dependency ?? new ChemicalDependencySystem();
            Dependency.OnMoraleDrainRequested += (sv, amount) => Consumers.ApplyMoraleDelta?.Invoke(sv, -amount);
            Dependency.OnCraftingPenaltyChanged += (sv, factor) =>
            {
                Consumers.ApplyCraftingPenaltyFactor?.Invoke(sv, factor);
                RecomputeSurvivorEffects(sv);
                RaiseStateChanged();
            };
            Dependency.OnCombatPenaltyChanged += (sv, factor) =>
            {
                Consumers.ApplyCombatPenaltyFactor?.Invoke(sv, factor);
                RecomputeSurvivorEffects(sv);
                RaiseStateChanged();
            };
            Dependency.OnDependencyFormed += (sv, item) => { LastEvent = $"DEPENDENCY: {sv} on {item}."; RaiseStateChanged(); };

            // ── 8. Trade Specialty ───────────────────────────────────────
            TradeSpecialty = new TradeSpecialtySystem
            {
                GrantSkillBonus = (sv, prof, bonus) =>
                {
                    LastEvent = $"Specialty: {sv} ({prof}) skill +{bonus:F2}.";
                    RaiseStateChanged();
                },
                ApplyMoraleDelta = (sv, delta) =>
                {
                    Consumers.ApplyMoraleDelta?.Invoke(sv, delta);
                    LastEvent = $"Specialty: {sv} morale {delta:+#.##;-#.##;0}.";
                    RaiseStateChanged();
                },
                GetNarrativeEventId = prof => $"narrative_trade_mastery_{prof}",
                FireNarrativeEvent = (narrativeId, sv) =>
                {
                    Consumers.FireNarrativeEvent?.Invoke(narrativeId, sv);
                    LastEvent = $"Narrative event fired: {narrativeId} for {sv}.";
                    RaiseStateChanged();
                }
            };

            // ── 9. Final Wish ────────────────────────────────────────────
            FinalWish = new FinalWishSystem
            {
                Rng = new CoreSeededRng(seed + 3),
                ApplyPermanentShelterMoraleBuff = delta =>
                {
                    PermanentShelterMoraleBuff += delta;
                    Consumers.ApplyShelterMoraleDelta?.Invoke(delta);
                    LastEvent = $"Permanent shelter morale {(delta >= 0 ? "+" : "")}{delta:F0} (total {PermanentShelterMoraleBuff:F0}).";
                    RaiseStateChanged();
                }
            };
            FinalWish.OnFinalWishCompleted += sv =>
            {
                Consumers.FireNarrativeEvent?.Invoke("narrative_final_wish_completed", sv);
                RecomputeSurvivorEffects(sv);
                RaiseStateChanged();
            };

            // ── 10. Respiratory Degeneration ─────────────────────────────
            Respiratory = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => GetFilterHealth?.Invoke() ?? 100f,
                IsInFalloutStorm = () => IsInFalloutStorm,
                IsInAshZone = () => IsInAshZone
            };
            Respiratory.OnStaminaPenaltyRequested += (sv, factor) =>
            {
                Consumers.ApplyStaminaDrainMultiplier?.Invoke(sv, factor);
                RecomputeSurvivorEffects(sv);
                RaiseStateChanged();
            };
            Respiratory.OnMoraleDrainRequested += (sv, amount) => Consumers.ApplyMoraleDelta?.Invoke(sv, amount);
            Respiratory.OnSevereCoughStarted += sv =>
            {
                LastEvent = $"SEVERE COUGH: {sv} — stamina reduced until treated.";
                RaiseStateChanged();
            };
            Respiratory.OnRequiresInhaler += sv => { LastEvent = $"{sv} now requires an inhaler."; RaiseStateChanged(); };

            // ── Global state-changed relay ───────────────────────────────
            _onRadiationPhaseStateChanged = () => { RecomputeAllEffects(); RaiseStateChanged(); };
            RadiationPhase.OnStateChanged += _onRadiationPhaseStateChanged;
            _onPhantomStateChanged = _ => RecomputeAllEffects();
            Phantom.OnStateChanged += _onPhantomStateChanged;
            _onGuiltStateChanged = () => RecomputeAllEffects();
            Guilt.OnStateChanged += _onGuiltStateChanged;
            _onCombatTraumaStateChanged = () => RecomputeAllEffects();
            CombatTrauma.OnStateChanged += _onCombatTraumaStateChanged;
            _onFlashbacksStateChanged = () => RecomputeAllEffects();
            Flashbacks.OnStateChanged += _onFlashbacksStateChanged;
            _onMoralStateChanged = () => RecomputeAllEffects();
            Moral.OnStateChanged += _onMoralStateChanged;
            _onDependencyStateChanged = () => RecomputeAllEffects();
            Dependency.OnStateChanged += _onDependencyStateChanged;
            _onTradeSpecialtyStateChanged = () => RaiseStateChanged();
            TradeSpecialty.OnStateChanged += _onTradeSpecialtyStateChanged;
            _onFinalWishStateChanged = () => RecomputeAllEffects();
            FinalWish.OnStateChanged += _onFinalWishStateChanged;
            _onRespiratoryStateChanged = () => RecomputeAllEffects();
            Respiratory.OnStateChanged += _onRespiratoryStateChanged;
        }

        /// <summary>
        /// Logs any production-required Phase-0 effects still unbound on the
        /// current <see cref="Consumers"/>. An empty list means every health,
        /// morale, fatigue, and progression effect reaches a real consumer.
        /// Call after the host assigns <see cref="Consumers"/>.
        /// </summary>
        public void ValidateConsumers()
        {
            var unbound = Consumers.UnboundRequiredEffects;
            if (unbound.Count > 0)
            {
                GD.PrintErr("[Ashfall Godot] Phase-0 effect consumers unbound: "
                    + string.Join(", ", unbound)
                    + ". Effects will silently no-op in production.");
            }
        }

        // ── Roster wiring ─────────────────────────────────────────────

        /// <summary>Load the phantom_triggers.json catalog into the engine (the authority).</summary>
        public void LoadPhantomRules(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            try
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                string path = System.IO.Path.Combine(dataDir, "phantom_triggers.json");
                if (!files.FileExists(path)) return;

                string text = files.ReadAllText(path);
                List<PhantomTriggerJsonEntry>? entries = null;
                try
                {
                    var catalog = json.Deserialize<PhantomTriggerCatalogJson>(text);
                    entries = catalog?.items;
                }
                catch
                {
                    // Fallback in case of bare array JSON
                    entries = json.Deserialize<List<PhantomTriggerJsonEntry>>(text);
                }

                if (entries == null) return;

                for (int i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];
                    if (entry == null || string.IsNullOrEmpty(entry.background_id)) continue;
                    if (entry.triggers == null) continue;
                    for (int j = 0; j < entry.triggers.Count; j++)
                    {
                        var t = entry.triggers[j];
                        if (t == null) continue;
                        Phantom.RegisterRule(
                            entry.background_id,
                            t.item_category,
                            t.motivation_chance,
                            t.description,
                            t.motivation_text,
                            t.breakdown_text);
                    }
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Phase0] Failed to load phantom rules: {ex.Message}");
            }
        }

        /// <summary>Built-in fallback phantom rules (host demo convenience).</summary>
        public void RegisterDefaultRules()
        {
            Phantom.RegisterRule("former_soldier", "military", 0.40f, "desc",
                "{name} pockets the tags. 'I'll remember them,' they say. Their posture straightens.",
                "{name} reads the name on the tag and goes pale. They knew this person.");
            Phantom.RegisterRule("nurse", "medical", 0.50f, "desc",
                "{name} taps the bell of the stethoscope. 'Still works,' they say.",
                "{name} listens to their own heartbeat through the stethoscope.");
            Phantom.RegisterRule("teacher", "correspondence", 0.50f, "desc",
                "{name} finds a blank page and writes a new lesson at the top.",
                "{name} reads a name written in clumsy letters on the cover.");
            Phantom.RegisterRule("generic", "photograph", 0.50f, "desc",
                "{name} props the photograph against the wall. 'They'd want us to keep going.'",
                "{name} can't stop looking at the photograph. 'These people had lives.'");
        }

        /// <summary>Register the alive survivor ids (the host's roster authority).</summary>
        public void RegisterSurvivors(IEnumerable<string> ids)
        {
            _aliveSurvivorIds.Clear();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    if (string.IsNullOrEmpty(id)) continue;
                    _aliveSurvivorIds.Add(id);
                    GetOrCreateEffects(id);
                    GetOrCreatePhaseState(id);
                    GetOrCreateMoralState(id);
                    CombatTrauma.RegisterSurvivor(id);
                }
            }
            RaiseStateChanged();
        }

        /// <summary>Seed a small demo roster (host demo convenience).</summary>
        public void SeedDemoRoster()
        {
            RegisterSurvivors(new[] { "survivor_dr_sarah_chen", "survivor_gunner_mikhail", "elena_vasquez" });
        }

        // ── Public actions (real commands, thin) ──────────────────────

        public string ScavengeItem(string survivorId, string itemId)
        {
            var sv = new PhantomSurvivorSnapshot
            {
                survivorId = survivorId,
                displayName = survivorId,
                backgroundId = InferBackground(survivorId),
                isAlive = true
            };
            var outcome = Phantom.OnItemScavenged(sv, itemId, _rng);
            LastEvent = outcome != TriggerOutcome.None
                ? $"Phantom: {survivorId} {(outcome == TriggerOutcome.Motivation ? "motivated" : "broke down")} on {itemId}."
                : $"Phantom: no memory triggered for {survivorId} ({itemId}).";
            RaiseStateChanged();
            return LastEvent;
        }

        public string RaiseNoise(string survivorId)
        {
            Flashbacks.OnAudioEvent("siren", 1f);
            LastEvent = "Noise event raised (flashbacks checked).";
            RaiseStateChanged();
            return LastEvent;
        }

        public string CraftItem(string survivorId, string professionId, string itemId)
        {
            TradeSpecialty.OnItemCrafted(survivorId, professionId, itemId);
            int tier = TradeSpecialty.GetMasteryTier(survivorId);
            LastEvent = $"{survivorId} crafted {itemId}: specialty tier {tier}/3.";
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>Record a moral choice for a survivor (real event/narrative flow).</summary>
        public string RecordMoralChoice(string survivorId, bool isEmpathyChoice)
        {
            var state = GetOrCreateMoralState(survivorId);
            Moral.RegisterMoralChoice(state, isEmpathyChoice);
            LastEvent = $"{survivorId}: moral choice recorded ({(isEmpathyChoice ? "empathy" : "pragmatism")}).";
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>Record guilt from a ruthless choice (real guilt source).</summary>
        public string RecordGuilt(string survivorId, string sourceId, float severity)
        {
            Guilt.RecordGuilt(survivorId, sourceId, severity, CurrentDay);
            LastEvent = $"Guilt recorded for {survivorId} ({sourceId}).";
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>Register a combat survival (real raid/skirmish outcome).</summary>
        public string RegisterCombatSurvived(string survivorId)
        {
            CombatTrauma.OnCombatSurvived(survivorId);
            LastEvent = $"{survivorId} survived combat. Hypervigilance rises.";
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>Consume a substance (real inventory consumption → dependency).</summary>
        public string ConsumeSubstance(string survivorId, string itemId, ChemicalDependencyKind kind)
        {
            Dependency.OnSubstanceConsumed(survivorId, itemId, kind);
            LastEvent = $"Substance consumed: {survivorId} ({itemId}).";
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>Declare a terminal prognosis and open the final wish questline.</summary>
        public string DeclareTerminalPrognosis(string survivorId, string archetypeId)
        {
            FinalWish.DeclareTerminalPrognosis(survivorId, archetypeId, true);
            LastEvent = $"Terminal prognosis declared for {survivorId}.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string AdvanceFinalWish(string survivorId, string stepId)
        {
            bool completed = FinalWish.AdvanceWishStep(survivorId, stepId);
            LastEvent = completed
                ? $"Final wish completed by {survivorId}."
                : $"Final wish advanced for {survivorId}.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string ApplyInhaler(string survivorId)
        {
            bool ok = Respiratory.ApplyInhaler(survivorId);
            LastEvent = ok
                ? $"Inhaler applied to {survivorId}. Cough suppressed."
                : $"Inhaler refused: {survivorId} has no respiratory damage.";
            RaiseStateChanged();
            return LastEvent;
        }

        // ── Tick ──────────────────────────────────────────────────────

        /// <summary>
        /// Tick all Phase-0 systems for elapsed game hours. Called by the host on
        /// the authoritative clock (hourly progression and day advance).
        /// </summary>
        public string TickHour(float gameHours = 1f)
        {
            if (gameHours <= 0f) return "No time elapsed.";
            for (int i = 0; i < _aliveSurvivorIds.Count; i++)
            {
                var id = _aliveSurvivorIds[i];
                Phantom.TickHour(id, gameHours);
                Guilt.Tick(id, gameHours, CurrentDay);
                CombatTrauma.Tick(id, gameHours, IsNightTime);
                Flashbacks.Tick(id, gameHours);
                Dependency.TickHours(id, gameHours);
                Respiratory.TickHours(id, gameHours);
                FinalWish.Tick(id, gameHours, true);
            }
            RadiationPhase.Tick(gameHours);
            RecomputeAllEffects();
            LastEvent = $"Phase-0 effects ticked {gameHours:F0}h.";
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>Daily boundary: reset per-night trauma flags and advance the sim day.</summary>
        public string TickDay(int day)
        {
            CurrentDay = day;
            CombatTrauma.ResetNightFlags();
            string msg = TickHour(24f);
            LastEvent = $"Day {day} Phase-0 pass complete.";
            RaiseStateChanged();
            return LastEvent;
        }

        // ── Status ─────────────────────────────────────────────────────

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("PHASE-0 — PSYCHOLOGICAL & MEDICAL EFFECTS\n");
            sb.Append("Permanent shelter morale buff: ").Append(PermanentShelterMoraleBuff.ToString("F0")).Append('\n');
            for (int i = 0; i < _effects.Count; i++)
            {
                var fx = _effects[i];
                if (fx == null) continue;
                sb.Append(fx.survivorId)
                  .Append(": work ×").Append(fx.workEfficiencyMultiplier.ToString("F2"))
                  .Append(" · refusal ").Append(fx.workRefusalHours.ToString("F1")).Append("h")
                  .Append(" · stamina ×").Append(fx.staminaMultiplier.ToString("F2"))
                  .Append(" · rad ").Append(fx.radiationPhase)
                  .Append(" · guilt ").Append(fx.guiltInsomniaSeverity.ToString("F2"))
                  .Append(" · hyper ").Append(fx.hypervigilance.ToString("F2"))
                  .Append(" · branch ").Append(fx.moralBranch)
                  .Append(IsInAshZone ? " · ASH ZONE" : "")
                  .Append('\n');
            }
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ────────────────────────────────────────────────

        public Phase0EffectsSaveState CaptureSave()
        {
            var save = new Phase0EffectsSaveState
            {
                radiationPhase = RadiationPhase.CaptureState(),
                phantom = Phantom.CaptureState(),
                guilt = Guilt.CaptureState(),
                combatTrauma = CombatTrauma.CaptureState(),
                flashbacks = Flashbacks.CaptureState(),
                moral = Moral.CaptureState(),
                tradeSpecialty = TradeSpecialty.CaptureState(),
                finalWishes = FinalWish.CaptureState(),
                respiratory = Respiratory.CaptureState(),
                permanentShelterMoraleBuff = PermanentShelterMoraleBuff
            };
            for (int i = 0; i < _effects.Count; i++)
            {
                var e = _effects[i];
                if (e == null) continue;
                save.effects.Add(new Phase0SurvivorEffects
                {
                    survivorId = e.survivorId,
                    workEfficiencyMultiplier = e.workEfficiencyMultiplier,
                    workRefusalHours = e.workRefusalHours,
                    staminaMultiplier = e.staminaMultiplier,
                    guiltInsomniaSeverity = e.guiltInsomniaSeverity,
                    hypervigilance = e.hypervigilance,
                    moralBranch = e.moralBranch,
                    radiationPhase = e.radiationPhase,
                    dependencyCraftingPenalty = e.dependencyCraftingPenalty,
                    dependencyCombatPenalty = e.dependencyCombatPenalty,
                    finalWishState = e.finalWishState
                });
            }
            return save;
        }

        public void RestoreSave(Phase0EffectsSaveState save)
        {
            if (save == null) return;

            // RadiationPhase.RestoreState only patches ALREADY-registered survivors,
            // so register the saved phase states first, then restore into them.
            _phaseStates.Clear();
            if (save.radiationPhase != null && save.radiationPhase.survivors != null)
            {
                for (int i = 0; i < save.radiationPhase.survivors.Count; i++)
                {
                    var s = save.radiationPhase.survivors[i];
                    if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                    GetOrCreatePhaseState(s.survivorId);
                    if (!_aliveSurvivorIds.Contains(s.survivorId))
                        _aliveSurvivorIds.Add(s.survivorId);
                }
            }
            RadiationPhase.RestoreState(save!.radiationPhase!);

            Phantom.RestoreState(save!.phantom!);
            Guilt.RestoreState(save!.guilt!);
            CombatTrauma.RestoreState(save!.combatTrauma!);
            Flashbacks.RestoreState(save.flashbacks);
            Moral.RestoreState(save.moral);
            TradeSpecialty.RestoreState(save.tradeSpecialty);
            FinalWish.RestoreState(save.finalWishes);
            Respiratory.RestoreState(save.respiratory);
            PermanentShelterMoraleBuff = save.permanentShelterMoraleBuff;

            // Rebuild moral state maps from restored data.
            _moralStates.Clear();
            foreach (var sv in Moral.CaptureState().Survivors)
                _moralStates[sv.SurvivorId] = sv;

            _effects.Clear();
            if (save.effects != null)
            {
                for (int i = 0; i < save.effects.Count; i++)
                {
                    var e = save.effects[i];
                    if (e == null || string.IsNullOrEmpty(e.survivorId)) continue;
                    _effects.Add(new Phase0SurvivorEffects
                    {
                        survivorId = e.survivorId,
                        workEfficiencyMultiplier = e.workEfficiencyMultiplier,
                        workRefusalHours = e.workRefusalHours,
                        staminaMultiplier = e.staminaMultiplier,
                        guiltInsomniaSeverity = e.guiltInsomniaSeverity,
                        hypervigilance = e.hypervigilance,
                        moralBranch = e.moralBranch,
                        radiationPhase = e.radiationPhase,
                        dependencyCraftingPenalty = e.dependencyCraftingPenalty,
                        dependencyCombatPenalty = e.dependencyCombatPenalty,
                        finalWishState = e.finalWishState
                    });
                    if (!_aliveSurvivorIds.Contains(e.survivorId))
                        _aliveSurvivorIds.Add(e.survivorId);
                }
            }
            RaiseStateChanged();
        }

        // ── Helpers ────────────────────────────────────────────────────

        private Phase0SurvivorEffects fx(string survivorId) => GetOrCreateEffects(survivorId);

        /// <summary>
        /// Derive the host view of one survivor from the Core systems. The
        /// aggregate fields are a pure function of Core state — never written
        /// directly by event handlers.
        /// </summary>
        private void RecomputeSurvivorEffects(string survivorId)
        {
            var fx = GetOrCreateEffects(survivorId);
            // Composition: a phantom motivation boost multiplies the flashback
            // penalty factor (e.g. 1.20 × 0.40 = 0.48 effective).
            fx.workEfficiencyMultiplier =
                Phantom.GetWorkEfficiencyMultiplier(survivorId)
                * (1f - Flashbacks.GetWorkEfficiencyPenalty(survivorId));
            fx.workRefusalHours = Phantom.GetWorkRefusalHours(survivorId);
            fx.staminaMultiplier = Respiratory.GetStaminaMultiplier(survivorId);
            fx.guiltInsomniaSeverity = Guilt.GetInsomniaSeverity(survivorId);
            fx.hypervigilance = CombatTrauma.GetHypervigilanceLevel(survivorId);
            var moral = _moralStates.TryGetValue(survivorId, out var m) ? m : null;
            fx.moralBranch = moral != null ? moral.BranchDirection.ToString() : "Neutral";
            var phase = _phaseStates.TryGetValue(survivorId, out var p) ? p : null;
            fx.radiationPhase = phase != null ? phase.Phase.ToString() : "Healthy";
            fx.dependencyCraftingPenalty = Dependency.HasActiveWithdrawal(survivorId)
                ? ChemicalDependencySystem.ColdTurkeyTremorCraftingPenalty : 0f;
            fx.dependencyCombatPenalty = Dependency.HasActiveWithdrawal(survivorId)
                ? ChemicalDependencySystem.ColdTurkeyTremorCombatPenalty : 0f;
            fx.finalWishState = FinalWish.HasActiveWish(survivorId) ? "active"
                : FinalWish.HasCompletedWish(survivorId) ? "completed"
                : FinalWish.HasTerminalPrognosis(survivorId) ? "failed" : string.Empty;
        }

        private void RecomputeAllEffects()
        {
            for (int i = 0; i < _aliveSurvivorIds.Count; i++)
                RecomputeSurvivorEffects(_aliveSurvivorIds[i]);
        }

        private Phase0SurvivorEffects GetOrCreateEffects(string survivorId)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                var e = _effects[i];
                if (e != null && e.survivorId == survivorId) return e;
            }
            var fx = new Phase0SurvivorEffects { survivorId = survivorId };
            _effects.Add(fx);
            return fx;
        }

        private PhaseProgressionState GetOrCreatePhaseState(string survivorId)
        {
            if (!_phaseStates.TryGetValue(survivorId, out var state))
            {
                state = new PhaseProgressionState { Id = survivorId, IsAlive = true };
                _phaseStates[survivorId] = state;
                RadiationPhase.Register(state);
            }
            return state;
        }

        private MoralBranchState GetOrCreateMoralState(string survivorId)
        {
            if (!_moralStates.TryGetValue(survivorId, out var state))
            {
                state = new MoralBranchState { SurvivorId = survivorId, IsAlive = true };
                _moralStates[survivorId] = state;
                Moral.Register(state);
            }
            return state;
        }

        private static string InferBackground(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return "generic";
            if (survivorId.Contains("gunner") || survivorId.Contains("soldier")) return "former_soldier";
            if (survivorId.Contains("sarah") || survivorId.Contains("nurse")) return "nurse";
            if (survivorId.Contains("teacher")) return "teacher";
            return "generic";
        }

        /// <summary>Deterministic ISeededRng adapter delegating to the core SeededRng.</summary>
        private sealed class CoreSeededRng : ISeededRng
        {
            private readonly SeededRng _rng;
            public int Seed { get; }
            public CoreSeededRng(int seed) { Seed = seed; _rng = new SeededRng(seed); }
            public int Next(int min, int max) => _rng.Next(min, max);
            public float NextFloat() => _rng.NextFloat();
            public double NextDouble() => _rng.NextDouble();
        }

        public void BindShelterAssignment(ShelterAssignmentSystem shelterAssignment)
        {
            if (shelterAssignment == null)
            {
                Flashbacks.IsCompanionInSameRoom = (a, b) => false;
                return;
            }

            Flashbacks.IsCompanionInSameRoom = shelterAssignment.AreInSameRoom;
            shelterAssignment.OnAssignmentChanged += ev =>
            {
                LastEvent = $"[Phase0] Shelter assignment changed: {ev?.SurvivorId ?? "unknown"} -> {ev?.RoomId ?? "unknown"}";
                RaiseStateChanged();
            };
        }

        protected override void UnsubscribeSystemEvents()
        {
            RadiationPhase.OnStateChanged -= _onRadiationPhaseStateChanged;
            Phantom.OnStateChanged -= _onPhantomStateChanged;
            Guilt.OnStateChanged -= _onGuiltStateChanged;
            CombatTrauma.OnStateChanged -= _onCombatTraumaStateChanged;
            Flashbacks.OnStateChanged -= _onFlashbacksStateChanged;
            Moral.OnStateChanged -= _onMoralStateChanged;
            Dependency.OnStateChanged -= _onDependencyStateChanged;
            TradeSpecialty.OnStateChanged -= _onTradeSpecialtyStateChanged;
            FinalWish.OnStateChanged -= _onFinalWishStateChanged;
            Respiratory.OnStateChanged -= _onRespiratoryStateChanged;
        }
    }
}
