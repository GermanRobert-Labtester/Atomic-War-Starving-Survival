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
            // Prompt #47 — location-bound forceOnArrival encounters
            TryFireForcedLocationEncounter(exp);
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
