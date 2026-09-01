using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class SkyLayerArmorCatalogTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            return string.Empty;
        }

        [Fact]
        public void ArmorCatalog_LoadsAllSixAuthoredConfigurations()
        {
            string dataDir = ResolveDataDir();
            var configs = SkyLayerArmorCatalogLoader.Load(dataDir);

            Assert.NotNull(configs);
            Assert.Equal(6, configs.Count);

            var expectedIds = new[]
            {
                "sky_armor_sandbag_layer",
                "sky_armor_scrap_overlay",
                "sky_armor_reinforced_concrete",
                "sky_armor_steel_hull_plating",
                "sky_armor_composite_military",
                "sky_armor_emergency_blast_canopy"
            };

            foreach (var id in expectedIds)
            {
                var cfg = configs.FirstOrDefault(c => c.id == id);
                Assert.NotNull(cfg);
                Assert.False(string.IsNullOrWhiteSpace(cfg.name));
                Assert.False(string.IsNullOrWhiteSpace(cfg.description));
                Assert.True(cfg.default_thickness_meters > 0f);
                Assert.True(cfg.blast_resistance_mj > 0f);
                Assert.True(cfg.attenuation_factor > 0f && cfg.attenuation_factor <= 1f);
                Assert.True(cfg.degradation_rate > 0f);
                Assert.NotEmpty(cfg.composition);
                Assert.NotEmpty(cfg.repair_cost);
            }
        }

        [Fact]
        public void ArmorCatalog_AllMaterialItemReferencesResolveInItemsCatalog()
        {
            string dataDir = ResolveDataDir();
            var configs = SkyLayerArmorCatalogLoader.Load(dataDir);
            string itemsPath = Path.Combine(dataDir, "items.json");
            Assert.True(File.Exists(itemsPath), "items.json must exist");

            string itemsJson = File.ReadAllText(itemsPath);
            using var doc = JsonDocument.Parse(itemsJson);
            var itemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (doc.RootElement.TryGetProperty("items", out var itemArray))
            {
                foreach (var item in itemArray.EnumerateArray())
                {
                    if (item.TryGetProperty("id", out var idProp))
                        itemIds.Add(idProp.GetString() ?? string.Empty);
                }
            }

            foreach (var cfg in configs)
            {
                foreach (var comp in cfg.composition)
                {
                    Assert.False(string.IsNullOrWhiteSpace(comp.item_id));
                    Assert.True(comp.quantity > 0);
                    Assert.True(itemIds.Contains(comp.item_id),
                        $"Armor config '{cfg.id}' references unknown composition item '{comp.item_id}'");
                }

                foreach (var rep in cfg.repair_cost)
                {
                    Assert.False(string.IsNullOrWhiteSpace(rep.item_id));
                    Assert.True(rep.quantity > 0);
                    Assert.True(itemIds.Contains(rep.item_id),
                        $"Armor config '{cfg.id}' references unknown repair item '{rep.item_id}'");
                }
            }
        }

        [Fact]
        public void ArmorCatalog_DefaultConfigurationsFallbackMatchesSixConfigs()
        {
            var defaultConfigs = SkyLayerArmorCatalogLoader.GetDefaultConfigurations();
            Assert.Equal(6, defaultConfigs.Count);

            var ids = defaultConfigs.Select(c => c.id).ToHashSet();
            Assert.Contains("sky_armor_sandbag_layer", ids);
            Assert.Contains("sky_armor_scrap_overlay", ids);
            Assert.Contains("sky_armor_reinforced_concrete", ids);
            Assert.Contains("sky_armor_steel_hull_plating", ids);
            Assert.Contains("sky_armor_composite_military", ids);
            Assert.Contains("sky_armor_emergency_blast_canopy", ids);
        }

        [Fact]
        public void OrbitalThreatCatalog_LoadsTwelveUniqueEvents()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var events = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, json);

            Assert.NotNull(events);
            Assert.Equal(12, events.Count);

            var eventIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var evt in events)
            {
                Assert.True(eventIds.Add(evt.id), $"Duplicate event ID: {evt.id}");
                Assert.False(string.IsNullOrWhiteSpace(evt.name));
                Assert.False(string.IsNullOrWhiteSpace(evt.description));
                if (!evt.is_false_positive)
                {
                    Assert.True(evt.impact_energy_mj > 0f);
                }
                Assert.True(evt.lead_time_days > 0);
                Assert.True(evt.affected_cell_spread >= 1);
                Assert.False(string.IsNullOrWhiteSpace(evt.salvage_yield_item_id));
                Assert.True(evt.salvage_yield_quantity >= 1);
            }
        }

        [Fact]
        public void SkyLayerArmor_InstallationAndAttenuationHierarchy()
        {
            string dataDir = ResolveDataDir();
            var configs = SkyLayerArmorCatalogLoader.Load(dataDir);
            var armor = new SkyLayerArmorSystem();

            int gridX = 0;
            foreach (var cfg in configs)
            {
                armor.InstallConfiguration(gridX, cfg);
                var cell = armor.GetCell(gridX);
                Assert.NotNull(cell);
                Assert.Equal(cfg.material_tier, cell.material);
                Assert.Equal(cfg.default_thickness_meters, cell.thicknessMeters);
                Assert.Equal(100f, cell.currentDurability);

                float attenuation = armor.GetAttenuationFactor(gridX);
                Assert.True(attenuation > 0f && attenuation <= 1f);
                gridX++;
            }

            // Tungsten composite should have much lower attenuation (higher shielding) than dirt
            float dirtAttenuation = armor.GetAttenuationFactor(0); // Sandbag (Dirt)
            float tungstenAttenuation = armor.GetAttenuationFactor(4); // Composite Military (Tungsten)
            Assert.True(tungstenAttenuation < dirtAttenuation);
        }

        [Fact]
        public void SkyLayerArmor_EvaluateImpact_MitigationAndBreach()
        {
            string dataDir = ResolveDataDir();
            var configs = SkyLayerArmorCatalogLoader.Load(dataDir);
            var concreteCfg = configs.First(c => c.id == "sky_armor_reinforced_concrete");

            var armor = new SkyLayerArmorSystem();
            armor.InstallConfiguration(5, concreteCfg);

            // Absorption threshold = 25 * 1.5 = 37.5 MJ
            // Strike 1: 20 MJ -> should be absorbed completely
            bool breached = armor.EvaluateKineticImpact(5, 20f, out float roofDamage);
            Assert.False(breached);
            Assert.Equal(0f, roofDamage);

            var cell = armor.GetCell(5);
            Assert.NotNull(cell);
            Assert.True(cell.currentDurability < 100f);

            // Strike 2: 50 MJ -> exceeds 37.5 MJ -> breach occurs
            breached = armor.EvaluateKineticImpact(5, 50f, out roofDamage);
            Assert.True(breached);
            Assert.Equal(50f - 37.5f, roofDamage, precision: 1);
            Assert.True(cell.currentDurability <= 50f);
        }

        [Fact]
        public void SkyLayerArmor_RepairCell_RestoresDurability()
        {
            var armor = new SkyLayerArmorSystem();
            armor.SetCellArmor(3, CeilingMaterialTier.ReinforcedConcrete, 1.0f, durability: 40f);

            armor.RepairCell(3, 30f);
            var cell = armor.GetCell(3);
            Assert.NotNull(cell);
            Assert.Equal(70f, cell.currentDurability);

            // Cannot exceed 100
            armor.RepairCell(3, 50f);
            Assert.Equal(100f, cell.currentDurability);
        }

        [Fact]
        public void SkyLayerArmor_SaveAndRestore_PreservesAllCells()
        {
            var armor1 = new SkyLayerArmorSystem();
            armor1.SetCellArmor(1, CeilingMaterialTier.Dirt, 0.8f, 75f);
            armor1.SetCellArmor(2, CeilingMaterialTier.LeadSheeting, 1.2f, 90f);
            armor1.SetCellArmor(3, CeilingMaterialTier.TungstenComposite, 2.0f, 100f);

            var state = armor1.CaptureState();
            Assert.Equal(3, state.cells.Count);

            var armor2 = new SkyLayerArmorSystem();
            armor2.RestoreState(state);

            var c1 = armor2.GetCell(1);
            var c2 = armor2.GetCell(2);
            var c3 = armor2.GetCell(3);

            Assert.NotNull(c1);
            Assert.Equal(CeilingMaterialTier.Dirt, c1.material);
            Assert.Equal(75f, c1.currentDurability);

            Assert.NotNull(c2);
            Assert.Equal(CeilingMaterialTier.LeadSheeting, c2.material);
            Assert.Equal(90f, c2.currentDurability);

            Assert.NotNull(c3);
            Assert.Equal(CeilingMaterialTier.TungstenComposite, c3.material);
            Assert.Equal(100f, c3.currentDurability);
        }

        [Fact]
        public void FullDefenseLoop_TelemetryWarning_Brace_Strike_Mitigation_Salvage()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var threatEvents = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, json);
            var configs = SkyLayerArmorCatalogLoader.Load(dataDir);

            var armor = new SkyLayerArmorSystem();
            var concreteCfg = configs.First(c => c.id == "sky_armor_reinforced_concrete");
            armor.InstallConfiguration(4, concreteCfg);

            var telemetry = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(1986));
            telemetry.ActivateTelemetry(1);

            var standardStrike = threatEvents.First(e => e.id == "event_orbital_kinetic_early_track");
            telemetry.ScheduleEventDef(standardStrike, day: 3, gridX: 4);

            Assert.True(telemetry.HasPendingImpact);
            Assert.Single(telemetry.State.warnings);

            // Preparation: Brace before impact
            var braceRes = telemetry.Brace("scrap_metal", 2);
            Assert.True(braceRes.IsSuccess);
            Assert.True(telemetry.State.isBraced);

            OrbitalImpactReport? finalReport = null;
            telemetry.OnImpactDetailed += r => finalReport = r;

            // Advance day to 3 (impact day)
            telemetry.TickDay(3);

            Assert.NotNull(finalReport);
            Assert.Equal("event_orbital_kinetic_early_track", finalReport.EventId);
            Assert.False(telemetry.HasPendingImpact);
            Assert.Equal(3, telemetry.State.lastImpactDay);

            // With bracing, 35 MJ * 0.5 = 17.5 MJ, well below concrete's 37.5 MJ threshold -> No breach
            Assert.False(finalReport.AnyBreached);
            Assert.Equal(0f, finalReport.TotalPenetrationDamage);

            // Salvage opportunity spawned
            Assert.Single(telemetry.ActiveSalvage);
            var salvage = telemetry.ActiveSalvage[0];
            Assert.False(salvage.isClaimed);

            var claimRes = telemetry.ClaimSalvage(standardStrike.id);
            Assert.True(claimRes.IsSuccess);
            Assert.True(salvage.isClaimed);

            // Post-impact repair
            var cell = armor.GetCell(4);
            Assert.NotNull(cell);
            Assert.True(cell.currentDurability < 100f);
            armor.RepairCell(4, 30f);
            Assert.Equal(100f, cell.currentDurability);
        }
    }
}
