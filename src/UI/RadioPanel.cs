using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Radio;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Radio panel.
    /// Shows radio signals, broadcasts, and communication logs.
    /// HYBRID target: dashboard shell wraps the existing tuner + sets a
    /// 5-card status rail (current frequency / day / monitored channels /
    /// strongest recent signal / last intercept day) plus a DataGrid of the
    /// last 16 intercepts. Per the brief, no waveform / spectrogram is added —
    /// there is no Core data source exposed.
    /// </summary>
    public partial class RadioPanel : Control
    {
        public event Action? OnClose;
        public event Action? OnRadioBroadcastSent;

        private readonly (float Freq, string Label)[] _presets =
        {
            (142.850f, "142.850 MHz · COLD COUNT"),
            (104.200f, "104.200 MHz · HYDRO-BARONS"),
            (98.500f, "098.500 MHz · SCAVENGER NET"),
            (120.400f, "120.400 MHz · DISTRESS BEACON")
        };

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private AshfallDataGrid? _interceptsGrid;
        private RadioHostSession? _radioHost;

        public bool IsBound => _radioHost != null;
        public int RenderedSignalCount => _interceptsGrid?.RowCount ?? 0;

        public void Bind(RadioHostSession radio)
        {
            _radioHost = radio;
            if (_radioHost != null)
            {
                _radioHost.StateChanged -= RefreshView;
                _radioHost.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public void RefreshView()
        {
            RefreshStatusRail();
            BuildInterceptsGrid();
        }

        private void RefreshStatusRail()
        {
            if (_statusRail == null) return;
            if (_radioHost == null)
            {
                _statusRail.Set("freq",     "—",     AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("day",      "—",     AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("channels", "0",     AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("history",  "0",     AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("highest",  "—",     AshfallMetricCard.Criticality.Normal);
                return;
            }
            int hist = _radioHost.History?.Count ?? 0;
            float strongest = 0f;
            int lastDay = -1;
            for (int i = 0; i < hist; i++)
            {
                var sig = _radioHost.History![i];
                if (sig.SignalStrength > strongest) strongest = sig.SignalStrength;
                if (sig.Day > lastDay) lastDay = sig.Day;
            }
            _statusRail.Set("freq",     $"{_radioHost.CurrentFrequency:00.00} MHz", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("day",      $"D{_radioHost.Day:00}",                     AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("channels", $"{_radioHost.Engine.FactionCount}",        AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("history",  $"{hist}",                                   AshfallMetricCard.Criticality.Normal);
            var strongestCrit = strongest >= 4 ? AshfallMetricCard.Criticality.Normal
                : strongest >= 3 ? AshfallMetricCard.Criticality.Caution
                : strongest >= 2 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;
            _statusRail.Set("highest",  hist == 0 ? "—" : $"SIG {strongest:0}/5",
                hist == 0 ? AshfallMetricCard.Criticality.Normal : strongestCrit);
        }

        private void BuildInterceptsGrid()
        {
            if (_interceptsGrid == null) return;
            var rows = new List<AshfallDataGrid.Row>();
            if (_radioHost != null && _radioHost.History != null && _radioHost.History.Count > 0)
            {
                int first = Math.Max(0, _radioHost.History.Count - 16);
                for (int i = _radioHost.History.Count - 1; i >= first; i--)
                {
                    var sig = _radioHost.History[i];
                    string source = string.IsNullOrWhiteSpace(sig.FactionId)
                        ? sig.Callsign
                        : $"{sig.Callsign} · {sig.FactionId}";
                    var sigQuality = sig.SignalStrength >= 4 ? AshfallDataGrid.CellState.Positive
                        : sig.SignalStrength >= 3 ? AshfallDataGrid.CellState.Normal
                        : sig.SignalStrength >= 2 ? AshfallDataGrid.CellState.Caution
                        : AshfallDataGrid.CellState.Warning;
                    rows.Add(new AshfallDataGrid.Row
                    {
                        Cells = new List<AshfallDataGrid.Cell>
                        {
                            new($"D{sig.Day:00} · {sig.FrequencyMhz:00.00}", AshfallDataGrid.CellState.Normal),
                            new(source, AshfallDataGrid.CellState.Normal),
                            new($"SIG {sig.SignalStrength}/5", sigQuality),
                            new(sig.Kind.ToString().ToUpperInvariant(),
                                sig.Kind == RadioEventKind.Silence ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
                            new(Truncate(sig.Message, 60), sig.Kind == RadioEventKind.Silence ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
                        }
                    });
                }
            }
            if (rows.Count == 0)
            {
                rows.Add(new AshfallDataGrid.Row
                {
                    Cells = new List<AshfallDataGrid.Cell>
                    {
                        new("—",      AshfallDataGrid.CellState.Muted),
                        new("—",      AshfallDataGrid.CellState.Muted),
                        new("—",      AshfallDataGrid.CellState.Muted),
                        new("silent", AshfallDataGrid.CellState.Muted),
                        new("Tuner offline", AshfallDataGrid.CellState.Muted),
                    }
                });
            }
            _interceptsGrid.SetRows(rows);
        }

        private static string Truncate(string s, int n)
        {
            if (string.IsNullOrEmpty(s)) return "—";
            if (s.Length <= n) return s;
            return s.Substring(0, n - 1) + "…";
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            _shell = new AshfallDashboardShell(
                "RADIO COMMUNICATIONS & INTERCEPTS",
                1180, 720);

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
            _statusRail.AddCard("freq",     "FREQUENCY", "—",      AshfallMetricCard.Criticality.Normal, 130);
            _statusRail.AddCard("day",      "DAY",      "—",      AshfallMetricCard.Criticality.Normal, 80);
            _statusRail.AddCard("channels", "CHANNELS", "0",      AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("history",  "HISTORY",  "0",      AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("highest",  "STRONGEST","—",      AshfallMetricCard.Criticality.Normal, 110);

            BuildContent();
            RefreshView();
        }

        private void BuildContent()
        {
            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => Close());

            // ── Preset + action row pinned above the grid ──
            var topRow = new HBoxContainer();
            topRow.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            topRow.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            topRow.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

            // Tuner pad on the left, presets quadrant-style.
            var presetCol = new VBoxContainer();
            presetCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            presetCol.SizeFlagsStretchRatio = 0.95f;
            presetCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            presetCol.AddChild(AshfallUiHelpers.MakeSectionHeader("FREQUENCY TUNER"));

            foreach (var (freq, label) in _presets)
            {
                float targetFreq = freq;
                var btnFreq = AshfallUiHelpers.MakeButton(label, () =>
                {
                    if (_radioHost != null)
                    {
                        _radioHost.Listen(targetFreq);
                        RefreshView();
                    }
                });
                btnFreq.CustomMinimumSize = new Vector2(0, 28);
                btnFreq.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                presetCol.AddChild(btnFreq);
            }
            var btnBeacon = AshfallUiHelpers.MakeButton("BROADCAST HOLDFAST EMERGENCY BEACON", () =>
            {
                if (_radioHost != null)
                {
                    _radioHost.BroadcastBeacon("Holdfast shelter holding. Awaiting survivor response.");
                    OnRadioBroadcastSent?.Invoke();
                    RefreshView();
                }
            });
            btnBeacon.CustomMinimumSize = new Vector2(0, 36);
            btnBeacon.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            presetCol.AddChild(btnBeacon);
            topRow.AddChild(presetCol);

            // ── Right: intercepts DataGrid ──
            var gridCol = new VBoxContainer();
            gridCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            gridCol.SizeFlagsStretchRatio = 1.45f;
            gridCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            gridCol.AddChild(AshfallUiHelpers.MakeSectionHeader("RECENT INTERCEPTS (16 LATEST)"));
            var cols = new[]
            {
                new AshfallDataGrid.Column { Header = "Day/Freq",  MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "Source",    MinWidth = 160, Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "Sig",       MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Center },
                new AshfallDataGrid.Column { Header = "Kind",      MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "Message",   MinWidth = 240, Alignment = AshfallDataGrid.ColumnAlign.Left   },
            };
            _interceptsGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 650, minHeight: 360);
            _interceptsGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _interceptsGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            gridCol.AddChild(_interceptsGrid);
            topRow.AddChild(gridCol);

            _shell.SetContent(topRow);
        }

        public void Open()
        {
            RefreshView();
            Visible = true;
            QueueRedraw();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
