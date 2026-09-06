using System;
using System.Collections.Generic;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class VehicleGarageSystemTests
    {
        private sealed class TestInventory : IPlayerInventoryPort
        {
            private readonly Dictionary<string, int> _items = new Dictionary<string, int>(StringComparer.Ordinal);

            public void Set(string itemId, int count) => _items[itemId] = count;

            public int CountById(string itemId) => _items.TryGetValue(itemId, out var c) ? c : 0;

            public bool HasSufficient(string itemId, int count) => CountById(itemId) >= count;

            public bool TryConsume(string itemId, int count, Action? onCommitted = null)
            {
                if (!HasSufficient(itemId, count)) return false;
                _items[itemId] -= count;
                onCommitted?.Invoke();
                return true;
            }

            public bool TryConsumeBill(IReadOnlyDictionary<string, int> bill, Action? onCommitted = null)
            {
                foreach (var kvp in bill)
                {
                    if (!HasSufficient(kvp.Key, kvp.Value)) return false;
                }
                foreach (var kvp in bill)
                {
                    _items[kvp.Key] -= kvp.Value;
                }
                onCommitted?.Invoke();
                return true;
            }

            public bool TryProduce(string itemId, int count, ItemDefinition? def = null)
            {
                _items[itemId] = CountById(itemId) + count;
                return true;
            }

            public InventoryTransactionQuote QuoteTransaction(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) => throw new NotImplementedException();
            public InventoryTransaction BeginTransaction(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) => throw new NotImplementedException();
            public bool TryExecuteTransaction(InventoryBill bill, Action? onCommitted = null, Func<string, ItemDefinition?>? lookup = null) => throw new NotImplementedException();
            public IReadOnlyList<InventorySlot> GetSlots() => throw new NotImplementedException();
        }

        private static VehicleGarageCatalog CreateSampleCatalog()
        {
            return new VehicleGarageCatalog
            {
                schema_version = 1,
                modifications = new List<VehicleModificationDefinition>
                {
                    new VehicleModificationDefinition
                    {
                        id = "vmod_test_cargo",
                        slot_type = "cargo",
                        compatible_vehicle_tags = new List<string> { "truck" },
                        install_cost = new List<VehicleModificationCost>
                        {
                            new VehicleModificationCost { item_id = "scrap_metal", amount = 10 }
                        },
                        effects = new VehicleModificationEffects
                        {
                            cargo_capacity_delta = 50f,
                            fuel_consumption_multiplier = 1.1f,
                            wear_rate_multiplier = 1.05f
                        }
                    },
                    new VehicleModificationDefinition
                    {
                        id = "vmod_test_armor",
                        slot_type = "protection",
                        compatible_vehicle_tags = new List<string> { "truck" },
                        install_cost = new List<VehicleModificationCost>
                        {
                            new VehicleModificationCost { item_id = "scrap_metal", amount = 15 }
                        },
                        effects = new VehicleModificationEffects
                        {
                            radiation_protection_permille = 300,
                            speed_multiplier_delta = -0.05f,
                            wear_rate_multiplier = 0.9f
                        }
                    }
                }
            };
        }

        [Fact]
        public void InstallModification_DeductsInventoryAndAppliesEffects()
        {
            var catalog = CreateSampleCatalog();
            var system = new VehicleGarageSystem(catalog);
            var inv = new TestInventory();
            inv.Set("scrap_metal", 20);

            bool success = system.InstallModification("truck_alpha", "cargo", "vmod_test_cargo", inv, out string reason);
            Assert.True(success, reason);
            Assert.Equal(10, inv.CountById("scrap_metal"));

            Assert.Equal(50f, system.GetEffectiveCargoCapacityDelta("truck_alpha"));
            Assert.Equal(1.1f, system.GetEffectiveFuelConsumptionMultiplier("truck_alpha"), 3);
            Assert.Equal(1.05f, system.GetEffectiveWearRateMultiplier("truck_alpha"), 3);
        }

        [Fact]
        public void InstallModification_FailsOnMismatchedSlotOrInsufficientMaterials()
        {
            var catalog = CreateSampleCatalog();
            var system = new VehicleGarageSystem(catalog);
            var inv = new TestInventory();
            inv.Set("scrap_metal", 5); // Needs 10

            bool failMaterials = system.InstallModification("truck_alpha", "cargo", "vmod_test_cargo", inv, out string reason1);
            Assert.False(failMaterials);
            Assert.Contains("Insufficient material", reason1);

            inv.Set("scrap_metal", 50);
            bool failSlot = system.InstallModification("truck_alpha", "engine", "vmod_test_cargo", inv, out string reason2);
            Assert.False(failSlot);
            Assert.Contains("cannot be installed", reason2);
        }

        [Fact]
        public void TripWear_AccumulatesAndCanImmobilize()
        {
            var system = new VehicleGarageSystem();
            system.RecordTripWear("scout_one", 100f, 1.0f);

            var record = system.GetOrCreateRecord("scout_one");
            Assert.True(record.chassisStressPermille > 0);
            Assert.True(record.engineFoulingPermille > 0);
            Assert.True(record.transmissionWearPermille > 0);
            Assert.False(record.isImmobilized);

            // Extreme overland trip causes critical failure
            system.RecordTripWear("scout_one", 600f, 2.0f);
            Assert.True(record.isImmobilized);
        }

        [Fact]
        public void ServiceComponents_ConsumesMaterialsAndRestoresHealth()
        {
            var system = new VehicleGarageSystem();
            var record = system.GetOrCreateRecord("truck_beta");
            record.chassisStressPermille = 400;
            record.engineFoulingPermille = 300;
            record.transmissionWearPermille = 200;

            var inv = new TestInventory();
            inv.Set("scrap_metal", 50);
            inv.Set("mechanical_parts", 50);

            bool okChassis = system.ServiceChassis("truck_beta", inv, 200, out string rChassis);
            Assert.True(okChassis, rChassis);
            Assert.Equal(200, record.chassisStressPermille);

            bool okEngine = system.ServiceEngine("truck_beta", inv, 150, out string rEngine);
            Assert.True(okEngine, rEngine);
            Assert.Equal(150, record.engineFoulingPermille);

            bool okTrans = system.ServiceTransmission("truck_beta", inv, 200, out string rTrans);
            Assert.True(okTrans, rTrans);
            Assert.Equal(0, record.transmissionWearPermille);
        }

        [Fact]
        public void RecoveryMission_LifecycleCompletesAndClearsImmobilization()
        {
            var system = new VehicleGarageSystem();
            var record = system.GetOrCreateRecord("crawler_gamma");
            record.isImmobilized = true;
            record.chassisStressPermille = 1000;

            bool registered = system.RegisterRecoveryMission("crawler_gamma", "loc_recovery_yard", 15, out string missionId, out string reason);
            Assert.True(registered, reason);
            Assert.NotEmpty(missionId);

            // Advance ticks
            system.AdvanceRecoveryMission(missionId, 60, out bool completed1);
            Assert.False(completed1);

            system.AdvanceRecoveryMission(missionId, 70, out bool completed2);
            Assert.True(completed2);

            bool finish = system.CompleteRecoveryMission(missionId, null, out string completeReason);
            Assert.True(finish, completeReason);
            Assert.False(record.isImmobilized);
            Assert.True(record.chassisStressPermille <= 800);
        }

        [Fact]
        public void StateRoundtrip_PreservesModificationsAndWear()
        {
            var catalog = CreateSampleCatalog();
            var system = new VehicleGarageSystem(catalog);
            var inv = new TestInventory();
            inv.Set("scrap_metal", 100);

            system.InstallModification("rig_delta", "cargo", "vmod_test_cargo", inv, out _);
            system.InstallModification("rig_delta", "protection", "vmod_test_armor", inv, out _);
            var rec = system.GetOrCreateRecord("rig_delta");
            rec.chassisStressPermille = 350;

            var state = system.CaptureState();

            var restoredSystem = new VehicleGarageSystem(catalog);
            restoredSystem.RestoreState(state);

            Assert.Equal(50f, restoredSystem.GetEffectiveCargoCapacityDelta("rig_delta"));
            Assert.Equal(300, restoredSystem.GetEffectiveRadiationProtectionPermille("rig_delta"));
            Assert.Equal(350, restoredSystem.GetOrCreateRecord("rig_delta").chassisStressPermille);
        }
    }
}
