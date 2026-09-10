// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.YearOfAsh;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 197: deep-freeze watch — indoor temperature, intake icing,
    /// insulation and pipeline risk for the deep-winter campaign phase.
    /// Presentation-only — mitigation commands are emitted via
    /// <see cref="OnActionRequested"/> and resolved by the host through the
    /// bound <see cref="YearOfAshDeepFreezeSystem"/> and the inventory
    /// authority. Numeric-freeze alarms pair text with color (accessibility).
    /// </summary>
    public partial class WinterFreezePanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private YearOfAshDeepFreezeSystem? _system;
        private VBoxContainer _detail = null!;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _system != null;

        public void Bind(YearOfAshDeepFreezeSystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("DEEP WINTER // FREEZE WATCH", minWidth: 950, minHeight: 620);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("indoor", "Indoor Temp", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("intake", "Intake Ice", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("insulation", "Insulation", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("frozen", "Frozen Days", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var detailScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            detailScroll.AddChild(_detail);

            _shell.SetContent(detailScroll);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            AddChild(_shell);
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close() { Visible = false; OnClose?.Invoke(); }

        /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        /// <summary>Host feedback strip — tied to the actual command result.</summary>
        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            LastFeedback = message;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_detail == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            if (_system == null)
            {
                _statusRail?.Set("indoor", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail?.Set("intake", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail?.Set("insulation", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail?.Set("frozen", "—", AshfallMetricCard.Criticality.Normal);
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No freeze telemetry is wired. The shelter's thermal state is owned by the shelter thermal layer.",
                    title: "TELEMETRY OFFLINE"));
                return;
            }

            var state = _system.State;

            // ── Status rail ──
            if (_statusRail != null)
            {
                _statusRail.Set("indoor", $"{_system.IndoorTempCelsius:0.0} °C",
                    _system.IndoorTempCelsius <= 0f ? AshfallMetricCard.Criticality.Critical :
                    _system.IndoorTempCelsius < 10f ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("intake", _system.IsIntakeBlocked
                        ? $"BLOCKED ({_system.IntakeIceMm:0} mm)"
                        : $"{_system.IntakeIceMm:0} mm",
                    _system.IsIntakeBlocked ? AshfallMetricCard.Criticality.Critical :
                    _system.IntakeIceMm > 30f ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("insulation", $"{state.thermalInsulationQuality * 100f:0}%",
                    state.thermalInsulationQuality < 0.5f ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("frozen", state.daysFrozenPipelinesExperienced.ToString(),
                    state.daysFrozenPipelinesExperienced > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            }

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader("SHELTER THERMAL STATE"));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Indoor temperature", $"{_system.IndoorTempCelsius:0.0} °C",
                _system.IndoorTempCelsius <= 0f ? AshfallUiHelpers.ColorCritical :
                _system.IndoorTempCelsius < 10f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorSuccess));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Target band", "18–22 °C", AshfallUiHelpers.ColorDim));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Insulation quality", $"{state.thermalInsulationQuality * 100f:0}%",
                state.thermalInsulationQuality < 0.5f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Geothermal flow", $"{state.geothermalFlowRatePercent:0}%",
                state.geothermalFlowRatePercent < 40f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("AIR INTAKE"));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Rime ice", $"{_system.IntakeIceMm:0} mm",
                _system.IsIntakeBlocked ? AshfallUiHelpers.ColorCritical :
                _system.IntakeIceMm > 30f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
            if (_system.IsIntakeBlocked)
                _detail.AddChild(AshfallUiHelpers.MakeWarning(
                    "Ventilation intake is choked with rime ice. Airflow suffers until a team clears it."));
            else if (_system.IntakeIceMm > 30f)
                _detail.AddChild(AshfallUiHelpers.MakeWarning(
                    "Ice is building on the intake cowling. Above 50 mm the intake blocks."));
            else
                _detail.AddChild(AshfallUiHelpers.MakeMetadata("Intake clear. Cold snaps below −15 °C build ice; milder weather melts it."));

            if (state.daysFrozenPipelinesExperienced > 0)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Pipelines frozen (days)", state.daysFrozenPipelinesExperienced.ToString(), AshfallUiHelpers.ColorCritical));

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            // ── Actions ──
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

            if (_system.IntakeIceMm > 0f || _system.IsIntakeBlocked)
            {
                var clearBtn = AshfallUiHelpers.MakeButton("DISPATCH ICE CLEARING", () =>
                    OnActionRequested?.Invoke("clear_ice", string.Empty));
                clearBtn.TooltipText = "Sends a cold-work party to knock the rime off the intake cowling.";
                row.AddChild(clearBtn);
            }

            if (state.thermalInsulationQuality < 1.0f)
            {
                var insulBtn = AshfallUiHelpers.MakeButton("REINFORCE INSULATION", () =>
                    OnActionRequested?.Invoke("insulate", string.Empty));
                insulBtn.TooltipText = "Costs 4 scrap wood and 1 cloth. Raises insulation quality by 10 points.";
                row.AddChild(insulBtn);
            }

            if (row.GetChildCount() == 0)
                row.AddChild(AshfallUiHelpers.MakeMetadata("Nothing needs doing — the shelter holds its warmth."));
            _detail.AddChild(row);

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSmall(
                "Cold does not negotiate: sub-zero indoor days freeze pipelines, chill crops and reach the sick first. Insulation and airflow are the two levers you own.", autowrap: true));
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
