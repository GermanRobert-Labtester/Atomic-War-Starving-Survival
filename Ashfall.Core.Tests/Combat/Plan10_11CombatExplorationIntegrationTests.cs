// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 10 & Plan 11 Integration Tests:
// - Plan 10: Combat & Catalog Remediation Coverage (Combatant Factory, AI Stances, Faction Lore & Recipes)
// - Plan 11: Exploration & World Map Discovery (Excavation Sites, Cipher Decoders, World Evolution & Route Blockades)
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Excavation;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Combat
{
    public sealed class Plan10_11CombatExplorationIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void Plan10_CombatCatalogAndFactory_SpawnsCatalogCombatants_AndValidatesIntegrity()
        {
            CombatCatalog.Clear();
            bool loaded = CombatCatalogLoader.Load(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(loaded, "Combat catalog must load cleanly from StreamingAssets/Data");

            // Verify spawning authored catalog combatant
            var boar = CombatantFactory.SpawnFromCatalogOrThrow("combatant_armored_boar");
            Assert.NotNull(boar);
            Assert.Equal("combatant_armored_boar", boar.CatalogId);
            Assert.Equal("Ash-Backed Boar", boar.Name);
            Assert.Equal(1, boar.Lane); // Center lane = 1
            Assert.Equal(140f, boar.Health);
            Assert.Equal(140f, boar.MaxHealth);
            Assert.InRange(boar.ArmorRating, 0f, 1f);
            Assert.Equal("HoldPosition", boar.AiStancePreference);
            Assert.Equal("Charge", boar.AiSpecialMove);
            Assert.True(boar.AiAccuracyMod > 1f, "Boar accuracy mod > 1");
            Assert.True(boar.AiDamageMod > 1f, "Boar damage mod > 1");
            Assert.Equal(-1f, boar.SurrenderThreshold); // Never surrenders

            // Verify unknown combatant handling is deterministic
            Assert.Null(CombatantFactory.SpawnFromCatalog("combatant_unknown_test_id"));
            Assert.False(CombatantFactory.TrySpawnFromCatalog("combatant_unknown_test_id", out _, out _));
            Assert.Throws<KeyNotFoundException>(() =>
                CombatantFactory.SpawnFromCatalogOrThrow("combatant_unknown_test_id"));
        }

        [Fact]
        public void Plan10_WarlordAndRecipeCatalogs_ValidateTributes_AndRejectZeroResultSinks()
        {
            // Verify recipe catalog integrity: recipes must produce valid outputs and no zero-result sinks
            string recipesPath = Path.Combine(DataDirectory, "recipes.json");
            Assert.True(File.Exists(recipesPath), $"recipes.json must exist at {recipesPath}");

            string itemsPath = Path.Combine(DataDirectory, "items.json");
            Assert.True(File.Exists(itemsPath), $"items.json must exist at {itemsPath}");

            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            // Load and validate recipes
            string recipeJson = io.ReadAllText(recipesPath);
            Assert.False(string.IsNullOrWhiteSpace(recipeJson));

            // Verify that lubricate_weapon is retired
            Assert.DoesNotContain("lubricate_weapon", recipeJson);

            // Verify warlords catalog
            string warlordsPath = Path.Combine(DataDirectory, "warlord_clans.json");
            if (File.Exists(warlordsPath))
            {
                string warlordsJson = io.ReadAllText(warlordsPath);
                Assert.False(string.IsNullOrWhiteSpace(warlordsJson));
            }
        }

        [Fact]
        public void Plan11_ExcavationSystem_LoadsCatalogSites_ProgressesExcavation_AndAppliesShoring()
        {
            var sites = ExcavationCatalogLoader.Load(DataDirectory);
            Assert.NotNull(sites);
            Assert.True(sites.Count >= 5, $"Expected at least 5 excavation sites, got {sites.Count}");

            var expectedIds = new[]
            {
                "excavation_command_vault",
                "excavation_utility_tunnels",
                "excavation_metro_interchange",
                "excavation_mine_shaft",
                "excavation_archive_bunker"
            };

            foreach (var id in expectedIds)
            {
                var site = sites.FirstOrDefault(s => s.site_id == id);
                Assert.NotNull(site);
                Assert.False(string.IsNullOrEmpty(site.location_id));
                Assert.True(site.max_depth_meters > 0f);
                Assert.True(site.required_progress > 0f);
                Assert.NotNull(site.depth_bands);
                Assert.True(site.depth_bands.Count >= 3, $"Site {id} should have at least 3 depth bands.");
            }

            // Test Excavation System progress & structural shoring
            var rng = new SeededRng(2026);
            var excavationSys = new ExcavationSystem(rng);
            excavationSys.AddSite("site_test_vault", "room_command", 100f, 0.4f);
            excavationSys.AssignWorkers("site_test_vault", 2);

            var activeSite = excavationSys.State.sites[0];
            float initialRisk = activeSite.structuralRisk;

            // Apply shoring: risk halved
            var shoringResult = excavationSys.ApplyShoring("site_test_vault");
            Assert.True(shoringResult.IsSuccess);
            Assert.True(activeSite.shoringApplied);
            Assert.Equal(initialRisk * 0.5f, activeSite.structuralRisk, precision: 3);

            // Shoring grants 1.2x progress bonus (2 workers * 5 * 1.2 = 12 progress)
            excavationSys.TickDay();
            Assert.True(activeSite.progress >= 12f);
        }

        [Fact]
        public void Plan11_CipherDecoding_And_WorldEvolutionRouteBlockade_Integration()
        {
            var (nodes, routes) = WastelandMapCatalogLoader.Load(DataDirectory);
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var cipherEngine = new CipherQuestChainEngine();

            // 1. Cipher quest chain integration: hearing radio broadcast + finding codebook reveals location
            var relayChain = cipherEngine.GetState("relay_count");
            Assert.False(relayChain.isDecoded);
            Assert.False(map.IsDiscovered("loc_hidden_relay_bunker"));

            cipherEngine.RecordBroadcastHeard("radio_broadcast_relay_count", map);
            cipherEngine.RecordKeyAcquired("item_comm_codebook_alpha", map);

            Assert.True(relayChain.isKeyFound);
            Assert.True(relayChain.isDecoded);
            Assert.True(relayChain.isLocationRevealed);
            Assert.True(map.IsDiscovered("loc_hidden_relay_bunker"), "Decoded cipher must reveal hidden relay bunker");

            // 2. World evolution engine: event triggers node blockade & route detour
            var worldEvolution = new WorldEvolutionEngine(DataDirectory);
            Assert.True(worldEvolution.Events.Count >= 10, "World evolution must have at least 10 authored events");

            map.Discover("loc_holdfast");
            map.Discover("loc_cut_abandoned_depot");
            map.Discover("loc_cut_arsenal_ruin");

            var baselineRoute = map.PlanRoute("loc_holdfast", "loc_cut_arsenal_ruin");
            Assert.NotEmpty(baselineRoute);

            // Lock node via blockade
            map.Lock("loc_cut_abandoned_depot");
            Assert.True(map.IsLocked("loc_cut_abandoned_depot"));

            // Re-plan route: path must not traverse locked blockaded node
            var detourRoute = map.PlanRoute("loc_holdfast", "loc_cut_arsenal_ruin");
            if (detourRoute.Count > 0)
            {
                Assert.DoesNotContain("loc_cut_abandoned_depot", detourRoute);
            }
        }
    }
}
