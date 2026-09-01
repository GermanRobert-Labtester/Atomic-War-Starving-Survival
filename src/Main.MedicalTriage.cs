using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Disease;
using Ashfall.Core.Memorial;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 60 / D5 + D7 — the two bindings that made medicine unreadable rather than
    /// merely thin:
    ///
    /// <para>D5 — illness never reached the sick list. <see cref="SickListSystem"/>
    /// named survivors by <em>dose</em> band only, and nothing bridged disease
    /// progression into it, so the ward's triage surface could not see an infection
    /// advancing. The bridge is pull-based from the disease snapshot each day: it is
    /// idempotent, survives save/restore, and cannot double-apply the way an
    /// event subscription that outlives a session swap would.</para>
    ///
    /// <para>D7 — grief was doubly unbound. <see cref="MemorialSystem.GriefSink"/>
    /// was never assigned, so <c>GriefSink?.ApplyDispersion(...)</c> no-oped, and
    /// <c>SurvivorRelationsSystem.ApplyGrief</c> had no caller outside tests. Binding
    /// them joins two authorities that already exist; it introduces no grief model,
    /// no morale channel and no second relationship ledger.</para>
    ///
    /// Clinical truth stays in Core: stage, band and plan all come from
    /// <see cref="DiseaseTriage"/>, derived from the authored catalog. Nothing here
    /// computes prognosis locally.
    /// </summary>
    public partial class Main
    {
        /// <summary>
        /// Numeric weight reported for a triage line that is already a warning
        /// (terminal / outcome-pending). The briefing's own threshold renders
        /// anything at or above 80 as critical, so this is the "act now" band and
        /// is deliberately a display convention, not a simulation input.
        /// </summary>
        private const float TriageCriticalNumeric = 90f;

        /// <summary>Numeric weight for an active but non-terminal infection.</summary>
        private const float TriageConditionNumeric = 50f;

        /// <summary>
        /// Reconcile the sick list against current infection state and bind the
        /// memorial grief sink. Called from the <c>medical_disease</c> day owner
        /// after the disease tick, and safe to call any number of times.
        /// </summary>
        private void SyncDiseaseTriage(int day, List<DayStateChangeEvent> events)
        {
            SetupDisease();
            SetupDoseLedger();
            if (_disease == null || _doseLedger == null) return;

            EnsureMemorialGriefSink();

            var catalog = _disease.Engine.Catalog;
            if (catalog == null) return;

            var sickList = _doseLedger.SickList;
            if (sickList == null) return;

            // ---- name / move illness cases into the shared band ladder ----
            var seen = new HashSet<string>(StringComparer.Ordinal);
            bool changed = false;

            var patients = _disease.Snapshot?.patients;
            if (patients != null)
            {
                for (int i = 0; i < patients.Count; i++)
                {
                    var p = patients[i];
                    if (p == null || string.IsNullOrEmpty(p.survivor_id)) continue;

                    var def = catalog.GetById(p.disease_id);
                    if (def == null) continue;   // unauthorised id: never guess a prognosis
                    if (!DiseaseTriage.ShouldNameToSickList(def, p.days_sick)) continue;

                    seen.Add(p.survivor_id);

                    int band = DiseaseTriage.SickBandFor(def, p.days_sick);
                    var stage = DiseaseTriage.StageOf(def, p.days_sick);
                    string plan = DiseaseTriage.PalliativePlanFor(def, p.days_sick);

                    var existing = sickList.GetBand(p.survivor_id);
                    bool fromIllness = existing != null
                        && existing.severitySource == SickListSystem.SourceIllness;

                    // Re-diagnosis only when something actually moved: repeated
                    // Diagnose() calls reset releaseDay and would churn the ledger.
                    if (existing == null || band != existing.band || !fromIllness)
                    {
                        sickList.Diagnose(
                            p.survivor_id, band, day,
                            SickListSystem.SourceIllness, p.disease_id);
                        changed = true;
                        ReportTriage(events, p.survivor_id, stage, band, day);
                    }

                    if (!string.IsNullOrEmpty(plan)
                        && !string.Equals(existing?.palliativePlan, plan, StringComparison.Ordinal))
                    {
                        if (sickList.AssignPalliative(p.survivor_id, plan))
                        {
                            changed = true;
                            _journal?.TryAddRawEntry(
                                $"palliative_{p.survivor_id}_{day}",
                                $"{p.survivor_id}: comfort care plan written — {plan}.",
                                null!, day);
                        }
                    }
                }
            }

            // ---- release illness rows that no longer have an active infection ----
            var bands = sickList.Bands;
            if (bands != null)
            {
                for (int i = 0; i < bands.Count; i++)
                {
                    var b = bands[i];
                    if (b == null || string.IsNullOrEmpty(b.survivorId)) continue;
                    if (b.severitySource != SickListSystem.SourceIllness) continue;
                    if (b.releaseDay >= 0) continue;
                    if (seen.Contains(b.survivorId)) continue;

                    if (sickList.Release(b.survivorId, day))
                    {
                        changed = true;
                        events?.Add(new DayStateChangeEvent(
                            "survivor_condition", "medical_disease",
                            b.survivorId, "recovered", 0f));
                    }
                }
            }

            if (changed)
            {
                _doseLedgerDirty = true;   // the sick list rides the dose section
            }
        }

        /// <summary>
        /// Report a triage move on the day-event channel using the vocabulary the
        /// briefing already renders (<c>survivor_condition</c>), so the player is told
        /// a name moved band and can click through to the surface that acts on it.
        /// </summary>
        private void ReportTriage(
            List<DayStateChangeEvent> events, string survivorId,
            DiseaseClinicalStage stage, int band, int day)
        {
            if (events == null) return;

            bool critical = stage == DiseaseClinicalStage.Terminal
                || stage == DiseaseClinicalStage.OutcomePending;

            events.Add(new DayStateChangeEvent(
                "survivor_condition", "medical_disease",
                survivorId,
                critical ? "critical" : DiseaseTriage.StageToken(stage),
                critical ? TriageCriticalNumeric : TriageConditionNumeric));
        }

        /// <summary>
        /// Bind the memorial grief sink to the relationship authority the social
        /// coordinator already owns. Idempotent, and never overwrites a sink another
        /// path installed first.
        /// </summary>
        private void EnsureMemorialGriefSink()
        {
            SetupMemorial();
            if (_memorial == null || _memorial.GriefSink != null) return;

            // Prefer the coordinator's instance so grief lands in the same ledger the
            // social systems mutate; fall back to the core relations instance the
            // coordinator was built from when social setup has not run yet.
            SetupSurvivorSocial();
            var relations = _survivorSocial?.Relations ?? _survivorRelationsCore;
            if (relations == null) return;

            _memorial.GriefSink = new RelationsGriefSink(
                relations,
                id => _survivors?.Find(id)?.IsAlive ?? true);
        }
    }
}
