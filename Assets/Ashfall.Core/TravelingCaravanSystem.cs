// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
#pragma warning disable CS8618

namespace Ashfall.Core
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
        public string originRegion; // flotilla, foundry, greenhouse, settlement, traplines
        public string currentNodeId;
        public List<string> routeNodeIds = new List<string>();
        public int routeIndex = 0;
        public int daysAtCurrentNode = 0;
        public int stayDurationDays = 2;
        public int guardCount = 4;
        public bool isRobbed = false;
        // Plan 14A — durable embargo transition flag (set when the route is
        // blocked, cleared when movement resumes). Saved so the blocked state
        // survives reloads; route blocking itself stays derived from weather.
        public bool embargoBlocked = false;
        public List<CaravanInventoryItem> inventory = new List<CaravanInventoryItem>();
    }

    [Serializable]
    public class TravelingCaravanState
    {
        public List<CaravanEntry> activeCaravans = new List<CaravanEntry>();
        public int completedTradesCount = 0;
    }

    /// <summary>
    /// Engine-agnostic Traveling Caravan System (Expansion V / Spec §3.3).
    /// Manages wandering merchant caravans across map nodes, offering trade,
    /// route progression, and state persistence without engine dependencies.
    /// </summary>
    public class TravelingCaravanSystem
    {
        public const string SystemId = "traveling_caravan_system";

        private TravelingCaravanState _state;

        /// <summary>
        /// Plan 56 phase 3 — when a goods catalog is bound, regional specialty
        /// stock is generated from <c>regionalSupply</c> provenance via
        /// RegionalSupplyRouter. Unbound systems (headless demo) keep the
        /// legacy hand-coded regional tables.
        /// </summary>
        public GoodsCatalog? Catalog { get; set; }
        public Narrative.TravelEncounterSystem? TravelEncounters { get; set; }
        /// <summary>
        /// Plan 14A — embargo authority. Bound by the host; unbound, caravan
        /// movement behaves exactly as before (no blocking, no slowdown).
        /// </summary>
        public TradeEmbargoSystem? Embargoes { get; set; }

        public event Action<CaravanEntry, string>? OnCaravanArrivedAtNode;
        public event Action<CaravanEntry, string, int>? OnTradeCompleted;
        public event Action<CaravanEntry, Narrative.TravelEncounterDefinition>? OnCaravanPatrolEncountered;
        /// <summary>Plan 14A — raised once per blocked TRANSITION (not per tick): caravan, weather name.</summary>
        public event Action<CaravanEntry, string>? OnCaravanEmbargoed;
        /// <summary>Plan 14A — raised once when a blocked caravan resumes movement.</summary>
        public event Action<CaravanEntry>? OnCaravanResumed;

        public TravelingCaravanState State => _state;
        public int CaravanCount => _state.activeCaravans?.Count ?? 0;

        public TravelingCaravanSystem(TravelingCaravanState? state = null)
        {
            _state = state ?? new TravelingCaravanState();
            if (_state.activeCaravans == null)
                _state.activeCaravans = new List<CaravanEntry>();
        }

        public void SpawnCaravan(string caravanId, string name, string factionId, List<string> route,
            string originRegion = "settlement")
        {
            if (route == null || route.Count == 0) return;

            var caravan = new CaravanEntry
            {
                caravanId = caravanId,
                caravanName = name,
                factionId = factionId,
                originRegion = originRegion,
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

            // Add regional specialty stock based on origin
            AddRegionalSpecialtyStock(this, caravan, originRegion);

            _state.activeCaravans.Add(caravan);
            OnCaravanArrivedAtNode?.Invoke(caravan, caravan.currentNodeId);
        }

        /// <summary>
        /// Add regional specialty goods to a caravan based on its origin region.
        /// With a bound goods catalog, the stock is generated from
        /// <c>regionalSupply</c> provenance (RegionalSupplyRouter) — the data
        /// authority decides what each region produces. Without a catalog the
        /// legacy hand-coded tables apply (headless demo compatibility).
        /// Each region carries goods not available elsewhere, creating
        /// route-planning pressure.
        /// </summary>
        private static void AddRegionalSpecialtyStock(TravelingCaravanSystem system, CaravanEntry caravan, string region)
        {
            if (system.Catalog != null)
            {
                var lots = RegionalSupplyRouter.SpecialtyCargoForOrigin(
                    system.Catalog, region, maxLots: 4);
                for (int i = 0; i < lots.Count; i++)
                {
                    caravan.inventory.Add(new CaravanInventoryItem
                    {
                        itemId = lots[i].GoodId,
                        quantity = lots[i].Quantity,
                        priceRations = lots[i].PriceRations
                    });
                }
                return;
            }

            switch (region)
            {
                case "flotilla":
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "item_desal_membrane", quantity = 2, priceRations = 8 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "item_foundry_brine_pipe", quantity = 3, priceRations = 6 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "item_ro_membrane", quantity = 2, priceRations = 7 });
                    break;
                case "foundry":
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "water_filter", quantity = 4, priceRations = 4 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "air_filter", quantity = 3, priceRations = 4 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "mechanical_parts", quantity = 10, priceRations = 1 });
                    break;
                case "greenhouse":
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "seed_packets", quantity = 5, priceRations = 6 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "canned_food", quantity = 6, priceRations = 2 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "herbal_tea", quantity = 4, priceRations = 3 });
                    break;
                case "traplines":
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "cooked_meat", quantity = 6, priceRations = 3 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "item_frostbite_salve", quantity = 3, priceRations = 4 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "leather_strap", quantity = 8, priceRations = 1 });
                    break;
                default: // settlement
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "medical_kit", quantity = 2, priceRations = 6 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "solar_cell", quantity = 2, priceRations = 7 });
                    caravan.inventory.Add(new CaravanInventoryItem { itemId = "anti_rad", quantity = 2, priceRations = 8 });
                    break;
            }
        }

        public CaravanEntry? GetCaravanAtNode(string nodeId)
        {
            return _state.activeCaravans.Find(c => c.currentNodeId == nodeId && !c.isRobbed);
        }

        /// <summary>
        /// Daily tick: increments stay duration and advances caravans to the next route waypoint.
        /// Plan 14A — with an embargo system bound and the day's weather
        /// supplied, a caravan whose origin region is blocked does not advance
        /// (one transition event, then daily silence) and a slowed region pays
        /// an additional travel-day cost (progress &lt; 1 extends the stay
        /// requirement by the missing fraction, ceil). Default weather Clear
        /// is embargo-neutral, so legacy callers are unchanged.
        /// </summary>
        public void DailyTick() => DailyTick(0, null);

        /// <summary>
        /// C2 / Plan 20C (§41) — optional weather-availability provider from
        /// the ONE weather-effects table (caravan_availability_multiplier).
        /// Applied as a movement-progress modifier — distinct from (and
        /// combined with) the geopolitical embargo gate, per plan §41. Unbound
        /// keeps legacy movement byte-identical.
        /// </summary>
        public Func<WeatherKind, float>? WeatherAvailabilityProvider { get; set; }

        public void DailyTick(
            int currentDay,
            ISeededRng? rng = null,
            string defaultRegion = "the_toll",
            int dangerLevel = 2,
            string season = "all",
            WeatherKind weather = WeatherKind.Clear)
        {
            if (_state.activeCaravans == null || _state.activeCaravans.Count == 0) return;

            foreach (var caravan in _state.activeCaravans)
            {
                if (caravan.isRobbed) continue;

                // Plan 14A — embargo evaluation against the authoritative
                // weather, per caravan origin region.
                bool blocked = false;
                float progress = 1f;
                if (Embargoes != null)
                {
                    blocked = Embargoes.IsRouteBlocked(caravan.originRegion, weather);
                    if (!blocked)
                        progress = Embargoes.GetRouteProgressMultiplier(caravan.originRegion, weather);
                }
                // Plan 20C §41 — physical weather inaccessibility combines with
                // (never mixes into) the embargo multiplier at this decision.
                if (!blocked && WeatherAvailabilityProvider != null)
                    progress *= Math.Clamp(WeatherAvailabilityProvider(weather), 0f, 5f);

                if (blocked)
                {
                    if (!caravan.embargoBlocked)
                    {
                        caravan.embargoBlocked = true;
                        OnCaravanEmbargoed?.Invoke(caravan, weather.ToString());
                    }
                    continue; // blocked caravans lose the movement day
                }

                if (caravan.embargoBlocked)
                {
                    caravan.embargoBlocked = false;
                    OnCaravanResumed?.Invoke(caravan);
                }

                // Slowdown as an additional travel-day cost: a progress of 0.5
                // doubles the days required per node; neutral adds none.
                int effectiveStay = caravan.stayDurationDays;
                if (progress < 1f && progress > 0f)
                    effectiveStay = caravan.stayDurationDays
                        + (int)Math.Ceiling(caravan.stayDurationDays * (1f / progress - 1f));

                caravan.daysAtCurrentNode++;
                if (caravan.daysAtCurrentNode >= effectiveStay)
                {
                    caravan.daysAtCurrentNode = 0;
                    caravan.routeIndex = (caravan.routeIndex + 1) % caravan.routeNodeIds.Count;
                    caravan.currentNodeId = caravan.routeNodeIds[caravan.routeIndex];
                    OnCaravanArrivedAtNode?.Invoke(caravan, caravan.currentNodeId);

                    if (TravelEncounters != null && rng != null)
                    {
                        CheckRouteEncounter(caravan, defaultRegion, dangerLevel, season, currentDay, rng);
                    }
                }
            }
        }

        public Narrative.TravelEncounterDefinition? CheckRouteEncounter(
            CaravanEntry caravan,
            string region,
            int dangerLevel,
            string season,
            int currentDay,
            ISeededRng rng,
            string stance = "Balanced")
        {
            if (TravelEncounters == null || caravan == null) return null;

            var context = Narrative.TravelEncounterSelectionContext.From(
                region: region,
                dangerLevel: dangerLevel,
                stance: stance,
                currentSeason: season,
                currentDay: currentDay,
                currentWeather: WeatherKind.Clear,
                rng: rng,
                locationId: caravan.currentNodeId,
                routeId: caravan.caravanId,
                mode: Narrative.TravelMode.Caravan);

            var encounter = TravelEncounters.SelectEncounter(context);
            if (encounter != null)
            {
                OnCaravanPatrolEncountered?.Invoke(caravan, encounter);
            }
            return encounter;
        }

        public bool ResolveRouteEncounterChoice(
            string encounterId,
            string choiceId,
            int currentDay,
            out Narrative.TravelEncounterResolutionResult? result)
        {
            if (TravelEncounters == null)
            {
                result = null;
                return false;
            }
            return TravelEncounters.ResolveChoice(encounterId, choiceId, currentDay, out result);
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

        public TravelingCaravanState CaptureState()
        {
            var snapshot = new TravelingCaravanState
            {
                completedTradesCount = _state.completedTradesCount,
                activeCaravans = new List<CaravanEntry>()
            };

            if (_state.activeCaravans != null)
            {
                foreach (var c in _state.activeCaravans)
                {
                    if (c == null) continue;
                    var copy = new CaravanEntry
                    {
                        caravanId = c.caravanId,
                        caravanName = c.caravanName,
                        factionId = c.factionId,
                        originRegion = c.originRegion ?? "settlement",
                        currentNodeId = c.currentNodeId,
                        routeIndex = c.routeIndex,
                        daysAtCurrentNode = c.daysAtCurrentNode,
                        stayDurationDays = c.stayDurationDays,
                        guardCount = c.guardCount,
                        isRobbed = c.isRobbed,
                        embargoBlocked = c.embargoBlocked,
                        routeNodeIds = c.routeNodeIds != null ? new List<string>(c.routeNodeIds) : new List<string>(),
                        inventory = new List<CaravanInventoryItem>()
                    };

                    if (c.inventory != null)
                    {
                        foreach (var inv in c.inventory)
                        {
                            if (inv == null) continue;
                            copy.inventory.Add(new CaravanInventoryItem
                            {
                                itemId = inv.itemId,
                                quantity = inv.quantity,
                                priceRations = inv.priceRations
                            });
                        }
                    }
                    snapshot.activeCaravans.Add(copy);
                }
            }

            return snapshot;
        }

        public void RestoreState(TravelingCaravanState state)
        {
            if (state == null) return;
            _state.completedTradesCount = Math.Max(0, state.completedTradesCount);
            _state.activeCaravans.Clear();
            if (state.activeCaravans != null)
            {
                foreach (var c in state.activeCaravans)
                {
                    if (c == null) continue;
                    var copy = new CaravanEntry
                    {
                        caravanId = c.caravanId,
                        caravanName = c.caravanName,
                        factionId = c.factionId,
                        originRegion = c.originRegion ?? "settlement",
                        currentNodeId = c.currentNodeId,
                        routeIndex = c.routeIndex,
                        daysAtCurrentNode = c.daysAtCurrentNode,
                        stayDurationDays = c.stayDurationDays,
                        guardCount = c.guardCount,
                        isRobbed = c.isRobbed,
                        embargoBlocked = c.embargoBlocked,
                        routeNodeIds = c.routeNodeIds != null ? new List<string>(c.routeNodeIds) : new List<string>(),
                        inventory = new List<CaravanInventoryItem>()
                    };
                    if (c.inventory != null)
                    {
                        foreach (var inv in c.inventory)
                        {
                            if (inv == null) continue;
                            copy.inventory.Add(new CaravanInventoryItem
                            {
                                itemId = inv.itemId,
                                quantity = inv.quantity,
                                priceRations = inv.priceRations
                            });
                        }
                    }
                    _state.activeCaravans.Add(copy);
                }
            }
        }
    }
}
