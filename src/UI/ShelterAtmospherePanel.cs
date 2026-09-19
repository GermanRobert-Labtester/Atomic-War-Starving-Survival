// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Text;
using Godot;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 220 & 205 — Shelter Atmosphere, Ambiance & Acoustic Discipline Panel.
    /// Presents the holistic shelter environmental profile: composite mood score,
    /// active ambiance profile, 7 environmental facet levels, holistic modifiers,
    /// noise sources, and quiet hours controls.
    /// </summary>
    public partial class ShelterAtmospherePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _breakdownText = null!;
        private Label _modifiersText = null!;
        private Label _noiseSourcesText = null!;
        private Button _toggleQuietHoursBtn = null!;
        private Button _soundproofWorkshopBtn = null!;

        private ShelterAtmosphereHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(ShelterAtmosphereHostSession session)
        {
            _host = session;
            if (_host != null) _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var atmo = _host.Atmosphere;
            var noise = _host.Noise;
            var mods = _host.Modifiers;

            var moodCriticality = atmo.CurrentMoodCategory switch
            {
                AtmosphereMoodCategory.Bleak => AshfallMetricCard.Criticality.Critical,
                AtmosphereMoodCategory.Tense => AshfallMetricCard.Criticality.Warn,
                AtmosphereMoodCategory.Neutral => AshfallMetricCard.Criticality.Normal,
                AtmosphereMoodCategory.Comfortable => AshfallMetricCard.Criticality.Normal,
                AtmosphereMoodCategory.Welcoming => AshfallMetricCard.Criticality.Normal,
                AtmosphereMoodCategory.Vibrant => AshfallMetricCard.Criticality.Normal,
                _ => AshfallMetricCard.Criticality.Normal
            };

            var noiseCriticality = noise.OverallNoiseLevel switch
            {
                > 65f => AshfallMetricCard.Criticality.Critical,
                > 45f => AshfallMetricCard.Criticality.Warn,
                _ => AshfallMetricCard.Criticality.Normal
            };

            var riskCriticality = noise.DetectionRisk switch
            {
                > 50f => AshfallMetricCard.Criticality.Critical,
                > 25f => AshfallMetricCard.Criticality.Warn,
                _ => AshfallMetricCard.Criticality.Normal
            };

            _statusRail.Set("mood", $"{atmo.OverallMoodScore:F1} · {atmo.CurrentMoodCategory}", moodCriticality);
            _statusRail.Set("profile", $"{atmo.ActiveProfile}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("noise", $"{noise.OverallNoiseLevel:F1} dB", noiseCriticality);
            _statusRail.Set("risk", $"{noise.DetectionRisk:F1}%", riskCriticality);

            // 1. Environmental Breakdown
            var sb = new StringBuilder();
            sb.AppendLine($"Overall Mood Score: {atmo.OverallMoodScore:F1}/100 ({atmo.CurrentMoodCategory.ToString().ToUpperInvariant()})");
            sb.AppendLine($"  Lighting Quality:   {atmo.LightingQuality:F1}/100");
            sb.AppendLine($"  Acoustic Comfort:   {atmo.AcousticComfort:F1}/100 (Shelter Noise: {noise.OverallNoiseLevel:F1} dB)");
            sb.AppendLine($"  Air Purity:         {atmo.AirPurity:F1}/100");
            sb.AppendLine($"  Thermal Comfort:    {atmo.ThermalComfort:F1}/100");
            sb.AppendLine($"  Cleanliness:        {atmo.Cleanliness:F1}/100");
            sb.AppendLine($"  Social Warmth:      {atmo.SocialWarmth:F1}/100");
            sb.AppendLine($"  Decoration Level:   {atmo.DecorationLevel:F1}/100");
            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                sb.AppendLine();
                sb.AppendLine($"Last Event: {_host.LastEvent}");
            }
            _breakdownText.Text = sb.ToString().TrimEnd();

            // 2. Modifiers
            var modSb = new StringBuilder();
            modSb.AppendLine($"Ambiance Profile: {atmo.ActiveProfile.ToString().ToUpperInvariant()}");
            modSb.AppendLine($"  Morale Delta:       {(mods.MoraleModifier >= 0 ? "+" : "")}{mods.MoraleModifier:F1} per day");
            modSb.AppendLine($"  Productivity Delta: {(mods.ProductivityModifier >= 0 ? "+" : "")}{mods.ProductivityModifier * 100:F0}%");
            modSb.AppendLine($"  Stress Relief:      {(mods.StressReliefModifier >= 0 ? "+" : "")}{mods.StressReliefModifier * 100:F0}%");
            _modifiersText.Text = modSb.ToString().TrimEnd();

            // 3. Noise Sources
            var noiseSb = new StringBuilder();
            noiseSb.AppendLine($"Quiet Hours: {(noise.QuietHoursActive ? $"ACTIVE ({noise.QuietHoursStart:D2}:00 - {noise.QuietHoursEnd:D2}:00)" : "DISABLED")}");
            noiseSb.AppendLine($"External Threat Detection Risk: {noise.DetectionRisk:F1}%");
            noiseSb.AppendLine("Active Acoustic Sources:");
            if (noise.Sources.Count == 0)
            {
                noiseSb.AppendLine("  (No active noise sources registered)");
            }
            else
            {
                foreach (var src in noise.Sources)
                {
                    noiseSb.AppendLine($"  • [{src.Type}] in {src.RoomId}: {src.NoiseOutput:F0} dB (Active: {src.IsActive})");
                }
            }
            _noiseSourcesText.Text = noiseSb.ToString().TrimEnd();

            _toggleQuietHoursBtn.Text = noise.QuietHoursActive ? "Lift Quiet Hours" : "Enforce Quiet Hours (22:00 - 06:00)";
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Shelter Atmosphere & Ambiance // Environmental Character", minWidth: 950, minHeight: 600);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("mood", "Composite Mood", "—", AshfallMetricCard.Criticality.Normal, minWidth: 190);
            _statusRail.AddCard("profile", "Ambiance Profile", "—", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("noise", "Shelter Noise", "—", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("risk", "Detection Risk", "—", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 14);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Facet Breakdown section
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ENVIRONMENTAL FACET RATINGS"));
            _breakdownText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _breakdownText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            _contentStack.AddChild(_breakdownText);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());

            // Modifiers section
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("HOLISTIC SHELTER MODIFIERS"));
            _modifiersText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _modifiersText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            _contentStack.AddChild(_modifiersText);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());

            // Noise discipline section
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ACOUSTIC DISCIPLINE & DETECTION RISK"));
            _noiseSourcesText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _noiseSourcesText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            _contentStack.AddChild(_noiseSourcesText);

            var actionRow = new HBoxContainer();
            actionRow.AddThemeConstantOverride("separation", 10);

            _toggleQuietHoursBtn = new Button { Text = "Toggle Quiet Hours" };
            _toggleQuietHoursBtn.Pressed += OnToggleQuietHoursPressed;
            actionRow.AddChild(_toggleQuietHoursBtn);

            _soundproofWorkshopBtn = new Button { Text = "Insulate Workshop (+25% Wall)" };
            _soundproofWorkshopBtn.Pressed += OnSoundproofWorkshopPressed;
            actionRow.AddChild(_soundproofWorkshopBtn);

            _contentStack.AddChild(actionRow);

            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });
        }

        private void OnToggleQuietHoursPressed()
        {
            if (_host == null) return;
            bool active = !_host.Noise.QuietHoursActive;
            _host.SetQuietHours(active, 22, 6);
        }

        private void OnSoundproofWorkshopPressed()
        {
            if (_host == null) return;
            _host.SoundproofRoom("workshop", 25f, 15f);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
            {
                Visible = false;
                OnClose?.Invoke();
                GetViewport()?.SetInputAsHandled();
            }
        }
    }
}
