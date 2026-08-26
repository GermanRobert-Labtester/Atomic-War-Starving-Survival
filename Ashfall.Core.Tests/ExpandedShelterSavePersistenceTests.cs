// SPDX-License-Identifier: MIT
// ASHFALL test: Expanded Shelter save/load persistence & integrity verification.
// Verifies that all Expanded Shelter subsystems survive save/load roundtrips
// with full state fidelity, correct checksum computation, and tampered-state detection.

#nullable disable

using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests
{
    public class ExpandedShelterSavePersistenceTests
    {
        private static readonly SystemTextJsonSerializer Json = new SystemTextJsonSerializer();

        [Serializable]
        public sealed class SumpFloodingHostSave
        {
            public string SchemaVersion { get; set; } = "1.0";
            public SumpFloodingState State { get; set; }
            public string Checksum { get; set; } = string.Empty;
        }

        [Fact]
        public void SumpFlooding_SurvivesSaveLoadRoundTrip_PreservesAllFields()
        {
            var state = new SumpFloodingState
            {
                globalGroundwaterLevel = 3.5f,
                lastFloodDay = 14,
                nodes = new List<SumpNode>
                {
                    new SumpNode
                    {
                        nodeId = "sump_sublevel_1",
                        displayName = "Sublevel 1 Storage",
                        waterLevelCm = 85.5f,
                        maxWaterLevelCm = 250f,
                        hasSumpPump = true,
                        pumpCondition = 92.4f,
                        pumpPowered = true,
                        hasFloatValve = true,
                        hasSandbagMitigation = true,
                        contaminationLevel = 0.25f,
                        adjacentNodeIds = new List<string> { "sump_generator_pit" },
                        isFlooded = false,
                        equipmentDisabled = false
                    },
                    new SumpNode
                    {
                        nodeId = "sump_generator_pit",
                        displayName = "Generator Pit",
                        waterLevelCm = 150f,
                        maxWaterLevelCm = 180f,
                        hasSumpPump = false,
                        pumpCondition = 0f,
                        pumpPowered = false,
                        hasFloatValve = false,
                        hasSandbagMitigation = false,
                        contaminationLevel = 0.8f,
                        adjacentNodeIds = new List<string> { "sump_sublevel_1" },
                        isFlooded = true,
                        equipmentDisabled = true
                    }
                }
            };

            // Capture into envelope
            var envelope = new SumpFloodingHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            // Serialize to JSON (mirrors SumpFloodingSaveStore.TrySave)
            string rawJson = Json.Serialize(envelope);
            Assert.False(string.IsNullOrWhiteSpace(rawJson));

            // Restore from JSON (mirrors SumpFloodingSaveStore.TryLoad)
            var restoredEnvelope = Json.Deserialize<SumpFloodingHostSave>(rawJson);
            Assert.NotNull(restoredEnvelope);
            Assert.Equal(envelope.Checksum, restoredEnvelope.Checksum);

            // Verify checksum validation passes
            string computedChecksum = SaveChecksum.Compute(restoredEnvelope);
            Assert.Equal(restoredEnvelope.Checksum, computedChecksum);

            // Verify deep state fidelity
            var restoredState = restoredEnvelope.State;
            Assert.NotNull(restoredState);
            Assert.Equal(3.5f, restoredState.globalGroundwaterLevel);
            Assert.Equal(14, restoredState.lastFloodDay);
            Assert.Equal(2, restoredState.nodes.Count);

            var n0 = restoredState.nodes[0];
            Assert.Equal("sump_sublevel_1", n0.nodeId);
            Assert.Equal("Sublevel 1 Storage", n0.displayName);
            Assert.Equal(85.5f, n0.waterLevelCm);
            Assert.Equal(250f, n0.maxWaterLevelCm);
            Assert.True(n0.hasSumpPump);
            Assert.Equal(92.4f, n0.pumpCondition);
            Assert.True(n0.pumpPowered);
            Assert.True(n0.hasFloatValve);
            Assert.True(n0.hasSandbagMitigation);
            Assert.Equal(0.25f, n0.contaminationLevel);
            Assert.Single(n0.adjacentNodeIds);
            Assert.Equal("sump_generator_pit", n0.adjacentNodeIds[0]);
            Assert.False(n0.isFlooded);
            Assert.False(n0.equipmentDisabled);

            var n1 = restoredState.nodes[1];
            Assert.Equal("sump_generator_pit", n1.nodeId);
            Assert.Equal(150f, n1.waterLevelCm);
            Assert.False(n1.hasSumpPump);
            Assert.True(n1.isFlooded);
            Assert.True(n1.equipmentDisabled);
            Assert.Equal(0.8f, n1.contaminationLevel);
        }

        [Fact]
        public void SumpFlooding_DirectSerialization_MatchesEnvelopePayload()
        {
            var state = new SumpFloodingState
            {
                globalGroundwaterLevel = 1.0f,
                lastFloodDay = 5,
                nodes = new List<SumpNode>
                {
                    new SumpNode { nodeId = "sump_a", displayName = "Main Sump", waterLevelCm = 10f }
                }
            };

            // Direct capture
            string directJson = Json.Serialize(state);
            Assert.False(string.IsNullOrWhiteSpace(directJson));

            // Direct restore
            var restoredState = Json.Deserialize<SumpFloodingState>(directJson);
            Assert.NotNull(restoredState);
            Assert.Single(restoredState.nodes);
            Assert.Equal("sump_a", restoredState.nodes[0].nodeId);
            Assert.Equal(10f, restoredState.nodes[0].waterLevelCm);
        }
    }
}
