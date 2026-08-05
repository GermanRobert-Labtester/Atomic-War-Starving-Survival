using System;
using System.Collections.Generic;
using UnityEngine;
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
        /// <summary>
        /// Day-30 intercept: sever comms on every active expedition, apply
        /// trauma + acute-dose spike, resolve the survivor's trait-driven
        /// behavior. Idempotent per expedition: re-firing on the same
        /// signal is a no-op (the first call sets the behavior).
        /// </summary>

        private ExpeditionState FindExpeditionById(string expeditionId)
        {
            if (string.IsNullOrEmpty(expeditionId)) return null;
            for (int i = 0; i < _activeExpeditions.Count; i++)
            {
                if (_activeExpeditions[i] != null && _activeExpeditions[i].ExpeditionId == expeditionId)
                    return _activeExpeditions[i];
            }
            return null;
        }

        private void RemoveExpedition(ExpeditionState exp)
        {
            _activeExpeditions.Remove(exp);
        }

        /// <summary>
        /// Advances phase logic. Returns true when the caller should skip the
        /// default encounter roll / tick event for this expedition index.
        /// </summary>
        private float CalculateStaminaDrain(ExpeditionState exp, float hours)
        {
            float drain = BaseStaminaDrainPerHour * hours;

            // Carry weight penalty: up to +15/hr at full capacity
            // Prompt #206 — Pack Mule halves the over-encumbrance portion.
            float loadRatio = exp.CarryingCapacity > 0f ? Mathf.Clamp01(exp.CurrentWeight / exp.CarryingCapacity) : 0f;
            float encumberPenalty = loadRatio * 15f * hours;
            if (_expeditionPerks != null && exp.Survivor != null)
                encumberPenalty *= _expeditionPerks.GetOverEncumberPenaltyMultiplier(exp.Survivor);
            drain += encumberPenalty;

            // Weather penalty
            if (_weatherSystem != null)
            {
                if (_weatherSystem.Current == WeatherKind.Blizzard
                    || _weatherSystem.Current == WeatherKind.FalloutStorm
                    || _weatherSystem.Current == WeatherKind.BlackRain)
                {
                    drain += 10f * hours;
                }
            }

            // Suit wear & degradation (Black Rain melts hazmat aggressively — Prompt #11)
            if (exp.Survivor.HasFullSuitEquipped)
            {
                float suitWearPerHour = 2f;
                if (_weatherSystem != null)
                    suitWearPerHour *= _weatherSystem.HazmatDegradeMultiplier;
                exp.SuitDegradation = Mathf.Clamp(exp.SuitDegradation + suitWearPerHour * hours, 0f, 100f);
                drain += 3f * hours; // suit heat & movement restriction
            }

            // Limp disability: permanently doubles stamina drain during expeditions
            if (exp.Survivor != null && exp.Survivor.HasDisability("limp"))
            {
                drain *= 2f;
            }

            return drain;
        }

        private void PerformLootRoll(ExpeditionState exp)
        {
            // Prompt #207 — Light Step scavengers do not raise scavenging noise.
            TryRaiseScavengeNoise(exp);

            if (_itemCatalog == null || _itemCatalog.items == null || _itemCatalog.items.Count == 0)
            {
                // Empty catalog still allows Forager guaranteed food.
                TryApplyForagerLoot(exp);
                return;
            }

            float chance = 0.5f + (exp.DangerLevel * 0.05f);
            // Prompt #69 — flooded nodes pay more (high risk, high reward).
            float floodMult = _floodedNodeSystem != null
                ? _floodedNodeSystem.GetLootMultiplier(exp.TargetLocationId)
                : 1f;
            chance = Mathf.Min(0.95f, chance * floodMult);

            int before = exp.CollectedLoot != null ? exp.CollectedLoot.Count : 0;
            int rolls = floodMult > 1.5f ? 2 : 1;
            for (int r = 0; r < rolls; r++)
            {
                if (_rng.NextDouble() >= chance) continue;
                TryAddLootItem(exp);
            }

            int after = exp.CollectedLoot != null ? exp.CollectedLoot.Count : 0;
            if (after <= before)
                TryApplyForagerLoot(exp);
        }

        /// <summary>Prompt #207 — scavenging makes noise unless Light Step.</summary>
        private void TryRaiseScavengeNoise(ExpeditionState exp)
        {
            if (_noiseSystem == null || exp?.Survivor == null) return;
            if (_expeditionPerks != null && _expeditionPerks.SuppressesScavengeNoise(exp.Survivor))
                return;
            bool storm = _isStormActive != null && _isStormActive();
            _noiseSystem.AddNoise(0.35f, storm);
        }

        /// <summary>
        /// Prompt #210 — Forager: empty loot still yields 1–2 Roots or Berries.
        /// Milestone counting for Forest/Swamp is handled on expedition complete.
        /// </summary>
        private void TryApplyForagerLoot(ExpeditionState exp)
        {
            if (exp?.Survivor == null || _expeditionPerks == null) return;
            if (exp.ForagerLootApplied) return;

            int existing = exp.CollectedLoot != null ? exp.CollectedLoot.Count : 0;
            int count = _expeditionPerks.GetForagerGuaranteedFoodCount(exp.Survivor, existing, _rng);
            if (count <= 0) return;

            EnsureForagerFoodItems();
            for (int i = 0; i < count; i++)
            {
                string id = ExpeditionPerkSystem.PickForagerFoodId(_rng);
                var item = string.Equals(id, ExpeditionPerkSystem.BerriesItemId, StringComparison.OrdinalIgnoreCase)
                    ? _foragerBerries
                    : _foragerRoots;
                if (item != null)
                    exp.TryAddLoot(item);
            }
            exp.ForagerLootApplied = true;
        }

        private void EnsureForagerFoodItems()
        {
            if (_foragerRoots == null)
                _foragerRoots = CreateForagerFood(ExpeditionPerkSystem.RootsItemId, "Roots", 0.2f, 8f);
            if (_foragerBerries == null)
                _foragerBerries = CreateForagerFood(ExpeditionPerkSystem.BerriesItemId, "Berries", 0.15f, 6f);
        }

        private static ItemDefinition CreateForagerFood(string id, string display, float weight, float hunger)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = display;
            item.description = "Low-nutrition forage. Staves off starvation.";
            item.type = ItemType.Food;
            item.weight = weight;
            item.stackMax = 20;
            item.hungerRestore = hunger;
            item.tradeValue = 1f;
            return item;
        }

        private void TryAddLootItem(ExpeditionState exp)
        {
            var item = _itemCatalog.items[_rng.Next(_itemCatalog.items.Count)];
            if (item == null) return;

            // Prompt #13 — hostile factions may swap medical loot for poison.
            if (_sabotagedCaches != null && exp.Survivor != null)
            {
                _sabotagedCaches.RecordScavengeLoot(exp.TargetLocationId);
                var outcome = _sabotagedCaches.ProcessLootCandidate(
                    exp.Survivor, item, out var resultItem);
                if (outcome == SabotagedLootOutcome.DetectedAndDiscarded)
                {
                    OnSabotagedCacheDetected?.Invoke(exp,
                        "The seals are wrong. Left the crate.");
                    return;
                }
                if (outcome == SabotagedLootOutcome.Poisoned && resultItem != null)
                {
                    exp.TryAddLoot(resultItem);
                    OnSabotagedCachePlanted?.Invoke(exp);
                    return;
                }
            }

            exp.TryAddLoot(item);
        }

        private void TryFireForcedLocationEncounter(ExpeditionState exp)
        {
            if (exp == null || exp.LocationEncounterFired) return;
            var forced = FindForcedLocationEncounter(exp.TargetLocationId);
            if (forced == null) return;
            exp.LocationEncounterFired = true;
            OnEncounterTriggered?.Invoke(exp, forced);
            ResolveEncounterWithPsychology(exp, forced);
        }

        /// <summary>Test hook: force the location-bound / flag-driven arrival beat.</summary>
        public bool ForceFireLocationEncounterForTests(ExpeditionState exp)
        {
            if (exp == null) return false;
            exp.LocationEncounterFired = false;
            TryFireForcedLocationEncounter(exp);
            return exp.LocationEncounterFired;
        }

        private void RollAndResolveEncounter(ExpeditionState exp)
        {
            if (_encounterPool.Count == 0) return;

            // Base encounter chance per tick: 30% modified by danger level and stance
            float encounterChance = 0.25f + (exp.DangerLevel * 0.05f);
            if (exp.Stance == ExpeditionStance.Speed) encounterChance *= 1.4f;
            else if (exp.Stance == ExpeditionStance.Stealth) encounterChance *= 0.6f;

            if (_rng.NextDouble() >= encounterChance) return;

            // Location-filtered weighted pick (Prompt #47)
            EncounterSO selected = PickEncounter(exp.TargetLocationId, exp.Stance, exp.DangerLevel);
            if (selected == null) return;

            OnEncounterTriggered?.Invoke(exp, selected);

            // Psychological auto-resolution
            ResolveEncounterWithPsychology(exp, selected);
        }

        private void EnsureDeserterStandRifle()
        {
            if (_deserterStandRifle != null) return;
            // Prefer catalog entry if present
            if (_itemCatalog?.items != null)
            {
                for (int i = 0; i < _itemCatalog.items.Count; i++)
                {
                    var it = _itemCatalog.items[i];
                    if (it != null
                        && string.Equals(it.id, DesertersStandSystem.ServiceRifleItemId,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        _deserterStandRifle = it;
                        return;
                    }
                }
            }
            _deserterStandRifle = DesertersStandSystem.CreateServiceRifleDefinition();
        }

        /// <summary>
        /// Prompt #12 — if the target node has UXO and the scavenger is Reckless,
        /// roll detonation after a loot action.
        /// </summary>

        /// <summary>
        /// Prompt #12 — fleeing an encounter on a UXO node may trigger a mine.
        /// </summary>

        /// <summary>
        /// Test / scripted hook: force a UXO check with an explicit detonation decision.
        /// </summary>

    }
}
