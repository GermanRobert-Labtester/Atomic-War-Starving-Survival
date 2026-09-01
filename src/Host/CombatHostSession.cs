using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.PlayerCommand;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the combat core (Expansion 6). Presents the
    /// engine, wires host ports (inventory ammo/scrap/loot, survivor health/
    /// morale/injury) when available, persists to user:// via CombatSaveStore,
    /// and exposes typed snapshots + real player actions for the Combat panels.
    /// No gameplay rules live here — hosts only present and wire.
    /// </summary>
    public sealed class CombatHostSession
    : HostSessionBase{
        public const int DemoSeed = 4242;

        public TacticalCombatSystem Engine { get; }

        /// <summary>Optional real inventory backing for ammo / scrap / loot.</summary>
        public InventoryHostSession Inventory { get; set; }

        /// <summary>Optional real survivor backing for health / morale / injury.</summary>
        public SurvivorsHostSession Survivors { get; set; }

        /// <summary>
        /// Optional equipment-condition authority. When set, the default weapon
        /// loadout is projected from its Weapon-family instances (condition
        /// 0–100 → combat 0–1) and combat wear is written back through the
        /// authority when the encounter ends — one persisted condition per
        /// weapon, no duplicate durability. Unset: legacy demo literals.
        /// </summary>
        public Ashfall.Core.EquipmentConditionSystem? Equipment { get; set; }

        /// <summary>Condition-at-start of bridge-bound weapons, for the post-combat write-back.</summary>
        private readonly Dictionary<string, float> _boundWeaponConditionAtStart = new();

        public string LastEvent { get; private set; } = string.Empty;
        public CombatHostSession(TacticalCombatSystem engine = null!, CombatHostPorts ports = null!)
        {
            Engine = engine ?? new TacticalCombatSystem(null!, ports ?? CombatHostPorts.NoOp());
            Engine.OnStateChanged += _ => RaiseStateChanged();
            Engine.OnCombatEvent += (s, e) => { LastEvent = e.Detail; RaiseStateChanged(); };
            Engine.OnEncounterEnded += s =>
            {
                LastEvent = "Combat ended: " + s.OutcomeText;
                SyncBoundWeaponsAfterCombat(s);
                RaiseStateChanged();
            };
        }

        /// <summary>
        /// Single post-combat write-back point: bridge-bound weapons sync their
        /// condition delta into the equipment authority. Runs once per
        /// encounter end; the snapshot is cleared after syncing.
        /// </summary>
        private void SyncBoundWeaponsAfterCombat(CombatState state)
        {
            if (Equipment == null || _boundWeaponConditionAtStart.Count == 0 || state?.Weapons == null)
            {
                _boundWeaponConditionAtStart.Clear();
                return;
            }

            foreach (var weapon in state.Weapons)
            {
                if (weapon == null || string.IsNullOrEmpty(weapon.InstanceId)) continue;
                if (_boundWeaponConditionAtStart.TryGetValue(weapon.InstanceId, out float start))
                    Ashfall.Core.Combat.WeaponEquipmentBridge.SyncAfterCombat(Equipment, weapon, start);
            }
            _boundWeaponConditionAtStart.Clear();
        }

        /// <summary>
        /// Wire the engine's host ports to real inventory + survivor sessions
        /// when present. <paramref name="markCombatSurvived"/> is an explicit
        /// callback rather than a new session-reference property: the combat
        /// host doesn't otherwise depend on Phase0/trauma tracking, and a
        /// required-effect port is small enough to pass directly without
        /// widening the session's dependency surface.
        /// </summary>
        public void WireRealState(Action<string>? markCombatSurvived = null)
        {
            var prior = Engine.Ports ?? CombatHostPorts.NoOp();

            Func<string, int, int>? consumeAmmo = null;
            Func<string, int, bool>? consumeItem = null;
            Action<CombatLootEntry>? grantLoot = null;
            if (Inventory != null)
            {
                consumeAmmo = (ammoId, n) =>
                {
                    if (Inventory.Inventory.CountById(ammoId) >= n)
                    {
                        Inventory.Remove(ammoId, n);
                        return 999; // consumed; report ample stock remaining
                    }
                    return -1; // cannot afford -> action refused
                };
                consumeItem = (itemId, n) =>
                {
                    if (Inventory.Inventory.CountById(itemId) >= n)
                    {
                        Inventory.Remove(itemId, n);
                        return true;
                    }
                    return false;
                };
                grantLoot = l => Inventory.Add(l.itemId, l.quantity);
            }

            Func<string, float, float>? damageSurvivor = null;
            Func<string, float, float>? healSurvivor = null;
            Action<string, float>? applyMoraleDelta = null;
            if (Survivors != null)
            {
                damageSurvivor = (id, d) =>
                {
                    var s = Survivors.Find(id);
                    if (s == null) return d;
                    s.Health = MathfCompat.Max(0f, s.Health - d);
                    if (s.Health <= 0f) { s.IsAlive = false; s.IsDead = true; }
                    return s.Health;
                };
                healSurvivor = (id, h) =>
                {
                    var s = Survivors.Find(id);
                    if (s == null) return h;
                    s.Health = MathfCompat.Min(s.MaxHealthCap, s.Health + h);
                    return s.Health;
                };
                applyMoraleDelta = (id, m) =>
                {
                    var s = Survivors.Find(id);
                    if (s == null) return;
                    s.Morale = MathfCompat.Clamp(s.Morale + m, 0f, 100f);
                };
            }

            Engine.Ports = new CombatHostPorts(
                damageSurvivor,
                healSurvivor,
                applyMoraleDelta,
                consumeAmmo,
                consumeItem,
                prior.RaiseTrauma,
                grantLoot,
                markCombatSurvived ?? prior.MarkCombatSurvived);
        }

        /// <summary>
        /// Logs any production-required combat effects still unbound after
        /// <see cref="WireRealState"/>. An empty list means every health, morale,
        /// inventory, and progression effect reaches a real consumer.
        /// </summary>
        public void ValidatePorts()
        {
            var unbound = Engine.Ports.UnboundRequiredEffects;
            if (unbound.Count > 0)
            {
                GD.PrintErr("[Ashfall Godot] Combat host ports unbound: "
                    + string.Join(", ", unbound)
                    + ". Effects will silently no-op/fallback in production.");
            }
        }

        public static CombatHostSession Create(string dataDir)
        {
            // The weapon/ammo/material catalog is the data authority
            // (combat_catalog.json). Without loading it here, every real
            // PlayerFire() call in production silently returns "Unknown
            // weapon" (CombatCatalog.GetWeapon returns null) and the
            // encounter can only ever resolve by the enemy attacking an
            // unarmed-in-effect player — combat was never actually being
            // fought. Clear+reload is safe to call every time a session is
            // created: the catalog is a static registry shared by the whole
            // process, and the data authority never changes mid-session.
            if (!string.IsNullOrEmpty(dataDir))
            {
                try
                {
                    CombatCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Combat] combat_catalog.json load failed, falling back to defaults: {ex.Message}");
                }
            }
            if (CombatCatalog.GetWeapon("weapon_assault_rifle") == null)
                CombatCatalog.SeedDefaults();

            var session = new CombatHostSession();
            var save = CombatSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save);
                session.LastEvent = "Combat state restored from save.";
            }
            return session;
        }

        // ── Production Combat Entry Point ────────────────────────────────

        /// <summary>
        /// Start a tactical combat encounter at a location, sourcing survivors and
        /// weapons from live state when not explicitly provided.
        /// </summary>
        public string StartCombat(
            string locationId,
            string locationName,
            IReadOnlyList<CombatantState>? roster = null,
            IReadOnlyList<WeaponInstanceState>? weapons = null,
            int enemyCount = 0,
            int enemyHealth = 0,
            int? seed = null)
        {
            if (!Engine.State.Resolved && !string.IsNullOrEmpty(Engine.State.EncounterId)
                && Engine.State.Phase != (int)CombatPhase.Setup)
                return "Combat already active — finish or retreat first.";

            var players = new List<CombatantState>();
            if (roster != null && roster.Count > 0)
            {
                players.AddRange(roster);
            }
            else if (Survivors != null && Survivors.RosterState.Count > 0)
            {
                foreach (var s in Survivors.RosterState)
                {
                    if (s == null || !s.IsAlive) continue;
                    players.Add(new CombatantState
                    {
                        Id = "p_" + s.Id.Replace("survivor_", ""),
                        Name = s.Id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant(),
                        SurvivorId = s.Id,
                        IsPlayer = true,
                        Health = (int)Math.Max(1f, s.Health),
                        MaxHealth = (int)Math.Max(1f, s.MaxHealthCap),
                        ArmorRating = 0.4f,
                        CoverRating = 0.3f
                    });
                    if (players.Count >= 4) break;
                }
            }

            if (players.Count == 0)
            {
                players.Add(new CombatantState { Id = "p_yuki", Name = "Yuki", SurvivorId = "survivor_yuki", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.4f, CoverRating = 0.3f });
                players.Add(new CombatantState { Id = "p_mikhail", Name = "Gunner Mikhail", SurvivorId = "survivor_gunner_mikhail", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.5f, CoverRating = 0.2f });
            }

            var weaponList = new List<WeaponInstanceState>();
            if (weapons != null && weapons.Count > 0)
            {
                weaponList.AddRange(weapons);
            }
            else
            {
                for (int i = 0; i < players.Count; i++)
                {
                    var p = players[i];
                    string wId = i == 0 ? "weapon_assault_rifle" : "weapon_pipe_rifle";
                    string aId = i == 0 ? "ammo_556" : "ammo_357";
                    // Project the persisted equipment authority when it tracks
                    // this weapon; otherwise fall back to the demo literal.
                    var token = Ashfall.Core.Combat.WeaponEquipmentBridge.ToCombatInstance(
                        Equipment, wId, p.SurvivorId);
                    bool bound = !string.IsNullOrEmpty(token.InstanceId);
                    weaponList.Add(new WeaponInstanceState
                    {
                        InstanceId = bound ? token.InstanceId : "w_" + p.Id,
                        WeaponId = wId,
                        OwnerSurvivorId = p.SurvivorId,
                        ConditionPct = bound ? token.ConditionPct : 0.9f,
                        AmmoId = aId,
                        AmmoRemaining = 50
                    });
                    if (bound)
                        _boundWeaponConditionAtStart[token.InstanceId] = token.ConditionPct;
                }
            }

            int finalEnemyCount = enemyCount > 0 ? enemyCount : 3;
            int finalEnemyHealth = enemyHealth > 0 ? enemyHealth : 45;

            bool ok = Engine.BeginEncounter(
                "enc_" + locationId + "_" + ScheduleDay(),
                "exp_" + locationId,
                locationId,
                locationName ?? locationId,
                ScheduleDay(),
                seed ?? DemoSeed,
                players,
                weaponList,
                enemyCount: finalEnemyCount,
                enemyHealth: finalEnemyHealth);

            return ok ? "Combat engaged at " + (locationName ?? locationId) + "." : "Could not start combat.";
        }

        public string StartDemoCombat(string locationId, string locationName)
            => StartCombat(locationId, locationName);

        public int ScheduleDay()
        {
            // Deterministic day from the engine state (host may override with the real clock).
            return Engine.State.Day > 0 ? Engine.State.Day : 1;
        }

        // ── Player actions (return the human message + refresh UI) ─────

        public string ActionStance(string stanceId)
        {
            if (TacticalCombatSystem.TryParseStance(stanceId, out var s))
            {
                var r = Engine.SetStance(s);
                return r.Message;
            }
            return "Unknown stance: " + stanceId;
        }

        public string ActionFire(string targetId)
        {
            var r = Engine.PlayerFire(targetId, new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionSuppress()
        {
            var r = Engine.PlayerSuppress(new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionClearJam(string subjectId)
        {
            var r = Engine.PlayerClearJam(subjectId, new SeededRng(RollSeed()));
            return r.Message;
        }

        public CommandResult ActionRepair(string subjectId)
        {
            var r = Engine.ExecutePlayerFieldRepair(subjectId, expectedStateVersion: StateVersion, currentStateVersion: StateVersion);
            if (r.IsSuccess)
            {
                LastEvent = r.FailureCode == string.Empty ? "Field repair completed." : $"Field repair: {r.FailureCode}";
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Field repair refused: {r.FailureCode}.";
            }
            return r;
        }

        public string ActionMoveLane(string subjectId, CombatLane lane)
        {
            var r = Engine.PlayerMoveLane(subjectId, lane, new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionDeployTrap()
        {
            var r = Engine.PlayerDeployTrap(new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionDecontaminate()
        {
            var r = Engine.PlayerDecontaminate(new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionBandage(string rescuerId, string downedId)
        {
            var r = Engine.PlayerBandage(rescuerId, downedId, new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionRetreat()
        {
            var r = Engine.PlayerRetreat(new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionLastStand(string subjectId)
        {
            var r = Engine.PlayerLastStand(subjectId, new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionEndTurn()
        {
            var r = Engine.EndTurn(new SeededRng(RollSeed()));
            return r.Message;
        }

        public string ActionEnvironmental(float severity)
        {
            var r = Engine.TickEnvironmental(severity, new SeededRng(RollSeed()));
            return r.Message;
        }

        /// <summary>Deterministic roll seed for this action (host owns seeding).</summary>
        private int RollSeed()
        {
            unchecked
            {
                int day = Engine.State.Day;
                int turn = Engine.State.Turn;
                return (DemoSeed * 31) + (day * 7) + (turn * 13);
            }
        }

        // ── Snapshot / status ───────────────────────────────────────────

        public CombatSnapshot Snapshot() => Engine.BuildSnapshot();

        public string StatusLine()
        {
            if (string.IsNullOrEmpty(Engine.State.EncounterId) ||
                (Engine.State.Resolved && Engine.State.Phase == (int)CombatPhase.Setup))
                return "Combat: none active.";

            var snap = Engine.BuildSnapshot();
            var sb = new System.Text.StringBuilder();
            sb.Append("Combat [").Append(snap.LocationName).Append("] phase ").Append(snap.Phase)
              .Append(" · turn ").Append(snap.Turn).Append(" · stance ").Append(snap.StanceId);
            foreach (var c in snap.Combatants)
            {
                sb.Append('\n').Append(c.IsPlayer ? "  ▶ " : "  ● ")
                  .Append(c.Name).Append(" (").Append(c.Lane).Append(") HP ")
                  .Append(c.Health).Append('/').Append(c.MaxHealth)
                  .Append(c.IsDowned ? " [DOWNED]" : "")
                  .Append(c.IsPinned ? " [PINNED]" : "")
                  .Append(" · ").Append(c.WeaponName).Append(" ").Append(c.WeaponConditionPct).Append("%")
                  .Append(c.WeaponJammed ? " [JAM]" : "");
            }
            if (snap.Resolved) sb.Append('\n').Append("OUTCOME: ").Append(snap.OutcomeText);
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ─────────────────────────────────────────────────

        public CombatState CaptureSave() => Engine.CaptureState();
        public void RestoreSave(CombatState state) => Engine.RestoreState(state);

        public bool TryPersist() => CombatSaveStore.TrySave(Engine.CaptureState());
        public CombatState? TryRestorePersisted() => CombatSaveStore.TryLoad();
    }
}
