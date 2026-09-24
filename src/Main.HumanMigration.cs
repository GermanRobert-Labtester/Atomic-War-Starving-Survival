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
