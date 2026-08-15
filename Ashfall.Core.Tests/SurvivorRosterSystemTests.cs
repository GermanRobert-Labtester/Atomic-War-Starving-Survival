using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SurvivorRosterSystemTests
    {
        private static SurvivorRosterSystem NewSystem()
        {
            var sys = new SurvivorRosterSystem();
            sys.RegisterDefinition(new SurvivorDefinition { id = "sv_a", displayName = "A", profession = "Farmer", baseHealth = 90f });
            sys.RegisterDefinition(new SurvivorDefinition { id = "sv_b", displayName = "B", profession = "Doctor", baseHealth = 100f });
            return sys;
        }

        [Fact]
        public void Register_NullAndDuplicateIgnored()
        {
            var sys = new SurvivorRosterSystem();
            sys.RegisterDefinition(null);
            sys.RegisterDefinition(new SurvivorDefinition());
            Assert.Empty(sys.Catalog);
            sys.RegisterDefinition(new SurvivorDefinition { id = "sv_a" });
            sys.RegisterDefinition(new SurvivorDefinition { id = "sv_a" });
            Assert.Single(sys.Catalog);
        }

        [Fact]
        public void Join_AddsEntryAndFiresEvent()
        {
            var sys = NewSystem();
            int joined = 0;
            sys.OnSurvivorJoined += e => joined++;
            Assert.True(sys.Join("sv_a", 10));
            Assert.Equal(1, joined);
            var entry = sys.Find("sv_a");
            Assert.NotNull(entry);
            Assert.Equal(10, entry.joinedDay);
            Assert.True(entry.isAlive);
            Assert.Equal(1, sys.LivingCount);
        }

        [Fact]
        public void Join_UnknownDefinitionOrDuplicateRejected()
        {
            var sys = NewSystem();
            Assert.False(sys.Join("sv_missing", 10));
            Assert.True(sys.Join("sv_a", 10));
            Assert.False(sys.Join("sv_a", 11));
            Assert.Single(sys.Roster);
        }

        [Fact]
        public void Die_MarksDeadWithReasonAndFiresEvent()
        {
            var sys = NewSystem();
            sys.Join("sv_a", 10);
            string reason = null;
            sys.OnSurvivorDied += (e, r) => reason = r;
            Assert.True(sys.Die("sv_a", "Died of thirst."));
            Assert.Equal("Died of thirst.", reason);
            Assert.False(sys.Find("sv_a").isAlive);
            Assert.Equal(0, sys.LivingCount);
            Assert.False(sys.Die("sv_a", "again")); // double death refused
        }

        [Fact]
        public void Die_UnknownSurvivorRejected()
        {
            var sys = NewSystem();
            Assert.False(sys.Die("sv_missing", "reason"));
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = NewSystem();
            sys.Join("sv_a", 10);
            var snapshot = sys.CaptureState();
            snapshot.entries[0].isAlive = false;
            snapshot.entries[0].deathReason = "injected";
            Assert.True(sys.Find("sv_a").isAlive);
            Assert.Equal(string.Empty, sys.Find("sv_a").deathReason);
        }

        [Fact]
        public void CaptureState_EmitsInOrdinalOrder()
        {
            var sys = NewSystem();
            sys.Join("sv_b", 5);
            sys.Join("sv_a", 6);
            var snapshot = sys.CaptureState();
            Assert.Equal("sv_a", snapshot.entries[0].survivorId);
            Assert.Equal("sv_b", snapshot.entries[1].survivorId);
        }

        [Fact]
        public void SaveLoad_RoundTripsAllState()
        {
            var sys = NewSystem();
            sys.Join("sv_a", 10);
            sys.Join("sv_b", 12);
            sys.Die("sv_a", "Radiation sickness.");

            var restored = new SurvivorRosterSystem();
            restored.RegisterDefinition(new SurvivorDefinition { id = "sv_a" });
            restored.RegisterDefinition(new SurvivorDefinition { id = "sv_b" });
            restored.RestoreState(sys.CaptureState());

            Assert.Equal(2, restored.Roster.Count);
            Assert.False(restored.Find("sv_a").isAlive);
            Assert.Equal("Radiation sickness.", restored.Find("sv_a").deathReason);
            Assert.Equal(1, restored.LivingCount);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = NewSystem();
            sys.Join("sv_a", 1);
            sys.Join("sv_b", 2);
            sys.Die("sv_b", "gone");
            string before = SaveChecksum.Compute(sys.CaptureState());

            var restored = new SurvivorRosterSystem();
            restored.RegisterDefinition(new SurvivorDefinition { id = "sv_a" });
            restored.RegisterDefinition(new SurvivorDefinition { id = "sv_b" });
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());

            Assert.Equal(before, after);
        }

        // ── Catalog data ───────────────────────────────────────────────

        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        [Fact]
        public void Catalog_LoadsAllSurvivorsWithBoundFields()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var defs = SurvivorCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(defs.Count >= 100, $"Expected >= 100 survivors, got {defs.Count}");
            foreach (var d in defs)
            {
                Assert.False(string.IsNullOrEmpty(d.displayName), d.id + " displayName unbound");
                Assert.False(string.IsNullOrEmpty(d.profession), d.id + " profession unbound");
                Assert.True(d.baseHealth > 0f, d.id + " baseHealth unbound");
            }
        }

        [Fact]
        public void Catalog_MissingDirectoryReturnsEmpty()
        {
            var defs = SurvivorCatalogLoader.Load(
                "/nonexistent", new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(defs);
        }

        [Fact]
        public void Catalog_UnityParityKnownSurvivors()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var defs = SurvivorCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Contains(defs, d => d.id == "elena_vasquez");
            Assert.Contains(defs, d => d.id == "marcus_olejnik");
            Assert.Contains(defs, d => d.id == "suki_tanaka");
        }
    }
}
