using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Economy
{
    [Serializable]
    public class CaravanInventoryItem
    {
        public string itemId;
        public int quantity;
        public int priceRations;
    }

    [Serializable]
    public class CaravanEntry
    {
        public string caravanId;
        public string caravanName;
        public string factionId;
        public string currentNodeId;
        public List<string> routeNodeIds = new List<string>();
        public int routeIndex = 0;
        public int daysAtCurrentNode = 0;
        public int stayDurationDays = 2;
        public int guardCount = 4;
        public bool isRobbed = false;
        public List<CaravanInventoryItem> inventory = new List<CaravanInventoryItem>();
    }

    [Serializable]
    public class TravelingCaravanState
    {
        public List<CaravanEntry> activeCaravans = new List<CaravanEntry>();
        public int completedTradesCount = 0;
    }

    /// <summary>
    /// Expansion V / Spec §3.3: Traveling Caravan & Convoy Encounters System.
    /// Drives wandering merchant caravans that travel across map nodes,
    /// offering dynamic trade, escort missions, or salvage opportunities.
    /// </summary>
    public class TravelingCaravanSystem
    {
        private TravelingCaravanState _state = new TravelingCaravanState();

        public event Action<CaravanEntry, string> OnCaravanArrivedAtNode;
        public event Action<CaravanEntry, string, int> OnTradeCompleted;

        public TravelingCaravanState State => _state;
        public int CaravanCount => _state.activeCaravans?.Count ?? 0;

        public TravelingCaravanSystem(TravelingCaravanState state = null)
        {
            _state = state ?? new TravelingCaravanState();
            if (_state.activeCaravans == null)
                _state.activeCaravans = new List<CaravanEntry>();
        }

        public void SpawnCaravan(string caravanId, string name, string factionId, List<string> route)
        {
            if (route == null || route.Count == 0) return;

            var caravan = new CaravanEntry
            {
                caravanId = caravanId,
                caravanName = name,
                factionId = factionId,
                currentNodeId = route[0],
                routeNodeIds = new List<string>(route),
                routeIndex = 0,
                daysAtCurrentNode = 0,
                stayDurationDays = 2,
                guardCount = 4,
                inventory = new List<CaravanInventoryItem>
                {
                    new CaravanInventoryItem { itemId = "item_canned_food", quantity = 8, priceRations = 2 },
                    new CaravanInventoryItem { itemId = "item_clean_water", quantity = 10, priceRations = 1 },
                    new CaravanInventoryItem { itemId = "item_antibiotics", quantity = 3, priceRations = 5 }
                }
            };

            _state.activeCaravans.Add(caravan);
            OnCaravanArrivedAtNode?.Invoke(caravan, caravan.currentNodeId);
        }

        public CaravanEntry GetCaravanAtNode(string nodeId)
        {
            return _state.activeCaravans.Find(c => c.currentNodeId == nodeId && !c.isRobbed);
        }

        /// <summary>
        /// Daily tick: increments stay duration and advances caravans to the next route waypoint.
        /// </summary>
        public void DailyTick()
        {
            if (_state.activeCaravans == null || _state.activeCaravans.Count == 0) return;

            foreach (var caravan in _state.activeCaravans)
            {
                if (caravan.isRobbed) continue;

                caravan.daysAtCurrentNode++;
                if (caravan.daysAtCurrentNode >= caravan.stayDurationDays)
                {
                    caravan.daysAtCurrentNode = 0;
                    caravan.routeIndex = (caravan.routeIndex + 1) % caravan.routeNodeIds.Count;
                    caravan.currentNodeId = caravan.routeNodeIds[caravan.routeIndex];
                    OnCaravanArrivedAtNode?.Invoke(caravan, caravan.currentNodeId);
                }
            }
        }

        public bool TryBuyItem(string caravanId, string itemId, int amount, ref int playerRations)
        {
            var caravan = _state.activeCaravans.Find(c => c.caravanId == caravanId);
            if (caravan == null || caravan.isRobbed) return false;

            var stock = caravan.inventory.Find(i => i.itemId == itemId);
            if (stock == null || stock.quantity < amount) return false;

            int totalCost = stock.priceRations * amount;
            if (playerRations < totalCost) return false;

            playerRations -= totalCost;
            stock.quantity -= amount;
            _state.completedTradesCount++;
            OnTradeCompleted?.Invoke(caravan, itemId, amount);
            return true;
        }

        public TravelingCaravanState CaptureState() => _state;

        public void RestoreState(TravelingCaravanState state)
        {
            _state = state ?? new TravelingCaravanState();
            if (_state.activeCaravans == null)
                _state.activeCaravans = new List<CaravanEntry>();
        }
    }
}
