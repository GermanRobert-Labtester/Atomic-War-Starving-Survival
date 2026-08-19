using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Research Detail panel.
    /// Shows detailed research progress, discovered knowledge, research queue, and research outcomes.
    /// </summary>
    public partial class ResearchDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblResearchInfoTitle;
        private VBoxContainer _researchInfo;
        private Label _lblKnowledgeTitle;
        private VBoxContainer _discoveredKnowledge;
        private Label _lblQueueTitle;
        private VBoxContainer _researchQueue;
        private Label _lblOutcomesTitle;
        private VBoxContainer _researchOutcomes;

        private readonly string[] _placeholderResearchInfo = {
            "Current Research: Advanced Water Filtration",
            "Category: Survival Technology",
            "Progress: 40% complete",
            "Estimated Completion: Day 35",
            "Researcher: David (Engineer)",
            "Resources Required: 5 materials, 2 knowledge points"
        };

        private readonly string[] _placeholderKnowledge = {
            "Nuclear Winter Basics — Discovered",
            "Radiation Medicine — Discovered",
            "Water Purification — Discovered",
            "Crop Cultivation — In Progress (60%)",
            "Radio Communication — Discovered",
            "Advanced Filtration — In Progress (40%)"
        };

        private readonly string[] _placeholderQueue = {
            "Queue 1: Advanced Water Filtration (40% complete)",
            "Queue 2: Crop Cultivation (60% complete)",
            "Queue 3: Solar Power Integration (15% complete)",
            "Queue 4: Food Preservation Techniques (75% complete)",
            "Queue 5: Radiation Shielding Materials (25% complete)",
            "Total Queue: 5 research projects"
        };

        private readonly string[] _placeholderOutcomes = {
            "Completed: Basic Water Filter — Unlocked",
            "Completed: Gas Mask (Basic) — Unlocked",
            "Completed: Radiation Dosimeter — Unlocked",
            "Completed: Hand-Crank Radio — Unlocked",
            "Completed: Improvised Stove — Unlocked",
            "Next Outcome: Advanced Water Filter (Day 35)"
        };

        public void Bind(object researchDetail)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_researchInfo == null || _discoveredKnowledge == null || _researchQueue == null || _researchOutcomes == null) return;

            AshfallUiHelpers.EmptyChildren(_researchInfo);
            AshfallUiHelpers.EmptyChildren(_discoveredKnowledge);
            AshfallUiHelpers.EmptyChildren(_researchQueue);
            AshfallUiHelpers.EmptyChildren(_researchOutcomes);

            foreach (string info in _placeholderResearchInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _researchInfo.AddChild(label);
            }

            foreach (string knowledge in _placeholderKnowledge)
            {
                var label = new Label { Text = knowledge };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _discoveredKnowledge.AddChild(label);
            }

            foreach (string queue in _placeholderQueue)
            {
                var label = new Label { Text = queue };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _researchQueue.AddChild(label);
            }

            foreach (string outcome in _placeholderOutcomes)
            {
                var label = new Label { Text = outcome };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _researchOutcomes.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("RESEARCH DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblResearchInfoTitle = AshfallUiHelpers.MakeSectionHeader("RESEARCH INFORMATION");
            vbox.AddChild(_lblResearchInfoTitle);

            _researchInfo = new VBoxContainer();
            _researchInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _researchInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_researchInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblKnowledgeTitle = AshfallUiHelpers.MakeSectionHeader("DISCOVERED KNOWLEDGE");
            vbox.AddChild(_lblKnowledgeTitle);

            _discoveredKnowledge = new VBoxContainer();
            _discoveredKnowledge.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _discoveredKnowledge.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_discoveredKnowledge);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblQueueTitle = AshfallUiHelpers.MakeSectionHeader("RESEARCH QUEUE");
            vbox.AddChild(_lblQueueTitle);

            _researchQueue = new VBoxContainer();
            _researchQueue.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _researchQueue.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_researchQueue);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblOutcomesTitle = AshfallUiHelpers.MakeSectionHeader("RESEARCH OUTCOMES");
            vbox.AddChild(_lblOutcomesTitle);

            _researchOutcomes = new VBoxContainer();
            _researchOutcomes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _researchOutcomes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_researchOutcomes);

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
