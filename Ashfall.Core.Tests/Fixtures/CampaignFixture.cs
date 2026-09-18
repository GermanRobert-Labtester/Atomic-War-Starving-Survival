// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Random;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Fixtures
{
    /// <summary>
    /// Plan 27A — Unified Campaign Test Fixture.
    /// Constructs a campaign-shaped harness for unit, contract, determinism, and journey tests.
    /// Exposes two explicit paths (INV-27.1, INV-27.2):
    /// 1. CreateAuthorityBacked(dataDir, seed) — loads shipped items.json and production catalogs.
    /// 2. CreateSynthetic(seed) — explicitly synthetic minimal in-memory catalogs for isolated micro-tests.
    /// </summary>
    public sealed class CampaignFixture
    {
        public bool IsAuthorityBacked { get; }
        public string DataDirectory { get; }
        public int Seed { get; }

        public ItemCatalog Catalog { get; }
        public ItemDescriptionCatalog Descriptions { get; }
        public ExpansionEnrichmentCatalog? Enrichment { get; }

        public Inventory.Inventory Inventory { get; }
        public CampaignCalendar Calendar { get; }
        public CampaignRngManager Rng { get; }
        public CampaignDayCoordinator Coordinator { get; }

        public StartingLevelSystem StartingLevel { get; }
        public CraftingSystem Crafting { get; }

        private CampaignFixture(
            bool isAuthorityBacked,
            string dataDirectory,
            int seed,
            ItemCatalog catalog,
            ItemDescriptionCatalog descriptions,
            ExpansionEnrichmentCatalog? enrichment,
            Inventory.Inventory inventory,
            CampaignCalendar calendar,
            CampaignRngManager rng,
            CampaignDayCoordinator coordinator,
            StartingLevelSystem startingLevel,
            CraftingSystem crafting)
        {
            IsAuthorityBacked = isAuthorityBacked;
            DataDirectory = dataDirectory;
            Seed = seed;
            Catalog = catalog;
            Descriptions = descriptions;
            Enrichment = enrichment;
            Inventory = inventory;
            Calendar = calendar;
            Rng = rng;
            Coordinator = coordinator;
            StartingLevel = startingLevel;
            Crafting = crafting;
        }

        /// <summary>
        /// Creates an authority-backed campaign fixture using shipped JSON catalogs.
        /// </summary>
        public static CampaignFixture CreateAuthorityBacked(string? dataDir = null, int seed = 4242)
        {
            dataDir = string.IsNullOrEmpty(dataDir) ? ResolveAuthorityDataDir() : dataDir;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
            var descriptions = ItemDescriptionCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
            var enrichmentLoader = new ExpansionEnrichmentCatalogLoader(fileIO, serializer);
            var enrichment = enrichmentLoader.Load(dataDir);

            var inventory = new Inventory.Inventory();
            var calendar = new CampaignCalendar(initialDay: 1);
            var rng = new CampaignRngManager();
            var coordinator = new CampaignDayCoordinator(calendar, rng);

            var startingLevel = new StartingLevelSystem();
            var crafting = new CraftingSystem(inventory);

            // Register standard Core day owners
            coordinator.Register("starting_level_rations", new FixtureRationsDayOwner(inventory), phase: 2);

            return new CampaignFixture(
                isAuthorityBacked: true,
                dataDirectory: dataDir,
                seed: seed,
                catalog: catalog,
                descriptions: descriptions,
                enrichment: enrichment,
                inventory: inventory,
                calendar: calendar,
                rng: rng,
                coordinator: coordinator,
                startingLevel: startingLevel,
                crafting: crafting);
        }

        /// <summary>
        /// Creates an explicit synthetic campaign fixture with in-memory sample catalogs.
        /// Intended only for isolated mathematical formulas or malformed input boundary tests.
        /// </summary>
        public static CampaignFixture CreateSynthetic(int seed = 4242)
        {
            var catalog = CatalogTestFixtures.CreateSampleItemCatalog();
            var descriptions = new ItemDescriptionCatalog();

            var inventory = new Inventory.Inventory();
            var calendar = new CampaignCalendar(initialDay: 1);
            var rng = new CampaignRngManager();
            var coordinator = new CampaignDayCoordinator(calendar, rng);

            var startingLevel = new StartingLevelSystem();
            var crafting = new CraftingSystem(inventory);

            coordinator.Register("starting_level_rations", new FixtureRationsDayOwner(inventory), phase: 2);

            return new CampaignFixture(
                isAuthorityBacked: false,
                dataDirectory: string.Empty,
                seed: seed,
                catalog: catalog,
                descriptions: descriptions,
                enrichment: null,
                inventory: inventory,
                calendar: calendar,
                rng: rng,
                coordinator: coordinator,
                startingLevel: startingLevel,
                crafting: crafting);
        }

        /// <summary>
        /// Seeds starting supplies from the loaded catalog into the campaign inventory.
        /// </summary>
        public void PopulateStandardStartingSupplies()
        {
            void Add(string id, int count)
            {
                var def = Catalog.Get(id);
                if (def != null)
                    Inventory.Add(def, count);
            }

            Add("clean_water", 12);
            Add("canned_food", 16);
            Add("bandage", 4);
            Add("iodine_pills", 4);
            Add("scrap_mechanical", 6);
            Add("gas_mask", 1);
            Add("hazmat_suit", 1);
        }

        /// <summary>
        /// Advances the campaign by one day through the authoritative coordinator.
        /// </summary>
        public DayAdvancedEventArgs? AdvanceDay()
        {
            int targetDay = Calendar.CurrentDay + 1;
            return Coordinator.Advance(targetDay, new NullDayAdvancePersistence());
        }

        /// <summary>
        /// Computes a deterministic SHA-256 state digest across the fixture's calendar,
        /// inventory, and seed.
        /// </summary>
        public string ComputeStateDigest()
        {
            var sb = new StringBuilder();
            sb.Append("seed=").Append(Seed).Append('\n');
            sb.Append("day=").Append(Calendar.CurrentDay).Append('\n');
            sb.Append("slots=").Append(Inventory.Slots.Count).Append('\n');
            sb.Append("weight=").Append(Inventory.GetCurrentWeight().ToString("F2", CultureInfo.InvariantCulture)).Append('\n');
            var invState = Inventory.CaptureState();
            sb.Append("inv_hash=").Append(SaveChecksum.Compute(invState)).Append('\n');

            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            using var sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(bytes);
            var result = new StringBuilder(hash.Length * 2);
            for (int i = 0; i < hash.Length; i++)
                result.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
            return result.ToString();
        }

        /// <summary>
        /// Engine-agnostic resolver for the shipped data directory from test context.
        /// </summary>
        public static string ResolveAuthorityDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            string dir = baseDir;
            for (int i = 0; i < 6; i++)
            {
                probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return probe;
        }

        private sealed class NullDayAdvancePersistence : IDayAdvancePersistence
        {
            public void PersistBeforeBriefing(int day, IReadOnlyList<DayOwnerReport> ownerReports)
            {
            }
        }

        private sealed class FixtureRationsDayOwner : IDayAdvanceOwner
        {
            private readonly Inventory.Inventory _inventory;

            public FixtureRationsDayOwner(Inventory.Inventory inventory)
            {
                _inventory = inventory;
            }

            public void CapturePreDaySnapshot(int day)
            {
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                if (_inventory.HasSufficient("canned_food", 1))
                {
                    _inventory.TryConsume("canned_food", 1);
                    events.Add(new DayStateChangeEvent("ration_consumed", "rations", "canned_food", null, 1f));
                }
            }
        }
    }
}
