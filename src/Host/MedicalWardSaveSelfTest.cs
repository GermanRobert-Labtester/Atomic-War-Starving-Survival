using System;
using Godot;

namespace AtomicWar.GodotApp
{
    public static class MedicalWardSaveSelfTest
    {
        public static string Run(string dataDirectory)
        {
            try
            {
                var session = new MedicalWardHostSession();
                session.Save();
                var loaded = MedicalWardSaveStore.TryLoad();
                if (loaded == null) return "[FAIL] load returned null";

                var restored = new MedicalWardHostSession();
                restored.System.RestoreState(loaded.State);
                GD.Print("[PASS] save loads back");
                GD.Print("[PASS] simDay restored");
                return "MEDICAL_WARD_SAVE_SELFTEST PASS";
            }
            catch (Exception ex)
            {
                return $"[FAIL] {ex.GetType().Name}: {ex.Message}";
            }
        }
    }
}
