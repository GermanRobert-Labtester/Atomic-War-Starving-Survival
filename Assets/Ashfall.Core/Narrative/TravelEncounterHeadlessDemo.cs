// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Narrative
{
    public sealed class TravelEncounterHeadlessReport : HeadlessReport
    {
        public int EncounterCount;
        public int PatrolCount;
        public int RestoredChainStage;
        public int RestoredCooldownDay;
    }

    /// <summary>
    /// Production-catalog patrol lifecycle oracle for the F5-F8 integration.
    /// This deliberately exercises the same catalog, selection, resolution,
    /// capture, JSON round-trip, and presentation projection used by hosts.
    /// </summary>
    public static class TravelEncounterHeadlessDemo
    {
        public static TravelEncounterHeadlessReport Run(string? dataDirectory = null, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log ??= NullLog.Instance;
            var report = new TravelEncounterHeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition)
                {
                    report.PassedCount++;
                    log.Info("[PASS] " + name);
                }
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
            }

            string? found = dataDirectory;
            if (string.IsNullOrWhiteSpace(found) &&
                CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string currentDir))
                found = currentDir;
            if (string.IsNullOrWhiteSpace(found) &&
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string baseDir))
                found = baseDir;

            Check(!string.IsNullOrWhiteSpace(found), "travel encounter data directory located");
            if (string.IsNullOrWhiteSpace(found))
                return Finish(report, log);

            var catalog = TravelEncounterCatalog.LoadFromDirectory(found!, new FileSystemIO());
            report.EncounterCount = catalog.Encounters.Count;
            report.PatrolCount = catalog.Encounters.Count(e =>
                e.Id.StartsWith("enc_patrol_", StringComparison.OrdinalIgnoreCase));
            Check(report.EncounterCount == 57,
                $"travel encounter catalog contains 57 definitions (got {report.EncounterCount})");
            Check(report.PatrolCount == 21,
                $"travel encounter catalog contains 21 patrol definitions (got {report.PatrolCount})");

            var checkpoint = catalog.GetEncounter("enc_patrol_garrison_checkpoint");
            Check(checkpoint != null, "garrison checkpoint is present");
            if (checkpoint == null)
                return Finish(report, log);

            Check(checkpoint.GetCooldownDays() == 3, "garrison checkpoint uses authored 3-day cooldown");
            Check(checkpoint.Choices.Any(c => c.ChoiceId == "choice_garrison_reduced_toll"),
                "garrison checkpoint exposes recognition choice");

            var inventory = new Inventory.Inventory { Capacity = 50, MaxWeight = 500f };
            inventory.TryProduce("canned_food", 10);
            var factionWar = new FactionWarSystem();
            var system = new TravelEncounterSystem(catalog, inventory, factionWar);

            Check(system.ResolveChoice(
                    checkpoint.Id,
                    "choice_pay_garrison_toll",
                    10,
                    out var firstResolution),
                "initial garrison toll resolves");
            Check(firstResolution != null && inventory.CountById("canned_food") == 8,
                "initial toll removes exactly two canned food");
            Check(factionWar.GetStanding("faction_central_garrison") == 1,
                "initial toll changes canonical garrison standing");
            Check(system.GetCooldownExpiry("patrol_garrison_checkpoint") == 13,
                "cooldown stores resolution day plus authored duration");
            Check(system.GetPatrolChainStage("iron_garrison", "patrol_garrison_trust") == 1,
                "initial toll advances garrison trust to stage one");

            var serializer = new SystemTextJsonSerializer();
            string savedJson = serializer.Serialize(system.CaptureState());
            var restoredState = serializer.Deserialize<TravelEncounterState>(savedJson);
            var restored = new TravelEncounterSystem(catalog, inventory, factionWar);
            restored.RestoreState(restoredState!);
            report.RestoredChainStage = restored.GetPatrolChainStage("iron_garrison", "patrol_garrison_trust");
            report.RestoredCooldownDay = restored.GetCooldownExpiry("patrol_garrison_checkpoint");
            Check(report.RestoredCooldownDay == 13, "save round-trip preserves patrol cooldown");
            Check(report.RestoredChainStage == 1, "save round-trip preserves recognition stage");
            Check(!restored.IsEncounterEligible(checkpoint, "high_scarp", 1f, "all", 12),
                "day before cooldown boundary remains blocked");
            Check(restored.IsEncounterEligible(checkpoint, "high_scarp", 1f, "all", 13),
                "exact cooldown boundary is eligible");

            var stageOne = restored.BuildPatrolPresentation(checkpoint.Id);
            var reducedToll = stageOne?.Choices.FirstOrDefault(c =>
                c.ChoiceId == "choice_garrison_reduced_toll");
            Check(reducedToll != null && reducedToll.IsAvailable,
                "stage-one presentation enables reduced toll");

            Check(restored.ResolveChoice(
                    checkpoint.Id,
                    "choice_garrison_reduced_toll",
                    13,
                    out _),
                "recognized garrison toll resolves after cooldown");
            Check(restored.GetPatrolChainStage("iron_garrison", "patrol_garrison_trust") == 2,
                "recognized toll advances garrison trust to stage two");
            var trusted = restored.BuildPatrolPresentation(checkpoint.Id);
            Check(trusted?.Choices.Any(c => c.ChoiceId == "choice_garrison_free_passage" && c.IsAvailable) == true,
                "trusted presentation enables free passage");

            var emptyInventory = new Inventory.Inventory { Capacity = 50, MaxWeight = 500f };
            var failed = new TravelEncounterSystem(catalog, emptyInventory, new FactionWarSystem());
            Check(!failed.ResolveChoice(
                    checkpoint.Id,
                    "choice_pay_garrison_toll",
                    10,
                    out _),
                "missing toll cost rejects resolution");
            Check(failed.GetCooldownExpiry("patrol_garrison_checkpoint") == 0 &&
                  failed.GetPatrolChainStage("iron_garrison", "patrol_garrison_trust") == 0,
                "failed cost path leaves cooldown and recognition unchanged");

            return Finish(report, log);
        }

        private static TravelEncounterHeadlessReport Finish(
            TravelEncounterHeadlessReport report,
            ILog log)
        {
            report.Passed = report.FailedCount == 0;
            report.Summary = $"TravelEncounterHeadlessDemo {(report.Passed ? "PASS" : "FAIL")} " +
                $"{report.PassedCount}/{report.PassedCount + report.FailedCount} " +
                $"(catalog={report.EncounterCount}, patrols={report.PatrolCount}, " +
                $"cooldown={report.RestoredCooldownDay}, chain={report.RestoredChainStage})";
            log.Info(report.Summary);
            return report;
        }
    }
}
