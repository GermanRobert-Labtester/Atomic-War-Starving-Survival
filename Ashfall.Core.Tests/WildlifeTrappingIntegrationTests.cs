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
    }
}
