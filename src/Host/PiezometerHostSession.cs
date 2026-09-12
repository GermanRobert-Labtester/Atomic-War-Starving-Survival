// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : PiezometerHostSession
// Core Source  : Ashfall.Core.Shelter.AquiferPiezometerEngine
// Purpose      : Plan 189 — thin adapter: catalog load, construction command,
//                daily intake advisory for the water authority. Never grants,
//                moves, or purifies water.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
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

        public string ConstructNetwork()
        {
            var result = System.ConstructNetwork();
            RaiseStateChanged();
            LastEvent = result.Status == ActionResult.StatusKind.Success
                ? "Aquifer monitoring network installed."
                : $"Cannot install monitoring network ({result.FailureCode}).";
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
