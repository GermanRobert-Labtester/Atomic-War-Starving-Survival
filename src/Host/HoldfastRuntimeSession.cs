using System;
#pragma warning disable CS8618
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Godot's playable Holdfast boundary. The world session owns existing
    /// Holdfast systems; the Core trade session owns mutable inventory/value/stock.
    ///
    /// Single-source-of-truth: Survival mechanics (Health/Hunger/Thirst/Radiation)
    /// project directly from Ashfall.Core.Survivors.NeedsSystem and RadiationSystem
    /// via the bound SurvivorsHostSession.
    /// </summary>
    public sealed class HoldfastRuntimeSession
    {
        public const long DefaultStartingValue = 100;
        public const int MaxHealth = 100;
        public const int MaxHunger = 100;
        public const int MaxThirst = 100;
        public const float RadDamageThreshold = 50f; // mSv/day causes HP loss
        public const float StarvationThreshold = 90f; // hunger above this causes HP loss
        public const float DehydrationThreshold = 90f; // thirst above this causes HP loss

        public CoreDemoSession World { get; }
        public HoldfastTradeSession Trade { get; }
        public HoldfastCatalog Catalog => World.Catalog;
        public string LastPersistenceMessage { get; private set; } = string.Empty;
        public bool HasPurchasedThisSession { get; set; }

        // ── Authoritative Cohort / Player Binding ────────────────────
        private SurvivorsHostSession? _survivors;
        public SurvivorsHostSession? Survivors
        {
            get => _survivors;
            set
            {
                _survivors = value;
                WireInventorySession();
            }
        }
        public string PlayerSurvivorId { get; set; } = "survivor_dr_sarah_chen";

        private InventoryHostSession? _inventorySession;
        public InventoryHostSession? InventorySession
        {
            get => _inventorySession;
            set
            {
                _inventorySession = value;
                WireInventorySession();
            }
        }

        public Ashfall.Core.Inventory.Inventory? Inventory { get; set; }

        public Ashfall.Core.Inventory.Inventory? EffectiveInventory =>
            _inventorySession?.Inventory ?? Inventory ?? Trade.PlayerInventory;

        // ── Fallback survival state (for headless/standalone tests) ──
        private int _fallbackHealth = MaxHealth;
        private int _fallbackHunger = 0;
        private int _fallbackThirst = 0;
        private float _fallbackRadiation = 0f;

        // ── Survival state projections ───────────────────────────────
        public int Health => Survivors?.Find(PlayerSurvivorId) != null
            ? (int)Math.Clamp(Survivors.Find(PlayerSurvivorId)!.Health, 0f, (float)MaxHealth)
            : _fallbackHealth;

        public float Radiation => Survivors != null
            ? (Survivors.RadStateFor(PlayerSurvivorId)?.RadiationDose ?? 0f)
            : _fallbackRadiation;

        public int Hunger => Survivors?.Find(PlayerSurvivorId) != null
            ? (int)Math.Clamp(Survivors.Find(PlayerSurvivorId)!.Hunger, 0f, (float)MaxHunger)
            : _fallbackHunger;

        public int Thirst => Survivors?.Find(PlayerSurvivorId) != null
            ? (int)Math.Clamp(Survivors.Find(PlayerSurvivorId)!.Thirst, 0f, (float)MaxThirst)
            : _fallbackThirst;

        // Day is a live projection of the shared campaign clock (World.Clock),
        // not an independent counter — HoldfastRuntimeSession itself has no
        // persisted save state (only its Trade sub-session does), so an
        // independently-incremented field silently reset to 1 on every
        // Continue while World.Clock.Day (the real, persisted campaign day)
        // kept its correct value. Projecting here means the HUD/dashboard/
        // death-and-victory stats and MarkActiveSlotTerminal all agree with
        // the same single day authority across a save/reload round-trip.
        public int Day => World.Clock.Day;
        public bool IsDead => Health <= 0;
        public string DeathCause { get; private set; } = string.Empty;

        // ── Quest / Win state ────────────────────────────────────────
        public bool IsGameWon => World.Quests != null && World.Quests.IsCompleted(HoldfastQuestSystem.Hatch);
        public string WinMessage { get; private set; } = string.Empty;

        public event Action StateChanged;
        public event Action<string> OnPlayerDied; // passes cause of death
        public event Action<string> OnGameWon; // passes win message

        public HoldfastRuntimeSession(CoreDemoSession world, long startingValue = DefaultStartingValue, Ashfall.Core.Inventory.Inventory? inventory = null)
        {
            World = world ?? throw new ArgumentNullException(nameof(world));
            Inventory = inventory;
            Trade = new HoldfastTradeSession(World.Catalog, startingValue, inventory);
            Trade.StateChanged += () => StateChanged?.Invoke();
        }

        public static HoldfastRuntimeSession Create(
            CoreDemoSession world,
            bool seedDevelopmentState = false,
            bool loadTradeSave = true,
            Ashfall.Core.Inventory.Inventory? inventory = null)
        {
            var session = new HoldfastRuntimeSession(world, DefaultStartingValue, inventory);
            if (loadTradeSave)
            {
                var saved = HoldfastTradeSaveStore.TryLoad();
                if (saved != null)
                {
                    if (!session.Trade.TryRestoreState(saved, out string error))
                        session.LastPersistenceMessage = "Holdfast trade save rejected: " + error;
                    else
                        session.LastPersistenceMessage = "Holdfast player and store state restored.";
                }
                else if (seedDevelopmentState)
                {
                    session.SeedDevelopmentState();
                }
            }
            else if (seedDevelopmentState)
            {
                session.SeedDevelopmentState();
            }
            return session;
        }

        [Obsolete("Migrate to aggregate campaign persistence.")]
        public bool TrySave(string basePathOverride = null!, string tradePathOverride = null!)
        {
            bool baseSaved = HoldfastSaveStore.TrySave(World.CaptureSave(), basePathOverride);
            bool tradeSaved = HoldfastTradeSaveStore.TrySave(Trade.CaptureState(), tradePathOverride);
            bool saved = baseSaved && tradeSaved;
            LastPersistenceMessage = saved
                ? "Holdfast player, store, and world state saved."
                : "Holdfast save failed; existing state remains in memory.";
            return saved;
        }

        [Obsolete("Migrate to aggregate campaign persistence.")]
        public bool TryReload(string basePathOverride = null!, string tradePathOverride = null!)
        {
            var worldSnapshot = World.CaptureSave();
            var tradeSnapshot = Trade.CaptureState();

            var baseSave = HoldfastSaveStore.TryLoad(basePathOverride);
            var tradeSave = HoldfastTradeSaveStore.TryLoad(tradePathOverride);

            if (baseSave == null && tradeSave == null)
            {
                LastPersistenceMessage = "No Holdfast save was available to reload.";
                return false;
            }

            try
            {
                if (baseSave != null)
                {
                    World.RestoreSave(baseSave);
                }

                if (tradeSave != null)
                {
                    if (!Trade.TryRestoreState(tradeSave, out string error))
                    {
                        // Rollback on partial or corrupted trade load
                        World.RestoreSave(worldSnapshot);
                        Trade.TryRestoreState(tradeSnapshot, out _);
                        LastPersistenceMessage = "Holdfast trade reload rejected: " + error;
                        return false;
                    }
                }

                LastPersistenceMessage = "Holdfast state reloaded from disk.";
                return true;
            }
            catch (Exception ex)
            {
                // Rollback on any failure
                World.RestoreSave(worldSnapshot);
                Trade.TryRestoreState(tradeSnapshot, out _);
                LastPersistenceMessage = "Holdfast reload failed: " + ex.Message;
                return false;
            }
        }

        public void SeedDevelopmentState()
        {
            Trade.SeedInventory("item_triplicate_carbon", 1);
        }

        // ── Survival mechanics ────────────────────────────────────────

        /// <summary>
        /// Advance one day. Advances quest progress and checks game over / win conditions.
        /// Simulation decay (hunger, thirst, radiation, health loss) is driven authoritatively
        /// by Core NeedsSystem and RadiationSystem (ticked via SurvivorsHostSession.TickHour).
        /// </summary>
        public string TickDay()
        {
            if (IsDead) return "The ledger is closed. No more days to count.";

            // Day is now a live projection of World.Clock.Day (see the Day
            // property above). The campaign's HoldfastCoreDayOwner always
            // calls World.TickDay() (which itself advances World.Clock)
            // immediately before this method, so the clock has already
            // moved — advancing it again here would double-increment.

            // Fallback decay when running in isolated test harnesses without SurvivorsHostSession
            if (Survivors == null)
            {
                _fallbackHunger = Math.Min(MaxHunger, _fallbackHunger + 8);
                _fallbackThirst = Math.Min(MaxThirst, _fallbackThirst + 10);
                if (_fallbackRadiation > 0)
                    _fallbackRadiation = Math.Max(0, _fallbackRadiation - _fallbackRadiation * 0.07f);

                int hpLoss = 0;
                if (_fallbackHunger >= StarvationThreshold)
                    hpLoss += (int)((_fallbackHunger - StarvationThreshold) * 0.5f);
                if (_fallbackThirst >= DehydrationThreshold)
                    hpLoss += (int)((_fallbackThirst - DehydrationThreshold) * 0.6f);
                if (_fallbackRadiation >= RadDamageThreshold)
                    hpLoss += (int)((_fallbackRadiation - RadDamageThreshold) * 0.1f);

                _fallbackHealth = Math.Max(0, _fallbackHealth - hpLoss);
            }

            // Advance quests
            if (World.Quests != null)
            {
                // Check if player has items that unlock quest story gates
                bool hasMapItem = Trade.GetHeld("item_map_sheet_ice_road") > 0;
                World.Quests.TickDaily(Day, hasMapItem, false, false);

                // Advance quest if player is at a location
                World.AdvanceQuest();
            }

            // Check death
            if (Health <= 0)
            {
                DeathCause = DetermineDeathCause();
                OnPlayerDied?.Invoke(DeathCause);
                return $"Day {Day}. {DeathCause}";
            }

            // Check win condition
            if (IsGameWon)
            {
                WinMessage = $"The hatch is open. Day {Day}. The Holdfast endures.";
                OnGameWon?.Invoke(WinMessage);
                return WinMessage;
            }

            StateChanged?.Invoke();
            return $"Day {Day}. HP:{Health} Hunger:{Hunger} Thirst:{Thirst} Rad:{Radiation:F0}mSv.";
        }

        private void WireInventorySession()
        {
            if (_inventorySession == null) return;
            if (Survivors != null)
            {
                _inventorySession.Survivors = Survivors;
            }
            else
            {
                _inventorySession.ApplyNeedOverride = (survivorId, needType, delta) =>
                {
                    switch (needType)
                    {
                        case ItemType.Food:
                            _fallbackHunger = Math.Max(0, (int)(_fallbackHunger + delta));
                            break;
                        case ItemType.Water:
                            _fallbackThirst = Math.Max(0, (int)(_fallbackThirst + delta));
                            break;
                        case ItemType.Medical:
                            _fallbackHealth = Math.Min(MaxHealth, Math.Max(0, (int)(_fallbackHealth + delta)));
                            break;
                    }
                    return true;
                };
                _inventorySession.ApplyRadCleanseOverride = (survivorId, rads) =>
                {
                    _fallbackRadiation = Math.Max(0f, _fallbackRadiation - rads);
                };
                _inventorySession.ApplyContaminationOverride = (survivorId, dose) =>
                {
                    _fallbackRadiation += dose;
                };
            }
        }

        private InventoryHostSession? GetOrCreateInventorySession()
        {
            if (_inventorySession != null)
            {
                WireInventorySession();
                return _inventorySession;
            }

            var inv = EffectiveInventory;
            if (inv != null)
            {
                _inventorySession = new InventoryHostSession(inv);
                WireInventorySession();
                return _inventorySession;
            }

            return null;
        }

        /// <summary>
        /// Consume food items from inventory to reduce hunger.
        /// Returns true if food was consumed.
        /// </summary>
        public bool ConsumeFood(string itemId, int amount = 1)
        {
            return ConsumeFoodResult(itemId, amount).IsSuccess;
        }

        public ActionResult ConsumeFoodResult(string itemId, int amount = 1, string? survivorId = null)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0)
                return ActionResult.Blocked("invalid_args", "Invalid item or amount.");

            string targetSurvivor = survivorId ?? PlayerSurvivorId;
            var session = GetOrCreateInventorySession();
            if (session != null)
            {
                int held = session.Inventory.CountById(itemId);
                if (held < amount)
                    return ActionResult.Blocked("insufficient_inventory", $"Insufficient {itemId} in inventory ({held}/{amount}).");

                var aggregatedDeltas = new Dictionary<string, double>(StringComparer.Ordinal);
                for (int i = 0; i < amount; i++)
                {
                    var r = session.ConsumeResult(itemId, targetSurvivor);
                    if (!r.IsSuccess) return r;
                    foreach (var kv in r.Deltas)
                    {
                        aggregatedDeltas.TryGetValue(kv.Key, out double cur);
                        aggregatedDeltas[kv.Key] = cur + kv.Value;
                    }
                }
                StateChanged?.Invoke();
                return ActionResult.Success($"Ate {amount} × {itemId}.", aggregatedDeltas);
            }

            return FallbackConsume(itemId, amount, targetSurvivor, ItemType.Food);
        }

        /// <summary>
        /// Consume water items from inventory to reduce thirst.
        /// Returns true if water was consumed.
        /// </summary>
        public bool ConsumeWater(string itemId, int amount = 1)
        {
            return ConsumeWaterResult(itemId, amount).IsSuccess;
        }

        public ActionResult ConsumeWaterResult(string itemId, int amount = 1, string? survivorId = null)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0)
                return ActionResult.Blocked("invalid_args", "Invalid item or amount.");

            string targetSurvivor = survivorId ?? PlayerSurvivorId;
            var session = GetOrCreateInventorySession();
            if (session != null)
            {
                int held = session.Inventory.CountById(itemId);
                if (held < amount)
                    return ActionResult.Blocked("insufficient_inventory", $"Insufficient {itemId} in inventory ({held}/{amount}).");

                var aggregatedDeltas = new Dictionary<string, double>(StringComparer.Ordinal);
                for (int i = 0; i < amount; i++)
                {
                    var r = session.ConsumeResult(itemId, targetSurvivor);
                    if (!r.IsSuccess) return r;
                    foreach (var kv in r.Deltas)
                    {
                        aggregatedDeltas.TryGetValue(kv.Key, out double cur);
                        aggregatedDeltas[kv.Key] = cur + kv.Value;
                    }
                }
                StateChanged?.Invoke();
                return ActionResult.Success($"Drank {amount} × {itemId}.", aggregatedDeltas);
            }

            return FallbackConsume(itemId, amount, targetSurvivor, ItemType.Water);
        }

        /// <summary>
        /// Take radiation exposure (from events, locations, etc.).
        /// </summary>
        public void ExposeRadiation(float msv)
        {
            if (msv <= 0f) return;
            if (Survivors != null)
            {
                Survivors.ExposeToZone(PlayerSurvivorId, msv);
            }
            else
            {
                _fallbackRadiation += msv;
            }
            StateChanged?.Invoke();
        }

        /// <summary>
        /// Use anti-rad items to reduce radiation.
        /// </summary>
        public bool UseAntiRad(string itemId, float reduction = 0f)
        {
            return UseAntiRadResult(itemId, reduction: reduction).IsSuccess;
        }

        public ActionResult UseAntiRadResult(string itemId, string? survivorId = null, float reduction = 0f)
        {
            if (string.IsNullOrEmpty(itemId))
                return ActionResult.Blocked("invalid_args", "Invalid item.");

            string targetSurvivor = survivorId ?? PlayerSurvivorId;
            var session = GetOrCreateInventorySession();
            if (session != null)
            {
                int held = session.Inventory.CountById(itemId);
                if (held < 1)
                    return ActionResult.Blocked("insufficient_inventory", $"Insufficient {itemId} in inventory ({held}/1).");

                var r = session.ConsumeResult(itemId, targetSurvivor);
                if (r.IsSuccess)
                    StateChanged?.Invoke();
                return r;
            }

            return FallbackConsume(itemId, 1, targetSurvivor, ItemType.AntiRad, reduction);
        }

        private ActionResult FallbackConsume(string itemId, int amount, string targetSurvivor, ItemType expectedType, float customReduction = 0f)
        {
            int held = Trade.GetHeld(itemId);
            if (held < amount)
                return ActionResult.Blocked("insufficient_inventory", $"Insufficient {itemId} held ({held}/{amount}).");

            Trade.Inventory.RemoveItem(itemId, amount);

            float hungerRestore = 0f;
            float thirstRestore = 0f;
            float healthEffect = 0f;
            float radCleanse = 0f;
            float contamination = 0f;

            string canonical = ItemAliases.ToCanonical(itemId);

            if (canonical == "canned_food") { hungerRestore = 40f; }
            else if (canonical == "crop_wheat") { hungerRestore = 50f; }
            else if (canonical == "crop_tuber") { hungerRestore = 35f; }
            else if (canonical == "crop_grain") { hungerRestore = 30f; }
            else if (canonical == "crop_mushroom") { hungerRestore = 25f; }
            else if (canonical == "clean_water" || canonical == "water_bottle" || canonical == "purified_water") { thirstRestore = 40f; }
            else if (canonical == "irradiated_water") { thirstRestore = 25f; contamination = 0.5f; }
            else if (canonical == "bandage") { healthEffect = 30f; }
            else if (canonical == "anti_rad") { radCleanse = customReduction > 0f ? customReduction : 50f; }
            else if (canonical == "rad_away") { radCleanse = customReduction > 0f ? customReduction : 30f; }
            else if (canonical == "iodine_pills") { radCleanse = customReduction > 0f ? customReduction : 20f; }
            else if (expectedType == ItemType.Food) { hungerRestore = 30f; }
            else if (expectedType == ItemType.Water) { thirstRestore = 35f; }
            else if (expectedType == ItemType.AntiRad) { radCleanse = customReduction > 0f ? customReduction : 20f; }

            var deltas = new Dictionary<string, double>(StringComparer.Ordinal);
            if (hungerRestore > 0f) deltas["hunger"] = -hungerRestore * amount;
            if (thirstRestore > 0f) deltas["thirst"] = -thirstRestore * amount;
            if (healthEffect > 0f) deltas["health"] = healthEffect * amount;
            if (radCleanse > 0f) deltas["rad_cleanse"] = radCleanse * amount;
            if (contamination > 0f) deltas["contamination"] = contamination * amount * Ashfall.Core.Inventory.Inventory.ContaminationDosePerUnit;

            if (Survivors != null)
            {
                if (hungerRestore > 0f) Survivors.Needs.Modify(targetSurvivor, NeedKind.Hunger, -hungerRestore * amount);
                if (thirstRestore > 0f) Survivors.Needs.Modify(targetSurvivor, NeedKind.Thirst, -thirstRestore * amount);
                if (healthEffect > 0f) Survivors.Needs.Modify(targetSurvivor, NeedKind.Health, healthEffect * amount);
                if (radCleanse > 0f) Survivors.AdministerAntiRad(targetSurvivor, radCleanse * amount);
                if (contamination > 0f)
                {
                    var rad = Survivors.RadStateFor(targetSurvivor);
                    if (rad != null) Survivors.Radiation.AdjustDose(rad, contamination * amount * Ashfall.Core.Inventory.Inventory.ContaminationDosePerUnit);
                }
            }
            else
            {
                if (hungerRestore > 0f) _fallbackHunger = Math.Max(0, (int)(_fallbackHunger - hungerRestore * amount));
                if (thirstRestore > 0f) _fallbackThirst = Math.Max(0, (int)(_fallbackThirst - thirstRestore * amount));
                if (healthEffect > 0f) _fallbackHealth = Math.Min(MaxHealth, (int)(_fallbackHealth + healthEffect * amount));
                if (radCleanse > 0f) _fallbackRadiation = Math.Max(0f, _fallbackRadiation - radCleanse * amount);
                if (contamination > 0f) _fallbackRadiation += contamination * amount * Ashfall.Core.Inventory.Inventory.ContaminationDosePerUnit;
            }

            StateChanged?.Invoke();
            return ActionResult.Success($"Consumed {amount} × {itemId}.", deltas);
        }

        public string? FindAvailableFoodItemId()
        {
            var inv = EffectiveInventory;
            if (inv != null)
            {
                for (int i = 0; i < inv.Slots.Count; i++)
                {
                    var slot = inv.Slots[i];
                    if (slot?.Item != null && slot.Amount > 0)
                    {
                        if (slot.Item.type == ItemType.Food ||
                            slot.Item.type == ItemType.ContaminatedFood ||
                            slot.Item.hungerRestore > 0f)
                            return slot.Item.id;
                    }
                }
            }
            string[] preferredFood = { "canned_food", "crop_wheat", "crop_tuber", "crop_grain", "crop_mushroom", "ration_pack", "dried_meat", "mre" };
            foreach (var food in preferredFood)
            {
                if (Trade.GetHeld(food) > 0) return food;
            }
            return null;
        }

        public string? FindAvailableWaterItemId()
        {
            var inv = EffectiveInventory;
            if (inv != null)
            {
                if (inv.CountById("clean_water") > 0) return "clean_water";
                for (int i = 0; i < inv.Slots.Count; i++)
                {
                    var slot = inv.Slots[i];
                    if (slot?.Item != null && slot.Amount > 0)
                    {
                        if (slot.Item.type == ItemType.Water ||
                            slot.Item.type == ItemType.IrradiatedWater ||
                            slot.Item.thirstRestore > 0f)
                            return slot.Item.id;
                    }
                }
            }
            string[] preferredWater = { "clean_water", "water_bottle", "purified_water", "irradiated_water" };
            foreach (var water in preferredWater)
            {
                if (Trade.GetHeld(water) > 0) return water;
            }
            return null;
        }

        public string? FindAvailableAntiRadItemId()
        {
            var inv = EffectiveInventory;
            if (inv != null)
            {
                if (inv.CountById("anti_rad") > 0) return "anti_rad";
                if (inv.CountById("rad_away") > 0) return "rad_away";
                if (inv.CountById("iodine_pills") > 0) return "iodine_pills";
                for (int i = 0; i < inv.Slots.Count; i++)
                {
                    var slot = inv.Slots[i];
                    if (slot?.Item != null && slot.Amount > 0)
                    {
                        if (slot.Item.type == ItemType.AntiRad ||
                            slot.Item.type == ItemType.Iodine ||
                            slot.Item.radCleanse > 0f)
                            return slot.Item.id;
                    }
                }
            }
            string[] preferredAntiRad = { "anti_rad", "rad_away", "iodine_pills" };
            foreach (var item in preferredAntiRad)
            {
                if (Trade.GetHeld(item) > 0) return item;
            }
            return null;
        }

        /// <summary>
        /// Heal health directly.
        /// </summary>
        public void Heal(int amount)
        {
            if (amount <= 0) return;
            if (Survivors != null)
            {
                Survivors.Needs.Modify(PlayerSurvivorId, NeedKind.Health, (float)amount);
            }
            else
            {
                _fallbackHealth = Math.Min(MaxHealth, _fallbackHealth + amount);
            }
            StateChanged?.Invoke();
        }

        // ── Location / Exploration ─────────────────────────────────────

        /// <summary>
        /// Visit a location. Applies radiation exposure based on zone,
        /// advances quests, and consumes resources. Returns a narrative summary.
        /// </summary>
        public string VisitLocation(string locationId)
        {
            if (IsDead) return "The ledger is closed. No more journeys.";

            // Look up location in catalog
            var loc = FindLocation(locationId);

            // Apply radiation from location
            float radExposure = loc?.baseRadsPerHour ?? 4f;

            // Reduce exposure if wearing protective gear
            int maskCount = Trade.GetHeld("gas_mask");
            int hazmatCount = Trade.GetHeld("hazmat_suit");
            if (hazmatCount > 0) radExposure *= 0.3f;
            else if (maskCount > 0) radExposure *= 0.6f;

            ExposeRadiation(radExposure);

            // Advance quest
            bool questAdvanced = false;
            if (World.Quests != null)
            {
                World.AdvanceQuest();
                questAdvanced = true;
            }

            string displayName = loc?.displayName ?? locationId;
            string radNote = radExposure > 0 ? $" Exposure: +{radExposure:F0} mSv." : " No contamination detected.";
            string questNote = questAdvanced ? " Quest progress updated." : "";

            StateChanged?.Invoke();
            return $"Visited {displayName}.{radNote}{questNote}";
        }

        private HoldfastLocationEntry? FindLocation(string id)
        {
            var locs = World.Catalog?.Locations;
            if (locs == null) return null;
            for (int i = 0; i < locs.Count; i++)
                if (locs[i] != null && locs[i].id == id)
                    return locs[i];
            return null;
        }

        // ── Quest Status ───────────────────────────────────────────────

        public string GetQuestSummary()
        {
            if (World.Quests == null) return "No quest system available.";

            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < HoldfastQuestSystem.MainQuestIds.Length; i++)
            {
                string qid = HoldfastQuestSystem.MainQuestIds[i];
                string name = World.Quests.GetDisplayName(qid);
                bool started = World.Quests.IsStarted(qid);
                bool completed = World.Quests.IsCompleted(qid);
                string status = completed ? "[DONE]" : (started ? "[IN PROGRESS]" : "[LOCKED]");
                sb.Append($"{status} {name}\n");
                if (started && !completed)
                {
                    string stageText = World.Quests.GetStageText(qid);
                    if (!string.IsNullOrEmpty(stageText))
                        sb.Append($"  → {stageText}\n");
                }
            }
            return sb.ToString().TrimEnd();
        }

        private string DetermineDeathCause()
        {
            if (Thirst >= MaxThirst)
                return "Dehydration. The water ran out three days ago. The body lasted longer than expected.";
            if (Hunger >= MaxHunger)
                return "Starvation. The shelves were bare. The last can was opened on Day " + (Day - 5) + ".";
            if (Radiation >= 200)
                return "Acute radiation syndrome. The dosimeter stopped counting. So did you.";
            if (Radiation >= 100)
                return "Radiation sickness. The symptoms were textbook. The treatment was not available.";
            return "The bunker fell silent. The ledger closes here.";
        }

        public bool ArchiveAndFreshStart(string basePathOverride = null!, string tradePathOverride = null!)
        {
            try
            {
                string basePath = basePathOverride ?? HoldfastSaveStore.SavePath;
                string tradePath = tradePathOverride ?? HoldfastTradeSaveStore.SavePath;
                string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture); // DETERMINISM_ALLOWLIST: Archive folder timestamp
                string archiveDir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(basePath) ?? string.Empty, "holdfast_archive_" + timestamp);

                bool archived = true;
                if (System.IO.File.Exists(basePath))
                {
                    try { System.IO.Directory.CreateDirectory(archiveDir); System.IO.File.Move(basePath, System.IO.Path.Combine(archiveDir, System.IO.Path.GetFileName(basePath))); }
                    catch (Exception) { archived = false; }
                }
                if (System.IO.File.Exists(tradePath))
                {
                    try { System.IO.Directory.CreateDirectory(archiveDir); System.IO.File.Move(tradePath, System.IO.Path.Combine(archiveDir, System.IO.Path.GetFileName(tradePath))); }
                    catch (Exception) { archived = false; }
                }

                // Reset mutable state.
                Trade.ResetToDefaults();
                HasPurchasedThisSession = false;
                LastPersistenceMessage = archived
                    ? "New ledger started. Prior records archived to " + System.IO.Path.GetFileName(archiveDir) + "."
                    : "New ledger started. Prior records could not be archived but have been cleared.";
                StateChanged?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                LastPersistenceMessage = "Fresh start failed: " + e.Message;
                return false;
            }
        }
    }
}
