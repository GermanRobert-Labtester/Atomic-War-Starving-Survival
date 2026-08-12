using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Protocol Zero — Thermal Grid System. Replaces the binary "room is warm"
    /// model with a node-based heat-transfer simulation. Rooms bleed heat
    /// through uninsulated concrete into adjacent rooms and the surface.
    ///
    /// Tick rate: every 5 in-game minutes (configurable) to preserve CPU.
    /// Pipes freeze when room temperature drops below 2C. Burst pipes trigger
    /// RoomFloodingSystem with freezing water, inflicting SevereFrostbite.
    ///
    /// Save/load safe. Plain C#.
    /// </summary>
    [Serializable]
    public class ThermalGridSave
    {
        public string systemId = "thermal_grid";
        public List<ThermalRoomState> rooms = new List<ThermalRoomState>();
        public float outdoorAmbient = -15f;
        public int frozenPipeCount;
        public List<string> burstPipeRoomIds = new List<string>();
    }

    [Serializable]
    public class ThermalRoomState
    {
        public string roomId;
        public float temperatureC = 15f;
        public float rValue = 2.5f;
        public bool hasHeater;
        public bool heaterActive;
        public float heaterOutputC = 45f;
        public bool pipesFrozen;
        public bool pipesBurst;
        public float freezeHoursAccumulated;
        public List<string> adjacentRoomIds = new List<string>();
    }

    /// <summary>
    /// Events raised by the thermal grid for UI + other systems to react.
    /// </summary>
    public struct ThermalPipeBurstEvent
    {
        public string RoomId;
        public float FloodTemperature;
    }

    public struct ThermalPipeFrozeEvent
    {
        public string RoomId;
        public int TotalFrozenPipes;
    }

    public class ThermalGridSystem
    {
        /// <summary>Room temperature below which pipes begin freezing.</summary>
        public const float PipeFreezeThresholdC = 2f;

        /// <summary>Hours below freeze threshold before pipes freeze solid.</summary>
        public const float PipeFreezeHours = 6f;

        /// <summary>Hours below freeze threshold after freezing before pipes burst.</summary>
        public const float PipeBurstHours = 24f;

        /// <summary>Heat lost per degree difference per tick to adjacent rooms.</summary>
        public const float HeatTransferCoefficient = 0.15f;

        /// <summary>Heat lost to the surface per degree above outdoor temp.</summary>
        public const float SurfaceBleedCoefficient = 0.08f;

        /// <summary>R-value divisor: higher R-value = slower heat loss.</summary>
        public const float InsulationFactor = 0.5f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<ThermalPipeBurstEvent> OnPipeBurst;
        public event Action<ThermalPipeFrozeEvent> OnPipeFroze;
        public event Action<string, float> OnRoomTemperatureChanged;

        // ── State ─────────────────────────────────────────────────────
        private readonly Dictionary<string, ThermalRoomState> _rooms = new Dictionary<string, ThermalRoomState>();
        private float _outdoorAmbient = -15f;
        private int _frozenPipeCount;

        public IReadOnlyDictionary<string, ThermalRoomState> Rooms => _rooms;
        public float OutdoorAmbient => _outdoorAmbient;
        public int FrozenPipeCount => _frozenPipeCount;
        public int BurstPipeCount { get; private set; }

        // ── Configuration ─────────────────────────────────────────────
        public void SetOutdoorTemperature(float celsius) => _outdoorAmbient = celsius;

        /// <summary>
        /// Register a room in the thermal grid. Call once per room at init.
        /// </summary>
        public void RegisterRoom(ShelterRoom room, float rValue = 2.5f, float startingTemp = 15f)
        {
            if (room == null || string.IsNullOrEmpty(room.RoomId)) return;
            if (_rooms.ContainsKey(room.RoomId)) return;

            _rooms[room.RoomId] = new ThermalRoomState
            {
                roomId = room.RoomId,
                temperatureC = startingTemp,
                rValue = Mathf.Max(0.1f, rValue),
                adjacentRoomIds = new List<string>()
            };
        }

        /// <summary>
        /// Declare that two rooms share a wall/floor for heat transfer.
        /// </summary>
        public void ConnectRooms(string roomA, string roomB)
        {
            if (string.IsNullOrEmpty(roomA) || string.IsNullOrEmpty(roomB)) return;
            if (!_rooms.TryGetValue(roomA, out var stateA)) return;
            if (!_rooms.TryGetValue(roomB, out var stateB)) return;

            if (!stateA.adjacentRoomIds.Contains(roomB))
                stateA.adjacentRoomIds.Add(roomB);
            if (!stateB.adjacentRoomIds.Contains(roomA))
                stateB.adjacentRoomIds.Add(roomA);
        }

        /// <summary>
        /// Set heater state for a room — called when player toggles heater module.
        /// </summary>
        public void SetHeater(string roomId, bool active, float outputC = 45f)
        {
            if (!_rooms.TryGetValue(roomId, out var state)) return;
            state.hasHeater = true;
            state.heaterActive = active;
            state.heaterOutputC = outputC;
        }

        /// <summary>
        /// Main tick — called every 5 in-game minutes. Advances heat transfer
        /// and pipe freeze/thaw/burst simulation.
        /// </summary>
        public void Tick(float gameHours, NeedsSystem needsSystem = null, List<Survivor> survivors = null)
        {
            if (gameHours <= 0f) return;

            // Phase 1: Heat transfer between adjacent rooms + surface bleed.
            var deltas = new Dictionary<string, float>();
            foreach (var kv in _rooms)
            {
                var room = kv.Value;
                float delta = 0f;

                // Heater input
                if (room.hasHeater && room.heaterActive)
                {
                    float heaterRise = (room.heaterOutputC - room.temperatureC) * 0.3f * gameHours;
                    delta += Mathf.Max(0f, heaterRise);
                }

                // Surface bleed
                float surfaceDelta = (room.temperatureC - _outdoorAmbient)
                    * SurfaceBleedCoefficient
                    * (1f / Mathf.Max(0.5f, room.rValue * InsulationFactor))
                    * gameHours;
                delta -= surfaceDelta;

                // Adjacent room transfer
                for (int i = 0; i < room.adjacentRoomIds.Count; i++)
                {
                    if (!_rooms.TryGetValue(room.adjacentRoomIds[i], out var adj)) continue;
                    float transfer = (room.temperatureC - adj.temperatureC)
                        * HeatTransferCoefficient * gameHours;
                    delta -= transfer;
                }

                deltas[room.roomId] = delta;
            }

            // Apply all deltas
            foreach (var kv in deltas)
            {
                if (!_rooms.TryGetValue(kv.Key, out var room)) continue;
                room.temperatureC = Mathf.Clamp(room.temperatureC + kv.Value, -60f, 60f);
                OnRoomTemperatureChanged?.Invoke(room.roomId, room.temperatureC);
            }

            // Phase 2: Pipe freeze / burst simulation
            _frozenPipeCount = 0;
            BurstPipeCount = 0;
            foreach (var kv in _rooms)
            {
                var room = kv.Value;

                if (room.temperatureC < PipeFreezeThresholdC)
                {
                    room.freezeHoursAccumulated += gameHours;

                    if (!room.pipesFrozen && room.freezeHoursAccumulated >= PipeFreezeHours)
                    {
                        room.pipesFrozen = true;
                        _frozenPipeCount++;
                        OnPipeFroze?.Invoke(new ThermalPipeFrozeEvent
                        {
                            RoomId = room.roomId,
                            TotalFrozenPipes = _frozenPipeCount
                        });
                    }

                    if (room.pipesFrozen && !room.pipesBurst
                        && room.freezeHoursAccumulated >= PipeBurstHours)
                    {
                        room.pipesBurst = true;
                        BurstPipeCount++;
                        OnPipeBurst?.Invoke(new ThermalPipeBurstEvent
                        {
                            RoomId = room.roomId,
                            FloodTemperature = room.temperatureC
                        });

                        // Apply SevereFrostbite to anyone in the room
                        if (survivors != null && needsSystem != null)
                        {
                            for (int i = 0; i < survivors.Count; i++)
                            {
                                var sv = survivors[i];
                                if (sv == null || !sv.IsAlive) continue;
                                if (sv.CurrentRoomId == room.roomId)
                                {
                                    needsSystem.Modify(sv, NeedKind.Warmth, -40f);
                                    needsSystem.Modify(sv, NeedKind.Health, -25f);
                                }
                            }
                        }
                    }
                }
                else if (room.temperatureC >= PipeFreezeThresholdC + 3f)
                {
                    // Thaw: temperature well above freezing clears freeze state
                    if (room.pipesFrozen && !room.pipesBurst)
                    {
                        room.pipesFrozen = false;
                        room.freezeHoursAccumulated = 0f;
                    }
                }
            }
        }

        /// <summary>
        /// Repair burst pipes in a room — requires thermal paste + blowtorch.
        /// Called by crafting system after successful thaw recipe.
        /// </summary>
        public bool RepairBurstPipes(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return false;
            if (!room.pipesBurst) return false;

            room.pipesBurst = false;
            room.pipesFrozen = false;
            room.freezeHoursAccumulated = 0f;
            return true;
        }

        /// <summary>
        /// Apply thermal paste to thaw pipes before they burst.
        /// </summary>
        public bool ThawPipes(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return false;
            if (!room.pipesFrozen || room.pipesBurst) return false;

            room.pipesFrozen = false;
            room.freezeHoursAccumulated = 0f;
            return true;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public ThermalGridSave CaptureState()
        {
            var save = new ThermalGridSave
            {
                outdoorAmbient = _outdoorAmbient,
                frozenPipeCount = _frozenPipeCount,
                burstPipeRoomIds = new List<string>()
            };

            foreach (var kv in _rooms)
            {
                var r = kv.Value;
                save.rooms.Add(new ThermalRoomState
                {
                    roomId = r.roomId,
                    temperatureC = r.temperatureC,
                    rValue = r.rValue,
                    hasHeater = r.hasHeater,
                    heaterActive = r.heaterActive,
                    heaterOutputC = r.heaterOutputC,
                    pipesFrozen = r.pipesFrozen,
                    pipesBurst = r.pipesBurst,
                    freezeHoursAccumulated = r.freezeHoursAccumulated,
                    adjacentRoomIds = new List<string>(r.adjacentRoomIds)
                });
                if (r.pipesBurst)
                    save.burstPipeRoomIds.Add(r.roomId);
            }

            return save;
        }

        public void RestoreState(ThermalGridSave save)
        {
            _rooms.Clear();
            _frozenPipeCount = 0;
            BurstPipeCount = 0;
            if (save == null) return;

            _outdoorAmbient = save.outdoorAmbient;
            _frozenPipeCount = save.frozenPipeCount;

            for (int i = 0; i < save.rooms.Count; i++)
            {
                var s = save.rooms[i];
                if (s == null) continue;
                _rooms[s.roomId] = s;
                if (s.pipesFrozen) _frozenPipeCount++;
                if (s.pipesBurst) BurstPipeCount++;
            }
        }
    }
}
