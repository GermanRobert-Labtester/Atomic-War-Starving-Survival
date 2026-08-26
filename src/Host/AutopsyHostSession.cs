using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for AutopsySystem.
    /// Manages clinical autopsy queue, tool sterilization, pathogen containment, and research discoveries.
    /// </summary>
    public sealed class AutopsyHostSession
    : HostSessionBase{
        public AutopsySystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public AutopsyHostSession(AutopsySystem system)
        {
            if (system == null)
            {
                var inv = new Ashfall.Core.Inventory.Inventory();
                var rad = new RadiationSystem(seed: 1986);
                var starting = new StartingLevelSystem();
                var vent = new VentilationSystem(starting);
                var res = new ResearchSystem();
                var wardState = new MedicalWardState();
                var bed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.General);
                var proc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
                var medical = new MedicalWardSystem(wardState, new[] { bed }, new[] { proc });
                system = new AutopsySystem(new SeededRng(1986), inv, rad, vent, res, medical, new GodotLog());
            }
            System = system;

            System.OnCaseCompleted += c =>
            {
                LastEvent = $"[Autopsy] Completed examination for specimen {c.specimenId}: {c.finding}";
                RaiseStateChanged();
            };

            System.OnAutopsyChanged += () =>
            {
                RaiseStateChanged();
            };
        }

        public ActionResult QueueCase(string specimenId, string procedureId, string medicId, int currentDay)
        {
            var res = System.QueueAutopsy(specimenId, procedureId, medicId);
            if (res.IsSuccess)
            {
                LastEvent = $"Queued autopsy for {specimenId} with {procedureId}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult BeginAutopsy(string caseId)
        {
            var res = System.BeginAutopsy(caseId);
            if (res.IsSuccess)
            {
                LastEvent = $"Started procedure on case {caseId}";
                RaiseStateChanged();
            }
            return res;
        }

        /// <summary>Load the autopsy_procedures.json catalog into the Core system (the authority).</summary>
        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            int count = AutopsyProcedureCatalogLoader.LoadAndRegister(System, dataDir, fileIO, serializer);
            if (count > 0)
            {
                LastEvent = $"Autopsy procedure catalog loaded: {count} procedures";
                RaiseStateChanged();
            }
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            AutopsySaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
