using System.Collections.Generic;
using Ashfall.Core.Maritime;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — unit tests for the
    /// StealthDiveInstance Core system. Phase 0 bug-fix verification.
    /// </summary>
    public class BlackFlotillaTests
    {
        // ── StealthDiveInstance ─────────────────────────────────────────────────

        [Fact]
        public void Dive_StartDive_InitializesCorrectly()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            Assert.True(dive.IsActive);
            Assert.Equal("diver_1", dive.DiverDwellerId);
            Assert.Equal("operator_1", dive.CompressorOperatorDwellerId);
            Assert.Equal(120f, dive.AirSupplySeconds);
            Assert.Equal(0, dive.CurrentRoomIndex);
            Assert.Equal(0, dive.NoiseLevel);
            Assert.False(dive.IsCompromised);
            Assert.Equal(4, dive.Rooms.Count);
        }

        [Fact]
        public void Dive_Tick_ConsumesAir()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            dive.Tick(10f);
            Assert.Equal(110f, dive.AirSupplySeconds);
        }

        [Fact]
        public void Dive_Tick_AirWarning_FiresOnce()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            int warningCount = 0;
            dive.OnAirWarning += _ => warningCount++;
            dive.Tick(95f);  // air = 25, below 30
            dive.Tick(5f);   // air = 20, still below 30
            dive.Tick(5f);   // air = 15, still below 30
            Assert.Equal(1, warningCount);
        }

        [Fact]
        public void Dive_Tick_AirDepleted_EndsDive()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 60f);
            bool ended = false;
            bool success = true;
            dive.OnDiveEnded += s => { ended = true; success = s; };
            dive.Tick(70f);
            Assert.True(ended);
            Assert.False(success);
            Assert.False(dive.IsActive);
        }

        [Fact]
        public void Dive_CrankCompressor_AddsAir()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            dive.Tick(60f);
            Assert.Equal(60f, dive.AirSupplySeconds);
            dive.CrankCompressor();
            Assert.Equal(90f, dive.AirSupplySeconds); // +30 base air per crank
        }

        [Fact]
        public void Dive_CrankCompressor_DoesNotExceedMax()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            dive.CrankCompressor();
            Assert.Equal(120f, dive.AirSupplySeconds); // capped at max
        }

        [Fact]
        public void Dive_AdvanceRoom_IncreasesNoise()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            int enteredRoom = -1;
            dive.OnRoomEntered += idx => enteredRoom = idx;
            bool advanced = dive.AdvanceToNextRoom(25);
            Assert.True(advanced);
            Assert.Equal(1, dive.CurrentRoomIndex);
            Assert.Equal(25, dive.NoiseLevel);
            Assert.Equal(1, enteredRoom);
            Assert.False(dive.IsCompromised);
        }

        [Fact]
        public void Dive_AdvanceRoom_Compromised_At80Noise()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            dive.AdvanceToNextRoom(40);
            Assert.False(dive.IsCompromised);
            dive.AdvanceToNextRoom(40);
            Assert.True(dive.IsCompromised);
            Assert.Equal(80, dive.NoiseLevel);
        }

        [Fact]
        public void Dive_AdvanceRoom_NoiseClamped_At100()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            dive.AdvanceToNextRoom(50);
            dive.AdvanceToNextRoom(50);
            dive.AdvanceToNextRoom(50);
            Assert.Equal(100, dive.NoiseLevel);
        }

        [Fact]
        public void Dive_AdvanceRoom_RejectsAtLastRoom()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            dive.AdvanceToNextRoom(10);
            dive.AdvanceToNextRoom(10);
            dive.AdvanceToNextRoom(10);
            Assert.Equal(3, dive.CurrentRoomIndex);
            Assert.False(dive.AdvanceToNextRoom(10));
            Assert.Equal(3, dive.CurrentRoomIndex);
        }

        [Fact]
        public void Dive_EndDive_Success()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            bool ended = false;
            bool success = false;
            dive.OnDiveEnded += s => { ended = true; success = s; };
            dive.EndDive(true);
            Assert.True(ended);
            Assert.True(success);
            Assert.False(dive.IsActive);
        }

        [Fact]
        public void Dive_Tick_NoOp_WhenInactive()
        {
            var dive = new StealthDiveInstance();
            dive.Tick(10f);
            Assert.False(dive.IsActive);
            Assert.Equal(0f, dive.AirSupplySeconds);
        }

        [Fact]
        public void Dive_CrankCompressor_NoOp_WhenInactive()
        {
            var dive = new StealthDiveInstance();
            dive.CrankCompressor();
            Assert.Equal(0f, dive.AirSupplySeconds);
        }

        [Fact]
        public void Dive_RoomTypes_CorrectOrder()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            Assert.Equal(DiveRoomType.Deckhouse, dive.Rooms[0].roomType);
            Assert.Equal(DiveRoomType.Companionway, dive.Rooms[1].roomType);
            Assert.Equal(DiveRoomType.HoldApproach, dive.Rooms[2].roomType);
            Assert.Equal(DiveRoomType.DeepHold, dive.Rooms[3].roomType);
        }

        [Fact]
        public void Dive_HazardLevels_Increase()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            Assert.Equal(1, dive.Rooms[0].hazardLevel);
            Assert.Equal(2, dive.Rooms[1].hazardLevel);
            Assert.Equal(3, dive.Rooms[2].hazardLevel);
            Assert.Equal(4, dive.Rooms[3].hazardLevel);
        }

        // ── Save / Load ─────────────────────────────────────────────────────────

        [Fact]
        public void Dive_CaptureRestore_Roundtrip()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            dive.Tick(30f);
            dive.CrankCompressor();
            dive.AdvanceToNextRoom(25);

            var save = dive.CaptureState();
            Assert.True(save.isActive);
            Assert.Equal("diver_1", save.diverDwellerId);
            Assert.Equal(4, save.rooms.Count);

            var restored = new StealthDiveInstance();
            restored.RestoreState(save);
            Assert.True(restored.IsActive);
            Assert.Equal("diver_1", restored.DiverDwellerId);
            Assert.Equal("operator_1", restored.CompressorOperatorDwellerId);
            Assert.Equal(1, restored.CurrentRoomIndex);
            Assert.Equal(25, restored.NoiseLevel);
            Assert.Equal(4, restored.Rooms.Count);
        }

        [Fact]
        public void Dive_RestoreNull_DoesNotCrash()
        {
            var dive = new StealthDiveInstance();
            dive.RestoreState(null);
            Assert.False(dive.IsActive);
            Assert.Empty(dive.Rooms);
        }

        [Fact]
        public void Dive_RestoreState_PreservesCompromise()
        {
            var dive = new StealthDiveInstance();
            dive.StartDive("diver_1", "operator_1", 120f);
            dive.AdvanceToNextRoom(80);
            Assert.True(dive.IsCompromised);

            var save = dive.CaptureState();
            var restored = new StealthDiveInstance();
            restored.RestoreState(save);
            Assert.True(restored.IsCompromised);
        }

        // ── ProceduralScavengeSystem (Core port) ────────────────────────────────

        [Fact]
        public void Scavenge_RollLootTable_DeterministicWithSameSeed()
        {
            var rng1 = new SeededRng(42);
            var rng2 = new SeededRng(42);
            var s1 = new ProceduralScavengeSystem(rng1);
            var s2 = new ProceduralScavengeSystem(rng2);
            s1.SetCurrentDay(30);
            s2.SetCurrentDay(30);

            var table = new List<VariableLootNode>
            {
                new VariableLootNode { ItemId = "scrap_metal", MinQty = 5, MaxQty = 20, SpawnChance = 1.0f }
            };

            var r1 = s1.RollLootTable("loc_a", table, 0f, false);
            var r2 = s2.RollLootTable("loc_a", table, 0f, false);
            Assert.Single(r1);
            Assert.Single(r2);
            Assert.Equal(r1[0].Quantity, r2[0].Quantity);
        }

        [Fact]
        public void Scavenge_RollLootTable_DifferentSeeds_DifferentResults()
        {
            var s1 = new ProceduralScavengeSystem(new SeededRng(1));
            var s2 = new ProceduralScavengeSystem(new SeededRng(999));
            s1.SetCurrentDay(30);
            s2.SetCurrentDay(30);

            var table = new List<VariableLootNode>
            {
                new VariableLootNode { ItemId = "scrap_metal", MinQty = 1, MaxQty = 50, SpawnChance = 0.5f }
            };

            int qty1 = 0, qty2 = 0;
            for (int i = 0; i < 20; i++)
            {
                var r1 = s1.RollLootTable("loc_" + i, table, 0f, false);
                var r2 = s2.RollLootTable("loc_" + i, table, 0f, false);
                if (r1.Count > 0) qty1 += r1[0].Quantity;
                if (r2.Count > 0) qty2 += r2[0].Quantity;
            }
            Assert.NotEqual(qty1, qty2);
        }

        [Fact]
        public void Scavenge_VisitCount_ReducesYields()
        {
            var scav = new ProceduralScavengeSystem(new SeededRng(42));
            scav.SetCurrentDay(10);
            var table = new List<VariableLootNode>
            {
                new VariableLootNode { ItemId = "scrap", MinQty = 10, MaxQty = 50, SpawnChance = 1.0f }
            };

            int firstQty = 0, laterQty = 0;
            for (int i = 0; i < 5; i++)
            {
                var r = scav.RollLootTable("loc_a", table, 0f, false);
                if (i == 0 && r.Count > 0) firstQty = r[0].Quantity;
            }
            for (int i = 0; i < 5; i++)
            {
                var r = scav.RollLootTable("loc_a", table, 0f, false);
                if (r.Count > 0) laterQty += r[0].Quantity;
            }
            Assert.True(laterQty > 0);
        }

        [Fact]
        public void Scavenge_Contamination_HighRad()
        {
            var scav = new ProceduralScavengeSystem(new SeededRng(42));
            scav.SetCurrentDay(10);
            var table = new List<VariableLootNode>
            {
                new VariableLootNode { ItemId = "scrap", MinQty = 1, MaxQty = 5, SpawnChance = 1.0f }
            };

            bool contaminated = false;
            scav.OnContaminationApplied += (_, __) => contaminated = true;
            scav.RollLootTable("loc_a", table, 20f, false);
            Assert.True(contaminated);
        }

        [Fact]
        public void Scavenge_Decontaminate_WithAndWithoutResources()
        {
            var scav = new ProceduralScavengeSystem();
            Assert.Equal(0, scav.Decontaminate(10, false, false));
            Assert.Equal(0, scav.Decontaminate(10, true, false));
            Assert.Equal(0, scav.Decontaminate(10, false, true));
            Assert.Equal(6, scav.Decontaminate(10, true, true));
        }

        [Fact]
        public void Scavenge_CaptureRestore_Roundtrip()
        {
            var scav = new ProceduralScavengeSystem(new SeededRng(42));
            scav.SetCurrentDay(50);
            var table = new List<VariableLootNode>
            {
                new VariableLootNode { ItemId = "scrap", MinQty = 1, MaxQty = 5, SpawnChance = 1.0f }
            };
            scav.RollLootTable("loc_a", table, 0f, false);
            scav.RollLootTable("loc_b", table, 0f, false);

            var save = scav.CaptureState();
            Assert.Equal(50, save.CurrentDay);
            Assert.Equal(2, save.LocationVisits.Length);

            var restored = new ProceduralScavengeSystem(new SeededRng(99));
            restored.RestoreState(save);
            Assert.Equal(1, restored.GetVisitCount("loc_a"));
            Assert.Equal(1, restored.GetVisitCount("loc_b"));
        }

        [Fact]
        public void Scavenge_NullLocationId_ReturnsEmpty()
        {
            var scav = new ProceduralScavengeSystem();
            var result = scav.RollLootTable(null, new List<VariableLootNode>(), 0f, false);
            Assert.Empty(result);
        }

        // ── PsychologicalContaminationSystem (Core port) ────────────────────────

        [Fact]
        public void Contam_Apply_HasContamination()
        {
            var sys = new PsychologicalContaminationSystem();
            sys.ApplyContamination("survivor_1", "location_sunshine_daycare", 50f);
            Assert.True(sys.HasContamination("survivor_1", PsychologicalContaminationSystem.Contam_ChildCotTrauma));
            Assert.True(sys.HasContamination("survivor_1", PsychologicalContaminationSystem.Contam_ThousandYardStare));
        }

        [Fact]
        public void Contam_Apply_Idempotent()
        {
            var sys = new PsychologicalContaminationSystem();
            sys.ApplyContamination("survivor_1", "location_sunshine_daycare", 50f);
            sys.ApplyContamination("survivor_1", "location_sunshine_daycare", 40f);
            var entries = sys.GetContaminations("survivor_1");
            int childCotCount = 0;
            foreach (var e in entries)
                if (e.Type == PsychologicalContaminationSystem.Contam_ChildCotTrauma) childCotCount++;
            Assert.Equal(1, childCotCount);
        }

        [Fact]
        public void Contam_ActionBlocked()
        {
            var sys = new PsychologicalContaminationSystem();
            sys.ApplyContamination("survivor_1", "location_sunshine_daycare", 50f);
            Assert.True(sys.IsActionBlocked("survivor_1", "action_teach_child"));
            Assert.False(sys.IsActionBlocked("survivor_1", "action_cook"));
        }

        [Fact]
        public void Contam_Tick_ExpiresAfterDuration()
        {
            var sys = new PsychologicalContaminationSystem();
            sys.ApplyContamination("survivor_1", "location_stadium_evacuation_center", 50f);
            Assert.True(sys.HasContamination("survivor_1", PsychologicalContaminationSystem.Contam_ThousandYardStare));

            sys.Tick(4f, "survivor_1", 50f, null);
            Assert.False(sys.HasContamination("survivor_1", PsychologicalContaminationSystem.Contam_ThousandYardStare));
        }

        [Fact]
        public void Contam_MentalBreak_OnBadAssignment()
        {
            var sys = new PsychologicalContaminationSystem();
            sys.ApplyContamination("survivor_1", "location_stadium_evacuation_center", 50f);
            string breakSurvivor = null;
            sys.OnMentalBreakFromContamination += id => breakSurvivor = id;
            sys.Tick(1f, "survivor_1", 30f, "shelter_module_autopsy");
            Assert.Equal("survivor_1", breakSurvivor);
        }

        [Fact]
        public void Contam_CaptureRestore_Roundtrip()
        {
            var sys = new PsychologicalContaminationSystem();
            sys.ApplyContamination("survivor_1", "location_automated_abattoir", 40f);
            sys.ApplyContamination("survivor_2", "location_sunshine_daycare", 60f);

            var save = sys.CaptureState();
            Assert.NotNull(save.Survivors);
            Assert.Equal(2, save.Survivors.Length);

            var restored = new PsychologicalContaminationSystem();
            restored.RestoreState(save);
            Assert.True(restored.HasContamination("survivor_1", PsychologicalContaminationSystem.Contam_DisgustCascade));
            Assert.True(restored.HasContamination("survivor_2", PsychologicalContaminationSystem.Contam_ChildCotTrauma));
        }

        [Fact]
        public void Contam_UnknownLocation_NoEffect()
        {
            var sys = new PsychologicalContaminationSystem();
            sys.ApplyContamination("survivor_1", "location_unknown_place", 50f);
            Assert.False(sys.HasContamination("survivor_1", PsychologicalContaminationSystem.Contam_ThousandYardStare));
        }

        [Fact]
        public void Contam_MoralChronicle_FiresForDaycare()
        {
            var sys = new PsychologicalContaminationSystem();
            string chronicleSurvivor = null;
            sys.OnMoralChronicleEntry += (id, _) => chronicleSurvivor = id;
            sys.ApplyContamination("survivor_1", "location_sunshine_daycare", 50f);
            Assert.Equal("survivor_1", chronicleSurvivor);
        }

        // ── DeepLoreLocationCatalogLoader ───────────────────────────────────────

        [Fact]
        public void DeepLore_Locations_LoadFromFile()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var locations = DeepLoreLocationCatalogLoader.Load(dataDir, io, json);
            Assert.True(locations.Count >= 10, $"expected >=10 deep lore locations, got {locations.Count}");

            var library = DeepLoreLocationCatalogLoader.FindById(locations, "location_municipal_library");
            Assert.NotNull(library);
            Assert.True(library.lootTable.Count > 0);
            Assert.True(library.dangerLevel > 0);
        }

        [Fact]
        public void DeepLore_Locations_ReturnsEmpty_WhenMissing()
        {
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var result = DeepLoreLocationCatalogLoader.Load("/nonexistent", io, json);
            Assert.Empty(result);
        }
    }
}
