using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Events;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Data;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Phase 2 — Wire the three psychological systems into live gameplay:
    ///   PhantomMemorySystem  → ExpeditionSystem.OnExpeditionCompleted
    ///   GuiltInsomniaSystem  → EventRunner.OnChoiceApplied
    ///   CombatTraumaSystem  → HatchDefenseSystem.OnRaidResolved
    ///
    /// Also registers default phantom-trigger rules for all backgrounds.
    /// </summary>
    public partial class GameBootstrap
    {
        /// <summary>
        /// Call during InitializeSystems, after EventRunner and
        /// HatchDefenseSystem are constructed.
        /// </summary>
        private void InitPhase2Wiring()
        {
            // ── Phantom Memory: register default trigger rules ─────────
            RegisterDefaultPhantomTriggers();

            // ── Phantom Memory: wire into ExpeditionSystem ─────────────
            if (ExpeditionSystem != null)
            {
                Action<ExpeditionState, List<ItemDefinition>> onExpeditionCompleted =
                    (state, items) =>
                    {
                        if (state == null || items == null || items.Count == 0) return;
                        var sv = FindSurvivorById(state.SurvivorId);
                        if (sv == null || !sv.IsAlive) return;
                        int checkCount = Math.Min(3, items.Count);
                        for (int j = 0; j < checkCount; j++)
                        {
                            PhantomMemorySystem?.OnItemScavenged(sv, items[j].Id);
                        }
                    };
                ExpeditionSystem.OnExpeditionCompleted += onExpeditionCompleted;
                _subscriptions.Track(() =>
                    ExpeditionSystem.OnExpeditionCompleted -= onExpeditionCompleted);
            }

            // ── Combat Trauma: wire into HatchDefenseSystem ────────────
            if (HatchDefenseSystem != null)
            {
                Action<RaidResolution> onRaidResolved = (resolution) =>
                {
                    if (resolution == null || !resolution.Launched) return;
                    // Register combat trauma for traumatized survivors
                    if (resolution.TraumatizedSurvivorIds != null)
                    {
                        for (int i = 0; i < resolution.TraumatizedSurvivorIds.Count; i++)
                        {
                            var sv = FindSurvivorById(resolution.TraumatizedSurvivorIds[i]);
                            if (sv != null && sv.IsAlive)
                                CombatTraumaSystem?.OnCombatSurvived(sv);
                        }
                    }
                    // Also register for anyone on guard duty
                    if (Survivors != null && resolution.Repelled)
                    {
                        for (int i = 0; i < Survivors.Count; i++)
                        {
                            var sv = Survivors[i];
                            if (sv != null && sv.IsAlive && sv.State == SurvivorState.Working)
                                CombatTraumaSystem?.OnCombatSurvived(sv);
                        }
                    }
                };
                HatchDefenseSystem.OnRaidResolved += onRaidResolved;
                _subscriptions.Track(() =>
                    HatchDefenseSystem.OnRaidResolved -= onRaidResolved);
            }

            // ── Guilt Insomnia: wire into EventRunner choice callbacks ──
            if (EventRunner != null)
            {
                Action<GameEvent, EventChoice, EventContext> onChoiceApplied =
                    (gameEvent, choice, context) =>
                    {
                        if (choice == null) return;
                        string eventId = gameEvent?.Id ?? "unknown";
                        string choiceId = choice.ChoiceId ?? choice.Text ?? "unknown";
                        float severity = GetGuiltSeverityForChoice(eventId, choiceId);
                        if (severity <= 0f) return;

                        if (Survivors == null) return;
                        for (int i = 0; i < Survivors.Count; i++)
                        {
                            var sv = Survivors[i];
                            if (sv == null || !sv.IsAlive) continue;
                            string sourceId = $"{eventId}_{choiceId}";
                            GuiltInsomniaSystem?.RecordGuilt(sv, sourceId, severity);
                        }
                    };
                EventRunner.OnChoiceApplied += onChoiceApplied;
                _subscriptions.Track(() =>
                    EventRunner.OnChoiceApplied -= onChoiceApplied);
            }
        }

        /// <summary>
        /// Determine guilt severity for a given event/choice combination.
        /// High-severity choices: ration cutting, leaving behind, execution.
        /// Medium: lying, stealing, refusing help.
        /// Low: minor harsh words, prioritizing self.
        /// </summary>
        private float GetGuiltSeverityForChoice(string eventId, string choiceId)
        {
            if (string.IsNullOrEmpty(choiceId)) return 0f;

            // Ration-related choices
            if (choiceId.Contains("cut_ration") || choiceId.Contains("reduce_food") ||
                choiceId.Contains("starve")) return 0.8f;

            // Abandonment / leaving behind
            if (choiceId.Contains("leave_behind") || choiceId.Contains("abandon") ||
                choiceId.Contains("refuse_help") || choiceId.Contains("turn_away"))
                return 0.7f;

            // Execution / violence
            if (choiceId.Contains("execute") || choiceId.Contains("kill") ||
                choiceId.Contains("shoot")) return 0.9f;

            // Theft
            if (choiceId.Contains("steal") || choiceId.Contains("hoard") ||
                choiceId.Contains("take_all")) return 0.5f;

            // Deception
            if (choiceId.Contains("lie") || choiceId.Contains("deceive") ||
                choiceId.Contains("betray")) return 0.4f;

            // Minor harshness
            if (choiceId.Contains("harsh") || choiceId.Contains("refuse") ||
                choiceId.Contains("deny")) return 0.2f;

            return 0f;
        }

        /// <summary>
        /// Register default phantom-trigger rules linking survivor backgrounds
        /// to item categories and outcome probabilities.
        /// </summary>
        private void RegisterDefaultPhantomTriggers()
        {
            if (PhantomMemorySystem == null) return;

            PhantomTriggerCatalogLoader.LoadInto(PhantomMemorySystem);
            if (PhantomMemorySystem.RulesByBackground.Count > 0) return;

            // Fallback defaults if JSON missing
            PhantomMemorySystem.RegisterRule("child_refugee", "childhood", 0.3f,
                "description_phantom_child_toy");
            PhantomMemorySystem.RegisterRule("child_refugee", "photograph", 0.5f,
                "description_phantom_child_photo");
            PhantomMemorySystem.RegisterRule("child_refugee", "correspondence", 0.4f,
                "description_phantom_child_letter");

            // former_soldier: military items, dog tags trigger combat memories
            PhantomMemorySystem.RegisterRule("former_soldier", "military", 0.2f,
                "description_phantom_soldier_military");
            PhantomMemorySystem.RegisterRule("former_soldier", "personal_item", 0.4f,
                "description_phantom_soldier_personal");
            PhantomMemorySystem.RegisterRule("former_soldier", "medical", 0.3f,
                "description_phantom_soldier_medical");

            // nurse: medical items trigger memories of patients
            PhantomMemorySystem.RegisterRule("nurse", "medical", 0.5f,
                "description_phantom_nurse_medical");
            PhantomMemorySystem.RegisterRule("nurse", "correspondence", 0.4f,
                "description_phantom_nurse_letter");
            PhantomMemorySystem.RegisterRule("nurse", "photograph", 0.4f,
                "description_phantom_nurse_photo");

            // teacher: books, journals trigger classroom memories
            PhantomMemorySystem.RegisterRule("teacher", "correspondence", 0.5f,
                "description_phantom_teacher_letter");
            PhantomMemorySystem.RegisterRule("teacher", "childhood", 0.3f,
                "description_phantom_teacher_child");

            // electrician: tools, gadgets trigger pre-war work memories
            PhantomMemorySystem.RegisterRule("electrician", "personal_item", 0.3f,
                "description_phantom_electrician_tool");

            // machinist: tools, mechanical items
            PhantomMemorySystem.RegisterRule("machinist", "personal_item", 0.3f,
                "description_phantom_machinist_tool");

            // generic: catch-all for any background not specified
            PhantomMemorySystem.RegisterRule("generic", "photograph", 0.5f,
                "description_phantom_generic_photo");
            PhantomMemorySystem.RegisterRule("generic", "correspondence", 0.4f,
                "description_phantom_generic_letter");
            PhantomMemorySystem.RegisterRule("generic", "personal_item", 0.2f,
                "description_phantom_generic_personal");
        }
    }
}
