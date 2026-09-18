// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 176 — Anomaly Hazard host wire
// System       : AnomalyHazardSystem (authored moving hazard layer)
// Authority    : Core owns movement/spawn/queries; the host only feeds the
//                weather wind vector, evaluates approach warnings toward the
//                shelter, journals typed events, and persists state.
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AnomalyHazardSystem? _anomalyHazard;
        private ScavengingTableCatalog? _anomalyScavengingTables;

        // ── Plan 176: Anomaly Hazards & Storm Fronts ─────────────────────

        public AnomalyHazardSystem EnsureAnomalyHazard()
        {
            if (_anomalyHazard != null) return _anomalyHazard;

            _anomalyHazard = new AnomalyHazardSystem();

            string catalogPath = CatalogPath.ResolveCatalog("anomalies.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var root = System.Text.Json.JsonSerializer.Deserialize<AnomalyCatalogRoot>(json);
                        if (root?.anomalies != null)
                        {
                            var load = new AnomalyCatalogLoadResult();
                            foreach (var def in root.anomalies)
                                if (def != null) load.Anomalies.Add(def);
                            _anomalyHazard.BindCatalog(AnomalyCatalogLoader.ToCatalog(load));
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Anomaly] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = AnomalyHazardSaveStore.TryLoad();
            if (saved != null)
            {
                _anomalyHazard.RestoreState(saved);
            }

            _anomalyHazard.OnStormApproaching += warning =>
            {
                _journal?.TryAddRawEntry(
                    "storm_approaching",
                    $"Anomaly signature {warning.AnomalyId}: {warning.IntensityBand} intensity, bearing {warning.DirectionCardinal}, " +
                    $"ETA ~{warning.EtaDays:F1} days (confidence {warning.Confidence:P0}).",
                    null!, _simDay);
            };

            _anomalyHazard.OnHazardExpired += hazard =>
            {
                _journal?.TryAddRawEntry(
                    "anomaly_dissipated",
                    $"Anomaly zone {hazard.anomaly_id} has dissipated after {hazard.age_days} days.",
                    null!, _simDay);
            };

            _anomalyHazard.OnHazardSpawned += hazard =>
            {
                _journal?.TryAddRawEntry(
                    "anomaly_spawned",
                    $"Survey teams report a new anomaly: {hazard.anomaly_id} (source region telemetry).",
                    null!, _simDay);
            };

            return _anomalyHazard;
        }

        private void SetupAnomalyHazard()
        {
            EnsureAnomalyHazard();
        }

        private void SaveAnomalyHazard()
        {
            if (_anomalyHazard != null)
            {
                CaptureSection("anomaly_hazard", AnomalyHazardSaveStore.TryCapturePersisted(_anomalyHazard.CaptureState()));
            }
        }

        /// <summary>
        /// Daily anomaly tick (Plan 176). Deterministic: consumes the current
        /// weather wind vector from the single weather authority, advances
        /// movement with a day-keyed forked RNG for storm-front wander, then
        /// evaluates approach warnings toward the shelter. Spawning is a host
        /// decision — nothing spawns automatically in Phase 2 (authored events
        /// and region-driven spawn rolls arrive with the campaign-content wave).
        /// </summary>
        public void TickAnomalyHazard(int day)
        {
            if (_anomalyHazard == null) return;

            float windDir = _world?.Weather?.WindDirectionDeg ?? 45f;
            float windSpeed = _world?.Weather?.WindSpeedKph ?? 15f;

            // Day-keyed fork: same day → same wander → split-run identical tracks.
            ISeededRng variation = _campaignDay != null
                ? _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.AnomalyHazard, day)
                : new SeededRng(unchecked(176 * 397 + day));

            _anomalyHazard.WindDailyAdvanceHintKm = windSpeed * 24f;
            _anomalyHazard.TickDay(day, windDir, windSpeed, variation);
            _anomalyHazard.EvaluateApproach("loc_holdfast", 0f, 0f);
        }

        // ── Cross-system consumers (Plan 176) ───────────────────────────

        /// <summary>
        /// Known zone coordinates in the same km-space the fallout host uses
        /// (loc_holdfast at origin). The campaign-content wave will make this
        /// data-driven; until then the anomaly layer shares the fallout
        /// convention exactly, so both layers answer queries consistently.
        /// </summary>
        internal static readonly IReadOnlyDictionary<string, (float X, float Y)> KnownZoneCoordinates =
            new Dictionary<string, (float, float)>(StringComparer.Ordinal)
            {
                { "loc_holdfast", (0f, 0f) },
                { "loc_river_delta", (25f, -10f) },
                { "loc_silo_ruins", (-30f, 40f) },
                { "loc_chemical_plant", (15f, 35f) }
            };

        /// <summary>Typed anomaly radiation handoff for a location id (rads/hr at
        /// that zone). Returns 0 for unknown zones or no anomaly layer — the
        /// dose stays with RadiationSystem, this only REPORTS the rate.</summary>
        public float GetAnomalyLocationRate(string locationId)
        {
            if (_anomalyHazard == null || string.IsNullOrEmpty(locationId)) return 0f;
            if (!KnownZoneCoordinates.TryGetValue(locationId, out var pos)) return 0f;
            return _anomalyHazard.GetRadiationRate(pos.X, pos.Y);
        }

        /// <summary>Detection capability (rads/hr the carried device can resolve).
        /// Uses canonical inventory items only — no parallel device store.</summary>
        public float GetAnomalyDetectionCapability()
        {
            var inv = _inventory?.Inventory;
            if (inv == null) return 0f;
            if (inv.CountById("geiger_counter") > 0 || inv.CountById("item_geiger_m3") > 0)
                return AnomalyHazardSystem.GeigerCapabilityRadsPerHour;
            if (inv.CountById("dosimeter") > 0 || inv.CountById("item_dosimeter_pen") > 0)
                return AnomalyHazardSystem.DosimeterCapabilityRadsPerHour;
            return 0f;
        }

        /// <summary>
        /// Player-facing command: resolve one gated anomaly loot site exactly
        /// once. Resolution grants NO items directly — the canonical
        /// table_loot_* table rolls through the expedition scavenging authority
        /// (single loot-table truth), and the roll lands in the shelter
        /// inventory (single item-quantity authority).
        /// </summary>
        public AnomalyLootResolution ResolveAnomalyLootSite(string siteId)
        {
            var resolution = _anomalyHazard?.TryResolveLootSite(siteId, _simDay);
            if (resolution == null || !resolution.Success)
                return resolution ?? new AnomalyLootResolution { Success = false, ReasonCode = "unknown_loot_site" };

            var tableCatalog = _expeditions?.Engine?.ScavengingCatalog ?? _anomalyScavengingTables;
            if (tableCatalog == null)
            {
                _anomalyScavengingTables = ScavengingTableCatalog.LoadFromDirectory(
                    _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                tableCatalog = _anomalyScavengingTables;
            }
            if (tableCatalog == null || string.IsNullOrEmpty(resolution.LootTableId))
            {
                // Site resolved but the definition carried no table: nothing to grant.
                _journal?.TryAddRawEntry("anomaly_site_looted",
                    $"Gated site {siteId} was opened. Whatever was cached there is gone.", null!, _simDay);
                return resolution;
            }

            ISeededRng rollRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Expedition, _simDay, 9)
                : new SeededRng(unchecked(_simDay * 31 + 17));
            var roll = tableCatalog.RollLoot(resolution.LootTableId, rollRng);
            if (roll != null && !string.IsNullOrEmpty(roll.ItemId) && _inventory?.Inventory != null)
            {
                _inventory.Inventory.AddById(roll.ItemId, Math.Max(1, roll.Quantity));
                _journal?.TryAddRawEntry("anomaly_site_looted",
                    $"Gated site {siteId} was opened: recovered {roll.Quantity}× {roll.ItemId}.", null!, _simDay);
            }
            else
            {
                _journal?.TryAddRawEntry("anomaly_site_looted",
                    $"Gated site {siteId} was opened. It was already picked clean.", null!, _simDay);
            }
            return resolution;
        }

        /// <summary>
        /// Per-wildlife-sector hazard modifiers for the ecology tick (Plan 176
        /// §4.11): maps known location seeds to anomaly modifier at that zone.
        /// Null/empty when no anomaly layer exists — ecology behavior unchanged.
        /// </summary>
        public IReadOnlyDictionary<string, float>? BuildSectorHazardModifiers()
        {
            if (_anomalyHazard == null) return null;
            var seeds = _world?.Seeds?.location_seeds;
            if (seeds == null) return null;

            var result = new Dictionary<string, float>(StringComparer.Ordinal);
            foreach (var seed in seeds)
            {
                if (seed == null || string.IsNullOrEmpty(seed.sector_id) || string.IsNullOrEmpty(seed.location_id))
                    continue;
                if (!KnownZoneCoordinates.TryGetValue(seed.location_id, out var pos)) continue;
                float modifier = _anomalyHazard.GetWildlifeModifier(pos.X, pos.Y);
                if (Math.Abs(modifier) < 0.001f) continue;
                // First location wins on duplicate sectors (deterministic).
                if (!result.ContainsKey(seed.sector_id)) result[seed.sector_id] = modifier;
            }
            return result;
        }
    }
}
