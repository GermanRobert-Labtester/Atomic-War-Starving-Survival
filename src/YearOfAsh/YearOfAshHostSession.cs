using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp.YearOfAsh
{
    /// <summary>
    /// Godot host coordinator for Expansion 05: The Year of Ash (Days 180 to 360).
    /// Manages the timeline system, door encounter evaluations, faction war state, branching questlines,
    /// deep freeze thermodynamics, and thaw radon ventilation.
    /// </summary>
    public class YearOfAshHostSession
    {
        private readonly YearOfAshTimelineSystem _timeline;
        private readonly DoorEncounterSystem _encounters;
        private readonly FactionWarSystem _factionWar;
        private readonly QuestlineSystem _quests;
        private readonly YearOfAshDeepFreezeSystem _deepFreeze;
        private readonly YearOfAshRadonSystem _radon;
        private readonly List<SurvivorOccupantSnapshot> _demoRoster;

        public YearOfAshTimelineSystem Timeline => _timeline;
        public DoorEncounterSystem Encounters => _encounters;
        public FactionWarSystem FactionWar => _factionWar;
        public QuestlineSystem Quests => _quests;
        public YearOfAshDeepFreezeSystem DeepFreeze => _deepFreeze;
        public YearOfAshRadonSystem Radon => _radon;
        public IReadOnlyList<SurvivorOccupantSnapshot> DemoRoster => _demoRoster;

        public YearOfAshHostSession(
            YearOfAshTimelineSystem timeline = null,
            DoorEncounterSystem encounters = null,
            FactionWarSystem factionWar = null,
            QuestlineSystem quests = null,
            YearOfAshDeepFreezeSystem deepFreeze = null,
            YearOfAshRadonSystem radon = null)
        {
            _timeline = timeline ?? new YearOfAshTimelineSystem();
            _encounters = encounters ?? new DoorEncounterSystem();
            _factionWar = factionWar ?? new FactionWarSystem();
            _quests = quests ?? new QuestlineSystem();
            _deepFreeze = deepFreeze ?? new YearOfAshDeepFreezeSystem();
            _radon = radon ?? new YearOfAshRadonSystem();
            _demoRoster = CreateDefaultDemoRoster();
        }

        public static YearOfAshHostSession Create(string dataDir = "")
        {
            var session = new YearOfAshHostSession();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                DoorEncounterCatalogLoader.LoadAndRegister(session.Encounters, dataDir, fileIO, serializer);
                YearOfAshCatalogLoader.LoadAndRegisterQuests(session.Quests, dataDir, fileIO, serializer);
                // ASHFALL: THE VERDICT (Expansion 08) — register its runtime questlines
                // into the same QuestlineSystem so they surface via GetPlayableQuestlines
                // and persist through the existing YearOfAshSave envelope.
                Ashfall.Core.Verdict.VerdictQuestCatalogLoader.LoadAndRegister(session.Quests, dataDir, fileIO, serializer);
                // ASHFALL: THE DOSE (Expansion 07) — register the four register quest lines
                // (first reading / sick room / child's number / signed hour) into the same
                // QuestlineSystem so they surface via GetPlayableQuestlines with the other
                // expansion quests and persist through YearOfAshSave.
                var doseContent = Ashfall.Core.DoseContentCatalogLoader.Load(dataDir, fileIO, serializer);
                foreach (var dq in doseContent.quests)
                {
                    if (dq == null || string.IsNullOrEmpty(dq.questlineId)) continue;
                    session.Quests.RegisterQuestline(dq);
                }
            }

            var existingSave = YearOfAshSaveStore.TryLoad();
            if (existingSave != null)
            {
                session.RestoreSave(existingSave);
            }
            return session;
        }

        public void TickDay(int day)
        {
            _timeline.AdvanceDay(day);
            _factionWar.SimulateDailyFriction(day);
            _deepFreeze.TickDailyThermal(day, _timeline.AmbientTemperatureCelsius);
            _radon.TickDailyRadon(day, _timeline.AmbientTemperatureCelsius);
        }

        public string GetStatusSummary()
        {
            return $"[Year of Ash] Day {_timeline.CurrentDay} ({_timeline.CurrentPhase}) | " +
                   $"Surface: {_timeline.AmbientTemperatureCelsius:F1}°C | " +
                   $"Bunker: {_deepFreeze.IndoorTempCelsius:F1}°C | " +
                   $"Radon: {_radon.IndoorRadonBqm3:F0} Bq/m³ | " +
                   $"War Tension: {_factionWar.WarTension}/100 | " +
                   $"Active Quests: {_quests.State.active.Count}";
        }

        /// <summary>
        /// Snapshots every system the session ticks. Deep-freeze, radon and questline
        /// were ticked daily but left out of the envelope, so a reload handed the player
        /// a fresh scrubber, a clear intake and no quest history at whatever day the
        /// timeline restored to.
        /// </summary>
        public YearOfAshSave CaptureSave()
        {
            return YearOfAshSaveCodec.Capture(
                _timeline,
                _encounters,
                _factionWar,
                null,
                _deepFreeze,
                _radon,
                _quests);
        }

        public void RestoreSave(YearOfAshSave save)
        {
            if (save == null) return;
            YearOfAshSaveCodec.Restore(
                save, _timeline, _encounters, _factionWar,
                _deepFreeze, _radon, _quests);
        }

        private List<SurvivorOccupantSnapshot> CreateDefaultDemoRoster()
        {
            return new List<SurvivorOccupantSnapshot>
            {
                // Ids come from the year_of_ash_survivors.json master list — never
                // invented locally (AGENTS.md id rule).
                new SurvivorOccupantSnapshot
                {
                    survivorId = "survivor_dr_sarah_chen",
                    name = "Dr. Sarah Chen (Trauma Surgeon)",
                    moralBranch = "humanist",
                    guiltLevel = 25,
                    traits = new List<string> { "trait_medic", "trait_altruistic" },
                    hasRespiratoryDegeneration = true,
                    hasTraumaBondWithLeader = true
                },
                new SurvivorOccupantSnapshot
                {
                    survivorId = "survivor_gunner_mikhail",
                    name = "Gunner Mikhail (Heavy Artillery Loader)",
                    moralBranch = "ruthless",
                    guiltLevel = 10,
                    traits = new List<string> { "trait_veteran", "trait_pragmatist" },
                    hasChemicalDependency = false,
                    hasTraumaBondWithLeader = false
                }
            };
        }
    }
}
