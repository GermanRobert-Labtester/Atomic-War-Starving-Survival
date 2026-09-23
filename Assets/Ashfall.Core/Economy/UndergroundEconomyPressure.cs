// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Four-band market temperature rating for underground and black-market trade (EN-03 / W2A-G03).
    /// </summary>
    public enum MarketTemperatureBand
    {
        Calm = 0,
        Raised = 1,
        Hot = 2,
        Relocated = 3
    }

    /// <summary>
    /// EN-03 / UNBLOCK-05 / UNBLOCK-02: Underground Economy Pressure Read Model.
    /// Pure projection over black market heat, trust, and relocation state rendering
    /// temperature bands, price pressure, and attention risk for panels and callers.
    /// Pure view over existing state; zero duplicate ledgers or mutable stores.
    /// </summary>
    public sealed class UndergroundEconomyPressure
    {
        public MarketTemperatureBand Band { get; }
        public int CurrentHeat { get; }
        public int HeatThreshold { get; }
        public int Trust { get; }
        public bool IsRelocated { get; }
        public float PricePressureMultiplier { get; }
        public float AttentionRiskMultiplier { get; }
        public string StatusSummary { get; }

        public UndergroundEconomyPressure(
            MarketTemperatureBand band,
            int currentHeat,
            int heatThreshold,
            int trust,
            bool isRelocated,
            float pricePressureMultiplier,
            float attentionRiskMultiplier,
            string statusSummary)
        {
            Band = band;
            CurrentHeat = Math.Max(0, currentHeat);
            HeatThreshold = Math.Max(1, heatThreshold);
            Trust = trust;
            IsRelocated = isRelocated;
            PricePressureMultiplier = pricePressureMultiplier;
            AttentionRiskMultiplier = attentionRiskMultiplier;
            StatusSummary = statusSummary ?? string.Empty;
        }

        public static UndergroundEconomyPressure Evaluate(
            int currentHeat,
            int heatThreshold = 100,
            int trust = 50,
            bool isRelocated = false)
        {
            int threshold = Math.Max(1, heatThreshold);
            int heat = Math.Max(0, currentHeat);

            MarketTemperatureBand band;
            float priceMult;
            float riskMult;
            string summary;

            if (isRelocated)
            {
                band = MarketTemperatureBand.Relocated;
                priceMult = 1.50f;
                riskMult = 3.00f;
                summary = "Black market contacts dispersed; elevated transaction friction and steep markups.";
            }
            else if (heat >= threshold)
            {
                band = MarketTemperatureBand.Hot;
                priceMult = 1.35f;
                riskMult = 2.50f;
                summary = "Heavy authority scrutiny; raid risk imminent, traders demand hazard premiums.";
            }
            else if (heat > 0 && heat * 2 >= threshold)
            {
                band = MarketTemperatureBand.Raised;
                priceMult = 1.15f;
                riskMult = 1.50f;
                summary = "Noticed activity; informants listening, slight price inflation.";
            }
            else
            {
                band = MarketTemperatureBand.Calm;
                priceMult = 1.00f;
                riskMult = 1.00f;
                summary = "Discreet operations; baseline tariff and routine transaction channels.";
            }

            return new UndergroundEconomyPressure(
                band,
                heat,
                threshold,
                trust,
                isRelocated,
                priceMult,
                riskMult,
                summary);
        }
    }
}
