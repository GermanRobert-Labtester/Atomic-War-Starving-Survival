using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE STANDING RECORD — room cards + adjacency. Node ticks, not a walker.
    /// No 3D interiors. No action stealth. Expedition at a parent picks a lit room.
    /// Spec: docs/expansions/expansion_03_the_standing_record_plan.md §5.1.
    /// </summary>
    public class LocationLayoutRoomDef
    {
        public string id;
        public string displayName;
        public string inspect;
        public string description;
        public string[] adjacent;
        public string unlockRule;
        public string inspectKey;
    }

    public class LocationLayoutDef
    {
        public string parentLocationId;
        public string displayName;
        public LocationLayoutRoomDef[] rooms;

        public int RoomCount => rooms != null ? rooms.Length : 0;

        public LocationLayoutRoomDef GetRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId) || rooms == null) return null;
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i] != null && rooms[i].id == roomId)
                    return rooms[i];
            }
            return null;
        }
    }

    [Serializable]
    public class LocationLayoutParentSave
    {
        public string parentLocationId;
        public List<string> unlockedRoomIds = new List<string>();
        public List<string> enteredRoomIds = new List<string>();
        public List<string> inspectedRoomIds = new List<string>();
        public List<string> flags = new List<string>();
    }

    [Serializable]
    public class LocationLayoutState
    {
        public string systemId = LocationLayoutSystem.SystemId;
        public bool expansionUnlocked;
        public string currentParentId;
        public List<LocationLayoutParentSave> parents = new List<LocationLayoutParentSave>();
    }

    /// <summary>
    /// Loads standing_record_layouts.json through host ports (no UnityEngine / Godot).
    /// Enter room → inspect → adjacent rooms light or stay dark.
    /// </summary>
    public sealed class LocationLayoutSystem
    {
        public const string SystemId = "location_layout_system";
        public const string FlagExpUnlocked = "exp_standing_record_unlocked";
        public const string LayoutsFile = "standing_record_layouts.json";
        public const string LocKilometre19 = "loc_cut_kilometre_19";
        public const string LocTransitHq = "loc_transit_authority_hq";

        public const string RoomKm19Post = "room_km19_post";
        public const string RoomKm19Seam = "room_km19_seam";
        public const string RoomKm19OilTin = "room_km19_oil_tin";
        public const string RoomKm19PlateCrate = "room_km19_plate_crate";
        public const string RoomTransitLobby = "room_transit_lobby";
        public const string RoomTransitMapGlass = "room_transit_map_glass";
        public const string RoomTransitDobDesk = "room_transit_dob_desk";
        public const string RoomTransitOverlayBench = "room_transit_overlay_bench";
        public const string RoomTransitRadioGallery = "room_transit_radio_gallery";

        public const string UnlockEntry = "entry";
        public const string UnlockInspectedNeighbour = "inspected_neighbour";
        public const string UnlockInspectPrefix = "inspect:";

        /// <summary>Average indoor tick (~12 minutes). Needs still sit on the parent expedition.</summary>
        public const float RoomTickHours = 0.2f;

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;
        private readonly List<LocationLayoutDef> _layouts = new List<LocationLayoutDef>();
        private readonly Dictionary<string, LocationLayoutDef> _byParent =
            new Dictionary<string, LocationLayoutDef>();
        private readonly Dictionary<string, ParentRuntime> _runtime =
            new Dictionary<string, ParentRuntime>();

        private LocationLayoutState _state = new LocationLayoutState();

        public event Action<string, string> OnRoomEntered;
        public event Action<string, string> OnRoomUnlocked;
        public event Action<string> OnLayoutMutated;
        public event Action<LocationLayoutState> OnStateChanged;

        public LocationLayoutState State => _state;
        public bool IsUnlocked => _state.expansionUnlocked;
        public string CurrentParentId => _state.currentParentId;
        public IReadOnlyList<LocationLayoutDef> Layouts => _layouts;

        public LocationLayoutSystem(IFileIO files, IJsonSerializer json, ILog log = null!)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        public LocationLayoutDef GetLayout(string parentLocationId)
        {
            if (string.IsNullOrEmpty(parentLocationId)) return null;
            LocationLayoutDef def;
            return _byParent.TryGetValue(parentLocationId, out def) ? def : null;
        }

        public int LayoutCount => _layouts.Count;

        public void Load(string dataDirectory)
        {
            _layouts.Clear();
            _byParent.Clear();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Standing Record layout directory missing: " + dataDirectory);
                return;
            }

            string path = _files.Combine(dataDirectory, LayoutsFile);
            if (!_files.FileExists(path))
            {
                _log.Warn("Standing Record layouts file missing: " + path);
                return;
            }

            try
            {
                string blob = _files.ReadAllText(path);
                var items = _json.Deserialize<List<LocationLayoutDef>>(blob);
                if (items == null) return;
                for (int i = 0; i < items.Count; i++)
                {
                    LocationLayoutDef def = items[i];
                    if (def == null || string.IsNullOrEmpty(def.parentLocationId)) continue;
                    _layouts.Add(def);
                    _byParent[def.parentLocationId] = def;
                    EnsureRuntime(def.parentLocationId);
                }
            }
            catch (Exception e)
            {
                _log.Error("Standing Record layouts parse failed: " + e.Message);
            }
        }

        /// <summary>Old saves: Overlay rooms stay dark until the seam quest unlocks the pack.</summary>
        public void Unlock()
        {
            if (_state.expansionUnlocked) return;
            _state.expansionUnlocked = true;
            RaiseChanged();
        }

        /// <summary>
        /// Indoor ticks at the parent node (travelHours 0 from parent). Lights entry rooms only.
        /// Cannot enter every room from the arrival click.
        /// </summary>
        public bool ArriveAtParent(string parentLocationId)
        {
            if (!_state.expansionUnlocked) return false;
            if (string.IsNullOrEmpty(parentLocationId)) return false;
            LocationLayoutDef def = GetLayout(parentLocationId)!;
            if (def == null) return false;

            _state.currentParentId = parentLocationId;
            ParentRuntime rt = EnsureRuntime(parentLocationId);
            LightEntryRooms(def, rt);
            RaiseChanged();
            return true;
        }

        public void LeaveParent()
        {
            _state.currentParentId = null!;
            RaiseChanged();
        }

        public bool CanEnter(string roomId)
        {
            return CanEnter(_state.currentParentId, roomId);
        }

        public bool CanEnter(string parentLocationId, string roomId)
        {
            if (!_state.expansionUnlocked) return false;
            if (string.IsNullOrEmpty(parentLocationId) || string.IsNullOrEmpty(roomId))
                return false;
            if (GetLayout(parentLocationId) == null) return false;
            ParentRuntime rt = EnsureRuntime(parentLocationId);
            return rt.Unlocked.Contains(roomId);
        }

        public bool IsRoomDark(string parentLocationId, string roomId)
        {
            if (string.IsNullOrEmpty(parentLocationId) || string.IsNullOrEmpty(roomId))
                return true;
            LocationLayoutDef def = GetLayout(parentLocationId);
            if (def == null || def.GetRoom(roomId) == null) return true;
            ParentRuntime rt = EnsureRuntime(parentLocationId);
            return !rt.Unlocked.Contains(roomId);
        }

        public bool HasEntered(string parentLocationId, string roomId)
        {
            if (string.IsNullOrEmpty(parentLocationId) || string.IsNullOrEmpty(roomId))
                return false;
            ParentRuntime rt;
            if (!_runtime.TryGetValue(parentLocationId, out rt)) return false;
            return rt.Entered.Contains(roomId);
        }

        public bool HasInspected(string parentLocationId, string roomId)
        {
            if (string.IsNullOrEmpty(parentLocationId) || string.IsNullOrEmpty(roomId))
                return false;
            ParentRuntime rt;
            if (!_runtime.TryGetValue(parentLocationId, out rt)) return false;
            return rt.Inspected.Contains(roomId);
        }

        /// <summary>Pick a lit room. Dark rooms are named, not enterable.</summary>
        public bool EnterRoom(string roomId)
        {
            string parentId = _state.currentParentId;
            if (!CanEnter(parentId, roomId)) return false;

            ParentRuntime rt = EnsureRuntime(parentId);
            if (rt.Entered.Add(roomId))
                OnRoomEntered?.Invoke(parentId, roomId);
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Inspect tick. Adjacent rooms light per unlockRule, or stay dark.
        /// Does not run from the bunker menu: must have arrived and entered.
        /// </summary>
        public bool InspectRoom(string roomId)
        {
            string parentId = _state.currentParentId;
            if (string.IsNullOrEmpty(parentId) || string.IsNullOrEmpty(roomId)) return false;
            LocationLayoutDef def = GetLayout(parentId)!;
            if (def == null || def.GetRoom(roomId) == null) return false;
            ParentRuntime rt = EnsureRuntime(parentId);
            if (!rt.Entered.Contains(roomId)) return false;

            rt.Inspected.Add(roomId);
            EvaluateUnlocks(def, rt);
            RaiseChanged();
            return true;
        }

        public string GetInspectKey(string parentLocationId, string roomId)
        {
            LocationLayoutDef def = GetLayout(parentLocationId);
            if (def == null) return null;
            LocationLayoutRoomDef room = def.GetRoom(roomId)!;
            if (room == null) return null;
            return string.IsNullOrEmpty(room.inspectKey) ? null : room.inspectKey;
        }

        public void MutateLayout(string parentLocationId, string mutationId)
        {
            if (string.IsNullOrEmpty(parentLocationId)) return;
            ParentRuntime rt = EnsureRuntime(parentLocationId);
            string flag = string.IsNullOrEmpty(mutationId) ? "mutated" : mutationId;
            if (!rt.Flags.Contains(flag))
                rt.Flags.Add(flag);
            OnLayoutMutated?.Invoke(parentLocationId);
            RaiseChanged();
        }

        public bool HasFlag(string parentLocationId, string flag)
        {
            if (string.IsNullOrEmpty(parentLocationId) || string.IsNullOrEmpty(flag))
                return false;
            ParentRuntime rt;
            if (!_runtime.TryGetValue(parentLocationId, out rt)) return false;
            return rt.Flags.Contains(flag);
        }

        public LocationLayoutState CaptureState()
        {
            var copy = new LocationLayoutState();
            copy.systemId = SystemId;
            copy.expansionUnlocked = _state.expansionUnlocked;
            copy.currentParentId = _state.currentParentId;
            copy.parents = new List<LocationLayoutParentSave>();
            // Ordinal-ordered emission: dictionary iteration order is not a
            // cross-host guarantee, and the parents list is part of the save.
            var parentIds = new List<string>(_runtime.Count);
            foreach (var pair in _runtime) parentIds.Add(pair.Key);
            parentIds.Sort(string.CompareOrdinal);
            for (int pi = 0; pi < parentIds.Count; pi++)
            {
                string parentId = parentIds[pi];
                ParentRuntime rt = _runtime[parentId];
                var save = new LocationLayoutParentSave();
                save.parentLocationId = parentId;
                CopySetToList(rt.Unlocked, save.unlockedRoomIds);
                CopySetToList(rt.Entered, save.enteredRoomIds);
                CopySetToList(rt.Inspected, save.inspectedRoomIds);
                for (int i = 0; i < rt.Flags.Count; i++)
                    save.flags.Add(rt.Flags[i]);
                copy.parents.Add(save);
            }
            return copy;
        }

        public void RestoreState(LocationLayoutState saved)
        {
            if (saved == null) _state = new LocationLayoutState();
            else
            {
                // Deep-copy: the live system must never alias the envelope's lists.
                var fresh = new LocationLayoutState
                {
                    systemId = saved.systemId,
                    expansionUnlocked = saved.expansionUnlocked,
                    currentParentId = saved.currentParentId,
                    parents = new List<LocationLayoutParentSave>()
                };
                if (saved.parents != null)
                {
                    for (int i = 0; i < saved.parents.Count; i++)
                    {
                        LocationLayoutParentSave row = saved.parents[i];
                        if (row == null || string.IsNullOrEmpty(row.parentLocationId)) continue;
                        fresh.parents.Add(new LocationLayoutParentSave
                        {
                            parentLocationId = row.parentLocationId,
                            unlockedRoomIds = row.unlockedRoomIds != null
                                ? new List<string>(row.unlockedRoomIds)
                                : new List<string>(),
                            enteredRoomIds = row.enteredRoomIds != null
                                ? new List<string>(row.enteredRoomIds)
                                : new List<string>(),
                            inspectedRoomIds = row.inspectedRoomIds != null
                                ? new List<string>(row.inspectedRoomIds)
                                : new List<string>(),
                            flags = row.flags != null
                                ? new List<string>(row.flags)
                                : new List<string>()
                        });
                    }
                }
                _state = fresh;
            }
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            if (_state.parents == null) _state.parents = new List<LocationLayoutParentSave>();
            _runtime.Clear();
            for (int i = 0; i < _state.parents.Count; i++)
            {
                LocationLayoutParentSave row = _state.parents[i];
                if (row == null || string.IsNullOrEmpty(row.parentLocationId)) continue;
                ParentRuntime rt = EnsureRuntime(row.parentLocationId);
                FillSet(rt.Unlocked, row.unlockedRoomIds);
                FillSet(rt.Entered, row.enteredRoomIds);
                FillSet(rt.Inspected, row.inspectedRoomIds);
                rt.Flags.Clear();
                if (row.flags != null)
                {
                    for (int f = 0; f < row.flags.Count; f++)
                    {
                        if (!string.IsNullOrEmpty(row.flags[f]) && !rt.Flags.Contains(row.flags[f]))
                            rt.Flags.Add(row.flags[f]);
                    }
                }
            }
            RaiseChanged();
        }

        private void LightEntryRooms(LocationLayoutDef def, ParentRuntime rt)
        {
            if (def.rooms == null) return;
            for (int i = 0; i < def.rooms.Length; i++)
            {
                LocationLayoutRoomDef room = def.rooms[i];
                if (room == null || string.IsNullOrEmpty(room.id)) continue;
                if (!IsEntryRule(room.unlockRule)) continue;
                UnlockRoom(def.parentLocationId, rt, room.id);
            }
        }

        private void EvaluateUnlocks(LocationLayoutDef def, ParentRuntime rt)
        {
            if (def.rooms == null) return;
            bool progressed = true;
            while (progressed)
            {
                progressed = false;
                for (int i = 0; i < def.rooms.Length; i++)
                {
                    LocationLayoutRoomDef room = def.rooms[i];
                    if (room == null || string.IsNullOrEmpty(room.id)) continue;
                    if (rt.Unlocked.Contains(room.id)) continue;
                    if (!RuleSatisfied(def, rt, room)) continue;
                    UnlockRoom(def.parentLocationId, rt, room.id);
                    progressed = true;
                }
            }
        }

        private bool RuleSatisfied(LocationLayoutDef def, ParentRuntime rt, LocationLayoutRoomDef room)
        {
            string rule = room.unlockRule;
            if (string.IsNullOrEmpty(rule) || rule == UnlockInspectedNeighbour)
                return NeighbourInspected(def, rt, room);
            if (IsEntryRule(rule))
                return false;
            if (rule.StartsWith(UnlockInspectPrefix, StringComparison.Ordinal))
            {
                string required = rule.Substring(UnlockInspectPrefix.Length);
                return !string.IsNullOrEmpty(required) && rt.Inspected.Contains(required);
            }
            return false;
        }

        private static bool NeighbourInspected(LocationLayoutDef def, ParentRuntime rt, LocationLayoutRoomDef room)
        {
            if (room.adjacent != null)
            {
                for (int i = 0; i < room.adjacent.Length; i++)
                {
                    string adj = room.adjacent[i];
                    if (!string.IsNullOrEmpty(adj) && rt.Inspected.Contains(adj))
                        return true;
                }
            }

            if (def.rooms == null) return false;
            for (int i = 0; i < def.rooms.Length; i++)
            {
                LocationLayoutRoomDef other = def.rooms[i];
                if (other == null || other == room || other.adjacent == null) continue;
                for (int a = 0; a < other.adjacent.Length; a++)
                {
                    if (other.adjacent[a] == room.id && rt.Inspected.Contains(other.id))
                        return true;
                }
            }
            return false;
        }

        private void UnlockRoom(string parentId, ParentRuntime rt, string roomId)
        {
            if (!rt.Unlocked.Add(roomId)) return;
            OnRoomUnlocked?.Invoke(parentId, roomId);
        }

        private static bool IsEntryRule(string rule)
        {
            return string.Equals(rule, UnlockEntry, StringComparison.Ordinal);
        }

        private ParentRuntime EnsureRuntime(string parentLocationId)
        {
            ParentRuntime rt;
            if (_runtime.TryGetValue(parentLocationId, out rt)) return rt;
            rt = new ParentRuntime();
            _runtime[parentLocationId] = rt;
            return rt;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);

        private static void CopySetToList(HashSet<string> set, List<string> dest)
        {
            dest.Clear();
            foreach (string id in set)
                dest.Add(id);
            // HashSet iteration order is not a cross-host guarantee; the room
            // id lists are part of the save envelope, so emit ordinal-ordered.
            dest.Sort(string.CompareOrdinal);
        }

        private static void FillSet(HashSet<string> dest, List<string> src)
        {
            dest.Clear();
            if (src == null) return;
            for (int i = 0; i < src.Count; i++)
            {
                if (!string.IsNullOrEmpty(src[i]))
                    dest.Add(src[i]);
            }
        }

        private sealed class ParentRuntime
        {
            public readonly HashSet<string> Unlocked = new HashSet<string>();
            public readonly HashSet<string> Entered = new HashSet<string>();
            public readonly HashSet<string> Inspected = new HashSet<string>();
            public readonly List<string> Flags = new List<string>();
        }
    }
}
