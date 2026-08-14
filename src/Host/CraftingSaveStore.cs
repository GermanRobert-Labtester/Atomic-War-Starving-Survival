using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Crafting;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Crafting save persistence — checksummed envelope, thin pattern sibling
    /// of InventorySaveStore / SurvivorsSaveStore.
    /// </summary>
    public static class CraftingSaveStore
    {
        public const string FileName = "crafting_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(CraftingSystemSave state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new CraftingHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = SavePath;
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(path, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Crafting] save failed: " + e.Message);
                return false;
            }
        }

        public static CraftingSystemSave TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<CraftingHostSave>(raw);
                if (envelope == null || envelope.State == null) return null;
                if (!string.IsNullOrEmpty(envelope.Checksum))
                {
                    string actual = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    {
                        GD.PrintErr("[Crafting] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                }
                return envelope.State;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Crafting] load failed: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>Crafting envelope: engine state + integrity checksum.</summary>
    public class CraftingHostSave
    {
        public CraftingSystemSave State;
        public string Checksum = string.Empty;
    }
}
