// SPDX-License-Identifier: MIT
// Plan 147 host integration tests: verifies NpcMemorySystem, catalog loading,
// dialogue tone derivation, trade price multipliers, save round-trips, and host wiring.

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class Plan147NpcMemoryHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string DataDir() => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        private static NpcMemorySystem CreateSystemWithCatalog()
        {
            var system = new NpcMemorySystem();
            string json = File.ReadAllText(Path.Combine(DataDir(), "npc_memory_dialogue.json"));
            system.LoadDialogueCatalog(json);
            return system;
        }

        [Fact]
        public void NpcMemory_CatalogLoading_LoadsAuthoredDialogueTemplates()
        {
            var system = CreateSystemWithCatalog();
            var templates = system.GetAllDialogueTemplates();

            Assert.NotEmpty(templates);
            Assert.True(templates.Count >= 10, $"Expected >= 10 dialogue templates, got {templates.Count}");
            Assert.Contains(templates, t => t.tone == "high_trust");
            Assert.Contains(templates, t => t.tone == "high_grudge");
            Assert.Contains(templates, t => t.tone == "favor_owed");
            Assert.Contains(templates, t => t.tone == "betrayed");
            Assert.Contains(templates, t => t.tone == "reconciled");
        }

        [Fact]
        public void NpcMemory_RecordAction_UpdatesTrustGrudgeAndFavors()
        {
            var system = CreateSystemWithCatalog();

            system.RecordAction("npc_alder", NpcMemoryActionType.Helped, day: 1);
            system.RecordAction("npc_alder", NpcMemoryActionType.SavedLife, day: 2);

            var rel = system.Get("npc_alder");
            Assert.NotNull(rel);
            Assert.Equal(50f, rel.PersonalTrust);
            Assert.Equal(25f, rel.FavorOwed);
            Assert.Equal(0f, rel.GrudgeLevel);
            Assert.Equal(2, rel.Memories.Count);
        }

        [Fact]
        public void NpcMemory_SevereGrudge_TriggersTradeEmbargo()
        {
            var system = CreateSystemWithCatalog();

            system.RecordAction("npc_moro", NpcMemoryActionType.Refused, day: 1);
            system.RecordAction("npc_moro", NpcMemoryActionType.Betrayed, day: 2);
            system.RecordAction("npc_moro", NpcMemoryActionType.Betrayed, day: 3);

            var rel = system.Get("npc_moro");
            Assert.NotNull(rel);
            Assert.True(rel.GrudgeLevel > 75f);
            Assert.True(system.IsTradeRefused("npc_moro"));
            Assert.Equal(float.PositiveInfinity, system.GetTradePriceMultiplier("npc_moro"));
            Assert.Equal(NpcDialogueTone.Betrayed, system.GetDialogueTone("npc_moro"));
        }

        [Fact]
        public void NpcMemory_HighTrust_GrantsTradeDiscount()
        {
            var system = CreateSystemWithCatalog();

            system.RecordAction("npc_sela", NpcMemoryActionType.SavedLife, day: 1);
            system.RecordAction("npc_sela", NpcMemoryActionType.GiftedFood, day: 2);

            var rel = system.Get("npc_sela");
            Assert.NotNull(rel);
            Assert.True(rel.PersonalTrust >= 50f);
            Assert.False(system.IsTradeRefused("npc_sela"));
            Assert.Equal(0.85f, system.GetTradePriceMultiplier("npc_sela"));
        }

        [Fact]
        public void NpcMemory_Forgiveness_ReducesGrudgeAndMarksMemories()
        {
            var system = CreateSystemWithCatalog();

            system.RecordAction("npc_vane", NpcMemoryActionType.Refused, day: 1);
            system.RecordAction("npc_vane", NpcMemoryActionType.Refused, day: 2);

            var rel = system.Get("npc_vane");
            Assert.NotNull(rel);
            Assert.Equal(20f, rel.GrudgeLevel);

            bool forgiven = system.Forgive("npc_vane", "restitution_settled", restitutionAmount: 20f);
            Assert.True(forgiven);
            Assert.Equal(0f, rel.GrudgeLevel);
            Assert.True(rel.Memories[0].Forgiven);
        }

        [Fact]
        public void NpcMemory_DailyDecay_DecaysGrudgeAndMemoryIntensity()
        {
            var system = CreateSystemWithCatalog();

            system.RecordAction("npc_decay_test", NpcMemoryActionType.Refused, day: 1, intensity: 80f);
            var rel = system.Get("npc_decay_test");
            Assert.NotNull(rel);
            Assert.Equal(10f, rel.GrudgeLevel);

            system.TickDailyDecay(currentDay: 5, decayRatePerDay: 2.0f);
            Assert.True(rel.GrudgeLevel < 10f);
            Assert.True(rel.Memories[0].Intensity < 80f);
        }

        [Fact]
        public void NpcMemory_SaveRestore_RoundTripPreservesState()
        {
            var system = CreateSystemWithCatalog();
            system.RecordAction("npc_save1", NpcMemoryActionType.SavedLife, day: 2, "bunker_defense", 90f);
            system.RecordAction("npc_save2", NpcMemoryActionType.Betrayed, day: 4);

            var state = system.CaptureState();
            Assert.Equal(2, state.Relationships.Count);

            var systemRestored = CreateSystemWithCatalog();
            systemRestored.RestoreState(state);

            var r1 = systemRestored.Get("npc_save1");
            var r2 = systemRestored.Get("npc_save2");
            Assert.NotNull(r1);
            Assert.NotNull(r2);
            Assert.Equal(40f, r1.PersonalTrust);
            Assert.Equal(40f, r2.GrudgeLevel);
        }

        [Fact]
        public void NpcMemory_Census_ReflectsMemoryState()
        {
            var system = CreateSystemWithCatalog();
            system.RecordAction("npc_c1", NpcMemoryActionType.SavedLife, day: 1);
            system.RecordAction("npc_c2", NpcMemoryActionType.Betrayed, day: 2);
            system.RecordAction("npc_c2", NpcMemoryActionType.Betrayed, day: 3);

            var census = system.GetCensus();
            Assert.Equal(2, census.TotalTrackedNpcs);
            Assert.Equal(3, census.TotalMemoriesRecorded);
            Assert.Equal(1, census.HighTrustNpcsCount);
            Assert.Equal(1, census.HighGrudgeNpcsCount);
            Assert.Equal(1, census.TradeEmbargoNpcsCount);
        }

        [Fact]
        public void NpcMemory_HostWiring_FilesAndHooksAreInPlace()
        {
            // Verify SaveSectionRegistry
            Assert.True(SaveSectionRegistry.TryGetSection("npc_memory", out _));
            Assert.Equal("npc_memory_save.json", SaveSectionRegistry.FileNameFor("npc_memory"));

            // Verify DayEventVocabulary
            var kind = DayEventVocabulary.GetSemanticKind("npc_memory_ticked");
            Assert.True(DayEventVocabulary.IsInternalHeartbeat("npc_memory_ticked"));

            // Verify HostCliRegistry
            var desc = HostCliRegistry.AllDescriptors.FirstOrDefault(d => d.Action == HostCliAction.NpcMemorySelfTest);
            Assert.NotNull(desc);
            Assert.Equal("--npc-memory-selftest", desc.PrimaryFlag);

            // Verify Host files exist and have required declarations
            string hostSessionCode = ReadRepoFile("src", "Host", "NpcMemoryHostSession.cs");
            Assert.Contains("class NpcMemoryHostSession", hostSessionCode);
            Assert.Contains("class NpcMemorySaveStore", hostSessionCode);

            string hostCliCode = ReadRepoFile("src", "Host", "HostCli.NpcMemory.cs");
            Assert.Contains("static class HostCliNpcMemory", hostCliCode);
            Assert.Contains("RunSelfTest", hostCliCode);

            string mainNpcMemory = ReadRepoFile("src", "Main.NpcMemory.cs");
            Assert.Contains("SetupNpcMemory", mainNpcMemory);
            Assert.Contains("SaveNpcMemory", mainNpcMemory);

            string mainSave = ReadRepoFile("src", "Main.SaveOrchestrator.cs");
            Assert.Contains("SetupNpcMemory", mainSave);
            Assert.Contains("SaveNpcMemory", mainSave);

            string mainCamp = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("NpcMemoryDayOwner", mainCamp);
        }
    }
}
