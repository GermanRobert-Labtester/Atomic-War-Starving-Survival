// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Ashfall.Core.Narrative;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Wave 40 Batch 6 Cross-System Integration Test:
    /// Validates Plan 94 (Verdict Radio Transmissions - 30 machine-register broadcasts)
    /// alongside Plan 102 (Foundry Accords & Faction Treaties - 18 inter-faction treaties).
    /// </summary>
    public sealed class Plan94_102RadioFoundryIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void Plan94_VerdictRadioCatalog_LoadsAll30Broadcasts_WithValidAttributes()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var list = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
            Assert.NotNull(list);
            Assert.Equal(30, list.Count);

            var validKinds = new HashSet<string>(StringComparer.Ordinal)
            {
                "telemetry",
                "maintenance",
                "census",
                "calibration",
                "anomaly",
                "emergency",
                "call",
                "carrier",
                "count",
                "readings",
                "witness"
            };

            var validFrequencies = new HashSet<string>(StringComparer.Ordinal)
            {
                "99.0 MHz",
                "88.5 MHz"
            };

            var seenIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var b in list)
            {
                Assert.True(b.id.StartsWith("radio_verdict_"), $"Id must start with radio_verdict_: {b.id}");
                Assert.True(seenIds.Add(b.id), $"Duplicate id found: {b.id}");
                Assert.Contains(b.frequency, validFrequencies);
                Assert.InRange(b.dayTrigger, 210, 365);
                Assert.Contains(b.kind, validKinds);
                Assert.False(string.IsNullOrWhiteSpace(b.source), $"Source cannot be empty for {b.id}");
                Assert.False(string.IsNullOrWhiteSpace(b.message), $"Message cannot be empty for {b.id}");
                Assert.False(string.IsNullOrWhiteSpace(b.signalStrength), $"SignalStrength cannot be empty for {b.id}");
            }
        }

        [Fact]
        public void Plan102_FoundryAccords_LoadsAll18Treaties_WithProperSignatoryAndQuotas()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            string accordsPath = Path.Combine(DataDirectory, SilentFoundryCatalogLoader.AccordsFileName);
            Assert.True(files.FileExists(accordsPath), $"File must exist at {accordsPath}");

            string raw = files.ReadAllText(accordsPath);
            var catalog = new RegionalTreatyCatalog();
            catalog.Load(raw, json);

            Assert.Equal(18, catalog.AllTreaties.Count);

            var foundryTreaties = catalog.GetByExactSignatoryFaction(SilentFoundryIds.FactionId);
            Assert.Equal(10, foundryTreaties.Count);

            var expectedDistrict8Ids = new[]
            {
                SilentFoundryIds.TreatyBrinePipe,
                SilentFoundryIds.TreatyLabourSchedule,
                SilentFoundryIds.TreatyRoadIron,
                SilentFoundryIds.TreatyClusterCharter,
                SilentFoundryIds.TreatySaltworksAccess,
                SilentFoundryIds.TreatyMembraneRepair,
                SilentFoundryIds.TreatyCoalWindow,
                SilentFoundryIds.TreatyApprenticeExchange,
                SilentFoundryIds.TreatyCrisisMutualAid,
                SilentFoundryIds.TreatyIncidentBook
            };

            foreach (var treatyId in expectedDistrict8Ids)
            {
                var treaty = catalog.GetById(treatyId);
                Assert.NotNull(treaty);
                Assert.InRange(treaty.ratified_day, 280, 365);
                Assert.True(treaty.water_allocation_lpm >= 0f, $"Water allocation must be >= 0 for {treatyId}");
                Assert.True(treaty.power_quota_kw >= 0f, $"Power quota must be >= 0 for {treatyId}");
                Assert.False(string.IsNullOrWhiteSpace(treaty.demarcated_territory), $"Demarcated territory missing for {treatyId}");
                Assert.False(string.IsNullOrWhiteSpace(treaty.tariff_schedule), $"Tariff schedule missing for {treatyId}");
                Assert.False(string.IsNullOrWhiteSpace(treaty.treaty_articles), $"Articles missing for {treatyId}");
                Assert.False(string.IsNullOrWhiteSpace(treaty.penalties), $"Penalties missing for {treatyId}");
                Assert.NotNull(treaty.signatory_factions);
                Assert.Contains(SilentFoundryIds.FactionId, treaty.signatory_factions);
            }
        }

        [Fact]
        public void CrossSystem_RadioTelemetryAndIndustrialTreaties_ExhibitCoherentWastelandTimeline()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var radioList = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
            string accordsPath = Path.Combine(DataDirectory, SilentFoundryCatalogLoader.AccordsFileName);
            string raw = files.ReadAllText(accordsPath);
            var treatyCatalog = new RegionalTreatyCatalog();
            treatyCatalog.Load(raw, json);

            // 1. Operational window alignment: Both machine telemetry broadcasts and Foundry treaties activate in day 280-365 window
            var lateRadio = radioList.Where(r => r.dayTrigger >= 280).ToList();
            Assert.True(lateRadio.Count >= 10, "Expected at least 10 machine broadcasts in the day 280-365 window");

            var lateTreaties = treatyCatalog.AllTreaties.Where(t => t.ratified_day >= 280).ToList();
            Assert.True(lateTreaties.Count >= 10, "Expected at least 10 treaties in the day 280-365 window");

            // 2. Resource monitoring alignment: Radio reports on breaker/substation and valves
            var breakerTest = radioList.Find(r => r.id == "radio_verdict_substation_breaker_test");
            Assert.NotNull(breakerTest);
            Assert.Equal("maintenance", breakerTest.kind);

            // Treaties allocate industrial power quotas
            var brineTreaty = treatyCatalog.GetById(SilentFoundryIds.TreatyBrinePipe);
            Assert.NotNull(brineTreaty);
            Assert.True(brineTreaty.power_quota_kw > 0, "Brine pipe treaty must demand electrical power");
            Assert.True(brineTreaty.water_allocation_lpm > 0, "Brine pipe treaty must allocate water");

            // 3. Water telemetry and greywater service cycles align with saltworks/membrane accords
            var greywaterRadio = radioList.Find(r => r.id == "radio_verdict_service_cycle_greywater");
            Assert.NotNull(greywaterRadio);
            Assert.Equal("maintenance", greywaterRadio.kind);

            var membraneTreaty = treatyCatalog.GetById(SilentFoundryIds.TreatyMembraneRepair);
            Assert.NotNull(membraneTreaty);
            Assert.Contains("membrane", membraneTreaty.treaty_title.ToLowerInvariant());
        }

        [Fact]
        public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string accordsPath = Path.Combine(DataDirectory, SilentFoundryCatalogLoader.AccordsFileName);

            for (int i = 0; i < 50; i++)
            {
                var radioList = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
                Assert.Equal(30, radioList.Count);

                string raw = files.ReadAllText(accordsPath);
                var treatyCatalog = new RegionalTreatyCatalog();
                treatyCatalog.Load(raw, json);
                Assert.Equal(18, treatyCatalog.AllTreaties.Count);
                Assert.Equal(10, treatyCatalog.GetByExactSignatoryFaction(SilentFoundryIds.FactionId).Count);
            }
        }
    }
}
