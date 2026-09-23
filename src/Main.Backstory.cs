// SPDX-License-Identifier: MIT
// ASHFALL Plan 174 — Procedural Survivor Backstories Host Wiring.

using System;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BackstoryHostSession? _backstory;
        private bool _backstoryDirty;

        public BackstoryHostSession? Backstory => _backstory;

        public void SetupBackstory()
        {
            if (_backstory != null) return;

            _backstory = BackstoryHostSession.Create(_dataDir);

            var saved = BackstorySaveStore.TryLoad();
            if (saved != null)
            {
                _backstory.RestoreState(saved);
            }

            _backstory.StateChanged += () => _backstoryDirty = true;

            if (_survivorDetailPanel != null)
            {
                _survivorDetailPanel.BackstoryProvider = id => _backstory?.GetBackstory(id);
            }
        }

        public void SaveBackstory()
        {
            if (_backstory == null) return;
            var state = _backstory.CaptureState();
            BackstorySaveStore.TrySave(state);
            if (CaptureSection("backstory", BackstorySaveStore.TryCapturePersisted(state)))
            {
                _backstoryDirty = false;
            }
        }

        public void TickBackstory(int day)
        {
            if (_backstory == null) SetupBackstory();
            // Heartbeat daily progression
        }

        public void FlushBackstoryIfDirty()
        {
            if (_backstoryDirty)
            {
                SaveBackstory();
            }
        }

        public void ResetBackstory()
        {
            _backstory = null;
            _backstoryDirty = false;
        }
    }
}
