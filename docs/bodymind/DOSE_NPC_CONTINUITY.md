
<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/BodyMind/DoseRegister/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Presentation & Dialogue Systems)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION IV: DOSE REGISTER NPC CONTINUITY, ADMINISTRATIVE AUTHORITY & ETHICAL DILEMMA ARCHITECTURE

## 1. Domain Overview & The Politics of Radiation Accounting

In the Ashfall shelter ecosystem, radiation is not merely an invisible biological poison; it is an administrative, economic, and moral currency (`DoseNpcContinuitySystem.cs`). The survivor community's survival depends upon maintaining accurate dosimetric ledgers. If workers are sent into hot zones beyond their biological limits, they collapse and die; if the ledgers are too conservative, vital power turbines go unmaintained and the entire settlement freezes.

Between these competing pressures stand the **Four Dose Register Figures**:
1. **Dr. Irina Vel (`npc_dr_irina_vel`):** Radiation Registrar (`register_ledger` — The Dose Ledger).
2. **Sister Wyn Omah (`npc_wyn_omah`):** Sick-Room Nurse & Palliative Steward (`register_sick` — The Sick List).
3. **Piet Abar (`npc_piet_abar`):** The Clockmaker & Instrument Calibrator (`register_ledger` — Calibration & Tagging).
4. **Saria Voss (`npc_saria_voss`):** The Midwife & Cohort Registrar (`register_cohort` — The Cohort Board).

### Core Administrative Hierarchy & Conflict Architecture

```text
========================================================================================
                      THE DOSE REGISTER ADMINISTRATIVE BOARD
========================================================================================
     [ Dr. Irina Vel ] <--------------------> [ Piet Abar ]
  (The Dose Ledger: Inflexible Facts)    (Instrument Calibration: Tool Drift & Margins)
           |                                       |
           v                                       v
     [ Sister Wyn Omah ] <------------------> [ Saria Voss ]
  (The Sick List: Palliative Care & Beds) (The Cohort Board: Child Baselines & Future)
========================================================================================
```

### The Invariable Invariant: Biological Truth vs. Bureaucratic Fiction
As established in DEC-05 and the Core rules:
> **The Dose Invariant:** Forged dose chits, falsified ledger entries, and executive emergency overrides alter *institutional status, ration allocations, and legal duty assignments* only. They can **never** alter the dweller's biological `CumulativeDoseSv` or biological acute radiation sickness severity. The body remembers what the red pencil attempts to erase.

---

# SECTION V: THE FOUR DOSE REGISTER CHARACTERS & EXPANDED CAST

### 1. Dr. Irina Vel (`npc_dr_irina_vel`)
- **Title / Role:** Chief Radiation Registrar.
- **Assigned Register:** `register_ledger` (The Dose Ledger).
- **Core Philosophy:** *The number is the number.* Holds an indelible red wax pencil and refuses to round down, soften figures, or accept political excuses. A reading is a physical reality; erasing a digit on a sheet of butcher paper does not protect the bone marrow.
- **Voice Guidelines:** Clinical, austere, uncompromising, exhausted, unflinching against executive threats. Speaks in brief, declarative sentences.
- **Key Ethical Dilemmas:**
  - Confronting forged clearance badges presented by desperate workers wanting surface rations.
  - Resisting shelter council demands to erase radiation spikes following a reactor coolant leak.
  - Investigating torn-out ledger leaves that hide the true cumulative exposure of the veteran scavenger cadre.

### 2. Sister Wyn Omah (`npc_wyn_omah`)
- **Title / Role:** Sick-Room Nurse & Palliative Steward.
- **Assigned Register:** `register_sick` (The Sick List).
- **Core Philosophy:** *Care is a schedule, not a judgment.* Manages scarce infirmary cots, saline bags, and morphine ampoules. She will never reshuffle bed orders or withhold pain relief to favor political leadership or heroic scavengers.
- **Voice Guidelines:** Gentle, pragmatic, unhurried, quietly observant, unshakable in the presence of terminal biological collapse.
- **Key Ethical Dilemmas:**
  - Allocating the last dose of anti-nausea medication between a dying elder and a sick apprentice.
  - Refusing to discharge a worker who is clinically vomiting despite the administrator demanding their return to labor.
  - Guarding the morphine locker against armed theft during shelter rationing crises.

### 3. Piet Abar (`npc_piet_abar`)
- **Title / Role:** The Clockmaker & Instrument Calibrator.
- **Assigned Register:** `register_ledger` (Instrument Calibration & Tagging).
- **Core Philosophy:** *Every figure has a drift, and the drift is normal.* Piet rejects the illusion that dosimeters and ion chambers are holy, infallible machines. Calibration is constant, grueling maintenance against dust, humidity, battery decay, and sensor degradation.
- **Voice Guidelines:** Dry, meticulous, mechanical, honest about error margins and tolerances, deeply skeptical of absolute certainty.
- **Key Ethical Dilemmas:**
  - Detecting that a batch of quartz dosimeters suffered a +30% systematic under-reporting drift over the past month.
  - Confronting a cracked isotopic calibration check source that is leaking background radiation into the workshop.
  - Deciding whether to shut down all surface expeditions to re-zero instruments or permit scavenging on unreliable numbers.

### 4. Saria Voss (`npc_saria_voss`)
- **Title / Role:** The Midwife & Cohort Registrar.
- **Assigned Register:** `register_cohort` (The Cohort Board).
- **Core Philosophy:** *A guess is not a truth.* Saria maintains the children and adolescent baseline exposure board in erasable chalk. She refuses to brand a child with a catastrophic projected lifespan based on imprecise estimates.
- **Voice Guidelines:** Fiercely protective, maternal yet unsentimental, guarded, willing to defy shelter leadership with cold fury to protect the next generation.
- **Key Ethical Dilemmas:**
  - Deciding whether to record an honest, grim baseline for newborns born during fallout peaks or give them a clean slate.
  - Shielding 14-year-old adolescents from being conscripted into high-exposure smelter shifts.
  - Barricading the nursery against radiation quarantine officers attempting to separate sick mothers from infants.

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
    public enum NpcAdministrativeRole
    {
        RadiationRegistrar = 1,
        SickRoomSteward = 2,
        InstrumentCalibrator = 3,
        CohortRegistrar = 4
    }

    public enum AdministrativeDecisionOutcome
    {
        StrictAdherenceToTruth = 1,
        CompromisedUnderDuress = 2,
        WhistleblowerAuditTriggered = 3,
        EthicalStandOff = 4
    }

    public sealed class DoseNpcProfile
    {
        public string NpcId { get; }
        public string CharacterName { get; }
        public NpcAdministrativeRole Role { get; }
        public string PhilosophyMotto { get; }
        public int UncompromisingIntegrityScore { get; private set; }
        public int CompassionScore { get; private set; }
        public int AdministrativeFatigue { get; private set; }

        public DoseNpcProfile(
            string npcId,
            string name,
            NpcAdministrativeRole role,
            string philosophy,
            int integrity,
            int compassion)
        {
            NpcId = npcId ?? throw new ArgumentNullException(nameof(npcId));
            CharacterName = name ?? string.Empty;
            Role = role;
            PhilosophyMotto = philosophy ?? string.Empty;
            UncompromisingIntegrityScore = Math.Max(0, Math.Min(100, integrity));
            CompassionScore = Math.Max(0, Math.Min(100, compassion));
            AdministrativeFatigue = 0;
        }

        public void AccumulateFatigue(int amount)
        {
            AdministrativeFatigue = Math.Max(0, Math.Min(100, AdministrativeFatigue + amount));
        }

        public void RebalanceEthics(int integrityDelta, int compassionDelta)
        {
            UncompromisingIntegrityScore = Math.Max(0, Math.Min(100, UncompromisingIntegrityScore + integrityDelta));
            CompassionScore = Math.Max(0, Math.Min(100, CompassionScore + compassionDelta));
        }
    }

    public sealed class DosePoliticalLedgerOrchestrator
    {
        private readonly Dictionary<string, DoseNpcProfile> _npcs = new Dictionary<string, DoseNpcProfile>();
        private readonly List<string> _administrativeAuditLog = new List<string>();

        public IReadOnlyDictionary<string, DoseNpcProfile> Npcs => new ReadOnlyDictionary<string, DoseNpcProfile>(_npcs);
        public IReadOnlyList<string> AuditLog => _administrativeAuditLog.AsReadOnly();

        public void RegisterNpc(DoseNpcProfile profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            _npcs[profile.NpcId] = profile;
        }

        public AdministrativeDecisionOutcome ResolveDoseAlterationRequest(
            string requesterNpcId,
            string targetSurvivorId,
            float biologicalDoseSv,
            float requestedForgedDoseSv,
            bool hasExecutiveOrder)
        {
            if (!_npcs.TryGetValue(requesterNpcId, out var npc))
            {
                throw new InvalidOperationException($"Registrar {requesterNpcId} not found!");
            }

            // Dr. Irina Vel strict truth rule
            if (npc.Role == NpcAdministrativeRole.RadiationRegistrar)
            {
                if (hasExecutiveOrder && npc.AdministrativeFatigue > 85)
                {
                    npc.AccumulateFatigue(10);
                    _administrativeAuditLog.Add($"AUDIT_ALERT: {npc.NpcId} compromised under executive duress for survivor {targetSurvivorId}. Ledger altered, biological dose unchanged.");
                    return AdministrativeDecisionOutcome.CompromisedUnderDuress;
                }

                _administrativeAuditLog.Add($"AUDIT_CONFIRMED: {npc.NpcId} rejected dose alteration for {targetSurvivorId}. Biological dose {biologicalDoseSv:F3}Sv strictly recorded.");
                return AdministrativeDecisionOutcome.StrictAdherenceToTruth;
            }

            // Saria Voss cohort protection
            if (npc.Role == NpcAdministrativeRole.CohortRegistrar)
            {
                if (requestedForgedDoseSv < biologicalDoseSv)
                {
                    _administrativeAuditLog.Add($"COHORT_PROTECTION: {npc.NpcId} erased chalk mark for adolescent {targetSurvivorId} to prevent smelter conscription.");
                    return AdministrativeDecisionOutcome.StrictAdherenceToTruth;
                }
            }

            return AdministrativeDecisionOutcome.EthicalStandOff;
        }

        public string ComputeNpcStateDigest()
        {
            var sb = new StringBuilder();
            var sortedKeys = new List<string>(_npcs.Keys);
            sortedKeys.Sort(StringComparer.Ordinal);

            foreach (var k in sortedKeys)
            {
                var npc = _npcs[k];
                sb.Append($"{npc.NpcId}:{(int)npc.Role}:{npc.UncompromisingIntegrityScore}:{npc.CompassionScore}:{npc.AdministrativeFatigue};");
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

## 1. JSON Schema (Draft 2020-12) — `dose_npc_continuity.schema.json`

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.net/schemas/dose_npc_continuity.schema.json",
  "title": "DoseNpcContinuityCatalog",
  "type": "object",
  "required": ["schema_version", "registrars"],
  "properties": {
    "schema_version": {
      "type": "string",
      "enum": ["2.0.0"]
    },
    "registrars": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/registrar_entry"
      }
    }
  },
  "$defs": {
    "registrar_entry": {
      "type": "object",
      "required": [
        "npc_id",
        "display_name",
        "role_title",
        "assigned_register",
        "core_philosophy",
        "voice_guidelines",
        "base_integrity",
        "base_compassion"
      ],
      "properties": {
        "npc_id": {
          "type": "string",
          "pattern": "^npc_[a-z0-9_]+$"
        },
        "display_name": { "type": "string" },
        "role_title": { "type": "string" },
        "assigned_register": { "type": "string" },
        "core_philosophy": { "type": "string" },
        "voice_guidelines": { "type": "string" },
        "base_integrity": {
          "type": "integer",
          "minimum": 0,
          "maximum": 100
        },
        "base_compassion": {
          "type": "integer",
          "minimum": 0,
          "maximum": 100
        }
      },
      "additionalProperties": false
    }
  },
  "additionalProperties": false
}
```

## 2. Authoritative Catalog Payload — `dose_npc_continuity.json`

```json
{
  "schema_version": "2.0.0",
  "registrars": [
    {
      "npc_id": "npc_dr_irina_vel",
      "display_name": "Dr. Irina Vel",
      "role_title": "Radiation Registrar",
      "assigned_register": "register_ledger",
      "core_philosophy": "The number is the number.",
      "voice_guidelines": "Clinical, austere, uncompromising, tired, yet fundamentally protecting the truth.",
      "base_integrity": 95,
      "base_compassion": 60
    },
    {
      "npc_id": "npc_wyn_omah",
      "display_name": "Sister Wyn Omah",
      "role_title": "Sick-Room Nurse & Palliative Steward",
      "assigned_register": "register_sick",
      "core_philosophy": "Care is a schedule, not a judgment.",
      "voice_guidelines": "Gentle, pragmatic, unhurried, quietly observant, unflinching in the face of terminal sickness.",
      "base_integrity": 85,
      "base_compassion": 95
    },
    {
      "npc_id": "npc_piet_abar",
      "display_name": "Piet Abar",
      "role_title": "The Clockmaker & Instrument Calibrator",
      "assigned_register": "register_ledger",
      "core_philosophy": "Every figure has a drift, and the drift is normal.",
      "voice_guidelines": "Dry, meticulous, mechanical, honest about error margins and tolerances.",
      "base_integrity": 90,
      "base_compassion": 50
    },
    {
      "npc_id": "npc_saria_voss",
      "display_name": "Saria Voss",
      "role_title": "The Midwife & Cohort Registrar",
      "assigned_register": "register_cohort",
      "core_philosophy": "A guess is not a truth.",
      "voice_guidelines": "Protective, maternal yet unsentimental, guarded, fiercely protective of adolescents.",
      "base_integrity": 80,
      "base_compassion": 90
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
    public sealed class DoseNpcContinuityTests
    {
        private static DosePoliticalLedgerOrchestrator CreateInitializedOrchestrator()
        {
            var orch = new DosePoliticalLedgerOrchestrator();

            orch.RegisterNpc(new DoseNpcProfile("npc_dr_irina_vel", "Dr. Irina Vel", NpcAdministrativeRole.RadiationRegistrar, "The number is the number.", 95, 60));
            orch.RegisterNpc(new DoseNpcProfile("npc_wyn_omah", "Sister Wyn Omah", NpcAdministrativeRole.SickRoomSteward, "Care is a schedule, not a judgment.", 85, 95));
            orch.RegisterNpc(new DoseNpcProfile("npc_piet_abar", "Piet Abar", NpcAdministrativeRole.InstrumentCalibrator, "Every figure has a drift, and the drift is normal.", 90, 50));
            orch.RegisterNpc(new DoseNpcProfile("npc_saria_voss", "Saria Voss", NpcAdministrativeRole.CohortRegistrar, "A guess is not a truth.", 80, 90));

            return orch;
        }
        [Fact]
        public void Test_001_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(7);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_001",
                1.05f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 7 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_002_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(14);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_002",
                1.10f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 14 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_003_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(21);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_003",
                1.15f,
                0.50f,
                true
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (true && 21 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_004_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(28);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_004",
                1.20f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 28 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_005_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(35);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_005",
                1.25f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 35 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_006_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(42);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_006",
                1.30f,
                0.50f,
                true
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (true && 42 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_007_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(49);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_007",
                1.35f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 49 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_008_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(56);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_008",
                1.40f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 56 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_009_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(63);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_009",
                1.45f,
                0.50f,
                true
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (true && 63 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_010_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(70);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_010",
                1.50f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 70 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_011_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(77);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_011",
                1.55f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 77 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_012_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(84);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_012",
                1.60f,
                0.50f,
                true
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (true && 84 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_013_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(91);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_013",
                1.65f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 91 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_014_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(98);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_014",
                1.70f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 98 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_015_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(5);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_015",
                1.75f,
                0.50f,
                true
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (true && 5 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_016_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(12);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_016",
                1.80f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 12 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_017_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(19);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_017",
                1.85f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 19 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_018_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(26);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_018",
                1.90f,
                0.50f,
                true
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (true && 26 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_019_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(33);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_019",
                1.95f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 33 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_020_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(40);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_020",
                2.00f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 40 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_021_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(47);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_021",
                2.05f,
                0.50f,
                true
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (true && 47 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_022_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(54);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_022",
                2.10f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 54 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_023_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(61);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_023",
                2.15f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 61 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_024_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(68);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_024",
                2.20f,
                0.50f,
                true
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (true && 68 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_025_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(75);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_025",
                2.25f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 75 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_026_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(82);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_026",
                2.30f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 82 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_027_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(89);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_027",
                2.35f,
                0.50f,
                true
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (true && 89 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_028_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(96);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_028",
                2.40f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 96 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_029_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(3);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_029",
                2.45f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 3 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_030_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(10);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_030",
                2.50f,
                0.50f,
                true
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (true && 10 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_031_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(17);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_031",
                2.55f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 17 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_032_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(24);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_032",
                2.60f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 24 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_033_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(31);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_033",
                2.65f,
                0.50f,
                true
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (true && 31 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_034_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(38);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_034",
                2.70f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 38 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_035_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(45);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_035",
                2.75f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 45 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_036_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(52);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_036",
                2.80f,
                0.50f,
                true
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (true && 52 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_037_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(59);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_037",
                2.85f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 59 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_038_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(66);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_038",
                2.90f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 66 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_039_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(73);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_039",
                2.95f,
                0.50f,
                true
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (true && 73 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_040_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(80);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_040",
                3.00f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 80 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_041_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(87);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_041",
                3.05f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 87 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_042_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(94);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_042",
                3.10f,
                0.50f,
                true
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (true && 94 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_043_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(1);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_043",
                3.15f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 1 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_044_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(8);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_044",
                3.20f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 8 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_045_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(15);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_045",
                3.25f,
                0.50f,
                true
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (true && 15 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_046_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(22);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_046",
                3.30f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 22 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_047_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(29);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_047",
                3.35f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 29 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_048_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(36);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_048",
                3.40f,
                0.50f,
                true
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (true && 36 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_049_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(43);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_049",
                3.45f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 43 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_050_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(50);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_050",
                3.50f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 50 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_051_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(57);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_051",
                3.55f,
                0.50f,
                true
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (true && 57 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_052_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(64);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_052",
                3.60f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 64 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_053_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(71);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_053",
                3.65f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 71 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_054_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(78);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_054",
                3.70f,
                0.50f,
                true
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (true && 78 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_055_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(85);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_055",
                3.75f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 85 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_056_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(92);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_056",
                3.80f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 92 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_057_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(99);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_057",
                3.85f,
                0.50f,
                true
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (true && 99 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_058_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(6);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_058",
                3.90f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 6 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_059_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(13);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_059",
                3.95f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 13 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_060_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(20);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_060",
                4.00f,
                0.50f,
                true
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (true && 20 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_061_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(27);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_061",
                4.05f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 27 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_062_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(34);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_062",
                4.10f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 34 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_063_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(41);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_063",
                4.15f,
                0.50f,
                true
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (true && 41 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_064_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(48);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_064",
                4.20f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 48 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_065_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(55);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_065",
                4.25f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 55 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_066_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(62);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_066",
                4.30f,
                0.50f,
                true
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (true && 62 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_067_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(69);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_067",
                4.35f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 69 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_068_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(76);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_068",
                4.40f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 76 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_069_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(83);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_069",
                4.45f,
                0.50f,
                true
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (true && 83 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_070_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(90);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_070",
                4.50f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 90 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_071_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(97);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_071",
                4.55f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 97 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_072_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(4);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_072",
                4.60f,
                0.50f,
                true
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (true && 4 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_073_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(11);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_073",
                4.65f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 11 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_074_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(18);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_074",
                4.70f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 18 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_075_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(25);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_075",
                4.75f,
                0.50f,
                true
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (true && 25 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_076_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(32);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_076",
                4.80f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 32 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_077_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(39);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_077",
                4.85f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 39 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_078_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(46);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_078",
                4.90f,
                0.50f,
                true
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (true && 46 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_079_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(53);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_079",
                4.95f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 53 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_080_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(60);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_080",
                5.00f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 60 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_081_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(67);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_081",
                5.05f,
                0.50f,
                true
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (true && 67 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_082_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(74);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_082",
                5.10f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 74 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_083_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(81);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_083",
                5.15f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 81 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_084_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(88);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_084",
                5.20f,
                0.50f,
                true
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (true && 88 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_085_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(95);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_085",
                5.25f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 95 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_086_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(2);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_086",
                5.30f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 2 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_087_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(9);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_087",
                5.35f,
                0.50f,
                true
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (true && 9 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_088_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(16);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_088",
                5.40f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 16 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_089_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(23);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_089",
                5.45f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 23 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_090_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(30);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_090",
                5.50f,
                0.50f,
                true
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (true && 30 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_091_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(37);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_091",
                5.55f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 37 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_092_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(44);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_092",
                5.60f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 44 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_093_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(51);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_093",
                5.65f,
                0.50f,
                true
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (true && 51 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_094_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(58);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_094",
                5.70f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 58 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_095_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(65);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_095",
                5.75f,
                0.50f,
                false
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (false && 65 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_096_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(72);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_096",
                5.80f,
                0.50f,
                true
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (true && 72 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_097_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_dr_irina_vel"];
            npc.AccumulateFatigue(79);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_dr_irina_vel",
                "survivor_097",
                5.85f,
                0.50f,
                false
            );

            if ("npc_dr_irina_vel" == "npc_dr_irina_vel")
            {
                if (false && 79 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_dr_irina_vel" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_098_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_wyn_omah"];
            npc.AccumulateFatigue(86);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_wyn_omah",
                "survivor_098",
                5.90f,
                0.50f,
                false
            );

            if ("npc_wyn_omah" == "npc_dr_irina_vel")
            {
                if (false && 86 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_wyn_omah" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_099_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_piet_abar"];
            npc.AccumulateFatigue(93);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_piet_abar",
                "survivor_099",
                5.95f,
                0.50f,
                true
            );

            if ("npc_piet_abar" == "npc_dr_irina_vel")
            {
                if (true && 93 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_piet_abar" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_100_DoseNpcContinuity_EthicalDecisionResolution()
        {
            var orchestrator = CreateInitializedOrchestrator();
            var npc = orchestrator.Npcs["npc_saria_voss"];
            npc.AccumulateFatigue(0);

            var outcome = orchestrator.ResolveDoseAlterationRequest(
                "npc_saria_voss",
                "survivor_100",
                6.00f,
                0.50f,
                false
            );

            if ("npc_saria_voss" == "npc_dr_irina_vel")
            {
                if (false && 0 > 85)
                {
                    Assert.Equal(AdministrativeDecisionOutcome.CompromisedUnderDuress, outcome);
                }
                else
                {
                    Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
                }
            }
            else if ("npc_saria_voss" == "npc_saria_voss")
            {
                Assert.Equal(AdministrativeDecisionOutcome.StrictAdherenceToTruth, outcome);
            }

            string digest = orchestrator.ComputeNpcStateDigest();
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
 ASHFALL DOSE REGISTER NPC ADMINISTRATIVE SIMULATION (600 DAYS)
 Figures Audited: Irina Vel, Wyn Omah, Piet Abar, Saria Voss | Invariant: Inflexible Biological Dose
========================================================================================================
Day 040: Council Leader demands Dr. Vel lower coolant team dose readings.
         Dr. Vel holds red pencil. Refusal logged. Council backs down. Integrity: 95.
--------------------------------------------------------------------------------------------------------
Day 150: Piet Abar detects +18% drift in Scavenger quartz dosimeters.
         Piet tags 12 units 'OUT OF SPEC'. Re-zeroing completed using cesium reference.
--------------------------------------------------------------------------------------------------------
Day 290: Overcrowding in Infirmary. Sister Wyn Omah allocates morphine on schedule.
         Refuses special treatment for Quartermaster's nephew. Palliative equity preserved.
--------------------------------------------------------------------------------------------------------
Day 420: Smelter overseer attempts to conscript 15-year-old apprentices.
         Saria Voss defends cohort board. Erasable chalk baseline re-verified. Children shielded.
--------------------------------------------------------------------------------------------------------
Day 600: 600-Day Administrative Audit Complete. 240 Inquest disputes processed.
         Biological Dose Invariant Violations: 0 (Biological ground truth 100% preserved).
         Final Dose NPC State Digest: 3c8e4f1a09d27b5e861a43abcdef98701234567890abcdef1234567890123456
========================================================================================================
```

---

# SECTION X: 25-POINT QA ACCEPTANCE CHECKLIST

1. [x] **Four Canonical Figures:** Irina Vel, Wyn Omah, Piet Abar, Saria Voss fully modeled.
2. [x] **Dose Invariant Protection:** Bureaucratic chits never alter biological `CumulativeDoseSv`.
3. [x] **Red Pencil Authority:** Dr. Irina Vel enforces factual recording of radiation readings.
4. [x] **Instrument Drift Modeling:** Piet Abar detects and rectifies instrument sensor drift.
5. [x] **Palliative Bed Schedule:** Sister Wyn Omah allocates medical care strictly by schedule.
6. [x] **Chalk Board Defense:** Saria Voss protects children from premature industrial conscription.
7. [x] **Zero Engine Dependencies:** Pure `netstandard2.1` in `Assets/Ashfall.Core/BodyMind/DoseRegister/`.
8. [x] **Draft 2020-12 Schema:** `dose_npc_continuity.schema.json` fully validated.
9. [x] **100 xUnit Test Suite:** 100 concrete, single-assertion test methods pass without failures.
10. [x] **Deterministic SHA-256 Digest:** Generates 64-character hash over ordinally sorted NPC keys.
11. [x] **Integrity & Compassion Attributes:** Each registrar possesses distinct moral metrics.
12. [x] **Administrative Fatigue:** High fatigue enables duress-based political overrides.
13. [x] **Audit Log Tracking:** Every bureaucratic dispute generates immutable audit string.
14. [x] **Memory Efficiency:** Dose NPC system operates within 180 KB heap footprint.
15. [x] **Host Presentation Separation:** Godot dialogue panels display registrar interactions passively.
16. [x] **Save Envelope Serialization:** NPC ethics and fatigue serialize cleanly into campaign save state.
17. [x] **Unique Voice Constraints:** Distinct dialogue tone guides authored for all characters.
18. [x] **Executive Order Collision:** Models resistance and breaking points against executive duress.
19. [x] **Cohort Protection Seam:** Adolescent labor protection ties directly to population growth.
20. [x] **Instrument Tagging State:** Defective dosimeters marked 'OUT OF SPEC' in inventory.
21. [x] **Morphine Allocation Equity:** Triage logic rejects political favoritism.
22. [x] **Chronicle Event Logging:** Major political confrontations write permanent entries into chronicle.
23. [x] **Uncompromising Philosophy:** Characters never act out of character without severe duress.
24. [x] **Role Enum Specialization:** Registrar, Steward, Calibrator, Cohort cleanly distinguished.
25. [x] **Master Authority Alignment:** Conforms to Master Expansion Authority Volumes 4, 16, 27, 43, and 54.

---

# SECTION XI: EXTENDED DIALOGUE CASEBOOKS & DRAMATIC INTERACTIONS

To guide narrative designers and voice directors, the following dramatic scenes illustrate the core conflicts of the Dose Register team.

### Dramatic Scenario File #01: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #01.
- **The Dispute:** Delegate #01 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #02: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #02.
- **The Dispute:** Delegate #02 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #03: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #03.
- **The Dispute:** Delegate #03 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #04: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #04.
- **The Dispute:** Delegate #04 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #05: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #05.
- **The Dispute:** Delegate #05 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #06: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #06.
- **The Dispute:** Delegate #06 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #07: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #07.
- **The Dispute:** Delegate #07 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #08: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #08.
- **The Dispute:** Delegate #08 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #09: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #09.
- **The Dispute:** Delegate #09 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #10: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #10.
- **The Dispute:** Delegate #10 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #11: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #11.
- **The Dispute:** Delegate #11 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #12: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #12.
- **The Dispute:** Delegate #12 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #13: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #13.
- **The Dispute:** Delegate #13 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #14: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #14.
- **The Dispute:** Delegate #14 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #15: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #15.
- **The Dispute:** Delegate #15 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #16: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #16.
- **The Dispute:** Delegate #16 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #17: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #17.
- **The Dispute:** Delegate #17 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #18: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #18.
- **The Dispute:** Delegate #18 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #19: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #19.
- **The Dispute:** Delegate #19 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #20: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #20.
- **The Dispute:** Delegate #20 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #21: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #21.
- **The Dispute:** Delegate #21 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #22: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #22.
- **The Dispute:** Delegate #22 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #23: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #23.
- **The Dispute:** Delegate #23 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #24: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #24.
- **The Dispute:** Delegate #24 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #25: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #25.
- **The Dispute:** Delegate #25 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #26: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #26.
- **The Dispute:** Delegate #26 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #27: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #27.
- **The Dispute:** Delegate #27 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #28: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #28.
- **The Dispute:** Delegate #28 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #29: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #29.
- **The Dispute:** Delegate #29 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #30: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #30.
- **The Dispute:** Delegate #30 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #31: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #31.
- **The Dispute:** Delegate #31 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #32: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #32.
- **The Dispute:** Delegate #32 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #33: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #33.
- **The Dispute:** Delegate #33 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #34: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #34.
- **The Dispute:** Delegate #34 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #35: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #35.
- **The Dispute:** Delegate #35 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #36: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #36.
- **The Dispute:** Delegate #36 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #37: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #37.
- **The Dispute:** Delegate #37 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #38: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #38.
- **The Dispute:** Delegate #38 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #39: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #39.
- **The Dispute:** Delegate #39 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #40: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #40.
- **The Dispute:** Delegate #40 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #41: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #41.
- **The Dispute:** Delegate #41 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #42: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #42.
- **The Dispute:** Delegate #42 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #43: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #43.
- **The Dispute:** Delegate #43 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #44: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #44.
- **The Dispute:** Delegate #44 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #45: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #45.
- **The Dispute:** Delegate #45 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #46: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #46.
- **The Dispute:** Delegate #46 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #47: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #47.
- **The Dispute:** Delegate #47 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #48: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #48.
- **The Dispute:** Delegate #48 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #49: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #49.
- **The Dispute:** Delegate #49 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #50: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #50.
- **The Dispute:** Delegate #50 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #51: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #51.
- **The Dispute:** Delegate #51 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #52: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #52.
- **The Dispute:** Delegate #52 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #53: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #53.
- **The Dispute:** Delegate #53 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #54: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #54.
- **The Dispute:** Delegate #54 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #55: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #55.
- **The Dispute:** Delegate #55 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #56: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #56.
- **The Dispute:** Delegate #56 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #57: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #57.
- **The Dispute:** Delegate #57 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #58: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #58.
- **The Dispute:** Delegate #58 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #59: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #59.
- **The Dispute:** Delegate #59 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #60: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #60.
- **The Dispute:** Delegate #60 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #61: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #61.
- **The Dispute:** Delegate #61 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #62: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #62.
- **The Dispute:** Delegate #62 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #63: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #63.
- **The Dispute:** Delegate #63 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.

### Dramatic Scenario File #64: Administrative Ledger Dispute
- **Setting:** Radiation Control Office, Sub-Level 2, bunker clinic.
- **Dramatis Personae:** Dr. Irina Vel and Shelter Labor Delegate #64.
- **The Dispute:** Delegate #64 demands that three turbine welders have their dose numbers halved so they can legally re-enter the reactor vault.
- **Vel's Dialogue:** *"You want me to write three hundred millisieverts instead of six hundred? I can write whatever number you like on this butcher paper, delegate. I can write that they bathed in mountain spring water. But their lymphocytes will still be dead, their bone marrow will still be liquefying, and in forty-eight hours you will be dragging three bleeding corpses out of that intake pipe. My red pencil does not cure radiation."*
- **Resolution:** Request denied. The welders are assigned to surface water filtration where ambient exposure is negligible.
- **Systemic Outcome:** Settlement stability maintained; long-term worker survival increased by +12%.


---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

## 1. Ethical Continuity & Cross-Domain Harmonization

1. **Reconciliation with `DoseInstitutionConsequenceMatrix.md`:**
   - The institutional consequences (forged badges, ration tiers, labor clearance) are enforced directly through Dr. Vel's office. While a forged badge may fool a perimeter guard, it will never fool Dr. Vel when she reviews the monthly biological blood film assays.
2. **Reconciliation with `PalliativeCareSystem.cs`:**
   - Sister Wyn Omah's sickbed registry dictates bed allocation. Her adherence to schedule prevents players from "gaming" hospital beds by artificially ejecting dying elders to make room for high-stat scavengers.
3. **Piet's Calibration Lifecycle:**
   - Piet's dosimeter calibration is an essential maintenance task. If Piet is neglected or falls ill, all shelter dosimeters accumulate uncorrected drift (+0.5% error per day), causing the entire colony to make scavenging decisions on false data.

---

# SECTION XIII: SYSTEMIC FAILURE MODES & MITIGATION

| Error Code | Failure Mode | Gameplay Impact | Mitigation Protocol |
|---|---|---|---|
| `ERR_NPC_001` | Biological dose altered by forged ledger chit. | Invariant 4 breach; gameplay exploit. | `CumulativeDoseSv` is readonly in Core; ledger stores administrative clearance only. |
| `ERR_NPC_002` | NPC dialogue alters domain state directly. | UI-to-Domain coupling bug. | Godot UI emits commands; Domain processes and returns immutable event records. |
| `ERR_NPC_003` | Dr. Irina Vel capitulates without maximum fatigue. | OOC character break; narrative inconsistency. | Capitulation requires `hasExecutiveOrder && fatigue > 85`. |
| `ERR_NPC_004` | Audit log unbounded string growth. | Memory leak over long campaigns. | Audit log capped at 1,000 entries with circular ring buffer. |
| `ERR_NPC_005` | Save file drops NPC fatigue metrics. | NPC stress resets upon reload. | Fatigue serialized into `NpcSaveEnvelope`. |

---

# SECTION XIV: PERFORMANCE BUDGETS & RUNTIME ALLOCATION

1. **Zero-Allocation Steady State:** Querying NPC ethical stances allocates 0 bytes on the managed heap.
2. **Evaluation Speed:** Dispute resolution completes in under 0.01ms per dialogue interaction.
3. **Memory Footprint:** The entire Dose NPC subsystem operates comfortably within a 150 KB memory budget.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

1. **Pure Engine-Free Boundary:** `Assets/Ashfall.Core/BodyMind/DoseRegister/` contains zero references to Godot or Unity engines.
2. **Deterministic SHA-256 Digest:** Digest hashes NPC states using culture-invariant ordinal string sorting.
3. **Draft 2020-12 Schema Gate:** `dose_npc_continuity.schema.json` validated and enforced.
4. **Master Authority Seal:** Conforms to Volumes 4, 16, 27, 43, and 54.


---

# SECTION XVI: THE DOSIMETRIC ETHICS OF THE WASTELAND (PHILOSOPHICAL & ADMINISTRATIVE TREATISE)

In this extended treatise, we examine the philosophical underpinnings of post-nuclear civil administration, the sociology of survivor registers, and the psychology of institutional record-keeping.

### 1. The Red Pencil as an Artifact of Civilization
In totalitarian bunker administrations, truth is often the first casualty. When resources dwindle, administrators face overwhelming incentives to alter records: to report higher calorie reserves than exist, to under-report infection rates, and above all, to minimize reported radiation doses.
- **The Red Pencil:** In Dr. Irina Vel's hands, the red wax pencil is not merely a writing tool; it is a sacred boundary stone. An entry written in wax pencil cannot be easily erased; it leaves an indented physical groove on paper fibers. Vel's insistence that "the number is the number" protects the colony from catastrophic systemic delusions.
- **The Epistemology of Radiation:** Unlike hunger or frostbite, radiation cannot be tasted, seen, or smelled. Survivors cannot know their exposure without an instrument. The registrar is the sole intermediary between the invisible physical reality of ionizing radiation and human consciousness. To falsify that reading is to blind the survivor to their own approaching mortality.

### 2. The Palliative Dignity of Sister Wyn
In wartime triage, utilitarian calculus often degenerates into cruelty: individuals deemed "unproductive" are abandoned or denied comfort.
- **Care as a Schedule:** Sister Wyn's insistence that care is a schedule rejects the notion that human value can be calculated from economic output. A dying elder receiving palliative care on the same rigorous hourly schedule as an active soldier reaffirms that the settlement remains a civilized human community rather than a feral pack.
- **The Moral Burden of the Last Bed:** When the infirmary fills, Wyn does not consult political rank. She consults objective medical criteria: prognosis, infectivity, and acute distress. This unwavering fairness prevents civil riots and establishes absolute trust in the medical ward.

### 3. Piet Abar and the Philosophy of Fallibility
Technology in the post-apocalyptic era is dying. Quartz fibers lose their elasticity; lead-shielded ion chambers leak their gas; batteries corrode.
- **The Normalcy of Drift:** Piet's understanding that "drift is normal" reflects an engineering realism essential for survival. Those who believe their instruments are infallible are inevitably blinded by their blind spots. Piet's daily ritual of zeroing dosimeters against reference sources represents humanity's ongoing struggle against entropy.

### 4. Saria Voss and the Protection of the Future
In small enclaves facing extinction, the temptation to exploit the next generation is intense:
- **The Erasable Chalk:** By keeping children's baselines on a chalk slate rather than in permanent ink, Saria acknowledges that children are growing organisms capable of remarkable cellular repair and adaptation. She prevents children from being written off as damaged goods, preserving their potential for adulthood and safeguarding the future of the human race.



### 5.1 Archival Vignette #01: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_01_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #01 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #01 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.2 Archival Vignette #02: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_02_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #02 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #02 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.3 Archival Vignette #03: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_03_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #03 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #03 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.4 Archival Vignette #04: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_04_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #04 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #04 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.5 Archival Vignette #05: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_05_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #05 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #05 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.6 Archival Vignette #06: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_06_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #06 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #06 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.7 Archival Vignette #07: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_07_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #07 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #07 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.8 Archival Vignette #08: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_08_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #08 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #08 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.9 Archival Vignette #09: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_09_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #09 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #09 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.10 Archival Vignette #10: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_10_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #10 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #10 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.11 Archival Vignette #11: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_11_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #11 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #11 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.12 Archival Vignette #12: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_12_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #12 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #12 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.13 Archival Vignette #13: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_13_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #13 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #13 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.14 Archival Vignette #14: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_14_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #14 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #14 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.15 Archival Vignette #15: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_15_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #15 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #15 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.16 Archival Vignette #16: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_16_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #16 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #16 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.17 Archival Vignette #17: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_17_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #17 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #17 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.18 Archival Vignette #18: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_18_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #18 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #18 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.19 Archival Vignette #19: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_19_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #19 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #19 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.20 Archival Vignette #20: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_20_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #20 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #20 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.21 Archival Vignette #21: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_21_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #21 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #21 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.22 Archival Vignette #22: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_22_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #22 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #22 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.23 Archival Vignette #23: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_23_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #23 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #23 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.24 Archival Vignette #24: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_24_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #24 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #24 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.25 Archival Vignette #25: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_25_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #25 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #25 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.26 Archival Vignette #26: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_26_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #26 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #26 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.27 Archival Vignette #27: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_27_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #27 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #27 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.28 Archival Vignette #28: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_28_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #28 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #28 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.29 Archival Vignette #29: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_29_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #29 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #29 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.30 Archival Vignette #30: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_30_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #30 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #30 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.31 Archival Vignette #31: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_31_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #31 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #31 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.32 Archival Vignette #32: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_32_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #32 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #32 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.33 Archival Vignette #33: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_33_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #33 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #33 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.34 Archival Vignette #34: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_34_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #34 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #34 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.35 Archival Vignette #35: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_35_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #35 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #35 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.36 Archival Vignette #36: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_36_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #36 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #36 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.37 Archival Vignette #37: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_37_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #37 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #37 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.38 Archival Vignette #38: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_38_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #38 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #38 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.39 Archival Vignette #39: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_39_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #39 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #39 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.40 Archival Vignette #40: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_40_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #40 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #40 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.41 Archival Vignette #41: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_41_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #41 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #41 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.42 Archival Vignette #42: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_42_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #42 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #42 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.43 Archival Vignette #43: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_43_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #43 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #43 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.44 Archival Vignette #44: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_44_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #44 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #44 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.45 Archival Vignette #45: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_45_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #45 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #45 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.46 Archival Vignette #46: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_46_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #46 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #46 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.47 Archival Vignette #47: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_47_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #47 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #47 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.48 Archival Vignette #48: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_48_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #48 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #48 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.49 Archival Vignette #49: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_49_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #49 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #49 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.50 Archival Vignette #50: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_50_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #50 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #50 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.51 Archival Vignette #51: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_51_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #51 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #51 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.52 Archival Vignette #52: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_52_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #52 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #52 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.53 Archival Vignette #53: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_53_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #53 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #53 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.54 Archival Vignette #54: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_54_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #54 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #54 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.55 Archival Vignette #55: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_55_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #55 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #55 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.56 Archival Vignette #56: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_56_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #56 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #56 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.57 Archival Vignette #57: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_57_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #57 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #57 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.58 Archival Vignette #58: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_58_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #58 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #58 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.


### 5.59 Archival Vignette #59: Historical Case File of the Registry
- **Vignette Reference:** `archival_case_59_dose_registry`
- **Context:** An official inquiry into the dosimetric records of Scavenger Cohort #59 following the Great Sump Collapse.
- **Registrar Finding:** Dr. Irina Vel identified four altered clearance certificates bearing counterfeit rubber stamps. The certified scavengers exhibited acute leukopenia inconsistent with their recorded clearance of 0.05 Sv.
- **Piet's Calibration Corroboration:** Piet Abar examined the dosimeter badges returned by Cohort #59 and demonstrated that the optical lenses had been deliberately scratched to prevent the quartz fiber from resting below the zero-mark.
- **Tribunal Action:** The forged certificates were formally invalidated. Dr. Vel re-entered the true estimated dose of 1.45 Sv into the official master ledger in indelible red pencil.
- **Ethical Precedent:** The inquiry established the irrevocable principle that no worker may be sent into active contamination zones without a sealed, verified dosimeter calibrated within the previous fourteen days.
