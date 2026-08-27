// ============================================================================
// Save Store : Phase0SaveStore
// Core State : Ashfall.Core.Phase0EffectsSaveState
// Host Caller: Main.Phase0 / Phase0HostSession
// Purpose    : Phase 0 survivor behavioral quirks, specialized perks, and lingering trauma
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Phase-0 effects save persistence — thin pattern sibling of the other
    /// host stores: user:// path, try/catch, checksummed envelope. The Phase-0
    /// session was previously never persisted; wiring it into a store closes
    /// that gap (permanent shelter morale buff must survive reloads).
    /// </summary>
    public static class Phase0SaveStore
    {
        public const string FileName = "phase0_save.json";
        public const string SectionName = "phase0";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(Phase0EffectsSaveState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static Phase0EffectsSaveState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(Phase0EffectsSaveState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[Phase0SaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static Phase0EffectsSaveState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<Phase0EffectsSaveState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[Phase0SaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(Phase0EffectsSaveState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new Phase0HostSave { State = state };
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
                GD.PrintErr("[Phase0] save failed: " + e.Message);
                return false;
            }
        }

        public static Phase0EffectsSaveState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<Phase0HostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    // The checksummed envelope is the only Phase-0 format; an
                    // empty checksum means a malformed new-format save.
                    if (string.IsNullOrEmpty(envelope.Checksum))
                    {
                        GD.PrintErr("[Phase0] load failed: checksum field missing (corrupt save).");
                        return null;
                    }
                    string actual = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    {
                        GD.PrintErr("[Phase0] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                    return envelope.State;
                }
                return null;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Phase0] load failed: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>Phase-0 save envelope: engine state + integrity checksum.</summary>
    public class Phase0HostSave
    {
        public Phase0EffectsSaveState State;
        public string Checksum = string.Empty;
    }
}
