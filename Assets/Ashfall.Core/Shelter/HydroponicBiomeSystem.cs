// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class HydroponicRackState
    {
        public string rackId = string.Empty;
        public string cropId = string.Empty;
        public int growthPermille; // 0..1000
        public string brinePHBand = "Optimal"; // Acidic, Optimal, Alkaline
        public int brinePPM = 800;
        public string ledSpectrum = "Growth_Blue"; // Growth_Blue, Flowering_Red, Hardening_Infrared
        public float contaminationLevel; // 0..100
        public bool isPowered = true;
        public float rootHealth = 100.0f; // 0..100
        public string assignedWorkerId = string.Empty;
        public int lastTickDay;
        public List<string> activeTraits = new List<string>();

        public HydroponicRackState Clone()
        {
            return new HydroponicRackState
            {
                rackId = rackId,
                cropId = cropId,
                growthPermille = growthPermille,
                brinePHBand = brinePHBand,
                brinePPM = brinePPM,
                ledSpectrum = ledSpectrum,
                contaminationLevel = contaminationLevel,
                isPowered = isPowered,
                rootHealth = rootHealth,
                assignedWorkerId = assignedWorkerId,
                lastTickDay = lastTickDay,
                activeTraits = new List<string>(activeTraits)
            };
        }
    }

    [Serializable]
    public sealed class HydroponicBiomeSave
    {
        public List<HydroponicRackState> racks = new List<HydroponicRackState>();
        public float nutrientTankReserve;
        public Dictionary<string, int> seedVaultInventory = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public List<string> unlockedStabilizedTraits = new List<string>();
        public float maintenanceState = 100.0f;
        public int lastTickDay;

        public HydroponicBiomeSave Clone()
        {
            return new HydroponicBiomeSave
            {
                racks = racks.Select(r => r.Clone()).ToList(),
                nutrientTankReserve = nutrientTankReserve,
                seedVaultInventory = new Dictionary<string, int>(seedVaultInventory, StringComparer.OrdinalIgnoreCase),
                unlockedStabilizedTraits = new List<string>(unlockedStabilizedTraits),
                maintenanceState = maintenanceState,
                lastTickDay = lastTickDay
            };
        }
    }

    public sealed class HydroponicBiomeSystem
    {
        public const float PowerDrawPerRackWatts = 450.0f;
        public const float RootHealthMax = 100.0f;
        public const int MaturePermille = 1000;

        private readonly Inventory.Inventory _inventory;
        private readonly HydroponicCropCatalog _catalog;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private readonly Func<bool>? _isGridPowered;
        private readonly Func<float, bool>? _waterConsume;

        private readonly List<HydroponicRackState> _racks = new List<HydroponicRackState>();
        private float _nutrientTankReserve;
        private readonly Dictionary<string, int> _seedVaultInventory = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _unlockedStabilizedTraits = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private float _maintenanceState = 100.0f;
        private int _lastTickDay;

        public static readonly string[] MutationTraitPool = new[]
        {
            "Trait_Drought_Resistant",
            "Trait_Double_Harvest",
            "Trait_Cold_Hardy",
            "Trait_Stunted",
            "Trait_Brittle_Roots"
        };

        public IReadOnlyList<HydroponicRackState> Racks => _racks;
        public float NutrientTankReserve => _nutrientTankReserve;
        public IReadOnlyDictionary<string, int> SeedVaultInventory => _seedVaultInventory;
        public IReadOnlyCollection<string> UnlockedStabilizedTraits => _unlockedStabilizedTraits;
        public float MaintenanceState => _maintenanceState;
        public int LastTickDay => _lastTickDay;

        public event Action<string, string>? OnCropPlanted;
        public event Action<string, string, int>? OnCropHarvested;
        public event Action<string, string>? OnCropMutated;
        public event Action<string>? OnCropDied;

        public HydroponicBiomeSystem(
            Inventory.Inventory inventory,
            HydroponicCropCatalog catalog,
            ISeededRng rng,
            ILog? log = null,
            Func<bool>? isGridPowered = null,
            Func<float, bool>? waterConsume = null,
            int initialRackCount = 4)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _isGridPowered = isGridPowered;
            _waterConsume = waterConsume;

            for (int i = 1; i <= initialRackCount; i++)
            {
                _racks.Add(new HydroponicRackState
                {
                    rackId = $"rack_{i:D2}",
                    cropId = string.Empty,
                    growthPermille = 0,
                    brinePHBand = "Optimal",
                    brinePPM = 800,
                    ledSpectrum = "Growth_Blue",
                    contaminationLevel = 0.0f,
                    isPowered = true,
                    rootHealth = RootHealthMax,
                    assignedWorkerId = string.Empty,
                    lastTickDay = 0
                });
            }
        }

        public HydroponicRackState? GetRack(string rackId)
        {
            if (string.IsNullOrEmpty(rackId)) return null;
            return _racks.FirstOrDefault(r => string.Equals(r.rackId, rackId, StringComparison.OrdinalIgnoreCase));
        }

        public void AddSeed(string cropId, int count = 1)
        {
            if (string.IsNullOrEmpty(cropId) || count <= 0) return;
            _seedVaultInventory.TryGetValue(cropId, out int current);
            _seedVaultInventory[cropId] = current + count;
        }

        public bool TryMixNutrientBatch(int batches = 1)
        {
            if (batches <= 0) return false;

            int waterCost = batches * 2;
            int chemCost = batches * 1;

            var bill = new InventoryBill();
            bill.AddCost("clean_water", waterCost);
            bill.AddCost("scrap_chemical", chemCost);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                _nutrientTankReserve += batches * 10.0f;
            });

            if (committed)
            {
                _log.Info($"[Hydroponics] Mixed {batches} nutrient batch(es). Tank reserve: {_nutrientTankReserve:F1} units.");
            }
            return committed;
        }

        public bool TryPlantCrop(string rackId, string cropId, string workerId = "")
        {
            var rack = GetRack(rackId);
            if (rack == null) return false;
            if (!string.IsNullOrEmpty(rack.cropId)) return false; // Rack occupied

            var def = _catalog.GetCrop(cropId);
            if (def == null) return false;

            // Deduct seed from vault or from inventory
            if (_seedVaultInventory.TryGetValue(cropId, out int seedCount) && seedCount > 0)
            {
                _seedVaultInventory[cropId] = seedCount - 1;
            }
            else
            {
                var bill = new InventoryBill();
                bill.AddCost(cropId, 1);
                if (!_inventory.TryExecuteTransaction(bill))
                    return false;
            }

            rack.cropId = cropId;
            rack.growthPermille = 0;
            rack.rootHealth = RootHealthMax;
            rack.contaminationLevel = 0.0f;
            rack.assignedWorkerId = workerId ?? string.Empty;
            rack.activeTraits.Clear();

            // Apply any already-unlocked stabilized traits
            foreach (var trait in _unlockedStabilizedTraits)
            {
                if (!rack.activeTraits.Contains(trait))
                    rack.activeTraits.Add(trait);
            }

            OnCropPlanted?.Invoke(rack.rackId, cropId);
            return true;
        }

        public bool SetLedSpectrum(string rackId, string spectrum)
        {
            var rack = GetRack(rackId);
            if (rack == null) return false;
            if (spectrum != "Growth_Blue" && spectrum != "Flowering_Red" && spectrum != "Hardening_Infrared")
                return false;

            rack.ledSpectrum = spectrum;
            return true;
        }

        public bool SetBrinePH(string rackId, string phBand)
        {
            var rack = GetRack(rackId);
            if (rack == null) return false;
            if (phBand != "Acidic" && phBand != "Optimal" && phBand != "Alkaline")
                return false;

            rack.brinePHBand = phBand;
            return true;
        }

        public bool TryStabilizeTrait(string traitId)
        {
            if (string.IsNullOrWhiteSpace(traitId)) return false;
            if (_unlockedStabilizedTraits.Contains(traitId)) return true; // Already stabilized

            var bill = new InventoryBill();
            bill.AddCost("scrap_chemical", 2);
            bill.AddCost("clean_water", 1);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                _unlockedStabilizedTraits.Add(traitId);
            });

            return committed;
        }

        public bool TryHarvest(string rackId)
        {
            var rack = GetRack(rackId);
            if (rack == null || string.IsNullOrEmpty(rack.cropId)) return false;
            if (rack.growthPermille < MaturePermille) return false; // Early harvest prevented

            var def = _catalog.GetCrop(rack.cropId);
            if (def == null) return false;

            int yieldQty = def.baseYieldQuantity;
            if (rack.ledSpectrum == "Flowering_Red")
                yieldQty += 1;
            if (rack.activeTraits.Contains("Trait_Double_Harvest"))
                yieldQty *= 2;
            if (rack.activeTraits.Contains("Trait_Stunted"))
                yieldQty = Math.Max(1, yieldQty / 2);

            var bill = new InventoryBill();
            bill.AddGrant(def.baseYieldItemId, yieldQty);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                // Returned seed to vault
                AddSeed(def.id, 1);

                string harvestedCrop = rack.cropId;
                rack.cropId = string.Empty;
                rack.growthPermille = 0;
                rack.activeTraits.Clear();

                OnCropHarvested?.Invoke(rack.rackId, harvestedCrop, yieldQty);
            });

            return committed;
        }

        public void TickDay(int currentDay, float ambientRadiation = 0f, bool? gridPoweredOverride = null)
        {
            _lastTickDay = currentDay;
            bool gridPowered = gridPoweredOverride ?? (_isGridPowered?.Invoke() ?? true);

            foreach (var rack in _racks)
            {
                rack.lastTickDay = currentDay;
                if (string.IsNullOrEmpty(rack.cropId))
                {
                    rack.isPowered = gridPowered;
                    continue;
                }

                var def = _catalog.GetCrop(rack.cropId);
                if (def == null) continue;

                // 1. Resolve power
                rack.isPowered = gridPowered;

                // 2. Resolve water & nutrient availability
                bool waterAvailable = _waterConsume?.Invoke(def.waterLitresPerDay) ?? true;
                bool nutrientsAvailable = _nutrientTankReserve >= def.nutrientUnitsPerDay;
                if (nutrientsAvailable)
                {
                    _nutrientTankReserve -= def.nutrientUnitsPerDay;
                }

                // 3. Update root health
                if (!rack.isPowered)
                {
                    rack.rootHealth -= 15.0f;
                }
                if (!waterAvailable || !nutrientsAvailable)
                {
                    rack.rootHealth -= 20.0f;
                }
                if (rack.brinePHBand != "Optimal")
                {
                    rack.rootHealth -= 5.0f;
                }

                rack.rootHealth = Math.Clamp(rack.rootHealth, 0f, RootHealthMax);
                if (rack.rootHealth <= 0f)
                {
                    string deadCrop = rack.cropId;
                    rack.cropId = string.Empty;
                    rack.growthPermille = 0;
                    rack.activeTraits.Clear();
                    OnCropDied?.Invoke(rack.rackId);
                    continue;
                }

                // 4. Advance growth
                if (rack.isPowered && waterAvailable && nutrientsAvailable && rack.rootHealth > 20.0f)
                {
                    int baseDaily = Math.Max(1, MaturePermille / def.growthTicks);
                    if (rack.ledSpectrum == "Growth_Blue")
                        baseDaily = (int)(baseDaily * 1.25f);
                    if (rack.activeTraits.Contains("Trait_Cold_Hardy"))
                        baseDaily = (int)(baseDaily * 1.10f);

                    rack.growthPermille = Math.Min(MaturePermille, rack.growthPermille + baseDaily);
                }

                // 5. Update contamination
                if (ambientRadiation > 20.0f)
                {
                    float addedContam = (ambientRadiation - 20.0f) * 0.1f;
                    if (rack.ledSpectrum == "Hardening_Infrared")
                        addedContam *= 0.5f;
                    rack.contaminationLevel = Math.Clamp(rack.contaminationLevel + addedContam, 0f, 100.0f);
                }

                // 6. Evaluate mutation if eligible
                if (ambientRadiation > 40.0f && def.mutationAffinity > 0f)
                {
                    double roll = _rng.NextDouble();
                    if (roll < (def.mutationAffinity * 0.5f))
                    {
                        int traitIdx = (int)(_rng.NextDouble() * MutationTraitPool.Length);
                        traitIdx = Math.Clamp(traitIdx, 0, MutationTraitPool.Length - 1);
                        string trait = MutationTraitPool[traitIdx];
                        if (!rack.activeTraits.Contains(trait))
                        {
                            rack.activeTraits.Add(trait);
                            OnCropMutated?.Invoke(rack.rackId, trait);
                        }
                    }
                }
            }
        }

        public HydroponicBiomeSave CaptureState()
        {
            var save = new HydroponicBiomeSave
            {
                nutrientTankReserve = _nutrientTankReserve,
                maintenanceState = _maintenanceState,
                lastTickDay = _lastTickDay,
                seedVaultInventory = new Dictionary<string, int>(_seedVaultInventory, StringComparer.OrdinalIgnoreCase),
                unlockedStabilizedTraits = new List<string>(_unlockedStabilizedTraits),
                racks = _racks.Select(r => r.Clone()).ToList()
            };
            return save;
        }

        public void RestoreState(HydroponicBiomeSave? save)
        {
            if (save == null) return;

            _nutrientTankReserve = save.nutrientTankReserve;
            _maintenanceState = save.maintenanceState;
            _lastTickDay = save.lastTickDay;

            _seedVaultInventory.Clear();
            if (save.seedVaultInventory != null)
            {
                foreach (var kv in save.seedVaultInventory)
                    _seedVaultInventory[kv.Key] = kv.Value;
            }

            _unlockedStabilizedTraits.Clear();
            if (save.unlockedStabilizedTraits != null)
            {
                foreach (var t in save.unlockedStabilizedTraits)
                    _unlockedStabilizedTraits.Add(t);
            }

            _racks.Clear();
            if (save.racks != null)
            {
                foreach (var r in save.racks)
                    _racks.Add(r.Clone());
            }
        }
    }
}
