// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Economy
{
    [Serializable]
    public sealed class CaravanStockItem
    {
        public string item_id { get; set; } = string.Empty;
        public int quantity { get; set; } = 1;
        public int price_multiplier_bp { get; set; } = 10000; // 10000 = 1.0x

        /// <summary>
        /// Plan 147 — optional campaign-day gate: the item only enters
        /// <c>remainingStock</c> when the restock (arrival) happens on/after
        /// this day. 0 = always available. Real campaign state (day/phase),
        /// evaluated once per restock; stock stays pinned for the stay —
        /// no reroll by reopening a screen.
        /// </summary>
        public int available_from_day { get; set; } = 0;
    }

    [Serializable]
    public sealed class MerchantCaravanDef
    {
        public string caravan_id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string faction_id { get; set; } = string.Empty;
        public int schedule_period_days { get; set; } = 14;
        public int stay_duration_days { get; set; } = 2;
        public int barter_tolerance_bp { get; set; } = 10000;
        public int counterfeit_risk_bp { get; set; } = 500;

        /// <summary>
        /// Plan 147 — when true, this caravan values goods it trades by the
        /// canonical item <c>tradeValue</c> (via the injected item lookup)
        /// instead of the legacy base-value table. Used by specialist
        /// merchants (e.g. the contraband broker) so no second pricing
        /// authority drifts away from the item definitions.
        /// </summary>
        public bool use_canonical_item_values { get; set; } = false;

        public List<CaravanStockItem> stock { get; set; } = new List<CaravanStockItem>();
        public List<string> demanded_item_tags { get; set; } = new List<string>();
        public int demanded_price_mult_bp { get; set; } = 12500;
    }

    [Serializable]
    public sealed class MerchantCaravanCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<MerchantCaravanDef> caravans { get; set; } = new List<MerchantCaravanDef>();
    }

    [Serializable]
    public sealed class CaravanRuntimeState
    {
        public string caravanId = string.Empty;
        public bool isAtAirlock;
        public int daysPresent;
        public Dictionary<string, int> remainingStock = new Dictionary<string, int>(StringComparer.Ordinal);
    }

    [Serializable]
    public sealed class ShelterBarterSaveState
    {
        public int currentDay;
        public int completedTradesCount;
        public int counterfeitsDetectedCount;
        public Dictionary<string, CaravanRuntimeState> caravans = new Dictionary<string, CaravanRuntimeState>(StringComparer.Ordinal);
    }

    public sealed class ShelterBarterSystem
    {
        public const string SystemId = "shelter_barter";
        public const string CatalogPath = "merchant_caravans.json";
        public const int BasisPointsScale = 10000;

        private readonly ISeededRng _rng;
        private readonly InventoryContainer _inventory;
        private readonly ShelterThermalSystem? _thermalSystem;
        private readonly ILog _log;
        private readonly Func<string, ItemDefinition?>? _itemLookup;

        private readonly Dictionary<string, MerchantCaravanDef> _catalog = new Dictionary<string, MerchantCaravanDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, float> _itemBaseValues = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
        {
            { "item_blowtorch", 45f },
            { "item_scrap_metal", 2f },
            { "scrap_metal", 2f },
            { "scrap_mechanical", 2f },
            { "item_fuel", 10f },
            { "fuel", 10f },
            { "item_canned_food", 15f },
            { "canned_food", 15f },
            { "clean_water", 8f },
            { "item_clean_water", 8f },
            { "cloth", 5f },
            { "plastic_material", 6f },
            { "chemicals", 8f },
            { "item_first_aid_kit", 30f },
            { "first_aid", 30f },
            { "item_manual_field_medicine", 65f },
            { "item_manual_rough_repairs", 55f },
            { "item_manual_generator_maintenance", 65f },
            { "item_manual_seismology", 70f },
            { "item_rad_away", 25f },
            { "item_ammo_9mm", 3f },
            { "ammo_9mm", 3f },
            { "duct_tape", 4f }
        };

        private ShelterBarterSaveState _state = new ShelterBarterSaveState();
        private bool _isSevereWinterWeather;

        public IReadOnlyDictionary<string, MerchantCaravanDef> Catalog => _catalog;
        public ShelterBarterSaveState State => _state;
        public bool IsSevereWinterWeather
        {
            get => _isSevereWinterWeather;
            set => _isSevereWinterWeather = value;
        }

        public event Action<MerchantCaravanDef>? OnCaravanArrived;
        public event Action<MerchantCaravanDef>? OnCaravanDeparted;
        public event Action? OnBarterStateChanged;

        public ShelterBarterSystem(
            ISeededRng rng,
            InventoryContainer inventory,
            ShelterThermalSystem? thermalSystem = null,
            ILog? log = null,
            Func<string, ItemDefinition?>? itemLookup = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _thermalSystem = thermalSystem;
            _log = log ?? NullLog.Instance;
            _itemLookup = itemLookup;

            RegisterDefaultCaravans();
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;
            try
            {
                var serializer = new SystemTextJsonSerializer();
                var cat = serializer.Deserialize<MerchantCaravanCatalog>(jsonContent);
                if (cat?.caravans != null)
                {
                    foreach (var c in cat.caravans)
                        RegisterCaravan(c);
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[ShelterBarter] Failed to parse caravan catalog: {ex.Message}");
            }
        }

        public void RegisterCaravan(MerchantCaravanDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.caravan_id)) return;
            _catalog[def.caravan_id] = def;
            EnsureCaravanState(def);
        }

        private void RegisterDefaultCaravans()
        {
            RegisterCaravan(new MerchantCaravanDef
            {
                caravan_id = "caravan_scrap_salvagers",
                name = "Rust-Valley Scrap Guild",
                description = "Hardened mechanics and junk haulers.",
                faction_id = "faction_salvagers",
                schedule_period_days = 14,
                stay_duration_days = 3,
                barter_tolerance_bp = 9500,
                counterfeit_risk_bp = 600,
                stock = new List<CaravanStockItem>
                {
                    new CaravanStockItem { item_id = "item_blowtorch", quantity = 1, price_multiplier_bp = 10000 },
                    new CaravanStockItem { item_id = "item_scrap_metal", quantity = 50, price_multiplier_bp = 9000 },
                    new CaravanStockItem { item_id = "item_fuel", quantity = 20, price_multiplier_bp = 11000 }
                },
                demanded_item_tags = new List<string> { "medical", "food", "clean_water" },
                demanded_price_mult_bp = 12500
            });
            RegisterCaravan(new MerchantCaravanDef
            {
                caravan_id = "caravan_permafrost_traders",
                name = "Deep-Tundra Fur & Ration Convoy",
                description = "Snow sled team trading rations and materials.",
                faction_id = "faction_permafrost_nomads",
                schedule_period_days = 20,
                stay_duration_days = 2,
                barter_tolerance_bp = 10000,
                counterfeit_risk_bp = 400,
                stock = new List<CaravanStockItem>
                {
                    new CaravanStockItem { item_id = "item_canned_food", quantity = 30, price_multiplier_bp = 10000 },
                    new CaravanStockItem { item_id = "cloth", quantity = 25, price_multiplier_bp = 9500 },
                    new CaravanStockItem { item_id = "plastic_material", quantity = 15, price_multiplier_bp = 10500 }
                },
                demanded_item_tags = new List<string> { "fuel", "ammunition", "tools" },
                demanded_price_mult_bp = 13000
            });
            RegisterCaravan(new MerchantCaravanDef
            {
                caravan_id = "caravan_medic_syndicate",
                name = "Black Cross Apothecary",
                description = "Traveling medics offering sterile antibiotics.",
                faction_id = "faction_black_cross",
                schedule_period_days = 25,
                stay_duration_days = 2,
                barter_tolerance_bp = 10500,
                counterfeit_risk_bp = 200,
                stock = new List<CaravanStockItem>
                {
                    new CaravanStockItem { item_id = "item_first_aid_kit", quantity = 8, price_multiplier_bp = 10000 },
                    new CaravanStockItem { item_id = "item_manual_field_medicine", quantity = 1, price_multiplier_bp = 12000 },
                    new CaravanStockItem { item_id = "item_rad_away", quantity = 10, price_multiplier_bp = 11500 }
                },
                demanded_item_tags = new List<string> { "clean_water", "chemicals" },
                demanded_price_mult_bp = 14000
            });
            RegisterCaravan(new MerchantCaravanDef
            {
                caravan_id = "caravan_black_market_munitions",
                name = "Iron-Sights Ordnance Runners",
                description = "Border smugglers dealing in surplus ammo.",
                faction_id = "faction_wasteland_outlaws",
                schedule_period_days = 18,
                stay_duration_days = 2,
                barter_tolerance_bp = 9000,
                counterfeit_risk_bp = 2500,
                stock = new List<CaravanStockItem>
                {
                    new CaravanStockItem { item_id = "item_ammo_9mm", quantity = 120, price_multiplier_bp = 9500 },
                    new CaravanStockItem { item_id = "item_manual_rough_repairs", quantity = 1, price_multiplier_bp = 8500 },
                    new CaravanStockItem { item_id = "item_fuel", quantity = 15, price_multiplier_bp = 11000 }
                },
                demanded_item_tags = new List<string> { "medical", "food" },
                demanded_price_mult_bp = 12000
            });
        }

        private CaravanRuntimeState EnsureCaravanState(MerchantCaravanDef def)
        {
            if (!_state.caravans.TryGetValue(def.caravan_id, out var cState))
            {
                cState = new CaravanRuntimeState { caravanId = def.caravan_id };
                RestockCaravan(def, cState);
                _state.caravans[def.caravan_id] = cState;
            }
            return cState;
        }

        private void RestockCaravan(MerchantCaravanDef def, CaravanRuntimeState cState)
        {
            cState.remainingStock.Clear();
            foreach (var item in def.stock)
            {
                // Plan 147: per-arrival day gate — high-tier stock only enters
                // the manifest from its gate day (real campaign state). Stock is
                // then pinned for the whole stay; no reroll by reopening.
                int qty = item.quantity;
                if (item.available_from_day > 0 && _state.currentDay < item.available_from_day)
                    qty = 0;
                cState.remainingStock[item.item_id] = qty;
            }
        }

        public bool IsAirlockAccessible()
        {
            if (_thermalSystem == null) return true;
            var room = _thermalSystem.State.rooms.Find(r => r.roomId == "room_airlock");
            if (room == null) return true;
            return !room.isFrozen;
        }

        public float GetBaseItemValue(string itemId)
            => ResolveBaseValue(itemId, caravan: null);

        /// <summary>
        /// Base value resolution order: legacy base-value table, then — for
        /// caravans flagged <c>use_canonical_item_values</c> (Plan 147) — the
        /// canonical item <c>tradeValue</c>, then the neutral fallback. The
        /// table first keeps every existing caravan price byte-identical.
        /// </summary>
        private float ResolveBaseValue(string itemId, MerchantCaravanDef? caravan)
        {
            string canon = ItemAliases.ToCanonical(itemId);
            if (_itemBaseValues.TryGetValue(itemId, out float val)) return val;
            if (_itemBaseValues.TryGetValue(canon, out val)) return val;
            if (caravan != null && caravan.use_canonical_item_values && _itemLookup != null)
            {
                var def = _itemLookup(itemId);
                if (def != null && def.tradeValue > 0f) return def.tradeValue;
            }
            return 5f; // reasonable neutral fallback
        }

        public float CalculatePlayerOfferValue(MerchantCaravanDef caravan, IReadOnlyDictionary<string, int> offerItems)
        {
            float total = 0f;
            foreach (var kvp in offerItems)
            {
                string itemId = kvp.Key;
                int count = kvp.Value;
                if (count <= 0) continue;

                float baseVal = ResolveBaseValue(itemId, caravan);
                float itemTotal = baseVal * count;

                // Demand premium
                bool isDemanded = false;
                foreach (var tag in caravan.demanded_item_tags)
                {
                    if (itemId.Contains(tag, StringComparison.OrdinalIgnoreCase))
                    {
                        isDemanded = true;
                        break;
                    }
                }
                if (isDemanded)
                {
                    itemTotal *= (caravan.demanded_price_mult_bp / (float)BasisPointsScale);
                }

                // Weather scarcity premium (fuel and food in winter)
                if (_isSevereWinterWeather && (itemId.Contains("fuel", StringComparison.OrdinalIgnoreCase) || itemId.Contains("food", StringComparison.OrdinalIgnoreCase)))
                {
                    itemTotal *= 1.30f;
                }

                total += itemTotal;
            }
            return total;
        }

        public float CalculateCaravanStockCost(MerchantCaravanDef caravan, IReadOnlyDictionary<string, int> requestedItems)
        {
            float total = 0f;
            foreach (var kvp in requestedItems)
            {
                string itemId = kvp.Key;
                int count = kvp.Value;
                if (count <= 0) continue;

                float baseVal = ResolveBaseValue(itemId, caravan);
                var stockItem = caravan.stock.Find(s => s.item_id == itemId);
                int multBp = stockItem?.price_multiplier_bp ?? BasisPointsScale;

                float unitPrice = baseVal * (multBp / (float)BasisPointsScale);
                total += unitPrice * count;
            }
            return total;
        }

        public ActionResult ExecuteTrade(
            string caravanId,
            IReadOnlyDictionary<string, int> playerOffers,
            IReadOnlyDictionary<string, int> playerRequests,
            int playerAppraisalSkillLevel = 0)
        {
            if (!_catalog.TryGetValue(caravanId, out var caravan))
                return ActionResult.Failed("unknown_caravan", "barter.unknown_caravan");

            var cState = EnsureCaravanState(caravan);
            if (!cState.isAtAirlock)
                return ActionResult.Blocked("caravan_not_at_airlock", "barter.caravan_not_at_airlock");

            if (!IsAirlockAccessible())
                return ActionResult.Blocked("airlock_inaccessible", "barter.airlock_inaccessible");

            if (playerRequests == null || playerRequests.Count == 0)
                return ActionResult.Blocked("no_items_requested", "barter.no_items_requested");

            // 1. Verify caravan has sufficient stock
            foreach (var req in playerRequests)
            {
                if (!cState.remainingStock.TryGetValue(req.Key, out int avail) || avail < req.Value)
                    return ActionResult.Blocked("insufficient_caravan_stock", "barter.insufficient_caravan_stock");
            }

            // 2. Valuation Check with Basis-Point Tolerance
            float playerOfferVal = CalculatePlayerOfferValue(caravan, playerOffers);
            float caravanRequiredVal = CalculateCaravanStockCost(caravan, playerRequests);

            long scaledOffer = (long)(playerOfferVal * BasisPointsScale);
            long scaledRequiredWithTolerance = (long)(caravanRequiredVal * caravan.barter_tolerance_bp);

            if (scaledOffer < scaledRequiredWithTolerance)
                return ActionResult.Blocked("insufficient_value", "barter.insufficient_value");

            // 3. Counterfeit Risk Check
            if (caravan.counterfeit_risk_bp > 0 && _rng.Next(0, BasisPointsScale) < caravan.counterfeit_risk_bp)
            {
                // Roll vs player appraisal skill
                if (playerAppraisalSkillLevel >= 2)
                {
                    _state.counterfeitsDetectedCount++;
                    OnBarterStateChanged?.Invoke();
                    return ActionResult.Blocked("counterfeit_detected", "barter.counterfeit_detected");
                }
            }

            // 4. Build InventoryBill for atomic verification & commit
            var bill = new InventoryBill();
            foreach (var off in playerOffers)
            {
                if (off.Value > 0)
                {
                    string canonCost = ItemAliases.ToCanonical(off.Key);
                    bill.AddCost(canonCost, off.Value);
                }
            }
            foreach (var req in playerRequests)
            {
                if (req.Value > 0)
                {
                    string canonGrant = ItemAliases.ToCanonical(req.Key);
                    bill.AddGrant(canonGrant, req.Value);
                }
            }

            // 5. Pre-commit validation against inventory capacity and constraints
            var validation = _inventory.ValidateTransaction(bill);
            if (!validation.IsValid)
            {
                return ActionResult.Blocked(validation.FailureReason ?? "storage_capacity_exceeded", "barter.storage_capacity_exceeded");
            }

            // 6. Atomic Execution
            bool committed = _inventory.TryExecuteTransaction(bill);
            if (!committed)
            {
                return ActionResult.Failed("transaction_commit_failed", "barter.transaction_commit_failed");
            }

            // 7. Deduct caravan stock
            foreach (var req in playerRequests)
            {
                cState.remainingStock[req.Key] -= req.Value;
            }

            _state.completedTradesCount++;
            OnBarterStateChanged?.Invoke();

            return ActionResult.Success("barter.trade_completed",
                new Dictionary<string, double>
                {
                    { "offer_value", playerOfferVal },
                    { "required_value", caravanRequiredVal },
                    { "completed_trades", _state.completedTradesCount }
                });
        }

        public void TickDay(int day)
        {
            _state.currentDay = day;

            foreach (var kvp in _catalog)
            {
                var def = kvp.Value;
                var cState = EnsureCaravanState(def);

                bool shouldBePresent = (day % def.schedule_period_days) < def.stay_duration_days;
                if (shouldBePresent && !cState.isAtAirlock)
                {
                    cState.isAtAirlock = true;
                    cState.daysPresent = 1;
                    RestockCaravan(def, cState);
                    OnCaravanArrived?.Invoke(def);
                }
                else if (shouldBePresent && cState.isAtAirlock)
                {
                    cState.daysPresent++;
                }
                else if (!shouldBePresent && cState.isAtAirlock)
                {
                    cState.isAtAirlock = false;
                    cState.daysPresent = 0;
                    OnCaravanDeparted?.Invoke(def);
                }
            }

            OnBarterStateChanged?.Invoke();
        }

        public ShelterBarterSaveState CaptureState() => CloneState(_state);

        public void RestoreState(ShelterBarterSaveState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static ShelterBarterSaveState CloneState(ShelterBarterSaveState src)
        {
            if (src == null) return new ShelterBarterSaveState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<ShelterBarterSaveState>(json) ?? new ShelterBarterSaveState();
        }
    }
}
