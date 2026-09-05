// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Crafting;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ChemicalSynthesisSystem.
    /// Manages industrial retorts, catalyst degradation, scrubber reserves,
    /// and hazardous synthesis reactions. Adapts Core events for Godot UI.
    /// </summary>
    public sealed class ChemicalSynthesisHostSession : HostSessionBase
    {
        public ChemicalSynthesisSystem System { get; }
        public ChemicalSynthesisCatalog Catalog { get; }
        public string LastEvent { get; private set; } = "Chemical synthesis apparatus online — ready for batch sequencing.";

        public ChemicalSynthesisHostSession(
            ChemicalSynthesisSystem system,
            ChemicalSynthesisCatalog catalog)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            Catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));

            System.OnStateChanged += () => RaiseStateChanged();

            System.OnProcessStarted += (vesselId, processId) =>
            {
                var def = Catalog.GetProcess(processId);
                string name = def?.displayName ?? processId;
                LastEvent = $"[RETORT {vesselId.ToUpperInvariant()}] Process started: {name}.";
                RaiseStateChanged();
            };

            System.OnProcessCompleted += (vesselId, processId) =>
            {
                var def = Catalog.GetProcess(processId);
                string name = def?.displayName ?? processId;
                LastEvent = $"[RETORT {vesselId.ToUpperInvariant()}] Process completed: {name}. Yield granted to inventory.";
                RaiseStateChanged();
            };

            System.OnProcessFailed += (vesselId, processId, reason) =>
            {
                var def = Catalog.GetProcess(processId);
                string name = def?.displayName ?? processId;
                LastEvent = $"[RETORT {vesselId.ToUpperInvariant()}] ALERT: Process '{name}' failed ({reason}).";
                RaiseStateChanged();
            };

            System.OnExposureIncident += (vesselId, operatorId, severity) =>
            {
                LastEvent = $"[RETORT {vesselId.ToUpperInvariant()}] CRITICAL: Scrubber breach! Exposure severity: {severity:P0}.";
                RaiseStateChanged();
            };
        }

        public bool TryStartProcess(string processId, string vesselId, string operatorId = "")
        {
            bool ok = System.TryStartProcess(processId, vesselId, operatorId);
            if (!ok)
            {
                var vessel = System.GetVessel(vesselId);
                var def = Catalog.GetProcess(processId);
                if (vessel != null && !string.IsNullOrEmpty(vessel.activeProcessId))
                    LastEvent = $"Cannot start process: Vessel '{vesselId}' is busy.";
                else if (def != null && System.ApparatusTier < def.requiredApparatusTier)
                    LastEvent = $"Cannot start process: Requires Apparatus Tier {def.requiredApparatusTier} (Current: {System.ApparatusTier}).";
                else
                    LastEvent = "Cannot start process: Missing required reagent supplies.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool TryHarvestOutput(string vesselId)
        {
            bool ok = System.TryHarvestOutput(vesselId);
            if (!ok)
            {
                LastEvent = $"Cannot harvest vessel '{vesselId}': Reaction not yet complete.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool TryServiceScrubber(string vesselId)
        {
            bool ok = System.TryServiceScrubber(vesselId);
            if (ok)
            {
                LastEvent = $"[RETORT {vesselId.ToUpperInvariant()}] Scrubber media recharged to 100%.";
            }
            else
            {
                LastEvent = "Cannot service scrubber: Requires 2 scrap chemicals and 1 clean water.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool TryPurgeVessel(string vesselId)
        {
            bool ok = System.TryPurgeVessel(vesselId);
            if (ok)
            {
                LastEvent = $"[RETORT {vesselId.ToUpperInvariant()}] Vessel purged and neutralized.";
            }
            return ok;
        }

        public bool TryUpgradeApparatus(int targetTier)
        {
            bool ok = System.TryUpgradeApparatus(targetTier);
            if (ok)
            {
                LastEvent = $"Chemical apparatus upgraded to Tier {targetTier}.";
            }
            else
            {
                LastEvent = $"Cannot upgrade apparatus to Tier {targetTier}: Insufficient scrap metal / wire.";
                RaiseStateChanged();
            }
            return ok;
        }
    }
}
