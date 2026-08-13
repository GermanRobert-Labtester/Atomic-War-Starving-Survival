using NUnit.Framework;
using AtomicWar._Game.Medical;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class NewAfflictionSystemsTests
    {
        [Test]
        public void CrushSyndrome_ProgressionAndTreatment()
        {
            var system = new CrushSyndromeSystem();
            bool renalTriggered = false;
            system.OnRenalCrisisTriggered += (id) => renalTriggered = true;

            system.ContractCrushSyndrome("survivor_viktor", "collapsed_ceiling");
            Assert.AreEqual(1, system.Afflicted.Count);

            // Tick days until renal crisis threshold
            system.TickDay("survivor_viktor");
            system.TickDay("survivor_viktor");
            system.TickDay("survivor_viktor");

            Assert.IsTrue(renalTriggered);
            Assert.IsFalse(system.TryTreat("survivor_viktor", false)); // fails without treatment
            Assert.IsTrue(system.TryTreat("survivor_viktor", true));   // cured
            Assert.AreEqual(0, system.Afflicted.Count);
        }

        [Test]
        public void Silicosis_StaminaPenaltyAndTherapy()
        {
            var system = new SilicosisSystem();
            system.ContractSilicosis("survivor_kess", "pulverized_concrete");

            Assert.AreEqual(0.85f, system.GetStaminaPenaltyMultiplier("survivor_kess"));

            for (int i = 0; i < 20; i++)
                system.TickDay("survivor_kess");

            Assert.AreEqual(0.65f, system.GetStaminaPenaltyMultiplier("survivor_kess"));
            Assert.IsTrue(system.TryManageTherapy("survivor_kess", true));
        }

        [Test]
        public void ElectrolyteCrisis_OnsetAndSaltsCure()
        {
            var system = new ElectrolyteCrisisSystem();
            bool spasmsFired = false;
            system.OnMuscleSpasmsTriggered += (id) => spasmsFired = true;

            system.ContractCrisis("survivor_elena", "intense_heat_exhaustion");
            Assert.IsTrue(spasmsFired);
            Assert.AreEqual(1, system.Afflicted.Count);

            Assert.IsTrue(system.TryAdministerSalts("survivor_elena", true));
            Assert.AreEqual(0, system.Afflicted.Count);
        }

        [Test]
        public void LivestockFever_InfectionAndAntibioticTreatment()
        {
            var system = new LivestockFeverSystem();
            bool bedridden = false;
            system.OnBedriddenTriggered += (id) => bedridden = true;

            system.ContractFever("survivor_tomas", "sick_ash_goat");
            Assert.AreEqual(1, system.Afflicted.Count);

            system.TickDay("survivor_tomas");
            system.TickDay("survivor_tomas");

            Assert.IsTrue(bedridden);
            Assert.IsTrue(system.TryTreatWithAntibiotics("survivor_tomas", true));
            Assert.AreEqual(0, system.Afflicted.Count);
        }
    }
}
