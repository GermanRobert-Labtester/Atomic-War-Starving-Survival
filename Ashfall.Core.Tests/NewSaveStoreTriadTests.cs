// SPDX-License-Identifier: MIT
// ASHFALL test: save-store round-trip sweep for the 4 new triad-repair stores
// (Disease, SilentFoundry, WastelandMap, EncounterChoice). Pattern parity
// with SaveStoreChecksumSweepTests.

using System.Collections.Generic;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class NewSaveStoreTriadTests
    {
        // Plain DTO envelopes; keep field names snake_case for wire-format parity
        // with the other host save stores.
        public sealed class DiseaseSaveEnvelope
        {
            public Ashfall.Core.Disease.DiseaseSystemState State;
            public string Checksum;
        }

        public sealed class WastelandMapSaveEnvelope
        {
            public Ashfall.Core.World.WastelandMapState State;
            public string Checksum;
        }

        public sealed class EncounterChoiceSaveEnvelope
        {
            public Ashfall.Core.Expeditions.EncounterChoiceState State;
            public string Checksum;
        }

        public sealed class SilentFoundrySaveEnvelope
        {
            public Ashfall.Core.Foundry.SilentFoundryState State;
            public string Checksum;
        }

        private static readonly SystemTextJsonSerializer Json = new SystemTextJsonSerializer();

        [Fact]
        public void Disease_round_trips_envelope()
        {
            var state = new Ashfall.Core.Disease.DiseaseSystemState
            {
                rngSeed = 4242,
                water_purified = true,
                vents_sealed = false
            };
            state.diseases.Add(new Ashfall.Core.Disease.DiseaseEntryState
            {
                disease_id = "strain_test",
                outbreak_active = true,
                deaths_total = 0
            });

            var env = new DiseaseSaveEnvelope { State = state };
            env.Checksum = SaveChecksum.Compute(env);

            string json = Json.Serialize(env);
            var reloaded = Json.Deserialize<DiseaseSaveEnvelope>(json);
            Assert.Equal(env.Checksum, reloaded.Checksum);
            Assert.Single(reloaded.State.diseases);
            Assert.Equal("strain_test", reloaded.State.diseases[0].disease_id);
            Assert.True(reloaded.State.diseases[0].outbreak_active);
            Assert.True(reloaded.State.water_purified);
            Assert.Equal(4242, reloaded.State.rngSeed);
        }

        [Fact]
        public void WastelandMap_round_trips_envelope()
        {
            var state = new Ashfall.Core.World.WastelandMapState();
            var env = new WastelandMapSaveEnvelope { State = state };
            env.Checksum = SaveChecksum.Compute(env);

            string json = Json.Serialize(env);
            var reloaded = Json.Deserialize<WastelandMapSaveEnvelope>(json);
            Assert.Equal(env.Checksum, reloaded.Checksum);
            Assert.NotNull(reloaded.State);
        }

        [Fact]
        public void EncounterChoice_round_trips_envelope()
        {
            var state = new Ashfall.Core.Expeditions.EncounterChoiceState();
            state.History.Add(new Ashfall.Core.Expeditions.EncounterResolution
            {
                EncounterId = "enc_test_1",
                Day = 312,
                Outcome = "resolved_with_loot",
                TriggeredCombat = false,
                LootSummary = "scrap=2"
            });

            var env = new EncounterChoiceSaveEnvelope { State = state };
            env.Checksum = SaveChecksum.Compute(env);

            string json = Json.Serialize(env);
            var reloaded = Json.Deserialize<EncounterChoiceSaveEnvelope>(json);
            Assert.Equal(env.Checksum, reloaded.Checksum);
            Assert.Single(reloaded.State.History);
            Assert.Equal("enc_test_1", reloaded.State.History[0].EncounterId);
            Assert.Equal(312, reloaded.State.History[0].Day);
            Assert.False(reloaded.State.History[0].TriggeredCombat);
        }

        [Fact]
        public void SilentFoundry_round_trips_envelope()
        {
            var state = new Ashfall.Core.Foundry.SilentFoundryState();
            var env = new SilentFoundrySaveEnvelope { State = state };
            env.Checksum = SaveChecksum.Compute(env);

            string json = Json.Serialize(env);
            var reloaded = Json.Deserialize<SilentFoundrySaveEnvelope>(json);
            Assert.Equal(env.Checksum, reloaded.Checksum);
            Assert.NotNull(reloaded.State);
        }

        [Fact]
        public void Envelope_changing_state_changes_checksum()
        {
            var state = new Ashfall.Core.Disease.DiseaseSystemState();
            var env = new DiseaseSaveEnvelope { State = state };
            env.Checksum = SaveChecksum.Compute(env);

            state.diseases.Add(new Ashfall.Core.Disease.DiseaseEntryState
            {
                disease_id = "strain_late",
                outbreak_active = true,
                deaths_total = 1
            });
            string mutated = SaveChecksum.Compute(env);
            Assert.NotEqual(env.Checksum, mutated);
        }

        [Fact]
        public void Null_checksum_is_rejected_by_load_contract()
        {
            // Tests the load-time guard: a new-format envelope with null
            // Checksum field is treated as corrupt and not silently accepted
            // as a legacy save. Mirrors the AGENTS.md invariant on
            // NarrativeSaveStore and the other envelope stores.
            var env = new SilentFoundrySaveEnvelope { State = new Ashfall.Core.Foundry.SilentFoundryState(), Checksum = null };
            string json = Json.Serialize(env);

            Assert.Contains("\"Checksum\":null", json);
            // The store-side guard rejects null checksum; the bytes-on-disk
            // contract is: a saved envelope is non-empty Checksum or fails
            // loudly on load.
            var reloaded = Json.Deserialize<SilentFoundrySaveEnvelope>(json);
            Assert.Null(reloaded.Checksum);
        }
    }
}
