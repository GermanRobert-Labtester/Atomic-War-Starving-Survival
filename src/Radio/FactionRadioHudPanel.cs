using AtomicWar.GodotApp.UI;
using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using static AtomicWar.GodotApp.UI.AshfallUiHelpers;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.Radio
{
    /// <summary>
    /// Full Godot host implementation of the Faction Radio &amp; Intercept Log HUD (The Heterodyne Rack).
    /// Built to Concept 1 specification:
    /// - 19" cold-war stamped steel rack frame (radio_frame_9slice.png)
    /// - Frequency tuner (50.0..150.0 MHz) with illuminated dial (frequency_dial.png)
    /// - Analogue S-meter gauge (meter_signal_strength.png)
    /// - CRT scanline terminal (signal_static_overlay.png) with live transcript stream
    /// - 12-faction quick preset bank with real-time signal carrier lock
    /// - Historical wiretap transcript log
    /// 
    /// Driven deterministically via IFactionRadioProvider and ISeededRng.
    /// </summary>
    public partial class FactionRadioHudPanel : PanelContainer
    {
        private IFactionRadioProvider _radioProvider;
        private ISeededRng _rng;
        private int _currentDay = 1;
        private float _currentFrequency = 88.4f;
        private bool _squelchActive = true;

        // UI Controls - Left Tuning Column
        private Label _lblFrequencyDisplay;
        private HSlider _sliderFrequency;
        private TextureRect _textureSmeter;
        private Label _lblSignalStatus;
        private TextureRect _textureFactionBadge;
        private Label _lblFactionCallsign;
        private VBoxContainer _presetGrid;

        // UI Controls - Right CRT Log Column
        private Label _lblCrtLiveHeader;
        private Label _lblCrtLiveText;
        private TextureRect _crtOverlay;
        private VBoxContainer _logEntriesContainer;
        private ScrollContainer _logScroll;
        private readonly List<RadioIntercept> _history = new();

        // ── Probing / Verification Surface ───────────────────────────
        public float TunedFrequency => _currentFrequency;
        public int LogCount => _history.Count;
        public bool HasFrameTexture => GetThemeStylebox("panel") != null;
        public bool HasFrequencyDial => _sliderFrequency != null;
        public bool HasSMeter => _textureSmeter != null;
        public bool HasCrtOverlay => _crtOverlay != null;
        public bool HasLiveDisplay => _lblCrtLiveText != null && !string.IsNullOrEmpty(_lblCrtLiveText.Text);
        public bool HasFactionBadge => _textureFactionBadge?.Texture != null;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            CustomMinimumSize = new Vector2(720, 480);

            // Apply 9-slice steel radio frame
            var frameTex = LoadTexture("res://assets/ui/Textures/radio_frame_9slice.png");
            if (frameTex != null)
            {
                var sb = new StyleBoxTexture
                {
                    Texture = frameTex,
                    TextureMarginLeft = 16,
                    TextureMarginTop = 16,
                    TextureMarginRight = 16,
                    TextureMarginBottom = 16
                };
                AddThemeStyleboxOverride("panel", sb);
            }

            BuildLayout();
        }

        private void BuildLayout()
        {
            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);
            AddChild(rootVbox);

            // 1. Top Header Bar
            var topBar = new HBoxContainer();
            topBar.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);

            var lblTitle = new Label
            {
                Text = "SURVEILLANCE RECEIVER — TYPE-88 HETERODYNE",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            lblTitle.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH3);
            lblTitle.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
            topBar.AddChild(lblTitle);

            topBar.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

            var btnSquelch = new Button { Text = "[SQUELCH: ON]" };
            btnSquelch.Pressed += () =>
            {
                _squelchActive = !_squelchActive;
                btnSquelch.Text = _squelchActive ? "[SQUELCH: ON]" : "[SQUELCH: OFF]";
                TuneToFrequency(_currentFrequency);
            };
            topBar.AddChild(btnSquelch);

            rootVbox.AddChild(topBar);

            // 2. Main 2-Column Body
            var bodyHbox = new HBoxContainer();
            bodyHbox.SizeFlagsVertical = SizeFlags.ExpandFill;
            bodyHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);

            // ── Left Column: Tuner & S-Meter ──
            var leftCol = new VBoxContainer { CustomMinimumSize = new Vector2(280, 0) };
            leftCol.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);

            var lblTunerHeader = new Label { Text = "FREQUENCY CONTROL (VHF)" };
            lblTunerHeader.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            lblTunerHeader.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
            leftCol.AddChild(lblTunerHeader);

            _lblFrequencyDisplay = new Label
            {
                Text = "88.40 MHz",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            _lblFrequencyDisplay.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH2);
            _lblFrequencyDisplay.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Hot));
            leftCol.AddChild(_lblFrequencyDisplay);

            _sliderFrequency = new HSlider
            {
                MinValue = 50.0,
                MaxValue = 150.0,
                Step = 0.01,
                Value = 88.4,
                CustomMinimumSize = new Vector2(260, 24)
            };
            _sliderFrequency.ValueChanged += val => TuneToFrequency((float)val);
            leftCol.AddChild(_sliderFrequency);

            // S-Meter Row
            var smeterHbox = new HBoxContainer();
            smeterHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);

            _textureSmeter = new TextureRect
            {
                CustomMinimumSize = new Vector2(64, 32),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                Texture = LoadTexture("res://assets/ui/Icons/meter_signal_strength.png")
            };
            smeterHbox.AddChild(_textureSmeter);

            _lblSignalStatus = new Label { Text = "S8 · CARRIER LOCK" };
            _lblSignalStatus.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblSignalStatus.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
            smeterHbox.AddChild(_lblSignalStatus);
            leftCol.AddChild(smeterHbox);

            // Active Faction Identification Box
            var identBox = new HBoxContainer();
            identBox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);

            _textureFactionBadge = new TextureRect
            {
                CustomMinimumSize = new Vector2(36, 36),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
            };
            identBox.AddChild(_textureFactionBadge);

            _lblFactionCallsign = new Label
            {
                Text = "OVERLORD ACTUAL\nMilitary Remnants",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            _lblFactionCallsign.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblFactionCallsign.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
            identBox.AddChild(_lblFactionCallsign);
            leftCol.AddChild(identBox);

            // 12 Presets Label & Grid
            var lblPresets = new Label { Text = "PRESET CHANNELS" };
            lblPresets.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
            lblPresets.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
            leftCol.AddChild(lblPresets);

            var scrollPresets = new ScrollContainer { CustomMinimumSize = new Vector2(0, 160), SizeFlagsVertical = SizeFlags.ExpandFill };
            _presetGrid = new VBoxContainer();
            _presetGrid.AddThemeConstantOverride("separation", 2);
            scrollPresets.AddChild(_presetGrid);
            leftCol.AddChild(scrollPresets);

            bodyHbox.AddChild(leftCol);

            // ── Right Column: Live CRT & Log Buffer ──
            var rightCol = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            rightCol.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);

            // Live CRT Screen Box
            var crtPanel = new PanelContainer { CustomMinimumSize = new Vector2(0, 120) };
            var crtVbox = new VBoxContainer();
            crtVbox.AddThemeConstantOverride("separation", 4);

            _lblCrtLiveHeader = new Label { Text = "[ LIVE INTERCEPT — MONITORING ]" };
            _lblCrtLiveHeader.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblCrtLiveHeader.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Hot));
            crtVbox.AddChild(_lblCrtLiveHeader);

            _lblCrtLiveText = new Label
            {
                Text = "Listening on carrier wave...",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _lblCrtLiveText.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeBody);
            _lblCrtLiveText.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
            crtVbox.AddChild(_lblCrtLiveText);
            crtPanel.AddChild(crtVbox);

            // Static overlay on CRT
            _crtOverlay = new TextureRect
            {
                Texture = LoadTexture("res://assets/ui/Textures/signal_static_overlay.png"),
                StretchMode = TextureRect.StretchModeEnum.Tile,
                MouseFilter = MouseFilterEnum.Ignore
            };
            crtPanel.AddChild(_crtOverlay);
            rightCol.AddChild(crtPanel);

            // History Log Section
            var lblLogHeader = new Label { Text = "INTERCEPT TRANSCRIPT ARCHIVE" };
            lblLogHeader.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            lblLogHeader.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
            rightCol.AddChild(lblLogHeader);

            _logScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0, 180) };
            _logEntriesContainer = new VBoxContainer();
            _logEntriesContainer.AddThemeConstantOverride("separation", 4);
            _logScroll.AddChild(_logEntriesContainer);
            rightCol.AddChild(_logScroll);

            bodyHbox.AddChild(rightCol);
            rootVbox.AddChild(bodyHbox);
        }

        public void BindProvider(IFactionRadioProvider provider, ISeededRng rng, int day = 1)
        {
            _radioProvider = provider;
            _rng = rng ?? new SeededRng(2026);
            _currentDay = Math.Max(1, day);

            PopulatePresets();
            TuneToFrequency(_currentFrequency);
        }

        public void SetDay(int day)
        {
            _currentDay = Math.Max(1, day);
            TuneToFrequency(_currentFrequency);
        }

        public void TuneToFrequency(float freqMhz)
        {
            _currentFrequency = (float)Math.Round(freqMhz, 2);
            if (_sliderFrequency != null && Math.Abs(_sliderFrequency.Value - _currentFrequency) > 0.05)
            {
                _sliderFrequency.Value = _currentFrequency;
            }

            if (_lblFrequencyDisplay != null)
            {
                _lblFrequencyDisplay.Text = $"{_currentFrequency:00.00} MHz";
            }

            if (_radioProvider == null) return;

            var intercept = _radioProvider.GetBroadcastAtFrequency(_currentFrequency, _currentDay, _rng);
            _history.Add(intercept);

            // Update S-Meter & Lock
            string signalTag = $"S{intercept.SignalStrength}";
            if (intercept.SignalStrength >= 7) signalTag += " · CARRIER LOCK";
            else if (intercept.SignalStrength >= 3) signalTag += " · WEAK MODULATION";
            else signalTag += " · NOISE FLOOR";

            if (_lblSignalStatus != null)
            {
                _lblSignalStatus.Text = signalTag;
                _lblSignalStatus.AddThemeColorOverride(
                    "font_color",
                    intercept.SignalStrength >= 7 ? ToGodotColor(global::Ashfall.Core.UI.Theme.Hot) : ToGodotColor(global::Ashfall.Core.UI.Theme.Dim));
            }

            // Update Faction Badge & Callsign
            if (!string.IsNullOrEmpty(intercept.FactionId))
            {
                _textureFactionBadge.Texture = AtomicWar.GodotApp.FactionIconLoader.LoadFor(intercept.FactionId);
                _lblFactionCallsign.Text = $"{intercept.Callsign}\n{intercept.FactionId.ToUpper().Replace('_', ' ')}";
                _lblCrtLiveHeader.Text = $"[ LIVE INTERCEPT — {intercept.Callsign} ({_currentFrequency:00.00} MHz) ]";
            }
            else
            {
                _textureFactionBadge.Texture = null;
                _lblFactionCallsign.Text = "DEAD AIR / STATIC\nUnallocated Channel";
                _lblCrtLiveHeader.Text = $"[ LIVE INTERCEPT — DEAD AIR ({_currentFrequency:00.00} MHz) ]";
            }

            // Update Live Text
            if (_lblCrtLiveText != null)
            {
                _lblCrtLiveText.Text = intercept.Message;
            }

            // Append to Archive Log
            AppendLogEntry(intercept);
        }

        private void AppendLogEntry(RadioIntercept intercept)
        {
            if (_logEntriesContainer == null) return;

            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);

            var lblMeta = new Label
            {
                Text = $"[D{intercept.Day} {intercept.FrequencyMhz:00.0}M S{intercept.SignalStrength}]",
                CustomMinimumSize = new Vector2(110, 0)
            };
            lblMeta.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
            lblMeta.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
            row.AddChild(lblMeta);

            var lblMsg = new Label
            {
                Text = string.IsNullOrEmpty(intercept.FactionId) ? intercept.Message : $"<{intercept.Callsign}> {intercept.Message}",
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            lblMsg.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            lblMsg.AddThemeColorOverride(
                "font_color",
                intercept.Kind == RadioEventKind.Silence ? ToGodotColor(global::Ashfall.Core.UI.Theme.Dim) : ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
            row.AddChild(lblMsg);

            _logEntriesContainer.AddChild(row);
        }

        private void PopulatePresets()
        {
            if (_presetGrid == null || _radioProvider == null) return;

            AshfallUiHelpers.EmptyChildren(_presetGrid);foreach (var f in _radioProvider.GetAllFactions())
            {
                float freq = _radioProvider.GetFactionFrequency(f);
                string callsign = _radioProvider.GetFactionCallsign(f);

                var btn = new Button
                {
                    Text = $"{freq:00.0}M · {f.ToUpper().Replace('_', ' ')}",
                    CustomMinimumSize = new Vector2(250, 22),
                    Alignment = HorizontalAlignment.Left
                };
                btn.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
                float fCopy = freq;
                btn.Pressed += () => TuneToFrequency(fCopy);

                _presetGrid.AddChild(btn);
            }
        }

        private static Color ToGodotColor((float r, float g, float b, float a) token)
        {
            return ToColor(token);
        }

        private static Texture2D? LoadTexture(string path)
        {
            return TryLoadTexture(path);
        }
    }
}
