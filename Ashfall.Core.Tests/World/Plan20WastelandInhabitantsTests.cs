using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class Plan20WastelandInhabitantsTests
    {
        private readonly string _dataDir;
        private readonly IFileIO _fileIO;

        public Plan20WastelandInhabitantsTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
        }

        [Fact]
        public void FieldGuide_Loads38Entries_20Fauna_12Flora_6Ecology()
        {
            var catalog = FieldGuideCatalog.LoadFromDirectory(_dataDir, _fileIO);
            Assert.Equal(38, catalog.Count);

            var fauna = catalog.GetEntriesByCategory("Fauna");
            var flora = catalog.GetEntriesByCategory("Flora");
            var ecology = catalog.GetEntriesByCategory("Ecology");

            Assert.Equal(20, fauna.Count);
            Assert.Equal(12, flora.Count);
            Assert.Equal(6, ecology.Count);

            // Verify specific entries
            Assert.True(catalog.TryGetEntry("field_fauna_two_headed_wolf", out var wolf));
            Assert.Equal("Two-Headed Steppe Wolf", wolf.CommonName);
            Assert.Equal(3, wolf.ThreatLevel);
            Assert.Contains("predator", wolf.Tags);

            Assert.True(catalog.TryGetEntry("field_flora_glowing_rad_rye", out var rye));
            Assert.Equal("Glowing Rad Rye", rye.CommonName);
            Assert.Equal("Flora", rye.Category);
            Assert.Contains("grain", rye.Tags);
        }

        [Fact]
        public void FieldGuide_UnlockState_CaptureRestore_Roundtrips()
        {
            var catalog = FieldGuideCatalog.LoadFromDirectory(_dataDir, _fileIO);
            Assert.Equal(0, catalog.UnlockedCount);

            Assert.True(catalog.UnlockEntry("field_fauna_two_headed_wolf"));
            Assert.True(catalog.UnlockEntry("field_flora_nitrogen_mushroom"));
            Assert.Equal(2, catalog.UnlockedCount);
            Assert.True(catalog.IsUnlocked("field_fauna_two_headed_wolf"));
            Assert.True(catalog.IsUnlocked("field_flora_nitrogen_mushroom"));
            Assert.False(catalog.IsUnlocked("field_fauna_cave_bear"));

            var state = catalog.CaptureState();
            Assert.Equal(2, state.UnlockedEntryIds.Count);

            var newCatalog = FieldGuideCatalog.LoadFromDirectory(_dataDir, _fileIO);
            newCatalog.RestoreState(state);
            Assert.Equal(2, newCatalog.UnlockedCount);
            Assert.True(newCatalog.IsUnlocked("field_fauna_two_headed_wolf"));
            Assert.True(newCatalog.IsUnlocked("field_flora_nitrogen_mushroom"));
        }

        [Fact]
        public void Settlements_Loads6Settlements_18Npcs_6Quests()
        {
            var catalog = SettlementCatalog.LoadFromDirectory(_dataDir, _fileIO);
            Assert.Equal(12, catalog.SettlementCount);
            Assert.Equal(18, catalog.NpcCount);
            Assert.Equal(6, catalog.QuestCount);

            // Verify settlements
            Assert.True(catalog.TryGetSettlement("settlement_brine_pans", out var brine));
            Assert.Equal("Brine-Pan Hollow", brine.DisplayName);
            Assert.Equal("Salt Camp", brine.Archetype);
            Assert.Equal("the_toll", brine.Region);
            Assert.Equal("npc_salt_marshal_varn", brine.KeeperNpcId);
            Assert.Equal("npc_salt_trader_elena", brine.TraderNpcId);
            Assert.Equal("npc_salt_boiler_petyr", brine.FixtureNpcId);

            Assert.True(catalog.TryGetSettlement("settlement_iron_siding", out var siding));
            Assert.Equal("Iron Siding", siding.DisplayName);
            Assert.Equal("Rail Siding Town", siding.Archetype);

            Assert.True(catalog.TryGetSettlement("settlement_cape_beacon", out var cape));
            Assert.Equal("Cape Beacon Commune", cape.DisplayName);

            Assert.True(catalog.TryGetSettlement("settlement_slate_hollow", out var slate));
            Assert.Equal("Slate Hollow Enclave", slate.DisplayName);

            Assert.True(catalog.TryGetSettlement("settlement_pilgrim_hearth", out var pilgrim));
            Assert.Equal("The Pilgrim's Hearth", pilgrim.DisplayName);

            Assert.True(catalog.TryGetSettlement("settlement_tinkers_notch", out var tinker));
            Assert.Equal("Tinker's Notch", tinker.DisplayName);
        }

        [Fact]
        public void Settlements_NpcGreetings_ReactToStanding()
        {
            var catalog = SettlementCatalog.LoadFromDirectory(_dataDir, _fileIO);
            string npcId = "npc_salt_marshal_varn";

            string lowGreeting = catalog.GetNpcGreeting(npcId, -30f);
            string neutralGreeting = catalog.GetNpcGreeting(npcId, 0f);
            string highGreeting = catalog.GetNpcGreeting(npcId, 50f);

            Assert.Contains("State your business", lowGreeting);
            Assert.Contains("Welcome to Brine-Pan Hollow", neutralGreeting);
            Assert.Contains("Good to see you", highGreeting);
        }

        [Fact]
        public void Settlements_RepeatableQuests_CooldownAndCompletion()
        {
            var catalog = SettlementCatalog.LoadFromDirectory(_dataDir, _fileIO);
            string questId = "quest_repeat_salt_boiler_scum";

            Assert.True(catalog.TryGetQuest(questId, out var quest));
            Assert.Equal("Boiler Descaling Haul", quest.DisplayName);
            Assert.Equal(7, quest.CooldownDays);

            Assert.True(catalog.IsQuestAvailable(questId, 10));
            catalog.CompleteQuest(questId, 10);
            Assert.Equal(1, catalog.GetCompletedQuestCount(questId));

            // Not available during cooldown
            Assert.False(catalog.IsQuestAvailable(questId, 11));
            Assert.False(catalog.IsQuestAvailable(questId, 16));

            // Available on or after cooldown
            Assert.True(catalog.IsQuestAvailable(questId, 17));
            Assert.True(catalog.IsQuestAvailable(questId, 20));

            // State capture/restore
            var state = catalog.CaptureState();
            var newCatalog = SettlementCatalog.LoadFromDirectory(_dataDir, _fileIO);
            newCatalog.RestoreState(state);

            Assert.False(newCatalog.IsQuestAvailable(questId, 15));
            Assert.True(newCatalog.IsQuestAvailable(questId, 17));
            Assert.Equal(1, newCatalog.GetCompletedQuestCount(questId));
        }

        [Fact]
        public void TravelEncounters_Loads24Encounters_And4Chains()
        {
            var catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
            Assert.True(catalog.Count >= 28); // 24 base + 12 chain stages (or 36 total)

            Assert.True(catalog.TryGetEncounter("enc_travel_wolf_pack_crossing", out var wolfEnc));
            Assert.Equal("Two-Headed Wolf Pack Scent Line", wolfEnc.Title);
            Assert.Equal("Creature", wolfEnc.Category);
            Assert.Equal(3, wolfEnc.Choices.Count);

            Assert.True(catalog.TryGetEncounter("enc_chain_pilgrim_stage1", out var p1));
            Assert.Equal("chain_wandering_pilgrim", p1.ChainId);
            Assert.Equal(1, p1.ChainStage);
            Assert.Equal(0, p1.PrereqChainStage);
        }

        [Fact]
        public void TravelEncounterSystem_DeterministicSelection_SameSeedSameResult()
        {
            var catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
            var sys = new TravelEncounterSystem(catalog);

            var rng1 = new SeededRng(12345);
            var rng2 = new SeededRng(12345);

            var enc1 = sys.SelectEncounter("the_toll", 2.0f, "Balanced", "all", 1, rng1);
            var enc2 = sys.SelectEncounter("the_toll", 2.0f, "Balanced", "all", 1, rng2);

            Assert.NotNull(enc1);
            Assert.NotNull(enc2);
            Assert.Equal(enc1!.Id, enc2!.Id);
        }

        [Fact]
        public void TravelEncounterSystem_ChainProgression_AdvancesStages()
        {
            var catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
            var sys = new TravelEncounterSystem(catalog);
            string chainId = "chain_wandering_pilgrim";

            Assert.Equal(0, sys.GetChainStage(chainId));

            // Stage 1 is eligible
            Assert.True(catalog.TryGetEncounter("enc_chain_pilgrim_stage1", out var s1));
            Assert.True(sys.IsEncounterEligible(s1, "high_scarp", 1.5f, "all", 1));

            // Stage 2 is NOT eligible yet
            Assert.True(catalog.TryGetEncounter("enc_chain_pilgrim_stage2", out var s2));
            Assert.False(sys.IsEncounterEligible(s2, "high_scarp", 1.5f, "all", 1));

            // Resolve stage 1 choice to advance chain
            bool resolved = sys.ResolveChoice("enc_chain_pilgrim_stage1", "choice_give_wood_and_water", 1, out int morale, out int guilt, out string fieldGuideId);
            Assert.True(resolved);
            Assert.Equal(4, morale);
            Assert.Equal(0, guilt);
            Assert.Equal(2, sys.GetChainStage(chainId));

            // Now Stage 2 IS eligible
            Assert.True(sys.IsEncounterEligible(s2, "high_scarp", 1.5f, "all", 1));

            // Stage 1 is no longer eligible (prereq was 0)
            Assert.False(sys.IsEncounterEligible(s1, "high_scarp", 1.5f, "all", 1));
        }

        [Fact]
        public void TravelEncounterSystem_ChoiceResolution_UnlocksFieldGuide()
        {
            var catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
            var fieldGuide = FieldGuideCatalog.LoadFromDirectory(_dataDir, _fileIO);
            var sys = new TravelEncounterSystem(catalog);

            bool resolved = sys.ResolveChoice("enc_travel_wolf_pack_crossing", "choice_throw_flare", 1, out int morale, out int guilt, out string fieldGuideId);
            Assert.True(resolved);
            Assert.Equal(2, morale);
            Assert.Equal("field_fauna_two_headed_wolf", fieldGuideId);

            Assert.True(fieldGuide.UnlockEntry(fieldGuideId));
            Assert.True(fieldGuide.IsUnlocked("field_fauna_two_headed_wolf"));
        }
    }
}
