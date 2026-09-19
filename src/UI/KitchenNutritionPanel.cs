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

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }



        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            _shell = new AshfallDashboardShell("SYS: KITCHEN GALLEY & NUTRITION // DIETARY MATRIX", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("prep_jobs", "ACTIVE PREP", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
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
                cardVbox.AddThemeConstantOverride("separation", 3);
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
                selectBtn.CustomMinimumSize = new Vector2(0, 24);
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

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
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
