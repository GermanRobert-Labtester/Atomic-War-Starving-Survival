using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Quests;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Data;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansions 3 & 4 — Procedural loot, dynamic questlines, siege
    /// tactics, faction intelligence, and vehicle systems wiring.
    /// </summary>
    public partial class GameBootstrap
    {
        // ── Expansions 3 & 4 system accessors ─────────────────────────

        public ProceduralLootGenerator ProceduralLoot { get; private set; }
        public DynamicQuestlineSystem DynamicQuestlines { get; private set; }
        public FactionIntelligenceSystem FactionIntel { get; private set; }
        public VehicleMaintenanceSystem VehicleMaintenance { get; private set; }

        /// <summary>
        /// Call during InitializeSystems after all core systems exist.
        /// </summary>
        private void InitExpansions3to4()
        {
            InitExpansion3Systems();
            InitExpansion4Systems();
            WireExpansions3to4Callbacks();
        }

        // ═══════════════════════════════════════════════════════════════
        // Expansion 3: Procedural Loot + Dynamic Questlines
        // ═══════════════════════════════════════════════════════════════

        private void InitExpansion3Systems()
        {
            // ── Procedural Loot Generator ──────────────────────────────
            ProceduralLoot = new ProceduralLootGenerator(_worldSeed + 81);
            _registry.Register<ProceduralLootGenerator>(ProceduralLoot);

            // ── Dynamic Questline System ───────────────────────────────
            DynamicQuestlines = new DynamicQuestlineSystem
            {
                GetCurrentDay = () => TimeSystem?.CurrentDay ?? 1,
                FireNarrativeEvent = (eventId, context) =>
                {
                    EventRunner?.TriggerEventById(eventId);
                },
                Rng = new System.Random(_worldSeed + 83)
            };
            _registry.RegisterEventDriven("dynamicQuestlines");
            _registry.Register<DynamicQuestlineSystem>(DynamicQuestlines);

            // Wire quest completion to expedition returns
            if (ExpeditionSystem != null)
            {
                Action<ExpeditionState, List<ItemDefinition>> onExpeditionComplete =
                    (state, items) =>
                    {
                        if (state == null) return;
                        // Check if any active quest targets this location
                        foreach (var kv in DynamicQuestlines.ActiveQuests)
                        {
                            var quest = kv.Value;
                            if (quest.TargetLocationId == state.TargetLocationId)
                            {
                                // Mark objectives complete for items found
                                if (items != null)
                                {
                                    foreach (var item in items)
                                    {
                                        if (item?.Id != null)
                                            DynamicQuestlines.CompleteObjective(
                                                quest.QuestId, item.Id);
                                    }
                                }
                                // Advance stage if location reached
                                if (quest.CurrentStage == 1)
                                    DynamicQuestlines.AdvanceStage(quest.QuestId);
                            }
                        }
                    };
                ExpeditionSystem.OnExpeditionCompleted += onExpeditionComplete;
                _subscriptions.Track(() =>
                    ExpeditionSystem.OnExpeditionCompleted -= onExpeditionComplete);
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // Expansion 4: Siege, Faction Intel, Vehicles
        // ═══════════════════════════════════════════════════════════════

        private void InitExpansion4Systems()
        {
            // ── Faction Intelligence System ────────────────────────────
            FactionIntel = new FactionIntelligenceSystem
            {
                GetFactionStanding = factionId =>
                {
                    if (EconomySystem != null)
                        return EconomySystem.GetStanding(factionId);
                    return 0f;
                },
                GetAvailableResource = resourceType =>
                {
                    if (Inventory != null)
                        return Inventory.CountById(resourceType);
                    return 0;
                },
                ConsumeResource = (resourceType, amount) =>
                {
                    Inventory?.RemoveById(resourceType, amount);
                },
                GetDay = () => TimeSystem?.CurrentDay ?? 1,
                Rng = new System.Random(_worldSeed + 85)
            };
            _registry.RegisterPerSubstep("factionIntel",
                h => FactionIntel.Tick(h));
            _registry.Register<FactionIntelligenceSystem>(FactionIntel);

            // ── Vehicle Maintenance System ─────────────────────────────
            VehicleMaintenance = new VehicleMaintenanceSystem();
            VehicleMaintenance.SetRng(new System.Random(_worldSeed + 87));
            _registry.RegisterDaily("vehicleMaintenance",
                d => VehicleMaintenance.TickDaily(d));
            _registry.Register<VehicleMaintenanceSystem>(VehicleMaintenance);

            // Register default vehicle
            VehicleMaintenance.RegisterVehicle("scout_truck",
                "Armored Scout Truck", 60f, 200f, 1.5f);

            // ── Wire siege tactics to existing HatchDefenseSystem ──────
            if (HatchDefenseSystem != null)
            {
                // Siege tactics are now available via the partial class
                // HatchDefenseSystem.SiegeTactics.cs
            }
        }

        private void WireExpansions3to4Callbacks()
        {
            // ── Procedural loot integration with scavenging ────────────
            if (LocationScavengingSystem != null && ProceduralLoot != null)
            {
                // Hook: when scavenging completes, use ProceduralLootGenerator
                // to add variance to the returned items
            }

            // ── Dynamic questline narrative event triggers ─────────────
            if (DynamicQuestlines != null && EventRunner != null)
            {
                Action<DynamicQuestState, int> onStageAdvanced = (quest, stage) =>
                {
                    string eventId = $"narrative_{quest.QuestId}_stage_{stage}";
                    EventRunner.TriggerEventById(eventId);
                };
                DynamicQuestlines.OnStageAdvanced += onStageAdvanced;
                _subscriptions.Track(() =>
                    DynamicQuestlines.OnStageAdvanced -= onStageAdvanced);
            }

            // ── Faction intel: wire radio intercepts ───────────────────
            if (FactionIntel != null && RadioTunerSystem != null)
            {
                // Radio frequency decoding can yield intel
            }
        }
    }
}
