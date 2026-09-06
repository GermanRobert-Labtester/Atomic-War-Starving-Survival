using System;
using System.Collections.Generic;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Factions
{
    public sealed class ShelterEspionageSystemTests
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

        private static FactionIntelligenceCatalog CreateSampleCatalog()
        {
            return new FactionIntelligenceCatalog
            {
                schema_version = 1,
                operations = new List<FactionOperationDefinition>
                {
                    new FactionOperationDefinition
                    {
                        id = "fop_test_recon",
                        display_name = "Test Reconnaissance",
                        target_faction = "warlords",
                        operation_class = "recon",
                        target_subsystem = "storage"
                    }
                },
                dead_drop_templates = new List<DeadDropTemplateDefinition>
                {
                    new DeadDropTemplateDefinition
                    {
                        id = "fdrop_test_culvert",
                        display_name = "Culvert Drop",
                        target_location = "loc_recovery_yard",
                        reward_intel_points = 25,
                        expiry_days = 4
                    }
                }
            };
        }

        [Fact]
        public void EnrollSleeperAgent_TracksOperativeAndSuspicion()
        {
            var system = new ShelterEspionageSystem(CreateSampleCatalog(), new SeededRng(100));

            bool enrolled = system.EnrollSleeperAgent("survivor_01", "iron_garrison", 700);
            Assert.True(enrolled);
            Assert.True(system.IsSleeperAgent("survivor_01"));

            var rec = system.GetSleeperRecord("survivor_01");
            Assert.NotNull(rec);
            Assert.Equal(0, rec.suspicionPermille);

            system.AddSuspicion("survivor_01", 300);
            Assert.Equal(300, rec.suspicionPermille);
        }

        [Fact]
        public void InvestigateSuspect_UnmasksOperativeWithHighSuspicion()
        {
            var system = new ShelterEspionageSystem(CreateSampleCatalog(), new SeededRng(100));
            system.EnrollSleeperAgent("survivor_02", "cult_of_ash", 800);
            system.AddSuspicion("survivor_02", 900);

            bool ok = system.InvestigateSuspect("survivor_02", investigatorSkill: 50, out bool unmasked, out string report);
            Assert.True(ok);
            Assert.True(unmasked);
            Assert.Contains("verified as active operative", report);
        }

        [Fact]
        public void TurnDoubleAgent_ConsumesBribeAndConvertsSleeper()
        {
            var system = new ShelterEspionageSystem(CreateSampleCatalog(), new SeededRng(100));
            system.EnrollSleeperAgent("survivor_03", "warlords", 600);
            var rec = system.GetSleeperRecord("survivor_03")!;
            rec.isIdentified = true;

            var inv = new TestInventory();
            inv.Set("scrap_metal", 10);

            bool turned = system.AttemptTurnDoubleAgent("survivor_03", inv, out string outcome);
            Assert.True(turned, outcome);
            Assert.True(rec.isTurnedDoubleAgent);
            Assert.Equal(5, inv.CountById("scrap_metal"));
            Assert.True(system.CaptureState().totalIntelPoints >= 25);
        }

        [Fact]
        public void DeadDropLifecycle_CanSpawnAndIntercept()
        {
            var system = new ShelterEspionageSystem(CreateSampleCatalog(), new SeededRng(100));
            var drop = system.SpawnRandomDeadDrop();
            Assert.NotNull(drop);
            Assert.Equal("loc_recovery_yard", drop.targetLocationId);

            bool intercepted = system.InterceptDeadDrop(drop.dropId, out int intelPoints, out string rep);
            Assert.True(intercepted, rep);
            Assert.Equal(25, intelPoints);
            Assert.Equal(25, system.CaptureState().totalIntelPoints);
        }

        [Fact]
        public void TickDay_AdvancesDropExpiryAndAccumulatesSleeperActivity()
        {
            var system = new ShelterEspionageSystem(CreateSampleCatalog(), new SeededRng(100));
            system.SetSecurityCounterIntelScore(300);
            system.EnrollSleeperAgent("survivor_04", "iron_garrison", 800);
            var drop = system.SpawnRandomDeadDrop()!;
            int initialDays = drop.remainingDays;

            var inv = new TestInventory();
            inv.Set("scrap_metal", 20);

            system.TickDay(1, inv);

            var rec = system.GetSleeperRecord("survivor_04")!;
            Assert.Equal(1, rec.leaksCommittedCount);
            Assert.True(rec.suspicionPermille > 0);
            Assert.Equal(initialDays - 1, drop.remainingDays);
        }

        [Fact]
        public void StateRoundtrip_PreservesSleeperAndIntel()
        {
            var system = new ShelterEspionageSystem(CreateSampleCatalog(), new SeededRng(100));
            system.EnrollSleeperAgent("survivor_05", "rebel_cells", 750);
            var rec = system.GetSleeperRecord("survivor_05")!;
            rec.isIdentified = true;
            rec.suspicionPermille = 450;
            var drop = system.SpawnRandomDeadDrop()!;

            var state = system.CaptureState();

            var restored = new ShelterEspionageSystem(CreateSampleCatalog(), new SeededRng(100));
            restored.RestoreState(state);

            Assert.True(restored.IsSleeperAgent("survivor_05"));
            var restoredRec = restored.GetSleeperRecord("survivor_05")!;
            Assert.True(restoredRec.isIdentified);
            Assert.Equal(450, restoredRec.suspicionPermille);
            Assert.Single(restored.CaptureState().activeDeadDrops);
        }
    }
}
