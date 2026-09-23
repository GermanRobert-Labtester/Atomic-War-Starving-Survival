#nullable enable
// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 134 — Dynamic Faction Territory & Supply Lines in the running game.
//
// Authority boundary: TerritoryControlSystem is the single authority for
// territorial control, contested nodes, fortification levels, and supply corridors.
// FactionWarSystem (Plan 30) and FactionEcology (Plan 25) observe or trigger
// territorial shifts, but all territory facts belong strictly to TerritoryControlSystem.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private TerritoryControlHostSession? _territoryControl;
        private bool _territoryControlDirty;

        public TerritoryControlHostSession? TerritoryControl => _territoryControl;

        /// <summary>
        /// Loads authored territory and supply line catalogs and restores any persisted state.
        /// A missing catalog logs an error and leaves the feature dormant.
        /// </summary>
        private TerritoryControlHostSession? EnsureTerritoryControl()
        {
            if (_territoryControl != null) return _territoryControl;
            try
            {
                var session = TerritoryControlHostSession.Load(_dataDir, new FileSystemIO());
                session.RestoreState(TerritoryControlSaveStore.TryLoad());

                session.System.OnTerritoryControlChangedSeam += (loc, oldF, newF) =>
                {
                    _journal?.TryAddRawEntry(
                        "territory_control_changed",
                        $"Control of {loc} shifted from {oldF} to {newF}.",
                        null!, Math.Max(1, _simDay));
                    _consequenceLedger?.Increment(
                        $"territory_shift::{loc}::{newF}", 1, "territory_control", "control_changed", Math.Max(1, _simDay));
                    _territoryControlDirty = true;
                };

                session.System.OnTerritoryContestedSeam += (loc, oldF, newF) =>
                {
                    _journal?.TryAddRawEntry(
                        "territory_contested",
                        $"{loc} held by {oldF} was contested by {newF}.",
                        null!, Math.Max(1, _simDay));
                    _territoryControlDirty = true;
                };

                session.System.OnSupplyLineStatusChangedSeam += (line, status) =>
                {
                    _journal?.TryAddRawEntry(
                        "supply_line_status_changed",
                        $"Supply corridor {line} status changed to {status}.",
                        null!, Math.Max(1, _simDay));
                    _territoryControlDirty = true;
                };

                session.System.OnSupplyLineDeliveredSeam += (line, amount) =>
                {
                    _territoryControlDirty = true;
                };

                session.System.OnLocationFortifiedSeam += (loc, lvl) =>
                {
                    _journal?.TryAddRawEntry(
                        "location_fortified",
                        $"{loc} fortification reinforced to rank {lvl}.",
                        null!, Math.Max(1, _simDay));
                    _territoryControlDirty = true;
                };

                _territoryControl = session;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[TerritoryControl] territory system unavailable: {ex.Message}");
                _territoryControl = null;
            }
            return _territoryControl;
        }

        private void SetupTerritoryControl()
        {
            var session = EnsureTerritoryControl();
            if (session == null) return;
            var census = session.ReadCensus();
            GD.Print($"[TerritoryControl] {census.TotalTerritories} territories, {census.TotalNodes} nodes: {census.Describe()}.");
        }

        private void SaveTerritoryControl()
        {
            var session = _territoryControl;
            if (session == null) return;
            CaptureSection(
                TerritoryControlSaveStore.SectionName,
                TerritoryControlSaveStore.TryCapturePersisted(session.CaptureState()));
            _territoryControlDirty = false;
        }

        private void RestoreTerritoryControl(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var session = EnsureTerritoryControl();
            if (session == null) return;

            var state = TerritoryControlSaveStore.TryRestoreBare(json)
                ?? TerritoryControlSaveStore.TryRestore(json);
            if (state != null)
            {
                session.RestoreState(state);
                _territoryControlDirty = false;
            }
        }

        private void FlushTerritoryControlIfDirty()
        {
            if (_territoryControlDirty) SaveTerritoryControl();
        }

        private void ResetTerritoryControl()
        {
            _territoryControl?.Dispose();
            _territoryControl = null;
            _territoryControlDirty = false;
        }

        internal void TickTerritoryControl(int day, ISeededRng? rng = null)
        {
            var session = EnsureTerritoryControl();
            if (session == null) return;
            session.TickDay(day, rng);
            _territoryControlDirty = true;
        }
    }
}
