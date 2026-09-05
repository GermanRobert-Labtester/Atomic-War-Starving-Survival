using System;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host adapter subclass forwarding to the canonical engine-agnostic
    /// <see cref="Ashfall.Core.CollectibleEffectDispatcher"/> in Core.
    /// Preserves binary/source compatibility for host callers.
    /// </summary>
    public sealed class CollectibleEffectDispatcher : Ashfall.Core.CollectibleEffectDispatcher
    {
        public CollectibleEffectDispatcher(
            CollectibleCatalog catalog,
            CollectibleDiscoveryState discovery,
            Func<NeedsSystem?>? needsProvider = null,
            Func<ResearchSystem?>? researchProvider = null,
            Func<JournalSystem?>? journalProvider = null,
            Func<WastelandMapSystem?>? mapProvider = null,
            Func<int>? dayProvider = null,
            ILog? log = null)
            : base(catalog, discovery, needsProvider, researchProvider, journalProvider, mapProvider, dayProvider, log)
        {
        }
    }
}
