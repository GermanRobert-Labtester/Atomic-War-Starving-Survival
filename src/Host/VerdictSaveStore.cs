// ============================================================================
// Save Store : VerdictSaveStore
// Core State : Ashfall.Core.Verdict.VerdictSave
// Host Caller: Main.Verdict / VerdictHostSession
// Purpose    : The Verdict tribunal reckoning stages, evidence dossier, and census tally
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Verdict;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — save persistence. Thin façade
    /// over the Core SaveStore&lt;T&gt; service (via SaveStoreHub, codec
    /// flavor). Shape and validation live in <see cref="VerdictSaveCodec"/>;
    /// path resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class VerdictSaveStore
    {
        public const string FileName = "verdict_save.json";
        public const string SectionName = "verdict";

        private static readonly SaveStore<VerdictSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(VerdictSaveStore),
            (save, json) => VerdictSaveCodec.Encode(save, json),
            DecodeVerdict);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(VerdictSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static VerdictSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(VerdictSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static VerdictSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(VerdictSave save, string pathOverride = null!) =>
            s_store.TrySave(save, pathOverride);

        public static VerdictSave? TryLoad(string pathOverride = null!) =>
            s_store.TryLoad(pathOverride);

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(VerdictSave save) => s_store.CapturePersisted(save);

        private static VerdictSave? DecodeVerdict(string raw, IJsonSerializer json)
        {
            if (VerdictSaveCodec.TryDecode(raw, json, out var save))
                return save;
            throw new InvalidOperationException("save rejected (bad checksum or version).");
        }
    }
}
