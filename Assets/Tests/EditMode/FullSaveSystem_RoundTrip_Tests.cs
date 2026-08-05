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
    public class FullSaveSystem_RoundTrip_Tests
    {
        private string _testDir;

        [SetUp]
        public void SetUp()
        {
            _testDir = Path.Combine(Path.GetTempPath(), "ashfall_full_save_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_testDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_testDir))
            {
                try { Directory.Delete(_testDir, true); } catch { /* best-effort cleanup */ }
            }
        }

        [Test]
        public void FullSaveSystem_RoundTrip_PreservesChecksumAndVersion()
        {
            var saveSys = MakeMinimalSaveSystem();
            Assert.IsTrue(saveSys.Save("test_slot"));

            string path = Path.Combine(_testDir, "save_test_slot.json");
            Assert.IsTrue(File.Exists(path), "Save file must be written to the configured directory.");

            string json = File.ReadAllText(path);
            Assert.IsTrue(json.Contains("\"SaveVersion\""), "JSON must include save version");
            Assert.IsTrue(json.Contains("\"Checksum\""), "JSON must include checksum");
            Assert.IsFalse(json.Contains("\"Checksum\": \"\""), "Checksum must not be empty after write.");
        }

        [Test]
        public void FullSaveSystem_TamperedFile_RejectsLoad()
        {
            var saveSys = MakeMinimalSaveSystem();
            saveSys.SetWorldFlag("test_flag", true);
            saveSys.SetWorldFlag("another_flag", true);
            Assert.IsTrue(saveSys.Save("test_slot"));

            // Tamper: flip one byte in the WorldFlagValues array. The byte
            // we flip is the boolean `true` value (4 bytes) — flipping the
            // low bit changes `1` to `0` in the lower byte without breaking
            // the JSON structure. JsonUtility's strict parsing rejects the
            // resulting non-`true`/`false` token, and even if it parses,
            // the rebuilt body no longer matches the saved checksum.
            string path = Path.Combine(_testDir, "save_test_slot.json");
            byte[] bytes = File.ReadAllBytes(path);
            // Find the first "true" token in the file and replace it with
            // "fals" (start of false). This breaks the JSON, so the loader
            // logs an error AND returns false. We use LogAssert.Expect so
            // the test framework does not treat the expected error log as
            // an unhandled error.
            for (int i = 0; i < bytes.Length - 3; i++)
            {
                if (bytes[i] == 't' && bytes[i+1] == 'r' && bytes[i+2] == 'u' && bytes[i+3] == 'e')
                {
                    bytes[i] = (byte)'f';
                    bytes[i+1] = (byte)'a';
                    bytes[i+2] = (byte)'l';
                    bytes[i+3] = (byte)'s';
                    break;
                }
            }
            File.WriteAllBytes(path, bytes);

            // The loader logs an error. The test framework treats any
            // Debug.LogError during a test as a failure unless the test
            // declares it as expected.
            UnityEngine.TestTools.LogAssert.Expect(LogType.Error,
                new System.Text.RegularExpressions.Regex(@"\[SaveSystem\] (Slot '.*' (is corrupt|corrupt and no backup available)|Load from '.*' failed).*"));

            var loadSys = MakeMinimalSaveSystem();
            Assert.IsFalse(loadSys.Load("test_slot"), "Tampered save must be rejected (by checksum mismatch or parse error).");
        }

        [Test]
        public void FullSaveSystem_LoadMissingSlot_ReturnsFalse()
        {
            var saveSys = MakeMinimalSaveSystem();
            Assert.IsFalse(saveSys.Load("nonexistent"));
        }

        [Test]
        public void FullSaveSystem_Delete_RemovesFile()
        {
            var saveSys = MakeMinimalSaveSystem();
            saveSys.Save("test_slot");
            Assert.IsTrue(saveSys.SlotExists("test_slot"));
            Assert.IsTrue(saveSys.Delete("test_slot"));
            Assert.IsFalse(saveSys.SlotExists("test_slot"));
        }

        [Test]
        public void FullSaveSystem_OverwriteSecondSave_ReplacesFirst()
        {
            // Sanity: SetWorldFlag mutates instance state; subsequent Save()
            // captures the current dictionary (both flags). To prove the
            // "overwrite replaces" contract, we use two SEPARATE SaveSystem
            // instances (each with their own _worldFlags).
            var saveSysA = MakeMinimalSaveSystem();
            saveSysA.SetWorldFlag("v1", true);
            Assert.IsTrue(saveSysA.Save("test_slot"));

            var saveSysB = MakeMinimalSaveSystem();
            saveSysB.SetWorldFlag("v2", true);
            Assert.IsTrue(saveSysB.Save("test_slot"));

            // Loading now returns saveSysB's state — only v2, not v1.
            var loadSys = MakeMinimalSaveSystem();
            Assert.IsTrue(loadSys.Load("test_slot"));
            Assert.IsTrue(loadSys.GetWorldFlag("v2"));
            Assert.IsFalse(loadSys.GetWorldFlag("v1"));
        }

        // -------------------------------------------------------------
        // Helper
        // -------------------------------------------------------------

        private SaveSystem MakeMinimalSaveSystem()
        {
            var gs = new GameState();
            var ws = new WeatherSystem(null, 42);
            var ts = new TemperatureSystem(null, ws);
            // NeedsSystem requires a non-null NeedsProfile ScriptableObject.
            // The fields default to safe decay values from the SO's class
            // defaults, so an empty SO is sufficient for the round-trip tests.
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var ns = new NeedsSystem(profile, sv => true);
            var rs = new RadiationSystem(ns);
            var shelter = new ShelterClass();
            return new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = gs,
                WeatherSystem = ws,
                TemperatureSystem = ts,
                NeedsSystem = ns,
                RadiationSystem = rs,
                Shelter = shelter,
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = _testDir
            });
        }
    }
}
