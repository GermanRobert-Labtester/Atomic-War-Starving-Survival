using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Cross-host save envelope for the Godot expansion hub: Waystation (Holdfast
    /// S2 vitals), Standing Record (Exp 03 layouts/memory/site encounters),
    /// Crossing gate (Exp 04 vouch), and Greenhouse (Exp 05 plots).
    /// Written through the IJsonSerializer port so a save written by one host
    /// loads in the other, same as HoldfastSave / YearOfAshSave / DutyRosterSave.
    /// </summary>
    [Serializable]
    public class ExpansionHubSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public WaystationSystemState waystation = new WaystationSystemState();
        public LocationLayoutState layouts = new LocationLayoutState();
        public LocationMemoryState memory = new LocationMemoryState();
        public SiteEncounterState siteEncounters = new SiteEncounterState();
        public VouchAccessSystemState vouch = new VouchAccessSystemState();
        public GreenhouseState greenhouse = new GreenhouseState();
        public CrossingArbitrationState arbitration = new CrossingArbitrationState();
        public LedgerDebtSystemState ledger = new LedgerDebtSystemState();

        /// <summary>Integrity hash computed over all payload fields.</summary>
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Serialization codec for the expansion-hub state. Same rules as the other
    /// save codecs: checksum recomputed on encode, hard-reject on decode for an
    /// empty payload, missing/mismatched checksum, or a newer saveVersion.
    /// </summary>
    public static class ExpansionHubSaveCodec
    {
        public static ExpansionHubSave Capture(
            int simDay,
            WaystationSystem waystation,
            LocationLayoutSystem layouts,
            LocationMemorySystem memory,
            SiteEncounterSystem siteEncounters,
            VouchAccessSystem vouch,
            GreenhouseSystem greenhouse,
            CrossingArbitrationSystem arbitration = null,
            LedgerDebtSystem ledger = null)
        {
            var save = new ExpansionHubSave
            {
                simDay = simDay,
                waystation = waystation.CaptureState(),
                layouts = layouts.CaptureState(),
                memory = memory.CaptureState(),
                siteEncounters = siteEncounters.CaptureState(),
                vouch = vouch.CaptureState(),
                greenhouse = greenhouse.CaptureState()
            };
            if (arbitration != null) save.arbitration = arbitration.CaptureState();
            if (ledger != null) save.ledger = ledger.CaptureState();
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string Encode(ExpansionHubSave save, IJsonSerializer json)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static ExpansionHubSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("ExpansionHubSave: empty save payload.");

            ExpansionHubSave save;
            try
            {
                save = json.Deserialize<ExpansionHubSave>(jsonText);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "ExpansionHubSave: malformed save payload: " + e.Message, e);
            }

            if (save == null)
                throw new InvalidOperationException("ExpansionHubSave: empty save payload.");

            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException(
                    "ExpansionHubSave: save carries no checksum (truncated or tampered file).");
            if (save.saveVersion > ExpansionHubSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "ExpansionHubSave: saveVersion " + save.saveVersion
                    + " is newer than this build supports (" + ExpansionHubSave.CurrentSaveVersion + ").");
            if (save.saveVersion < 1)
                throw new InvalidOperationException(
                    "ExpansionHubSave: saveVersion " + save.saveVersion + " is not a valid version.");

            string actual = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "ExpansionHubSave: checksum mismatch (corrupt or foreign save).");

            return save;
        }

        /// <summary>Restores all hub systems. Idempotent.</summary>
        public static void Restore(
            ExpansionHubSave save,
            WaystationSystem waystation,
            LocationLayoutSystem layouts,
            LocationMemorySystem memory,
            SiteEncounterSystem siteEncounters,
            VouchAccessSystem vouch,
            GreenhouseSystem greenhouse,
            CrossingArbitrationSystem arbitration = null,
            LedgerDebtSystem ledger = null)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            waystation?.RestoreState(save.waystation);
            layouts?.RestoreState(save.layouts);
            memory?.RestoreState(save.memory);
            siteEncounters?.RestoreState(save.siteEncounters);
            vouch?.RestoreState(save.vouch);
            greenhouse?.RestoreState(save.greenhouse);
            arbitration?.RestoreState(save.arbitration);
            ledger?.RestoreState(save.ledger);
        }
    }
}
