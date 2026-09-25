// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Player-facing aggregate over the existing water-source authorities.
    /// It coordinates atomic inventory bills and Core commands; it owns no
    /// water, equipment, monitoring, or save state of its own.
    /// </summary>
    public sealed class WaterSourcesHostSession : IDisposable
    {
        private readonly Inventory _inventory;
        private readonly PowerGridSystem _powerGrid;
        private readonly ResearchSystem _research;
        private bool _disposed;

        public DeepWellHostSession DeepWell { get; }
        public WaterCondenserHostSession Condenser { get; }
        public PiezometerHostSession Piezometer { get; }
        public event Action? StateChanged;

        public string LastEvent { get; private set; } = string.Empty;

        public WaterSourcesHostSession(
            Inventory inventory,
            PowerGridSystem powerGrid,
            ResearchSystem research,
            DeepWellHostSession deepWell,
            WaterCondenserHostSession condenser,
            PiezometerHostSession piezometer)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _powerGrid = powerGrid ?? throw new ArgumentNullException(nameof(powerGrid));
            _research = research ?? throw new ArgumentNullException(nameof(research));
            DeepWell = deepWell ?? throw new ArgumentNullException(nameof(deepWell));
            Condenser = condenser ?? throw new ArgumentNullException(nameof(condenser));
            Piezometer = piezometer ?? throw new ArgumentNullException(nameof(piezometer));

            _inventory.OnInventoryChanged += NotifyStateChanged;
            _research.OnManualUnlocked += OnResearchChanged;
            _research.OnResearchCompleted += OnResearchCompleted;
            _powerGrid.OnPowerChanged += OnPowerChanged;
            DeepWell.System.OnStateChanged += OnDeepWellChanged;
            Condenser.System.OnStateChanged += OnCondenserChanged;
            Piezometer.StateChanged += OnPiezometerChanged;
        }

        public DeepWellState DeepWellState => DeepWell.System.CaptureState();
        public AtmosphericCondenserState CondenserState => Condenser.System.CaptureState();
        public HydrogeologyNetworkState PiezometerState => Piezometer.CaptureSave();

        public bool DeepWellPowerServed =>
            _powerGrid.IsRoomServed(DeepWellSystem.PowerRoomId);
        public bool CondenserPowerServed =>
            _powerGrid.IsRoomServed(AtmosphericCondenserSystem.PowerRoomId);
        public bool HasDeepWellCapability =>
            _research.HasCapability(DeepWellSystem.RequiredKnowledgeId);
        public bool HasCondenserCapability =>
            _research.HasCapability(AtmosphericCondenserSystem.RequiredKnowledgeId);

        public bool CanBuildDeepWell =>
            !DeepWell.System.IsBuilt
            && HasDeepWellCapability
            && HasBill(DeepWellBuildBill);

        public bool CanServiceDeepWell =>
            DeepWell.System.IsBuilt
            && DeepWell.System.Condition < 100f
            && _inventory.CountById(DeepWellSystem.MaintenanceItemId) >= 1;

        public bool CanBuildCondenser =>
            !Condenser.System.IsBuilt
            && HasCondenserCapability
            && HasBill(CondenserBuildBill);

        public bool CanReplaceCondenserMembrane =>
            Condenser.System.IsBuilt
            && Condenser.System.MembraneIntegrity < 100f
            && _inventory.CountById(AtmosphericCondenserSystem.MembraneItemId) >= 1;

        public bool CanConstructPiezometer =>
            !Piezometer.System.IsConstructed
            && (Piezometer.System.Catalog.strata?.Count ?? 0) > 0
            && HasBill(PiezometerInstallBill());

        public int ItemCount(string itemId) => _inventory.CountById(itemId);

        /// <summary>
        /// Captures the three existing save payloads without writing files or
        /// introducing a fourth persistence owner.
        /// </summary>
        public WaterSourcesPersistedSnapshots CapturePersistedSnapshots() =>
            new WaterSourcesPersistedSnapshots(
                DeepWellSaveStore.TryCapturePersisted(DeepWell.System.CaptureState()),
                WaterCondenserSaveStore.TryCapturePersisted(Condenser.System.CaptureState()),
                PiezometerSaveStore.TryCapturePersisted(Piezometer.CaptureSave()));

        public bool TryBuildDeepWell()
        {
            if (!HasDeepWellCapability)
                return Fail("Deep-well build blocked: required research capability is not available.");
            return ExecuteBill(DeepWellBuildBill, () =>
            {
                if (!DeepWell.TryBuild(true, out var reason))
                    throw new InvalidOperationException("Deep-well build blocked: " + reason);
            }, "Deep-well pump installed.");
        }

        public bool TrySetDeepWellEnabled(bool enabled)
        {
            if (!DeepWell.SetEnabled(enabled, out var reason))
                return Fail("Deep-well command blocked: " + reason);
            LastEvent = enabled ? "Deep-well pump engaged." : "Deep-well pump disengaged.";
            NotifyStateChanged();
            return true;
        }

        public bool TryServiceDeepWell()
        {
            return ExecuteBill(
                new Dictionary<string, int> { [DeepWellSystem.MaintenanceItemId] = 1 },
                () =>
                {
                    if (!DeepWell.PerformMaintenance(out var reason))
                        throw new InvalidOperationException("Deep-well service blocked: " + reason);
                },
                "Deep-well pump serviced.");
        }

        public bool TryBuildCondenser()
        {
            if (!HasCondenserCapability)
                return Fail("Condenser build blocked: required research capability is not available.");
            return ExecuteBill(CondenserBuildBill, () =>
            {
                if (!Condenser.TryBuild(true, out var reason))
                    throw new InvalidOperationException("Condenser build blocked: " + reason);
            }, "Atmospheric condenser installed.");
        }

        public bool TrySetCondenserEnabled(bool enabled)
        {
            if (!Condenser.SetEnabled(enabled, out var reason))
                return Fail("Condenser command blocked: " + reason);
            LastEvent = enabled ? "Condenser array engaged." : "Condenser array disengaged.";
            NotifyStateChanged();
            return true;
        }

        public bool TryReplaceCondenserMembrane()
        {
            return ExecuteBill(
                new Dictionary<string, int> { [AtmosphericCondenserSystem.MembraneItemId] = 1 },
                () =>
                {
                    if (!Condenser.ReplaceMembrane(out var reason))
                        throw new InvalidOperationException("Condenser membrane replacement blocked: " + reason);
                },
                "Condensation membrane replaced.");
        }

        public bool TryConstructPiezometer()
        {
            bool wasConstructed = Piezometer.System.IsConstructed;
            string result = Piezometer.ConstructNetwork(_inventory);
            LastEvent = result;
            NotifyStateChanged();
            return !wasConstructed && Piezometer.System.IsConstructed;
        }

        private bool ExecuteBill(
            IReadOnlyDictionary<string, int> bill,
            Action command,
            string successMessage)
        {
            try
            {
                if (!_inventory.TryConsumeBill(bill, command))
                    return Fail("Action blocked: required materials are unavailable.");
                LastEvent = successMessage;
                NotifyStateChanged();
                return true;
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        private bool Fail(string message)
        {
            LastEvent = message;
            NotifyStateChanged();
            return false;
        }

        private bool HasBill(IReadOnlyDictionary<string, int> bill)
        {
            foreach (var cost in bill)
                if (cost.Value > 0 && _inventory.CountById(cost.Key) < cost.Value)
                    return false;
            return true;
        }

        private static readonly IReadOnlyDictionary<string, int> DeepWellBuildBill =
            new Dictionary<string, int>
            {
                [DeepWellSystem.BuildItemId] = 1,
                ["mechanical_parts"] = 2
            };

        private static readonly IReadOnlyDictionary<string, int> CondenserBuildBill =
            new Dictionary<string, int>
            {
                [AtmosphericCondenserSystem.MembraneItemId] = 1,
                ["metal_pipe"] = 2,
                ["scrap_metal"] = 4
            };

        private IReadOnlyDictionary<string, int> PiezometerInstallBill()
        {
            var totals = new Dictionary<string, int>(StringComparer.Ordinal);
            var strata = Piezometer.System.Catalog.strata;
            if (strata == null) return totals;

            foreach (var zone in strata)
            {
                if (zone?.sensor_install_cost == null) continue;
                foreach (var cost in zone.sensor_install_cost)
                {
                    if (string.IsNullOrWhiteSpace(cost.Key) || cost.Value <= 0) continue;
                    totals[cost.Key] = totals.TryGetValue(cost.Key, out var amount)
                        ? checked(amount + cost.Value)
                        : cost.Value;
                }
            }
            return totals;
        }

        private void OnDeepWellChanged(DeepWellState _) => NotifyStateChanged();
        private void OnCondenserChanged(AtmosphericCondenserState _) => NotifyStateChanged();
        private void OnPiezometerChanged() => NotifyStateChanged();
        private void OnPowerChanged(PowerGridEvent _) => NotifyStateChanged();
        private void OnResearchChanged(string _) => NotifyStateChanged();
        private void OnResearchCompleted(ResearchKnowledgeDef _) => NotifyStateChanged();
        private void NotifyStateChanged() => StateChanged?.Invoke();

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _inventory.OnInventoryChanged -= NotifyStateChanged;
            _research.OnManualUnlocked -= OnResearchChanged;
            _research.OnResearchCompleted -= OnResearchCompleted;
            _powerGrid.OnPowerChanged -= OnPowerChanged;
            DeepWell.System.OnStateChanged -= OnDeepWellChanged;
            Condenser.System.OnStateChanged -= OnCondenserChanged;
            Piezometer.StateChanged -= OnPiezometerChanged;
            StateChanged = null;
        }
    }

    public sealed class WaterSourcesPersistedSnapshots
    {
        public string DeepWell { get; }
        public string Condenser { get; }
        public string Piezometer { get; }

        public WaterSourcesPersistedSnapshots(string deepWell, string condenser, string piezometer)
        {
            DeepWell = deepWell;
            Condenser = condenser;
            Piezometer = piezometer;
        }
    }
}
