using System;
using System.Collections.Generic;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Single host path for chem consumption side-effects: addiction, blood
    /// toxicity (morphine / anti_rad / amphetamines), and polypharmacy dose log.
    /// Keeps GameBootstrap / AI / inventory callers from diverging.
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
        private Func<int> _getDay;
        private Func<float> _getGameHours;

        public void Bind(
            AddictionSystem addiction,
            BloodToxicitySystem bloodToxicity,
            PolypharmacySystem polypharmacy,
            Func<int> getDay,
            Func<float> getGameHours)
        {
            _addiction = addiction;
            _bloodToxicity = bloodToxicity;
            _polypharmacy = polypharmacy;
            _getDay = getDay;
            _getGameHours = getGameHours;
        }

        /// <summary>
        /// Record that <paramref name="sv"/> used <paramref name="itemId"/>.
        /// Safe to call for any item — non-tracked ids are ignored by each system.
        /// </summary>
        public void Notify(Survivor sv, string itemId)
        {
            if (sv == null || string.IsNullOrEmpty(itemId)) return;

            int day = _getDay != null ? _getDay() : 1;
            float gameHours = _getGameHours != null ? _getGameHours() : 0f;

            _addiction?.OnItemConsumed(sv, itemId, day);
            if (!string.IsNullOrEmpty(sv.Id))
            {
                _bloodToxicity?.RecordChemUse(sv.Id, itemId);
                if (PolypharmacyDrugIds.Contains(itemId))
                    _polypharmacy?.RecordDose(sv.Id, itemId, gameHours);
            }
        }

        /// <summary>True when item id participates in polypharmacy window tracking.</summary>
        public static bool IsPolypharmacyDrug(string itemId) =>
            !string.IsNullOrEmpty(itemId) && PolypharmacyDrugIds.Contains(itemId);
    }
}
