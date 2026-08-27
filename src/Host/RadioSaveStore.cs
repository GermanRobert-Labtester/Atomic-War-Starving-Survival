// ============================================================================
// Save Store : RadioSaveStore
// Core State : Ashfall.Core.Radio.RadioSaveState
// Host Caller: Main.Narrative / RadioHostSession
// Purpose    : Radio frequency tuning, intercepted broadcast history, and signal triangulation
// ============================================================================
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Radio save persistence — thin pattern sibling of the other host stores:
    /// user:// path, try/catch, checksummed codec serialization via the
    /// engine-agnostic <see cref="RadioSaveCodec"/>. This is the single
    /// canonical persisted owner of receiver state; no other store serializes it.
    /// </summary>
    public static class RadioSaveStore
    {
        public const string FileName = "radio_save.json";
        public const string SectionName = "radio";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(RadioSaveState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static RadioSaveState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(RadioSaveState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[RadioSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static RadioSaveState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<RadioSaveState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[RadioSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(RadioSaveState state, string pathOverride = null!)
        {
            try
            {
                if (state == null) return false;
                string path = pathOverride ?? SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(path, RadioSaveCodec.Encode(state, s_json));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Radio] save failed: " + e.Message);
                return false;
            }
        }

        public static RadioSaveState? TryLoad(string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                if (RadioSaveCodec.TryDecode(raw, s_json, out var state))
                    return state;
                GD.PrintErr("[Radio] save rejected (bad checksum or version).");
                return null;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Radio] load failed: " + e.Message);
                return null;
            }
        }
    }
}
