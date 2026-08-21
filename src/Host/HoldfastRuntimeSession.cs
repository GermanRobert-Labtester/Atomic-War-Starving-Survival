using System;
#pragma warning disable CS8618
using System.IO;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Godot's playable Holdfast boundary. The world session owns existing
    /// Holdfast systems; the Core trade session owns mutable inventory/value/stock.
    ///
    /// ARCHITECTURAL DEBT: This class duplicates survival mechanics (Health/Hunger/Thirst/Radiation)
    /// that already live in Ashfall.Core.Survivors.NeedsSystem. The constants MaxHealth, MaxHunger,
    /// MaxThirst, StarvationThreshold, DehydrationThreshold, and RadDamageThreshold, plus the
    /// AdvanceOneDay() method, must be migrated into NeedsSystem. Do not add new survival logic here.
    /// Tracked by: AGENTS.md issue H1.
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

        // ── Survival state ───────────────────────────────────────────
        public int Health { get; private set; } = MaxHealth;
        public float Radiation { get; private set; } = 0f; // mSv accumulated
        public int Hunger { get; private set; } = 0; // 0 = fed, 100 = starving
        public int Thirst { get; private set; } = 0; // 0 = hydrated, 100 = dehydrated
        public int Day { get; private set; } = 1;
        public bool IsDead => Health <= 0;
        public string DeathCause { get; private set; } = string.Empty;

        // ── Quest / Win state ────────────────────────────────────────
        public bool IsGameWon => World.Quests != null && World.Quests.IsCompleted(HoldfastQuestSystem.Hatch);
        public string WinMessage { get; private set; } = string.Empty;

        public event Action StateChanged;
        public event Action<string> OnPlayerDied; // passes cause of death
        public event Action<string> OnGameWon; // passes win message

        public HoldfastRuntimeSession(CoreDemoSession world, long startingValue = DefaultStartingValue)
        {
            World = world ?? throw new ArgumentNullException(nameof(world));
            Trade = new HoldfastTradeSession(World.Catalog, startingValue);
            Trade.StateChanged += () => StateChanged?.Invoke();
        }

        public static HoldfastRuntimeSession Create(
            CoreDemoSession world,
            bool seedDevelopmentState = true,
            bool loadTradeSave = true)
        {
            var session = new HoldfastRuntimeSession(world);
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

        public bool TryReload(string basePathOverride = null!, string tradePathOverride = null!)
        {
            bool restoredAny = false;
            var baseSave = HoldfastSaveStore.TryLoad(basePathOverride);
            if (baseSave != null)
            {
                World.RestoreSave(baseSave);
                restoredAny = true;
            }

            var tradeSave = HoldfastTradeSaveStore.TryLoad(tradePathOverride);
            if (tradeSave != null)
            {
                if (!Trade.TryRestoreState(tradeSave, out string error))
                {
                    LastPersistenceMessage = "Holdfast trade reload rejected: " + error;
                    return false;
                }
                restoredAny = true;
            }

            LastPersistenceMessage = restoredAny
                ? "Holdfast state reloaded from disk."
                : "No Holdfast save was available to reload.";
            return restoredAny;
        }

        public void SeedDevelopmentState()
        {
            Trade.SeedInventory("item_triplicate_carbon", 1);
        }

        // ── Survival mechanics ────────────────────────────────────────

        /// <summary>
        /// Advance one day. Hunger and thirst increase; radiation accumulates;
        /// health decreases if thresholds are exceeded. Quests auto-advance.
        /// Returns a summary string.
        /// </summary>
        public string TickDay()
        {
            if (IsDead) return "The ledger is closed. No more days to count.";

            Day++;

            // Hunger and thirst increase daily
            Hunger = Math.Min(MaxHunger, Hunger + 8);
            Thirst = Math.Min(MaxThirst, Thirst + 10);

            // Radiation decays slowly (biological half-life ~10 days)
            if (Radiation > 0)
                Radiation = Math.Max(0, Radiation - Radiation * 0.07f);

            // Health damage from starvation, dehydration, radiation
            int hpLoss = 0;
            if (Hunger >= StarvationThreshold)
                hpLoss += (int)((Hunger - StarvationThreshold) * 0.5f);
            if (Thirst >= DehydrationThreshold)
                hpLoss += (int)((Thirst - DehydrationThreshold) * 0.6f);
            if (Radiation >= RadDamageThreshold)
                hpLoss += (int)((Radiation - RadDamageThreshold) * 0.1f);

            Health = Math.Max(0, Health - hpLoss);

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

        /// <summary>
        /// Consume food items from inventory to reduce hunger.
        /// Returns true if food was consumed.
        /// </summary>
        public bool ConsumeFood(string itemId, int amount = 1)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0) return false;
            int held = Trade.GetHeld(itemId);
            if (held < amount) return false;

            Trade.Inventory.RemoveItem(itemId, amount);
            Hunger = Math.Max(0, Hunger - 30 * amount);
            StateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Consume water items from inventory to reduce thirst.
        /// Returns true if water was consumed.
        /// </summary>
        public bool ConsumeWater(string itemId, int amount = 1)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0) return false;
            int held = Trade.GetHeld(itemId);
            if (held < amount) return false;

            Trade.Inventory.RemoveItem(itemId, amount);
            Thirst = Math.Max(0, Thirst - 35 * amount);
            StateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Take radiation exposure (from events, locations, etc.).
        /// </summary>
        public void ExposeRadiation(float msv)
        {
            if (msv <= 0f) return;
            Radiation += msv;
            StateChanged?.Invoke();
        }

        /// <summary>
        /// Use anti-rad items to reduce radiation.
        /// </summary>
        public bool UseAntiRad(string itemId, float reduction = 20f)
        {
            if (string.IsNullOrEmpty(itemId) || reduction <= 0f) return false;
            int held = Trade.GetHeld(itemId);
            if (held < 1) return false;

            Trade.Inventory.RemoveItem(itemId, 1);
            Radiation = Math.Max(0, Radiation - reduction);
            StateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Heal health directly.
        /// </summary>
        public void Heal(int amount)
        {
            if (amount <= 0) return;
            Health = Math.Min(MaxHealth, Health + amount);
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
                string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture);
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
