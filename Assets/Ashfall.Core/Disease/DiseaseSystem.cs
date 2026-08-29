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
        public const int CurrentVersion = 1;

        public int stateVersion = CurrentVersion;
        public string system_id = DiseaseIds.ExpansionId;

        // Protocol flags (vector countermeasures).
        public bool water_purified = false;
        public bool vents_sealed = false;
        public bool tools_sterilized = false;
        public bool air_filtration = false;

        // Preserved across ticks / restores so a reload reproduces the same
        // outcome sequence (determinism invariant).
        public int rngSeed = 0;

        public List<DiseaseEntryState> diseases = new List<DiseaseEntryState>();
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

        // Typed events (established convention — no third bus).
        public event Action<string, string> OnInfection;                    // survivorId, diseaseId
        public event Action<string, string> OnQuarantineStarted;            // survivorId, diseaseId
        public event Action<string, string> OnQuarantineEnded;              // survivorId, diseaseId
        public event Action<string> OnOutbreakDeclared;                     // diseaseId
        public event Action<string, bool> OnOutbreakContained;              // diseaseId, prevented
        public event Action<string, string, bool> OnOutcomeResolved;        // survivorId, diseaseId, recovered
        public event Action<DiseaseSystemState> OnStateChanged;
        /// <summary>Forwarder for the string event bus (optional).</summary>
        public event Action<string, string> OnEventRaised;                  // eventId, detail

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
                days_sick = 0
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
            // Public candidates: everyone not already infected with this disease
            // and not quarantined (quarantine is the isolation protocol).
            var pool = new List<string>(candidates.Count);
            for (int i = 0; i < candidates.Count; i++)
            {
                string c = candidates[i];
                if (string.IsNullOrEmpty(c)) continue;
                if (ContainsInfection(entry, c)) continue;      // already infected
                if (IsQuarantinedAnywhere(c)) continue;         // isolation ward
                if (!pool.Contains(c)) pool.Add(c);              // de-dupe, keep host order
            }
            if (pool.Count == 0 || def.infectivity <= 0f) return;

            // The first contagious (past-incubation, un-quarantined) patient
            // drives this interval's spread attempt; one patient per interval.
            var targets = new List<string>();
            for (int p = 0; p < entry.infected.Count; p++)
            {
                var patient = entry.infected[p];
                if (patient == null || patient.quarantined) continue;
                if (!IsContagious(entry, patient, def)) continue; // still incubating
                if (_rng.NextDouble() >= def.infectivity) break;  // this patient does not shed

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

                if (patient.days_sick < def.illness_days) continue;

                bool died = def.lethality > 0f && _rng.NextDouble() < def.lethality;
                removed.Add(patient);
                if (died)
                {
                    entry.deaths_total++;
                    if (entry.outbreak_active) entry.deaths_during_outbreak++;
                    Raise(OnOutcomeResolved, DiseaseIds.EventDied,
                        patient.survivor_id + " died of " + entry.disease_id + " (day " + day + ")",
                        patient.survivor_id, entry.disease_id, false);
                }
                else
                {
                    entry.recovered_total++;
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

        private static bool IsContagious(DiseaseEntryState entry, DiseaseInfectionState patient,
            DiseaseDefinition def)
        {
            if (patient == null || patient.quarantined) return false;
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

        public void PurifyWater()
        {
            if (!_state.water_purified)
            {
                _state.water_purified = true;
                RaiseProtocol(DiseaseIds.EventProtocolApplied, "water purified — waterborne vectors blocked");
            }
        }

        public void ResetWaterPurification()
        {
            if (_state.water_purified)
            {
                _state.water_purified = false;
                RaiseProtocol(DiseaseIds.EventProtocolReset, "water purification lapsed");
            }
        }

        public void SealVents()
        {
            if (!_state.vents_sealed)
            {
                _state.vents_sealed = true;
                RaiseProtocol(DiseaseIds.EventProtocolApplied, "ventilators sealed — airborne vectors blocked");
            }
        }

        public void ResetVentSeal()
        {
            if (_state.vents_sealed)
            {
                _state.vents_sealed = false;
                RaiseProtocol(DiseaseIds.EventProtocolReset, "vent seal breached");
            }
        }

        public void SterilizeTools()
        {
            if (!_state.tools_sterilized)
            {
                _state.tools_sterilized = true;
                RaiseProtocol(DiseaseIds.EventProtocolApplied, "surgical tools sterilised — bloodborne vectors blocked");
            }
        }

        public void ResetToolSterilization()
        {
            if (_state.tools_sterilized)
            {
                _state.tools_sterilized = false;
                RaiseProtocol(DiseaseIds.EventProtocolReset, "tool sterilisation spent");
            }
        }

        public void SetAirFiltration(bool active)
        {
            if (_state.air_filtration == active) return;
            _state.air_filtration = active;
            RaiseProtocol(active ? DiseaseIds.EventProtocolApplied : DiseaseIds.EventProtocolReset,
                active ? "air filtration engaged — spore vectors blocked" : "air filtration offline");
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

                    snap.patients.Add(new DiseasePatientSnapshot
                    {
                        survivor_id = patient.survivor_id,
                        disease_id = entry.disease_id,
                        disease_name = def != null ? def.display_name : entry.disease_id,
                        days_sick = patient.days_sick,
                        quarantined = patient.quarantined,
                        contagious = contagious,
                        contagion_risk_percent = def != null
                            ? (int)(Math.Min(1f, Math.Max(0f, def.infectivity)) * 100f) : 0
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
                                quarantined = p.quarantined
                            });
                        }
                    }
                    _state.diseases.Add(copy);
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
