// ============================================================================
// Save Store : WorldSaveStore
// Core State : Ashfall.Core.World.WorldHostSave
// Host Caller: Main.World / WorldHostSession
// Purpose    : World aggregate domain save: weather, hazard maps, and regional state
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// World (weather port) save persistence — façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). The World
    /// section is the one host store whose persisted shape is a multi-field
    /// envelope (weather state plus sky armor, location evolution, wildlife,
    /// and landmark sections), so checksum stamping/validation and the legacy
    /// bare-weather wrap are World-specific delegates here; path resolution,
    /// atomic write, and error handling live in the service.
    /// </summary>
    public static class WorldSaveStore
    {
        public const string FileName = "world_save.json";
        public const string SectionName = "world";

        private static readonly SaveStore<WorldHostSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(WorldSaveStore),
            EncodeEnvelope,
            DecodeEnvelope);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(WorldHostSave envelope) => s_store.CaptureBare(envelope);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static WorldHostSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(WorldHostSave envelope) => s_store.CaptureBare(envelope);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static WorldHostSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(
            WorldWeatherState state,
            SkyArmorSaveState skyArmor = null!,
            LocationEvolutionSaveState locationEvolution = null!,
            WildlifeSaveState wildlife = null!,
            LandmarkSaveState landmark = null!)
        {
            if (state == null) return false;
            var envelope = new WorldHostSave
            {
                State = state,
                SkyArmor = skyArmor,
                LocationEvolution = locationEvolution,
                Wildlife = wildlife,
                Landmark = landmark
            };
            return s_store.TrySave(envelope);
        }

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(
            WorldWeatherState state,
            SkyArmorSaveState skyArmor = null!,
            LocationEvolutionSaveState locationEvolution = null!,
            WildlifeSaveState wildlife = null!,
            LandmarkSaveState landmark = null!)
        {
            if (state == null) return string.Empty;
            var envelope = new WorldHostSave
            {
                State = state,
                SkyArmor = skyArmor,
                LocationEvolution = locationEvolution,
                Wildlife = wildlife,
                Landmark = landmark
            };
            return s_store.CapturePersisted(envelope);
        }

        public static WorldHostSave? TryLoadEnvelope() => s_store.TryLoad();

        public static WorldWeatherState? TryLoad()
        {
            return TryLoadEnvelope()?.State;
        }

        private static string EncodeEnvelope(WorldHostSave envelope, IJsonSerializer json)
        {
            envelope.Checksum = SaveChecksum.Compute(envelope);
            return json.Serialize(envelope);
        }

        private static WorldHostSave? DecodeEnvelope(string raw, IJsonSerializer json)
        {
            var envelope = json.Deserialize<WorldHostSave>(raw);
            if (envelope != null && envelope.State != null)
            {
                // A non-empty checksum field is required for any save in the
                // new envelope format. Empty/null is a malformed new-format
                // save — reject it, do not silently trust it.
                if (string.IsNullOrEmpty(envelope.Checksum))
                    throw new InvalidOperationException("checksum field missing (corrupt save).");
                string actual = SaveChecksum.Compute(envelope);
                if (!string.Equals(envelope.Checksum, actual, StringComparison.Ordinal))
                    throw new InvalidOperationException("checksum mismatch (corrupt or foreign save).");
                return envelope;
            }

            // Legacy bare-state save (written before the checksum envelope):
            // a bare weather state, wrapped into the current envelope shape.
            var legacy = json.Deserialize<WorldWeatherState>(raw);
            return legacy == null ? null : new WorldHostSave { State = legacy };
        }
    }

    /// <summary>World save envelope: engine state + sky armor + integrity checksum.</summary>
    public class WorldHostSave
    {
        public WorldWeatherState State;
        public SkyArmorSaveState SkyArmor;
        public LocationEvolutionSaveState LocationEvolution;
        public WildlifeSaveState Wildlife;
        public LandmarkSaveState Landmark;
        public string Checksum = string.Empty;
    }
}
