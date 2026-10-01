// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    /// <summary>
    /// DEBT-ENRICHMENT-KEEPSAKE-ORPHANS closure gate.
    ///
    /// The C3-174 mechanical-origin seam grants a survivor's authored personal
    /// keepsake through the canonical inventory owner, but only when the id
    /// resolves in the item catalog. Sixty-five of the seventy-six authored
    /// keepsake ids previously resolved in no catalog, so those survivors
    /// silently received no keepsake.
    ///
    /// Every authored keepsake id is now a canonical item. This test is the
    /// fast regression gate: a future enrichment edit that introduces an
    /// unresolved keepsake id fails here rather than squatting in the debt log.
    /// </summary>
    public class EnrichmentKeepsakeResolutionTests : CatalogTestBase
    {
        private static ItemCatalog LoadItems()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return ItemCatalogLoader.LoadCatalog(DataDirectory, fileIO, json);
        }

        private static ExpansionEnrichmentCatalog LoadEnrichment()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return new ExpansionEnrichmentCatalogLoader(fileIO, json).Load(DataDirectory);
        }

        [Fact]
        public void EveryAuthoredKeepsakeId_ResolvesInTheCanonicalItemCatalog()
        {
            var catalog = LoadEnrichment();
            var items = LoadItems();

            var keepsakeIds = catalog.GetEnrichedSurvivorIds()
                .Select(id => catalog.GetKeepsakeItemId(id))
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(id => id, StringComparer.Ordinal)
                .ToList();

            var unresolved = keepsakeIds
                .Where(id => items.Get(id) == null)
                .ToList();

            Assert.True(unresolved.Count == 0,
                "Authored keepsake ids that resolve in no item catalog: " + string.Join(", ", unresolved));
            Assert.Equal(76, keepsakeIds.Count);
        }

        [Fact]
        public void KeepsakeResolution_IsStableAndNonEmpty()
        {
            var catalog = LoadEnrichment();
            var items = LoadItems();

            foreach (var survivorId in catalog.GetEnrichedSurvivorIds())
            {
                string keepsakeId = catalog.GetKeepsakeItemId(survivorId);
                if (string.IsNullOrEmpty(keepsakeId)) continue;
                var item = items.Get(keepsakeId);
                Assert.NotNull(item);
                Assert.False(string.IsNullOrWhiteSpace(item!.displayName),
                    $"Keepsake '{keepsakeId}' resolved to an item without a display name.");
            }
        }
    }
}
