using System;
using System.Collections.Generic;

namespace Ashfall.Core.Foundry
{
    public sealed partial class SilentFoundrySystem
    {
        // Journal triggers (once-only, deterministic)
        // -----------------------------------------------------------------

        /// <summary>
        /// Trigger a journal template once. The narrative text itself stays in
        /// the authored template (NarrativeBatchCatalog); Core only owns the
        /// once-only guard, the typed deltas, and the event the host bridges.
        /// </summary>
        private void MaybeTriggerJournal(string templateId, int day)
        {
            if (IsJournalTriggered(templateId)) return;
            if (!TryGetJournalDeltas(templateId, out float stressDelta, out float hopeEarned)) return;

            _state.triggeredJournals.Add(templateId);
            if (templateId == SilentFoundryIds.JournalFirstHeat && _state.firstHeatDay == 0)
                _state.firstHeatDay = day;
            if (templateId == SilentFoundryIds.JournalStrike && _state.strikeDay == 0)
                _state.strikeDay = day;

            _state.cumulativeStress += stressDelta;
            _state.cumulativeHope += hopeEarned;

            var trigger = new FoundryJournalTrigger
            {
                TemplateId = templateId,
                StressDelta = stressDelta,
                HopeEarned = hopeEarned,
                Day = day
            };
            Raise(EventJournalTriggered, templateId + " triggered day " + day + " (stress " + stressDelta + ", hope " + hopeEarned + ")");
            OnJournalTriggered?.Invoke(trigger);
        }

        /// <summary>
        /// Typed journal moral deltas. These mirror the authored stress_delta /
        /// hope_earned values in jrnl_templates_cycle_d.json; the tests pin them
        /// to the JSON so drift is caught at build time, not in play.
        /// </summary>
        public static bool TryGetJournalDeltas(string templateId, out float stressDelta, out float hopeEarned)
        {
            switch (templateId)
            {
                case SilentFoundryIds.JournalFirstHeat:
                    stressDelta = -5f;
                    hopeEarned = 5f;
                    return true;
                case SilentFoundryIds.JournalStrike:
                    stressDelta = 7f;
                    hopeEarned = 2f;
                    return true;
                default:
                    stressDelta = 0f;
                    hopeEarned = 0f;
                    return false;
            }
        }

        // -----------------------------------------------------------------
        // Save / restore
        // -----------------------------------------------------------------

        public SilentFoundryState CaptureState()
        {
            NormalizeState();
            _state.stateVersion = SilentFoundryState.CurrentVersion;
            _state.rngSeed = _rng.Seed;
            return _state;
        }

        /// <summary>Capture the durable consequence ledger (rides the hub save envelope).</summary>
        public SilentFoundryConsequenceState CaptureConsequenceState()
        {
            if (_consequenceState.applied == null) _consequenceState.applied = new List<FoundryConsequenceRecord>();
            _consequenceState.stateVersion = SilentFoundryConsequenceState.CurrentVersion;
            return _consequenceState;
        }

        /// <summary>
        /// Restore the consequence ledger. Missing state (older saves) defaults to
        /// an empty ledger and neutral standing — nothing is re-applied because
        /// the ledger is the idempotency authority.
        /// </summary>
        public void RestoreConsequenceState(SilentFoundryConsequenceState save)
        {
            if (save == null) return;
            _consequenceState.stateVersion = Math.Max(1, save.stateVersion);
            _consequenceState.applied = save.applied ?? new List<FoundryConsequenceRecord>();
            _consequenceState.guildStanding = MathfCompat.Clamp(save.guildStanding, StandingMin, StandingMax);
        }

        public void RestoreState(SilentFoundryState save)
        {
            if (save == null) return;
            _state.stateVersion = save.stateVersion;
            _state.unlocked = save.unlocked;
            _state.unlockDay = save.unlockDay;
            _state.refractoryLining = save.refractoryLining;
            _state.hearthTuyeres = save.hearthTuyeres;
            _state.sandBeds = save.sandBeds;
            _state.structuralSupports = save.structuralSupports;
            _state.safetyExhaust = save.safetyExhaust;
            _state.maintenanceCycleDays = save.maintenanceCycleDays > 0 ? save.maintenanceCycleDays : 4;
            _state.maintenanceDueDay = save.maintenanceDueDay;
            _state.daysSinceMaintenance = save.daysSinceMaintenance;
            _state.maintenancePerformed = save.maintenancePerformed;
            _state.sandQuality = save.sandQuality;
            _state.sandMoisture = save.sandMoisture;
            _state.binderQuality = save.binderQuality;
            _state.patternQuality = save.patternQuality;
            _state.contamination = save.contamination;
            _state.moldReuseCount = save.moldReuseCount;
            _state.compaction = save.compaction;
            _state.heatStage = save.heatStage;
            _state.heatStartedDay = save.heatStartedDay;
            _state.stageElapsedDays = save.stageElapsedDays;
            _state.activeProductId = save.activeProductId;
            _state.assignedWorkers = save.assignedWorkers;
            _state.workerSkill = save.workerSkill;
            _state.laborAccumulated = save.laborAccumulated;
            _state.workerExposure = save.workerExposure;
            _state.materialsConsumed = save.materialsConsumed;
            _state.childLaborUsed = save.childLaborUsed;
            _state.pendingQuality = save.pendingQuality;
            _state.completed = save.completed ?? new List<FoundryProductionRecord>();
            _state.failed = save.failed ?? new List<FoundryFailedCastRecord>();
            _state.incidents = save.incidents ?? new List<FoundryIncidentRecord>();
            _state.repairs = save.repairs ?? new List<FoundryRepairRecord>();
            _state.laborDispute = save.laborDispute;
            _state.laborDisputeStartedDay = save.laborDisputeStartedDay;
            _state.strikeStartedDay = save.strikeStartedDay;
            _state.overtimeFlag = save.overtimeFlag;
            _state.educationConflictFlag = save.educationConflictFlag;
            _state.treatyCompliance = save.treatyCompliance ?? new List<FoundryTreatyCompliance>();
            _state.triggeredJournals = save.triggeredJournals ?? new List<string>();
            _state.cumulativeStress = save.cumulativeStress;
            _state.cumulativeHope = save.cumulativeHope;
            _state.firstHeatDay = save.firstHeatDay;
            _state.strikeDay = save.strikeDay;
            if (save.rngSeed != 0 && save.rngSeed != _rng.Seed)
                _rng = _rngFactory(save.rngSeed);
            _state.rngSeed = _rng.Seed;
            NormalizeState();
            EnsureTreatyComplianceRows();
        }

        // -----------------------------------------------------------------
    }
}
