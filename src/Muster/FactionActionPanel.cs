using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.Muster;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.Muster
{
    /// <summary>
    /// Plan 25 peacetime faction-action panel: lists the offers the
    /// FactionActionBoard makes available today (standing-band variant already
    /// resolved) with one button per authored choice. Thin presentation only —
    /// selection, effects, idempotence and flags all live in core.
    /// </summary>
    public partial class FactionActionPanel : PanelContainer
    {
        private FactionActionBoard _board;
        private VBoxContainer _offerList;
        private VBoxContainer _cultureList;
        private List<FactionCultureEntry> _culture = new List<FactionCultureEntry>();
        private int _day;

        /// <summary>Host callback: the player pressed a choice button.</summary>
        public event Action<string, string> OnChoicePressed;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(400, 220);

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "FACTION ECOLOGY — PEACETIME OFFERS",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(title);

            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 180)
            };
            rootVbox.AddChild(scroll);

            _offerList = new VBoxContainer();
            scroll.AddChild(_offerList);

            var cultureTitle = new Label
            {
                Text = "FACTION CULTURE — CODEX",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            cultureTitle.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(cultureTitle);

            var cultureScroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 140)
            };
            rootVbox.AddChild(cultureScroll);

            _cultureList = new VBoxContainer();
            cultureScroll.AddChild(_cultureList);
        }

        public void Bind(FactionActionBoard board) => _board = board;

        public void BindCulture(List<FactionCultureEntry> culture) =>
            _culture = culture ?? new List<FactionCultureEntry>();

        public void RefreshView(int day)
        {
            _day = day;
            if (_offerList == null || _board == null) return;
            AshfallUiHelpers.EmptyChildren(_offerList);

            var offers = _board.AvailableActions(day);
            if (offers.Count == 0)
            {
                _offerList.AddChild(new Label
                {
                    Text = $"No faction offers on the table today (day {day}).",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                });
                return;
            }

            for (int i = 0; i < offers.Count; i++)
            {
                var offer = offers[i];
                var card = new VBoxContainer();
                card.AddThemeConstantOverride("separation", 2);

                var header = new Label
                {
                    Text = $"{offer.Definition.title}  [{offer.Band}]",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                header.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                header.AddThemeColorOverride("font_color",
                    AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                card.AddChild(header);

                var body = new Label
                {
                    Text = string.IsNullOrEmpty(offer.VariantText)
                        ? offer.Definition.text
                        : offer.VariantText,
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                body.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                card.AddChild(body);

                var def = offer.Definition;
                var band = offer.Band;
                var variant = FactionActionBoard.SelectVariant(def, band);
                if (variant != null)
                {
                    for (int c = 0; c < variant.choices.Count; c++)
                    {
                        var choice = variant.choices[c];
                        var button = new Button
                        {
                            Text = choice.text,
                            TooltipText = $"{def.id} / {choice.choiceId}",
                        };
                        string actionId = def.id;
                        string choiceId = choice.choiceId;
                        button.Pressed += () => OnChoicePressed?.Invoke(actionId, choiceId);
                        card.AddChild(button);
                    }
                }

                _offerList.AddChild(card);
            }

            RenderCulture();
        }

        private void RenderCulture()
        {
            if (_cultureList == null) return;
            AshfallUiHelpers.EmptyChildren(_cultureList);
            for (int i = 0; i < _culture.Count; i++)
            {
                var entry = _culture[i];
                var card = new VBoxContainer();
                card.AddThemeConstantOverride("separation", 2);

                var header = new Label
                {
                    Text = $"{entry.title}  ({entry.factionId})",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                header.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                header.AddThemeColorOverride("font_color",
                    AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                card.AddChild(header);

                var body = new Label
                {
                    Text = entry.body,
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                body.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                card.AddChild(body);

                _cultureList.AddChild(card);
            }
        }
    }
}
