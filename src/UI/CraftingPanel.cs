// SPDX-License-Identifier: MIT
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
    public partial class CraftingPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action? OnCraftStarted;
        public event Action? OnOpenWorkshopRequested;
        public event Action? OnOpenPharmaLabRequested;

        public bool IsBound => _craftingHost != null;

        private CraftingHostSession? _craftingHost;
        private InventoryHostSession? _inventoryHost;
        private SurvivorsHostSession? _survivorsHost;

        private VBoxContainer _recipeList = null!;
        private VBoxContainer _queueList = null!;
        private Label _queueHeader = null!;
        private Label _filterStatus = null!;

        // ── Bench operator (trade specialty attribution) ───────────────
        private OptionButton? _crafterSelect;
        private Label? _crafterDetail;
        private readonly List<string> _crafterIds = new();
        private Ashfall.Core.Survivors.TradeSpecialtySystem? _tradeSpecialty;
        private Func<string, string>? _professionResolver;

        /// <summary>
        /// The survivor chosen at the bench, or "" for unassigned — in which case
        /// the host auto-credits a living survivor whose trade matches the item.
        /// Index 0 is the UNASSIGNED row, so ids are offset by one.
        /// </summary>
        public string SelectedCrafterId =>
            _crafterSelect != null && _crafterSelect.Selected > 0 && _crafterSelect.Selected - 1 < _crafterIds.Count
                ? _crafterIds[_crafterSelect.Selected - 1]
                : string.Empty;

        private string _activeFilter = "all";  // "all" | "craftable" | "queued"
        private bool _craftSubmitting; // debounce

        // ── Binding ────────────────────────────────────────────────────

        public void Bind(
            CraftingHostSession crafting,
            InventoryHostSession? inventory = null,
            SurvivorsHostSession? survivors = null,
            Ashfall.Core.Survivors.TradeSpecialtySystem? tradeSpecialty = null,
            Func<string, string>? professionResolver = null)
        {
            _craftingHost = crafting;
            _inventoryHost = inventory;
            _survivorsHost = survivors;
            _tradeSpecialty = tradeSpecialty;
            _professionResolver = professionResolver;

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

        private void OnEngineCraftCompleted(Recipe recipe, string crafterId) => RefreshView();

        // ── Refresh ────────────────────────────────────────────────────

        public void RefreshView()
        {
            if (_recipeList == null || _queueList == null) return;

            RefreshCrafterOptions();

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
                    // Read at click time, not at card-build time: the player may
                    // change the bench operator after the recipe list was drawn.
                    _craftingHost?.Start(recipeId, SelectedCrafterId);
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

        /// <summary>
        /// Rebuild the bench-operator list from the living roster. The current
        /// selection is preserved across refreshes so starting a craft does not
        /// silently reassign the bench to UNASSIGNED.
        /// </summary>
        private void RefreshCrafterOptions()
        {
            if (_crafterSelect == null) return;
            string previous = SelectedCrafterId;

            _crafterSelect.Clear();
            _crafterIds.Clear();
            _crafterSelect.AddItem("UNASSIGNED (auto-credit)");

            var rosterSystem = _survivorsHost?.Roster;
            if (rosterSystem != null)
            {
                for (int i = 0; i < rosterSystem.Roster.Count; i++)
                {
                    var entry = rosterSystem.Roster[i];
                    if (entry == null || !entry.isAlive || string.IsNullOrEmpty(entry.survivorId)) continue;

                    var def = rosterSystem.FindDefinition(entry.definitionId);
                    string name = def != null && !string.IsNullOrEmpty(def.displayName)
                        ? def.displayName
                        : entry.survivorId;
                    string trade = def != null && !string.IsNullOrEmpty(def.profession)
                        ? def.profession
                        : "no trade";

                    _crafterIds.Add(entry.survivorId);
                    _crafterSelect.AddItem($"{name} — {trade}");
                }
            }

            int restore = previous.Length > 0 ? _crafterIds.IndexOf(previous) : -1;
            _crafterSelect.Selected = restore >= 0 ? restore + 1 : 0;
            UpdateCrafterDetail();
        }

        /// <summary>
        /// Show the bench operator's real trade-specialty state: the authored tier
        /// title and progress, or the authored mastery_bonus_text once mastered.
        /// Presentation only — every value is read from TradeSpecialtySystem, which
        /// owns the catalog and the per-survivor progress.
        /// </summary>
        private void UpdateCrafterDetail()
        {
            if (_crafterDetail == null) return;

            string survivorId = SelectedCrafterId;
            if (string.IsNullOrEmpty(survivorId))
            {
                _crafterDetail.Text =
                    "No one assigned — the shelter will auto-credit a living survivor whose trade matches the item.";
                return;
            }

            string name = SurvivorDisplayName(survivorId);
            string professionId = _tradeSpecialty != null
                ? ResolveProfessionId(survivorId)
                : string.Empty;
            var info = Ashfall.Core.Survivors.TradeSpecialtySystem.GetProfessionInfo(professionId);
            if (info == null)
            {
                _crafterDetail.Text = $"{name} — no trade specialty on record.";
                return;
            }

            int tier = _tradeSpecialty?.GetMasteryTier(survivorId) ?? 0;
            bool mastered = _tradeSpecialty?.HasMasteredTrade(survivorId) ?? false;
            if (mastered)
            {
                string bonus = Ashfall.Core.Survivors.TradeSpecialtySystem
                    .GetMasteryBonusText(info.ProfessionId)
                    .Replace("{name}", name);
                string masteryTitle = Ashfall.Core.Survivors.TradeSpecialtySystem
                    .GetMilestoneTitle(info.ProfessionId, 3);
                _crafterDetail.Text = string.IsNullOrEmpty(masteryTitle)
                    ? $"{name} — {info.DisplayName}, MASTERED. {bonus}"
                    : $"{name} — {info.DisplayName}, {masteryTitle}. {bonus}";
                return;
            }

            string tierTitle = tier > 0
                ? Ashfall.Core.Survivors.TradeSpecialtySystem.GetMilestoneTitle(info.ProfessionId, tier)
                : string.Empty;
            _crafterDetail.Text = string.IsNullOrEmpty(tierTitle)
                ? $"{name} — {info.DisplayName}, not yet started ({tier}/3)"
                : $"{name} — {info.DisplayName}, {tierTitle} ({tier}/3)";
        }

        private string SurvivorDisplayName(string survivorId)
        {
            var def = _survivorsHost?.Roster?.FindDefinition(survivorId);
            return def != null && !string.IsNullOrEmpty(def.displayName) ? def.displayName : survivorId;
        }

        private string ResolveProfessionId(string survivorId)
        {
            // Prefer the host resolver so the panel and the craft-attribution
            // bridge can never disagree about who has which trade.
            if (_professionResolver != null)
                return _professionResolver(survivorId) ?? string.Empty;

            var def = _survivorsHost?.Roster?.FindDefinition(survivorId);
            return Ashfall.Core.Survivors.TradeSpecialtySystem.ResolveProfessionId(
                string.Empty, def?.profession);
        }

        private static void ClearChildren(Node parent)
        {
            AshfallUiHelpers.EmptyChildren(parent);
        }

        // ── Godot lifecycle ────────────────────────────────────────────

        public override void _Ready()
        {
            // Ticket #125 follow-up: layout chrome owned by
            // res://assets/ui/panels/CraftingPanel.tscn. SceneBinder resolves
            // the typed unique-name nodes; sibling bind logic unchanged.
            var binder = new SceneBinder(this, typeof(CraftingPanel));
            binder.Require<VBoxContainer>("RecipeList");
            binder.Require<VBoxContainer>("QueueList");
            binder.Require<Label>("QueueHeader");
            binder.Require<Label>("FilterStatus");
            binder.Require<Button>("CloseButton");
            binder.Require<Button>("FilterAllButton");
            binder.Require<Button>("FilterCraftableButton");
            binder.Require<Button>("RelicWorkshopButton");
            binder.Require<Button>("PharmaLabButton");
            binder.Require<OptionButton>("CrafterSelect");
            binder.Require<Label>("CrafterDetail");

            _recipeList = binder.Get<VBoxContainer>("RecipeList");
            _queueList = binder.Get<VBoxContainer>("QueueList");
            _queueHeader = binder.Get<Label>("QueueHeader");
            _queueHeader.Text = "CRAFTING QUEUE  [idle]";
            _filterStatus = binder.Get<Label>("FilterStatus");
            _filterStatus.Text = "Filter:";
            _crafterSelect = binder.Get<OptionButton>("CrafterSelect");
            _crafterDetail = binder.Get<Label>("CrafterDetail");
            _crafterSelect.ItemSelected += _ => UpdateCrafterDetail();

            binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();

            binder.Get<Button>("FilterAllButton").Pressed += () => {
                _activeFilter = "all";
                RefreshView();
            };
            binder.Get<Button>("FilterCraftableButton").Pressed += () => {
                _activeFilter = "craftable";
                RefreshView();
            };
            binder.Get<Button>("RelicWorkshopButton").Pressed += () => {
                Visible = false;
                OnOpenWorkshopRequested?.Invoke();
            };
            binder.Get<Button>("PharmaLabButton").Pressed += () => {
                Visible = false;
                OnOpenPharmaLabRequested?.Invoke();
            };

            Visible = false;
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
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }


    public void Unbind()
    {
        if (_craftingHost != null)
            {
                _craftingHost.Engine.OnCraftStarted -= OnEngineCraftStarted;
                _craftingHost.Engine.OnCraftCompleted -= OnEngineCraftCompleted;
            }
    }

    public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
