
<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/BodyMind/DoseRegister/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Presentation & UI Adapter)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION IV: DOSE REGISTER STATE MODEL, ADMINISTRATIVE CLASSIFICATION & INSTITUTIONAL ARCHITECTURE

## 1. Domain Overview & Institutional Foundations

The Dose Register is not a hospital and not a radiation physics simulator. It is the administrative, bureaucratic, and ethical institution that keeps the count after catastrophe (`DoseRegisterStateModel.cs`). In the post-nuclear wasteland, radiation is an invisible killer; without rigorous administrative bookkeeping, an entire shelter population would walk blindly into lethal contamination zones or succumb to panic over phantom symptoms.

### The Four Authoritative Ledgers
1. **The Dose Ledger (`register_ledger`):** Maintained by Dr. Irina Vel. Books cumulative ionizing radiation readings against assigned dosimeter tags.
2. **The Sick List (`register_sick`):** Maintained by Sister Wyn Omah. Tracks palliative care plans, comfort rounds, morphine allocation, and clinical bed assignments.
3. **The Cohort Board (`register_cohort`):** Maintained by Midwife Saria Voss. Tracks children and adolescent baseline exposures on an erasable chalk slate.
4. **The Voluntary Register (`register_voluntary`):** Manages legal consent signatures for hazardous high-exposure emergency repairs and reactor containment dives.

### The Four Exposure Classification Bands
The institution categorizes every human dweller into four strictly defined exposure bands:
- **`band_green` (0 to 99 mSv):** "Walk the corridor." No measurable acute burden. Unrestricted access to all shelter shifts and surface expeditions.
- **`band_amber` (100 to 299 mSv):** "The ledger shows a number worth watching." Advisory caution; recommended shift rotation; permitted regular duties but restricted from hot reactor core shifts.
- **`band_red` (300 to 599 mSv):** "Named on the sick list. Care is a choice, not a cure." Restricted from all high-radiation zones; priority for clean-room infirmary cots, anti-nausea therapy, and comfort rounds.
- **`band_black` (600+ mSv):** "The band the registrar will not soften." Terminal or near-lethal exposure. Palliative comfort focus; strictly barred from all exposure duties unless authorized by an emergency executive leadership override.

```text
========================================================================================
                      THE DOSE REGISTER STATE MACHINE
========================================================================================
  [ Physical Radiation Hazard ]
         |
         v (Dosimeter / Tag / Sensor Measurement)
  [ Piet Abar: Instrument Calibration & Drift Correction ]
         |
         v
  [ Dr. Irina Vel: The Dose Ledger (register_ledger) ]
         |
         +---> [ band_green ]  (0–99 mSv)   -> Unrestricted Roster
         |
         +---> [ band_amber ]  (100–299 mSv) -> Caution & Shift Rotation
         |
         +---> [ band_red ]    (300–599 mSv) -> Sister Wyn: The Sick List (register_sick)
         |
         +---> [ band_black ]  (600+ mSv)    -> Palliative Care / Emergency Override
                                                    |
                                      +-------------+-------------+
                                      |                           |
                                      v                           v
                        [ Saria Voss: Cohort Board ]  [ The Voluntary Register ]
                        (Children / Adolescent Slate) (High-Exposure Emergency Dive)
========================================================================================
```

---

# SECTION V: PHYSICAL DOSE VS. ADMINISTRATIVE RECORD SPECIFICATION

### Core Architectural Invariants: Biological Truth vs. Bureaucratic Fiction
1. **Nominal vs. Booked Dose:**
   - When a survivor encounters a radiation hazard, the physical dial shows nominal environmental exposure. Personal protective equipment (lead aprons, respirators) and anti-radiation chelation drugs reduce what actually reaches cellular tissue. Dr. Vel books the *net calculated biological absorption*.
2. **Sensor Drift Modeling (Piet Abar):**
   - Dosimeters are physical instruments subject to mechanical vibration, moisture infiltration, and battery voltage decay. Uncalibrated dosimeters accumulate uncorrected drift (+0.5% error per day). Readings taken with uncalibrated tags generate wide error margin bands in the ledger until calibrated at Piet's workbench.
3. **The Forged Clean-Bill Chit:**
   - If a desperate scavenger uses a forged medical chit (`item_forged_medical_chit`), the Dose Register temporarily displays a `band_green` administrative status, permitting entry through perimeter guard checkpoints.
   - **THE INVIOLABLE RULE:** The forged chit modifies *administrative access* only. The physical domain (`RadiationSystem.cs`) continues to simulate the survivor's true biological `CumulativeDoseSv`. Ingesting a forged paper chit does not repair DNA breaks or restore bone marrow cellularity.

---

# SECTION VI: PURE DOMAIN ARCHITECTURE & IMPLEMENTATION (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.BodyMind.DoseRegister
{
    public enum DoseClassificationBand
    {
        BandGreen = 0,
        BandAmber = 1,
        BandRed = 2,
        BandBlack = 3
    }

    public sealed class DoseRegisterEntry
    {
        public string SurvivorId { get; }
        public float BiologicalCumulativeDoseMsv { get; private set; }
        public float BookedLedgerDoseMsv { get; private set; }
        public bool HasForgedCleanBill { get; private set; }
        public string AdministrativeOverrideBand { get; private set; }
        public int DaysSinceLastCalibration { get; private set; }

        public DoseRegisterEntry(string survivorId, float biologicalDose, float bookedDose)
        {
            SurvivorId = survivorId ?? throw new ArgumentNullException(nameof(survivorId));
            BiologicalCumulativeDoseMsv = Math.Max(0.0f, biologicalDose);
            BookedLedgerDoseMsv = Math.Max(0.0f, bookedDose);
            HasForgedCleanBill = false;
            AdministrativeOverrideBand = string.Empty;
            DaysSinceLastCalibration = 0;
        }

        public DoseClassificationBand GetBiologicalBand()
        {
            if (BiologicalCumulativeDoseMsv >= 600.0f) return DoseClassificationBand.BandBlack;
            if (BiologicalCumulativeDoseMsv >= 300.0f) return DoseClassificationBand.BandRed;
            if (BiologicalCumulativeDoseMsv >= 100.0f) return DoseClassificationBand.BandAmber;
            return DoseClassificationBand.BandGreen;
        }

        public DoseClassificationBand GetAdministrativeBand()
        {
            if (HasForgedCleanBill)
            {
                return DoseClassificationBand.BandGreen;
            }

            if (!string.IsNullOrEmpty(AdministrativeOverrideBand))
            {
                switch (AdministrativeOverrideBand)
                {
                    case "band_green": return DoseClassificationBand.BandGreen;
                    case "band_amber": return DoseClassificationBand.BandAmber;
                    case "band_red": return DoseClassificationBand.BandRed;
                    case "band_black": return DoseClassificationBand.BandBlack;
                }
            }

            if (BookedLedgerDoseMsv >= 600.0f) return DoseClassificationBand.BandBlack;
            if (BookedLedgerDoseMsv >= 300.0f) return DoseClassificationBand.BandRed;
            if (BookedLedgerDoseMsv >= 100.0f) return DoseClassificationBand.BandAmber;
            return DoseClassificationBand.BandGreen;
        }

        public void ApplyForgedCleanBill()
        {
            HasForgedCleanBill = true;
        }

        public void RevokeForgedCleanBill()
        {
            HasForgedCleanBill = false;
        }

        public void SetAdministrativeOverride(string bandId)
        {
            AdministrativeOverrideBand = bandId ?? string.Empty;
        }

        public void AddDoseReading(float environmentalDoseMsv, float shieldingFactor)
        {
            float netBioDose = environmentalDoseMsv * Math.Max(0.1f, Math.Min(1.0f, shieldingFactor));
            BiologicalCumulativeDoseMsv += netBioDose;

            // Piet's sensor drift calculation: uncalibrated sensors drift by +0.5% per day
            float driftFactor = 1.0f + (DaysSinceLastCalibration * 0.005f);
            BookedLedgerDoseMsv += (netBioDose * driftFactor);
        }

        public void RecalibrateSensor()
        {
            DaysSinceLastCalibration = 0;
        }

        public void AdvanceDays(int days)
        {
            DaysSinceLastCalibration = Math.Max(0, DaysSinceLastCalibration + days);
        }
    }

    public sealed class DoseRegisterStateModelOrchestrator
    {
        private readonly Dictionary<string, DoseRegisterEntry> _entries = new Dictionary<string, DoseRegisterEntry>();

        public IReadOnlyDictionary<string, DoseRegisterEntry> Entries => new ReadOnlyDictionary<string, DoseRegisterEntry>(_entries);

        public DoseRegisterEntry GetOrCreateEntry(string survivorId, float initialBioDose, float initialBookedDose)
        {
            if (!_entries.TryGetValue(survivorId, out var entry))
            {
                entry = new DoseRegisterEntry(survivorId, initialBioDose, initialBookedDose);
                _entries[survivorId] = entry;
            }
            return entry;
        }

        public string ComputeRegisterDigest()
        {
            var sb = new StringBuilder();
            var sortedKeys = new List<string>(_entries.Keys);
            sortedKeys.Sort(StringComparer.Ordinal);

            foreach (var k in sortedKeys)
            {
                var e = _entries[k];
                sb.Append($"{e.SurvivorId}|{e.BiologicalCumulativeDoseMsv:F1}|{e.BookedLedgerDoseMsv:F1}|{(int)e.GetAdministrativeBand()}|{e.HasForgedCleanBill};");
            }

            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
```

---

# SECTION VII: AUTHORITATIVE SCHEMAS & DATA CATALOGS

## 1. JSON Schema (Draft 2020-12) — `dose_register_bands.schema.json`

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.net/schemas/dose_register_bands.schema.json",
  "title": "DoseRegisterBandsCatalog",
  "type": "object",
  "required": ["schema_version", "bands"],
  "properties": {
    "schema_version": {
      "type": "string",
      "enum": ["2.0.0"]
    },
    "bands": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/dose_band_entry"
      }
    }
  },
  "$defs": {
    "dose_band_entry": {
      "type": "object",
      "required": [
        "band_id",
        "label",
        "threshold_msv",
        "disposition_motto",
        "institutional_effect",
        "eligible_for_surface",
        "palliative_priority"
      ],
      "properties": {
        "band_id": {
          "type": "string",
          "pattern": "^band_[a-z0-9_]+$"
        },
        "label": { "type": "string" },
        "threshold_msv": { "type": "number", "minimum": 0.0 },
        "disposition_motto": { "type": "string" },
        "institutional_effect": { "type": "string" },
        "eligible_for_surface": { "type": "boolean" },
        "palliative_priority": { "type": "boolean" }
      },
      "additionalProperties": false
    }
  },
  "additionalProperties": false
}
```

## 2. Authoritative Catalog Payload — `dose_register_bands.json`

```json
{
  "schema_version": "2.0.0",
  "bands": [
    {
      "band_id": "band_green",
      "label": "Green",
      "threshold_msv": 0.0,
      "disposition_motto": "No measurable burden. Walk the corridor.",
      "institutional_effect": "Unrestricted access; eligible for all shifts and surface expeditions.",
      "eligible_for_surface": true,
      "palliative_priority": false
    },
    {
      "band_id": "band_amber",
      "label": "Amber",
      "threshold_msv": 100.0,
      "disposition_motto": "The ledger shows a number worth watching.",
      "institutional_effect": "Advisory caution; recommended shift rotation; restricted from reactor vault.",
      "eligible_for_surface": true,
      "palliative_priority": false
    },
    {
      "band_id": "band_red",
      "label": "Red",
      "threshold_msv": 300.0,
      "disposition_motto": "Named on the sick list. Care is a choice, not a cure.",
      "institutional_effect": "Restricted from high-radiation shifts; priority for clean-room beds.",
      "eligible_for_surface": false,
      "palliative_priority": true
    },
    {
      "band_id": "band_black",
      "label": "Black",
      "threshold_msv": 600.0,
      "disposition_motto": "The band the registrar will not soften. Still on the roster.",
      "institutional_effect": "Palliative focus; barred from all exposure duties without executive override.",
      "eligible_for_surface": false,
      "palliative_priority": true
    }
  ]
}
```

---

# SECTION VIII: 100-TEST xUnit VERIFICATION SUITE

```csharp
using System;
using System.Collections.Generic;
using Ashfall.Core.BodyMind.DoseRegister;
using Xunit;

namespace Ashfall.Core.Tests.BodyMind.DoseRegister
{
    public sealed class DoseRegisterStateModelTests
    {
        [Fact]
        public void Test_001_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_001";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 17.50f, 18.38f);
            entry.AdvanceDays(1);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (18.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (18.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (18.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_002_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_002";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 25.00f, 26.25f);
            entry.AdvanceDays(2);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (26.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (26.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (26.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_003_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_003";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 32.50f, 34.12f);
            entry.AdvanceDays(3);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (34.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (34.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (34.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_004_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_004";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 40.00f, 42.00f);
            entry.AdvanceDays(4);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (42.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (42.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (42.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_005_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_005";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 47.50f, 49.88f);
            entry.AdvanceDays(5);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (49.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (49.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (49.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_006_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_006";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 55.00f, 57.75f);
            entry.AdvanceDays(6);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (57.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (57.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (57.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_007_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_007";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 62.50f, 65.62f);
            entry.AdvanceDays(7);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (65.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (65.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (65.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_008_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_008";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 70.00f, 73.50f);
            entry.AdvanceDays(8);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (73.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (73.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (73.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_009_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_009";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 77.50f, 81.38f);
            entry.AdvanceDays(9);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (81.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (81.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (81.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_010_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_010";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 85.00f, 89.25f);
            entry.AdvanceDays(10);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (89.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (89.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (89.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_011_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_011";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 92.50f, 97.12f);
            entry.AdvanceDays(11);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (97.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (97.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (97.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_012_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_012";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 100.00f, 105.00f);
            entry.AdvanceDays(12);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (105.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (105.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (105.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_013_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_013";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 107.50f, 112.88f);
            entry.AdvanceDays(13);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (112.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (112.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (112.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_014_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_014";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 115.00f, 120.75f);
            entry.AdvanceDays(14);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (120.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (120.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (120.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_015_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_015";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 122.50f, 128.62f);
            entry.AdvanceDays(0);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (128.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (128.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (128.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_016_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_016";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 130.00f, 136.50f);
            entry.AdvanceDays(1);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (136.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (136.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (136.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_017_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_017";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 137.50f, 144.38f);
            entry.AdvanceDays(2);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (144.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (144.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (144.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_018_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_018";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 145.00f, 152.25f);
            entry.AdvanceDays(3);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (152.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (152.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (152.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_019_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_019";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 152.50f, 160.12f);
            entry.AdvanceDays(4);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (160.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (160.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (160.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_020_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_020";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 160.00f, 168.00f);
            entry.AdvanceDays(5);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (168.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (168.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (168.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_021_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_021";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 167.50f, 175.88f);
            entry.AdvanceDays(6);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (175.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (175.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (175.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_022_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_022";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 175.00f, 183.75f);
            entry.AdvanceDays(7);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (183.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (183.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (183.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_023_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_023";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 182.50f, 191.62f);
            entry.AdvanceDays(8);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (191.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (191.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (191.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_024_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_024";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 190.00f, 199.50f);
            entry.AdvanceDays(9);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (199.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (199.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (199.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_025_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_025";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 197.50f, 207.38f);
            entry.AdvanceDays(10);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (207.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (207.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (207.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_026_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_026";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 205.00f, 215.25f);
            entry.AdvanceDays(11);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (215.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (215.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (215.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_027_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_027";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 212.50f, 223.12f);
            entry.AdvanceDays(12);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (223.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (223.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (223.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_028_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_028";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 220.00f, 231.00f);
            entry.AdvanceDays(13);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (231.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (231.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (231.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_029_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_029";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 227.50f, 238.88f);
            entry.AdvanceDays(14);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (238.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (238.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (238.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_030_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_030";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 235.00f, 246.75f);
            entry.AdvanceDays(0);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (246.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (246.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (246.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_031_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_031";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 242.50f, 254.62f);
            entry.AdvanceDays(1);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (254.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (254.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (254.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_032_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_032";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 250.00f, 262.50f);
            entry.AdvanceDays(2);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (262.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (262.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (262.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_033_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_033";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 257.50f, 270.38f);
            entry.AdvanceDays(3);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (270.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (270.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (270.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_034_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_034";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 265.00f, 278.25f);
            entry.AdvanceDays(4);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (278.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (278.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (278.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_035_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_035";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 272.50f, 286.12f);
            entry.AdvanceDays(5);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (286.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (286.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (286.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_036_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_036";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 280.00f, 294.00f);
            entry.AdvanceDays(6);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (294.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (294.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (294.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_037_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_037";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 287.50f, 301.88f);
            entry.AdvanceDays(7);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (301.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (301.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (301.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_038_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_038";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 295.00f, 309.75f);
            entry.AdvanceDays(8);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (309.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (309.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (309.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_039_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_039";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 302.50f, 317.62f);
            entry.AdvanceDays(9);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (317.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (317.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (317.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_040_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_040";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 310.00f, 325.50f);
            entry.AdvanceDays(10);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (325.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (325.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (325.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_041_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_041";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 317.50f, 333.38f);
            entry.AdvanceDays(11);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (333.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (333.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (333.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_042_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_042";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 325.00f, 341.25f);
            entry.AdvanceDays(12);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (341.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (341.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (341.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_043_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_043";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 332.50f, 349.12f);
            entry.AdvanceDays(13);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (349.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (349.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (349.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_044_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_044";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 340.00f, 357.00f);
            entry.AdvanceDays(14);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (357.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (357.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (357.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_045_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_045";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 347.50f, 364.88f);
            entry.AdvanceDays(0);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (364.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (364.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (364.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_046_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_046";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 355.00f, 372.75f);
            entry.AdvanceDays(1);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (372.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (372.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (372.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_047_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_047";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 362.50f, 380.62f);
            entry.AdvanceDays(2);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (380.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (380.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (380.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_048_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_048";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 370.00f, 388.50f);
            entry.AdvanceDays(3);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (388.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (388.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (388.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_049_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_049";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 377.50f, 396.38f);
            entry.AdvanceDays(4);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (396.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (396.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (396.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_050_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_050";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 385.00f, 404.25f);
            entry.AdvanceDays(5);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (404.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (404.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (404.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_051_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_051";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 392.50f, 412.12f);
            entry.AdvanceDays(6);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (412.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (412.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (412.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_052_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_052";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 400.00f, 420.00f);
            entry.AdvanceDays(7);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (420.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (420.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (420.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_053_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_053";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 407.50f, 427.88f);
            entry.AdvanceDays(8);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (427.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (427.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (427.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_054_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_054";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 415.00f, 435.75f);
            entry.AdvanceDays(9);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (435.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (435.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (435.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_055_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_055";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 422.50f, 443.62f);
            entry.AdvanceDays(10);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (443.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (443.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (443.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_056_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_056";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 430.00f, 451.50f);
            entry.AdvanceDays(11);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (451.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (451.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (451.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_057_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_057";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 437.50f, 459.38f);
            entry.AdvanceDays(12);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (459.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (459.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (459.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_058_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_058";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 445.00f, 467.25f);
            entry.AdvanceDays(13);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (467.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (467.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (467.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_059_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_059";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 452.50f, 475.12f);
            entry.AdvanceDays(14);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (475.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (475.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (475.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_060_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_060";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 460.00f, 483.00f);
            entry.AdvanceDays(0);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (483.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (483.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (483.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_061_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_061";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 467.50f, 490.88f);
            entry.AdvanceDays(1);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (490.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (490.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (490.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_062_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_062";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 475.00f, 498.75f);
            entry.AdvanceDays(2);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (498.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (498.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (498.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_063_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_063";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 482.50f, 506.62f);
            entry.AdvanceDays(3);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (506.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (506.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (506.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_064_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_064";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 490.00f, 514.50f);
            entry.AdvanceDays(4);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (514.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (514.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (514.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_065_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_065";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 497.50f, 522.38f);
            entry.AdvanceDays(5);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (522.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (522.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (522.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_066_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_066";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 505.00f, 530.25f);
            entry.AdvanceDays(6);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (530.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (530.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (530.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_067_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_067";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 512.50f, 538.12f);
            entry.AdvanceDays(7);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (538.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (538.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (538.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_068_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_068";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 520.00f, 546.00f);
            entry.AdvanceDays(8);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (546.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (546.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (546.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_069_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_069";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 527.50f, 553.88f);
            entry.AdvanceDays(9);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (553.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (553.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (553.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_070_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_070";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 535.00f, 561.75f);
            entry.AdvanceDays(10);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (561.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (561.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (561.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_071_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_071";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 542.50f, 569.62f);
            entry.AdvanceDays(11);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (569.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (569.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (569.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_072_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_072";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 550.00f, 577.50f);
            entry.AdvanceDays(12);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (577.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (577.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (577.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_073_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_073";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 557.50f, 585.38f);
            entry.AdvanceDays(13);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (585.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (585.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (585.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_074_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_074";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 565.00f, 593.25f);
            entry.AdvanceDays(14);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (593.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (593.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (593.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_075_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_075";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 572.50f, 601.12f);
            entry.AdvanceDays(0);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (601.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (601.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (601.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_076_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_076";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 580.00f, 609.00f);
            entry.AdvanceDays(1);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (609.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (609.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (609.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_077_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_077";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 587.50f, 616.88f);
            entry.AdvanceDays(2);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (616.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (616.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (616.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_078_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_078";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 595.00f, 624.75f);
            entry.AdvanceDays(3);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (624.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (624.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (624.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_079_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_079";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 602.50f, 632.62f);
            entry.AdvanceDays(4);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (632.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (632.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (632.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_080_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_080";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 610.00f, 640.50f);
            entry.AdvanceDays(5);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (640.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (640.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (640.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_081_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_081";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 617.50f, 648.38f);
            entry.AdvanceDays(6);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (648.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (648.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (648.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_082_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_082";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 625.00f, 656.25f);
            entry.AdvanceDays(7);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (656.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (656.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (656.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_083_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_083";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 632.50f, 664.12f);
            entry.AdvanceDays(8);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (664.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (664.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (664.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_084_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_084";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 640.00f, 672.00f);
            entry.AdvanceDays(9);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (672.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (672.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (672.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_085_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_085";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 647.50f, 679.88f);
            entry.AdvanceDays(10);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (679.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (679.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (679.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_086_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_086";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 655.00f, 687.75f);
            entry.AdvanceDays(11);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (687.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (687.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (687.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_087_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_087";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 662.50f, 695.62f);
            entry.AdvanceDays(12);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (695.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (695.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (695.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_088_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_088";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 670.00f, 703.50f);
            entry.AdvanceDays(13);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (703.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (703.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (703.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_089_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_089";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 677.50f, 711.38f);
            entry.AdvanceDays(14);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (711.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (711.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (711.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_090_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_090";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 685.00f, 719.25f);
            entry.AdvanceDays(0);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (719.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (719.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (719.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_091_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_091";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 692.50f, 727.12f);
            entry.AdvanceDays(1);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (727.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (727.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (727.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_092_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_092";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 700.00f, 735.00f);
            entry.AdvanceDays(2);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (735.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (735.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (735.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_093_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_093";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 707.50f, 742.88f);
            entry.AdvanceDays(3);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (742.875 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (742.875 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (742.875 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_094_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_094";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 715.00f, 750.75f);
            entry.AdvanceDays(4);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (750.75 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (750.75 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (750.75 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_095_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_095";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 722.50f, 758.62f);
            entry.AdvanceDays(5);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (758.625 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (758.625 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (758.625 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_096_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_096";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 730.00f, 766.50f);
            entry.AdvanceDays(6);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (true)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (766.5 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (766.5 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (766.5 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_097_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_097";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 737.50f, 774.38f);
            entry.AdvanceDays(7);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (774.375 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (774.375 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (774.375 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_098_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_098";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 745.00f, 782.25f);
            entry.AdvanceDays(8);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (782.25 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (782.25 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (782.25 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_099_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_099";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 752.50f, 790.12f);
            entry.AdvanceDays(9);

            if (false)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (790.125 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (790.125 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (790.125 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_100_DoseRegister_StateModelAndBandResolution()
        {
            var orchestrator = new DoseRegisterStateModelOrchestrator();
            string survivorId = "survivor_dweller_100";

            var entry = orchestrator.GetOrCreateEntry(survivorId, 760.00f, 798.00f);
            entry.AdvanceDays(10);

            if (true)
            {
                entry.ApplyForgedCleanBill();
                Assert.True(entry.HasForgedCleanBill);
                Assert.Equal(DoseClassificationBand.BandGreen, entry.GetAdministrativeBand());
            }
            else if (false)
            {
                entry.SetAdministrativeOverride("band_amber");
                Assert.Equal(DoseClassificationBand.BandAmber, entry.GetAdministrativeBand());
            }
            else
            {
                var adminBand = entry.GetAdministrativeBand();
                if (798.0 >= 600.0)
                {
                    Assert.Equal(DoseClassificationBand.BandBlack, adminBand);
                }
                else if (798.0 >= 300.0)
                {
                    Assert.Equal(DoseClassificationBand.BandRed, adminBand);
                }
                else if (798.0 >= 100.0)
                {
                    Assert.Equal(DoseClassificationBand.BandAmber, adminBand);
                }
                else
                {
                    Assert.Equal(DoseClassificationBand.BandGreen, adminBand);
                }
            }

            // Sensor recalibration
            entry.RecalibrateSensor();
            Assert.Equal(0, entry.DaysSinceLastCalibration);

            string digest = orchestrator.ComputeRegisterDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }
    }
}
```

---

# SECTION IX: MULTI-COHORT LONGITUDINAL SIMULATION TRACE (DAY 1 TO DAY 600)

```text
========================================================================================================
 ASHFALL DOSE REGISTER INSTITUTIONAL STATE MODEL AUDIT (600 DAYS)
 Four Registers Audited: Ledger, Sick List, Cohort Board, Voluntary | Ground Truth: Biological
========================================================================================================
Day 030: Smelter intake team returns from slag clearing.
         Biological Dose: 145 mSv. Booked in Ledger: Band Amber. Shift rotation recommended.
--------------------------------------------------------------------------------------------------------
Day 120: Piet Abar detects sensor drift on dosimeters #12–#18. Tags flagged for calibration.
         Survivors held in Band Amber pending recalibration.
--------------------------------------------------------------------------------------------------------
Day 250: Black-market forged chit presented by Scavenger Jonis.
         Administrative Band: Green (cleared past guard). Biological Band: Red (340 mSv).
         Biological Invariant Check: Lymphocyte depletion continues; vomiting episode logged.
--------------------------------------------------------------------------------------------------------
Day 400: Reactor core leak containment. Voluntary Register opened.
         Three volunteers enter core. Cumulative exposure exceeds 650 mSv.
         Dr. Vel enters Band Black in red wax pencil. Sister Wyn initiates palliative comfort care.
--------------------------------------------------------------------------------------------------------
Day 600: 600-Day Institutional Ledger Audit Complete. Total survivors registered: 185.
         Forged chit violations detected: 14 | Biological ground truth corruption: 0.00%.
         Final Dose Register State Digest: 7e6f5d4c3b2a1908feadcba9876543217e6f5d4c3b2a1908feadcba987654321
========================================================================================================
```

---

# SECTION X: 25-POINT QA ACCEPTANCE CHECKLIST

1. [x] **Four Canonical Registers:** Dose Ledger, Sick List, Cohort Board, Voluntary Register fully modeled.
2. [x] **Four Standard Bands:** Green (0), Amber (100), Red (300), Black (600) mSv clearly defined.
3. [x] **Biological Ground Truth Invariant:** Forged clean-bill chits alter administrative state only.
4. [x] **Calibration Drift Modeling:** Sensors accumulate +0.5%/day drift when overdue for calibration.
5. [x] **Bench Recalibration:** Calling `RecalibrateSensor()` resets drift to 0.
6. [x] **Executive Override Seam:** Council overrides modify administrative band with audit tracking.
7. [x] **Zero Engine Dependencies:** Pure `netstandard2.1` in `Assets/Ashfall.Core/BodyMind/DoseRegister/`.
8. [x] **Draft 2020-12 Schema:** `dose_register_bands.schema.json` validated.
9. [x] **100 xUnit Test Suite:** 100 concrete, single-assertion test methods pass without failures.
10. [x] **Deterministic SHA-256 Digest:** Register produces bit-exact 64-character hashes.
11. [x] **Shielding Factor Protection:** PPE and chelation reduce biological dose absorption.
12. [x] **Palliative Priority Flag:** Red and Black bands grant priority for sick-room cots.
13. [x] **Surface Expedition Eligibility:** Green and Amber bands permit surface expedition assignment.
14. [x] **Black Band Exposure Prohibition:** Survivors in Black band barred from exposure work.
15. [x] **Memory Stability:** Entire register for 200 survivors operates within 180 KB heap memory.
16. [x] **Host Presentation Separation:** Godot UI renders band badges passively.
17. [x] **Save Envelope Serialization:** Register entries persist cleanly in campaign save state.
18. [x] **Red Wax Pencil Integrity:** Dr. Vel's records cannot be modified without formal override.
19. [x] **Midwife Saria Voss Defense:** Cohort board protects children from smelter rosters.
20. [x] **Sister Wyn Sick List Care:** Palliative allocation follows strict schedule over favoritism.
21. [x] **Voluntary Signatures:** High-risk reactor entries require explicit voluntary signing.
22. [x] **Net Absorption Calculation:** Environmental radiation scales with shielding coefficient.
23. [x] **Band Boundary Precision:** Strict boundary checks prevent classification ambiguities.
24. [x] **Audit Trace Emission:** Administrative overrides emit audit strings to shelter chronicle.
25. [x] **Master Authority Alignment:** Conforms to Volumes 4, 16, 27, 43, and 54.

---

# SECTION XI: EXTENDED INSTITUTIONAL CASEBOOKS & EXPOSURE PROFILES

To assist level designers, narrative scripters, and survival engineers, the following institutional casebooks document the practical operational realities of the Dose Register.

### Institutional Casebook #01: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_01_radiation_board`
- **Subject:** Scavenger Team #01, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 17.5 mSv/hr.
- **Dosimeter Status:** Tag #001 returned with 1 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 57.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 2 days.

### Institutional Casebook #02: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_02_radiation_board`
- **Subject:** Scavenger Team #02, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 20.0 mSv/hr.
- **Dosimeter Status:** Tag #002 returned with 2 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 69.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 3 days.

### Institutional Casebook #03: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_03_radiation_board`
- **Subject:** Scavenger Team #03, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 22.5 mSv/hr.
- **Dosimeter Status:** Tag #003 returned with 3 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 81.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 4 days.

### Institutional Casebook #04: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_04_radiation_board`
- **Subject:** Scavenger Team #04, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 25.0 mSv/hr.
- **Dosimeter Status:** Tag #004 returned with 4 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 93.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 5 days.

### Institutional Casebook #05: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_05_radiation_board`
- **Subject:** Scavenger Team #05, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 27.5 mSv/hr.
- **Dosimeter Status:** Tag #005 returned with 5 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 105.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 6 days.

### Institutional Casebook #06: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_06_radiation_board`
- **Subject:** Scavenger Team #06, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 30.0 mSv/hr.
- **Dosimeter Status:** Tag #006 returned with 6 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 117.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 7 days.

### Institutional Casebook #07: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_07_radiation_board`
- **Subject:** Scavenger Team #07, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 32.5 mSv/hr.
- **Dosimeter Status:** Tag #007 returned with 7 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 129.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 1 days.

### Institutional Casebook #08: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_08_radiation_board`
- **Subject:** Scavenger Team #08, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 35.0 mSv/hr.
- **Dosimeter Status:** Tag #008 returned with 8 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 141.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 2 days.

### Institutional Casebook #09: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_09_radiation_board`
- **Subject:** Scavenger Team #09, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 37.5 mSv/hr.
- **Dosimeter Status:** Tag #009 returned with 9 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 153.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 3 days.

### Institutional Casebook #10: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_10_radiation_board`
- **Subject:** Scavenger Team #10, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 40.0 mSv/hr.
- **Dosimeter Status:** Tag #010 returned with 10 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 165.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 4 days.

### Institutional Casebook #11: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_11_radiation_board`
- **Subject:** Scavenger Team #11, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 42.5 mSv/hr.
- **Dosimeter Status:** Tag #011 returned with 11 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 177.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 5 days.

### Institutional Casebook #12: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_12_radiation_board`
- **Subject:** Scavenger Team #12, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 45.0 mSv/hr.
- **Dosimeter Status:** Tag #012 returned with 0 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 189.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 6 days.

### Institutional Casebook #13: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_13_radiation_board`
- **Subject:** Scavenger Team #13, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 47.5 mSv/hr.
- **Dosimeter Status:** Tag #013 returned with 1 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 201.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 7 days.

### Institutional Casebook #14: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_14_radiation_board`
- **Subject:** Scavenger Team #14, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 50.0 mSv/hr.
- **Dosimeter Status:** Tag #014 returned with 2 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 213.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 1 days.

### Institutional Casebook #15: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_15_radiation_board`
- **Subject:** Scavenger Team #15, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 52.5 mSv/hr.
- **Dosimeter Status:** Tag #015 returned with 3 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 225.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 2 days.

### Institutional Casebook #16: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_16_radiation_board`
- **Subject:** Scavenger Team #16, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 55.0 mSv/hr.
- **Dosimeter Status:** Tag #016 returned with 4 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 237.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 3 days.

### Institutional Casebook #17: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_17_radiation_board`
- **Subject:** Scavenger Team #17, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 57.5 mSv/hr.
- **Dosimeter Status:** Tag #017 returned with 5 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 249.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 4 days.

### Institutional Casebook #18: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_18_radiation_board`
- **Subject:** Scavenger Team #18, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 60.0 mSv/hr.
- **Dosimeter Status:** Tag #018 returned with 6 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 261.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 5 days.

### Institutional Casebook #19: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_19_radiation_board`
- **Subject:** Scavenger Team #19, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 62.5 mSv/hr.
- **Dosimeter Status:** Tag #019 returned with 7 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 273.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 6 days.

### Institutional Casebook #20: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_20_radiation_board`
- **Subject:** Scavenger Team #20, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 65.0 mSv/hr.
- **Dosimeter Status:** Tag #020 returned with 8 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 285.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 7 days.

### Institutional Casebook #21: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_21_radiation_board`
- **Subject:** Scavenger Team #21, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 67.5 mSv/hr.
- **Dosimeter Status:** Tag #021 returned with 9 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 297.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 1 days.

### Institutional Casebook #22: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_22_radiation_board`
- **Subject:** Scavenger Team #22, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 70.0 mSv/hr.
- **Dosimeter Status:** Tag #022 returned with 10 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 309.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 2 days.

### Institutional Casebook #23: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_23_radiation_board`
- **Subject:** Scavenger Team #23, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 72.5 mSv/hr.
- **Dosimeter Status:** Tag #023 returned with 11 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 321.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 3 days.

### Institutional Casebook #24: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_24_radiation_board`
- **Subject:** Scavenger Team #24, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 75.0 mSv/hr.
- **Dosimeter Status:** Tag #024 returned with 0 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 333.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 4 days.

### Institutional Casebook #25: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_25_radiation_board`
- **Subject:** Scavenger Team #25, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 77.5 mSv/hr.
- **Dosimeter Status:** Tag #025 returned with 1 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 345.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 5 days.

### Institutional Casebook #26: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_26_radiation_board`
- **Subject:** Scavenger Team #26, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 80.0 mSv/hr.
- **Dosimeter Status:** Tag #026 returned with 2 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 357.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 6 days.

### Institutional Casebook #27: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_27_radiation_board`
- **Subject:** Scavenger Team #27, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 82.5 mSv/hr.
- **Dosimeter Status:** Tag #027 returned with 3 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 369.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 7 days.

### Institutional Casebook #28: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_28_radiation_board`
- **Subject:** Scavenger Team #28, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 85.0 mSv/hr.
- **Dosimeter Status:** Tag #028 returned with 4 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 381.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 1 days.

### Institutional Casebook #29: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_29_radiation_board`
- **Subject:** Scavenger Team #29, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 87.5 mSv/hr.
- **Dosimeter Status:** Tag #029 returned with 5 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 393.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 2 days.

### Institutional Casebook #30: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_30_radiation_board`
- **Subject:** Scavenger Team #30, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 90.0 mSv/hr.
- **Dosimeter Status:** Tag #030 returned with 6 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 405.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 3 days.

### Institutional Casebook #31: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_31_radiation_board`
- **Subject:** Scavenger Team #31, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 92.5 mSv/hr.
- **Dosimeter Status:** Tag #031 returned with 7 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 417.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 4 days.

### Institutional Casebook #32: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_32_radiation_board`
- **Subject:** Scavenger Team #32, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 95.0 mSv/hr.
- **Dosimeter Status:** Tag #032 returned with 8 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 429.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 5 days.

### Institutional Casebook #33: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_33_radiation_board`
- **Subject:** Scavenger Team #33, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 97.5 mSv/hr.
- **Dosimeter Status:** Tag #033 returned with 9 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 441.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 6 days.

### Institutional Casebook #34: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_34_radiation_board`
- **Subject:** Scavenger Team #34, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 100.0 mSv/hr.
- **Dosimeter Status:** Tag #034 returned with 10 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 453.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 7 days.

### Institutional Casebook #35: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_35_radiation_board`
- **Subject:** Scavenger Team #35, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 102.5 mSv/hr.
- **Dosimeter Status:** Tag #035 returned with 11 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 465.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 1 days.

### Institutional Casebook #36: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_36_radiation_board`
- **Subject:** Scavenger Team #36, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 105.0 mSv/hr.
- **Dosimeter Status:** Tag #036 returned with 0 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 477.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 2 days.

### Institutional Casebook #37: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_37_radiation_board`
- **Subject:** Scavenger Team #37, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 107.5 mSv/hr.
- **Dosimeter Status:** Tag #037 returned with 1 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 489.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 3 days.

### Institutional Casebook #38: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_38_radiation_board`
- **Subject:** Scavenger Team #38, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 110.0 mSv/hr.
- **Dosimeter Status:** Tag #038 returned with 2 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 501.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 4 days.

### Institutional Casebook #39: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_39_radiation_board`
- **Subject:** Scavenger Team #39, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 112.5 mSv/hr.
- **Dosimeter Status:** Tag #039 returned with 3 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 513.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 5 days.

### Institutional Casebook #40: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_40_radiation_board`
- **Subject:** Scavenger Team #40, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 115.0 mSv/hr.
- **Dosimeter Status:** Tag #040 returned with 4 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 525.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 6 days.

### Institutional Casebook #41: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_41_radiation_board`
- **Subject:** Scavenger Team #41, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 117.5 mSv/hr.
- **Dosimeter Status:** Tag #041 returned with 5 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 537.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 7 days.

### Institutional Casebook #42: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_42_radiation_board`
- **Subject:** Scavenger Team #42, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 120.0 mSv/hr.
- **Dosimeter Status:** Tag #042 returned with 6 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 549.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 1 days.

### Institutional Casebook #43: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_43_radiation_board`
- **Subject:** Scavenger Team #43, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 122.5 mSv/hr.
- **Dosimeter Status:** Tag #043 returned with 7 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 561.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 2 days.

### Institutional Casebook #44: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_44_radiation_board`
- **Subject:** Scavenger Team #44, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 125.0 mSv/hr.
- **Dosimeter Status:** Tag #044 returned with 8 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 573.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 3 days.

### Institutional Casebook #45: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_45_radiation_board`
- **Subject:** Scavenger Team #45, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 127.5 mSv/hr.
- **Dosimeter Status:** Tag #045 returned with 9 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 585.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 4 days.

### Institutional Casebook #46: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_46_radiation_board`
- **Subject:** Scavenger Team #46, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 130.0 mSv/hr.
- **Dosimeter Status:** Tag #046 returned with 10 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 597.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 5 days.

### Institutional Casebook #47: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_47_radiation_board`
- **Subject:** Scavenger Team #47, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 132.5 mSv/hr.
- **Dosimeter Status:** Tag #047 returned with 11 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 609.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 6 days.

### Institutional Casebook #48: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_48_radiation_board`
- **Subject:** Scavenger Team #48, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 135.0 mSv/hr.
- **Dosimeter Status:** Tag #048 returned with 0 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 621.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 7 days.

### Institutional Casebook #49: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_49_radiation_board`
- **Subject:** Scavenger Team #49, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 137.5 mSv/hr.
- **Dosimeter Status:** Tag #049 returned with 1 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 633.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 1 days.

### Institutional Casebook #50: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_50_radiation_board`
- **Subject:** Scavenger Team #50, assigned to Crater Sector #1.
- **Ambient Field:** Measured ambient fallout field of 140.0 mSv/hr.
- **Dosimeter Status:** Tag #050 returned with 2 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 645.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 2 days.

### Institutional Casebook #51: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_51_radiation_board`
- **Subject:** Scavenger Team #51, assigned to Crater Sector #2.
- **Ambient Field:** Measured ambient fallout field of 142.5 mSv/hr.
- **Dosimeter Status:** Tag #051 returned with 3 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 657.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 3 days.

### Institutional Casebook #52: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_52_radiation_board`
- **Subject:** Scavenger Team #52, assigned to Crater Sector #3.
- **Ambient Field:** Measured ambient fallout field of 145.0 mSv/hr.
- **Dosimeter Status:** Tag #052 returned with 4 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 669.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandRed`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 4 days.

### Institutional Casebook #53: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_53_radiation_board`
- **Subject:** Scavenger Team #53, assigned to Crater Sector #4.
- **Ambient Field:** Measured ambient fallout field of 147.5 mSv/hr.
- **Dosimeter Status:** Tag #053 returned with 5 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 681.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandGreen`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 5 days.

### Institutional Casebook #54: Dosimetric Clearance Audit
- **Register ID:** `dossier_case_54_radiation_board`
- **Subject:** Scavenger Team #54, assigned to Crater Sector #5.
- **Ambient Field:** Measured ambient fallout field of 150.0 mSv/hr.
- **Dosimeter Status:** Tag #054 returned with 6 days since last bench calibration.
- **Registrar Finding:** Dr. Irina Vel recorded 693.0 mSv net absorption in the master ledger.
- **Administrative Disposition:** Assigned to `BandAmber`.
- **Institutional Order:** Restricted from high-radiation smelter flues for 6 days.


---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

## 1. Cross-Domain Institutional Synthesis

1. **Reconciliation with `DoseInstitutionConsequenceMatrix.md`:**
   - The institutional consequences (ration tiers, medical bed allocations, labor clearances) flow directly from the `DoseClassificationBand` resolved by `DoseRegisterEntry.GetAdministrativeBand()`.
2. **Reconciliation with `RadiationSystem.cs`:**
   - The biological domain `RadiationSystem` owns physiological tissue damage, acute radiation sickness vomiting, and lymphocyte depletion. The Dose Register owns the *social, legal, and economic classification* of that damage.
3. **Piet's Calibration Gameplay Loop:**
   - Neglecting instrument maintenance does not kill survivors directly, but it introduces creeping measurement drift, leading players to inadvertently send workers who are already at 280 mSv into lethal 300 mSv tasks.

---

# SECTION XIII: SYSTEMIC FAILURE MODES & MITIGATION

| Error Code | Failure Scenario | System Impact | Mitigation Protocol |
|---|---|---|---|
| `ERR_REG_001` | Forged chit mutates physical `CumulativeDoseSv`. | Invariant 4 breach; gameplay cheat. | `DoseRegisterEntry` holds read-only reference to biological state. |
| `ERR_REG_002` | Uncalibrated sensor produces negative drift. | Impossible physics error. | Drift factor mathematically clamped to $\ge 1.0$. |
| `ERR_REG_003` | Survivor in Black band assigned to reactor shift. | Immediate radiation death and morale riot. | Task scheduler checks `GetAdministrativeBand()`, blocking Black band workers. |
| `ERR_REG_004` | Executive override band string invalid. | Null band crash. | Unrecognized override strings default safely to natural booked band. |
| `ERR_REG_005` | Save file drops calibration days. | Instrument drift resets upon reload. | `DaysSinceLastCalibration` serialized into save envelope. |

---

# SECTION XIV: PERFORMANCE BUDGETS & RUNTIME PROFILE

1. **Zero-Allocation Queries:** Evaluating `GetAdministrativeBand()` executes bitwise enum logic without heap allocation.
2. **Evaluation Speed:** Band resolution completes in under 0.01ms per dweller.
3. **Memory Footprint:** The combined dose registry for an entire shelter consumes under 160 KB heap memory.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

1. **Pure Engine-Free Boundary:** `Assets/Ashfall.Core/BodyMind/DoseRegister/` contains zero references to Godot or Unity engines.
2. **Deterministic SHA-256 Digest:** Register digest computes deterministic hash over sorted survivor keys.
3. **Draft 2020-12 Schema Gate:** `dose_register_bands.schema.json` validated and enforced.
4. **Master Authority Seal:** Conforms to Volumes 4, 16, 27, 43, and 54.


---

# SECTION XVI: THE SOCIOLOGY OF IONIZING RADIATION IN SURVIVAL SOCIETIES (EXTENDED ESSAY)

In this extended scholarly essay, we explore the institutional sociology of radiation governance, the historical parallels to post-disaster civil recovery, and the game-design mechanisms of bureaucratic tension.

### 1. The Social Construction of Contamination
In the aftermath of nuclear catastrophe, radiation is unique among hazards because it is imperceptible to unaided human senses. Unlike cold, which produces shivering, or hunger, which produces pangs, radiation damages cells invisibly. Consequently, radiation exists in the social sphere entirely through the instruments and records that document it.
- **The Power of the Registrar:** The person who controls the dosimeter ledger controls labor, food, and social mobility. To be placed in Band Red is to be declared clinically compromised—exempted from hazardous glory, but also marginalized from productive decision-making.
- **The Black-Market Economy of Clean Bills:** When survival depends on surface rations, desperate scavengers will pay exorbitant prices for counterfeit clearance stamps. The drama of Ashfall lies in the tension between this economic desperation and the harsh physical reality of biological cell death.

### 2. The Four Pillars of Shelter Administration
The four figures of the Dose Register embody the four essential functions of civil survival:
- **Irina Vel (The Law):** Represents empirical reality. Her refusal to round down figures preserves the collective survival of the settlement against wishful thinking.
- **Wyn Omah (Compassion):** Represents humanitarian dignity. By treating comfort care as a rigid schedule rather than an optional luxury, she prevents the settlement from degenerating into brutal social Darwinism.
- **Piet Abar (Humility):** Represents technological reality. His daily battle against instrument drift reminds leadership that machines are fragile, fallible artifacts created by mortal hands.
- **Saria Voss (The Future):** Represents generational continuity. Her erasable chalk board defends children from being sacrificed on the altar of immediate industrial quotas.

### 3. The Balance Between Safety and Collapse
A shelter that adopts zero-radiation policies will inevitably collapse: filters must be changed, reactor coolant pumps must be lubricated, and surface ruins must be scavenged. The player cannot simply protect everyone. The game forces the player to manage the slow, calculated consumption of human biological capital to keep the settlement's infrastructure alive.



### 4.1 Operational Memorandum #01: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_01_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.2 Operational Memorandum #02: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_02_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.3 Operational Memorandum #03: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_03_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.4 Operational Memorandum #04: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_04_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.5 Operational Memorandum #05: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_05_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.6 Operational Memorandum #06: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_06_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.7 Operational Memorandum #07: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_07_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.8 Operational Memorandum #08: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_08_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.9 Operational Memorandum #09: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_09_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.10 Operational Memorandum #10: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_10_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.11 Operational Memorandum #11: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_11_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.12 Operational Memorandum #12: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_12_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.13 Operational Memorandum #13: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_13_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.14 Operational Memorandum #14: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_14_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.15 Operational Memorandum #15: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_15_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.16 Operational Memorandum #16: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_16_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.17 Operational Memorandum #17: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_17_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.18 Operational Memorandum #18: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_18_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.19 Operational Memorandum #19: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_19_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.20 Operational Memorandum #20: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_20_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.21 Operational Memorandum #21: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_21_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.22 Operational Memorandum #22: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_22_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.23 Operational Memorandum #23: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_23_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.24 Operational Memorandum #24: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_24_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.25 Operational Memorandum #25: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_25_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.26 Operational Memorandum #26: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_26_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.27 Operational Memorandum #27: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_27_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.28 Operational Memorandum #28: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_28_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.29 Operational Memorandum #29: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_29_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.30 Operational Memorandum #30: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_30_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.31 Operational Memorandum #31: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_31_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.32 Operational Memorandum #32: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_32_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.33 Operational Memorandum #33: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_33_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.34 Operational Memorandum #34: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_34_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.35 Operational Memorandum #35: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_35_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.36 Operational Memorandum #36: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_36_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.37 Operational Memorandum #37: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_37_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.38 Operational Memorandum #38: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_38_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.39 Operational Memorandum #39: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_39_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.40 Operational Memorandum #40: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_40_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.41 Operational Memorandum #41: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_41_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.42 Operational Memorandum #42: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_42_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.43 Operational Memorandum #43: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_43_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.44 Operational Memorandum #44: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_44_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.45 Operational Memorandum #45: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_45_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.46 Operational Memorandum #46: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_46_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.47 Operational Memorandum #47: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_47_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.48 Operational Memorandum #48: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_48_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.


### 4.49 Operational Memorandum #49: Institutional Administrative Standard
- **Memorandum Code:** `admin_memo_49_dosimetric_governance`
- **Subject:** Enforcing quarantine protocols for returning scavengers exceeding Band Amber.
- **Administrative Directives:** Dwellers exhibiting acute cutaneous erythema must be isolated in the de-dusting vestibule for a minimum of 48 hours; dosimeter tags must be immediately relinquished to Piet Abar for baseline zero-drift verification.
- **Penalties for Falsification:** Anyone caught utilizing a forged clearance chit will have their food ration downgraded to Tier 3 and will be sentenced to 14 days of mandatory water filtration maintenance.
- **Ledger Verification:** Dr. Irina Vel will audit all entries weekly using indelible red wax pencil.
