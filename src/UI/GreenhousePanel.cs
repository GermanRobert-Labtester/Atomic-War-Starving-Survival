using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Tactile 9-slice panel for THE GLASS ORCHARD (Greenhouse / Sub-surface Hydroponics).
    /// Presents real-time cultivation bed status, irrigation management, blight control, and harvesting.
    /// </summary>
    public partial class GreenhousePanel : Control
    {
        public event Action? OnClose;

        private GreenhouseHostSession? _host;
        private Label _lblSummary = null!;
        private Label _lblStatusMsg = null!;
        private VBoxContainer _plotsContainer = null!;

        public bool IsBound => _host != null;

        public void Bind(GreenhouseHostSession session)
        {
            _host = session;
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.03f, 0.04f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(960, 680);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
            panel.AddChild(margins);

            var rootVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            margins.AddChild(rootVBox);

            // ── Header ──
            var header = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("THE GLASS ORCHARD // SUB-SURFACE HYDROPONICS", DesignTheme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            rootVBox.AddChild(header);

            rootVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Summary Strip ──
            _lblSummary = AshfallUiHelpers.MakeMono("PLOTS: 0/4 ACTIVE · HARVESTS: 0 · PRE-WAR SEED VAULT: SEALED");
            _lblSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            rootVBox.AddChild(_lblSummary);

            rootVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Scrollable Plots List ──
            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(920, 480),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            rootVBox.AddChild(scroll);

            _plotsContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            _plotsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(_plotsContainer);

            rootVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Footer Status Log ──
            _lblStatusMsg = AshfallUiHelpers.MakeSmall("System ready. Select a soil bed to manage irrigation or seeding.");
            _lblStatusMsg.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
            rootVBox.AddChild(_lblStatusMsg);

            RefreshView();
        }

        public void RefreshView()
        {
            if (_plotsContainer == null) return;

            while (_plotsContainer.GetChildCount() > 0)
                _plotsContainer.RemoveChild(_plotsContainer.GetChild(0));

            if (_host == null)
            {
                _lblSummary.Text = "GREENHOUSE ENGINE OFFLINE // NO SESSION BOUND";
                _plotsContainer.AddChild(AshfallUiHelpers.MakeMetadata("Greenhouse system waiting for host binding."));
                return;
            }

            var state = _host.System.State;
            int activePlots = 0;
            for (int i = 0; i < state.plots.Count; i++)
            {
                if (!GreenhouseSystem.IsFallow(state.plots[i]))
                    activePlots++;
            }

            _lblSummary.Text = $"PLOTS: {activePlots}/{state.plots.Count} ACTIVE · HARVESTS: {state.totalHarvests} · SEED VAULT: {(state.preWarWheatUnlocked ? "OPEN" : "SEALED")}";
            if (!string.IsNullOrEmpty(_host.LastEvent))
                _lblStatusMsg.Text = _host.LastEvent;

            for (int i = 0; i < state.plots.Count; i++)
            {
                var plot = state.plots[i];
                var card = BuildPlotCard(i, plot);
                _plotsContainer.AddChild(card);
            }
        }

        private Control BuildPlotCard(int plotIndex, GreenhousePlotState plot)
        {
            var cardPanel = AshfallUiHelpers.MakePanel(900, 105);
            var margins = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            cardPanel.AddChild(margins);

            var hbox = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
            hbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            margins.AddChild(hbox);

            // Left column: Bed info and Stage
            var leftVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            leftVBox.CustomMinimumSize = new Vector2(240, 85);

            string seedName = string.IsNullOrEmpty(plot.seedItemId) ? "UNPLANTED / FALLOW" : FormatCropName(plot.seedItemId);
            var title = AshfallUiHelpers.MakeMono($"BED #{plotIndex + 1}: {seedName.ToUpperInvariant()}");
            leftVBox.AddChild(title);

            var stageEnum = (GreenhouseStage)plot.stage;
            var (badgeColor, badgeText) = GetStageDisplay(stageEnum);
            var badge = AshfallUiHelpers.MakeSmall($"STATUS: [{badgeText}]");
            badge.AddThemeColorOverride("font_color", badgeColor);
            leftVBox.AddChild(badge);

            if (plot.blight > 0f)
            {
                var blightLbl = AshfallUiHelpers.MakeSmall($"! BLIGHT LEVEL: {plot.blight * 100f:0}% !");
                blightLbl.AddThemeColorOverride("font_color", new Color(0.95f, 0.35f, 0.25f));
                leftVBox.AddChild(blightLbl);
            }
            hbox.AddChild(leftVBox);

            // Middle column: Gauges
            var midVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            midVBox.CustomMinimumSize = new Vector2(300, 85);

            midVBox.AddChild(AshfallUiHelpers.MakeDataRow("Growth Progress", $"{plot.growth:0.0}%", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            midVBox.AddChild(AshfallUiHelpers.MakeDataRow("Moisture Level", $"{plot.water:0.0} / 100", plot.water < 15f ? new Color(0.9f, 0.4f, 0.3f) : AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
            midVBox.AddChild(AshfallUiHelpers.MakeDataRow("Soil Contamination", $"{plot.soilContamination:0.0} mSv", plot.soilContamination > 40f ? new Color(0.9f, 0.4f, 0.3f) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            hbox.AddChild(midVBox);

            // Right column: Action buttons
            var rightVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            rightVBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            if (GreenhouseSystem.IsFallow(plot))
            {
                var plantRow1 = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
                var btnMushroom = AshfallUiHelpers.MakeButton("PLANT MUSHROOM", () =>
                {
                    _host?.Plant(plotIndex, GreenhouseExpansionCatalog.Items.SeedMushroom, 1);
                    RefreshView();
                });
                btnMushroom.CustomMinimumSize = new Vector2(140, 28);
                plantRow1.AddChild(btnMushroom);

                var btnTuber = AshfallUiHelpers.MakeButton("PLANT TUBER", () =>
                {
                    _host?.Plant(plotIndex, GreenhouseExpansionCatalog.Items.SeedTuber, 1);
                    RefreshView();
                });
                btnTuber.CustomMinimumSize = new Vector2(140, 28);
                plantRow1.AddChild(btnTuber);
                rightVBox.AddChild(plantRow1);

                var plantRow2 = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
                var btnGrain = AshfallUiHelpers.MakeButton("PLANT GRAIN", () =>
                {
                    _host?.Plant(plotIndex, GreenhouseExpansionCatalog.Items.SeedGrain, 1);
                    RefreshView();
                });
                btnGrain.CustomMinimumSize = new Vector2(140, 28);
                plantRow2.AddChild(btnGrain);

                if (_host?.System.IsPreWarWheatUnlocked == true)
                {
                    var btnWheat = AshfallUiHelpers.MakeButton("PRE-WAR WHEAT", () =>
                    {
                        _host?.Plant(plotIndex, GreenhouseExpansionCatalog.Items.SeedWheat, 1);
                        RefreshView();
                    });
                    btnWheat.CustomMinimumSize = new Vector2(140, 28);
                    plantRow2.AddChild(btnWheat);
                }
                rightVBox.AddChild(plantRow2);
            }
            else
            {
                var actRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
                var btnWaterClean = AshfallUiHelpers.MakeButton("+20 WATER (CLEAN)", () =>
                {
                    _host?.Water(plotIndex, 20f, tainted: false);
                    RefreshView();
                });
                btnWaterClean.CustomMinimumSize = new Vector2(140, 28);
                actRow.AddChild(btnWaterClean);

                var btnWaterTainted = AshfallUiHelpers.MakeButton("+20 WATER (TAINTED)", () =>
                {
                    _host?.Water(plotIndex, 20f, tainted: true);
                    RefreshView();
                });
                btnWaterTainted.CustomMinimumSize = new Vector2(140, 28);
                actRow.AddChild(btnWaterTainted);
                rightVBox.AddChild(actRow);

                var actRow2 = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
                if (stageEnum == GreenhouseStage.Mature)
                {
                    var btnHarvest = AshfallUiHelpers.MakeButton("★ HARVEST CROP ★", () =>
                    {
                        _host?.Harvest(plotIndex);
                        RefreshView();
                    });
                    btnHarvest.CustomMinimumSize = new Vector2(140, 28);
                    actRow2.AddChild(btnHarvest);
                }
                else if (plot.blight > 0f)
                {
                    var btnTreat = AshfallUiHelpers.MakeButton("TREAT BLIGHT", () =>
                    {
                        _host?.TreatBlight(plotIndex);
                        RefreshView();
                    });
                    btnTreat.CustomMinimumSize = new Vector2(140, 28);
                    actRow2.AddChild(btnTreat);
                }

                var btnClear = AshfallUiHelpers.MakeButton("CLEAR BED", () =>
                {
                    _host?.Clear(plotIndex);
                    RefreshView();
                });
                btnClear.CustomMinimumSize = new Vector2(100, 28);
                actRow2.AddChild(btnClear);
                rightVBox.AddChild(actRow2);
            }

            hbox.AddChild(rightVBox);
            return cardPanel;
        }

        private static string FormatCropName(string seedId) => seedId switch
        {
            GreenhouseExpansionCatalog.Items.SeedMushroom => "Cave Mushroom Spores",
            GreenhouseExpansionCatalog.Items.SeedTuber => "Hardy Frost Tuber",
            GreenhouseExpansionCatalog.Items.SeedGrain => "Winter Rye Grain",
            GreenhouseExpansionCatalog.Items.SeedWheat => "Pre-War Heritage Wheat",
            _ => seedId
        };

        private static (Color, string) GetStageDisplay(GreenhouseStage stage) => stage switch
        {
            GreenhouseStage.Fallow => (new Color(0.5f, 0.5f, 0.5f), "FALLOW / EMPTY"),
            GreenhouseStage.Sprouting => (new Color(0.85f, 0.75f, 0.35f), "SPROUTING"),
            GreenhouseStage.Growing => (new Color(0.45f, 0.75f, 0.45f), "GROWING"),
            GreenhouseStage.Mature => (new Color(0.35f, 0.95f, 0.45f), "READY TO HARVEST"),
            GreenhouseStage.Failed => (new Color(0.95f, 0.3f, 0.25f), "FAILED / DEAD"),
            _ => (new Color(0.5f, 0.5f, 0.5f), stage.ToString().ToUpperInvariant())
        };

        public void Open()
        {
            Visible = true;
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
