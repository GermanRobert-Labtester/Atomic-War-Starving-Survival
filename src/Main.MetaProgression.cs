// SPDX-License-Identifier: MIT
// ASHFALL Plan 175 — Meta Progression & Cross-Run Profile Store Host Wiring.

using System;
using Godot;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private MetaProgressionHostSession? _metaProgression;
        private bool _metaProgressionDirty;

        public MetaProgressionHostSession? MetaProgression => _metaProgression;

        public void SetupMetaProgression()
        {
            if (_metaProgression != null) return;

            _metaProgression = MetaProgressionHostSession.Create(_dataDir);

            var saved = MetaProgressionSaveStore.TryLoad();
            if (saved != null)
            {
                _metaProgression.RestoreState(saved);
            }

            _metaProgression.StateChanged += () => _metaProgressionDirty = true;
        }

        public void SaveMetaProgression()
        {
            if (_metaProgression == null) return;
            var state = _metaProgression.CaptureState();
            MetaProgressionSaveStore.TrySave(state);
            if (CaptureSection("meta_progression", MetaProgressionSaveStore.TryCapturePersisted(state)))
            {
                _metaProgressionDirty = false;
            }
        }

        public void TickMetaProgression(int day)
        {
            if (_metaProgression == null) SetupMetaProgression();
            // Heartbeat daily progression
        }

        public void FlushMetaProgressionIfDirty()
        {
            if (_metaProgressionDirty)
            {
                SaveMetaProgression();
            }
        }

        public void ResetMetaProgression()
        {
            _metaProgression = null;
            _metaProgressionDirty = false;
        }
    }
}
