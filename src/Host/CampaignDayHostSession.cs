// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Campaign;
using Ashfall.Core.Events;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host boundary for the canonical day coordinator. Registration, ordering,
    /// rollback, deterministic streams and persistence remain with that coordinator.
    /// This adapter exposes its committed day fact through the shared event bus.
    /// </summary>
    internal sealed class CampaignDayHostSession : IDisposable
    {
        public const string DayAdvancedEvent = "campaign_day_advanced";
        public CampaignDayCoordinator Coordinator { get; }
        private readonly IEventBus _events;

        public CampaignDayHostSession(CampaignDayCoordinator coordinator, IEventBus events)
        {
            Coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
            _events = events ?? throw new ArgumentNullException(nameof(events));
            Coordinator.OnDayAdvanced += PublishCommittedDay;
        }

        public DayAdvancedEventArgs? Advance(int day, IDayAdvancePersistence? persistence = null)
            => Coordinator.Advance(day, persistence);

        private void PublishCommittedDay(DayAdvancedEventArgs args)
        {
            // Publish the committed calendar fact, without retaining the large per-owner
            // report graph in SimpleEventBus's diagnostic event history.
            if (args.Succeeded) _events.Publish(DayAdvancedEvent, args.Day);
        }

        public void Dispose() => Coordinator.OnDayAdvanced -= PublishCommittedDay;
    }
}
