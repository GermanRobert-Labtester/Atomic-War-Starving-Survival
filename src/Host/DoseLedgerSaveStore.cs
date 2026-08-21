using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="DoseLedgerSave"/> as JSON under user://dose_ledger_save.json
    /// using the core IFileIO / SystemTextJsonSerializer ports. Shape and validation
    /// live in Ashfall.Core.DoseLedgerSaveCodec. This type only picks the Godot path and
    /// the log, mirroring the other expansion save stores.
    /// </summary>
    public static class DoseLedgerSaveStore
    {
        public const string FileName = "dose_ledger_save.json";

        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
        public static bool TrySave(DoseLedgerSave save, string pathOverride = null!)
        {
            if (save == null) return false;
            try
            {
                string path = pathOverride ?? SavePath;
                s_files.WriteAllText(path, DoseLedgerSaveCodec.Encode(save, s_json));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[DoseLedgerSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
        public static DoseLedgerSave? TryLoad(string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                return DoseLedgerSaveCodec.Decode(s_files.ReadAllText(path), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[DoseLedgerSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}