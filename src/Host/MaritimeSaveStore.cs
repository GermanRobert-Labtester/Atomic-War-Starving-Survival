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
        public const string SectionName = "maritime";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(MaritimeHostSave save)
    {
        return TryCapture(save);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static MaritimeHostSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(MaritimeHostSave save)
    {
        try
        {
            if (save == null) return string.Empty;
            return s_json.Serialize(save);
        }
        catch (Exception e)
        {
            GD.PrintErr("[MaritimeSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static MaritimeHostSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<MaritimeHostSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[MaritimeSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(MaritimeHostSave save)
        {
            try
            {
                if (save == null) return false;
                save.Checksum = SaveChecksum.Compute(save);
                string path = SavePath;
                string? dir = Path.GetDirectoryName(path);
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

        public static MaritimeHostSave? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var save = s_json.Deserialize<MaritimeHostSave>(raw);
                if (save == null) return null;
                // The checksummed envelope is the only Maritime format; an empty
                // checksum means a malformed new-format save, not "legacy".
                if (string.IsNullOrEmpty(save.Checksum))
                {
                    GD.PrintErr("[Maritime] load failed: checksum field missing (corrupt save).");
                    return null;
                }
                string actual = SaveChecksum.Compute(save);
                if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                {
                    GD.PrintErr("[Maritime] load failed: checksum mismatch (corrupt or foreign save).");
                    return null;
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
