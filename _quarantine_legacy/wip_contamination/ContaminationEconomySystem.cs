using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// First-class contamination economy: cross-contamination spread between adjacent
    /// items in storage, room ambient contamination rising from dirty items, and
    /// indoor radiation contribution from contaminated rooms. Ticks each game hour
    /// and integrates with ShelterRoom and RadiationSystem.
    /// </summary>
    public class ContaminationEconomySystem
    {
        private readonly List<Shelter.ShelterRoom> _rooms = new List<Shelter.ShelterRoom>();

        /// <summary>Base decay rate for room ambient contamination (per hour, per contamination unit).</summary>
        public float AmbientDecayRatePerHour = 0.01f;

        /// <summary>Fired when any room's contamination changes significantly.</summary>
        public event Action<Shelter.ShelterRoom> OnRoomContaminationChanged;

        /// <summary>Register a room so it participates in cross-contamination and ambient ticks.</summary>
        public void RegisterRoom(Shelter.ShelterRoom room)
        {
            if (room != null && !_rooms.Contains(room))
            {
                _rooms.Add(room);
            }
        }

        /// <summary>Stop ticking a room.</summary>
        public void UnregisterRoom(Shelter.ShelterRoom room)
        {
            _rooms.Remove(room);
        }

        /// <summary>
        /// Advance contamination logic over elapsed game hours:
        /// 1) Cross-contamination spread between adjacent slots within each room.
        /// 2) Room ambient contamination rises from stored dirty items.
        /// 3) Natural ambient decay (ventilation, settling).
        /// 4) Indoor radiation contribution (queried by RadiationSystem via Shelter).
        /// </summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            for (int r = 0; r < _rooms.Count; r++)
            {
                var room = _rooms[r];
                if (room == null || room.Layout == null) continue;

                // 1) Cross-contamination: dirty items contaminate adjacent clean items
                TickCrossContamination(room, gameHours);

                // 2) Room ambient rises from stored dirty items
                TickAmbientAccumulation(room, gameHours);

                // 3) Natural ambient decay
                room.DecayAmbient(gameHours, AmbientDecayRatePerHour);
            }
        }

        /// <summary>
        /// Cross-contamination: each dirty item spreads its contamination to adjacent
        /// clean items over time, with falloff by distance (adjacency).
        /// </summary>
        private void TickCrossContamination(Shelter.ShelterRoom room, float gameHours)
        {
            if (room.Slots == null || room.Layout == null) return;

            float transferRate = room.Layout.contaminationTransferRate;
            if (transferRate <= 0f) return;

            // Snapshot contamination values to avoid order-dependent propagation
            var snapshot = new float[room.Slots.Count];
            for (int i = 0; i < room.Slots.Count; i++)
            {
                snapshot[i] = room.Slots[i].Contamination;
            }

            for (int i = 0; i < room.Slots.Count; i++)
            {
                var slot = room.Slots[i];
                if (slot.IsEmpty || snapshot[i] <= 0f) continue;

                // Spread to adjacent slots
                foreach (int adjIdx in slot.AdjacentSlotIndices)
                {
                    if (adjIdx < 0 || adjIdx >= room.Slots.Count) continue;
                    var adjSlot = room.Slots[adjIdx];
                    if (adjSlot.IsEmpty) continue;

                    // Distance-based falloff
                    float dist = Mathf.Abs(slot.Position.x - adjSlot.Position.x) +
                                 Mathf.Abs(slot.Position.y - adjSlot.Position.y);
                    float falloff = room.Layout.FalloffAtDistance(dist);

                    // Transfer: dirty -> clean
                    float sourceContam = snapshot[i];
                    float targetContam = snapshot[adjIdx];

                    if (sourceContam > targetContam)
                    {
                        float delta = (sourceContam - targetContam) * transferRate * falloff * gameHours;
                        adjSlot.Contamination = Mathf.Clamp01(adjSlot.Contamination + delta);
                    }
                }
            }
        }

        /// <summary>
        /// Room ambient contamination rises from stored dirty items. The total
        /// "contamination load" (sum of each item's contamination * amount) slowly
        /// elevates the room's ambient level.
        /// </summary>
        private void TickAmbientAccumulation(Shelter.ShelterRoom room, float gameHours)
        {
            float load = room.GetStoredContaminationLoad();
            if (load <= 0f) return;

            // Ambient rises proportional to load, capped at 1.0
            float accumulation = load * 0.002f * gameHours; // 0.2% per unit-load per hour
            room.AmbientContamination = Mathf.Clamp01(room.AmbientContamination + accumulation);
        }

        /// <summary>
        /// Total indoor rad contribution from all registered rooms. Queried by
        /// RadiationSystem via Shelter.GetInteriorRadsPerHour.
        /// </summary>
        public float GetTotalIndoorRadContribution()
        {
            float total = 0f;
            foreach (var room in _rooms)
            {
                if (room != null)
                {
                    total += room.GetIndoorRadContribution();
                }
            }
            return total;
        }

        /// <summary>
        /// Total morale penalty from all contaminated rooms. Applied by NeedsSystem.
        /// </summary>
        public float GetTotalMoralePenaltyPerHour()
        {
            float total = 0f;
            foreach (var room in _rooms)
            {
                if (room != null)
                {
                    total += room.GetMoralePenaltyPerHour();
                }
            }
            return total;
        }

        /// <summary>
        /// Clean a room's ambient contamination (survivor action: costs time + water).
        /// Returns the amount cleaned.
        /// </summary>
        public float CleanRoom(Shelter.ShelterRoom room, float cleanAmount)
        {
            if (room == null || cleanAmount <= 0f) return 0f;
            float oldVal = room.AmbientContamination;
            room.AmbientContamination = Mathf.Max(0f, room.AmbientContamination - cleanAmount);
            float cleaned = oldVal - room.AmbientContamination;
            if (cleaned > 0f)
            {
                OnRoomContaminationChanged?.Invoke(room);
            }
            return cleaned;
        }

        /// <summary>Get a room by id.</summary>
        public Shelter.ShelterRoom GetRoom(string roomId)
        {
            foreach (var room in _rooms)
            {
                if (room != null && room.RoomId == roomId) return room;
            }
            return null;
        }

        /// <summary>All registered rooms.</summary>
        public IReadOnlyList<Shelter.ShelterRoom> Rooms => _rooms;
    }
}
