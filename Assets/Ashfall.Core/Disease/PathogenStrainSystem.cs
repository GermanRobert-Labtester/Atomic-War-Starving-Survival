using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Disease
{
    /// <summary>One fictional cure/treatment research project (Plan 155.12).</summary>
    public sealed class PathogenCureProjectState
    {
        public string strainId = string.Empty;
        public int startedDay;
        public int daysInvested;
        public int requiredDays = 10;
        public bool complete;
    }

    /// <summary>Authoritative strain-layer state (cure projects; mutation results live in the engine's own state).</summary>
    public sealed class PathogenStrainSystemState
    {
        public int schemaVersion = 1;
        public List<PathogenCureProjectState> cureProjects = new List<PathogenCureProjectState>();
        /// <summary>Strain ids with a completed cure, mapped to the completion day.</summary>
        public List<string> curedStrainIds = new List<string>();
    }

    /// <summary>
    /// Flagship XI — Plan 155 strain layer over the canonical
    /// <see cref="DiseaseSystem"/>. Owns ONLY what the base engine lacks:
    /// <list type="bullet">
    /// <item>merging fictional strains (<see cref="PathogenStrainDef"/>) into the
    /// disease catalog as derived definitions, so spread, quarantine, treatment
    /// and engine persistence run strains with zero parallel state;</item>
    /// <item>deterministic mutation transitions between sibling strains
    /// (per-active-infection seeded rolls; selection survives save via the
    /// engine's own infection state);</item>
    /// <item>abstract radiation coupling — a read-only dose query raises both the
    /// effective outcome severity (engine lethality-modifier hook) and the
    /// mutation pressure; the dose itself is never written here;</item>
    /// <item>the fictional cure project (bounded abstract research; completion
    /// reduces outcome severity through the same hook and per-patient
    /// treatment ledger).</item>
    /// </list>
    /// All gameplay content is fictional and non-procedural: incubation, spread
    /// and outcome classes only — no real-world laboratory property is modeled.
    /// </summary>
    public sealed class PathogenStrainSystem
    {
        public const string SystemId = "pathogen_strains";
        public const float CureEfficacyLethalityRelief = 0.5f;
        public const float MaxRadiationLethalityPressure = 0.3f;
        public const int DefaultCureDays = 10;

        private readonly PathogenStrainCatalogContainer _catalog;
        private readonly DiseaseSystem _disease;
        private readonly PathogenStrainSystemState _state = new PathogenStrainSystemState();
        private readonly Dictionary<string, PathogenStrainDef> _strainById =
            new Dictionary<string, PathogenStrainDef>(StringComparer.Ordinal);
        private bool _attached;

        /// <summary>Canonical read-only dose query (0..100). Host wires the radiation authority.</summary>
        public Func<string, float>? RadiationDoseQuery { get; set; }

        /// <summary>Raised when a fictional strain mutation lands (survivorId, fromId, toId).</summary>
        public event Action<string, string, string>? OnStrainMutation;
        /// <summary>Raised when a fictional cure project completes (strainId, day).</summary>
        public event Action<string, int>? OnCureCompleted;

        public PathogenStrainSystem(PathogenStrainCatalogContainer catalog, DiseaseSystem disease)
        {
            _catalog = catalog ?? new PathogenStrainCatalogContainer();
            _disease = disease ?? throw new ArgumentNullException(nameof(disease));
            foreach (var strain in _catalog.pathogen_strains)
            {
                if (strain == null || string.IsNullOrEmpty(strain.id)) continue;
                // Duplicate ids: keep the first (later rows are ignored, never
                // overwrite — a catalog typo cannot silently replace a strain).
                if (!_strainById.TryAdd(strain.id, strain))
                    continue;
            }
        }

        // ------------------------------------------------------------- attach

        /// <summary>
        /// Merges the strain catalog into the engine as first-class simulation
        /// rows (definition + entry, idempotent). Returns false if any strain
        /// names an unknown parent disease.
        /// </summary>
        public bool AttachStrains()
        {
            if (_attached) return true;

            bool allAttached = true;
            foreach (var strain in _catalog.pathogen_strains.OrderBy(s => s.id, StringComparer.Ordinal))
            {
                if (strain == null || string.IsNullOrEmpty(strain.id)) continue;
                if (_disease.GetDefinition(strain.id) != null)
                    continue; // already registered (attach or restore path)

                var parent = _disease.GetDefinition(strain.strain_of);
                if (parent == null) { allAttached = false; continue; }

                _disease.RegisterStrain(new DiseaseDefinition
                {
                    id = strain.id,
                    display_name = strain.display_name,
                    vector = parent.vector,
                    lethality = Math.Clamp(strain.lethality, 0f, 1f),
                    incubation_days = Math.Max(0, strain.incubation_days),
                    illness_days = Math.Max(1, strain.illness_days),
                    infectivity = Math.Clamp(strain.infectivity, 0f, 1f),
                    spread_interval_days = parent.spread_interval_days,
                    spread_radius = parent.spread_radius,
                    countermeasure_item_id = parent.countermeasure_item_id,
                    guidance = parent.guidance,
                    tell = string.IsNullOrWhiteSpace(parent.tell)
                        ? "A variant course of " + parent.display_name + "."
                        : "A strain of " + parent.display_name + ": " + parent.tell,
                    tell_secondary = parent.tell_secondary,
                    timing_clue = parent.timing_clue,
                    source_note = "fictional gameplay strain of " + parent.id,
                    treatments = parent.treatments
                });
            }

            _attached = allAttached;
            return _attached;
        }

        // ----------------------------------------------------------- mutation

        /// <summary>
        /// One deterministic mutation pass (Flagship XI QA repair). Two
        /// directions, both ordinal by id and seeded per (day, survivor, pair):
        /// 1. PRIMARY — an authored parent disease can emerge into its child
        ///    strain (this is how strains reach the holdfast: any natural
        ///    infection is a potential patient zero). Emergence chance is the
        ///    child's own mutation_chance_per_day, raised by the dose factor.
        ///    Owner-tunable entirely through pathogens.json.
        /// 2. DRIFT — an active strain infection transitions into a sibling
        ///    strain from its mutation_targets.
        /// </summary>
        public void TickMutations(int day)
        {
            if (!_attached) AttachStrains();

            var radiation = RadiationDoseQuery;
            foreach (var strainId in _strainById.Keys.OrderBy(id => id, StringComparer.Ordinal).ToList())
            {
                var strain = _strainById[strainId];
                var entry = _disease.GetDiseaseState(strainId);

                // Primary direction: parent infections may emerge into this strain.
                var parentEntry = _disease.GetDiseaseState(strain.strain_of);
                if (parentEntry != null && parentEntry.infected.Count > 0)
                {
                    foreach (var patient in parentEntry.infected
                        .OrderBy(p => p.survivor_id, StringComparer.Ordinal).ToList())
                    {
                        if (patient == null || string.IsNullOrEmpty(patient.survivor_id)) continue;
                        // Already carrying this strain (or mutating onward): skip.
                        if (PatientCarries(patient.survivor_id, strainId)) continue;
                        float dose = radiation?.Invoke(patient.survivor_id) ?? 0f;
                        float chance = MutationChance(strain, dose);
                        if (chance <= 0f) continue;

                        var rng = new SeededRng(MutationSeed(day, patient.survivor_id, strain.strain_of + "->" + strainId));
                        if (rng.NextDouble() >= chance) continue;

                        if (_disease.MutateInfection(patient.survivor_id, strain.strain_of, strainId))
                            OnStrainMutation?.Invoke(patient.survivor_id, strain.strain_of, strainId);
                    }
                }

                // Drift direction: active strain infections transition onward.
                if (strain.mutation_chance_per_day <= 0f || strain.mutation_targets.Count == 0) continue;
                if (entry == null || entry.infected.Count == 0) continue;

                foreach (var patient in entry.infected.OrderBy(p => p.survivor_id, StringComparer.Ordinal).ToList())
                {
                    if (patient == null || string.IsNullOrEmpty(patient.survivor_id)) continue;
                    float dose = radiation?.Invoke(patient.survivor_id) ?? 0f;
                    float chance = MutationChance(strain, dose);
                    if (chance <= 0f) continue;

                    var rng = new SeededRng(MutationSeed(day, patient.survivor_id, strainId));
                    if (rng.NextDouble() >= chance) continue;

                    // Deterministic target: ordinal-first sibling (the catalog is
                    // the authoring authority; no reroll after save because the
                    // transition itself is persisted in the engine's state).
                    var target = strain.mutation_targets
                        .Where(t => _strainById.ContainsKey(t))
                        .OrderBy(t => t, StringComparer.Ordinal)
                        .FirstOrDefault();
                    if (target == null) continue;

                    if (_disease.MutateInfection(patient.survivor_id, strainId, target))
                        OnStrainMutation?.Invoke(patient.survivor_id, strainId, target);
                }
            }
        }

        private bool PatientCarries(string survivorId, string diseaseId)
        {
            var entry = _disease.GetDiseaseState(diseaseId);
            if (entry == null) return false;
            foreach (var patient in entry.infected)
                if (patient != null && string.Equals(patient.survivor_id, survivorId, StringComparison.Ordinal))
                    return true;
            return false;
        }

        /// <summary>Pure: effective per-day mutation chance under the current radiation dose.</summary>
        public static float MutationChance(PathogenStrainDef strain, float dose)
        {
            if (strain == null) return 0f;
            float baseChance = Math.Clamp(strain.mutation_chance_per_day, 0f, 1f);
            float gain = Math.Clamp(strain.radiation_severity_gain, 0f, 1f);
            float doseFactor = 1f + (Math.Clamp(dose, 0f, 100f) / 100f) * gain;
            return Math.Clamp(baseChance * doseFactor, 0f, 1f);
        }

        /// <summary>Stable per-(day, survivor, strain) roll seed — FNV-1a over the parts.</summary>
        public static int MutationSeed(int day, string survivorId, string strainId)
        {
            unchecked
            {
                uint hash = 2166136261u;
                void Mix(int value)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        hash ^= (byte)((value >> (i * 8)) & 0xFF);
                        hash *= 16777619u;
                    }
                }
                Mix(day);
                foreach (char c in survivorId ?? string.Empty) { hash ^= c; hash *= 16777619u; }
                hash ^= 0x7F; hash *= 16777619u;
                foreach (char c in strainId ?? string.Empty) { hash ^= c; hash *= 16777619u; }
                return (int)hash;
            }
        }

        // ------------------------------------------------------ radiation hook

        /// <summary>
        /// The engine's lethality-modifier: abstract radiation severity pressure
        /// for strain infections only, reduced by the per-patient treatment
        /// ledger already applied by the engine (never below zero there). A
        /// COMPLETED CURE turns the modifier negative for that strain
        /// (CureEfficacyLethalityRelief) — the unlock's realized gameplay effect.
        /// </summary>
        public float RadiationSeverityPressure(string survivorId, string diseaseId)
        {
            if (!_strainById.TryGetValue(diseaseId, out var strain)) return 0f;
            float dose = RadiationDoseQuery?.Invoke(survivorId) ?? 0f;
            float pressure = (Math.Clamp(dose, 0f, 100f) / 100f)
                             * Math.Clamp(strain.radiation_severity_gain, 0f, 1f)
                             * MaxRadiationLethalityPressure;
            if (IsCureUnlocked(diseaseId)) pressure -= CureEfficacyLethalityRelief;
            return pressure;
        }

        /// <summary>Wires the engine hook (call after construction).</summary>
        public void BindEngineHooks()
        {
            _disease.EffectiveLethalityModifier = RadiationSeverityPressure;
        }

        // --------------------------------------------------------------- cure

        public IReadOnlyList<PathogenCureProjectState> CureProjects => _state.cureProjects;

        public bool IsCureUnlocked(string strainId) => _state.curedStrainIds.Contains(strainId);

        /// <summary>
        /// Starts the fictional cure project for a strain (costs are charged by
        /// the host before calling; completion needs daily research labor).
        /// </summary>
        public bool StartCureProject(string strainId, int day, int? requiredDays = null)
        {
            if (!_strainById.ContainsKey(strainId)) return false;
            if (IsCureUnlocked(strainId)) return false;
            foreach (var project in _state.cureProjects)
                if (project != null && string.Equals(project.strainId, strainId, StringComparison.Ordinal)
                    && !project.complete)
                    return false; // already in progress

            _state.cureProjects.Add(new PathogenCureProjectState
            {
                strainId = strainId,
                startedDay = day,
                requiredDays = Math.Max(1, requiredDays ?? DefaultCureDays)
            });
            return true;
        }

        /// <summary>One day of research labor on every incomplete project.</summary>
        public bool AdvanceCureProjects(int day)
        {
            bool anyCompleted = false;
            foreach (var project in _state.cureProjects)
            {
                if (project == null || project.complete) continue;
                project.daysInvested++;
                if (project.daysInvested >= project.requiredDays)
                {
                    project.complete = true;
                    if (!_state.curedStrainIds.Contains(project.strainId))
                        _state.curedStrainIds.Add(project.strainId);
                    anyCompleted = true;
                    OnCureCompleted?.Invoke(project.strainId, day);
                }
            }
            return anyCompleted;
        }

        // -------------------------------------------------------- persistence

        public PathogenStrainSystemState CaptureState()
        {
            var copy = new PathogenStrainSystemState();
            foreach (var project in _state.cureProjects)
                copy.cureProjects.Add(new PathogenCureProjectState
                {
                    strainId = project.strainId,
                    startedDay = project.startedDay,
                    daysInvested = project.daysInvested,
                    requiredDays = project.requiredDays,
                    complete = project.complete
                });
            copy.curedStrainIds = new List<string>(_state.curedStrainIds);
            return copy;
        }

        /// <summary>NON-OPERATIVE restore: reconstructs project state only.</summary>
        public void RestoreState(PathogenStrainSystemState state)
        {
            _state.cureProjects.Clear();
            _state.curedStrainIds.Clear();
            if (state == null) return;
            foreach (var project in state.cureProjects ?? new List<PathogenCureProjectState>())
                if (project != null && !string.IsNullOrEmpty(project.strainId))
                    _state.cureProjects.Add(new PathogenCureProjectState
                    {
                        strainId = project.strainId,
                        startedDay = project.startedDay,
                        daysInvested = project.daysInvested,
                        requiredDays = project.requiredDays,
                        complete = project.complete
                    });
            foreach (var id in state.curedStrainIds ?? new List<string>())
                if (!string.IsNullOrEmpty(id))
                    _state.curedStrainIds.Add(id);
        }
    }
}
