using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Faction Detail panel.
    /// Shows detailed faction information, relationship status, trade offers, and diplomatic events.
    /// </summary>
    public partial class FactionDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblFactionInfoTitle;
        private VBoxContainer _factionInfo;
        private Label _lblRelationsTitle;
        private VBoxContainer _relationsData;
        private Label _lblTradeTitle;
        private VBoxContainer _tradeOffers;
        private Label _lblEventsTitle;
        private VBoxContainer _factionEvents;

        private readonly string[] _placeholderFactionInfo = {
            "Faction: The Black Flotilla",
            "Type: Maritime Traders",
            "Stance: Neutral",
            "Members: 45 survivors",
            "Base: Floating vessel (Sector 9)",
            "Leadership: Captain Zara"
        };

        private readonly string[] _placeholderRelations = {
            "Trade Relations: 45/100 (Willing to barter)",
            "Military Alliance: 20/100 (No agreement)",
            "Information Exchange: 60/100 (Active)",
            "Mutual Defense: 15/100 (Not established)",
            "Cultural Exchange: 35/100 (Limited)",
            "Current Tension: Low (Stable)"
        };

        private readonly string[] _placeholderTrade = {
            "Offer: 5 food for 2 medicine (Valid until Day 30)",
            "Offer: 10 fuel for 5 materials (Open negotiation)",
            "Request: Information on Sector 12 (Trade required)",
            "Trade History: 12 successful transactions",
            "Debt Outstanding: 15 units (Due Day 30)",
            "Credit Rating: Good (85/100)"
        };

        private readonly string[] _placeholderEvents = {
            "[Day 24] Trade offer received — 5 food for 2 medicine",
            "[Day 20] Information exchange completed — Sector 12 data",
            "[Day 18] Diplomatic envoy visited — relationship improved",
            "[Day 15] Trade dispute resolved — no penalties",
            "[Day 10] Mutual defense proposal rejected — low priority"
        };

        public void Bind(object faction)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_factionInfo == null || _relationsData == null || _tradeOffers == null || _factionEvents == null) return;

            while (_factionInfo.GetChildCount() > 0) _factionInfo.RemoveChild(_factionInfo.GetChild(0));
            while (_relationsData.GetChildCount() > 0) _relationsData.RemoveChild(_relationsData.GetChild(0));
            while (_tradeOffers.GetChildCount() > 0) _tradeOffers.RemoveChild(_tradeOffers.GetChild(0));
            while (_factionEvents.GetChildCount() > 0) _factionEvents.RemoveChild(_factionEvents.GetChild(0));

            foreach (string info in _placeholderFactionInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _factionInfo.AddChild(label);
            }

            foreach (string relation in _placeholderRelations)
            {
                var label = new Label { Text = relation };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _relationsData.AddChild(label);
            }

            foreach (string trade in _placeholderTrade)
            {
                var label = new Label { Text = trade };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _tradeOffers.AddChild(label);
            }

            foreach (string eventEntry in _placeholderEvents)
            {
                var label = new Label { Text = eventEntry };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
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

            var title = AshfallUiHelpers.MakeTitle("FACTION DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblFactionInfoTitle = AshfallUiHelpers.MakeSectionHeader("FACTION INFORMATION");
            vbox.AddChild(_lblFactionInfoTitle);

            _factionInfo = new VBoxContainer();
            _factionInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _factionInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_factionInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblRelationsTitle = AshfallUiHelpers.MakeSectionHeader("RELATIONSHIP STATUS");
            vbox.AddChild(_lblRelationsTitle);

            _relationsData = new VBoxContainer();
            _relationsData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _relationsData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_relationsData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTradeTitle = AshfallUiHelpers.MakeSectionHeader("TRADE OFFERS");
            vbox.AddChild(_lblTradeTitle);

            _tradeOffers = new VBoxContainer();
            _tradeOffers.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tradeOffers.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_tradeOffers);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblEventsTitle = AshfallUiHelpers.MakeSectionHeader("FACTION EVENTS");
            vbox.AddChild(_lblEventsTitle);

            _factionEvents = new VBoxContainer();
            _factionEvents.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _factionEvents.CustomMinimumSize = new Vector2(400, 0);
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
