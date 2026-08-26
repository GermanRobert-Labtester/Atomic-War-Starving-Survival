using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Tutorial / Help panel.
    /// Shows the real control scheme and honest survival basics. Controls are
    /// curated to match the actual key handlers in Main.Application.cs and the
    /// sidebar-driven panel navigation — no fabricated bindings (e.g. no WASD
    /// camera, no F5 quick-save; those do not exist in ASHFALL).
    /// </summary>
    public partial class TutorialPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblControlsTitle;
        private VBoxContainer _controlsList;
        private Label _lblBasicsTitle;
        private VBoxContainer _basicsList;
        private Label _lblTipsTitle;
        private VBoxContainer _tipsList;

        private int _simDay = 1;

        /// <summary>True once Bind has been called with a real day.</summary>
        public bool IsBound { get; private set; }

        /// <summary>Number of control rows rendered (probe for ui-tests).</summary>
        public int RenderedControlsCount { get; private set; }

        // Real controls — curated to match Main.Application.cs key handlers +
        // the sidebar-driven navigation. No InputMap actions exist in this
        // project, so the list is honest and hand-maintained against the code.
        private static readonly (string key, string action)[] RealControls =
        {
            ("[Click Sidebar]", "Open shelter systems — Survivors, Inventory, Medical, Map, etc."),
            ("[Esc]", "Close the current panel or return to menu"),
            ("[J]", "Open the Journal / Codex"),
            ("[E]", "Open the Events log"),
            ("[F]", "Open the Weather forecast"),
            ("[H]", "Open the Weather history"),
            ("[1-5]", "Switch Journal tabs (while the Journal is open)"),
            ("[Enter] / [Space]", "Confirm the daily briefing"),
            ("[F1]", "Toggle the developer console")
        };

        // Honest survival basics — aligned with the systems that actually exist.
        private static readonly string[] RealBasics =
        {
            "Needs — Hunger, Thirst, Fatigue, Warmth, and Morale decay every hour; let any hit critical and Health suffers.",
            "Radiation — Dose accumulates from fallout zones and storms; above 50 mSv survivors risk acute sickness. Iodine grants temporary resistance.",
            "Shelter — The bunker's ceiling material attenuates outdoor radiation; upgrade the weakest room first.",
            "Power — The grid burns fuel to generate watts; a brownout disables systems. Watch the battery reserve.",
            "Duty Roster — Assign survivors to shifts so needs decay is managed while you scavenge.",
            "Expeditions — Send survivors to locations for rare resources; they travel, scavenge, and return.",
            "Weather — Fallout storms and black rain add outdoor radiation modifiers; keep survivors indoors during hazards."
        };

        // Honest tips — no fabricated item behaviour.
        private static readonly string[] RealTips =
        {
            "Keep iodine pills in stock — they grant hours of radiation resistance when a storm hits.",
            "Clean water is scarcer than food — prioritize the water filter and desalination membranes.",
            "A gas mask cuts outdoor dose; a hazmat suit cuts it further, but both degrade with use.",
            "Watch the Dose Ledger — cumulative exposure causes chronic illness, not just acute sickness.",
            "Low morale reduces work efficiency; the Vinyl morale system and caregiving can recover it.",
            "End the day deliberately via the day-advance flow — it ticks every subsystem exactly once.",
            "Save before risky expeditions; Continue resumes from the last day-advance save."
        };

        public void Bind(int simDay = 1)
        {
            _simDay = simDay;
            IsBound = true;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_controlsList == null || _basicsList == null || _tipsList == null) return;

            AshfallUiHelpers.EmptyChildren(_controlsList);
            AshfallUiHelpers.EmptyChildren(_basicsList);
            AshfallUiHelpers.EmptyChildren(_tipsList);

            RenderedControlsCount = 0;
            foreach (var (key, action) in RealControls)
            {
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var k = AshfallUiHelpers.MakeMono(key);
                k.CustomMinimumSize = new Vector2(150, 0);
                k.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                row.AddChild(k);

                var desc = AshfallUiHelpers.MakeSmall(action, true);
                desc.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(desc);
                _controlsList.AddChild(row);
                RenderedControlsCount++;
            }

            foreach (string basic in RealBasics)
            {
                var label = new Label { Text = basic };
                label.CustomMinimumSize = new Vector2(400, 0);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _basicsList.AddChild(label);
            }

            foreach (string tip in RealTips)
            {
                var label = new Label { Text = tip };
                label.CustomMinimumSize = new Vector2(400, 0);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _tipsList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("TUTORIAL & HELP", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Controls section
            _lblControlsTitle = AshfallUiHelpers.MakeSectionHeader("CONTROLS");
            vbox.AddChild(_lblControlsTitle);

            _controlsList = new VBoxContainer();
            _controlsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _controlsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_controlsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Basics section
            _lblBasicsTitle = AshfallUiHelpers.MakeSectionHeader("SURVIVAL BASICS");
            vbox.AddChild(_lblBasicsTitle);

            _basicsList = new VBoxContainer();
            _basicsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _basicsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_basicsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Tips section
            _lblTipsTitle = AshfallUiHelpers.MakeSectionHeader("SURVIVAL TIPS");
            vbox.AddChild(_lblTipsTitle);

            _tipsList = new VBoxContainer();
            _tipsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tipsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_tipsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc/F1]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && (key.Keycode == Key.Escape || key.Keycode == Key.F1))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
