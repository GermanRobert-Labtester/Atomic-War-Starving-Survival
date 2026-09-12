// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Subterranean
{
    [Serializable]
    public class SubterraneanNodeSaveEntry
    {
        public string nodeId = string.Empty;
        public int depthTier = 1;
        public bool discovered;
        public bool blocked;
        public float structuralIntegrity = 100f;
        public int shoringLevel;
        public float oxygenLevel = 100f;
        public bool ventilationInstalled;
        public float waterLevel;
        public int lastHazardDay = -1;
    }

    /// <summary>
    /// ASHFALL — subterranean network save state (Version 1). Persists the
    /// generated topology and node state verbatim; restore never regenerates
    /// (the generated flag and network seed travel with the save). Versioned +
    /// checksummed via <see cref="SubterraneanSaveCodec"/>.
    /// </summary>
    [Serializable]
    public class SubterraneanSaveState
    {
        public int saveVersion = SubterraneanSaveCodec.CurrentSaveVersion;
        public int networkSeed;
        public bool generated;
        public List<SubterraneanNodeSaveEntry> nodes = new List<SubterraneanNodeSaveEntry>();

        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Subterranean save codec: checksum recomputed on encode, hard-reject on
    /// decode for tamper / checksumless / newer-version payloads. Old campaigns
    /// without this section load as "no network generated" — the network is
    /// generated on first underground unlock, from the campaign seed.
    /// </summary>
    public static class SubterraneanSaveCodec
    {
        public const int CurrentSaveVersion = 1;

        public static string Encode(SubterraneanSaveState state, IJsonSerializer json)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (json == null) throw new ArgumentNullException(nameof(json));
            state.saveVersion = CurrentSaveVersion;
            state.Checksum = SaveChecksum.Compute(state);
            return json.Serialize(state);
        }

        public static bool TryDecode(string json, IJsonSerializer serializer, out SubterraneanSaveState state)
        {
            state = null!;
            if (string.IsNullOrEmpty(json) || serializer == null) return false;
            try
            {
                var decoded = serializer.Deserialize<SubterraneanSaveState>(json);
                if (decoded == null) return false;
                if (decoded.saveVersion > CurrentSaveVersion) return false;
                if (decoded.saveVersion < CurrentSaveVersion) return false;

                if (string.IsNullOrEmpty(decoded.Checksum)) return false;
                if (!string.Equals(SaveChecksum.Compute(decoded), decoded.Checksum, StringComparison.Ordinal))
                    return false;

                if (decoded.nodes == null) decoded.nodes = new List<SubterraneanNodeSaveEntry>();
                state = decoded;
                return true;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<decode>", "SubterraneanSaveState", ex_CATDIAG);
                return false;
            }
        }

        public static SubterraneanSaveState ToSaveState(SubterraneanNetworkState state)
        {
            var save = new SubterraneanSaveState
            {
                networkSeed = state.networkSeed,
                generated = state.generated
            };
            foreach (var node in state.nodes)
                save.nodes.Add(new SubterraneanNodeSaveEntry
                {
                    nodeId = node.nodeId,
                    depthTier = node.depthTier,
                    discovered = node.discovered,
                    blocked = node.blocked,
                    structuralIntegrity = node.structuralIntegrity,
                    shoringLevel = node.shoringLevel,
                    oxygenLevel = node.oxygenLevel,
                    ventilationInstalled = node.ventilationInstalled,
                    waterLevel = node.waterLevel,
                    lastHazardDay = node.lastHazardDay
                });
            return save;
        }

        public static SubterraneanNetworkState FromSaveState(SubterraneanSaveState save)
        {
            var state = new SubterraneanNetworkState
            {
                networkSeed = save.networkSeed,
                generated = save.generated
            };
            foreach (var node in save.nodes)
                state.nodes.Add(new SubterraneanNodeState
                {
                    nodeId = node.nodeId,
                    depthTier = node.depthTier,
                    discovered = node.discovered,
                    blocked = node.blocked,
                    structuralIntegrity = node.structuralIntegrity,
                    shoringLevel = node.shoringLevel,
                    oxygenLevel = node.oxygenLevel,
                    ventilationInstalled = node.ventilationInstalled,
                    waterLevel = node.waterLevel,
                    lastHazardDay = node.lastHazardDay
                });
            return state;
        }
    }
}
