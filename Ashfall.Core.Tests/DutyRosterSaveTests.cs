// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
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
            Assert.Single(encountersB.ActiveVisitorQueue);
        }

        [Fact]
        public void Restore_WithQuestsAndOverflowAndAssignments_RestoresAllSubsystems()
        {
            var roster = new DutyRosterSystem(908);
            var marks = new MoraleMarkSystem();
            var encounters = new ShelterEncounterSystem(908);
            var clock = new SimClock(1);
            var quests = new DutyRosterQuestRuntime();

            var catalog = new DutyRosterCatalog();
            catalog.Quests.Add(new DutyRosterQuestEntry { id = "quest_test_alpha", min_day = 1 });
            quests.BindCatalog(catalog);

            roster.Unlock(5);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 5);
            roster.TickMorning(5, new List<DutyRosterOccupant>
            {
                new DutyRosterOccupant { survivorId = "sv_kess", displayName = "Kess", occupationObserved = "Mechanic", sleptHere = true }
            });
            roster.ExecuteAssign(DutyRosterIds.RoleNightWatch, "sv_kess", 0, 0, false);
            roster.GrantOverflowAccess();
            roster.RegisterOverflowVisit(DutyRosterIds.LocOverflowAlloc11);

            marks.SetMark("mark_ration_protocol", "payload_protocol", 6);
            encounters.Unlock(5);
            encounters.QueueVisitor(ShelterEncounterSystem.VisitorLen, 6);
            clock.SetDay(15);
            quests.StartQuest("quest_test_alpha", 15);

            var save = DutyRosterSaveCodec.Capture(roster, marks, encounters, clock, quests);
            var json = new SystemTextJsonSerializer();
            var loaded = DutyRosterSaveCodec.Decode(DutyRosterSaveCodec.Encode(save, json), json);

            var rosterB = new DutyRosterSystem(908);
            var marksB = new MoraleMarkSystem();
            var encountersB = new ShelterEncounterSystem(908);
            var clockB = new SimClock(1);
            var questsB = new DutyRosterQuestRuntime();
            questsB.BindCatalog(catalog);

            DutyRosterSaveCodec.Restore(loaded, rosterB, marksB, encountersB, clockB, questsB);

            Assert.Equal(15, clockB.Day);
            Assert.True(rosterB.IsUnlocked);
            Assert.Equal(DutyRosterIds.ScriptPencil, rosterB.ChartScript);
            Assert.True(rosterB.HasVisitedOverflow(DutyRosterIds.LocOverflowAlloc11));
            Assert.Equal("sv_kess", rosterB.GetAssignment(DutyRosterIds.RoleNightWatch));
            Assert.True(marksB.HasMark("mark_ration_protocol"));
            Assert.True(encountersB.IsUnlocked);
            Assert.Equal(ShelterEncounterSystem.VisitorLen, encountersB.PeekVisitor());
            Assert.True(questsB.IsStarted("quest_test_alpha"));
        }

        [Fact]
        public void Restore_NullSave_ThrowsArgumentNullException()
        {
            var roster = new DutyRosterSystem(908);
            var marks = new MoraleMarkSystem();
            var encounters = new ShelterEncounterSystem(908);
            var clock = new SimClock(1);
            var quests = new DutyRosterQuestRuntime();

            Assert.Throws<ArgumentNullException>(() =>
                DutyRosterSaveCodec.Restore(null!, roster, marks, encounters, clock, quests));
        }

        [Fact]
        public void Restore_NullOptionalSubsystems_HandledSafely()
        {
            var roster = new DutyRosterSystem(908);
            roster.Unlock(3);
            var save = DutyRosterSaveCodec.Capture(roster, new MoraleMarkSystem(), new ShelterEncounterSystem(908), new SimClock(3));

            var targetRoster = new DutyRosterSystem(908);
            DutyRosterSaveCodec.Restore(save, targetRoster, null!, null!, null!, null!);

            Assert.True(targetRoster.IsUnlocked);
        }

        [Fact]
        public void Restore_SimDayZeroOrNegative_PreservesExistingClockDay()
        {
            var roster = new DutyRosterSystem(908);
            var save = DutyRosterSaveCodec.Capture(roster, new MoraleMarkSystem(), new ShelterEncounterSystem(908), new SimClock(0));
            save.simDay = 0;

            var clock = new SimClock(42);
            DutyRosterSaveCodec.Restore(save, roster, new MoraleMarkSystem(), new ShelterEncounterSystem(908), clock);

            Assert.Equal(42, clock.Day);
        }

        [Fact]
        public void Restore_Idempotent_CanBeCalledRepeatedlyWithoutDuplicatingState()
        {
            var roster = new DutyRosterSystem(908);
            var marks = new MoraleMarkSystem();
            var encounters = new ShelterEncounterSystem(908);
            var clock = new SimClock(10);
            roster.Unlock(5);
            marks.SetMark("mark_ration_protocol", "test", 5);

            var save = DutyRosterSaveCodec.Capture(roster, marks, encounters, clock);

            var targetRoster = new DutyRosterSystem(908);
            var targetMarks = new MoraleMarkSystem();
            var targetEncounters = new ShelterEncounterSystem(908);
            var targetClock = new SimClock(1);

            DutyRosterSaveCodec.Restore(save, targetRoster, targetMarks, targetEncounters, targetClock);
            DutyRosterSaveCodec.Restore(save, targetRoster, targetMarks, targetEncounters, targetClock);

            Assert.Single(targetMarks.State.marks);
            Assert.Equal(10, targetClock.Day);
            Assert.True(targetRoster.IsUnlocked);
        }

        [Fact]
        public void HostSession_RestoreSave_Contract_DelegatesToAllSubsystems()
        {
            var roster = new DutyRosterSystem(908);
            var marks = new MoraleMarkSystem();
            var encounters = new ShelterEncounterSystem(908);
            var clock = new SimClock(1);
            var quests = new DutyRosterQuestRuntime();

            roster.Unlock(5);
            marks.SetMark("mark_ration_protocol", "delegation_test", 5);
            encounters.Unlock(5);
            clock.SetDay(25);

            var save = DutyRosterSaveCodec.Capture(roster, marks, encounters, clock, quests);

            var freshRoster = new DutyRosterSystem(908);
            var freshMarks = new MoraleMarkSystem();
            var freshEncounters = new ShelterEncounterSystem(908);
            var freshClock = new SimClock(1);
            var freshQuests = new DutyRosterQuestRuntime();

            Action<DutyRosterSave> restoreSave = s =>
                DutyRosterSaveCodec.Restore(s, freshRoster, freshMarks, freshEncounters, freshClock, freshQuests);

            restoreSave(save);

            Assert.Equal(25, freshClock.Day);
            Assert.True(freshRoster.IsUnlocked);
            Assert.True(freshEncounters.IsUnlocked);
            Assert.True(freshMarks.HasMark("mark_ration_protocol"));
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
