// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Narrative;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    public class PatrolBountyHandoffTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public PatrolBountyHandoffTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
        }

        [Theory]
        [InlineData(-5, FactionBountySeverity.None)]
        [InlineData(-9, FactionBountySeverity.None)]
        [InlineData(-10, FactionBountySeverity.Moderate)]
        [InlineData(-12, FactionBountySeverity.Moderate)]
        [InlineData(-14, FactionBountySeverity.Moderate)]
        [InlineData(-15, FactionBountySeverity.Severe)]
        [InlineData(-17, FactionBountySeverity.Severe)]
        [InlineData(-19, FactionBountySeverity.Severe)]
        [InlineData(-20, FactionBountySeverity.Extreme)]
        [InlineData(-25, FactionBountySeverity.Extreme)]
        public void SeverityCalculation_MapsToThresholds(int standingDelta, FactionBountySeverity expected)
        {
            var severity = FactionBountySystem.CalculateSeverity(standingDelta);
            Assert.Equal(expected, severity);
        }

        [Fact]
        public void SeverePatrolViolation_TriggersBountyHandoff_WithCorrectProvenance()
        {
            var bountySys = new FactionBountySystem();
            var warSys = new FactionWarSystem();
            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: warSys, bountySystem: bountySys);

            var warlordRaid = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            Assert.NotNull(warlordRaid);
            // choice_warlord_fight has faction_standing_delta = -15
            var fightChoice = warlordRaid.Choices.Find(c => c.ChoiceId == "choice_warlord_fight")!;
            Assert.NotNull(fightChoice);
            Assert.Equal(-15, fightChoice.FactionStandingDelta);

            FactionBountyRecord? eventFiredRecord = null;
            bountySys.OnBountyIssued += b => eventFiredRecord = b;

            bool resolved = sys.ResolveChoice(warlordRaid.Id, fightChoice.ChoiceId, currentDay: 12, out var result);
            Assert.True(resolved);
            Assert.NotNull(result);
            Assert.NotNull(result.BountyRecord);
            Assert.Same(result.BountyRecord, eventFiredRecord);

            var bounty = result.BountyRecord;
            Assert.Equal("faction_scavenger_warlords", bounty.FactionId);
            Assert.Equal(-15, bounty.AuthoredStandingDelta);
            Assert.Equal(FactionBountySeverity.Severe, bounty.Severity);
            Assert.Equal(FactionBountyState.Active, bounty.State);
            Assert.Equal(12, bounty.IssuedDay);

            Assert.Equal("patrol_violation", bounty.Provenance.SourceType);
            Assert.Equal("enc_patrol_warlord_raid", bounty.Provenance.EncounterId);
            Assert.Equal("choice_warlord_fight", bounty.Provenance.ChoiceId);
            Assert.Equal("enc_patrol_warlord_raid:choice_warlord_fight:12", bounty.Provenance.SourceResolutionId);
            Assert.Equal(12, bounty.Provenance.Day);

            Assert.True(bountySys.HasActiveBounty("faction_scavenger_warlords"));
        }

        [Fact]
        public void NonSevereViolation_DoesNotTriggerBounty()
        {
            var bountySys = new FactionBountySystem();
            var warSys = new FactionWarSystem();
            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: warSys, bountySystem: bountySys);

            var warlordRaid = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            // choice_warlord_flee has delta = -5 (below the -10 threshold)
            var fleeChoice = warlordRaid.Choices.Find(c => c.ChoiceId == "choice_warlord_flee")!;
            Assert.NotNull(fleeChoice);
            Assert.Equal(-5, fleeChoice.FactionStandingDelta);

            bool resolved = sys.ResolveChoice(warlordRaid.Id, fleeChoice.ChoiceId, currentDay: 12, out var result);
            Assert.True(resolved);
            Assert.NotNull(result);
            Assert.Null(result.BountyRecord);
            Assert.False(bountySys.HasActiveBounty("warlords_sector_4"));
            Assert.Empty(bountySys.AllBounties);
        }

        [Fact]
        public void Deduplication_BySourceResolutionId_PreventsDuplicates()
        {
            var bountySys = new FactionBountySystem();

            var b1 = bountySys.IssuePatrolBounty("warlords_sector_4", "enc_test", "choice_fight", -15, 10);
            var b2 = bountySys.IssuePatrolBounty("warlords_sector_4", "enc_test", "choice_fight", -15, 10);

            Assert.NotNull(b1);
            Assert.NotNull(b2);
            Assert.Same(b1, b2);
            Assert.Single(bountySys.AllBounties);
        }

        [Fact]
        public void CanonicalClearance_ResolveAndClearForFaction()
        {
            var bountySys = new FactionBountySystem();

            var b1 = bountySys.IssuePatrolBounty("warlords_sector_4", "enc_1", "choice_fight", -10, 5)!;
            var b2 = bountySys.IssuePatrolBounty("warlords_sector_4", "enc_2", "choice_fight", -15, 6)!;
            var b3 = bountySys.IssuePatrolBounty("faction_central_garrison", "enc_3", "choice_attack", -20, 7)!;

            Assert.Equal(3, bountySys.GetActiveBounties().Count);

            // Resolve b1 individually
            bool resolved1 = bountySys.ResolveBounty(b1.BountyId, day: 8);
            Assert.True(resolved1);
            Assert.Equal(FactionBountyState.Resolved, b1.State);
            Assert.Equal(8, b1.ResolvedDay);
            Assert.Equal(2, bountySys.GetActiveBounties().Count);

            // Clear all remaining for warlords_sector_4
            int cleared = bountySys.ClearBountiesForFaction("warlords_sector_4", day: 9);
            Assert.Equal(1, cleared); // only b2 was active
            Assert.Equal(FactionBountyState.Resolved, b2.State);
            Assert.False(bountySys.HasActiveBounty("warlords_sector_4"));

            // Garrison bounty still active
            Assert.True(bountySys.HasActiveBounty("faction_central_garrison"));
            Assert.Single(bountySys.GetActiveBounties());
        }

        [Fact]
        public void SaveLoad_Idempotency_PreservesBountyRecords()
        {
            var bountySys = new FactionBountySystem();
            bountySys.IssuePatrolBounty("warlords_sector_4", "enc_1", "choice_fight", -15, 10);
            bountySys.IssuePatrolBounty("faction_ash_sign", "enc_2", "choice_raid", -20, 12);

            var state = bountySys.CaptureState();
            Assert.Equal(2, state.Bounties.Count);

            var newBountySys = new FactionBountySystem();
            newBountySys.RestoreState(state);

            Assert.Equal(2, newBountySys.AllBounties.Count);
            Assert.True(newBountySys.HasActiveBounty("warlords_sector_4"));
            Assert.True(newBountySys.HasActiveBounty("faction_ash_sign"));

            var b1 = newBountySys.AllBounties[0];
            Assert.Equal("faction_scavenger_warlords", b1.FactionId);
            Assert.Equal(FactionBountySeverity.Severe, b1.Severity);
            Assert.Equal(10, b1.Provenance.Day);
        }

        [Fact]
        public void AuthoredStandingDelta_Evaluated_EvenIfStandingClamped()
        {
            var bountySys = new FactionBountySystem();
            var warSys = new FactionWarSystem();
            // Force standing to minimum (-100)
            warSys.ModifyStanding("warlords_sector_4", -100);
            Assert.Equal(-100, warSys.GetStanding("warlords_sector_4"));

            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: warSys, bountySystem: bountySys);
            var warlordRaid = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            var fightChoice = warlordRaid.Choices.Find(c => c.ChoiceId == "choice_warlord_fight")!;

            bool ok = sys.ResolveChoice(warlordRaid.Id, fightChoice.ChoiceId, 25, out var res);
            Assert.True(ok);
            Assert.NotNull(res?.BountyRecord);
            // Even though net standing remained clamped at -100, authored delta -15 successfully issued Severe bounty
            Assert.Equal(-15, res!.BountyRecord!.AuthoredStandingDelta);
            Assert.Equal(FactionBountySeverity.Severe, res.BountyRecord.Severity);
            Assert.Equal(-100, warSys.GetStanding("warlords_sector_4"));
        }
    }
}
