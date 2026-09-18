// SPDX-License-Identifier: MIT
// Plan 31C — opt-in replayable day-record diagnostics (JSONL).
// Dev/debug only: gated by the ASHFALL_DAY_RECORD=1 environment flag; off in
// release. Observational only — never read back into simulation state.
using System;
using System.IO;
using Godot;
using Ashfall.Core.Campaign;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private const string DayRecordEnvVar = "ASHFALL_DAY_RECORD";

        private static bool DayRecordEnabled =>
            string.Equals(System.Environment.GetEnvironmentVariable(DayRecordEnvVar), "1", StringComparison.Ordinal);

        private void AppendDayRecordIfEnabled(int day, DayAdvancedEventArgs? args)
        {
            if (args == null || !DayRecordEnabled) return;
            try
            {
                string sessionId = _saveLoadHost?.ActiveSlotId?.ToString() ?? "unsaved";
                var record = DayRecordBuilder.FromDay(CampaignSeedForGeneration(), sessionId, day, args);
                string path = ProjectSettings.GlobalizePath("user://day-record.jsonl");
                File.AppendAllText(path, DayRecordBuilder.ToJsonLine(record) + "\n");
            }
            catch (Exception e)
            {
                GD.PushWarning($"[DayRecord] append failed: {e.Message}");
            }
        }
    }
}
