using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Field-only SaveSystem injects promoted to RegisterSystem when they have
    /// real CaptureState/RestoreState (dual-path with CapIf/RestIf preserved).
    /// Complex special-path systems (Expedition, EventRunner, GeneratedMap,
    /// ShiftingHotspot, FactionRaidPlan) stay field-only.
    /// </summary>
    [TestFixture]
    public class FieldOnlyPromoteWiringTests
    {
        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_promote_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static SaveSystem MakeSave(string dir, Action<SaveSystem> wire)
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var weather = new WeatherSystem(null, 3);
            var temp = new TemperatureSystem(null, weather);
            var rad = new RadiationSystem(needs);
            var ss = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = new GameState(),
                WeatherSystem = weather,
                TemperatureSystem = temp,
                NeedsSystem = needs,
                RadiationSystem = rad,
                Shelter = new ShelterClass(),
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = dir
            });
            wire(ss);
            return ss;
        }

        private static bool HasSaveable(SaveSystem ss, string saveId)
        {
            var field = typeof(SaveSystem).GetField("_saveables", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field);
            var list = field.GetValue(ss) as System.Collections.IList;
            Assert.IsNotNull(list);
            foreach (var item in list)
            {
                var prop = item.GetType().GetProperty("SaveId");
                if (prop != null && string.Equals(prop.GetValue(item) as string, saveId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        [Test]
        public void Cartography_Tracker_DeadDrop_Register()
        {
            var carto = new CartographySystem();
            var tracker = new TrackerSystem();
            var drop = new DeadDropSystem();
            Assert.IsNotNull(carto.CaptureState());
            Assert.IsNotNull(tracker.CaptureState());
            Assert.IsNotNull(drop.CaptureState());

            string dir = TempDir("mapside");
            try
            {
                var ss = MakeSave(dir, s =>
                {
                    s.SetCartographySystem(carto);
                    s.SetTrackerSystem(tracker);
                    s.SetDeadDropSystem(drop);
                });
                Assert.IsTrue(HasSaveable(ss, "cartography"));
                Assert.IsTrue(HasSaveable(ss, "tracker"));
                Assert.IsTrue(HasSaveable(ss, "dead_drops"));
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void Medical_Family_Register()
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var medical = new MedicalSystem(needs);
            var blood = new BloodTransfusionSystem();
            var amp = new AmputationSystem();
            var scurvy = new ScurvySystem();
            var mut = new RadiationMutagenesisSystem();
            Assert.IsNotNull(medical.CaptureState());
            Assert.IsNotNull(blood.CaptureState());
            Assert.IsNotNull(amp.CaptureState());
            Assert.IsNotNull(scurvy.CaptureState());
            Assert.IsNotNull(mut.CaptureState());

            string dir = TempDir("med");
            try
            {
                var ss = MakeSave(dir, s =>
                {
                    s.SetMedicalSystem(medical);
                    s.SetBloodTransfusionSystem(blood);
                    s.SetAmputationSystem(amp);
                    s.SetScurvySystem(scurvy);
                    s.SetMutagenesisSystem(mut);
                });
                Assert.IsTrue(HasSaveable(ss, "medical"));
                Assert.IsTrue(HasSaveable(ss, "blood_transfusion"));
                Assert.IsTrue(HasSaveable(ss, "amputation"));
                Assert.IsTrue(HasSaveable(ss, "scurvy"));
                Assert.IsTrue(HasSaveable(ss, "mutagenesis"));
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void Shelter_Tactical_Register()
        {
            var integrity = new StructuralIntegritySystem();
            var waste = new WasteSystem();
            var vermin = new VerminSystem();
            var jury = new JuryRigSystem();
            var freeze = new FreezePipeSystem();
            var water = new WaterStorage();
            Assert.IsNotNull(integrity.CaptureState());
            Assert.IsNotNull(waste.CaptureState());
            Assert.IsNotNull(vermin.CaptureState());
            Assert.IsNotNull(jury.CaptureState());
            Assert.IsNotNull(freeze.CaptureState());
            Assert.IsNotNull(water.CaptureState());

            string dir = TempDir("shelter");
            try
            {
                var ss = MakeSave(dir, s =>
                {
                    s.SetStructuralIntegritySystem(integrity);
                    s.SetWasteSystem(waste);
                    s.SetVerminSystem(vermin);
                    s.SetJuryRigSystem(jury);
                    s.SetFreezePipeSystem(freeze);
                    s.SetWaterStorage(water);
                });
                Assert.IsTrue(HasSaveable(ss, "structural_integrity"));
                Assert.IsTrue(HasSaveable(ss, "waste"));
                Assert.IsTrue(HasSaveable(ss, "vermin"));
                Assert.IsTrue(HasSaveable(ss, "jury_rig"));
                Assert.IsTrue(HasSaveable(ss, "freeze_pipe"));
                Assert.IsTrue(HasSaveable(ss, "water_storage"));
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void World_Narrative_Register()
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var inv = new InventoryClass { Capacity = 10, MaxWeight = 50f };
            var medical = new MedicalSystem(needs);
            var phase = new WorldPhaseSystem();
            var journal = new JournalSystem();
            var suspicion = new SuspicionTracker();
            var hatch = new HatchEntrapmentSystem();
            var corpse = new CorpseManagementSystem(needs, inv, medical);
            Assert.IsNotNull(phase.CaptureState());
            Assert.IsNotNull(journal.CaptureState());
            Assert.IsNotNull(suspicion.CaptureState());
            Assert.IsNotNull(hatch.CaptureState());
            Assert.IsNotNull(corpse.CaptureState());

            string dir = TempDir("world");
            try
            {
                var ss = MakeSave(dir, s =>
                {
                    s.SetWorldPhaseSystem(phase);
                    s.SetJournalSystem(journal);
                    s.SetSuspicionTracker(suspicion);
                    s.SetHatchEntrapment(hatch);
                    s.SetCorpseSystem(corpse);
                });
                Assert.IsTrue(HasSaveable(ss, "world_phase"));
                Assert.IsTrue(HasSaveable(ss, "journal"));
                Assert.IsTrue(HasSaveable(ss, "suspicion"));
                Assert.IsTrue(HasSaveable(ss, "hatch_entrapment"));
                Assert.IsTrue(HasSaveable(ss, "corpses"));
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void Photoperiod_And_Knowledge_Register()
        {
            var photo = new PhotoperiodSystem(null, null);
            var knowledge = new RadiationKnowledgeMap();
            Assert.IsNotNull(photo.GetState());
            Assert.IsNotNull(knowledge.CaptureState());

            string dir = TempDir("env");
            try
            {
                var ss = MakeSave(dir, s =>
                {
                    s.SetPhotoPeriodSystem(photo);
                    s.SetKnowledgeMap(knowledge);
                });
                Assert.IsTrue(HasSaveable(ss, "photoperiod"));
                Assert.IsTrue(HasSaveable(ss, "radiation_knowledge"));
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void Complex_SpecialPath_Stay_FieldOnly()
        {
            // These remain field injects (custom restore / scheduled capture).
            string dir = TempDir("complex");
            try
            {
                var ss = MakeSave(dir, s =>
                {
                    s.SetExpeditionSystem(null);
                    s.SetEventRunner(null);
                    s.SetGeneratedMap(null);
                    s.SetShiftingHotspotSystem(null);
                    s.SetFactionRaidPlanSystem(null);
                    s.SetClothingSystem(null);
                    s.SetMentalBreakSystem(null);
                    s.SetPhantomIntruderSystem(null);
                });
                Assert.IsFalse(HasSaveable(ss, "expedition"));
                Assert.IsFalse(HasSaveable(ss, "event_runner"));
                Assert.IsFalse(HasSaveable(ss, "generated_map"));
                Assert.IsFalse(HasSaveable(ss, "shifting_hotspots"));
                Assert.IsFalse(HasSaveable(ss, "faction_raid_plans"));
                Assert.IsFalse(HasSaveable(ss, "clothing"));
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void Inventory_Register_With_ItemLookup()
        {
            var inv = new InventoryClass { Capacity = 10, MaxWeight = 50f };
            Assert.IsNotNull(inv.CaptureState());
            string dir = TempDir("inv");
            try
            {
                var ss = MakeSave(dir, s => s.SetInventory(inv));
                Assert.IsTrue(HasSaveable(ss, "inventory"));
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }
    }
}
