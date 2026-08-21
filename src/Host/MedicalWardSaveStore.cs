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
        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

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
