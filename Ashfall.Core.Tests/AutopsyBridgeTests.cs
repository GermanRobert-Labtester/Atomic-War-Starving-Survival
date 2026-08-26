using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class AutopsyBridgeTests
    {
        private static AutopsySystem CreateAutopsy(out DiseaseSystem disease, out JournalSystem journal, out MemorialSystem memorial)
        {
            var rng = new SeededRng(42);
            var inv = new Ashfall.Core.Inventory.Inventory();
            // Ensure required tools/consumables exist for the test procedure
            inv.AddById("item_scalpel", 5);
            inv.AddById("item_formalin", 5);
            var radiation = new Ashfall.Core.Radiation.RadiationSystem(seed: 42);
            var starting = new Ashfall.Core.StartingLevel.StartingLevelSystem();
            var ventilation = new Ashfall.Core.VentilationSystem(starting);
            var research = new Ashfall.Core.ResearchSystem();
            var medical = new Ashfall.Core.Medical.MedicalWardSystem(
                new Ashfall.Core.Medical.MedicalWardState(),
                new[] { new Ashfall.Core.Medical.MedicalBed("bed_1", "Bed 1", Ashfall.Core.Medical.MedicalBedCategory.General) },
                new[] { new Ashfall.Core.Medical.MedicalProcedureDef("proc_1", "Proc", "Med") });

            var autopsy = new AutopsySystem(rng, inv, radiation, ventilation, research, medical);
            autopsy.LoadCatalog(new System.Collections.Generic.List<AutopsyProcedure>
            {
                new AutopsyProcedure
                {
                    procedure_id = "proc_test_zoonotic",
                    display_name = "Zoonotic Screen",
                    requiredTools = new System.Collections.Generic.List<string> { "item_scalpel" },
                    requiredConsumables = new System.Collections.Generic.List<string> { "item_formalin" },
                    possibleFindings = new System.Collections.Generic.List<string> { "zoonotic_influenza_detected" },
                    researchUnlocks = new System.Collections.Generic.List<string>()
                },
                new AutopsyProcedure
                {
                    procedure_id = "proc_test_clean",
                    display_name = "Clean Screen",
                    requiredTools = new System.Collections.Generic.List<string> { "item_scalpel" },
                    requiredConsumables = new System.Collections.Generic.List<string> { "item_formalin" },
                    possibleFindings = new System.Collections.Generic.List<string> { "no_pathogen" },
                    researchUnlocks = new System.Collections.Generic.List<string>()
                }
            });

            disease = new DiseaseSystem(new DiseaseSystemState(), new SeededRng(99));
            var catalog = new DiseaseCatalog();
            catalog.Diseases.Add(new DiseaseDefinition { id = DiseaseIds.ZoonoticFlu, vector = DiseaseVectorNames.Air });
            disease.BindCatalog(catalog);

            journal = new JournalSystem();
            memorial = new MemorialSystem(new MemorialState());

            return autopsy;
        }

        [Fact]
        public void ZoonoticFinding_InfectsMedic()
        {
            var autopsy = CreateAutopsy(out var disease, out var journal, out var memorial);
            // Wire as host does
            autopsy.OnCaseCompleted += c =>
            {
                string finding = c.finding ?? string.Empty;
                if (finding.IndexOf("zoonotic", System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (!string.IsNullOrEmpty(c.assignedMedicId))
                        disease.Infect(c.assignedMedicId, DiseaseIds.ZoonoticFlu, 10);
                }
                journal.TryAddRawEntry("autopsy_completed", $"Autopsy {c.caseId}: {finding}", null!, 10);
                try
                {
                    memorial.Memorialize(new MemorialInput { SurvivorId = c.specimenId, Cause = finding, Day = 10, BirthDay = 0, Epitaph = finding });
                }
                catch { }
            };

            autopsy.QueueAutopsy("specimen_a", "proc_test_zoonotic", "medic_b");
            var c1 = autopsy.State.cases[0];
            autopsy.BeginAutopsy(c1.caseId);
            autopsy.TickDay(10);

            Assert.True(disease.IsInfected("medic_b", DiseaseIds.ZoonoticFlu), "Zoonotic finding should infect medic");
            Assert.Single(journal.Entries);
            Assert.Single(memorial.Entries);
            Assert.Equal("specimen_a", memorial.Entries[0].SurvivorId);
        }

        [Fact]
        public void CleanFinding_DoesNotInfectMedic()
        {
            var autopsy = CreateAutopsy(out var disease, out _, out _);
            autopsy.OnCaseCompleted += c =>
            {
                string finding = c.finding ?? string.Empty;
                if (finding.IndexOf("zoonotic", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    disease.Infect(c.assignedMedicId, DiseaseIds.ZoonoticFlu, 10);
            };

            autopsy.QueueAutopsy("specimen_b", "proc_test_clean", "medic_c");
            var c1 = autopsy.State.cases[0];
            autopsy.BeginAutopsy(c1.caseId);
            autopsy.TickDay(10);

            Assert.False(disease.IsInfected("medic_c", DiseaseIds.ZoonoticFlu));
            Assert.Equal("no_pathogen", c1.finding);
        }

        [Fact]
        public void AutopsySaveRoundTrip_PreservesCase()
        {
            var autopsy = CreateAutopsy(out _, out _, out _);
            autopsy.QueueAutopsy("specimen_x", "proc_test_clean", "medic_x");
            var state = autopsy.CaptureState();
            var autopsy2 = CreateAutopsy(out _, out _, out _);
            autopsy2.RestoreState(state);
            Assert.Single(autopsy2.State.cases);
            Assert.Equal("specimen_x", autopsy2.State.cases[0].specimenId);
        }
    }
}
