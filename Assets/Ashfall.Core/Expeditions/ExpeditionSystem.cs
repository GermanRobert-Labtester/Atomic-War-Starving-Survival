using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.PlayerCommand;

namespace Ashfall.Core.Expeditions
{
    public enum ExpeditionStance
    {
        Stealth, // lower encounter chance, standard travel speed
        Speed    // 1.5x travel speed, higher encounter chance
    }

    public enum ExpeditionPhase
    {
        Outbound,  // traveling to the target
        Looting,   // at the site, push-your-luck scavenging
        Inbound,   // returning to shelter
        Completed, // returned with loot unloaded
        Failed,    // collapsed or killed
        Camp       // overnight camp during travel
    }

    [Serializable]
    public class ExpeditionLootEntry
    {
        public string itemId = string.Empty;
        public int quantity = 0;
        public float weightKg = 0f;
    }

    /// <summary>Camp shelter assignment for overnight survival.</summary>
    [Serializable]
    public class CampShelterAssignment
    {
        public string survivorId = string.Empty;
        public bool hasTent = false;
        public bool hasBedroll = false;
        public string shelterType = "none"; // none, lean_to, tent, cave
    }

    /// <summary>Camp watch/sentry shift assignment.</summary>
    [Serializable]
    public class CampWatchShift
    {
        public string survivorId = string.Empty;
        public int shiftIndex = 0;       // 0 = first half, 1 = second half
        public float alertness = 1.0f;   // 0..1, degrades with fatigue
        public bool isActive = false;
    }

    /// <summary>Serialized state of an overnight camp (part of expedition save).</summary>
    [Serializable]
    public class CampState
    {
        public int campStartDay = 0;
        public float campStartHour = 0f;
        public int nightSegmentsCompleted = 0;
        public int totalNightSegments = 4;  // 4 segments = one night
        public float firewoodRemaining = 0f;
        public float firewoodConsumed = 0f;
        public float heatOutput = 0f;       // degrees C added
        public float waterReserved = 0f;
        public float waterConsumed = 0f;
        public float foodReserved = 0f;
        public float foodConsumed = 0f;
        public float temperatureC = 0f;    // ambient at camp
        public string weatherCondition = "Clear";
        public float coldExposure = 0f;    // accumulated cold damage
        public float radiationExposure = 0f;
        public int wildlifeThreatLevel = 0;
        public bool encounterTriggered = false;
        public string encounterKey = string.Empty;
        public bool encounterResolved = false;
        public string campOutcome = string.Empty; // resume, retreat, injury, loss, failed
        public List<CampShelterAssignment> shelterAssignments = new List<CampShelterAssignment>();
        public List<CampWatchShift> watchShifts = new List<CampWatchShift>();
    }

    /// <summary>
    /// Vehicle facts for one dispatch, projected by the host from
    /// ExpeditionVehicleSystem's garage. Passed at Start; the expedition core
    /// stays decoupled from the garage (fuel tank, repairs, ownership).
    /// </summary>
    [Serializable]
    public sealed class ExpeditionVehicleProfile
    {
        public string vehicleId = string.Empty;
        /// <summary>Travel-step multiplier while the vehicle runs (foot = 1.0).</summary>
        public float speedMultiplier = 1f;
        /// <summary>Loot capacity in kg while the vehicle runs (0 = keep foot cap).</summary>
        public float cargoCapacityKg = 0f;
        /// <summary>Per-travel-tick breakdown chance while the vehicle runs.</summary>
        public float breakdownChancePerTick = 0f;
        /// <summary>Fuel units consumed per travel tick (for estimates).</summary>
        public float fuelPerTravelTick = 0f;
    }

    /// <summary>Pre-dispatch numbers for the expedition UI (pure, no RNG).</summary>
    [Serializable]
    public sealed class ExpeditionEstimate
    {
        public string locationId = string.Empty;
        public string stance = string.Empty;
        public int distanceTicks;
        public float outboundTicks;
        public float inboundTicks;
        public float lootingTicks;
        public float totalTicks;
        public float cargoCapacityKg;
        public float fuelRequired;
        public float breakdownRiskPerTick;
        public float breakdownRiskTotal;
        public float encounterRiskPerTick;
        public float weaponReadiness = 1f;
        public float weaponJamRisk;
        public bool usingVehicle;
    }

    /// <summary>Serialized state of one expedition (save/load safe).</summary>
    [Serializable]
    public class ExpeditionState
    {
        public string systemId = ExpeditionSystem.SystemId;
        public string expeditionId = string.Empty;
        public string survivorId = string.Empty;
        public string locationId = string.Empty;
        public string displayName = string.Empty;
        public string stance = "Stealth";
        public int phase = (int)ExpeditionPhase.Outbound;
        public int startedDay = 0;
        public int distanceTicks = 0;
        public int travelTicksCompleted = 0;
        public int lootingTicksCompleted = 0;
        public float stamina = 100f;
        public float maxLootCapacityKg = 40f;
        public float currentWeightKg = 0f;
        public int dangerLevel = 1;
        public float encounterChancePerTick = 0.12f;
        public int encounterCount = 0;
        public bool isPushingLuck = false;
        public bool isNightScavenge = false;
        public bool hasBicycle = false;
        public bool hasFlashlight = false;
        public string vehicleId = string.Empty;
        public float vehicleSpeedMultiplier = 1f;
        public float vehicleBreakdownChancePerTick = 0f;
        public bool vehicleBrokenDown = false;
        public string outcomeText = string.Empty;
        public List<ExpeditionLootEntry> loot = new List<ExpeditionLootEntry>();
        public CampState campState = new CampState();
    }

    /// <summary>Data-driven target definition for an expedition.</summary>
    [Serializable]
    public class ExpeditionDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public int distanceTicks = 8;
        public int dangerLevel = 1;
        public float encounterChancePerTick = 0.12f;
        public float baseStaminaDrainPerHour = 2.0f;
        public List<string> lootCategories = new List<string>();
        public string scavenging_table_id = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic expedition core (port of the Unity ExpeditionSystem's
    /// travel/looting/inbound mechanics). Tick-based: Outbound travel, arrival,
    /// push-your-luck looting with capacity, auto-retreat, Inbound return,
    /// completion or collapse failure. All rolls go through ISeededRng passed
    /// per tick — the host owns seeding, so the core never stores RNG state.
    /// Zero engine namespaces; events + save/load per the house pattern.
    /// </summary>
    public class ExpeditionSystem
    {
        public const string SystemId = "expedition_system";
        public const float MaxStamina = 100f;
        public const int AutoRetreatAfterLootTicks = 3;
        public const float EncumberPenaltyPerTickMax = 15f;

        // Camp constants
        public const int CampNightSegments = 4;
        public const float CampFirewoodPerSegment = 2.0f;
        public const float CampHeatPerFirewood = 3.0f;     // degrees C per unit
        public const float CampWaterPerSegment = 0.5f;
        public const float CampFoodPerSegment = 0.5f;
        public const float CampColdDamageThresholdC = -5f;  // below this, cold damage
        public const float CampColdDamagePerSegment = 5f;   // HP per segment below threshold
        public const float CampStaminaRecoveryPerSegment = 8f;
        public const float CampEncounterChanceBase = 0.15f;
        public const float CampSentryDetectionBonus = 0.3f; // reduces encounter chance

        private readonly Dictionary<string, ExpeditionState> _active = new Dictionary<string, ExpeditionState>();

        /// <summary>
        /// Optional per-survivor stamina-drain multiplier hook (0..N). Set by the
        /// host so Phase-0 effects (respiratory severe cough, phantom work refusal,
        /// guilt insomnia fatigue) reach the real expedition stamina consumer.
        /// Returns 1.0 when unset or unknown. The multiplier is applied to the
        /// base per-hour drain in ApplyStaminaDrain.
        /// </summary>
        private Func<string, float> _staminaDrainMultiplier;

        public ScavengingTableCatalog? ScavengingCatalog { get; set; }

        public event Action<ExpeditionState> OnExpeditionStarted;
        public event Action<ExpeditionState> OnExpeditionTick;
        public event Action<ExpeditionState> OnPhaseChanged;
        public event Action<ExpeditionState> OnLootAdded;                 // state, itemId, qty
        public event Action<ExpeditionState> OnEncounterTriggered;
        public event Action<ExpeditionState> OnVehicleBreakdown;
        public event Action<ExpeditionState> OnExpeditionCompleted;
        public event Action<ExpeditionState, string> OnExpeditionFailed;
        public event Action<ExpeditionState> OnStateChanged;

        // Camp events
        public event Action<ExpeditionState> OnCampEntered;
        public event Action<ExpeditionState> OnCampSuppliesReserved;
        public event Action<ExpeditionState> OnCampNightSegmentResolved;
        public event Action<ExpeditionState> OnCampEncounterSurfaced;
        public event Action<ExpeditionState> OnCampEncounterResolved;
        public event Action<ExpeditionState> OnCampDawnResolved;

        public ExpeditionSystem()
        {
        }

        /// <summary>
        /// Bind the per-survivor stamina-drain multiplier. Hosts wire Phase-0
        /// respiratory/guilt/phantom effects here so they alter real expedition
        /// stamina consumption rather than living in a display value.
        /// </summary>
        public void SetStaminaDrainMultiplier(Func<string, float> multiplier)
        {
            _staminaDrainMultiplier = multiplier;
        }

        /// <summary>
        /// Optional per-location encounter-chance multiplier (1.0 = authored
        /// rate). Hosts wire faction/territory danger here (e.g. the warlord's
        /// TravelDangerModifier for controlled/contested road) so hostile ground
        /// raises the chance of meeting trouble on a real sortie. The roll is
        /// still seeded and deterministic for a given multiplier.
        /// </summary>
        private Func<string, float> _encounterChanceMultiplier;

        /// <summary>
        /// Bind the per-location encounter-chance multiplier. Returns 1.0 when
        /// unset or unknown. Multipliers clamp the resulting chance to [0,1].
        /// </summary>
        public void SetEncounterChanceMultiplier(Func<string, float> multiplier)
        {
            _encounterChanceMultiplier = multiplier;
        }

        public IReadOnlyDictionary<string, ExpeditionState> Active => _active;
        public int ActiveCount => _active.Count;

        // ── Lifecycle ──────────────────────────────────────────────────

        public bool Start(
            ExpeditionDefinition def,
            string survivorId,
            int day,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            bool isNightScavenge = false,
            bool hasBicycle = false,
            bool hasFlashlight = false,
            ExpeditionVehicleProfile? vehicle = null,
            float startingStamina = MaxStamina)
        {
            if (def == null || string.IsNullOrEmpty(def.id) || string.IsNullOrEmpty(survivorId))
                return false;
            if (_active.ContainsKey(survivorId)) return false; // one expedition per survivor

            var exp = new ExpeditionState
            {
                // Unique per survivor+target (Unity keys expeditions by id).
                expeditionId = survivorId + ":" + def.id,
                survivorId = survivorId,
                locationId = def.id,
                displayName = def.displayName,
                stance = stance.ToString(),
                startedDay = day,
                distanceTicks = Math.Max(1, def.distanceTicks),
                dangerLevel = def.dangerLevel,
                encounterChancePerTick = def.encounterChancePerTick,
                stamina = Math.Clamp(startingStamina, 0f, MaxStamina),
                isNightScavenge = isNightScavenge,
                hasBicycle = hasBicycle,
                hasFlashlight = hasFlashlight
            };
            if (vehicle != null && !string.IsNullOrEmpty(vehicle.vehicleId))
            {
                exp.vehicleId = vehicle.vehicleId;
                exp.vehicleSpeedMultiplier = vehicle.speedMultiplier > 0f ? vehicle.speedMultiplier : 1f;
                exp.vehicleBreakdownChancePerTick = Math.Clamp(vehicle.breakdownChancePerTick, 0f, 1f);
                if (vehicle.cargoCapacityKg > 0f)
                    exp.maxLootCapacityKg = vehicle.cargoCapacityKg;
            }
            _active[survivorId] = exp;
            OnExpeditionStarted?.Invoke(exp);
            OnStateChanged?.Invoke(exp);
            return true;
        }

        /// <summary>
        /// Side-effect-free preview of an expedition dispatch.
        /// Shares the same validation path as <see cref="Start"/>.
        /// </summary>
        public CommandPreview PreviewStart(
            ExpeditionDefinition def,
            string survivorId,
            int day,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            bool isNightScavenge = false,
            bool hasBicycle = false,
            bool hasFlashlight = false,
            ExpeditionVehicleProfile? vehicle = null,
            long stateVersion = 0)
        {
            if (def == null || string.IsNullOrEmpty(def.id) || string.IsNullOrEmpty(survivorId))
                return CommandPreview.Unavailable(PlayerCommandCode.ExpeditionDispatch, "invalid_params", "expedition.invalid_params", stateVersion);
            if (_active.ContainsKey(survivorId))
                return CommandPreview.Unavailable(PlayerCommandCode.ExpeditionDispatch, "already_active", "expedition.already_active", stateVersion);

            var projected = new Dictionary<string, double>();
            var estimate = Estimate(def, stance, isNightScavenge, vehicle);
            projected["travel_ticks"] = estimate.totalTicks;
            projected["stamina_cost"] = estimate.totalTicks * (def != null ? def.baseStaminaDrainPerHour : 2.0);
            if (vehicle != null && !string.IsNullOrEmpty(vehicle.vehicleId))
                projected["fuel_cost"] = estimate.fuelRequired;

            return CommandPreview.Available(
                PlayerCommandCode.ExpeditionDispatch,
                stateVersion,
                projected,
                estimate.totalTicks,
                riskCodes: new[] { "encounter_risk", "stamina_drain" },
                isIrreversible: true,
                messageKey: "expedition.preview_available");
        }

        /// <summary>
        /// Execute an expedition dispatch using the same validation path as <see cref="PreviewStart"/>.
        /// Stale previews are rejected without mutation.
        /// </summary>
        public CommandResult ExecuteStart(
            ExpeditionDefinition def,
            string survivorId,
            int day,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            bool isNightScavenge = false,
            bool hasBicycle = false,
            bool hasFlashlight = false,
            ExpeditionVehicleProfile? vehicle = null,
            long expectedStateVersion = 0,
            long currentStateVersion = 0,
            float startingStamina = MaxStamina)
        {
            var preview = PreviewStart(def, survivorId, day, stance, isNightScavenge, hasBicycle, hasFlashlight, vehicle, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.ExpeditionDispatch, preview.StateVersion, currentStateVersion);

            bool ok = Start(def, survivorId, day, stance, isNightScavenge, hasBicycle, hasFlashlight, vehicle, startingStamina);
            if (!ok)
                return new CommandResult(
                    PlayerCommandCode.ExpeditionDispatch,
                    ActionResult.Failed("execute_failed", "expedition.execute_failed"),
                    expectedStateVersion, currentStateVersion);

            var deltas = new Dictionary<string, double>();
            foreach (var kv in preview.ProjectedDeltas)
                deltas[kv.Key] = kv.Value;

            return CommandResult.FromSuccess(
                PlayerCommandCode.ExpeditionDispatch,
                ActionResult.Success("expedition.dispatched", deltas),
                expectedStateVersion,
                currentStateVersion + 1);
        }

        /// <summary>
        /// Side-effect-free preview of a push-luck command during looting.
        /// </summary>
        public CommandPreview PreviewPushLuck(string survivorId, long stateVersion = 0)
        {
            if (!_active.TryGetValue(survivorId, out var exp))
                return CommandPreview.Unavailable(PlayerCommandCode.ExpeditionPushLuck, "not_active", "expedition.not_active", stateVersion);
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Looting)
                return CommandPreview.Unavailable(PlayerCommandCode.ExpeditionPushLuck, "wrong_phase", "expedition.wrong_phase", stateVersion);

            return CommandPreview.Available(
                PlayerCommandCode.ExpeditionPushLuck,
                stateVersion,
                isIrreversible: false,
                riskCodes: new[] { "injury", "detection" },
                messageKey: "expedition.push_luck_preview");
        }

        /// <summary>Execute push-luck using the same validation path as preview.</summary>
        public CommandResult ExecutePushLuck(string survivorId, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewPushLuck(survivorId, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);
            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.ExpeditionPushLuck, preview.StateVersion, currentStateVersion);

            bool ok = PushLuck(survivorId);
            if (!ok)
                return new CommandResult(
                    PlayerCommandCode.ExpeditionPushLuck,
                    ActionResult.Failed("execute_failed", "expedition.execute_failed"),
                    expectedStateVersion, currentStateVersion);

            return CommandResult.FromSuccess(
                PlayerCommandCode.ExpeditionPushLuck,
                ActionResult.Success("expedition.push_luck"),
                expectedStateVersion,
                currentStateVersion + 1);
        }

        /// <summary>
        /// Side-effect-free preview of a retreat command during looting.
        /// </summary>
        public CommandPreview PreviewRetreat(string survivorId, long stateVersion = 0)
        {
            if (!_active.TryGetValue(survivorId, out var exp))
                return CommandPreview.Unavailable(PlayerCommandCode.ExpeditionRetreat, "not_active", "expedition.not_active", stateVersion);
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Looting)
                return CommandPreview.Unavailable(PlayerCommandCode.ExpeditionRetreat, "wrong_phase", "expedition.wrong_phase", stateVersion);

            return CommandPreview.Available(
                PlayerCommandCode.ExpeditionRetreat,
                stateVersion,
                isIrreversible: false,
                messageKey: "expedition.retreat_preview");
        }

        /// <summary>Execute retreat using the same validation path as preview.</summary>
        public CommandResult ExecuteRetreat(string survivorId, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewRetreat(survivorId, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);
            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.ExpeditionRetreat, preview.StateVersion, currentStateVersion);

            bool ok = Retreat(survivorId);
            if (!ok)
                return new CommandResult(
                    PlayerCommandCode.ExpeditionRetreat,
                    ActionResult.Failed("execute_failed", "expedition.execute_failed"),
                    expectedStateVersion, currentStateVersion);

            return CommandResult.FromSuccess(
                PlayerCommandCode.ExpeditionRetreat,
                ActionResult.Success("expedition.retreated"),
                expectedStateVersion,
                currentStateVersion + 1);
        }

        /// <summary>
        /// Pure pre-dispatch math for the UI: travel ticks by stance/vehicle,
        /// capacity, fuel need, breakdown and encounter risk. Mirrors the
        /// exact step/risk formulas the tick loop applies — no RNG.
        /// </summary>
        public static ExpeditionEstimate Estimate(
            ExpeditionDefinition def,
            ExpeditionStance stance,
            bool isNightScavenge = false,
            ExpeditionVehicleProfile? vehicle = null,
            float weaponReadiness = 1f,
            float weaponJamRisk = 0f)
        {
            var est = new ExpeditionEstimate
            {
                locationId = def?.id ?? string.Empty,
                stance = stance.ToString(),
                distanceTicks = def != null ? Math.Max(1, def.distanceTicks) : 0,
                cargoCapacityKg = 40f,
                weaponReadiness = Math.Clamp(weaponReadiness, 0f, 1f),
                weaponJamRisk = Math.Clamp(weaponJamRisk, 0f, 1f),
            };

            float speed = stance == ExpeditionStance.Speed ? 1.5f : 1.0f;
            float breakdown = 0f;
            float fuelPerTick = 0f;
            if (vehicle != null && !string.IsNullOrEmpty(vehicle.vehicleId))
            {
                est.usingVehicle = true;
                float vSpeed = vehicle.speedMultiplier > 0f ? vehicle.speedMultiplier : 1f;
                speed *= vSpeed;
                breakdown = Math.Clamp(vehicle.breakdownChancePerTick, 0f, 1f);
                fuelPerTick = vehicle.fuelPerTravelTick;
                if (vehicle.cargoCapacityKg > 0f)
                    est.cargoCapacityKg = vehicle.cargoCapacityKg;
            }

            est.breakdownRiskPerTick = breakdown;
            est.outboundTicks = (float)Math.Ceiling(est.distanceTicks / speed);
            // Inbound keeps the bicycle bonus estimate at foot pace for
            // simplicity: 0.5 extra only when no vehicle is projected.
            float inboundSpeed = speed;
            if (!est.usingVehicle && stance != ExpeditionStance.Speed)
                inboundSpeed += 0.5f; // bicycle-friendly foot estimate bookkeeping
            est.inboundTicks = (float)Math.Ceiling(est.distanceTicks / Math.Max(0.5f, inboundSpeed));
            est.lootingTicks = AutoRetreatAfterLootTicks;
            est.totalTicks = est.outboundTicks + est.lootingTicks + est.inboundTicks;
            est.fuelRequired = fuelPerTick * (est.outboundTicks + est.inboundTicks);
            float travelTicksForRisk = est.outboundTicks + est.inboundTicks;
            est.breakdownRiskTotal = travelTicksForRisk > 0
                ? 1f - (float)Math.Pow(1f - breakdown, travelTicksForRisk)
                : 0f;

            float encounter = def != null ? def.encounterChancePerTick : 0.12f;
            if (stance == ExpeditionStance.Stealth) encounter *= 0.5f;
            // A degraded weapon cannot deter trouble as well: poor readiness
            // raises the effective encounter risk by up to half again.
            encounter *= 1f + (1f - est.weaponReadiness) * 0.5f;
            est.encounterRiskPerTick = Math.Clamp(encounter, 0f, 1f);
            return est;
        }

        private readonly List<string> _tickKeyBuffer = new();

        /// <summary>Advance the sector clock by tick hours for every active expedition.</summary>
        public void TickHours(float hours, ISeededRng rng)
        {
            if (hours <= 0f) return;
            _tickKeyBuffer.Clear();
            foreach (var k in _active.Keys)
                _tickKeyBuffer.Add(k);
            _tickKeyBuffer.Sort(string.CompareOrdinal); // deterministic iteration
            for (int i = 0; i < _tickKeyBuffer.Count; i++)
            {
                var exp = _active[_tickKeyBuffer[i]];
                if (exp.phase == (int)ExpeditionPhase.Completed || exp.phase == (int)ExpeditionPhase.Failed)
                    continue;

                ApplyStaminaDrain(exp, hours);
                if (exp.stamina <= 0f)
                {
                    Fail(exp, "Collapsed from exhaustion.");
                    continue;
                }

                RollEncounter(exp, rng); // every leg can meet trouble (Unity parity)

                // Mid-route vehicle breakdown: one seeded roll per travel tick
                // while the vehicle still runs. A breakdown drops the sortie
                // to foot speed and foot capacity for the remainder.
                if (!string.IsNullOrEmpty(exp.vehicleId) && !exp.vehicleBrokenDown &&
                    exp.vehicleBreakdownChancePerTick > 0f &&
                    ((ExpeditionPhase)exp.phase == ExpeditionPhase.Outbound ||
                     (ExpeditionPhase)exp.phase == ExpeditionPhase.Inbound) &&
                    rng.NextDouble() < exp.vehicleBreakdownChancePerTick)
                {
                    exp.vehicleBrokenDown = true;
                    exp.maxLootCapacityKg = Math.Min(exp.maxLootCapacityKg, 40f);
                    exp.outcomeText = $"The {exp.vehicleId} gave out — continuing on foot.";
                    OnVehicleBreakdown?.Invoke(exp);
                }

                switch ((ExpeditionPhase)exp.phase)
                {
                    case ExpeditionPhase.Outbound:
                        AdvanceOutbound(exp);
                        break;
                    case ExpeditionPhase.Looting:
                        AdvanceLooting(exp, rng);
                        break;
                    case ExpeditionPhase.Inbound:
                        AdvanceInbound(exp);
                        break;
                    case ExpeditionPhase.Camp:
                        // Camp has its own tick method (CampTick); skip here.
                        break;
                }

                if (exp.phase == (int)ExpeditionPhase.Completed)
                {
                    OnExpeditionCompleted?.Invoke(exp);
                    _active.Remove(exp.survivorId);
                    OnStateChanged?.Invoke(exp);
                    continue;
                }

                OnExpeditionTick?.Invoke(exp);
                OnStateChanged?.Invoke(exp);
            }
        }

        public bool PushLuck(string survivorId)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return false;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Looting) return false;
            exp.isPushingLuck = true;
            OnStateChanged?.Invoke(exp);
            return true;
        }

        public bool Retreat(string survivorId)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return false;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Looting) return false;
            SetPhase(exp, ExpeditionPhase.Inbound);
            OnStateChanged?.Invoke(exp);
            return true;
        }

        // ── Camp lifecycle ─────────────────────────────────────────────

        /// <summary>
        /// Enter camp phase. Called when outbound travel reaches a configured
        /// dusk boundary. Supplies must be reserved before night ticks begin.
        /// </summary>
        public bool EnterCamp(
            string survivorId,
            int day,
            float hour,
            float temperatureC,
            string weatherCondition,
            float firewood,
            float water,
            float food,
            bool hasTent,
            bool hasBedroll,
            string shelterType,
            bool hasSentry)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return false;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Outbound) return false;

            var camp = exp.campState;
            camp.campStartDay = day;
            camp.campStartHour = hour;
            camp.nightSegmentsCompleted = 0;
            camp.totalNightSegments = CampNightSegments;
            camp.firewoodRemaining = Math.Max(0f, firewood);
            camp.firewoodConsumed = 0f;
            camp.waterReserved = Math.Max(0f, water);
            camp.waterConsumed = 0f;
            camp.foodReserved = Math.Max(0f, food);
            camp.foodConsumed = 0f;
            camp.temperatureC = temperatureC;
            camp.weatherCondition = weatherCondition ?? "Clear";
            camp.coldExposure = 0f;
            camp.radiationExposure = 0f;
            camp.encounterTriggered = false;
            camp.encounterKey = string.Empty;
            camp.encounterResolved = false;
            camp.campOutcome = string.Empty;

            camp.shelterAssignments.Clear();
            camp.shelterAssignments.Add(new CampShelterAssignment
            {
                survivorId = survivorId,
                hasTent = hasTent,
                hasBedroll = hasBedroll,
                shelterType = shelterType ?? "none"
            });

            camp.watchShifts.Clear();
            if (hasSentry)
            {
                camp.watchShifts.Add(new CampWatchShift
                {
                    survivorId = survivorId,
                    shiftIndex = 0,
                    alertness = 1.0f,
                    isActive = true
                });
                camp.watchShifts.Add(new CampWatchShift
                {
                    survivorId = survivorId,
                    shiftIndex = 1,
                    alertness = 0.8f, // second shift slightly more fatigued
                    isActive = true
                });
            }

            SetPhase(exp, ExpeditionPhase.Camp);
            OnCampEntered?.Invoke(exp);
            OnStateChanged?.Invoke(exp);
            return true;
        }

        /// <summary>
        /// Reserve supplies for the night. Must be called after EnterCamp
        /// and before CampTick. Can be called multiple times to adjust.
        /// </summary>
        public bool ReserveCampSupplies(
            string survivorId,
            float firewood,
            float water,
            float food)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return false;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Camp) return false;

            var camp = exp.campState;
            camp.firewoodRemaining = Math.Max(0f, firewood);
            camp.waterReserved = Math.Max(0f, water);
            camp.foodReserved = Math.Max(0f, food);
            OnCampSuppliesReserved?.Invoke(exp);
            OnStateChanged?.Invoke(exp);
            return true;
        }

        /// <summary>
        /// Advance one night segment. Consumes supplies, applies cold/fatigue,
        /// rolls encounters. Returns true if the night is complete (dawn).
        /// </summary>
        public bool CampTick(string survivorId, ISeededRng rng)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return false;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Camp) return false;

            var camp = exp.campState;
            if (camp.nightSegmentsCompleted >= camp.totalNightSegments)
                return true; // already dawn

            // Consume firewood
            float firewoodNeeded = CampFirewoodPerSegment;
            if (camp.firewoodRemaining >= firewoodNeeded)
            {
                camp.firewoodRemaining -= firewoodNeeded;
                camp.firewoodConsumed += firewoodNeeded;
                camp.heatOutput = CampHeatPerFirewood * firewoodNeeded;
            }
            else
            {
                camp.heatOutput = CampHeatPerFirewood * camp.firewoodRemaining;
                camp.firewoodConsumed += camp.firewoodRemaining;
                camp.firewoodRemaining = 0f;
            }

            // Consume water and food
            float waterNeeded = CampWaterPerSegment;
            if (camp.waterReserved >= waterNeeded)
            {
                camp.waterReserved -= waterNeeded;
                camp.waterConsumed += waterNeeded;
            }
            else
            {
                camp.waterConsumed += camp.waterReserved;
                camp.waterReserved = 0f;
            }

            float foodNeeded = CampFoodPerSegment;
            if (camp.foodReserved >= foodNeeded)
            {
                camp.foodReserved -= foodNeeded;
                camp.foodConsumed += foodNeeded;
            }
            else
            {
                camp.foodConsumed += camp.foodReserved;
                camp.foodReserved = 0f;
            }

            // Calculate effective temperature
            float effectiveTemp = camp.temperatureC + camp.heatOutput;
            // Shelter bonus
            foreach (var shelter in camp.shelterAssignments)
            {
                if (shelter.shelterType == "tent") effectiveTemp += 5f;
                else if (shelter.shelterType == "cave") effectiveTemp += 8f;
                else if (shelter.shelterType == "lean_to") effectiveTemp += 2f;
                if (shelter.hasBedroll) effectiveTemp += 3f;
            }

            // Cold exposure
            if (effectiveTemp < CampColdDamageThresholdC)
            {
                float damage = CampColdDamagePerSegment * (1f - (effectiveTemp - CampColdDamageThresholdC) / 10f);
                camp.coldExposure += Math.Max(0f, damage);
            }

            // Stamina recovery (partial, reduced by cold exposure)
            float recovery = CampStaminaRecoveryPerSegment;
            if (camp.coldExposure > 0f) recovery *= 0.5f;
            exp.stamina = Math.Clamp(exp.stamina + recovery, 0f, MaxStamina);

            // Encounter roll
            if (!camp.encounterTriggered && rng != null)
            {
                float encounterChance = CampEncounterChanceBase;
                // Sentry reduces encounter chance
                foreach (var shift in camp.watchShifts)
                {
                    if (shift.isActive && shift.shiftIndex == (camp.nightSegmentsCompleted < 2 ? 0 : 1))
                    {
                        encounterChance -= CampSentryDetectionBonus * shift.alertness;
                        break;
                    }
                }
                encounterChance = Math.Clamp(encounterChance, 0.05f, 0.5f);
                if (rng.NextDouble() < encounterChance)
                {
                    camp.encounterTriggered = true;
                    camp.encounterKey = "camp_night_" + camp.campStartDay + "_" + survivorId;
                    camp.wildlifeThreatLevel = rng.Next(1, 4);
                    OnCampEncounterSurfaced?.Invoke(exp);
                }
            }

            camp.nightSegmentsCompleted++;
            OnCampNightSegmentResolved?.Invoke(exp);
            OnStateChanged?.Invoke(exp);

            // Check if night is complete
            if (camp.nightSegmentsCompleted >= camp.totalNightSegments)
            {
                return true; // dawn
            }
            return false;
        }

        /// <summary>
        /// Resolve a camp encounter. Must be called after an encounter is surfaced.
        /// outcome: "resolved" (fought off), "injury" (took damage), "loss" (lost supplies)
        /// </summary>
        public bool ResolveCampEncounter(string survivorId, string outcome, float staminaCost = 0f)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return false;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Camp) return false;

            var camp = exp.campState;
            if (!camp.encounterTriggered || camp.encounterResolved) return false;

            camp.encounterResolved = true;
            if (staminaCost > 0f)
                exp.stamina = Math.Clamp(exp.stamina - staminaCost, 0f, MaxStamina);

            if (outcome == "injury")
            {
                camp.coldExposure += 10f; // injury adds stress
            }
            else if (outcome == "loss")
            {
                // Lose some supplies
                float lossFraction = 0.25f;
                camp.firewoodRemaining *= (1f - lossFraction);
                camp.waterReserved *= (1f - lossFraction);
                camp.foodReserved *= (1f - lossFraction);
            }

            OnCampEncounterResolved?.Invoke(exp);
            OnStateChanged?.Invoke(exp);
            return true;
        }

        /// <summary>
        /// Break camp at dawn. Resumes outbound travel or retreats based on
        /// camp outcome. Must be called when CampTick returns true (dawn).
        /// </summary>
        public bool BreakCamp(string survivorId, bool retreat = false)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return false;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Camp) return false;

            var camp = exp.campState;
            if (camp.nightSegmentsCompleted < camp.totalNightSegments)
                return false; // night not over yet

            // Determine outcome
            if (exp.stamina <= 0f)
            {
                camp.campOutcome = "failed";
                Fail(exp, "Collapsed during overnight camp.");
                return true;
            }

            if (retreat)
            {
                camp.campOutcome = "retreat";
                SetPhase(exp, ExpeditionPhase.Inbound);
            }
            else
            {
                camp.campOutcome = "resume";
                // Resume outbound from where we left off
                SetPhase(exp, ExpeditionPhase.Outbound);
            }

            OnCampDawnResolved?.Invoke(exp);
            OnStateChanged?.Invoke(exp);
            return true;
        }

        /// <summary>Query camp state for UI display.</summary>
        public CampState? GetCampState(string survivorId)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return null;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Camp) return null;
            return exp.campState;
        }

        // ── Phase mechanics (ported 1:1 from the Unity host) ──────────

        private void AdvanceOutbound(ExpeditionState exp)
        {
            float step = exp.stance == nameof(ExpeditionStance.Speed) ? 1.5f : 1.0f;
            step *= VehicleTravelMultiplier(exp);
            exp.travelTicksCompleted += (int)Math.Round(step, MidpointRounding.AwayFromZero);
            if (exp.travelTicksCompleted >= exp.distanceTicks)
                SetPhase(exp, ExpeditionPhase.Looting);
        }

        private bool AdvanceLooting(ExpeditionState exp, ISeededRng rng)
        {
            exp.lootingTicksCompleted++;
            PerformLootRoll(exp, rng);
            MaybeAutoRetreat(exp);
            return false;
        }

        private void PerformLootRoll(ExpeditionState exp, ISeededRng rng)
        {
            if (rng == null) return;
            float chance = 0.5f + exp.dangerLevel * 0.05f;
            if (exp.isNightScavenge) chance += 0.1f;   // riskier, richer
            if (rng.NextDouble() >= chance) return;

            var def = ExpeditionDefinitionRegistry.Get(exp.locationId);
            string tableId = def?.scavenging_table_id ?? string.Empty;

            if (ScavengingCatalog != null && !string.IsNullOrEmpty(tableId))
            {
                var rollResult = ScavengingCatalog.RollLoot(tableId, rng);
                if (rollResult != null && !string.IsNullOrEmpty(rollResult.ItemId))
                {
                    const float itemWeight = 1.0f;
                    float totalWeightToAdd = itemWeight * rollResult.Quantity;
                    if (exp.currentWeightKg + totalWeightToAdd > exp.maxLootCapacityKg)
                    {
                        exp.outcomeText = "Capacity full; the find stays behind.";
                        return;
                    }

                    AddLoot(exp, rollResult.ItemId, itemWeight, rollResult.Quantity);
                    return;
                }
            }

            // Pick a category (or a generic item when the table is empty).
            string itemId = exp.loot.Count > 0
                ? PickLootCategory(exp, rng)
                : "scrap_metal";
            if (string.IsNullOrEmpty(itemId)) itemId = "scrap_metal";

            const float fallbackWeight = 1.0f;
            if (exp.currentWeightKg + fallbackWeight > exp.maxLootCapacityKg)
            {
                exp.outcomeText = "Capacity full; the find stays behind.";
                return;
            }

            AddLoot(exp, itemId, fallbackWeight, 1);
        }

        private static string PickLootCategory(ExpeditionState exp, ISeededRng rng)
        {
            var def = ExpeditionDefinitionRegistry.Get(exp.locationId);
            if (def != null && def.lootCategories != null && def.lootCategories.Count > 0)
            {
                int idx = rng.Next(0, def.lootCategories.Count);
                return def.lootCategories[idx];
            }
            // Fall back to categories already found, else a generic item.
            int existing = rng.Next(0, exp.loot.Count);
            return exp.loot[existing].itemId;
        }

        private void AddLoot(ExpeditionState exp, string itemId, float weightKg, int quantity = 1)
        {
            if (quantity <= 0) quantity = 1;
            for (int i = 0; i < exp.loot.Count; i++)
            {
                if (exp.loot[i].itemId == itemId)
                {
                    exp.loot[i].quantity += quantity;
                    exp.currentWeightKg += weightKg * quantity;
                    OnLootAdded?.Invoke(exp);
                    return;
                }
            }
            exp.loot.Add(new ExpeditionLootEntry { itemId = itemId, quantity = quantity, weightKg = weightKg * quantity });
            exp.currentWeightKg += weightKg * quantity;
            OnLootAdded?.Invoke(exp);
        }

        private void RollEncounter(ExpeditionState exp, ISeededRng rng)
        {
            if (rng == null) return;
            float chance = exp.encounterChancePerTick;
            if (_encounterChanceMultiplier != null && !string.IsNullOrEmpty(exp.locationId))
            {
                float mult = _encounterChanceMultiplier(exp.locationId);
                if (mult >= 0f) chance *= mult; // 0 ⇒ no encounters on this ground
            }
            chance = Math.Clamp(chance, 0f, 1f);
            if (exp.stance == nameof(ExpeditionStance.Stealth)) chance *= 0.5f;
            if (rng.NextDouble() < chance)
            {
                exp.encounterCount++;
                OnEncounterTriggered?.Invoke(exp);
            }
        }

        private void MaybeAutoRetreat(ExpeditionState exp)
        {
            if (exp.isPushingLuck) return;
            if (exp.lootingTicksCompleted >= AutoRetreatAfterLootTicks)
                SetPhase(exp, ExpeditionPhase.Inbound);
        }

        private void AdvanceInbound(ExpeditionState exp)
        {
            float step = exp.stance == nameof(ExpeditionStance.Speed) ? 1.5f : 1.0f;
            if (exp.hasBicycle) step += 0.5f; // faster return on a bicycle
            step *= VehicleTravelMultiplier(exp);
            exp.travelTicksCompleted -= (int)Math.Round(step, MidpointRounding.AwayFromZero);
            if (exp.travelTicksCompleted <= 0)
            {
                exp.travelTicksCompleted = 0;
                SetPhase(exp, ExpeditionPhase.Completed);
            }
        }

        /// <summary>
        /// Travel multiplier of the dispatched vehicle. A broken-down vehicle
        /// multiplies by nothing — the rest of the sortie is on foot.
        /// </summary>
        private static float VehicleTravelMultiplier(ExpeditionState exp)
        {
            if (string.IsNullOrEmpty(exp.vehicleId) || exp.vehicleBrokenDown)
                return 1f;
            return exp.vehicleSpeedMultiplier > 0f ? exp.vehicleSpeedMultiplier : 1f;
        }

        private void ApplyStaminaDrain(ExpeditionState exp, float hours)
        {
            var def = ExpeditionDefinitionRegistry.Get(exp.locationId);
            float baseDrain = def != null ? def.baseStaminaDrainPerHour : 2.0f;
            float drain = baseDrain * hours;
            float loadRatio = exp.maxLootCapacityKg > 0f
                ? Math.Clamp(exp.currentWeightKg / exp.maxLootCapacityKg, 0f, 1f)
                : 0f;
            drain += loadRatio * EncumberPenaltyPerTickMax * hours;

            // Phase-0 effect hook: respiratory severe cough, guilt insomnia
            // fatigue, phantom work refusal etc. increase the drain for this
            // survivor (multiplier defaults to 1.0 when unset/unknown).
            if (_staminaDrainMultiplier != null && !string.IsNullOrEmpty(exp.survivorId))
            {
                float mult = _staminaDrainMultiplier(exp.survivorId);
                if (mult > 0f) drain *= mult;
            }

            exp.stamina = Math.Clamp(exp.stamina - drain, 0f, MaxStamina);
        }

        private void Fail(ExpeditionState exp, string reason)
        {
            SetPhase(exp, ExpeditionPhase.Failed);
            exp.outcomeText = reason;
            OnExpeditionFailed?.Invoke(exp, reason);
            _active.Remove(exp.survivorId);
            OnStateChanged?.Invoke(exp);
        }

        private void SetPhase(ExpeditionState exp, ExpeditionPhase phase)
        {
            if (exp.phase == (int)phase) return;
            exp.phase = (int)phase;
            OnPhaseChanged?.Invoke(exp);
        }

        // ── Save / Load ────────────────────────────────────────────────

        /// <summary>Snapshot of the single active-envelope shape: one state per active expedition, ordinal-ordered.</summary>
        public List<ExpeditionState> CaptureState()
        {
            var copy = new List<ExpeditionState>();
            var ids = new List<string>(_active.Keys);
            ids.Sort(string.CompareOrdinal);
            for (int i = 0; i < ids.Count; i++)
                copy.Add(CloneExpedition(_active[ids[i]]));
            return copy;
        }

        public void RestoreState(List<ExpeditionState> saved)
        {
            _active.Clear();
            if (saved == null) return;
            for (int i = 0; i < saved.Count; i++)
            {
                var s = saved[i];
                if (s == null || string.IsNullOrEmpty(s.survivorId) || string.IsNullOrEmpty(s.expeditionId))
                    continue;
                var exp = CloneExpedition(s);
                _active[exp.survivorId] = exp;
            }
            OnStateChanged?.Invoke(null!);
        }

        private static ExpeditionState CloneExpedition(ExpeditionState src)
        {
            var copy = new ExpeditionState
            {
                systemId = src.systemId,
                expeditionId = src.expeditionId,
                survivorId = src.survivorId,
                locationId = src.locationId,
                displayName = src.displayName,
                stance = src.stance,
                phase = Math.Clamp(src.phase, (int)ExpeditionPhase.Outbound, (int)ExpeditionPhase.Camp),
                startedDay = src.startedDay,
                distanceTicks = src.distanceTicks,
                travelTicksCompleted = src.travelTicksCompleted,
                lootingTicksCompleted = src.lootingTicksCompleted,
                stamina = Math.Clamp(src.stamina, 0f, MaxStamina),
                maxLootCapacityKg = src.maxLootCapacityKg,
                currentWeightKg = Math.Max(0f, src.currentWeightKg),
                dangerLevel = src.dangerLevel,
                encounterChancePerTick = src.encounterChancePerTick,
                encounterCount = src.encounterCount,
                isPushingLuck = src.isPushingLuck,
                isNightScavenge = src.isNightScavenge,
                hasBicycle = src.hasBicycle,
                hasFlashlight = src.hasFlashlight,
                vehicleId = src.vehicleId,
                vehicleSpeedMultiplier = src.vehicleSpeedMultiplier,
                vehicleBreakdownChancePerTick = src.vehicleBreakdownChancePerTick,
                vehicleBrokenDown = src.vehicleBrokenDown,
                outcomeText = src.outcomeText
            };
            if (src.loot != null)
            {
                var ordered = new List<ExpeditionLootEntry>(src.loot);
                ordered.Sort((a, b) => string.CompareOrdinal(a.itemId, b.itemId));
                for (int i = 0; i < ordered.Count; i++)
                    copy.loot.Add(new ExpeditionLootEntry
                    {
                        itemId = ordered[i].itemId,
                        quantity = Math.Max(0, ordered[i].quantity),
                        weightKg = ordered[i].weightKg
                    });
            }
            // Clone camp state
            if (src.campState != null)
            {
                copy.campState = new CampState
                {
                    campStartDay = src.campState.campStartDay,
                    campStartHour = src.campState.campStartHour,
                    nightSegmentsCompleted = src.campState.nightSegmentsCompleted,
                    totalNightSegments = src.campState.totalNightSegments,
                    firewoodRemaining = src.campState.firewoodRemaining,
                    firewoodConsumed = src.campState.firewoodConsumed,
                    heatOutput = src.campState.heatOutput,
                    waterReserved = src.campState.waterReserved,
                    waterConsumed = src.campState.waterConsumed,
                    foodReserved = src.campState.foodReserved,
                    foodConsumed = src.campState.foodConsumed,
                    temperatureC = src.campState.temperatureC,
                    weatherCondition = src.campState.weatherCondition,
                    coldExposure = src.campState.coldExposure,
                    radiationExposure = src.campState.radiationExposure,
                    wildlifeThreatLevel = src.campState.wildlifeThreatLevel,
                    encounterTriggered = src.campState.encounterTriggered,
                    encounterKey = src.campState.encounterKey,
                    encounterResolved = src.campState.encounterResolved,
                    campOutcome = src.campState.campOutcome
                };
                if (src.campState.shelterAssignments != null)
                {
                    foreach (var sa in src.campState.shelterAssignments)
                        copy.campState.shelterAssignments.Add(new CampShelterAssignment
                        {
                            survivorId = sa.survivorId,
                            hasTent = sa.hasTent,
                            hasBedroll = sa.hasBedroll,
                            shelterType = sa.shelterType
                        });
                }
                if (src.campState.watchShifts != null)
                {
                    foreach (var ws in src.campState.watchShifts)
                        copy.campState.watchShifts.Add(new CampWatchShift
                        {
                            survivorId = ws.survivorId,
                            shiftIndex = ws.shiftIndex,
                            alertness = ws.alertness,
                            isActive = ws.isActive
                        });
                }
            }
            return copy;
        }
    }

    /// <summary>
    /// Registry of expedition definitions so the core stays data-free while
    /// still resolving loot tables by location id (hosts register the JSON
    /// catalogs they load; tests register inline tables).
    /// </summary>
    public static class ExpeditionDefinitionRegistry
    {
        private static readonly Dictionary<string, ExpeditionDefinition> s_defs =
            new Dictionary<string, ExpeditionDefinition>();

        public static void Register(ExpeditionDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            s_defs[def.id] = def;
        }

        public static ExpeditionDefinition? Get(string id)
        {
            return !string.IsNullOrEmpty(id) && s_defs.TryGetValue(id, out var def) ? def : null;
        }

        public static void Clear() => s_defs.Clear();
    }
}
