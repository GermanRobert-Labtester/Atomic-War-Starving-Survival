// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Difficulty;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using AtomicWar.GodotApp.Localization;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 181 / DEBT-PLAN181-DIFFICULTY-RUNTIME-UI — the runtime difficulty
    /// settings surface.
    ///
    /// This panel is a truthful front end for the already-shipped
    /// <see cref="DifficultySettingsHostSession"/> authority: authored preset
    /// selection, the custom scalar lanes, and the irreversible ironman lock.
    /// It never owns difficulty state and never rebinds scalar consumers — every
    /// edit is routed through the host delegates, which call the canonical
    /// <c>Main</c> commands so the XP-01 bound consumers see the new provider.
    ///
    /// When the campaign is locked the controls are disabled and the panel
    /// states why, rather than pretending the edit was accepted.
    /// </summary>
    public partial class DifficultySettingsPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private DifficultySettingsHostSession? _host;
        private Func<string, bool>? _applyPreset;
        private Func<string, float, bool>? _applyScalar;
        private Func<bool>? _applyLock;
        private Func<bool>? _isDirty;
        private Action? _save;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _presetRows = null!;
        private VBoxContainer _scalarRows = null!;
        private Button _lockButton = null!;
        private Label _feedback = null!;
        private Label _offline = null!;
        private Label _saveState = null!;
        private Button _saveButton = null!;

        private readonly Dictionary<string, HSlider> _sliders = new(StringComparer.Ordinal);
        private readonly Dictionary<string, Label> _valueLabels = new(StringComparer.Ordinal);
        private bool _refreshing;
        private bool _lockConfirmPending;

        public bool IsBound => _host != null;

        public void Bind(
            DifficultySettingsHostSession session,
            Func<string, bool> applyPreset,
            Func<string, float, bool> applyScalar,
            Func<bool> applyLock,
            Func<bool>? isDirty = null,
            Action? save = null)
        {
            Unbind();
            _host = session;
            _applyPreset = applyPreset;
            _applyScalar = applyScalar;
            _applyLock = applyLock;
            _isDirty = isDirty;
            _save = save;
            _lockConfirmPending = false;
            if (_host != null)
                _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
            _applyPreset = null;
            _applyScalar = null;
            _applyLock = null;
            _isDirty = null;
            _save = null;
            _lockConfirmPending = false;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Difficulty // Campaign Parameters", minWidth: 900, minHeight: 620);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("preset", "Active Preset", "STANDARD", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("state", "Campaign Lock", "OPEN", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            var content = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            content.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);

            _offline = AshfallUiHelpers.MakeBody(AshfallLocalization.Tr(
                "ui.difficulty.offline",
                "Difficulty settings are offline until a campaign is composed."));
            _offline.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            content.AddChild(_offline);

            var presetHeader = AshfallUiHelpers.MakeSectionHeader("AUTHORED PRESETS");
            content.AddChild(presetHeader);
            _presetRows = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _presetRows.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            content.AddChild(_presetRows);

            var scalarHeader = AshfallUiHelpers.MakeSectionHeader("CUSTOM SCALARS");
            content.AddChild(scalarHeader);

            var bandNote = AshfallUiHelpers.MakeSmall("Custom lanes clamp to the authored 0.25–2.50 band. Any edit switches the campaign to CUSTOM.");
            bandNote.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            content.AddChild(bandNote);

            _scalarRows = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _scalarRows.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            content.AddChild(_scalarRows);

            // Persistence surfacing: the panel states whether the checksummed
            // difficulty_settings section has unsaved changes and exposes an
            // explicit save of that canonical section (never a parallel store).
            var saveRow = new HBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _saveState = AshfallUiHelpers.MakeSmall(string.Empty);
            _saveState.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _saveState.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            saveRow.AddChild(_saveState);
            _saveButton = AshfallUiHelpers.MakeButton("SAVE SETTINGS", OnSavePressed);
            _saveButton.CustomMinimumSize = new Vector2(160, 28);
            saveRow.AddChild(_saveButton);
            content.AddChild(saveRow);

            _lockButton = AshfallUiHelpers.MakeButton("LOCK DIFFICULTY (IRONMAN)", OnLockPressed);
            _lockButton.CustomMinimumSize = new Vector2(280, 32);
            content.AddChild(_lockButton);

            _feedback = AshfallUiHelpers.MakeSmall(string.Empty);
            _feedback.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            content.AddChild(_feedback);

            _shell.SetContent(content);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            BuildPresetRows();
            BuildScalarRows();
            RefreshView();
        }

        private void BuildPresetRows()
        {
            if (_presetRows == null) return;
            AshfallUiHelpers.EmptyChildren(_presetRows);
            if (_host == null) return;

            foreach (var preset in _host.Presets)
            {
                string id = preset.id;
                var row = new HBoxContainer();
                var button = AshfallUiHelpers.MakeButton(preset.display_name ?? id, () => OnPresetPressed(id));
                button.CustomMinimumSize = new Vector2(180, 28);
                row.AddChild(button);

                var description = AshfallUiHelpers.MakeSmall(preset.description ?? string.Empty);
                description.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                description.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                row.AddChild(description);
                _presetRows.AddChild(row);
            }
        }

        private void BuildScalarRows()
        {
            if (_scalarRows == null) return;
            AshfallUiHelpers.EmptyChildren(_scalarRows);
            _sliders.Clear();
            _valueLabels.Clear();

            foreach (string name in DifficultySettingsHostSession.ScalarNames)
            {
                var row = new HBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };

                var label = AshfallUiHelpers.MakeSmall(LabelFor(name));
                label.CustomMinimumSize = new Vector2(220, 0);
                row.AddChild(label);

                var slider = new HSlider
                {
                    MinValue = 0.25,
                    MaxValue = 2.5,
                    Step = 0.05,
                    SizeFlagsHorizontal = SizeFlags.ExpandFill,
                    CustomMinimumSize = new Vector2(220, 28)
                };
                string captured = name;
                slider.ValueChanged += value => OnScalarChanged(captured, (float)value);
                row.AddChild(slider);

                var value = AshfallUiHelpers.MakeMono("1.00");
                value.CustomMinimumSize = new Vector2(64, 0);
                row.AddChild(value);

                _sliders[name] = slider;
                _valueLabels[name] = value;
                _scalarRows.AddChild(row);
            }
        }

        private static string LabelFor(string scalarName) => scalarName switch
        {
            "hunger_rate_mult" => "Hunger rate",
            "thirst_rate_mult" => "Thirst rate",
            "radiation_gain_mult" => "Radiation gain",
            "disease_onset_mult" => "Disease onset",
            "hostile_encounter_mult" => "Hostile encounters",
            "enemy_damage_mult" => "Enemy damage",
            "market_price_mult" => "Market prices",
            "equipment_decay_mult" => "Equipment decay",
            "crisis_deadline_mult" => "Crisis deadlines",
            _ => scalarName
        };

        private void OnPresetPressed(string presetId)
        {
            _lockConfirmPending = false;
            bool applied = _applyPreset?.Invoke(presetId) ?? false;
            SetFeedback(applied
                ? $"Applied preset {presetId}."
                : "Preset refused — the campaign is locked.");
            RefreshView();
        }

        private void OnScalarChanged(string scalarName, float value)
        {
            if (_refreshing) return;
            _lockConfirmPending = false;
            bool applied = _applyScalar?.Invoke(scalarName, value) ?? false;
            if (!applied)
            {
                SetFeedback("Scalar refused — the campaign is locked.");
                RefreshView();
                return;
            }

            SetFeedback($"{LabelFor(scalarName)} set to {value:0.00} (custom).");
            // Update the readout without re-entering the slider.
            if (_valueLabels.TryGetValue(scalarName, out var valueLabel))
                valueLabel.Text = value.ToString("0.00");
            RefreshPrestateOnly();
        }

        private void OnLockPressed()
        {
            if (_host == null || _host.IsLocked)
            {
                RefreshView();
                return;
            }

            // The ironman lock is irreversible for the campaign, so require a
            // second deliberate press rather than locking on a stray click.
            if (!_lockConfirmPending)
            {
                _lockConfirmPending = true;
                SetFeedback("Press LOCK again to confirm — this cannot be undone for this campaign.");
                RefreshView();
                return;
            }

            _lockConfirmPending = false;
            bool locked = _applyLock?.Invoke() ?? false;
            SetFeedback(locked
                ? "Difficulty locked for the rest of the campaign."
                : "Lock unavailable (already locked or no active campaign).");
            RefreshView();
        }

        private void OnSavePressed()
        {
            if (_save == null)
            {
                SetFeedback("Save unavailable — no active campaign.");
                return;
            }
            _save();
            SetFeedback("Difficulty settings saved.");
            RefreshView();
        }

        private void SetFeedback(string text)
        {
            if (_feedback != null)
                _feedback.Text = text;
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null)
            {
                if (_offline != null)
                    _offline.Text = AshfallLocalization.Tr(
                        "ui.difficulty.offline",
                        "Difficulty settings are offline until a campaign is composed.");
                return;
            }

            if (_offline != null)
                _offline.Text = string.Empty;

            RefreshPrestateOnly();

            _refreshing = true;
            foreach (string name in DifficultySettingsHostSession.ScalarNames)
            {
                float value = ScalarValue(_host.EffectiveScalars, name);
                if (_sliders.TryGetValue(name, out var slider))
                {
                    slider.Value = value;
                    slider.Editable = !_host.IsLocked;
                }
                if (_valueLabels.TryGetValue(name, out var label))
                    label.Text = value.ToString("0.00");
            }
            _refreshing = false;

            if (_lockButton != null)
            {
                _lockButton.Disabled = _host.IsLocked;
                _lockButton.Text = _host.IsLocked
                    ? "DIFFICULTY LOCKED"
                    : (_lockConfirmPending ? "CONFIRM LOCK — CANNOT BE UNDONE" : "LOCK DIFFICULTY (IRONMAN)");
            }

            if (_saveState != null && _saveButton != null)
            {
                bool dirty = _isDirty?.Invoke() ?? false;
                _saveState.Text = dirty ? "UNSAVED CHANGES" : "Settings saved.";
                _saveState.AddThemeColorOverride("font_color",
                    AshfallUiHelpers.ToColor(dirty ? DesignTheme.Warm : DesignTheme.Pale));
                _saveButton.Disabled = !dirty || _save == null;
            }
        }

        private void RefreshPrestateOnly()
        {
            if (_host == null || _statusRail == null) return;
            _statusRail.Set("preset", _host.ActiveDisplayName(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("state", _host.IsLocked ? "LOCKED" : "OPEN",
                _host.IsLocked ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            if (_presetRows == null) return;
            foreach (Node child in _presetRows.GetChildren())
            {
                if (child is HBoxContainer row && row.GetChildCount() > 0 && row.GetChild(0) is Button button)
                    button.Disabled = _host.IsLocked || _host.IsCustom;
            }
        }

        private static float ScalarValue(DifficultyScalars scalars, string name) => name switch
        {
            "hunger_rate_mult" => scalars.hunger_rate_mult,
            "thirst_rate_mult" => scalars.thirst_rate_mult,
            "radiation_gain_mult" => scalars.radiation_gain_mult,
            "disease_onset_mult" => scalars.disease_onset_mult,
            "hostile_encounter_mult" => scalars.hostile_encounter_mult,
            "enemy_damage_mult" => scalars.enemy_damage_mult,
            "market_price_mult" => scalars.market_price_mult,
            "equipment_decay_mult" => scalars.equipment_decay_mult,
            "crisis_deadline_mult" => scalars.crisis_deadline_mult,
            _ => 1f
        };
    }
}
