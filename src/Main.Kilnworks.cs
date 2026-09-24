// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 31 — The Kiln: ceramics firing host wiring.
// The signed pure KilnFiringEngine is the authority for firing stages, draw
// grade, lime calcination, and lining wear. This host owns the kiln's own fuel
// reserve and batch queue only: metallurgy stays with CupolaFoundryEngine and
// shelter upgrades stay with the canonical infrastructure owners. Firing is
// deterministic — the daily owner advances one stage at the fixed optimal
// temperature, and no RNG stream is consumed.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private KilnworksHostSession? _kilnworks;
        private bool _kilnworksDirty;

        public KilnworksHostSession? Kilnworks => _kilnworks;

        public void SetupKilnworks()
        {
            if (_kilnworks != null) return;

            var saved = KilnworksSaveStore.TryLoad();
            _kilnworks = KilnworksHostSession.Create(saved);
            _kilnworks.StateChanged += () => _kilnworksDirty = true;
        }

        /// <summary>Queues a kiln batch (clay pottery, brick, refractory tile, ceramic tile).</summary>
        public bool QueueKilnBatch(string batchId, KilnLoadKind loadKind, int rawMaterialQualityPermille = 700)
        {
            SetupKilnworks();
            return _kilnworks?.AddBatch(batchId, loadKind, rawMaterialQualityPermille) ?? false;
        }

        /// <summary>Advances one firing stage of a specific batch.</summary>
        public KilnBatchState? AdvanceKilnBatch(string batchId, int kilnTemperaturePermille = KilnFiringLedger.OptimalFiringTemperaturePermille)
        {
            SetupKilnworks();
            return _kilnworks?.AdvanceFiring(batchId, kilnTemperaturePermille);
        }

        /// <summary>
        /// Advances the oldest active batch one stage. This is the deterministic
        /// daily kiln path used by the phase-5 day owner; it consumes no RNG.
        /// </summary>
        public KilnBatchState? TickKilnworksFiring()
        {
            SetupKilnworks();
            return _kilnworks?.AdvanceOldestBatch(KilnFiringLedger.OptimalFiringTemperaturePermille);
        }

        /// <summary>Runs a lime calcination pass from the kiln's fuel reserve.</summary>
        public LimeCalcinationResult CalcinateLime(
            int limestoneKg,
            int kilnTemperaturePermille = KilnFiringLedger.OptimalFiringTemperaturePermille,
            int soakHours = 24)
        {
            SetupKilnworks();
            return _kilnworks?.CalcinateLimestone(limestoneKg, kilnTemperaturePermille, soakHours)
                ?? new LimeCalcinationResult(0, 1000, false, 0);
        }

        public void RefuelKiln(int fuelPermille)
        {
            SetupKilnworks();
            _kilnworks?.Refuel(fuelPermille);
        }

        public void RelineKiln(int relinePermille = 1000)
        {
            SetupKilnworks();
            _kilnworks?.Reline(relinePermille);
        }

        public KilnFiringCensus GetKilnworksCensus() => _kilnworks?.Census ?? default;

        public void SaveKilnworks()
        {
            if (_kilnworks == null) return;
            var state = _kilnworks.CaptureState();
            KilnworksSaveStore.TrySave(state);
            if (CaptureSection(KilnworksSaveStore.SectionName, KilnworksSaveStore.TryCapturePersisted(state)))
                _kilnworksDirty = false;
        }

        public void FlushKilnworksIfDirty()
        {
            if (_kilnworksDirty)
                SaveKilnworks();
        }

        public void ResetKilnworks()
        {
            _kilnworks = null;
            _kilnworksDirty = false;
        }
    }
}
