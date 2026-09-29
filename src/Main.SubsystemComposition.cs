// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Audio;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Disease;
using Ashfall.Core.Economy;
using Ashfall.Core.Excavation;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Farming;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Ashfall.Core.Needs;
using Ashfall.Core.Quests;
using Ashfall.Core.Radio;
using Ashfall.Core.Random;
using Ashfall.Core.Recreation;
using Ashfall.Core.Research;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;
using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

        private void SetupPlans110To113()
        {
            SetupChlorAlkali();
            SetupSolarConcentrator();
            SetupPrecisionOptics();
            SetupBallisticShield();
        }

        // Composite orchestration only; the child SaveXxx methods are the
        // registered campaign sections.
        private void PersistPlans110To113()
        {
            SaveChlorAlkali();
            SaveSolarConcentrator();
            SavePrecisionOptics();
            SaveBallisticShield();
        }

        // ─── Tick (cadence) ───

        private void TickPlans110To113(int day)
        {
            _chlorAlkali?.System.TickDay(day);
            _solarConcentrator?.System.TickDay(day);
            // B5–B8 Phase 2: the tick raises OnSolarOutputChanged, but republish
            // explicitly so a restored session (no event yet) still feeds the
            // grid before the next day's power tick.
            PublishSolarConcentratorGeneration();
        }

        // ── Panels (Plan 122-125 Phase 9): created hidden; opened via the
        //    expanded-panel route. Presentation only.
        private void EnsurePlans122to125Panels()
        {
            if (_sofcPower != null && _sofcPanel == null)
            {
                _sofcPanel = new SolidOxideFuelCellPanel();
                _sofcPanel.Bind(_sofcPower);
                _sofcPanel.Visible = false;
                AddChild(_sofcPanel);
            }
            if (_cvdDiamond != null && _cvdPanel == null)
            {
                _cvdPanel = new CvdDiamondPanel();
                _cvdPanel.Bind(_cvdDiamond);
                _cvdPanel.Visible = false;
                AddChild(_cvdPanel);
            }
            if (_soundRanging != null && _soundRangingPanel == null)
            {
                _soundRangingPanel = new SoundRangingPanel();
                _soundRangingPanel.Bind(_soundRanging);
                _soundRangingPanel.Visible = false;
                AddChild(_soundRangingPanel);
            }
            if (_amphibiousDraisine != null && _amphibiousPanel == null)
            {
                _amphibiousPanel = new AmphibiousDraisinePanel();
                _amphibiousPanel.Bind(_amphibiousDraisine);
                _amphibiousPanel.Visible = false;
                AddChild(_amphibiousPanel);
            }
        }

        /// <summary>Daily tick for the flagship tranche (Plan 126 active this wave).</summary>
        private void TickPlans126_129(int currentDay)
        {
            _bioFermentation?.TickDay(currentDay);
        }
        private Plans130To133Panel? _plans130To133Panel;

        private void SetupPlans130To133()
        {
            SetupPowderMetallurgy();
            SetupNvisCommunications();
            SetupLyophilization();
            SetupDraisineRerailing();
            SetupPlans130To133Panel();
        }

        private void SetupPlans130To133Panel()
        {
            if (_plans130To133Panel == null)
            {
                _plans130To133Panel = new Plans130To133Panel();
                AddChild(_plans130To133Panel);
                _plans130To133Panel.OnClose += () => _plans130To133Panel.Visible = false;
            }

            _plans130To133Panel.Bind(
                _powderMetallurgy!,
                _nvisCommunications!,
                _lyophilization!,
                _draisineRerailing!,
                EnsureRailway(),
                _expeditions,
                () => _simDay,
                AcknowledgeNvisRecall);
        }

        // Composite orchestration only; the child SaveXxx methods are the
        // registered campaign sections.
        private void PersistPlans130To133()
        {
            SavePowderMetallurgy();
            SaveNvisCommunications();
            SaveLyophilization();
            SaveDraisineRerailing();
        }

        private void CaptureIfPresent<T>(
            string section,
            T? state,
            Func<T, string> capture)
            where T : class
        {
            if (state != null) CaptureSection(section, capture(state));
        }

        private void TickPlans130To133(int day)
        {
            _powderMetallurgy?.TickDay(day);
            _nvisCommunications?.TickDay(day);
            _lyophilization?.TickDay(day);
            _draisineRerailing?.TickDay(day);
        }

        private void ResetPlans130To133Panel()
        {
            _plans130To133Panel?.Unbind();
            if (_plans130To133Panel != null && _plans130To133Panel.IsInsideTree())
                RemoveChild(_plans130To133Panel);
            _plans130To133Panel = null;
        }

        private void OpenPlans130To133Panel()
        {
            SetupPlans130To133();
            _plans130To133Panel!.Open();
        }

        /// <summary>
        /// Binds route hazard/travel modifiers into the expedition estimate
        /// preview and refreshes the live encounter composer so minefields
        /// and ground rails affect player-facing travel numbers.
        /// </summary>
        private void WirePlans146ExpeditionRouteModifiers()
        {
            if (_routeInfrastructure == null) return;
            if (_expeditions != null)
            {
                var routes = _routeInfrastructure;
                // D16 — the wasteland-map route hazard evaluator owns traversal
                // feasibility (flooding, amphibious waterways, seasonal mud,
                // radiation hotspots). The route-infrastructure system still owns
                // minefield / rail-roughness modifiers; both are composed here so
                // the expedition estimate has a single projection point.
                _expeditions.SetEstimateRouteModifiers(
                    locationId => routes.GetHazardModifier(locationId) * GetRouteTraversalHazardMultiplier(locationId),
                    locationId => routes.GetTravelModifier(locationId) * GetRouteTraversalDelayMultiplier(locationId));
            }
            // Reinstall the encounter composer so GetHazardModifier is included
            // even when evolving-world wiring ran before route setup.
            if (_expeditions != null)
            {
                _expeditionDangerComposer = ComposeExpeditionDangerMultiplier();
                _expeditions.SetEncounterChanceMultiplier(_expeditionDangerComposer);
            }
        }

        // ─── Catalog authority (JSON wins; engine seeds remain as fallback) ───

        private string ResolvePlans146DataDir() =>
            string.IsNullOrEmpty(_dataDir) ? CatalogPath.ResolveDataDir() : _dataDir;

        private void SetupPlans146To149()
        {
            SetupRouteInfrastructure();
            SetupEbPvdCoating();
            SetupMicrofluidicDiagnostic();
            SetupMineClearingFlail();
            SetupRailGrinding();
            WirePlans146ExpeditionRouteModifiers();
        }

        private string ResolvePlans146OperatorId()
        {
            if (_survivors != null)
            {
                foreach (var s in _survivors.RosterState)
                {
                    if (s != null && s.IsAliveState && !string.IsNullOrEmpty(s.Id))
                        return s.Id;
                }
            }
            return "shelter_operator";
        }

        private bool TryConsumePlans146Demands(System.Collections.Generic.IReadOnlyList<InventoryDemand> demands)
        {
            if (_inventory == null || demands == null || demands.Count == 0) return false;
            var bill = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < demands.Count; i++)
            {
                var d = demands[i];
                if (d == null || string.IsNullOrEmpty(d.ItemId) || d.Quantity <= 0) continue;
                if (bill.TryGetValue(d.ItemId, out int existing))
                    bill[d.ItemId] = existing + d.Quantity;
                else
                    bill[d.ItemId] = d.Quantity;
            }
            if (bill.Count == 0) return false;
            return _inventory.Inventory.TryConsumeBill(bill);
        }

        private static bool TrySplitPlans146Param(string param, out string left, out string right)
        {
            left = string.Empty;
            right = string.Empty;
            if (string.IsNullOrEmpty(param)) return false;
            int sep = param.IndexOf('|');
            if (sep <= 0 || sep >= param.Length - 1) return false;
            left = param.Substring(0, sep);
            right = param.Substring(sep + 1);
            return !string.IsNullOrEmpty(left) && !string.IsNullOrEmpty(right);
        }

        // Composite orchestration only; the child SaveXxx methods are the
        // registered campaign sections.
        private void PersistPlans146To149()
        {
            SaveRouteInfrastructure();
            SaveEbPvdCoating();
            SaveMicrofluidicDiagnostic();
            SaveMineClearingFlail();
            SaveRailGrinding();
        }

        // ─── Tick (day cadence) ───

        // One trained-operator working shift per campaign day. Jobs measured in
        // machine-hours (EB-PVD 3-6h) or machine-minutes (assays 25-60min) resolve
        // within a day-to-two-day cadence at this rate.
        private const float Plans146MachineHoursPerDay = 8f;

        private void TickPlans146To149(int day)
        {
            if (_ebPvdCoating != null)
            {
                float requiredKw = _ebPvdCoating.System.StateDto.ActiveJob?.BeamPowerKw ?? 10f;
                _ebPvdCoating.TickProcess(Plans146MachineHoursPerDay, BuildPlans146PowerContext(requiredKw), null, _ebpvdRng!);
            }

            if (_microfluidicDiagnostic != null)
            {
                // Clinical truth comes from the disease authority only; without it
                // the engine resolves runs as indeterminate rather than guessing.
                Func<string, string, bool>? clinicalTruth = null;
                if (_disease != null)
                {
                    var diseaseEngine = _disease.Engine;
                    clinicalTruth = (patientId, diseaseId) => diseaseEngine.IsInfected(patientId, diseaseId);
                }
                _microfluidicDiagnostic.Tick(
                    Plans146MachineHoursPerDay * 60f,
                    BuildPlans146PowerContext(_microfluidicDiagnostic.System.NominalPowerKw),
                    null,
                    _microfluidicRng!,
                    clinicalTruth);
            }

            if (_mineClearingFlail != null && _routeInfrastructure != null)
            {
                _mineClearingFlail.TickBreach(Plans146MachineHoursPerDay, day, _routeInfrastructure, null, _mineFlailRng!);
            }

            if (_railGrinding != null && _routeInfrastructure != null)
            {
                _railGrinding.TickGrinding(Plans146MachineHoursPerDay, day, _routeInfrastructure, null, _railGrindingRng!);
            }
        }

        /// <summary>
        /// Projects the power grid into the shared machine power context. Without a
        /// grid session the machines assume supply; otherwise net surplus (kW) feeds
        /// the brownout/throttle math owned by the Core engines.
        /// </summary>
        private PowerSupplyContext BuildPlans146PowerContext(float requiredKw)
        {
            var grid = _powerGrid?.System;
            if (grid == null)
            {
                return PowerSupplyContext.Full(requiredKw);
            }
            float availableKw = Math.Max(0f, grid.NetWatts) / 1000f;
            if (availableKw >= requiredKw)
            {
                return PowerSupplyContext.Full(Math.Max(availableKw, requiredKw));
            }
            return PowerSupplyContext.Throttled(availableKw, Math.Max(requiredKw, 0.001f));
        }

        // ─── UI Panel Construction & Binding ───

        private void EnsurePlans146To149Panels()
        {
            if (_ebPvdCoatingPanel != null) return;
            BuildPlans146To149Panels();
        }

        private void BuildPlans146To149Panels()
        {
            SetupPlans146To149();

            _ebPvdCoatingPanel = new EbPvdCoatingPanel();
            _ebPvdCoatingPanel.Visible = false;
            _ebPvdCoatingPanel.OnClose += () => { if (_ebPvdCoatingPanel != null) _ebPvdCoatingPanel.Visible = false; };
            _ebPvdCoatingPanel.OnActionRequested += HandleEbPvdCoatingAction;
            if (_ebPvdCoating != null) _ebPvdCoatingPanel.Bind(_ebPvdCoating);
            AddChild(_ebPvdCoatingPanel);

            _microfluidicDiagnosticPanel = new MicrofluidicDiagnosticPanel();
            _microfluidicDiagnosticPanel.Visible = false;
            _microfluidicDiagnosticPanel.OnClose += () => { if (_microfluidicDiagnosticPanel != null) _microfluidicDiagnosticPanel.Visible = false; };
            _microfluidicDiagnosticPanel.OnActionRequested += HandleMicrofluidicDiagnosticAction;
            if (_microfluidicDiagnostic != null) _microfluidicDiagnosticPanel.Bind(_microfluidicDiagnostic);
            AddChild(_microfluidicDiagnosticPanel);

            _mineFlailPanel = new MineFlailPanel();
            _mineFlailPanel.Visible = false;
            _mineFlailPanel.OnClose += () => { if (_mineFlailPanel != null) _mineFlailPanel.Visible = false; };
            _mineFlailPanel.OnActionRequested += HandleMineFlailAction;
            if (_mineClearingFlail != null) _mineFlailPanel.Bind(_mineClearingFlail);
            AddChild(_mineFlailPanel);

            _railGrindingPanel = new RailGrindingPanel();
            _railGrindingPanel.Visible = false;
            _railGrindingPanel.OnClose += () => { if (_railGrindingPanel != null) _railGrindingPanel.Visible = false; };
            _railGrindingPanel.OnActionRequested += HandleRailGrindingAction;
            if (_railGrinding != null) _railGrindingPanel.Bind(_railGrinding);
            AddChild(_railGrindingPanel);
        }

        /// <summary>
        /// Composes the Plans 166–169 campaign seams. Existing ResearchSystem,
        /// WaterTreatmentSystem, faction authorities, and domain quest systems
        /// remain owners of their facts; these sessions only add the new
        /// progression, distribution, intelligence, and shared quest edges.
        /// </summary>
        private void SetupPlans166To169()
        {
            SetupCampaignDay();
            SetupSurvivors();
            SetupInventory();
            SetupWorld();
            SetupPowerGrid();
            SetupCrafting();
            SetupWaterTreatment();
            SetupDisease();
            SetupPsyOps();
            SetupYearOfAsh();
            EnsureCaravanTrade();
            _sharedResearch = EnsureSharedResearch();

            if (_espionage166 == null)
            {
                var system = new EspionageSystem(new GodotLog());
                system.BindRng(_campaignDay.Rng.Fork("espionage"));
                system.BindAgentAvailability(
                    id => _survivors?.Find(id)?.IsAliveState == true,
                    id => _survivors?.Find(id)?.IsAliveState == true && !_espionageAgentsAway.Contains(id),
                    (id, away) =>
                    {
                        if (away) _espionageAgentsAway.Add(id);
                        else _espionageAgentsAway.Remove(id);
                    });
                system.BindAgentCapability(_ => 1f);
                system.BindResearchGate(id =>
                    _sharedResearch?.GetKnowledge(id)?.isCompleted == true
                    || _sharedResearch?.State.completedIds?.Contains(id) == true);
                _espionage166 = EspionageHostSession.Create(_dataDir, system);
                _espionage166.StateChanged += () => _espionage166Dirty = true;
                var saved = EspionageSaveStore.TryLoad();
                if (saved != null) _espionage166.RestoreState(saved);
            }

            BindEspionageConsequenceRouting();

            if (_fluidLogistics168 == null)
            {
                var system = new FluidLogisticsSystem(new GodotLog());
                system.BindRng(_campaignDay.Rng.Fork("fluid_logistics"));
                system.EnsureDefaultShelterTopology();
                _fluidLogistics168 = FluidLogisticsHostSession.Create(_dataDir, system);
                _fluidLogistics168.StateChanged += () => _fluidLogistics168Dirty = true;
                var saved = FluidLogisticsSaveStore.TryLoad();
                if (saved != null)
                {
                    _fluidLogistics168.RestoreState(saved);
                    // Reload may restore a partial/empty graph; re-seed defaults.
                    _fluidLogistics168.System.EnsureDefaultShelterTopology();
                }
            }

            if (_proceduralNarrative169 == null)
            {
                _proceduralNarrative169 = ProceduralNarrativeHostSession.Create(_dataDir);
                _proceduralNarrative169.StateChanged += () => _proceduralNarrative169Dirty = true;
                var saved = ProceduralNarrativeSaveStore.TryLoad();
                if (saved != null) _proceduralNarrative169.RestoreState(saved.narrative, saved.quests);
            }
        }

        // ── Daily Tick Advance ──────────────────────────────────────────

        public void TickPlans178_181(int currentDay)
        {
            EnsureGenerational().GrowthTick(currentDay);
            EnsurePrisoners().TickUpkeepAndEscape(currentDay);
        }

        // ── Daily Simulation Advance Hook ─────────────────────────────────

        public void TickPlans182_185(int currentDay)
        {
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("plans182_185") : new SeededRng(182);

            // Plan 182: Aviation tick for active flights
            var av = EnsureAviation();
            var active = new List<FlightPlan>(av.ActiveFlights);
            foreach (var flight in active)
            {
                av.AdvanceFlightTick(flight.flightId, 4.0f, 15.0f, 0.8f, -5.0f, 0.2f, rng);
            }

            // Plan 183: Forced Labor daily shift
            EnsureForcedLabor().AdvanceDailyShift(rng);

            // Plan 184: Narcotics medical tick (24h tick)
            EnsureNarcotics().AdvanceMedicalTick(24.0f, rng);

            // Plan 185: Politics daily progression
            float foodSat = 0.8f;
            float secSat = 0.75f;
            float cruelty = EnsureForcedLabor().CrueltyIndex;
            EnsurePolitics().AdvanceDailyPolitics(foodSat, secSat, cruelty, 0, rng);
        }

        // ── Daily / Hourly Tick Coordination ─────────────────────────────

        public void TickPlans186_189(int day, float deltaHours)
        {
            if (_fallout != null)
            {
                var zones = new Dictionary<string, (float x, float y)>(StringComparer.Ordinal)
                {
                    { "loc_holdfast", (0f, 0f) },
                    { "loc_river_delta", (25f, -10f) },
                    { "loc_silo_ruins", (-30f, 40f) },
                    { "loc_chemical_plant", (15f, 35f) }
                };
                _fallout.Tick(deltaHours, 45.0f, 15.0f, zones);
            }

            if (_mercenary != null)
            {
                // Deterministic board refresh at the day boundary (no-op when
                // already generated for this day; Core owns the seed).
                _mercenary.GenerateBoard(day, _mercenaryCandidateTargets ?? (System.Collections.Generic.IReadOnlyList<string>)Array.Empty<string>());
                _mercenary.TickDay(day);
            }
        }

        // ── Daily Tick Orchestration for Plans 190-193 ───────────────────

        private void TickPlans190_193(int currentDay)
        {
            _amputation?.TickDay(currentDay);
            _railway?.TickDay(currentDay);

            // Plan 204: project real per-room thermal state into the fungi beds when the
            // shelter thermal authority is live; otherwise the Core default band applies.
            if (_fungi != null)
            {
                var thermalRooms = _shelterThermal?.System.State.rooms;
                if (thermalRooms != null && thermalRooms.Count > 0)
                {
                    var temps = new Dictionary<string, float>(StringComparer.Ordinal);
                    for (int i = 0; i < thermalRooms.Count; i++)
                        temps[thermalRooms[i].roomId] = thermalRooms[i].currentTempC;
                    _fungi.TickDay(currentDay, roomTemperatureOverride: roomId =>
                        temps.TryGetValue(roomId, out var t) ? t : 15f);
                }
                else
                {
                    _fungi.TickDay(currentDay);
                }
            }

            _justice?.TickDay(currentDay);
        }

        // ── Daily Tick Coordination ──────────────────────────────────────

        public void TickPlans194_197(int day, List<Ashfall.Core.Campaign.DayStateChangeEvent>? events = null)
        {
            _recreation?.TickDay(day);
        }

        // ── Daily Tick Orchestration for Plans 198-201 ──────────────────

        private void TickPlans198_201(int day, List<DayStateChangeEvent> events)
        {
            float gridWatts = _powerGrid?.System != null ? _powerGrid.System.GenerationWatts : 1000f;
            // C2[6] 23A: the comms array is the radio-room load; use the
            // allocation-aware served state so a brownout that still serves
            // room_radio_tuner keeps the array online.
            bool gridPowered = _powerGrid?.System == null
                || _powerGrid.System.IsRoomServed("room_radio_tuner");

            if (_chemWarfare != null)
            {
                var weather = _world != null ? _world.Weather.Current : WeatherKind.Clear;
                _chemWarfare.TickCombat(weather, 0, 1);
                _chemWarfareDirty = true;
            }

            if (_commsArray != null)
            {
                _commsArray.SetPowerState(gridPowered, gridWatts);
                _commsArray.TickScan(day, 12, 0.5f);
                _commsArrayDirty = true;
            }

            if (_ceremonySystem != null)
            {
                _ceremonySystem.TickDay(day, out _);
                _ceremonyDirty = true;
            }

            if (_robotics != null)
            {
                _robotics.TickLabor(24, gridPowered, gridWatts);
                _roboticsDirty = true;
            }
        }

        /// <summary>Atomically consumes one bill through the canonical
        /// inventory authority. Returns false (nothing consumed) if unaffordable.</summary>
        private bool TryPayBill(InventoryBill bill)
        {
            if (_inventory?.Inventory == null) return false;
            using var tx = _inventory.Inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid) return false;
            return tx.TryCommit();
        }

        /// <summary>Refund path for a committed bill whose Core command was
        /// rejected afterwards — restores the same items.</summary>
        private void RefundBill(InventoryBill bill)
        {
            if (_inventory?.Inventory == null) return;
            foreach (var cost in bill.Costs)
                _inventory.Inventory.TryProduce(cost.ItemId, cost.Amount);
        }

        // ── Daily Tick Orchestration for Plans 46-49 ───────────────────

        private void TickPlans46_49(int day, List<DayStateChangeEvent> events)
        {
            // Canonical campaign day drives internal notice expiry. This is
            // deliberately called once from the existing world/day seam; no
            // second day owner or wall-clock timer is introduced.
            TickInternalCommunication(day);

            // Plan 218 — daily museum exhibition expiry rides the same existing
            // world/day orchestration seam; no second day owner is introduced.
            TickShelterMuseum(day);

            if (_shelterWorkshop != null)
            {
                _shelterWorkshop.TickDay(day);
                _shelterWorkshopDirty = true;

                foreach (var job in _shelterWorkshop.State.jobs)
                {
                    if (job.Status == WorkshopJobStatus.Completed || job.Status == WorkshopJobStatus.CompletedPendingCollection)
                    {
                        if (_shelterWorkshop.Recipes.TryGetValue(job.RecipeId, out var recipe))
                        {
                            events.Add(new DayStateChangeEvent("workshop_job_completed", "shelter_workshop", recipe.DisplayName, job.RoomId, job.YieldProduced));
                        }
                    }
                }

                foreach (var machine in _shelterWorkshop.State.machines.Values)
                {
                    if (machine.ToolingHealth <= 0.25f)
                    {
                        events.Add(new DayStateChangeEvent("workshop_machine_degraded", "shelter_workshop", machine.RoomId, null, machine.ToolingHealth));
                    }
                }
            }

            if (_radioStationSystem != null)
            {
                // Plan 71: room_radio_tuner — monitoring pauses while the radio
                // room is unpowered (level gate). Expiry timestamps are absolute,
                // so intercepts batch-expire on restoration; no state is lost.
                bool radioPowered = _powerGrid?.System?.IsRoomPowered("room_radio_tuner") ?? true;
                if (radioPowered)
                    _radioStationSystem.TickDay(day);
                _radioStationDirty = true;

                foreach (var progress in _radioStationSystem.State.intercepts)
                {
                    if (!progress.IsExpired && progress.ExpiresOnDay.HasValue)
                    {
                        int daysLeft = progress.ExpiresOnDay.Value - day;
                        if (daysLeft <= 1)
                        {
                            events.Add(new DayStateChangeEvent("radio_distress_expiring", "radio_station", progress.InterceptId, null, daysLeft));
                        }
                        else
                        {
                            events.Add(new DayStateChangeEvent("radio_distress_active", "radio_station", progress.InterceptId, null, daysLeft));
                        }
                    }
                }
            }

            if (_shelterSocialDynamics != null)
            {
                _shelterSocialDynamics.TickDay(day);
                _shelterSocialDirty = true;

                foreach (var profile in _shelterSocialDynamics.State.privacyProfiles.Values)
                {
                    if (profile.PrivacyFatiguePermille >= 700)
                    {
                        events.Add(new DayStateChangeEvent("social_privacy_warning", "shelter_social_dynamics", profile.SurvivorId, profile.AssignedRoomId, profile.PrivacyFatiguePermille));
                    }
                }

                foreach (var inc in _shelterSocialDynamics.State.recentIncidents)
                {
                    if (inc.Day == day && !inc.Resolved)
                    {
                        events.Add(new DayStateChangeEvent("social_dispute_unresolved", "shelter_social_dynamics", inc.IncidentId, inc.RoomId, 0f));
                    }
                }
            }

            if (_excavationHazards != null)
            {
                _excavationHazards.TickDay(day);
                _excavationHazardsDirty = true;

                foreach (var sec in _excavationHazards.State.sectors.Values)
                {
                    if (sec.MethanePpm >= 2500)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_methane_warning", "excavation_hazards", sec.SectorId, null, sec.MethanePpm));
                    }

                    if (sec.FloodLevelPermille >= 500)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_flood_warning", "excavation_hazards", sec.SectorId, null, sec.FloodLevelPermille));
                    }

                    if (sec.ShoringHealthPermille <= 300)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_shoring_warning", "excavation_hazards", sec.SectorId, null, sec.ShoringHealthPermille));
                    }

                    if (sec.ActiveTrappedMiners.Count > 0 && !sec.RescueCompleted && !sec.RescueFailed)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_rescue_active", "excavation_hazards", sec.SectorId, null, sec.RescueLaborRemaining));
                    }
                    else if (sec.RescueFailed)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_rescue_failed", "excavation_hazards", sec.SectorId, null, 0f));
                    }
                }
            }

            if (_dynamicQuests != null)
            {
                _dynamicQuests.TickDay(day);
                _dynamicQuestsDirty = true;
            }
        }

        // ── Combined Lifecycle ───────────────────────────────────────────

        public void SetupPlans50To53()
        {
            EnsureVehicleGarage();
            SetupShelterEspionage();
            SetupSurvivorMentalHealth();
            EnsureShelterAcoustics();
        }

        public void FlushPlans50To53()
        {
            if (_vehicleGarageDirty) SaveVehicleGarage();
            if (_shelterEspionageDirty) SaveShelterEspionage();
            if (_survivorMentalHealthDirty) SaveSurvivorMentalHealth();
        }

        public void TickPlans50To53(int currentDay)
        {
            if (_shelterEspionage != null)
            {
                _shelterEspionage.TickDay(currentDay, _inventory?.Inventory);
                _shelterEspionageDirty = true;
            }

            if (_survivorMentalHealth != null)
            {
                _survivorMentalHealth.TickDay(currentDay);
                _survivorMentalHealthDirty = true;
            }

            _shelterAcousticBridge?.SyncAcoustics();
        }

        /// <summary>
        /// Composes Plans 62–65 late-midgame systems:
        /// 1. Plan 62: Deep-Strata Pre-war Archive Decryption
        /// 2. Plan 63: Raider Captives & Penal Labor
        /// 3. Plan 64: Food Spoilage & Cryogenic Preservation
        /// 4. Plan 65: Grand Epilogue Simulator
        /// </summary>
        private void SetupPlans62To65()
        {
            SetupCampaignDay();
            SetupSurvivors();
            SetupInventory();
            SetupPowerGrid();

            if (_inventory == null) return;

            // 1. Food Preservation (Plan 64)
            if (_foodPreservation64 == null)
            {
                var foodCatalog = FoodPreservationCatalogLoader.Load(_dataDir, new FileSystemIO());
                var rng = _campaignDay != null ? _campaignDay.Rng.Fork("food_preservation") : new SeededRng(64);
                _foodPreservation64 = new FoodPreservationSystem(rng, _inventory.Inventory, foodCatalog, new GodotLog());

                var savedFood = FoodPreservationSaveStore.TryLoad();
                if (savedFood != null)
                {
                    _foodPreservation64.RestoreState(savedFood);
                }

                _foodPreservation64.OnFoodSpoiled += _ => _foodPreservation64Dirty = true;
                _foodPreservation64.OnCuringCompleted += _ => _foodPreservation64Dirty = true;
                _foodPreservation64.OnFoodConsumed += (_, _, _) => _foodPreservation64Dirty = true;
            }

            // CORE-MECH W1: foodborne disease bridge. The holdfast session raises
            // one FoodConsumed event per successful meal; this handler is the
            // single translation point from preservation spoilage to the disease
            // authority's exposure pipeline (AA.1 receiver contract).
            if (_holdfastRuntime != null)
                _holdfastRuntime.FoodConsumed += OnFoodConsumedForSpoilage;

            // 2. Pre-war Archive Decryption (Plan 62)
            if (_archiveDecryption62 == null)
            {
                var archiveCatalog = PrewarArchiveCatalogLoader.Load(_dataDir, new FileSystemIO());
                var rng = _campaignDay != null ? _campaignDay.Rng.Fork("archive_decrypt") : new SeededRng(62);
                _archiveDecryption62 = new PrewarArchiveDecryptionSystem(rng, _inventory.Inventory, archiveCatalog, new GodotLog());

                var savedArchives = PrewarArchiveSaveStore.TryLoad();
                if (savedArchives != null)
                {
                    _archiveDecryption62.RestoreState(savedArchives);
                }

                _archiveDecryption62.OnArchiveDecrypted += (_, _) => _archiveDecryption62Dirty = true;
                _archiveDecryption62.OnArchiveDiscovered += _ => _archiveDecryption62Dirty = true;
            }

            // 3. Shelter Prisoner System (Plan 63) — RETIRED as a live authority
            // by ORPHAN-SEAL-W1 (2026-09-23): PrisonerSystem (Plan 179) is the
            // single captive authority (§6.13-6.14); the Plan 63 class and its
            // save store remain for history, and EnsurePrisoners() imports the
            // legacy shelter_prisoners section once on upgrade.

            // 4. Grand Epilogue Engine (Plan 65)
            if (_epilogueEngine65 == null)
            {
                var epilogueCatalog = CampaignEpilogueCatalogLoader.Load(_dataDir, new FileSystemIO());
                _epilogueEngine65 = new CampaignEpilogueEngine(epilogueCatalog);
            }
        }

        public void TickPlans62To65(int day)
        {
            bool isPowerOnline = _powerGrid?.System == null || !_powerGrid.System.IsBrownout;

            if (_foodPreservation64 != null)
            {
                // C2[6] 23A: powered refrigeration follows the served power of the
                // kitchen/preservation load rather than the global brownout. A
                // brownout that still serves room_kitchen keeps cold storage live;
                // a shed kitchen lets it warm on the 22B thermal-mass curve.
                bool preservationPower = _powerGrid?.System == null
                    || _powerGrid.System.IsRoomServed("room_kitchen");
                _foodPreservation64.SetPowerStatus(preservationPower);
                // Plan 196: project storage-bay °C from thermal authority (no weather copy).
                float storageTempC = FoodPreservationSystem.DefaultStorageTemperatureC;
                var thermalRooms = _shelterThermal?.System.State.rooms;
                if (thermalRooms != null)
                {
                    for (int i = 0; i < thermalRooms.Count; i++)
                    {
                        if (thermalRooms[i].roomId == FoodPreservationSystem.DefaultStorageRoomId)
                        {
                            storageTempC = thermalRooms[i].currentTempC;
                            break;
                        }
                    }
                }
                _foodPreservation64.SetStorageTemperatureC(storageTempC);
                _foodPreservation64.TickDay(day);
                _foodPreservation64Dirty = true;
            }

            if (_archiveDecryption62 != null)
            {
                _archiveDecryption62.SetPowerStatus(isPowerOnline);
                _archiveDecryption62.TickDay(day);
                _archiveDecryption62Dirty = true;
            }
        }

        private void ComposePlans74To77()
        {
            SetupPowerGrid();
            SetupInventory();
            SetupCrafting();
            SetupEquipmentCondition();

            SetupGeothermalOrc();
            SetupBallisticsWorkbench();
            SetupAeroponics();
            SetupPneumaticDispatch();
        }

        private void SetupPlans78To81()
        {
            SetupGeodeticSurvey();
            SetupKineticStorage();
            SetupChemicalRecon();
        }

        // Composite orchestration only; the child SaveXxx methods are the
        // registered campaign sections.
        private void PersistPlans78To81()
        {
            SaveGeodeticSurvey();
            SaveKineticStorage();
            SaveChemicalRecon();
        }

        // ─── Tick (single simulation owner per system; day cadence) ───

        private void TickPlans78To81(int day)
        {
            _geodeticSurvey?.System.TickDay(day);
            _kineticStorage?.System.TickDay(day);
            _chemicalRecon?.System.TickDay(day);
        }
        private UI.Plans94To97Panel? _plans94To97Panel;

        private void SetupPlans94To97Panel()
        {
            if (_plans94To97Panel == null)
            {
                _plans94To97Panel = new UI.Plans94To97Panel();
                AddChild(_plans94To97Panel);
                _plans94To97Panel.OnClose += () => _plans94To97Panel.Visible = false;
            }
            _plans94To97Panel.Bind(
                _grainProcessing!, _cryogenicAirSeparation!, _heliograph!, () => _simDay);
        }

        private void TickPlans94To97(int day)
        {
            _grainProcessing?.TickDay(day);
            _cryogenicAirSeparation?.TickDay(day);
        }

    }
}
