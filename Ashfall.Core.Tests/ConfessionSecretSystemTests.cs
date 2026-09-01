using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Phantoms;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ConfessionSecretSystemTests
    {
        private readonly string _dataDir;
        private readonly IJsonSerializer _serializer = new SystemTextJsonSerializer();

        public ConfessionSecretSystemTests()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir))
            {
                _dataDir = dir;
            }
            else
            {
                _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            }
        }

        private ConfessionSecretCatalog CreateLoadedCatalog()
        {
            var catalog = new ConfessionSecretCatalog();
            string filePath = Path.Combine(_dataDir, "confession_secrets.json");
            if (File.Exists(filePath))
            {
                catalog.Load(File.ReadAllText(filePath), _serializer);
            }
            return catalog;
        }

        [Fact]
        public void ConfessionSecretCatalog_Load_ParsesAll26Secrets()
        {
            var catalog = CreateLoadedCatalog();
            Assert.True(catalog.AllSecrets.Count >= 26, "Expected at least 26 authored confession secrets.");

            var surgeonSecret = catalog.GetById("secret_surgeon_lost_patient");
            Assert.NotNull(surgeonSecret);
            Assert.Equal("The Patient They Lost", surgeonSecret.secret_title);
            Assert.Equal("the_surgeon", surgeonSecret.subject_id);
            Assert.Equal("silver_scalpel", surgeonSecret.discovery_source_id);

            var militarySecret = catalog.GetById("secret_faction_military_rigged_census");
            Assert.NotNull(militarySecret);
            Assert.Equal("faction_institutional", militarySecret.category);
        }

        [Fact]
        public void ConfessionSecretSystem_DiscoverSecret_RegistersState()
        {
            var catalog = CreateLoadedCatalog();
            var system = new ConfessionSecretSystem(catalog);

            bool discovered = system.DiscoverSecret("secret_soldier_civilian_order", currentDay: 5, sourceId: "dog_tags");
            Assert.True(discovered);
            Assert.True(system.IsDiscovered("secret_soldier_civilian_order"));
            Assert.False(system.IsResolved("secret_soldier_civilian_order"));
        }

        [Fact]
        public void ConfessionSecretSystem_ExposeSecret_AppliesFactionAndGuilt()
        {
            var catalog = CreateLoadedCatalog();
            var system = new ConfessionSecretSystem(catalog);
            var guilt = new GuiltInsomniaSystem();

            system.DiscoverSecret("secret_soldier_civilian_order", currentDay: 5, sourceId: "dog_tags");

            string changedFaction = null;
            float changedDelta = 0f;

            bool exposed = system.ExposeSecret(
                "secret_soldier_civilian_order",
                currentDay: 6,
                needs: null,
                guilt: guilt,
                onFactionStandingChanged: (faction, delta) =>
                {
                    changedFaction = faction;
                    changedDelta = delta;
                });

            Assert.True(exposed);
            Assert.True(system.IsResolved("secret_soldier_civilian_order"));
            Assert.Equal("faction_rebel", changedFaction);
            Assert.True(changedDelta > 0);
            Assert.True(guilt.GetGuiltSourceCount("the_soldier") > 0);
        }

        [Fact]
        public void ConfessionSecretSystem_BlackmailSecret_AppliesHardening()
        {
            var catalog = CreateLoadedCatalog();
            var system = new ConfessionSecretSystem(catalog);
            var moral = new MoralBranchingSystem();
            moral.Register(new MoralBranchState { SurvivorId = "the_pharmacist" });

            system.DiscoverSecret("secret_pharmacist_stolen_morphine", currentDay: 10, sourceId: "morphine");

            bool blackmailed = system.BlackmailSecret(
                "secret_pharmacist_stolen_morphine",
                currentDay: 11,
                moral: moral);

            Assert.True(blackmailed);
            Assert.True(system.IsResolved("secret_pharmacist_stolen_morphine"));
            Assert.True(moral.GetState("the_pharmacist")!.NumbedResilienceLevel > 0);
        }

        [Fact]
        public void ConfessionSecretSystem_KeepSecret_AppliesTrust()
        {
            var catalog = CreateLoadedCatalog();
            var system = new ConfessionSecretSystem(catalog);
            var rng = new SeededRng(42);
            var relations = new SurvivorRelationsSystem(rng);
            var rel = relations.GetOrCreateRelationship("the_mother", "sv_confidant");
            rel.trust = 50;

            system.DiscoverSecret("secret_mother_child_left", currentDay: 12, sourceId: "childs_mitten");

            bool kept = system.KeepSecret(
                "secret_mother_child_left",
                currentDay: 13,
                relations: relations,
                confidantSurvivorId: "sv_confidant");

            Assert.True(kept);
            Assert.True(system.IsResolved("secret_mother_child_left"));
            Assert.True(rel.trust > 50);
        }

        [Fact]
        public void ConfessionSecretSystem_ResolveInterpersonal_ForgivenessAndGrudge()
        {
            var catalog = CreateLoadedCatalog();
            var system = new ConfessionSecretSystem(catalog);
            var rng = new SeededRng(42);
            var relations = new SurvivorRelationsSystem(rng);
            var rel1 = relations.GetOrCreateRelationship("sv_surgeon", "sv_listener");
            rel1.affinity = 50;

            system.DiscoverSecret("secret_surgeon_lost_patient", currentDay: 15, sourceId: "silver_scalpel");

            // Forgiveness path
            bool forgiven = system.ResolveInterpersonal(
                "secret_surgeon_lost_patient",
                currentDay: 16,
                forgive: true,
                confessorId: "sv_surgeon",
                listenerId: "sv_listener",
                relations: relations);

            Assert.True(forgiven);
            Assert.True(system.IsResolved("secret_surgeon_lost_patient"));
            Assert.True(rel1.affinity > 50);

            // Grudge path on second secret
            system.DiscoverSecret("secret_hunter_treeline_shot", currentDay: 17, sourceId: "engraved_lighter");
            var rel2 = relations.GetOrCreateRelationship("sv_hunter", "sv_listener");
            rel2.affinity = 50;

            bool grudged = system.ResolveInterpersonal(
                "secret_hunter_treeline_shot",
                currentDay: 18,
                forgive: false,
                confessorId: "sv_hunter",
                listenerId: "sv_listener",
                relations: relations);

            Assert.True(grudged);
            Assert.True(system.IsResolved("secret_hunter_treeline_shot"));
            Assert.True(rel2.affinity < 50);
        }

        [Fact]
        public void ConfessionSecretSystem_IdempotentLeverageResolution_PreventsDoubleAction()
        {
            var catalog = CreateLoadedCatalog();
            var system = new ConfessionSecretSystem(catalog);

            system.DiscoverSecret("secret_cook_ration_cache", currentDay: 20, sourceId: "recipe_tin");

            bool first = system.KeepSecret("secret_cook_ration_cache", currentDay: 21);
            Assert.True(first);

            // Second resolution attempt must fail
            bool second = system.ExposeSecret("secret_cook_ration_cache", currentDay: 22);
            Assert.False(second);
        }

        [Fact]
        public void ConfessionSecretSystem_CaptureAndRestoreState_Roundtrips()
        {
            var catalog = CreateLoadedCatalog();

            var systemA = new ConfessionSecretSystem(catalog);
            systemA.DiscoverSecret("secret_bunker_quartermaster_skimming", currentDay: 25, sourceId: "recipe_tin");
            systemA.KeepSecret("secret_bunker_quartermaster_skimming", currentDay: 26);

            var state = systemA.CaptureState();

            var systemB = new ConfessionSecretSystem(catalog);
            systemB.RestoreState(state);

            Assert.True(systemB.IsDiscovered("secret_bunker_quartermaster_skimming"));
            Assert.True(systemB.IsResolved("secret_bunker_quartermaster_skimming"));
            var record = systemB.GetChoice("secret_bunker_quartermaster_skimming");
            Assert.NotNull(record);
            Assert.Equal("keep", record.choice);
            Assert.Equal(26, record.dayResolved);
        }
    }
}
