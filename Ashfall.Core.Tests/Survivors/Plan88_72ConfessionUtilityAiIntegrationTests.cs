// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Phantoms;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class Plan88_72ConfessionUtilityAiIntegrationTests
    {
        private readonly string _dataDir;
        private readonly IJsonSerializer _serializer = new SystemTextJsonSerializer();

        public Plan88_72ConfessionUtilityAiIntegrationTests()
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

        private ConfessionSecretCatalog CreateLoadedConfessionCatalog()
        {
            var catalog = new ConfessionSecretCatalog();
            string filePath = Path.Combine(_dataDir, "confession_secrets.json");
            Assert.True(File.Exists(filePath), $"confession_secrets.json must exist at {filePath}");
            catalog.Load(File.ReadAllText(filePath), _serializer);
            return catalog;
        }

        private List<UtilityActionDef> LoadUtilityActionCatalog()
        {
            var actions = UtilityActionCatalogLoader.Load(
                _dataDir, new FileSystemIO(), _serializer);
            Assert.NotNull(actions);
            return actions;
        }

        [Fact]
        public void Catalogs_LoadCleanly_WithValidSchemaAndReferentialIntegrity()
        {
            // Plan 88: Confession secrets
            var confessionCatalog = CreateLoadedConfessionCatalog();
            Assert.True(confessionCatalog.AllSecrets.Count >= 20,
                $"Expected at least 20 confession secrets, found {confessionCatalog.AllSecrets.Count}");

            foreach (var secret in confessionCatalog.AllSecrets)
            {
                Assert.False(string.IsNullOrWhiteSpace(secret.secret_id), "secret_id must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(secret.secret_title), "secret_title must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(secret.secret_text), "secret_text must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(secret.category), "category must not be empty");
                Assert.True(secret.forgiveness_affinity != 0 || secret.grudge_affinity != 0,
                    $"Secret {secret.secret_id} must specify affinity consequences");
            }

            // Plan 72: Utility actions
            var utilityActions = LoadUtilityActionCatalog();
            Assert.Equal(20, utilityActions.Count);

            foreach (var action in utilityActions)
            {
                Assert.False(string.IsNullOrWhiteSpace(action.id), "action id must not be empty");
                Assert.StartsWith("action_", action.id);
                Assert.False(string.IsNullOrWhiteSpace(action.displayName), "displayName must not be empty");
                Assert.True(action.baseScore > 0f, $"Action {action.id} must have positive baseScore");
                Assert.True(action.weight > 0f, $"Action {action.id} must have positive weight");
                Assert.NotNull(action.curvePoints);
                Assert.True(action.curvePoints.Length >= 2, $"Action {action.id} must define curve points");
            }
        }

        [Fact]
        public void ConfessionResolution_Forgiveness_RestoresSocialBonds_AndEnablesHighProductivityActions()
        {
            var confessionCatalog = CreateLoadedConfessionCatalog();
            var confessionSystem = new ConfessionSecretSystem(confessionCatalog);
            var relations = new SurvivorRelationsSystem(new SeededRng(101));

            string secretId = "secret_soldier_civilian_order";
            string confessor = "sv_soldier";
            string listener = "sv_nurse";

            // 1. Discover secret
            bool discovered = confessionSystem.DiscoverSecret(secretId, currentDay: 4, sourceId: "dog_tags");
            Assert.True(discovered);
            Assert.True(confessionSystem.IsDiscovered(secretId));
            Assert.False(confessionSystem.IsResolved(secretId));

            // 2. Resolve via interpersonal forgiveness
            bool resolved = confessionSystem.ResolveInterpersonal(
                secretId,
                currentDay: 5,
                forgive: true,
                confessorId: confessor,
                listenerId: listener,
                relations: relations);

            Assert.True(resolved);
            Assert.True(confessionSystem.IsResolved(secretId));

            var choice = confessionSystem.GetChoice(secretId);
            Assert.NotNull(choice);
            Assert.Equal("forgive", choice.choice);
            Assert.Equal(5, choice.dayResolved);

            // Trust and affinity improved
            var rel = relations.GetOrCreateRelationship(confessor, listener);
            Assert.NotNull(rel);
            Assert.True(rel.trust > 0f, $"Expected positive trust after forgiveness, got {rel.trust}");
            Assert.True(rel.affinity > 0f, $"Expected positive affinity after forgiveness, got {rel.affinity}");

            // 3. In good psychological standing, survivor autonomous utility actions evaluate high productivity
            var actions = LoadUtilityActionCatalog();
            var aiSystem = new UtilityAiSystem();
            var rng = new SeededRng(202);

            var context = new AIActionContext
            {
                SurvivorId = confessor,
                IsAlive = true,
                Fatigue = 10f, // Well-rested
                CraftingSkill = 0.8f,
                IsListless = false,
                HasHazmat = false
            };

            var chosenAction = aiSystem.SelectAction(context, actions, rng);
            Assert.NotNull(chosenAction);
            Assert.NotEqual("action_rest", chosenAction.id); // Not forced to rest when well-rested
            Assert.False(string.IsNullOrEmpty(chosenAction.id));
        }

        [Fact]
        public void ConfessionResolution_ExposureOrGuilt_IncreasesFatigue_GatingLaborAndBiasingRest()
        {
            var confessionCatalog = CreateLoadedConfessionCatalog();
            var confessionSystem = new ConfessionSecretSystem(confessionCatalog);
            var guiltSystem = new GuiltInsomniaSystem();

            string secretId = "secret_pharmacist_stolen_morphine";
            string confessor = "the_pharmacist";

            confessionSystem.DiscoverSecret(secretId, currentDay: 10, sourceId: "morphine_vial");

            // Expose secret -> records guilt
            bool exposed = confessionSystem.ExposeSecret(
                secretId,
                currentDay: 11,
                needs: null,
                guilt: guiltSystem);

            Assert.True(exposed);
            Assert.True(confessionSystem.IsResolved(secretId));
            Assert.True(guiltSystem.GetGuiltSourceCount(confessor) > 0);

            // High guilt/insomnia induces heavy dweller exhaustion/fatigue
            float exhaustedFatigue = 95.0f;

            var actions = LoadUtilityActionCatalog();
            var aiSystem = new UtilityAiSystem();

            var exhaustedContext = new AIActionContext
            {
                SurvivorId = confessor,
                IsAlive = true,
                Fatigue = exhaustedFatigue,
                CraftingSkill = 0.5f,
                IsListless = true
            };

            // Inspect fatigue-gated actions: heavy labor must evaluate to 0
            var weighAction = actions.Find(a => a.id == "action_weigh_goods");
            Assert.NotNull(weighAction);
            Assert.Equal(85.0f, weighAction.fatigueGate);
            Assert.Equal(0f, weighAction.EvaluateRaw(exhaustedContext));

            var repairAction = actions.Find(a => a.id == "action_repair_equipment");
            Assert.NotNull(repairAction);
            Assert.Equal(80.0f, repairAction.fatigueGate);
            Assert.Equal(0f, repairAction.EvaluateRaw(exhaustedContext));

            // Rest action has no fatigue gate and remains viable
            var restAction = actions.Find(a => a.id == "action_rest");
            Assert.NotNull(restAction);
            Assert.Equal(0f, restAction.fatigueGate);
            Assert.True(restAction.EvaluateRaw(exhaustedContext) > 0f);

            // Utility AI action selection should bias towards rest
            var chosen = aiSystem.SelectAction(exhaustedContext, new[] { weighAction, repairAction, restAction }, new SeededRng(303));
            Assert.NotNull(chosen);
            Assert.Equal("action_rest", chosen.id);
        }

        [Fact]
        public void ConfessionAndUtilityAi_DeterministicReplayAndSaveRestoreRoundTrip()
        {
            var confessionCatalog = CreateLoadedConfessionCatalog();
            var originalSystem = new ConfessionSecretSystem(confessionCatalog);

            originalSystem.DiscoverSecret("secret_mother_child_left", currentDay: 3, sourceId: "childs_mitten");
            originalSystem.DiscoverSecret("secret_surgeon_lost_patient", currentDay: 4, sourceId: "silver_scalpel");

            originalSystem.KeepSecret("secret_mother_child_left", currentDay: 5);

            // Capture state
            var state = originalSystem.CaptureState();
            Assert.NotNull(state);
            Assert.Contains("secret_mother_child_left", state.discoveredSecretIds);
            Assert.Contains("secret_surgeon_lost_patient", state.discoveredSecretIds);
            Assert.Contains("secret_mother_child_left", state.resolvedSecretIds);
            Assert.DoesNotContain("secret_surgeon_lost_patient", state.resolvedSecretIds);

            // Restore state into new system
            var restoredSystem = new ConfessionSecretSystem(confessionCatalog);
            restoredSystem.RestoreState(state);

            Assert.True(restoredSystem.IsDiscovered("secret_mother_child_left"));
            Assert.True(restoredSystem.IsDiscovered("secret_surgeon_lost_patient"));
            Assert.True(restoredSystem.IsResolved("secret_mother_child_left"));
            Assert.False(restoredSystem.IsResolved("secret_surgeon_lost_patient"));

            var choice = restoredSystem.GetChoice("secret_mother_child_left");
            Assert.NotNull(choice);
            Assert.Equal("keep", choice.choice);
            Assert.Equal(5, choice.dayResolved);

            // Deterministic replay check on Utility AI selection
            var actions = LoadUtilityActionCatalog();
            var aiSystem = new UtilityAiSystem();

            var context = new AIActionContext
            {
                SurvivorId = "sv_deterministic",
                IsAlive = true,
                Fatigue = 30f,
                CraftingSkill = 0.6f
            };

            var run1 = aiSystem.SelectAction(context, actions, new SeededRng(777));
            var run2 = aiSystem.SelectAction(context, actions, new SeededRng(777));

            Assert.NotNull(run1);
            Assert.NotNull(run2);
            Assert.Equal(run1.id, run2.id);
        }
    }
}
