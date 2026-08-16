using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Medical History panel.
    /// Shows detailed medical history, treatment timeline, and medical outcomes.
    /// </summary>
    public partial class MedicalHistoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHistoryTitle;
        private VBoxContainer _medicalHistory;
        private Label _lblTreatmentTitle;
        private VBoxContainer _treatmentTimeline;
        private Label _lblOutcomesTitle;
        private VBoxContainer _medicalOutcomes;

        private readonly string[] _placeholderHistory = {
            "[Day 1] Initial assessment — Healthy",
            "[Day 5] Routine checkup — All clear",
            "[Day 10] Flu symptoms — Rest and fluids",
            "[Day 15] Mild radiation exposure — Treated with iodine",
            "[Day 18] Minor wound — Healed with bandages"
        };

        private readonly string[] _placeholderTreatment = {
            "Day 1-5: No treatment required",
            "Day 10: Rest, fluids, symptom management",
            "Day 15: Iodine pills, radiation monitoring",
            "Day 18: Bandages, wound care, pain management",
            "Day 25: Current — Radiation monitoring ongoing",
            "Total Treatments: 4 major treatments"
        };

        private readonly string[] _placeholderOutcomes = {
            "Day 10 Flu: Fully recovered in 3 days",
            "Day 15 Radiation: Managed, no long-term effects",
            "Day 18 Wound: Healed in 2 days, no infection",
            "Day 25 Current: Radiation stable at 12 mSv",
            "Overall Health Trend: Improving",
            "Next Checkup: Day 26 (Scheduled)"
        };

        public void Bind(object medicalHistory)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_medicalHistory == null || _treatmentTimeline == null || _medicalOutcomes == null) return;

            while (_medicalHistory.GetChildCount() > 0) _medicalHistory.RemoveChild(_medicalHistory.GetChild(0));
            while (_treatmentTimeline.GetChildCount() > 0) _treatmentTimeline.RemoveChild(_treatmentTimeline.GetChild(0));
            while (_medicalOutcomes.GetChildCount() > 0) _medicalOutcomes.RemoveChild(_medicalOutcomes.GetChild(0));

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _medicalHistory.AddChild(label);
            }

            foreach (string treatment in _placeholderTreatment)
            {
                var label = new Label { Text = treatment };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _treatmentTimeline.AddChild(label);
            }

            foreach (string outcome in _placeholderOutcomes)
            {
                var label = new Label { Text = outcome };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _medicalOutcomes.AddChild(label);
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("MEDICAL HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("MEDICAL HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _medicalHistory = new VBoxContainer();
            _medicalHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _medicalHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_medicalHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTreatmentTitle = AshfallUiHelpers.MakeSectionHeader("TREATMENT TIMELINE");
            vbox.AddChild(_lblTreatmentTitle);

            _treatmentTimeline = new VBoxContainer();
            _treatmentTimeline.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _treatmentTimeline.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_treatmentTimeline);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblOutcomesTitle = AshfallUiHelpers.MakeSectionHeader("MEDICAL OUTCOMES");
            vbox.AddChild(_lblOutcomesTitle);

            _medicalOutcomes = new VBoxContainer();
            _medicalOutcomes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _medicalOutcomes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_medicalOutcomes);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
        }

        public void Open()
        {
            Visible = true;
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
