// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Overall performance condition band across all needs.
    /// </summary>
    public enum PerformanceBand
    {
        Optimal = 0,
        Impaired = 1,
        Severe = 2,
        Critical = 3
    }

    /// <summary>
    /// Single need contributor to the performance cascade.
    /// </summary>
    public sealed class NeedPerformanceContribution
    {
        public NeedKind Need { get; }
        public float RawValue { get; }
        public float NormalizedSeverity { get; }
        public PerformanceBand Band { get; }
        public float CombatAccuracyMod { get; }
        public float CombatDamageMod { get; }
        public float WorkSpeedMod { get; }
        public float ExpeditionSpeedMod { get; }
        public float StaminaDrainMod { get; }
        public string ReasonKey { get; }

        public NeedPerformanceContribution(
            NeedKind need,
            float rawValue,
            float normalizedSeverity,
            PerformanceBand band,
            float combatAccuracyMod,
            float combatDamageMod,
            float workSpeedMod,
            float expeditionSpeedMod,
            float staminaDrainMod,
            string reasonKey)
        {
            Need = need;
            RawValue = rawValue;
            NormalizedSeverity = normalizedSeverity;
            Band = band;
            CombatAccuracyMod = combatAccuracyMod;
            CombatDamageMod = combatDamageMod;
            WorkSpeedMod = workSpeedMod;
            ExpeditionSpeedMod = expeditionSpeedMod;
            StaminaDrainMod = staminaDrainMod;
            ReasonKey = reasonKey;
        }
    }

    /// <summary>
    /// Composite performance multipliers resulting from the needs cascade.
    /// Pure projection model — does not own or mutate survivor state.
    /// </summary>
    public sealed class NeedsPerformanceModifiers
    {
        public float CombatAccuracyMultiplier { get; }
        public float CombatDamageMultiplier { get; }
        public float WorkSpeedMultiplier { get; }
        public float ExpeditionSpeedMultiplier { get; }
        public float ExpeditionStaminaDrainMultiplier { get; }
        public PerformanceBand OverallBand { get; }
        public IReadOnlyList<NeedPerformanceContribution> Contributions { get; }

        public static NeedsPerformanceModifiers Neutral { get; } = new NeedsPerformanceModifiers(
            1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
            PerformanceBand.Optimal,
            Array.Empty<NeedPerformanceContribution>());

        public NeedsPerformanceModifiers(
            float combatAccuracyMultiplier,
            float combatDamageMultiplier,
            float workSpeedMultiplier,
            float expeditionSpeedMultiplier,
            float expeditionStaminaDrainMultiplier,
            PerformanceBand overallBand,
            IReadOnlyList<NeedPerformanceContribution> contributions)
        {
            CombatAccuracyMultiplier = combatAccuracyMultiplier;
            CombatDamageMultiplier = combatDamageMultiplier;
            WorkSpeedMultiplier = workSpeedMultiplier;
            ExpeditionSpeedMultiplier = expeditionSpeedMultiplier;
            ExpeditionStaminaDrainMultiplier = expeditionStaminaDrainMultiplier;
            OverallBand = overallBand;
            Contributions = contributions ?? Array.Empty<NeedPerformanceContribution>();
        }
    }

    public struct NeedsPerformanceCensus
    {
        public readonly int OptimalCount;
        public readonly int ImpairedCount;
        public readonly int SevereCount;
        public readonly int CriticalCount;

        public NeedsPerformanceCensus(int optimal, int impaired, int severe, int critical)
        {
            OptimalCount = optimal;
            ImpairedCount = impaired;
            SevereCount = severe;
            CriticalCount = critical;
        }

        public int TotalSurvivorsEvaluated => OptimalCount + ImpairedCount + SevereCount + CriticalCount;
    }

    /// <summary>
    /// Plan 137 — Contract for combat systems (e.g. TacticalCombatSystem) to query needs-derived combat modifiers.
    /// </summary>
    public interface ICombatPerformanceModifier
    {
        (float accuracy, float damage) GetCombatModifiers(string survivorId);
    }

    /// <summary>
    /// Plan 137 — Contract for work assignment systems (e.g. DutyRosterSystem) to query needs-derived work speed modifiers.
    /// </summary>
    public interface IWorkEfficiencyModifier
    {
        float GetWorkSpeedModifier(string survivorId);
    }

    /// <summary>
    /// Plan 137 — Contract for expedition systems (e.g. ExpeditionSystem) to query needs-derived travel & stamina modifiers.
    /// </summary>
    public interface IExpeditionPerformanceModifier
    {
        float GetExpeditionSpeedModifier(string survivorId);
        float GetExpeditionStaminaDrainModifier(string survivorId);
    }

    /// <summary>
    /// Configuration data loaded from needs_performance.json or seeded with defaults.
    /// </summary>
    public sealed class NeedsPerformanceConfig
    {
        public float MinCombatAccuracyMultiplier { get; set; } = 0.25f;
        public float MinCombatDamageMultiplier { get; set; } = 0.30f;
        public float MinWorkSpeedMultiplier { get; set; } = 0.20f;
        public float MinExpeditionSpeedMultiplier { get; set; } = 0.30f;
        public float MaxExpeditionStaminaDrainMultiplier { get; set; } = 2.50f;

        public float DemoralizedThreshold { get; set; } = 30.0f;
        public float DemoralizedPenaltyAmplification { get; set; } = 0.10f;
        public float HighMoraleThreshold { get; set; } = 70.0f;
        public float HighMoralePenaltyMitigation { get; set; } = 0.10f;

        public static NeedsPerformanceConfig Default { get; } = new NeedsPerformanceConfig();
    }

    /// <summary>
    /// Pure functional query bridge projecting survivor needs (hunger, thirst, fatigue, warmth, morale)
    /// into domain-specific performance multipliers for combat accuracy/damage, work speed,
    /// and expedition travel/stamina.
    /// Engine-agnostic; no UnityEngine / Godot dependencies.
    /// </summary>
    public static class NeedsPerformanceBridge
    {
        private static NeedsPerformanceConfig s_config = NeedsPerformanceConfig.Default;

        public static NeedsPerformanceConfig ActiveConfig => s_config;

        public static void SetConfig(NeedsPerformanceConfig config)
        {
            s_config = config ?? NeedsPerformanceConfig.Default;
        }

        public static void LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                var cfg = new NeedsPerformanceConfig();

                if (root.TryGetProperty("floors", out var floors))
                {
                    if (floors.TryGetProperty("min_combat_accuracy_multiplier", out var ca))
                        cfg.MinCombatAccuracyMultiplier = (float)ca.GetDouble();
                    if (floors.TryGetProperty("min_combat_damage_multiplier", out var cd))
                        cfg.MinCombatDamageMultiplier = (float)cd.GetDouble();
                    if (floors.TryGetProperty("min_work_speed_multiplier", out var ws))
                        cfg.MinWorkSpeedMultiplier = (float)ws.GetDouble();
                    if (floors.TryGetProperty("min_expedition_speed_multiplier", out var es))
                        cfg.MinExpeditionSpeedMultiplier = (float)es.GetDouble();
                    if (floors.TryGetProperty("max_expedition_stamina_drain_multiplier", out var sd))
                        cfg.MaxExpeditionStaminaDrainMultiplier = (float)sd.GetDouble();
                }

                if (root.TryGetProperty("morale_interaction", out var morale))
                {
                    if (morale.TryGetProperty("demoralized_threshold", out var dt))
                        cfg.DemoralizedThreshold = (float)dt.GetDouble();
                    if (morale.TryGetProperty("demoralized_penalty_amplification", out var dpa))
                        cfg.DemoralizedPenaltyAmplification = (float)dpa.GetDouble();
                    if (morale.TryGetProperty("high_morale_threshold", out var hmt))
                        cfg.HighMoraleThreshold = (float)hmt.GetDouble();
                    if (morale.TryGetProperty("high_morale_penalty_mitigation", out var hpm))
                        cfg.HighMoralePenaltyMitigation = (float)hpm.GetDouble();
                }

                s_config = cfg;
            }
            catch
            {
                // Fall back to defaults on corrupt payload
                s_config = NeedsPerformanceConfig.Default;
            }
        }

        /// <summary>
        /// Pure projection from a survivor's needs state into performance modifiers.
        /// </summary>
        public static NeedsPerformanceModifiers Project(SurvivorNeedsState? state)
        {
            if (state == null || state.IsDead || !state.IsAlive)
                return NeedsPerformanceModifiers.Neutral;

            return Project(state.Hunger, state.Thirst, state.Fatigue, state.Warmth, state.Morale);
        }

        /// <summary>
        /// Pure projection from explicit need values.
        /// </summary>
        public static NeedsPerformanceModifiers Project(
            float hunger,
            float thirst,
            float fatigue,
            float warmth,
            float morale)
        {
            var contributions = new List<NeedPerformanceContribution>(4);
            var worstBand = PerformanceBand.Optimal;

            // 1. Hunger (0..100, higher = worse)
            var cHunger = EvaluateHunger(hunger);
            if (cHunger != null)
            {
                contributions.Add(cHunger);
                if (cHunger.Band > worstBand) worstBand = cHunger.Band;
            }

            // 2. Thirst (0..100, higher = worse)
            var cThirst = EvaluateThirst(thirst);
            if (cThirst != null)
            {
                contributions.Add(cThirst);
                if (cThirst.Band > worstBand) worstBand = cThirst.Band;
            }

            // 3. Fatigue (0..100, higher = worse)
            var cFatigue = EvaluateFatigue(fatigue);
            if (cFatigue != null)
            {
                contributions.Add(cFatigue);
                if (cFatigue.Band > worstBand) worstBand = cFatigue.Band;
            }

            // 4. Cold / Warmth (Warmth 0..100, lower = worse, so cold severity = 100 - warmth)
            var cCold = EvaluateCold(warmth);
            if (cCold != null)
            {
                contributions.Add(cCold);
                if (cCold.Band > worstBand) worstBand = cCold.Band;
            }

            if (contributions.Count == 0)
                return NeedsPerformanceModifiers.Neutral;

            // Multiplicative stacking of penalties
            float combatAccMult = 1.0f;
            float combatDmgMult = 1.0f;
            float workSpeedMult = 1.0f;
            float expedSpeedMult = 1.0f;
            float staminaDrainMult = 1.0f;

            for (int i = 0; i < contributions.Count; i++)
            {
                var c = contributions[i];
                combatAccMult *= (1.0f + c.CombatAccuracyMod);
                combatDmgMult *= (1.0f + c.CombatDamageMod);
                workSpeedMult *= (1.0f + c.WorkSpeedMod);
                expedSpeedMult *= (1.0f + c.ExpeditionSpeedMod);
                staminaDrainMult *= (1.0f + c.StaminaDrainMod);
            }

            // Morale interaction
            if (morale < s_config.DemoralizedThreshold)
            {
                // Penalties amplified
                float factor = 1.0f + s_config.DemoralizedPenaltyAmplification;
                combatAccMult = AmplifyPenalty(combatAccMult, factor);
                combatDmgMult = AmplifyPenalty(combatDmgMult, factor);
                workSpeedMult = AmplifyPenalty(workSpeedMult, factor);
                expedSpeedMult = AmplifyPenalty(expedSpeedMult, factor);
                staminaDrainMult = 1.0f + (staminaDrainMult - 1.0f) * factor;
            }
            else if (morale > s_config.HighMoraleThreshold)
            {
                // Penalties slightly mitigated
                float factor = 1.0f - s_config.HighMoralePenaltyMitigation;
                combatAccMult = MitigatePenalty(combatAccMult, factor);
                combatDmgMult = MitigatePenalty(combatDmgMult, factor);
                workSpeedMult = MitigatePenalty(workSpeedMult, factor);
                expedSpeedMult = MitigatePenalty(expedSpeedMult, factor);
                staminaDrainMult = 1.0f + (staminaDrainMult - 1.0f) * factor;
            }

            // Apply data-driven bounds/floors
            combatAccMult = Math.Clamp(combatAccMult, s_config.MinCombatAccuracyMultiplier, 1.0f);
            combatDmgMult = Math.Clamp(combatDmgMult, s_config.MinCombatDamageMultiplier, 1.0f);
            workSpeedMult = Math.Clamp(workSpeedMult, s_config.MinWorkSpeedMultiplier, 1.0f);
            expedSpeedMult = Math.Clamp(expedSpeedMult, s_config.MinExpeditionSpeedMultiplier, 1.0f);
            staminaDrainMult = Math.Clamp(staminaDrainMult, 1.0f, s_config.MaxExpeditionStaminaDrainMultiplier);

            return new NeedsPerformanceModifiers(
                combatAccMult,
                combatDmgMult,
                workSpeedMult,
                expedSpeedMult,
                staminaDrainMult,
                worstBand,
                contributions);
        }

        private static float AmplifyPenalty(float currentMult, float amplification)
        {
            float penalty = 1.0f - currentMult;
            return 1.0f - (penalty * amplification);
        }

        private static float MitigatePenalty(float currentMult, float factor)
        {
            float penalty = 1.0f - currentMult;
            return 1.0f - (penalty * factor);
        }

        private static NeedPerformanceContribution? EvaluateHunger(float hunger)
        {
            float s = Math.Clamp(hunger, 0f, 100f);
            if (s < 30f) return null;

            if (s < 60f)
            {
                return new NeedPerformanceContribution(
                    NeedKind.Hunger, hunger, s,
                    PerformanceBand.Impaired,
                    combatAccuracyMod: -0.10f,
                    combatDamageMod: -0.10f,
                    workSpeedMod: -0.15f,
                    expeditionSpeedMod: -0.10f,
                    staminaDrainMod: 0.15f,
                    reasonKey: "hunger_impaired");
            }

            if (s < 90f)
            {
                return new NeedPerformanceContribution(
                    NeedKind.Hunger, hunger, s,
                    PerformanceBand.Severe,
                    combatAccuracyMod: -0.25f,
                    combatDamageMod: -0.25f,
                    workSpeedMod: -0.30f,
                    expeditionSpeedMod: -0.25f,
                    staminaDrainMod: 0.35f,
                    reasonKey: "hunger_severe");
            }

            return new NeedPerformanceContribution(
                NeedKind.Hunger, hunger, s,
                PerformanceBand.Critical,
                combatAccuracyMod: -0.50f,
                combatDamageMod: -0.50f,
                workSpeedMod: -0.70f,
                expeditionSpeedMod: -0.50f,
                staminaDrainMod: 0.75f,
                reasonKey: "hunger_critical");
        }

        private static NeedPerformanceContribution? EvaluateThirst(float thirst)
        {
            float s = Math.Clamp(thirst, 0f, 100f);
            if (s < 20f) return null;

            if (s < 50f)
            {
                return new NeedPerformanceContribution(
                    NeedKind.Thirst, thirst, s,
                    PerformanceBand.Impaired,
                    combatAccuracyMod: -0.15f,
                    combatDamageMod: -0.15f,
                    workSpeedMod: -0.20f,
                    expeditionSpeedMod: -0.15f,
                    staminaDrainMod: 0.20f,
                    reasonKey: "thirst_impaired");
            }

            if (s < 80f)
            {
                return new NeedPerformanceContribution(
                    NeedKind.Thirst, thirst, s,
                    PerformanceBand.Severe,
                    combatAccuracyMod: -0.30f,
                    combatDamageMod: -0.30f,
                    workSpeedMod: -0.40f,
                    expeditionSpeedMod: -0.30f,
                    staminaDrainMod: 0.45f,
                    reasonKey: "thirst_severe");
            }

            return new NeedPerformanceContribution(
                NeedKind.Thirst, thirst, s,
                PerformanceBand.Critical,
                combatAccuracyMod: -0.50f,
                combatDamageMod: -0.50f,
                workSpeedMod: -0.60f,
                expeditionSpeedMod: -0.50f,
                staminaDrainMod: 0.80f,
                reasonKey: "thirst_critical");
        }

        private static NeedPerformanceContribution? EvaluateFatigue(float fatigue)
        {
            float s = Math.Clamp(fatigue, 0f, 100f);
            if (s < 40f) return null;

            if (s < 70f)
            {
                return new NeedPerformanceContribution(
                    NeedKind.Fatigue, fatigue, s,
                    PerformanceBand.Impaired,
                    combatAccuracyMod: -0.10f,
                    combatDamageMod: -0.10f,
                    workSpeedMod: -0.20f,
                    expeditionSpeedMod: -0.15f,
                    staminaDrainMod: 0.25f,
                    reasonKey: "fatigue_impaired");
            }

            if (s < 90f)
            {
                return new NeedPerformanceContribution(
                    NeedKind.Fatigue, fatigue, s,
                    PerformanceBand.Severe,
                    combatAccuracyMod: -0.25f,
                    combatDamageMod: -0.25f,
                    workSpeedMod: -0.40f,
                    expeditionSpeedMod: -0.30f,
                    staminaDrainMod: 0.50f,
                    reasonKey: "fatigue_severe");
            }

            return new NeedPerformanceContribution(
                NeedKind.Fatigue, fatigue, s,
                PerformanceBand.Critical,
                combatAccuracyMod: -0.50f,
                combatDamageMod: -0.50f,
                workSpeedMod: -0.60f,
                expeditionSpeedMod: -0.50f,
                staminaDrainMod: 1.00f,
                reasonKey: "fatigue_critical");
        }

        private static NeedPerformanceContribution? EvaluateCold(float warmth)
        {
            float cold = Math.Clamp(100f - warmth, 0f, 100f);
            if (cold < 20f) return null;

            if (cold < 50f)
            {
                return new NeedPerformanceContribution(
                    NeedKind.Warmth, warmth, cold,
                    PerformanceBand.Impaired,
                    combatAccuracyMod: -0.10f,
                    combatDamageMod: -0.10f,
                    workSpeedMod: -0.15f,
                    expeditionSpeedMod: -0.10f,
                    staminaDrainMod: 0.15f,
                    reasonKey: "cold_impaired");
            }

            if (cold < 80f)
            {
                return new NeedPerformanceContribution(
                    NeedKind.Warmth, warmth, cold,
                    PerformanceBand.Severe,
                    combatAccuracyMod: -0.20f,
                    combatDamageMod: -0.20f,
                    workSpeedMod: -0.30f,
                    expeditionSpeedMod: -0.25f,
                    staminaDrainMod: 0.30f,
                    reasonKey: "cold_severe");
            }

            return new NeedPerformanceContribution(
                NeedKind.Warmth, warmth, cold,
                PerformanceBand.Critical,
                combatAccuracyMod: -0.40f,
                combatDamageMod: -0.40f,
                workSpeedMod: -0.50f,
                expeditionSpeedMod: -0.40f,
                staminaDrainMod: 0.60f,
                reasonKey: "cold_critical");
        }
    }
}
