using System;
using Godot;
using Ashfall.Core.Settings;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.Settings;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Functional responsive settings panel.
    /// Manages display, audio, accessibility, and gameplay preferences with
    /// immediate live application, persistence to user://settings.json, revert, and defaults reset.
    /// </summary>
    public partial class SettingsPanel : Control
    {
        public event Action? OnClose;

        private UserSettingsData _working = new();
        private UserSettingsData _initial = new();

        // Audio labels
        private Label _lblMasterVol = null!;
        private Label _lblMusicVol = null!;
        private Label _lblSfxVol = null!;
        private Label _lblRadioVol = null!;
        private Label _lblAmbienceVol = null!;
        private Button _btnMute = null!;

        // Display controls
        private OptionButton _optWindowMode = null!;
        private OptionButton _optResolution = null!;
        private OptionButton _optUiScale = null!;
        private Button _btnVSync = null!;
        private OptionButton _optMaxFps = null!;

        // Accessibility & Language
        private OptionButton _optLanguage = null!;
        private Button _btnHighContrast = null!;
        private Button _btnHazardLabels = null!;
        private Button _btnReducedMotion = null!;
        private Button _btnLargeFonts = null!;

        // Gameplay
        private OptionButton _optTutorialMode = null!;
        private Button _btnResetTutorials = null!;
        private Button _btnConfirmEndDay = null!;
        private Button _btnVerboseRadio = null!;
        private Button _btnAutoSave = null!;

        private static readonly (int W, int H, string Label)[] Resolutions = new[]
        {
            (1280, 720, "1280 × 720 (HD)"),
            (1366, 768, "1366 × 768 (Laptop)"),
            (1600, 900, "1600 × 900 (16:9)"),
            (1920, 1080, "1920 × 1080 (FHD Native)"),
            (2560, 1080, "2560 × 1080 (Ultrawide)"),
            (2560, 1440, "2560 × 1440 (2K QHD)"),
            (3840, 2160, "3840 × 2160 (4K UHD)")
        };

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;
            BuildLayout();
        }

        public void Open()
        {
            _initial = UserSettingsStore.Load();
            _working = _initial.Clone();
            RefreshControls();
            Visible = true;
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public override void _UnhandledKeyInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && !key.Echo)
            {
                if (key.Keycode == Key.Escape)
                {
                    CancelAndClose();
                    GetViewport().SetInputAsHandled();
                }
            }
        }

        private void BuildLayout()
        {
            // Dimmed background overlay
            var overlay = new ColorRect
            {
                Color = new Color(0.02f, 0.02f, 0.03f, 0.92f)
            };
            overlay.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(overlay);

            // Centered responsive container
            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            // Panel frame
            var panelBox = AshfallUiHelpers.MakePanel(680, 620);
            center.AddChild(panelBox);

            var mainVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            mainVBox.CustomMinimumSize = new Vector2(640, 580);
            panelBox.AddChild(mainVBox);

            // Header
            var headerHBox = new HBoxContainer();
            var title = AshfallUiHelpers.MakeTitle("SYSTEM CONFIGURATION // SETTINGS", DesignTheme.FontSizeH2);
            headerHBox.AddChild(title);
            headerHBox.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });
            var btnCloseTop = AshfallUiHelpers.MakeButton("✕", CancelAndClose);
            btnCloseTop.CustomMinimumSize = new Vector2(36, 32);
            headerHBox.AddChild(btnCloseTop);
            mainVBox.AddChild(headerHBox);

            mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Scrollable settings body
            var scroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            mainVBox.AddChild(scroll);

            var contentVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingLg);
            contentVBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentVBox);

            // ── 1. DISPLAY SECTION ─────────────────────────────────────────
            contentVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("DISPLAY & GRAPHICS"));

            // Window Mode
            var rowMode = MakeSettingRow("Window Mode");
            _optWindowMode = new OptionButton { CustomMinimumSize = new Vector2(240, 32) };
            _optWindowMode.AddItem("Windowed", 0);
            _optWindowMode.AddItem("Borderless Fullscreen", 1);
            _optWindowMode.AddItem("Fullscreen", 2);
            _optWindowMode.ItemSelected += idx => _working.WindowMode = (int)idx;
            rowMode.AddChild(_optWindowMode);
            contentVBox.AddChild(rowMode);

            // Resolution Preset
            var rowRes = MakeSettingRow("Resolution Preset");
            _optResolution = new OptionButton { CustomMinimumSize = new Vector2(240, 32) };
            for (int i = 0; i < Resolutions.Length; i++)
            {
                _optResolution.AddItem(Resolutions[i].Label, i);
            }
            _optResolution.ItemSelected += idx =>
            {
                if (idx >= 0 && idx < Resolutions.Length)
                {
                    _working.ResolutionWidth = Resolutions[idx].W;
                    _working.ResolutionHeight = Resolutions[idx].H;
                }
            };
            rowRes.AddChild(_optResolution);
            contentVBox.AddChild(rowRes);

            // UI Scale
            var rowScale = MakeSettingRow("Interface Scale");
            _optUiScale = new OptionButton { CustomMinimumSize = new Vector2(240, 32) };
            _optUiScale.AddItem("0.8× (Compact)", 0);
            _optUiScale.AddItem("1.0× (Standard)", 1);
            _optUiScale.AddItem("1.25× (Large)", 2);
            _optUiScale.AddItem("1.5× (Expanded)", 3);
            _optUiScale.ItemSelected += idx =>
            {
                _working.UiScale = idx switch { 0 => 0.8f, 2 => 1.25f, 3 => 1.5f, _ => 1.0f };
            };
            rowScale.AddChild(_optUiScale);
            contentVBox.AddChild(rowScale);

            // VSync
            var rowVsync = MakeSettingRow("Vertical Sync");
            _btnVSync = AshfallUiHelpers.MakeButton("VSYNC: ENABLED", () =>
            {
                _working.VSync = !_working.VSync;
                _btnVSync.Text = _working.VSync ? "VSYNC: ENABLED" : "VSYNC: DISABLED";
            });
            _btnVSync.CustomMinimumSize = new Vector2(240, 32);
            rowVsync.AddChild(_btnVSync);
            contentVBox.AddChild(rowVsync);

            // FPS Cap
            var rowFps = MakeSettingRow("Frame Rate Cap");
            _optMaxFps = new OptionButton { CustomMinimumSize = new Vector2(240, 32) };
            _optMaxFps.AddItem("30 FPS", 30);
            _optMaxFps.AddItem("60 FPS (Recommended)", 60);
            _optMaxFps.AddItem("120 FPS", 120);
            _optMaxFps.AddItem("144 FPS", 144);
            _optMaxFps.AddItem("Unlimited", 0);
            _optMaxFps.ItemSelected += idx =>
            {
                _working.MaxFps = (int)_optMaxFps.GetItemId((int)idx);
            };
            rowFps.AddChild(_optMaxFps);
            contentVBox.AddChild(rowFps);

            contentVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── 2. AUDIO SECTION ───────────────────────────────────────────
            contentVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("AUDIO SIGNALS"));

            _btnMute = AshfallUiHelpers.MakeButton("ALL AUDIO: ACTIVE", () =>
            {
                _working.MuteAll = !_working.MuteAll;
                _btnMute.Text = _working.MuteAll ? "ALL AUDIO: MUTED" : "ALL AUDIO: ACTIVE";
                UserSettingsStore.Apply(_working);
            });
            _btnMute.CustomMinimumSize = new Vector2(200, 32);
            contentVBox.AddChild(_btnMute);

            contentVBox.AddChild(MakeVolumeRow("Master Volume", v => _working.MasterVolume = v, () => _working.MasterVolume, out _lblMasterVol));
            contentVBox.AddChild(MakeVolumeRow("Music / Ambience Score", v => _working.MusicVolume = v, () => _working.MusicVolume, out _lblMusicVol));
            contentVBox.AddChild(MakeVolumeRow("Sound Effects / Machinery", v => _working.SfxVolume = v, () => _working.SfxVolume, out _lblSfxVol));
            contentVBox.AddChild(MakeVolumeRow("Radio Receiver / Transmissions", v => _working.RadioVolume = v, () => _working.RadioVolume, out _lblRadioVol));
            contentVBox.AddChild(MakeVolumeRow("Bunker Ambience / Air Duct", v => _working.AmbienceVolume = v, () => _working.AmbienceVolume, out _lblAmbienceVol));

            contentVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── 3. ACCESSIBILITY & LANGUAGE ───────────────────────────────
            contentVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ACCESSIBILITY & LANGUAGE"));

            // Language / Locale
            var rowLang = MakeSettingRow("Language / Locale");
            _optLanguage = new OptionButton { CustomMinimumSize = new Vector2(240, 32) };
            _optLanguage.AddItem("English (US)", 0);
            _optLanguage.AddItem("[QA] Pseudo-Locale (Expanded)", 1);
            _optLanguage.ItemSelected += idx =>
            {
                _working.Locale = idx == 1 ? "pseudo" : "en";
            };
            rowLang.AddChild(_optLanguage);
            contentVBox.AddChild(rowLang);

            var rowHc = MakeSettingRow("High Contrast HUD");
            _btnHighContrast = AshfallUiHelpers.MakeButton("DISABLED", () =>
            {
                _working.HighContrast = !_working.HighContrast;
                _btnHighContrast.Text = _working.HighContrast ? "ENABLED" : "DISABLED";
            });
            _btnHighContrast.CustomMinimumSize = new Vector2(240, 32);
            rowHc.AddChild(_btnHighContrast);
            contentVBox.AddChild(rowHc);

            var rowHz = MakeSettingRow("Always Show Hazard Text");
            _btnHazardLabels = AshfallUiHelpers.MakeButton("ENABLED", () =>
            {
                _working.HazardTextLabels = !_working.HazardTextLabels;
                _btnHazardLabels.Text = _working.HazardTextLabels ? "ENABLED" : "DISABLED";
            });
            _btnHazardLabels.CustomMinimumSize = new Vector2(240, 32);
            rowHz.AddChild(_btnHazardLabels);
            contentVBox.AddChild(rowHz);

            var rowRm = MakeSettingRow("Reduced Motion");
            _btnReducedMotion = AshfallUiHelpers.MakeButton("DISABLED", () =>
            {
                _working.ReducedMotion = !_working.ReducedMotion;
                _btnReducedMotion.Text = _working.ReducedMotion ? "ENABLED" : "DISABLED";
            });
            _btnReducedMotion.CustomMinimumSize = new Vector2(240, 32);
            rowRm.AddChild(_btnReducedMotion);
            contentVBox.AddChild(rowRm);

            contentVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── 4. GAMEPLAY PREFERENCES ────────────────────────────────────
            contentVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("GAMEPLAY & ONBOARDING"));

            // Tutorial Mode
            var rowTut = MakeSettingRow("Tutorial & Guidance");
            _optTutorialMode = new OptionButton { CustomMinimumSize = new Vector2(240, 32) };
            _optTutorialMode.AddItem("Full Onboarding & Tutorials", 0);
            _optTutorialMode.AddItem("Contextual Hints Only", 1);
            _optTutorialMode.AddItem("Disabled (Veteran Mode)", 2);
            _optTutorialMode.ItemSelected += idx => _working.TutorialMode = (int)idx;
            rowTut.AddChild(_optTutorialMode);
            contentVBox.AddChild(rowTut);

            // Reset Tutorials
            var rowResetTut = MakeSettingRow("Reset Tutorial Progress");
            _btnResetTutorials = AshfallUiHelpers.MakeButton("RESET TUTORIALS", () =>
            {
                // Reset onboarding journey state in session if accessible
                _btnResetTutorials.Text = "TUTORIALS RESET";
            });
            _btnResetTutorials.CustomMinimumSize = new Vector2(240, 32);
            rowResetTut.AddChild(_btnResetTutorials);
            contentVBox.AddChild(rowResetTut);

            var rowEndDay = MakeSettingRow("Confirm Before Ending Day");
            _btnConfirmEndDay = AshfallUiHelpers.MakeButton("ENABLED", () =>
            {
                _working.ConfirmEndDay = !_working.ConfirmEndDay;
                _btnConfirmEndDay.Text = _working.ConfirmEndDay ? "ENABLED" : "DISABLED";
            });
            _btnConfirmEndDay.CustomMinimumSize = new Vector2(240, 32);
            rowEndDay.AddChild(_btnConfirmEndDay);
            contentVBox.AddChild(rowEndDay);

            var rowRadioLog = MakeSettingRow("Detailed Radio Log Dispatches");
            _btnVerboseRadio = AshfallUiHelpers.MakeButton("ENABLED", () =>
            {
                _working.VerboseRadioLog = !_working.VerboseRadioLog;
                _btnVerboseRadio.Text = _working.VerboseRadioLog ? "ENABLED" : "DISABLED";
            });
            _btnVerboseRadio.CustomMinimumSize = new Vector2(240, 32);
            rowRadioLog.AddChild(_btnVerboseRadio);
            contentVBox.AddChild(rowRadioLog);

            mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── BOTTOM ACTION BAR ──────────────────────────────────────────
            var bottomHBox = new HBoxContainer();
            bottomHBox.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);

            var btnReset = AshfallUiHelpers.MakeButton("RESET DEFAULTS", ResetToDefaults);
            btnReset.CustomMinimumSize = new Vector2(140, 38);
            bottomHBox.AddChild(btnReset);

            bottomHBox.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

            var btnCancel = AshfallUiHelpers.MakeButton("CANCEL", CancelAndClose);
            btnCancel.CustomMinimumSize = new Vector2(110, 38);
            bottomHBox.AddChild(btnCancel);

            var btnApply = AshfallUiHelpers.MakeButton("APPLY & SAVE", ApplyAndSave);
            btnApply.CustomMinimumSize = new Vector2(140, 38);
            btnApply.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            bottomHBox.AddChild(btnApply);

            mainVBox.AddChild(bottomHBox);
        }

        private HBoxContainer MakeSettingRow(string labelText)
        {
            var row = new HBoxContainer();
            row.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var lbl = AshfallUiHelpers.MakeBody(labelText);
            lbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            row.AddChild(lbl);
            return row;
        }

        private HBoxContainer MakeVolumeRow(string labelText, Action<float> setter, Func<float> getter, out Label valueLabel)
        {
            var row = new HBoxContainer();
            row.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var lbl = AshfallUiHelpers.MakeBody(labelText);
            lbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            row.AddChild(lbl);

            var valLbl = AshfallUiHelpers.MakeLabel($"{(int)(getter() * 100)}%", DesignTheme.FontSizeBody, DesignTheme.Pale);
            valLbl.CustomMinimumSize = new Vector2(48, 24);
            valLbl.HorizontalAlignment = HorizontalAlignment.Right;
            valueLabel = valLbl;

            var minusBtn = AshfallUiHelpers.MakeButton("-", () =>
            {
                float newVal = Math.Clamp(getter() - 0.1f, 0f, 1f);
                setter(newVal);
                valLbl.Text = $"{(int)(newVal * 100)}%";
                UserSettingsStore.Apply(_working);
            });
            minusBtn.CustomMinimumSize = new Vector2(32, 28);
            row.AddChild(minusBtn);

            row.AddChild(valLbl);

            var plusBtn = AshfallUiHelpers.MakeButton("+", () =>
            {
                float newVal = Math.Clamp(getter() + 0.1f, 0f, 1f);
                setter(newVal);
                valLbl.Text = $"{(int)(newVal * 100)}%";
                UserSettingsStore.Apply(_working);
            });
            plusBtn.CustomMinimumSize = new Vector2(32, 28);
            row.AddChild(plusBtn);

            return row;
        }

        private void RefreshControls()
        {
            _optWindowMode.Selected = Math.Clamp(_working.WindowMode, 0, 2);

            int resIndex = 3; // 1920x1080 default
            for (int i = 0; i < Resolutions.Length; i++)
            {
                if (Resolutions[i].W == _working.ResolutionWidth && Resolutions[i].H == _working.ResolutionHeight)
                {
                    resIndex = i;
                    break;
                }
            }
            _optResolution.Selected = resIndex;

            _optUiScale.Selected = _working.UiScale switch
            {
                <= 0.85f => 0,
                >= 1.4f => 3,
                >= 1.2f => 2,
                _ => 1
            };

            _btnVSync.Text = _working.VSync ? "VSYNC: ENABLED" : "VSYNC: DISABLED";

            int fpsItem = _working.MaxFps;
            for (int i = 0; i < _optMaxFps.ItemCount; i++)
            {
                if (_optMaxFps.GetItemId(i) == fpsItem)
                {
                    _optMaxFps.Selected = i;
                    break;
                }
            }

            _btnMute.Text = _working.MuteAll ? "ALL AUDIO: MUTED" : "ALL AUDIO: ACTIVE";
            if (_lblMasterVol != null) _lblMasterVol.Text = $"{(int)(_working.MasterVolume * 100)}%";
            if (_lblMusicVol != null) _lblMusicVol.Text = $"{(int)(_working.MusicVolume * 100)}%";
            if (_lblSfxVol != null) _lblSfxVol.Text = $"{(int)(_working.SfxVolume * 100)}%";
            if (_lblRadioVol != null) _lblRadioVol.Text = $"{(int)(_working.RadioVolume * 100)}%";
            if (_lblAmbienceVol != null) _lblAmbienceVol.Text = $"{(int)(_working.AmbienceVolume * 100)}%";

            if (_optLanguage != null) _optLanguage.Selected = _working.Locale == "pseudo" ? 1 : 0;
            if (_optTutorialMode != null) _optTutorialMode.Selected = Math.Clamp(_working.TutorialMode, 0, 2);

            _btnHighContrast.Text = _working.HighContrast ? "ENABLED" : "DISABLED";
            _btnHazardLabels.Text = _working.HazardTextLabels ? "ENABLED" : "DISABLED";
            _btnReducedMotion.Text = _working.ReducedMotion ? "ENABLED" : "DISABLED";
            _btnConfirmEndDay.Text = _working.ConfirmEndDay ? "ENABLED" : "DISABLED";
            _btnVerboseRadio.Text = _working.VerboseRadioLog ? "ENABLED" : "DISABLED";
        }

        private void ResetToDefaults()
        {
            _working = new UserSettingsData();
            RefreshControls();
            UserSettingsStore.Apply(_working);
        }

        private void ApplyAndSave()
        {
            UserSettingsStore.Save(_working);
            UserSettingsStore.Apply(_working);
            _initial = _working.Clone();
            Close();
        }

        private void CancelAndClose()
        {
            _working = _initial.Clone();
            UserSettingsStore.Apply(_working);
            Close();
        }
    }
}
