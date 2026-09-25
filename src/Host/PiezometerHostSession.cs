// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : PiezometerHostSession
// Core Source  : Ashfall.Core.Shelter.AquiferPiezometerEngine
// Purpose      : Plan 189 — thin adapter: catalog load, construction command,
//                daily intake advisory for the water authority. Never grants,
//                moves, or purifies water.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class PiezometerHostSession : HostSessionBase
    {
        public const string CatalogFileName = "piezometer_network_catalog.json";

        public AquiferPiezometerEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public PiezometerHostSession(AquiferPiezometerEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        /// <summary>Loads the authored piezometer catalog; missing file is non-fatal.</summary>
        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            string path = Path.Combine(dataDir, CatalogFileName);
            if (!File.Exists(path)) return;
            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return;

            var catalog = new SystemTextJsonSerializer().Deserialize<AquiferPiezometerCatalog>(json);
            if (catalog != null)
                System.BindCatalog(catalog);
        }

        public string ConstructNetwork(Inventory inventory)
        {
            if (inventory == null)
                return LastEvent = "Piezometer network unavailable: inventory is not connected.";

            var costs = new Dictionary<string, int>(StringComparer.Ordinal);
            var strata = System.Catalog.strata;
            if (strata == null || strata.Count == 0)
                return LastEvent = "Cannot install monitoring network (piez.no_strata).";

            foreach (var zone in strata)
            {
                if (zone?.sensor_install_cost == null) continue;
                foreach (var cost in zone.sensor_install_cost)
                {
                    if (string.IsNullOrWhiteSpace(cost.Key) || cost.Value <= 0) continue;
                    costs[cost.Key] = costs.TryGetValue(cost.Key, out int amount)
                        ? checked(amount + cost.Value)
                        : cost.Value;
                }
            }

            try
            {
                bool committed = inventory.TryConsumeBill(costs, () =>
                {
                    // Core owns the construction rules and cost verification.
                    // Inventory has already staged the bill, so expose the
                    // pre-transaction count and make its internal consume port
                    // a no-op. The inventory transaction rolls back if Core
                    // refuses construction.
                    System.BindInventory(
                        itemId => inventory.CountById(itemId)
                            + (costs.TryGetValue(itemId, out int prepaid) ? prepaid : 0),
                        (_, _) => { });
                    var result = System.ConstructNetwork();
                    if (result.Status != ActionResult.StatusKind.Success)
                        throw new InvalidOperationException(
                            $"Cannot install monitoring network ({result.FailureCode}).");
                });

                if (!committed)
                    LastEvent = "Cannot install monitoring network (piez.materials_missing).";
                else
                {
                    LastEvent = "Aquifer monitoring network installed.";
                    RaiseStateChanged();
                }
            }
            catch (Exception ex)
            {
                LastEvent = ex.Message;
            }
            finally
            {
                System.BindInventory(
                    itemId => inventory.CountById(itemId),
                    (itemId, count) => inventory.TryConsumeById(itemId, count));
            }
            return LastEvent;
        }

        /// <summary>
        /// Daily intelligence step: tick the network and hand the resulting
        /// advisory to the water authority's intake gate. Returns true when a
        /// new advisory was registered.
        /// </summary>
        public bool PublishAdvisory(Ashfall.Core.WaterTreatmentSystem waterTreatment, int day)
        {
            if (waterTreatment == null || !System.IsConstructed) return false;

            System.TickDay(day);
            var advisory = System.BuildAdvisory();
            bool accepted = waterTreatment.RegisterContaminationAdvisory(
                advisory.advisory_id,
                advisory.advisory_level,
                advisory.contaminated_source_ids);

            if (accepted)
            {
                LastEvent = advisory.advisory_level == "none"
                    ? $"Aquifer advisory clear ({advisory.drawdown_state})."
                    : $"Aquifer advisory '{advisory.advisory_level}' — {advisory.contaminated_source_ids.Count} source(s) flagged.";
                RaiseStateChanged();
            }
            return accepted;
        }

        public HydrogeologyNetworkState CaptureSave() => System.CaptureState();

        public void RestoreSave(HydrogeologyNetworkState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "Aquifer monitoring network restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            PiezometerSaveStore.TrySave(CaptureSave());
        }
    }
}
