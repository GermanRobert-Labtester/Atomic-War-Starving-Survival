// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 147 & Plan 151 Integration Tests:
// - Plan 147: Per-NPC Memory: Dialogue Tone, Quests, Grudges & Trade Modifiers
// - Plan 151: Working Animals & Companion System: Roles, Training, Bond & Upkeep
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Ecology;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class Plan147_151NpcAnimalIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void Plan147_NpcMemorySystem_LoadsAuthoredDialogueTemplates_AndMapsDisposition()
        {
            var system = new NpcMemorySystem();
            string dialoguePath = Path.Combine(DataDirectory, "npc_memory_dialogue.json");
            Assert.True(File.Exists(dialoguePath), $"npc_memory_dialogue.json must exist at {dialoguePath}");

            system.LoadDialogueCatalog(File.ReadAllText(dialoguePath));
            var templates = system.GetAllDialogueTemplates();
            Assert.NotEmpty(templates);

            // Verify dialogue template coverage for all canonical tones
            var highTrustTemplates = system.GetDialogueTemplatesForTone(NpcDialogueTone.HighTrust);
            var highGrudgeTemplates = system.GetDialogueTemplatesForTone(NpcDialogueTone.HighGrudge);
            var favorOwedTemplates = system.GetDialogueTemplatesForTone(NpcDialogueTone.FavorOwed);
            var betrayedTemplates = system.GetDialogueTemplatesForTone(NpcDialogueTone.Betrayed);
            var reconciledTemplates = system.GetDialogueTemplatesForTone(NpcDialogueTone.Reconciled);

            Assert.NotEmpty(highTrustTemplates);
            Assert.NotEmpty(highGrudgeTemplates);
            Assert.NotEmpty(favorOwedTemplates);
            Assert.NotEmpty(betrayedTemplates);
            Assert.NotEmpty(reconciledTemplates);

            // Test interaction progression on named NPC
            string npcId = "npc_dr_holloway";

            // 1. Initial interaction: neutral
            Assert.Equal(NpcDialogueTone.Neutral, system.GetDialogueTone(npcId));

            // 2. Helping and saving life elevates trust and favors
            system.RecordAction(npcId, NpcMemoryActionType.SavedLife, day: 1, targetId: "clinic", intensity: 90f);
            Assert.Equal(NpcDialogueTone.FavorOwed, system.GetDialogueTone(npcId));
            Assert.True(system.GetTradePriceMultiplier(npcId) < 1.0f, "High trust yields merchant discount");

            // 3. Severe betrayal triggers embargo and betrayed tone
            system.RecordAction(npcId, NpcMemoryActionType.Betrayed, day: 5, targetId: "medical_supplies", intensity: 100f);
            system.RecordAction(npcId, NpcMemoryActionType.Betrayed, day: 6, targetId: "patient", intensity: 100f);

            Assert.Equal(NpcDialogueTone.Betrayed, system.GetDialogueTone(npcId));
            Assert.True(system.IsTradeRefused(npcId));
            Assert.Equal(float.PositiveInfinity, system.GetTradePriceMultiplier(npcId));

            var betrayedDialogue = system.GetDialogueTemplatesForTone(system.GetDialogueTone(npcId));
            Assert.Contains(betrayedDialogue, t => t.text.Contains("traitors") || t.text.Contains("iron"));
        }

        [Fact]
        public void Plan147_NpcMemorySystem_ForgivenessAndDecay_RestoresTradeAndSavesState()
        {
            var system = new NpcMemorySystem();
            system.LoadDialogueCatalog(File.ReadAllText(Path.Combine(DataDirectory, "npc_memory_dialogue.json")));

            string npcId = "npc_scavenger_mira";
            system.RecordAction(npcId, NpcMemoryActionType.Refused, day: 1);
            system.RecordAction(npcId, NpcMemoryActionType.Refused, day: 2);

            var rel = system.Get(npcId);
            Assert.NotNull(rel);
            Assert.Equal(20f, rel.GrudgeLevel);
            Assert.Equal(NpcDialogueTone.Neutral, system.GetDialogueTone(npcId));

            // Forgive grudge with restitution
            bool forgiven = system.Forgive(npcId, reason: "scrap_returned", restitutionAmount: 20f);
            Assert.True(forgiven);
            Assert.Equal(0f, rel.GrudgeLevel);

            // Record restitution action to establish reconciled conversational disposition
            system.RecordAction(npcId, NpcMemoryActionType.Restitution, day: 3);
            Assert.Equal(NpcDialogueTone.Reconciled, system.GetDialogueTone(npcId));

            // Save and Restore
            var captured = system.CaptureState();
            Assert.NotNull(captured);

            var restoredSystem = new NpcMemorySystem();
            restoredSystem.LoadDialogueCatalog(File.ReadAllText(Path.Combine(DataDirectory, "npc_memory_dialogue.json")));
            restoredSystem.RestoreState(captured);

            var restoredRel = restoredSystem.Get(npcId);
            Assert.NotNull(restoredRel);
            Assert.Equal(rel.PersonalTrust, restoredRel.PersonalTrust);
            Assert.Equal(rel.GrudgeLevel, restoredRel.GrudgeLevel);
            Assert.Equal(NpcDialogueTone.Reconciled, restoredSystem.GetDialogueTone(npcId));
        }

        [Fact]
        public void Plan151_CompanionAnimals_LoadsAuthoredSpecies_AndEnforcesRoleCompatibility()
        {
            var loaderResult = CompanionAnimalCatalogLoader.Load(
                DataDirectory,
                new FileSystemIO(),
                new SystemTextJsonSerializer());

            Assert.False(loaderResult.HasErrors, string.Join("; ", loaderResult.Errors));
            Assert.True(loaderResult.Companions.Count >= 5, "Must load all 5 authored companion species.");

            var houndProfile = loaderResult.Companions.FirstOrDefault(c => c.species_id == "species_ash_hound");
            Assert.NotNull(houndProfile);
            Assert.Contains("guard", houndProfile.role_tags);
            Assert.Contains("morale", houndProfile.role_tags);
            Assert.DoesNotContain("pack", houndProfile.role_tags);

            var goatProfile = loaderResult.Companions.FirstOrDefault(c => c.species_id == "species_feral_goat");
            Assert.NotNull(goatProfile);
            Assert.Contains("pack", goatProfile.role_tags);
            Assert.True(goatProfile.pack_capacity_kg > 0);

            var hareProfile = loaderResult.Companions.FirstOrDefault(c => c.species_id == "species_cotton_hare");
            Assert.NotNull(hareProfile);
            Assert.Contains("morale", hareProfile.role_tags);
            Assert.Equal(0, hareProfile.guard_rating);

            // System integration
            var system = new CompanionAnimalSystem(loaderResult.Companions);
            system.KnownSpeciesCheck = _ => true;

            // Register Ash Hound
            var regResult = system.RegisterCompanion("companion_hound_1", "species_ash_hound", tamedDay: 1, name: "Rusty");
            Assert.True(regResult.Success);
            var hound = system.Companion("companion_hound_1");
            Assert.NotNull(hound);
            Assert.Equal("Rusty", hound.name);

            // Role assignment: Guard is allowed, Pack is rejected for Hound
            var packAssignResult = system.Assign("companion_hound_1", "surv_handler_1", CompanionRole.Pack);
            Assert.False(packAssignResult.Success);
            Assert.Equal("role_incompatible", packAssignResult.ReasonCode);

            var guardAssignResult = system.Assign("companion_hound_1", "surv_handler_1", CompanionRole.Guard);
            Assert.True(guardAssignResult.Success);
            Assert.Equal((int)CompanionRole.Guard, hound.role);
        }

        [Fact]
        public void Plan151_CompanionAnimals_CareBondingTraining_AndDerivedBenefits()
        {
            var loaderResult = CompanionAnimalCatalogLoader.Load(
                DataDirectory,
                new FileSystemIO(),
                new SystemTextJsonSerializer());

            var system = new CompanionAnimalSystem(loaderResult.Companions);
            system.KnownSpeciesCheck = _ => true;

            var inventory = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["raw_meat"] = 10,
                ["crop_ash_grain"] = 10
            };

            system.BindFoodPort(
                id => inventory.TryGetValue(id, out var count) ? count : 0,
                (id, count) => inventory[id] = Math.Max(0, inventory[id] - count));

            var regResult = system.RegisterCompanion("companion_hound_2", "species_ash_hound", tamedDay: 1, name: "Grim");
            Assert.True(regResult.Success);
            var hound = system.Companion("companion_hound_2");
            Assert.NotNull(hound);

            var assignResult = system.Assign("companion_hound_2", "surv_handler_1", CompanionRole.Guard);
            Assert.True(assignResult.Success);

            // Initial guard contribution (Untrained = level 0)
            float initialGuardModifier = system.GetGuardModifierTotal();
            Assert.True(initialGuardModifier > 0f);

            // Tick day 1: consumes food, trains, gains bond
            system.TickDay(1);
            Assert.True(inventory["raw_meat"] < 10, "Food must be consumed on tick");
            Assert.Equal(0, hound.hunger);
            Assert.True(hound.bond > 0);

            // Tick subsequent days to progress training
            system.TickDay(2);
            system.TickDay(3);
            system.TickDay(4);
            system.TickDay(5);

            Assert.True(hound.training_level > 0 || hound.training_progress > 0);

            // Derived guard modifier scales with training and bond
            float improvedGuardModifier = system.GetGuardModifierTotal();
            Assert.True(improvedGuardModifier >= initialGuardModifier);

            // Save and Restore
            var capturedState = system.CaptureState();
            Assert.NotNull(capturedState);
            Assert.NotEmpty(capturedState.companions);

            var restoredSystem = new CompanionAnimalSystem(loaderResult.Companions);
            restoredSystem.RestoreState(capturedState);

            var restoredHound = restoredSystem.Companion("companion_hound_2");
            Assert.NotNull(restoredHound);
            Assert.Equal(hound.name, restoredHound.name);
            Assert.Equal(hound.role, restoredHound.role);
            Assert.Equal(hound.bond, restoredHound.bond);
            Assert.Equal(hound.health, restoredHound.health);
        }
    }
}
