using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Events;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — diegetic radio corpus engine tests.
    /// Verifies the 13 authored broadcasts load, fire once each, gate on the
    /// Culpable+ window, publish to the shared bus, and round-trip fired-ids.
    /// </summary>
    public class VerdictRadioSystemTests
    {
        private static List<VerdictCatalogLoader.VerdictRadioEntry> SampleCorpus()
        {
            var list = new List<VerdictCatalogLoader.VerdictRadioEntry>();
            void add(string id, int day, string kind)
            {
                list.Add(new VerdictCatalogLoader.VerdictRadioEntry
                {
                    id = id,
                    frequency = "99.0 MHz",
                    dayTrigger = day,
                    source = "Census Carrier",
                    message = id,
                    kind = kind
                });
            }
            add("radio_verdict_pilot", 210, "carrier");
            add("radio_verdict_reckoning", 241, "call");
            add("radio_verdict_late", 300, "telemetry");
            return list;
        }

        private static (SimpleEventBus bus, VerdictRadioSystem sys) Build()
        {
            var bus = new SimpleEventBus();
            var sys = new VerdictRadioSystem(bus, null, SampleCorpus());
            return (bus, sys);
        }

        [Fact]
        public void Poll_GatesOnCulpableWindow_NothingBefore()
        {
            var (bus, sys) = Build();
            // Dormant / Knowing: nothing fires even past dayTrigger.
            Assert.Empty(sys.Poll(400, ReckoningPhase.Dormant));
            Assert.Empty(sys.Poll(400, ReckoningPhase.Knowing));
        }

        [Fact]
        public void Poll_FiresCorpusOnceInsideWindow()
        {
            var (bus, sys) = Build();
            // Culpable at day 211: the pilot (trigger 210) fires; not the call (241).
            var fired = sys.Poll(211, ReckoningPhase.Culpable);
            Assert.Contains("radio_verdict_pilot", fired);
            Assert.DoesNotContain("radio_verdict_reckoning", fired);

            // Re-poll same day: nothing new (idempotent).
            Assert.Empty(sys.Poll(211, ReckoningPhase.Culpable));
        }

        [Fact]
        public void Poll_FiresAllAtDeadline()
        {
            var (bus, sys) = Build();
            var fired = sys.Poll(301, ReckoningPhase.Counted);
            Assert.Contains("radio_verdict_pilot", fired);
            Assert.Contains("radio_verdict_reckoning", fired);
            Assert.Contains("radio_verdict_late", fired);
        }

        [Fact]
        public void FiredBroadcastsPublishToBus()
        {
            var (bus, sys) = Build();
            sys.Poll(301, ReckoningPhase.Counted);
            int published = 0;
            foreach (var e in bus.PublishedEvents)
                if (e.name == "radio.verdict.broadcast") published++;
            Assert.Equal(3, published);
        }

        [Fact]
        public void SaveLoad_RoundTripsFiredIds_NoReplay()
        {
            var (bus, sys) = Build();
            sys.Poll(241, ReckoningPhase.Counted); // pilot + call fired; late (300) not yet

            var restored = new VerdictRadioSystem(new SimpleEventBus(), null, SampleCorpus());
            restored.RestoreState(sys.CaptureState());

            Assert.True(restored.HasFired("radio_verdict_pilot"));
            Assert.True(restored.HasFired("radio_verdict_reckoning"));
            Assert.False(restored.HasFired("radio_verdict_late"));

            // Re-poll at day 301: only the late one fires (no replay).
            var fired = restored.Poll(301, ReckoningPhase.Counted);
            Assert.Contains("radio_verdict_late", fired);
            Assert.DoesNotContain("radio_verdict_pilot", fired);
            Assert.DoesNotContain("radio_verdict_reckoning", fired);
        }

        [Fact]
        public void LoadFrom_LoadsThirteenAuthoredBroadcasts()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;
            var sys = new VerdictRadioSystem();
            int n = sys.LoadFrom(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(13, n);
            Assert.Equal(13, sys.Corpus.Count);
        }

        [Fact]
        public void EvidenceEnrollment_FieldsPresentInItems()
        {
            // The authored verdict_items.json induces enrollment via
            // mechanical_effects.enrolled_evidence when present.
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;
            var items = VerdictCatalogLoader.LoadItems(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            int withEffect = 0;
            foreach (var it in items)
                if (it.mechanical_effects != null && it.mechanical_effects.enrolled_evidence > 0) withEffect++;
            // The authored file marks the 12 evidence_* rows as enrolling.
            Assert.Equal(12, withEffect);
        }

        private static string FindDataDir()
        {
            string start = System.IO.Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
            return string.Empty;
        }
    }
}
