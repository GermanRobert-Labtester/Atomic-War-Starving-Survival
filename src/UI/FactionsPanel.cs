using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Factions panel.
    /// Shows faction relationships, trade stances, diplomatic status, and faction events.
    /// </summary>
    public partial class FactionsPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblFactionsTitle;
        private VBoxContainer _factionsList;
        private Label _lblRelationsTitle;
        private VBoxContainer _relationsList;
        private Label _lblEventsTitle;
        private VBoxContainer _factionEvents;

        // Placeholder faction data
        private readonly string[] _placeholderFactions = {
            "The Black Flotilla — Maritime traders, neutral stance",
            "The Ashen Hand — Scavengers, hostile but tradeable",
            "The Ledger Keepers — Archivists, neutral, value knowledge",
            "The Iron Covenant — Military survivors, wary of outsiders",
            "The Green Thread — Environmentalists, cautious allies"
        };

        private readonly string[] _placeholderRelations = {
            "Black Flotilla: Trade relations (45/100) — Willing to barter",
            "Ashen Hand: Hostile (15/100) — Avoid direct contact",
            "Ledger Keepers: Neutral (60/100) — Exchange information",
            "Iron Covenant: Wary (35/100) — Military presence noted",
            "Green Thread: Cautious (50/100) — Shared environmental concerns"
        };

        private readonly string[] _placeholderEvents = {
            "[Day 10] Black Flotilla offered trade route to Sector 12",
            "[Day 8] Ashen Hand raid detected near perimeter",
            "[Day 5] Ledger Keepers sent emissary with knowledge exchange proposal",
            "[Day 3] Iron Covenant increased patrols in northern sectors",
            "[Day 1] Green Thread requested mutual defense agreement"
        };

        // Real data from host session
        // private FactionsHostSession? _factionsHost;

        public void Bind(object factions) // placeholder for FactionsHostSession
        {
            // _factionsHost = (FactionsHostSession)factions;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_factionsList == null || _relationsList == null || _factionEvents == null) return;

            // Clear existing lists
            while (_factionsList.GetChildCount() > 0)
                _factionsList.RemoveChild(_factionsList.GetChild(0));
            while (_relationsList.GetChildCount() > 0)
                _relationsList.RemoveChild(_relationsList.GetChild(0));
            while (_factionEvents.GetChildCount() > 0)
                _factionEvents.RemoveChild(_factionEvents.GetChild(0));

            // Display placeholder factions
            foreach (string faction in _placeholderFactions)
            {
                var label = new Label { Text = faction };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _factionsList.AddChild(label);
            }

            // Display placeholder relations
            foreach (string relation in _placeholderRelations)
            {
                var label = new Label { Text = relation };
                label.CustomMinimumSize = new Vector2(400, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _relationsList.AddChild(label);
            }

            // Display placeholder faction events
            foreach (string factionEvent in _placeholderEvents)
            {
                var label = new Label { Text = factionEvent };
                label.CustomMinimumSize = new Vector2(400, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _factionEvents.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("FACTIONS & DIPLOMACY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Factions section
            _lblFactionsTitle = AshfallUiHelpers.MakeSectionHeader("KNOWN FACTIONS");
            vbox.AddChild(_lblFactionsTitle);

            _factionsList = new VBoxContainer();
            _factionsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _factionsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_factionsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Relations section
            _lblRelationsTitle = AshfallUiHelpers.MakeSectionHeader("RELATIONSHIPS");
            vbox.AddChild(_lblRelationsTitle);

            _relationsList = new VBoxContainer();
            _relationsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _relationsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_relationsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Faction events section
            _lblEventsTitle = AshfallUiHelpers.MakeSectionHeader("FACTION EVENTS");
            vbox.AddChild(_lblEventsTitle);

            _factionEvents = new VBoxContainer();
            _factionEvents.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _factionEvents.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_factionEvents);

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
