// SPDX-License-Identifier: MIT
// ============================================================================
// UI Panel: Aviation & Aerial Reconnaissance (Plan 182)
// Displays hangar aircraft, airworthiness, active flights, and flight risk breakdown.
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class AviationUI : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private AviationSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(AviationSystem system)
        {
            _system = system;
            RefreshView();
        }

        public void Unbind()
        {
            _system = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Aviation // Airfield & Reconnaissance", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("aircraft", "Hangar Planes", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("active_flights", "Airborne Flights", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("crashes", "Crashes / Incidents", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "No active aircraft registered in the shelter airfield or hangar.";
            _contentStack.AddChild(_detailText);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());

            // Overlay panels start hidden; PanelRegistry drives visibility.
            Visible = false;

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public void RefreshView()
        {
            if (_statusRail == null || _detailText == null) return;
            if (_system == null)
            {
                _detailText.Text = "Aviation system offline.";
                return;
            }

            _statusRail.Set("aircraft", _system.Aircraft.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("active_flights", _system.ActiveFlights.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("crashes", _system.TotalCrashes.ToString(), _system.TotalCrashes > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== HANGAR INVENTORY & AIRWORTHINESS ===");
            if (_system.Aircraft.Count == 0)
            {
                sb.AppendLine("  No airframes available. Construct an observation balloon or ultralight from parts.");
            }
            else
            {
                foreach (var plane in _system.Aircraft)
                {
                    var def = _system.GetDefinition(plane.definitionId);
                    string name = def?.name ?? plane.definitionId;
                    string status = plane.isCommitted ? "[AIRBORNE / COMMITTED]" : "[READY]";
                    sb.AppendLine($"  - {plane.aircraftId} ({name}): Airworthiness {plane.airworthiness:F0}% | Total Flight Hours: {plane.totalHoursFlown:F1}h {status}");
                }
            }

            sb.AppendLine();
            sb.AppendLine("=== ACTIVE RECONNAISSANCE FLIGHTS ===");
            if (_system.ActiveFlights.Count == 0)
            {
                sb.AppendLine("  No active sorties in flight.");
            }
            else
            {
                foreach (var flight in _system.ActiveFlights)
                {
                    sb.AppendLine($"  - Flight {flight.flightId}: Plane: {flight.aircraftId} | Phase: {flight.phase} | Route Progress: {flight.progressKm:F1} / {flight.routeDistanceKm:F1} km | Map Cells Uncovered: {flight.mapCellsRevealed}");
                }
            }

            _detailText.Text = sb.ToString();
        }
    }
}
