// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunWaterSourcesSelfTest(string dataDirectory)
        {
            int failures = 0;
            int passed = 0;
            void Check(bool condition, string name)
            {
                if (condition)
                {
                    passed++;
                    GD.Print("[PASS] " + name);
                }
                else
                {
                    failures++;
                    GD.PrintErr("[FAIL] " + name);
                }
            }

            try
            {
                var inventory = new Inventory();
                var power = PowerGridHostSession.CreateDefault(new SeededRng(503), dataDirectory);
                var water = new WaterTreatmentSystem(new GodotLog());
                var deepWell = new DeepWellHostSession(new DeepWellSystem(power.System, water));
                var condenser = new WaterCondenserHostSession(
                    new AtmosphericCondenserSystem(power.System, water, new WeatherSystem()));
                var piezometerEngine = new AquiferPiezometerEngine(log: new GodotLog());
                piezometerEngine.BindInventory(
                    itemId => inventory.CountById(itemId),
                    (itemId, amount) => inventory.TryConsumeById(itemId, amount));
                var piezometer = new PiezometerHostSession(piezometerEngine);
                piezometer.LoadCatalog(dataDirectory);
                var research = new ResearchSystem(log: new GodotLog());
                using var session = new WaterSourcesHostSession(
                    inventory, power.System, research, deepWell, condenser, piezometer);

                inventory.AddById(DeepWellSystem.BuildItemId, 1);
                inventory.AddById("mechanical_parts", 2);
                inventory.AddById(AtmosphericCondenserSystem.MembraneItemId, 2);
                inventory.AddById("metal_pipe", 2);
                inventory.AddById("scrap_metal", 10);
                inventory.AddById("item_groundwater_sensor", 3);
                inventory.AddById("item_bedrock_sensor_rig", 1);

                Check(!session.CanBuildDeepWell, "research gates well build control");
                int actuatorBefore = inventory.CountById(DeepWellSystem.BuildItemId);
                Check(!session.TryBuildDeepWell()
                    && inventory.CountById(DeepWellSystem.BuildItemId) == actuatorBefore
                    && !deepWell.System.IsBuilt, "blocked well build preserves inventory and state");
                research.UnlockManual(DeepWellSystem.RequiredKnowledgeId);
                Check(session.CanBuildDeepWell && session.TryBuildDeepWell()
                    && deepWell.System.IsBuilt
                    && inventory.CountById(DeepWellSystem.BuildItemId) == 0
                    && inventory.CountById("mechanical_parts") == 0,
                    "well build commits its complete bill exactly once");
                Check(!session.TryBuildDeepWell(), "repeat well build is blocked");

                research.UnlockManual(AtmosphericCondenserSystem.RequiredKnowledgeId);
                Check(session.CanBuildCondenser && session.TryBuildCondenser()
                    && condenser.System.IsBuilt
                    && inventory.CountById(AtmosphericCondenserSystem.MembraneItemId) == 1
                    && inventory.CountById("metal_pipe") == 0
                    && inventory.CountById("scrap_metal") == 6,
                    "condenser build commits its complete bill exactly once");

                int sensorsBefore = inventory.CountById("item_groundwater_sensor");
                int scrapBefore = inventory.CountById("scrap_metal");
                Check(session.CanConstructPiezometer && session.TryConstructPiezometer()
                    && piezometer.System.IsConstructed
                    && inventory.CountById("item_groundwater_sensor") == sensorsBefore - 3
                    && inventory.CountById("scrap_metal") == scrapBefore - 6
                    && inventory.CountById("item_bedrock_sensor_rig") == 0,
                    "piezometer catalog bill is consumed once atomically");

                inventory.AddById("item_groundwater_sensor", 3);
                inventory.AddById("scrap_metal", 6);
                inventory.AddById("item_bedrock_sensor_rig", 1);
                int duplicateSensorsBefore = inventory.CountById("item_groundwater_sensor");
                int duplicateScrapBefore = inventory.CountById("scrap_metal");
                int duplicateRigBefore = inventory.CountById("item_bedrock_sensor_rig");
                Check(!session.TryConstructPiezometer()
                    && inventory.CountById("item_groundwater_sensor") == duplicateSensorsBefore
                    && inventory.CountById("scrap_metal") == duplicateScrapBefore
                    && inventory.CountById("item_bedrock_sensor_rig") == duplicateRigBefore,
                    "rejected repeat monitoring-network construction rolls back its bill");

                var snapshots = session.CapturePersistedSnapshots();
                var json = new SystemTextJsonSerializer();
                Check(!string.IsNullOrWhiteSpace(snapshots.DeepWell)
                    && SchemaVersionedEnvelope<DeepWellState>.Decode(snapshots.DeepWell, json)?.built == true,
                    "well snapshot uses its existing save envelope");
                Check(!string.IsNullOrWhiteSpace(snapshots.Condenser)
                    && SchemaVersionedEnvelope<AtmosphericCondenserState>.Decode(snapshots.Condenser, json)?.built == true,
                    "condenser snapshot uses its existing save envelope");
                Check(!string.IsNullOrWhiteSpace(snapshots.Piezometer)
                    && SchemaVersionedEnvelope<HydrogeologyNetworkState>.Decode(snapshots.Piezometer, json)?.constructed == true,
                    "monitoring snapshot uses its existing save envelope");

                Check(!string.IsNullOrWhiteSpace(session.DeepWellState.systemId)
                    && !string.IsNullOrWhiteSpace(session.CondenserState.systemId)
                    && session.PiezometerState.nodes.Count > 0,
                    "aggregate session projects all three live Core states");

                if (failures == 0)
                    GD.Print("[WaterSourcesSelfTest] Water source commands and snapshots verified.");
            }
            catch (Exception ex)
            {
                failures++;
                GD.PrintErr($"[WaterSourcesSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
            }

            return EmitSummary(
                "water_sources_selftest",
                failures == 0,
                failures == 0 ? 0 : 1,
                passedCount: passed,
                failedCount: failures,
                details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }
    }
}
