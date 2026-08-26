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
    public partial class KitchenNutritionPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _recipeList = null!;
        private VBoxContainer _prepStation = null!;
        private VBoxContainer _serviceLogContainer = null!;
        private Label _eventLogLabel = null!;

        private KitchenNutritionHostSession? _host;
        private string _selectedRecipeId = "recipe_fungal_stew";

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
            if (_host == null || _statusRail == null) return;

            AshfallUiHelpers.EmptyChildren(_recipeList);
            AshfallUiHelpers.EmptyChildren(_prepStation);
            AshfallUiHelpers.EmptyChildren(_serviceLogContainer);

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
                new { id = "recipe_fungal_stew", name = "Nutritive Fungal Stew", desc = "Hearty broth fortified with underground mushroom caps.", cost = "1x Mushroom, 1x Clean Water", morale = 3 },
                new { id = "recipe_canned_mash", name = "Heated Military Rations", desc = "Standard shelf-stable calories with mild vitamin paste.", cost = "1x Rations, 1x Fuel", morale = 2 },
                new { id = "recipe_greenhouse_salad", name = "Fresh Harvest Greens", desc = "Crisp hydroponic root leaves and tuber salad.", cost = "2x Fresh Greens", morale = 5 },
                new { id = "recipe_cured_jerky_broth", name = "Smoked Jerky & Bone Broth", desc = "Rich protein soup providing sustained work endurance.", cost = "1x Meat, 2x Clean Water", morale = 4 }
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
                headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_scurvy", 18));
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
            _prepStation.AddChild(AshfallUiHelpers.MakeDataRow("Scurvy Prevention", "High Vitamin C Equivalent", AshfallUiHelpers.ToColor(DesignTheme.Pale)));

            _prepStation.AddChild(AshfallUiHelpers.MakeSeparator());
            _prepStation.AddChild(AshfallUiHelpers.MakeSubsectionHeader("COOK ACTIONS"));

            var btnStartPrep = AshfallUiHelpers.MakeButton($"COOK {curRecipe.name.ToUpperInvariant()}", () =>
            {
                var reqs = new Dictionary<string, int> { { "clean_water", 1 }, { "canned_rations", 1 } };
                _host.StartPrepJob(curRecipe.id, "survivor_dweller_cook", reqs);
                RefreshView();
            });
            _prepStation.AddChild(btnStartPrep);

            var btnServe = AshfallUiHelpers.MakeButton("SERVE MEAL TO SURVIVORS", () =>
            {
                _host.ServeMeal("survivor_dweller_1", curRecipe.id);
                RefreshView();
            });
            _prepStation.AddChild(btnServe);

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
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
