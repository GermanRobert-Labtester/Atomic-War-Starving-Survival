// SPDX-License-Identifier: MIT
// ASHFALL Plan 174 — Procedural Survivor Backstories Host Wiring.

using System;
using System.Collections.Generic;
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
            AssignMissingBackstories();
        }

        /// <summary>
        /// Plan 174 — assigns a deterministic procedural origin to every roster
        /// survivor that does not already have one. The template index is derived
        /// only from the survivor id via <see cref="Ashfall.Core.StableHash"/>,
        /// so a replayed day re-derives the identical origin and no campaign RNG
        /// stream is consumed (the consumption order of every other stream is
        /// therefore unchanged). Reassignment is skipped for survivors that
        /// already have a restored origin, so a loaded save is never rewritten.
        /// </summary>
        public void AssignMissingBackstories()
        {
            if (_survivors == null) return;
            if (_backstory == null) SetupBackstory();
            if (_backstory == null) return;

            var templates = _backstory.System.GetAllTemplates();
            if (templates == null || templates.Count == 0) return;

            // Stable ordinal ordering: the derived index must be identical on
            // every host and in every replay, independent of dictionary order.
            var ordered = new List<BackstoryTemplateDef>(templates);
            ordered.Sort((a, b) => string.CompareOrdinal(a.template_id, b.template_id));

            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var survivor = _survivors.RosterState[i];
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;
                if (_backstory.GetBackstory(survivor.Id) != null) continue;

                int index = (int)((uint)Ashfall.Core.StableHash.Of(survivor.Id) % (uint)ordered.Count);
                _backstory.AssignFromTemplate(survivor.Id, ordered[index].template_id, _simDay);
            }
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
