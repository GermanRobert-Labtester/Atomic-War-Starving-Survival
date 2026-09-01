// SPDX-License-Identifier: MIT
// Task #132 — Legacy Memorial to typed component parity harness.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Ashfall.Core.Memorial;

namespace Ashfall.Core.Survivors
{
    /// <summary>Stable categories emitted by the Memorial dual-run parity check.</summary>
    public static class MemorialParityCode
    {
        public const string LegacyRowNull = "legacy_row_null";
        public const string LegacyIdInvalid = "legacy_id_invalid";
        public const string LegacyDuplicateId = "legacy_duplicate_id";
        public const string LegacyFieldNull = "legacy_field_null";
        public const string TypedRecordMissing = "typed_record_missing";
        public const string TypedRecordExtra = "typed_record_extra";
        public const string FieldMismatch = "field_mismatch";
    }

    /// <summary>One deterministic Memorial parity finding.</summary>
    public sealed class MemorialParityFinding
    {
        public string Code { get; }
        public SurvivorId SurvivorId { get; }
        public string RawId { get; }
        public string Field { get; }
        public string Expected { get; }
        public string Actual { get; }
        public string Message { get; }

        public MemorialParityFinding(
            string code,
            SurvivorId survivorId,
            string rawId,
            string field,
            string expected,
            string actual,
            string message)
        {
            Code = code ?? string.Empty;
            SurvivorId = survivorId;
            RawId = rawId ?? string.Empty;
            Field = field ?? string.Empty;
            Expected = expected ?? string.Empty;
            Actual = actual ?? string.Empty;
            Message = message ?? string.Empty;
        }

        public override string ToString()
        {
            string id = string.IsNullOrEmpty(RawId) ? SurvivorId.Value : RawId;
            string field = string.IsNullOrEmpty(Field) ? string.Empty : $" [{Field}]";
            return $"Memorial parity [{Code}] {id}{field}: {Message}";
        }
    }

    /// <summary>Outcome of comparing the legacy Memorial list with the typed ledger.</summary>
    public sealed class MemorialParityReport
    {
        public int LegacyRows { get; internal set; }
        public int TypedRows { get; internal set; }
        public List<MemorialParityFinding> Findings { get; } = new List<MemorialParityFinding>();

        public bool IsMatch => Findings.Count == 0;
        public int FindingCount => Findings.Count;

        public string Describe()
        {
            var sb = new StringBuilder();
            sb.Append("MEMORIAL PARITY — legacy=")
              .Append(LegacyRows)
              .Append(" typed=")
              .Append(TypedRows)
              .Append(" finding(s)=")
              .Append(Findings.Count);
            for (int i = 0; i < Findings.Count; i++)
                sb.Append('\n').Append("  ").Append(Findings[i]);
            return sb.ToString();
        }

        public override string ToString()
            => $"[MemorialParity] legacy={LegacyRows} typed={TypedRows} findings={Findings.Count}";
    }

    /// <summary>
    /// Read-only comparison of the legacy Memorial ledger and its typed projection.
    /// The legacy list is grouped by canonical id only after each raw id is parsed;
    /// exact raw-string duplicate behavior remains visible, and no typed key/raw-id
    /// mismatch category is invented because <see cref="MemorialRecord"/> derives
    /// its identity from its immutable typed owner.
    /// </summary>
    public static class MemorialComponentParity
    {
        public static MemorialParityReport Compare(
            IReadOnlyList<MemorialEntry> legacyEntries,
            MemorialComponentStore typed)
        {
            if (legacyEntries == null) throw new ArgumentNullException(nameof(legacyEntries));
            if (typed == null) throw new ArgumentNullException(nameof(typed));

            var report = new MemorialParityReport
            {
                LegacyRows = legacyEntries.Count,
                TypedRows = typed.Count
            };
            var legacyById = new Dictionary<SurvivorId, List<MemorialEntry>>();
            var rawCounts = new Dictionary<string, int>(StringComparer.Ordinal);
            var rawLabels = new Dictionary<string, string>(StringComparer.Ordinal);
            var rawOwners = new Dictionary<string, SurvivorId>(StringComparer.Ordinal);

            for (int i = 0; i < legacyEntries.Count; i++)
            {
                var entry = legacyEntries[i];
                if (entry == null)
                {
                    report.Findings.Add(new MemorialParityFinding(
                        MemorialParityCode.LegacyRowNull,
                        SurvivorId.None,
                        string.Empty,
                        "record",
                        "Memorial entry",
                        "null",
                        "legacy Memorial list contains a null entry"));
                    continue;
                }

                string? rawId = entry.SurvivorId;
                string rawKey = RawKey(rawId);
                if (rawCounts.TryGetValue(rawKey, out int count))
                    rawCounts[rawKey] = count + 1;
                else
                {
                    rawCounts.Add(rawKey, 1);
                    rawLabels.Add(rawKey, rawId ?? string.Empty);
                }

                bool valid = SurvivorId.TryParse(rawId, out var owner, out string idError);
                if (!valid)
                {
                    report.Findings.Add(new MemorialParityFinding(
                        MemorialParityCode.LegacyIdInvalid,
                        SurvivorId.None,
                        rawId ?? string.Empty,
                        "id",
                        "canonical survivor id",
                        rawId == null ? "<null>" : rawId,
                        "legacy Memorial entry id is not a canonical SurvivorId: " + idError));
                    rawOwners[rawKey] = SurvivorId.None;
                    AddNullFieldFindings(report, SurvivorId.None, rawId ?? string.Empty, entry);
                    continue;
                }

                rawOwners[rawKey] = owner;
                AddNullFieldFindings(report, owner, rawId!, entry);
                if (!legacyById.TryGetValue(owner, out var rows))
                {
                    rows = new List<MemorialEntry>();
                    legacyById.Add(owner, rows);
                }
                rows.Add(entry);
            }

            foreach (var pair in rawCounts)
            {
                if (pair.Value <= 1) continue;
                string rawId = rawLabels[pair.Key];
                SurvivorId owner = rawOwners[pair.Key];
                report.Findings.Add(new MemorialParityFinding(
                    MemorialParityCode.LegacyDuplicateId,
                    owner,
                    rawId,
                    "id",
                    "one legacy entry",
                    pair.Value.ToString(CultureInfo.InvariantCulture),
                    $"legacy Memorial list has {pair.Value} entries for raw id '{rawId}'; the first entry is the comparison row"));
            }

            var typedOwners = new List<SurvivorId>();
            foreach (var owner in typed.OwnerIds)
                typedOwners.Add(owner);

            for (int i = 0; i < typedOwners.Count; i++)
            {
                var owner = typedOwners[i];
                if (!typed.TryGet(owner, out var typedRecord) || typedRecord == null)
                {
                    // This cannot occur through the public immutable store API,
                    // but retaining a finding makes the parity boundary defensive.
                    report.Findings.Add(new MemorialParityFinding(
                        MemorialParityCode.TypedRecordExtra,
                        owner,
                        owner.Value,
                        "record",
                        "typed record",
                        "missing",
                        $"typed Memorial owner '{owner}' has no record"));
                    continue;
                }

                if (!legacyById.TryGetValue(owner, out var legacyRows))
                {
                    report.Findings.Add(new MemorialParityFinding(
                        MemorialParityCode.TypedRecordExtra,
                        owner,
                        owner.Value,
                        string.Empty,
                        "legacy record",
                        "typed record",
                        $"typed Memorial component has '{owner}', but the legacy Memorial list has no matching id"));
                    continue;
                }

                CompareFields(report, owner, legacyRows[0], typedRecord);
            }

            foreach (var pair in legacyById)
            {
                if (typed.Contains(pair.Key)) continue;
                report.Findings.Add(new MemorialParityFinding(
                    MemorialParityCode.TypedRecordMissing,
                    pair.Key,
                    pair.Key.Value,
                    string.Empty,
                    "legacy record",
                    "typed record",
                    $"legacy Memorial list has '{pair.Key}', but the typed Memorial component has no record"));
            }

            SortFindings(report.Findings);
            return report;
        }

        private static void CompareFields(
            MemorialParityReport report,
            SurvivorId owner,
            MemorialEntry legacy,
            MemorialRecord typed)
        {
            string rawId = legacy.SurvivorId ?? owner.Value;
            CompareString(report, owner, rawId, "cause", legacy.Cause, typed.Cause);
            CompareInt(report, owner, rawId, "day", legacy.Day, typed.Day);
            CompareInt(report, owner, rawId, "survived_days", legacy.SurvivedDays, typed.SurvivedDays);
            CompareBool(report, owner, rawId, "final_wish_resolved", legacy.FinalWishResolved, typed.FinalWishResolved);
            CompareString(report, owner, rawId, "epitaph", legacy.Epitaph, typed.Epitaph);
            CompareString(report, owner, rawId, "heirloom_item_id", legacy.HeirloomItemId, typed.HeirloomItemId);
            CompareString(report, owner, rawId, "heirloom_recipient_id", legacy.HeirloomRecipientId, typed.HeirloomRecipientId);
            CompareFloat(report, owner, rawId, "morale_delta", legacy.MoraleDelta, typed.MoraleDelta);
        }

        private static void AddNullFieldFindings(
            MemorialParityReport report,
            SurvivorId owner,
            string rawId,
            MemorialEntry entry)
        {
            AddIfNull(report, owner, rawId, "cause", entry.Cause);
            AddIfNull(report, owner, rawId, "epitaph", entry.Epitaph);
            AddIfNull(report, owner, rawId, "heirloom_item_id", entry.HeirloomItemId);
            AddIfNull(report, owner, rawId, "heirloom_recipient_id", entry.HeirloomRecipientId);
        }

        private static void AddIfNull(
            MemorialParityReport report,
            SurvivorId owner,
            string rawId,
            string field,
            string? value)
        {
            if (value != null) return;
            report.Findings.Add(new MemorialParityFinding(
                MemorialParityCode.LegacyFieldNull,
                owner,
                rawId,
                field,
                "non-null string",
                "<null>",
                $"legacy Memorial field '{field}' is null; canonical import defaults it to an empty string"));
        }

        private static void CompareString(
            MemorialParityReport report,
            SurvivorId owner,
            string rawId,
            string field,
            string? expected,
            string actual)
        {
            // Null legacy strings have their own malformed-source finding. Do not
            // duplicate that one defect as a second value mismatch after the typed
            // boundary's intentional null-to-empty defaulting.
            if (expected == null) return;
            if (string.Equals(expected, actual, StringComparison.Ordinal)) return;
            AddFieldMismatch(report, owner, rawId, field, Format(expected), Format(actual));
        }

        private static void CompareInt(
            MemorialParityReport report,
            SurvivorId owner,
            string rawId,
            string field,
            int expected,
            int actual)
        {
            if (expected == actual) return;
            AddFieldMismatch(
                report,
                owner,
                rawId,
                field,
                expected.ToString(CultureInfo.InvariantCulture),
                actual.ToString(CultureInfo.InvariantCulture));
        }

        private static void CompareBool(
            MemorialParityReport report,
            SurvivorId owner,
            string rawId,
            string field,
            bool expected,
            bool actual)
        {
            if (expected == actual) return;
            AddFieldMismatch(report, owner, rawId, field, expected ? "true" : "false", actual ? "true" : "false");
        }

        private static void CompareFloat(
            MemorialParityReport report,
            SurvivorId owner,
            string rawId,
            string field,
            float expected,
            float actual)
        {
            if (expected.Equals(actual)) return;
            AddFieldMismatch(report, owner, rawId, field, Format(expected), Format(actual));
        }

        private static void AddFieldMismatch(
            MemorialParityReport report,
            SurvivorId owner,
            string rawId,
            string field,
            string expected,
            string actual)
        {
            report.Findings.Add(new MemorialParityFinding(
                MemorialParityCode.FieldMismatch,
                owner,
                rawId,
                field,
                expected,
                actual,
                $"legacy value '{expected}' differs from typed value '{actual}'"));
        }

        private static string Format(string? value) => value ?? "<null>";

        private static string Format(float value)
            => value.ToString("R", CultureInfo.InvariantCulture);

        private static string RawKey(string? rawId)
            => rawId == null ? "\u0000" : "\u0001" + rawId;

        private static void SortFindings(List<MemorialParityFinding> findings)
        {
            findings.Sort((left, right) =>
            {
                int comparison = left.SurvivorId.CompareTo(right.SurvivorId);
                if (comparison != 0) return comparison;

                comparison = string.CompareOrdinal(left.RawId, right.RawId);
                if (comparison != 0) return comparison;

                comparison = string.CompareOrdinal(left.Code, right.Code);
                if (comparison != 0) return comparison;

                comparison = string.CompareOrdinal(left.Field, right.Field);
                if (comparison != 0) return comparison;

                return string.CompareOrdinal(left.Message, right.Message);
            });
        }
    }
}
