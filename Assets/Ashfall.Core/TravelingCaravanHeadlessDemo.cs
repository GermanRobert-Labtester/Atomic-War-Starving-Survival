using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    public static class TravelingCaravanHeadlessDemo
    {
        public static int Run()
        {
            Console.WriteLine("[TravelingCaravanHeadlessDemo] begin");
            int pass = 0;
            int total = 0;

            void Assert(bool cond, string msg)
            {
                total++;
                if (cond)
                {
                    pass++;
                    Console.WriteLine($"[PASS] {msg}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] {msg}");
                }
            }

            var system = new TravelingCaravanSystem();
            Assert(system.CaravanCount == 0, "caravan system starts empty");

            var route = new List<string> { "node_crossing", "node_depot", "node_hydro" };
            system.SpawnCaravan("caravan_scalehouse", "Scalehouse Grain Train", "faction_the_scale", route);
            Assert(system.CaravanCount == 1, "caravan spawned");

            var cAtCrossing = system.GetCaravanAtNode("node_crossing");
            Assert(cAtCrossing != null, "caravan starts at first waypoint");
            Assert(cAtCrossing?.caravanId == "caravan_scalehouse", "correct caravan ID");

            system.DailyTick();
            Assert(system.GetCaravanAtNode("node_crossing") != null, "caravan stays on day 1");

            system.DailyTick();
            Assert(system.GetCaravanAtNode("node_crossing") == null, "caravan left node_crossing after stay duration");
            Assert(system.GetCaravanAtNode("node_depot") != null, "caravan arrived at node_depot");

            int playerRations = 10;
            bool bought = system.TryBuyItem("caravan_scalehouse", "item_clean_water", 2, ref playerRations);
            Assert(bought, "buying clean water succeeds");
            Assert(playerRations == 8, "player rations deducted");
            Assert(system.State.completedTradesCount == 1, "completed trades incremented");

            int insufficientRations = 1;
            bool failBuy = system.TryBuyItem("caravan_scalehouse", "item_canned_food", 5, ref insufficientRations);
            Assert(!failBuy, "purchase fails when player lacks rations");
            Assert(insufficientRations == 1, "rations unchanged on failed buy");

            var snapshot = system.CaptureState();
            Assert(snapshot.completedTradesCount == 1, "state capture preserves trade count");
            Assert(snapshot.activeCaravans.Count == 1, "state capture preserves active caravans");

            var newSystem = new TravelingCaravanSystem();
            newSystem.RestoreState(snapshot);
            Assert(newSystem.CaravanCount == 1, "state restored active caravans count");
            Assert(newSystem.State.completedTradesCount == 1, "state restored trade count");

            Console.WriteLine($"TravelingCaravanHeadlessDemo PASS {pass}/{total}");
            return pass == total ? 0 : 1;
        }
    }
}
