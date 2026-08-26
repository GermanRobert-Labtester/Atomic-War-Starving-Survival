using Godot;
using AtomicWar.GodotApp.UI;
using Ashfall.Core.UI;
using Ashfall.Core.Muster;

#pragma warning disable CS8618
namespace AtomicWar.GodotApp.Muster
{
    /// <summary>
    /// Section VI.2 holding-ground status panel: members rallied, chosen
    /// campaign strategy, Garrison lockout risk. Thin presentation of
    /// CoalitionCampSystem only — zero simulation logic.
    /// </summary>
    public partial class DeserterCoalitionCampWidget : PanelContainer
    {
        private CoalitionCampSystem _camp;
        private Label _lblStatus;
        private Label _lblMembers;
        private Label _lblStrategy;
        private ProgressBar _pbLockout;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(380, 150);

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "DESERTER COALITION — HOLDING GROUND",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(title);

            _lblStatus = new Label { Text = "Not formed." };
            rootVbox.AddChild(_lblStatus);

            _lblMembers = new Label { Text = "Members rallied: 0" };
            rootVbox.AddChild(_lblMembers);

            _lblStrategy = new Label { Text = "Strategy: none" };
            rootVbox.AddChild(_lblStrategy);

            _pbLockout = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 100,
                Value = 0,
                CustomMinimumSize = new Vector2(0, 14)
            };
            rootVbox.AddChild(_pbLockout);
        }

        public void Bind(CoalitionCampSystem camp)
        {
            _camp = camp;
        }

        public void RefreshView()
        {
            if (_camp == null || _pbLockout == null) return;
            var s = _camp.State;
            _lblStatus.Text = s.formed
                ? $"Formed Day {s.formedDay} @ {s.holdingGroundId} · Vask {(s.vaskWithCamp ? "with the camp" : "gone")}"
                : "Not formed (Muster opens Day 260).";
            _lblMembers.Text = $"Members rallied: {s.membersRallied}";
            _lblStrategy.Text = string.IsNullOrEmpty(s.chosenStrategy)
                ? "Strategy: none chosen"
                : $"Strategy: {s.chosenStrategy} — lockout risk {s.garrisonLockoutRisk}%";
            _pbLockout.Value = s.garrisonLockoutRisk;
        }
    }
}
