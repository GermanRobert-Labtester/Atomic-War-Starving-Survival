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
        public int schema_version { get; set; } = 1;
        public int last_tick_day { get; set; } = 1;
        public List<EmplacementRuntimeState> emplacements { get; set; } = new List<EmplacementRuntimeState>();
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

        public event Action<EmplacementRuntimeState>? OnEmplacementConstructed;
        public event Action<EmplacementRuntimeState, int>? OnAmmoLoaded;
        public event Action<EmplacementRuntimeState>? OnTurretJammed;
        public event Action<EmplacementRuntimeState>? OnEmplacementDestroyed;
        public event Action<AssaultSimulationResult>? OnAssaultRepelled;
        public event Action<AssaultSimulationResult>? OnPerimeterBreached;

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

        public ActionResult ConstructEmplacement(string defenseId)
        {
            if (!_defsById.TryGetValue(defenseId, out var def))
                return ActionResult.Failed("unknown_defense", "defense.unknown_defense");

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
            Func<string, bool>? isEmplacementPowered = null)
        {
            var result = new AssaultSimulationResult
            {
                InitialRaiderStrength = raiderStrength,
                RemainingRaiderStrength = raiderStrength
            };

            // 1. Check early-warning tripwire flares
            float nightAccuracyBonus = 0f;
            for (int i = 0; i < _state.emplacements.Count; i++)
            {
                var emp = _state.emplacements[i];
                if (emp.is_destroyed || !emp.is_active) continue;
                if (!_defsById.TryGetValue(emp.defense_id, out var def)) continue;

                if (def.prevents_stealth_breach)
                {
                    result.StealthInfiltrationNeutralized = true;
                }
                if (isNight && def.night_accuracy_bonus > 0f)
                {
                    nightAccuracyBonus = Math.Max(nightAccuracyBonus, def.night_accuracy_bonus);
                }
            }

            // 2. Turret engagements
            for (int i = 0; i < _state.emplacements.Count; i++)
            {
                var emp = _state.emplacements[i];
                if (emp.is_destroyed || !emp.is_active || emp.is_jammed) continue;
                if (!_defsById.TryGetValue(emp.defense_id, out var def)) continue;

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

                    // Jam check if barrel wear > 50%
                    if (emp.barrel_wear_percent > 50f)
                    {
                        double jamRoll = _rng.NextDouble();
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
                    OnPerimeterBreached?.Invoke(result);
                    _log.Warn($"[PerimeterDefense] CRITICAL: Perimeter defense breached by {result.RemainingRaiderStrength} raiders!");
                    return result;
                }
            }

            result.Repelled = true;
            result.Breached = false;
            OnAssaultRepelled?.Invoke(result);
            _log.Info($"[PerimeterDefense] Assault repelled! Raiders neutralized or routed.");
            return result;
        }

        public PerimeterDefenseSave CaptureState()
        {
            var save = new PerimeterDefenseSave
            {
                systemId = SystemId,
                schema_version = 1,
                last_tick_day = _state.last_tick_day
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

            return save;
        }

        public void RestoreState(PerimeterDefenseSave? save)
        {
            if (save == null) return;
            _state.last_tick_day = save.last_tick_day;
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
        }
    }
}
