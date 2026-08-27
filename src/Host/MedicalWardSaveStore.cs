// ============================================================================
// Save Store : MedicalWardSaveStore
// Core State : Ashfall.Core.MedicalWardSave
// Host Caller: Main.Medical / MedicalWardHostSession
// Purpose    : Medical ward bed occupancy, hospital triage, and critical care status
// ============================================================================
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    /// <summary>Persists MedicalWardSave under user://medical_ward_save.json.</summary>
    public static class MedicalWardSaveStore
    {
        public const string FileName = "medical_ward_save.json";
        public const string SectionName = "medical_ward";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(MedicalWardSave state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static MedicalWardSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(MedicalWardSave state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[MedicalWardSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static MedicalWardSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<MedicalWardSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[MedicalWardSaveStore] restore failed: " + e.Message);
            return null;
        }
    }

        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool TrySave(MedicalWardSave save)
        {
            if (save == null) return false;
            try
            {
                s_files.WriteAllText(SavePath, MedicalWardSaveCodec.EncodeToString(save, s_json));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[MedicalWardSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        public static MedicalWardSave? TryLoad()
        {
            try
            {
                if (!s_files.FileExists(SavePath)) return null;
                return MedicalWardSaveCodec.Decode(s_files.ReadAllText(SavePath), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[MedicalWardSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
