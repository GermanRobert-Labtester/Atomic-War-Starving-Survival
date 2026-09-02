using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Player-facing game shell for the active Godot host.
    ///
    /// This is presentation and routing only. Simulation state is pushed in through
    /// UpdateState; buttons emit intent back to Main, which owns the host sessions.
    /// </summary>
    public partial class GameDashboardPanel : Control
    {
        public event Action? OnMenuRequested;
        public event Action? OnAdvanceDayRequested;
        public event Action? OnSaveRequested;
        public event Action? OnDeveloperRequested;
        public event Action? OnServiceFilterRequested;
        public event Action? OnReplaceFilterRequested;
        public event Action<string>? OnOpenPanelRequested;

        /// <summary>
        /// Presentation snapshot supplied by Main. It deliberately contains no
        /// simulation rules; the dashboard only formats the values it receives.
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
            public System.Collections.Generic.List<Ashfall.Core.World.WeatherForecastEntry> Forecast = new();
            public System.Collections.Generic.Dictionary<string, string> DutyAssignments = new();
            public string MachineTellText = string.Empty;
        }

        private Label _dayLabel = null!;
        private Label _locationLabel = null!;
        private Label _weatherLabel = null!;
        private Label _healthValue = null!;
        private Label _radiationValue = null!;
        private Label _hungerValue = null!;
        private Label _thirstValue = null!;
        private Label _survivorSummary = null!;
        private Label _shelterState = null!;
        private Label _directiveText = null!;
        private Label _eventLabel = null!;
        private Label _resourceSummary = null!;
        private Label _waterValue = null!;
        private Label _foodValue = null!;
        private Label _medicalValue = null!;
        private Label _filterValue = null!;
        private Label _machineTellLabel = null!;
        private Label _scrapValue = null!;
        private Label _nextShiftValue = null!;
        private Label _hatchValue = null!;
        private Label _airQualityValue = null!;
        private Label _radonLabel = null!;
        private Label _forecastLabel = null!;
        private Label _dutyRosterSummary = null!;
        private Button _btnServiceFilter = null!;
        private Button _btnReplaceFilter = null!;
        private ProgressBar _healthBar = null!;
        private ProgressBar _radiationBar = null!;
        private ProgressBar _hungerBar = null!;
        private ProgressBar _thirstBar = null!;
        private ProgressBar _airFilterBar = null!;
        private Label _airFilterValue = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;
            BuildView();
        }

        public void UpdateState(
            int day,
            int health,
            int maxHealth,
            float radiation,
            long value,
            string weather,
            string location = "THE HOLDFAST")
        {
            UpdateState(new DashboardSnapshot
            {
                Day = day,
                Health = health,
                MaxHealth = maxHealth,
                Radiation = radiation,
                Value = value,
                Weather = weather,
                Location = location
            });
        }

        public void UpdateState(DashboardSnapshot state)
        {
            if (_dayLabel == null || state == null) return;

            int safeMaxHealth = Math.Max(1, state.MaxHealth);
            int safeHealth = Math.Clamp(state.Health, 0, safeMaxHealth);
            int safeHunger = Math.Clamp(state.Hunger, 0, 100);
            int safeThirst = Math.Clamp(state.Thirst, 0, 100);
            float safeRadiation = Math.Max(0f, state.Radiation);
            float visibility = Math.Clamp(state.WeatherVisibility, 0f, 1f);
            float outdoorRadiation = Math.Max(0f, state.OutdoorRadiation);
            int totalSurvivors = Math.Max(0, state.TotalSurvivors);
            int livingSurvivors = Math.Clamp(state.LivingSurvivors, 0, totalSurvivors);
            string weather = string.IsNullOrWhiteSpace(state.Weather)
                ? "UNREAD"
                : state.Weather.Replace('_', ' ').ToUpperInvariant();
            bool outdoorHazard = outdoorRadiation > 0f || visibility < 0.99f;

            _dayLabel.Text = $"DAY {state.Day:00}";
            _locationLabel.Text = string.IsNullOrWhiteSpace(state.Location)
                ? "THE HOLDFAST"
                : state.Location.Replace('_', ' ').ToUpperInvariant();
            _weatherLabel.Text = $"WEATHER // {weather} · VIS {visibility:P0} · RAD +{outdoorRadiation:0}";

            _healthValue.Text = $"{safeHealth}/{safeMaxHealth}";
            _healthBar.MaxValue = safeMaxHealth;
            _healthBar.Value = safeHealth;
            _healthValue.AddThemeColorOverride(
                "font_color",
                AshfallUiHelpers.ToColor(safeHealth <= safeMaxHealth * 0.25f
                    ? DesignTheme.Critical
                    : safeHealth <= safeMaxHealth * 0.5f
                        ? DesignTheme.Entropy
                        : DesignTheme.Pale));

            _radiationValue.Text = $"{safeRadiation:0.0} mSv";
            _radiationBar.MaxValue = 100f;
            _radiationBar.Value = Math.Clamp(safeRadiation, 0f, 100f);
            _radiationValue.AddThemeColorOverride(
                "font_color",
                AshfallUiHelpers.ToColor(safeRadiation >= 100f
                    ? DesignTheme.Critical
                    : safeRadiation >= 50f
                        ? DesignTheme.Entropy
                        : DesignTheme.Lethe));

            _hungerValue.Text = $"{safeHunger}/100";
            _hungerBar.MaxValue = 100f;
            _hungerBar.Value = safeHunger;
            _hungerValue.AddThemeColorOverride(
                "font_color",
                AshfallUiHelpers.ToColor(safeHunger >= 80
                    ? DesignTheme.Critical
                    : safeHunger >= 50
                        ? DesignTheme.Entropy
                        : DesignTheme.Pale));

            _thirstValue.Text = $"{safeThirst}/100";
            _thirstBar.MaxValue = 100f;
            _thirstBar.Value = safeThirst;
            _thirstValue.AddThemeColorOverride(
                "font_color",
                AshfallUiHelpers.ToColor(safeThirst >= 80
                    ? DesignTheme.Critical
                    : safeThirst >= 50
                        ? DesignTheme.Entropy
                        : DesignTheme.Pale));

            _survivorSummary.Text = totalSurvivors == 0
                ? "ROSTER // NO SURVIVORS REGISTERED"
                : $"ROSTER // {livingSurvivors}/{totalSurvivors} LIVING · AVG HP {Math.Max(0f, state.AverageSurvivorHealth):0}%";

            _shelterState.Text = state.FilterSpares <= 0
                ? "SHELTER STATUS // NO FILTER SPARES"
                : safeHealth <= safeMaxHealth * 0.25f
                ? "SHELTER STATUS // MEDICAL ATTENTION REQUIRED"
                : safeRadiation >= 50f
                    ? "SHELTER STATUS // DECONTAMINATION ADVISED"
                    : "SHELTER STATUS // HOLDING";

            _resourceSummary.Text = $"STORES // VALUE {state.Value:N0} · WATER {Math.Max(0, state.CleanWater):00} · FOOD {Math.Max(0, state.Food):00}";
            _waterValue.Text = $"{Math.Max(0, state.CleanWater):00} units";
            _foodValue.Text = $"{Math.Max(0, state.Food):00} units";
            _medicalValue.Text = $"{Math.Max(0, state.MedicalStock):00} doses";
            _filterValue.Text = $"{Math.Max(0, state.FilterSpares):00} spares";
            if (_machineTellLabel != null)
            {
                _machineTellLabel.Text = string.IsNullOrWhiteSpace(state.MachineTellText)
                    ? "MACHINES // NOMINAL"
                    : $"MACHINES // {state.MachineTellText}";
                _machineTellLabel.AddThemeColorOverride(
                    "font_color",
                    AshfallUiHelpers.ToColor(state.MachineTellText.Contains("CRITICAL") || state.MachineTellText.Contains("FAULT")
                        ? DesignTheme.Critical
                        : state.MachineTellText.Contains("WORN") || state.MachineTellText.Contains("RATTLE") || state.MachineTellText.Contains("CHOKE") || state.MachineTellText.Contains("COUGH")
                            ? DesignTheme.Entropy
                            : DesignTheme.Pale));
            }
            if (_scrapValue != null) _scrapValue.Text = $"{Math.Max(0, state.MechanicalScrap):00} scrap";

            // ── Air Filtration & Atmosphere ──
            if (_airFilterValue != null && _airFilterBar != null)
            {
                float safeFilter = Math.Clamp(state.AirFilterHealth, 0f, 100f);
                _airFilterValue.Text = $"{safeFilter:0}%";
                _airFilterBar.Value = safeFilter;
                _airFilterValue.AddThemeColorOverride(
                    "font_color",
                    AshfallUiHelpers.ToColor(safeFilter < 50f
                        ? DesignTheme.Critical
                        : safeFilter < 75f
                            ? DesignTheme.Entropy
                            : DesignTheme.Pale));
            }

            if (_airQualityValue != null)
            {
                string qualityTag = state.AirWarning ? "[WARNING: CONTAMINATED]" : "[STABLE]";
                _airQualityValue.Text = $"AIR QUALITY: {state.AirQuality:0}% · RADON: {state.RadonLevel:0} Bq/m³ {qualityTag}";
                _airQualityValue.AddThemeColorOverride(
                    "font_color",
                    AshfallUiHelpers.ToColor(state.AirWarning ? DesignTheme.Critical : DesignTheme.Warm));
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

            // ── Forecast & Duty Roster ──
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

            _directiveText.Text = outdoorHazard
                ? $"Hold the hatch. {weather} is reading {outdoorRadiation:0} mSv outside with visibility at {visibility:P0}."
                : state.AirWarning
                    ? "Atmospheric contamination rising. Service the HEPA filtration stack before toxic air settles."
                    : safeHunger >= 75 || safeThirst >= 75
                        ? "Rations are becoming the next problem. Reconcile food and water before the next shift."
                        : "Keep the shelter quiet. Check the filter pressure before the next outdoor shift.";
            _nextShiftValue.Text = outdoorHazard ? $"HOLD / {weather}" : $"OPEN / {weather}";
            _hatchValue.Text = outdoorHazard ? "SEALED // HAZARD" : "SEALED";
            _eventLabel.Text = string.IsNullOrWhiteSpace(state.LastEvent)
                ? outdoorHazard
                    ? "No new dispatch. The weather station is carrying the warning for us."
                    : "No fresh signal. The radio is holding a weak carrier from the north line."
                : state.LastEvent;
        }

        public void SetDeveloperMode(bool enabled)
        {
            _shelterState.Text = enabled
                ? "DEVELOPER CONSOLE // ACTIVE"
                : "SHELTER STATUS // HOLDING";
        }

        private void BuildView()
        {
            var background = new TextureRect
            {
                Texture = AshfallUiHelpers.TryLoadTexture("res://assets/art/bg_bunker_corridor.png")
                       ?? AshfallUiHelpers.TryLoadTexture("res://assets/art/bg_ice_road_hatch.png"),
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered,
                Modulate = new Color(1f, 1f, 1f, 0.22f),
                MouseFilter = MouseFilterEnum.Ignore
            };
            background.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(background);

            var shade = new ColorRect
            {
                Color = new Color(DesignTheme.Ink.r, DesignTheme.Ink.g, DesignTheme.Ink.b, 0.82f),
                MouseFilter = MouseFilterEnum.Ignore
            };
            shade.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(shade);

            var outer = AshfallUiHelpers.MakeMargins(DesignTheme.HudEdge, DesignTheme.SpacingLg, DesignTheme.HudEdge, DesignTheme.SpacingMd);
            outer.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(outer);

            var shell = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            outer.AddChild(shell);
            shell.AddChild(BuildHeader());

            var body = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
            body.SizeFlagsVertical = SizeFlags.ExpandFill;
            shell.AddChild(body);

            body.AddChild(BuildNavigationRail());

            var workspace = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            workspace.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            workspace.SizeFlagsVertical = SizeFlags.ExpandFill;
            body.AddChild(workspace);
            workspace.AddChild(BuildWorkspaceHeader());

            var columns = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
            columns.SizeFlagsVertical = SizeFlags.ExpandFill;
            workspace.AddChild(columns);
            columns.AddChild(BuildOverviewColumn());
            columns.AddChild(BuildFieldColumn());

            shell.AddChild(BuildFooter());
        }

        private Control BuildHeader()
        {
            var content = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingLg);
            content.CustomMinimumSize = new Vector2(0, 58);

            var brand = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            brand.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var title = AshfallUiHelpers.MakeTitle("ASHFALL", DesignTheme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            brand.AddChild(title);
            brand.AddChild(AshfallUiHelpers.MakeMetadata("ATOMIC WAR // STARVING SURVIVAL"));
            content.AddChild(brand);

            _locationLabel = AshfallUiHelpers.MakeMono("THE HOLDFAST");
            _locationLabel.HorizontalAlignment = HorizontalAlignment.Right;
            content.AddChild(_locationLabel);

            _dayLabel = AshfallUiHelpers.MakeMono("DAY 01");
            _dayLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            content.AddChild(_dayLabel);

            var journal = MakeActionButton("JOURNAL", () => OnOpenPanelRequested?.Invoke("journal"));
            journal.CustomMinimumSize = new Vector2(92, 34);
            content.AddChild(journal);

            var menu = MakeActionButton("MENU", () => OnMenuRequested?.Invoke(), true);
            menu.CustomMinimumSize = new Vector2(84, 34);
            content.AddChild(menu);

            return WrapSurface(content, new Vector2(0, 58), DesignTheme.SpacingMd);
        }

        private Control BuildWorkspaceHeader()
        {
            var content = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
            var heading = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            heading.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            heading.AddChild(AshfallUiHelpers.MakeSectionHeader("BUNKER OPERATIONS"));
            heading.AddChild(AshfallUiHelpers.MakeMetadata("A quiet room, a working filter, and one more day to account for."));
            content.AddChild(heading);

            _weatherLabel = AshfallUiHelpers.MakeMono("WEATHER // UNREAD");
            _weatherLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
            content.AddChild(_weatherLabel);
            return content;
        }

        private Control BuildNavigationRail()
        {
            var content = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            content.CustomMinimumSize = new Vector2(196, 0);
            content.AddChild(AshfallUiHelpers.MakeSectionHeader("SYSTEMS"));
            content.AddChild(AshfallUiHelpers.MakeMetadata("SELECT A SURFACE"));
            content.AddChild(AshfallUiHelpers.MakeSeparator());

            AddNavButton(content, "OVERVIEW", "overview", true);
            AddNavButton(content, "STATUS", "status");
            AddNavButton(content, "SURVIVORS", "survivors");
            AddNavButton(content, "SURVIVAL", "survival_detail");
            AddNavButton(content, "INVENTORY", "inventory");
            AddNavButton(content, "CRAFTING", "crafting");
            AddNavButton(content, "MEDICAL", "medical");
            AddNavButton(content, "AFFLICTIONS", "afflictions");
            AddNavButton(content, "EXPEDITIONS", "expeditions");
            AddNavButton(content, "WEATHER", "weather");
            AddNavButton(content, "WEATHER DETAIL", "weather_detail");
            AddNavButton(content, "RADIO", "radio");
            AddNavButton(content, "MAP", "map");
            AddNavButton(content, "SHELTER", "shelter");
            AddNavButton(content, "TRADE", "trade");
            AddNavButton(content, "ECONOMY", "economy_detail");
            AddNavButton(content, "RESEARCH", "research");
            AddNavButton(content, "GREENHOUSE", "greenhouse");
            AddNavButton(content, "FACTIONS", "factions");
            AddNavButton(content, "MUSTER", "muster");
            AddNavButton(content, "VERDICT", "verdict");
            AddNavButton(content, "MARITIME", "maritime");
            AddNavButton(content, "DUTY ROSTER", "duty_roster");
            AddNavButton(content, "QUESTS", "quests");
            AddNavButton(content, "EVENTS", "event_detail");
            AddNavButton(content, "JOURNAL", "journal_detail");
            AddNavButton(content, "RADIATION", "radiation_detail");
            AddNavButton(content, "RAD HISTORY", "radiation_history");
            AddNavButton(content, "ACHIEVEMENTS", "achievements");
            AddNavButton(content, "HELP", "help");

            content.AddChild(AshfallUiHelpers.MakeSeparator());
            content.AddChild(AshfallUiHelpers.MakeSectionHeader("EXPANSION SURFACES"));
            AddNavButton(content, "POLITICS", "politics");
            AddNavButton(content, "PRISONERS", "prisoners");
            AddNavButton(content, "FORCED LABOR", "forced_labor");
            AddNavButton(content, "NARCOTICS LAB", "narcotics");
            AddNavButton(content, "MUTATIONS", "mutation_tree");
            AddNavButton(content, "NURSERY", "nursery");
            AddNavButton(content, "AVIATION", "aviation");
            AddNavButton(content, "STEALTH OPS", "stealth");
            AddNavButton(content, "FALLOUT RADAR", "fallout_detail");

            content.AddChild(new Control { SizeFlagsVertical = SizeFlags.ExpandFill });
            content.AddChild(AshfallUiHelpers.MakeSeparator());

            var save = MakeActionButton("SAVE LEDGER", () => OnSaveRequested?.Invoke());
            save.CustomMinimumSize = new Vector2(0, 34);
            content.AddChild(save);

            var developer = MakeActionButton("DEV CONSOLE", () => OnDeveloperRequested?.Invoke());
            developer.CustomMinimumSize = new Vector2(0, 34);
            content.AddChild(developer);

            return WrapSurface(content, new Vector2(196, 0), DesignTheme.SpacingMd);
        }

        private Control BuildOverviewColumn()
        {
            var column = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            column.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            column.SizeFlagsVertical = SizeFlags.ExpandFill;
            column.SizeFlagsStretchRatio = 1.12f;

            var condition = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            condition.AddChild(AshfallUiHelpers.MakeSectionHeader("CONDITION REPORT"));
            _shelterState = AshfallUiHelpers.MakeMetadata("SHELTER STATUS // HOLDING");
            _shelterState.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            condition.AddChild(_shelterState);
            condition.AddChild(AshfallUiHelpers.MakeSeparator());
            condition.AddChild(MakeGaugeRow("HEALTH", out _healthBar, out _healthValue, DesignTheme.Warm));
            condition.AddChild(MakeGaugeRow("RADIATION", out _radiationBar, out _radiationValue, DesignTheme.Lethe));
            condition.AddChild(MakeGaugeRow("HUNGER", out _hungerBar, out _hungerValue, DesignTheme.Warm));
            condition.AddChild(MakeGaugeRow("THIRST", out _thirstBar, out _thirstValue, DesignTheme.Lethe));
            _survivorSummary = AshfallUiHelpers.MakeMetadata("ROSTER // --");
            condition.AddChild(_survivorSummary);
            var conditionPanel = WrapSurface(condition);
            conditionPanel.SizeFlagsVertical = SizeFlags.ExpandFill;
            column.AddChild(conditionPanel);

            // ── Air Filtration Card ──
            var airStack = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            airStack.AddChild(AshfallUiHelpers.MakeSectionHeader("AIR FILTRATION & ATMOSPHERE"));
            _airQualityValue = AshfallUiHelpers.MakeMetadata("AIR QUALITY: 100% · RADON: 12 Bq/m³ [STABLE]");
            _airQualityValue.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            airStack.AddChild(_airQualityValue);
            airStack.AddChild(AshfallUiHelpers.MakeSeparator());
            airStack.AddChild(MakeGaugeRow("HEPA FILTER", out _airFilterBar, out _airFilterValue, DesignTheme.Entropy));

            var airActions = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            _btnServiceFilter = MakeActionButton("SERVICE FILTER (-1 SCRAP)", () => OnServiceFilterRequested?.Invoke());
            _btnServiceFilter.CustomMinimumSize = new Vector2(0, 32);
            _btnServiceFilter.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            airActions.AddChild(_btnServiceFilter);

            _btnReplaceFilter = MakeActionButton("REPLACE CORE (-1 HEPA)", () => OnReplaceFilterRequested?.Invoke());
            _btnReplaceFilter.CustomMinimumSize = new Vector2(0, 32);
            _btnReplaceFilter.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            airActions.AddChild(_btnReplaceFilter);
            airStack.AddChild(airActions);

            var airPanel = WrapSurface(airStack);
            column.AddChild(airPanel);

            var directive = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            directive.AddChild(AshfallUiHelpers.MakeSectionHeader("CURRENT DIRECTIVE"));
            _directiveText = AshfallUiHelpers.MakeBody("Keep the shelter quiet. Check the filter pressure before the next outdoor shift.");
            directive.AddChild(_directiveText);

            var btnRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var protocolButton = MakeActionButton("DIRECTIVES PROTOCOL", () => OnOpenPanelRequested?.Invoke("protocol"), true);
            protocolButton.CustomMinimumSize = new Vector2(0, 34);
            protocolButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            btnRow.AddChild(protocolButton);

            var directiveButton = MakeActionButton("OPEN SHELTER", () => OnOpenPanelRequested?.Invoke("shelter"));
            directiveButton.CustomMinimumSize = new Vector2(0, 34);
            directiveButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            btnRow.AddChild(directiveButton);
            directive.AddChild(btnRow);

            column.AddChild(WrapSurface(directive));

            return column;
        }

        private Control BuildFieldColumn()
        {
            var column = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            column.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            column.SizeFlagsVertical = SizeFlags.ExpandFill;
            column.SizeFlagsStretchRatio = 0.88f;

            var stores = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            stores.AddChild(AshfallUiHelpers.MakeSectionHeader("STORES WATCH"));
            _resourceSummary = AshfallUiHelpers.MakeMono("STORES // VALUE 100 · WATER 62 · FOOD 84 · FILTER 03");
            _resourceSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            stores.AddChild(_resourceSummary);
            stores.AddChild(AshfallUiHelpers.MakeSeparator());
            stores.AddChild(MakeLiveDataRow("CLEAN WATER", "--", out _waterValue, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
            stores.AddChild(MakeLiveDataRow("PRESERVED FOOD", "--", out _foodValue, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            stores.AddChild(MakeLiveDataRow("FILTER SPARES", "--", out _filterValue, AshfallUiHelpers.ToColor(DesignTheme.Entropy)));
            stores.AddChild(MakeLiveDataRow("MACHINE TELLS", "--", out _machineTellLabel, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            stores.AddChild(MakeLiveDataRow("MECHANICAL SCRAP", "--", out _scrapValue, AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            stores.AddChild(MakeLiveDataRow("MEDICAL STOCK", "--", out _medicalValue, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            var inventoryButton = MakeActionButton("OPEN INVENTORY", () => OnOpenPanelRequested?.Invoke("inventory"));
            inventoryButton.CustomMinimumSize = new Vector2(0, 34);
            stores.AddChild(inventoryButton);
            column.AddChild(WrapSurface(stores));

            var report = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            report.AddChild(AshfallUiHelpers.MakeSectionHeader("FIELD & DUTY REPORT"));
            _forecastLabel = AshfallUiHelpers.MakeMono("FORECAST // D01: CLEAR · D02: OVERCAST · D03: ASHFALL");
            _forecastLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            report.AddChild(_forecastLabel);

            _dutyRosterSummary = AshfallUiHelpers.MakeMetadata("DUTY ROSTER // INTAKE FILTRATION: Dr. Sarah Chen");
            _dutyRosterSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            report.AddChild(_dutyRosterSummary);

            _eventLabel = AshfallUiHelpers.MakeBody("No fresh signal. The radio is holding a weak carrier from the north line.");
            _eventLabel.CustomMinimumSize = new Vector2(0, 44);
            report.AddChild(_eventLabel);
            report.AddChild(AshfallUiHelpers.MakeSeparator());
            report.AddChild(MakeLiveDataRow("OUTDOOR READ", "--", out _nextShiftValue, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            report.AddChild(MakeLiveDataRow("HATCH", "SEALED", out _hatchValue, AshfallUiHelpers.ToColor(DesignTheme.Pale)));

            var rosterButton = MakeActionButton("DUTY ROSTER SHIFTS", () => OnOpenPanelRequested?.Invoke("duty_roster"));
            rosterButton.CustomMinimumSize = new Vector2(0, 34);
            report.AddChild(rosterButton);

            var advance = MakeActionButton("ADVANCE TO NEXT DAY", () => OnAdvanceDayRequested?.Invoke(), true);
            advance.CustomMinimumSize = new Vector2(0, 42);
            report.AddChild(advance);
            column.AddChild(WrapSurface(report));

            return column;
        }

        private Control BuildFooter()
        {
            var content = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingLg);
            content.AddChild(AshfallUiHelpers.MakeMetadata("[J] JOURNAL"));
            content.AddChild(AshfallUiHelpers.MakeMetadata("[ESC] MENU"));
            content.AddChild(AshfallUiHelpers.MakeMetadata("[F1] DEV CONSOLE"));
            var spacer = new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            content.AddChild(spacer);
            var status = AshfallUiHelpers.MakeMetadata("AUTOSAVE // LEDGER READY");
            status.HorizontalAlignment = HorizontalAlignment.Right;
            content.AddChild(status);
            return content;
        }

        private void AddNavButton(VBoxContainer parent, string text, string panelId, bool active = false)
        {
            var button = MakeActionButton(text, () =>
            {
                if (panelId == "overview") return;
                OnOpenPanelRequested?.Invoke(panelId);
            }, active);
            button.CustomMinimumSize = new Vector2(0, 30);
            button.Alignment = HorizontalAlignment.Left;
            parent.AddChild(button);
        }

        private static HBoxContainer MakeGaugeRow(string label, out ProgressBar bar, out Label value, (float r, float g, float b, float a) fillColor)
        {
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var name = AshfallUiHelpers.MakeLabel(label);
            name.CustomMinimumSize = new Vector2(74, 0);
            row.AddChild(name);

            bar = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 100,
                Value = 0,
                ShowPercentage = false,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                CustomMinimumSize = new Vector2(90, 12)
            };
            bar.AddThemeStyleboxOverride("background", AshfallUiHelpers.MakeFlatBg(
                new Color(DesignTheme.Ink.r, DesignTheme.Ink.g, DesignTheme.Ink.b, 0.9f), AshfallUiHelpers.ToColor(DesignTheme.LineSoft), 1, DesignTheme.RadiusSm));
            bar.AddThemeStyleboxOverride("fill", AshfallUiHelpers.MakeFlatBg(
                new Color(fillColor.r, fillColor.g, fillColor.b, 0.92f), null, 0, DesignTheme.RadiusSm));
            row.AddChild(bar);

            value = AshfallUiHelpers.MakeMono("--");
            value.CustomMinimumSize = new Vector2(82, 0);
            value.HorizontalAlignment = HorizontalAlignment.Right;
            row.AddChild(value);
            return row;
        }

        private static HBoxContainer MakeLiveDataRow(string label, string initialValue, out Label value, Color color)
        {
            var row = AshfallUiHelpers.MakeDataRow(label, initialValue, color);
            value = row.GetChild(1) as Label ?? AshfallUiHelpers.MakeMono(initialValue);
            return row;
        }

        private static Button MakeActionButton(string text, Action action, bool primary = false)
        {
            var button = AshfallUiHelpers.MakeButton(text, action);
            var normalBackground = primary
                ? new Color(DesignTheme.Warm.r, DesignTheme.Warm.g, DesignTheme.Warm.b, 0.16f)
                : new Color(DesignTheme.Ink.r, DesignTheme.Ink.g, DesignTheme.Ink.b, 0.46f);
            var hoverBackground = new Color(DesignTheme.Warm.r, DesignTheme.Warm.g, DesignTheme.Warm.b, primary ? 0.28f : 0.12f);
            button.AddThemeStyleboxOverride("normal", AshfallUiHelpers.MakeFlatBg(normalBackground, AshfallUiHelpers.ToColor(DesignTheme.Line), 1, DesignTheme.RadiusSm));
            button.AddThemeStyleboxOverride("hover", AshfallUiHelpers.MakeFlatBg(hoverBackground, AshfallUiHelpers.ToColor(DesignTheme.Warm), 1, DesignTheme.RadiusSm));
            button.AddThemeStyleboxOverride("pressed", AshfallUiHelpers.MakeFlatBg(
                new Color(DesignTheme.Warm.r, DesignTheme.Warm.g, DesignTheme.Warm.b, 0.34f), AshfallUiHelpers.ToColor(DesignTheme.Hot), 1, DesignTheme.RadiusSm));
            button.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            button.AddThemeColorOverride("font_hover_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            button.AddThemeColorOverride("font_pressed_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            return button;
        }

        private static PanelContainer WrapSurface(Control content, Vector2 minSize = default, int padding = DesignTheme.SpacingMd)
        {
            var panel = AshfallUiHelpers.MakePanel((int)minSize.X, (int)minSize.Y);
            panel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var margin = AshfallUiHelpers.MakeMargins(padding);
            panel.AddChild(margin);
            margin.AddChild(content);
            return panel;
        }
    }
}
