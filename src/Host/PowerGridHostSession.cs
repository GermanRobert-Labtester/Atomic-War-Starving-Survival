using System;
using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Power Grid host session (item 13).
    ///
    /// Thin Godot-side glue: holds the Core <see cref="PowerGridSystem"/>,
    /// exposes a small command surface to UI panels, loads/saves through
    /// <see cref="PowerGridSaveStore"/>, and registers the system with the
    /// Campaign Day Coordinator so its tick runs at the right point in the
    /// daily seam.
    /// </summary>
    public sealed class PowerGridHostSession
    : HostSessionBase{
        public PowerGridSystem System { get; private set; }
        public PowerGridSnapshot LastSnapshot { get; private set; }

        private ISeededRng _tickRng;
        private readonly List<PowerGridRoom> _rooms;
        private readonly List<PowerGridRoomSave> _roomSaves;

        public event Action? OnStateChanged;

        public static PowerGridHostSession CreateDefault(ISeededRng rng)
        {
            var grid = LoadGridJson();
            var rooms = new List<PowerGridRoom>();
            foreach (var r in grid.Rooms)
            {
                rooms.Add(new PowerGridRoom(r.RoomId, r.DisplayName, r.DrawWatts,
                    (PowerGridRoomPriority)(int)r.DefaultPriority, r.FailureEffectId));
            }
            var state = new PowerGridState
            {
                GenerationWatts = grid.GenerationWattsDefault,
                FuelUnits = grid.FuelUnitsDefault,
                BatteryCapacityWh = grid.BatteryCapacityWhDefault,
                BatteryReserveWh = grid.BatteryCapacityWhDefault
            };
            return new PowerGridHostSession(rooms, state, rng);
        }

        public PowerGridHostSession(List<PowerGridRoom> rooms,
            PowerGridState initialState, ISeededRng rng)
        {
            _rooms = rooms ?? throw new ArgumentNullException(nameof(rooms));
            _roomSaves = new List<PowerGridRoomSave>(_rooms.Count);
            foreach (var r in _rooms) _roomSaves.Add(PowerGridSaveCodec.FromRoom(r));
            _tickRng = rng ?? throw new ArgumentNullException(nameof(rng));
            System = new PowerGridSystem(initialState, rooms, rng);
            System.OnPowerChanged += _ => OnStateChanged?.Invoke();
            System.OnTickSummary += _ => OnStateChanged?.Invoke();
            LastSnapshot = System.Snapshot();
        }

        public bool ToggleBreaker(string roomId)
        {
            bool ok = System.ToggleBreaker(roomId);
            if (ok) LastSnapshot = System.Snapshot();
            return ok;
        }

        public bool SetBreaker(string roomId, bool closed)
        {
            bool ok = System.SetBreaker(roomId, closed);
            if (ok) LastSnapshot = System.Snapshot();
            return ok;
        }

        public bool SetPriority(string roomId, PowerGridRoomPriority priority)
        {
            bool ok = System.SetPriority(roomId, priority);
            if (ok) LastSnapshot = System.Snapshot();
            return ok;
        }

        public void AddFuel(float units)
        {
            System.AddFuel(units);
            LastSnapshot = System.Snapshot();
        }

        public PowerGridTickSummary TickDay(int day)
        {
            var sum = System.TickDay(day, _tickRng);
            LastSnapshot = System.Snapshot();
            OnStateChanged?.Invoke();
            return sum;
        }

        public bool TrySave()
        {
            var save = new PowerGridSave
            {
                simDay = System.State.SimDay,
                Rooms = _roomSaves,
                State = System.State.Capture()
            };
            return PowerGridSaveStore.TrySave(save);
        }

        public bool TryLoad()
        {
            var loaded = PowerGridSaveStore.TryLoad();
            if (loaded == null) return false;
            System.State.RestoreInto(loaded.State, _rooms);
            LastSnapshot = System.Snapshot();
            OnStateChanged?.Invoke();
            return true;
        }

        private static PowerGridJson LoadGridJson()
        {
            // Default rooms match the authoritative power_grid.json catalog
            // (Assets/StreamingAssets/Data/power_grid.json). The Core
            // PowerGridSystem does not read catalog JSON; the host carries the
            // defaults so the live session starts in the documented state.
            return DefaultGrid();
        }

        private static PowerGridJson DefaultGrid() => new PowerGridJson
        {
            GenerationWattsDefault = 800f,
            BatteryCapacityWhDefault = 4000f,
            FuelUnitsDefault = 100f,
            Rooms = new List<PowerGridRoomJson>
            {
                new PowerGridRoomJson { RoomId = "room_air_filtration", DisplayName = "Air Filtration",
                    DrawWatts = 180f, DefaultPriority = (int)PowerGridRoomPriority.Critical },
                new PowerGridRoomJson { RoomId = "room_clinic", DisplayName = "Clinic",
                    DrawWatts = 120f, DefaultPriority = (int)PowerGridRoomPriority.Critical },
                new PowerGridRoomJson { RoomId = "room_greenhouse", DisplayName = "Greenhouse",
                    DrawWatts = 160f, DefaultPriority = (int)PowerGridRoomPriority.Standard },
                new PowerGridRoomJson { RoomId = "room_foundry", DisplayName = "Silent Foundry",
                    DrawWatts = 220f, DefaultPriority = (int)PowerGridRoomPriority.Low }
            }
        };

        private sealed class PowerGridJson
        {
            public float GenerationWattsDefault;
            public float BatteryCapacityWhDefault;
            public float FuelUnitsDefault;
            public List<PowerGridRoomJson> Rooms;
        }

        private sealed class PowerGridRoomJson
        {
            public string RoomId;
            public string DisplayName;
            public float DrawWatts;
            public int DefaultPriority;
            public string FailureEffectId;
        }
    }
}
