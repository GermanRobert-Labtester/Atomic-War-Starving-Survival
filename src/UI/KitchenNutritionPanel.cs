// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Kitchen Galley & Nutrition Management Interface.
    /// Manages meal preparation, cook assignments, nutrition/scurvy prevention,
    /// and survivor dietary morale buffs.
    /// </summary>
    public partial class KitchenNutritionPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public Func<string?>? DefaultSurvivorResolver { get; set; }
        public Func<IReadOnlyList<string>>? LivingSurvivorsResolver { get; set; }

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _recipeList = null!;
        private VBoxContainer _prepStation = null!;
        private VBoxContainer _serviceLogContainer = null!;
        private Label _eventLogLabel = null!;

        // Plan 136 cooking authority (recipes_cooking.json) bound as a cooking
        // strip alongside the nutrition prep/serve path. The strip is the only
        // player-operable surface for CookingSystem.StartCooking/Progress/Cancel;
        // meal serving keeps routing through KitchenNutritionSystem.
        private VBoxContainer _cookingStrip = null!;
        private CookingHostSession? _cooking;
        private string _selectedCookingRecipeId = string.Empty;
        private string _cookingFeedbackLine = string.Empty;

        private KitchenNutritionHostSession? _host;
        private string _selectedRecipeId = "recipe_fungal_stew";

        private sealed class RecipeOption
        {
            public string id = string.Empty;
            public string name = string.Empty;
            public string desc = string.Empty;
            public string cost = string.Empty;
            public int morale;
            public Dictionary<string, int> inputs = new Dictionary<string, int>();
        }

        public bool IsBound => _host != null;

        /// <summary>True when the live Plan 136 cooking authority is bound to the strip.</summary>
        public bool IsCookingBound => _cooking != null;

        /// <summary>Recipe id the cooking strip will start; selected by the chip buttons.</summary>
        public string SelectedCookingRecipeId
        {
            get => _selectedCookingRecipeId;
            set { _selectedCookingRecipeId = value; RefreshView(); }
        }

        /// <summary>Last cooking-strip feedback sentence (success or refusal), or empty.</summary>
        public string LastCookingFeedback => _cookingFeedbackLine;

        /// <summary>Active CookingSystem operations owned by the bound authority.</summary>
        public int ActiveCookingOperationCount => _cooking?.Census.ActiveOperationsCount ?? 0;

        public void Bind(KitchenNutritionHostSession session)
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        /// <summary>
        /// Binds the live cooking authority to the kitchen panel's cooking strip.
        /// Idempotent and re-bind safe: drops the previous subscription first.
        /// </summary>
        public void BindCooking(CookingHostSession? session)
        {
            if (_cooking != null)
            {
                _cooking.StateChanged -= RefreshView;
            }
            _cooking = session;
            if (_cooking != null)
            {
                _cooking.StateChanged += RefreshView;
                if (string.IsNullOrEmpty(_selectedCookingRecipeId))
                {
                    var first = _cooking.System.Recipes
                        .OrderBy(r => r.id, StringComparer.Ordinal)
                        .FirstOrDefault();
                    if (first != null) _selectedCookingRecipeId = first.id;
                }
            }
            RefreshView();
        }

        /// <summary>Detaches only the cooking strip; the nutrition session binding is untouched.</summary>
        public void UnbindCooking()
        {
            if (_cooking != null)
            {
                _cooking.StateChanged -= RefreshView;
                _cooking = null;
            }
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
            UnbindCooking();
        }



        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = AshfallUiHelpers.PanelScrim() };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            _shell = new AshfallDashboardShell("SYS: KITCHEN GALLEY & NUTRITION // DIETARY MATRIX", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("prep_jobs", "ACTIVE PREP", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("cook_ops", "COOK OPS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("cooked", "MEALS COOKED", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("served", "MEALS SERVED", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("morale", "NUTRITION BUFF", "+0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("status", "GALLEY STATUS", "READY", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            // 3-Column Layout
            var gridRow = new HBoxContainer();
            gridRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            gridRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            gridRow.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Column 1: Recipe Roster
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("GALLEY RECIPE CATALOG"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _recipeList = new VBoxContainer();
            _recipeList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _recipeList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_recipeList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Meal Preparation Station
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("GALLEY COOKING STATION — RAW INGREDIENTS TO COOKED MEALS"));
            _cookingStrip = new VBoxContainer();
            _cookingStrip.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _cookingStrip.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerVbox.AddChild(_cookingStrip);
            centerVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("MEAL PREPARATION & DISPATCH"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _prepStation = new VBoxContainer();
            _prepStation.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _prepStation.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_prepStation);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Meal Distribution & Logs
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("MEAL SERVING LOG"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _serviceLogContainer = new VBoxContainer();
            _serviceLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _serviceLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_serviceLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent meal events.");
            _eventLogLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            rightVbox.AddChild(_eventLogLabel);

            gridRow.AddChild(rightPanel);

            _shell.SetContent(gridRow);
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void RefreshView()
        {
            if (_recipeList == null || _prepStation == null || _serviceLogContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_recipeList);
            AshfallUiHelpers.EmptyChildren(_prepStation);
            AshfallUiHelpers.EmptyChildren(_serviceLogContainer);
            RefreshCookingStrip();

            if (_host == null || _statusRail == null)
            {
                _recipeList.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No kitchen nutrition session bound", "offline"));
                _prepStation.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Prep station offline", "offline"));
                _serviceLogContainer.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Service log unavailable", "offline"));
                return;
            }

            var s = _host.System.State;
            int prepCount = s.activeJobs.Count;
            int totalServed = s.totalMealsServed;
            int totalMealsPrepared = s.totalMealsPrepared;

            _statusRail.Set("prep_jobs", prepCount.ToString(), prepCount > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("cooked", totalMealsPrepared.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("served", totalServed.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("morale", totalServed > 0 ? "+4 MORALE" : "NORMAL", totalServed > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("status", prepCount > 0 ? "COOKING" : "IDLE", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            // Standard recipe catalog definitions
            var recipes = new[]
            {
                new RecipeOption { id = "recipe_fungal_stew", name = "Nutritive Fungal Stew", desc = "Hearty broth fortified with underground mushroom caps.", cost = "1x Phosphor Cap Fungi, 1x Clean Water", morale = 3, inputs = new Dictionary<string, int> { ["crop_biolum_mushroom"] = 1, ["clean_water"] = 1 } },
                new RecipeOption { id = "recipe_subterranean_mushroom_mash", name = "Cultivated Mushroom Mash", desc = "Dense mash pressed from dark-bed mushroom harvests. Reliable calories when the greenhouse fails.", cost = "2x Subterranean Cap Mushrooms, 1x Clean Water", morale = 2, inputs = new Dictionary<string, int> { ["harvested_mushrooms_subterranean"] = 2, ["clean_water"] = 1 } },
                new RecipeOption { id = "recipe_canned_mash", name = "Heated Military Rations", desc = "Standard shelf-stable calories with mild vitamin paste.", cost = "1x Military Rations, 1x Fuel", morale = 2, inputs = new Dictionary<string, int> { ["military_rations"] = 1, ["fuel"] = 1 } },
                new RecipeOption { id = "recipe_greenhouse_salad", name = "Fresh Harvest Greens", desc = "Crisp hydroponic root leaves and tuber salad.", cost = "2x Fresh Winter Cress", morale = 5, inputs = new Dictionary<string, int> { ["crop_leafy_green"] = 2 } },
                new RecipeOption { id = "recipe_cured_jerky_broth", name = "Smoked Jerky & Bone Broth", desc = "Rich protein soup providing sustained work endurance.", cost = "1x Cooked Meat, 2x Clean Water", morale = 4, inputs = new Dictionary<string, int> { ["cooked_meat"] = 1, ["clean_water"] = 2 } },
                new RecipeOption
                {
                    id = "recipe_ash_flour_bread",
                    name = "Ash-Flour Bread",
                    desc = "Dense shelter bread made from grain-system flour.",
                    cost = "1x Milled Ash-Barley Flour",
                    morale = 4,
                    inputs = new Dictionary<string, int> { ["item_grain_flour"] = 1 }
                },
                new RecipeOption
                {
                    id = "recipe_confit_tuber",
                    name = "Cold-Confit Root Tubers",
                    desc = "Preserved root tubers slowly cooked and sealed in seed-pressed cooking oil.",
                    cost = "3x Frost Tuber, 1x Cooking Oil, 1x Salt",
                    morale = 4,
                    inputs = new Dictionary<string, int> { ["crop_tuber"] = 3, ["cooking_oil"] = 1, ["item_preservation_salt"] = 1 }
                }
            };

            // Populate Recipe List
            foreach (var r in recipes)
            {
                var card = AshfallUiHelpers.MakePanel();
                var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                card.AddChild(cardMargin);
                var cardVbox = new VBoxContainer();
                cardVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
                cardMargin.AddChild(cardVbox);

                var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                string badgeIcon = r.id == "recipe_greenhouse_salad" ? "badge_scurvy" : "badge_morale";
                headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon(badgeIcon, 18));
                var nameLbl = AshfallUiHelpers.MakeBody(r.name);
                nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                headerRow.AddChild(nameLbl);
                cardVbox.AddChild(headerRow);

                var costLbl = AshfallUiHelpers.MakeMono($"COST: {r.cost} (+{r.morale} Morale)");
                costLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                cardVbox.AddChild(costLbl);

                var descLbl = AshfallUiHelpers.MakeSmall(r.desc);
                descLbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                cardVbox.AddChild(descLbl);

                var selectBtn = AshfallUiHelpers.MakeButton($"SELECT // {r.id}", () =>
                {
                    _selectedRecipeId = r.id;
                    RefreshView();
                });
                selectBtn.CustomMinimumSize = new Vector2(0, 28);
                cardVbox.AddChild(selectBtn);

                _recipeList.AddChild(card);
            }

            // Prep Station Inspector
            var curRecipe = recipes.FirstOrDefault(r => r.id == _selectedRecipeId) ?? recipes[0];
            _prepStation.AddChild(AshfallUiHelpers.MakeSectionHeader($"MEAL PREP: {curRecipe.name.ToUpperInvariant()}"));
            _prepStation.AddChild(AshfallUiHelpers.MakeDataRow("Recipe ID", curRecipe.id, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            _prepStation.AddChild(AshfallUiHelpers.MakeDataRow("Ingredients Required", curRecipe.cost, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _prepStation.AddChild(AshfallUiHelpers.MakeDataRow("Nutritional Morale Impact", $"+{curRecipe.morale} Morale Bonus", AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
            string dietaryImpact = curRecipe.id switch
            {
                "recipe_greenhouse_salad" => "High Vitamin C Equivalent (Prevents Scurvy)",
                "recipe_fungal_stew" => "Trace Minerals & Fungal Antioxidants",
                "recipe_subterranean_mushroom_mash" => "Subterranean Fiber & Protein",
                "recipe_cured_jerky_broth" => "High Animal Protein & Iron",
                "recipe_ash_flour_bread" => "Complex Carbohydrates & Energy",
                "recipe_confit_tuber" => "Calorie-Dense Lipids & Electrolytes",
                _ => "Standard Caloric Rations"
            };
            _prepStation.AddChild(AshfallUiHelpers.MakeDataRow("Dietary Impact", dietaryImpact, AshfallUiHelpers.ToColor(DesignTheme.Pale)));

            _prepStation.AddChild(AshfallUiHelpers.MakeSeparator());
            _prepStation.AddChild(AshfallUiHelpers.MakeSubsectionHeader("COOK ASSIGNMENT"));
            _prepStation.AddChild(AshfallUiHelpers.MakeBody("Assign shelter cooks through the Duty Roster to schedule daily meal prep batches for this recipe."));
            _prepStation.AddChild(AshfallUiHelpers.MakeButton("START PREP // SHELTER COOK", () =>
            {
                // Use the live survivor selected by the host instead of a
                // synthetic cook id. The kitchen Core gate must evaluate the
                // same survivor fitness projection as duty and expedition work.
                string cookId = DefaultSurvivorResolver?.Invoke() ?? "cook_shelter";
                var result = _host.StartPrepJob(curRecipe.id, cookId, curRecipe.inputs);
                _eventLogLabel.Text = result.IsSuccess
                    ? $"Prep started: {curRecipe.name} ({cookId})."
                    : $"Prep blocked: {result.FailureCode}.";
                RefreshView();
            }));

            _prepStation.AddChild(AshfallUiHelpers.MakeSeparator());
            _prepStation.AddChild(AshfallUiHelpers.MakeSubsectionHeader("PANTRY & PREPARED MEALS"));

            int availablePortions = _host.System.GetAvailablePortions(curRecipe.id);
            var matchingPantry = s.pantry.Where(p => p.itemId == curRecipe.id && !p.isSpoiled && p.portionCount > 0).ToList();
            if (matchingPantry.Count > 0)
            {
                foreach (var p in matchingPantry)
                {
                    string preservationText = p.preservation switch
                    {
                        PreservationMethod.Refrigeration => "Refrigerated",
                        PreservationMethod.RootCellar => "Root Cellar",
                        _ => "Ambient"
                    };
                    _prepStation.AddChild(AshfallUiHelpers.MakeDataRow(
                        $"{curRecipe.name}",
                        $"{p.portionCount} portions · spoil in {p.spoilageTimer:F0}d ({preservationText})",
                        AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                }
            }
            else
            {
                _prepStation.AddChild(AshfallUiHelpers.MakeMetadata("No prepared portions in pantry. Start a prep job to cook."));
            }

            var serveHbox = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnServeOne = AshfallUiHelpers.MakeButton("SERVE MEAL", () =>
            {
                string? survivorId = DefaultSurvivorResolver?.Invoke();
                if (string.IsNullOrEmpty(survivorId)) survivorId = "player";

                var res = _host.ServeMeal(survivorId, curRecipe.id);
                _eventLogLabel.Text = res.IsSuccess
                    ? $"Served {curRecipe.name} to {survivorId}."
                    : $"Cannot serve: {res.FailureCode}.";
                RefreshView();
            });
            serveHbox.AddChild(btnServeOne);

            var btnServeAll = AshfallUiHelpers.MakeButton("SERVE ALL LIVING CREW", () =>
            {
                var livingSurvivors = LivingSurvivorsResolver?.Invoke()
                    ?? new List<string> { "player" };

                var res = _host.ServeAllMeals(livingSurvivors, curRecipe.id);
                _eventLogLabel.Text = res.IsSuccess
                    ? $"Served all living crew ({livingSurvivors.Count}) with {curRecipe.name}."
                    : $"Cannot serve crew: {res.FailureCode}.";
                RefreshView();
            });
            btnServeAll.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            serveHbox.AddChild(btnServeAll);
            _prepStation.AddChild(serveHbox);

            // Populate Serving Log
            if (s.servingLog.Count == 0)
            {
                _serviceLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("No meals distributed today."));
            }
            else
            {
                foreach (var serv in s.servingLog.TakeLast(8))
                {
                    _serviceLogContainer.AddChild(AshfallUiHelpers.MakeMono($"Day {serv.day}: {serv.recipeId} -> {serv.survivorId} (+{serv.moraleBonus} morale)"));
                }
            }
        }

        /// <summary>
        /// Renders the Plan 136 cooking station: the authored recipe roster from
        /// the live <see cref="CookingSystem"/> plus start/advance/cancel verbs.
        /// The strip is the player-operable link the food pipeline was missing;
        /// it consumes real inventory ingredients and delivers real cooked items,
        /// while the nutrition prep/serve path above is preserved unchanged.
        /// </summary>
        private void RefreshCookingStrip()
        {
            if (_cookingStrip == null || !GodotObject.IsInstanceValid(_cookingStrip)) return;
            AshfallUiHelpers.EmptyChildren(_cookingStrip);

            if (_cooking == null)
            {
                _statusRail?.Set("cook_ops", "0", AshfallMetricCard.Criticality.Normal);
                _cookingStrip.AddChild(AshfallUiHelpers.MakeEmptyStateLabel(
                    "Cooking authority not bound", "start a campaign to load recipes_cooking.json"));
                return;
            }

            var recipes = _cooking.System.Recipes
                .OrderBy(r => r.id, StringComparer.Ordinal)
                .ToList();
            if (recipes.Count == 0)
            {
                _statusRail?.Set("cook_ops", "0", AshfallMetricCard.Criticality.Warn);
                _cookingStrip.AddChild(AshfallUiHelpers.MakeEmptyStateLabel(
                    "No authored cooking recipes loaded", "recipes_cooking.json did not bind"));
                return;
            }

            if (string.IsNullOrEmpty(_selectedCookingRecipeId)
                || !recipes.Any(r => r.id == _selectedCookingRecipeId))
            {
                _selectedCookingRecipeId = recipes[0].id;
            }
            var selected = recipes.First(r => r.id == _selectedCookingRecipeId);

            int activeOps = _cooking.System.State.activeOperations.Count;
            _statusRail?.Set("cook_ops", activeOps.ToString(),
                activeOps > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

            // ── Recipe roster (bounded, wrapping chip strip) ──
            var chipScroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(0, 76),
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                VerticalScrollMode = ScrollContainer.ScrollMode.Auto
            };
            var chipFlow = new HFlowContainer();
            chipFlow.AddThemeConstantOverride("h_separation", DesignTheme.SpacingXs);
            chipFlow.AddThemeConstantOverride("v_separation", DesignTheme.SpacingXs);
            chipFlow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            chipScroll.AddChild(chipFlow);
            foreach (var recipe in recipes)
            {
                string recipeId = recipe.id;
                bool isSelected = recipeId == selected.id;
                var chip = AshfallUiHelpers.MakeButton(
                    isSelected ? $"[{recipe.displayName}]" : recipe.displayName,
                    () => { _selectedCookingRecipeId = recipeId; RefreshView(); });
                chip.TooltipText = $"{recipe.id} · equipment: {recipe.requiredEquipment} · {recipe.cookTimeMinutes:0} min";
                chipFlow.AddChild(chip);
            }
            _cookingStrip.AddChild(chipScroll);

            // ── Selected recipe truth ──
            string inputsText = selected.inputItems != null && selected.inputItems.Count > 0
                ? string.Join(", ", selected.inputItems.Select(i => $"{i.quantity}x {i.itemId}"))
                : "no ingredients";
            _cookingStrip.AddChild(AshfallUiHelpers.MakeMono(
                $"SELECTED: {selected.displayName} [{selected.id}] · {selected.cookTimeMinutes:0} min · {selected.requiredEquipment}"));
            _cookingStrip.AddChild(AshfallUiHelpers.MakeSmall(
                $"REQUIRES: {inputsText}  →  {selected.outputQuantity}x {selected.outputItemId}"));
            _cookingStrip.AddChild(AshfallUiHelpers.MakeSmall(
                $"EFFECT: −{selected.radiationRemoval * 100f:0}% radiation · +{selected.moraleBonus:0} morale · nutrition {selected.nutritionValue:0}"));
            bool hasInputs = _cooking.Source?.HasIngredients(selected.inputItems) ?? false;
            _cookingStrip.AddChild(AshfallUiHelpers.MakeDataRow(
                "Ingredients on hand", hasInputs ? "READY" : "MISSING",
                AshfallUiHelpers.ToColor(hasInputs ? DesignTheme.Lethe : DesignTheme.Hot)));

            // ── Verbs ──
            var actions = AshfallUiHelpers.MakeActionBar();
            actions.AddChild(AshfallUiHelpers.MakeButton("START COOK", () =>
            {
                string cookId = DefaultSurvivorResolver?.Invoke() ?? "cook_shelter";
                StartSelectedCooking(cookId);
            }));
            actions.AddChild(AshfallUiHelpers.MakeButton("ADVANCE 30 MIN", () => AdvanceCooking(30f)));
            _cookingStrip.AddChild(actions);

            // ── Active operations ──
            if (activeOps == 0)
            {
                _cookingStrip.AddChild(AshfallUiHelpers.MakeMetadata("No active cooking operations."));
            }
            else
            {
                foreach (var op in _cooking.System.State.activeOperations
                             .OrderBy(o => o.operationId, StringComparer.Ordinal))
                {
                    string name = _cooking.System.TryGetRecipe(op.recipeId, out var opRecipe) && opRecipe != null
                        ? opRecipe.displayName
                        : op.recipeId;
                    float pct = op.totalMinutesRequired > 0f
                        ? Mathf.Clamp(op.progressMinutes / op.totalMinutesRequired, 0f, 1f)
                        : 0f;
                    var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    var progress = AshfallUiHelpers.MakeMono(
                        $"{name} · {op.progressMinutes:0}/{op.totalMinutesRequired:0} min ({pct * 100f:0}%) · cook {op.assignedCookId}");
                    progress.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    row.AddChild(progress);
                    string opId = op.operationId;
                    row.AddChild(AshfallUiHelpers.MakeButton("CANCEL", () => CancelCooking(opId)));
                    _cookingStrip.AddChild(row);
                }
            }

            _cookingStrip.AddChild(AshfallUiHelpers.MakeMetadata(
                string.IsNullOrEmpty(_cookingFeedbackLine) ? "No cooking action yet." : _cookingFeedbackLine));
        }

        /// <summary>
        /// Starts the selected authored recipe through the bound
        /// <see cref="CookingHostSession"/> (inventory ingredients in, cooked items out).
        /// </summary>
        public ActionResult StartSelectedCooking(string cookId)
        {
            if (_cooking == null)
            {
                _cookingFeedbackLine = "Cannot cook: cooking authority not bound.";
                RefreshView();
                return ActionResult.Blocked("no_cooking_session", "cooking.session_offline");
            }
            if (!_cooking.System.TryGetRecipe(_selectedCookingRecipeId, out var recipe) || recipe == null)
            {
                _cookingFeedbackLine = $"Cannot cook: unknown recipe {_selectedCookingRecipeId}.";
                RefreshView();
                return ActionResult.Blocked("unknown_recipe", "cooking.unknown_recipe");
            }

            var result = _cooking.StartCooking(recipe.id, cookId, recipe.requiredEquipment);
            _cookingFeedbackLine = string.IsNullOrEmpty(_cooking.LastEvent)
                ? (result.IsSuccess
                    ? $"Cooking started: {recipe.displayName} ({cookId})."
                    : $"Cannot cook: {result.FailureCode}.")
                : _cooking.LastEvent;
            RefreshView();
            return result;
        }

        /// <summary>Advances every active cooking operation by <paramref name="minutes"/>.</summary>
        public int AdvanceCooking(float minutes)
        {
            if (_cooking == null)
            {
                _cookingFeedbackLine = "Cannot advance cooking: authority not bound.";
                RefreshView();
                return 0;
            }

            int completed = _cooking.ProgressCooking(minutes);
            _cookingFeedbackLine = string.IsNullOrEmpty(_cooking.LastEvent)
                ? (completed > 0
                    ? $"Completed {completed} cooking batch(es), delivered to the inventory."
                    : $"Cooking advanced {minutes:0} min.")
                : _cooking.LastEvent;
            RefreshView();
            return completed;
        }

        /// <summary>Cancels one active cooking operation by id.</summary>
        public ActionResult CancelCooking(string operationId)
        {
            if (_cooking == null)
            {
                _cookingFeedbackLine = "Cannot cancel: cooking authority not bound.";
                RefreshView();
                return ActionResult.Blocked("no_cooking_session", "cooking.session_offline");
            }

            var result = _cooking.CancelCooking(operationId);
            _cookingFeedbackLine = string.IsNullOrEmpty(_cooking.LastEvent)
                ? (result.IsSuccess
                    ? $"Cancelled cooking operation {operationId}."
                    : $"Cannot cancel: {result.FailureCode}.")
                : _cooking.LastEvent;
            RefreshView();
            return result;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                Visible = false;
                GetViewport().SetInputAsHandled();
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
