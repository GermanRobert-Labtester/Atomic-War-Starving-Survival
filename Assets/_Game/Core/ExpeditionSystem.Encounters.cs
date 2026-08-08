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

            // Prompt #207 — Light Step completely bypasses Feral Dog / Sleeping Ghoul.
            if (TryLightStepBypass(exp, selected, survivor))
                return;

            bool fled;
            EventChoice chosen = TryAutoResolveByTrait(exp, selected, survivor, out fled);

            if (chosen == null)
                chosen = PickBeliefWeightedChoice(selected, survivor);

            fled = ApplyEncounterChoice(exp, survivor, chosen, fled);
            ApplyDesertersStandIfNeeded(exp, selected, survivor, chosen);
            ApplyCombatPerkMilestones(exp, selected, survivor, chosen, fled);
            ApplyExpeditionPerkEncounterMilestones(exp, selected, survivor, chosen, fled);
            ApplyAmmoResolveHitOnEngage(exp, selected, survivor, chosen, fled);
            ApplyBloodToxicityBiteRetaliation(exp, selected, survivor, chosen, fled);
            // REPROMOTE-Encounter-001 — class roadblock Engage/Resolve when map/SO tags match.
            TryDispatchClassRoadblock(exp, selected, chosen, fled);

            if (fled)
                TryProcessUxoFlee(exp);

            OnEncounterResolved?.Invoke(exp, selected, chosen);
        }

        /// <summary>
        /// When the encounter SO id, choice, or map node carries a roadblock tag,
        /// run <see cref="Encounter_Roadblock.ResolveChoice"/> so the class tracker
        /// is not a save-only ghost (REPROMOTE-Encounter-001).
        /// </summary>
        private void TryDispatchClassRoadblock(
            ExpeditionState exp,
            EncounterSO selected,
            EventChoice chosen,
            bool fled)
        {
            if (_classRoadblock == null || exp == null || fled) return;
            if (!IsClassRoadblockBeat(exp, selected)) return;

            RoadblockChoice rb = MapEventChoiceToRoadblock(chosen);
            int fuel = 0;
            if (_countItem != null)
            {
                fuel = Mathf.Max(_countItem("fuel"), _countItem("fuel_can"));
                if (fuel <= 0) fuel = _countItem("jerry_can");
            }

            float chassis = exp.HasBicycle ? exp.BicycleDurability : 100f;
            float hours;
            bool ok = _classRoadblock.ResolveChoice(rb, ref fuel, ref chassis, out hours);
            if (!ok && rb == RoadblockChoice.PayToll)
            {
                // Not enough fuel — fall back to reverse detour.
                _classRoadblock.ResolveChoice(
                    RoadblockChoice.ReverseDetour, ref fuel, ref chassis, out hours);
                rb = RoadblockChoice.ReverseDetour;
            }

            if (rb == RoadblockChoice.PayToll && _consumeItem != null)
            {
                int toll = _classRoadblock.State != null ? _classRoadblock.State.tollFuelCost : 5;
                if (!_consumeItem("fuel", toll))
                {
                    if (!_consumeItem("fuel_can", toll))
                        _consumeItem("jerry_can", toll);
                }
            }

            if (hours > 0f)
            {
                // Detour burns travel ticks (1 tick ≈ 1 hour in the expedition engine).
                int extra = Mathf.Max(1, Mathf.CeilToInt(hours));
                exp.TotalDistanceTicks += extra;
            }

            if (exp.HasBicycle)
                exp.BicycleDurability = Mathf.Clamp(chassis, 0f, 100f);
        }

        private bool IsClassRoadblockBeat(ExpeditionState exp, EncounterSO selected)
        {
            if (selected != null && !string.IsNullOrEmpty(selected.id))
            {
                string id = selected.id;
                if (id.IndexOf("roadblock", StringComparison.OrdinalIgnoreCase) >= 0) return true;
                if (id.IndexOf("barricade", StringComparison.OrdinalIgnoreCase) >= 0) return true;
                if (id.IndexOf("toll", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            }

            string loc = exp?.TargetLocationId;
            if (!string.IsNullOrEmpty(loc)
                && loc.IndexOf("roadblock", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            if (_generatedMap != null && !string.IsNullOrEmpty(loc))
            {
                var node = _generatedMap.GetNode(loc);
                if (node != null && node.HasTag("roadblock"))
                    return true;
            }

            return false;
        }

        private static RoadblockChoice MapEventChoiceToRoadblock(EventChoice chosen)
        {
            string id = chosen?.ChoiceId ?? string.Empty;
            if (id.IndexOf("pay", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("toll", StringComparison.OrdinalIgnoreCase) >= 0)
                return RoadblockChoice.PayToll;
            if (id.IndexOf("ram", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("force", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("engage", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("fight", StringComparison.OrdinalIgnoreCase) >= 0)
                return RoadblockChoice.RamBarricade;
            // detour / reverse / flee / sneak / default
            return RoadblockChoice.ReverseDetour;
        }

        /// <summary>
        /// Consume one bunker/carried ammo stack and ResolveHit against encounter armor.
        /// Strong hits grant a small morale bump; weak JHP-on-armor shots leave a health nick.
        /// </summary>
        private void ApplyAmmoResolveHitOnEngage(
            ExpeditionState exp, EncounterSO selected, Survivor survivor, EventChoice chosen, bool fled)
        {
            LastCombatShotDamage = 0f;
            LastCombatAmmoId = null;
            LastCombatMoraleDelta = 0f;
            LastCombatHealthDelta = 0f;
            LastCombatLogLine = null;
            LastCombatArmorPenalty = false;
            if (fled || _ammoTypes == null || survivor == null || selected == null) return;

            bool isCombat = selected.category == EncounterCategory.Combat;
            if (!isCombat) return;

            string choiceId = chosen?.ChoiceId ?? string.Empty;
            bool engaged = string.Equals(choiceId, "engage", StringComparison.OrdinalIgnoreCase)
                || string.Equals(choiceId, "fight", StringComparison.OrdinalIgnoreCase)
                || (!string.Equals(choiceId, "flee", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "sneak", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "pay", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "detour", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "light_step_bypass", StringComparison.OrdinalIgnoreCase));
            if (!engaged) return;

            string ammoId = TryConsumeBestCombatAmmo();
            if (string.IsNullOrEmpty(ammoId)) return;

            float armor = Item_AmmoTypes.InferEncounterArmor(selected.id);
            bool barrier = selected.id != null
                && selected.id.IndexOf("barricade", StringComparison.OrdinalIgnoreCase) >= 0;
            float baseDmg = 12f;
            if (Item_AmmoTypes.TryGetLoad(ammoId, out var load))
                baseDmg = load.BaseDamage;

            var hit = _ammoTypes.ResolveHit(ammoId, baseDmg, armor, barrier, rangeMeters: 40f);
            LastCombatShotDamage = hit.FinalDamage;
            LastCombatAmmoId = ammoId;
            LastCombatArmorPenalty = hit.ArmorPenaltyApplied;

            float moraleDelta = 0f;
            float healthDelta = 0f;

            if (hit.FinalDamage >= baseDmg * 0.9f)
            {
                // Clean terminal effect — brief morale lift.
                moraleDelta += 2f;
            }
            else if (hit.ArmorPenaltyApplied || hit.FinalDamage < baseDmg * 0.35f)
            {
                // Soft lead / JHP failed on armor — return fire nicks the scavenger.
                float nick = Mathf.Clamp(4f + (baseDmg - hit.FinalDamage) * 0.15f, 3f, 12f);
                healthDelta -= nick;
            }

            if (hit.BurnDamagePerSecond > 0f)
                moraleDelta += 1f;

            if (Mathf.Abs(moraleDelta) > 0.01f)
                survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + moraleDelta, 0f, 100f);
            if (Mathf.Abs(healthDelta) > 0.01f)
                SurvivorNeedWrite.AdjustHealth(survivor, healthDelta);

            LastCombatMoraleDelta = moraleDelta;
            LastCombatHealthDelta = healthDelta;
            LastCombatLogLine = Item_AmmoTypes.FormatCombatEncounterLog(
                survivor.DisplayName ?? survivor.Id,
                selected.id,
                ammoId,
                hit.FinalDamage,
                moraleDelta,
                healthDelta,
                hit.ArmorPenaltyApplied);
        }

        /// <summary>
        /// Prefer spending craftable ammo; falls back to any ammo_* / handgun / shells.
        /// </summary>
        private string TryConsumeBestCombatAmmo()
        {
            if (_inventory?.Slots == null) return null;

            int bestIdx = -1;
            int bestPri = int.MaxValue;
            for (int i = 0; i < _inventory.Slots.Count; i++)
            {
                var slot = _inventory.Slots[i];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                string id = slot.Item.id;
                if (!Item_AmmoTypes.IsAmmoItemId(id)) continue;
                int pri = Item_AmmoTypes.AmmoSpendPriority(id);
                if (pri < bestPri)
                {
                    bestPri = pri;
                    bestIdx = i;
                }
            }
            if (bestIdx < 0) return null;

            var pick = _inventory.Slots[bestIdx];
            string ammoId = pick.Item.id;
            _inventory.Remove(pick.Item, 1);
            return ammoId;
        }

        /// <summary>
        /// Prompt #551 — chem-toxic blood poisons feral dogs / cannibals that bite
        /// during an engaged combat encounter.
        /// </summary>
        private void ApplyBloodToxicityBiteRetaliation(
            ExpeditionState exp, EncounterSO selected, Survivor survivor, EventChoice chosen, bool fled)
        {
            LastBiteRetaliationDamage = 0f;
            if (fled || _bloodToxicity == null || survivor == null || selected == null) return;

            bool isCombat = selected.category == EncounterCategory.Combat;
            if (!isCombat) return;

            string choiceId = chosen?.ChoiceId ?? string.Empty;
            bool engaged = string.Equals(choiceId, "engage", StringComparison.OrdinalIgnoreCase)
                || string.Equals(choiceId, "fight", StringComparison.OrdinalIgnoreCase)
                || (!string.Equals(choiceId, "flee", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "sneak", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "pay", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "detour", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(choiceId, "light_step_bypass", StringComparison.OrdinalIgnoreCase));
            if (!engaged) return;

            string attackerType = ResolveBiteAttackerType(selected);
            if (attackerType == null) return;

            float dmg = _bloodToxicity.GetBiteRetaliationDamage(survivor.Id, attackerType);
            if (dmg <= 0f) return;

            LastBiteRetaliationDamage = dmg;
            // Grim win: poisoned attacker breaks off — small morale relief.
            survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + 3f, 0f, 100f);
        }

        private static string ResolveBiteAttackerType(EncounterSO selected)
        {
            if (selected == null) return null;
            string id = selected.id ?? string.Empty;
            if (id.IndexOf("feral_dog", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("feraldog", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("feral_dogs", StringComparison.OrdinalIgnoreCase) >= 0)
                return "feral_dog";
            if (id.IndexOf("cannibal", StringComparison.OrdinalIgnoreCase) >= 0)
                return "cannibal";
            return null;
        }

        /// <summary>
        /// Prompt #207 — Light Step: skip FeralDog / SleepingGhoul with no stealth roll.
        /// Does not raise a hostile trigger; logs a silent bypass resolution only.
        /// </summary>
        private bool TryLightStepBypass(ExpeditionState exp, EncounterSO selected, Survivor survivor)
        {
            if (_expeditionPerks == null || survivor == null || selected == null) return false;
            if (!_expeditionPerks.CanBypassEncounter(survivor, selected.id)) return false;

            var bypass = new EventChoice
            {
                ChoiceId = "light_step_bypass",
                Text = "Slip past unseen.",
                MoraleDelta = 0f
            };
            // Silent pass — no OnEncounterTriggered (would surface as a combat beat).
            OnEncounterResolved?.Invoke(exp, selected, bypass);
            return true;
        }

        /// <summary>Prompt #207 — sneak successes count toward Light Step.</summary>
        private void ApplyExpeditionPerkEncounterMilestones(
            ExpeditionState exp, EncounterSO selected, Survivor survivor, EventChoice chosen, bool fled)
        {
            if (_expeditionPerks == null || survivor == null || !survivor.IsAlive) return;
            if (fled || chosen == null) return;

            int day = _getDay != null ? _getDay() : 0;
            string choiceId = chosen.ChoiceId ?? string.Empty;
            if (string.Equals(choiceId, "sneak", StringComparison.OrdinalIgnoreCase)
                || string.Equals(choiceId, "sneak_past", StringComparison.OrdinalIgnoreCase))
            {
                _expeditionPerks.RecordSneakPast(survivor, day);
            }
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
            {
                float moraleDelta = chosen.MoraleDelta;
                // Prompt #209 — Night Terror: combat success at night boosts morale gains
                // from aggressive choices (engage/fight) by the combat bonus factor.
                if (_expeditionPerks != null && exp != null && exp.IsNightScavenge
                    && moraleDelta > 0f
                    && (string.Equals(chosen.ChoiceId, "engage", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(chosen.ChoiceId, "fight", StringComparison.OrdinalIgnoreCase)))
                {
                    float combat = _expeditionPerks.GetNightCombatMultiplier(survivor, isNight: true);
                    moraleDelta *= combat;
                }
                survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + moraleDelta, 0f, 100f);
            }

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
