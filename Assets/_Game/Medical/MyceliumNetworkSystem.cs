using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Expansion II — The Mycelium Network (Ash-Blight).
    /// A hyper-aggressive, decentralised fungal network that feeds on
    /// radiation and organic decay. Tracks Spore Density per room.
    /// Spores travel through AirFiltration ducts.
    ///
    /// The Bloom Cycle:
    ///   If a corpse sits un-incinerated for &gt;12 hours, the Blight
    ///   blooms, releasing hallucinogenic and lethal spores into the
    ///   ShelterAtmosphereSystem.
    ///
    /// Symptoms:
    ///   Low density:  Affliction_SporeLung (coughing blood, stamina drain)
    ///   High density: Affliction_MycoHallucinations (Utility AI breaks
    ///                 down; survivors attack shadows or open the outer
    ///                 hatch to "let the garden in")
    ///
    /// Counterplay:
    ///   item_fungicide_fogger: clears room instantly
    ///   ShelterModule_Incinerator: burns biomass
    ///   item_uv_lamp_ballast: stunts growth (consumes power)
    ///
    /// Integration: hooks into CorpseManagementSystem.OnCorpseCreated
    /// via the EventBus. If Event_CorpseIncinerated or Event_CorpseBuried
    /// is not fired within 12 in-game hours, trigger Event_SporeBloom.
    ///
    /// Save/load safe. Plain C#. No MonoBehaviour.
    /// </summary>
    [Serializable]
    public class MyceliumNetworkSave
    {
        public string systemId = "mycelium_network";
        public List<MyceliumRoomState> rooms = new List<MyceliumRoomState>();
        public List<MyceliumCorpseTimer> corpseTimers = new List<MyceliumCorpseTimer>();
        public int totalBloomsTriggered;
        public int totalSporeLungCases;
    }

    [Serializable]
    public class MyceliumRoomState
    {
        public string roomId;
        public float sporeDensity; // 0..100 percent
        public bool uvLampActive;
        public float uvLampHoursRemaining;
        public bool bloomActive;
        public float bloomHoursRemaining;
    }

    [Serializable]
    public class MyceliumCorpseTimer
    {
        public string sourceSurvivorId;
        public string roomId;
        public float hoursSinceDeath;
        public bool resolved; // incinerated, buried, or bloomed
    }

    public struct SporeBloomEvent
    {
        public string RoomId;
        public float SporeDensity;
        public string SourceCorpseId;
    }

    public struct SporeDensityChangedEvent
    {
        public string RoomId;
        public float OldDensity;
        public float NewDensity;
        public bool IsThresholdCrossed;
    }

    public struct MycoHallucinationEvent
    {
        public string SurvivorId;
        public string RoomId;
        public float SporeDensity;
    }

    public class MyceliumNetworkSystem
    {
        /// <summary>Hours a corpse can sit before triggering a spore bloom.</summary>
        public const float CorpseBloomThresholdHours = 12f;

        /// <summary>Spore density added per hour by an unprocessed corpse.</summary>
        public const float CorpseSporeRatePerHour = 8f;

        /// <summary>Spore density added per bloom event (instant injection).</summary>
        public const float BloomSporeInjection = 60f;

        /// <summary>Spore density at which SporeLung affliction triggers.</summary>
        public const float SporeLungThreshold = 30f;

        /// <summary>Spore density at which hallucination affliction triggers.</summary>
        public const float HallucinationThreshold = 70f;

        /// <summary>Spore travel rate through ducts per hour (% transferred to adjacent rooms).</summary>
        public const float DuctTransferRate = 0.15f;

        /// <summary>Natural spore decay per hour (dormant settling).</summary>
        public const float NaturalDecayPerHour = 0.5f;

        /// <summary>UV lamp spore suppression rate per hour.</summary>
        public const float UVSuppressionPerHour = 5f;

        /// <summary>UV lamp lifespan in hours.</summary>
        public const float UVLampLifespanHours = 24f;

        /// <summary>UV lamp power draw per hour (kWh).</summary>
        public const float UVPowerDrawPerHour = 2f;

        /// <summary>Fungicide fogger instant clear amount.</summary>
        public const float FungicideClearAmount = 100f;

        /// <summary>Spore carrier trait daily density increase.</summary>
        public const float SporeCarrierDailyIncrease = 5f;

        /// <summary>Spore density above which a room is "locked down" (bloom active).</summary>
        public const float LockdownThreshold = 80f;

        /// <summary>Hours a bloom event lasts before naturally subsiding.</summary>
        public const float BloomDurationHours = 6f;

        /// <summary>Chance per hour per survivor in room to contract SporeLung.</summary>
        public const float SporeLungInfectionChancePerHour = 0.08f;

        /// <summary>Chance per hour per survivor at hallucination density.</summary>
        public const float HallucinationInfectionChancePerHour = 0.15f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<SporeBloomEvent> OnSporeBloom;
        public event Action<SporeDensityChangedEvent> OnSporeDensityChanged;
        public event Action<MycoHallucinationEvent> OnHallucinationTriggered;
        public event Action<string, float> OnRoomBloomStarted;
        public event Action<string> OnRoomBloomSubsided;
        public event Action OnNetworkStateChanged;

        // ── State ─────────────────────────────────────────────────────
        private readonly Dictionary<string, MyceliumRoomState> _rooms = new Dictionary<string, MyceliumRoomState>();
        private readonly List<MyceliumCorpseTimer> _corpseTimers = new List<MyceliumCorpseTimer>();
        private int _totalBloomsTriggered;
        private int _totalSporeLungCases;

        // Host callbacks
        public Action<string, string> InflictAffliction; // survivorId, afflictionId
        public Func<string, bool> HasTrait; // survivorId → bool
        public Func<string, string> GetSurvivorRoom; // survivorId → roomId
        public Func<string, IReadOnlyList<Survivor>> GetSurvivorsInRoom; // roomId → survivors
        public Func<float> GetPowerAvailable; // kWh available

        public IReadOnlyDictionary<string, MyceliumRoomState> Rooms => _rooms;
        public int TotalBloomsTriggered => _totalBloomsTriggered;
        public int TotalSporeLungCases => _totalSporeLungCases;

        /// <summary>Get spore density for a room (0 if room not tracked).</summary>
        public float GetSporeDensity(string roomId)
        {
            return _rooms.TryGetValue(roomId, out var state) ? state.sporeDensity : 0f;
        }

        /// <summary>Check if a room is in active bloom lockdown.</summary>
        public bool IsRoomInBloom(string roomId)
        {
            return _rooms.TryGetValue(roomId, out var state) && state.bloomActive;
        }

        // ── Registration ──────────────────────────────────────────────

        /// <summary>Register a shelter room for spore tracking.</summary>
        public void RegisterRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            if (_rooms.ContainsKey(roomId)) return;
            _rooms[roomId] = new MyceliumRoomState { roomId = roomId };
        }

        // ── Tick ──────────────────────────────────────────────────────
        /// <summary>
        /// Called every game-hour. Advances corpse timers, spore density,
        /// duct transfer, UV suppression, and infection rolls.
        /// </summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            // Phase 1: Corpse timers — check for bloom triggers
            for (int i = _corpseTimers.Count - 1; i >= 0; i--)
            {
                var timer = _corpseTimers[i];
                if (timer.resolved) continue;

                timer.hoursSinceDeath += gameHours;

                if (timer.hoursSinceDeath >= CorpseBloomThresholdHours)
                {
                    TriggerBloom(timer);
                    timer.resolved = true;
                }
            }

            // Phase 2: Per-room spore dynamics
            var roomIds = new List<string>(_rooms.Keys);
            for (int i = 0; i < roomIds.Count; i++)
            {
                var state = _rooms[roomIds[i]];
                float oldDensity = state.sporeDensity;

                // Active bloom injects spores
                if (state.bloomActive)
                {
                    state.sporeDensity = Mathf.Min(100f, state.sporeDensity + CorpseSporeRatePerHour * gameHours);
                    state.bloomHoursRemaining -= gameHours;
                    if (state.bloomHoursRemaining <= 0f)
                    {
                        state.bloomActive = false;
                        OnRoomBloomSubsided?.Invoke(state.roomId);
                    }
                }

                // UV lamp suppression
                if (state.uvLampActive && state.uvLampHoursRemaining > 0f)
                {
                    state.sporeDensity = Mathf.Max(0f, state.sporeDensity - UVSuppressionPerHour * gameHours);
                    state.uvLampHoursRemaining -= gameHours;
                    if (state.uvLampHoursRemaining <= 0f)
                        state.uvLampActive = false;
                }

                // Natural decay
                state.sporeDensity = Mathf.Max(0f, state.sporeDensity - NaturalDecayPerHour * gameHours);

                // Duct transfer to adjacent rooms (simplified: bleed to all other rooms)
                float transfer = state.sporeDensity * DuctTransferRate * gameHours;
                state.sporeDensity = Mathf.Max(0f, state.sporeDensity - transfer);

                // Distribute transferred spores to other rooms
                float perRoom = roomIds.Count > 1 ? transfer / (roomIds.Count - 1) : 0f;
                for (int j = 0; j < roomIds.Count; j++)
                {
                    if (roomIds[j] == state.roomId) continue;
                    if (_rooms.TryGetValue(roomIds[j], out var adj))
                        adj.sporeDensity = Mathf.Min(100f, adj.sporeDensity + perRoom);
                }

                if (Mathf.Abs(state.sporeDensity - oldDensity) > 0.01f)
                {
                    OnSporeDensityChanged?.Invoke(new SporeDensityChangedEvent
                    {
                        RoomId = state.roomId,
                        OldDensity = oldDensity,
                        NewDensity = state.sporeDensity,
                        IsThresholdCrossed =
                            (oldDensity < SporeLungThreshold && state.sporeDensity >= SporeLungThreshold) ||
                            (oldDensity < HallucinationThreshold && state.sporeDensity >= HallucinationThreshold)
                    });
                }
            }

            // Phase 3: Infection rolls for survivors in contaminated rooms
            RollInfections(gameHours);

            OnNetworkStateChanged?.Invoke();
        }

        /// <summary>
        /// Daily tick — handles spore carrier trait accumulation.
        /// </summary>
        public void TickDaily(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                // trait_spore_carrier: asymptomatic carrier
                if (sv.HasTrait("trait_spore_carrier") && !string.IsNullOrEmpty(sv.CurrentRoomId))
                {
                    EnsureRoom(sv.CurrentRoomId);
                    var state = _rooms[sv.CurrentRoomId];
                    state.sporeDensity = Mathf.Min(100f, state.sporeDensity + SporeCarrierDailyIncrease);
                }

                // trait_rot_immunity: immune to SporeLung but -10 morale for roommates
                // (handled by morale system reading the trait)
            }
        }

        private void RollInfections(float gameHours)
        {
            if (InflictAffliction == null || GetSurvivorsInRoom == null) return;

            foreach (var kv in _rooms)
            {
                var state = kv.Value;
                if (state.sporeDensity < SporeLungThreshold) continue;

                var survivors = GetSurvivorsInRoom(state.roomId);
                if (survivors == null) continue;

                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive) continue;

                    // trait_rot_immunity: immune
                    if (sv.HasTrait("trait_rot_immunity")) continue;

                    // SporeLung infection
                    if (state.sporeDensity >= SporeLungThreshold)
                    {
                        float chance = SporeLungInfectionChancePerHour * gameHours *
                            (state.sporeDensity / 100f);
                        if (UnityEngine.Random.value < chance)
                        {
                            InflictAffliction?.Invoke(sv.Id, "affliction_spore_lung");
                            _totalSporeLungCases++;
                        }
                    }

                    // Hallucination trigger at high density
                    if (state.sporeDensity >= HallucinationThreshold)
                    {
                        float chance = HallucinationInfectionChancePerHour * gameHours *
                            (state.sporeDensity / 100f);
                        if (UnityEngine.Random.value < chance)
                        {
                            InflictAffliction?.Invoke(sv.Id, "affliction_myco_hallucinations");
                            OnHallucinationTriggered?.Invoke(new MycoHallucinationEvent
                            {
                                SurvivorId = sv.Id,
                                RoomId = state.roomId,
                                SporeDensity = state.sporeDensity
                            });
                        }
                    }
                }
            }
        }

        private void TriggerBloom(MyceliumCorpseTimer timer)
        {
            EnsureRoom(timer.roomId);
            var state = _rooms[timer.roomId];

            state.sporeDensity = Mathf.Min(100f, state.sporeDensity + BloomSporeInjection);
            state.bloomActive = true;
            state.bloomHoursRemaining = BloomDurationHours;
            _totalBloomsTriggered++;

            OnSporeBloom?.Invoke(new SporeBloomEvent
            {
                RoomId = timer.roomId,
                SporeDensity = state.sporeDensity,
                SourceCorpseId = timer.sourceSurvivorId
            });

            OnRoomBloomStarted?.Invoke(timer.roomId, state.sporeDensity);
        }

        // ── Corpse Integration ────────────────────────────────────────

        /// <summary>
        /// Called when a corpse is created. Starts the bloom countdown.
        /// Wire to CorpseManagementSystem.OnCorpseCreated.
        /// </summary>
        public void OnCorpseSpawned(Survivor survivor, string roomId = null)
        {
            var timer = new MyceliumCorpseTimer
            {
                sourceSurvivorId = survivor?.Id ?? "unknown",
                roomId = roomId ?? survivor?.CurrentRoomId ?? "stores",
                hoursSinceDeath = 0f,
                resolved = false
            };
            _corpseTimers.Add(timer);
        }

        /// <summary>
        /// Called when a corpse is incinerated or buried. Cancels the bloom timer.
        /// Wire to CorpseManagementSystem.OnCorpseBuried / OnCorpseProcessedForFertilizer.
        /// </summary>
        public void OnCorpseResolved(string sourceSurvivorId)
        {
            for (int i = 0; i < _corpseTimers.Count; i++)
            {
                if (_corpseTimers[i].sourceSurvivorId == sourceSurvivorId)
                {
                    _corpseTimers[i].resolved = true;
                    return;
                }
            }
        }

        // ── Actions ───────────────────────────────────────────────────

        /// <summary>
        /// Deploy a fungicide fogger in a room. Instantly clears all spores.
        /// </summary>
        public bool DeployFungicideFogger(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var state)) return false;
            state.sporeDensity = 0f;
            state.bloomActive = false;
            state.bloomHoursRemaining = 0f;
            OnSporeDensityChanged?.Invoke(new SporeDensityChangedEvent
            {
                RoomId = roomId,
                OldDensity = state.sporeDensity,
                NewDensity = 0f,
                IsThresholdCrossed = false
            });
            return true;
        }

        /// <summary>
        /// Install a UV lamp in a room. Suppresses spore growth for 24 hours.
        /// </summary>
        public bool InstallUVLamp(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var state)) return false;
            state.uvLampActive = true;
            state.uvLampHoursRemaining = UVLampLifespanHours;
            return true;
        }

        /// <summary>
        /// Burn out a room with accelerant. Permanently destroys the room
        /// but eradicates all spores. Returns the room id for host to handle
        /// room destruction (10-day excavation rebuild).
        /// </summary>
        public string BurnOutRoom(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var state)) return null;
            state.sporeDensity = 0f;
            state.bloomActive = false;
            state.bloomHoursRemaining = 0f;
            return roomId;
        }

        /// <summary>
        /// Vent a blooming room to the exterior. Loses 40% heat and 15L
        /// clean water but clears spores. Returns heat loss fraction.
        /// </summary>
        public float VentRoom(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var state)) return 0f;
            state.sporeDensity *= 0.2f;
            state.bloomActive = false;
            state.bloomHoursRemaining = 0f;
            return 0.4f; // 40% heat loss
        }

        private void EnsureRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            if (!_rooms.ContainsKey(roomId))
                _rooms[roomId] = new MyceliumRoomState { roomId = roomId };
        }

        // ── Save / Load ────────────────────────────────────────────────
        public MyceliumNetworkSave CaptureState()
        {
            var save = new MyceliumNetworkSave
            {
                totalBloomsTriggered = _totalBloomsTriggered,
                totalSporeLungCases = _totalSporeLungCases
            };

            foreach (var kv in _rooms)
            {
                var s = kv.Value;
                save.rooms.Add(new MyceliumRoomState
                {
                    roomId = s.roomId,
                    sporeDensity = s.sporeDensity,
                    uvLampActive = s.uvLampActive,
                    uvLampHoursRemaining = s.uvLampHoursRemaining,
                    bloomActive = s.bloomActive,
                    bloomHoursRemaining = s.bloomHoursRemaining
                });
            }

            for (int i = 0; i < _corpseTimers.Count; i++)
            {
                var t = _corpseTimers[i];
                if (!t.resolved)
                {
                    save.corpseTimers.Add(new MyceliumCorpseTimer
                    {
                        sourceSurvivorId = t.sourceSurvivorId,
                        roomId = t.roomId,
                        hoursSinceDeath = t.hoursSinceDeath,
                        resolved = false
                    });
                }
            }

            return save;
        }

        public void RestoreState(MyceliumNetworkSave save)
        {
            _rooms.Clear();
            _corpseTimers.Clear();
            _totalBloomsTriggered = 0;
            _totalSporeLungCases = 0;

            if (save == null) return;

            _totalBloomsTriggered = save.totalBloomsTriggered;
            _totalSporeLungCases = save.totalSporeLungCases;

            for (int i = 0; i < save.rooms.Count; i++)
            {
                if (save.rooms[i] != null)
                    _rooms[save.rooms[i].roomId] = save.rooms[i];
            }

            for (int i = 0; i < save.corpseTimers.Count; i++)
            {
                if (save.corpseTimers[i] != null)
                    _corpseTimers.Add(save.corpseTimers[i]);
            }
        }
    }
}
