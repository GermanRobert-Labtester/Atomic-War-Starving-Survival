// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// ASHFALL Shelter Assignment System (item 3).
    ///
    /// Tracks survivor-to-room/workstation assignments, room capacity,
    /// eligibility, and assignment status. The Core owns the deterministic
    /// assignment logic; the host reuses the existing HoldfastInteriorView,
    /// RoomHotspotView, and SurvivorActorView to render and interact.
    /// </summary>
    public sealed class ShelterAssignmentSystem
    {
        private readonly ShelterAssignmentState _state;
        private readonly List<ShelterRoom> _rooms;
        private readonly Dictionary<string, int> _baseCapacities;

        public event Action<ShelterAssignmentEvent>? OnAssignmentChanged;

        public ShelterAssignmentSystem(ShelterAssignmentState state,
            IEnumerable<ShelterRoom> rooms, ISeededRng rng)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (rooms == null) throw new ArgumentNullException(nameof(rooms));
            _ = rng ?? throw new ArgumentNullException(nameof(rng));

            _rooms = new List<ShelterRoom>();
            _baseCapacities = new Dictionary<string, int>(StringComparer.Ordinal);
            var roomIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var room in rooms)
            {
                if (room == null || string.IsNullOrWhiteSpace(room.RoomId)) continue;
                string roomId = room.RoomId.Trim();
                if (!roomIds.Add(roomId)) continue;
                _rooms.Add(CloneRoom(room, roomId));
                _baseCapacities[roomId] = Math.Max(0, room.Capacity);
            }
            if (_rooms.Count == 0)
                throw new InvalidOperationException("ShelterAssignmentSystem: at least one room required.");

            _state = new ShelterAssignmentState();
            _state.RestoreInto(state, _rooms);
        }

        public IReadOnlyList<ShelterRoom> Rooms => _rooms;
        public ShelterAssignmentState State => _state;

        public IReadOnlyList<ShelterAssignment> GetAssignments() => _state.Assignments;

        public ShelterAssignment? GetAssignmentForSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            for (int i = 0; i < _state.Assignments.Count; i++)
            {
                var assignment = _state.Assignments[i];
                if (assignment != null
                    && string.Equals(assignment.SurvivorId, survivorId, StringComparison.Ordinal))
                    return assignment;
            }
            return null;
        }

        public IReadOnlyList<ShelterAssignment> GetAssignmentsForRoom(string roomId)
        {
            var list = new List<ShelterAssignment>();
            if (string.IsNullOrEmpty(roomId)) return list;
            for (int i = 0; i < _state.Assignments.Count; i++)
            {
                var assignment = _state.Assignments[i];
                if (assignment != null
                    && assignment.Status == ShelterAssignmentStatus.Active
                    && string.Equals(assignment.RoomId, roomId, StringComparison.Ordinal))
                    list.Add(assignment);
            }
            return list;
        }

        public int GetRoomOccupancy(string roomId)
        {
            int n = 0;
            for (int i = 0; i < _state.Assignments.Count; i++)
            {
                var assignment = _state.Assignments[i];
                if (assignment != null
                    && assignment.Status == ShelterAssignmentStatus.Active
                    && string.Equals(assignment.RoomId, roomId, StringComparison.Ordinal))
                    n++;
            }
            return n;
        }

        /// <summary>
        /// Null-safe ordinal check: returns true if two distinct survivors are actively assigned to the same shelter room.
        /// </summary>
        public bool AreInSameRoom(string? survivorA, string? survivorB)
        {
            if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB)) return false;
            if (string.Equals(survivorA, survivorB, StringComparison.Ordinal)) return false;

            var assignA = GetAssignmentForSurvivor(survivorA);
            var assignB = GetAssignmentForSurvivor(survivorB);

            if (assignA == null || assignB == null) return false;
            if (assignA.Status != ShelterAssignmentStatus.Active || assignB.Status != ShelterAssignmentStatus.Active) return false;
            if (string.IsNullOrEmpty(assignA.RoomId) || string.IsNullOrEmpty(assignB.RoomId)) return false;

            return string.Equals(assignA.RoomId, assignB.RoomId, StringComparison.Ordinal);
        }

        public int GetRoomCapacity(string roomId)
        {
            for (int i = 0; i < _rooms.Count; i++)
                if (_rooms[i].RoomId == roomId) return _rooms[i].Capacity;
            return 0;
        }

        /// <summary>
        /// Reprojects authored room capacities plus completed construction bonuses.
        /// The supplied map is derived from ShelterExpansionSystem's completed
        /// rooms; repeated calls replace, rather than stack, those bonuses.
        /// </summary>
        public void ApplyCapacityBonuses(IReadOnlyDictionary<string, int> capacityBonuses)
        {
            foreach (var room in _rooms)
            {
                _baseCapacities.TryGetValue(room.RoomId, out int baseCapacity);
                int bonus = 0;
                if (capacityBonuses != null)
                    capacityBonuses.TryGetValue(room.RoomId, out bonus);
                room.Capacity = Math.Max(0, baseCapacity + Math.Max(0, bonus));
            }
        }

        public bool CanAssign(string survivorId, string roomId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(roomId))
                return false;
            var room = FindRoom(roomId);
            if (room == null) return false;
            if (GetRoomOccupancy(roomId) >= room.Capacity) return false;
            var existing = GetAssignmentForSurvivor(survivorId);
            if (existing?.Status == ShelterAssignmentStatus.Active) return false;
            return true;
        }

        public ShelterAssignmentResult Assign(string survivorId, string roomId,
string? workstationId = null, int day = 0)
        {
            if (string.IsNullOrEmpty(survivorId))
                return new ShelterAssignmentResult(false, "missing_survivor_id", null!);
            if (string.IsNullOrEmpty(roomId))
                return new ShelterAssignmentResult(false, "missing_room_id", null!);
            var room = FindRoom(roomId);
            if (room == null)
                return new ShelterAssignmentResult(false, "unknown_room", null!);
            var existing = GetAssignmentForSurvivor(survivorId);
            if (existing?.Status == ShelterAssignmentStatus.Active)
                return new ShelterAssignmentResult(false, "already_assigned", null!);
            if (GetRoomOccupancy(roomId) >= room.Capacity)
                return new ShelterAssignmentResult(false, "room_full", null!);

            ShelterAssignment assignment;
            if (existing != null)
            {
                existing.RoomId = roomId;
                existing.WorkstationId = workstationId ?? string.Empty;
                existing.AssignedDay = day;
                existing.Status = ShelterAssignmentStatus.Active;
                assignment = existing;
            }
            else
            {
                assignment = new ShelterAssignment
                {
                    SurvivorId = survivorId,
                    RoomId = roomId,
                    WorkstationId = workstationId ?? string.Empty,
                    AssignedDay = day,
                    Status = ShelterAssignmentStatus.Active
                };
                _state.Assignments.Add(assignment);
            }
            OnAssignmentChanged?.Invoke(new ShelterAssignmentEvent(
                ShelterAssignmentEventKind.Assigned, survivorId, roomId, day));
            return new ShelterAssignmentResult(true, "ok", assignment);
        }

        public ShelterAssignmentResult Unassign(string survivorId, int day = 0)
        {
            if (string.IsNullOrEmpty(survivorId))
                return new ShelterAssignmentResult(false, "missing_survivor_id", null!);
            for (int i = 0; i < _state.Assignments.Count; i++)
            {
                if (_state.Assignments[i] != null
                    && string.Equals(_state.Assignments[i].SurvivorId, survivorId, StringComparison.Ordinal))
                {
                    string roomId = _state.Assignments[i].RoomId;
                    _state.Assignments.RemoveAt(i);
                    OnAssignmentChanged?.Invoke(new ShelterAssignmentEvent(
                        ShelterAssignmentEventKind.Unassigned, survivorId, roomId, day));
                    return new ShelterAssignmentResult(true, "ok", null!);
                }
            }
            return new ShelterAssignmentResult(false, "not_assigned", null!);
        }

        public ShelterAssignmentState CaptureState() => _state.Capture();

        public void RestoreState(ShelterAssignmentState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state, _rooms);
        }

        private ShelterRoom? FindRoom(string roomId)
        {
            for (int i = 0; i < _rooms.Count; i++)
                if (string.Equals(_rooms[i].RoomId, roomId, StringComparison.Ordinal)) return _rooms[i];
            return null;
        }

        private static ShelterRoom CloneRoom(ShelterRoom source, string roomId)
        {
            return new ShelterRoom
            {
                RoomId = roomId,
                DisplayName = source.DisplayName ?? string.Empty,
                Capacity = Math.Max(0, source.Capacity),
                RequiredSkillId = source.RequiredSkillId ?? string.Empty,
                WorkstationId = source.WorkstationId ?? string.Empty
            };
        }
    }

    [Serializable]
    public sealed class ShelterRoom
    {
        public string RoomId;
        public string DisplayName;
        public int Capacity;
        public string RequiredSkillId; // optional gating; empty = no requirement
        public string WorkstationId; // optional default workstation

        public ShelterRoom() { }

        public ShelterRoom(string roomId, string displayName, int capacity,
string? requiredSkillId = null, string? workstationId = null)
        {
            RoomId = roomId;
            DisplayName = displayName;
            Capacity = capacity;
            RequiredSkillId = requiredSkillId;
            WorkstationId = workstationId;
        }
    }

    [Serializable]
    public sealed class ShelterAssignment
    {
        public string SurvivorId;
        public string RoomId;
        public string WorkstationId;
        public int AssignedDay;
        public ShelterAssignmentStatus Status;

        public ShelterAssignment() { }
    }

    public enum ShelterAssignmentStatus
    {
        Active = 0,
        OnLeave = 1,
        Decommissioned = 2
    }

    [Serializable]
    public sealed class ShelterAssignmentState
    {
        public List<ShelterAssignment> Assignments = new List<ShelterAssignment>();

        public void NormalizeAndValidate(IReadOnlyList<ShelterRoom> rooms)
        {
            Assignments ??= new List<ShelterAssignment>();
            var validIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < rooms.Count; i++) validIds.Add(rooms[i].RoomId);

            // De-duplicate survivor ids (last write wins).
            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = Assignments.Count - 1; i >= 0; i--)
            {
                var assignment = Assignments[i];
                if (assignment == null || string.IsNullOrWhiteSpace(assignment.SurvivorId) ||
                    string.IsNullOrWhiteSpace(assignment.RoomId) ||
                    !validIds.Contains(assignment.RoomId))
                {
                    Assignments.RemoveAt(i);
                    continue;
                }
                if (!seen.Add(assignment.SurvivorId))
                {
                    Assignments.RemoveAt(i);
                    continue;
                }
                if (!Enum.IsDefined(typeof(ShelterAssignmentStatus), assignment.Status))
                    assignment.Status = ShelterAssignmentStatus.Decommissioned;
                assignment.SurvivorId = assignment.SurvivorId.Trim();
                assignment.RoomId = assignment.RoomId.Trim();
                assignment.WorkstationId ??= string.Empty;
            }
        }

        public ShelterAssignmentState Capture()
        {
            var copy = new ShelterAssignmentState
            {
                Assignments = new List<ShelterAssignment>()
            };
            if (Assignments == null) return copy;
            foreach (var assignment in Assignments)
            {
                if (assignment == null) continue;
                copy.Assignments.Add(new ShelterAssignment
                {
                    SurvivorId = assignment.SurvivorId,
                    RoomId = assignment.RoomId,
                    WorkstationId = assignment.WorkstationId,
                    AssignedDay = assignment.AssignedDay,
                    Status = assignment.Status
                });
            }
            return copy;
        }

        public void RestoreInto(ShelterAssignmentState state, IReadOnlyList<ShelterRoom> rooms)
        {
            Assignments = state?.Capture().Assignments ?? new List<ShelterAssignment>();
            NormalizeAndValidate(rooms);
        }
    }

    public enum ShelterAssignmentEventKind
    {
        Assigned,
        Unassigned
    }

    [Serializable]
    public sealed class ShelterAssignmentEvent
    {
        public ShelterAssignmentEventKind Kind;
        public string SurvivorId;
        public string RoomId;
        public int Day;

        public ShelterAssignmentEvent() { }

        public ShelterAssignmentEvent(ShelterAssignmentEventKind kind,
            string survivorId, string roomId, int day)
        {
            Kind = kind;
            SurvivorId = survivorId ?? string.Empty;
            RoomId = roomId ?? string.Empty;
            Day = day;
        }
    }

    [Serializable]
    public sealed class ShelterAssignmentResult
    {
        public bool Succeeded;
        public string ReasonCode;
        public ShelterAssignment Assignment;

        public ShelterAssignmentResult() { }

        public ShelterAssignmentResult(bool succeeded, string reasonCode,
            ShelterAssignment assignment)
        {
            Succeeded = succeeded;
            ReasonCode = reasonCode ?? string.Empty;
            Assignment = assignment;
        }
    }
}
