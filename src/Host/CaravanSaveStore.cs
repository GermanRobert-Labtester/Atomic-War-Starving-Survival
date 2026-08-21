using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Traveling Caravan save persistence — thin pattern sibling of the other
    /// host stores: user:// path, try/catch, checksummed envelope.
    /// </summary>
    public static class CaravanSaveStore
    {
        public const string FileName = "caravan_save.json";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(TravelingCaravanState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new CaravanHostSave { State = state };
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
                GD.PrintErr("[Caravan] save failed: " + e.Message);
                return false;
            }
        }

        public static TravelingCaravanState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;

                var envelope = s_json.Deserialize<CaravanHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    // A non-empty checksum is required for any save in the new
                    // envelope format. Empty/null previously slipped past as
                    // "legacy" — a malformed save in the new format must be
                    // rejected, not silently trusted.
                    if (string.IsNullOrEmpty(envelope.Checksum))
                    {
                        GD.PrintErr("[Caravan] load failed: checksum field missing (corrupt save).");
                        return null;
                    }
                    string actual = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    {
                        GD.PrintErr("[Caravan] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                    return envelope.State;
                }

                // Legacy bare-state save (written before the checksum envelope).
                return s_json.Deserialize<TravelingCaravanState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Caravan] load failed: " + e.Message);
                return null;
            }
        }
    }

    /// <summary>Caravan save envelope: engine state + integrity checksum.</summary>
    public class CaravanHostSave
    {
        public TravelingCaravanState State;
        public string Checksum = string.Empty;
    }
}
