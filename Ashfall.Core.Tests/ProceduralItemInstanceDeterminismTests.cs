using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ProceduralItemInstanceDeterminismTests
    {
        [Fact]
        public void SameSeed_GeneratesIdenticalFirstNIds()
        {
            const int count = 50;
            const int seed = 987654;

            ProceduralItemInstance.ConfigureSequence(seed, 0);
            var runA = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                var item = new ProceduralItemInstance("item_medical_gauze");
                runA.Add(item.InstanceId);
            }

            ProceduralItemInstance.ConfigureSequence(seed, 0);
            var runB = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                var item = new ProceduralItemInstance("item_medical_gauze");
                runB.Add(item.InstanceId);
            }

            Assert.Equal(runA.Count, runB.Count);
            for (int i = 0; i < count; i++)
            {
                Assert.Equal(runA[i], runB[i]);
            }
        }

        [Fact]
        public void DifferentSeed_GeneratesDifferentSequence()
        {
            const int count = 20;

            ProceduralItemInstance.ConfigureSequence(11111, 0);
            var listA = new List<string>();
            for (int i = 0; i < count; i++)
                listA.Add(new ProceduralItemInstance("item_geiger_counter").InstanceId);

            ProceduralItemInstance.ConfigureSequence(22222, 0);
            var listB = new List<string>();
            for (int i = 0; i < count; i++)
                listB.Add(new ProceduralItemInstance("item_geiger_counter").InstanceId);

            bool anyDifferent = false;
            for (int i = 0; i < count; i++)
            {
                if (listA[i] != listB[i])
                {
                    anyDifferent = true;
                    break;
                }
            }
            Assert.True(anyDifferent, "Different seeds must produce different sequences of instance IDs.");
        }

        [Fact]
        public void LargeBatch_GeneratesAllUniqueIds()
        {
            const int count = 5000;
            ProceduralItemInstance.ConfigureSequence(54321, 0);

            var seen = new HashSet<string>();
            for (int i = 0; i < count; i++)
            {
                var item = new ProceduralItemInstance("item_clean_water");
                bool added = seen.Add(item.InstanceId);
                Assert.True(added, $"Collision detected at index {i} for ID {item.InstanceId}");
            }
            Assert.Equal(count, seen.Count);
        }

        [Fact]
        public void SaveRestore_ResumesSequenceCounter()
        {
            const int seed = 777;

            // Uninterrupted run of 30 items
            ProceduralItemInstance.ConfigureSequence(seed, 0);
            var uninterrupted = new List<string>();
            for (int i = 0; i < 30; i++)
                uninterrupted.Add(new ProceduralItemInstance("item_scrap_metal").InstanceId);

            // Resumed run: generate 15, capture state, configure, generate remaining 15
            ProceduralItemInstance.ConfigureSequence(seed, 0);
            var resumed = new List<string>();
            for (int i = 0; i < 15; i++)
                resumed.Add(new ProceduralItemInstance("item_scrap_metal").InstanceId);

            var (savedSeed, savedCounter) = ProceduralItemInstance.GetSequenceState();
            Assert.Equal(seed, savedSeed);
            Assert.Equal(15, savedCounter);

            // Restore state into sequence
            ProceduralItemInstance.ConfigureSequence(savedSeed, savedCounter);
            for (int i = 0; i < 15; i++)
                resumed.Add(new ProceduralItemInstance("item_scrap_metal").InstanceId);

            Assert.Equal(uninterrupted.Count, resumed.Count);
            for (int i = 0; i < 30; i++)
            {
                Assert.Equal(uninterrupted[i], resumed[i]);
            }
        }

        [Fact]
        public void OldSaveDefault_InitializesCleanlyAtZero()
        {
            ProceduralItemInstance.ConfigureSequence(0, 0);
            var item = new ProceduralItemInstance("item_ration_pack");

            Assert.NotNull(item.InstanceId);
            Assert.Equal(8, item.InstanceId.Length);
            var (seed, counter) = ProceduralItemInstance.GetSequenceState();
            Assert.Equal(0, seed);
            Assert.Equal(1, counter);
        }

        [Fact]
        public void HexFormat_IsStrict8CharLowerHex()
        {
            ProceduralItemInstance.ConfigureSequence(42, 0);
            var regex = new Regex("^[0-9a-f]{8}$");

            for (int i = 0; i < 100; i++)
            {
                var item = new ProceduralItemInstance($"item_{i}");
                Assert.Matches(regex, item.InstanceId);
            }
        }
    }
}
