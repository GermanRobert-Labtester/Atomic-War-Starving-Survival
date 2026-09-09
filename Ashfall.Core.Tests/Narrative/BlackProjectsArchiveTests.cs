using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// PLAN 152 — Black Projects intelligence archive tests.
    /// Pins: 30-record baseline, global ID uniqueness, producer-map validity
    /// (against the real deep-lore location authority), discovery idempotence,
    /// save safety, deterministic relations, and the §4/§17 authority firewall
    /// (archival fields are never executable).
    /// </summary>
    public sealed class BlackProjectsArchiveTests : CatalogTestBase
    {
        private static string NarrativeDir => Path.Combine(DataDirectory, "narrative");

        private static BlackProjectsCatalog LoadCatalog()
            => BlackProjectsCatalog.LoadFromDirectory(NarrativeDir);

        private static HashSet<string> LoadDeepLoreLocationIds()
        {
            // Use the production loader — the DTO uses public fields.
            var entries = DeepLoreLocationCatalogLoader.Load(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            return new HashSet<string>(entries.Select(l => l.id), StringComparer.Ordinal);
        }

        private static BlackProjectsArchiveSystem CreateArchive(BlackProjectsCatalog? catalog = null, bool withProducers = true)
        {
            catalog ??= LoadCatalog();
            var locationIds = LoadDeepLoreLocationIds();
            var archive = new BlackProjectsArchiveSystem(catalog, id => locationIds.Contains(id));
            if (withProducers)
            {
                foreach (var (recordId, producer) in BlackProjectsArchiveSystem.DefaultProducerMap())
                    Assert.True(archive.TryRegisterProducer(recordId, producer),
                        $"producer registration failed: {recordId} -> {producer}");
            }
            return archive;
        }

        // ── Baseline (Task A / §18.1–18.3) ──────────────────────────────

        [Fact]
        public void Catalog_LoadsAll30Records_FamilyCountsPreserved()
        {
            var catalog = LoadCatalog();
            Assert.Equal(8, catalog.OrbitalEntries.Count);
            Assert.Equal(8, catalog.DroneEntries.Count);
            Assert.Equal(7, catalog.CobaltEntries.Count);
            Assert.Equal(7, catalog.VaultEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void RecordIds_GloballyUnique_AcrossAllFourFamilies()
        {
            var catalog = LoadCatalog();
            var all = catalog.OrbitalEntries.Select(e => e.Id)
                .Concat(catalog.DroneEntries.Select(e => e.Id))
                .Concat(catalog.CobaltEntries.Select(e => e.Id))
                .Concat(catalog.VaultEntries.Select(e => e.Id))
                .ToList();

            Assert.Equal(30, all.Count);
            Assert.Equal(30, all.Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void AllRecords_HaveRequiredFields_AndSchemaVersionedFiles()
        {
            var catalog = LoadCatalog();
            foreach (var o in catalog.OrbitalEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(o.Callsign));
                Assert.True(o.OrbitalAltitudeKm > 0);
                Assert.False(string.IsNullOrWhiteSpace(o.PayloadStatus));
                Assert.NotEmpty(o.Tags);
            }
            foreach (var d in catalog.DroneEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(d.CarrierId));
                Assert.False(string.IsNullOrWhiteSpace(d.SystemHealth));
                Assert.True(d.AltitudeFeet >= 0);
                Assert.True(d.AirspeedKnots >= 0);
            }
            foreach (var c in catalog.CobaltEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(c.DirectiveCode));
                Assert.False(string.IsNullOrWhiteSpace(c.IssuingAuthority));
                Assert.True(c.AuthorizedSalvoSize >= 0);
            }
            foreach (var v in catalog.VaultEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(v.VaultId));
                Assert.False(string.IsNullOrWhiteSpace(v.ComplianceStatus));
            }
        }

        // ── Producer map (Task C / §18.5 / §19) ──────────────────────────

        [Fact]
        public void ProducerMap_EveryProducer_ExistsInDeepLoreLocationAuthority()
        {
            var locationIds = LoadDeepLoreLocationIds();
            foreach (var (recordId, producer) in BlackProjectsArchiveSystem.DefaultProducerMap())
            {
                Assert.True(locationIds.Contains(producer),
                    $"{recordId}: producer '{producer}' is not a deep-lore location id — no invented sites");
            }
        }

        [Fact]
        public void ProducerMap_Activates16Records_AcrossAtLeast5Producers()
        {
            var map = BlackProjectsArchiveSystem.DefaultProducerMap();
            Assert.Equal(16, map.Count);
            Assert.True(map.Select(m => m.producerLocationId).Distinct().Count() >= 5);

            // Family coverage: 4 orbital, 4 drone, 4 cobalt, 4 vault (§19).
            var catalog = LoadCatalog();
            Assert.Equal(4, map.Count(m => catalog.GetOrbital(m.recordId) != null));
            Assert.Equal(4, map.Count(m => catalog.GetDrone(m.recordId) != null));
            Assert.Equal(4, map.Count(m => catalog.GetCobalt(m.recordId) != null));
            Assert.Equal(4, map.Count(m => catalog.GetVault(m.recordId) != null));
        }

        [Fact]
        public void DeferredRecords_AreExactlyThe14WithoutProducers()
        {
            var archive = CreateArchive();
            var deferred = archive.DeferredRecordIds();
            Assert.Equal(14, deferred.Count);
            foreach (var id in deferred)
                Assert.False(archive.HasProducer(id));
        }

        [Fact]
        public void Registration_FailsClosed_UnknownRecordOrInvalidProducer()
        {
            var archive = CreateArchive(withProducers: false);

            Assert.False(archive.TryRegisterProducer("record_does_not_exist", "location_radar_site"));
            Assert.False(archive.TryRegisterProducer("telemetry_olympus_perigee_decay", "location_not_in_catalog"));
            Assert.False(archive.TryRegisterProducer("telemetry_olympus_perigee_decay", ""));
            Assert.True(archive.TryRegisterProducer("telemetry_olympus_perigee_decay", "location_radar_site"));
            Assert.False(archive.TryRegisterProducer("telemetry_olympus_perigee_decay", "location_weather_station"),
                "every record has exactly one primary producer");
        }

        // ── Discovery: idempotence & first-discovery discipline (§18.7) ──

        [Fact]
        public void Discovery_FirstDiscovery_IsNew_SecondCall_Idempotent()
        {
            var archive = CreateArchive();

            int firstDiscoveredEvents = 0;
            archive.OnRecordFirstDiscovered += (_, _) => firstDiscoveredEvents++;

            var first = archive.DiscoverAtProducer("location_radar_site");
            Assert.Equal(3, first.Count);
            Assert.Equal(3, firstDiscoveredEvents);

            // Repeat discovery (revisit) is idempotent: no new records, no
            // replayed first-discovery consequences (§17).
            var second = archive.DiscoverAtProducer("location_radar_site");
            Assert.Empty(second);
            Assert.Equal(3, firstDiscoveredEvents);

            // A different producer can still surface records it owns.
            var third = archive.DiscoverAtProducer("location_weather_station");
            Assert.Single(third);
            Assert.Equal(4, firstDiscoveredEvents);
        }

        [Fact]
        public void Discovery_UnknownProducer_FailsClosed_WithoutMutation()
        {
            var archive = CreateArchive();

            var result = archive.DiscoverAtProducer("location_not_in_catalog");
            Assert.Empty(result);
            Assert.Equal(0, archive.State.discoveredRecordIds.Count);

            var empty = archive.DiscoverAtProducer("");
            Assert.Empty(empty);
        }

        [Fact]
        public void Discovery_MutatesOnlyArchiveState_NoWorldMechanicsTouched()
        {
            // §4 firewall: discovery is informative-only. The archive system's
            // entire mutable surface is the discovered-id set — there is no
            // countdown, spawn, launch, clearance or access API to call. Pin
            // the observable contract: discovery changes exactly one thing.
            var archive = CreateArchive();
            var before = archive.CaptureState();

            archive.DiscoverAtProducer("location_ammunition_depot");

            var after = archive.CaptureState();
            Assert.Empty(before.discoveredRecordIds);
            Assert.Equal(4, after.discoveredRecordIds.Count);
            // The state shape contains record ids and nothing else.
            Assert.All(after.discoveredRecordIds, id =>
                Assert.True(id.StartsWith("telemetry_olympus_", StringComparison.Ordinal)
                         || id.StartsWith("blackbox_valkyrie_", StringComparison.Ordinal)
                         || id.StartsWith("directive_cobalt_", StringComparison.Ordinal)
                         || id.StartsWith("audit_architect_vault_", StringComparison.Ordinal)));
        }

        // ── Related-record graph (Task H / §18.10) ───────────────────────

        [Fact]
        public void RelatedGraph_Deterministic_Ordering()
        {
            var archive = CreateArchive();
            archive.DiscoverAtProducer("location_radar_site");
            archive.DiscoverAtProducer("location_ammunition_depot");
            archive.DiscoverAtProducer("location_weather_station");

            var r1 = archive.GetRelated("telemetry_olympus_perigee_decay");
            var r2 = archive.GetRelated("telemetry_olympus_perigee_decay");
            Assert.NotEmpty(r1);
            Assert.Equal(r1.Select(x => x.RecordId), r2.Select(x => x.RecordId));
            Assert.Equal(r1.Select(x => x.RelationKind), r2.Select(x => x.RelationKind));

            var ordered = r1.ToList();
            Assert.Equal(ordered.OrderBy(x => x.RecordId, StringComparer.Ordinal)
                                 .ThenBy(x => x.RelationKind, StringComparer.Ordinal)
                                 .Select(x => (x.RecordId, x.RelationKind)),
                         ordered.Select(x => (x.RecordId, x.RelationKind)));
        }

        [Fact]
        public void RelatedGraph_NeverRevealsUndiscoveredRecords()
        {
            var archive = CreateArchive();
            // Discover ONLY the radar-site records; the curated corroboration
            // partner (target_misidentification) lives at the ammo depot and
            // stays undiscovered.
            archive.DiscoverAtProducer("location_radar_site");

            var related = archive.GetRelated("blackbox_valkyrie_radar_interrogation_loop");
            // Structural relation: the other VALKYRIE-09 blackboxes are not
            // discovered yet — the archive must not spoil them.
            Assert.DoesNotContain(related, r => r.RecordId == "blackbox_valkyrie_target_misidentification");
            Assert.All(related, r => Assert.True(archive.IsDiscovered(r.RecordId)));
        }

        [Fact]
        public void RelatedGraph_StructuralRelations_ByCallsignCarrierVault()
        {
            var archive = CreateArchive();
            // Discover two OLYMPUS-PLATFORM-04 telemetry records.
            archive.DiscoverRecord("telemetry_olympus_perigee_decay");
            archive.DiscoverRecord("telemetry_olympus_solar_array_spallation");

            var related = archive.GetRelated("telemetry_olympus_perigee_decay");
            Assert.Contains(related, r => r.RecordId == "telemetry_olympus_solar_array_spallation"
                                       && r.RelationKind == "related_by_callsign"
                                       && r.Detail == "OLYMPUS-PLATFORM-04");
        }

        [Fact]
        public void RelatedGraph_CuratedCorroboration_SurfacesWhenBothDiscovered()
        {
            var archive = CreateArchive();
            archive.DiscoverRecord("blackbox_valkyrie_radar_interrogation_loop");
            archive.DiscoverRecord("blackbox_valkyrie_target_misidentification");

            var related = archive.GetRelated("blackbox_valkyrie_radar_interrogation_loop");
            Assert.Contains(related, r => r.RecordId == "blackbox_valkyrie_target_misidentification"
                                       && r.RelationKind == "corroborates");
        }

        [Fact]
        public void RelatedGraph_UndiscoveredRecord_ReturnsEmpty()
        {
            var archive = CreateArchive();
            Assert.Empty(archive.GetRelated("telemetry_olympus_perigee_decay"));
        }

        // ── Persistence (§16 / §18.8) ────────────────────────────────────

        [Fact]
        public void SaveRoundTrip_PreservesDiscoveredIds_Exactly()
        {
            var archive = CreateArchive();
            archive.DiscoverAtProducer("location_radar_site");
            var captured = archive.CaptureState();

            var restored = CreateArchive();
            restored.RestoreState(captured);

            Assert.Equal(
                captured.discoveredRecordIds.OrderBy(x => x, StringComparer.Ordinal),
                restored.State.discoveredRecordIds.OrderBy(x => x, StringComparer.Ordinal));
            Assert.All(captured.discoveredRecordIds, id => Assert.True(restored.IsDiscovered(id)));

            // Idempotence survives the round trip (§20: no replayed rewards).
            Assert.Empty(restored.DiscoverAtProducer("location_radar_site"));
        }

        [Fact]
        public void Restore_OldSave_Null_LeavesEmptyArchive_NoWorldEffect()
        {
            var archive = CreateArchive();
            archive.RestoreState(null);
            Assert.Equal(0, archive.State.discoveredRecordIds.Count);
            Assert.All(BlackProjectsArchiveSystem.DefaultProducerMap(),
                m => Assert.False(archive.IsDiscovered(m.recordId)));
        }

        [Fact]
        public void Restore_UnknownFutureIds_ToleratedAndPreservedInertly()
        {
            var archive = CreateArchive();
            archive.RestoreState(new BlackProjectsArchiveState
            {
                discoveredRecordIds = new List<string>
                {
                    "telemetry_olympus_perigee_decay",
                    "telemetry_olympus_record_removed_in_a_later_build",
                    "blackbox_valkyrie_renamed_id"
                }
            });

            // Round-trip preservation: unknown ids stay in the set inertly —
            // they neither crash, nor activate, nor disappear.
            Assert.Equal(3, archive.State.discoveredRecordIds.Count);
            Assert.True(archive.IsDiscovered("telemetry_olympus_record_removed_in_a_later_build"));
        }

        // ── Truth-class taxonomy (Task A) ─────────────────────────────────

        [Fact]
        public void TruthClasses_FollowFamilyTaxonomy()
        {
            var archive = CreateArchive();
            Assert.Equal(BlackProjectsTruthClass.InstrumentTelemetry,
                archive.TruthClassFor("telemetry_olympus_perigee_decay"));
            Assert.Equal(BlackProjectsTruthClass.VehicleBlackbox,
                archive.TruthClassFor("blackbox_valkyrie_takeoff_sortie_01"));
            Assert.Equal(BlackProjectsTruthClass.ClassifiedDirective,
                archive.TruthClassFor("directive_cobalt_dual_key_authorization"));
            Assert.Equal(BlackProjectsTruthClass.ComplianceAudit,
                archive.TruthClassFor("audit_architect_vault_biometric_decay"));
        }

        // ── Executable-field firewall (§17) ──────────────────────────────

        [Fact]
        public void Firewall_CobaltDirective_FieldsAreHistoricalDocumentData()
        {
            // EO-99-Z-COBALT-RELEASE authorizes a 24-round salvo IN 1989 — the
            // archive must expose it as document data with no action attached.
            var catalog = LoadCatalog();
            var directive = catalog.GetCobalt("directive_cobalt_dual_key_authorization");
            Assert.NotNull(directive);
            Assert.Equal(24, directive!.AuthorizedSalvoSize);
            // The record itself carries no action/effect/command field.
            Assert.False(string.IsNullOrWhiteSpace(directive.EffectiveDayRange));
            // And the archive system has no API that could act on it: the only
            // mutation paths are DiscoverAtProducer/DiscoverRecord (proven above).
        }

        [Fact]
        public void Firewall_OrbitalDecay_IsNotACountdown()
        {
            // The archive system exposes no schedule/impact/countdown surface:
            // discovering the terminal deorbit record (even if it were
            // activated) cannot create an event. Pin that the deferred
            // endgame-adjacent records have no producer and no scheduling API
            // exists anywhere on the system.
            var archive = CreateArchive();
            Assert.False(archive.HasProducer("telemetry_olympus_terminal_deorbit_burn"));
            Assert.Contains("telemetry_olympus_terminal_deorbit_burn", archive.DeferredRecordIds());

            var type = typeof(BlackProjectsArchiveSystem);
            var forbidden = new[] { "Schedule", "Countdown", "Impact", "Launch", "Arm", "Spawn", "Unlock", "Grant" };
            foreach (var method in type.GetMethods())
            {
                foreach (var marker in forbidden)
                {
                    Assert.False(method.Name.Contains(marker, StringComparison.OrdinalIgnoreCase),
                        $"archive method '{method.Name}' must not imply {marker} — authority firewall");
                }
            }
        }

        [Fact]
        public void Firewall_BlackboxAndVaultRecords_GrantNothing()
        {
            // No drone/vehicle item, avionics grant, vault access or loot
            // reveal: the archive's public API surface is limited to
            // producer registration, discovery and read-only queries.
            var type = typeof(BlackProjectsArchiveSystem);
            var publicMethods = type.GetMethods()
                .Where(m => m.DeclaringType == type && !m.IsSpecialName)
                .Select(m => m.Name)
                .Distinct()
                .OrderBy(n => n, StringComparer.Ordinal)
                .ToList();

            Assert.Equal(new[]
            {
                "CaptureState", "DefaultProducerMap", "DeferredRecordIds", "DiscoverAtProducer", "DiscoverRecord",
                "GetProducer", "GetRelated", "HasProducer", "IsDiscovered",
                "RestoreState", "TruthClassFor", "TryRegisterProducer"
            }, publicMethods);
        }
    }
}
