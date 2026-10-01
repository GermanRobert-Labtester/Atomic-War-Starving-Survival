// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// Failure & restart journey (Task 9): prove the full loss → restart
        /// cycle through real production entry points only.
        ///
        ///   StartNewGame → survivor deaths through the SurvivorFateSystem
        ///   pipeline → OnLastSurvivorDied → ShowGameOver (terminal slot seal)
        ///   → ReturnToMenu → StartNewGame again (no stale state) → Continue
        ///   after a simulated crash → corrupt campaign.json → fail-closed
        ///   load with the live session intact → verified backup recovery.
        ///
        /// Every step uses the same callbacks the player-facing UI raises;
        /// no standalone fixtures. The save/load host points at an isolated
        /// temp directory so a real player's saves are never touched.
        /// </summary>
        private void RunFailureRestartSelfTestAndQuit()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "ashfall_failure_restart_" + DateTime.UtcNow.Ticks); // DETERMINISM_ALLOWLIST: Test harness temporary directory
            bool pass = true;
            void Check(bool cond, string name)
            {
                if (cond) GD.Print($"  [PASS] {name}");
                else { GD.PrintErr($"  [FAIL] {name}"); pass = false; }
            }

            try
            {
                Directory.CreateDirectory(tempDir);

                GD.Print("── FAILURE & RESTART SELF-TEST ──");

                // ── Boot: the same save/load host wiring _Ready() performs,
                // pointed at an isolated temp directory. ──
                BuildUserInterface();
                _saveLoadHost = new SaveLoadHostSession();
                _saveLoadHost.Initialize(tempDir);
                AddChild(_saveLoadHost);

                // ── Phase A: fresh campaign through the real entry point. ──
                StartNewGame();
                Check(_state == GameState.Playing, "New Game entered the Playing state");
                Check(_saveLoadHost.ActiveSlotId != null, "New Game selected an active save slot");
                SaveSlotId slotA = _saveLoadHost.ActiveSlotId!.Value;
                Check(_campaignDay != null && _survivors != null && _inventory != null,
                    "ComposeCampaign() constructed the real campaign services");
                Check(_campaignDay!.Calendar.CurrentDay == 1,
                    $"fresh campaign starts on day 1 (was {_campaignDay.Calendar.CurrentDay})");
                Check(_survivors!.RosterState.Count == 3 &&
                    _survivors.RosterState.All(s => s == null || s.IsAliveState),
                    "fresh campaign holds three living survivors");

                // A real mutation + day advance so the first envelope carries
                // non-trivial state before the loss is sealed.
                SetupInventory();
                _inventory!.Inventory.Clear();
                _inventory.Add("canned_food", 5);
                TickSimDay(2);
                Check(_campaignDay.Calendar.CurrentDay == 2,
                    $"real day advance reached day 2 (was {_campaignDay.Calendar.CurrentDay})");
                Check(SaveAll(playCue: false), "SaveAll() committed the first campaign envelope");

                // ── Phase B: survivor deaths → campaign loss → game over. ──
                // ReportScriptedDeath is the narrative death seam; the fate
                // system is the idempotency authority and fires
                // OnLastSurvivorDied when the last roster member falls.
                var rosterIds = _survivors.RosterState
                    .Where(s => s != null && s.IsAliveState)
                    .Select(s => s!.Id)
                    .ToList();
                Check(rosterIds.Count == 3, "three living roster ids were captured for the loss cascade");
                foreach (string id in rosterIds)
                    ReportScriptedDeath(id, "failure-restart self-test cascade");

                Check(_state == GameState.GameOver,
                    "the last survivor's death moved the host to the GameOver state");
                Check(_gameOver.Visible, "ShowGameOver made the game-over screen visible");
                Check(!_mainMenu.Visible && !_dashboard.Visible,
                    "the menu and dashboard are hidden behind the game-over screen");
                Check(_survivorFate != null && _survivorFate.DeathCount == 3,
                    $"the fate ledger recorded all three deaths (got {_survivorFate?.DeathCount ?? -1})");

                var cardA = _saveLoadHost.BuildSlotCard(slotA);
                Check(cardA.IsTerminalIronMan,
                    "the lost campaign's slot was sealed terminal (TerminalLoss)");
                bool resurrected = TryLoadAndRestoreGame(slotA, out string resurrectMessage);
                Check(!resurrected,
                    $"a sealed terminal campaign cannot be continued through TryLoadAndRestoreGame ({resurrectMessage})");

                // ── Phase C: return to menu. ──
                ReturnToMenu();
                Check(_state == GameState.Menu, "ReturnToMenu moved the host to the Menu state");
                Check(_mainMenu.Visible, "the main menu is visible again");
                Check(!_dashboard.Visible && !_gameOver.Visible,
                    "the dashboard and game-over screens are hidden in the menu");
                Check(!AnyOverlayPanelOpen(), "no overlay panel survived the return to menu");
                Check(_saveLoadHost.BuildSlotCard(slotA).IsTerminalIronMan,
                    "the terminal seal survived the return to menu");

                // ── Phase D: new game from the menu — no stale state. ──
                StartNewGame();
                Check(_state == GameState.Playing, "the second New Game entered the Playing state");
                Check(_saveLoadHost.ActiveSlotId != null &&
                    _saveLoadHost.ActiveSlotId.Value.Value != slotA.Value,
                    "the second New Game allocated a fresh slot (previous campaign preserved)");
                SaveSlotId slotB = _saveLoadHost.ActiveSlotId!.Value;
                Check(_campaignDay!.Calendar.CurrentDay == 1,
                    $"the second campaign restarts on day 1 (was {_campaignDay.Calendar.CurrentDay})");
                Check(_survivors!.RosterState.Count == 3 &&
                    _survivors.RosterState.All(s => s == null || s.IsAliveState),
                    "the second campaign holds three living survivors — the dead roster did not leak");
                SetupSurvivorFate();
                var freshFate = _survivorFate;
                Check(freshFate != null && freshFate.DeathCount == 0,
                    $"the fate ledger is clean in the new campaign (got {freshFate?.DeathCount ?? -1})");
                Check(string.IsNullOrEmpty(_lastCampaignPanelId),
                    "the resumed-panel hint did not leak from the lost campaign");
                Check(!_gameOver.Visible && !_mainMenu.Visible,
                    "the game-over screen and menu are hidden in the new campaign");
                Check(_saveLoadHost.BuildSlotCard(slotA).IsTerminalIronMan,
                    "the previous terminal memorial stayed sealed on disk");

                // ── Phase E: continue after a simulated crash. ──
                // TryLoadAndRestoreGame performs the same full in-memory
                // reset a fresh process would need, so driving it here is the
                // in-process equivalent of relaunching and pressing Continue.
                _inventory.Add("canned_food", 5);
                var crashConsume = _inventory.ConsumeResult("canned_food");
                Check(crashConsume.IsSuccess, "a real consume action mutated the second campaign");
                TickSimDay(2);
                Check(SaveAll(playCue: false), "SaveAll() committed the crash-recovery envelope");
                bool crashRestored = TryLoadAndRestoreGame(slotB, out string crashMessage);
                Check(crashRestored, $"Continue restored the campaign after the simulated crash: {crashMessage}");
                Check(_campaignDay!.Calendar.CurrentDay == 2,
                    $"the crash-continue restored day 2 (got {_campaignDay.Calendar.CurrentDay})");
                var postCrashConsume = _inventory.ConsumeResult("canned_food");
                Check(postCrashConsume.IsSuccess,
                    "a further real action succeeds against the crash-restored session");

                // ── Phase F: corrupt-save recovery check. ──
                // A second generation on slot B rotates a verified backup;
                // corrupting campaign.json must fail the load closed (live
                // session intact) and offer the backup, and explicit recovery
                // must restore that older generation. No further day advance
                // here: an unmanaged crew can die mid-advance, which would
                // seal the slot through the (correct) game-over path instead
                // of exercising corruption recovery.
                _inventory.Add("canned_food", 5);
                int cannedBeforeSecondSave = _inventory.Inventory.CountById("canned_food");
                Check(SaveAll(playCue: false), "SaveAll() committed a second generation (backup rotation)");
                int liveDay = _campaignDay!.Calendar.CurrentDay;
                string aggregatePath = Path.Combine(tempDir, SaveSlotService.SavesBaseDir,
                    "profile-default", "slot-" + slotB.Value, SaveSlotService.AggregateFileName);
                Check(File.Exists(aggregatePath), "the campaign envelope exists on disk before corruption");
                File.WriteAllText(aggregatePath, "CORRUPTED-BY-FAILURE-RESTART-SELFTEST { not json");

                bool corruptLoad = TryLoadAndRestoreGame(slotB, out string corruptMessage);
                Check(!corruptLoad,
                    $"loading a corrupt envelope fails closed ({corruptMessage})");
                Check(_state == GameState.Playing && _campaignDay.Calendar.CurrentDay == liveDay,
                    $"the failed corrupt load left the live session intact (day {liveDay})");
                Check(_survivors != null && _survivors.RosterState.All(s => s == null || s.IsAliveState),
                    "the live roster survived the failed corrupt load");

                var backupPreview = _saveLoadHost.FindRecoverableBackup(slotB);
                Check(backupPreview.IsSuccess && backupPreview.Envelope != null,
                    $"a verified backup was offered for the corrupt campaign ({backupPreview.UserMessage})");
                int backupDay = backupPreview.Envelope?.manifest.currentDay ?? -1;

                var recovered = _saveLoadHost.RecoverBackup(slotB);
                Check(recovered.IsSuccess,
                    $"explicit backup recovery quarantined the corrupt envelope and restored the backup ({recovered.UserMessage})");
                bool afterRecovery = TryLoadAndRestoreGame(slotB, out string recoveryMessage);
                Check(afterRecovery,
                    $"Continue succeeded after backup recovery: {recoveryMessage}");
                Check(_campaignDay!.Calendar.CurrentDay == backupDay,
                    $"the recovered campaign resumed from the backup day ({backupDay}, got {_campaignDay.Calendar.CurrentDay})");
                Check(_inventory.Inventory.CountById("canned_food") < cannedBeforeSecondSave,
                    "the recovered session is the older backup generation (pre-top-up inventory)");
                var postRecoveryConsume = _inventory.ConsumeResult("canned_food");
                Check(postRecoveryConsume.IsSuccess,
                    "a further real action succeeds against the recovered session");

                HostCli.EmitSummary("failure_restart_selftest", pass, pass ? 0 : 1);
                QuitUiTestAfterFrame(pass ? 0 : 1);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] FailureRestartSelfTest exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                HostCli.EmitSummary("failure_restart_selftest", false, 1);
                QuitUiTestAfterFrame(1);
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
                    // Temp cleanup is best-effort; never let it mask the test result.
                }
            }
        }
    }
}
