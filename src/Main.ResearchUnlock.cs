// SPDX-License-Identifier: MIT
// ASHFALL Plan 141 — Research Downstream Unlocks Bridge Host Wiring.

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Research;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ResearchUnlockHostSession? _researchUnlock;
        private bool _researchUnlockDirty;

        public ResearchUnlockHostSession? ResearchUnlock => _researchUnlock;

        public void SetupResearchUnlockBridge()
        {
            if (_researchUnlock != null) return;

            var sharedResearch = EnsureSharedResearch();
            _researchUnlock = ResearchUnlockHostSession.Create(_dataDir, _inventory?.Inventory);

            var saved = ResearchUnlockSaveStore.TryLoad();
            if (saved != null)
            {
                _researchUnlock.RestoreState(saved);
            }

            _researchUnlock.BindResearchSystem(sharedResearch);

            // Retroactive synchronization: ensure completed nodes from save grant downstream unlocks
            if (sharedResearch.State?.completedIds != null && sharedResearch.State.completedIds.Count > 0)
            {
                _researchUnlock.SynchronizeCompletedResearch(sharedResearch.State.completedIds);
            }

            _researchUnlock.StateChanged += () => _researchUnlockDirty = true;
        }

        public void SaveResearchUnlock()
        {
            if (_researchUnlock == null) return;
            var state = _researchUnlock.CaptureState();
            ResearchUnlockSaveStore.TrySave(state);
            if (CaptureSection("research_unlock", ResearchUnlockSaveStore.TryCapturePersisted(state)))
            {
                _researchUnlockDirty = false;
            }
        }

        public void TickResearchUnlock(int day)
        {
            if (_researchUnlock == null) SetupResearchUnlockBridge();

            if (_sharedResearch?.State?.completedIds != null && _researchUnlock != null)
            {
                _researchUnlock.SynchronizeCompletedResearch(_sharedResearch.State.completedIds);
            }
        }

        public void FlushResearchUnlockIfDirty()
        {
            if (_researchUnlockDirty)
            {
                SaveResearchUnlock();
            }
        }

        public void ResetResearchUnlock()
        {
            _researchUnlock = null;
            _researchUnlockDirty = false;
        }
    }
}
