using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using AtomicWar.GodotApp.UI;
using Ashfall.Core.UI;
using Ashfall.Core.Muster;

namespace AtomicWar.GodotApp.Muster
{
    /// <summary>
    /// Godot 4.7+ UI Control presenting the sector's currents (currents.json,
    /// now fifteen entries including faction_hydro_barons). Thin presentation
    /// only: renders CurrentDefinition list + MusterSystem escalation status.
    /// Zero simulation logic.
    /// </summary>
    public partial class CurrentsRosterWidget : PanelContainer
    {
        private List<CurrentDefinition> _roster;
        private MusterSystem _muster;
        private VBoxContainer _currentsList;
        private Label _lblEscalation;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(380, 260);

            // Apply standard panel 9-slice via shared helper (frame_9slice first)
            AddThemeStyleboxOverride("panel", AtomicWar.GodotApp.UI.AshfallUiHelpers.MakePanelFrameStyleBox());

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "THE SECTOR'S CURRENTS (15)",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(title);

            _lblEscalation = new Label { Text = "Escalation: dormant" };
            rootVbox.AddChild(_lblEscalation);

            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 200)
            };
            rootVbox.AddChild(scroll);

            _currentsList = new VBoxContainer();
            scroll.AddChild(_currentsList);
        }

        public void Bind(List<CurrentDefinition> roster, MusterSystem muster)
        {
            _roster = roster ?? new List<CurrentDefinition>();
            _muster = muster;
        }

        public void RefreshView()
        {
            if (_currentsList == null) return;
            foreach (Node child in _currentsList.GetChildren())
                child.QueueFree();

            if (_muster != null)
            {
                if (_muster.EscalationDay < 0)
                    _lblEscalation.Text = "Escalation: dormant";
                else if (_muster.MusterTriggered)
                    _lblEscalation.Text = $"Escalation: Day {_muster.EscalationDay} — THE MUSTER IS OPEN";
                else
                    _lblEscalation.Text = $"Escalation: Day {_muster.EscalationDay} (opens Day {MusterSystem.MusterOpeningDay})";
            }

            for (int i = 0; i < _roster.Count; i++)
            {
                var c = _roster[i];
                string state = c.isActive ? "active" : "dormant";
                var row = new Label
                {
                    Text = $"{c.displayName} — {state} · {c.alignment} · trust {c.trust:0}"
                };
                row.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                _currentsList.AddChild(row);
            }
        }
    }
}
