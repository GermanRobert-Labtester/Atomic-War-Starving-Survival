using System;
using System.IO;
using Godot;

namespace AtomicWar.GodotApp
{
    public static class HoldfastTradeSaveStoreSelfTest
    {
        public static int Run()
        {
            int passed = 0;
            int total = 0;

            void Check(bool condition, string name)
            {
                total++;
                if (condition)
                {
                    passed++;
                    GD.Print($"  [PASS] {name}");
                }
                else
                {
                    GD.Print($"  [FAIL] {name}");
                }
            }

            GD.Print("[HoldfastTradeSaveStoreSelfTest] begin");

            string tempPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "holdfast_test_" + Guid.NewGuid().ToString("N") + ".json");
            string backupPath = tempPath + ".bak";

            try
            {
                // Test 1: Load corrupted main with no backup
                File.WriteAllText(tempPath, "{ corrupted json... }");
                var result = HoldfastTradeSaveStore.TryLoad(tempPath);
                Check(result == null, "TryLoad returns null on corrupt primary with no backup");

                // Test 2: Load corrupted main and corrupted backup
                File.WriteAllText(backupPath, "{ corrupted backup json... }");
                var result2 = HoldfastTradeSaveStore.TryLoad(tempPath);
                Check(result2 == null, "TryLoad returns null when both primary and backup are corrupt");

                // Test 3: Load corrupted main but recover from valid backup
                if (File.Exists(tempPath)) File.Delete(tempPath);
                if (File.Exists(backupPath)) File.Delete(backupPath);

                var state = new Ashfall.Core.HoldfastTradeSaveState();
                state.value = 42;
                HoldfastTradeSaveStore.TrySave(state, backupPath); // write to backup
                File.WriteAllText(tempPath, "{ corrupted json... }"); // write bad main

                var result3 = HoldfastTradeSaveStore.TryLoad(tempPath);
                Check(result3 != null && result3.value == 42, "TryLoad recovers valid backup when primary is corrupt");
            }
            catch (Exception ex)
            {
                GD.PrintErr("[HoldfastTradeSaveStoreSelfTest] error: " + ex);
            }
            finally
            {
                if (File.Exists(tempPath)) File.Delete(tempPath);
                if (File.Exists(backupPath)) File.Delete(backupPath);
                string dir = Path.GetDirectoryName(tempPath);
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                {
                    foreach (var file in Directory.GetFiles(dir, Path.GetFileName(tempPath) + ".corrupt-*"))
                    {
                        File.Delete(file);
                    }
                }
            }

            bool ok = passed == total && total > 0;
            return HostCli.EmitSummary("holdfast_trade_save_selftest", ok, ok ? 0 : 1, passed, total - passed);
        }
    }
}
