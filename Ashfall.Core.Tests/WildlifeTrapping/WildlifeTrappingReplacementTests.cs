using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Workstream A — trap-site replacement semantics (flagship trapping tranche).
    ///
    /// Locked core contract (WildlifeTrappingSystem.SetTrap, verified against
    /// source before writing these tests):
    ///
    ///   A site is BLOCKED with "trap_active" iff
    ///       !hasCatch && setDay > 0 && !isBroken
    ///   i.e. a site is REPLACEABLE when it has a pending catch (intentional —
    ///   replacing the trap discards the pending catch, see below), was never
    ///   armed (legacy setDay &lt;= 0), or its trap is broken.
    ///
    ///   Replacement mutates the site IN PLACE: siteId is stable by
    ///   construction; assignedHunterId is overwritten with the hunter passed
    ///   to the call (the host always passes the assigning hunter); all catch
    ///   payload fields are cleared; trapId/durability/check schedule are
    ///   regenerated from the replacement trap definition.
    /// </summary>
    public sealed class WildlifeTrappingReplacementTests
    {
        private static string FindDataDir()
        {
            var dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        private static WildlifeTrappingCatalog LoadCatalog()
        {
            var catalog = WildlifeTrappingCatalogLoader.Load(
                FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(catalog);
            return catalog!;
        }

        private static WildlifeTrappingSystem MakeSystem(WildlifeTrappingCatalog catalog, int seed = 42)
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sys);
            return sys;
        }

        /// <summary>Build a site state via the sanctioned capture/restore path so no test
        /// mutates trap-site fields by hand in the live system.</summary>
        private static WildlifeTrappingSystem SystemWithSites(params TrapSite[] sites)
        {
            var sys = MakeSystem(LoadCatalog());
            var state = new WildlifeTrappingState();
            state.trapSites.AddRange(sites);
            sys.RestoreState(state);
            return sys;
        }

        private static TrapSite MakeSite(string siteId, string trapId, string trapType,
            string hunterId, int setDay, int checkDay, int interval, int durability,
            bool isBroken = false, bool hasCatch = false, string catchSpecies = "")
        {
            return new TrapSite
            {
                siteId = siteId,
                assignedHunterId = hunterId,
                trapType = trapType,
                trapId = trapId,
                baitType = "bait_scrap_meat",
                setDay = setDay,
                checkDay = checkDay,
                checkIntervalDays = interval,
                remainingDurability = durability,
                isBroken = isBroken,
                hasCatch = hasCatch,
                catchSpecies = catchSpecies
            };
        }

        private static string SiteJson(WildlifeTrappingSystem sys, string siteId)
        {
            var site = sys.State.trapSites.Single(s => s.siteId == siteId);
            return new SystemTextJsonSerializer().Serialize(site);
        }

        private static TrapSite Site(WildlifeTrappingSystem sys, string siteId)
            => sys.State.trapSites.Single(s => s.siteId == siteId);

        // ── Broken trap replacement ────────────────────────────────────────

        [Fact]
        public void Replace_BrokenTrap_Succeeds_PreservesSiteAndHunterIdentity()
        {
            var catalog = LoadCatalog();
            var sys = SystemWithSites(
                MakeSite("site_a", "trap_snare", "snare", "hunter_1", setDay: 1, checkDay: 3,
                    interval: 2, durability: 0, isBroken: true));

            var def = catalog.Traps["trap_snare"];
            var res = sys.SetTrap("site_a", "bait_scrap_meat", "hunter_1",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.True(res.IsSuccess, $"broken trap must be replaceable, got '{res.FailureCode}'");
            Assert.Single(sys.State.trapSites); // same logical site, no new site appended
            var site = Site(sys, "site_a");
            Assert.Equal("site_a", site.siteId);
            Assert.Equal("hunter_1", site.assignedHunterId);
            Assert.Equal("trap_snare", site.trapId);
        }

        [Fact]
        public void Replace_BrokenTrap_RegeneratesFreshEquipmentState()
        {
            var catalog = LoadCatalog();
            var sys = SystemWithSites(
                MakeSite("site_a", "trap_snare", "snare", "hunter_1", setDay: 1, checkDay: 3,
                    interval: 2, durability: 0, isBroken: true));

            sys.TickDay(10, 0f); // freeze RNG/catch work: density 0, but day is now 10
            var def = catalog.Traps["trap_snare"];
            var res = sys.SetTrap("site_a", "bait_scrap_meat", "hunter_1",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.True(res.IsSuccess);
            var site = Site(sys, "site_a");
            Assert.False(site.isBroken);
            Assert.Equal(def.durabilityChecks, site.remainingDurability);
            Assert.Equal(def.checkIntervalDays, site.checkIntervalDays);
            Assert.Equal(10 + def.checkIntervalDays, site.checkDay);
            Assert.Equal(10, site.setDay);
            Assert.False(site.hasCatch);
            Assert.Equal(string.Empty, site.diseaseId);
            Assert.Equal(0f, site.contaminationDose);
        }

        [Theory]
        [InlineData("trap_snare", 2, 8)]
        [InlineData("trap_fish", 3, 12)]
        public void Replace_CheckDayResetsFromNewTrapInterval(string trapId, int interval, int durability)
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.Traps.ContainsKey(trapId), $"catalog trap missing: {trapId}");
            var sys = SystemWithSites(
                MakeSite("site_a", "trap_improvised_wire", "improvised_wire", "hunter_1",
                    setDay: 1, checkDay: 2, interval: 1, durability: 0, isBroken: true));

            const int replaceDay = 40;
            sys.TickDay(replaceDay, 0f);
            var def = catalog.Traps[trapId];
            Assert.Equal(interval, def.checkIntervalDays); // derive from definition, never hard-code
            var res = sys.SetTrap("site_a", "bait_scrap_meat", "hunter_1",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.True(res.IsSuccess);
            var site = Site(sys, "site_a");
            Assert.Equal(replaceDay + interval, site.checkDay);
            Assert.Equal(durability, site.remainingDurability);
        }

        // ── Healthy active trap rejection ──────────────────────────────────

        [Fact]
        public void Replace_HealthyActiveTrap_BlockedWithTrapActive_AndDoesNotMutate()
        {
            var catalog = LoadCatalog();
            var sys = SystemWithSites(
                MakeSite("site_healthy", "trap_snare", "snare", "hunter_1", setDay: 1, checkDay: 3,
                    interval: 2, durability: 6),
                MakeSite("site_other", "trap_fish", "fish_trap", "hunter_2", setDay: 1, checkDay: 4,
                    interval: 3, durability: 11));

            var before = SiteJson(sys, "site_healthy");
            var otherBefore = SiteJson(sys, "site_other");

            var def = catalog.Traps["trap_snare"];
            var res = sys.SetTrap("site_healthy", "bait_scrap_meat", "hunter_1",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.False(res.IsSuccess);
            Assert.Equal("trap_active", res.FailureCode);
            Assert.Equal(before, SiteJson(sys, "site_healthy"));
            Assert.Equal(otherBefore, SiteJson(sys, "site_other"));
        }

        // ── Pending-catch replacement (intentional contract) ───────────────

        [Fact]
        public void Replace_PendingCatch_Succeeds_AndBeginsClean()
        {
            // INTENTIONAL BEHAVIOR — do not "fix" without a design decision:
            // SetTrap permits replacing a site that holds a pending catch
            // (the guard only blocks !hasCatch && setDay > 0 && !isBroken).
            // Deploying a fresh trap over a pending catch discards that
            // catch; these tests lock the allowance and the clean reset.
            var catalog = LoadCatalog();
            var sys = SystemWithSites(
                MakeSite("site_catch", "trap_snare", "snare", "hunter_1", setDay: 1, checkDay: 3,
                    interval: 2, durability: 4, hasCatch: true, catchSpecies: "rabbit"));

            var def = catalog.Traps["trap_snare"];
            var res = sys.SetTrap("site_catch", "bait_scrap_meat", "hunter_1",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.True(res.IsSuccess, $"pending-catch site must be replaceable, got '{res.FailureCode}'");
            var site = Site(sys, "site_catch");
            Assert.False(site.hasCatch);
            Assert.Equal(string.Empty, site.catchSpecies);
            Assert.False(site.isBroken);
            Assert.Equal(def.durabilityChecks, site.remainingDurability);
        }

        [Fact]
        public void Replace_ClearsEntireCatchPayload_NotJustTheFlag()
        {
            var catalog = LoadCatalog();
            var legacyCatch = MakeSite("site_catch", "trap_snare", "snare", "hunter_1",
                setDay: 1, checkDay: 3, interval: 2, durability: 4,
                hasCatch: true, catchSpecies: "rabbit");
            legacyCatch.bycatchSpecies = "rat";
            legacyCatch.carcassYield = 2.5f;
            legacyCatch.isToxic = true;
            legacyCatch.toxinRemoved = true;
            legacyCatch.isMeatProcessed = true;
            legacyCatch.hidePreserved = true;
            legacyCatch.diseaseId = "disease_zoonotic_flu";
            legacyCatch.contaminationDose = 3.5f;
            var sys = SystemWithSites(legacyCatch);

            var def = catalog.Traps["trap_snare"];
            var res = sys.SetTrap("site_catch", "bait_scrap_meat", "hunter_1",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.True(res.IsSuccess);
            var site = Site(sys, "site_catch");
            Assert.Equal(string.Empty, site.catchSpecies);
            Assert.Equal(string.Empty, site.bycatchSpecies);
            Assert.Equal(0f, site.carcassYield);
            Assert.False(site.isToxic);
            Assert.False(site.toxinRemoved);
            Assert.False(site.isMeatProcessed);
            Assert.False(site.hidePreserved);
            Assert.Equal(string.Empty, site.diseaseId);
            Assert.Equal(0f, site.contaminationDose);
        }

        // ── Legacy trap replacement ─────────────────────────────────────────

        [Fact]
        public void Replace_LegacyNoTrapIdNonActiveSite_Succeeds_AndUpgradesToModernState()
        {
            // Legacy save shape: never armed (setDay <= 0), no catalog link,
            // untracked durability. Not "active" under the legacy-state rules,
            // so replacement must succeed and upgrade the site.
            var catalog = LoadCatalog();
            var legacy = MakeSite("site_legacy", trapId: "", trapType: "snare", hunterId: "hunter_old",
                setDay: -1, checkDay: -1, interval: 2, durability: -1);
            var sys = SystemWithSites(legacy);

            var def = catalog.Traps["trap_snare"];
            var res = sys.SetTrap("site_legacy", "bait_scrap_meat", "hunter_old",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.True(res.IsSuccess, $"legacy non-active trap must be replaceable, got '{res.FailureCode}'");
            var site = Site(sys, "site_legacy");
            Assert.Equal("site_legacy", site.siteId);
            Assert.Equal("hunter_old", site.assignedHunterId);
            Assert.Equal("trap_snare", site.trapId);
            Assert.Equal(def.durabilityChecks, site.remainingDurability);
            Assert.False(site.isBroken);
            Assert.True(site.setDay > 0);
            Assert.True(site.checkDay > site.setDay);
        }

        // ── Multi-site isolation ────────────────────────────────────────────

        [Fact]
        public void Replace_IsolatedToTargetSite_OtherSitesFieldForFieldUnchanged()
        {
            var catalog = LoadCatalog();
            var sys = SystemWithSites(
                MakeSite("site_target", "trap_snare", "snare", "hunter_1", setDay: 1, checkDay: 3,
                    interval: 2, durability: 0, isBroken: true),
                MakeSite("site_healthy", "trap_fish", "fish_trap", "hunter_2", setDay: 2, checkDay: 5,
                    interval: 3, durability: 11),
                MakeSite("site_catch", "trap_cage", "cage", "hunter_3", setDay: 3, checkDay: 5,
                    interval: 2, durability: 9, hasCatch: true, catchSpecies: "rat"));

            var healthyBefore = SiteJson(sys, "site_healthy");
            var catchBefore = SiteJson(sys, "site_catch");

            var def = catalog.Traps["trap_improvised_wire"];
            var res = sys.SetTrap("site_target", "bait_scrap_meat", "hunter_1",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.True(res.IsSuccess);
            Assert.Equal(3, sys.State.trapSites.Count);
            Assert.Equal(healthyBefore, SiteJson(sys, "site_healthy"));
            Assert.Equal(catchBefore, SiteJson(sys, "site_catch"));
            Assert.Equal("trap_improvised_wire", Site(sys, "site_target").trapId);
        }

        [Fact]
        public void SetTrap_NewSite_IsAddedWithoutTouchingExistingSites()
        {
            var catalog = LoadCatalog();
            var sys = SystemWithSites(
                MakeSite("site_existing", "trap_snare", "snare", "hunter_1", setDay: 1, checkDay: 3,
                    interval: 2, durability: 6));
            var before = SiteJson(sys, "site_existing");

            var def = catalog.Traps["trap_fish"];
            var res = sys.SetTrap("site_new", "bait_scrap_meat", "hunter_2",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.True(res.IsSuccess);
            Assert.Equal(2, sys.State.trapSites.Count);
            Assert.Equal(before, SiteJson(sys, "site_existing"));
        }
    }
}
