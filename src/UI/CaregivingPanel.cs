using System;
using Godot;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Caregiving & Bedside Tending Management Interface.
    /// Thin presentation panel displaying active caregiver-to-patient pairings, bond strengths,
    /// and tending capacities.
    ///
    /// Presentation only — all caregiving mechanics, recovery boosts, and bond progressions
    /// are evaluated authoritatively in <see cref="Ashfall.Core.Survivors.CaregivingSystem"/>
    /// via <see cref="CaregivingHostSession"/>.
    /// </summary>
    public partial class CaregivingPanel : Control
    {
        /// <summary>Raised when the panel is dismissed by the player.</summary>
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _assignBtn = null!;
        private Button _unassignBtn = null!;

        private CaregivingHostSession? _host;

        /// <summary>Indicates whether the panel is currently wired to a live host session.</summary>
        public bool IsBound => _host != null;

        /// <summary>
        /// Binds this panel to the host session and subscribes to state change events.
        /// </summary>
        /// <param name="session">The active caregiving host session.</param>
        public void Bind(CaregivingHostSession session)
        {
            _host = session;
            if (_host != null)
                _host.StateChanged += RefreshView;
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Caregiving // Bedside Tending", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("assignments", "Active Pairs", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("bonds", "Bonds", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("capacity", "Patients Tended", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _assignBtn = new Button { Text = "Demo Assign (caregiver_a → patient_b)", CustomMinimumSize = new Vector2(260, 36) };
            _assignBtn.Pressed += () =>
            {
                if (_host != null)
                    _host.AssignCaregiver("caregiver_a", "patient_b");
            };
            buttonRow.AddChild(_assignBtn);

            _unassignBtn = new Button { Text = "Unassign patient_b", CustomMinimumSize = new Vector2(160, 36) };
            _unassignBtn.Pressed += () =>
            {
                if (_host != null)
                    _host.UnassignCaregiver("patient_b");
            };
            buttonRow.AddChild(_unassignBtn);

            _contentStack.AddChild(buttonRow);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        /// <summary>
        /// Renders active metrics, assignment pairs, and bond strengths from the
        /// authoritative <see cref="Ashfall.Core.Survivors.CaregivingSystem"/> state snapshot.
        /// </summary>
        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var s = _host.System.CaptureState();
            int active = _host.ActiveAssignmentCount;
            _statusRail.Set("assignments", active.ToString(), active > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("bonds", s.Assignments.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("capacity", active.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string text = $"Active Caregiving Pairs: {active}\n";
                foreach (var a in s.Assignments)
                    text += $"  • {a.CaregiverId} → {a.PatientId} (bond {a.BondStrength:F2})\n";
                if (s.Assignments.Count == 0)
                    text += "  (no active assignments)\n";
                text += $"\nLast Event: {_host.LastEvent}";
                _detailText.Text = text;
            }
        }

        public override void _ExitTree()
        {
            if (_host != null)
                _host.StateChanged -= RefreshView;
            base._ExitTree();
        }
    }
}
