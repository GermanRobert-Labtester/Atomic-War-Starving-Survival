using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    public partial class ExpeditionSystem
    {

        private bool TryProcessUxoLoot(ExpeditionState exp)
        {
            if (exp == null || exp.UxoDetonated) return false;
            if (!NodeHasUxo(exp.TargetLocationId)) return false;
            if (exp.Survivor == null) return false;
            if (!UxoHazardSystem.ShouldDetonateOnLoot(exp.Survivor.RiskBias, _rng)) return false;
            return DetonateUxo(exp);
        }

        private bool TryProcessUxoFlee(ExpeditionState exp)
        {
            if (exp == null || exp.UxoDetonated) return false;
            if (!NodeHasUxo(exp.TargetLocationId)) return false;
            if (!UxoHazardSystem.ShouldDetonateOnFlee(_rng)) return false;
            return DetonateUxo(exp);
        }

        private bool NodeHasUxo(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId) || _generatedMap == null) return false;
            var node = _generatedMap.GetNode(nodeId);
            return node != null && node.HasUxo;
        }

        private bool DetonateUxo(ExpeditionState exp)
        {
            if (!UxoHazardSystem.ApplyDetonation(exp, _medicalSystem)) return false;
            OnUxoDetonated?.Invoke(exp, UxoHazardSystem.DetonationLogMessage);
            GameLog.Log($"[UXO] {UxoHazardSystem.DetonationLogMessage}");
            return true;
        }

        public bool ForceUxoDetonationForTests(ExpeditionState exp)
        {
            if (exp == null) return false;
            return DetonateUxo(exp);
        }

        private void CompleteExpedition(ExpeditionState exp)
        {
            exp.Phase = ExpeditionPhase.Completed;

            // Transfer collected loot into bunker Inventory
            if (_inventory != null && exp.CollectedLoot != null)
            {
                for (int i = 0; i < exp.CollectedLoot.Count; i++)
                {
                    if (exp.CollectedLoot[i] != null)
                    {
                        _inventory.Add(exp.CollectedLoot[i], 1);
                    }
                }
            }

            // Return survivor to Idle state in shelter
            if (exp.Survivor != null)
            {
                exp.Survivor.State = SurvivorState.Idle;
            }

            // Prompts #206 / #209 — max-weight returns + night no-flashlight survival.
            ApplyExpeditionPerkMilestonesOnComplete(exp);

            OnExpeditionCompleted?.Invoke(exp, exp.CollectedLoot);
        }

        private void ApplyExpeditionPerkMilestonesOnComplete(ExpeditionState exp)
        {
            if (_expeditionPerks == null || exp?.Survivor == null || !exp.Survivor.IsAlive) return;
            int day = _getDay != null ? _getDay() : 0;

            // Pack Mule: returned at maximum weight capacity.
            _expeditionPerks.RecordMaxWeightReturn(
                exp.Survivor, exp.CurrentWeight, exp.CarryingCapacity, day);

            // Night Terror: night expedition completed without a working flashlight.
            if (exp.IsNightScavenge && !exp.HasFlashlight)
                _expeditionPerks.RecordNightExpeditionNoFlashlight(exp.Survivor, day);

            // Forager milestone if node was forest/swamp (also recorded on empty loot path).
            var node = ResolveMapNode(exp.TargetLocationId);
            if (node != null && ExpeditionPerkSystem.IsForestOrSwampTags(node.Tags))
                _expeditionPerks.RecordForestOrSwampScavenge(exp.Survivor, day);
        }

        private void CreateDefaultEncounters()
        {
            // 1. Feral Dogs
            var feralDogs = ScriptableObject.CreateInstance<EncounterSO>();
            feralDogs.id = ExpeditionPerkSystem.EncFeralDogs;
            feralDogs.title = "Feral Dog Pack";
            feralDogs.description = "A hungry pack of mutated dogs guards the alley ahead.";
            feralDogs.category = EncounterCategory.Combat;
            feralDogs.baseWeight = 1.0f;
            feralDogs.stealthWeightMultiplier = 0.4f;
            feralDogs.speedWeightMultiplier = 1.6f;
            feralDogs.autoEngageTrait = RiskBiasTrait.Reckless;
            feralDogs.autoFleeTrait = RiskBiasTrait.Paranoid;
            feralDogs.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "engage", Text = "Drive them off aggressively", MoraleDelta = +5f },
                new EventChoice { ChoiceId = "sneak", Text = "Sneak past silently", MoraleDelta = 0f },
                new EventChoice { ChoiceId = "flee", Text = "Drop loot and flee", MoraleDelta = -5f }
            };
            _encounterPool.Add(feralDogs);

            // 1b. Sleeping Ghoul (Prompt #207 Light Step bypass target)
            var sleepingGhoul = ScriptableObject.CreateInstance<EncounterSO>();
            sleepingGhoul.id = ExpeditionPerkSystem.EncSleepingGhoul;
            sleepingGhoul.title = "Sleeping Ghoul";
            sleepingGhoul.description = "Something that used to be human sleeps in the rubble. One wrong step and it wakes.";
            sleepingGhoul.category = EncounterCategory.Combat;
            sleepingGhoul.baseWeight = 0.7f;
            sleepingGhoul.stealthWeightMultiplier = 0.3f;
            sleepingGhoul.speedWeightMultiplier = 1.4f;
            sleepingGhoul.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "engage", Text = "Kill it before it wakes", MoraleDelta = -5f },
                new EventChoice { ChoiceId = "sneak", Text = "Creep past on soft soles", MoraleDelta = 0f },
                new EventChoice { ChoiceId = "flee", Text = "Back away carefully", MoraleDelta = -2f }
            };
            _encounterPool.Add(sleepingGhoul);

            // 2. Civil War Deserters (combat toll — distinct from Deserter's Stand)
            var deserters = ScriptableObject.CreateInstance<EncounterSO>();
            deserters.id = DesertersStandSystem.CombatDesertersEncounterId;
            deserters.title = "Civil War Deserters";
            deserters.description = "Armed scavengers demand a toll to pass through their sector.";
            deserters.category = EncounterCategory.Combat;
            deserters.baseWeight = 0.8f;
            deserters.minDangerLevel = 2f;
            deserters.stealthWeightMultiplier = 0.3f;
            deserters.speedWeightMultiplier = 1.8f;
            deserters.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "fight", Text = "Fight your way through", MoraleDelta = +10f },
                new EventChoice { ChoiceId = "pay", Text = "Pay them off with rations", MoraleDelta = -10f }
            };
            _encounterPool.Add(deserters);

            // 3. Collapsed Rubble
            var rubble = ScriptableObject.CreateInstance<EncounterSO>();
            rubble.id = "enc_collapsed_rubble";
            rubble.title = "Collapsed Concrete Rubble";
            rubble.description = "A heavy slab blocks the quick path forward.";
            rubble.category = EncounterCategory.Hazard;
            rubble.baseWeight = 1.2f;
            rubble.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "clear", Text = "Force your way through rubble", MoraleDelta = -2f },
                new EventChoice { ChoiceId = "detour", Text = "Take a long detour", MoraleDelta = -5f }
            };
            _encounterPool.Add(rubble);

            // 4. Prompt #15 — Deserter's Stand (narrative discovery; fires via map flag)
            _encounterPool.Add(DesertersStandSystem.CreateEncounter());
        }

    }
}
