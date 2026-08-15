using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class CensusClaimSystemTests
    {
        [Test]
        public void LevyCapsAtThreeNamedIds()
        {
            var census = new CensusClaimSystem();
            var ids = new List<string> { "a", "b", "c", "d", "e" };
            Assert.IsTrue(census.IssueLevy(ids, day: 40));
            Assert.AreEqual(CensusClaimSystem.MaxLevyCount, census.ActiveLevy.survivorIds.Length);
            Assert.AreEqual("a", census.ActiveLevy.survivorIds[0]);
            Assert.AreEqual("c", census.ActiveLevy.survivorIds[2]);
        }

        [Test]
        public void HonourSetsFlagAndAssignsAway()
        {
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "sv_one", "sv_two", "sv_three" }, 10);
            Assert.IsTrue(census.HonourLevy());
            Assert.IsTrue(census.LevyHonour);
            Assert.IsFalse(census.LevyRefuse);
            Assert.IsTrue(census.IsAssignedAway("sv_two"));
            Assert.AreEqual(3, census.AssignedAwayIds().Count);
        }

        [Test]
        public void SubstituteReplacesNamesStillMaxThree()
        {
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "a", "b", "c" }, 10);
            Assert.IsTrue(census.SubstituteLevy(new[] { "x", "y", "z", "extra" }));
            Assert.IsTrue(census.LevySubstitute);
            Assert.AreEqual(3, census.ActiveLevy.survivorIds.Length);
            Assert.AreEqual("x", census.ActiveLevy.survivorIds[0]);
            Assert.IsFalse(census.IsAssignedAway("a"));
            Assert.IsTrue(census.IsAssignedAway("y"));
        }

        [Test]
        public void RefuseSetsWaitFlagWithoutKidnappingRoster()
        {
            var census = new CensusClaimSystem();
            census.UpsertLedger("home_one", "One", "caretaker", listed: false);
            census.UpsertLedger("home_two", "Two", "clerk", listed: false);
            census.UpsertLedger("home_three", "Three", "vet", listed: false);
            census.UpsertLedger("home_four", "Four", "lamp", listed: false);
            census.IssueLevy(new[] { "home_one", "home_two", "home_three" }, 20);
            Assert.IsTrue(census.RefuseLevy(21));
            Assert.IsTrue(census.LevyRefuse);
            Assert.IsTrue(census.State.edorWaitingAtHatch);
            Assert.IsFalse(census.IsAssignedAway("home_one"));
            Assert.IsFalse(census.IsAssignedAway("home_four"));
        }

        [Test]
        public void SaveRoundtripPreservesLevyFlags()
        {
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "n1", "n2", "n3" }, 5);
            census.HonourLevy();
            var json = JsonUtility.ToJson(census.CaptureState());
            var restored = new CensusClaimSystem();
            restored.RestoreState(JsonUtility.FromJson<CensusClaimSystemState>(json));
            Assert.IsTrue(restored.LevyHonour);
            Assert.AreEqual(3, restored.ActiveLevy.survivorIds.Length);
            Assert.IsTrue(restored.IsAssignedAway("n2"));
        }

        [Test]
        public void CannotIssueSecondLevyWhileActive()
        {
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "a", "b" }, 1);
            census.HonourLevy();
            Assert.IsFalse(census.IssueLevy(new[] { "c" }, 2));
        }
    }
}
