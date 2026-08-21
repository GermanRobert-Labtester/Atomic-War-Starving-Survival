using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Crafting Detail panel.
    /// Shows detailed crafting recipes, materials needed, crafting progress, and unlocked recipes.
    /// </summary>
    public partial class CraftingDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblRecipeTitle;
        private VBoxContainer _recipeInfo;
        private Label _lblMaterialsTitle;
        private VBoxContainer _materialsList;
        private Label _lblProgressTitle;
        private VBoxContainer _craftingProgress;
        private Label _lblUnlockedTitle;
        private VBoxContainer _unlockedRecipes;

        private readonly string[] _placeholderRecipe = {
            "Recipe: Improvised Cooking Stove",
            "Category: Survival Equipment",
            "Difficulty: Medium",
            "Time Required: 4 hours",
            "Result: Functional cooking device",
            "Benefits: +20% food efficiency, unlocks advanced recipes"
        };

        private readonly string[] _placeholderMaterials = {
            "Metal Sheet: 2 units (Have: 3) ✓",
            "Wire: 1 unit (Have: 1) ✓",
            "Bricks: 4 units (Have: 5) ✓",
            "Glass Shard: 1 unit (Have: 0) ✗",
            "Copper Wire: 1 unit (Have: 0) ✗",
            "Missing Materials: 2/5 (40% complete)"
        };

        private readonly string[] _placeholderProgress = {
            "Current Recipe: Improvised Cooking Stove",
            "Progress: 60% complete",
            "Materials Collected: 3/5",
            "Assembly: In progress",
            "Quality Check: Pending",
            "Estimated Completion: Day 26, 18:00"
        };

        private readonly string[] _placeholderUnlocked = {
            "Basic Water Filter ✓",
            "Improvised Stove ✓",
            "Gas Mask (Basic) ✓",
            "Bandage Kit ✓",
            "Radiation Dosimeter ✓",
            "Next Recipe: Advanced Water Purifier (Locked)"
        };

        public void Bind(object craftingDetail)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_recipeInfo == null || _materialsList == null || _craftingProgress == null || _unlockedRecipes == null) return;

            AshfallUiHelpers.EmptyChildren(_recipeInfo);
            AshfallUiHelpers.EmptyChildren(_materialsList);
            AshfallUiHelpers.EmptyChildren(_craftingProgress);
            AshfallUiHelpers.EmptyChildren(_unlockedRecipes);

            foreach (string info in _placeholderRecipe)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _recipeInfo.AddChild(label);
            }

            foreach (string material in _placeholderMaterials)
            {
                var label = new Label { Text = material };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _materialsList.AddChild(label);
            }

            foreach (string progress in _placeholderProgress)
            {
                var label = new Label { Text = progress };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _craftingProgress.AddChild(label);
            }

            foreach (string recipe in _placeholderUnlocked)
            {
                var label = new Label { Text = recipe };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _unlockedRecipes.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("CRAFTING DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblRecipeTitle = AshfallUiHelpers.MakeSectionHeader("RECIPE INFORMATION");
            vbox.AddChild(_lblRecipeTitle);

            _recipeInfo = new VBoxContainer();
            _recipeInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _recipeInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_recipeInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblMaterialsTitle = AshfallUiHelpers.MakeSectionHeader("MATERIALS REQUIRED");
            vbox.AddChild(_lblMaterialsTitle);

            _materialsList = new VBoxContainer();
            _materialsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _materialsList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_materialsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblProgressTitle = AshfallUiHelpers.MakeSectionHeader("CRAFTING PROGRESS");
            vbox.AddChild(_lblProgressTitle);

            _craftingProgress = new VBoxContainer();
            _craftingProgress.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _craftingProgress.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_craftingProgress);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblUnlockedTitle = AshfallUiHelpers.MakeSectionHeader("UNLOCKED RECIPES");
            vbox.AddChild(_lblUnlockedTitle);

            _unlockedRecipes = new VBoxContainer();
            _unlockedRecipes.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _unlockedRecipes.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_unlockedRecipes);

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
