using System.Collections.Generic;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Regression gate for the save-envelope aliasing class of bug:
    /// RestoreState / CaptureState must deep-copy, so a mutation of the live
    /// system (or of the caller's envelope) can never corrupt the other side.
    /// </summary>
    public class SaveAliasingRegressionTests
    {
        private static readonly IJsonSerializer Json = new SystemTextJsonSerializer();

        [Fact]
        public void WaystationRestoreDoesNotAliasWatchArray()
        {
            var sys = new WaystationSystem();
            sys.Unlock();
            sys.AssignWatch(new[] { "elena_vasquez", "marcus_olejnik" });
            var envelope = sys.CaptureState();

            var restored = new WaystationSystem();
            restored.RestoreState(CloneViaJson(envelope));

            Assert.False(ReferenceEquals(envelope.watchSurvivorIds, restored.State.watchSurvivorIds),
                "live watch array must not alias the envelope");
            restored.AssignWatch(new[] { "suki_tanaka" });
            Assert.Equal("suki_tanaka", restored.State.watchSurvivorIds[0]);
            Assert.Contains("elena_vasquez", envelope.watchSurvivorIds);
        }

        [Fact]
        public void DutyRosterRestoreDoesNotAliasRowsOrAssignments()
        {
            var sys = new DutyRosterSystem(808);
            sys.Unlock(5);
            sys.WriteName("elena_vasquez", "Elena", "scrounger",
                DutyRosterIds.ScriptPencil, 6, true);
            sys.WriteName("marcus_olejnik", "Marcus", "machinist",
                DutyRosterIds.ScriptPencil, 6, true);
            sys.Assign(DutyRosterIds.RoleNightWatch, "elena_vasquez");
            var envelope = sys.CaptureState();

            var restored = new DutyRosterSystem();
            restored.RestoreState(CloneViaJson(envelope));

            Assert.False(ReferenceEquals(envelope.rows, restored.State.rows));
            Assert.False(ReferenceEquals(envelope.assignments, restored.State.assignments));
            restored.EraseName("elena_vasquez");
            Assert.Contains(envelope.rows, r => r != null && r.survivorId == "elena_vasquez");
        }

        [Fact]
        public void MoraleMarkRestoreDoesNotAliasMarks()
        {
            var sys = new MoraleMarkSystem();
            sys.SetMark("mark_tag_burned", "payload", 12);
            var envelope = sys.CaptureState();

            var restored = new MoraleMarkSystem();
            restored.RestoreState(CloneViaJson(envelope));

            Assert.False(ReferenceEquals(envelope.marks, restored.State.marks));
            restored.ClearMark("mark_tag_burned");
            Assert.Single(envelope.marks);
            Assert.Equal("payload", envelope.marks[0].payload);
        }

        [Fact]
        public void ShelterEncounterRestoreDoesNotAliasQueues()
        {
            var sys = new ShelterEncounterSystem(1808);
            sys.Unlock(100);
            sys.QueueVisitor("visitor_len", 100);
            sys.StartEncounter("enc_shelter_len_01", ShelterEncounterSystem.KindNightSlate, 100);
            var envelope = sys.CaptureState();

            var restored = new ShelterEncounterSystem();
            restored.RestoreState(CloneViaJson(envelope));

            Assert.False(ReferenceEquals(envelope.activeVisitorQueue, restored.State.activeVisitorQueue));
            Assert.False(ReferenceEquals(envelope.resolvedIds, restored.State.resolvedIds));
            Assert.False(ReferenceEquals(envelope.history, restored.State.history));
        }

        [Fact]
        public void SiteEncounterRestoreDoesNotAliasHistory()
        {
            var sys = new SiteEncounterSystem(1808);
            sys.Unlock();
            sys.StartEncounter("enc_site_gauge_read", "room_lock_gauges",
                SiteEncounterSystem.KindGaugeRead, 90, "mutation_lock_gauges_filed");
            sys.ResolveEncounter("enc_site_gauge_read", 90);
            var envelope = sys.CaptureState();

            var restored = new SiteEncounterSystem();
            restored.RestoreState(CloneViaJson(envelope));

            Assert.False(ReferenceEquals(envelope.resolvedIds, restored.State.resolvedIds));
            Assert.False(ReferenceEquals(envelope.history, restored.State.history));
            restored.StartEncounter("enc_site_transit_02", "room_transit_turnstiles",
                SiteEncounterSystem.KindGaugeRead, 91);
            Assert.DoesNotContain(envelope.resolvedIds, id => id == "enc_site_transit_02");
        }

        [Fact]
        public void CrossingArbitrationRestoreDoesNotAliasLists()
        {
            var sys = new CrossingArbitrationSystem();
            sys.LoadBackerPool(new List<BackerDef>
            {
                new BackerDef { id = "npc_osran_kell", principled = true },
                new BackerDef { id = "npc_mattis_cray", principled = true },
                new BackerDef { id = "npc_bram_ostrowski" }
            });
            sys.CallStanding("quest_crossing_the_standing", 40);
            var envelope = sys.CaptureState();

            var restored = new CrossingArbitrationSystem();
            restored.RestoreState(CloneViaJson(envelope));

            Assert.False(ReferenceEquals(envelope.backerPool, restored.State.backerPool));
            Assert.False(ReferenceEquals(envelope.rulings, restored.State.rulings));
            // Live mutation must leave the envelope's rulings untouched.
            int before = envelope.rulings.Count;
            sys.CallStanding("quest_crossing_the_standing", 60);
            Assert.True(before == envelope.rulings.Count,
                "mutating the live system must not grow the captured envelope");
        }

        [Fact]
        public void LocationLayoutRestoreDoesNotAliasParents()
        {
            var sys = new LocationLayoutSystem(new FileSystemIO(), Json);
            sys.Unlock();
            sys.ArriveAtParent("loc_cut_kilometre_19");
            var envelope = sys.CaptureState();

            var restored = new LocationLayoutSystem(new FileSystemIO(), Json);
            restored.RestoreState(CloneViaJson(envelope));

            Assert.False(ReferenceEquals(envelope.parents, restored.State.parents));
            if (envelope.parents.Count > 0 && restored.State.parents.Count > 0)
                Assert.False(ReferenceEquals(envelope.parents[0], restored.State.parents[0]));
        }

        [Fact]
        public void LocationMemoryRestoreDoesNotAliasFlags()
        {
            var sys = new LocationMemorySystem(new FileSystemIO(), Json);
            sys.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
            var envelope = sys.CaptureState();

            var restored = new LocationMemorySystem(new FileSystemIO(), Json);
            restored.RestoreState(CloneViaJson(envelope));

            Assert.False(ReferenceEquals(envelope.activeFlags, restored.State.activeFlags));
            Assert.False(ReferenceEquals(envelope.strata, restored.State.strata));
            restored.ApplyMutation(LocationMemorySystem.MutationPumpLive);
            Assert.DoesNotContain(envelope.activeFlags, f => f == LocationMemorySystem.MutationPumpLive);
        }

        [Fact]
        public void GreenhouseCaptureReturnsSnapshotNotLiveState()
        {
            var sys = new GreenhouseSystem(1);
            sys.EnsurePlots(2);
            sys.Plant(0, "item_seed_tuber", 5, out _);
            var envelope = sys.CaptureState();

            Assert.False(ReferenceEquals(envelope.plots, sys.State.plots),
                "capture must not alias the live plots list");
            sys.Plant(1, "item_seed_tuber", 6, out _);
            Assert.Equal(5, envelope.plots[0].plantedDay);
            Assert.True(string.IsNullOrEmpty(envelope.plots[1].seedItemId),
                "mutating the live system must not leak into the captured envelope");
        }

        private static T CloneViaJson<T>(T state) where T : class
        {
            string blob = Json.Serialize(state);
            return Json.Deserialize<T>(blob);
        }
    }
}
