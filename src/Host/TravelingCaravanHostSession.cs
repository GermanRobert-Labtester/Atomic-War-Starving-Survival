using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Traveling Caravan system (Expansion V
    /// spec §3.3). Wires the engine-agnostic caravan core that was
    /// selftest-only into the live host: spawn, daily route ticks, and
    /// ration-based trade. No gameplay rules here — hosts only present.
    /// </summary>
    public sealed class TravelingCaravanHostSession
    {
        public TravelingCaravanSystem Engine { get; }

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public TravelingCaravanHostSession(TravelingCaravanSystem engine = null!)
        {
            Engine = engine ?? new TravelingCaravanSystem();
            Engine.OnCaravanArrivedAtNode += (c, node) => { LastEvent = c.caravanName + " arrived at " + node + "."; StateChanged?.Invoke(); };
            Engine.OnTradeCompleted += (c, item, qty) => { LastEvent = "Traded " + qty + " × " + item + " with " + c.caravanName + "."; StateChanged?.Invoke(); };
        }

        public static TravelingCaravanHostSession Create(string dataDir)
        {
            var session = new TravelingCaravanHostSession();
            var save = CaravanSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save);
                session.LastEvent = "Caravan state restored from save.";
            }
            return session;
        }

        // ── Demo actions (dev buttons / headless) ─────────────────────

        public string SpawnDemoCaravan(string nodeId)
        {
            Engine.SpawnCaravan(
                "caravan_menders",
                "The Menders' Cart",
                "faction_wandering_menders",
                new List<string> { nodeId, "loc_cut_kilometre_19", "loc_the_allotments", nodeId });
            LastEvent = "Caravan spawned at " + nodeId + ".";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string TickDemo()
        {
            Engine.DailyTick();
            LastEvent = "Caravan day ticked.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string BuyDemo(string caravanId, string itemId, int amount, ref int playerRations)
        {
            bool ok = Engine.TryBuyItem(caravanId, itemId, amount, ref playerRations);
            LastEvent = ok
                ? $"Bought {amount} × {itemId} for {amount * ItemPrice(caravanId, itemId)} rations."
                : $"Could not buy {amount} × {itemId} (stock or rations short).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        private int ItemPrice(string caravanId, string itemId)
        {
            var c = Engine.State.activeCaravans.Find(x => x.caravanId == caravanId);
            var s = c?.inventory.Find(i => i.itemId == itemId);
            return s?.priceRations ?? 1;
        }

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder("Caravans: " + Engine.CaravanCount + " active · " +
                Engine.State.completedTradesCount + " trades");
            for (int i = 0; i < Engine.State.activeCaravans.Count; i++)
            {
                var c = Engine.State.activeCaravans[i];
                sb.Append("\n  ").Append(c.caravanName).Append(" @ ").Append(c.currentNodeId)
                    .Append(" (").Append(c.daysAtCurrentNode).Append("/").Append(c.stayDurationDays).Append("d)");
            }
            return sb.ToString();
        }

        public TravelingCaravanState CaptureSave() => Engine.CaptureState();
        public void RestoreSave(TravelingCaravanState state) => Engine.RestoreState(state);
    }
}
