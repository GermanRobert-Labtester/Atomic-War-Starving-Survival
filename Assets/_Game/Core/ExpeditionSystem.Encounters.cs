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
        private void ResolveEncounterWithPsychology(ExpeditionState exp, EncounterSO selected)
        {
            var survivor = exp.Survivor;
            bool fled;
            EventChoice chosen = TryAutoResolveByTrait(exp, selected, survivor, out fled);

            if (chosen == null)
                chosen = PickBeliefWeightedChoice(selected, survivor);

            fled = ApplyEncounterChoice(exp, survivor, chosen, fled);
            ApplyDesertersStandIfNeeded(exp, selected, survivor, chosen);
            ApplyCombatPerkMilestones(exp, selected, survivor, chosen, fled);

            if (fled)
                TryProcessUxoFlee(exp);

            OnEncounterResolved?.Invoke(exp, selected, chosen);
        }

        /// <summary>
        /// Prompts #183–#188: stealth kills, confined fights, flees, human kills, cold bore.
        /// </summary>
        private void ApplyCombatPerkMilestones(
            ExpeditionState exp, EncounterSO selected, Survivor survivor, EventChoice chosen, bool fled)
        {
            if (_combatPerks == null || survivor == null || !survivor.IsAlive) return;
            int day = _getDay != null ? _getDay() : 0;

            if (fled)
            {
                _combatPerks.RecordFlee(survivor, day);
                return;
            }

            bool isCombat = selected != null && selected.category == EncounterCategory.Combat;
            string choiceId = chosen?.ChoiceId ?? string.Empty;
            bool engaged = isCombat && (
                string.Equals(choiceId, "engage", StringComparison.OrdinalIgnoreCase)
                || string.Equals(choiceId, "fight", StringComparison.OrdinalIgnoreCase)
                || (chosen != null && !string.Equals(choiceId, "flee", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "sneak", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "pay", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "detour", StringComparison.OrdinalIgnoreCase)));

            // Urban / subway confined milestone (#185)
            var node = ResolveMapNode(exp?.TargetLocationId);
            if (node != null && node.IsUrbanOrSubway && isCombat)
                _combatPerks.RecordConfinedEncounterSurvived(survivor, day);

            if (!engaged) return;

            // Stealth kill (#183)
            if (exp != null && exp.Stance == ExpeditionStance.Stealth && isCombat)
            {
                _combatPerks.RecordStealthKill(survivor, day);
                // Cold Bore first-shot crit roll (informational / combat outcome hook)
                string encKey = (selected?.id ?? "enc") + "|" + (exp.ExpeditionId ?? "");
                _combatPerks.RollFirstShotCrit(survivor, encKey, _rng);
            }

            // Human NPC kill (#188) — combat deserters / looters
            if (isCombat && IsHumanHostileEncounter(selected))
            {
                var all = _getAllSurvivors?.Invoke();
                _combatPerks.RecordHumanKill(survivor, day, all, _affinity);
                _combatPerks.ApplyHumanKillMorale(survivor);
            }

            // Close Quarters damage mult available to callers via GetCloseQuartersDamageMultiplier
            // when node is confined (breach handled in HatchDefense).
        }

        private static bool IsHumanHostileEncounter(EncounterSO selected)
        {
            if (selected == null) return false;
            string id = selected.id ?? string.Empty;
            return id.IndexOf("deserter", StringComparison.OrdinalIgnoreCase) >= 0
                   || id.IndexOf("looter", StringComparison.OrdinalIgnoreCase) >= 0
                   || id.IndexOf("raider", StringComparison.OrdinalIgnoreCase) >= 0
                   || id.IndexOf("bandit", StringComparison.OrdinalIgnoreCase) >= 0
                   || string.Equals(id, "enc_deserters", StringComparison.Ordinal);
        }

        private MapNode ResolveMapNode(string locationId)
        {
            if (string.IsNullOrEmpty(locationId) || _generatedMap?.Nodes == null) return null;
            for (int i = 0; i < _generatedMap.Nodes.Count; i++)
            {
                var n = _generatedMap.Nodes[i];
                if (n != null && string.Equals(n.NodeId, locationId, StringComparison.Ordinal))
                    return n;
            }
            return null;
        }

        private EventChoice TryAutoResolveByTrait(
            ExpeditionState exp, EncounterSO selected, Survivor survivor, out bool fled)
        {
            fled = false;
            if (!selected.enableAutoResolution || survivor == null) return null;

            if (survivor.RiskBias == selected.autoEngageTrait)
            {
                // Reckless: engage directly
                PerformLootRoll(exp);
                TryProcessUxoLoot(exp);
                return selected.choices != null && selected.choices.Count > 0
                    ? selected.choices[0]
                    : null;
            }

            if (survivor.RiskBias == selected.autoFleeTrait || survivor.HasRadiationAnxietyStatus)
            {
                ApplyFleeLootDrop(exp, survivor);
                exp.Phase = ExpeditionPhase.Inbound;
                fled = true;
                return selected.choices != null && selected.choices.Count > 1
                    ? selected.choices[selected.choices.Count - 1]
                    : null;
            }

            return null;
        }

        private EventChoice PickBeliefWeightedChoice(EncounterSO selected, Survivor survivor)
        {
            if (selected.choices == null || selected.choices.Count == 0) return null;
            var eventContext = new EventContext(survivor, null, _inventory, _rng);
            var gameEvent = ScriptableObject.CreateInstance<GameEvent>();
            gameEvent.choices = selected.choices;
            return EventRunner.PickWeightedChoice(gameEvent, eventContext, _rng);
        }

        private bool ApplyEncounterChoice(
            ExpeditionState exp, Survivor survivor, EventChoice chosen, bool alreadyFled)
        {
            if (chosen == null) return alreadyFled;

            if (chosen.MoraleDelta != 0f && survivor != null)
                survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + chosen.MoraleDelta, 0f, 100f);

            // Explicit flee choice (ChoiceId "flee") on a UXO node can still detonate.
            if (!alreadyFled && string.Equals(chosen.ChoiceId, "flee", StringComparison.OrdinalIgnoreCase))
            {
                ApplyFleeLootDrop(exp, survivor);
                exp.Phase = ExpeditionPhase.Inbound;
                return true;
            }

            return alreadyFled;
        }

        /// <summary>Prompt #187 — Looter's Reflex keeps the single best item on flee.</summary>
        private void ApplyFleeLootDrop(ExpeditionState exp, Survivor survivor)
        {
            if (exp == null) return;
            if (_combatPerks == null || exp.CollectedLoot == null || exp.CollectedLoot.Count == 0)
            {
                exp.DropLoot(0.5f);
                return;
            }

            var loot = exp.CollectedLoot;
            var ids = exp.CollectedLootItemIds;
            var dropIndices = _combatPerks.ComputeFleeDropIndices(
                survivor,
                loot.Count,
                i => loot[i] != null ? loot[i].tradeValue : 0f,
                i => loot[i] != null ? loot[i].weight : 0f,
                defaultDropFraction: 0.5f);

            // Indices are already descending for the perk path; sort to be safe.
            dropIndices.Sort((a, b) => b.CompareTo(a));
            for (int d = 0; d < dropIndices.Count; d++)
            {
                int idx = dropIndices[d];
                if (idx < 0 || idx >= loot.Count) continue;
                loot.RemoveAt(idx);
                if (ids != null && idx < ids.Count)
                    ids.RemoveAt(idx);
            }

            exp.RecalculateWeight();
        }

        private void ApplyDesertersStandIfNeeded(
            ExpeditionState exp, EncounterSO selected, Survivor survivor, EventChoice chosen)
        {
            if (!DesertersStandSystem.IsDesertersStandEncounter(selected) || survivor == null) return;
            EnsureDeserterStandRifle();
            string choiceId = chosen != null ? chosen.ChoiceId : "gather_the_weapons";
            DesertersStandSystem.Apply(exp, survivor, _deserterStandRifle, choiceId);
            OnDesertersStandResolved?.Invoke(exp, DesertersStandSystem.LogMessage);
            Debug.Log($"[Deserter's Stand] {DesertersStandSystem.LogMessage}");
        }


        /// <summary>
        /// First forceOnArrival encounter bound to this location (highest weight wins).
        /// Also returns Deserter's Stand when the proc-gen node is flagged (Prompt #15).
        /// </summary>
        public EncounterSO FindForcedLocationEncounter(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            EncounterSO best = null;
            float bestW = -1f;
            for (int i = 0; i < _encounterPool.Count; i++)
            {
                var enc = _encounterPool[i];
                if (!IsForcedEncounterCandidate(enc, locationId)) continue;
                float w = ResolveForcedEncounterWeight(enc, locationId);
                if (w <= bestW) continue;
                bestW = w;
                best = enc;
            }
            return best;
        }

        private bool IsForcedEncounterCandidate(EncounterSO enc, string locationId)
        {
            if (enc == null || !enc.forceOnArrival) return false;
            // Location-bound (Safe Haven etc.)
            if (!string.IsNullOrEmpty(enc.requiredLocationId))
                return string.Equals(enc.requiredLocationId, locationId, StringComparison.Ordinal);
            // Flag-driven narrative (Deserter's Stand): only if node tagged.
            return DesertersStandSystem.IsDesertersStandEncounter(enc)
                   && DesertersStandSystem.NodeHasStand(_generatedMap, locationId);
        }

        private static float ResolveForcedEncounterWeight(EncounterSO enc, string locationId)
        {
            float w = enc.GetEffectiveWeight(ExpeditionStance.Stealth, 99f, locationId);
            // Flag-driven encounters have empty requiredLocationId — weight still applies.
            bool isStand = string.IsNullOrEmpty(enc.requiredLocationId)
                           && DesertersStandSystem.IsDesertersStandEncounter(enc);
            return isStand ? Mathf.Max(w, enc.baseWeight) : w;
        }
    }
}
