// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Legacy;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan217GenealogyIntegrationTests
    {
        private const string SampleFamilyCatalog = @"{
  ""schema_version"": 1,
  ""cultural_archetypes"": [
    {
      ""archetype"": ""old_world"",
      ""prefixes"": [""Von"", ""De""],
      ""roots"": [""Vance"", ""Sterling""],
      ""suffixes"": [""field"", ""wood""]
    }
  ],
  ""templates"": [
    ""Vance"",
    ""Sterling"",
    ""Rustborn""
  ]
}";

        [Fact]
        public void LoadFamilyNameCatalog_LoadsTemplatesAndAllowsGeneration()
        {
            var engine = new GenerationalSuccessionEngine();
            var lineage = new GenerationalLineageExtension(engine);

            lineage.LoadFamilyNameCatalog(SampleFamilyCatalog);

            var rng = new SeededRng(42);
            string generated = lineage.GenerateFamilyName(rng);
            Assert.Contains(generated, new[] { "Vance", "Sterling", "Rustborn" });
        }

        [Fact]
        public void GenealogyBridge_OnChildBorn_EstablishesLineage_AndInheritsFamilyName()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("father_1", 35);
            var lineage = new GenerationalLineageExtension(engine);
            lineage.AssignFamilyName("father_1", "Blackwood");
            var bridge = new GenealogyBridge(lineage);

            string? loggedEvent = null;
            bridge.OnGenealogyEventLogged += (ev, id, desc) => loggedEvent = ev;

            var result = bridge.OnChildBorn("father_1", "daughter_1", day: 10);

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal("birth", loggedEvent);
            Assert.Equal("Blackwood", lineage.GetFamilyName("daughter_1"));

            var members = bridge.GetFamilyMembers("father_1");
            Assert.Contains("daughter_1", members);
            Assert.Contains("father_1", members);
        }

        [Fact]
        public void GenealogyBridge_OnMarriage_SetsMutualSpouses_AndFormsFamilyUnit()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("groom_1", 28);
            engine.RegisterDweller("bride_1", 26);
            var lineage = new GenerationalLineageExtension(engine);
            lineage.AssignFamilyName("groom_1", "Montgomery");
            var bridge = new GenealogyBridge(lineage);

            bool ok = bridge.OnMarriage("groom_1", "bride_1", day: 15);

            Assert.True(ok);
            Assert.Equal("bride_1", lineage.GetSpouse("groom_1"));
            Assert.Equal("groom_1", lineage.GetSpouse("bride_1"));

            float affinity = bridge.GetKinshipAffinity("groom_1", "bride_1");
            Assert.Equal(25f, affinity); // Spouse affinity bonus
        }

        [Fact]
        public void GenealogyBridge_OnDivorce_ClearsSpouseRelationship()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("partner_a", 30);
            engine.RegisterDweller("partner_b", 32);
            var lineage = new GenerationalLineageExtension(engine);
            var bridge = new GenealogyBridge(lineage);

            bridge.OnMarriage("partner_a", "partner_b", day: 5);
            Assert.Equal("partner_b", lineage.GetSpouse("partner_a"));

            bool divorced = bridge.OnDivorce("partner_a", "partner_b", day: 20);
            Assert.True(divorced);
            Assert.Equal(string.Empty, lineage.GetSpouse("partner_a"));
            Assert.Equal(string.Empty, lineage.GetSpouse("partner_b"));
        }

        [Fact]
        public void GenealogyBridge_OnAdoption_EstablishesAdoptiveLineage()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("guardian", 45);
            var lineage = new GenerationalLineageExtension(engine);
            lineage.AssignFamilyName("guardian", "Sinclair");
            var bridge = new GenealogyBridge(lineage);

            var res = bridge.OnAdoption("guardian", "orphan_child", day: 12);

            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal("Sinclair", lineage.GetFamilyName("orphan_child"));
            var parentRec = lineage.GetParent("orphan_child");
            Assert.NotNull(parentRec);
            Assert.Equal("guardian", parentRec.parentId);
            Assert.Equal("adopted", parentRec.relationshipType);
        }

        [Fact]
        public void KinshipAffinity_ReflectsGenerationalBonds()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("patriarch", 60);
            engine.RegisterDweller("son", 35);
            engine.RegisterDweller("daughter", 30);
            engine.RegisterDweller("grandson", 10);
            var lineage = new GenerationalLineageExtension(engine);
            var bridge = new GenealogyBridge(lineage);

            bridge.OnChildBorn("patriarch", "son", day: 1);
            bridge.OnChildBorn("patriarch", "daughter", day: 2);
            bridge.OnChildBorn("son", "grandson", day: 3);

            // Parent-child: +20
            Assert.Equal(20f, bridge.GetKinshipAffinity("patriarch", "son"));
            // Siblings: +15
            Assert.Equal(15f, bridge.GetKinshipAffinity("son", "daughter"));
            // Grandparent-grandchild (ancestor): +10
            Assert.Equal(10f, bridge.GetKinshipAffinity("patriarch", "grandson"));
            // Stranger: 0
            Assert.Equal(0f, bridge.GetKinshipAffinity("grandson", "unrelated_survivor"));
        }

        [Fact]
        public void GenerationalLineage_CaptureAndRestore_PreservesFullGenealogicalHistory()
        {
            var engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("ancestor", 50);
            var lineage = new GenerationalLineageExtension(engine);
            lineage.AssignFamilyName("ancestor", "Ironwood");
            var bridge = new GenealogyBridge(lineage);

            bridge.OnChildBorn("ancestor", "descendant", day: 5);
            bridge.OnMarriage("descendant", "spouse_x", day: 10);

            var savedState = lineage.CaptureState();

            var restoredEngine = new GenerationalSuccessionEngine();
            var restoredLineage = new GenerationalLineageExtension(restoredEngine);
            restoredLineage.RestoreState(savedState);

            Assert.Equal("Ironwood", restoredLineage.GetFamilyName("descendant"));
            Assert.Equal("spouse_x", restoredLineage.GetSpouse("descendant"));
            Assert.Equal("descendant", restoredLineage.GetSpouse("spouse_x"));
            Assert.NotEmpty(restoredLineage.GetDescendants("ancestor"));
        }
    }
}
