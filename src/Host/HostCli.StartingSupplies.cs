// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunStartingSuppliesSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition)
                    GD.Print($"  [PASS] {name}");
                else
                {
                    GD.PrintErr($"  [FAIL] {name}");
                    failures++;
                }
            }

            string scratchRoot = Path.Combine(
                Path.GetTempPath(),
                "ashfall_starting_supplies_" + Guid.NewGuid().ToString("N")); // DETERMINISM_ALLOWLIST: Selftest scratch folder path
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            try
            {
                Directory.CreateDirectory(scratchRoot);
                var itemCatalog = ItemCatalogLoader.LoadCatalog(dataDirectory, fileIO, serializer);
                var loaded = ItemCatalogLoader.LoadStartingSuppliesCatalog(
                    dataDirectory,
                    fileIO,
                    serializer,
                    itemCatalog);
                Check(
                    loaded.Profiles.Count == 6,
                    $"six authored starting profiles loaded (got {loaded.Profiles.Count})");

                foreach (var profile in loaded.Profiles)
                {
                    string profileRoot = Path.Combine(scratchRoot, profile.id);
                    Directory.CreateDirectory(profileRoot);
                    SaveSlotRoot.CurrentRoot = profileRoot;

                    var session = InventoryHostSession.Create(dataDirectory, profile.id);
                    var before = Snapshot(session.Inventory, profile);
                    Check(
                        Matches(session.Inventory, profile),
                        $"{profile.id} seeds exact authored inventory");

                    session.LoadOrSeedStartingSupplies(
                        dataDirectory,
                        fileIO,
                        serializer,
                        failClosed: false,
                        profileId: profile.id);
                    Check(
                        Matches(session.Inventory, profile) &&
                        Snapshot(session.Inventory, profile) == before,
                        $"{profile.id} duplicate seed is ignored");

                    Check(
                        InventorySaveStore.TrySave(session.CaptureSave()),
                        $"{profile.id} inventory save writes");
                    var restoredWithDifferentRequest =
                        InventoryHostSession.Create(
                            dataDirectory,
                            StartingSuppliesCatalog.StandardProfileId);
                    Check(
                        Matches(restoredWithDifferentRequest.Inventory, profile),
                        $"{profile.id} existing save bypasses profile reseeding");
                }

                string emptyRoot = Path.Combine(scratchRoot, "empty-save");
                Directory.CreateDirectory(emptyRoot);
                SaveSlotRoot.CurrentRoot = emptyRoot;
                var empty = InventoryHostSession.Create(dataDirectory, seedWhenNoSave: false);
                Check(
                    InventorySaveStore.TrySave(empty.CaptureSave()),
                    "explicit empty inventory save writes");
                var restoredEmpty = InventoryHostSession.Create(
                    dataDirectory,
                    "origin_machine_room");
                Check(
                    restoredEmpty.Inventory.Slots.Count == 0,
                    "explicit empty inventory save is not mistaken for a fresh campaign");

                string fallbackRoot = Path.Combine(scratchRoot, "unknown-profile");
                Directory.CreateDirectory(fallbackRoot);
                SaveSlotRoot.CurrentRoot = fallbackRoot;
                var unknown = InventoryHostSession.Create(
                    dataDirectory,
                    "origin_not_authored");
                var standard = loaded.DefaultProfile;
                Check(
                    Matches(unknown.Inventory, standard),
                    "unknown profile safely resolves to Standard Holdfast");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] starting supplies selftest exception: {ex}");
                failures++;
            }
            finally
            {
                SaveSlotRoot.CurrentRoot = null;
                TryDeleteTempDirectory(scratchRoot);
            }

            return EmitSummary(
                "starting_supplies_selftest",
                failures == 0,
                failures == 0 ? 0 : 1,
                details: failures == 0
                    ? "six profile fresh-start matrix, fallback, idempotence, and save bypass passed"
                    : $"{failures} check(s) failed");
        }

        private static bool Matches(
            InventoryContainer inventory,
            StartingSuppliesProfile profile)
        {
            for (int i = 0; i < profile.supplies.Count; i++)
            {
                var entry = profile.supplies[i];
                if (inventory.CountById(entry.itemId) != entry.amount)
                    return false;
            }

            return true;
        }

        private static string Snapshot(
            InventoryContainer inventory,
            StartingSuppliesProfile profile)
        {
            var values = new List<string>();
            for (int i = 0; i < profile.supplies.Count; i++)
            {
                var entry = profile.supplies[i];
                values.Add(entry.itemId + "=" + inventory.CountById(entry.itemId));
            }
            return string.Join("|", values);
        }
    }
}
