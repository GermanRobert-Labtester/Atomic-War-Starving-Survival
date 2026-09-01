// SPDX-License-Identifier: MIT
// Task #132 — Legacy Needs to typed component parity harness.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Ashfall.Core.Survivors
{
    /// <summary>Stable categories emitted by the Needs dual-run parity check.</summary>
    public static class NeedsParityCode
    {
        public const string LegacyIdInvalid = "legacy_id_invalid";
        public const string LegacyDuplicateId = "legacy_duplicate_id";
        public const string TypedIdMismatch = "typed_id_mismatch";
        public const string TypedRecordMissing = "typed_record_missing";
        public const string TypedRecordExtra = "typed_record_extra";
        public const string FieldMismatch = "field_mismatch";
    }

    /// <summary>
    /// One deterministic Needs parity finding. For a field mismatch, Expected is
    /// the legacy <see cref="NeedsSystem" /> value and Actual is the typed
    /// component value. Missing and extra records use an empty side.
    /// </summary>
    public sealed class NeedsParityFinding
    {
        public string Code { get; }
        public SurvivorId SurvivorId { get; }
        public string RawId { get; }
        public string Field { get; }
        public string Expected { get; }
        public string Actual { get; }
        public string Message { get; }

        public NeedsParityFinding(
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
            return $"Needs parity [{Code}] {id}{field}: {Message}";
        }
    }

    /// <summary>Outcome of comparing the legacy list with the typed component.</summary>
    public sealed class NeedsParityReport
    {
        public int LegacyRows { get; internal set; }
        public int TypedRows { get; internal set; }
        public List<NeedsParityFinding> Findings { get; } = new List<NeedsParityFinding>();

        public bool IsMatch => Findings.Count == 0;
        public int FindingCount => Findings.Count;

        public string Describe()
        {
            var sb = new StringBuilder();
            sb.Append("NEEDS PARITY — legacy=")
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
            => $"[NeedsParity] legacy={LegacyRows} typed={TypedRows} findings={Findings.Count}";
    }

    /// <summary>
    /// Compares the pre-migration <see cref="NeedsSystem" /> registration list to
    /// the typed <see cref="NeedsComponentStore" />.
    ///
    /// <para>The comparison deliberately does not tick or mutate either side. It
    /// reports invalid/duplicate legacy ids, missing and extra typed records, raw
    /// id/key disagreement, and every persisted Needs field. Findings are sorted by
    /// canonical id, then code and field, so a host can log them without making
    /// diagnostics depend on registration or dictionary order.</para>
    /// </summary>
    public static class NeedsComponentParity
    {
        public static NeedsParityReport Compare(
            NeedsSystem legacy,
            NeedsComponentStore typed)
        {
            if (legacy == null) throw new ArgumentNullException(nameof(legacy));
            if (typed == null) throw new ArgumentNullException(nameof(typed));

            var report = new NeedsParityReport
            {
                LegacyRows = legacy.Registered.Count,
                TypedRows = typed.Count
            };

            var legacyById = new Dictionary<SurvivorId, List<SurvivorNeedsState>>();
            var invalidLegacyCounts = new Dictionary<string, int>(StringComparer.Ordinal);

            for (int i = 0; i < legacy.Registered.Count; i++)
            {
                var state = legacy.Registered[i];
                if (state == null)
                {
                    report.Findings.Add(new NeedsParityFinding(
                        NeedsParityCode.LegacyIdInvalid,
                        SurvivorId.None,
                        string.Empty,
                        "id",
                        "canonical survivor id",
                        "null",
                        "legacy registration contains a null Needs state"));
                    continue;
                }

                if (!SurvivorId.TryParse(state.Id, out var id))
                {
                    if (invalidLegacyCounts.TryGetValue(state.Id, out int invalidCount))
                        invalidLegacyCounts[state.Id] = invalidCount + 1;
                    else
                        invalidLegacyCounts.Add(state.Id, 1);

                    report.Findings.Add(new NeedsParityFinding(
                        NeedsParityCode.LegacyIdInvalid,
                        SurvivorId.None,
                        state.Id,
                        "id",
                        "canonical survivor id",
                        state.Id,
                        "legacy Needs state id is not a canonical SurvivorId"));
                    continue;
                }

                if (!legacyById.TryGetValue(id, out var rows))
                {
                    rows = new List<SurvivorNeedsState>();
                    legacyById.Add(id, rows);
                }
                rows.Add(state);
            }

            foreach (var pair in invalidLegacyCounts)
            {
                if (pair.Value <= 1) continue;
                report.Findings.Add(new NeedsParityFinding(
                    NeedsParityCode.LegacyDuplicateId,
                    SurvivorId.None,
                    pair.Key,
                    "id",
                    "one legacy registration",
                    pair.Value.ToString(CultureInfo.InvariantCulture),
                    $"legacy NeedsSystem has {pair.Value} registrations for malformed raw id '{pair.Key}'"));
            }

            foreach (var pair in legacyById)
            {
                if (pair.Value.Count <= 1) continue;
                report.Findings.Add(new NeedsParityFinding(
                    NeedsParityCode.LegacyDuplicateId,
                    pair.Key,
                    pair.Key.Value,
                    "id",
                    "one legacy registration",
                    pair.Value.Count.ToString(CultureInfo.InvariantCulture),
                    $"legacy NeedsSystem has {pair.Value.Count} registrations for '{pair.Key}'; the first registration is the comparison row"));
            }

            var typedOwners = new List<SurvivorId>();
            foreach (var owner in typed.OwnerIds)
                typedOwners.Add(owner);

            for (int i = 0; i < typedOwners.Count; i++)
            {
                var owner = typedOwners[i];
                if (!typed.TryGet(owner, out var typedState) || typedState == null)
                {
                    report.Findings.Add(new NeedsParityFinding(
                        NeedsParityCode.TypedIdMismatch,
                        owner,
                        owner.Value,
                        "state",
                        "typed state",
                        "missing",
                        $"typed owner '{owner}' is present in the key set but has no state"));
                    continue;
                }

                if (!SurvivorId.TryParse(typedState.Id, out var embeddedId) || embeddedId != owner)
                {
                    report.Findings.Add(new NeedsParityFinding(
                        NeedsParityCode.TypedIdMismatch,
                        owner,
                        typedState.Id,
                        "id",
                        owner.Value,
                        typedState.Id,
                        $"typed owner key '{owner}' does not match its state's raw id '{typedState.Id}'"));
                }

                if (!legacyById.TryGetValue(owner, out var legacyRows))
                {
                    report.Findings.Add(new NeedsParityFinding(
                        NeedsParityCode.TypedRecordExtra,
                        owner,
                        owner.Value,
                        string.Empty,
                        "legacy record",
                        "typed record",
                        $"typed Needs component has '{owner}', but the legacy NeedsSystem has no matching id"));
                    continue;
                }

                // NeedsSystem.Get(id) and the legacy simulation use the first row.
                // Compare against that same row while still reporting every duplicate.
                CompareFields(report, owner, legacyRows[0], typedState);
            }

            foreach (var pair in legacyById)
            {
                if (typed.Contains(pair.Key)) continue;
                report.Findings.Add(new NeedsParityFinding(
                    NeedsParityCode.TypedRecordMissing,
                    pair.Key,
                    pair.Key.Value,
                    string.Empty,
                    "legacy record",
                    "typed record",
                    $"legacy NeedsSystem has '{pair.Key}', but the typed Needs component has no record"));
            }

            SortFindings(report.Findings);
            return report;
        }

        private static void CompareFields(
            NeedsParityReport report,
            SurvivorId id,
            SurvivorNeedsState legacy,
            SurvivorNeedsState typed)
        {
            CompareFloat(report, id, "hunger", legacy.Hunger, typed.Hunger);
            CompareFloat(report, id, "thirst", legacy.Thirst, typed.Thirst);
            CompareFloat(report, id, "fatigue", legacy.Fatigue, typed.Fatigue);
            CompareFloat(report, id, "warmth", legacy.Warmth, typed.Warmth);
            CompareFloat(report, id, "morale", legacy.Morale, typed.Morale);
            CompareFloat(report, id, "health", legacy.Health, typed.Health);
            CompareFloat(report, id, "hygiene", legacy.Hygiene, typed.Hygiene);
            CompareBool(report, id, "was_hunger_critical", legacy.WasHungerCritical, typed.WasHungerCritical);
            CompareBool(report, id, "was_thirst_critical", legacy.WasThirstCritical, typed.WasThirstCritical);
            CompareBool(report, id, "was_warmth_critical", legacy.WasWarmthCritical, typed.WasWarmthCritical);
            CompareFloat(report, id, "max_health_cap", legacy.MaxHealthCap, typed.MaxHealthCap);
            CompareBool(report, id, "is_alive", legacy.IsAlive, typed.IsAlive);
            CompareBool(report, id, "is_dead", legacy.IsDead, typed.IsDead);
            // IsAliveState is derived, not a wire field, but it is the host-facing
            // mirror that determines whether the simulation ticks this state.
            CompareBool(report, id, "is_alive_state", legacy.IsAliveState, typed.IsAliveState);
        }

        private static void CompareFloat(
            NeedsParityReport report,
            SurvivorId id,
            string field,
            float expected,
            float actual)
        {
            if (expected.Equals(actual)) return;
            AddFieldMismatch(report, id, field, Format(expected), Format(actual));
        }

        private static void CompareBool(
            NeedsParityReport report,
            SurvivorId id,
            string field,
            bool expected,
            bool actual)
        {
            if (expected == actual) return;
            AddFieldMismatch(report, id, field, expected ? "true" : "false", actual ? "true" : "false");
        }

        private static void AddFieldMismatch(
            NeedsParityReport report,
            SurvivorId id,
            string field,
            string expected,
            string actual)
        {
            report.Findings.Add(new NeedsParityFinding(
                NeedsParityCode.FieldMismatch,
                id,
                id.Value,
                field,
                expected,
                actual,
                $"legacy value '{expected}' differs from typed value '{actual}'"));
        }

        private static string Format(float value)
            => value.ToString("R", CultureInfo.InvariantCulture);

        private static void SortFindings(List<NeedsParityFinding> findings)
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
