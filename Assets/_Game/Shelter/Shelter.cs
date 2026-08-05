using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Shelter.Modules;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// The bunker aggregate: manages installed upgradeable modules, exposes aggregate
    /// stats (IndoorRadLevel, IndoorTempBonus, AirQuality), and advances module logic.
    /// Save/load safe and null-reference safe on add/remove at runtime.
    /// </summary>
    [Serializable]
    public partial class Shelter
    {
        [SerializeField]
        private List<ShelterModuleInstance> _modules = new List<ShelterModuleInstance>();

        /// <summary>Runtime shelter rooms (atmosphere, rubble, storage). Prompt #5 + Internal Horror.</summary>
        [SerializeField]
        private List<ShelterRoom> _rooms = new List<ShelterRoom>();

        /// <summary>Undirected room adjacency pairs ("a|b" with a &lt; b) for noise/sleep.</summary>
        [SerializeField]
        private List<string> _roomAdjacencyKeys = new List<string>();

        public IReadOnlyList<ShelterModuleInstance> Modules => _modules;

        /// <summary>Registered bunker rooms (entry, stores, sealed wings, …).</summary>
        public IReadOnlyList<ShelterRoom> Rooms => _rooms;

        public event Action<ShelterModuleInstance> OnModuleAdded;
        public event Action<string> OnModuleRemoved;
        public event Action<ShelterModuleInstance, int> OnModuleUpgraded;

        // Legacy compatibility properties
        public Shielding Shielding { get; private set; }
        public AirFiltration AirFiltration { get; private set; }

        public Shelter()
        {
            Shielding = new Shielding();
            AirFiltration = new AirFiltration();
            Shielding.BindShelter(this);
            AirFiltration.BindShelter(this);
        }

        public ShelterModuleInstance GetModule(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId) || _modules == null) return null;
            for (int i = 0; i < _modules.Count; i++)
            {
                if (_modules[i] != null && _modules[i].ModuleId == moduleId)
                {
                    return _modules[i];
                }
            }
            return null;
        }

        public ShelterModuleInstance GetModule<T>() where T : ShelterModule
        {
            if (_modules == null) return null;
            for (int i = 0; i < _modules.Count; i++)
            {
                if (_modules[i] != null && _modules[i].Definition is T)
                {
                    return _modules[i];
                }
            }
            return null;
        }

        public void AddModule(ShelterModuleInstance module)
        {
            if (module == null || string.IsNullOrEmpty(module.ModuleId)) return;
            var existing = GetModule(module.ModuleId);
            if (existing != null)
            {
                _modules.Remove(existing);
            }
            _modules.Add(module);
            OnModuleAdded?.Invoke(module);
        }

        public bool RemoveModule(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId) || _modules == null) return false;
            var existing = GetModule(moduleId);
            if (existing != null)
            {
                bool removed = _modules.Remove(existing);
                if (removed)
                {
                    OnModuleRemoved?.Invoke(moduleId);
                }
                return removed;
            }
            return false;
        }

        /// <summary>Half-life of bunker contamination in game-hours. Roughly 4 days.</summary>
        public const float BunkerContaminationHalfLifeHours = 96f;

        public void NotifyModuleUpgraded(ShelterModuleInstance module, int newLevel)
        {
            OnModuleUpgraded?.Invoke(module, newLevel);
        }

        /// <summary>Lookup a registered room by id.</summary>
        public ShelterRoom GetRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId) || _rooms == null) return null;
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i] != null && _rooms[i].RoomId == roomId)
                    return _rooms[i];
            }
            return null;
        }

        /// <summary>Every unique RoomId across registered rooms and modules.</summary>
        public List<string> GetRoomIds()
        {
            var ids = new List<string>();
            if (_rooms != null)
            {
                for (int i = 0; i < _rooms.Count; i++)
                {
                    var r = _rooms[i];
                    if (r == null || string.IsNullOrEmpty(r.RoomId)) continue;
                    if (!ids.Contains(r.RoomId)) ids.Add(r.RoomId);
                }
            }
            if (_modules == null) return ids;
            for (int i = 0; i < _modules.Count; i++)
            {
                var m = _modules[i];
                if (m == null || string.IsNullOrEmpty(m.RoomId)) continue;
                if (!ids.Contains(m.RoomId)) ids.Add(m.RoomId);
            }
            return ids;
        }

        private static string RoomAdjacencyKey(string roomA, string roomB)
        {
            if (string.CompareOrdinal(roomA, roomB) <= 0)
                return roomA + "|" + roomB;
            return roomB + "|" + roomA;
        }
    }
}
