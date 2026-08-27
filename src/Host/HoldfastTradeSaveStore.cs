// ============================================================================
// Save Store : HoldfastTradeSaveStore
// Core State : Ashfall.Core.HoldfastTradeSaveState
// Host Caller: Main.Holdfast, Main.SaveOrchestrator / HoldfastRuntimeSession
// Purpose    : Holdfast trade ledger, merchant transactions, and trade credit balance
// ============================================================================
using System;
using System.Globalization;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Godot-side persistence for mutable Holdfast player/trade state. Canonical
    /// item definitions remain in the Core catalog; the save stores ids and state.
    /// Façade over the Core SaveStore&lt;T&gt; service (codec flavor) for
    /// envelope/checksum/atomic-write; the backup rotation (preserve the oldest
    /// snapshot) and corruption quarantine need direct file access and stay here.
    /// </summary>
    public static class HoldfastTradeSaveStore
    {
        public const string FileName = "holdfast_trade_save.json";
        public const string SectionName = "holdfast_trade";

        private static readonly IFileIO s_files = new FileSystemIO();

        private static readonly SaveStore<HoldfastTradeSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(HoldfastTradeSaveStore),
            EncodeState,
            DecodeState);

        public static string SavePath => s_store.SavePath;

        public static string BackupPath => SavePath + ".bak";

        public static string BackupPathFor(string path) => path + ".bak";

        public static bool Exists => s_files.FileExists(SavePath);

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(HoldfastTradeSaveState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static HoldfastTradeSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(HoldfastTradeSaveState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static HoldfastTradeSaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(HoldfastTradeSaveState state, string pathOverride = null!)
        {
            if (state == null) return false;
            string path = pathOverride ?? s_store.SavePath;
            string backup = path + ".bak";

            // Rotate prior file to backup before overwriting (keeping the
            // oldest snapshot: only rotate while no backup exists).
            if (s_files.FileExists(path) && !s_files.FileExists(backup))
            {
                try { File.Move(path, backup); }
                catch (Exception) { /* best-effort rotation */ }
            }

            return s_store.TrySave(state, pathOverride);
        }

        public static HoldfastTradeSaveState? TryLoad(string pathOverride = null!)
        {
            try
            {
                string path = pathOverride ?? s_store.SavePath;
                if (!s_files.FileExists(path)) return null;

                string text = s_files.ReadAllText(path);
                var envelope = DecodeEnvelope(text);
                if (envelope != null)
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
                        var backupEnvelope = DecodeEnvelope(s_files.ReadAllText(backupPath));
                        if (backupEnvelope != null)
                        {
                            GD.Print("[HoldfastTrade] Loaded from backup after primary was quarantined.");
                            return backupEnvelope.State;
                        }
                    }
                    catch (Exception) { /* cleanup: fallback when backup is also corrupt */ }
                }

                return null;
            }
            catch (Exception e)
            {
                GD.PrintErr("[HoldfastTrade] load failed: " + e.Message);
                return null;
            }
        }

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(HoldfastTradeSaveState state) => s_store.CapturePersisted(state);

        private static string EncodeState(HoldfastTradeSaveState state, IJsonSerializer json)
        {
            var envelope = new HoldfastTradeSaveEnvelope
            {
                State = state,
                Checksum = SaveChecksum.Compute(state)
            };
            return json.Serialize(envelope);
        }

        private static HoldfastTradeSaveState? DecodeState(string raw, IJsonSerializer json)
        {
            return DecodeEnvelope(raw)?.State;
        }

        private static HoldfastTradeSaveEnvelope? DecodeEnvelope(string text)
        {
            var envelope = new SystemTextJsonSerializer().Deserialize<HoldfastTradeSaveEnvelope>(text);
            if (envelope != null && envelope.State != null && !string.IsNullOrEmpty(envelope.Checksum)
                && string.Equals(SaveChecksum.Compute(envelope.State), envelope.Checksum, StringComparison.Ordinal))
            {
                return envelope;
            }
            return null;
        }

        private static void QuarantineCorrupt(string path, string text)
        {
            try
            {
                string corruptPath = path + ".corrupt-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
                if (!File.Exists(corruptPath))
                    File.WriteAllText(corruptPath, text);
                GD.Print("[HoldfastTrade] Corrupt save quarantined to " + Path.GetFileName(corruptPath));
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
