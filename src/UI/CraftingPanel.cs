using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Crafting panel.
    /// Shows available recipes and crafting progress.
    /// </summary>
    public partial class CraftingPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblRecipesTitle;
        private VBoxContainer _recipeList;

        // Placeholder recipes
        private readonly string[] _placeholderRecipes = {
            "Bandage (2x Cloth + 1x Water)",
            "Ration (1x Canned Food + 1x Water)",
            "Medkit (3x Bandage + 1x Iodine)",
            "Gas Mask Filter (2x Charcoal + 1x Cloth)",
            "Water Purifier (1x Sand + 1x Cloth + 1x Metal)"
        };

        // Real data from host session
        private CraftingHostSession? _craftingHost;

        public void Bind(CraftingHostSession crafting)
        {
            _craftingHost = crafting;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_recipeList == null) return;

            // Clear existing recipes
            while (_recipeList.GetChildCount() > 0)
            {
                _recipeList.RemoveChild(_recipeList.GetChild(0));
            }

            if (_craftingHost != null)
            {
                // Bind real crafting data (placeholder - actual implementation would use real recipe API)
                for (int i = 0; i < _placeholderRecipes.Length; i++)
                {
                    var recipeLabel = new Label { Text = _placeholderRecipes[i] };
                    recipeLabel.CustomMinimumSize = new Vector2(400, 50);
                    recipeLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    recipeLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                    _recipeList.AddChild(recipeLabel);
                }
            }
            else
            {
                // Fall back to placeholders
                foreach (string recipe in _placeholderRecipes)
                {
                    var recipeLabel = new Label { Text = recipe };
                    recipeLabel.CustomMinimumSize = new Vector2(400, 50);
                    recipeLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    recipeLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                    _recipeList.AddChild(recipeLabel);
                }
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            // Background overlay
            var bg = new ColorRect
            {
                Color = new Color(0.05f, 0.05f, 0.05f, 0.92f)
            };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            // Content container
            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(500, 0);
            container.AddChild(vbox);

            // Title
            var title = AshfallUiHelpers.MakeTitle("CRAFTING", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Recipes section
            _lblRecipesTitle = AshfallUiHelpers.MakeSectionHeader("AVAILABLE RECIPES");
            vbox.AddChild(_lblRecipesTitle);

            _recipeList = new VBoxContainer();
            _recipeList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _recipeList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_recipeList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Close button
            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            // Keyboard shortcut
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
