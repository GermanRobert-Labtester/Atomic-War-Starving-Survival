// ============================================================================
// Save Store : PowerGridSaveStore
// Core State : Ashfall.Core.Shelter.PowerGridSave
// Host Caller: Main.ShelterInfrastructure / PowerGridHostSession
// Purpose    : Shelter electrical power grid, generator fuel, battery capacity, and blackout zones
// ============================================================================
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="PowerGridSave"/> as JSON under
    /// <c>user://power_grid_save.json</c> using the core
    /// <see cref="IFileIO"/> / <see cref="SystemTextJsonSerializer"/> ports.
    /// Shape/checksum live in <see cref="Ashfall.Core.Shelter.PowerGridSaveCodec"/>.
    /// </summary>
    public static class PowerGridSaveStore
    {
        public const string FileName = "power_grid_save.json";
        public const string SectionName = "power_grid";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(PowerGridSave state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static PowerGridSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(PowerGridSave state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[PowerGridSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static PowerGridSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<PowerGridSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[PowerGridSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(PowerGridSave save)
        {
            if (save == null) return false;
            try
            {
                s_files.WriteAllText(SavePath, PowerGridSaveCodec.EncodeToString(save, s_json));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[PowerGridSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        public static PowerGridSave? TryLoad()
        {
            try
            {
                if (!s_files.FileExists(SavePath)) return null;
                return PowerGridSaveCodec.Decode(s_files.ReadAllText(SavePath), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[PowerGridSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
