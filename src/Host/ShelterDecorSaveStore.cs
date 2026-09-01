// ============================================================================
// Save Store : ShelterDecorSaveStore
// Core State : Ashfall.Core.Shelter.ShelterDecorState
// Host Caller: Main.Campaign / ShelterDecorHostSession
// Purpose    : Per-room decor placements, memorial plaque mounts, and the
//              localized-morale modifier registry that the host wires from
//              items.json at boot.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists ShelterDecorState under user://shelter_decor_save.json \u2014
    /// a thin static façade over the Core SaveStore&lt;T&gt; service via
    /// SaveStoreHub (codec flavor). The Core state class is
    /// self-checksummed (the checksum is a field of the state itself), so
    /// encode/decode stamp and verify it directly; path resolution, atomic
    /// write, and error handling live in the service.
    /// </summary>
    public static class ShelterDecorSaveStore
    {
        public const string FileName = "shelter_decor_save.json";
        public const string SectionName = "shelter_decor";

        private static readonly SaveStore<ShelterDecorStateCapture> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterDecorSaveStore),
            EncodeSave,
            DecodeSave);

        public static string SavePath => s_store.SavePath;

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ShelterDecorState state) => s_store.CaptureBare(ToCapture(state));

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ShelterDecorState? TryRestoreDirect(string json)
        {
            var cap = s_store.RestoreBare(json);
            return cap == null ? null : FromCapture(cap);
        }

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ShelterDecorState state) => s_store.CaptureBare(ToCapture(state));

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ShelterDecorState? TryRestore(string json)
        {
            var cap = s_store.RestoreBare(json);
            return cap == null ? null : FromCapture(cap);
        }

        public static bool TrySave(ShelterDecorState state) => s_store.TrySave(ToCapture(state));

        public static ShelterDecorState? TryLoad()
        {
            var cap = s_store.TryLoad();
            return cap == null ? null : FromCapture(cap);
        }

        public static string TryCapturePersisted(ShelterDecorState state)
            => s_store.CapturePersisted(ToCapture(state));

        private static ShelterDecorStateCapture ToCapture(ShelterDecorState state)
        {
            var cap = new ShelterDecorStateCapture
            {
                systemId = state.systemId,
                Placements = state.Placements ?? new System.Collections.Generic.List<ShelterDecorPlacement>()
            };
            cap.Checksum = SaveChecksum.Compute(cap);
            return cap;
        }

        private static ShelterDecorState FromCapture(ShelterDecorStateCapture cap)
        {
            var s = new ShelterDecorState
            {
                systemId = cap.systemId,
                Placements = cap.Placements ?? new System.Collections.Generic.List<ShelterDecorPlacement>()
            };
            return s;
        }

        private static string EncodeSave(ShelterDecorStateCapture cap, IJsonSerializer json)
        {
            cap.Checksum = SaveChecksum.Compute(cap);
            return json.Serialize(cap);
        }

        private static ShelterDecorStateCapture? DecodeSave(string raw, IJsonSerializer json)
        {
            var cap = json.Deserialize<ShelterDecorStateCapture>(raw);
            if (cap == null) return null;
            if (string.IsNullOrEmpty(cap.Checksum))
                throw new InvalidOperationException("ShelterDecor: empty checksum");
            if (!string.Equals(cap.Checksum, SaveChecksum.Compute(cap), StringComparison.Ordinal))
                throw new InvalidOperationException("ShelterDecor: checksum mismatch");
            return cap;
        }
    }
}
