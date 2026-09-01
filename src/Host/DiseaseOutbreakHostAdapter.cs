using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core;
using Ashfall.Core.Disease;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 09 / 9A follow-up host wire. Bridges two host-resident
    /// Core systems (SumpFloodingSystem, ExcavationSystem) into the new
    /// <see cref="DiseaseSystem.TriggerOutbreak(IDiseaseOutbreakSource, string, int, IReadOnlyList{string})"/>
    /// extension point from Plan 09 9A Core (commit 53bb8259).
    ///
    /// Two <see cref="IDiseaseOutbreakSource"/> instances live in this
    /// file so the bridge has a single audit surface: if the project
    /// later adds an "outbreak triggered" replay log, this is the
    /// only file to touch.
    ///
    /// Engine-agnostic Core authority governs all decisions — the
    /// adapter is intentionally thin and per-ship audit-friendly.
    /// </summary>
    public sealed class DiseaseOutbreakHostAdapter
    {
        /// <summary>
        /// Sump flood contamination source. Contracted to seed
        /// <c>disease_silt_jaundice</c> per the Plan 09 9A data
        /// responsibility — a flood receded, the lower wells return
        /// to the drinking supply, the silt-borne spirochete rides
        /// in on the next well-water haul.
        /// </summary>
        public sealed class SumpFloodingSource : IDiseaseOutbreakSource
        {
            public string SourceId => "sump_flooding";
            public IReadOnlyList<string> AuthoredDiseaseIds { get; }
                = new[] { "disease_silt_jaundice" };
        }

        /// <summary>
        /// Deep dig completion source. Contracted to seed
        /// <c>disease_deep_excavation_mold_lung</c> per the Plan 09 9A
        /// data responsibility — a deep dig opens the stratosphere
        /// lane and the seated mold blooms into nearby workers'
        /// air passages within days.
        /// </summary>
        public sealed class ExcavationSource : IDiseaseOutbreakSource
        {
            public string SourceId => "excavation";
            public IReadOnlyList<string> AuthoredDiseaseIds { get; }
                = new[] { "disease_deep_excavation_mold_lung" };
        }

        // // ── Wiring state ───────────────────────────────────────────────

        private readonly DiseaseSystem _disease;
        private readonly Func<int> _simDayProvider;
        private readonly Func<IReadOnlyList<string>> _survivorPoolProvider;
        private readonly SumpFloodingSource _sumpSource = new SumpFloodingSource();
        private readonly ExcavationSource _excavationSource = new ExcavationSource();

        // Tracks which excavation sites have already fired so we
        // don't re-trigger on every coarse OnExcavationChanged.
        // Host calls FlushExcavationTick(state) per tick; the adapter
        // computes deltas against this set.
        private readonly HashSet<string> _previouslyCompleteSites =
            new HashSet<string>(StringComparer.Ordinal);

        private SumpFloodingSystem? _subscribedSump;
        private ExcavationSystem? _subscribedExcavation;
        private Action<FloodIncident>? _sumpHandler;

        /// <summary>
        /// Number of triggers actually applied since adapter creation.
        /// Selftest + a future debug overlay read this field.
        /// </summary>
        public int SumpTriggersApplied { get; private set; }
        public int DigTriggersApplied { get; private set; }

        public bool IsSubscribed { get; private set; }

        public DiseaseOutbreakHostAdapter(
            DiseaseSystem disease,
            Func<int> simDayProvider,
            Func<IReadOnlyList<string>> survivorPoolProvider)
        {
            _disease = disease ?? throw new ArgumentNullException(nameof(disease));
            _simDayProvider = simDayProvider
                ?? throw new ArgumentNullException(nameof(simDayProvider));
            _survivorPoolProvider = survivorPoolProvider
                ?? throw new ArgumentNullException(nameof(survivorPoolProvider));
        }

        /// <summary>
        /// Subscribe to upstream events. Idempotent. The caller passes
        /// the concrete <see cref="SumpFloodingSystem"/> +
        /// <see cref="ExcavationSystem"/>; we keep weak refs so
        /// <see cref="Unwire"/> doesn't need the caller to re-pass
        /// them.
        /// </summary>
        public void Wire(SumpFloodingSystem? sump, ExcavationSystem? excavation)
        {
            if (IsSubscribed) return;
            _subscribedSump = sump;
            _subscribedExcavation = excavation;
            _sumpHandler = OnSumpIncident;
            if (sump != null) sump.OnIncident += _sumpHandler;
            // ExcavationSystem.OnExcavationChanged is a coarse signal;
            // the host explicitly calls FlushExcavationTick(state) —
            // we don't auto-subscribe to avoid noise on shoring /
            // worker-assignment events.
            IsSubscribed = true;
        }

        /// <summary>
        /// Detach the sump subscription. Used by disposal paths.
        /// Idempotent. Excavation subscriptions follow FlushExcavationTick
        /// (each call reads the current state snapshot, no unsubscribe
        /// needed for the coarse event since we don't subscribe to it).
        /// </summary>
        public void Unwire()
        {
            if (!IsSubscribed) return;
            if (_subscribedSump != null && _sumpHandler != null)
                _subscribedSump.OnIncident -= _sumpHandler;
            _subscribedSump = null;
            _sumpHandler = null;
            // ExcavationSystem subscription is host-driven, no cleanup
            // needed — host stops calling FlushExcavationTick on
            // disposal.
            _subscribedExcavation = null;
            IsSubscribed = false;
        }

        // // ── Sump handler (live event) ────────────────────────────────

        private void OnSumpIncident(FloodIncident incident)
        {
            // Only the "contamination" kind surfaces the trigger.
            // DrainComplete (a pump running again) is too cheap to
            // be worth a contract check.
            if (incident?.kind != FloodIncidentKind.Contamination) return;
            int day = _simDayProvider();
            var candidates = _survivorPoolProvider() ?? Array.Empty<string>();
            var result = _disease.TriggerOutbreak(
                _sumpSource, "disease_silt_jaundice", day, candidates);
            if (result.InfectionsApplied > 0) SumpTriggersApplied++;
        }

        // // ── Excavation flush (state-driven, host instrumented) ──────

        /// <summary>
        /// Called by the host once per TickDay for the excavation
        /// system. Compares the current sites against the previously
        /// complete set; for each newly-completed site the adapter
        /// calls <see cref="DiseaseSystem.TriggerOutbreak(IDiseaseOutbreakSource, string, int, IReadOnlyList{string})"/>
        /// with the excavation source contract.
        ///
        /// Passing the state's sites explicitly (rather than reading
        /// <see cref="ExcavationSystem.State"/>) keeps the adapter
        /// free of the host session's reference chain — useful for
        /// selftest with a synthesized <see cref="ExcavationState"/>.
        /// </summary>
        public void FlushExcavationTick(IReadOnlyList<ExcavationSiteSnapshot>? sites)
        {
            if (sites == null || sites.Count == 0) return;
            int day = _simDayProvider();
            var candidates = _survivorPoolProvider() ?? Array.Empty<string>();
            if (candidates.Count == 0) return;

            for (int i = 0; i < sites.Count; i++)
            {
                var s = sites[i];
                if (s == null || string.IsNullOrEmpty(s.SiteId) || !s.IsComplete) continue;
                if (!_previouslyCompleteSites.Add(s.SiteId)) continue;
                var result = _disease.TriggerOutbreak(
                    _excavationSource,
                    "disease_deep_excavation_mold_lung",
                    day,
                    candidates);
                if (result.InfectionsApplied > 0) DigTriggersApplied++;
            }
        }
    }

    /// <summary>
    /// Host-side projection of an <see cref="ExcavationSite"/> for the
    /// adapter. We pass a snapshot rather than the Core type so that
    /// selftest can synthesise the value without file IO or the
    /// Core SumpFloodingSystem dependency chain.
    /// </summary>
    public sealed class ExcavationSiteSnapshot
    {
        public string SiteId { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }
}
