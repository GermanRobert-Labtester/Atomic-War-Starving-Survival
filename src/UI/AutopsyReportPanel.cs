using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class AutopsyReportPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _queueAutopsyBtn = null!;

        private AutopsyHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(AutopsyHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Clinical Autopsy // Forensic Pathology", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("cases", "Autopsy Cases", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("completed", "Examinations Complete", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _queueAutopsyBtn = new Button { Text = "Queue Standard Post-Mortem", CustomMinimumSize = new Vector2(220, 36) };
            _queueAutopsyBtn.Pressed += () => _host?.QueueCase("specimen_survivor_01", "procedure_standard_autopsy", "Chief_Medical_Officer", 1);
            buttonRow.AddChild(_queueAutopsyBtn);

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
            if (_host == null || _statusRail == null)
            {
                if (_detailText != null)
                {
                    _detailText.Text = "Autopsy report host session is not bound. Post-mortem records and tissue pathology findings are offline.";
                }
                return;
            }

            var s = _host.System.State;
            _statusRail.Set("cases", s.cases.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("completed", s.completedSpecimenIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                if (s.cases.Count == 0)
                {
                    _detailText.Text = "No forensic autopsy cases registered.\nPost-mortem examinations and pathology findings will appear here when specimens are brought to the medical bay.\n\nLast Event: " + (string.IsNullOrEmpty(_host.LastEvent) ? "None recorded" : _host.LastEvent);
                }
                else
                {
                    string text = $"Forensic Autopsy Reports ({s.cases.Count} total):\n";
                    foreach (var c in s.cases)
                    {
                        text += $"  • [{c.status}] Specimen {c.specimenId} ({c.procedureId}) — Medic: {c.assignedMedicId} | Finding: {(string.IsNullOrEmpty(c.finding) ? "EXAMINATION IN PROGRESS" : c.finding)}\n";
                    }
                    text += $"\nLast Event: {_host.LastEvent}";
                    _detailText.Text = text;
                }
            }
        }

        public override void _ExitTree()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
