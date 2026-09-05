using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    public enum ChlorAlkaliProcessStatus
    {
        Offline,
        Ready,
        Charging,
        Processing,
        Finishing,
        BatchReady,
        MaintenanceRequired,
        VentilationFault,
        ContainmentFault,
        EmergencyShutdown
    }

    public enum ChlorAlkaliHazardState
    {
        Safe,
        Elevated,
        LeakDetected,
        Critical
    }

    [Serializable]
    public sealed class ChlorAlkaliFeedstockCost
    {
        public string item_id = string.Empty;
        public int amount;
    }

    [Serializable]
    public sealed class ChlorAlkaliProcessDef
    {
        public string process_id = string.Empty;
        public string display_name = string.Empty;
        public List<ChlorAlkaliFeedstockCost> feedstock_costs = new List<ChlorAlkaliFeedstockCost>();
        public float power_kw;
        public int duration_ticks = 4;
        public float process_efficiency = 0.7f;
        public int sanitation_output_units = 8;
        public int caustic_output_units = 4;
        public float byproduct_hazard_load = 0.25f;
        public float membrane_wear = 0.04f;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class ChlorAlkaliCatalog
    {
        public int schema_version = 1;
        public List<ChlorAlkaliProcessDef> processes = new List<ChlorAlkaliProcessDef>();
    }

    [Serializable]
    public sealed class ChlorAlkaliPlantState
    {
        public string plantId = "chlor_alkali_plant_01";
        public ChlorAlkaliProcessStatus status = ChlorAlkaliProcessStatus.Ready;
        public string? activeProcessId;
        public float feedstockCharge;
        public float membraneHealth = 1.0f;
        public float processProgress;
        public float hazardLoad;
        public ChlorAlkaliHazardState hazardState = ChlorAlkaliHazardState.Safe;
        public bool scrubberOperational = true;
        public bool ventilationOperational = true;
        public int lastProcessedTick = -1;
        public int completedBatches;
        public int totalSanitationProduced;
        public int totalCausticProduced;
        public bool installed = true;
    }

    public static class ChlorAlkaliSynthesisCatalogLoader
    {
        public const string DefaultFileName = "chlor_alkali_synthesis_catalog.json";

        public static ChlorAlkaliCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null)
        {
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                log?.Warn($"[ChlorAlkali] catalog not found at {path}");
                return new ChlorAlkaliCatalog();
            }

            try
            {
                string text = fileIO.ReadAllText(path);
                var cat = json.Deserialize<ChlorAlkaliCatalog>(text);
                return cat ?? new ChlorAlkaliCatalog();
            }
            catch (Exception ex)
            {
                log?.Error($"[ChlorAlkali] failed loading catalog: {ex.Message}");
                return new ChlorAlkaliCatalog();
            }
        }
    }

    public sealed class ChlorAlkaliSynthesisEngine
    {
        public const string SystemId = "chlor_alkali_synthesis";
        public const string ItemBleach = "item_liquid_bleach_carboy";
        public const string ItemCausticSoda = "item_caustic_soda_flakes";
        public const string ItemRockSalt = "item_rock_salt_sack";
        public const string ItemAnode = "item_industrial_cell_anode";

        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly Func<float>? _netWattsAvailable;
        private readonly ILog? _log;

        private ChlorAlkaliCatalog _catalog = new ChlorAlkaliCatalog();
        private ChlorAlkaliPlantState _state = new ChlorAlkaliPlantState();

        public event Action<ChlorAlkaliPlantState>? OnProcessStateChanged;
        public event Action<string, int, int>? OnBatchCompleted; // processId, bleachCount, causticCount
        public event Action<string, string>? OnPlantFault; // faultType, message

        public ChlorAlkaliPlantState State => _state;
        public ChlorAlkaliCatalog Catalog => _catalog;

        public ChlorAlkaliSynthesisEngine(
            Inventory.Inventory inventory,
            ISeededRng? rng = null,
            Func<float>? netWattsAvailable = null,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? new SeededRng(110);
            _netWattsAvailable = netWattsAvailable;
            _log = log;
        }

        public void LoadCatalog(ChlorAlkaliCatalog catalog)
        {
            _catalog = catalog ?? new ChlorAlkaliCatalog();
        }

        public ChlorAlkaliProcessDef? GetProcess(string processId)
        {
            return _catalog.processes.FirstOrDefault(p => p.process_id == processId);
        }

        public ActionResult StartProcess(string processId)
        {
            if (!_state.installed)
                return ActionResult.Blocked("plant_not_installed", "Chlor-alkali plant is not installed.");

            if (_state.status == ChlorAlkaliProcessStatus.Processing)
                return ActionResult.Blocked("already_processing", "A synthesis batch is already in progress.");

            if (_state.status == ChlorAlkaliProcessStatus.MaintenanceRequired)
                return ActionResult.Blocked("maintenance_required", "Cell membrane is depleted. Maintenance required.");

            if (_state.status is ChlorAlkaliProcessStatus.ContainmentFault or ChlorAlkaliProcessStatus.EmergencyShutdown or ChlorAlkaliProcessStatus.VentilationFault)
                return ActionResult.Blocked("plant_faulted", $"Plant is in fault state: {_state.status}.");

            var def = GetProcess(processId);
            if (def == null)
                return ActionResult.Failed("unknown_process", $"Unknown chlor-alkali process: {processId}");

            // Verify power
            float requiredWatts = def.power_kw * 1000f;
            if (_netWattsAvailable != null && _netWattsAvailable() < requiredWatts)
                return ActionResult.Blocked("insufficient_power", $"Requires {def.power_kw:F1} kW of electrical power.");

            // Check inventory atomically
            foreach (var cost in def.feedstock_costs)
            {
                if (_inventory.CountById(cost.item_id) < cost.amount)
                    return ActionResult.Blocked("insufficient_feedstock", $"Missing feedstock: {cost.item_id} (requires {cost.amount}).");
            }

            // Consume feedstocks atomically
            foreach (var cost in def.feedstock_costs)
            {
                _inventory.TryConsumeById(cost.item_id, cost.amount);
            }

            _state.activeProcessId = processId;
            _state.processProgress = 0f;
            _state.status = ChlorAlkaliProcessStatus.Processing;
            _log?.Info($"[ChlorAlkali] Started process {processId}.");
            OnProcessStateChanged?.Invoke(_state);
            return ActionResult.Success("process_started");
        }

        public ActionResult TickProcess(int currentTick)
        {
            _state.lastProcessedTick = currentTick;

            if (_state.status != ChlorAlkaliProcessStatus.Processing || string.IsNullOrEmpty(_state.activeProcessId))
                return ActionResult.Success("idle");

            var def = GetProcess(_state.activeProcessId);
            if (def == null)
            {
                _state.status = ChlorAlkaliProcessStatus.Ready;
                _state.activeProcessId = null;
                return ActionResult.Failed("invalid_process_state", "chloralkali.invalid_process_state");
            }

            // Power check during processing
            float requiredWatts = def.power_kw * 1000f;
            if (_netWattsAvailable != null && _netWattsAvailable() < requiredWatts)
            {
                _state.status = ChlorAlkaliProcessStatus.EmergencyShutdown;
                OnPlantFault?.Invoke("power_loss", "Electrolytic process shut down due to grid brownout.");
                OnProcessStateChanged?.Invoke(_state);
                return ActionResult.Blocked("power_loss", "Process interrupted by power loss.");
            }

            // Ventilation safety check
            if (!_state.ventilationOperational)
            {
                _state.hazardLoad = Math.Clamp(_state.hazardLoad + 0.35f, 0f, 1f);
                if (_state.hazardLoad > 0.4f)
                {
                    _state.status = ChlorAlkaliProcessStatus.VentilationFault;
                    _state.hazardState = ChlorAlkaliHazardState.Critical;
                    OnPlantFault?.Invoke("ventilation_failure", "Process ventilation offline. Gas accumulation detected.");
                    OnProcessStateChanged?.Invoke(_state);
                    return ActionResult.Blocked("ventilation_failure", "Ventilation failure during electrolysis.");
                }
            }

            _state.processProgress += 1f;

            // Check completion
            if (_state.processProgress >= def.duration_ticks)
            {
                return CompleteBatch(def);
            }

            OnProcessStateChanged?.Invoke(_state);
            return ActionResult.Success("progressed");
        }

        public void TickDay(int day)
        {
            TickProcess(day);
        }

        private ActionResult CompleteBatch(ChlorAlkaliProcessDef def)
        {
            float healthMultiplier = Math.Clamp(_state.membraneHealth, 0.4f, 1.0f);
            int bleachUnits = Math.Max(1, (int)Math.Round(def.sanitation_output_units * def.process_efficiency * healthMultiplier));
            int causticUnits = Math.Max(1, (int)Math.Round(def.caustic_output_units * def.process_efficiency));

            _inventory.AddById(ItemBleach, bleachUnits);
            _inventory.AddById(ItemCausticSoda, causticUnits);

            _state.membraneHealth = Math.Max(0f, _state.membraneHealth - def.membrane_wear);
            float hazardIncrease = def.byproduct_hazard_load * (_state.scrubberOperational ? 0.3f : 1.0f);
            _state.hazardLoad = Math.Clamp(_state.hazardLoad + hazardIncrease, 0f, 1f);

            UpdateHazardState();

            _state.completedBatches++;
            _state.totalSanitationProduced += bleachUnits;
            _state.totalCausticProduced += causticUnits;
            _state.activeProcessId = null;
            _state.processProgress = 0f;

            if (_state.membraneHealth <= 0.15f)
            {
                _state.status = ChlorAlkaliProcessStatus.MaintenanceRequired;
                OnPlantFault?.Invoke("membrane_depleted", "Electrolytic diaphragm requires replacement anode and service.");
            }
            else
            {
                _state.status = ChlorAlkaliProcessStatus.Ready;
            }

            _log?.Info($"[ChlorAlkali] Completed batch {def.process_id}: granted {bleachUnits} bleach, {causticUnits} caustic soda.");
            OnBatchCompleted?.Invoke(def.process_id, bleachUnits, causticUnits);
            OnProcessStateChanged?.Invoke(_state);
            return ActionResult.Success("batch_completed");
        }

        private void UpdateHazardState()
        {
            if (_state.hazardLoad >= 0.75f)
            {
                _state.hazardState = ChlorAlkaliHazardState.Critical;
                _state.status = ChlorAlkaliProcessStatus.ContainmentFault;
                OnPlantFault?.Invoke("containment_leak", "Critical byproduct accumulation. Chemical containment compromised.");
            }
            else if (_state.hazardLoad >= 0.50f)
            {
                _state.hazardState = ChlorAlkaliHazardState.LeakDetected;
            }
            else if (_state.hazardLoad >= 0.25f)
            {
                _state.hazardState = ChlorAlkaliHazardState.Elevated;
            }
            else
            {
                _state.hazardState = ChlorAlkaliHazardState.Safe;
            }
        }

        public ActionResult PerformMaintenance(string maintenanceItem = ItemAnode)
        {
            if (_inventory.CountById(maintenanceItem) < 1)
                return ActionResult.Blocked("missing_maintenance_item", $"Requires 1x {maintenanceItem} for cell refurbishment.");

            _inventory.TryConsumeById(maintenanceItem, 1);
            _state.membraneHealth = 1.0f;
            if (_state.status is ChlorAlkaliProcessStatus.MaintenanceRequired or ChlorAlkaliProcessStatus.ContainmentFault or ChlorAlkaliProcessStatus.EmergencyShutdown)
            {
                _state.status = ChlorAlkaliProcessStatus.Ready;
            }
            _log?.Info("[ChlorAlkali] Cell maintenance performed; membrane health restored to 100%.");
            OnProcessStateChanged?.Invoke(_state);
            return ActionResult.Success("maintenance_complete");
        }

        public ActionResult VentAndScrubHazard()
        {
            _state.hazardLoad = 0f;
            _state.hazardState = ChlorAlkaliHazardState.Safe;
            if (_state.status == ChlorAlkaliProcessStatus.ContainmentFault || _state.status == ChlorAlkaliProcessStatus.VentilationFault)
            {
                _state.status = ChlorAlkaliProcessStatus.Ready;
            }
            _log?.Info("[ChlorAlkali] Scrubbers flushed and plant atmosphere purged.");
            OnProcessStateChanged?.Invoke(_state);
            return ActionResult.Success("scrubbed");
        }

        public void SetScrubberOperational(bool operational)
        {
            _state.scrubberOperational = operational;
            OnProcessStateChanged?.Invoke(_state);
        }

        public void SetVentilationOperational(bool operational)
        {
            _state.ventilationOperational = operational;
            OnProcessStateChanged?.Invoke(_state);
        }

        public ChlorAlkaliPlantState CaptureState()
        {
            return new ChlorAlkaliPlantState
            {
                plantId = _state.plantId,
                status = _state.status,
                activeProcessId = _state.activeProcessId,
                feedstockCharge = _state.feedstockCharge,
                membraneHealth = _state.membraneHealth,
                processProgress = _state.processProgress,
                hazardLoad = _state.hazardLoad,
                hazardState = _state.hazardState,
                scrubberOperational = _state.scrubberOperational,
                ventilationOperational = _state.ventilationOperational,
                lastProcessedTick = _state.lastProcessedTick,
                completedBatches = _state.completedBatches,
                totalSanitationProduced = _state.totalSanitationProduced,
                totalCausticProduced = _state.totalCausticProduced,
                installed = _state.installed
            };
        }

        public void RestoreState(ChlorAlkaliPlantState? state)
        {
            if (state == null) return;
            _state = new ChlorAlkaliPlantState
            {
                plantId = state.plantId,
                status = state.status,
                activeProcessId = state.activeProcessId,
                feedstockCharge = state.feedstockCharge,
                membraneHealth = state.membraneHealth,
                processProgress = state.processProgress,
                hazardLoad = state.hazardLoad,
                hazardState = state.hazardState,
                scrubberOperational = state.scrubberOperational,
                ventilationOperational = state.ventilationOperational,
                lastProcessedTick = state.lastProcessedTick,
                completedBatches = state.completedBatches,
                totalSanitationProduced = state.totalSanitationProduced,
                totalCausticProduced = state.totalCausticProduced,
                installed = state.installed
            };
            OnProcessStateChanged?.Invoke(_state);
        }
    }
}
