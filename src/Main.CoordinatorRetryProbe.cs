// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 38 follow-up — runtime retry-injection probes.
//
//  1. RunCoordinatorRetryProductionProbe — the real `inventory_custody` +
//     `starting_level_rations` owners with a late fault and a same-day retry.
//  2. RunFullCompositionRetryProbe — the FULL production owner list (built UI +
//     ComposeCampaign), a control day, then an identical fresh campaign whose
//     day is faulted and retried; the pre/post inventory checksums must match the
//     control, proving the whole day rolls back atomically.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Campaign;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private sealed class RetryProbeFaultOwner : IDayAdvanceOwner
        {
            public bool Armed;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                if (Armed)
                    throw new InvalidOperationException("injected coordinator retry fault");
            }
        }

        /// <summary>
        /// Runtime retry-injection probe. Fresh campaign composition; the real
        /// inventory-custody and starting-level-rations owners run a day with a
        /// late fault, then the same day is retried with the fault cleared.
        /// The retry must roll the inventory back so the second attempt lands
        /// exactly one ration delta, and a third assertion proves it did not
        /// double-consume.
        /// </summary>
        internal bool RunCoordinatorRetryProductionProbe()
        {
            string scratchRoot = Path.Combine(Path.GetTempPath(), "ashfall_coordinator_retry");
            var originalChildren = new HashSet<Node>(GetChildren().Cast<Node>());
            try
            {
                if (Directory.Exists(scratchRoot))
                    Directory.Delete(scratchRoot, recursive: true);
                Directory.CreateDirectory(scratchRoot);
                SaveSlotRoot.CurrentRoot = scratchRoot;

                ResetAllSessionsInMemory();
                _campaignInitializationMode = CampaignInitializationMode.FreshInitialize;
                _startingCohortProfileId = Ashfall.Core.Survivors.StartingCohortCatalog.StandardProfileId;
                _startingSuppliesProfileId = Ashfall.Core.Inventory.StartingSuppliesCatalog.StandardProfileId;
                SetupCampaignDay();
                SetupStartingLevel();
                SetupInventory();
                if (_campaignDay == null || _inventory == null || _startingLevel == null)
                {
                    GD.PrintErr("[CoordinatorRetryProbe] campaign composition did not initialize");
                    return false;
                }

                var owners = _campaignDay.Owners;
                IDayAdvanceOwner? custody = owners.FirstOrDefault(o => o.GetType().Name == "InventoryDayOwner");
                IDayAdvanceOwner? rations = owners.FirstOrDefault(o => o.GetType().Name == "StartingLevelRationsDayOwner");
                if (custody == null || rations == null)
                {
                    GD.PrintErr("[CoordinatorRetryProbe] production inventory owners are not registered");
                    return false;
                }

                var probe = new CampaignDayCoordinator();
                probe.Register("inventory_custody", custody, phase: 1);
                probe.Register("starting_level_rations", rations, phase: 2);
                var fault = new RetryProbeFaultOwner();
                probe.Register("retry_probe_fault", fault, phase: 5);

                int before = _inventory.Inventory.CountById("canned_food");
                if (before < 12)
                    _inventory.Inventory.AddById("canned_food", 12 - before);
                before = _inventory.Inventory.CountById("canned_food");

                int day = probe.Calendar.CurrentDay + 1;

                fault.Armed = true;
                var first = probe.Advance(day);
                if (first == null || !first.HasFailures)
                {
                    GD.PrintErr("[CoordinatorRetryProbe] first attempt did not fail closed");
                    return false;
                }
                int afterFail = _inventory.Inventory.CountById("canned_food");
                int delta = before - afterFail;
                if (delta <= 0)
                {
                    GD.PrintErr($"[CoordinatorRetryProbe] first attempt did not consume rations (before={before}, after={afterFail})");
                    return false;
                }

                fault.Armed = false;
                var second = probe.Advance(day);
                if (second == null || !second.Succeeded)
                {
                    GD.PrintErr("[CoordinatorRetryProbe] retry did not succeed");
                    return false;
                }

                int afterRetry = _inventory.Inventory.CountById("canned_food");
                bool exactlyOnce = afterRetry == before - delta;
                bool notDoubled = afterRetry != before - (2 * delta);
                GD.Print(exactlyOnce && notDoubled
                    ? $"[PASS] coordinator retry: rations consumed once (before={before}, after_fail={afterFail}, after_retry={afterRetry}, delta={delta})"
                    : $"[FAIL] coordinator retry: before={before}, after_fail={afterFail}, after_retry={afterRetry}, delta={delta}");
                return exactlyOnce && notDoubled;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[CoordinatorRetryProbe] probe failed: {ex}");
                return false;
            }
            finally
            {
                ResetAllSessionsInMemory();
                foreach (var child in GetChildren().Cast<Node>().ToArray())
                {
                    if (!originalChildren.Contains(child))
                        child.Free();
                }
                SaveSlotRoot.CurrentRoot = null;
                try
                {
                    if (Directory.Exists(scratchRoot))
                        Directory.Delete(scratchRoot, recursive: true);
                }
                catch
                {
                    // Disposable probe scratch cleanup is best effort.
                }
            }
        }

        /// <summary>Deterministic checksum of every inventory stack (id + amount).</summary>
        private string InventoryChecksum()
        {
            if (_inventory?.Inventory == null) return string.Empty;
            var slots = _inventory.Inventory.CaptureState().slots;
            if (slots == null) return string.Empty;
            return string.Join(";", slots
                .Where(s => s != null && !string.IsNullOrEmpty(s.itemId))
                .OrderBy(s => s.itemId, StringComparer.Ordinal)
                .Select(s => s.itemId + ":" + s.amount.ToString(CultureInfo.InvariantCulture)));
        }

        private void ResetForFullCompositionPhase(string slotRoot)
        {
            if (Directory.Exists(slotRoot)) Directory.Delete(slotRoot, recursive: true);
            Directory.CreateDirectory(slotRoot);
            SaveSlotRoot.CurrentRoot = slotRoot;
            ResetAllSessionsInMemory();
            _campaignDay = null;
            _campaignDayHost = null;
            _campaignInitializationMode = CampaignInitializationMode.FreshInitialize;
            _startingCohortProfileId = Ashfall.Core.Survivors.StartingCohortCatalog.StandardProfileId;
            _startingSuppliesProfileId = Ashfall.Core.Inventory.StartingSuppliesCatalog.StandardProfileId;
            ComposeCampaign();
        }

        /// <summary>
        /// Full-composition retry probe: builds the UI and composes the real
        /// campaign once, runs a control day, then resets to a second identical
        /// fresh campaign and runs the same day with a late fault + same-day
        /// retry. The pre-day and post-day inventory checksums must match the
        /// control, proving the whole owner list rolls back atomically.
        /// </summary>
        internal bool RunFullCompositionRetryProbe()
        {
            string scratchRoot = Path.Combine(Path.GetTempPath(), "ashfall_full_retry");
            var originalChildren = new HashSet<Node>(GetChildren().Cast<Node>());
            try
            {
                BuildUserInterface();

                // Phase A — control.
                ResetForFullCompositionPhase(Path.Combine(scratchRoot, "control"));
                if (_campaignDay == null || _inventory == null)
                {
                    GD.PrintErr("[FullRetryProbe] control composition did not initialize");
                    return false;
                }
                string controlBefore = InventoryChecksum();
                int day = _campaignDay.Calendar.CurrentDay + 1;
                var control = AdvanceCampaignDayForValidation(day);
                if (control == null || !control.Succeeded)
                {
                    if (control != null)
                        foreach (var report in control.FailedReports)
                            GD.PrintErr($"[FullRetryProbe] control owner failed: {report.OwnerId}: {report.FailureMessage}");
                    GD.PrintErr("[FullRetryProbe] control day did not succeed");
                    return false;
                }
                string controlAfter = InventoryChecksum();

                // Phase B — identical fresh campaign, fault + same-day retry.
                ResetForFullCompositionPhase(Path.Combine(scratchRoot, "retry"));
                if (_campaignDay == null || _inventory == null)
                {
                    GD.PrintErr("[FullRetryProbe] retry composition did not initialize");
                    return false;
                }
                string retryBefore = InventoryChecksum();
                if (retryBefore != controlBefore)
                {
                    GD.PrintErr("[FullRetryProbe] fresh compositions are not deterministic");
                    return false;
                }

                var fault = new RetryProbeFaultOwner();
                _campaignDay.Register("retry_probe_fault", fault, phase: 5);
                fault.Armed = true;
                var first = AdvanceCampaignDayForValidation(day);
                if (first == null || !first.HasFailures)
                {
                    GD.PrintErr("[FullRetryProbe] fault attempt did not fail closed");
                    return false;
                }
                fault.Armed = false;
                var second = AdvanceCampaignDayForValidation(day);
                if (second == null || !second.Succeeded)
                {
                    GD.PrintErr("[FullRetryProbe] retry did not succeed");
                    return false;
                }
                string retryAfter = InventoryChecksum();

                bool pass = retryAfter == controlAfter;
                GD.Print(pass
                    ? $"[PASS] full-composition retry: {_campaignDay.Owners.Count} owners rolled back to the no-failure baseline"
                    : "[FAIL] full-composition retry diverged from the control inventory checksum");
                return pass;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FullRetryProbe] probe failed: {ex}");
                return false;
            }
            finally
            {
                ResetAllSessionsInMemory();
                foreach (var child in GetChildren().Cast<Node>().ToArray())
                {
                    if (!originalChildren.Contains(child))
                        child.Free();
                }
                SaveSlotRoot.CurrentRoot = null;
                try
                {
                    if (Directory.Exists(scratchRoot))
                        Directory.Delete(scratchRoot, recursive: true);
                }
                catch
                {
                    // Disposable probe scratch cleanup is best effort.
                }
            }
        }
    }
}
