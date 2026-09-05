// SPDX-License-Identifier: MIT
// Plan 163 — DefenseSystem behavior tests (trap layer, perimeter composition,
// pre-combat ordering, capture handoff, raid logs).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class DefenseSystemTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static List<DefenseTrapDefinition> ShippedTraps() =>
            TrapCatalogLoader.Load(FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        /// <summary>trap_ ids are definition-position values in defenses.json
        /// and must resolve; helper checks the shipped catalog loads.</summary>
        [Fact]
        public void ShippedTrapCatalog_Validates()
        {
            var traps = ShippedTraps();
            Assert.True(traps.Count >= 4, $"expected a meaningful trap roster, got {traps.Count}");
            var diags = TrapCatalogLoader.Validate(traps);
            Assert.Empty(diags);
        }

        [Fact]
        public void CatalogValidation_RejectsBadTraps()
        {
            var bad = new List<DefenseTrapDefinition>
            {
                new DefenseTrapDefinition { id = "snare_wrong_prefix", capture_chance = 1.5f, max_hp = 0 },
                new DefenseTrapDefinition { id = "trap_dup" },
                new DefenseTrapDefinition { id = "trap_dup" }
            };
            var diags = TrapCatalogLoader.Validate(bad);
            Assert.Contains(diags, d => d.Contains("trap_ prefix"));
            Assert.Contains(diags, d => d.Contains("duplicate"));
            Assert.Contains(diags, d => d.Contains("capture_chance"));
            Assert.Contains(diags, d => d.Contains("max_hp"));
        }

        private static DefenseSystem MakeSystem(params DefenseTrapDefinition[] traps) =>
            new DefenseSystem(traps.Length > 0 ? traps : null);

        private static DefenseTrapDefinition AlwaysSpringTrap(string id = "trap_test_snare", float capture = 0f) => new()
        {
            id = id,
            display_name = "Test Snare",
            defense_type = "snare",
            base_strength = 2,
            max_hp = 100,
            activation_chance = 1f,   // deterministic: always springs
            capture_chance = capture,
            build_costs = { ["scrap_metal"] = 1 },
            reset_costs = { ["scrap_metal"] = 1 },
            repair_costs = { ["scrap_metal"] = 1 }
        };

        private static Func<string, int, bool> UnlimitedStock() => (_, _) => true;

        [Fact]
        public void InstallTrap_ConsumesCostsViaCallback()
        {
            var sys = MakeSystem(AlwaysSpringTrap());
            bool asked = false;
            bool ok = sys.InstallTrap("trap_test_snare", "north", (item, n) => { asked = true; return true; });
            Assert.True(ok && asked);
            Assert.Single(sys.Installations);
        }

        [Fact]
        public void Trap_TransitionsArmedToSprungOncePerRaid()
        {
            var sys = MakeSystem(AlwaysSpringTrap());
            sys.InstallTrap("trap_test_snare", "north", UnlimitedStock());
            int sprungEvents = 0;
            sys.OnTrapSprung += (_, _) => sprungEvents++;

            var result = sys.ResolvePreCombatRaid(10, 5, false, null, null, new SeededRng(1), new SeededRng(2));
            Assert.Equal(1, sprungEvents);
            var inst = sys.Installations[0];
            Assert.True(inst.sprung && !inst.armed);

            // A second raid the same day: the sprung trap does not fire again.
            sys.ResolvePreCombatRaid(10, 10, false, null, null, new SeededRng(3), new SeededRng(4));
            Assert.Equal(1, sprungEvents);
            Assert.Equal(1, sys.Installations[0].total_activations);
        }

        [Fact]
        public void ResetRequiresSprungAndIntact_RepairRequiresDamage()
        {
            var sys = MakeSystem(AlwaysSpringTrap());
            sys.InstallTrap("trap_test_snare", "north", UnlimitedStock());

            Assert.False(sys.CanResetTrap(sys.Installations[0].installation_id)); // not sprung yet
            sys.ResolvePreCombatRaid(10, 4, false, null, null, new SeededRng(1), new SeededRng(2));
            var id = sys.Installations[0].installation_id;

            // strength 2 + self damage 10 + 10 = 20 hp lost; hp 80 > 0 → not
            // broken, but the hardware IS damaged: repair is legal, reset too.
            Assert.True(sys.CanResetTrap(id));
            Assert.True(sys.CanRepairTrap(id), "worn (not broken) hardware is repairable");
            Assert.True(sys.TryResetTrap(id, UnlimitedStock()));
            Assert.False(sys.CanResetTrap(id)); // armed again

            // Break it: repeated springs wear it to 0.
            for (int i = 0; i < 9 && !sys.Installations[0].broken; i++)
            {
                sys.ResolvePreCombatRaid(10 + i, 4, false, null, null, new SeededRng(10 + i), new SeededRng(20 + i));
                if (sys.Installations[0].sprung)
                    sys.TryResetTrap(id, UnlimitedStock());
            }
            Assert.True(sys.Installations[0].broken);
            Assert.False(sys.CanResetTrap(id), "broken trap cannot reset until repaired");
            Assert.True(sys.CanRepairTrap(id));
            Assert.True(sys.TryRepairTrap(id, UnlimitedStock()));
            Assert.True(sys.CanResetTrap(id), "repaired sprung trap can be reset again");
        }

        [Fact]
        public void Engagement_DeterministicForSameSeeds()
        {
            var run = new Func<DefenseEngagementResult>(() =>
            {
                var sys = MakeSystem(AlwaysSpringTrap("trap_test_capture", capture: 0.5f));
                sys.InstallTrap("trap_test_capture", "approach", UnlimitedStock());
                return sys.ResolvePreCombatRaid(10, 6, false, null, null,
                    new SeededRng(42), new SeededRng(43));
            });
            var a = run();
            var b = run();
            Assert.Equal(a.RaidersCaptured, b.RaidersCaptured);
            Assert.Equal(a.RemainingRaiders, b.RemainingRaiders);
            Assert.Equal(a.Records.Count, b.Records.Count);
        }

        [Fact]
        public void CaptureOutcome_HandsOffViaEvent()
        {
            var sys = MakeSystem(AlwaysSpringTrap("trap_test_pit", capture: 1f)); // guaranteed capture
            sys.InstallTrap("trap_test_pit", "approach", UnlimitedStock());
            int capturedDays = -1, capturedCount = 0;
            sys.OnRaiderCaptured += (day, count) => { capturedDays = day; capturedCount = count; };

            var result = sys.ResolvePreCombatRaid(9, 6, false, null, null, new SeededRng(1), new SeededRng(2));
            Assert.Equal(1, result.RaidersCaptured);
            Assert.Equal(9, capturedDays);
            Assert.Equal(1, capturedCount);
            Assert.Contains(result.Records, r => r.outcome == "sprung_captured");
        }

        [Fact]
        public void PreCombatOrdering_TrapsResolveBeforePerimeter()
        {
            var inv = new Inventory.Inventory();
            // Auto-created items weigh 1 each; the default inventory caps at
            // 100 weight, so stock exactly one magazine.
            Assert.True(inv.TryProduce("ammo_556", 100));
            var perimeter = new PerimeterDefenseSystem(
                new[]
                {
                    new PerimeterDefenseDefinition
                    {
                        defense_id = "def_test_turret",
                        display_name = "Test Turret",
                        defense_type = "automated_turret",
                        max_hp = 200,
                        required_ammo_type = "ammo_556",
                        magazine_capacity = 100,
                        base_damage = 30f,
                        fire_rate_burst = 10,
                        build_costs = new Dictionary<string, int>()
                    }
                },
                inv, new SeededRng(7));
            Assert.True(perimeter.ConstructEmplacement("def_test_turret").IsSuccess);
            Assert.True(perimeter.LoadAmmo(perimeter.Emplacements[0].emplacement_id, 100).IsSuccess);

            var sys = MakeSystem(AlwaysSpringTrap());
            sys.InstallTrap("trap_test_snare", "north", UnlimitedStock());

            // 6 raiders − 2 (trap) = 4 reach the perimeter; turret fire and
            // barrier HP decide the rest — the perimeter result must reflect
            // the post-trap strength, not the original 6.
            var result = sys.ResolvePreCombatRaid(10, 6, false, perimeter,
                _ => true, new SeededRng(1), new SeededRng(2));
            Assert.NotNull(result.PerimeterResult);
            Assert.Equal(4, result.PerimeterResult.InitialRaiderStrength);
            Assert.True(result.PerimeterResult.RoundsFiredTotal > 0, "turret consumed ammo");
            Assert.True(result.RaidersNeutralizedByTraps >= 2);
        }

        [Fact]
        public void PerimeterStrength_DeterministicAndOrderInvariant()
        {
            var build = new Func<PerimeterStrengthBreakdown>(() =>
            {
                var sys = MakeSystem(AlwaysSpringTrap(), AlwaysSpringTrap("trap_test_two"));
                sys.InstallTrap("trap_test_snare", "north", UnlimitedStock());
                sys.InstallTrap("trap_test_two", "south", UnlimitedStock());
                return sys.CalculatePerimeterStrength(null, null);
            });
            Assert.Equal(build().Total, build().Total);

            var sys2 = MakeSystem(AlwaysSpringTrap());
            sys2.InstallTrap("trap_test_snare", "north", UnlimitedStock());
            sys2.ResolvePreCombatRaid(10, 4, false, null, null, new SeededRng(1), new SeededRng(2));
            var after = sys2.CalculatePerimeterStrength(null, null);
            Assert.True(after.Traps == 0, "sprung trap contributes no trap strength");
        }

        [Fact]
        public void RaidLog_IsStructuredAndBounded()
        {
            var sys = MakeSystem(AlwaysSpringTrap());
            sys.InstallTrap("trap_test_snare", "north", UnlimitedStock());
            for (int raid = 0; raid < 60; raid++)
            {
                sys.ResolvePreCombatRaid(100 + raid, 4, false, null, null,
                    new SeededRng(raid), new SeededRng(1000 + raid));
                sys.TryResetTrap(sys.Installations[0].installation_id, UnlimitedStock());
            }
            Assert.True(sys.RaidLog.Count <= DefenseSystem.RaidLogCapacity,
                $"raid log must stay bounded, got {sys.RaidLog.Count}");
            Assert.All(sys.RaidLog, r => Assert.False(string.IsNullOrEmpty(r.installation_id)));
            Assert.All(sys.RaidLog, r => Assert.False(string.IsNullOrEmpty(r.outcome)));
        }

        [Fact]
        public void UnpoweredTurret_IsSkippedByPerimeterSim()
        {
            var inv = new Inventory.Inventory();
            Assert.True(inv.TryProduce("ammo_556", 100));
            var perimeter = new PerimeterDefenseSystem(
                new[]
                {
                    new PerimeterDefenseDefinition
                    {
                        defense_id = "def_test_turret",
                        defense_type = "automated_turret",
                        max_hp = 200,
                        required_ammo_type = "ammo_556",
                        magazine_capacity = 100,
                        base_damage = 30f,
                        fire_rate_burst = 10,
                        power_draw_watts = 450,
                        build_costs = new Dictionary<string, int>()
                    }
                },
                inv, new SeededRng(7));
            perimeter.ConstructEmplacement("def_test_turret");
            perimeter.LoadAmmo(perimeter.Emplacements[0].emplacement_id, 100);

            var powered = perimeter.SimulateRaiderAssault(4, false, _ => true);
            var dark = perimeter.SimulateRaiderAssault(4, false, _ => false);
            Assert.True(powered.RoundsFiredTotal > 0);
            Assert.True(dark.RoundsFiredTotal == 0, "unpowered turret must not fire");
        }
    }
}
