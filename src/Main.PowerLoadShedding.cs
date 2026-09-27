// SPDX-License-Identifier: MIT
// ASHFALL Expansion 21 (The Grid) — microgrid load-shedding host wiring.
//
// Binds the sealed PowerLoadSheddingEngine to the live canonical power owners
// (PowerGridSystem + PowerDistributionSubgridSystem) and projects its verdict.
// DERIVED READ MODEL: no save section, no second power ledger, no breaker
// mutation. The energy-poverty morale penalty is routed into the canonical
// morale owner exactly once per canonical day.

using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PowerLoadSheddingHostSession? _powerLoadShedding;
        private int _powerLoadSheddingMoraleDay;

        public PowerLoadSheddingHostSession? PowerLoadShedding => _powerLoadShedding;

        /// <summary>
        /// Binds the engine to the live owners. Safe to call repeatedly: the
        /// session is created once and re-bound to whatever the canonical owners
        /// currently are (the power grid may be constructed after this call).
        /// </summary>
        public void SetupPowerLoadShedding()
        {
            if (_powerLoadShedding == null)
                _powerLoadShedding = new PowerLoadSheddingHostSession();
            BindPowerLoadShedding();
        }

        private void BindPowerLoadShedding()
        {
            if (_powerLoadShedding == null) return;
            var grid = _powerGrid?.System;
            var subgrids = _powerSubgrids;
            if (grid == null && subgrids == null) return;
            _powerLoadShedding.Bind(grid, subgrids);
        }

        /// <summary>
        /// One canonical day: evaluate shedding over the live owners and apply
        /// the energy-poverty morale penalty through the canonical morale owner,
        /// exactly once per day.
        /// </summary>
        public void TickPowerLoadShedding(int day)
        {
            SetupPowerLoadShedding();
            if (_powerLoadShedding == null) return;

            var projection = _powerLoadShedding.GetProjection();

            if (day != _powerLoadSheddingMoraleDay)
            {
                _powerLoadSheddingMoraleDay = day;
                ApplyEnergyPovertyMorale(_powerLoadShedding.GetMoralePenaltyPoints(
                    projection.EnergyPovertyMoralePenaltyPermille));
            }
        }

        private void ApplyEnergyPovertyMorale(float delta)
        {
            if (delta == 0f || _survivors == null) return;
            foreach (var survivor in _survivors.Needs.Registered)
            {
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;
                _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
            }
        }

        public PowerLoadSheddingProjection? GetPowerLoadSheddingProjection()
        {
            SetupPowerLoadShedding();
            return _powerLoadShedding?.GetProjection();
        }

        public void ResetPowerLoadShedding()
        {
            _powerLoadShedding = null;
            _powerLoadSheddingMoraleDay = 0;
        }
    }
}
