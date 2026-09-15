// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    public enum RadioProgramJobStatus
    {
        Preparing = 0,
        Ready = 1,
        Delivered = 2,
        Cancelled = 3,
        Failed = 4
    }

    [Serializable]
    public sealed class RadioProgramJob
    {
        public string JobId { get; set; } = string.Empty;
        public string TemplateId { get; set; } = string.Empty;
        public string PresenterId { get; set; } = string.Empty;
        public string StationId { get; set; } = string.Empty;
        public string SlotId { get; set; } = string.Empty;
        public string PsyopsCampaignId { get; set; } = string.Empty;
        public int DayStarted { get; set; }
        public int PrepTicks { get; set; }
        public int PrepTicksRequired { get; set; } = 1;
        public int Status { get; set; } = (int)RadioProgramJobStatus.Preparing;
        public string LastDeliveryBroadcastId { get; set; } = string.Empty;
        public bool PropagandaStarted { get; set; }
        public string FailureCode { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class RadioProgramFollowUpHook
    {
        public string HookId { get; set; } = string.Empty;
        public string SourceJobId { get; set; } = string.Empty;
        public string TemplateId { get; set; } = string.Empty;
        public int CreatedDay { get; set; }
        public bool Resolved { get; set; }
    }

    [Serializable]
    public sealed class RadioProgramProductionState
    {
        public string SystemId { get; set; } = RadioProgramProductionSystem.SystemId;
        public int SchemaVersion { get; set; } = 1;
        public List<RadioProgramJob> Jobs { get; set; } = new List<RadioProgramJob>();
        public List<RadioProgramFollowUpHook> FollowUps { get; set; } = new List<RadioProgramFollowUpHook>();
        public int NextJobSeq { get; set; }
        public int TotalDelivered { get; set; }
        public int TotalCancelled { get; set; }
    }

    /// <summary>
    /// Plan 173 Phase 1 — player program prep/delivery against existing radio adapters.
    /// Owns production jobs and follow-up hooks only. Schedule, reception, and
    /// propaganda pressure remain external authorities.
    /// </summary>
    public sealed class RadioProgramProductionSystem
    {
        public const string SystemId = "radio_program_production";

        private RadioProgramProductionState _state = new RadioProgramProductionState();
        private readonly RadioProgramCatalog _catalog;
        private readonly RadioStationCatalog? _stations;
        private readonly ILog _log;

        /// <summary>
        /// Host wires <see cref="PsyOpsSystem.StartCampaign"/>. Null-safe: delivery
        /// still succeeds when no campaign is configured.
        /// </summary>
        public Func<string, int, bool>? StartPropagandaCampaign { get; set; }

        /// <summary>
        /// Host wires inventory presence for <c>required_equipment_item_ids</c>.
        /// Null-safe when the template lists no equipment (fixtures).
        /// </summary>
        public Func<IReadOnlyList<string>, bool>? HasRequiredEquipment { get; set; }

        /// <summary>
        /// Host wires inventory consume for prep cost. Null-safe when no cost set.
        /// </summary>
        public Func<string, int, bool>? TryConsumePrepCost { get; set; }

        public event Action<RadioProgramJob>? OnJobReady;
        public event Action<RadioProgramJob>? OnJobDelivered;
        public event Action<RadioProgramJob>? OnJobCancelled;

        public RadioProgramProductionState State => _state;
        public RadioProgramCatalog Catalog => _catalog;

        public RadioProgramProductionSystem(
            RadioProgramCatalog catalog,
            RadioStationCatalog? stations = null,
            ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _stations = stations;
            _log = log ?? NullLog.Instance;
            if (_catalog.All.Count == 0)
                _catalog.Index();
        }

        public ActionResult StartPrep(string templateId, string presenterId, int day)
        {
            if (string.IsNullOrEmpty(templateId) || string.IsNullOrEmpty(presenterId))
                return ActionResult.Blocked("invalid_args", "radio_program.invalid_args");

            var template = _catalog.Get(templateId);
            if (template == null)
                return ActionResult.Blocked("unknown_template", "radio_program.unknown_template");

            if (string.IsNullOrEmpty(template.station_id) || string.IsNullOrEmpty(template.slot_id))
                return ActionResult.Blocked("template_missing_slot", "radio_program.template_missing_slot");

            if (!SlotExists(template.station_id, template.slot_id))
                return ActionResult.Blocked("unknown_slot", "radio_program.unknown_slot");

            if (_state.Jobs.Exists(j =>
                    string.Equals(j.TemplateId, templateId, StringComparison.Ordinal)
                    && (j.Status == (int)RadioProgramJobStatus.Preparing
                        || j.Status == (int)RadioProgramJobStatus.Ready)))
            {
                return ActionResult.Blocked("job_active", "radio_program.job_active");
            }

            var equipment = template.required_equipment_item_ids;
            if (equipment != null && equipment.Count > 0)
            {
                if (HasRequiredEquipment == null || !HasRequiredEquipment(equipment))
                    return ActionResult.Blocked("missing_equipment", "radio_program.missing_equipment");
            }

            string costItem = template.prep_cost_item_id ?? string.Empty;
            int costCount = template.prep_cost_count;
            if (!string.IsNullOrEmpty(costItem) && costCount > 0)
            {
                if (TryConsumePrepCost == null || !TryConsumePrepCost(costItem, costCount))
                    return ActionResult.Blocked("missing_prep_cost", "radio_program.missing_prep_cost");
            }

            _state.NextJobSeq++;
            var job = new RadioProgramJob
            {
                JobId = $"radio_prog_{day}_{templateId}_{_state.NextJobSeq}",
                TemplateId = templateId,
                PresenterId = presenterId,
                StationId = template.station_id,
                SlotId = template.slot_id,
                PsyopsCampaignId = template.psyops_campaign_id ?? string.Empty,
                DayStarted = day,
                PrepTicksRequired = Math.Max(1, template.prep_ticks_required),
                Status = (int)RadioProgramJobStatus.Preparing
            };
            _state.Jobs.Add(job);
            _log.Info($"[RadioProgram] Started prep {job.JobId} for slot {job.StationId}/{job.SlotId}");
            return ActionResult.Success("radio_program.prep_started");
        }

        public ActionResult CancelJob(string jobId)
        {
            var job = FindJob(jobId);
            if (job == null)
                return ActionResult.Blocked("no_job", "radio_program.no_job");
            if (job.Status == (int)RadioProgramJobStatus.Delivered
                || job.Status == (int)RadioProgramJobStatus.Cancelled)
            {
                return ActionResult.Blocked("job_terminal", "radio_program.job_terminal");
            }

            job.Status = (int)RadioProgramJobStatus.Cancelled;
            _state.TotalCancelled++;
            OnJobCancelled?.Invoke(job);
            return ActionResult.Success("radio_program.cancelled");
        }

        public void TickDay(int day)
        {
            foreach (var job in _state.Jobs)
            {
                if (job.Status != (int)RadioProgramJobStatus.Preparing) continue;
                job.PrepTicks++;
                if (job.PrepTicks >= job.PrepTicksRequired)
                {
                    job.Status = (int)RadioProgramJobStatus.Ready;
                    OnJobReady?.Invoke(job);
                    _log.Info($"[RadioProgram] Job {job.JobId} ready on day {day}");
                }
            }
        }

        /// <summary>
        /// Delivers a ready job when the schedule adapter reports successful airtime
        /// for the bound station. Does not own Resolve — host supplies the fact.
        /// </summary>
        public ActionResult TryDeliver(string jobId, ScheduledBroadcastResult delivery, int day)
        {
            var job = FindJob(jobId);
            if (job == null)
                return ActionResult.Blocked("no_job", "radio_program.no_job");
            if (job.Status != (int)RadioProgramJobStatus.Ready)
                return ActionResult.Blocked("not_ready", "radio_program.not_ready");
            if (delivery == null)
                return ActionResult.Blocked("missing_delivery", "radio_program.missing_delivery");

            if (!delivery.HasTransmission || delivery.IsJammed || delivery.IsSilence)
            {
                job.FailureCode = delivery.IsJammed ? "delivery_jammed" : "delivery_no_transmission";
                return ActionResult.Blocked(job.FailureCode, "radio_program.delivery_failed");
            }

            if (!string.Equals(delivery.StationId, job.StationId, StringComparison.Ordinal))
            {
                job.FailureCode = "station_mismatch";
                return ActionResult.Blocked("station_mismatch", "radio_program.station_mismatch");
            }

            job.Status = (int)RadioProgramJobStatus.Delivered;
            job.LastDeliveryBroadcastId = delivery.BroadcastId ?? string.Empty;
            _state.TotalDelivered++;

            if (!string.IsNullOrEmpty(job.PsyopsCampaignId) && StartPropagandaCampaign != null)
            {
                job.PropagandaStarted = StartPropagandaCampaign(job.PsyopsCampaignId, day);
                if (!job.PropagandaStarted)
                    _log.Warn($"[RadioProgram] Delivery ok but StartCampaign failed for {job.PsyopsCampaignId}");
            }

            _state.FollowUps.Add(new RadioProgramFollowUpHook
            {
                HookId = $"followup_{job.JobId}",
                SourceJobId = job.JobId,
                TemplateId = job.TemplateId,
                CreatedDay = day,
                Resolved = false
            });

            OnJobDelivered?.Invoke(job);
            _log.Info($"[RadioProgram] Delivered {job.JobId} broadcast={job.LastDeliveryBroadcastId}");
            return ActionResult.Success("radio_program.delivered");
        }

        public bool SlotExists(string stationId, string slotId)
        {
            if (_stations == null)
                return true; // catalog-less unit fixtures may skip station lookup
            var station = _stations.GetStation(stationId);
            if (station?.Schedule == null) return false;
            for (int i = 0; i < station.Schedule.Count; i++)
            {
                if (string.Equals(station.Schedule[i].SlotId, slotId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        public RadioProgramJob? FindJob(string jobId)
        {
            if (string.IsNullOrEmpty(jobId)) return null;
            return _state.Jobs.Find(j => string.Equals(j.JobId, jobId, StringComparison.Ordinal));
        }

        public List<RadioProgramJob> GetActiveJobs() =>
            _state.Jobs.FindAll(j =>
                j.Status == (int)RadioProgramJobStatus.Preparing
                || j.Status == (int)RadioProgramJobStatus.Ready);

        public RadioProgramProductionState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<RadioProgramProductionState>(json) ?? new RadioProgramProductionState();
        }

        public void RestoreState(RadioProgramProductionState? saved)
        {
            if (saved == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<RadioProgramProductionState>(json) ?? new RadioProgramProductionState();
            if (string.IsNullOrEmpty(_state.SystemId))
                _state.SystemId = SystemId;
        }
    }
}
