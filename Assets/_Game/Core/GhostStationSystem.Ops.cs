using System;
using System.Collections.Generic;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GhostStationSystem
    {
        /// <summary>
        /// Apply ghost-station hear effects for an intel node (or force by id in tests).
        /// Returns true if effects applied (first hear of this station).
        /// </summary>
        public bool ApplyGhostHear(IntelNode intel)
        {
            if (intel == null || intel.Type != IntelType.GhostLoop) return false;
            return ApplyGhostHear(intel.SourceFrequencyId, intel);
        }

        /// <summary>
        /// Create runtime RadioFrequencySO assets for all catalog ghosts and
        /// inject them into the bound tuner. Safe to call repeatedly.
        /// </summary>
        public void EnsureFrequenciesInjected()
        {
            if (!_unlocked || _tuner == null) return;

            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (def == null || string.IsNullOrEmpty(def.Id)) continue;
                if (_tuner.GetFrequency(def.Id) != null) continue;

                var freq = BuildFrequency(def);
                if (freq == null) continue;
                _runtimeFreqs.Add(freq);
                _tuner.AddFrequency(freq);
            }

            _frequenciesInjected = true;
        }

        public GhostStationSave CaptureState()
        {
            var heard = new string[_heard.Count];
            int i = 0;
            foreach (var id in _heard)
                heard[i++] = id;
            return new GhostStationSave
            {
                Unlocked = _unlocked,
                HeardStationIds = heard
            };
        }

        public void RestoreState(GhostStationSave save)
        {
            _heard.Clear();
            _unlocked = false;
            _frequenciesInjected = false;
            if (save == null) return;
            _unlocked = save.Unlocked;
            if (save.HeardStationIds != null)
            {
                for (int i = 0; i < save.HeardStationIds.Length; i++)
                {
                    string id = save.HeardStationIds[i];
                    if (!string.IsNullOrEmpty(id))
                        _heard.Add(id);
                }
            }
            if (_unlocked)
                EnsureFrequenciesInjected();
        }

        // -----------------------------------------------------------------

        private void HandleIntelExtracted(IntelNode intel)
        {
            if (intel == null || intel.Type != IntelType.GhostLoop) return;
            ApplyGhostHear(intel);
        }

        /// <summary>
        /// Call when the nuclear-exchange EMP fires (Flashpoint or fallback).
        /// Idempotent — unlocks ghost bands once.
        /// </summary>
        public bool NotifyEmpOccurred()
        {
            if (_unlocked) return false;
            _unlocked = true;
            EnsureFrequenciesInjected();
            OnUnlocked?.Invoke();
            OnStateChanged?.Invoke();
            return true;
        }

    }
}
