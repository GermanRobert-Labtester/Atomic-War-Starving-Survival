// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// Plan 210 Phase 1 — SanitationSystem behavior: accumulation scaling,
    /// facility processing (power/condition/storage capacity), cleaning
    /// duty, bounded hygiene + pathogen modifier, compost conversion,
    /// cross-contamination rejection, deterministic spills, tick-exactly-
    /// once, save/restore, legacy defaults, and paired determinism.
    /// </summary>
    public sealed class Plan210SanitationSystemTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static SanitationSystem CreateSystem(int day = 1)
        {
            var load = SanitationFacilityCatalogLoader.Load(
                GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            var system = new SanitationSystem();
            system.BindFacilityCatalog(SanitationFacilityCatalogLoader.ToCatalog(load));
            return system;
        }

        // ── Accumulation ─────────────────────────────────────────────

        [Fact]
        public void OrganicAccumulation_ScalesWithPopulation_AcrossResidentialRooms()
        {
            var system = CreateSystem();
            system.EnsureRoom("room_a", RoomWasteRole.Residential);
            system.EnsureRoom("room_b", RoomWasteRole.Residential);
            system.TickDaily(1, population: 6);
            // 6 survivors × 1.0 split across 2 rooms = 3.0 each.
            Assert.Equal(3f, system.FindRoom("room_a")!.organic, 4);
            Assert.Equal(3f, system.FindRoom("room_b")!.organic, 4);
            system.TickDaily(2, population: 12);
            Assert.Equal(9f, system.FindRoom("room_a")!.organic, 4);
        }

        [Fact]
        public void FoodPrepRooms_ContributeFlatOrganicStream()
        {
            var system = CreateSystem();
            system.EnsureRoom("kitchen", RoomWasteRole.FoodPrep);
            system.TickDaily(1, population: 0);
            Assert.Equal(SanitationSystem.OrganicPerFoodPrepRoomPerDay, system.FindRoom("kitchen")!.organic, 4);
        }

        [Fact]
        public void ChemicalAndRadioactive_AccumulateOnlyThroughProducers()
        {
            var system = CreateSystem();
            system.EnsureRoom("workshop", RoomWasteRole.Industrial);
            system.TickDaily(5, population: 8);
            Assert.Equal(0f, system.FindRoom("workshop")!.chemical, 4);   // no silent auto-emission
            Assert.True(system.EmitWaste("workshop", WasteType.Chemical, 4f));
            Assert.True(system.EmitWaste("workshop", WasteType.Radioactive, 2f));
            Assert.Equal(4f, system.FindRoom("workshop")!.chemical, 4);
            Assert.Equal(2f, system.FindRoom("workshop")!.radioactive, 4);
            Assert.False(system.EmitWaste("ghost_room", WasteType.Chemical, 1f));
            Assert.False(system.EmitWaste("workshop", WasteType.Chemical, -1f));
        }

        // ── Facility processing ──────────────────────────────────────

        [Fact]
        public void FacilityProcessing_ReducesWaste_RespectingPowerAndRate()
        {
            var system = CreateSystem();
            var room = system.EnsureRoom("dorm", RoomWasteRole.Residential);
            room.organic = 30f;
            system.InstallFacility("sanitation_wash_station", "dorm");
            system.TickDaily(1, population: 0);
            // Rate 8 → 30 − 8 = 22.
            Assert.Equal(22f, room.organic, 4);

            // Unpowered facility with a power draw stalls entirely.
            var installed = system.State.facilities.Single(f => f.roomId == "dorm");
            installed.powered = false;
            system.TickDaily(2, population: 0);
            Assert.Equal(22f, room.organic, 4);
        }

        [Fact]
        public void StorageFacility_ContainsWaste_UntilFull_ThenBacksUp()
        {
            var system = CreateSystem();
            var room = system.EnsureRoom("chem_bay", RoomWasteRole.Industrial);
            room.chemical = 100f;
            system.InstallFacility("sanitation_chemical_locker", "chem_bay");
            system.TickDaily(1, population: 0);
            // Rate 4 contained into the locker.
            Assert.Equal(96f, room.chemical, 4);
            Assert.Equal(4f, system.State.facilities[0].fill, 4);

            // Fill the locker to capacity; further waste backs up.
            var facility = system.State.facilities[0];
            facility.fill = 120f;   // at capacity
            system.TickDaily(2, population: 0);
            Assert.Equal(96f, room.chemical, 4);   // no processing, no silent deletion
        }

        // ── Cleaning ─────────────────────────────────────────────────

        [Fact]
        public void CleaningDuty_ReducesWaste_BoundedByContent()
        {
            var system = CreateSystem();
            var room = system.EnsureRoom("dorm", RoomWasteRole.Residential);
            room.organic = 10f;
            var result = system.ApplyCleaning("dorm", workers: 2, skill01: 1f, priority: CleaningPriority.Normal, day: 1);
            Assert.True(result.Accepted);
            Assert.Equal(10f, result.OrganicRemoved, 4);   // capped by content
            Assert.Equal(0f, room.organic, 4);

            Assert.False(system.ApplyCleaning("ghost", 1, 1f, CleaningPriority.Normal, 1).Accepted);
            Assert.False(system.ApplyCleaning("dorm", 0, 1f, CleaningPriority.Normal, 1).Accepted);
        }

        [Fact]
        public void CleaningPriority_ScalesEffort()
        {
            var system = CreateSystem();
            var big = system.EnsureRoom("flooded", RoomWasteRole.Other);
            big.organic = 1000f;
            var normal = system.ApplyCleaning("flooded", 2, 1f, CleaningPriority.Normal, 1);
            Assert.Equal(10f, normal.OrganicRemoved, 4);   // 2 × 5
            big.organic = 1000f;
            var critical = system.ApplyCleaning("flooded", 2, 1f, CleaningPriority.Critical, 2);
            Assert.Equal(20f, critical.OrganicRemoved, 4);  // 2 × 5 × 2
        }

        // ── Hygiene / pathogen modifier ──────────────────────────────

        [Fact]
        public void Hygiene_IsDerived_Bounded_AndMonotoneAgainstWaste()
        {
            var system = CreateSystem();
            var room = system.EnsureRoom("dorm", RoomWasteRole.Residential);
            Assert.Equal(1000, system.GetRoomHygienePermille("dorm"));

            room.organic = 30f;   // half the allowance
            int half = system.GetRoomHygienePermille("dorm");
            Assert.InRange(half, 400, 600);

            room.organic = 120f;  // above allowance
            int swamped = system.GetRoomHygienePermille("dorm");
            Assert.True(swamped < half, "more waste must mean lower hygiene");
            Assert.InRange(swamped, 0, 1000);
        }

        [Fact]
        public void PathogenExposureModifier_IsBounded()
        {
            var system = CreateSystem();
            var clean = system.EnsureRoom("clean", RoomWasteRole.Residential);
            var foul = system.EnsureRoom("foul", RoomWasteRole.Residential);
            foul.organic = 200f;
            foul.chemical = 50f;
            float cleanMod = system.GetPathogenExposureModifier("clean");
            float foulMod = system.GetPathogenExposureModifier("foul");
            Assert.Equal(1.0f, cleanMod, 4);
            Assert.InRange(foulMod, 1.0f, 2.0f);   // never unbounded
            Assert.True(foulMod > cleanMod);
            Assert.Equal(1.0f, system.GetPathogenExposureModifier("ghost_room"), 4);
        }

        [Fact]
        public void Spill_CapsRoomHygiene()
        {
            var system = CreateSystem();
            var room = system.EnsureRoom("dorm", RoomWasteRole.Residential);
            room.organic = 200f;
            system.TickDaily(1, population: 0);   // overcapacity → spill
            Assert.NotNull(system.ActiveSpill);
            // Spilled room can never read better than the spill cap.
            Assert.InRange(system.GetRoomHygienePermille("dorm"), 0, (int)SanitationSystem.SpillHygieneCapPermille);
        }

        // ── Compost ──────────────────────────────────────────────────

        [Fact]
        public void CompostUnit_ConvertsOrganic_IntoFertilizerBatch_AfterProcessingDays()
        {
            var system = CreateSystem();
            var room = system.EnsureRoom("greenhouse", RoomWasteRole.Other);
            room.organic = 10f;
            system.InstallFacility("sanitation_compost_unit", "greenhouse");

            var ready = new List<CompostBatchState>();
            system.OnCompostReady += b => ready.Add(b);

            system.TickDaily(1, population: 0);
            Assert.Equal(5f, room.organic, 4);   // rate 5 converted
            Assert.Single(system.CompostQueue);
            var batch = system.CompostQueue[0];
            Assert.Equal(7, batch.readyDay);     // day 1 + 6
            Assert.Equal("item_compost_humus", batch.outputItemId);
            Assert.Equal(3f, batch.outputUnits, 4);   // 5 × 0.6

            system.TickDaily(7, population: 0);
            // Day 7 tick: the remaining 5 units converted FIRST (batch 2,
            // readyDay 13), then batch 1 matured and fired.
            Assert.Single(ready);
            Assert.Equal(3f, ready[0].outputUnits, 4);
            Assert.Single(system.CompostQueue);
            Assert.Equal(13, system.CompostQueue[0].readyDay);

            system.TickDaily(13, population: 0);
            Assert.Equal(2, ready.Count);
            Assert.Empty(system.CompostQueue);
        }

        [Fact]
        public void CrossContamination_CompostRejectsChemicalAndRadioactive()
        {
            var system = CreateSystem();
            var room = system.EnsureRoom("mixed_bay", RoomWasteRole.Industrial);
            room.organic = 20f;
            room.chemical = 20f;
            room.radioactive = 20f;
            system.InstallFacility("sanitation_compost_unit", "mixed_bay");
            system.TickDaily(1, population: 0);
            // Organic converted; chemical/radioactive untouched (type check at
            // the Core API level, not a data convention).
            Assert.Equal(15f, room.organic, 4);
            Assert.Equal(20f, room.chemical, 4);
            Assert.Equal(20f, room.radioactive, 4);
            foreach (var batch in system.CompostQueue)
                Assert.Equal("item_compost_humus", batch.outputItemId);
        }

        // ── Spills ───────────────────────────────────────────────────

        [Fact]
        public void Spill_TriggersDeterministically_OnlyOnOvercapacity()
        {
            var spills = new List<ActiveSpillState>();
            var system = CreateSystem();
            system.OnSpillStarted += s => spills.Add(s);
            var room = system.EnsureRoom("dorm", RoomWasteRole.Residential);

            // Within safe capacity: no arbitrary catastrophe.
            room.organic = 80f;   // below 2× allowance (120 weighted)
            system.TickDaily(1, population: 0);
            Assert.Empty(spills);
            Assert.Null(system.ActiveSpill);

            // Overcapacity: deterministic spill in the worst room.
            room.organic = 200f;
            system.TickDaily(2, population: 0);
            Assert.Single(spills);
            Assert.Equal("dorm", spills[0].roomId);
            Assert.Equal(WasteType.Organic, spills[0].type);
            Assert.Equal("overcapacity_organic", spills[0].reason);
            Assert.NotNull(system.ActiveSpill);
        }

        [Fact]
        public void Spill_Resolves_WhenWasteDropsBackWithinCapacity()
        {
            var resolved = new List<ActiveSpillState>();
            var system = CreateSystem();
            system.OnSpillResolved += s => resolved.Add(s);
            var room = system.EnsureRoom("dorm", RoomWasteRole.Residential);
            room.organic = 200f;
            system.TickDaily(1, population: 0);
            Assert.NotNull(system.ActiveSpill);
            system.ApplyCleaning("dorm", 10, 1f, CleaningPriority.Critical, 2);   // heavy cleanup
            system.TickDaily(3, population: 0);
            // After cleaning + no new waste, the spill must resolve.
            Assert.Single(resolved);
            Assert.Null(system.ActiveSpill);
        }

        // ── Complaints ───────────────────────────────────────────────

        [Fact]
        public void NarrativeComplaints_RespectThresholdAndCooldown()
        {
            var complaints = new List<(string topic, HygieneBand band)>();
            var system = CreateSystem();
            system.OnSanitationComplaint += (topic, band) => complaints.Add((topic, band));
            var room = system.EnsureRoom("dorm", RoomWasteRole.Residential);

            room.organic = 30f;
            system.TickDaily(1, population: 0);   // hygiene ~500 → Poor
            Assert.Single(complaints);
            system.TickDaily(2, population: 0);
            system.TickDaily(3, population: 0);
            Assert.Single(complaints);   // cooldown holds

            system.TickDaily(5, population: 0);   // day 5 − day 1 >= 4
            Assert.Equal(2, complaints.Count);
        }

        // ── Tick exactly once ────────────────────────────────────────

        [Fact]
        public void TickExactlyOnce_PerDay_Guarded()
        {
            var system = CreateSystem();
            system.EnsureRoom("dorm", RoomWasteRole.Residential);
            system.TickDaily(1, population: 4);
            float afterFirst = system.FindRoom("dorm")!.organic;
            system.TickDaily(1, population: 4);   // same-day re-entry
            Assert.Equal(afterFirst, system.FindRoom("dorm")!.organic, 4);
            system.TickDaily(2, population: 4);
            Assert.Equal(afterFirst + 4f, system.FindRoom("dorm")!.organic, 4);
        }

        // ── Save / restore ───────────────────────────────────────────

        [Fact]
        public void SaveRestore_RoundTripsExactly()
        {
            var system = CreateSystem();
            system.EnsureRoom("dorm", RoomWasteRole.Residential);
            system.EnsureRoom("bay", RoomWasteRole.Industrial);
            system.InstallFacility("sanitation_incinerator", "bay");
            system.EmitWaste("bay", WasteType.Chemical, 50f);
            system.TickDaily(3, population: 8);
            var saved = system.CaptureState();
            Assert.Equal(1, saved.schemaVersion);

            var restored = new SanitationSystem();
            restored.BindFacilityCatalog(system.FacilityCatalog);
            restored.RestoreState(saved);

            Assert.Equal(system.GetRoomHygienePermille("dorm"), restored.GetRoomHygienePermille("dorm"));
            Assert.Equal(system.GetRoomHygienePermille("bay"), restored.GetRoomHygienePermille("bay"));
            Assert.Equal(system.GetShelterHygienePermille(), restored.GetShelterHygienePermille());
            Assert.Equal(saved.facilities.Count, restored.State.facilities.Count);
            Assert.Equal(saved.compostQueue.Count, restored.CompostQueue.Count);
            // Spill (if any) is not recreated beyond its persisted state.
            if (saved.activeSpill != null)
            {
                Assert.NotNull(restored.ActiveSpill);
                Assert.Equal(saved.activeSpill.roomId, restored.ActiveSpill!.roomId);
                Assert.Equal(saved.activeSpill.severity, restored.ActiveSpill.severity, 5);
            }
            else
            {
                Assert.Null(restored.ActiveSpill);
            }
        }

        [Fact]
        public void LegacySave_RestoresCleanBaseline_NeverHazardous()
        {
            var restored = new SanitationSystem();
            // Null rooms/facilities/spill (pre-210 save shape) → clean.
            restored.RestoreState(new SanitationState { schemaVersion = 1 });
            Assert.Empty(restored.State.rooms);
            Assert.Equal(1000, restored.GetShelterHygienePermille());
            Assert.Null(restored.ActiveSpill);
            Assert.Equal(1.0f, restored.GetPathogenExposureModifier("dorm"), 4);
        }

        [Fact]
        public void Restore_NewerVersion_ThrowsLoudly()
        {
            var system = new SanitationSystem();
            Assert.Throws<InvalidOperationException>(() =>
                system.RestoreState(new SanitationState { schemaVersion = 2 }));
        }

        // ── Paired determinism ───────────────────────────────────────

        [Fact]
        public void PairedRun_SameInputs_IdenticalState()
        {
            var a = CreateSystem();
            var b = CreateSystem();
            int day = 1;
            for (int i = 0; i < 30; i++)
            {
                foreach (var s in new[] { a, b })
                {
                    s.EnsureRoom("dorm", RoomWasteRole.Residential);
                    s.EnsureRoom("bay", RoomWasteRole.Industrial);
                    if (i == 10) s.EmitWaste("bay", WasteType.Chemical, 12f);
                }
                a.TickDaily(day, population: 8);
                b.TickDaily(day, population: 8);
                Assert.Equal(a.GetShelterHygienePermille(), b.GetShelterHygienePermille());
                Assert.Equal(a.FindRoom("bay")!.chemical, b.FindRoom("bay")!.chemical, 4);
                Assert.Equal(a.FindRoom("dorm")!.organic, b.FindRoom("dorm")!.organic, 4);
                day++;
            }
            var savedA = a.CaptureState();
            var savedB = b.CaptureState();
            Assert.Equal(savedA.rooms.Count, savedB.rooms.Count);
            for (int i = 0; i < savedA.rooms.Count; i++)
            {
                Assert.Equal(savedA.rooms[i].organic, savedB.rooms[i].organic, 5);
                Assert.Equal(savedA.rooms[i].chemical, savedB.rooms[i].chemical, 5);
            }
        }
    }
}
