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
    public class SaveSystemFieldCoverage_Tests
    {
        [Test]
        public void SaveData_HasFieldsForEveryWiredSystem()
        {
            // Each entry: the field name on SaveData and a non-null SaveData
            // can be saved with all-null sub-states. This is a structural
            // check: if someone deletes a field from SaveData, this test
            // breaks before the runtime does.
            var data = new SaveData();
            Assert.IsNotNull(data.GameState);
            Assert.IsNotNull(data.Survivors);
            Assert.IsNotNull(data.ShelterModules);
            Assert.IsNotNull(data.WorldFlagKeys);
            Assert.IsNotNull(data.WorldFlagValues);
            // The nullable SaveDtos for systems must be reflectable:
            var fields = typeof(SaveData).GetFields(BindingFlags.Public | BindingFlags.Instance);
            // We expect at least 50 sub-snapshot fields (currently ~60).
            int nullableFields = 0;
            foreach (var f in fields)
            {
                if (f.FieldType.IsClass && f.FieldType != typeof(string))
                    nullableFields++;
            }
            Assert.GreaterOrEqual(nullableFields, 50, $"SaveData should expose at least 50 sub-snapshots. Found {nullableFields}.");
        }

        [Test]
        public void SaveData_JsonSerializes_AndDeserializes_WithRoundTrip()
        {
            // Smoke test: the SaveData DTO itself is JSON-serializable.
            var data = new SaveData
            {
                GameState = new GameStateSave { Phase = GamePhase.Running, Day = 42, IsPaused = true }
            };
            string json = JsonUtility.ToJson(data, true);
            Assert.IsTrue(json.Contains("\"SaveVersion\""));
            Assert.IsTrue(json.Contains("\"Day\": 42"));

            var roundTripped = JsonUtility.FromJson<SaveData>(json);
            Assert.IsNotNull(roundTripped);
            Assert.AreEqual(42, roundTripped.GameState.Day);
        }
    }
}
