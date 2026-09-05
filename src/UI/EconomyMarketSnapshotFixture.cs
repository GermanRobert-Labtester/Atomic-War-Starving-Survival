using System;
using AtomicWar.GodotApp.Economy;
using Godot;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 56 phase 4 — snapshot fixture for the market panel: binds a real
    /// catalog-loaded economy session and evaluates provenance from the
    /// shelter region ("settlement") so the golden shows the text provenance
    /// tags exactly as players see them.
    /// </summary>
    internal static class EconomyMarketSnapshotFixture
    {
        public static IDisposable? Bind(Node node)
        {
            if (node is not EconomyMarketPanel panel)
                return null;

            string dataDir = CatalogPath.ResolveDataDir();
            var session = EconomyHostSession.Create(dataDir);
            panel.CurrentRegion = "settlement";
            panel.BindSession(session);
            return session;
        }
    }
}
