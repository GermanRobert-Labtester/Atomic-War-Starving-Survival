using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// End-to-end proof that the id-keyed state maps survive a real save file.
    ///
    /// <see cref="SaveStateCompletenessTests"/> guards the shape of these systems;
    /// this guards the behaviour. Both are needed — a DTO can carry a list and still
    /// lose the data if capture, JSON serialization, or restore drops it, and that is
    /// exactly the failure these systems shipped with.
    ///
    /// Representative systems are covered rather than all fifteen: they were fixed by
    /// one shared helper (<see cref="SaveMap"/>), so the variation worth testing is
    /// the key field (survivorId / vehicleId / locationId), not the system.
    /// </summary>
    [TestFixture]
    public class AfflictionPersistenceRoundTripTests
    {
        private string _testDir;

        [SetUp]
        public void SetUp()
        {
            _testDir = Path.Combine(Path.GetTempPath(), "ashfall_afflict_" + System.Guid.NewGuid().ToString("N"));
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

        /// <summary>Bare SaveSystem; callers attach whichever subsystems the test needs.</summary>
        private SaveSystem MakeSaveSystem()
        {
            var ws = new WeatherSystem(null, 7);
            var ns = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>(), sv => true);
            return new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = new GameState(),
                WeatherSystem = ws,
                TemperatureSystem = new TemperatureSystem(null, ws),
                NeedsSystem = ns,
                RadiationSystem = new RadiationSystem(ns),
                Shelter = new ShelterClass(),
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = _testDir
            });
        }

        [Test]
        public void ToothDecay_SurvivesSaveAndLoad()
        {
            var writeSide = new ToothDecaySystem();
            writeSide.ContractToothache("sv_ana");

            var writer = MakeSaveSystem();
            writer.SetToothDecaySystem(writeSide);
            Assert.IsTrue(writer.Save("teeth"));

            var readSide = new ToothDecaySystem();
            var reader = MakeSaveSystem();
            reader.SetToothDecaySystem(readSide);
            Assert.IsTrue(reader.Load("teeth"));

            Assert.IsTrue(readSide.TeethMap.ContainsKey("sv_ana"),
                "A toothache does not heal because the player quit the game.");
            Assert.IsTrue(readSide.TeethMap["sv_ana"].hasToothache);
            Assert.IsTrue(readSide.TeethMap["sv_ana"].preventsSleep,
                "The gameplay consequence must come back with the affliction.");
        }

        [Test]
        public void ToothDecay_CuredBeforeSaving_StaysCured()
        {
            // The mirror case. A restore that merged into live state instead of
            // replacing it would resurrect the cured affliction.
            var writeSide = new ToothDecaySystem();
            writeSide.ContractToothache("sv_ana");
            float trauma = 0f;
            Assume.That(writeSide.PullTooth("sv_ana", true, true, ref trauma));

            var writer = MakeSaveSystem();
            writer.SetToothDecaySystem(writeSide);
            Assert.IsTrue(writer.Save("teeth"));

            var readSide = new ToothDecaySystem();
            readSide.ContractToothache("sv_ana"); // live state that the save contradicts
            var reader = MakeSaveSystem();
            reader.SetToothDecaySystem(readSide);
            Assert.IsTrue(reader.Load("teeth"));

            Assert.IsFalse(readSide.TeethMap["sv_ana"].hasToothache,
                "Loading must replace live state, not merge with it.");
        }

        [Test]
        public void EmptyState_RoundTripsAsEmpty()
        {
            var writer = MakeSaveSystem();
            writer.SetToothDecaySystem(new ToothDecaySystem());
            Assert.IsTrue(writer.Save("teeth"));

            var readSide = new ToothDecaySystem();
            readSide.ContractToothache("ghost");
            var reader = MakeSaveSystem();
            reader.SetToothDecaySystem(readSide);
            Assert.IsTrue(reader.Load("teeth"));

            Assert.IsEmpty(readSide.TeethMap,
                "Nobody had a toothache when this save was written.");
        }

        [Test]
        public void MultipleSurvivors_AllPersist()
        {
            var writeSide = new TetanusAfflictionSystem();
            var reference = new List<string> { "sv_a", "sv_b", "sv_c" };
            foreach (string id in reference) writeSide.ContractTetanus(id);

            var writer = MakeSaveSystem();
            writer.SetTetanusAfflictionSystem(writeSide);
            Assert.IsTrue(writer.Save("tetanus"));

            var readSide = new TetanusAfflictionSystem();
            var reader = MakeSaveSystem();
            reader.SetTetanusAfflictionSystem(readSide);
            Assert.IsTrue(reader.Load("tetanus"));

            CollectionAssert.AreEquivalent(reference, new List<string>(readSide.TetanusMap.Keys));
        }
    }
}
