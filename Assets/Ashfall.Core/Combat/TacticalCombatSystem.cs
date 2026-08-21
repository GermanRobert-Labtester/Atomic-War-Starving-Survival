using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Combat
{
    /// <summary>Stance numeric modifiers applied to combat values.</summary>
    public struct StanceMods
    {
        public float Accuracy;      // player fire accuracy
        public float Damage;        // player fire damage
        public float Defense;       // reduces enemy accuracy (0..1)
        public float AmmoUse;       // rounds per burst multiplier
        public float Degrade;       // weapon degradation multiplier
        public float JamRisk;       // jam chance multiplier
        public float Noise;         // encounter exposure / noise
        public float Mobility;      // chance to successfully flee
        public float MoraleDelta;   // stance morale impact
        public bool CanFlee;        // may this stance retreat?
        public bool DeathIsInstant; // last stand: 0 HP = instant death + mutual kill
    }

    /// <summary>
    /// Engine-agnostic tactical combat authority. Wires weapon condition/jam,
    /// ballistic resolution, tactical stances and lanes, suppression, flanking,
    /// bleed-out, last-stand, retreat, injury/morale/loot/journal hooks, and
    /// save/load. Every roll consumes the injected ISeededRng — the host owns
    /// seeding, so replaying identical commands from an identical state with a
    /// fresh rng of the same seed reproduces identical outcomes.
    /// </summary>
    public class TacticalCombatSystem
    {
        public const string SystemId = "combat_system";

        public const int DefaultBleedTurns = 3;
        public const int DefaultSuppressDuration = 1;
        public const int MaxRicochetBounces = BallisticsSystem.MaxRicochetCount;

        private CombatState _state = new CombatState();
        private readonly WeaponConditionSystem _condition = new WeaponConditionSystem();
        private readonly Dictionary<string, CombatPerks> _perksBySurvivor = new Dictionary<string, CombatPerks>(StringComparer.Ordinal);
        private CombatHostPorts _ports;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<CombatState> OnStateChanged;
        public event Action<CombatState, CombatEvent> OnCombatEvent;
        public event Action<CombatState> OnEncounterEnded;

        public CombatState State => _state;
        public CombatHostPorts Ports { get => _ports; set => _ports = value; }

        public TacticalCombatSystem(CombatState state = null!, CombatHostPorts ports = null!)
        {
            if (state != null) _state = state;
            _ports = ports ?? new CombatHostPorts();
            CombatCatalog.SeedDefaults();
        }

        /// <summary>The perks tracker for a survivor (lazily created, save-safe).</summary>
        public CombatPerks? PerksFor(string survivorId, int seed)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            if (!_perksBySurvivor.TryGetValue(survivorId, out var p))
            {
                p = new CombatPerks(seed);
                _perksBySurvivor[survivorId] = p;
            }
            return p;
        }

        // ══ Stance table ══════════════════════════════════════════════════

        public static string StanceId(TacticalStance s) => "combat_stance_" + s.ToString().ToLowerInvariant();

        public static bool TryParseStance(string id, out TacticalStance stance)
        {
            stance = TacticalStance.HoldPosition;
            if (string.IsNullOrEmpty(id)) return false;
            foreach (TacticalStance s in Enum.GetValues(typeof(TacticalStance)))
            {
                if (string.Equals(id, StanceId(s), StringComparison.OrdinalIgnoreCase))
                {
                    stance = s;
                    return true;
                }
            }
            return false;
        }

        public static StanceMods GetStanceMods(TacticalStance s)
        {
            switch (s)
            {
                case TacticalStance.HoldPosition:
                    return new StanceMods { Accuracy = 1.0f, Damage = 1.0f, Defense = 0.15f, AmmoUse = 1.0f, Degrade = 1.0f, JamRisk = 1.0f, Noise = 1.0f, Mobility = 0.75f, MoraleDelta = 0f, CanFlee = true };
                case TacticalStance.Advance:
                    return new StanceMods { Accuracy = 1.15f, Damage = 1.10f, Defense = 0f, AmmoUse = 1.3f, Degrade = 1.15f, JamRisk = 1.1f, Noise = 1.3f, Mobility = 0.6f, MoraleDelta = 2f, CanFlee = true };
                case TacticalStance.SuppressiveFire:
                    return new StanceMods { Accuracy = 0.6f, Damage = 0.6f, Defense = 0.05f, AmmoUse = 2.0f, Degrade = 1.25f, JamRisk = 1.2f, Noise = 1.5f, Mobility = 0.5f, MoraleDelta = 1f, CanFlee = true };
                case TacticalStance.Retreat:
                    return new StanceMods { Accuracy = 0.6f, Damage = 0.8f, Defense = 0f, AmmoUse = 1.0f, Degrade = 1.0f, JamRisk = 1.0f, Noise = 1.0f, Mobility = 0.9f, MoraleDelta = -2f, CanFlee = true };
                case TacticalStance.LastStand:
                    return new StanceMods { Accuracy = 2.0f, Damage = 2.0f, Defense = 0f, AmmoUse = 1.0f, Degrade = 1.3f, JamRisk = 1.0f, Noise = 1.2f, Mobility = 0f, MoraleDelta = 4f, CanFlee = false, DeathIsInstant = true };
                default:
                    return new StanceMods { Accuracy = 1f, Damage = 1f, Defense = 0f, AmmoUse = 1f, Degrade = 1f, JamRisk = 1f, Noise = 1f, Mobility = 0.75f, MoraleDelta = 0f, CanFlee = true };
            }
        }

        // ══ Encounter lifecycle ══════════════════════════════════════════

        /// <summary>
        /// Begin a combat encounter. Player survivors carry their weapons;
        /// enemies are generated from a count/health template. Returns structured
        /// failure (false) instead of throwing on invalid inputs.
        /// </summary>
        public bool BeginEncounter(
            string encounterId,
            string expeditionId,
            string locationId,
            string locationName,
            int day,
            int seed,
            IReadOnlyList<CombatantState> players,
            IReadOnlyList<WeaponInstanceState> playerWeapons,
            int enemyCount,
            float enemyHealth,
            ILog log = null!)
        {
            if (string.IsNullOrEmpty(encounterId)
                || players == null || players.Count == 0
                || enemyCount < 1)
                return false;

            _state = new CombatState
            {
                SaveVersion = CombatState.CurrentSaveVersion,
                EncounterId = encounterId,
                ExpeditionId = expeditionId ?? string.Empty,
                LocationId = locationId ?? string.Empty,
                LocationName = locationName ?? string.Empty,
                Day = day,
                Seed = seed,
                Turn = 1,
                Phase = (int)CombatPhase.PlayerTurn,
                PlayerStance = StanceId(TacticalStance.HoldPosition),
                RoundNumber = 1,
                Resolved = false
            };

            // Deep-copy players into state with stable ids.
            for (int i = 0; i < players.Count; i++)
                _state.Combatants.Add(CloneCombatant(players[i]));

            // Register player weapons.
            if (playerWeapons != null)
            {
                for (int i = 0; i < playerWeapons.Count; i++)
                    _state.Weapons.Add(CloneWeapon(playerWeapons[i]));
            }

            // Link each player combatant to its weapon (first unassigned).
            AssignPlayerWeapons();

            // Generate enemies deterministically.
            for (int i = 0; i < enemyCount; i++)
            {
                _state.Combatants.Add(new CombatantState
                {
                    Id = "enemy_" + encounterId + "_" + i,
                    Name = "Raider",
                    IsPlayer = false,
                    FactionId = "faction_raiders",
                    Lane = (int)(i % 3),
                    Health = enemyHealth,
                    MaxHealth = enemyHealth,
                    ArmorRating = 0f,
                    CoverRating = 0.3f // raiders use rubble cover
                });
            }

            // Seed ammo for player weapons that lack a live host catalog.
            SeedWeaponAmmo();

            AddEvent("encounter_start", encounterId, "Combat begins at " + (string.IsNullOrEmpty(locationName) ? locationId : locationName));
            OnStateChanged?.Invoke(_state);
            return true;
        }

        private void AssignPlayerWeapons()
        {
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (!c.IsPlayer || c.IsDowned || !string.IsNullOrEmpty(c.WeaponInstanceId))
                    continue;
                // find next unassigned weapon owned by this survivor, else any free
                for (int w = 0; w < _state.Weapons.Count; w++)
                {
                    var wp = _state.Weapons[w];
                    if (wp.OwnerSurvivorId != c.SurvivorId || !string.IsNullOrEmpty(wp.OwnerCombatantId))
                        continue;
                    c.WeaponInstanceId = wp.InstanceId;
                    wp.OwnerCombatantId = c.Id;
                    break;
                }
            }
        }

        private void SeedWeaponAmmo()
        {
            for (int i = 0; i < _state.Weapons.Count; i++)
            {
                var w = _state.Weapons[i];
                var def = CombatCatalog.GetWeapon(w.WeaponId);
                if (def != null && string.IsNullOrEmpty(w.AmmoId)) w.AmmoId = def.caliber;
                if (w.AmmoRemaining <= 0 && def != null) w.AmmoRemaining = def.burst * 10;
            }
        }

        // ══ Targeting / stance helper ═════════════════════════════════════

        private TacticalStance CurrentStance()
        {
            return TryParseStance(_state.PlayerStance, out var s) ? s : TacticalStance.HoldPosition;
        }

        private CombatantState? FindPlayerCombatant(string survivorIdOrCombatantId)
        {
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (!c.IsPlayer) continue;
                if (c.Id == survivorIdOrCombatantId || c.SurvivorId == survivorIdOrCombatantId)
                    return c;
            }
            return null;
        }

        private CombatantState? FindCombatant(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < _state.Combatants.Count; i++)
                if (_state.Combatants[i].Id == id) return _state.Combatants[i];
            return null;
        }

        private List<CombatantState> LivingPlayers()
        {
            var list = new List<CombatantState>();
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (!c.IsPlayer || c.HasFled) continue;
                list.Add(c);
            }
            list.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            return list;
        }

        private List<CombatantState> LivingEnemies()
        {
            var list = new List<CombatantState>();
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (c.IsPlayer || c.HasFled) continue;
                list.Add(c);
            }
            list.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            return list;
        }

        private CombatantState? PickActiveShooter()
        {
            var players = LivingPlayers();
            if (players.Count == 0) return null;
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (!c.IsDowned && !string.IsNullOrEmpty(c.WeaponInstanceId)) return c;
            }
            return null;
        }

        private WeaponInstanceState? WeaponOf(CombatantState c)
        {
            if (c == null || string.IsNullOrEmpty(c.WeaponInstanceId)) return null;
            for (int i = 0; i < _state.Weapons.Count; i++)
                if (_state.Weapons[i].InstanceId == c.WeaponInstanceId) return _state.Weapons[i];
            return null;
        }

        private bool IsLaneContestedForPlayer(CombatantState player)
        {
            var enemies = LivingEnemies();
            for (int i = 0; i < enemies.Count; i++)
                if (enemies[i].Lane == player.Lane && !enemies[i].IsDowned) return true;
            return false;
        }

        private float FlankMultiplier(CombatantState player)
        {
            return IsLaneContestedForPlayer(player) ? 1f : 1.5f;
        }

        private void AddEvent(string kind, string targetId, string detail, float value = 0f)
        {
            var e = new CombatEvent
            {
                Kind = kind,
                Day = _state.Day,
                Turn = _state.Turn,
                SubjectId = _state.EncounterId,
                TargetId = targetId ?? string.Empty,
                Detail = detail,
                Value = value
            };
            _state.Events.Add(e);
            OnCombatEvent?.Invoke(_state, e);
        }

        private void Notify() => OnStateChanged?.Invoke(_state);

        // ══ Player actions ═══════════════════════════════════════════════

        public CombatActionResult SetStance(TacticalStance stance, string subjectSurvivorId = null!)
        {
            var res = new CombatActionResult();
            if (_state.Resolved)
            {
                res.Message = "Encounter is over.";
                return res;
            }
            if (!GetStanceMods(stance).CanFlee && stance == TacticalStance.Retreat)
            {
                // handled below; retreat is its own action, keep stance allowed
            }

            _state.PlayerStance = StanceId(stance);
            var mods = GetStanceMods(stance);
            if (subjectSurvivorId != null && mods.MoraleDelta != 0f && _ports.ApplyMoraleDelta != null)
                _ports.ApplyMoraleDelta(subjectSurvivorId, mods.MoraleDelta);

            AddEvent("stance", subjectSurvivorId ?? _state.EncounterId, "Stance set to " + stance);
            res.Success = true;
            res.Message = "Stance: " + stance;
            Notify();
            return res;
        }

        public CombatActionResult PlayerFire(string targetId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }

            var shooter = PickActiveShooter();
            if (shooter == null) { res.Message = "No armed standing survivor to fire."; return res; }

            var weapon = WeaponOf(shooter);
            if (weapon == null) { res.Message = shooter.Name + " has no weapon."; return res; }

            var target = FindCombatant(targetId);
            if (target == null || target.IsPlayer || target.HasFled)
            {
                res.Message = "Invalid target: " + targetId;
                return res;
            }

            var def = CombatCatalog.GetWeapon(weapon.WeaponId);
            if (def == null) { res.Message = "Unknown weapon: " + weapon.WeaponId; return res; }

            var stance = CurrentStance();
            var mods = GetStanceMods(stance);

            // ── Burst / jam / burst-failure ──
            if (weapon.IsJammed)
            {
                AddEvent("jam_persist", shooter.Id, weapon.WeaponId + " is jammed; clear it first.");
                res.Message = weapon.WeaponId + " is jammed — clear the jam first.";
                Notify();
                return res;
            }

            // Ammo: consume burst rounds through host or the weapon token.
            int burst = Math.Max(1, def.burst);
            int rounds = (int)Math.Max(1, Math.Round(burst * mods.AmmoUse, MidpointRounding.AwayFromZero));
            int remaining;
            if (_ports.ConsumeAmmo != null)
            {
                remaining = _ports.ConsumeAmmo(weapon.AmmoId, rounds);
                if (remaining < 0)
                {
                    res.Message = "No " + (CombatCatalog.GetAmmo(weapon.AmmoId)?.displayName ?? weapon.AmmoId) + " ammunition.";
                    Notify();
                    return res;
                }
            }
            else
            {
                if (weapon.AmmoRemaining < rounds)
                {
                    res.Message = "Weapon out of ammunition.";
                    Notify();
                    return res;
                }
                weapon.AmmoRemaining -= rounds;
            }

            weapon.ShotsFired += burst;
            var perShooter = PerksFor(shooter.SurvivorId, _state.Seed);
            perShooter?.RecordAmmoExpended(shooter.SurvivorId, rounds);

            // ── Degradation ──
            float degrade = WeaponConditionSystem.ComputeDegradePerBurst(weapon) * mods.Degrade;
            WeaponConditionSystem.Degrade(weapon, degrade);

            // ── Jam / burst failure ──
            bool jammed = WeaponConditionSystem.TryJammed(weapon, rng);
            if (jammed)
            {
                AddEvent("weapon_jam", shooter.Id, weapon.WeaponId + " jammed mid-action.");
                res.Success = true;
                res.Message = weapon.WeaponId + " jammed — clear it.";
                Notify();
                return res;
            }

            bool burstFailure = WeaponConditionSystem.TryWeaponBurst(weapon, rng);
            if (burstFailure)
            {
                AddEvent("weapon_burst", shooter.Id, weapon.WeaponId + " burst in the hand.");
                if (_ports.RaiseTrauma != null) _ports.RaiseTrauma(shooter.SurvivorId, "combat_injury", 1f);
                res.Success = true;
                res.Message = weapon.WeaponId + " burst — the action is wrecked.";
                Notify();
                return res;
            }

            // ── Ballistics ──
            var ammo = CombatCatalog.GetAmmo(weapon.AmmoId);
            var coverMaterial = CombatCatalog.GetMaterial("material_concrete"); // default rubble cover
            var armorMaterial = CombatCatalog.GetMaterial(GetArmorMaterialId(shooter)!);
            var barrier = FindPlayerLaneBarrier(target.Lane); // enemy behind a player barrier? use enemy barrier

            var ctx = new BallisticContext
            {
                ShooterId = shooter.Id,
                ShooterName = shooter.Name,
                IsPlayerShooter = true,
                WeaponId = weapon.WeaponId,
                WeaponName = def.displayName,
                WeaponAccuracy = def.accuracy,
                WeaponDamage = def.damage,
                WeaponRangeMod = def.range,
                AmmoId = weapon.AmmoId,
                AmmoName = ammo?.displayName ?? weapon.AmmoId,
                AmmoDamageMod = ammo?.damageMod ?? 1f,
                AmmoRangeMod = ammo?.rangeMod ?? 1f,
                StanceAccuracyMod = mods.Accuracy,
                StanceDamageMod = mods.Damage,
                ExternalAccuracyMod = 1f - (weapon.IsJammed ? 1f : 0f),
                ExternalDamageMod = FlankMultiplier(shooter) * GetCloseQuartersBonus(shooter),
                IsFirstShotCritBonus = false,
                ExtraCritChance = 0f,
                IntendedTarget = target,
                CoverMaterial = coverMaterial!,
                ArmorMaterial = armorMaterial!,
                BarrierMaterial = null!,
                RicochetTargets = LivingEnemies()
            };

            var outcome = BallisticsSystem.Resolve(ctx, rng);

            var ev = new CombatEvent
            {
                Kind = "fire",
                Day = _state.Day,
                Turn = _state.Turn,
                SubjectId = shooter.Id,
                TargetId = target.Id,
                Detail = shooter.Name + " fires " + weapon.WeaponId + " → " + Describe(outcome),
                Value = outcome.DamageDealt
            };
            _state.Events.Add(ev);
            OnCombatEvent?.Invoke(_state, ev);

            // Apply damage to the resolved target.
            if (outcome.DamageDealt > 0f && !string.IsNullOrEmpty(outcome.ResolvedTargetId))
            {
                var victim = FindCombatant(outcome.ResolvedTargetId);
                if (victim != null)
                    ApplyDamage(victim, outcome.DamageDealt, shooter, outcome.IsCritical, rng);
            }

            res.Success = true;
            res.Message = Describe(outcome);
            Notify();
            CheckResolution();
            return res;
        }

        private string? GetArmorMaterialId(CombatantState c)
        {
            if (c.ArmorRating >= 0.6f) return "armor_plate";
            if (c.ArmorRating >= 0.4f) return "armor_kevlar";
            if (c.ArmorRating >= 0.2f) return "armor_cloth";
            return null;
        }

        private float GetCloseQuartersBonus(CombatantState shooter)
        {
            var p = PerksFor(shooter.SurvivorId, _state.Seed);
            return p != null ? p.GetCloseQuartersDamageMultiplier(shooter.SurvivorId, false) : 1f;
        }

        private BarrierState? FindPlayerLaneBarrier(int lane)
        {
            for (int i = 0; i < _state.Barriers.Count; i++)
                if (_state.Barriers[i].IsPlayer && _state.Barriers[i].Lane == lane) return _state.Barriers[i];
            return null;
        }

        public CombatActionResult PlayerSuppress(ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }

            var shooter = PickActiveShooter();
            if (shooter == null) { res.Message = "No standing armed survivor."; return res; }
            var weapon = WeaponOf(shooter);
            if (weapon == null) { res.Message = shooter.Name + " has no weapon."; return res; }
            var def = CombatCatalog.GetWeapon(weapon.WeaponId);
            if (def == null || !def.isSuppressionCapable)
            {
                res.Message = "Weapon cannot lay suppressive fire (needs a rifle or LMG).";
                return res;
            }

            var stance = CurrentStance();
            var mods = GetStanceMods(stance);
            int rounds = (int)Math.Max(2, Math.Round(CombatPerks.SuppressingFireAmmoCost * mods.AmmoUse, MidpointRounding.AwayFromZero));

            int rem = _ports.ConsumeAmmo != null ? _ports.ConsumeAmmo(weapon.AmmoId, rounds) : (weapon.AmmoRemaining -= rounds);
            if (_ports.ConsumeAmmo != null && rem < 0)
            {
                res.Message = "Not enough ammo for suppressive fire.";
                Notify();
                return res;
            }
            if (_ports.ConsumeAmmo == null && weapon.AmmoRemaining < 0)
            {
                weapon.AmmoRemaining += rounds;
                res.Message = "Not enough ammo for suppressive fire.";
                Notify();
                return res;
            }

            weapon.ShotsFired += rounds;
            var perks = PerksFor(shooter.SurvivorId, _state.Seed);
            perks?.RecordAmmoExpended(shooter.SurvivorId, rounds);

            // Pinned accuracy drops to 0 for the duration.
            var enemies = LivingEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].IsPinned = true;
                enemies[i].PinnedTurnsRemaining = DefaultSuppressDuration;
            }

            AddEvent("suppress", _state.EncounterId, shooter.Name + " lays suppressive fire, pinning " + enemies.Count + " hostiles.");
            res.Success = true;
            res.Message = "Suppressive fire — " + enemies.Count + " enemies pinned.";
            Notify();
            return res;
        }

        public CombatActionResult PlayerClearJam(string survivorIdOrCombatantId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            var weapon = WeaponOf(c);
            if (weapon == null) { res.Message = "No weapon to clear."; return res; }
            if (!weapon.IsJammed) { res.Message = weapon.WeaponId + " is not jammed."; return res; }

            var perks = PerksFor(c.SurvivorId, _state.Seed);
            int ticks = perks != null ? perks.GetJamClearTicks(c.SurvivorId) : WeaponConditionSystem.DefaultJamClearTicks;
            bool cleared = WeaponConditionSystem.TickJamClear(weapon, ticks);
            perks?.RecordWeaponJamSurvived(c.SurvivorId);

            AddEvent("clear_jam", c.Id, weapon.WeaponId + " jam cleared (" + ticks + " ticks).");
            res.Success = true;
            res.Message = cleared ? weapon.WeaponId + " cleared." : weapon.WeaponId + " jam being worked (" + weapon.JamClearTicksRemaining + " ticks left).";
            Notify();
            return res;
        }

        /// <summary>Item 6: explicit Reload action. Refills ammo for the survivor's
        /// weapon if the inventory adapter can supply the magazine.</summary>
        public CombatActionResult PlayerReload(string survivorIdOrCombatantId)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            var weapon = WeaponOf(c);
            if (weapon == null) { res.Message = "No weapon to reload."; return res; }
            string ammoId = weapon.AmmoId;
            if (string.IsNullOrEmpty(ammoId))
            {
                res.Message = weapon.WeaponId + " has no ammo type assigned.";
                return res;
            }
            int maxLoad = weapon.MagazineCapacity > 0 ? weapon.MagazineCapacity : 30;
            int needed = Math.Max(0, maxLoad - weapon.AmmoRemaining);
            if (needed == 0)
            {
                res.Message = weapon.WeaponId + " is already fully loaded.";
                return res;
            }
            int granted = _ports?.ConsumeAmmo != null ? _ports.ConsumeAmmo(ammoId, needed) : 0;
            if (granted <= 0)
            {
                res.Message = "No " + ammoId + " available to reload.";
                return res;
            }
            weapon.AmmoRemaining += granted;
            AddEvent("reload", c.Id,
                weapon.WeaponId + " reloaded +" + granted + " " + ammoId + " (" +
                weapon.AmmoRemaining + "/" + maxLoad + ").");
            res.Success = true;
            res.Message = weapon.WeaponId + " +" + granted + " " + ammoId + ".";
            Notify();
            return res;
        }

        public CombatActionResult PlayerFieldRepair(string survivorIdOrCombatantId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            var weapon = WeaponOf(c);
            if (weapon == null) { res.Message = "No weapon to repair."; return res; }

            int cost = WeaponConditionSystem.GetScrapRepairCost(weapon);
            bool ok = _condition.TryFieldRepair(weapon, _ports);
            if (!ok)
            {
                res.Message = "Cannot repair — need " + cost + " " + WeaponConditionSystem.ScrapMaterialId + ".";
                Notify();
                return res;
            }
            AddEvent("repair", c.Id, weapon.WeaponId + " field-repaired (" + cost + " scrap).");
            res.Success = true;
            res.Message = weapon.WeaponId + " repaired to full condition.";
            Notify();
            return res;
        }

        public CombatActionResult PlayerMoveLane(string survivorIdOrCombatantId, CombatLane lane, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            if (c.IsDowned) { res.Message = c.Name + " is downed and cannot reposition."; return res; }

            c.Lane = (int)lane;
            AddEvent("lane", c.Id, c.Name + " moved to " + lane + " lane.");
            res.Success = true;
            res.Message = c.Name + " is now " + lane + ".";
            Notify();
            return res;
        }

        public CombatActionResult PlayerDeployTrap(ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }
            var enemies = LivingEnemies();
            if (enemies.Count == 0) { res.Message = "No hostiles to trap."; return res; }

            // A trap wounds the leading enemy in the most contested lane.
            var leading = enemies[0];
            var perkMultiplier = GetMaxTrapDamageMultiplier();
            float dmg = 14f * perkMultiplier;
            ApplyDamage(leading, dmg, null!, false, rng);
            AddEvent("trap", leading.Id, "A jury-rigged trap tears into " + leading.Name + ".");
            res.Success = true;
            res.Message = "Trap wounds " + leading.Name + " (" + dmg.ToString("0") + ").";
            Notify();
            CheckResolution();
            return res;
        }

        private float GetMaxTrapDamageMultiplier()
        {
            float m = 1f;
            var players = LivingPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var p = PerksFor(players[i].SurvivorId, _state.Seed);
                if (p != null) m = Math.Max(m, p.GetTrapDamageMultiplier(players[i].SurvivorId));
            }
            return m;
        }

        /// <summary>Clean ash/contamination from all player weapons (decon flush / maintenance).</summary>
        public CombatActionResult PlayerDecontaminate(ISeededRng rng)
        {
            var res = new CombatActionResult();
            var players = LivingPlayers();
            int cleaned = 0;
            for (int i = 0; i < _state.Weapons.Count; i++)
            {
                var w = _state.Weapons[i];
                if (w.OwnerSurvivorId == null) continue;
                if (w.CachedJamChance > 0.5f || w.IsJammed)
                {
                    w.IsJammed = false;
                    w.JamClearTicksRemaining = 0;
                    w.CachedJamChance = WeaponConditionSystem.ComputeJamChance(w);
                    cleaned++;
                }
            }
            AddEvent("decon", _state.EncounterId, "Decontamination flush cleaned " + cleaned + " weapon action(s).");
            res.Success = true;
            res.Message = cleaned + " weapon(s) flushed clean.";
            Notify();
            return res;
        }

        public CombatActionResult PlayerBandage(string rescuerId, string downedId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var rescuer = FindPlayerCombatant(rescuerId);
            if (rescuer == null) { res.Message = "Unknown rescuer."; return res; }
            var downed = FindPlayerCombatant(downedId);
            if (downed == null || !downed.IsDowned) { res.Message = "That survivor is not downed."; return res; }

            bool consumed = _ports.ConsumeItem != null && _ports.ConsumeItem("bandage", 1);
            if (_ports.ConsumeItem != null && !consumed)
            {
                res.Message = "No bandage available.";
                Notify();
                return res;
            }

            downed.IsDowned = false;
            downed.BleedTurnsRemaining = 0;
            downed.Health = MathfCompat.Max(downed.Health, 15f);
            if (_ports.HealSurvivor != null)
                downed.Health = _ports.HealSurvivor(downed.SurvivorId, 15f);

            AddEvent("bandage", downed.Id, rescuer.Name + " bandages " + downed.Name + ".");
            res.Success = true;
            res.Message = downed.Name + " bandaged and stabilized.";
            Notify();
            return res;
        }

        public CombatActionResult PlayerRetreat(ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }
            var mods = GetStanceMods(CurrentStance());
            if (!mods.CanFlee)
            {
                res.Message = "You cannot flee from a last stand.";
                return res;
            }

            float success = mods.Mobility;
            foreach (var c in LivingPlayers())
            {
                var p = PerksFor(c.SurvivorId, _state.Seed);
                p?.RecordFlee(c.SurvivorId);
            }

            if (rng.NextDouble() < success)
            {
                // Clean retreat.
                foreach (var c in LivingPlayers()) { c.HasFled = true; if (c.IsPinned) c.IsPinned = false; }
                _state.Phase = (int)CombatPhase.Retreated;
                _state.Resolved = true;
                _state.OutcomeText = "Your people fall back and break contact.";
                AddEvent("retreat", _state.EncounterId, "The squad disengages and retreats.");
                OnEncounterEnded?.Invoke(_state);
                Notify();
                res.Success = true;
                res.Message = "Squad extracts successfully.";
                return res;
            }

            // Failed retreat: take injury on the way out.
            var players = LivingPlayers();
            if (players.Count > 0)
            {
                var victim = players[0];
                float inj = MathfCompat.Max(8f, victim.Health * 0.4f);
                ApplyDamage(victim, inj, null!, false, rng);
                AddEvent("retreat_fail", victim.Id, "The retreat collapses; " + victim.Name + " is hit.");
            }
            res.Success = true;
            res.Message = "Retreat disrupted — someone is hit.";
            Notify();
            CheckResolution();
            return res;
        }

        public CombatActionResult PlayerLastStand(string survivorIdOrCombatantId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            c.IsLastStand = true;
            SetStance(TacticalStance.LastStand, c.SurvivorId);
            AddEvent("last_stand", c.Id, c.Name + " declares a last stand — no retreat, doubled accuracy.");
            res.Success = true;
            res.Message = c.Name + " is fighting to the death.";
            Notify();
            return res;
        }

        // ══ Damage / bleed / end-of-turn ══════════════════════════════════

        private void ApplyDamage(CombatantState victim, float amount, CombatantState attacker, bool critical, ISeededRng rng)
        {
            if (victim == null || amount <= 0f) return;

            victim.Health = MathfCompat.Max(0f, victim.Health - amount);

            if (victim.Health <= 0f)
            {
                if (victim.IsPlayer && victim.IsLastStand)
                {
                    // Last stand: instant death + mutual kill.
                    Kill(victim);
                    var enemies = LivingEnemies();
                    if (enemies.Count > 0)
                    {
                        var taken = enemies[0];
                        Kill(taken);
                        AddEvent("mutual_kill", victim.Id, victim.Name + " falls; a hostile is dragged down too.");
                    }
                    if (_ports.RaiseTrauma != null && !string.IsNullOrEmpty(victim.SurvivorId))
                        _ports.RaiseTrauma(victim.SurvivorId, "combat_trauma", 1f);
                    return;
                }

                if (!victim.IsDowned)
                {
                    // Downed + bleed-out.
                    victim.IsDowned = true;
                    victim.BleedTurnsRemaining = DefaultBleedTurns;
                    AddEvent("downed", victim.Id, victim.Name + " is downed and bleeding out.");
                    if (_ports.RaiseTrauma != null && !string.IsNullOrEmpty(victim.SurvivorId))
                        _ports.RaiseTrauma(victim.SurvivorId, "combat_injury", 1f);
                }
                else
                {
                    // Already downed and hit again → dead.
                    Kill(victim);
                }
            }
        }

        private void Kill(CombatantState c)
        {
            c.Health = 0f;
            c.HasFled = true; // removed from living pools (deterministic)
            c.IsDowned = false;
            c.BleedTurnsRemaining = 0;
            AddEvent("death", c.Id, c.Name + " is dead.");
            if (c.IsPlayer && !string.IsNullOrEmpty(c.SurvivorId) && _ports.DamageSurvivor != null)
                _ports.DamageSurvivor(c.SurvivorId, 99999f);
            if (!c.IsPlayer)
            {
                // A human kill has consequences.
                var players = LivingPlayers();
                for (int i = 0; i < players.Count; i++)
                {
                    var p = PerksFor(players[i].SurvivorId, _state.Seed);
                    p?.RecordHumanKill(players[i].SurvivorId);
                }
                if (_ports.ApplyMoraleDelta != null)
                    _ports.ApplyMoraleDelta(_state.EncounterId, -CombatPerks.HumanKillMoralePenalty * 0.25f);
            }
        }

        /// <summary>End the player's turn: enemies act, bleed-out ticks, pins decay.</summary>
        public CombatActionResult EndTurn(ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }

            _state.Phase = (int)CombatPhase.EnemyTurn;

            // Bleed-out ticks first (only units downed in EARLIER turns burn turns,
            // so a fresh wound gets a full bleed-out window).
            TickBleedOut();

            // Enemies fire at player targets.
            var enemies = LivingEnemies();
            var players = LivingPlayers();
            var stance = CurrentStance();
            var mods = GetStanceMods(stance);

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                if (enemy.IsPinned || enemy.IsDowned) continue; // suppressed: accuracy → 0, no fire
                if (players.Count == 0) break;

                var target = players[rng.Next(0, players.Count)];
                if (target.IsDowned) continue;

                // Enemy accuracy: base 0.5, defence reduces it.
                float acc = 0.50f * (1f - mods.Defense);
                if (rng.NextDouble() < acc)
                {
                    float dmg = 6f + (float)(enemy.Lane == target.Lane ? 4f : 0f);
                    ApplyDamage(target, dmg, enemy, false, rng);
                    AddEvent("enemy_fire", target.Id, enemy.Name + " hits " + target.Name + ".");
                }
                else
                {
                    AddEvent("enemy_fire", target.Id, enemy.Name + " misses " + target.Name + ".");
                }
            }

            // Pins decay.
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (c.IsPinned && !c.IsPlayer)
                {
                    c.PinnedTurnsRemaining--;
                    if (c.PinnedTurnsRemaining <= 0) c.IsPinned = false;
                }
            }

            _state.Turn++;
            _state.Phase = (int)CombatPhase.PlayerTurn;
            res.Success = true;
            res.Message = "Enemy turn complete.";
            Notify();
            CheckResolution();
            return res;
        }

        private void TickBleedOut()
        {
            var players = LivingPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (!c.IsDowned) continue;
                c.BleedTurnsRemaining--;
                AddEvent("bleed", c.Id, c.Name + " bleeding out (" + Math.Max(0, c.BleedTurnsRemaining) + " turns).");
                if (c.BleedTurnsRemaining <= 0)
                    Kill(c);
            }
        }

        /// <summary>Ash Dunes / environmental exposure jams every equipped player firearm.</summary>
        public CombatActionResult TickEnvironmental(float severity, ISeededRng rng)
        {
            var res = new CombatActionResult();
            int affected = 0;
            for (int i = 0; i < _state.Weapons.Count; i++)
            {
                var w = _state.Weapons[i];
                if (w.OwnerSurvivorId == null) continue;
                WeaponConditionSystem.ExposeToAsh(w, severity);
                affected++;
            }
            AddEvent("ash_dunes", _state.EncounterId, "Ash drifts clog " + affected + " firearm action(s).");
            res.Success = affected > 0;
            res.Message = affected + " weapon(s) fouled by ash.";
            Notify();
            return res;
        }

        /// <summary>Record a combat survived for every still-standing player (trauma hook).</summary>
        public void RecordSurvivorsSurvived()
        {
            var players = LivingPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (!c.IsDowned && !string.IsNullOrEmpty(c.SurvivorId))
                {
                    var p = PerksFor(c.SurvivorId, _state.Seed);
                    p?.RecordConfinedEncounterSurvived(c.SurvivorId);
                    if (_ports.MarkCombatSurvived != null) _ports.MarkCombatSurvived(c.SurvivorId);
                }
            }
        }

        // ══ Resolution ═══════════════════════════════════════════════════

        private void CheckResolution()
        {
            if (_state.Resolved) return;

            var enemies = LivingEnemies();
            var players = LivingPlayers();

            if (enemies.Count == 0)
            {
                _state.Phase = (int)CombatPhase.Won;
                _state.Resolved = true;
                _state.OutcomeText = "Hostiles neutralized.";
                if (_ports.ApplyMoraleDelta != null)
                    _ports.ApplyMoraleDelta(_state.EncounterId, +5f);
                GrantVictoryLoot();
                RecordSurvivorsSurvived();
                AddEvent("victory", _state.EncounterId, "Victory — " + _state.Loot.Count + " loot captured.");
                OnEncounterEnded?.Invoke(_state);
                Notify();
                return;
            }

            bool anyStanding = false;
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].IsDowned) { anyStanding = true; break; }
            }
            if (players.Count == 0 || !anyStanding)
            {
                _state.Phase = (int)CombatPhase.Lost;
                _state.Resolved = true;
                _state.OutcomeText = "The squad is wiped out.";
                if (_ports.ApplyMoraleDelta != null)
                    _ports.ApplyMoraleDelta(_state.EncounterId, -12f);
                AddEvent("defeat", _state.EncounterId, "Defeat — no survivors standing.");
                OnEncounterEnded?.Invoke(_state);
                Notify();
            }
        }

        private void GrantVictoryLoot()
        {
            var loot = new CombatLootEntry { itemId = "scrap_metal", quantity = 3, weightKg = 1.4f };
            _state.Loot.Add(loot);
            if (_ports.GrantLoot != null) _ports.GrantLoot(loot);
            var loot2 = new CombatLootEntry { itemId = "ammo_556", quantity = 6, weightKg = 0.6f };
            _state.Loot.Add(loot2);
            if (_ports.GrantLoot != null) _ports.GrantLoot(loot2);
        }

        /// <summary>Run deterministic turns until the encounter resolves (headless/demo).</summary>
        public List<CombatEvent> ResolveToEnd(ISeededRng rng, int maxTurns = 40)
        {
            var done = new List<CombatEvent>();
            int guard = 0;
            while (!_state.Resolved && guard++ < maxTurns)
            {
                var shooter = PickActiveShooter();
                var enemies = LivingEnemies();
                if (shooter != null && enemies.Count > 0)
                {
                    var target = enemies[rng.Next(0, enemies.Count)];
                    PlayerFire(target.Id, rng);
                }
                if (!_state.Resolved)
                    EndTurn(rng);
            }
            done.AddRange(_state.Events);
            return done;
        }

        /// <summary>Human-readable one-line outcome for logging/labels.</summary>
        public static string Describe(BallisticOutcome o)
        {
            switch (o.Result)
            {
                case BallisticResult.DirectHit:
                    return "direct hit (" + o.DamageDealt.ToString("0") + " dmg)" + (o.IsCritical ? " CRIT" : "");
                case BallisticResult.Missed:
                    return "miss";
                case BallisticResult.Blocked:
                    return "blocked by cover/barrier";
                case BallisticResult.Penetrated:
                    return "penetrated cover, " + o.DamageDealt.ToString("0") + " throughput";
                case BallisticResult.Ricocheted:
                    return "ricochet → " + o.ResolvedTargetId + " (" + o.DamageDealt.ToString("0") + " dmg)";
                case BallisticResult.Stopped:
                    return "stopped by armor";
                default:
                    return o.Result.ToString();
            }
        }

        /// <summary>Jam chance a given survivor's weapon would show in the UI — same value as resolution.</summary>
        public float UIJamChance(CombatantState c)
        {
            var w = WeaponOf(c);
            return w == null ? 0f : WeaponConditionSystem.ComputeJamChance(w);
        }

        // ══ Snapshot for the host / UI ════════════════════════════════════

        public CombatSnapshot BuildSnapshot()
        {
            var snap = new CombatSnapshot
            {
                EncounterId = _state.EncounterId,
                LocationName = _state.LocationName,
                Day = _state.Day,
                Turn = _state.Turn,
                Phase = ((CombatPhase)_state.Phase).ToString(),
                StanceId = _state.PlayerStance,
                Resolved = _state.Resolved,
                OutcomeText = _state.OutcomeText,
                IsActive = !string.IsNullOrEmpty(_state.EncounterId) && !_state.Resolved
            };

            var combatants = new List<CombatantState>(_state.Combatants);
            combatants.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            for (int i = 0; i < combatants.Count; i++)
            {
                var c = combatants[i];
                var w = WeaponOf(c);
                var csnap = new CombatantSnapshot
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPlayer = c.IsPlayer,
                    FactionId = c.FactionId,
                    Lane = ((CombatLane)MathfCompat.Clamp(c.Lane, 0, 2)).ToString(),
                    Health = (int)Math.Round(c.Health),
                    MaxHealth = (int)Math.Round(c.MaxHealth),
                    ArmorRating = (int)Math.Round(c.ArmorRating * 100f),
                    CoverRating = (int)Math.Round(c.CoverRating * 100f),
                    IsDowned = c.IsDowned,
                    IsPinned = c.IsPinned,
                    IsLastStand = c.IsLastStand,
                    WeaponName = w != null ? (CombatCatalog.GetWeapon(w.WeaponId)?.displayName ?? w.WeaponId) : "—",
                    WeaponConditionPct = w != null ? (int)Math.Round(w.ConditionPct * 100f) : 0,
                    WeaponJammed = w != null && w.IsJammed,
                    WeaponAmmo = w != null ? w.AmmoId : string.Empty
                };
                if (c.IsDowned) csnap.Status = "DOWNED (" + c.BleedTurnsRemaining + ")";
                else if (c.IsPinned) csnap.Status = "PINNED";
                else if (c.IsLastStand) csnap.Status = "LAST STAND";
                else csnap.Status = "OK";
                snap.Combatants.Add(csnap);
            }

            var weapons = new List<WeaponInstanceState>(_state.Weapons);
            weapons.Sort((a, b) => string.CompareOrdinal(a.InstanceId, b.InstanceId));
            for (int i = 0; i < weapons.Count; i++)
            {
                var w = weapons[i];
                var def = CombatCatalog.GetWeapon(w.WeaponId);
                snap.Weapons.Add(new WeaponSnapshot
                {
                    InstanceId = w.InstanceId,
                    WeaponId = w.WeaponId,
                    WeaponName = def?.displayName ?? w.WeaponId,
                    ConditionPct = (int)Math.Round(w.ConditionPct * 100f),
                    JamChancePct = (int)Math.Round(WeaponConditionSystem.ComputeJamChance(w) * 100f),
                    IsJammed = w.IsJammed,
                    ScrapRepairCost = WeaponConditionSystem.GetScrapRepairCost(w),
                    AmmoRemaining = w.AmmoRemaining,
                    OwnerSurvivorId = w.OwnerSurvivorId
                });
            }

            snap.Events.AddRange(_state.Events);
            snap.Loot.AddRange(_state.Loot);
            return snap;
        }

        // ══ Save / Load (deep-copy, deterministic ordering) ═══════════════

        public CombatState CaptureState()
        {
            var copy = new CombatState
            {
                SystemId = _state.SystemId,
                SaveVersion = CombatState.CurrentSaveVersion,
                EncounterId = _state.EncounterId,
                ExpeditionId = _state.ExpeditionId,
                LocationId = _state.LocationId,
                LocationName = _state.LocationName,
                Day = _state.Day,
                Seed = _state.Seed,
                Turn = _state.Turn,
                Phase = _state.Phase,
                PlayerStance = _state.PlayerStance,
                RoundNumber = _state.RoundNumber,
                Resolved = _state.Resolved,
                OutcomeText = _state.OutcomeText
            };
            copy.Combatants = CloneCombatants(_state.Combatants);
            copy.Weapons = CloneWeapons(_state.Weapons);
            copy.Barriers = CloneBarriers(_state.Barriers);
            copy.Events = CloneEvents(_state.Events);
            copy.Loot.AddRange(_state.Loot);
            return copy;
        }

        /// <summary>
        /// Restore from a save. Migrates legacy/foreign fields with backward-
        /// compatible defaults (null-safe, clamps phase/lane).
        /// </summary>
        public void RestoreState(CombatState saved)
        {
            if (saved == null) return;
            var migrated = Migrate(saved);
            _state = migrated;
            Notify();
        }

        /// <summary>Migrate an older/foreign CombatState to the current version (null-safe).</summary>
        public static CombatState Migrate(CombatState s)
        {
            var m = new CombatState
            {
                SystemId = string.IsNullOrEmpty(s.SystemId) ? SystemId : s.SystemId,
                SaveVersion = CombatState.CurrentSaveVersion,
                EncounterId = s.EncounterId ?? string.Empty,
                ExpeditionId = s.ExpeditionId ?? string.Empty,
                LocationId = s.LocationId ?? string.Empty,
                LocationName = s.LocationName ?? string.Empty,
                Day = s.Day,
                Seed = s.Seed,
                Turn = Math.Max(1, s.Turn),
                Phase = MathfCompat.Clamp(s.Phase, (int)CombatPhase.Setup, (int)CombatPhase.Retreated),
                PlayerStance = string.IsNullOrEmpty(s.PlayerStance) ? StanceId(TacticalStance.HoldPosition) : s.PlayerStance,
                RoundNumber = s.RoundNumber,
                Resolved = s.Resolved,
                OutcomeText = s.OutcomeText ?? string.Empty
            };
            m.Combatants = CloneCombatants(s.Combatants);
            m.Weapons = CloneWeapons(s.Weapons);
            m.Barriers = CloneBarriers(s.Barriers);
            m.Events = CloneEvents(s.Events);
            if (s.Loot != null) m.Loot.AddRange(s.Loot);
            return m;
        }

        private static List<CombatantState> CloneCombatants(List<CombatantState> src)
        {
            var ordered = src ?? new List<CombatantState>();
            var copy = new List<CombatantState>();
            for (int i = 0; i < ordered.Count; i++)
            {
                var c = ordered[i];
                if (c == null) continue;
                copy.Add(CloneCombatant(c));
            }
            return copy;
        }

        private static CombatantState CloneCombatant(CombatantState c)
        {
            return new CombatantState
            {
                Id = c.Id ?? string.Empty,
                Name = c.Name ?? string.Empty,
                SurvivorId = c.SurvivorId ?? string.Empty,
                IsPlayer = c.IsPlayer,
                FactionId = c.FactionId ?? string.Empty,
                Lane = MathfCompat.Clamp(c.Lane, 0, 2),
                Health = c.Health,
                MaxHealth = MathfCompat.Max(1f, c.MaxHealth),
                ArmorRating = MathfCompat.Clamp01(c.ArmorRating),
                CoverRating = MathfCompat.Clamp01(c.CoverRating),
                IsDowned = c.IsDowned,
                BleedTurnsRemaining = c.BleedTurnsRemaining,
                IsPinned = c.IsPinned,
                PinnedTurnsRemaining = c.PinnedTurnsRemaining,
                IsLastStand = c.IsLastStand,
                WeaponInstanceId = c.WeaponInstanceId ?? string.Empty,
                HasFled = c.HasFled
            };
        }

        private static List<WeaponInstanceState> CloneWeapons(List<WeaponInstanceState> src)
        {
            var copy = new List<WeaponInstanceState>();
            var ordered = src != null ? new List<WeaponInstanceState>(src) : new List<WeaponInstanceState>();
            ordered.Sort((a, b) => string.CompareOrdinal(a.InstanceId, b.InstanceId));
            for (int i = 0; i < ordered.Count; i++)
            {
                var w = ordered[i];
                if (w == null) continue;
                copy.Add(CloneWeapon(w));
            }
            return copy;
        }

        private static WeaponInstanceState CloneWeapon(WeaponInstanceState w)
        {
            return new WeaponInstanceState
            {
                InstanceId = w.InstanceId ?? string.Empty,
                WeaponId = w.WeaponId ?? string.Empty,
                OwnerSurvivorId = w.OwnerSurvivorId ?? string.Empty,
                OwnerCombatantId = w.OwnerCombatantId ?? string.Empty,
                ConditionPct = MathfCompat.Clamp01(w.ConditionPct),
                IsJammed = w.IsJammed,
                JamClearTicksRemaining = w.JamClearTicksRemaining,
                JamsSurvived = w.JamsSurvived,
                ShotsFired = w.ShotsFired,
                BurstCount = w.BurstCount,
                CachedJamChance = MathfCompat.Clamp01(w.CachedJamChance),
                AshFoul = MathfCompat.Clamp01(w.AshFoul),
                AmmoId = w.AmmoId ?? string.Empty,
                AmmoRemaining = w.AmmoRemaining,
                ScrapRepairCost = w.ScrapRepairCost
            };
        }

        private static List<BarrierState> CloneBarriers(List<BarrierState> src)
        {
            var copy = new List<BarrierState>();
            var ordered = src != null ? new List<BarrierState>(src) : new List<BarrierState>();
            ordered.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            for (int i = 0; i < ordered.Count; i++)
            {
                var b = ordered[i];
                if (b == null) continue;
                copy.Add(new BarrierState { Id = b.Id ?? string.Empty, Lane = MathfCompat.Clamp(b.Lane, 0, 2), IsPlayer = b.IsPlayer, MaterialId = b.MaterialId ?? string.Empty, IntegrityPct = MathfCompat.Clamp01(b.IntegrityPct * 0.01f) * 100f, ArmorRating = b.ArmorRating });
            }
            return copy;
        }

        private static List<CombatEvent> CloneEvents(List<CombatEvent> src)
        {
            var copy = new List<CombatEvent>();
            var ordered = src != null ? new List<CombatEvent>(src) : new List<CombatEvent>();
            for (int i = 0; i < ordered.Count; i++)
            {
                var e = ordered[i];
                if (e == null) continue;
                copy.Add(new CombatEvent
                {
                    Kind = e.Kind ?? string.Empty,
                    Day = e.Day,
                    Turn = e.Turn,
                    SubjectId = e.SubjectId ?? string.Empty,
                    TargetId = e.TargetId ?? string.Empty,
                    Detail = e.Detail ?? string.Empty,
                    Value = e.Value
                });
            }
            return copy;
        }
    }
}
