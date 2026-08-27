// ============================================================================
// Save Store : RadioSaveStore
// Core State : Ashfall.Core.Radio.RadioSaveState
// Host Caller: Main.Narrative / RadioHostSession
// Purpose    : Radio frequency tuning, intercepted broadcast history, and signal triangulation
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Radio save persistence — thin façade over the Core SaveStore&lt;T&gt;
    /// service (via SaveStoreHub, codec flavor). Checksummed codec
    /// serialization lives in <see cref="RadioSaveCodec"/>; path resolution,
    /// atomic write, and error handling live in the service. This is the
    /// single canonical persisted owner of receiver state; no other store
    /// serializes it.
    /// </summary>
    public static class RadioSaveStore
    {
        public const string FileName = "radio_save.json";
        public const string SectionName = "radio";

        private static readonly SaveStore<RadioSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RadioSaveStore),
            (state, json) => RadioSaveCodec.Encode(state, json),
            DecodeRadio);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(RadioSaveState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static RadioSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(RadioSaveState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static RadioSaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(RadioSaveState state, string pathOverride = null!) =>
            s_store.TrySave(state, pathOverride);

        public static RadioSaveState? TryLoad(string pathOverride = null!) =>
            s_store.TryLoad(pathOverride);

        private static RadioSaveState? DecodeRadio(string raw, IJsonSerializer json)
        {
            if (RadioSaveCodec.TryDecode(raw, json, out var state))
                return state;
            throw new InvalidOperationException("save rejected (bad checksum or version).");
        }
    }
}
