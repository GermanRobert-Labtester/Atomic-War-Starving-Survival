// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Excavation
{
    [Serializable]
    public sealed class MitigationItemCost
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public int Amount { get; set; } = 1;
    }

    [Serializable]
    public sealed class ExcavationMitigationDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("hazard_tags")]
        public List<string> HazardTags { get; set; } = new List<string>();

        [JsonPropertyName("required_items")]
        public List<MitigationItemCost> RequiredItems { get; set; } = new List<MitigationItemCost>();

        [JsonPropertyName("labor_ticks")]
        public int LaborTicks { get; set; } = 60;

        [JsonPropertyName("effect")]
        public Dictionary<string, int> Effect { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);

        [JsonPropertyName("requires_respiratory_protection")]
        public bool RequiresRespiratoryProtection { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class ExcavationHazardCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("mitigations")]
        public List<ExcavationMitigationDefinition> Mitigations { get; set; } = new List<ExcavationMitigationDefinition>();
    }

    [Serializable]
    public sealed class ExcavationSectorHazardState
    {
        public string SectorId { get; set; } = string.Empty;
        public int MethanePpm { get; set; } = 500; // 0 - 10000+
        public int FloodLevelPermille { get; set; } = 0; // 0 - 1000
        public int SporeConcentrationPermille { get; set; } = 0; // 0 - 1000
        public int ShoringHealthPermille { get; set; } = 1000; // 0 - 1000
        public bool IsBulkheadSealed { get; set; }
        public List<string> InstalledMitigationIds { get; set; } = new List<string>();
        public List<string> ActiveTrappedMiners { get; set; } = new List<string>();
        public int? RescueDeadlineDay { get; set; }
        public int RescueLaborRemaining { get; set; }
        public bool RescueCompleted { get; set; }
        public bool RescueFailed { get; set; }
    }

    [Serializable]
    public sealed class ExcavationHazardSave
    {
        public string systemId = ExcavationHazardSystem.SystemId;
        public int schemaVersion = 1;
        public Dictionary<string, ExcavationSectorHazardState> sectors = new(StringComparer.Ordinal);
        public int currentDay;
    }

    public sealed class ExcavationHazardSystem
    {
        public const string SystemId = "excavation_hazards";

        private ExcavationHazardSave _state = new ExcavationHazardSave();
        private readonly Dictionary<string, ExcavationMitigationDefinition> _catalog = new(StringComparer.Ordinal);
        private readonly InventoryContainer _inventory;
        private readonly ExcavationSystem? _excavation;
        private readonly SkyLayerArmorSystem? _skyArmor;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public ExcavationHazardSave State => _state;
        public IReadOnlyDictionary<string, ExcavationMitigationDefinition> Catalog => _catalog;

        public event Action<string, string>? OnMitigationInstalled; // sectorId, mitigationId
        public event Action<string>? OnMethaneIgnition; // sectorId
        public event Action<string>? OnSectorFlooded;
        public event Action<string, int>? OnRescueStarted; // sectorId, trappedCount
        public event Action<string>? OnRescueSucceeded;
        public event Action<string>? OnRescueFailed;
        public event Action? OnHazardStateChanged;

        public ExcavationHazardSystem(
            InventoryContainer inventory,
            ISeededRng rng,
            ExcavationSystem? excavation = null,
            SkyLayerArmorSystem? skyArmor = null,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _excavation = excavation;
            _skyArmor = skyArmor;
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(ExcavationHazardCatalogData? data)
        {
            if (data?.Mitigations == null) return;
            _catalog.Clear();
            foreach (var m in data.Mitigations)
            {
                if (!string.IsNullOrEmpty(m.Id))
                    _catalog[m.Id] = m;
            }
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var serializer = new SystemTextJsonSerializer();
            var data = serializer.Deserialize<ExcavationHazardCatalogData>(json);
            LoadCatalog(data);
        }

        public ExcavationSectorHazardState GetOrCreateSector(string sectorId)
        {
            if (string.IsNullOrEmpty(sectorId)) sectorId = "sector_excavation_alpha";
            if (_state.sectors.TryGetValue(sectorId, out var sector))
                return sector;

            var created = new ExcavationSectorHazardState
            {
                SectorId = sectorId,
                MethanePpm = 300,
                FloodLevelPermille = 0,
                SporeConcentrationPermille = 0,
                ShoringHealthPermille = 1000
            };
            _state.sectors[sectorId] = created;
            return created;
        }

        public bool CanApplyMitigation(string sectorId, string mitigationId, out string reason)
        {
            reason = string.Empty;
            if (!_catalog.TryGetValue(mitigationId, out var def))
            {
                reason = "unknown_mitigation";
                return false;
            }

            var sector = GetOrCreateSector(sectorId);
            if (sector.IsBulkheadSealed && def.Id != "mitigation_emergency_bulkhead_seal")
            {
                reason = "sector_bulkhead_sealed";
                return false;
            }

            // Respiratory requirement check
            if (def.RequiresRespiratoryProtection && _inventory.CountById("gas_mask") < 1)
            {
                reason = "missing_gas_mask";
                return false;
            }

            // Item validation
            var bill = BuildBill(def);
            var validation = _inventory.ValidateTransaction(bill);
            if (!validation.IsValid)
            {
                reason = validation.FailureReason;
                return false;
            }

            return true;
        }

        private InventoryBill BuildBill(ExcavationMitigationDefinition def)
        {
            var bill = new InventoryBill();
            foreach (var item in def.RequiredItems)
            {
                if (!string.IsNullOrEmpty(item.ItemId) && item.Amount > 0)
                    bill.AddCost(item.ItemId, item.Amount);
            }
            return bill;
        }

        public ActionResult TryApplyMitigation(string sectorId, string mitigationId, IReadOnlyList<string>? workerIds = null)
        {
            if (!CanApplyMitigation(sectorId, mitigationId, out var reason))
                return ActionResult.Blocked("cannot_apply", reason);

            var def = _catalog[mitigationId];
            var bill = BuildBill(def);

            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("transaction_failed", "insufficient_materials");

            var sector = GetOrCreateSector(sectorId);

            // Apply mitigation effects
            if (def.Effect.TryGetValue("methane_vent_rate_permille", out int methaneVent))
            {
                int reduction = (int)(sector.MethanePpm * (methaneVent / 1000f));
                sector.MethanePpm = Math.Max(0, sector.MethanePpm - reduction);
            }

            if (def.Effect.TryGetValue("flood_drain_rate_permille", out int floodDrain))
            {
                sector.FloodLevelPermille = Math.Max(0, sector.FloodLevelPermille - floodDrain);
            }

            if (def.Effect.TryGetValue("spore_reduction_permille", out int sporeRed))
            {
                sector.SporeConcentrationPermille = Math.Max(0, sector.SporeConcentrationPermille - sporeRed);
            }

            if (def.Effect.TryGetValue("shoring_health_restore_permille", out int shoringGain))
            {
                sector.ShoringHealthPermille = Math.Min(1000, sector.ShoringHealthPermille + shoringGain);
            }

            if (def.Tags.Contains("installed") && !sector.InstalledMitigationIds.Contains(def.Id))
            {
                sector.InstalledMitigationIds.Add(def.Id);
            }

            if (def.Id == "mitigation_trapped_miner_clearance" && sector.ActiveTrappedMiners.Count > 0)
            {
                int clearance = def.Effect.GetValueOrDefault("rubble_clearance_progress_permille", 340);
                ProgressRescueLabor(sectorId, clearance);
            }

            OnMitigationInstalled?.Invoke(sectorId, mitigationId);
            OnHazardStateChanged?.Invoke();
            return ActionResult.Success("excavation.mitigation_applied");
        }

        public ActionResult TryToggleBulkhead(string sectorId, bool seal, out string reason)
        {
            reason = string.Empty;
            var sector = GetOrCreateSector(sectorId);
            if (seal && sector.ActiveTrappedMiners.Count > 0)
            {
                reason = "cannot_seal_trapped_miners";
                return ActionResult.Blocked("trapped_miners", reason);
            }

            sector.IsBulkheadSealed = seal;
            OnHazardStateChanged?.Invoke();
            return ActionResult.Success(seal ? "bulkhead.sealed" : "bulkhead.opened");
        }

        public void TriggerCaveInRescue(
            string sectorId,
            IReadOnlyList<string> trappedSurvivorIds,
            int deadlineDays = 3,
            int requiredLabor = 240)
        {
            var sector = GetOrCreateSector(sectorId);
            sector.ActiveTrappedMiners = new List<string>(trappedSurvivorIds);
            sector.RescueDeadlineDay = _state.currentDay + deadlineDays;
            sector.RescueLaborRemaining = requiredLabor;
            sector.RescueCompleted = false;
            sector.RescueFailed = false;

            OnRescueStarted?.Invoke(sectorId, trappedSurvivorIds.Count);
            OnHazardStateChanged?.Invoke();
        }

        public void ProgressRescueLabor(string sectorId, int laborAmount)
        {
            var sector = GetOrCreateSector(sectorId);
            if (sector.ActiveTrappedMiners.Count == 0 || sector.RescueCompleted || sector.RescueFailed)
                return;

            sector.RescueLaborRemaining = Math.Max(0, sector.RescueLaborRemaining - laborAmount);
            if (sector.RescueLaborRemaining <= 0)
            {
                sector.RescueCompleted = true;
                sector.ActiveTrappedMiners.Clear();
                sector.RescueDeadlineDay = null;
                OnRescueSucceeded?.Invoke(sectorId);
            }
            OnHazardStateChanged?.Invoke();
        }

        public (float collapseRisk, float ignitionRisk, bool respiratoryHazard) EvaluateOperationRisk(string sectorId)
        {
            var sector = GetOrCreateSector(sectorId);
            float baseCollapse = (1000 - sector.ShoringHealthPermille) / 1000f * 0.40f;

            if (sector.FloodLevelPermille > 500) baseCollapse += 0.20f;
            if (sector.InstalledMitigationIds.Contains("mitigation_sky_armor_blast_matting"))
                baseCollapse *= 0.55f;

            float ignitionRisk = 0f;
            if (sector.MethanePpm > 4000)
            {
                ignitionRisk = Math.Clamp((sector.MethanePpm - 4000) / 6000f, 0f, 0.90f);
            }

            bool respHazard = sector.SporeConcentrationPermille > 200 || sector.MethanePpm > 5000;
            return (Math.Clamp(baseCollapse, 0f, 1f), ignitionRisk, respHazard);
        }

        public void TickDay(int day)
        {
            _state.currentDay = day;

            foreach (var sector in _state.sectors.Values)
            {
                if (sector.IsBulkheadSealed) continue;

                // Passive hazard accumulation
                sector.MethanePpm += _rng.Next(50, 150);
                if (sector.InstalledMitigationIds.Contains("mitigation_ventilation_blower_install"))
                {
                    sector.MethanePpm = Math.Max(0, sector.MethanePpm - 200);
                }

                // Passive shoring decay
                sector.ShoringHealthPermille = Math.Max(0, sector.ShoringHealthPermille - _rng.Next(20, 50));

                // Check rescue deadline expiry
                if (sector.ActiveTrappedMiners.Count > 0 && !sector.RescueCompleted && !sector.RescueFailed)
                {
                    if (sector.RescueDeadlineDay.HasValue && sector.RescueDeadlineDay.Value <= day)
                    {
                        sector.RescueFailed = true;
                        OnRescueFailed?.Invoke(sector.SectorId);
                    }
                }
            }

            OnHazardStateChanged?.Invoke();
        }

        public ExcavationHazardSave CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<ExcavationHazardSave>(json) ?? new ExcavationHazardSave();
        }

        public void RestoreState(ExcavationHazardSave? saved)
        {
            if (saved == null)
            {
                _state = new ExcavationHazardSave();
                return;
            }

            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<ExcavationHazardSave>(json) ?? new ExcavationHazardSave();
            OnHazardStateChanged?.Invoke();
        }
    }
}
