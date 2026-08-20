using System.Collections.Generic;
using Ashfall.Core.Campaign;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Adapter that lets the engine-agnostic
    /// <see cref="CampaignDayCoordinator"/> persist state before any
    /// blocking UI is presented. The adapter is a thin Godot-side glue
    /// layer; persistence logic still lives in the per-system save stores
    /// under <c>src/Host/</c>.
    /// </summary>
    internal sealed class CampaignDayPersistenceAdapter : IDayAdvancePersistence
    {
        private readonly Main _main;

        public CampaignDayPersistenceAdapter(Main main)
        {
            _main = main;
        }

        public void PersistBeforeBriefing(int day, IReadOnlyList<DayOwnerReport> ownerReports)
        {
            if (_main == null) return;
            // The actual deferred flushes happen inside SaveAll. We trigger
            // one focused save of any systems whose dirty flag was set
            // during this tick; full SaveAll still runs at the end of
            // CommitAdvance for the auto-save setting.
            _main.FlushDirtyStoresForDayAdvance();
        }
    }
}
