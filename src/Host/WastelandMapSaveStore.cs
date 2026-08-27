// ============================================================================
// Save Store : WastelandMapSaveStore
// Core State : Ashfall.Core.World.WastelandMapState
// Host Caller: Main.Expeditions / WorldHostSession
// Purpose    : Wasteland travel map exploration, discovered POIs, and route fog-of-war
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Wasteland Map save persistence — closes the Setup-without-Save triad gap
    /// found by the audit on Main.cs. Pattern sibling of Combat/Narrative stores.
    /// </summary>
    public static class WastelandMapSaveStore
    {
        public const string FileName = "wasteland_map_save.json";
        public const string SectionName = "wasteland_map";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(WastelandMapState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static WastelandMapState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(WastelandMapState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[WastelandMapSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static WastelandMapState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<WastelandMapState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[WastelandMapSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(WastelandMapState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new WastelandMapSaveEnvelope { State = state };
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
                GD.PrintErr("[WastelandMap] save failed: " + e.Message);
                return false;
            }
        }

        public static WastelandMapState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<WastelandMapSaveEnvelope>(raw);
                if (envelope == null) return null;
                if (string.IsNullOrEmpty(envelope.Checksum))
                {
                    GD.PrintErr("[WastelandMap] save envelope missing checksum (corrupt save)");
                    return null;
                }
                string computed = SaveChecksum.Compute(envelope);
                if (!string.Equals(envelope.Checksum, computed, StringComparison.Ordinal))
                {
                    GD.PrintErr("[WastelandMap] checksum mismatch — possible tampering");
                    return null;
                }
                return envelope.State;
            }
            catch (Exception e)
            {
                GD.PrintErr("[WastelandMap] load failed: " + e.Message);
                return null;
            }
        }
    }

    [Serializable]
    public sealed class WastelandMapSaveEnvelope
    {
        public WastelandMapState State;
        public string Checksum;
    }
}
