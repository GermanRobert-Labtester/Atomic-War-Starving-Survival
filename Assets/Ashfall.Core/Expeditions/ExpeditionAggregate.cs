using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Save;
#pragma warning disable CS8618

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Aggregate expedition save payload (section "expedition"): active
    /// expeditions plus the vehicle garage. Prior formats — a bare
    /// List&lt;ExpeditionState&gt; and the { State: [...] } envelope — are
    /// migrated by the host store's decode.
    /// </summary>
    [Serializable]
    public sealed class ExpeditionAggregateState
    {
        public string systemId = "expedition_aggregate";
        public List<ExpeditionState> expeditions = new List<ExpeditionState>();
        public ExpeditionVehicleState vehicles = new ExpeditionVehicleState();

        /// <summary>F4 — discovered expedition-destination IDs. Null on legacy
        /// aggregates (restore then reconstructs from the narrative resolution
        /// history); a present list (even empty) is authoritative.</summary>
        public List<string>? knownLocationIds = new List<string>();

        /// <summary>Lifetime successful returns. Defaults to 0 on legacy aggregates.</summary>
        public int completedCount;
    }

    /// <summary>
    /// Loads the vehicles.json catalog from the data authority. A missing file
    /// yields an empty catalog — expeditions simply stay on foot.
    /// </summary>
    public static class VehicleCatalogLoader
    {
        public const string FileName = "vehicles.json";

        public static VehicleCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(dataDir) || files == null || json == null)
                return new VehicleCatalog();

            try
            {
                string path = files.Combine(dataDir, FileName);
                if (!files.FileExists(path))
                    return new VehicleCatalog();
                string raw = files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw))
                    return new VehicleCatalog();
                return json.Deserialize<VehicleCatalog>(raw) ?? new VehicleCatalog();
            }
            catch
            {
                // A corrupt catalog must never block the game — empty garage.
                return new VehicleCatalog();
            }
        }

        /// <summary>Convenience for hosts holding a data directory string.</summary>
        public static VehicleCatalog Load(string dataDir)
            => Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
    }

    /// <summary>
    /// Codec for the expedition aggregate section payload: the current
    /// checksummed envelope plus migration from the two legacy shapes (the
    /// pre-aggregate { State: [...] } envelope and the older bare list).
    /// Payload bytes for live aggregates are the standard checksummed
    /// envelope, so integrity guarantees match every other section.
    /// </summary>
    public static class ExpeditionAggregateCodec
    {
        public static string Encode(ExpeditionAggregateState aggregate, IJsonSerializer json)
            => SaveEnvelopeHelper.CaptureEnvelope(aggregate, json);

        public static ExpeditionAggregateState? Decode(string raw, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;

            var (ok, aggregate, _) = SaveEnvelopeHelper.RestoreEnvelope<ExpeditionAggregateState>(raw, json);
            if (ok) return aggregate;

            // Not the current aggregate. Route by payload shape — no probing
            // catches: malformed JSON propagates to the store service's
            // logging catch, which rejects the save exactly as before.
            string trimmed = raw.TrimStart();
            if (trimmed.StartsWith("[", StringComparison.Ordinal))
            {
                // Legacy shape 2: the bare expedition list (pre-checksum store).
                var bare = json.Deserialize<List<ExpeditionState>>(raw);
                return bare != null ? new ExpeditionAggregateState { expeditions = bare } : null;
            }

            if (trimmed.StartsWith("{", StringComparison.Ordinal))
            {
                // Legacy shape 1: the pre-aggregate checksummed envelope whose
                // State is the bare expedition list. An object that is neither
                // shape (or fails integrity) decodes to null — rejected.
                var legacy = json.Deserialize<SaveEnvelope<List<ExpeditionState>>>(raw);
                if (legacy?.State != null)
                    return new ExpeditionAggregateState { expeditions = legacy.State };
                return null;
            }

            return null;
        }
    }
}
