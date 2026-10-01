// SPDX-License-Identifier: MIT
// PLAN-SURGICAL-WARD-TRUTH-213 — surgical graft records host wiring.
// The ward bed authority stays with MedicalWardSystem; this partial owns the
// graft-record lifecycle and its own checksummed save section.

using System;
using Godot;
using Ashfall.Core.Medical;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SurgicalGraftHostSession? _surgicalGraft;
        private bool _surgicalGraftDirty;

        public SurgicalGraftHostSession? SurgicalGraft => _surgicalGraft;

        public void SetupSurgicalGraft()
        {
            if (_surgicalGraft != null) return;
            _surgicalGraft = new SurgicalGraftHostSession();
            var saved = SurgicalGraftSaveStore.TryLoad();
            if (saved != null)
            {
                _surgicalGraft.RestoreState(saved);
            }
            _surgicalGraft.StateChanged += () => _surgicalGraftDirty = true;
        }

        public void SaveSurgicalGraft()
        {
            if (_surgicalGraft == null) return;
            var state = _surgicalGraft.CaptureState();
            if (CaptureSection("surgical_graft", SurgicalGraftSaveStore.TryCapturePersisted(state)))
            {
                _surgicalGraftDirty = false;
            }
        }

        public void ResetSurgicalGraft()
        {
            _surgicalGraft = null;
            _surgicalGraftDirty = false;
        }

        /// <summary>Daily tick rides the existing expanded-shelter day orchestration.</summary>
        public void TickSurgicalGraft(int day)
        {
            if (_surgicalGraft == null) return;
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("surgical_graft", day) : new SeededRng(day);
            _surgicalGraft.TickDay(day, (graftId, riskPermille) =>
            {
                // Deterministic per-graft roll: engine compares the roll to the
                // risk permille internally; the host only supplies the draw.
                return rng.Next(0, 1000);
            });
        }
    }
}
