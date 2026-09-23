// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Mods;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan47_65ModAgencyIntegrationTests
    {
        [Fact]
        public void Plan47_ModCompatibilityAndDependencySort_EndToEnd()
        {
            // 1. Version range evaluation checks
            Assert.True(ModCompatibilityEvaluator.EvaluateGameVersion("1.5.0", ">=1.0.0 <2.0.0", out string? err1));
            Assert.Null(err1);

            Assert.False(ModCompatibilityEvaluator.EvaluateGameVersion("2.1.0", ">=1.0.0 <2.0.0", out _));

            Assert.False(ModCompatibilityEvaluator.EvaluateGameVersion("1.0.0", ">=not_a_version", out string? malformedErr));
            Assert.NotNull(malformedErr);

            Assert.True(ModCompatibilityEvaluator.EvaluateGameVersion("1.2.3", "^1.2.0", out _));
            Assert.False(ModCompatibilityEvaluator.EvaluateGameVersion("2.0.0", "^1.2.0", out _));

            Assert.True(ModCompatibilityEvaluator.EvaluateGameVersion("1.2.9", "~1.2.0", out _));
            Assert.False(ModCompatibilityEvaluator.EvaluateGameVersion("1.3.0", "~1.2.0", out _));

            Assert.True(ModCompatibilityEvaluator.EvaluateModContractVersion(1, "==1", out string? err3));
            Assert.Null(err3);

            Assert.False(ModCompatibilityEvaluator.EvaluateModContractVersion(2, "==1", out _));

            // 2. Mod manifest configuration and compatibility metadata
            var coreManifest = new ModManifest
            {
                ModId = "mod_expansion_core",
                DisplayName = "Core Expansion Framework",
                Version = "1.0.0",
                LoadOrder = 10,
                AllowOverrides = false,
                GameRange = ">=1.0.0 <2.0.0",
                ModContractRange = "==1",
                PackType = "framework",
                Catalogs = new List<string> { "items.json" }
            };

            var contentManifest = new ModManifest
            {
                ModId = "mod_final_wishes_pack",
                DisplayName = "Final Wishes Expansion Pack",
                Version = "1.1.0",
                LoadOrder = 50,
                AllowOverrides = false,
                GameRange = ">=1.0.0",
                ModContractRange = "==1",
                PackType = "content_pack",
                Dependencies = new List<string> { "mod_expansion_core" },
                Catalogs = new List<string> { "final_wishes.json" }
            };

            Assert.Equal("mod_expansion_core", coreManifest.EffectiveModId);
            Assert.Equal(">=1.0.0 <2.0.0", coreManifest.EffectiveGameRange);
            Assert.Equal("==1", coreManifest.EffectiveModContractRange);

            Assert.Equal("mod_final_wishes_pack", contentManifest.EffectiveModId);
            Assert.Equal("content_pack", contentManifest.PackType);
            Assert.Single(contentManifest.Dependencies);
            Assert.Equal("mod_expansion_core", contentManifest.Dependencies[0]);

            // 3. Mod rejection codes contract verification
            var codes = (ModRejectionCode[])Enum.GetValues(typeof(ModRejectionCode));
            Assert.Contains(ModRejectionCode.IncompatibleGameVersion, codes);
            Assert.Contains(ModRejectionCode.IncompatibleModContract, codes);
            Assert.Contains(ModRejectionCode.MissingDependency, codes);
            Assert.Contains(ModRejectionCode.CircularDependency, codes);
            Assert.Contains(ModRejectionCode.AcceptancePipelineFailed, codes);
        }

        [Fact]
        public void Plan65_FinalWishAndEpilogueChronicle_EndToEnd()
        {
            // 1. FinalWishSystem lifecycle
            var wishSystem = new FinalWishSystem();
            wishSystem.RegisterWish("the_surgeon", FinalWishSystem.WishTeachLesson);

            bool stepCompletedFired = false;
            bool wishCompletedFired = false;
            float recordedMoraleBuff = 0f;

            wishSystem.OnFinalWishStepCompleted += (sId, stepId) => stepCompletedFired = true;
            wishSystem.OnFinalWishCompleted += sId => wishCompletedFired = true;
            wishSystem.ApplyPermanentShelterMoraleBuff = buff => recordedMoraleBuff = buff;

            wishSystem.DeclareTerminalPrognosis("dweller_dr_vance", "the_surgeon", isAlive: true);
            Assert.True(wishSystem.HasTerminalPrognosis("dweller_dr_vance"));
            Assert.True(wishSystem.HasActiveWish("dweller_dr_vance"));
            Assert.Equal(FinalWishSystem.WishTeachLesson, wishSystem.GetWishType("dweller_dr_vance"));
            Assert.Equal(0, wishSystem.GetStepsCompleted("dweller_dr_vance"));

            // Advance steps (teach_lesson requires 2 steps)
            wishSystem.AdvanceWishStep("dweller_dr_vance", "step_1_teach_apprentice");
            Assert.True(stepCompletedFired);
            Assert.Equal(1, wishSystem.GetStepsCompleted("dweller_dr_vance"));
            Assert.False(wishSystem.HasCompletedWish("dweller_dr_vance"));

            wishSystem.AdvanceWishStep("dweller_dr_vance", "step_2_pass_surgical_notes");
            Assert.True(wishCompletedFired);
            Assert.True(wishSystem.HasCompletedWish("dweller_dr_vance"));
            Assert.False(wishSystem.HasActiveWish("dweller_dr_vance"));
            Assert.Equal(FinalWishSystem.WishCompletedMoraleBuff, recordedMoraleBuff);

            // 2. Save/Restore roundtrip
            var save = wishSystem.CaptureState();
            var restoredSystem = new FinalWishSystem();
            restoredSystem.RestoreState(save);

            Assert.True(restoredSystem.HasCompletedWish("dweller_dr_vance"));
            Assert.Equal(2, restoredSystem.GetStepsCompleted("dweller_dr_vance"));

            // 3. Campaign Epilogue Chronicle generation
            var epilogueCatalog = new CampaignEpilogueCatalog
            {
                vignettes = new List<EpilogueVignetteDef>
                {
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_demo_thriving",
                        category = "demographics",
                        priority = 10,
                        min_survivors = 8,
                        max_survivors = 999,
                        max_starvation_deaths = 0,
                        title = "A Beacon in the Ash",
                        narrative = "The community stood resolute."
                    },
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_legacy_fulfilled",
                        category = "governance",
                        priority = 10,
                        min_survivors = 1,
                        max_survivors = 999,
                        title = "Honoring the Departed",
                        narrative = "Every fallen companion was remembered in stone and song."
                    }
                }
            };
            epilogueCatalog.Index();

            var epilogueEngine = new CampaignEpilogueEngine(epilogueCatalog);
            var snapshot = new CampaignEpilogueSnapshot
            {
                FinalDay = 100,
                SurvivorsAlive = 15,
                TotalCasualties = 2,
                StarvationDeaths = 0,
                DiseaseDeaths = 1,
                CampaignSeed = 999
            };

            var chronicle = epilogueEngine.GenerateChronicle(snapshot);
            Assert.NotNull(chronicle);
            Assert.Equal(100, chronicle.TotalDays);
            Assert.Contains(chronicle.Chapters, c => c.Category == "Demographics");
            Assert.Contains(chronicle.Chapters, c => c.Category == "Governance");
        }

        [Fact]
        public void Plan47_Plan65_CombinedEcosystem_ModdedWishesEpilogueJourney()
        {
            // Scenario:
            // 1. Mod Compatibility layer validates a modded content pack adding customized survivor wishes and epilogue vignettes.
            // 2. Metadata declares dependencies and compatible contract versions.
            // 3. In the bunker, an elder survivor facing terminal prognosis communicates their final wish.
            // 4. The community fulfills their final wish, buffering settlement morale.
            // 5. The campaign reaches day 150, and the epilogue engine synthesizes a triumphant chronicle honoring both survival and memory.

            // Step 1: Mod compatibility verification
            string targetGameVersion = "1.2.0";
            int targetModContractVersion = 1;

            var coreMod = new ModManifest
            {
                ModId = "mod_shelter_wishes_core",
                GameRange = ">=1.0.0 <2.0.0",
                ModContractRange = "==1",
                LoadOrder = 10
            };

            var addonMod = new ModManifest
            {
                ModId = "mod_expanded_narrative_epilogues",
                GameRange = "^1.2.0",
                ModContractRange = "==1",
                LoadOrder = 20,
                Dependencies = new List<string> { "mod_shelter_wishes_core" }
            };

            Assert.True(ModCompatibilityEvaluator.EvaluateGameVersion(targetGameVersion, coreMod.GameRange, out _));
            Assert.True(ModCompatibilityEvaluator.EvaluateGameVersion(targetGameVersion, addonMod.GameRange, out _));
            Assert.True(ModCompatibilityEvaluator.EvaluateModContractVersion(targetModContractVersion, coreMod.ModContractRange, out _));
            Assert.True(ModCompatibilityEvaluator.EvaluateModContractVersion(targetModContractVersion, addonMod.ModContractRange, out _));

            // Step 2: Metadata linkage validation
            Assert.Contains("mod_shelter_wishes_core", addonMod.Dependencies);
            Assert.True(addonMod.LoadOrder > coreMod.LoadOrder);

            // Step 3 & 4: Elder survivor terminal wish fulfillment
            var wishSystem = new FinalWishSystem();
            wishSystem.RegisterWish("the_historian", FinalWishSystem.WishDeliverLetter);

            float grantedMorale = 0f;
            wishSystem.ApplyPermanentShelterMoraleBuff = b => grantedMorale = b;

            wishSystem.DeclareTerminalPrognosis("dweller_elder_soren", "the_historian", isAlive: true);
            Assert.True(wishSystem.HasActiveWish("dweller_elder_soren"));

            wishSystem.AdvanceWishStep("dweller_elder_soren", "step_find_courier");
            wishSystem.AdvanceWishStep("dweller_elder_soren", "step_seal_dispatch");

            Assert.True(wishSystem.HasCompletedWish("dweller_elder_soren"));
            Assert.Equal(FinalWishSystem.WishCompletedMoraleBuff, grantedMorale);

            // Step 5: Endgame Epilogue Chronicle Synthesis
            var epilogueCatalog = new CampaignEpilogueCatalog
            {
                vignettes = new List<EpilogueVignetteDef>
                {
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_thriving_bastion",
                        category = "demographics",
                        priority = 20,
                        min_survivors = 10,
                        max_survivors = 999,
                        title = "The Bastion of Ashfall",
                        narrative = "Generations took root beneath the reinforced vaults."
                    }
                }
            };
            epilogueCatalog.Index();

            var epilogueEngine = new CampaignEpilogueEngine(epilogueCatalog);
            var chronicle = epilogueEngine.GenerateChronicle(new CampaignEpilogueSnapshot
            {
                FinalDay = 150,
                SurvivorsAlive = 18,
                TotalCasualties = 1,
                StarvationDeaths = 0,
                DiseaseDeaths = 0,
                CampaignSeed = 12345
            });

            Assert.NotNull(chronicle);
            Assert.Equal(150, chronicle.TotalDays);
            Assert.Single(chronicle.Chapters);
            Assert.Equal("The Bastion of Ashfall", chronicle.Chapters[0].Title);
        }
    }
}
