using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Verdict
{
    public enum AccusationAllowed
    {
        Allowed,
        MissingEvidence,
        PhaseNotReached,
        AlreadyResolved,
        UnknownCase
    }

    public sealed class AccusationResult
    {
        public AccusationAllowed Status;
        public string CaseId = string.Empty;
        public string SuspectId = string.Empty;
        /// <summary>Human-readable reason the accusation is blocked (empty when Allowed).</summary>
        public string Reason = string.Empty;
    }

    public sealed class TribunalVerdict
    {
        public string CaseId = string.Empty;
        public string SuspectId = string.Empty;
        public bool Guilty;
        public int EvidenceCount;
        /// <summary>Maps to ReckoningSystem.SelectEnding key.</summary>
        public string EndingKey = string.Empty;
        /// <summary>e.g. "faction_the_office:+15" or "faction_the_tempest:-10"</summary>
        public string FactionStandingEffect = string.Empty;
        public int MoralDelta;
        /// <summary>Diegetic journal entry — no real places, wars, or organisations.</summary>
        public string JournalEntry = string.Empty;
    }

    [Serializable]
    public class VerdictAccusationState
    {
        public List<string> resolvedCaseIds = new List<string>();
        /// <summary>caseId → ending key selected by tribunal resolution.</summary>
        public Dictionary<string, string> caseVerdicts = new Dictionary<string, string>(StringComparer.Ordinal);
    }

    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — typed accusation eligibility and
    /// tribunal resolution. Thin layer over ReckoningSystem + VerdictEvidenceChain;
    /// no direct evidence manipulation here — all evidence counting is delegated.
    ///
    /// Known cases (canon Verdict lore — fictional constructs only):
    ///   case_the_census_machine / suspect_the_bureau_clerk
    ///   case_the_long_silence    / suspect_the_broadcast_director
    ///   case_the_missing_count   / suspect_the_provincial_recorder
    /// </summary>
    public sealed class VerdictAccusationSystem
    {
        // ── Known case registry ──────────────────────────────────────────────

        private static readonly IReadOnlyDictionary<string, string> KnownCases =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "case_the_census_machine",  "suspect_the_bureau_clerk"         },
                { "case_the_long_silence",    "suspect_the_broadcast_director"   },
                { "case_the_missing_count",   "suspect_the_provincial_recorder"  }
            };

        // ── Journal entries per case (diegetic; no real places or wars) ──────

        private static readonly IReadOnlyDictionary<string, string> GuiltyJournalEntries =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "case_the_census_machine",  "The census machine's ledger is held open. The Bureau Clerk's tallies do not balance. The sector counts the gap — and assigns it a name." },
                { "case_the_long_silence",    "The broadcast record shows a deliberate pause. The Broadcast Director held the frequency dark when the sector needed it most. The tribunal returns a verdict: responsible." },
                { "case_the_missing_count",   "The provincial registry carries a blank where survivors were recorded. The Provincial Recorder signed it blank. The count was not lost — it was withheld." }
            };

        private static readonly IReadOnlyDictionary<string, string> NotGuiltyJournalEntries =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "case_the_census_machine",  "The census machine's ledger is set aside. The evidence does not reach the threshold. The Bureau Clerk's record is sealed but not condemned." },
                { "case_the_long_silence",    "The broadcast silence is recorded but not charged. The Broadcast Director's account remains open; the frequency outage is attributed to technical failure." },
                { "case_the_missing_count",   "The blank in the provincial registry is noted but not prosecuted. The Provincial Recorder's disposition is deferred. Insufficient evidence." }
            };

        // ── Faction effects and moral deltas ─────────────────────────────────

        private static readonly IReadOnlyDictionary<string, (string faction, int moralDelta)> GuiltyConsequences =
            new Dictionary<string, (string, int)>(StringComparer.Ordinal)
            {
                { "case_the_census_machine",  ("faction_the_office:+15",   +8) },
                { "case_the_long_silence",    ("faction_the_tempest:-10",  +6) },
                { "case_the_missing_count",   ("faction_the_office:+10",   +5) }
            };

        private static readonly IReadOnlyDictionary<string, (string faction, int moralDelta)> NotGuiltyConsequences =
            new Dictionary<string, (string, int)>(StringComparer.Ordinal)
            {
                { "case_the_census_machine",  ("faction_the_office:-5",   -3) },
                { "case_the_long_silence",    ("faction_the_tempest:+8",  -4) },
                { "case_the_missing_count",   ("faction_the_office:-3",   -2) }
            };

        // ── State ────────────────────────────────────────────────────────────

        private readonly VerdictAccusationState _state;
        private ReckoningSystem? _reckoning;
        private VerdictEvidenceChain? _evidenceChain;

        public IReadOnlyList<string> ResolvedCaseIds => _state.resolvedCaseIds;

        public VerdictAccusationSystem(VerdictAccusationState? state = null)
        {
            _state = state ?? new VerdictAccusationState();
        }

        /// <summary>Binds to live reckoning + evidence chain. Call before any eligibility query.</summary>
        public void Bind(ReckoningSystem reckoning, VerdictEvidenceChain? evidenceChain = null)
        {
            _reckoning = reckoning ?? throw new ArgumentNullException(nameof(reckoning));
            _evidenceChain = evidenceChain;
        }

        // ── Eligibility ──────────────────────────────────────────────────────

        /// <summary>
        /// Pure eligibility check — no side effects.
        /// Does not require evidenceChain to be bound; reads enrolledEvidence from ReckoningState.
        /// </summary>
        public AccusationResult CanAccuse(string caseId, string suspectId, int currentDay)
        {
            var result = new AccusationResult { CaseId = caseId, SuspectId = suspectId };

            if (!KnownCases.ContainsKey(caseId))
            {
                result.Status = AccusationAllowed.UnknownCase;
                result.Reason = $"Case '{caseId}' is not in the Verdict canon register.";
                return result;
            }

            if (IsResolved(caseId))
            {
                result.Status = AccusationAllowed.AlreadyResolved;
                result.Reason = $"Case '{caseId}' has already been brought before the tribunal.";
                return result;
            }

            if (_reckoning == null || _reckoning.Phase < ReckoningPhase.Culpable)
            {
                result.Status = AccusationAllowed.PhaseNotReached;
                result.Reason = "The Reckoning has not reached the Culpable phase. The tribunal will not convene yet.";
                return result;
            }

            int enrolled = _reckoning?.State.enrolledEvidence ?? 0;
            if (enrolled < 1)
            {
                result.Status = AccusationAllowed.MissingEvidence;
                result.Reason = "No evidence has been enrolled. At least one machine-log entry must be read before an accusation can be filed.";
                return result;
            }

            result.Status = AccusationAllowed.Allowed;
            return result;
        }

        // ── Tribunal resolution ──────────────────────────────────────────────

        /// <summary>
        /// Resolves the tribunal for a case. Returns null if not eligible.
        /// Determines guilt by evidenceCount >= 2; applies moral delta via moralSystem if bound.
        /// Calls reckoning.SelectEnding with the appropriate ending key.
        /// </summary>
        public TribunalVerdict? ResolveTribunal(
            string caseId,
            string suspectId,
            int currentDay,
            MoralChoiceSystem? moralSystem = null)
        {
            var eligibility = CanAccuse(caseId, suspectId, currentDay);
            if (eligibility.Status != AccusationAllowed.Allowed) return null;

            int evidenceCount = _reckoning?.State.enrolledEvidence ?? 0;
            bool guilty = evidenceCount >= 2;

            string endingKey = guilty
                ? "ending_verdict_the_sector_recounts"
                : "ending_verdict_the_count_is_held";

            // Inform the ReckoningSystem of the ending choice
            _reckoning?.SelectEnding(endingKey, currentDay);

            // Consequence lookup
            var consequences = guilty
                ? GuiltyConsequences.TryGetValue(caseId, out var gc) ? gc : (string.Empty, 0)
                : NotGuiltyConsequences.TryGetValue(caseId, out var nc) ? nc : (string.Empty, 0);

            string journalEntry = guilty
                ? (GuiltyJournalEntries.TryGetValue(caseId, out var gj) ? gj : string.Empty)
                : (NotGuiltyJournalEntries.TryGetValue(caseId, out var nj) ? nj : string.Empty);

            var verdict = new TribunalVerdict
            {
                CaseId = caseId,
                SuspectId = suspectId,
                Guilty = guilty,
                EvidenceCount = evidenceCount,
                EndingKey = endingKey,
                FactionStandingEffect = consequences.Item1,
                MoralDelta = consequences.Item2,
                JournalEntry = journalEntry
            };

            // Apply moral delta — use SetFlag as a lightweight moral event if moralSystem is wired
            if (moralSystem != null && consequences.Item2 != 0)
            {
                // Apply via SetFlag on a tribunal-specific moral marker (non-quest path)
                string moralFlag = guilty
                    ? $"flag_tribunal_guilty_{caseId}"
                    : $"flag_tribunal_acquitted_{caseId}";
                moralSystem.SetFlag(moralFlag);
            }

            // Record resolution
            _state.resolvedCaseIds.Add(caseId);
            _state.caseVerdicts[caseId] = endingKey;

            return verdict;
        }

        public bool IsResolved(string caseId) =>
            _state.resolvedCaseIds.Contains(caseId);

        // ── Save / Load ──────────────────────────────────────────────────────

        public VerdictAccusationState CaptureState()
        {
            var copy = new VerdictAccusationState();
            copy.resolvedCaseIds.AddRange(_state.resolvedCaseIds);
            foreach (var kv in _state.caseVerdicts)
                copy.caseVerdicts[kv.Key] = kv.Value;
            return copy;
        }

        public void RestoreState(VerdictAccusationState? state)
        {
            if (state == null) return;
            _state.resolvedCaseIds.Clear();
            if (state.resolvedCaseIds != null)
                _state.resolvedCaseIds.AddRange(state.resolvedCaseIds);
            _state.caseVerdicts.Clear();
            if (state.caseVerdicts != null)
                foreach (var kv in state.caseVerdicts)
                    _state.caseVerdicts[kv.Key] = kv.Value;
        }
    }
}
