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

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

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
