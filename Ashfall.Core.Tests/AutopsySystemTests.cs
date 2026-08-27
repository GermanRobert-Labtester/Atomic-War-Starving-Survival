using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class AutopsySystemTests
    {
        [Fact] public void QueueAutopsy_UnknownProcedure_Fails()
        {
            var a = Create(out _, out _, out _, out _, out _);
            var r = a.QueueAutopsy("deceased_1", "unknown_proc", "medic_1");
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        [Fact] public void QueueAutopsy_MissingTool_Blocks()
        {
            var a = Create(out var inv, out _, out _, out _, out _);
            a.LoadCatalog(new System.Collections.Generic.List<AutopsyProcedure>
            {
                new AutopsyProcedure { procedure_id = "standard", requiredTools = new System.Collections.Generic.List<string> { "scalpel" } }
            });
            var r = a.QueueAutopsy("deceased_1", "standard", "medic_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void QueueAutopsy_Valid_QueuesCase()
        {
            var a = Create(out var inv, out _, out _, out _, out _);
            a.LoadCatalog(new System.Collections.Generic.List<AutopsyProcedure>
            {
                new AutopsyProcedure { procedure_id = "standard", requiredTools = new System.Collections.Generic.List<string> { "scalpel" } }
            });
            inv.AddById("scalpel", 1);
            var r = a.QueueAutopsy("deceased_1", "standard", "medic_1");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(a.State.cases);
        }

        [Fact] public void TickDay_CompletesAutopsy()
        {
            var a = Create(out var inv, out _, out _, out var research, out _);
            a.LoadCatalog(new System.Collections.Generic.List<AutopsyProcedure>
            {
                new AutopsyProcedure { procedure_id = "standard", requiredTools = new System.Collections.Generic.List<string> { "scalpel" }, procedureHours = 2, researchUnlocks = new System.Collections.Generic.List<string> { "vaccine_rad" } }
            });
            inv.AddById("scalpel", 1);
            a.QueueAutopsy("deceased_1", "standard", "medic_1");
            a.BeginAutopsy(a.State.cases[0].caseId);
            a.TickDay(1);
            Assert.Contains("deceased_1", a.State.completedSpecimenIds);
            Assert.True(research.IsManualUnlocked("vaccine_rad"));
        }

        [Fact]
        public void BeginAutopsy_ConsumableMissing_LeavesToolsInInventory()
        {
            var a = Create(out var inv, out _, out _, out _, out _);
            a.LoadCatalog(new System.Collections.Generic.List<AutopsyProcedure>
            {
                new AutopsyProcedure
                {
                    procedure_id = "standard",
                    requiredTools = new System.Collections.Generic.List<string> { "scalpel" },
                    requiredConsumables = new System.Collections.Generic.List<string> { "formalin" }
                }
            });
            // Provide tool (scalpel) and consumable (formalin) to queue successfully
            inv.AddById("scalpel", 1);
            inv.AddById("formalin", 1);
            a.QueueAutopsy("deceased_1", "standard", "medic_1");
            Assert.Single(a.State.cases);

            // Consume/remove formalin before beginning autopsy
            inv.RemoveById("formalin", 1);
            var r = a.BeginAutopsy(a.State.cases[0].caseId);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("missing_supplies", r.FailureCode);
            // 0 scalpels consumed (atomic rollback)
            Assert.Equal(1, inv.CountById("scalpel"));
            Assert.Equal(AutopsyStatus.Queued, a.State.cases[0].status);
        }

        [Fact] public void CompletedSpecimen_BlocksReuse()
        {
            var a = Create(out var inv, out _, out _, out _, out _);
            a.LoadCatalog(new System.Collections.Generic.List<AutopsyProcedure>
            {
                new AutopsyProcedure { procedure_id = "standard", requiredTools = new System.Collections.Generic.List<string> { "scalpel" } }
            });
            inv.AddById("scalpel", 1);
            a.QueueAutopsy("deceased_1", "standard", "medic_1");
            a.BeginAutopsy(a.State.cases[0].caseId);
            a.TickDay(1);
            var r = a.QueueAutopsy("deceased_1", "standard", "medic_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void CaptureRestoreState_PreservesCases()
        {
            var a = Create(out var inv, out _, out _, out _, out _);
            a.LoadCatalog(new System.Collections.Generic.List<AutopsyProcedure>
            {
                new AutopsyProcedure { procedure_id = "standard", requiredTools = new System.Collections.Generic.List<string> { "scalpel" } }
            });
            inv.AddById("scalpel", 1);
            a.QueueAutopsy("deceased_1", "standard", "medic_1");
            var state = a.CaptureState();
            Assert.Single(state.cases);

            var a2 = Create(out _, out _, out _, out _, out _);
            a2.LoadCatalog(new System.Collections.Generic.List<AutopsyProcedure>
            {
                new AutopsyProcedure { procedure_id = "standard", requiredTools = new System.Collections.Generic.List<string> { "scalpel" } }
            });
            a2.RestoreState(state);
            Assert.Single(a2.State.cases);
        }

        private static AutopsySystem Create(out Inventory.Inventory inv, out RadiationSystem rad, out VentilationSystem vent, out ResearchSystem research, out MedicalWardSystem medical)
        {
            inv = new Inventory.Inventory();
            rad = new RadiationSystem(seed: 42);
            vent = new VentilationSystem(new Ashfall.Core.StartingLevel.StartingLevelSystem());
            research = new ResearchSystem();
            var wardState = new MedicalWardState();
            var bed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.General);
            var proc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
            medical = new MedicalWardSystem(wardState, new[] { bed }, new[] { proc });
            return new AutopsySystem(new SeededRng(42), inv, rad, vent, research, medical);
        }
    }
}
