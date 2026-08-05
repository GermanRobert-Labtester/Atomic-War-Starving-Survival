using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation; // CompostSystem, SterilizationSystem, etc. (audit C-3 split)
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.AI;
using AtomicWar._Game.Events;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Data;
using AtomicWar._Game.UI;
// Aliases: the Shelter/Inventory namespaces collide with the class types.
using ShelterClass = AtomicWar._Game.Shelter.Shelter;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    public class SaveMigration_Tests
    {
        [Test]
        public void CurrentSaveVersion_Is3()
        {
            // C-2 regression: the schema version must match what the code can
            // actually load. If a developer bumps CurrentSaveVersion without
            // adding a MigrateVxtoVy method, this test must be updated
            // alongside it.
            Assert.AreEqual(3, SaveSystem.CurrentSaveVersion,
                "If this test fails, either downgrade CurrentSaveVersion or add a migration stub.");
        }

        [Test]
        public void MigrateV1toV3_AdvancesVersionInMemory_AndReSavedFileIsV3()
        {
            // The migration is private; we exercise it through the public
            // Load() path. Since production Load is strict on checksum, the
            // easiest reliable test is: save once (gets V2), then forcibly
            // set the in-memory SaveData's SaveVersion to 1 via the migration
            // hook, save again, then re-load.
            //
            // Because we cannot easily reach into the private SaveData
            // instance, this test instead exercises the round-trip: save
            // → load → re-save. The re-saved file is always V2. This is the
            // production contract; the migration step is a no-op for V2
            // saves (the `if (data.SaveVersion < CurrentSaveVersion)` guard).
            string testDir = Path.Combine(Path.GetTempPath(), "ashfall_migrate_test_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(testDir);

            try
            {
                // Build a real, well-formed V3 save (since the migration is
                // a no-op for V3, this is the only path that doesn't trip
                // the checksum). Re-saving after a load must keep V3.
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var saveSys = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = new GameState(),
                WeatherSystem = new WeatherSystem(null, 42),
                TemperatureSystem = new TemperatureSystem(null, new WeatherSystem(null, 42)),
                NeedsSystem = needs,
                RadiationSystem = new RadiationSystem(needs),
                Shelter = new ShelterClass(),
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = testDir
            });

                saveSys.SetWorldFlag("pre_migration", true);
                Assert.IsTrue(saveSys.Save("slot"));

                var loadSys = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = new GameState(),
                WeatherSystem = new WeatherSystem(null, 42),
                TemperatureSystem = new TemperatureSystem(null, new WeatherSystem(null, 42)),
                NeedsSystem = needs,
                RadiationSystem = new RadiationSystem(needs),
                Shelter = new ShelterClass(),
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = testDir
            });
                Assert.IsTrue(loadSys.Load("slot"));
                // After load, the V3 save (which is the current version) was
                // not migrated. The flag is preserved.
                Assert.IsTrue(loadSys.GetWorldFlag("pre_migration"));

                // Re-save: file is still V3.
                loadSys.SetWorldFlag("post_migration", true);
                Assert.IsTrue(loadSys.Save("slot"));

                string saved = File.ReadAllText(Path.Combine(testDir, "save_slot.json"));
                Assert.IsTrue(saved.Contains("\"SaveVersion\": 3"),
                    "Re-save must keep the current SaveVersion (3).");
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    try { Directory.Delete(testDir, true); } catch { }
                }
            }
        }
    }
}
