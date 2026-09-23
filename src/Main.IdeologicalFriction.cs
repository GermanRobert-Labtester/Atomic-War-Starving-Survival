// SPDX-License-Identifier: MIT
// ASHFALL Plan 148 — Ideological Friction Events & Quests Host Wiring.

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private IdeologicalFrictionHostSession? _ideologicalFriction;
        private bool _ideologicalFrictionDirty;

        public IdeologicalFrictionHostSession? IdeologicalFriction => _ideologicalFriction;

        public void SetupIdeologicalFriction()
        {
            if (_ideologicalFriction != null) return;

            _ideologicalFriction = IdeologicalFrictionHostSession.Create(_dataDir);

            var saved = IdeologicalFrictionSaveStore.TryLoad();
            if (saved != null)
            {
                _ideologicalFriction.RestoreState(saved);
            }

            _ideologicalFriction.StateChanged += () => _ideologicalFrictionDirty = true;
        }

        public void SaveIdeologicalFriction()
        {
            if (_ideologicalFriction == null) return;
            var state = _ideologicalFriction.CaptureState();
            IdeologicalFrictionSaveStore.TrySave(state);
            if (CaptureSection("ideological_friction", IdeologicalFrictionSaveStore.TryCapturePersisted(state)))
            {
                _ideologicalFrictionDirty = false;
            }
        }

        public void TickIdeologicalFriction(int day)
        {
            if (_ideologicalFriction == null) SetupIdeologicalFriction();
            if (_ideologicalFriction == null) return;

            // Collect active survivor beliefs and update bunker factions
            if (_survivors != null)
            {
                var beliefs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var s in _survivors.RosterState)
                {
                    string b = _ideologicalFriction.GetBelief(s.Id);
                    if (!string.IsNullOrEmpty(b))
                    {
                        beliefs[s.Id] = b;
                    }
                }

                if (beliefs.Count >= 3)
                {
                    _ideologicalFriction.UpdateBunkerFactions(beliefs);
                }
            }
        }

        public void FlushIdeologicalFrictionIfDirty()
        {
            if (_ideologicalFrictionDirty)
            {
                SaveIdeologicalFriction();
            }
        }

        public void ResetIdeologicalFriction()
        {
            _ideologicalFriction = null;
            _ideologicalFrictionDirty = false;
        }
    }
}
