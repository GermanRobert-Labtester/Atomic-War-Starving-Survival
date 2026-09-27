// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : VerdictAccusationHostSession
// Purpose      : PLAN-INVESTIGATION-EVIDENCE-TRUTH-121 — host the typed
//                accusation/tribunal layer over the existing Reckoning and
//                EvidenceChain owners. Persists under `verdict_accusation`.
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Verdict;

namespace AtomicWar.GodotApp
{
    public static class VerdictAccusationSaveStore
    {
        public const string FileName = "verdict_accusation_save.json";
        public const string SectionName = "verdict_accusation";
        private static readonly SaveStore<VerdictAccusationState> s_store =
            SaveStoreHub.Checksummed<VerdictAccusationState>(FileName, nameof(VerdictAccusationSaveStore));
        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static string TryCapturePersisted(VerdictAccusationState state) => s_store.CaptureBare(state);
        public static VerdictAccusationState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(VerdictAccusationState state) => s_store.TrySave(state);
        public static VerdictAccusationState? TryLoad() => s_store.TryLoad();
    }

    public sealed class VerdictAccusationHostSession : HostSessionBase
    {
        public VerdictAccusationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public VerdictAccusationHostSession(ReckoningSystem reckoning, VerdictEvidenceChain? evidenceChain = null, VerdictAccusationState? state = null)
        {
            System = new VerdictAccusationSystem(state ?? new VerdictAccusationState());
            System.Bind(reckoning, evidenceChain);
        }

        public AccusationResult CanAccuse(string caseId, string suspectId, int currentDay)
        {
            var result = System.CanAccuse(caseId, suspectId, currentDay);
            LastEvent = result.Status == AccusationAllowed.Allowed
                ? $"Accusation allowed: {caseId}/{suspectId}."
                : $"Accusation blocked: {caseId}/{suspectId} ({result.Reason}).";
            return result;
        }

        public TribunalVerdict? ResolveTribunal(string caseId, string suspectId, int currentDay)
        {
            var verdict = System.ResolveTribunal(caseId, suspectId, currentDay);
            if (verdict != null)
            {
                LastEvent = $"Tribunal resolved: {caseId} → {(verdict.Guilty ? "guilty" : "held")}.";
                RaiseStateChanged();
            }
            return verdict;
        }

        public bool IsResolved(string caseId) => System.IsResolved(caseId);
        public VerdictAccusationState CaptureState() => System.CaptureState();
        public void RestoreState(VerdictAccusationState? state) => System.RestoreState(state);
    }
}
