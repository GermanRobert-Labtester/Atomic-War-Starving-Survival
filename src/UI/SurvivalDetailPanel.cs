using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
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

        private VBoxContainer _contentVBox = null!;
        private Label _lblHealthTitle;
        private VBoxContainer _healthData;
        private Label _lblNeedsTitle;
        private VBoxContainer _needsData;
        private Label _lblRadiationTitle;
        private VBoxContainer _radiationData;
        private Label _lblStatusTitle;
        private VBoxContainer _statusData;

        private SurvivorsHostSession? _survivors;

        public bool IsBound => _survivors != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(SurvivorsHostSession? survivors)
        {
            _survivors = survivors;
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
                _healthData.AddChild(MakeDimLine("No survivor roster bound."));
                return;
            }

            var roster = _survivors.RosterState.Where(s => s != null).ToList();
            int alive = roster.Count(s => s.IsAlive);
            float avgHealth = roster.Count > 0 ? roster.Average(s => s.Health) : 0f;
            float avgHunger = roster.Count > 0 ? roster.Average(s => s.Hunger) : 0f;
            float avgThirst = roster.Count > 0 ? roster.Average(s => s.Thirst) : 0f;
            float avgFatigue = roster.Count > 0 ? roster.Average(s => s.Fatigue) : 0f;
            float avgMorale = roster.Count > 0 ? roster.Average(s => s.Morale) : 0f;
            float avgDose = roster.Count > 0 ? roster.Average(s => _survivors.RadStateFor(s.Id)?.RadiationDose ?? 0f) : 0f;

            AddRow(_healthData, $"Roster: {alive} / {roster.Count} alive", alive < roster.Count ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe);
            AddRow(_healthData, $"Avg Health: {avgHealth:0} / 100", avgHealth < 50 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Lethe);
            RenderedRowCount += 2;

            AddRow(_needsData, $"Avg Hunger: {avgHunger:0}", avgHunger >= 80 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsData, $"Avg Thirst: {avgThirst:0}", avgThirst >= 80 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsData, $"Avg Fatigue: {avgFatigue:0}", avgFatigue >= 80 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsData, $"Avg Morale: {avgMorale:0} / 100", avgMorale < 30 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale);
            RenderedRowCount += 4;

            AddRow(_radiationData, $"Avg Dose: {avgDose:0.0} mSv", avgDose >= 50 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe);
            int dosed = roster.Count(s => (_survivors.RadStateFor(s.Id)?.RadiationDose ?? 0f) >= 50f);
            AddRow(_radiationData, $"Survivors above 50 mSv: {dosed}", dosed > 0 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 2;

            int critical = roster.Count(s => s.IsAlive && s.Health < 30f);
            AddRow(_statusData, $"Critical health: {critical} survivor(s)", critical > 0 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Dim);
            AddRow(_statusData, $"Shelter weakest ceiling: {_survivors.Shelter?.GetWeakestCeilingAttenuation() * 100f ?? 0:0}%", Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 2;
        }

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
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
