// SPDX-License-Identifier: MIT
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
    public class CombatBreachingEngineTests
    {
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

        private static BreachingCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = BreachingCatalogLoader.Load(FindDataDir(), files, json);
            BreachingCatalogLoader.Validate(catalog);
            return catalog;
        }

        private static CombatBreachingEngine CreateEngine(int seed = 86)
        {
            var engine = new CombatBreachingEngine(new SeededRng(seed));
            engine.LoadCatalog(LoadCatalog());
            return engine;
        }

        private static InventoryContainer MakeInv(params (string id, int qty)[] stock)
        {
            var inv = new InventoryContainer { Capacity = 64, MaxWeight = 500f };
            foreach (var (id, qty) in stock)
                Assert.True(inv.TryProduce(id, qty), "stock " + id);
            return inv;
        }

        private static BarrierState MakeBarrier(string profileId, string id = "bar_1")
        {
            return new BarrierState
            {
                Id = id,
                Lane = (int)CombatLane.Center,
                IsPlayer = true,
                ObstacleProfileId = profileId,
                IntegrityPct = 100f,
                PathBlocking = 1f,
                BreachPhase = BreachPhaseIds.Available
            };
        }

        [Fact]
        public void Catalog_Loads_TwelveObstacles_FiveTools()
        {
            var catalog = LoadCatalog();
            Assert.Equal(1, catalog.schema_version);
            Assert.Equal(12, catalog.obstacles.Count);
            Assert.Equal(5, catalog.tools.Count);
        }

        [Fact]
        public void Evaluate_Rejects_UnsupportedMethod()
        {
            var engine = CreateEngine();
            var barrier = MakeBarrier("obstacle_anti_vehicle_hedgehog");
            var eval = engine.Evaluate(barrier, "breach_tool_manual_shears", inventory: MakeInv(("item_manual_bolt_shears", 1)));
            Assert.False(eval.CanBegin);
            Assert.Equal("unsupported_method", eval.FailureCode);
        }

        [Fact]
        public void Evaluate_Rejects_VehicleRequired_WhenUnavailable()
        {
            var engine = CreateEngine();
            var barrier = MakeBarrier("obstacle_wreck_blockage");
            var inv = MakeInv(("item_expedition_winch_kit", 1));
            var blocked = engine.Evaluate(barrier, "breach_tool_winch_assist", vehicleAvailable: false, inventory: inv);
            Assert.False(blocked.CanBegin);
            Assert.Equal("vehicle_required", blocked.FailureCode);

            var ok = engine.Evaluate(barrier, "breach_tool_winch_assist", vehicleAvailable: true, inventory: inv);
            Assert.True(ok.CanBegin);
        }

        [Fact]
        public void QuietCut_ClearsWire_WithLowerNoiseThanLoudBreach()
        {
            var quiet = CreateEngine(seed: 101);
            var loud = CreateEngine(seed: 101);
            var wire = MakeBarrier("obstacle_concertina_wire", "wire");
            var door = MakeBarrier("obstacle_armored_door", "door");
            var invQuiet = MakeInv(("item_hydraulic_wire_cutter", 1));
            var invLoud = MakeInv(("item_linear_breach_section", 1));

            Assert.True(quiet.Begin(wire, "breach_tool_hydraulic_cutter", operatorSkill01: 0.9f, inventory: invQuiet).Success);
            Assert.True(loud.Begin(door, "breach_tool_linear_section", operatorSkill01: 0.9f, inventory: invLoud).Success);
            Assert.Equal(0, invLoud.CountById("item_linear_breach_section"));

            float quietNoise = 0f, loudNoise = 0f;
            for (int i = 0; i < 25; i++)
            {
                if (!string.Equals(wire.BreachPhase, BreachPhaseIds.Cleared, StringComparison.Ordinal))
                {
                    var t = quiet.Advance(wire, operatorSkill01: 0.95f, rng: new SeededRng(2000 + i));
                    quietNoise += t.NoiseEmitted;
                }
                if (!string.Equals(door.BreachPhase, BreachPhaseIds.Cleared, StringComparison.Ordinal))
                {
                    var t = loud.Advance(door, operatorSkill01: 0.95f, rng: new SeededRng(2000 + i));
                    loudNoise += t.NoiseEmitted;
                }
            }

            Assert.Equal(BreachPhaseIds.Cleared, wire.BreachPhase);
            Assert.Equal(BreachPhaseIds.Cleared, door.BreachPhase);
            Assert.True(wire.PathBlocking <= 0.01f);
            Assert.True(door.PathBlocking <= 0.01f);
            Assert.True(quietNoise < loudNoise, $"quiet={quietNoise:F2} loud={loudNoise:F2}");
        }

        [Fact]
        public void DamagedTool_TakesLongerOrMoreTicks()
        {
            var pristine = CreateEngine(11);
            var damaged = CreateEngine(11);
            var a = MakeBarrier("obstacle_concertina_wire", "a");
            var b = MakeBarrier("obstacle_concertina_wire", "b");
            var invA = MakeInv(("item_manual_bolt_shears", 1));
            var invB = MakeInv(("item_manual_bolt_shears", 1));

            var evalA = pristine.Evaluate(a, "breach_tool_manual_shears", equipmentCondition01: 1f, inventory: invA);
            var evalB = damaged.Evaluate(b, "breach_tool_manual_shears", equipmentCondition01: 0.2f, inventory: invB);
            Assert.True(evalA.CanBegin && evalB.CanBegin);
            Assert.True(evalB.ClearTicks >= evalA.ClearTicks);
            Assert.True(evalB.EffectiveClearance < evalA.EffectiveClearance);
        }

        [Fact]
        public void Abandon_PreservesPartialProgress_AndResumesWithoutReset()
        {
            var engine = CreateEngine(33);
            var barrier = MakeBarrier("obstacle_sandbag_redoubt");
            var inv = MakeInv(("item_mechanical_breach_ram", 1));
            Assert.True(engine.Begin(barrier, "breach_tool_mechanical_ram", inventory: inv).Success);
            // Finish setup if any, then one clear tick.
            for (int i = 0; i < 5 && string.Equals(barrier.BreachPhase, BreachPhaseIds.SettingUp, StringComparison.Ordinal); i++)
                engine.Advance(barrier, rng: new SeededRng(3300 + i));
            engine.Advance(barrier, rng: new SeededRng(3399));
            float progress = barrier.BreachProgress01;
            int remaining = barrier.BreachClearTicksRemaining;
            Assert.True(progress > 0f || remaining < barrier.BreachClearTicksTotal);

            var abandon = engine.Abandon(barrier);
            Assert.True(abandon.Success);
            Assert.Equal(BreachPhaseIds.Abandoned, barrier.BreachPhase);
            Assert.Equal(progress, barrier.BreachProgress01, 4);
            Assert.Equal("breach_tool_mechanical_ram", barrier.ActiveBreachToolId);

            var resume = engine.Begin(barrier, "breach_tool_mechanical_ram", inventory: inv);
            Assert.True(resume.Success);
            Assert.Equal(BreachPhaseIds.Clearing, barrier.BreachPhase);
            Assert.Equal(progress, barrier.BreachProgress01, 4);
            Assert.Equal(remaining, barrier.BreachClearTicksRemaining);
        }

        [Fact]
        public void Evaluate_FailsClosed_WhenInventoryUnbound()
        {
            var engine = CreateEngine();
            var barrier = MakeBarrier("obstacle_concertina_wire");
            var eval = engine.Evaluate(barrier, "breach_tool_hydraulic_cutter", inventory: null);
            Assert.False(eval.CanBegin);
            Assert.Equal("logistics_unbound", eval.FailureCode);
        }

        [Fact]
        public void Begin_Rejected_WhileInterrupted()
        {
            var engine = CreateEngine(44);
            var barrier = MakeBarrier("obstacle_sandbag_redoubt");
            var inv = MakeInv(("item_mechanical_breach_ram", 1));
            Assert.True(engine.Begin(barrier, "breach_tool_mechanical_ram", inventory: inv).Success);
            barrier.BreachPhase = BreachPhaseIds.Interrupted;
            barrier.ActiveBreachToolId = "breach_tool_mechanical_ram";
            barrier.BreachProgress01 = 0.4f;

            var eval = engine.Evaluate(barrier, "breach_tool_mechanical_ram", inventory: inv);
            Assert.False(eval.CanBegin);
            Assert.Equal("breach_in_progress", eval.FailureCode);

            var advance = engine.Advance(barrier, rng: new SeededRng(4401));
            Assert.True(advance.Success);
            Assert.Equal(BreachPhaseIds.Clearing, barrier.BreachPhase);
        }

        [Fact]
        public void CombatSystem_MidBreach_SaveRoundTrip_PreservesFields()
        {
            var catalog = LoadCatalog();
            var combat = new TacticalCombatSystem(null, CombatHostPorts.NoOp());
            combat.LoadBreachingCatalog(catalog);
            var players = new List<CombatantState>
            {
                new CombatantState
                {
                    Id = "p1", Name = "Cutter", SurvivorId = "survivor_cutter",
                    IsPlayer = true, Health = 100, MaxHealth = 100
                }
            };
            var weapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState
                {
                    InstanceId = "wp1", WeaponId = "weapon_pipe_rifle",
                    OwnerSurvivorId = "survivor_cutter", ConditionPct = 0.9f,
                    AmmoId = "ammo_357", AmmoRemaining = 12
                }
            };
            Assert.True(combat.BeginEncounter("enc_mid", "exp", "loc", "Loc", 1, 42, players, weapons, 1, 30f));
            combat.ConfigureBreachingLogistics(MakeInv(("item_hydraulic_wire_cutter", 1)), false);
            var barrier = combat.EnsureObstacleBarrier("bar_wire", "obstacle_concertina_wire");
            Assert.True(combat.BeginBreach(barrier.Id, "breach_tool_hydraulic_cutter").Success);
            combat.AdvanceBreach(barrier.Id, new SeededRng(4201));

            var saved = combat.CaptureState();
            var restored = new TacticalCombatSystem(null, CombatHostPorts.NoOp());
            restored.RestoreState(saved);
            var copy = restored.FindBarrier("bar_wire");
            Assert.NotNull(copy);
            Assert.Equal("obstacle_concertina_wire", copy!.ObstacleProfileId);
            Assert.Equal("breach_tool_hydraulic_cutter", copy.ActiveBreachToolId);
            Assert.Equal(barrier.BreachPhase, copy.BreachPhase);
            Assert.Equal(barrier.BreachClearTicksTotal, copy.BreachClearTicksTotal);
            Assert.Equal(barrier.BreachProgress01, copy.BreachProgress01, 4);
        }

        [Fact]
        public void Deterministic_Replay_SameSeed_SameProgress()
        {
            BreachingTickResult Run(int seed)
            {
                var engine = CreateEngine(seed);
                var barrier = MakeBarrier("obstacle_reinforced_fence");
                var inv = MakeInv(("item_hydraulic_wire_cutter", 1));
                engine.Begin(barrier, "breach_tool_hydraulic_cutter", operatorSkill01: 0.6f, inventory: inv);
                BreachingTickResult last = new BreachingTickResult();
                for (int i = 0; i < 8; i++)
                    last = engine.Advance(barrier, operatorSkill01: 0.6f, rng: new SeededRng(seed * 100 + i));
                last.Progress01 = barrier.BreachProgress01;
                last.Phase = barrier.BreachPhase;
                return last;
            }

            var a = Run(77);
            var b = Run(77);
            Assert.Equal(a.Phase, b.Phase);
            Assert.Equal(a.Progress01, b.Progress01, 5);
            Assert.Equal(a.OperatorIncident, b.OperatorIncident);
        }
    }
}
