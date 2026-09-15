// SPDX-License-Identifier: MIT
// ============================================================================
// System     : DefenseSystem (Plan 163 — Dynamic Settlement Defenses & Traps)
// Layer      : Pre-combat raid defense authority COMPOSING the existing
//              PerimeterDefenseSystem (emplacements/turrets own by it stay
//              there). This system owns: trap installations (armed/sprung/
//              broken with reset ≠ repair), capture outcomes, the structured
//              raid log, and the perimeter-strength breakdown.
// Handoff    : captures raise OnRaiderCaptured; the host calls
//              PrisonerSystem.TakePrisoner (single captive authority).
//              Raiders that survive the static phase escalate to
//              TacticalCombatSystem through the host raid seam.
// RNG        : injected per engagement (host forks defense.targeting /
//              defense.capture per raid) — Core stores no RNG state.
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Defense
{
    [Serializable]
    public sealed class DefenseTrapDefinition
    {
        public string id { get; set; } = string.Empty;              // trap_*
        public string display_name { get; set; } = string.Empty;
        public string defense_type { get; set; } = "snare";          // snare | deadfall | capture_pit | spike_border
        public int base_strength { get; set; } = 1;                  // raiders neutralized when sprung
        public int max_hp { get; set; } = 60;
        public float activation_chance { get; set; } = 0.75f;
        public float capture_chance { get; set; }                    // 0..1, only for incapacitating traps
        public bool concealed { get; set; }
        public string placement_tag { get; set; } = "perimeter";
        public Dictionary<string, int> build_costs { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public Dictionary<string, int> reset_costs { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public Dictionary<string, int> repair_costs { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class TrapInstallationState
    {
        public string installation_id { get; set; } = string.Empty;
        public string trap_id { get; set; } = string.Empty;
        public string placement_id { get; set; } = string.Empty;
        public int current_hp { get; set; }
        public int max_hp { get; set; }
        public bool armed { get; set; } = true;
        public bool sprung { get; set; }
        public bool broken { get; set; }
        public string? manned_by { get; set; }                       // optional survivor id
        public int last_activation_day { get; set; } = -1;
        public int total_activations { get; set; }
        public int total_captures { get; set; }
    }

    /// <summary>Structured raid-log entry; the narrative system formats text.</summary>
    [Serializable]
    public sealed class DefenseActivationRecord
    {
        public int day { get; set; }
        public string installation_id { get; set; } = string.Empty;
        public string defense_type { get; set; } = string.Empty;
        public string target { get; set; } = "raider";               // game abstraction, never a spawn coordinate
        public string outcome { get; set; } = "";                    // sprung_captured | sprung_killed | missed | broken
        public int raiders_neutralized { get; set; }
    }

    [Serializable]
    public sealed class DefenseSystemState
    {
        public string system_id { get; set; } = "settlement_defenses";
        public int schema_version { get; set; } = 1;
        public int last_tick_day { get; set; }
        public List<TrapInstallationState> installations { get; set; } = new List<TrapInstallationState>();
        public List<DefenseActivationRecord> raid_log { get; set; } = new List<DefenseActivationRecord>();
    }

    /// <summary>Perimeter strength with a breakdown — never one opaque number.</summary>
    public sealed class PerimeterStrengthBreakdown
    {
        public float Walls;
        public int Traps;
        public int Turrets;
        public int Power;             // powered emplacements count (0 if grid query absent)
        public int Manning;           // armed traps with an assigned survivor
        public float DamagePenalties; // HP-integrity penalties across installations
        public int Total => Math.Max(0, (int)Math.Round(Walls + Traps + Turrets + Power + Manning - DamagePenalties));
    }

    public sealed class DefenseEngagementResult
    {
        public int Day;
        public int InitialRaiderStrength;
        public int RaidersNeutralizedByTraps;
        public int RaidersCaptured;
        public AssaultSimulationResult? PerimeterResult;   // null when no emplacements engaged
        public int RemainingRaiders;
        public bool Repelled;
        public bool Breached;
        public List<DefenseActivationRecord> Records { get; } = new List<DefenseActivationRecord>();
    }

    public sealed class DefenseSystem
    {
        public const string SystemId = "settlement_defenses";
        public const int RaidLogCapacity = 50;
        /// <summary>HP a trap loses when it springs (before retaliation).</summary>
        public const int TrapSelfDamagePerSpring = 10;

        private readonly List<DefenseTrapDefinition> _trapDefs = new List<DefenseTrapDefinition>();
        private readonly Dictionary<string, DefenseTrapDefinition> _trapsById =
            new Dictionary<string, DefenseTrapDefinition>(StringComparer.Ordinal);
        private readonly DefenseSystemState _state = new DefenseSystemState();
        private int _installationCounter;

        public event Action<string, string>? OnTrapSprung;            // installationId, trapDefinitionId
        public event Action<TrapInstallationState>? OnTrapBroken;
        public event Action<int, int>? OnRaiderCaptured;              // day, count — host hands off to PrisonerSystem
        public event Action<DefenseEngagementResult>? OnRaidResolved;

        public DefenseSystem(IEnumerable<DefenseTrapDefinition>? trapDefinitions)
        {
            if (trapDefinitions == null) return;
            foreach (var def in trapDefinitions)
            {
                if (def == null || string.IsNullOrEmpty(def.id)) continue;
                _trapDefs.Add(def);
                _trapsById[def.id] = def;
            }
        }

        public string SaveId => SystemId;
        public DefenseSystemState State => _state;
        public IReadOnlyList<DefenseTrapDefinition> TrapDefinitions => _trapDefs;
        public IReadOnlyList<TrapInstallationState> Installations => _state.installations;
        public IReadOnlyList<DefenseActivationRecord> RaidLog => _state.raid_log;

        public DefenseTrapDefinition? FindTrapDefinition(string trapId) =>
            _trapsById.TryGetValue(trapId, out var def) ? def : null;

        public TrapInstallationState? FindInstallation(string installationId)
        {
            for (int i = 0; i < _state.installations.Count; i++)
                if (string.Equals(_state.installations[i].installation_id, installationId, StringComparison.Ordinal))
                    return _state.installations[i];
            return null;
        }

        // ------------------------------------------------------------------
        // Installation / reset / repair (costs consumed by the host first)
        // ------------------------------------------------------------------

        public bool CanInstall(string trapId) => FindTrapDefinition(trapId) != null;

        public bool InstallTrap(
            string trapId,
            string placementId,
            Func<string, int, bool> consumeItems)
        {
            var def = FindTrapDefinition(trapId);
            if (def == null || consumeItems == null) return false;
            foreach (var cost in def.build_costs)
                if (!consumeItems(cost.Key, cost.Value)) return false;

            _installationCounter++;
            var inst = new TrapInstallationState
            {
                installation_id = $"trap_{_installationCounter}_{def.id}",
                trap_id = def.id,
                placement_id = placementId,
                current_hp = def.max_hp,
                max_hp = def.max_hp,
                armed = true,
                sprung = false,
                broken = false
            };
            _state.installations.Add(inst);
            _state.installations.Sort((a, b) => string.CompareOrdinal(a.installation_id, b.installation_id));
            return true;
        }

        /// <summary>Reset: return a functional SPRUNG trap to armed. A broken
        /// trap must be repaired first (reset and repair are separate states
        /// and separate actions, plan §6.12).</summary>
        public bool CanResetTrap(string installationId)
        {
            var inst = FindInstallation(installationId);
            return inst != null && inst.sprung && !inst.broken;
        }

        public bool TryResetTrap(string installationId, Func<string, int, bool> consumeItems)
        {
            var inst = FindInstallation(installationId);
            if (inst == null || !CanResetTrap(installationId)) return false;
            var def = FindTrapDefinition(inst.trap_id);
            if (def == null || consumeItems == null) return false;
            foreach (var cost in def.reset_costs)
                if (!consumeItems(cost.Key, cost.Value)) return false;
            inst.sprung = false;
            inst.armed = true;
            return true;
        }

        /// <summary>Repair: restore broken/damaged hardware. Does not arm —
        /// a repaired trap still needs reset if it was sprung.</summary>
        public bool CanRepairTrap(string installationId)
        {
            var inst = FindInstallation(installationId);
            return inst != null && (inst.broken || inst.current_hp < inst.max_hp);
        }

        public bool TryRepairTrap(string installationId, Func<string, int, bool> consumeItems)
        {
            var inst = FindInstallation(installationId);
            if (inst == null || !CanRepairTrap(installationId)) return false;
            var def = FindTrapDefinition(inst.trap_id);
            if (def == null || consumeItems == null) return false;
            foreach (var cost in def.repair_costs)
                if (!consumeItems(cost.Key, cost.Value)) return false;
            inst.current_hp = inst.max_hp;
            inst.broken = false;
            return true;
        }

        public void AssignManning(string installationId, string? survivorId)
        {
            var inst = FindInstallation(installationId);
            if (inst != null) inst.manned_by = survivorId;
        }

        // ------------------------------------------------------------------
        // Perimeter strength (composed breakdown, plan §6.6)
        // ------------------------------------------------------------------

        public PerimeterStrengthBreakdown CalculatePerimeterStrength(
            PerimeterDefenseSystem? perimeter,
            Func<string, bool>? isEmplacementPowered = null)
        {
            var b = new PerimeterStrengthBreakdown();
            foreach (var inst in _state.installations)
            {
                if (inst.broken) { b.DamagePenalties += 2; continue; }
                if (inst.armed && !inst.sprung)
                {
                    var def = FindTrapDefinition(inst.trap_id);
                    b.Traps += def?.base_strength ?? 1;
                    if (!string.IsNullOrEmpty(inst.manned_by)) b.Manning += 1;
                }
                else
                {
                    b.DamagePenalties += 1; // sprung/unarmed hardware adds nothing
                }
                b.DamagePenalties += (inst.max_hp - inst.current_hp) / 30;
            }

            if (perimeter != null)
            {
                foreach (var emp in perimeter.Emplacements)
                {
                    if (emp.is_destroyed || !emp.is_active) { b.DamagePenalties += 2; continue; }
                    var def = perimeter.FindDefinition(emp.defense_id);
                    if (def == null) continue;
                    if (def.defense_type == "automated_turret")
                    {
                        b.Turrets += 1;
                        if (def.power_draw_watts > 0 && isEmplacementPowered != null
                            && isEmplacementPowered(emp.emplacement_id))
                            b.Power += 1;
                    }
                    else
                    {
                        b.Walls += 1 + emp.current_hp / (float)Math.Max(1, emp.max_hp);
                    }
                    b.DamagePenalties += (emp.max_hp - emp.current_hp) / (float)Math.Max(1, emp.max_hp);
                }
            }
            return b;
        }

        // ------------------------------------------------------------------
        // Pre-combat raid resolution (plan §6.7 ordering)
        // ------------------------------------------------------------------

        /// <summary>
        /// Static-defense phase: traps resolve first (targeting then capture
        /// rolls from the injected streams), then surviving raiders meet the
        /// perimeter emplacements (PerimeterDefenseSystem), and only raiders
        /// that BREACH are returned for direct survivor combat.
        /// </summary>
        public DefenseEngagementResult ResolvePreCombatRaid(
            int day,
            int raiderStrength,
            bool isNight,
            PerimeterDefenseSystem? perimeter,
            Func<string, bool>? isEmplacementPowered,
            ISeededRng targetingRng,
            ISeededRng captureRng,
            float guardNightDetection = 0f)
        {
            var result = new DefenseEngagementResult
            {
                Day = day,
                InitialRaiderStrength = raiderStrength,
                RemainingRaiders = raiderStrength
            };
            if (raiderStrength <= 0)
            {
                result.Repelled = true;
                return result;
            }

            // 1. Traps (stable installation order).
            foreach (var inst in _state.installations)
            {
                if (result.RemainingRaiders <= 0) break;
                if (inst.broken || inst.sprung || !inst.armed) continue;
                var def = FindTrapDefinition(inst.trap_id);
                if (def == null) continue;

                // Concealed traps keep their full activation chance at night;
                // exposed traps are less reliable in the dark. Plan 174: a
                // guard animal's night senses offset that penalty (0 = legacy
                // behavior, 1 = a fully effective guard erases it).
                float nightFactor = isNight && !def.concealed
                    ? Math.Clamp(0.75f + 0.25f * Math.Clamp(guardNightDetection, 0f, 1f), 0.75f, 1f)
                    : 1f;
                float activation = def.activation_chance * nightFactor;
                bool sprung = targetingRng == null || targetingRng.NextDouble() < activation;

                var record = new DefenseActivationRecord
                {
                    day = day,
                    installation_id = inst.installation_id,
                    defense_type = def.defense_type
                };

                if (sprung)
                {
                    inst.sprung = true;
                    inst.armed = false;
                    inst.last_activation_day = day;
                    inst.total_activations++;
                    OnTrapSprung?.Invoke(inst.installation_id, inst.trap_id);

                    bool captured = def.capture_chance > 0f
                                    && captureRng != null
                                    && captureRng.NextDouble() < def.capture_chance;
                    if (captured)
                    {
                        inst.total_captures++;
                        result.RaidersCaptured++;
                        record.outcome = "sprung_captured";
                    }
                    else
                    {
                        record.outcome = "sprung_killed";
                    }
                    record.raiders_neutralized = def.base_strength;
                    result.RaidersNeutralizedByTraps += def.base_strength;
                    result.RemainingRaiders = Math.Max(0, result.RemainingRaiders - def.base_strength);

                    // Springing damages the trap; badly worn traps break.
                    inst.current_hp = Math.Max(0, inst.current_hp - TrapSelfDamagePerSpring - def.base_strength * 5);
                    if (inst.current_hp <= 0)
                    {
                        inst.broken = true;
                        record.outcome = captured ? "sprung_captured" : "broken";
                        OnTrapBroken?.Invoke(inst);
                    }
                }
                else
                {
                    record.outcome = "missed";
                }
                result.Records.Add(record);
            }

            // 2. Perimeter emplacements engage what is left.
            if (result.RemainingRaiders > 0 && perimeter != null)
            {
                var assault = perimeter.SimulateRaiderAssault(
                    result.RemainingRaiders, isNight, isEmplacementPowered, currentDay: day);
                result.PerimeterResult = assault;
                result.RemainingRaiders = assault.RemainingRaiderStrength;
                result.Breached = assault.Breached;
                result.Repelled = assault.Repelled;
            }
            else if (result.RemainingRaiders <= 0)
            {
                result.Repelled = true;
            }

            // 3. Captures hand off (host → PrisonerSystem.TakePrisoner).
            if (result.RaidersCaptured > 0)
                OnRaiderCaptured?.Invoke(day, result.RaidersCaptured);

            // 4. Bounded structured raid log.
            foreach (var r in result.Records)
            {
                _state.raid_log.Add(r);
            }
            if (_state.raid_log.Count > RaidLogCapacity)
                _state.raid_log.RemoveRange(0, _state.raid_log.Count - RaidLogCapacity);

            OnRaidResolved?.Invoke(result);
            return result;
        }

        // ------------------------------------------------------------------
        // Daily passive tick (wear bookkeeping only; no RNG)
        // ------------------------------------------------------------------

        public void TickDay(int day)
        {
            if (_state.last_tick_day == day) return;
            _state.last_tick_day = day;
        }

        // ------------------------------------------------------------------
        // Save
        // ------------------------------------------------------------------

        public DefenseSystemState CaptureState()
        {
            var copy = new DefenseSystemState
            {
                last_tick_day = _state.last_tick_day,
                installations = new List<TrapInstallationState>(_state.installations.Count),
                raid_log = new List<DefenseActivationRecord>(_state.raid_log.Count)
            };
            foreach (var i in _state.installations)
                copy.installations.Add(new TrapInstallationState
                {
                    installation_id = i.installation_id,
                    trap_id = i.trap_id,
                    placement_id = i.placement_id,
                    current_hp = i.current_hp,
                    max_hp = i.max_hp,
                    armed = i.armed,
                    sprung = i.sprung,
                    broken = i.broken,
                    manned_by = i.manned_by,
                    last_activation_day = i.last_activation_day,
                    total_activations = i.total_activations,
                    total_captures = i.total_captures
                });
            foreach (var r in _state.raid_log)
                copy.raid_log.Add(new DefenseActivationRecord
                {
                    day = r.day,
                    installation_id = r.installation_id,
                    defense_type = r.defense_type,
                    target = r.target,
                    outcome = r.outcome,
                    raiders_neutralized = r.raiders_neutralized
                });
            return copy;
        }

        public void RestoreState(DefenseSystemState? state)
        {
            if (state == null) return;
            _state.last_tick_day = state.last_tick_day;
            _state.installations = new List<TrapInstallationState>(state.installations?.Count ?? 0);
            if (state.installations != null)
                foreach (var i in state.installations)
                    _state.installations.Add(new TrapInstallationState
                    {
                        installation_id = i.installation_id,
                        trap_id = i.trap_id,
                        placement_id = i.placement_id,
                        current_hp = i.current_hp,
                        max_hp = i.max_hp,
                        armed = i.armed,
                        sprung = i.sprung,
                        broken = i.broken,
                        manned_by = i.manned_by,
                        last_activation_day = i.last_activation_day,
                        total_activations = i.total_activations,
                        total_captures = i.total_captures
                    });
            _state.raid_log = new List<DefenseActivationRecord>(state.raid_log?.Count ?? 0);
            if (state.raid_log != null)
                foreach (var r in state.raid_log)
                    _state.raid_log.Add(new DefenseActivationRecord
                    {
                        day = r.day,
                        installation_id = r.installation_id,
                        defense_type = r.defense_type,
                        target = r.target,
                        outcome = r.outcome,
                        raiders_neutralized = r.raiders_neutralized
                    });

            // Counter continues past restored ids (format: trap_{n}_{defId}).
            foreach (var i in _state.installations)
            {
                if (string.IsNullOrEmpty(i.installation_id)) continue;
                int secondUnderscore = i.installation_id.IndexOf('_', 5);
                if (secondUnderscore <= 5) continue;
                if (int.TryParse(i.installation_id.Substring(5, secondUnderscore - 5), out var n) && n > _installationCounter)
                    _installationCounter = n;
            }
        }
    }

    [Serializable]
    public sealed class TrapCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<DefenseTrapDefinition> traps { get; set; } = new List<DefenseTrapDefinition>();
    }

    /// <summary>Loads defenses.json (the trap layer; emplacements stay in
    /// perimeter_defenses.json) and validates it deterministically.</summary>
    public static class TrapCatalogLoader
    {
        public const string DefaultFileName = "defenses.json";

        public static List<DefenseTrapDefinition> Load(
            string dataDir,
            Ashfall.Core.IFileIO? fileIO = null,
            Ashfall.Core.IJsonSerializer? json = null)
        {
            fileIO ??= new Ashfall.Core.FileSystemIO();
            json ??= new Ashfall.Core.SystemTextJsonSerializer();
            var path = System.IO.Path.Combine(dataDir ?? string.Empty, DefaultFileName);
            if (!fileIO.FileExists(path)) return new List<DefenseTrapDefinition>();
            try
            {
                var text = fileIO.ReadAllText(path);
                var container = json.Deserialize<TrapCatalogContainer>(text);
                return container?.traps ?? new List<DefenseTrapDefinition>();
            }
            catch (Exception)
            {
                return new List<DefenseTrapDefinition>();
            }
        }

        public static List<string> Validate(IEnumerable<DefenseTrapDefinition>? traps)
        {
            var diags = new List<string>();
            if (traps == null) return diags;
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var t in traps)
            {
                if (t == null) continue;
                if (string.IsNullOrEmpty(t.id) || !t.id.StartsWith("trap_", StringComparison.Ordinal))
                    diags.Add($"{t.id}: id must use the trap_ prefix");
                else if (!seen.Add(t.id))
                    diags.Add($"{t.id}: duplicate trap id");
                if (t.base_strength < 0) diags.Add($"{t.id}: base_strength must be >= 0");
                if (t.max_hp <= 0) diags.Add($"{t.id}: max_hp must be > 0");
                if (t.activation_chance < 0f || t.activation_chance > 1f)
                    diags.Add($"{t.id}: activation_chance must be within [0,1]");
                if (t.capture_chance < 0f || t.capture_chance > 1f)
                    diags.Add($"{t.id}: capture_chance must be within [0,1]");
                foreach (var cost in t.build_costs)
                    if (cost.Value < 0) diags.Add($"{t.id}: negative build cost {cost.Key}");
                foreach (var cost in t.reset_costs)
                    if (cost.Value < 0) diags.Add($"{t.id}: negative reset cost {cost.Key}");
                foreach (var cost in t.repair_costs)
                    if (cost.Value < 0) diags.Add($"{t.id}: negative repair cost {cost.Key}");
            }
            diags.Sort(StringComparer.Ordinal);
            return diags;
        }
    }
}
