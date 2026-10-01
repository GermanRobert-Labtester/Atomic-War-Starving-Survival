// SPDX-License-Identifier: MIT
using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Achievements panel.
    /// Shows real survival milestones derived from live state (days survived,
    /// roster health, radiation management) — no fabricated achievements.
    /// An AchievementsHostSession does not exist yet; milestones are derived
    /// from the SurvivorsHostSession + sim day.
    /// </summary>
    public partial class AchievementsPanel : Control
    {
        public event Action? OnClose;

        private Label _lblStatsTitle;
        private VBoxContainer _statsList;
        private Label _lblAchievementsTitle;
        private VBoxContainer _achievementsList;
        private Label _lblMilestonesTitle;
        private VBoxContainer _milestonesList;

        private SurvivorsHostSession? _survivors;
        private int _simDay = 1;
        private Ashfall.Core.Achievements.AchievementSystem? _achievementSystem;

        public bool IsBound => _survivors != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(SurvivorsHostSession? survivors, int simDay = 1, Ashfall.Core.Achievements.AchievementSystem? achievementSystem = null)
        {
            if (_survivors != null) _survivors.StateChanged -= RefreshView;

            _survivors = survivors;
            _simDay = simDay;
            _achievementSystem = achievementSystem;

            if (_survivors != null) _survivors.StateChanged += RefreshView;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_statsList == null || _achievementsList == null || _milestonesList == null) return;

            AshfallUiHelpers.EmptyChildren(_statsList);
            AshfallUiHelpers.EmptyChildren(_achievementsList);
            AshfallUiHelpers.EmptyChildren(_milestonesList);

            RenderedRowCount = 0;

            if (_survivors?.RosterState == null || _survivors.RosterState.Count == 0)
            {
                _statsList.AddChild(MakeDimLine(Tr("ui.achievements.no_roster", "No survivor roster bound.")));
                return;
            }

            var roster = _survivors.RosterState.Where(s => s != null).ToList();
            int alive = roster.Count(s => s.IsAlive);
            float avgHealth = roster.Count > 0 ? roster.Average(s => s.Health) : 0f;
            float avgDose = roster.Count > 0 ? roster.Average(s => _survivors.RadStateFor(s.Id)?.RadiationDose ?? 0f) : 0f;

            // ── Stats ──
            AddRow(_statsList, TrFormat("ui.achievements.days_survived", _simDay), Ashfall.Core.UI.Theme.Warm);
            AddRow(_statsList, TrFormat("ui.achievements.roster_alive", alive, roster.Count), alive == roster.Count ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Critical);
            AddRow(_statsList, TrFormat("ui.achievements.avg_health", $"{avgHealth:0}"), Ashfall.Core.UI.Theme.Pale);
            AddRow(_statsList, TrFormat("ui.achievements.avg_dose", $"{avgDose:0.0}"), AshfallUiBands.ForDose(avgDose));
            RenderedRowCount += 4;

            // ── Achievements ──
            if (_achievementSystem != null)
            {
                _achievementSystem.EvaluateRosterSnapshot(_simDay, roster.Count, alive, avgHealth, avgDose, 50.0f);
                var unlocked = _achievementSystem.GetCompletedAchievementIds();
                foreach (var def in _achievementSystem.Catalog.All)
                {
                    if (unlocked.Contains(def.Id))
                    {
                        AddRow(_achievementsList, $"{AchievementName(def)} — {AchievementDescription(def)}", Ashfall.Core.UI.Theme.Warm);
                        RenderedRowCount++;
                    }
                }
                if (RenderedRowCount == 4)
                    _achievementsList.AddChild(MakeDimLine(Tr("ui.achievements.no_milestones", "No milestones reached yet.")));
            }
            else
            {
                // Derived fallback when AchievementSystem is not bound
                if (_simDay >= 7)
                { AddRow(_achievementsList, Tr("ui.achievements.milestone.first_week", "First Week Survivor — reached Day 7"), Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; }
                if (_simDay >= 14)
                { AddRow(_achievementsList, Tr("ui.achievements.milestone.two_week", "Two-Week Endurance — reached Day 14"), Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; }
                if (_simDay >= 30)
                { AddRow(_achievementsList, Tr("ui.achievements.milestone.month_ash", "Month of Ash — reached Day 30"), Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; }
                if (alive == roster.Count && roster.Count > 0)
                { AddRow(_achievementsList, Tr("ui.achievements.milestone.no_casualties", "No Casualties — full roster alive"), Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
                if (avgDose < 20 && roster.Count > 0)
                { AddRow(_achievementsList, Tr("ui.achievements.milestone.low_exposure", "Low Exposure — average dose below 20 mSv"), Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
                if (avgHealth > 80 && roster.Count > 0)
                { AddRow(_achievementsList, Tr("ui.achievements.milestone.healthy_cohort", "Healthy Cohort — average health above 80"), Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
                if (RenderedRowCount == 4)
                    _achievementsList.AddChild(MakeDimLine(Tr("ui.achievements.no_milestones", "No milestones reached yet.")));
            }

            // ── Milestones (next targets) ──
            AddRow(_milestonesList, _simDay < 7 ? Tr("ui.achievements.target.day7", "Next: survive to Day 7") : Tr("ui.achievements.target.day7_done", "Day 7 milestone reached"), _simDay < 7 ? Ashfall.Core.UI.Theme.Dim : Ashfall.Core.UI.Theme.Lethe);
            AddRow(_milestonesList, _simDay < 14 ? Tr("ui.achievements.target.day14", "Next: survive to Day 14") : Tr("ui.achievements.target.day14_done", "Day 14 milestone reached"), _simDay < 14 ? Ashfall.Core.UI.Theme.Dim : Ashfall.Core.UI.Theme.Lethe);
            AddRow(_milestonesList, _simDay < 30 ? Tr("ui.achievements.target.day30", "Next: survive to Day 30") : Tr("ui.achievements.target.day30_done", "Day 30 milestone reached"), _simDay < 30 ? Ashfall.Core.UI.Theme.Dim : Ashfall.Core.UI.Theme.Lethe);
            RenderedRowCount += 3;
        }

        private static string Tr(string key, string fallback) => AshfallUiText.Tr(key, fallback);

        /// <summary>Per-achievement catalog key with the authored name fallback.
        /// The achievement catalog is data; a localized row can override it
        /// without a Core change once the key exists.</summary>
        private static string AchievementName(Ashfall.Core.Achievements.AchievementDefinition def)
            => AshfallUiText.Tr($"achievement.{def.Id}.name", def.Name);

        /// <summary>Per-achievement description catalog key with the authored fallback.</summary>
        private static string AchievementDescription(Ashfall.Core.Achievements.AchievementDefinition def)
            => AshfallUiText.Tr($"achievement.{def.Id}.description", def.Description);

        private static string TrFormat(string key, params object[] args) => AshfallUiText.TrFormat(key, args);

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            return l;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = AshfallUiHelpers.PanelScrim() };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle(Tr("ui.achievements.title", "ACHIEVEMENTS & MILESTONES"), Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblStatsTitle = AshfallUiHelpers.MakeSectionHeader(Tr("ui.achievements.section.stats", "RUN STATISTICS"));
            vbox.AddChild(_lblStatsTitle);
            _statsList = new VBoxContainer();
            _statsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _statsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_statsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblAchievementsTitle = AshfallUiHelpers.MakeSectionHeader(Tr("ui.achievements.section.earned", "EARNED MILESTONES"));
            vbox.AddChild(_lblAchievementsTitle);
            _achievementsList = new VBoxContainer();
            _achievementsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _achievementsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_achievementsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblMilestonesTitle = AshfallUiHelpers.MakeSectionHeader(Tr("ui.achievements.section.targets", "NEXT TARGETS"));
            vbox.AddChild(_lblMilestonesTitle);
            _milestonesList = new VBoxContainer();
            _milestonesList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _milestonesList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_milestonesList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton(Tr("ui.achievements.close", "CLOSE [Esc]"), () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);
        }

        public void Open()
        {
            Visible = true;
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
    }
}
