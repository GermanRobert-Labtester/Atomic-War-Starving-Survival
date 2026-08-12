using NUnit.Framework;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    // ═══════════════════════════════════════════════════════════════════
    //  Save/Load round-trip wiring tests for Black Aquifer systems
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class BlackAquiferSaveWiringTests
    {
        [Test]
        public void HydrostaticPressureSave_HasCanonicalId()
        {
            var save = new HydrostaticPressureSave();
            Assert.AreEqual("hydrostatic_pressure", save.systemId);
        }

        [Test]
        public void TunnelingStressSave_HasCanonicalId()
        {
            var save = new TunnelingAndStructuralStressSave();
            Assert.AreEqual("tunneling_stress", save.systemId);
        }

        [Test]
        public void MyceliumNetworkSave_HasCanonicalId()
        {
            var save = new MyceliumNetworkSave();
            Assert.AreEqual("mycelium_network", save.systemId);
        }

        [Test]
        public void HydrostaticPressure_RoundTrips()
        {
            var sys = new HydrostaticPressureSystem();
            sys.SetPumpRate(7f);
            sys.Tick(5f);
            var save = sys.CaptureState();

            var sys2 = new HydrostaticPressureSystem();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.PumpRateLitersPerHour, sys2.PumpRateLitersPerHour, 0.001f);
            Assert.AreEqual(sys.CleanLensDepth, sys2.CleanLensDepth, 0.001f);
            Assert.AreEqual(sys.SludgePressureKpa, sys2.SludgePressureKpa, 0.001f);
            Assert.AreEqual(sys.ToxicityIndex, sys2.ToxicityIndex, 0.001f);
        }

        [Test]
        public void TunnelingStress_RoundTrips()
        {
            var sys = new TunnelingAndStructuralStress();
            sys.BeginExcavation("sub_level_4", 4, true);
            sys.Tick(20f);
            sys.InstallShoring("sub_level_4");
            var save = sys.CaptureState();

            var sys2 = new TunnelingAndStructuralStress();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.DeepestExcavatedLevel, sys2.DeepestExcavatedLevel);
            Assert.AreEqual(sys.OverburdenStress, sys2.OverburdenStress, 0.001f);
            Assert.AreEqual(sys.ShoringTimberInstalled, sys2.ShoringTimberInstalled);
        }

        [Test]
        public void MyceliumNetwork_RoundTrips()
        {
            var sys = new MyceliumNetworkSystem();
            sys.RegisterRoom("quarters");
            sys.RegisterRoom("plant");

            var sv = new AtomicWar._Game.Survivors.Survivor
            {
                Id = "sv_dead",
                DisplayName = "Dead",
                CurrentRoomId = "quarters",
                State = AtomicWar._Game.Survivors.SurvivorState.Idle
            };
            sys.OnCorpseSpawned(sv, "quarters");
            sys.Tick(5f);

            var save = sys.CaptureState();
            var sys2 = new MyceliumNetworkSystem();
            sys2.RestoreState(save);

            Assert.AreEqual(sys.GetSporeDensity("quarters"), sys2.GetSporeDensity("quarters"), 0.001f);
            Assert.AreEqual(sys.GetSporeDensity("plant"), sys2.GetSporeDensity("plant"), 0.001f);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  Affliction ID wiring tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class BlackAquiferAfflictionIdTests
    {
        [Test]
        public void ChemicalToxicity_HasCanonicalId()
        {
            Assert.AreEqual("affliction_chemical_toxicity", AfflictionSO.Ids.ChemicalToxicity);
        }

        [Test]
        public void MycoHallucinations_HasCanonicalId()
        {
            Assert.AreEqual("affliction_myco_hallucinations", AfflictionSO.Ids.MycoHallucinations);
        }

        [Test]
        public void SporeLung_AlreadyExists()
        {
            Assert.AreEqual("affliction_spore_lung", AfflictionSO.Ids.SporeLung);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  Faction ID wiring tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class BlackAquiferFactionIdTests
    {
        [Test]
        public void SumpDredgers_HasCanonicalId()
        {
            Assert.AreEqual("sump_dredgers", FactionSO.Ids.SumpDredgers);
        }

        [Test]
        public void RotFarmers_HasCanonicalId()
        {
            Assert.AreEqual("rot_farmers", FactionSO.Ids.RotFarmers);
        }

        [Test]
        public void HydroBarons_HasCanonicalId()
        {
            Assert.AreEqual("hydro_barons", FactionSO.Ids.HydroBarons);
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  Item catalog wiring tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class BlackAquiferItemWiringTests
    {
        [Test]
        public void AllCatalogIds_MatchConstants()
        {
            var items = BlackAquiferItemCatalog.CreateAll();
            Assert.AreEqual(BlackAquiferItemCatalog.Item_ShoringTimber, items[0].id);
            Assert.AreEqual(BlackAquiferItemCatalog.Item_MyceliumBricks, items[1].id);
            Assert.AreEqual(BlackAquiferItemCatalog.Item_RebreatherScrubber, items[2].id);
            Assert.AreEqual(BlackAquiferItemCatalog.Item_BlackWaterVial, items[3].id);
            Assert.AreEqual(BlackAquiferItemCatalog.Item_GeigerTether, items[4].id);
            Assert.AreEqual(BlackAquiferItemCatalog.Item_BioluminescentMoss, items[5].id);
            Assert.AreEqual(BlackAquiferItemCatalog.Item_PneumaticJack, items[6].id);
            Assert.AreEqual(BlackAquiferItemCatalog.Item_ROMembrane, items[7].id);
            Assert.AreEqual(BlackAquiferItemCatalog.Item_FungicideFogger, items[8].id);
            Assert.AreEqual(BlackAquiferItemCatalog.Item_SubmergedServer, items[9].id);
        }

        [Test]
        public void AllItems_HaveNonEmptyDisplayName()
        {
            var items = BlackAquiferItemCatalog.CreateAll();
            for (int i = 0; i < items.Count; i++)
                Assert.IsFalse(string.IsNullOrEmpty(items[i].displayName), $"Item {items[i].id} has empty displayName");
        }

        [Test]
        public void AllItems_HaveNonEmptyDescription()
        {
            var items = BlackAquiferItemCatalog.CreateAll();
            for (int i = 0; i < items.Count; i++)
                Assert.IsFalse(string.IsNullOrEmpty(items[i].description), $"Item {items[i].id} has empty description");
        }

        [Test]
        public void AllItems_HavePositiveWeight()
        {
            var items = BlackAquiferItemCatalog.CreateAll();
            for (int i = 0; i < items.Count; i++)
                Assert.Greater(items[i].weight, 0f, $"Item {items[i].id} has zero weight");
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  System integration smoke tests
    // ═══════════════════════════════════════════════════════════════════

    [TestFixture]
    public class BlackAquiferIntegrationSmokeTests
    {
        [Test]
        public void HydrostaticAndMycelium_CanCoexist()
        {
            var hydro = new HydrostaticPressureSystem();
            var myco = new MyceliumNetworkSystem();
            myco.RegisterRoom("quarters");

            hydro.SetPumpRate(5f);
            hydro.Tick(1f);
            myco.Tick(1f);

            // Both should tick without interference
            Assert.Pass();
        }

        [Test]
        public void TunnelingAndMycelium_GasPocketDoesNotCrash()
        {
            var tunnel = new TunnelingAndStructuralStress();
            var myco = new MyceliumNetworkSystem();
            myco.RegisterRoom("sub_level_4");

            var rng = new System.Random(42);
            tunnel.BeginExcavation("sub_level_4", 4, true);
            for (int i = 0; i < 50; i++)
            {
                tunnel.Tick(1f, rng);
                myco.Tick(1f);
            }
            Assert.Pass();
        }

        [Test]
        public void FullExpansionCycle_NoErrors()
        {
            // Simulate a full day with all three systems ticking
            var hydro = new HydrostaticPressureSystem();
            var tunnel = new TunnelingAndStructuralStress();
            var myco = new MyceliumNetworkSystem();

            myco.RegisterRoom("quarters");
            myco.RegisterRoom("plant");
            tunnel.BeginExcavation("sub_level_4", 4, true);
            hydro.SetPumpRate(3f);

            var rng = new System.Random(123);
            for (int h = 0; h < 24; h++)
            {
                hydro.Tick(1f);
                tunnel.Tick(1f, rng);
                myco.Tick(1f);
            }
            hydro.TickDaily(1);

            Assert.Greater(hydro.TotalWaterExtracted, 0f);
            Assert.Pass();
        }
    }
}
