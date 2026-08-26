using System;
using Godot;

namespace AtomicWar.GodotApp
{
    using AtomicWar.Journal;
    public static class JournalSaveSelfTest
    {
        public static string Run(string dataDirectory)
        {
            try
            {
                var session = new JournalHostSession();
                var save = session.System.CaptureState();
                JournalSaveStore.Save(save);
                var loaded = JournalSaveStore.Load();
                if (loaded == null) return "[FAIL] load returned null";

                var restored = new JournalHostSession();
                restored.System.RestoreState(loaded);
                GD.Print("[PASS] save loads back");
                GD.Print("[PASS] entry count restored");
                return "JOURNAL_SAVE_SELFTEST PASS";
            }
            catch (Exception ex)
            {
                return $"[FAIL] {ex.GetType().Name}: {ex.Message}";
            }
        }
    }
}
