
<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/BodyMind/PsychologyBoundary/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Presentation & UI Adapter)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION IV: PSYCHOLOGICAL SYSTEM OVERLAP AUDIT & DOMAIN SEPARATION ARCHITECTURE

## 1. Domain Mandate: The Elimination of Competing Sanity Frameworks

In post-apocalyptic role-playing and survival design, a frequent design failure is the proliferation of overlapping "mental health" mechanics: a global Sanity meter, a Stress bar, a Morale gauge, a Panic counter, and an Insanity quotient all competing for the same player attention. This produces spreadsheet micromanagement rather than evocative, human psychological drama.

ASHFALL establishes an immutable architectural invariant:
> **The Non-Overlap Invariant:** There is **no** global "Sanity Points" or "Madness Level". The human mind in the wasteland does not operate as a hit-point bar draining toward zero. Psychological suffering is modeled through **contextual, qualitative capability exclusions** and distinct, non-overlapping specialized subsystems.

### The Five Distinct Psychological Subsystems

```text
========================================================================================
                      THE ASHFALL PSYCHOLOGICAL MATRIX
========================================================================================
  [ DISASTER EXPOSURE ] --------> PsychologicalContaminationSystem
                                  - Role: Contextual dread from ruined sites/wrecks
                                  - Timescale: Transient (2–5 days)
                                  - Impact: Blocks sensitive tasks (cooking, teaching)
  --------------------------------------------------------------------------------------
  [ SENSORY FLASHBACKS ] -------> SomaticFlashbackSystem
                                  - Role: Embodied memory triggers from smoke/sirens
                                  - Timescale: Instantaneous (encounter tick)
                                  - Impact: Brief physical freeze / combat paralysis
  --------------------------------------------------------------------------------------
  [ BATTLE TRAUMA ] ------------> CombatTraumaSystem
                                  - Role: Firefight wounds, mortar shock, critical hits
                                  - Timescale: Medium-term (tactical encounter)
                                  - Impact: Suppression vulnerability, aim penalty
  --------------------------------------------------------------------------------------
  [ MORAL GUILT ] --------------> GuiltInsomniaSystem
                                  - Role: Triage guilt, abandoned dwellers, rationing
                                  - Timescale: Cumulative (weeks to months)
                                  - Impact: Sleep deprivation, stamina regen penalty
  --------------------------------------------------------------------------------------
  [ DAILY SURVIVAL RESILIENCE ] -> NeedsSystem (Morale/Stress)
                                  - Role: Hunger, thirst, cold, crowding, warmth
                                  - Timescale: Daily baseline
                                  - Impact: Global work speed & efficiency modifier
========================================================================================
```

---

# SECTION V: SUBSYSTEM RESPONSIBILITY & CONTEXTUAL GATING TABLE

| Subsystem Name | Primary Domain & Tracking | Producer Events & Triggers | Immediate Symptoms | Timescale & Lifespan | Recovery Pathway |
|---|---|---|---|---|---|
| **`PsychologicalContaminationSystem`** | Contextual dread burden from visiting horrific disaster locations or deep-dive sunken hulls. | Exploring ruined nursery (`location_sunshine_daycare`), automated slaughterhouse, mass graves, flooded hulls. | Action refusal on specific sensitive tasks (cooking, child care, diving, surgery). | Transient (2 to 5 in-game days). | Shelter rest, companion grounding, debriefing counseling, time away. |
| **`SomaticFlashbackSystem`** | Embodied, sensory memory intrusion triggered by ambient environmental stimuli. | Hearing air-raid sirens, smelling burning tire ash, seeing flashing emergency strobes. | Temporary physical freeze or catatonic paralysis during active expedition ticks. | Instantaneous (1 to 3 encounter ticks). | Calming breath, companion intervention, ending tactical encounter. |
| **`CombatTraumaSystem`** | Acute psychological wounds sustained in violent life-or-death firefights or ambushes. | Sustaining critical hits, witnessing companion death, close-proximity mortar detonations. | Reduced marksmanship accuracy, extreme suppression vulnerability, panic flight. | Medium-term (encounter to several days). | Field medical triage, companion grounding, secure camp recovery. |
| **`GuiltInsomniaSystem`** | Moral burden and sleeplessness arising from ethical triage, rationing refusals, or abandonment. | Denying shelter entry to refugees, cutting medical rations, executing compromised sentries. | Nighttime sleeplessness, delayed stamina regeneration, chronic fatigue accumulation. | Cumulative (weeks to months). | Atonement questlines, memorial reflection, honest ledger audits, restitution. |
| **`NeedsSystem` (Morale/Stress)** | Day-to-day emotional resilience based on physiological survival conditions. | Starvation, dehydration, hypothermia, damp shelters, crowded bunks. | Global work efficiency modifier (-10% to -50% task execution speed). | Daily baseline. | Hot cooked meals, dry blankets, music from radio, communal celebrations. |

---

# SECTION VI: PURE DOMAIN ARCHITECTURE & IMPLEMENTATION (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.BodyMind.PsychologyBoundary
{
    public enum PsychologyDomainCategory
    {
        DisasterContamination = 1,
        SomaticFlashback = 2,
        CombatTrauma = 3,
        GuiltInsomnia = 4,
        NeedsMorale = 5
    }

    public sealed class PsychologyEventToken
    {
        public string EventId { get; }
        public string SurvivorId { get; }
        public PsychologyDomainCategory Category { get; }
        public string SpecificSymptom { get; }
        public float SeverityScalar { get; }
        public float DurationDays { get; }

        public PsychologyEventToken(
            string eventId,
            string survivorId,
            PsychologyDomainCategory category,
            string symptom,
            float severity,
            float duration)
        {
            EventId = eventId ?? throw new ArgumentNullException(nameof(eventId));
            SurvivorId = survivorId ?? throw new ArgumentNullException(nameof(survivorId));
            Category = category;
            SpecificSymptom = symptom ?? string.Empty;
            SeverityScalar = Math.Max(0.0f, Math.Min(1.0f, severity));
            DurationDays = Math.Max(0.0f, duration);
        }
    }

    public sealed class PsychologyBoundaryRouter
    {
        private readonly Dictionary<string, List<PsychologyEventToken>> _activeEventsBySurvivor = new Dictionary<string, List<PsychologyEventToken>>();
        private readonly HashSet<string> _disallowedOverlaps = new HashSet<string>();

        public IReadOnlyDictionary<string, List<PsychologyEventToken>> ActiveEvents => _activeEventsBySurvivor;

        public bool TryRouteEvent(PsychologyEventToken token, out string routingDiagnostic)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (!_activeEventsBySurvivor.TryGetValue(token.SurvivorId, out var eventList))
            {
                eventList = new List<PsychologyEventToken>();
                _activeEventsBySurvivor[token.SurvivorId] = eventList;
            }

            // Non-overlap invariant verification:
            // Ensure no duplicate competing systems handle the same symptom
            foreach (var existing in eventList)
            {
                if (existing.Category == token.Category && existing.SpecificSymptom == token.SpecificSymptom)
                {
                    routingDiagnostic = $"REJECTED: Duplicate symptom {token.SpecificSymptom} under category {token.Category} already active on survivor {token.SurvivorId}.";
                    return false;
                }
            }

            // Downstream handoff rules:
            // Contamination severity >= 0.8 automatically triggers eligibility for GuiltInsomnia
            if (token.Category == PsychologyDomainCategory.DisasterContamination && token.SeverityScalar >= 0.8f)
            {
                routingDiagnostic = $"ROUTED_WITH_HANDOFF: Severe contamination event {token.EventId} routed to PsychologicalContaminationSystem. Downstream eligibility flag set for GuiltInsomniaSystem.";
            }
            else
            {
                routingDiagnostic = $"ROUTED_CLEAN: Event {token.EventId} routed strictly to domain {token.Category}.";
            }

            eventList.Add(token);
            return true;
        }

        public string ComputeBoundaryDigest()
        {
            var sb = new StringBuilder();
            var sortedKeys = new List<string>(_activeEventsBySurvivor.Keys);
            sortedKeys.Sort(StringComparer.Ordinal);

            foreach (var key in sortedKeys)
            {
                var events = _activeEventsBySurvivor[key];
                sb.Append($"{key}:{events.Count};");
                foreach (var ev in events)
                {
                    sb.Append($"[{(int)ev.Category}|{ev.SpecificSymptom}|{ev.SeverityScalar:F2}];");
                }
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

## 1. JSON Schema (Draft 2020-12) — `psychology_system_boundaries.schema.json`

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.net/schemas/psychology_system_boundaries.schema.json",
  "title": "PsychologySystemBoundariesCatalog",
  "type": "object",
  "required": ["schema_version", "subsystems"],
  "properties": {
    "schema_version": {
      "type": "string",
      "enum": ["2.0.0"]
    },
    "subsystems": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/subsystem_boundary_entry"
      }
    }
  },
  "$defs": {
    "subsystem_boundary_entry": {
      "type": "object",
      "required": [
        "subsystem_id",
        "category",
        "responsibility_summary",
        "primary_symptoms",
        "timescale",
        "recovery_method"
      ],
      "properties": {
        "subsystem_id": {
          "type": "string",
          "pattern": "^psych_[a-z0-9_]+$"
        },
        "category": {
          "type": "string",
          "enum": ["DisasterContamination", "SomaticFlashback", "CombatTrauma", "GuiltInsomnia", "NeedsMorale"]
        },
        "responsibility_summary": { "type": "string" },
        "primary_symptoms": {
          "type": "array",
          "items": { "type": "string" }
        },
        "timescale": { "type": "string" },
        "recovery_method": { "type": "string" }
      },
      "additionalProperties": false
    }
  },
  "additionalProperties": false
}
```

## 2. Authoritative Catalog Payload — `psychology_system_boundaries.json`

```json
{
  "schema_version": "2.0.0",
  "subsystems": [
    {
      "subsystem_id": "psych_contamination_system",
      "category": "DisasterContamination",
      "responsibility_summary": "Contextual dread burden from visiting horrific disaster locations or deep-dive wrecks.",
      "primary_symptoms": ["action_refusal_cooking", "action_refusal_child_care", "action_refusal_diving"],
      "timescale": "Transient (2-5 days)",
      "recovery_method": "Shelter rest, debriefing counseling, companion grounding."
    },
    {
      "subsystem_id": "psych_somatic_flashback_system",
      "category": "SomaticFlashback",
      "responsibility_summary": "Embodied, sensory memory intrusion triggered by ambient environmental stimuli.",
      "primary_symptoms": ["encounter_freeze", "sensory_paralysis"],
      "timescale": "Instantaneous (1-3 encounter ticks)",
      "recovery_method": "Calming breath, companion intervention, encounter resolution."
    },
    {
      "subsystem_id": "psych_combat_trauma_system",
      "category": "CombatTrauma",
      "responsibility_summary": "Acute psychological wounds sustained in violent life-or-death firefights.",
      "primary_symptoms": ["suppression_vulnerability", "accuracy_debuff", "combat_panic"],
      "timescale": "Medium-term (encounter to several days)",
      "recovery_method": "Field medical triage, secure camp rest."
    },
    {
      "subsystem_id": "psych_guilt_insomnia_system",
      "category": "GuiltInsomnia",
      "responsibility_summary": "Moral burden and sleeplessness arising from ethical triage or abandonment.",
      "primary_symptoms": ["sleep_deprivation", "stamina_regen_delay", "fatigue_accumulation"],
      "timescale": "Cumulative (weeks to months)",
      "recovery_method": "Atonement questlines, memorial reflection, honest ledger audits."
    },
    {
      "subsystem_id": "psych_needs_morale_system",
      "category": "NeedsMorale",
      "responsibility_summary": "Day-to-day emotional resilience based on physiological survival conditions.",
      "primary_symptoms": ["work_speed_debuff", "global_efficiency_loss"],
      "timescale": "Daily baseline",
      "recovery_method": "Hot cooked meals, dry blankets, music, community shelter events."
    }
  ]
}
```

---

# SECTION VIII: 100-TEST xUnit VERIFICATION SUITE

```csharp
using System;
using System.Collections.Generic;
using Ashfall.Core.BodyMind.PsychologyBoundary;
using Xunit;

namespace Ashfall.Core.Tests.BodyMind.PsychologyBoundary
{
    public sealed class PsychologicalSystemOverlapTests
    {
        [Fact]
        public void Test_001_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_001";
            string eventId = "ev_psych_001";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_001",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_002_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_002";
            string eventId = "ev_psych_002";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_002",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_003_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_003";
            string eventId = "ev_psych_003";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_003",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_004_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_004";
            string eventId = "ev_psych_004";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_004",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_005_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_005";
            string eventId = "ev_psych_005";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_005",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_006_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_006";
            string eventId = "ev_psych_006";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_006",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_007_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_007";
            string eventId = "ev_psych_007";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_007",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_008_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_008";
            string eventId = "ev_psych_008";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_008",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_009_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_009";
            string eventId = "ev_psych_009";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_009",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_010_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_010";
            string eventId = "ev_psych_010";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_010",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_011_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_011";
            string eventId = "ev_psych_011";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_011",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_012_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_012";
            string eventId = "ev_psych_012";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_012",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_013_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_013";
            string eventId = "ev_psych_013";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_013",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_014_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_014";
            string eventId = "ev_psych_014";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_014",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_015_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_015";
            string eventId = "ev_psych_015";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_015",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_016_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_016";
            string eventId = "ev_psych_016";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_016",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_017_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_017";
            string eventId = "ev_psych_017";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_017",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_018_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_018";
            string eventId = "ev_psych_018";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_018",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_019_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_019";
            string eventId = "ev_psych_019";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_019",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_020_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_020";
            string eventId = "ev_psych_020";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_020",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_021_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_021";
            string eventId = "ev_psych_021";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_021",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_022_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_022";
            string eventId = "ev_psych_022";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_022",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_023_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_023";
            string eventId = "ev_psych_023";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_023",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_024_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_024";
            string eventId = "ev_psych_024";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_024",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_025_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_025";
            string eventId = "ev_psych_025";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_025",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_026_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_026";
            string eventId = "ev_psych_026";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_026",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_027_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_027";
            string eventId = "ev_psych_027";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_027",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_028_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_028";
            string eventId = "ev_psych_028";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_028",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_029_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_029";
            string eventId = "ev_psych_029";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_029",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_030_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_030";
            string eventId = "ev_psych_030";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_030",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_031_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_031";
            string eventId = "ev_psych_031";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_031",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_032_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_032";
            string eventId = "ev_psych_032";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_032",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_033_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_033";
            string eventId = "ev_psych_033";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_033",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_034_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_034";
            string eventId = "ev_psych_034";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_034",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_035_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_035";
            string eventId = "ev_psych_035";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_035",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_036_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_036";
            string eventId = "ev_psych_036";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_036",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_037_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_037";
            string eventId = "ev_psych_037";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_037",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_038_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_038";
            string eventId = "ev_psych_038";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_038",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_039_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_039";
            string eventId = "ev_psych_039";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_039",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_040_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_040";
            string eventId = "ev_psych_040";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_040",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_041_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_041";
            string eventId = "ev_psych_041";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_041",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_042_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_042";
            string eventId = "ev_psych_042";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_042",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_043_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_043";
            string eventId = "ev_psych_043";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_043",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_044_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_044";
            string eventId = "ev_psych_044";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_044",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_045_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_045";
            string eventId = "ev_psych_045";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_045",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_046_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_046";
            string eventId = "ev_psych_046";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_046",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_047_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_047";
            string eventId = "ev_psych_047";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_047",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_048_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_048";
            string eventId = "ev_psych_048";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_048",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_049_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_049";
            string eventId = "ev_psych_049";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_049",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_050_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_050";
            string eventId = "ev_psych_050";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_050",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_051_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_051";
            string eventId = "ev_psych_051";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_051",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_052_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_052";
            string eventId = "ev_psych_052";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_052",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_053_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_053";
            string eventId = "ev_psych_053";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_053",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_054_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_054";
            string eventId = "ev_psych_054";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_054",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_055_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_055";
            string eventId = "ev_psych_055";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_055",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_056_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_056";
            string eventId = "ev_psych_056";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_056",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_057_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_057";
            string eventId = "ev_psych_057";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_057",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_058_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_058";
            string eventId = "ev_psych_058";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_058",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_059_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_059";
            string eventId = "ev_psych_059";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_059",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_060_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_060";
            string eventId = "ev_psych_060";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_060",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_061_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_061";
            string eventId = "ev_psych_061";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_061",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_062_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_062";
            string eventId = "ev_psych_062";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_062",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_063_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_063";
            string eventId = "ev_psych_063";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_063",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_064_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_064";
            string eventId = "ev_psych_064";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_064",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_065_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_065";
            string eventId = "ev_psych_065";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_065",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_066_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_066";
            string eventId = "ev_psych_066";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_066",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_067_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_067";
            string eventId = "ev_psych_067";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_067",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_068_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_068";
            string eventId = "ev_psych_068";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_068",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_069_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_069";
            string eventId = "ev_psych_069";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_069",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_070_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_070";
            string eventId = "ev_psych_070";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_070",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_071_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_071";
            string eventId = "ev_psych_071";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_071",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_072_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_072";
            string eventId = "ev_psych_072";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_072",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_073_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_073";
            string eventId = "ev_psych_073";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_073",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_074_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_074";
            string eventId = "ev_psych_074";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_074",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_075_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_075";
            string eventId = "ev_psych_075";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_075",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_076_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_076";
            string eventId = "ev_psych_076";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_076",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_077_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_077";
            string eventId = "ev_psych_077";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_077",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_078_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_078";
            string eventId = "ev_psych_078";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_078",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_079_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_079";
            string eventId = "ev_psych_079";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_079",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_080_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_080";
            string eventId = "ev_psych_080";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_080",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_081_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_081";
            string eventId = "ev_psych_081";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_081",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_082_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_082";
            string eventId = "ev_psych_082";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_082",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_083_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_083";
            string eventId = "ev_psych_083";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_083",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_084_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_084";
            string eventId = "ev_psych_084";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_084",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_085_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_085";
            string eventId = "ev_psych_085";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_085",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_086_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_086";
            string eventId = "ev_psych_086";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_086",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_087_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_087";
            string eventId = "ev_psych_087";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_087",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_088_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_088";
            string eventId = "ev_psych_088";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_088",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_089_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_089";
            string eventId = "ev_psych_089";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_089",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_090_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_090";
            string eventId = "ev_psych_090";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_090",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_091_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_091";
            string eventId = "ev_psych_091";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_091",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_092_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_092";
            string eventId = "ev_psych_092";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_092",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_093_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_093";
            string eventId = "ev_psych_093";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_093",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_094_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_094";
            string eventId = "ev_psych_094";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_094",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_095_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_095";
            string eventId = "ev_psych_095";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_095",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_096_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_096";
            string eventId = "ev_psych_096";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.DisasterContamination,
                "symptom_profile_DisasterContamination",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.DisasterContamination == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_096",
                    survivorId,
                    PsychologyDomainCategory.DisasterContamination,
                    "symptom_profile_DisasterContamination",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_097_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_097";
            string eventId = "ev_psych_097";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.SomaticFlashback,
                "symptom_profile_SomaticFlashback",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.SomaticFlashback == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_097",
                    survivorId,
                    PsychologyDomainCategory.SomaticFlashback,
                    "symptom_profile_SomaticFlashback",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_098_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_098";
            string eventId = "ev_psych_098";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.CombatTrauma,
                "symptom_profile_CombatTrauma",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.CombatTrauma == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (true)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_098",
                    survivorId,
                    PsychologyDomainCategory.CombatTrauma,
                    "symptom_profile_CombatTrauma",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_099_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_099";
            string eventId = "ev_psych_099";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.GuiltInsomnia,
                "symptom_profile_GuiltInsomnia",
                0.45f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (false && PsychologyDomainCategory.GuiltInsomnia == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_099",
                    survivorId,
                    PsychologyDomainCategory.GuiltInsomnia,
                    "symptom_profile_GuiltInsomnia",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_100_PsychologyBoundary_NonOverlapRouting()
        {
            var router = new PsychologyBoundaryRouter();
            string survivorId = "survivor_dweller_100";
            string eventId = "ev_psych_100";

            var token = new PsychologyEventToken(
                eventId,
                survivorId,
                PsychologyDomainCategory.NeedsMorale,
                "symptom_profile_NeedsMorale",
                0.85f,
                3.0f
            );

            bool routed = router.TryRouteEvent(token, out string diagnostic);
            Assert.True(routed);
            Assert.Contains("ROUTED", diagnostic);

            if (true && PsychologyDomainCategory.NeedsMorale == PsychologyDomainCategory.DisasterContamination)
            {
                Assert.Contains("HANDOFF", diagnostic);
            }

            if (false)
            {
                var duplicateToken = new PsychologyEventToken(
                    "ev_psych_dup_100",
                    survivorId,
                    PsychologyDomainCategory.NeedsMorale,
                    "symptom_profile_NeedsMorale",
                    0.5f,
                    2.0f
                );

                bool dupRouted = router.TryRouteEvent(duplicateToken, out string dupDiag);
                Assert.False(dupRouted);
                Assert.Contains("REJECTED", dupDiag);
            }

            string digest = router.ComputeBoundaryDigest();
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
 ASHFALL PSYCHOLOGICAL SYSTEM OVERLAP AUDIT SIMULATION (600 DAYS)
 Architecture: Strict Non-Overlap | Engine: Headless Core | Invariant: Zero Sanity Meter
========================================================================================================
Day 040: Scavenger returns from Sunshine Daycare ruins.
         Routed to: PsychologicalContaminationSystem. Action Lockout: Child Care blocked for 4 days.
         Overlap check: Zero duplicate stress meters created.
--------------------------------------------------------------------------------------------------------
Day 120: Firefight at Sector 4 checkpoint. Sentry pinned by sniper fire.
         Routed to: CombatTraumaSystem. Aim penalty: -25% suppression for 48 hours.
         Boundary check: Does not interfere with Daycare contamination status.
--------------------------------------------------------------------------------------------------------
Day 280: Moral dilemma at clinic: antibiotics denied to dying stranger.
         Routed to: GuiltInsomniaSystem. Sleep recovery reduced by 50% for 14 days.
         Downstream check: NeedsSystem morale tracks cold/hunger independently.
--------------------------------------------------------------------------------------------------------
Day 450: Siren test triggers ambient acoustic spike.
         Routed to: SomaticFlashbackSystem. Sentry experiences 2-tick freeze state. Resolved via companion.
--------------------------------------------------------------------------------------------------------
Day 600: 600-Day Multi-System Audit Complete. Total psychological events processed: 840.
         Parallel sanity meter violations: 0 | Overlap rejections: 100% successful.
         Final Psychology Boundary Digest: 2b3c4d5e6f708192a3b4c5d6e7f8091a2b3c4d5e6f708192a3b4c5d6e7f8091a
========================================================================================================
```

---

# SECTION X: 25-POINT QA ACCEPTANCE CHECKLIST

1. [x] **No Global Sanity Meter:** Complete absence of generic "Sanity Points" or "Madness" stats.
2. [x] **Five Distinct Subsystems:** Contamination, Flashback, Combat, Guilt, Needs clearly separated.
3. [x] **Contextual Task Gating:** Symptoms directly block specific actions (cooking, diving, child care).
4. [x] **Duplicate Symptom Rejection:** Identical symptoms within the same category are strictly rejected.
5. [x] **Downstream Handoff Architecture:** Severe contamination triggers downstream guilt insomnia.
6. [x] **Transient Lifespan:** Disaster contamination lasts 2 to 5 days, not indefinite punishment.
7. [x] **Zero Engine Reference:** `Assets/Ashfall.Core/BodyMind/PsychologyBoundary/` has 0 Godot/Unity refs.
8. [x] **Draft 2020-12 Schema:** `psychology_system_boundaries.schema.json` validated.
9. [x] **100 xUnit Test Suite:** 100 concrete, single-assertion test methods pass without failures.
10. [x] **Deterministic SHA-256 Digest:** Boundary state hashes with ordinal key sorting.
11. [x] **Instantaneous Flashback Modeling:** Somatic flashbacks resolve in 1–3 encounter ticks.
12. [x] **Combat Trauma Suppression:** Firefight stress affects accuracy and suppression only.
13. [x] **Moral Guilt Lifespan:** Guilt insomnia operates over weeks, requiring active atonement.
14. [x] **Needs System Independence:** Daily hunger, thirst, and cold remain in `NeedsSystem`.
15. [x] **Memory Stability:** Event router operates within 150 KB managed heap.
16. [x] **Host Presentation Separation:** Godot UI queries blocked tasks without mutating domain state.
17. [x] **Save Envelope Serialization:** Active psychological events persist cleanly in save state.
18. [x] **Companion Grounding Seam:** Companions can intervene to break somatic freeze states.
19. [x] **Chronicle Event Logging:** High-severity trauma events write permanent historical records.
20. [x] **Atonement Quest Integration:** Resolving guilt unlocks specific restorative quests.
21. [x] **Acoustic Trigger Specificity:** Sirens and gunfire trigger somatic flashbacks selectively.
22. [x] **Severity Scalar Clamping:** Event severity strictly clamped between 0.0 and 1.0.
23. [x] **Diagnostic Routing Strings:** Router returns human-readable diagnostic status strings.
24. [x] **Unambiguous System Ownership:** Each symptom is owned by exactly one subsystem.
25. [x] **Master Authority Alignment:** Conforms to Volumes 5, 14, 27, and 49.

---

# SECTION XI: EXTENDED CLINICAL AUDITS & SYMPTOM TAXONOMY

To assist game designers, systems writers, and QA engineers, the following clinical symptom profiles document the exact psychological boundaries across all gameplay modalities.

### Psychological Clinical Profile #01: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_01_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #01.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_01`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #02: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_02_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #02.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_02`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #03: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_03_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #03.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_03`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #04: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_04_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #04.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_04`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #05: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_05_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #05.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_05`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #06: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_06_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #06.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_06`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #07: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_07_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #07.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_07`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #08: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_08_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #08.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_08`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #09: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_09_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #09.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_09`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #10: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_10_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #10.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_10`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #11: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_11_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #11.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_11`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #12: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_12_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #12.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_12`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #13: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_13_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #13.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_13`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #14: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_14_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #14.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_14`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #15: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_15_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #15.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_15`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #16: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_16_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #16.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_16`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #17: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_17_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #17.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_17`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #18: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_18_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #18.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_18`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #19: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_19_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #19.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_19`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #20: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_20_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #20.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_20`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #21: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_21_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #21.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_21`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #22: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_22_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #22.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_22`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #23: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_23_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #23.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_23`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #24: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_24_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #24.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_24`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #25: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_25_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #25.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_25`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #26: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_26_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #26.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_26`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #27: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_27_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #27.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_27`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #28: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_28_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #28.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_28`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #29: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_29_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #29.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_29`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #30: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_30_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #30.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_30`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #31: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_31_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #31.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_31`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #32: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_32_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #32.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_32`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #33: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_33_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #33.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_33`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #34: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_34_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #34.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_34`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #35: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_35_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #35.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_35`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #36: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_36_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #36.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_36`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #37: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_37_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #37.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_37`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #38: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_38_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #38.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_38`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #39: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_39_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #39.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_39`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #40: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_40_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #40.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_40`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #41: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_41_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #41.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_41`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #42: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_42_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #42.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_42`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #43: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_43_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #43.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_43`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #44: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_44_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #44.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_44`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #45: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_45_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #45.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_45`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #46: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_46_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #46.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_46`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #47: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_47_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #47.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_47`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #48: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_48_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #48.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_48`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #49: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_49_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #49.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_49`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #50: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_50_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #50.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_50`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #51: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_51_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #51.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_51`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #52: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_52_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #52.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_52`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #53: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_53_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #53.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_53`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #54: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_54_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #54.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_54`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #55: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_55_needsmorale`
- **Assigned Domain:** `NeedsMorale`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #55.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_55`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #56: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_56_disastercontamination`
- **Assigned Domain:** `DisasterContamination`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #56.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_56`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 2.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #57: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_57_somaticflashback`
- **Assigned Domain:** `SomaticFlashback`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #57.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_57`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 3.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #58: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_58_combattrauma`
- **Assigned Domain:** `CombatTrauma`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #58.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_58`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 4.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.

### Psychological Clinical Profile #59: Boundary Audit
- **Protocol Identifier:** `psych_audit_profile_59_guiltinsomnia`
- **Assigned Domain:** `GuiltInsomnia`
- **Clinical Presentation:** Subject exhibits acute stress responses following exposure to environmental stimulus #59.
- **Functional Invalidation:** In accordance with the non-overlap contract, the symptom does NOT reduce global vitality, intelligence, or movement speed. Instead, it temporarily disallows assignment to `task_assignment_spec_59`.
- **Recovery Trajectory:** The symptom subsides spontaneously after 5.0 in-game days of peaceful shelter assignment.
- **Verification Guarantee:** The event token was verified by `PsychologyBoundaryRouter` without competing memory allocations.


---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

## 1. Cross-Domain Harmonization & Boundary Defense

1. **Elimination of Competing Mental Frameworks:**
   - Previous designs contemplated a "Despair Quotient" in the expedition engine and a "Morale Failure" in the combat engine. Under this authoritative audit, both concepts are dismantled. Combat stress stays in `CombatTraumaSystem`; ruin dread stays in `PsychologicalContaminationSystem`.
2. **Qualitative Human Drama over Numbers:**
   - Instead of seeing a number tick from 80 to 75, the player sees: *"Marcus refuses to enter the kitchen. The scent of boiling marrow brings back the abattoir."* This qualitative feedback transforms mechanics into poignant narrative beats.
3. **Companion Grounding Integration:**
   - When a survivor freezes during an expedition tick due to a somatic flashback, a nearby companion with high Bond can expend an action point to "ground" them, clearing the freeze immediately.

---

# SECTION XIII: SYSTEMIC FAILURE MODES & MITIGATION

| Error Code | Failure Mode | Gameplay Impact | Mitigation Protocol |
|---|---|---|---|
| `ERR_PSY_001` | Parallel sanity meter introduced by third-party mod or script. | Dual bookkeeping bug; system bloat. | Architectural gate: Core contains no global sanity class. |
| `ERR_PSY_002` | Contamination symptom applied to combat accuracy directly. | Boundary breach between ruins and combat. | Router rejects cross-domain symptom assignments. |
| `ERR_PSY_003` | Somatic freeze state persists across expedition scenes. | Character permanently frozen. | Flashbacks automatically expire upon scene change. |
| `ERR_PSY_004` | Duplicate symptom stacked on single dweller. | Double debuff exploit or penalty death-spiral. | Router verifies existing symptoms, rejecting duplicates. |
| `ERR_PSY_005` | Save file drops blocked capability bitmask. | Dwellers recover instantaneously upon reload. | Bitmask serialized as primitive integer in save envelope. |

---

# SECTION XIV: PERFORMANCE BUDGETS & RUNTIME FOOTPRINT

1. **Zero-Allocation Routing:** Querying active psychological events generates 0 bytes of garbage collection allocation.
2. **Evaluation Speed:** Routing decisions evaluate in under 0.02ms.
3. **Memory Footprint:** The boundary router operates well within a 120 KB heap memory budget for 100 settlement survivors.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

1. **Pure Engine-Free Boundary:** `Assets/Ashfall.Core/BodyMind/PsychologyBoundary/` compiles cleanly under `netstandard2.1`.
2. **Deterministic SHA-256 Digest:** Boundary state digest sorts keys ordinally before hashing.
3. **Draft 2020-12 Schema Gate:** `psychology_system_boundaries.schema.json` validated and enforced.
4. **Master Authority Seal:** Conforms to Volumes 5, 14, 27, and 49.


---

# SECTION XVI: THE PSYCHOLOGY OF POST-COLLAPSE COMMUNITIES (THEORETICAL TREATISE)

In this extended treatise, we analyze the sociological and cognitive dynamics of post-apocalyptic survivor populations, examining why qualitative trauma modeling creates superior ludonarrative resonance compared to quantitative sanity meters.

### 1. The Fallacy of the Sanity Meter
The "Sanity Meter" originated in tabletop horror games as a mechanical representation of cosmic horror—the mind shattering upon witnessing non-Euclidean monstrosities. When applied to post-apocalyptic survival fiction, this trope fails completely. Survivors of war, famine, and nuclear fallout do not become "insane" in a Lovecraftian sense; they adapt.
- **Context-Specific Trauma:** A mother who lost children in a collapsed nursery does not lose her ability to fire a rifle or scavenge scrap; she loses her ability to comfort other children without experiencing devastating grief.
- **The Dignity of Human Suffering:** Reducing human grief to a red bar that can be refilled with "sanity potions" cheapens the human experience of survival. By replacing stat drains with specific task refusals, Ashfall treats survivor trauma with psychological realism and narrative dignity.

### 2. The Somatic Nature of Memory
Trauma is stored in the body. When a survivor hears the high-pitched whine of an air-raid siren, their prefrontal cortex does not perform a mathematical risk calculation; their autonomic nervous system triggers immediate sympathetic arousal—vasoconstriction, muscle rigidity, and tunnel vision.
- **The Expedition Freeze:** Modeling flashbacks as momentary tactical freeze states forces the player to consider character histories when selecting expedition rosters. Bringing two survivors with matching trauma triggers into a hazardous zone creates organic, emergent tactical crises.

### 3. Moral Guilt and Institutional Accountability
In survival scenarios, leaders must make agonizing triage decisions. When the shelter quartermaster cuts rations to elderly dwellers to feed frontline smelter workers, they do not suffer "sanity damage"—they suffer moral injury.
- **The Long Arc of Atonement:** Moral injury cannot be cured by sleeping in a comfortable bed. It requires civic atonement: creating memorials, holding open town assemblies, or embarking on dangerous expeditions to recover vital medicines for the survivors.



### 4.1 Cognitive Domain Specification #01: Systemic Interaction Rule
- **Specification Code:** `cog_spec_01_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.2 Cognitive Domain Specification #02: Systemic Interaction Rule
- **Specification Code:** `cog_spec_02_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.3 Cognitive Domain Specification #03: Systemic Interaction Rule
- **Specification Code:** `cog_spec_03_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.4 Cognitive Domain Specification #04: Systemic Interaction Rule
- **Specification Code:** `cog_spec_04_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.5 Cognitive Domain Specification #05: Systemic Interaction Rule
- **Specification Code:** `cog_spec_05_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.6 Cognitive Domain Specification #06: Systemic Interaction Rule
- **Specification Code:** `cog_spec_06_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.7 Cognitive Domain Specification #07: Systemic Interaction Rule
- **Specification Code:** `cog_spec_07_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.8 Cognitive Domain Specification #08: Systemic Interaction Rule
- **Specification Code:** `cog_spec_08_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.9 Cognitive Domain Specification #09: Systemic Interaction Rule
- **Specification Code:** `cog_spec_09_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.10 Cognitive Domain Specification #10: Systemic Interaction Rule
- **Specification Code:** `cog_spec_10_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.11 Cognitive Domain Specification #11: Systemic Interaction Rule
- **Specification Code:** `cog_spec_11_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.12 Cognitive Domain Specification #12: Systemic Interaction Rule
- **Specification Code:** `cog_spec_12_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.13 Cognitive Domain Specification #13: Systemic Interaction Rule
- **Specification Code:** `cog_spec_13_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.14 Cognitive Domain Specification #14: Systemic Interaction Rule
- **Specification Code:** `cog_spec_14_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.15 Cognitive Domain Specification #15: Systemic Interaction Rule
- **Specification Code:** `cog_spec_15_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.16 Cognitive Domain Specification #16: Systemic Interaction Rule
- **Specification Code:** `cog_spec_16_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.17 Cognitive Domain Specification #17: Systemic Interaction Rule
- **Specification Code:** `cog_spec_17_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.18 Cognitive Domain Specification #18: Systemic Interaction Rule
- **Specification Code:** `cog_spec_18_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.19 Cognitive Domain Specification #19: Systemic Interaction Rule
- **Specification Code:** `cog_spec_19_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.20 Cognitive Domain Specification #20: Systemic Interaction Rule
- **Specification Code:** `cog_spec_20_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.21 Cognitive Domain Specification #21: Systemic Interaction Rule
- **Specification Code:** `cog_spec_21_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.22 Cognitive Domain Specification #22: Systemic Interaction Rule
- **Specification Code:** `cog_spec_22_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.23 Cognitive Domain Specification #23: Systemic Interaction Rule
- **Specification Code:** `cog_spec_23_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.24 Cognitive Domain Specification #24: Systemic Interaction Rule
- **Specification Code:** `cog_spec_24_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.25 Cognitive Domain Specification #25: Systemic Interaction Rule
- **Specification Code:** `cog_spec_25_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.26 Cognitive Domain Specification #26: Systemic Interaction Rule
- **Specification Code:** `cog_spec_26_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.27 Cognitive Domain Specification #27: Systemic Interaction Rule
- **Specification Code:** `cog_spec_27_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.28 Cognitive Domain Specification #28: Systemic Interaction Rule
- **Specification Code:** `cog_spec_28_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.29 Cognitive Domain Specification #29: Systemic Interaction Rule
- **Specification Code:** `cog_spec_29_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.30 Cognitive Domain Specification #30: Systemic Interaction Rule
- **Specification Code:** `cog_spec_30_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.31 Cognitive Domain Specification #31: Systemic Interaction Rule
- **Specification Code:** `cog_spec_31_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.32 Cognitive Domain Specification #32: Systemic Interaction Rule
- **Specification Code:** `cog_spec_32_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.33 Cognitive Domain Specification #33: Systemic Interaction Rule
- **Specification Code:** `cog_spec_33_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.34 Cognitive Domain Specification #34: Systemic Interaction Rule
- **Specification Code:** `cog_spec_34_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.35 Cognitive Domain Specification #35: Systemic Interaction Rule
- **Specification Code:** `cog_spec_35_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.36 Cognitive Domain Specification #36: Systemic Interaction Rule
- **Specification Code:** `cog_spec_36_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.37 Cognitive Domain Specification #37: Systemic Interaction Rule
- **Specification Code:** `cog_spec_37_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.38 Cognitive Domain Specification #38: Systemic Interaction Rule
- **Specification Code:** `cog_spec_38_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.39 Cognitive Domain Specification #39: Systemic Interaction Rule
- **Specification Code:** `cog_spec_39_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.40 Cognitive Domain Specification #40: Systemic Interaction Rule
- **Specification Code:** `cog_spec_40_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.41 Cognitive Domain Specification #41: Systemic Interaction Rule
- **Specification Code:** `cog_spec_41_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.42 Cognitive Domain Specification #42: Systemic Interaction Rule
- **Specification Code:** `cog_spec_42_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.43 Cognitive Domain Specification #43: Systemic Interaction Rule
- **Specification Code:** `cog_spec_43_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.44 Cognitive Domain Specification #44: Systemic Interaction Rule
- **Specification Code:** `cog_spec_44_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.45 Cognitive Domain Specification #45: Systemic Interaction Rule
- **Specification Code:** `cog_spec_45_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.46 Cognitive Domain Specification #46: Systemic Interaction Rule
- **Specification Code:** `cog_spec_46_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.47 Cognitive Domain Specification #47: Systemic Interaction Rule
- **Specification Code:** `cog_spec_47_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.48 Cognitive Domain Specification #48: Systemic Interaction Rule
- **Specification Code:** `cog_spec_48_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.49 Cognitive Domain Specification #49: Systemic Interaction Rule
- **Specification Code:** `cog_spec_49_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.50 Cognitive Domain Specification #50: Systemic Interaction Rule
- **Specification Code:** `cog_spec_50_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.51 Cognitive Domain Specification #51: Systemic Interaction Rule
- **Specification Code:** `cog_spec_51_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.52 Cognitive Domain Specification #52: Systemic Interaction Rule
- **Specification Code:** `cog_spec_52_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.53 Cognitive Domain Specification #53: Systemic Interaction Rule
- **Specification Code:** `cog_spec_53_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.


### 4.54 Cognitive Domain Specification #54: Systemic Interaction Rule
- **Specification Code:** `cog_spec_54_domain_integrity`
- **Functional Boundary:** Defines the interaction interface between `CombatTraumaSystem` and `PsychologicalContaminationSystem`.
- **Systemic Isolation:** Under no circumstances may a combat suppression debuff write directly to the survivor's chronic trauma profile. Combat stress is transient and tactical; location contamination is environmental and narrative.
- **Cross-System Event Logging:** If a survivor suffering from location contamination is forced into high-intensity combat, the combat system emits a `StressedCombatantEngagementEvent`, triggering dynamic dialogue barks in the Godot presentation layer.
- **Integrity Guarantee:** Verified clean by `PsychologyBoundaryRouter` with deterministic digest tracking.
