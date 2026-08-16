using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Research panel.
    /// Shows discovered knowledge, technological advancements, and research progress.
    /// </summary>
    public partial class ResearchPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblKnowledgeTitle;
        private VBoxContainer _knowledgeList;
        private Label _lblResearchTitle;
        private VBoxContainer _researchList;
        private Label _lblTechTitle;
        private VBoxContainer _techList;

        // Placeholder research data
        private readonly string[] _placeholderKnowledge = {
            "Nuclear Winter Basics — Understanding climate impact",
            "Radiation Medicine — Basic treatment protocols",
            "Water Purification — Filtration methods discovered",
            "Crop Cultivation — Hydroponic techniques learned",
            "Radio Communication — Basic signal processing"
        };

        private readonly string[] _placeholderResearch = {
            "Advanced Water Filtration — 40% complete",
            "Radiation Shielding Materials — 25% complete",
            "Improved Gas Masks — 60% complete",
            "Solar Power Systems — 10% complete",
            "Food Preservation Techniques — 75% complete"
        };

        private readonly string[] _placeholderTech = {
            "Basic Water Filter — Unlocked",
            "Gas Mask (Basic) — Unlocked",
            "Radiation Dosimeter — Unlocked",
            "Hand-Crank Radio — Unlocked",
            "Improvised Stove — Unlocked"
        };

        // Real data from host session
        // private ResearchHostSession? _researchHost;

        public void Bind(object research) // placeholder for ResearchHostSession
        {
            // _researchHost = (ResearchHostSession)research;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_knowledgeList == null || _researchList == null || _techList == null) return;

            // Clear existing lists
            while (_knowledgeList.GetChildCount() > 0)
                _knowledgeList.RemoveChild(_knowledgeList.GetChild(0));
            while (_researchList.GetChildCount() > 0)
                _researchList.RemoveChild(_researchList.GetChild(0));
            while (_techList.GetChildCount() > 0)
                _techList.RemoveChild(_techList.GetChild(0));

            // Display placeholder knowledge
            foreach (string knowledge in _placeholderKnowledge)
            {
                var label = new Label { Text = knowledge };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _knowledgeList.AddChild(label);
            }

            // Display placeholder research progress
            foreach (string research in _placeholderResearch)
            {
                var label = new Label { Text = research };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _researchList.AddChild(label);
            }

            // Display placeholder unlocked tech
            foreach (string tech in _placeholderTech)
            {
                var label = new Label { Text = tech };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _techList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("RESEARCH & TECHNOLOGY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Knowledge section
            _lblKnowledgeTitle = AshfallUiHelpers.MakeSectionHeader("DISCOVERED KNOWLEDGE");
            vbox.AddChild(_lblKnowledgeTitle);

            _knowledgeList = new VBoxContainer();
            _knowledgeList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _knowledgeList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_knowledgeList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Research progress section
            _lblResearchTitle = AshfallUiHelpers.MakeSectionHeader("ACTIVE RESEARCH");
            vbox.AddChild(_lblResearchTitle);

            _researchList = new VBoxContainer();
            _researchList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _researchList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_researchList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Unlocked tech section
            _lblTechTitle = AshfallUiHelpers.MakeSectionHeader("UNLOCKED TECHNOLOGY");
            vbox.AddChild(_lblTechTitle);

            _techList = new VBoxContainer();
            _techList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _techList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_techList);

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
