using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WorldSaveablesTests
    {
        [Fact]
        public void LocationEvolution_CaptureRestore_PreservesMutations()
        {
            var sys = new LocationEvolutionSystem(new SeededRng(42));
            sys.SetLocationOwner("loc_old_mill", "faction_iron_covenant");
            sys.MarkCleared("loc_old_mill", 5);
            sys.TickDay(10);

            var state = sys.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Single(state.mutations);
            Assert.True(state.mutations[0].isCleared);
            Assert.Equal(10, state.lastEvolutionDay);

            var sys2 = new LocationEvolutionSystem(new SeededRng(42));
            sys2.RestoreState(state);
            var restored = sys2.GetOrCreateRecord("loc_old_mill");
            Assert.NotNull(restored);
            Assert.True(restored.isCleared);
            Assert.Equal(10, sys2.State.lastEvolutionDay);
        }

        [Fact]
        public void WildlifeMigration_CaptureRestore_PreservesPacks()
        {
            var sys = new WildlifeMigrationSystem(new SeededRng(42));
            var reg = sys.RegisterPack("pack_dogs_1", "species_rad_dog", "sector_north_ruins", 6);
            Assert.Equal(ActionResult.StatusKind.Success, reg.Status);

            var mig = sys.MigratePack("pack_dogs_1", "sector_marshlands");
            Assert.Equal(ActionResult.StatusKind.Success, mig.Status);

            sys.TickDay(3);
            var state = sys.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Single(state.packs);
            Assert.Equal("sector_marshlands", state.packs[0].currentSectorId);
            Assert.True(state.packs[0].starvationLevel > 0f);

            var sys2 = new WildlifeMigrationSystem(new SeededRng(42));
            sys2.RestoreState(state);
            Assert.Single(sys2.State.packs);
            Assert.Equal("sector_marshlands", sys2.State.packs[0].currentSectorId);
            Assert.Equal(3, sys2.State.lastMigrationDay);
        }

        [Fact]
        public void LandmarkDegradation_CaptureRestore_PreservesIntegrity()
        {
            var sys = new LandmarkDegradationSystem(new SeededRng(42));
            sys.RegisterLandmark("landmark_water_tower", "loc_suburb_a", 100f);
            sys.DamageLandmark("landmark_water_tower", 30f, 1);
            sys.TickDay(2, weatherAshfallMm: 15f);

            var state = sys.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Single(state.landmarks);
            Assert.True(state.landmarks[0].structuralIntegrity < 70f);
            Assert.True(state.landmarks[0].ashBurialCm > 0f);

            var sys2 = new LandmarkDegradationSystem(new SeededRng(42));
            sys2.RestoreState(state);
            Assert.Single(sys2.State.landmarks);
            Assert.Equal(state.landmarks[0].structuralIntegrity, sys2.State.landmarks[0].structuralIntegrity);
            Assert.Equal(state.landmarks[0].ashBurialCm, sys2.State.landmarks[0].ashBurialCm);
        }
    }
}
