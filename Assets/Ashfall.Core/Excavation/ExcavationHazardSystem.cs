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

        /// <summary>Methane concentration (PPM) above which ignition risk begins; shared by the risk evaluator and the exactly-once ignition event.</summary>
        public const int MethaneIgnitionThresholdPpm = 4000;

        /// <summary>Flood level (permille) above which a sector counts as flooded; shared by the risk evaluator and the exactly-once flood event.</summary>
        public const int FloodCriticalThresholdPermille = 500;

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

            // Apply mitigation effects (mutation flows through the canonical
            // AddMethane/AddFloodWater seams so hazard notifications cannot drift).
            if (def.Effect.TryGetValue("methane_vent_rate_permille", out int methaneVent))
            {
                int reduction = (int)(sector.MethanePpm * (methaneVent / 1000f));
                ApplyMethaneDelta(sector, -reduction, int.MaxValue);
            }

            if (def.Effect.TryGetValue("flood_drain_rate_permille", out int floodDrain))
            {
                ApplyFloodDelta(sector, -floodDrain);
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

            if (sector.FloodLevelPermille > FloodCriticalThresholdPermille) baseCollapse += 0.20f;
            if (sector.InstalledMitigationIds.Contains("mitigation_sky_armor_blast_matting"))
                baseCollapse *= 0.55f;

            float ignitionRisk = 0f;
            if (sector.MethanePpm > MethaneIgnitionThresholdPpm)
            {
                ignitionRisk = Math.Clamp((sector.MethanePpm - MethaneIgnitionThresholdPpm) / 6000f, 0f, 0.90f);
            }

            bool respHazard = sector.SporeConcentrationPermille > 200 || sector.MethanePpm > 5000;
            return (Math.Clamp(baseCollapse, 0f, 1f), ignitionRisk, respHazard);
        }

        /// <summary>
        /// Canonical methane mutation. Every producer (daily accumulation,
        /// ventilation, seismic outgassing) routes through here so crossing
        /// <see cref="MethaneIgnitionThresholdPpm"/> raises
        /// <see cref="OnMethaneIgnition"/> exactly once per crossing.
        /// </summary>
        public void AddMethane(string sectorId, int deltaPpm, int cap = int.MaxValue)
        {
            var sector = GetOrCreateSector(sectorId);
            if (ApplyMethaneDelta(sector, deltaPpm, cap))
                OnHazardStateChanged?.Invoke();
        }

        /// <summary>
        /// Canonical flood-level mutation. Crossing
        /// <see cref="FloodCriticalThresholdPermille"/> raises
        /// <see cref="OnSectorFlooded"/> exactly once per crossing. Returns true
        /// when the level actually changed.
        /// </summary>
        public bool AddFloodWater(string sectorId, int deltaPermille)
        {
            var sector = GetOrCreateSector(sectorId);
            bool changed = ApplyFloodDelta(sector, deltaPermille);
            if (changed)
                OnHazardStateChanged?.Invoke();
            return changed;
        }

        private bool ApplyMethaneDelta(ExcavationSectorHazardState sector, int deltaPpm, int cap)
        {
            if (sector == null || deltaPpm == 0) return false;

            long next = (long)sector.MethanePpm + deltaPpm;
            if (next < 0) next = 0;
            if (next > cap) next = cap;
            int after = (int)next;
            if (after == sector.MethanePpm) return false;

            int before = sector.MethanePpm;
            sector.MethanePpm = after;

            if (before <= MethaneIgnitionThresholdPpm && after > MethaneIgnitionThresholdPpm)
            {
                _log.Warn($"[ExcavationHazard] methane ignition threshold crossed in {sector.SectorId} ({after}ppm)");
                OnMethaneIgnition?.Invoke(sector.SectorId);
            }

            return true;
        }

        private bool ApplyFloodDelta(ExcavationSectorHazardState sector, int deltaPermille)
        {
            if (sector == null || deltaPermille == 0) return false;

            int before = sector.FloodLevelPermille;
            int after = Math.Clamp(before + deltaPermille, 0, 1000);
            if (after == before) return false;

            sector.FloodLevelPermille = after;

            if (before <= FloodCriticalThresholdPermille && after > FloodCriticalThresholdPermille)
            {
                _log.Warn($"[ExcavationHazard] sector flooding in {sector.SectorId} ({after} permille)");
                OnSectorFlooded?.Invoke(sector.SectorId);
            }

            return true;
        }

        /// <summary>
        /// Authored daily mitigation upkeep: an installed mitigation keeps
        /// working between applications. The JSON
        /// <c>passive_decay_bonus_permille</c> value applies to the same hazard
        /// its primary effect targets (methane / flood / spores / shoring).
        /// </summary>
        private void ApplyPassiveMitigationDecay(ExcavationSectorHazardState sector)
        {
            if (sector?.InstalledMitigationIds == null) return;
            for (int i = 0; i < sector.InstalledMitigationIds.Count; i++)
            {
                string mitigationId = sector.InstalledMitigationIds[i];
                if (string.IsNullOrEmpty(mitigationId)) continue;
                if (!_catalog.TryGetValue(mitigationId, out var def) || def?.Effect == null) continue;
                if (!def.Effect.TryGetValue("passive_decay_bonus_permille", out int bonus) || bonus <= 0) continue;

                if (def.Effect.ContainsKey("methane_vent_rate_permille"))
                    ApplyMethaneDelta(sector, -bonus, int.MaxValue);
                if (def.Effect.ContainsKey("flood_drain_rate_permille"))
                    ApplyFloodDelta(sector, -bonus);
                if (def.Effect.ContainsKey("spore_reduction_permille"))
                    sector.SporeConcentrationPermille = Math.Max(0, sector.SporeConcentrationPermille - bonus);
                if (def.Effect.ContainsKey("shoring_health_restore_permille"))
                    sector.ShoringHealthPermille = Math.Min(1000, sector.ShoringHealthPermille + bonus);
            }
        }

        public void TickDay(int day)
        {
            _state.currentDay = day;

            foreach (var sector in _state.sectors.Values)
            {
                if (sector.IsBulkheadSealed) continue;

                // Passive hazard accumulation (RNG draw order is part of the
                // deterministic replay contract and must not change).
                ApplyMethaneDelta(sector, _rng.Next(50, 150), int.MaxValue);

                // Authored passive mitigation upkeep (replaces the former
                // hardcoded blower-only drain; JSON is the authority).
                ApplyPassiveMitigationDecay(sector);

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
