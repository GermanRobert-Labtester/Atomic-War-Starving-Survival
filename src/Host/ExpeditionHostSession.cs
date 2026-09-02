using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.PlayerCommand;

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

        /// <summary>Vehicle garage (fuel, condition, repair) — persisted inside the expedition aggregate.</summary>
        public ExpeditionVehicleSystem Vehicles { get; }

        /// <summary>Optional crossing gate — when set, crossing-node expeditions require vouch access.</summary>
        public VouchAccessSystem CrossingGate { get; set; }

        /// <summary>
        /// Optional extra dispatch gate (ice road seasonal + deep-coast route
        /// stage). When set, any location it blocks cannot be dispatched.
        /// </summary>
        public Func<string, bool> ExtraBlocked { get; set; }

        /// <summary>Passthrough to the Core per-location encounter-chance multiplier (faction/territory danger).</summary>
        public void SetEncounterChanceMultiplier(Func<string, float> multiplier) => Engine.SetEncounterChanceMultiplier(multiplier);

        /// <summary>Current sim day, supplied by Main so EncounterApplyChoice can pass day to Core.</summary>
        public int CurrentDay { get; set; }

        public string LastEvent { get; private set; } = string.Empty;
        /// <summary>Fired when Core rolls an encounter and the bridge surfaces a DTO. Host UI subscribes here.</summary>
        public event Action<ExpeditionEncounterBridge.EncounterSurfaced>? OnEncounterSurfaced;

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

        public ExpeditionHostSession(ExpeditionSystem engine = null!, NarrativeEncounterSystem narrative = null!)
        {
            Engine = engine ?? new ExpeditionSystem();
            _rng = new SeededRng(DemoSeed);
            _narrative = narrative ?? new NarrativeEncounterSystem();
            _bridge = new ExpeditionEncounterBridge(_narrative, _rng);
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
                    _narrative.EnqueuePending(dto.encounter_id, dto.trigger.locationId, dto.trigger.encounterCount, CurrentDay);
                RaiseStateChanged();
                OnEncounterSurfaced?.Invoke(dto);
            };
            Engine.OnEncounterTriggered += s => _bridge.Surface(s);
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
                lootCategories = new List<string> { "scrap_metal", "clean_water", "bandages", "food_rations" }
            };
            var cut = new ExpeditionDefinition
            {
                id = "loc_denial_cut_substation",
                displayName = "The Denial Cut Substation",
                distanceTicks = 8,
                dangerLevel = 4,
                encounterChancePerTick = 0.18f,
                baseStaminaDrainPerHour = 3.0f,
                lootCategories = new List<string> { "dosimeter", "copper_wire", "fuel", "item_hydro_baron_queue_chit" }
            };
            ExpeditionDefinitionRegistry.Register(allotments);
            ExpeditionDefinitionRegistry.Register(cut);
            Definitions.Add(allotments);
            Definitions.Add(cut);
        }

        public static ExpeditionHostSession Create(string dataDir, NarrativeEncounterSystem narrative = null!)
        {
            var session = new ExpeditionHostSession(null!, narrative);
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
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
            }

            var save = ExpeditionSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save.expeditions);
                if (save.vehicles != null)
                    session.Vehicles.RestoreState(save.vehicles);
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
        public bool IsLocationBlocked(string locationId)
        {
            if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
                return true;
            if (ExtraBlocked != null && ExtraBlocked(locationId))
                return true;
            return false;
        }

        /// <summary>Production API to start an expedition to a specified location.
        /// When a vehicleId is given, dispatch preparation runs the garage's
        /// fuel burn, wear, and prep-breakdown roll before the sortie starts.</summary>
        public CommandResult StartExpedition(
            string survivorId,
            string locationId,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            int staminaBudget = 40,
            string vehicleId = "",
            long? stateVersion = null)
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
            var result = Engine.ExecuteStart(def, survivorId, staminaBudget, stance, vehicle: profile, expectedStateVersion: preparedVersion, currentStateVersion: preparedVersion);
            if (result.IsSuccess)
            {
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
            var inst = Vehicles.GetVehicle(vehicleId);
            if (inst == null) return null;
            var vdef = Vehicles.GetDefinition(vehicleId);
            float consumption = vdef?.fuel_consumption_per_km ?? 0.5f;
            return new ExpeditionVehicleProfile
            {
                vehicleId = vehicleId,
                speedMultiplier = inst.speedMultiplier,
                cargoCapacityKg = inst.cargoCapacity,
                fuelPerTravelTick = consumption * KmPerTravelTick,
                // Worn vehicles risk a mid-route breakdown each travel tick;
                // pristine metal carries a ~0 chance.
                breakdownChancePerTick = (100f - inst.condition) / 100f * 0.15f,
            };
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
            long? stateVersion = null)
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
            var result = Engine.ExecuteStart(def, survivorId, day, stance, vehicle: profile, expectedStateVersion: dispatchVersion, currentStateVersion: dispatchVersion);
            if (result.IsSuccess)
            {
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
        /// Apply a player choice for a surfaced encounter through Core. The
        /// location is taken from that encounter's own pending entry when one
        /// exists, so resolving a backlog row records where that row actually
        /// happened rather than wherever the newest encounter surfaced.
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
            return ok;
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

        /// <summary>Aggregate save payload: active expeditions + the vehicle garage.</summary>
        public ExpeditionAggregateState CaptureSaveAggregate() => new ExpeditionAggregateState
        {
            expeditions = Engine.CaptureState(),
            vehicles = Vehicles.CaptureState(),
        };

        public void RestoreSaveAggregate(ExpeditionAggregateState aggregate)
        {
            if (aggregate == null) return;
            if (aggregate.expeditions != null)
                Engine.RestoreState(aggregate.expeditions);
            if (aggregate.vehicles != null)
                Vehicles.RestoreState(aggregate.vehicles);
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
