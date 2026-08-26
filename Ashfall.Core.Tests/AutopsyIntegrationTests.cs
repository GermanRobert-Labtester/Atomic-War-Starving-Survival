using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class AutopsyIntegrationTests
    {
        private static AutopsySystem Create(out Inventory.Inventory inv, out ResearchSystem res)
        {
            inv = new Inventory.Inventory();
            var rad = new RadiationSystem(seed: 42);
            var starting = new StartingLevelSystem();
            var vent = new VentilationSystem(starting);
            res = new ResearchSystem();
            var wardState = new MedicalWardState();
            var bed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.General);
            var proc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
            var medical = new MedicalWardSystem(wardState, new[] { bed }, new[] { proc });
            var sys = new AutopsySystem(new SeededRng(42), inv, rad, vent, res, medical);
            sys.LoadCatalog(new List<AutopsyProcedure>
            {
                new AutopsyProcedure
                {
                    procedure_id = "proc_standard",
                    display_name = "Standard Post-Mortem",
                    procedureHours = 20, // 20 hours takes multiple days
                    requiredTools = new List<string> { "scalpel" }
                }
            });
            return sys;
        }

        [Fact]
        public void QueueAutopsy_AndTick_AdvancesAutopsy()
        {
            var sys = Create(out var inv, out _);
            inv.AddById("scalpel", 1);
            var q = sys.QueueAutopsy("dweller_deceased", "proc_standard", "dweller_medic");
            Assert.True(q.IsSuccess);
            Assert.Single(sys.State.cases);

            sys.BeginAutopsy(sys.State.cases[0].caseId);
            sys.TickDay(1);
            Assert.Single(sys.State.cases);
            Assert.Equal(8f, sys.State.cases[0].progressHours);
        }

        [Fact]
        public void SaveAndRestore_PreservesAutopsyCases()
        {
            var sys1 = Create(out var inv, out _);
            inv.AddById("scalpel", 1);
            sys1.QueueAutopsy("specimen_1", "proc_standard", "medic_1");
            sys1.BeginAutopsy(sys1.State.cases[0].caseId);
            sys1.TickDay(1);

            var state = sys1.CaptureState();
            var sys2 = Create(out _, out _);
            sys2.RestoreState(state);

            Assert.Single(sys2.State.cases);
            Assert.Equal("specimen_1", sys2.State.cases[0].specimenId);
            Assert.Equal(sys1.State.cases[0].progressHours, sys2.State.cases[0].progressHours);
        }
    }
}
