using System;
using System.IO;
using System.Text.Json;
using Godot;
using Ashfall.Core.Journal;

namespace AtomicWar.Journal
{
    /// <summary>
    /// Persists JournalSave as JSON under user://journal_save.json. Plain
    /// System.Text.Json with fields included, so the save shape stays portable
    /// between the Godot and Unity implementations.
    /// </summary>
    public static class JournalSaveStore
    {
        public const string FileName = "journal_save.json";

        private static readonly JsonSerializerOptions s_options = new JsonSerializerOptions
        {
            IncludeFields = true,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => File.Exists(SavePath);

        public static void Save(JournalSave save, string? pathOverride = null)
        {
            if (save == null) return;
            try
            {
                string path = pathOverride ?? SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string json = JsonSerializer.Serialize(save, s_options);
                File.WriteAllText(path, json);
            }
            catch (Exception e)
            {
                GD.PrintErr($"[JournalSaveStore] save failed: {e.Message}");
            }
        }

        public static JournalSave? Load(string? pathOverride = null)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!File.Exists(path)) return null;
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<JournalSave>(json, s_options);
            }
            catch (Exception e)
            {
                GD.PrintErr($"[JournalSaveStore] load failed: {e.Message}");
                return null;
            }
        }
    }
}
