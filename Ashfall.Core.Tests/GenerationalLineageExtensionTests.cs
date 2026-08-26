using Ashfall.Core;
using Ashfall.Core.Legacy;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class GenerationalLineageExtensionTests
    {
        [Fact] public void EstablishLineage_CreatesRecord()
        {
            var gl = Create(out _);
            var r = gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(gl.State.lineages);
        }

        [Fact] public void EstablishLineage_Duplicate_Blocks()
        {
            var gl = Create(out _);
            gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            var r = gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void GetLineage_ReturnsRelated()
        {
            var gl = Create(out _);
            gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            var lineage = gl.GetLineage("founder_1");
            Assert.Single(lineage);
        }

        [Fact] public void GetParent_ReturnsParent()
        {
            var gl = Create(out _);
            gl.EstablishLineage("founder_1", "apprentice_1", "parent");
            var parent = gl.GetParent("apprentice_1");
            Assert.NotNull(parent);
            Assert.Equal("founder_1", parent.parentId);
        }

        [Fact] public void PerformSuccession_RetiresAndAdvances()
        {
            var gl = Create(out var engine);
            engine.RegisterDweller("founder_1", 40);
            engine.RegisterDweller("successor_1", 20);
            var r = gl.PerformSuccession("founder_1", "successor_1");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(engine.GetRecord("founder_1")?.isRetired);
        }

        [Fact] public void PerformSuccession_Unknown_Fails()
        {
            var gl = Create(out _);
            var r = gl.PerformSuccession("nonexistent", "successor_1");
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        [Fact] public void CaptureRestoreState_PreservesLineages()
        {
            var gl = Create(out _);
            gl.EstablishLineage("founder_1", "apprentice_1", "mentor");
            var state = gl.CaptureState();
            Assert.Single(state.lineages);

            var gl2 = Create(out _);
            gl2.RestoreState(state);
            Assert.Single(gl2.State.lineages);
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
