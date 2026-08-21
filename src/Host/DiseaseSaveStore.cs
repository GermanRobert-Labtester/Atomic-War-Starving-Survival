using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Disease;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Disease save persistence — pattern sibling of the other host stores:
    /// user:// path, try/catch, checksummed envelope. Closes triad gap for
    /// the Disease expansion (SetupXxx exists in Main.cs, SaveXxx was missing).
    /// </summary>
    public static class DiseaseSaveStore
    {
        public const string FileName = "disease_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(DiseaseSystemState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new DiseaseSaveEnvelope { State = state };
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
                GD.PrintErr("[Disease] save failed: " + e.Message);
                return false;
            }
        }

        public static DiseaseSystemState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path))
                    return TryLoadLegacy(path);
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw))
                    return TryLoadLegacy(path);
                var envelope = s_json.Deserialize<DiseaseSaveEnvelope>(raw);
                if (envelope == null) return TryLoadLegacy(path);
                if (string.IsNullOrEmpty(envelope.Checksum))
                {
                    GD.PrintErr("[Disease] save envelope missing checksum (corrupt save)");
                    return TryLoadLegacy(path);
                }
                string computed = SaveChecksum.Compute(envelope);
                if (!string.Equals(envelope.Checksum, computed, StringComparison.Ordinal))
                {
                    GD.PrintErr("[Disease] checksum mismatch — possible tampering");
                    return TryLoadLegacy(path);
                }
                return envelope.State;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Disease] load failed: " + e.Message);
                return TryLoadLegacy(SavePath);
            }
        }

        // Pre-checksum legacy loader: the host did not always issue envelopes.
        private static DiseaseSystemState? TryLoadLegacy(string path)
        {
            try
            {
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                return s_json.Deserialize<DiseaseSystemState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Disease] legacy load failed: " + e.Message);
                return null;
            }
        }
    }

    [Serializable]
    public sealed class DiseaseSaveEnvelope
    {
        public DiseaseSystemState State;
        public string Checksum;
    }
}
