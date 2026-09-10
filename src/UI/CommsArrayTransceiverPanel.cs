// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 199: long-range communications array console.
    /// Presentation-only — tuning, tier upgrades and strategic uplink requests
    /// are emitted via <see cref="OnActionRequested"/>; the host resolves them
    /// against the bound <see cref="CommsArraySystem"/>, the power grid and the
    /// campaign journal. Orbital windows are stylized game abstractions.
    /// </summary>
    public partial class CommsArrayTransceiverPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private CommsArraySystem? _system;
        private ItemList _targetList = null!;
        private VBoxContainer _detail = null!;
        private int _selectedTargetIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _system != null;

        public void Bind(CommsArraySystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("LONG EAR // COMMUNICATIONS ARRAY", minWidth: 1050, minHeight: 680);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("tier", "Array Tier", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
            _statusRail.AddCard("power", "Grid Power", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("tuned", "Tuned To", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("contacts", "Contacts", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
            _statusRail.AddCard("codes", "Auth Codes", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);

            _targetList = new ItemList
            {
                CustomMinimumSize = new Vector2(320, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _targetList.ItemSelected += index => { _selectedTargetIndex = (int)index; RefreshView(); };

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_targetList);

            var detailScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            detailScroll.AddChild(_detail);
            bodyRow.AddChild(detailScroll);

            _shell.SetContent(bodyRow);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            AddChild(_shell);
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close() { Visible = false; OnClose?.Invoke(); }

        /// <summary>Host feedback strip — tied to the actual command result.</summary>
        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            RefreshView();
        }

        private CommsTargetDefinition? SelectedTarget()
        {
            if (_system == null) return null;
            int idx = _selectedTargetIndex;
            var targets = new List<CommsTargetDefinition>(_system.TargetCatalog.Values);
            targets.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            if (idx < 0 || idx >= targets.Count) return null;
            return targets[idx];
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _targetList == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var state = _system.State;

            // ── Status rail ──
            if (_statusRail != null)
            {
                _statusRail.Set("tier", $"T{state.ArrayTier}", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("power", state.IsPowered ? $"{state.AvailablePowerWatts:0} W" : "BROWNOUT",
                    state.IsPowered ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Critical);
                _statusRail.Set("tuned", $"{state.CurrentFrequencyKhz} kHz {state.CurrentBand}", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("contacts", state.DecodedTargetIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("codes", state.StrategicAuthorizationCodes.Count.ToString(),
                    state.StrategicAuthorizationCodes.Count > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            }

            // ── Target list ──
            _targetList.Clear();
            var targets = new List<CommsTargetDefinition>(_system.TargetCatalog.Values);
            targets.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            _selectedTargetIndex = Math.Clamp(_selectedTargetIndex, -1, Math.Max(0, targets.Count - 1));
            for (int i = 0; i < targets.Count; i++)
            {
                var t = targets[i];
                var lockState = _system.GetOrCreateLock(t.Id);
                string status = lockState.IsContactEstablished ? "CONTACT" : $"lock {lockState.LockPermille / 10.0:0}%";
                string label = $"{t.DisplayName} — {status}";
                if (t.MinArrayTier > state.ArrayTier) label += " (out of range)";
                _targetList.AddItem(label, null, false);
                if (i == _selectedTargetIndex)
                    _targetList.Select(i);
            }

            var selected = _selectedTargetIndex >= 0 && _selectedTargetIndex < targets.Count
                ? targets[_selectedTargetIndex] : null;

            // ── Detail ──
            if (selected == null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No signal families are catalogued. The array scans whatever the antenna can reach.",
                    title: "NO TARGETS KNOWN"));
                return;
            }

            var lock0 = _system.GetOrCreateLock(selected.Id);

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader(selected.DisplayName.ToUpperInvariant()));
            _detail.AddChild(AshfallUiHelpers.MakeMetadata(selected.Description));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Transmission type", ItemDisplay.Prettify(selected.TargetType), AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Carrier", $"{selected.FrequencyKhz} kHz ({selected.Band})", AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Min array tier", $"Tier {selected.MinArrayTier}",
                selected.MinArrayTier > state.ArrayTier ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Power draw", $"{selected.RequiredPowerWatts} W",
                state.AvailablePowerWatts < selected.RequiredPowerWatts ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
            if (selected.HasSatelliteWindow)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Orbital pass", "Short daily window — lock decays outside it", AshfallUiHelpers.ColorInfo));

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("SIGNAL LOCK"));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Lock strength", $"{lock0.LockPermille / 10.0:0}%",
                lock0.IsContactEstablished ? AshfallUiHelpers.ColorSuccess :
                lock0.LockPermille > 0 ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorDim));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Contact", lock0.IsContactEstablished ? "ESTABLISHED" : "not yet",
                lock0.IsContactEstablished ? AshfallUiHelpers.ColorSuccess : AshfallUiHelpers.ColorDim));
            if (lock0.InSatelliteWindow && !lock0.IsContactEstablished)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Window", "OPEN — lock advancing", AshfallUiHelpers.ColorSuccess));

            if (!string.IsNullOrEmpty(lock0.InterceptedData))
            {
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Decoded payload",
                    selected.IsStrategic ? "Strategic authorization code intercepted" : "Telemetry carrier decoded",
                    selected.IsStrategic ? AshfallUiHelpers.ColorCritical : AshfallUiHelpers.ColorText));
            }

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

            bool tunedAway = !string.Equals(state.CurrentBand, selected.Band, StringComparison.OrdinalIgnoreCase)
                             || Math.Abs(state.CurrentFrequencyKhz - selected.FrequencyKhz) > CommsArraySystem.FrequencyToleranceKhz;
            if (tunedAway)
            {
                var tuneBtn = AshfallUiHelpers.MakeButton("TUNE CARRIER", () =>
                    OnActionRequested?.Invoke("tune", selected.Id));
                tuneBtn.TooltipText = $"Moves the array to {selected.FrequencyKhz} kHz {selected.Band}.";
                row.AddChild(tuneBtn);
            }

            if (state.ArrayTier < 3)
            {
                var upgradeBtn = AshfallUiHelpers.MakeButton("UPGRADE ARRAY", () =>
                    OnActionRequested?.Invoke("upgrade_tier", string.Empty));
                upgradeBtn.TooltipText = "Costs 5 salvaged electronics. Raises reach — higher tiers hear farther stations.";
                row.AddChild(upgradeBtn);
            }

            if (selected.IsStrategic && lock0.IsContactEstablished && state.StrategicAuthorizationCodes.Count > 0)
            {
                var strikeBtn = AshfallUiHelpers.MakeButton("REQUEST STRATEGIC UPLINK", () =>
                    OnActionRequested?.Invoke("request_strike", selected.Id));
                strikeBtn.TooltipText = "Spends one intercepted authorization code on a single off-map strategic request. Cannot be undone.";
                row.AddChild(strikeBtn);
            }

            if (row.GetChildCount() == 0)
                row.AddChild(AshfallUiHelpers.MakeMetadata(
                    lock0.IsContactEstablished
                        ? "Carrier held. Keep the array powered to keep the contact alive."
                        : "Awaiting lock — keep the array powered and keep scanning."));
            _detail.AddChild(row);
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
