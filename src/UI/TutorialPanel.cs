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
        // the sidebar-driven navigation. Canonical InputMap actions are defined
        // in AshfallInputActions.cs and reconciled at runtime.
        private static (string key, string action)[] GetCurrentControls() => new[]
        {
            ("[Click Sidebar]", "Open shelter systems — Survivors, Inventory, Medical, Map, etc."),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Close), "Close current panel or return to main menu"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Journal), "Open the Journal / Field Manual / Codex"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Events), "Open the Events log"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Forecast), "Open the Weather forecast"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.WeatherHistory), "Open the Weather history"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Expeditions), "Open the Expeditions management panel"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Holdfast), "Open the Holdfast trade terminal"),
            ("[1-5]", "Switch Journal tabs (while Journal is open)"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Confirm), "Confirm daily briefing / advance day"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Help), "Toggle this Tutorial & Help overlay")
        };

        // Honest survival basics — aligned with the systems that actually exist.
        private static readonly string[] RealBasics =
        {
            "Needs — Hunger, Thirst, Fatigue, Warmth, and Morale decay continuously; when any need hits critical (90+), Health suffers irreversible decay.",
            "Radiation & Acute Sickness — Dose accumulates from outdoor fallout and storms; above 50 mSv survivors suffer Acute Sickness (-5 HP/hr decay). Administer Rad-Away or Iodine in Medical immediately.",
            "Water & Rationing — 3 survivors consume ~3.6 clean water units daily. Review inventory runway before advancing days.",
            "Power & Grid — Air and water filtration consume watts; a power brownout stops filtration and raises indoor radiation.",
            "Duty Roster — Assign survivors to shifts (Kitchen, Water, Maintenance, Guard) so needs decay is managed while you scavenge.",
            "Expeditions — Send survivors to scavenge rare resources; verify loadout, gas masks, fuel, and radiation readiness before departure.",
            "Weather — Fallout storms and black rain add severe outdoor radiation modifiers; keep survivors indoors during hazard alerts."
        };

        // Honest tips — no fabricated item behaviour.
        private static readonly string[] RealTips =
        {
            "Mikhail starts with Acute Radiation on Day 1 — administer Rad-Away in the Medical panel to prevent death within 16 hours.",
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
            foreach (var (key, action) in GetCurrentControls())
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
