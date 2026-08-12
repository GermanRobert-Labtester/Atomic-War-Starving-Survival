using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    /// <summary>
    /// Expansion VIII — Bureaucratic Friction System. Factions in the late-game
    /// do not just raid; they audit. They demand paperwork: ration logs, water
    /// output, radio licenses. If you have stamped documents, they leave a
    /// "Compliance Stipend." If you lack paperwork, they confiscate your
    /// generator_alternator as "collateral."
    /// Save/load safe. Plain C#.
    /// </summary>
    public class BureaucraticFrictionSystem
    {
        // ── Inspection constants ──────────────────────────────────────
        public const float InspectionChancePerDay = 0.08f;
        public const string RequiredDoc_TransitPass = "transit_pass_forged";
        public const string RequiredDoc_RationCard = "ration_card_fake";
        public const string RequiredDoc_RadioLicense = "radio_license";

        // ── Stipend constants ─────────────────────────────────────────
        public const int ComplianceStipendAmmo = 20;

        // ── Fine constants ────────────────────────────────────────────
        public const string ConfiscatedItem = "generator_alternator";
        public const float FineScrapKg = 50f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnInspectionTriggered;
        public event Action<string, bool> OnInspectionResolved;  // factionId, passed
        public event Action<string> OnComplianceStipend;
        public event Action<string> OnBureaucraticFine;
        public event Action<string> OnItemConfiscated;

        private readonly System.Random _rng;
        private int _inspectionsPassed;
        private int _inspectionsFailed;
        private int _stipendsReceived;
        private float _totalFinesScrap;

        public int InspectionsPassed => _inspectionsPassed;
        public int InspectionsFailed => _inspectionsFailed;

        public BureaucraticFrictionSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(9000);
        }

        /// <summary>
        /// Roll for a daily inspection by a faction patrol.
        /// </summary>
        public string RollInspection(int currentDay)
        {
            if (_rng.NextDouble() > InspectionChancePerDay) return null;

            string[] factions = { "faction_central_garrison", "faction_upland_militia" };
            string faction = factions[_rng.Next(factions.Length)];
            OnInspectionTriggered?.Invoke(faction);
            return faction;
        }

        /// <summary>
        /// Resolve an inspection. If the player has correct stamped paperwork,
        /// the patrol leaves a Compliance Stipend. Otherwise, a Bureaucratic Fine.
        /// </summary>
        public InspectionResult ResolveInspection(string factionId,
            bool hasTransitPass, bool hasRationCard, bool hasRadioLicense,
            float forgeryQuality)
        {
            bool hasAllDocs = hasTransitPass && hasRationCard && hasRadioLicense;

            if (hasAllDocs)
            {
                // Check if forgery is detected
                if (_rng.NextDouble() < 0.10f * (1f - forgeryQuality))
                {
                    _inspectionsFailed++;
                    OnInspectionResolved?.Invoke(factionId, false);
                    return new InspectionResult
                    {
                        Passed = false,
                        ForgeryDetected = true,
                        Message = "The inspector's eyes narrow. 'This stamp is wrong.' The patrol reaches for their weapons."
                    };
                }

                _inspectionsPassed++;
                _stipendsReceived++;
                OnInspectionResolved?.Invoke(factionId, true);
                OnComplianceStipend?.Invoke(factionId);

                return new InspectionResult
                {
                    Passed = true,
                    StipendAmmo = ComplianceStipendAmmo,
                    Message = "The inspector stamps your log. 'Compliant.' They leave a crate of ammunition as a stipend. The bureaucracy rewards obedience."
                };
            }

            // Missing paperwork
            _inspectionsFailed++;
            OnInspectionResolved?.Invoke(factionId, false);
            OnBureaucraticFine?.Invoke(factionId);
            OnItemConfiscated?.Invoke(ConfiscatedItem);

            return new InspectionResult
            {
                Passed = false,
                ItemConfiscated = ConfiscatedItem,
                FineScrapKg = FineScrapKg,
                Message = "The inspector does not shoot you. He confiscates your generator alternator as 'collateral' until you pay a fine of 50kg of scrap_metal. The friction of the old world strangles the new."
            };
        }

        // ── Save / Load ───────────────────────────────────────────────

        public FrictionSystemSave CaptureState()
        {
            return new FrictionSystemSave
            {
                InspectionsPassed = _inspectionsPassed,
                InspectionsFailed = _inspectionsFailed,
                StipendsReceived = _stipendsReceived,
                TotalFinesScrap = _totalFinesScrap
            };
        }

        public void RestoreState(FrictionSystemSave save)
        {
            _inspectionsPassed = 0;
            _inspectionsFailed = 0;
            _stipendsReceived = 0;
            _totalFinesScrap = 0f;
            if (save == null) return;
            _inspectionsPassed = save.InspectionsPassed;
            _inspectionsFailed = save.InspectionsFailed;
            _stipendsReceived = save.StipendsReceived;
            _totalFinesScrap = save.TotalFinesScrap;
        }
    }

    [Serializable]
    public class InspectionResult
    {
        public bool Passed;
        public bool ForgeryDetected;
        public int StipendAmmo;
        public string ItemConfiscated;
        public float FineScrapKg;
        public string Message;
    }

    [Serializable]
    public class FrictionSystemSave
    {
        public int InspectionsPassed;
        public int InspectionsFailed;
        public int StipendsReceived;
        public float TotalFinesScrap;
    }
}
