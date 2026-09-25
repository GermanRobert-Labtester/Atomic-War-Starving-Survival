// SPDX-License-Identifier: MIT
// ASHFALL Plan 199 — Seasonal Human Migration Engine Host Wiring.

using System;
using Godot;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private HumanMigrationHostSession? _humanMigration;
        private bool _humanMigrationDirty;

        public HumanMigrationHostSession? HumanMigration => _humanMigration;

        public void SetupHumanMigration()
        {
            if (_humanMigration != null) return;

            _humanMigration = HumanMigrationHostSession.Create(_dataDir);

            var saved = HumanMigrationSaveStore.TryLoad();
            if (saved != null)
            {
                _humanMigration.RestoreState(saved);
            }

            _humanMigration.StateChanged += () => _humanMigrationDirty = true;

            // CORE-MECH W6: seasonal migration becomes travel pressure. The
            // migration engine owns the region weights; the travel engine reads
            // them through one bounded provider (identity when unbound), so the
            // map gets alive without a second encounter selector.
            BindMigrationToTravelEncounters();
        }

        /// <summary>
        /// CORE-MECH W6 — map migration region weights onto encounter pressure in
        /// [0,1]. Population weight is normalized against the strongest region so
        /// the busiest corridor reads as full pressure and quiet regions as none.
        /// </summary>
        private void BindMigrationToTravelEncounters()
        {
            if (_humanMigration == null) return;
            var travel = _expeditions?.TravelEngine;
            if (travel == null) return;

            travel.RegionEncounterPressureProvider = regionId =>
            {
                if (string.IsNullOrEmpty(regionId)) return 0f;
                if (!_humanMigration.RegionWeights.TryGetValue(regionId, out int weight) || weight <= 0) return 0f;
                int peak = 1;
                foreach (int w in _humanMigration.RegionWeights.Values)
                    if (w > peak) peak = w;
                if (peak <= 0) return 0f;
                return Math.Clamp((float)weight / peak, 0f, 1f);
            };
        }

        public void SaveHumanMigration()
        {
            if (_humanMigration == null) return;
            var state = _humanMigration.CaptureState();
            HumanMigrationSaveStore.TrySave(state);
            if (CaptureSection("human_migration", HumanMigrationSaveStore.TryCapturePersisted(state)))
            {
                _humanMigrationDirty = false;
            }
        }

        public void TickHumanMigration(int day)
        {
            if (_humanMigration == null) SetupHumanMigration();
            // CORE-MECH W6: setup order is not guaranteed (expeditions may be built
            // after migration), so the pressure binding is refreshed on the tick.
            // The provider is replaced, never duplicated.
            BindMigrationToTravelEncounters();

            // Deterministic 90-day season phase cycle matching authored migration schedules
            string phase = (day % 360) switch
            {
                < 90 => "deep_winter",
                < 180 => "thaw",
                < 270 => "dry_heat",
                _ => "ash_winds"
            };

            _humanMigration?.TickDay(day, phase);
        }

        public void FlushHumanMigrationIfDirty()
        {
            if (_humanMigrationDirty)
            {
                SaveHumanMigration();
            }
        }

        public void ResetHumanMigration()
        {
            _humanMigration = null;
            _humanMigrationDirty = false;
        }
    }
}
