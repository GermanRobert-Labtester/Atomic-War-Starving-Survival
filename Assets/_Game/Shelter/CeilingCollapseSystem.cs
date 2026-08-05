using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Prompt #122 — Ceiling load: rooms > 3 tiles without ShoringStrut collapse during shockwaves.</summary>
    public class CeilingCollapseSystem
    {
        public const int MaxUnsupportedTiles = 3;
        public const string ShoringStrutModuleId = "shoring_strut";
        public const float CollapseChancePerShockwave = 0.6f;

        private readonly Dictionary<string, float> _roomTiles = new Dictionary<string, float>(); // roomId -> tile count
        private readonly Dictionary<string, float> _roomLoadMultiplier = new Dictionary<string, float>(); // roomId -> ceiling capacity mult
        private readonly HashSet<string> _collapsedRooms = new HashSet<string>();

        public event Action<string> OnCeilingCollapsed;

        public void RegisterRoom(string roomId, float tileCount) { if (tileCount > 0) _roomTiles[roomId] = tileCount; }
        public bool IsCollapsed(string roomId) => _collapsedRooms.Contains(roomId);

        /// <summary>
        /// Prompt #196 — Structural Engineer: rooms they reinforce gain 2× ceiling-load capacity.
        /// </summary>
        public void ReinforceRoom(string roomId, float loadMultiplier)
        {
            if (string.IsNullOrEmpty(roomId) || loadMultiplier <= 0f) return;
            float current = _roomLoadMultiplier.TryGetValue(roomId, out float m) ? m : 1f;
            _roomLoadMultiplier[roomId] = Mathf.Max(current, loadMultiplier);
        }

        public float GetCeilingLoadMultiplier(string roomId) =>
            !string.IsNullOrEmpty(roomId) && _roomLoadMultiplier.TryGetValue(roomId, out float m)
                ? m : 1f;

        public bool NeedsStrut(string roomId, Shelter shelter)
        {
            if (!_roomTiles.TryGetValue(roomId, out float tiles)) return false;
            float capacity = MaxUnsupportedTiles * GetCeilingLoadMultiplier(roomId);
            if (tiles <= capacity) return false;
            var strut = shelter?.GetModule(ShoringStrutModuleId);
            return strut == null || !strut.IsOperational || strut.Level < Mathf.CeilToInt(tiles / capacity);
        }

        public void ApplyShockwave(Shelter shelter, System.Random rng)
        {
            if (shelter?.Rooms == null) return;
            for (int i = 0; i < shelter.Rooms.Count; i++)
            {
                var room = shelter.Rooms[i];
                if (room == null || _collapsedRooms.Contains(room.RoomId)) continue;
                if (!NeedsStrut(room.RoomId, shelter)) continue;
                if ((rng?.NextDouble() ?? 0.5) < CollapseChancePerShockwave)
                {
                    _collapsedRooms.Add(room.RoomId);
                    OnCeilingCollapsed?.Invoke(room.RoomId);
                }
            }
        }

        /// <summary>
        /// Per-day passive check (SystemWiring). For over-tile rooms that are
        /// missing struts, roll a small daily collapse chance. Captures the
        /// "creaking ceiling" failure mode without a dedicated shockwave event.
        /// </summary>
        public void DailyCollapseCheck(Shelter shelter, System.Random rng)
        {
            if (shelter?.Rooms == null || rng == null) return;
            const float DailyPassiveChance = 0.04f; // 4%/day/over-tile room
            for (int i = 0; i < shelter.Rooms.Count; i++)
            {
                var room = shelter.Rooms[i];
                if (room == null || _collapsedRooms.Contains(room.RoomId)) continue;
                if (!NeedsStrut(room.RoomId, shelter)) continue;
                if (rng.NextDouble() < DailyPassiveChance)
                {
                    _collapsedRooms.Add(room.RoomId);
                    OnCeilingCollapsed?.Invoke(room.RoomId);
                }
            }
        }

        public CeilingCollapseSave CaptureState()
        {
            SaveCollectionHelpers.CaptureStringFloatDict(_roomTiles, out var keys, out var vals);
            SaveCollectionHelpers.CaptureStringFloatDict(_roomLoadMultiplier, out var loadKeys, out var loadVals);
            return new CeilingCollapseSave
            {
                RoomTileKeys = keys,
                RoomTileValues = vals,
                LoadMultKeys = loadKeys,
                LoadMultValues = loadVals,
                CollapsedRoomIds = SaveCollectionHelpers.CaptureStringSet(_collapsedRooms)
            };
        }

        public void RestoreState(CeilingCollapseSave save)
        {
            _roomTiles.Clear();
            _roomLoadMultiplier.Clear();
            _collapsedRooms.Clear();
            if (save == null) return;
            SaveCollectionHelpers.RestoreStringFloatDict(_roomTiles, save.RoomTileKeys, save.RoomTileValues);
            SaveCollectionHelpers.RestoreStringFloatDict(_roomLoadMultiplier, save.LoadMultKeys, save.LoadMultValues);
            SaveCollectionHelpers.RestoreStringSet(_collapsedRooms, save.CollapsedRoomIds);
        }
    }
    [Serializable]
    public class CeilingCollapseSave
    {
        public string[] RoomTileKeys;
        public float[] RoomTileValues;
        public string[] LoadMultKeys;
        public float[] LoadMultValues;
        public string[] CollapsedRoomIds;
    }
}
