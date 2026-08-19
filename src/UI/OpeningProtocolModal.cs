using System;
using Godot;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Tactile 9-slice modal for resolving the Day 1 opening survival protocol:
    /// Morning Ration Triage, Midday Maintenance, and Evening Radio Transmission.
    /// </summary>
    public partial class OpeningProtocolModal : Control
    {
        public event Action<RationPolicy>? OnRationPolicySelected;
        public event Action<MaintenanceDirective>? OnMaintenanceDirectiveSelected;
        public event Action<RadioProtocol>? OnRadioProtocolSelected;
        public event Action? OnClose;

        private Label _rationStatus = null!;
        private Label _maintenanceStatus = null!;
        private Label _radioStatus = null!;
        private VBoxContainer _logList = null!;

        private StartingLevelHostSession? _startingHost;

        public void Bind(StartingLevelHostSession session)
        {
            _startingHost = session;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_rationStatus == null || _startingHost == null) return;

            var state = _startingHost.System.State;

            _rationStatus.Text = state.morningTriageResolved
                ? $"RESOLVED // {state.rationPolicy.ToString().ToUpperInvariant()}"
                : "PENDING // AWAITING RATION DIRECTIVE";
            _rationStatus.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(state.morningTriageResolved ? DesignTheme.Pale : DesignTheme.Warm));

            _maintenanceStatus.Text = state.middayMaintenanceResolved
                ? $"RESOLVED // {state.maintenanceDirective.ToString().ToUpperInvariant()}"
                : "PENDING // AWAITING SHIFT DIRECTIVE";
            _maintenanceStatus.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(state.middayMaintenanceResolved ? DesignTheme.Pale : DesignTheme.Warm));

            _radioStatus.Text = state.eveningRadioResolved
                ? $"RESOLVED // {state.radioProtocol.ToString().ToUpperInvariant()}"
                : "PENDING // AWAITING SIGNAL PROTOCOL";
            _radioStatus.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(state.eveningRadioResolved ? DesignTheme.Pale : DesignTheme.Warm));

            AshfallUiHelpers.EmptyChildren(_logList);

            int count = Math.Min(6, state.journalDirectives.Count);
            for (int i = state.journalDirectives.Count - count; i < state.journalDirectives.Count; i++)
            {
                if (i >= 0 && i < state.journalDirectives.Count)
                {
                    var lbl = AshfallUiHelpers.MakeSmall(state.journalDirectives[i]);
                    lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
                    _logList.AddChild(lbl);
                }
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(740, 600);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("HOLDFAST PROTOCOL DIRECTIVES", DesignTheme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(700, 480),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            var content = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            content.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(content);

            // ── PART 1: MORNING RATION TRIAGE ──
            content.AddChild(AshfallUiHelpers.MakeSectionHeader("1. MORNING RATION TRIAGE"));
            _rationStatus = AshfallUiHelpers.MakeMono("PENDING // AWAITING RATION DIRECTIVE");
            content.AddChild(_rationStatus);

            var rationRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnStandard = AshfallUiHelpers.MakeButton("STANDARD (100%)", () =>
            {
                OnRationPolicySelected?.Invoke(RationPolicy.Standard);
                RefreshView();
            });
            btnStandard.CustomMinimumSize = new Vector2(210, 34);
            rationRow.AddChild(btnStandard);

            var btnHalf = AshfallUiHelpers.MakeButton("HALF RATIONS (50%)", () =>
            {
                OnRationPolicySelected?.Invoke(RationPolicy.Half);
                RefreshView();
            });
            btnHalf.CustomMinimumSize = new Vector2(210, 34);
            rationRow.AddChild(btnHalf);

            var btnIrradiated = AshfallUiHelpers.MakeButton("IRRADIATED WATER", () =>
            {
                OnRationPolicySelected?.Invoke(RationPolicy.Irradiated);
                RefreshView();
            });
            btnIrradiated.CustomMinimumSize = new Vector2(210, 34);
            rationRow.AddChild(btnIrradiated);
            content.AddChild(rationRow);

            content.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── PART 2: MIDDAY MAINTENANCE DIRECTIVE ──
            content.AddChild(AshfallUiHelpers.MakeSectionHeader("2. MIDDAY MAINTENANCE DIRECTIVE"));
            _maintenanceStatus = AshfallUiHelpers.MakeMono("PENDING // AWAITING SHIFT DIRECTIVE");
            content.AddChild(_maintenanceStatus);

            var maintRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnServiceFilter = AshfallUiHelpers.MakeButton("SERVICE HEPA FILTER", () =>
            {
                OnMaintenanceDirectiveSelected?.Invoke(MaintenanceDirective.ServiceFilterStack);
                RefreshView();
            });
            btnServiceFilter.CustomMinimumSize = new Vector2(210, 34);
            maintRow.AddChild(btnServiceFilter);

            var btnLeadBunk = AshfallUiHelpers.MakeButton("FORTIFY BUNK CEILING", () =>
            {
                OnMaintenanceDirectiveSelected?.Invoke(MaintenanceDirective.FortifyBunksLead);
                RefreshView();
            });
            btnLeadBunk.CustomMinimumSize = new Vector2(210, 34);
            maintRow.AddChild(btnLeadBunk);

            var btnCalibrate = AshfallUiHelpers.MakeButton("CALIBRATE DOSIMETERS", () =>
            {
                OnMaintenanceDirectiveSelected?.Invoke(MaintenanceDirective.CalibrateMonitors);
                RefreshView();
            });
            btnCalibrate.CustomMinimumSize = new Vector2(210, 34);
            maintRow.AddChild(btnCalibrate);
            content.AddChild(maintRow);

            content.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── PART 3: EVENING RADIO PROTOCOL ──
            content.AddChild(AshfallUiHelpers.MakeSectionHeader("3. EVENING RADIO TRANSMISSION PROTOCOL"));
            _radioStatus = AshfallUiHelpers.MakeMono("PENDING // AWAITING SIGNAL PROTOCOL");
            content.AddChild(_radioStatus);

            var radioRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnAckBarons = AshfallUiHelpers.MakeButton("ACKNOWLEDGE BARONS", () =>
            {
                OnRadioProtocolSelected?.Invoke(RadioProtocol.AcknowledgeHydroBarons);
                RefreshView();
            });
            btnAckBarons.CustomMinimumSize = new Vector2(210, 34);
            radioRow.AddChild(btnAckBarons);

            var btnSilence = AshfallUiHelpers.MakeButton("MAINTAIN SILENCE", () =>
            {
                OnRadioProtocolSelected?.Invoke(RadioProtocol.MaintainSilence);
                RefreshView();
            });
            btnSilence.CustomMinimumSize = new Vector2(210, 34);
            radioRow.AddChild(btnSilence);

            var btnBeacon = AshfallUiHelpers.MakeButton("BROADCAST BEACON", () =>
            {
                OnRadioProtocolSelected?.Invoke(RadioProtocol.BroadcastBeacon);
                RefreshView();
            });
            btnBeacon.CustomMinimumSize = new Vector2(210, 34);
            radioRow.AddChild(btnBeacon);
            content.AddChild(radioRow);

            content.AddChild(AshfallUiHelpers.MakeSeparator());

            content.AddChild(AshfallUiHelpers.MakeSectionHeader("RECENT PROTOCOL DIRECTIVES"));
            _logList = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            content.AddChild(_logList);

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
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
