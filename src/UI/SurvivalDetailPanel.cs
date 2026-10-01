// SPDX-License-Identifier: MIT
using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.Radiation;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Survival Detail panel.
    /// Shows overall survival state across the roster — health, needs, radiation,
    /// and status — bound to the live SurvivorsHostSession.
    /// </summary>
    public partial class SurvivalDetailPanel : Control
    {
        public event Action? OnClose;

        // Loop-1 of the verification pass — the four *_lbl*Title fields below
        // were declared and never read or written: this panel's section titles
        // live in SurvivalDetailPanel.tscn and are bound by SceneBinder, so the
        // C# handles were dead state that a future edit could easily have
        // written to and believed was displayed. Removed.

        private VBoxContainer _healthData;
        private VBoxContainer _needsData;
        private VBoxContainer _radiationData;
        private VBoxContainer _statusData;

        private SurvivorsHostSession? _survivors;

        public bool IsBound => _survivors != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(SurvivorsHostSession? survivors)
        {
            if (_survivors != null) _survivors.StateChanged -= RefreshView;

            _survivors = survivors;

            if (_survivors != null) _survivors.StateChanged += RefreshView;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_healthData == null || _needsData == null || _radiationData == null || _statusData == null) return;

            AshfallUiHelpers.EmptyChildren(_healthData);
            AshfallUiHelpers.EmptyChildren(_needsData);
            AshfallUiHelpers.EmptyChildren(_radiationData);
            AshfallUiHelpers.EmptyChildren(_statusData);

            RenderedRowCount = 0;

            if (_survivors?.RosterState == null || _survivors.RosterState.Count == 0)
            {
                _healthData.AddChild(MakeDimLine(Tr("ui.survival_detail.no_roster", "No survivor roster bound.")));
                return;
            }

            var roster = _survivors.RosterState.Where(s => s != null).ToList();
            var profile = _survivors.Needs.Profile;
            int alive = roster.Count(s => s.IsAlive);
            float avgHealth = roster.Count > 0 ? roster.Average(s => s.Health) : 0f;
            float avgHunger = roster.Count > 0 ? roster.Average(s => s.Hunger) : 0f;
            float avgThirst = roster.Count > 0 ? roster.Average(s => s.Thirst) : 0f;
            float avgFatigue = roster.Count > 0 ? roster.Average(s => s.Fatigue) : 0f;
            float avgMorale = roster.Count > 0 ? roster.Average(s => s.Morale) : 0f;
            float avgDose = roster.Count > 0 ? roster.Average(s => _survivors.RadStateFor(s.Id)?.RadiationDose ?? 0f) : 0f;

            // Task 1 — every band below now reads the owning authority. This
            // panel previously banded against literals that contradicted the
            // simulation: health warned at a hardcoded 50 (profile says
            // healthWarn 30 / healthCritical 25) and hunger/thirst went Critical
            // at a hardcoded 80 while the profile's critical values are 90. The
            // panel was teaching thresholds the game does not enforce.
            AddRow(_healthData, TrFormat("ui.survival_detail.roster", alive, roster.Count),
                alive < roster.Count ? AshfallUiBands.Critical : AshfallUiBands.Calm);
            AddRow(_healthData, TrFormat("ui.survival_detail.avg_health", $"{avgHealth:0}"),
                AshfallUiBands.ForNeed(avgHealth, highIsBad: false, warnAt: profile.healthWarn, criticalAt: profile.healthCritical));
            RenderedRowCount += 2;

            AddRow(_needsData, TrFormat("ui.survival_detail.avg_hunger", $"{avgHunger:0}"),
                AshfallUiBands.ForNeed(avgHunger, highIsBad: true, warnAt: profile.hungerWarn, criticalAt: profile.hungerCritical));
            AddRow(_needsData, TrFormat("ui.survival_detail.avg_thirst", $"{avgThirst:0}"),
                AshfallUiBands.ForNeed(avgThirst, highIsBad: true, warnAt: profile.thirstWarn, criticalAt: profile.thirstCritical));
            AddRow(_needsData, TrFormat("ui.survival_detail.avg_fatigue", $"{avgFatigue:0}"),
                AshfallUiBands.ForNeed(avgFatigue, highIsBad: true, warnAt: profile.fatigueWarn, criticalAt: profile.fatigueCritical));
            AddRow(_needsData, TrFormat("ui.survival_detail.avg_morale", $"{avgMorale:0}"),
                AshfallUiBands.ForLow(avgMorale, warnAt: profile.moraleWarn, criticalAt: profile.moraleCritical));
            RenderedRowCount += 4;

            // Loop-4 (previous package) — this is the cohort twin of the
            // per-survivor dose row in SurvivorDetailPanel and the avg-dose card
            // in StatusPanel: same roster, same RadiationDose field, same
            // meaning. All three share one band authority.
            AddRow(_radiationData, TrFormat("ui.survival_detail.avg_dose", avgDose),
                AshfallUiBands.ForDose(avgDose));
            int dosed = roster.Count(s => (_survivors.RadStateFor(s.Id)?.RadiationDose ?? 0f) >= RadiationSystem.WarnThreshold);
            // Loop-2 — "0 survivors above threshold" is good news, but it was
            // rendered in Dim, the muted token this panel uses for absent
            // secondary detail. A healthy cohort read as broken. Healthy states
            // now use the calm token; only the alarming state is coloured.
            AddRow(_radiationData, TrFormat("ui.survival_detail.above_threshold", $"{RadiationSystem.WarnThreshold:0}", dosed),
                dosed > 0 ? Ashfall.Core.UI.Theme.Warm : AshfallUiBands.Calm);
            RenderedRowCount += 2;

            int critical = roster.Count(s => s.IsAlive && s.Health < profile.healthCritical);
            AddRow(_statusData, TrFormat("ui.survival_detail.critical_health", critical),
                critical > 0 ? Ashfall.Core.UI.Theme.Critical : AshfallUiBands.Calm);

            // Loop-2 — the shelter figure was the panel's last unbanded
            // measurement: it read the same Dim regardless of value, so a
            // collapsing shelter was visually identical to a sound one. Banded
            // with named consts, matching how the stores rows treat host-side
            // presentation thresholds (no simulation owner exists for this).
            float weakestCeiling = _survivors.Shelter?.GetWeakestCeilingAttenuation() * 100f ?? 0f;
            AddRow(_statusData, TrFormat("ui.survival_detail.weakest_ceiling", $"{weakestCeiling:0}"),
                weakestCeiling <= 0f ? AshfallUiBands.Critical
                    : AshfallUiBands.ForLow(weakestCeiling, warnAt: ShelterWarnCeilingPct, criticalAt: ShelterCriticalCeilingPct));
            RenderedRowCount += 2;
        }

        private const float ShelterCriticalCeilingPct = 20f;
        private const float ShelterWarnCeilingPct = 50f;

        // Task 2 — the panel previously had no localization helper at all; every
        // row was a hardcoded English interpolation. This is the same shared
        // accessor the other panels use (task 4 of the previous package).
        private static string Tr(string key, string fallback) => AshfallUiText.Tr(key, fallback);

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
            // Ticket #125: layout chrome owned by res://assets/ui/panels/SurvivalDetailPanel.tscn; SceneBinder resolves typed unique-name nodes once.
            // Sibling refresh code is unchanged.
            var binder = new SceneBinder(this, typeof(SurvivalDetailPanel));
            binder.Require<VBoxContainer>("HealthData");
            binder.Require<VBoxContainer>("NeedsData");
            binder.Require<VBoxContainer>("RadiationData");
            binder.Require<VBoxContainer>("StatusData");
            binder.Require<Button>("CloseButton");
            _healthData = binder.Get<VBoxContainer>("HealthData");
            _needsData = binder.Get<VBoxContainer>("NeedsData");
            _radiationData = binder.Get<VBoxContainer>("RadiationData");
            _statusData = binder.Get<VBoxContainer>("StatusData");
            binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();

            Visible = false;
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
