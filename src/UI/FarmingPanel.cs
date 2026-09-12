// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Farming;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Farming Panel (Plan 162, Advanced Agriculture).
/// Bound surface for AgricultureHostSession: plot strain layer, growing
/// medium, water quality, pests, compost, and harvest forecasts. Every
/// button routes a real command through Main.HandleAgricultureAction;
/// costs are shown on the buttons and failures return via LastEvent.
/// </summary>
public partial class FarmingPanel : Control
{
    public event Action? OnClose;
    /// <summary>Raised as (action, param). Param is a plot index or recipe id.</summary>
    public event Action<string, string>? OnActionRequested;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _grid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;

    private int _selectedPlot = -1;
    private string _filter = "all";
    private readonly List<int> _rowPlotIndices = new List<int>();
    private OptionButton? _strainPicker;

    private AgricultureHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(AgricultureHostSession session)
    {
        if (_host != null)
            _host.StateChanged -= RefreshView;
        _host = session;
        if (_host != null)
            _host.StateChanged += RefreshView;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Advanced Agriculture // Bench & Compost", minWidth: 1100, minHeight: 720);
        var root = _shell;
        AddChild(root);
        root.SetAnchorsPreset(LayoutPreset.FullRect);
        root.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        root.SizeFlagsVertical = SizeFlags.ExpandFill;

        var sidebarItems = new[]
        {
            new AshfallSidebar.Item { Id = "all",      Label = "All Plots",   Hint = "every bench plot", IconPath = "" },
            new AshfallSidebar.Item { Id = "planted",  Label = "Planted",     Hint = "crop in the ground", IconPath = "" },
            new AshfallSidebar.Item { Id = "mature",   Label = "Ready",       Hint = "ready for harvest", IconPath = "" },
            new AshfallSidebar.Item { Id = "infested", Label = "Infested",    Hint = "pests at work", IconPath = "" },
            new AshfallSidebar.Item { Id = "fallow",   Label = "Fallow",      Hint = "empty plots", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(sidebarItems, "Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("plots", "Plots", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
        _statusRail.AddCard("mature", "Ready", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
        _statusRail.AddCard("pests", "Infested", "—", AshfallMetricCard.Criticality.Caution, minWidth: 100);
        _statusRail.AddCard("compost", "Compost Ready", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
        _statusRail.AddCard("lights", "Grow Lights", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);

        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Plot",   MinWidth = 50,  Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Strain", MinWidth = 150, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Stage",  MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Growth", MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Water",  MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Medium", MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Tox ‰",  MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Pest",   MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _grid = new AshfallDataGrid(cols, showHeader: true, minWidth: 720, minHeight: 320);
        _grid.OnRowSelected += HandleRowSelected;

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_grid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(340, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _shell.SetContent(body);
        RefreshView();
    }

    private void HandleSidebar(string id)
    {
        _filter = id;
        _selectedPlot = -1;
        RefreshView();
    }

    private void HandleRowSelected(int idx)
    {
        // Selection carries the stable plot index, never the raw row position.
        _selectedPlot = idx >= 0 && idx < _rowPlotIndices.Count ? _rowPlotIndices[idx] : -1;
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildGridRows();
        RefreshDetail();
    }

    private static string StageName(GreenhouseStage stage) => stage switch
    {
        GreenhouseStage.Fallow => "fallow",
        GreenhouseStage.Sprouting => "sprouting",
        GreenhouseStage.Growing => "growing",
        GreenhouseStage.Mature => "READY",
        GreenhouseStage.Failed => "failed",
        _ => "?"
    };

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("plots", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("mature", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("pests", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("compost", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("lights", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        var gh = _host.System.Greenhouse;
        int plots = gh.Plots.Count;
        int mature = 0, pests = 0, planted = 0;
        foreach (var p in gh.Plots)
        {
            if (p == null || GreenhouseSystem.IsFallow(p)) continue;
            planted++;
            if (p.stage == (int)GreenhouseStage.Mature) mature++;
        }
        foreach (var ap in _host.System.State.plots)
            if (ap != null && ap.pest_severity > 0f) pests++;

        int compostReady = 0;
        var day = _host.System.LastTickDay;
        foreach (var r in _host.System.Catalog.compost_recipes)
        {
            if (r != null && _host.System.IsCompostReady(r.id, day))
                compostReady++;
        }

        _statusRail.Set("plots", $"{planted}/{plots}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("mature", $"{mature}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("pests", $"{pests}", pests > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("compost", compostReady > 0 ? $"{compostReady} batch(es)" : "none",
            compostReady > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("lights", "see power grid", AshfallMetricCard.Criticality.Normal);
    }

    private bool FilterPass(Ashfall.Core.GreenhousePlotState ghPlot, Ashfall.Core.Farming.AgriPlotState? agri)
    {
        bool fallow = GreenhouseSystem.IsFallow(ghPlot);
        return _filter switch
        {
            "planted" => !fallow,
            "mature" => !fallow && ghPlot.stage == (int)GreenhouseStage.Mature,
            "infested" => agri != null && agri.pest_severity > 0f,
            "fallow" => fallow,
            _ => true
        };
    }

    private void BuildGridRows()
    {
        if (_grid == null) return;
        _rowPlotIndices.Clear();

        if (_host == null)
        {
            _grid.SetRows(new List<AshfallDataGrid.Row>());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        var gh = _host.System.Greenhouse;
        for (int i = 0; i < gh.Plots.Count; i++)
        {
            var gp = gh.Plots[i];
            if (gp == null) continue;
            var ap = PlotOf(i);
            if (!FilterPass(gp, ap)) continue;

            string strain = ap != null && !string.IsNullOrEmpty(ap.strain_id)
                ? ap.strain_id.Replace("strain_", "").Replace('_', ' ')
                : (GreenhouseSystem.IsFallow(gp) ? "—" : "field crop");

            var stage = (GreenhouseStage)gp.stage;
            var stageCell = stage == GreenhouseStage.Mature ? AshfallDataGrid.CellState.Positive
                : stage == GreenhouseStage.Failed ? AshfallDataGrid.CellState.Critical
                : GreenhouseSystem.IsFallow(gp) ? AshfallDataGrid.CellState.Muted
                : AshfallDataGrid.CellState.Normal;

            var pestCell = ap != null && ap.pest_severity > 0f
                ? AshfallDataGrid.CellState.Warning
                : AshfallDataGrid.CellState.Muted;

            var toxCell = ap != null && ap.toxicity_permille >= 500
                ? AshfallDataGrid.CellState.Caution
                : AshfallDataGrid.CellState.Normal;

            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new($"{i + 1}", AshfallDataGrid.CellState.Normal),
                    new(strain, GreenhouseSystem.IsFallow(gp) ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
                    new(StageName(stage), stageCell),
                    new(GreenhouseSystem.IsFallow(gp) ? "—" : $"{gp.growth:F0}%", stageCell),
                    new($"{gp.water:F0}", gp.water <= 12f ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Normal),
                    new(ap != null ? $"{ap.medium_quality:F0}" : "—", ap != null && ap.medium_quality < 40f ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Normal),
                    new(ap != null ? $"{ap.toxicity_permille}" : "—", toxCell),
                    new(ap != null && ap.pest_severity > 0f ? $"{ap.pest_id.Replace("infestation_", "").Replace('_', ' ')} {ap.pest_severity * 100f:F0}%" : "clear", pestCell),
                },
                Selectable = true
            });
            _rowPlotIndices.Add(i);
        }

        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no plots match filter —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        _grid.SetRows(rows);
    }

    private Ashfall.Core.Farming.AgriPlotState? PlotOf(int plotIndex)
    {
        foreach (var p in _host!.System.State.plots)
            if (p != null && p.plot_index == plotIndex) return p;
        return null;
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        _strainPicker = null;
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("PLOT DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        var separator = AshfallUiHelpers.MakeSeparator();
        separator.CustomMinimumSize = new Vector2(0, 2);
        _detailBox.AddChild(separator);

        if (_host == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("System offline. Bind an AgricultureHostSession."));
            return;
        }

        BuildCompostSection();

        if (_selectedPlot < 0 || _selectedPlot >= _host.System.Greenhouse.Plots.Count)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a plot to view its bench state and commands."));
            return;
        }

        var gp = _host.System.Greenhouse.Plots[_selectedPlot];
        var ap = PlotOf(_selectedPlot);
        bool fallow = GreenhouseSystem.IsFallow(gp);
        var stage = (GreenhouseStage)gp.stage;

        _detailTitle.Text = $"PLOT {_selectedPlot + 1} — {StageName(stage).ToUpperInvariant()}";

        var strainDef = _host.System.EffectiveStrain(_selectedPlot);
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Strain",
            strainDef != null ? strainDef.display_name : (fallow ? "none" : "field crop"),
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        if (!fallow)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Growth", $"{gp.growth:F0}% / water {gp.water:F0}",
                gp.water <= 12f ? AshfallUiHelpers.ToColor(DesignTheme.LetheAmber) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            if (ap != null)
            {
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Medium", $"{ap.medium_quality:F0} / tox {ap.toxicity_permille}‰",
                    ap.toxicity_permille >= 500 ? AshfallUiHelpers.ToColor(DesignTheme.LetheAmber) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Pests",
                    ap.pest_severity > 0f ? $"{ap.pest_id.Replace("infestation_", "").Replace('_', ' ')} ({ap.pest_severity * 100f:F0}%)" : "clear",
                    ap.pest_severity > 0f ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                // Mutation is shown only once determined at maturity — the roll
                // itself stays hidden (plan §5.25).
                if (stage == GreenhouseStage.Mature && (AgriMutationOutcome)ap.mutation_outcome != AgriMutationOutcome.None)
                    _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Mutation",
                        ((AgriMutationOutcome)ap.mutation_outcome).ToString(),
                        AshfallUiHelpers.ToColor(DesignTheme.Entropy)));
                var forecast = _host.System.ForecastYield(_selectedPlot, AgricultureEnvironmentSnapshot.Default());
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Forecast (full light)",
                    $"{forecast.FinalYield} @ {forecast.QualityTier}",
                    AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
            }
        }

        if (!string.IsNullOrEmpty(_host.LastEvent))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
        }

        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("BENCH COMMANDS"));
        var actionRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

        if (fallow)
        {
            var picker = new OptionButton();
            foreach (var s in _host.System.Catalog.strains)
            {
                if (s == null) continue;
                picker.AddItem(s.display_name);
                picker.SetItemMetadata(picker.ItemCount - 1, s.id);
            }
            picker.CustomMinimumSize = new Vector2(190, 30);
            if (picker.ItemCount > 0) picker.Selected = 0;
            _strainPicker = picker;
            actionRow.AddChild(picker);

            var plantBtn = AshfallUiHelpers.MakeButton("PLANT (1 seed)", () =>
            {
                string strainId = "strain_tuber_heirloom";
                if (_strainPicker != null && _strainPicker.Selected >= 0)
                {
                    string picked = _strainPicker.GetItemMetadata(_strainPicker.Selected).AsString();
                    if (!string.IsNullOrEmpty(picked)) strainId = picked;
                }
                OnActionRequested?.Invoke("PLANT", $"{_selectedPlot}|{strainId}");
            }, disabled: picker.ItemCount == 0);
            plantBtn.CustomMinimumSize = new Vector2(130, 30);
            actionRow.AddChild(plantBtn);
        }
        else
        {
            var waterBtn = AshfallUiHelpers.MakeButton("WATER (5 clean)",
                () => OnActionRequested?.Invoke("WATER_CLEAN", $"{_selectedPlot}"));
            waterBtn.CustomMinimumSize = new Vector2(140, 30);
            actionRow.AddChild(waterBtn);

            var taintBtn = AshfallUiHelpers.MakeButton("WATER (5 tainted)",
                () => OnActionRequested?.Invoke("WATER_TAINTED", $"{_selectedPlot}"));
            taintBtn.CustomMinimumSize = new Vector2(150, 30);
            actionRow.AddChild(taintBtn);

            if (ap != null && ap.pest_severity > 0f)
            {
                var treatBtn = AshfallUiHelpers.MakeButton("TREAT (1 dust)",
                    () => OnActionRequested?.Invoke("TREAT", $"{_selectedPlot}"));
                treatBtn.CustomMinimumSize = new Vector2(120, 30);
                actionRow.AddChild(treatBtn);
            }

            if (stage == GreenhouseStage.Mature)
            {
                var harvestBtn = AshfallUiHelpers.MakeButton("HARVEST",
                    () => OnActionRequested?.Invoke("HARVEST", $"{_selectedPlot}"));
                harvestBtn.CustomMinimumSize = new Vector2(100, 30);
                harvestBtn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
                actionRow.AddChild(harvestBtn);
            }

            var clearBtn = AshfallUiHelpers.MakeButton("CLEAR",
                () => OnActionRequested?.Invoke("CLEAR", $"{_selectedPlot}"));
            clearBtn.CustomMinimumSize = new Vector2(80, 30);
            actionRow.AddChild(clearBtn);
        }

        _detailBox.AddChild(actionRow);
    }

    private void BuildCompostSection()
    {
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("COMPOST"));
        foreach (var r in _host!.System.Catalog.compost_recipes)
        {
            if (r == null) continue;
            bool ready = _host.System.IsCompostReady(r.id, _host.System.LastTickDay);
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            row.AddChild(AshfallUiHelpers.MakeSmall(
                $"{r.display_name}: {r.input_count}× {r.input_item_id} → {r.output_count}× {r.output_item_id} ({r.duration_days}d)"));
            var btn = AshfallUiHelpers.MakeButton(ready ? "COLLECT" : "START",
                () => OnActionRequested?.Invoke(ready ? "COMPOST_COLLECT" : "COMPOST_START", r.id));
            btn.CustomMinimumSize = new Vector2(90, 28);
            row.AddChild(btn);
            _detailBox.AddChild(row);
        }
        if (_selectedPlot >= 0)
        {
            var applyRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            applyRow.AddChild(AshfallUiHelpers.MakeSmall($"Apply 1× {_host.System.CompostRecipe("recipe_compost_humus")?.output_item_id ?? "item_compost_humus"} to plot {_selectedPlot + 1}:"));
            var applyBtn = AshfallUiHelpers.MakeButton("APPLY",
                () => OnActionRequested?.Invoke("COMPOST_APPLY", $"{_selectedPlot}"));
            applyBtn.CustomMinimumSize = new Vector2(80, 28);
            applyRow.AddChild(applyBtn);
            _detailBox.AddChild(applyRow);
        }
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
    }

    public void Open()
    {
        Visible = true;
        RefreshView();
        QueueRedraw();
    }

    public void Close()
    {
        Visible = false;
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

    public void Unbind()
    {
        if (_host != null)
        {
            _host.StateChanged -= RefreshView;
            _host = null;
        }
        RefreshView();
    }

    public override void _ExitTree()
    {
        Unbind();
        base._ExitTree();
    }
}
