// ============================================================================
// Save Store : InventorySaveStore
// Core State : Ashfall.Core.Inventory.InventorySaveState
// Host Caller: Main.Inventory / InventoryHostSession
// Purpose    : Shelter storage inventory, container contents, item stacks, and gear durability
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Inventory save persistence — thin pattern sibling of the other host stores:
    /// user:// path, try/catch, checksummed envelope serialization.
    /// </summary>
    public static class InventorySaveStore
    {
        public const string FileName = "inventory_save.json";
        public const string SectionName = "inventory";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(InventorySaveState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static InventorySaveState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(InventorySaveState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[InventorySaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static InventorySaveState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<InventorySaveState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[InventorySaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(InventorySaveState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new InventoryHostSave { State = state };
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
                GD.PrintErr("[Inventory] save failed: " + e.Message);
                return false;
            }
        }

        public static InventorySaveState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<InventoryHostSave>(raw);
                if (envelope == null || envelope.State == null) return null;
                // The checksummed envelope is the current Inventory format; an
                // empty checksum means a malformed new-format save, not "legacy".
                if (string.IsNullOrEmpty(envelope.Checksum))
                {
                    GD.PrintErr("[Inventory] load failed: checksum field missing (corrupt save).");
                    return null;
                }
                string actual = SaveChecksum.Compute(envelope);
                if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                {
                    GD.PrintErr("[Inventory] load failed: checksum mismatch (corrupt or foreign save).");
                    return null;
                }
                return envelope.State;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Inventory] load failed: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>Inventory envelope: engine state + integrity checksum.</summary>
    public class InventoryHostSave
    {
        public InventorySaveState State;
        public string Checksum = string.Empty;
    }
}
