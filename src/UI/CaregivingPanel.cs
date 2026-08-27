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
    public partial class CaregivingPanel : Control, IBindablePanel
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

            _detailText = AshfallUiHelpers.MakeBody("", autowrap: true);
            _contentStack.AddChild(_detailText);

            var buttonRow = AshfallUiHelpers.MakeActionBar(separation: 10);

            _assignBtn = AshfallUiHelpers.MakeButton("Demo Assign (caregiver_a → patient_b)", () =>
            {
                if (_host != null)
                    _host.AssignCaregiver("caregiver_a", "patient_b");
            });
            _assignBtn.CustomMinimumSize = new Vector2(260, 36);
            buttonRow.AddChild(_assignBtn);

            _unassignBtn = AshfallUiHelpers.MakeButton("Unassign patient_b", () =>
            {
                if (_host != null)
                    _host.UnassignCaregiver("patient_b");
            });
            _unassignBtn.CustomMinimumSize = new Vector2(160, 36);
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
            if (_host == null || _statusRail == null)
            {
                if (_detailText != null)
                {
                    _detailText.Text = "Caregiving host session is not bound. Bedside tending assignments and bond records are offline.";
                }
                if (_assignBtn != null) _assignBtn.Disabled = true;
                if (_unassignBtn != null) _unassignBtn.Disabled = true;
                return;
            }

            var s = _host.System.CaptureState();
            int active = _host.ActiveAssignmentCount;
            _statusRail.Set("assignments", active.ToString(), active > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("bonds", s.Assignments.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("capacity", active.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string text = active > 0
                    ? $"Active Caregiving Pairs: {active}\n"
                    : "No active caregiving assignments registered.\nPair healthy caregivers with recovering patients to accelerate bedside healing and build emotional bonds.\n";
                foreach (var a in s.Assignments)
                    text += $"  • {a.CaregiverId} → {a.PatientId} (bond {a.BondStrength:F2})\n";
                text += $"\nLast Event: " + (string.IsNullOrEmpty(_host.LastEvent) ? "None recorded" : _host.LastEvent);
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
