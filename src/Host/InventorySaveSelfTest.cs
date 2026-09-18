// SPDX-License-Identifier: MIT
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
                if (System.IO.File.Exists(InventorySaveStore.SavePath))
                {
                    try { System.IO.File.Delete(InventorySaveStore.SavePath); } catch { /* cleanup: best-effort removal of stale test save */ }
                }
                var session = InventoryHostSession.Create(dataDirectory, seedWhenNoSave: false);
                session.Inventory.Clear();
                session.Add("clean_water", 5);
                session.Add("canned_food", 3);
                bool saved = InventorySaveStore.TrySave(session.CaptureSave());
                if (!saved) return "[FAIL] save failed";

                var loaded = InventorySaveStore.TryLoad();
                if (loaded == null) return "[FAIL] load returned null";

                var restored = InventoryHostSession.Create(dataDirectory, seedWhenNoSave: false);
                restored.RestoreSave(loaded);
                if (restored.Inventory.CountById("clean_water") != 5 || restored.Inventory.CountById("canned_food") != 3)
                    return "[FAIL] restored item counts did not match saved state";

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
