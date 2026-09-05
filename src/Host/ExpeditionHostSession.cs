using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Ashfall.Core.PlayerCommand;
using Ashfall.Core.World;

#pragma warning disable CS8618

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the expedition core.
    /// Manages expedition definitions, drives day/hour ticks with a seeded RNG,
    /// and persists active expeditions. Presentation and wiring only — all rules in Core.
    /// </summary>
    public sealed class ExpeditionHostSession : HostSessionBase
    {
        public const int DemoSeed = 7071;

        /// <summary>Deterministic seed for the vehicle garage's own rolls (prep breakdowns).</summary>
        public const int VehicleSeed = 7072;

        /// <summary>Host-side km per travel tick — bridges the tick economy to the km-based vehicle math.</summary>
        public const float KmPerTravelTick = 2.5f;

        /// <summary>Vehicle granted to a fresh shelter so vehicle logistics are live from day one.</summary>
        public const string StarterVehicleId = "vehicle_utility_quad";

        public ExpeditionSystem Engine { get; }
        public List<ExpeditionDefinition> Definitions { get; }
        public List<ExpeditionDefinition> DemoDefinitions => Definitions;
        public DiveInstanceRunner DiveRunner { get; private set; }
        public Ashfall.Core.Flags.IFlagLedger Flags { get; set; } = new Ashfall.Core.Flags.CampaignConsequenceLedger();

        /// <summary>F17 — host wire into the canonical disease authority for
        /// micro-location hazard consequences (same lazy-delegate pattern as
        /// WildlifeTrappingHostSession.ApplyDisease). Signature:
        /// (survivorId, diseaseId, day). Null (headless) degrades honestly:
        /// hazards report SkippedNoAuthority and resolution still succeeds.</summary>
        public Action<string, string, int>? ApplyDisease { get; set; }

        /// <summary>F3 — journal authority for encounter knowledge unlocks.
        /// Null (headless) degrades honestly: unlocks are skipped, resolution
        /// still succeeds.</summary>
        public JournalSystem? Journal { get; set; }

        /// <summary>F2 — shelter inventory authority for negative item deltas
        /// (offerings). Consumed only through the canonical TryConsume
        /// transaction; never underflows.</summary>
        public Ashfall.Core.Inventory.Inventory? ShelterInventory { get; set; }

        /// <summary>F2 — item catalog for loot weights and display names.
        /// Falls back to the scavenging convention (1 kg/item) when unset.</summary>
        public ItemCatalog? Items { get; set; }

        /// <summary>Vehicle garage (fuel, condition, repair) — persisted inside the expedition aggregate.</summary>
        public ExpeditionVehicleSystem Vehicles { get; }

        /// <summary>Optional crossing gate — when set, crossing-node expeditions require vouch access.</summary>
        public VouchAccessSystem CrossingGate { get; set; }

        /// <summary>
        /// Optional extra dispatch gate (ice road seasonal + deep-coast route
        /// stage). When set, any location it blocks cannot be dispatched.
        /// </summary>
        public Func<string, bool> ExtraBlocked { get; set; }

        /// <summary>
        /// Optional reason-carrying dispatch gate (GAP-48A): returns a
        /// player-facing block for a location, or null when passable.
        /// Evaluated after <see cref="ExtraBlocked"/>; either gate blocks.
        /// The block carries the weather gate's force cost (GAP-48B).
        /// </summary>
        public Func<string, WeatherGateBlock?> ExtraGateBlock { get; set; }

        /// <summary>Player-facing block reason for a location, or null when
        /// dispatchable. Composes the crossing gate, the boolean extra gate,
        /// and the reason-carrying extra gate.</summary>
        public string? GetBlockReason(string locationId)
        {
            if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
                return "Crossing gate closed — no vouch";
            if (ExtraBlocked != null && ExtraBlocked(locationId))
                return "Route blocked";
            // Plan 85 — hidden installations stay undispatchable until the
            // treasure map is completed and the site is revealed.
            if (Engine.DamagedMap != null && Engine.DamagedMap.IsDestinationLocked(locationId))
                return "Map incomplete — location unidentified";
            // F4 — clue-gated destinations stay undispatchable until an
            // expedition encounter (e.g. observation post) reveals them.
            var def = Definitions.Find(d => d != null && d.id == locationId);
            if (def != null && def.requiresDiscovery && !Engine.IsLocationKnown(locationId))
                return "Location unidentified — no clues found yet";
            return ExtraGateBlock?.Invoke(locationId)?.ShortReason;
        }

        /// <summary>The weather gate blocking a location, or null when the
        /// location is passable or blocked by a non-weather gate.</summary>
        public WeatherGateBlock? GetWeatherGateBlock(string locationId)
            => ExtraGateBlock?.Invoke(locationId);

        /// <summary>Passthrough to the Core per-location encounter-chance multiplier (faction/territory danger).</summary>
        public void SetEncounterChanceMultiplier(Func<string, float> multiplier) => Engine.SetEncounterChanceMultiplier(multiplier);

        private int _currentDay;
        /// <summary>Current sim day, supplied by Main so EncounterApplyChoice can pass day to Core.</summary>
        public int CurrentDay
        {
            get => _currentDay;
            set
            {
                _currentDay = value;
                if (_bridge != null)
                    _bridge.CurrentDay = value;
            }
        }

        private string _currentSeason = "autumn";
        public string CurrentSeason
        {
            get => _currentSeason;
            set
            {
                _currentSeason = value;
                if (_bridge != null)
                    _bridge.CurrentSeason = value;
            }
        }

        public string LastEvent { get; private set; } = string.Empty;
        /// <summary>Fired when Core rolls an encounter and the bridge surfaces a DTO. Host UI subscribes here.</summary>
        public event Action<ExpeditionEncounterBridge.EncounterSurfaced>? OnEncounterSurfaced;

        /// <summary>
        /// Plan 45 phase 2 — fired when a resolved travel-encounter choice
        /// escalates to tactical combat. Carries the EnemyCompositionSelector
        /// composition (wildlife pack / raid crew) so the host starts the
        /// fight with catalog enemies instead of the legacy template.
        /// </summary>
        public sealed class TravelCombatTrigger
        {
            public string EncounterId = string.Empty;
            public string Title = string.Empty;
            public string LocationId = string.Empty;
            public int DangerLevel;
            public IReadOnlyList<string> CombatantIds = Array.Empty<string>();
        }
        public event Action<TravelCombatTrigger>? OnTravelEncounterCombatTriggered;

        /// <summary>GAP-48B — raised after a successful forced entry through a
        /// weather gate. The radiation owner applies `block.ForceRadDose` to
        /// the survivor; the stamina cost is already applied by the engine.</summary>
        public event Action<string, string, WeatherGateBlock>? OnWeatherGateForced;

        private TravelEncounterSystem? _travelEngine;
        /// <summary>The Plan 20 wasteland-inhabitants encounter engine. Null outside Create(dataDir) hosts — combat binding degrades honestly.</summary>
        public TravelEncounterSystem? TravelEngine
        {
            get => _travelEngine;
            set
            {
                _travelEngine = value;
                if (_bridge != null)
                    _bridge.TravelEngine = value;
            }
        }

        /// <summary>When true (default), the UI shows a modal encounter notice. When false, a transient autoplay banner.</summary>
        public static bool UseEncounterModal { get; set; } = true;

        /// <summary>
        /// Encounter surfacing bridge. Read-only so hosts can drive the surface
        /// pipeline; Core owns all encounter rules.
        /// </summary>
        public ExpeditionEncounterBridge Bridge => _bridge;

        private readonly ExpeditionEncounterBridge _bridge;
        private readonly ISeededRng _rng;
        private readonly NarrativeEncounterSystem _narrative;
        private readonly ExpeditionNavalSystem _naval = new();

        /// <summary>
        /// Destinations reachable by water from the home holdfast, mapped to
        /// the water route's weather hazard. Populated by the host from the
        /// wasteland map authority (routes with travel_domain "water"). When
        /// a dispatch targets one of these, the sortie travels by river craft
        /// (ExpeditionNavalSystem profile projection) and piracy risk is
        /// folded into the encounter chance.
        /// </summary>
        public Dictionary<string, float> WaterRouteHazards { get; } = new(StringComparer.Ordinal);

        private static readonly IReadOnlyList<PendingSurfacedEncounter> NoPending =
            new List<PendingSurfacedEncounter>(0);

        /// <summary>
        /// Surfaced-but-unresolved encounters from this trip, straight off
        /// NarrativeEncounterState.pending (the save DTO). Read-only for UI.
        /// </summary>
        public IReadOnlyList<PendingSurfacedEncounter> Pending =>
            _narrative?.State?.pending ?? NoPending;

        /// <summary>Resolve a pending entry's catalog definition, or null when the catalog has no record.</summary>
        public EncounterDefinition? FindEncounter(string encounterId) => _narrative?.Find(encounterId);

        /// <summary>Drop the pending queue without resolving. No invented outcomes.</summary>
        public void ClearAllPending() => _narrative?.ClearAllPending();

        /// <summary>
        /// The travel encounter engine this session resolves through. Main
        /// uses it to attach the Plan 52 arc QuestLink (encounter decisions
        /// land in the persisted expansion-quest ledger). Read-only.
        /// </summary>
        public NarrativeEncounterSystem? NarrativeEngine => _narrative;

        public ExpeditionHostSession(ExpeditionSystem engine = null!, NarrativeEncounterSystem narrative = null!)
        {
            Engine = engine ?? new ExpeditionSystem();
            _rng = new SeededRng(DemoSeed);
            _narrative = narrative ?? new NarrativeEncounterSystem();
            _bridge = new ExpeditionEncounterBridge(_narrative, _rng);
            _bridge.RegionResolver = locId =>
            {
                if (string.IsNullOrEmpty(locId)) return "the_toll";
                if (locId.Contains("cut") || locId.Contains("toll") || locId.Contains("holdfast")) return "the_toll";
                if (locId.Contains("scarp") || locId.Contains("high") || locId.Contains("ridge")) return "high_scarp";
                if (locId.Contains("industrial") || locId.Contains("depot") || locId.Contains("factory") || locId.Contains("plant") || locId.Contains("arsenal")) return "industrial_belt";
                if (locId.Contains("shelf") || locId.Contains("coast") || locId.Contains("flotilla") || locId.Contains("drown")) return "coastal_shelf";
                if (locId.Contains("suburb") || locId.Contains("house") || locId.Contains("hospital") || locId.Contains("gas")) return "dead_suburbs";
                return "the_toll";
            };
            Definitions = new List<ExpeditionDefinition>();
            RegisterDefaultDefinitions();
            Vehicles = new ExpeditionVehicleSystem(new SeededRng(VehicleSeed));
            Vehicles.OnVehicleStateChanged += () => RaiseStateChanged();
            Engine.OnVehicleBreakdown += s =>
            {
                LastEvent = $"Vehicle breakdown: {s.survivorId}'s sortie continues on foot.";
                RaiseStateChanged();
            };
            Engine.OnExpeditionStarted += s => { LastEvent = $"Expedition started: {s.survivorId} -> {s.displayName}."; RaiseStateChanged(); };
            Engine.OnExpeditionCompleted += s => { LastEvent = $"Expedition completed: {s.survivorId} returned with {s.loot.Count} loot lines."; RaiseStateChanged(); };
            Engine.OnExpeditionFailed += (s, r) => { LastEvent = $"Expedition failed: {s.survivorId} — {r}"; RaiseStateChanged(); };
            _bridge.OnSurfaced += dto =>
            {
                LastEvent = $"Encounter triggered: {dto.trigger.survivorId} at {dto.trigger.displayName} (#{dto.trigger.encounterCount}) -> {dto.encounter_id ?? "bare-notice"}.";
                if (!string.IsNullOrEmpty(dto.encounter_id))
                {
                    _narrative.EnqueuePending(dto.encounter_id, dto.trigger.locationId, dto.trigger.encounterCount, CurrentDay);
                    // F2 — remember which sortie surfaced the encounter so a
                    // prompt choice routes its loot grant to the right pack.
                    _lastSurfacedEncounterId = dto.encounter_id;
                    _lastSurfacedTriggerSurvivor = dto.trigger?.survivorId ?? string.Empty;
                }
                RaiseStateChanged();
                OnEncounterSurfaced?.Invoke(dto);
            };
            Engine.OnEncounterTriggered += s =>
            {
                _bridge.CurrentDay = CurrentDay;
                _bridge.CurrentSeason = CurrentSeason;
                _bridge.Surface(s);
            };
            Engine.OnStateChanged += _ => RaiseStateChanged();
        }

        private void RegisterDefaultDefinitions()
        {
            var allotments = new ExpeditionDefinition
            {
                id = "loc_the_allotments",
                displayName = "The Works Allotment Commune",
                distanceTicks = 5,
                dangerLevel = 2,
                encounterChancePerTick = 0.12f,
                baseStaminaDrainPerHour = 2.0f,
                lootCategories = new List<string> { "scrap_metal", "clean_water", "bandage", "dried_rations" }
            };
            var cut = new ExpeditionDefinition
            {
                id = "loc_denial_cut_substation",
                displayName = "The Denial Cut Substation",
                distanceTicks = 8,
                dangerLevel = 4,
                encounterChancePerTick = 0.18f,
                baseStaminaDrainPerHour = 3.0f,
                lootCategories = new List<string> { "dosimeter", "copper_wire_10m_of_10m", "fuel", "item_hydro_baron_queue_chit" }
            };
            ExpeditionDefinitionRegistry.Register(allotments);
            ExpeditionDefinitionRegistry.Register(cut);
            Definitions.Add(allotments);
            Definitions.Add(cut);
        }

        public static ExpeditionHostSession Create(string dataDir, NarrativeEncounterSystem narrative = null!, TravelEncounterSystem travel = null!)
        {
            var session = new ExpeditionHostSession(null!, narrative);
            if (travel != null)
            {
                session.TravelEngine = travel;
            }
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();

                // F1–F4 — when no shared narrative engine was passed, the
                // session's own engine must still load the encounter catalog;
                // otherwise micro-locations can never surface or resolve.
                if (session._narrative.Catalog.Count == 0)
                {
                    session._narrative.RegisterRange(
                        NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, serializer));
                }

                var loaded = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);
                if (loaded != null && loaded.Count > 0)
                {
                    session.Definitions.Clear();
                    session.Definitions.AddRange(loaded);
                    foreach (var def in loaded)
                    {
                        if (def != null && !string.IsNullOrEmpty(def.id))
                            ExpeditionDefinitionRegistry.Register(def);
                    }
                }
                session.Vehicles.LoadCatalog(VehicleCatalogLoader.Load(dataDir, fileIO, serializer));

                // Plan 46 — the location-typed scavenging tables are the loot
                // authority for destinations that declare a table id; rolls
                // may also surface damaged-map fragment tokens (Plan 85).
                var scavengeCatalog = ScavengingTableCatalog.LoadFromDirectory(dataDir, fileIO, serializer);
                if (scavengeCatalog.TableCount > 0)
                    session.Engine.ScavengingCatalog = scavengeCatalog;
                // Plan 45 phase 2 — the wasteland-inhabitants layer: creature
                // / human travel encounters resolve through the combat binder.
                if (session.TravelEngine == null)
                {
                    var travelCatalog = TravelEncounterCatalog.LoadFromDirectory(dataDir, fileIO);
                    if (travelCatalog != null && travelCatalog.Count > 0)
                        session.TravelEngine = new TravelEncounterSystem(travelCatalog);
                }
            }
            if (session.TravelEngine != null)
            {
                session._bridge.TravelEngine = session.TravelEngine;
            }

            var save = ExpeditionSaveStore.TryLoad();
            if (save != null)
            {
                // Full aggregate restore: active sorties, garage, F4 known
                // locations, and lifetime CompletedCount for endgame metrics.
                session.RestoreSaveAggregate(save);
                session.LastEvent = "Expedition state restored from save.";
            }
            else if (session.Vehicles.State.ownedVehicles.Count == 0)
            {
                // Fresh shelter: the compound's quad starts in the garage so
                // vehicle logistics participate from the first sortie.
                session.Vehicles.AcquireVehicle(StarterVehicleId);
                session.LastEvent = $"Garage initialized with the shelter's {StarterVehicleId}.";
            }
            return session;
        }

        // ── Production Expedition Actions ─────────────────────────────

        /// <summary>True when the player cannot dispatch to this location right now.</summary>
        public bool IsLocationBlocked(string locationId) => GetBlockReason(locationId) != null;

        /// <summary>Production API to start an expedition to a specified location.
        /// When a vehicleId is given, dispatch preparation runs the garage's
        /// fuel burn, wear, and prep-breakdown roll before the sortie starts.</summary>
        public CommandResult StartExpedition(
            string survivorId,
            string locationId,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            int staminaBudget = 40,
            string vehicleId = "",
            long? stateVersion = null,
            bool forceWeatherGate = false)
        {
            long version = stateVersion ?? StateVersion;

            // Adjudicate caller staleness BEFORE this call mutates anything.
            // Vehicle preparation below burns fuel and rolls wear, which bumps
            // StateVersion; comparing the pre-prepare version against the
            // post-prepare version rejected every successful vehicle dispatch as a
            // stale preview, and did so only after the fuel had been spent.
            if (version != StateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.ExpeditionDispatch, version, StateVersion);

            if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
                return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "crossing_closed", "expedition.crossing_closed", version);
            if (ExtraBlocked != null && ExtraBlocked(locationId))
                return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "route_blocked", "expedition.route_blocked", version);
            float forcedGateStaminaCost = 0f;
            var gateBlock = ExtraGateBlock?.Invoke(locationId);
            if (gateBlock != null)
            {
                // GAP-48B — a weather gate can be forced only when its data
                // carries a force cost; the sortie then starts stamina-short.
                if (!forceWeatherGate || gateBlock.ForceStaminaCost <= 0f)
                    return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "route_blocked", "expedition.route_blocked", version);
                forcedGateStaminaCost = gateBlock.ForceStaminaCost;
            }
            var def = ExpeditionDefinitionRegistry.Get(locationId)
                      ?? Definitions.Find(d => d.id == locationId);
            if (def == null)
                return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "unknown_target", "expedition.unknown_target", version);

            // Water crossings travel by river craft, not garage vehicles: the
            // naval profile projection replaces the land profile and piracy
            // risk is folded into the encounter chance for this sortie.
            var navalDispatch = ResolveNavalDispatch(def, locationId);
            if (navalDispatch != null)
                def = navalDispatch.Value.def;

            ExpeditionVehicleProfile? profile = null;
            if (navalDispatch != null)
            {
                profile = navalDispatch.Value.profile;
            }
            else if (!string.IsNullOrEmpty(vehicleId))
            {
                string? prepared = PrepareVehicleForDispatch(def, vehicleId);
                if (prepared != null)
                    return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "vehicle_unready", prepared, version);
                profile = BuildProfile(vehicleId);
            }

            // Staleness was adjudicated before preparation, so compare the current
            // version against itself here — preparation's own mutation must not
            // invalidate the dispatch it is preparing for.
            long preparedVersion = StateVersion;
            var result = Engine.ExecuteStart(def, survivorId, staminaBudget, stance, vehicle: profile, expectedStateVersion: preparedVersion, currentStateVersion: preparedVersion,
                startingStamina: ExpeditionSystem.MaxStamina - forcedGateStaminaCost);
            if (result.IsSuccess)
            {
                if (forcedGateStaminaCost > 0f)
                    OnWeatherGateForced?.Invoke(survivorId, locationId, gateBlock!);
                RaiseStateChanged();
                LastEvent = navalDispatch != null
                    ? $"Sent {survivorId} to {def.displayName} by river raft (piracy waters)."
                    : $"Sent {survivorId} to {def.displayName}{(profile != null ? $" by {profile.vehicleId}" : "")}.";
            }
            else
            {
                LastEvent = $"Expedition start refused: {result.FailureCode}.";
            }
            return result;
        }

        /// <summary>
        /// Non-consuming pre-dispatch estimate for the UI: ticks, fuel need vs
        /// tank, capacity, breakdown and encounter risk, weapon readiness.
        /// Returns null for unknown locations.
        /// </summary>
        public (ExpeditionEstimate estimate, bool fuelSufficient)? EstimateExpedition(
            string locationId,
            ExpeditionStance stance,
            string vehicleId = "",
            float weaponReadiness = 1f,
            float weaponJamRisk = 0f)
        {
            var def = ExpeditionDefinitionRegistry.Get(locationId)
                      ?? Definitions.Find(d => d.id == locationId);
            if (def == null) return null;

            // Preview parity with dispatch: water crossings project the naval
            // profile and piracy-weighted encounter chance.
            var navalDispatch = ResolveNavalDispatch(def, locationId);
            if (navalDispatch != null)
                def = navalDispatch.Value.def;

            ExpeditionVehicleProfile? profile = null;
            bool fuelOk = true;
            if (navalDispatch != null)
            {
                profile = navalDispatch.Value.profile;
            }
            else if (!string.IsNullOrEmpty(vehicleId))
            {
                var inst = Vehicles.GetVehicle(vehicleId);
                if (inst != null && !inst.isBrokenDown)
                {
                    profile = BuildProfile(vehicleId);
                    fuelOk = inst.fuel >= profile!.fuelPerTravelTick * 2f * def.distanceTicks;
                }
            }
            var estimate = ExpeditionSystem.Estimate(def, stance, false, profile, weaponReadiness, weaponJamRisk);
            return (estimate, fuelOk);
        }

        /// <summary>Refuel from carried fuel units (inventory consumption is the caller's concern).</summary>
        public CommandResult RefuelVehicle(string vehicleId, float units)
        {
            var r = Vehicles.Refuel(vehicleId, units);
            var result = r.Status == ActionResult.StatusKind.Success
                ? CommandResult.FromSuccess(PlayerCommandCode.RepairVehicle, ActionResult.Success("vehicle.refueled"), StateVersion, StateVersion + 1)
                : new CommandResult(PlayerCommandCode.RepairVehicle, ActionResult.Blocked(r.FailureCode, "vehicle.refuel_failed"), StateVersion, StateVersion);
            if (result.IsSuccess) RaiseStateChanged();
            LastEvent = result.IsSuccess ? $"Refueled {vehicleId}." : $"Cannot refuel {vehicleId}: {result.FailureCode}.";
            return result;
        }

        /// <summary>Repair a garage vehicle by the given condition amount.</summary>
        public CommandResult RepairVehicle(string vehicleId, float amount)
        {
            var r = Vehicles.Repair(vehicleId, amount);
            var result = r.Status == ActionResult.StatusKind.Success
                ? CommandResult.FromSuccess(PlayerCommandCode.RepairVehicle, ActionResult.Success("vehicle.repaired", new Dictionary<string, double> { ["condition"] = amount }), StateVersion, StateVersion + 1)
                : new CommandResult(PlayerCommandCode.RepairVehicle, ActionResult.Blocked(r.FailureCode, "vehicle.repair_failed"), StateVersion, StateVersion);
            if (result.IsSuccess) RaiseStateChanged();
            LastEvent = result.IsSuccess ? $"Repaired {vehicleId}." : $"Cannot repair {vehicleId}: {result.FailureCode}.";
            return result;
        }

        /// <summary>Install an authored track-gear definition from vehicles.json.</summary>
        public CommandResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f)
        {
            var action = Vehicles.InstallTrackGear(vehicleId, gearId, condition);
            var result = action.Status == ActionResult.StatusKind.Success
                ? CommandResult.FromSuccess(
                    PlayerCommandCode.RepairVehicle,
                    action,
                    StateVersion,
                    StateVersion + 1)
                : new CommandResult(
                    PlayerCommandCode.RepairVehicle,
                    action,
                    StateVersion,
                    StateVersion);
            if (result.IsSuccess) RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? $"Installed {gearId} on {vehicleId}."
                : $"Cannot install {gearId}: {action.FailureCode}.";
            return result;
        }

        public CommandResult RemoveTrackGear(string vehicleId)
        {
            var action = Vehicles.RemoveTrackGear(vehicleId);
            var result = action.Status == ActionResult.StatusKind.Success
                ? CommandResult.FromSuccess(PlayerCommandCode.RepairVehicle, action, StateVersion, StateVersion + 1)
                : new CommandResult(PlayerCommandCode.RepairVehicle, action, StateVersion, StateVersion);
            if (result.IsSuccess) RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? $"Removed track gear from {vehicleId}."
                : $"Cannot remove track gear: {action.FailureCode}.";
            return result;
        }

        public CommandResult RepairTrackGear(string vehicleId, float amount)
        {
            var action = Vehicles.RepairTrackGear(vehicleId, amount);
            var result = action.Status == ActionResult.StatusKind.Success
                ? CommandResult.FromSuccess(PlayerCommandCode.RepairVehicle, action, StateVersion, StateVersion + 1)
                : new CommandResult(PlayerCommandCode.RepairVehicle, action, StateVersion, StateVersion);
            if (result.IsSuccess) RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? $"Repaired track gear on {vehicleId}."
                : $"Cannot repair track gear: {action.FailureCode}.";
            return result;
        }

        /// <summary>
        /// Plan 60 — vehicle kit → vehicle assembly bridge. Consumes one kit
        /// item from the shelter inventory atomically and acquires the mapped
        /// vehicle through the garage. Inventory consumption is the kit's
        /// cost; vehicle ownership/state remains owned by
        /// <see cref="ExpeditionVehicleSystem"/>.
        /// </summary>
        public CommandResult AssembleVehicleFromKit(string kitItemId, Inventory shelterInventory)
        {
            if (string.IsNullOrEmpty(kitItemId) || shelterInventory == null)
                return new CommandResult(PlayerCommandCode.AssembleVehicle,
                    ActionResult.Failed("invalid_kit", "vehicle.kit_invalid"), StateVersion, StateVersion);

            if (!_kitVehicleMap.TryGetValue(kitItemId, out var vehicleId))
                return new CommandResult(PlayerCommandCode.AssembleVehicle,
                    ActionResult.Failed("invalid_kit", "vehicle.kit_invalid"), StateVersion, StateVersion);

            if (Vehicles.GetVehicle(vehicleId) != null)
                return new CommandResult(PlayerCommandCode.AssembleVehicle,
                    ActionResult.Blocked("already_owned", "vehicle.already_owned"), StateVersion, StateVersion);

            // Atomic kit consumption BEFORE acquisition; on acquisition
            // failure the kit is refunded via a grant bill. The player is
            // never left owning a vehicle without paying its kit, nor holding
            // a consumed kit without a vehicle.
            if (!shelterInventory.TryConsumeBill(new Dictionary<string, int> { { kitItemId, 1 } }))
                return new CommandResult(PlayerCommandCode.AssembleVehicle,
                    ActionResult.Blocked("missing_kit", "vehicle.kit_missing"), StateVersion, StateVersion);

            var acquireResult = Vehicles.AcquireVehicle(vehicleId);
            if (acquireResult.Status != ActionResult.StatusKind.Success)
            {
                // Refund the kit — acquisition did not go through.
                var refund = new InventoryBill();
                refund.AddGrant(kitItemId, 1);
                shelterInventory.TryExecuteTransaction(refund);
                return new CommandResult(PlayerCommandCode.AssembleVehicle,
                    ActionResult.Blocked(acquireResult.FailureCode ?? "vehicle.acquire_failed", "vehicle.acquire_failed"), StateVersion, StateVersion);
            }

            RaiseStateChanged();
            LastEvent = $"Assembled {vehicleId} from {kitItemId}.";
            return CommandResult.FromSuccess(PlayerCommandCode.AssembleVehicle,
                ActionResult.Success("vehicle.assembled", new Dictionary<string, double>()),
                StateVersion, StateVersion + 1);
        }

        /// <summary>Plan 60 — kit item id → vehicle id mapping.</summary>
        private static readonly Dictionary<string, string> _kitVehicleMap =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["item_vehicle_kit_bicycle"] = "vehicle_bicycle",
                ["item_vehicle_kit_cargo_cart"] = "vehicle_cargo_cart",
                ["item_vehicle_kit_scout_motorcycle"] = "vehicle_scout_motorcycle",
            };

        /// <summary>
        /// Dispatch preparation through the garage: exact fuel need check, the
        /// consuming PrepareForExpedition (fuel burn, wear, prep-breakdown
        /// roll). Returns null when ready to roll, otherwise a refusal message.
        /// </summary>
        private string? PrepareVehicleForDispatch(ExpeditionDefinition def, string vehicleId)
        {
            var inst = Vehicles.GetVehicle(vehicleId);
            if (inst == null) return $"No such vehicle in the garage: {vehicleId}.";
            if (inst.isBrokenDown) return $"{vehicleId} is broken down — repair it before dispatch.";

            float distanceKm = 2f * def.distanceTicks * KmPerTravelTick;
            var vdef = Vehicles.GetDefinition(vehicleId);
            float consumption = vdef?.fuel_consumption_per_km ?? 0.5f;
            float fuelNeeded = distanceKm * consumption;
            if (inst.fuel < fuelNeeded)
                return $"{vehicleId} needs {fuelNeeded:F1} fuel for this run — tank holds {inst.fuel:F1}. Refuel first.";

            var (_, _, prepBreakdown) = Vehicles.PrepareForExpedition(vehicleId, distanceKm);
            if (prepBreakdown)
                return $"{vehicleId} threw a breakdown during preparation — the sortie is aborted and the vehicle needs repair.";
            return null;
        }

        /// <summary>
        /// Naval dispatch resolution for water-route destinations. Returns
        /// (adjustedDefinition, profile) when the location is a water crossing:
        /// the sortie travels by river craft (raft — always available) and
        /// piracy risk is folded into the encounter chance. Returns null when
        /// the destination is a land route.
        /// </summary>
        private (ExpeditionDefinition def, ExpeditionVehicleProfile profile)? ResolveNavalDispatch(
            ExpeditionDefinition def, string locationId)
        {
            if (!WaterRouteHazards.TryGetValue(locationId, out float hazard)) return null;
            var vessel = _naval.CreateInstance("vessel_improvised_raft");
            var profile = _naval.ProjectToVehicleProfile(vessel);
            var adjusted = _naval.ApplyPiracyToDefinition(def, hazard, vessel.vesselId);
            return (adjusted, profile);
        }

        private ExpeditionVehicleProfile? BuildProfile(string vehicleId)
        {
            return Vehicles.CreateExpeditionProfile(vehicleId, KmPerTravelTick);
        }

        public CommandResult StartDemoExpedition(string survivorId, string locationId)
            => StartExpedition(survivorId, locationId);

        /// <summary>
        /// UI dispatch path: gates, vehicle dispatch preparation, and the
        /// sortie start in one call, with the caller's day (the UI keeps its
        /// own day semantics). Returns the player-facing result message.
        /// </summary>
        public CommandResult DispatchSortie(
            string survivorId,
            string locationId,
            ExpeditionStance stance,
            int day,
            string vehicleId = "",
            long? stateVersion = null,
            bool forceWeatherGate = false)
        {
            long version = stateVersion ?? StateVersion;

            // Same rule as StartExpedition: reject a stale caller version before
            // preparation spends fuel, then compare current-to-current afterwards.
            if (version != StateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.ExpeditionDispatch, version, StateVersion);

            var def = ExpeditionDefinitionRegistry.Get(locationId)
                      ?? Definitions.Find(d => d.id == locationId);
            if (def == null)
                return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "unknown_target", "expedition.unknown_target", version);
            if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
                return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "crossing_closed", "expedition.crossing_closed", version);
            if (ExtraBlocked != null && ExtraBlocked(locationId))
                return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "route_blocked", "expedition.route_blocked", version);
            float forcedGateStaminaCost = 0f;
            var gateBlock2 = ExtraGateBlock?.Invoke(locationId);
            if (gateBlock2 != null)
            {
                if (!forceWeatherGate || gateBlock2.ForceStaminaCost <= 0f)
                    return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "route_blocked", "expedition.route_blocked", version);
                forcedGateStaminaCost = gateBlock2.ForceStaminaCost;
            }

            var navalDispatch = ResolveNavalDispatch(def, locationId);
            if (navalDispatch != null)
                def = navalDispatch.Value.def;

            ExpeditionVehicleProfile? profile = null;
            if (navalDispatch != null)
            {
                profile = navalDispatch.Value.profile;
            }
            else if (!string.IsNullOrEmpty(vehicleId))
            {
                string? prepared = PrepareVehicleForDispatch(def, vehicleId);
                if (prepared != null)
                    return CommandResult.ContextBlocked(PlayerCommandCode.ExpeditionDispatch, "vehicle_unready", prepared, version);
                profile = BuildProfile(vehicleId);
            }

            long dispatchVersion = StateVersion;
            var result = Engine.ExecuteStart(def, survivorId, day, stance, vehicle: profile, expectedStateVersion: dispatchVersion, currentStateVersion: dispatchVersion,
                startingStamina: ExpeditionSystem.MaxStamina - forcedGateStaminaCost);
            if (result.IsSuccess)
            {
                if (forcedGateStaminaCost > 0f)
                    OnWeatherGateForced?.Invoke(survivorId, locationId, gateBlock2!);
                RaiseStateChanged();
                LastEvent = navalDispatch != null
                    ? $"{survivorId} takes the river route to {def.displayName} by raft (piracy waters)."
                    : profile != null
                        ? $"{survivorId} rolls out for {def.displayName} in the {Vehicles.GetVehicle(vehicleId)?.displayName ?? vehicleId}."
                        : $"{survivorId} sets out on foot for {def.displayName}.";
            }
            else
            {
                LastEvent = $"Expedition start refused: {result.FailureCode}.";
            }
            return result;
        }

        /// <summary>Production API to advance active expeditions by the specified duration.</summary>
        public string TickHours(float hours)
        {
            Engine.TickHours(hours, _rng);
            return $"Tick: {Engine.ActiveCount} active expedition(s).";
        }

        /// <summary>
        /// F2/F3/F4 — typed outcome of applying one resolution's cross-system
        /// consequences. Each subsystem reports independently so a partial
        /// application is observable, never silently folded into a boolean.
        /// </summary>
        public sealed class EncounterApplicationResult
        {
            public string ResolutionId = string.Empty;

            public enum Status { NotApplicable, Applied, AlreadyKnown, RejectedCapacity, RejectedInsufficientItems, NoActiveExpedition, SkippedNoAuthority, RejectedUnknownId }

            public Status Item = Status.NotApplicable;
            public string ItemId = string.Empty;
            public int ItemQuantity;

            public Status Journal = Status.NotApplicable;
            public string JournalId = string.Empty;

            public Status Location = Status.NotApplicable;
            public string LocationId = string.Empty;

            public Status Flag = Status.NotApplicable;
            public string FlagId = string.Empty;

            /// <summary>F17 — micro-location hazard routing outcome. NotApplicable
            /// for flags without a registered hazard; Applied when the canonical
            /// disease authority received the consequence exactly once.</summary>
            public MicroLocationHazardRegistry.HazardStatus Hazard = MicroLocationHazardRegistry.HazardStatus.NotApplicable;
            public string HazardDiseaseId = string.Empty;
        }

        /// <summary>F2/F3/F4 — outcome of the most recent consequence
        /// application (observability for UI, tests, and diagnostics).</summary>
        public EncounterApplicationResult? LastApplication { get; private set; }

        /// <summary>Fired after a resolution's consequences were applied.</summary>
        public event Action<EncounterApplicationResult>? OnEncounterConsequencesApplied;

        /// <summary>
        /// Apply a player choice for a surfaced encounter through Core. The
        /// location is taken from that encounter's own pending entry when one
        /// exists, so resolving a backlog row records where that row actually
        /// happened rather than wherever the newest encounter surfaced. After
        /// the Core resolve commits, the returned consequence payload is
        /// applied through the owning subsystems exactly once (F2 items, F3
        /// journal, F4 location, world flag).
        /// </summary>
        public bool EncounterApplyChoice(string encounterId, string choiceId, int day)
            => EncounterApplyChoice(encounterId, choiceId, day, null!);

        /// <summary>
        /// Apply a player choice with an explicit locationId. Pass null to let the
        /// pending queue supply it, falling back to the last surfaced encounter.
        /// </summary>
        public bool EncounterApplyChoice(string encounterId, string choiceId, int day, string locationId)
        {
            if (_bridge == null || string.IsNullOrEmpty(encounterId)) return false;

            string effectiveLocation = locationId ?? PendingLocationFor(encounterId)!;
            bool ok = _bridge.ResolveChoice(encounterId, choiceId, day, effectiveLocation!);

            // The player has acknowledged this one — shrink the pending list.
            if (ok) _narrative.ClearPending(encounterId);

            // F2/F3/F4 — apply the resolved consequence payload exactly once,
            // through the subsystems that own each effect.
            if (ok && _bridge.LastResolution != null)
            {
                var application = ApplyEncounterConsequences(_bridge.LastResolution);
                LastApplication = application;
                OnEncounterConsequencesApplied?.Invoke(application);
            }
            return ok;
        }

        /// <summary>
        /// Plan 45 phase 2 — resolve a travel-encounter choice through the
        /// wasteland-inhabitants layer and, when the choice is hostile,
        /// raise <see cref="OnTravelEncounterCombatTriggered"/> carrying the
        /// EnemyCompositionSelector composition (wildlife pack for Creature
        /// encounters, raid crew for high-danger Human ones). The data
        /// outcomes (morale / guilt / field-guide unlock) resolve exactly as
        /// they would without combat — combat rides on top, once per
        /// resolution. Returns false when the travel engine is unavailable,
        /// the encounter/choice is unknown, or the choice is non-hostile.
        /// </summary>
        public bool ResolveTravelChoiceWithCombat(
            string encounterId, string choiceId, int day, string locationId, int dangerLevel, int enemyCount)
        {
            if (TravelEngine == null || string.IsNullOrEmpty(encounterId) || string.IsNullOrEmpty(choiceId))
                return false;

            var catalog = TravelEngine.Catalog;
            if (catalog == null || !catalog.TryGetEncounter(encounterId, out var definition) || definition == null)
                return false;
            TravelEncounterChoice? choice = null;
            for (int i = 0; i < definition.Choices.Count; i++)
            {
                if (definition.Choices[i] != null && definition.Choices[i].ChoiceId == choiceId)
                {
                    choice = definition.Choices[i];
                    break;
                }
            }
            if (choice == null) return false;

            // Data outcomes first (exactly the no-combat path).
            bool resolved = TravelEngine.ResolveChoice(encounterId, choiceId, day, out _, out _, out _);
            if (!resolved) return false;

            // Combat escalation — the single binding authority.
            if (TravelEncounterCombatBinder.TryBind(definition, choice, dangerLevel, enemyCount, out var ids, _rng))
            {
                OnTravelEncounterCombatTriggered?.Invoke(new TravelCombatTrigger
                {
                    EncounterId = encounterId,
                    Title = definition.Title,
                    LocationId = locationId ?? string.Empty,
                    DangerLevel = dangerLevel,
                    CombatantIds = ids,
                });
                return true;
            }
            return resolved;
        }

        /// <summary>The pending entry's recorded location for this encounter, or null when it is not pending.</summary>
        private string? PendingLocationFor(string encounterId)
        {
            var pending = _narrative?.State?.pending;
            if (pending == null) return null;
            for (int i = 0; i < pending.Count; i++)
            {
                if (pending[i] != null && pending[i].encounterId == encounterId)
                    return pending[i].locationId;
            }
            return null;
        }

        // ── F2/F3/F4 consequence application ───────────────────

        /// <summary>F2 — the sortie that surfaced the most recent resolvable
        /// encounter (loot-routing hint), or empty for backlog rows.</summary>
        private string _lastSurfacedTriggerSurvivor = string.Empty;
        private string _lastSurfacedEncounterId = string.Empty;

        /// <summary>
        /// F2/F3/F4 consequence application. Order (per the integration plan):
        /// item delta → journal → location → world flag. Narrative state
        /// (history/depletion) already committed in Core; every external
        /// effect is idempotent or capacity-checked, and failures surface as
        /// typed statuses instead of exceptions.
        /// </summary>
        private EncounterApplicationResult ApplyEncounterConsequences(NarrativeEncounterResolutionResult r)
        {
            var app = new EncounterApplicationResult { ResolutionId = r.ResolutionId };
            var feedback = new List<string>();

            // ── F2: signed item delta ──
            app.ItemId = r.GrantItemId;
            app.ItemQuantity = r.GrantItemQuantity;
            if (!string.IsNullOrEmpty(r.GrantItemId) && r.GrantItemQuantity != 0)
            {
                if (r.GrantItemQuantity > 0)
                {
                    // Positive grant → the active sortie's loot list (never the
                    // shelter inventory: the expedition return flow unloads it).
                    string? survivorId = ResolveGrantSurvivorId(r.EncounterId, r.LocationId);
                    float unitWeight = ItemWeight(r.GrantItemId);
                    var grant = string.IsNullOrEmpty(survivorId)
                        ? ExpeditionSystem.LootGrantStatus.NoActiveExpedition
                        : Engine.TryGrantLoot(survivorId, r.GrantItemId, unitWeight, r.GrantItemQuantity);
                    app.Item = grant switch
                    {
                        ExpeditionSystem.LootGrantStatus.Granted => EncounterApplicationResult.Status.Applied,
                        ExpeditionSystem.LootGrantStatus.RejectedCapacity => EncounterApplicationResult.Status.RejectedCapacity,
                        _ => EncounterApplicationResult.Status.NoActiveExpedition
                    };
                    feedback.Add(app.Item switch
                    {
                        EncounterApplicationResult.Status.Applied =>
                            $"Recovered: {ItemDisplayName(r.GrantItemId)} ×{r.GrantItemQuantity}.",
                        EncounterApplicationResult.Status.RejectedCapacity =>
                            $"Cargo found, but your expedition cannot carry the {ItemDisplayName(r.GrantItemId).ToLowerInvariant()}.",
                        _ => $"The {ItemDisplayName(r.GrantItemId)} had to be left behind — no pack was out to carry it."
                    });
                }
                else
                {
                    // Negative grant (offering) → shelter inventory authority.
                    int needed = -r.GrantItemQuantity;
                    if (ShelterInventory == null)
                    {
                        app.Item = EncounterApplicationResult.Status.SkippedNoAuthority;
                    }
                    else if (ShelterInventory.HasSufficient(r.GrantItemId, needed) && ShelterInventory.TryConsume(r.GrantItemId, needed))
                    {
                        app.Item = EncounterApplicationResult.Status.Applied;
                        feedback.Add($"Offering left: {ItemDisplayName(r.GrantItemId)} ×{needed}.");
                    }
                    else
                    {
                        app.Item = EncounterApplicationResult.Status.RejectedInsufficientItems;
                        feedback.Add($"An offering wanted {ItemDisplayName(r.GrantItemId)} — none could be spared.");
                    }
                }
            }

            // ── F3: journal unlock ──
            app.JournalId = r.JournalUnlockId;
            if (!string.IsNullOrEmpty(r.JournalUnlockId))
            {
                if (Journal == null)
                {
                    app.Journal = EncounterApplicationResult.Status.SkippedNoAuthority;
                }
                else
                {
                    // One atomic Core path: entry written AND codex event fired
                    // exactly once per key (single KnowledgeBase dedup gate).
                    var entry = Journal.TryDiscoverKnowledge(r.JournalUnlockId, ExpeditionJournalAuthor.Instance, r.Day);
                    app.Journal = entry != null
                        ? EncounterApplicationResult.Status.Applied
                        : EncounterApplicationResult.Status.AlreadyKnown;
                    if (entry != null)
                        feedback.Add($"Journal updated: {HumanizeId(r.JournalUnlockId)}.");
                }
            }

            // ── F4: location discovery ──
            app.LocationId = r.DiscoverLocationId;
            if (!string.IsNullOrEmpty(r.DiscoverLocationId))
            {
                bool already = Engine.IsLocationKnown(r.DiscoverLocationId);
                bool discovered = Engine.DiscoverLocation(r.DiscoverLocationId);
                if (discovered)
                {
                    app.Location = already
                        ? EncounterApplicationResult.Status.AlreadyKnown
                        : EncounterApplicationResult.Status.Applied;
                    if (!already)
                        feedback.Add($"New location discovered: {DestinationDisplayName(r.DiscoverLocationId)}.");
                }
                else
                {
                    app.Location = EncounterApplicationResult.Status.RejectedUnknownId;
                }
            }

            // ── World flag (authored micro-location consequence) ──
            app.FlagId = r.SetWorldFlagId;
            bool flagWasAlreadySet = true;
            if (!string.IsNullOrEmpty(r.SetWorldFlagId))
            {
                flagWasAlreadySet = Flags != null && Flags.IsSet(r.SetWorldFlagId);
                Flags?.Set(r.SetWorldFlagId, NarrativeEncounterSystem.SystemId, r.ResolutionId, r.Day);
                app.Flag = flagWasAlreadySet
                    ? EncounterApplicationResult.Status.AlreadyKnown
                    : EncounterApplicationResult.Status.Applied;

                // F17 — hazard consequence: a freshly-set micro-location hazard
                // flag routes into the owning disease authority exactly once
                // (never on AlreadyKnown — a persistent flag cannot re-infect on
                // revisit, save/reload, or event replay). The registry owns the
                // flag→consequence mapping; the survivor is the same scavenger
                // who received the grant (or the ordinal-first active expedition
                // at the site), resolved by the same deterministic rule as loot.
                var hazard = MicroLocationHazardRegistry.ApplyFlagHazard(
                    r.SetWorldFlagId,
                    flagWasAlreadySet,
                    ResolveGrantSurvivorId(r.EncounterId, r.LocationId),
                    r.Day,
                    ApplyDisease);
                app.Hazard = hazard.Status;
                app.HazardDiseaseId = hazard.DiseaseId;

                // The exposure is the choice's biggest consequence — surface it
                // on the same feedback strip as loot and journal lines.
                // Presentation only; the ledger and the disease authority own
                // the state. Restrained wording: show the risk, not the diagnosis.
                if (hazard.Status == MicroLocationHazardRegistry.HazardStatus.Applied)
                    feedback.Add($"Exposure: {HumanizeId(hazard.SurvivorId)} worked among the remains. Watch for fever.");
            }

            if (feedback.Count > 0)
            {
                LastEvent = string.Join(" ", feedback);
                RaiseStateChanged();
            }
            return app;
        }

        /// <summary>
        /// F2 — decide which active sortie receives a positive grant. The
        /// surfaced trigger's survivor wins when it still matches an active
        /// expedition; otherwise the ordinal-first active expedition at the
        /// resolution's location. Deterministic. Null when no sortie qualifies.
        /// </summary>
        private string? ResolveGrantSurvivorId(string encounterId, string locationId)
        {
            if (!string.IsNullOrEmpty(_lastSurfacedTriggerSurvivor)
                && _lastSurfacedEncounterId == encounterId
                && Engine.Active.ContainsKey(_lastSurfacedTriggerSurvivor))
                return _lastSurfacedTriggerSurvivor;

            string? best = null;
            foreach (var kv in Engine.Active)
            {
                var state = kv.Value;
                if (state == null || !string.Equals(state.locationId, locationId, StringComparison.Ordinal)) continue;
                if (best == null || string.CompareOrdinal(kv.Key, best) < 0)
                    best = kv.Key;
            }
            return best;
        }

        /// <summary>F2 — catalog weight, falling back to the scavenging
        /// convention (1 kg per item) for unknown items.</summary>
        private float ItemWeight(string itemId)
        {
            var def = Items?.Get(itemId);
            return def != null && def.weight > 0f ? def.weight : 1f;
        }

        private string ItemDisplayName(string itemId)
        {
            var def = Items?.Get(itemId);
            return def != null && !string.IsNullOrEmpty(def.displayName) ? def.displayName : HumanizeId(itemId);
        }

        private string DestinationDisplayName(string locationId)
        {
            var def = Definitions.Find(d => d != null && d.id == locationId);
            return def != null && !string.IsNullOrEmpty(def.displayName) ? def.displayName : HumanizeId(locationId);
        }

        private static string HumanizeId(string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;
            var parts = id.Split('_');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length == 0) continue;
                parts[i] = char.ToUpperInvariant(parts[i][0]) + (parts[i].Length > 1 ? parts[i][1..] : string.Empty);
            }
            return string.Join(" ", parts);
        }

        private sealed class ExpeditionJournalAuthor : ISurvivorAuthor
        {
            public static readonly ExpeditionJournalAuthor Instance = new ExpeditionJournalAuthor();
            public string Id => "expedition";
            public string DisplayName => "Expedition";
            public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
        }
        public string PushLuck(string survivorId)
        {
            return Engine.PushLuck(survivorId) ? $"{survivorId} is pushing luck." : "Cannot push luck (not looting).";
        }

        public string PushLuckDemo(string survivorId) => PushLuck(survivorId);

        public string Retreat(string survivorId)
        {
            return Engine.Retreat(survivorId) ? $"{survivorId} is retreating." : "Cannot retreat (not looting).";
        }

        public string RetreatDemo(string survivorId) => Retreat(survivorId);

        // ── Camp actions ──────────────────────────────────────────────

        /// <summary>Enter camp phase for an outbound expedition.</summary>
        public string EnterCamp(
            string survivorId,
            float temperatureC = -10f,
            string weatherCondition = "Clear",
            float firewood = 8f,
            float water = 4f,
            float food = 4f,
            bool hasTent = true,
            bool hasBedroll = true,
            string shelterType = "tent",
            bool hasSentry = true)
        {
            bool ok = Engine.EnterCamp(
                survivorId, CurrentDay, 18f,
                temperatureC, weatherCondition,
                firewood, water, food,
                hasTent, hasBedroll, shelterType, hasSentry);
            return ok
                ? $"{survivorId} established camp. Night begins."
                : "Cannot enter camp (not outbound or unknown expedition).";
        }

        public string EnterCampDemo(
            string survivorId,
            float temperatureC = -10f,
            string weatherCondition = "Clear",
            float firewood = 8f,
            float water = 4f,
            float food = 4f,
            bool hasTent = true,
            bool hasBedroll = true,
            string shelterType = "tent",
            bool hasSentry = true)
            => EnterCamp(survivorId, temperatureC, weatherCondition, firewood, water, food, hasTent, hasBedroll, shelterType, hasSentry);

        /// <summary>Advance one night segment. Returns dawn message when complete.</summary>
        public string CampTick(string survivorId)
        {
            bool dawn = Engine.CampTick(survivorId, _rng);
            var camp = Engine.GetCampState(survivorId);
            if (camp == null) return "No active camp.";
            if (dawn)
                return $"Dawn. Night complete. Segments: {camp.nightSegmentsCompleted}/{camp.totalNightSegments}.";
            return $"Night segment {camp.nightSegmentsCompleted}/{camp.totalNightSegments}. " +
                   $"Firewood: {camp.firewoodRemaining:F1}. Temp: {camp.temperatureC + camp.heatOutput:F1}C.";
        }

        public string CampTickDemo(string survivorId) => CampTick(survivorId);

        /// <summary>Resolve a camp encounter.</summary>
        public string ResolveCampEncounter(string survivorId, string outcome)
        {
            bool ok = Engine.ResolveCampEncounter(survivorId, outcome, outcome == "injury" ? 15f : 0f);
            return ok ? $"Camp encounter resolved: {outcome}." : "No unresolved encounter.";
        }

        public string ResolveCampEncounterDemo(string survivorId, string outcome)
            => ResolveCampEncounter(survivorId, outcome);

        /// <summary>Break camp at dawn.</summary>
        public string BreakCamp(string survivorId, bool retreat = false)
        {
            bool ok = Engine.BreakCamp(survivorId, retreat);
            return ok
                ? $"Camp broken. {(retreat ? "Retreating to shelter." : "Resuming travel.")}"
                : "Cannot break camp (night not over or no camp).";
        }

        public string BreakCampDemo(string survivorId, bool retreat = false) => BreakCamp(survivorId, retreat);

        /// <summary>Get camp status for UI display.</summary>
        public CampState? GetCampState(string survivorId) => Engine.GetCampState(survivorId);

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"Expeditions active: {Engine.ActiveCount}\n");
            var ids = new List<string>(Engine.Active.Keys);
            ids.Sort(string.CompareOrdinal);
            for (int i = 0; i < ids.Count; i++)
            {
                var e = Engine.Active[ids[i]];
                sb.Append($"  {e.survivorId} -> {e.displayName} [{((ExpeditionPhase)e.phase)}] " +
                          $"travel {e.travelTicksCompleted}/{e.distanceTicks} loot {e.loot.Count} " +
                          $"stamina {e.stamina:F0}%");
                if (e.isPushingLuck) sb.Append(" [PUSHING LUCK]");
                sb.Append('\n');
            }
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ──────────────────────────────────────────────

        public List<ExpeditionState> CaptureSave() => Engine.CaptureState();
        public void RestoreSave(List<ExpeditionState> state) => Engine.RestoreState(state);

        /// <summary>Aggregate save payload: active expeditions + the vehicle garage + the F4 destination-discovery ledger.</summary>
        public ExpeditionAggregateState CaptureSaveAggregate()
        {
            var aggregate = new ExpeditionAggregateState
            {
                expeditions = Engine.CaptureState(),
                vehicles = Vehicles.CaptureState(),
                knownLocationIds = Engine.CaptureKnownLocations(),
                completedCount = Engine.CompletedCount
            };
            if (aggregate.knownLocationIds.Count == 0)
                aggregate.knownLocationIds = new List<string>(); // explicit empty — authoritative, not legacy
            return aggregate;
        }

        public void RestoreSaveAggregate(ExpeditionAggregateState aggregate)
        {
            if (aggregate == null) return;
            if (aggregate.expeditions != null)
                Engine.RestoreState(aggregate.expeditions);
            if (aggregate.vehicles != null)
                Vehicles.RestoreState(aggregate.vehicles);
            Engine.RestoreCompletedCount(aggregate.completedCount);

            // F4 restore: a present list (even empty) is authoritative. A null
            // list marks a legacy aggregate that predates the ledger —
            // reconstruct discoveries from the narrative resolution history so
            // pre-feature clue choices keep their revealed destinations.
            if (aggregate.knownLocationIds != null)
                Engine.RestoreKnownLocations(aggregate.knownLocationIds);
            else
                Engine.RestoreKnownLocations(ReconstructDiscoveriesFromHistory());
        }

        /// <summary>
        /// F4 legacy migration: walk the narrative resolution history, resolve
        /// each recorded choice against the catalog, and collect the location
        /// IDs its choices discovered. Deterministic; unknown historical
        /// encounters/choices are skipped — never guessed.
        /// </summary>
        private List<string> ReconstructDiscoveriesFromHistory()
        {
            var discovered = new List<string>();
            var history = _narrative?.State?.history;
            var narrativeEngine = _narrative;
            if (history == null || narrativeEngine == null) return discovered;
            for (int i = 0; i < history.Count; i++)
            {
                var record = history[i];
                if (record == null || string.IsNullOrEmpty(record.encounterId)) continue;
                var def = narrativeEngine.Find(record.encounterId);
                if (def == null) continue;
                var choice = def.choices?.Find(c => c != null && c.choiceId == record.choiceId);
                if (choice == null || string.IsNullOrEmpty(choice.discoverLocationId)) continue;
                if (!discovered.Contains(choice.discoverLocationId))
                    discovered.Add(choice.discoverLocationId);
            }
            discovered.Sort(string.CompareOrdinal);
            return discovered;
        }

        // ── Dive Instance (Exp 09) ──────────────────────────────────

        public string StartDive(string siteId = "site_exp09_ss_sovereign")
        {
            var site = new DiveSiteDefinition(siteId, 120, 0.5, "q_keeper_of_logs");
            DiveRunner = new DiveInstanceRunner(new Ashfall.Core.Events.SimpleEventBus(),
                Flags ?? new Ashfall.Core.Flags.CampaignConsequenceLedger(), new SeededRng(DemoSeed), site);
            return $"Dive started at {siteId}. Oxygen: {DiveRunner.OxygenRemaining} ticks.";
        }

        public string StartDiveDemo(string siteId = "site_exp09_ss_sovereign") => StartDive(siteId);

        public string AdvanceDive()
        {
            if (DiveRunner == null) return "No active dive.";
            bool ok = DiveRunner.Advance();
            return ok ? $"Advanced to {DiveRunner.CurrentRoom}. O2: {DiveRunner.OxygenRemaining}." : "Cannot advance (at end or no oxygen).";
        }

        public string AdvanceDiveDemo() => AdvanceDive();

        public string TickDiveOxygen()
        {
            if (DiveRunner == null) return "No active dive.";
            DiveRunner.TickOxygen();
            return $"O2: {DiveRunner.OxygenRemaining}. Room: {DiveRunner.CurrentRoom}.";
        }

        public string TickDiveOxygenDemo() => TickDiveOxygen();

        public string CommitDiveChoice(string choice)
        {
            if (DiveRunner == null) return "No active dive.";
            if (choice == "flood") DiveRunner.CommitChoice(SovereignChoice.flood_the_market);
            else if (choice == "burn") DiveRunner.CommitChoice(SovereignChoice.burn_the_hold);
            else return $"Unknown choice: {choice}";
            return $"Choice committed: {DiveRunner.Choice}.";
        }

        public string CommitDiveChoiceDemo(string choice) => CommitDiveChoice(choice);

        public string DiveStatusLine()
        {
            if (DiveRunner == null) return "Dive: idle";
            return $"Dive: {DiveRunner.CurrentRoom} · O2 {DiveRunner.OxygenRemaining} · " +
                   $"choice {DiveRunner.Choice} · risk {DiveRunner.DetectionRisk(0.5, false):F2}";
        }
    }
}
