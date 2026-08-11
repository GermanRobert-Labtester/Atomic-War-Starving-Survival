using NUnit.Framework;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Simulation;

namespace AtomicWar.Tests.EditMode
{
    // Test helpers shared by all new-system tests below.
    internal static class NewContentTestHelpers
    {
        public static Survivor MakeSurvivor(string id)
        {
            return new Survivor { Id = id, DisplayName = id, State = SurvivorState.Idle };
        }
    }

    [TestFixture]
    public class SleepDeprivationSystemTests
    {
        [Test]
        public void FullSleepResetsConsecutiveMissedNights()
        {
            var sys = new SleepDeprivationSystem();
            var sv = NewContentTestHelpers.MakeSurvivor("sv1");
            sys.GetOrCreate(sv.Id).ConsecutiveMissedNights = 2;
            sys.OnEndOfDay(sv, currentDay: 3, currentCycleHour: 24f, hoursSleptLastNight: 7f);
            Assert.AreEqual(0, sys.Get(sv.Id).ConsecutiveMissedNights);
        }

        [Test]
        public void FourMissedNightsTriggersCollapse()
        {
            var sys = new SleepDeprivationSystem();
            var sv = NewContentTestHelpers.MakeSurvivor("sv1");
            for (int d = 0; d < 4; d++)
                sys.OnEndOfDay(sv, d, d * 24f, 0f);
            Assert.IsTrue(sys.IsCollapsed(sv));
            Assert.IsTrue(sys.IsHallucinating(sv));
        }

        [Test]
        public void BedTypeAffectsRecovery()
        {
            var sys = new SleepDeprivationSystem
            {
                GetBedTypeIdForSurvivor = _ => "woolbed"
            };
            var sv = NewContentTestHelpers.MakeSurvivor("sv1");
            // 4.5h * 0.85 = 3.83 effective, less than 6 -> miss
            sys.OnEndOfDay(sv, 0, 0f, 4.5f);
            Assert.AreEqual(1, sys.Get(sv.Id).ConsecutiveMissedNights);
        }

        [Test]
        public void SaveRestoreRoundTrips()
        {
            var sys = new SleepDeprivationSystem();
            var sv = NewContentTestHelpers.MakeSurvivor("sv1");
            sys.OnEndOfDay(sv, 0, 0f, 0f);
            var state = sys.CaptureState();
            var sys2 = new SleepDeprivationSystem();
            sys2.RestoreState(state);
            Assert.AreEqual(1, sys2.Get(sv.Id).MissedNights);
        }
    }

    [TestFixture]
    public class ShelterDegradationSystemTests
    {
        [Test]
        public void HatchSealDoesNotDegradeBeforeDay20()
        {
            var sys = new ShelterDegradationSystem { GetDay = () => 5f };
            sys.Tick();
            Assert.AreEqual(1f, sys.Current.HatchSealIntegrity, 0.001f);
        }

        [Test]
        public void HatchSealDegradesAfterDay20()
        {
            var sys = new ShelterDegradationSystem
            {
                GetDay = () => 21f,
                Rng = new System.Random(1)
            };
            sys.Tick();
            Assert.Less(sys.Current.HatchSealIntegrity, 1f);
        }

        [Test]
        public void PipesDevelopLeaksOverTime()
        {
            var sys = new ShelterDegradationSystem { GetDay = () => 75f, Rng = new System.Random(1) };
            sys.Tick();
            Assert.Greater(sys.Current.ActivePipeLeaks, 0);
        }
    }

    [TestFixture]
    public class GriefSystemTests
    {
        [Test]
        public void CloseBondCausesWorkRefusal()
        {
            var grief = new GriefSystem
            {
                GetDay = () => 5f,
                GetAffinity = (a, b) => 0.9f,
                ApplyMoraleDelta = (s, d) => { },
                MarkProductivityReduced = s => { },
                AddAffliction = (s, id) => { },
                Rng = new System.Random(1)
            };
            var dead = NewContentTestHelpers.MakeSurvivor("dead");
            var friend = NewContentTestHelpers.MakeSurvivor("friend");
            grief.OnSurvivorDied(dead, new[] { friend });
            Assert.IsTrue(grief.IsRefusingWork("friend"));
        }

        [Test]
        public void EnemyAffinityGrantsPositiveMorale()
        {
            float applied = 0f;
            var grief = new GriefSystem
            {
                GetDay = () => 5f,
                GetAffinity = (a, b) => -0.5f,
                ApplyMoraleDelta = (s, d) => applied += d,
                Rng = new System.Random(1)
            };
            var dead = NewContentTestHelpers.MakeSurvivor("dead");
            var enemy = NewContentTestHelpers.MakeSurvivor("enemy");
            grief.OnSurvivorDied(dead, new[] { enemy });
            Assert.AreEqual(5f, applied, 0.001f);
        }
    }

    [TestFixture]
    public class AshAccumulationSystemTests
    {
        [Test]
        public void AshAccumulatesEachDay()
        {
            var sys = new AshAccumulationSystem { GetDay = () => 5f };
            sys.Tick();
            Assert.Greater(sys.Current.SurfaceCm, 0f);
        }

        [Test]
        public void HatchTwoPeopleTriggersAt5cm()
        {
            var sys = new AshAccumulationSystem { GetDay = () => 30f };
            sys.Tick();
            Assert.IsTrue(sys.HatchRequiresTwoPeople());
        }

        [Test]
        public void ClearingAshReducesAccumulation()
        {
            var sys = new AshAccumulationSystem { GetDay = () => 30f, GetRosterSize = () => 1 };
            sys.Tick();
            float before = sys.Current.HatchCm;
            sys.ClearAsh(2f, "hatch");
            Assert.Less(sys.Current.HatchCm, before);
        }
    }

    [TestFixture]
    public class DiseaseMutationSystemTests
    {
        [Test]
        public void AbortEvolvesToResistant()
        {
            var sys = new DiseaseMutationSystem();
            var inf = sys.StartInfection("sv1", "wound", new System.Random(1));
            sys.AbortTreatment(inf);
            Assert.AreEqual(DiseaseMutationSystem.Resistance.Resistant, inf.Resistance);
            Assert.AreEqual(4f, inf.PillsRequired);
        }

        [Test]
        public void TwoAbortsEvolveToMultiResistant()
        {
            var sys = new DiseaseMutationSystem();
            var inf = sys.StartInfection("sv1", "lung", new System.Random(1));
            sys.AbortTreatment(inf);
            sys.AbortTreatment(inf);
            Assert.AreEqual(DiseaseMutationSystem.Resistance.MultiResistant, inf.Resistance);
        }
    }

    [TestFixture]
    public class NoiseDisciplineSystemTests
    {
        [Test]
        public void LoudTriggersIncreasedRaidProbability()
        {
            var sys = new NoiseDisciplineSystem();
            sys.RegisterSource("generator", 15f);
            sys.SetSourceActive("generator", true);
            sys.RegisterSource("hammering", 20f);
            sys.SetSourceActive("hammering", true);
            sys.RegisterSource("radio", 25f);
            sys.SetSourceActive("radio", true);
            // 15 + 20 + 25 = 60 (no mitigation)
            Assert.AreEqual("loud", sys.Severity);
            Assert.AreEqual(0.35f, sys.GetRaidProbabilityDelta(), 0.001f);
        }

        [Test]
        public void SilentWhenNoSourcesActive()
        {
            var sys = new NoiseDisciplineSystem();
            sys.RegisterSource("generator", 15f);
            sys.SetSourceActive("generator", false);
            Assert.AreEqual("silent", sys.Severity);
        }
    }

    [TestFixture]
    public class CalorieAccountingSystemTests
    {
        [Test]
        public void LightWorkDefaultTargetIs1800()
        {
            var sys = new CalorieAccountingSystem();
            var sv = NewContentTestHelpers.MakeSurvivor("sv1");
            Assert.AreEqual(1800f, sys.DailyTarget(sv), 0.001f);
        }

        [Test]
        public void ConsumedKcalAccumulates()
        {
            var sys = new CalorieAccountingSystem();
            var sv = NewContentTestHelpers.MakeSurvivor("sv1");
            sys.Consume(sv, "canned_food", 2);
            Assert.AreEqual(900f, sys.GetKcalToday(sv), 0.001f);
        }

        [Test]
        public void StarvingSurvivorAfterThreeDaysTriggersDeath()
        {
            int deaths = 0;
            var sys = new CalorieAccountingSystem();
            var sv = NewContentTestHelpers.MakeSurvivor("sv1");
            sys.OnStarvationDeath += s => deaths++;
            for (int d = 0; d < 4; d++) sys.Tick(sv, d);
            Assert.GreaterOrEqual(deaths, 1);
        }
    }
}
