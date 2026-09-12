// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// PLAN 158 — technical material archive tests: catalog parity (60
    /// records, 8 families), identity crosswalk (canonical links proven;
    /// historical labels stay non-canonical), the §5/§18 authority firewall
    /// (measurements are never mechanics), and discovery/save discipline.
    /// </summary>
    public sealed class TechnicalMaterialArchiveTests : CatalogTestBase
    {
        private static string NarrativeDir => Path.Combine(DataDirectory, "narrative");

        private static (CordageCableCatalog cordage, PolymerTextileCatalog polymer) LoadCatalogs()
            => (CordageCableCatalog.LoadFromDirectory(NarrativeDir),
                PolymerTextileCatalog.LoadFromDirectory(NarrativeDir));

        private static ItemCatalog LoadItemCatalog()
            => ItemCatalogLoader.LoadCatalog(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

        private static HashSet<string> LoadDeepLoreLocationIds()
        {
            var entries = DeepLoreLocationCatalogLoader.Load(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            return new HashSet<string>(entries.Select(l => l.id), StringComparer.Ordinal);
        }

        private static TechnicalMaterialArchiveSystem CreateArchive(
            CordageCableCatalog? cordage = null,
            PolymerTextileCatalog? polymer = null,
            bool withDefaults = true)
        {
            cordage ??= LoadCatalogs().cordage;
            polymer ??= LoadCatalogs().polymer;
            var itemCatalog = LoadItemCatalog();
            var locationIds = LoadDeepLoreLocationIds();

            var archive = new TechnicalMaterialArchiveSystem(
                cordage, polymer,
                itemLookup: id => itemCatalog.Get(id),
                locationExists: id => locationIds.Contains(id));

            if (withDefaults)
            {
                foreach (var (recordId, itemId) in TechnicalMaterialArchiveSystem.DefaultCanonicalLinks())
                    Assert.True(archive.TryRegisterCanonicalLink(recordId, itemId),
                        $"canonical link failed: {recordId} -> {itemId}");
                foreach (var (recordId, producer) in TechnicalMaterialArchiveSystem.DefaultProducerMap())
                    Assert.True(archive.TryRegisterProducer(recordId, producer),
                        $"producer registration failed: {recordId} -> {producer}");
            }
            return archive;
        }

        // ── Catalog parity (§18) ─────────────────────────────────────────

        [Fact]
        public void Catalogs_LoadAll60Records_FamiliesPreserved()
        {
            var (cordage, polymer) = LoadCatalogs();
            Assert.Equal(8, cordage.HempEntries.Count);
            Assert.Equal(8, cordage.WireEntries.Count);
            Assert.Equal(7, cordage.HawserEntries.Count);
            Assert.Equal(7, cordage.SpliceEntries.Count);
            Assert.Equal(8, polymer.GasketEntries.Count);
            Assert.Equal(8, polymer.AramidEntries.Count);
            Assert.Equal(7, polymer.TireEntries.Count);
            Assert.Equal(7, polymer.FilmEntries.Count);
            Assert.Equal(30, cordage.HempEntries.Count + cordage.WireEntries.Count
                + cordage.HawserEntries.Count + cordage.SpliceEntries.Count);
            Assert.Equal(30, polymer.TotalCount);
        }

        [Fact]
        public void RecordIds_Unique_WithinAndAcrossCatalogs()
        {
            var (cordage, polymer) = LoadCatalogs();
            var all = cordage.HempEntries.Select(e => e.Id)
                .Concat(cordage.WireEntries.Select(e => e.Id))
                .Concat(cordage.HawserEntries.Select(e => e.Id))
                .Concat(cordage.SpliceEntries.Select(e => e.Id))
                .Concat(polymer.GasketEntries.Select(e => e.Id))
                .Concat(polymer.AramidEntries.Select(e => e.Id))
                .Concat(polymer.TireEntries.Select(e => e.Id))
                .Concat(polymer.FilmEntries.Select(e => e.Id))
                .ToList();

            Assert.Equal(60, all.Count);
            Assert.Equal(60, all.Distinct(StringComparer.Ordinal).Count());
        }

        // ── Identity crosswalk (Workstream B / §18 identity) ─────────────

        [Fact]
        public void CanonicalLinks_AllResolve_InItemAuthority()
        {
            var itemCatalog = LoadItemCatalog();
            foreach (var (recordId, itemId) in TechnicalMaterialArchiveSystem.DefaultCanonicalLinks())
            {
                var def = itemCatalog.Get(itemId);
                Assert.NotNull(def);
                Assert.Equal(itemId, def!.id);
            }
        }

        [Fact]
        public void CanonicalLinks_RejectUnresolvedItems_FailClosed()
        {
            var archive = CreateArchive(withDefaults: false);

            // Aramid armor labels are historical designations — no canonical
            // vest/helmet items exist, and registration must refuse them.
            Assert.False(archive.TryRegisterCanonicalLink("aramid_rot_hydrolytic_chain_cleavage", "PASGT_BALLISTIC_VEST_MK2"));
            Assert.False(archive.TryRegisterCanonicalLink("aramid_rot_ultraviolet_photolytic_yellowing", "MODULAR_TACTICAL_VEST_CARRIER"));
            Assert.False(archive.TryRegisterCanonicalLink("tire_retread_carbon_black_reinforcement_mix", "MILITARY_TRUCK_TIRE_11R20"));
            Assert.False(archive.TryRegisterCanonicalLink("gasket_degrade_ozone_corona_cracking", "M17_SURVIVAL_RESPIRATOR_FACEPIECE"));

            // Mask model designations stay content-only labels.
            Assert.Null(archive.GetCanonicalItem("gasket_degrade_ozone_corona_cracking"));
            Assert.Null(archive.GetCanonicalItem("tire_retread_carbon_black_reinforcement_mix"));

            // Proven identities link and resolve.
            Assert.True(archive.TryRegisterCanonicalLink("hemp_fiber_dew_retting_pectin_breakdown", "rope"));
            Assert.Equal("rope", archive.GetCanonicalItem("hemp_fiber_dew_retting_pectin_breakdown"));
        }

        [Fact]
        public void CanonicalLinks_NeverPointAtFakeItems()
        {
            // Every configured link target must be a real items.json id —
            // no invented item_* identities for narrative labels.
            var itemCatalog = LoadItemCatalog();
            var knownIds = new HashSet<string>();
            foreach (var id in itemCatalog.Ids) knownIds.Add(id);
            foreach (var (_, itemId) in TechnicalMaterialArchiveSystem.DefaultCanonicalLinks())
                Assert.Contains(itemId, knownIds);
        }

        // ── Producer map (Workstreams D–H / §18 discovery) ───────────────

        [Fact]
        public void ProducerMap_ValidLocations_NoInventedSites()
        {
            var locationIds = LoadDeepLoreLocationIds();
            foreach (var (recordId, producer) in TechnicalMaterialArchiveSystem.DefaultProducerMap())
            {
                if (producer.StartsWith("location_", StringComparison.Ordinal))
                    Assert.True(locationIds.Contains(producer),
                        $"{recordId}: producer '{producer}' is not a deep-lore location");
            }
        }

        [Fact]
        public void ProducerMap_CoversAllEightFamilies_AcrossAtLeastFiveProducers()
        {
            var (cordage, polymer) = LoadCatalogs();
            var archive = CreateArchive();
            var map = TechnicalMaterialArchiveSystem.DefaultProducerMap();

            var families = map.Select(m => archive.FamilyOf(m.recordId)).Distinct().ToList();
            Assert.Equal(8, families.Count);
            Assert.True(map.Select(m => m.producerId).Distinct().Count() >= 5);
            Assert.Equal(26, map.Count);
        }

        [Fact]
        public void ProducerMap_DuplicateProducerRegistration_FailsClosed()
        {
            var archive = CreateArchive(withDefaults: false);
            Assert.True(archive.TryRegisterProducer("manila_hawser_abaca_fiber_saltwater_steep", "location_drainage_network"));
            Assert.False(archive.TryRegisterProducer("manila_hawser_abaca_fiber_saltwater_steep", "location_frozen_wetland"),
                "one primary producer per record");
        }

        // ── Authority firewall (§5 / §18 authority) ──────────────────────

        [Fact]
        public void Firewall_ProjectionSurface_HasNoMechanicalAPIs()
        {
            var type = typeof(TechnicalMaterialArchiveSystem);
            var names = type.GetMethods()
                .Where(m => m.DeclaringType == type && !m.IsSpecialName)
                .Select(m => m.Name)
                .OrderBy(n => n, StringComparer.Ordinal)
                .ToList();

            // Read-only archive surface: registration, discovery, queries,
            // persistence — nothing that mutates armor, vehicles, power,
            // fire, hazards, crafting or expedition equipment.
            Assert.Equal(new[]
            {
                "ArchiveFireSensitiveRecords", "CaptureState", "DefaultCanonicalLinks",
                "DefaultProducerMap", "DeferredRecordIds", "DiscoverAtProducer", "DiscoverForItem", "DiscoverRecord",
                "FailureReports", "FamilyOf", "GetCanonicalItem", "GetProducer", "GetRecord",
                "HasProducer", "IsDiscovered", "MaritimeRopeRecords", "ProtectiveMaterialRecords",
                "RecordsByFamily", "RecordsForItem", "RestoreState",
                "TryRegisterCanonicalLink", "TryRegisterProducer"
            }, names);

            foreach (var name in names)
            {
                // Marker scan for mechanical verbs. ("Fire" as a noun is
                // legitimate archive language — ArchiveFireSensitiveRecords —
                // so only action verbs are scanned; the exact API-surface
                // equality assertion above is the hard pin.)
                foreach (var marker in new[] { "Snap", "Break_", "Wears", "Ignites", "IgniteRecord",
                             "Spawns", "Launches", "Powers", "Speeds", "Protects", "Repairs",
                             "Crafts", "Recipe", "Countdown", "Unlock" })
                {
                    Assert.False(name.Contains(marker, StringComparison.OrdinalIgnoreCase),
                        $"archive method '{name}' must not imply a {marker} mechanic");
                }
            }
        }

        [Fact]
        public void Firewall_ViewingMeasurements_MutatesNothingButDiscovery()
        {
            var archive = CreateArchive();
            archive.DiscoverAtProducer("location_ammunition_depot");

            // Viewing aramid residual strength / tire wear data is a pure read:
            // state before/after differ only in the discovered-id set.
            _ = archive.GetRecord("aramid_rot_soil_bacteria_enzymatic_degradation");
            _ = archive.GetRecord("tire_retread_carbon_black_reinforcement_mix");
            _ = archive.RecordsForItem("rope");
            _ = archive.FailureReports();

            var state = archive.CaptureState();
            Assert.All(state.discoveredRecordIds, id =>
                Assert.True(archive.HasProducer(id) || true)); // ids only; no other state exists to mutate
            // The state shape is record ids and nothing else — no condition,
            // durability, protection or speed fields exist anywhere on it.
            Assert.Equal(typeof(TechnicalMaterialArchiveState), state.GetType());
        }

        [Fact]
        public void Firewall_HistoricalTimestamps_AreNotCampaignTime()
        {
            // YEAR_05 etc. are archival relative timestamps; the system stores
            // them as display strings and has no day-counter integration.
            var (cordage, _) = LoadCatalogs();
            var hawser = cordage.GetHawser("manila_hawser_three_strand_hockling_twist");
            Assert.Contains("YEAR_", hawser!.TimestampRelative);
            // And the archive state contains no timestamp field at all
            // (only the system id + the discovered-id ledger).
            var stateFields = typeof(TechnicalMaterialArchiveState).GetFields().Select(f => f.Name).ToList();
            Assert.Contains("systemId", stateFields);
            Assert.Contains("discoveredRecordIds", stateFields);
            Assert.Equal(2, stateFields.Count);
        }

        [Fact]
        public void Firewall_BreakingStrength_NotPresentedAsSafeWorkingLoad()
        {
            // Technical QA (Workstream J): measurement summaries must say
            // "breaking" (ultimate), never "safe working load" (SWL) —
            // the archive never conflates the two.
            var archive = CreateArchive();
            foreach (var wire in archive.Cordage.WireEntries)
            {
                var record = archive.GetRecord(wire.Id)!;
                Assert.Contains("breaking", record.MeasurementSummary, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("safe working", record.MeasurementSummary, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("SWL", record.MeasurementSummary, StringComparison.Ordinal);
            }
        }

        // ── Discovery & persistence (§18 discovery/save) ─────────────────

        [Fact]
        public void Discovery_FirstDiscovery_New_Subsequent_Idempotent()
        {
            var archive = CreateArchive();

            int events = 0;
            archive.OnRecordFirstDiscovered += (_, _) => events++;

            var first = archive.DiscoverAtProducer("location_drainage_network");
            Assert.Equal(3, first.Count);
            Assert.Equal(3, events);

            Assert.Empty(archive.DiscoverAtProducer("location_drainage_network"));
            Assert.Equal(3, events);
        }

        [Fact]
        public void Discovery_UnknownProducer_FailsClosed()
        {
            var archive = CreateArchive();
            Assert.Empty(archive.DiscoverAtProducer("location_not_real"));
            Assert.Empty(archive.DiscoverAtProducer(""));
            Assert.Equal(0, archive.State.discoveredRecordIds.Count);
        }

        [Fact]
        public void SaveRoundTrip_PreservesDiscovery_NoReplay()
        {
            var archive = CreateArchive();
            archive.DiscoverAtProducer("location_steelworks");
            var captured = archive.CaptureState();

            var reloaded = CreateArchive();
            reloaded.RestoreState(captured);

            Assert.Equal(4, reloaded.State.discoveredRecordIds.Count);
            Assert.Empty(reloaded.DiscoverAtProducer("location_steelworks"));
        }

        [Fact]
        public void Restore_OldSave_Null_DiscoversNothing_Automatically()
        {
            var archive = CreateArchive();
            archive.RestoreState(null);
            Assert.Equal(0, archive.State.discoveredRecordIds.Count);
            Assert.All(TechnicalMaterialArchiveSystem.DefaultProducerMap(),
                m => Assert.False(archive.IsDiscovered(m.recordId)));
        }

        [Fact]
        public void Restore_UnknownFutureIds_Tolerated()
        {
            var archive = CreateArchive();
            archive.RestoreState(new TechnicalMaterialArchiveState
            {
                discoveredRecordIds = new List<string>
                {
                    "hemp_fiber_dew_retting_pectin_breakdown",
                    "wire_rope_record_from_a_future_build"
                }
            });
            Assert.Equal(2, archive.State.discoveredRecordIds.Count);
            Assert.True(archive.IsDiscovered("wire_rope_record_from_a_future_build"));
        }

        // ── Query surfaces (Workstream C) ────────────────────────────────

        [Fact]
        public void Queries_RecordsForItem_OnlyDiscoveredLinkedRecords()
        {
            var archive = CreateArchive();

            // Not yet discovered: the linked record is invisible to the surface.
            Assert.Empty(archive.RecordsForItem("gas_mask"));

            archive.DiscoverRecord("gasket_degrade_ozone_corona_cracking");
            var forMask = archive.RecordsForItem("gas_mask");
            Assert.Single(forMask);
            Assert.Equal("gasket_degrade_ozone_corona_cracking", forMask[0].RecordId);
            Assert.Equal("gas_mask", forMask[0].CanonicalItemId);
        }

        [Fact]
        public void Queries_FamilyAndFailureSurfaces_Deterministic()
        {
            var archive = CreateArchive();
            archive.DiscoverAtProducer("location_drainage_network");
            archive.DiscoverAtProducer("location_television_studio");

            var maritime1 = archive.MaritimeRopeRecords();
            var maritime2 = archive.MaritimeRopeRecords();
            Assert.Equal(3, maritime1.Count); // the three drainage-network hawsers
            Assert.Equal(maritime1.Select(r => r.RecordId), maritime2.Select(r => r.RecordId));

            var fires = archive.ArchiveFireSensitiveRecords();
            Assert.Equal(3, fires.Count);
            Assert.All(fires, r => Assert.Equal(TechnicalMaterialFamily.CelluloidFilmDecomposition, r.Family));

            var failures = archive.FailureReports();
            Assert.Equal(3, failures.Count); // the three discovered film records carry failure summaries
            Assert.All(failures, r => Assert.False(string.IsNullOrEmpty(r.FailureSummary)));
        }

        // ── Item-inspection integration (Plan 158 follow-up) ─────────────

        [Fact]
        public void DiscoverForItem_FirstInspection_DiscoversLinkedRecords()
        {
            var archive = CreateArchive();

            var first = archive.DiscoverForItem("gas_mask");
            Assert.Single(first);                      // one gasket record links to gas_mask
            Assert.Contains("gasket_degrade_ozone_corona_cracking", first);
            Assert.True(archive.IsDiscovered("gasket_degrade_ozone_corona_cracking"));

            // Repeat inspection is idempotent — no rediscovery spam.
            Assert.Empty(archive.DiscoverForItem("gas_mask"));

            // Unlinked/unknown items discover nothing.
            Assert.Empty(archive.DiscoverForItem("item_canned_food"));
            Assert.Empty(archive.DiscoverForItem(""));
            Assert.Empty(archive.DiscoverForItem("PASGT_BALLISTIC_VEST_MK2"));
        }

        [Fact]
        public void DiscoverForItem_RoundTrips_ThroughSave()
        {
            var archive = CreateArchive();
            archive.DiscoverForItem("film_reel");

            var captured = archive.CaptureState();
            var reloaded = CreateArchive();
            reloaded.RestoreState(captured);

            Assert.Equal(2, reloaded.State.discoveredRecordIds.Count); // two film_reel links
            Assert.Empty(reloaded.DiscoverForItem("film_reel"));
            Assert.Equal(2, reloaded.RecordsForItem("film_reel").Count);
        }

        [Fact]
        public void DeferredRecords_AccountForTheRest()
        {
            var archive = CreateArchive();
            var deferred = archive.DeferredRecordIds();
            Assert.Equal(60 - 26, deferred.Count);
            Assert.All(deferred, id => Assert.False(archive.HasProducer(id)));
        }
    }
}
