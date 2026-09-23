// SPDX-License-Identifier: MIT
// ASHFALL Plan 147 — Per-NPC Memory & Relationship Depth Host Wiring.

using System;
using Godot;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private NpcMemoryHostSession? _npcMemory;
        private bool _npcMemoryDirty;

        public NpcMemoryHostSession? NpcMemory => _npcMemory;

        public void SetupNpcMemory()
        {
            if (_npcMemory != null) return;

            _npcMemory = NpcMemoryHostSession.Create(_dataDir);

            var saved = NpcMemorySaveStore.TryLoad();
            if (saved != null)
            {
                _npcMemory.RestoreState(saved);
            }

            _npcMemory.StateChanged += () => _npcMemoryDirty = true;
        }

        public void SaveNpcMemory()
        {
            if (_npcMemory == null) return;
            var state = _npcMemory.CaptureState();
            NpcMemorySaveStore.TrySave(state);
            if (CaptureSection("npc_memory", NpcMemorySaveStore.TryCapturePersisted(state)))
            {
                _npcMemoryDirty = false;
            }
        }

        public void TickNpcMemory(int day)
        {
            if (_npcMemory == null) SetupNpcMemory();
            _npcMemory?.TickDailyDecay(day);
        }

        public void FlushNpcMemoryIfDirty()
        {
            if (_npcMemoryDirty)
            {
                SaveNpcMemory();
            }
        }

        public void ResetNpcMemory()
        {
            _npcMemory = null;
            _npcMemoryDirty = false;
        }
    }
}
