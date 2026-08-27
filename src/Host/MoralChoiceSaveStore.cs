// ============================================================================
// Save Store : MoralChoiceSaveStore
// Core State : Ashfall.Core.MoralChoice.MoralChoiceState
// Host Caller: Main.MoralChoice / MoralChoiceHostSession
// Purpose    : Moral choice branches, ethical dilemmas, community trust, and faction reactions
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists MoralChoiceState as JSON under user://moral_choice_save.json via the
    /// engine-agnostic core serializer inside a checksummed envelope, matching every
    /// other host save store. Legacy bare-state saves (pre-checksum) still load.
    /// </summary>
    public static class MoralChoiceSaveStore
    {
        public const string FileName = "moral_choice_save.json";
        public const string SectionName = "host_event";

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static void Save(MoralChoiceState state, string? pathOverride = null)
        {
            if (state == null) return;
            try
            {
                var envelope = new MoralChoiceHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = pathOverride ?? SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(path, s_json.Serialize(envelope));
            }
            catch (Exception e)
            {
                GD.PrintErr($"[MoralChoiceSaveStore] save failed: {e.Message}");
            }
        }

        public static MoralChoiceState? TryLoad(string? pathOverride = null)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;
                string json = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json)) return null;

                var envelope = s_json.Deserialize<MoralChoiceHostSave>(json);
                if (envelope != null && envelope.State != null)
                {
                    // A non-empty checksum field is required for any save in the
                    // new envelope format; an empty one means corrupt, not legacy.
                    if (string.IsNullOrEmpty(envelope.Checksum))
                    {
                        GD.PrintErr("[MoralChoiceSaveStore] load failed: checksum field missing (corrupt save).");
                        return null;
                    }
                    string actual = SaveChecksum.Compute(envelope);
                    if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    {
                        GD.PrintErr("[MoralChoiceSaveStore] load failed: checksum mismatch (corrupt or foreign save).");
                        return null;
                    }
                    return envelope.State;
                }

                // Legacy bare-state save (written before the checksum envelope).
                return s_json.Deserialize<MoralChoiceState>(json);
            }
            catch (Exception e)
            {
                GD.PrintErr($"[MoralChoiceSaveStore] load failed: {e.Message}");
                return null;
            }
        }
    }

    /// <summary>Moral choice save envelope: ledger state + integrity checksum.</summary>
    public class MoralChoiceHostSave
    {
        public MoralChoiceState State;
        public string Checksum = string.Empty;
    }
}
