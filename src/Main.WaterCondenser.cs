// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// B5–B8 expansion (§9.9): shelter atmospheric condenser — standard triad
    /// (Setup / Save / Tick) plus build/membrane routes. Same §15.3 discipline
    /// as the deep well: live capability query → canonical bill consumed
    /// atomically → Core commit.
    /// </summary>
    public partial class Main
    {
        private WaterCondenserHostSession? _waterCondenser;

        private void SetupWaterCondenser()
        {
            if (_waterCondenser != null) return;
            SetupCampaignDay();
            SetupPowerGrid();
            SetupWaterTreatment();
            SetupWorld();

            var saved = WaterCondenserSaveStore.TryLoad() ?? new AtmosphericCondenserState();
            var system = new AtmosphericCondenserSystem(
                _powerGrid.System,
                _waterTreatment.System,
                _world.Weather,
                new GodotLog());
            system.RestoreState(saved);
            _waterCondenser = new WaterCondenserHostSession(system);
            // B5–B8 expansion (§27): spent-membrane failure edge → journal.
            system.OnMembraneSpent += s =>
            {
                _journal?.TryAddRawEntry("condenser_membrane_spent",
                    "The condensation membrane is spent — the array condenses nothing until it is replaced with a desalination membrane.",
                    null!, _simDay);
            };
        }

        private void SaveWaterCondenser()
        {
            if (_waterCondenser != null)
                CaptureSection("water_condenser", WaterCondenserSaveStore.TryCapturePersisted(_waterCondenser.System.CaptureState()));
        }

        private void TickWaterCondenser(int day)
        {
            _waterCondenser?.TickDay(day);
            if (_waterCondenser != null && _waterCondenser.IsDirty)
                SaveWaterCondenser();
        }

        /// <summary>Build route: live capability → canonical bill (membrane +
        /// pipes + scrap) → Core commit. Blocked reasons are truthful; nothing
        /// is consumed on a blocked build.</summary>
        public bool WaterCondenserTryBuild()
        {
            bool built = EnsureWaterSourcesSession().TryBuildCondenser();
            ObserveSigil(built ? "condenser.built" : "condenser.build_blocked_water_sources");
            return built;
        }

        /// <summary>Membrane replacement route: canonical membrane consumed
        /// once on commit.</summary>
        public bool WaterCondenserTryReplaceMembrane()
        {
            bool replaced = EnsureWaterSourcesSession().TryReplaceCondenserMembrane();
            ObserveSigil(replaced ? "condenser.membrane_replaced" : "condenser.membrane_blocked");
            return replaced;
        }

        public bool WaterCondenserSetEnabled(bool enabled) =>
            EnsureWaterSourcesSession().TrySetCondenserEnabled(enabled);
    }
}
