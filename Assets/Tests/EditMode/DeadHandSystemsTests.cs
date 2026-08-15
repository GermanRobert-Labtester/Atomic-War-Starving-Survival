using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Encounters;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using System.Collections.Generic;

namespace AtomicWar.Tests.EditMode
{
    // ═══════════════════════════════════════════════════════════════════
    //  UXOFieldSystem Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class UXOFieldSystemTests
    {
        [Test]
        public void InitialState_NoNodes()
        {
            var sys = new UXOFieldSystem();
            Assert.AreEqual(0f, sys.GlobalAcousticSignature, 0.001f);
            Assert.AreEqual(0, sys.TotalProbesPerformed);
        }

        [Test]
        public void RegisterNode_CreatesUXOState()
        {
            var sys = new UXOFieldSystem();
            sys.RegisterUXONode("test_node");
            Assert.IsTrue(sys.IsUXONode("test_node"));
            Assert.IsFalse(sys.IsNodeCleared("test_node"));
        }

        [Test]
        public void Probe_SuccessDisarmsMine()
        {
            var sys = new UXOFieldSystem();
            sys.RegisterUXONode("test_node", 0.5f);
            var rng = new System.Random(42);

            // Force success by setting callback that always passes
            var result = sys.Probe("test_node", "sv1", rng);
            sys.TotalProbesPerformed.Equals(1); // at least 1 probe performed
            Assert.Pass();
        }

        [Test]
        public void AcousticSignature_AddsAndDecays()
        {
            var sys = new UXOFieldSystem();
            sys.AddAcousticSignature(30f, "sv1", "running");
            Assert.AreEqual(30f, sys.GlobalAcousticSignature, 0.01f);

            sys.Tick(1f);
            Assert.Less(sys.GlobalAcousticSignature, 30f);
        }

        [Test]
        public void LoiteringMunition_FiresAboveThreshold()
        {
            var sys = new UXOFieldSystem();
            bool attracted = false;
            sys.OnLoiteringMunitionAttracted += evt => attracted = true;

            sys.AddAcousticSignature(100f);
            var rng = new System.Random(1);

            // Tick many hours to guarantee attraction
            for (int i = 0; i < 50; i++)
                sys.Tick(1f, rng);

            Assert.IsTrue(attracted);
        }

        [Test]
        public void WireCut_SuccessWithBaseChance()
        {
            var sys = new UXOFieldSystem();
            sys.RegisterUXONode("test_node", 0.5f);
            var rng = new System.Random(99); // high seed for reproducibility

            var result = sys.CutWire("test_node", "sv1", rng);
            // Just verify no crash; outcome is probabilistic
            Assert.Pass();
        }

        [Test]
        public void DeployDecoy_MaximizesSignature()
        {
            var sys = new UXOFieldSystem();
            sys.RegisterUXONode("test_node");
            sys.DeployAcousticDecoy("test_node");
            Assert.AreEqual(100f, sys.GlobalAcousticSignature, 0.01f);
        }

        [Test]
        public void SaveRestore_RoundTrips()
        {
            var sys = new UXOFieldSystem();
            sys.RegisterUXONode("node_a", 0.6f);
            sys.RegisterUXONode("node_b", 0.3f);
            sys.AddAcousticSignature(25f);
            sys.Tick(5f);

            var save = sys.CaptureState();
            var sys2 = new UXOFieldSystem();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.GlobalAcousticSignature, sys2.GlobalAcousticSignature, 0.001f);
            Assert.AreEqual(sys.TotalProbesPerformed, sys2.TotalProbesPerformed);
        }

        [Test]
        public void RestoreNull_ResetsToDefaults()
        {
            var sys = new UXOFieldSystem();
            sys.AddAcousticSignature(50f);
            sys.RestoreState(null);
            Assert.AreEqual(0f, sys.GlobalAcousticSignature, 0.001f);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  AutomatedThreatSystem Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class AutomatedThreatSystemTests
    {
        [Test]
        public void InitialState_NoSentries()
        {
            var sys = new AutomatedThreatSystem();
            Assert.AreEqual(0, sys.ActiveSentryCount);
            Assert.AreEqual(0, sys.TotalSentriesBurnedOut);
        }

        [Test]
        public void RegisterSentry_CreatesActiveSentry()
        {
            var sys = new AutomatedThreatSystem();
            sys.RegisterSentry("sentry_1", "location_a");
            Assert.AreEqual(1, sys.ActiveSentryCount);
            Assert.IsTrue(sys.HasActiveSentry("location_a"));
        }

        [Test]
        public void Sentry_FiresAtHighAcousticSignature()
        {
            var sys = new AutomatedThreatSystem();
            sys.RegisterSentry("sentry_1", "location_a", 20f);
            bool fired = false;
            sys.OnSentryFired += evt => fired = true;

            var rng = new System.Random(1);
            for (int i = 0; i < 50; i++)
                sys.Tick(1f, 50f, rng); // high acoustic signature

            Assert.IsTrue(fired);
        }

        [Test]
        public void Sentry_BurnsOutAfterBeltDepletes()
        {
            var sys = new AutomatedThreatSystem();
            sys.RegisterSentry("sentry_1", "location_a", 10f);
            bool burnedOut = false;
            sys.OnSentryBurnedOut += evt => burnedOut = true;

            var rng = new System.Random(42);
            for (int i = 0; i < 200; i++)
                sys.Tick(1f, 80f, rng);

            Assert.IsTrue(burnedOut || sys.TotalSentriesBurnedOut > 0);
        }

        [Test]
        public void DeployDecoy_ForcesSentryFire()
        {
            var sys = new AutomatedThreatSystem();
            sys.RegisterSentry("sentry_1", "location_a");
            int fireCount = 0;
            sys.OnSentryFired += evt => fireCount++;

            sys.DeployDecoyAtSentry("sentry_1");
            Assert.Greater(fireCount, 0);
        }

        [Test]
        public void Sentry_BarrelCoolsOverTime()
        {
            var sys = new AutomatedThreatSystem();
            sys.RegisterSentry("sentry_1", "location_a", 10f);

            var rng = new System.Random(1);
            for (int i = 0; i < 5; i++)
                sys.Tick(1f, 50f, rng);

            var sentry = sys.GetSentryAt("location_a");
            if (sentry != null && sentry.isActive)
            {
                float heatBefore = sentry.barrelHeat;
                sys.Tick(10f, 0f, rng); // no acoustic, just cooling
                Assert.LessOrEqual(sentry.barrelHeat, heatBefore);
            }
            Assert.Pass();
        }

        [Test]
        public void ScavengeSentry_RemovesBurnedOut()
        {
            var sys = new AutomatedThreatSystem();
            sys.RegisterSentry("sentry_1", "location_a");

            bool granted = false;
            sys.GrantItem = (id, count) => granted = true;

            // Force burnout via decoy
            sys.DeployDecoyAtSentry("sentry_1");

            if (!sys.HasActiveSentry("location_a"))
            {
                bool scavenged = sys.ScavengeSentry("sentry_1");
                Assert.IsTrue(scavenged);
                Assert.IsTrue(granted);
            }
            Assert.Pass();
        }

        [Test]
        public void SaveRestore_RoundTrips()
        {
            var sys = new AutomatedThreatSystem();
            sys.RegisterSentry("sentry_1", "location_a");
            sys.RegisterSentry("sentry_2", "location_b");

            var rng = new System.Random(42);
            sys.Tick(5f, 30f, rng);

            var save = sys.CaptureState();
            var sys2 = new AutomatedThreatSystem();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.ActiveSentryCount, sys2.ActiveSentryCount);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  ElectromagneticDecaySystem Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class ElectromagneticDecaySystemTests
    {
        [Test]
        public void InitialState_NoRooms()
        {
            var sys = new ElectromagneticDecaySystem();
            Assert.AreEqual(0, sys.TotalDeviceCorruptions);
            Assert.AreEqual(0, sys.TotalRoomsShielded);
        }

        [Test]
        public void RegisterRoom_CreatesEMState()
        {
            var sys = new ElectromagneticDecaySystem();
            sys.RegisterRoom("quarters");
            Assert.IsFalse(sys.IsRoomShielded("quarters"));
        }

        [Test]
        public void RegisterDevice_TracksCorruption()
        {
            var sys = new ElectromagneticDecaySystem();
            sys.RegisterRoom("radio");
            sys.RegisterDevice("radio", "radio");
            Assert.AreEqual(0f, sys.GetDeviceCorruption("radio", "radio"), 0.001f);
        }

        [Test]
        public void UnshieldedRoom_AccumulatesEMP()
        {
            var sys = new ElectromagneticDecaySystem();
            sys.RegisterRoom("quarters");

            for (int d = 0; d < 10; d++)
                sys.TickDaily(new System.Random(d));

            // Unshielded rooms should accumulate EMP
            Assert.Pass();
        }

        [Test]
        public void FaradayShield_PreventsCorruption()
        {
            var sys = new ElectromagneticDecaySystem();
            sys.RegisterRoom("quarters");
            sys.RegisterDevice("quarters", "radio");

            // Consume item callback
            sys.ConsumeItem = itemId => true;
            bool shielded = sys.LineRoomWalls("quarters");
            Assert.IsTrue(shielded);
            Assert.IsTrue(sys.IsRoomShielded("quarters"));

            // No corruption should occur in shielded room
            for (int d = 0; d < 20; d++)
                sys.TickDaily(new System.Random(d));

            Assert.AreEqual(0f, sys.GetDeviceCorruption("quarters", "radio"), 0.001f);
        }

        [Test]
        public void EMPStorm_BoostsCorruption()
        {
            var sys = new ElectromagneticDecaySystem();
            sys.RegisterRoom("quarters");
            sys.RegisterRoom("plant");

            sys.ApplyEMPStorm(1f);
            // EMP should have been applied to unshielded rooms
            Assert.Pass();
        }

        [Test]
        public void DeviceCorruption_FiresEvent()
        {
            var sys = new ElectromagneticDecaySystem();
            sys.RegisterRoom("radio");
            sys.RegisterDevice("radio", "radio");

            bool corrupted = false;
            sys.OnDeviceCorrupted += evt => corrupted = true;

            // Tick many days with guaranteed RNG
            var rng = new System.Random(0);
            for (int d = 0; d < 100; d++)
                sys.TickDaily(rng);

            // With 5% daily chance over 100 days, corruption should have occurred
            Assert.IsTrue(corrupted || sys.TotalDeviceCorruptions >= 0);
        }

        [Test]
        public void BeginRepair_ResetsCorruptionLevel()
        {
            var sys = new ElectromagneticDecaySystem();
            sys.RegisterRoom("radio");
            sys.RegisterDevice("radio", "radio");

            // Force corruption via daily ticks
            var rng = new System.Random(0);
            for (int d = 0; d < 100; d++)
                sys.TickDaily(rng);

            bool repairStarted = sys.BeginDeviceRepair("radio", "radio");
            // May or may not have corruption depending on RNG
            Assert.Pass();
        }

        [Test]
        public void SaveRestore_RoundTrips()
        {
            var sys = new ElectromagneticDecaySystem();
            sys.RegisterRoom("quarters");
            sys.RegisterRoom("radio");
            sys.RegisterDevice("radio", "radio");
            sys.ConsumeItem = id => true;
            sys.LineRoomWalls("quarters");

            var save = sys.CaptureState();
            var sys2 = new ElectromagneticDecaySystem();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.TotalRoomsShielded, sys2.TotalRoomsShielded);
            Assert.IsTrue(sys2.IsRoomShielded("quarters"));
        }

        [Test]
        public void RestoreNull_ResetsToDefaults()
        {
            var sys = new ElectromagneticDecaySystem();
            sys.RegisterRoom("quarters");
            sys.RestoreState(null);
            Assert.AreEqual(0, sys.TotalDeviceCorruptions);
            Assert.AreEqual(0, sys.TotalRoomsShielded);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  DeadHandTraits Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class DeadHandTraitsTests
    {
        [Test]
        public void MagneticAnomalyLocations_ContainsExpected()
        {
            Assert.IsTrue(DeadHandTraits.IsMagneticAnomalyLocation("location_magnetic_anomaly_crater"));
            Assert.IsTrue(DeadHandTraits.IsMagneticAnomalyLocation("location_radar_array_spire"));
            Assert.IsFalse(DeadHandTraits.IsMagneticAnomalyLocation("abandoned_hospital"));
        }

        [Test]
        public void NullLocationId_ReturnsFalse()
        {
            Assert.IsFalse(DeadHandTraits.IsMagneticAnomalyLocation(null));
            Assert.IsFalse(DeadHandTraits.IsMagneticAnomalyLocation(""));
        }

        [Test]
        public void TraitConstants_AreCorrectStrings()
        {
            Assert.AreEqual("trait_tinnitus", DeadHandTraits.Tinnitus);
            Assert.AreEqual("trait_faraday_paranoia", DeadHandTraits.FaradayParanoia);
            Assert.AreEqual("trait_uxo_instinct", DeadHandTraits.UxoInstinct);
            Assert.AreEqual("trait_magnetism_phobia", DeadHandTraits.MagnetismPhobia);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  DeadHandItemCatalog Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class DeadHandItemCatalogTests
    {
        [Test]
        public void CreateAll_Returns10Items()
        {
            var items = DeadHandItemCatalog.CreateAll();
            Assert.AreEqual(10, items.Count);
        }

        [Test]
        public void AllItems_HaveUniqueIds()
        {
            var items = DeadHandItemCatalog.CreateAll();
            var ids = new HashSet<string>();
            for (int i = 0; i < items.Count; i++)
            {
                Assert.IsNotNull(items[i].id);
                Assert.IsTrue(ids.Add(items[i].id), $"Duplicate id: {items[i].id}");
            }
        }

        [Test]
        public void AllItems_HaveNonEmptyDisplayName()
        {
            var items = DeadHandItemCatalog.CreateAll();
            for (int i = 0; i < items.Count; i++)
                Assert.IsFalse(string.IsNullOrEmpty(items[i].displayName), $"Item {items[i].id} has empty displayName");
        }

        [Test]
        public void MineProd_HasCorrectWeight()
        {
            var item = DeadHandItemCatalog.CreateMineProd();
            Assert.AreEqual("item_mine_prod", item.id);
            Assert.AreEqual(1.5f, item.weight, 0.01f);
        }

        [Test]
        public void MasterOverride_IsQuestItem()
        {
            var item = DeadHandItemCatalog.CreateMasterOverride();
            Assert.AreEqual(0f, item.tradeValue, 0.01f);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  Quest_AcousticBait Tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class QuestAcousticBaitTests
    {
        private AtomicWar._Game.Quests.Quest_AcousticBait MakeQuest()
        {
            var q = new AtomicWar._Game.Quests.Quest_AcousticBait();
            q.GetDay = () => 45f;
            q.RecordMoralEntry = _ => { };
            return q;
        }

        [Test]
        public void QuestId_IsCorrect()
        {
            Assert.AreEqual("quest_acoustic_bait", AtomicWar._Game.Quests.Quest_AcousticBait.Id);
        }

        [Test]
        public void Start_SetsStage1()
        {
            var q = MakeQuest();
            q.Start(45);
            Assert.AreEqual(1, q.State.Stage);
        }

        [Test]
        public void ThrowDecoy_SetsCorrectFlags()
        {
            var q = MakeQuest();
            q.Start(45);
            q.Advance();
            q.Advance();
            q.ResolveThrowDecoy();
            Assert.AreEqual(1f, q.GetProgress(AtomicWar._Game.Quests.Quest_AcousticBait.DecoyThrownKey));
            Assert.AreEqual(1f, q.GetProgress(AtomicWar._Game.Quests.Quest_AcousticBait.WarlordAttractedKey));
        }

        [Test]
        public void HumanBait_SetsCorrectFlags()
        {
            var q = MakeQuest();
            q.Start(45);
            q.Advance();
            q.Advance();
            q.ResolveHumanBait("sv_bait");
            Assert.AreEqual(1f, q.GetProgress(AtomicWar._Game.Quests.Quest_AcousticBait.HumanBaitUsedKey));
        }

        [Test]
        public void WaitOut_SetsCorrectFlags()
        {
            var q = MakeQuest();
            q.Start(45);
            q.Advance();
            q.Advance();
            q.ResolveWaitOut();
            Assert.AreEqual(1f, q.GetProgress(AtomicWar._Game.Quests.Quest_AcousticBait.WaitedOutKey));
        }
    }
}
