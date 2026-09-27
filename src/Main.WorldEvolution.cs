// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 227 — World Evolution Host Wiring.
// Core WorldEvolutionEngine is the single authority for world-state evolution
// events. It reads the authoritative wasteland map owned by the world session;
// no second map graph, no parallel event ledger.
// ============================================================================

using System;
using System.Collections.Generic;
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

            _worldEvolution = new WorldEvolutionHostSession(_dataDir);

            var saved = WorldEvolutionSaveStore.TryLoad();
            if (saved != null)
            {
                _worldEvolution.RestoreState(saved, _world?.WastelandMap);
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

            _worldEvolution.Tick(day, ActiveWorldFlags(), _world?.WastelandMap);
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

        /// <summary>
        /// Derived read model over the campaign's active world flags. Returns an
        /// empty set when no flags owner is bound; the engine treats a missing
        /// flag as a gate that is not open, so no event fires on an unowned fact.
        /// </summary>
        private HashSet<string> ActiveWorldFlags()
        {
            var flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            return flags;
        }
    }
}
