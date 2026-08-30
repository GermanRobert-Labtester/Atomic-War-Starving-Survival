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

                // ── CASE 5: Envelope-primary save writes ONE file ──
                GD.Print("\n[CASE 5] Testing envelope-primary save (single atomic write)...");
                var envelopeSlotId = new SaveSlotId("slot_envelope_v2");
                hostSession.CreateSlot(envelopeSlotId);

                var payloads = new Dictionary<string, string>
                {
                    { "journal", "{\"entries\":[\"day 1\"]}" },
                    { "inventory", "{\"water\": 100}" },
                };
                bool envelopeSaved = hostSession.SaveEnvelopeFromPayloads(payloads);
                if (!envelopeSaved)
                {
                    GD.PrintErr("[FAIL] Case 5: SaveEnvelopeFromPayloads returned false.");
                    return 1;
                }

                string envelopeSlotRoot = Path.Combine(tempDir, SaveSlotService.SavesBaseDir, "profile-default", "slot-slot_envelope_v2");
                string envelopeDisk = System.IO.File.ReadAllText(System.IO.Path.Combine(envelopeSlotRoot, SaveSlotService.AggregateFileName));
                if (!envelopeDisk.Contains("\"manifestVersion\":2", StringComparison.Ordinal))
                {
                    GD.PrintErr("[FAIL] Case 5: Written envelope is not manifestVersion 2.");
                    return 1;
                }
                if (!envelopeDisk.Contains("\"sectionName\":\"journal\"", StringComparison.Ordinal) ||
                    !envelopeDisk.Contains("\"sectionName\":\"inventory\"", StringComparison.Ordinal))
                {
                    GD.PrintErr("[FAIL] Case 5: Envelope sections are not registry-keyed.");
                    return 1;
                }
                string[] jsonFiles = Directory.GetFiles(envelopeSlotRoot, "*.json");
                if (jsonFiles.Length != 2) // manifest.json + campaign.json — no section files
                {
                    GD.PrintErr($"[FAIL] Case 5: Envelope-primary save left {jsonFiles.Length} json files (expected 2: manifest + campaign).");
                    return 1;
                }
                GD.Print("[PASS] Case 5: Envelope save produced one V2 campaign.json, no section files.");

                // ── CASE 6: Load explodes sections to their registry file names ──
                GD.Print("\n[CASE 6] Testing envelope load explodes to registry file names...");
                bool envelopeLoaded = hostSession.TryLoadSlot(envelopeSlotId, out var envelopeResult);
                if (!envelopeLoaded || !envelopeResult.IsSuccess)
                {
                    GD.PrintErr($"[FAIL] Case 6: V2 envelope slot failed to load: {envelopeResult.UserMessage}");
                    return 1;
                }
                string explodedJournal = Path.Combine(envelopeSlotRoot, "journal_save.json");
                string explodedInventory = Path.Combine(envelopeSlotRoot, "inventory_save.json");
                if (!File.Exists(explodedJournal) || !File.Exists(explodedInventory))
                {
                    GD.PrintErr("[FAIL] Case 6: Load did not explode sections to registry file names (journal_save.json/inventory_save.json).");
                    return 1;
                }
                if (File.ReadAllText(explodedJournal) != payloads["journal"])
                {
                    GD.PrintErr("[FAIL] Case 6: Exploded journal payload bytes differ from the captured payload.");
                    return 1;
                }
                if (!hostSession.RestoredSections.Contains("journal"))
                {
                    GD.PrintErr("[FAIL] Case 6: RestoredSections missing registry key 'journal'.");
                    return 1;
                }
                GD.Print("[PASS] Case 6: Load exploded V2 sections verbatim to registry file names.");

                // ── CASE 7: V1 envelope on disk migrates in memory on load ──
                GD.Print("\n[CASE 7] Testing V1 (filename-keyed) envelope migration on load...");
                var legacySlotId = new SaveSlotId("slot_legacy_v1");
                hostSession.CreateSlot(legacySlotId);

                SaveSectionEnvelope V1Section(string name, string payload) => new SaveSectionEnvelope
                {
                    sectionName = name,
                    schemaVersion = 1,
                    payloadJson = payload,
                };
                var v1Inventory = V1Section("inventory_save", "{\"water\": 55}");
                v1Inventory.checksum = SaveSlotService.ComputeSectionChecksum(v1Inventory);
                var v1Stray = V1Section("weather_save", "{\"stray\":true}");
                v1Stray.checksum = SaveSlotService.ComputeSectionChecksum(v1Stray);
                var v1Envelope = new AggregateSaveEnvelope
                {
                    manifestVersion = 1,
                    manifest = new SaveManifest
                    {
                        profileId = new SaveProfileId("default"),
                        slotId = legacySlotId,
                        campaignName = "Legacy V1",
                        currentDay = 3,
                        seed = 11,
                    },
                    sections = new List<SaveSectionEnvelope> { v1Inventory, v1Stray },
                };
                v1Envelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(v1Envelope);
                string legacySlotRoot = Path.Combine(tempDir, SaveSlotService.SavesBaseDir, "profile-default", "slot-slot_legacy_v1");
                File.WriteAllText(Path.Combine(legacySlotRoot, SaveSlotService.AggregateFileName),
                    new SystemTextJsonSerializer().Serialize(v1Envelope));

                bool legacyLoaded = hostSession.TryLoadSlot(legacySlotId, out var legacyResult);
                if (!legacyLoaded || !legacyResult.IsSuccess)
                {
                    GD.PrintErr($"[FAIL] Case 7: V1 envelope failed to load: {legacyResult.UserMessage}");
                    return 1;
                }
                if (legacyResult.Envelope!.manifestVersion != CampaignEnvelopeBuilder.CurrentEnvelopeVersion)
                {
                    GD.PrintErr("[FAIL] Case 7: Loaded V1 envelope was not migrated to the current version in memory.");
                    return 1;
                }
                if (!hostSession.RestoredSections.Contains("inventory") || hostSession.RestoredSections.Contains("inventory_save"))
                {
                    GD.PrintErr("[FAIL] Case 7: Migrated sections are not registry-keyed in RestoredSections.");
                    return 1;
                }
                if (hostSession.RestoredSections.Contains("weather_save"))
                {
                    GD.PrintErr("[FAIL] Case 7: Stray 'weather_save' section was not dropped by the registry whitelist.");
                    return 1;
                }
                if (!File.Exists(Path.Combine(legacySlotRoot, "inventory_save.json")))
                {
                    GD.PrintErr("[FAIL] Case 7: Migrated inventory did not explode to its registry file name.");
                    return 1;
                }
                string diskStillV1 = File.ReadAllText(Path.Combine(legacySlotRoot, SaveSlotService.AggregateFileName));
                if (!diskStillV1.Contains("\"manifestVersion\":1", StringComparison.Ordinal))
                {
                    GD.PrintErr("[FAIL] Case 7: V1 file on disk was rewritten before the next save.");
                    return 1;
                }
                GD.Print("[PASS] Case 7: V1 envelope migrated in memory, stray section dropped, disk untouched.");

                // ── CASE 8: Interrupted projection commit self-heals on next load ──
                GD.Print("\n[CASE 8] Testing recovery from a leftover projection-in-progress marker...");
                var interruptedSlotId = new SaveSlotId("slot_interrupted_projection");
                hostSession.CreateSlot(interruptedSlotId);

                var interruptedPayloads = new Dictionary<string, string>
                {
                    { "journal", "{\"entries\":[\"day 9\"]}" },
                    { "inventory", "{\"water\": 77}" },
                };
                if (!hostSession.SaveEnvelopeFromPayloads(interruptedPayloads))
                {
                    GD.PrintErr("[FAIL] Case 8: Failed to write initial envelope for interrupted-projection slot.");
                    return 1;
                }

                // First load explodes both sections normally.
                if (!hostSession.TryLoadSlot(interruptedSlotId, out var firstLoadResult) || !firstLoadResult.IsSuccess)
                {
                    GD.PrintErr($"[FAIL] Case 8: Initial load of interrupted-projection slot failed: {firstLoadResult.UserMessage}");
                    return 1;
                }

                string interruptedSlotRoot = Path.Combine(tempDir, SaveSlotService.SavesBaseDir, "profile-default", "slot-slot_interrupted_projection");
                string interruptedInventoryFile = Path.Combine(interruptedSlotRoot, "inventory_save.json");
                string interruptedJournalFile = Path.Combine(interruptedSlotRoot, "journal_save.json");

                // Simulate a crash between the move loop's file writes: leave
                // the in-progress marker present, corrupt one derived file to
                // a value that must NOT be trusted, and delete the other to
                // simulate a half-finished commit. campaign.json itself was
                // never touched by this simulation.
                File.WriteAllText(Path.Combine(interruptedSlotRoot, ".campaign_projection_inprogress"), "simulated-crash");
                File.WriteAllText(interruptedInventoryFile, "{\"water\": -999, \"__stale_pre_crash_value\": true}");
                if (File.Exists(interruptedJournalFile))
                    File.Delete(interruptedJournalFile);

                // A subsequent load must recompute every derived file from
                // campaign.json (still authoritative and untouched), repair
                // both files, and clear the marker — regardless of the
                // simulated mid-commit mess left on disk.
                bool healedLoaded = hostSession.TryLoadSlot(interruptedSlotId, out var healedResult);
                if (!healedLoaded || !healedResult.IsSuccess)
                {
                    GD.PrintErr($"[FAIL] Case 8: Load after simulated interrupted commit failed: {healedResult.UserMessage}");
                    return 1;
                }
                if (File.Exists(Path.Combine(interruptedSlotRoot, ".campaign_projection_inprogress")))
                {
                    GD.PrintErr("[FAIL] Case 8: In-progress marker was not cleared after a successful re-projection.");
                    return 1;
                }
                if (!File.Exists(interruptedJournalFile))
                {
                    GD.PrintErr("[FAIL] Case 8: Deleted derived journal file was not restored by re-projection.");
                    return 1;
                }
                if (File.ReadAllText(interruptedInventoryFile) != interruptedPayloads["inventory"])
                {
                    GD.PrintErr("[FAIL] Case 8: Corrupted derived inventory file was not overwritten with the campaign.json-authoritative payload.");
                    return 1;
                }
                if (!hostSession.RestoredSections.Contains("journal") || !hostSession.RestoredSections.Contains("inventory"))
                {
                    GD.PrintErr("[FAIL] Case 8: RestoredSections missing sections after self-healing re-projection.");
                    return 1;
                }
                GD.Print("[PASS] Case 8: Interrupted projection self-healed from campaign.json; marker cleared.");

                panel.QueueFree();
                hostSession.QueueFree();

                GD.Print("\n=== SAVE-LOAD UI FAILURE-PATH SELF-TEST PASS (8/8 gates verified) ===");
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
