// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Crafting
{
    [Serializable]
    public sealed class ChemicalRetortState
    {
        public string vesselId = string.Empty;
        public string activeProcessId = string.Empty;
        public int processProgress;
        public int processingTicksRequired;
        public string heatBand = "Nominal"; // Low, Nominal, High, Runaway
        public string pressureBand = "Nominal"; // Vacuum, Nominal, Elevated, Critical
        public float catalystCondition = 100.0f; // 0..100
        public float scrubberCondition = 100.0f; // 0..100
        public bool isSealed = true;
        public string assignedOperatorId = string.Empty;
        public string failureState = "None"; // None, BatchLoss, VesselDamage, ScrubberFailure, ExposureEvent
        public int lastTickDay;

        public ChemicalRetortState Clone()
        {
            return new ChemicalRetortState
            {
                vesselId = vesselId,
                activeProcessId = activeProcessId,
                processProgress = processProgress,
                processingTicksRequired = processingTicksRequired,
                heatBand = heatBand,
                pressureBand = pressureBand,
                catalystCondition = catalystCondition,
                scrubberCondition = scrubberCondition,
                isSealed = isSealed,
                assignedOperatorId = assignedOperatorId,
                failureState = failureState,
                lastTickDay = lastTickDay
            };
        }
    }

    [Serializable]
    public sealed class ChemicalSynthesisSave
    {
        public List<ChemicalRetortState> vessels = new List<ChemicalRetortState>();
        public float scrubberReserve = 100.0f;
        public int apparatusTier = 1;
        public int lastTickDay;

        public ChemicalSynthesisSave Clone()
        {
            return new ChemicalSynthesisSave
            {
                vessels = vessels.Select(v => v.Clone()).ToList(),
                scrubberReserve = scrubberReserve,
                apparatusTier = apparatusTier,
                lastTickDay = lastTickDay
            };
        }
    }

    public sealed class ChemicalSynthesisSystem
    {
        private readonly Inventory.Inventory _inventory;
        private readonly ChemicalSynthesisCatalog _catalog;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private readonly List<ChemicalRetortState> _vessels = new List<ChemicalRetortState>();
        private float _scrubberReserve = 100.0f;
        private int _apparatusTier = 1;
        private int _lastTickDay;

        public IReadOnlyList<ChemicalRetortState> Vessels => _vessels;
        public float ScrubberReserve => _scrubberReserve;
        public int ApparatusTier => _apparatusTier;
        public int LastTickDay => _lastTickDay;

        public event Action<string, string>? OnProcessStarted;
        public event Action<string, string>? OnProcessCompleted;
        public event Action<string, string, string>? OnProcessFailed;
        public event Action<string, string, float>? OnExposureIncident; // vesselId, operatorId, severity
        public event Action? OnStateChanged;

        public ChemicalSynthesisSystem(
            Inventory.Inventory inventory,
            ChemicalSynthesisCatalog catalog,
            ISeededRng rng,
            ILog? log = null,
            int initialVesselCount = 2,
            int startingTier = 1)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _apparatusTier = Math.Clamp(startingTier, 1, 3);

            for (int i = 1; i <= initialVesselCount; i++)
            {
                _vessels.Add(new ChemicalRetortState
                {
                    vesselId = $"retort_{i:D2}",
                    activeProcessId = string.Empty,
                    processProgress = 0,
                    processingTicksRequired = 0,
                    heatBand = "Nominal",
                    pressureBand = "Nominal",
                    catalystCondition = 100.0f,
                    scrubberCondition = 100.0f,
                    isSealed = true,
                    assignedOperatorId = string.Empty,
                    failureState = "None",
                    lastTickDay = 0
                });
            }
        }

        public ChemicalRetortState? GetVessel(string vesselId)
        {
            if (string.IsNullOrEmpty(vesselId)) return null;
            return _vessels.FirstOrDefault(v => string.Equals(v.vesselId, vesselId, StringComparison.OrdinalIgnoreCase));
        }

        public bool TryUpgradeApparatus(int targetTier)
        {
            if (targetTier <= _apparatusTier || targetTier > 3) return false;

            int metalCost = targetTier * 4;
            int wireCost = targetTier * 2;

            var bill = new InventoryBill();
            bill.AddCost("scrap_metal", metalCost);
            bill.AddCost("copper_wire_10m_of_10m", wireCost);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                _apparatusTier = targetTier;
            });

            if (committed)
            {
                _log.Info($"[ChemicalSynthesis] Upgraded apparatus to Tier {_apparatusTier}.");
                OnStateChanged?.Invoke();
            }
            return committed;
        }

        public bool TryStartProcess(string processId, string vesselId, string operatorId = "")
        {
            var vessel = GetVessel(vesselId);
            if (vessel == null) return false;
            if (!string.IsNullOrEmpty(vessel.activeProcessId)) return false; // Vessel busy

            var def = _catalog.GetProcess(processId);
            if (def == null) return false;

            if (_apparatusTier < def.requiredApparatusTier) return false; // Apparatus tier insufficient

            // Build atomic bill for input items
            var bill = new InventoryBill();
            foreach (var kv in def.inputItems)
            {
                bill.AddCost(kv.Key, kv.Value);
            }

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                vessel.activeProcessId = processId;
                vessel.processProgress = 0;
                vessel.processingTicksRequired = def.processingTicks;
                vessel.heatBand = def.heatBand;
                vessel.pressureBand = "Nominal";
                vessel.assignedOperatorId = operatorId ?? string.Empty;
                vessel.failureState = "None";
                OnProcessStarted?.Invoke(vessel.vesselId, processId);
                OnStateChanged?.Invoke();
            });

            return committed;
        }

        public bool TryHarvestOutput(string vesselId)
        {
            var vessel = GetVessel(vesselId);
            if (vessel == null || string.IsNullOrEmpty(vessel.activeProcessId)) return false;
            if (vessel.processProgress < vessel.processingTicksRequired) return false;
            if (vessel.failureState != "None") return false;

            var def = _catalog.GetProcess(vessel.activeProcessId);
            if (def == null) return false;

            var bill = new InventoryBill();
            foreach (var kv in def.outputItems)
            {
                bill.AddGrant(kv.Key, kv.Value);
            }

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                string finished = vessel.activeProcessId;
                vessel.activeProcessId = string.Empty;
                vessel.processProgress = 0;
                vessel.processingTicksRequired = 0;
                vessel.failureState = "None";
                OnProcessCompleted?.Invoke(vessel.vesselId, finished);
                OnStateChanged?.Invoke();
            });

            return committed;
        }

        public bool TryServiceScrubber(string vesselId)
        {
            var vessel = GetVessel(vesselId);
            if (vessel == null) return false;

            var bill = new InventoryBill();
            bill.AddCost("scrap_chemical", 2);
            bill.AddCost("clean_water", 1);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                vessel.scrubberCondition = 100.0f;
                OnStateChanged?.Invoke();
            });

            return committed;
        }

        public bool TryPurgeVessel(string vesselId)
        {
            var vessel = GetVessel(vesselId);
            if (vessel == null || string.IsNullOrEmpty(vessel.activeProcessId)) return false;

            string cancelled = vessel.activeProcessId;
            vessel.activeProcessId = string.Empty;
            vessel.processProgress = 0;
            vessel.processingTicksRequired = 0;
            vessel.failureState = "None";
            OnProcessFailed?.Invoke(vessel.vesselId, cancelled, "Purged");
            OnStateChanged?.Invoke();
            return true;
        }

        public void TickDay(int currentDay)
        {
            _lastTickDay = currentDay;

            foreach (var vessel in _vessels)
            {
                vessel.lastTickDay = currentDay;
                if (string.IsNullOrEmpty(vessel.activeProcessId) || vessel.failureState != "None")
                    continue;

                var def = _catalog.GetProcess(vessel.activeProcessId);
                if (def == null) continue;

                // Advance progress
                vessel.processProgress++;

                // Degrade scrubber and catalyst
                vessel.scrubberCondition = Math.Max(0f, vessel.scrubberCondition - (def.scrubberDemand * 5.0f));
                vessel.catalystCondition = Math.Max(0f, vessel.catalystCondition - def.equipmentWear);

                // Hazard & volatility evaluation
                if (vessel.scrubberCondition < 30.0f && def.volatilityRating > 0f)
                {
                    double roll = _rng.NextDouble();
                    if (roll < (def.volatilityRating * 0.35f))
                    {
                        // Failure event abstraction
                        if (vessel.scrubberCondition <= 0f)
                        {
                            vessel.failureState = "ScrubberFailure";
                            OnExposureIncident?.Invoke(vessel.vesselId, vessel.assignedOperatorId, def.volatilityRating);
                        }
                        else
                        {
                            vessel.failureState = "BatchLoss";
                        }
                        OnProcessFailed?.Invoke(vessel.vesselId, def.id, vessel.failureState);
                    }
                }
            }

            OnStateChanged?.Invoke();
        }

        public ChemicalSynthesisSave CaptureState()
        {
            return new ChemicalSynthesisSave
            {
                vessels = _vessels.Select(v => v.Clone()).ToList(),
                scrubberReserve = _scrubberReserve,
                apparatusTier = _apparatusTier,
                lastTickDay = _lastTickDay
            };
        }

        public void RestoreState(ChemicalSynthesisSave? save)
        {
            if (save == null) return;

            _scrubberReserve = save.scrubberReserve;
            _apparatusTier = save.apparatusTier;
            _lastTickDay = save.lastTickDay;

            _vessels.Clear();
            if (save.vessels != null)
            {
                foreach (var v in save.vessels)
                    _vessels.Add(v.Clone());
            }
        }
    }
}
