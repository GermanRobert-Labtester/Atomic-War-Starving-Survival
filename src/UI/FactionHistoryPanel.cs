using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Faction History panel.
    /// Shows detailed faction history, relationship evolution, and diplomatic events timeline.
    /// </summary>
    public partial class FactionHistoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHistoryTitle;
        private VBoxContainer _factionHistory;
        private Label _lblRelationsTitle;
        private VBoxContainer _relationEvolution;
        private Label _lblDiplomacyTitle;
        private VBoxContainer _diplomaticEvents;

        private readonly string[] _placeholderHistory = {
            "[Day 10] First contact with Black Flotilla — Neutral stance",
            "[Day 15] Trade negotiation initiated — Willing to barter",
            "[Day 18] Information exchange completed — Relationship improved",
            "[Day 20] Trade dispute resolved — No penalties",
            "[Day 24] Trade offer received — 5 food for 2 medicine"
        };

        private readonly string[] _placeholderRelations = {
            "Day 10: Trade Relations 20/100 (Initial contact)",
            "Day 15: Trade Relations 35/100 (Negotiation started)",
            "Day 18: Trade Relations 50/100 (Information exchange)",
            "Day 20: Trade Relations 55/100 (Dispute resolved)",
            "Day 24: Trade Relations 45/100 (Trade offer received)",
            "Trend: Fluctuating but generally improving"
        };

        private readonly string[] _placeholderDiplomacy = {
            "[Day 10] Diplomatic envoy exchanged — First contact",
            "[Day 15] Trade proposal submitted — Willing to negotiate",
            "[Day 18] Knowledge exchange completed — Mutual benefit",
            "[Day 20] Trade dispute mediation — Fair resolution",
            "[Day 24] Trade offer received — Active negotiation",
            "Next: Respond to trade offer by Day 30"
        };

        public void Bind(object factionHistory)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_factionHistory == null || _relationEvolution == null || _diplomaticEvents == null) return;

            AshfallUiHelpers.EmptyChildren(_factionHistory);
            AshfallUiHelpers.EmptyChildren(_relationEvolution);
            AshfallUiHelpers.EmptyChildren(_diplomaticEvents);

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _factionHistory.AddChild(label);
            }

            foreach (string relation in _placeholderRelations)
            {
                var label = new Label { Text = relation };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _relationEvolution.AddChild(label);
            }

            foreach (string diplomacy in _placeholderDiplomacy)
            {
                var label = new Label { Text = diplomacy };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _diplomaticEvents.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("FACTION HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("FACTION HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _factionHistory = new VBoxContainer();
            _factionHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _factionHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_factionHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblRelationsTitle = AshfallUiHelpers.MakeSectionHeader("RELATIONSHIP EVOLUTION");
            vbox.AddChild(_lblRelationsTitle);

            _relationEvolution = new VBoxContainer();
            _relationEvolution.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _relationEvolution.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_relationEvolution);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblDiplomacyTitle = AshfallUiHelpers.MakeSectionHeader("DIPLOMATIC EVENTS");
            vbox.AddChild(_lblDiplomacyTitle);

            _diplomaticEvents = new VBoxContainer();
            _diplomaticEvents.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _diplomaticEvents.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_diplomaticEvents);

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
