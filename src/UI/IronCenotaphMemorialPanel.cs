// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class IronCenotaphMemorialPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label? _headerTitleLabel;
        private Label? _statusBadgeLabel;
        private Button? _closeButton;
        private VBoxContainer? _telemetryContainer;
        private VBoxContainer? _buttonContainer;
        private VBoxContainer? _dataContainer;
        private Label? _logOutputLabel;

        /// <summary>Plan 24C (A3) — the vigil command: mourns the most recent
        /// unmourned loss through the memorial owner; the result text is the
        /// only feedback (never a silent no-op).</summary>
        private void OnVigilPressed()
        {
            if (_mourning == null)
            {
                if (_logOutputLabel != null)
                    _logOutputLabel.Text = "[RIT-01] No memorial ledger bound. The dead stay uncounted.";
                return;
            }
            var pending = _mourning.LatestUnmourned();
            if (pending == null)
            {
                if (_logOutputLabel != null)
                    _logOutputLabel.Text = "[RIT-01] Every recorded loss has had its vigil.";
                return;
            }
            var result = _mourning.Mourn(pending.Value.id);
            if (_logOutputLabel != null)
                _logOutputLabel.Text = result.IsSuccess
                    ? $"[RIT-01] The shelter stood together and mourned. (D{pending.Value.day} loss)"
                    : "[RIT-01] The vigil could not begin: "
                        + (result.MessageKey ?? result.FailureCode ?? "unspecified")
                        .Replace("memorial.", "").Replace('_', ' ') + ".";
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildInterface();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public bool IsBound { get; private set; } = true;

        public void Bind(object? session)
        {
            IsBound = true;
            RefreshView();
        }

        public void Unbind()
        {
            IsBound = false;
        }

        /// <summary>
        /// Plan 24C (A3) — the mourning route's binding: the memorial owner's
        /// read model plus the once-per-death vigil command. The panel renders
        /// truthful state and dispatches the command; it never recomputes
        /// grief or morale (the host owns the attributed recovery).
        /// </summary>
        public sealed class MourningBinding
        {
            public Func<int> TotalDeaths { get; init; } = () => 0;
            public Func<(string id, int day)?> LatestUnmourned { get; init; } = () => null;
            /// <summary>deceasedId → the memorial owner's action result.</summary>
            public Func<string, ActionResult> Mourn { get; init; } = _ =>
                ActionResult.Blocked("unbound", "memorial.mourn_unbound");
        }

        private MourningBinding? _mourning;

        public void BindMourning(MourningBinding binding)
        {
            _mourning = binding;
            IsBound = binding != null;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_statusBadgeLabel != null)
            {
                // Plan 24C (A3): truthful status from the memorial owner when
                // bound; the unbound placeholder keeps its historical text.
                if (_mourning != null)
                {
                    var pending = _mourning.LatestUnmourned();
                    _statusBadgeLabel.Text = pending == null
                        ? $"STATUS: MEMORIAL FLAME ACTIVE - RECORDED: {_mourning.TotalDeaths()} SOULS - ALL MOURNED"
                        : $"STATUS: MEMORIAL FLAME ACTIVE - RECORDED: {_mourning.TotalDeaths()} SOULS - VIGIL PENDING (D{pending.Value.day})";
                }
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "SHELTER COMMEMORATION // THE IRON CENOTAPH [RIT-01]",
                "STATUS: MEMORIAL FLAME ACTIVE - RECORDED: 38 SOULS (-18% GRIEF)",
                AshfallUiHelpers.ToColor(DesignTheme.Warm),
                "[X] CLOSE CONSOLE",
                "[RIT-01] Bronze plaque carved for Dr. Aris Thorne.\n[RIT-01] Memorial vigil conducted. Living survivor grief mitigated.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("CASUALTY ROLL & MORTALITY CAUSES");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RECORDED CASUALTIES", "38 SOULS ON MEMORIAL WALL", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RADIATION POISONING", "42.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("STARVATION & DEHYDRATION", "24.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COMBAT & TRAUMA", "34.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("WALL PLAQUE CAPACITY", "38 / 80 SLOTS OCCUPIED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("BRONZE EPITAPH ENGRAVER & ETERNAL FLAME");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[ENGRAVE BRONZE MEMORIAL PLAQUE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[REFUEL ETERNAL MEMORIAL FLAME]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            // Plan 24C (A3): the vigil is the mourning action — once per death,
            // routed through the memorial owner's command with truthful feedback.
            var vigilButton = new Button { Text = "[HOLD ALL-SHELTER VIGIL & MOMENT OF SILENCE]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            vigilButton.Pressed += OnVigilPressed;
            _buttonContainer.AddChild(vigilButton);
            _buttonContainer.AddChild(new Button { Text = "[RECITE DIEGETIC COMMEMORATION EULOGY]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("MEMORIAL RELICS & VIGIL ATTENDANCE");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("ETERNAL OIL FLAME RESERVOIR", "42.5 LITERS (0.2 L/DAY)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("MEMORIAL RELICS PRESERVED", "24 DOG TAGS / 6 WATCHES", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SHELTER GRIEF MITIGATION", "-18.5% DESPAIR INDEX", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LAST VIGIL ATTENDANCE", "94% OF LIVING POPULATION", AshfallUiHelpers.ToColor(DesignTheme.Warm)));

        }
    }
}
