// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// PLAN 159 — tanning/leather knowledge archive over TanningLeatherCatalog
    /// (30 records, four families). Parity, fail-closed identity, authority
    /// firewall, idempotent discovery, and save round-trip coverage.
    /// </summary>
    public sealed class LeatherworkArchiveTests : CatalogTestBase
    {
        private readonly string _narrativeDir;

        public LeatherworkArchiveTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        private static readonly string DataDir =
            Path.Combine(DataDirectory, "narrative");

        private static TanningLeatherCatalog LoadCatalog() => TanningLeatherCatalog.LoadFromDirectory(DataDir);

        private static (ItemCatalog items, LeatherworkArchiveSystem archive) MakeArchive()
        {
            var catalog = LoadCatalog();
            var items = ItemCatalogLoader.LoadCatalog(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            var archive = new LeatherworkArchiveSystem(catalog, itemLookup: id => items.Get(id));
            foreach (var (recordId, itemId) in LeatherworkArchiveSystem.DefaultCanonicalLinks())
                Assert.True(archive.TryRegisterCanonicalLink(recordId, itemId),
                    $"canonical link failed: {recordId} -> {itemId}");
            foreach (var (recordId, producer) in LeatherworkArchiveSystem.DefaultProducerMap())
                Assert.True(archive.TryRegisterProducer(recordId, producer),
                    $"producer registration failed: {recordId} -> {producer}");
            return (items, archive);
        }

        // ── Catalog parity (§20) ─────────────────────────────────────────

        [Fact]
        public void Catalog_LoadsAll30Records_FourFamiliesPreserved()
        {
            var catalog = LoadCatalog();
            Assert.Equal(8, catalog.BarkEntries.Count);
            Assert.Equal(8, catalog.MineralEntries.Count);
            Assert.Equal(7, catalog.BatingEntries.Count);
            Assert.Equal(7, catalog.CurryingEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void RecordIds_Unique_AcrossAllFamilies()
        {
            var catalog = LoadCatalog();
            var all = catalog.BarkEntries.Select(e => e.Id)
                .Concat(catalog.MineralEntries.Select(e => e.Id))
                .Concat(catalog.BatingEntries.Select(e => e.Id))
                .Concat(catalog.CurryingEntries.Select(e => e.Id))
                .ToList();
            Assert.Equal(all.Count, all.Distinct(StringComparer.Ordinal).Count());
            Assert.Equal(30, all.Count);
        }

        // ── Identity (§20) — fail-closed canonical links ─────────────────

        [Fact]
        public void CanonicalLinks_AllResolve_InItemAuthority()
        {
            var (items, _) = MakeArchive();
            foreach (var (recordId, itemId) in LeatherworkArchiveSystem.DefaultCanonicalLinks())
                Assert.NotNull(items.Get(itemId));
        }

        [Fact]
        public void CanonicalLinks_RejectUnresolvedItems_FailClosed()
        {
            var catalog = LoadCatalog();
            var archive = new LeatherworkArchiveSystem(catalog, itemLookup: _ => null);
            Assert.False(archive.TryRegisterCanonicalLink("oak_bark_tan_chestnut_liquor_density", "item_vegetable_tanned_hide"));
            Assert.Null(archive.GetCanonicalItem("oak_bark_tan_chestnut_liquor_density"));
        }

        [Fact]
        public void CanonicalLinks_NeverPromoteFacilityLabelsToWorldOrItemIds()
        {
            // Vat/pit/workshop equipment designations must never become item IDs.
            var catalog = LoadCatalog();
            var (_, archive) = MakeArchive();
            foreach (var label in catalog.BarkEntries.Select(e => e.TanneryVatId)
                .Concat(catalog.MineralEntries.Select(e => e.MineralTanLiquorId))
                .Concat(catalog.BatingEntries.Select(e => e.BeamhousePitId))
                .Concat(catalog.CurryingEntries.Select(e => e.CurryingWorkshopId)))
            {
                Assert.DoesNotContain(label, LeatherworkArchiveSystem.DefaultCanonicalLinks().Select(l => l.itemId));
            }
            Assert.All(LeatherworkArchiveSystem.DefaultProducerMap(), p =>
                Assert.DoesNotMatch("^(TANNERY|PIT|DRUM|BENCH|VAT|WASHER)", p.producerId));
            _ = archive;
        }

        // ── Authority firewall (§5) — projection surface has no mechanical APIs ──

        [Fact]
        public void Firewall_ProjectionSurface_HasNoMechanicalAPIs()
        {
            const BindingFlags Flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly;

            // The ONLY mutators the archive may expose are discovery/ledger
            // operations. Anything that looks like a mutator and is not in
            // this allowlist fails the gate.
            var allowedMutators = new System.Collections.Generic.HashSet<string>
            {
                nameof(LeatherworkArchiveSystem.DiscoverForItem),
                nameof(LeatherworkArchiveSystem.DiscoverAtProducer),
                nameof(LeatherworkArchiveSystem.DiscoverRecord),
                nameof(LeatherworkArchiveSystem.TryRegisterProducer),
                nameof(LeatherworkArchiveSystem.TryRegisterCanonicalLink),
                nameof(LeatherworkArchiveSystem.RestoreState)
            };
            foreach (var m in typeof(LeatherworkArchiveSystem).GetMethods(Flags))
            {
                if (m.IsSpecialName) continue; // property getters
                bool mutatorShaped = m.Name.StartsWith("Discover") || m.Name.StartsWith("Try")
                    || m.Name.StartsWith("Restore") || m.Name.StartsWith("Add") || m.Name.StartsWith("Remove")
                    || m.Name.StartsWith("Set") || m.Name.StartsWith("Apply") || m.Name.StartsWith("Consume")
                    || m.Name.StartsWith("Craft") || m.Name.StartsWith("Repair") || m.Name.StartsWith("Damage");
                if (mutatorShaped)
                    Assert.True(allowedMutators.Contains(m.Name), $"unexpected mutator on archive surface: {m.Name}");
            }

            // No writable properties: the archive surface is a projection.
            foreach (var p in typeof(LeatherworkArchiveSystem).GetProperties(Flags))
                Assert.False(p.CanWrite, $"archive property must be read-only: {p.Name}");
        }

        [Fact]
        public void Firewall_ViewingRecords_MutatesNothingButDiscovery()
        {
            var (_, archive) = MakeArchive();
            int before = archive.State.discoveredRecordIds.Count;

            var record = archive.GetRecord("leather_harness_neatsfoot_oil_cold_stuffing");
            Assert.NotNull(record);
            // Archival measurement display: PSI is a historical batch spec, not durability.
            Assert.Contains("PSI", record!.MeasurementSummary);
            // Projection is a pure read — the discovery ledger is untouched.
            Assert.Equal(before, archive.State.discoveredRecordIds.Count);
            Assert.False(archive.IsDiscovered(record.RecordId));
        }

        [Fact]
        public void Firewall_HistoricalTimestamps_AreNotCampaignTime()
        {
            var (_, archive) = MakeArchive();
            var record = archive.GetRecord("oak_bark_tan_chestnut_liquor_density");
            Assert.NotNull(record);
            Assert.Contains("YEAR_", record!.TimestampRelative);
            Assert.DoesNotContain("day", record.TimestampRelative, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Firewall_HideSteepMonths_AreNotCraftingTimers()
        {
            var (_, archive) = MakeArchive();
            var record = archive.GetRecord("oak_bark_tan_chestnut_liquor_density");
            Assert.NotNull(record);
            // Steep duration renders as an archival measurement summary only.
            Assert.Contains("12 mo", record!.MeasurementSummary);
            Assert.DoesNotContain("timer", record.MeasurementSummary, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("craft", record.MeasurementSummary, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Firewall_FormulaAndPh_RenderAsProvenanceOnly()
        {
            var (_, archive) = MakeArchive();
            var currying = archive.GetRecord("leather_harness_currying_dubbin_waterproofing");
            Assert.NotNull(currying);
            Assert.Contains("DUBBIN", currying!.MaterialSource); // formula label shown as-is, never expanded into a recipe

            var mineral = archive.GetRecord("mineral_tan_hexavalent_chromium_toxic_rash");
            Assert.NotNull(mineral);
            Assert.Contains("pH", mineral!.MeasurementSummary); // pH shown as archival assay only
            Assert.DoesNotContain("damage", mineral.MeasurementSummary, StringComparison.OrdinalIgnoreCase);
        }

        // ── Producers (§20) — real locations, all four families, no inventions ──

        [Fact]
        public void ProducerMap_ValidLocations_NoInventedSites()
        {
            var locationIds = DeepLoreLocationCatalogLoader.Load(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer())
                .Select(l => l.id)
                .ToHashSet(StringComparer.Ordinal);

            foreach (var (recordId, producerId) in LeatherworkArchiveSystem.DefaultProducerMap())
            {
                Assert.True(producerId.StartsWith("location_", StringComparison.Ordinal),
                    $"producer '{producerId}' for {recordId} must be an existing deep-lore site");
                Assert.True(locationIds.Contains(producerId),
                    $"producer '{producerId}' for {recordId} is not a real deep-lore location");
            }
        }

        [Fact]
        public void ProducerMap_CoversAllFourFamilies_AcrossAtLeastFiveProducers()
        {
            var (_, archive) = MakeArchive();

            var producers = LeatherworkArchiveSystem.DefaultProducerMap().Select(p => p.producerId)
                .Distinct(StringComparer.Ordinal).ToList();
            Assert.True(producers.Count >= 5, $"expected >= 5 producers, got {producers.Count}");

            // Every family has at least one record with a producer (or is
            // explicitly deferred — none are here; the deferred record is
            // in addition to family coverage).
            var producerRecordIds = LeatherworkArchiveSystem.DefaultProducerMap()
                .Select(p => p.recordId).ToHashSet(StringComparer.Ordinal);
            var catalog = LoadCatalog();
            Assert.Contains(LeatherProcessFamily.OakBarkVegetableTan,
                catalog.BarkEntries.Where(e => producerRecordIds.Contains(e.Id)).Select(e => archive.FamilyOf(e.Id)));
            Assert.Contains(LeatherProcessFamily.MineralTanLiquor,
                catalog.MineralEntries.Where(e => producerRecordIds.Contains(e.Id)).Select(e => archive.FamilyOf(e.Id)));
            Assert.Contains(LeatherProcessFamily.RawhideBatingFailure,
                catalog.BatingEntries.Where(e => producerRecordIds.Contains(e.Id)).Select(e => archive.FamilyOf(e.Id)));
            Assert.Contains(LeatherProcessFamily.HarnessCurrying,
                catalog.CurryingEntries.Where(e => producerRecordIds.Contains(e.Id)).Select(e => archive.FamilyOf(e.Id)));
        }

        [Fact]
        public void ProducerMap_DuplicateProducerRegistration_FailsClosed()
        {
            var (_, archive) = MakeArchive();
            Assert.False(archive.TryRegisterProducer("oak_bark_tan_chestnut_liquor_density", "location_steelworks"));
        }

        // ── Discovery + save (§19/§20) ───────────────────────────────────

        [Fact]
        public void Discovery_FirstDiscovery_New_Subsequent_Idempotent()
        {
            var (_, archive) = MakeArchive();
            var first = archive.DiscoverAtProducer("location_automated_abattoir");
            Assert.Equal(7, first.Count);
            Assert.Equal(0, archive.DiscoverAtProducer("location_automated_abattoir").Count);
            Assert.All(first, id => Assert.True(archive.IsDiscovered(id)));
        }

        [Fact]
        public void Discovery_UnknownProducerAndRecord_FailClosed()
        {
            var (_, archive) = MakeArchive();
            Assert.Empty(archive.DiscoverAtProducer("location_tannery_ruins"));
            Assert.False(archive.DiscoverRecord("rawhide_bate_does_not_exist"));
            Assert.False(archive.DiscoverRecord(""));
        }

        [Fact]
        public void SaveRoundTrip_PreservesDiscovery_NoReplay()
        {
            var (_, archive) = MakeArchive();
            archive.DiscoverAtProducer("location_chemical_plant");
            var captured = archive.CaptureState();

            var fresh = new LeatherworkArchiveSystem(LoadCatalog());
            fresh.RestoreState(captured);
            Assert.Equal(0, fresh.DiscoverAtProducer("location_chemical_plant").Count); // no duplicate rewards
            Assert.Equal(captured.discoveredRecordIds.Count, fresh.State.discoveredRecordIds.Count);
        }

        [Fact]
        public void Restore_OldSave_Null_DiscoversNothing_Automatically()
        {
            var fresh = new LeatherworkArchiveSystem(LoadCatalog());
            fresh.RestoreState(null);
            Assert.Equal(0, fresh.State.discoveredRecordIds.Count);
            Assert.Equal(30, fresh.DeferredRecordIds().Count);
        }

        [Fact]
        public void Restore_UnknownFutureIds_Tolerated()
        {
            var fresh = new LeatherworkArchiveSystem(LoadCatalog());
            fresh.RestoreState(new LeatherworkArchiveState
            {
                discoveredRecordIds = new System.Collections.Generic.List<string>
                {
                    "oak_bark_tan_chestnut_liquor_density",
                    "future_tanning_record_from_a_later_schema",
                    ""
                }
            });
            Assert.True(fresh.IsDiscovered("oak_bark_tan_chestnut_liquor_density"));
            // Unknown future IDs are tolerated (not discovered-failed); the
            // empty entry is dropped. No exception, no replay of producers.
            Assert.Equal(2, fresh.State.discoveredRecordIds.Count);
            Assert.Equal(0, fresh.DiscoverAtProducer("location_automated_abattoir").Count);
        }

        // ── Item-inspection producer (Workstream G) ──────────────────────

        [Fact]
        public void DiscoverForItem_FirstInspection_DiscoversLinkedRecords()
        {
            var (_, archive) = MakeArchive();
            var newly = archive.DiscoverForItem("leather_strap");
            Assert.Equal(2, newly.Count); // neatsfoot stuffing + red rot strap records
            Assert.Equal(0, archive.DiscoverForItem("leather_strap").Count);
            Assert.Contains("leather_harness_neatsfoot_oil_cold_stuffing", newly);
        }

        [Fact]
        public void DiscoverForItem_UnknownItem_ReturnsEmpty()
        {
            var (_, archive) = MakeArchive();
            Assert.Empty(archive.DiscoverForItem("item_bark_tannin_extract"));
            Assert.Empty(archive.DiscoverForItem(""));
        }

        [Fact]
        public void DiscoverForItem_RoundTrips_ThroughSave()
        {
            var (_, archive) = MakeArchive();
            archive.DiscoverForItem("gas_mask");
            var captured = archive.CaptureState();

            var fresh = new LeatherworkArchiveSystem(LoadCatalog());
            fresh.RestoreState(captured);
            Assert.True(fresh.IsDiscovered("mineral_tan_potassium_alum_white_tawing"));
            Assert.Empty(fresh.DiscoverForItem("gas_mask"));
        }

        [Fact]
        public void Queries_RecordsForItem_OnlyDiscoveredLinkedRecords()
        {
            var (_, archive) = MakeArchive();
            Assert.Empty(archive.RecordsForItem("leather_strap")); // linked but undiscovered
            archive.DiscoverRecord("leather_harness_neatsfoot_oil_cold_stuffing");
            var rows = archive.RecordsForItem("leather_strap");
            Assert.Single(rows);
            Assert.Equal("leather_strap", rows[0].CanonicalItemId);
        }

        [Fact]
        public void DeferredRecords_AccountForTheRest()
        {
            var (_, archive) = MakeArchive();
            var deferred = archive.DeferredRecordIds();
            var producerRecords = LeatherworkArchiveSystem.DefaultProducerMap().Select(p => p.recordId).ToHashSet();
            Assert.Equal(30, deferred.Count + producerRecords.Count);
            Assert.DoesNotContain("mineral_tan_formaldehyde_synthetic_oil_tannage", producerRecords);
            Assert.Contains("mineral_tan_formaldehyde_synthetic_oil_tannage", deferred);
        }

        [Fact]
        public void FamilyOf_RoutesEachCatalogFamily_Correctly()
        {
            var (_, archive) = MakeArchive();
            Assert.Equal(LeatherProcessFamily.OakBarkVegetableTan, archive.FamilyOf("oak_bark_tan_chestnut_liquor_density"));
            Assert.Equal(LeatherProcessFamily.MineralTanLiquor, archive.FamilyOf("mineral_tan_potassium_alum_white_tawing"));
            Assert.Equal(LeatherProcessFamily.RawhideBatingFailure, archive.FamilyOf("rawhide_bate_ammonium_sulfate_deliming_stall"));
            Assert.Equal(LeatherProcessFamily.HarnessCurrying, archive.FamilyOf("leather_harness_neatsfoot_oil_cold_stuffing"));
        }

        [Fact]
        public void FailureReports_SurfaceOnlyFailureRecords_Deterministic()
        {
            var (_, archive) = MakeArchive();
            archive.DiscoverAtProducer("location_automated_abattoir");
            var failures = archive.FailureReports();
            Assert.Equal(7, failures.Count);
            Assert.All(failures, r => Assert.False(string.IsNullOrEmpty(r.FailureSummary)));
            var again = archive.FailureReports();
            Assert.Equal(failures.Select(f => f.RecordId), again.Select(f => f.RecordId));
        }
    }
}
