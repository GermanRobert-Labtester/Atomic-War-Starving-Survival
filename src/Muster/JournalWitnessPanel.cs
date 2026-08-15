using System.Collections.Generic;
using Godot;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;

namespace AtomicWar.GodotApp.Muster
{
    /// <summary>
    /// Section III witness panel: renders the three Harven succession accounts
    /// (muster_witnesses.json) with the journal's trait-based framing —
    /// JournalVoice.ComposeFullText per the authoring survivor's RiskBiasTrait.
    /// Thin presentation only; framing logic lives in the core journal.
    /// </summary>
    public partial class JournalWitnessPanel : PanelContainer
    {
        private List<WitnessDefinition> _witnesses;
        private VBoxContainer _witnessList;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(400, 240);

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "THE UNSIGNED ORDER — THREE ACCOUNTS",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", 13);
            rootVbox.AddChild(title);

            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 200)
            };
            rootVbox.AddChild(scroll);

            _witnessList = new VBoxContainer();
            scroll.AddChild(_witnessList);
        }

        public void Bind(List<WitnessDefinition> witnesses)
        {
            _witnesses = witnesses ?? new List<WitnessDefinition>();
        }

        /// <summary>
        /// Renders the witnesses whose day gate has opened. The framing is
        /// keyed to the RECORDING survivor's RiskBiasTrait (Section III) —
        /// whoever wrote it down colours how it reads — never to a fixed
        /// per-witness bias.
        /// </summary>
        public void RefreshView(int day, RiskBiasTrait authorBias)
        {
            if (_witnessList == null) return;
            foreach (Node child in _witnessList.GetChildren())
                child.QueueFree();

            for (int i = 0; i < _witnesses.Count; i++)
            {
                var w = _witnesses[i];
                if (day < w.dayMin) continue;

                string framing = JournalVoice.ComposeFullText(w.knowledgeKey, authorBias, day);
                var card = new VBoxContainer();
                card.AddThemeConstantOverride("separation", 2);

                var header = new Label { Text = $"{w.witnessName} — {w.locationId}" };
                header.AddThemeFontSizeOverride("font_size", 12);
                header.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 0.6f));
                card.AddChild(header);

                var body = new Label
                {
                    Text = w.body + "\n" + framing,
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                body.AddThemeFontSizeOverride("font_size", 11);
                card.AddChild(body);

                _witnessList.AddChild(card);
            }
        }
    }
}
