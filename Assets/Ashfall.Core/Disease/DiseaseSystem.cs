using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Disease
{
    // ---------------------------------------------------------------------
    // Identity (exact ids — never aliased)
    // ---------------------------------------------------------------------

    /// <summary>Exact ids of the Disease Expansion. Data authority: disease_catalog.json.</summary>
    public static class DiseaseIds
    {
        public const string ExpansionId = "expansion_disease_expansion";
        public const string CatalogCollectionId = DiseaseCatalog.CollectionId;

        // Authored disease ids (disease_catalog.json).
        public const string Cholera = "disease_cholera";
        public const string ZoonoticFlu = "disease_zoonotic_flu";
        public const string BloodFever = "disease_blood_fever";
        public const string SporeBlight = "disease_spore_blight";

        // Event-id strings forwarded on the string bus (typed events are the
        // primary surface; hosts may forward these).
        public const string EventInfection = "disease_infection";
        public const string EventQuarantineStarted = "disease_quarantine_started";
        public const string EventQuarantineEnded = "disease_quarantine_ended";
        public const string EventOutbreakDeclared = "disease_outbreak_declared";
        public const string EventOutbreakContained = "disease_outbreak_contained";
        public const string EventRecovered = "disease_recovered";
        public const string EventDied = "disease_death";
        public const string EventProtocolApplied = "disease_protocol_applied";
        public const string EventProtocolReset = "disease_protocol_reset";

        /// <summary>Plan 60 / D3 — a treatment was applied to a patient.</summary>
        public const string EventTreatmentApplied = "disease_treatment_applied";
    }

    // ---------------------------------------------------------------------
    // History / patient records (mutable, serialized)
    // ---------------------------------------------------------------------

    /// <summary>One active infection of one survivor with one disease.</summary>
    [Serializable]
    public sealed class DiseaseInfectionState
    {
        public string survivor_id = string.Empty;
        public int infected_day = 0;
        public int days_sick = 0;
        public bool quarantined = false;

        /// <summary>Plan 63 / B4 — current clinical stage of this infection.</summary>
        public string current_stage = DiseaseStageNames.Incubating;

        /// <summary>Day the infection entered its current clinical stage.</summary>
        public int stage_entered_day = 0;

        /// <summary>
        /// Plan 60 / D3 — doses of authorised treatment this patient has received.
        /// Additive: a pre-D3 save loads as 0, i.e. "never treated", the truth.
        /// </summary>
        public int treatments_applied = 0;

        /// <summary>
        /// Accumulated, capped reduction of this disease's lethality <em>for this
        /// patient</em>. Kept per patient, never on the disease, so treating one case
        /// cannot silently protect another.
        /// </summary>
        public float lethality_reduction = 0f;

        /// <summary>Day of the last accepted treatment (-1 = none).</summary>
        public int last_treatment_day = -1;
    }

    /// <summary>
    /// Plan 63 / B4 — temporary immunity window acquired upon recovery.
    /// </summary>
    [Serializable]
    public sealed class DiseaseImmunityRecord
    {
        public string survivor_id = string.Empty;
        public string disease_id = string.Empty;
        public int immunity_until_day = 0;
        public float strength = 1.0f;
    }

    /// <summary>
    /// Plan 63 / B4 — typed exposure context for all disease ingestion and environmental bridges.
    /// </summary>
    public sealed class DiseaseExposureContext
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string DiseaseId { get; set; } = string.Empty;
        public string SourceId { get; set; } = string.Empty;
        public float ProbabilityModifier { get; set; } = 1.0f;
        public bool BypassImmunity { get; set; } = false;
        public int Day { get; set; } = 0;
    }

    /// <summary>
    /// Plan 63 / B4 — typed exposure evaluation result.
    /// </summary>
    public sealed class DiseaseExposureResult
    {
        public bool Infected { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string DiseaseId { get; set; } = string.Empty;
        public float EffectiveProbability { get; set; }

        public static DiseaseExposureResult CreateInfected(string survivorId, string diseaseId, float prob) =>
            new DiseaseExposureResult { Infected = true, Reason = "infected", SurvivorId = survivorId, DiseaseId = diseaseId, EffectiveProbability = prob };

        public static DiseaseExposureResult CreateBlocked(string reason, string survivorId, string diseaseId, float prob = 0f) =>
            new DiseaseExposureResult { Infected = false, Reason = reason, SurvivorId = survivorId, DiseaseId = diseaseId, EffectiveProbability = prob };
    }

    /// <summary>
    /// Plan 60 / D3 — outcome of a treatment attempt. A refusal is a named, player-facing
    /// fact ("outside the window", "no doses left"), never a silent no-op.
    /// </summary>
    public sealed class DiseaseTreatmentResult
    {
        public bool Accepted { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public string DiseaseId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public float LethalityReduction { get; set; }
        public bool Cured { get; set; }

        public static DiseaseTreatmentResult Refuse(string reason, string itemId, string diseaseId, string survivorId) =>
            new DiseaseTreatmentResult
            {
                Accepted = false,
                Reason = reason,
                ItemId = itemId ?? string.Empty,
                DiseaseId = diseaseId ?? string.Empty,
                SurvivorId = survivorId ?? string.Empty,
            };
    }

    /// <summary>Named refusal reasons for <see cref="DiseaseSystem.TryTreat"/>.</summary>
    public static class DiseaseTreatmentRefusals
    {
        public const string NotPatient = "not_patient";
        public const string UnknownDisease = "unknown_disease";
        public const string NoTreatmentAuthorised = "no_treatment_authorised";
        public const string ItemNotAuthorised = "item_not_authorised";
        public const string OutsideWindow = "outside_window";
        public const string AlreadyTreatedToday = "already_treated_today";
        public const string NoSupplyChannel = "no_supply_channel";
        public const string SupplyUnavailable = "supply_unavailable";
    }

    /// <summary>Per-disease mutable simulation snapshot (serialized).</summary>
    [Serializable]
    public sealed class DiseaseEntryState
    {
        public string disease_id = string.Empty;
        public string vector_type = DiseaseVectorNames.Water;
        public float spread_timer = 0f;
        public bool outbreak_active = false;

        public int deaths_during_outbreak = 0;

        // Cumulative history.
        public int outbreaks_total = 0;
        public int outbreaks_prevented = 0;
        public int recovered_total = 0;
        public int deaths_total = 0;
        public int infections_total = 0;

        public List<DiseaseInfectionState> infected = new List<DiseaseInfectionState>();
    }

    // ---------------------------------------------------------------------
    // Save DTO (versioned, plain public fields, no host objects)
    // ---------------------------------------------------------------------

    /// <summary>
    /// Cross-host save state of the Disease Expansion. Rides the expansion-hub
    /// save envelope (ExpansionHubSave v4). Same byte shape on both hosts via
    /// the IJsonSerializer port.
    /// </summary>
    [Serializable]
    public sealed class DiseaseSystemState
    {
        public const int CurrentVersion = 2;

        public int stateVersion = CurrentVersion;
        public string system_id = DiseaseIds.ExpansionId;

        // Protocol flags (vector countermeasures).
        public bool water_purified = false;
        public bool vents_sealed = false;
        public bool tools_sterilized = false;
        public bool air_filtration = false;

        // Plan 60 / D4 — the day each protocol lapses (day >= until ⇒ expired).
        // 0 means "not armed": either the protocol is off, or a pre-D4 save had it
        // on with no recorded expiry (that save re-arms on the next tick and
        // lapses one duration later, which is the honest reading of an old save).
        public int water_purified_until_day = 0;
        public int vents_sealed_until_day = 0;
        public int tools_sterilized_until_day = 0;
        public int air_filtration_until_day = 0;

        // Preserved across ticks / restores so a reload reproduces the same
        // outcome sequence (determinism invariant).
        public int rngSeed = 0;

        public List<DiseaseEntryState> diseases = new List<DiseaseEntryState>();

        /// <summary>Plan 63 / B4 — temporary immunity records per survivor and disease.</summary>
        public List<DiseaseImmunityRecord> immunities = new List<DiseaseImmunityRecord>();
    }

    // ---------------------------------------------------------------------
    // UI snapshot payloads (derived, never persisted)
    // ---------------------------------------------------------------------

    /// <summary>One contagious/sick survivor shown on the outbreak ward.</summary>
    public sealed class DiseasePatientSnapshot
    {
        public string survivor_id = string.Empty;
        public string disease_id = string.Empty;
        public string disease_name = string.Empty;
        public int days_sick = 0;
        public bool quarantined = false;
        public bool contagious = false;          // past incubation, not isolated
        public int contagion_risk_percent = 0;   // infectivity * 100

        /// <summary>Doses of authorised treatment received (Plan 60 / D3).</summary>
        public int treatments_applied = 0;

        /// <summary>
        /// Lethality actually in force for this patient after treatment. The surface
        /// reads this rather than re-deriving it.
        /// </summary>
        public float effective_lethality = 0f;

        /// <summary>Plan 63 / B4 — 8-stage clinical progression arc stage.</summary>
        public string current_stage = DiseaseStageNames.Incubating;
        public string stage_token = "incubating";
    }

    /// <summary>Derived read model for the ward / HUD. Always rebuilt on demand.</summary>
    public sealed class DiseaseSnapshot
    {
        public int total_infected = 0;
        public int total_quarantined = 0;
        public int total_contagious = 0;
        public int total_outbreaks = 0;
        public int total_outbreaks_prevented = 0;
        public int total_recovered = 0;
        public int total_deaths = 0;
        public List<DiseasePatientSnapshot> patients = new List<DiseasePatientSnapshot>();
    }

    // ---------------------------------------------------------------------
    // The system
    // ---------------------------------------------------------------------

    /// <summary>
    /// DISEASE EXPANSION — epidemic contagion, quarantine protocols, waterborne
    /// pathogens and contagious spore vectors (migrated from the legacy Unity
    /// DiseaseSystem_Expansion.cs, Prompt #834).
    ///
    /// A disease spreads through one authored vector (water / air / blood /
    /// spore). Each vector is neutralised by a protocol: purified water, sealed
    /// vents, sterilised surgical tools, sealed/filtered air. Quarantine arrests
    /// a contagious patient's ability to spread; an outbreak is declared at
    /// three active infections and counts as *prevented* only when it is
    /// contained without a death. The host drives the candidate pool and item
    /// costs — this owns the rules, deterministically, through the injected
    /// ISeededRng. Same seed ⇒ same outbreak. No System.Random, no GUIDs.
    /// </summary>
    public sealed class DiseaseSystem
    {
        public const int DefaultSeed = 1013;
        public const int OutbreakThreshold = 3;

        /// <summary>
        /// Plan 60 / D3 — treatment can improve a patient's odds, never make them
        /// immortal: cumulative lethality reduction is capped, so no accumulation of
        /// doses turns a lethal disease into a guaranteed survival.
        /// </summary>
        public const float MaxLethalityReduction = 0.9f;

        // Typed events (established convention — no third bus).
        public event Action<string, string> OnInfection;                    // survivorId, diseaseId
        public event Action<string, string> OnQuarantineStarted;            // survivorId, diseaseId
        public event Action<string, string> OnQuarantineEnded;              // survivorId, diseaseId
        public event Action<string> OnOutbreakDeclared;                     // diseaseId
        public event Action<string, bool> OnOutbreakContained;              // diseaseId, prevented
        public event Action<string, string, bool> OnOutcomeResolved;        // survivorId, diseaseId, recovered

        /// <summary>
        /// Plan 60 / D3 — survivorId, diseaseId, itemId, role, day. Raised only for an
        /// <em>accepted</em> treatment, so a listener never sees a refused attempt.
        /// </summary>
        public event Action<string, string, string, string, int>? OnTreatmentApplied;
        public event Action<DiseaseSystemState> OnStateChanged;
        /// <summary>Forwarder for the string event bus (optional).</summary>
        public event Action<string, string> OnEventRaised;                  // eventId, detail

        /// <summary>
        /// Flagship XI (Plan 155) — optional abstract severity coupling:
        /// (survivorId, diseaseId) → additive lethality delta applied in
        /// ResolveOutcomes. Null (default) leaves outcome math untouched. The
        /// pathogen strain system supplies this from the canonical radiation
        /// query; the engine never reads radiation itself.
        /// </summary>
        public Func<string, string, float>? EffectiveLethalityModifier;

        /// <summary>Raised when an active infection transitions between strains (Plan 155.10): survivorId, newStrainId.</summary>
        public event Action<string, string>? OnStrainMutated;

        /// <summary>
        /// Plan 63 / B4 — optional isolation quality provider: survivorId -> isolationQuality01.
        /// When null, defaults to 1.0f for isolated patients.
        /// </summary>
        public Func<string, float>? GetIsolationQuality;

        /// <summary>
        /// Plan 63 / B4 — optional containment capability projected from research.
        /// </summary>
        public ContainmentCapability Containment { get; set; } = ContainmentCapability.None;

        private readonly DiseaseSystemState _state;
        private readonly List<DiseaseEntryState> _entries = new List<DiseaseEntryState>();
        private readonly Dictionary<string, DiseaseEntryState> _byId =
            new Dictionary<string, DiseaseEntryState>(StringComparer.Ordinal);

        private DiseaseCatalog _catalog = new DiseaseCatalog();
        private ISeededRng _rng;
        private readonly Func<int, ISeededRng> _rngFactory;
        private readonly ILog _log;

        public DiseaseSystem(
DiseaseSystemState? state = null,
ISeededRng? rng = null,
Func<int, ISeededRng>? rngFactory = null,
ILog? log = null)
        {
            _rngFactory = rngFactory ?? (seed => new SeededRng(seed));
            _state = state ?? new DiseaseSystemState();
            _rng = rng ?? _rngFactory(_state.rngSeed == 0 ? DefaultSeed : _state.rngSeed);
            _state.rngSeed = _rng.Seed;
            _log = log ?? NullLog.Instance;
            RebuildIndexFromState();
        }

        public DiseaseSystemState State => _state;
        public DiseaseCatalog Catalog => _catalog;
        public string SystemId => _state.system_id;

        // -----------------------------------------------------------------
        // Immunity & Exposure APIs (Plan 63 / B4)
        // -----------------------------------------------------------------

        public bool HasImmunity(string survivorId, string diseaseId, int currentDay)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId) || _state.immunities == null)
                return false;
            for (int i = 0; i < _state.immunities.Count; i++)
            {
                var imm = _state.immunities[i];
                if (imm != null && string.Equals(imm.survivor_id, survivorId, StringComparison.Ordinal)
                    && string.Equals(imm.disease_id, diseaseId, StringComparison.Ordinal)
                    && currentDay < imm.immunity_until_day)
                {
                    return true;
                }
            }
            return false;
        }

        public DiseaseImmunityRecord? GetImmunity(string survivorId, string diseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId) || _state.immunities == null)
                return null;
            for (int i = 0; i < _state.immunities.Count; i++)
            {
                var imm = _state.immunities[i];
                if (imm != null && string.Equals(imm.survivor_id, survivorId, StringComparison.Ordinal)
                    && string.Equals(imm.disease_id, diseaseId, StringComparison.Ordinal))
                    return imm;
            }
            return null;
        }

        public void SetImmunity(string survivorId, string diseaseId, int untilDay, float strength = 1.0f)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return;
            if (_state.immunities == null) _state.immunities = new List<DiseaseImmunityRecord>();
            for (int i = 0; i < _state.immunities.Count; i++)
            {
                var imm = _state.immunities[i];
                if (imm != null && string.Equals(imm.survivor_id, survivorId, StringComparison.Ordinal)
                    && string.Equals(imm.disease_id, diseaseId, StringComparison.Ordinal))
                {
                    imm.immunity_until_day = untilDay;
                    imm.strength = strength;
                    return;
                }
            }
            _state.immunities.Add(new DiseaseImmunityRecord
            {
                survivor_id = survivorId,
                disease_id = diseaseId,
                immunity_until_day = untilDay,
                strength = strength
            });
        }

        public DiseaseExposureResult TryExpose(DiseaseExposureContext context)
        {
            if (context == null || string.IsNullOrEmpty(context.SurvivorId) || string.IsNullOrEmpty(context.DiseaseId))
                return DiseaseExposureResult.CreateBlocked("invalid_arguments", context?.SurvivorId ?? "", context?.DiseaseId ?? "");

            if (!_byId.TryGetValue(context.DiseaseId, out var entry))
                return DiseaseExposureResult.CreateBlocked("unknown_disease", context.SurvivorId, context.DiseaseId);

            var def = _catalog.GetById(context.DiseaseId);
            if (def == null)
                return DiseaseExposureResult.CreateBlocked("unknown_disease", context.SurvivorId, context.DiseaseId);

            // 1. Check if already infected
            if (ContainsInfection(entry, context.SurvivorId))
                return DiseaseExposureResult.CreateBlocked("already_infected", context.SurvivorId, context.DiseaseId);

            // 2. Check vector countermeasure
            if (IsVectorBlocked(entry.vector_type))
                return DiseaseExposureResult.CreateBlocked("vector_blocked", context.SurvivorId, context.DiseaseId);

            // 3. Check temporary immunity
            if (!context.BypassImmunity && HasImmunity(context.SurvivorId, context.DiseaseId, context.Day))
            {
                var imm = GetImmunity(context.SurvivorId, context.DiseaseId);
                float immStrength = imm?.strength ?? 1.0f;
                if (_rng.NextDouble() < immStrength)
                    return DiseaseExposureResult.CreateBlocked("immune", context.SurvivorId, context.DiseaseId);
            }

            // 4. Calculate effective probability
            float prob = def.infectivity;
            if (!string.IsNullOrEmpty(context.SourceId))
            {
                var src = _catalog.GetExposureSource(context.SourceId);
                if (src != null) prob = src.base_probability;
            }
            prob = Math.Min(1.0f, Math.Max(0f, prob * Math.Max(0f, context.ProbabilityModifier)));

            // 5. Roll exposure
            if (_rng.NextDouble() < prob)
            {
                Infect(context.SurvivorId, context.DiseaseId, context.Day);
                return DiseaseExposureResult.CreateInfected(context.SurvivorId, context.DiseaseId, prob);
            }

            return DiseaseExposureResult.CreateBlocked("roll_passed", context.SurvivorId, context.DiseaseId, prob);
        }

        public DiseaseExposureResult TryInfect(string survivorId, string diseaseId, int day, string? sourceId = null)
        {
            return TryExpose(new DiseaseExposureContext
            {
                SurvivorId = survivorId,
                DiseaseId = diseaseId,
                Day = day,
                SourceId = sourceId ?? string.Empty,
                ProbabilityModifier = 1.0f
            });
        }

        private void RebuildIndexFromState()
        {
            _entries.Clear();
            _byId.Clear();
            if (_state.diseases == null) _state.diseases = new List<DiseaseEntryState>();
            bool changed = false;
            for (int i = 0; i < _state.diseases.Count; i++)
            {
                var e = _state.diseases[i];
                if (e == null || string.IsNullOrEmpty(e.disease_id)) continue;
                if (e.infected == null) { e.infected = new List<DiseaseInfectionState>(); changed = true; }
                if (_byId.ContainsKey(e.disease_id)) continue; // duplicate rows: keep first
                _entries.Add(e);
                _byId[e.disease_id] = e;
            }
            if (changed) _state.diseases = new List<DiseaseEntryState>(_entries);
        }

        // -----------------------------------------------------------------
        // Binding
        // -----------------------------------------------------------------

        /// <summary>Bind the static disease catalog. Idempotent; never mutates it.</summary>
        public void BindCatalog(DiseaseCatalog catalog)
        {
            if (catalog == null) return;
            _catalog = catalog;
            if (catalog.HasErrors)
            {
                for (int i = 0; i < catalog.Errors.Count; i++)
                    _log.Warn("[Disease] catalog: " + catalog.Errors[i]);
            }
            // Register every authored disease as a simulation row (idempotent —
            // a restored save that already carries the disease keeps its state).
            for (int i = 0; i < catalog.Diseases.Count; i++)
            {
                var d = catalog.Diseases[i];
                if (d == null || string.IsNullOrEmpty(d.id)) continue;
                EnsureEntry(d.id, d.vector);
            }
        }

        /// <summary>Definition lookup through the bound catalog (null when unknown).</summary>
        public DiseaseDefinition? GetDefinition(string diseaseId)
        {
            return string.IsNullOrEmpty(diseaseId) ? null : _catalog.GetById(diseaseId);
        }

        /// <summary>
        /// Flagship XI (Plan 155) — register a fictional strain as a first-class
        /// simulation row: catalog definition + engine entry, idempotent. The
        /// strain layer stays the authoring authority; the engine runs strains
        /// exactly like authored diseases (spread, quarantine, treatment, save).
        /// </summary>
        public bool RegisterStrain(DiseaseDefinition strainDefinition)
        {
            if (strainDefinition == null || string.IsNullOrEmpty(strainDefinition.id)) return false;
            if (_catalog.GetById(strainDefinition.id) != null)
            {
                EnsureEntry(strainDefinition.id, strainDefinition.vector); // restore path: state without definition
                return true;
            }
            _catalog.Add(strainDefinition);
            EnsureEntry(strainDefinition.id, strainDefinition.vector);
            return true;
        }

        /// <summary>Ensure a simulation row exists for a disease id.</summary>
        private DiseaseEntryState EnsureEntry(string diseaseId, string vectorType)
        {
            if (_byId.TryGetValue(diseaseId, out var e)) return e;
            e = new DiseaseEntryState
            {
                disease_id = diseaseId,
                vector_type = string.IsNullOrEmpty(vectorType)
                    ? DiseaseVectorNames.Water : vectorType
            };
            _entries.Add(e);
            _byId[diseaseId] = e;
            _state.diseases.Add(e);
            return e;
        }

        // -----------------------------------------------------------------
        // Simulation
        // -----------------------------------------------------------------

        /// <summary>
        /// Infect a survivor with a registered disease. No-op when already
        /// infected with it. Declares an outbreak at the threshold (3 active
        /// infections, matching the legacy behaviour).
        /// </summary>
        public void Infect(string survivorId, string diseaseId, int day)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return;
            if (!_byId.TryGetValue(diseaseId, out var entry))
            {
                _log.Warn("[Disease] unknown disease '" + diseaseId + "' rejected.");
                return;
            }

            for (int i = 0; i < entry.infected.Count; i++)
            {
                if (string.Equals(entry.infected[i].survivor_id, survivorId, StringComparison.Ordinal))
                    return; // already infected
            }

            entry.infected.Add(new DiseaseInfectionState
            {
                survivor_id = survivorId,
                infected_day = day,
                days_sick = 0,
                current_stage = DiseaseStageNames.Incubating,
                stage_entered_day = day
            });
            entry.infections_total++;

            Raise(OnInfection, DiseaseIds.EventInfection, survivorId + " infected with " + diseaseId, survivorId, diseaseId);

            if (!entry.outbreak_active && entry.infected.Count >= OutbreakThreshold)
            {
                entry.outbreak_active = true;
                entry.outbreaks_total++;
                entry.deaths_during_outbreak = 0;
                Raise(OnOutbreakDeclared, DiseaseIds.EventOutbreakDeclared, "outbreak declared: " + diseaseId, diseaseId);
            }

            RaiseStateChanged();
        }

        // // ── World-trigger arrival (Plan 09 9A follow‑up) ──────────────

        /// <summary>Forwarded by trigger sources so telemetry can audit
        /// which world event seeded which disease.</summary>
        public event Action<string, string, string, int>? OnOutbreakTriggered;
            // (diseaseId, sourceId, reason, infectionsApplied)

        /// <summary>
        /// Seed a disease through an <see cref="IDiseaseOutbreakSource"/>
        /// <em>if and only if</em> the source's contract lists the disease
        /// and the catalog knows the id. Returns an envelope that tells the
        /// caller what landed — useful so the host can decide whether to
        /// surface a broadcast or a healer's note without re-querying the
        /// system.
        ///
        /// Candidate selection: on a true trigger, take up to one survivor
        /// from the candidate pool and <see cref="Infect(string,string,int)"/>
        /// them. The seed is single-target by design (the disease's natural
        /// spread picks up the rest). Deterministic given the seeded RNG
        /// and candidate order.
        /// </summary>
        public DiseaseOutbreakResult TriggerOutbreak(
            IDiseaseOutbreakSource source,
            string diseaseId,
            int day,
            IReadOnlyList<string>? candidates = null)
        {
            if (source == null || string.IsNullOrEmpty(diseaseId))
                return DiseaseOutbreakResult.Empty;

            DiseaseOutbreakResult result;
            // The catalog check runs BEFORE the contract check so a host
            // adapter that names a misspelled disease id hears "unknown"
            // rather than "wrong contract" — same call enters the system
            // either way, but the reason tells the operator what drifted.
            if (_catalog.GetById(diseaseId) == null)
            {
                _log.Warn($"[Disease] trigger from '{source.SourceId}' rejected — " +
                          $"unknown disease '{diseaseId}'");
                result = new DiseaseOutbreakResult(0, 0, 1, 0);
            }
            else
            {
                // Contract check — protects against host adapters accidentally
                // seeding an unrelated disease.
                bool contracted = false;
                for (int i = 0; i < source.AuthoredDiseaseIds.Count; i++)
                {
                    if (string.Equals(source.AuthoredDiseaseIds[i], diseaseId, StringComparison.Ordinal))
                    {
                        contracted = true;
                        break;
                    }
                }
                if (!contracted)
                {
                    result = new DiseaseOutbreakResult(0, 1, 0, 0);
                }
                else if (candidates == null || candidates.Count == 0)
                {
                    // The trigger fired but no survivor is available — we do NOT
                    // queue the outbreak; the host re-fires on a later day if
                    // the source remains (a sump flood that hasn't cleared the
                    // roster shouldn't silently re-arm).
                    result = new DiseaseOutbreakResult(0, 0, 0, 1);
                }
                else
                {
                    // Pick a single survivor deterministically. The system's
                    // seeded RNG is the only random source, so re-running this
                    // trigger after Restore produces the same pick.
                    int pick = _rng.Next(0, candidates.Count);
                    string survivorId = candidates[pick];
                    Infect(survivorId, diseaseId, day);
                    result = new DiseaseOutbreakResult(1, 0, 0, 0);
                }
            }

            OnOutbreakTriggered?.Invoke(
                diseaseId,
                source.SourceId ?? string.Empty,
                result.InfectionsApplied > 0 ? "applied" :
                result.RejectedByContract > 0 ? "rejected_by_contract" :
                result.UnknownDisease > 0 ? "unknown_disease" :
                result.NoCandidates > 0 ? "no_candidates" : "noop",
                result.InfectionsApplied);
            return result;
        }

        // // ── Spread ─────────────

        /// <summary>
        /// Advance one simulation day. Spreads each disease through its vector
        /// against the provided candidate pool (the host's live roster), then
        /// resolves each patient's outcome (recovery / death) after their
        /// illness days. Deterministic given the seeded RNG and candidate order.
        /// </summary>
        /// <param name="day">Current in-game day.</param>
        /// <param name="candidates">
        /// Survivor ids eligible for exposure this day (non-infected, non-
        /// quarantined pool supplied by the host). Null/empty: no autonomous
        /// spread — the host can still call Infect() directly (legacy hook
        /// contract preserved).
        /// </param>
        public void TickDaily(int day, IReadOnlyList<string>? candidates = null)
        {
            // Plan 60 / D4 — protocols are maintenance, not switches. A lapsed one
            // must come down even if nobody was watching, before any spread roll
            // reads the vector as still blocked.
            TickProtocolExpiry(day);

            for (int i = 0; i < _entries.Count; i++)
            {
                var entry = _entries[i];
                var def = _catalog.GetById(entry.disease_id);
                if (def == null) continue;

                // 1. Spread attempt.
                entry.spread_timer += 1f;
                if (IsVectorBlocked(entry.vector_type))
                {
                    entry.spread_timer = 0f; // protocol holds the line
                }
                else if (candidates != null && candidates.Count > 0
                         && entry.spread_timer >= SpreadInterval(def))
                {
                    TrySpread(entry, def, candidates, day);
                    entry.spread_timer = 0f;
                }

                // 2. Patient outcome progression.
                ResolveOutcomes(entry, def, day);
                MaybeContain(entry);
            }

            RaiseStateChanged();
        }

        private int SpreadInterval(DiseaseDefinition def)
        {
            return def != null && def.spread_interval_days > 0 ? def.spread_interval_days : 1;
        }

        private void TrySpread(DiseaseEntryState entry, DiseaseDefinition def,
            IReadOnlyList<string> candidates, int day)
        {
            // Public candidates: everyone not already infected with this disease,
            // not quarantined (quarantine is the isolation protocol), and without active immunity.
            var pool = new List<string>(candidates.Count);
            for (int i = 0; i < candidates.Count; i++)
            {
                string c = candidates[i];
                if (string.IsNullOrEmpty(c)) continue;
                if (ContainsInfection(entry, c)) continue;      // already infected
                if (IsQuarantinedAnywhere(c)) continue;         // isolation ward
                if (HasImmunity(c, entry.disease_id, day)) continue; // temporary immunity (Plan 63 / B4)
                if (!pool.Contains(c)) pool.Add(c);              // de-dupe, keep host order
            }
            if (pool.Count == 0 || def.infectivity <= 0f) return;

            // The first contagious (past-incubation, un-quarantined or leaking isolation) patient
            // drives this interval's spread attempt; one patient per interval.
            var targets = new List<string>();
            for (int p = 0; p < entry.infected.Count; p++)
            {
                var patient = entry.infected[p];
                if (patient == null) continue;
                if (patient.quarantined && GetIsolationQuality == null) continue; // legacy/unmanaged quarantine blocks 100%
                if (!IsPhysiologicallyContagious(patient, def)) continue; // still incubating

                float baseContagiousness = def.GetPhaseContagiousness(patient.days_sick, def.infectivity);
                float sheddingProb = baseContagiousness;

                if (patient.quarantined && GetIsolationQuality != null)
                {
                    float isolationQuality = Math.Clamp(GetIsolationQuality(patient.survivor_id), 0f, 1.0f);
                    float efficacy = Math.Min(0.95f, 0.85f + Containment.EfficacyBonus);
                    float reductionFactor = Math.Max(0.02f, 1.0f - (isolationQuality * efficacy));
                    sheddingProb *= reductionFactor;
                }

                if (_rng.NextDouble() >= sheddingProb) break;  // this patient does not shed

                int toExpose = Math.Max(1, def.spread_radius);
                int remaining = pool.Count;
                for (int k = 0; k < toExpose && remaining > 0; k++)
                {
                    int pick = _rng.Next(0, remaining);
                    string target = pool[pick];
                    // Swap-remove keeps the remaining pool compact and the RNG
                    // stream deterministic.
                    pool[pick] = pool[remaining - 1];
                    remaining--;
                    targets.Add(target);
                }
                break;
            }

            // Apply exposures after the walk (no in-place list mutation).
            for (int i = 0; i < targets.Count; i++)
                Infect(targets[i], entry.disease_id, day);
        }

        private void ResolveOutcomes(DiseaseEntryState entry, DiseaseDefinition def, int day)
        {
            if (def == null || entry.infected.Count == 0) return;

            // Collected removals applied after the walk (no in-place mutation).
            var removed = new List<DiseaseInfectionState>();
            for (int i = 0; i < entry.infected.Count; i++)
            {
                var patient = entry.infected[i];
                if (patient == null) continue;
                patient.days_sick++;

                // Plan 63 / B4: advance clinical stage
                var currentStage = def.GetStage(patient.days_sick);
                string stageName = currentStage.ToString();
                if (!string.Equals(patient.current_stage, stageName, StringComparison.Ordinal))
                {
                    patient.current_stage = stageName;
                    patient.stage_entered_day = day;
                }

                if (patient.days_sick < def.illness_days) continue;

                // Plan 60 / D3 — the roll is against what is left of the disease for
                // this patient after treatment, not the raw authored value.
                // Flagship XI (Plan 155): the optional modifier carries abstract
                // radiation-severity pressure for strain infections; null = 0.
                float modifier = EffectiveLethalityModifier?.Invoke(patient.survivor_id, entry.disease_id) ?? 0f;
                float lethal = Math.Max(0f, def.lethality + modifier - patient.lethality_reduction);
                bool died = lethal > 0f && _rng.NextDouble() < lethal;
                removed.Add(patient);
                if (died)
                {
                    patient.current_stage = DiseaseStageNames.Terminal;
                    patient.stage_entered_day = day;
                    entry.deaths_total++;
                    if (entry.outbreak_active) entry.deaths_during_outbreak++;
                    Raise(OnOutcomeResolved, DiseaseIds.EventDied,
                        patient.survivor_id + " died of " + entry.disease_id + " (day " + day + ")",
                        patient.survivor_id, entry.disease_id, false);
                }
                else
                {
                    patient.current_stage = DiseaseStageNames.Recovered;
                    patient.stage_entered_day = day;
                    entry.recovered_total++;
                    if (def.immunity_duration_days > 0)
                    {
                        SetImmunity(patient.survivor_id, entry.disease_id, day + def.immunity_duration_days, def.immunity_strength);
                    }
                    bool prevQuarantined = patient.quarantined;
                    Raise(OnOutcomeResolved, DiseaseIds.EventRecovered,
                        patient.survivor_id + " recovered from " + entry.disease_id + " (day " + day + ")",
                        patient.survivor_id, entry.disease_id, true);
                    if (prevQuarantined)
                        Raise(OnQuarantineEnded, DiseaseIds.EventQuarantineEnded,
                            patient.survivor_id + " left quarantine on recovery", patient.survivor_id, entry.disease_id);
                }
            }

            for (int i = 0; i < removed.Count; i++)
                entry.infected.Remove(removed[i]);
        }

        /// <summary>
        /// Declared outbreaks come home: when no case remains contagious (all
        /// isolated, recovered or dead) the outbreak is contained. Counted as
        /// *prevented* only when the containment cost no lives.
        /// </summary>
        private void MaybeContain(DiseaseEntryState entry)
        {
            if (!entry.outbreak_active) return;

            bool anyContagious = false;
            for (int i = 0; i < entry.infected.Count; i++)
            {
                var patient = entry.infected[i];
                if (patient == null || patient.quarantined) continue;
                var def = _catalog.GetById(entry.disease_id);
                if (IsContagious(entry!, patient, def!))
                {
                    anyContagious = true;
                    break;
                }
            }

            if (anyContagious) return;

            entry.outbreak_active = false;
            bool prevented = entry.deaths_during_outbreak == 0;
            if (prevented) entry.outbreaks_prevented++;
            Raise(OnOutbreakContained, DiseaseIds.EventOutbreakContained,
                "outbreak contained: " + entry.disease_id + (prevented ? " (no lives lost)" : ""),
                entry.disease_id, prevented);
            RaiseStateChanged();
        }

        // -----------------------------------------------------------------
        // Quarantine protocol
        // -----------------------------------------------------------------

        /// <summary>Place a survivor in the isolation ward (per disease).</summary>
        public void Quarantine(string survivorId, string diseaseId)
        {
            if (!TryFindPatient(survivorId, diseaseId, out var entry, out var patient)) return;
            if (patient!.quarantined) return;

            patient.quarantined = true;
            Raise(OnQuarantineStarted, DiseaseIds.EventQuarantineStarted,
                survivorId + " isolated for " + diseaseId, survivorId, diseaseId);
            RaiseStateChanged();
            MaybeContain(entry);
        }

        /// <summary>Release a survivor from the isolation ward.</summary>
        public void EndQuarantine(string survivorId, string diseaseId)
        {
            if (!TryFindPatient(survivorId, diseaseId, out var entry, out var patient)) return;
            if (!patient!.quarantined) return;

            patient.quarantined = false;
            Raise(OnQuarantineEnded, DiseaseIds.EventQuarantineEnded,
                survivorId + " released from quarantine", survivorId, diseaseId);
            RaiseStateChanged();
        }

        /// <summary>
        /// Flagship XI (Plan 155.10) — fictional mutation as a pure gameplay state
        /// transition: move one survivor's active infection from one registered
        /// disease/strain to another, preserving its clinical history (infection
        /// day, days sick, treatment ledger, quarantine). No laboratory semantics.
        /// Returns false when the survivor is not infected with the source id or
        /// the target id is not registered.
        /// </summary>
        public bool MutateInfection(string survivorId, string fromDiseaseId, string toDiseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(fromDiseaseId)
                || string.IsNullOrEmpty(toDiseaseId)
                || string.Equals(fromDiseaseId, toDiseaseId, StringComparison.Ordinal))
                return false;

            if (!_byId.TryGetValue(fromDiseaseId, out var fromEntry)
                || !_byId.TryGetValue(toDiseaseId, out var toEntry))
                return false;

            DiseaseInfectionState? patient = null;
            for (int i = 0; i < fromEntry.infected.Count; i++)
            {
                if (string.Equals(fromEntry.infected[i].survivor_id, survivorId, StringComparison.Ordinal))
                {
                    patient = fromEntry.infected[i];
                    fromEntry.infected.RemoveAt(i);
                    break;
                }
            }
            if (patient == null) return false;

            // A survivor carries one infection per disease entry; the mutated
            // strain replaces any pre-existing one for the same survivor.
            for (int i = toEntry.infected.Count - 1; i >= 0; i--)
            {
                if (string.Equals(toEntry.infected[i].survivor_id, survivorId, StringComparison.Ordinal))
                    toEntry.infected.RemoveAt(i);
            }

            toEntry.infected.Add(patient);
            toEntry.infections_total++;

            Raise(OnStrainMutated, "disease_strain_mutated",
                survivorId + ": " + fromDiseaseId + " has shifted into " + toDiseaseId,
                survivorId, toDiseaseId);
            RaiseStateChanged();
            MaybeContain(fromEntry);
            return true;
        }

        /// <summary>
        /// Item id → dose count → true when the supply could be spent. Set by the host so
        /// the disease engine never touches an inventory: consumption stays on the single
        /// item authority the rest of the game uses, and an unwired host refuses treatment
        /// loudly instead of pretending it happened.
        /// </summary>
        public Func<string, int, bool>? TryConsumeItem;

        /// <summary>
        /// Plan 60 / D3 — treat one patient with one item.
        ///
        /// <para>Everything that decides <em>what treatment means</em> lives in the
        /// catalog: the item must be an authorised treatment for that disease, its role
        /// (curative / suppressive / symptomatic / supportive) is authored, and its
        /// window is enforced by <c>max_days</c>. Callers cannot assert a role, so a
        /// supportive infusion cannot be smuggled through as a cure.</para>
        ///
        /// <para>Deterministic: same state, same day, same item ⇒ same result. One
        /// accepted dose per patient per day, so repeated clicks cannot drain a stockpile
        /// or double-dose a patient. Only a <em>curative</em> treatment removes the
        /// infection; the other roles buy odds, not outcomes.</para>
        /// </summary>
        public DiseaseTreatmentResult TryTreat(
            string survivorId, string diseaseId, string itemId, int day)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)
                || string.IsNullOrEmpty(itemId))
                return DiseaseTreatmentResult.Refuse(DiseaseTreatmentRefusals.NotPatient, itemId, diseaseId, survivorId);

            if (!_byId.TryGetValue(diseaseId, out var entry))
                return DiseaseTreatmentResult.Refuse(DiseaseTreatmentRefusals.UnknownDisease, itemId, diseaseId, survivorId);

            var patient = FindPatient(entry, survivorId);
            if (patient == null)
                return DiseaseTreatmentResult.Refuse(DiseaseTreatmentRefusals.NotPatient, itemId, diseaseId, survivorId);

            var def = _catalog.GetById(diseaseId);
            if (def == null || def.treatments == null || def.treatments.Count == 0)
                return DiseaseTreatmentResult.Refuse(DiseaseTreatmentRefusals.NoTreatmentAuthorised, itemId, diseaseId, survivorId);

            var treatment = def.TreatmentFor(itemId);
            if (treatment == null)
                return DiseaseTreatmentResult.Refuse(DiseaseTreatmentRefusals.ItemNotAuthorised, itemId, diseaseId, survivorId);

            if (treatment.max_days > 0 && patient.days_sick > treatment.max_days)
                return DiseaseTreatmentResult.Refuse(DiseaseTreatmentRefusals.OutsideWindow, itemId, diseaseId, survivorId);

            if (patient.last_treatment_day == day)
                return DiseaseTreatmentResult.Refuse(DiseaseTreatmentRefusals.AlreadyTreatedToday, itemId, diseaseId, survivorId);

            if (TryConsumeItem == null)
                return DiseaseTreatmentResult.Refuse(DiseaseTreatmentRefusals.NoSupplyChannel, itemId, diseaseId, survivorId);

            if (!TryConsumeItem(itemId, 1))
                return DiseaseTreatmentResult.Refuse(DiseaseTreatmentRefusals.SupplyUnavailable, itemId, diseaseId, survivorId);

            // ---- accepted ----
            patient.treatments_applied++;
            patient.last_treatment_day = day;

            float reduction = Math.Clamp(treatment.lethality_reduction, 0f, 1f);
            float before = patient.lethality_reduction;
            patient.lethality_reduction = Math.Min(MaxLethalityReduction, before + reduction);
            float applied = patient.lethality_reduction - before;

            bool cured = false;
            if (DiseaseTreatmentRoles.IsCurative(treatment.role))
            {
                // Curative means the infection is gone, not that the next roll is
                // kinder — the patient recovers now and stops being contagious.
                cured = true;
                patient.current_stage = DiseaseStageNames.Recovered;
                patient.stage_entered_day = day;
                entry.infected.Remove(patient);
                entry.recovered_total++;
                if (def.immunity_duration_days > 0)
                {
                    SetImmunity(survivorId, diseaseId, day + def.immunity_duration_days, def.immunity_strength);
                }
                Raise(OnOutcomeResolved, DiseaseIds.EventRecovered,
                    survivorId + " was treated and recovered from " + diseaseId + " (day " + day + ")",
                    survivorId, diseaseId, true);
                if (patient.quarantined)
                    Raise(OnQuarantineEnded, DiseaseIds.EventQuarantineEnded,
                        survivorId + " left quarantine on cure", survivorId, diseaseId);
                MaybeContain(entry);
            }

            Raise(null!, DiseaseIds.EventTreatmentApplied,
                survivorId + " treated with " + itemId + " (" + treatment.role + ")", survivorId, diseaseId);
            OnTreatmentApplied?.Invoke(survivorId, diseaseId, itemId, treatment.role, day);
            RaiseStateChanged();

            return new DiseaseTreatmentResult
            {
                Accepted = true,
                Reason = "treated",
                Role = treatment.role,
                ItemId = itemId,
                DiseaseId = diseaseId,
                SurvivorId = survivorId,
                LethalityReduction = applied,
                Cured = cured,
            };
        }

        /// <summary>
        /// Effective lethality for one patient after their treatment history — what the
        /// outcome roll uses, exposed so a clinical surface can say "better odds" without
        /// recomputing it.
        /// </summary>
        public float GetEffectiveLethality(string survivorId, string diseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return 0f;
            var def = _catalog.GetById(diseaseId);
            if (def == null) return 0f;
            if (!_byId.TryGetValue(diseaseId, out var entry)) return def.lethality;
            var patient = FindPatient(entry, survivorId);
            return patient == null ? def.lethality
                : Math.Max(0f, def.lethality - patient.lethality_reduction);
        }

        /// <summary>True when the survivor is infected, past incubation, and not isolated.</summary>
        public bool IsContagious(string survivorId, string diseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return false;
            if (!_byId.TryGetValue(diseaseId, out var entry)) return false;
            var patient = FindPatient(entry, survivorId);
            if (patient == null || patient.quarantined) return false;
            return IsContagious(entry!, patient!, _catalog.GetById(diseaseId)!);
        }

        /// <summary>True when the survivor carries an active infection of the disease.</summary>
        public bool IsInfected(string survivorId, string diseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return false;
            if (!_byId.TryGetValue(diseaseId, out var entry)) return false;
            return FindPatient(entry, survivorId) != null;
        }

        /// <summary>True when the survivor is currently held in the isolation ward for the disease.</summary>
        public bool IsQuarantined(string survivorId, string diseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return false;
            if (!_byId.TryGetValue(diseaseId, out var entry)) return false;
            var patient = FindPatient(entry, survivorId);
            return patient != null && patient.quarantined;
        }

        /// <summary>
        /// Read-only infection lookup for the medical pipeline (Task #133 P1).
        /// Reports days_sick and the quarantine flag without exposing the
        /// mutable row. No state is created or mutated.
        /// </summary>
        public bool TryGetInfection(string survivorId, string diseaseId, out int daysSick, out bool quarantined)
        {
            daysSick = 0;
            quarantined = false;
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return false;
            if (!_byId.TryGetValue(diseaseId, out var entry)) return false;
            var patient = FindPatient(entry, survivorId);
            if (patient == null) return false;
            daysSick = patient.days_sick;
            quarantined = patient.quarantined;
            return true;
        }

        private static bool IsContagious(DiseaseEntryState entry, DiseaseInfectionState patient,
            DiseaseDefinition def)
        {
            if (patient == null || patient.quarantined) return false;
            int incubation = def != null ? def.incubation_days : 0;
            return patient.days_sick >= incubation;
        }

        private static bool IsPhysiologicallyContagious(DiseaseInfectionState patient, DiseaseDefinition def)
        {
            if (patient == null) return false;
            int incubation = def != null ? def.incubation_days : 0;
            return patient.days_sick >= incubation;
        }

        private bool TryFindPatient(string survivorId, string diseaseId,
            out DiseaseEntryState entry, out DiseaseInfectionState? patient)
        {
            patient = null;
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId))
            {
                entry = null!;
                return false;
            }
            if (!_byId.TryGetValue(diseaseId, out var foundEntry))
            {
                entry = null!;
                return false;
            }
            entry = foundEntry;
            patient = FindPatient(foundEntry, survivorId);
            return patient != null;
        }

        private static DiseaseInfectionState? FindPatient(DiseaseEntryState entry, string survivorId)
        {
            if (entry == null || entry.infected == null) return null;
            for (int i = 0; i < entry.infected.Count; i++)
            {
                var p = entry.infected[i];
                if (p != null && string.Equals(p.survivor_id, survivorId, StringComparison.Ordinal))
                    return p;
            }
            return null;
        }

        private bool ContainsInfection(DiseaseEntryState entry, string survivorId)
        {
            return FindPatient(entry, survivorId) != null;
        }

        private bool IsQuarantinedAnywhere(string survivorId)
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                var p = FindPatient(_entries[i], survivorId);
                if (p != null && p.quarantined) return true;
            }
            return false;
        }

        // -----------------------------------------------------------------
        // Vector countermeasures (protocols)
        // -----------------------------------------------------------------

        /// <summary>Transmission vector of a disease, or empty when unknown.</summary>
        public string GetTransmissionVector(string diseaseId)
        {
            if (string.IsNullOrEmpty(diseaseId)) return string.Empty;
            return _byId.TryGetValue(diseaseId, out var e) ? e.vector_type : string.Empty;
        }

        /// <summary>True when the authored protocol neutralises a vector.</summary>
        public bool IsVectorBlocked(string vectorType)
        {
            switch (DiseaseVectorNames.Parse(vectorType))
            {
                case DiseaseVector.Water: return _state.water_purified;
                case DiseaseVector.Air: return _state.vents_sealed;
                case DiseaseVector.Blood: return _state.tools_sterilized;
                case DiseaseVector.Spore: return _state.air_filtration;
                default: return false;
            }
        }

        public void PurifyWater(int day = 0)
        {
            if (!_state.water_purified)
            {
                _state.water_purified = true;
                _state.water_purified_until_day = ArmExpiry(day, DiseaseVectorNames.Water);
                RaiseProtocol(DiseaseIds.EventProtocolApplied, ArmDetail("water purified — waterborne vectors blocked", day, DiseaseVectorNames.Water));
            }
        }

        public void ResetWaterPurification()
        {
            if (_state.water_purified)
            {
                _state.water_purified = false;
                _state.water_purified_until_day = 0;
                RaiseProtocol(DiseaseIds.EventProtocolReset, "water purification lapsed");
            }
        }

        public void SealVents(int day = 0)
        {
            if (!_state.vents_sealed)
            {
                _state.vents_sealed = true;
                _state.vents_sealed_until_day = ArmExpiry(day, DiseaseVectorNames.Air);
                RaiseProtocol(DiseaseIds.EventProtocolApplied, ArmDetail("ventilators sealed — airborne vectors blocked", day, DiseaseVectorNames.Air));
            }
        }

        public void ResetVentSeal()
        {
            if (_state.vents_sealed)
            {
                _state.vents_sealed = false;
                _state.vents_sealed_until_day = 0;
                RaiseProtocol(DiseaseIds.EventProtocolReset, "vent seal breached");
            }
        }

        public void SterilizeTools(int day = 0)
        {
            if (!_state.tools_sterilized)
            {
                _state.tools_sterilized = true;
                _state.tools_sterilized_until_day = ArmExpiry(day, DiseaseVectorNames.Blood);
                RaiseProtocol(DiseaseIds.EventProtocolApplied, ArmDetail("surgical tools sterilised — bloodborne vectors blocked", day, DiseaseVectorNames.Blood));
            }
        }

        public void ResetToolSterilization()
        {
            if (_state.tools_sterilized)
            {
                _state.tools_sterilized = false;
                _state.tools_sterilized_until_day = 0;
                RaiseProtocol(DiseaseIds.EventProtocolReset, "tool sterilisation spent");
            }
        }

        public void SetAirFiltration(bool active, int day = 0)
        {
            if (!active)
            {
                if (!_state.air_filtration) return;
                _state.air_filtration = false;
                _state.air_filtration_until_day = 0;
                RaiseProtocol(DiseaseIds.EventProtocolReset, "air filtration offline");
                return;
            }
            // Re-engaging while already on silently refreshes the window — the
            // equivalent of swapping the filters — without re-announcing it.
            bool wasActive = _state.air_filtration;
            _state.air_filtration = true;
            _state.air_filtration_until_day = ArmExpiry(day, DiseaseVectorNames.Spore);
            _state.air_filtration_until_day = ArmExpiry(day, DiseaseVectorNames.Spore);
            if (!wasActive)
                RaiseProtocol(DiseaseIds.EventProtocolApplied,
                    ArmDetail("air filtration engaged — spore vectors blocked", day, DiseaseVectorNames.Spore));
        }

        // ----- Plan 60 / D4: protocol expiry -------------------------------------

        /// <summary>Expiry day for a protocol applied on <paramref name="day"/>:
        /// applied day + authored duration, or 0 when the catalog authors no
        /// duration (protocol holds until manually disengaged).</summary>
        private int ArmExpiry(int day, string vectorName)
        {
            int duration = _catalog.ProtocolDurationDays(vectorName);
            if (day <= 0 || duration <= 0) return 0;
            return day + duration;
        }

        private string ArmDetail(string appliedDetail, int day, string vectorName)
        {
            int duration = _catalog.ProtocolDurationDays(vectorName);
            return duration > 0
                ? appliedDetail + $" (holds ~{duration}d)"
                : appliedDetail;
        }

        /// <summary>
        /// Plan 60 / D4 — lapse every expired protocol. Called from the top of
        /// <see cref="TickDaily"/> so a protocol cannot outlive its authored window
        /// just because nobody remembered to reset it. Deterministic: pure day
        /// arithmetic, no rolls, no RNG consumption.
        /// </summary>
        public void TickProtocolExpiry(int day)
        {
            if (day <= 0) return;

            // Water carries the documented legacy path: a pre-D4 save has the flag
            // on with no recorded expiry (until_day 0), so it arms on the first
            // tick after restore and lapses one full window later. The other three
            // use the same rule — a flag without an armed window re-arms now.
            if (_state.water_purified)
            {
                if (_state.water_purified_until_day == 0)
                    _state.water_purified_until_day = ArmExpiry(day, DiseaseVectorNames.Water);
                else if (day >= _state.water_purified_until_day) ResetWaterPurification();
            }
            if (_state.vents_sealed)
            {
                if (_state.vents_sealed_until_day == 0)
                    _state.vents_sealed_until_day = ArmExpiry(day, DiseaseVectorNames.Air);
                else if (day >= _state.vents_sealed_until_day) ResetVentSeal();
            }
            if (_state.tools_sterilized)
            {
                if (_state.tools_sterilized_until_day == 0)
                    _state.tools_sterilized_until_day = ArmExpiry(day, DiseaseVectorNames.Blood);
                else if (day >= _state.tools_sterilized_until_day) ResetToolSterilization();
            }
            if (_state.air_filtration)
            {
                if (_state.air_filtration_until_day > 0 && day >= _state.air_filtration_until_day)
                    SetAirFiltration(false, day);
                else if (_state.air_filtration_until_day == 0)
                    _state.air_filtration_until_day = ArmExpiry(day, DiseaseVectorNames.Spore);
            }
        }

        /// <summary>Days remaining before the protocol lapses (≤ 0: not armed;
        /// <see cref="int.MaxValue"/>: holds until manually disengaged).</summary>
        public int ProtocolDaysRemaining(string vectorType, int today)
        {
            bool active;
            int until;
            switch (DiseaseVectorNames.Parse(vectorType))
            {
                case DiseaseVector.Water: active = _state.water_purified; until = _state.water_purified_until_day; break;
                case DiseaseVector.Air: active = _state.vents_sealed; until = _state.vents_sealed_until_day; break;
                case DiseaseVector.Blood: active = _state.tools_sterilized; until = _state.tools_sterilized_until_day; break;
                case DiseaseVector.Spore: active = _state.air_filtration; until = _state.air_filtration_until_day; break;
                default: return 0;
            }
            if (!active) return -1;
            if (until <= 0) return int.MaxValue;
            return Math.Max(0, until - today);
        }

        private void RaiseProtocol(string eventId, string detail)
        {
            Raise(null!, eventId, detail, (string)null!, null!);
            RaiseStateChanged();
        }

        // -----------------------------------------------------------------
        // Read model
        // -----------------------------------------------------------------

        public DiseaseSnapshot GetSnapshot()
        {
            var snap = new DiseaseSnapshot();
            for (int i = 0; i < _entries.Count; i++)
            {
                var entry = _entries[i];
                var def = _catalog.GetById(entry.disease_id);
                snap.total_outbreaks += entry.outbreaks_total;
                snap.total_outbreaks_prevented += entry.outbreaks_prevented;
                snap.total_recovered += entry.recovered_total;
                snap.total_deaths += entry.deaths_total;

                for (int p = 0; p < entry.infected.Count; p++)
                {
                    var patient = entry.infected[p];
                    if (patient == null) continue;
                    snap.total_infected++;
                    if (patient.quarantined) snap.total_quarantined++;
                    bool contagious = IsContagious(entry!, patient!, def!);
                    if (contagious) snap.total_contagious++;

                    var stage = def != null ? def.GetStage(patient.days_sick) : DiseaseStage.Incubating;
                    string stageName = stage.ToString();
                    string stageToken = stageName.ToLowerInvariant();

                    snap.patients.Add(new DiseasePatientSnapshot
                    {
                        survivor_id = patient.survivor_id,
                        disease_id = entry.disease_id,
                        disease_name = def != null ? def.display_name : entry.disease_id,
                        days_sick = patient.days_sick,
                        quarantined = patient.quarantined,
                        contagious = contagious,
                        // Plan 60 / D3 — treatment history is part of the clinical
                        // picture, so the read model carries it instead of making the
                        // surface guess.
                        treatments_applied = patient.treatments_applied,
                        effective_lethality = def != null
                            ? Math.Max(0f, def.lethality - patient.lethality_reduction) : 0f,
                        contagion_risk_percent = def != null
                            ? (int)(Math.Min(1f, Math.Max(0f, def.infectivity)) * 100f) : 0,
                        current_stage = stageName,
                        stage_token = stageToken
                    });
                }
            }
            return snap;
        }

        public int TotalInfectionsHistory
        {
            get
            {
                int n = 0;
                for (int i = 0; i < _entries.Count; i++) n += _entries[i].infections_total;
                return n;
            }
        }

        public DiseaseEntryState? GetDiseaseState(string diseaseId)
        {
            return diseaseId != null && _byId.TryGetValue(diseaseId, out var e) ? e : null;
        }

        // -----------------------------------------------------------------
        // Save / load
        // -----------------------------------------------------------------

        public DiseaseSystemState CaptureState()
        {
            _state.rngSeed = _rng.Seed;
            return _state;
        }

        public void RestoreState(DiseaseSystemState saved)
        {
            if (saved == null) return;
            _state.stateVersion = saved.stateVersion;
            _state.system_id = saved.system_id;
            _state.water_purified = saved.water_purified;
            _state.vents_sealed = saved.vents_sealed;
            _state.tools_sterilized = saved.tools_sterilized;
            _state.air_filtration = saved.air_filtration;
            _state.water_purified_until_day = saved.water_purified_until_day;
            _state.vents_sealed_until_day = saved.vents_sealed_until_day;
            _state.tools_sterilized_until_day = saved.tools_sterilized_until_day;
            _state.air_filtration_until_day = saved.air_filtration_until_day;
            _state.rngSeed = saved.rngSeed;

            // Deep copy so the caller's DTO (and the save envelope) is not
            // aliased into live memory.
            _state.diseases = new List<DiseaseEntryState>();
            if (saved.diseases != null)
            {
                for (int i = 0; i < saved.diseases.Count; i++)
                {
                    var src = saved.diseases[i];
                    if (src == null || string.IsNullOrEmpty(src.disease_id)) continue;
                    var copy = new DiseaseEntryState
                    {
                        disease_id = src.disease_id,
                        vector_type = src.vector_type,
                        spread_timer = src.spread_timer,
                        outbreak_active = src.outbreak_active,
                        deaths_during_outbreak = src.deaths_during_outbreak,
                        outbreaks_total = src.outbreaks_total,
                        outbreaks_prevented = src.outbreaks_prevented,
                        recovered_total = src.recovered_total,
                        deaths_total = src.deaths_total,
                        infections_total = src.infections_total
                    };
                    if (src.infected != null)
                    {
                        for (int j = 0; j < src.infected.Count; j++)
                        {
                            var p = src.infected[j];
                            if (p == null) continue;
                            copy.infected.Add(new DiseaseInfectionState
                            {
                                survivor_id = p.survivor_id,
                                infected_day = p.infected_day,
                                days_sick = p.days_sick,
                                quarantined = p.quarantined,
                                current_stage = string.IsNullOrEmpty(p.current_stage) ? DiseaseStageNames.Incubating : p.current_stage,
                                stage_entered_day = p.stage_entered_day,
                                // Additive D3 fields: absent in a pre-D3 save, which
                                // truthfully means "this patient was never treated".
                                treatments_applied = p.treatments_applied,
                                lethality_reduction = p.lethality_reduction,
                                last_treatment_day = p.last_treatment_day
                            });
                        }
                    }
                    _state.diseases.Add(copy);
                }
            }

            _state.immunities = new List<DiseaseImmunityRecord>();
            if (saved.immunities != null)
            {
                for (int k = 0; k < saved.immunities.Count; k++)
                {
                    var imm = saved.immunities[k];
                    if (imm == null || string.IsNullOrEmpty(imm.survivor_id) || string.IsNullOrEmpty(imm.disease_id)) continue;
                    _state.immunities.Add(new DiseaseImmunityRecord
                    {
                        survivor_id = imm.survivor_id,
                        disease_id = imm.disease_id,
                        immunity_until_day = imm.immunity_until_day,
                        strength = imm.strength
                    });
                }
            }

            RebuildIndexFromState();

            // Re-seed from the saved seed so post-restore outcomes are identical
            // to the same run without an intervening save.
            _rng = _rngFactory(_state.rngSeed == 0 ? DefaultSeed : _state.rngSeed);
            _state.rngSeed = _rng.Seed;
        }

        // -----------------------------------------------------------------
        // Event plumbing
        // -----------------------------------------------------------------

        private void Raise(Action<string, string> typed, string eventId, string detail,
            string survivorId, string diseaseId)
        {
            if (typed != null && !string.IsNullOrEmpty(survivorId) && !string.IsNullOrEmpty(diseaseId))
                typed.Invoke(survivorId, diseaseId);
            if (OnEventRaised != null && !string.IsNullOrEmpty(eventId))
                OnEventRaised.Invoke(eventId, detail ?? string.Empty);
        }

        private void Raise(Action<string> typed, string eventId, string detail, string diseaseId)
        {
            if (typed != null && !string.IsNullOrEmpty(diseaseId))
                typed.Invoke(diseaseId);
            if (OnEventRaised != null && !string.IsNullOrEmpty(eventId))
                OnEventRaised.Invoke(eventId, detail ?? string.Empty);
        }

        private void Raise(Action<string, bool> typed, string eventId, string detail, string diseaseId, bool prevented)
        {
            if (typed != null && !string.IsNullOrEmpty(diseaseId))
                typed.Invoke(diseaseId, prevented);
            if (OnEventRaised != null && !string.IsNullOrEmpty(eventId))
                OnEventRaised.Invoke(eventId, detail ?? string.Empty);
        }

        private void Raise(Action<string, string, bool> typed, string eventId, string detail,
            string survivorId, string diseaseId, bool recovered)
        {
            if (typed != null && !string.IsNullOrEmpty(survivorId) && !string.IsNullOrEmpty(diseaseId))
                typed.Invoke(survivorId, diseaseId, recovered);
            if (OnEventRaised != null && !string.IsNullOrEmpty(eventId))
                OnEventRaised.Invoke(eventId, detail ?? string.Empty);
        }

        private void RaiseStateChanged()
        {
            CaptureState();
            OnStateChanged?.Invoke(_state);
        }
    }
}
