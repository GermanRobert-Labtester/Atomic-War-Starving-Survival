// SPDX-License-Identifier: MIT
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
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<SilentFoundryState>(json) ?? new SilentFoundryState();
        }

        /// <summary>Capture the durable consequence ledger (rides the hub save envelope).</summary>
        public SilentFoundryConsequenceState CaptureConsequenceState()
        {
            if (_consequenceState.applied == null) _consequenceState.applied = new List<FoundryConsequenceRecord>();
            _consequenceState.stateVersion = SilentFoundryConsequenceState.CurrentVersion;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_consequenceState);
            return s.Deserialize<SilentFoundryConsequenceState>(json) ?? new SilentFoundryConsequenceState();
        }

        /// <summary>
        /// Restore the consequence ledger. Missing state (older saves) defaults to
        /// an empty ledger and neutral standing — nothing is re-applied because
        /// the ledger is the idempotency authority.
        /// </summary>
        public void RestoreConsequenceState(SilentFoundryConsequenceState save)
        {
            if (save == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(save);
            var cloned = s.Deserialize<SilentFoundryConsequenceState>(json)
                ?? new SilentFoundryConsequenceState();
            _consequenceState.stateVersion = Math.Max(1, cloned.stateVersion);
            _consequenceState.applied = cloned.applied ?? new List<FoundryConsequenceRecord>();
            _consequenceState.guildStanding = MathfCompat.Clamp(
                cloned.guildStanding, StandingMin, StandingMax);
        }

        public void RestoreState(SilentFoundryState save)
        {
            if (save == null) return;
            var serializer = new SystemTextJsonSerializer();
            var json = serializer.Serialize(save);
            var cloned = serializer.Deserialize<SilentFoundryState>(json) ?? new SilentFoundryState();
            _state.stateVersion = cloned.stateVersion;
            _state.unlocked = cloned.unlocked;
            _state.unlockDay = cloned.unlockDay;
            _state.refractoryLining = cloned.refractoryLining;
            _state.hearthTuyeres = cloned.hearthTuyeres;
            _state.sandBeds = cloned.sandBeds;
            _state.structuralSupports = cloned.structuralSupports;
            _state.safetyExhaust = cloned.safetyExhaust;
            _state.maintenanceCycleDays = cloned.maintenanceCycleDays > 0 ? cloned.maintenanceCycleDays : 4;
            _state.maintenanceDueDay = cloned.maintenanceDueDay;
            _state.daysSinceMaintenance = cloned.daysSinceMaintenance;
            _state.maintenancePerformed = cloned.maintenancePerformed;
            _state.sandQuality = cloned.sandQuality;
            _state.sandMoisture = cloned.sandMoisture;
            _state.binderQuality = cloned.binderQuality;
            _state.patternQuality = cloned.patternQuality;
            _state.contamination = cloned.contamination;
            _state.moldReuseCount = cloned.moldReuseCount;
            _state.compaction = cloned.compaction;
            _state.heatStage = cloned.heatStage;
            _state.heatStartedDay = cloned.heatStartedDay;
            _state.stageElapsedDays = cloned.stageElapsedDays;
            _state.activeProductId = cloned.activeProductId;
            _state.assignedWorkers = cloned.assignedWorkers;
            _state.workerSkill = cloned.workerSkill;
            _state.laborAccumulated = cloned.laborAccumulated;
            _state.workerExposure = cloned.workerExposure;
            _state.materialsConsumed = cloned.materialsConsumed;
            _state.childLaborUsed = cloned.childLaborUsed;
            _state.pendingQuality = cloned.pendingQuality;
            // Cloned lists are independent of the save DTO — no shared element refs.
            _state.completed = cloned.completed ?? new List<FoundryProductionRecord>();
            _state.failed = cloned.failed ?? new List<FoundryFailedCastRecord>();
            _state.incidents = cloned.incidents ?? new List<FoundryIncidentRecord>();
            _state.repairs = cloned.repairs ?? new List<FoundryRepairRecord>();
            _state.laborDispute = cloned.laborDispute;
            _state.laborDisputeStartedDay = cloned.laborDisputeStartedDay;
            _state.strikeStartedDay = cloned.strikeStartedDay;
            _state.overtimeFlag = cloned.overtimeFlag;
            _state.educationConflictFlag = cloned.educationConflictFlag;
            _state.treatyCompliance = cloned.treatyCompliance ?? new List<FoundryTreatyCompliance>();
            _state.triggeredJournals = cloned.triggeredJournals ?? new List<string>();
            _state.cumulativeStress = cloned.cumulativeStress;
            _state.cumulativeHope = cloned.cumulativeHope;
            _state.firstHeatDay = cloned.firstHeatDay;
            _state.strikeDay = cloned.strikeDay;
            // Plan B66 — heavy metallurgy fields (were silently dropped on
            // restore; additive restore keeps old saves' neutral defaults —
            // no active heavy batch, no slag — per the B66 legacy contract).
            _state.activeMetallurgyRecipeId = cloned.activeMetallurgyRecipeId ?? string.Empty;
            _state.metallurgySlag = MathfCompat.Clamp(cloned.metallurgySlag, 0f, 100f);
            _state.metallurgyBatchesCompleted = Math.Max(0, cloned.metallurgyBatchesCompleted);
            // Plan 213 — forging session (null = no pass, legacy clean; a
            // mid-batch restore resumes the exact submitted sequence).
            _state.activeForging = cloned.activeForging;
            if (cloned.rngSeed != 0 && cloned.rngSeed != _rng.Seed)
                _rng = _rngFactory(cloned.rngSeed);
            _state.rngSeed = _rng.Seed;
            NormalizeState();
            EnsureTreatyComplianceRows();
        }

        // -----------------------------------------------------------------
    }
}
