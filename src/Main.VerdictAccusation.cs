// SPDX-License-Identifier: MIT
// PLAN-INVESTIGATION-EVIDENCE-TRUTH-121 — accusation/tribunal host wiring over
// the existing Verdict Reckoning + EvidenceChain owners.

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private VerdictAccusationHostSession? _verdictAccusation;
        private bool _verdictAccusationDirty;

        public VerdictAccusationHostSession? VerdictAccusation => _verdictAccusation;

        public void SetupVerdictAccusation()
        {
            if (_verdictAccusation != null) return;
            if (_verdict == null) return; // SetupVerdict must run first
            var saved = VerdictAccusationSaveStore.TryLoad();
            _verdictAccusation = new VerdictAccusationHostSession(_verdict.Reckoning, _verdict.EvidenceChain, saved);
            _verdictAccusation.StateChanged += () => _verdictAccusationDirty = true;
        }

        public void SaveVerdictAccusation()
        {
            if (_verdictAccusation == null) return;
            var state = _verdictAccusation.CaptureState();
            VerdictAccusationSaveStore.TrySave(state);
            if (CaptureSection("verdict_accusation", VerdictAccusationSaveStore.TryCapturePersisted(state)))
            {
                _verdictAccusationDirty = false;
            }
        }

        public void FlushVerdictAccusationIfDirty()
        {
            if (_verdictAccusationDirty) SaveVerdictAccusation();
        }

        public void ResetVerdictAccusation()
        {
            _verdictAccusation = null;
            _verdictAccusationDirty = false;
        }
    }
}
