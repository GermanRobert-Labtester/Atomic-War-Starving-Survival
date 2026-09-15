// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the sanitation authority (Plan 210).
    /// Loads the facility catalog, registers shelter rooms, feeds the daily
    /// population, delivers compost output to the canonical inventory, and
    /// persists state. No rules here — hosts only wire and present.
    /// </summary>
    public sealed class SanitationHostSession
    : HostSessionBase
    {
        public SanitationSystem System { get; }

        public string LastEvent { get; private set; } = string.Empty;

        public SanitationHostSession(SanitationSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnSpillStarted += spill =>
            {
                LastEvent = $"Toxic spill ({WasteLabel(spill.type)}) in {spill.roomId} (severity {spill.severity:0.00})";
                RaiseStateChanged();
            };
            System.OnSpillResolved += spill =>
            {
                LastEvent = $"Spill cleaned in {spill.roomId}";
                RaiseStateChanged();
            };
            System.OnCompostReady += batch =>
            {
                LastEvent = $"Compost ready in {batch.roomId}: {batch.outputUnits:0.0} × {batch.outputItemId}";
                RaiseStateChanged();
            };
            System.OnSanitationComplaint += (topic, band) =>
            {
                LastEvent = $"Complaint ({topic}) — hygiene {band}";
                RaiseStateChanged();
            };
        }

        public static SanitationHostSession Create(string dataDir, SanitationSystem? system = null)
        {
            var load = SanitationFacilityCatalogLoader.Load(
                dataDir ?? string.Empty, new FileSystemIO(), new SystemTextJsonSerializer());
            var session = new SanitationHostSession(system ?? new SanitationSystem());
            if (!load.HasErrors && load.Facilities.Count > 0)
            {
                session.System.BindFacilityCatalog(SanitationFacilityCatalogLoader.ToCatalog(load));
            }
            else if (load.HasErrors)
            {
                session.LastEvent = "Sanitation facility catalog rejected: " + load.Errors[0];
            }

            var saved = SanitationSaveStore.TryLoad();
            if (saved != null)
            {
                session.System.RestoreState(saved);
                session.LastEvent = "Sanitation state restored from save.";
            }
            return session;
        }

        /// <summary>
        /// Advance the sanitation authority one day. Called by the `hygiene`
        /// day owner through the campaign coordinator (phase 3, before the
        /// disease tick). Facilities ride their persisted powered/staffed
        /// flags until the power-grid feed lands (Wave 6 integration).
        /// </summary>
        public void TickDay(int day, int population)
        {
            System.TickDaily(day, population);
        }

        public string StatusLine()
        {
            var spill = System.ActiveSpill;
            return $"Sanitation: hygiene {System.GetShelterHygienePermille()}/1000 ({System.GetShelterHygieneBand()})"
                   + (spill != null ? $" · ACTIVE SPILL {spill.roomId}" : string.Empty)
                   + $" · compost {System.CompostQueue.Count} batches";
        }

        private static string WasteLabel(WasteType type) => type switch
        {
            WasteType.Chemical => "chemical",
            WasteType.Radioactive => "radioactive",
            _ => "organic"
        };

        // ── Save / Load ──────────────────────────────────────────────

        public SanitationState CaptureSave() => System.CaptureState();
        public void RestoreSave(SanitationState state) => System.RestoreState(state);
    }
}
