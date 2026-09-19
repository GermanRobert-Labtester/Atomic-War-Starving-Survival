// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Legacy;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class GenerationalLineageExtensionTests
    {
        [Fact]
        public void EstablishLineage_CreatesRecord()
        {
            var gl = Create(out _);
            var r = gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(gl.State.lineages);
        }

        [Fact]
        public void EstablishLineage_Duplicate_Blocks()
        {
            var gl = Create(out _);
            gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            var r = gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact]
        public void GetLineage_ReturnsRelated()
        {
            var gl = Create(out _);
            gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            var lineage = gl.GetLineage("founder_1");
            Assert.Single(lineage);
        }

        [Fact]
        public void GetParent_ReturnsParent()
        {
            var gl = Create(out _);
            gl.EstablishLineage("founder_1", "apprentice_1", "parent");
            var parent = gl.GetParent("apprentice_1");
            Assert.NotNull(parent);
            Assert.Equal("founder_1", parent.parentId);
        }

        [Fact]
        public void PerformSuccession_RetiresAndAdvances()
        {
            var gl = Create(out var engine);
            engine.RegisterDweller("founder_1", 40);
            engine.RegisterDweller("successor_1", 20);
            var r = gl.PerformSuccession("founder_1", "successor_1");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(engine.GetRecord("founder_1")?.isRetired);
        }

        [Fact]
        public void PerformSuccession_Unknown_Fails()
        {
            var gl = Create(out _);
            var r = gl.PerformSuccession("nonexistent", "successor_1");
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        [Fact]
        public void SiblingsAndSpouse_ComputedCorrectly()
        {
            var gl = Create(out _);
            // founder_1 is parent of both apprentice_1 and child_2
            gl.EstablishLineage("founder_1", "apprentice_1", "parent");
            gl.EstablishLineage("founder_1", "child_2", "parent");

            var siblings = gl.GetSiblings("apprentice_1");
            Assert.Single(siblings);
            Assert.Contains("child_2", siblings);

            // Spouse setting
            bool ok = gl.SetSpouse("founder_1", "spouse_1");
            Assert.True(ok);
            Assert.Equal("spouse_1", gl.GetSpouse("founder_1"));
            Assert.Equal("founder_1", gl.GetSpouse("spouse_1"));
        }

        [Fact]
        public void AncestorsAndLineageDepth_CalculatedCorrectly()
        {
            var gl = Create(out _);
            // gen0 -> gen1 -> gen2
            gl.EstablishLineage("founder_1", "gen1_dweller", "parent");
            gl.EstablishLineage("gen1_dweller", "gen2_dweller", "parent");

            var ancestors = gl.GetAncestors("gen2_dweller");
            Assert.Contains("gen1_dweller", ancestors);
            Assert.Contains("founder_1", ancestors);
            Assert.Equal(2, ancestors.Count);

            int depth = gl.GetLineageDepth("gen2_dweller");
            Assert.Equal(2, depth);

            var descendants = gl.GetDescendants("founder_1");
            Assert.Contains("gen1_dweller", descendants);
            Assert.Contains("gen2_dweller", descendants);
        }

        [Fact]
        public void FamilyUnitAndEvents_TrackedCorrectly()
        {
            var gl = Create(out _);
            var unit = gl.FormFamilyUnit("House of Vance", foundingDay: 1, new[] { "founder_1", "apprentice_1" });
            Assert.NotNull(unit);
            Assert.Equal("House of Vance", unit.familyName);

            var foundUnit = gl.GetFamilyUnit("founder_1");
            Assert.NotNull(foundUnit);
            Assert.Equal(unit.unitId, foundUnit.unitId);

            var ev = gl.RecordFamilyEvent("milestone", 10, new[] { "founder_1" }, "First harvest festival.", "major");
            Assert.NotNull(ev);
            var dwellerEvents = gl.GetFamilyEvents("founder_1");
            Assert.NotEmpty(dwellerEvents);
        }

        [Fact]
        public void KinshipAffinityBonus_CalculatesExpectedValues()
        {
            var gl = Create(out _);
            gl.EstablishLineage("founder_1", "apprentice_1", "parent");
            gl.EstablishLineage("founder_1", "child_2", "parent");
            gl.SetSpouse("founder_1", "spouse_1");

            // Spouse bonus: +25
            Assert.Equal(25f, gl.GetKinshipAffinityBonus("founder_1", "spouse_1"));
            // Parent-child bonus: +20
            Assert.Equal(20f, gl.GetKinshipAffinityBonus("founder_1", "apprentice_1"));
            // Sibling bonus: +15
            Assert.Equal(15f, gl.GetKinshipAffinityBonus("apprentice_1", "child_2"));
            // Unrelated: 0
            Assert.Equal(0f, gl.GetKinshipAffinityBonus("apprentice_1", "unrelated_stranger"));
        }

        [Fact]
        public void CaptureRestoreState_PreservesLineagesAndFamilyUnits()
        {
            var gl = Create(out _);
            gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            gl.FormFamilyUnit("Clan Iron", 1, new[] { "founder_1" });
            var state = gl.CaptureState();
            Assert.Single(state.lineages);
            Assert.Single(state.familyUnits);

            var gl2 = Create(out _);
            gl2.RestoreState(state);
            Assert.Single(gl2.State.lineages);
            Assert.Single(gl2.State.familyUnits);
            Assert.Equal("Clan Iron", gl2.State.familyUnits[0].familyName);
        }

        private static GenerationalLineageExtension Create(out GenerationalSuccessionEngine engine)
        {
            engine = new GenerationalSuccessionEngine();
            engine.RegisterDweller("founder_1", 40);
            engine.RegisterDweller("apprentice_1", 15);
            engine.RegisterDweller("successor_1", 20);
            return new GenerationalLineageExtension(engine);
        }
    }
}
