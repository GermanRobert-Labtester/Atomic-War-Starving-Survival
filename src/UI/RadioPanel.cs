// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Radio;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
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
    /// Plan 173 Phase 3 adds a thin PROGRAM PRODUCTION strip over
    /// <see cref="RadioProgramProductionHostSession"/> (start/cancel/status only).
    /// </summary>
    public partial class RadioPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action? OnRadioBroadcastSent;

        private const string DefaultPresenterId = "presenter_shelter_desk";

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
        private AshfallDataGrid? _stationsGrid;
        private VBoxContainer? _productionBox;
        private Label? _productionEventLabel;
        private VBoxContainer? _rescueBox;
        private Label? _rescueEventLabel;
        private VBoxContainer? _followUpBox;
        private Label? _followUpEventLabel;
        private RadioHostSession? _radioHost;
        private RadioProgramProductionHostSession? _productionHost;

        public bool IsBound => _radioHost != null;
        public bool IsProductionBound => _productionHost != null;
        public int RenderedSignalCount => _interceptsGrid?.RowCount ?? 0;

        public void Bind(RadioHostSession radio)
        {
            if (_radioHost != null)
                _radioHost.StateChanged -= RefreshView;
            _radioHost = radio;
            if (_radioHost != null)
                _radioHost.StateChanged += RefreshView;
            RefreshView();
        }

        /// <summary>Plan 173 — bind program production session for the production strip.</summary>
        public void BindProduction(RadioProgramProductionHostSession production)
        {
            if (_productionHost != null)
                _productionHost.StateChanged -= RefreshView;
            _productionHost = production;
            if (_productionHost != null)
                _productionHost.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_radioHost != null)
            {
                _radioHost.StateChanged -= RefreshView;
                _radioHost = null;
            }
            if (_productionHost != null)
            {
                _productionHost.StateChanged -= RefreshView;
                _productionHost = null;
            }
        }

        public void RefreshView()
        {
            RefreshStatusRail();
            BuildStationsGrid();
            BuildInterceptsGrid();
            RefreshProductionStrip();
            RefreshRescueStrip();
            RefreshFollowUpStrip();
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

        private void BuildStationsGrid()
        {
            if (_stationsGrid == null) return;
            var rows = new List<AshfallDataGrid.Row>();
            if (_radioHost != null && _radioHost.Stations != null)
            {
                foreach (var station in _radioHost.Stations.AllStations)
                {
                    var state = _radioHost.Stations.GetStationState(station.StationId);
                    var currentSlot = _radioHost.GetCurrentSlot(station.StationId);
                    var nextSlot = _radioHost.GetNextSlot(station.StationId);
                    var sig = _radioHost.GetSignalStrength(station.StationId);

                    string curText = currentSlot != null
                        ? $"{currentSlot.ProgramType} ({currentSlot.StartHour:D2}:00-{currentSlot.EndHour:D2}:00)"
                        : "Off Air";

                    string nextText = nextSlot != null
                        ? $"{nextSlot.ProgramType} ({nextSlot.StartHour:D2}:00)"
                        : "—";

                    var stateCell = state == RadioStationState.Normal
                        ? AshfallDataGrid.CellState.Normal
                        : state == RadioStationState.Silent
                            ? AshfallDataGrid.CellState.Muted
                            : AshfallDataGrid.CellState.Warning;

                    var sigCell = sig.QualityBand == "Optimal" || sig.QualityBand == "Good"
                        ? AshfallDataGrid.CellState.Positive
                        : sig.QualityBand == "Degraded"
                            ? AshfallDataGrid.CellState.Caution
                            : AshfallDataGrid.CellState.Warning;

                    string reasons = sig.Reasons.Count > 0 ? string.Join(", ", sig.Reasons) : "Nominal";

                    rows.Add(new AshfallDataGrid.Row
                    {
                        Cells = new List<AshfallDataGrid.Cell>
                        {
                            new(station.DisplayName, AshfallDataGrid.CellState.Normal),
                            new($"{station.FrequencyMhz:00.00} MHz", AshfallDataGrid.CellState.Normal),
                            new(state.ToString(), stateCell),
                            new(curText, AshfallDataGrid.CellState.Normal),
                            new(nextText, AshfallDataGrid.CellState.Normal),
                            new(sig.QualityBand, sigCell),
                            new(reasons, sig.Reasons.Count > 0 ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Normal)
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
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("No stations loaded", AshfallDataGrid.CellState.Muted),
                    }
                });
            }
            _stationsGrid.SetRows(rows);
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
            // Multi-Band Navigation Controls
            var bandRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnPrevBand = AshfallUiHelpers.MakeButton("< BAND", () =>
            {
                if (_radioHost != null)
                {
                    var prev = RadioReceiverPlan.PreviousBand(_radioHost.CurrentBand);
                    _radioHost.SetBand(prev.BandId);
                    RefreshView();
                }
            });
            btnPrevBand.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            bandRow.AddChild(btnPrevBand);

            var btnNextBand = AshfallUiHelpers.MakeButton("BAND >", () =>
            {
                if (_radioHost != null)
                {
                    _radioHost.CycleBand();
                    RefreshView();
                }
            });
            btnNextBand.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            bandRow.AddChild(btnNextBand);
            presetCol.AddChild(bandRow);

            // Manual Stepper Controls
            var stepRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            stepRow.AddChild(AshfallUiHelpers.MakeButton("-5.0", () => { _radioHost?.TuneDelta(-5.0f); RefreshView(); }));
            stepRow.AddChild(AshfallUiHelpers.MakeButton("-0.5", () => { _radioHost?.TuneDelta(-0.5f); RefreshView(); }));
            stepRow.AddChild(AshfallUiHelpers.MakeButton("+0.5", () => { _radioHost?.TuneDelta(+0.5f); RefreshView(); }));
            stepRow.AddChild(AshfallUiHelpers.MakeButton("+5.0", () => { _radioHost?.TuneDelta(+5.0f); RefreshView(); }));
            presetCol.AddChild(stepRow);

            presetCol.AddChild(AshfallUiHelpers.MakeSeparator());
            presetCol.AddChild(AshfallUiHelpers.MakeSectionHeader("CHANNEL PRESETS"));

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

            presetCol.AddChild(AshfallUiHelpers.MakeSeparator());
            presetCol.AddChild(AshfallUiHelpers.MakeSectionHeader("DIRECTION FINDING & TRIANGULATION"));

            var dfRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var btnObs = AshfallUiHelpers.MakeButton("RECORD BEARING (045°)", () =>
            {
                if (_radioHost != null)
                {
                    _radioHost.RecordBearingObservation(45f);
                    RefreshView();
                }
            });
            btnObs.CustomMinimumSize = new Vector2(0, 30);
            btnObs.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            dfRow.AddChild(btnObs);

            var btnTri = AshfallUiHelpers.MakeButton("TRIANGULATE", () =>
            {
                if (_radioHost != null)
                {
                    _radioHost.TriangulateCurrentSignal();
                    RefreshView();
                }
            });
            btnTri.CustomMinimumSize = new Vector2(100, 30);
            dfRow.AddChild(btnTri);
            presetCol.AddChild(dfRow);

            var btnBeacon = AshfallUiHelpers.MakeButton("BROADCAST HOLDFAST EMERGENCY BEACON", () =>
            {
                if (_radioHost != null)
                {
                    _radioHost.BroadcastBeacon("Holdfast shelter holding. Awaiting survivor response.");
                    OnRadioBroadcastSent?.Invoke();
                    RefreshView();
                }
            });
            btnBeacon.CustomMinimumSize = new Vector2(0, 34);
            btnBeacon.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            presetCol.AddChild(btnBeacon);
            topRow.AddChild(presetCol);

            // ── Right: stations & intercepts DataGrids ──
            var gridCol = new VBoxContainer();
            gridCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            gridCol.SizeFlagsStretchRatio = 1.45f;
            gridCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            gridCol.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE STATIONS & SCHEDULES"));

            var stationCols = new[]
            {
                new AshfallDataGrid.Column { Header = "Station",         MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "Freq",            MinWidth = 75,  Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "State",           MinWidth = 65,  Alignment = AshfallDataGrid.ColumnAlign.Center },
                new AshfallDataGrid.Column { Header = "Current Program", MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "Next Program",    MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "Quality",         MinWidth = 75,  Alignment = AshfallDataGrid.ColumnAlign.Center },
                new AshfallDataGrid.Column { Header = "Status / Reasons",MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Left   },
            };
            _stationsGrid = new AshfallDataGrid(stationCols, showHeader: true, minWidth: 650, minHeight: 180);
            _stationsGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _stationsGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            gridCol.AddChild(_stationsGrid);

            gridCol.AddChild(AshfallUiHelpers.MakeSeparator());
            gridCol.AddChild(AshfallUiHelpers.MakeSectionHeader("RECENT INTERCEPTS (16 LATEST)"));
            var cols = new[]
            {
                new AshfallDataGrid.Column { Header = "Day/Freq",  MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "Source",    MinWidth = 160, Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "Sig",       MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Center },
                new AshfallDataGrid.Column { Header = "Kind",      MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left   },
                new AshfallDataGrid.Column { Header = "Message",   MinWidth = 240, Alignment = AshfallDataGrid.ColumnAlign.Left   },
            };
            _interceptsGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 650, minHeight: 180);
            _interceptsGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _interceptsGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            gridCol.AddChild(_interceptsGrid);
            topRow.AddChild(gridCol);

            // Plan 173 — PROGRAM PRODUCTION strip under tuner/intercepts.
            var root = new VBoxContainer();
            root.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            root.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            root.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            root.AddChild(topRow);
            root.AddChild(AshfallUiHelpers.MakeSeparator());
            root.AddChild(AshfallUiHelpers.MakeSectionHeader("PROGRAM PRODUCTION"));
            _productionEventLabel = AshfallUiHelpers.MakeMono("Last event: —");
            _productionEventLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            root.AddChild(_productionEventLabel);
            _productionBox = new VBoxContainer();
            _productionBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _productionBox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            root.AddChild(_productionBox);

            // ── Rescue-signal runtime strip: truthful stage/deadline/analysis state ──
            root.AddChild(AshfallUiHelpers.MakeSeparator());
            root.AddChild(AshfallUiHelpers.MakeSectionHeader("RESCUE SIGNALS"));
            _rescueEventLabel = AshfallUiHelpers.MakeMono("—");
            _rescueEventLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            root.AddChild(_rescueEventLabel);
            _rescueBox = new VBoxContainer();
            _rescueBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _rescueBox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            root.AddChild(_rescueBox);

            // ── Tasks 9–12: follow-up transmissions strip (pending + last fired) ──
            root.AddChild(AshfallUiHelpers.MakeSeparator());
            root.AddChild(AshfallUiHelpers.MakeSectionHeader("FOLLOW-UP TRANSMISSIONS"));
            _followUpEventLabel = AshfallUiHelpers.MakeMono("—");
            _followUpEventLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            root.AddChild(_followUpEventLabel);
            _followUpBox = new VBoxContainer();
            _followUpBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _followUpBox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            root.AddChild(_followUpBox);

            _shell.SetContent(root);
        }

        /// <summary>
        /// Tasks 9–12 follow-up strip: the scheduler's pending queue (due day
        /// always shown) and the last fired transmission. Truthful scheduler
        /// projection — the panel never invents schedules or replays state.
        /// </summary>
        private void RefreshFollowUpStrip()
        {
            if (_followUpBox == null) return;
            AshfallUiHelpers.EmptyChildren(_followUpBox);

            if (_radioHost == null)
            {
                _followUpBox.AddChild(AshfallUiHelpers.MakeMono("Follow-up scheduler offline."));
                if (_followUpEventLabel != null) _followUpEventLabel.Text = "—";
                return;
            }

            var pending = _radioHost.FollowUps.GetPendingView();
            if (pending.Count == 0)
                _followUpBox.AddChild(AshfallUiHelpers.MakeMono("No follow-up transmissions scheduled."));
            foreach (var p in pending)
            {
                _followUpBox.AddChild(AshfallUiHelpers.MakeMono($"{p.ParentSignalId} → {p.FollowUpId} (due day {p.DueDay})"));
            }

            var last = _radioHost.FollowUps.LastFired;
            if (_followUpEventLabel != null)
            {
                _followUpEventLabel.Text = last.HasValue
                    ? $"Last fired: {last.Value.ParentSignalId} / {last.Value.FollowUp.Id} on day {last.Value.Day}"
                    : "—";
            }
        }

        /// <summary>
        /// Rescue-signal runtime strip: one truthful line per registered rescue
        /// mission — stage, deadline, sender survival, persisted analysis
        /// verdict, and the structured preflight advisory. Verdicts and
        /// recommendations are always words, never color-only.
        /// </summary>
        private void RefreshRescueStrip()
        {
            if (_rescueBox == null) return;
            AshfallUiHelpers.EmptyChildren(_rescueBox);

            if (_radioHost == null)
            {
                _rescueBox.AddChild(AshfallUiHelpers.MakeMono("Rescue signal runtime offline."));
                if (_rescueEventLabel != null) _rescueEventLabel.Text = "—";
                return;
            }

            var missions = _radioHost.RescueMissions.AllMissions;
            if (missions == null || missions.Count == 0)
            {
                _rescueBox.AddChild(AshfallUiHelpers.MakeMono("No rescue missions registered."));
                if (_rescueEventLabel != null) _rescueEventLabel.Text = "—";
                return;
            }

            int active = 0, threats = 0, overdue = 0;
            var sorted = new List<DistressRescueMission>(missions);
            sorted.Sort((a, b) => string.Compare(a.QuestId, b.QuestId, StringComparison.Ordinal));
            foreach (var mission in sorted)
            {
                if (mission == null) continue;
                var preflight = _radioHost.RescueMissions.GetDispatchPreflight(mission.SignalId);
                if (preflight != null)
                {
                    if (!mission.IsTerminal && !mission.Expired) active++;
                    if (preflight.ThreatDetected) threats++;
                    if (mission.Expired) overdue++;
                }
                _rescueBox.AddChild(AshfallUiHelpers.MakeMono(FormatRescueLine(mission, preflight)));
            }

            if (_rescueEventLabel != null)
            {
                _rescueEventLabel.Text =
                    $"{sorted.Count} rescue calls tracked · {active} actionable · " +
                    (threats > 0 ? $"{threats} deception warning(s) · " : string.Empty) +
                    (overdue > 0 ? $"{overdue} gone unanswered past deadline" : "none expired");
            }
        }

        private static string FormatRescueLine(DistressRescueMission mission, RescueDispatchPreflight? preflight)
        {
            string stage = mission.Expired ? "EXPIRED (unanswered)" : mission.Stage.ToString();
            string sender = mission.SenderDeathDay > 0
                ? (mission.SenderAlive ? $"sender alive · dies day {mission.SenderDeathDay}" : $"sender dead (day {mission.SenderDeathDay})")
                : "no live-sender window";
            string analysis = mission.AuthenticityChecked
                ? $"analysis: {((SignalAuthenticityCategory)mission.AuthenticityAssessment).ToString().ToUpperInvariant()}" +
                  (mission.AssessmentThreatDetected ? " · DECEPTION FLAGGED" : string.Empty)
                : "analysis: not performed";
            string advisory = preflight != null ? $"advisory: {preflight.Recommendation}" : "advisory: unavailable";
            return $"{mission.QuestId} — {stage} · deadline day {mission.ExpiryDay} · {sender} · {analysis} · {advisory}";
        }

        private void RefreshProductionStrip()
        {
            if (_productionBox == null) return;
            AshfallUiHelpers.EmptyChildren(_productionBox);

            if (_productionHost == null)
            {
                _productionBox.AddChild(AshfallUiHelpers.MakeMono("Program production offline."));
                if (_productionEventLabel != null)
                    _productionEventLabel.Text = "Last event: —";
                return;
            }

            if (_productionEventLabel != null)
            {
                string last = string.IsNullOrEmpty(_productionHost.LastEvent)
                    ? "None recorded"
                    : _productionHost.LastEvent;
                _productionEventLabel.Text = $"Last event: {last}";
            }

            _productionBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("TEMPLATES"));
            var templates = _productionHost.System.Catalog.All;
            if (templates.Count == 0)
            {
                _productionBox.AddChild(AshfallUiHelpers.MakeMono("No program templates loaded."));
            }
            else
            {
                foreach (var template in templates)
                {
                    string tid = template.id;
                    string name = string.IsNullOrEmpty(template.display_name) ? tid : template.display_name;
                    string equip = template.required_equipment_item_ids != null && template.required_equipment_item_ids.Count > 0
                        ? string.Join(", ", template.required_equipment_item_ids)
                        : "none";
                    string cost = !string.IsNullOrEmpty(template.prep_cost_item_id) && template.prep_cost_count > 0
                        ? $"{template.prep_cost_count}× {template.prep_cost_item_id}"
                        : "none";

                    var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    var info = AshfallUiHelpers.MakeMono($"{name} · equip {equip} · cost {cost}");
                    info.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                    row.AddChild(info);
                    var startBtn = AshfallUiHelpers.MakeButton("START PREP", () =>
                    {
                        if (_productionHost == null) return;
                        int day = _radioHost?.Day ?? 1;
                        _productionHost.StartPrep(tid, DefaultPresenterId, day);
                        RefreshView();
                    });
                    startBtn.CustomMinimumSize = new Vector2(110, 26);
                    row.AddChild(startBtn);
                    _productionBox.AddChild(row);
                }
            }

            _productionBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIVE JOBS"));
            var jobs = _productionHost.System.GetActiveJobs();
            if (jobs.Count == 0)
            {
                _productionBox.AddChild(AshfallUiHelpers.MakeMono("No active prep jobs."));
                return;
            }

            for (int i = 0; i < jobs.Count; i++)
            {
                var job = jobs[i];
                string jid = job.JobId;
                string status = ((RadioProgramJobStatus)job.Status).ToString().ToUpperInvariant();
                string progress = job.Status == (int)RadioProgramJobStatus.Preparing
                    ? $"{job.PrepTicks}/{job.PrepTicksRequired}"
                    : "ready";

                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var info = AshfallUiHelpers.MakeMono($"{job.TemplateId} · {status} · {progress}");
                info.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(info);
                var cancelBtn = AshfallUiHelpers.MakeButton("CANCEL", () =>
                {
                    _productionHost?.CancelJob(jid);
                    RefreshView();
                });
                cancelBtn.CustomMinimumSize = new Vector2(90, 26);
                row.AddChild(cancelBtn);
                _productionBox.AddChild(row);
            }
        }

        public void Open()
        {
            RefreshView();
            Visible = true;
            QueueRedraw();
        }

        public void Close() {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
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

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
