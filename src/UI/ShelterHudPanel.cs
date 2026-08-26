using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Shelter HUD (#40 Stitch).
///
/// Sibling surface to GameDashboardPanel that exercises the new reusable
/// primitives (Shell + Sidebar + Status Rail + DataGrid) instead of bespoke
/// VBox containers, gauge rows, and footer widgets. Both surfaces consume the
/// same <see cref="DashboardSnapshot"/> presentation struct and read the
/// same authoritative host APIs in Main; this surface is the variant
/// engineered along the Phase 12 dashboard component architecture.
///
/// The live HUD continues to use GameDashboardPanel (unchanged). This panel
/// is the snapshot target that demonstrates the primitives compose into a
/// HUD-shaped surface without losing any of the existing data points.
///
/// Status wall widgets draw from:
///   • Day / Location — already a header strip
///   • Forecast + duty roster — written into a single-line rail card
///   • Stores watch (water / food / filter / scrap / medical) — DataGrid
///   • Condition report (health / rad / hunger / thirst) — DataGrid
///   • Air filtration (HEPA health, AQI, radon) — Status Rail
///
/// No new data fields are introduced. No fake metrics. No gizmos.
/// </summary>
public partial class ShelterHudPanel : Control
{
    public event Action? OnMenuRequested;
    public event Action? OnAdvanceDayRequested;
    public event Action? OnSaveRequested;
    public event Action<string>? OnOpenPanelRequested;
    public event Action? OnServiceFilterRequested;
    public event Action? OnReplaceFilterRequested;

    /// <summary>
    /// Identical snapshot shape to GameDashboardPanel.DashboardSnapshot.
    /// Carried verbatim so Main can call UpdateState on either panel with the
    /// same payload.
    /// </summary>
    public sealed class DashboardSnapshot
    {
        public int Day = 1;
        public int Health = 100;
        public int MaxHealth = 100;
        public float Radiation;
        public int Hunger;
        public int Thirst;
        public long Value;
        public string Weather = string.Empty;
        public string Location = "THE HOLDFAST";
        public float WeatherVisibility = 1f;
        public float OutdoorRadiation;
        public int LivingSurvivors;
        public int TotalSurvivors;
        public float AverageSurvivorHealth;
        public int CleanWater;
        public int Food;
        public int MedicalStock;
        public int FilterSpares = 1;
        public int MechanicalScrap = 6;
        public float AirFilterHealth = 100.0f;
        public float AirQuality = 100.0f;
        public float RadonLevel = 12.0f;
        public bool AirWarning;
        public string FilterDutyAssignee = "Dr. Sarah Chen";
        public string LastEvent = string.Empty;
        public List<Ashfall.Core.World.WeatherForecastEntry> Forecast = new();
        public Dictionary<string, string> DutyAssignments = new();
    }

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _storesGrid;
    private AshfallDataGrid? _conditionGrid;
    private Label _dayLabel = null!;
    private Label _locationLabel = null!;
    private Label _weatherLabel = null!;
    private Label _forecastLabel = null!;
    private Label _dutyRosterSummary = null!;
    private Label _directiveText = null!;
    private Label _eventLabel = null!;
    private Button _btnServiceFilter = null!;
    private Button _btnReplaceFilter = null!;
    private ProgressBar _airFilterBar = null!;
    private Label _airFilterValue = null!;

    public bool IsBound => _shell != null;

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);
        Visible = false;

        var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.06f, 0.92f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        _shell = new AshfallDashboardShell(
            "SHELTER HUD — BUNKER_OPERATIONS",
            1180, 720);

        var hostContainer = new MarginContainer();
        hostContainer.AddThemeConstantOverride("margin_left", DesignTheme.SpacingLg);
        hostContainer.AddThemeConstantOverride("margin_top", DesignTheme.SpacingLg);
        hostContainer.AddThemeConstantOverride("margin_right", DesignTheme.SpacingLg);
        hostContainer.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingLg);
        hostContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        hostContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        hostContainer.AddChild(_shell);
        AddChild(hostContainer);

        // Sidebar mirrors GameDashboard's rail taxonomy.
        _sidebar = _shell.SetSidebar(new[]
        {
            new AshfallSidebar.Item { Id = "overview",    Label = "Overview",            Hint = "DAY // LOCATION" },
            new AshfallSidebar.Item { Id = "status",      Label = "Status",              Hint = "OPERATIONS" },
            new AshfallSidebar.Item { Id = "survivors",   Label = "Survivors",           Hint = "ROSTER STATUS" },
            new AshfallSidebar.Item { Id = "inventory",   Label = "Inventory",           Hint = "STORAGE" },
            new AshfallSidebar.Item { Id = "crafting",    Label = "Crafting",            Hint = "WORKSTATION" },
            new AshfallSidebar.Item { Id = "medical",     Label = "Medical",             Hint = "TRIAGE" },
            new AshfallSidebar.Item { Id = "expeditions", Label = "Expeditions",         Hint = "CARAVAN" },
            new AshfallSidebar.Item { Id = "weather",     Label = "Weather",             Hint = "FORECAST" },
            new AshfallSidebar.Item { Id = "radio",       Label = "Radio",               Hint = "INTERCEPT" },
            new AshfallSidebar.Item { Id = "map",         Label = "Map",                 Hint = "SECTOR MAP" },
            new AshfallSidebar.Item { Id = "shelter",     Label = "Shelter",             Hint = "INTEGRITY" },
            new AshfallSidebar.Item { Id = "trade",       Label = "Trade",               Hint = "CARAVAN LEDGER" },
            new AshfallSidebar.Item { Id = "factions",    Label = "Factions",            Hint = "STANCES" },
            new AshfallSidebar.Item { Id = "verdict",     Label = "Verdict",             Hint = "EVALUATION" },
            new AshfallSidebar.Item { Id = "help",       Label = "Help & Controls",     Hint = "TUTORIAL" },
        }, "BUNKER OPS", "overview");

        if (_sidebar != null)
        {
            _sidebar.OnSelected += id =>
            {
                if (id == "overview") return;
                OnOpenPanelRequested?.Invoke(id);
            };
        }

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("day",      "DAY",     "—", AshfallMetricCard.Criticality.Normal, 90);
        _statusRail.AddCard("loc",      "LOCATION","—", AshfallMetricCard.Criticality.Normal, 180);
        _statusRail.AddCard("weather",  "WEATHER", "—", AshfallMetricCard.Criticality.Normal, 180);
        _statusRail.AddCard("hp",       "AVG HP",  "—%", AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddCard("rad",      "MAX RAD", "— mSv", AshfallMetricCard.Criticality.Normal, 130);
        _statusRail.AddCard("air",      "HEPA",    "—%",  AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddSeparator();
        _statusRail.AddCard("air_q",    "AIR Q",   "—%",  AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddCard("radon",    "RADON",   "— Bq/m³", AshfallMetricCard.Criticality.Normal, 130);

        _shell.AttachHeaderCloseButton("MENU [Esc]", () => OnMenuRequested?.Invoke());

        BuildContent();

        // Apply an empty snapshot by default so the surface ships inspectable.
        UpdateState(new DashboardSnapshot());
    }

    private void BuildContent()
    {
        var contentStack = new VBoxContainer();
        contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        contentStack.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        contentStack.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

        // Forecast + duty roster band
        var bandStack = new VBoxContainer();
        bandStack.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        bandStack.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        var bandTop = new HBoxContainer();
        bandTop.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        bandTop.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        _dayLabel = MakeMono("DAY —", DesignTheme.Hot);
        bandTop.AddChild(_dayLabel);
        _locationLabel = MakeMono("THE HOLDFAST", DesignTheme.Pale);
        bandTop.AddChild(_locationLabel);
        bandTop.AddChild(new Control { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill });
        _weatherLabel = MakeMono("WEATHER // UNREAD", DesignTheme.Lethe);
        bandTop.AddChild(_weatherLabel);
        bandStack.AddChild(bandTop);

        _forecastLabel = new Label
        {
            Text = "FORECAST // --",
            VerticalAlignment = VerticalAlignment.Center,
        };
        _forecastLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        _forecastLabel.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(DesignTheme.Hot));
        var mono = AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf");
        if (mono != null) _forecastLabel.AddThemeFontOverride("font", mono);
        _forecastLabel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        bandStack.AddChild(_forecastLabel);

        _dutyRosterSummary = new Label
        {
            Text = "DUTY ROSTER // --",
            VerticalAlignment = VerticalAlignment.Center,
        };
        _dutyRosterSummary.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        _dutyRosterSummary.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(DesignTheme.Warm));
        _dutyRosterSummary.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        bandStack.AddChild(_dutyRosterSummary);

        contentStack.AddChild(bandStack);

        // Two-column DataGrid area: stores + condition.
        var gridRow = new HBoxContainer();
        gridRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        gridRow.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        gridRow.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

        var storesCol = new VBoxContainer();
        storesCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        storesCol.SizeFlagsStretchRatio = 1.05f;
        storesCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        storesCol.AddChild(AshfallUiHelpers.MakeSectionHeader("STORES WATCH"));
        var storeCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Resource", MinWidth = 160, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Stock",    MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Status",   MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Center },
        };
        _storesGrid = new AshfallDataGrid(storeCols, showHeader: true, minWidth: 380, minHeight: 180);
        _storesGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        _storesGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        storesCol.AddChild(_storesGrid);

        var inventoryBtn = AshfallUiHelpers.MakeButton("OPEN INVENTORY",
            () => OnOpenPanelRequested?.Invoke("inventory"));
        inventoryBtn.CustomMinimumSize = new Vector2(0, 30);
        storesCol.AddChild(inventoryBtn);

        gridRow.AddChild(storesCol);

        var conditionCol = new VBoxContainer();
        conditionCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        conditionCol.SizeFlagsStretchRatio = 0.95f;
        conditionCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        conditionCol.AddChild(AshfallUiHelpers.MakeSectionHeader("CONDITION REPORT"));
        var condCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Driver", MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Value",  MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Status", MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Center },
        };
        _conditionGrid = new AshfallDataGrid(condCols, showHeader: true, minWidth: 380, minHeight: 180);
        _conditionGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        _conditionGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        conditionCol.AddChild(_conditionGrid);

        var shelterBtn = AshfallUiHelpers.MakeButton("OPEN SHELTER",
            () => OnOpenPanelRequested?.Invoke("shelter"));
        shelterBtn.CustomMinimumSize = new Vector2(0, 30);
        conditionCol.AddChild(shelterBtn);

        gridRow.AddChild(conditionCol);
        contentStack.AddChild(gridRow);

        // Footer area: directive + air filtration strips
        var footerRow = new HBoxContainer();
        footerRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        footerRow.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        // Directive card
        var directivePanel = AshfallUiHelpers.MakePanel();
        directivePanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        directivePanel.SizeFlagsStretchRatio = 1.4f;
        var directiveMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
        directivePanel.AddChild(directiveMargin);
        var directiveVBox = new VBoxContainer();
        directiveVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        directiveMargin.AddChild(directiveVBox);
        directiveVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("CURRENT DIRECTIVE"));
        _directiveText = AshfallUiHelpers.MakeBody("Keep the shelter quiet. Check the filter pressure before the next outdoor shift.");
        directiveVBox.AddChild(_directiveText);
        _eventLabel = new Label
        {
            Text = "No fresh signal.",
            VerticalAlignment = VerticalAlignment.Center,
        };
        _eventLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        _eventLabel.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(DesignTheme.Muted));
        _eventLabel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        directiveVBox.AddChild(_eventLabel);

        // Action row inside directive: advance day + save
        var actionSubRow = new HBoxContainer();
        actionSubRow.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        var advance = AshfallUiHelpers.MakeButton("ADVANCE TO NEXT DAY",
            () => OnAdvanceDayRequested?.Invoke());
        advance.CustomMinimumSize = new Vector2(220, 34);
        actionSubRow.AddChild(advance);
        var save = AshfallUiHelpers.MakeButton("SAVE LEDGER", () => OnSaveRequested?.Invoke());
        save.CustomMinimumSize = new Vector2(140, 34);
        actionSubRow.AddChild(save);
        directiveVBox.AddChild(actionSubRow);
        footerRow.AddChild(directivePanel);

        // Air filtration card
        var airPanel = AshfallUiHelpers.MakePanel();
        airPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        airPanel.SizeFlagsStretchRatio = 0.95f;
        var airMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
        airPanel.AddChild(airMargin);
        var airVBox = new VBoxContainer();
        airVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        airMargin.AddChild(airVBox);
        airVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("AIR FILTRATION"));
        BuildAirGaugeRow(airVBox);
        var airActions = new HBoxContainer();
        airActions.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _btnServiceFilter = AshfallUiHelpers.MakeButton("SERVICE FILTER (-1 SCRAP)",
            () => OnServiceFilterRequested?.Invoke());
        _btnServiceFilter.CustomMinimumSize = new Vector2(0, 30);
        _btnServiceFilter.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        airActions.AddChild(_btnServiceFilter);
        _btnReplaceFilter = AshfallUiHelpers.MakeButton("REPLACE HEPA (-1 SPARE)",
            () => OnReplaceFilterRequested?.Invoke());
        _btnReplaceFilter.CustomMinimumSize = new Vector2(0, 30);
        _btnReplaceFilter.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        airActions.AddChild(_btnReplaceFilter);
        airVBox.AddChild(airActions);
        footerRow.AddChild(airPanel);

        contentStack.AddChild(footerRow);
        _shell.SetContent(contentStack);
    }

    private void BuildAirGaugeRow(VBoxContainer host)
    {
        var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
        _airFilterBar = new ProgressBar
        {
            MinValue = 0,
            MaxValue = 100,
            Value = 0,
            ShowPercentage = false,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            CustomMinimumSize = new Vector2(120, 12),
        };
        _airFilterBar.AddThemeStyleboxOverride("background",
            AshfallUiHelpers.MakeFlatBg(
                new Color(DesignTheme.Ink.r, DesignTheme.Ink.g, DesignTheme.Ink.b, 0.9f),
                AshfallUiHelpers.ToColor(DesignTheme.LineSoft), 1, DesignTheme.RadiusSm));
        _airFilterBar.AddThemeStyleboxOverride("fill",
            AshfallUiHelpers.MakeFlatBg(
                new Color(DesignTheme.Entropy.r, DesignTheme.Entropy.g, DesignTheme.Entropy.b, 0.92f),
                null, 0, DesignTheme.RadiusSm));
        row.AddChild(_airFilterBar);

        _airFilterValue = AshfallUiHelpers.MakeMono("--%");
        _airFilterValue.CustomMinimumSize = new Vector2(80, 0);
        _airFilterValue.HorizontalAlignment = HorizontalAlignment.Right;
        row.AddChild(_airFilterValue);
        host.AddChild(row);
    }

    private static Label MakeMono(string text, (float r, float g, float b, float a) color)
    {
        var lbl = new Label
        {
            Text = text,
            VerticalAlignment = VerticalAlignment.Center,
        };
        lbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeMono);
        lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(color));
        var mono = AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf");
        if (mono != null) lbl.AddThemeFontOverride("font", mono);
        return lbl;
    }

    public void UpdateState(DashboardSnapshot state)
    {
        if (state == null) return;
        if (_dayLabel != null)
            _dayLabel.Text = $"DAY {state.Day:00}";
        if (_locationLabel != null)
            _locationLabel.Text = (string.IsNullOrWhiteSpace(state.Location) ? "THE HOLDFAST" : state.Location)
                .Replace('_', ' ').ToUpperInvariant();

        if (_weatherLabel != null)
        {
            string weather = string.IsNullOrWhiteSpace(state.Weather)
                ? "UNREAD"
                : state.Weather.Replace('_', ' ').ToUpperInvariant();
            _weatherLabel.Text =
                $"WEATHER // {weather} · VIS {state.WeatherVisibility:P0} · RAD +{state.OutdoorRadiation:0}";
        }

        if (_statusRail != null)
        {
            _statusRail.Set("day",   $"DAY {state.Day:00}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("loc",   string.IsNullOrWhiteSpace(state.Location) ? "THE HOLDFAST" : state.Location.Replace('_', ' ').ToUpperInvariant(),
                AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("weather", string.IsNullOrWhiteSpace(state.Weather) ? "UNREAD" : state.Weather.Replace('_', ' ').ToUpperInvariant(),
                AshfallMetricCard.Criticality.Normal);

            int hpMax = Math.Max(1, state.MaxHealth);
            float avgHp = Math.Max(0f, state.AverageSurvivorHealth);
            AshfallMetricCard.Criticality hpCrit =
                avgHp >= 75 ? AshfallMetricCard.Criticality.Normal
                : avgHp >= 50 ? AshfallMetricCard.Criticality.Caution
                : avgHp > 0 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;
            _statusRail.Set("hp", $"{avgHp:0.0}%", hpCrit);

            float rad = Math.Max(0f, state.Radiation);
            AshfallMetricCard.Criticality radCrit =
                rad < 25 ? AshfallMetricCard.Criticality.Normal
                : rad < 50 ? AshfallMetricCard.Criticality.Caution
                : rad < 100 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;
            _statusRail.Set("rad", $"{rad:0.0} mSv", radCrit);
            _statusRail.Set("air", $"{state.AirFilterHealth:0}%",
                state.AirFilterHealth < 50 ? AshfallMetricCard.Criticality.Warn
                : state.AirFilterHealth < 75 ? AshfallMetricCard.Criticality.Caution
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("air_q", $"{state.AirQuality:0}%",
                state.AirWarning ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("radon", $"{state.RadonLevel:0} Bq/m³", AshfallMetricCard.Criticality.Normal);
        }

        // Stores Watch DataGrid
        if (_storesGrid != null)
        {
            var rows = new List<AshfallDataGrid.Row>
            {
                BuildStoreRow("CLEAN WATER",      $"{Math.Max(0, state.CleanWater):00} units",
                    state.CleanWater,
                    state.CleanWater <= 0 ? AshfallDataGrid.CellState.Critical
                    : state.CleanWater < 5 ? AshfallDataGrid.CellState.Warning
                    : AshfallDataGrid.CellState.Normal),
                BuildStoreRow("PRESERVED FOOD",   $"{Math.Max(0, state.Food):00} units",
                    state.Food,
                    state.Food <= 0 ? AshfallDataGrid.CellState.Critical
                    : state.Food < 5 ? AshfallDataGrid.CellState.Warning
                    : AshfallDataGrid.CellState.Normal),
                BuildStoreRow("FILTER SPARES",    $"{Math.Max(0, state.FilterSpares):00} spares",
                    state.FilterSpares,
                    state.FilterSpares <= 0 ? AshfallDataGrid.CellState.Critical : AshfallDataGrid.CellState.Normal),
                BuildStoreRow("MECHANICAL SCRAP", $"{Math.Max(0, state.MechanicalScrap):00} scrap",
                    state.MechanicalScrap,
                    state.MechanicalScrap <= 0 ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal),
                BuildStoreRow("MEDICAL STOCK",    $"{Math.Max(0, state.MedicalStock):00} doses",
                    state.MedicalStock,
                    state.MedicalStock <= 0 ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal),
            };
            _storesGrid.SetRows(rows);
        }

        // Condition Report DataGrid
        if (_conditionGrid != null)
        {
            int hpMax = Math.Max(1, state.MaxHealth);
            float hpPct = Math.Clamp((float)state.Health / hpMax * 100f, 0f, 100f);
            float radClamped = Math.Clamp(state.Radiation, 0f, 100f);
            float hungerClamped = Math.Clamp(state.Hunger, 0f, 100f);
            float thirstClamped = Math.Clamp(state.Thirst, 0f, 100f);
            var rows = new List<AshfallDataGrid.Row>
            {
                BuildConditionRow("HEALTH",    $"{state.Health:0}/{state.MaxHealth:0}", hpPct,
                    hpPct <= 25 ? AshfallDataGrid.CellState.Critical
                    : hpPct <= 50 ? AshfallDataGrid.CellState.Warning
                    : AshfallDataGrid.CellState.Normal),
                BuildConditionRow("RADIATION", $"{state.Radiation:0.0} mSv", radClamped,
                    radClamped >= 100 ? AshfallDataGrid.CellState.Critical
                    : radClamped >= 50 ? AshfallDataGrid.CellState.Warning
                    : radClamped >= 25 ? AshfallDataGrid.CellState.Caution
                    : AshfallDataGrid.CellState.Normal),
                BuildConditionRow("HUNGER",    $"{state.Hunger:0}/100",    hungerClamped,
                    hungerClamped >= 80 ? AshfallDataGrid.CellState.Warning
                    : hungerClamped >= 50 ? AshfallDataGrid.CellState.Caution
                    : AshfallDataGrid.CellState.Normal),
                BuildConditionRow("THIRST",    $"{state.Thirst:0}/100",    thirstClamped,
                    thirstClamped >= 80 ? AshfallDataGrid.CellState.Warning
                    : thirstClamped >= 50 ? AshfallDataGrid.CellState.Caution
                    : AshfallDataGrid.CellState.Normal),
                BuildConditionRow("ROSTER",
                    state.TotalSurvivors == 0 ? "—" : $"{state.LivingSurvivors}/{state.TotalSurvivors} LIVING",
                    state.TotalSurvivors == 0 ? 0f : (float)state.LivingSurvivors / state.TotalSurvivors,
                    state.LivingSurvivors == state.TotalSurvivors ? AshfallDataGrid.CellState.Normal
                    : state.LivingSurvivors >= state.TotalSurvivors * 0.75f ? AshfallDataGrid.CellState.Caution
                    : AshfallDataGrid.CellState.Warning),
            };
            _conditionGrid.SetRows(rows);
        }

        if (_airFilterBar != null && _airFilterValue != null)
        {
            _airFilterBar.Value = state.AirFilterHealth;
            _airFilterValue.Text = $"{state.AirFilterHealth:0}%";
            _airFilterValue.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(state.AirFilterHealth < 50f
                    ? DesignTheme.Critical
                    : state.AirFilterHealth < 75f
                        ? DesignTheme.Entropy
                        : DesignTheme.Pale));
        }

        if (_btnServiceFilter != null)
        {
            _btnServiceFilter.Disabled = state.MechanicalScrap <= 0 || state.AirFilterHealth >= 100f;
            _btnServiceFilter.Text = $"SERVICE FILTER (-1 SCRAP) [{state.MechanicalScrap}]";
        }
        if (_btnReplaceFilter != null)
        {
            _btnReplaceFilter.Disabled = state.FilterSpares <= 0 || state.AirFilterHealth >= 100f;
            _btnReplaceFilter.Text = $"REPLACE HEPA CORE (-1 SPARE) [{state.FilterSpares}]";
        }

        if (_forecastLabel != null && state.Forecast != null && state.Forecast.Count > 0)
        {
            var sb = new System.Text.StringBuilder("FORECAST // ");
            for (int i = 0; i < state.Forecast.Count; i++)
            {
                var f = state.Forecast[i];
                sb.Append($"D{f.Day}: {f.Kind} (RAD +{f.OutdoorRad:0})");
                if (i < state.Forecast.Count - 1) sb.Append(" · ");
            }
            _forecastLabel.Text = sb.ToString();
        }

        if (_dutyRosterSummary != null)
        {
            string intake = string.IsNullOrWhiteSpace(state.FilterDutyAssignee) ? "UNASSIGNED" : state.FilterDutyAssignee;
            _dutyRosterSummary.Text = $"DUTY ROSTER // INTAKE FILTRATION: {intake}";
        }

        bool outdoorHazard = state.OutdoorRadiation > 0f || state.WeatherVisibility < 0.99f;
        if (_directiveText != null)
        {
            string weather = string.IsNullOrWhiteSpace(state.Weather) ? "UNREAD" : state.Weather.Replace('_', ' ').ToUpperInvariant();
            _directiveText.Text = outdoorHazard
                ? $"Hold the hatch. {weather} is reading {state.OutdoorRadiation:0} mSv outside with visibility at {state.WeatherVisibility:P0}."
                : state.AirWarning
                    ? "Atmospheric contamination rising. Service the HEPA filtration stack before toxic air settles."
                    : state.Hunger >= 75 || state.Thirst >= 75
                        ? "Rations are becoming the next problem. Reconcile food and water before the next shift."
                        : "Keep the shelter quiet. Check the filter pressure before the next outdoor shift.";
        }
        if (_eventLabel != null)
            _eventLabel.Text = string.IsNullOrWhiteSpace(state.LastEvent)
                ? outdoorHazard
                    ? "No new dispatch. The weather station is carrying the warning for us."
                    : "No fresh signal. The radio is holding a weak carrier from the north line."
                : state.LastEvent;
    }

    private static AshfallDataGrid.Row BuildStoreRow(string name, string stockAsText, int stockValue, AshfallDataGrid.CellState state)
    {
        var cells = new List<AshfallDataGrid.Cell>
        {
            new(name, AshfallDataGrid.CellState.Normal),
            new(stockAsText, state),
            new(
                stockValue <= 0 ? "EMPTY" :
                stockValue < 5 ? "LOW" :
                "OK",
                state),
        };
        return new AshfallDataGrid.Row { Cells = cells, Selectable = true };
    }

    private static AshfallDataGrid.Row BuildConditionRow(string name, string valueText, float ratio, AshfallDataGrid.CellState state)
    {
        string statusText = state switch
        {
            AshfallDataGrid.CellState.Critical => "CRITICAL",
            AshfallDataGrid.CellState.Warning => "WARNING",
            AshfallDataGrid.CellState.Caution => "CAUTION",
            _ => "OK",
        };
        var cells = new List<AshfallDataGrid.Cell>
        {
            new(name, AshfallDataGrid.CellState.Normal),
            new(valueText, state),
            new(statusText, state),
        };
        return new AshfallDataGrid.Row { Cells = cells, Selectable = true };
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
            OnMenuRequested?.Invoke();
            GetViewport().SetInputAsHandled();
        }
    }
}
