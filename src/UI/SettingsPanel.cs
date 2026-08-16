using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Settings panel.
    /// Toggleable overlay with audio, graphics, and gameplay options.
    /// Placeholder implementation for UI architecture demonstration.
    /// </summary>
    public partial class SettingsPanel : Control
    {
        public event Action? OnClose;
        public event Action<string, bool>? OnSettingChanged;

        private Button _btnMusic;
        private Button _btnSfx;
        private Button _btnVibration;
        private Label _lblMusicVolume;
        private Label _lblSfxVolume;
        private float _musicVolume = 100f;
        private float _sfxVolume = 100f;

        private bool _musicEnabled = false;
        private bool _sfxEnabled = false;
        private bool _vibrationEnabled = true;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            // Background overlay
            var bg = new ColorRect
            {
                Color = new Color(0.05f, 0.05f, 0.05f, 0.9f)
            };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            // Content container
            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(400, 0);
            container.AddChild(vbox);

            // Title
            var title = AshfallUiHelpers.MakeTitle("SETTINGS", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Audio section
            var audioGroup = new VBoxContainer();
            audioGroup.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);

            var audioTitle = AshfallUiHelpers.MakeSectionHeader("AUDIO");
            audioGroup.AddChild(audioTitle);

            // Music toggle
            _btnMusic = AshfallUiHelpers.MakeButton("MUSIC: OFF", ToggleMusic);
            audioGroup.AddChild(_btnMusic);

            // Music volume
            var musicRow = new HBoxContainer();
            musicRow.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _lblMusicVolume = AshfallUiHelpers.MakeSmall($"Volume: {_musicVolume:F0}%");
            _lblMusicVolume.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            musicRow.AddChild(_lblMusicVolume);

            var musicSlider = new HBoxContainer();
            musicSlider.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            
            var minusBtn = AshfallUiHelpers.MakeButton("-", () => { _musicVolume = Mathf.Clamp(_musicVolume - 10f, 0f, 100f); UpdateMusicVolume(); });
            minusBtn.CustomMinimumSize = new Vector2(30, 20);
            musicSlider.AddChild(minusBtn);

            _lblMusicVolume = new Label { Text = $"{_musicVolume:F0}%" };
            _lblMusicVolume.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _lblMusicVolume.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            musicSlider.AddChild(_lblMusicVolume);

            var plusBtn = AshfallUiHelpers.MakeButton("+", () => { _musicVolume = Mathf.Clamp(_musicVolume + 10f, 0f, 100f); UpdateMusicVolume(); });
            plusBtn.CustomMinimumSize = new Vector2(30, 20);
            musicSlider.AddChild(plusBtn);

            musicRow.AddChild(musicSlider);
            audioGroup.AddChild(musicRow);

            // SFX toggle
            _btnSfx = AshfallUiHelpers.MakeButton("SOUND FX: OFF", ToggleSfx);
            audioGroup.AddChild(_btnSfx);

            // SFX volume
            var sfxRow = new HBoxContainer();
            sfxRow.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _lblSfxVolume = AshfallUiHelpers.MakeSmall($"Volume: {_sfxVolume:F0}%");
            _lblSfxVolume.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            sfxRow.AddChild(_lblSfxVolume);

            var sfxSlider = new HBoxContainer();
            sfxSlider.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            
            var sfxMinusBtn = AshfallUiHelpers.MakeButton("-", () => { _sfxVolume = Mathf.Clamp(_sfxVolume - 10f, 0f, 100f); UpdateSfxVolume(); });
            sfxMinusBtn.CustomMinimumSize = new Vector2(30, 20);
            sfxSlider.AddChild(sfxMinusBtn);

            _lblSfxVolume = new Label { Text = $"{_sfxVolume:F0}%" };
            _lblSfxVolume.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _lblSfxVolume.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            sfxSlider.AddChild(_lblSfxVolume);

            var sfxPlusBtn = AshfallUiHelpers.MakeButton("+", () => { _sfxVolume = Mathf.Clamp(_sfxVolume + 10f, 0f, 100f); UpdateSfxVolume(); });
            sfxPlusBtn.CustomMinimumSize = new Vector2(30, 20);
            sfxSlider.AddChild(sfxPlusBtn);

            sfxRow.AddChild(sfxSlider);
            audioGroup.AddChild(sfxRow);

            vbox.AddChild(audioGroup);
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Gameplay section
            var gameplayGroup = new VBoxContainer();
            gameplayGroup.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);

            var gameplayTitle = AshfallUiHelpers.MakeSectionHeader("GAMEPLAY");
            gameplayGroup.AddChild(gameplayTitle);

            // Vibration toggle
            _btnVibration = AshfallUiHelpers.MakeButton("VIBRATION: ON", ToggleVibration);
            gameplayGroup.AddChild(_btnVibration);

            vbox.AddChild(gameplayGroup);
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Close button
            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            // Keyboard shortcut
            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
        }

        private void ToggleMusic()
        {
            _musicEnabled = !_musicEnabled;
            _btnMusic.Text = _musicEnabled ? "MUSIC: ON" : "MUSIC: OFF";
            OnSettingChanged?.Invoke("music", _musicEnabled);
        }

        private void ToggleSfx()
        {
            _sfxEnabled = !_sfxEnabled;
            _btnSfx.Text = _sfxEnabled ? "SOUND FX: ON" : "SOUND FX: OFF";
            OnSettingChanged?.Invoke("sfx", _sfxEnabled);
        }

        private void ToggleVibration()
        {
            _vibrationEnabled = !_vibrationEnabled;
            _btnVibration.Text = _vibrationEnabled ? "VIBRATION: ON" : "VIBRATION: OFF";
            OnSettingChanged?.Invoke("vibration", _vibrationEnabled);
        }

        private void UpdateMusicVolume()
        {
            _lblMusicVolume.Text = $"{_musicVolume:F0}%";
            OnSettingChanged?.Invoke("music_volume", _musicVolume > 0);
        }

        private void UpdateSfxVolume()
        {
            _lblSfxVolume.Text = $"{_sfxVolume:F0}%";
            OnSettingChanged?.Invoke("sfx_volume", _sfxVolume > 0);
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
