using System;
using System.Collections.Generic;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL: THE DUTY ROSTER — thin Godot-host session.
    /// Wraps the engine-agnostic DutyRosterSystem, MoraleMarkSystem and
    /// ShelterEncounterSystem. Does not invent rules. Player-visible state:
    /// the chart, the marks, the encounters, the Second Winter.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §5.
    /// </summary>
    public sealed class DutyRosterHostSession
    {
        public const int DefaultSeed = 908; // roster seed offset: _worldSeed + 1208 style

        public DutyRosterSystem Roster { get; }
        public MoraleMarkSystem Marks { get; }
        public ShelterEncounterSystem Encounters { get; }
        public DutyRosterCatalog Catalog { get; }
        public SimClock Clock { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public int LocationCount => Catalog.Locations.Count;
        public int QuestCount => Catalog.Quests.Count;
        public int MarkCount => Catalog.Marks.Count;
        public int SeasonCount => Catalog.Seasons.Count;

        public DutyRosterHostSession(
            DutyRosterSystem roster,
            MoraleMarkSystem marks,
            ShelterEncounterSystem encounters,
            DutyRosterCatalog catalog,
            SimClock clock)
        {
            Roster = roster;
            Marks = marks;
            Encounters = encounters;
            Catalog = catalog;
            Clock = clock;

            Roster.OnRosterUpdated += () => LastEvent = "wall updated";
            Roster.OnNameWritten += id => LastEvent = "name written: " + id;
            Roster.OnNameErased += id => LastEvent = "name erased: " + id;
            Roster.OnRosterBurned += () => LastEvent = "CHART BURNED";
            Encounters.OnShelterEncounterStarted += rec =>
                LastEvent = "encounter: " + rec.kind + " (" + (rec.visitorId ?? "none") + ")";

            // Persistence: any system-level state change marks the save dirty.
            Roster.OnStateChanged += _ => StateChanged?.Invoke();
            Marks.OnStateChanged += _ => StateChanged?.Invoke();
            Encounters.OnStateChanged += _ => StateChanged?.Invoke();
        }

        /// <summary>Raised when any roster/mark/encounter state changes (save dirty flag).</summary>
        public event Action StateChanged;

        public static DutyRosterHostSession Create(string dataDirectory, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log ??= new GodotLog();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new DutyRosterCatalogLoader(files, json, log);
            var catalog = loader.Load(dataDirectory);

            var roster = new DutyRosterSystem(DefaultSeed);
            var marks = new MoraleMarkSystem();
            marks.BindCatalog(catalog);
            var encounters = new ShelterEncounterSystem(DefaultSeed);
            var clock = new SimClock(1);
            return new DutyRosterHostSession(roster, marks, encounters, catalog, clock);
        }

        public void Unlock(int day)
        {
            if (!Roster.IsUnlocked)
                Roster.Unlock(day);
            if (!Encounters.IsUnlocked)
                Encounters.Unlock(day);
        }

        /// <summary>Cross-host save envelope. Shape and checksum owned by DutyRosterSaveCodec.</summary>
        public DutyRosterSave CaptureSave() =>
            DutyRosterSaveCodec.Capture(Roster, Marks, Encounters, Clock);

        public void RestoreSave(DutyRosterSave save) =>
            DutyRosterSaveCodec.Restore(save, Roster, Marks, Encounters, Clock);

        /// <summary>Morning tick: pencil fills rows from the demo occupants.</summary>
        public string TickDay()
        {
            LastEvent = string.Empty;
            int day = Clock.Day;
            Clock.AdvanceDays(1);
            Unlock(day);

            if (!Roster.State.wallInspected)
                Roster.NotifyWallInspected();

            var occupants = DemoOccupants();
            Roster.TickMorning(Clock.Day, occupants);

            // Shelter encounters: one per night; morning bookkeeping resets the counter.
            Encounters.ResetNightCounter(Clock.Day);

            return string.IsNullOrEmpty(LastEvent) ? "morning row checked" : LastEvent;
        }

        /// <summary>Player action: inspect the wall. Returns the roster card prose.</summary>
        public string InspectWall()
        {
            var wall = Catalog.GetLocation(DutyRosterSystem.LocStackRosterWall);
            if (!Roster.State.wallInspected)
                Roster.NotifyWallInspected();
            return wall != null ? wall.inspect + "\n\n" + wall.description : "No chart here.";
        }

        /// <summary>Player action: the chart choice (pencil / blank / wait ink).</summary>
        public string ResolveChart(string choiceId)
        {
            return Roster.ResolveChartChoice(choiceId, Clock.Day)
                ? "chart: " + choiceId
                : "chart: not resolved";
        }

        /// <summary>Player action: ink the wall (ending).</summary>
        public string ResolveInk()
        {
            return Roster.ResolveInkEnding(Clock.Day)
                ? "ink ending written: " + Roster.State.endingId
                : "ink ending not available";
        }

        /// <summary>Player action: burn the chart.</summary>
        public string BurnChart()
        {
            return Roster.BurnChart(Clock.Day) ? "chart burned" : "cannot burn";
        }

        /// <summary>Player action: queue a visitor at the hatch.</summary>
        public string QueueVisitor(string visitorId)
        {
            return Encounters.QueueVisitor(visitorId, Clock.Day)
                ? "visitor waiting: " + visitorId
                : "visitor not queued";
        }

        /// <summary>Player action: start a shelter encounter by kind.</summary>
        public string StartEncounter(string kind)
        {
            string id = "se_" + kind + "_" + Clock.Day;
            return Encounters.StartEncounter(id, kind, Clock.Day)
                ? "encounter started: " + kind
                : "no encounter tonight";
        }

        /// <summary>Second Winter: shorten windows + raise encounter weight.</summary>
        public string ActivateSecondWinter()
        {
            if (Roster.IsSecondWinterActive)
                return "second winter already active";
            Roster.SetSecondWinterActive(true);
            Encounters.SetSecondWinter(DutyRosterSystem.SecondWinterEncounterWeight, Clock.Day);
            Marks.SetMark("mark_second_winter", null, Clock.Day);
            LastEvent = "SECOND WINTER";
            return "second winter active: windows 8-12d, encounters x" + DutyRosterSystem.SecondWinterEncounterWeight;
        }

        public string WallLine()
        {
            string script = Roster.ChartScript;
            string rows = Roster.OccupiedRowCount + "/14";
            string ending = string.IsNullOrEmpty(Roster.State.endingId) ? "" : " · " + Roster.State.endingId;
            return
                $"Roster: {script} · {rows} · " +
                $"blank rows {Roster.State.daysLeftBlank}d · " +
                $"marks {Marks.Count} · second winter {(Roster.IsSecondWinterActive ? "ACTIVE" : "no")}" + ending;
        }

        public string EncountersLine()
        {
            string visitor = Encounters.PeekVisitor() ?? "none";
            return
                $"Encounters: weight x{Encounters.EncounterWeightMultiplier:0.0} · " +
                $"visitors {Encounters.ActiveVisitorQueue.Count} (next {visitor}) · " +
                $"last {Encounters.LastEncounterDay}";
        }

        public string MarksLine()
        {
            var state = Marks.State;
            if (state == null || state.marks == null || state.marks.Count == 0)
                return "Marks: none yet. The wall is blank. Later prose will come.";
            var sb = new System.Text.StringBuilder("Marks: ");
            for (int i = 0; i < state.marks.Count && i < 5; i++)
            {
                if (i > 0) sb.Append(" · ");
                var rec = state.marks[i];
                string later = Marks.GetLaterProse(rec.id);
                sb.Append(string.IsNullOrEmpty(later) ? rec.id : later);
            }
            if (state.marks.Count > 5)
                sb.Append(" · +" + (state.marks.Count - 5) + " more");
            return sb.ToString();
        }

        private static List<DutyRosterOccupant> DemoOccupants()
        {
            return new List<DutyRosterOccupant>
            {
                new DutyRosterOccupant { survivorId = "npc_kess_adler", displayName = "Kess Adler", occupationObserved = "records_clerk", sleptHere = true },
                new DutyRosterOccupant { survivorId = "npc_ansel_duth", displayName = "Ansel Duth", occupationObserved = "parent", sleptHere = true },
                new DutyRosterOccupant { survivorId = "npc_hadi_morrow", displayName = "Hadi Morrow", occupationObserved = "veterinary_assistant", sleptHere = true },
                new DutyRosterOccupant { survivorId = "npc_tamsin_rook", displayName = "Tamsin Rook", occupationObserved = "harbour_night_clerk", sleptHere = true }
            };
        }

        public string CatalogLine()
        {
            if (LocationCount == 0 && QuestCount == 0)
                return "Duty Roster catalog: empty - check ASHFALL_DATA / Assets/StreamingAssets/Data";
            return $"Duty Roster: {LocationCount} locations · {QuestCount} quests · {MarkCount} marks · {SeasonCount} seasons";
        }
    }
}