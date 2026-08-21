using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Afflictions panel.
    /// Shows current afflictions, chronic conditions, and medical treatments.
    /// </summary>
    public partial class AfflictionsPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblActiveTitle;
        private VBoxContainer _activeList;
        private Label _lblChronicTitle;
        private VBoxContainer _chronicList;
        private Label _lblTreatmentTitle;
        private VBoxContainer _treatmentList;

        // Placeholder affliction data
        private readonly string[] _placeholderActive = {
            "Radiation Sickness — Mild (Day 12-15) — Recovering",
            "Minor Wound — Right forearm (Day 18) — Healed",
            "Mild Infection — Left leg (Day 20) — Treating with antibiotics"
        };

        private readonly string[] _placeholderChronic = {
            "Chronic Radiation Exposure — Low level (Day 1-25) — Monitoring",
            "Respiratory Degeneration — Mild (Day 15-25) — Managing with inhalers",
            "Psychological Trauma — Moderate (Day 5-25) — Counseling sessions"
        };

        private readonly string[] _placeholderTreatment = {
            "Iodine Pills — Reduce radiation exposure by 20%",
            "Antibiotics — Treat bacterial infections",
            "Inhalers — Manage respiratory symptoms",
            "Counseling — Address psychological trauma",
            "Rest — Allow natural healing and recovery"
        };

        // Real data from host session
        // private MedicalHostSession? _medicalHost;

        public void Bind(object medical) // placeholder for MedicalHostSession
        {
            // _medicalHost = (MedicalHostSession)medical;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_activeList == null || _chronicList == null || _treatmentList == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_activeList);
            AshfallUiHelpers.EmptyChildren(_chronicList);
            AshfallUiHelpers.EmptyChildren(_treatmentList);

            // Display placeholder active afflictions
            foreach (string affliction in _placeholderActive)
            {
                var label = new Label { Text = affliction };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _activeList.AddChild(label);
            }

            // Display placeholder chronic conditions
            foreach (string condition in _placeholderChronic)
            {
                var label = new Label { Text = condition };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy));
                _chronicList.AddChild(label);
            }

            // Display placeholder treatments
            foreach (string treatment in _placeholderTreatment)
            {
                var label = new Label { Text = treatment };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _treatmentList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("AFFLICTIONS & TREATMENTS", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Active afflictions section
            _lblActiveTitle = AshfallUiHelpers.MakeSectionHeader("ACTIVE AFFLICTIONS");
            vbox.AddChild(_lblActiveTitle);

            _activeList = new VBoxContainer();
            _activeList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _activeList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_activeList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Chronic conditions section
            _lblChronicTitle = AshfallUiHelpers.MakeSectionHeader("CHRONIC CONDITIONS");
            vbox.AddChild(_lblChronicTitle);

            _chronicList = new VBoxContainer();
            _chronicList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _chronicList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_chronicList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Treatments section
            _lblTreatmentTitle = AshfallUiHelpers.MakeSectionHeader("TREATMENTS & MEDICATIONS");
            vbox.AddChild(_lblTreatmentTitle);

            _treatmentList = new VBoxContainer();
            _treatmentList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _treatmentList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_treatmentList);

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
