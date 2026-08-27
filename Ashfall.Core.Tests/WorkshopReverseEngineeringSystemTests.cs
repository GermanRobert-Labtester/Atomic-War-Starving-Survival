using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WorkshopReverseEngineeringSystemTests
    {
        private static WorkshopReverseEngineeringSystem CreateSystem(
            out Inventory.Inventory inv,
            out ResearchSystem research,
            out CraftingSystem crafting)
        {
            inv = new Inventory.Inventory();
            research = new ResearchSystem();
            research.RegisterDefaults();
            crafting = new CraftingSystem(inv);
            var workshop = new WorkshopReverseEngineeringSystem(inv, research, crafting);
            workshop.BindSkillEvaluator(_ => 1.0f);
            return workshop;
        }

        private static RelicDefinition MakeRelic(string id = "test_relic",
            float repairHours = 4f, int moraleBonus = 3,
            string dismantleYield = "scrap_metal", int dismantleAmount = 2,
            string researchUnlock = "knowledge_water_basics")
        {
            return new RelicDefinition
            {
                relic_id = id,
                display_name = "Test Relic",
                description = "A test relic.",
                required_components = new List<string> { "mechanical_parts", "electronic_scrap" },
                repair_time_hours = repairHours,
                morale_bonus = moraleBonus,
                dismantle_yield_item = dismantleYield,
                dismantle_yield_amount = dismantleAmount,
                research_unlock_id = researchUnlock,
                world_flag = "relic_restored_test"
            };
        }

        // ── Examine ─────────────────────────────────────────────────────

        [Fact]
        public void Examine_ReturnsRelicInfo()
        {
            var workshop = CreateSystem(out _, out _, out _);
            workshop.RegisterRelic(MakeRelic());

            var result = workshop.Examine("test_relic");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal("workshop.examine_success", result.MessageKey);
            Assert.Equal(4, result.Deltas["repair_hours"]);
            Assert.Equal(3, result.Deltas["morale_bonus"]);
        }

        [Fact]
        public void Examine_UnknownRelic_Fails()
        {
            var workshop = CreateSystem(out _, out _, out _);
            var result = workshop.Examine("nonexistent");
            Assert.Equal(ActionResult.StatusKind.Failed, result.Status);
            Assert.Equal("unknown_relic", result.FailureCode);
        }

        // ── Dismantle ────────────────────────────────────────────────────

        [Fact]
        public void StartDismantle_BeginsJob()
        {
            var workshop = CreateSystem(out _, out _, out _);
            workshop.RegisterRelic(MakeRelic());

            var result = workshop.StartDismantle("test_relic", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.True(workshop.IsBusy);
            Assert.Equal(2, workshop.State.workPhase); // dismantling
        }

        [Fact]
        public void StartDismantle_WhenBusy_Blocks()
        {
            var workshop = CreateSystem(out _, out _, out _);
            workshop.RegisterRelic(MakeRelic());
            workshop.StartDismantle("test_relic", "survivor_1");

            var result = workshop.StartDismantle("test_relic", "survivor_2");
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("workshop_busy", result.FailureCode);
        }

        [Fact]
        public void Dismantle_CompletesAndYieldsItems()
        {
            var workshop = CreateSystem(out var inv, out _, out _);
            workshop.RegisterRelic(MakeRelic());
            workshop.StartDismantle("test_relic", "survivor_1");

            var result = workshop.TickProgress(10f); // more than enough hours
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal("workshop.dismantle_complete", result.MessageKey);
            Assert.False(workshop.IsBusy);
            Assert.True(workshop.IsRelicCompleted("test_relic"));
            Assert.Equal(2, inv.CountById("scrap_metal"));
        }

        // ── Repair ───────────────────────────────────────────────────────

        [Fact]
        public void StartRepair_WithoutComponents_Blocks()
        {
            var workshop = CreateSystem(out var inv, out _, out _);
            workshop.RegisterRelic(MakeRelic());

            var result = workshop.StartRepair("test_relic", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("missing_components", result.FailureCode);
        }

        [Fact]
        public void StartRepair_WithComponents_ReservesThem()
        {
            var workshop = CreateSystem(out var inv, out _, out _);
            workshop.RegisterRelic(MakeRelic());
            inv.AddById("mechanical_parts", 5);
            inv.AddById("electronic_scrap", 5);

            var result = workshop.StartRepair("test_relic", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.True(workshop.IsBusy);
            Assert.Equal(3, workshop.State.workPhase); // repairing
            // Components consumed from inventory
            Assert.Equal(4, inv.CountById("mechanical_parts")); // 5 - 1
            Assert.Equal(4, inv.CountById("electronic_scrap")); // 5 - 1
        }

        [Fact]
        public void StartRepair_LateComponentMissing_LeavesAllComponentsInInventory()
        {
            var workshop = CreateSystem(out var inv, out _, out _);
            workshop.RegisterRelic(MakeRelic());
            // Supply 1st component (mechanical_parts) but not 2nd (electronic_scrap)
            inv.AddById("mechanical_parts", 3);

            var result = workshop.StartRepair("test_relic", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("missing_components", result.FailureCode);
            Assert.False(workshop.IsBusy);
            // 0 mechanical_parts consumed
            Assert.Equal(3, inv.CountById("mechanical_parts"));
        }

        [Fact]
        public void Repair_CompletesAndSetsFlag()
        {
            var workshop = CreateSystem(out var inv, out _, out _);
            workshop.RegisterRelic(MakeRelic());
            inv.AddById("mechanical_parts", 5);
            inv.AddById("electronic_scrap", 5);
            workshop.StartRepair("test_relic", "survivor_1");

            var result = workshop.TickProgress(10f);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal("workshop.repair_complete", result.MessageKey);
            Assert.True(workshop.IsRelicCompleted("test_relic"));
            Assert.Equal(3, result.Deltas["morale_bonus"]);
        }

        // ── Research ─────────────────────────────────────────────────────

        [Fact]
        public void StartResearch_UnlocksKnowledgeNode()
        {
            var workshop = CreateSystem(out _, out var research, out _);
            workshop.RegisterRelic(MakeRelic(researchUnlock: "knowledge_water_basics"));

            var result = workshop.StartResearch("test_relic", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.True(workshop.IsBusy);

            workshop.TickProgress(10f);
            Assert.True(workshop.IsRelicCompleted("test_relic"));
            // The research node should be unlocked via ResearchSystem
            var node = research.GetKnowledge("knowledge_water_basics");
            Assert.NotNull(node);
        }

        [Fact]
        public void StartResearch_NoUnlockId_Blocks()
        {
            var workshop = CreateSystem(out _, out _, out _);
            workshop.RegisterRelic(MakeRelic(researchUnlock: ""));

            var result = workshop.StartResearch("test_relic", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("no_research_unlock", result.FailureCode);
        }

        // ── Progress / Cancel ────────────────────────────────────────────

        [Fact]
        public void TickProgress_ReportsProgress()
        {
            var workshop = CreateSystem(out _, out _, out _);
            workshop.RegisterRelic(MakeRelic());
            workshop.StartDismantle("test_relic", "survivor_1");

            var result = workshop.TickProgress(1f);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(1, result.Deltas["progress"]);
        }

        [Fact]
        public void CancelJob_RefundsComponents()
        {
            var workshop = CreateSystem(out var inv, out _, out _);
            workshop.RegisterRelic(MakeRelic());
            inv.AddById("mechanical_parts", 5);
            inv.AddById("electronic_scrap", 5);
            workshop.StartRepair("test_relic", "survivor_1");

            // Components consumed
            Assert.Equal(4, inv.CountById("mechanical_parts"));

            var result = workshop.CancelJob();
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal("workshop.cancelled", result.MessageKey);
            Assert.False(workshop.IsBusy);

            // Components refunded
            Assert.Equal(5, inv.CountById("mechanical_parts"));
        }

        [Fact]
        public void CancelJob_WhenIdle_Blocks()
        {
            var workshop = CreateSystem(out _, out _, out _);
            var result = workshop.CancelJob();
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("workshop_idle", result.FailureCode);
        }

        // ── Save / Load ──────────────────────────────────────────────────

        [Fact]
        public void CaptureRestoreState_PreservesCompletedRelics()
        {
            var workshop = CreateSystem(out var inv, out _, out _);
            workshop.RegisterRelic(MakeRelic());
            workshop.StartDismantle("test_relic", "survivor_1");
            workshop.TickProgress(10f);

            var state = workshop.CaptureState();
            Assert.Contains("test_relic", state.completedRelicIds);

            // Restore into a fresh system
            var workshop2 = CreateSystem(out _, out _, out _);
            workshop2.RegisterRelic(MakeRelic());
            workshop2.RestoreState(state);
            Assert.True(workshop2.IsRelicCompleted("test_relic"));
        }

        [Fact]
        public void CaptureRestoreState_PreservesActiveJob()
        {
            var workshop = CreateSystem(out _, out _, out _);
            workshop.RegisterRelic(MakeRelic());
            workshop.StartDismantle("test_relic", "survivor_1");
            workshop.TickProgress(0.5f);

            var state = workshop.CaptureState();
            Assert.Equal("test_relic", state.selectedRelicId);
            Assert.Equal(2, state.workPhase);
            Assert.True(state.progressHours > 0);
            Assert.False(state.isComplete);

            var workshop2 = CreateSystem(out _, out _, out _);
            workshop2.RegisterRelic(MakeRelic());
            workshop2.RestoreState(state);
            Assert.True(workshop2.IsBusy);
            Assert.Equal(0.5, workshop2.State.progressHours, 1);
        }

        // ── Catalog Loading ──────────────────────────────────────────────

        [Fact]
        public void LoadCatalog_PopulatesRelics()
        {
            var workshop = CreateSystem(out _, out _, out _);
            var catalog = new RelicCatalog
            {
                relics = new List<RelicDefinition>
                {
                    MakeRelic("relic_a"),
                    MakeRelic("relic_b")
                }
            };
            workshop.LoadCatalog(catalog);
            Assert.NotNull(workshop.GetRelic("relic_a"));
            Assert.NotNull(workshop.GetRelic("relic_b"));
            Assert.Null(workshop.GetRelic("nonexistent"));
        }
    }
}
