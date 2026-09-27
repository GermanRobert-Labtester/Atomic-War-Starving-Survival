// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 227 — World Evolution Host Wiring.
// Core WorldEvolutionEngine is the authority for world-state evolution events
// and campaign progression tracking.
// ============================================================================

using System;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private WorldEvolutionHostSession? _worldEvolution;
        private bool _worldEvolutionDirty;

        public WorldEvolutionHostSession? WorldEvolution => _worldEvolution;

        public void SetupWorldEvolution()
        {
            if (_worldEvolution != null) return;

            _worldEvolution = new WorldEvolutionHostSession(dataDir: _dataDir);
            _worldEvolution.Setup();

            var saved = WorldEvolutionSaveStore.TryLoad();
            if (saved != null)
            {
                _worldEvolution.RestoreState(saved);
            }

            _worldEvolution.StateChanged += () => _worldEvolutionDirty = true;
        }

        public void SaveWorldEvolution()
        {
            if (_worldEvolution == null) return;
            var state = _worldEvolution.CaptureState();
            WorldEvolutionSaveStore.TrySave(state);
            if (CaptureSection("world_evolution", WorldEvolutionSaveStore.TryCapturePersisted(state)))
            {
                _worldEvolutionDirty = false;
            }
        }

        public void TickWorldEvolution(int day)
        {
            if (_worldEvolution == null) SetupWorldEvolution();
            if (_worldEvolution == null) return;

            _worldEvolution.Tick(day);
        }

        public void FlushWorldEvolutionIfDirty()
        {
            if (_worldEvolutionDirty && _worldEvolution != null)
            {
                SaveWorldEvolution();
            }
        }

        public void ResetWorldEvolution()
        {
            _worldEvolution?.Reset();
            _worldEvolutionDirty = false;
        }
    }
}
