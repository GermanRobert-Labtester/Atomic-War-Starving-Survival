using System;
using System.Collections.Generic;

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
        private ShelterThermalState _state = new ShelterThermalState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly NeedsSystem _needs;
        private readonly StartingLevelSystem _startingLevel;
        private readonly YearOfAshDeepFreezeSystem _deepFreeze;
        private int _currentDay;

        public ShelterThermalState State => _state;
        public event Action<ThermalIncident> OnIncident;
        public event Action OnThermalChanged;

        public ShelterThermalSystem(
            ISeededRng rng,
            NeedsSystem needs,
            StartingLevelSystem startingLevel,
            YearOfAshDeepFreezeSystem deepFreeze,
            ILog log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _needs = needs ?? throw new ArgumentNullException(nameof(needs));
            _startingLevel = startingLevel ?? throw new ArgumentNullException(nameof(startingLevel));
            _deepFreeze = deepFreeze ?? throw new ArgumentNullException(nameof(deepFreeze));
            _log = log ?? NullLog.Instance;
        }

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
            float totalHeatKw = _state.boilerActive ? _state.boilerFuelLevel * 10f : 0f;
            _state.totalHeatOutputKw = totalHeatKw;

            foreach (var room in _state.rooms)
            {
                float heatGain = 0f;
                if (_state.boilerActive && room.hasRadiator && room.radiatorValveOpen > 0 && !room.isFrozen)
                {
                    float roomShare = room.isPriorityRoom ? 1.5f : 1f;
                    heatGain = totalHeatKw * room.radiatorValveOpen * roomShare / Math.Max(1, _state.rooms.Count);
                }

                // Heat loss to environment (exponential decay toward outdoor temp)
                float outdoorTemp = _deepFreeze.IndoorTempCelsius;
                float heatLoss = (room.currentTempC - outdoorTemp) * 0.1f / (room.insulationFactor + 0.1f);
                heatLoss *= deepFreezeFactor;

                room.currentTempC += heatGain * 0.1f - heatLoss;
                room.currentTempC = Math.Clamp(room.currentTempC, outdoorTemp - 5f, _state.boilerTargetTempC + 10f);

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

            // Feed warmth to NeedsSystem (lightweight port — direct call is the sanctioned path)
            foreach (var room in _state.rooms)
            {
                float warmthDelta = room.currentTempC > 15f ? (room.currentTempC - 15f) * 0.1f : 0f;
                // NeedsSystem warmth is applied via event; here we just set the room temperature
                // The host session reads this and applies to survivor warmth
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

        public ShelterThermalState CaptureState() => _state;
        public void RestoreState(ShelterThermalState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnThermalChanged?.Invoke();
        }
    }
}
