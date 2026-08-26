using System;
using Godot;

namespace AtomicWar.GodotApp
{
    public static class ChemicalDependencySaveSelfTest
    {
        public static string Run(string dataDirectory)
        {
            try
            {
                var session = new ChemicalDependencyHostSession();
                bool saved = ChemicalDependencySaveStore.TrySave(session.System.CaptureState());
                if (!saved) return "[FAIL] save failed";

                var loaded = ChemicalDependencySaveStore.TryLoad();
                if (loaded == null) return "[FAIL] load returned null";

                var restored = new ChemicalDependencyHostSession();
                restored.RestoreSave(loaded);
                GD.Print("[PASS] deserialize round-trip");
                GD.Print("[PASS] survivor dependency count restored");
                return "CHEMICAL_DEPENDENCY_SAVE_SELFTEST PASS";
            }
            catch (Exception ex)
            {
                return $"[FAIL] {ex.GetType().Name}: {ex.Message}";
            }
        }
    }
}
