// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Borehole seismograph surface (UI/UX audit 2026-09-25 — converted from a
    /// static prototype shell into a truthful read-only projection).
    ///
    /// Authority: <see cref="SeismicDynamicsSystem"/> (Plan B68) — recent quake
    /// events, fault tension/shoring, active early warnings, dampener
    /// integrity, geophone coverage, and shear/rupture damage lists. No local
    /// state: values are refreshed from the system on every bind and open.
    /// </summary>
    public partial class BoreholeSeismographPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _status = null!;
        private VBoxContainer _events = null!;
        private VBoxContainer _faults = null!;
        private SeismicDynamicsSystem? _seismic;

        public bool IsBound { get; private set; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildInterface();
        }

        /// <summary>Binds the live seismic dynamics system.</summary>
        public void Bind(SeismicDynamicsSystem? seismic)
        {
            _seismic = seismic;
            IsBound = seismic != null;
            RefreshView();
        }

        public void Unbind()
        {
            _seismic = null;
            IsBound = false;
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "BOREHOLE SEISMOGRAPH // FAULT MONITORING & EARLY WARNING",
                "[WAITING FOR SESSION]",
                AshfallUiHelpers.ToColor(DesignTheme.Dim),
                "[X] CLOSE CONSOLE",
                "Seismic telemetry reads from the live SeismicDynamicsSystem; values refresh on open.",
                () => OnClose?.Invoke());
            _status = chrome.Status;
            var body = chrome.Body;

            var left = ThreePanePanelScaffold.CreatePanelFrame("RECENT SEISMIC EVENTS");
            body.AddChild(left);
            _events = ThreePanePanelScaffold.CreateColumn(left, DesignTheme.SpacingSm);

            var right = ThreePanePanelScaffold.CreatePanelFrame("FAULT TENSION, DAMPENERS & WARNINGS");
            body.AddChild(right);
            _faults = ThreePanePanelScaffold.CreateColumn(right, DesignTheme.SpacingSm);

            RefreshView();
        }

        public void RefreshView()
        {
            ClearChildren(_events);
            ClearChildren(_faults);
            if (_seismic == null)
            {
                _status.Text = "[NOT CONNECTED — SEISMIC NETWORK NOT INITIALIZED]";
                _status.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                _faults.AddChild(AshfallUiHelpers.MakeBody(
                    "Seismic authority unavailable — start or load a campaign to read live monitoring."));
                return;
            }

            var state = _seismic.State;
            int quakeCount = state.recentQuakes?.Count ?? 0;
            float latestMagnitude = state.recentQuakes is { Count: > 0 } quakes ? quakes[^1].magnitude : 0f;
            bool warningActive = state.activeEarlyWarnings != null && state.activeEarlyWarnings.Count > 0;
            int shearedPipes = state.recentQuakes?.Sum(q => q.shearedPipes?.Count ?? 0) ?? 0;
            int rupturedRadiators = state.recentQuakes?.Sum(q => q.rupturedRadiators?.Count ?? 0) ?? 0;
            bool damage = shearedPipes > 0 || rupturedRadiators > 0;

            _status.Text = warningActive
                ? $"EARLY WARNING ACTIVE — {state.activeEarlyWarnings!.Count} SECTOR(S)"
                : damage
                    ? "STRUCTURAL DAMAGE LOGGED — INSPECT SHEAR LIST"
                    : $"MONITORING — {quakeCount} EVENT(S) LOGGED";
            _status.AddThemeColorOverride("font_color",
                warningActive || damage
                    ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                    : AshfallUiHelpers.ToColor(DesignTheme.Success));

            _events.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "EVENTS LOGGED", $"{quakeCount}",
                AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            _events.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "LATEST MAGNITUDE", quakeCount > 0 ? $"{latestMagnitude:0.0}" : "—",
                latestMagnitude >= 5.5f
                    ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                    : AshfallUiHelpers.ToColor(DesignTheme.Pale)));

            _events.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "STRUCTURAL DAMAGE LOGGED",
                $"PIPES {shearedPipes} · RADIATORS {rupturedRadiators}",
                damage
                    ? AshfallUiHelpers.ToColor(DesignTheme.Warning)
                    : AshfallUiHelpers.ToColor(DesignTheme.Muted)));

            if (state.recentQuakes != null)
            {
                foreach (var quake in state.recentQuakes.Skip(Math.Max(0, quakeCount - 6)))
                {
                    _events.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                        $"DAY {quake.day} · {quake.faultId}",
                        $"M{quake.magnitude:0.0} · {quake.depthLayer}",
                        quake.magnitude >= 5.5f
                            ? AshfallUiHelpers.ToColor(DesignTheme.Warning)
                            : AshfallUiHelpers.ToColor(DesignTheme.Muted)));
                }
            }

            foreach (var warning in (state.activeEarlyWarnings ?? new System.Collections.Generic.List<string>()).Take(4))
            {
                _faults.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                    "WARNING", warning, AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            }

            if (state.faults != null)
            {
                foreach (var fault in state.faults.Values.Take(6))
                {
                    string shoring = fault.temporaryShoringDays > 0 ? $" · SHORED {fault.temporaryShoringDays}D" : "";
                    _faults.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                        fault.faultId,
                        $"TENSION {fault.currentTension:0.#} · SLIPS {fault.totalSlips}{shoring}",
                        fault.currentTension >= 70f
                            ? AshfallUiHelpers.ToColor(DesignTheme.Warning)
                            : AshfallUiHelpers.ToColor(DesignTheme.Muted)));
                }
            }

            float dampenerAvg = state.dampenerIntegrity != null && state.dampenerIntegrity.Count > 0
                ? state.dampenerIntegrity.Values.Average()
                : 100f;
            _faults.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "DAMPENER INTEGRITY (AVG)", $"{dampenerAvg:0.#}%",
                dampenerAvg < 50f
                    ? AshfallUiHelpers.ToColor(DesignTheme.Warning)
                    : AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            _faults.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "GEOPHONE SECTORS", $"{state.geophoneSectors?.Count ?? 0}",
                AshfallUiHelpers.ToColor(DesignTheme.Muted)));
        }

        private static void ClearChildren(Node parent)
        {
            for (int i = parent.GetChildCount() - 1; i >= 0; i--)
            {
                var child = parent.GetChild(i);
                parent.RemoveChild(child);
                child.Free();
            }
        }
    }
}
