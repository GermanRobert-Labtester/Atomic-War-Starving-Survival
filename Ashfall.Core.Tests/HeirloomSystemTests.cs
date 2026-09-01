using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Legacy;
using Ashfall.Core.Phantoms;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class HeirloomSystemTests
    {
        private readonly string _dataDir;
        private readonly IJsonSerializer _serializer = new SystemTextJsonSerializer();

        public HeirloomSystemTests()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir))
            {
                _dataDir = dir;
            }
            else
            {
                _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            }
        }

        private HeirloomCatalog CreateLoadedCatalog()
        {
            var catalog = new HeirloomCatalog();
            string filePath = Path.Combine(_dataDir, "phantom_heirlooms.json");
            if (File.Exists(filePath))
            {
                catalog.Load(File.ReadAllText(filePath), _serializer);
            }
            return catalog;
        }

        [Fact]
        public void HeirloomCatalog_Load_ParsesHeirloomsCorrectly()
        {
            var catalog = CreateLoadedCatalog();
            Assert.True(catalog.AllHeirlooms.Count >= 12, "Expected at least 12 authored heirlooms.");

            var dosimeter = catalog.GetById("heirloom_grandfathers_dosimeter");
            Assert.NotNull(dosimeter);
            Assert.Equal("Grandfather's Dosimeter", dosimeter.title);
            Assert.Equal("dosimeter", dosimeter.base_item_id);
            Assert.True(dosimeter.is_legacy_candidate);
            Assert.True(dosimeter.memorial_eligible);
            Assert.Equal(3, dosimeter.stages.Count);
        }

        [Fact]
        public void HeirloomSystem_CreateInstance_InitializesHolderAndProvenance()
        {
            var catalog = CreateLoadedCatalog();
            var system = new HeirloomSystem(catalog);

            var inst = system.CreateInstance("heirloom_grandfathers_dosimeter", "survivor_01", 1);
            Assert.NotNull(inst);
            Assert.Equal("survivor_01", inst.current_holder_id);
            Assert.Contains(1, inst.unlocked_stages);
            Assert.Single(inst.provenance);
            Assert.Equal("survivor_01", inst.provenance[0].holder_id);
            Assert.Equal("initial_discovery", inst.provenance[0].transfer_reason);
        }

        [Fact]
        public void HeirloomSystem_TransferHeirloom_AppendsProvenance()
        {
            var catalog = CreateLoadedCatalog();
            var system = new HeirloomSystem(catalog);

            var inst = system.CreateInstance("heirloom_regiment_lighter", "survivor_01", 1);
            bool transferred = system.AssignHolder(inst.instance_id, "survivor_02", 5, "direct_trade");

            Assert.True(transferred);
            Assert.Equal("survivor_02", inst.current_holder_id);
            Assert.Equal(2, inst.provenance.Count);
            Assert.Equal("direct_trade", inst.provenance[1].transfer_reason);
        }

        [Fact]
        public void HeirloomSystem_HandleSurvivorDeath_PassesToKinWhenAvailable()
        {
            var catalog = CreateLoadedCatalog();
            var system = new HeirloomSystem(catalog);
            var succEngine = new GenerationalSuccessionEngine();
            var lineage = new GenerationalLineageExtension(succEngine);
            lineage.EstablishLineage("parent_01", "child_01", "parent");

            var inst = system.CreateInstance("heirloom_mothers_recipe_tin", "parent_01", 1);

            int handled = system.HandleSurvivorDeath(
                "parent_01",
                currentDay: 15,
                lineage: lineage,
                relations: null);

            Assert.Equal(1, handled);
            Assert.Equal("child_01", inst.current_holder_id);
            Assert.Contains(3, inst.unlocked_stages);
        }

        [Fact]
        public void HeirloomSystem_HandleSurvivorDeath_FallsBackToTrustBondWhenNoKin()
        {
            var catalog = CreateLoadedCatalog();
            var system = new HeirloomSystem(catalog);
            var rng = new SeededRng(42);
            var relations = new SurvivorRelationsSystem(rng);
            var rel = relations.GetOrCreateRelationship("survivor_01", "best_friend_02");
            rel.affinity = 80;
            rel.trust = 75;

            var inst = system.CreateInstance("heirloom_foremans_whistle", "survivor_01", 1);

            int handled = system.HandleSurvivorDeath(
                "survivor_01",
                currentDay: 20,
                lineage: null,
                relations: relations);

            Assert.Equal(1, handled);
            Assert.Equal("best_friend_02", inst.current_holder_id);
        }

        [Fact]
        public void HeirloomSystem_HandleSurvivorDeath_FallsBackToCommunalStorageWhenNoEligible()
        {
            var catalog = new HeirloomCatalog();
            var system = new HeirloomSystem(catalog);

            var inst = system.CreateInstance("heirloom_train_ticket_book", "solo_survivor", 1);

            int handled = system.HandleSurvivorDeath(
                "solo_survivor",
                currentDay: 25,
                lineage: null,
                relations: null);

            Assert.Equal(1, handled);
            Assert.Equal(string.Empty, inst.current_holder_id);
        }

        [Fact]
        public void HeirloomSystem_BoundedProvenance_CapsHistoryAtLimit()
        {
            var catalog = new HeirloomCatalog();
            var system = new HeirloomSystem(catalog);

            var inst = system.CreateInstance("heirloom_apartment_key", "holder_0", 1);

            for (int i = 1; i <= 35; i++)
            {
                system.AssignHolder(inst.instance_id, $"holder_{i}", i, "transfer");
            }

            Assert.Equal(HeirloomSystem.MaxProvenanceEntriesPerInstance, inst.provenance.Count);
            Assert.Equal(24, inst.provenance.Count);
            Assert.Equal("holder_35", inst.current_holder_id);
        }

        [Fact]
        public void HeirloomSystem_TriggerHolderMemory_ReturnsMatchingReaction()
        {
            var catalog = new HeirloomCatalog();
            string filePath = Path.Combine(_dataDir, "phantom_heirlooms.json");
            if (File.Exists(filePath))
            {
                catalog.Load(File.ReadAllText(filePath), _serializer);
                var system = new HeirloomSystem(catalog);
                var inst = system.CreateInstance("heirloom_grandfathers_dosimeter", "doc_01", 1);

                var sv = new PhantomSurvivorSnapshot
                {
                    survivorId = "doc_01",
                    displayName = "Elena",
                    backgroundId = "nurse",
                    traitIds = new List<string> { "medic" },
                    isAlive = true
                };

                var (text, moraleDelta, guiltDelta) = system.ResolveHolderMemory(inst.instance_id, sv);

                Assert.NotNull(text);
                Assert.Contains("Elena", text);
                Assert.True(moraleDelta > 0);
            }
        }

        [Fact]
        public void HeirloomSystem_CaptureRestoreState_PreservesData()
        {
            var catalog = new HeirloomCatalog();
            var systemA = new HeirloomSystem(catalog);

            var inst = systemA.CreateInstance("heirloom_midwifes_satchel", "nurse_mary", 1);
            systemA.AssignHolder(inst.instance_id, "nurse_sarah", 10, "handoff");
            systemA.SetLegacySelected(inst.instance_id, true);
            systemA.SetMemorialized(inst.instance_id, true);

            var state = systemA.CaptureState();

            var systemB = new HeirloomSystem(catalog);
            systemB.RestoreState(state);

            var restoredInst = systemB.GetInstance(inst.instance_id);
            Assert.NotNull(restoredInst);
            Assert.Equal("nurse_sarah", restoredInst.current_holder_id);
            Assert.Equal(2, restoredInst.provenance.Count);
            Assert.True(restoredInst.is_legacy_selected);
            Assert.True(restoredInst.is_memorialized);
        }
    }
}
