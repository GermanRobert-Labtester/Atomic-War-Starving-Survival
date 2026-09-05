// SPDX-License-Identifier: MIT
// ASHFALL Patrol Cost and Requirement Tests (PAT-F2 & PAT-F3)

using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core.IO;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Tests
{
    public class PatrolCostAndRequirementTests
    {
        private readonly string _dataDir;
        private readonly TravelEncounterCatalog _catalog;

        public PatrolCostAndRequirementTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, new FileSystemIO());
        }

        [Fact]
        public void AtomicPayment_SufficientItems_DeductsExactCosts()
        {
            var inv = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inv.TryProduce("canned_food", 10);
            var sys = new TravelEncounterSystem(_catalog, inv);

            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var res);
            Assert.True(ok);
            Assert.NotNull(res);
            // 2 canned_food deducted
            Assert.Equal(8, inv.CountById("canned_food"));
        }

        [Fact]
        public void AtomicPayment_InsufficientItems_FailsAndDeductsZero()
        {
            var inv = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inv.TryProduce("canned_food", 1); // needs 2
            var sys = new TravelEncounterSystem(_catalog, inv);

            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var res);
            Assert.False(ok);
            Assert.Null(res);
            // 0 items deducted; rollback intact
            Assert.Equal(1, inv.CountById("canned_food"));
        }

        [Fact]
        public void RequiredItem_NonConsuming_PreservesItemAfterResolution()
        {
            var inv = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inv.TryProduce("sealed_government_document", 1);
            var sys = new TravelEncounterSystem(_catalog, inv);

            // Choice requires sealed_government_document x1
            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_show_garrison_pass", 1, out var res);
            Assert.True(ok);
            Assert.NotNull(res);
            // Non-consuming requirement: item is STILL in inventory
            Assert.Equal(1, inv.CountById("sealed_government_document"));
        }

        [Fact]
        public void RequiredItem_MissingPrerequisite_RejectsChoiceWithoutSideEffects()
        {
            var inv = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            var sys = new TravelEncounterSystem(_catalog, inv);

            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_show_garrison_pass", 1, out var res);
            Assert.False(ok);
            Assert.Null(res);
        }

        [Fact]
        public void EvaluateChoiceAvailability_ReportsAccurateDeficits()
        {
            var inv = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inv.TryProduce("canned_food", 1);
            var sys = new TravelEncounterSystem(_catalog, inv);

            Assert.True(sys.TryBuildResolutionPlan("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var plan));
            Assert.NotNull(plan);
            Assert.False(plan!.Availability.IsAvailable);
            Assert.Contains(plan.Availability.Failures, f => f.ItemId == "canned_food");
        }
    }
}
