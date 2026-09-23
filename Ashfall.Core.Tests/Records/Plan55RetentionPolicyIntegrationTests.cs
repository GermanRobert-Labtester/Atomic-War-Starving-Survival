// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Records;
using Xunit;

namespace Ashfall.Core.Tests.Records
{
    public sealed class Plan55RetentionPolicyIntegrationTests
    {
        [Fact]
        public void RollingLog_EnforcesCapacityAndMaintainsMonotonicTotal()
        {
            var log = new RollingLog<int>(capacity: 5);

            for (int i = 1; i <= 10; i++)
            {
                log.Append(i);
            }

            Assert.Equal(5, log.Count);
            Assert.Equal(10, log.TotalRecordedCount);
            Assert.Equal(5, log.PrunedCount);

            Assert.Equal(new[] { 6, 7, 8, 9, 10 }, log.CurrentEntries);
        }

        [Fact]
        public void RollingLog_LoadSnapshotTrimsExcessToCapacity()
        {
            var log = new RollingLog<string>(capacity: 3);

            var items = new List<string> { "item_1", "item_2", "item_3", "item_4", "item_5" };
            log.LoadSnapshot(items, totalRecorded: 12);

            Assert.Equal(3, log.Count);
            Assert.Equal(12, log.TotalRecordedCount);
            Assert.Equal(2, log.PrunedCount);
            Assert.Equal(new[] { "item_3", "item_4", "item_5" }, log.CurrentEntries);
        }

        [Fact]
        public void RetentionPolicyCatalog_AppliesCapacityLimitsToRegisteredCollections()
        {
            var catalog = new RetentionPolicyCatalog();

            var kitchenLog = new List<string>();
            for (int i = 1; i <= 250; i++)
            {
                kitchenLog.Add($"meal_{i}");
            }

            bool applied = catalog.ApplyRetention("kitchen_serving_log", kitchenLog, out int prunedCount);

            Assert.True(applied);
            Assert.Equal(50, prunedCount);
            Assert.Equal(200, kitchenLog.Count);
            Assert.Equal("meal_51", kitchenLog[0]);
            Assert.Equal("meal_250", kitchenLog[199]);
        }

        [Fact]
        public void RetentionPolicyCatalog_NeverPrunesProtectedObligations()
        {
            var catalog = new RetentionPolicyCatalog();

            var deadlines = new List<string>();
            for (int i = 1; i <= 500; i++)
            {
                deadlines.Add($"deadline_promise_{i}");
            }

            bool result = catalog.ApplyRetention("campaign_deadlines", deadlines, out int prunedCount);

            Assert.True(result);
            Assert.Equal(0, prunedCount);
            Assert.Equal(500, deadlines.Count);

            var memorials = new List<string>();
            for (int i = 1; i <= 300; i++)
            {
                memorials.Add($"fallen_survivor_{i}");
            }

            catalog.ApplyRetention("memorial_monuments", memorials, out int memorialPruned);
            Assert.Equal(0, memorialPruned);
            Assert.Equal(300, memorials.Count);
        }

        [Fact]
        public void RetentionPolicyCatalog_FiresSeamWhenPruningOccurs()
        {
            var catalog = new RetentionPolicyCatalog();

            string? prunedKey = null;
            int beforeCount = 0;
            int afterCount = 0;

            catalog.OnEntriesPrunedSeam = (key, before, after) =>
            {
                prunedKey = key;
                beforeCount = before;
                afterCount = after;
            };

            var decrees = new List<string>();
            for (int i = 1; i <= 150; i++)
            {
                decrees.Add($"decree_{i}");
            }

            catalog.ApplyRetention("faction_war_decrees", decrees, out _);

            Assert.Equal("faction_war_decrees", prunedKey);
            Assert.Equal(150, beforeCount);
            Assert.Equal(100, afterCount);
            Assert.Equal(100, decrees.Count);
        }
    }
}
