// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// Plan 138 lifecycle proof. Exercises the production New Game and
        /// Continue routes in an isolated save root without relying on the
        /// broader combat/trade journey.
        /// </summary>
        private void RunStartingCohortLifecycleSelfTestAndQuit()
        {
            string tempDir = Path.Combine(
                Path.GetTempPath(),
                "ashfall_starting_cohort_lifecycle_" + DateTime.UtcNow.Ticks); // DETERMINISM_ALLOWLIST: Test harness temporary directory
            bool pass = true;
            void Check(bool condition, string name)
            {
                if (condition)
                    GD.Print($"  [PASS] {name}");
                else
                {
                    GD.PrintErr($"  [FAIL] {name}");
                    pass = false;
                }
            }

            try
            {
                Directory.CreateDirectory(tempDir);
                BuildUserInterface();
                _saveLoadHost = new SaveLoadHostSession();
                _saveLoadHost.Initialize(tempDir);
                AddChild(_saveLoadHost);

                var cohortCatalog = EnsureStartingCohortCatalog();
                _startingCohortSetupPanel.Bind(
                    cohortCatalog,
                    EnsureStartingSuppliesCatalog(),
                    EnsureDifficultyCatalog());
                _startingCohortSetupPanel.Open();
                Check(
                    _startingCohortSetupPanel.Visible &&
                    _startingCohortSetupPanel.GetChildCount() > 0,
                    "cohort selector opens with authored profile content");
                _startingCohortSetupPanel.Close();
                Check(
                    !_startingCohortSetupPanel.Visible,
                    "cohort selector cancel/close leaves campaign state untouched");
                GD.Print("[StartingCohortLifecycle] UI probe complete; starting campaign A.");

                // Campaign A: preserve an observable marker and a changed
                // survivor state so a later fresh run cannot accidentally
                // appear clean merely because both runs use defaults.
                StartNewGame(StartingCohortCatalog.StandardProfileId);
                var campaignA = _saveLoadHost.ActiveSlotId!.Value;
                Check(campaignA.Value == "slot_1", "first fresh campaign allocates slot_1");
                Check(_survivors.RosterState.Count == 3,
                    "Standard Holdfast initializes exactly three survivors");
                Check(
                    SetEquals(
                        _survivors.RosterState.Select(s => s.Id),
                        new[]
                        {
                            "survivor_dr_sarah_chen",
                            "survivor_gunner_mikhail",
                            "elena_vasquez"
                        }),
                    "Standard Holdfast preserves the legacy survivor IDs");
                _survivors.RosterState[0].Health = 42f;
                const string campaignMarker = "scrap_metal";
                int campaignABaselineMarker = _inventory.Inventory.CountById(campaignMarker);
                _inventory.Inventory.AddById(campaignMarker, 1);
                Check(SaveAll(playCue: false), "campaign A saves before starting campaign B");

                // Campaign B: this must create a new root, reset only memory,
                // and apply the selected alternate once.
                StartNewGame(
                    "cohort_repair_crew",
                    "origin_machine_room",
                    "difficulty_sparing");
                var campaignB = _saveLoadHost.ActiveSlotId!.Value;
                Check(campaignB.Value == "slot_2", "second fresh campaign allocates slot_2");
                Check(_saveLoadHost.GetSlots().Count >= 2,
                    "fresh campaign allocation preserves the previous slot root");
                Check(
                    DifficultyPresetId == "difficulty_sparing",
                    "selected difficulty becomes the active campaign authority");
                Check(_survivors.RosterState.Count == 3,
                    "alternate cohort initializes exactly three survivors");
                int campaignBBaselineMarker = _inventory.Inventory.CountById(campaignMarker);
                Check(
                    SetEquals(
                        _survivors.RosterState.Select(s => s.Id),
                        new[] { "jamie_chen", "casey_garcia", "hayden_reyes" }),
                    "Repair Crew applies the selected canonical roster");
                Check(
                    _survivors.Find("survivor_dr_sarah_chen") == null &&
                    _inventory.Inventory.CountById(campaignMarker) == campaignBBaselineMarker,
                    "fresh campaign does not inherit the old roster or inventory marker");
                Check(
                    _inventory.Inventory.CountById("battery") == 14 &&
                    _inventory.Inventory.CountById("scrap_mechanical") == 24,
                    "selected Machine-Room Holdout supplies are applied to the fresh campaign");
                Check(
                    _inventory.Inventory.CountById("canned_food") == 11 &&
                    _inventory.Inventory.CountById("iodine_pills") == 3,
                    "Sparing difficulty adds its authored starter supplies once");
                Check(SaveAll(playCue: false), "campaign B saves its alternate cohort");
                Check(
                    TryReadPersistedDifficulty(out string savedDifficulty) &&
                    savedDifficulty == "difficulty_sparing",
                    "campaign header persists the selected difficulty before restore");

                // Continue campaign A and prove its state is still present.
                Check(
                    TryLoadAndRestoreGame(campaignA, out string restoreA),
                    $"campaign A remains loadable after campaign B: {restoreA}");
                Check(
                    _survivors.Find("survivor_dr_sarah_chen") != null &&
                    _survivors.RosterState.Count == 3,
                    "restoring campaign A restores its original roster");
                Check(
                    _survivors.RosterState[0].Health == 42f ||
                    _survivors.Find("survivor_dr_sarah_chen")!.Health == 42f,
                    "restoring campaign A preserves its changed survivor state");
                Check(
                    _inventory.Inventory.CountById(campaignMarker) == campaignABaselineMarker + 1,
                    "restoring campaign A preserves its old-campaign inventory marker");

                // Replace campaign B's survivor payload with a valid, explicit
                // empty save. Restore must honor that payload rather than
                // reseeding Standard or the selected alternate.
                Check(
                    TryLoadAndRestoreGame(campaignB, out string restoreB),
                    $"campaign B can be selected for empty-roster probe: {restoreB}");
                Check(
                    DifficultyPresetId == "difficulty_sparing",
                    "campaign restore preserves its checksummed difficulty selection");
                var emptyPayloads = CurrentEnvelopePayloads();
                emptyPayloads["survivors"] = SurvivorsSaveStore.TryCapturePersisted(
                    new SurvivorsSaveState
                    {
                        roster = new SurvivorRosterState()
                    });
                Check(
                    _saveLoadHost.SaveEnvelopeFromPayloads(emptyPayloads),
                    "empty survivor roster payload commits as a valid campaign save");
                Check(
                    TryLoadAndRestoreGame(campaignB, out string restoreEmpty),
                    $"empty survivor roster save restores successfully: {restoreEmpty}");
                Check(
                    _survivors.RosterState.Count == 0 &&
                    _survivors.Roster.LivingCount == 0,
                    "restore honors an explicitly empty saved roster without reseeding");

                // A failed restore must not turn into New Game or reset the
                // currently active in-memory campaign.
                var missing = new SaveSlotId("slot_missing");
                bool failedRestore = !TryLoadAndRestoreGame(missing, out string failedMessage);
                Check(failedRestore, "missing restore is rejected");
                Check(
                    _saveLoadHost.ActiveSlotId == campaignB &&
                    _campaignInitializationMode == CampaignInitializationMode.Restore &&
                    _survivors.RosterState.Count == 0,
                    $"failed restore preserves the current campaign instead of starting New Game: {failedMessage}");

                // A third fresh run proves deterministic next-free allocation
                // after both prior roots exist and starts Standard again.
                StartNewGame();
                Check(
                    _saveLoadHost.ActiveSlotId?.Value == "slot_3",
                    "third fresh campaign allocates the next free deterministic slot");
                Check(
                    SetEquals(
                        _survivors.RosterState.Select(s => s.Id),
                        new[]
                        {
                            "survivor_dr_sarah_chen",
                            "survivor_gunner_mikhail",
                            "elena_vasquez"
                        }),
                    "direct New Game still defaults to Standard Holdfast");
                Check(
                    _inventory.Inventory.CountById("battery") == 4 &&
                    _inventory.Inventory.CountById("scrap_mechanical") == 6,
                    "direct New Game still defaults to Standard Holdfast supplies");

                HostCli.EmitSummary(
                    "starting_cohort_lifecycle_selftest",
                    pass,
                    pass ? 0 : 1);
                QuitUiTestAfterFrame(pass ? 0 : 1);
            }
            catch (Exception ex)
            {
                GD.PrintErr(
                    $"[FAIL] StartingCohortLifecycleSelfTest exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                HostCli.EmitSummary("starting_cohort_lifecycle_selftest", false, 1);
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
                    // Temp cleanup is best-effort; never mask the test result.
                }
            }
        }

        private Dictionary<string, string> CurrentEnvelopePayloads()
        {
            if (_saveLoadHost.ActiveEnvelope?.sections == null)
                throw new InvalidOperationException("No active campaign envelope is available.");

            return _saveLoadHost.ActiveEnvelope.sections.ToDictionary(
                section => section.sectionName,
                section => section.payloadJson,
                StringComparer.Ordinal);
        }

        private bool TryReadPersistedDifficulty(out string presetId)
        {
            presetId = string.Empty;
            if (!_saveLoadHost.TryGetSectionPayload("campaign_day", out string payload))
                return false;

            try
            {
                var header = CampaignDaySaveCodec.Decode(
                    payload,
                    new SystemTextJsonSerializer());
                presetId = header.difficulty_preset_id;
                return true;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[StartingCohortLifecycle] campaign header probe failed: " + ex.Message);
                return false;
            }
        }

        private static bool SetEquals(
            IEnumerable<string> actual,
            IEnumerable<string> expected)
        {
            return new HashSet<string>(actual ?? Array.Empty<string>(), StringComparer.Ordinal)
                .SetEquals(expected);
        }
    }
}
