using System;
using Ashfall.Core.Crossing;
using Ashfall.Core.Foundry;
using Ashfall.Core.Legacy;

namespace Ashfall.Core
{
    /// <summary>
    /// Cross-host save envelope for the Godot expansion hub: Waystation (Holdfast
    /// S2 vitals), Standing Record (Exp 03 layouts/memory/site encounters),
    /// Crossing gate (Exp 04 vouch), Greenhouse (Exp 05 plots), the
    /// Silent Foundry (Exp 10 smelter bay), and its treaty consequence ledger.
    /// Written through the IJsonSerializer port so a save written by one host
    /// loads in the other, same as HoldfastSave / YearOfAshSave / DutyRosterSave.
    /// </summary>
    [Serializable]
    public class ExpansionHubSave
    {
        /// <summary>
        /// v2 added the Silent Foundry (Exp 10) state; v3 adds the durable
        /// treaty-consequence ledger (standing + market/logistics modifiers).
        /// v1 and v2 saves migrate forward with safe defaults.
        /// </summary>
        public const int CurrentSaveVersion = 3;

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
        public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
        public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
        public SilentFoundryState foundry = new SilentFoundryState();
        public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();

        /// <summary>Integrity hash computed over all payload fields.</summary>
        public string Checksum = string.Empty;
    }

    /// <summary>Frozen v1 shape — used to validate legacy saves against their own checksum.</summary>
    [Serializable]
    public sealed class ExpansionHubSaveV1
    {
        public int saveVersion = 1;
        public int simDay;
        public WaystationSystemState waystation = new WaystationSystemState();
        public LocationLayoutState layouts = new LocationLayoutState();
        public LocationMemoryState memory = new LocationMemoryState();
        public SiteEncounterState siteEncounters = new SiteEncounterState();
        public VouchAccessSystemState vouch = new VouchAccessSystemState();
        public GreenhouseState greenhouse = new GreenhouseState();
        public CrossingArbitrationState arbitration = new CrossingArbitrationState();
        public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
        public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
        public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
        public string Checksum = string.Empty;
    }

    /// <summary>Frozen v2 shape (foundry added; consequence ledger did not exist).</summary>
    [Serializable]
    public sealed class ExpansionHubSaveV2
    {
        public int saveVersion = 2;
        public int simDay;
        public WaystationSystemState waystation = new WaystationSystemState();
        public LocationLayoutState layouts = new LocationLayoutState();
        public LocationMemoryState memory = new LocationMemoryState();
        public SiteEncounterState siteEncounters = new SiteEncounterState();
        public VouchAccessSystemState vouch = new VouchAccessSystemState();
        public GreenhouseState greenhouse = new GreenhouseState();
        public CrossingArbitrationState arbitration = new CrossingArbitrationState();
        public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
        public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
        public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
        public SilentFoundryState foundry = new SilentFoundryState();
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
            LedgerDebtSystem ledger = null,
            CrossingQuestSystem crossingQuests = null,
            GenerationalSuccessionEngine generational = null,
            SilentFoundrySystem silentFoundry = null)
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
            if (crossingQuests != null) save.crossingQuests = crossingQuests.CaptureState();
            if (generational != null) save.generational = generational.CaptureState();
            if (silentFoundry != null)
            {
                save.foundry = silentFoundry.CaptureState();
                save.consequences = silentFoundry.CaptureConsequenceState();
            }
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

            try
            {
                // v1 saves are validated against their FROZEN shape (the file is
                // deserialized into the v1 type, so any field added later — the
                // foundry, then the consequence ledger — is dropped, never blessed)
                // and migrated forward with safe defaults.
                var v1 = json.Deserialize<ExpansionHubSaveV1>(jsonText);
                if (v1 != null && v1.saveVersion == 1)
                {
                    ValidateChecksum(v1.Checksum, v1, "v1");
                    var migrated = new ExpansionHubSave
                    {
                        saveVersion = ExpansionHubSave.CurrentSaveVersion,
                        simDay = v1.simDay,
                        waystation = v1.waystation ?? new WaystationSystemState(),
                        layouts = v1.layouts ?? new LocationLayoutState(),
                        memory = v1.memory ?? new LocationMemoryState(),
                        siteEncounters = v1.siteEncounters ?? new SiteEncounterState(),
                        vouch = v1.vouch ?? new VouchAccessSystemState(),
                        greenhouse = v1.greenhouse ?? new GreenhouseState(),
                        arbitration = v1.arbitration ?? new CrossingArbitrationState(),
                        ledger = v1.ledger ?? new LedgerDebtSystemState(),
                        crossingQuests = v1.crossingQuests ?? new CrossingQuestSystemState(),
                        generational = v1.generational ?? new GenerationalSuccessionSaveState(),
                        foundry = new SilentFoundryState(),
                        consequences = new SilentFoundryConsequenceState()
                    };
                    migrated.Checksum = SaveChecksum.Compute(migrated);
                    return migrated;
                }

                // v2 saves carry the foundry but predate the consequence ledger;
                // the ledger starts empty and the foundry state is preserved.
                var v2 = json.Deserialize<ExpansionHubSaveV2>(jsonText);
                if (v2 != null && v2.saveVersion == 2)
                {
                    ValidateChecksum(v2.Checksum, v2, "v2");
                    var migrated = new ExpansionHubSave
                    {
                        saveVersion = ExpansionHubSave.CurrentSaveVersion,
                        simDay = v2.simDay,
                        waystation = v2.waystation ?? new WaystationSystemState(),
                        layouts = v2.layouts ?? new LocationLayoutState(),
                        memory = v2.memory ?? new LocationMemoryState(),
                        siteEncounters = v2.siteEncounters ?? new SiteEncounterState(),
                        vouch = v2.vouch ?? new VouchAccessSystemState(),
                        greenhouse = v2.greenhouse ?? new GreenhouseState(),
                        arbitration = v2.arbitration ?? new CrossingArbitrationState(),
                        ledger = v2.ledger ?? new LedgerDebtSystemState(),
                        crossingQuests = v2.crossingQuests ?? new CrossingQuestSystemState(),
                        generational = v2.generational ?? new GenerationalSuccessionSaveState(),
                        foundry = v2.foundry ?? new SilentFoundryState(),
                        consequences = new SilentFoundryConsequenceState()
                    };
                    migrated.Checksum = SaveChecksum.Compute(migrated);
                    return migrated;
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "ExpansionHubSave: malformed save payload: " + e.Message, e);
            }

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

            ValidateChecksum(save.Checksum, save, "v" + save.saveVersion);

            // Defensive defaults for files that predate a field (older v3 builds
            // without the consequence ledger, or hand-written fixtures).
            if (save.foundry == null) save.foundry = new SilentFoundryState();
            if (save.consequences == null) save.consequences = new SilentFoundryConsequenceState();
            return save;
        }

        private static void ValidateChecksum(string expected, object payload, string label)
        {
            if (string.IsNullOrEmpty(expected))
                throw new InvalidOperationException(
                    "ExpansionHubSave: save carries no checksum (truncated or tampered file).");
            string actual = SaveChecksum.Compute(payload);
            if (!string.Equals(expected, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "ExpansionHubSave: checksum mismatch (" + label + ", corrupt or foreign save).");
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
            LedgerDebtSystem ledger = null,
            CrossingQuestSystem crossingQuests = null,
            GenerationalSuccessionEngine generational = null,
            SilentFoundrySystem silentFoundry = null)
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
            crossingQuests?.RestoreState(save.crossingQuests);
            if (save.generational != null) generational?.RestoreState(save.generational);
            // Safe default for older saves / missing foundry state. Never resets
            // an active furnace from a save that carries one.
            if (silentFoundry != null && save.foundry != null)
                silentFoundry.RestoreState(save.foundry);
            // Consequence ledger: missing state (v1/v2 migration) defaults to an
            // empty ledger and neutral standing; nothing is re-applied because
            // the ledger is the idempotency authority.
            if (silentFoundry != null)
                silentFoundry.RestoreConsequenceState(save.consequences);
        }
    }
}
