// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Disease;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    /// <summary>
    /// B5–B8 §27 expansion 4: the three-piece integration slice —
    /// (a) the formal defense snapshot (typed readiness counts, breach
    ///     surface, alarm states);
    /// (b) unsafe-water exposure rules (dose→disease mapping, bounded);
    /// (c) emergency priority presets (deterministic Core policy).
    /// </summary>
    public class Expansion4IntegrationTests
    {
        // ─── (a) Defense snapshot ──────────────────────────────────────────

        private static PerimeterDefenseSystem MakeFunded()
        {
            var defs = PerimeterDefenseCatalogLoader.Load(
                System.IO.Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data"),
                new FileSystemIO(), new SystemTextJsonSerializer());
            var inv = new Inventory.Inventory();
            inv.AddById("sandbags", 20); inv.AddById("scrap_wood", 20);
            inv.AddById("scrap_metal", 50); inv.AddById("electrical_wire", 20);
            inv.AddById("item_sentry_targeting_chip", 5);
            return new PerimeterDefenseSystem(defs, inv, new SeededRng(207));
        }

        [Fact]
        public void DefenseSnapshot_TypedCounts_ReflectPhysicalState()
        {
            var p = MakeFunded();
            p.ConstructEmplacement("def_sandbag_berm", true);          // barrier (gate sector)
            p.ConstructEmplacement("def_sentry_turret_9mm", true);     // turret, no ammo

            var snap = p.GetEncounterSnapshot();
            Assert.Equal(2, snap.emplacements_ready);
            Assert.Equal(0, snap.turrets_ready); // built but magazine empty
            Assert.Equal(0, snap.emplacements_disabled);
            // Auto-sectoring: first emplacement → PerimeterSector.All[1] (east).
            Assert.False(snap.unguarded_sectors.Contains("east"));

            // Destroy the sandbag: it leaves the ready count, joins disabled,
            // and its sector honestly reads unguarded.
            var sandbag = p.Emplacements[0];
            sandbag.current_hp = 0;
            sandbag.is_destroyed = true;
            sandbag.is_active = false;
            var degraded = p.GetEncounterSnapshot();
            Assert.Equal(1, degraded.emplacements_disabled);
            Assert.Equal(1, degraded.emplacements_ready);
            Assert.True(degraded.unguarded_sectors.Contains("east")); // its sector honestly reads unguarded
        }

        [Fact]
        public void DefenseSnapshot_UnguardedSectorList_IsCanonical()
        {
            var p = MakeFunded();
            var snap = p.GetEncounterSnapshot();
            // Empty perimeter: every canonical sector honestly unguarded.
            foreach (var sectorId in PerimeterSector.All)
                Assert.Contains(sectorId, snap.unguarded_sectors);
        }

        // ─── (b) Unsafe-water exposure rules ───────────────────────────────

        [Fact]
        public void WaterborneRules_CleanOutput_ExposesNobody()
        {
            Assert.False(WaterborneExposureRules.ShouldRunExposureSweep(0f));
            Assert.False(WaterborneExposureRules.ShouldRunExposureSweep(0.005f));
            Assert.Empty(WaterborneExposureRules.DiseasesFor(0f));
        }

        [Fact]
        public void WaterborneRules_ContaminatedDraw_MapsToAuthoredWaterborneDiseases()
        {
            var ordinary = WaterborneExposureRules.DiseasesFor(0.5f);
            Assert.Equal(new[] { DiseaseIds.TyphoidWaterborne }, ordinary);

            var severe = WaterborneExposureRules.DiseasesFor(3f);
            Assert.Equal(new[] { DiseaseIds.TyphoidWaterborne, DiseaseIds.Dysentery }, severe);

            // Dose grows probability but never guarantees infection — and the
            // mapping is bounded + deterministic.
            float low = WaterborneExposureRules.ProbabilityModifierFor(0.5f);
            float high = WaterborneExposureRules.ProbabilityModifierFor(10f);
            Assert.InRange(low, WaterborneExposureRules.ProbabilityModifierMin,
                WaterborneExposureRules.ProbabilityModifierMax);
            Assert.Equal(WaterborneExposureRules.ProbabilityModifierMax, high, 5);
            Assert.True(high > low);
        }

        [Fact]
        public void WaterborneRules_AuthoredIds_ResolveInCatalog()
        {
            // Every id the rules emit must resolve in the data authority.
            var data = System.IO.Path.Combine(AppContext.BaseDirectory,
                "../../../..", "Assets/StreamingAssets/Data/disease_catalog.json");
            var json = System.Text.Json.JsonDocument.Parse(System.IO.File.ReadAllText(data));
            var ids = new HashSet<string>();
            foreach (var el in json.RootElement.EnumerateObject())
            {
                if (el.Value.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var row in el.Value.EnumerateArray())
                        if (row.TryGetProperty("id", out var id))
                            ids.Add(id.GetString()!);
                }
            }
            Assert.Contains(DiseaseIds.TyphoidWaterborne, ids);
            Assert.Contains(DiseaseIds.Dysentery, ids);
        }

        // ─── (c) Emergency priority presets ────────────────────────────────

        private static PowerGridSystem MakeGrid()
        {
            var rooms = new List<PowerGridRoom>
            {
                new("room_air_filtration", "Air", 180f, PowerGridRoomPriority.Critical, "fx"),
                new("room_greenhouse", "GH", 160f, PowerGridRoomPriority.Standard, "fx"),
                new("room_lighting_main", "ML", 80f, PowerGridRoomPriority.Low, "fx")
            };
            return new PowerGridSystem(new PowerGridState
            { GenerationWatts = 800f, FuelUnits = 100f, BatteryCapacityWh = 4000f, BatteryReserveWh = 0f },
                rooms, new SeededRng(44));
        }

        [Fact]
        public void ShedPreset_DemotesStandard_KeepsCritical()
        {
            var grid = MakeGrid();
            var changed = grid.ApplyBrownoutShedPreset();

            Assert.Contains("room_greenhouse", changed);
            Assert.Equal(PowerGridRoomPriority.Critical, grid.EffectivePriority("room_air_filtration"));
            Assert.Equal(PowerGridRoomPriority.Low, grid.EffectivePriority("room_greenhouse"));
            Assert.Equal(PowerGridRoomPriority.Low, grid.EffectivePriority("room_lighting_main"));

            // Idempotent: a second application changes nothing.
            Assert.Empty(grid.ApplyBrownoutShedPreset());
        }

        [Fact]
        public void DefaultsPreset_ClearsOverrides_CatalogRulesAgain()
        {
            var grid = MakeGrid();
            grid.ApplyBrownoutShedPreset();
            Assert.Equal(PowerGridRoomPriority.Low, grid.EffectivePriority("room_greenhouse"));

            int cleared = grid.ApplyCatalogDefaultPriorities();
            Assert.True(cleared > 0);
            Assert.Equal(PowerGridRoomPriority.Standard, grid.EffectivePriority("room_greenhouse"));
            Assert.Equal(PowerGridRoomPriority.Low, grid.EffectivePriority("room_lighting_main"));
        }

        [Fact]
        public void ShedPreset_SurvivesRoundTrip()
        {
            var grid = MakeGrid();
            grid.ApplyBrownoutShedPreset();
            var restored = MakeGrid();
            restored.RestoreState(grid.CaptureState());
            Assert.Equal(PowerGridRoomPriority.Low, restored.EffectivePriority("room_greenhouse"));
        }
    }
}
