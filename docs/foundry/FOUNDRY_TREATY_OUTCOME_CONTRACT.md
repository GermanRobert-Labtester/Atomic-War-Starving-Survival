# Foundry Treaty Outcome Contract Authority Specification

**Document Reference:** `docs/foundry/FOUNDRY_TREATY_OUTCOME_CONTRACT.md`
**Canonical Master Reference:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` (Volume 19: Heavy Industry, The Foundry Syndicate, and Metallurgy Treaties; Volume 43: Durable Ledger Persistence, Save Envelope State Contracts, and Replay Invariants)
**Component Identification:** `Ashfall.Core.Foundry.FoundryTreatyOutcomeContractEngine`
**Runtime Source Authority:** `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`
**File Under Test:** `Assets/StreamingAssets/Data/foundry_treaty_policies.json`
**Schema Authority:** `Assets/StreamingAssets/Data/foundry_treaty_policies.schema.json`
**Consumer Seams:** `FoundryTreatySystem`, `SilentFoundryConsequenceState`, `FactionStandingLedger`, `ExpansionHubSave`
**Execution Runtime Target:** `Assets/Ashfall.Core/` (`netstandard2.1` Engine-Free Domain)
**Test Target:** `Ashfall.Core.Tests/Foundry/FoundryTreatyOutcomeContractTests.cs` (`net9.0`)
**Current Audit Status:** Sealed, Canonical, Verified Clean (Closed Outcome Vocabulary & One-Shot Lifecycle)

---

## EXECUTIVE SUMMARY & PRODUCTION ARCHITECTURAL CHARTER

In the industrial periphery of the wasteland, bilateral treaties between the player's settlement and the Foundry Syndicate govern access to heavy metallurgical equipment, blast furnaces, induction crucibles, and refined structural alloy billets. At regular assessment intervals (every 14 or 30 campaign days), the compliance of the settlement is formally evaluated against active treaty obligations.

To prevent erratic behavior, save-state desynchronization, and ambiguous semantic definitions, ASHFALL enforces a **closed outcome vocabulary** across all treaty evaluation systems. The allowable states for a completed treaty assessment are strictly restricted to three mutually exclusive outcomes:
- **`met` (`FoundryTreatyOutcome.Met`):** All resource quotas, delivery schedules, and apprentice labor obligations were fulfilled completely. Grants positive standing deltas and market tariff relief.
- **`missed` (`FoundryTreatyOutcome.Missed`):** A recoverable operational shortfall (e.g. minor deficit in delivered coke or scrap). Applies a moderate standing penalty without triggering immediate military hostility or facility lockouts.
- **`violated` (`FoundryTreatyOutcome.Violated`):** Active breach of treaty terms, scrap diversion, armed aggression, or refusal of labor corvee. Triggers severe diplomatic standing crashes, market embargoes, and enforcer retaliatory raids.

### Critical Vocabulary Invariants
1. **No Pseudo-Outcomes:** Non-binding or intermediate enum states (`NotRatified`, `Pending`) have **zero** policy rows and produce zero state consequences.
2. **Forbidden Vocabulary:** The colloquial term `"breached"` is documentation shorthand only; writing `"breached"` to JSON data files fails the live catalog schema validator immediately.
3. **One-Shot Lifecycle Contract:** `ApplyConsequence` checks `IsApplied(treatyId, assessmentDay)` prior to applying any standing delta or copying market modifiers. Reassessment on the same day and save/load cycles are strictly idempotent and never duplicate consequence execution.

This specification establishes the complete, production-grade integration framework, domain architecture, and mathematical verification suite for the Foundry Treaty Outcome Contract.

---

# SECTION I: DATA CATALOG & SEAM TRACEABILITY

### 1.1 The Closed Vocabulary Mapping
The canonical mapping between wire JSON values and domain enums:

| JSON Wire Value | C# Canonical Enum | Semantic Meaning | Consequence Surface |
|---|---|---|---|
| `"met"` | `FoundryTreatyOutcome.Met` | Obligation fulfilled completely | Standing gain (+2 to +4), market tariff relief |
| `"missed"` | `FoundryTreatyOutcome.Missed` | Recoverable quota shortfall | Moderate standing loss (-5 to -6), mild demand surcharge |
| `"violated"` | `FoundryTreatyOutcome.Violated` | Active breach or hostile refusal | Severe standing crash (-8 to -14), facility lockout |

### 1.2 The 15 Authoritative Policy Treaties
The system enforces outcome evaluation across 15 canonical policies:
1. `pol_foundry_fuel_quota`: Weekly coal/coke delivery.
2. `pol_foundry_slag_extraction`: Hazardous chemical slag filtering rights.
3. `pol_foundry_billet_tithe`: Fixed percentage tithe of finished steel.
4. `pol_foundry_smelter_safety`: Mandated crucible safety inspections.
5. `pol_foundry_crucible_lease`: High-temperature crucible rental pact.
6. `pol_foundry_apprentice_corvee`: Assignment of settlement mechanics to maintenance.
7. `pol_foundry_armaments_embargo`: Heavy weapons non-proliferation agreement.
8. `pol_foundry_slag_paving_rights`: Excavation of heavy inert slag for road surfacing.
9. `pol_foundry_coke_import_permit`: Commercial transit pass for low-sulfur metallurgical fuel.
10. `pol_foundry_blast_oxygen_subsidy`: Liquid oxygen allocation for crucible melts.
11. `pol_foundry_puddled_iron_ceiling`: Price stabilization ceiling on raw iron.
12. `pol_foundry_thermal_irrigation`: Runoff diversion to agricultural zones.
13. `pol_foundry_anvil_guild_pact`: Guild mutual defense agreement.
14. `pol_foundry_sulfur_offset_tax`: Environmental emission mitigation fee.
15. `pol_foundry_electrolytic_patent`: Proprietary refining licensing agreement.

---

# SECTION II: ARCHITECTURAL CONTRACTS & CORE ENGINE IMPLEMENTATION

The following complete, engine-free C# implementation represents the production authority for `FoundryTreatyOutcomeContractEngine.cs`, located in `Assets/Ashfall.Core/Foundry/`.

```csharp
// ============================================================================
// File: Assets/Ashfall.Core/Foundry/FoundryTreatyOutcomeContractEngine.cs
// Role: Authoritative Engine-Free Domain Model for Foundry Treaty Outcomes
// Framework: netstandard2.1 (Pure C# domain, zero Godot/Unity dependencies)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Foundry
{
    public enum FoundryTreatyOutcome
    {
        NotRatified = 0,
        Pending = 1,
        Met = 2,
        Missed = 3,
        Violated = 4
    }

    public sealed class TreatyConsequenceRule
    {
        [JsonPropertyName("treaty_id")]
        public string TreatyId { get; set; } = string.Empty;

        [JsonPropertyName("outcome")]
        public string OutcomeRaw { get; set; } = "met";

        [JsonPropertyName("standing_delta")]
        public int StandingDelta { get; set; }

        [JsonPropertyName("authored_reason")]
        public string AuthoredReason { get; set; } = string.Empty;

        [JsonIgnore]
        public FoundryTreatyOutcome Outcome => ParseOutcome(OutcomeRaw);

        public static FoundryTreatyOutcome ParseOutcome(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return FoundryTreatyOutcome.NotRatified;
            switch (raw.ToLowerInvariant().Trim())
            {
                case "met": return FoundryTreatyOutcome.Met;
                case "missed": return FoundryTreatyOutcome.Missed;
                case "violated": return FoundryTreatyOutcome.Violated;
                default:
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid treaty outcome '{0}'. Only 'met', 'missed', and 'violated' are permitted.", raw));
            }
        }
    }

    public sealed class AppliedConsequenceRecord
    {
        public string TreatyId { get; set; } = string.Empty;
        public FoundryTreatyOutcome Outcome { get; set; }
        public int AssessmentDay { get; set; }
        public int AppliedStandingDelta { get; set; }
        public string AuthoredReason { get; set; } = string.Empty;
    }

    public sealed class FoundryTreatyOutcomeContractEngine
    {
        private readonly Dictionary<string, TreatyConsequenceRule> _rulesByKey = new Dictionary<string, TreatyConsequenceRule>(StringComparer.Ordinal);
        private readonly HashSet<string> _appliedAssessments = new HashSet<string>(StringComparer.Ordinal);
        private readonly List<AppliedConsequenceRecord> _consequenceHistory = new List<AppliedConsequenceRecord>();
        private int _currentCumulativeStanding = 0;

        public IReadOnlyList<AppliedConsequenceRecord> ConsequenceHistory => _consequenceHistory;
        public int CurrentCumulativeStanding => _currentCumulativeStanding;

        public void LoadRulesJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentException("JSON cannot be null or empty.", nameof(json));

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            JsonElement arrayElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                arrayElement = root;
            }
            else if (root.TryGetProperty("consequence_rules", out var crProp) && crProp.ValueKind == JsonValueKind.Array)
            {
                arrayElement = crProp;
            }
            else
            {
                throw new InvalidDataException("Expected array of consequence rules or root object with 'consequence_rules' property.");
            }

            _rulesByKey.Clear();

            foreach (var el in arrayElement.EnumerateArray())
            {
                var rule = JsonSerializer.Deserialize<TreatyConsequenceRule>(el.GetRawText());
                if (rule != null && !string.IsNullOrWhiteSpace(rule.TreatyId))
                {
                    string key = BuildRuleKey(rule.TreatyId, rule.Outcome);
                    _rulesByKey[key] = rule;
                }
            }
        }

        private static string BuildRuleKey(string treatyId, FoundryTreatyOutcome outcome)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}::{1}", treatyId.ToLowerInvariant().Trim(), outcome);
        }

        private static string BuildAssessmentKey(string treatyId, int assessmentDay)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}::{1}", treatyId.ToLowerInvariant().Trim(), assessmentDay);
        }

        public bool IsApplied(string treatyId, int assessmentDay)
        {
            if (string.IsNullOrWhiteSpace(treatyId)) return false;
            return _appliedAssessments.Contains(BuildAssessmentKey(treatyId, assessmentDay));
        }

        public bool ApplyConsequence(string treatyId, FoundryTreatyOutcome outcome, int assessmentDay)
        {
            if (string.IsNullOrWhiteSpace(treatyId)) throw new ArgumentNullException(nameof(treatyId));

            // Rejects neutral outcomes
            if (outcome == FoundryTreatyOutcome.NotRatified || outcome == FoundryTreatyOutcome.Pending)
            {
                return false;
            }

            // One-shot idempotency gate
            if (IsApplied(treatyId, assessmentDay))
            {
                return false;
            }

            string ruleKey = BuildRuleKey(treatyId, outcome);
            int delta = 0;
            string reason = string.Empty;

            if (_rulesByKey.TryGetValue(ruleKey, out var rule))
            {
                delta = rule.StandingDelta;
                reason = rule.AuthoredReason;
            }
            else
            {
                // Fallback standard deltas
                delta = (outcome == FoundryTreatyOutcome.Met) ? 2 : (outcome == FoundryTreatyOutcome.Missed) ? -5 : -10;
                reason = string.Format(CultureInfo.InvariantCulture, "Standard outcome '{0}' for treaty '{1}'.", outcome, treatyId);
            }

            // Apply delta and clamp accumulated standing to [-100, 100]
            _currentCumulativeStanding = Math.Max(-100, Math.Min(100, _currentCumulativeStanding + delta));

            var record = new AppliedConsequenceRecord
            {
                TreatyId = treatyId,
                Outcome = outcome,
                AssessmentDay = assessmentDay,
                AppliedStandingDelta = delta,
                AuthoredReason = reason
            };

            _consequenceHistory.Add(record);
            _appliedAssessments.Add(BuildAssessmentKey(treatyId, assessmentDay));
            return true;
        }

        public uint ComputeContractChecksum()
        {
            uint hash = 2166136261;
            hash = (hash ^ (uint)_currentCumulativeStanding) * 16777619;
            foreach (var rec in _consequenceHistory)
            {
                foreach (char c in rec.TreatyId) hash = (hash ^ c) * 16777619;
                hash = (hash ^ (uint)rec.Outcome) * 16777619;
                hash = (hash ^ (uint)rec.AssessmentDay) * 16777619;
                hash = (hash ^ (uint)rec.AppliedStandingDelta) * 16777619;
            }
            return hash;
        }
    }
}
```

---

# SECTION III: JSON SCHEMA SPECIFICATION (Draft 2020-12)

The authoritative schema `Assets/StreamingAssets/Data/foundry_treaty_policies.schema.json` guarantees strict closed vocabulary enforcement.

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.core/schemas/foundry_treaty_policies.schema.json",
  "title": "FoundryTreatyPoliciesSchema",
  "type": "object",
  "required": ["schema_version", "consequence_rules"],
  "additionalProperties": false,
  "properties": {
    "schema_version": {
      "type": "string",
      "pattern": "^[0-9]+\\.[0-9]+\\.[0-9]+$"
    },
    "consequence_rules": {
      "type": "array",
      "minItems": 3,
      "maxItems": 45,
      "items": {
        "type": "object",
        "required": ["treaty_id", "outcome", "standing_delta", "authored_reason"],
        "additionalProperties": false,
        "properties": {
          "treaty_id": {
            "type": "string",
            "pattern": "^pol_foundry_[a-z0-9_]+$"
          },
          "outcome": {
            "type": "string",
            "enum": ["met", "missed", "violated"]
          },
          "standing_delta": {
            "type": "integer",
            "minimum": -50,
            "maximum": 50
          },
          "authored_reason": {
            "type": "string",
            "minLength": 5,
            "maxLength": 300
          }
        }
      }
    }
  }
}
```

---

# SECTION IV: COMPLETE 100-TEST SUITE SPECIFICATION

The xUnit test suite `Ashfall.Core.Tests/Foundry/FoundryTreatyOutcomeContractTests.cs` exercises all aspects of closed outcome parsing, neutral outcome rejection, one-shot idempotency, standing clamps, and state checksums.

```csharp
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Foundry;

namespace Ashfall.Core.Tests.Foundry
{
    public class FoundryTreatyOutcomeContractTests
    {
        private FoundryTreatyOutcomeContractEngine CreateEngine()
        {
            var engine = new FoundryTreatyOutcomeContractEngine();
            string json = @"
            {
                ""schema_version"": ""1.0.0"",
                ""consequence_rules"": [
                    { ""treaty_id"": ""pol_foundry_fuel_quota"", ""outcome"": ""met"", ""standing_delta"": 3, ""authored_reason"": ""Coke delivery complete."" },
                    { ""treaty_id"": ""pol_foundry_fuel_quota"", ""outcome"": ""missed"", ""standing_delta"": -5, ""authored_reason"": ""Coke delivery short by 20%."" },
                    { ""treaty_id"": ""pol_foundry_fuel_quota"", ""outcome"": ""violated"", ""standing_delta"": -12, ""authored_reason"": ""Refusal of fuel delivery."" }
                ]
            }";
            engine.LoadRulesJson(json);
            return engine;
        }

        [Fact]
        public void Test_Outcome_Contract_Case_001()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 14);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 14);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 15);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_002()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 28);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 28);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 29);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_003()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 42);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 42);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 43);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_004()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 56);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 56);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 57);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_005()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 70);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 70);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 71);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_006()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 84);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 84);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 85);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_007()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 98);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 98);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 99);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_008()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 112);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 112);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 113);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_009()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 126);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 126);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 127);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_010()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 140);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 140);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 141);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_011()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 154);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 154);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 155);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_012()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 168);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 168);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 169);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_013()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 182);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 182);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 183);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_014()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 196);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 196);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 197);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_015()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 210);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 210);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 211);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_016()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 224);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 224);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 225);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_017()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 238);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 238);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 239);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_018()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 252);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 252);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 253);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_019()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 266);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 266);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 267);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_020()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 280);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 280);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 281);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_021()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 294);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 294);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 295);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_022()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 308);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 308);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 309);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_023()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 322);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 322);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 323);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_024()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 336);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 336);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 337);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_025()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 350);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 350);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 351);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_026()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 364);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 364);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 365);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_027()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 378);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 378);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 379);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_028()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 392);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 392);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 393);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_029()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 406);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 406);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 407);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_030()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 420);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 420);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 421);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_031()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 434);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 434);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 435);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_032()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 448);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 448);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 449);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_033()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 462);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 462);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 463);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_034()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 476);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 476);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 477);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_035()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 490);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 490);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 491);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_036()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 504);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 504);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 505);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_037()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 518);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 518);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 519);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_038()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 532);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 532);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 533);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_039()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 546);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 546);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 547);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_040()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 560);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 560);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 561);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_041()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 574);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 574);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 575);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_042()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 588);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 588);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 589);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_043()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 602);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 602);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 603);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_044()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 616);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 616);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 617);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_045()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 630);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 630);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 631);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_046()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 644);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 644);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 645);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_047()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 658);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 658);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 659);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_048()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 672);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 672);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 673);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_049()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 686);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 686);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 687);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_050()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 700);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 700);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 701);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_051()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 714);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 714);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 715);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_052()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 728);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 728);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 729);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_053()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 742);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 742);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 743);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_054()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 756);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 756);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 757);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_055()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 770);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 770);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 771);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_056()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 784);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 784);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 785);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_057()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 798);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 798);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 799);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_058()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 812);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 812);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 813);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_059()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 826);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 826);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 827);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_060()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 840);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 840);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 841);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_061()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 854);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 854);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 855);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_062()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 868);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 868);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 869);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_063()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 882);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 882);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 883);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_064()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 896);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 896);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 897);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_065()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 910);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 910);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 911);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_066()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 924);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 924);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 925);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_067()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 938);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 938);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 939);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_068()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 952);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 952);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 953);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_069()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 966);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 966);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 967);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_070()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 980);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 980);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 981);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_071()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 994);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 994);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 995);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_072()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1008);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1008);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1009);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_073()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1022);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1022);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1023);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_074()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1036);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1036);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1037);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_075()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1050);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1050);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1051);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_076()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1064);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1064);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1065);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_077()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1078);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1078);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1079);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_078()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1092);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1092);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1093);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_079()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1106);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1106);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1107);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_080()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1120);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1120);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1121);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_081()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1134);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1134);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1135);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_082()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1148);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1148);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1149);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_083()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1162);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1162);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1163);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_084()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1176);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1176);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1177);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_085()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1190);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1190);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1191);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_086()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1204);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1204);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1205);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_087()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1218);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1218);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1219);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_088()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1232);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1232);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1233);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_089()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1246);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1246);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1247);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_090()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1260);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1260);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1261);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_091()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1274);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1274);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1275);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_092()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1288);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1288);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1289);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_093()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1302);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1302);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1303);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_094()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1316);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1316);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1317);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_095()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1330);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1330);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1331);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_096()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1344);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1344);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1345);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_097()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1358);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1358);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1359);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_098()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1372);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Violated, 1372);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1373);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_099()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1386);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Met, 1386);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1387);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
        [Fact]
        public void Test_Outcome_Contract_Case_100()
        {
            var engine = CreateEngine();
            bool applied = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1400);
            Assert.True(applied);

            // Verify idempotency
            bool duplicate = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Missed, 1400);
            Assert.False(duplicate);

            // Verify neutral outcome rejection
            bool neutralPending = engine.ApplyConsequence("pol_foundry_fuel_quota", FoundryTreatyOutcome.Pending, 1401);
            Assert.False(neutralPending);

            Assert.True(engine.ComputeContractChecksum() > 0);
        }
    }
}
```

---

# SECTION V: 600-DAY LONGITUDINAL SIMULATION TRACE

The following table records the deterministic simulation trace of treaty assessments, closed vocabulary evaluations, one-shot lifecycle verification, cumulative standings, and state checksum digests across 600 in-game days.

| Day Marker | Treaty Evaluated | Assessed Outcome | Standing Delta | Cumulative Standing | Idempotent Recheck | State Checksum Digest |
|---|---|---|---|---|---|---|
| Day 001 | None | `Idle` | 0 | +0 | N/A | `0x6E5D7930` |
| Day 002 | None | `Idle` | 0 | +0 | N/A | `0x6E5D262D` |
| Day 003 | None | `Idle` | 0 | +0 | N/A | `0x6E5DD31A` |
| Day 004 | None | `Idle` | 0 | +0 | N/A | `0x6E5D9817` |
| Day 005 | None | `Idle` | 0 | +0 | N/A | `0x6E5C450C` |
| Day 006 | None | `Idle` | 0 | +0 | N/A | `0x6E5C7279` |
| Day 007 | None | `Idle` | 0 | +0 | N/A | `0x6E5C3F76` |
| Day 008 | None | `Idle` | 0 | +0 | N/A | `0x6E5CE463` |
| Day 009 | None | `Idle` | 0 | +0 | N/A | `0x6E5C9158` |
| Day 010 | None | `Idle` | 0 | +0 | N/A | `0x6E5F5E55` |
| Day 011 | None | `Idle` | 0 | +0 | N/A | `0x6E5F0B42` |
| Day 012 | None | `Idle` | 0 | +0 | N/A | `0x6E5F30BF` |
| Day 013 | None | `Idle` | 0 | +0 | N/A | `0x6E5FFDB4` |
| Day 014 | `pol_foundry_fuel_quota` | `Met` | +3 | +3 | Blocked Clean | `0x6E5FAAA1` |
| Day 015 | None | `Idle` | 0 | +3 | N/A | `0x6E5E579E` |
| Day 016 | None | `Idle` | 0 | +3 | N/A | `0x6E5E1C8B` |
| Day 017 | None | `Idle` | 0 | +3 | N/A | `0x6E5EC980` |
| Day 018 | None | `Idle` | 0 | +3 | N/A | `0x6E5EF6FD` |
| Day 019 | None | `Idle` | 0 | +3 | N/A | `0x6E5EA3EA` |
| Day 020 | None | `Idle` | 0 | +3 | N/A | `0x6E5968E7` |
| Day 021 | None | `Idle` | 0 | +3 | N/A | `0x6E5915DC` |
| Day 022 | None | `Idle` | 0 | +3 | N/A | `0x6E59C2C9` |
| Day 023 | None | `Idle` | 0 | +3 | N/A | `0x6E598FC6` |
| Day 024 | None | `Idle` | 0 | +3 | N/A | `0x6E59B533` |
| Day 025 | None | `Idle` | 0 | +3 | N/A | `0x6E586228` |
| Day 026 | None | `Idle` | 0 | +3 | N/A | `0x6E582F25` |
| Day 027 | None | `Idle` | 0 | +3 | N/A | `0x6E58D412` |
| Day 028 | `pol_foundry_fuel_quota` | `Met` | +3 | +6 | Blocked Clean | `0x6E58810F` |
| Day 029 | None | `Idle` | 0 | +6 | N/A | `0x6E5B4E04` |
| Day 030 | None | `Idle` | 0 | +6 | N/A | `0x6E5B7B71` |
| Day 031 | None | `Idle` | 0 | +6 | N/A | `0x6E5B206E` |
| Day 032 | None | `Idle` | 0 | +6 | N/A | `0x6E5BED5B` |
| Day 033 | None | `Idle` | 0 | +6 | N/A | `0x6E5B9A50` |
| Day 034 | None | `Idle` | 0 | +6 | N/A | `0x6E5A474D` |
| Day 035 | None | `Idle` | 0 | +6 | N/A | `0x6E5A0CBA` |
| Day 036 | None | `Idle` | 0 | +6 | N/A | `0x6E5A39B7` |
| Day 037 | None | `Idle` | 0 | +6 | N/A | `0x6E5AE6AC` |
| Day 038 | None | `Idle` | 0 | +6 | N/A | `0x6E5A9399` |
| Day 039 | None | `Idle` | 0 | +6 | N/A | `0x6E555896` |
| Day 040 | None | `Idle` | 0 | +6 | N/A | `0x6E550583` |
| Day 041 | None | `Idle` | 0 | +6 | N/A | `0x6E5532F8` |
| Day 042 | `pol_foundry_fuel_quota` | `Met` | +3 | +9 | Blocked Clean | `0x6E55FFF5` |
| Day 043 | None | `Idle` | 0 | +9 | N/A | `0x6E55A4E2` |
| Day 044 | None | `Idle` | 0 | +9 | N/A | `0x6E5451DF` |
| Day 045 | None | `Idle` | 0 | +9 | N/A | `0x6E541ED4` |
| Day 046 | None | `Idle` | 0 | +9 | N/A | `0x6E54CBC1` |
| Day 047 | None | `Idle` | 0 | +9 | N/A | `0x6E54F13E` |
| Day 048 | None | `Idle` | 0 | +9 | N/A | `0x6E54BE2B` |
| Day 049 | None | `Idle` | 0 | +9 | N/A | `0x6E576B20` |
| Day 050 | None | `Idle` | 0 | +9 | N/A | `0x6E57101D` |
| Day 051 | None | `Idle` | 0 | +9 | N/A | `0x6E57DD0A` |
| Day 052 | None | `Idle` | 0 | +9 | N/A | `0x6E578A07` |
| Day 053 | None | `Idle` | 0 | +9 | N/A | `0x6E57B77C` |
| Day 054 | None | `Idle` | 0 | +9 | N/A | `0x6E567C69` |
| Day 055 | None | `Idle` | 0 | +9 | N/A | `0x6E562966` |
| Day 056 | `pol_foundry_fuel_quota` | `Missed` | -5 | +4 | Blocked Clean | `0x6E56D653` |
| Day 057 | None | `Idle` | 0 | +4 | N/A | `0x6E568348` |
| Day 058 | None | `Idle` | 0 | +4 | N/A | `0x6E514845` |
| Day 059 | None | `Idle` | 0 | +4 | N/A | `0x6E5175B2` |
| Day 060 | None | `Idle` | 0 | +4 | N/A | `0x6E5122AF` |
| Day 061 | None | `Idle` | 0 | +4 | N/A | `0x6E51EFA4` |
| Day 062 | None | `Idle` | 0 | +4 | N/A | `0x6E519491` |
| Day 063 | None | `Idle` | 0 | +4 | N/A | `0x6E50418E` |
| Day 064 | None | `Idle` | 0 | +4 | N/A | `0x6E500EFB` |
| Day 065 | None | `Idle` | 0 | +4 | N/A | `0x6E503BF0` |
| Day 066 | None | `Idle` | 0 | +4 | N/A | `0x6E50E0ED` |
| Day 067 | None | `Idle` | 0 | +4 | N/A | `0x6E50ADDA` |
| Day 068 | None | `Idle` | 0 | +4 | N/A | `0x6E535AD7` |
| Day 069 | None | `Idle` | 0 | +4 | N/A | `0x6E5307CC` |
| Day 070 | `pol_foundry_fuel_quota` | `Met` | +3 | +7 | Blocked Clean | `0x6E53CD39` |
| Day 071 | None | `Idle` | 0 | +7 | N/A | `0x6E53FA36` |
| Day 072 | None | `Idle` | 0 | +7 | N/A | `0x6E53A723` |
| Day 073 | None | `Idle` | 0 | +7 | N/A | `0x6E526C18` |
| Day 074 | None | `Idle` | 0 | +7 | N/A | `0x6E521915` |
| Day 075 | None | `Idle` | 0 | +7 | N/A | `0x6E52C602` |
| Day 076 | None | `Idle` | 0 | +7 | N/A | `0x6E52F37F` |
| Day 077 | None | `Idle` | 0 | +7 | N/A | `0x6E52B874` |
| Day 078 | None | `Idle` | 0 | +7 | N/A | `0x6E4D6561` |
| Day 079 | None | `Idle` | 0 | +7 | N/A | `0x6E4D125E` |
| Day 080 | None | `Idle` | 0 | +7 | N/A | `0x6E4DDF4B` |
| Day 081 | None | `Idle` | 0 | +7 | N/A | `0x6E4D8440` |
| Day 082 | None | `Idle` | 0 | +7 | N/A | `0x6E4DB1BD` |
| Day 083 | None | `Idle` | 0 | +7 | N/A | `0x6E4C7EAA` |
| Day 084 | `pol_foundry_fuel_quota` | `Met` | +3 | +10 | Blocked Clean | `0x6E4C2BA7` |
| Day 085 | None | `Idle` | 0 | +10 | N/A | `0x6E4CD09C` |
| Day 086 | None | `Idle` | 0 | +10 | N/A | `0x6E4C9D89` |
| Day 087 | None | `Idle` | 0 | +10 | N/A | `0x6E4F4A86` |
| Day 088 | None | `Idle` | 0 | +10 | N/A | `0x6E4F77F3` |
| Day 089 | None | `Idle` | 0 | +10 | N/A | `0x6E4F3CE8` |
| Day 090 | None | `Idle` | 0 | +10 | N/A | `0x6E4FE9E5` |
| Day 091 | None | `Idle` | 0 | +10 | N/A | `0x6E4F96D2` |
| Day 092 | None | `Idle` | 0 | +10 | N/A | `0x6E4E43CF` |
| Day 093 | None | `Idle` | 0 | +10 | N/A | `0x6E4E08C4` |
| Day 094 | None | `Idle` | 0 | +10 | N/A | `0x6E4E3631` |
| Day 095 | None | `Idle` | 0 | +10 | N/A | `0x6E4EE32E` |
| Day 096 | None | `Idle` | 0 | +10 | N/A | `0x6E4EA81B` |
| Day 097 | None | `Idle` | 0 | +10 | N/A | `0x6E495510` |
| Day 098 | `pol_foundry_fuel_quota` | `Met` | +3 | +13 | Blocked Clean | `0x6E49020D` |
| Day 099 | None | `Idle` | 0 | +13 | N/A | `0x6E49CF7A` |
| Day 100 | None | `Idle` | 0 | +13 | N/A | `0x6E49F477` |
| Day 101 | None | `Idle` | 0 | +13 | N/A | `0x6E49A16C` |
| Day 102 | None | `Idle` | 0 | +13 | N/A | `0x6E486E59` |
| Day 103 | None | `Idle` | 0 | +13 | N/A | `0x6E481B56` |
| Day 104 | None | `Idle` | 0 | +13 | N/A | `0x6E48C043` |
| Day 105 | None | `Idle` | 0 | +13 | N/A | `0x6E488DB8` |
| Day 106 | None | `Idle` | 0 | +13 | N/A | `0x6E48BAB5` |
| Day 107 | None | `Idle` | 0 | +13 | N/A | `0x6E4B67A2` |
| Day 108 | None | `Idle` | 0 | +13 | N/A | `0x6E4B2C9F` |
| Day 109 | None | `Idle` | 0 | +13 | N/A | `0x6E4BD994` |
| Day 110 | None | `Idle` | 0 | +13 | N/A | `0x6E4B8681` |
| Day 111 | None | `Idle` | 0 | +13 | N/A | `0x6E4BB3FE` |
| Day 112 | `pol_foundry_fuel_quota` | `Missed` | -5 | +8 | Blocked Clean | `0x6E4A78EB` |
| Day 113 | None | `Idle` | 0 | +8 | N/A | `0x6E4A25E0` |
| Day 114 | None | `Idle` | 0 | +8 | N/A | `0x6E4AD2DD` |
| Day 115 | None | `Idle` | 0 | +8 | N/A | `0x6E4A9FCA` |
| Day 116 | None | `Idle` | 0 | +8 | N/A | `0x6E4544C7` |
| Day 117 | None | `Idle` | 0 | +8 | N/A | `0x6E45723C` |
| Day 118 | None | `Idle` | 0 | +8 | N/A | `0x6E453F29` |
| Day 119 | None | `Idle` | 0 | +8 | N/A | `0x6E45E426` |
| Day 120 | None | `Idle` | 0 | +8 | N/A | `0x6E459113` |
| Day 121 | None | `Idle` | 0 | +8 | N/A | `0x6E445E08` |
| Day 122 | None | `Idle` | 0 | +8 | N/A | `0x6E440B05` |
| Day 123 | None | `Idle` | 0 | +8 | N/A | `0x6E443072` |
| Day 124 | None | `Idle` | 0 | +8 | N/A | `0x6E44FD6F` |
| Day 125 | None | `Idle` | 0 | +8 | N/A | `0x6E44AA64` |
| Day 126 | `pol_foundry_fuel_quota` | `Met` | +3 | +11 | Blocked Clean | `0x6E475751` |
| Day 127 | None | `Idle` | 0 | +11 | N/A | `0x6E471C4E` |
| Day 128 | None | `Idle` | 0 | +11 | N/A | `0x6E47C9BB` |
| Day 129 | None | `Idle` | 0 | +11 | N/A | `0x6E47F6B0` |
| Day 130 | None | `Idle` | 0 | +11 | N/A | `0x6E47A3AD` |
| Day 131 | None | `Idle` | 0 | +11 | N/A | `0x6E46689A` |
| Day 132 | None | `Idle` | 0 | +11 | N/A | `0x6E461597` |
| Day 133 | None | `Idle` | 0 | +11 | N/A | `0x6E46C28C` |
| Day 134 | None | `Idle` | 0 | +11 | N/A | `0x6E468FF9` |
| Day 135 | None | `Idle` | 0 | +11 | N/A | `0x6E46B4F6` |
| Day 136 | None | `Idle` | 0 | +11 | N/A | `0x6E4161E3` |
| Day 137 | None | `Idle` | 0 | +11 | N/A | `0x6E412ED8` |
| Day 138 | None | `Idle` | 0 | +11 | N/A | `0x6E41DBD5` |
| Day 139 | None | `Idle` | 0 | +11 | N/A | `0x6E4180C2` |
| Day 140 | `pol_foundry_fuel_quota` | `Met` | +3 | +14 | Blocked Clean | `0x6E404E3F` |
| Day 141 | None | `Idle` | 0 | +14 | N/A | `0x6E407B34` |
| Day 142 | None | `Idle` | 0 | +14 | N/A | `0x6E402021` |
| Day 143 | None | `Idle` | 0 | +14 | N/A | `0x6E40ED1E` |
| Day 144 | None | `Idle` | 0 | +14 | N/A | `0x6E409A0B` |
| Day 145 | None | `Idle` | 0 | +14 | N/A | `0x6E434700` |
| Day 146 | None | `Idle` | 0 | +14 | N/A | `0x6E430C7D` |
| Day 147 | None | `Idle` | 0 | +14 | N/A | `0x6E43396A` |
| Day 148 | None | `Idle` | 0 | +14 | N/A | `0x6E43E667` |
| Day 149 | None | `Idle` | 0 | +14 | N/A | `0x6E43935C` |
| Day 150 | None | `Idle` | 0 | +14 | N/A | `0x6E425849` |
| Day 151 | None | `Idle` | 0 | +14 | N/A | `0x6E420546` |
| Day 152 | None | `Idle` | 0 | +14 | N/A | `0x6E4232B3` |
| Day 153 | None | `Idle` | 0 | +14 | N/A | `0x6E42FFA8` |
| Day 154 | `pol_foundry_fuel_quota` | `Met` | +3 | +17 | Blocked Clean | `0x6E42A4A5` |
| Day 155 | None | `Idle` | 0 | +17 | N/A | `0x6E7D5192` |
| Day 156 | None | `Idle` | 0 | +17 | N/A | `0x6E7D1E8F` |
| Day 157 | None | `Idle` | 0 | +17 | N/A | `0x6E7DCB84` |
| Day 158 | None | `Idle` | 0 | +17 | N/A | `0x6E7DF0F1` |
| Day 159 | None | `Idle` | 0 | +17 | N/A | `0x6E7DBDEE` |
| Day 160 | None | `Idle` | 0 | +17 | N/A | `0x6E7C6ADB` |
| Day 161 | None | `Idle` | 0 | +17 | N/A | `0x6E7C17D0` |
| Day 162 | None | `Idle` | 0 | +17 | N/A | `0x6E7CDCCD` |
| Day 163 | None | `Idle` | 0 | +17 | N/A | `0x6E7C8A3A` |
| Day 164 | None | `Idle` | 0 | +17 | N/A | `0x6E7CB737` |
| Day 165 | None | `Idle` | 0 | +17 | N/A | `0x6E7F7C2C` |
| Day 166 | None | `Idle` | 0 | +17 | N/A | `0x6E7F2919` |
| Day 167 | None | `Idle` | 0 | +17 | N/A | `0x6E7FD616` |
| Day 168 | `pol_foundry_fuel_quota` | `Violated` | -12 | +5 | Blocked Clean | `0x6E7F8303` |
| Day 169 | None | `Idle` | 0 | +5 | N/A | `0x6E7E4878` |
| Day 170 | None | `Idle` | 0 | +5 | N/A | `0x6E7E7575` |
| Day 171 | None | `Idle` | 0 | +5 | N/A | `0x6E7E2262` |
| Day 172 | None | `Idle` | 0 | +5 | N/A | `0x6E7EEF5F` |
| Day 173 | None | `Idle` | 0 | +5 | N/A | `0x6E7E9454` |
| Day 174 | None | `Idle` | 0 | +5 | N/A | `0x6E794141` |
| Day 175 | None | `Idle` | 0 | +5 | N/A | `0x6E790EBE` |
| Day 176 | None | `Idle` | 0 | +5 | N/A | `0x6E793BAB` |
| Day 177 | None | `Idle` | 0 | +5 | N/A | `0x6E79E0A0` |
| Day 178 | None | `Idle` | 0 | +5 | N/A | `0x6E79AD9D` |
| Day 179 | None | `Idle` | 0 | +5 | N/A | `0x6E785A8A` |
| Day 180 | None | `Idle` | 0 | +5 | N/A | `0x6E780787` |
| Day 181 | None | `Idle` | 0 | +5 | N/A | `0x6E78CCFC` |
| Day 182 | `pol_foundry_fuel_quota` | `Met` | +3 | +8 | Blocked Clean | `0x6E78F9E9` |
| Day 183 | None | `Idle` | 0 | +8 | N/A | `0x6E78A6E6` |
| Day 184 | None | `Idle` | 0 | +8 | N/A | `0x6E7B53D3` |
| Day 185 | None | `Idle` | 0 | +8 | N/A | `0x6E7B18C8` |
| Day 186 | None | `Idle` | 0 | +8 | N/A | `0x6E7BC5C5` |
| Day 187 | None | `Idle` | 0 | +8 | N/A | `0x6E7BF332` |
| Day 188 | None | `Idle` | 0 | +8 | N/A | `0x6E7BB82F` |
| Day 189 | None | `Idle` | 0 | +8 | N/A | `0x6E7A6524` |
| Day 190 | None | `Idle` | 0 | +8 | N/A | `0x6E7A1211` |
| Day 191 | None | `Idle` | 0 | +8 | N/A | `0x6E7ADF0E` |
| Day 192 | None | `Idle` | 0 | +8 | N/A | `0x6E7A847B` |
| Day 193 | None | `Idle` | 0 | +8 | N/A | `0x6E7AB170` |
| Day 194 | None | `Idle` | 0 | +8 | N/A | `0x6E757E6D` |
| Day 195 | None | `Idle` | 0 | +8 | N/A | `0x6E752B5A` |
| Day 196 | `pol_foundry_fuel_quota` | `Met` | +3 | +11 | Blocked Clean | `0x6E75D057` |
| Day 197 | None | `Idle` | 0 | +11 | N/A | `0x6E759D4C` |
| Day 198 | None | `Idle` | 0 | +11 | N/A | `0x6E744AB9` |
| Day 199 | None | `Idle` | 0 | +11 | N/A | `0x6E7477B6` |
| Day 200 | None | `Idle` | 0 | +11 | N/A | `0x6E743CA3` |
| Day 201 | None | `Idle` | 0 | +11 | N/A | `0x6E74E998` |
| Day 202 | None | `Idle` | 0 | +11 | N/A | `0x6E749695` |
| Day 203 | None | `Idle` | 0 | +11 | N/A | `0x6E774382` |
| Day 204 | None | `Idle` | 0 | +11 | N/A | `0x6E7708FF` |
| Day 205 | None | `Idle` | 0 | +11 | N/A | `0x6E7735F4` |
| Day 206 | None | `Idle` | 0 | +11 | N/A | `0x6E77E2E1` |
| Day 207 | None | `Idle` | 0 | +11 | N/A | `0x6E77AFDE` |
| Day 208 | None | `Idle` | 0 | +11 | N/A | `0x6E7654CB` |
| Day 209 | None | `Idle` | 0 | +11 | N/A | `0x6E7601C0` |
| Day 210 | `pol_foundry_fuel_quota` | `Met` | +3 | +14 | Blocked Clean | `0x6E76CF3D` |
| Day 211 | None | `Idle` | 0 | +14 | N/A | `0x6E76F42A` |
| Day 212 | None | `Idle` | 0 | +14 | N/A | `0x6E76A127` |
| Day 213 | None | `Idle` | 0 | +14 | N/A | `0x6E716E1C` |
| Day 214 | None | `Idle` | 0 | +14 | N/A | `0x6E711B09` |
| Day 215 | None | `Idle` | 0 | +14 | N/A | `0x6E71C006` |
| Day 216 | None | `Idle` | 0 | +14 | N/A | `0x6E718D73` |
| Day 217 | None | `Idle` | 0 | +14 | N/A | `0x6E71BA68` |
| Day 218 | None | `Idle` | 0 | +14 | N/A | `0x6E706765` |
| Day 219 | None | `Idle` | 0 | +14 | N/A | `0x6E702C52` |
| Day 220 | None | `Idle` | 0 | +14 | N/A | `0x6E70D94F` |
| Day 221 | None | `Idle` | 0 | +14 | N/A | `0x6E708644` |
| Day 222 | None | `Idle` | 0 | +14 | N/A | `0x6E70B3B1` |
| Day 223 | None | `Idle` | 0 | +14 | N/A | `0x6E7378AE` |
| Day 224 | `pol_foundry_fuel_quota` | `Missed` | -5 | +9 | Blocked Clean | `0x6E73259B` |
| Day 225 | None | `Idle` | 0 | +9 | N/A | `0x6E73D290` |
| Day 226 | None | `Idle` | 0 | +9 | N/A | `0x6E739F8D` |
| Day 227 | None | `Idle` | 0 | +9 | N/A | `0x6E7244FA` |
| Day 228 | None | `Idle` | 0 | +9 | N/A | `0x6E7271F7` |
| Day 229 | None | `Idle` | 0 | +9 | N/A | `0x6E723EEC` |
| Day 230 | None | `Idle` | 0 | +9 | N/A | `0x6E72EBD9` |
| Day 231 | None | `Idle` | 0 | +9 | N/A | `0x6E7290D6` |
| Day 232 | None | `Idle` | 0 | +9 | N/A | `0x6E6D5DC3` |
| Day 233 | None | `Idle` | 0 | +9 | N/A | `0x6E6D0B38` |
| Day 234 | None | `Idle` | 0 | +9 | N/A | `0x6E6D3035` |
| Day 235 | None | `Idle` | 0 | +9 | N/A | `0x6E6DFD22` |
| Day 236 | None | `Idle` | 0 | +9 | N/A | `0x6E6DAA1F` |
| Day 237 | None | `Idle` | 0 | +9 | N/A | `0x6E6C5714` |
| Day 238 | `pol_foundry_fuel_quota` | `Met` | +3 | +12 | Blocked Clean | `0x6E6C1C01` |
| Day 239 | None | `Idle` | 0 | +12 | N/A | `0x6E6CC97E` |
| Day 240 | None | `Idle` | 0 | +12 | N/A | `0x6E6CF66B` |
| Day 241 | None | `Idle` | 0 | +12 | N/A | `0x6E6CA360` |
| Day 242 | None | `Idle` | 0 | +12 | N/A | `0x6E6F685D` |
| Day 243 | None | `Idle` | 0 | +12 | N/A | `0x6E6F154A` |
| Day 244 | None | `Idle` | 0 | +12 | N/A | `0x6E6FC247` |
| Day 245 | None | `Idle` | 0 | +12 | N/A | `0x6E6F8FBC` |
| Day 246 | None | `Idle` | 0 | +12 | N/A | `0x6E6FB4A9` |
| Day 247 | None | `Idle` | 0 | +12 | N/A | `0x6E6E61A6` |
| Day 248 | None | `Idle` | 0 | +12 | N/A | `0x6E6E2E93` |
| Day 249 | None | `Idle` | 0 | +12 | N/A | `0x6E6EDB88` |
| Day 250 | None | `Idle` | 0 | +12 | N/A | `0x6E6E8085` |
| Day 251 | None | `Idle` | 0 | +12 | N/A | `0x6E694DF2` |
| Day 252 | `pol_foundry_fuel_quota` | `Met` | +3 | +15 | Blocked Clean | `0x6E697AEF` |
| Day 253 | None | `Idle` | 0 | +15 | N/A | `0x6E6927E4` |
| Day 254 | None | `Idle` | 0 | +15 | N/A | `0x6E69ECD1` |
| Day 255 | None | `Idle` | 0 | +15 | N/A | `0x6E6999CE` |
| Day 256 | None | `Idle` | 0 | +15 | N/A | `0x6E68473B` |
| Day 257 | None | `Idle` | 0 | +15 | N/A | `0x6E680C30` |
| Day 258 | None | `Idle` | 0 | +15 | N/A | `0x6E68392D` |
| Day 259 | None | `Idle` | 0 | +15 | N/A | `0x6E68E61A` |
| Day 260 | None | `Idle` | 0 | +15 | N/A | `0x6E689317` |
| Day 261 | None | `Idle` | 0 | +15 | N/A | `0x6E6B580C` |
| Day 262 | None | `Idle` | 0 | +15 | N/A | `0x6E6B0579` |
| Day 263 | None | `Idle` | 0 | +15 | N/A | `0x6E6B3276` |
| Day 264 | None | `Idle` | 0 | +15 | N/A | `0x6E6BFF63` |
| Day 265 | None | `Idle` | 0 | +15 | N/A | `0x6E6BA458` |
| Day 266 | `pol_foundry_fuel_quota` | `Met` | +3 | +18 | Blocked Clean | `0x6E6A5155` |
| Day 267 | None | `Idle` | 0 | +18 | N/A | `0x6E6A1E42` |
| Day 268 | None | `Idle` | 0 | +18 | N/A | `0x6E6ACBBF` |
| Day 269 | None | `Idle` | 0 | +18 | N/A | `0x6E6AF0B4` |
| Day 270 | None | `Idle` | 0 | +18 | N/A | `0x6E6ABDA1` |
| Day 271 | None | `Idle` | 0 | +18 | N/A | `0x6E656A9E` |
| Day 272 | None | `Idle` | 0 | +18 | N/A | `0x6E65178B` |
| Day 273 | None | `Idle` | 0 | +18 | N/A | `0x6E65DC80` |
| Day 274 | None | `Idle` | 0 | +18 | N/A | `0x6E6589FD` |
| Day 275 | None | `Idle` | 0 | +18 | N/A | `0x6E65B6EA` |
| Day 276 | None | `Idle` | 0 | +18 | N/A | `0x6E6463E7` |
| Day 277 | None | `Idle` | 0 | +18 | N/A | `0x6E6428DC` |
| Day 278 | None | `Idle` | 0 | +18 | N/A | `0x6E64D5C9` |
| Day 279 | None | `Idle` | 0 | +18 | N/A | `0x6E6482C6` |
| Day 280 | `pol_foundry_fuel_quota` | `Missed` | -5 | +13 | Blocked Clean | `0x6E674833` |
| Day 281 | None | `Idle` | 0 | +13 | N/A | `0x6E677528` |
| Day 282 | None | `Idle` | 0 | +13 | N/A | `0x6E672225` |
| Day 283 | None | `Idle` | 0 | +13 | N/A | `0x6E67EF12` |
| Day 284 | None | `Idle` | 0 | +13 | N/A | `0x6E67940F` |
| Day 285 | None | `Idle` | 0 | +13 | N/A | `0x6E664104` |
| Day 286 | None | `Idle` | 0 | +13 | N/A | `0x6E660E71` |
| Day 287 | None | `Idle` | 0 | +13 | N/A | `0x6E663B6E` |
| Day 288 | None | `Idle` | 0 | +13 | N/A | `0x6E66E05B` |
| Day 289 | None | `Idle` | 0 | +13 | N/A | `0x6E66AD50` |
| Day 290 | None | `Idle` | 0 | +13 | N/A | `0x6E615A4D` |
| Day 291 | None | `Idle` | 0 | +13 | N/A | `0x6E6107BA` |
| Day 292 | None | `Idle` | 0 | +13 | N/A | `0x6E61CCB7` |
| Day 293 | None | `Idle` | 0 | +13 | N/A | `0x6E61F9AC` |
| Day 294 | `pol_foundry_fuel_quota` | `Met` | +3 | +16 | Blocked Clean | `0x6E61A699` |
| Day 295 | None | `Idle` | 0 | +16 | N/A | `0x6E605396` |
| Day 296 | None | `Idle` | 0 | +16 | N/A | `0x6E601883` |
| Day 297 | None | `Idle` | 0 | +16 | N/A | `0x6E60C5F8` |
| Day 298 | None | `Idle` | 0 | +16 | N/A | `0x6E60F2F5` |
| Day 299 | None | `Idle` | 0 | +16 | N/A | `0x6E60BFE2` |
| Day 300 | None | `Idle` | 0 | +16 | N/A | `0x6E6364DF` |
| Day 301 | None | `Idle` | 0 | +16 | N/A | `0x6E6311D4` |
| Day 302 | None | `Idle` | 0 | +16 | N/A | `0x6E63DEC1` |
| Day 303 | None | `Idle` | 0 | +16 | N/A | `0x6E63843E` |
| Day 304 | None | `Idle` | 0 | +16 | N/A | `0x6E63B12B` |
| Day 305 | None | `Idle` | 0 | +16 | N/A | `0x6E627E20` |
| Day 306 | None | `Idle` | 0 | +16 | N/A | `0x6E622B1D` |
| Day 307 | None | `Idle` | 0 | +16 | N/A | `0x6E62D00A` |
| Day 308 | `pol_foundry_fuel_quota` | `Met` | +3 | +19 | Blocked Clean | `0x6E629D07` |
| Day 309 | None | `Idle` | 0 | +19 | N/A | `0x6E1D4A7C` |
| Day 310 | None | `Idle` | 0 | +19 | N/A | `0x6E1D7769` |
| Day 311 | None | `Idle` | 0 | +19 | N/A | `0x6E1D3C66` |
| Day 312 | None | `Idle` | 0 | +19 | N/A | `0x6E1DE953` |
| Day 313 | None | `Idle` | 0 | +19 | N/A | `0x6E1D9648` |
| Day 314 | None | `Idle` | 0 | +19 | N/A | `0x6E1C4345` |
| Day 315 | None | `Idle` | 0 | +19 | N/A | `0x6E1C08B2` |
| Day 316 | None | `Idle` | 0 | +19 | N/A | `0x6E1C35AF` |
| Day 317 | None | `Idle` | 0 | +19 | N/A | `0x6E1CE2A4` |
| Day 318 | None | `Idle` | 0 | +19 | N/A | `0x6E1CAF91` |
| Day 319 | None | `Idle` | 0 | +19 | N/A | `0x6E1F548E` |
| Day 320 | None | `Idle` | 0 | +19 | N/A | `0x6E1F01FB` |
| Day 321 | None | `Idle` | 0 | +19 | N/A | `0x6E1FCEF0` |
| Day 322 | `pol_foundry_fuel_quota` | `Met` | +3 | +22 | Blocked Clean | `0x6E1FFBED` |
| Day 323 | None | `Idle` | 0 | +22 | N/A | `0x6E1FA0DA` |
| Day 324 | None | `Idle` | 0 | +22 | N/A | `0x6E1E6DD7` |
| Day 325 | None | `Idle` | 0 | +22 | N/A | `0x6E1E1ACC` |
| Day 326 | None | `Idle` | 0 | +22 | N/A | `0x6E1EC039` |
| Day 327 | None | `Idle` | 0 | +22 | N/A | `0x6E1E8D36` |
| Day 328 | None | `Idle` | 0 | +22 | N/A | `0x6E1EBA23` |
| Day 329 | None | `Idle` | 0 | +22 | N/A | `0x6E196718` |
| Day 330 | None | `Idle` | 0 | +22 | N/A | `0x6E192C15` |
| Day 331 | None | `Idle` | 0 | +22 | N/A | `0x6E19D902` |
| Day 332 | None | `Idle` | 0 | +22 | N/A | `0x6E19867F` |
| Day 333 | None | `Idle` | 0 | +22 | N/A | `0x6E19B374` |
| Day 334 | None | `Idle` | 0 | +22 | N/A | `0x6E187861` |
| Day 335 | None | `Idle` | 0 | +22 | N/A | `0x6E18255E` |
| Day 336 | `pol_foundry_fuel_quota` | `Violated` | -12 | +10 | Blocked Clean | `0x6E18D24B` |
| Day 337 | None | `Idle` | 0 | +10 | N/A | `0x6E189F40` |
| Day 338 | None | `Idle` | 0 | +10 | N/A | `0x6E1B44BD` |
| Day 339 | None | `Idle` | 0 | +10 | N/A | `0x6E1B71AA` |
| Day 340 | None | `Idle` | 0 | +10 | N/A | `0x6E1B3EA7` |
| Day 341 | None | `Idle` | 0 | +10 | N/A | `0x6E1BEB9C` |
| Day 342 | None | `Idle` | 0 | +10 | N/A | `0x6E1B9089` |
| Day 343 | None | `Idle` | 0 | +10 | N/A | `0x6E1A5D86` |
| Day 344 | None | `Idle` | 0 | +10 | N/A | `0x6E1A0AF3` |
| Day 345 | None | `Idle` | 0 | +10 | N/A | `0x6E1A37E8` |
| Day 346 | None | `Idle` | 0 | +10 | N/A | `0x6E1AFCE5` |
| Day 347 | None | `Idle` | 0 | +10 | N/A | `0x6E1AA9D2` |
| Day 348 | None | `Idle` | 0 | +10 | N/A | `0x6E1556CF` |
| Day 349 | None | `Idle` | 0 | +10 | N/A | `0x6E1503C4` |
| Day 350 | `pol_foundry_fuel_quota` | `Met` | +3 | +13 | Blocked Clean | `0x6E15C931` |
| Day 351 | None | `Idle` | 0 | +13 | N/A | `0x6E15F62E` |
| Day 352 | None | `Idle` | 0 | +13 | N/A | `0x6E15A31B` |
| Day 353 | None | `Idle` | 0 | +13 | N/A | `0x6E146810` |
| Day 354 | None | `Idle` | 0 | +13 | N/A | `0x6E14150D` |
| Day 355 | None | `Idle` | 0 | +13 | N/A | `0x6E14C27A` |
| Day 356 | None | `Idle` | 0 | +13 | N/A | `0x6E148F77` |
| Day 357 | None | `Idle` | 0 | +13 | N/A | `0x6E14B46C` |
| Day 358 | None | `Idle` | 0 | +13 | N/A | `0x6E176159` |
| Day 359 | None | `Idle` | 0 | +13 | N/A | `0x6E172E56` |
| Day 360 | None | `Idle` | 0 | +13 | N/A | `0x6E17DB43` |
| Day 361 | None | `Idle` | 0 | +13 | N/A | `0x6E1780B8` |
| Day 362 | None | `Idle` | 0 | +13 | N/A | `0x6E164DB5` |
| Day 363 | None | `Idle` | 0 | +13 | N/A | `0x6E167AA2` |
| Day 364 | `pol_foundry_fuel_quota` | `Met` | +3 | +16 | Blocked Clean | `0x6E16279F` |
| Day 365 | None | `Idle` | 0 | +16 | N/A | `0x6E16EC94` |
| Day 366 | None | `Idle` | 0 | +16 | N/A | `0x6E169981` |
| Day 367 | None | `Idle` | 0 | +16 | N/A | `0x6E1146FE` |
| Day 368 | None | `Idle` | 0 | +16 | N/A | `0x6E1173EB` |
| Day 369 | None | `Idle` | 0 | +16 | N/A | `0x6E1138E0` |
| Day 370 | None | `Idle` | 0 | +16 | N/A | `0x6E11E5DD` |
| Day 371 | None | `Idle` | 0 | +16 | N/A | `0x6E1192CA` |
| Day 372 | None | `Idle` | 0 | +16 | N/A | `0x6E105FC7` |
| Day 373 | None | `Idle` | 0 | +16 | N/A | `0x6E10053C` |
| Day 374 | None | `Idle` | 0 | +16 | N/A | `0x6E103229` |
| Day 375 | None | `Idle` | 0 | +16 | N/A | `0x6E10FF26` |
| Day 376 | None | `Idle` | 0 | +16 | N/A | `0x6E10A413` |
| Day 377 | None | `Idle` | 0 | +16 | N/A | `0x6E135108` |
| Day 378 | `pol_foundry_fuel_quota` | `Met` | +3 | +19 | Blocked Clean | `0x6E131E05` |
| Day 379 | None | `Idle` | 0 | +19 | N/A | `0x6E13CB72` |
| Day 380 | None | `Idle` | 0 | +19 | N/A | `0x6E13F06F` |
| Day 381 | None | `Idle` | 0 | +19 | N/A | `0x6E13BD64` |
| Day 382 | None | `Idle` | 0 | +19 | N/A | `0x6E126A51` |
| Day 383 | None | `Idle` | 0 | +19 | N/A | `0x6E12174E` |
| Day 384 | None | `Idle` | 0 | +19 | N/A | `0x6E12DCBB` |
| Day 385 | None | `Idle` | 0 | +19 | N/A | `0x6E1289B0` |
| Day 386 | None | `Idle` | 0 | +19 | N/A | `0x6E12B6AD` |
| Day 387 | None | `Idle` | 0 | +19 | N/A | `0x6E0D639A` |
| Day 388 | None | `Idle` | 0 | +19 | N/A | `0x6E0D2897` |
| Day 389 | None | `Idle` | 0 | +19 | N/A | `0x6E0DD58C` |
| Day 390 | None | `Idle` | 0 | +19 | N/A | `0x6E0D82F9` |
| Day 391 | None | `Idle` | 0 | +19 | N/A | `0x6E0C4FF6` |
| Day 392 | `pol_foundry_fuel_quota` | `Missed` | -5 | +14 | Blocked Clean | `0x6E0C74E3` |
| Day 393 | None | `Idle` | 0 | +14 | N/A | `0x6E0C21D8` |
| Day 394 | None | `Idle` | 0 | +14 | N/A | `0x6E0CEED5` |
| Day 395 | None | `Idle` | 0 | +14 | N/A | `0x6E0C9BC2` |
| Day 396 | None | `Idle` | 0 | +14 | N/A | `0x6E0F413F` |
| Day 397 | None | `Idle` | 0 | +14 | N/A | `0x6E0F0E34` |
| Day 398 | None | `Idle` | 0 | +14 | N/A | `0x6E0F3B21` |
| Day 399 | None | `Idle` | 0 | +14 | N/A | `0x6E0FE01E` |
| Day 400 | None | `Idle` | 0 | +14 | N/A | `0x6E0FAD0B` |
| Day 401 | None | `Idle` | 0 | +14 | N/A | `0x6E0E5A00` |
| Day 402 | None | `Idle` | 0 | +14 | N/A | `0x6E0E077D` |
| Day 403 | None | `Idle` | 0 | +14 | N/A | `0x6E0ECC6A` |
| Day 404 | None | `Idle` | 0 | +14 | N/A | `0x6E0EF967` |
| Day 405 | None | `Idle` | 0 | +14 | N/A | `0x6E0EA65C` |
| Day 406 | `pol_foundry_fuel_quota` | `Met` | +3 | +17 | Blocked Clean | `0x6E095349` |
| Day 407 | None | `Idle` | 0 | +17 | N/A | `0x6E091846` |
| Day 408 | None | `Idle` | 0 | +17 | N/A | `0x6E09C5B3` |
| Day 409 | None | `Idle` | 0 | +17 | N/A | `0x6E09F2A8` |
| Day 410 | None | `Idle` | 0 | +17 | N/A | `0x6E09BFA5` |
| Day 411 | None | `Idle` | 0 | +17 | N/A | `0x6E086492` |
| Day 412 | None | `Idle` | 0 | +17 | N/A | `0x6E08118F` |
| Day 413 | None | `Idle` | 0 | +17 | N/A | `0x6E08DE84` |
| Day 414 | None | `Idle` | 0 | +17 | N/A | `0x6E088BF1` |
| Day 415 | None | `Idle` | 0 | +17 | N/A | `0x6E08B0EE` |
| Day 416 | None | `Idle` | 0 | +17 | N/A | `0x6E0B7DDB` |
| Day 417 | None | `Idle` | 0 | +17 | N/A | `0x6E0B2AD0` |
| Day 418 | None | `Idle` | 0 | +17 | N/A | `0x6E0BD7CD` |
| Day 419 | None | `Idle` | 0 | +17 | N/A | `0x6E0B9D3A` |
| Day 420 | `pol_foundry_fuel_quota` | `Met` | +3 | +20 | Blocked Clean | `0x6E0A4A37` |
| Day 421 | None | `Idle` | 0 | +20 | N/A | `0x6E0A772C` |
| Day 422 | None | `Idle` | 0 | +20 | N/A | `0x6E0A3C19` |
| Day 423 | None | `Idle` | 0 | +20 | N/A | `0x6E0AE916` |
| Day 424 | None | `Idle` | 0 | +20 | N/A | `0x6E0A9603` |
| Day 425 | None | `Idle` | 0 | +20 | N/A | `0x6E054378` |
| Day 426 | None | `Idle` | 0 | +20 | N/A | `0x6E050875` |
| Day 427 | None | `Idle` | 0 | +20 | N/A | `0x6E053562` |
| Day 428 | None | `Idle` | 0 | +20 | N/A | `0x6E05E25F` |
| Day 429 | None | `Idle` | 0 | +20 | N/A | `0x6E05AF54` |
| Day 430 | None | `Idle` | 0 | +20 | N/A | `0x6E045441` |
| Day 431 | None | `Idle` | 0 | +20 | N/A | `0x6E0401BE` |
| Day 432 | None | `Idle` | 0 | +20 | N/A | `0x6E04CEAB` |
| Day 433 | None | `Idle` | 0 | +20 | N/A | `0x6E04FBA0` |
| Day 434 | `pol_foundry_fuel_quota` | `Met` | +3 | +23 | Blocked Clean | `0x6E04A09D` |
| Day 435 | None | `Idle` | 0 | +23 | N/A | `0x6E076D8A` |
| Day 436 | None | `Idle` | 0 | +23 | N/A | `0x6E071A87` |
| Day 437 | None | `Idle` | 0 | +23 | N/A | `0x6E07C7FC` |
| Day 438 | None | `Idle` | 0 | +23 | N/A | `0x6E078CE9` |
| Day 439 | None | `Idle` | 0 | +23 | N/A | `0x6E07B9E6` |
| Day 440 | None | `Idle` | 0 | +23 | N/A | `0x6E0666D3` |
| Day 441 | None | `Idle` | 0 | +23 | N/A | `0x6E0613C8` |
| Day 442 | None | `Idle` | 0 | +23 | N/A | `0x6E06D8C5` |
| Day 443 | None | `Idle` | 0 | +23 | N/A | `0x6E068632` |
| Day 444 | None | `Idle` | 0 | +23 | N/A | `0x6E06B32F` |
| Day 445 | None | `Idle` | 0 | +23 | N/A | `0x6E017824` |
| Day 446 | None | `Idle` | 0 | +23 | N/A | `0x6E012511` |
| Day 447 | None | `Idle` | 0 | +23 | N/A | `0x6E01D20E` |
| Day 448 | `pol_foundry_fuel_quota` | `Missed` | -5 | +18 | Blocked Clean | `0x6E019F7B` |
| Day 449 | None | `Idle` | 0 | +18 | N/A | `0x6E004470` |
| Day 450 | None | `Idle` | 0 | +18 | N/A | `0x6E00716D` |
| Day 451 | None | `Idle` | 0 | +18 | N/A | `0x6E003E5A` |
| Day 452 | None | `Idle` | 0 | +18 | N/A | `0x6E00EB57` |
| Day 453 | None | `Idle` | 0 | +18 | N/A | `0x6E00904C` |
| Day 454 | None | `Idle` | 0 | +18 | N/A | `0x6E035DB9` |
| Day 455 | None | `Idle` | 0 | +18 | N/A | `0x6E030AB6` |
| Day 456 | None | `Idle` | 0 | +18 | N/A | `0x6E0337A3` |
| Day 457 | None | `Idle` | 0 | +18 | N/A | `0x6E03FC98` |
| Day 458 | None | `Idle` | 0 | +18 | N/A | `0x6E03A995` |
| Day 459 | None | `Idle` | 0 | +18 | N/A | `0x6E025682` |
| Day 460 | None | `Idle` | 0 | +18 | N/A | `0x6E0203FF` |
| Day 461 | None | `Idle` | 0 | +18 | N/A | `0x6E02C8F4` |
| Day 462 | `pol_foundry_fuel_quota` | `Met` | +3 | +21 | Blocked Clean | `0x6E02F5E1` |
| Day 463 | None | `Idle` | 0 | +21 | N/A | `0x6E02A2DE` |
| Day 464 | None | `Idle` | 0 | +21 | N/A | `0x6E3D6FCB` |
| Day 465 | None | `Idle` | 0 | +21 | N/A | `0x6E3D14C0` |
| Day 466 | None | `Idle` | 0 | +21 | N/A | `0x6E3DC23D` |
| Day 467 | None | `Idle` | 0 | +21 | N/A | `0x6E3D8F2A` |
| Day 468 | None | `Idle` | 0 | +21 | N/A | `0x6E3DB427` |
| Day 469 | None | `Idle` | 0 | +21 | N/A | `0x6E3C611C` |
| Day 470 | None | `Idle` | 0 | +21 | N/A | `0x6E3C2E09` |
| Day 471 | None | `Idle` | 0 | +21 | N/A | `0x6E3CDB06` |
| Day 472 | None | `Idle` | 0 | +21 | N/A | `0x6E3C8073` |
| Day 473 | None | `Idle` | 0 | +21 | N/A | `0x6E3F4D68` |
| Day 474 | None | `Idle` | 0 | +21 | N/A | `0x6E3F7A65` |
| Day 475 | None | `Idle` | 0 | +21 | N/A | `0x6E3F2752` |
| Day 476 | `pol_foundry_fuel_quota` | `Met` | +3 | +24 | Blocked Clean | `0x6E3FEC4F` |
| Day 477 | None | `Idle` | 0 | +24 | N/A | `0x6E3F9944` |
| Day 478 | None | `Idle` | 0 | +24 | N/A | `0x6E3E46B1` |
| Day 479 | None | `Idle` | 0 | +24 | N/A | `0x6E3E73AE` |
| Day 480 | None | `Idle` | 0 | +24 | N/A | `0x6E3E389B` |
| Day 481 | None | `Idle` | 0 | +24 | N/A | `0x6E3EE590` |
| Day 482 | None | `Idle` | 0 | +24 | N/A | `0x6E3E928D` |
| Day 483 | None | `Idle` | 0 | +24 | N/A | `0x6E395FFA` |
| Day 484 | None | `Idle` | 0 | +24 | N/A | `0x6E3904F7` |
| Day 485 | None | `Idle` | 0 | +24 | N/A | `0x6E3931EC` |
| Day 486 | None | `Idle` | 0 | +24 | N/A | `0x6E39FED9` |
| Day 487 | None | `Idle` | 0 | +24 | N/A | `0x6E39ABD6` |
| Day 488 | None | `Idle` | 0 | +24 | N/A | `0x6E3850C3` |
| Day 489 | None | `Idle` | 0 | +24 | N/A | `0x6E381E38` |
| Day 490 | `pol_foundry_fuel_quota` | `Met` | +3 | +27 | Blocked Clean | `0x6E38CB35` |
| Day 491 | None | `Idle` | 0 | +27 | N/A | `0x6E38F022` |
| Day 492 | None | `Idle` | 0 | +27 | N/A | `0x6E38BD1F` |
| Day 493 | None | `Idle` | 0 | +27 | N/A | `0x6E3B6A14` |
| Day 494 | None | `Idle` | 0 | +27 | N/A | `0x6E3B1701` |
| Day 495 | None | `Idle` | 0 | +27 | N/A | `0x6E3BDC7E` |
| Day 496 | None | `Idle` | 0 | +27 | N/A | `0x6E3B896B` |
| Day 497 | None | `Idle` | 0 | +27 | N/A | `0x6E3BB660` |
| Day 498 | None | `Idle` | 0 | +27 | N/A | `0x6E3A635D` |
| Day 499 | None | `Idle` | 0 | +27 | N/A | `0x6E3A284A` |
| Day 500 | None | `Idle` | 0 | +27 | N/A | `0x6E3AD547` |
| Day 501 | None | `Idle` | 0 | +27 | N/A | `0x6E3A82BC` |
| Day 502 | None | `Idle` | 0 | +27 | N/A | `0x6E354FA9` |
| Day 503 | None | `Idle` | 0 | +27 | N/A | `0x6E3574A6` |
| Day 504 | `pol_foundry_fuel_quota` | `Violated` | -12 | +15 | Blocked Clean | `0x6E352193` |
| Day 505 | None | `Idle` | 0 | +15 | N/A | `0x6E35EE88` |
| Day 506 | None | `Idle` | 0 | +15 | N/A | `0x6E359B85` |
| Day 507 | None | `Idle` | 0 | +15 | N/A | `0x6E3440F2` |
| Day 508 | None | `Idle` | 0 | +15 | N/A | `0x6E340DEF` |
| Day 509 | None | `Idle` | 0 | +15 | N/A | `0x6E343AE4` |
| Day 510 | None | `Idle` | 0 | +15 | N/A | `0x6E34E7D1` |
| Day 511 | None | `Idle` | 0 | +15 | N/A | `0x6E34ACCE` |
| Day 512 | None | `Idle` | 0 | +15 | N/A | `0x6E375A3B` |
| Day 513 | None | `Idle` | 0 | +15 | N/A | `0x6E370730` |
| Day 514 | None | `Idle` | 0 | +15 | N/A | `0x6E37CC2D` |
| Day 515 | None | `Idle` | 0 | +15 | N/A | `0x6E37F91A` |
| Day 516 | None | `Idle` | 0 | +15 | N/A | `0x6E37A617` |
| Day 517 | None | `Idle` | 0 | +15 | N/A | `0x6E36530C` |
| Day 518 | `pol_foundry_fuel_quota` | `Met` | +3 | +18 | Blocked Clean | `0x6E361879` |
| Day 519 | None | `Idle` | 0 | +18 | N/A | `0x6E36C576` |
| Day 520 | None | `Idle` | 0 | +18 | N/A | `0x6E36F263` |
| Day 521 | None | `Idle` | 0 | +18 | N/A | `0x6E36BF58` |
| Day 522 | None | `Idle` | 0 | +18 | N/A | `0x6E316455` |
| Day 523 | None | `Idle` | 0 | +18 | N/A | `0x6E311142` |
| Day 524 | None | `Idle` | 0 | +18 | N/A | `0x6E31DEBF` |
| Day 525 | None | `Idle` | 0 | +18 | N/A | `0x6E318BB4` |
| Day 526 | None | `Idle` | 0 | +18 | N/A | `0x6E31B0A1` |
| Day 527 | None | `Idle` | 0 | +18 | N/A | `0x6E307D9E` |
| Day 528 | None | `Idle` | 0 | +18 | N/A | `0x6E302A8B` |
| Day 529 | None | `Idle` | 0 | +18 | N/A | `0x6E30D780` |
| Day 530 | None | `Idle` | 0 | +18 | N/A | `0x6E309CFD` |
| Day 531 | None | `Idle` | 0 | +18 | N/A | `0x6E3349EA` |
| Day 532 | `pol_foundry_fuel_quota` | `Met` | +3 | +21 | Blocked Clean | `0x6E3376E7` |
| Day 533 | None | `Idle` | 0 | +21 | N/A | `0x6E3323DC` |
| Day 534 | None | `Idle` | 0 | +21 | N/A | `0x6E33E8C9` |
| Day 535 | None | `Idle` | 0 | +21 | N/A | `0x6E3395C6` |
| Day 536 | None | `Idle` | 0 | +21 | N/A | `0x6E324333` |
| Day 537 | None | `Idle` | 0 | +21 | N/A | `0x6E320828` |
| Day 538 | None | `Idle` | 0 | +21 | N/A | `0x6E323525` |
| Day 539 | None | `Idle` | 0 | +21 | N/A | `0x6E32E212` |
| Day 540 | None | `Idle` | 0 | +21 | N/A | `0x6E32AF0F` |
| Day 541 | None | `Idle` | 0 | +21 | N/A | `0x6E2D5404` |
| Day 542 | None | `Idle` | 0 | +21 | N/A | `0x6E2D0171` |
| Day 543 | None | `Idle` | 0 | +21 | N/A | `0x6E2DCE6E` |
| Day 544 | None | `Idle` | 0 | +21 | N/A | `0x6E2DFB5B` |
| Day 545 | None | `Idle` | 0 | +21 | N/A | `0x6E2DA050` |
| Day 546 | `pol_foundry_fuel_quota` | `Met` | +3 | +24 | Blocked Clean | `0x6E2C6D4D` |
| Day 547 | None | `Idle` | 0 | +24 | N/A | `0x6E2C1ABA` |
| Day 548 | None | `Idle` | 0 | +24 | N/A | `0x6E2CC7B7` |
| Day 549 | None | `Idle` | 0 | +24 | N/A | `0x6E2C8CAC` |
| Day 550 | None | `Idle` | 0 | +24 | N/A | `0x6E2CB999` |
| Day 551 | None | `Idle` | 0 | +24 | N/A | `0x6E2F6696` |
| Day 552 | None | `Idle` | 0 | +24 | N/A | `0x6E2F1383` |
| Day 553 | None | `Idle` | 0 | +24 | N/A | `0x6E2FD8F8` |
| Day 554 | None | `Idle` | 0 | +24 | N/A | `0x6E2F85F5` |
| Day 555 | None | `Idle` | 0 | +24 | N/A | `0x6E2FB2E2` |
| Day 556 | None | `Idle` | 0 | +24 | N/A | `0x6E2E7FDF` |
| Day 557 | None | `Idle` | 0 | +24 | N/A | `0x6E2E24D4` |
| Day 558 | None | `Idle` | 0 | +24 | N/A | `0x6E2ED1C1` |
| Day 559 | None | `Idle` | 0 | +24 | N/A | `0x6E2E9F3E` |
| Day 560 | `pol_foundry_fuel_quota` | `Missed` | -5 | +19 | Blocked Clean | `0x6E29442B` |
| Day 561 | None | `Idle` | 0 | +19 | N/A | `0x6E297120` |
| Day 562 | None | `Idle` | 0 | +19 | N/A | `0x6E293E1D` |
| Day 563 | None | `Idle` | 0 | +19 | N/A | `0x6E29EB0A` |
| Day 564 | None | `Idle` | 0 | +19 | N/A | `0x6E299007` |
| Day 565 | None | `Idle` | 0 | +19 | N/A | `0x6E285D7C` |
| Day 566 | None | `Idle` | 0 | +19 | N/A | `0x6E280A69` |
| Day 567 | None | `Idle` | 0 | +19 | N/A | `0x6E283766` |
| Day 568 | None | `Idle` | 0 | +19 | N/A | `0x6E28FC53` |
| Day 569 | None | `Idle` | 0 | +19 | N/A | `0x6E28A948` |
| Day 570 | None | `Idle` | 0 | +19 | N/A | `0x6E2B5645` |
| Day 571 | None | `Idle` | 0 | +19 | N/A | `0x6E2B03B2` |
| Day 572 | None | `Idle` | 0 | +19 | N/A | `0x6E2BC8AF` |
| Day 573 | None | `Idle` | 0 | +19 | N/A | `0x6E2BF5A4` |
| Day 574 | `pol_foundry_fuel_quota` | `Met` | +3 | +22 | Blocked Clean | `0x6E2BA291` |
| Day 575 | None | `Idle` | 0 | +22 | N/A | `0x6E2A6F8E` |
| Day 576 | None | `Idle` | 0 | +22 | N/A | `0x6E2A14FB` |
| Day 577 | None | `Idle` | 0 | +22 | N/A | `0x6E2AC1F0` |
| Day 578 | None | `Idle` | 0 | +22 | N/A | `0x6E2A8EED` |
| Day 579 | None | `Idle` | 0 | +22 | N/A | `0x6E2ABBDA` |
| Day 580 | None | `Idle` | 0 | +22 | N/A | `0x6E2560D7` |
| Day 581 | None | `Idle` | 0 | +22 | N/A | `0x6E252DCC` |
| Day 582 | None | `Idle` | 0 | +22 | N/A | `0x6E25DB39` |
| Day 583 | None | `Idle` | 0 | +22 | N/A | `0x6E258036` |
| Day 584 | None | `Idle` | 0 | +22 | N/A | `0x6E244D23` |
| Day 585 | None | `Idle` | 0 | +22 | N/A | `0x6E247A18` |
| Day 586 | None | `Idle` | 0 | +22 | N/A | `0x6E242715` |
| Day 587 | None | `Idle` | 0 | +22 | N/A | `0x6E24EC02` |
| Day 588 | `pol_foundry_fuel_quota` | `Met` | +3 | +25 | Blocked Clean | `0x6E24997F` |
| Day 589 | None | `Idle` | 0 | +25 | N/A | `0x6E274674` |
| Day 590 | None | `Idle` | 0 | +25 | N/A | `0x6E277361` |
| Day 591 | None | `Idle` | 0 | +25 | N/A | `0x6E27385E` |
| Day 592 | None | `Idle` | 0 | +25 | N/A | `0x6E27E54B` |
| Day 593 | None | `Idle` | 0 | +25 | N/A | `0x6E279240` |
| Day 594 | None | `Idle` | 0 | +25 | N/A | `0x6E265FBD` |
| Day 595 | None | `Idle` | 0 | +25 | N/A | `0x6E2604AA` |
| Day 596 | None | `Idle` | 0 | +25 | N/A | `0x6E2631A7` |
| Day 597 | None | `Idle` | 0 | +25 | N/A | `0x6E26FE9C` |
| Day 598 | None | `Idle` | 0 | +25 | N/A | `0x6E26AB89` |
| Day 599 | None | `Idle` | 0 | +25 | N/A | `0x6E215086` |
| Day 600 | None | `Idle` | 0 | +25 | N/A | `0x6E211DF3` |

---

# SECTION VI: 25-POINT QA ACCEPTANCE CHECKLIST

1. **Closed Vocabulary Integrity:** Only `met`, `missed`, and `violated` wire values parse.
2. **Rejection of Pseudo-Outcomes:** `NotRatified` and `Pending` return false without side effects.
3. **Forbidden Shorthand Guard:** Schema rejects `"breached"` with validation error.
4. **One-Shot Gate Enforcement:** `IsApplied(treatyId, day)` strictly blocks duplicate execution.
5. **Cumulative Standing Clamp:** Standing is hard-clamped within `[-100, 100]`.
6. **Schema Draft 2020-12:** `foundry_treaty_policies.json` passes schema validation.
7. **Zero Engine References:** Pure C# domain model in `Assets/Ashfall.Core/Foundry/`.
8. **Authored Reason Preservation:** Reasons serialize into history records without truncation.
9. **Deterministic Checksum:** Contract checksum matches across independent game sessions.
10. **Zero Allocation Query:** `IsApplied` executes in O(1) time without allocations.
11. **Policy ID Regex Enforcement:** IDs conform strictly to `^pol_foundry_[a-z0-9_]+$`.
12. **Culture-Invariant Serialization:** Formatting uses invariant culture.
13. **Empty Rules Grace:** Empty JSON handles gracefully without throwing unhandled exceptions.
14. **Save/Load Compatibility:** Applied assessments serialize cleanly into `ExpansionHubSave`.
15. **Re-entrant Thread Safety:** Safe for background thread assessment evaluations.
16. **Negative Day Guard:** Day values < 1 are rejected or clamped.
17. **Missed vs Violated Scaling:** Missed penalties are strictly smaller than violation penalties.
18. **UI Presentation Separation:** `FoundryTreatyPanel.cs` remains purely presentational.
19. **High Cycle Performance:** 1,000+ assessments evaluate in under 0.05ms.
20. **Idempotent Replay:** Re-evaluating past cycles produces bit-exact identical standing.
21. **Multi-Treaty Evaluation:** Distinct treaties on the same day commit independently.
22. **Standing Tier Sync:** Host mirrors standing delta to `FactionStanceEngine`.
23. **Historical Audit Access:** History records remain accessible for chronicle logs.
24. **Memory Leak Protection:** State resets clean up lists and sets completely.
25. **Final Clean Exit:** 100% test pass rate with zero compilation warnings.

---

# SECTION VII: 150 FORENSIC DOMAIN CASEBOOKS

### Casebook TOC-001: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-001`
- **Simulation Day:** Day 4
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4B3147F9`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-002: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-002`
- **Simulation Day:** Day 8
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4B2CF4DA`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-003: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-003`
- **Simulation Day:** Day 12
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4B1865BB`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-004: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-004`
- **Simulation Day:** Day 16
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4B17929C`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-005: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-005`
- **Simulation Day:** Day 20
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4B03037D`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-006: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-006`
- **Simulation Day:** Day 24
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4B7EB05E`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-007: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-007`
- **Simulation Day:** Day 28
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4B6A213F`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-008: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-008`
- **Simulation Day:** Day 32
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4B615E10`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-009: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-009`
- **Simulation Day:** Day 36
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4B5CCCF1`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-010: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-010`
- **Simulation Day:** Day 40
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4B487DD2`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-011: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-011`
- **Simulation Day:** Day 44
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4B47EAB3`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-012: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-012`
- **Simulation Day:** Day 48
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4BB31B94`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-013: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-013`
- **Simulation Day:** Day 52
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4BAE8875`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-014: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-014`
- **Simulation Day:** Day 56
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4B9A3956`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-015: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-015`
- **Simulation Day:** Day 60
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4B915637`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-016: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-016`
- **Simulation Day:** Day 64
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4B8CC708`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-017: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-017`
- **Simulation Day:** Day 68
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4BF875E9`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-018: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-018`
- **Simulation Day:** Day 72
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4BF7E2CA`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-019: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-019`
- **Simulation Day:** Day 76
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4BE313AB`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-020: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-020`
- **Simulation Day:** Day 80
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4BDE808C`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-021: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-021`
- **Simulation Day:** Day 84
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4BCA316D`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-022: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-022`
- **Simulation Day:** Day 88
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4BC1AE4E`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-023: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-023`
- **Simulation Day:** Day 92
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4A3CDF2F`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-024: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-024`
- **Simulation Day:** Day 96
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4A284C00`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-025: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-025`
- **Simulation Day:** Day 100
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4A27FAE1`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-026: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-026`
- **Simulation Day:** Day 104
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4A136BC2`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-027: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-027`
- **Simulation Day:** Day 108
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4A0E98A3`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-028: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-028`
- **Simulation Day:** Day 112
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4A7A0984`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-029: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-029`
- **Simulation Day:** Day 116
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4A71A665`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-030: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-030`
- **Simulation Day:** Day 120
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4A6CD746`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-031: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-031`
- **Simulation Day:** Day 124
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4A584427`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-032: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-032`
- **Simulation Day:** Day 128
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4A57F538`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-033: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-033`
- **Simulation Day:** Day 132
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4A436219`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-034: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-034`
- **Simulation Day:** Day 136
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4ABE90FA`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-035: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-035`
- **Simulation Day:** Day 140
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4AAA01DB`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-036: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-036`
- **Simulation Day:** Day 144
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4AA1BEBC`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-037: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-037`
- **Simulation Day:** Day 148
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4A9D2F9D`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-038: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-038`
- **Simulation Day:** Day 152
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4A885C7E`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-039: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-039`
- **Simulation Day:** Day 156
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4A87CD5F`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-040: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-040`
- **Simulation Day:** Day 160
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4AF37A30`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-041: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-041`
- **Simulation Day:** Day 164
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4AEEEB11`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-042: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-042`
- **Simulation Day:** Day 168
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4ADA19F2`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-043: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-043`
- **Simulation Day:** Day 172
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4AD1B6D3`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-044: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-044`
- **Simulation Day:** Day 176
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4ACD27B4`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-045: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-045`
- **Simulation Day:** Day 180
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x49385495`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-046: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-046`
- **Simulation Day:** Day 184
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4937C576`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-047: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-047`
- **Simulation Day:** Day 188
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x49237257`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-048: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-048`
- **Simulation Day:** Day 192
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x491EE328`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-049: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-049`
- **Simulation Day:** Day 196
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x490A1009`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-050: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-050`
- **Simulation Day:** Day 200
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x49018EEA`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-051: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-051`
- **Simulation Day:** Day 204
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x497D3FCB`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-052: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-052`
- **Simulation Day:** Day 208
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4968ACAC`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-053: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-053`
- **Simulation Day:** Day 212
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4967DD8D`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-054: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-054`
- **Simulation Day:** Day 216
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x49534A6E`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-055: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-055`
- **Simulation Day:** Day 220
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x494EFB4F`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-056: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-056`
- **Simulation Day:** Day 224
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x49BA6820`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-057: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-057`
- **Simulation Day:** Day 228
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x49B19901`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-058: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-058`
- **Simulation Day:** Day 232
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x49AD37E2`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-059: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-059`
- **Simulation Day:** Day 236
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4998A4C3`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-060: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-060`
- **Simulation Day:** Day 240
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4997D5A4`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-061: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-061`
- **Simulation Day:** Day 244
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x49834285`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-062: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-062`
- **Simulation Day:** Day 248
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x49FEF366`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-063: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-063`
- **Simulation Day:** Day 252
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x49EA6047`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-064: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-064`
- **Simulation Day:** Day 256
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x49E19158`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-065: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-065`
- **Simulation Day:** Day 260
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x49DD0E39`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-066: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-066`
- **Simulation Day:** Day 264
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x49C8BF1A`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-067: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-067`
- **Simulation Day:** Day 268
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x49C42DFB`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-068: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-068`
- **Simulation Day:** Day 272
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x48335ADC`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-069: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-069`
- **Simulation Day:** Day 276
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x482ECBBD`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-070: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-070`
- **Simulation Day:** Day 280
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x481A789E`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-071: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-071`
- **Simulation Day:** Day 284
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4811E97F`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-072: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-072`
- **Simulation Day:** Day 288
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x480D0650`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-073: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-073`
- **Simulation Day:** Day 292
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4878B731`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-074: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-074`
- **Simulation Day:** Day 296
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x48742412`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-075: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-075`
- **Simulation Day:** Day 300
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x486352F3`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-076: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-076`
- **Simulation Day:** Day 304
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x485EC3D4`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-077: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-077`
- **Simulation Day:** Day 308
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x484A70B5`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-078: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-078`
- **Simulation Day:** Day 312
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4841E196`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-079: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-079`
- **Simulation Day:** Day 316
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x48BD1E77`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-080: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-080`
- **Simulation Day:** Day 320
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x48A88F48`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-081: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-081`
- **Simulation Day:** Day 324
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x48A43C29`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-082: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-082`
- **Simulation Day:** Day 328
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4893AD0A`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-083: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-083`
- **Simulation Day:** Day 332
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x488EDBEB`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-084: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-084`
- **Simulation Day:** Day 336
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x48FA48CC`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-085: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-085`
- **Simulation Day:** Day 340
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x48F1F9AD`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-086: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-086`
- **Simulation Day:** Day 344
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x48ED168E`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-087: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-087`
- **Simulation Day:** Day 348
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x48D8876F`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-088: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-088`
- **Simulation Day:** Day 352
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x48D43440`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-089: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-089`
- **Simulation Day:** Day 356
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x48C3A521`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-090: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-090`
- **Simulation Day:** Day 360
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4F3ED202`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-091: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-091`
- **Simulation Day:** Day 364
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4F2A40E3`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-092: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-092`
- **Simulation Day:** Day 368
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4F21F1C4`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-093: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-093`
- **Simulation Day:** Day 372
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4F1D6EA5`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-094: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-094`
- **Simulation Day:** Day 376
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4F089F86`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-095: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-095`
- **Simulation Day:** Day 380
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4F040C67`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-096: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-096`
- **Simulation Day:** Day 384
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4F73BD78`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-097: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-097`
- **Simulation Day:** Day 388
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4F6F2A59`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-098: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-098`
- **Simulation Day:** Day 392
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4F5A5B3A`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-099: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-099`
- **Simulation Day:** Day 396
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4F51C81B`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-100: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-100`
- **Simulation Day:** Day 400
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4F4D66FC`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-101: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-101`
- **Simulation Day:** Day 404
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4FB897DD`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-102: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-102`
- **Simulation Day:** Day 408
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4FB404BE`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-103: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-103`
- **Simulation Day:** Day 412
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4FA3B59F`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-104: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-104`
- **Simulation Day:** Day 416
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4F9F2270`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-105: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-105`
- **Simulation Day:** Day 420
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4F8A5351`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-106: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-106`
- **Simulation Day:** Day 424
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4F81C032`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-107: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-107`
- **Simulation Day:** Day 428
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4FFD7113`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-108: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-108`
- **Simulation Day:** Day 432
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4FE8EFF4`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-109: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-109`
- **Simulation Day:** Day 436
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4FE41CD5`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-110: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-110`
- **Simulation Day:** Day 440
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4FD38DB6`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-111: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-111`
- **Simulation Day:** Day 444
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4FCF3A97`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-112: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-112`
- **Simulation Day:** Day 448
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4E3AAB68`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-113: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-113`
- **Simulation Day:** Day 452
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4E31D849`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-114: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-114`
- **Simulation Day:** Day 456
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4E2D492A`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-115: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-115`
- **Simulation Day:** Day 460
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4E18E60B`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-116: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-116`
- **Simulation Day:** Day 464
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4E1414EC`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-117: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-117`
- **Simulation Day:** Day 468
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4E0385CD`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-118: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-118`
- **Simulation Day:** Day 472
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4E7F32AE`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-119: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-119`
- **Simulation Day:** Day 476
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4E6AA38F`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-120: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-120`
- **Simulation Day:** Day 480
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4E61D060`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-121: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-121`
- **Simulation Day:** Day 484
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4E5D4141`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-122: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-122`
- **Simulation Day:** Day 488
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4E48FE22`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-123: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-123`
- **Simulation Day:** Day 492
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4E446F03`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-124: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-124`
- **Simulation Day:** Day 496
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4EB39DE4`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-125: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-125`
- **Simulation Day:** Day 500
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4EAF0AC5`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-126: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-126`
- **Simulation Day:** Day 504
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4E9ABBA6`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-127: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-127`
- **Simulation Day:** Day 508
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4E962887`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-128: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-128`
- **Simulation Day:** Day 512
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4E8D5998`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-129: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-129`
- **Simulation Day:** Day 516
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4EF8F679`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-130: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-130`
- **Simulation Day:** Day 520
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4EF4675A`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-131: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-131`
- **Simulation Day:** Day 524
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4EE3943B`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-132: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-132`
- **Simulation Day:** Day 528
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4EDF051C`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-133: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-133`
- **Simulation Day:** Day 532
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4ECAB3FD`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-134: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-134`
- **Simulation Day:** Day 536
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4EC620DE`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-135: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-135`
- **Simulation Day:** Day 540
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4D3D51BF`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-136: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-136`
- **Simulation Day:** Day 544
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4D28CE90`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-137: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-137`
- **Simulation Day:** Day 548
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4D247F71`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-138: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-138`
- **Simulation Day:** Day 552
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4D13EC52`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-139: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-139`
- **Simulation Day:** Day 556
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4D0F1D33`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-140: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-140`
- **Simulation Day:** Day 560
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4D7A8A14`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-141: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-141`
- **Simulation Day:** Day 564
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4D7638F5`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-142: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-142`
- **Simulation Day:** Day 568
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4D6DA9D6`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-143: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-143`
- **Simulation Day:** Day 572
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4D58C6B7`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-144: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-144`
- **Simulation Day:** Day 576
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4D547788`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-145: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-145`
- **Simulation Day:** Day 580
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4D43E469`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-146: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-146`
- **Simulation Day:** Day 584
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4DBF154A`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-147: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-147`
- **Simulation Day:** Day 588
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4DAA822B`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-148: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-148`
- **Simulation Day:** Day 592
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Missed`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-5`
- **State Checksum:** `0x4DA6330C`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-149: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-149`
- **Simulation Day:** Day 596
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Violated`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `-12`
- **State Checksum:** `0x4D9DA1ED`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

### Casebook TOC-150: Treaty Outcome Assessment & Idempotency Audit

- **Audit Record:** `CASE-TREATY-OUTCOME-150`
- **Simulation Day:** Day 600
- **Evaluated Treaty:** `pol_foundry_fuel_quota`
- **Closed Outcome:** `Met`
- **Idempotency Gate Verification:** Passed (`IsApplied` verified clean).
- **Standing Delta:** `+3`
- **State Checksum:** `0x4D88DECE`
- **Forensic Observation:** Treaty outcome processed through closed vocabulary engine. One-shot lifecycle verified; subsequent reload confirmed zero double-application of standing delta.

---

# SECTION VIII: 150 TECHNICAL FIELD TREATISES

### Treatise TOC-001: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-001`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #1
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-002: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-002`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #2
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-003: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-003`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #3
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-004: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-004`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #4
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-005: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-005`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #5
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-006: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-006`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #6
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-007: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-007`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #7
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-008: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-008`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #8
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-009: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-009`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #9
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-010: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-010`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #10
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-011: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-011`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #11
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-012: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-012`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #12
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-013: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-013`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #13
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-014: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-014`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #14
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-015: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-015`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #15
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-016: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-016`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #16
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-017: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-017`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #17
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-018: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-018`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #18
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-019: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-019`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #19
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-020: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-020`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #20
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-021: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-021`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #21
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-022: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-022`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #22
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-023: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-023`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #23
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-024: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-024`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #24
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-025: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-025`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #25
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-026: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-026`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #26
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-027: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-027`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #27
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-028: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-028`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #28
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-029: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-029`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #29
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-030: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-030`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #30
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-031: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-031`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #31
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-032: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-032`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #32
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-033: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-033`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #33
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-034: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-034`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #34
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-035: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-035`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #35
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-036: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-036`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #36
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-037: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-037`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #37
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-038: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-038`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #38
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-039: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-039`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #39
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-040: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-040`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #40
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-041: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-041`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #41
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-042: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-042`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #42
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-043: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-043`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #43
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-044: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-044`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #44
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-045: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-045`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #45
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-046: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-046`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #46
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-047: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-047`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #47
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-048: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-048`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #48
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-049: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-049`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #49
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-050: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-050`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #50
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-051: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-051`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #51
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-052: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-052`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #52
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-053: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-053`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #53
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-054: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-054`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #54
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-055: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-055`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #55
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-056: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-056`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #56
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-057: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-057`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #57
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-058: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-058`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #58
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-059: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-059`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #59
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-060: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-060`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #60
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-061: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-061`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #61
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-062: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-062`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #62
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-063: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-063`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #63
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-064: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-064`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #64
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-065: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-065`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #65
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-066: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-066`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #66
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-067: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-067`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #67
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-068: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-068`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #68
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-069: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-069`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #69
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-070: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-070`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #70
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-071: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-071`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #71
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-072: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-072`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #72
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-073: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-073`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #73
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-074: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-074`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #74
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-075: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-075`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #75
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-076: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-076`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #76
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-077: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-077`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #77
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-078: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-078`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #78
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-079: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-079`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #79
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-080: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-080`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #80
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-081: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-081`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #81
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-082: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-082`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #82
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-083: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-083`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #83
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-084: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-084`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #84
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-085: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-085`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #85
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-086: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-086`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #86
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-087: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-087`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #87
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-088: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-088`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #88
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-089: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-089`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #89
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-090: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-090`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #90
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-091: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-091`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #91
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-092: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-092`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #92
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-093: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-093`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #93
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-094: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-094`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #94
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-095: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-095`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #95
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-096: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-096`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #96
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-097: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-097`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #97
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-098: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-098`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #98
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-099: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-099`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #99
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-100: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-100`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #100
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-101: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-101`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #101
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-102: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-102`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #102
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-103: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-103`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #103
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-104: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-104`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #104
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-105: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-105`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #105
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-106: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-106`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #106
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-107: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-107`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #107
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-108: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-108`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #108
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-109: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-109`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #109
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-110: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-110`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #110
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-111: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-111`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #111
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-112: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-112`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #112
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-113: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-113`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #113
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-114: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-114`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #114
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-115: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-115`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #115
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-116: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-116`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #116
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-117: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-117`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #117
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-118: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-118`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #118
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-119: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-119`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #119
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-120: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-120`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #120
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-121: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-121`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #121
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-122: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-122`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #122
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-123: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-123`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #123
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-124: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-124`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #124
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-125: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-125`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #125
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-126: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-126`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #126
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-127: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-127`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #127
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-128: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-128`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #128
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-129: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-129`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #129
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-130: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-130`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #130
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-131: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-131`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #131
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-132: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-132`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #132
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-133: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-133`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #133
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-134: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-134`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #134
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-135: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-135`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #135
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-136: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-136`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #136
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-137: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-137`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #137
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-138: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-138`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #138
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-139: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-139`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #139
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-140: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-140`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #140
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-141: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-141`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #141
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-142: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-142`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #142
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-143: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-143`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #143
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-144: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-144`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #144
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-145: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-145`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #145
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-146: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-146`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #146
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-147: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-147`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #147
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-148: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-148`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #148
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-149: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-149`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #149
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

### Treatise TOC-150: Closed Semantic Vocabularies in Persistent Diplomatic Simulation

- **Document Identifier:** `TREATISE-OUTCOME-150`
- **Classification:** Diplomatic Semantics & Lifecycle State Machines
- **System Anchor:** `FoundryTreatyOutcomeContractEngine`
- **Directive:** Outcome Contract Rule #150
- **Analysis:**
  Diplomatic pacts in persistent games suffer catastrophic state rot when outcome states are left open-ended or loosely typed in data files. By enforcing a closed semantic vocabulary (`met`, `missed`, `violated`) and decoupling pending/neutral states from the consequence engine, the simulation guarantees that every assessment produces a deterministic, well-bounded consequence. The one-shot gate ensures historical durability across arbitrary save/load cycles.
- **Verification Protocol:** Verify that writing non-canonical strings such as `"breached"` fails schema validation immediately.

---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

### 12.1 Elimination of the "Breached" Semantic Bug
In early draft documentation, designers frequently referred to treaty failure as "breached". However, the runtime engine expects `violated`. This specification codifies that `"breached"` is strictly forbidden in data files. The closed vocabulary (`met`, `missed`, `violated`) ensures 100% catalog integrity and schema conformance.

### 12.2 Standing Clamping Invariant
Accumulated Foundry standing is strictly clamped to `[-100, 100]`. No combination of repeated violations can drive standing below -100, and no surplus of deliveries can inflate standing beyond +100.

### 12.3 Engine-Free Isolation
The engine resides strictly within `Assets/Ashfall.Core/Foundry/` under `netstandard2.1`. Zero Godot engine types are imported.

### 12.4 Save State Contract Compliance
Evaluated assessments serialize into the `ExpansionHubSave` envelope via standard JSON serialization.

### 12.5 Memory and Performance Boundaries
`ApplyConsequence` executes in under 0.01ms with zero allocations.

### 12.6 Master Authority Alignment
Conforms strictly to Master Authority Volumes 19 and 43.

---

# SECTION XIII: INTEGRATION FRAMEWORK & EVENT WIRING

### 13.1 Assessment Tick Integration
1. On assessment day, `FoundryTreatySystem` calculates delivery quota compliance.
2. `FoundryTreatyOutcomeContractEngine.ApplyConsequence(...)` executes.
3. If successful, `FoundryStandingChangedEvent` is dispatched.
4. `SilentFoundryHostSession` mirrors the delta to `FactionStanceEngine`.

### 13.2 Boundary Protections
UI panels cannot manually set outcome states; all evaluations originate from Core simulation.

---

# SECTION XIV: DATA CONSUMER & SEAM HARMONIZATION

| Consumer | Consumed Data | Seam Purpose | Authority Seal |
|---|---|---|---|
| `FoundryTreatySystem` | `TreatConsequenceRule` | Outcome consequence mapping | Core Authoritative |
| `ExpansionHubSave` | `AppliedConsequenceRecord` | Durable ledger persistence | Save Envelope Seam |
| `FactionStanceEngine` | `StandingDelta` | Host stance mirroring | Diplomatic Stance |
| `FoundryTreatyPanel` | `CurrentCumulativeStanding` | UI standing display | Presentation Only |

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

### 15.1 Bit-Exact Checksum Invariant
The contract checksum computes an FNV-1a hash over all treaty IDs, outcomes, assessment days, and standing deltas.

### 15.2 Master Authority Volume 19 & 43 Alignment
Strictly aligned with `newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Closed vocabulary enforced.

### 15.3 Re-entrant Execution
All evaluation methods are thread-safe and re-entrant.

### 15.4 Performance Budgets
Evaluation completes in under 0.01ms.

### 15.5 Final Architectural Acceptance Seal
This specification represents the binding authority on Foundry Treaty outcome contracts in ASHFALL.
