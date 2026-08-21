using System;
using System.Collections.Generic;

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
        private readonly ISeededRng _rng;

        public event Action<ShelterAssignmentEvent>? OnAssignmentChanged;

        public ShelterAssignmentSystem(ShelterAssignmentState state,
            IEnumerable<ShelterRoom> rooms, ISeededRng rng)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            if (rooms == null) throw new ArgumentNullException(nameof(rooms));
            _rooms = new List<ShelterRoom>();
            foreach (var r in rooms)
            {
                if (r == null || string.IsNullOrEmpty(r.RoomId)) continue;
                _rooms.Add(r);
            }
            if (_rooms.Count == 0)
                throw new InvalidOperationException("ShelterAssignmentSystem: at least one room required.");
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _state.NormalizeAndValidate(_rooms);
        }

        public IReadOnlyList<ShelterRoom> Rooms => _rooms;
        public ShelterAssignmentState State => _state;

        public IReadOnlyList<ShelterAssignment> GetAssignments() => _state.Assignments;

        public ShelterAssignment? GetAssignmentForSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            for (int i = 0; i < _state.Assignments.Count; i++)
                if (_state.Assignments[i].SurvivorId == survivorId)
                    return _state.Assignments[i];
            return null;
        }

        public IReadOnlyList<ShelterAssignment> GetAssignmentsForRoom(string roomId)
        {
            var list = new List<ShelterAssignment>();
            if (string.IsNullOrEmpty(roomId)) return list;
            for (int i = 0; i < _state.Assignments.Count; i++)
                if (_state.Assignments[i].RoomId == roomId)
                    list.Add(_state.Assignments[i]);
            return list;
        }

        public int GetRoomOccupancy(string roomId)
        {
            int n = 0;
            for (int i = 0; i < _state.Assignments.Count; i++)
                if (_state.Assignments[i].RoomId == roomId) n++;
            return n;
        }

        public int GetRoomCapacity(string roomId)
        {
            for (int i = 0; i < _rooms.Count; i++)
                if (_rooms[i].RoomId == roomId) return _rooms[i].Capacity;
            return 0;
        }

        public bool CanAssign(string survivorId, string roomId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(roomId))
                return false;
            var room = FindRoom(roomId);
            if (room == null) return false;
            if (GetRoomOccupancy(roomId) >= room.Capacity) return false;
            if (GetAssignmentForSurvivor(survivorId) != null) return false;
            return true;
        }

        public ShelterAssignmentResult Assign(string survivorId, string roomId,
            string workstationId = null!, int day = 0)
        {
            if (string.IsNullOrEmpty(survivorId))
                return new ShelterAssignmentResult(false, "missing_survivor_id", null!);
            if (string.IsNullOrEmpty(roomId))
                return new ShelterAssignmentResult(false, "missing_room_id", null!);
            var room = FindRoom(roomId);
            if (room == null)
                return new ShelterAssignmentResult(false, "unknown_room", null!);
            if (GetAssignmentForSurvivor(survivorId) != null)
                return new ShelterAssignmentResult(false, "already_assigned", null!);
            if (GetRoomOccupancy(roomId) >= room.Capacity)
                return new ShelterAssignmentResult(false, "room_full", null!);
            var assignment = new ShelterAssignment
            {
                SurvivorId = survivorId,
                RoomId = roomId,
                WorkstationId = workstationId ?? string.Empty,
                AssignedDay = day,
                Status = ShelterAssignmentStatus.Active
            };
            _state.Assignments.Add(assignment);
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
                if (_state.Assignments[i].SurvivorId == survivorId)
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
                if (_rooms[i].RoomId == roomId) return _rooms[i];
            return null;
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
            string requiredSkillId = null!, string workstationId = null!)
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
            var validIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < rooms.Count; i++) validIds.Add(rooms[i].RoomId);

            // De-duplicate survivor ids (last write wins).
            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = Assignments.Count - 1; i >= 0; i--)
            {
                var a = Assignments[i];
                if (a == null || string.IsNullOrEmpty(a.SurvivorId) ||
                    !validIds.Contains(a.RoomId))
                {
                    Assignments.RemoveAt(i);
                    continue;
                }
                if (!seen.Add(a.SurvivorId)) Assignments.RemoveAt(i);
            }
        }

        public ShelterAssignmentState Capture() => new ShelterAssignmentState
        {
            Assignments = new List<ShelterAssignment>(Assignments)
        };

        public void RestoreInto(ShelterAssignmentState state, IReadOnlyList<ShelterRoom> rooms)
        {
            Assignments = state.Assignments ?? new List<ShelterAssignment>();
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
