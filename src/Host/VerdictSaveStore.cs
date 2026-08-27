// ============================================================================
// Save Store : VerdictSaveStore
// Core State : Ashfall.Core.Verdict.VerdictSave
// Host Caller: Main.Verdict / VerdictHostSession
// Purpose    : The Verdict tribunal reckoning stages, evidence dossier, and census tally
// ============================================================================
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Verdict;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — save persistence. Thin pattern
    /// sibling of MusterSaveStore: user:// path, try/catch, codec serialization.
    /// </summary>
    public static class VerdictSaveStore
    {
        public const string FileName = "verdict_save.json";
        public const string SectionName = "verdict";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(VerdictSave state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static VerdictSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(VerdictSave state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[VerdictSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static VerdictSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<VerdictSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[VerdictSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(VerdictSave save, string pathOverride = null!)
        {
            try
            {
                if (save == null) return false;
                string path = pathOverride ?? SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                string encoded = VerdictSaveCodec.Encode(save, s_json);
                System.IO.File.WriteAllText(path, encoded);
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Verdict] save failed: " + e.Message);
                return false;
            }
        }

        public static VerdictSave? TryLoad(string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                if (VerdictSaveCodec.TryDecode(raw, s_json, out var save))
                    return save;
                GD.PrintErr("[Verdict] save rejected (bad checksum or version).");
                return null;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Verdict] load failed: " + e.Message);
                return null;
            }
        }
    }
}
