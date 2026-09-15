// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Xunit;

namespace Ashfall.Core.Tests.Defense
{
    /// <summary>
    /// B5–B8 Phase 7 (Plan 67): research-gated emplacement construction and
    /// the canonical targeting-chip dependency. Research is permission — an
    /// unlocked node never grants a built emplacement (§15.3); basic fieldworks
    /// stay ungated (legacy parity); the data rows carry the gates (aggregated
    /// catalog check with per-row failure output).
    /// </summary>
    public class PerimeterDefensePhase7Tests
    {
        private static PerimeterDefenseSystem MakeSystem(out Inventory.Inventory inv)
        {
            var defs = PerimeterDefenseCatalogLoader.Load(
                System.IO.Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data"),
                new FileSystemIO(), new SystemTextJsonSerializer());
            inv = new Inventory.Inventory();
            // Fund every cost the catalog can charge (fieldworks + turrets).
            inv.AddById("sandbags", 20);
            inv.AddById("scrap_wood", 20);
            inv.AddById("scrap_metal", 50);
            inv.AddById("electrical_wire", 20);
            inv.AddById("item_sentry_targeting_chip", 5);
            return new PerimeterDefenseSystem(defs, inv, new SeededRng(203));
        }

        private static PerimeterDefenseSystem MakeSystem() => MakeSystem(out _);

        [Fact]
        public void Build_UngatedFieldworks_ConstructFreely()
        {
            var sys = MakeSystem(out var inv);
            // Sandbags: no required_knowledge — constructible with capability
            // flag false (legacy parity for pre-gate callers).
            var r = sys.ConstructEmplacement("def_sandbag_berm", hasRequiredCapability: false);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(sys.Emplacements);
            Assert.Equal(16, inv.CountById("sandbags")); // 4 consumed once, atomically
        }

        [Fact]
        public void Build_Turret_WithoutCapability_Blocked_MutatesNothing()
        {
            var sys = MakeSystem(out var inv);
            var r = sys.ConstructEmplacement("def_sentry_turret_9mm", hasRequiredCapability: false);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("missing_knowledge", r.FailureCode);
            Assert.Empty(sys.Emplacements); // no build, no half-state
            Assert.Equal(5, inv.CountById("item_sentry_targeting_chip")); // not consumed on block (seeded 5)
        }

        [Fact]
        public void Build_Turret_WithCapability_Succeeds()
        {
            var sys = MakeSystem();
            var r = sys.ConstructEmplacement("def_sentry_turret_9mm", hasRequiredCapability: true);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(sys.Emplacements);
            Assert.Equal("ammo_9x19", sys.Emplacements[0].required_ammo_type);
        }

        [Fact]
        public void Build_UnknownDefense_Fails()
        {
            var sys = MakeSystem();
            var r = sys.ConstructEmplacement("def_does_not_exist", hasRequiredCapability: true);
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        // ─── Data authority: every gated row resolves + chip is a real cost ───

        // TEST-AGGREGATION: source_rows=8 aggregate_cases=2 saved_cases=6
        // (homogeneous catalog mapping checks with per-row failure output).
        [Fact]
        public void CatalogRows_GatedDefenses_CarryResolvableKnowledgeIds()
        {
            var expected = new (string DefenseId, string KnowledgeId)[]
            {
                ("def_sentry_turret_9mm", "knowledge_automated_sentry_doctrine"),
                ("def_sentry_turret_556", "knowledge_turret_controller_blueprint"),
                ("def_reinforced_outer_gate", "knowledge_fortified_chokepoints"),
                ("def_heavy_barricade", "knowledge_fortified_chokepoints"),
            };
            var sys = MakeSystem();
            foreach (var (defenseId, knowledgeId) in expected)
            {
                var def = sys.FindDefinition(defenseId);
                Assert.True(def != null, $"{defenseId}: row missing from catalog");
                Assert.True(def!.required_knowledge == knowledgeId,
                    $"{defenseId}: expected gate {knowledgeId}, found '{def.required_knowledge}'");
            }
        }

        [Fact]
        public void CatalogRows_Turrets_RequireCanonicalTargetingChip()
        {
            // The chip was catalog-only with zero consumers; it is now a real
            // construction dependency of both turret emplacements (§10.8).
            var sys = MakeSystem();
            foreach (var turretId in new[] { "def_sentry_turret_9mm", "def_sentry_turret_556" })
            {
                var def = sys.FindDefinition(turretId);
                Assert.True(def != null, $"{turretId}: row missing");
                Assert.True(def!.build_costs.TryGetValue("item_sentry_targeting_chip", out int n) && n >= 1,
                    $"{turretId}: build_costs must consume item_sentry_targeting_chip");
                Assert.True(def.power_draw_watts > 0, $"{turretId}: turrets are powered loads");
            }
        }

        [Fact]
        public void CatalogRows_TripwireEarlyWarning_IsResearchGated()
        {
            // B5–B8 Phase 9 (§16.4 honesty): the tripwire node's description
            // promises early warning rigs; the flare line is that rig and is
            // now gated on the node (basic fieldworks remain ungated).
            var sys = MakeSystem();
            var def = sys.FindDefinition("def_tripwire_flare_line");
            Assert.True(def != null, "def_tripwire_flare_line: row missing");
            Assert.True(def!.alert_device, "tripwire must be an alert device");
            Assert.Equal("knowledge_defensive_tripwire_arrays", def.required_knowledge);
        }
    }
}
