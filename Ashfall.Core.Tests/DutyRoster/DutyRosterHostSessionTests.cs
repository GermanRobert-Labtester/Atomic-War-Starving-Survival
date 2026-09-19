// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.DutyRoster;
using Xunit;

namespace Ashfall.Core.Tests.DutyRoster
{
    /// <summary>
    /// Tests for the DutyRosterHostSession.RestoreSave contract:
    /// validating that restoring a DutyRosterSave re-populates the internal
    /// subsystems (Roster, MoraleMarks, ShelterEncounters, SimClock, Quests),
    /// raises state changes, handles edge cases (null save, simDay 0), and is idempotent.
    /// </summary>
    public class DutyRosterHostSessionTests
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

        private sealed class TestDutyRosterHostSession : StatefulSessionBase
        {
            public const int DefaultSeed = 908;

            public DutyRosterSystem Roster { get; }
            public MoraleMarkSystem Marks { get; }
            public ShelterEncounterSystem Encounters { get; }
            public DutyRosterQuestRuntime Quests { get; }
            public DutyRosterCatalog Catalog { get; }
            public SimClock Clock { get; }
            public string LastEvent { get; private set; } = string.Empty;

            public TestDutyRosterHostSession(
                DutyRosterSystem? roster = null,
                MoraleMarkSystem? marks = null,
                ShelterEncounterSystem? encounters = null,
                DutyRosterCatalog? catalog = null,
                SimClock? clock = null,
                DutyRosterQuestRuntime? quests = null)
            {
                Roster = roster ?? new DutyRosterSystem(DefaultSeed);
                Marks = marks ?? new MoraleMarkSystem();
                Encounters = encounters ?? new ShelterEncounterSystem(DefaultSeed);
                Catalog = catalog ?? new DutyRosterCatalog();
                Clock = clock ?? new SimClock(1);
                Quests = quests ?? new DutyRosterQuestRuntime();
                Quests.BindCatalog(Catalog);

                Roster.OnStateChanged += _ => RaiseStateChanged();
                Marks.OnStateChanged += _ => RaiseStateChanged();
                Encounters.OnStateChanged += _ => RaiseStateChanged();
                Quests.OnStateChanged += _ => RaiseStateChanged();
            }

            public static TestDutyRosterHostSession Create(string dataDirectory)
            {
                CatalogLocator.UseInvariantCulture();
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                var loader = new DutyRosterCatalogLoader(files, json, NullLog.Instance);
                var catalog = loader.Load(dataDirectory);

                var roster = new DutyRosterSystem(DefaultSeed);
                var marks = new MoraleMarkSystem();
                marks.BindCatalog(catalog);
                var encounters = new ShelterEncounterSystem(DefaultSeed);
                var clock = new SimClock(1);
                return new TestDutyRosterHostSession(roster, marks, encounters, catalog, clock);
            }

            public void Unlock(int day)
            {
                if (!Roster.IsUnlocked) Roster.Unlock(day);
                if (!Encounters.IsUnlocked) Encounters.Unlock(day);
            }

            public DutyRosterSave CaptureSave() =>
                DutyRosterSaveCodec.Capture(Roster, Marks, Encounters, Clock, Quests);

            public void RestoreSave(DutyRosterSave save) =>
                DutyRosterSaveCodec.Restore(save, Roster, Marks, Encounters, Clock, Quests);
        }

        [Fact]
        public void RestoreSave_RestoresClockRosterMarksAndEncounters()
        {
            var session = new TestDutyRosterHostSession();
            session.Unlock(5);
            session.Marks.SetMark("mark_bowl_cold", "test payload", 6);
            session.Encounters.QueueVisitor(ShelterEncounterSystem.VisitorLen, 6);
            session.Clock.SetDay(7);

            var save = session.CaptureSave();

            var fresh = new TestDutyRosterHostSession();
            bool stateChanged = false;
            fresh.StateChanged += () => stateChanged = true;

            fresh.RestoreSave(save);

            Assert.Equal(7, fresh.Clock.Day);
            Assert.True(fresh.Roster.IsUnlocked);
            Assert.True(fresh.Encounters.IsUnlocked);
            Assert.True(fresh.Marks.HasMark("mark_bowl_cold"));
            Assert.Equal(ShelterEncounterSystem.VisitorLen, fresh.Encounters.PeekVisitor());
            Assert.True(stateChanged);
            Assert.True(fresh.IsDirty);
        }

        [Fact]
        public void RestoreSave_WithQuestsAndOverflowAndAssignments_RestoresCompleteState()
        {
            string dataDir = GetDataDir();
            var session = TestDutyRosterHostSession.Create(dataDir);
            session.Unlock(5);
            session.Roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 5);
            session.Roster.TickMorning(5, new List<DutyRosterOccupant>
            {
                new DutyRosterOccupant { survivorId = "sv_kess", displayName = "Kess", occupationObserved = "Mechanic", sleptHere = true }
            });
            session.Roster.ExecuteAssign(DutyRosterIds.RoleNightWatch, "sv_kess", 0, 0, false);
            session.Roster.GrantOverflowAccess();
            session.Roster.RegisterOverflowVisit(DutyRosterIds.LocOverflowAlloc11);

            session.Marks.SetMark("mark_ration_protocol", "payload", 5);
            session.Clock.SetDay(60);
            session.Quests.StartQuest(DutyRosterIds.QuestTheChart, 60);

            var save = session.CaptureSave();

            var fresh = TestDutyRosterHostSession.Create(dataDir);
            fresh.RestoreSave(save);

            Assert.Equal(60, fresh.Clock.Day);
            Assert.True(fresh.Roster.IsUnlocked);
            Assert.Equal(DutyRosterIds.ScriptPencil, fresh.Roster.ChartScript);
            Assert.True(fresh.Roster.HasVisitedOverflow(DutyRosterIds.LocOverflowAlloc11));
            Assert.Equal("sv_kess", fresh.Roster.GetAssignment(DutyRosterIds.RoleNightWatch));
            Assert.True(fresh.Marks.HasMark("mark_ration_protocol"));
            Assert.True(fresh.Quests.IsStarted(DutyRosterIds.QuestTheChart));
        }

        [Fact]
        public void RestoreSave_NullSave_ThrowsArgumentNullException()
        {
            var session = new TestDutyRosterHostSession();
            Assert.Throws<ArgumentNullException>(() => session.RestoreSave(null!));
        }

        [Fact]
        public void RestoreSave_Idempotent_CanBeCalledMultipleTimesWithoutDuplication()
        {
            var session = new TestDutyRosterHostSession();
            session.Unlock(5);
            session.Marks.SetMark("mark_ration_protocol", "payload", 5);
            session.Clock.SetDay(10);

            var save = session.CaptureSave();

            var fresh = new TestDutyRosterHostSession();
            fresh.RestoreSave(save);
            fresh.RestoreSave(save);

            Assert.Equal(10, fresh.Clock.Day);
            Assert.Single(fresh.Marks.State.marks);
            Assert.True(fresh.Roster.IsUnlocked);
        }

        [Fact]
        public void RestoreSave_SimDayZero_PreservesExistingClockDay()
        {
            var session = new TestDutyRosterHostSession();
            var save = session.CaptureSave();
            save.simDay = 0;

            var fresh = new TestDutyRosterHostSession(clock: new SimClock(35));
            fresh.RestoreSave(save);

            Assert.Equal(35, fresh.Clock.Day);
        }

        [Fact]
        public void RestoreSave_RoundtripThroughSerializationCodec()
        {
            var session = new TestDutyRosterHostSession();
            session.Unlock(5);
            session.Marks.SetMark("mark_ration_protocol", "payload", 5);
            session.Encounters.QueueVisitor(ShelterEncounterSystem.VisitorEdor, 5);
            session.Clock.SetDay(12);

            var save = session.CaptureSave();

            var json = new SystemTextJsonSerializer();
            string text = DutyRosterSaveCodec.Encode(save, json);
            var decoded = DutyRosterSaveCodec.Decode(text, json);

            var fresh = new TestDutyRosterHostSession();
            fresh.RestoreSave(decoded);

            Assert.Equal(12, fresh.Clock.Day);
            Assert.True(fresh.Roster.IsUnlocked);
            Assert.True(fresh.Marks.HasMark("mark_ration_protocol"));
            Assert.Equal(ShelterEncounterSystem.VisitorEdor, fresh.Encounters.PeekVisitor());
        }
    }
}
