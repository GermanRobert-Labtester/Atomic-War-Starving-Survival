// ============================================================================
// Save Store : MusterSaveStore
// Core State : Ashfall.Core.Muster.MusterHostSave
// Host Caller: Main.Muster / MusterHostSession
// Purpose    : The Muster expansion coalition standings, faction votes, and military escalations
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>Combined Muster envelope: escalation state + coalition camp + the six
    /// Section V current state machines + Hydro-Barons.</summary>
    public class MusterHostSave
    {
        public MusterState Muster;
        public CoalitionCampState Camp;
        public ColdCountState ColdCount;
        public ProvisionedState Provisioned;
        public LongWalkState LongWalk;
        public ScavengerGuildState ScavengerGuild;
        public IronRaidersState IronRaiders;
        public HydroBaronsState HydroBarons;
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Muster (Expansion 06) save persistence — façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). The
    /// multi-field Muster envelope is self-checksummed; encode/decode stamp
    /// and verify it directly while path resolution, atomic write, and error
    /// handling live in the service.
    /// </summary>
    public static class MusterSaveStore
    {
        public const string FileName = "muster_save.json";
        public const string SectionName = "muster";

        private static readonly SaveStore<MusterHostSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(MusterSaveStore),
            EncodeSave,
            DecodeSave);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(MusterHostSave save) => s_store.CaptureBare(save);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static MusterHostSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(MusterHostSave save) => s_store.CaptureBare(save);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static MusterHostSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(MusterHostSave save) => s_store.TrySave(save);

        public static MusterHostSave? TryLoad() => s_store.TryLoad();

        private static string EncodeSave(MusterHostSave save, IJsonSerializer json)
        {
            // Recompute so a mutated envelope cannot persist a stale hash.
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        private static MusterHostSave? DecodeSave(string raw, IJsonSerializer json)
        {
            var save = json.Deserialize<MusterHostSave>(raw);
            if (save == null) return null;
            // The checksummed envelope is the current Muster format; an empty
            // checksum means a malformed new-format save, not "legacy". A
            // pre-envelope bare-state file yields a null-shaped envelope and
            // falls through to a fresh state (no partial restore).
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException("checksum field missing (corrupt save).");
            if (!string.Equals(save.Checksum, SaveChecksum.Compute(save), StringComparison.Ordinal))
                throw new InvalidOperationException("checksum mismatch (corrupt or foreign save).");
            return save;
        }
    }
}
