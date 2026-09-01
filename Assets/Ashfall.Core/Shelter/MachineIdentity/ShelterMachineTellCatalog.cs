// SPDX-License-Identifier: MIT
// ASHFALL Core: shelter machine identity + diagnostic tell projections
// (Plan 29 Phase 3 — Task 29B pilot, §29B.1–29B.9).
//
// The machines already own their condition (see MACHINE_CONDITION_PROVENANCE.md):
//   • HEPA stack            → StartingLevelSystem.airFilterHealthPercent (+ radon, weather, duty)
//   • Silent Foundry cupola → SilentFoundrySystem's five facility components
// This catalog persists nothing and mutates nothing: machines/quirks are authored
// data (shelter_machine_identities.json), and tells are projected from readings the
// host snapshots off the owning systems at query time (§29B.7 "projection").
//
// Truthfulness rule (§1.5): a diagnostic tell's threshold must equal a threshold the
// owning system actually acts on. Authored floors copy the owners' own warning
// values (HEPA: StartingLevelSystem warns at filter < 50; Foundry:
// SilentFoundrySystem.GetSafetyWarnings() floors 35/30/25) and tests pin the
// equality against the real systems.
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    /// <summary>Named condition keys the projector can read. Each maps to a real field on the owning system.</summary>
    public static class MachineConditionKeys
    {
        /// <summary>StartingLevelSystem.State.airFilterHealthPercent (0–100).</summary>
        public const string HepaFilterHealth = "hepa.filter_health";
        /// <summary>StartingLevelSystem.State.radonLevelBqm3 (12–150 Bq/m³).</summary>
        public const string HepaRadon = "hepa.radon_bqm3";
        /// <summary>SilentFoundrySystem facility components (0–100 each).</summary>
        public const string FoundryRefractoryLining = "foundry.refractory_lining";
        public const string FoundryHearthTuyeres = "foundry.hearth_tuyeres";
        public const string FoundrySandBeds = "foundry.sand_beds";
        public const string FoundryStructuralSupports = "foundry.structural_supports";
        public const string FoundrySafetyExhaust = "foundry.safety_exhaust";
        /// <summary>SilentFoundrySystem.AverageFacilityCondition() — the owner's own overall figure.</summary>
        public const string FoundryAverageCondition = "foundry.average_condition";

        /// <summary>All resolvable condition keys (ordinal-stable).</summary>
        // New machines (Plan 29B Phase 4 roster completion).
        public const string PowerFuelUnits = "power.fuel_units";
        public const string PowerBatteryReserve = "power.battery_reserve";
        public const string VentilationFilterSaturation = "ventilation.filter_saturation";
        public const string VentilationDuctIntegrity = "ventilation.duct_integrity";
        public const string WaterFilterIntegrity = "water.filter_integrity";
        public const string ThermalBoilerFuel = "thermal.boiler_fuel";
        public const string AirlockIncidentActive = "airlock.incident_active";

        public static readonly string[] All =
        {
            HepaFilterHealth, HepaRadon,
            FoundryRefractoryLining, FoundryHearthTuyeres, FoundrySandBeds,
            FoundryStructuralSupports, FoundrySafetyExhaust, FoundryAverageCondition,
            PowerFuelUnits, PowerBatteryReserve,
            VentilationFilterSaturation, VentilationDuctIntegrity,
            WaterFilterIntegrity, ThermalBoilerFuel, AirlockIncidentActive
        };

        /// <summary>Condition-key family of a machine id: machine_hepa_stack → "hepa".</summary>
        public static string FamilyOf(string machineId)
        {
            if (string.IsNullOrEmpty(machineId)) return string.Empty;
            string rest = machineId.StartsWith("machine_", StringComparison.Ordinal)
                ? machineId.Substring("machine_".Length)
                : machineId;
            int underscore = rest.IndexOf('_');
            return underscore > 0 ? rest.Substring(0, underscore) : rest;
        }
    }

    /// <summary>
    /// Condition readings for one evaluation, snapshotted from the owning systems.
    /// Typed fields (not a string map) so the provenance chain stays explicit.
    /// </summary>
    public sealed class MachineConditionReadings
    {
        /// <summary>StartingLevelSystem.State.airFilterHealthPercent (0–100).</summary>
        public float HepaFilterHealth = 100f;
        /// <summary>StartingLevelSystem radonLevelBqm3 (12–150 Bq/m³).</summary>
        public float HepaRadon = 12f;
        /// <summary>Authoritative hazard weather (fallout storm / black rain / ashfall).</summary>
        public bool HazardWeather;

        public float FoundryRefractoryLining = 100f;
        public float FoundryHearthTuyeres = 100f;
        public float FoundrySandBeds = 100f;
        public float FoundryStructuralSupports = 100f;
        public float FoundrySafetyExhaust = 100f;

        /// <summary>PowerGridSystem.State.FuelUnits (absolute; default scale 0–100+).</summary>
        public float PowerFuelUnits = 100f;
        /// <summary>PowerGridSystem battery reserve as a percent of capacity (0–100).</summary>
        public float PowerBatteryReserve = 100f;
        /// <summary>PowerGridSystem.IsBrownout.</summary>
        public bool PowerBrownout;
        /// <summary>VentilationSystem exhaustFilterSaturation (0–100, higher = clogged).</summary>
        public float VentilationFilterSaturation;
        /// <summary>VentilationSystem ductIntegrity (0–100).</summary>
        public float VentilationDuctIntegrity = 100f;
        /// <summary>VentilationSystem smokeSootLevel (0–100, higher = hazard).</summary>
        public float VentilationSmokeSoot;
        /// <summary>VentilationSystem.mainDuctOpen (authoritative duct state).</summary>
        public bool VentilationMainDuctOpen = true;
        /// <summary>WaterTreatmentSystem.filterIntegrity (0–100).</summary>
        public float WaterFilterIntegrity = 100f;
        /// <summary>ShelterThermalSystem boiler fuel (0–100).</summary>
        public float ThermalBoilerFuel = 100f;
        /// <summary>AirlockSecuritySystem.HasPendingIncident (context gate, 0/1).</summary>
        public bool AirlockIncidentActive;

        /// <summary>Optional authoring note: which host snapshot supplied these readings (diagnostics only).</summary>
        public string source = string.Empty;

        /// <summary>Resolve a condition key. Unknown keys yield null — never a guess (§13.2).</summary>
        public float? Get(string conditionKey)
        {
            switch (conditionKey)
            {
                case MachineConditionKeys.HepaFilterHealth: return HepaFilterHealth;
                case MachineConditionKeys.HepaRadon: return HepaRadon;
                case MachineConditionKeys.FoundryRefractoryLining: return FoundryRefractoryLining;
                case MachineConditionKeys.FoundryHearthTuyeres: return FoundryHearthTuyeres;
                case MachineConditionKeys.FoundrySandBeds: return FoundrySandBeds;
                case MachineConditionKeys.FoundryStructuralSupports: return FoundryStructuralSupports;
                case MachineConditionKeys.FoundrySafetyExhaust: return FoundrySafetyExhaust;
                case MachineConditionKeys.FoundryAverageCondition:
                    return (FoundryRefractoryLining + FoundryHearthTuyeres + FoundrySandBeds
                            + FoundryStructuralSupports + FoundrySafetyExhaust) / 5f;
                case MachineConditionKeys.PowerFuelUnits: return PowerFuelUnits;
                case MachineConditionKeys.PowerBatteryReserve: return PowerBatteryReserve;
                case MachineConditionKeys.VentilationFilterSaturation: return VentilationFilterSaturation;
                case MachineConditionKeys.VentilationDuctIntegrity: return VentilationDuctIntegrity;
                case MachineConditionKeys.WaterFilterIntegrity: return WaterFilterIntegrity;
                case MachineConditionKeys.ThermalBoilerFuel: return ThermalBoilerFuel;
                case MachineConditionKeys.AirlockIncidentActive: return AirlockIncidentActive ? 1f : 0f;
                default: return null;
            }
        }

        /// <summary>Authoritative context gates (§29B.8). Unknown contexts never fire.</summary>
        public bool HasContext(string context)
        {
            if (string.IsNullOrEmpty(context)) return true;
            return string.Equals(context, "hazard_weather", StringComparison.Ordinal) && HazardWeather;
        }
    }

    /// <summary>Quirk kinds (§29B.9): diagnostic tells mean a real condition changed; personality tells are stable behaviour with no fault meaning.</summary>
    public static class MachineQuirkKinds
    {
        public const string Diagnostic = "diagnostic";
        public const string Personality = "personality";
    }

    /// <summary>Projection bands (§29B.5/§29C.8 shape). Presentation only — never persisted.</summary>
    public enum MachineConditionBand
    {
        Healthy = 0,
        Worn = 1,
        ServiceDue = 2,
        Critical = 3,
        Failed = 4
    }

    /// <summary>Root DTO for shelter_machine_identities.json (snake_case matches the data authority).</summary>
    [Serializable]
    public sealed class ShelterMachineCatalogData
    {
        public int schema_version = 1;
        public string collection_id = string.Empty;
        public List<MachineIdentityRecord> machines = new List<MachineIdentityRecord>();
        public List<MachineQuirkRecord> quirks = new List<MachineQuirkRecord>();
        public List<ShelterGlitchEvent>? glitch_events;
    }

    /// <summary>Identity record for one shelter machine (§29B.2). The owning system keeps the condition; this never mirrors it.</summary>
    [Serializable]
    public sealed class MachineIdentityRecord
    {
        /// <summary>Machine id, prefix machine_ (definition position).</summary>
        public string id = string.Empty;
        /// <summary>Runtime owner + field, e.g. "StartingLevelSystem.airFilterHealthPercent".</summary>
        public string condition_owner = string.Empty;
        public string display_name = string.Empty;
        /// <summary>Survivor nickname; empty when the machine stays technical (§29B.3–29B.4).</summary>
        public string nickname = string.Empty;
        /// <summary>Canonical room id the machine stands in (reference — must resolve).</summary>
        public string room_id = string.Empty;
        public string purpose = string.Empty;
        public string age_origin = string.Empty;
        /// <summary>Healthy-state sound description; basis of the machine's audio family.</summary>
        public string baseline_sound = string.Empty;
        /// <summary>Condition key the band projection reads (a MachineConditionKeys value).</summary>
        public string condition_key = string.Empty;
        public List<string> quirk_ids = new List<string>();
        public string audio_cue_family = string.Empty;
        /// <summary>Plan 26B specialty hook — empty until those capability APIs exist.</summary>
        public string maintenance_skill_hook = string.Empty;
        /// <summary>Beloved-machine failure beat hook (§29B.19); empty for the pilot.</summary>
        public string memorial_hook = string.Empty;
    }

    /// <summary>One machine tell (§29B.7). Diagnostic tells must sit on a threshold the owning system acts on.</summary>
    [Serializable]
    public sealed class MachineQuirkRecord
    {
        /// <summary>Quirk id, prefix machine_quirk_ (definition position).</summary>
        public string id = string.Empty;
        public string machine_id = string.Empty;
        /// <summary>diagnostic | personality (§29B.9).</summary>
        public string kind = MachineQuirkKinds.Diagnostic;
        /// <summary>Named condition the tell reads (MachineConditionKeys); empty for personality tells.</summary>
        public string condition_key = string.Empty;
        /// <summary>below (fires while condition < trigger_value) | above (fires while condition > trigger_value — e.g. saturation, absolute-quantity gates).</summary>
        public string comparison = "below";
        /// <summary>Diagnostic trigger value. Must equal the owner's own floor/ceiling.</summary>
        public float trigger_below = -1f;
        /// <summary>Optional authoritative context gate: "hazard_weather" or "" (§29B.8 — no hidden random conditions).</summary>
        public string context = string.Empty;
        public string text_cue = string.Empty;
        /// <summary>Host audio cue family (Plan 07B); semantics documented in PLAN29_AUDIO_HOOKS.md.</summary>
        public string audio_cue = string.Empty;
        /// <summary>info | warning | critical. Personality tells must be info (§29B.9 styling rule).</summary>
        public string severity = "info";
        /// <summary>The real maintenance action this tell leads to (§29B.14); required for diagnostic tells.</summary>
        public string maintenance_action = string.Empty;
        /// <summary>continuous (projection re-fires while the condition holds) | once_per_crossing (event layer, journal-owned).</summary>
        public string repeat_policy = "continuous";
    }

    /// <summary>
    /// One maintenance glitch event (§29B.11–29B.12): a bounded, deterministic
    /// narrative/diagnostic moment bound to a real machine. Eligibility is a
    /// threshold or context gate on the owning system — never arbitrary random
    /// damage. Resolution names the owning system's real API; kits reference
    /// existing items only (§13.3). One-shot/cooldown state lives in the journal
    /// knowledge store (glitch_noted_*) — no second save authority.
    /// </summary>
    [Serializable]
    public sealed class ShelterGlitchEvent
    {
        /// <summary>Glitch id, prefix glitch_ (definition position; numbering continues the canonical log).</summary>
        public string id = string.Empty;
        public string machine_id = string.Empty;
        /// <summary>harmless (explicitly non-diagnostic) | real_fault.</summary>
        public string kind = "real_fault";
        /// <summary>Optional log code tying this event to the engineering record (e.g. ENG-FL-162-GROUND).</summary>
        public string log_code = string.Empty;
        public string title = string.Empty;
        /// <summary>Eligibility: same condition/comparison model as tells; context-only diagnostics allowed.</summary>
        public string condition_key = string.Empty;
        public string comparison = "below";
        public float trigger_value = -1f;
        public string context = string.Empty;
        /// <summary>Diegetic presentation (log/notice voice).</summary>
        public string presentation = string.Empty;
        /// <summary>Player-facing resolution routed through the owner's real API.</summary>
        public string resolution = string.Empty;
        /// <summary>Repair kit — existing items.json ids only (§13.3).</summary>
        public List<string> repair_kit = new List<string>();
        /// <summary>Severity: info | warning | critical.</summary>
        public string severity = "warning";
        /// <summary>continuous re-notifies while eligible; once (one-shot) records a journal key so old saves never replay it.</summary>
        public string repeat_policy = "continuous";
        /// <summary>Minimum days between re-firings for continuous events (event-layer pacing, §29B.13).</summary>
        public int cooldown_days;
    }

    /// <summary>
    /// Machine identity + diagnostic tell catalog. Read-only projection over the
    /// owning systems' condition; missing file → empty valid catalog (overlay rule).
    /// Deterministic: same readings → same tells in authored order, no RNG, no
    /// dictionary-order exposure. Carries no condition state of its own (§1.2).
    /// </summary>
    public sealed class ShelterMachineTellCatalog
    {
        public const string FileName = "shelter_machine_identities.json";

        private readonly List<MachineIdentityRecord> _machines = new List<MachineIdentityRecord>();
        private readonly List<MachineQuirkRecord> _quirks = new List<MachineQuirkRecord>();
        private readonly Dictionary<string, MachineIdentityRecord> _machineById =
            new Dictionary<string, MachineIdentityRecord>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<MachineQuirkRecord>> _quirksByMachine =
            new Dictionary<string, List<MachineQuirkRecord>>(StringComparer.Ordinal);
        private readonly Dictionary<string, MachineQuirkRecord> _quirkById =
            new Dictionary<string, MachineQuirkRecord>(StringComparer.Ordinal);
        private readonly List<ShelterGlitchEvent> _glitchEvents = new List<ShelterGlitchEvent>();
        private readonly Dictionary<string, List<ShelterGlitchEvent>> _glitchesByMachine =
            new Dictionary<string, List<ShelterGlitchEvent>>(StringComparer.Ordinal);
        private readonly Dictionary<string, ShelterGlitchEvent> _glitchById =
            new Dictionary<string, ShelterGlitchEvent>(StringComparer.Ordinal);

        public IReadOnlyList<MachineIdentityRecord> Machines => _machines;
        public IReadOnlyList<MachineQuirkRecord> Quirks => _quirks;
        public IReadOnlyList<ShelterGlitchEvent> GlitchEvents => _glitchEvents;
        public int MachineCount => _machines.Count;

        /// <summary>Load from the data authority. Missing file → empty valid catalog; malformed → logged warning. Never throws.</summary>
        public static ShelterMachineTellCatalog Load(IFileIO files, IJsonSerializer json, string dataDirectory)
        {
            var catalog = new ShelterMachineTellCatalog();
            if (files == null || json == null || string.IsNullOrEmpty(dataDirectory)) return catalog;
            string path = System.IO.Path.Combine(dataDirectory, FileName);
            try
            {
                if (!files.FileExists(path)) return catalog;
                string raw = files.ReadAllText(path);
                var data = json.Deserialize<ShelterMachineCatalogData>(raw);
                if (data == null) return catalog;
                catalog.Build(data);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "ShelterMachineTellCatalog", ex);
            }
            return catalog;
        }

        private void Build(ShelterMachineCatalogData data)
        {
            _machines.Clear();
            _quirks.Clear();
            _machineById.Clear();
            _quirksByMachine.Clear();
            _quirkById.Clear();

            if (data.machines != null)
            {
                foreach (var machine in data.machines)
                {
                    if (machine == null || string.IsNullOrEmpty(machine.id)) continue;
                    _machines.Add(machine);
                    _machineById[machine.id] = machine;
                }
            }
            if (data.quirks != null)
            {
                foreach (var quirk in data.quirks)
                {
                    if (quirk == null || string.IsNullOrEmpty(quirk.id) || string.IsNullOrEmpty(quirk.machine_id)) continue;
                    _quirks.Add(quirk);
                    _quirkById[quirk.id] = quirk;
                    if (!_quirksByMachine.TryGetValue(quirk.machine_id, out var list))
                    {
                        list = new List<MachineQuirkRecord>();
                        _quirksByMachine[quirk.machine_id] = list;
                    }
                    list.Add(quirk);
                }
            }
            if (data.glitch_events != null)
            {
                foreach (var glitch in data.glitch_events)
                {
                    if (glitch == null || string.IsNullOrEmpty(glitch.id) || string.IsNullOrEmpty(glitch.machine_id)) continue;
                    _glitchEvents.Add(glitch);
                    if (!_glitchesByMachine.TryGetValue(glitch.machine_id, out var list))
                    {
                        list = new List<ShelterGlitchEvent>();
                        _glitchesByMachine[glitch.machine_id] = list;
                    }
                    list.Add(glitch);
                }
            }
        }

        /// <summary>Glitch events bound to one machine, authored order.</summary>
        public IReadOnlyList<ShelterGlitchEvent> GetGlitchEventsForMachine(string machineId)
        {
            if (string.IsNullOrEmpty(machineId)) return Array.Empty<ShelterGlitchEvent>();
            return _glitchesByMachine.TryGetValue(machineId, out var list) ? list : Array.Empty<ShelterGlitchEvent>();
        }

        /// <summary>Glitch event by id, or null.</summary>
        public ShelterGlitchEvent? GetGlitchEvent(string glitchId)
        {
            if (string.IsNullOrEmpty(glitchId)) return null;
            for (int i = 0; i < _glitchEvents.Count; i++)
                if (string.Equals(_glitchEvents[i].id, glitchId, StringComparison.Ordinal))
                    return _glitchEvents[i];
            return null;
        }

        /// <summary>
        /// Deterministic glitch-event eligibility (§29B.12): threshold/context gate on
        /// the owning system, never random. One-shot events already noted in the
        /// journal (via the host-supplied predicate) never replay — old saves default
        /// un-noted and reveal once. Continuous events re-fire on their cooldown,
        /// paced by the caller's day bookkeeping (§29B.13).
        /// </summary>
        public IReadOnlyList<ShelterGlitchEvent> EvaluateGlitchEvents(
            string machineId, MachineConditionReadings readings, Func<string, bool>? isNoted = null)
        {
            var machine = GetMachine(machineId);
            if (readings == null || machine == null || !_glitchesByMachine.TryGetValue(machine.id, out var events))
                return Array.Empty<ShelterGlitchEvent>();

            List<ShelterGlitchEvent>? result = null;
            for (int i = 0; i < events.Count; i++)
            {
                var glitch = events[i];
                if (!GlitchEligible(glitch, readings)) continue;
                if (string.Equals(glitch.repeat_policy, "once", StringComparison.Ordinal) &&
                    isNoted != null && isNoted(glitch.id))
                    continue; // journal-owned one-shot state: never replayed

                (result ??= new List<ShelterGlitchEvent>()).Add(glitch);
            }
            return result ?? (IReadOnlyList<ShelterGlitchEvent>)Array.Empty<ShelterGlitchEvent>();
        }

        private static bool GlitchEligible(ShelterGlitchEvent glitch, MachineConditionReadings readings)
        {
            if (readings == null) return false;
            if (string.Equals(glitch.kind, "harmless", StringComparison.Ordinal) &&
                string.IsNullOrWhiteSpace(glitch.condition_key) && string.IsNullOrWhiteSpace(glitch.context))
                return true; // explicitly harmless flavour has no mechanical gate
            if (!string.IsNullOrWhiteSpace(glitch.context) && !readings.HasContext(glitch.context)) return false;
            if (string.IsNullOrWhiteSpace(glitch.condition_key)) return true;
            float? value = readings.Get(glitch.condition_key);
            if (!value.HasValue) return false;
            bool above = string.Equals(glitch.comparison, "above", StringComparison.Ordinal);
            return above ? value.Value > glitch.trigger_value : value.Value < glitch.trigger_value;
        }

        public MachineIdentityRecord? GetMachine(string machineId)
        {
            if (string.IsNullOrEmpty(machineId)) return null;
            return _machineById.TryGetValue(machineId, out var machine) ? machine : null;
        }

        public MachineQuirkRecord? GetQuirk(string quirkId)
        {
            if (string.IsNullOrEmpty(quirkId)) return null;
            return _quirkById.TryGetValue(quirkId, out var quirk) ? quirk : null;
        }

        public IReadOnlyList<MachineQuirkRecord> GetQuirksForMachine(string machineId)
        {
            if (string.IsNullOrEmpty(machineId)) return Array.Empty<MachineQuirkRecord>();
            return _quirksByMachine.TryGetValue(machineId, out var list) ? list : Array.Empty<MachineQuirkRecord>();
        }

        /// <summary>
        /// Deterministic tell evaluation (§12): a diagnostic tell fires while its named
        /// condition is below its authored threshold; a personality tell fires whenever
        /// its stable behaviour applies. Unknown machines yield nothing; missing
        /// conditions never invent tells (§13.2). Same readings → same tells, authored order.
        /// </summary>
        public IReadOnlyList<MachineQuirkRecord> EvaluateQuirks(string machineId, MachineConditionReadings readings)
        {
            if (readings == null || !_quirksByMachine.TryGetValue(machineId ?? string.Empty, out var quirks))
                return Array.Empty<MachineQuirkRecord>();

            List<MachineQuirkRecord>? result = null;
            for (int i = 0; i < quirks.Count; i++)
            {
                if (QuirkFires(quirks[i], readings))
                    (result ??= new List<MachineQuirkRecord>()).Add(quirks[i]);
            }
            return result ?? (IReadOnlyList<MachineQuirkRecord>)Array.Empty<MachineQuirkRecord>();
        }

        /// <summary>
        /// Condition band for one machine from the same readings the tells use.
        /// HEPA bands key on filter health (owner warns at 50); foundry bands key on
        /// the owner's own average facility condition.
        /// </summary>
        public MachineConditionBand EvaluateBand(string machineId, MachineConditionReadings readings)
        {
            var machine = GetMachine(machineId);
            if (machine == null || string.IsNullOrWhiteSpace(machine.condition_key) || readings == null)
                return MachineConditionBand.Healthy;
            float? value = readings.Get(machine.condition_key);
            return BandFor(value ?? 100f);
        }

        /// <summary>
        /// Band thresholds (§29C.8 shape). Floors mirror the owners: 50 = StartingLevelSystem's
        /// air-hazard warning floor; 25 = the foundry's critical warning floor. Bands are
        /// presentation only — the condition stays owned by the machine systems (§1.2).
        /// </summary>
        public static MachineConditionBand BandFor(float condition)
        {
            if (condition <= 0f) return MachineConditionBand.Failed;
            if (condition < 25f) return MachineConditionBand.Critical;
            if (condition < 50f) return MachineConditionBand.ServiceDue;
            if (condition < 70f) return MachineConditionBand.Worn;
            return MachineConditionBand.Healthy;
        }

        private static bool QuirkFires(MachineQuirkRecord quirk, MachineConditionReadings readings)
        {
            if (string.Equals(quirk.kind, MachineQuirkKinds.Personality, StringComparison.Ordinal))
                return true; // stable behaviour, not threshold-bound
            if (!readings.HasContext(quirk.context)) return false;
            if (string.IsNullOrWhiteSpace(quirk.condition_key)) return false;
            float? value = readings.Get(quirk.condition_key);
            if (!value.HasValue) return false;
            bool isAbove = string.Equals(quirk.comparison, "above", StringComparison.Ordinal);
            return isAbove ? value.Value > quirk.trigger_below : value.Value < quirk.trigger_below;
        }

        /// <summary>
        /// Contract validation (§29B.7 + §13). Errors: duplicate ids, unknown machine
        /// or condition-key references, family mismatch, missing text/maintenance
        /// action, personality styling violations, unknown severity/repeat policy,
        /// orphan quirks, and mismatched machine.quirk_ids listings.
        /// </summary>
        public List<string> Validate()
        {
            var errors = new List<string>();
            var seenMachines = new HashSet<string>(StringComparer.Ordinal);
            var seenQuirks = new HashSet<string>(StringComparer.Ordinal);
            var referencedQuirks = new HashSet<string>(StringComparer.Ordinal);

            for (int i = 0; i < _machines.Count; i++)
            {
                var machine = _machines[i];
                if (!seenMachines.Add(machine.id))
                    errors.Add($"duplicate machine id '{machine.id}'");
                if (string.IsNullOrWhiteSpace(machine.display_name))
                    errors.Add($"machine '{machine.id}' missing display_name");
                if (string.IsNullOrWhiteSpace(machine.condition_owner))
                    errors.Add($"machine '{machine.id}' missing condition_owner");
                if (string.IsNullOrWhiteSpace(machine.room_id))
                    errors.Add($"machine '{machine.id}' missing room_id");
                if (string.IsNullOrWhiteSpace(machine.condition_key) ||
                    Array.IndexOf(MachineConditionKeys.All, machine.condition_key) < 0)
                    errors.Add($"machine '{machine.id}' has unknown condition_key '{machine.condition_key}'");
                if (string.IsNullOrWhiteSpace(machine.audio_cue_family))
                    errors.Add($"machine '{machine.id}' missing audio_cue_family");
            }

            for (int i = 0; i < _quirks.Count; i++)
            {
                var quirk = _quirks[i];
                if (!seenQuirks.Add(quirk.id))
                    errors.Add($"duplicate quirk id '{quirk.id}'");
                bool diagnostic = string.Equals(quirk.kind, MachineQuirkKinds.Diagnostic, StringComparison.Ordinal);
                bool personality = string.Equals(quirk.kind, MachineQuirkKinds.Personality, StringComparison.Ordinal);

                if (string.IsNullOrWhiteSpace(quirk.text_cue))
                    errors.Add($"quirk '{quirk.id}' missing text_cue");
                if (quirk.repeat_policy != "continuous" && quirk.repeat_policy != "once_per_crossing")
                    errors.Add($"quirk '{quirk.id}' has unsupported repeat_policy '{quirk.repeat_policy}'");

                if (diagnostic)
                {
                    if (quirk.trigger_below <= 0f || quirk.trigger_below > 100f)
                        errors.Add($"diagnostic quirk '{quirk.id}' trigger_below must be in (0, 100]");
                    if (string.IsNullOrWhiteSpace(quirk.condition_key))
                    {
                        errors.Add($"diagnostic quirk '{quirk.id}' missing condition_key");
                    }
                    else
                    {
                        if (Array.IndexOf(MachineConditionKeys.All, quirk.condition_key) < 0)
                            errors.Add($"diagnostic quirk '{quirk.id}' reads unknown condition '{quirk.condition_key}'");
                        else
                        {
                            var quirkMachine = GetMachine(quirk.machine_id);
                            string family = quirkMachine != null && !string.IsNullOrWhiteSpace(quirkMachine.condition_key)
                                ? quirkMachine.condition_key.Split('.')[0]
                                : MachineConditionKeys.FamilyOf(quirk.machine_id);
                            if (!quirk.condition_key.StartsWith(family + ".", StringComparison.Ordinal))
                                errors.Add($"diagnostic quirk '{quirk.id}' reads '{quirk.condition_key}' outside its machine family '{family}'");
                        }
                        if (string.IsNullOrWhiteSpace(quirk.maintenance_action))
                            errors.Add($"diagnostic quirk '{quirk.id}' missing maintenance_action (§29B.14: a tell must lead to a real action)");
                    }
                }
                else if (personality)
                {
                    if (!string.IsNullOrWhiteSpace(quirk.condition_key))
                        errors.Add($"personality quirk '{quirk.id}' must not bind a condition key (§29B.9)");
                    if (!string.Equals(quirk.severity, "info", StringComparison.Ordinal))
                        errors.Add($"personality quirk '{quirk.id}' must use severity 'info' (§29B.9 styling rule)");
                }
                else
                {
                    errors.Add($"quirk '{quirk.id}' has unsupported kind '{quirk.kind}'");
                }

                if (!string.Equals(quirk.severity, "info", StringComparison.Ordinal) &&
                    !string.Equals(quirk.severity, "warning", StringComparison.Ordinal) &&
                    !string.Equals(quirk.severity, "critical", StringComparison.Ordinal))
                    errors.Add($"quirk '{quirk.id}' has unsupported severity '{quirk.severity}'");
                if (string.IsNullOrWhiteSpace(quirk.text_cue))
                    errors.Add($"quirk '{quirk.id}' missing text_cue");
                if (quirk.repeat_policy != "continuous" && quirk.repeat_policy != "once_per_crossing")
                    errors.Add($"quirk '{quirk.id}' has unsupported repeat_policy '{quirk.repeat_policy}'");
            }

            for (int i = 0; i < _machines.Count; i++)
            {
                var machine = _machines[i];
                if (machine.quirk_ids == null) continue;
                foreach (var quirkId in machine.quirk_ids)
                {
                    if (string.IsNullOrEmpty(quirkId)) continue;
                    referencedQuirks.Add(quirkId);
                    var quirk = GetQuirk(quirkId);
                    if (quirk == null)
                        errors.Add($"machine '{machine.id}' references unknown quirk '{quirkId}'");
                    else if (!string.Equals(quirk.machine_id, machine.id, StringComparison.Ordinal))
                        errors.Add($"quirk '{quirkId}' listed by machine '{machine.id}' but owned by '{quirk.machine_id}'");
                }
            }
            for (int i = 0; i < _quirks.Count; i++)
            {
                if (!referencedQuirks.Contains(_quirks[i].id))
                    errors.Add($"quirk '{_quirks[i].id}' is not listed by its machine (orphan)");
                if (GetMachine(_quirks[i].machine_id) == null)
                    errors.Add($"quirk '{_quirks[i].id}' references unknown machine '{_quirks[i].machine_id}'");
            }

            // ── Glitch events (§29B.11–29B.12) ──
            var seenGlitches = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < _glitchEvents.Count; i++)
            {
                var glitch = _glitchEvents[i];
                if (!seenGlitches.Add(glitch.id))
                    errors.Add($"duplicate glitch id '{glitch.id}'");
                if (GetMachine(glitch.machine_id) == null)
                    errors.Add($"glitch '{glitch.id}' references unknown machine '{glitch.machine_id}'");
                if (string.IsNullOrWhiteSpace(glitch.title))
                    errors.Add($"glitch '{glitch.id}' missing title");
                if (string.IsNullOrWhiteSpace(glitch.presentation))
                    errors.Add($"glitch '{glitch.id}' missing presentation");
                bool harmless = string.Equals(glitch.kind, "harmless", StringComparison.Ordinal);
                if (!harmless && !string.Equals(glitch.kind, "real_fault", StringComparison.Ordinal))
                    errors.Add($"glitch '{glitch.id}' has unsupported kind '{glitch.kind}'");
                if (!harmless)
                {
                    if (string.IsNullOrWhiteSpace(glitch.condition_key) && string.IsNullOrWhiteSpace(glitch.context))
                        errors.Add($"real-fault glitch '{glitch.id}' needs a condition or context gate (a gated-less real-fault warning lies)");
                    if (string.IsNullOrWhiteSpace(glitch.resolution))
                        errors.Add($"real-fault glitch '{glitch.id}' missing resolution (must route through the owner API, §29B.12)");
                }
                if (string.IsNullOrWhiteSpace(glitch.repeat_policy) ||
                    (glitch.repeat_policy != "continuous" && glitch.repeat_policy != "once"))
                    errors.Add($"glitch '{glitch.id}' has unsupported repeat_policy '{glitch.repeat_policy}'");
                if (!string.IsNullOrWhiteSpace(glitch.condition_key) && glitch.trigger_value < 0f)
                    errors.Add($"glitch '{glitch.id}' has a condition gate without a trigger value");
                if (glitch.cooldown_days < 0)
                    errors.Add($"glitch '{glitch.id}' has a negative cooldown");
            }

            return errors;
        }
    }
}
