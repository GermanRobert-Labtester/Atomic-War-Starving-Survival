using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Event bus payload fired when concrete spalling occurs in a shelter room.
    /// </summary>
    public struct SpallingEvent
    {
        public string RoomId;
        public float MaterialShieldingRemaining;

        public SpallingEvent(string roomId, float shielding)
        {
            RoomId = roomId;
            MaterialShieldingRemaining = shielding;
        }
    }

    /// <summary>
    /// Expansion IV — Chapter 38.1 Structural Entropy & Material Decay.
    /// Replaces binary structural integrity with node-based CarbonationDepth & RebarCorrosion simulation.
    /// High room Humidity and CO2 levels accelerate rebar corrosion. When RebarCorrosion reaches 1.0,
    /// Event_Spalling is triggered: concrete explodes inward, injuring occupants and permanently reducing
    /// MaterialShielding (letting ambient radiation leak inside).
    /// </summary>
    public class StructuralEntropySystem
    {
        public const float BaseCorrosionRatePerHour = 0.0008f;
        public const float SpallingHealthDamage = 20f;
        public const float SpallingShieldingLoss = 0.35f;
        public const float SpallingAmbientRadLeak = 15f;

        private readonly List<ShelterRoom> _rooms = new List<ShelterRoom>();
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private NeedsSystem _needsSystem;

        public event Action<ShelterRoom> OnSpallingTriggered;
        public event Action<SpallingEvent> OnSpallingEventBus;
        public event Action<ShelterRoom> OnRebarRepaired;

        /// <summary>Read-only view of all registered shelter rooms for the HUD wireframe.</summary>
        public IReadOnlyList<ShelterRoom> Rooms => _rooms;

        /// <summary>
        /// Overall shelter integrity [0,1] — mean of (1 - RebarCorrosion) across non-spalling rooms.
        /// Returns 1 when no rooms are registered.
        /// </summary>
        public float ShelterIntegrity
        {
            get
            {
                if (_rooms.Count == 0) return 1f;
                float sum = 0f;
                int count = 0;
                for (int i = 0; i < _rooms.Count; i++)
                {
                    if (_rooms[i] == null) continue;
                    sum += _rooms[i].IsSpalling ? 0f : (1f - _rooms[i].RebarCorrosion);
                    count++;
                }
                return count > 0 ? sum / count : 1f;
            }
        }

        public StructuralEntropySystem()
        {
        }

        public void BindDependencies(Func<IReadOnlyList<Survivor>> getSurvivors, NeedsSystem needsSystem)
        {
            _getSurvivors = getSurvivors;
            _needsSystem = needsSystem;
        }

        public void RegisterRoom(ShelterRoom room)
        {
            if (room == null || string.IsNullOrEmpty(room.RoomId)) return;
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i] != null && _rooms[i].RoomId == room.RoomId)
                {
                    _rooms[i] = room;
                    return;
                }
            }
            _rooms.Add(room);
        }

        public void OnAtmosphereChanged()
        {
            // Responds to shelter atmosphere changes
        }

        /// <summary>
        /// Daily/hourly tick updating CarbonationDepth and RebarCorrosion for all registered shelter rooms.
        /// </summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            for (int i = 0; i < _rooms.Count; i++)
            {
                var room = _rooms[i];
                if (room == null) continue;

                // High CO2 and Humidity accelerate carbonation and rebar corrosion
                float co2Mult = 1f + (room.Co2Ppm / 500f);
                float humidityMult = 1f + (room.Humidity * 2.5f);
                float corrosionDelta = BaseCorrosionRatePerHour * co2Mult * humidityMult * gameHours;

                room.CarbonationDepth = Mathf.Clamp01(room.CarbonationDepth + corrosionDelta * 0.8f);
                room.RebarCorrosion = Mathf.Clamp01(room.RebarCorrosion + corrosionDelta);

                if (room.RebarCorrosion >= 1.0f && !room.IsSpalling)
                {
                    TriggerSpalling(room);
                }
            }
        }

        /// <summary>
        /// Triggers Event_Spalling on a room whose rebar corrosion hit 1.0.
        /// </summary>
        public void TriggerSpalling(ShelterRoom room)
        {
            if (room == null) return;
            bool alreadySpalling = room.IsSpalling;
            room.IsSpalling = true;

            // Idempotent: do not re-damage occupants or re-apply shielding loss if
            // this room is already spalling. Events are still raised so callers can
            // observe the spalling state without relying on the automatic threshold path.
            if (alreadySpalling)
            {
                OnSpallingTriggered?.Invoke(room);
                OnSpallingEventBus?.Invoke(new SpallingEvent(room.RoomId, room.MaterialShielding));
                return;
            }
            room.MaterialShielding = Mathf.Max(0.1f, room.MaterialShielding - SpallingShieldingLoss);
            room.AmbientRadiation += SpallingAmbientRadLeak;

            // Damage occupants in room
            if (_getSurvivors != null && _needsSystem != null)
            {
                var survivors = _getSurvivors();
                if (survivors != null)
                {
                    for (int i = 0; i < survivors.Count; i++)
                    {
                        var sv = survivors[i];
                        if (sv != null && sv.IsAlive && string.Equals(sv.CurrentRoomId, room.RoomId, StringComparison.OrdinalIgnoreCase))
                        {
                            _needsSystem.Modify(sv, NeedKind.Health, -SpallingHealthDamage);
                            _needsSystem.Modify(sv, NeedKind.Morale, -15f);
                        }
                    }
                }
            }

            OnSpallingTriggered?.Invoke(room);
            OnSpallingEventBus?.Invoke(new SpallingEvent(room.RoomId, room.MaterialShielding));
        }

        /// <summary>
        /// Deep structural epoxy injection counterplay. Resets rebar corrosion to 0.0 and repairs spalling.
        /// </summary>
        public bool InjectEpoxy(ShelterRoom room, Survivor worker)
        {
            if (room == null || worker == null || !worker.IsAlive) return false;

            room.RebarCorrosion = 0f;
            room.CarbonationDepth = 0f;
            room.IsSpalling = false;
            room.MaterialShielding = Mathf.Min(1.0f, room.MaterialShielding + 0.25f);

            OnRebarRepaired?.Invoke(room);
            return true;
        }

        /// <summary>
        /// Event "The Concrete Weep" choice: Human Pillar sacrifice.
        /// </summary>
        public bool ExecuteHumanPillarSacrifice(ShelterRoom room, Survivor martyr)
        {
            if (room == null || martyr == null) return false;

            SurvivorNeedWrite.SetHealth(martyr, 0f);
            room.RebarCorrosion = 0f;
            room.CarbonationDepth = 0f;
            room.IsSpalling = false;
            room.MaterialShielding = 1.0f;

            OnRebarRepaired?.Invoke(room);
            return true;
        }

        public StructuralEntropySave GetState()
        {
            var save = new StructuralEntropySave
            {
                Rooms = new RoomEntropySave[_rooms.Count]
            };
            for (int i = 0; i < _rooms.Count; i++)
            {
                var r = _rooms[i];
                if (r == null) continue;
                save.Rooms[i] = new RoomEntropySave
                {
                    RoomId = r.RoomId,
                    RebarCorrosion = r.RebarCorrosion,
                    CarbonationDepth = r.CarbonationDepth,
                    MaterialShielding = r.MaterialShielding,
                    IsSpalling = r.IsSpalling
                };
            }
            return save;
        }

        public void RestoreState(StructuralEntropySave save)
        {
            if (save?.Rooms == null) return;
            for (int i = 0; i < save.Rooms.Length; i++)
            {
                var row = save.Rooms[i];
                if (row == null || string.IsNullOrEmpty(row.RoomId)) continue;
                for (int r = 0; r < _rooms.Count; r++)
                {
                    if (_rooms[r] != null && _rooms[r].RoomId == row.RoomId)
                    {
                        _rooms[r].RebarCorrosion = row.RebarCorrosion;
                        _rooms[r].CarbonationDepth = row.CarbonationDepth;
                        _rooms[r].MaterialShielding = row.MaterialShielding;
                        _rooms[r].IsSpalling = row.IsSpalling;
                        break;
                    }
                }
            }
        }
    }

    public struct AtmosphereChangedEvent { }

    [Serializable]
    public class StructuralEntropySave
    {
        public RoomEntropySave[] Rooms;
    }

    [Serializable]
    public class RoomEntropySave
    {
        public string RoomId;
        public float RebarCorrosion;
        public float CarbonationDepth;
        public float MaterialShielding;
        public bool IsSpalling;
    }
}
