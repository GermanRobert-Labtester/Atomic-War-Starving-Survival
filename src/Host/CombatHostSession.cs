using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.Combat;
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

        public string LastEvent { get; private set; } = string.Empty;
        public CombatHostSession(TacticalCombatSystem engine = null!, CombatHostPorts ports = null!)
        {
            Engine = engine ?? new TacticalCombatSystem(null!, ports ?? new CombatHostPorts());
            Engine.OnStateChanged += _ => RaiseStateChanged();
            Engine.OnCombatEvent += (s, e) => { LastEvent = e.Detail; RaiseStateChanged(); };
            Engine.OnEncounterEnded += s =>
            {
                LastEvent = "Combat ended: " + s.OutcomeText;
                RaiseStateChanged();
            };
        }

        /// <summary>Wire the engine's host ports to real inventory + survivor sessions when present.</summary>
        public void WireRealState()
        {
            var ports = Engine.Ports ?? new CombatHostPorts();

            if (Inventory != null)
            {
                ports.ConsumeAmmo = (ammoId, n) =>
                {
                    if (Inventory.Inventory.CountById(ammoId) >= n)
                    {
                        Inventory.Remove(ammoId, n);
                        return 999; // consumed; report ample stock remaining
                    }
                    return -1; // cannot afford -> action refused
                };
                ports.ConsumeItem = (itemId, n) =>
                {
                    if (Inventory.Inventory.CountById(itemId) >= n)
                    {
                        Inventory.Remove(itemId, n);
                        return true;
                    }
                    return false;
                };
                ports.GrantLoot = l => Inventory.Add(l.itemId, l.quantity);
            }

            if (Survivors != null)
            {
                ports.DamageSurvivor = (id, d) =>
                {
                    var s = Survivors.Find(id);
                    if (s == null) return d;
                    s.Health = MathfCompat.Max(0f, s.Health - d);
                    if (s.Health <= 0f) { s.IsAlive = false; s.IsDead = true; }
                    return s.Health;
                };
                ports.HealSurvivor = (id, h) =>
                {
                    var s = Survivors.Find(id);
                    if (s == null) return h;
                    s.Health = MathfCompat.Min(s.MaxHealthCap, s.Health + h);
                    return s.Health;
                };
                ports.ApplyMoraleDelta = (id, m) =>
                {
                    var s = Survivors.Find(id);
                    if (s == null) return;
                    s.Morale = MathfCompat.Clamp(s.Morale + m, 0f, 100f);
                };
            }

            Engine.Ports = ports;
        }

        public static CombatHostSession Create(string dataDir)
        {
            var session = new CombatHostSession();
            var save = CombatSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save);
                session.LastEvent = "Combat state restored from save.";
            }
            return session;
        }

        // ── Demo entry point ─────────────────────────────────────────────

        /// <summary>Start a scripted raider encounter at a location (vertical-slice entry point).</summary>
        public string StartDemoCombat(string locationId, string locationName)
        {
            if (!Engine.State.Resolved && string.IsNullOrEmpty(Engine.State.EncounterId) == false
                && Engine.State.Phase != (int)CombatPhase.Setup)
                return "Combat already active — finish or retreat first.";

            var players = new List<CombatantState>
            {
                new CombatantState { Id = "p_yuki", Name = "Yuki", SurvivorId = "survivor_yuki", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.4f, CoverRating = 0.3f },
                new CombatantState { Id = "p_mikhail", Name = "Gunner Mikhail", SurvivorId = "survivor_gunner_mikhail", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.5f, CoverRating = 0.2f }
            };
            var weapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState { InstanceId = "w_yuki", WeaponId = "weapon_assault_rifle", OwnerSurvivorId = "survivor_yuki", ConditionPct = 0.95f, AmmoId = "ammo_556", AmmoRemaining = 60 },
                new WeaponInstanceState { InstanceId = "w_mikhail", WeaponId = "weapon_pipe_rifle", OwnerSurvivorId = "survivor_gunner_mikhail", ConditionPct = 0.8f, AmmoId = "ammo_357", AmmoRemaining = 40 }
            };

            bool ok = Engine.BeginEncounter(
                "enc_demo_" + locationId,
                "exp_" + locationId,
                locationId,
                locationName ?? locationId,
                ScheduleDay(),
                DemoSeed,
                players, weapons, enemyCount: 3, enemyHealth: 45);
            return ok ? "Combat engaged at " + locationName + "." : "Could not start combat.";
        }

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

        public string ActionRepair(string subjectId)
        {
            var r = Engine.PlayerFieldRepair(subjectId, new SeededRng(RollSeed()));
            return r.Message;
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
