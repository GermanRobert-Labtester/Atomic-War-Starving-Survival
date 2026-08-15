using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Expansion III — Electromagnetic Decay System (The Dead Hand).
    /// Tracks AmbientEMP and MagneticAnomaly levels in shelter rooms.
    /// If a room housing ShelterModule_Radio or ShelterModule_Autodoc
    /// is not lined with item_faraday_mesh, the ambient EMP slowly
    /// corrupts the DeviceState.
    ///
    /// The Corruption Loop:
    ///   Daily tick evaluates Shielding_Faraday boolean on each room.
    ///   If false, 5% chance of Event_LogicGateFailure to any device
    ///   in that room.
    ///
    /// Symptoms:
    ///   Radio injects phantom coordinates into MapKnowledgeHUD.
    ///   Autodoc administers lethal doses of opioids due to logic errors.
    ///
    /// Counterplay:
    ///   Craft item_faraday_mesh and assign The_Electrician to
    ///   "Line the Walls" — permanently shields the room.
    ///
    /// Save/load safe. Plain C#. No MonoBehaviour.
    /// </summary>
    [Serializable]
    public class ElectromagneticDecaySave
    {
        public string systemId = "electromagnetic_decay";
        public List<EMRoomState> rooms = new List<EMRoomState>();
        public int totalDeviceCorruptions;
        public int totalRoomsShielded;
        public int totalFaradayMeshesUsed;
    }

    [Serializable]
    public class EMRoomState
    {
        public string roomId;
        public float ambientEMP; // 0..100
        public float magneticAnomaly; // 0..100
        public bool faradayShielded;
        public float corruptionLevel; // 0..100, accumulates over time
        public List<EMDeviceState> devices = new List<EMDeviceState>();
    }

    [Serializable]
    public class EMDeviceState
    {
        public string moduleId; // "radio", "autodoc", "water_purifier", etc.
        public float corruptionLevel; // 0..100
        public bool corrupted; // logic gate failure occurred
        public float hoursSinceLastCorruption;
    }

    public struct DeviceCorruptionEvent
    {
        public string RoomId;
        public string ModuleId;
        public float CorruptionLevel;
        public bool IsLogicGateFailure;
    }

    public struct FaradayShieldEvent
    {
        public string RoomId;
        public bool Shielded;
        public float MeshesUsed;
    }

    public struct EMPStormEvent
    {
        public float Intensity;
        public int AffectedRooms;
    }

    public class ElectromagneticDecaySystem
    {
        /// <summary>Daily chance of logic gate failure for unshielded devices (%).</summary>
        public const float DailyCorruptionChance = 5f;

        /// <summary>Corruption added per EMP storm event.</summary>
        public const float EMPStormCorruptionBoost = 25f;

        /// <summary>Corruption level at which a device produces phantom data.</summary>
        public const string PhantomDataThresholdKey = "phantom_threshold";
        public const float PhantomDataThreshold = 30f;

        /// <summary>Corruption level at which a device produces lethal errors.</summary>
        public const float LethalErrorThreshold = 70f;

        /// <summary>Faraday meshes required to shield one room.</summary>
        public const int FaradayMeshesPerRoom = 3;

        /// <summary>Natural EMP decay per day in shielded rooms.</summary>
        public const float ShieldedEMPDecayPerDay = 5f;

        /// <summary>Ambient EMP growth per day in unshielded rooms.</summary>
        public const float UnshieldedEMPGrowthPerDay = 2f;

        /// <summary>Magnetic anomaly added per day near anomaly locations.</summary>
        public const float MagneticAnomalyGrowthPerDay = 1f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<DeviceCorruptionEvent> OnDeviceCorrupted;
        public event Action<FaradayShieldEvent> OnFaradayShieldApplied;
        public event Action<EMPStormEvent> OnEMPStorm;
        public event Action OnDecayStateChanged;

        // ── State ─────────────────────────────────────────────────────
        private readonly Dictionary<string, EMRoomState> _rooms = new Dictionary<string, EMRoomState>();
        private int _totalDeviceCorruptions;
        private int _totalRoomsShielded;
        private int _totalFaradayMeshesUsed;

        // Host callbacks
        public Action<string, string> DisableModule; // roomId, moduleId
        public Action<string> LogPhantomData; // roomId → inject false coordinates
        public Func<string, bool> ConsumeItem; // itemId → bool (consumed)

        public IReadOnlyDictionary<string, EMRoomState> Rooms => _rooms;
        public int TotalDeviceCorruptions => _totalDeviceCorruptions;
        public int TotalRoomsShielded => _totalRoomsShielded;

        /// <summary>Check if a room is Faraday-shielded.</summary>
        public bool IsRoomShielded(string roomId)
        {
            return _rooms.TryGetValue(roomId, out var state) && state.faradayShielded;
        }

        /// <summary>Get corruption level for a specific device in a room.</summary>
        public float GetDeviceCorruption(string roomId, string moduleId)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return 0f;
            for (int i = 0; i < room.devices.Count; i++)
            {
                if (room.devices[i].moduleId == moduleId)
                    return room.devices[i].corruptionLevel;
            }
            return 0f;
        }

        // ── Registration ──────────────────────────────────────────────

        /// <summary>Register a shelter room for EM tracking.</summary>
        public void RegisterRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            if (_rooms.ContainsKey(roomId)) return;
            _rooms[roomId] = new EMRoomState { roomId = roomId };
        }

        /// <summary>Register a device/module in a room for corruption tracking.</summary>
        public void RegisterDevice(string roomId, string moduleId)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return;
            for (int i = 0; i < room.devices.Count; i++)
                if (room.devices[i].moduleId == moduleId) return;
            room.devices.Add(new EMDeviceState { moduleId = moduleId });
        }

        // ── Tick ──────────────────────────────────────────────────────
        /// <summary>
        /// Daily tick — evaluates Faraday shielding on each room.
        /// If unshielded, applies 5% chance of logic gate failure
        /// to any device in that room.
        /// </summary>
        public void TickDaily(System.Random rng = null)
        {
            if (rng == null) rng = new System.Random();

            foreach (var kv in _rooms)
            {
                var room = kv.Value;

                // Phase 1: EMP accumulation/decay
                if (room.faradayShielded)
                {
                    room.ambientEMP = Mathf.Max(0f, room.ambientEMP - ShieldedEMPDecayPerDay);
                    room.corruptionLevel = Mathf.Max(0f, room.corruptionLevel - 1f);
                }
                else
                {
                    room.ambientEMP = Mathf.Min(100f, room.ambientEMP + UnshieldedEMPGrowthPerDay);
                    room.corruptionLevel = Mathf.Min(100f, room.corruptionLevel + 0.5f);
                }

                // Phase 2: Device corruption rolls
                if (!room.faradayShielded)
                {
                    for (int i = 0; i < room.devices.Count; i++)
                    {
                        var device = room.devices[i];
                        if (device.corrupted) continue;

                        device.hoursSinceLastCorruption += 24f;

                        // 5% daily chance of logic gate failure
                        if ((float)rng.NextDouble() * 100f < DailyCorruptionChance)
                        {
                            device.corruptionLevel = Mathf.Min(100f, device.corruptionLevel + 20f);
                            device.hoursSinceLastCorruption = 0f;

                            bool isLogicFailure = device.corruptionLevel >= LethalErrorThreshold;
                            if (isLogicFailure)
                            {
                                device.corrupted = true;
                                _totalDeviceCorruptions++;
                                DisableModule?.Invoke(room.roomId, device.moduleId);
                            }

                            OnDeviceCorrupted?.Invoke(new DeviceCorruptionEvent
                            {
                                RoomId = room.roomId,
                                ModuleId = device.moduleId,
                                CorruptionLevel = device.corruptionLevel,
                                IsLogicGateFailure = isLogicFailure
                            });

                            // Phantom data at moderate corruption
                            if (device.corruptionLevel >= PhantomDataThreshold)
                                LogPhantomData?.Invoke(room.roomId);
                        }
                    }
                }
            }

            OnDecayStateChanged?.Invoke();
        }

        /// <summary>
        /// Apply EMP storm effects — massive corruption boost to all
        /// unshielded rooms. Called when Weather_EMPStorm is active.
        /// </summary>
        public void ApplyEMPStorm(float intensity)
        {
            int affected = 0;
            foreach (var kv in _rooms)
            {
                var room = kv.Value;
                if (room.faradayShielded) continue;

                room.ambientEMP = Mathf.Min(100f, room.ambientEMP + EMPStormCorruptionBoost * intensity);
                room.corruptionLevel = Mathf.Min(100f, room.corruptionLevel + 10f * intensity);
                affected++;
            }

            if (affected > 0)
            {
                OnEMPStorm?.Invoke(new EMPStormEvent
                {
                    Intensity = intensity,
                    AffectedRooms = affected
                });
                OnDecayStateChanged?.Invoke();
            }
        }

        // ── Actions ───────────────────────────────────────────────────

        /// <summary>
        /// Apply Faraday mesh to a room. Permanently shields all devices.
        /// Consumes FaradayMeshesPerRoom meshes from inventory.
        /// </summary>
        public bool LineRoomWalls(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return false;
            if (room.faradayShielded) return false;
            if (ConsumeItem == null) return false;

            // Consume all meshes — partial consumption on failure is acceptable
            // (items are spent one at a time; if supply runs out mid-way the
            //  player simply needs to gather more and retry).
            int consumed = 0;
            for (int i = 0; i < FaradayMeshesPerRoom; i++)
            {
                if (!ConsumeItem("item_faraday_mesh"))
                    break;
                consumed++;
            }

            if (consumed < FaradayMeshesPerRoom) return false;

            room.faradayShielded = true;
            _totalRoomsShielded++;
            _totalFaradayMeshesUsed += FaradayMeshesPerRoom;

            OnFaradayShieldApplied?.Invoke(new FaradayShieldEvent
            {
                RoomId = roomId,
                Shielded = true,
                MeshesUsed = FaradayMeshesPerRoom
            });

            OnDecayStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Repair a corrupted device. Requires item_circuit_board and
        /// 10 days of The_Electrician's labor (host-tracked).
        /// Returns false if device is not corrupted.
        /// </summary>
        public bool BeginDeviceRepair(string roomId, string moduleId)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return false;
            for (int i = 0; i < room.devices.Count; i++)
            {
                if (room.devices[i].moduleId == moduleId && room.devices[i].corrupted)
                {
                    // Reset corruption but keep device marked until repair completes
                    room.devices[i].corruptionLevel = 0f;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Complete device repair — clears corrupted flag.
        /// Called by host when The_Electrician finishes the 10-day project.
        /// </summary>
        public bool CompleteDeviceRepair(string roomId, string moduleId)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return false;
            for (int i = 0; i < room.devices.Count; i++)
            {
                if (room.devices[i].moduleId == moduleId)
                {
                    room.devices[i].corrupted = false;
                    room.devices[i].corruptionLevel = 0f;
                    OnDecayStateChanged?.Invoke();
                    return true;
                }
            }
            return false;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public ElectromagneticDecaySave CaptureState()
        {
            var save = new ElectromagneticDecaySave
            {
                totalDeviceCorruptions = _totalDeviceCorruptions,
                totalRoomsShielded = _totalRoomsShielded,
                totalFaradayMeshesUsed = _totalFaradayMeshesUsed
            };

            foreach (var kv in _rooms)
            {
                var r = kv.Value;
                var roomSave = new EMRoomState
                {
                    roomId = r.roomId,
                    ambientEMP = r.ambientEMP,
                    magneticAnomaly = r.magneticAnomaly,
                    faradayShielded = r.faradayShielded,
                    corruptionLevel = r.corruptionLevel
                };

                for (int i = 0; i < r.devices.Count; i++)
                {
                    var d = r.devices[i];
                    roomSave.devices.Add(new EMDeviceState
                    {
                        moduleId = d.moduleId,
                        corruptionLevel = d.corruptionLevel,
                        corrupted = d.corrupted,
                        hoursSinceLastCorruption = d.hoursSinceLastCorruption
                    });
                }

                save.rooms.Add(roomSave);
            }

            return save;
        }

        public void RestoreState(ElectromagneticDecaySave save)
        {
            _rooms.Clear();
            _totalDeviceCorruptions = 0;
            _totalRoomsShielded = 0;
            _totalFaradayMeshesUsed = 0;

            if (save == null) return;

            _totalDeviceCorruptions = save.totalDeviceCorruptions;
            _totalRoomsShielded = save.totalRoomsShielded;
            _totalFaradayMeshesUsed = save.totalFaradayMeshesUsed;

            for (int i = 0; i < save.rooms.Count; i++)
            {
                if (save.rooms[i] != null)
                    _rooms[save.rooms[i].roomId] = save.rooms[i];
            }
        }
    }
}
