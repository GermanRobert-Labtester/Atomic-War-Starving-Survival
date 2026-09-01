// SPDX-License-Identifier: MIT
// Task #132 — Survivor referential integrity and aggregate invariants.
using System;
using System.Collections.Generic;
using System.Text;

namespace Ashfall.Core.Survivors
{
    /// <summary>How seriously to take an integrity finding.</summary>
    public enum SurvivorIntegritySeverity
    {
        /// <summary>
        /// A declared invariant the owning domain does not enforce yet. Expected
        /// during migration; must reach zero before the corresponding legacy
        /// authority is removed.
        /// </summary>
        Warning = 0,

        /// <summary>
        /// Structurally broken state — a component belonging to nobody, a lifecycle
        /// value that is not a state, an expedition whose member does not exist.
        /// Never acceptable.
        /// </summary>
        Error = 1
    }

    /// <summary>Stable codes for integrity findings.</summary>
    public static class SurvivorIntegrityCode
    {
        public const string LifecycleIllegalState = "lifecycle_illegal_state";
        public const string LifecycleRevisionInvalid = "lifecycle_revision_invalid";
        public const string LifecycleDayBeforeJoin = "lifecycle_day_before_join";
        public const string DefinitionMissing = "definition_missing";
        public const string AwayWithoutExpedition = "away_without_expedition";
        public const string ExpeditionOnNonAway = "expedition_on_non_away";
        public const string IterationOrderUnstable = "iteration_order_unstable";
        public const string ComponentOwnerUnknown = "component_owner_unknown";
        public const string ComponentOnDeceased = "component_on_deceased";
        public const string ComponentMissingForEligible = "component_missing_for_eligible";
        public const string ExpeditionMemberUnknown = "expedition_member_unknown";
        public const string ExpeditionMemberNotAway = "expedition_member_not_away";
        public const string ExpeditionIdMismatch = "expedition_id_mismatch";
        public const string AwayWithoutActiveExpedition = "away_without_active_expedition";
        public const string AssignmentOwnerUnknown = "assignment_owner_unknown";
        public const string AssignmentLifecycleIneligible = "assignment_lifecycle_ineligible";
    }

    /// <summary>One integrity finding, phrased to be actionable in a log.</summary>
    public sealed class SurvivorIntegrityFinding
    {
        public SurvivorIntegritySeverity Severity { get; }
        public string Code { get; }
        public SurvivorId SurvivorId { get; }

        /// <summary>Owning domain or component, when the finding is about one. Empty otherwise.</summary>
        public string Component { get; }

        public string Message { get; }

        public SurvivorIntegrityFinding(
            SurvivorIntegritySeverity severity,
            string code,
            SurvivorId survivorId,
            string component,
            string message)
        {
            Severity = severity;
            Code = code ?? string.Empty;
            SurvivorId = survivorId;
            Component = component ?? string.Empty;
            Message = message ?? string.Empty;
        }

        public override string ToString()
        {
            string label = Severity == SurvivorIntegritySeverity.Error ? "failure" : "warning";
            string where = string.IsNullOrEmpty(Component) ? string.Empty : $" [{Component}]";
            return $"Survivor invariant {label}{where}: {Message} ({Code})";
        }
    }

    /// <summary>Outcome of an integrity sweep.</summary>
    public sealed class SurvivorIntegrityReport
    {
        public List<SurvivorIntegrityFinding> Findings { get; } = new List<SurvivorIntegrityFinding>();

        public int SurvivorsChecked { get; internal set; }
        public int ComponentStoresChecked { get; internal set; }

        public int ErrorCount
        {
            get
            {
                int n = 0;
                for (int i = 0; i < Findings.Count; i++)
                    if (Findings[i].Severity == SurvivorIntegritySeverity.Error) n++;
                return n;
            }
        }

        public int WarningCount => Findings.Count - ErrorCount;

        /// <summary>True when no errors were found. Warnings do not invalidate a campaign.</summary>
        public bool IsValid => ErrorCount == 0;

        /// <summary>True when the campaign is fully coherent — no errors and no warnings.</summary>
        public bool IsClean => Findings.Count == 0;

        /// <summary>Findings of one severity, in discovery order.</summary>
        public IEnumerable<SurvivorIntegrityFinding> BySeverity(SurvivorIntegritySeverity severity)
        {
            for (int i = 0; i < Findings.Count; i++)
                if (Findings[i].Severity == severity) yield return Findings[i];
        }

        /// <summary>Multi-line human-readable report.</summary>
        public string Describe()
        {
            var sb = new StringBuilder();
            sb.Append("SURVIVOR INTEGRITY — ")
              .Append(SurvivorsChecked).Append(" survivor(s), ")
              .Append(ComponentStoresChecked).Append(" component store(s): ")
              .Append(ErrorCount).Append(" error(s), ")
              .Append(WarningCount).Append(" warning(s)");
            for (int i = 0; i < Findings.Count; i++)
                sb.Append('\n').Append("  ").Append(Findings[i]);
            return sb.ToString();
        }

        public override string ToString()
            => $"[SurvivorIntegrity] survivors={SurvivorsChecked} errors={ErrorCount} warnings={WarningCount}";
    }

    /// <summary>
    /// Facts the validator cannot discover on its own, supplied by the caller.
    ///
    /// <para>The validator deliberately does not reference <c>ExpeditionSystem</c> or
    /// <c>DutyRosterSystem</c>. Those domains keep their own state and their own
    /// rules; the caller passes in the small projection needed to cross-check
    /// lifecycle coherence. Anything left null is simply not checked.</para>
    /// </summary>
    public sealed class SurvivorIntegrityInputs
    {
        /// <summary>
        /// Active expeditions as expedition id to participant. Mirrors
        /// <c>ExpeditionSystem.Active</c>, whose key is the survivor id and whose
        /// <c>expeditionId</c> is on the state.
        /// </summary>
        public IEnumerable<KeyValuePair<string, SurvivorId>>? ActiveExpeditions { get; set; }

        /// <summary>
        /// Survivors currently holding an active shelter duty. Mirrors the duty
        /// roster's assignments.
        /// </summary>
        public IEnumerable<SurvivorId>? AssignedSurvivors { get; set; }
    }

    /// <summary>
    /// Checks that the canonical survivor model is internally coherent and that no
    /// domain has invented a survivor.
    ///
    /// <para><b>The central invariant:</b> every live survivor id resolves to exactly
    /// one aggregate, and no component may exist for an unknown id. Everything else
    /// here follows from that.</para>
    ///
    /// <para>Pure and read-only — it never repairs anything. Repair belongs to the
    /// restore path, which can describe what it changed;
    /// silently correcting state during validation would hide the divergence this
    /// exists to surface.</para>
    ///
    /// <para><b>Cost.</b> Linear in survivors plus component records. Intended for
    /// post-migration, post-restore, after lifecycle transactions in tests, and
    /// repository gates — not every frame.</para>
    /// </summary>
    public static class SurvivorIntegrityValidator
    {
        /// <summary>Run a full integrity sweep.</summary>
        public static SurvivorIntegrityReport Validate(
            SurvivorEntityStore store,
            SurvivorIntegrityInputs? inputs = null)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));

            var report = new SurvivorIntegrityReport
            {
                SurvivorsChecked = store.Count,
                ComponentStoresChecked = store.ComponentStores.Count
            };

            ValidateAggregates(store, report);
            ValidateIterationOrder(store, report);
            ValidateComponents(store, report);

            if (inputs?.ActiveExpeditions != null)
                ValidateExpeditions(store, inputs.ActiveExpeditions, report);

            if (inputs?.AssignedSurvivors != null)
                ValidateAssignments(store, inputs.AssignedSurvivors, report);

            return report;
        }

        // ── Aggregate invariants ───────────────────────────────────────

        private static void ValidateAggregates(SurvivorEntityStore store, SurvivorIntegrityReport report)
        {
            var survivors = store.Survivors;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];

                if (!SurvivorLifecycle.IsLegalState(s.Lifecycle))
                {
                    Error(report, SurvivorIntegrityCode.LifecycleIllegalState, s.Id, string.Empty,
                        $"{s.Id} holds lifecycle value {(int)s.Lifecycle}, which is not a legal state");
                }

                if (s.Revision < 1L)
                {
                    Error(report, SurvivorIntegrityCode.LifecycleRevisionInvalid, s.Id, string.Empty,
                        $"{s.Id} has revision {s.Revision}; revisions start at 1");
                }

                if (string.IsNullOrEmpty(s.DefinitionId))
                {
                    Error(report, SurvivorIntegrityCode.DefinitionMissing, s.Id, string.Empty,
                        $"{s.Id} has no definition link");
                }

                bool away = s.Lifecycle == SurvivorLifecycleState.Away;
                bool hasExpedition = !string.IsNullOrEmpty(s.ActiveExpeditionId);

                if (away && !hasExpedition)
                {
                    Error(report, SurvivorIntegrityCode.AwayWithoutExpedition, s.Id, string.Empty,
                        $"{s.Id} is Away but carries no expedition id");
                }
                else if (!away && hasExpedition)
                {
                    Error(report, SurvivorIntegrityCode.ExpeditionOnNonAway, s.Id, string.Empty,
                        $"{s.Id} is {s.Lifecycle} but still carries expedition '{s.ActiveExpeditionId}'");
                }

                if (s.LifecycleDay < s.JoinedDay)
                {
                    Warning(report, SurvivorIntegrityCode.LifecycleDayBeforeJoin, s.Id, string.Empty,
                        $"{s.Id} changed state on day {s.LifecycleDay}, before joining on day {s.JoinedDay}");
                }
            }
        }

        /// <summary>
        /// The canonical order must be strictly ascending by ordinal id. A duplicate
        /// or an out-of-order entry means the simulation order is not reproducible,
        /// which breaks determinism even when every aggregate looks fine.
        /// </summary>
        private static void ValidateIterationOrder(SurvivorEntityStore store, SurvivorIntegrityReport report)
        {
            var survivors = store.Survivors;
            for (int i = 1; i < survivors.Count; i++)
            {
                int cmp = survivors[i - 1].Id.CompareTo(survivors[i].Id);
                if (cmp < 0) continue;

                string detail = cmp == 0
                    ? $"'{survivors[i].Id}' appears twice in canonical order"
                    : $"'{survivors[i - 1].Id}' precedes '{survivors[i].Id}' out of ordinal order";

                Error(report, SurvivorIntegrityCode.IterationOrderUnstable, survivors[i].Id, string.Empty,
                    $"canonical survivor order is not strictly ascending — {detail}");
            }
        }

        // ── Referential integrity ──────────────────────────────────────

        private static void ValidateComponents(SurvivorEntityStore store, SurvivorIntegrityReport report)
        {
            var stores = store.ComponentStores;
            for (int i = 0; i < stores.Count; i++)
            {
                var component = stores[i];
                string name = component.ComponentName;

                foreach (var owner in component.OwnerIds)
                {
                    if (!store.TryGet(owner, out var aggregate))
                    {
                        Error(report, SurvivorIntegrityCode.ComponentOwnerUnknown, owner, name,
                            $"{name} holds state for '{owner}', who is not a survivor in this campaign");
                        continue;
                    }

                    if (aggregate.IsDeceased && !component.RetainsHistoryAfterDeath)
                    {
                        Warning(report, SurvivorIntegrityCode.ComponentOnDeceased, owner, name,
                            $"{owner} is {aggregate.Lifecycle} but {name} still holds active state, and {name} does not retain history");
                    }
                }

                if (component.Cardinality != SurvivorComponentCardinality.OnePerEligible) continue;

                var survivors = store.Survivors;
                for (int s = 0; s < survivors.Count; s++)
                {
                    var aggregate = survivors[s];
                    if (!aggregate.IsAlive) continue;
                    if (component.Contains(aggregate.Id)) continue;

                    Warning(report, SurvivorIntegrityCode.ComponentMissingForEligible, aggregate.Id, name,
                        $"{aggregate.Id} is {aggregate.Lifecycle} but {name} has no record, and {name} requires one per eligible survivor");
                }
            }
        }

        // ── Cross-domain coherence ─────────────────────────────────────

        private static void ValidateExpeditions(
            SurvivorEntityStore store,
            IEnumerable<KeyValuePair<string, SurvivorId>> activeExpeditions,
            SurvivorIntegrityReport report)
        {
            var seen = new Dictionary<SurvivorId, string>();

            foreach (var kv in activeExpeditions)
            {
                string expeditionId = kv.Key ?? string.Empty;
                SurvivorId member = kv.Value;

                if (!store.TryGet(member, out var aggregate))
                {
                    Error(report, SurvivorIntegrityCode.ExpeditionMemberUnknown, member, "expedition",
                        $"expedition '{expeditionId}' lists '{member}', who is not a survivor in this campaign");
                    continue;
                }

                if (seen.TryGetValue(member, out string other))
                {
                    Error(report, SurvivorIntegrityCode.ExpeditionIdMismatch, member, "expedition",
                        $"{member} is listed on two active expeditions at once ('{other}' and '{expeditionId}')");
                    continue;
                }
                seen[member] = expeditionId;

                if (aggregate.Lifecycle != SurvivorLifecycleState.Away)
                {
                    Error(report, SurvivorIntegrityCode.ExpeditionMemberNotAway, member, "expedition",
                        $"{member} is {aggregate.Lifecycle} but expedition '{expeditionId}' still lists them as deployed");
                    continue;
                }

                if (!string.Equals(aggregate.ActiveExpeditionId, expeditionId, StringComparison.Ordinal))
                {
                    Error(report, SurvivorIntegrityCode.ExpeditionIdMismatch, member, "expedition",
                        $"{member} is Away on '{aggregate.ActiveExpeditionId}' but expedition '{expeditionId}' claims them");
                }
            }

            var survivors = store.Survivors;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s.Lifecycle != SurvivorLifecycleState.Away) continue;
                if (seen.ContainsKey(s.Id)) continue;

                Error(report, SurvivorIntegrityCode.AwayWithoutActiveExpedition, s.Id, "expedition",
                    $"{s.Id} is Away on '{s.ActiveExpeditionId}' but no active expedition lists them");
            }
        }

        private static void ValidateAssignments(
            SurvivorEntityStore store,
            IEnumerable<SurvivorId> assigned,
            SurvivorIntegrityReport report)
        {
            foreach (var id in assigned)
            {
                if (!store.TryGet(id, out var aggregate))
                {
                    Error(report, SurvivorIntegrityCode.AssignmentOwnerUnknown, id, "assignment",
                        $"an active duty is assigned to '{id}', who is not a survivor in this campaign");
                    continue;
                }

                if (SurvivorLifecycle.IsAssignmentEligible(aggregate.Lifecycle)) continue;

                // Warning, not error: the duty roster has no lifecycle awareness yet,
                // so this is a real divergence the game currently permits.
                Warning(report, SurvivorIntegrityCode.AssignmentLifecycleIneligible, id, "assignment",
                    $"{id} is {aggregate.Lifecycle} but still holds an active duty");
            }
        }

        // ── Finding helpers ────────────────────────────────────────────

        private static void Error(
            SurvivorIntegrityReport report, string code, SurvivorId id, string component, string message)
            => report.Findings.Add(new SurvivorIntegrityFinding(
                SurvivorIntegritySeverity.Error, code, id, component, message));

        private static void Warning(
            SurvivorIntegrityReport report, string code, SurvivorId id, string component, string message)
            => report.Findings.Add(new SurvivorIntegrityFinding(
                SurvivorIntegritySeverity.Warning, code, id, component, message));
    }
}
