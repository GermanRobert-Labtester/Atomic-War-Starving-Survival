// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Defense
{
    [Serializable]
    public sealed class EmplacementRuntimeState
    {
        public string emplacement_id { get; set; } = string.Empty;
        public string defense_id { get; set; } = string.Empty;
        public int current_hp { get; set; }
        public int max_hp { get; set; }
        public bool is_active { get; set; } = true;
        public bool is_destroyed { get; set; }
        public int loaded_ammo_count { get; set; }
        public string required_ammo_type { get; set; } = string.Empty;
        public int magazine_capacity { get; set; }
        public float barrel_wear_percent { get; set; } // 0 - 100% (2% per 50 rounds)
        public bool is_jammed { get; set; }
    }

    [Serializable]
    public sealed class PerimeterDefenseSave
    {
        public string systemId { get; set; } = "perimeter_defense";
        public int schema_version { get; set; } = 2;
        public int last_tick_day { get; set; } = 1;
        public List<EmplacementRuntimeState> emplacements { get; set; } = new List<EmplacementRuntimeState>();

        // ── Plan 203 additions (additive; old saves default safe) ──
        public List<PerimeterSectorState> sectors { get; set; } = new List<PerimeterSectorState>();
        public List<PerimeterIntrusionLogEntry> intrusion_log { get; set; } = new List<PerimeterIntrusionLogEntry>();
        /// <summary>Plan 203 Wave F: assault sequence for day-derived jam rolls when the caller cannot supply a day.</summary>
        public int assault_count { get; set; }
    }

    public sealed class AssaultSimulationResult
    {
        public bool Repelled { get; set; }
        public bool Breached { get; set; }
        public int InitialRaiderStrength { get; set; }
        public int RemainingRaiderStrength { get; set; }
        public int RoundsFiredTotal { get; set; }
        public int EmplacementsDamaged { get; set; }
        public int EmplacementsDestroyed { get; set; }
        public bool StealthInfiltrationNeutralized { get; set; }
        public int AttackersKilled => Math.Max(0, InitialRaiderStrength - RemainingRaiderStrength);
        public int AttackersBreached => RemainingRaiderStrength;
    }

    public sealed class PerimeterDefenseSystem
    {
        public const string SystemId = "perimeter_defense";
        public const float BarrelWearPerRound = 0.04f; // 2% per 50 rounds

        private readonly List<PerimeterDefenseDefinition> _defenseDefs = new List<PerimeterDefenseDefinition>();
        private readonly Dictionary<string, PerimeterDefenseDefinition> _defsById = new Dictionary<string, PerimeterDefenseDefinition>(StringComparer.Ordinal);
        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private PerimeterDefenseSave _state = new PerimeterDefenseSave();

        // Plan 203: bounded intrusion history (never unbounded persistence).
        public const int IntrusionLogCapacity = 32;
        // Attacker counter tags (roadmap §6.10) — abstract capability vocabulary.
        public const string CounterCuttingTools = "cutting_tools";
        public const string CounterExplosives = "explosives";
        public const string CounterStealth = "stealth";
        public const string CounterVehicleBreach = "vehicle_breach";
        public const string CounterEmp = "emp";

        public event Action<EmplacementRuntimeState>? OnEmplacementConstructed;
        public event Action<EmplacementRuntimeState, int>? OnAmmoLoaded;
        public event Action<EmplacementRuntimeState>? OnTurretJammed;
        public event Action<EmplacementRuntimeState>? OnEmplacementDestroyed;
        public event Action<AssaultSimulationResult>? OnAssaultRepelled;
        public event Action<AssaultSimulationResult>? OnPerimeterBreached;

        // ── Plan 203 events ──
        public event Action<string, int>? OnFalseAlarm;                   // sectorId, day
        public event Action<string, bool>? OnSectorAlarmTriggered;        // sectorId, isFalse
        public event Action<float>? OnWeatherWear;                        // total HP lost today
        public event Action<PerimeterIntrusionLogEntry>? OnIntrusionLogged;
        /// <summary>Forwarder for the string event bus (optional).</summary>
        public event Action<string>? OnEventRaised;

        public IReadOnlyList<PerimeterDefenseDefinition> Definitions => _defenseDefs;
        public IReadOnlyList<EmplacementRuntimeState> Emplacements => _state.emplacements;

        public PerimeterDefenseSystem(
            IEnumerable<PerimeterDefenseDefinition> definitions,
            Inventory.Inventory inventory,
            ISeededRng rng,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;

            if (definitions != null)
            {
                foreach (var def in definitions)
                {
                    if (def == null || string.IsNullOrEmpty(def.defense_id)) continue;
                    _defenseDefs.Add(def);
                    _defsById[def.defense_id] = def;
                }
            }
        }

        public PerimeterDefenseDefinition? FindDefinition(string defenseId) =>
            _defsById.TryGetValue(defenseId, out var def) ? def : null;

        public EmplacementRuntimeState? FindEmplacement(string emplacementId)
        {
            for (int i = 0; i < _state.emplacements.Count; i++)
            {
                if (_state.emplacements[i].emplacement_id == emplacementId) return _state.emplacements[i];
            }
            return null;
        }

        public ActionResult ConstructEmplacement(string defenseId, bool hasRequiredCapability = true)
        {
            if (!_defsById.TryGetValue(defenseId, out var def))
                return ActionResult.Failed("unknown_defense", "defense.unknown_defense");

            // B5–B8 Phase 7 (Plan 67 §10.5): research-gated builds. The host
            // passes the live capability query result; an unlocked node without
            // the physical build never contributes defense. Empty
            // required_knowledge = basic fieldworks (always constructible).
            if (!string.IsNullOrEmpty(def.required_knowledge) && !hasRequiredCapability)
                return ActionResult.Blocked("missing_knowledge", "defense.missing_knowledge");

            // Check costs
            foreach (var cost in def.build_costs)
            {
                if (!_inventory.HasSufficient(cost.Key, cost.Value))
                    return ActionResult.Blocked($"missing_material_{cost.Key}", "defense.missing_material");
            }

            // Consume costs atomically
            foreach (var cost in def.build_costs)
            {
                if (!_inventory.TryConsume(cost.Key, cost.Value))
                    throw new InvalidOperationException($"Atomic construction failed consuming {cost.Key}");
            }

            string empId = $"emp_{defenseId}_{_state.emplacements.Count + 1}";
            var emp = new EmplacementRuntimeState
            {
                emplacement_id = empId,
                defense_id = defenseId,
                current_hp = def.max_hp,
                max_hp = def.max_hp,
                is_active = true,
                is_destroyed = false,
                loaded_ammo_count = 0,
                required_ammo_type = def.required_ammo_type,
                magazine_capacity = def.magazine_capacity,
                barrel_wear_percent = 0f,
                is_jammed = false
            };

            _state.emplacements.Add(emp);

            // Plan 203: auto-assign new emplacements round-robin across the canonical
            // sector graph so the grid has topology without player micromanagement.
            var autoSector = PerimeterSector.All[_state.emplacements.Count % PerimeterSector.All.Count];
            AssignEmplacementToSector(emp.emplacement_id, autoSector);
            OnEmplacementConstructed?.Invoke(emp);
            _log.Info($"[PerimeterDefense] Constructed {def.display_name} ({empId}).");
            return ActionResult.Success("defense.constructed");
        }

        public ActionResult LoadAmmo(string emplacementId, int amount)
        {
            var emp = FindEmplacement(emplacementId);
            if (emp == null)
                return ActionResult.Failed("emplacement_not_found", "defense.emplacement_not_found");

            if (emp.is_destroyed || !emp.is_active)
                return ActionResult.Blocked("emplacement_disabled", "defense.emplacement_disabled");

            if (string.IsNullOrEmpty(emp.required_ammo_type) || emp.magazine_capacity <= 0)
                return ActionResult.Blocked("not_a_turret", "defense.not_a_turret");

            if (amount <= 0)
                return ActionResult.Failed("invalid_amount", "defense.invalid_amount");

            int capacityRemaining = emp.magazine_capacity - emp.loaded_ammo_count;
            if (capacityRemaining <= 0)
                return ActionResult.Blocked("magazine_full", "defense.magazine_full");

            int toLoad = Math.Min(amount, capacityRemaining);
            if (!_inventory.HasSufficient(emp.required_ammo_type, toLoad))
                return ActionResult.Blocked("insufficient_ammo", "defense.insufficient_ammo");

            if (!_inventory.TryConsume(emp.required_ammo_type, toLoad))
                return ActionResult.Failed("ammo_consume_failed", "defense.ammo_consume_failed");

            emp.loaded_ammo_count += toLoad;
            OnAmmoLoaded?.Invoke(emp, toLoad);
            _log.Info($"[PerimeterDefense] Loaded {toLoad} rounds of {emp.required_ammo_type} into {emplacementId}. Total: {emp.loaded_ammo_count}/{emp.magazine_capacity}");
            return ActionResult.Success("defense.ammo_loaded");
        }

        public ActionResult ServiceTurretBarrel(string emplacementId)
        {
            var emp = FindEmplacement(emplacementId);
            if (emp == null)
                return ActionResult.Failed("emplacement_not_found", "defense.emplacement_not_found");

            if (!_inventory.HasSufficient("scrap_metal", 1))
                return ActionResult.Blocked("missing_scrap_metal", "defense.missing_scrap_metal");

            if (!_inventory.TryConsume("scrap_metal", 1))
                return ActionResult.Failed("scrap_consume_failed", "defense.scrap_consume_failed");

            emp.barrel_wear_percent = 0f;
            emp.is_jammed = false;
            _log.Info($"[PerimeterDefense] Serviced and cleared barrel for {emplacementId}. Barrel wear reset to 0%.");
            return ActionResult.Success("defense.turret_serviced");
        }

        // ─────────────────────────────────────────────────────────────
        // Plan 203 — sector grid, false alarms, weather wear, alerts,
        // counterplay. Adds encounter-context state only; combat damage
        // and injuries remain owned by the combat authority (§6.1).
        // ─────────────────────────────────────────────────────────────

        public PerimeterSectorState? FindSector(string sectorId)
        {
            foreach (var s in _state.sectors)
                if (s.sector_id == sectorId) return s;
            return null;
        }

        public IReadOnlyList<PerimeterSectorState> Sectors => _state.sectors;
        public IReadOnlyList<PerimeterIntrusionLogEntry> IntrusionLog => _state.intrusion_log;

        private PerimeterSectorState EnsureSector(string sectorId)
        {
            var existing = FindSector(sectorId);
            if (existing != null) return existing;
            var sector = new PerimeterSectorState { sector_id = sectorId };
            _state.sectors.Add(sector);
            return sector;
        }

        public ActionResult AssignEmplacementToSector(string emplacementId, string sectorId)
        {
            if (!PerimeterSector.IsValid(sectorId))
                return ActionResult.Failed("unknown_sector", "defense.unknown_sector");
            var emp = FindEmplacement(emplacementId);
            if (emp == null)
                return ActionResult.Failed("emplacement_not_found", "defense.emplacement_not_found");

            // Remove from any current sector, then assign.
            foreach (var s in _state.sectors)
                s.emplacement_ids.RemoveAll(id => id == emplacementId);

            EnsureSector(sectorId).emplacement_ids.Add(emplacementId);
            OnEventRaised?.Invoke("defense.emplacement_assigned");
            return ActionResult.Success("defense.emplacement_assigned");
        }

        /// <summary>
        /// Daily perimeter tick. Severe weather (corrosive/abrasive kinds, host-projected)
        /// wears every intact emplacement by its catalog wear. Alert devices may false-trigger.
        /// Deterministic under the seeded RNG. No combat runs here.
        /// </summary>
        public void TickDay(int day, bool severeWeather)
        {
            if (severeWeather)
            {
                float totalWear = 0f;
                for (int i = 0; i < _state.emplacements.Count; i++)
                {
                    var emp = _state.emplacements[i];
                    if (emp.is_destroyed) continue;
                    var def = _defsById.TryGetValue(emp.defense_id, out var d) ? d : null;
                    float wear = def?.weather_wear_per_storm ?? 0f;
                    if (wear <= 0f) continue;
                    int applied = (int)Math.Min(wear, (float)emp.current_hp);
                    emp.current_hp -= applied;
                    totalWear += applied;
                    if (emp.current_hp <= 0)
                    {
                        emp.current_hp = 0;
                        emp.is_destroyed = true;
                        emp.is_active = false;
                        OnEmplacementDestroyed?.Invoke(emp);
                        _log.Warn($"[PerimeterDefense] {emp.emplacement_id} destroyed by weather wear.");
                    }
                }
                if (totalWear > 0f)
                {
                    OnWeatherWear?.Invoke(totalWear);
                    OnEventRaised?.Invoke("defense.weather_wear");
                }
            }

            // False alarms: each armed, unspent alert device rolls its base rate.
            for (int i = 0; i < _state.sectors.Count; i++)
            {
                var sector = _state.sectors[i];
                if (!sector.alarm_armed || sector.alarm_spent) continue;

                int deviceIndex = 0;
                foreach (var empId in sector.emplacement_ids)
                {
                    var emp = FindEmplacement(empId);
                    if (emp == null || emp.is_destroyed || !emp.is_active) continue;
                    if (!_defsById.TryGetValue(emp.defense_id, out var def)) continue;
                    if (!def.alert_device || def.false_alarm_rate_bp <= 0) continue;

                    // Wave F: day-derived fresh-seed roll (house pattern) — split-run safe.
                    var alarmRng = new SeededRng(unchecked(day * 31 + i * 97 + deviceIndex * 7));
                    deviceIndex++;
                    if (alarmRng.NextDouble() < def.false_alarm_rate_bp / 10000.0)
                    {
                        TriggerSectorAlarm(sector, day, isFalse: true, emp.emplacement_id);
                        break; // one false trigger per sector per day
                    }
                }
            }
        }

        private void TriggerSectorAlarm(PerimeterSectorState sector, int day, bool isFalse, string detail)
        {
            sector.alarm_spent = true;
            sector.last_trigger_day = day;
            if (isFalse) sector.false_alarm_count++;
            else sector.hostile_trigger_count++;

            LogIntrusion(new PerimeterIntrusionLogEntry
            {
                day = day,
                sector_id = sector.sector_id,
                kind = isFalse ? "false_alarm" : "hostile_trigger",
                detail = detail
            });

            OnSectorAlarmTriggered?.Invoke(sector.sector_id, isFalse);
            if (isFalse) OnFalseAlarm?.Invoke(sector.sector_id, day);
            _log.Info($"[PerimeterDefense] Sector '{sector.sector_id}' alarm {(isFalse ? "FALSE trigger" : "triggered")} (device spent)." );
        }

        /// <summary>Rearms a spent sector alarm device (player action — no cost, no cooldown).</summary>
        public ActionResult ResetSectorAlarm(string sectorId)
        {
            var sector = FindSector(sectorId);
            if (sector == null)
                return ActionResult.Failed("unknown_sector", "defense.unknown_sector");
            if (!sector.alarm_spent)
                return ActionResult.Blocked("alarm_not_spent", "defense.alarm_not_spent");

            sector.alarm_spent = false;
            OnEventRaised?.Invoke("defense.alarm_reset");
            return ActionResult.Success("defense.alarm_reset");
        }

        public ActionResult DisarmSector(string sectorId)
        {
            var sector = FindSector(sectorId);
            if (sector == null)
                return ActionResult.Failed("unknown_sector", "defense.unknown_sector");
            sector.alarm_armed = !sector.alarm_armed;
            if (!sector.alarm_armed) sector.alarm_spent = false;
            OnEventRaised?.Invoke("defense.alarm_disarmed");
            return ActionResult.Success(sector.alarm_armed ? "defense.alarm_armed" : "defense.alarm_disarmed");
        }

        private void LogIntrusion(PerimeterIntrusionLogEntry entry)
        {
            _state.intrusion_log.Add(entry);
            while (_state.intrusion_log.Count > IntrusionLogCapacity)
                _state.intrusion_log.RemoveAt(0);
            OnIntrusionLogged?.Invoke(entry);
        }

        private static bool IsCountered(PerimeterDefenseDefinition def, HashSet<string>? attackerCounters)
        {
            if (attackerCounters == null || attackerCounters.Count == 0) return false;
            foreach (var tag in def.counter_tags)
                if (attackerCounters.Contains(tag)) return true;
            return false;
        }

        private bool SectorAlarmSpentFor(EmplacementRuntimeState emp)
        {
            for (int i = 0; i < _state.sectors.Count; i++)
            {
                var s = _state.sectors[i];
                if (s.emplacement_ids.Contains(emp.emplacement_id)) return s.alarm_spent;
            }
            return false;
        }

        /// <summary>
        /// Encounter-start context snapshot for the raid/combat authority (§6.9).
        /// Combat consumes these modifiers; it never writes perimeter state.
        /// </summary>
        public PerimeterEncounterSnapshot GetEncounterSnapshot(IEnumerable<string>? attackerCounterTags = null)
        {
            var counters = attackerCounterTags != null ? new HashSet<string>(attackerCounterTags, StringComparer.Ordinal) : null;
            var snap = new PerimeterEncounterSnapshot();
            float delay = 1f;
            var touchedSectors = new HashSet<string>(StringComparer.Ordinal);

            // B5–B8 expansion (§10.11): disabled/jammed counts first — they are
            // part of the snapshot even when they cannot slow an approach.
            for (int i = 0; i < _state.emplacements.Count; i++)
            {
                var emp = _state.emplacements[i];
                if (emp.is_destroyed || !emp.is_active)
                {
                    snap.emplacements_disabled++;
                    continue;
                }
                if (emp.is_jammed) snap.turrets_jammed++;
            }

            for (int i = 0; i < _state.emplacements.Count; i++)
            {
                var emp = _state.emplacements[i];
                if (emp.is_destroyed || !emp.is_active) continue;
                if (!_defsById.TryGetValue(emp.defense_id, out var def)) continue;

                bool countered = IsCountered(def, counters);
                if (countered) snap.countered_emplacement_ids.Add(emp.emplacement_id);

                // Alert devices only deny stealth while armed and unspent (§6.7 lifecycle).
                if (def.prevents_stealth_breach && !countered && !SectorAlarmSpentFor(emp))
                    snap.stealth_denied = true;

                if (!countered)
                {
                    snap.emplacements_ready++;
                    if (emp.magazine_capacity > 0 && emp.loaded_ammo_count > 0 && !emp.is_jammed)
                        snap.turrets_ready++;

                    // Entanglements and barriers slow approach proportionally to integrity.
                    if (def.slow_factor > 0f)
                    {
                        float integrity = emp.max_hp > 0 ? emp.current_hp / (float)emp.max_hp : 0f;
                        delay += def.slow_factor * integrity;
                    }
                    // Observation/early-warning hardware contributes detection initiative.
                    if (def.defense_type == "observation" || def.defense_type == "early_warning")
                        snap.detection_initiative_bonus += 0.1f + def.night_accuracy_bonus;

                    var sector = FindSectorOf(emp.emplacement_id);
                    if (sector != null)
                    {
                        touchedSectors.Add(sector.sector_id);
                        if (sector.alarm_spent && !snap.alarms_spent.Contains(sector.sector_id))
                            snap.alarms_spent.Add(sector.sector_id);
                        if (!snap.protected_sectors.Contains(sector.sector_id))
                            snap.protected_sectors.Add(sector.sector_id);
                    }
                }
            }

            // B5–B8 expansion: the honest breach surface — canonical sectors
            // with no intact emplacement at all.
            foreach (var sectorId in PerimeterSector.All)
            {
                if (!touchedSectors.Contains(sectorId))
                    snap.unguarded_sectors.Add(sectorId);
            }

            snap.movement_delay_multiplier = delay;
            return snap;
        }

        private PerimeterSectorState? FindSectorOf(string emplacementId)
        {
            for (int i = 0; i < _state.sectors.Count; i++)
            {
                var s = _state.sectors[i];
                if (s.emplacement_ids.Contains(emplacementId)) return s;
            }
            return null;
        }

        public ActionResult RepairEmplacement(string emplacementId, int hpToRestore)
        {
            var emp = FindEmplacement(emplacementId);
            if (emp == null)
                return ActionResult.Failed("emplacement_not_found", "defense.emplacement_not_found");

            if (emp.current_hp >= emp.max_hp)
                return ActionResult.Blocked("full_hp", "defense.full_hp");

            int scrapCost = Math.Max(1, (hpToRestore + 49) / 50);
            if (!_inventory.HasSufficient("scrap_metal", scrapCost))
                return ActionResult.Blocked("missing_scrap_metal", "defense.missing_scrap_metal");

            if (!_inventory.TryConsume("scrap_metal", scrapCost))
                return ActionResult.Failed("scrap_consume_failed", "defense.scrap_consume_failed");

            emp.current_hp = Math.Min(emp.max_hp, emp.current_hp + hpToRestore);
            emp.is_destroyed = false;
            emp.is_active = true;
            _log.Info($"[PerimeterDefense] Repaired {emplacementId} (+{hpToRestore} HP). Current HP: {emp.current_hp}/{emp.max_hp}");
            return ActionResult.Success("defense.repaired");
        }

        public AssaultSimulationResult SimulateRaiderAssault(
            int raiderStrength,
            bool isNight = false,
            Func<string, bool>? isEmplacementPowered = null,
            IEnumerable<string>? attackerCounterTags = null,
            int currentDay = -1)
        {
            var counters = attackerCounterTags != null ? new HashSet<string>(attackerCounterTags, StringComparer.Ordinal) : null;
            _state.assault_count++;
            var result = new AssaultSimulationResult
            {
                InitialRaiderStrength = raiderStrength,
                RemainingRaiderStrength = raiderStrength
            };

            // 1. Check early-warning tripwire flares. Plan 203: stealth denial only
            //    applies while the device is armed, unspent, and not countered.
            float nightAccuracyBonus = 0f;
            for (int i = 0; i < _state.emplacements.Count; i++)
            {
                var emp = _state.emplacements[i];
                if (emp.is_destroyed || !emp.is_active) continue;
                if (!_defsById.TryGetValue(emp.defense_id, out var def)) continue;

                if (def.prevents_stealth_breach && !IsCountered(def, counters) && !SectorAlarmSpentFor(emp))
                {
                    result.StealthInfiltrationNeutralized = true;
                }
                if (isNight && def.night_accuracy_bonus > 0f && !IsCountered(def, counters))
                {
                    nightAccuracyBonus = Math.Max(nightAccuracyBonus, def.night_accuracy_bonus);
                }
            }

            // 1b. Hostile approach triggers sector alert devices (§6.7 lifecycle —
            //     the device spends; detection/initiative context flows to combat).
            for (int i = 0; i < _state.sectors.Count; i++)
            {
                var sector = _state.sectors[i];
                if (!sector.alarm_armed || sector.alarm_spent) continue;
                foreach (var empId in sector.emplacement_ids)
                {
                    var emp = FindEmplacement(empId);
                    if (emp == null || emp.is_destroyed || !emp.is_active) continue;
                    if (!_defsById.TryGetValue(emp.defense_id, out var alertDef)) continue;
                    if (!alertDef.alert_device) continue;
                    if (IsCountered(alertDef, counters)) continue;
                    TriggerSectorAlarm(sector, currentDay < 0 ? _state.last_tick_day : currentDay, isFalse: false, empId);
                    break; // one hostile trigger per sector per assault
                }
            }

            // 2. Turret engagements
            for (int i = 0; i < _state.emplacements.Count; i++)
            {
                var emp = _state.emplacements[i];
                if (emp.is_destroyed || !emp.is_active || emp.is_jammed) continue;
                if (!_defsById.TryGetValue(emp.defense_id, out var def)) continue;

                // Plan 203 counterplay: attackers with matching capability tags
                // neutralize the emplacement entirely (§6.10 — no guaranteed immunity).
                if (IsCountered(def, counters)) continue;

                // Check power requirement
                if (def.power_draw_watts > 0 && isEmplacementPowered != null && !isEmplacementPowered(emp.emplacement_id))
                {
                    continue; // unpowered turret frozen
                }

                if (def.fire_rate_burst > 0 && emp.loaded_ammo_count > 0 && result.RemainingRaiderStrength > 0)
                {
                    int burst = Math.Min(def.fire_rate_burst, emp.loaded_ammo_count);
                    emp.loaded_ammo_count -= burst;
                    result.RoundsFiredTotal += burst;

                    // Barrel wear
                    emp.barrel_wear_percent = Math.Min(100f, emp.barrel_wear_percent + (burst * BarrelWearPerRound));

                    // Jam check if barrel wear > 50%. Wave F: day/sequence-derived
                    // fresh-seed roll — split-run safe.
                    if (emp.barrel_wear_percent > 50f)
                    {
                        int jamSeed = unchecked((currentDay >= 0 ? currentDay : _state.assault_count + 1) * 613 + i * 97);
                        double jamRoll = new SeededRng(jamSeed).NextDouble();
                        float jamChance = (emp.barrel_wear_percent - 50f) * 0.01f;
                        if (jamRoll < jamChance)
                        {
                            emp.is_jammed = true;
                            OnTurretJammed?.Invoke(emp);
                            _log.Warn($"[PerimeterDefense] Turret {emp.emplacement_id} jammed during assault!");
                        }
                    }

                    // Inflict casualties
                    float acc = isNight ? (0.6f + nightAccuracyBonus) : 0.85f;
                    int casualties = (int)Math.Floor(burst * (def.base_damage / 15f) * acc);
                    result.RemainingRaiderStrength = Math.Max(0, result.RemainingRaiderStrength - casualties);
                }
            }

            // 3. Raider retaliation against barriers
            if (result.RemainingRaiderStrength > 0)
            {
                int raiderAttackPower = result.RemainingRaiderStrength * 10;
                for (int i = 0; i < _state.emplacements.Count; i++)
                {
                    var emp = _state.emplacements[i];
                    if (emp.is_destroyed) continue;

                    int dmg = Math.Min(emp.current_hp, raiderAttackPower);
                    emp.current_hp -= dmg;
                    raiderAttackPower -= dmg;
                    result.EmplacementsDamaged++;

                    if (emp.current_hp <= 0)
                    {
                        emp.current_hp = 0;
                        emp.is_destroyed = true;
                        emp.is_active = false;
                        result.EmplacementsDestroyed++;
                        OnEmplacementDestroyed?.Invoke(emp);
                    }

                    if (raiderAttackPower <= 0) break;
                }

                // If raiders still have attack power after smashing barriers -> BREACH!
                if (raiderAttackPower > 0)
                {
                    result.Breached = true;
                    result.Repelled = false;
                    LogIntrusion(new PerimeterIntrusionLogEntry
                    {
                        day = currentDay < 0 ? _state.last_tick_day : currentDay,
                        sector_id = "perimeter",
                        kind = "breach",
                        detail = $"{result.RemainingRaiderStrength} raiders breached"
                    });
                    OnPerimeterBreached?.Invoke(result);
                    _log.Warn($"[PerimeterDefense] CRITICAL: Perimeter defense breached by {result.RemainingRaiderStrength} raiders!");
                    return result;
                }
            }

            result.Repelled = true;
            result.Breached = false;
            LogIntrusion(new PerimeterIntrusionLogEntry
            {
                day = currentDay < 0 ? _state.last_tick_day : currentDay,
                sector_id = "perimeter",
                kind = "repelled",
                detail = $"{result.AttackersKilled} attackers neutralized"
            });
            OnAssaultRepelled?.Invoke(result);
            _log.Info($"[PerimeterDefense] Assault repelled! Raiders neutralized or routed.");
            return result;
        }

        public PerimeterDefenseSave CaptureState()
        {
            var save = new PerimeterDefenseSave
            {
                systemId = SystemId,
                schema_version = 2,
                last_tick_day = _state.last_tick_day,
                assault_count = _state.assault_count
            };

            foreach (var emp in _state.emplacements)
            {
                save.emplacements.Add(new EmplacementRuntimeState
                {
                    emplacement_id = emp.emplacement_id,
                    defense_id = emp.defense_id,
                    current_hp = emp.current_hp,
                    max_hp = emp.max_hp,
                    is_active = emp.is_active,
                    is_destroyed = emp.is_destroyed,
                    loaded_ammo_count = emp.loaded_ammo_count,
                    required_ammo_type = emp.required_ammo_type,
                    magazine_capacity = emp.magazine_capacity,
                    barrel_wear_percent = emp.barrel_wear_percent,
                    is_jammed = emp.is_jammed
                });
            }

            // Plan 203: deep-clone sectors + intrusion log (emplacement_ids are mutable).
            save.sectors = new List<PerimeterSectorState>(_state.sectors.Count);
            foreach (var s in _state.sectors)
            {
                if (s == null) continue;
                save.sectors.Add(CloneSector(s));
            }
            save.intrusion_log = new List<PerimeterIntrusionLogEntry>(_state.intrusion_log.Count);
            foreach (var e in _state.intrusion_log)
            {
                if (e == null) continue;
                save.intrusion_log.Add(CloneIntrusion(e));
            }

            return save;
        }

        public void RestoreState(PerimeterDefenseSave? save)
        {
            if (save == null) return;
            _state.last_tick_day = save.last_tick_day;
            _state.assault_count = save.assault_count;
            _state.emplacements.Clear();

            if (save.emplacements != null)
            {
                foreach (var emp in save.emplacements)
                {
                    _state.emplacements.Add(new EmplacementRuntimeState
                    {
                        emplacement_id = emp.emplacement_id,
                        defense_id = emp.defense_id,
                        current_hp = emp.current_hp,
                        max_hp = emp.max_hp,
                        is_active = emp.is_active,
                        is_destroyed = emp.is_destroyed,
                        loaded_ammo_count = emp.loaded_ammo_count,
                        required_ammo_type = emp.required_ammo_type,
                        magazine_capacity = emp.magazine_capacity,
                        barrel_wear_percent = emp.barrel_wear_percent,
                        is_jammed = emp.is_jammed
                    });
                }
            }

            // Plan 203: additive restore — deep-copy so save DTOs never alias live state.
            _state.sectors = new List<PerimeterSectorState>();
            if (save.sectors != null)
            {
                foreach (var s in save.sectors)
                {
                    if (s == null) continue;
                    _state.sectors.Add(CloneSector(s));
                }
            }
            _state.intrusion_log = new List<PerimeterIntrusionLogEntry>();
            if (save.intrusion_log != null)
            {
                foreach (var e in save.intrusion_log)
                {
                    if (e == null) continue;
                    _state.intrusion_log.Add(CloneIntrusion(e));
                }
            }
            if (_state.intrusion_log.Count > IntrusionLogCapacity)
                _state.intrusion_log.RemoveRange(0, _state.intrusion_log.Count - IntrusionLogCapacity);
        }

        private static PerimeterSectorState CloneSector(PerimeterSectorState s)
        {
            return new PerimeterSectorState
            {
                sector_id = s.sector_id,
                emplacement_ids = s.emplacement_ids != null
                    ? new List<string>(s.emplacement_ids)
                    : new List<string>(),
                alarm_armed = s.alarm_armed,
                alarm_spent = s.alarm_spent,
                last_trigger_day = s.last_trigger_day,
                false_alarm_count = s.false_alarm_count,
                hostile_trigger_count = s.hostile_trigger_count,
            };
        }

        private static PerimeterIntrusionLogEntry CloneIntrusion(PerimeterIntrusionLogEntry e)
        {
            return new PerimeterIntrusionLogEntry
            {
                day = e.day,
                sector_id = e.sector_id,
                kind = e.kind,
                detail = e.detail,
            };
        }
    }
}
