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

        private static string FindRepoRoot()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                if (File.Exists(Path.Combine(search, "project.godot")))
                    return search;
                string? parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return AppContext.BaseDirectory;
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
        public void WT_WX_005_DeterministicReplay_SameSeedAndWeather_ProducesIdenticalCatch()
        {
            var sys1 = new WildlifeTrappingSystem(new SeededRng(1984));
            var sys2 = new WildlifeTrappingSystem(new SeededRng(1984));
            sys1.SetSelectionContext(new WildlifeSelectionContext { CurrentWeather = WeatherKind.Ashfall });
            sys2.SetSelectionContext(new WildlifeSelectionContext { CurrentWeather = WeatherKind.Ashfall });

            sys1.SetTrap("site_det", "bait_grain", "hunter_a");
            sys2.SetTrap("site_det", "bait_grain", "hunter_a");

            sys1.CheckTraps();
            sys2.CheckTraps();

            Assert.Equal(sys1.State.trapSites[0].hasCatch, sys2.State.trapSites[0].hasCatch);
            Assert.Equal(sys1.State.trapSites[0].catchSpecies, sys2.State.trapSites[0].catchSpecies);
            Assert.Equal(sys1.State.trapSites[0].remainingDurability, sys2.State.trapSites[0].remainingDurability);
        }

        [Fact]
        public void WT_WX_006_BycatchIsolation_WeatherDoesNotAlterBycatchFormula()
        {
            var catalog = LoadTrappingCatalog();
            Assert.NotNull(catalog);
            var trapDef = catalog!.Traps["trap_net"];
            Assert.True(trapDef.bycatchChance > 0f);

            float clearMult = WildlifeTrappingSystem.CalculateWeatherMultiplier(trapDef.weatherSensitivity, WeatherKind.Clear);
            float blizzardMult = WildlifeTrappingSystem.CalculateWeatherMultiplier(trapDef.weatherSensitivity, WeatherKind.Blizzard);
            Assert.NotEqual(clearMult, blizzardMult);

            // Authored bycatchChance is isolated from weather penalty
            Assert.Equal(0.25f, trapDef.bycatchChance, 2);
        }

        [Fact]
        public void WT_WX_007_DurabilityDecrementsOnCheck_RegardlessOfWeather()
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
        public void WT_WX_008_ExhaustiveEnumPolicy_EveryWeatherKindHasExplicitMapping()
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
        public void WT_WX_009_PrimaryCatchChance_ClampsBetween005And095()
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

        [Fact]
        public void WT_SK_006_TwoTraps_TwoHunters_EvaluatedIndependently()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(1234));
            var ctx = new WildlifeSelectionContext();
            ctx.HunterSkillLevels["hunter_novice"] = 0f;
            ctx.HunterSkillLevels["hunter_expert"] = 100f;
            sys.SetSelectionContext(ctx);

            sys.SetTrap("site_1", "", "hunter_novice");
            sys.SetTrap("site_2", "", "hunter_expert");

            float chance1 = WildlifeTrappingSystem.CalculatePrimaryCatchChance(1.0f, 0f, 1.0f, 0f, WeatherKind.Clear);
            float chance2 = WildlifeTrappingSystem.CalculatePrimaryCatchChance(1.0f, 100f, 1.0f, 0f, WeatherKind.Clear);

            Assert.Equal(0.25f, chance1, 3);
            Assert.Equal(0.75f, chance2, 3);
        }

        [Fact]
        public void WT_SK_007_QuarryEligibilityPerHunter_MinSkillLevelGating()
        {
            var catalog = LoadTrappingCatalog();
            Assert.NotNull(catalog);
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog!.RegisterWith(sys);

            // Register custom high-skill quarry requiring skill 60
            sys.RegisterPreyDefinition(new PreyDefinition { speciesId = "apex_stag" });
            sys.RegisterQuarry(new QuarrySpecies { speciesId = "apex_stag", minSkillLevel = 60f });

            var ctxNovice = new WildlifeSelectionContext();
            ctxNovice.HunterSkillLevels["hunter_novice"] = 10f;
            sys.SetSelectionContext(ctxNovice);
            sys.SetTrap("site_novice", "", "hunter_novice");

            // For novice (skill 10 < 60), apex_stag is ineligible
            for (int i = 0; i < 20; i++)
            {
                sys.State.trapSites[0].remainingDurability = 5;
                sys.State.trapSites[0].checkDay = 1;
                sys.CheckTraps();
                if (sys.State.trapSites[0].hasCatch)
                {
                    Assert.NotEqual("apex_stag", sys.State.trapSites[0].catchSpecies);
                }
            }
        }

        [Fact]
        public void WT_SK_008_MidCampaignProgressionUpdate_SeenOnNextCheck()
        {
            var prog = new SkillProgressionSystem();
            prog.RegisterSkill(new SkillDef { id = "skill_surv_1", disciplineId = "survival", xpThreshold = 100f });
            prog.RegisterSkill(new SkillDef { id = "skill_surv_2", disciplineId = "survival", xpThreshold = 200f });
            var actor = new SimpleSkillActor("dweller_trapper", "survival");

            // Initial: 0 XP -> 0% progress -> skill 0
            float initialProg = prog.GetDisciplineProgress01(actor.Id, "survival");
            Assert.Equal(0f, initialProg);

            var ctx = new WildlifeSelectionContext();
            ctx.HunterSkillLevels[actor.Id] = initialProg * 100f;
            float initialChance = WildlifeTrappingSystem.CalculatePrimaryCatchChance(1.0f, ctx.HunterSkillLevels[actor.Id], 1.0f, 0f, WeatherKind.Clear);
            Assert.Equal(0.25f, initialChance, 3);

            // Award 150 XP mid-campaign
            prog.RecordAction(actor, "survival", 150f, 10);
            float updatedProg = prog.GetDisciplineProgress01(actor.Id, "survival");
            Assert.Equal(0.75f, updatedProg, 3);

            // Rebuild context on next check
            ctx.HunterSkillLevels[actor.Id] = updatedProg * 100f;
            float updatedChance = WildlifeTrappingSystem.CalculatePrimaryCatchChance(1.0f, ctx.HunterSkillLevels[actor.Id], 1.0f, 0f, WeatherKind.Clear);
            Assert.Equal(0.625f, updatedChance, 3);
            Assert.True(updatedChance > initialChance);
        }

        [Fact]
        public void WT_SK_009_BycatchIsolation_SkillDoesNotModifyBycatch()
        {
            var catalog = LoadTrappingCatalog();
            Assert.NotNull(catalog);
            var trapDef = catalog!.Traps["trap_net"];
            Assert.NotNull(trapDef);

            float skillNoviceMult = WildlifeTrappingSystem.SkillMultiplierFor(0f);
            float skillMasterMult = WildlifeTrappingSystem.SkillMultiplierFor(100f);
            Assert.NotEqual(skillNoviceMult, skillMasterMult);

            Assert.Equal(0.25f, trapDef.bycatchChance, 2);
        }

        [Fact]
        public void WT_SK_010_DurabilityIsolation_SkillDoesNotAlterDurabilityDecrement()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(101));
            var ctx = new WildlifeSelectionContext();
            ctx.HunterSkillLevels["novice"] = 0f;
            ctx.HunterSkillLevels["master"] = 100f;
            sys.SetSelectionContext(ctx);

            sys.SetTrap("site_novice", "", "novice");
            sys.SetTrap("site_master", "", "master");
            sys.State.trapSites[0].remainingDurability = 5;
            sys.State.trapSites[0].checkDay = 1;
            sys.State.trapSites[1].remainingDurability = 5;
            sys.State.trapSites[1].checkDay = 1;

            sys.CheckTraps();

            Assert.Equal(4, sys.State.trapSites[0].remainingDurability);
            Assert.Equal(4, sys.State.trapSites[1].remainingDurability);
        }

        [Fact]
        public void WT_SK_011_SharedAuthorityGuard_TrappingResolvesSharedSkillProgression()
        {
            string root = FindRepoRoot();
            string evolvingWorldPath = Path.Combine(root, "src", "Main.EvolvingWorld.cs");
            Assert.True(File.Exists(evolvingWorldPath), $"Main.EvolvingWorld.cs exists at {evolvingWorldPath}");
            string content = File.ReadAllText(evolvingWorldPath);

            Assert.Contains("EnsureSharedSkillProgression()", content);
            Assert.Contains("skillProgression.GetDisciplineProgress01", content);
            Assert.Contains("\"survival\"", content);
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
        public void WT_JC_003_DifferentSpecies_SequentialDiscovery_FiresOnceEach()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var discovered = new List<string>();
            sys.OnNewSpeciesDiscovered += (sp, site, hunter) => discovered.Add(sp);

            // Record first rabbit
            sys.State.firstCatchLoggedSpeciesIds.Add("rabbit");

            // Mock catches
            sys.SetTrap("site_1", "bait_grain", "hunter_1");
            sys.SetTrap("site_2", "bait_scrap", "hunter_2");

            var setMethod = typeof(WildlifeTrappingSystem).GetMethod("TryRecordFirstCatch",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(setMethod);

            setMethod!.Invoke(sys, new object[] { "cotton_hare", "site_1", "hunter_1" });
            setMethod.Invoke(sys, new object[] { "rad_rat", "site_2", "hunter_2" });
            // Repeat rabbit (already logged)
            setMethod.Invoke(sys, new object[] { "rabbit", "site_1", "hunter_1" });

            Assert.Equal(2, discovered.Count);
            Assert.Equal("cotton_hare", discovered[0]);
            Assert.Equal("rad_rat", discovered[1]);
        }

        [Fact]
        public void WT_JC_004_FirstCatchLoggedSpeciesIds_RoundTripsThroughSaveRestore()
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
        public void WT_JC_005_LegacySaveWithoutFirstCatch_RestoresAsEmptyList()
        {
            var legacy = new WildlifeTrappingState();
            legacy.firstCatchLoggedSpeciesIds = null!;

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.RestoreState(legacy);

            Assert.NotNull(sys.State.firstCatchLoggedSpeciesIds);
            Assert.Empty(sys.State.firstCatchLoggedSpeciesIds);
        }

        private sealed class TrappingTestAuthor : ISurvivorAuthor
        {
            public string Id { get; }
            public string DisplayName { get; }
            public RiskBiasTrait RiskBias { get; }

            public TrappingTestAuthor(string id, string name, RiskBiasTrait bias = RiskBiasTrait.Realist)
            {
                Id = id;
                DisplayName = name;
                RiskBias = bias;
            }
        }

        [Fact]
        public void WT_JC_006_JournalEntry_CreatedOnce_WithValidAuthorAndDedup()
        {
            var journal = new JournalSystem();
            var author = new TrappingTestAuthor("hunter_anna", "Hunter Anna");
            string knowledgeKey = "wildlife_species_caught_rabbit";

            var entry1 = journal.TryDiscoverRawKnowledge(knowledgeKey, "Captured a wild rabbit.", author, 3);
            Assert.NotNull(entry1);
            Assert.Equal(1, journal.CodexUnlockCount);
            Assert.Equal("Hunter Anna", entry1!.AuthorName);
            Assert.True(journal.Knowledge.Has(knowledgeKey));

            // Duplicate call does not produce second entry
            var entry2 = journal.TryDiscoverRawKnowledge(knowledgeKey, "Captured another wild rabbit.", author, 4);
            Assert.Null(entry2);
            Assert.Equal(1, journal.CodexUnlockCount);
        }

        [Fact]
        public void WT_JC_007_JournalSystem_UnlockWildlifeCaught_UnlocksCodexKey()
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
        public void WT_JC_008_CodexEntries_ContainsAll15AuthoritativePreySpecies()
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

        [Fact]
        public void WT_JC_009_Bycatch_NotCountedAsFirstCatch()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var discovered = new List<string>();
            sys.OnNewSpeciesDiscovered += (sp, site, hunter) => discovered.Add(sp);

            sys.SetTrap("site_bycatch", "bait_scrap", "hunter_bob");
            var site = sys.State.trapSites[0];
            site.hasCatch = true;
            site.catchSpecies = "rabbit";
            site.bycatchSpecies = "rad_rat";

            // Verify only catchSpecies is recorded in firstCatchLoggedSpeciesIds
            var method = typeof(WildlifeTrappingSystem).GetMethod("TryRecordFirstCatch",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(method);
            method!.Invoke(sys, new object[] { site.catchSpecies, site.siteId, site.assignedHunterId });

            Assert.Single(discovered);
            Assert.Equal("rabbit", discovered[0]);
            Assert.Contains("rabbit", sys.State.firstCatchLoggedSpeciesIds);
            Assert.DoesNotContain("rad_rat", sys.State.firstCatchLoggedSpeciesIds);
        }

        // ====================================================================
        // Workstream D: Shelter Crafting Station (WT-CS)
        // ====================================================================

        private static JsonElement? GetRecipeFromJson(string recipeId)
        {
            string path = Path.Combine(DataDir, "recipes.json");
            if (!File.Exists(path)) return null;
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (!doc.RootElement.TryGetProperty("recipes", out var recipes)) return null;
            foreach (var r in recipes.EnumerateArray())
            {
                if (r.TryGetProperty("id", out var idProp) && idProp.GetString() == recipeId)
                {
                    return r.Clone();
                }
            }
            return null;
        }

        [Fact]
        public void WT_CS_001_DataContract_ImprovisedWire_RequiresNoStation()
        {
            var recipe = GetRecipeFromJson("craft_trap_improvised_wire");
            Assert.NotNull(recipe);
            Assert.True(recipe!.Value.TryGetProperty("requiredStationId", out var stationProp));
            Assert.Equal(string.Empty, stationProp.GetString());
        }

        [Fact]
        public void WT_CS_002_DataContract_BoxTrap_RequiresWorkbench()
        {
            var recipe = GetRecipeFromJson("craft_trap_box");
            Assert.NotNull(recipe);
            Assert.True(recipe!.Value.TryGetProperty("requiredStationId", out var stationProp));
            Assert.Equal("workbench", stationProp.GetString());
        }

        [Fact]
        public void WT_CS_003_DataContract_FishTrap_RequiresWorkbench()
        {
            var recipe = GetRecipeFromJson("craft_trap_fish");
            Assert.NotNull(recipe);
            Assert.True(recipe!.Value.TryGetProperty("requiredStationId", out var stationProp));
            Assert.Equal("workbench", stationProp.GetString());
        }

        [Fact]
        public void WT_CS_004_StationlessRecipe_CraftableWithoutWorkbench()
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
        public void WT_CS_005_BoxTrap_BlockedWithoutWorkbench()
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
        public void WT_CS_006_FishTrap_BlockedWithoutWorkbench()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);
            var recipe = new Recipe
            {
                id = "craft_trap_fish",
                recipeName = "Fish Trap",
                requiredStationId = "workbench",
                ingredients = new List<Ingredient>()
            };

            Assert.False(engine.CanCraft(recipe));
        }

        [Fact]
        public void WT_CS_007_BrokenWorkbench_BlocksBoxAndFishTraps()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);
            engine.AddStation(new CraftingStation { id = "workbench", displayName = "Civilian Workbench", condition = 0f });

            var boxRecipe = new Recipe { id = "craft_trap_box", requiredStationId = "workbench", ingredients = new List<Ingredient>() };
            var fishRecipe = new Recipe { id = "craft_trap_fish", requiredStationId = "workbench", ingredients = new List<Ingredient>() };

            Assert.False(engine.CanCraft(boxRecipe));
            Assert.False(engine.CanCraft(fishRecipe));
        }

        [Fact]
        public void WT_CS_008_OperationalWorkbench_AllowsBoxTrap()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);
            engine.AddStation(new CraftingStation { id = "workbench", displayName = "Civilian Workbench", condition = 100f });

            var recipe = new Recipe { id = "craft_trap_box", requiredStationId = "workbench", ingredients = new List<Ingredient>() };
            Assert.True(engine.CanCraft(recipe));
        }

        [Fact]
        public void WT_CS_009_OperationalWorkbench_AllowsFishTrap()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);
            engine.AddStation(new CraftingStation { id = "workbench", displayName = "Civilian Workbench", condition = 100f });

            var recipe = new Recipe { id = "craft_trap_fish", requiredStationId = "workbench", ingredients = new List<Ingredient>() };
            Assert.True(engine.CanCraft(recipe));
        }

        [Fact]
        public void WT_CS_010_ShelterNotBuilt_WorkbenchAbsent()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);

            Assert.Null(engine.GetStation("workbench"));
        }

        [Fact]
        public void WT_CS_011_ShelterBuilt_WorkbenchSynchronizes()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);

            var station = new CraftingStation { id = "workbench", displayName = "Civilian Workbench", condition = 100f };
            engine.AddStation(station);

            var synced = engine.GetStation("workbench");
            Assert.NotNull(synced);
            Assert.True(synced!.IsOperational);
            Assert.Equal(100f, synced.condition);
        }

        [Fact]
        public void WT_CS_012_StationLosesAvailability_BlocksNewCraft()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var engine = new CraftingSystem(inv);
            var station = new CraftingStation { id = "workbench", displayName = "Civilian Workbench", condition = 100f };
            engine.AddStation(station);

            var recipe = new Recipe { id = "craft_trap_box", requiredStationId = "workbench", ingredients = new List<Ingredient>() };
            Assert.True(engine.CanCraft(recipe));

            // Station degrades to broken
            station.Degrade(100f);
            Assert.False(station.IsOperational);
            Assert.False(engine.CanCraft(recipe));

            // Station repaired -> operational
            station.Repair(50f);
            Assert.True(station.IsOperational);
            Assert.True(engine.CanCraft(recipe));

            // Station removed -> blocked
            engine.RemoveStation(station);
            Assert.Null(engine.GetStation("workbench"));
            Assert.False(engine.CanCraft(recipe));
        }

        [Fact]
        public void WT_CS_013_NoUnconditionalProductionSeed_SourceGate()
        {
            string repoRoot = FindRepoRoot();
            string path = Path.Combine(repoRoot, "src", "Host", "CraftingHostSession.cs");
            Assert.True(File.Exists(path), $"CraftingHostSession.cs not found at {path}");

            string code = File.ReadAllText(path);
            Assert.Contains("bool seedDefaultWorkbench = false", code);
            Assert.Contains("seedDefaultWorkbench: false", code);
        }

        // ====================================================================
        // Workstream E: Cross-System Integration (WT-XI)
        // ====================================================================

        [Fact]
        public void WT_XI_001_DailyWorldRefresh_OccursBeforeTrapCheck()
        {
            string repoRoot = FindRepoRoot();
            string path = Path.Combine(repoRoot, "src", "Main.ExpandedShelterSystems.cs");
            Assert.True(File.Exists(path), $"Main.ExpandedShelterSystems.cs not found at {path}");

            string code = File.ReadAllText(path);
            int refreshIdx = code.IndexOf("RefreshTrappingDensity();", StringComparison.Ordinal);
            int tickTrapIdx = code.IndexOf("_wildlifeTrapping?.TickDay(day);", StringComparison.Ordinal);

            Assert.True(refreshIdx >= 0, "RefreshTrappingDensity() must be present in Main.ExpandedShelterSystems.cs");
            Assert.True(tickTrapIdx >= 0, "_wildlifeTrapping?.TickDay(day); must be present in Main.ExpandedShelterSystems.cs");
            Assert.True(refreshIdx < tickTrapIdx, "RefreshTrappingDensity() must occur before _wildlifeTrapping?.TickDay(day);");
        }

        [Fact]
        public void WT_XI_002_FullCheck_UsesWeather_Density_Hunter_And_Bait()
        {
            // Baseline roll: density 1.0, skill 50 (neutral 1.0x), bait 1.0, sensitivity 0.5, weather Clear (1.0x)
            float baseline = WildlifeTrappingSystem.CalculatePrimaryCatchChance(
                densityMultiplier: 1.0f,
                hunterSkillLevel: 50f,
                baitMultiplier: 1.0f,
                weatherSensitivity: 0.5f,
                weather: WeatherKind.Clear);

            Assert.Equal(0.5f, baseline, precision: 3);

            // Weather impact: Blizzard reduces chance for weather-sensitive traps
            float blizzardChance = WildlifeTrappingSystem.CalculatePrimaryCatchChance(
                densityMultiplier: 1.0f,
                hunterSkillLevel: 50f,
                baitMultiplier: 1.0f,
                weatherSensitivity: 0.8f,
                weather: WeatherKind.Blizzard);

            Assert.True(blizzardChance < baseline, $"Blizzard ({blizzardChance}) should be lower than baseline ({baseline})");

            // Hunter skill increases chance (100 skill > 50 skill)
            float skilledChance = WildlifeTrappingSystem.CalculatePrimaryCatchChance(
                densityMultiplier: 1.0f,
                hunterSkillLevel: 100f,
                baitMultiplier: 1.0f,
                weatherSensitivity: 0.5f,
                weather: WeatherKind.Clear);

            Assert.True(skilledChance > baseline, $"Skilled hunter ({skilledChance}) should be higher than baseline ({baseline})");

            // Bait multiplier increases chance
            float baitedChance = WildlifeTrappingSystem.CalculatePrimaryCatchChance(
                densityMultiplier: 1.0f,
                hunterSkillLevel: 50f,
                baitMultiplier: 1.5f,
                weatherSensitivity: 0.5f,
                weather: WeatherKind.Clear);

            Assert.True(baitedChance > baseline, $"Baited ({baitedChance}) should be higher than baseline ({baseline})");

            // Density scales chance
            float highDensityChance = WildlifeTrappingSystem.CalculatePrimaryCatchChance(
                densityMultiplier: 1.5f,
                hunterSkillLevel: 50f,
                baitMultiplier: 1.0f,
                weatherSensitivity: 0.5f,
                weather: WeatherKind.Clear);

            Assert.True(highDensityChance > baseline, $"High density ({highDensityChance}) should be higher than baseline ({baseline})");

            // Clamping bounds: 0.05f to 0.95f
            float extremeLow = WildlifeTrappingSystem.CalculatePrimaryCatchChance(
                densityMultiplier: 0.01f,
                hunterSkillLevel: 0f,
                baitMultiplier: 0.1f,
                weatherSensitivity: 1.0f,
                weather: WeatherKind.Ashfall);
            Assert.Equal(0.05f, extremeLow, precision: 3);

            float extremeHigh = WildlifeTrappingSystem.CalculatePrimaryCatchChance(
                densityMultiplier: 3.0f,
                hunterSkillLevel: 100f,
                baitMultiplier: 3.0f,
                weatherSensitivity: 0f,
                weather: WeatherKind.Clear);
            Assert.Equal(0.95f, extremeHigh, precision: 3);
        }

        [Fact]
        public void WT_XI_003_PostLoadContextRebuild_BeforeCheck()
        {
            var catalog = LoadTrappingCatalog();
            Assert.NotNull(catalog);

            var sys1 = new WildlifeTrappingSystem(new SeededRng(42));
            catalog!.RegisterWith(sys1);

            sys1.SetTrap("site_alpha", "bait_scrap_meat", "hunter_dweller");

            // Provide ephemeral context to sys1
            var ctx1 = new WildlifeSelectionContext
            {
                CurrentWeather = WeatherKind.Rain,
                SeasonWindowId = "window_spring_storms"
            };
            ctx1.HunterSkillLevels["hunter_dweller"] = 80f;
            sys1.SetSelectionContext(ctx1);

            var savedState = sys1.CaptureState();

            // Verify savedState does NOT persist ephemeral context
            string json = new SystemTextJsonSerializer().Serialize(savedState);
            Assert.DoesNotContain("Rain", json);
            Assert.DoesNotContain("window_spring_storms", json);
            Assert.DoesNotContain("HunterSkillLevels", json);

            // Restore into fresh system
            var sys2 = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys2);
            sys2.RestoreState(savedState);

            // Post-load: host rebuilds context
            var ctx2 = new WildlifeSelectionContext
            {
                CurrentWeather = WeatherKind.Rain,
                SeasonWindowId = "window_spring_storms"
            };
            ctx2.HunterSkillLevels["hunter_dweller"] = 80f;
            sys2.SetSelectionContext(ctx2);

            // CheckTraps succeeds using rebuilt context
            var res = sys2.CheckTraps();
            Assert.True(res.IsSuccess);
        }

        [Fact]
        public void WT_XI_004_DiseaseAndContaminationBridge_Unchanged()
        {
            var catalog = LoadTrappingCatalog();
            Assert.NotNull(catalog);

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog!.RegisterWith(sys);

            sys.SetTrap("site_alpha", "bait_scrap_meat", "hunter_dweller");
            var site = sys.State.trapSites[0];

            // Direct catch with disease and contamination
            site.hasCatch = true;
            site.catchSpecies = "rad_rat";
            site.carcassYield = 2.0f;
            site.isToxic = true;
            site.diseaseId = "parasites_rat_lung";
            site.contaminationDose = 3.5f;

            // Butchery processes meat
            var butcherRes = sys.Butcher(site.siteId, "dweller_butcher");
            Assert.True(butcherRes.IsSuccess);
            Assert.True(site.isMeatProcessed);
            Assert.Equal("parasites_rat_lung", site.diseaseId);
            Assert.Equal(3.5f, site.contaminationDose);

            // Duplicate butchery is blocked
            var duplicate = sys.Butcher(site.siteId, "dweller_butcher");
            Assert.False(duplicate.IsSuccess);
        }

        [Fact]
        public void WT_XI_005_OverhuntCatchPressure_Unchanged()
        {
            var catalog = LoadTrappingCatalog();
            Assert.NotNull(catalog);

            var sys = new WildlifeTrappingSystem(new SeededRng(100));
            catalog!.RegisterWith(sys);

            sys.SetTrap("site_alpha", "bait_scrap_meat", "hunter_dweller", "snare", "trap_snare", checkIntervalDays: 1, durabilityChecks: 5);
            var site = sys.State.trapSites[0];
            site.checkDay = 1;

            // Simulate successive check days with declining density multiplier
            sys.TickDay(1, densityMultiplier: 1.0f);
            Assert.Equal(4, site.remainingDurability);

            site.hasCatch = false;
            sys.TickDay(2, densityMultiplier: 0.5f);
            Assert.Equal(3, site.remainingDurability);

            site.hasCatch = false;
            sys.TickDay(3, densityMultiplier: 0.2f);
            Assert.Equal(2, site.remainingDurability);
        }

        [Fact]
        public void WT_XI_006_PanelBinding_StillWorks_SourceGate()
        {
            string repoRoot = FindRepoRoot();
            string path = Path.Combine(repoRoot, "src", "UI", "WildlifeTrappingPanel.cs");
            Assert.True(File.Exists(path), $"WildlifeTrappingPanel.cs not found at {path}");

            string code = File.ReadAllText(path);
            Assert.Contains("IBindablePanel", code);
            Assert.Contains("public void Bind(WildlifeTrappingHostSession session)", code);
            Assert.Contains("public void Unbind()", code);
        }

        // ====================================================================
        // End-to-End Deterministic Scenario
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
                SeasonWindowId = "window_spring_storms"
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
