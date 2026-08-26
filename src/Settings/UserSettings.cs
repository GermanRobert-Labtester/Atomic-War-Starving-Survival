using System;
using System.IO;
using Godot;
using Ashfall.Core.Settings;

namespace AtomicWar.GodotApp.Settings
{
    /// <summary>
    /// Manages loading, saving, recovery, and immediate engine application of UserSettings.
    /// Hardened to gracefully recover from malformed JSON without throwing or blocking startup,
    /// while preserving diagnostic messages for troubleshooting.
    /// </summary>
    public static class UserSettingsStore
    {
        private const string SettingsPath = "user://settings.json";

        private static UserSettingsData? _current;
        private static string? _lastDiagnosticMessage;

        public static UserSettingsData Current => _current ??= Load();

        /// <summary>
        /// Diagnostic message from the most recent load/save/recovery operation, or null if clean.
        /// </summary>
        public static string? LastDiagnosticMessage => _lastDiagnosticMessage;

        /// <summary>
        /// True if the most recent load or save encountered an issue and had to recover.
        /// </summary>
        public static bool HasDiagnosticError => !string.IsNullOrEmpty(_lastDiagnosticMessage);

        public static void ClearDiagnosticMessage() => _lastDiagnosticMessage = null;

        public static UserSettingsData Load(string path = SettingsPath)
        {
            string globalPath = ProjectSettings.GlobalizePath(path);
            if (!File.Exists(globalPath))
            {
                _lastDiagnosticMessage = null;
                var defaults = new UserSettingsData();
                Save(defaults, path);
                return defaults;
            }

            try
            {
                string json = File.ReadAllText(globalPath);
                var (data, diagnosticMessage) = UserSettingsCodec.DeserializeWithRecovery(json);
                _lastDiagnosticMessage = diagnosticMessage;
                if (!string.IsNullOrEmpty(diagnosticMessage))
                {
                    GD.PrintErr($"[UserSettingsStore] {diagnosticMessage}");
                }
                return data;
            }
            catch (Exception ex)
            {
                _lastDiagnosticMessage = $"[UserSettingsStore] Failed to read settings from '{path}' ({ex.Message}). Recovered with safe defaults.";
                GD.PrintErr(_lastDiagnosticMessage);
                return new UserSettingsData();
            }
        }

        public static bool Save(UserSettingsData data, string path = SettingsPath)
        {
            if (data == null) return false;
            _current = data.Clone();

            string globalPath = ProjectSettings.GlobalizePath(path);
            string tempPath = globalPath + ".tmp";

            try
            {
                string? dir = Path.GetDirectoryName(globalPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string json = UserSettingsCodec.Serialize(data);
                File.WriteAllText(tempPath, json);

                if (File.Exists(globalPath))
                {
                    File.Replace(tempPath, globalPath, null);
                }
                else
                {
                    File.Move(tempPath, globalPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                _lastDiagnosticMessage = $"[UserSettingsStore] Failed to save settings to '{path}': {ex.Message}";
                GD.PrintErr(_lastDiagnosticMessage);
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch (Exception cleanupEx) { GD.PrintErr($"[UserSettings] Failed to clean temp file: {cleanupEx.Message}"); }
                return false;
            }
        }

        /// <summary>
        /// Applies settings immediately to Godot's audio buses, display server, and engine limits.
        /// Safely catches headless/unsupported display exceptions without throwing.
        /// </summary>
        public static void Apply(UserSettingsData data)
        {
            if (data == null) return;
            _current = data.Clone();

            // 1. Audio Application
            ApplyAudio("Master", data.MasterVolume, data.MuteAll);
            ApplyAudio("Music", data.MusicVolume, data.MuteAll);
            ApplyAudio("SFX", data.SfxVolume, data.MuteAll);
            ApplyAudio("Radio", data.RadioVolume, data.MuteAll);
            ApplyAudio("Ambience", data.AmbienceVolume, data.MuteAll);

            // 2. Engine FPS Cap
            Engine.MaxFps = Math.Max(0, data.MaxFps);

            // 3. Display Application (Safe for headless)
            try
            {
                if (DisplayServer.GetName() != "headless")
                {
                    // Window Mode
                    DisplayServer.WindowMode mode = data.WindowMode switch
                    {
                        1 => DisplayServer.WindowMode.ExclusiveFullscreen,
                        2 => DisplayServer.WindowMode.Fullscreen,
                        _ => DisplayServer.WindowMode.Windowed
                    };

                    if (DisplayServer.WindowGetMode() != mode)
                    {
                        DisplayServer.WindowSetMode(mode);
                    }

                    // Resolution (in windowed mode)
                    if (mode == DisplayServer.WindowMode.Windowed && data.ResolutionWidth > 0 && data.ResolutionHeight > 0)
                    {
                        DisplayServer.WindowSetSize(new Vector2I(data.ResolutionWidth, data.ResolutionHeight));
                    }

                    // VSync
                    DisplayServer.VSyncMode vsync = data.VSync
                        ? DisplayServer.VSyncMode.Enabled
                        : DisplayServer.VSyncMode.Disabled;
                    DisplayServer.WindowSetVsyncMode(vsync);
                }
            }
            catch (Exception ex)
            {
                GD.Print($"[UserSettingsStore] Display apply notice (headless/unsupported): {ex.Message}");
            }
        }

        private static void ApplyAudio(string busName, float linearVolume, bool muteAll)
        {
            int busIdx = AudioServer.GetBusIndex(busName);
            if (busIdx < 0) return;

            float clamped = Math.Clamp(linearVolume, 0f, 1f);
            float db = clamped <= 0.0001f ? -80f : Mathf.LinearToDb(clamped);
            AudioServer.SetBusVolumeDb(busIdx, db);
            AudioServer.SetBusMute(busIdx, muteAll || clamped <= 0.0001f);
        }
    }
}
