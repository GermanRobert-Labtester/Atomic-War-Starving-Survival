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

        [Fact]
        public void LocationEvolution_SnapshotIsolation_MutatingCapturedDoesNotAffectLive()
        {
            var sys = new LocationEvolutionSystem(new SeededRng(1));
            sys.SetLocationOwner("loc_test", "faction_a");
            var snap = sys.CaptureState();
            // Mutate captured snapshot
            snap.mutations[0].currentOwner = "tampered";
            snap.mutations[0].activeThreats.Add("threat_new");
            snap.lastEvolutionDay = 999;
            // Live state must be unaffected
            var live = sys.GetOrCreateRecord("loc_test");
            Assert.NotNull(live);
            Assert.Equal("faction_a", live.currentOwner);
            Assert.DoesNotContain("threat_new", live.activeThreats);
            Assert.NotEqual(999, sys.State.lastEvolutionDay);
            // Reverse: mutate live must not affect snap
            live.currentOwner = "faction_b";
            Assert.Equal("tampered", snap.mutations[0].currentOwner);
        }

        [Fact]
        public void WildlifeMigration_SnapshotIsolation_MutatingCapturedDoesNotAffectLive()
        {
            var sys = new WildlifeMigrationSystem(new SeededRng(2));
            sys.RegisterPack("pack_1", "species_dog", "sector_a", 5);
            var snap = sys.CaptureState();
            snap.packs[0].currentSectorId = "tampered_sector";
            snap.lastMigrationDay = 999;
            var live = sys.State.packs[0];
            Assert.Equal("sector_a", live.currentSectorId);
            Assert.NotEqual(999, sys.State.lastMigrationDay);
            live.currentSectorId = "sector_b";
            Assert.Equal("tampered_sector", snap.packs[0].currentSectorId);
        }

        [Fact]
        public void LandmarkDegradation_SnapshotIsolation_MutatingCapturedDoesNotAffectLive()
        {
            var sys = new LandmarkDegradationSystem(new SeededRng(3));
            sys.RegisterLandmark("lm_1", "loc_a", 80f);
            var snap = sys.CaptureState();
            snap.landmarks[0].structuralIntegrity = 1f;
            snap.lastDegradationDay = 999;
            var live = sys.State.landmarks[0];
            Assert.Equal(80f, live.structuralIntegrity);
            Assert.NotEqual(999, sys.State.lastDegradationDay);
            live.structuralIntegrity = 5f;
            Assert.Equal(1f, snap.landmarks[0].structuralIntegrity);
        }
    }
}
