// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : AgricultureSaveStore
// Core State : Ashfall.Core.Farming (AgricultureState + NutritionDiversityState)
// Host Caller: Main.Plans162_165
// Purpose    : Advanced agriculture plot layer, compost, strains, and the
//              per-survivor dietary-diversity log (one campaign section).
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Farming;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>Combined persisted payload for the agriculture section.</summary>
    [System.Serializable]
    public sealed class AgricultureCampaignState
    {
        public AgricultureState agriculture = new AgricultureState();
        public NutritionDiversityState nutrition = new NutritionDiversityState();
    }

    public static class AgricultureSaveStore
    {
        public const string FileName = "agriculture_save.json";
        public const string SectionName = "agriculture";

        private static readonly SaveStore<AgricultureCampaignState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(AgricultureSaveStore),
            Encode,
            Decode);

        private static string Encode(AgricultureCampaignState state, IJsonSerializer json)
        {
            var envelope = new SchemaVersionedPayload
            {
                schemaVersion = 1,
                State = state,
                Checksum = SaveChecksum.Compute(json.Serialize(state))
            };
            return json.Serialize(envelope);
        }

        private static AgricultureCampaignState? Decode(string text, IJsonSerializer json)
        {
            var envelope = json.Deserialize<SchemaVersionedPayload>(text);
            if (envelope?.State != null && !string.IsNullOrEmpty(envelope.Checksum))
            {
                string actual = SaveChecksum.Compute(json.Serialize(envelope.State));
                if (!string.Equals(actual, envelope.Checksum, System.StringComparison.Ordinal))
                    return null; // corrupt — caller falls back to defaults
                return envelope.State;
            }
            // Legacy bare-state payload (pre-envelope) still loads.
            var bare = json.Deserialize<AgricultureCampaignState>(text);
            return bare;
        }

        [System.Serializable]
        private sealed class SchemaVersionedPayload
        {
            public int schemaVersion;
            public AgricultureCampaignState State = null!;
            public string Checksum = string.Empty;
        }

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(AgricultureCampaignState state) => s_store.TrySave(state);
        public static AgricultureCampaignState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(AgricultureCampaignState state) => s_store.CapturePersisted(state);
    }
}
