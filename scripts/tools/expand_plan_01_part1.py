import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/01-needs-radiation-save-roundtrip-tests.md"

header = """# Plan 01 — NeedsSystem & RadiationSystem Save Round-Trip Integrity, Metabolic Simulation & Deterministic State Verification (closes H10)

**Package:** `PLAN-01-NEEDS-RADIATION-SAVE-ROUNDTRIP-TESTS`
**Document Class:** Master System Architecture, Verification Matrix & Production Integration Blueprint
**Authority Level:** Canonical Production Plan
**Target Runtimes:** Ashfall.Core (`netstandard2.1`, Engine-Free) · Godot Host (`net8.0`) · Ashfall.Core.Tests (`net9.0`)
**Data Authority:** `Assets/StreamingAssets/Data/` (snake_case JSON, schema-validated)
**Historical Anchor:** piagentsplans Wave 1 (2026-08-30) · Flagship Core Survival Suite · Master Authority Volumes 1, 10, 14, 27, 42
**Save Authority:** Checksummed Sections `survivor_needs` & `survivor_radiation` via `SaveStoreHub` (Sections 101 & 102)
**Determinism Mandate:** Pure Domain Invariants under `ISeededRng` / `SeededRng.Fork("needs_radiation")`; Zero Wall-Clock reads; Zero `System.Random`

---

# SECTION I: COMPREHENSIVE ARCHITECTURAL OBJECTIVES & SYSTEM BOUNDARIES

Plan 01 addresses the foundational survival mechanics of *ASHFALL*. While preliminary implementations of `NeedsSystem` and `RadiationSystem` possessed rudimentary tick routines, they lacked comprehensive serialization round-trip verification, culture-invariant floating-point stability, internal organ dosimetry modeling, and multi-day state reconstruction guarantees (identified in Known Debt as issue **H10**).

This plan elevates survival physiology from simple local counters into a fully hardened, mathematically deterministic, engine-free domain subsystem:

```
+===================================================================================================+
|                                  ASHFALL SURVIVAL DOMAIN CORE                                     |
|  Assets/Ashfall.Core/Survivors/ & Assets/Ashfall.Core/Radiation/                                  |
+===================================================================================================+
                                                  │
         ┌────────────────────────────────────────┴────────────────────────────────────────┐
         ▼                                                                                 ▼
+─────────────────────────────────────────────────+       +─────────────────────────────────────────────────+
|               NEEDS SYSTEM (8 VITALS)           |       |            RADIATION & DOSIMETRY SYSTEM         |
|  - Caloric Energy (0 - 2,500 kcal)              |       |  - Whole-Body Absorbed Dose (cGy / Rad)         |
|  - Hydration Balance (0.0 - 100.0%)             |       |  - Acute Radiation Syndrome (ARS) State Machine |
|  - Somatic Exhaustion & Sleep Debt              |       |  - Internal Radioisotope Deposition (Sr, Cs, I) |
|  - Core Body Temperature (Hypo/Hyperthermia)    |       |  - Sub-Cellular DNA Repair & Apoptosis Kinetics |
|  - Pathogen & Sepsis Resistance                 |       |  - Biological Elimination & Chelation Clearance |
|  - Psychological Morale & Stress Load           |       |  - Somatic Mutation & Late Carcinogenesis Risk  |
|  - Pulmonary Gas Exchange (O2/CO2/Smoke)        |       |  - Environmental Flux & Shielding Attenuation   |
|  - Social Cohesion & Solitude Strain            |       |  - Triage Categorization (Minimal to Expectant) |
+─────────────────────────────────────────────────+       +─────────────────────────────────────────────────+
         │                                                                                 │
         └────────────────────────────────────────┬────────────────────────────────────────┘
                                                  ▼
+===================================================================================================+
|                     SERIALIZATION, ENCODING & SAVE-STATE ROUND-TRIP SEAM                          |
|  Assets/Ashfall.Core/Serialization/ & SaveStoreHub                                                |
|  - Section 101 (`survivor_needs`) & Section 102 (`survivor_radiation`)                            |
|  - Culture-Invariant IEEE 754 Roundtrip (R-Format Double / Float Formatting)                      |
|  - 64-Bit CRC SaveChecksum Protection Against State Corruption                                   |
|  - Fallback Decay Handlers for Headless Server and Multi-Agent Fast-Forward                       |
+===================================================================================================+
                                                  │
                                                  ▼
+===================================================================================================+
|                       XUNIT TEST VERIFICATION & FUZZING HARNESS (H10 GATES)                       |
|  Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs                                           |
|  - 100 Exhaustive Unit Tests: Full Round-Trip, Null Invariant, Dirty State, Checksum Stability   |
|  - Seeded Replay Invariant Verification: Identical State Reconstruction Across Machine Compiles    |
+===================================================================================================+
```

### 1.1 Non-Negotiable Invariants
1. **Engine-Free Core**: `Assets/Ashfall.Core/Survivors/` and `Assets/Ashfall.Core/Radiation/` must never import `Godot` or `UnityEngine`.
2. **Culture-Invariant Persistence**: All floating-point fields in save payloads must be serialized using `CultureInfo.InvariantCulture` or standard IEEE-754 binary streams. A German (`de-DE`) or French (`fr-FR`) locale must never substitute commas for decimal periods in string representations.
3. **Deterministic Biological Clock**: All metabolic decays, radiation biological half-life eliminations, and recovery ticks are driven by discrete integer simulation ticks, never OS wall-clock time (`DateTime.Now` / `DateTime.UtcNow`).
4. **Complete State Reconstruction**: Every private and public state variable required to resume continuous simulation must be captured. Restoring a captured state onto a fresh system instance must produce bit-identical tick outcomes compared to an un-serialized instance running consecutively.

---

# SECTION II: DEEP PHYSIOLOGICAL DOMAIN MODELS & MATHEMATICAL EQUATIONS

### 2.1 The 8-Vital Metabolic Needs System
Every human survivor in *ASHFALL* maintains eight coupled vital vectors governed by differential metabolic decay equations:

1. **Caloric Energy ($C_t \in [0, 2500]$ kcal)**:
   $$\frac{dC}{dt} = -BMR \cdot M_{\\text{activity}} \cdot M_{\\text{illness}} \cdot M_{\\text{cold}}$$
   - Basal Metabolic Rate ($BMR$): 65.0 kcal/hr baseline.
   - Activity Multipliers ($M_{\\text{activity}}$): Sleep (0.8x), Idle (1.0x), Light Labor (1.5x), Heavy Mining/Foundry (2.4x), Combat Expedition (3.2x).
   - Cold Multiplier ($M_{\\text{cold}}$): Increases by up to 1.8x when core body temperature drops below 36.0°C due to involuntary shivering thermogenesis.

2. **Hydration Balance ($H_t \in [0.0, 100.0]\%$)**:
   $$\frac{dH}{dt} = -R_{\\text{base}} \cdot \left(1.0 + \frac{\max(0, T_{\\text{ambient}} - 22.0)}{10.0}\right) \cdot M_{\\text{diaphoresis}}$$
   - At $H_t < 25.0\%$, survivor suffers severe dehydration delirium (-40% labor speed, -30% cryptanalysis and combat accuracy).
   - At $H_t \le 0.0\%$, acute hypovolemic shock inflicts $5.0$ somatic health damage per hour until death.

3. **Sleep Debt & Fatigue ($F_t \in [0.0, 100.0]$)**:
   - Accumulates at $4.16$ units/hr while awake. Decreases by $12.5$ units/hr while resting in an insulated bunk.
   - At $F_t > 80.0$, micro-sleep episodes occur during manual machinery operation (+35% industrial catastrophic accident probability).

4. **Core Body Temperature ($T_{\\text{core}} \in [28.0, 43.0]^\circ\text{C}$)**:
   $$\frac{dT_{\\text{core}}}{dt} = \frac{T_{\\text{ambient}} - T_{\\text{core}}}{R_{\\text{insulation}}} + \dot{Q}_{\\text{metabolic}} - \dot{Q}_{\\text{sweat}}$$
   - Hypothermia stages trigger at 35.0°C (mild), 32.0°C (moderate - stupor), and 28.0°C (ventricular fibrillation / cardiac arrest).

5. **Pathogen & Sepsis Resistance ($P_t \in [0.0, 100.0]$)**:
   - Modulated by immune suppression from acute radiation dose and sanitation state of living quarters.

6. **Psychological Morale & Stress ($M_t \in [0.0, 100.0]$)**:
   - Couples directly with subterranean confinement, bereavement from comrade demise, and dietary variety.

7. **Pulmonary Gas Exchange ($O_t \in [0.0, 100.0]\%$)**:
   - Degrades rapidly in unventilated underground chambers when carbon monoxide ($CO$) or radon gas accumulates.

8. **Social Cohesion ($S_t \in [0.0, 100.0]$)**:
   - Decays during solitary quarantine confinement; replenished by communal dining, radio listening circles, and medical palliative visits.

### 2.2 Ionizing Radiation & Acute Radiation Syndrome (ARS) Dynamics
The radiation model simulates both external whole-body gamma penetration and internal radioisotope deposition:

$$D_{\\text{total}}(t) = D_{\\text{external}}(t) + \sum_{k \in \\{\text{Sr90}, \text{Cs137}, \text{I131}\\}} D_{\\text{internal}, k}(t)$$

Internal organ isotopic burden follows two-compartment pharmacokinetic clearance:

$$\frac{dC_{\\text{organ}}}{dt} = I_{\\text{intake}}(t) \cdot f_{\\text{absorption}} - \left(\lambda_{\\text{physical}} + \lambda_{\\text{biological}} + K_{\\text{chelation}}\\right) \cdot C_{\\text{organ}}(t)$$

- **Strontium-90 ($^{90}\text{Sr}$)**: Bone-seeking osteotropic beta-emitter. Physical half-life: 28.8 years. Causes irreversible marrow suppression.
- **Cesium-137 ($^{137}\text{Cs}$)**: Uniform soft-tissue gamma/beta emitter. Biological half-life: 110 days in humans; accelerated by Prussian Blue chelator.
- **Iodine-131 ($^{131}\text{I}$)**: Concentrates exclusively in thyroid follicules. Blocked by prophylactic Potassium Iodide (KI).

---

# SECTION III: AUTHORITATIVE DATA SCHEMAS (`Assets/StreamingAssets/Data/`)

### 3.1 `needs_vital_parameters.json`
Authoritative metabolic consumption rates and damage thresholds.
```json
{
  "schema_version": 1,
  "catalog_id": "needs_vital_parameters_v1",
  "metabolic_constants": {
    "basal_metabolic_rate_kcal_per_hour": 65.0,
    "hydration_loss_ml_per_hour_baseline": 85.0,
    "sleep_debt_accumulation_per_hour": 4.167,
    "sleep_debt_clearance_per_hour_bunk": 12.5,
    "body_temp_nominal_celsius": 37.0,
    "hypothermia_mild_celsius": 35.0,
    "hypothermia_severe_celsius": 30.0,
    "hyperthermia_critical_celsius": 41.5
  },
  "damage_thresholds": {
    "starvation_caloric_depletion_health_damage_per_hour": 1.25,
    "dehydration_zero_health_damage_per_hour": 5.0,
    "hypothermia_health_damage_per_hour": 8.0,
    "asphyxiation_oxygen_depletion_health_damage_per_hour": 25.0
  }
}
```

### 3.2 `radiation_biological_constants.json`
Authoritative biological damage coefficients and ARS phase transition boundaries.
```json
{
  "schema_version": 1,
  "catalog_id": "radiation_biological_constants_v1",
  "ars_phases": [
    { "phase_id": "none", "min_dose_cgy": 0.0, "max_dose_cgy": 50.0, "nausea_prob": 0.0, "marrow_depression": 0.0 },
    { "phase_id": "mild_prodromal", "min_dose_cgy": 50.0, "max_dose_cgy": 150.0, "nausea_prob": 0.35, "marrow_depression": 0.15 },
    { "phase_id": "hematopoietic_manifest", "min_dose_cgy": 150.0, "max_dose_cgy": 400.0, "nausea_prob": 0.85, "marrow_depression": 0.65 },
    { "phase_id": "gastrointestinal_severe", "min_dose_cgy": 400.0, "max_dose_cgy": 800.0, "nausea_prob": 1.0, "marrow_depression": 0.95 },
    { "phase_id": "neurovascular_lethal", "min_dose_cgy": 800.0, "max_dose_cgy": 9999.0, "nausea_prob": 1.0, "marrow_depression": 1.0 }
  ],
  "isotopes": [
    { "isotope_id": "sr_90", "target_tissue": "bone_marrow", "biological_half_life_days": 18000, "chelation_agent_id": "chel_dmsa_01" },
    { "isotope_id": "cs_137", "target_tissue": "muscle_soft", "biological_half_life_days": 110, "chelation_agent_id": "prussian_blue_01" },
    { "isotope_id": "i_131", "target_tissue": "thyroid", "biological_half_life_days": 8, "chelation_agent_id": "potassium_iodide_01" }
  ]
}
```

---

# SECTION IV: COMPLETE ENGINE-FREE PURE C# DOMAIN IMPLEMENTATION

The following source artifacts reside in `Assets/Ashfall.Core/Survivors/` and `Assets/Ashfall.Core/Radiation/` (`netstandard2.1`):

### 4.1 `NeedsSystemStateDto.cs` & `NeedsSystem.cs`
```csharp
namespace Ashfall.Core.Survivors
{
    using System;
    using System.Globalization;

    [Serializable]
    public sealed class NeedsSystemStateDto
    {
        public int SchemaVersion { get; set; } = 1;
        public string SurvivorId { get; set; } = string.Empty;
        public float CaloricEnergy { get; set; } = 2000f;
        public float HydrationPercent { get; set; } = 100f;
        public float SleepDebt { get; set; } = 0f;
        public float CoreBodyTemperatureCelsius { get; set; } = 37.0f;
        public float PathogenResistance { get; set; } = 100f;
        public float Morale { get; set; } = 100f;
        public float OxygenationPercent { get; set; } = 100f;
        public float SocialCohesion { get; set; } = 100f;
        public int ConsecutiveStarvationHours { get; set; }
        public int ConsecutiveDehydrationHours { get; set; }

        public string ToChecksumString()
        {
            // Strict culture-invariant formatting (R format for lossless roundtrip)
            var inv = CultureInfo.InvariantCulture;
            return $"{SurvivorId}:{CaloricEnergy.ToString("R", inv)}:{HydrationPercent.ToString("R", inv)}:" +
                   $"{SleepDebt.ToString("R", inv)}:{CoreBodyTemperatureCelsius.ToString("R", inv)}:" +
                   $"{PathogenResistance.ToString("R", inv)}:{Morale.ToString("R", inv)}:" +
                   $"{OxygenationPercent.ToString("R", inv)}:{SocialCohesion.ToString("R", inv)}:" +
                   $"{ConsecutiveStarvationHours}:{ConsecutiveDehydrationHours}";
        }
    }

    public sealed class NeedsSystem
    {
        public string SurvivorId { get; }
        public float CaloricEnergy { get; private set; } = 2000f;
        public float HydrationPercent { get; private set; } = 100f;
        public float SleepDebt { get; private set; } = 0f;
        public float CoreBodyTemperatureCelsius { get; private set; } = 37.0f;
        public float PathogenResistance { get; private set; } = 100f;
        public float Morale { get; private set; } = 100f;
        public float OxygenationPercent { get; private set; } = 100f;
        public float SocialCohesion { get; private set; } = 100f;
        public int ConsecutiveStarvationHours { get; private set; }
        public int ConsecutiveDehydrationHours { get; private set; }

        public NeedsSystem(string survivorId)
        {
            SurvivorId = survivorId ?? throw new ArgumentNullException(nameof(survivorId));
        }

        public NeedsSystemStateDto CaptureState()
        {
            return new NeedsSystemStateDto
            {
                SchemaVersion = 1,
                SurvivorId = this.SurvivorId,
                CaloricEnergy = this.CaloricEnergy,
                HydrationPercent = this.HydrationPercent,
                SleepDebt = this.SleepDebt,
                CoreBodyTemperatureCelsius = this.CoreBodyTemperatureCelsius,
                PathogenResistance = this.PathogenResistance,
                Morale = this.Morale,
                OxygenationPercent = this.OxygenationPercent,
                SocialCohesion = this.SocialCohesion,
                ConsecutiveStarvationHours = this.ConsecutiveStarvationHours,
                ConsecutiveDehydrationHours = this.ConsecutiveDehydrationHours
            };
        }

        public void RestoreState(NeedsSystemStateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.SurvivorId != this.SurvivorId && !string.IsNullOrEmpty(dto.SurvivorId))
            {
                throw new InvalidOperationException($"SurvivorId mismatch during RestoreState. Expected '{SurvivorId}', got '{dto.SurvivorId}'.");
            }

            CaloricEnergy = Math.Max(0f, Math.Min(2500f, dto.CaloricEnergy));
            HydrationPercent = Math.Max(0f, Math.Min(100f, dto.HydrationPercent));
            SleepDebt = Math.Max(0f, Math.Min(100f, dto.SleepDebt));
            CoreBodyTemperatureCelsius = Math.Max(25f, Math.Min(45f, dto.CoreBodyTemperatureCelsius));
            PathogenResistance = Math.Max(0f, Math.Min(100f, dto.PathogenResistance));
            Morale = Math.Max(0f, Math.Min(100f, dto.Morale));
            OxygenationPercent = Math.Max(0f, Math.Min(100f, dto.OxygenationPercent));
            SocialCohesion = Math.Max(0f, Math.Min(100f, dto.SocialCohesion));
            ConsecutiveStarvationHours = Math.Max(0, dto.ConsecutiveStarvationHours);
            ConsecutiveDehydrationHours = Math.Max(0, dto.ConsecutiveDehydrationHours);
        }

        public void TickHourly(float ambientTempCelsius, float activityMultiplier, bool isSleeping, float ambientOxygenPercent)
        {
            // Caloric consumption
            float kcalBurn = 65.0f * activityMultiplier;
            if (CoreBodyTemperatureCelsius < 36.0f)
            {
                kcalBurn *= 1.4f; // Involuntary shivering thermogenesis
            }
            CaloricEnergy = Math.Max(0f, CaloricEnergy - kcalBurn);
            if (CaloricEnergy <= 0f) ConsecutiveStarvationHours++;
            else ConsecutiveStarvationHours = 0;

            // Hydration consumption
            float waterLossPercent = 3.5f * (1.0f + Math.Max(0f, ambientTempCelsius - 22f) / 15f);
            HydrationPercent = Math.Max(0f, HydrationPercent - waterLossPercent);
            if (HydrationPercent <= 0f) ConsecutiveDehydrationHours++;
            else ConsecutiveDehydrationHours = 0;

            // Sleep and Fatigue
            if (isSleeping)
            {
                SleepDebt = Math.Max(0f, SleepDebt - 12.5f);
            }
            else
            {
                SleepDebt = Math.Min(100f, SleepDebt + 4.167f);
            }

            // Core Body Temperature Newton cooling approximation
            float thermalGradient = ambientTempCelsius - CoreBodyTemperatureCelsius;
            float insulationConductance = 0.05f; // Standard wool blanket/clothing
            CoreBodyTemperatureCelsius += thermalGradient * insulationConductance;
            // Internal metabolic heating
            if (CaloricEnergy > 0f)
            {
                CoreBodyTemperatureCelsius += 0.1f;
            }
            CoreBodyTemperatureCelsius = Math.Max(26f, Math.Min(44f, CoreBodyTemperatureCelsius));

            // Oxygenation
            OxygenationPercent = Math.Max(0f, Math.Min(100f, ambientOxygenPercent));
        }

        public void ConsumeFood(float calories)
        {
            CaloricEnergy = Math.Min(2500f, CaloricEnergy + Math.Max(0f, calories));
            ConsecutiveStarvationHours = 0;
        }

        public void DrinkWater(float hydrationDeltaPercent)
        {
            HydrationPercent = Math.Min(100f, HydrationPercent + Math.Max(0f, hydrationDeltaPercent));
            ConsecutiveDehydrationHours = 0;
        }
    }
}
```

### 4.2 `RadiationSystemStateDto.cs` & `RadiationSystem.cs`
```csharp
namespace Ashfall.Core.Radiation
{
    using System;
    using System.Globalization;

    public enum AcuteRadiationPhase
    {
        Asymptomatic = 0,
        ProdromalNausea = 1,
        LatentRemission = 2,
        HematopoieticManifest = 3,
        GastrointestinalSevere = 4,
        NeurovascularTerminal = 5
    }

    [Serializable]
    public sealed class RadiationSystemStateDto
    {
        public int SchemaVersion { get; set; } = 1;
        public string SurvivorId { get; set; } = string.Empty;
        public float AccumulatedWholeBodyDoseCgy { get; set; }
        public AcuteRadiationPhase CurrentArsPhase { get; set; }
        public float BoneStrontium90Bq { get; set; }
        public float MuscleCesium137Bq { get; set; }
        public float ThyroidIodine131Bq { get; set; }
        public float DnaRepairCapacity { get; set; } = 100f;
        public int LatentPhaseRemainingTicks { get; set; }

        public string ToChecksumString()
        {
            var inv = CultureInfo.InvariantCulture;
            return $"{SurvivorId}:{AccumulatedWholeBodyDoseCgy.ToString("R", inv)}:{(int)CurrentArsPhase}:" +
                   $"{BoneStrontium90Bq.ToString("R", inv)}:{MuscleCesium137Bq.ToString("R", inv)}:" +
                   $"{ThyroidIodine131Bq.ToString("R", inv)}:{DnaRepairCapacity.ToString("R", inv)}:" +
                   $"{LatentPhaseRemainingTicks}";
        }
    }

    public sealed class RadiationSystem
    {
        public string SurvivorId { get; }
        public float AccumulatedWholeBodyDoseCgy { get; private set; }
        public AcuteRadiationPhase CurrentArsPhase { get; private set; } = AcuteRadiationPhase.Asymptomatic;
        public float BoneStrontium90Bq { get; private set; }
        public float MuscleCesium137Bq { get; private set; }
        public float ThyroidIodine131Bq { get; private set; }
        public float DnaRepairCapacity { get; private set; } = 100f;
        public int LatentPhaseRemainingTicks { get; private set; }

        public RadiationSystem(string survivorId)
        {
            SurvivorId = survivorId ?? throw new ArgumentNullException(nameof(survivorId));
        }

        public RadiationSystemStateDto CaptureState()
        {
            return new RadiationSystemStateDto
            {
                SchemaVersion = 1,
                SurvivorId = this.SurvivorId,
                AccumulatedWholeBodyDoseCgy = this.AccumulatedWholeBodyDoseCgy,
                CurrentArsPhase = this.CurrentArsPhase,
                BoneStrontium90Bq = this.BoneStrontium90Bq,
                MuscleCesium137Bq = this.MuscleCesium137Bq,
                ThyroidIodine131Bq = this.ThyroidIodine131Bq,
                DnaRepairCapacity = this.DnaRepairCapacity,
                LatentPhaseRemainingTicks = this.LatentPhaseRemainingTicks
            };
        }

        public void RestoreState(RadiationSystemStateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.SurvivorId != this.SurvivorId && !string.IsNullOrEmpty(dto.SurvivorId))
            {
                throw new InvalidOperationException($"SurvivorId mismatch during RadiationSystem.RestoreState. Expected '{SurvivorId}', got '{dto.SurvivorId}'.");
            }

            AccumulatedWholeBodyDoseCgy = Math.Max(0f, dto.AccumulatedWholeBodyDoseCgy);
            CurrentArsPhase = dto.CurrentArsPhase;
            BoneStrontium90Bq = Math.Max(0f, dto.BoneStrontium90Bq);
            MuscleCesium137Bq = Math.Max(0f, dto.MuscleCesium137Bq);
            ThyroidIodine131Bq = Math.Max(0f, dto.ThyroidIodine131Bq);
            DnaRepairCapacity = Math.Max(0f, Math.Min(100f, dto.DnaRepairCapacity));
            LatentPhaseRemainingTicks = Math.Max(0, dto.LatentPhaseRemainingTicks);
        }

        public void AbsorbExternalRadiation(float doseCgy)
        {
            if (doseCgy <= 0f) return;
            AccumulatedWholeBodyDoseCgy += doseCgy;
            UpdateArsPhase();
        }

        public void IngestContaminatedWaterOrFood(float sr90Bq, float cs137Bq, float i131Bq)
        {
            BoneStrontium90Bq += Math.Max(0f, sr90Bq);
            MuscleCesium137Bq += Math.Max(0f, cs137Bq);
            ThyroidIodine131Bq += Math.Max(0f, i131Bq);
        }

        public void TickDailyBiologicalElimination()
        {
            // Internal isotope biological decay
            MuscleCesium137Bq *= 0.9937f; // ~110-day half-life: 0.5^(1/110)
            ThyroidIodine131Bq *= 0.9170f; // ~8-day half-life: 0.5^(1/8)
            BoneStrontium90Bq *= 0.99996f; // ~28.8-year half-life

            // Internal dose contribution
            float internalDailyDoseCgy = (BoneStrontium90Bq * 0.0001f) + (MuscleCesium137Bq * 0.00005f) + (ThyroidIodine131Bq * 0.0002f);
            AccumulatedWholeBodyDoseCgy += internalDailyDoseCgy;

            UpdateArsPhase();
        }

        private void UpdateArsPhase()
        {
            if (AccumulatedWholeBodyDoseCgy >= 800f) CurrentArsPhase = AcuteRadiationPhase.NeurovascularTerminal;
            else if (AccumulatedWholeBodyDoseCgy >= 400f) CurrentArsPhase = AcuteRadiationPhase.GastrointestinalSevere;
            else if (AccumulatedWholeBodyDoseCgy >= 150f) CurrentArsPhase = AcuteRadiationPhase.HematopoieticManifest;
            else if (AccumulatedWholeBodyDoseCgy >= 50f) CurrentArsPhase = AcuteRadiationPhase.ProdromalNausea;
            else CurrentArsPhase = AcuteRadiationPhase.Asymptomatic;
        }
    }
}
```
"""

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(header)

print(f"Plan 01 Part 1 written! Current size: {len(header)} chars")
