// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.Audio;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class EmergencyResponseHud : Control, IBindablePanel
    {
        public bool IsVisible => Visible;
        public bool IsBound { get; private set; }
        public event Action? OnPanelClosed;
        public event Action? OnAcknowledge;
        public event Action<string>? OnRequestNavigateToPanel;
        public event Action<string>? OnActionRequested;

        private ColorRect? _backdrop;
        private Label? _severityHeader;
        private Label? _countdown;
        private Button? _closeButton;

        private Label? _crisisTitle;
        private Label? _crisisSummary;
        private Label? _causeChain;

        private GridContainer? _metricGrid;
        private VBoxContainer? _affectedList;
        private VBoxContainer? _actionList;
        private VBoxContainer? _eventLog;

        private Button? _openSourcePanelButton;
        private Button? _acknowledgeButton;

        private string _sourcePanelRoute = "";

        public override void _Ready()
        {
            Visible = false;
            _backdrop = GetNodeOrNull<ColorRect>("%Backdrop");
            _severityHeader = GetNodeOrNull<Label>("%SeverityHeader");
            _countdown = GetNodeOrNull<Label>("%Countdown");
            _closeButton = GetNodeOrNull<Button>("%CloseButton");

            _crisisTitle = GetNodeOrNull<Label>("%CrisisTitle");
            _crisisSummary = GetNodeOrNull<Label>("%CrisisSummary");
            _causeChain = GetNodeOrNull<Label>("%CauseChain");

            _metricGrid = GetNodeOrNull<GridContainer>("%MetricGrid");
            _affectedList = GetNodeOrNull<VBoxContainer>("%AffectedList");
            _actionList = GetNodeOrNull<VBoxContainer>("%ActionList");
            _eventLog = GetNodeOrNull<VBoxContainer>("%EventLog");

            _openSourcePanelButton = GetNodeOrNull<Button>("%OpenSourcePanelButton");
            _acknowledgeButton = GetNodeOrNull<Button>("%AcknowledgeButton");

            if (_closeButton != null)
            {
                _closeButton.Pressed += () =>
                {
                    AudioManager.Instance?.PlayUiClick();
                    Close();
                };
            }

            if (_acknowledgeButton != null)
            {
                _acknowledgeButton.Pressed += () =>
                {
                    AudioManager.Instance?.PlayUiConfirm();
                    OnAcknowledge?.Invoke();
                    Close();
                };
            }

            if (_openSourcePanelButton != null)
            {
                _openSourcePanelButton.Pressed += () =>
                {
                    AudioManager.Instance?.PlayUiConfirm();
                    if (!string.IsNullOrEmpty(_sourcePanelRoute))
                    {
                        OnRequestNavigateToPanel?.Invoke(_sourcePanelRoute);
                        Close();
                    }
                };
            }
        }

        public void Bind(CrisisPresentationSnapshot snapshot)
        {
            if (snapshot == null) return;
            IsBound = true;

            // 1. Header & Severity
            if (_severityHeader != null)
            {
                string severityTag = snapshot.Severity switch
                {
                    CrisisSeverity.Terminal => "[TERMINAL EMERGENCY]",
                    CrisisSeverity.Catastrophic => "[CATASTROPHIC BREACH]",
                    CrisisSeverity.Critical => "[CRITICAL CRISIS COMMAND]",
                    CrisisSeverity.Severe => "[SEVERE ALERT]",
                    CrisisSeverity.Warning => "[WARNING ADVISORY]",
                    CrisisSeverity.Elevated => "[ELEVATED INCIDENT]",
                    CrisisSeverity.Advisory => "[ADVISORY NOTICE]",
                    _ => "[STANDARD MONITORING]"
                };
                _severityHeader.Text = $"{severityTag} — {snapshot.Kind.ToUpperInvariant()}";
            }

            // Severity color modulation
            Color accentColor = snapshot.Severity switch
            {
                CrisisSeverity.Terminal or CrisisSeverity.Catastrophic or CrisisSeverity.Critical => AshfallUiHelpers.ToColor(DesignTheme.Critical),
                CrisisSeverity.Severe => AshfallUiHelpers.ToColor(DesignTheme.Warning),
                CrisisSeverity.Warning or CrisisSeverity.Elevated => AshfallUiHelpers.ToColor(DesignTheme.Warning),
                _ => AshfallUiHelpers.ToColor(DesignTheme.Pale)
            };
            if (_severityHeader != null) _severityHeader.Modulate = accentColor;
            if (_backdrop != null)
            {
                _backdrop.Color = snapshot.Severity >= CrisisSeverity.Severe
                    ? new Color(DesignTheme.Critical.r * 0.12f, DesignTheme.Critical.g * 0.12f, DesignTheme.Critical.b * 0.12f, 0.95f)
                    : AshfallUiHelpers.PanelScrim();
            }

            // 2. Countdown
            if (_countdown != null)
            {
                if (snapshot.SecondsRemaining.HasValue && snapshot.SecondsRemaining.Value > 0f)
                {
                    _countdown.Visible = true;
                    int mins = (int)(snapshot.SecondsRemaining.Value / 60f);
                    int secs = (int)(snapshot.SecondsRemaining.Value % 60f);
                    _countdown.Text = $"TIME: {mins:D2}:{secs:D2}";
                }
                else
                {
                    _countdown.Visible = false;
                }
            }

            // 3. Titles and Cause Chain
            if (_crisisTitle != null) _crisisTitle.Text = snapshot.Title;
            if (_crisisSummary != null) _crisisSummary.Text = snapshot.Summary;
            if (_causeChain != null)
            {
                string cause = !string.IsNullOrEmpty(snapshot.Cause) ? snapshot.Cause : "System anomaly";
                string effect = !string.IsNullOrEmpty(snapshot.EffectText) ? snapshot.EffectText : snapshot.Summary;
                _causeChain.Text = $"CAUSE: {cause}  →  EFFECT: {effect}";
            }

            // 4. Metrics Grid
            if (_metricGrid != null)
            {
                foreach (Node child in _metricGrid.GetChildren())
                    child.QueueFree();

                foreach (var metric in snapshot.Metrics)
                {
                    var lbl = new Label();
                    string trendStr = !string.IsNullOrEmpty(metric.Trend) ? $" {metric.Trend}" : "";
                    lbl.Text = $"{metric.Label}: {metric.ValueText}{trendStr}";
                    if (metric.IsFailing)
                        lbl.Modulate = AshfallUiHelpers.ToColor(DesignTheme.Critical);
                    _metricGrid.AddChild(lbl);
                }
            }

            // 5. Affected Roster
            _sourcePanelRoute = "";
            if (_affectedList != null)
            {
                foreach (Node child in _affectedList.GetChildren())
                    child.QueueFree();

                foreach (var aff in snapshot.Affected)
                {
                    if (string.IsNullOrEmpty(_sourcePanelRoute) && !string.IsNullOrEmpty(aff.NavigationTarget))
                        _sourcePanelRoute = aff.NavigationTarget;

                    var row = new HBoxContainer();
                    var nameLbl = new Label { Text = $"• {aff.Name} ({aff.Role}):", SizeFlagsHorizontal = SizeFlags.ExpandFill };
                    var statusLbl = new Label { Text = aff.Status };
                    if (aff.IsCritical) statusLbl.Modulate = AshfallUiHelpers.ToColor(DesignTheme.Critical);

                    row.AddChild(nameLbl);
                    row.AddChild(statusLbl);
                    _affectedList.AddChild(row);
                }
            }

            // 6. Action Cards
            if (_actionList != null)
            {
                foreach (Node child in _actionList.GetChildren())
                    child.QueueFree();

                foreach (var act in snapshot.Actions)
                {
                    var btn = new Button
                    {
                        Text = string.IsNullOrEmpty(act.CostText) ? act.Label : $"{act.Label} ({act.CostText})",
                        CustomMinimumSize = new Vector2(240, 36),
                        Disabled = !act.IsEnabled
                    };
                    if (!act.IsEnabled)
                        btn.TooltipText = "Action unavailable — prerequisites not met.";
                    string actionId = act.ActionId;
                    btn.Pressed += () =>
                    {
                        AudioManager.Instance?.PlayUiConfirm();
                        if (actionId == "ack")
                        {
                            OnAcknowledge?.Invoke();
                            Close();
                        }
                        else
                        {
                            OnActionRequested?.Invoke(actionId);
                        }
                    };
                    _actionList.AddChild(btn);
                }
            }

            // 7. Event Log
            if (_eventLog != null)
            {
                foreach (Node child in _eventLog.GetChildren())
                    child.QueueFree();

                foreach (var entry in snapshot.Log)
                {
                    var logLbl = new Label
                    {
                        Text = $"[{entry.Timestamp}] {entry.Message}",
                        AutowrapMode = TextServer.AutowrapMode.WordSmart
                    };
                    if (entry.IsError)
                        logLbl.Modulate = AshfallUiHelpers.ToColor(DesignTheme.Critical);
                    _eventLog.AddChild(logLbl);
                }
            }

            // 8. Navigation Button
            if (_openSourcePanelButton != null)
            {
                _openSourcePanelButton.Visible = !string.IsNullOrEmpty(_sourcePanelRoute);
                if (!string.IsNullOrEmpty(_sourcePanelRoute))
                    _openSourcePanelButton.Text = $"VIEW {_sourcePanelRoute.ToUpperInvariant().Replace('_', ' ')}";
            }
        }

        public void Open()
        {
            Visible = true;

            // Truthful standby state (UI/UX audit silent-failure sweep): an
            // unbound HUD must say so instead of rendering an empty shell, and a
            // stale snapshot from a previous crisis must not be re-presented.
            if (!IsBound)
            {
                if (_severityHeader != null) _severityHeader.Text = "RESPONSE STANDBY";
                if (_crisisTitle != null) _crisisTitle.Text = "NO ACTIVE CRISIS";
                if (_crisisSummary != null)
                    _crisisSummary.Text = "No crisis is bound. This console populates when a crisis is detected.";
            }

            // Focus only when anchored: the panel audits construct this HUD
            // outside the scene tree, where GrabFocus would push an engine error.
            if (_acknowledgeButton != null && _acknowledgeButton.IsInsideTree())
                _acknowledgeButton.GrabFocus();
            if (IsBound)
            {
                AudioManager.Instance?.SetSnapshot(AudioSnapshot.ShelterCrisis);
            }
        }

        public void Close() {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            AudioManager.Instance?.SetSnapshot(AudioSnapshot.Normal);
            OnPanelClosed?.Invoke();
        }

        public void Unbind()
        {
            IsBound = false;
        }

        // Unhandled phase, not `_Input` (a11y audit 2026-09-29, §9.2): `_Input`
        // preempted stacked modals added later in the tree, so Esc closed the
        // crisis HUD instead of the topmost modal. Tree-order unhandled
        // propagation gives later siblings the first claim.
        public override void _UnhandledKeyInput(InputEvent @event)
        {
            if (!Visible || !@event.IsPressed() || @event.IsEcho()) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                Close();
                GetViewport()?.SetInputAsHandled();
            }
        }
    }
}
