using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Weather panel.
    /// Shows current weather and a 3-day forecast. Wrapped in the dashboard
    /// shell with a forecast DataGrid and a status rail that carries the
    /// day's live outdoor radiation + crew visibility.
    /// </summary>
    public partial class WeatherPanel : Control
    {
        public event Action? OnClose;

        public WeatherKind? BoundWeather => ActiveWeather?.Current;
        public bool IsBound => _worldHost != null || _weatherHost != null;
        public int RenderedHazardCount => _advisoryList?.GetChildCount() ?? 0;

        private WorldHostSession? _worldHost;
        private WeatherHostSession? _weatherHost;

        private WeatherSystem? ActiveWeather => _worldHost?.Weather ?? _weatherHost?.System;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private AshfallDataGrid? _forecastGrid;
        private VBoxContainer _advisoryList = null!;
        private VBoxContainer _seasonList = null!;

        public void Bind(WeatherHostSession weather)
        {
            _weatherHost = weather;
            if (_weatherHost?.System != null)
            {
                _weatherHost.System.OnWeatherChanged -= HandleWeatherChanged;
                _weatherHost.System.OnWeatherChanged += HandleWeatherChanged;
            }
            RefreshView();
        }

        public void Bind(WorldHostSession weather)
        {
            _worldHost = weather;
            if (_worldHost?.Weather != null)
            {
                _worldHost.Weather.OnWeatherChanged -= HandleWeatherChanged;
                _worldHost.Weather.OnWeatherChanged += HandleWeatherChanged;
            }
            RefreshView();
        }

        private void HandleWeatherChanged(WeatherKind _) => RefreshView();

        public void RefreshView()
        {
            RefreshStatusRail();
            BuildForecastRows();
            BuildAdvisory();
            BuildSeasonRows();
        }

        private void RefreshStatusRail()
        {
            if (_statusRail == null) return;
            var w = ActiveWeather;
            if (w == null)
            {
                _statusRail.Set("pattern", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("outdoor", "0", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("temp_pen", "0°C", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("vis", "0%", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("hazmat_decay", "×1.0", AshfallMetricCard.Criticality.Normal);
                return;
            }
            float tempPen = WeatherSystem.TemperaturePenaltyForWeather(w.Current);
            float outdoor = w.OutdoorRadModifier;
            float vis = w.VisibilityFactor;
            _statusRail.Set("pattern", w.Current.ToString().ToUpperInvariant(),
                outdoor > 0 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("outdoor", $"+{outdoor:0} mSv/h",
                outdoor > 100 ? AshfallMetricCard.Criticality.Critical
                : outdoor > 25 ? AshfallMetricCard.Criticality.Warn
                : outdoor > 0 ? AshfallMetricCard.Criticality.Caution
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("temp_pen", $"{tempPen:+#;-#;0}°C",
                tempPen <= -10 ? AshfallMetricCard.Criticality.Warn
                : tempPen <= -5 ? AshfallMetricCard.Criticality.Caution
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("vis", $"{vis:P0}",
                vis < 0.5 ? AshfallMetricCard.Criticality.Critical
                : vis < 0.8 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hazmat_decay", $"×{w.HazmatDegradeMultiplier:0.0}",
                w.HazmatDegradeMultiplier > 1.5 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal);
            if (_statusRail.GetCard("hazmat_decay") != null)
            {
                // Subtle: hazmat decay rides with outdoor rad sub-critically.
                if (outdoor > 100 && w.HazmatDegradeMultiplier > 1.0)
                    _statusRail.Set("hazmat_decay", $"×{w.HazmatDegradeMultiplier:0.0}", AshfallMetricCard.Criticality.Critical);
            }
        }

        private void BuildForecastRows()
        {
            if (_forecastGrid == null) return;
            var w = ActiveWeather;
            if (w == null)
            {
                _forecastGrid.SetRows(BuildForecastFixture());
                return;
            }
            int day = Math.Max(1, (int)Math.Floor(w.State.totalElapsedHours / 24f) + 1);
            var forecast = w.PeekForecast(3);
            var rows = new List<AshfallDataGrid.Row>();
            foreach (var f in forecast)
            {
                var crit = f.OutdoorRad > 100 ? AshfallDataGrid.CellState.Critical
                    : f.OutdoorRad > 25 ? AshfallDataGrid.CellState.Warning
                    : f.OutdoorRad > 0 ? AshfallDataGrid.CellState.Caution
                    : AshfallDataGrid.CellState.Positive;
                rows.Add(new AshfallDataGrid.Row
                {
                    Cells = new List<AshfallDataGrid.Cell>
                    {
                        new($"D{f.Day:00}", AshfallDataGrid.CellState.Normal),
                        new(f.Kind.ToString().ToUpperInvariant(), crit),
                        new($"+{f.OutdoorRad:0} mSv/h", crit),
                        new(f.Visibility < 0.5 ? "VIS LOW" : f.Visibility < 0.8 ? "VIS DIM" : "VIS OK",
                            f.Visibility < 0.5 ? AshfallDataGrid.CellState.Warning
                            : f.Visibility < 0.8 ? AshfallDataGrid.CellState.Caution
                            : AshfallDataGrid.CellState.Positive),
                        new(WeathersRiskLabel(f), AshfallDataGrid.CellState.Muted),
                    }
                });
            }
            if (rows.Count == 0)
            {
                rows.Add(new AshfallDataGrid.Row
                {
                    Cells = new List<AshfallDataGrid.Cell>
                    {
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("no forecast available", AshfallDataGrid.CellState.Muted),
                    }
                });
            }
            _forecastGrid.SetRows(rows);
        }

        private static string WeathersRiskLabel(WeatherForecastEntry f)
        {
            if (f.OutdoorRad > 100) return "no overhang";
            if (f.OutdoorRad > 25) return "short window";
            if (f.OutdoorRad > 0) return "calm";
            return "ideal";
        }

        private void BuildSeasonRows()
        {
            if (_seasonList == null) return;
            AshfallUiHelpers.EmptyChildren(_seasonList);
            var w = ActiveWeather;
            if (w == null)
            {
                _seasonList.AddChild(AshfallUiHelpers.MakeMetadata("No season profile bound."));
                return;
            }
            int day = Math.Max(1, (int)Math.Floor(w.State.totalElapsedHours / 24f) + 1);
            var season = w.GetSeasonForDay(day);
            string profileName = _worldHost?.Profile?.displayName ?? season.displayName;
            _seasonList.AddChild(AshfallUiHelpers.MakeDataRow("Active Season Profile", profileName, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
            _seasonList.AddChild(AshfallUiHelpers.MakeDataRow("Next Weather Shift", $"In {w.State.hoursUntilNextCheck:0.0} Hours", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            _seasonList.AddChild(AshfallUiHelpers.MakeDataRow("Recorded Rolls", $"{w.State.rollCount}", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        }

        private void BuildAdvisory()
        {
            if (_advisoryList == null) return;
            AshfallUiHelpers.EmptyChildren(_advisoryList);
            var w = ActiveWeather;
            if (w == null)
            {
                _advisoryList.AddChild(AshfallUiHelpers.MakeMetadata("No advisories available."));
                return;
            }
            int count = 0;
            if (w.IsScavengingBlocked(false))
            {
                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon("badge_corneal_burn", 22);
                row.AddChild(icon);
                var lbl = AshfallUiHelpers.MakeWarning("Scavenging expedition blocked without full hazard gear.");
                lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(lbl);
                _advisoryList.AddChild(row);
                count++;
            }
            if (w.OutdoorRadModifier > 0f)
            {
                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon("badge_radon_poisoning", 22);
                row.AddChild(icon);
                var lbl = AshfallUiHelpers.MakeCritical($"Fallout radiation elevated: +{w.OutdoorRadModifier:0} mSv/hr.");
                lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(lbl);
                _advisoryList.AddChild(row);
                count++;
            }
            float tempPen = WeatherSystem.TemperaturePenaltyForWeather(w.Current);
            if (tempPen < 0f)
            {
                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon("badge_hypothermia", 22);
                row.AddChild(icon);
                var lbl = AshfallUiHelpers.MakeWarning($"Severe cold exposure risk: {tempPen:0}°C.");
                lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(lbl);
                _advisoryList.AddChild(row);
                count++;
            }
            if (count == 0)
                _advisoryList.AddChild(AshfallUiHelpers.MakeMetadata("No acute environmental hazards detected. Outdoor scavenging permitted."));
        }

        private static List<AshfallDataGrid.Row> BuildForecastFixture()
        {
            return new List<AshfallDataGrid.Row>
            {
                new AshfallDataGrid.Row
                {
                    Cells = new List<AshfallDataGrid.Cell>
                    {
                        new("D/D?", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("unbound", AshfallDataGrid.CellState.Muted),
                    }
                }
            };
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            _shell = new AshfallDashboardShell(
                "WEATHER & FALLOUT FORECAST — RAD_NOW_LEDGER",
                1100, 720);

            var hostContainer = new MarginContainer();
            hostContainer.AddThemeConstantOverride("margin_left", DesignTheme.HudEdge);
            hostContainer.AddThemeConstantOverride("margin_top", DesignTheme.SpacingLg);
            hostContainer.AddThemeConstantOverride("margin_right", DesignTheme.HudEdge);
            hostContainer.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
            hostContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            hostContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            hostContainer.AddChild(_shell);
            AddChild(hostContainer);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("pattern",   "PATTERN",     "—",       AshfallMetricCard.Criticality.Normal, 130);
            _statusRail.AddCard("outdoor",   "EXT RAD",     "0",        AshfallMetricCard.Criticality.Normal, 120);
            _statusRail.AddCard("temp_pen",  "TEMP PEN",    "0°C",      AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("vis",       "VISIBILITY",  "0%",       AshfallMetricCard.Criticality.Normal, 130);
            _statusRail.AddCard("hazmat_decay", "HAZMAT",   "×1.0",     AshfallMetricCard.Criticality.Normal, 110);
            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

            BuildContent();
            RefreshView();
        }

        private void BuildContent()
        {
            var contentStack = new VBoxContainer();
            contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            contentStack.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            contentStack.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

            contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("3-DAY FORECAST"));
            var cols = new[]
            {
                new AshfallDataGrid.Column { Header = "Day",  MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Left  },
                new AshfallDataGrid.Column { Header = "Pattern", MinWidth = 180, Alignment = AshfallDataGrid.ColumnAlign.Left  },
                new AshfallDataGrid.Column { Header = "Rad+", MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Right },
                new AshfallDataGrid.Column { Header = "Visibility", MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Center },
                new AshfallDataGrid.Column { Header = "Risk",  MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left  },
            };
            _forecastGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 600, minHeight: 240);
            _forecastGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _forecastGrid.SizeFlagsVertical = SizeFlags.ExpandFill;
            contentStack.AddChild(_forecastGrid);

            var split = new HBoxContainer();
            split.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            split.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            split.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

            var seasonPanel = AshfallUiHelpers.MakePanel();
            seasonPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            seasonPanel.SizeFlagsStretchRatio = 1f;
            var seasonMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
            seasonPanel.AddChild(seasonMargin);
            var seasonVbox = new VBoxContainer();
            seasonVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            seasonMargin.AddChild(seasonVbox);
            seasonVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("SEASON CYCLE"));
            _seasonList = new VBoxContainer();
            _seasonList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _seasonList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            seasonVbox.AddChild(_seasonList);
            split.AddChild(seasonPanel);

            var advisoryPanel = AshfallUiHelpers.MakePanel();
            advisoryPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            advisoryPanel.SizeFlagsStretchRatio = 1f;
            var advMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
            advisoryPanel.AddChild(advMargin);
            var advVBox = new VBoxContainer();
            advVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            advMargin.AddChild(advVBox);
            advVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ENVIRONMENTAL ADVISORIES"));
            _advisoryList = new VBoxContainer();
            _advisoryList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _advisoryList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            advVBox.AddChild(_advisoryList);
            split.AddChild(advisoryPanel);

            contentStack.AddChild(split);
            _shell.SetContent(contentStack);
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

        public void Unbind()
        {
            if (_worldHost?.Weather != null)
            {
                _worldHost.Weather.OnWeatherChanged -= HandleWeatherChanged;
            }
            if (_weatherHost?.System != null)
            {
                _weatherHost.System.OnWeatherChanged -= HandleWeatherChanged;
            }
            _worldHost = null;
            _weatherHost = null;
            RefreshView();
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

        public override void _ExitTree()
        {
            if (_worldHost?.Weather != null)
            {
                _worldHost.Weather.OnWeatherChanged -= HandleWeatherChanged;
            }
            if (_weatherHost?.System != null)
            {
                _weatherHost.System.OnWeatherChanged -= HandleWeatherChanged;
            }
            base._ExitTree();
        }
    }
}
