using System;
using System.Collections.Generic;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Daily briefing store — engine-agnostic save envelope for unacknowledged
    /// <see cref="DailyBriefingReport"/>s. Follows the same shape/checksum rules
    /// as DoseLedger/Holdfast/YearOfAsh envelopes:
    ///   * checksum recomputed on encode
    ///   * hard-reject on decode for tamper / missing checksum / newer version
    ///   * missing fields in older saves default safely via the v1 frozen shape
    /// </summary>
    [Serializable]
    public class DailyBriefingSave
    {
        public const int CurrentSaveVersion = 1;
        public const int MigrationFromVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public List<DailyBriefingReport> PendingReports = new List<DailyBriefingReport>();
        public List<int> AcknowledgedDays = new List<int>();
        public string Checksum = string.Empty;
    }

    [Serializable]
    public sealed class DailyBriefingState
    {
        public List<DailyBriefingReport> Pending = new List<DailyBriefingReport>();
        public HashSet<int> AcknowledgedDays = new HashSet<int>();

        public bool HasUnacknowledged(int day)
        {
            for (int i = 0; i < Pending.Count; i++)
                if (Pending[i] != null && Pending[i].Day == day) return true;
            return false;
        }

        public DailyBriefingReport? Consume(int day)
        {
            for (int i = 0; i < Pending.Count; i++)
            {
                if (Pending[i] != null && Pending[i].Day == day)
                {
                    var r = Pending[i];
                    Pending.RemoveAt(i);
                    AcknowledgedDays.Add(day);
                    return r;
                }
            }
            return null;
        }

        public void Enqueue(DailyBriefingReport report)
        {
            if (report == null) return;
            Pending.Add(report);
        }

        public DailyBriefingSave CaptureState() => new DailyBriefingSave
        {
            saveVersion = DailyBriefingSave.CurrentSaveVersion,
            simDay = Pending.Count > 0 ? Pending[Pending.Count - 1].Day : 0,
            PendingReports = new List<DailyBriefingReport>(Pending),
            AcknowledgedDays = new List<int>(AcknowledgedDays)
        };

        public void RestoreState(DailyBriefingSave save)
        {
            Pending.Clear();
            AcknowledgedDays.Clear();
            if (save == null) return;
            if (save.PendingReports != null)
                for (int i = 0; i < save.PendingReports.Count; i++)
                    if (save.PendingReports[i] != null)
                        Pending.Add(save.PendingReports[i]);
            if (save.AcknowledgedDays != null)
                for (int i = 0; i < save.AcknowledgedDays.Count; i++)
                    AcknowledgedDays.Add(save.AcknowledgedDays[i]);
        }
    }

    public static class DailyBriefingSaveCodec
    {
        public static DailyBriefingSave Encode(DailyBriefingSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (save.saveVersion > DailyBriefingSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "DailyBriefingSave: refusing to encode a saveVersion newer than supported.");
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string EncodeToString(DailyBriefingSave save, IJsonSerializer json)
        {
            Encode(save, json);
            return json.Serialize(save);
        }

        public static DailyBriefingSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("DailyBriefingSave: empty save payload.");
            DailyBriefingSave save;
            try { save = json.Deserialize<DailyBriefingSave>(jsonText!); }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "DailyBriefingSave: malformed save payload: " + e.Message, e);
            }
            if (save == null)
                throw new InvalidOperationException("DailyBriefingSave: empty save payload.");
            if (save.saveVersion > DailyBriefingSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "DailyBriefingSave: saveVersion " + save.saveVersion + " is newer than supported.");
            if (save.saveVersion < DailyBriefingSave.MigrationFromVersion)
                throw new InvalidOperationException("DailyBriefingSave: invalid saveVersion.");
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException(
                    "DailyBriefingSave: save carries no checksum (truncated or tampered file).");
            string actual = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "DailyBriefingSave: checksum mismatch (corrupt or foreign save).");
            return save;
        }
    }
}
