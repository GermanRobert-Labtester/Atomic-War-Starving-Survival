// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 185 — Memory & Knowledge Decay host wiring.
// The pure domain MemoryDecaySystem is the authority for memory clarity,
// gradual cognitive degradation, reinforcement, certification, and archive preservation.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Cognition;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private MemoryDecayHostSession? _memoryDecay;
        private bool _memoryDecayDirty;

        public MemoryDecayHostSession? MemoryDecay => _memoryDecay;

        public void SetupMemoryDecay()
        {
            if (_memoryDecay != null) return;

            var saved = MemoryDecaySaveStore.TryLoad();
            _memoryDecay = MemoryDecayHostSession.Create(saved);
            _memoryDecay.StateChanged += () => _memoryDecayDirty = true;

            // Load authored decay rates catalog
            string catalogPath = CatalogPath.ResolveCatalog("memory_decay_rates.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
            {
                _memoryDecay.LoadCatalog(catalogIo.ReadAllText(catalogPath));
            }
        }

        public MemoryRecord RegisterSurvivorMemory(
            string survivorId,
            MemoryDomain domain,
            string referenceId,
            int day,
            float initialStrength = 100f,
            bool isCertified = false,
            bool isPreserved = false)
        {
            SetupMemoryDecay();
            return _memoryDecay!.RegisterOrUpdate(survivorId, domain, referenceId, day, initialStrength, isCertified, isPreserved);
        }

        public bool ReinforceSurvivorMemory(
            string survivorId,
            MemoryDomain domain,
            string referenceId,
            int day,
            ReinforcementType type = ReinforcementType.Review,
            float boostAmount = 25f)
        {
            SetupMemoryDecay();
            return _memoryDecay!.Reinforce(survivorId, domain, referenceId, day, type, boostAmount);
        }

        public MemoryDecayCensus GetMemoryDecayCensus() =>
            _memoryDecay?.Census ?? default;

        public void TickMemoryDecay(int day)
        {
            SetupMemoryDecay();
            _memoryDecay!.TickDay(day);
        }

        public void SaveMemoryDecay()
        {
            if (_memoryDecay == null) return;
            var state = _memoryDecay.System.CaptureState();
            MemoryDecaySaveStore.TrySave(state);
            if (CaptureSection(
                    MemoryDecaySaveStore.SectionName,
                    MemoryDecaySaveStore.TryCapturePersisted(state)))
            {
                _memoryDecayDirty = false;
            }
        }

        public void FlushMemoryDecayIfDirty()
        {
            if (_memoryDecayDirty)
            {
                SaveMemoryDecay();
            }
        }

        public void ResetMemoryDecay()
        {
            _memoryDecay = null;
            _memoryDecayDirty = false;
        }
    }
}
