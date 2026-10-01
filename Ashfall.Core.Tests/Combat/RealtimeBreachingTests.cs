// SPDX-License-Identifier: MIT
// T21 — realtime breaching: the phase guards admit ActiveRealtime and the
// realtime clock auto-advances active breaches once per sim second.
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Combat
{
    public class RealtimeBreachingTests
    {
        public RealtimeBreachingTests()
        {
            CombatCatalog.SeedDefaults();
            CombatArenaCatalog.SeedDefaults();
        }

        // Same data-authority loading the turn-based breaching tests use.
        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 10 && dir != null; i++)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "breaching_equipment_catalog.json");
                if (File.Exists(probe))
                    return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Directory.GetParent(dir)?.FullName;
            }
            string cwd = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (File.Exists(Path.Combine(cwd, "breaching_equipment_catalog.json")))
                return cwd;
            throw new DirectoryNotFoundException(
                "Assets/StreamingAssets/Data/breaching_equipment_catalog.json not found from " + AppContext.BaseDirectory);
        }

        private static void LoadBreachingCatalog(TacticalCombatSystem sys)
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = BreachingCatalogLoader.Load(FindDataDir(), files, json);
            BreachingCatalogLoader.Validate(catalog);
            sys.LoadBreachingCatalog(catalog);
            // Plan B86 fail-closed: breach tools require bound logistics stock.
            var inv = new InventoryContainer { Capacity = 64, MaxWeight = 500f };
            Assert.True(inv.TryProduce("item_hydraulic_wire_cutter", 1), "stock cutter");
            sys.ConfigureBreachingLogistics(inv, vehicleAvailable: false);
        }

        private static TacticalCombatSystem BeginFight(int seed = 42, bool enableRealtime = true)
        {
            var sys = new TacticalCombatSystem();
            var roster = new List<CombatantState>
            {
                new CombatantState
                {
                    Id = "p0",
                    Name = "Scout",
                    SurvivorId = "survivor_0",
                    IsPlayer = true,
                    Health = 100,
                    MaxHealth = 100,
                    Lane = 1
                }
            };
            var weapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState
                {
                    InstanceId = "w0",
                    WeaponId = "weapon_assault_rifle",
                    OwnerSurvivorId = "survivor_0",
                    ConditionPct = 0.9f,
                    AmmoId = "ammo_556",
                    AmmoRemaining = 30
                }
            };
            Assert.True(sys.BeginEncounter(
                "enc_rt", "exp_rt", "loc_rt", "Realtime Yard", 1, seed,
                roster, weapons, enemyCount: 1, enemyHealth: 5000f));
            if (enableRealtime)
                Assert.True(sys.EnableRealtime(CombatArenaCatalog.DefaultArenaId, new SeededRng(seed)));
            return sys;
        }

        private static readonly CombatInputFrame Idle = new CombatInputFrame { SubjectId = "p0" };

        [Fact]
        public void EvaluateAndBeginBreach_AreLegalInRealtime()
        {
            var sys = BeginFight(42);
            LoadBreachingCatalog(sys);
            var barrier = sys.EnsureObstacleBarrier("bar_wire", "obstacle_concertina_wire");

            var pf = sys.EvaluateBreach(barrier.Id, "breach_tool_hydraulic_cutter");
            Assert.True(pf.CanExecute, "breach must be legal in realtime: " + pf.Reason);

            var begin = sys.BeginBreach(barrier.Id, "breach_tool_hydraulic_cutter");
            Assert.True(begin.Success, begin.Message);
            Assert.NotEqual(BreachPhaseIds.Available, barrier.BreachPhase);
        }

        [Fact]
        public void RealtimeBreach_AutoAdvances_AndClears()
        {
            var sys = BeginFight(77);
            LoadBreachingCatalog(sys);
            var barrier = sys.EnsureObstacleBarrier("bar_wire", "obstacle_concertina_wire");
            Assert.True(sys.BeginBreach(barrier.Id, "breach_tool_hydraulic_cutter").Success);

            var rng = new SeededRng(77);
            const float dt = TacticalCombatSystem.RealtimeSimDt;
            for (int i = 0; i < 6000 && barrier.BreachPhase != BreachPhaseIds.Cleared; i++)
                Assert.True(sys.TickRealtime(dt, Idle, rng).Success);

            Assert.True(sys.State.Resolved || barrier.BreachPhase == BreachPhaseIds.Cleared
                || barrier.BreachPhase == BreachPhaseIds.Failed,
                "realtime breach must progress, not stall");
            Assert.True(sys.State.Resolved
                || barrier.BreachProgress01 > 0f
                || barrier.BreachPhase == BreachPhaseIds.Failed,
                "breach progress must have advanced under the realtime clock");
        }

        [Fact]
        public void LegacyTurnBasedBreach_StillWorks()
        {
            // Turn-based authority: BeginEncounter alone (no EnableRealtime)
            // leaves the encounter in PlayerTurn — the original breach path.
            var sys = BeginFight(21, enableRealtime: false);
            LoadBreachingCatalog(sys);
            var barrier = sys.EnsureObstacleBarrier("bar_wire", "obstacle_concertina_wire");
            Assert.True(sys.BeginBreach(barrier.Id, "breach_tool_hydraulic_cutter").Success);
            var advance = sys.AdvanceBreach(barrier.Id, new SeededRng(21));
            Assert.True(advance.Success || sys.State.Resolved, advance.Message);
        }
    }
}
