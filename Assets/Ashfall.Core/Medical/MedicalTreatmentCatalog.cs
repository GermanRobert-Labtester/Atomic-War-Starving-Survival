// SPDX-License-Identifier: MIT
// Task #133 — Treatment definitions: authored capability, separate from execution.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// One authored treatment capability. A definition describes what a treatment
    /// requires and targets; a scheduled or executed instance lives in
    /// <see cref="MedicalProcedureSchedule"/> / the command result. Definitions
    /// here are code constants matching the existing item economy 1:1 — this is
    /// not a balance rewrite, and no treatment invents a resource the game does
    /// not already consume.
    /// </summary>
    [Serializable]
    public sealed class MedicalTreatmentDef
    {
        /// <summary>Stable snake_case treatment id, e.g. <c>treatment_inhaler</c>.</summary>
        public string TreatmentId = string.Empty;

        public string DisplayName = string.Empty;

        /// <summary>Affliction definition this treatment targets.</summary>
        public string AfflictionId = string.Empty;

        /// <summary>Inventory item costs (authoritative item ids). Empty for diagnosis-style actions.</summary>
        public Dictionary<string, int> ItemCosts = new Dictionary<string, int>();

        /// <summary>True when a Confirmed diagnosis is required before the treatment is legal.</summary>
        public bool RequiresConfirmedDiagnosis;

        /// <summary>True when the treatment consumes campaign time via the procedure schedule.</summary>
        public bool IsScheduled;

        /// <summary>Duration in game hours for scheduled treatments (0 for immediate).</summary>
        public float DurationHours;

        /// <summary>True when a patient may hold at most one active procedure of this treatment.</summary>
        public bool ExclusivePerPatient;

        /// <summary>True when the treatment needs an active ward admission (future ward procedures).</summary>
        public bool RequiresWardAdmission;

        public MedicalTreatmentDef() { }

        public MedicalTreatmentDef(string treatmentId, string displayName, string afflictionId)
        {
            TreatmentId = treatmentId;
            DisplayName = displayName;
            AfflictionId = afflictionId;
        }

        public MedicalTreatmentDef Clone()
        {
            var clone = new MedicalTreatmentDef(TreatmentId, DisplayName, AfflictionId)
            {
                RequiresConfirmedDiagnosis = RequiresConfirmedDiagnosis,
                IsScheduled = IsScheduled,
                DurationHours = DurationHours,
                ExclusivePerPatient = ExclusivePerPatient,
                RequiresWardAdmission = RequiresWardAdmission
            };
            foreach (var kv in ItemCosts) clone.ItemCosts[kv.Key] = kv.Value;
            return clone;
        }
    }

    /// <summary>
    /// The authored treatment catalog. Affliction ids used here:
    /// <c>affliction_respiratory_degeneration</c>,
    /// <c>affliction_radiation_sickness</c>. Costs match the current UI buttons
    /// (MedicalPanel) exactly: one item per application.
    /// </summary>
    public static class MedicalTreatmentCatalog
    {
        public const string RespiratoryDegenerationId = "affliction_respiratory_degeneration";
        public const string RadiationSicknessId = "affliction_radiation_sickness";
        public const string ChemicalDependencyId = "affliction_chemical_dependency";
        public const string DiseaseId = "affliction_disease";
        /// <summary>Low health as a treatable condition; the Needs domain owns the value.</summary>
        public const string HealthDeficitId = "affliction_health_deficit";

        // ── Psychology (Task #133 P1c) — observe-only projection ─────
        // Definition identities only: the Phase-0 systems keep every rule
        // and the phase0_psychology tick, and no MedicalTreatmentDef exists
        // for these conditions (their handlers always refuse treatment).
        public const string CombatTraumaId = "affliction_combat_trauma";
        public const string SomaticFlashbackId = "affliction_somatic_flashback";
        public const string GuiltInsomniaId = "affliction_guilt_insomnia";

        // Canonical inventory item ids (aliases resolved by ItemAliases upstream).
        public const string ItemInhaler = "inhaler";
        public const string ItemHerbalTea = "herbal_tea";
        public const string ItemBandage = "bandage";
        public const string ItemIodine = "iodine_pills";
        public const string ItemAntiRad = "rad_away";

        // ── Respiratory ───────────────────────────────────────────────
        public const string TreatmentInhaler = "treatment_inhaler";
        public const string TreatmentHerbalTea = "treatment_herbal_tea";

        // ── General care ──────────────────────────────────────────────
        public const string TreatmentBandage = "treatment_bandage";

        // ── Radiation ─────────────────────────────────────────────────
        public const string TreatmentIodine = "treatment_iodine";
        public const string TreatmentAntiRad = "treatment_anti_rad";

        // ── Disease (Task #133 P1) ────────────────────────────────────
        // Quarantine/release carry no fixed affliction: the caller targets the
        // disease handler per call (AfflictionId empty ⇒ target required).
        public const string TreatmentQuarantine = "treatment_quarantine";
        public const string TreatmentRelease = "treatment_release";
        /// <summary>Masked identity shown for suspected-but-unidentified disease episodes.</summary>
        public const string UnidentifiedIllnessId = "affliction_unidentified_illness";

        // ── Camp-wide vector protocols (Task #133 P1) ────────────────
        public const string ProtocolPurifyWater = "protocol_purify_water";
        public const string ProtocolSealVents = "protocol_seal_vents";
        public const string ProtocolSterilizeTools = "protocol_sterilize_tools";
        public const string ProtocolAirFiltration = "protocol_air_filtration";

        // ── Chemical dependency (Task #133 P1b) ──────────────────────
        // Detox STARTS flow through the pipeline; the 120h/72h withdrawal
        // clocks stay entirely inside ChemicalDependencySystem (TickHours).
        // No item cost: the ledger is player-facing and detox consumes time,
        // not supplies. The substance is chosen per call via targetItem.
        public const string TreatmentManagedDetox = "treatment_managed_detox";
        public const string TreatmentColdTurkey = "treatment_cold_turkey";

        // Canonical countermeasure items authored by disease_catalog.json.
        public const string ItemCleanWater = "clean_water";
        public const string ItemGasMask = "gas_mask";
        public const string ItemAntibiotics = "antibiotics";
        public const string ItemHazmatSuit = "hazmat_suit";

        private static readonly Dictionary<string, MedicalTreatmentDef> s_defs = Build();

        private static Dictionary<string, MedicalTreatmentDef> Build()
        {
            var map = new Dictionary<string, MedicalTreatmentDef>(StringComparer.Ordinal);

            var inhaler = new MedicalTreatmentDef(TreatmentInhaler, "Inhaler", RespiratoryDegenerationId);
            inhaler.ItemCosts[ItemInhaler] = 1;
            map[inhaler.TreatmentId] = inhaler;

            var tea = new MedicalTreatmentDef(TreatmentHerbalTea, "Herbal Tea", RespiratoryDegenerationId);
            tea.ItemCosts[ItemHerbalTea] = 1;
            map[tea.TreatmentId] = tea;

            var bandage = new MedicalTreatmentDef(TreatmentBandage, "Bandage", HealthDeficitId);
            bandage.ItemCosts[ItemBandage] = 1;
            map[bandage.TreatmentId] = bandage;

            var iodine = new MedicalTreatmentDef(TreatmentIodine, "Potassium Iodide", RadiationSicknessId);
            iodine.ItemCosts[ItemIodine] = 1;
            map[iodine.TreatmentId] = iodine;

            var antiRad = new MedicalTreatmentDef(TreatmentAntiRad, "Anti-Rad Chelation", RadiationSicknessId);
            antiRad.ItemCosts[ItemAntiRad] = 1;
            map[antiRad.TreatmentId] = antiRad;

            // Disease isolation (Task #133 P1): no item cost; the disease
            // handler is chosen per call via the target affliction. Requiring a
            // confirmed diagnosis keeps quarantine targeting leak-free — the
            // player identifies the illness first, then isolates by name.
            var quarantine = new MedicalTreatmentDef(TreatmentQuarantine, "Quarantine", string.Empty);
            quarantine.RequiresConfirmedDiagnosis = true;
            map[quarantine.TreatmentId] = quarantine;

            var release = new MedicalTreatmentDef(TreatmentRelease, "Release From Quarantine", string.Empty);
            release.RequiresConfirmedDiagnosis = true;
            map[release.TreatmentId] = release;

            // Chemical-dependency detox starts (Task #133 P1b): immediate
            // actions that flip the domain's program flags; the withdrawal
            // clock then advances via the domain TickHours exactly as before.
            map[TreatmentManagedDetox] = new MedicalTreatmentDef(TreatmentManagedDetox, "Managed Detox", ChemicalDependencyId);
            map[TreatmentColdTurkey] = new MedicalTreatmentDef(TreatmentColdTurkey, "Cold Turkey", ChemicalDependencyId);

            return map;
        }

        public static MedicalTreatmentDef? Get(string treatmentId)
        {
            return s_defs.TryGetValue(treatmentId, out var def) ? def : null;
        }

        public static IReadOnlyCollection<MedicalTreatmentDef> All
        {
            get
            {
                var keys = new List<string>(s_defs.Keys);
                keys.Sort(string.CompareOrdinal);
                var list = new List<MedicalTreatmentDef>(keys.Count);
                foreach (var k in keys) list.Add(s_defs[k].Clone());
                return list;
            }
        }
    }
}
