using System.Collections.Generic;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Port of the Unity CensusClaimSystemTests (NUnit → xUnit) plus core-idiomatic
    /// extras. The six original scenarios are kept verbatim; JSON roundtrip now
    /// goes through the IJsonSerializer port instead of JsonUtility.
    /// </summary>
    public class CensusClaimSystemTests
    {
        [Fact]
        public void LevyCapsAtThreeNamedIds()
        {
            var census = new CensusClaimSystem();
            var ids = new List<string> { "a", "b", "c", "d", "e" };
            Assert.True(census.IssueLevy(ids, day: 40));
            Assert.Equal(CensusClaimSystem.MaxLevyCount, census.ActiveLevy.survivorIds.Length);
            Assert.Equal("a", census.ActiveLevy.survivorIds[0]);
            Assert.Equal("c", census.ActiveLevy.survivorIds[2]);
        }

        [Fact]
        public void HonourSetsFlagAndAssignsAway()
        {
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "sv_one", "sv_two", "sv_three" }, 10);
            Assert.True(census.HonourLevy());
            Assert.True(census.LevyHonour);
            Assert.False(census.LevyRefuse);
            Assert.True(census.IsAssignedAway("sv_two"));
            Assert.Equal(3, census.AssignedAwayIds().Count);
        }

        [Fact]
        public void SubstituteReplacesNamesStillMaxThree()
        {
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "a", "b", "c" }, 10);
            Assert.True(census.SubstituteLevy(new[] { "x", "y", "z", "extra" }));
            Assert.True(census.LevySubstitute);
            Assert.Equal(3, census.ActiveLevy.survivorIds.Length);
            Assert.Equal("x", census.ActiveLevy.survivorIds[0]);
            Assert.False(census.IsAssignedAway("a"));
            Assert.True(census.IsAssignedAway("y"));
        }

        [Fact]
        public void RefuseSetsWaitFlagWithoutKidnappingRoster()
        {
            var census = new CensusClaimSystem();
            census.UpsertLedger("home_one", "One", "caretaker", listed: false);
            census.UpsertLedger("home_two", "Two", "clerk", listed: false);
            census.UpsertLedger("home_three", "Three", "vet", listed: false);
            census.UpsertLedger("home_four", "Four", "lamp", listed: false);
            census.IssueLevy(new[] { "home_one", "home_two", "home_three" }, 20);
            Assert.True(census.RefuseLevy(21));
            Assert.True(census.LevyRefuse);
            Assert.True(census.State.edorWaitingAtHatch);
            Assert.False(census.IsAssignedAway("home_one"));
            Assert.False(census.IsAssignedAway("home_four"));
        }

        [Fact]
        public void SaveRoundtripPreservesLevyFlags()
        {
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "n1", "n2", "n3" }, 5);
            census.HonourLevy();
            var json = new SystemTextJsonSerializer();
            var restored = new CensusClaimSystem();
            restored.RestoreState(json.Deserialize<CensusClaimSystemState>(json.Serialize(census.CaptureState())));
            Assert.True(restored.LevyHonour);
            Assert.Equal(3, restored.ActiveLevy.survivorIds.Length);
            Assert.True(restored.IsAssignedAway("n2"));
        }

        [Fact]
        public void CannotIssueSecondLevyWhileActive()
        {
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "a", "b" }, 1);
            census.HonourLevy();
            Assert.False(census.IssueLevy(new[] { "c" }, 2));
        }

        // -----------------------------------------------------------------
        // Core-idiomatic extras (behaviour not covered by the Unity suite)
        // -----------------------------------------------------------------

        [Fact]
        public void TickDailyExpiresActiveLevyAndClearsAssignments()
        {
            var census = new CensusClaimSystem();
            census.IssueLevy(new[] { "a", "b", "c" }, day: 10, durationDays: 3);
            census.HonourLevy();
            Assert.True(census.ActiveLevy.active);
            census.TickDaily(11);
            census.TickDaily(12);
            Assert.True(census.ActiveLevy.active);
            census.TickDaily(13);
            Assert.False(census.ActiveLevy.active);
            Assert.False(census.IsAssignedAway("a"));
            Assert.Empty(census.AssignedAwayIds());
        }

        [Fact]
        public void Activate12CIsIdempotentAndRaisesEvent()
        {
            var census = new CensusClaimSystem();
            int fired = 0;
            census.On12CActivated += () => fired++;
            census.Activate12C();
            census.Activate12C();
            Assert.True(census.Order12CActive);
            Assert.Equal(1, fired);
        }

        [Fact]
        public void AdjustOfficeTrustClampsToHundred()
        {
            var census = new CensusClaimSystem();
            census.AdjustOfficeTrust(-1000f);
            Assert.Equal(-100f, census.State.officeTrust);
            census.AdjustOfficeTrust(1000f);
            Assert.Equal(100f, census.State.officeTrust);
        }

        [Fact]
        public void CorrectOccupationOnlyTouchesListedRows()
        {
            var census = new CensusClaimSystem();
            census.UpsertLedger("sv_one", "One", "caretaker", listed: true);
            census.CorrectOccupation("sv_one", "lamp");
            census.CorrectOccupation("sv_unknown", "clerk");
            Assert.Equal("lamp", census.State.ledger[0].occupationObserved);
            Assert.Single(census.State.ledger);
        }

        [Fact]
        public void RestoreStateIsIdempotent()
        {
            var census = new CensusClaimSystem();
            census.UpsertLedger("sv_one", "One", "caretaker", listed: false);
            census.RefuseLevy(5);
            var saved = census.CaptureState();
            var restored = new CensusClaimSystem();
            restored.RestoreState(saved);
            restored.RestoreState(saved);
            Assert.Single(restored.State.ledger);
            Assert.True(restored.LevyRefuse);
        }

        [Fact]
        public void StateChangedEventFiresOnEveryMutation()
        {
            var census = new CensusClaimSystem();
            int changed = 0;
            census.OnStateChanged += _ => changed++;
            census.UpsertLedger("sv_one", "One", "clerk", listed: false);
            census.IssueLevy(new[] { "sv_one" }, 3);
            census.HonourLevy();
            census.AdjustOfficeTrust(1f);
            Assert.True(changed >= 4);
        }
    }

    public class CensusHeadlessDemoTests
    {
        [Fact]
        public void HeadlessDemoPasses()
        {
            var report = CensusHeadlessDemo.Run();
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
            Assert.True(report.Checks.Count >= 15);
        }
    }
}
