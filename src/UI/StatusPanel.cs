using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Status panel.
    /// Shows overall game status, day counter, current objectives, and quick stats
    /// using tactile 9-slice card framing and status badge icons.
    /// </summary>
    public partial class StatusPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _dayInfo = null!;
        private VBoxContainer _objectivesList = null!;
        private VBoxContainer _statsData = null!;
        private VBoxContainer _statusData = null!;

        private readonly (string label, string val, Color col)[] _placeholderDayInfo = {
            ("Current Day", "Day 25", new Color(0.96f, 0.78f, 0.46f)),
            ("Total Days Survived", "25 Days", new Color(0.96f, 0.78f, 0.46f)),
            ("Season Cycle", "Nuclear Winter (Cold Count active)", new Color(0.43f, 0.64f, 0.66f)),
            ("External Reading", "-5°C · Ash Storm warning", new Color(0.58f, 0.56f, 0.52f)),
            ("Radio Frequency", "142.850 MHz [Carrier Stable]", new Color(0.43f, 0.64f, 0.66f)),
            ("Next Scheduled Shift", "Day 26 // Dawn Ration Triage", new Color(0.83f, 0.67f, 0.38f))
        };

        private readonly (string type, string text)[] _placeholderObjectives = {
            ("PRIMARY", "Maintain shelter filtration and prevent osteophage escalation"),
            ("SECONDARY", "Scavenge desalination membrane cartridges from Unit 4"),
            ("TERTIARY", "Arbitrate crossing fee dispute with Coastal Hydro-Barons"),
            ("DAILY", "Log dosimeter levels and reconcile water rations"),
            ("STANDING", "Keep roster casualty count below critical threshold")
        };

        private readonly (string label, string val, string badge)[] _placeholderStats = {
            ("Survivor Cohort", "5 / 20 Registered", "badge_exhaustion"),
            ("Water Stores", "30 units (10 days)", "item_desal_membrane"),
            ("Food Stores", "45 units (15 days)", "item_brine_salt"),
            ("Dosimetry Dose", "12.4 mSv (Low Risk)", "item_dosimeter_pen"),
            ("Bunker Morale", "75 / 100 [Stable]", "badge_guilt_insomnia")
        };

        private readonly (string system, string status, Color col)[] _placeholderStatus = {
            ("Shelter Air Filtration", "HEPA Pleats 78% · Spares: 03", new Color(0.43f, 0.64f, 0.66f)),
            ("Radiation Shielding", "Lead Plating Active (65% reduction)", new Color(0.83f, 0.67f, 0.38f)),
            ("Hatch Perimeter", "Sealed & Barricaded against fallout", new Color(0.9f, 0.9f, 0.9f)),
            ("Emergency Power", "85% Battery + Fuel Reserves", new Color(0.96f, 0.78f, 0.46f)),
            ("Medical Triage", "Potassium Iodide stock holding", new Color(0.43f, 0.64f, 0.66f))
        };

        public void Bind(object gameStatus)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_dayInfo == null || _objectivesList == null || _statsData == null || _statusData == null) return;

            while (_dayInfo.GetChildCount() > 0) _dayInfo.RemoveChild(_dayInfo.GetChild(0));
            while (_objectivesList.GetChildCount() > 0) _objectivesList.RemoveChild(_objectivesList.GetChild(0));
            while (_statsData.GetChildCount() > 0) _statsData.RemoveChild(_statsData.GetChild(0));
            while (_statusData.GetChildCount() > 0) _statusData.RemoveChild(_statusData.GetChild(0));

            foreach (var item in _placeholderDayInfo)
            {
                var row = AshfallUiHelpers.MakeDataRow(item.label, item.val, item.col);
                _dayInfo.AddChild(row);
            }

            foreach (var obj in _placeholderObjectives)
            {
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var tag = AshfallUiHelpers.MakeSmall($"[{obj.type}]");
                tag.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                tag.CustomMinimumSize = new Vector2(90, 0);
                row.AddChild(tag);

                var desc = AshfallUiHelpers.MakeSmall(obj.text, true);
                desc.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(desc);
                _objectivesList.AddChild(row);
            }

            foreach (var stat in _placeholderStats)
            {
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon(stat.badge, 22);
                row.AddChild(icon);

                var lbl = AshfallUiHelpers.MakeSmall(stat.label);
                lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(lbl);

                var val = AshfallUiHelpers.MakeMono(stat.val);
                val.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                row.AddChild(val);

                _statsData.AddChild(row);
            }

            foreach (var stat in _placeholderStatus)
            {
                var row = AshfallUiHelpers.MakeDataRow(stat.system, stat.status, stat.col);
                _statusData.AddChild(row);
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(680, 560);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("HOLDFAST STATUS & OPERATIONS", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(640, 440),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            var contentBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("DAY & ENVIRONMENT"));
            _dayInfo = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_dayInfo);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("STANDING DIRECTIVES"));
            _objectivesList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_objectivesList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("COHORT & SUPPLY TELEMETRY"));
            _statsData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_statsData);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SHELTER SUBSYSTEM HEALTH"));
            _statusData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_statusData);

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
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
