using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class RegionalTreatyPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _ratifyBtn = null!;
        private Button _proposeBtn = null!;

        private RegionalTreatyHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(RegionalTreatyHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }



        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Regional Treaty // Diplomatic Accords", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active_treaties", "Active Accords", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("compliance", "Compliance Rate", "100%", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _proposeBtn = new Button { Text = "Propose Non-Aggression Pact", CustomMinimumSize = new Vector2(200, 36) };
            _proposeBtn.Pressed += () => _host?.ProposeTreaty("treaty_meridian_non_aggression", 1);
            buttonRow.AddChild(_proposeBtn);

            _ratifyBtn = new Button { Text = "Ratify Pending Accord", CustomMinimumSize = new Vector2(180, 36) };
            _ratifyBtn.Pressed += () => _host?.RatifyTreaty("treaty_meridian_non_aggression", 1);
            buttonRow.AddChild(_ratifyBtn);

            _contentStack.AddChild(buttonRow);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var s = _host.System.State;
            int active = s.treaties.FindAll(t => t.status == TreatyStatus.Active || t.status == TreatyStatus.Ratified).Count;
            _statusRail.Set("active_treaties", active.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("compliance", active > 0 ? "95%" : "100%", AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string text = $"Regional Diplomatic Treaties ({s.treaties.Count} total):\n";
                foreach (var t in s.treaties)
                {
                    text += $"  • [{t.status}] {t.treatyId} (Compliance: {t.complianceScore:P0}, Ratified Day: {t.ratifiedDay})\n";
                }
                text += $"\nLast Event: {_host.LastEvent}";
                _detailText.Text = text;
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
