using System;
using System.Collections.Generic;
using Ashfall.Core.Events;
using Ashfall.Core.Clock;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — the diegetic radio corpus engine.
    /// Loads verdict_radio.json (the 12-signal §6.2 corpus + the Reckoning Call)
    /// and schedules each broadcast through the 99.0 MHz census/carrier contract.
    /// A broadcast fires once when (a) its dayTrigger has passed AND (b) the
    /// Reckoning has reached at least CULPABLE (the census carrier window). Each
    /// fire publishes a `radio.verdict.broadcast` event on the shared bus.
    /// Engine-agnostic, deterministic, save/load safe — never per-frame.
    /// </summary>
    public sealed class VerdictRadioSystem
    {
        public const string SystemId = "verdict_radio_system";

        /// <summary>First day the census carrier is audible (CANON §5.1: Day 210±3).</summary>
        public const int CarrierOpenDay = 210;

        private readonly List<VerdictCatalogLoader.VerdictRadioEntry> _corpus = new List<VerdictCatalogLoader.VerdictRadioEntry>();
        private readonly HashSet<string> _firedIds = new HashSet<string>(StringComparer.Ordinal);
        private readonly IEventBus _bus;
        private readonly ISimClock _clock;

        public VerdictRadioSystem(
            IEventBus bus = null,
            ISimClock clock = null,
            IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> corpus = null)
        {
            _bus = bus;
            _clock = clock;
            if (corpus != null)
                foreach (var e in corpus)
                    if (e != null && !string.IsNullOrEmpty(e.id))
                        _corpus.Add(e);
        }

        public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> Corpus => _corpus;
        public bool HasFired(string id) => _firedIds.Contains(id);
        public int FiredCount => _firedIds.Count;

        /// <summary>
        /// Evaluate the corpus against the current day + phase. A broadcast fires
        /// at most once: dayTrigger passed AND phase >= Culpable. Returns the ids
        /// fired this call (for tests/observability). Publish is via the shared bus.
        /// </summary>
        public System.Collections.Generic.List<string> Poll(int day, ReckoningPhase phase)
        {
            var fired = new System.Collections.Generic.List<string>();
            if (phase < ReckoningPhase.Culpable) return fired;
            if (day < CarrierOpenDay) return fired;

            for (int i = 0; i < _corpus.Count; i++)
            {
                var e = _corpus[i];
                if (e == null || _firedIds.Contains(e.id)) continue;
                if (day < e.dayTrigger) continue;
                _firedIds.Add(e.id);
                if (_bus != null)
                    _bus.Publish("radio.verdict.broadcast", e);
                fired.Add(e.id);
            }
            return fired;
        }

        /// <summary>Load the corpus from disk into this system (host convenience).</summary>
        public int LoadFrom(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return 0;
            var loaded = VerdictCatalogLoader.LoadRadio(dataDir, fileIO, json);
            int added = 0;
            foreach (var e in loaded)
            {
                if (e == null || string.IsNullOrEmpty(e.id)) continue;
                if (_corpus.Exists(x => x.id == e.id)) continue;
                _corpus.Add(e);
                added++;
            }
            return added;
        }

        // ── Save / Load (fired-ids state so reloads don't replay) ────────────

        [Serializable]
        public class VerdictRadioState
        {
            public string systemId = SystemId;
            public List<string> firedIds = new List<string>();
        }

        public VerdictRadioState CaptureState()
        {
            var c = new VerdictRadioState { systemId = SystemId };
            var sorted = new List<string>(_firedIds);
            sorted.Sort(StringComparer.Ordinal);
            c.firedIds = sorted;
            return c;
        }

        public void RestoreState(VerdictRadioState state)
        {
            if (state == null) return;
            _firedIds.Clear();
            if (state.firedIds != null)
                foreach (var id in state.firedIds)
                    if (!string.IsNullOrEmpty(id)) _firedIds.Add(id);
        }
    }
}
