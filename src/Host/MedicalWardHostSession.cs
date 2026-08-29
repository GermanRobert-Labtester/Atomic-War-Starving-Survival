using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public class MedicalWardHostSession : HostSessionBase
    {
        public MedicalWardSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public int SimDay { get; set; }
        public List<MedicalProcedureDef> Procedures { get; set; } = new List<MedicalProcedureDef>();

        private Action<MedicalWardEvent>? _onwardchanged_handler;

        public MedicalWardHostSession(MedicalWardSystem? system = null)
        {
            if (system != null)
            {
                System = system;
            }
            else
            {
                var defaultState = new MedicalWardState();
                var defaultBed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.General);
                var defaultProc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
                System = new MedicalWardSystem(defaultState, new[] { defaultBed }, new[] { defaultProc });
            }
            _onwardchanged_handler = _ => RaiseChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            try
            {
                var save = new MedicalWardSave
                {
                    simDay = SimDay,
                    Beds = new List<MedicalBedSave>(),
                    Procedures = new List<MedicalProcedureDef>(Procedures),
                    State = System.CaptureState()
                };
                if (MedicalWardSaveStore.TrySave(save))
                    base.Save();
            }
            catch (Exception e)
            {
                GD.PrintErr("[MedicalWard] save failed: " + e.Message);
            }
        }

        public void RestoreSave(MedicalWardSave? state)
        {
            if (state == null) return;
            try
            {
                System.RestoreState(state.State);
                SimDay = state.simDay;
                Procedures = new List<MedicalProcedureDef>(state.Procedures);
                IsDirty = false;
            }
            catch (Exception e)
            {
                GD.PrintErr("[MedicalWard] restore failed: " + e.Message);
            }
        }

        private void RaiseChanged()
        {
            MarkDirty();
        }
    }
}
