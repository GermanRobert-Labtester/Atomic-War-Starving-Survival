// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Inventory;
using Xunit;
using ArchaeologyState = global::Ashfall.Core.Archaeology.ArchaeologyState;
using ArchaeologySite = global::Ashfall.Core.Archaeology.ExcavationSite;
using ArchaeologyArchive = global::Ashfall.Core.Archaeology.PreWarArchiveInstance;

namespace Ashfall.Core.Tests.Archaeology
{
    public class ArchaeologySystemTests
    {
        [Fact]
        public void SurveyRuins_ReservesArchiveUntilItIsRecovered()
        {
            var system = CreateSystem();
            using var catalogDir = new TemporaryCatalog("""
                {"schema_version":1,"archives":[{"archive_id":"only_archive","title_key":"Only","summary_key":"One","base_work_hours":10,"broker_value":10}]}
                """);
            system.LoadCatalog(catalogDir.Path);

            var first = system.SurveyRuins("zone_one", 1f);
            var second = system.SurveyRuins("zone_two", 1f);

            Assert.NotNull(first);
            Assert.Equal("only_archive", first!.archiveId);
            Assert.Null(second);
        }

        [Fact]
        public void SurveyRuins_CatalogSelectionAndZoneIdentity_AreDeterministic()
        {
            var system = CreateSystem();
            system.LoadCatalog(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
            system.RegisterArchive(new LoreArchiveDef { archive_id = "zzz_archive" });
            system.RegisterArchive(new LoreArchiveDef { archive_id = "aaa_archive" });

            var first = system.SurveyRuins(" LOC_ONE ", 1f);
            var duplicate = system.SurveyRuins("loc_one", 1f);

            Assert.NotNull(first);
            Assert.Equal("aaa_archive", first!.archiveId);
            Assert.NotNull(duplicate);
            Assert.Equal(first.siteId, duplicate!.siteId);
        }

        [Fact]
        public void InvalidCatalogReload_ClearsStaleAuthority()
        {
            var system = CreateSystem();
            Assert.NotNull(system.SurveyRuins("zone_before", 1f));
            using var catalogDir = new TemporaryCatalog("{ invalid json");
            system.LoadCatalog(catalogDir.Path);

            Assert.Null(system.SurveyRuins("zone_after", 1f));
        }

        [Fact]
        public void RestoreSequence_DoesNotReusePersistedSiteIds()
        {
            var system = CreateSystem();
            system.RestoreState(new ArchaeologyState
            {
                sites = new List<ArchaeologySite>
                {
                    new ArchaeologySite
                    {
                        siteId = "site_7_existing",
                        zoneId = "existing_zone",
                        displayName = "Existing",
                        discovered = true,
                        archiveId = "archive_existing"
                    }
                }
            });

            var created = system.SurveyRuins("new_zone", 1f);

            Assert.NotNull(created);
            Assert.NotEqual("site_7_existing", created!.siteId);
            Assert.Equal(2, system.Sites.Count);
        }

        [Fact]
        public void ProgressExcavation_InvalidHours_DoNotRewindOrPoisonProgress()
        {
            var system = CreateSystem();
            var site = system.SurveyRuins("zone_invalid_hours", 1f)!;
            system.ProgressExcavation(site.siteId, 2f);

            Assert.Null(system.ProgressExcavation(site.siteId, -3f));
            Assert.Null(system.ProgressExcavation(site.siteId, float.NaN));

            Assert.Equal(20f, system.Sites[0].excavationProgress);
        }

        [Fact]
        public void ProgressDecryption_InvalidWork_IsBlockedWithoutMutation()
        {
            var system = CreateSystem();
            var site = system.SurveyRuins("zone_decrypt_invalid", 1f)!;
            var recovered = system.ProgressExcavation(site.siteId, 10f)!;

            var negative = system.ProgressDecryption(recovered.archiveId, -1f, 2f, true);
            var nonFinite = system.ProgressDecryption(recovered.archiveId, float.NaN, 2f, true);

            Assert.Equal(ActionResult.StatusKind.Blocked, negative.Status);
            Assert.Equal("invalid_work", negative.FailureCode);
            Assert.Equal(ActionResult.StatusKind.Blocked, nonFinite.Status);
            Assert.Equal(0f, system.Archives[0].decryptionProgress);
        }

        [Fact]
        public void Restore_MalformedState_FiltersAndNormalizes()
        {
            var system = CreateSystem();
            var saved = new ArchaeologyState
            {
                systemId = "  ",
                sites = new List<ArchaeologySite>
                {
                    null!,
                    new ArchaeologySite
                    {
                        siteId = " site_1_zone ",
                        zoneId = " zone ",
                        displayName = " Zone ",
                        discovered = true,
                        excavationProgress = float.NaN,
                        archiveId = " ARCHIVE_A "
                    },
                    new ArchaeologySite
                    {
                        siteId = "site_1_duplicate",
                        zoneId = " zone ",
                        archiveId = "archive_b"
                    }
                },
                archives = new List<ArchaeologyArchive>
                {
                    null!,
                    new ArchaeologyArchive
                    {
                        archiveId = " ARCHIVE_A ",
                        titleKey = " Archive A ",
                        decryptionProgress = float.PositiveInfinity,
                        encrypted = true,
                        researchPoints = -5,
                        brokerValue = float.NaN
                    },
                    new ArchaeologyArchive
                    {
                        archiveId = "archive_a",
                        titleKey = "duplicate"
                    }
                },
                unlockedLoreIds = new List<string> { " ARCHIVE_A ", "archive_a", "ghost_archive" },
                soldArchiveIds = new List<string> { "ghost_archive" }
            };

            system.RestoreState(saved);
            var captured = system.CaptureState();

            Assert.Equal(ArchaeologySystem.SystemId, captured.systemId);
            Assert.Single(captured.sites);
            Assert.Equal("site_1_zone", captured.sites[0].siteId);
            Assert.Equal("zone", captured.sites[0].zoneId);
            Assert.InRange(captured.sites[0].excavationProgress, 0f, 100f);
            Assert.Single(captured.archives);
            Assert.Equal("ARCHIVE_A", captured.archives[0].archiveId);
            Assert.InRange(captured.archives[0].decryptionProgress, 0f, 100f);
            Assert.Equal(0, captured.archives[0].researchPoints);
            Assert.True(float.IsFinite(captured.archives[0].brokerValue));
            Assert.Empty(captured.soldArchiveIds);
        }

        [Fact]
        public void StateQueriesAndEvents_AreDetachedSnapshots()
        {
            var system = CreateSystem();
            ArchaeologySite? discoveredEvent = null;
            ArchaeologyArchive? recoveredEvent = null;
            system.OnExcavationSiteDiscovered += site => discoveredEvent = site;
            system.OnArchiveRecovered += archive => recoveredEvent = archive;

            var site = system.SurveyRuins("zone_snapshot", 1f)!;
            var archive = system.ProgressExcavation(site.siteId, 10f)!;
            system.State.sites.Clear();
            system.State.archives.Clear();
            discoveredEvent!.archiveId = "tampered_site";
            recoveredEvent!.titleKey = "tampered_archive";
            site.exhausted = false;
            archive.unlocked = true;

            Assert.Single(system.Sites);
            Assert.Single(system.Archives);
            Assert.NotEqual("tampered_site", system.Sites[0].archiveId);
            Assert.NotEqual("tampered_archive", system.Archives[0].titleKey);
            Assert.True(system.Sites[0].exhausted);
            Assert.False(system.Archives[0].unlocked);
        }

        [Fact]
        public void SellArchiveToBroker_PaymentFailure_DoesNotConsumeArchive()
        {
            var inventory = new Inventory.Inventory { Capacity = 1 };
            Assert.True(inventory.AddById("scrap_metal", 99));
            var system = CreateSystem(inventory);
            var site = system.SurveyRuins("zone_broker_failure", 1f)!;
            var archive = system.ProgressExcavation(site.siteId, 10f)!;
            system.ProgressDecryption(archive.archiveId, 10f, 5f, true);

            var result = system.SellArchiveToBroker(archive.archiveId);

            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("broker_payment_failed", result.FailureCode);
            Assert.False(system.Archives[0].sold);
            Assert.Empty(system.State.soldArchiveIds);
            Assert.Equal(99, inventory.CountById("scrap_metal"));
        }

        [Fact]
        public void CaptureRestore_AreDeepCopies()
        {
            var system = CreateSystem();
            var site = system.SurveyRuins("zone_copy", 1f)!;
            system.ProgressExcavation(site.siteId, 10f);
            var saved = system.CaptureState();
            saved.sites.Clear();
            saved.archives.Clear();

            Assert.Single(system.Sites);
            Assert.Single(system.Archives);

            var restored = CreateSystem();
            var source = system.CaptureState();
            restored.RestoreState(source);
            source.sites[0].zoneId = "tampered";
            source.archives[0].titleKey = "tampered";

            Assert.Single(restored.Sites);
            Assert.Single(restored.Archives);
            Assert.NotEqual("tampered", restored.Sites[0].zoneId);
            Assert.NotEqual("tampered", restored.Archives[0].titleKey);
        }

        [Fact]
        public void ArchaeologySystem_SurveyRuins_DiscoversExcavationSite()
        {
            var rng = new SeededRng(189);
            var inv = new Inventory.Inventory();
            var system = new ArchaeologySystem(rng, inv);

            bool siteDiscoveredFired = false;
            system.OnExcavationSiteDiscovered += (_) => siteDiscoveredFired = true;

            var site = system.SurveyRuins("loc_ruined_silo", 3.0f);

            Assert.NotNull(site);
            Assert.True(siteDiscoveredFired);
            Assert.Equal("loc_ruined_silo", site.zoneId);
            Assert.False(site.exhausted);
            Assert.NotEmpty(site.archiveId);
        }

        [Fact]
        public void ArchaeologySystem_ProgressExcavation_CompletesAndRecoversArchive()
        {
            var rng = new SeededRng(189);
            var inv = new Inventory.Inventory();
            var system = new ArchaeologySystem(rng, inv);

            var site = system.SurveyRuins("loc_hydro_dam", 2.0f);
            Assert.NotNull(site);

            bool archiveRecoveredFired = false;
            system.OnArchiveRecovered += (_) => archiveRecoveredFired = true;

            // 10 hours labor at 10x multiplier = 100% progress
            var archive = system.ProgressExcavation(site.siteId, 10f);

            Assert.NotNull(archive);
            Assert.True(archiveRecoveredFired);
            Assert.True(system.Sites[0].exhausted);
            Assert.True(archive.encrypted);
            Assert.False(archive.unlocked);
        }

        [Fact]
        public void ArchaeologySystem_ProgressDecryption_RequiresPower_UnlocksLoreAndResearch()
        {
            var rng = new SeededRng(189);
            var inv = new Inventory.Inventory();
            var researchState = new ResearchState();
            var researchSystem = new ResearchSystem(null, researchState);

            var system = new ArchaeologySystem(rng, inv, researchSystem);
            var site = system.SurveyRuins("loc_sub_bunker", 2.0f);
            Assert.NotNull(site);

            var archive = system.ProgressExcavation(site.siteId, 10f);
            Assert.NotNull(archive);

            // Attempt decryption without electrical power
            var noPowerResult = system.ProgressDecryption(archive.archiveId, 2f, 3.0f, hasPower: false);
            Assert.False(noPowerResult.IsSuccess);

            bool loreUnlockedFired = false;
            system.OnLoreUnlocked += (_, _) => loreUnlockedFired = true;

            // Progress with power and engineering skill
            var successResult = system.ProgressDecryption(archive.archiveId, 5f, 3.0f, hasPower: true, hasKeycard: true);
            Assert.True(successResult.IsSuccess);

            Assert.True(system.Archives[0].unlocked);
            Assert.False(system.Archives[0].encrypted);
            Assert.True(system.Archives[0].researchClaimed);
            Assert.True(loreUnlockedFired);
            Assert.Contains(archive.archiveId, system.State.unlockedLoreIds);
            Assert.Contains(archive.archiveId, researchState.unlockedIds);
        }

        [Fact]
        public void ArchaeologySystem_SellArchiveToBroker_GrantsScrap_CannotSellTwice()
        {
            var rng = new SeededRng(189);
            var inv = new Inventory.Inventory();
            var system = new ArchaeologySystem(rng, inv);

            var site = system.SurveyRuins("loc_archive_vault", 2.0f);
            Assert.NotNull(site);
            var archive = system.ProgressExcavation(site.siteId, 10f);
            Assert.NotNull(archive);

            system.ProgressDecryption(archive.archiveId, 10f, 5.0f, hasPower: true);
            Assert.True(system.Archives[0].unlocked);

            bool soldFired = false;
            system.OnArchiveSold += (_, _) => soldFired = true;

            var sellResult = system.SellArchiveToBroker(archive.archiveId);
            Assert.True(sellResult.IsSuccess);
            Assert.True(soldFired);
            Assert.True(system.Archives[0].sold);
            Assert.True(inv.CountById("scrap_metal") > 0);

            // Cannot sell twice
            var duplicateSell = system.SellArchiveToBroker(archive.archiveId);
            Assert.False(duplicateSell.IsSuccess);
            Assert.Equal("already_sold", duplicateSell.FailureCode);
        }

        private static ArchaeologySystem CreateSystem(Inventory.Inventory? inventory = null) =>
            new ArchaeologySystem(new SeededRng(189), inventory ?? new Inventory.Inventory());

        private sealed class TemporaryCatalog : IDisposable
        {
            public string Path { get; }

            public TemporaryCatalog(string json)
            {
                Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(Path);
                File.WriteAllText(System.IO.Path.Combine(Path, "lore_archives.json"), json);
            }

            public void Dispose()
            {
                try { Directory.Delete(Path, recursive: true); }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }
        }
    }
}
