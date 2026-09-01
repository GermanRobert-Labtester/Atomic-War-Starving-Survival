using System;
using System.IO;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.BodyMind
{
    public class Plan27BodyMindTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string dataDir = string.Empty;
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) { dataDir = candidate; break; }
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return dataDir;
        }

        [Fact]
        public void DoseContent_ContainsTwelveQuestsNineItemsFiveLocations()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.True(catalog.quests.Count >= 12, $"Expected >= 12 dose quests, found {catalog.quests.Count}");
            Assert.True(catalog.items.Count >= 9, $"Expected >= 9 dose items, found {catalog.items.Count}");
            Assert.True(catalog.locations.Count >= 5, $"Expected >= 5 dose locations, found {catalog.locations.Count}");

            // Verify specific new items
            var itemIds = new HashSet<string>();
            foreach (var item in catalog.items) itemIds.Add(item.id);
            Assert.Contains("item_calibrated_dosimeter", itemIds);
            Assert.Contains("item_forged_clean_bill_chit", itemIds);
            Assert.Contains("item_chelation_decorporation_course", itemIds);
            Assert.Contains("item_shielded_badge_case", itemIds);

            // Verify specific new locations
            var locIds = new HashSet<string>();
            foreach (var loc in catalog.locations) locIds.Add(loc.id);
            Assert.Contains("loc_the_register_hall", locIds);
            Assert.Contains("loc_the_screening_station", locIds);
        }

        [Fact]
        public void DoseMigration_RecognizesAllCanonicalQuestlines()
        {
            Assert.True(DoseQuestMigration.CanonicalQuestlineIds.Length >= 12);
            Assert.True(DoseQuestMigration.IsDoseQuestline("quest_the_falsified_reading"));
            Assert.True(DoseQuestMigration.IsDoseQuestline("quest_the_stolen_dosimeter"));
            Assert.True(DoseQuestMigration.IsDoseQuestline("quest_child_over_the_limit"));
            Assert.True(DoseQuestMigration.IsDoseQuestline("quest_the_register_audit"));
            Assert.True(DoseQuestMigration.IsDoseQuestline("quest_black_market_clean_bill"));
            Assert.True(DoseQuestMigration.IsDoseQuestline("quest_the_broken_calibration_chain"));
            Assert.True(DoseQuestMigration.IsDoseQuestline("quest_exposure_for_the_essential_worker"));
            Assert.True(DoseQuestMigration.IsDoseQuestline("quest_the_missing_page"));
        }

        [Fact]
        public void ForgeryInvariant_DoesNotMutatePhysicalRadiationDose()
        {
            var radState = new SurvivorRadState { Id = "sv_infiltrator", RadiationDose = 75f, LifetimeRadiationExposure = 400f };
            var doseLedger = new DoseLedgerSystem();
            doseLedger.AssignDosimeter(radState.Id, "tag_infiltrator");
            doseLedger.BookReading(radState.Id, 1, 400f, "fallout", false, false, false, new SeededRng(1));

            Assert.Equal(DoseLedgerSystem.BandRed, DoseLedgerSystem.BandFor(doseLedger.GetCumulative(radState.Id)));
            Assert.Equal(DoseLedgerSystem.BandRed, doseLedger.GetAdministrativeBand(radState.Id));

            // Issue forged clean-bill chit
            doseLedger.SetForgedCleanBill(radState.Id, true);

            // Administrative classification is Green
            Assert.Equal(DoseLedgerSystem.BandGreen, doseLedger.GetAdministrativeBand(radState.Id));

            // True physical radiation state is completely untouched!
            Assert.Equal(75f, radState.RadiationDose);
            Assert.Equal(400f, radState.LifetimeRadiationExposure);
            Assert.Equal(400f, doseLedger.GetCumulative(radState.Id));
        }

        [Fact]
        public void Autopsy_CatalogParsesNineProceduresAndYieldsResearch()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            string path = fileIO.Combine(dataDir, "autopsy_procedures.json");
            Assert.True(fileIO.FileExists(path));

            var procedures = AutopsyProcedureCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.True(procedures.Count >= 9, $"Expected >= 9 autopsy procedures, found {procedures.Count}");

            var procMap = new Dictionary<string, AutopsyProcedure>();
            foreach (var p in procedures) procMap[p.procedure_id] = p;

            Assert.Contains("procedure_rad_pathology", procMap.Keys);
            Assert.Contains("procedure_toxicology", procMap.Keys);
            Assert.Contains("procedure_containment_autopsy", procMap.Keys);
            Assert.Contains("procedure_blunt_trauma", procMap.Keys);
            Assert.Contains("procedure_ballistic_forensics", procMap.Keys);
            Assert.Contains("procedure_respiratory_contamination", procMap.Keys);
            Assert.Contains("procedure_hypothermia_pathology", procMap.Keys);
            Assert.Contains("procedure_spore_infection_isolation", procMap.Keys);
            Assert.Contains("procedure_poison_biochemical_assay", procMap.Keys);

            // Verify research unlocks
            Assert.Contains("knowledge_radiation_basics", procMap["procedure_rad_pathology"].researchUnlocks);
            Assert.Contains("knowledge_field_trauma_surgery", procMap["procedure_blunt_trauma"].researchUnlocks);
            Assert.Contains("knowledge_pharmacology_synthesis", procMap["procedure_poison_biochemical_assay"].researchUnlocks);
        }

        [Fact]
        public void PsychologicalContamination_StagesAndGroundingWorkDeterministically()
        {
            var psych = new PsychologicalContaminationSystem();
            string survivorId = "sv_scout";

            // Initially Baseline (Stage 0)
            Assert.Equal(0, psych.GetStage(survivorId));
            Assert.Equal("Normal", psych.GetUIStatusTag(survivorId));

            // Expose to Stadium (ThousandYardStare: 1 contamination with blocked action)
            psych.ApplyContamination(survivorId, "location_stadium_evacuation_center", 80f);
            Assert.True(psych.HasContamination(survivorId, PsychologicalContaminationSystem.Contam_ThousandYardStare));
            Assert.True(psych.IsActionBlocked(survivorId, "action_teach_child"));
            Assert.True(psych.IsActionBlocked(survivorId, "action_tell_stories"));
            Assert.False(psych.IsActionBlocked(survivorId, "action_cook"));

            // Stage 2 (Strain with blocked action)
            Assert.Equal(2, psych.GetStage(survivorId));
            Assert.Equal("Task Avoidance", psych.GetUIStatusTag(survivorId));

            // Expose to Abattoir (DisgustCascade + PhantomSmell) -> Now 3 contaminations (Stage 3: Intrusion)
            psych.ApplyContamination(survivorId, "location_automated_abattoir", 70f);
            Assert.Equal(3, psych.GetStage(survivorId));
            Assert.Equal("Severe Strain", psych.GetUIStatusTag(survivorId));

            // If assigned to autopsy while having ThousandYardStare -> Stage 4: Acute Limit
            Assert.Equal(4, psych.GetStage(survivorId, "shelter_module_autopsy"));
            Assert.Equal("Mental Break Risk", psych.GetUIStatusTag(survivorId, "shelter_module_autopsy"));

            // Companion Grounding reduces duration
            var listBefore = psych.GetContaminations(survivorId);
            Assert.NotNull(listBefore);
            float cotDaysBefore = listBefore[0].DaysRemaining;

            bool grounded = psych.GroundSurvivor(survivorId, "sv_companion", 75f);
            Assert.True(grounded);
            Assert.True(listBefore[0].DaysRemaining < cotDaysBefore);

            // Shelter Rest accelerates decay
            psych.ApplyShelterRest(survivorId, 3.0f);
            Assert.False(psych.HasContamination(survivorId, PsychologicalContaminationSystem.Contam_ChildCotTrauma));
        }

        [Fact]
        public void PsychologicalContamination_SaveRoundTrip_PreservesEntries()
        {
            var psychA = new PsychologicalContaminationSystem();
            psychA.ApplyContamination("sv_a", "location_automated_abattoir", 50f);

            var save = psychA.CaptureState();
            Assert.NotNull(save);
            Assert.NotEmpty(save.Survivors);

            var psychB = new PsychologicalContaminationSystem();
            psychB.RestoreState(save);

            Assert.True(psychB.HasContamination("sv_a", PsychologicalContaminationSystem.Contam_DisgustCascade));
            Assert.True(psychB.HasContamination("sv_a", PsychologicalContaminationSystem.Contam_PhantomSmell));
            Assert.True(psychB.IsActionBlocked("sv_a", "action_cook"));
        }
    }
}
