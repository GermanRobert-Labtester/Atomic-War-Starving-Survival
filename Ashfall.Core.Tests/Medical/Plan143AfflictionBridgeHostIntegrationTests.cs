// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 143: Medical Afflictions → Quest & Work Bridge — Host & Query Tests
// Verifies QueryQuestGate side-effect-free queries, role exclusion mapping,
// census reporting, floor clamping, and recovery behavior.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Plan143AfflictionBridge
{
    public sealed class Plan143AfflictionBridgeHostIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates) { if (File.Exists(c)) return Path.GetFullPath(c); }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        private static AfflictionQuestWorkBridge CreateLoadedBridge()
        {
            var bridge = new AfflictionQuestWorkBridge();
            string path = ResolveDataPath("affliction_bridge_rules.json");
            bridge.LoadCatalog(File.ReadAllText(path));
            return bridge;
        }

        [Fact]
        public void QueryQuestGate_DoesNotFireEvents_AndReturnsExpectedResult()
        {
            var bridge = CreateLoadedBridge();
            bool eventFired = false;
            bridge.OnQuestUnlocked += (_, _) => eventFired = true;
            bridge.OnQuestBlocked += (_, _) => eventFired = true;

            var blockedRes = bridge.QueryQuestGate("expedition_quest", new[] { "affliction_broken_leg" });
            Assert.True(blockedRes.IsBlocked);
            Assert.Contains("affliction_broken_leg", blockedRes.BlockingAfflictionIds);
            Assert.False(eventFired, "QueryQuestGate must be side-effect-free and NOT fire events");

            var unlockRes = bridge.QueryQuestGate("find_anti_rad", new[] { "affliction_radiation_sickness" });
            Assert.False(unlockRes.IsBlocked);
            Assert.True(unlockRes.IsUnlocked);
            Assert.Contains("find_anti_rad", unlockRes.UnlockedQuestTags);
            Assert.Contains("affliction_radiation_sickness", unlockRes.UnlockingAfflictionIds);
            Assert.False(eventFired, "QueryQuestGate must remain side-effect-free when returning unlocks");
        }

        [Fact]
        public void IsRoleExcluded_MapsCanonicalRolesToAuthoredDutyExclusions()
        {
            var bridge = CreateLoadedBridge();

            // Broken leg excludes expedition duty
            bool expExcluded = bridge.IsRoleExcluded(new[] { "affliction_broken_leg" }, "RoleExpedition", out var expDuties);
            Assert.True(expExcluded);
            Assert.Contains("expedition", expDuties);

            // Broken leg does NOT exclude research or cooking
            bool researchExcluded = bridge.IsRoleExcluded(new[] { "affliction_broken_leg" }, "RoleResearch", out _);
            Assert.False(researchExcluded);

            // Radiation sickness excludes food handling -> RoleCook
            bool cookExcluded = bridge.IsRoleExcluded(new[] { "affliction_radiation_sickness" }, "RoleCook", out var cookDuties);
            Assert.True(cookExcluded);
            Assert.Contains("food_handling", cookDuties);

            // Combat trauma excludes guard duty -> RoleGuard
            bool guardExcluded = bridge.IsRoleExcluded(new[] { "affliction_combat_trauma" }, "RoleGuard", out var guardDuties);
            Assert.True(guardExcluded);
            Assert.Contains("guard_duty", guardDuties);
        }

        [Fact]
        public void GetCensus_ReportsValidCountsMatchingAuthoredCatalog()
        {
            var bridge = CreateLoadedBridge();
            var census = bridge.GetCensus();

            Assert.Equal(1, census.SchemaVersion);
            Assert.Equal(6, census.AuthoredWorkModifiersCount);
            Assert.Equal(6, census.AuthoredQuestGatesCount);
            Assert.Equal(6, census.WorkModifierCount);
            Assert.Equal(6, census.QuestGateCount);
        }

        [Fact]
        public void CalculateWorkModifiers_ClampsSpeedFloorAt10Percent()
        {
            var bridge = CreateLoadedBridge();
            var extremeAfflictions = new[]
            {
                "affliction_broken_leg",
                "affliction_broken_arm",
                "affliction_respiratory_degeneration",
                "affliction_radiation_sickness",
                "affliction_chemical_dependency"
            };

            var result = bridge.CalculateWorkModifiers(extremeAfflictions);
            Assert.Equal(0.10f, result.SpeedMultiplier, 2);
            Assert.True(result.QualityMultiplier >= 0.10f);
        }

        [Fact]
        public void HealedSurvivor_RestoresNominalCapacity()
        {
            var bridge = CreateLoadedBridge();
            var healedResult = bridge.CalculateWorkModifiers(Array.Empty<string>());

            Assert.Equal(1.0f, healedResult.SpeedMultiplier, 2);
            Assert.Equal(1.0f, healedResult.QualityMultiplier, 2);
            Assert.Empty(healedResult.ExcludedDutyTypes);

            bool roleExcluded = bridge.IsRoleExcluded(Array.Empty<string>(), "RoleExpedition", out var duties);
            Assert.False(roleExcluded);
            Assert.Empty(duties);

            var gateResult = bridge.QueryQuestGate("expedition_quest", Array.Empty<string>());
            Assert.False(gateResult.IsBlocked);
            Assert.False(gateResult.IsUnlocked);
        }
    }
}
