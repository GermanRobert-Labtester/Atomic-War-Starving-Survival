using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Medical panel.
    /// Shows health status, radiation levels, treatments, and medical supplies.
    /// </summary>
    public partial class MedicalPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHealthTitle;
        private VBoxContainer _healthStats;
        private Label _lblTreatmentsTitle;
        private VBoxContainer _treatmentList;
        private Label _lblSuppliesTitle;
        private VBoxContainer _supplyList;

        // Placeholder medical data
        private readonly string[] _placeholderHealthStats = {
            "Health: 85/100",
            "Radiation: 12 mSv (Low)",
            "Hydration: Normal",
            "Nutrition: Adequate",
            "Infections: None"
        };

        private readonly string[] _placeholderTreatments = {
            "Iodine Pills (Rad reduction)",
            "Bandages (Wound treatment)",
            "Antibiotics (Infection control)",
            "Anti-rad (Chronic radiation)",
            "Painkillers (Pain management)"
        };

        private readonly string[] _placeholderSupplies = {
            "Iodine Pills: 24 units",
            "Bandages: 8 units",
            "Antibiotics: 3 units",
            "Anti-rad: 1 unit",
            "Painkillers: 12 units"
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
            if (_healthStats == null || _treatmentList == null || _supplyList == null) return;

            // Clear existing items
            while (_healthStats.GetChildCount() > 0)
                _healthStats.RemoveChild(_healthStats.GetChild(0));
            while (_treatmentList.GetChildCount() > 0)
                _treatmentList.RemoveChild(_treatmentList.GetChild(0));
            while (_supplyList.GetChildCount() > 0)
                _supplyList.RemoveChild(_supplyList.GetChild(0));

            // Display placeholder data
            foreach (string stat in _placeholderHealthStats)
            {
                var label = new Label { Text = stat };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _healthStats.AddChild(label);
            }

            foreach (string treatment in _placeholderTreatments)
            {
                var label = new Label { Text = treatment };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _treatmentList.AddChild(label);
            }

            foreach (string supply in _placeholderSupplies)
            {
                var label = new Label { Text = supply };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _supplyList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("MEDICAL STATUS", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Health stats section
            _lblHealthTitle = AshfallUiHelpers.MakeSectionHeader("HEALTH STATUS");
            vbox.AddChild(_lblHealthTitle);

            _healthStats = new VBoxContainer();
            _healthStats.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _healthStats.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_healthStats);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Treatments section
            _lblTreatmentsTitle = AshfallUiHelpers.MakeSectionHeader("AVAILABLE TREATMENTS");
            vbox.AddChild(_lblTreatmentsTitle);

            _treatmentList = new VBoxContainer();
            _treatmentList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _treatmentList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_treatmentList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Supplies section
            _lblSuppliesTitle = AshfallUiHelpers.MakeSectionHeader("MEDICAL SUPPLIES");
            vbox.AddChild(_lblSuppliesTitle);

            _supplyList = new VBoxContainer();
            _supplyList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _supplyList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_supplyList);

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
