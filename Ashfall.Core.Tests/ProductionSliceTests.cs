using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ProductionSliceTests
    {
        private readonly string _dataDir;

        public ProductionSliceTests()
        {
            // Resolve path to StreamingAssets/Data
            string baseDir = AppContext.BaseDirectory;
            string candidate = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            _dataDir = Path.GetFullPath(candidate);
        }

        [Fact]
        public void AuthoritativeJson_RelicRecipes_LoadsSuccessfully()
        {
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = RelicCatalogLoader.Load(_dataDir, fileIO, serializer);
            Assert.NotNull(catalog);
            Assert.NotEmpty(catalog.relics);
            Assert.True(catalog.relics.Count >= 6, $"Expected >= 6 relics, got {catalog.relics.Count}");

            // Verify canonical gramophone and ham_radio
            var gramophone = catalog.relics.Find(r => r.relic_id == "gramophone");
            Assert.NotNull(gramophone);
            Assert.Contains("vacuum_tube", gramophone.required_components);

            var hamRadio = catalog.relics.Find(r => r.relic_id == "ham_radio");
            Assert.NotNull(hamRadio);
            Assert.Contains("soldering_kit", hamRadio.required_components);
        }

        [Fact]
        public void AuthoritativeJson_PharmaRecipes_LoadsSuccessfully()
        {
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = PharmaRecipeCatalogLoader.Load(_dataDir, fileIO, serializer);
            Assert.NotNull(catalog);
            Assert.NotEmpty(catalog.recipes);
            Assert.True(catalog.recipes.Count >= 20, $"Expected >= 20 pharma recipes, got {catalog.recipes.Count}");

            var chelation = catalog.recipes.Find(r => r.recipe_id == "recipe_edta_chelation");
            Assert.NotNull(chelation);
            Assert.Contains("chemicals", chelation.input_ids);
            Assert.Equal("item_palliative_morphine", chelation.output_item_id);
        }

        [Fact]
        public void Workshop_InsufficientStock_BlocksRepairAtomically()
        {
            var inv = new Inventory.Inventory();
            var research = new ResearchSystem();
            var crafting = new CraftingSystem(inv);
            var workshop = new WorkshopReverseEngineeringSystem(inv, research, crafting);

            var relic = new RelicDefinition
            {
                relic_id = "relic_sonar",
                display_name = "Submarine Sonar",
                required_components = new List<string> { "vacuum_tube", "sonar_crystal", "gold_wire" },
                repair_time_hours = 6f
            };
            workshop.RegisterRelic(relic);

            // Give only vacuum_tube and sonar_crystal, but missing gold_wire
            inv.AddById("vacuum_tube", 2);
            inv.AddById("sonar_crystal", 1);

            var result = workshop.StartRepair("relic_sonar", "survivor_tech");
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.False(workshop.IsBusy);

            // Verify no components were deducted
            Assert.Equal(2, inv.CountById("vacuum_tube"));
            Assert.Equal(1, inv.CountById("sonar_crystal"));
        }

        [Fact]
        public void Workshop_CancelJob_RefundsReservedComponents()
        {
            var inv = new Inventory.Inventory();
            var research = new ResearchSystem();
            var crafting = new CraftingSystem(inv);
            var workshop = new WorkshopReverseEngineeringSystem(inv, research, crafting);

            var relic = new RelicDefinition
            {
                relic_id = "relic_radio",
                display_name = "Military Radio",
                required_components = new List<string> { "vacuum_tube", "soldering_kit" },
                repair_time_hours = 8f
            };
            workshop.RegisterRelic(relic);

            inv.AddById("vacuum_tube", 3);
            inv.AddById("soldering_kit", 1);

            var startRes = workshop.StartRepair("relic_radio", "survivor_tech");
            Assert.Equal(ActionResult.StatusKind.Success, startRes.Status);
            Assert.True(workshop.IsBusy);

            // Components consumed
            Assert.Equal(2, inv.CountById("vacuum_tube"));
            Assert.Equal(0, inv.CountById("soldering_kit"));

            // Cancel
            var cancelRes = workshop.CancelJob();
            Assert.Equal(ActionResult.StatusKind.Success, cancelRes.Status);
            Assert.False(workshop.IsBusy);

            // Components refunded
            Assert.Equal(3, inv.CountById("vacuum_tube"));
            Assert.Equal(1, inv.CountById("soldering_kit"));
        }

        [Fact]
        public void Workshop_SkillMultiplier_AcceleratesRepair()
        {
            var inv = new Inventory.Inventory();
            var research = new ResearchSystem();
            var crafting = new CraftingSystem(inv);
            var workshop = new WorkshopReverseEngineeringSystem(inv, research, crafting);

            workshop.BindSkillEvaluator(id => id == "master_mechanic" ? 2.0f : 1.0f);

            var relic = new RelicDefinition
            {
                relic_id = "relic_engine",
                display_name = "Diesel Pump Engine",
                required_components = new List<string> { "machine_oil" },
                repair_time_hours = 10f
            };
            workshop.RegisterRelic(relic);

            inv.AddById("machine_oil", 1);
            workshop.StartRepair("relic_engine", "master_mechanic");

            // 10 hours / 2.0 skill multiplier = 5.0 hours required
            Assert.Equal(5.0f, workshop.State.hoursRequired);

            // Tick 5 hours -> completes
            workshop.TickProgress(5.0f);
            Assert.True(workshop.State.isComplete);
            Assert.True(workshop.IsRelicCompleted("relic_engine"));
        }

        [Fact]
        public void Workshop_ResearchBlueprint_UnlocksAndCompletesResearchNode()
        {
            var inv = new Inventory.Inventory();
            var research = new ResearchSystem();
            research.RegisterDefaults();
            var crafting = new CraftingSystem(inv);
            var workshop = new WorkshopReverseEngineeringSystem(inv, research, crafting);

            var relic = new RelicDefinition
            {
                relic_id = "relic_cipher",
                display_name = "Enigma Rotor",
                research_unlock_id = "knowledge_radio_advanced",
                repair_time_hours = 4f
            };
            workshop.RegisterRelic(relic);

            // Ensure knowledge node isn't completed yet
            var node = research.GetKnowledge("knowledge_radio_advanced");
            Assert.NotNull(node);
            Assert.False(node.isCompleted);

            workshop.StartResearch("relic_cipher", "survivor_scientist");
            workshop.TickProgress(10f);

            Assert.True(workshop.State.isComplete);
            Assert.True(node.isCompleted);
        }

        [Fact]
        public void PharmaLab_InsufficientInputs_BlocksTransactionAtomically()
        {
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(100);
            var pharma = new PharmaLabSystem(inv, rng);

            var recipe = new PharmaRecipe
            {
                recipe_id = "recipe_burn_gel",
                display_name = "Silver Burn Gel",
                input_ids = new List<string> { "chemicals", "clean_water", "silver_powder" },
                input_amounts = new List<int> { 2, 1, 1 },
                output_item_id = "item_burn_gel",
                output_amount = 2,
                base_hours = 2f
            };
            pharma.RegisterRecipe(recipe);

            inv.AddById("chemicals", 5);
            inv.AddById("clean_water", 2);
            // silver_powder is missing

            var res = pharma.StartBatch("recipe_burn_gel", "chemist_bob");
            Assert.Equal(ActionResult.StatusKind.Blocked, res.Status);
            Assert.False(pharma.IsProcessing);

            // Ensure zero inputs were consumed
            Assert.Equal(5, inv.CountById("chemicals"));
            Assert.Equal(2, inv.CountById("clean_water"));
        }

        [Fact]
        public void PharmaLab_CancelBatch_RefundsReagents()
        {
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(101);
            var pharma = new PharmaLabSystem(inv, rng);

            var recipe = new PharmaRecipe
            {
                recipe_id = "recipe_penicillin",
                display_name = "Penicillin Broth",
                input_ids = new List<string> { "mold_culture", "clean_water" },
                input_amounts = new List<int> { 1, 2 },
                output_item_id = "antibiotics",
                output_amount = 3,
                base_hours = 4f
            };
            pharma.RegisterRecipe(recipe);

            inv.AddById("mold_culture", 2);
            inv.AddById("clean_water", 5);

            var startRes = pharma.StartBatch("recipe_penicillin", "chemist_bob");
            Assert.Equal(ActionResult.StatusKind.Success, startRes.Status);
            Assert.True(pharma.IsProcessing);

            Assert.Equal(1, inv.CountById("mold_culture"));
            Assert.Equal(3, inv.CountById("clean_water"));

            pharma.CancelBatch();
            Assert.False(pharma.IsProcessing);

            // Refunded
            Assert.Equal(2, inv.CountById("mold_culture"));
            Assert.Equal(5, inv.CountById("clean_water"));
        }

        [Fact]
        public void PharmaLab_DeterministicCompletion_DeliversMedicine()
        {
            var inv1 = new Inventory.Inventory();
            var rng1 = new SeededRng(1986);
            var pharma1 = new PharmaLabSystem(inv1, rng1);

            var inv2 = new Inventory.Inventory();
            var rng2 = new SeededRng(1986);
            var pharma2 = new PharmaLabSystem(inv2, rng2);

            var recipe = new PharmaRecipe
            {
                recipe_id = "recipe_rad_flush",
                display_name = "Rad-Flush Solution",
                input_ids = new List<string> { "chemicals", "clean_water" },
                input_amounts = new List<int> { 2, 1 },
                output_item_id = "rad_away",
                output_amount = 2,
                base_hours = 3f,
                dependency_risk = 0.25f,
                purity_target = 0.95f
            };
            pharma1.RegisterRecipe(recipe);
            pharma2.RegisterRecipe(recipe);

            inv1.AddById("chemicals", 4);
            inv1.AddById("clean_water", 2);

            inv2.AddById("chemicals", 4);
            inv2.AddById("clean_water", 2);

            pharma1.StartBatch("recipe_rad_flush", "chemist_a");
            pharma2.StartBatch("recipe_rad_flush", "chemist_a");

            pharma1.TickProgress(5f);
            pharma2.TickProgress(5f);

            int delivered1 = inv1.CountById("rad_away");
            int delivered2 = inv2.CountById("rad_away");
            Assert.True(delivered1 > 0, "Expected at least 1 rad_away produced");
            Assert.Equal(delivered1, delivered2);

            Assert.Equal(pharma1.State.purity, pharma2.State.purity);
            Assert.Equal(pharma1.State.totalDependencyEvents, pharma2.State.totalDependencyEvents);
        }

        [Fact]
        public void ProductionSlice_CraftingSaveStore_AggregateRoundTrip()
        {
            var inv = new Inventory.Inventory();
            var research = new ResearchSystem();
            research.RegisterDefaults();
            var crafting = new CraftingSystem(inv);
            var workshop = new WorkshopReverseEngineeringSystem(inv, research, crafting);
            var pharma = new PharmaLabSystem(inv, new SeededRng(42));

            // Setup Workshop
            var relic = new RelicDefinition
            {
                relic_id = "relic_meter",
                display_name = "Geiger Counter",
                required_components = new List<string> { "scrap_electronic" },
                repair_time_hours = 12f
            };
            workshop.RegisterRelic(relic);
            inv.AddById("scrap_electronic", 5);
            workshop.StartRepair("relic_meter", "survivor_tech");
            workshop.TickProgress(4f); // 4 of 12 hours completed

            // Setup Pharma
            var pharmaRecipe = new PharmaRecipe
            {
                recipe_id = "recipe_antidote",
                display_name = "Toxin Antidote",
                input_ids = new List<string> { "scrap_chemical" },
                input_amounts = new List<int> { 2 },
                output_item_id = "bandage",
                output_amount = 1,
                base_hours = 6f
            };
            pharma.RegisterRecipe(pharmaRecipe);
            inv.AddById("scrap_chemical", 5);
            pharma.StartBatch("recipe_antidote", "chemist_bob");
            pharma.TickProgress(2f); // 2 of 6 hours completed

            // Capture aggregate save
            var save = crafting.CaptureState();
            save.WorkshopState = workshop.CaptureState();
            save.PharmaState = pharma.CaptureState();

            Assert.NotNull(save.WorkshopState);
            Assert.NotNull(save.PharmaState);
            Assert.Equal("relic_meter", save.WorkshopState.selectedRelicId);
            Assert.Equal(4f, save.WorkshopState.progressHours);
            Assert.Equal("recipe_antidote", save.PharmaState.currentRecipeId);
            Assert.Equal(2f, save.PharmaState.progressHours);

            // Restore into fresh systems
            var newInv = new Inventory.Inventory();
            var newCrafting = new CraftingSystem(newInv);
            var newWorkshop = new WorkshopReverseEngineeringSystem(newInv, new ResearchSystem(), newCrafting);
            var newPharma = new PharmaLabSystem(newInv, new SeededRng(42));

            newWorkshop.RegisterRelic(relic);
            newPharma.RegisterRecipe(pharmaRecipe);

            newCrafting.RestoreState(save);
            if (save.WorkshopState != null) newWorkshop.RestoreState(save.WorkshopState);
            if (save.PharmaState != null) newPharma.RestoreState(save.PharmaState);

            Assert.True(newWorkshop.IsBusy);
            Assert.Equal("relic_meter", newWorkshop.State.selectedRelicId);
            Assert.Equal(4f, newWorkshop.State.progressHours);

            Assert.True(newPharma.IsProcessing);
            Assert.Equal("recipe_antidote", newPharma.State.currentRecipeId);
            Assert.Equal(2f, newPharma.State.progressHours);
        }
    }
}
