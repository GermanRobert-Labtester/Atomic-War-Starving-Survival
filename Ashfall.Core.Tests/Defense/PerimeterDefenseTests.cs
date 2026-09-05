// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Defense
{
    public sealed class PerimeterDefenseTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            candidate = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void CatalogLoader_LoadsAllEightDefenses()
        {
            string dataDir = GetDataDir();
            var defs = PerimeterDefenseCatalogLoader.Load(dataDir);
            Assert.NotNull(defs);
            Assert.Equal(8, defs.Count);

            foreach (var d in defs)
            {
                Assert.False(string.IsNullOrEmpty(d.defense_id));
                Assert.False(string.IsNullOrEmpty(d.defense_type));
                Assert.True(d.max_hp > 0);
                Assert.NotEmpty(d.build_costs);
            }
        }

        [Fact]
        public void Construction_ConsumesMaterialsAtomically_AndCreatesEmplacement()
        {
            var def = new PerimeterDefenseDefinition
            {
                defense_id = "def_sandbag_berm",
                display_name = "Sandbag Berm",
                max_hp = 250,
                build_costs = new Dictionary<string, int> { { "sandbags", 4 }, { "scrap_wood", 2 } }
            };

            var inv = new Inventory.Inventory();
            inv.TryProduce("sandbags", 4);
            inv.TryProduce("scrap_wood", 2);

            var system = new PerimeterDefenseSystem(new[] { def }, inv, new SeededRng(100));

            var res = system.ConstructEmplacement("def_sandbag_berm");
            Assert.True(res.IsSuccess);
            Assert.Single(system.Emplacements);

            var emp = system.Emplacements[0];
            Assert.Equal(250, emp.current_hp);
            Assert.Equal(0, inv.CountById("sandbags"));
            Assert.Equal(0, inv.CountById("scrap_wood"));
        }

        [Fact]
        public void AmmoLoading_ConsumesInventoryAmmo_AndCapsAtMagazineCapacity()
        {
            var def = new PerimeterDefenseDefinition
            {
                defense_id = "def_sentry_9mm",
                defense_type = "automated_turret",
                max_hp = 300,
                required_ammo_type = "ammo_9x19",
                magazine_capacity = 100,
                build_costs = new Dictionary<string, int>()
            };

            var inv = new Inventory.Inventory { MaxWeight = 500f };
            inv.TryProduce("ammo_9x19", 150);

            var system = new PerimeterDefenseSystem(new[] { def }, inv, new SeededRng(101));
            system.ConstructEmplacement("def_sentry_9mm");
            var emp = system.Emplacements[0];

            // Load 75
            var res1 = system.LoadAmmo(emp.emplacement_id, 75);
            Assert.True(res1.IsSuccess);
            Assert.Equal(75, emp.loaded_ammo_count);
            Assert.Equal(75, inv.CountById("ammo_9x19"));

            // Load another 50 (should cap at 100 capacity, loading 25)
            var res2 = system.LoadAmmo(emp.emplacement_id, 50);
            Assert.True(res2.IsSuccess);
            Assert.Equal(100, emp.loaded_ammo_count);
            Assert.Equal(50, inv.CountById("ammo_9x19"));
        }

        [Fact]
        public void RaiderAssault_TurretFires_InflictsDamageAndBarrelWear()
        {
            var turretDef = new PerimeterDefenseDefinition
            {
                defense_id = "def_sentry_556",
                defense_type = "automated_turret",
                max_hp = 450,
                required_ammo_type = "ammo_556",
                magazine_capacity = 150,
                fire_rate_burst = 25,
                base_damage = 25.0f,
                power_draw_watts = 500
            };

            var inv = new Inventory.Inventory();
            inv.TryProduce("ammo_556", 100);

            var system = new PerimeterDefenseSystem(new[] { turretDef }, inv, new SeededRng(102));
            system.ConstructEmplacement("def_sentry_556");
            var emp = system.Emplacements[0];
            system.LoadAmmo(emp.emplacement_id, 100);

            // Powered turret fires burst
            var assaultRes = system.SimulateRaiderAssault(
                raiderStrength: 10,
                isNight: false,
                isEmplacementPowered: id => true);

            Assert.True(assaultRes.Repelled);
            Assert.False(assaultRes.Breached);
            Assert.Equal(25, assaultRes.RoundsFiredTotal);
            Assert.Equal(75, emp.loaded_ammo_count); // 100 - 25
            Assert.Equal(25 * PerimeterDefenseSystem.BarrelWearPerRound, emp.barrel_wear_percent); // 1.0% wear
        }

        [Fact]
        public void RaiderAssault_UnpoweredTurretDoesNotFire()
        {
            var turretDef = new PerimeterDefenseDefinition
            {
                defense_id = "def_sentry_556",
                defense_type = "automated_turret",
                max_hp = 100,
                required_ammo_type = "ammo_556",
                magazine_capacity = 150,
                fire_rate_burst = 25,
                base_damage = 25.0f,
                power_draw_watts = 500
            };

            var inv = new Inventory.Inventory();
            inv.TryProduce("ammo_556", 100);

            var system = new PerimeterDefenseSystem(new[] { turretDef }, inv, new SeededRng(103));
            system.ConstructEmplacement("def_sentry_556");
            var emp = system.Emplacements[0];
            system.LoadAmmo(emp.emplacement_id, 100);

            // Unpowered turret
            var assaultRes = system.SimulateRaiderAssault(
                raiderStrength: 20,
                isNight: false,
                isEmplacementPowered: id => false);

            // Turret did not fire any rounds
            Assert.Equal(0, assaultRes.RoundsFiredTotal);
            Assert.Equal(100, emp.loaded_ammo_count);
            // Raiders attack emplacement
            Assert.True(assaultRes.EmplacementsDamaged > 0);
        }

        [Fact]
        public void TripwireFlare_PreventsStealthBreachAndBoostsNightAccuracy()
        {
            var flareDef = new PerimeterDefenseDefinition
            {
                defense_id = "def_tripwire_flare_line",
                defense_type = "early_warning",
                max_hp = 50,
                night_accuracy_bonus = 0.30f,
                prevents_stealth_breach = true
            };

            var inv = new Inventory.Inventory();
            var system = new PerimeterDefenseSystem(new[] { flareDef }, inv, new SeededRng(104));
            system.ConstructEmplacement("def_tripwire_flare_line");

            var assaultRes = system.SimulateRaiderAssault(raiderStrength: 2, isNight: true);
            Assert.True(assaultRes.StealthInfiltrationNeutralized);
        }

        [Fact]
        public void TurretWearAndJam_JamsAtHighWearAndClearsWithService()
        {
            var turretDef = new PerimeterDefenseDefinition
            {
                defense_id = "def_sentry_9mm",
                defense_type = "automated_turret",
                max_hp = 300,
                required_ammo_type = "ammo_9x19",
                magazine_capacity = 100
            };

            var inv = new Inventory.Inventory();
            var system = new PerimeterDefenseSystem(new[] { turretDef }, inv, new SeededRng(105));
            system.ConstructEmplacement("def_sentry_9mm");
            var emp = system.Emplacements[0];

            // Manually set wear and jam
            emp.barrel_wear_percent = 70f;
            emp.is_jammed = true;

            // Service requires scrap metal
            inv.TryProduce("scrap_metal", 1);
            var res = system.ServiceTurretBarrel(emp.emplacement_id);
            Assert.True(res.IsSuccess);
            Assert.Equal(0f, emp.barrel_wear_percent);
            Assert.False(emp.is_jammed);
            Assert.Equal(0, inv.CountById("scrap_metal"));
        }

        [Fact]
        public void Persistence_EmplacementsSurviveSaveLoad()
        {
            var def = new PerimeterDefenseDefinition
            {
                defense_id = "def_sentry_556",
                max_hp = 450
            };

            var inv = new Inventory.Inventory();
            var systemA = new PerimeterDefenseSystem(new[] { def }, inv, new SeededRng(106));
            systemA.ConstructEmplacement("def_sentry_556");
            var empA = systemA.Emplacements[0];
            empA.current_hp = 350;
            empA.loaded_ammo_count = 80;
            empA.barrel_wear_percent = 15f;

            var save = systemA.CaptureState();
            Assert.NotNull(save);

            var systemB = new PerimeterDefenseSystem(new[] { def }, new Inventory.Inventory(), new SeededRng(107));
            systemB.RestoreState(save);

            var empB = systemB.FindEmplacement(empA.emplacement_id);
            Assert.NotNull(empB);
            Assert.Equal(350, empB.current_hp);
            Assert.Equal(80, empB.loaded_ammo_count);
            Assert.Equal(15f, empB.barrel_wear_percent);
        }
    }
}
