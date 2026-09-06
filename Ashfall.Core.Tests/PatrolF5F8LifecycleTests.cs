// SPDX-License-Identifier: MIT
// ASHFALL Patrol F5-F8 lifecycle, recognition, and presentation coverage.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    public sealed class PatrolF5F8LifecycleTests
    {
        private readonly TravelEncounterCatalog _catalog;

        public PatrolF5F8LifecycleTests()
        {
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(dataDir))
            {
                dataDir = Path.GetFullPath(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _catalog = TravelEncounterCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
        }

        private static Inventory.Inventory MakeInventory(int food = 10)
        {
            var inventory = new Inventory.Inventory { Capacity = 50, MaxWeight = 500f };
            if (food > 0) inventory.TryProduce("canned_food", food);
            inventory.TryProduce("sealed_government_document", 1);
            return inventory;
        }

        [Fact]
        public void GarrisonTrustLifecycle_PersistsCooldownHistoryAndChangedChoices()
        {
            var inventory = MakeInventory();
            var factionWar = new FactionWarSystem();
            var system = new TravelEncounterSystem(_catalog, inventory, factionWar);
            int historyEvents = 0;
            system.OnPatrolHistoryRecorded += (_, _) => historyEvents++;

            Assert.True(system.ResolveChoice(
                "enc_patrol_garrison_checkpoint",
                "choice_pay_garrison_toll",
                10,
                out var first));
            Assert.NotNull(first);
            Assert.Equal(8, inventory.CountById("canned_food"));
            Assert.Equal(1, factionWar.GetStanding("faction_central_garrison"));
            Assert.Equal(13, system.GetCooldownExpiry("patrol_garrison_checkpoint"));
            Assert.Equal(1, system.GetPatrolChainStage("iron_garrison", "patrol_garrison_trust"));
            Assert.Equal(1, historyEvents);

            var recognition = system.GetRecognitionContext("iron_garrison", "enc_patrol_garrison_checkpoint", "patrol_garrison_trust");
            Assert.True(recognition.HasChoice("choice_pay_garrison_toll"));
            Assert.True(recognition.HasTag("paid_toll"));
            Assert.Equal("faction_central_garrison", recognition.FactionId);

            var saved = system.CaptureState();
            string json = new SystemTextJsonSerializer().Serialize(saved);
            var restoredState = new SystemTextJsonSerializer().Deserialize<TravelEncounterState>(json);
            Assert.NotNull(restoredState);

            var restored = new TravelEncounterSystem(_catalog, inventory, factionWar);
            int restoredHistoryEvents = 0;
            restored.OnPatrolHistoryRecorded += (_, _) => restoredHistoryEvents++;
            restored.RestoreState(restoredState);
            Assert.Equal(13, restored.GetCooldownExpiry("patrol_garrison_checkpoint"));
            Assert.Equal(1, restored.GetPatrolChainStage("iron_garrison", "patrol_garrison_trust"));
            Assert.Equal(1, restored.GetRecognitionContext("iron_garrison").PaidTollCount);
            Assert.False(restored.IsEncounterEligible(
                _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!,
                "high_scarp", 1f, "all", 12));
            Assert.True(restored.IsEncounterEligible(
                _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!,
                "high_scarp", 1f, "all", 13));

            var stageOneView = restored.BuildPatrolPresentation("enc_patrol_garrison_checkpoint");
            Assert.NotNull(stageOneView);
            var reduced = stageOneView!.Choices.Single(c => c.ChoiceId == "choice_garrison_reduced_toll");
            var free = stageOneView.Choices.Single(c => c.ChoiceId == "choice_garrison_free_passage");
            Assert.True(reduced.IsAvailable);
            Assert.False(free.IsAvailable);

            Assert.True(restored.ResolveChoice(
                "enc_patrol_garrison_checkpoint_v2",
                "choice_garrison_reduced_toll",
                13,
                out var second));
            Assert.NotNull(second);
            Assert.Equal(7, inventory.CountById("canned_food"));
            Assert.Equal(2, factionWar.GetStanding("faction_central_garrison"));
            Assert.Equal(2, restored.GetPatrolChainStage("iron_garrison", "patrol_garrison_trust"));
            Assert.Equal(2, restored.GetRecognitionContext("iron_garrison").EncountersWithFaction);
            Assert.Equal(1, historyEvents);
            Assert.Equal(1, restoredHistoryEvents);

            var trustedView = restored.BuildPatrolPresentation("enc_patrol_garrison_checkpoint_v3");
            Assert.NotNull(trustedView);
            Assert.Equal("Trusted regular", trustedView!.RecognitionLabel);
            Assert.True(trustedView.Choices.Single(c => c.ChoiceId == "choice_garrison_free_passage").IsAvailable);
        }

        [Fact]
        public void WarlordHostilityMemory_IsFactionScopedAndChangesFutureWeight()
        {
            var factionWar = new FactionWarSystem();
            var system = new TravelEncounterSystem(_catalog, MakeInventory(20), factionWar);
            var raid = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            var garrison = _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!;

            float before = system.GetEffectiveWeight(raid, "Balanced");
            Assert.True(system.ResolveChoice("enc_patrol_warlord_raid", "choice_warlord_fight", 20, out _));
            float after = system.GetEffectiveWeight(raid, "Balanced");

            Assert.True(after > before);
            Assert.True(system.GetRecognitionContext("warlords_sector_4").HasTag("hostile"));
            Assert.False(system.GetRecognitionContext("iron_garrison").HasTag("hostile"));
            Assert.Equal(0, system.GetRecognitionContext("iron_garrison").EncountersWithFaction);
            Assert.Equal(7, raid.GetCooldownDays());
            Assert.Equal(3, garrison.GetCooldownDays());
        }

        [Fact]
        public void PatrolPresentation_UsesCoreAvailabilityAndExplicitReasonCodes()
        {
            var inventory = MakeInventory(1);
            var system = new TravelEncounterSystem(_catalog, inventory, new FactionWarSystem());
            var presentation = system.BuildPatrolPresentation("enc_patrol_garrison_checkpoint");

            Assert.NotNull(presentation);
            Assert.Equal("faction_central_garrison", presentation!.FactionId);
            Assert.Equal("checkpoint", presentation.PatrolArchetype);
            Assert.Equal("No prior contact", presentation.RecognitionLabel);

            var toll = presentation.Choices.Single(c => c.ChoiceId == "choice_pay_garrison_toll");
            Assert.False(toll.IsAvailable);
            Assert.Equal("cost_unavailable", toll.DisabledReasonCode);
            Assert.Single(toll.Costs);
            Assert.Equal("canned_food", toll.Costs[0].ItemId);
            Assert.Equal(2, toll.Costs[0].Quantity);

            var reduced = presentation.Choices.Single(c => c.ChoiceId == "choice_garrison_reduced_toll");
            Assert.False(reduced.IsAvailable);
            Assert.Equal("recognition_requirement_unmet", reduced.DisabledReasonCode);
        }

        [Fact]
        public void PatrolCooldownAndStance_AreDefinitionDriven()
        {
            var system = new TravelEncounterSystem(_catalog);
            var checkpoint = _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!;
            var raid = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            var pressGang = _catalog.GetEncounter("enc_patrol_warlord_press_gang")!;

            Assert.Equal(3, checkpoint.GetCooldownDays());
            Assert.Equal(7, raid.GetCooldownDays());
            Assert.Equal(10, pressGang.GetCooldownDays());
            Assert.Equal(5, new TravelEncounterDefinition { Id = "legacy" }.GetCooldownDays());
            Assert.True(system.GetEffectiveWeight(checkpoint, "Rapid") < system.GetEffectiveWeight(checkpoint, "Balanced"));
            Assert.True(system.GetEffectiveWeight(checkpoint, "Cautious") > system.GetEffectiveWeight(checkpoint, "Balanced"));
            Assert.True(system.GetEffectiveWeight(raid, "Aggressive") > system.GetEffectiveWeight(raid, "Cautious"));
        }

        [Fact]
        public void FailedPatrolCost_IsAtomicAndDoesNotRecordRecognition()
        {
            var factionWar = new FactionWarSystem();
            var inventory = MakeInventory(1);
            var system = new TravelEncounterSystem(_catalog, inventory, factionWar);

            Assert.False(system.ResolveChoice(
                "enc_patrol_garrison_checkpoint",
                "choice_pay_garrison_toll",
                10,
                out _));
            Assert.Equal(1, inventory.CountById("canned_food"));
            Assert.Equal(0, factionWar.GetStanding("faction_central_garrison"));
            Assert.Equal(0, system.GetPatrolChainStage("iron_garrison", "patrol_garrison_trust"));
            Assert.Empty(system.CaptureState().PatrolHistory);
            Assert.Equal(0, system.GetCooldownExpiry("patrol_garrison_checkpoint"));
        }

        [Fact]
        public void HeadlessDemo_CoversCatalogAndLifecycleThroughProductionPath()
        {
            var report = TravelEncounterHeadlessDemo.Run(null, NullLog.Instance);

            Assert.True(report.Passed,
                $"Patrol headless demo failed: {report.Summary}; " +
                string.Join(", ", report.Checks.Where(c => !c.Passed).Select(c => c.Name)));
            Assert.Equal(57, report.EncounterCount);
            Assert.Equal(21, report.PatrolCount);
            Assert.Equal(13, report.RestoredCooldownDay);
            Assert.Equal(1, report.RestoredChainStage);
        }
    }
}
