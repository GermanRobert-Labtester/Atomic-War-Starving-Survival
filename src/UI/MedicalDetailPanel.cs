using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Medical Detail panel.
    /// Shows detailed medical status, treatment plans, medication schedules, and medical history.
    /// </summary>
    public partial class MedicalDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblPatientTitle;
        private VBoxContainer _patientInfo;
        private Label _lblTreatmentTitle;
        private VBoxContainer _treatmentPlan;
        private Label _lblMedicationTitle;
        private VBoxContainer _medicationSchedule;
        private Label _lblHistoryTitle;
        private VBoxContainer _medicalHistory;

        private readonly string[] _placeholderPatientInfo = {
            "Patient: Elena (Leader)",
            "Age: 42",
            "Current Health: 85/100",
            "Radiation: 12 mSv (Low)",
            "Allergies: None known",
            "Blood Type: O+"
        };

        private readonly string[] _placeholderTreatmentPlan = {
            "Current Treatment: Radiation monitoring (Daily)",
            "Next Checkup: Day 26 (In 1 day)",
            "Physical Therapy: None required",
            "Dietary Plan: High protein, vitamin-rich",
            "Rest Requirements: 8 hours minimum",
            "Work Restrictions: None (Full duty)"
        };

        private readonly string[] _placeholderMedication = {
            "Iodine Pills: 1/day (Prophylactic)",
            "Multivitamins: 2/day (Maintenance)",
            "Painkillers: As needed (PRN)",
            "Antibiotics: None (No infection)",
            "Supplements: Iron, Vitamin D, Calcium",
            "Last Medication: Today 08:00"
        };

        private readonly string[] _placeholderMedicalHistory = {
            "[Day 18] Minor wound (right forearm) — Healed",
            "[Day 15] Mild radiation exposure — Treated with iodine",
            "[Day 10] Flu symptoms — Rest and fluids",
            "[Day 5] Routine checkup — All clear",
            "[Day 1] Initial assessment — Healthy"
        };

        public void Bind(object medical)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_patientInfo == null || _treatmentPlan == null || _medicationSchedule == null || _medicalHistory == null) return;

            AshfallUiHelpers.EmptyChildren(_patientInfo);
            AshfallUiHelpers.EmptyChildren(_treatmentPlan);
            AshfallUiHelpers.EmptyChildren(_medicationSchedule);
            AshfallUiHelpers.EmptyChildren(_medicalHistory);

            foreach (string info in _placeholderPatientInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _patientInfo.AddChild(label);
            }

            foreach (string treatment in _placeholderTreatmentPlan)
            {
                var label = new Label { Text = treatment };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _treatmentPlan.AddChild(label);
            }

            foreach (string med in _placeholderMedication)
            {
                var label = new Label { Text = med };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _medicationSchedule.AddChild(label);
            }

            foreach (string history in _placeholderMedicalHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _medicalHistory.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("MEDICAL DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblPatientTitle = AshfallUiHelpers.MakeSectionHeader("PATIENT INFORMATION");
            vbox.AddChild(_lblPatientTitle);

            _patientInfo = new VBoxContainer();
            _patientInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _patientInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_patientInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTreatmentTitle = AshfallUiHelpers.MakeSectionHeader("TREATMENT PLAN");
            vbox.AddChild(_lblTreatmentTitle);

            _treatmentPlan = new VBoxContainer();
            _treatmentPlan.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _treatmentPlan.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_treatmentPlan);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblMedicationTitle = AshfallUiHelpers.MakeSectionHeader("MEDICATION SCHEDULE");
            vbox.AddChild(_lblMedicationTitle);

            _medicationSchedule = new VBoxContainer();
            _medicationSchedule.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _medicationSchedule.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_medicationSchedule);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("MEDICAL HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _medicalHistory = new VBoxContainer();
            _medicalHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _medicalHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_medicalHistory);

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
