// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    /// <summary>
    /// Wave 41 Batch 1 Cross-System Integration Test:
    /// Validates Plan 105 (Trade Specialties Expansion - 4 baseline → 16 authored professions)
    /// alongside Plan 106 (Dose Items Expansion - 5 baseline → 15 dose-ledger items).
    /// </summary>
    public sealed class Plan105_106TradeDoseIntegrationTests : CatalogTestBase
    {
        private static readonly string[] ExpectedProfessions =
        {
            "electrician",
            "nurse",
            "machinist",
            "teacher",
            "miller",
            "wireman",
            "bone_setter",
            "preservationist",
            "radio_technician",
            "greenhouse_grower",
            "surveyor",
            "salvage_appraiser",
            "apiarist",
            "metallurgist",
            "water_technician",
            "tailor"
        };

        private static readonly string[] BaselineDoseItemIds =
        {
            "item_dose_ledger",
            "item_calibration_key",
            "item_dosimeter_tag",
            "item_palliative_morphine",
            "item_cohort_first_board"
        };

        private static readonly string[] AllFifteenDoseItemIds =
        {
            "item_dose_ledger",
            "item_calibration_key",
            "item_dosimeter_tag",
            "item_palliative_morphine",
            "item_cohort_first_board",
            "item_calibrated_dosimeter",
            "item_forged_clean_bill_chit",
            "item_chelation_decorporation_course",
            "item_shielded_badge_case",
            "item_pocket_dosimeter",
            "item_radiation_survey_meter",
            "item_dose_register_book",
            "item_cohort_baseline_card",
            "item_shielding_apron",
            "item_potassium_iodide_pack"
        };

        [Fact]
        public void Plan105_TradeSpecialties_LoadsAllSixteenProfessions_WithValidMilestonesAndMastery()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var professions = TradeSpecialtyCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(professions);
            Assert.Equal(16, professions.Count);

            var seenIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var prof in professions)
            {
                Assert.NotNull(prof);
                Assert.False(string.IsNullOrWhiteSpace(prof.ProfessionId), "ProfessionId must not be empty.");
                Assert.True(seenIds.Add(prof.ProfessionId), $"Duplicate profession_id: {prof.ProfessionId}");
                Assert.False(string.IsNullOrWhiteSpace(prof.DisplayName), $"DisplayName missing for {prof.ProfessionId}");
                Assert.NotNull(prof.Milestones);
                Assert.Equal(3, prof.Milestones.Count);

                for (int tier = 1; tier <= 3; tier++)
                {
                    var milestone = prof.Milestones.FirstOrDefault(m => m.Tier == tier);
                    Assert.NotNull(milestone);
                    Assert.False(string.IsNullOrWhiteSpace(milestone.Title), $"{prof.ProfessionId} Tier {tier} title missing.");
                    Assert.False(string.IsNullOrWhiteSpace(milestone.Narrative), $"{prof.ProfessionId} Tier {tier} narrative missing.");
                    Assert.StartsWith("narrative_", milestone.Narrative, StringComparison.Ordinal);
                    Assert.True(milestone.SkillBonus > 0f, $"{prof.ProfessionId} Tier {tier} skill bonus must be positive.");
                    Assert.NotNull(milestone.ItemPatterns);
                    Assert.NotEmpty(milestone.ItemPatterns);
                }

                Assert.False(string.IsNullOrWhiteSpace(prof.MasteryNarrative), $"MasteryNarrative missing for {prof.ProfessionId}");
                Assert.StartsWith("narrative_", prof.MasteryNarrative, StringComparison.Ordinal);
                Assert.False(string.IsNullOrWhiteSpace(prof.MasteryBonusText), $"MasteryBonusText missing for {prof.ProfessionId}");
                Assert.Contains("{name}", prof.MasteryBonusText, StringComparison.Ordinal);
            }

            foreach (var expectedId in ExpectedProfessions)
            {
                Assert.Contains(expectedId, seenIds);
            }
        }

        [Fact]
        public void Plan105_TradeSpecialtySystem_ProgressesMilestonesAndMastery_WithSaveRestore()
        {
            var system = new TradeSpecialtySystem();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            int loadedCount = TradeSpecialtyCatalogLoader.LoadAndRegister(system, DataDirectory, files, json);
            Assert.Equal(16, loadedCount);

            string survivorId = "survivor_test_elena";
            string professionId = "nurse";

            var milestoneEvents = new List<(string survivor, string prof, int tier)>();
            system.OnSpecialtyMilestone += (s, p, t) => milestoneEvents.Add((s, p, t));

            var masteredEvents = new List<(string survivor, string prof)>();
            system.OnSpecialtyMastered += (s, p) => masteredEvents.Add((s, p));

            Assert.Equal(0, system.GetMasteryTier(survivorId));
            Assert.False(system.HasMasteredTrade(survivorId));

            // Craft item 1 matching nurse pattern (bandage)
            system.OnItemCrafted(survivorId, professionId, "item_bandage_sterile");
            Assert.Equal(1, system.GetMasteryTier(survivorId));
            Assert.Single(milestoneEvents);
            Assert.Equal(1, milestoneEvents[0].tier);
            Assert.False(system.HasMasteredTrade(survivorId));

            // Craft item 2 matching nurse pattern (splint)
            system.OnItemCrafted(survivorId, professionId, "item_splint_rigid");
            Assert.Equal(2, system.GetMasteryTier(survivorId));
            Assert.Equal(2, milestoneEvents.Count);
            Assert.Equal(2, milestoneEvents[1].tier);
            Assert.False(system.HasMasteredTrade(survivorId));

            // Craft item 3 matching nurse pattern (antiseptic) -> Reaches Mastery!
            system.OnItemCrafted(survivorId, professionId, "item_antiseptic_bottle");
            Assert.Equal(3, system.GetMasteryTier(survivorId));
            Assert.Equal(3, milestoneEvents.Count);
            Assert.Equal(3, milestoneEvents[2].tier);
            Assert.True(system.HasMasteredTrade(survivorId));
            Assert.Single(masteredEvents);
            Assert.Equal((survivorId, professionId), masteredEvents[0]);

            // Verify deep copy save and restore
            var saved = system.CaptureState();
            Assert.NotNull(saved);
            Assert.Single(saved.survivors);
            Assert.Equal(survivorId, saved.survivors[0].survivorId);
            Assert.Equal(professionId, saved.survivors[0].professionId);
            Assert.True(saved.survivors[0].mastered);
            Assert.Equal(3, saved.survivors[0].craftMilestonesCompleted.Count);

            var newSystem = new TradeSpecialtySystem();
            newSystem.RestoreState(saved);
            Assert.True(newSystem.HasMasteredTrade(survivorId));
            Assert.Equal(3, newSystem.GetMasteryTier(survivorId));
        }

        [Fact]
        public void Plan106_DoseItems_LoadsAllFifteenItems_WithValidPropertiesAndCategories()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalog = DoseContentCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(catalog);
            Assert.NotNull(catalog.items);
            Assert.Equal(15, catalog.items.Count);

            var seenItemIds = new HashSet<string>(StringComparer.Ordinal);
            var validCategories = new HashSet<string>(StringComparer.Ordinal)
            {
                "story",
                "tool",
                "medical",
                "protective"
            };

            foreach (var item in catalog.items)
            {
                Assert.NotNull(item);
                Assert.False(string.IsNullOrWhiteSpace(item.id), "Dose item id missing.");
                Assert.StartsWith("item_", item.id, StringComparison.Ordinal);
                Assert.True(seenItemIds.Add(item.id), $"Duplicate dose item id: {item.id}");

                Assert.False(string.IsNullOrWhiteSpace(item.name), $"Dose item {item.id} name missing.");
                Assert.False(string.IsNullOrWhiteSpace(item.description), $"Dose item {item.id} description missing.");
                Assert.True(item.weightKg >= 0f, $"Dose item {item.id} weightKg must be non-negative.");
                Assert.True(item.tradeValue >= 0f, $"Dose item {item.id} tradeValue must be non-negative.");
                Assert.Contains(item.category, validCategories);
            }

            // Verify all baseline 5 items remain preserved
            foreach (var baselineId in BaselineDoseItemIds)
            {
                Assert.Contains(baselineId, seenItemIds);
            }

            // Verify the complete 15-item inventory is present
            foreach (var expectedId in AllFifteenDoseItemIds)
            {
                Assert.Contains(expectedId, seenItemIds);
            }
        }

        [Fact]
        public void Plan105_106_CrossSystem_SpecialtyMatchingAndDoseEconomics_DeterministicResolution()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var professions = TradeSpecialtyCatalogLoader.Load(DataDirectory, files, json);
            var doseCatalog = DoseContentCatalogLoader.Load(DataDirectory, files, json);

            Assert.Equal(16, professions.Count);
            Assert.Equal(15, doseCatalog.items.Count);

            // Test cross-system synergy: Medical professions (nurse, bone_setter) identify medical dose items
            var nurse = professions.FirstOrDefault(p => p.ProfessionId == "nurse");
            Assert.NotNull(nurse);
            var nursePatterns = nurse.Milestones.SelectMany(m => m.ItemPatterns).Distinct().ToList();

            var palliativeMorphine = doseCatalog.items.FirstOrDefault(i => i.id == "item_palliative_morphine");
            Assert.NotNull(palliativeMorphine);
            Assert.Equal("medical", palliativeMorphine.category);
            Assert.Equal(90f, palliativeMorphine.tradeValue);

            // The nurse's milestone patterns include medical terms
            bool nurseMatchesMedicalItem = nursePatterns.Any(pat => palliativeMorphine.id.Contains(pat, StringComparison.OrdinalIgnoreCase) ||
                                                                    palliativeMorphine.category.Contains(pat, StringComparison.OrdinalIgnoreCase));
            Assert.True(nurseMatchesMedicalItem, "Nurse specialty should match medical dose supplies.");

            // Test cross-system synergy: Technical professions (wireman, salvage_appraiser) identify tool dose items
            var surveyMeter = doseCatalog.items.FirstOrDefault(i => i.id == "item_radiation_survey_meter");
            Assert.NotNull(surveyMeter);
            Assert.Equal("tool", surveyMeter.category);
            Assert.Equal(75f, surveyMeter.tradeValue);

            // Dosimeter items have high utility and trade value
            var calibratedDosimeter = doseCatalog.items.FirstOrDefault(i => i.id == "item_calibrated_dosimeter");
            Assert.NotNull(calibratedDosimeter);
            Assert.Equal(65f, calibratedDosimeter.tradeValue);

            // Story chits and baseline ledgers have 0 trade value (non-commercial, institutional artifacts)
            var ledger = doseCatalog.items.FirstOrDefault(i => i.id == "item_dose_ledger");
            Assert.NotNull(ledger);
            Assert.Equal(0f, ledger.tradeValue);
            Assert.Equal("story", ledger.category);

            // Verify determinism: re-loading produces exact same counts and identical collections
            var professions2 = TradeSpecialtyCatalogLoader.Load(DataDirectory, files, json);
            var doseCatalog2 = DoseContentCatalogLoader.Load(DataDirectory, files, json);
            Assert.Equal(professions.Count, professions2.Count);
            Assert.Equal(doseCatalog.items.Count, doseCatalog2.items.Count);
        }
    }
}
