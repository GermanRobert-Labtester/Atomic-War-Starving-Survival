using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// SaveMap backs the id-keyed persistence for the fifteen CoreFamiliesWiringTests
    /// systems whose CaptureState/RestoreState just plumb straight through to
    /// SaveMap.Capture/SaveMap.Restore (tetanus, frostbite, cataracts, stranded
    /// vehicles, blocked routes, etc.). Testing the shared helper once here covers
    /// an actual bug in all fifteen call sites, rather than repeating a smoke test
    /// per system.
    /// </summary>
    [TestFixture]
    public class SaveMapTests
    {
        private class Entry
        {
            public string survivorId;
            public int severity;
        }

        [Test]
        public void Capture_FlattensMapValuesIntoList()
        {
            var map = new Dictionary<string, Entry>
            {
                ["sv_a"] = new Entry { survivorId = "sv_a", severity = 2 },
                ["sv_b"] = new Entry { survivorId = "sv_b", severity = 5 },
            };

            var list = SaveMap.Capture(map);

            Assert.AreEqual(2, list.Count);
            CollectionAssert.Contains(list, map["sv_a"]);
            CollectionAssert.Contains(list, map["sv_b"]);
        }

        [Test]
        public void Restore_RepopulatesMapKeyedByIdSelector()
        {
            var saved = new List<Entry>
            {
                new Entry { survivorId = "sv_a", severity = 2 },
                new Entry { survivorId = "sv_b", severity = 5 },
            };
            var map = new Dictionary<string, Entry>();

            SaveMap.Restore(map, saved, e => e.survivorId);

            Assert.AreEqual(2, map.Count);
            Assert.AreEqual(2, map["sv_a"].severity);
            Assert.AreEqual(5, map["sv_b"].severity);
        }

        [Test]
        public void Restore_ClearsExistingEntriesNotInSave()
        {
            var map = new Dictionary<string, Entry>
            {
                ["stale"] = new Entry { survivorId = "stale", severity = 9 },
            };

            SaveMap.Restore(map, new List<Entry> { new Entry { survivorId = "sv_a", severity = 1 } }, e => e.survivorId);

            Assert.IsFalse(map.ContainsKey("stale"));
            Assert.IsTrue(map.ContainsKey("sv_a"));
        }

        [Test]
        public void Restore_NullSaved_ClearsMap()
        {
            var map = new Dictionary<string, Entry> { ["sv_a"] = new Entry { survivorId = "sv_a", severity = 1 } };

            SaveMap.Restore(map, null, e => e.survivorId);

            Assert.AreEqual(0, map.Count);
        }

        [Test]
        public void CaptureThenRestore_RoundTripsFieldValues()
        {
            var source = new Dictionary<string, Entry>
            {
                ["sv_a"] = new Entry { survivorId = "sv_a", severity = 7 },
            };
            var saved = SaveMap.Capture(source);

            var target = new Dictionary<string, Entry>();
            SaveMap.Restore(target, saved, e => e.survivorId);

            Assert.AreEqual(1, target.Count);
            Assert.AreEqual(7, target["sv_a"].severity);
        }
    }
}
