// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : RadioProgramProductionHostSession
// Core Source  : Ashfall.Core.Radio.RadioProgramProductionSystem
// Purpose      : Thin Godot adapter — LastEvent + commands; no gameplay math.
// ============================================================================
using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    public sealed class RadioProgramProductionHostSession : HostSessionBase
    {
        public RadioProgramProductionSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public RadioProgramProductionHostSession(RadioProgramProductionSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnJobReady += job =>
            {
                LastEvent = $"Program ready to air: {job.TemplateId}.";
                RaiseStateChanged();
            };
            System.OnJobDelivered += job =>
            {
                LastEvent = job.PropagandaStarted
                    ? $"Program delivered and campaign queued ({job.TemplateId})."
                    : $"Program delivered ({job.TemplateId}).";
                RaiseStateChanged();
            };
            System.OnJobCancelled += job =>
            {
                LastEvent = $"Program prep cancelled ({job.TemplateId}).";
                RaiseStateChanged();
            };
        }

        public string StartPrep(string templateId, string presenterId, int day)
        {
            var result = System.StartPrep(templateId, presenterId, day);
            RaiseStateChanged();
            return result.Status == ActionResult.StatusKind.Success
                ? $"Prep started for {templateId}."
                : $"Cannot start prep ({result.FailureCode}).";
        }

        public string CancelJob(string jobId)
        {
            var result = System.CancelJob(jobId);
            RaiseStateChanged();
            return result.Status == ActionResult.StatusKind.Success
                ? "Program prep cancelled."
                : $"Cannot cancel ({result.FailureCode}).";
        }

        public string TryDeliver(string jobId, ScheduledBroadcastResult delivery, int day)
        {
            var result = System.TryDeliver(jobId, delivery, day);
            RaiseStateChanged();
            return result.Status == ActionResult.StatusKind.Success
                ? "Program delivered."
                : $"Delivery blocked ({result.FailureCode}).";
        }

        public string ResolveFollowUp(string hookId, string resolutionAction, int day)
        {
            var result = System.ResolveFollowUpHook(hookId, resolutionAction, day);
            RaiseStateChanged();
            return result.Status == ActionResult.StatusKind.Success
                ? $"Follow-up resolved ({hookId})."
                : $"Cannot resolve follow-up ({result.FailureCode}).";
        }

        public System.Collections.Generic.IReadOnlyList<RadioProgramFollowUpHook> GetUnresolvedFollowUps() =>
            System.State.FollowUps.Where(f => !f.Resolved).ToList();

        public int ActiveJobsCount => System.State.Jobs.Count(j => j.Status == (int)RadioProgramJobStatus.Preparing || j.Status == (int)RadioProgramJobStatus.Ready);
        public int DeliveredCount => System.State.TotalDelivered;

        public RadioProgramProductionState CaptureSave() => System.CaptureState();

        public void RestoreSave(RadioProgramProductionState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "Radio program production restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            RadioProgramProductionSaveStore.TrySave(CaptureSave());
        }
    }
}
