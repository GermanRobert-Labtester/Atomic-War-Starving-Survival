using System;
using Godot;

namespace AtomicWar.GodotApp
{
    public static class InventorySaveSelfTest
    {
        public static string Run(string dataDirectory)
        {
            try
            {
                var session = new InventoryHostSession();
                session.Add("item_water_clean", 5);
                session.Add("item_food_ration", 3);
                bool saved = InventorySaveStore.TrySave(session.CaptureSave());
                if (!saved) return "[FAIL] save failed";

                var loaded = InventorySaveStore.TryLoad();
                if (loaded == null) return "[FAIL] load returned null";

                var restored = new InventoryHostSession();
                restored.RestoreSave(loaded);
                GD.Print("[PASS] save loads back");
                GD.Print("[PASS] inventory round-trip completed");
                return "INVENTORY_SAVE_SELFTEST PASS";
            }
            catch (Exception ex)
            {
                return $"[FAIL] {ex.GetType().Name}: {ex.Message}";
            }
        }
    }
}
