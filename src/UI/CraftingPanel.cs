using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Crafting;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Crafting panel.
    /// Displays real recipes from CraftingHostSession, shows ingredient availability,
    /// lets the player start crafts and track the active queue.
    /// Thin presentation layer — all craft logic lives in CraftingSystem.
    /// </summary>
    public partial class CraftingPanel : Control
    {
        public event Action? OnClose;
        public event Action? OnCraftStarted;

        public bool IsBound => _craftingHost != null;

        private CraftingHostSession? _craftingHost;
        private InventoryHostSession? _inventoryHost;

        private VBoxContainer _recipeList = null!;
        private VBoxContainer _queueList = null!;
        private Label _queueHeader = null!;
        private Label _filterStatus = null!;

        private string _activeFilter = "all";  // "all" | "craftable" | "queued"
        private bool _craftSubmitting; // debounce

        // ── Binding ────────────────────────────────────────────────────

        public void Bind(CraftingHostSession crafting, InventoryHostSession? inventory = null)
        {
            _craftingHost = crafting;
            _inventoryHost = inventory;

            // Subscribe to crafting events so the panel stays fresh
            _craftingHost.Engine.OnCraftStarted -= OnEngineCraftStarted;
            _craftingHost.Engine.OnCraftCompleted -= OnEngineCraftCompleted;
            _craftingHost.Engine.OnCraftStarted += OnEngineCraftStarted;
            _craftingHost.Engine.OnCraftCompleted += OnEngineCraftCompleted;

            RefreshView();
        }

        private void OnEngineCraftStarted(Recipe _)
        {
            _craftSubmitting = false;
            RefreshView();
            OnCraftStarted?.Invoke();
        }

        private void OnEngineCraftCompleted(Recipe _) => RefreshView();

        // ── Refresh ────────────────────────────────────────────────────

        public void RefreshView()
        {
            if (_recipeList == null || _queueList == null) return;

            // Clear
            ClearChildren(_recipeList);
            ClearChildren(_queueList);

            if (_craftingHost == null)
            {
                _recipeList.AddChild(AshfallUiHelpers.MakeMetadata("No crafting session bound."));
                return;
            }

            // ── Recipe list ────────────────────────────────────────────
            int shown = 0;
            foreach (var recipe in _craftingHost.Recipes)
            {
                if (recipe == null) continue;

                bool canCraft = _craftingHost.Engine.CanCraft(recipe);

                if (_activeFilter == "craftable" && !canCraft) continue;
                if (_activeFilter == "queued") continue; // queued-only shown in queue section

                _recipeList.AddChild(MakeRecipeCard(recipe, canCraft));
                shown++;
            }

            if (shown == 0)
                _recipeList.AddChild(AshfallUiHelpers.MakeMetadata(
                    _activeFilter == "craftable" ? "No recipes currently craftable." : "No recipes available."));

            // ── Active queue ───────────────────────────────────────────
            bool hasActive = _craftingHost.Engine.ActiveCraftCount > 0;
            _queueHeader.Text = hasActive
                ? $"CRAFTING QUEUE  [{_craftingHost.Engine.ActiveCraftCount} active]"
                : "CRAFTING QUEUE  [idle]";

            if (hasActive)
            {
                foreach (var active in _craftingHost.Engine.ActiveCrafts)
                {
                    if (active?.Recipe == null) continue;
                    _queueList.AddChild(MakeQueueRow(active));
                }
            }
            else
            {
                _queueList.AddChild(AshfallUiHelpers.MakeMetadata("No active crafts. Start a recipe above."));
            }
        }

        private Control MakeRecipeCard(Recipe recipe, bool canCraft)
        {
            var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            card.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            // Header row: name + duration
            var headerRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var nameLabel = AshfallUiHelpers.MakeSmall(recipe.recipeName.ToUpperInvariant());
            nameLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            nameLabel.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(canCraft ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim));
            headerRow.AddChild(nameLabel);

            var duration = AshfallUiHelpers.MakeMono($"{recipe.craftingTimeHours:F0}h");
            headerRow.AddChild(duration);
            card.AddChild(headerRow);

            // Output row
            string outputText = recipe.result != null
                ? $"→ {recipe.result.displayName} ×{recipe.resultAmount}"
                : "→ [unknown output]";
            if (!string.IsNullOrEmpty(recipe.requiredStationId))
                outputText += $"  [station: {recipe.requiredStationId}]";
            var outputLabel = AshfallUiHelpers.MakeMetadata(outputText);
            card.AddChild(outputLabel);

            // Ingredient rows
            foreach (var ing in recipe.ingredients)
            {
                if (ing?.item == null) continue;
                int held = CountItem(ing.item.id);
                bool sufficient = held >= ing.amount;
                var ingRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingXs);

                var statusMark = AshfallUiHelpers.MakeMono(sufficient ? "[OK] " : "[!!] ");
                statusMark.AddThemeColorOverride("font_color",
                    AshfallUiHelpers.ToColor(sufficient ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Critical));
                ingRow.AddChild(statusMark);

                var ingLabel = AshfallUiHelpers.MakeSmall(
                    $"{ing.item.displayName} ×{ing.amount}  (held: {held})");
                ingLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                ingRow.AddChild(ingLabel);
                card.AddChild(ingRow);
            }

            // Craft button
            var actionRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            string recipeId = recipe.id;
            var btnCraft = AshfallUiHelpers.MakeButton(
                canCraft ? $"CRAFT {recipe.result?.displayName ?? "item"}" : "INGREDIENTS NEEDED",
                () =>
                {
                    if (_craftSubmitting) return;
                    _craftSubmitting = true;
                    _craftingHost?.Start(recipeId);
                });
            btnCraft.Disabled = !canCraft || _craftSubmitting;
            btnCraft.CustomMinimumSize = new Vector2(200, 30);
            actionRow.AddChild(btnCraft);

            if (!canCraft)
            {
                string reason = GetCraftBlockReason(recipe);
                var reasonLabel = AshfallUiHelpers.MakeMetadata(reason);
                reasonLabel.AddThemeColorOverride("font_color",
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
                actionRow.AddChild(reasonLabel);
            }

            card.AddChild(actionRow);

            var panel = AshfallUiHelpers.MakePanel();
            panel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            panel.AddChild(card);
            return panel;
        }

        private Control MakeQueueRow(ActiveCraft active)
        {
            var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            row.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var name = AshfallUiHelpers.MakeSmall(active.Recipe.recipeName);
            name.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            row.AddChild(name);

            var time = AshfallUiHelpers.MakeMono($"{active.HoursRemaining:F1}h remaining");
            time.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            row.AddChild(time);

            return row;
        }

        private int CountItem(string id)
        {
            if (_inventoryHost == null) return 0;
            return _inventoryHost.Inventory.CountById(id);
        }

        private string GetCraftBlockReason(Recipe recipe)
        {
            if (recipe.ingredients != null)
            {
                foreach (var ing in recipe.ingredients)
                {
                    if (ing?.item == null) continue;
                    if (CountItem(ing.item.id) < ing.amount)
                        return $"Need {ing.item.displayName} ×{ing.amount}";
                }
            }
            if (!string.IsNullOrEmpty(recipe.requiredStationId))
            {
                var station = _craftingHost?.Engine.GetStation(recipe.requiredStationId);
                if (station == null || !station.IsOperational)
                    return $"Station '{recipe.requiredStationId}' unavailable";
            }
            return "Cannot craft";
        }

        private static void ClearChildren(Node parent)
        {
            AshfallUiHelpers.EmptyChildren(parent);
        }

        // ── Godot lifecycle ────────────────────────────────────────────

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.90f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(720, 600);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            // ── Title bar ─────────────────────────────────────────────
            var titleRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("CRAFTING // WORKBENCH", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            titleRow.AddChild(title);
            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            titleRow.AddChild(btnClose);
            vbox.AddChild(titleRow);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Filter tabs ────────────────────────────────────────────
            var filterRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            _filterStatus = AshfallUiHelpers.MakeSmall("Filter:");
            filterRow.AddChild(_filterStatus);

            foreach (var (label, key) in new[] { ("ALL", "all"), ("CRAFTABLE NOW", "craftable") })
            {
                string filterKey = key;
                var btn = AshfallUiHelpers.MakeButton(label, () =>
                {
                    _activeFilter = filterKey;
                    RefreshView();
                });
                btn.CustomMinimumSize = new Vector2(120, 28);
                filterRow.AddChild(btn);
            }
            vbox.AddChild(filterRow);

            // ── Scrollable recipe list ─────────────────────────────────
            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("AVAILABLE RECIPES"));
            var recipeScroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(680, 280),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(recipeScroll);

            _recipeList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            _recipeList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            recipeScroll.AddChild(_recipeList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Queue section ──────────────────────────────────────────
            _queueHeader = AshfallUiHelpers.MakeSectionHeader("CRAFTING QUEUE  [idle]");
            vbox.AddChild(_queueHeader);

            _queueList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            _queueList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            vbox.AddChild(_queueList);

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            _craftSubmitting = false;
            RefreshView();
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
