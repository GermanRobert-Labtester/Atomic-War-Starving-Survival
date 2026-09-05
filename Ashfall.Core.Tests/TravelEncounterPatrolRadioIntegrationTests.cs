// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.Radio;

namespace Ashfall.Core.Tests
{
    public class TravelEncounterPatrolRadioIntegrationTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public TravelEncounterPatrolRadioIntegrationTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
        }

        private Inventory.Inventory CreateInventory()
        {
            var inv = new Inventory.Inventory { Capacity = 50, MaxWeight = 500f };
            inv.TryProduce("canned_food", 10);
            return inv;
        }

        [Theory]
        [InlineData("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", "radio_patrol_garrison_checkpoint")]
        [InlineData("enc_patrol_garrison_checkpoint_v2", "choice_pay_garrison_toll", "radio_patrol_garrison_checkpoint")]
        [InlineData("enc_patrol_warlord_raid", "choice_warlord_comply", "radio_patrol_warlord_raid")]
        [InlineData("enc_patrol_central_garrison_border", "choice_central_negotiate", "radio_patrol_border_closed")]
        [InlineData("enc_patrol_railway_convoy", "choice_guild_join_escort", "radio_patrol_convoy_attacked")]
        [InlineData("enc_patrol_warlord_press_gang", "choice_press_ignore", "radio_patrol_press_gang")]
        public void EncounterResolution_QueuesMappedRadioSignal(string encounterId, string choiceId, string expectedRadioId)
        {
            var inv = CreateInventory();
            var travelSys = new TravelEncounterSystem(_catalog, inv);
            var radioHooks = new PatrolRadioHooks();
            radioHooks.Subscribe(travelSys);

            bool ok = travelSys.ResolveChoice(encounterId, choiceId, 1, out _);
            Assert.True(ok, $"Failed to resolve choice {choiceId} on {encounterId}");

            Assert.Equal(1, radioHooks.PendingCount);
            Assert.Contains(expectedRadioId, radioHooks.PendingSignals);

            radioHooks.Unsubscribe();
        }

        [Fact]
        public void OneShotSemantics_DoesNotDuplicateQueuedOrConsumedSignal()
        {
            var inv = CreateInventory();
            var travelSys = new TravelEncounterSystem(_catalog, inv);
            var radioHooks = new PatrolRadioHooks();
            radioHooks.Subscribe(travelSys);

            // First resolution queues signal
            travelSys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out _);
            Assert.Equal(1, radioHooks.PendingCount);

            // Re-queuing same signal while pending is rejected
            bool queuedAgain = radioHooks.QueueSignal("radio_patrol_garrison_checkpoint");
            Assert.False(queuedAgain);
            Assert.Equal(1, radioHooks.PendingCount);

            // Dispatch and consume
            var dispatched = radioHooks.TickRadio();
            Assert.Single(dispatched);
            Assert.Equal("radio_patrol_garrison_checkpoint", dispatched[0]);
            Assert.Equal(0, radioHooks.PendingCount);
            Assert.Contains("radio_patrol_garrison_checkpoint", radioHooks.ConsumedSignals);

            // Resolving v2 after cooldown expires does NOT re-emit consumed signal
            travelSys.ResolveChoice("enc_patrol_garrison_checkpoint_v2", "choice_pay_garrison_toll", 10, out _);
            Assert.Equal(0, radioHooks.PendingCount);
        }

        [Fact]
        public void FactionRadioCapability_EnforcesLoreBoundary()
        {
            // Capable factions
            Assert.True(PatrolRadioHooks.IsFactionRadioCapable("military_remnants"));
            Assert.True(PatrolRadioHooks.IsFactionRadioCapable("iron_garrison"));
            Assert.True(PatrolRadioHooks.IsFactionRadioCapable("upland_militia"));
            Assert.True(PatrolRadioHooks.IsFactionRadioCapable("faction_central_garrison"));
            Assert.True(PatrolRadioHooks.IsFactionRadioCapable("faction_railway_guild"));

            // Non-capable factions
            Assert.False(PatrolRadioHooks.IsFactionRadioCapable("faction_scavengers"));
            Assert.False(PatrolRadioHooks.IsFactionRadioCapable("cult_of_ash_sign"));
            Assert.False(PatrolRadioHooks.IsFactionRadioCapable("cult_of_the_glow"));
            Assert.False(PatrolRadioHooks.IsFactionRadioCapable("warlords_sector_4"));
            Assert.False(PatrolRadioHooks.IsFactionRadioCapable("unknown_faction"));
            Assert.False(PatrolRadioHooks.IsFactionRadioCapable(string.Empty));
        }

        [Fact]
        public void StatePersistence_RoundTrip_PreservesPendingAndConsumed()
        {
            var hooks1 = new PatrolRadioHooks();
            hooks1.QueueSignal("radio_patrol_garrison_checkpoint");
            hooks1.QueueSignal("radio_patrol_warlord_raid");

            // Consume first signal
            var firstBatch = hooks1.TickRadio();
            Assert.Equal(2, firstBatch.Count);

            // Queue a second signal that remains pending
            hooks1.QueueSignal("radio_patrol_border_closed");
            Assert.Equal(1, hooks1.PendingCount);

            // Capture state
            var state = hooks1.CaptureState();
            Assert.Equal(2, state.ConsumedSignals.Count);
            Assert.Single(state.PendingSignals);

            // Restore into new hooks
            var hooks2 = new PatrolRadioHooks();
            hooks2.RestoreState(state);

            Assert.Equal(2, hooks2.ConsumedSignals.Count);
            Assert.Contains("radio_patrol_garrison_checkpoint", hooks2.ConsumedSignals);
            Assert.Contains("radio_patrol_warlord_raid", hooks2.ConsumedSignals);
            Assert.Single(hooks2.PendingSignals);
            Assert.Contains("radio_patrol_border_closed", hooks2.PendingSignals);

            // Cannot re-queue consumed signals
            Assert.False(hooks2.QueueSignal("radio_patrol_garrison_checkpoint"));

            // Dequeue remaining pending
            var secondBatch = hooks2.TickRadio();
            Assert.Single(secondBatch);
            Assert.Equal("radio_patrol_border_closed", secondBatch[0]);
            Assert.Equal(0, hooks2.PendingCount);
            Assert.Equal(3, hooks2.ConsumedSignals.Count);
        }

        [Fact]
        public void AuthoredCorpus_HasAll5BroadcastDefinitions()
        {
            string corpusPath = Path.Combine(_dataDir, "faction_radio_corpus.json");
            Assert.True(File.Exists(corpusPath));

            using var doc = JsonDocument.Parse(File.ReadAllText(corpusPath));
            var root = doc.RootElement;
            var broadcasts = root.GetProperty("broadcasts").EnumerateArray().ToList();

            string[] expectedIds = new[]
            {
                "radio_patrol_garrison_checkpoint",
                "radio_patrol_warlord_raid",
                "radio_patrol_border_closed",
                "radio_patrol_convoy_attacked",
                "radio_patrol_press_gang"
            };

            foreach (var id in expectedIds)
            {
                var match = broadcasts.FirstOrDefault(b => b.GetProperty("id").GetString() == id);
                Assert.True(match.ValueKind != JsonValueKind.Undefined, $"Broadcast {id} not found in corpus.");

                Assert.Equal("patrol_report", match.GetProperty("type").GetString());
                string faction = match.GetProperty("faction_id").GetString()!;
                Assert.True(PatrolRadioHooks.IsFactionRadioCapable(faction), $"Origin faction {faction} must be radio-capable.");
                Assert.True(match.GetProperty("frequency_mhz").GetDouble() > 0);
                Assert.False(string.IsNullOrWhiteSpace(match.GetProperty("message").GetString()));
            }
        }
    }
}
