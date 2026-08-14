using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp.YearOfAsh
{
    /// <summary>
    /// File persistence adapter for YearOfAshSave in the Godot host environment.
    /// Stores the save file in user://year_of_ash_save.json.
    /// </summary>
    public static class YearOfAshSaveStore
    {
        public const string FileName = "year_of_ash_save.json";

        public static string SavePath =>
            Path.Combine(OS.GetUserDataDir(), FileName);

        public static bool Exists => File.Exists(SavePath);

        public static bool TrySave(YearOfAshSave save)
        {
            if (save == null) return false;
            try
            {
                var serializer = new SystemTextJsonSerializer();
                string json = YearOfAshSaveCodec.Encode(save, serializer);
                File.WriteAllText(SavePath, json);
                return true;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[YearOfAshSaveStore] Failed to write save: {ex.Message}");
                return false;
            }
        }

        public static YearOfAshSave TryLoad()
        {
            if (!Exists) return null;
            try
            {
                var serializer = new SystemTextJsonSerializer();
                string json = File.ReadAllText(SavePath);
                return YearOfAshSaveCodec.Decode(json, serializer);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[YearOfAshSaveStore] Failed to load save: {ex.Message}");
                return null;
            }
        }
    }
}
