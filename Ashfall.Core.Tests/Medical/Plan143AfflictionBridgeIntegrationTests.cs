// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 143: Medical Afflictions → Quest & Work Bridge — Integration Tests
// Verifies catalog loading, work modifier stacking, duty exclusion, quest
// blocking, quest unlocking, and combined multi-affliction behaviour.
// ============================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Plan143AfflictionBridge
{
    public sealed class Plan143AfflictionBridgeIntegrationTests
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

        [Fact]
        public void LoadCatalog_LoadsWorkModifiersAndQuestGates()
        {
            var bridge = new AfflictionQuestWorkBridge();
            string path = ResolveDataPath("affliction_bridge_rules.json");
            Assert.True(File.Exists(path), $"affliction_bridge_rules.json must exist at {path}");

            bridge.LoadCatalog(File.ReadAllText(path));

            var modifiers = bridge.GetAllWorkModifiers();
            Assert.True(modifiers.Count >= 6, $"Expected >= 6 work modifiers, got {modifiers.Count}");
            Assert.Contains(modifiers, m => m.affliction_id == "affliction_broken_leg");
            Assert.Contains(modifiers, m => m.affliction_id == "affliction_combat_trauma");

            var gates = bridge.GetAllQuestGates();
            Assert.True(gates.Count >= 6, $"Expected >= 6 quest gates, got {gates.Count}");
            Assert.Contains(gates, g => g.gate_type == "blocks");
            Assert.Contains(gates, g => g.gate_type == "unlocks");
        }

        [Fact]
        public void CalculateWorkModifiers_BrokenLegReducesSpeedAndExcludesExpedition()
        {
            var bridge = new AfflictionQuestWorkBridge();
            bridge.LoadCatalog(File.ReadAllText(ResolveDataPath("affliction_bridge_rules.json")));

            var result = bridge.CalculateWorkModifiers(new[] { "affliction_broken_leg" });

            Assert.True(result.SpeedMultiplier < 1.0f, "Broken leg should reduce work speed");
            Assert.Equal(0.5f, result.SpeedMultiplier, 2);
            Assert.True(result.IsDutyExcluded("expedition"), "Expedition should be excluded with broken leg");
            Assert.True(result.IsDutyExcluded("heavy_labour"), "Heavy labour should be excluded with broken leg");
        }

        [Fact]
        public void CalculateWorkModifiers_MultipleAfflictionsStackMultiplicatively()
        {
            var bridge = new AfflictionQuestWorkBridge();
            bridge.LoadCatalog(File.ReadAllText(ResolveDataPath("affliction_bridge_rules.json")));

            // Broken leg (0.5) × Combat trauma (0.8) = 0.40 speed
            var result = bridge.CalculateWorkModifiers(new[]
            {
                "affliction_broken_leg",
                "affliction_combat_trauma"
            });

            Assert.Equal(0.5f * 0.8f, result.SpeedMultiplier, 2);
            Assert.True(result.IsDutyExcluded("expedition"));
            Assert.True(result.IsDutyExcluded("combat_duty"));
            Assert.Equal(2, result.ActiveAfflictionIds.Count);
        }

        [Fact]
        public void CheckQuestGates_RadiationSicknessUnlocksAntiRadQuest()
        {
            var bridge = new AfflictionQuestWorkBridge();
            bridge.LoadCatalog(File.ReadAllText(ResolveDataPath("affliction_bridge_rules.json")));

            string? unlockedTag = null;
            bridge.OnQuestUnlocked += (sId, tag) => unlockedTag = tag;

            var result = bridge.CheckQuestGates(
                "survivor_maya",
                "find_anti_rad",
                new[] { "affliction_radiation_sickness" });

            Assert.False(result.IsBlocked);
            Assert.Contains("find_anti_rad", result.UnlockedQuestTags);
            Assert.Equal("find_anti_rad", unlockedTag);
        }

        [Fact]
        public void CheckQuestGates_BrokenLegBlocksExpeditionQuest()
        {
            var bridge = new AfflictionQuestWorkBridge();
            bridge.LoadCatalog(File.ReadAllText(ResolveDataPath("affliction_bridge_rules.json")));

            string? blockedTag = null;
            bridge.OnQuestBlocked += (sId, tag) => blockedTag = tag;

            var result = bridge.CheckQuestGates(
                "survivor_kael",
                "expedition_quest",
                new[] { "affliction_broken_leg" });

            Assert.True(result.IsBlocked);
            Assert.Contains("affliction_broken_leg", result.BlockingAfflictionIds);
            Assert.Equal("expedition_quest", blockedTag);
        }

        [Fact]
        public void GetUnlockedQuestTags_ReturnsAllTagsForMultipleAfflictions()
        {
            var bridge = new AfflictionQuestWorkBridge();
            bridge.LoadCatalog(File.ReadAllText(ResolveDataPath("affliction_bridge_rules.json")));

            var tags = bridge.GetUnlockedQuestTags(new[]
            {
                "affliction_radiation_sickness",   // → find_anti_rad
                "affliction_combat_trauma",        // → ptsd_support
                "affliction_chemical_dependency"   // → detox_program
            });

            Assert.Contains("find_anti_rad", tags);
            Assert.Contains("ptsd_support", tags);
            Assert.Contains("detox_program", tags);
            Assert.Equal(3, tags.Count);
        }
    }
}
