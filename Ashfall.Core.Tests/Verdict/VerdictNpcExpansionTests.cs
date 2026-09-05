using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests.Verdict
{
    public class VerdictNpcExpansionTests : CatalogTestBase
    {
        private static VerdictNpcSystem LoadSystem()
        {
            var system = new VerdictNpcSystem();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            int count = VerdictNpcCatalogLoader.LoadAndRegister(system, DataDirectory, files, json);
            Assert.True(count >= 15, $"Expected at least 15 NPCs registered, got {count}");
            return system;
        }

        [Fact]
        public void Catalog_Loads_All_18_Npc_Entries()
        {
            var system = LoadSystem();
            Assert.Equal(18, system.Catalog.Count);
            Assert.True(system.Catalog.Count >= 15);
        }

        [Fact]
        public void All_18_Npc_Ids_Are_Unique_And_Prefixed()
        {
            var system = LoadSystem();
            var ids = system.Catalog.Select(e => e.id).ToList();
            var distinct = ids.Distinct(StringComparer.Ordinal).ToList();
            Assert.Equal(ids.Count, distinct.Count);
            foreach (var id in ids)
            {
                Assert.True(id.StartsWith("npc_"), $"NPC id '{id}' must start with npc_ prefix");
            }
        }

        [Fact]
        public void Original_6_Baseline_Npcs_Preserved()
        {
            var system = LoadSystem();
            var baselineIds = new[]
            {
                "npc_eden_vale",
                "npc_ferris_voss",
                "npc_iran_bell",
                "npc_selya_saltmarsh",
                "npc_maro_veen",
                "npc_whisper_cipher"
            };

            foreach (var bId in baselineIds)
            {
                var npc = system.Find(bId);
                Assert.NotNull(npc);
                Assert.False(string.IsNullOrWhiteSpace(npc.name));
                Assert.False(string.IsNullOrWhiteSpace(npc.role));
                Assert.NotEmpty(npc.dialogue);
            }
        }

        [Fact]
        public void Plan18_Tribunal_Npcs_Preserved()
        {
            var system = LoadSystem();
            var plan18Ids = new[]
            {
                "npc_tomas_reid",
                "npc_elena_vane",
                "npc_kasper_holt"
            };

            foreach (var pId in plan18Ids)
            {
                var npc = system.Find(pId);
                Assert.NotNull(npc);
                Assert.False(string.IsNullOrWhiteSpace(npc.name));
                Assert.False(string.IsNullOrWhiteSpace(npc.role));
                Assert.NotEmpty(npc.dialogue);
            }
        }

        [Fact]
        public void All_9_Plan93_Investigation_Npcs_Present()
        {
            var system = LoadSystem();
            var plan93Ids = new[]
            {
                "npc_mara_elsen",
                "npc_ilya_venn",
                "npc_garrick_daal",
                "npc_sena_korr",
                "npc_torin_rask",
                "npc_oren_varek",
                "npc_lena_rost",
                "npc_tessa_mirn",
                "npc_karel_norn"
            };

            foreach (var id in plan93Ids)
            {
                var npc = system.Find(id);
                Assert.NotNull(npc);
                Assert.False(string.IsNullOrWhiteSpace(npc.name));
                Assert.False(string.IsNullOrWhiteSpace(npc.role));
                Assert.False(string.IsNullOrWhiteSpace(npc.gatingFlag));
                Assert.False(string.IsNullOrWhiteSpace(npc.locationId));
                Assert.InRange(npc.phaseMin, 1, 3);
                Assert.InRange(npc.dialogue.Count, 2, 4);
                foreach (var line in npc.dialogue)
                {
                    Assert.False(string.IsNullOrWhiteSpace(line));
                }
            }
        }

        [Fact]
        public void All_Npc_Kinds_Are_Supported()
        {
            var system = LoadSystem();
            var validKinds = new HashSet<string>(StringComparer.Ordinal)
            {
                "paper_ghost",
                "tape_echo",
                "living",
                "readings"
            };

            foreach (var npc in system.Catalog)
            {
                Assert.Contains(npc.kind, validKinds);
            }
        }

        [Fact]
        public void All_Plan93_LocationIds_Map_To_Distinct_Verdict_Sites()
        {
            var system = LoadSystem();
            var expectedSiteMappings = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["npc_mara_elsen"] = "loc_abandoned_tide_gauge",
                ["npc_ilya_venn"] = "loc_coastal_meteorological_station",
                ["npc_garrick_daal"] = "loc_clifftop_observation_bunker",
                ["npc_sena_korr"] = "loc_sealed_marine_laboratory",
                ["npc_torin_rask"] = "loc_forestry_survey_post",
                ["npc_oren_varek"] = "loc_geological_core_vault",
                ["npc_lena_rost"] = "loc_river_gauging_station",
                ["npc_tessa_mirn"] = "loc_abandoned_agricultural_station",
                ["npc_karel_norn"] = "loc_decommissioned_signal_relay"
            };

            foreach (var kvp in expectedSiteMappings)
            {
                var npc = system.Find(kvp.Key);
                Assert.NotNull(npc);
                Assert.Equal(kvp.Value, npc.locationId);
            }
        }

        [Fact]
        public void GetAvailable_Filters_By_Phase_And_Flag_And_Location()
        {
            var system = LoadSystem();
            const string npcId = "npc_garrick_daal";
            var npc = system.Find(npcId);
            Assert.NotNull(npc);
            Assert.Equal(2, npc.phaseMin);
            Assert.Equal("flag_verdict_cliff_signal_decoded", npc.gatingFlag);
            Assert.Equal("loc_clifftop_observation_bunker", npc.locationId);

            var flags = new[] { "flag_verdict_cliff_signal_decoded" };

            // Phase 1 -> hidden (requires phase 2)
            var p1 = system.GetAvailable(flags, 1, npc.locationId);
            Assert.DoesNotContain(p1, e => e.id == npcId);

            // Phase 2, flag missing -> hidden
            var noFlag = system.GetAvailable(Array.Empty<string>(), 2, npc.locationId);
            Assert.DoesNotContain(noFlag, e => e.id == npcId);

            // Phase 2, flag present, wrong location -> hidden
            var wrongLoc = system.GetAvailable(flags, 2, "loc_abandoned_tide_gauge");
            Assert.DoesNotContain(wrongLoc, e => e.id == npcId);

            // Phase 2, flag present, right location -> visible
            var valid = system.GetAvailable(flags, 2, npc.locationId);
            Assert.Contains(valid, e => e.id == npcId);

            // Phase 3, flag present, right location -> visible
            var p3 = system.GetAvailable(flags, 3, npc.locationId);
            Assert.Contains(p3, e => e.id == npcId);
        }

        [Fact]
        public void Speak_Is_OneShot_And_Persists_In_State()
        {
            var system = LoadSystem();
            const string npcId = "npc_mara_elsen";

            // Speak at correct location
            bool first = system.Speak(npcId, "loc_abandoned_tide_gauge");
            Assert.True(first);

            // Speak second time -> false (one-shot)
            bool second = system.Speak(npcId, "loc_abandoned_tide_gauge");
            Assert.False(second);

            // Round-trip state
            var state = system.CaptureState();
            Assert.Contains(npcId, state.spokenNpcIds);

            var newSystem = LoadSystem();
            newSystem.RestoreState(state);
            Assert.False(newSystem.Speak(npcId, "loc_abandoned_tide_gauge"));
        }

        [Fact]
        public void Availability_Is_Deterministic_Across_Invocations()
        {
            var system = LoadSystem();
            var allFlags = system.Catalog.Select(e => e.gatingFlag).Where(f => !string.IsNullOrEmpty(f)).ToList();

            var run1 = system.GetAvailable(allFlags, 3);
            var run2 = system.GetAvailable(allFlags, 3);

            Assert.Equal(run1.Count, run2.Count);
            for (int i = 0; i < run1.Count; i++)
            {
                Assert.Equal(run1[i].id, run2[i].id);
            }
        }
    }
}
