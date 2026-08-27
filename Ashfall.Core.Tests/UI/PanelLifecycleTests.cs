using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public class PanelLifecycleTests
    {
        [Fact]
        public void ResearchPanel_Lifecycle_RebindAndStateContinuity()
        {
            var research = new ResearchSystem();
            research.RegisterDefaults();

            // Unlock and start research
            research.UnlockManual("knowledge_water_basics");
            bool started = research.StartResearch("knowledge_water_basics", day: 1);
            Assert.True(started);
            research.Tick(newDay: 2);

            Assert.True(research.IsManualUnlocked("knowledge_water_basics"));
            Assert.Equal(1, research.State.activeResearchDays);

            // Simulate Close -> Reopen with rebind
            var active = research.GetActiveResearch();
            Assert.NotNull(active);
            Assert.Equal("knowledge_water_basics", active.id);

            // Advance further and complete
            research.Tick(newDay: 10);
            Assert.Contains("knowledge_water_basics", research.State.completedIds);
        }

        [Fact]
        public void JournalPanel_Lifecycle_EventDeduplicationAndCleanState()
        {
            var journal = new JournalSystem();
            int entryAddCount = 0;
            Action<JournalEntry> listener = _ => entryAddCount++;

            // First bind
            journal.OnEntryAdded += listener;
            var author = new MockAuthor("survivor_1", RiskBiasTrait.Realist);
            var entry1 = journal.TryDiscover("flag_water_pipe_broken", author, day: 1);
            Assert.NotNull(entry1);
            Assert.Equal(1, entryAddCount);

            // Simulate Unbind -> Rebind
            journal.OnEntryAdded -= listener;
            journal.OnEntryAdded += listener;

            var entry2 = journal.TryDiscover("flag_water_pipe_repaired", author, day: 2);
            Assert.NotNull(entry2);
            Assert.Equal(2, entryAddCount); // Exactly 2, not 3 (no duplicate callbacks)
            Assert.Equal(2, journal.Entries.Count);
        }

        [Fact]
        public void WeatherPanel_Lifecycle_WeatherChangedCallbackDeduplication()
        {
            var weather = new WeatherSystem();
            int weatherChangeCount = 0;
            Action<WeatherKind> listener = _ => weatherChangeCount++;

            // Initial bind
            weather.OnWeatherChanged += listener;
            weather.ForceWeather(WeatherKind.FalloutStorm);
            Assert.Equal(1, weatherChangeCount);

            // Simulate Unbind -> Rebind
            weather.OnWeatherChanged -= listener;
            weather.OnWeatherChanged += listener;

            weather.ForceWeather(WeatherKind.BlackRain);
            Assert.Equal(2, weatherChangeCount); // Exactly 2, not 3
            Assert.Equal(WeatherKind.BlackRain, weather.Current);
        }

        [Fact]
        public void ExpeditionPanel_Lifecycle_ExpeditionCompletedCallbackDeduplication()
        {
            var system = new ExpeditionSystem();
            int completionCount = 0;
            Action<ExpeditionState> listener = _ => completionCount++;
            var rng = new SeededRng(42);

            var def = new ExpeditionDefinition
            {
                id = "loc_depot",
                displayName = "Supply Depot",
                dangerLevel = 1,
                distanceTicks = 1,
                encounterChancePerTick = 0f
            };

            // Initial bind
            system.OnExpeditionCompleted += listener;
            bool started = system.Start(def, "survivor_1", day: 1);
            Assert.True(started);

            // Tick through Outbound -> Looting -> Inbound -> Completed
            for (int i = 0; i < 20 && system.ActiveCount > 0; i++)
            {
                system.TickHours(1.0f, rng);
            }
            Assert.Equal(1, completionCount);

            // Simulate Unbind -> Rebind
            system.OnExpeditionCompleted -= listener;
            system.OnExpeditionCompleted += listener;

            bool started2 = system.Start(def, "survivor_2", day: 2);
            Assert.True(started2);
            for (int i = 0; i < 20 && system.ActiveCount > 0; i++)
            {
                system.TickHours(1.0f, rng);
            }
            Assert.Equal(2, completionCount); // Exactly 2, not 3
        }

        private class MockAuthor : ISurvivorAuthor
        {
            public string Id { get; }
            public string DisplayName { get; }
            public RiskBiasTrait RiskBias { get; }

            public MockAuthor(string id, RiskBiasTrait riskBias)
            {
                Id = id;
                DisplayName = id;
                RiskBias = riskBias;
            }
        }
    }
}
