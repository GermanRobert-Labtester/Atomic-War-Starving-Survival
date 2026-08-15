using NUnit.Framework;
using System.Collections.Generic;
using AtomicWar._Game.Core;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// MISC-005 follow-up. The seeded-rng work replaced wall-clock Random with
    /// salted streams so a save replays identically, but every "no rng was
    /// injected" call site built its stream with <see cref="SeededRandom.CreateFixed"/>,
    /// which returns a *fresh* Random. Called once per roll that re-seeds
    /// identically every time; cached in a process-static field it never rewinds
    /// between campaigns. <see cref="SeededRandom.Stream"/> is the fallback that
    /// does both correctly, so these are the properties it must hold.
    /// </summary>
    [TestFixture]
    public class SeededRandomStreamTests
    {
        [TearDown]
        public void ResetGlobalSeed()
        {
            // WorldSeed is process-global; leaving it set would leak into every
            // later fixture. Assigning also clears the stream registry.
            SeededRandom.WorldSeed = -1;
        }

        [Test]
        public void Stream_AdvancesAcrossCalls_RatherThanRestarting()
        {
            int first = SeededRandom.Stream("ctx_advance").Next();
            int second = SeededRandom.Stream("ctx_advance").Next();

            Assert.AreNotEqual(first, second,
                "Repeated fallback rolls must advance one stream, not re-seed it.");
        }

        [Test]
        public void CreateFixed_RestartsEveryCall_WhichIsWhyStreamExists()
        {
            // Pins the distinction the call sites depend on: CreateFixed is only
            // correct where the context itself is the key (one stream per node).
            int first = SeededRandom.CreateFixed("ctx_restart").Next();
            int second = SeededRandom.CreateFixed("ctx_restart").Next();

            Assert.AreEqual(first, second);
        }

        [Test]
        public void Stream_DivergesByContext()
        {
            int a = SeededRandom.Stream("ctx_a").Next();
            int b = SeededRandom.Stream("ctx_b").Next();

            Assert.AreNotEqual(a, b, "Different contexts must be independent streams.");
        }

        [Test]
        public void ResetStreams_RewindsToPositionZero()
        {
            int first = SeededRandom.Stream("ctx_rewind").Next();
            SeededRandom.Stream("ctx_rewind").Next();

            SeededRandom.ResetStreams();

            Assert.AreEqual(first, SeededRandom.Stream("ctx_rewind").Next(),
                "A restored save must replay the same rolls, not resume mid-stream.");
        }

        [Test]
        public void AssigningWorldSeed_RewindsStreams()
        {
            SeededRandom.WorldSeed = 7;
            int first = SeededRandom.Stream("ctx_seeded").Next();
            SeededRandom.Stream("ctx_seeded").Next();

            SeededRandom.WorldSeed = 7;

            Assert.AreEqual(first, SeededRandom.Stream("ctx_seeded").Next(),
                "Starting a run re-seeds the fallback streams even on the same seed.");
        }

        [Test]
        public void WorldSeed_SaltsTheStream()
        {
            SeededRandom.WorldSeed = 7;
            int withSeven = SeededRandom.Stream("ctx_salt").Next();

            SeededRandom.WorldSeed = 8;
            int withEight = SeededRandom.Stream("ctx_salt").Next();

            Assert.AreNotEqual(withSeven, withEight,
                "Two campaigns must not share the fallback sequence.");
        }

        /// <summary>
        /// SEED-002: different world seeds must reach a representative gameplay
        /// system (System_BloodTypes) with a different RNG sequence.
        /// </summary>
        [Test]
        public void WorldSeed_SaltsRepresentativeSystem_BloodTypes()
        {
            var a = GetBloodTypeSequence(42);
            var b = GetBloodTypeSequence(99);

            Assert.AreNotEqual(string.Join(",", a), string.Join(",", b),
                "Different world seeds should produce different System_BloodTypes RNG sequences.");
        }

        private static List<string> GetBloodTypeSequence(int seed)
        {
            SeededRandom.WorldSeed = seed;
            var sys = new System_BloodTypes();
            var seq = new List<string>();
            for (int i = 0; i < 8; i++)
                seq.Add(sys.EnsureBloodType($"sv_{i}"));
            return seq;
        }
    }
}
