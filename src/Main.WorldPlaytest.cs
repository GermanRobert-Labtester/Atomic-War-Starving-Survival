// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Campaign;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// Runs the production composition root and its private
        /// EvolvingWorldDayOwner in an isolated fresh campaign. The detailed
        /// deterministic artifact replay remains in HostCli; this probe makes
        /// the CLI gate fail if the real Main-bound owner is not registered or
        /// cannot advance the same 30-day window.
        /// </summary>
        internal bool RunWorldPlaytestProductionOwnerProbe(int days)
        {
            string scratchRoot = Path.Combine(Path.GetTempPath(), "ashfall_world_playtest_production");
            var originalChildren = new HashSet<Node>(GetChildren().Cast<Node>());
            try
            {
                // Keep production setup away from the user's active campaign.
                // The root is explicit and disposable; no save is written by
                // this probe.
                if (Directory.Exists(scratchRoot))
                    Directory.Delete(scratchRoot, recursive: true);
                Directory.CreateDirectory(scratchRoot);
                SaveSlotRoot.CurrentRoot = scratchRoot;

                // Register the actual production owner through the production
                // campaign setup, then run that owner in a fresh coordinator
                // so unrelated UI-only owners cannot mask its result.
                _campaignInitializationMode = CampaignInitializationMode.FreshInitialize;
                _startingCohortProfileId = Ashfall.Core.Survivors.StartingCohortCatalog.StandardProfileId;
                _startingSuppliesProfileId = Ashfall.Core.Inventory.StartingSuppliesCatalog.StandardProfileId;
                SetupCampaignDay();
                SetupWorld();
                if (_campaignDay == null || _world == null)
                    return false;

                var productionOwners = _campaignDay.Owners;
                int worldOwnerCount = productionOwners.Count(owner =>
                    string.Equals(owner.GetType().Name, "EvolvingWorldDayOwner", StringComparison.Ordinal));
                if (worldOwnerCount != 1)
                {
                    GD.PrintErr($"[WorldPlaytest] production owner count was {worldOwnerCount}, expected 1");
                    return false;
                }

                var productionOwner = productionOwners.First(owner =>
                    string.Equals(owner.GetType().Name, "EvolvingWorldDayOwner", StringComparison.Ordinal));
                var productionCoordinator = new CampaignDayCoordinator();
                productionCoordinator.Register("world_evolution", productionOwner, phase: 4);
                _campaignDay = productionCoordinator;

                int eventDays = 0;
                _campaignDay.OnDayAdvanced += args =>
                {
                    if (args.OwnerReports.Any(report =>
                        string.Equals(report.OwnerId, "world_evolution", StringComparison.Ordinal)))
                        eventDays++;
                };

                int firstDay = _campaignDay.Calendar.CurrentDay + 1;
                int lastDay = firstDay + days - 1;
                for (int day = firstDay; day <= lastDay; day++)
                {
                    DayAdvancedEventArgs? result = AdvanceCampaignDayForValidation(day);
                    if (result == null || result.HasFailures || _campaignDay.Calendar.CurrentDay != day)
                    {
                        GD.PrintErr($"[WorldPlaytest] production coordinator stopped at day {_campaignDay.Calendar.CurrentDay}, expected {day}");
                        return false;
                    }
                }

                bool worldAdvanced = _world.LocationEvolution?.State.lastEvolutionDay == lastDay
                    && _world.Wildlife?.State.lastMigrationDay == lastDay
                    && _world.Landmarks?.State.lastDegradationDay == lastDay;
                bool passed = eventDays == days && worldAdvanced;
                GD.Print(passed
                    ? $"[PASS] production EvolvingWorldDayOwner advanced days {firstDay}-{lastDay} ({eventDays} reports)"
                    : $"[FAIL] production EvolvingWorldDayOwner probe: reports={eventDays}, expected={days}, worldAdvanced={worldAdvanced}");
                return passed;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[WorldPlaytest] production owner probe failed: {ex}");
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
                    // Disposable test scratch cleanup is best effort.
                }
            }
        }
    }
}
