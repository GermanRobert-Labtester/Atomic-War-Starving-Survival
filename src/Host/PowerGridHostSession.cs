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

        /// <summary>
        /// Headless/selftest convenience overload: constructs from the embedded
        /// fallback defaults when no data directory is supplied.
        /// </summary>
        public static PowerGridHostSession CreateDefault(ISeededRng rng)
            => CreateDefault(rng, dataDir: null);

        /// <summary>
        /// Constructs from the authoritative power_grid.json catalog
        /// (Assets/StreamingAssets/Data/power_grid.json) via the Core loader.
        /// A missing or unusable catalog falls back to embedded defaults
        /// (ShelterPowerGridCatalogLoader.FallbackDefault) so boot never fails
        /// over a catalog. The previous hardcoded DefaultGrid() snapshot was
        /// removed — catalog and runtime can no longer drift.
        /// </summary>
        public static PowerGridHostSession CreateDefault(ISeededRng rng, string? dataDir)
        {
            var grid = LoadGridJson(dataDir);
            var rooms = new List<PowerGridRoom>();
            foreach (var r in grid.Rooms)
            {
                var priority = ShelterPowerGridCatalogLoader.MapPriority(r.Id, r.DefaultPriority)
                    ?? PowerGridRoomPriority.Standard;
                rooms.Add(new PowerGridRoom(r.Id, r.DisplayName, r.DrawWatts, priority, r.FailureEffectId));
            }
            var state = new PowerGridState
            {
                GenerationWatts = grid.GenerationWattsDefault,
                FuelUnits = grid.FuelUnitsDefault,
                BatteryCapacityWh = grid.BatteryCapacityWhDefault,
                BatteryReserveWh = grid.BatteryCapacityWhDefault
            };
            var session = new PowerGridHostSession(rooms, state, rng);
            // SHELTER_HARDENING: surge tuning is catalog-driven (fields optional;
            // an explicit 0 legitimately disables the weather surge path).
            session.System.ConfigureSurge(
                grid.EmpStormSeverity ?? Ashfall.Core.Shelter.PowerGridSystem.DefaultEmpStormSurgeSeverity,
                grid.SurgeBatteryDrainFraction ?? Ashfall.Core.Shelter.PowerGridSystem.DefaultSurgeBatteryDrainFraction);
            return session;
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

        private static ShelterPowerGridCatalogDef LoadGridJson(string? dataDir)
        {
            if (string.IsNullOrWhiteSpace(dataDir))
                return ShelterPowerGridCatalogLoader.FallbackDefault();
            return ShelterPowerGridCatalogLoader.LoadOrDefault(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }
    }
}
