using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Combat panel.
    /// Shows combat encounters, battle logs, casualties, and combat outcomes.
    /// </summary>
    public partial class CombatPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblEncountersTitle;
        private VBoxContainer _encounterList;
        private Label _lblBattleLogTitle;
        private VBoxContainer _battleLog;
        private Label _lblCasualtiesTitle;
        private VBoxContainer _casualtyList;

        // Placeholder combat data
        private readonly string[] _placeholderEncounters = {
            "Encounter 1: Raid on Supply Caravan (Day 7) — Victory, 2 casualties",
            "Encounter 2: Ambush in Sector 4 (Day 12) — Retreat, 1 casualty",
            "Encounter 3: Defense of Bunker (Day 18) — Victory, 0 casualties",
            "Encounter 4: Skirmish at Radio Tower (Day 22) — Inconclusive, 3 casualties"
        };

        private readonly string[] _placeholderBattleLog = {
            "[Day 7] Raid on Supply Caravan — Our forces repelled attackers. Lost 2 scouts.",
            "[Day 12] Ambush in Sector 4 — Forced retreat. 1 medic wounded.",
            "[Day 18] Bunker Defense — Repelled raiders. No casualties. Captured supplies.",
            "[Day 22] Radio Tower Skirmish — Failed to secure tower. 2 injured, 1 captured."
        };

        private readonly string[] _placeholderCasualties = {
            "Yuki — Wounded (Day 12) — Leg injury, 3 days recovery",
            "Marcus — Wounded (Day 22) — Minor burns, 1 day recovery",
            "Unknown Survivor — Captured (Day 22) — Status unknown",
            "2 Scouts — Killed (Day 7) — Buried at perimeter"
        };

        // Real data from host session
        // private CombatHostSession? _combatHost;

        public void Bind(object combat) // placeholder for CombatHostSession
        {
            // _combatHost = (CombatHostSession)combat;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_encounterList == null || _battleLog == null || _casualtyList == null) return;

            // Clear existing lists
            while (_encounterList.GetChildCount() > 0)
                _encounterList.RemoveChild(_encounterList.GetChild(0));
            while (_battleLog.GetChildCount() > 0)
                _battleLog.RemoveChild(_battleLog.GetChild(0));
            while (_casualtyList.GetChildCount() > 0)
                _casualtyList.RemoveChild(_casualtyList.GetChild(0));

            // Display placeholder encounters
            foreach (string encounter in _placeholderEncounters)
            {
                var label = new Label { Text = encounter };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _encounterList.AddChild(label);
            }

            // Display placeholder battle log
            foreach (string log in _placeholderBattleLog)
            {
                var label = new Label { Text = log };
                label.CustomMinimumSize = new Vector2(400, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _battleLog.AddChild(label);
            }

            // Display placeholder casualties
            foreach (string casualty in _placeholderCasualties)
            {
                var label = new Label { Text = casualty };
                label.CustomMinimumSize = new Vector2(400, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _casualtyList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("COMBAT & ENCOUNTERS", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Encounters section
            _lblEncountersTitle = AshfallUiHelpers.MakeSectionHeader("ENCOUNTERS");
            vbox.AddChild(_lblEncountersTitle);

            _encounterList = new VBoxContainer();
            _encounterList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _encounterList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_encounterList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Battle log section
            _lblBattleLogTitle = AshfallUiHelpers.MakeSectionHeader("BATTLE LOG");
            vbox.AddChild(_lblBattleLogTitle);

            _battleLog = new VBoxContainer();
            _battleLog.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _battleLog.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_battleLog);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Casualties section
            _lblCasualtiesTitle = AshfallUiHelpers.MakeSectionHeader("CASUALTIES & LOSSES");
            vbox.AddChild(_lblCasualtiesTitle);

            _casualtyList = new VBoxContainer();
            _casualtyList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _casualtyList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_casualtyList);

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
