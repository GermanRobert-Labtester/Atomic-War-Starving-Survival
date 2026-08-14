using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="DutyRosterSave"/> as JSON under user://duty_roster_save.json
    /// using the core IFileIO / SystemTextJsonSerializer ports. The save shape and all
    /// validation live in Ashfall.Core.DutyRosterSaveCodec — this type only picks the
    /// Godot path and the log. Mirrors HoldfastSaveStore / YearOfAshSaveStore.
    /// </summary>
    public static class DutyRosterSaveStore
    {
        public const string FileName = "duty_roster_save.json";

        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
        public static bool TrySave(DutyRosterSave save, string pathOverride = null)
        {
            if (save == null) return false;
            try
            {
                string path = pathOverride ?? SavePath;
                s_files.WriteAllText(path, DutyRosterSaveCodec.Encode(save, s_json));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[DutyRosterSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static DutyRosterSave TryLoad(string pathOverride = null)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                return DutyRosterSaveCodec.Decode(s_files.ReadAllText(path), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[DutyRosterSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
