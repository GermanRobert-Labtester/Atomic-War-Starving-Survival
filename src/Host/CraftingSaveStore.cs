using System;
#pragma warning disable CS8618
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
        public const string SectionName = "crafting";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(CraftingSystemSave state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static CraftingSystemSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(CraftingSystemSave state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[CraftingSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static CraftingSystemSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<CraftingSystemSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[CraftingSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(CraftingSystemSave state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new CraftingHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = SavePath;
                string? dir = Path.GetDirectoryName(path);
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

        public static CraftingSystemSave? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<CraftingHostSave>(raw);
                if (envelope == null || envelope.State == null) return null;
                // The checksummed envelope is the current Crafting format; an
                // empty checksum means a malformed new-format save, not "legacy".
                if (string.IsNullOrEmpty(envelope.Checksum))
                {
                    GD.PrintErr("[Crafting] load failed: checksum field missing (corrupt save).");
                    return null;
                }
                string actual = SaveChecksum.Compute(envelope);
                if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                {
                    GD.PrintErr("[Crafting] load failed: checksum mismatch (corrupt or foreign save).");
                    return null;
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
