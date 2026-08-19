using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.UI;
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
    public partial class SurvivorsPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallSidebar? _sidebar;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _survivorList = null!;
        private VBoxContainer _statsGroup = null!;

        private SurvivorsHostSession? _survivorsHost;
        private string _activeFilter = "all"; // all | living | strained | critical

        public bool IsBound => _survivorsHost != null;
        public int RenderedSurvivorCount => _survivorList?.GetChildCount() ?? 0;

        public void Bind(SurvivorsHostSession survivors)
        {
            _survivorsHost = survivors;
            if (_survivorsHost != null)
            {
                _survivorsHost.StateChanged -= RefreshView;
                _survivorsHost.StateChanged += RefreshView;
            }
            RefreshView();
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
            int strained = _survivorsHost.RosterState.Count(s =>
                s != null && s.IsAliveState && (s.Hunger >= 90f || s.Thirst >= 90f || s.Warmth <= 20f || s.Health < 25f));

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
                avgRad < 25 ? AshfallMetricCard.Criticality.Normal
                : avgRad < 50 ? AshfallMetricCard.Criticality.Caution
                : avgRad < 100 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical);
            _statusRail.Set("avgMor",   $"{avgMor:0}%",
                avgMor >= 60 ? AshfallMetricCard.Criticality.Normal
                : avgMor >= 30 ? AshfallMetricCard.Criticality.Caution
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
                _survivorList.AddChild(AshfallUiHelpers.MakeMetadata("No survivor session bound."));
                return;
            }

            var slices = _survivorsHost.CaptureSave().survivors
                .Where(slice => slice != null)
                .ToDictionary(s => s.id, StringComparer.Ordinal);
            int rendered = 0;

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
                    : survivor.Health < 25f
                        ? "CRITICAL"
                        : survivor.Hunger >= 90f || survivor.Thirst >= 90f || survivor.Warmth <= 20f
                            ? "STRAINED"
                            : "STABLE";
                float lifetimeDose = slice?.lifetimeRadiationExposure ?? 0f;

                if (!FilterPass(status, survivor.IsAliveState)) continue;

                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon(lifetimeDose >= 50f ? "badge_rad_sickness" : survivor.Health < 30f ? "badge_trench_foot" : "badge_exhaustion", 22);
                row.AddChild(icon);
                var nameLbl = AshfallUiHelpers.MakeSmall(displayName);
                nameLbl.CustomMinimumSize = new Vector2(140, 0);
                row.AddChild(nameLbl);
                var statsText = AshfallUiHelpers.MakeMono(
                    $"HP {survivor.Health:0} · HUN {survivor.Hunger:0} · THI {survivor.Thirst:0} · " +
                    $"WARM {survivor.Warmth:0} · RAD {lifetimeDose:0} mSv");
                statsText.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                statsText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
                row.AddChild(statsText);
                var statusLbl = AshfallUiHelpers.MakeSmall($"[{status}]");
                statusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(StatusColor(status)));
                row.AddChild(statusLbl);
                _survivorList.AddChild(row);
                rendered++;
            }

            if (rendered == 0)
            {
                if (_survivorsHost.RosterState.Count == 0)
                    _survivorList.AddChild(AshfallUiHelpers.MakeMetadata("Roster empty. No registered shelter survivors."));
                else
                    _survivorList.AddChild(AshfallUiHelpers.MakeMetadata("No survivors match the current filter."));
            }
        }

        private void RefreshCohortStats()
        {
            if (_statsGroup == null) return;
            AshfallUiHelpers.EmptyChildren(_statsGroup);
            if (_survivorsHost == null) return;

            float avgHp = _survivorsHost.RosterState.Count == 0 ? 0f : _survivorsHost.RosterState.Average(s => s?.Health ?? 0f);
            float avgMor = _survivorsHost.RosterState.Count == 0 ? 0f : _survivorsHost.RosterState.Average(s => s?.Morale ?? 0f);
            _statsGroup.AddChild(AshfallUiHelpers.MakeBody($"Cohort morale reads {avgMor:0}% (bunker-wide); " +
                $"average survivor wellness sits at {avgHp:0}% HP. " +
                "Skill Matrix is deferred — see docs/ui/PHASE13_DATA_AVAILABILITY.md."));
            if (!string.IsNullOrWhiteSpace(_survivorsHost.LastEvent))
                _statsGroup.AddChild(AshfallUiHelpers.MakeMetadata($"Latest roster event: {_survivorsHost.LastEvent}"));
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

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            _shell = new AshfallDashboardShell(
                "SURVIVOR ROSTER & DUTY COHORT",
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
                new AshfallSidebar.Item { Id = "filter_all",      Label = "Filter: All",        Hint = "every survivor" },
                new AshfallSidebar.Item { Id = "filter_living",   Label = "Filter: Living",     Hint = "alive only" },
                new AshfallSidebar.Item { Id = "filter_strained", Label = "Filter: Strained",   Hint = "STRAINED + worse" },
                new AshfallSidebar.Item { Id = "filter_critical", Label = "Filter: Critical",   Hint = "CRITICAL + DEAD" },
            }, "ROSTER OPS", "filter_all");
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
            _statusRail.AddCard("living",   "LIVING",   "—",   AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("avgHp",    "AVG HP",   "—%",  AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("avgRad",   "AVG RAD",  "—",   AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("avgMor",   "AVG MOR",  "—%",  AshfallMetricCard.Criticality.Normal, 110);
            _statusRail.AddCard("strained", "STRAINED", "0",   AshfallMetricCard.Criticality.Normal, 120);

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
            listCol.AddChild(AshfallUiHelpers.MakeSectionHeader("RESIDENT ROSTER"));
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
            rVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("COHORT TELEMETRY"));
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
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }

        private static (float r, float g, float b, float a) StatusColor(string status) => status switch
        {
            "CRITICAL" => DesignTheme.Critical,
            "STRAINED" => DesignTheme.Warm,
            "DEAD" => DesignTheme.Muted,
            _ => DesignTheme.Pale,
        };
    }
}
