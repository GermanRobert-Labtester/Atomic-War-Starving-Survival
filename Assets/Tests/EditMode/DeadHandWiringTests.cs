using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Encounters;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Environment;
using System.Collections.Generic;

namespace AtomicWar.Tests.EditMode
{
    // ═══════════════════════════════════════════════════════════════════
    //  Save/Load round-trip wiring tests for Dead Hand systems
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class DeadHandSaveWiringTests
    {
        [Test]
        public void UXOFieldSave_HasCanonicalId()
        {
            var save = new UXOFieldSystemSave();
            Assert.AreEqual("uxo_field", save.systemId);
        }

        [Test]
        public void AutomatedThreatSave_HasCanonicalId()
        {
            var save = new AutomatedThreatSystemSave();
            Assert.AreEqual("automated_threats", save.systemId);
        }

        [Test]
        public void ElectromagneticDecaySave_HasCanonicalId()
        {
            var save = new ElectromagneticDecaySave();
            Assert.AreEqual("electromagnetic_decay", save.systemId);
        }

        [Test]
        public void UXOField_RoundTrips()
        {
            var sys = new UXOFieldSystem();
            sys.RegisterUXONode("node_a", 0.5f);
            sys.RegisterUXONode("node_b", 0.3f);
            sys.AddAcousticSignature(40f);
            sys.Tick(3f);

            var save = sys.CaptureState();
            var sys2 = new UXOFieldSystem();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.GlobalAcousticSignature, sys2.GlobalAcousticSignature, 0.001f);
            Assert.AreEqual(sys.TotalProbesPerformed, sys2.TotalProbesPerformed);
        }

        [Test]
        public void AutomatedThreats_RoundTrips()
        {
            var sys = new AutomatedThreatSystem();
            sys.RegisterSentry("sentry_1", "location_a");
            sys.RegisterSentry("sentry_2", "location_b");

            var rng = new System.Random(42);
            sys.Tick(10f, 30f, rng);

            var save = sys.CaptureState();
            var sys2 = new AutomatedThreatSystem();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.ActiveSentryCount, sys2.ActiveSentryCount);
            Assert.AreEqual(sys.TotalSentriesBurnedOut, sys2.TotalSentriesBurnedOut);
        }

        [Test]
        public void ElectromagneticDecay_RoundTrips()
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
    }

    // ═══════════════════════════════════════════════════════════════════
    //  Affliction ID wiring tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class DeadHandAfflictionIdTests
    {
        [Test]
        public void TraumaticAmputation_HasCanonicalId()
        {
            Assert.AreEqual("affliction_traumatic_amputation", AfflictionSO.Ids.TraumaticAmputation);
        }

        [Test]
        public void EMPPhantomBlip_HasCanonicalId()
        {
            Assert.AreEqual("affliction_emp_phantom_blip", AfflictionSO.Ids.EMPPhantomBlip);
        }

        [Test]
        public void LogicGateFailure_HasCanonicalId()
        {
            Assert.AreEqual("affliction_logic_gate_failure", AfflictionSO.Ids.LogicGateFailure);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  Faction ID wiring tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class DeadHandFactionIdTests
    {
        [Test]
        public void WireHeads_HasCanonicalId()
        {
            Assert.AreEqual("wire_heads", FactionSO.Ids.WireHeads);
        }

        [Test]
        public void EchoBats_HasCanonicalId()
        {
            Assert.AreEqual("echo_bats", FactionSO.Ids.EchoBats);
        }

        [Test]
        public void Custodians_HasCanonicalId()
        {
            Assert.AreEqual("custodians", FactionSO.Ids.Custodians);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  Item catalog wiring tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class DeadHandItemWiringTests
    {
        [Test]
        public void AllCatalogIds_MatchConstants()
        {
            var items = DeadHandItemCatalog.CreateAll();
            Assert.AreEqual(DeadHandItemCatalog.Item_MineProd, items[0].id);
            Assert.AreEqual(DeadHandItemCatalog.Item_FaradayMesh, items[1].id);
            Assert.AreEqual(DeadHandItemCatalog.Item_AcousticDecoy, items[2].id);
            Assert.AreEqual(DeadHandItemCatalog.Item_LogicBoard, items[3].id);
            Assert.AreEqual(DeadHandItemCatalog.Item_SoundBaffling, items[4].id);
            Assert.AreEqual(DeadHandItemCatalog.Item_EMPGrenade, items[5].id);
            Assert.AreEqual(DeadHandItemCatalog.Item_TungstenCore, items[6].id);
            Assert.AreEqual(DeadHandItemCatalog.Item_PneumaticHose, items[7].id);
            Assert.AreEqual(DeadHandItemCatalog.Item_MasterOverride, items[8].id);
            Assert.AreEqual(DeadHandItemCatalog.Item_HeadphonesMil, items[9].id);
        }

        [Test]
        public void AllItems_HavePositiveWeight()
        {
            var items = DeadHandItemCatalog.CreateAll();
            for (int i = 0; i < items.Count; i++)
                Assert.Greater(items[i].weight, 0f, $"Item {items[i].id} has zero weight");
        }

        [Test]
        public void AllItems_HaveNonEmptyDescription()
        {
            var items = DeadHandItemCatalog.CreateAll();
            for (int i = 0; i < items.Count; i++)
                Assert.IsFalse(string.IsNullOrEmpty(items[i].description), $"Item {items[i].id} has empty description");
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  Cross-system integration smoke tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class DeadHandIntegrationSmokeTests
    {
        [Test]
        public void UXOAndAutomatedThreats_CanCoexist()
        {
            var uxo = new UXOFieldSystem();
            var threats = new AutomatedThreatSystem();

            uxo.RegisterUXONode("location_uxo_highway_choke", 0.6f);
            threats.RegisterSentry("sentry_highway", "location_uxo_highway_choke");

            var rng = new System.Random(42);
            for (int i = 0; i < 24; i++)
            {
                uxo.Tick(1f, rng);
                threats.Tick(1f, uxo.GlobalAcousticSignature, rng);
            }
            Assert.Pass();
        }

        [Test]
        public void ElectromagneticAndBlackAquifer_NoConflict()
        {
            var em = new ElectromagneticDecaySystem();
            var hydro = new HydrostaticPressureSystem();

            em.RegisterRoom("plant");
            em.RegisterDevice("plant", "water_purifier");
            hydro.SetPumpRate(5f);

            for (int i = 0; i < 24; i++)
            {
                hydro.Tick(1f);
                em.TickDaily(new System.Random(i));
            }
            Assert.Pass();
        }

        [Test]
        public void FullDeadHandCycle_NoErrors()
        {
            var uxo = new UXOFieldSystem();
            var threats = new AutomatedThreatSystem();
            var em = new ElectromagneticDecaySystem();

            uxo.RegisterUXONode("node_1", 0.4f);
            threats.RegisterSentry("sentry_1", "node_1", 25f);
            em.RegisterRoom("radio");
            em.RegisterDevice("radio", "radio");

            var rng = new System.Random(123);
            for (int h = 0; h < 24; h++)
            {
                uxo.Tick(1f, rng);
                threats.Tick(1f, uxo.GlobalAcousticSignature, rng);
            }
            em.TickDaily(rng);

            Assert.GreaterOrEqual(uxo.GlobalAcousticSignature, 0f);
            Assert.Pass();
        }

        [Test]
        public void AllThreeExpansions_CanTickTogether()
        {
            // Black Aquifer
            var hydro = new HydrostaticPressureSystem();
            var tunnel = new TunnelingAndStructuralStress();
            var myco = new MyceliumNetworkSystem();
            myco.RegisterRoom("quarters");

            // Dead Hand
            var uxo = new UXOFieldSystem();
            var threats = new AutomatedThreatSystem();
            var em = new ElectromagneticDecaySystem();
            em.RegisterRoom("radio");

            uxo.RegisterUXONode("node_1");
            threats.RegisterSentry("sentry_1", "node_1");

            var rng = new System.Random(999);
            for (int h = 0; h < 24; h++)
            {
                hydro.Tick(1f);
                tunnel.Tick(1f, rng);
                myco.Tick(1f);
                uxo.Tick(1f, rng);
                threats.Tick(1f, uxo.GlobalAcousticSignature, rng);
            }
            em.TickDaily(rng);
            hydro.TickDaily(1);
            myco.TickDaily(null);

            Assert.Pass();
        }
    }
}
