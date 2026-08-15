using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Maritime (Expansion 09) save persistence — thin pattern sibling of the
    /// other host stores: user:// path, try/catch, checksummed envelope.
    /// </summary>
    public static class MaritimeSaveStore
    {
        public const string FileName = "maritime_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(MaritimeHostSave save)
        {
            try
            {
                if (save == null) return false;
                save.Checksum = SaveChecksum.Compute(save);
                string path = SavePath;
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(path, s_json.Serialize(save));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Maritime] save failed: " + e.Message);
                return false;
            }
        }

        public static MaritimeHostSave TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var save = s_json.Deserialize<MaritimeHostSave>(raw);
                if (save == null) return null;
                if (!string.IsNullOrEmpty(save.Checksum))
                {
                    string actual = SaveChecksum.Compute(save);
                    if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                    {
                        GD.PrintErr("[Maritime] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                }
                return save;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Maritime] load failed: " + e.Message);
                return null;
            }
        }
    }
}
