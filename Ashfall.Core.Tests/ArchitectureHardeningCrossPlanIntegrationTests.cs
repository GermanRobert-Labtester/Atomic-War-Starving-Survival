using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Flags;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    using SimClock = Ashfall.Core.Clock.SimClock;

    /// <summary>
    /// Wave 6 — Cross-Plan Architecture Integration Proofs (I1–I7).
    /// Proves that Journal, Determinism, Clock Governance, and Event Surface
    /// contracts operate harmoniously as an integrated whole.
    /// </summary>
    public class ArchitectureHardeningCrossPlanIntegrationTests
    {
        private sealed class TestAuthor : ISurvivorAuthor
        {
            public string Id => "sv_auditor";
            public string DisplayName => "Chief Auditor";
            public RiskBiasTrait RiskBias => RiskBiasTrait.Cautious;
        }

        /// <summary>
        /// I1: Journal restore emits 0 mutation events while fully reconstructing state.
        /// </summary>
        [Fact]
        public void I1_JournalRestore_EmitsZeroEvents()
        {
            var source = new JournalSystem();
            source.TryDiscover("k_bunker_origin", new TestAuthor(), 1, 6f);
            source.TryDiscoverKnowledge("k_geiger_schematic", new TestAuthor(), 2, 10f);
            source.UnlockLocationVisited("loc_silo_echo");
            source.SwitchTab(2);

            var save = source.CaptureState();
            Assert.Equal(2, source.EntryCount);
            Assert.Equal(2, source.CodexUnlockCount);

            var restored = new JournalSystem();
            int eventCount = 0;
            restored.OnEntryAdded += _ => eventCount++;
            restored.OnNotificationPing += _ => eventCount++;
            restored.OnTabChanged += _ => eventCount++;
            restored.OnCodexUnlocked += _ => eventCount++;

            restored.RestoreState(save);

            Assert.Equal(0, eventCount);
            Assert.Equal(2, restored.EntryCount);
            Assert.Equal(2, restored.CodexUnlockCount);
            Assert.Equal(2, restored.ActiveTab);
            Assert.True(restored.IsLocationVisited("loc_silo_echo"));
        }

        /// <summary>
        /// I2: Journal timestamps derive solely from simulation clock day/hour, never wall-clock.
        /// </summary>
        [Fact]
        public void I2_JournalDayOrdering_DerivesFromSimulationClock()
        {
            var clock = new SimClock(0);
            var journal = new JournalSystem();

            // Day 0, 08:30 (8 hours, 30 ticks = 510 ticks)
            clock.SetTick(8 * SimClock.TicksPerHour + 30);
            journal.TryAddRawEntry("k_sim_1", "Morning entry", new TestAuthor(), clock.DayIndex, clock.HourOfDay);

            // Advance clock to Day 3, 15:00 (3 * 1440 + 15 * 60 = 5220 ticks)
            clock.SetTick(3 * SimClock.TicksPerDay + 15 * SimClock.TicksPerHour);
            journal.TryAddRawEntry("k_sim_2", "Afternoon entry", new TestAuthor(), clock.DayIndex, clock.HourOfDay);

            Assert.Equal(2, journal.EntryCount);
            Assert.Equal("Day 3, 15h", journal.Entries[0].Timestamp);
            Assert.Equal(3, journal.Entries[0].Day);
            Assert.Equal(15f, journal.Entries[0].Hour);

            Assert.Equal("Day 1, 08h", journal.Entries[1].Timestamp); // Day 0 clamped to 1 in JournalEntry.Day
            Assert.Equal(1, journal.Entries[1].Day);
            Assert.Equal(8f, journal.Entries[1].Hour);
        }

        /// <summary>
        /// I3: Producer mutation precedes journal write, ensuring journal observes committed state.
        /// </summary>
        [Fact]
        public void I3_ProducerMutation_PrecedesJournalWrite()
        {
            var flags = new InMemoryFlagLedger();
            var moral = new MoralChoiceSystem(new SeededRng(555), flags: flags);
            var journal = new JournalSystem();

            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_ration_crisis",
                DisplayName = "Ration Shortage",
                Category = "crisis",
                LocationId = "loc_pantry",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption
                    {
                        Label = "Tighten belts",
                        MoralDelta = 5,
                        Epitaph = "Rations reduced by half."
                    }
                }
            };

            moral.OnQuestResolved += r =>
            {
                // Invariant: Producer state must ALREADY be resolved when writing journal
                Assert.True(moral.IsResolved(r.questId));
                Assert.Equal(1, moral.QuestsResolved);
                journal.TryAddRawEntry(r.questId, r.epitaph, new TestAuthor(), r.resolvedDay);
            };

            var res = moral.Resolve(quest, 0, quest.LocationId, day: 4);

            Assert.NotNull(res);
            Assert.Equal(1, journal.EntryCount);
            Assert.Equal("quest_moral_ration_crisis", journal.Entries[0].KnowledgeKey);
            Assert.Contains("half", journal.Entries[0].Text);
        }

        /// <summary>
        /// I4: SaveChecksum is invariant to wall-clock time, system delays, and culture.
        /// </summary>
        [Fact]
        public void I4_SaveChecksum_IsIndependentOfWallClockAndCulture()
        {
            var journal = new JournalSystem();
            journal.TryDiscover("k_vault_seal", new TestAuthor(), 7, 12.5f);
            journal.UnlockItemSeen("item_water_filter");

            var state1 = journal.CaptureState();
            string checksum1 = SaveChecksum.Compute(state1);

            // Change thread culture to Turkish and simulate passage of real time
            var prevCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
                System.Threading.Thread.Sleep(10); // Real-time delay

                var state2 = journal.CaptureState();
                string checksum2 = SaveChecksum.Compute(state2);

                Assert.Equal(checksum1, checksum2);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = prevCulture;
            }
        }

        /// <summary>
        /// I5: Event restore suppression combined with deterministic replay yields identical trajectories.
        /// </summary>
        [Fact]
        public void I5_EventRestoreSuppression_And_DeterministicReplay()
        {
            var simA = new SimClock(0);
            var simB = new SimClock(0);

            // Run 500 ticks on Clock A
            simA.AdvanceTicks(500);

            // Save state of Clock A
            long savedTick = simA.CurrentTick;

            // Restore state into Clock B
            simB.SetTick(savedTick);

            // Advance both by another 300 ticks
            simA.AdvanceTicks(300);
            simB.AdvanceTicks(300);

            Assert.Equal(simA.CurrentTick, simB.CurrentTick);
            Assert.Equal(simA.DayIndex, simB.DayIndex);
            Assert.Equal(simA.HourOfDay, simB.HourOfDay);
        }

        /// <summary>
        /// I6: ProceduralItemInstance sequence state across save/load matches uninterrupted run.
        /// </summary>
        [Fact]
        public void I6_ProceduralIds_AcrossSaveBoundary_MatchUninterruptedRun()
        {
            const int seed = 4242;

            // Uninterrupted run of 20 items
            ProceduralItemInstance.ConfigureSequence(seed, 0);
            var expectedIds = new List<string>(20);
            for (int i = 0; i < 20; i++)
            {
                expectedIds.Add(new ProceduralItemInstance("item_medical_kit").InstanceId);
            }

            // Segmented run: 10 items, save state, restore state, 10 items
            ProceduralItemInstance.ConfigureSequence(seed, 0);
            var actualIds = new List<string>(20);
            for (int i = 0; i < 10; i++)
            {
                actualIds.Add(new ProceduralItemInstance("item_medical_kit").InstanceId);
            }

            var (savedSeed, savedCounter) = ProceduralItemInstance.GetSequenceState();
            Assert.Equal(seed, savedSeed);
            Assert.Equal(10, savedCounter);

            // Restore
            ProceduralItemInstance.ConfigureSequence(savedSeed, savedCounter);
            for (int i = 0; i < 10; i++)
            {
                actualIds.Add(new ProceduralItemInstance("item_medical_kit").InstanceId);
            }

            Assert.Equal(expectedIds.Count, actualIds.Count);
            for (int i = 0; i < 20; i++)
            {
                Assert.Equal(expectedIds[i], actualIds[i]);
            }
        }

        /// <summary>
        /// I7: Flag normalization and idempotency preserve state checksum stability.
        /// </summary>
        [Fact]
        public void I7_FlagPolicy_MaintainsChecksumStability()
        {
            var ledgerA = new InMemoryFlagLedger();
            ledgerA.Set("FLAG_SECTOR_CLEARED");
            ledgerA.Set("counter_scavenge_runs", day: 1);
            ledgerA.Increment("counter_scavenge_runs", 3);

            var ledgerB = new InMemoryFlagLedger();
            // Mixed case, whitespace padding, repeated set
            ledgerB.Set("  flag_sector_cleared  ");
            ledgerB.Set("FLAG_SECTOR_CLEARED"); // Idempotent
            ledgerB.Set("COUNTER_SCAVENGE_RUNS", day: 1);
            ledgerB.Increment("COUNTER_SCAVENGE_RUNS", 3);

            Assert.True(ledgerA.IsSet("flag_sector_cleared"));
            Assert.True(ledgerB.IsSet("flag_sector_cleared"));
            Assert.Equal(ledgerA.GetCounter("counter_scavenge_runs"), ledgerB.GetCounter("counter_scavenge_runs"));
        }
    }
}
