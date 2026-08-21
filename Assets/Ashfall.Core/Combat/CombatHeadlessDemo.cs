using System;
using System.Collections.Generic;
using System.Text;

namespace Ashfall.Core.Combat
{
    using Ashfall.Core;

    /// <summary>Headless-report payload for the Combat slice.</summary>
    public class CombatHeadlessReport : HeadlessReport
    {
        public CombatState FinalState;
        public CombatSnapshot Snapshot;
    }

    /// <summary>
    /// Vertical-slice smoke + deterministic-replay proof for the Combat
    /// expansion. Drives a full encounter (stances, fire, suppression, jam,
    /// bleed, retreat/last-stand paths), verifies weapon condition/jam/repair,
    /// Ash-Dunes jamming, save/restore deep-copy, and identical replay from the
    /// same seed + state. Invoked by `dotnet test` and `godot -- --combat-selftest`.
    /// </summary>
    public static class CombatHeadlessDemo
    {
        public const int DefaultSeed = 1337;
        public const int EnemyCount = 3;
        public const float EnemyHealth = 40f;

        public static CombatHeadlessReport Run(ILog log = null)
        {
            log = log ?? NullLog.Instance;
            var report = new CombatHeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) { report.PassedCount++; log.Info("[PASS] " + name); }
                else { report.FailedCount++; log.Error("[FAIL] " + name); }
            }

            log.Info("[CombatHeadlessDemo] begin");
            CombatCatalog.SeedDefaults();

            // ── 1. Weapon condition / jam / repair unit passes ──
            var weapon = new WeaponInstanceState
            {
                InstanceId = "w1",
                WeaponId = "weapon_pipe_rifle",
                OwnerSurvivorId = "survivor_a",
                ConditionPct = 0.9f,
                AmmoId = "ammo_357",
                AmmoRemaining = 40
            };
            float c0 = WeaponConditionSystem.ComputeJamChance(weapon);
            Check(c0 > 0.01f && c0 <= 1f, "jam chance is a computed probability (pipe rifle)");
            Check(WeaponConditionSystem.GetScrapRepairCost(weapon) >= 1, "pristine pipe rifle has a positive scrap repair cost");

            // degrade it to poor condition → jam chance rises
            WeaponConditionSystem.Degrade(weapon, 0.72f);
            float cLow = WeaponConditionSystem.ComputeJamChance(weapon);
            Check(cLow > c0, "degraded condition raises jam chance");
            WeaponConditionSystem.ClearJam(weapon);

            // ── 2. Environmental Ash-Dunes clogging ──
            var ashWeapon = new WeaponInstanceState
            {
                InstanceId = "wA",
                WeaponId = "weapon_assault_rifle",
                OwnerSurvivorId = "survivor_a",
                ConditionPct = 1f,
                AmmoId = "ammo_556"
            };
            float beforeAsh = ashWeapon.ConditionPct;
            WeaponConditionSystem.ExposeToAsh(ashWeapon, 1f);
            Check(ashWeapon.IsJammed, "ash dunes jam the firearm");
            Check(ashWeapon.ConditionPct < beforeAsh, "ash dunes degrade the firearm");
            Check(WeaponConditionSystem.ComputeJamChance(ashWeapon) >= 0.5f, "ash-fouled jam chance is severe");

            // ── 3. Burst failure: pipe + military ammo can burst ──
            var pipe = new WeaponInstanceState
            {
                InstanceId = "wB",
                WeaponId = "weapon_pipe_rifle",
                OwnerSurvivorId = "survivor_a",
                ConditionPct = 0.5f,
                AmmoId = "ammo_556" // military-tier
            };
            // Deterministic: force the burst roll by testing if any seed triggers it,
            // and verify a burst wrecks the weapon.
            bool burstTriggered = false;
            for (int seed = 1; seed < 60; seed++)
            {
                var probe = new SeededRng(seed);
                var probeWeapon = new WeaponInstanceState
                {
                    InstanceId = "wB",
                    WeaponId = "weapon_pipe_rifle",
                    OwnerSurvivorId = "survivor_a",
                    ConditionPct = 0.5f,
                    AmmoId = "ammo_556"
                };
                if (WeaponConditionSystem.TryWeaponBurst(probeWeapon, probe))
                {
                    burstTriggered = true;
                    Check(probeWeapon.ConditionPct < 0.1f, "burst failure wrecks the weapon condition");
                    break;
                }
            }
            Check(burstTriggered, "pipe rifle with military ammo can burst (observed across seeds)");

            // ── 4. Full encounter: victory path ──
            var ports = new CombatHostPorts();
            int moraleClicked = 0;
            int lootGranted = 0;
            int survivorDamaged = 0;
            int traumaRaised = 0;
            int ammoConsumed = 0;
            ports.ApplyMoraleDelta = (id, d) => moraleClicked++;
            ports.GrantLoot = l => lootGranted++;
            ports.DamageSurvivor = (id, d) => { survivorDamaged++; return Math.Max(0f, 100f - d); };
            ports.RaiseTrauma = (id, k, s) => traumaRaised++;
            ports.ConsumeAmmo = (ammoId, n) => { ammoConsumed += n; return 100 - ammoConsumed; };
            ports.ConsumeItem = (id, n) => id == "scrap_metal";

            var sys = new TacticalCombatSystem(null, ports);
            var players = new List<CombatantState>
            {
                new CombatantState { Id = "p1", Name = "Yuki", SurvivorId = "survivor_yuki", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.4f, CoverRating = 0.3f },
                new CombatantState { Id = "p2", Name = "Gunner", SurvivorId = "survivor_gunner", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.5f, CoverRating = 0.2f }
            };
            var pweapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState { InstanceId = "wp1", WeaponId = "weapon_assault_rifle", OwnerSurvivorId = "survivor_yuki", ConditionPct = 0.95f, AmmoId = "ammo_556", AmmoRemaining = 60 },
                new WeaponInstanceState { InstanceId = "wp2", WeaponId = "weapon_pipe_rifle", OwnerSurvivorId = "survivor_gunner", ConditionPct = 0.8f, AmmoId = "ammo_357", AmmoRemaining = 40 }
            };

            bool started = sys.BeginEncounter("enc_demo_1", "exp_demo", "loc_denial_cut", "The Denial Cut", 12, DefaultSeed, players, pweapons, EnemyCount, EnemyHealth);
            Check(started, "encounter starts");

            sys.SetStance(TacticalStance.HoldPosition, "survivor_yuki");
            Check(sys.State.PlayerStance == TacticalCombatSystem.StanceId(TacticalStance.HoldPosition), "stance set & serialized");

            // Fire until resolved (deterministic).
            sys.ResolveToEnd(new SeededRng(DefaultSeed), 60);
            Check(sys.State.Resolved, "encounter resolves to a terminal state");
            Check(sys.State.Phase == (int)CombatPhase.Won || sys.State.Phase == (int)CombatPhase.Lost || sys.State.Phase == (int)CombatPhase.Retreated, "terminal phase is Won/Lost/Retreated");
            Check(moraleClicked > 0, "victory/defeat propagated morale through host port");
            if (sys.State.Phase == (int)CombatPhase.Won)
                Check(lootGranted > 0, "victory granted loot through host port");
            Check(ammoConsumed > 0, "firing consumed real ammunition through host port");

            // ── 5. Save → restore deep-copy ✓
            var save = sys.CaptureState();
            Check(save != null && save.Combatants.Count == players.Count + EnemyCount, "capture deep-copies full roster");
            if (save == null) return report;
            Check(save.Weapons.Count == 2, "capture retains both weapons");
            Check(save.Events.Count > 0, "capture retains combat history");

            var restored = new TacticalCombatSystem();
            restored.RestoreState(save);
            Check(restored.State.Resolved == sys.State.Resolved, "restored resolution flag matches");
            Check(restored.CaptureState().Events.Count == save.Events.Count, "restore round-trip preserves event history");

            // ── 6. Deterministic replay from same seed + state ──
            var sysA = MakeEngine();
            var portsA = new CombatHostPorts();
            sysA.Ports = portsA;
            var seqA = RunScenario(sysA);

            var sysB = MakeEngine();
            var portsB = new CombatHostPorts();
            sysB.Ports = portsB;
            var seqB = RunScenario(sysB);

            Check(seqA.Count == seqB.Count, "replay produces the same number of events");
            bool identical = seqA.Count == seqB.Count;
            for (int i = 0; i < seqA.Count && identical; i++)
            {
                if (!string.Equals(seqA[i], seqB[i], StringComparison.Ordinal))
                    identical = false;
            }
            Check(identical, "replay from same seed reproduces identical event log");

            // ── 7. Migration from a legacy/older save shape ──
            var legacy = new CombatState { SaveVersion = 1 };
            // legacy combatant without clamps
            legacy.Combatants.Add(new CombatantState { Id = "l1", Name = "legacy", IsPlayer = true });
            legacy.Phase = 99; // out-of-range phase
            var migrated = TacticalCombatSystem.Migrate(legacy);
            Check(migrated != null && migrated.SaveVersion == CombatState.CurrentSaveVersion, "migration bumps save version");
            if (migrated == null) return report;
            Check(migrated.Phase >= (int)CombatPhase.Setup && migrated.Phase <= (int)CombatPhase.Retreated, "migration clamps out-of-range phase");
            Check(migrated.Combatants.Count == 1, "migration preserves legacy combatants null-safely");

            report.FinalState = sys.CaptureState();
            report.Snapshot = sys.BuildSnapshot();

            // ── 8. Expedition → combat handoff (raiding / ambush seam) ──
            var expSys = new Ashfall.Core.Expeditions.ExpeditionSystem();
            var handoff = new TacticalCombatSystem(null, new CombatHostPorts());
            int triggered = 0;
            expSys.OnEncounterTriggered += st =>
            {
                triggered++;
                var hPlayers = new List<CombatantState>
                {
                    new CombatantState { Id = "p1", Name = "Yuki", SurvivorId = "svy", IsPlayer = true, Health = 100, MaxHealth = 100 }
                };
                var hWeapons = new List<WeaponInstanceState>
                {
                    new WeaponInstanceState { InstanceId = "wh1", WeaponId = "weapon_assault_rifle", OwnerSurvivorId = "svy", ConditionPct = 1f, AmmoId = "ammo_556", AmmoRemaining = 60 }
                };
                handoff.BeginEncounter("enc_handoff_" + st.locationId, st.expeditionId, st.locationId,
                    st.displayName, st.startedDay, DefaultSeed + 1, hPlayers, hWeapons, 1, 30);
            };
            var ambushDef = new Ashfall.Core.Expeditions.ExpeditionDefinition
            {
                id = "loc_ambush_ridge", displayName = "Ambush Ridge",
                distanceTicks = 2, dangerLevel = 3, encounterChancePerTick = 1.0f,
                baseStaminaDrainPerHour = 1f
            };
            Ashfall.Core.Expeditions.ExpeditionDefinitionRegistry.Register(ambushDef);
            expSys.Start(ambushDef, "sv_handoff", 1, Ashfall.Core.Expeditions.ExpeditionStance.Stealth);
            for (int t = 0; t < 12 && triggered == 0; t++)
                expSys.TickHours(2f, new SeededRng(DefaultSeed + 2));
            Check(triggered > 0, "expedition travel triggers an encounter");
            Check(!string.IsNullOrEmpty(handoff.State.EncounterId)
                && handoff.State.Phase == (int)CombatPhase.PlayerTurn,
                "encounter handoff populates active combat");
            Ashfall.Core.Expeditions.ExpeditionDefinitionRegistry.Clear();
            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("CombatHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }

        // ── helpers for the deterministic-replay check ──
        private static TacticalCombatSystem MakeEngine()
        {
            var sys = new TacticalCombatSystem();
            var players = new List<CombatantState>
            {
                new CombatantState { Id = "p1", Name = "Yuki", SurvivorId = "survivor_yuki", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.4f, CoverRating = 0.3f },
                new CombatantState { Id = "p2", Name = "Gunner", SurvivorId = "survivor_gunner", IsPlayer = true, Health = 100, MaxHealth = 100 }
            };
            var pw = new List<WeaponInstanceState>
            {
                new WeaponInstanceState { InstanceId = "wp1", WeaponId = "weapon_assault_rifle", OwnerSurvivorId = "survivor_yuki", ConditionPct = 0.95f, AmmoId = "ammo_556", AmmoRemaining = 60 },
                new WeaponInstanceState { InstanceId = "wp2", WeaponId = "weapon_pipe_rifle", OwnerSurvivorId = "survivor_gunner", ConditionPct = 0.8f, AmmoId = "ammo_357", AmmoRemaining = 40 }
            };
            sys.BeginEncounter("enc_rep", "exp", "loc_x", "Loc", 1, DefaultSeed, players, pw, EnemyCount, EnemyHealth);
            return sys;
        }

        private static List<string> RunScenario(TacticalCombatSystem sys)
        {
            var rng = new SeededRng(DefaultSeed);
            int guard = 0;
            while (!sys.State.Resolved && guard++ < 40)
            {
                var res = sys.PlayerFire(TargetOf(sys)!, rng);
                if (!sys.State.Resolved)
                    sys.EndTurn(rng);
            }
            // Determinism proof: return the ordered combat-event log details.
            var details = new List<string>();
            foreach (var e in sys.State.Events)
                details.Add(e.Detail);
            return details;
        }

        private static string? TargetOf(TacticalCombatSystem sys)
        {
            var enemies = sys.State.Combatants.FindAll(c => !c.IsPlayer && !c.HasFled);
            if (enemies.Count == 0) return null;
            enemies.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            return enemies[0].Id;
        }
    }
}
