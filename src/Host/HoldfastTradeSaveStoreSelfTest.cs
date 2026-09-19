// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;

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

            string baseDir = Directory.Exists(ProjectSettings.GlobalizePath("user://"))
                ? ProjectSettings.GlobalizePath("user://")
                : Path.GetTempPath();
            string tempPath = Path.Combine(baseDir, "holdfast_trade_test_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Test scratch file path
            string backupPath = tempPath + ".bak";

            try
            {
                // Test 1: Non-existent file returns null
                string nonExistentPath = tempPath + "_does_not_exist.json";
                Check(HoldfastTradeSaveStore.TryLoad(nonExistentPath) == null,
                    "TryLoad returns null on non-existent path");

                // Test 2: Empty / whitespace primary file returns null
                File.WriteAllText(tempPath, "   \n\t  ");
                Check(HoldfastTradeSaveStore.TryLoad(tempPath) == null,
                    "TryLoad returns null on empty/whitespace primary file");

                // Test 3: Load corrupted primary with no backup -> returns null and creates quarantine
                File.WriteAllText(tempPath, "{ corrupted json payload... }");
                var resultCorruptNoBak = HoldfastTradeSaveStore.TryLoad(tempPath);
                string dir = Path.GetDirectoryName(tempPath)!;
                string[] quarantined = Directory.GetFiles(dir, Path.GetFileName(tempPath) + ".corrupt-*");
                Check(resultCorruptNoBak == null && quarantined.Length > 0,
                    "TryLoad returns null on corrupt primary with no backup and creates quarantine file");

                // Clean quarantine files created in Test 3
                foreach (var qf in quarantined) { try { File.Delete(qf); } catch (Exception ex) { GD.PrintErr("[Cleanup] Best-effort quarantine file delete failed: " + ex.Message); } }

                // Test 4: Load corrupted primary and corrupted backup -> returns null
                File.WriteAllText(tempPath, "{ corrupted primary json... }");
                File.WriteAllText(backupPath, "{ corrupted backup json... }");
                var resultBothCorrupt = HoldfastTradeSaveStore.TryLoad(tempPath);
                Check(resultBothCorrupt == null,
                    "TryLoad returns null when both primary and backup are corrupt JSON");

                // Test 5: Load corrupted primary and empty backup -> returns null
                File.WriteAllText(tempPath, "{ corrupted primary json... }");
                File.WriteAllText(backupPath, "");
                var resultEmptyBak = HoldfastTradeSaveStore.TryLoad(tempPath);
                Check(resultEmptyBak == null,
                    "TryLoad returns null when primary is corrupt and backup is empty");

                // Test 6: Load corrupted primary and tampered backup (checksum mismatch in backup) -> returns null
                var backupState = new HoldfastTradeSaveState { schemaVersion = 1, value = 777 };
                HoldfastTradeSaveStore.TrySave(backupState, backupPath);
                string bakRaw = File.ReadAllText(backupPath);
                string tamperedBak = bakRaw.Replace("777", "999");
                File.WriteAllText(backupPath, tamperedBak);
                File.WriteAllText(tempPath, "{ corrupted primary json... }");
                var resultTamperedBak = HoldfastTradeSaveStore.TryLoad(tempPath);
                Check(resultTamperedBak == null,
                    "TryLoad returns null when primary is corrupt and backup has tampered checksum");

                // Test 7: Load corrupted primary but recover from valid backup
                if (File.Exists(tempPath)) File.Delete(tempPath);
                if (File.Exists(backupPath)) File.Delete(backupPath);
                var validBackupState = new HoldfastTradeSaveState
                {
                    schemaVersion = 1,
                    value = 42,
                    held = new Dictionary<string, int> { ["scrap_metal"] = 10 }
                };
                HoldfastTradeSaveStore.TrySave(validBackupState, backupPath);
                File.WriteAllText(tempPath, "{ corrupted primary json... }");
                var resultRecovered = HoldfastTradeSaveStore.TryLoad(tempPath);
                Check(resultRecovered != null && resultRecovered.value == 42 && resultRecovered.held["scrap_metal"] == 10,
                    "TryLoad recovers valid backup when primary is corrupt");

                // Test 8: Tampered primary checksum with no backup -> returns null
                if (File.Exists(tempPath)) File.Delete(tempPath);
                if (File.Exists(backupPath)) File.Delete(backupPath);
                var validState = new HoldfastTradeSaveState { schemaVersion = 1, value = 500 };
                HoldfastTradeSaveStore.TrySave(validState, tempPath);
                string rawPrimary = File.ReadAllText(tempPath);
                string tamperedPrimary = rawPrimary.Replace("500", "600");
                File.WriteAllText(tempPath, tamperedPrimary);
                Check(HoldfastTradeSaveStore.TryLoad(tempPath) == null,
                    "TryLoad returns null when primary checksum does not match tampered state");

                // Test 9: Stripped checksum in primary -> returns null
                string strippedChecksum = rawPrimary.Replace(SaveChecksum.Compute(validState), "");
                File.WriteAllText(tempPath, strippedChecksum);
                Check(HoldfastTradeSaveStore.TryLoad(tempPath) == null,
                    "TryLoad returns null when primary checksum is empty/missing");

                // Test 10: Clean save and load round-trip with full trade inventory
                if (File.Exists(tempPath)) File.Delete(tempPath);
                if (File.Exists(backupPath)) File.Delete(backupPath);
                var richState = new HoldfastTradeSaveState
                {
                    schemaVersion = 1,
                    value = 1250,
                    held = new Dictionary<string, int> { ["scrap_metal"] = 25, ["clean_water"] = 8 },
                    stock = new Dictionary<string, int> { ["first_aid"] = 3, ["ammo_9mm"] = 50 }
                };
                bool savedOk = HoldfastTradeSaveStore.TrySave(richState, tempPath);
                var loadedState = HoldfastTradeSaveStore.TryLoad(tempPath);
                Check(savedOk
                    && loadedState != null
                    && loadedState.value == 1250
                    && loadedState.held["scrap_metal"] == 25
                    && loadedState.held["clean_water"] == 8
                    && loadedState.stock["first_aid"] == 3
                    && loadedState.stock["ammo_9mm"] == 50,
                    "TrySave and TryLoad perform clean round-trip with full trade inventories");

                // Test 11: TrySave with null state returns false
                Check(!HoldfastTradeSaveStore.TrySave(null!, tempPath),
                    "TrySave returns false when given null state");

                // Test 12: Backup rotation preserves the oldest snapshot
                if (File.Exists(tempPath)) File.Delete(tempPath);
                if (File.Exists(backupPath)) File.Delete(backupPath);
                var snapA = new HoldfastTradeSaveState { schemaVersion = 1, value = 100 };
                HoldfastTradeSaveStore.TrySave(snapA, tempPath);
                Check(!File.Exists(backupPath), "First save does not create backup yet");

                var snapB = new HoldfastTradeSaveState { schemaVersion = 1, value = 200 };
                HoldfastTradeSaveStore.TrySave(snapB, tempPath);
                Check(File.Exists(backupPath) && HoldfastTradeSaveStore.TryLoad(backupPath)?.value == 100,
                    "Second save rotates original snapshot A (value=100) into backup");

                var snapC = new HoldfastTradeSaveState { schemaVersion = 1, value = 300 };
                HoldfastTradeSaveStore.TrySave(snapC, tempPath);
                Check(HoldfastTradeSaveStore.TryLoad(backupPath)?.value == 100
                    && HoldfastTradeSaveStore.TryLoad(tempPath)?.value == 300,
                    "Third save preserves oldest snapshot A (value=100) in backup and updates primary to C (value=300)");

                // Test 13: Direct in-memory capture and restore
                string captured = HoldfastTradeSaveStore.TryCaptureDirect(richState);
                var restored = HoldfastTradeSaveStore.TryRestoreDirect(captured);
                Check(restored != null && restored.value == 1250 && restored.held["scrap_metal"] == 25,
                    "TryCaptureDirect and TryRestoreDirect round-trip in memory");
            }
            catch (Exception ex)
            {
                GD.PrintErr("[HoldfastTradeSaveStoreSelfTest] error: " + ex);
                Check(false, "HoldfastTradeSaveStoreSelfTest threw unhandled: " + ex.Message);
            }
            finally
            {
                if (File.Exists(tempPath)) { try { File.Delete(tempPath); } catch (Exception ex) { GD.PrintErr("[Cleanup] Best-effort temp file delete failed: " + ex.Message); } }
                if (File.Exists(backupPath)) { try { File.Delete(backupPath); } catch (Exception ex) { GD.PrintErr("[Cleanup] Best-effort backup file delete failed: " + ex.Message); } }
                string dir = Path.GetDirectoryName(tempPath);
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                {
                    foreach (var file in Directory.GetFiles(dir, Path.GetFileNameWithoutExtension(tempPath) + "*"))
                    {
                        try { File.Delete(file); } catch (Exception ex) { GD.PrintErr("[Cleanup] Best-effort wildcard file delete failed: " + ex.Message); }
                    }
                }
            }

            bool ok = passed == total && total > 0;
            return HostCli.EmitSummary("holdfast_trade_save_selftest", ok, ok ? 0 : 1, passed, total - passed);
        }
    }
}
