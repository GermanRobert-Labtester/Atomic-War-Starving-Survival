using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class ShelterThermalState
    {
        public string systemId = ShelterThermalSystem.SystemId;
        public List<ThermalRoomNode> rooms = new List<ThermalRoomNode>();
        public List<PipeSegment> pipes = new List<PipeSegment>();
        public float boilerFuelLevel = 100f;
        public float boilerTargetTempC = 70f;
        public float boilerCurrentTempC = 20f;
        public bool boilerActive;
        public float totalHeatOutputKw;
        public int lastIncidentDay = -1;
        public List<ThermalIncident> incidentLog = new List<ThermalIncident>();
    }

    [Serializable]
    public sealed class ThermalRoomNode
    {
        public string roomId = string.Empty;
        public string displayName = string.Empty;
        public float targetTempC = 18f;
        public float currentTempC = 10f;
        public float volumeM3 = 50f;
        public float insulationFactor = 1f;         // 0-2, higher = better insulated
        public bool hasRadiator;
        public float radiatorValveOpen;             // 0-1
        public bool isPriorityRoom;
        public bool isFrozen;
        public float freezeDamage;                  // accumulated structural damage from freezing
        public List<string> adjacentRoomIds = new List<string>();
    }

    [Serializable]
    public sealed class PipeSegment
    {
        public string pipeId = string.Empty;
        public string fromRoomId = string.Empty;
        public string toRoomId = string.Empty;
        public float condition = 100f;
        public bool hasBurst;
        public int burstDay = -1;
        public float burstSeverity;                 // 0-1
    }

    [Serializable]
    public sealed class ThermalIncident
    {
        public int day;
        public string pipeId = string.Empty;
        public string roomId = string.Empty;
        public ThermalIncidentKind kind;
        public string description = string.Empty;
    }

    public enum ThermalIncidentKind { PipeBurst, FreezeDamage, BoilerOverheat, ValveFailure }

    public sealed class ShelterThermalSystem
    {
        public const string SystemId = "shelter_thermal";

        // BUG-04 physics constants. First-pass placeholders (HeatGainBaseRate,
        // HeatLossBaseRate, InsulationDivisionEpsilon, the magic-literal *0.1f
        // in TickDay) replaced by an air-thermodynamics derivation. The
        // per-room solve is the analytic form of dT/dt = (G - k·(T - T_out))/C
        // (stable at any timestep; explicit Euler against 86400 s was the
        // Batch 4 overshoot bug). Tuning: NewtonCoolingCoefficient in
        // kW/(m³·K) sets the relaxation time constant τ = ρ·cp / h —
        // independent of room volume.
        public const float PriorityRoomShare = 1.5f;
        public const float AirDensityKgPerM3 = 1.225f;             // ISA sea-level
        public const float AirSpecificHeatJPerKgK = 1005f;         // ISA dry air
        public const float SecondsPerDay = 86400f;
        public const float NewtonCoolingCoefficient = 0.001f;       // h in kW/(m³·K); τ ≈ 20 min at ISA air
        public const float MinRoomVolumeM3 = 1f;                    // floor for mass math on degenerate inputs

        // BUG-04 host-tuning constant: 0.05 kW/fuel gives 100 fuel × 0.05 =
        // 5 kW sustained. In a 100 m³ room at insulation 1 the steady state
        // is ~50 °C above ambient (k = 100 W/K), clamped by boiler target
        // + 10 — enough to thaw a stock bunker in a day without the
        // old instant-saturation. Tunable without touching the math.
        public const float KwPerFuelUnit = 0.05f;

        private ShelterThermalState _state = new ShelterThermalState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly NeedsSystem _needs;
        private readonly StartingLevelSystem _startingLevel;
        private readonly YearOfAshDeepFreezeSystem _deepFreeze;
        private ShelterAssignmentSystem? _assignments;
        private int _currentDay;

        public ShelterThermalState State => _state;
        public event Action<ThermalIncident> OnIncident;
        public event Action OnThermalChanged;
        public event Action<string, string> OnFrostbiteRisk; // roomId, survivorId — cold <5°C with occupant

        public void SetAssignments(ShelterAssignmentSystem? assignments)
        {
            _assignments = assignments;
        }

        public ShelterThermalSystem(
            ISeededRng rng,
            NeedsSystem needs,
            StartingLevelSystem startingLevel,
            YearOfAshDeepFreezeSystem deepFreeze,
ILog? log = null,
ShelterAssignmentSystem? assignment = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _needs = needs ?? throw new ArgumentNullException(nameof(needs));
            _startingLevel = startingLevel ?? throw new ArgumentNullException(nameof(startingLevel));
            _deepFreeze = deepFreeze ?? throw new ArgumentNullException(nameof(deepFreeze));
            _assignments = assignment;
            _log = log ?? NullLog.Instance;
        }

        private readonly Dictionary<string, float> _auxiliaryHeatKw = new Dictionary<string, float>(StringComparer.Ordinal);

        /// <summary>
        /// Inject auxiliary waste heat (e.g. from active Silent Foundry heats, industrial smelting).
        /// Applied to the specified room during the daily thermal update and cleared after the day tick.
        /// </summary>
        public void AddAuxiliaryHeat(string roomId, float heatKw)
        {
            if (string.IsNullOrEmpty(roomId) || heatKw <= 0f) return;
            if (_auxiliaryHeatKw.TryGetValue(roomId, out float cur))
                _auxiliaryHeatKw[roomId] = cur + heatKw;
            else
                _auxiliaryHeatKw[roomId] = heatKw;
        }

        public float GetAuxiliaryHeat(string roomId) =>
            !string.IsNullOrEmpty(roomId) && _auxiliaryHeatKw.TryGetValue(roomId, out float val) ? val : 0f;

        public void ClearAuxiliaryHeat() => _auxiliaryHeatKw.Clear();

        public ActionResult AddRoom(string roomId, string displayName, float volumeM3, float insulationFactor = 1f, bool hasRadiator = true)
        {
            if (_state.rooms.Exists(r => r.roomId == roomId))
                return ActionResult.Blocked("room_exists", "thermal.room_exists");

            _state.rooms.Add(new ThermalRoomNode
            {
                roomId = roomId, displayName = displayName,
                volumeM3 = volumeM3, insulationFactor = Math.Clamp(insulationFactor, 0.1f, 2f),
                hasRadiator = hasRadiator,
                // Bug-12: a fresh room starts at the indoor baseline (ambient
                // shelter temperature) rather than inheriting a stale or
                // field-default boilerCurrentTempC value. The boiler contributes
                // heat gain via TickDay; this floor only avoids seeding a false
                // 20°C default when the bunker has never been warmed.
                currentTempC = _deepFreeze.IndoorTempCelsius
            });
            OnThermalChanged?.Invoke();
            return ActionResult.Success("thermal.room_added");
        }

        public ActionResult AddPipe(string pipeId, string fromRoomId, string toRoomId)
        {
            if (_state.pipes.Exists(p => p.pipeId == pipeId))
                return ActionResult.Blocked("pipe_exists", "thermal.pipe_exists");

            _state.pipes.Add(new PipeSegment
            {
                pipeId = pipeId, fromRoomId = fromRoomId, toRoomId = toRoomId
            });
            OnThermalChanged?.Invoke();
            return ActionResult.Success("thermal.pipe_added");
        }

        public ActionResult SetBoilerActive(bool active, float targetTempC = 70f)
        {
            _state.boilerActive = active;
            _state.boilerTargetTempC = Math.Clamp(targetTempC, 30f, 120f);
            if (active && _state.boilerFuelLevel <= 0)
                _state.boilerFuelLevel = 10f; // initial fuel when activated
            OnThermalChanged?.Invoke();
            return ActionResult.Success("thermal.boiler_toggled",
                new Dictionary<string, double> { { "active", active ? 1 : 0 } });
        }

        public ActionResult SetRadiatorValve(string roomId, float valveOpen)
        {
            var room = _state.rooms.Find(r => r.roomId == roomId);
            if (room == null) return ActionResult.Failed("unknown_room", "thermal.unknown_room");
            if (!room.hasRadiator) return ActionResult.Blocked("no_radiator", "thermal.no_radiator");

            room.radiatorValveOpen = Math.Clamp(valveOpen, 0f, 1f);
            OnThermalChanged?.Invoke();
            return ActionResult.Success("thermal.valve_set",
                new Dictionary<string, double> { { "valve", room.radiatorValveOpen } });
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            // Deep freeze input
            float deepFreezeFactor = _deepFreeze.IsIntakeBlocked ? 0.3f : 1f;

            // Boiler heat production
            if (_state.boilerActive)
            {
                _state.boilerCurrentTempC = Math.Max(_state.boilerCurrentTempC, _state.boilerTargetTempC);
                _state.boilerFuelLevel = Math.Max(0, _state.boilerFuelLevel - 0.5f);
                if (_state.boilerFuelLevel <= 0)
                {
                    _state.boilerActive = false;
                    _log.Warn("[Thermal] boiler out of fuel");
                }
            }
            else
            {
                _state.boilerCurrentTempC = Math.Max(10f, _state.boilerCurrentTempC - 2f);
            }

            // Distribute heat to rooms
            float totalHeatKw = _state.boilerActive ? _state.boilerFuelLevel * KwPerFuelUnit : 0f;
            _state.totalHeatOutputKw = totalHeatKw;

            foreach (var room in _state.rooms)
            {
                float volumeM3 = Math.Max(MinRoomVolumeM3, room.volumeM3);
                float heatCapacityJ = volumeM3 * AirDensityKgPerM3 * AirSpecificHeatJPerKgK;

                float outdoorTemp = _deepFreeze.IndoorTempCelsius;

                float heatGainKw = GetAuxiliaryHeat(room.roomId);
                if (_state.boilerActive && room.hasRadiator && room.radiatorValveOpen > 0 && !room.isFrozen)
                {
                    // Per-room allocation: valve × priority-share — independent of roomCount.
                    // Boiler kW total is split by valve and priority share; each room's
                    // ΔT is its own stable steady-state estimate.
                    float roomShare = room.isPriorityRoom ? PriorityRoomShare : 1f;
                    heatGainKw += totalHeatKw * room.radiatorValveOpen * roomShare;
                }

                // Analytic per-day solve of dT/dt = (G - k·(T - T_out)) / C:
                //   T(t) = T_out + G/k + (T_0 - T_out - G/k) · exp(-k·t/C)
                // This is stable at any timestep; the Batch 4 explicit-Euler step
                // (T += gainC - lossC with t = 86400 s) overshot into the clamp
                // (numerical instability, not physics).
                float gainW = heatGainKw * 1000f;
                float conductionWPerK = Math.Max(1f, NewtonCoolingCoefficient * volumeM3
                                      / Math.Max(0.05f, room.insulationFactor) * 1000f)
                                      * deepFreezeFactor;
                float steadyC = outdoorTemp + gainW / conductionWPerK;
                float relaxFactor = (float)Math.Exp(-conductionWPerK * SecondsPerDay / heatCapacityJ);

                float newTempC = steadyC + (room.currentTempC - steadyC) * relaxFactor;
                newTempC = Math.Clamp(newTempC, outdoorTemp - 5f, _state.boilerTargetTempC + 10f);
                room.currentTempC = newTempC;

                // Freeze detection
                if (room.currentTempC < -2f)
                {
                    room.isFrozen = true;
                    room.freezeDamage += 0.1f;
                    _log.Warn($"[Thermal] {room.displayName} froze solid");
                }

                // Pipe burst risk near cold pipes
                foreach (var pipe in _state.pipes)
                {
                    if (pipe.hasBurst) continue;
                    if (pipe.fromRoomId == room.roomId || pipe.toRoomId == room.roomId)
                    {
                        if (room.currentTempC < 0f && pipe.condition < 50f && _rng.NextDouble() < 0.05f)
                        {
                            pipe.hasBurst = true;
                            pipe.burstDay = day;
                            pipe.burstSeverity = _rng.NextFloat();
                            _state.lastIncidentDay = day;

                            var incident = new ThermalIncident
                            {
                                day = day, pipeId = pipe.pipeId, roomId = room.roomId,
                                kind = ThermalIncidentKind.PipeBurst,
                                description = $"Pipe burst in {room.displayName} at {room.currentTempC:F1}°C"
                            };
                            _state.incidentLog.Add(incident);
                            _log.Warn($"[Thermal] {incident.description}");
                            OnIncident?.Invoke(incident);
                        }
                    }
                }
            }

            // BUG-03 warmth propagation: the room-level temperature loop above
            // is where temperature rises; this loop is where that warmth
            // reaches the survivors. Uses the optional ShelterAssignmentSystem
            // reference to enumerate in-room survivors. The assignment
            // system is optional at Core build — tests without a wired
            // assignment system skip this block entirely.
            //
            // Warmth is 0..100 where LOW = worse. Room warmth modifier is
            // positive above 15°C, so a warm room restores Warmth (positive
            // delta = good); a cold room contributes 0 (no further drain
            // here, natural decay still applies in NeedsSystem.Tick).
            //
            // gameHours per day = 24; GetRoomWarmthModifier is the per-day
            // additive pull. NeedsSystem.Modify(survivor, Warmth, +x) restores
            // warmth — including a healthy room capping at 100 via NeedsSystem
            // clamp, plus Warmth-critical threshold checks inside NeedsSystem.
            if (_assignments != null && _needs != null)
                {
                    foreach (var room in _state.rooms)
                    {
                        float warmth = GetRoomWarmthModifier(room.roomId);
                        if (warmth <= 0f) continue;
                        var inRoom = _assignments.GetAssignmentsForRoom(room.roomId);
                        for (int i = 0; i < inRoom.Count; i++)
                        {
                            string survivorId = inRoom[i].SurvivorId;
                            if (!string.IsNullOrEmpty(survivorId))
                                _needs.Modify(survivorId, NeedKind.Warmth, warmth * 24f);
                        }
                    }
                }

            // Frostbite risk: cold rooms (<5°C) with occupants trigger host-handled affliction
            if (_assignments != null)
            {
                foreach (var room in _state.rooms)
                {
                    if (room.currentTempC >= 5f) continue;
                    var occupants = _assignments.GetAssignmentsForRoom(room.roomId);
                    for (int i = 0; i < occupants.Count; i++)
                    {
                        string sid = occupants[i].SurvivorId;
                        if (!string.IsNullOrEmpty(sid))
                            OnFrostbiteRisk?.Invoke(room.roomId, sid);
                    }
                }
            }

            OnThermalChanged?.Invoke();
        }

        public ActionResult RepairPipe(string pipeId, float repairAmount = 20f)
        {
            var pipe = _state.pipes.Find(p => p.pipeId == pipeId);
            if (pipe == null) return ActionResult.Failed("unknown_pipe", "thermal.unknown_pipe");
            if (!pipe.hasBurst) return ActionResult.Blocked("not_burst", "thermal.not_burst");

            pipe.condition = Math.Min(100f, pipe.condition + repairAmount);
            if (pipe.condition >= 50f)
            {
                pipe.hasBurst = false;
                pipe.burstDay = -1;
            }
            OnThermalChanged?.Invoke();
            return ActionResult.Success("thermal.pipe_repaired",
                new Dictionary<string, double> { { "condition", pipe.condition } });
        }

        public ActionResult ThawRoom(string roomId)
        {
            var room = _state.rooms.Find(r => r.roomId == roomId);
            if (room == null) return ActionResult.Failed("unknown_room", "thermal.unknown_room");
            if (!room.isFrozen) return ActionResult.Blocked("not_frozen", "thermal.not_frozen");

            room.isFrozen = false;
            room.currentTempC = Math.Max(room.currentTempC, _state.boilerCurrentTempC * 0.5f);
            OnThermalChanged?.Invoke();
            return ActionResult.Success("thermal.room_thawed");
        }

        public float GetRoomWarmthModifier(string roomId)
        {
            var room = _state.rooms.Find(r => r.roomId == roomId);
            if (room == null) return 0f;
            return room.currentTempC > 15f ? (room.currentTempC - 15f) * 0.02f : 0f;
        }

        public bool IsRoomAvailable(string roomId)
        {
            var room = _state.rooms.Find(r => r.roomId == roomId);
            if (room == null) return false;
            return !room.isFrozen;
        }

        public ShelterThermalState CaptureState() => CloneState(_state);

        public void RestoreState(ShelterThermalState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static ShelterThermalState CloneState(ShelterThermalState src)
        {
            if (src == null) return new ShelterThermalState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<ShelterThermalState>(json) ?? new ShelterThermalState();
        }
    }
}
