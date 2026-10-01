// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Survivors panel (HYBRID, lightweight).
    /// Roster of survivors with per-row HP / radiation / hunger / thirst.
    /// Panel gains the Phase-13 dashboard shell (sidebar + status rail) but
    /// keeps the existing row-major list rendering — applying DataGrid to a
    /// roster would not improve readability over the icon + name + vitals
    /// row, and the brief explicitly cautions against converting every
    /// focused modal into a full-screen dashboard.
    /// </summary>
    public partial class SurvivorsPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallSidebar? _sidebar;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _survivorList = null!;
        private VBoxContainer _statsGroup = null!;

        private SurvivorsHostSession? _survivorsHost;
        private int _day;
        private string _activeFilter = "all"; // all | living | strained | critical

        public bool IsBound => _survivorsHost != null;
        public int RenderedSurvivorCount => _survivorList?.GetChildCount() ?? 0;

        public void Bind(SurvivorsHostSession survivors, int day = 0)
        {
            Unbind();
            _survivorsHost = survivors;
            _day = day;
            if (_survivorsHost != null)
            {
                _survivorsHost.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_survivorsHost != null)
            {
                _survivorsHost.StateChanged -= RefreshView;
                _survivorsHost = null;
            }
        }



        public void RefreshView()
        {
            RefreshStatusRail();
            RefreshRoster();
            RefreshCohortStats();
        }

        private void RefreshStatusRail()
        {
            if (_statusRail == null) return;
            if (_survivorsHost == null)
            {
                _statusRail.Set("living",  "—",   AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("avgHp",   "—%",  AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("avgRad",  "—",   AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("avgMor",  "—%",  AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("strained","0",   AshfallMetricCard.Criticality.Normal);
                return;
            }
            int total = _survivorsHost.RosterState.Count;
            int living = _survivorsHost.RosterState.Count(s => s != null && s.IsAliveState);
            float avgHp = _survivorsHost.RosterState.Count == 0 ? 0f : _survivorsHost.RosterState.Average(s => s?.Health ?? 0f);
            var slices = _survivorsHost.CaptureSave()?.survivors;
            float avgRad = slices == null || slices.Count == 0 ? 0f : slices.Average(s => s?.lifetimeRadiationExposure ?? 0f);
            float avgMor = _survivorsHost.RosterState.Count == 0 ? 0f : _survivorsHost.RosterState.Average(s => s?.Morale ?? 0f);
            // P011 — strain classification reads the same critical values the
            // needs simulation enforces; never re-typed magic numbers.
            var profile = _survivorsHost.Needs.Profile;
            int strained = _survivorsHost.RosterState.Count(s =>
                s != null && s.IsAliveState && (profile.IsHungerCritical(s.Hunger)
                    || profile.IsThirstCritical(s.Thirst)
                    || profile.IsWarmthCritical(s.Warmth)
                    || profile.IsHealthCritical(s.Health)));

            _statusRail.Set("living",   $"{living}/{total}", total == 0 ? AshfallMetricCard.Criticality.Normal
                : living == total ? AshfallMetricCard.Criticality.Normal
                : living >= (int)(total * 0.75f) ? AshfallMetricCard.Criticality.Caution
                : AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("avgHp",    $"{avgHp:0}%",
                avgHp >= 75 ? AshfallMetricCard.Criticality.Normal
                : avgHp >= 50 ? AshfallMetricCard.Criticality.Caution
                : avgHp > 0 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical);
            _statusRail.Set("avgRad",   $"{avgRad:0} mSv",
                // Task 9 — read the shared radiation bands instead of re-typing
                // 25/50/100; a threshold change must move every surface at once.
                avgRad < Ashfall.Core.Radiation.RadiationSystem.WarnThreshold * 0.5f ? AshfallMetricCard.Criticality.Normal
                : avgRad < Ashfall.Core.Radiation.RadiationSystem.WarnThreshold ? AshfallMetricCard.Criticality.Caution
                : avgRad < Ashfall.Core.Radiation.RadiationSystem.AcuteThreshold ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical);
            _statusRail.Set("avgMor",   $"{avgMor:0}%",
                // Task 9 — morale bands from the authored profile (warn > critical).
                avgMor >= profile.moraleWarn ? AshfallMetricCard.Criticality.Normal
                : avgMor >= profile.moraleCritical ? AshfallMetricCard.Criticality.Caution
                : AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("strained", $"{strained}",
                strained == 0 ? AshfallMetricCard.Criticality.Normal
                : strained <= 2 ? AshfallMetricCard.Criticality.Caution
                : AshfallMetricCard.Criticality.Warn);
        }

        private void RefreshRoster()
        {
            if (_survivorList == null) return;
            AshfallUiHelpers.EmptyChildren(_survivorList);
            if (_survivorsHost == null)
            {
                _survivorList.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.survivors.empty.no_session", "No survivor session bound.")));
                return;
            }

            var slices = _survivorsHost.CaptureSave().survivors
                .Where(slice => slice != null)
                .ToDictionary(s => s.id, StringComparer.Ordinal);
            int rendered = 0;
            var profile = _survivorsHost.Needs.Profile;

            foreach (var survivor in _survivorsHost.RosterState)
            {
                if (survivor == null) continue;
                slices.TryGetValue(survivor.Id, out var slice);
                var definition = _survivorsHost.Roster.FindDefinition(survivor.Id);
                string displayName = !string.IsNullOrWhiteSpace(definition?.displayName)
                    ? definition.displayName
                    : survivor.Id;
                string status = !survivor.IsAliveState
                    ? "DEAD"
                    : profile.IsHealthCritical(survivor.Health)
                        ? "CRITICAL"
                        : profile.IsHungerCritical(survivor.Hunger)
                            || profile.IsThirstCritical(survivor.Thirst)
                            || profile.IsWarmthCritical(survivor.Warmth)
                            ? "STRAINED"
                            : "STABLE";
                float lifetimeDose = slice?.lifetimeRadiationExposure ?? 0f;

                if (!FilterPass(status, survivor.IsAliveState)) continue;

                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon(lifetimeDose >= Ashfall.Core.Radiation.RadiationSystem.WarnThreshold ? "badge_rad_sickness" : profile.IsHealthWarn(survivor.Health) ? "badge_trench_foot" : "badge_exhaustion", 22);
                row.AddChild(icon);
                var nameLbl = AshfallUiHelpers.MakeSmall(displayName);
                nameLbl.CustomMinimumSize = new Vector2(140, 0);
                // Overflow precision (a11y audit 2026-09-29 §6): long display
                // names clip at the row column instead of pushing the stats.
                nameLbl.ClipText = true;
                nameLbl.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
                row.AddChild(nameLbl);
                var statsText = AshfallUiHelpers.MakeMono(
                    $"HP {survivor.Health:0} · HUN {survivor.Hunger:0}{DayDelta(survivor.Id, NeedKind.Hunger)} · " +
                    $"THI {survivor.Thirst:0}{DayDelta(survivor.Id, NeedKind.Thirst)} · " +
                    $"FAT {survivor.Fatigue:0}{DayDelta(survivor.Id, NeedKind.Fatigue)} · " +
                    $"MOR {survivor.Morale:0}{DayDelta(survivor.Id, NeedKind.Morale)} · " +
                    $"WARM {survivor.Warmth:0}{DayDelta(survivor.Id, NeedKind.Warmth)} · RAD {lifetimeDose:0} mSv");
                statsText.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                statsText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
                row.AddChild(statsText);
                var statusLbl = AshfallUiHelpers.MakeSmall($"[{StatusLabel(status)}]");
                statusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(StatusColor(status)));
                row.AddChild(statusLbl);
                _survivorList.AddChild(row);
                rendered++;
            }

            if (rendered == 0)
            {
                if (_survivorsHost.RosterState.Count == 0)
                    _survivorList.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.survivors.empty.roster", "Roster empty. No registered shelter survivors.")));
                else
                    _survivorList.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.survivors.empty.no_match", "No survivors match the current filter.")));
            }
        }

        /// <summary>
        /// P010 — compact day-over-day delta suffix (" (+19/d)") for a need, or
        /// an empty string when no baseline exists (fresh load). Reads only the
        /// transient host read model; never mutates needs.
        /// </summary>
        private string DayDelta(string survivorId, NeedKind kind)
        {
            if (_survivorsHost == null) return string.Empty;
            if (!_survivorsHost.TryGetNeedDayDelta(survivorId, kind, out float delta)) return string.Empty;
            // Annotate a skipped/offline gap so it is not read as a one-day rate.
            int span = _day > 0 ? _survivorsHost.NeedDaySpan(_day) : 1;
            string unit = span > 1 ? $"/{span}d" : "/d";
            // Loop-5 hardening — route through the shared formatter so a
            // non-finite or sub-unit delta renders "0" instead of "-0"/"NaN".
            return $" ({Ashfall.Core.Survivors.NeedsDayDeltaFormat.Signed(delta)}{unit})";
        }

        private void RefreshCohortStats()
        {
            if (_statsGroup == null) return;
            AshfallUiHelpers.EmptyChildren(_statsGroup);
            if (_survivorsHost == null) return;

            float avgHp = _survivorsHost.RosterState.Count == 0 ? 0f : _survivorsHost.RosterState.Average(s => s?.Health ?? 0f);
            float avgMor = _survivorsHost.RosterState.Count == 0 ? 0f : _survivorsHost.RosterState.Average(s => s?.Morale ?? 0f);
            _statsGroup.AddChild(AshfallUiHelpers.MakeBody(TrFormat("ui.survivors.cohort.summary", $"{avgMor:0}", $"{avgHp:0}")));
            if (!string.IsNullOrWhiteSpace(_survivorsHost.LastEvent))
                _statsGroup.AddChild(AshfallUiHelpers.MakeMetadata(
                    TrFormat("ui.survivors.event.line", Tr("ui.survivors.event.latest", "Latest roster event"), _survivorsHost.LastEvent)));
            else
                _statsGroup.AddChild(AshfallUiHelpers.MakeMetadata(Tr("ui.survivors.event.none", "No roster events recorded yet.")));
        }

        private bool FilterPass(string status, bool alive)
        {
            return _activeFilter switch
            {
                "living" => alive,
                "strained" => status == "STRAINED" || status == "CRITICAL" || status == "DEAD",
                "critical" => status == "CRITICAL" || status == "DEAD",
                _ => true,
            };
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = AshfallUiHelpers.PanelScrim() };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            _shell = new AshfallDashboardShell(
                Tr("ui.survivors.shell.title", "SURVIVOR ROSTER & DUTY COHORT"),
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

            _sidebar = _shell.SetSidebar(new[]
            {
                new AshfallSidebar.Item { Id = "filter_all",      Label = Tr("ui.survivors.filter.all", "Filter: All"),           Hint = Tr("ui.survivors.filter.all.hint", "every survivor") },
                new AshfallSidebar.Item { Id = "filter_living",   Label = Tr("ui.survivors.filter.living", "Filter: Living"),     Hint = Tr("ui.survivors.filter.living.hint", "alive only") },
                new AshfallSidebar.Item { Id = "filter_strained", Label = Tr("ui.survivors.filter.strained", "Filter: Strained"), Hint = Tr("ui.survivors.filter.strained.hint", "STRAINED + worse") },
                new AshfallSidebar.Item { Id = "filter_critical", Label = Tr("ui.survivors.filter.critical", "Filter: Critical"), Hint = Tr("ui.survivors.filter.critical.hint", "CRITICAL + DEAD") },
            }, Tr("ui.survivors.sidebar.title", "ROSTER OPS"), "filter_all");
            if (_sidebar != null)
                _sidebar.OnSelected += id =>
                {
                    _activeFilter = id switch
                    {
                        "filter_living" => "living",
                        "filter_strained" => "strained",
                        "filter_critical" => "critical",
                        _ => "all",
                    };
                    RefreshRoster();
                };

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("living",   Tr("ui.survivors.rail.living", "LIVING"),   "—",   AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("avgHp",    Tr("ui.survivors.rail.avg_hp", "AVG HP"),   "—%",  AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("avgRad",   Tr("ui.survivors.rail.avg_rad", "AVG RAD"),  "—",   AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("avgMor",   Tr("ui.survivors.rail.avg_mor", "AVG MOR"),  "—%",  AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("strained", Tr("ui.survivors.rail.strained", "STRAINED"), "0",   AshfallMetricCard.Criticality.Normal, 120);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

            BuildContent();
            RefreshView();
        }

        private void BuildContent()
        {
            var content = new HBoxContainer();
            content.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            content.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            content.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

            var listCol = new VBoxContainer();
            listCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            listCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            listCol.SizeFlagsStretchRatio = 1.45f;
            listCol.AddChild(AshfallUiHelpers.MakeSectionHeader(AshfallUiText.Tr("ui.survivors.header.roster", "RESIDENT ROSTER")));
            _survivorList = new VBoxContainer();
            _survivorList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _survivorList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _survivorList.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            listCol.AddChild(_survivorList);
            content.AddChild(listCol);

            var rightCol = new VBoxContainer();
            rightCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            rightCol.SizeFlagsStretchRatio = 1.0f;
            var cohortPanel = AshfallUiHelpers.MakePanel();
            cohortPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            cohortPanel.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            var rMargin = new MarginContainer();
            rMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingMd);
            rMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingMd);
            rMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingMd);
            rMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
            cohortPanel.AddChild(rMargin);
            var rVBox = new VBoxContainer();
            rVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rMargin.AddChild(rVBox);
            rVBox.AddChild(AshfallUiHelpers.MakeSectionHeader(AshfallUiText.Tr("ui.survivors.header.telemetry", "COHORT TELEMETRY")));
            _statsGroup = new VBoxContainer();
            _statsGroup.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _statsGroup.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            rVBox.AddChild(_statsGroup);
            rightCol.AddChild(cohortPanel);
            content.AddChild(rightCol);

            _shell.SetContent(content);
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }

        // Task 4 — resolve through the shared AshfallUiText helper.
        private static string Tr(string key, string fallback) => AshfallUiText.Tr(key, fallback);

        private static string TrFormat(string key, params object[] args) => AshfallUiText.TrFormat(key, args);

        /// <summary>Localizes the internal status code for display; the code
        /// itself stays stable for <see cref="FilterPass"/>.</summary>
        private static string StatusLabel(string status) => status switch
        {
            "DEAD" => Tr("ui.survivors.status.dead", "DEAD"),
            "CRITICAL" => Tr("ui.survivors.status.critical", "CRITICAL"),
            "STRAINED" => Tr("ui.survivors.status.strained", "STRAINED"),
            _ => Tr("ui.survivors.status.stable", "STABLE"),
        };

        private static (float r, float g, float b, float a) StatusColor(string status) => status switch
        {
            "CRITICAL" => DesignTheme.Critical,
            "STRAINED" => DesignTheme.Warm,
            "DEAD" => DesignTheme.Muted,
            _ => DesignTheme.Pale,
        };

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
