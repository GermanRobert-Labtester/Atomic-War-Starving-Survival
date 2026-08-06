using System;
using System.Collections.Generic;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Single host path for chem consumption side-effects: addiction, blood
    /// toxicity, polypharmacy dose log, and tolerance (morphine / anti_rad /
    /// amphetamines). Keeps GameBootstrap / AI / inventory callers from diverging.
    /// </summary>
    public sealed class ChemUseRouter
    {
        /// <summary>Drugs that feed polypharmacy interaction windows (Prompt #178).</summary>
        private static readonly HashSet<string> PolypharmacyDrugIds =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "morphine",
                "anti_rad",
                "amphetamines",
                "iodine",
                "iodine_pills",
                "antibiotics",
                "painkiller",
                "stimulant"
            };

        private AddictionSystem _addiction;
        private BloodToxicitySystem _bloodToxicity;
        private PolypharmacySystem _polypharmacy;
        private System_Tolerance _tolerance;
        private Func<int> _getDay;
        private Func<float> _getGameHours;

        /// <summary>Duration hours applied for the last <see cref="Notify"/> (pre-increment).</summary>
        public float LastAppliedDurationHours { get; private set; } = System_Tolerance.BaseDurationHours;

        /// <summary>Therapeutic scale applied for the last <see cref="Notify"/> (pre-increment).</summary>
        public float LastAppliedEffectiveness { get; private set; } = 1f;

        public void Bind(
            AddictionSystem addiction,
            BloodToxicitySystem bloodToxicity,
            PolypharmacySystem polypharmacy,
            System_Tolerance tolerance,
            Func<int> getDay,
            Func<float> getGameHours)
        {
            _addiction = addiction;
            _bloodToxicity = bloodToxicity;
            _polypharmacy = polypharmacy;
            _tolerance = tolerance;
            _getDay = getDay;
            _getGameHours = getGameHours;
        }

        /// <summary>Peek next-dose effectiveness without recording use.</summary>
        public float PeekEffectiveness(Survivor sv, string itemId)
        {
            if (sv == null || string.IsNullOrEmpty(sv.Id) || string.IsNullOrEmpty(itemId))
                return 1f;
            if (_tolerance == null || !System_Tolerance.IsToleranceChem(itemId))
                return 1f;
            return _tolerance.GetEffectiveness(sv.Id, itemId);
        }

        /// <summary>Peek next-dose duration hours without recording use.</summary>
        public float PeekDurationHours(Survivor sv, string itemId)
        {
            if (sv == null || string.IsNullOrEmpty(sv.Id) || string.IsNullOrEmpty(itemId))
                return System_Tolerance.BaseDurationHours;
            if (_tolerance == null || !System_Tolerance.IsToleranceChem(itemId))
                return System_Tolerance.BaseDurationHours;
            return _tolerance.GetDuration(sv.Id, itemId);
        }

        /// <summary>
        /// Record that <paramref name="sv"/> used <paramref name="itemId"/>.
        /// Safe to call for any item — non-tracked ids are ignored by each system.
        /// Peeks duration/effectiveness before incrementing tolerance.
        /// </summary>
        public void Notify(Survivor sv, string itemId)
        {
            if (sv == null || string.IsNullOrEmpty(itemId)) return;

            int day = _getDay != null ? _getDay() : 1;
            float gameHours = _getGameHours != null ? _getGameHours() : 0f;

            LastAppliedDurationHours = System_Tolerance.BaseDurationHours;
            LastAppliedEffectiveness = 1f;

            _addiction?.OnItemConsumed(sv, itemId, day);
            if (string.IsNullOrEmpty(sv.Id)) return;

            _bloodToxicity?.RecordChemUse(sv.Id, itemId);
            if (PolypharmacyDrugIds.Contains(itemId))
                _polypharmacy?.RecordDose(sv.Id, itemId, gameHours);

            if (_tolerance != null && System_Tolerance.IsToleranceChem(itemId))
            {
                LastAppliedDurationHours = _tolerance.GetDuration(sv.Id, itemId);
                LastAppliedEffectiveness = _tolerance.GetEffectiveness(sv.Id, itemId);
                _tolerance.UseChem(sv.Id, itemId, gameHours);
            }
        }

        /// <summary>True when item id participates in polypharmacy window tracking.</summary>
        public static bool IsPolypharmacyDrug(string itemId) =>
            !string.IsNullOrEmpty(itemId) && PolypharmacyDrugIds.Contains(itemId);

        /// <summary>True when item builds chem tolerance.</summary>
        public static bool IsToleranceChem(string itemId) =>
            System_Tolerance.IsToleranceChem(itemId);
    }
}
