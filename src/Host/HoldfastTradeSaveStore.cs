// ============================================================================
// Save Store : HoldfastTradeSaveStore
// Core State : Ashfall.Core.HoldfastTradeSaveState
// Host Caller: Main.Holdfast, Main.SaveOrchestrator / HoldfastRuntimeSession
// Purpose    : Holdfast trade ledger, merchant transactions, and trade credit balance
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Godot-side persistence for mutable Holdfast player/trade state. Canonical
    /// item definitions remain in the Core catalog; the save stores ids and state.
    /// Includes backup rotation and corruption quarantine.
    /// </summary>
    public static class HoldfastTradeSaveStore
    {
        public const string FileName = "holdfast_trade_save.json";
        public const string SectionName = "holdfast_trade";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(HoldfastTradeSaveState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static HoldfastTradeSaveState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(HoldfastTradeSaveState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[HoldfastTradeSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static HoldfastTradeSaveState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<HoldfastTradeSaveState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[HoldfastTradeSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static string BackupPath => SavePath + ".bak";

        public static string BackupPathFor(string path) => path + ".bak";

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(HoldfastTradeSaveState state, string pathOverride = null!)
        {
            try
            {
                if (state == null) return false;
                string path = pathOverride ?? SavePath;
                string backup = path + ".bak";

                // Rotate prior file to backup before overwriting.
                if (s_files.FileExists(path) && !s_files.FileExists(backup))
                {
                    try { System.IO.File.Move(path, backup); }
                    catch (Exception) { /* best-effort rotation */ }
                }

                var envelope = new HoldfastTradeSaveEnvelope
                {
                    State = state,
                    Checksum = SaveChecksum.Compute(state)
                };
                s_files.WriteAllText(path, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[HoldfastTrade] save failed: " + e.Message);
                return false;
            }
        }

        public static HoldfastTradeSaveState? TryLoad(string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? SavePath;
                if (!s_files.FileExists(path)) return null;

                string text = s_files.ReadAllText(path);
                var envelope = s_json.Deserialize<HoldfastTradeSaveEnvelope>(text);
                if (envelope != null && envelope.State != null && !string.IsNullOrEmpty(envelope.Checksum)
                    && string.Equals(SaveChecksum.Compute(envelope.State), envelope.Checksum, StringComparison.Ordinal))
                {
                    return envelope.State;
                }

                // Primary failed: quarantine it and try the backup.
                QuarantineCorrupt(path, text);
                string backupPath = BackupPathFor(path);
                if (s_files.FileExists(backupPath))
                {
                    try
                    {
                        string backupText = s_files.ReadAllText(backupPath);
                        var backupEnvelope = s_json.Deserialize<HoldfastTradeSaveEnvelope>(backupText);
                        if (backupEnvelope != null && backupEnvelope.State != null
                            && !string.IsNullOrEmpty(backupEnvelope.Checksum)
                            && string.Equals(SaveChecksum.Compute(backupEnvelope.State), backupEnvelope.Checksum, StringComparison.Ordinal))
                        {
                            GD.Print("[HoldfastTrade] Loaded from backup after primary was quarantined.");
                            return backupEnvelope.State;
                        }
                    }
                    catch (Exception) { /* backup also corrupt */ }
                }

                return null;
            }
            catch (Exception e)
            {
                GD.PrintErr("[HoldfastTrade] load failed: " + e.Message);
                return null;
            }
        }

        private static void QuarantineCorrupt(string path, string text)
        {
            try
            {
                string corruptPath = path + ".corrupt-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture);
                if (!System.IO.File.Exists(corruptPath))
                    System.IO.File.WriteAllText(corruptPath, text);
                GD.Print("[HoldfastTrade] Corrupt save quarantined to " + System.IO.Path.GetFileName(corruptPath));
            }
            catch (Exception) { /* quarantine is best-effort */ }
        }

        private sealed class HoldfastTradeSaveEnvelope
        {
            public HoldfastTradeSaveState State;
            public string Checksum = string.Empty;
        }
    }
}
