// SPDX-License-Identifier: MIT
// ============================================================================
// Suggestion #1 — UI-built headless simulation harness.
//
// The full ~60-owner campaign day could previously only be exercised for the
// 2-owner inventory probe, because production owners such as
// `narrative_quests_verdict` (TickVerdict) and `holdfast_core`
// (SetupVerdict) dereference the fully built UI surface. This harness is the
// missing instrument: it builds the real UI/composition root, then drives the
// production CampaignDayCoordinator through
//
//   1. a deterministic multi-day run,
//   2. mid-run fault injection + same-day retry (fail-closed rollback),
//   3. a persisted-envelope checksum comparison (in-memory envelope vs the
//      campaign.json written to disk), and
//   4. repeated soak replays of the fault/retry cycle.
//
// It is validation-only: it registers no production owner, adds no gameplay
// authority, and unregisters its temporary fault owner and tears down every
// added node/session before it quits.
// ============================================================================

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
        private const int UiHarnessWarmupDays = 3;
        private const int UiHarnessSoakIterations = 3;
        private const string UiHarnessFaultOwnerId = "harness_fault_injection";

        /// <summary>
        /// Temporary day owner whose only job is to fail once on demand so the
        /// coordinator's fail-closed retry path can be exercised over the full
        /// production owner set. Registered at phase 99 so every production
        /// owner has already ticked when the fault fires.
        /// </summary>
        private sealed class UiHarnessFaultOwner : IDayAdvanceOwner
        {
            public bool Armed;
            public int FaultsInjected;

            public void CapturePreDaySnapshot(int day) { }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                if (!Armed) return;
                FaultsInjected++;
                throw new InvalidOperationException("ui-composition-harness injected owner fault");
            }
        }

        [Serializable]
        private sealed class UiHarnessCheck
        {
            public string id = string.Empty;
            public bool passed;
            public string evidence = string.Empty;
        }

        [Serializable]
        private sealed class UiHarnessArtifact
        {
            public int schema_version = 1;
            public int owner_count;
            public int restorable_owner_count;
            public List<string> non_restorable_owners = new();
            public int warmup_days;
            public List<int> warmup_days_advanced = new();
            public int soak_iterations;
            public int faults_injected;
            public int failed_closed_count;
            public int retry_succeeded_count;
            public int no_double_application_count;
            public int envelope_section_count;
            public bool envelope_aggregate_integrity;
            public bool envelope_section_integrity;
            public bool envelope_disk_matches_memory;
            public string envelope_aggregate_checksum = string.Empty;
            public int persisted_day;
            public List<UiHarnessCheck> checks = new();
        }

        /// <summary>
        /// Run the UI-built headless simulation harness and quit. The summary
        /// id is <c>ui_composition_harness_selftest</c>.
        /// </summary>
        private void RunUiCompositionHarnessAndQuit()
        {
            string scratchRoot = Path.Combine(Path.GetTempPath(), "ashfall_ui_composition_harness");
            var checks = new List<UiHarnessCheck>();
            bool pass = true;

            void Check(bool ok, string label, string evidence = "")
            {
                checks.Add(new UiHarnessCheck { id = label, passed = ok, evidence = evidence });
                if (ok)
                    GD.Print($"[PASS] {label}{(string.IsNullOrEmpty(evidence) ? string.Empty : $" ({evidence})")}");
                else
                {
                    pass = false;
                    GD.PrintErr($"[FAIL] {label}{(string.IsNullOrEmpty(evidence) ? string.Empty : $" ({evidence})")}");
                }
            }

            var artifact = new UiHarnessArtifact
            {
                warmup_days = UiHarnessWarmupDays,
                soak_iterations = UiHarnessSoakIterations
            };
            var fault = new UiHarnessFaultOwner();
            IDayAdvancePersistence? persistence = null;
            bool faultRegistered = false;

            try
            {
                if (Directory.Exists(scratchRoot))
                    Directory.Delete(scratchRoot, recursive: true);
                Directory.CreateDirectory(scratchRoot);

                // ── Phase 1: build the real UI and compose the full campaign. ──
                BuildUserInterface();
                _saveLoadHost = new SaveLoadHostSession();
                _saveLoadHost.Initialize(scratchRoot);
                AddChild(_saveLoadHost);

                StartNewGame();

                Check(_campaignDay != null && _campaignDayHost != null,
                    "full campaign composition is live after StartNewGame()");
                Check(_inventory != null && _survivors != null && _world != null,
                    "core campaign services are composed");

                if (_campaignDay == null || _campaignDayHost == null || _inventory == null)
                {
                    // The finally block emits the FAIL summary exactly once.
                    return;
                }

                // ── Owner census: how much of the full composition participates
                // in fail-closed rollback. Informational, but the harness is the
                // only place this is measurable over the real UI-built owner set. ──
                var owners = _campaignDay.Owners;
                artifact.owner_count = owners.Count;
                foreach (var owner in owners)
                {
                    if (owner is IPreDaySnapshotRestore)
                        artifact.restorable_owner_count++;
                    else
                        artifact.non_restorable_owners.Add(owner.GetType().Name);
                }
                Check(artifact.owner_count >= 40,
                    "full production owner set composed",
                    $"owners={artifact.owner_count}");
                Check(artifact.restorable_owner_count > 0,
                    "at least one owner participates in fail-closed rollback",
                    $"restorable={artifact.restorable_owner_count}/{artifact.owner_count}");

                // ── Phase 2: deterministic multi-day run. ──
                persistence = new CampaignDayPersistenceAdapter(this);
                int startDay = _campaignDay.Calendar.CurrentDay;
                for (int i = 1; i <= UiHarnessWarmupDays; i++)
                {
                    StabilizeUiHarnessRoster();
                    int day = _campaignDay.LastAdvancedDay > 0
                        ? _campaignDay.LastAdvancedDay + 1
                        : startDay + i;
                    var args = _campaignDayHost.Advance(day, persistence);
                    bool ok = args != null && args.Succeeded;
                    string evidence = args == null
                        ? "coordinator rejected the day"
                        : ok
                            ? $"owners={args.OwnerCount} events={args.AllEvents().Count()}"
                            : string.Join("; ", args.FailedReports.Select(r => r.OwnerId + ":" + r.FailureMessage));
                    Check(ok, $"warmup day {day} advanced through the full composition", evidence);
                    if (!ok) break;
                    artifact.warmup_days_advanced.Add(day);
                }
                Check(artifact.warmup_days_advanced.Count == UiHarnessWarmupDays,
                    "multi-day deterministic run reached its target",
                    $"days={string.Join(",", artifact.warmup_days_advanced)}");

                // Seed a known surplus so ration consumption is measurable.
                int held = _inventory.Inventory.CountById("canned_food");
                if (held < 20)
                    _inventory.Add("canned_food", 20 - held);

                // ── Phase 3+4: soak replays of the fault-injection / same-day
                // retry cycle. The fault owner runs last (phase 99), so the
                // failed attempt contains every production owner's tick and the
                // retry's rollback contract is exercised end-to-end. ──
                _campaignDay.Register(UiHarnessFaultOwnerId, fault, phase: 99);
                faultRegistered = true;

                for (int iteration = 0; iteration < UiHarnessSoakIterations; iteration++)
                {
                    StabilizeUiHarnessRoster();
                    int day = _campaignDay.LastAdvancedDay + 1;
                    int before = _inventory.Inventory.CountById("canned_food");

                    fault.Armed = true;
                    var failed = _campaignDayHost.Advance(day, persistence);
                    bool failedClosed = failed != null && failed.HasFailures
                        && failed.FailedReports.Any(r => r.OwnerId == UiHarnessFaultOwnerId);
                    int afterFail = _inventory.Inventory.CountById("canned_food");
                    if (!failedClosed)
                    {
                        Check(false, $"soak {iteration} day {day} fault failed closed",
                            failed == null ? "coordinator rejected the day"
                            : $"failed={string.Join(",", failed.FailedReports.Select(r => r.OwnerId))}");
                        break;
                    }
                    artifact.failed_closed_count++;

                    fault.Armed = false;
                    var retried = _campaignDayHost.Advance(day, persistence);
                    bool retrySucceeded = retried != null && retried.Succeeded;
                    int afterRetry = _inventory.Inventory.CountById("canned_food");
                    if (!retrySucceeded)
                    {
                        Check(false, $"soak {iteration} day {day} same-day retry succeeded",
                            retried == null ? "coordinator rejected the retry"
                            : string.Join("; ", retried.FailedReports.Select(r => r.OwnerId + ":" + r.FailureMessage)));
                        break;
                    }
                    artifact.retry_succeeded_count++;

                    // Never below the failed attempt's own consumption: a value
                    // below after_fail proves the retry double-applied a
                    // restorable owner's mutation.
                    bool noDoubleApply = afterRetry >= afterFail;
                    Check(noDoubleApply,
                        $"soak {iteration} day {day} retry did not double-apply inventory",
                        $"before={before}, after_fail={afterFail}, after_retry={afterRetry}");
                    if (!noDoubleApply) break;
                    artifact.no_double_application_count++;

                    bool committed = _campaignDay.LastAdvancedDay == day
                        && _campaignDay.Calendar.CurrentDay == day;
                    Check(committed,
                        $"soak {iteration} day {day} retry committed the calendar",
                        $"last_advanced={_campaignDay.LastAdvancedDay}, calendar={_campaignDay.Calendar.CurrentDay}");
                    if (!committed) break;
                }

                Check(artifact.failed_closed_count == UiHarnessSoakIterations,
                    "every injected fault failed closed",
                    $"failed_closed={artifact.failed_closed_count}/{UiHarnessSoakIterations}");
                Check(artifact.retry_succeeded_count == UiHarnessSoakIterations,
                    "every same-day retry succeeded",
                    $"retried={artifact.retry_succeeded_count}/{UiHarnessSoakIterations}");
                Check(artifact.no_double_application_count == UiHarnessSoakIterations,
                    "no retry double-applied a restorable owner mutation",
                    $"no_double={artifact.no_double_application_count}/{UiHarnessSoakIterations}");
                artifact.faults_injected = fault.FaultsInjected;

                // ── Phase 5: persisted-envelope checksum comparison. ──
                bool saved = SaveAll(playCue: false);
                Check(saved, "SaveAll() committed the post-soak campaign envelope");

                var envelope = _saveLoadHost.ActiveEnvelope;
                artifact.envelope_section_count = envelope?.sections.Count ?? 0;
                artifact.envelope_aggregate_checksum = envelope?.aggregateChecksum ?? string.Empty;
                artifact.persisted_day = envelope?.manifest?.currentDay ?? 0;

                artifact.envelope_aggregate_integrity = envelope != null
                    && !string.IsNullOrEmpty(envelope.aggregateChecksum)
                    && envelope.aggregateChecksum == SaveSlotService.ComputeAggregateChecksum(envelope);
                Check(artifact.envelope_aggregate_integrity,
                    "persisted envelope aggregate checksum recomputes to the stored value",
                    $"sections={artifact.envelope_section_count}, checksum={artifact.envelope_aggregate_checksum}");

                artifact.envelope_section_integrity = envelope != null
                    && envelope.sections.Count > 0
                    && envelope.sections.All(s => s != null
                        && !string.IsNullOrEmpty(s.checksum)
                        && s.checksum == SaveSlotService.ComputeSectionChecksum(s));
                Check(artifact.envelope_section_integrity,
                    "every persisted section checksum recomputes to its stored value",
                    $"sections={artifact.envelope_section_count}");

                Check(artifact.persisted_day == _campaignDay.Calendar.CurrentDay,
                    "persisted envelope manifest day matches the committed calendar",
                    $"manifest_day={artifact.persisted_day}, calendar={_campaignDay.Calendar.CurrentDay}");

                var slot = _saveLoadHost.ActiveSlotId;
                string aggregatePath = slot.HasValue
                    ? Path.Combine(scratchRoot, SaveSlotService.SavesBaseDir, "profile-default",
                        "slot-" + slot.Value.Value, SaveSlotService.AggregateFileName)
                    : string.Empty;
                bool diskExists = !string.IsNullOrEmpty(aggregatePath) && File.Exists(aggregatePath);
                Check(diskExists, "campaign.json exists on disk after the harness save", aggregatePath);
                if (diskExists && envelope != null)
                {
                    var serializer = new SystemTextJsonSerializer();
                    var disk = serializer.Deserialize<AggregateSaveEnvelope>(File.ReadAllText(aggregatePath));
                    artifact.envelope_disk_matches_memory = disk != null
                        && disk.aggregateChecksum == envelope.aggregateChecksum
                        && disk.aggregateChecksum == SaveSlotService.ComputeAggregateChecksum(disk);
                }
                Check(artifact.envelope_disk_matches_memory,
                    "on-disk envelope checksum equals the in-memory envelope",
                    $"memory={artifact.envelope_aggregate_checksum}");
            }
            catch (Exception ex)
            {
                pass = false;
                GD.PrintErr($"[FAIL] ui-composition-harness exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                checks.Add(new UiHarnessCheck
                {
                    id = "harness_exception",
                    passed = false,
                    evidence = $"{ex.GetType().Name}: {ex.Message}"
                });
            }
            finally
            {
                if (faultRegistered && _campaignDay != null)
                {
                    try { _campaignDay.Unregister(UiHarnessFaultOwnerId); }
                    catch (Exception unregisterEx) { GD.PushWarning($"[ui-composition-harness] fault owner unregister skipped: {unregisterEx.Message}"); }
                }

                artifact.checks = checks;
                artifact.envelope_aggregate_integrity = artifact.envelope_aggregate_integrity
                    && checks.FirstOrDefault(c => c.id.StartsWith("persisted envelope aggregate", StringComparison.Ordinal))?.passed == true;
                WriteUiCompositionHarnessArtifact(artifact);

                try { ResetAllSessionsInMemory(); }
                catch (Exception resetEx) { GD.PushWarning($"[ui-composition-harness] session reset skipped: {resetEx.Message}"); }
                try
                {
                    if (Directory.Exists(scratchRoot)) Directory.Delete(scratchRoot, recursive: true);
                }
                catch
                {
                    // Disposable harness scratch cleanup is best effort.
                }

                HostCli.EmitSummary("ui_composition_harness_selftest", pass, pass ? 0 : 1,
                    checks.Count(c => c.passed), checks.Count(c => !c.passed));
                QuitUiTestAfterFrame(pass ? 0 : 1);
            }
        }

        /// <summary>
        /// Harness fixture: an unattended full-composition campaign starves its
        /// starting roster within days and seals the slot as a terminal loss,
        /// which would end the run before a multi-day determinism/retry soak
        /// can execute. Top up the authoritative need state and stock before
        /// each advance so the day coordinator is exercised without the
        /// survival balance ending the campaign. This mutates only existing
        /// need/inventory authority; it creates no parallel system.
        /// </summary>
        private void StabilizeUiHarnessRoster()
        {
            if (_inventory != null)
            {
                int food = _inventory.Inventory.CountById("canned_food");
                if (food < 80) _inventory.Add("canned_food", 80 - food);
                int water = _inventory.Inventory.CountById("clean_water");
                if (water < 80) _inventory.Add("clean_water", 80 - water);
            }

            if (_survivors?.RosterState == null) return;
            foreach (var record in _survivors.RosterState)
            {
                if (record == null || !record.IsAliveState) continue;
                record.Hunger = 0f;
                record.Thirst = 0f;
                record.Fatigue = 0f;
                record.Warmth = 100f;
                if (record.Health < record.MaxHealthCap) record.Health = record.MaxHealthCap;
                var radiation = _survivors.RadStateFor(record.Id);
                if (radiation != null) _survivors.Radiation.SetDose(radiation, 0f);
            }
        }

        private static void WriteUiCompositionHarnessArtifact(UiHarnessArtifact artifact)
        {
            try
            {
                string artifactDir = Path.Combine(CatalogPath.ResolveRepoRoot(), "artifacts");
                Directory.CreateDirectory(artifactDir);
                var serializer = new SystemTextJsonSerializer();
                File.WriteAllText(
                    Path.Combine(artifactDir, "ui-composition-harness.json"),
                    serializer.Serialize(artifact),
                    new System.Text.UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[ui-composition-harness] artifact write skipped: {ex.Message}");
            }
        }
    }
}
