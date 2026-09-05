using System;
using Ashfall.Core.YearOfAsh;

using Ashfall.Core.IO;
namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — cross-host save envelope.
    /// Mirrors DoseLedgerSave / HoldfastSave: checksum recomputed on encode,
    /// hard-reject on decode for tamper / checksumless / newer version.
    ///
    /// v3 adds the Verdict questline section. Verdict quest progress is owned
    /// here — the Year of Ash envelope is no longer a second owner (its
    /// registration was removed). v1/v2 saves migrate with an empty quest
    /// section; a one-time adoption helper folds any quest_verdict_* progress
    /// that a pre-v3 save carried inside the Year of Ash envelope into the
    /// Verdict envelope (see <see cref="VerdictQuestMigration"/>).
    ///
    /// Migration validates the checksum over each version's FROZEN shape (see
    /// <see cref="VerdictSaveV1"/> / <see cref="VerdictSaveV2"/>) because
    /// <see cref="SaveChecksum"/> walks public fields — validating a legacy
    /// payload against the current shape would always mismatch.
    /// </summary>
    [Serializable]
    public class VerdictSave
    {
        public const int CurrentSaveVersion = 4;
        public const int MigrationFromVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public MachineLogSystemState machineLog = new MachineLogSystemState();
        public ReckoningState reckoning = new ReckoningState();
        public EvidenceLedgerState evidence = new EvidenceLedgerState();
        public VerdictNpcState npcs = new VerdictNpcState();
        public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
        public QuestlineSystemState quests = new QuestlineSystemState();
        public int censusLastWindowDay = -1;

        // v4 section.
        public VerdictAccusationState accusations = new VerdictAccusationState();

        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Frozen v3 envelope shape (npcs + radio + quests, no accusations section).
    /// Do not add fields here — it must match what v3 wrote byte-for-byte in field set.
    /// </summary>
    [Serializable]
    public class VerdictSaveV3
    {
        public int saveVersion = 3;
        public int simDay;
        public MachineLogSystemState machineLog = new MachineLogSystemState();
        public ReckoningState reckoning = new ReckoningState();
        public EvidenceLedgerState evidence = new EvidenceLedgerState();
        public VerdictNpcState npcs = new VerdictNpcState();
        public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
        public QuestlineSystemState quests = new QuestlineSystemState();
        public int censusLastWindowDay = -1;
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Frozen v1 envelope shape (no npcs, no radio, no quests). Kept so a v1
    /// file on disk validates against the field set it was actually hashed with.
    /// Do not add fields here.
    /// </summary>
    [Serializable]
    public class VerdictSaveV1
    {
        public int saveVersion = 1;
        public int simDay;
        public MachineLogSystemState machineLog = new MachineLogSystemState();
        public ReckoningState reckoning = new ReckoningState();
        public EvidenceLedgerState evidence = new EvidenceLedgerState();
        public int censusLastWindowDay = -1;
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Frozen v2 envelope shape (npcs + radio, no quests). Kept so a v2 file on
    /// disk validates against the field set it was actually hashed with.
    /// Do not add fields here.
    /// </summary>
    [Serializable]
    public class VerdictSaveV2
    {
        public int saveVersion = 2;
        public int simDay;
        public MachineLogSystemState machineLog = new MachineLogSystemState();
        public ReckoningState reckoning = new ReckoningState();
        public EvidenceLedgerState evidence = new EvidenceLedgerState();
        public VerdictNpcState npcs = new VerdictNpcState();
        public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
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
VerdictNpcSystem? npcs = null,
VerdictRadioSystem? radio = null,
QuestlineSystem? quests = null,
VerdictAccusationSystem? accusations = null)
        {
            var save = new VerdictSave
            {
                simDay = simDay,
                machineLog = machineLog.CaptureState(),
                reckoning = reckoning.CaptureState(),
                evidence = evidence.CaptureState(),
                npcs = npcs != null ? npcs.CaptureState() : new VerdictNpcState(),
                radio = radio != null ? radio.CaptureState() : new VerdictRadioSystem.VerdictRadioState(),
                quests = quests != null ? quests.CaptureState() : new QuestlineSystemState(),
                accusations = accusations != null ? accusations.CaptureState() : new VerdictAccusationState(),
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

        /// <summary>
        /// Decodes and migrates a Verdict save. Legacy versions are parsed as
        /// their FROZEN shapes so the checksum is verified over exactly the
        /// fields that version wrote. Rejects: newer versions, too-old versions,
        /// checksumless payloads, and tampered payloads.
        /// </summary>
        public static bool TryDecode(string json, IJsonSerializer serializer, out VerdictSave save)
        {
            save = null!;
            if (string.IsNullOrEmpty(json) || serializer == null) return false;
            try
            {
                var decoded = serializer.Deserialize<VerdictSave>(json);
                if (decoded == null) return false;
                if (decoded.saveVersion > VerdictSave.CurrentSaveVersion) return false; // newer — reject
                if (decoded.saveVersion < VerdictSave.MigrationFromVersion) return false; // too old — reject

                if (decoded.saveVersion == 1) return MigrateV1(json, serializer, out save);
                if (decoded.saveVersion == 2) return MigrateV2(json, serializer, out save);
                if (decoded.saveVersion == 3) return MigrateV3(json, serializer, out save);

                // Current version: validate over the current shape.
                if (string.IsNullOrEmpty(decoded.Checksum)) return false;   // tamper/legacy
                string recomputed = SaveChecksum.Compute(decoded);
                if (!string.Equals(recomputed, decoded.Checksum, StringComparison.Ordinal))
                    return false; // tampered
                save = decoded;
                return true;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<decode>", "VerdictSave", ex_CATDIAG);
                return false;
            }
        }

        private static bool MigrateV1(string json, IJsonSerializer serializer, out VerdictSave save)
        {
            save = null!;
            var v1 = serializer.Deserialize<VerdictSaveV1>(json);
            if (v1 == null) return false;
            if (string.IsNullOrEmpty(v1.Checksum)) return false;
            if (!string.Equals(SaveChecksum.Compute(v1), v1.Checksum, StringComparison.Ordinal)) return false;

            var migrated = new VerdictSave
            {
                saveVersion = VerdictSave.CurrentSaveVersion,
                simDay = v1.simDay,
                machineLog = v1.machineLog,
                reckoning = v1.reckoning,
                evidence = v1.evidence,
                // npcs / radio / quests / accusations stay at their field initialisers (fresh defaults).
                censusLastWindowDay = v1.censusLastWindowDay
            };
            migrated.Checksum = SaveChecksum.Compute(migrated);
            save = migrated;
            return true;
        }

        private static bool MigrateV2(string json, IJsonSerializer serializer, out VerdictSave save)
        {
            save = null!;
            var v2 = serializer.Deserialize<VerdictSaveV2>(json);
            if (v2 == null) return false;
            if (string.IsNullOrEmpty(v2.Checksum)) return false;
            if (!string.Equals(SaveChecksum.Compute(v2), v2.Checksum, StringComparison.Ordinal)) return false;

            var migrated = new VerdictSave
            {
                saveVersion = VerdictSave.CurrentSaveVersion,
                simDay = v2.simDay,
                machineLog = v2.machineLog,
                reckoning = v2.reckoning,
                evidence = v2.evidence,
                npcs = v2.npcs ?? new VerdictNpcState(),
                radio = v2.radio ?? new VerdictRadioSystem.VerdictRadioState(),
                // quests / accusations stay at their field initialisers (fresh defaults).
                censusLastWindowDay = v2.censusLastWindowDay
            };
            migrated.Checksum = SaveChecksum.Compute(migrated);
            save = migrated;
            return true;
        }

        private static bool MigrateV3(string json, IJsonSerializer serializer, out VerdictSave save)
        {
            save = null!;
            var v3 = serializer.Deserialize<VerdictSaveV3>(json);
            if (v3 == null) return false;
            if (string.IsNullOrEmpty(v3.Checksum)) return false;
            if (!string.Equals(SaveChecksum.Compute(v3), v3.Checksum, StringComparison.Ordinal)) return false;

            var migrated = new VerdictSave
            {
                saveVersion = VerdictSave.CurrentSaveVersion,
                simDay = v3.simDay,
                machineLog = v3.machineLog,
                reckoning = v3.reckoning,
                evidence = v3.evidence,
                npcs = v3.npcs ?? new VerdictNpcState(),
                radio = v3.radio ?? new VerdictRadioSystem.VerdictRadioState(),
                quests = v3.quests ?? new QuestlineSystemState(),
                // accusations stays at its field initialiser (fresh empty state).
                censusLastWindowDay = v3.censusLastWindowDay
            };
            migrated.Checksum = SaveChecksum.Compute(migrated);
            save = migrated;
            return true;
        }

        public static void Restore(
            VerdictSave save,
            MachineLogSystem machineLog,
            ReckoningSystem reckoning,
            EvidenceLedger evidence,
VerdictNpcSystem? npcs = null,
VerdictRadioSystem? radio = null,
QuestlineSystem? quests = null,
VerdictAccusationSystem? accusations = null)
        {
            if (save == null) return;
            machineLog.RestoreState(save.machineLog);
            reckoning.RestoreState(save.reckoning);
            evidence.RestoreState(save.evidence);
            if (npcs != null)
                npcs.RestoreState(save.npcs ?? new VerdictNpcState());
            if (radio != null)
                radio.RestoreState(save.radio ?? new VerdictRadioSystem.VerdictRadioState());
            if (quests != null)
                quests.RestoreState(save.quests ?? new QuestlineSystemState());
            if (accusations != null)
                accusations.RestoreState(save.accusations ?? new VerdictAccusationState());
        }
    }
}
