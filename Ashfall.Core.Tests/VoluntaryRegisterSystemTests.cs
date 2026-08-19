using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class VoluntaryRegisterSystemTests
    {
        private static VolunteerEntry FindEntry(System.Collections.Generic.IReadOnlyList<VolunteerEntry> entries, string survivorId)
        {
            for (int i = 0; i < entries.Count; i++)
                if (entries[i].survivorId == survivorId) return entries[i];
            return null;
        }

        [Fact]
        public void Volunteer_AppendsPendingEntryAndFiresEvent()
        {
            var sys = new VoluntaryRegisterSystem();
            int fired = 0;
            sys.OnVolunteered += (sv, task) => fired++;
            Assert.True(sys.Volunteer("sv_mae", "water_haul", 240, "I can carry the yoke."));
            Assert.Equal(1, fired);
            var entry = FindEntry(sys.Entries, "sv_mae");
            Assert.NotNull(entry);
            Assert.Equal("water_haul", entry.task);
            Assert.Equal(240, entry.acceptedDay);
            Assert.Equal(-1, entry.completedDay);
            Assert.False(entry.completed);
            Assert.Equal("I can carry the yoke.", entry.reasonText);
        }

        [Fact]
        public void Volunteer_NullSurvivorRejected()
        {
            var sys = new VoluntaryRegisterSystem();
            Assert.False(sys.Volunteer(null, "water_haul", 240));
            Assert.False(sys.Volunteer("", "water_haul", 240));
            Assert.Empty(sys.Entries);
        }

        [Fact]
        public void CompleteVolunteer_ClosesEntryAndBanksDose()
        {
            var sys = new VoluntaryRegisterSystem();
            sys.Volunteer("sv_mae", "water_haul", 240);
            int fired = 0;
            sys.OnVolunteerCompleted += (sv, dose) => fired++;
            Assert.True(sys.CompleteVolunteer("sv_mae", "water_haul", 12.5f, 245));
            Assert.Equal(1, fired);
            var entry = FindEntry(sys.Entries, "sv_mae");
            Assert.True(entry.completed);
            Assert.Equal(245, entry.completedDay);
            Assert.Equal(12.5f, entry.doseIncurred);
        }

        [Fact]
        public void CompleteVolunteer_UnknownEntryRejected()
        {
            var sys = new VoluntaryRegisterSystem();
            Assert.False(sys.CompleteVolunteer("sv_mae", "water_haul", 12.5f, 245));
        }

        [Fact]
        public void CompleteVolunteer_SecondCompletionRejected()
        {
            var sys = new VoluntaryRegisterSystem();
            sys.Volunteer("sv_mae", "water_haul", 240);
            Assert.True(sys.CompleteVolunteer("sv_mae", "water_haul", 12.5f, 245));
            Assert.False(sys.CompleteVolunteer("sv_mae", "water_haul", 3f, 246));
            Assert.Equal(12.5f, sys.Entries[0].doseIncurred);
        }

        [Fact]
        public void Volunteer_SameTaskTwiceRefused_NoStateDivergence()
        {
            var sys = new VoluntaryRegisterSystem();
            Assert.True(sys.Volunteer("sv_mae", "water_haul", 240));
            Assert.False(sys.Volunteer("sv_mae", "water_haul", 242));
            Assert.Single(sys.Entries);
            Assert.Single(sys.CaptureState().entries);
            // Live and saved must agree (no silent data loss on save).
            Assert.Single(sys.Entries);
            Assert.Single(sys.CaptureState().entries);
        }

        [Fact]
        public void Volunteer_SameSurvivorDifferentTaskAllowed()
        {
            var sys = new VoluntaryRegisterSystem();
            Assert.True(sys.Volunteer("sv_mae", "water_haul", 240));
            Assert.True(sys.Volunteer("sv_mae", "night_watch", 241));
            Assert.Equal(2, sys.Entries.Count);
            Assert.Equal(2, sys.CaptureState().entries.Count);
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = new VoluntaryRegisterSystem();
            sys.Volunteer("sv_mae", "water_haul", 240);
            var snapshot = sys.CaptureState();
            snapshot.entries[0].completed = true;
            snapshot.entries[0].doseIncurred = 99f;
            Assert.False(sys.Entries[0].completed);
            Assert.Equal(0f, sys.Entries[0].doseIncurred);
        }

        [Fact]
        public void CaptureState_EmitsInOrdinalOrder()
        {
            var sys = new VoluntaryRegisterSystem();
            sys.Volunteer("sv_zed", "task_b", 240);
            sys.Volunteer("sv_a", "task_a", 240);
            var snapshot = sys.CaptureState();
            Assert.Equal("sv_a", snapshot.entries[0].survivorId);
            Assert.Equal("sv_zed", snapshot.entries[1].survivorId);
        }

        [Fact]
        public void SaveLoad_RoundTripsAllState()
        {
            var sys = new VoluntaryRegisterSystem();
            sys.Volunteer("sv_mae", "water_haul", 240);
            sys.CompleteVolunteer("sv_mae", "water_haul", 12.5f, 245);
            sys.Volunteer("sv_ged", "night_watch", 250, "Someone has to watch.");

            var restored = new VoluntaryRegisterSystem();
            restored.RestoreState(sys.CaptureState());

            Assert.Equal(2, restored.Entries.Count);
            var mae = FindEntry(restored.Entries, "sv_mae");
            Assert.True(mae.completed);
            Assert.Equal(12.5f, mae.doseIncurred);
            var ged = FindEntry(restored.Entries, "sv_ged");
            Assert.False(ged.completed);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = new VoluntaryRegisterSystem();
            sys.Volunteer("sv_a", "task_a", 240);
            sys.Volunteer("sv_b", "task_b", 241);
            sys.CompleteVolunteer("sv_a", "task_a", 5f, 245);
            string before = SaveChecksum.Compute(sys.CaptureState());

            var restored = new VoluntaryRegisterSystem();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());

            Assert.Equal(before, after);
        }

        [Fact]
        public void SaveLoad_RestoreDoesNotAliasEnvelope()
        {
            var sys = new VoluntaryRegisterSystem();
            sys.Volunteer("sv_mae", "water_haul", 240);
            var snapshot = sys.CaptureState();

            var restored = new VoluntaryRegisterSystem();
            restored.RestoreState(snapshot);

            // Mutating the envelope after restore must not touch live state.
            snapshot.entries.Clear();
            snapshot.entries.Add(new VolunteerEntry { survivorId = "sv_ghost", task = "x", acceptedDay = 1 });

            Assert.Single(restored.Entries);
            Assert.NotNull(FindEntry(restored.Entries, "sv_mae"));
            Assert.Null(FindEntry(restored.Entries, "sv_ghost"));
        }
    }
}
