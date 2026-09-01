using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Flags;
using Ashfall.Core.Factions;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CampaignConsequenceLedgerTests
    {
        [Fact]
        public void Normalization_EnforcesOrdinalLowercase()
        {
            var ledger = new CampaignConsequenceLedger();

            ledger.Set("FLAG_FACTION_MILITARY_JOINED", "military", "oath_sworn", 5, "survivor_1");
            Assert.True(ledger.IsSet("flag_faction_military_joined"));
            Assert.True(ledger.IsSet("  FLAG_FACTION_MILITARY_JOINED  "));

            ledger.Increment("COUNTER_CRIME_COMMITTED", 2, "tribunal", "theft", 5);
            Assert.Equal(2, ledger.GetCounter("counter_crime_committed"));
            Assert.Equal(2, ledger.GetCounter("  COUNTER_CRIME_COMMITTED  "));
        }

        [Fact]
        public void ConsequenceRecord_CapturesCausalityAndProvenance()
        {
            var ledger = new CampaignConsequenceLedger();
            ConsequenceRecord? lastRecorded = null;
            ledger.OnConsequenceRecorded += rec => lastRecorded = rec;

            ledger.Set("flag_verdict_signed", "verdict", "tribunal_ruling", day: 12, subjectId: "survivor_daniels");

            Assert.NotNull(lastRecorded);
            Assert.Equal("flag_verdict_signed", lastRecorded.key);
            Assert.Equal("flag", lastRecorded.kind);
            Assert.Equal(1, lastRecorded.value);
            Assert.Equal("verdict", lastRecorded.originSystem);
            Assert.Equal("tribunal_ruling", lastRecorded.sourceEvent);
            Assert.Equal(12, lastRecorded.day);
            Assert.Equal("survivor_daniels", lastRecorded.subjectId);

            var history = ledger.GetHistory();
            Assert.Single(history);
            Assert.Equal("flag_verdict_signed", history[0].key);
        }

        [Fact]
        public void CrossSystemVisibility_FlagSetInOneSystemIsImmediatelyVisibleInOthers()
        {
            var unifiedLedger = new CampaignConsequenceLedger();

            // Set a flag from MoralChoiceSystem
            var moralChoice = new MoralChoiceSystem(new SeededRng(12345), flags: unifiedLedger);
            moralChoice.SetFlag("flag_spared_raider");

            Assert.True(unifiedLedger.IsSet("flag_spared_raider"));

            // Check visibility from Faction coordinator with same ledger
            var coordinator = new FactionBranchCoordinator(flags: unifiedLedger);
            Assert.NotNull(coordinator.Military.State.setFlags); // durable flags
            Assert.True(unifiedLedger.IsSet("flag_spared_raider"));

            // Set faction PoNR flag via coordinator
            unifiedLedger.Set(MilitaryBranchIds.FlagPonrLoyalSoldier, "military", "ponr_locked", 14);
            Assert.True(unifiedLedger.IsSet("flag_branch_mil_1_ponr"));
            Assert.True(moralChoice.HasFlag("flag_branch_mil_1_ponr"));
        }

        [Fact]
        public void LegacyImport_ReconcilesFlagsAndCountersWithoutDuplication()
        {
            var ledger = new CampaignConsequenceLedger();
            ledger.Set("flag_known_flag", "core", "initial", 1);
            ledger.SetCounter("counter_kills", 3, "combat", "skirmish", 1);

            var oldFactionsFlags = new List<string> { "flag_known_flag", "flag_new_rebel_secret", "FLAG_MILITARY_PACT" };
            int flagsImported = ledger.ImportLegacyFlags(oldFactionsFlags, "factions_legacy", 10);

            Assert.Equal(2, flagsImported);
            Assert.True(ledger.IsSet("flag_known_flag"));
            Assert.True(ledger.IsSet("flag_new_rebel_secret"));
            Assert.True(ledger.IsSet("flag_military_pact"));

            var oldCounters = new Dictionary<string, int>
            {
                { "counter_kills", 5 }, // higher than 3
                { "counter_scavenges", 10 }
            };
            int countersImported = ledger.ImportLegacyCounters(oldCounters, "scavenge_legacy", 10);

            Assert.Equal(2, countersImported);
            Assert.Equal(5, ledger.GetCounter("counter_kills"));
            Assert.Equal(10, ledger.GetCounter("counter_scavenges"));
        }

        [Fact]
        public void HistoryQueries_FilterBySystemAndSubject()
        {
            var ledger = new CampaignConsequenceLedger();
            ledger.Set("flag_treated_infection", "medical", "surgery", 3, "survivor_alice");
            ledger.Set("flag_treated_trauma", "medical", "counseling", 4, "survivor_bob");
            ledger.Set("flag_verdict_closed", "verdict", "trial", 4, "survivor_alice");

            var medicalHistory = ledger.GetHistoryForSystem("medical");
            Assert.Equal(2, medicalHistory.Count);

            var aliceHistory = ledger.GetHistoryForSubject("survivor_alice");
            Assert.Equal(2, aliceHistory.Count);
            Assert.Contains(aliceHistory, h => h.key == "flag_treated_infection");
            Assert.Contains(aliceHistory, h => h.key == "flag_verdict_closed");
        }

        [Fact]
        public void SaveLoadRoundTrip_PreservesFlagsCountersAndHistory()
        {
            var original = new CampaignConsequenceLedger();
            original.Set("flag_bunker_sealed", "shelter", "seal_hatch", 1);
            original.Set("flag_generator_fixed", "engineering", "repair", 2);
            original.SetCounter("counter_days_survived", 15, "calendar", "tick", 15);

            var saveState = original.CaptureState();
            Assert.Equal(1, saveState.schemaVersion);
            Assert.Equal(2, saveState.flags.Count);
            Assert.Single(saveState.counters);
            Assert.Equal(3, saveState.history.Count);

            var restored = new CampaignConsequenceLedger();
            restored.RestoreState(saveState);

            Assert.True(restored.IsSet("flag_bunker_sealed"));
            Assert.True(restored.IsSet("flag_generator_fixed"));
            Assert.Equal(15, restored.GetCounter("counter_days_survived"));
            Assert.Equal(3, restored.GetHistory().Count);
        }

        [Fact]
        public void Clear_RemovesSpecificFlagWithoutAffectingOthers()
        {
            var ledger = new CampaignConsequenceLedger();
            ledger.Set("flag_lockdown", "security", "alarm", 1);
            ledger.Set("flag_power_on", "grid", "switch", 1);

            Assert.True(ledger.IsSet("flag_lockdown"));
            Assert.True(ledger.IsSet("flag_power_on"));

            ledger.Clear("flag_lockdown");
            Assert.False(ledger.IsSet("flag_lockdown"));
            Assert.True(ledger.IsSet("flag_power_on"));
        }
    }
}
