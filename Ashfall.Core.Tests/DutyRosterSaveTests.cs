using System;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Codec roundtrip and integrity tests for the Duty Roster save envelope
    /// (Ashfall.Core.DutyRosterSave / DutyRosterSaveCodec).
    /// </summary>
    public class DutyRosterSaveTests
    {
        [Fact]
        public void RoundTrip_RestoresRosterMarksAndEncounters()
        {
            var roster = new DutyRosterSystem(908);
            var marks = new MoraleMarkSystem();
            var encounters = new ShelterEncounterSystem(908);
            var clock = new SimClock(7);

            roster.Unlock(5);
            marks.SetMark("mark_bowl_cold", "test payload", 6);
            encounters.Unlock(5);
            encounters.QueueVisitor(ShelterEncounterSystem.VisitorLen, 6);

            var save = DutyRosterSaveCodec.Capture(roster, marks, encounters, clock);
            var json = new SystemTextJsonSerializer();
            var loaded = DutyRosterSaveCodec.Decode(
                DutyRosterSaveCodec.Encode(save, json), json);

            var rosterB = new DutyRosterSystem(908);
            var marksB = new MoraleMarkSystem();
            var encountersB = new ShelterEncounterSystem(908);
            var clockB = new SimClock(1);
            DutyRosterSaveCodec.Restore(loaded, rosterB, marksB, encountersB, clockB);

            Assert.Equal(7, clockB.Day);
            Assert.True(rosterB.IsUnlocked);
            Assert.True(encountersB.IsUnlocked);
            Assert.Single(marksB.State.marks);
            Assert.Equal("mark_bowl_cold", marksB.State.marks[0].id);
            Assert.Equal(1, encountersB.ActiveVisitorQueue.Count);
        }

        [Fact]
        public void Decode_RejectsTamperedChecksum()
        {
            var save = DutyRosterSaveCodec.Capture(
                new DutyRosterSystem(908),
                new MoraleMarkSystem(),
                new ShelterEncounterSystem(908),
                new SimClock(3));
            var json = new SystemTextJsonSerializer();
            string text = DutyRosterSaveCodec.Encode(save, json);

            string tampered = text.Replace("\"simDay\":3", "\"simDay\":99");
            Assert.NotEqual(text, tampered);
            Assert.Throws<InvalidOperationException>(
                () => DutyRosterSaveCodec.Decode(tampered, json));
        }

        [Fact]
        public void Decode_RejectsNewerVersion()
        {
            var save = DutyRosterSaveCodec.Capture(
                new DutyRosterSystem(908),
                new MoraleMarkSystem(),
                new ShelterEncounterSystem(908),
                new SimClock(3));
            save.saveVersion = DutyRosterSave.CurrentSaveVersion + 1;
            save.Checksum = string.Empty;
            var json = new SystemTextJsonSerializer();

            Assert.Throws<InvalidOperationException>(
                () => DutyRosterSaveCodec.Decode(DutyRosterSaveCodec.Encode(save, json), json));
        }
    }
}
