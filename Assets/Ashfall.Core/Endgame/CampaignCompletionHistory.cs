// SPDX-License-Identifier: MIT
// Wave 11 B3 — user-level, observation-only campaign-completion history.

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Endgame
{
    /// <summary>
    /// Immutable semantic facts captured when an already-selected campaign ending is sealed.
    /// This DTO deliberately contains stable identifiers and canonical epilogue inputs only;
    /// presentation text, reward state, and ending-selection logic remain outside this owner.
    /// </summary>
    [Serializable]
    public sealed class CampaignCompletionRecord
    {
        public int schemaVersion = 1;
        public string completionId = string.Empty;
        public int completionOrdinal;
        public string runIdentity = string.Empty;
        public string endingId = string.Empty;
        public int daysSurvived;
        public int livingDwellers;
        public int deathsRecorded;
        public bool grandTreatySigned;
        public bool tempestDecommissioned;
        public bool debtLedgersBurned;
        public bool childrenSurvived;
        public bool velSecretExposed;
        public string previousChecksum = string.Empty;

        // SaveChecksum deliberately excludes a root field with this exact name,
        // allowing the checksum to cover every immutable semantic field above.
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// User-level persistence DTO. Its list is ordered by application append ordinal,
    /// never by localized title or wall-clock metadata.
    /// </summary>
    [Serializable]
    public sealed class CampaignCompletionHistory
    {
        public int schemaVersion = 1;
        public List<CampaignCompletionRecord> records = new();
    }

    /// <summary>
    /// Read-only input supplied by the existing host composition when an ending is sealed.
    /// The caller owns run identity and ending selection; this type cannot select an ending.
    /// </summary>
    public sealed class CampaignCompletionObservation
    {
        public CampaignCompletionObservation(string runIdentity, string endingId, EpilogueContextInputs context)
        {
            RunIdentity = runIdentity ?? string.Empty;
            EndingId = endingId ?? string.Empty;
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string RunIdentity { get; }
        public string EndingId { get; }
        public EpilogueContextInputs Context { get; }
    }

    public enum CompletionHistoryAppendResult
    {
        Appended = 0,
        AlreadyRecorded = 1,
        InvalidObservation = 2,
        InvalidHistory = 3
    }

    /// <summary>
    /// Pure append-only record builder and integrity validator. It does not perform IO,
    /// mutate campaign state, evaluate endings, or grant cross-campaign rewards.
    /// </summary>
    public static class CampaignCompletionHistoryService
    {
        private const int CurrentSchemaVersion = 1;

        public static CompletionHistoryAppendResult Append(
            CampaignCompletionHistory history,
            CampaignCompletionObservation observation,
            out CampaignCompletionRecord? appendedRecord)
        {
            appendedRecord = null;
            if (history == null || observation == null || !IsValid(observation))
                return CompletionHistoryAppendResult.InvalidObservation;

            if (!TryValidate(history, out _))
                return CompletionHistoryAppendResult.InvalidHistory;

            string completionId = ComputeCompletionId(observation);
            for (int i = 0; i < history.records.Count; i++)
            {
                if (string.Equals(history.records[i].completionId, completionId, StringComparison.Ordinal))
                    return CompletionHistoryAppendResult.AlreadyRecorded;
            }

            var context = observation.Context;
            var record = new CampaignCompletionRecord
            {
                schemaVersion = CurrentSchemaVersion,
                completionId = completionId,
                completionOrdinal = history.records.Count + 1,
                runIdentity = observation.RunIdentity,
                endingId = observation.EndingId,
                daysSurvived = context.Days,
                livingDwellers = context.LivingDwellers,
                deathsRecorded = context.DeathsRecorded,
                grandTreatySigned = context.GrandTreatySigned,
                tempestDecommissioned = context.TempestDecommissioned,
                debtLedgersBurned = context.DebtLedgersBurned,
                childrenSurvived = context.ChildrenSurvived,
                velSecretExposed = context.VelSecretExposed,
                previousChecksum = history.records.Count == 0
                    ? string.Empty
                    : history.records[history.records.Count - 1].Checksum
            };
            record.Checksum = SaveChecksum.Compute(record);
            history.records.Add(record);
            appendedRecord = CloneRecord(record);
            return CompletionHistoryAppendResult.Appended;
        }

        /// <summary>
        /// Verifies ordinal ordering, semantic identity, per-record checksums, and the append
        /// chain. A valid prefix remains valid after a tail is lost; no claim of anti-tamper
        /// protection beyond these checksum checks is made.
        /// </summary>
        public static bool TryValidate(CampaignCompletionHistory? history, out string error)
        {
            if (history == null)
            {
                error = "Completion history is missing.";
                return false;
            }

            if (history.schemaVersion != CurrentSchemaVersion)
            {
                error = $"Unsupported completion-history schema {history.schemaVersion}.";
                return false;
            }

            if (history.records == null)
            {
                error = "Completion history records are missing.";
                return false;
            }

            string previousChecksum = string.Empty;
            var ids = new HashSet<string>(StringComparer.Ordinal);
            for (int index = 0; index < history.records.Count; index++)
            {
                CampaignCompletionRecord? record = history.records[index];
                if (record == null)
                {
                    error = $"Completion record {index + 1} is missing.";
                    return false;
                }

                if (record.schemaVersion != CurrentSchemaVersion || record.completionOrdinal != index + 1)
                {
                    error = $"Completion record {index + 1} has an invalid schema or append ordinal.";
                    return false;
                }

                if (!IsValidRecord(record) || !ids.Add(record.completionId))
                {
                    error = $"Completion record {index + 1} has invalid semantic identity.";
                    return false;
                }

                if (!string.Equals(record.previousChecksum, previousChecksum, StringComparison.Ordinal))
                {
                    error = $"Completion record {index + 1} does not link to the previous record.";
                    return false;
                }

                if (!string.Equals(record.Checksum, SaveChecksum.Compute(record), StringComparison.Ordinal))
                {
                    error = $"Completion record {index + 1} failed checksum validation.";
                    return false;
                }

                if (!string.Equals(record.completionId, ComputeCompletionId(record), StringComparison.Ordinal))
                {
                    error = $"Completion record {index + 1} has a mismatched completion identity.";
                    return false;
                }

                previousChecksum = record.Checksum;
            }

            error = string.Empty;
            return true;
        }

        public static CampaignCompletionHistory Clone(CampaignCompletionHistory history)
        {
            if (history == null) throw new ArgumentNullException(nameof(history));
            var clone = new CampaignCompletionHistory { schemaVersion = history.schemaVersion };
            if (history.records != null)
            {
                for (int i = 0; i < history.records.Count; i++)
                {
                    if (history.records[i] != null)
                        clone.records.Add(CloneRecord(history.records[i]));
                }
            }
            return clone;
        }

        public static string Serialize(CampaignCompletionHistory history)
        {
            if (!TryValidate(history, out string error))
                throw new InvalidOperationException("Cannot serialize invalid completion history: " + error);
            return JsonSerializer.Serialize(history, JsonOptions);
        }

        public static bool TryDeserialize(string? json, out CampaignCompletionHistory history, out string error)
        {
            history = new CampaignCompletionHistory();
            if (string.IsNullOrWhiteSpace(json))
            {
                error = string.Empty;
                return true;
            }

            try
            {
                CampaignCompletionHistory? parsed = JsonSerializer.Deserialize<CampaignCompletionHistory>(json, JsonOptions);
                if (parsed == null)
                {
                    error = "Completion history deserialized to null.";
                    return false;
                }

                if (!TryValidate(parsed, out error))
                    return false;

                history = Clone(parsed);
                return true;
            }
            catch (Exception ex)
            {
                error = "Invalid completion-history JSON (" + ex.Message + ").";
                return false;
            }
        }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        private static bool IsValid(CampaignCompletionObservation observation)
        {
            if (string.IsNullOrWhiteSpace(observation.RunIdentity) || string.IsNullOrWhiteSpace(observation.EndingId))
                return false;

            EpilogueContextInputs context = observation.Context;
            return context.Days >= 0 && context.LivingDwellers >= 0 && context.DeathsRecorded >= 0;
        }

        private static bool IsValidRecord(CampaignCompletionRecord record)
        {
            return !string.IsNullOrWhiteSpace(record.completionId) &&
                   !string.IsNullOrWhiteSpace(record.runIdentity) &&
                   !string.IsNullOrWhiteSpace(record.endingId) &&
                   record.daysSurvived >= 0 &&
                   record.livingDwellers >= 0 &&
                   record.deathsRecorded >= 0 &&
                   !string.IsNullOrWhiteSpace(record.Checksum);
        }

        private static string ComputeCompletionId(CampaignCompletionObservation observation)
        {
            var context = observation.Context;
            return SaveChecksum.Compute(new CompletionIdentity
            {
                runIdentity = observation.RunIdentity,
                endingId = observation.EndingId,
                daysSurvived = context.Days,
                livingDwellers = context.LivingDwellers,
                deathsRecorded = context.DeathsRecorded,
                grandTreatySigned = context.GrandTreatySigned,
                tempestDecommissioned = context.TempestDecommissioned,
                debtLedgersBurned = context.DebtLedgersBurned,
                childrenSurvived = context.ChildrenSurvived,
                velSecretExposed = context.VelSecretExposed
            });
        }

        private static string ComputeCompletionId(CampaignCompletionRecord record)
        {
            return SaveChecksum.Compute(new CompletionIdentity
            {
                runIdentity = record.runIdentity,
                endingId = record.endingId,
                daysSurvived = record.daysSurvived,
                livingDwellers = record.livingDwellers,
                deathsRecorded = record.deathsRecorded,
                grandTreatySigned = record.grandTreatySigned,
                tempestDecommissioned = record.tempestDecommissioned,
                debtLedgersBurned = record.debtLedgersBurned,
                childrenSurvived = record.childrenSurvived,
                velSecretExposed = record.velSecretExposed
            });
        }

        private static CampaignCompletionRecord CloneRecord(CampaignCompletionRecord record)
        {
            return new CampaignCompletionRecord
            {
                schemaVersion = record.schemaVersion,
                completionId = record.completionId,
                completionOrdinal = record.completionOrdinal,
                runIdentity = record.runIdentity,
                endingId = record.endingId,
                daysSurvived = record.daysSurvived,
                livingDwellers = record.livingDwellers,
                deathsRecorded = record.deathsRecorded,
                grandTreatySigned = record.grandTreatySigned,
                tempestDecommissioned = record.tempestDecommissioned,
                debtLedgersBurned = record.debtLedgersBurned,
                childrenSurvived = record.childrenSurvived,
                velSecretExposed = record.velSecretExposed,
                previousChecksum = record.previousChecksum,
                Checksum = record.Checksum
            };
        }

        [Serializable]
        private sealed class CompletionIdentity
        {
            public string runIdentity = string.Empty;
            public string endingId = string.Empty;
            public int daysSurvived;
            public int livingDwellers;
            public int deathsRecorded;
            public bool grandTreatySigned;
            public bool tempestDecommissioned;
            public bool debtLedgersBurned;
            public bool childrenSurvived;
            public bool velSecretExposed;
        }
    }
}
