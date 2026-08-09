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
        private void ProcessSingleTick(float tickHours)
        {
            for (int i = _activeExpeditions.Count - 1; i >= 0; i--)
            {
                var exp = _activeExpeditions[i];
                if (exp == null || exp.Phase == ExpeditionPhase.Completed || exp.Phase == ExpeditionPhase.Failed)
                    continue;

                if (!EnsureSurvivorAlive(exp, i))
                    continue;

                exp.CurrentTick++;
                ApplyStaminaAndFatigue(exp, tickHours);
                ApplyRadiationExposure(exp, tickHours);
                ApplyBicycleAndFloodedTick(exp, tickHours);

                if (AdvanceExpeditionPhase(exp, i))
                    continue;

                RollAndResolveEncounter(exp);
                OnExpeditionTick?.Invoke(exp);
            }
        }

        private bool EnsureSurvivorAlive(ExpeditionState exp, int index)
        {
            if (exp.Survivor != null && exp.Survivor.IsAlive) return true;
            exp.Phase = ExpeditionPhase.Failed;
            OnExpeditionFailed?.Invoke(exp, "Survivor died during expedition.");
            _activeExpeditions.RemoveAt(index);
            return false;
        }

        private void ApplyStaminaAndFatigue(ExpeditionState exp, float tickHours)
        {
            float staminaDrain = CalculateStaminaDrain(exp, tickHours);
            exp.Stamina = Mathf.Clamp(exp.Stamina - staminaDrain, 0f, 100f);
            if (_needsSystem != null)
                _needsSystem.Modify(exp.Survivor, NeedKind.Fatigue, staminaDrain * 0.5f);
            else
                exp.Survivor.Needs.Fatigue = Mathf.Clamp(exp.Survivor.Needs.Fatigue + staminaDrain * 0.5f, 0f, 100f);

            if (exp.Stamina > 0f) return;
            // Exhaustion penalty: drop half loot, take health hit
            exp.DropLoot(0.5f);
            var s = exp.Survivor;
            if (s?.Needs == null) return;
            // MISC-006 — do not leave Health at 0 while State stays Alive.
            SurvivorNeedWrite.AdjustHealth(s, -5f);
        }

        private void ApplyRadiationExposure(ExpeditionState exp, float tickHours)
        {
            float radRate = exp.TrueRadPerHour;
            if (_weatherSystem != null)
                radRate += _weatherSystem.OutdoorRadModifier;

            if (exp.Phase == ExpeditionPhase.Looting)
            {
                radRate *= 1f + (exp.LootingTicksCompleted * 0.15f);
                MaybeTriggerPushYourLuckStorm(exp);
            }

            _radSystem?.Expose(exp.Survivor, radRate, tickHours);
        }

        private void MaybeTriggerPushYourLuckStorm(ExpeditionState exp)
        {
            if (_weatherSystem == null || _weatherSystem.Current == WeatherKind.FalloutStorm) return;
            float stormChance = 0.02f + (exp.LootingTicksCompleted * 0.05f);
            if (_rng.NextDouble() < stormChance)
                _weatherSystem.ForceWeather(WeatherKind.FalloutStorm);
        }

        private bool AdvanceExpeditionPhase(ExpeditionState exp, int index)
        {
            switch (exp.Phase)
            {
                case ExpeditionPhase.Outbound:
                    AdvanceOutbound(exp);
                    return false;
                case ExpeditionPhase.Looting:
                    return AdvanceLooting(exp, index);
                case ExpeditionPhase.Inbound:
                    return AdvanceInbound(exp, index);
                default:
                    return false;
            }
        }

        private void AdvanceOutbound(ExpeditionState exp)
        {
            float travelStep = exp.Stance == ExpeditionStance.Speed ? 1.5f : 1.0f;
            exp.TravelTicksCompleted += Mathf.RoundToInt(travelStep);
            if (exp.TravelTicksCompleted < exp.TotalDistanceTicks) return;

            // First arrival: mark proc-gen node visited + reveal fog-of-war
            _generatedMap?.MarkVisited(exp.TargetLocationId);
            exp.Phase = ExpeditionPhase.Looting;
            // Prompt #69 — flooded subway / ruins wading or pump drain.
            _floodedNodeSystem?.ProcessFloodedArrival(exp, _hasItem);
            // REPROMOTE-MapHazard-001 — swamp berry bushes may be carnivorous plants.
            TryVenusTrapOnLootingArrival(exp);
            // Prompt #902 — a shape in the snow that might still be breathing.
            TryFrozenSurvivorOnLootingArrival(exp);
            // REPROMOTE-Item-001 — secure doors / keycard nodes on arrival.
            TryKeycardDoorOnLootingArrival(exp);
            // Prompt #47 — location-bound forceOnArrival encounters
            TryFireForcedLocationEncounter(exp);
        }

        /// <summary>
        /// On keycard_door / secure military nodes: report a found card (if inventory
        /// carries one) and attempt TryOpenDoor for the door color implied by the node.
        /// </summary>
        private void TryKeycardDoorOnLootingArrival(ExpeditionState exp)
        {
            if (_keycards == null || exp?.Survivor == null || !exp.Survivor.IsAlive) return;

            string nodeId = exp.TargetLocationId;
            if (!IsKeycardDoorNode(nodeId)) return;

            KeycardColor required = InferRequiredKeycard(nodeId);
            var owned = CollectOwnedKeycards(exp.Survivor.Id);

            // Field loot: if no matching card yet, finding one on a secure node is the fiction.
            if (!owned.Contains(required))
            {
                _keycards.ReportKeycardFound(exp.Survivor.Id, required);
                owned.Add(required);
            }

            _keycards.TryOpenDoor(exp.Survivor.Id, required, owned);
        }

        private bool IsKeycardDoorNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            if (nodeId.IndexOf("keycard", System.StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            if (_generatedMap == null) return false;
            var node = _generatedMap.GetNode(nodeId);
            if (node == null) return false;
            if (node.HasTag("keycard_door") || node.HasTag("secure") || node.HasTag("military"))
                return true;
            string name = node.DisplayName ?? string.Empty;
            return name.IndexOf("hangar", System.StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("command", System.StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("silo", System.StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("bunker rim", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static KeycardColor InferRequiredKeycard(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return KeycardColor.Green;
            if (nodeId.IndexOf("ground_zero", System.StringComparison.OrdinalIgnoreCase) >= 0
                || nodeId.IndexOf("silo", System.StringComparison.OrdinalIgnoreCase) >= 0)
                return KeycardColor.Red;
            if (nodeId.IndexOf("command", System.StringComparison.OrdinalIgnoreCase) >= 0
                || nodeId.IndexOf("hangar", System.StringComparison.OrdinalIgnoreCase) >= 0)
                return KeycardColor.Blue;
            return KeycardColor.Green;
        }

        private List<KeycardColor> CollectOwnedKeycards(string survivorId)
        {
            var owned = new List<KeycardColor>();
            if (_keycards == null) return owned;
            // Cards already reported found on this tracker (save-backed).
            var found = _keycards.FoundCardIds;
            if (found != null)
            {
                for (int i = 0; i < found.Count; i++)
                {
                    string id = found[i];
                    if (id != null && id.IndexOf("red", System.StringComparison.OrdinalIgnoreCase) >= 0)
                        owned.Add(KeycardColor.Red);
                    else if (id != null && id.IndexOf("blue", System.StringComparison.OrdinalIgnoreCase) >= 0)
                        owned.Add(KeycardColor.Blue);
                    else if (id != null && id.IndexOf("green", System.StringComparison.OrdinalIgnoreCase) >= 0)
                        owned.Add(KeycardColor.Green);
                }
            }
            // Inventory physical keycard items if host wired count.
            if (_countItem != null)
            {
                if (_countItem("item_keycard_red") > 0 && !owned.Contains(KeycardColor.Red))
                    owned.Add(KeycardColor.Red);
                if (_countItem("item_keycard_blue") > 0 && !owned.Contains(KeycardColor.Blue))
                    owned.Add(KeycardColor.Blue);
                if (_countItem("item_keycard_green") > 0 && !owned.Contains(KeycardColor.Green))
                    owned.Add(KeycardColor.Green);
            }
            return owned;
        }

        /// <summary>
        /// When the target node is tagged swamp, arm VenusTrap and run a harvest
        /// strength check (Navigate/loot "berry bush" fiction).
        /// </summary>
        private void TryVenusTrapOnLootingArrival(ExpeditionState exp)
        {
            if (_venusTrap == null || exp?.Survivor == null || !exp.Survivor.IsAlive) return;

            string nodeId = exp.TargetLocationId;
            bool swamp = false;
            if (_generatedMap != null && !string.IsNullOrEmpty(nodeId))
            {
                var node = _generatedMap.GetNode(nodeId);
                if (node != null && node.HasTag("swamp"))
                    swamp = true;
            }
            if (!swamp && !string.IsNullOrEmpty(nodeId)
                && nodeId.IndexOf("swamp", System.StringComparison.OrdinalIgnoreCase) >= 0)
                swamp = true;
            if (!swamp) return;

            _venusTrap.EnterNode(nodeId);

            var sv = exp.Survivor;
            // Perception: morale proxy for fieldcraft (no separate skill on every build).
            float perception = UnityEngine.Mathf.Clamp01(sv.Needs.Morale / 100f);
            if (_venusTrap.CheckDisguise(sv.Id, perception))
                return; // spotted — no snap

            // Strength: inverse of fatigue (exhausted scavengers lose arms).
            float strength = UnityEngine.Mathf.Clamp01(1f - (sv.Needs.Fatigue / 100f));
            _venusTrap.AttemptHarvest(sv.Id, strength);
        }

        /// <summary>
        /// Prompt #902 — a body in the ash that might not be a body. Arms on looting
        /// arrival during a blizzard, or on nodes explicitly tagged snowfield /
        /// frozen_lake. The scavenger is out there alone, so the choice resolves on
        /// their behalf: attempt the rescue only when there is body heat to spare,
        /// otherwise walk away and carry it.
        /// </summary>
        private void TryFrozenSurvivorOnLootingArrival(ExpeditionState exp)
        {
            if (_frozenSurvivor == null || exp?.Survivor == null || !exp.Survivor.IsAlive) return;

            string nodeId = exp.TargetLocationId;
            bool frozenGround =
                _weatherSystem != null && _weatherSystem.Current == WeatherKind.Blizzard;
            if (!frozenGround && _generatedMap != null && !string.IsNullOrEmpty(nodeId))
            {
                var node = _generatedMap.GetNode(nodeId);
                if (node != null && (node.HasTag("snowfield") || node.HasTag("frozen_lake")))
                    frozenGround = true;
            }
            if (!frozenGround) return;

            // Returns false once this node's encounter is spent, so revisits stay quiet.
            if (!_frozenSurvivor.EnterNode(nodeId)) return;

            var sv = exp.Survivor;
            // Perception: morale proxy for fieldcraft, same basis as the venus trap check.
            float perception = UnityEngine.Mathf.Clamp01(sv.Needs.Morale / 100f);
            if (!_frozenSurvivor.CheckForSignsOfLife(perception))
            {
                // As far as they can tell it is just another corpse in the drift.
                ApplyMoraleDelta(sv, -_frozenSurvivor.LootCorpse());
                return;
            }

            // Warming someone else costs heat this scavenger may not be able to lose.
            float warmthCost = _frozenSurvivor.GetRescueWarmthCost();
            if (sv.Needs.Warmth <= warmthCost)
            {
                ApplyMoraleDelta(sv, -_frozenSurvivor.WalkAway());
                return;
            }

            if (_needsSystem != null)
                _needsSystem.Modify(sv, NeedKind.Warmth, -warmthCost);

            // AttemptRescue reads medical skill on a 0-100 scale; Survivor stores 0-1.
            bool rescued = _frozenSurvivor.AttemptRescue(sv.EffectiveMedicalSkill * 100f);
            ApplyMoraleDelta(sv, rescued
                ? _frozenSurvivor.GetRescueSuccessMoraleBoost()
                : -_frozenSurvivor.GetRescueFailMoraleHit());
        }

        private void ApplyMoraleDelta(Survivor sv, float delta)
        {
            if (sv == null || Mathf.Approximately(delta, 0f)) return;
            if (_needsSystem != null)
                _needsSystem.Modify(sv, NeedKind.Morale, delta);
        }

        private void ApplyBicycleAndFloodedTick(ExpeditionState exp, float tickHours)
        {
            if (exp == null || tickHours <= 0f) return;

            if (_bicycleSystem != null
                && (exp.Phase == ExpeditionPhase.Outbound || exp.Phase == ExpeditionPhase.Inbound))
            {
                var weather = _weatherSystem != null ? _weatherSystem.Current : WeatherKind.Clear;
                _bicycleSystem.TickBicycle(exp, tickHours, weather);
            }

            _floodedNodeSystem?.TickWading(exp, tickHours);
        }

        private bool AdvanceLooting(ExpeditionState exp, int index)
        {
            exp.LootingTicksCompleted++;
            PerformLootRoll(exp);

            // Prompt #12 — Reckless loot on a UXO node may detonate a mine.
            if (TryProcessUxoLoot(exp))
            {
                if (exp.Phase == ExpeditionPhase.Failed)
                {
                    OnExpeditionFailed?.Invoke(exp, UxoHazardSystem.DetonationLogMessage);
                    _activeExpeditions.RemoveAt(index);
                    return true;
                }
                OnExpeditionTick?.Invoke(exp);
                return true;
            }

            MaybeAutoRetreatFromLooting(exp);
            return false;
        }

        private static void MaybeAutoRetreatFromLooting(ExpeditionState exp)
        {
            var survivor = exp.Survivor;
            if (survivor.RiskBias == RiskBiasTrait.Paranoid || survivor.HasRadiationAnxietyStatus)
            {
                if (exp.LootingTicksCompleted >= 2 || survivor.RadiationAnxiety > 0.6f)
                {
                    exp.DropLoot(0.3f);
                    exp.Phase = ExpeditionPhase.Inbound;
                }
                return;
            }

            if (survivor.RiskBias == RiskBiasTrait.Cautious && exp.Stamina < 30f)
            {
                exp.Phase = ExpeditionPhase.Inbound;
                return;
            }

            if (!exp.IsPushingLuck && exp.LootingTicksCompleted >= 3)
                exp.Phase = ExpeditionPhase.Inbound;
        }

        private bool AdvanceInbound(ExpeditionState exp, int index)
        {
            // Cautious shelter delay: pause the return until the shelter timer counts down.
            if (exp.shelterDelayTicksRemaining > 0)
            {
                exp.shelterDelayTicksRemaining--;
                OnExpeditionTick?.Invoke(exp);
                return true;
            }

            float baseReturnStep = exp.Stance == ExpeditionStance.Speed ? 1.5f : 1.0f;
            float returnStep = baseReturnStep * exp.returnSpeedMultiplier / Mathf.Max(0.01f, exp.returnSpeedDivisor);
            exp.TravelTicksCompleted -= Mathf.RoundToInt(returnStep);
            if (exp.TravelTicksCompleted > 0) return false;

            // Comms-severed expeditions pause at the hatch and fire the dilemma.
            if (exp.isCommsSevered)
            {
                exp.Phase = ExpeditionPhase.AtHatchDilemma;
                bool alive = exp.Survivor != null && exp.Survivor.IsAlive;
                EventBus.Raise(new HatchDilemmaReadySignal(exp, alive));
                OnHatchDilemmaReady?.Invoke(exp);
                return true;
            }

            CompleteExpedition(exp);
            _activeExpeditions.RemoveAt(index);
            return true;
        }

    }
}
