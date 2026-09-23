// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Spiritual
{
    public readonly struct HolyDayObservance
    {
        public string HolyDayId { get; }
        public string MovementId { get; }
        public string Title { get; }
        public int DayOfYear { get; }
        public int MoraleBonusPermille { get; }
        public int FrictionReductionPermille { get; }

        public HolyDayObservance(
            string holyDayId,
            string movementId,
            string title,
            int dayOfYear,
            int moraleBonusPermille,
            int frictionReductionPermille)
        {
            HolyDayId = holyDayId ?? string.Empty;
            MovementId = movementId ?? string.Empty;
            Title = title ?? string.Empty;
            DayOfYear = dayOfYear;
            MoraleBonusPermille = moraleBonusPermille;
            FrictionReductionPermille = frictionReductionPermille;
        }
    }

    public readonly struct RitualEffectResult
    {
        public bool IsAllowed { get; }
        public int MoraleDeltaPermille { get; }
        public int FrictionReductionPermille { get; }
        public int CooldownRemainingDays { get; }
        public string Reason { get; }

        public RitualEffectResult(
            bool isAllowed,
            int moraleDeltaPermille,
            int frictionReductionPermille,
            int cooldownRemainingDays,
            string reason)
        {
            IsAllowed = isAllowed;
            MoraleDeltaPermille = moraleDeltaPermille;
            FrictionReductionPermille = frictionReductionPermille;
            CooldownRemainingDays = Math.Max(0, cooldownRemainingDays);
            Reason = reason ?? string.Empty;
        }
    }

    /// <summary>
    /// Expansion 13: The Faithful & The Fractured — Spiritual Ritual Calendar & Ideological Friction Engine.
    /// Pure domain engine calculating holy calendar observances, ritual friction mitigation,
    /// and symmetrical creed evaluation. No parallel piety meters or unseeded RNG.
    /// </summary>
    public static class SpiritualRitualCalendarEngine
    {
        public const int DaysPerYear = 360;

        private static readonly HolyDayObservance[] CanonicalHolyDays = new[]
        {
            new HolyDayObservance(
                "holy_day_unforgotten",
                "ash_witnesses",
                "Day of the Unforgotten",
                dayOfYear: 45, // Mid-winter
                moraleBonusPermille: 120,
                frictionReductionPermille: 150),

            new HolyDayObservance(
                "holy_day_foundry_consecration",
                "rebuilders",
                "Foundry Consecration",
                dayOfYear: 90, // Spring equinox
                moraleBonusPermille: 150,
                frictionReductionPermille: 100),

            new HolyDayObservance(
                "holy_day_static_vigil",
                "listeners",
                "Vigil of the Static Air",
                dayOfYear: 270, // Autumn gathering
                moraleBonusPermille: 100,
                frictionReductionPermille: 200),
        };

        public static HolyDayObservance? GetScheduledObservance(int campaignDay, string movementId)
        {
            if (campaignDay <= 0) return null;

            int dayOfYear = ((campaignDay - 1) % DaysPerYear) + 1;

            foreach (var holyDay in CanonicalHolyDays)
            {
                if (string.Equals(holyDay.MovementId, movementId, StringComparison.OrdinalIgnoreCase))
                {
                    // Observance window: within 2 days of the anchor day
                    if (Math.Abs(dayOfYear - holyDay.DayOfYear) <= 2)
                    {
                        return holyDay;
                    }
                }
            }

            return null;
        }

        public static RitualEffectResult EvaluateRitualObservance(
            SpiritualRitualDefinition ritual,
            int daysSinceLastPerformed,
            int shelterMoralePermille = 500)
        {
            if (ritual == null)
            {
                return new RitualEffectResult(false, 0, 0, 0, "No ritual definition.");
            }

            int cooldown = Math.Max(1, ritual.CooldownDays);

            if (daysSinceLastPerformed < cooldown)
            {
                int remaining = cooldown - daysSinceLastPerformed;
                return new RitualEffectResult(false, 0, 0, remaining, $"Ritual on cooldown; {remaining} days remaining.");
            }

            // Convert MoraleDelta float to permille (e.g. +2.0f -> +200 permille)
            int baseMoralePermille = (int)(ritual.MoraleDelta * 100f);

            // If shelter morale is critically low (<300 permille), spiritual comfort is 50% more impactful
            if (shelterMoralePermille < 300 && baseMoralePermille > 0)
            {
                baseMoralePermille = (baseMoralePermille * 1500) / 1000;
            }

            int frictionReduction = string.IsNullOrEmpty(ritual.FrictionFlag) ? 50 : 150;

            return new RitualEffectResult(
                true,
                baseMoralePermille,
                frictionReduction,
                0,
                "Ritual performed successfully.");
        }

        public static int CalculateIdeologicalFrictionMitigation(
            string movementA,
            string movementB,
            bool sharedRitualObserved)
        {
            if (string.IsNullOrEmpty(movementA) || string.IsNullOrEmpty(movementB))
                return 0;

            // Same movement has no ideological friction
            if (string.Equals(movementA, movementB, StringComparison.OrdinalIgnoreCase))
                return 1000; // 100% mitigated

            // Base cross-movement mitigation
            int mitigationPermille = 100;

            // Known complementary movements (e.g., Listeners & Ash Witnesses share reverence for memorial logs)
            if ((movementA.Equals("listeners", StringComparison.OrdinalIgnoreCase) && movementB.Equals("ash_witnesses", StringComparison.OrdinalIgnoreCase)) ||
                (movementA.Equals("ash_witnesses", StringComparison.OrdinalIgnoreCase) && movementB.Equals("listeners", StringComparison.OrdinalIgnoreCase)))
            {
                mitigationPermille += 150;
            }

            // If a shared ceremony or ritual was observed, grant major friction reduction (+250 permille)
            if (sharedRitualObserved)
            {
                mitigationPermille += 250;
            }

            return Math.Min(1000, mitigationPermille);
        }
    }
}
