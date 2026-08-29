using System;
using System.Linq;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Survivor Detail panel. Shows per-survivor info, needs, traits,
    /// and status — bound to the live SurvivorsHostSession for a specific
    /// survivor id.
    ///
    /// Ticket #125: layout chrome (dialog frame, section headers, separators,
    /// close button) is owned by
    /// <c>res://assets/ui/panels/SurvivorDetailPanel.tscn</c>. Dynamic content
    /// (the four lists) is filled at refresh time.
    /// </summary>
    public partial class SurvivorDetailPanel : Control
    {
        public event Action? OnClose;

        private SceneBinder? _binder;
        private VBoxContainer _survivorInfo = null!;
        private VBoxContainer _needsList = null!;
        private VBoxContainer _traitsList = null!;
        private VBoxContainer _statusList = null!;
        private Button _closeButton = null!;

        private SurvivorsHostSession? _survivors;
        private string _survivorId = string.Empty;

        public bool IsBound => _survivors != null && !string.IsNullOrEmpty(_survivorId);
        public int RenderedRowCount { get; private set; }

        public void Bind(SurvivorsHostSession? survivors, string survivorId)
        {
            _survivors = survivors;
            _survivorId = survivorId ?? string.Empty;
            RefreshView();
        }

        public override void _Ready()
        {
            _binder = new SceneBinder(this, typeof(SurvivorDetailPanel));
            _binder.Require<VBoxContainer>("SurvivorInfo");
            _binder.Require<VBoxContainer>("NeedsList");
            _binder.Require<VBoxContainer>("TraitsList");
            _binder.Require<VBoxContainer>("StatusList");
            _binder.Require<Button>("CloseButton");

            _survivorInfo = _binder.Get<VBoxContainer>("SurvivorInfo");
            _needsList = _binder.Get<VBoxContainer>("NeedsList");
            _traitsList = _binder.Get<VBoxContainer>("TraitsList");
            _statusList = _binder.Get<VBoxContainer>("StatusList");
            _closeButton = _binder.Get<Button>("CloseButton");
            _closeButton.Pressed += () => OnClose?.Invoke();

            Visible = false;
        }

        public void RefreshView()
        {
            if (_survivorInfo == null || _needsList == null || _traitsList == null || _statusList == null) return;

            AshfallUiHelpers.EmptyChildren(_survivorInfo);
            AshfallUiHelpers.EmptyChildren(_needsList);
            AshfallUiHelpers.EmptyChildren(_traitsList);
            AshfallUiHelpers.EmptyChildren(_statusList);

            RenderedRowCount = 0;

            if (_survivors == null || string.IsNullOrEmpty(_survivorId))
            {
                _survivorInfo.AddChild(MakeDimLine("No survivor selected."));
                return;
            }

            var s = _survivors.RosterState.FirstOrDefault(r => r != null && r.Id == _survivorId);
            if (s == null)
            {
                _survivorInfo.AddChild(MakeDimLine($"Survivor '{_survivorId}' not found in roster."));
                return;
            }

            var rad = _survivors.RadStateFor(s.Id);

            // ── Survivor info ──
            AddRow(_survivorInfo, $"Name: {Name(s.Id)}", Ashfall.Core.UI.Theme.Pale);
            AddRow(_survivorInfo, $"Alive: {s.IsAlive}", s.IsAlive ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Critical);
            AddRow(_survivorInfo, $"Max Health Cap: {s.MaxHealthCap:0}", Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 3;

            // ── Needs ──
            AddRow(_needsList, $"Health: {s.Health:0} / {s.MaxHealthCap:0}", s.Health < 30 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe);
            AddRow(_needsList, $"Hunger: {s.Hunger:0}", s.Hunger >= 90 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"Thirst: {s.Thirst:0}", s.Thirst >= 90 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"Fatigue: {s.Fatigue:0}", s.Fatigue >= 90 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"Warmth: {s.Warmth:0}", s.Warmth < 20 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"Morale: {s.Morale:0}", s.Morale < 20 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale);
            AddRow(_needsList, $"Hygiene: {s.Hygiene:0}", Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 7;

            // ── Traits (from rad state) ──
            if (rad != null)
            {
                AddRow(_traitsList, $"Radiation Dose: {rad.RadiationDose:0} mSv", rad.RadiationDose >= 50 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe);
                AddRow(_traitsList, $"Lifetime Exposure: {rad.LifetimeRadiationExposure:0} mSv", Ashfall.Core.UI.Theme.Dim);
                AddRow(_traitsList, $"Rad Resistance: {rad.HasRadResistance}{(rad.HasRadResistance ? $" ({rad.RadResistanceHoursRemaining:0}h)" : "")}",
                    rad.HasRadResistance ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Dim);
                AddRow(_traitsList, $"Acute Sickness: {rad.HasAcuteRadiationSickness}", rad.HasAcuteRadiationSickness ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Dim);
                AddRow(_traitsList, $"Chronic Illness: {rad.HasChronicIllness}", rad.HasChronicIllness ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount += 5;
            }
            else
            {
                _traitsList.AddChild(MakeDimLine("No radiation state tracked."));
            }

            // ── Status ──
            AddRow(_statusList, $"Critical flags: hunger={s.WasHungerCritical} thirst={s.WasThirstCritical} warmth={s.WasWarmthCritical}",
                (s.WasHungerCritical || s.WasThirstCritical || s.WasWarmthCritical) ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount++;
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

        private static string Name(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            int us = id.IndexOf('_');
            return us >= 0 ? id.Substring(us + 1).Replace('_', ' ') : id;
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
