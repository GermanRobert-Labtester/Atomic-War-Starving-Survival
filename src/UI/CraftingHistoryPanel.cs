using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Crafting History panel.
    /// Shows detailed crafting history, recipes crafted, and crafting outcomes.
    /// </summary>
    public partial class CraftingHistoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHistoryTitle;
        private VBoxContainer _craftingHistory;
        private Label _lblRecipesTitle;
        private VBoxContainer _craftedRecipes;
        private Label _lblOutcomesTitle;
        private VBoxContainer _craftingOutcomes;

        private readonly string[] _placeholderHistory = {
            "[Day 10] Crafted Basic Water Filter — Success",
            "[Day 12] Crafted Improvised Stove — Success",
            "[Day 15] Crafted Gas Mask (Basic) — Success",
            "[Day 18] Crafted Bandage Kit — Success",
            "[Day 20] Crafted Radiation Dosimeter — Success"
        };

        private readonly string[] _placeholderRecipes = {
            "Basic Water Filter ✓ — Unlocked",
            "Improvised Stove ✓ — Unlocked",
            "Gas Mask (Basic) ✓ — Unlocked",
            "Bandage Kit ✓ — Unlocked",
            "Radiation Dosimeter ✓ — Unlocked",
            "Next Recipe: Advanced Water Purifier (Locked)"
        };

        private readonly string[] _placeholderOutcomes = {
            "Day 10: Water filter — +20% water efficiency",
            "Day 12: Cooking stove — +15% food efficiency",
            "Day 15: Gas mask — +40% radiation protection",
            "Day 18: Bandages — +50% wound treatment",
            "Day 20: Dosimeter — +100% radiation monitoring",
            "Total Recipes Crafted: 5 unlocked"
        };

        public void Bind(object craftingHistory)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_craftingHistory == null || _craftedRecipes == null || _craftingOutcomes == null) return;

            AshfallUiHelpers.EmptyChildren(_craftingHistory);
            AshfallUiHelpers.EmptyChildren(_craftedRecipes);
            AshfallUiHelpers.EmptyChildren(_craftingOutcomes);

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _craftingHistory.AddChild(label);
            }

            foreach (string recipe in _placeholderRecipes)
            {
                var label = new Label { Text = recipe };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _craftedRecipes.AddChild(label);
            }

            foreach (string outcome in _placeholderOutcomes)
            {
                var label = new Label { Text = outcome };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _craftingOutcomes.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("CRAFTING HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("CRAFTING HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _craftingHistory = new VBoxContainer();
            _craftingHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _craftingHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_craftingHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblRecipesTitle = AshfallUiHelpers.MakeSectionHeader("CRAFTED RECIPES");
            vbox.AddChild(_lblRecipesTitle);

            _craftedRecipes = new VBoxContainer();
            _craftedRecipes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _craftedRecipes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_craftedRecipes);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblOutcomesTitle = AshfallUiHelpers.MakeSectionHeader("CRAFTING OUTCOMES");
            vbox.AddChild(_lblOutcomesTitle);

            _craftingOutcomes = new VBoxContainer();
            _craftingOutcomes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _craftingOutcomes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_craftingOutcomes);

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
