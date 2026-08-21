using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Medical
{
    /// <summary>Checksummed save envelope for the medical ward.</summary>
    [Serializable]
    public class MedicalWardSave
    {
        public const int CurrentSaveVersion = 1;
        public const int MigrationFromVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public List<MedicalBedSave> Beds = new List<MedicalBedSave>();
        public List<MedicalProcedureDef> Procedures = new List<MedicalProcedureDef>();
        public MedicalWardState State = new MedicalWardState();
        public string Checksum = string.Empty;
    }

    [Serializable]
    public sealed class MedicalBedSave
    {
        public string BedId;
        public string DisplayName;
        public int Category; // serialized as int for cross-host stability
        public bool Isolation;
    }

    public static class MedicalWardSaveCodec
    {
        public static MedicalWardSave Encode(MedicalWardSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (save.saveVersion > MedicalWardSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "MedicalWardSave: refusing to encode a saveVersion newer than supported.");
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string EncodeToString(MedicalWardSave save, IJsonSerializer json)
        {
            Encode(save, json);
            return json.Serialize(save);
        }

        public static MedicalWardSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("MedicalWardSave: empty save payload.");
            MedicalWardSave save;
            try { save = json.Deserialize<MedicalWardSave>(jsonText!); }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "MedicalWardSave: malformed save payload: " + e.Message, e);
            }
            if (save == null)
                throw new InvalidOperationException("MedicalWardSave: empty save payload.");
            if (save.saveVersion > MedicalWardSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "MedicalWardSave: saveVersion " + save.saveVersion + " is newer than supported.");
            if (save.saveVersion < MedicalWardSave.MigrationFromVersion)
                throw new InvalidOperationException("MedicalWardSave: invalid saveVersion.");
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException(
                    "MedicalWardSave: save carries no checksum (truncated or tampered file).");
            string actual = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "MedicalWardSave: checksum mismatch (corrupt or foreign save).");
            return save;
        }
    }
}
