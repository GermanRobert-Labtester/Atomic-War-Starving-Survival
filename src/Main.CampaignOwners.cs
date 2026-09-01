using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private void RegisterProductionCampaignOwners()
        {
            if (_campaignDay == null) return;

            // Phase 1: Environment, Weather & Base Core
            _campaignDay.Register("holdfast_core", new HoldfastCoreDayOwner(this), phase: 1);
            _campaignDay.Register("maritime_deep_coast", new DeepCoastMaritimeDayOwner(this), phase: 1);
            _campaignDay.Register("power_grid", new PowerGridDayOwner(this), phase: 1);
            _campaignDay.Register("weather_world", new WeatherWorldDayOwner(this), phase: 1);

            // Phase 2: Production, Infrastructure & Survival Basics
            _campaignDay.Register("crafting_production", new CraftingProductionDayOwner(this), phase: 2);
            _campaignDay.Register("economy_market", new EconomyMarketDayOwner(this), phase: 2);
            _campaignDay.Register("greenhouse_foundry", new GreenhouseFoundryDayOwner(this), phase: 2);
            _campaignDay.Register("shelter_facilities", new ShelterFacilitiesDayOwner(this), phase: 2);
            _campaignDay.Register("starting_level_rations", new StartingLevelRationsDayOwner(this), phase: 2);

            // Phase 3: Survivors, Medical, Disease & Social
            _campaignDay.Register("duty_roster", new DutyRosterDayOwner(this), phase: 3);
            _campaignDay.Register("medical_disease", new MedicalDiseaseDayOwner(this), phase: 3);
            _campaignDay.Register("phase0_psychology", new Phase0PsychologyDayOwner(this), phase: 3);
            _campaignDay.Register("survivor_social", new SurvivorSocialDayOwner(this), phase: 3);
            _campaignDay.Register("survivors_needs", new SurvivorsNeedsDayOwner(this), phase: 3);

            // Phase 4: Expeditions, World, Factions & Quests
            _campaignDay.Register("expeditions_caravans", new ExpeditionsCaravansDayOwner(this), phase: 4);
            _campaignDay.Register("narrative_quests_verdict", new NarrativeQuestsVerdictDayOwner(this), phase: 4);
            // Task 122: ticks after expeditions (ordinal 'w' > 'e') so it reads
            // fresh sortie results, and after narrative for fresh faction dominance.
            _campaignDay.Register("world_evolution", new EvolvingWorldDayOwner(this), phase: 4);
            // Plan IV: ledger debt ages with the campaign; forfeits dispatch
            // consequences into faction war / raids / inventory / labor.
            _campaignDay.Register("debt_ledger", new DebtLedgerDayOwner(this), phase: 4);

            // Phase 5: Events, Memorial & Final Evaluation
            _campaignDay.Register("host_events", new HostEventsDayOwner(this), phase: 5);
            _campaignDay.Register("memorial", new MemorialDayOwner(this), phase: 5);
            // Plan 29 29A: room-history day milestones. Reads only the identity
            // catalog and writes journal knowledge keys; no system ticks here.
            _campaignDay.Register("shelter_room_history", new ShelterRoomHistoryDayOwner(this), phase: 5);
        }

        // ── Phase 1 Owners ───────────────────────────────────────────────

        private sealed class WeatherWorldDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.World.WorldWeatherState? _snapshot;
            public WeatherWorldDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupWorld();
                _snapshot = _m._world.Weather.CaptureState();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._world.Weather.RestoreState(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupWorld();
                _m._world.TickHours(24f);
                _m._world.WeatherIntelligence.TickDay(day);
                events.Add(new DayStateChangeEvent("weather_ticked", "weather_world",
                    _m._world.Weather.Current.ToString(), null, _m._world.Weather.OutdoorRadModifier));
            }
        }

        private sealed class DeepCoastMaritimeDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public DeepCoastMaritimeDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMaritime();
                if (_m._maritime.Dive.IsActive)
                    _m._maritime.TickDive(60f);
                _m.SetupDeepCoast();
                _m._deepCoast.TickDaily(day, _m._core.Weather);
                _m._deepCoastPanel?.SetSimDay(day);
                events.Add(new DayStateChangeEvent("maritime_ticked", "maritime_deep_coast", null, null, day));
            }
        }

        private sealed class PowerGridDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public PowerGridDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickPowerGrid(day);
                events.Add(new DayStateChangeEvent("power_ticked", "power_grid", null, null, day));
            }
        }

        private sealed class HoldfastCoreDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private int _clockDaySnapshot;
            public HoldfastCoreDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                // The clock is the double-day hazard: if a later phase fails,
                // a retry must not tick the calendar twice.
                _clockDaySnapshot = _m._core.Clock.Day;
            }
            public void RestorePreDaySnapshot(int day)
            {
                _m._core.Clock.SetDay(_clockDaySnapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupIceRoad();
                string delta = _m._core.TickDay();
                if (_m._holdfastRuntime != null && !_m._holdfastRuntime.IsDead)
                {
                    _m._holdfastRuntime.Survivors = _m._survivors;
                    _m._holdfastRuntime.TickDay();
                }
                // Calendar-led authority: the clock is a projection and must
                // land exactly on the campaign day being committed, whatever
                // its internal tick state said.
                _m._core.Clock.SetDay(day);
                events.Add(new DayStateChangeEvent("holdfast_ticked", "holdfast_core", delta, null, _m._core.Clock.Day));
            }
        }

        // ── Phase 2 Owners ───────────────────────────────────────────────

        private sealed class StartingLevelRationsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public StartingLevelRationsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupStartingLevel();
                _m._startingLevel.TickDay();

                _m.SetupInventory();
                int foodToConsume = _m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3;
                int waterToConsume = _m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Irradiated ? 0 : (_m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3);
                _m._inventory.Remove("canned_food", foodToConsume);
                if (waterToConsume > 0)
                    _m._inventory.Remove("clean_water", waterToConsume);
                else
                    _m._inventory.Remove("irradiated_water", 2);

                events.Add(new DayStateChangeEvent("consumed_rations", "starting_level_rations", "canned_food", null, foodToConsume));
            }
        }

        private sealed class CraftingProductionDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public CraftingProductionDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupCrafting();
                _m._crafting.CompleteAll(24f);
                events.Add(new DayStateChangeEvent("crafting_completed", "crafting_production", null, null, 24f));
            }
        }

        private sealed class GreenhouseFoundryDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public GreenhouseFoundryDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupExpansions();
                if (_m._expansions.Greenhouse.PlotCount > 0)
                    _m._expansions.TickGreenhouse(day);

                _m.SetupGreenhouse();
                _m._greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);

                _m.SetupSilentFoundry();
                _m._silentFoundry.TickDaily(day);
                _m._silentFoundryPanel?.RefreshView();
                if (_m._foundryDirty) _m.SaveExpansionHub();

                events.Add(new DayStateChangeEvent("greenhouse_foundry_ticked", "greenhouse_foundry", null, null, day));
            }
        }

        private sealed class EconomyMarketDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Ashfall.Core.Economy.MarketState? _snapshot;
            public EconomyMarketDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupEconomy();
                _snapshot = _m._economy.CaptureSave();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._economy.RestoreSave(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupEconomy();
                _m._economy.TickDay(day, _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Economy, day, 0));
                events.Add(new DayStateChangeEvent("market_ticked", "economy_market", null, null, _m._economy.Market.Day));
            }
        }

        private sealed class ShelterFacilitiesDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public ShelterFacilitiesDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickAllExpandedShelterSystems(day);
                events.Add(new DayStateChangeEvent("shelter_facilities_ticked", "shelter_facilities", null, null, day));
            }
        }

        // ── Phase 3 Owners ───────────────────────────────────────────────

        private sealed class SurvivorsNeedsDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private SurvivorsSaveState? _snapshot;
            public SurvivorsNeedsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupSurvivors();
                _snapshot = _m._survivors.CaptureSave();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._survivors.RestoreSave(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupSurvivors();
                _m._survivors.TickHour(24f);
                _m.SetupShelterDecor();
                int decorRecipients = _m._shelterDecor?.ApplyDailyMorale(day) ?? 0;
                if (decorRecipients > 0)
                    events.Add(new DayStateChangeEvent("shelter_decor_morale", "shelter_decor", null, null, decorRecipients));
                // Drain any survivor_perished events from the death pipeline
                // into the briefing feed. Needs/radiation OnDied fires inside
                // TickHour — every death this day lands here exactly once.
                _m.SetupSurvivorFate();
                if (_m._survivorFate != null)
                    _m._survivorFate.DrainDayEvents(events);
                events.Add(new DayStateChangeEvent("survivors_ticked", "survivors_needs", null, null, _m._survivors.RosterState.Count));
            }
        }

        private sealed class MedicalDiseaseDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private static readonly Ashfall.Core.SystemTextJsonSerializer s_json = new Ashfall.Core.SystemTextJsonSerializer();
            private Ashfall.Core.Medical.ChemicalDependencyLedgerState? _medicalSnapshot;
            private string? _diseaseSnapshotJson;
            private Ashfall.Core.Medical.MedicalPipelineSaveState? _pipelineSnapshot;
            public MedicalDiseaseDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupMedical();
                _medicalSnapshot = _m._medical.CaptureSave();
                _m.EnsureMedicalPipeline();
                _pipelineSnapshot = _m._medical.CapturePipelineSave();

                _m.SetupDisease();
                // DiseaseSystem.CaptureState() returns its live state object by
                // reference (not a copy), so a plain assignment here would
                // alias the pre-day baseline to the same instance TickDay
                // mutates next. Round-trip through JSON — the system's own
                // save format — to take a true independent snapshot without
                // adding a new capture contract to Core.
                _diseaseSnapshotJson = s_json.Serialize(_m._disease.Engine.CaptureState());
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_medicalSnapshot != null)
                    _m._medical.RestoreSave(_medicalSnapshot);
                if (_pipelineSnapshot != null && _m._medical.Pipeline != null)
                    _m._medical.Pipeline.RestoreState(_pipelineSnapshot);
                if (_diseaseSnapshotJson != null && _m._disease != null)
                {
                    var restored = s_json.Deserialize<Ashfall.Core.Disease.DiseaseSystemState>(_diseaseSnapshotJson);
                    if (restored != null)
                        _m._disease.Engine.RestoreState(restored);
                }
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMedical();
                _m.EnsureMedicalPipeline();

                // Task #133 medical progression order (documented in the plan):
                // 1. scheduled procedures resolve (consume + apply at completion)
                // 2. chemical dependency progression (single tick owner)
                // 3. disease progression
                if (_m._medical.Pipeline != null)
                    _m._medical.Pipeline.AdvanceScheduled(24f, day);
                _m._medical.TickHours(24f);

                _m.SetupDisease();
                _m._disease.TickDaily(day);

                // Plan 60 / D5 + D7 — bridge illness into the shared sick-list band
                // ladder and keep the memorial grief sink bound. Runs after the
                // disease tick so it reads this day's stage, and is idempotent.
                _m.SyncDiseaseTriage(day, events);

                if (_m._expansionHubDirty) _m.SaveExpansionHub();

                events.Add(new DayStateChangeEvent("medical_disease_ticked", "medical_disease", null, null, day));
            }
        }

        private sealed class DutyRosterDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public DutyRosterDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupDutyRoster();
                _m._dutyRoster!.SyncDay(day);
                _m._dutyRoster!.TickDay(_m.BuildHomeOccupantSnapshot());
                _m.SetupIceRoad();
                _m._dutyRoster.SyncHoldfastToDuty(_m._core.Census, _m._core.IceRoad, _m._expansions.Waystation, _m._core.Brine, day);
                _m._dutyRosterPanel?.RefreshView();
                if (_m._dutyRosterDirty) _m.SaveDutyRoster();

                events.Add(new DayStateChangeEvent("duty_roster_ticked", "duty_roster", null, null, day));
            }
        }

        private sealed class SurvivorSocialDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public SurvivorSocialDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickSurvivorSocial(day);
                events.Add(new DayStateChangeEvent("survivor_social_ticked", "survivor_social", null, null, day));
            }
        }

        private sealed class Phase0PsychologyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private Phase0EffectsSaveState? _snapshot;
            public Phase0PsychologyDayOwner(Main m) => _m = m;
            // Phase0EffectsSaveState is built fresh by CaptureSave (deep-copied
            // sub-states), so holding it directly is a true independent snapshot.
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupPhase0();
                _snapshot = _m._phase0.CaptureSave();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot != null)
                    _m._phase0.RestoreSave(_snapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupPhase0();
                _m._phase0.CurrentDay = day;
                _m._phase0.IsInFalloutStorm = _m._world != null && _m._world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
                _m._phase0.IsNightTime = day % 2 == 0;
                _m._phase0.TickDay(day);

                events.Add(new DayStateChangeEvent("phase0_ticked", "phase0_psychology", null, null, day));
            }
        }

        // ── Phase 4 Owners ───────────────────────────────────────────────

        private sealed class ExpeditionsCaravansDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private ExpeditionAggregateState? _expeditionSnapshot;
            private TravelingCaravanState? _caravanSnapshot;
            public ExpeditionsCaravansDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupExpeditions();
                _expeditionSnapshot = _m._expeditions.CaptureSaveAggregate();
                _m.SetupCaravans();
                _caravanSnapshot = _m._caravans.CaptureSave();
            }
            public void RestorePreDaySnapshot(int day)
            {
                if (_expeditionSnapshot != null)
                    _m._expeditions.RestoreSaveAggregate(_expeditionSnapshot);
                if (_caravanSnapshot != null)
                    _m._caravans.RestoreSave(_caravanSnapshot);
            }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupExpeditions();
                _m._expeditions.TickHours(24f);

                _m.SetupDutyRoster();
                var expeditions = _m._expeditions.Engine.CaptureState();
                if (expeditions != null && _m._dutyRoster != null)
                {
                    for (int i = 0; i < expeditions.Count; i++)
                    {
                        var ex = expeditions[i];
                        if (ex == null) continue;
                        if (ex.phase == (int)ExpeditionPhase.Completed && !string.IsNullOrEmpty(ex.survivorId))
                        {
                            bool crisis = _m._dutyRoster.Quests.IsCrisisQuestActive();
                            _m._dutyRoster.BridgeHatchReturn(ex.survivorId, crisis: crisis);
                            break;
                        }
                    }
                }

                _m.SetupCaravans();
                _m._caravans.TickRoute();

                events.Add(new DayStateChangeEvent("expeditions_caravans_ticked", "expeditions_caravans", null, null, day));
            }
        }

        /// <summary>
        /// Task 122 — the world changes because of time and player action:
        /// feeds live weather into landmark decay and location contamination,
        /// runs seeded wildlife migration, records expedition consequences on
        /// locations, bridges faction dominance into ownership, shifts market
        /// scarcity with wildlife pressure, and surfaces every major change
        /// through briefing events, journal lines, and radio intercepts.
        /// </summary>
        private sealed class EvolvingWorldDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Main _m;
            private LocationEvolutionSaveState? _locSnapshot;
            private WildlifeSaveState? _wildSnapshot;
            private LandmarkSaveState? _landSnapshot;
            private readonly HashSet<string> _processedExpeditions = new HashSet<string>();
            private string _lastDominantFaction = string.Empty;

            public EvolvingWorldDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day)
            {
                _m.SetupWorld();
                _locSnapshot = _m._world.LocationEvolution?.CaptureState();
                _wildSnapshot = _m._world.Wildlife?.CaptureState();
                _landSnapshot = _m._world.Landmarks?.CaptureState();
            }

            public void RestorePreDaySnapshot(int day)
            {
                if (_locSnapshot != null) _m._world.LocationEvolution?.RestoreState(_locSnapshot);
                if (_wildSnapshot != null) _m._world.Wildlife?.RestoreState(_wildSnapshot);
                if (_landSnapshot != null) _m._world.Landmarks?.RestoreState(_landSnapshot);
                _processedExpeditions.Clear();
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupWorld();
                var world = _m._world;

                var kind = world.Weather.Current;
                bool hazard = kind == WeatherKind.FalloutStorm || kind == WeatherKind.BlackRain;
                float ashfallMm = Main.AshfallMmFor(kind);

                // Pre-tick deltas we report on.
                var collapsedBefore = CollapsedSet(world);
                var sectorsBefore = SectorMap(world);
                var ownersBefore = OwnerMap(world);

                // ── The world moves ──
                world.Landmarks?.TickDay(day, ashfallMm);
                world.LocationEvolution?.TickDay(day,
                    new LocationEvolutionInputs(world.Weather.OutdoorRadModifier, hazard),
                    _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.WorldEvolution, day, 0));
                world.Wildlife?.TickDay(day,
                    _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.WorldEvolution, day, 1));

                // ── Landmark collapses → warning, journal ──
                if (world.Landmarks != null)
                {
                    foreach (var lm in world.Landmarks.State.landmarks)
                    {
                        if (lm == null || !lm.isCollapsed || lm.collapseDay != day) continue;
                        if (collapsedBefore.Contains(lm.landmarkId)) continue;
                        events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                            $"Landmark collapsed: {lm.landmarkId}", $"at {lm.locationId} (day {day})", lm.structuralIntegrity));
                        _m.SetupJournal();
                        _m._journal.TryAddRawEntry($"world_{lm.landmarkId}_collapse",
                            $"🔺 {lm.landmarkId} came down at {lm.locationId}. The skyline is poorer by one shape.",
                            null!, day);
                    }
                }

                // ── Pack migrations → radio intercepts ──
                if (world.Wildlife != null)
                {
                    int reported = 0;
                    var after = SectorMap(world);
                    foreach (var pack in world.Wildlife.State.packs)
                    {
                        if (pack == null || reported >= 3) continue;
                        if (sectorsBefore.TryGetValue(pack.packId, out var before)
                            && before != pack.currentSectorId)
                        {
                            // Plan 28: archetype-flavored coarse sighting; the
                            // generic move line stays for unremarkable species.
                            string notice = WildlifeSeasonalCalendar.MigrationNotice(
                                WildlifeSeasonalCalendar.ArchetypeOf(pack.speciesId),
                                pack.speciesId, before, pack.currentSectorId, day);
                            events.Add(new DayStateChangeEvent("radio_intercept", "world_evolution",
                                "wildlife net",
                                notice ?? $"{pack.packId} sighted moving {before} into {pack.currentSectorId}",
                                pack.population));
                            reported++;
                            // Plan 28 Phase 5: observation drives knowledge — a
                            // sighted species unlocks its field-guide teach
                            // entry (session knowledge; persistence = Plan 20A).
                            var teach = WildlifeSeasonalCalendar.FieldGuideEntryFor(pack.speciesId);
                            if (teach != null) _m.UnlockFieldGuideObservation(teach);
                        }
                        if (pack.isRabid && pack.lastThreatFiredDay == day)
                        {
                            events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                                $"Rabid {pack.speciesId}", $"{pack.packId} turned in {pack.currentSectorId}", pack.aggressionScore));
                        }
                    }
                }

                // ── Expedition consequences on locations ──
                _m.SetupExpeditions();
                var expeditions = _m._expeditions.Engine.CaptureState();
                if (expeditions != null)
                {
                    foreach (var ex in expeditions)
                    {
                        if (ex == null || string.IsNullOrEmpty(ex.expeditionId)) continue;
                        if (ex.phase != (int)ExpeditionPhase.Completed && ex.phase != (int)ExpeditionPhase.Failed) continue;
                        if (!_processedExpeditions.Add(ex.expeditionId)) continue;
                        if (string.IsNullOrEmpty(ex.locationId) || world.LocationEvolution == null) continue;

                        if (ex.phase == (int)ExpeditionPhase.Completed)
                        {
                            world.LocationEvolution.MarkCleared(ex.locationId, day);
                            events.Add(new DayStateChangeEvent("expedition_milestone", "world_evolution",
                                ex.locationId, "swept clean — salvage thins here for a while", 1));
                        }
                        else
                        {
                            world.LocationEvolution.MarkVisited(ex.locationId, day);
                            world.LocationEvolution.AddThreat(ex.locationId, LocationEvolutionSystem.ThreatSquatters);
                            events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                                ex.locationId, "sortie lost — stragglers now haunt the ground", 1));
                        }
                    }
                }

                // ── Faction dominance → location ownership ──
                _m.SetupYearOfAsh();
                string dominant = _m._yearOfAsh?.FactionWar?.DominantFactionId ?? string.Empty;
                if (!string.IsNullOrEmpty(dominant) && dominant != _lastDominantFaction)
                {
                    bool firstObservation = _lastDominantFaction.Length == 0;
                    _lastDominantFaction = dominant;
                    if (!firstObservation && world.Seeds?.location_seeds != null)
                    {
                        foreach (var seed in world.Seeds.location_seeds)
                        {
                            if (seed == null || seed.owner != dominant) continue;
                            var before = ownersBefore.TryGetValue(seed.location_id, out var o) ? o : null;
                            if (before == dominant) continue;
                            world.LocationEvolution?.SetLocationOwner(seed.location_id, dominant);
                            events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                                seed.location_id, $"control passes to {dominant}", 1));
                            _m.SetupJournal();
                            _m._journal.TryAddRawEntry($"world_{seed.location_id}_owner_{dominant}",
                                $"🔻 {seed.location_id} answer to {dominant} now. Flags change; the ground stays.",
                                null!, day);
                        }
                    }
                }

                // ── Wildlife pressure → market scarcity & trapping density ──
                _m.SetupEvolvingWorldInfluence();
                float ratio = world.Wildlife?.GetGlobalPopulationRatio() ?? 1f;
                var goods = EvolvingWorldSeeder.ScarcityGoods(world.Seeds);
                if (goods.Count > 0)
                {
                    float delta = ratio < 0.6f ? 0.02f : ratio < 0.85f ? 0.005f : ratio > 1.2f ? -0.005f : 0f;
                    if (Math.Abs(delta) > 0f)
                    {
                        _m.SetupEconomy();
                        foreach (var g in goods)
                            _m._economy.Market.AdjustDemand(g, delta);
                    }
                }

                // ── Plan 28 Phase 3: war-blocked corridors & collapse notice ──
                // Faction dominance projects onto the sector graph: sectors
                // holding dominant-faction ground close to wildlife movement
                // (stateless projection — the migration runtime never
                // persists blockage). Binding: seeds' location records carry
                // an optional sector_id.
                {
                    world.Wildlife?.ClearSectorBlockages();
                    if (!string.IsNullOrEmpty(dominant) && world.Seeds?.location_seeds != null)
                    {
                        foreach (var seed in world.Seeds.location_seeds)
                        {
                            if (seed == null || seed.owner != dominant) continue;
                            var sector = SectorOfLocation(world, seed.location_id);
                            if (!string.IsNullOrEmpty(sector)) world.Wildlife?.SetSectorBlocked(sector, true);
                        }
                    }
                }

                // ── Plan 28 Phase 4: ecological infestations ──
                _m.TickEcologicalInfestations(day, events);

                // ── Plans 46-49: Workshop, Radio, Social, Subterranean Hazards ──
                _m.TickPlans46_49(day, events);

                // ── Plan 28 Phase 3: collapse/scarcity notice (bounded) ──

                // ── Plan 28 Phase 3: collapse/scarcity notice (bounded) ──
                if (world.Wildlife != null && ratio <= 0.45f
                    && day - _lastCollapseNoticeDay >= CollapseNoticeCooldownDays)
                {
                    _lastCollapseNoticeDay = day;
                    events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
                        "wildlife collapse",
                        "the land has gone quiet — snare lines and larders both", ratio));
                    _m.SetupJournal();
                    _m._journal.TryAddRawEntry($"world_wildlife_collapse_{day}",
                        "Something changed in the counts. The dogs range wider; the snares come back empty.",
                        null!, day);
                }

                events.Add(new DayStateChangeEvent("world_evolution_ticked", "world_evolution", null, null, day));
            }

            /// <summary>Collapse notices re-arm after this many days (anti-spam).</summary>
            private const int CollapseNoticeCooldownDays = 12;
            private int _lastCollapseNoticeDay = -30;

            /// <summary>
            /// Plan 28 Phase 3 — sector binding for war-blocked corridors: the
            /// seeds' location records carry an optional sector binding so the
            /// dominant faction's ground closes its representative sector to
            /// wildlife movement (stateless projection, never persisted).
            /// </summary>
            private static string? SectorOfLocation(WorldHostSession world, string locationId)
            {
                if (world.Seeds?.location_seeds == null) return null;
                foreach (var seed in world.Seeds.location_seeds)
                    if (seed != null && string.Equals(seed.location_id, locationId, StringComparison.Ordinal))
                        return string.IsNullOrEmpty(seed.sector_id) ? null : seed.sector_id;
                return null;
            }

            private static HashSet<string> CollapsedSet(WorldHostSession world)
            {
                var set = new HashSet<string>();
                foreach (var lm in world.Landmarks?.State.landmarks ?? new List<LandmarkStatusRecord>())
                    if (lm != null && lm.isCollapsed) set.Add(lm.landmarkId);
                return set;
            }

            private static Dictionary<string, string> SectorMap(WorldHostSession world)
            {
                var map = new Dictionary<string, string>();
                foreach (var p in world.Wildlife?.State.packs ?? new List<WildlifePackRecord>())
                    if (p != null) map[p.packId] = p.currentSectorId;
                return map;
            }

            private static Dictionary<string, string> OwnerMap(WorldHostSession world)
            {
                var map = new Dictionary<string, string>();
                foreach (var m in world.LocationEvolution?.State.mutations ?? new List<LocationMutationRecord>())
                    if (m != null) map[m.locationId] = m.currentOwner;
                return map;
            }
        }

        private sealed class NarrativeQuestsVerdictDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public NarrativeQuestsVerdictDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMoralChoice();
                _m._moralChoice.Reconcile(day);

                _m.TickVerdict(day, _m.LivingDwellerCountEstimate());

                if (day >= 180 && day <= 360)
                {
                    _m.SetupYearOfAsh();
                    _m._yearOfAsh.TickDay(day);
                }

                if (day >= 260)
                {
                    _m.SetupMuster();
                    _m._muster.Escalate(day);
                }

                _m.SetupExpansions();
                _m._expansions.Ledger.TickDaily(day);
                _m._expansions.TickCrossingQuests(day);

                _m.SetupExpansionQuests();
                _m._expansionQuests.TickDay(day);

                events.Add(new DayStateChangeEvent("narrative_ticked", "narrative_quests_verdict", null, null, day));
            }
        }

        // ── Phase 5 Owners ───────────────────────────────────────────────

        private sealed class HostEventsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public HostEventsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupEventAdapter();
                bool hydroAudit = _m._muster?.HydroBarons?.AdminReform ?? false;
                bool hydroSeized = _m._muster?.HydroBarons?.PlantSeized ?? false;
                bool osteophageInquiry = (_m._yearOfAsh != null && _m._yearOfAsh.Timeline.CurrentDay >= 205) || day >= 205;
                bool coldCountBroadcast = _m._muster?.ColdCount?.BroadcastSent ?? false;
                _m._hostEventAdapter?.EvaluateTriggers(day, hydroAudit, hydroSeized, osteophageInquiry, coldCountBroadcast);

                events.Add(new DayStateChangeEvent("events_evaluated", "host_events", null, null, day));
            }
        }

        private sealed class MemorialDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public MemorialDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMemorial();
                events.Add(new DayStateChangeEvent("memorial_checked", "memorial", null, null, day));
            }
        }

        /// <summary>Plan 29 Task 29A: once-daily room-history milestone pass.</summary>
        private sealed class ShelterRoomHistoryDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public ShelterRoomHistoryDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickShelterRoomHistoryMilestones(day);
            }
        }
    }
}
