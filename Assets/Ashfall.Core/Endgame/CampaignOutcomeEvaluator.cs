using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Verdict;

namespace Ashfall.Core.Endgame
{
    /// <summary>
    /// Inputs required to evaluate an authoritative campaign outcome snapshot.
    /// Pure Core DTO with zero engine dependencies (Invariant 1 / FX-01).
    /// </summary>
    public sealed class CampaignOutcomeEvaluationInput
    {
        public int TotalDaysSurvived { get; set; }
        public int LivingDwellerCount { get; set; }
        public int TotalDeathsRecorded { get; set; }

        // ── Grand Treaty Inputs ──────────────────────────────────────
        public bool? GrandTreatySignedOverride { get; set; }
        public RegionalTreatyState? TreatiesState { get; set; }

        // ── Tempest / Verdict Inputs ─────────────────────────────────
        public bool? TempestDecommissionedOverride { get; set; }
        public ReckoningState? VerdictReckoningState { get; set; }
        public int EnrolledEvidenceCount { get; set; }

        // ── Debt Inputs ──────────────────────────────────────────────
        public bool? DebtLedgersBurnedOverride { get; set; }
        public bool LedgerTampered { get; set; }
        public IReadOnlyList<DebtContract>? Debts { get; set; }

        // ── Children / Generational Inputs ───────────────────────────
        public bool? ChildrenSurvivedOverride { get; set; }
        public int ChildrenCount { get; set; }
        public IReadOnlyList<CohortChild>? CohortChildren { get; set; }
        public GenerationalSuccessionSaveState? GenerationalState { get; set; }

        // ── Vel Secret Inputs ────────────────────────────────────────
        public bool? VelSecretExposedOverride { get; set; }

        // ── Shared Flag Ledger ───────────────────────────────────────
        public IFlagLedger? Flags { get; set; }
    }

    /// <summary>
    /// Evaluates canonical campaign authorities into an authoritative, immutable
    /// <see cref="CampaignOutcomeSnapshot"/> with detailed forensic provenance (FX-01).
    /// </summary>
    public static class CampaignOutcomeEvaluator
    {
        private static readonly EpilogueMatrixRuntime Runtime = new EpilogueMatrixRuntime();

        public static CampaignOutcomeSnapshot Evaluate(CampaignOutcomeEvaluationInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var snapshot = new CampaignOutcomeSnapshot();
            var trace = snapshot.OutcomeTrace;

            // 1. Census & Day Metrics
            snapshot.TotalDaysSurvived = Math.Max(0, input.TotalDaysSurvived);
            snapshot.LivingDwellerCount = Math.Max(0, input.LivingDwellerCount);
            snapshot.TotalDeathsRecorded = Math.Max(0, input.TotalDeathsRecorded);

            trace.Add($"[Census] Total days survived: {snapshot.TotalDaysSurvived}.");
            trace.Add($"[Census] Living dweller count: {snapshot.LivingDwellerCount}.");
            trace.Add($"[Census] Recorded deaths: {snapshot.TotalDeathsRecorded}.");

            // 2. Grand Treaty Signed
            snapshot.GrandTreatySigned = EvaluateGrandTreaty(input, snapshot, trace);

            // 3. Tempest Decommissioned
            snapshot.TempestDecommissioned = EvaluateTempestDecommissioned(input, snapshot, trace);

            // 4. Debt Ledgers Burned
            snapshot.DebtLedgersBurned = EvaluateDebtLedgersBurned(input, snapshot, trace);

            // 5. Children Survived
            snapshot.ChildrenSurvived = EvaluateChildrenSurvived(input, snapshot, trace);

            // 6. Vel Secret Exposed
            snapshot.VelSecretExposed = EvaluateVelSecretExposed(input, snapshot, trace);

            // 7. Matrix Narrative & Classifications
            var ctx = snapshot.ToContext();
            snapshot.Fate = Runtime.EvaluateRegionalFate(ctx);
            snapshot.Demographics = Runtime.EvaluateDemographics(ctx);
            snapshot.MoralStanding = Runtime.EvaluateMoralStanding(ctx);
            snapshot.NarrativeProse = Runtime.GenerateEpilogueNarrative(ctx);

            trace.Add($"[Resolution] Regional Fate evaluated to: '{snapshot.Fate}' (GrandTreaty={snapshot.GrandTreatySigned}, TempestDecommissioned={snapshot.TempestDecommissioned}, DebtBurned={snapshot.DebtLedgersBurned}, Deaths={snapshot.TotalDeathsRecorded}).");
            trace.Add($"[Resolution] Demographic Legacy evaluated to: '{snapshot.Demographics}' (Living={snapshot.LivingDwellerCount}, ChildrenSurvived={snapshot.ChildrenSurvived}).");
            trace.Add($"[Resolution] Moral Standing evaluated to: '{snapshot.MoralStanding}' (DebtBurned={snapshot.DebtLedgersBurned}, ChildrenSurvived={snapshot.ChildrenSurvived}).");

            return snapshot;
        }

        private static bool EvaluateGrandTreaty(CampaignOutcomeEvaluationInput input, CampaignOutcomeSnapshot snapshot, List<string> trace)
        {
            if (input.GrandTreatySignedOverride.HasValue)
            {
                trace.Add($"[Treaty] GrandTreatySigned set via explicit override: {input.GrandTreatySignedOverride.Value}.");
                return input.GrandTreatySignedOverride.Value;
            }

            // Only grand/constitution-specific flags qualify. Generic
            // flag_treaty_ratified / flag_peace_treaty_ratified must not mint
            // a grand-treaty outcome (ordinary regional ratification).
            if (input.Flags != null && (input.Flags.IsSet("flag_grand_treaty_signed")
                || input.Flags.IsSet("flag_constitution_of_the_valley_ratified")))
            {
                trace.Add("[Treaty] GrandTreatySigned is TRUE (grand/constitution ratification flag active).");
                return true;
            }

            if (input.TreatiesState?.treaties != null && input.TreatiesState.treaties.Count > 0)
            {
                int ratified = input.TreatiesState.treaties.Count(t => t != null && (t.status == TreatyStatus.Ratified || t.status == TreatyStatus.Active));
                snapshot.RatifiedTreatiesCount = ratified;

                var grandTreaty = input.TreatiesState.treaties.FirstOrDefault(t => t != null
                    && (t.status == TreatyStatus.Ratified || t.status == TreatyStatus.Active)
                    && (t.treatyId.Contains("grand", StringComparison.OrdinalIgnoreCase)
                        || t.treatyId.Contains("constitution", StringComparison.OrdinalIgnoreCase)
                        || t.treatyId == "treaty_16_the_constitution_of_the_valley_of_tessarat"));

                if (grandTreaty != null)
                {
                    trace.Add($"[Treaty] GrandTreatySigned is TRUE: treaty '{grandTreaty.treatyId}' is ratified/active.");
                    return true;
                }

                // Ordinary regional treaties must not mint a grand-treaty outcome.
                // Only constitution/grand treaty ids (or explicit flags/override) qualify.
                if (ratified > 0)
                {
                    trace.Add($"[Treaty] GrandTreatySigned is FALSE: {ratified} regional treaty instance(s) ratified/active, but none is a grand/constitution treaty.");
                    return false;
                }
            }

            trace.Add("[Treaty] GrandTreatySigned is FALSE: no ratified grand/constitution treaty or treaty flags found.");
            return false;
        }

        private static bool EvaluateTempestDecommissioned(CampaignOutcomeEvaluationInput input, CampaignOutcomeSnapshot snapshot, List<string> trace)
        {
            if (input.TempestDecommissionedOverride.HasValue)
            {
                trace.Add($"[Tempest] TempestDecommissioned set via explicit override: {input.TempestDecommissionedOverride.Value}.");
                return input.TempestDecommissionedOverride.Value;
            }

            if (input.Flags != null && (input.Flags.IsSet("flag_tempest_decommissioned") || input.Flags.IsSet("flag_verdict_counted")))
            {
                trace.Add("[Tempest] TempestDecommissioned is TRUE (decommission flag active in consequence ledger).");
                return true;
            }

            if (input.VerdictReckoningState != null)
            {
                if (VerdictEndingEvaluator.IsTempestDecommissioned(input.VerdictReckoningState))
                {
                    trace.Add("[Tempest] TempestDecommissioned is TRUE: Verdict Reckoning recount was presented.");
                    snapshot.VerdictEndingKey = VerdictEndingEvaluator.EndingKeyCounted;
                    return true;
                }

                string? resolved = VerdictEndingEvaluator.DecideEnding(input.VerdictReckoningState, input.EnrolledEvidenceCount, input.TotalDaysSurvived);
                snapshot.VerdictEndingKey = resolved ?? string.Empty;
                if (resolved == VerdictEndingEvaluator.EndingKeyCounted)
                {
                    trace.Add($"[Tempest] TempestDecommissioned is TRUE: Verdict ending decided as '{resolved}'.");
                    return true;
                }

                trace.Add($"[Tempest] TempestDecommissioned is FALSE: Reckoning phase is {input.VerdictReckoningState.phase}, ending is '{resolved ?? "unresolved"}'.");
                return false;
            }

            trace.Add("[Tempest] TempestDecommissioned is FALSE: no Verdict reckoning state or flag present.");
            return false;
        }

        private static bool EvaluateDebtLedgersBurned(CampaignOutcomeEvaluationInput input, CampaignOutcomeSnapshot snapshot, List<string> trace)
        {
            if (input.DebtLedgersBurnedOverride.HasValue)
            {
                trace.Add($"[Debt] DebtLedgersBurned set via explicit override: {input.DebtLedgersBurnedOverride.Value}.");
                return input.DebtLedgersBurnedOverride.Value;
            }

            if (input.LedgerTampered)
            {
                trace.Add("[Debt] DebtLedgersBurned is TRUE: LedgerDebtSystem.LedgerTampered is true.");
                return true;
            }

            if (input.Flags != null && (input.Flags.IsSet("flag_debt_ledgers_burned")
                || input.Flags.IsSet("flag_roster_burned")
                || input.Flags.IsSet("flag_debts_cleared")))
            {
                trace.Add("[Debt] DebtLedgersBurned is TRUE: debt ledger burn flag active in consequence ledger.");
                return true;
            }

            if (input.Debts != null)
            {
                int unpaid = input.Debts.Count(d => d != null && d.signed && !d.paid && !d.forgiven);
                snapshot.ActiveDebtsCount = unpaid;

                if (input.Debts.Count > 0 && unpaid == 0)
                {
                    trace.Add($"[Debt] DebtLedgersBurned is TRUE: all {input.Debts.Count} debt contracts are settled or forgiven.");
                    return true;
                }

                if (unpaid > 0)
                {
                    trace.Add($"[Debt] DebtLedgersBurned is FALSE: {unpaid} signed unpaid debt contract(s) remain on ledger.");
                    return false;
                }
            }

            trace.Add("[Debt] DebtLedgersBurned is FALSE: no debt ledger tamper, clearance, or burn recorded.");
            return false;
        }

        private static bool EvaluateChildrenSurvived(CampaignOutcomeEvaluationInput input, CampaignOutcomeSnapshot snapshot, List<string> trace)
        {
            if (snapshot.LivingDwellerCount <= 0)
            {
                trace.Add("[Children] ChildrenSurvived is FALSE: shelter suffered total dweller extinction.");
                return false;
            }

            if (input.ChildrenSurvivedOverride.HasValue)
            {
                trace.Add($"[Children] ChildrenSurvived set via explicit override: {input.ChildrenSurvivedOverride.Value}.");
                return input.ChildrenSurvivedOverride.Value;
            }

            if (input.Flags != null && (input.Flags.IsSet("flag_children_survived")
                || input.Flags.IsSet("flag_cohort_survived")
                || input.Flags.IsSet("flag_child_born")))
            {
                trace.Add("[Children] ChildrenSurvived is TRUE: child survival flag active in consequence ledger.");
                return true;
            }

            if (input.CohortChildren != null && input.CohortChildren.Count > 0)
            {
                snapshot.ChildrenCount = input.CohortChildren.Count;
                trace.Add($"[Children] ChildrenSurvived is TRUE: {input.CohortChildren.Count} cohort child/children active in shelter.");
                return true;
            }

            if (input.GenerationalState?.generationRecords != null)
            {
                int survivingDescendants = input.GenerationalState.generationRecords.Count(r => r != null && r.generationIndex > 0 && !r.isDeceased);
                if (survivingDescendants > 0)
                {
                    snapshot.ChildrenCount = survivingDescendants;
                    trace.Add($"[Children] ChildrenSurvived is TRUE: {survivingDescendants} surviving next-generation descendant(s) registered.");
                    return true;
                }
            }

            if (input.ChildrenCount > 0)
            {
                snapshot.ChildrenCount = input.ChildrenCount;
                trace.Add($"[Children] ChildrenSurvived is TRUE: {input.ChildrenCount} children recorded in input count.");
                return true;
            }

            trace.Add("[Children] ChildrenSurvived is FALSE: no cohort children or living descendants registered.");
            return false;
        }

        private static bool EvaluateVelSecretExposed(CampaignOutcomeEvaluationInput input, CampaignOutcomeSnapshot snapshot, List<string> trace)
        {
            if (input.VelSecretExposedOverride.HasValue)
            {
                trace.Add($"[Secret] VelSecretExposed set via explicit override: {input.VelSecretExposedOverride.Value}.");
                return input.VelSecretExposedOverride.Value;
            }

            if (input.Flags != null && (input.Flags.IsSet("flag_vel_secret_exposed") || input.Flags.IsSet("flag_evidence_exposed")))
            {
                trace.Add("[Secret] VelSecretExposed is TRUE: secret exposed flag active in consequence ledger.");
                return true;
            }

            if (input.VerdictReckoningState != null)
            {
                if (input.VerdictReckoningState.callResolved || input.VerdictReckoningState.countPresented || input.VerdictReckoningState.phase >= ReckoningPhase.Counted)
                {
                    trace.Add($"[Secret] VelSecretExposed is TRUE: Reckoning Call resolved or phase reached {input.VerdictReckoningState.phase}.");
                    return true;
                }
            }

            if (input.EnrolledEvidenceCount >= VerdictEndingEvaluator.MinimumEvidenceForRecount)
            {
                trace.Add($"[Secret] VelSecretExposed is TRUE: {input.EnrolledEvidenceCount} evidence documents enrolled (threshold: {VerdictEndingEvaluator.MinimumEvidenceForRecount}).");
                return true;
            }

            trace.Add("[Secret] VelSecretExposed is FALSE: Reckoning Call unresolved and insufficient evidence enrolled.");
            return false;
        }
    }
}
