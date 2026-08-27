// ============================================================================
// Save Store : PhantomMemorySaveStore
// Core State : Ashfall.Core.PhantomMemoryEngineState
// Host Caller: Main.Phase0 / PhantomMemoryHostSession
// Purpose    : Phase 0 phantom memory engine, trauma flashbacks, and psychological echoes
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>Phantom Memory envelope: engine state + integrity checksum.</summary>
    public class PhantomMemoryHostSave
    {
        public PhantomMemoryEngineState State;
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Phantom Memory (Antigravity #41) save persistence — same thin pattern as
    /// the other host stores: user:// path, try/catch, codec serialization.
    /// </summary>
    public static class PhantomMemorySaveStore
    {
        public const string FileName = "phantom_memory_save.json";
        public const string SectionName = "phantom_memory";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(PhantomMemoryEngineState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static PhantomMemoryEngineState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(PhantomMemoryEngineState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[PhantomMemorySaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static PhantomMemoryEngineState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<PhantomMemoryEngineState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[PhantomMemorySaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(PhantomMemoryEngineState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new PhantomMemoryHostSave { State = state };
                // Recompute so a mutated envelope cannot persist a stale hash.
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
                GD.PrintErr("[PhantomMemory] save failed: " + e.Message);
                return false;
            }
        }

        public static PhantomMemoryEngineState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<PhantomMemoryHostSave>(raw);
                if (envelope == null || envelope.State == null) return null;
                // The checksummed envelope is the current Phantom Memory format;
                // an empty checksum means a malformed new-format save, not
                // "legacy" (a pre-envelope bare-state file yields State == null
                // and is dropped above).
                if (string.IsNullOrEmpty(envelope.Checksum))
                {
                    GD.PrintErr("[PhantomMemory] load failed: checksum field missing (corrupt save).");
                    return null;
                }
                string actual = SaveChecksum.Compute(envelope);
                if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                {
                    GD.PrintErr("[PhantomMemory] load failed: checksum mismatch (corrupt or foreign save).");
                    return null;
                }
                return envelope.State;
            }
            catch (Exception e)
            {
                GD.PrintErr("[PhantomMemory] load failed: " + e.Message);
                return null;
            }
        }
    }
}
