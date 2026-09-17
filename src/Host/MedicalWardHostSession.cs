// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public class MedicalWardHostSession : HostSessionBase
    {
        public MedicalWardSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public int SimDay { get; set; }
        public List<MedicalProcedureDef> Procedures { get; set; } = new List<MedicalProcedureDef>();
        private readonly List<DayStateChangeEvent> _pendingDayEvents = new List<DayStateChangeEvent>();

        /// <summary>
        /// Task #133 P1b: the unified medical pipeline, injected by Main once
        /// bound. When present, procedures with a pipeline treatment
        /// (bandage/chelation) execute through it before the ward log;
        /// when null (headless/CLI) the legacy log-only path is preserved.
        /// </summary>
        public MedicalPipelineCoordinator? Pipeline { get; set; }

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
            _onwardchanged_handler = OnWardChanged;
            System.OnWardChanged += _onwardchanged_handler;
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

        private void OnWardChanged(MedicalWardEvent evt)
        {
            RaiseChanged();
            if (evt == null) return;

            string? kind = evt.Kind switch
            {
                MedicalWardEventKind.Admitted => "medical_admitted",
                MedicalWardEventKind.Discharged => "medical_discharged",
                _ => null
            };
            if (kind == null) return;

            _pendingDayEvents.Add(new DayStateChangeEvent(
                kind, "medical_ward", evt.PatientId, evt.BedId, evt.Day));
        }

        /// <summary>Drains canonical admission/discharge transitions to the daily briefing.</summary>
        public void DrainDayEvents(List<DayStateChangeEvent> target)
        {
            if (target == null) return;
            for (int i = 0; i < _pendingDayEvents.Count; i++)
                target.Add(_pendingDayEvents[i]);
            _pendingDayEvents.Clear();
        }

        /// <summary>
        /// Task #133 P1b: run a ward procedure through the ward-pipeline
        /// bridge — pipeline treatment first for mapped procedures (the ward
        /// log is written only on success), log-only otherwise.
        /// </summary>
        public MedicalWardProcedureResult RunProcedure(string patientId, string procedureId, int day)
        {
            var result = MedicalWardPipelineBridge.RunProcedure(System, Pipeline, patientId, procedureId, day);
            if (result.Succeeded)
                LastEvent = $"Procedure {procedureId} completed for {patientId}.";
            else
                LastEvent = $"Procedure {procedureId} refused for {patientId}: {result.ReasonCode}.";
            return result;
        }
    }
}
