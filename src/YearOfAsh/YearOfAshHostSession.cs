using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Warlords;
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
        private WarlordDoctrineSystem _warlord;
        private FactionWarChainRunner _warRunner;
        private readonly List<SurvivorOccupantSnapshot> _demoRoster;
        private readonly SeededRng _warlordRng;

        public YearOfAshTimelineSystem Timeline => _timeline;
        public DoorEncounterSystem Encounters => _encounters;
        public FactionWarSystem FactionWar => _factionWar;
        public QuestlineSystem Quests => _quests;
        public YearOfAshDeepFreezeSystem DeepFreeze => _deepFreeze;
        public YearOfAshRadonSystem Radon => _radon;
        public WarlordDoctrineSystem Warlord => _warlord;
        public FactionWarChainRunner WarRunner => _warRunner;
        public IReadOnlyList<SurvivorOccupantSnapshot> DemoRoster => _demoRoster;

        public YearOfAshHostSession(
            YearOfAshTimelineSystem timeline = null!,
            DoorEncounterSystem encounters = null!,
            FactionWarSystem factionWar = null!,
            QuestlineSystem quests = null!,
            YearOfAshDeepFreezeSystem deepFreeze = null!,
            YearOfAshRadonSystem radon = null!,
            WarlordDoctrineSystem warlord = null!,
            FactionWarChainRunner warRunner = null!)
        {
            _timeline = timeline ?? new YearOfAshTimelineSystem();
            _encounters = encounters ?? new DoorEncounterSystem();
            _factionWar = factionWar ?? new FactionWarSystem();
            _quests = quests ?? new QuestlineSystem();
            _deepFreeze = deepFreeze ?? new YearOfAshDeepFreezeSystem();
            _radon = radon ?? new YearOfAshRadonSystem();
            _warlord = warlord ?? new WarlordDoctrineSystem();
            _warRunner = warRunner ?? new FactionWarChainRunner(new FactionWarContentCatalog());
            _warlordRng = new SeededRng(2026);
            _demoRoster = CreateDefaultDemoRoster();
            WireWarlordConsequences();
        }

        /// <summary>
        /// Swaps in the catalog-bound warlord (loaded after construction) and
        /// re-wires the consequence subscriptions onto it.
        /// </summary>
        public void BindWarlord(WarlordDoctrineSystem warlord)
        {
            _warlord = warlord ?? _warlord;
            WireWarlordConsequences();
        }

        /// <summary>
        /// Thin consequence wiring (host-owned, no rules): hostile warlord
        /// actions and tribute short-payments move the canonical
        /// FactionWarSystem standing for warlords_sector_4, which persists with
        /// the factionWar envelope section.
        /// </summary>
        private void WireWarlordConsequences()
        {
            _warlord.OnActionExecuted += result =>
            {
                if (result == null) return;
                bool hostile = result.Action == WarlordStrategicAction.Raid
                    || result.Action == WarlordStrategicAction.Annex
                    || result.Action == WarlordStrategicAction.Contest;
                if (!hostile) return;
                _factionWar.ModifyStanding(WarlordDoctrineSystem.CanonicalFactionId,
                    result.Success ? 3 : -2);
            };
            _warlord.OnTributeSettled += (paidFull, _) =>
            {
                if (!paidFull)
                    _factionWar.ModifyStanding(WarlordDoctrineSystem.CanonicalFactionId, -2);
            };
        }

        public static YearOfAshHostSession Create(string dataDir = "", bool loadExistingSave = true)
        {
            var session = new YearOfAshHostSession();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = AtomicWar.GodotApp.CatalogPath.CreateFileIOForDataDir(dataDir);
                var serializer = new SystemTextJsonSerializer();
                DoorEncounterCatalogLoader.LoadAndRegister(session.Encounters, dataDir, fileIO, serializer);
                YearOfAshCatalogLoader.LoadAndRegisterQuests(session.Quests, dataDir, fileIO, serializer);
                // ASHFALL: THE VERDICT (Expansion 08) questlines are NOT registered
                // here — Verdict is the sole owner of its quest progress, registered
                // and persisted via VerdictHostSession / VerdictSave (v3+). Older
                // Year-of-Ash-carried quest_verdict_* records are adopted into the
                // Verdict envelope on load (VerdictQuestMigration).
                // ASHFALL: THE DOSE (Expansion 07) questlines are NOT registered
                // here either — Dose owns its quest progress via
                // DoseLedgerHostSession / DoseLedgerSave (v2+). Older
                // Year-of-Ash-carried Dose quest records are adopted into the Dose
                // envelope on load (DoseQuestMigration).
                // Adaptive Warlord AI (proposed model): load + validate the doctrine
                // catalog, then bind the warlord to the warlords_sector_4 identity.
                var warlordCatalog = WarlordDoctrineCatalogLoader.Load(dataDir, fileIO, serializer);
                var validation = WarlordCatalogValidator.Validate(warlordCatalog, dataDir, fileIO);
                if (!validation.Clean)
                {
                    var sb = new System.Text.StringBuilder("WarlordDoctrineCatalog validation failed:");
                    for (int i = 0; i < validation.Errors.Count; i++)
                        sb.Append("\n  ").Append(validation.Errors[i]);
                    throw new InvalidOperationException(sb.ToString());
                }
                session._warlord = new WarlordDoctrineSystem(warlordCatalog, seedSalt: 2026);
                for (int i = 0; i < validation.AliasWarnings.Count; i++)
                    GD.Print("[warlord] " + validation.AliasWarnings[i]);
                session.BindWarlord(session._warlord);

                // Load Faction War content catalog and bind runner
                var warCatalogLoader = new FactionWarContentCatalogLoader(fileIO, serializer, new GodotLog());
                var warCatalog = warCatalogLoader.Load(dataDir);
                session._warRunner = new FactionWarChainRunner(warCatalog);
            }

            var existingSave = loadExistingSave ? YearOfAshSaveStore.TryLoad() : null;
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
            _warRunner.TickDay(day);
            _deepFreeze.TickDailyThermal(day, _timeline.AmbientTemperatureCelsius);
            _radon.TickDailyRadon(day, _timeline.AmbientTemperatureCelsius);
            TickWarlord(day);
        }

        /// <summary>
        /// One warlord operation tick per day (idempotent in Core). The world
        /// view the warlord acts on is explicit and non-omniscient: the host
        /// reports only what the warlord could plausibly learn (scouts on
        /// adjacent chokepoints), plus environment/rival/player context.
        /// </summary>
        private void TickWarlord(int day)
        {
            // Scouts report the chokepoints within reach of warlord ground.
            var catalog = _warlord.Catalog;
            for (int i = 0; i < catalog.Territory.Count; i++)
            {
                var node = catalog.Territory[i];
                if (node == null) continue;
                // Only nodes adjacent to warlord ground get observed — the
                // warlord is not omniscient.
                if (!IsAdjacentToWarlordGround(node.location_id) && !node.home) continue;
                _warlord.Observe(node.location_id, _warlord.TerritoryState(node.location_id), day, confidence: 1f);
            }

            float environmentHazard = Math.Clamp(
                (Math.Abs(_timeline.AmbientTemperatureCelsius) + _deepFreeze.State.intakeIceThicknessMm) / 60f, 0f, 1f);
            float rivalPressure = Math.Clamp(_factionWar.WarTension / 100f, 0f, 1f);
            int playerStanding = _factionWar.GetStanding(WarlordDoctrineSystem.CanonicalFactionId);
            var context = new WarlordContext
            {
                EnvironmentHazard = environmentHazard,
                RivalPressure = rivalPressure,
                PlayerStanding = playerStanding
            };
            _warlord.TickDaily(day, _warlordRng, context);
        }

        private bool IsAdjacentToWarlordGround(string locationId)
        {
            var neighbors = _warlord.Catalog.Neighbors(locationId);
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (_warlord.TerritoryState(neighbors[i]) == WarlordTerritoryState.Controlled)
                    return true;
            }
            return false;
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

        /// <summary>The current collector's ask (base × escalation multiplier).</summary>
        public int CurrentTributeAsk =>
            Math.Max(1, (int)(_warlord.Catalog.Warlord.tribute_base_amount * _warlord.TributeMultiplier));

        /// <summary>
        /// Player settles the current tribute ask through Core (no rules here).
        /// Returns false when the payment is refused/zeroed; nextAsk is always
        /// the resulting ask for display.
        /// </summary>
        public bool SettleWarlordTribute(int amountPaid, int day, out int nextAsk)
        {
            if (amountPaid <= 0)
            {
                _warlord.SettleTribute(0, day, out nextAsk);
                return false;
            }
            return _warlord.SettleTribute(amountPaid, day, out nextAsk);
        }

        /// <summary>Authored collector prose for the given outcome state (demand/paid/short/refused).</summary>
        public string CollectorLine(string state, int day) => _warlord.Catalog.CollectorLine(state, day);

        /// <summary>Player-facing warlord readout (doctrine, territory, tribute, ledger).</summary>
        public string WarlordLine()
        {
            var w = _warlord;
            var sb = new System.Text.StringBuilder();
            sb.Append("Warlord: ").Append(w.DoctrineId).Append(" · supply ").Append(w.Supply)
                .Append('/').Append(w.SupplyNeed)
                .Append(" · ops ").Append(w.TotalOperations)
                .Append(" · tribute ×").Append(w.TributeMultiplier.ToString("0.##"));
            var territory = w.State.territory;
            if (territory != null)
            {
                for (int i = 0; i < territory.Count; i++)
                {
                    var rec = territory[i];
                    if (rec == null) continue;
                    sb.Append("\n  ").Append(rec.locationId).Append(": ")
                        .Append(((WarlordTerritoryState)rec.state).ToString());
                }
            }
            return sb.ToString();
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
                null!,
                _deepFreeze,
                _radon,
                _quests,
                _warlord,
                _warRunner);
        }

        public void RestoreSave(YearOfAshSave save)
        {
            if (save == null) return;
            YearOfAshSaveCodec.Restore(
                save, _timeline, _encounters, _factionWar,
                _deepFreeze, _radon, _quests, _warlord,
                _warRunner);
            // Verdict quest progress is owned by the Verdict envelope (v3+) and
            // Dose quest progress by the Dose envelope (v2+). After the one-time
            // adoption in their host sessions, strip any quest_verdict_* /
            // Dose quest records a legacy save still carries so this envelope
            // stops re-serializing them (one persisted owner per expansion).
            Ashfall.Core.Verdict.VerdictQuestMigration.StripFromYearOfAsh(_quests.State);
            Ashfall.Core.DoseQuestMigration.StripFromYearOfAsh(_quests.State);
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
