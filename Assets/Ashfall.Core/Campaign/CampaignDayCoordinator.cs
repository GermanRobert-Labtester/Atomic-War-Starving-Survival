using System;
using System.Collections.Generic;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// ASHFALL — Campaign Day Coordinator.
    ///
    /// Single authority for advancing the campaign by exactly one in-game day.
    /// Owns the seam between <c>Main.CommitAdvance</c> / <c>TickSimDay</c> and
    /// the dozens of subsystems that participate in a daily tick. Replaces the
    /// historical open-coded TickSimDay method so the host can only:
    ///
    /// 1. capture pre-day snapshots in deterministic owner order
    /// 2. advance each owner exactly once in deterministic owner order
    /// 3. collect typed state-change events per owner
    /// 4. persist state through the host's <see cref="IDayAdvancePersistence"/>
    ///    before any blocking UI is presented
    /// 5. emit a single <see cref="DayAdvanced"/> event with the full report
    /// 6. reject re-entrant / double-click advance while a tick is in flight
    ///
    /// This is engine-agnostic. It must not reference Godot, UnityEngine, or
    /// JsonUtility. Host adapters live under <c>src/Host</c> and depend on
    /// Core through this interface only.
    /// </summary>
    public sealed class CampaignDayCoordinator
    {
        private readonly List<RegisteredOwner> _owners = new List<RegisteredOwner>();
        private readonly Dictionary<string, RegisteredOwner> _byId =
            new Dictionary<string, RegisteredOwner>(StringComparer.Ordinal);

        private bool _advancing;
        private int _lastAdvancedDay = int.MinValue;

        /// <summary>
        /// Raised once per successful <see cref="Advance"/> call, after every
        /// owner has been ticked and persistence has been requested. The host
        /// uses this to present the daily briefing modal.
        /// </summary>
        public event Action<DayAdvancedEventArgs> OnDayAdvanced;

        /// <summary>True while an <see cref="Advance"/> is in flight.</summary>
        public bool IsAdvancing => _advancing;

        /// <summary>The most recent day that successfully advanced. -1 before the first call.</summary>
        public int LastAdvancedDay => _lastAdvancedDay == int.MinValue ? -1 : _lastAdvancedDay;

        /// <summary>Registered owners in deterministic ordinal order.</summary>
        public IReadOnlyList<IDayAdvanceOwner> Owners
        {
            get
            {
                var view = new List<IDayAdvanceOwner>(_owners.Count);
                foreach (var r in _owners) view.Add(r.Owner);
                return view;
            }
        }

        /// <summary>
        /// Register a daily tick owner. <paramref name="ownerId"/> must be unique,
        /// stable, and snake_case. The coordinator orders owners by ownerId so
        /// the same owners always tick in the same order regardless of host.
        /// </summary>
        public void Register(string ownerId, IDayAdvanceOwner owner)
        {
            if (string.IsNullOrEmpty(ownerId))
                throw new ArgumentException("ownerId must be non-empty", nameof(ownerId));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (_byId.ContainsKey(ownerId))
                throw new InvalidOperationException(
                    $"CampaignDayCoordinator: owner '{ownerId}' already registered.");
            if (_advancing)
                throw new InvalidOperationException(
                    $"CampaignDayCoordinator: cannot register '{ownerId}' while a day advance is in flight.");
            _owners.Add(new RegisteredOwner(ownerId, owner));
            _byId.Add(ownerId, _owners[_owners.Count - 1]);
            // Keep owners sorted by id so Advance ticks in deterministic order.
            _owners.Sort(static (a, b) => string.CompareOrdinal(a.OwnerId, b.OwnerId));
            // Rebuild byId after the sort to keep references consistent.
            _byId.Clear();
            for (int i = 0; i < _owners.Count; i++)
                _byId[_owners[i].OwnerId] = _owners[i];
        }

        /// <summary>
        /// Unregister an owner. Throws if a tick is in flight (state mutating
        /// during a tick is the very thing this coordinator prevents).
        /// </summary>
        public bool Unregister(string ownerId)
        {
            if (_advancing)
                throw new InvalidOperationException(
                    "CampaignDayCoordinator: cannot unregister while a day advance is in flight.");
            if (string.IsNullOrEmpty(ownerId)) return false;
            if (!_byId.TryGetValue(ownerId, out var reg)) return false;
            _owners.Remove(reg);
            _byId.Remove(ownerId);
            return true;
        }

        /// <summary>
        /// Try to begin advancing <paramref name="day"/>. Returns false if a
        /// previous <see cref="Advance"/> is still in flight (double-click /
        /// re-entrant guard), or if <paramref name="day"/> has already advanced
        /// this coordinator (idempotency guard).
        /// </summary>
        public bool TryBegin(int day)
        {
            if (_advancing) return false;
            if (day <= _lastAdvancedDay) return false;
            _advancing = true;
            return true;
        }

        /// <summary>
        /// Release the re-entrance gate. Callers that successfully
        /// <see cref="TryBegin"/> MUST call this (typically from a finally
        /// block) so the next click can advance.
        /// </summary>
        public void EndAdvance()
        {
            _advancing = false;
        }

        /// <summary>
        /// Advance the campaign by exactly one in-game day. Returns a
        /// <see cref="DayAdvancedEventArgs"/> describing every owner result,
        /// or null when a guard rejects the call (already advancing, or stale
        /// day). The host should treat a null return as "no-op".
        /// </summary>
        public DayAdvancedEventArgs Advance(int day, IDayAdvancePersistence persistence = null)
        {
            if (!TryBegin(day)) return null;

            var reports = new List<DayOwnerReport>(_owners.Count);
            try
            {
                for (int i = 0; i < _owners.Count; i++)
                {
                    var reg = _owners[i];
                    DayOwnerReport report;
                    try
                    {
                        // Capture pre-day snapshot first (idempotent hook).
                        reg.Owner.CapturePreDaySnapshot(day);
                        var events = new List<DayStateChangeEvent>();
                        reg.Owner.TickDay(day, events);
                        report = new DayOwnerReport(reg.OwnerId, true, events, null);
                    }
                    catch (Exception e)
                    {
                        report = new DayOwnerReport(reg.OwnerId, false,
                            Array.Empty<DayStateChangeEvent>(), e.Message);
                    }
                    reports.Add(report);
                }

                // Persistence must happen before the host shows blocking UI.
                persistence?.PersistBeforeBriefing(day, reports);

                _lastAdvancedDay = day;
                var args = new DayAdvancedEventArgs(day, reports);
                OnDayAdvanced?.Invoke(args);
                return args;
            }
            finally
            {
                _advancing = false;
            }
        }

        private sealed class RegisteredOwner
        {
            public readonly string OwnerId;
            public readonly IDayAdvanceOwner Owner;
            public RegisteredOwner(string ownerId, IDayAdvanceOwner owner)
            {
                OwnerId = ownerId;
                Owner = owner;
            }
        }
    }

    /// <summary>Engine-agnostic contract for a system that participates in daily ticks.</summary>
    public interface IDayAdvanceOwner
    {
        /// <summary>
        /// Capture a pre-day snapshot for the given <paramref name="day"/>.
        /// Owners that need a baseline (e.g. consumption accounting) implement
        /// this; owners that are pure side-effect producers may leave it empty.
        /// MUST be idempotent. MUST NOT mutate persistent state.
        /// </summary>
        void CapturePreDaySnapshot(int day);

        /// <summary>
        /// Tick exactly one day. Append typed <see cref="DayStateChangeEvent"/>s
        /// to <paramref name="events"/> in deterministic order. Implementations
        /// MUST be idempotent: calling twice on the same day must yield
        /// identical state and identical event lists.
        /// </summary>
        void TickDay(int day, List<DayStateChangeEvent> events);
    }

    /// <summary>
    /// Optional persistence callback the host injects so the coordinator can
    /// guarantee state is on disk before a blocking modal opens. Implementations
    /// should be small (deferred-flush to the existing per-system save stores).
    /// </summary>
    public interface IDayAdvancePersistence
    {
        void PersistBeforeBriefing(int day, IReadOnlyList<DayOwnerReport> ownerReports);
    }

    /// <summary>Typed state-change event emitted by an owner during a daily tick.</summary>
    [Serializable]
    public sealed class DayStateChangeEvent
    {
        public string Kind;
        public string SourceOwnerId;
        public string PrimaryId;
        public string SecondaryId;
        public float Numeric;

        public DayStateChangeEvent() { }

        public DayStateChangeEvent(string kind, string sourceOwnerId,
            string primaryId = null, string secondaryId = null, float numeric = 0f)
        {
            Kind = kind;
            SourceOwnerId = sourceOwnerId;
            PrimaryId = primaryId ?? string.Empty;
            SecondaryId = secondaryId ?? string.Empty;
            Numeric = numeric;
        }
    }

    /// <summary>Result of a single owner tick during a day advance.</summary>
    [Serializable]
    public sealed class DayOwnerReport
    {
        public string OwnerId;
        public bool Succeeded;
        public DayStateChangeEvent[] Events;
        public string FailureMessage;

        public DayOwnerReport() { }

        public DayOwnerReport(string ownerId, bool succeeded,
            IReadOnlyList<DayStateChangeEvent> events, string failureMessage)
        {
            OwnerId = ownerId;
            Succeeded = succeeded;
            Events = new DayStateChangeEvent[events?.Count ?? 0];
            if (events != null)
                for (int i = 0; i < events.Count; i++)
                    Events[i] = events[i];
            FailureMessage = failureMessage ?? string.Empty;
        }
    }

    /// <summary>Aggregate event emitted after a successful day advance.</summary>
    [Serializable]
    public sealed class DayAdvancedEventArgs
    {
        public int Day;
        public DayOwnerReport[] OwnerReports;
        public int OwnerCount => OwnerReports?.Length ?? 0;

        public DayAdvancedEventArgs() { }

        public DayAdvancedEventArgs(int day, IReadOnlyList<DayOwnerReport> reports)
        {
            Day = day;
            OwnerReports = new DayOwnerReport[reports?.Count ?? 0];
            if (reports != null)
                for (int i = 0; i < reports.Count; i++)
                    OwnerReports[i] = reports[i];
        }

        public IEnumerable<DayStateChangeEvent> AllEvents()
        {
            if (OwnerReports == null) yield break;
            for (int i = 0; i < OwnerReports.Length; i++)
            {
                var rep = OwnerReports[i];
                if (rep?.Events == null) continue;
                for (int j = 0; j < rep.Events.Length; j++)
                    yield return rep.Events[j];
            }
        }
    }
}
