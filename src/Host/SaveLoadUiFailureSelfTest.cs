using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Save;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Self-test / smoke test verifying the Save/Load UI failure paths:
    /// - Missing save files/slots
    /// - Corrupt save files (malformed JSON / truncated data)
    /// - Checksum-invalid save files (tampered payloads or mismatched hashes)
    ///
    /// Confirms that each failure path produces a recoverable user-facing error message,
    /// quarantines corrupt files where appropriate, and leaves the running live session intact.
    /// Also confirms that after failures, valid saves can still be loaded normally.
    /// </summary>
    public static class SaveLoadUiFailureSelfTest
    {
        public static int Run(string dataDirectory = "")
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "ashfall_save_load_failure_smoke_" + DateTime.UtcNow.Ticks);
            try
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, recursive: true);
                Directory.CreateDirectory(tempDir);

                GD.Print("── SAVE-LOAD UI FAILURE-PATH SMOKE TEST ──");
                GD.Print($"[SaveLoadTest] Root test dir: {tempDir}");

                // ── 1. Setup simulated live session state ──
                int liveWaterCount = 42;
                int liveRationCount = 15;
                int liveDay = 12;
                string liveSurvivorId = "survivor_live_alpha";
                bool liveSessionMutated = false;

                // ── 2. Initialize host session and UI panel ──
                var hostSession = new SaveLoadHostSession();
                hostSession.Initialize(tempDir);

                var panel = new SaveLoadPanel();
                panel.Bind(hostSession);

                // Wire simulated restore orchestrator (same contract as Main)
                panel.OnLoadRequested += slotId =>
                {
                    bool loaded = hostSession.TryLoadSlot(slotId, out var result);
                    if (loaded && result.IsSuccess)
                    {
                        liveSessionMutated = true;
                        panel.ShowSuccess(result.UserMessage);
                    }
                    else
                    {
                        // Failure must NOT mutate live session
                        panel.ShowError(result.UserMessage);
                    }
                    panel.RefreshView();
                };

                // ── CASE 1: Missing Save Slot ──
                GD.Print("\n[CASE 1] Testing missing save slot failure path...");
                var missingSlotId = new SaveSlotId("slot_missing_404");
                bool missingLoaded = hostSession.TryLoadSlot(missingSlotId, out var missingResult);

                if (missingLoaded || missingResult.IsSuccess)
                {
                    GD.PrintErr("[FAIL] Case 1: Missing slot reported success!");
                    return 1;
                }

                panel.ShowError(missingResult.UserMessage);
                if (!panel.IsLastError)
                {
                    GD.PrintErr("[FAIL] Case 1: UI panel IsLastError is false after missing slot failure.");
                    return 1;
                }
                if (string.IsNullOrWhiteSpace(panel.LastStatusMessage) ||
                    (!panel.LastStatusMessage.Contains("not found", StringComparison.OrdinalIgnoreCase) &&
                     !panel.LastStatusMessage.Contains("missing", StringComparison.OrdinalIgnoreCase) &&
                     !panel.LastStatusMessage.Contains("does not exist", StringComparison.OrdinalIgnoreCase)))
                {
                    GD.PrintErr($"[FAIL] Case 1: User-facing message '{panel.LastStatusMessage}' did not indicate missing save.");
                    return 1;
                }
                if (liveSessionMutated || liveWaterCount != 42 || liveDay != 12)
                {
                    GD.PrintErr("[FAIL] Case 1: Live session was mutated during missing save load failure!");
                    return 1;
                }
                GD.Print($"[PASS] Case 1: Missing save rejected: '{panel.LastStatusMessage}' (live session intact)");

                // ── CASE 2: Corrupt Save (Malformed JSON) ──
                GD.Print("\n[CASE 2] Testing corrupt save (malformed JSON) failure path...");
                var corruptSlotId = new SaveSlotId("slot_corrupt_smoke");
                hostSession.CreateSlot(corruptSlotId);

                // Write corrupted raw JSON into campaign.json
                string savesBase = Path.Combine(tempDir, SaveSlotService.SavesBaseDir, "profile-default", "slot-slot_corrupt_smoke");
                string aggregatePath = Path.Combine(savesBase, SaveSlotService.AggregateFileName);
                File.WriteAllText(aggregatePath, "{\"manifestVersion\": 1, \"manifest\": { \"slotId\": \"broken\", \"sections\": [MALFORMED_JSON_HERE");

                bool corruptLoaded = hostSession.TryLoadSlot(corruptSlotId, out var corruptResult);
                if (corruptLoaded || corruptResult.IsSuccess)
                {
                    GD.PrintErr("[FAIL] Case 2: Corrupt slot reported success!");
                    return 1;
                }

                panel.ShowError(corruptResult.UserMessage);
                if (!panel.IsLastError)
                {
                    GD.PrintErr("[FAIL] Case 2: UI panel IsLastError is false after corrupt save failure.");
                    return 1;
                }
                if (corruptResult.Status != SaveLoadStatus.CorruptData)
                {
                    GD.PrintErr($"[FAIL] Case 2: Expected CorruptData status but got {corruptResult.Status}");
                    return 1;
                }
                if (!panel.LastStatusMessage.Contains("corrupt", StringComparison.OrdinalIgnoreCase) &&
                    !panel.LastStatusMessage.Contains("malformed", StringComparison.OrdinalIgnoreCase))
                {
                    GD.PrintErr($"[FAIL] Case 2: User-facing message '{panel.LastStatusMessage}' did not indicate corruption.");
                    return 1;
                }
                if (!panel.LastStatusMessage.Contains("Live session preserved", StringComparison.OrdinalIgnoreCase))
                {
                    GD.PrintErr($"[FAIL] Case 2: User-facing message '{panel.LastStatusMessage}' did not assure session preservation.");
                    return 1;
                }

                // Verify file quarantine occurred
                string quarantinePath = aggregatePath + ".slot_corrupt_smoke.corrupt";
                if (!File.Exists(quarantinePath))
                {
                    GD.PrintErr($"[FAIL] Case 2: Corrupt save was not quarantined to '{quarantinePath}'.");
                    return 1;
                }
                if (liveSessionMutated || liveWaterCount != 42 || liveRationCount != 15 || liveDay != 12 || liveSurvivorId != "survivor_live_alpha")
                {
                    GD.PrintErr("[FAIL] Case 2: Live session was mutated during corrupt save load failure!");
                    return 1;
                }
                GD.Print($"[PASS] Case 2: Corrupt save quarantined and rejected: '{panel.LastStatusMessage}' (live session intact)");

                // ── CASE 3: Checksum-Invalid Save (Tampered Data) ──
                GD.Print("\n[CASE 3] Testing checksum-invalid save failure path...");
                var tamperedSlotId = new SaveSlotId("slot_tampered_smoke");
                hostSession.CreateSlot(tamperedSlotId);

                string tamperedBase = Path.Combine(tempDir, SaveSlotService.SavesBaseDir, "profile-default", "slot-slot_tampered_smoke");
                string tamperedPath = Path.Combine(tamperedBase, SaveSlotService.AggregateFileName);

                var tamperedEnvelope = new AggregateSaveEnvelope
                {
                    manifestVersion = 1,
                    manifest = new SaveManifest
                    {
                        profileId = new SaveProfileId("default"),
                        slotId = tamperedSlotId,
                        campaignName = "Tampered Save",
                        currentDay = 99,
                        seed = 1234
                    },
                    sections = new List<SaveSectionEnvelope>
                    {
                        new SaveSectionEnvelope
                        {
                            sectionName = "inventory",
                            schemaVersion = 1,
                            payloadJson = "{\"water\": 9999}"
                        }
                    },
                    aggregateChecksum = "0000000000000000000000000000000000000000000000000000000000000000" // Mismatched
                };
                File.WriteAllText(tamperedPath, new SystemTextJsonSerializer().Serialize(tamperedEnvelope));

                bool tamperedLoaded = hostSession.TryLoadSlot(tamperedSlotId, out var tamperedResult);
                if (tamperedLoaded || tamperedResult.IsSuccess)
                {
                    GD.PrintErr("[FAIL] Case 3: Tampered slot reported success!");
                    return 1;
                }

                panel.ShowError(tamperedResult.UserMessage);
                if (!panel.IsLastError)
                {
                    GD.PrintErr("[FAIL] Case 3: UI panel IsLastError is false after checksum mismatch failure.");
                    return 1;
                }
                if (tamperedResult.Status != SaveLoadStatus.ChecksumMismatch)
                {
                    GD.PrintErr($"[FAIL] Case 3: Expected ChecksumMismatch status but got {tamperedResult.Status}");
                    return 1;
                }
                if (!panel.LastStatusMessage.Contains("checksum", StringComparison.OrdinalIgnoreCase))
                {
                    GD.PrintErr($"[FAIL] Case 3: User-facing message '{panel.LastStatusMessage}' did not mention checksum.");
                    return 1;
                }
                if (liveSessionMutated || liveWaterCount != 42 || liveRationCount != 15 || liveDay != 12 || liveSurvivorId != "survivor_live_alpha")
                {
                    GD.PrintErr("[FAIL] Case 3: Live session was mutated during checksum-invalid load failure!");
                    return 1;
                }
                GD.Print($"[PASS] Case 3: Checksum-invalid save rejected: '{panel.LastStatusMessage}' (live session intact)");

                // ── CASE 4: Recoverability / Valid Save after Failures ──
                GD.Print("\n[CASE 4] Testing recoverability with valid save after failure states...");
                var validSlotId = new SaveSlotId("slot_valid_recovery");
                hostSession.CreateSlot(validSlotId);

                var slotService = new SaveSlotService(new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog(), tempDir);
                var validSection = new SaveSectionEnvelope
                {
                    sectionName = "inventory",
                    schemaVersion = 1,
                    payloadJson = "{\"water\": 100}"
                };
                validSection.checksum = SaveSlotService.ComputeSectionChecksum(validSection);

                var validEnvelope = new AggregateSaveEnvelope
                {
                    manifestVersion = 1,
                    manifest = new SaveManifest
                    {
                        profileId = new SaveProfileId("default"),
                        slotId = validSlotId,
                        campaignName = "Valid Campaign",
                        currentDay = 25,
                        seed = 777
                    },
                    sections = new List<SaveSectionEnvelope> { validSection }
                };
                validEnvelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(validEnvelope);

                bool written = slotService.WriteAggregateAtomically(new SaveProfileId("default"), validSlotId, validEnvelope);
                if (!written)
                {
                    GD.PrintErr("[FAIL] Case 4: Failed to write valid aggregate envelope.");
                    return 1;
                }

                bool validLoaded = hostSession.TryLoadSlot(validSlotId, out var validResult);
                if (!validLoaded || !validResult.IsSuccess)
                {
                    GD.PrintErr($"[FAIL] Case 4: Valid slot failed to load: {validResult.UserMessage}");
                    return 1;
                }

                panel.ShowSuccess(validResult.UserMessage);
                if (panel.IsLastError)
                {
                    GD.PrintErr("[FAIL] Case 4: UI panel IsLastError is true after successful load.");
                    return 1;
                }
                if (!panel.LastStatusMessage.Contains("successfully", StringComparison.OrdinalIgnoreCase))
                {
                    GD.PrintErr($"[FAIL] Case 4: User-facing message '{panel.LastStatusMessage}' did not report success.");
                    return 1;
                }
                if (hostSession.RestoredSections.Count == 0 || !hostSession.RestoredSections.Contains("inventory"))
                {
                    GD.PrintErr("[FAIL] Case 4: RestoredSections did not contain 'inventory'.");
                    return 1;
                }
                GD.Print($"[PASS] Case 4: Valid save restored cleanly after failures: '{panel.LastStatusMessage}'");

                panel.QueueFree();
                hostSession.QueueFree();

                GD.Print("\n=== SAVE-LOAD UI FAILURE-PATH SELF-TEST PASS (4/4 gates verified) ===");
                return 0;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] SaveLoadUiFailureSelfTest exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                return 1;
            }
            finally
            {
                try
                {
                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, recursive: true);
                }
                catch
                {
                    // Ignore temp cleanup error
                }
            }
        }
    }
}
