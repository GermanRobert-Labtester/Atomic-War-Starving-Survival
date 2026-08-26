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
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("SURVIVAL DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHealthTitle = AshfallUiHelpers.MakeSectionHeader("ROSTER HEALTH");
            vbox.AddChild(_lblHealthTitle);
            _healthData = new VBoxContainer();
            _healthData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _healthData.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_healthData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblNeedsTitle = AshfallUiHelpers.MakeSectionHeader("AVERAGE NEEDS");
            vbox.AddChild(_lblNeedsTitle);
            _needsData = new VBoxContainer();
            _needsData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _needsData.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_needsData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblRadiationTitle = AshfallUiHelpers.MakeSectionHeader("RADIATION OVERVIEW");
            vbox.AddChild(_lblRadiationTitle);
            _radiationData = new VBoxContainer();
            _radiationData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _radiationData.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_radiationData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblStatusTitle = AshfallUiHelpers.MakeSectionHeader("STATUS SUMMARY");
            vbox.AddChild(_lblStatusTitle);
            _statusData = new VBoxContainer();
            _statusData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _statusData.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_statusData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
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
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
