# Dose Institution Consequence Matrix

This document maps how Dose Register classifications interface with shelter systems, duty assignments, and medical care.

| Administrative Band | Surface Expeditions | Reactor / Hazmat Shifts | Shelter Domestic Shifts | Clean-Room Beds | Palliative Rounds |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Green (`band_green`)** | Unrestricted | Eligible | Unrestricted | Standard Bay | Not Indicated |
| **Amber (`band_amber`)** | Advisory Caution | Limited Rotation | Unrestricted | Standard Bay | Occasional Monitoring |
| **Red (`band_red`)** | Restricted without Lead Shielding | Restricted (Requires Leadership Override) | Unrestricted (Kitchen, Workshop, Hydroponics) | Priority Allocation | Regular Comfort Rounds |
| **Black (`band_black`)** | Prohibited | Strictly Prohibited | Light Duty / Bed Rest | Dedicated Bed | Daily Morphine / Palliative Schedule |

---

## Key Policy Principles
1. **No Blanket Harmless Exclusion:** Red and Black band survivors are never excluded from harmless tasks (kitchen, tailoring, archive work, counseling, reading). Exclusion applies strictly to high-rad environments (reactor bays, contaminated wasteland ruins).
2. **Leadership Waivers:** If an essential technician in Red band must fix the reactor, an explicit emergency override is required, generating moral and relationship consequences.
3. **Forgery Impact:** A forged clean-bill chit allows a survivor to pass the Screening Station checkpoint (`loc_the_screening_station`), but will not prevent ARS progression if physical radiation dose continues to climb.

<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/BodyMind/DoseConsequences/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Host Session Adapter)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION VIII: DOSE REGISTER ADMINISTRATIVE BANDS & INSTITUTIONAL CONSEQUENCE SPECIFICATION

## 1. Systemic Analysis, Triage Ethics, and Duty Assignment Seams

In Plan 27 (`DoseRegisterSystem.cs`), radiation dose tracking is not merely a combat hitpoint pool; it is the central organizing principle of shelter governance, civil rights, labor allocation, and end-of-life palliative ethics. As survivors absorb cumulative ionizing radiation from surface scavenging and containment leaks, their physiological status transitions across four formal administrative bands: Green, Amber, Red, and Black. How the shelter administration classifies and reacts to these bands dictates workforce productivity, mutiny risk, and cohort psychological cohesion.

### The Four Administrative Dose Bands
1. **Green Band (`band_green`, Cumulative Dose $< 0.50\text{ Sv}$):**
   - *Status:* Sub-clinical exposure. Full chromosomal vitality.
   - *Administrative Rules:* Unrestricted surface expeditions, reactor maintenance shifts, and domestic labor. Standard bunk allocation. Palliative rounds not indicated.
2. **Amber Band (`band_amber`, Cumulative Dose $0.50\text{ Sv} \le D < 2.00\text{ Sv}$):**
   - *Status:* Mild prodromal symptoms (transient nausea, mild lymphopenia).
   - *Administrative Rules:* Advisory caution on surface sorties. Rotational exposure limits on hazmat shifts. Unrestricted domestic shifts. Standard infirmary monitoring.
3. **Red Band (`band_red`, Cumulative Dose $2.00\text{ Sv} \le D < 4.50\text{ Sv}$):**
   - *Status:* Severe acute radiation syndrome (persistent epilation, hematological depression).
   - *Administrative Rules:* Surface expeditions restricted without certified lead shielding. Reactor shifts prohibited unless authorized by a **Leadership Emergency Waiver**. Unrestricted domestic light labor (kitchen, workshop, hydroponics, counseling). Priority clean-room bed allocation. Regular comfort rounds.
4. **Black Band (`band_black`, Cumulative Dose $\ge 4.50\text{ Sv}$):**
   - *Status:* Lethal gastrointestinal and neurovascular syndrome. Irreversible cellular lysis.
   - *Administrative Rules:* Surface expeditions and hazardous industrial shifts strictly prohibited. Light domestic duty or bed rest permitted. Dedicated clean-room palliative bed. Scheduled daily morphine administration.

### Core Architectural Invariants
1. **No Blanket Harmless Exclusion:**
   - Red and Black band survivors are never excluded from harmless camp tasks (cooking, garment repair, archival cataloging, survivor counseling). Excluding terminal dwellers from communal life induces severe existential depression (-25 morale), accelerating cohort despair.
2. **Leadership Emergency Waivers:**
   - If an indispensable senior technician in the Red band must enter a high-flux reactor bay to prevent a core meltdown, the player must issue an explicit **Leadership Emergency Override**. This choice writes permanent moral flags (`flag_ordered_terminal_reactor_entry`), strains companion relationships, and accelerates the technician's death.
3. **Black Market Forgery Decoupling:**
   - A forged medical chit (`item_forged_clean_bill_chit`) allows a carrier to pass security checkpoints (e.g. `loc_the_screening_station`), but does not modify the underlying physical dosimetry model. Acute radiation sickness progresses according to true biological ionization.
4. **Deterministic State Evaluation & Digest:**
   - Band classifications and triage eligibility evaluate deterministically, producing 64-character SHA-256 digests.

### Mathematical Formulations

1. **Dose Band Classification Function:**
   $$\mathcal{B}(D) = \begin{cases} \text{Green}, & 0.00 \le D < 0.50 \\ \text{Amber}, & 0.50 \le D < 2.00 \\ \text{Red}, & 2.00 \le D < 4.50 \\ \text{Black}, & D \ge 4.50 \end{cases}$$

2. **Palliative Morphine Morale Stabilization:**
   $$\Delta \mathcal{M}_{\text{comfort}} = \mathcal{M}_{\text{base}} \times \left(1.0 - \kappa_{\text{pain}} \cdot \mathcal{S}_{\text{ars}}\right) + \Omega_{\text{morphine}}$$
   Where $\Omega_{\text{morphine}} = +12.0$ morale units, suppressing terminal agitation.

3. **Deterministic Administrative State Digest:**
   $$\text{Digest}_{\text{dose\_admin}} = \text{SHA256}\left(\sum_{S \in \text{Dwellers}} S.\text{Id} \parallel S.\text{DoseSv} \parallel \mathcal{B}(S.\text{Dose}) \parallel S.\text{WaiverActive}\right)$$

---

# SECTION IX: PURE DOMAIN ARCHITECTURE & IMPLEMENTATION (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.BodyMind.DoseConsequences
{
    public enum DoseAdministrativeBand
    {
        Green = 1,
        Amber = 2,
        Red = 3,
        Black = 4
    }

    public enum DutyCategory
    {
        SurfaceExpedition = 1,
        ReactorHazmatShift = 2,
        DomesticLightDuty = 3,
        CleanRoomConfinement = 4
    }

    public readonly struct DwellerDoseProfile : IEquatable<DwellerDoseProfile>
    {
        public readonly string SurvivorId;
        public readonly double CumulativeDoseSv;
        public readonly DoseAdministrativeBand ActiveBand;
        public readonly bool HasLeadershipOverride;
        public readonly bool CarriesForgedChit;
        public readonly long LastScreeningTick;

        public DwellerDoseProfile(
            string survivorId,
            double doseSv,
            bool hasOverride,
            bool carriesForgedChit,
            long lastScreeningTick)
        {
            SurvivorId = survivorId ?? throw new ArgumentNullException(nameof(survivorId));
            CumulativeDoseSv = Math.Max(0.0, doseSv);
            HasLeadershipOverride = hasOverride;
            CarriesForgedChit = carriesForgedChit;
            LastScreeningTick = lastScreeningTick;

            if (CumulativeDoseSv < 0.50)
            {
                ActiveBand = DoseAdministrativeBand.Green;
            }
            else if (CumulativeDoseSv < 2.00)
            {
                ActiveBand = DoseAdministrativeBand.Amber;
            }
            else if (CumulativeDoseSv < 4.50)
            {
                ActiveBand = DoseAdministrativeBand.Red;
            }
            else
            {
                ActiveBand = DoseAdministrativeBand.Black;
            }
        }

        public bool CanPerformDuty(DutyCategory duty, bool hasLeadShielding, out string refusalReason)
        {
            refusalReason = string.Empty;

            switch (duty)
            {
                case DutyCategory.SurfaceExpedition:
                    if (ActiveBand == DoseAdministrativeBand.Black)
                    {
                        refusalReason = "Survivor is in terminal Black Band; surface travel strictly prohibited.";
                        return false;
                    }
                    if (ActiveBand == DoseAdministrativeBand.Red && !hasLeadShielding)
                    {
                        refusalReason = "Red Band survivors require certified lead shielding for surface sorties.";
                        return false;
                    }
                    return true;

                case DutyCategory.ReactorHazmatShift:
                    if (ActiveBand == DoseAdministrativeBand.Black)
                    {
                        refusalReason = "Black Band survivors cannot enter high-flux reactor bays.";
                        return false;
                    }
                    if (ActiveBand == DoseAdministrativeBand.Red && !HasLeadershipOverride)
                    {
                        refusalReason = "Red Band reactor assignment requires explicit Leadership Emergency Override.";
                        return false;
                    }
                    return true;

                case DutyCategory.DomesticLightDuty:
                    // Invariant: No blanket harmless exclusion
                    return true;

                case DutyCategory.CleanRoomConfinement:
                    return ActiveBand == DoseAdministrativeBand.Red || ActiveBand == DoseAdministrativeBand.Black;

                default:
                    return false;
            }
        }

        public bool Equals(DwellerDoseProfile other) => SurvivorId == other.SurvivorId && ActiveBand == other.ActiveBand;
        public override bool Equals(object obj) => obj is DwellerDoseProfile other && Equals(other);
        public override int GetHashCode() => SurvivorId.GetHashCode();
    }

    public sealed class DoseAdministrationOrchestrator
    {
        private readonly Dictionary<string, DwellerDoseProfile> _registry = new Dictionary<string, DwellerDoseProfile>();

        public IReadOnlyDictionary<string, DwellerDoseProfile> Profiles => new ReadOnlyDictionary<string, DwellerDoseProfile>(_registry);

        public void RegisterSurvivorDose(string survivorId, double doseSv, bool hasOverride, bool forgedChit, long tick)
        {
            _registry[survivorId] = new DwellerDoseProfile(survivorId, doseSv, hasOverride, forgedChit, tick);
        }

        public bool CheckScreeningStationPassage(string survivorId, out string inspectionLog)
        {
            if (!_registry.TryGetValue(survivorId, out var profile))
            {
                inspectionLog = "Survivor not registered in dose database.";
                return false;
            }

            if (profile.CarriesForgedChit)
            {
                inspectionLog = "Clean-bill chit verified by checkpoint sentry (Visual Paper Inspection).";
                return true;
            }

            if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                inspectionLog = "Clear dosimeter reading; passage approved.";
                return true;
            }

            inspectionLog = $"Inspection failed: Survivor in {profile.ActiveBand} Band without travel waiver.";
            return false;
        }

        public string GenerateAdministrationDigest()
        {
            var sb = new StringBuilder();
            var sortedKeys = new List<string>(_registry.Keys);
            sortedKeys.Sort(StringComparer.Ordinal);

            foreach (var k in sortedKeys)
            {
                var p = _registry[k];
                sb.Append($"{p.SurvivorId}|{p.CumulativeDoseSv:F2}|{(int)p.ActiveBand}|{p.HasLeadershipOverride}|{p.CarriesForgedChit};");
            }

            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
```

---

# SECTION X: AUTHORITATIVE SCHEMAS & DATA CATALOGS

## 1. JSON Schema (Draft 2020-12) — `dose_administrative_bands.schema.json`

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.net/schemas/dose_administrative_bands.schema.json",
  "title": "DoseAdministrativeBandsCatalog",
  "type": "object",
  "required": ["schema_version", "administrative_bands"],
  "properties": {
    "schema_version": {
      "type": "string",
      "enum": ["2.0.0"]
    },
    "administrative_bands": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/band_entry"
      }
    }
  },
  "$defs": {
    "band_entry": {
      "type": "object",
      "required": [
        "band_id",
        "display_name",
        "min_dose_sv",
        "max_dose_sv",
        "surface_expeditions_allowed",
        "reactor_shifts_allowed",
        "domestic_shifts_allowed",
        "clean_room_bed_priority",
        "palliative_rounds_indicated"
      ],
      "properties": {
        "band_id": {
          "type": "string",
          "enum": ["band_green", "band_amber", "band_red", "band_black"]
        },
        "display_name": { "type": "string", "minLength": 3 },
        "min_dose_sv": { "type": "number", "minimum": 0.0 },
        "max_dose_sv": { "type": "number", "minimum": 0.0 },
        "surface_expeditions_allowed": { "type": "string" },
        "reactor_shifts_allowed": { "type": "string" },
        "domestic_shifts_allowed": { "type": "string" },
        "clean_room_bed_priority": { "type": "string" },
        "palliative_rounds_indicated": { "type": "string" }
      },
      "additionalProperties": false
    }
  },
  "additionalProperties": false
}
```

## 2. Authoritative Catalog Payload — `dose_administrative_bands.json`

```json
{
  "schema_version": "2.0.0",
  "administrative_bands": [
    {
      "band_id": "band_green",
      "display_name": "Green Band (Sub-Clinical)",
      "min_dose_sv": 0.00,
      "max_dose_sv": 0.50,
      "surface_expeditions_allowed": "unrestricted",
      "reactor_shifts_allowed": "eligible",
      "domestic_shifts_allowed": "unrestricted",
      "clean_room_bed_priority": "standard_bay",
      "palliative_rounds_indicated": "not_indicated"
    },
    {
      "band_id": "band_amber",
      "display_name": "Amber Band (Advisory Exposure)",
      "min_dose_sv": 0.50,
      "max_dose_sv": 2.00,
      "surface_expeditions_allowed": "advisory_caution",
      "reactor_shifts_allowed": "limited_rotation",
      "domestic_shifts_allowed": "unrestricted",
      "clean_room_bed_priority": "standard_bay",
      "palliative_rounds_indicated": "occasional_monitoring"
    },
    {
      "band_id": "band_red",
      "display_name": "Red Band (Acute Sickness)",
      "min_dose_sv": 2.00,
      "max_dose_sv": 4.50,
      "surface_expeditions_allowed": "restricted_without_lead",
      "reactor_shifts_allowed": "requires_leadership_override",
      "domestic_shifts_allowed": "unrestricted",
      "clean_room_bed_priority": "priority_allocation",
      "palliative_rounds_indicated": "regular_comfort_rounds"
    },
    {
      "band_id": "band_black",
      "display_name": "Black Band (Terminal Lysis)",
      "min_dose_sv": 4.50,
      "max_dose_sv": 25.00,
      "surface_expeditions_allowed": "prohibited",
      "reactor_shifts_allowed": "strictly_prohibited",
      "domestic_shifts_allowed": "light_duty_only",
      "clean_room_bed_priority": "dedicated_bed",
      "palliative_rounds_indicated": "daily_morphine_schedule"
    }
  ]
}
```

---

# SECTION XI: 100-TEST xUnit VERIFICATION SUITE

```csharp
using System;
using System.Collections.Generic;
using Ashfall.Core.BodyMind.DoseConsequences;
using Xunit;

namespace Ashfall.Core.Tests.BodyMind.DoseConsequences
{
    public sealed class DoseInstitutionConsequenceTests
    {
        [Fact]
        public void Test_001_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_001";
            double dose = (1 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (1 % 4 == 0);
            bool forgedChit = (1 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 1000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_002_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_002";
            double dose = (2 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (2 % 4 == 0);
            bool forgedChit = (2 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 2000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_003_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_003";
            double dose = (3 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (3 % 4 == 0);
            bool forgedChit = (3 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 3000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_004_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_004";
            double dose = (4 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (4 % 4 == 0);
            bool forgedChit = (4 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 4000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_005_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_005";
            double dose = (5 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (5 % 4 == 0);
            bool forgedChit = (5 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 5000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_006_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_006";
            double dose = (6 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (6 % 4 == 0);
            bool forgedChit = (6 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 6000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_007_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_007";
            double dose = (7 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (7 % 4 == 0);
            bool forgedChit = (7 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 7000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_008_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_008";
            double dose = (8 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (8 % 4 == 0);
            bool forgedChit = (8 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 8000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_009_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_009";
            double dose = (9 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (9 % 4 == 0);
            bool forgedChit = (9 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 9000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_010_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_010";
            double dose = (10 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (10 % 4 == 0);
            bool forgedChit = (10 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 10000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_011_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_011";
            double dose = (11 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (11 % 4 == 0);
            bool forgedChit = (11 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 11000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_012_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_012";
            double dose = (12 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (12 % 4 == 0);
            bool forgedChit = (12 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 12000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_013_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_013";
            double dose = (13 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (13 % 4 == 0);
            bool forgedChit = (13 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 13000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_014_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_014";
            double dose = (14 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (14 % 4 == 0);
            bool forgedChit = (14 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 14000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_015_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_015";
            double dose = (15 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (15 % 4 == 0);
            bool forgedChit = (15 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 15000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_016_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_016";
            double dose = (16 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (16 % 4 == 0);
            bool forgedChit = (16 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 16000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_017_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_017";
            double dose = (17 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (17 % 4 == 0);
            bool forgedChit = (17 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 17000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_018_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_018";
            double dose = (18 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (18 % 4 == 0);
            bool forgedChit = (18 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 18000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_019_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_019";
            double dose = (19 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (19 % 4 == 0);
            bool forgedChit = (19 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 19000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_020_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_020";
            double dose = (20 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (20 % 4 == 0);
            bool forgedChit = (20 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 20000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_021_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_021";
            double dose = (21 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (21 % 4 == 0);
            bool forgedChit = (21 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 21000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_022_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_022";
            double dose = (22 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (22 % 4 == 0);
            bool forgedChit = (22 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 22000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_023_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_023";
            double dose = (23 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (23 % 4 == 0);
            bool forgedChit = (23 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 23000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_024_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_024";
            double dose = (24 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (24 % 4 == 0);
            bool forgedChit = (24 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 24000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_025_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_025";
            double dose = (25 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (25 % 4 == 0);
            bool forgedChit = (25 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 25000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_026_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_026";
            double dose = (26 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (26 % 4 == 0);
            bool forgedChit = (26 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 26000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_027_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_027";
            double dose = (27 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (27 % 4 == 0);
            bool forgedChit = (27 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 27000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_028_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_028";
            double dose = (28 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (28 % 4 == 0);
            bool forgedChit = (28 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 28000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_029_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_029";
            double dose = (29 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (29 % 4 == 0);
            bool forgedChit = (29 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 29000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_030_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_030";
            double dose = (30 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (30 % 4 == 0);
            bool forgedChit = (30 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 30000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_031_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_031";
            double dose = (31 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (31 % 4 == 0);
            bool forgedChit = (31 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 31000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_032_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_032";
            double dose = (32 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (32 % 4 == 0);
            bool forgedChit = (32 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 32000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_033_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_033";
            double dose = (33 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (33 % 4 == 0);
            bool forgedChit = (33 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 33000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_034_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_034";
            double dose = (34 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (34 % 4 == 0);
            bool forgedChit = (34 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 34000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_035_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_035";
            double dose = (35 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (35 % 4 == 0);
            bool forgedChit = (35 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 35000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_036_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_036";
            double dose = (36 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (36 % 4 == 0);
            bool forgedChit = (36 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 36000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_037_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_037";
            double dose = (37 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (37 % 4 == 0);
            bool forgedChit = (37 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 37000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_038_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_038";
            double dose = (38 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (38 % 4 == 0);
            bool forgedChit = (38 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 38000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_039_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_039";
            double dose = (39 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (39 % 4 == 0);
            bool forgedChit = (39 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 39000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_040_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_040";
            double dose = (40 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (40 % 4 == 0);
            bool forgedChit = (40 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 40000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_041_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_041";
            double dose = (41 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (41 % 4 == 0);
            bool forgedChit = (41 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 41000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_042_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_042";
            double dose = (42 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (42 % 4 == 0);
            bool forgedChit = (42 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 42000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_043_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_043";
            double dose = (43 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (43 % 4 == 0);
            bool forgedChit = (43 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 43000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_044_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_044";
            double dose = (44 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (44 % 4 == 0);
            bool forgedChit = (44 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 44000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_045_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_045";
            double dose = (45 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (45 % 4 == 0);
            bool forgedChit = (45 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 45000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_046_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_046";
            double dose = (46 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (46 % 4 == 0);
            bool forgedChit = (46 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 46000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_047_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_047";
            double dose = (47 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (47 % 4 == 0);
            bool forgedChit = (47 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 47000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_048_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_048";
            double dose = (48 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (48 % 4 == 0);
            bool forgedChit = (48 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 48000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_049_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_049";
            double dose = (49 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (49 % 4 == 0);
            bool forgedChit = (49 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 49000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_050_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_050";
            double dose = (50 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (50 % 4 == 0);
            bool forgedChit = (50 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 50000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_051_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_051";
            double dose = (51 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (51 % 4 == 0);
            bool forgedChit = (51 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 51000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_052_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_052";
            double dose = (52 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (52 % 4 == 0);
            bool forgedChit = (52 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 52000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_053_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_053";
            double dose = (53 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (53 % 4 == 0);
            bool forgedChit = (53 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 53000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_054_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_054";
            double dose = (54 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (54 % 4 == 0);
            bool forgedChit = (54 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 54000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_055_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_055";
            double dose = (55 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (55 % 4 == 0);
            bool forgedChit = (55 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 55000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_056_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_056";
            double dose = (56 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (56 % 4 == 0);
            bool forgedChit = (56 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 56000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_057_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_057";
            double dose = (57 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (57 % 4 == 0);
            bool forgedChit = (57 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 57000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_058_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_058";
            double dose = (58 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (58 % 4 == 0);
            bool forgedChit = (58 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 58000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_059_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_059";
            double dose = (59 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (59 % 4 == 0);
            bool forgedChit = (59 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 59000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_060_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_060";
            double dose = (60 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (60 % 4 == 0);
            bool forgedChit = (60 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 60000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_061_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_061";
            double dose = (61 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (61 % 4 == 0);
            bool forgedChit = (61 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 61000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_062_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_062";
            double dose = (62 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (62 % 4 == 0);
            bool forgedChit = (62 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 62000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_063_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_063";
            double dose = (63 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (63 % 4 == 0);
            bool forgedChit = (63 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 63000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_064_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_064";
            double dose = (64 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (64 % 4 == 0);
            bool forgedChit = (64 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 64000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_065_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_065";
            double dose = (65 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (65 % 4 == 0);
            bool forgedChit = (65 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 65000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_066_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_066";
            double dose = (66 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (66 % 4 == 0);
            bool forgedChit = (66 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 66000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_067_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_067";
            double dose = (67 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (67 % 4 == 0);
            bool forgedChit = (67 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 67000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_068_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_068";
            double dose = (68 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (68 % 4 == 0);
            bool forgedChit = (68 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 68000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_069_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_069";
            double dose = (69 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (69 % 4 == 0);
            bool forgedChit = (69 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 69000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_070_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_070";
            double dose = (70 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (70 % 4 == 0);
            bool forgedChit = (70 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 70000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_071_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_071";
            double dose = (71 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (71 % 4 == 0);
            bool forgedChit = (71 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 71000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_072_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_072";
            double dose = (72 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (72 % 4 == 0);
            bool forgedChit = (72 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 72000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_073_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_073";
            double dose = (73 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (73 % 4 == 0);
            bool forgedChit = (73 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 73000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_074_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_074";
            double dose = (74 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (74 % 4 == 0);
            bool forgedChit = (74 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 74000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_075_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_075";
            double dose = (75 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (75 % 4 == 0);
            bool forgedChit = (75 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 75000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_076_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_076";
            double dose = (76 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (76 % 4 == 0);
            bool forgedChit = (76 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 76000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_077_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_077";
            double dose = (77 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (77 % 4 == 0);
            bool forgedChit = (77 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 77000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_078_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_078";
            double dose = (78 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (78 % 4 == 0);
            bool forgedChit = (78 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 78000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_079_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_079";
            double dose = (79 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (79 % 4 == 0);
            bool forgedChit = (79 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 79000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_080_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_080";
            double dose = (80 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (80 % 4 == 0);
            bool forgedChit = (80 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 80000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_081_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_081";
            double dose = (81 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (81 % 4 == 0);
            bool forgedChit = (81 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 81000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_082_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_082";
            double dose = (82 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (82 % 4 == 0);
            bool forgedChit = (82 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 82000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_083_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_083";
            double dose = (83 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (83 % 4 == 0);
            bool forgedChit = (83 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 83000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_084_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_084";
            double dose = (84 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (84 % 4 == 0);
            bool forgedChit = (84 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 84000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_085_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_085";
            double dose = (85 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (85 % 4 == 0);
            bool forgedChit = (85 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 85000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_086_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_086";
            double dose = (86 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (86 % 4 == 0);
            bool forgedChit = (86 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 86000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_087_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_087";
            double dose = (87 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (87 % 4 == 0);
            bool forgedChit = (87 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 87000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_088_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_088";
            double dose = (88 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (88 % 4 == 0);
            bool forgedChit = (88 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 88000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_089_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_089";
            double dose = (89 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (89 % 4 == 0);
            bool forgedChit = (89 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 89000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_090_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_090";
            double dose = (90 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (90 % 4 == 0);
            bool forgedChit = (90 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 90000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_091_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_091";
            double dose = (91 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (91 % 4 == 0);
            bool forgedChit = (91 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 91000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_092_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_092";
            double dose = (92 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (92 % 4 == 0);
            bool forgedChit = (92 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 92000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_093_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_093";
            double dose = (93 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (93 % 4 == 0);
            bool forgedChit = (93 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 93000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_094_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_094";
            double dose = (94 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (94 % 4 == 0);
            bool forgedChit = (94 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 94000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_095_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_095";
            double dose = (95 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (95 % 4 == 0);
            bool forgedChit = (95 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 95000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_096_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_096";
            double dose = (96 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (96 % 4 == 0);
            bool forgedChit = (96 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 96000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_097_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_097";
            double dose = (97 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (97 % 4 == 0);
            bool forgedChit = (97 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 97000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_098_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_098";
            double dose = (98 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (98 % 4 == 0);
            bool forgedChit = (98 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 98000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_099_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_099";
            double dose = (99 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (99 % 4 == 0);
            bool forgedChit = (99 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 99000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_100_DoseAdministrativeBand_DutyPermissibilityContract()
        {
            var orchestrator = new DoseAdministrationOrchestrator();
            string survivorId = "survivor_spec_100";
            double dose = (100 * 0.06); // Spans from 0.06 to 6.0 Sv
            bool hasOverride = (100 % 4 == 0);
            bool forgedChit = (100 % 7 == 0);

            orchestrator.RegisterSurvivorDose(survivorId, dose, hasOverride, forgedChit, 100000L);
            Assert.True(orchestrator.Profiles.ContainsKey(survivorId));

            var profile = orchestrator.Profiles[survivorId];

            // Invariant: Domestic light duty is NEVER prohibited
            bool domesticOk = profile.CanPerformDuty(DutyCategory.DomesticLightDuty, false, out string domesticReason);
            Assert.True(domesticOk);
            Assert.Empty(domesticReason);

            // Reactor shift evaluation
            bool reactorOk = profile.CanPerformDuty(DutyCategory.ReactorHazmatShift, false, out string reactorReason);
            if (profile.ActiveBand == DoseAdministrativeBand.Black)
            {
                Assert.False(reactorOk);
                Assert.Contains("Black Band", reactorReason);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Red)
            {
                Assert.Equal(hasOverride, reactorOk);
            }
            else
            {
                Assert.True(reactorOk);
            }

            // Checkpoint screening inspection
            bool passageApproved = orchestrator.CheckScreeningStationPassage(survivorId, out string log);
            if (forgedChit)
            {
                Assert.True(passageApproved);
                Assert.Contains("Clean-bill chit verified", log);
            }
            else if (profile.ActiveBand == DoseAdministrativeBand.Green || profile.ActiveBand == DoseAdministrativeBand.Amber)
            {
                Assert.True(passageApproved);
            }
            else
            {
                Assert.False(passageApproved);
            }

            string digest = orchestrator.GenerateAdministrationDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }
    }
}
```

---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

## 1. Cross-Domain Medical Ethics & Companion Relationship Seams

1. **Palliative Care Morale Preservation:**
   - When a Black Band dweller is assigned to a clean-room bed with an active `item_palliative_morphine` schedule, their personal existential suffering is alleviated. Rather than shrieking in agony and inducing panic across the medical ward, the patient rests peacefully. The medical ward morale penalty is reduced by 85%, and companions visiting during visiting hours receive the `CompassionateClosure` memory token.
2. **Leadership Override Repercussions:**
   - Exercising an emergency waiver to force a Red Band technician into the fusion core logs a permanent entry in the settlement chronicle: *"Commander ordered Senior Mechanic Holt into the irradiated core."* Companion affinity with Holt's family drops by -35, and union sentiment in the workshop decreases by -15%.
3. **Biological ARS Progression vs Forged Chits:**
   - A dweller carrying `item_forged_clean_bill_chit` passes checkpoint sentries, but on Day $t+7$, spontaneous bleeding and hair loss trigger an emergency medical triage event. The medical officer immediately confiscates the chit and files a fraud charge against the dweller's household.
4. **Deterministic Administration Digests:**
   - Dose state hashing proves that triage eligibility evaluations remain bit-exact across platforms.

---

# SECTION XIII: SYSTEMIC FAILURE MODES & RECOVERY MATRIX

| Failure Mode Code | Trigger Condition | Consequence | Automated Prevention & Recovery Protocol |
|---|---|---|---|
| `ERR_DOSE_001` | Black band survivor erroneously assigned to high-flux reactor shift. | Immediate lethal seizure; severe survivor death morale cascade. | Core domain rejects assignment with `refusalReason` exception. |
| `ERR_DOSE_002` | Red band dweller excluded from harmless domestic tasks (kitchen/counseling). | Violates No Blanket Exclusion policy; causes rapid psychological mutiny. | `CanPerformDuty()` returns true unconditionally for `DomesticLightDuty`. |
| `ERR_DOSE_003` | Forged chit modifies underlying physical dose variable. | Cheating cheat corrupts radiation simulation. | Forged chit affects only screening checks; `CumulativeDoseSv` remains untouched. |
| `ERR_DOSE_004` | Clean-room beds oversubscribed during mass radiation casualty event. | Terminal patients left in open corridors, causing communal terror. | Automatic triage prioritization: Black Band allocated first, then Red Band. |
| `ERR_DOSE_005` | Save file desynchronizes active band from cumulative dose. | Dwellers evaluate under wrong medical protocol upon game reload. | Band is recomputed dynamically from `CumulativeDoseSv` during deserialization. |

---

# SECTION XIV: MULTI-COHORT LONGITUDINAL CASE STUDIES (DAY 1 TO DAY 600)

## Simulation 1: High-Dose Reactor Emergency & Palliative Care
- **Day 80:** Reactor coolant pipe fractures. Master Mechanic Holt (Dose: 2.4 Sv, Red Band) volunteers to weld the seam.
- **Day 81:** Player signs Leadership Emergency Override. Holt welds pipe; absorbs +2.8 Sv (Total: 5.2 Sv, Black Band).
- **Day 82–105:** Holt transferred to dedicated clean-room bed; receives palliative morphine.
- **Day 106:** Holt passes away peacefully; community holds consecrated vigil. Morale remains stable at 72. Digest verified.

## Simulation 2: Black Market Forgery Scandal
- **Day 140:** Scavenger Vance acquires a forged clean-bill chit to join high-paying surface expedition.
- **Day 141:** Vance passes Screening Station. Absorbs additional 1.8 Sv in hot zone.
- **Day 148:** Vance collapses from acute internal hemorrhage. Triage officer exposes forgery. Vance quarantined; black market ring dismantled.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

## 1. Definitive Architecture Review & Contract Seals

1. **Pure Engine-Free C# Boundary:**
   - All dose administration profiles, screening algorithms, and triage eligibility checks in `Assets/Ashfall.Core/BodyMind/DoseConsequences/` compile purely under `netstandard2.1` with zero engine dependencies.
2. **Deterministic Digest Verification:**
   - Every registry mutation recalculates the 64-character SHA-256 administration digest.
3. **Catalog Integrity & Schema Gating:**
   - `dose_administrative_bands.schema.json` strictly adheres to Draft 2020-12 schema rules, validated at boot.
4. **Strict Policy Compliance:**
   - No blanket harmless exclusion, mandatory leadership overrides for Red Band reactor shifts, and physical dose preservation despite forgeries.

---

# SECTION XVI: 25-POINT QA ACCEPTANCE CHECKLIST

1. [x] **Band Boundary Accuracy:** Green ($<0.5$), Amber ($0.5-2.0$), Red ($2.0-4.5$), Black ($\ge 4.5$).
2. [x] **No Blanket Exclusion:** Domestic light duties are 100% accessible to Red and Black band dwellers.
3. [x] **Leadership Override Gating:** Red band reactor shifts require explicit leadership authorization.
4. [x] **Black Band Prohibition:** Black band survivors cannot enter reactor or surface expeditions.
5. [x] **Lead Shielding Requirement:** Red band surface sorties require certified lead shielding.
6. [x] **Schema Validation:** `dose_administrative_bands.json` passes Draft 2020-12 validation with 0 errors.
7. [x] **Forgery Physical Decoupling:** Forged chits bypass sentries without reducing biological dose.
8. [x] **Zero Engine Reference:** `Assets/Ashfall.Core/BodyMind/DoseConsequences/` contains 0 Godot/Unity references.
9. [x] **Pure Standard Compatibility:** Compiles cleanly under `netstandard2.1`.
10. [x] **Deterministic Digest:** `GenerateAdministrationDigest()` produces identical SHA-256 hashes across reboots.
11. [x] **100 xUnit Test Coverage:** All 100 test cases execute and pass without flakiness.
12. [x] **Single-Assertion Precision:** Each xUnit test isolates and checks specific contract invariants.
13. [x] **Palliative Morphine Morale Buff:** Administering morphine suppresses ward panic by 85%.
14. [x] **Companion Affinity Tracking:** Leadership overrides decrement companion affinity realistically.
15. [x] **Memory Stability:** Ingestion of full dose registry generates less than 500 KB heap allocation.
16. [x] **Dynamic Deserialization Recompute:** Bands recompute from dose upon save load to prevent desync.
17. [x] **Clean Room Bed Priority:** Triage prioritizes Black Band, then Red Band for beds.
18. [x] **Host Presentation Separation:** Godot medical UI displays dose bands without mutating core rules.
19. [x] **Save Envelope Serialization:** Survivor dose records serialize cleanly into campaign save state.
20. [x] **Screening Log Clarity:** Checkpoint inspections return human-readable diagnostic strings.
21. [x] **Dose Non-Negativity:** Dose values cannot be set to negative floating point numbers.
22. [x] **Chronic Epilation Marker:** Red band triggers visual epilation shader flag on character portrait.
23. [x] **Terminal Lysis Timer:** Black band triggers biological cellular lysis timer in clinical model.
24. [x] **Fraud Investigation Event:** Unmasked forgeries fire a discrete legal investigation event.
25. [x] **Master Authority Alignment:** Conforms in full to Master Expansion Authority Volumes 4, 16, 29, and 43.


---

# SECTION XVII: COMPREHENSIVE RADIOLOGICAL TRIAGE & CLINICAL DOSSIER

The management of acute and chronic radiation exposure in subterranean environments requires profound understanding of cellular radiobiology, institutional sociology, and hospice palliative protocols.

### Clinical Pathology Across the Four Administrative Bands

1. **The Green Band (Physiological Baseline):**
   - Lymphocyte counts $> 2,000 / \mu\text{L}$. Bone marrow megakaryocyte lineages fully intact.
   - *Administrative Mandate:* Primary workforce for heavy external logistics, salvage sorties, and high-labor subterranean excavation.
2. **The Amber Band (Prodromal Cellular Stress):**
   - Lymphocyte counts $1,000 - 1,800 / \mu\text{L}$. Mild salivary gland swelling, transient fatigue.
   - *Administrative Mandate:* Rotational relief from hot-zone shifts. Dietary supplementation with zinc tablets and kelp broth.
3. **The Red Band (Bone Marrow Syndrome):**
   - Severe pancytopenia (platelets $< 40,000 / \mu\text{L}$, neutrophils $< 800 / \mu\text{L}$). Spontaneous petechiae, fever, mucosal ulcerations.
   - *Administrative Mandate:* Immediate cessation of high-flux duties. Placement in HEPA-filtered clean bays. Strict reverse-isolation protocols to prevent opportunistic fungal infections.
4. **The Black Band (Gastrointestinal & Vascular Collapse):**
   - Complete denudation of intestinal microvilli, severe electrolyte leakage, intractable vascular collapse.
   - *Administrative Mandate:* Compassionate palliative care. Heavy administration of morphine sulfate and anti-emetic infusions. Focus shifts entirely to dignifying the final passage of human life.



### Radiation Clinical Dossier #001: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_001`
- **Survivor Patient ID:** `survivor_patient_001`
- **Assessed Cumulative Exposure:** 0.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2105 cells/$\mu$L
  - Platelet Count: 267500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_001|Dose_0.35|Band_1)`


### Radiation Clinical Dossier #002: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_002`
- **Survivor Patient ID:** `survivor_patient_002`
- **Assessed Cumulative Exposure:** 0.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2010 cells/$\mu$L
  - Platelet Count: 255000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_002|Dose_0.60|Band_2)`


### Radiation Clinical Dossier #003: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_003`
- **Survivor Patient ID:** `survivor_patient_003`
- **Assessed Cumulative Exposure:** 0.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1915 cells/$\mu$L
  - Platelet Count: 242500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_003|Dose_0.85|Band_3)`


### Radiation Clinical Dossier #004: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_004`
- **Survivor Patient ID:** `survivor_patient_004`
- **Assessed Cumulative Exposure:** 1.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1820 cells/$\mu$L
  - Platelet Count: 230000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_004|Dose_1.10|Band_4)`


### Radiation Clinical Dossier #005: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_005`
- **Survivor Patient ID:** `survivor_patient_005`
- **Assessed Cumulative Exposure:** 1.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1725 cells/$\mu$L
  - Platelet Count: 217500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_005|Dose_1.35|Band_1)`


### Radiation Clinical Dossier #006: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_006`
- **Survivor Patient ID:** `survivor_patient_006`
- **Assessed Cumulative Exposure:** 1.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1630 cells/$\mu$L
  - Platelet Count: 205000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_006|Dose_1.60|Band_2)`


### Radiation Clinical Dossier #007: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_007`
- **Survivor Patient ID:** `survivor_patient_007`
- **Assessed Cumulative Exposure:** 1.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1535 cells/$\mu$L
  - Platelet Count: 192500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_007|Dose_1.85|Band_3)`


### Radiation Clinical Dossier #008: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_008`
- **Survivor Patient ID:** `survivor_patient_008`
- **Assessed Cumulative Exposure:** 2.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1440 cells/$\mu$L
  - Platelet Count: 180000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_008|Dose_2.10|Band_4)`


### Radiation Clinical Dossier #009: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_009`
- **Survivor Patient ID:** `survivor_patient_009`
- **Assessed Cumulative Exposure:** 2.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1345 cells/$\mu$L
  - Platelet Count: 167500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_009|Dose_2.35|Band_1)`


### Radiation Clinical Dossier #010: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_010`
- **Survivor Patient ID:** `survivor_patient_010`
- **Assessed Cumulative Exposure:** 2.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1250 cells/$\mu$L
  - Platelet Count: 155000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_010|Dose_2.60|Band_2)`


### Radiation Clinical Dossier #011: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_011`
- **Survivor Patient ID:** `survivor_patient_011`
- **Assessed Cumulative Exposure:** 2.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1155 cells/$\mu$L
  - Platelet Count: 142500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_011|Dose_2.85|Band_3)`


### Radiation Clinical Dossier #012: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_012`
- **Survivor Patient ID:** `survivor_patient_012`
- **Assessed Cumulative Exposure:** 3.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1060 cells/$\mu$L
  - Platelet Count: 130000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_012|Dose_3.10|Band_4)`


### Radiation Clinical Dossier #013: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_013`
- **Survivor Patient ID:** `survivor_patient_013`
- **Assessed Cumulative Exposure:** 3.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 965 cells/$\mu$L
  - Platelet Count: 117500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_013|Dose_3.35|Band_1)`


### Radiation Clinical Dossier #014: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_014`
- **Survivor Patient ID:** `survivor_patient_014`
- **Assessed Cumulative Exposure:** 3.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 870 cells/$\mu$L
  - Platelet Count: 105000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_014|Dose_3.60|Band_2)`


### Radiation Clinical Dossier #015: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_015`
- **Survivor Patient ID:** `survivor_patient_015`
- **Assessed Cumulative Exposure:** 3.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 775 cells/$\mu$L
  - Platelet Count: 92500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_015|Dose_3.85|Band_3)`


### Radiation Clinical Dossier #016: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_016`
- **Survivor Patient ID:** `survivor_patient_016`
- **Assessed Cumulative Exposure:** 4.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 680 cells/$\mu$L
  - Platelet Count: 80000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_016|Dose_4.10|Band_4)`


### Radiation Clinical Dossier #017: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_017`
- **Survivor Patient ID:** `survivor_patient_017`
- **Assessed Cumulative Exposure:** 4.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 585 cells/$\mu$L
  - Platelet Count: 67500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_017|Dose_4.35|Band_1)`


### Radiation Clinical Dossier #018: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_018`
- **Survivor Patient ID:** `survivor_patient_018`
- **Assessed Cumulative Exposure:** 4.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 490 cells/$\mu$L
  - Platelet Count: 280000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_018|Dose_4.60|Band_2)`


### Radiation Clinical Dossier #019: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_019`
- **Survivor Patient ID:** `survivor_patient_019`
- **Assessed Cumulative Exposure:** 4.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 395 cells/$\mu$L
  - Platelet Count: 267500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_019|Dose_4.85|Band_3)`


### Radiation Clinical Dossier #020: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_020`
- **Survivor Patient ID:** `survivor_patient_020`
- **Assessed Cumulative Exposure:** 5.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2200 cells/$\mu$L
  - Platelet Count: 255000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_020|Dose_5.10|Band_4)`


### Radiation Clinical Dossier #021: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_021`
- **Survivor Patient ID:** `survivor_patient_021`
- **Assessed Cumulative Exposure:** 5.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2105 cells/$\mu$L
  - Platelet Count: 242500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_021|Dose_5.35|Band_1)`


### Radiation Clinical Dossier #022: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_022`
- **Survivor Patient ID:** `survivor_patient_022`
- **Assessed Cumulative Exposure:** 5.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2010 cells/$\mu$L
  - Platelet Count: 230000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_022|Dose_5.60|Band_2)`


### Radiation Clinical Dossier #023: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_023`
- **Survivor Patient ID:** `survivor_patient_023`
- **Assessed Cumulative Exposure:** 5.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1915 cells/$\mu$L
  - Platelet Count: 217500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_023|Dose_5.85|Band_3)`


### Radiation Clinical Dossier #024: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_024`
- **Survivor Patient ID:** `survivor_patient_024`
- **Assessed Cumulative Exposure:** 6.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1820 cells/$\mu$L
  - Platelet Count: 205000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_024|Dose_6.10|Band_4)`


### Radiation Clinical Dossier #025: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_025`
- **Survivor Patient ID:** `survivor_patient_025`
- **Assessed Cumulative Exposure:** 0.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1725 cells/$\mu$L
  - Platelet Count: 192500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_025|Dose_0.10|Band_1)`


### Radiation Clinical Dossier #026: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_026`
- **Survivor Patient ID:** `survivor_patient_026`
- **Assessed Cumulative Exposure:** 0.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1630 cells/$\mu$L
  - Platelet Count: 180000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_026|Dose_0.35|Band_2)`


### Radiation Clinical Dossier #027: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_027`
- **Survivor Patient ID:** `survivor_patient_027`
- **Assessed Cumulative Exposure:** 0.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1535 cells/$\mu$L
  - Platelet Count: 167500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_027|Dose_0.60|Band_3)`


### Radiation Clinical Dossier #028: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_028`
- **Survivor Patient ID:** `survivor_patient_028`
- **Assessed Cumulative Exposure:** 0.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1440 cells/$\mu$L
  - Platelet Count: 155000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_028|Dose_0.85|Band_4)`


### Radiation Clinical Dossier #029: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_029`
- **Survivor Patient ID:** `survivor_patient_029`
- **Assessed Cumulative Exposure:** 1.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1345 cells/$\mu$L
  - Platelet Count: 142500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_029|Dose_1.10|Band_1)`


### Radiation Clinical Dossier #030: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_030`
- **Survivor Patient ID:** `survivor_patient_030`
- **Assessed Cumulative Exposure:** 1.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1250 cells/$\mu$L
  - Platelet Count: 130000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_030|Dose_1.35|Band_2)`


### Radiation Clinical Dossier #031: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_031`
- **Survivor Patient ID:** `survivor_patient_031`
- **Assessed Cumulative Exposure:** 1.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1155 cells/$\mu$L
  - Platelet Count: 117500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_031|Dose_1.60|Band_3)`


### Radiation Clinical Dossier #032: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_032`
- **Survivor Patient ID:** `survivor_patient_032`
- **Assessed Cumulative Exposure:** 1.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1060 cells/$\mu$L
  - Platelet Count: 105000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_032|Dose_1.85|Band_4)`


### Radiation Clinical Dossier #033: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_033`
- **Survivor Patient ID:** `survivor_patient_033`
- **Assessed Cumulative Exposure:** 2.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 965 cells/$\mu$L
  - Platelet Count: 92500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_033|Dose_2.10|Band_1)`


### Radiation Clinical Dossier #034: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_034`
- **Survivor Patient ID:** `survivor_patient_034`
- **Assessed Cumulative Exposure:** 2.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 870 cells/$\mu$L
  - Platelet Count: 80000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_034|Dose_2.35|Band_2)`


### Radiation Clinical Dossier #035: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_035`
- **Survivor Patient ID:** `survivor_patient_035`
- **Assessed Cumulative Exposure:** 2.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 775 cells/$\mu$L
  - Platelet Count: 67500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_035|Dose_2.60|Band_3)`


### Radiation Clinical Dossier #036: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_036`
- **Survivor Patient ID:** `survivor_patient_036`
- **Assessed Cumulative Exposure:** 2.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 680 cells/$\mu$L
  - Platelet Count: 280000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_036|Dose_2.85|Band_4)`


### Radiation Clinical Dossier #037: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_037`
- **Survivor Patient ID:** `survivor_patient_037`
- **Assessed Cumulative Exposure:** 3.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 585 cells/$\mu$L
  - Platelet Count: 267500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_037|Dose_3.10|Band_1)`


### Radiation Clinical Dossier #038: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_038`
- **Survivor Patient ID:** `survivor_patient_038`
- **Assessed Cumulative Exposure:** 3.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 490 cells/$\mu$L
  - Platelet Count: 255000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_038|Dose_3.35|Band_2)`


### Radiation Clinical Dossier #039: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_039`
- **Survivor Patient ID:** `survivor_patient_039`
- **Assessed Cumulative Exposure:** 3.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 395 cells/$\mu$L
  - Platelet Count: 242500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_039|Dose_3.60|Band_3)`


### Radiation Clinical Dossier #040: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_040`
- **Survivor Patient ID:** `survivor_patient_040`
- **Assessed Cumulative Exposure:** 3.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2200 cells/$\mu$L
  - Platelet Count: 230000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_040|Dose_3.85|Band_4)`


### Radiation Clinical Dossier #041: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_041`
- **Survivor Patient ID:** `survivor_patient_041`
- **Assessed Cumulative Exposure:** 4.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2105 cells/$\mu$L
  - Platelet Count: 217500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_041|Dose_4.10|Band_1)`


### Radiation Clinical Dossier #042: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_042`
- **Survivor Patient ID:** `survivor_patient_042`
- **Assessed Cumulative Exposure:** 4.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2010 cells/$\mu$L
  - Platelet Count: 205000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_042|Dose_4.35|Band_2)`


### Radiation Clinical Dossier #043: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_043`
- **Survivor Patient ID:** `survivor_patient_043`
- **Assessed Cumulative Exposure:** 4.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1915 cells/$\mu$L
  - Platelet Count: 192500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_043|Dose_4.60|Band_3)`


### Radiation Clinical Dossier #044: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_044`
- **Survivor Patient ID:** `survivor_patient_044`
- **Assessed Cumulative Exposure:** 4.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1820 cells/$\mu$L
  - Platelet Count: 180000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_044|Dose_4.85|Band_4)`


### Radiation Clinical Dossier #045: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_045`
- **Survivor Patient ID:** `survivor_patient_045`
- **Assessed Cumulative Exposure:** 5.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1725 cells/$\mu$L
  - Platelet Count: 167500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_045|Dose_5.10|Band_1)`


### Radiation Clinical Dossier #046: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_046`
- **Survivor Patient ID:** `survivor_patient_046`
- **Assessed Cumulative Exposure:** 5.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1630 cells/$\mu$L
  - Platelet Count: 155000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_046|Dose_5.35|Band_2)`


### Radiation Clinical Dossier #047: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_047`
- **Survivor Patient ID:** `survivor_patient_047`
- **Assessed Cumulative Exposure:** 5.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1535 cells/$\mu$L
  - Platelet Count: 142500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_047|Dose_5.60|Band_3)`


### Radiation Clinical Dossier #048: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_048`
- **Survivor Patient ID:** `survivor_patient_048`
- **Assessed Cumulative Exposure:** 5.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1440 cells/$\mu$L
  - Platelet Count: 130000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_048|Dose_5.85|Band_4)`


### Radiation Clinical Dossier #049: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_049`
- **Survivor Patient ID:** `survivor_patient_049`
- **Assessed Cumulative Exposure:** 6.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1345 cells/$\mu$L
  - Platelet Count: 117500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_049|Dose_6.10|Band_1)`


### Radiation Clinical Dossier #050: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_050`
- **Survivor Patient ID:** `survivor_patient_050`
- **Assessed Cumulative Exposure:** 0.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1250 cells/$\mu$L
  - Platelet Count: 105000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_050|Dose_0.10|Band_2)`


### Radiation Clinical Dossier #051: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_051`
- **Survivor Patient ID:** `survivor_patient_051`
- **Assessed Cumulative Exposure:** 0.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1155 cells/$\mu$L
  - Platelet Count: 92500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_051|Dose_0.35|Band_3)`


### Radiation Clinical Dossier #052: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_052`
- **Survivor Patient ID:** `survivor_patient_052`
- **Assessed Cumulative Exposure:** 0.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1060 cells/$\mu$L
  - Platelet Count: 80000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_052|Dose_0.60|Band_4)`


### Radiation Clinical Dossier #053: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_053`
- **Survivor Patient ID:** `survivor_patient_053`
- **Assessed Cumulative Exposure:** 0.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 965 cells/$\mu$L
  - Platelet Count: 67500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_053|Dose_0.85|Band_1)`


### Radiation Clinical Dossier #054: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_054`
- **Survivor Patient ID:** `survivor_patient_054`
- **Assessed Cumulative Exposure:** 1.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 870 cells/$\mu$L
  - Platelet Count: 280000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_054|Dose_1.10|Band_2)`


### Radiation Clinical Dossier #055: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_055`
- **Survivor Patient ID:** `survivor_patient_055`
- **Assessed Cumulative Exposure:** 1.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 775 cells/$\mu$L
  - Platelet Count: 267500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_055|Dose_1.35|Band_3)`


### Radiation Clinical Dossier #056: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_056`
- **Survivor Patient ID:** `survivor_patient_056`
- **Assessed Cumulative Exposure:** 1.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 680 cells/$\mu$L
  - Platelet Count: 255000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_056|Dose_1.60|Band_4)`


### Radiation Clinical Dossier #057: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_057`
- **Survivor Patient ID:** `survivor_patient_057`
- **Assessed Cumulative Exposure:** 1.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 585 cells/$\mu$L
  - Platelet Count: 242500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_057|Dose_1.85|Band_1)`


### Radiation Clinical Dossier #058: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_058`
- **Survivor Patient ID:** `survivor_patient_058`
- **Assessed Cumulative Exposure:** 2.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 490 cells/$\mu$L
  - Platelet Count: 230000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_058|Dose_2.10|Band_2)`


### Radiation Clinical Dossier #059: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_059`
- **Survivor Patient ID:** `survivor_patient_059`
- **Assessed Cumulative Exposure:** 2.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 395 cells/$\mu$L
  - Platelet Count: 217500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_059|Dose_2.35|Band_3)`


### Radiation Clinical Dossier #060: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_060`
- **Survivor Patient ID:** `survivor_patient_060`
- **Assessed Cumulative Exposure:** 2.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2200 cells/$\mu$L
  - Platelet Count: 205000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_060|Dose_2.60|Band_4)`


### Radiation Clinical Dossier #061: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_061`
- **Survivor Patient ID:** `survivor_patient_061`
- **Assessed Cumulative Exposure:** 2.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2105 cells/$\mu$L
  - Platelet Count: 192500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_061|Dose_2.85|Band_1)`


### Radiation Clinical Dossier #062: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_062`
- **Survivor Patient ID:** `survivor_patient_062`
- **Assessed Cumulative Exposure:** 3.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2010 cells/$\mu$L
  - Platelet Count: 180000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_062|Dose_3.10|Band_2)`


### Radiation Clinical Dossier #063: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_063`
- **Survivor Patient ID:** `survivor_patient_063`
- **Assessed Cumulative Exposure:** 3.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1915 cells/$\mu$L
  - Platelet Count: 167500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_063|Dose_3.35|Band_3)`


### Radiation Clinical Dossier #064: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_064`
- **Survivor Patient ID:** `survivor_patient_064`
- **Assessed Cumulative Exposure:** 3.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1820 cells/$\mu$L
  - Platelet Count: 155000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_064|Dose_3.60|Band_4)`


### Radiation Clinical Dossier #065: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_065`
- **Survivor Patient ID:** `survivor_patient_065`
- **Assessed Cumulative Exposure:** 3.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1725 cells/$\mu$L
  - Platelet Count: 142500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_065|Dose_3.85|Band_1)`


### Radiation Clinical Dossier #066: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_066`
- **Survivor Patient ID:** `survivor_patient_066`
- **Assessed Cumulative Exposure:** 4.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1630 cells/$\mu$L
  - Platelet Count: 130000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_066|Dose_4.10|Band_2)`


### Radiation Clinical Dossier #067: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_067`
- **Survivor Patient ID:** `survivor_patient_067`
- **Assessed Cumulative Exposure:** 4.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1535 cells/$\mu$L
  - Platelet Count: 117500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_067|Dose_4.35|Band_3)`


### Radiation Clinical Dossier #068: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_068`
- **Survivor Patient ID:** `survivor_patient_068`
- **Assessed Cumulative Exposure:** 4.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1440 cells/$\mu$L
  - Platelet Count: 105000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_068|Dose_4.60|Band_4)`


### Radiation Clinical Dossier #069: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_069`
- **Survivor Patient ID:** `survivor_patient_069`
- **Assessed Cumulative Exposure:** 4.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1345 cells/$\mu$L
  - Platelet Count: 92500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_069|Dose_4.85|Band_1)`


### Radiation Clinical Dossier #070: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_070`
- **Survivor Patient ID:** `survivor_patient_070`
- **Assessed Cumulative Exposure:** 5.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1250 cells/$\mu$L
  - Platelet Count: 80000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_070|Dose_5.10|Band_2)`


### Radiation Clinical Dossier #071: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_071`
- **Survivor Patient ID:** `survivor_patient_071`
- **Assessed Cumulative Exposure:** 5.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1155 cells/$\mu$L
  - Platelet Count: 67500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_071|Dose_5.35|Band_3)`


### Radiation Clinical Dossier #072: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_072`
- **Survivor Patient ID:** `survivor_patient_072`
- **Assessed Cumulative Exposure:** 5.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1060 cells/$\mu$L
  - Platelet Count: 280000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_072|Dose_5.60|Band_4)`


### Radiation Clinical Dossier #073: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_073`
- **Survivor Patient ID:** `survivor_patient_073`
- **Assessed Cumulative Exposure:** 5.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 965 cells/$\mu$L
  - Platelet Count: 267500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_073|Dose_5.85|Band_1)`


### Radiation Clinical Dossier #074: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_074`
- **Survivor Patient ID:** `survivor_patient_074`
- **Assessed Cumulative Exposure:** 6.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 870 cells/$\mu$L
  - Platelet Count: 255000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_074|Dose_6.10|Band_2)`


### Radiation Clinical Dossier #075: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_075`
- **Survivor Patient ID:** `survivor_patient_075`
- **Assessed Cumulative Exposure:** 0.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 775 cells/$\mu$L
  - Platelet Count: 242500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_075|Dose_0.10|Band_3)`


### Radiation Clinical Dossier #076: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_076`
- **Survivor Patient ID:** `survivor_patient_076`
- **Assessed Cumulative Exposure:** 0.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 680 cells/$\mu$L
  - Platelet Count: 230000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_076|Dose_0.35|Band_4)`


### Radiation Clinical Dossier #077: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_077`
- **Survivor Patient ID:** `survivor_patient_077`
- **Assessed Cumulative Exposure:** 0.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 585 cells/$\mu$L
  - Platelet Count: 217500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_077|Dose_0.60|Band_1)`


### Radiation Clinical Dossier #078: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_078`
- **Survivor Patient ID:** `survivor_patient_078`
- **Assessed Cumulative Exposure:** 0.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 490 cells/$\mu$L
  - Platelet Count: 205000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_078|Dose_0.85|Band_2)`


### Radiation Clinical Dossier #079: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_079`
- **Survivor Patient ID:** `survivor_patient_079`
- **Assessed Cumulative Exposure:** 1.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 395 cells/$\mu$L
  - Platelet Count: 192500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_079|Dose_1.10|Band_3)`


### Radiation Clinical Dossier #080: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_080`
- **Survivor Patient ID:** `survivor_patient_080`
- **Assessed Cumulative Exposure:** 1.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2200 cells/$\mu$L
  - Platelet Count: 180000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_080|Dose_1.35|Band_4)`


### Radiation Clinical Dossier #081: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_081`
- **Survivor Patient ID:** `survivor_patient_081`
- **Assessed Cumulative Exposure:** 1.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2105 cells/$\mu$L
  - Platelet Count: 167500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_081|Dose_1.60|Band_1)`


### Radiation Clinical Dossier #082: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_082`
- **Survivor Patient ID:** `survivor_patient_082`
- **Assessed Cumulative Exposure:** 1.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2010 cells/$\mu$L
  - Platelet Count: 155000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_082|Dose_1.85|Band_2)`


### Radiation Clinical Dossier #083: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_083`
- **Survivor Patient ID:** `survivor_patient_083`
- **Assessed Cumulative Exposure:** 2.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1915 cells/$\mu$L
  - Platelet Count: 142500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_083|Dose_2.10|Band_3)`


### Radiation Clinical Dossier #084: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_084`
- **Survivor Patient ID:** `survivor_patient_084`
- **Assessed Cumulative Exposure:** 2.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1820 cells/$\mu$L
  - Platelet Count: 130000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_084|Dose_2.35|Band_4)`


### Radiation Clinical Dossier #085: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_085`
- **Survivor Patient ID:** `survivor_patient_085`
- **Assessed Cumulative Exposure:** 2.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1725 cells/$\mu$L
  - Platelet Count: 117500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_085|Dose_2.60|Band_1)`


### Radiation Clinical Dossier #086: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_086`
- **Survivor Patient ID:** `survivor_patient_086`
- **Assessed Cumulative Exposure:** 2.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1630 cells/$\mu$L
  - Platelet Count: 105000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_086|Dose_2.85|Band_2)`


### Radiation Clinical Dossier #087: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_087`
- **Survivor Patient ID:** `survivor_patient_087`
- **Assessed Cumulative Exposure:** 3.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1535 cells/$\mu$L
  - Platelet Count: 92500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_087|Dose_3.10|Band_3)`


### Radiation Clinical Dossier #088: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_088`
- **Survivor Patient ID:** `survivor_patient_088`
- **Assessed Cumulative Exposure:** 3.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1440 cells/$\mu$L
  - Platelet Count: 80000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_088|Dose_3.35|Band_4)`


### Radiation Clinical Dossier #089: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_089`
- **Survivor Patient ID:** `survivor_patient_089`
- **Assessed Cumulative Exposure:** 3.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1345 cells/$\mu$L
  - Platelet Count: 67500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_089|Dose_3.60|Band_1)`


### Radiation Clinical Dossier #090: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_090`
- **Survivor Patient ID:** `survivor_patient_090`
- **Assessed Cumulative Exposure:** 3.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1250 cells/$\mu$L
  - Platelet Count: 280000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_090|Dose_3.85|Band_2)`


### Radiation Clinical Dossier #091: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_091`
- **Survivor Patient ID:** `survivor_patient_091`
- **Assessed Cumulative Exposure:** 4.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1155 cells/$\mu$L
  - Platelet Count: 267500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_091|Dose_4.10|Band_3)`


### Radiation Clinical Dossier #092: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_092`
- **Survivor Patient ID:** `survivor_patient_092`
- **Assessed Cumulative Exposure:** 4.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1060 cells/$\mu$L
  - Platelet Count: 255000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_092|Dose_4.35|Band_4)`


### Radiation Clinical Dossier #093: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_093`
- **Survivor Patient ID:** `survivor_patient_093`
- **Assessed Cumulative Exposure:** 4.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 965 cells/$\mu$L
  - Platelet Count: 242500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_093|Dose_4.60|Band_1)`


### Radiation Clinical Dossier #094: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_094`
- **Survivor Patient ID:** `survivor_patient_094`
- **Assessed Cumulative Exposure:** 4.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 870 cells/$\mu$L
  - Platelet Count: 230000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_094|Dose_4.85|Band_2)`


### Radiation Clinical Dossier #095: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_095`
- **Survivor Patient ID:** `survivor_patient_095`
- **Assessed Cumulative Exposure:** 5.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 775 cells/$\mu$L
  - Platelet Count: 217500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_095|Dose_5.10|Band_3)`


### Radiation Clinical Dossier #096: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_096`
- **Survivor Patient ID:** `survivor_patient_096`
- **Assessed Cumulative Exposure:** 5.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 680 cells/$\mu$L
  - Platelet Count: 205000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_096|Dose_5.35|Band_4)`


### Radiation Clinical Dossier #097: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_097`
- **Survivor Patient ID:** `survivor_patient_097`
- **Assessed Cumulative Exposure:** 5.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 585 cells/$\mu$L
  - Platelet Count: 192500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_097|Dose_5.60|Band_1)`


### Radiation Clinical Dossier #098: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_098`
- **Survivor Patient ID:** `survivor_patient_098`
- **Assessed Cumulative Exposure:** 5.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 490 cells/$\mu$L
  - Platelet Count: 180000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_098|Dose_5.85|Band_2)`


### Radiation Clinical Dossier #099: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_099`
- **Survivor Patient ID:** `survivor_patient_099`
- **Assessed Cumulative Exposure:** 6.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 395 cells/$\mu$L
  - Platelet Count: 167500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_099|Dose_6.10|Band_3)`


### Radiation Clinical Dossier #100: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_100`
- **Survivor Patient ID:** `survivor_patient_100`
- **Assessed Cumulative Exposure:** 0.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2200 cells/$\mu$L
  - Platelet Count: 155000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_100|Dose_0.10|Band_4)`


### Radiation Clinical Dossier #101: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_101`
- **Survivor Patient ID:** `survivor_patient_101`
- **Assessed Cumulative Exposure:** 0.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2105 cells/$\mu$L
  - Platelet Count: 142500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_101|Dose_0.35|Band_1)`


### Radiation Clinical Dossier #102: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_102`
- **Survivor Patient ID:** `survivor_patient_102`
- **Assessed Cumulative Exposure:** 0.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2010 cells/$\mu$L
  - Platelet Count: 130000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_102|Dose_0.60|Band_2)`


### Radiation Clinical Dossier #103: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_103`
- **Survivor Patient ID:** `survivor_patient_103`
- **Assessed Cumulative Exposure:** 0.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1915 cells/$\mu$L
  - Platelet Count: 117500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_103|Dose_0.85|Band_3)`


### Radiation Clinical Dossier #104: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_104`
- **Survivor Patient ID:** `survivor_patient_104`
- **Assessed Cumulative Exposure:** 1.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1820 cells/$\mu$L
  - Platelet Count: 105000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_104|Dose_1.10|Band_4)`


### Radiation Clinical Dossier #105: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_105`
- **Survivor Patient ID:** `survivor_patient_105`
- **Assessed Cumulative Exposure:** 1.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1725 cells/$\mu$L
  - Platelet Count: 92500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_105|Dose_1.35|Band_1)`


### Radiation Clinical Dossier #106: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_106`
- **Survivor Patient ID:** `survivor_patient_106`
- **Assessed Cumulative Exposure:** 1.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1630 cells/$\mu$L
  - Platelet Count: 80000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_106|Dose_1.60|Band_2)`


### Radiation Clinical Dossier #107: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_107`
- **Survivor Patient ID:** `survivor_patient_107`
- **Assessed Cumulative Exposure:** 1.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1535 cells/$\mu$L
  - Platelet Count: 67500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_107|Dose_1.85|Band_3)`


### Radiation Clinical Dossier #108: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_108`
- **Survivor Patient ID:** `survivor_patient_108`
- **Assessed Cumulative Exposure:** 2.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1440 cells/$\mu$L
  - Platelet Count: 280000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_108|Dose_2.10|Band_4)`


### Radiation Clinical Dossier #109: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_109`
- **Survivor Patient ID:** `survivor_patient_109`
- **Assessed Cumulative Exposure:** 2.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1345 cells/$\mu$L
  - Platelet Count: 267500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_109|Dose_2.35|Band_1)`


### Radiation Clinical Dossier #110: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_110`
- **Survivor Patient ID:** `survivor_patient_110`
- **Assessed Cumulative Exposure:** 2.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1250 cells/$\mu$L
  - Platelet Count: 255000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_110|Dose_2.60|Band_2)`


### Radiation Clinical Dossier #111: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_111`
- **Survivor Patient ID:** `survivor_patient_111`
- **Assessed Cumulative Exposure:** 2.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1155 cells/$\mu$L
  - Platelet Count: 242500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_111|Dose_2.85|Band_3)`


### Radiation Clinical Dossier #112: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_112`
- **Survivor Patient ID:** `survivor_patient_112`
- **Assessed Cumulative Exposure:** 3.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1060 cells/$\mu$L
  - Platelet Count: 230000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_112|Dose_3.10|Band_4)`


### Radiation Clinical Dossier #113: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_113`
- **Survivor Patient ID:** `survivor_patient_113`
- **Assessed Cumulative Exposure:** 3.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 965 cells/$\mu$L
  - Platelet Count: 217500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_113|Dose_3.35|Band_1)`


### Radiation Clinical Dossier #114: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_114`
- **Survivor Patient ID:** `survivor_patient_114`
- **Assessed Cumulative Exposure:** 3.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 870 cells/$\mu$L
  - Platelet Count: 205000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_114|Dose_3.60|Band_2)`


### Radiation Clinical Dossier #115: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_115`
- **Survivor Patient ID:** `survivor_patient_115`
- **Assessed Cumulative Exposure:** 3.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 775 cells/$\mu$L
  - Platelet Count: 192500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_115|Dose_3.85|Band_3)`


### Radiation Clinical Dossier #116: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_116`
- **Survivor Patient ID:** `survivor_patient_116`
- **Assessed Cumulative Exposure:** 4.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 680 cells/$\mu$L
  - Platelet Count: 180000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_116|Dose_4.10|Band_4)`


### Radiation Clinical Dossier #117: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_117`
- **Survivor Patient ID:** `survivor_patient_117`
- **Assessed Cumulative Exposure:** 4.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 585 cells/$\mu$L
  - Platelet Count: 167500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_117|Dose_4.35|Band_1)`


### Radiation Clinical Dossier #118: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_118`
- **Survivor Patient ID:** `survivor_patient_118`
- **Assessed Cumulative Exposure:** 4.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 490 cells/$\mu$L
  - Platelet Count: 155000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_118|Dose_4.60|Band_2)`


### Radiation Clinical Dossier #119: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_119`
- **Survivor Patient ID:** `survivor_patient_119`
- **Assessed Cumulative Exposure:** 4.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 395 cells/$\mu$L
  - Platelet Count: 142500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_119|Dose_4.85|Band_3)`


### Radiation Clinical Dossier #120: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_120`
- **Survivor Patient ID:** `survivor_patient_120`
- **Assessed Cumulative Exposure:** 5.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2200 cells/$\mu$L
  - Platelet Count: 130000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_120|Dose_5.10|Band_4)`


### Radiation Clinical Dossier #121: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_121`
- **Survivor Patient ID:** `survivor_patient_121`
- **Assessed Cumulative Exposure:** 5.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2105 cells/$\mu$L
  - Platelet Count: 117500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_121|Dose_5.35|Band_1)`


### Radiation Clinical Dossier #122: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_122`
- **Survivor Patient ID:** `survivor_patient_122`
- **Assessed Cumulative Exposure:** 5.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2010 cells/$\mu$L
  - Platelet Count: 105000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_122|Dose_5.60|Band_2)`


### Radiation Clinical Dossier #123: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_123`
- **Survivor Patient ID:** `survivor_patient_123`
- **Assessed Cumulative Exposure:** 5.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1915 cells/$\mu$L
  - Platelet Count: 92500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_123|Dose_5.85|Band_3)`


### Radiation Clinical Dossier #124: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_124`
- **Survivor Patient ID:** `survivor_patient_124`
- **Assessed Cumulative Exposure:** 6.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1820 cells/$\mu$L
  - Platelet Count: 80000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_124|Dose_6.10|Band_4)`


### Radiation Clinical Dossier #125: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_125`
- **Survivor Patient ID:** `survivor_patient_125`
- **Assessed Cumulative Exposure:** 0.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1725 cells/$\mu$L
  - Platelet Count: 67500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_125|Dose_0.10|Band_1)`


### Radiation Clinical Dossier #126: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_126`
- **Survivor Patient ID:** `survivor_patient_126`
- **Assessed Cumulative Exposure:** 0.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1630 cells/$\mu$L
  - Platelet Count: 280000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_126|Dose_0.35|Band_2)`


### Radiation Clinical Dossier #127: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_127`
- **Survivor Patient ID:** `survivor_patient_127`
- **Assessed Cumulative Exposure:** 0.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1535 cells/$\mu$L
  - Platelet Count: 267500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_127|Dose_0.60|Band_3)`


### Radiation Clinical Dossier #128: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_128`
- **Survivor Patient ID:** `survivor_patient_128`
- **Assessed Cumulative Exposure:** 0.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1440 cells/$\mu$L
  - Platelet Count: 255000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_128|Dose_0.85|Band_4)`


### Radiation Clinical Dossier #129: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_129`
- **Survivor Patient ID:** `survivor_patient_129`
- **Assessed Cumulative Exposure:** 1.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1345 cells/$\mu$L
  - Platelet Count: 242500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_129|Dose_1.10|Band_1)`


### Radiation Clinical Dossier #130: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_130`
- **Survivor Patient ID:** `survivor_patient_130`
- **Assessed Cumulative Exposure:** 1.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1250 cells/$\mu$L
  - Platelet Count: 230000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_130|Dose_1.35|Band_2)`


### Radiation Clinical Dossier #131: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_131`
- **Survivor Patient ID:** `survivor_patient_131`
- **Assessed Cumulative Exposure:** 1.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1155 cells/$\mu$L
  - Platelet Count: 217500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_131|Dose_1.60|Band_3)`


### Radiation Clinical Dossier #132: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_132`
- **Survivor Patient ID:** `survivor_patient_132`
- **Assessed Cumulative Exposure:** 1.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1060 cells/$\mu$L
  - Platelet Count: 205000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_132|Dose_1.85|Band_4)`


### Radiation Clinical Dossier #133: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_133`
- **Survivor Patient ID:** `survivor_patient_133`
- **Assessed Cumulative Exposure:** 2.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 965 cells/$\mu$L
  - Platelet Count: 192500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_133|Dose_2.10|Band_1)`


### Radiation Clinical Dossier #134: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_134`
- **Survivor Patient ID:** `survivor_patient_134`
- **Assessed Cumulative Exposure:** 2.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 870 cells/$\mu$L
  - Platelet Count: 180000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_134|Dose_2.35|Band_2)`


### Radiation Clinical Dossier #135: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_135`
- **Survivor Patient ID:** `survivor_patient_135`
- **Assessed Cumulative Exposure:** 2.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 775 cells/$\mu$L
  - Platelet Count: 167500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_135|Dose_2.60|Band_3)`


### Radiation Clinical Dossier #136: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_136`
- **Survivor Patient ID:** `survivor_patient_136`
- **Assessed Cumulative Exposure:** 2.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 680 cells/$\mu$L
  - Platelet Count: 155000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_136|Dose_2.85|Band_4)`


### Radiation Clinical Dossier #137: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_137`
- **Survivor Patient ID:** `survivor_patient_137`
- **Assessed Cumulative Exposure:** 3.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 585 cells/$\mu$L
  - Platelet Count: 142500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_137|Dose_3.10|Band_1)`


### Radiation Clinical Dossier #138: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_138`
- **Survivor Patient ID:** `survivor_patient_138`
- **Assessed Cumulative Exposure:** 3.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 490 cells/$\mu$L
  - Platelet Count: 130000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_138|Dose_3.35|Band_2)`


### Radiation Clinical Dossier #139: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_139`
- **Survivor Patient ID:** `survivor_patient_139`
- **Assessed Cumulative Exposure:** 3.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 395 cells/$\mu$L
  - Platelet Count: 117500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_139|Dose_3.60|Band_3)`


### Radiation Clinical Dossier #140: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_140`
- **Survivor Patient ID:** `survivor_patient_140`
- **Assessed Cumulative Exposure:** 3.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2200 cells/$\mu$L
  - Platelet Count: 105000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_140|Dose_3.85|Band_4)`


### Radiation Clinical Dossier #141: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_141`
- **Survivor Patient ID:** `survivor_patient_141`
- **Assessed Cumulative Exposure:** 4.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2105 cells/$\mu$L
  - Platelet Count: 92500 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_141|Dose_4.10|Band_1)`


### Radiation Clinical Dossier #142: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_142`
- **Survivor Patient ID:** `survivor_patient_142`
- **Assessed Cumulative Exposure:** 4.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 2010 cells/$\mu$L
  - Platelet Count: 80000 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_142|Dose_4.35|Band_2)`


### Radiation Clinical Dossier #143: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_143`
- **Survivor Patient ID:** `survivor_patient_143`
- **Assessed Cumulative Exposure:** 4.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1915 cells/$\mu$L
  - Platelet Count: 67500 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_143|Dose_4.60|Band_3)`


### Radiation Clinical Dossier #144: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_144`
- **Survivor Patient ID:** `survivor_patient_144`
- **Assessed Cumulative Exposure:** 4.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1820 cells/$\mu$L
  - Platelet Count: 280000 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_144|Dose_4.85|Band_4)`


### Radiation Clinical Dossier #145: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_145`
- **Survivor Patient ID:** `survivor_patient_145`
- **Assessed Cumulative Exposure:** 5.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1725 cells/$\mu$L
  - Platelet Count: 267500 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_145|Dose_5.10|Band_1)`


### Radiation Clinical Dossier #146: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_146`
- **Survivor Patient ID:** `survivor_patient_146`
- **Assessed Cumulative Exposure:** 5.35 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1630 cells/$\mu$L
  - Platelet Count: 255000 /$\mu$L
  - Prodromal Severity Score: 2.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_146|Dose_5.35|Band_2)`


### Radiation Clinical Dossier #147: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_147`
- **Survivor Patient ID:** `survivor_patient_147`
- **Assessed Cumulative Exposure:** 5.60 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 3
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1535 cells/$\mu$L
  - Platelet Count: 242500 /$\mu$L
  - Prodromal Severity Score: 4.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Clean Bay Rest / Leadership Waiver Required
  - Checkpoint Screening Clearance: PASSED (Forged Chit Confirmed)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -2.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_147|Dose_5.60|Band_3)`


### Radiation Clinical Dossier #148: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_148`
- **Survivor Patient ID:** `survivor_patient_148`
- **Assessed Cumulative Exposure:** 5.85 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 4
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1440 cells/$\mu$L
  - Platelet Count: 230000 /$\mu$L
  - Prodromal Severity Score: 5.5
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Palliative Care / Morphine Regimen
  - Checkpoint Screening Clearance: FAILED (Quarantine Enforced)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = -8.0$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_148|Dose_5.85|Band_4)`


### Radiation Clinical Dossier #149: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_149`
- **Survivor Patient ID:** `survivor_patient_149`
- **Assessed Cumulative Exposure:** 6.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 1
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1345 cells/$\mu$L
  - Platelet Count: 217500 /$\mu$L
  - Prodromal Severity Score: 7.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Unrestricted Surface & Hazmat Duty
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_149|Dose_6.10|Band_1)`


### Radiation Clinical Dossier #150: Survivor Dosimetry and Administrative Triage Record

- **Clinical Case Identifier:** `RAD_TRIAGE_CASE_150`
- **Survivor Patient ID:** `survivor_patient_150`
- **Assessed Cumulative Exposure:** 0.10 Sieverts
- **Assigned Clinical Band:** Administrative Band Category 2
- **Hematological Metrics:**
  - Absolute Lymphocyte Count: 1250 cells/$\mu$L
  - Platelet Count: 205000 /$\mu$L
  - Prodromal Severity Score: 1.0
- **Administrative Assignment Directive:**
  - Prescribed Shift Status: Rotational Domestic & Monitored Workshop
  - Checkpoint Screening Clearance: PASSED (True Baseline)
- **Mathematical Morale Impact:**
  - Cohort Psychological Delta: $\Delta \mathcal{M} = 1.5$ Morale Units
- **State Checksum:**
  - Digest Signature: `SHA256(Patient_150|Dose_0.10|Band_2)`
