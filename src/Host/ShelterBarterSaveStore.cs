// ============================================================================
// Save Store : ShelterBarterSaveStore
// Core State : Ashfall.Core.Economy.ShelterBarterSaveState
// Host Caller: Main.Plans147 / ShelterBarterSystem
// Purpose    : Shelter barter persistence (Plan 54 system, Plan 147 host
//              wiring) — caravan schedules, pinned per-arrival stock, and
//              the contraband broker counter. The broker definition itself
//              is code-built from the contraband activation map and is
//              never serialized.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Shelter barter save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter. No pre-envelope legacy format existed for this section.
    /// </summary>
    public static class ShelterBarterSaveStore
    {
        public const string FileName = "shelter_barter_save.json";
        public const string SectionName = "shelter_barter";

        private static readonly SaveStore<ShelterBarterSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterBarterSaveStore),
            SchemaVersionedEnvelope<ShelterBarterSaveState>.Encode,
            SchemaVersionedEnvelope<ShelterBarterSaveState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture the exact persisted envelope bytes for the campaign aggregate without writing to disk.</summary>
        public static string TryCapturePersisted(ShelterBarterSaveState state) => s_store.CapturePersisted(state);

        public static bool TrySave(ShelterBarterSaveState state) => s_store.TrySave(state);

        public static ShelterBarterSaveState? TryLoad() => s_store.TryLoad();
    }
}
