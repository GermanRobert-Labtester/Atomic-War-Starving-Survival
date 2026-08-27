// ============================================================================
// Save Store : DutyRosterSaveStore
// Core State : Ashfall.Core.DutyRosterSave
// Host Caller: Main.DutyRoster, Main.Holdfast / DutyRosterHostSession
// Purpose    : Duty roster shift allocations, work assignments, and fatigue modifiers
// ============================================================================
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
        public const string SectionName = "duty_roster";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(DutyRosterSave state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static DutyRosterSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(DutyRosterSave state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[DutyRosterSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static DutyRosterSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<DutyRosterSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[DutyRosterSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
        public static bool TrySave(DutyRosterSave save, string pathOverride = null!)
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
        public static DutyRosterSave? TryLoad(string pathOverride = null!)
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
