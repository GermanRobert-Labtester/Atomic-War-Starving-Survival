using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #859 — System_LegacyStart: prepare from Last Will, activate run,
    /// excavation / ghosts, save round-trip.
    /// </summary>
    [TestFixture]
    public class LegacyStartTests
    {
        private const float Eps = 1e-3f;

        private static GraveSiteData MakeGrave()
        {
            return new GraveSiteData
            {
                locationId = "grave_player_bunker",
                originalSeed = 42,
                dayOfDeath = 19,
                causeOfDeath = "radiation",
                deadSurvivorNames = new List<string> { "Elena", "Marcus" },
                diaryEntries = new List<string> { "Filters failed." },
                remainingLootIds = new List<string> { "iodine", "canned_beans" }
            };
        }

        [Test]
        public void PrepareFromGrave_SeedsRoomsCorpsesAndLoot()
        {
            var sys = new System_LegacyStart();
            var rooms = new List<string> { "quarters", "stores", "entry" };
            sys.PrepareFromGrave(MakeGrave(), rooms, "slot_prior");

            Assert.IsTrue(sys.IsPrepared);
            Assert.IsTrue(sys.CheckAvailability());
            Assert.IsFalse(sys.IsLegacyRunActive);
            Assert.AreEqual("slot_prior", sys.PreviousSaveId);
            Assert.AreEqual(19, sys.PriorDayOfDeath);
            Assert.AreEqual(3, sys.GetRuinedRooms().Count);
            Assert.AreEqual(2, sys.GetCorpseLocations().Count);
            Assert.AreEqual("quarters", sys.GetCorpseLocations()[0].room_id);
            Assert.AreEqual("stores", sys.GetCorpseLocations()[1].room_id);
            Assert.AreEqual(2, sys.GetRemainingLootIds().Count);
        }

        [Test]
        public void PrepareFromLastWill_RequiresGrave()
        {
            var legacy = new System_LegacyStart();
            var will = new LastWillSystem();
            Assert.IsFalse(legacy.PrepareFromLastWill(will, new List<string> { "quarters" }));

            will.GenerateGraveSite(
                "grave_x", 1, 5,
                new List<string> { "Ash" },
                new List<string> { "note" },
                new List<string> { "fuel" },
                "hunger");
            Assert.IsTrue(legacy.PrepareFromLastWill(will, new List<string> { "quarters", "plant" }));
            Assert.AreEqual(1, legacy.GetCorpseLocations().Count);
            Assert.AreEqual("fuel", legacy.GetRemainingLootIds()[0]);
        }

        [Test]
        public void BeginLegacyRun_FiresEvents_AndGrantsLootList()
        {
            var sys = new System_LegacyStart();
            sys.PrepareFromGrave(MakeGrave(), new List<string> { "quarters", "entry" });

            string loadedId = null;
            var ruined = new List<string>();
            var corpses = new List<string>();
            IReadOnlyList<string> lootEvent = null;
            sys.OnLegacyLoaded += id => loadedId = id;
            sys.OnRuinedRoomDiscovered += r => ruined.Add(r);
            sys.OnCorpseFound += (sid, rid) => corpses.Add(sid + "@" + rid);
            sys.OnLegacyLootGranted += loot => lootEvent = loot;

            var loot = sys.BeginLegacyRun();

            Assert.IsTrue(sys.IsLegacyRunActive);
            Assert.IsTrue(sys.IsBunkerRuined);
            Assert.AreEqual("grave_player_bunker", loadedId);
            Assert.AreEqual(2, ruined.Count);
            Assert.AreEqual(2, corpses.Count);
            Assert.AreEqual(2, loot.Count);
            Assert.AreEqual(2, lootEvent.Count);
            // Second call is idempotent.
            var loot2 = sys.BeginLegacyRun();
            Assert.AreEqual(2, loot2.Count);
            Assert.AreEqual(2, ruined.Count, "Should not re-fire ruined events on second begin.");
        }

        [Test]
        public void ExcavateRoom_GhostsAndProgress()
        {
            var sys = new System_LegacyStart();
            sys.PrepareFromGrave(MakeGrave(), new List<string> { "quarters", "stores" });
            sys.BeginLegacyRun();

            var ghosts = new List<string>();
            var excavated = new List<string>();
            sys.OnGhostEncountered += id => ghosts.Add(id);
            sys.OnRoomExcavated += r => excavated.Add(r);

            sys.ExcavateRoom("quarters");
            Assert.AreEqual(1, ghosts.Count);
            Assert.AreEqual("Elena", ghosts[0]);
            Assert.AreEqual(0.5f, sys.GetExcavationProgress(), Eps);
            Assert.IsTrue(sys.IsRoomExcavated("quarters"));

            sys.ExcavateRoom("quarters"); // idempotent
            Assert.AreEqual(1, excavated.Count);

            sys.ExcavateRoom("stores");
            Assert.AreEqual(2, ghosts.Count);
            Assert.AreEqual(1f, sys.GetExcavationProgress(), Eps);
        }

        [Test]
        public void CaptureRestore_RoundTrip_PreservesExcavation()
        {
            var a = new System_LegacyStart();
            a.PrepareFromGrave(MakeGrave(), new List<string> { "quarters", "stores", "entry" });
            a.BeginLegacyRun();
            a.ExcavateRoom("quarters");

            var save = a.CaptureState();
            Assert.AreEqual("system_legacy_start", save.system_id);
            Assert.IsTrue(save.legacy_run_active);
            Assert.AreEqual(1, save.excavated_rooms.Count);

            var b = new System_LegacyStart();
            b.RestoreState(save);

            Assert.IsTrue(b.IsLegacyRunActive);
            Assert.IsTrue(b.IsBunkerRuined);
            Assert.IsTrue(b.IsRoomExcavated("quarters"));
            Assert.IsFalse(b.IsRoomExcavated("stores"));
            Assert.AreEqual(2, b.GetCorpseLocations().Count);
            Assert.AreEqual(19, b.PriorDayOfDeath);
            Assert.AreEqual(a.GetExcavationProgress(), b.GetExcavationProgress(), Eps);

            // Mutating restore copy must not affect captured snapshot lists.
            b.ExcavateRoom("stores");
            Assert.AreEqual(1, save.excavated_rooms.Count);
        }

        [Test]
        public void RestoreState_Null_Resets()
        {
            var sys = new System_LegacyStart();
            sys.PrepareFromGrave(MakeGrave(), new List<string> { "quarters" });
            sys.BeginLegacyRun();
            sys.RestoreState(null);
            Assert.IsFalse(sys.IsPrepared);
            Assert.IsFalse(sys.IsLegacyRunActive);
            Assert.AreEqual(0, sys.GetRuinedRooms().Count);
        }

        [Test]
        public void ForceFlood_AppliesRuinedRooms()
        {
            var flood = new RoomFloodingSystem();
            flood.ForceFlood("quarters");
            flood.ForceFlood("quarters"); // idempotent
            flood.ForceFlood("stores");
            Assert.IsTrue(flood.IsFlooded("quarters"));
            Assert.IsTrue(flood.IsFlooded("stores"));
            Assert.AreEqual(2, flood.FloodedRooms.Count);
        }

        [Test]
        public void SaveSystemAdapter_RoundTrip_LegacyStart()
        {
            string dir = SaveSystemTestFactory.TempDir("legacy");
            try
            {
                var willA = new LastWillSystem();
                willA.GenerateGraveSite(
                    "grave_player_bunker", 7, 11,
                    new List<string> { "Mara" },
                    new List<string> { "day eleven" },
                    new List<string> { "bandage" },
                    "starved");

                var legacyA = new System_LegacyStart();
                legacyA.PrepareFromLastWill(willA, new List<string> { "quarters", "entry" });
                legacyA.BeginLegacyRun();
                legacyA.ExcavateRoom("entry");

                SaveSystem Make(LastWillSystem will, System_LegacyStart legacy) =>
                    SaveSystemTestFactory.MakeSave(dir, ss => { ss.SetLastWillSystem(will); ss.SetLegacyStartSystem(legacy); });

                Assert.IsTrue(Make(willA, legacyA).Save("legacy_slot"));

                var willB = new LastWillSystem();
                var legacyB = new System_LegacyStart();
                Assert.IsTrue(Make(willB, legacyB).Load("legacy_slot"));

                Assert.IsTrue(willB.HasGraveSite);
                Assert.IsTrue(legacyB.IsLegacyRunActive);
                Assert.IsTrue(legacyB.IsRoomExcavated("entry"));
                Assert.AreEqual(1, legacyB.GetCorpseLocations().Count);
                Assert.AreEqual("Mara", legacyB.GetCorpseLocations()[0].survivor_id);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
