using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Pharmaceutical Laboratory & Compounding Panel.
    /// Exposes chemical distillation, purity analysis, and drug formulation state machines.
    /// Thin presentation layer — all chemistry and dependency logic lives in Core.
    /// </summary>
    public partial class PharmaLabPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        public bool IsBound => _pharma != null;

        private PharmaLabSystem? _pharma;
        private Ashfall.Core.Inventory.Inventory? _inventory;
        private ChemicalDependencySystem? _chemicalDependency;
        private SurvivorsHostSession? _survivors;

        private VBoxContainer _recipeListContainer = null!;
        private VBoxContainer _detailContainer = null!;
        private Label _labStatusHeader = null!;
        private ProgressBar _distillationProgressBar = null!;
        private Label _phaseMetricsLabel = null!;
        private Button _cancelBatchButton = null!;

        private string _selectedRecipeId = string.Empty;
        private string _categoryFilter = "all";

        public void Bind(
            PharmaLabSystem pharma,
            Ashfall.Core.Inventory.Inventory inventory,
            ChemicalDependencySystem? chemicalDependency = null,
            SurvivorsHostSession? survivors = null)
        {
            _pharma = pharma;
            _inventory = inventory;
            _chemicalDependency = chemicalDependency;
            _survivors = survivors;

            _pharma.OnPharmaStateChanged -= RefreshView;
            _pharma.OnPharmaStateChanged += RefreshView;

            RefreshView();
        }

        public void Unbind()
        {
            if (_pharma != null)
            {
                _pharma.OnPharmaStateChanged -= RefreshView;
            }
            _pharma = null;
            _inventory = null;
            _chemicalDependency = null;
            _survivors = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            var root = new PanelContainer();
            root.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(root);

            var mainVBox = new VBoxContainer();
            mainVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            root.AddChild(mainVBox);

            // Header
            var headerHBox = new HBoxContainer();
            headerHBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var title = new Label();
            title.Text = "PHARMACEUTICAL LABORATORY // CHEMICAL COMPOUNDING & DISTILLATION";
            title.AddThemeFontSizeOverride("font_size", 20);
            title.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
            headerHBox.AddChild(title);

            headerHBox.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

            var closeBtn = new Button();
            closeBtn.Text = " [X] CLOSE ";
            closeBtn.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            headerHBox.AddChild(closeBtn);

            mainVBox.AddChild(headerHBox);

            // Live State Machine Progress Card
            var statusCard = new PanelContainer();
            statusCard.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var statusBox = new VBoxContainer();
            statusBox.AddThemeConstantOverride("separation", 6);
            statusCard.AddChild(statusBox);

            _labStatusHeader = new Label();
            _labStatusHeader.Text = "LAB STATUS: IDLE";
            _labStatusHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            statusBox.AddChild(_labStatusHeader);

            _distillationProgressBar = new ProgressBar();
            _distillationProgressBar.MinValue = 0;
            _distillationProgressBar.MaxValue = 100;
            _distillationProgressBar.CustomMinimumSize = new Vector2(0, 18);
            statusBox.AddChild(_distillationProgressBar);

            var footerHBox = new HBoxContainer();
            _phaseMetricsLabel = new Label();
            _phaseMetricsLabel.Text = "No active distillation or compounding run.";
            _phaseMetricsLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            footerHBox.AddChild(_phaseMetricsLabel);

            _cancelBatchButton = new Button();
            _cancelBatchButton.Text = " ABORT BATCH ";
            _cancelBatchButton.Pressed += OnCancelBatchClicked;
            footerHBox.AddChild(_cancelBatchButton);

            statusBox.AddChild(footerHBox);
            mainVBox.AddChild(statusCard);

            // Categories bar
            var filterHBox = new HBoxContainer();
            filterHBox.AddThemeConstantOverride("separation", 6);

            string[] categories = { "all", "chelator", "psychotropic", "stimulant", "emergency", "anesthetic", "antibiotic", "antiseptic" };
            foreach (var cat in categories)
            {
                var btn = new Button();
                btn.Text = $" [{cat.ToUpperInvariant()}] ";
                btn.Pressed += () =>
                {
                    _categoryFilter = cat;
                    RefreshView();
                };
                filterHBox.AddChild(btn);
            }
            mainVBox.AddChild(filterHBox);

            // Two-column layout
            var bodyHBox = new HBoxContainer();
            bodyHBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            bodyHBox.SizeFlagsVertical = SizeFlags.ExpandFill;
            bodyHBox.AddThemeConstantOverride("separation", DesignTheme.SpacingLg);

            var leftScroll = new ScrollContainer();
            leftScroll.CustomMinimumSize = new Vector2(400, 0);
            leftScroll.SizeFlagsVertical = SizeFlags.ExpandFill;

            _recipeListContainer = new VBoxContainer();
            _recipeListContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _recipeListContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftScroll.AddChild(_recipeListContainer);
            bodyHBox.AddChild(leftScroll);

            var rightScroll = new ScrollContainer();
            rightScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailContainer = new VBoxContainer();
            _detailContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _detailContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            rightScroll.AddChild(_detailContainer);
            bodyHBox.AddChild(rightScroll);

            mainVBox.AddChild(bodyHBox);

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_recipeListContainer == null || _detailContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_recipeListContainer);
            AshfallUiHelpers.EmptyChildren(_detailContainer);

            if (_pharma == null || _inventory == null)
            {
                _recipeListContainer.AddChild(AshfallUiHelpers.MakeMetadata("No pharma lab session bound."));
                return;
            }

            // Update Progress & Status Header
            bool processing = _pharma.IsProcessing;
            _cancelBatchButton.Visible = processing;

            if (processing)
            {
                var state = _pharma.State;
                _labStatusHeader.Text = $"LAB STATUS: RUNNING // PHASE: {state.currentPhase} // {state.currentRecipeId.ToUpperInvariant()}";
                _labStatusHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));

                float pct = state.hoursRequired > 0f ? (state.progressHours / state.hoursRequired) * 100f : 0f;
                _distillationProgressBar.Value = Math.Clamp(pct, 0f, 100f);

                string chemist = !string.IsNullOrEmpty(state.assignedChemistId) ? state.assignedChemistId : "Chemist";
                _phaseMetricsLabel.Text = $"Chemist: {chemist} | Temp: {state.temperature:F1}°C | Progress: {state.progressHours:F1}h / {state.hoursRequired:F1}h ({pct:F0}%)";
            }
            else
            {
                _labStatusHeader.Text = "LAB STATUS: IDLE";
                _labStatusHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
                _distillationProgressBar.Value = 0;
                _phaseMetricsLabel.Text = "Distillation apparatus clean. Select a pharmaceutical recipe below.";
            }

            // Filter & list recipes
            var recipes = _pharma.Recipes.Values
                .Where(r => _categoryFilter == "all" || string.Equals(r.category, _categoryFilter, StringComparison.OrdinalIgnoreCase))
                .OrderBy(r => r.recipe_id)
                .ToList();

            if (recipes.Count == 0)
            {
                _recipeListContainer.AddChild(AshfallUiHelpers.MakeMetadata("No formulas match the current category."));
            }
            else
            {
                if (string.IsNullOrEmpty(_selectedRecipeId) || !_pharma.Recipes.ContainsKey(_selectedRecipeId))
                {
                    _selectedRecipeId = recipes[0].recipe_id;
                }

                foreach (var r in recipes)
                {
                    bool isSelected = r.recipe_id == _selectedRecipeId;
                    _recipeListContainer.AddChild(MakeRecipeCard(r, isSelected));
                }
            }

            // Render selected recipe details
            if (!string.IsNullOrEmpty(_selectedRecipeId) && _pharma.Recipes.TryGetValue(_selectedRecipeId, out var selected))
            {
                RenderRecipeDetail(selected);
            }
        }

        private Control MakeRecipeCard(PharmaRecipe recipe, bool isSelected)
        {
            var card = new PanelContainer();
            card.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var hbox = new HBoxContainer();
            hbox.AddThemeConstantOverride("separation", 8);
            card.AddChild(hbox);

            var selectBtn = new Button();
            selectBtn.Text = $"[{recipe.category.ToUpperInvariant()}] {recipe.display_name} -> {recipe.output_amount}x {recipe.output_item_id}";
            selectBtn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            if (isSelected)
            {
                selectBtn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
            }

            selectBtn.Pressed += () =>
            {
                _selectedRecipeId = recipe.recipe_id;
                RefreshView();
            };

            hbox.AddChild(selectBtn);
            return card;
        }

        private void RenderRecipeDetail(PharmaRecipe recipe)
        {
            var title = new Label();
            title.Text = recipe.display_name;
            title.AddThemeFontSizeOverride("font_size", 18);
            title.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
            _detailContainer.AddChild(title);

            var yieldLabel = new Label();
            yieldLabel.Text = $"Yield: {recipe.output_amount}x {recipe.output_item_id} | Class: {recipe.category.ToUpperInvariant()}";
            yieldLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _detailContainer.AddChild(yieldLabel);

            // Reagents Section
            var reagentsBox = new VBoxContainer();
            reagentsBox.AddChild(AshfallUiHelpers.MakeSectionHeader("CHEMICAL REAGENTS & PRECURSORS"));

            bool canCompound = true;
            for (int i = 0; i < recipe.input_ids.Count; i++)
            {
                string inputId = recipe.input_ids[i];
                int requiredAmt = i < recipe.input_amounts.Count ? recipe.input_amounts[i] : 1;
                int held = _inventory != null ? _inventory.CountById(inputId) : 0;
                bool hasEnough = held >= requiredAmt;
                if (!hasEnough) canCompound = false;

                var reqLabel = new Label();
                reqLabel.Text = $"  • {inputId}: {held}/{requiredAmt} {(hasEnough ? "[IN STOCK]" : "[DEFICIENT]")}";
                reqLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(hasEnough ? DesignTheme.Success : DesignTheme.Critical));
                reagentsBox.AddChild(reqLabel);
            }

            _detailContainer.AddChild(reagentsBox);

            // Reaction Parameters
            var paramsBox = new VBoxContainer();
            paramsBox.AddChild(AshfallUiHelpers.MakeSectionHeader("DISTILLATION & REACTION PARAMETERS"));
            paramsBox.AddChild(AshfallUiHelpers.MakeMetadata($"  • Base Compounding Time: {recipe.base_hours:F1} hours"));
            paramsBox.AddChild(AshfallUiHelpers.MakeMetadata($"  • Optimal Reaction Temperature: {recipe.required_temperature:F0}°C"));
            paramsBox.AddChild(AshfallUiHelpers.MakeMetadata($"  • Standard Purity Target: {(recipe.purity_target * 100f):F0}%"));

            if (recipe.dependency_risk > 0f)
            {
                var warnLabel = new Label();
                warnLabel.Text = $"  • WARNING: Addiction / Contamination Risk: {(recipe.dependency_risk * 100f):F0}%";
                warnLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
                paramsBox.AddChild(warnLabel);
            }
            else
            {
                paramsBox.AddChild(AshfallUiHelpers.MakeMetadata("  • Contamination / Addiction Risk: 0% (Safe formulation)"));
            }

            _detailContainer.AddChild(paramsBox);

            // Action Box
            var actionBox = new HBoxContainer();
            actionBox.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);

            bool isProcessing = _pharma != null && _pharma.IsProcessing;

            var startBtn = new Button();
            startBtn.Text = " START COMPOUNDING RUN ";
            startBtn.Disabled = isProcessing || !canCompound;
            startBtn.Pressed += () =>
            {
                string chemist = GetBestChemist();
                _pharma?.StartBatch(recipe.recipe_id, chemist);
                RefreshView();
            };
            actionBox.AddChild(startBtn);

            _detailContainer.AddChild(actionBox);
        }

        private string GetBestChemist()
        {
            if (_survivors?.Roster?.Roster != null)
            {
                foreach (var entry in _survivors.Roster.Roster)
                {
                    if (entry != null && entry.isAlive) return entry.survivorId;
                }
            }
            return "survivor_medic";
        }

        private void OnCancelBatchClicked()
        {
            _pharma?.CancelBatch();
            RefreshView();
        }
    }
}
