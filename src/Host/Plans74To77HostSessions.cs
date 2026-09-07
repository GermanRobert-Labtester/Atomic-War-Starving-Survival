using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>Godot adapter for the geothermal ORC authority.</summary>
    public sealed class GeothermalOrcHostSession : HostSessionBase
    {
        public GeothermalOrcSystem System { get; }
        public GeothermalOrcSnapshot Snapshot { get; private set; }

        private readonly PowerGridSystem? _powerGrid;

        private GeothermalOrcHostSession(
            GeothermalOrcSystem system,
            PowerGridSystem? powerGrid)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            _powerGrid = powerGrid;
            Snapshot = System.Snapshot();
            PublishPowerContribution();
        }

        public static GeothermalOrcHostSession Create(
            string dataDir,
            ISeededRng rng,
            Inventory? inventory = null,
            PowerGridSystem? powerGrid = null)
        {
            var state = GeothermalOrcSaveStore.TryLoad() ?? new GeothermalOrcState();
            var system = new GeothermalOrcSystem(rng, state, inventory, new GodotLog());
            system.LoadCatalog(GeothermalStrataCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
            return new GeothermalOrcHostSession(system, powerGrid);
        }

        public bool AddLoop(string loopId, string stratumId, string roomId = "")
        {
            bool added = System.AddLoop(loopId, stratumId, roomId);
            if (added) RaiseStateChanged();
            return added;
        }

        public ActionResult CommissionLoop(string loopId)
            => HandleActionResult(System.CommissionLoop(loopId));

        public ActionResult SetFlow(string loopId, float flowLPerMin)
            => HandleActionResult(System.SetFlow(loopId, flowLPerMin));

        public ActionResult SetBypass(string loopId, bool open)
            => HandleActionResult(System.SetBypass(loopId, open));

        public ActionResult Descale(string loopId, float amount = 25f)
            => HandleActionResult(System.Descale(loopId, amount));

        public ActionResult Repair(string loopId, float amount = 20f)
            => HandleActionResult(System.Repair(loopId, amount));

        public GeothermalOrcSnapshot TickDay(int day)
        {
            Snapshot = System.OperateDay(day);
            PublishPowerContribution();
            RaiseStateChanged();
            return Snapshot;
        }

        public string CapturePersisted() =>
            GeothermalOrcSaveStore.TryCapturePersisted(System.CaptureState());

        public void Restore(GeothermalOrcState state)
        {
            System.RestoreState(state);
            Snapshot = System.Snapshot();
            PublishPowerContribution();
            ClearDirty();
        }

        private void PublishPowerContribution()
        {
            _powerGrid?.SetGenerationContribution(
                GeothermalOrcSystem.PowerSourceId,
                Math.Max(0f, Snapshot.ElectricalOutputKw * 1000f));
        }
    }

    /// <summary>Godot adapter for weapon calibration and ammunition quality.</summary>
    public sealed class BallisticsWorkbenchHostSession : HostSessionBase
    {
        public BallisticsWorkbenchSystem System { get; }

        private BallisticsWorkbenchHostSession(BallisticsWorkbenchSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public static BallisticsWorkbenchHostSession Create(
            string dataDir,
            ISeededRng rng,
            Inventory? inventory = null,
            EquipmentConditionSystem? equipment = null)
        {
            var state = BallisticsWorkbenchSaveStore.TryLoad() ?? new BallisticsWorkbenchState();
            var system = new BallisticsWorkbenchSystem(
                rng, state, equipment, inventory, new GodotLog());
            system.LoadCatalog(BallisticsWorkbenchCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
            return new BallisticsWorkbenchHostSession(system);
        }

        public ActionResult Inspect(string weaponInstanceId, int day)
            => HandleActionResult(System.Inspect(weaponInstanceId, day));

        public WeaponBallisticsProfile EnsureProfile(string weaponInstanceId, string profileId)
        {
            var profile = System.EnsureProfile(weaponInstanceId, profileId);
            RaiseStateChanged();
            return profile;
        }

        public ActionResult Calibrate(
            string weaponInstanceId,
            float operatorSkill,
            float toolingCalibration,
            int day)
            => HandleActionResult(System.Calibrate(
                weaponInstanceId, operatorSkill, toolingCalibration, day));

        public ActionResult Refurbish(
            string weaponInstanceId,
            IReadOnlyList<string>? parts,
            float serviceQuality,
            int day)
            => HandleActionResult(System.Refurbish(
                weaponInstanceId, parts, serviceQuality, day));

        public ActionResult AttachOptic(string weaponInstanceId, float opticQuality)
            => HandleActionResult(System.AttachOptic(weaponInstanceId, opticQuality));

        public CustomAmmoBatchState? CreateAmmoBatch(
            string ammoItemId,
            string creatorSurvivorId,
            float skill,
            float toolingQuality,
            int day,
            int amount = 1)
        {
            var batch = System.CreateAmmoBatch(
                ammoItemId, creatorSurvivorId, skill, toolingQuality, day, amount);
            if (batch != null) RaiseStateChanged();
            return batch;
        }

        public BallisticsFireResult RecordFiring(
            string weaponInstanceId,
            string eventId,
            int rounds,
            bool corrosiveAmmo,
            bool overpressureAmmo)
        {
            var result = System.RecordFiring(
                weaponInstanceId, eventId, rounds, corrosiveAmmo, overpressureAmmo);
            if (result.Accepted) RaiseStateChanged();
            return result;
        }

        public string CapturePersisted() =>
            BallisticsWorkbenchSaveStore.TryCapturePersisted(System.CaptureState());

        public void Restore(BallisticsWorkbenchState state)
        {
            System.RestoreState(state);
            ClearDirty();
        }
    }

    /// <summary>Godot adapter for chamber-local aeroponic production.</summary>
    public sealed class AeroponicsHostSession : HostSessionBase
    {
        public AeroponicsSystem System { get; }

        private AeroponicsHostSession(AeroponicsSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public static AeroponicsHostSession Create(
            string dataDir,
            ISeededRng rng,
            Inventory inventory,
            PowerGridSystem? powerGrid = null)
        {
            var state = AeroponicsSaveStore.TryLoad() ?? new AeroponicsState();
            var system = new AeroponicsSystem(
                rng,
                inventory,
                roomId => powerGrid == null || powerGrid.IsRoomPowered(roomId) ? 1f : 0f,
                state,
                new GodotLog());
            system.LoadCatalog(AeroponicsCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
            return new AeroponicsHostSession(system);
        }

        public ActionResult AddChamber(string chamberId, string roomId)
            => HandleActionResult(System.AddChamber(chamberId, roomId));

        public ActionResult Plant(
            string chamberId,
            string cropCycleId,
            string nutrientProfileId,
            int day)
            => HandleActionResult(System.Plant(
                chamberId, cropCycleId, nutrientProfileId, day));

        public ActionResult SetChemistry(string chamberId, float ecMsCm, float ph)
            => HandleActionResult(System.SetChemistry(chamberId, ecMsCm, ph));

        public ActionResult SetLightMode(string chamberId, AeroponicLightMode mode)
            => HandleActionResult(System.SetLightMode(chamberId, mode));

        public ActionResult AddWater(string chamberId, float litres, float quality01)
            => HandleActionResult(System.AddWater(chamberId, litres, quality01));

        public ActionResult MaintainNozzles(string chamberId, float amount, int day)
            => HandleActionResult(System.MaintainNozzles(chamberId, amount, day));

        public ActionResult TreatRootDisease(string chamberId, int day)
            => HandleActionResult(System.TreatRootDisease(chamberId, day));

        public AeroponicHarvestResult Harvest(string chamberId, int day)
        {
            var result = System.Harvest(chamberId, day);
            if (result.Success) RaiseStateChanged();
            return result;
        }

        public void TickDay(int day, float operatorSkill = 0f)
        {
            System.TickDay(day, operatorSkill);
            RaiseStateChanged();
        }

        public string CapturePersisted() =>
            AeroponicsSaveStore.TryCapturePersisted(System.CaptureState());

        public void Restore(AeroponicsState state)
        {
            System.RestoreState(state);
            ClearDirty();
        }
    }

    /// <summary>Godot adapter for pneumatic cargo and memo dispatch.</summary>
    public sealed class PneumaticDispatchHostSession : HostSessionBase
    {
        public PneumaticDispatchSystem System { get; }

        private PneumaticDispatchHostSession(PneumaticDispatchSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public static PneumaticDispatchHostSession Create(
            string dataDir,
            ISeededRng rng)
        {
            var state = PneumaticDispatchSaveStore.TryLoad() ?? new PneumaticNetworkState();
            var system = new PneumaticDispatchSystem(rng, state, new GodotLog());
            system.LoadCatalog(PneumaticNetworkCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
            return new PneumaticDispatchHostSession(system);
        }

        public void RegisterEndpoint(string stationId, Inventory inventory)
            => System.RegisterEndpoint(stationId, inventory);

        public PneumaticDispatchResult Dispatch(
            string sourceStationId,
            string destinationStationId,
            string itemId,
            int amount,
            float massKg,
            float volumeLitres,
            PneumaticDispatchPriority priority,
            int day)
        {
            var result = System.Dispatch(
                sourceStationId,
                destinationStationId,
                itemId,
                amount,
                massKg,
                volumeLitres,
                priority,
                day);
            if (result.Success) RaiseStateChanged();
            return result;
        }

        public ActionResult ClearJam(string capsuleId)
            => HandleActionResult(System.ClearJam(capsuleId));

        public ActionResult Maintain(string linkId, float amount, int day)
            => HandleActionResult(System.Maintain(linkId, amount, day));

        public void SetBlackout(bool blackout)
        {
            System.SetBlackout(blackout);
            RaiseStateChanged();
        }

        public void TickDay(int day, float powerAvailability01 = 1f)
        {
            System.TickDay(day, powerAvailability01);
            RaiseStateChanged();
        }

        public string CapturePersisted() =>
            PneumaticDispatchSaveStore.TryCapturePersisted(System.CaptureState());

        public void Restore(PneumaticNetworkState state)
        {
            System.RestoreState(state);
            ClearDirty();
        }
    }

    public static class GeothermalOrcSaveStore
    {
        public const string FileName = "geothermal_orc_save.json";
        public const string SectionName = "geothermal_orc";
        private static readonly SaveStore<GeothermalOrcState> Store =
            SaveStoreHub.Checksummed<GeothermalOrcState>(FileName, nameof(GeothermalOrcSaveStore));
        public static GeothermalOrcState? TryLoad() => Store.TryLoad();
        public static string TryCapturePersisted(GeothermalOrcState state) => Store.CapturePersisted(state);
    }

    public static class BallisticsWorkbenchSaveStore
    {
        public const string FileName = "ballistics_workbench_save.json";
        public const string SectionName = "ballistics_workbench";
        private static readonly SaveStore<BallisticsWorkbenchState> Store =
            SaveStoreHub.Checksummed<BallisticsWorkbenchState>(FileName, nameof(BallisticsWorkbenchSaveStore));
        public static BallisticsWorkbenchState? TryLoad() => Store.TryLoad();
        public static string TryCapturePersisted(BallisticsWorkbenchState state) => Store.CapturePersisted(state);
    }

    public static class AeroponicsSaveStore
    {
        public const string FileName = "aeroponics_save.json";
        public const string SectionName = "aeroponics";
        private static readonly SaveStore<AeroponicsState> Store =
            SaveStoreHub.Checksummed<AeroponicsState>(FileName, nameof(AeroponicsSaveStore));
        public static AeroponicsState? TryLoad() => Store.TryLoad();
        public static string TryCapturePersisted(AeroponicsState state) => Store.CapturePersisted(state);
    }

    public static class PneumaticDispatchSaveStore
    {
        public const string FileName = "pneumatic_dispatch_save.json";
        public const string SectionName = "pneumatic_dispatch";
        private static readonly SaveStore<PneumaticNetworkState> Store =
            SaveStoreHub.Checksummed<PneumaticNetworkState>(FileName, nameof(PneumaticDispatchSaveStore));
        public static PneumaticNetworkState? TryLoad() => Store.TryLoad();
        public static string TryCapturePersisted(PneumaticNetworkState state) => Store.CapturePersisted(state);
    }
}
