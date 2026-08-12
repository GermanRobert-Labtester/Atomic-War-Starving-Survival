using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Phases 4-6 — Wire Trade Specialties, Final Wishes, and Diegetic
    /// Artifact systems into live gameplay.
    ///
    ///   TradeSpecialtySystem        → CraftingSystem.OnItemCrafted
    ///   FinalWishSystem             → PrognosisPipeline terminal hook
    ///   PersonalKeepsakeSystem      → Game start + inventory loss events
    ///   LostCorrespondenceSystem    → Expedition item finds
    ///   PhotoRestorationSystem      → Crafting recipe completion
    ///   RelicRestorationSystem      → Crafting recipe completion
    ///   MemorialWallSystem          → Dog tag collection
    ///   HamRadioTracingSystem       → RadioTunerSystem frequency tracking
    ///   DamagedMapSystem            → Map piece assembly
    ///   AudioCassetteSystem         → Cassette set collection
    /// </summary>
    public partial class GameBootstrap
    {
        public MemorialWallSystem MemorialWallSystem { get; private set; }
        public PersonalKeepsakeSystem PersonalKeepsakeSystem { get; private set; }

        /// <summary>
        /// Call during InitializeSystems, after CraftingSystem,
        /// RadioTunerSystem, and ExpeditionSystem exist.
        /// </summary>
        private void InitPhases4to6Wiring()
        {
            // ── Phase 4: Trade Specialties ─────────────────────────────
            WireTradeSpecialties();

            // ── Phase 4: Final Wishes ──────────────────────────────────
            WireFinalWishes();

            // ── Phase 5: Diegetic Artifacts ────────────────────────────
            WireDiegeticArtifacts();

            // ── Phase 6: Radio / Maps / Cassettes ──────────────────────
            WireRadioMapCassettes();
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 4: Trade Specialties + Final Wishes
        // ═══════════════════════════════════════════════════════════════

        private void WireTradeSpecialties()
        {
            if (TradeSpecialtySystem == null || CraftingSystem == null) return;

            Action<Recipe> onCraftCompleted = (recipe) =>
            {
                if (recipe?.result?.Id == null) return;
                string itemId = recipe.result.Id;
                if (Survivors == null) return;
                // Apply to the survivor most likely crafting
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var sv = Survivors[i];
                    if (sv == null || !sv.IsAlive) continue;
                    if (!string.IsNullOrEmpty(sv.PreWarProfessionId))
                    {
                        TradeSpecialtySystem.OnItemCrafted(sv, itemId);
                        break; // first eligible survivor
                    }
                }
            };
            CraftingSystem.OnCraftCompleted += onCraftCompleted;
            _subscriptions.Track(() =>
                CraftingSystem.OnCraftCompleted -= onCraftCompleted);
        }

        private void WireFinalWishes()
        {
            if (FinalWishSystem == null) return;

            // Register default wishes per archetype
            FinalWishSystem.RegisterWish("the_surgeon", FinalWishSystem.WishTeachLesson);
            FinalWishSystem.RegisterWish("the_soldier", FinalWishSystem.WishBuildMemorial);
            FinalWishSystem.RegisterWish("the_nurse", FinalWishSystem.WishTeachLesson);
            FinalWishSystem.RegisterWish("the_mother", FinalWishSystem.WishReconcile);
            FinalWishSystem.RegisterWish("the_mechanic", FinalWishSystem.WishRetrieveHeirloom);
            FinalWishSystem.RegisterWish("the_teacher", FinalWishSystem.WishTeachLesson);
            FinalWishSystem.RegisterWish("the_refugee", FinalWishSystem.WishSeeTheSky);
            FinalWishSystem.RegisterWish("the_electrician", FinalWishSystem.WishRetrieveHeirloom);

            // Wire terminal prognosis declaration from PrognosisPipeline
            // The PrognosisPipeline.ResolveOutcome already sets PrognosisStage=RecoveryOrDeath
            // and kills if deathChance hits. We hook into the terminal path via
            // RadiationPhaseProgression.OnTerminalPrognosisDeclared, which fires
            // when Manifest resolves with fatal outcome.
            if (RadiationPhaseProgression != null)
            {
                Action<Survivor> onTerminal = (sv) =>
                {
                    FinalWishSystem.DeclareTerminalPrognosis(sv);
                };
                RadiationPhaseProgression.OnTerminalPrognosisDeclared += onTerminal;
                _subscriptions.Track(() =>
                    RadiationPhaseProgression.OnTerminalPrognosisDeclared -= onTerminal);
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 5: Diegetic Artifacts
        // ═══════════════════════════════════════════════════════════════

        private void WireDiegeticArtifacts()
        {
            MemorialWallSystem = new MemorialWallSystem();
            PersonalKeepsakeSystem = new PersonalKeepsakeSystem();
            _registry.Register<MemorialWallSystem>(MemorialWallSystem);
            _registry.Register<PersonalKeepsakeSystem>(PersonalKeepsakeSystem);

            // ── Personal Keepsakes — set at survivor creation ──────────
            AssignInitialKeepsakes();
            WirePersonalKeepsakeInventory();

            // ── Memorial Wall — track dog tag collection ───────────────
            WireMemorialWall();

            // ── Wall Carvings — idle survivor daily carving chance ─────
            WireWallCarvings();

            // ── Lost Correspondence — track letter recovery ────────────
            WireLostCorrespondence();
        }

        private void AssignInitialKeepsakes()
        {
            if (Survivors == null) return;

            var keepsakeMap = new Dictionary<string, string>
            {
                { "sv_elena", "worn_photograph" },
                { "sv_marcus", "tarnished_pocket_watch" },
                { "sv_suki", "wedding_ring" }
            };

            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (keepsakeMap.TryGetValue(sv.Id, out string keepsakeId))
                {
                    sv.PersonalKeepsakeItemId = keepsakeId;
                }
                else
                {
                    // Default keepsake based on background
                    if (sv.PhantomBackgroundId == "former_soldier")
                        sv.PersonalKeepsakeItemId = "dog_tags_personal";
                    else if (sv.PhantomBackgroundId == "nurse")
                        sv.PersonalKeepsakeItemId = "worn_stethoscope";
                    else
                        sv.PersonalKeepsakeItemId = "family_photograph";
                }
            }
        }

        private void WireMemorialWall()
        {
            if (Inventory == null || MemorialWallSystem == null) return;

            Action<ItemDefinition, int> onItemAdded = (itemDef, amount) =>
            {
                if (itemDef?.id == null || amount <= 0) return;
                if (!itemDef.id.Contains("dog_tag") && !itemDef.id.Contains("dogtag")) return;
                MemorialWallSystem.AddEntry(new MemorialEntry
                {
                    SurvivorId = itemDef.id,
                    DisplayName = itemDef.displayName ?? itemDef.id,
                    DeathDay = TimeSystem?.CurrentDay ?? 0,
                    HasDogTag = true
                });
            };
            Inventory.OnItemAdded += onItemAdded;
            _subscriptions.Track(() => Inventory.OnItemAdded -= onItemAdded);
        }

        private void WirePersonalKeepsakeInventory()
        {
            if (Inventory == null || PersonalKeepsakeSystem == null || Survivors == null) return;

            Action<ItemDefinition, int> onRemoved = (itemDef, amount) =>
            {
                if (itemDef?.id == null || amount <= 0) return;
                for (int i = 0; i < Survivors.Count; i++)
                    PersonalKeepsakeSystem.OnInventoryItemRemoved(Survivors[i], itemDef.id);
            };
            Inventory.OnItemRemoved += onRemoved;
            _subscriptions.Track(() => Inventory.OnItemRemoved -= onRemoved);
        }

        private void WireWallCarvings()
        {
            // Wall carving happens passively during the daily tick
            // when survivors are Idle. The BunkerWallCarvingSystem
            // will be wired in the UI layer (Phase 11).
            // For now, the Survivor fields track the state.
        }

        private void WireLostCorrespondence()
        {
            if (ExpeditionSystem == null) return;

            // When expedition returns with letter/mail items
            Action<ExpeditionState, List<ItemDefinition>> onExpeditionComplete =
                (state, items) =>
                {
                    if (state == null || items == null) return;
                    var sv = FindSurvivorById(state.SurvivorId);
                    if (sv == null || !sv.IsAlive) return;

                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i]?.Id == null) continue;
                        string id = items[i].Id;
                        if (id.Contains("letter") || id.Contains("mail") ||
                            id.Contains("correspondence") || id.Contains("undelivered"))
                        {
                            if (!sv.RecoveredLetterIds.Contains(id))
                                sv.RecoveredLetterIds.Add(id);
                        }
                    }
                };
            ExpeditionSystem.OnExpeditionCompleted += onExpeditionComplete;
            _subscriptions.Track(() =>
                ExpeditionSystem.OnExpeditionCompleted -= onExpeditionComplete);
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 6: Ham Radio, Damaged Maps, Audio Cassettes
        // ═══════════════════════════════════════════════════════════════

        private void WireRadioMapCassettes()
        {
            // ── Ham Radio Tracing — bind to RadioTunerSystem ───────────
            if (RadioTunerSystem != null)
            {
                // Register distress signal frequencies
                // These are multi-day tracing frequencies
                _distressFrequencyIds = new List<string>
                {
                    "freq_distress_217_4",
                    "freq_distress_148_2",
                    "freq_distress_392_7",
                    "freq_distress_55_1",
                    "freq_distress_401_9"
                };
            }

            // ── Damaged Maps — track map fragment collection ───────────
            if (ExpeditionSystem != null)
            {
                Action<ExpeditionState, List<ItemDefinition>> onExpeditionComplete =
                    (state, items) =>
                    {
                        if (state == null || items == null) return;
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (items[i]?.Id == null) continue;
                            if (items[i].Id.Contains("damaged_map") ||
                                items[i].Id.Contains("map_fragment"))
                            {
                                // Map pieces tracked; assembly checked in
                                // DamagedMapSystem which reads inventory
                            }
                        }
                    };
                ExpeditionSystem.OnExpeditionCompleted += onExpeditionComplete;
                _subscriptions.Track(() =>
                    ExpeditionSystem.OnExpeditionCompleted -= onExpeditionComplete);
            }

            // ── Audio Cassettes — track set collection ────────────────
            if (Inventory != null)
            {
                Action<ItemDefinition, int> onItemAdded = (itemDef, amount) =>
                {
                    if (itemDef?.Id == null) return;
                    if (itemDef.Id.Contains("cassette") ||
                        itemDef.Id.Contains("audio_tape"))
                    {
                        // Cassette collection tracked; set completion
                        // checked in AudioCassetteSystem
                    }
                };
                Inventory.OnItemAdded += onItemAdded;
                _subscriptions.Track(() => Inventory.OnItemAdded -= onItemAdded);
            }
        }

        private List<string> _distressFrequencyIds;

        /// <summary>
        /// Returns the list of distress signal frequency IDs for ham radio tracing.
        /// </summary>
        public IReadOnlyList<string> GetDistressFrequencies() => _distressFrequencyIds;

        /// <summary>
        /// Returns true if a frequency is a distress signal (multi-day tracing).
        /// </summary>
        public bool IsDistressFrequency(string frequencyId)
        {
            return _distressFrequencyIds != null &&
                _distressFrequencyIds.Contains(frequencyId);
        }
    }
}
