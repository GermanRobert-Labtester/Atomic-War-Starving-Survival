using System;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — cross-host save envelope.
    /// Mirrors DoseLedgerSave / HoldfastSave: checksum recomputed on encode,
    /// hard-reject on decode for tamper / checksumless / newer version.
    /// </summary>
    [Serializable]
    public class VerdictSave
    {
        public const int CurrentSaveVersion = 2;
        public const int MigrationFromVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public MachineLogSystemState machineLog = new MachineLogSystemState();
        public ReckoningState reckoning = new ReckoningState();
        public EvidenceLedgerState evidence = new EvidenceLedgerState();
        public VerdictNpcState npcs = new VerdictNpcState();
        public int censusLastWindowDay = -1;

        public string Checksum = string.Empty;
    }

    public static class VerdictSaveCodec
    {
        public static VerdictSave Capture(
            int simDay,
            MachineLogSystem machineLog,
            ReckoningSystem reckoning,
            EvidenceLedger evidence,
            int censusLastWindowDay,
            VerdictNpcSystem npcs = null)
        {
            var save = new VerdictSave
            {
                simDay = simDay,
                machineLog = machineLog.CaptureState(),
                reckoning = reckoning.CaptureState(),
                evidence = evidence.CaptureState(),
                npcs = npcs != null ? npcs.CaptureState() : new VerdictNpcState(),
                censusLastWindowDay = censusLastWindowDay
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string Encode(VerdictSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static bool TryDecode(string json, IJsonSerializer serializer, out VerdictSave save)
        {
            save = null;
            if (string.IsNullOrEmpty(json) || serializer == null) return false;
            try
            {
                var decoded = serializer.Deserialize<VerdictSave>(json);
                if (decoded == null) return false;
                if (decoded.saveVersion > VerdictSave.CurrentSaveVersion) return false; // newer — reject
                if (decoded.saveVersion < VerdictSave.MigrationFromVersion) return false; // too old — reject
                if (string.IsNullOrEmpty(decoded.Checksum)) return false;               // tamper/legacy
                string recomputed = SaveChecksum.Compute(decoded);
                if (!string.Equals(recomputed, decoded.Checksum, StringComparison.Ordinal))
                    return false; // tampered

                // Migration v1 → v2: v1 lacked NPC state; backfill empty so restores are safe.
                if (decoded.saveVersion == 1)
                {
                    var migrated = new VerdictSave
                    {
                        saveVersion = VerdictSave.CurrentSaveVersion,
                        simDay = decoded.simDay,
                        machineLog = decoded.machineLog,
                        reckoning = decoded.reckoning,
                        evidence = decoded.evidence,
                        npcs = decoded.npcs ?? new VerdictNpcState(),
                        censusLastWindowDay = decoded.censusLastWindowDay
                    };
                    migrated.Checksum = SaveChecksum.Compute(migrated);
                    save = migrated;
                    return true;
                }

                save = decoded;
                return true;
            }
            catch { return false; }
        }

        public static void Restore(
            VerdictSave save,
            MachineLogSystem machineLog,
            ReckoningSystem reckoning,
            EvidenceLedger evidence,
            VerdictNpcSystem npcs = null)
        {
            if (save == null) return;
            machineLog.RestoreState(save.machineLog);
            reckoning.RestoreState(save.reckoning);
            evidence.RestoreState(save.evidence);
            if (npcs != null)
                npcs.RestoreState(save.npcs ?? new VerdictNpcState());
        }
    }
}
