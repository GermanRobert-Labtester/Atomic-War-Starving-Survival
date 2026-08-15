using NUnit.Framework;
using AtomicWar._Game.Data;
using System.Collections.Generic;

namespace AtomicWar.Tests.EditMode
{
    // Lore bible 04_ENCOUNTERS II-b — the runtime echoes.json pipeline.
    [TestFixture]
    public class EchoCatalogLoaderTests
    {
        private static List<AtomicWar._Game.Events.GameEvent> Load() => EchoCatalogLoader.Load();

        [Test]
        public void LoadsAllTwentyThreeEchoes()
        {
            var echoes = Load();
            Assert.AreEqual(23, echoes.Count,
                "echoes.json should hold the 15 authored echoes + the 8 bible echoes");
        }

        [Test]
        public void AllIdsUnique()
        {
            var echoes = Load();
            var set = new HashSet<string>();
            for (int i = 0; i < echoes.Count; i++)
                Assert.IsTrue(set.Add(echoes[i].id), $"duplicate echo id {echoes[i].id}");
        }

        [Test]
        public void EightBibleEchoesPresent()
        {
            var set = new HashSet<string>();
            foreach (var e in Load()) set.Add(e.id);
            string[] bible = {
                "echo_the_nameplates", "echo_unopened_boots", "echo_the_frying_pan",
                "echo_school_register", "echo_dosimeter_pilgrim", "echo_the_receipt",
                "echo_answering_service", "echo_the_second_chalk_count"
            };
            foreach (var id in bible)
                Assert.IsTrue(set.Contains(id), $"missing bible echo {id}");
        }

        [Test]
        public void NameplatesMeltChoiceCarriesBrassAndFlag()
        {
            var nameplates = EchoCatalogLoader.Load().Find(e => e.id == "echo_the_nameplates");
            Assert.IsNotNull(nameplates);
            Assert.AreEqual(3, nameplates.choices.Count);

            var melt = nameplates.choices.Find(c => c.ChoiceId == "melt_them");
            Assert.IsNotNull(melt);
            Assert.AreEqual(-20f, melt.MoraleDelta, 0.001f);

            bool hasFlag = false, hasBrass = false;
            foreach (var fx in melt.Effects)
            {
                if (fx.SetWorldFlag == "nameplates_melted" && fx.WorldFlagValue) hasFlag = true;
                if (fx.ItemId == "brass_fittings" && fx.ItemAmount == 14) hasBrass = true;
            }
            Assert.IsTrue(hasFlag, "melt_them must set the nameplates_melted flag (checked at the arrival)");
            Assert.IsTrue(hasBrass, "melt_them must yield the fourteen brass nameplates as brass_fittings");
        }

        [Test]
        public void MountChoiceSetsHungFlagAtMoraleCost()
        {
            var nameplates = EchoCatalogLoader.Load().Find(e => e.id == "echo_the_nameplates");
            var mount = nameplates.choices.Find(c => c.ChoiceId == "mount_them");
            Assert.IsNotNull(mount);
            Assert.AreEqual(-8f, mount.MoraleDelta, 0.001f);
            bool hasFlag = false;
            foreach (var fx in mount.Effects)
                if (fx.SetWorldFlag == "nameplates_hung" && fx.WorldFlagValue) hasFlag = true;
            Assert.IsTrue(hasFlag, "mount_them must set the permanent nameplates_hung flag");
        }

        [Test]
        public void LegacyEchoesKeepScarredStateGate()
        {
            var answering = EchoCatalogLoader.Load().Find(e => e.id == "echo_answering_machine");
            Assert.IsNotNull(answering);
            Assert.AreEqual("scarred_state", answering.conditions.RequiredFlagId);
            Assert.AreEqual(31, answering.conditions.MinDay);
        }

        [Test]
        public void ConditionsMinDayFallsBackToTopLevel()
        {
            // Bible echoes author both top-level minDay and conditions.MinDay;
            // loader prefers the nested value.
            var boots = EchoCatalogLoader.Load().Find(e => e.id == "echo_unopened_boots");
            Assert.IsNotNull(boots);
            Assert.AreEqual(5, boots.conditions.MinDay);
        }
    }
}
