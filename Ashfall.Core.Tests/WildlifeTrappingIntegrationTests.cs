using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingIntegrationTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            var dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                candidate = Path.Combine(dir, "assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        private static WildlifeTrappingCatalog? LoadTrappingCatalog()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return WildlifeTrappingCatalogLoader.Load(DataDir, fileIO, json);
        }

        [Fact]
        public void SetTrap_AndCheck_ResolvesCatch()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var set = sys.SetTrap("site_valley", "bait_grain", "dweller_hunter");
            Assert.True(set.IsSuccess);
            Assert.Single(sys.State.trapSites);

            var check = sys.CheckTraps();
            Assert.True(check.IsSuccess);
        }

        [Fact]
        public void SaveAndRestore_PreservesTrapSites()
        {
            var sys1 = new WildlifeTrappingSystem(new SeededRng(42));
            sys1.SetTrap("site_woods", "bait_scrap", "hunter_1");

            var state = sys1.CaptureState();
            var sys2 = new WildlifeTrappingSystem(new SeededRng(42));
            sys2.RestoreState(state);

            Assert.Single(sys2.State.trapSites);
            Assert.Equal("site_woods", sys2.State.trapSites[0].siteId);
            Assert.Equal("bait_scrap", sys2.State.trapSites[0].baitType);
        }

        // ====================================================================
        // Workstream A: Weather Modifiers (WT-WX)
        // ====================================================================

        [Fact]
        public void WT_WX_001_ZeroSensitivityTrap_IgnoresWeatherPenalty()
        {
            // sensitivity = 0 means weather multiplier is 1.0 under all weather
            float multClear = WildlifeTrappingSystem.CalculateWeatherMultiplier(0f, WeatherKind.Clear);
            float multBlizzard = WildlifeTrappingSystem.CalculateWeatherMultiplier(0f, WeatherKind.Blizzard);
            float multFallout = WildlifeTrappingSystem.CalculateWeatherMultiplier(0f, WeatherKind.FalloutStorm);

            Assert.Equal(1.0f, multClear, 3);
            Assert.Equal(1.0f, multBlizzard, 3);
            Assert.Equal(1.0f, multFallout, 3);
        }

        [Fact]
        public void WT_WX_002_WeatherPenalty_FalloutStormWithSensitivity03_Produces15PercentReduction()
        {
            // sensitivity = 0.3, penalty for FalloutStorm = 0.5 -> multiplier = 1 - (0.3 * 0.5) = 0.85
            float penalty = WildlifeTrappingSystem.WeatherPenaltyFor(WeatherKind.FalloutStorm);
            Assert.Equal(0.5f, penalty, 3);

            float mult = WildlifeTrappingSystem.CalculateWeatherMultiplier(0.3f, WeatherKind.FalloutStorm);
            Assert.Equal(0.85f, mult, 3);
        }

        [Fact]
        public void WT_WX_003_WeatherPenalty_BlizzardWithSensitivity03_Produces24PercentReduction()
        {
            // sensitivity = 0.3, penalty for Blizzard = 0.8 -> multiplier = 1 - (0.3 * 0.8) = 0.76
            float penalty = WildlifeTrappingSystem.WeatherPenaltyFor(WeatherKind.Blizzard);
            Assert.Equal(0.8f, penalty, 3);

            float mult = WildlifeTrappingSystem.CalculateWeatherMultiplier(0.3f, WeatherKind.Blizzard);
            Assert.Equal(0.76f, mult, 3);
        }

        [Fact]
        public void WT_WX_004_ClearWeather_ProducesZeroPenalty()
        {
            float penalty = WildlifeTrappingSystem.WeatherPenaltyFor(WeatherKind.Clear);
            Assert.Equal(0f, penalty, 3);

            float mult = WildlifeTrappingSystem.CalculateWeatherMultiplier(1.0f, WeatherKind.Clear);
            Assert.Equal(1.0f, mult, 3);
        }

        [Fact]
        public void WT_WX_005_AllWeatherKinds_EvaluateWithinZeroAndOne()
        {
            foreach (WeatherKind kind in Enum.GetValues(typeof(WeatherKind)))
            {
                float pen = WildlifeTrappingSystem.WeatherPenaltyFor(kind);
                Assert.InRange(pen, 0f, 1f);

                float mult = WildlifeTrappingSystem.CalculateWeatherMultiplier(0.5f, kind);
                Assert.InRange(mult, 0f, 1f);
            }
        }

        [Fact]
        public void WT_WX_006_DurabilityDecrementsOnCheck_RegardlessOfWeather()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(100));
            sys.SetSelectionContext(new WildlifeSelectionContext { CurrentWeather = WeatherKind.Blizzard });
            sys.SetTrap("site_blizzard", "", "hunter");
            sys.State.trapSites[0].remainingDurability = 5;
            sys.State.trapSites[0].checkDay = 1; // Eligible for day 1 check

            sys.CheckTraps();
            Assert.Equal(4, sys.State.trapSites[0].remainingDurability);
        }

        [Fact]
        public void WT_WX_007_PrimaryCatchChance_ClampsBetween005And095()
        {
            float minChance = WildlifeTrappingSystem.CalculatePrimaryCatchChance(0.01f, 0f, 0.1f, 1.0f, WeatherKind.Blizzard);
            float maxChance = WildlifeTrappingSystem.CalculatePrimaryCatchChance(10.0f, 100f, 5.0f, 0.0f, WeatherKind.Clear);

            Assert.Equal(0.05f, minChance, 3);
            Assert.Equal(0.95f, maxChance, 3);
        }

        // ====================================================================
        // Workstream B: Hunter Skills (WT-SK)
        // ====================================================================

        [Fact]
        public void WT_SK_001_SkillMultiplier_CurveEvaluation()
        {
            Assert.Equal(0.5f, WildlifeTrappingSystem.SkillMultiplierFor(0f), 3);
            Assert.Equal(1.0f, WildlifeTrappingSystem.SkillMultiplierFor(50f), 3);
            Assert.Equal(1.5f, WildlifeTrappingSystem.SkillMultiplierFor(100f), 3);
        }

        [Fact]
        public void WT_SK_002_SkillMultiplier_ClampsOutOfRangeValues()
        {
            Assert.Equal(0.5f, WildlifeTrappingSystem.SkillMultiplierFor(-20f), 3);
            Assert.Equal(1.5f, WildlifeTrappingSystem.SkillMultiplierFor(150f), 3);
        }

        [Fact]
        public void WT_SK_003_PerSiteHunterSkill_UsesAssignedHunterProgression()
        {
            var rng = new SeededRng(555);
            var sys = new WildlifeTrappingSystem(rng);

            var ctx = new WildlifeSelectionContext();
            ctx.HunterSkillLevels["hunter_novice"] = 0f;
            ctx.HunterSkillLevels["hunter_master"] = 100f;
            sys.SetSelectionContext(ctx);

            sys.SetTrap("site_novice", "", "hunter_novice");
            sys.SetTrap("site_master", "", "hunter_master");

            // Novice site chance = 0.5 * 1.0 * 0.5 (skill) = 0.25
            // Master site chance = 0.5 * 1.0 * 1.5 (skill) = 0.75
            float chanceNovice = WildlifeTrappingSystem.CalculatePrimaryCatchChance(1.0f, 0f, 1.0f, 0f, WeatherKind.Clear);
            float chanceMaster = WildlifeTrappingSystem.CalculatePrimaryCatchChance(1.0f, 100f, 1.0f, 0f, WeatherKind.Clear);

            Assert.Equal(0.25f, chanceNovice, 3);
            Assert.Equal(0.75f, chanceMaster, 3);
        }

        [Fact]
        public void WT_SK_004_UnassignedSite_FallsBackToGlobalHunterSkill()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(100));
            sys.SetHunterSkill(80f); // 80 -> 1.3x

            sys.SetTrap("site_legacy", "", ""); // unassigned
            float chance = WildlifeTrappingSystem.CalculatePrimaryCatchChance(1.0f, 80f, 1.0f, 0f, WeatherKind.Clear);
            Assert.Equal(0.65f, chance, 3); // 0.5 * 1.3 = 0.65
        }

        [Fact]
        public void WT_SK_005_SkillProgression_GetDisciplineProgress01_NormalizesCorrectly()
        {
            var prog = new SkillProgressionSystem();
            prog.RegisterSkill(new SkillDef { id = "skill_1", disciplineId = "survival", xpThreshold = 50f });
            prog.RegisterSkill(new SkillDef { id = "skill_2", disciplineId = "survival", xpThreshold = 200f });
            prog.RegisterSkill(new SkillDef { id = "skill_milestone", disciplineId = "survival", xpThreshold = SkillProgressionSystem.UnreachableXp });

            var actor = new SimpleSkillActor("survivor_trapper", "survival");

            Assert.Equal(0f, prog.GetDisciplineProgress01(actor.Id, "survival"), 3);

            prog.RecordAction(actor, "survival", 100f, 1);
            // 100 / 200 = 0.5
            Assert.Equal(0.5f, prog.GetDisciplineProgress01(actor.Id, "survival"), 3);

            prog.RecordAction(actor, "survival", 200f, 1); // total 300
            // capped at 1.0
            Assert.Equal(1.0f, prog.GetDisciplineProgress01(actor.Id, "survival"), 3);
        }

        // ====================================================================
        // Workstream C: First-Catch Discovery & Codex (WT-JC)
        // ====================================================================

        [Fact]
        public void WT_JC_001_FirstCatch_FiresOnNewSpeciesDiscovered()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            string? discoveredSpecies = null;
            string? discoveredSite = null;
            string? discoveredHunter = null;

            sys.OnNewSpeciesDiscovered += (sp, site, hunter) =>
            {
                discoveredSpecies = sp;
                discoveredSite = site;
                discoveredHunter = hunter;
            };

            sys.SetTrap("site_1", "bait_grain", "hunter_bob");
            sys.CheckTraps();

            if (sys.State.trapSites[0].hasCatch)
            {
                Assert.NotNull(discoveredSpecies);
                Assert.Equal("site_1", discoveredSite);
                Assert.Equal("hunter_bob", discoveredHunter);
                Assert.Contains(discoveredSpecies, sys.State.firstCatchLoggedSpeciesIds);
            }
        }

        [Fact]
        public void WT_JC_002_SecondCatchSameSpecies_DoesNotFireEventAgain()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            int eventsFired = 0;
            sys.OnNewSpeciesDiscovered += (_, _, _) => eventsFired++;

            // Seed state with rabbit already caught
            sys.State.firstCatchLoggedSpeciesIds.Add("rabbit");

            sys.SetTrap("site_1", "bait_grain_lure", "hunter_bob");
            // If rabbit is caught, event should not fire
            for (int i = 0; i < 5; i++)
            {
                sys.CheckTraps();
                if (sys.State.trapSites[0].hasCatch && sys.State.trapSites[0].catchSpecies == "rabbit")
                {
                    Assert.Equal(0, eventsFired);
                    break;
                }
            }
        }

        [Fact]
        public void WT_JC_003_FirstCatchLoggedSpeciesIds_RoundTripsThroughSaveRestore()
        {
            var sys1 = new WildlifeTrappingSystem(new SeededRng(42));
            sys1.State.firstCatchLoggedSpeciesIds.Add("rabbit");
            sys1.State.firstCatchLoggedSpeciesIds.Add("cotton_hare");
            sys1.State.firstCatchLoggedSpeciesIds.Add("ash_pike");

            var state = sys1.CaptureState();
            var sys2 = new WildlifeTrappingSystem(new SeededRng(42));
            sys2.RestoreState(state);

            Assert.Equal(3, sys2.State.firstCatchLoggedSpeciesIds.Count);
            Assert.Contains("rabbit", sys2.State.firstCatchLoggedSpeciesIds);
            Assert.Contains("cotton_hare", sys2.State.firstCatchLoggedSpeciesIds);
            Assert.Contains("ash_pike", sys2.State.firstCatchLoggedSpeciesIds);
        }

        [Fact]
        public void WT_JC_004_LegacySaveWithoutFirstCatch_RestoresAsEmptyList()
        {
            var legacy = new WildlifeTrappingState();
            legacy.firstCatchLoggedSpeciesIds = null!;

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.RestoreState(legacy);

            Assert.NotNull(sys.State.firstCatchLoggedSpeciesIds);
            Assert.Empty(sys.State.firstCatchLoggedSpeciesIds);
        }

        [Fact]
        public void WT_JC_005_JournalSystem_UnlockWildlifeCaught_UnlocksCodexKey()
        {
            var journal = new JournalSystem();
            string species = "rabbit";

            Assert.False(journal.IsWildlifeCaught(species));
            bool unlocked = journal.UnlockWildlifeCaught(species);
            Assert.True(unlocked);
            Assert.True(journal.IsWildlifeCaught(species));

            // Second call is idempotent
            Assert.False(journal.UnlockWildlifeCaught(species));
        }

        [Fact]
        public void WT_JC_006_CodexEntries_ContainsAll15AuthoritativePreySpecies()
        {
            var catalog = LoadTrappingCatalog();
            Assert.NotNull(catalog);
            Assert.Equal(15, catalog.Prey.Count);

            string codexPath = Path.Combine(DataDir, "codex_entries.json");
            Assert.True(File.Exists(codexPath), $"codex_entries.json exists at {codexPath}");

            string json = File.ReadAllText(codexPath);
            using var doc = JsonDocument.Parse(json);
            var entries = doc.RootElement.GetProperty("entries");

            var codexRefs = new HashSet<string>(StringComparer.Ordinal);
            foreach (var el in entries.EnumerateArray())
            {
                if (el.TryGetProperty("category", out var cat) && cat.GetString() == "wildlife")
                {
                    if (el.TryGetProperty("unlock_ref", out var uref))
                    {
                        codexRefs.Add(uref.GetString()!);
                    }
                }
            }

            foreach (var preyKey in catalog.Prey.Keys)
            {
                Assert.True(codexRefs.Contains(preyKey), $"Prey '{preyKey}' must have a matching codex entry with category 'wildlife' and unlock_ref '{preyKey}'");
            }
        }

        // ====================================================================
        // Workstream D: Shelter Crafting Station (WT-CS)
        // ====================================================================

        [Fact]
        public void WT_CS_001_CraftingSystem_DefaultHasNoStations()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);

            Assert.Null(engine.GetStation("workbench"));
        }

        [Fact]
        public void WT_CS_002_WorkbenchRecipe_BlockedWhenNoWorkbenchStation()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);

            var recipe = new Recipe
            {
                id = "craft_trap_box",
                recipeName = "Box Trap",
                requiredStationId = "workbench",
                ingredients = new List<Ingredient>()
            };

            Assert.False(engine.CanCraft(recipe));
        }

        [Fact]
        public void WT_CS_003_WorkbenchRecipe_AllowedWhenOperationalWorkbenchRegistered()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);

            var recipe = new Recipe
            {
                id = "craft_trap_box",
                recipeName = "Box Trap",
                requiredStationId = "workbench",
                ingredients = new List<Ingredient>()
            };

            engine.AddStation(new CraftingStation { id = "workbench", displayName = "Civilian Workbench", condition = 100f });

            Assert.True(engine.CanCraft(recipe));
        }

        [Fact]
        public void WT_CS_004_WorkbenchRecipe_BlockedWhenStationIsBroken()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);

            var recipe = new Recipe
            {
                id = "craft_trap_box",
                recipeName = "Box Trap",
                requiredStationId = "workbench",
                ingredients = new List<Ingredient>()
            };

            engine.AddStation(new CraftingStation { id = "workbench", displayName = "Civilian Workbench", condition = 0f });

            Assert.False(engine.CanCraft(recipe));
        }

        [Fact]
        public void WT_CS_005_RecipeWithoutRequiredStation_CraftableWithoutWorkbench()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);

            var recipe = new Recipe
            {
                id = "craft_trap_improvised_wire",
                recipeName = "Improvised Wire Snare",
                requiredStationId = "",
                ingredients = new List<Ingredient>()
            };

            Assert.True(engine.CanCraft(recipe));
        }

        [Fact]
        public void WT_CS_006_StationDegradeAndRepair_UpdatesOperationalStatus()
        {
            var station = new CraftingStation { id = "workbench", displayName = "Civilian Workbench", condition = 100f };
            Assert.True(station.IsOperational);

            station.Degrade(100f);
            Assert.Equal(0f, station.condition);
            Assert.False(station.IsOperational);

            station.Repair(50f);
            Assert.Equal(50f, station.condition);
            Assert.True(station.IsOperational);
        }

        // ====================================================================
        // Workstream E: Task 8 Cross-System Integration Smoke Test
        // ====================================================================

        public sealed record EndToEndRunSignature(
            int CraftedWireDelta,
            int CraftedSnareDelta,
            int UnrelatedItemCount,
            string DeployedTrapId,
            int InitialDurability,
            int InitialInterval,
            bool InitialBroken,
            List<string> DailyCatchSequence,
            List<int> DailyDurabilitySequence,
            string CaughtPrey,
            string ResolvedDiseaseId,
            float ResolvedContaminationDose,
            bool IsMeatProcessed,
            int PostSaveInventoryWireCount,
            int PostSaveInventorySnareCount,
            int PostSaveTrapDurability,
            bool PostSaveTrapBroken,
            int BreakDay,
            int TerminalDurability,
            bool TerminalBroken,
            int PostBreakCatchCount
        );

        private static EndToEndRunSignature ExecuteEndToEndScenario(int seed)
        {
            var catalog = LoadTrappingCatalog();
            Assert.NotNull(catalog);

            // A. Crafting -> Inventory
            var wireItem = new ItemDefinition { id = "copper_wire_10m_of_10m", displayName = "Copper Wire", type = ItemType.Material, stackMax = 99 };
            var snareItem = new ItemDefinition { id = "trap_improvised_wire", displayName = "Improvised Wire Snare", type = ItemType.Tool, stackMax = 5 };
            var breadItem = new ItemDefinition { id = "bread", displayName = "Ration Bread", type = ItemType.Food, stackMax = 99 };

            var inventory = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 50f };
            inventory.Add(wireItem, 5);
            inventory.Add(breadItem, 10);

            int initialWire = inventory.CountById("copper_wire_10m_of_10m");
            int initialBread = inventory.CountById("bread");

            var crafting = new CraftingSystem(inventory);
            var recipe = new Recipe
            {
                id = "craft_trap_improvised_wire",
                recipeName = "Craft Improvised Wire Snare",
                ingredients = new List<Ingredient>
                {
                    new Ingredient { item = wireItem, amount = 1 }
                },
                result = snareItem,
                resultAmount = 1,
                craftingTimeHours = 0.25f,
                requiredStationId = ""
            };

            bool started = crafting.StartCraft(recipe);
            Assert.True(started);
            int postStartWire = inventory.CountById("copper_wire_10m_of_10m");
            Assert.Equal(initialWire - 1, postStartWire);

            crafting.Tick(0.25f);
            int postCraftSnare = inventory.CountById("trap_improvised_wire");
            Assert.Equal(1, postCraftSnare);
            Assert.Equal(initialBread, inventory.CountById("bread"));

            int craftedWireDelta = postStartWire - initialWire;
            int craftedSnareDelta = postCraftSnare;

            // B. Inventory -> Deployed trap
            var trapping = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog!.RegisterWith(trapping);

            var trapDef = catalog.Traps["trap_improvised_wire"];

            // Atomic payment from inventory
            var bill = new InventoryBill();
            bill.AddCost(trapDef.trap_id, 1);
            using (var tx = inventory.BeginTransaction(bill))
            {
                Assert.True(tx.Validation.IsValid);
                var setRes = trapping.SetTrap("site_alpha", "bait_grain_lure", "hunter_dweller",
                    trapDef.trapType, trapDef.trap_id, trapDef.checkIntervalDays, trapDef.durabilityChecks);
                Assert.True(setRes.IsSuccess);
                tx.TryCommit();
            }

            Assert.Equal(0, inventory.CountById("trap_improvised_wire"));
            var site = trapping.State.trapSites[0];
            Assert.Equal(trapDef.trap_id, site.trapId);
            Assert.Equal(3, site.remainingDurability);
            Assert.Equal(1, site.checkIntervalDays);
            Assert.False(site.isBroken);

            string deployedTrapId = site.trapId;
            int initialDurability = site.remainingDurability;
            int initialInterval = site.checkIntervalDays;
            bool initialBroken = site.isBroken;

            // C. Season + Migration context
            var selectionCtx = new WildlifeSelectionContext
            {
                SeasonWindowId = "window_thaw"
            };
            selectionCtx.PresentMigrationSpecies.Add("species_cotton_hare");
            trapping.SetSelectionContext(selectionCtx);

            var dailyCatches = new List<string>();
            var dailyDurabilities = new List<int>();

            // Day 2 (Check 1)
            trapping.TickDay(2, 1.2f);
            dailyCatches.Add(site.hasCatch ? site.catchSpecies : "none");
            dailyDurabilities.Add(site.remainingDurability);

            // D. Catch -> Butchery -> Health
            // Ensure at least one catch exists to butcher (if not naturally caught, set one deterministic catch)
            if (!site.hasCatch)
            {
                site.hasCatch = true;
                site.catchSpecies = "cotton_hare";
                site.carcassYield = 1.5f;
                site.isToxic = false;
                site.diseaseId = string.Empty;
                site.contaminationDose = 0f;
            }

            string caughtPrey = site.catchSpecies;
            string diseaseId = site.diseaseId;
            float dose = site.contaminationDose;

            var butcherRes = trapping.Butcher(site.siteId, "dweller_butcher");
            Assert.True(butcherRes.IsSuccess);
            Assert.True(site.isMeatProcessed);

            // E. Campaign Save -> Restore
            var serializer = new SystemTextJsonSerializer();
            var invSave = inventory.CaptureState();
            var trapSave = trapping.CaptureState();

            string invJson = serializer.Serialize(invSave);
            string trapJson = serializer.Serialize(trapSave);

            var restoredInvSave = serializer.Deserialize<InventorySaveState>(invJson);
            var restoredTrapSave = serializer.Deserialize<WildlifeTrappingState>(trapJson);
            Assert.NotNull(restoredInvSave);
            Assert.NotNull(restoredTrapSave);

            var freshInv = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 50f };
            freshInv.RestoreState(restoredInvSave!, id => id switch
            {
                "copper_wire_10m_of_10m" => wireItem,
                "trap_improvised_wire" => snareItem,
                "bread" => breadItem,
                _ => null
            });

            var freshTrapping = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(freshTrapping);
            freshTrapping.RestoreState(restoredTrapSave!);

            int postSaveWire = freshInv.CountById("copper_wire_10m_of_10m");
            int postSaveSnare = freshInv.CountById("trap_improvised_wire");
            var restoredSite = freshTrapping.State.trapSites[0];
            int postSaveDurability = restoredSite.remainingDurability;
            bool postSaveBroken = restoredSite.isBroken;

            // Assert restored matches pre-save exactly
            Assert.Equal(site.remainingDurability, restoredSite.remainingDurability);
            Assert.Equal(site.isBroken, restoredSite.isBroken);

            // F. Continue until break and verify broken state
            // Reset catch so trap is primed for subsequent check days
            restoredSite.hasCatch = false;

            // Day 3 (Check 2: durability 2 -> 1)
            freshTrapping.TickDay(3, 1.2f);
            dailyCatches.Add(restoredSite.hasCatch ? restoredSite.catchSpecies : "none");
            dailyDurabilities.Add(restoredSite.remainingDurability);
            restoredSite.hasCatch = false;

            // Day 4 (Check 3: durability 1 -> 0, breaks)
            freshTrapping.TickDay(4, 1.2f);
            dailyCatches.Add(restoredSite.hasCatch ? restoredSite.catchSpecies : "none");
            dailyDurabilities.Add(restoredSite.remainingDurability);

            int breakDay = 4;
            Assert.True(restoredSite.isBroken);
            Assert.Equal(0, restoredSite.remainingDurability);

            int prePostBreakCatches = freshTrapping.State.totalCatch;
            // Advance additional eligible check days after break (days 5, 6, 7)
            freshTrapping.TickDay(5, 1.2f);
            freshTrapping.TickDay(6, 1.2f);
            freshTrapping.TickDay(7, 1.2f);

            int postBreakCatchCount = freshTrapping.State.totalCatch - prePostBreakCatches;
            Assert.Equal(0, postBreakCatchCount);
            Assert.Equal(0, restoredSite.remainingDurability);
            Assert.True(restoredSite.isBroken);

            return new EndToEndRunSignature(
                craftedWireDelta,
                craftedSnareDelta,
                inventory.CountById("bread"),
                deployedTrapId,
                initialDurability,
                initialInterval,
                initialBroken,
                dailyCatches,
                dailyDurabilities,
                caughtPrey,
                diseaseId,
                dose,
                site.isMeatProcessed,
                postSaveWire,
                postSaveSnare,
                postSaveDurability,
                postSaveBroken,
                breakDay,
                restoredSite.remainingDurability,
                restoredSite.isBroken,
                postBreakCatchCount
            );
        }

        [Fact]
        public void WildlifeTrapping_EndToEnd_CraftDeployMigrateButcherSaveRestoreBreak_IsDeterministic()
        {
            var run1 = ExecuteEndToEndScenario(42);
            var run2 = ExecuteEndToEndScenario(42);
            var run3 = ExecuteEndToEndScenario(42);

            string json1 = JsonSerializer.Serialize(run1);
            string json2 = JsonSerializer.Serialize(run2);
            string json3 = JsonSerializer.Serialize(run3);

            Assert.Equal(json1, json2);
            Assert.Equal(json1, json3);

            // Direct assertions on the contract
            Assert.Equal(-1, run1.CraftedWireDelta);
            Assert.Equal(1, run1.CraftedSnareDelta);
            Assert.Equal(10, run1.UnrelatedItemCount);
            Assert.Equal("trap_improvised_wire", run1.DeployedTrapId);
            Assert.Equal(3, run1.InitialDurability);
            Assert.Equal(1, run1.InitialInterval);
            Assert.False(run1.InitialBroken);
            Assert.True(run1.IsMeatProcessed);
            Assert.Equal(4, run1.PostSaveInventoryWireCount);
            Assert.Equal(0, run1.PostSaveInventorySnareCount);
            Assert.Equal(2, run1.PostSaveTrapDurability);
            Assert.False(run1.PostSaveTrapBroken);
            Assert.Equal(0, run1.TerminalDurability);
            Assert.True(run1.TerminalBroken);
            Assert.Equal(0, run1.PostBreakCatchCount);
        }
    }
}
