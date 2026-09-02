// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Archaeology
{
    public class ArchaeologySystemTests
    {
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
            Assert.True(site.exhausted);
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

            Assert.True(archive.unlocked);
            Assert.False(archive.encrypted);
            Assert.True(archive.researchClaimed);
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
            Assert.True(archive.unlocked);

            bool soldFired = false;
            system.OnArchiveSold += (_, _) => soldFired = true;

            var sellResult = system.SellArchiveToBroker(archive.archiveId);
            Assert.True(sellResult.IsSuccess);
            Assert.True(soldFired);
            Assert.True(archive.sold);
            Assert.True(inv.CountById("scrap_metal") > 0);

            // Cannot sell twice
            var duplicateSell = system.SellArchiveToBroker(archive.archiveId);
            Assert.False(duplicateSell.IsSuccess);
            Assert.Equal("already_sold", duplicateSell.FailureCode);
        }
    }
}
