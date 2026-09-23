// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Endgame
{
    public enum ProfileRecordResult
    {
        Success = 0,
        NoNewRecords = 1,
        InvalidHistory = 2
    }

    [Serializable]
    public sealed class ProfileRunRow
    {
        public int RunOrdinal { get; set; }
        public string RunIdentity { get; set; } = string.Empty;
        public string CompletionId { get; set; } = string.Empty;
        public string EndingId { get; set; } = string.Empty;
        public string DifficultyPresetId { get; set; } = string.Empty;
        public int DaysSurvived { get; set; }
        public int LivingDwellers { get; set; }
        public int DeathsRecorded { get; set; }
        public bool GrandTreatySigned { get; set; }
        public bool TempestDecommissioned { get; set; }
        public bool DebtLedgersBurned { get; set; }
        public bool ChildrenSurvived { get; set; }
        public bool VelSecretExposed { get; set; }
    }

    [Serializable]
    public sealed class CrossRunProfileSaveState
    {
        public int schemaVersion { get; set; } = 1;
        public List<ProfileRunRow> Runs { get; set; } = new();
        public string Checksum { get; set; } = string.Empty;
    }

    /// <summary>
    /// C3-175 / Plan 175: User-level cross-run profile store.
    /// Resides outside campaign slots (user://profile.json).
    /// Derives append-only run rows from CampaignCompletionHistory facts.
    /// Adheres to DEC-20: strictly no rewards, unlocks, prestige, or NG+ gameplay effects.
    /// </summary>
    public sealed class CrossRunProfileStore
    {
        public const int CurrentSchemaVersion = 1;

        private readonly List<ProfileRunRow> _runs = new();
        private readonly HashSet<string> _completionIds = new(StringComparer.Ordinal);

        public int SchemaVersion => CurrentSchemaVersion;
        public IReadOnlyList<ProfileRunRow> Runs => _runs;
        public int TotalRunsCompleted => _runs.Count;

        public CrossRunProfileStore()
        {
        }

        public ProfileRecordResult Record(CampaignCompletionHistory? history)
        {
            if (history == null || history.records == null)
                return ProfileRecordResult.InvalidHistory;

            if (!CampaignCompletionHistoryService.TryValidate(history, out _))
                return ProfileRecordResult.InvalidHistory;

            int addedCount = 0;
            for (int i = 0; i < history.records.Count; i++)
            {
                var record = history.records[i];
                if (record == null || string.IsNullOrWhiteSpace(record.completionId))
                    continue;

                if (_completionIds.Contains(record.completionId))
                    continue;

                var row = new ProfileRunRow
                {
                    RunOrdinal = _runs.Count + 1,
                    RunIdentity = record.runIdentity,
                    CompletionId = record.completionId,
                    EndingId = record.endingId,
                    DifficultyPresetId = record.difficultyPresetId ?? string.Empty,
                    DaysSurvived = record.daysSurvived,
                    LivingDwellers = record.livingDwellers,
                    DeathsRecorded = record.deathsRecorded,
                    GrandTreatySigned = record.grandTreatySigned,
                    TempestDecommissioned = record.tempestDecommissioned,
                    DebtLedgersBurned = record.debtLedgersBurned,
                    ChildrenSurvived = record.childrenSurvived,
                    VelSecretExposed = record.velSecretExposed
                };

                _runs.Add(row);
                _completionIds.Add(record.completionId);
                addedCount++;
            }

            return addedCount > 0 ? ProfileRecordResult.Success : ProfileRecordResult.NoNewRecords;
        }

        public CrossRunProfileSaveState CaptureState()
        {
            var state = new CrossRunProfileSaveState
            {
                schemaVersion = CurrentSchemaVersion,
                Runs = new List<ProfileRunRow>(_runs)
            };
            state.Checksum = SaveChecksum.Compute(state);
            return state;
        }

        public static CrossRunProfileStore RestoreState(CrossRunProfileSaveState? state)
        {
            var store = new CrossRunProfileStore();
            if (state == null || state.Runs == null)
                return store;

            for (int i = 0; i < state.Runs.Count; i++)
            {
                var r = state.Runs[i];
                if (r != null && !string.IsNullOrWhiteSpace(r.CompletionId) && store._completionIds.Add(r.CompletionId))
                {
                    store._runs.Add(r);
                }
            }

            return store;
        }
    }
}
