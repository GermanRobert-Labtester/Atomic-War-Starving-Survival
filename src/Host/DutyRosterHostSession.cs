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
        public DutyRosterQuestRuntime Quests { get; }
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
            SimClock clock,
            DutyRosterQuestRuntime quests = null,
            Ashfall.Core.Journal.JournalSystem journal = null,
            ILog log = null)
        {
            _log = log ?? NullLog.Instance;
            Roster = roster;
            Marks = marks;
            Encounters = encounters;
            Catalog = catalog;
            Clock = clock;
            Quests = quests ?? new DutyRosterQuestRuntime();
            Quests.BindCatalog(catalog);
            _journal = journal;

            Roster.OnRosterUpdated += () => LastEvent = "wall updated";
            Roster.OnNameWritten += id => LastEvent = "name written: " + id;
            Roster.OnNameErased += id => LastEvent = "name erased: " + id;
            Roster.OnRosterBurned += () => LastEvent = "CHART BURNED";
            Encounters.OnShelterEncounterStarted += rec =>
                LastEvent = "encounter: " + rec.kind + " (" + (rec.visitorId ?? "none") + ")";
            Quests.OnQuestStarted += p => LastEvent = "quest started: " + p.questId;
            Quests.OnQuestCompleted += p =>
            {
                LastEvent = "quest complete: " + p.questId;
                Quests.ApplyKnownEffects(Roster, Marks, p.completedDay, log!);
                BridgeQuestKnowledge(p);
            };
            Quests.OnQuestFailed += p =>
            {
                LastEvent = "quest failed: " + p.questId;
                Quests.ApplyKnownEffects(Roster, Marks, p.failedDay, log!);
            };

            // Persistence: any system-level state change marks the save dirty.
            Roster.OnStateChanged += _ => StateChanged?.Invoke();
            Marks.OnStateChanged += _ => StateChanged?.Invoke();
            Encounters.OnStateChanged += _ => StateChanged?.Invoke();
            Quests.OnStateChanged += _ => StateChanged?.Invoke();
        }

        private readonly Ashfall.Core.Journal.JournalSystem? _journal;
        private readonly ILog _log;

        /// <summary>Bridge the authored knowledge_key (or the quest id) into the real journal.</summary>
        private void BridgeQuestKnowledge(DutyRosterQuestProgress p)
        {
            if (_journal == null || p == null) return;
            var def = Catalog.GetQuest(p.questId);
            if (def == null) return;
            // The authored knowledge key is the canonical journal key; quests without
            // one fall back to the quest id. Either way the briefing prose renders
            // in the journal and KnowledgeBase dedupes on reload.
            string key = string.IsNullOrEmpty(def.knowledge_key) ? p.questId : def.knowledge_key;
            string text = def.briefing ?? key;
            _journal.TryAddRawEntry(key, text, null, p.completedDay);
        }

        /// <summary>Raised when any roster/mark/encounter state changes (save dirty flag).</summary>
        public event Action StateChanged;

        public static DutyRosterHostSession Create(string dataDirectory, ILog? log = null,
            Ashfall.Core.Journal.JournalSystem journal = null)
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
            return new DutyRosterHostSession(roster, marks, encounters, catalog, clock,
                quests: null, journal: journal, log: log);
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
            DutyRosterSaveCodec.Capture(Roster, Marks, Encounters, Clock, Quests);

        public void RestoreSave(DutyRosterSave save) =>
            DutyRosterSaveCodec.Restore(save, Roster, Marks, Encounters, Clock, Quests);

        /// <summary>Persist through the Godot save store (host path).</summary>
        public bool SaveState()
        {
            return DutyRosterSaveStore.TrySave(CaptureSave());
        }

        // ── Quest runtime commands (thins; rules live in Core) ────────

        public string StartRosterQuest(string questId)
        {
            return Quests.StartQuest(questId, Clock.Day) ? "quest started: " + questId : "cannot start: " + questId;
        }

        public string AdvanceRosterQuest(string questId)
        {
            return Quests.AdvanceStage(questId, Clock.Day) ? "quest advanced: " + questId : "cannot advance: " + questId;
        }

        public string ResolveRosterChoice(string questId, string choiceId)
        {
            return Quests.ResolveChoiceWithEffects(questId, choiceId, Roster, Marks, Clock.Day)
                ? "choice resolved: " + choiceId
                : "cannot resolve choice: " + choiceId;
        }

        /// <summary>Current stage prose for an active quest (authored text rendered to the player).</summary>
        public string ActiveQuestProse(string questId)
        {
            var def = Catalog.GetQuest(questId);
            var p = Quests.GetProgress(questId);
            if (def == null || p == null || !p.started || p.completed || p.failed) return string.Empty;
            if (def.stages == null || p.currentStage < 0 || p.currentStage >= def.stages.Length)
                return string.Empty;
            return def.stages[p.currentStage].text ?? string.Empty;
        }

        public string QuestsLine()
        {
            return $"Quests: {Quests.StartedCount} started · {Quests.CompletedCount} complete · " +
                   $"{Quests.GetActiveQuests().Count} active · {Quests.GetAvailableQuests(Clock.Day).Count} available";
        }

        /// <summary>Morning tick: pencil fills rows from the demo occupants.</summary>
        public string TickDay()
        {
            return TickDay(DemoOccupants());
        }

        /// <summary>
        /// Morning tick with the host's real home-occupant snapshot. Kess fills
        /// pencil rows; ink never auto-fills; the chart is a document other
        /// systems read. Deterministic.
        /// </summary>
        public string TickDay(IReadOnlyList<DutyRosterOccupant> occupants)
        {
            LastEvent = string.Empty;
            int day = Clock.Day;
            Clock.AdvanceDays(1);
            Unlock(day);

            if (!Roster.State.wallInspected)
                Roster.NotifyWallInspected();

            Roster.TickMorning(Clock.Day, occupants);

            // Shelter encounters: one per night; morning bookkeeping resets the counter.
            Encounters.ResetNightCounter(Clock.Day);

            return string.IsNullOrEmpty(LastEvent) ? "morning row checked" : LastEvent;
        }

        /// <summary>
        /// Holdfast → Duty consequences (plan Appendix A.1). Deterministic;
        /// marks persist through the owning MoraleMarkSystem.
        /// </summary>
        public void SyncHoldfastToDuty(
            CensusClaimSystem census,
            IceRoadSystem iceRoad,
            WaystationSystem waystation,
            BrineWaterSystem brine,
            int day)
        {
            DutyRosterHoldfastBridge.SyncFromHoldfast(Roster, Marks, Encounters,
                census, iceRoad, waystation, brine, day);
        }

        /// <summary>Duty → Holdfast read model (plan Appendix A.2).</summary>
        public DutyRosterHoldfastSnapshot SnapshotForHoldfast()
        {
            return DutyRosterHoldfastBridge.SnapshotForHoldfast(Roster, Quests, Marks);
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
            // quest_roster_window opens the crisis window: more than one scene/night.
            bool crisis = Quests.IsCrisisQuestActive();
            bool ok = crisis
                ? Encounters.StartEncounterCrisis(id, kind, Clock.Day)
                : Encounters.StartEncounter(id, kind, Clock.Day);
            return ok ? "encounter started: " + kind : "no encounter tonight";
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

        // ── Overflow practice (bounded void, spec §2.4) ────────────────

        public string GrantOverflowAccess()
        {
            return Roster.GrantOverflowAccess() ? "overflow access granted" : "overflow already open";
        }

        public string RegisterOverflowVisit(string nodeId)
        {
            return Roster.RegisterOverflowVisit(nodeId)
                ? "overflow node visited: " + nodeId
                : "overflow visit rejected (closed or unknown node)";
        }

        // ── Hatch-return bridge (owned magnitudes stay in ExpeditionSystem) ──

        public string BridgeHatchReturn(string survivorId = null, bool crisis = false)
        {
            bool ok = Encounters.BridgeHatchReturn(Clock.Day, survivorId, null, crisis);
            return ok ? "hatch return staged" : "no hatch scene tonight (one per night unless crisis)";
        }

        public string GrantBlankRowsAccess()
        {
            return Roster.GrantBlankRowsAccess() ? "blank rows access restored" : "access already open";
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