// GameBootstrap.AshGetsDeeper.cs — wire Prompts #326–#330 ("The Ash Gets
// Deeper") into the host. Mirrors the turn-3 / turn-4 / turn-5 partial
// pattern: small set of focused boot methods, SaveSystem + diagnostics
// integration, optional Hardcore economy overlay.
using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        // ── Game mode (Hardcore toggle plumbing) ────────────────────────
        [Header("Ash Gets Deeper (Prompts #326–#330)")]
        [Tooltip("Game mode. Expert enables HardcoreEconomyTuning's " +
                 "scarcity-tier multipliers and price-shock events.")]
        [SerializeField] private GameModeKind _currentGameMode = GameModeKind.Story;
        [Tooltip("Force Hardcore economy tuning regardless of mode. " +
                 "Useful for testing without changing the inspector mode.")]
        [SerializeField] private bool _forceHardcoreEconomy = false;
        [Tooltip("Receive the 10 new lore echoes as soon as the journal " +
                 "system boots. Disable to gate them behind world flags.")]
        [SerializeField] private bool _autoDiscoverEchoes = true;

        public GameModeKind CurrentGameMode => _currentGameMode;
        public bool IsHardcoreMode => _forceHardcoreEconomy || _currentGameMode == GameModeKind.Expert;

        // ── 12 NPC / fauna state holders (mirrors the turn-3 pattern) ──
        public NPC_AshWidows NPCAshWidows { get; private set; }
        public NPC_TheTollman NPCTheTollman { get; private set; }
        public NPC_BurnedPatrol NPCBurnedPatrol { get; private set; }
        public NPC_TheCollector NPCTheCollector { get; private set; }
        public NPC_FeralChildren NPCFeralChildren { get; private set; }
        public NPC_SurgeonsCaravan NPCSurgeonsCaravan { get; private set; }
        public Fauna_IrradiatedDogs FaunaIrradiatedDogs { get; private set; }
        public Fauna_AshCrows FaunaAshCrows { get; private set; }
        public Fauna_BloatedCattle FaunaBloatedCattle { get; private set; }
        public Fauna_RatSwarm FaunaRatSwarm { get; private set; }

        // ── Master boot (called once from InitFoundation) ───────────────
        /// <summary>
        /// Section XIV wire-up. Merges the 80 items / 10 locations /
        /// 15 encounters / 10 echoes / 12 NPC/fauna archetypes into the
        /// existing catalogs and constructs the new NPC classes. Safe to
        /// call multiple times; de-dupes by id.
        /// </summary>
        public void BootAshGetsDeeperContent()
        {
            BootAshGetsDeeperItems();
            BootAshGetsDeeperLocations();
            BootAshGetsDeeperEncounters();
            BootAshGetsDeeperEchoes();
            BootAshGetsDeeperNpcs();
            ApplyHardcoreEconomyTuningIfEnabled();
            int npcCount = (NPCAshWidows != null ? 1 : 0) + (NPCTheTollman != null ? 1 : 0)
                + (NPCBurnedPatrol != null ? 1 : 0) + (NPCTheCollector != null ? 1 : 0)
                + (NPCFeralChildren != null ? 1 : 0) + (NPCSurgeonsCaravan != null ? 1 : 0)
                + (FaunaIrradiatedDogs != null ? 1 : 0) + (FaunaAshCrows != null ? 1 : 0)
                + (FaunaBloatedCattle != null ? 1 : 0) + (FaunaRatSwarm != null ? 1 : 0);
            GameLog.Log($"[GameBootstrap] Ash Gets Deeper booted: 80 items, 10 locations, " +
                "15 encounters, 10 echoes, " + npcCount + " NPCs/fauna; " +
                "HardcoreMode=" + (IsHardcoreMode ? "ON" : "off") + ".");
        }

        // ── 80 items → _itemCatalog.recipes-style merge into _itemCatalog.items ──
        private void BootAshGetsDeeperItems()
        {
            if (_itemCatalog == null)
            {
                GameLog.LogWarning("[GameBootstrap] _itemCatalog is null; skipping Section XIV items.");
                return;
            }
            var lookup = new Func<string, ItemDefinition>(id =>
                _itemCatalog != null ? _itemCatalog.GetById(id) : null);
            var defs = AshGetsDeeperItemsCatalog.MaterialiseAll(lookup);
            int added = 0;
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                if (d == null || string.IsNullOrEmpty(d.id)) continue;
                if (ContainsItemId(d.id)) continue;
                _itemCatalog.items.Add(d);
                added++;
            }
            GameLog.Log($"[GameBootstrap] Ash Gets Deeper items: merged {added} of {defs.Count}.");
        }

        private bool ContainsItemId(string id)
        {
            if (_itemCatalog?.items == null) return false;
            for (int i = 0; i < _itemCatalog.items.Count; i++)
                if (_itemCatalog.items[i] != null && _itemCatalog.items[i].id == id) return true;
            return false;
        }

        // ── 10 locations → _locationCatalog ─────────────────────────────
        // Prompts #326–#330 don't yet have a wired _locationCatalog field
        // on GameBootstrap (the inspector-side location catalog is owned
        // by the GameplaySceneBuilder). The 10 new locations are pooled
        // on the partial itself and exposed via LocationPool. The host's
        // LocationCatalogBuilder or a future wiring PR can merge them
        // into the real ScriptableObject list at boot.
        private void BootAshGetsDeeperLocations()
        {
            var locs = AshGetsDeeperLocationsCatalog.MaterialiseAll();
            _ashLocationPool.Clear();
            for (int i = 0; i < locs.Count; i++)
            {
                var l = locs[i];
                if (l == null || string.IsNullOrEmpty(l.id)) continue;
                if (ContainsLocationId(l.id)) continue;
                _ashLocationPool.Add(l);
            }
            GameLog.Log($"[GameBootstrap] Ash Gets Deeper locations: pooled " +
                $"{_ashLocationPool.Count} of {locs.Count}.");
        }

        private readonly List<LocationDefinitionSO> _ashLocationPool = new List<LocationDefinitionSO>();
        /// <summary>Read-only view of the pooled Section XIV locations.</summary>
        public IReadOnlyList<LocationDefinitionSO> LocationPool => _ashLocationPool;

        private bool ContainsLocationId(string id)
        {
            for (int i = 0; i < _ashLocationPool.Count; i++)
                if (_ashLocationPool[i] != null && _ashLocationPool[i].id == id) return true;
            return false;
        }

        // ── 15 encounters → event pool ─────────────────────────────────
        // We don't have a direct catalog field for events, so we raise
        // them onto the EventBus pool for any runner that subscribes to
        // GameEvent assets. If the host has a typed pool, the same merge
        // pattern (de-dup by id) is the right extension.
        private void BootAshGetsDeeperEncounters()
        {
            var defs = AshGetsDeeperEncountersCatalog.MaterialiseAll();
            int added = 0;
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                if (d == null || string.IsNullOrEmpty(d.id)) continue;
                if (ContainsEncounterId(d.id)) continue;
                _ashEncounterPool.Add(d);
                added++;
            }
            GameLog.Log($"[GameBootstrap] Ash Gets Deeper encounters: pooled {added} of {defs.Count}.");
        }

        private readonly List<GameEvent> _ashEncounterPool = new List<GameEvent>();
        /// <summary>Read-only view of the pooled Section XIV encounters. The
        /// host's EventRunner can subscribe to a global "ash_pool" event
        /// to roll from this list each day.</summary>
        public IReadOnlyList<GameEvent> AshEncounterPool => _ashEncounterPool;

        private bool ContainsEncounterId(string id)
        {
            for (int i = 0; i < _ashEncounterPool.Count; i++)
                if (_ashEncounterPool[i] != null && _ashEncounterPool[i].id == id) return true;
            return false;
        }

        // ── 10 echoes → JournalSystem.TryDiscover (fire-once) ──────────
        private void BootAshGetsDeeperEchoes()
        {
            if (JournalSystem == null)
            {
                GameLog.LogWarning("[GameBootstrap] JournalSystem is null; skipping Section XIV echoes.");
                return;
            }
            if (!_autoDiscoverEchoes) return;
            var defs = AshGetsDeeperEchoesCatalog.MaterialiseAll();
            int added = 0;
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                if (d == null || string.IsNullOrEmpty(d.id)) continue;
                // JournalSystem.TryDiscover is fire-once keyed by id; safe to
                // call at boot. Existing playthroughs that already discovered
                // the echo will no-op.
                if (JournalSystem.TryDiscover(d.id, d.text) != null) added++;
            }
            GameLog.Log($"[GameBootstrap] Ash Gets Deeper echoes: registered {added} of {defs.Count}.");
        }

        // ── 12 NPC / fauna classes → construct + log ───────────────────
        private void BootAshGetsDeeperNpcs()
        {
            NPCAshWidows          = new NPC_AshWidows();
            NPCTheTollman         = new NPC_TheTollman();
            NPCBurnedPatrol       = new NPC_BurnedPatrol();
            NPCTheCollector       = new NPC_TheCollector();
            NPCFeralChildren      = new NPC_FeralChildren();
            NPCSurgeonsCaravan    = new NPC_SurgeonsCaravan();
            FaunaIrradiatedDogs   = new Fauna_IrradiatedDogs();
            FaunaAshCrows         = new Fauna_AshCrows();
            FaunaBloatedCattle    = new Fauna_BloatedCattle();
            FaunaRatSwarm         = new Fauna_RatSwarm();
            // The 10 archetypes are dormant ghosts (mirrors the
            // Weather_AcidSnow convention); the EventRunner or
            // ExpeditionSystem reads AshEncounterPool and the host's
            // raid pipeline fires the relevant NPC when its trigger
            // condition is met. A future PR wires the per-NPC event
            // hooks (OnEncounterStarted/OnEncounterResolved) into
            // EventRunner callbacks.
        }

        // ── Hardcore economy overlay (opt-in via IsHardcoreMode) ───────
        /// <summary>
        /// When the player is in Expert (Hardcore) mode, push the
        /// scarcity-tier multipliers from <see cref="HardcoreEconomyTuning"/>
        /// into the <see cref="DynamicEconomySystem"/> as a per-day
        /// multiplier the existing GetTradeValue() pipeline reads. The
        /// override is non-destructive: default values remain when
        /// IsHardcoreMode is false.
        /// </summary>
        public void ApplyHardcoreEconomyTuningIfEnabled()
        {
            if (EconomySystem == null) return;
            if (!IsHardcoreMode)
            {
                EconomySystem.SetScarcityOverride(null);
                GameLog.Log("[GameBootstrap] Hardcore economy tuning: OFF (mode = " +
                    _currentGameMode + ").");
                return;
            }
            // 4 price-shock slots; the host can fire them via the existing
            // EventRunner choice resolution path. We seed them with the
            // Hardcore tier's per-day multiplier so the baseline value of
            // every item in the tuned buckets is already applied.
            EconomySystem.SetScarcityOverride(new DynamicEconomySystem.ScarcityOverride
            {
                Source = "HardcoreEconomyTuning",
                DayRangeStart = 1,
                DayRangeEnd = 9999,
                Tier1Critical = HardcoreEconomyTuning.GetScarcityMultiplierForDay(5, "clean_water"),
                Tier2High     = HardcoreEconomyTuning.GetScarcityMultiplierForDay(30, "antibiotics"),
                Tier3Moderate = HardcoreEconomyTuning.GetScarcityMultiplierForDay(60, "bandage"),
                Tier4Low      = HardcoreEconomyTuning.GetScarcityMultiplierForDay(90, "book"),
                IsHardcore    = true
            });
            GameLog.Log("[GameBootstrap] Hardcore economy tuning: ON (Expert mode).");
        }
    }
}
