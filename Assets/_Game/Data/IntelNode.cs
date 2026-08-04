using System;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Type of intel extracted from radio broadcasts.
    /// </summary>
    public enum IntelType
    {
        PlumeReport,        // Radiation plume data for map update
        WeatherForecast,    // Weather prediction
        LootLocation,       // Supply cache location hint
        MortarWarning,      // Impending strike warning (pre-Day 30)
        TroopMovement,      // Military movement intel (pre-Day 30)
        Propaganda,         // Pre-war propaganda (flavor only)
        EmergencyLoop,      // Post-Day 30 automated emergency broadcast
        NumbersStation,     // Encoded numbers station transmission
        /// <summary>Post-EMP ghost station loop (Prompt #19). Never grants map/extraction intel.</summary>
        GhostLoop,
        Unknown             // Unparseable static
    }

    /// <summary>
    /// Data fragment extracted from radio broadcasts by spending time tuning the radio.
    /// Intel nodes have a type, optional target location, confidence level, expiration,
    /// and display text. Used to update RadiationKnowledgeMap (PlumeReports), inform
    /// scavenging decisions (LootLocation), or provide narrative flavor.
    /// </summary>
    [Serializable]
    public class IntelNode
    {
        public string Id;
        public IntelType Type;
        public string SourceFrequencyId;
        public int ExtractedDay;
        public int ExpirationDay;

        /// <summary>Target location ID (for PlumeReport, LootLocation, MortarWarning).</summary>
        public string TargetLocationId;

        /// <summary>Confidence/uncertainty (0..1). Higher = more reliable intel.</summary>
        [Range(0f, 1f)]
        public float Confidence;

        /// <summary>
        /// For PlumeReport: rumored radiation level at target location.
        /// For WeatherForecast: predicted weather kind.
        /// For LootLocation: supply value estimate (0..1).
        /// </summary>
        public float NumericValue;

        /// <summary>Weather forecast: predicted weather kind (0..3 enum index).</summary>
        public int WeatherForecastKind;

        /// <summary>Display text for UI/narrative.</summary>
        [TextArea(2, 5)]
        public string Text;

        /// <summary>Whether this intel has been applied/consumed.</summary>
        public bool IsConsumed;

        /// <summary>Check if this intel has expired.</summary>
        public bool IsExpired(int currentDay)
        {
            return ExpirationDay >= 0 && currentDay > ExpirationDay;
        }

        /// <summary>Create a plume report intel node.</summary>
        public static IntelNode CreatePlumeReport(string locationId, float rumoredRad, float confidence, int extractedDay, int expirationDay, string text)
        {
            return new IntelNode
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                Type = IntelType.PlumeReport,
                TargetLocationId = locationId,
                NumericValue = rumoredRad,
                Confidence = Mathf.Clamp01(confidence),
                ExtractedDay = extractedDay,
                ExpirationDay = expirationDay,
                Text = text
            };
        }

        /// <summary>Create a weather forecast intel node.</summary>
        public static IntelNode CreateWeatherForecast(int weatherKind, float confidence, int extractedDay, int expirationDay, string text)
        {
            return new IntelNode
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                Type = IntelType.WeatherForecast,
                WeatherForecastKind = weatherKind,
                Confidence = Mathf.Clamp01(confidence),
                ExtractedDay = extractedDay,
                ExpirationDay = expirationDay,
                Text = text
            };
        }

        /// <summary>Create a loot location intel node.</summary>
        public static IntelNode CreateLootLocation(string locationId, float supplyValue, float confidence, int extractedDay, int expirationDay, string text)
        {
            return new IntelNode
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                Type = IntelType.LootLocation,
                TargetLocationId = locationId,
                NumericValue = supplyValue,
                Confidence = Mathf.Clamp01(confidence),
                ExtractedDay = extractedDay,
                ExpirationDay = expirationDay,
                Text = text
            };
        }

        /// <summary>Create a mortar warning intel node (pre-Day 30 tactical intel).</summary>
        public static IntelNode CreateMortarWarning(string locationId, float confidence, int extractedDay, int expirationDay, string text)
        {
            return new IntelNode
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                Type = IntelType.MortarWarning,
                TargetLocationId = locationId,
                Confidence = Mathf.Clamp01(confidence),
                ExtractedDay = extractedDay,
                ExpirationDay = expirationDay,
                Text = text
            };
        }

        /// <summary>
        /// Create a ghost-station loop node (Prompt #19). Low confidence, no target
        /// location, never a PlumeReport / military payload.
        /// </summary>
        public static IntelNode CreateGhostLoop(
            string sourceFrequencyId,
            int extractedDay,
            string text,
            float confidence = 0.15f)
        {
            return new IntelNode
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                Type = IntelType.GhostLoop,
                SourceFrequencyId = sourceFrequencyId ?? string.Empty,
                TargetLocationId = string.Empty,
                Confidence = Mathf.Clamp01(confidence),
                ExtractedDay = extractedDay,
                ExpirationDay = -1, // loops do not expire as tactical intel
                Text = text ?? string.Empty,
                NumericValue = 0f
            };
        }
    }
}
