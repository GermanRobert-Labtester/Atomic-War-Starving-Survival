// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Economy
{
    public enum CaravanStatus
    {
        Scheduled,
        InTransit,
        Arrived,
        Departed,
        Lost
    }

    public enum CaravanHazardOutcome
    {
        None,
        Safe,
        Delayed,
        StockLoss,
        Lost
    }

    [Serializable]
    public sealed class CaravanManifestState
    {
        public string manifest_id { get; set; } = string.Empty;
        public string route_id { get; set; } = string.Empty;
        public string faction_id { get; set; } = string.Empty;
        public CaravanStatus status { get; set; } = CaravanStatus.Scheduled;
        public int departure_origin_day { get; set; }
        public int expected_arrival_day { get; set; }
        public int actual_arrival_day { get; set; }
        public int departure_day { get; set; }
        public int transit_progress_days { get; set; }
        public int escort_guard_strength { get; set; }
        public bool hazard_resolved { get; set; }
        public CaravanHazardOutcome hazard_outcome { get; set; } = CaravanHazardOutcome.None;
        public Dictionary<string, int> stocks { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public int stay_duration_days { get; set; } = 3;
    }

    [Serializable]
    public sealed class CaravanTradeNetworkSave
    {
        public string systemId { get; set; } = "caravan_trade_network";
        public int schema_version { get; set; } = 1;
        public int last_tick_day { get; set; } = 1;
        public List<CaravanManifestState> caravans { get; set; } = new List<CaravanManifestState>();
        public Dictionary<string, int> faction_profitable_trades { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public List<string> favored_factions { get; set; } = new List<string>();
    }

    public sealed class BarterTransactionResult
    {
        public bool Success { get; set; }
        public string FailureReason { get; set; } = string.Empty;
        public float OfferedValue { get; set; }
        public float RequestedValue { get; set; }
        public bool UnlockedFavoredStatus { get; set; }

        public static BarterTransactionResult Fail(string reason) =>
            new BarterTransactionResult { Success = false, FailureReason = reason };
    }

    public sealed class CaravanTradeNetworkSystem
    {
        public const string SystemId = "caravan_trade_network";
        public const int TradesRequiredForFavoredStatus = 5;
        public const float FavoredTariffDiscount = 0.15f; // 15% tariff reduction
        public const float ImportDemandMultiplier = 1.50f; // +50% demand premium
        public const float ExportSurplusMultiplier = 0.70f; // -30% surplus discount

        private readonly List<CaravanRouteDefinition> _routes = new List<CaravanRouteDefinition>();
        private readonly Dictionary<string, CaravanRouteDefinition> _routesById = new Dictionary<string, CaravanRouteDefinition>(StringComparer.Ordinal);
        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private CaravanTradeNetworkSave _state = new CaravanTradeNetworkSave();

        public event Action<CaravanManifestState>? OnCaravanScheduled;
        public event Action<CaravanManifestState>? OnCaravanArrived;
        public event Action<CaravanManifestState, CaravanHazardOutcome>? OnCaravanHazardResolved;
        public event Action<CaravanManifestState>? OnCaravanDeparted;
        public event Action<string, float, float>? OnTradeCompleted; // faction, offeredVal, requestedVal
        public event Action<string>? OnFavoredBarterStatusUnlocked; // factionId

        public IReadOnlyList<CaravanRouteDefinition> Routes => _routes;
        public IReadOnlyList<CaravanManifestState> Caravans => _state.caravans;
        public IReadOnlyDictionary<string, int> FactionTrades => _state.faction_profitable_trades;
        public IReadOnlyList<string> FavoredFactions => _state.favored_factions;

        public CaravanTradeNetworkSystem(
            IEnumerable<CaravanRouteDefinition> routes,
            Inventory.Inventory inventory,
            ISeededRng rng,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;

            if (routes != null)
            {
                foreach (var r in routes)
                {
                    if (r == null || string.IsNullOrEmpty(r.route_id)) continue;
                    _routes.Add(r);
                    _routesById[r.route_id] = r;
                }
            }
        }

        public bool HasFavoredStatus(string factionId) =>
            !string.IsNullOrEmpty(factionId) && _state.favored_factions.Contains(factionId);

        public int GetProfitableTradeCount(string factionId) =>
            _state.faction_profitable_trades.TryGetValue(factionId, out int count) ? count : 0;

        public CaravanManifestState? FindActiveCaravanForRoute(string route_id)
        {
            for (int i = 0; i < _state.caravans.Count; i++)
            {
                var c = _state.caravans[i];
                if (c.route_id == route_id && (c.status == CaravanStatus.InTransit || c.status == CaravanStatus.Arrived))
                    return c;
            }
            return null;
        }

        public CaravanManifestState? FindManifest(string manifestId)
        {
            for (int i = 0; i < _state.caravans.Count; i++)
            {
                if (_state.caravans[i].manifest_id == manifestId) return _state.caravans[i];
            }
            return null;
        }

        public CaravanManifestState ScheduleCaravan(string routeId, int currentDay, int escortStrength = 0)
        {
            if (!_routesById.TryGetValue(routeId, out var route))
                throw new ArgumentException($"Unknown route: {routeId}", nameof(routeId));

            string manifestId = $"manifest_{routeId}_{currentDay}_{_state.caravans.Count + 1}";
            var manifest = new CaravanManifestState
            {
                manifest_id = manifestId,
                route_id = routeId,
                faction_id = route.faction_id,
                status = CaravanStatus.Scheduled,
                departure_origin_day = currentDay,
                expected_arrival_day = currentDay + route.travel_days,
                actual_arrival_day = currentDay + route.travel_days,
                departure_day = currentDay + route.travel_days + 3,
                transit_progress_days = 0,
                escort_guard_strength = escortStrength > 0 ? escortStrength : route.base_guard_strength,
                stay_duration_days = 3
            };

            // Populate initial stocks deterministically
            foreach (var item in route.export_surpluses)
            {
                manifest.stocks[item] = 10 + _rng.Next(0, 10);
            }

            _state.caravans.Add(manifest);
            OnCaravanScheduled?.Invoke(manifest);
            return manifest;
        }

        public float CalculateItemBuyPrice(CaravanManifestState manifest, string itemId, float crisisMultiplier = 1.0f)
        {
            if (!_routesById.TryGetValue(manifest.route_id, out var route))
                return 10.0f;

            float baseValue = GetCanonicalItemValue(itemId);
            float multiplier = 1.0f;

            // Faction export surpluses are sold at 30% discount
            if (route.export_surpluses.Contains(itemId))
            {
                multiplier *= ExportSurplusMultiplier;
            }

            multiplier *= Math.Max(0.5f, crisisMultiplier);

            // Favored barter status discount (-15%)
            if (HasFavoredStatus(manifest.faction_id))
            {
                multiplier *= (1.0f - FavoredTariffDiscount);
            }

            // Treaty trade-pact price relief (Plan VIII · Task 21.4) — visible
            // factor in the same multiplier stack, never a post-hoc price edit.
            float treatyRelief = GetTreatyPriceRelief(manifest.faction_id);
            if (treatyRelief > 0f)
            {
                multiplier *= (1.0f - treatyRelief);
            }

            return Math.Max(1.0f, (float)Math.Round(baseValue * multiplier, 2));
        }

        public float CalculateItemSellPrice(CaravanManifestState manifest, string itemId, float crisisMultiplier = 1.0f)
        {
            if (!_routesById.TryGetValue(manifest.route_id, out var route))
                return 10.0f;

            float baseValue = GetCanonicalItemValue(itemId);
            float multiplier = 1.0f;

            // Faction import demands are bought at +50% premium
            if (route.import_demands.Contains(itemId))
            {
                multiplier *= ImportDemandMultiplier;
            }

            multiplier *= Math.Max(0.5f, crisisMultiplier);

            return Math.Max(1.0f, (float)Math.Round(baseValue * multiplier, 2));
        }

        public BarterTransactionResult ExecuteBarter(
            string manifestId,
            Dictionary<string, int> playerOffered,
            Dictionary<string, int> playerRequested,
            float crisisMultiplier = 1.0f)
        {
            var manifest = FindManifest(manifestId);
            if (manifest == null)
                return BarterTransactionResult.Fail("manifest_not_found");
            if (manifest.status != CaravanStatus.Arrived)
                return BarterTransactionResult.Fail("caravan_not_arrived");
            if (playerOffered == null || playerOffered.Count == 0)
                return BarterTransactionResult.Fail("empty_offered_goods");
            if (playerRequested == null || playerRequested.Count == 0)
                return BarterTransactionResult.Fail("empty_requested_goods");

            // Validate player goods
            foreach (var kv in playerOffered)
            {
                if (kv.Value <= 0) return BarterTransactionResult.Fail("invalid_offered_quantity");
                if (!_inventory.HasSufficient(kv.Key, kv.Value))
                    return BarterTransactionResult.Fail($"insufficient_player_stock_{kv.Key}");
            }

            // Validate caravan goods
            foreach (var kv in playerRequested)
            {
                if (kv.Value <= 0) return BarterTransactionResult.Fail("invalid_requested_quantity");
                if (!manifest.stocks.TryGetValue(kv.Key, out int stock) || stock < kv.Value)
                    return BarterTransactionResult.Fail($"insufficient_caravan_stock_{kv.Key}");
            }

            // Calculate valuation
            float totalOfferedValue = 0f;
            foreach (var kv in playerOffered)
            {
                totalOfferedValue += CalculateItemSellPrice(manifest, kv.Key, crisisMultiplier) * kv.Value;
            }

            float totalRequestedValue = 0f;
            foreach (var kv in playerRequested)
            {
                totalRequestedValue += CalculateItemBuyPrice(manifest, kv.Key, crisisMultiplier) * kv.Value;
            }

            if (totalOfferedValue < totalRequestedValue)
            {
                return BarterTransactionResult.Fail("insufficient_barter_value");
            }

            // Execute atomic mutations
            foreach (var kv in playerOffered)
            {
                if (!_inventory.TryConsume(kv.Key, kv.Value))
                    throw new InvalidOperationException($"Atomic barter failed during consumption of {kv.Key}");
                if (!manifest.stocks.ContainsKey(kv.Key))
                    manifest.stocks[kv.Key] = 0;
                manifest.stocks[kv.Key] += kv.Value;
            }

            foreach (var kv in playerRequested)
            {
                manifest.stocks[kv.Key] -= kv.Value;
                if (!_inventory.TryProduce(kv.Key, kv.Value))
                    throw new InvalidOperationException($"Atomic barter failed during grant of {kv.Key}");
            }

            // Update reputation & check favored status
            string faction = manifest.faction_id;
            if (!_state.faction_profitable_trades.ContainsKey(faction))
                _state.faction_profitable_trades[faction] = 0;
            _state.faction_profitable_trades[faction]++;

            bool unlocked = false;
            if (_state.faction_profitable_trades[faction] >= TradesRequiredForFavoredStatus &&
                !_state.favored_factions.Contains(faction))
            {
                _state.favored_factions.Add(faction);
                unlocked = true;
                OnFavoredBarterStatusUnlocked?.Invoke(faction);
                _log.Info($"[Caravan] Faction {faction} granted Favored Barter Status (-15% tariffs)!");
            }

            OnTradeCompleted?.Invoke(faction, totalOfferedValue, totalRequestedValue);

            return new BarterTransactionResult
            {
                Success = true,
                OfferedValue = totalOfferedValue,
                RequestedValue = totalRequestedValue,
                UnlockedFavoredStatus = unlocked
            };
        }

        public void TickDay(int day)
        {
            _state.last_tick_day = day;

            // 1. Advance transit & resolve hazards
            for (int i = 0; i < _state.caravans.Count; i++)
            {
                var c = _state.caravans[i];
                if (c.status == CaravanStatus.Scheduled)
                {
                    c.status = CaravanStatus.InTransit;
                }

                if (c.status == CaravanStatus.InTransit)
                {
                    c.transit_progress_days++;
                    _routesById.TryGetValue(c.route_id, out var route);
                    int totalTravel = route?.travel_days ?? 5;

                    // Resolve hazard at midpoint
                    if (!c.hazard_resolved && c.transit_progress_days >= (totalTravel / 2))
                    {
                        ResolveHazard(c, route);
                    }

                    if (c.status == CaravanStatus.Lost) continue;

                    // Check arrival
                    if (c.transit_progress_days >= totalTravel)
                    {
                        c.status = CaravanStatus.Arrived;
                        c.actual_arrival_day = day;
                        c.departure_day = day + c.stay_duration_days;
                        OnCaravanArrived?.Invoke(c);
                        _log.Info($"[Caravan] {c.manifest_id} arrived at shelter on day {day}.");
                    }
                }
                else if (c.status == CaravanStatus.Arrived)
                {
                    if (day >= c.departure_day)
                    {
                        c.status = CaravanStatus.Departed;
                        OnCaravanDeparted?.Invoke(c);
                        _log.Info($"[Caravan] {c.manifest_id} departed on day {day}.");
                    }
                }
            }

            // 2. Schedule recurring seasonal caravans
            for (int i = 0; i < _routes.Count; i++)
            {
                var r = _routes[i];
                if (day >= r.season_start_day && day <= r.season_end_day)
                {
                    if ((day % r.arrival_interval_days) == 0 && FindActiveCaravanForRoute(r.route_id) == null)
                    {
                        ScheduleCaravan(r.route_id, day);
                    }
                }
            }
        }

        private void ResolveHazard(CaravanManifestState c, CaravanRouteDefinition? route)
        {
            c.hazard_resolved = true;
            if (route == null)
            {
                c.hazard_outcome = CaravanHazardOutcome.Safe;
                return;
            }

            float rawRisk = route.base_risk_permille * route.weather_risk_multiplier * route.bandit_risk_multiplier;
            float netRisk = Math.Max(10f, rawRisk - (c.escort_guard_strength * 15f));
            double roll = _rng.NextDouble() * 1000.0;

            if (roll < netRisk * 0.15) // Severe catastrophe
            {
                c.status = CaravanStatus.Lost;
                c.hazard_outcome = CaravanHazardOutcome.Lost;
                _log.Warn($"[Caravan] {c.manifest_id} was lost in transit!");
            }
            else if (roll < netRisk * 0.5) // Stock loss
            {
                c.hazard_outcome = CaravanHazardOutcome.StockLoss;
                var keys = new List<string>(c.stocks.Keys);
                foreach (var k in keys)
                {
                    c.stocks[k] = Math.Max(1, c.stocks[k] / 2);
                }
                _log.Warn($"[Caravan] {c.manifest_id} suffered stock loss from travel hazards.");
            }
            else if (roll < netRisk) // Delayed
            {
                c.hazard_outcome = CaravanHazardOutcome.Delayed;
                c.expected_arrival_day += 2;
                c.departure_day += 2;
                _log.Info($"[Caravan] {c.manifest_id} delayed by 2 days due to route hazards.");
            }
            else
            {
                c.hazard_outcome = CaravanHazardOutcome.Safe;
            }

            OnCaravanHazardResolved?.Invoke(c, c.hazard_outcome);
        }

        private Func<string, float>? _itemValueResolver;
        public void SetItemValueResolver(Func<string, float>? resolver) => _itemValueResolver = resolver;

        /// <summary>Plan VIII · Task 21.4 — optional treaty price-relief provider:
        /// factionId → active discount fraction (e.g. 0.10 = −10% buy price) while
        /// a trade pact with that faction is ratified. Derived on every read by the
        /// provider (typically RegionalTreatySystem.GetTradeDiscount), so nothing is
        /// granted-and-persisted here and save/restore can never double-apply it.</summary>
        private Func<string, float>? _treatyPriceReliefProvider;
        public void SetTreatyPriceReliefProvider(Func<string, float>? provider) =>
            _treatyPriceReliefProvider = provider;

        /// <summary>Active treaty discount for a caravan's faction (clamped 0..0.5),
        /// exposed so the UI can explain "why this price".</summary>
        public float GetTreatyPriceRelief(string factionId) =>
            Math.Clamp(_treatyPriceReliefProvider?.Invoke(factionId) ?? 0f, 0f, 0.5f);

        private float GetCanonicalItemValue(string itemId)
        {
            if (_itemValueResolver != null)
            {
                float custom = _itemValueResolver(itemId);
                if (custom > 0f) return custom;
            }

            return itemId switch
            {
                "anesthetic_ether" => 25f,
                "sterile_gauze" => 6f,
                "surgical_scalpel" => 15f,
                "chemical_filter" => 20f,
                "electrical_wire" => 8f,
                "copper_fuse" => 12f,
                "sandbags" => 5f,
                "flare_tripwire" => 14f,
                "clean_water" => 4f,
                "scrap_metal" => 3f,
                "scrap_wood" => 2f,
                "machine_oil" => 18f,
                "ammo_9x19" => 8f,
                "ammo_556" => 12f,
                _ => 10f
            };
        }

        public CaravanTradeNetworkSave CaptureState()
        {
            var save = new CaravanTradeNetworkSave
            {
                systemId = SystemId,
                schema_version = 1,
                last_tick_day = _state.last_tick_day,
                faction_profitable_trades = new Dictionary<string, int>(_state.faction_profitable_trades, StringComparer.Ordinal),
                favored_factions = new List<string>(_state.favored_factions)
            };

            foreach (var c in _state.caravans)
            {
                save.caravans.Add(new CaravanManifestState
                {
                    manifest_id = c.manifest_id,
                    route_id = c.route_id,
                    faction_id = c.faction_id,
                    status = c.status,
                    departure_origin_day = c.departure_origin_day,
                    expected_arrival_day = c.expected_arrival_day,
                    actual_arrival_day = c.actual_arrival_day,
                    departure_day = c.departure_day,
                    transit_progress_days = c.transit_progress_days,
                    escort_guard_strength = c.escort_guard_strength,
                    hazard_resolved = c.hazard_resolved,
                    hazard_outcome = c.hazard_outcome,
                    stay_duration_days = c.stay_duration_days,
                    stocks = new Dictionary<string, int>(c.stocks, StringComparer.Ordinal)
                });
            }

            return save;
        }

        public void RestoreState(CaravanTradeNetworkSave? save)
        {
            if (save == null) return;
            _state.last_tick_day = save.last_tick_day;
            _state.faction_profitable_trades = new Dictionary<string, int>(save.faction_profitable_trades ?? new(), StringComparer.Ordinal);
            _state.favored_factions = new List<string>(save.favored_factions ?? new());
            _state.caravans.Clear();

            if (save.caravans != null)
            {
                foreach (var c in save.caravans)
                {
                    _state.caravans.Add(new CaravanManifestState
                    {
                        manifest_id = c.manifest_id,
                        route_id = c.route_id,
                        faction_id = c.faction_id,
                        status = c.status,
                        departure_origin_day = c.departure_origin_day,
                        expected_arrival_day = c.expected_arrival_day,
                        actual_arrival_day = c.actual_arrival_day,
                        departure_day = c.departure_day,
                        transit_progress_days = c.transit_progress_days,
                        escort_guard_strength = c.escort_guard_strength,
                        hazard_resolved = c.hazard_resolved,
                        hazard_outcome = c.hazard_outcome,
                        stay_duration_days = c.stay_duration_days,
                        stocks = new Dictionary<string, int>(c.stocks ?? new(), StringComparer.Ordinal)
                    });
                }
            }
        }
    }
}
