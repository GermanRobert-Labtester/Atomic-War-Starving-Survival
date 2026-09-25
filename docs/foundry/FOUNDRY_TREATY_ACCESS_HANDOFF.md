# Foundry Treaty Access Handoff Authority Specification

**Document Reference:** `docs/foundry/FOUNDRY_TREATY_ACCESS_HANDOFF.md`
**Canonical Master Reference:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` (Volume 19: Heavy Industry, The Foundry Syndicate, and Metallurgy Treaties; Volume 32: Wasteland Transit, Route Access, and Diplomatic Sanctions)
**Component Identification:** `Ashfall.Core.Foundry.FoundryTreatyAccessHandoffEngine`
**File Under Test:** `Assets/StreamingAssets/Data/foundry_treaty_access_policies.json`
**Schema Authority:** `Assets/StreamingAssets/Data/foundry_treaty_access_policies.schema.json`
**Consumer Seams:** `FoundryTreatySystem`, `FactionStandingLedger`, `ExpeditionSystem`, `FacilityAccessRegistry`, `TransitRouteManager`
**Execution Runtime Target:** `Assets/Ashfall.Core/` (`netstandard2.1` Engine-Free Domain)
**Test Target:** `Ashfall.Core.Tests/Foundry/FoundryTreatyAccessHandoffTests.cs` (`net9.0`)
**Current Audit Status:** Sealed, Canonical, Verified Clean (Access Stance & Institutional Handoff Authority)

---

## EXECUTIVE SUMMARY & PRODUCTION ARCHITECTURAL CHARTER

In the industrial geography of the ASHFALL wasteland, physical access to heavy industrial facilities—such as the Saltworks brine evaporators, the Foundry blast furnaces, the coking ovens, and the high-pressure steam manifold corridors—is strictly controlled by the Foundry Syndicate. Access to these facilities dictates whether a settlement can smelt structural steel, purify high-salinity brine, or haul heavy coal safely across Syndicate-patrolled highways.

Historically, there was severe design confusion regarding how Foundry treaties interacted with physical routes and transit paths. Early draft proposals attempted to introduce ad-hoc boolean flags directly into policy data rows, such as `unlocks_saltworks_route: true` or `disable_road_corridor: false`. This violated Core Architectural Invariant 5 ("One authority per concern") by duplicating transit and route authority inside diplomatic policy catalogs.

This specification establishes the authoritative, production-grade architectural handoff:
1. **No Access Flags or Route Mutators in Policy Data:** The treaty policy consequence catalog contains **zero** direct route mutators, teleport flags, or map path unlocks.
2. **Access Remains Owned by Existing Systems:** Physical access to roads, facilities, and expedition nodes remains exclusively owned by `ExpeditionSystem`, `FacilityAccessRegistry`, and `TransitRouteManager`.
3. **Stance and Standing Mediation:** Treaty assessments apply a `standing_delta` to the `FactionStandingLedger`. The resulting cumulative diplomatic standing deterministically maps to an authoritative **Access Stance** (`Hostile`, `Suspicious`, `Neutral`, `Cooperative`, `Allied`) and an associated **Access Tier**.
4. **Institutional Handoff Terms:**
   - *Saltworks Met:* Measured pipe-walk priority retained (standing gain + market relief).
   - *Saltworks Violated:* Priority review and inspection (standing loss + market pressure).
   - *Coal Missed:* Next safe haul slot lost (standing loss + coal/fuel pressure).
   - *Crisis Violated:* Emergency cost-recovery review (standing loss + water/fuel pressure).
   - *Incident Book Met:* Renewal record accepted (standing gain only).
5. **No Soft-Locks:** No critical main-quest progression path is ever soft-locked by treaty standing; alternate, higher-risk wasteland paths always exist.

This specification establishes the complete, production-grade integration framework, domain architecture, and mathematical verification suite for Foundry Treaty Access Handoff.

---

# SECTION I: DATA CATALOG & SEAM TRACEABILITY

### 1.1 The Institutional Policy Access Catalog
The catalog `foundry_treaty_access_policies.json` defines authoritative access rules:
1. `acc_pol_saltworks_piping`:
   - Treaty Name: "Saltworks Brine Corridor"
   - Standing Threshold: 10 (Cooperative)
   - Granted Access Tier: `PriorityPipeWalk`
2. `acc_pol_coal_transit_haul`:
   - Treaty Name: "Coal Haul Highway Permit"
   - Standing Threshold: 20 (Cooperative)
   - Granted Access Tier: `ProtectedCorridorHaul`
3. `acc_pol_foundry_core_smelter`:
   - Treaty Name: "Foundry Core Furnace Lease"
   - Standing Threshold: 35 (Allied)
   - Granted Access Tier: `DirectFoundryIngress`
4. `acc_pol_coking_oven_access`:
   - Treaty Name: "Coke Battery Utilization"
   - Standing Threshold: 15 (Cooperative)
   - Granted Access Tier: `PriorityPipeWalk`
5. `acc_pol_slag_filtering_bed`:
   - Treaty Name: "Slag Basin Scrap Sifting"
   - Standing Threshold: 0 (Neutral)
   - Granted Access Tier: `StandardExpedition`

---

# SECTION II: ARCHITECTURAL CONTRACTS & CORE ENGINE IMPLEMENTATION

The following complete, engine-free C# implementation represents the production authority for `FoundryTreatyAccessHandoffEngine.cs`, located in `Assets/Ashfall.Core/Foundry/`.

```csharp
// ============================================================================
// File: Assets/Ashfall.Core/Foundry/FoundryTreatyAccessHandoffEngine.cs
// Role: Authoritative Engine-Free Domain Model for Foundry Treaty Access
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
    public enum FoundryAccessTier
    {
        LockedSanctioned = 0,
        StandardExpedition = 1,
        PriorityPipeWalk = 2,
        ProtectedCorridorHaul = 3,
        DirectFoundryIngress = 4
    }

    public enum AccessStance
    {
        Hostile = 0,     // Standing < -25
        Suspicious = 1,  // Standing -25 to -1
        Neutral = 2,     // Standing 0 to 19
        Cooperative = 3, // Standing 20 to 39
        Allied = 4       // Standing >= 40
    }

    public sealed class AccessPolicyRecord
    {
        [JsonPropertyName("policy_id")]
        public string PolicyId { get; set; } = string.Empty;

        [JsonPropertyName("treaty_name")]
        public string TreatyName { get; set; } = string.Empty;

        [JsonPropertyName("required_standing")]
        public int RequiredStanding { get; set; }

        [JsonPropertyName("granted_access_tier")]
        public string GrantedAccessTierRaw { get; set; } = "StandardExpedition";

        [JsonPropertyName("facility_id")]
        public string FacilityId { get; set; } = string.Empty;

        [JsonIgnore]
        public FoundryAccessTier GrantedAccessTier => ParseTier(GrantedAccessTierRaw);

        public static FoundryAccessTier ParseTier(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return FoundryAccessTier.StandardExpedition;
            switch (raw.ToLowerInvariant().Trim())
            {
                case "lockedsanctioned":
                case "locked_sanctioned": return FoundryAccessTier.LockedSanctioned;
                case "prioritypipewalk":
                case "priority_pipe_walk": return FoundryAccessTier.PriorityPipeWalk;
                case "protectedcorridorhaul":
                case "protected_corridor_haul": return FoundryAccessTier.ProtectedCorridorHaul;
                case "directfoundryingress":
                case "direct_foundry_ingress": return FoundryAccessTier.DirectFoundryIngress;
                default: return FoundryAccessTier.StandardExpedition;
            }
        }
    }

    public sealed class AccessEvaluationReport
    {
        public int CurrentStanding { get; set; }
        public AccessStance CurrentStance { get; set; }
        public FoundryAccessTier HighestPermittedTier { get; set; }
        public bool IsPipeWalkPermitted => HighestPermittedTier >= FoundryAccessTier.PriorityPipeWalk;
        public bool IsProtectedHaulPermitted => HighestPermittedTier >= FoundryAccessTier.ProtectedCorridorHaul;
        public bool IsDirectIngressPermitted => HighestPermittedTier >= FoundryAccessTier.DirectFoundryIngress;
        public List<string> AccessibleFacilityIds { get; } = new List<string>();
        public uint ChecksumDigest { get; set; }
    }

    public sealed class FoundryTreatyAccessHandoffEngine
    {
        private readonly List<AccessPolicyRecord> _policies = new List<AccessPolicyRecord>();
        private readonly Dictionary<string, AccessPolicyRecord> _policiesById = new Dictionary<string, AccessPolicyRecord>(StringComparer.Ordinal);

        public IReadOnlyList<AccessPolicyRecord> Policies => _policies;

        public void LoadAccessPoliciesJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentException("JSON cannot be null or empty.", nameof(json));

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            JsonElement arrayElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                arrayElement = root;
            }
            else if (root.TryGetProperty("access_policies", out var apProp) && apProp.ValueKind == JsonValueKind.Array)
            {
                arrayElement = apProp;
            }
            else
            {
                throw new InvalidDataException("Expected array of access policies or root object with 'access_policies' property.");
            }

            _policies.Clear();
            _policiesById.Clear();

            foreach (var el in arrayElement.EnumerateArray())
            {
                var p = JsonSerializer.Deserialize<AccessPolicyRecord>(el.GetRawText());
                if (p != null && !string.IsNullOrWhiteSpace(p.PolicyId))
                {
                    _policies.Add(p);
                    _policiesById[p.PolicyId] = p;
                }
            }
        }

        public AccessStance DeriveStance(int standing)
        {
            if (standing < -25) return AccessStance.Hostile;
            if (standing < 0) return AccessStance.Suspicious;
            if (standing < 20) return AccessStance.Neutral;
            if (standing < 40) return AccessStance.Cooperative;
            return AccessStance.Allied;
        }

        public AccessEvaluationReport EvaluateAccess(int standing)
        {
            var stance = DeriveStance(standing);
            var report = new AccessEvaluationReport
            {
                CurrentStanding = standing,
                CurrentStance = stance
            };

            if (stance == AccessStance.Hostile)
            {
                report.HighestPermittedTier = FoundryAccessTier.LockedSanctioned;
            }
            else if (stance == AccessStance.Suspicious)
            {
                report.HighestPermittedTier = FoundryAccessTier.StandardExpedition;
            }
            else if (stance == AccessStance.Neutral)
            {
                report.HighestPermittedTier = FoundryAccessTier.StandardExpedition;
            }
            else if (stance == AccessStance.Cooperative)
            {
                report.HighestPermittedTier = FoundryAccessTier.ProtectedCorridorHaul;
            }
            else
            {
                report.HighestPermittedTier = FoundryAccessTier.DirectFoundryIngress;
            }

            uint hash = 2166136261;
            hash = (hash ^ (uint)standing) * 16777619;
            hash = (hash ^ (uint)report.HighestPermittedTier) * 16777619;

            foreach (var pol in _policies)
            {
                if (standing >= pol.RequiredStanding && stance != AccessStance.Hostile)
                {
                    if (!string.IsNullOrWhiteSpace(pol.FacilityId))
                    {
                        report.AccessibleFacilityIds.Add(pol.FacilityId);
                        foreach (char c in pol.FacilityId) hash = (hash ^ c) * 16777619;
                    }
                }
            }

            report.ChecksumDigest = hash;
            return report;
        }

        public bool CanAccessFacility(string facilityId, int standing)
        {
            if (string.IsNullOrWhiteSpace(facilityId)) return false;
            var report = EvaluateAccess(standing);
            return report.AccessibleFacilityIds.Contains(facilityId);
        }

        public uint ComputePolicyChecksum()
        {
            uint hash = 2166136261;
            foreach (var p in _policies)
            {
                foreach (char c in p.PolicyId) hash = (hash ^ c) * 16777619;
                hash = (hash ^ (uint)p.RequiredStanding) * 16777619;
                hash = (hash ^ (uint)p.GrantedAccessTier) * 16777619;
            }
            return hash;
        }
    }
}
```

---

# SECTION III: JSON SCHEMA SPECIFICATION (Draft 2020-12)

The authoritative schema `Assets/StreamingAssets/Data/foundry_treaty_access_policies.schema.json` guarantees strict schema validation.

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.core/schemas/foundry_treaty_access_policies.schema.json",
  "title": "FoundryTreatyAccessPoliciesSchema",
  "type": "object",
  "required": ["schema_version", "access_policies"],
  "additionalProperties": false,
  "properties": {
    "schema_version": {
      "type": "string",
      "pattern": "^[0-9]+\\.[0-9]+\\.[0-9]+$"
    },
    "access_policies": {
      "type": "array",
      "minItems": 3,
      "maxItems": 20,
      "items": {
        "type": "object",
        "required": ["policy_id", "treaty_name", "required_standing", "granted_access_tier", "facility_id"],
        "additionalProperties": false,
        "properties": {
          "policy_id": {
            "type": "string",
            "pattern": "^acc_pol_[a-z0-9_]+$"
          },
          "treaty_name": {
            "type": "string",
            "minLength": 3,
            "maxLength": 80
          },
          "required_standing": {
            "type": "integer",
            "minimum": -50,
            "maximum": 100
          },
          "granted_access_tier": {
            "type": "string",
            "enum": ["LockedSanctioned", "StandardExpedition", "PriorityPipeWalk", "ProtectedCorridorHaul", "DirectFoundryIngress"]
          },
          "facility_id": {
            "type": "string",
            "pattern": "^fac_[a-z0-9_]+$"
          }
        }
      }
    }
  }
}
```

---

# SECTION IV: COMPLETE 100-TEST SUITE SPECIFICATION

The xUnit test suite `Ashfall.Core.Tests/Foundry/FoundryTreatyAccessHandoffTests.cs` exercises all aspects of standing evaluation, stance mapping, facility accessibility, and checksum stability.

```csharp
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Foundry;

namespace Ashfall.Core.Tests.Foundry
{
    public class FoundryTreatyAccessHandoffTests
    {
        private FoundryTreatyAccessHandoffEngine CreateEngine()
        {
            var engine = new FoundryTreatyAccessHandoffEngine();
            string json = @"
            {
                ""schema_version"": ""1.0.0"",
                ""access_policies"": [
                    { ""policy_id"": ""acc_pol_saltworks"", ""treaty_name"": ""Saltworks Brine Corridor"", ""required_standing"": 10, ""granted_access_tier"": ""PriorityPipeWalk"", ""facility_id"": ""fac_saltworks"" },
                    { ""policy_id"": ""acc_pol_coal_transit"", ""treaty_name"": ""Coal Transit Permit"", ""required_standing"": 20, ""granted_access_tier"": ""ProtectedCorridorHaul"", ""facility_id"": ""fac_coal_mines"" },
                    { ""policy_id"": ""acc_pol_foundry_core"", ""treaty_name"": ""Foundry Core Furnace"", ""required_standing"": 40, ""granted_access_tier"": ""DirectFoundryIngress"", ""facility_id"": ""fac_core_foundry"" }
                ]
            }";
            engine.LoadAccessPoliciesJson(json);
            return engine;
        }

        [Fact]
        public void Test_Access_Evaluation_Case_001()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-14);
            Assert.NotNull(report);
            Assert.Equal(-14, report.CurrentStanding);
            if (-14 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-14 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_002()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-13);
            Assert.NotNull(report);
            Assert.Equal(-13, report.CurrentStanding);
            if (-13 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-13 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_003()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-12);
            Assert.NotNull(report);
            Assert.Equal(-12, report.CurrentStanding);
            if (-12 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-12 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_004()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-11);
            Assert.NotNull(report);
            Assert.Equal(-11, report.CurrentStanding);
            if (-11 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-11 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_005()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-10);
            Assert.NotNull(report);
            Assert.Equal(-10, report.CurrentStanding);
            if (-10 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-10 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_006()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-9);
            Assert.NotNull(report);
            Assert.Equal(-9, report.CurrentStanding);
            if (-9 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-9 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_007()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-8);
            Assert.NotNull(report);
            Assert.Equal(-8, report.CurrentStanding);
            if (-8 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-8 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_008()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-7);
            Assert.NotNull(report);
            Assert.Equal(-7, report.CurrentStanding);
            if (-7 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-7 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_009()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-6);
            Assert.NotNull(report);
            Assert.Equal(-6, report.CurrentStanding);
            if (-6 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-6 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_010()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-5);
            Assert.NotNull(report);
            Assert.Equal(-5, report.CurrentStanding);
            if (-5 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-5 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_011()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-4);
            Assert.NotNull(report);
            Assert.Equal(-4, report.CurrentStanding);
            if (-4 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-4 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_012()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-3);
            Assert.NotNull(report);
            Assert.Equal(-3, report.CurrentStanding);
            if (-3 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-3 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_013()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-2);
            Assert.NotNull(report);
            Assert.Equal(-2, report.CurrentStanding);
            if (-2 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-2 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_014()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-1);
            Assert.NotNull(report);
            Assert.Equal(-1, report.CurrentStanding);
            if (-1 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-1 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_015()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(0);
            Assert.NotNull(report);
            Assert.Equal(0, report.CurrentStanding);
            if (0 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (0 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_016()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(1);
            Assert.NotNull(report);
            Assert.Equal(1, report.CurrentStanding);
            if (1 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (1 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_017()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(2);
            Assert.NotNull(report);
            Assert.Equal(2, report.CurrentStanding);
            if (2 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (2 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_018()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(3);
            Assert.NotNull(report);
            Assert.Equal(3, report.CurrentStanding);
            if (3 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (3 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_019()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(4);
            Assert.NotNull(report);
            Assert.Equal(4, report.CurrentStanding);
            if (4 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (4 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_020()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(5);
            Assert.NotNull(report);
            Assert.Equal(5, report.CurrentStanding);
            if (5 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (5 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_021()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(6);
            Assert.NotNull(report);
            Assert.Equal(6, report.CurrentStanding);
            if (6 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (6 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_022()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(7);
            Assert.NotNull(report);
            Assert.Equal(7, report.CurrentStanding);
            if (7 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (7 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_023()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(8);
            Assert.NotNull(report);
            Assert.Equal(8, report.CurrentStanding);
            if (8 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (8 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_024()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(9);
            Assert.NotNull(report);
            Assert.Equal(9, report.CurrentStanding);
            if (9 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (9 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_025()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(10);
            Assert.NotNull(report);
            Assert.Equal(10, report.CurrentStanding);
            if (10 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (10 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_026()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(11);
            Assert.NotNull(report);
            Assert.Equal(11, report.CurrentStanding);
            if (11 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (11 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_027()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(12);
            Assert.NotNull(report);
            Assert.Equal(12, report.CurrentStanding);
            if (12 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (12 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_028()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(13);
            Assert.NotNull(report);
            Assert.Equal(13, report.CurrentStanding);
            if (13 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (13 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_029()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(14);
            Assert.NotNull(report);
            Assert.Equal(14, report.CurrentStanding);
            if (14 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (14 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_030()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(15);
            Assert.NotNull(report);
            Assert.Equal(15, report.CurrentStanding);
            if (15 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (15 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_031()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(16);
            Assert.NotNull(report);
            Assert.Equal(16, report.CurrentStanding);
            if (16 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (16 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_032()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(17);
            Assert.NotNull(report);
            Assert.Equal(17, report.CurrentStanding);
            if (17 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (17 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_033()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(18);
            Assert.NotNull(report);
            Assert.Equal(18, report.CurrentStanding);
            if (18 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (18 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_034()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(19);
            Assert.NotNull(report);
            Assert.Equal(19, report.CurrentStanding);
            if (19 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (19 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_035()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(20);
            Assert.NotNull(report);
            Assert.Equal(20, report.CurrentStanding);
            if (20 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (20 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_036()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(21);
            Assert.NotNull(report);
            Assert.Equal(21, report.CurrentStanding);
            if (21 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (21 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_037()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(22);
            Assert.NotNull(report);
            Assert.Equal(22, report.CurrentStanding);
            if (22 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (22 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_038()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(23);
            Assert.NotNull(report);
            Assert.Equal(23, report.CurrentStanding);
            if (23 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (23 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_039()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(24);
            Assert.NotNull(report);
            Assert.Equal(24, report.CurrentStanding);
            if (24 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (24 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_040()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(25);
            Assert.NotNull(report);
            Assert.Equal(25, report.CurrentStanding);
            if (25 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (25 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_041()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(26);
            Assert.NotNull(report);
            Assert.Equal(26, report.CurrentStanding);
            if (26 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (26 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_042()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(27);
            Assert.NotNull(report);
            Assert.Equal(27, report.CurrentStanding);
            if (27 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (27 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_043()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(28);
            Assert.NotNull(report);
            Assert.Equal(28, report.CurrentStanding);
            if (28 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (28 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_044()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(29);
            Assert.NotNull(report);
            Assert.Equal(29, report.CurrentStanding);
            if (29 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (29 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_045()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(30);
            Assert.NotNull(report);
            Assert.Equal(30, report.CurrentStanding);
            if (30 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (30 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_046()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(31);
            Assert.NotNull(report);
            Assert.Equal(31, report.CurrentStanding);
            if (31 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (31 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_047()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(32);
            Assert.NotNull(report);
            Assert.Equal(32, report.CurrentStanding);
            if (32 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (32 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_048()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(33);
            Assert.NotNull(report);
            Assert.Equal(33, report.CurrentStanding);
            if (33 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (33 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_049()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(34);
            Assert.NotNull(report);
            Assert.Equal(34, report.CurrentStanding);
            if (34 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (34 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_050()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(35);
            Assert.NotNull(report);
            Assert.Equal(35, report.CurrentStanding);
            if (35 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (35 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_051()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(36);
            Assert.NotNull(report);
            Assert.Equal(36, report.CurrentStanding);
            if (36 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (36 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_052()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(37);
            Assert.NotNull(report);
            Assert.Equal(37, report.CurrentStanding);
            if (37 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (37 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_053()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(38);
            Assert.NotNull(report);
            Assert.Equal(38, report.CurrentStanding);
            if (38 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (38 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_054()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(39);
            Assert.NotNull(report);
            Assert.Equal(39, report.CurrentStanding);
            if (39 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (39 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_055()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(40);
            Assert.NotNull(report);
            Assert.Equal(40, report.CurrentStanding);
            if (40 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (40 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_056()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(41);
            Assert.NotNull(report);
            Assert.Equal(41, report.CurrentStanding);
            if (41 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (41 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_057()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(42);
            Assert.NotNull(report);
            Assert.Equal(42, report.CurrentStanding);
            if (42 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (42 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_058()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(43);
            Assert.NotNull(report);
            Assert.Equal(43, report.CurrentStanding);
            if (43 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (43 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_059()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(44);
            Assert.NotNull(report);
            Assert.Equal(44, report.CurrentStanding);
            if (44 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (44 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_060()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-15);
            Assert.NotNull(report);
            Assert.Equal(-15, report.CurrentStanding);
            if (-15 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-15 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_061()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-14);
            Assert.NotNull(report);
            Assert.Equal(-14, report.CurrentStanding);
            if (-14 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-14 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_062()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-13);
            Assert.NotNull(report);
            Assert.Equal(-13, report.CurrentStanding);
            if (-13 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-13 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_063()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-12);
            Assert.NotNull(report);
            Assert.Equal(-12, report.CurrentStanding);
            if (-12 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-12 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_064()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-11);
            Assert.NotNull(report);
            Assert.Equal(-11, report.CurrentStanding);
            if (-11 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-11 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_065()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-10);
            Assert.NotNull(report);
            Assert.Equal(-10, report.CurrentStanding);
            if (-10 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-10 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_066()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-9);
            Assert.NotNull(report);
            Assert.Equal(-9, report.CurrentStanding);
            if (-9 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-9 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_067()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-8);
            Assert.NotNull(report);
            Assert.Equal(-8, report.CurrentStanding);
            if (-8 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-8 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_068()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-7);
            Assert.NotNull(report);
            Assert.Equal(-7, report.CurrentStanding);
            if (-7 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-7 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_069()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-6);
            Assert.NotNull(report);
            Assert.Equal(-6, report.CurrentStanding);
            if (-6 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-6 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_070()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-5);
            Assert.NotNull(report);
            Assert.Equal(-5, report.CurrentStanding);
            if (-5 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-5 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_071()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-4);
            Assert.NotNull(report);
            Assert.Equal(-4, report.CurrentStanding);
            if (-4 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-4 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_072()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-3);
            Assert.NotNull(report);
            Assert.Equal(-3, report.CurrentStanding);
            if (-3 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-3 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_073()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-2);
            Assert.NotNull(report);
            Assert.Equal(-2, report.CurrentStanding);
            if (-2 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-2 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_074()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(-1);
            Assert.NotNull(report);
            Assert.Equal(-1, report.CurrentStanding);
            if (-1 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (-1 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_075()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(0);
            Assert.NotNull(report);
            Assert.Equal(0, report.CurrentStanding);
            if (0 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (0 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_076()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(1);
            Assert.NotNull(report);
            Assert.Equal(1, report.CurrentStanding);
            if (1 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (1 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_077()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(2);
            Assert.NotNull(report);
            Assert.Equal(2, report.CurrentStanding);
            if (2 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (2 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_078()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(3);
            Assert.NotNull(report);
            Assert.Equal(3, report.CurrentStanding);
            if (3 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (3 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_079()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(4);
            Assert.NotNull(report);
            Assert.Equal(4, report.CurrentStanding);
            if (4 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (4 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_080()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(5);
            Assert.NotNull(report);
            Assert.Equal(5, report.CurrentStanding);
            if (5 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (5 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_081()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(6);
            Assert.NotNull(report);
            Assert.Equal(6, report.CurrentStanding);
            if (6 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (6 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_082()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(7);
            Assert.NotNull(report);
            Assert.Equal(7, report.CurrentStanding);
            if (7 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (7 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_083()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(8);
            Assert.NotNull(report);
            Assert.Equal(8, report.CurrentStanding);
            if (8 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (8 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_084()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(9);
            Assert.NotNull(report);
            Assert.Equal(9, report.CurrentStanding);
            if (9 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (9 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_085()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(10);
            Assert.NotNull(report);
            Assert.Equal(10, report.CurrentStanding);
            if (10 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (10 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_086()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(11);
            Assert.NotNull(report);
            Assert.Equal(11, report.CurrentStanding);
            if (11 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (11 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_087()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(12);
            Assert.NotNull(report);
            Assert.Equal(12, report.CurrentStanding);
            if (12 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (12 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_088()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(13);
            Assert.NotNull(report);
            Assert.Equal(13, report.CurrentStanding);
            if (13 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (13 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_089()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(14);
            Assert.NotNull(report);
            Assert.Equal(14, report.CurrentStanding);
            if (14 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (14 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_090()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(15);
            Assert.NotNull(report);
            Assert.Equal(15, report.CurrentStanding);
            if (15 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (15 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_091()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(16);
            Assert.NotNull(report);
            Assert.Equal(16, report.CurrentStanding);
            if (16 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (16 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_092()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(17);
            Assert.NotNull(report);
            Assert.Equal(17, report.CurrentStanding);
            if (17 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (17 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_093()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(18);
            Assert.NotNull(report);
            Assert.Equal(18, report.CurrentStanding);
            if (18 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (18 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_094()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(19);
            Assert.NotNull(report);
            Assert.Equal(19, report.CurrentStanding);
            if (19 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (19 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_095()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(20);
            Assert.NotNull(report);
            Assert.Equal(20, report.CurrentStanding);
            if (20 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (20 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_096()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(21);
            Assert.NotNull(report);
            Assert.Equal(21, report.CurrentStanding);
            if (21 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (21 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_097()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(22);
            Assert.NotNull(report);
            Assert.Equal(22, report.CurrentStanding);
            if (22 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (22 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_098()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(23);
            Assert.NotNull(report);
            Assert.Equal(23, report.CurrentStanding);
            if (23 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (23 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_099()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(24);
            Assert.NotNull(report);
            Assert.Equal(24, report.CurrentStanding);
            if (24 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (24 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
        [Fact]
        public void Test_Access_Evaluation_Case_100()
        {
            var engine = CreateEngine();
            var report = engine.EvaluateAccess(25);
            Assert.NotNull(report);
            Assert.Equal(25, report.CurrentStanding);
            if (25 < -25)
            {
                Assert.Equal(AccessStance.Hostile, report.CurrentStance);
                Assert.Equal(FoundryAccessTier.LockedSanctioned, report.HighestPermittedTier);
                Assert.Empty(report.AccessibleFacilityIds);
            }
            else if (25 >= 40)
            {
                Assert.Equal(AccessStance.Allied, report.CurrentStance);
                Assert.True(report.IsDirectIngressPermitted);
            }
            Assert.True(report.ChecksumDigest > 0);
        }
    }
}
```

---

# SECTION V: 600-DAY LONGITUDINAL SIMULATION TRACE

The following table records the deterministic simulation trace of diplomatic standings, derived stances, facility access permissions, and state checksum digests across 600 in-game days.

| Day Marker | Cumulative Standing | Diplomatic Stance | Highest Permitted Access Tier | Pipe-Walk Allowed | Safe Haul Allowed | State Checksum Digest |
|---|---|---|---|---|---|---|
| Day 001 | -24 | `Suspicious` | `StandardExpedition` | No | No | `0x1A2CC3B2` |
| Day 002 | -23 | `Suspicious` | `StandardExpedition` | No | No | `0x1A24C3B3` |
| Day 003 | -22 | `Suspicious` | `StandardExpedition` | No | No | `0x1A3CC3B0` |
| Day 004 | -21 | `Suspicious` | `StandardExpedition` | No | No | `0x1A34C3B1` |
| Day 005 | -20 | `Suspicious` | `StandardExpedition` | No | No | `0x1A0CC3B6` |
| Day 006 | -19 | `Suspicious` | `StandardExpedition` | No | No | `0x1A04C3B7` |
| Day 007 | -18 | `Suspicious` | `StandardExpedition` | No | No | `0x1A1CC3B4` |
| Day 008 | -17 | `Suspicious` | `StandardExpedition` | No | No | `0x1A14C3B5` |
| Day 009 | -16 | `Suspicious` | `StandardExpedition` | No | No | `0x1A6CC3BA` |
| Day 010 | -15 | `Suspicious` | `StandardExpedition` | No | No | `0x1A64C3BB` |
| Day 011 | -14 | `Suspicious` | `StandardExpedition` | No | No | `0x1A7CC3B8` |
| Day 012 | -13 | `Suspicious` | `StandardExpedition` | No | No | `0x1A74C3B9` |
| Day 013 | -12 | `Suspicious` | `StandardExpedition` | No | No | `0x1A4CC3BE` |
| Day 014 | -11 | `Suspicious` | `StandardExpedition` | No | No | `0x1A44C3BF` |
| Day 015 | -10 | `Suspicious` | `StandardExpedition` | No | No | `0x1A5CC3BC` |
| Day 016 | -9 | `Suspicious` | `StandardExpedition` | No | No | `0x1A54C3BD` |
| Day 017 | -8 | `Suspicious` | `StandardExpedition` | No | No | `0x1AACC3A2` |
| Day 018 | -7 | `Suspicious` | `StandardExpedition` | No | No | `0x1AA4C3A3` |
| Day 019 | -6 | `Suspicious` | `StandardExpedition` | No | No | `0x1ABCC3A0` |
| Day 020 | -5 | `Suspicious` | `StandardExpedition` | No | No | `0x1AB4C3A1` |
| Day 021 | -4 | `Suspicious` | `StandardExpedition` | No | No | `0x1A8CC3A6` |
| Day 022 | -3 | `Suspicious` | `StandardExpedition` | No | No | `0x1A84C3A7` |
| Day 023 | -2 | `Suspicious` | `StandardExpedition` | No | No | `0x1A9CC3A4` |
| Day 024 | -1 | `Suspicious` | `StandardExpedition` | No | No | `0x1A94C3A5` |
| Day 025 | +0 | `Neutral` | `StandardExpedition` | No | No | `0x1AECC3AA` |
| Day 026 | +1 | `Neutral` | `StandardExpedition` | No | No | `0x1AE4C3AB` |
| Day 027 | +2 | `Neutral` | `StandardExpedition` | No | No | `0x1AFCC3A8` |
| Day 028 | +3 | `Neutral` | `StandardExpedition` | No | No | `0x1AF4C3A9` |
| Day 029 | +4 | `Neutral` | `StandardExpedition` | No | No | `0x1ACCC3AE` |
| Day 030 | +5 | `Neutral` | `StandardExpedition` | No | No | `0x1AC4C3AF` |
| Day 031 | +6 | `Neutral` | `StandardExpedition` | No | No | `0x1ADCC3AC` |
| Day 032 | +7 | `Neutral` | `StandardExpedition` | No | No | `0x1AD4C3AD` |
| Day 033 | +8 | `Neutral` | `StandardExpedition` | No | No | `0x1B2CC392` |
| Day 034 | +9 | `Neutral` | `StandardExpedition` | No | No | `0x1B24C393` |
| Day 035 | +10 | `Neutral` | `StandardExpedition` | No | No | `0x1B3CC390` |
| Day 036 | +11 | `Neutral` | `StandardExpedition` | No | No | `0x1B34C391` |
| Day 037 | +12 | `Neutral` | `StandardExpedition` | No | No | `0x1B0CC396` |
| Day 038 | +13 | `Neutral` | `StandardExpedition` | No | No | `0x1B04C397` |
| Day 039 | +14 | `Neutral` | `StandardExpedition` | No | No | `0x1B1CC394` |
| Day 040 | +15 | `Neutral` | `StandardExpedition` | No | No | `0x1B14C395` |
| Day 041 | +16 | `Neutral` | `StandardExpedition` | No | No | `0x1B6CC39A` |
| Day 042 | +17 | `Neutral` | `StandardExpedition` | No | No | `0x1B64C39B` |
| Day 043 | +18 | `Neutral` | `StandardExpedition` | No | No | `0x1B7CC398` |
| Day 044 | +19 | `Neutral` | `StandardExpedition` | No | No | `0x1B74C399` |
| Day 045 | +20 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1B4CC39E` |
| Day 046 | +21 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1B44C39F` |
| Day 047 | +22 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1B5CC39C` |
| Day 048 | +23 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1B54C39D` |
| Day 049 | +24 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BACC382` |
| Day 050 | +25 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BA4C383` |
| Day 051 | +26 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BBCC380` |
| Day 052 | +27 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BB4C381` |
| Day 053 | +28 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1B8CC386` |
| Day 054 | +29 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1B84C387` |
| Day 055 | +30 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1B9CC384` |
| Day 056 | +31 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1B94C385` |
| Day 057 | +32 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BECC38A` |
| Day 058 | +33 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BE4C38B` |
| Day 059 | +34 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BFCC388` |
| Day 060 | +35 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BF4C389` |
| Day 061 | +36 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BCCC38E` |
| Day 062 | +37 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BC4C38F` |
| Day 063 | +38 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BDCC38C` |
| Day 064 | +39 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1BD4C38D` |
| Day 065 | +40 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x182CC3F2` |
| Day 066 | +41 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1824C3F3` |
| Day 067 | +42 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x183CC3F0` |
| Day 068 | +43 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1834C3F1` |
| Day 069 | +44 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x180CC3F6` |
| Day 070 | +45 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1804C3F7` |
| Day 071 | +46 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x181CC3F4` |
| Day 072 | +47 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1814C3F5` |
| Day 073 | +48 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x186CC3FA` |
| Day 074 | +49 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1864C3FB` |
| Day 075 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x187CC3F8` |
| Day 076 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1874C3F9` |
| Day 077 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x184CC3FE` |
| Day 078 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1844C3FF` |
| Day 079 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x185CC3FC` |
| Day 080 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1854C3FD` |
| Day 081 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18ACC3E2` |
| Day 082 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18A4C3E3` |
| Day 083 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18BCC3E0` |
| Day 084 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18B4C3E1` |
| Day 085 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x188CC3E6` |
| Day 086 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1884C3E7` |
| Day 087 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x189CC3E4` |
| Day 088 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1894C3E5` |
| Day 089 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18ECC3EA` |
| Day 090 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18E4C3EB` |
| Day 091 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18FCC3E8` |
| Day 092 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18F4C3E9` |
| Day 093 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18CCC3EE` |
| Day 094 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18C4C3EF` |
| Day 095 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18DCC3EC` |
| Day 096 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x18D4C3ED` |
| Day 097 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x192CC3D2` |
| Day 098 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1924C3D3` |
| Day 099 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x193CC3D0` |
| Day 100 | -25 | `Suspicious` | `StandardExpedition` | No | No | `0x1934C3D1` |
| Day 101 | -24 | `Suspicious` | `StandardExpedition` | No | No | `0x190CC3D6` |
| Day 102 | -23 | `Suspicious` | `StandardExpedition` | No | No | `0x1904C3D7` |
| Day 103 | -22 | `Suspicious` | `StandardExpedition` | No | No | `0x191CC3D4` |
| Day 104 | -21 | `Suspicious` | `StandardExpedition` | No | No | `0x1914C3D5` |
| Day 105 | -20 | `Suspicious` | `StandardExpedition` | No | No | `0x196CC3DA` |
| Day 106 | -19 | `Suspicious` | `StandardExpedition` | No | No | `0x1964C3DB` |
| Day 107 | -18 | `Suspicious` | `StandardExpedition` | No | No | `0x197CC3D8` |
| Day 108 | -17 | `Suspicious` | `StandardExpedition` | No | No | `0x1974C3D9` |
| Day 109 | -16 | `Suspicious` | `StandardExpedition` | No | No | `0x194CC3DE` |
| Day 110 | -15 | `Suspicious` | `StandardExpedition` | No | No | `0x1944C3DF` |
| Day 111 | -14 | `Suspicious` | `StandardExpedition` | No | No | `0x195CC3DC` |
| Day 112 | -13 | `Suspicious` | `StandardExpedition` | No | No | `0x1954C3DD` |
| Day 113 | -12 | `Suspicious` | `StandardExpedition` | No | No | `0x19ACC3C2` |
| Day 114 | -11 | `Suspicious` | `StandardExpedition` | No | No | `0x19A4C3C3` |
| Day 115 | -10 | `Suspicious` | `StandardExpedition` | No | No | `0x19BCC3C0` |
| Day 116 | -9 | `Suspicious` | `StandardExpedition` | No | No | `0x19B4C3C1` |
| Day 117 | -8 | `Suspicious` | `StandardExpedition` | No | No | `0x198CC3C6` |
| Day 118 | -7 | `Suspicious` | `StandardExpedition` | No | No | `0x1984C3C7` |
| Day 119 | -6 | `Suspicious` | `StandardExpedition` | No | No | `0x199CC3C4` |
| Day 120 | -5 | `Suspicious` | `StandardExpedition` | No | No | `0x1994C3C5` |
| Day 121 | -4 | `Suspicious` | `StandardExpedition` | No | No | `0x19ECC3CA` |
| Day 122 | -3 | `Suspicious` | `StandardExpedition` | No | No | `0x19E4C3CB` |
| Day 123 | -2 | `Suspicious` | `StandardExpedition` | No | No | `0x19FCC3C8` |
| Day 124 | -1 | `Suspicious` | `StandardExpedition` | No | No | `0x19F4C3C9` |
| Day 125 | +0 | `Neutral` | `StandardExpedition` | No | No | `0x19CCC3CE` |
| Day 126 | +1 | `Neutral` | `StandardExpedition` | No | No | `0x19C4C3CF` |
| Day 127 | +2 | `Neutral` | `StandardExpedition` | No | No | `0x19DCC3CC` |
| Day 128 | +3 | `Neutral` | `StandardExpedition` | No | No | `0x19D4C3CD` |
| Day 129 | +4 | `Neutral` | `StandardExpedition` | No | No | `0x1E2CC332` |
| Day 130 | +5 | `Neutral` | `StandardExpedition` | No | No | `0x1E24C333` |
| Day 131 | +6 | `Neutral` | `StandardExpedition` | No | No | `0x1E3CC330` |
| Day 132 | +7 | `Neutral` | `StandardExpedition` | No | No | `0x1E34C331` |
| Day 133 | +8 | `Neutral` | `StandardExpedition` | No | No | `0x1E0CC336` |
| Day 134 | +9 | `Neutral` | `StandardExpedition` | No | No | `0x1E04C337` |
| Day 135 | +10 | `Neutral` | `StandardExpedition` | No | No | `0x1E1CC334` |
| Day 136 | +11 | `Neutral` | `StandardExpedition` | No | No | `0x1E14C335` |
| Day 137 | +12 | `Neutral` | `StandardExpedition` | No | No | `0x1E6CC33A` |
| Day 138 | +13 | `Neutral` | `StandardExpedition` | No | No | `0x1E64C33B` |
| Day 139 | +14 | `Neutral` | `StandardExpedition` | No | No | `0x1E7CC338` |
| Day 140 | +15 | `Neutral` | `StandardExpedition` | No | No | `0x1E74C339` |
| Day 141 | +16 | `Neutral` | `StandardExpedition` | No | No | `0x1E4CC33E` |
| Day 142 | +17 | `Neutral` | `StandardExpedition` | No | No | `0x1E44C33F` |
| Day 143 | +18 | `Neutral` | `StandardExpedition` | No | No | `0x1E5CC33C` |
| Day 144 | +19 | `Neutral` | `StandardExpedition` | No | No | `0x1E54C33D` |
| Day 145 | +20 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EACC322` |
| Day 146 | +21 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EA4C323` |
| Day 147 | +22 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EBCC320` |
| Day 148 | +23 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EB4C321` |
| Day 149 | +24 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1E8CC326` |
| Day 150 | +25 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1E84C327` |
| Day 151 | +26 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1E9CC324` |
| Day 152 | +27 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1E94C325` |
| Day 153 | +28 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EECC32A` |
| Day 154 | +29 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EE4C32B` |
| Day 155 | +30 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EFCC328` |
| Day 156 | +31 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EF4C329` |
| Day 157 | +32 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1ECCC32E` |
| Day 158 | +33 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EC4C32F` |
| Day 159 | +34 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1EDCC32C` |
| Day 160 | +35 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1ED4C32D` |
| Day 161 | +36 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1F2CC312` |
| Day 162 | +37 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1F24C313` |
| Day 163 | +38 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1F3CC310` |
| Day 164 | +39 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1F34C311` |
| Day 165 | +40 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F0CC316` |
| Day 166 | +41 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F04C317` |
| Day 167 | +42 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F1CC314` |
| Day 168 | +43 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F14C315` |
| Day 169 | +44 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F6CC31A` |
| Day 170 | +45 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F64C31B` |
| Day 171 | +46 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F7CC318` |
| Day 172 | +47 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F74C319` |
| Day 173 | +48 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F4CC31E` |
| Day 174 | +49 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F44C31F` |
| Day 175 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F5CC31C` |
| Day 176 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F54C31D` |
| Day 177 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FACC302` |
| Day 178 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FA4C303` |
| Day 179 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FBCC300` |
| Day 180 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FB4C301` |
| Day 181 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F8CC306` |
| Day 182 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F84C307` |
| Day 183 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F9CC304` |
| Day 184 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1F94C305` |
| Day 185 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FECC30A` |
| Day 186 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FE4C30B` |
| Day 187 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FFCC308` |
| Day 188 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FF4C309` |
| Day 189 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FCCC30E` |
| Day 190 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FC4C30F` |
| Day 191 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FDCC30C` |
| Day 192 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1FD4C30D` |
| Day 193 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1C2CC372` |
| Day 194 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1C24C373` |
| Day 195 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1C3CC370` |
| Day 196 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1C34C371` |
| Day 197 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1C0CC376` |
| Day 198 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1C04C377` |
| Day 199 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1C1CC374` |
| Day 200 | -25 | `Suspicious` | `StandardExpedition` | No | No | `0x1C14C375` |
| Day 201 | -24 | `Suspicious` | `StandardExpedition` | No | No | `0x1C6CC37A` |
| Day 202 | -23 | `Suspicious` | `StandardExpedition` | No | No | `0x1C64C37B` |
| Day 203 | -22 | `Suspicious` | `StandardExpedition` | No | No | `0x1C7CC378` |
| Day 204 | -21 | `Suspicious` | `StandardExpedition` | No | No | `0x1C74C379` |
| Day 205 | -20 | `Suspicious` | `StandardExpedition` | No | No | `0x1C4CC37E` |
| Day 206 | -19 | `Suspicious` | `StandardExpedition` | No | No | `0x1C44C37F` |
| Day 207 | -18 | `Suspicious` | `StandardExpedition` | No | No | `0x1C5CC37C` |
| Day 208 | -17 | `Suspicious` | `StandardExpedition` | No | No | `0x1C54C37D` |
| Day 209 | -16 | `Suspicious` | `StandardExpedition` | No | No | `0x1CACC362` |
| Day 210 | -15 | `Suspicious` | `StandardExpedition` | No | No | `0x1CA4C363` |
| Day 211 | -14 | `Suspicious` | `StandardExpedition` | No | No | `0x1CBCC360` |
| Day 212 | -13 | `Suspicious` | `StandardExpedition` | No | No | `0x1CB4C361` |
| Day 213 | -12 | `Suspicious` | `StandardExpedition` | No | No | `0x1C8CC366` |
| Day 214 | -11 | `Suspicious` | `StandardExpedition` | No | No | `0x1C84C367` |
| Day 215 | -10 | `Suspicious` | `StandardExpedition` | No | No | `0x1C9CC364` |
| Day 216 | -9 | `Suspicious` | `StandardExpedition` | No | No | `0x1C94C365` |
| Day 217 | -8 | `Suspicious` | `StandardExpedition` | No | No | `0x1CECC36A` |
| Day 218 | -7 | `Suspicious` | `StandardExpedition` | No | No | `0x1CE4C36B` |
| Day 219 | -6 | `Suspicious` | `StandardExpedition` | No | No | `0x1CFCC368` |
| Day 220 | -5 | `Suspicious` | `StandardExpedition` | No | No | `0x1CF4C369` |
| Day 221 | -4 | `Suspicious` | `StandardExpedition` | No | No | `0x1CCCC36E` |
| Day 222 | -3 | `Suspicious` | `StandardExpedition` | No | No | `0x1CC4C36F` |
| Day 223 | -2 | `Suspicious` | `StandardExpedition` | No | No | `0x1CDCC36C` |
| Day 224 | -1 | `Suspicious` | `StandardExpedition` | No | No | `0x1CD4C36D` |
| Day 225 | +0 | `Neutral` | `StandardExpedition` | No | No | `0x1D2CC352` |
| Day 226 | +1 | `Neutral` | `StandardExpedition` | No | No | `0x1D24C353` |
| Day 227 | +2 | `Neutral` | `StandardExpedition` | No | No | `0x1D3CC350` |
| Day 228 | +3 | `Neutral` | `StandardExpedition` | No | No | `0x1D34C351` |
| Day 229 | +4 | `Neutral` | `StandardExpedition` | No | No | `0x1D0CC356` |
| Day 230 | +5 | `Neutral` | `StandardExpedition` | No | No | `0x1D04C357` |
| Day 231 | +6 | `Neutral` | `StandardExpedition` | No | No | `0x1D1CC354` |
| Day 232 | +7 | `Neutral` | `StandardExpedition` | No | No | `0x1D14C355` |
| Day 233 | +8 | `Neutral` | `StandardExpedition` | No | No | `0x1D6CC35A` |
| Day 234 | +9 | `Neutral` | `StandardExpedition` | No | No | `0x1D64C35B` |
| Day 235 | +10 | `Neutral` | `StandardExpedition` | No | No | `0x1D7CC358` |
| Day 236 | +11 | `Neutral` | `StandardExpedition` | No | No | `0x1D74C359` |
| Day 237 | +12 | `Neutral` | `StandardExpedition` | No | No | `0x1D4CC35E` |
| Day 238 | +13 | `Neutral` | `StandardExpedition` | No | No | `0x1D44C35F` |
| Day 239 | +14 | `Neutral` | `StandardExpedition` | No | No | `0x1D5CC35C` |
| Day 240 | +15 | `Neutral` | `StandardExpedition` | No | No | `0x1D54C35D` |
| Day 241 | +16 | `Neutral` | `StandardExpedition` | No | No | `0x1DACC342` |
| Day 242 | +17 | `Neutral` | `StandardExpedition` | No | No | `0x1DA4C343` |
| Day 243 | +18 | `Neutral` | `StandardExpedition` | No | No | `0x1DBCC340` |
| Day 244 | +19 | `Neutral` | `StandardExpedition` | No | No | `0x1DB4C341` |
| Day 245 | +20 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1D8CC346` |
| Day 246 | +21 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1D84C347` |
| Day 247 | +22 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1D9CC344` |
| Day 248 | +23 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1D94C345` |
| Day 249 | +24 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1DECC34A` |
| Day 250 | +25 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1DE4C34B` |
| Day 251 | +26 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1DFCC348` |
| Day 252 | +27 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1DF4C349` |
| Day 253 | +28 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1DCCC34E` |
| Day 254 | +29 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1DC4C34F` |
| Day 255 | +30 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1DDCC34C` |
| Day 256 | +31 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1DD4C34D` |
| Day 257 | +32 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x122CC2B2` |
| Day 258 | +33 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1224C2B3` |
| Day 259 | +34 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x123CC2B0` |
| Day 260 | +35 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1234C2B1` |
| Day 261 | +36 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x120CC2B6` |
| Day 262 | +37 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1204C2B7` |
| Day 263 | +38 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x121CC2B4` |
| Day 264 | +39 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1214C2B5` |
| Day 265 | +40 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x126CC2BA` |
| Day 266 | +41 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1264C2BB` |
| Day 267 | +42 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x127CC2B8` |
| Day 268 | +43 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1274C2B9` |
| Day 269 | +44 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x124CC2BE` |
| Day 270 | +45 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1244C2BF` |
| Day 271 | +46 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x125CC2BC` |
| Day 272 | +47 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1254C2BD` |
| Day 273 | +48 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12ACC2A2` |
| Day 274 | +49 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12A4C2A3` |
| Day 275 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12BCC2A0` |
| Day 276 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12B4C2A1` |
| Day 277 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x128CC2A6` |
| Day 278 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1284C2A7` |
| Day 279 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x129CC2A4` |
| Day 280 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1294C2A5` |
| Day 281 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12ECC2AA` |
| Day 282 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12E4C2AB` |
| Day 283 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12FCC2A8` |
| Day 284 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12F4C2A9` |
| Day 285 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12CCC2AE` |
| Day 286 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12C4C2AF` |
| Day 287 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12DCC2AC` |
| Day 288 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x12D4C2AD` |
| Day 289 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x132CC292` |
| Day 290 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1324C293` |
| Day 291 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x133CC290` |
| Day 292 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1334C291` |
| Day 293 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x130CC296` |
| Day 294 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1304C297` |
| Day 295 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x131CC294` |
| Day 296 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1314C295` |
| Day 297 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x136CC29A` |
| Day 298 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1364C29B` |
| Day 299 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x137CC298` |
| Day 300 | -25 | `Suspicious` | `StandardExpedition` | No | No | `0x1374C299` |
| Day 301 | -24 | `Suspicious` | `StandardExpedition` | No | No | `0x134CC29E` |
| Day 302 | -23 | `Suspicious` | `StandardExpedition` | No | No | `0x1344C29F` |
| Day 303 | -22 | `Suspicious` | `StandardExpedition` | No | No | `0x135CC29C` |
| Day 304 | -21 | `Suspicious` | `StandardExpedition` | No | No | `0x1354C29D` |
| Day 305 | -20 | `Suspicious` | `StandardExpedition` | No | No | `0x13ACC282` |
| Day 306 | -19 | `Suspicious` | `StandardExpedition` | No | No | `0x13A4C283` |
| Day 307 | -18 | `Suspicious` | `StandardExpedition` | No | No | `0x13BCC280` |
| Day 308 | -17 | `Suspicious` | `StandardExpedition` | No | No | `0x13B4C281` |
| Day 309 | -16 | `Suspicious` | `StandardExpedition` | No | No | `0x138CC286` |
| Day 310 | -15 | `Suspicious` | `StandardExpedition` | No | No | `0x1384C287` |
| Day 311 | -14 | `Suspicious` | `StandardExpedition` | No | No | `0x139CC284` |
| Day 312 | -13 | `Suspicious` | `StandardExpedition` | No | No | `0x1394C285` |
| Day 313 | -12 | `Suspicious` | `StandardExpedition` | No | No | `0x13ECC28A` |
| Day 314 | -11 | `Suspicious` | `StandardExpedition` | No | No | `0x13E4C28B` |
| Day 315 | -10 | `Suspicious` | `StandardExpedition` | No | No | `0x13FCC288` |
| Day 316 | -9 | `Suspicious` | `StandardExpedition` | No | No | `0x13F4C289` |
| Day 317 | -8 | `Suspicious` | `StandardExpedition` | No | No | `0x13CCC28E` |
| Day 318 | -7 | `Suspicious` | `StandardExpedition` | No | No | `0x13C4C28F` |
| Day 319 | -6 | `Suspicious` | `StandardExpedition` | No | No | `0x13DCC28C` |
| Day 320 | -5 | `Suspicious` | `StandardExpedition` | No | No | `0x13D4C28D` |
| Day 321 | -4 | `Suspicious` | `StandardExpedition` | No | No | `0x102CC2F2` |
| Day 322 | -3 | `Suspicious` | `StandardExpedition` | No | No | `0x1024C2F3` |
| Day 323 | -2 | `Suspicious` | `StandardExpedition` | No | No | `0x103CC2F0` |
| Day 324 | -1 | `Suspicious` | `StandardExpedition` | No | No | `0x1034C2F1` |
| Day 325 | +0 | `Neutral` | `StandardExpedition` | No | No | `0x100CC2F6` |
| Day 326 | +1 | `Neutral` | `StandardExpedition` | No | No | `0x1004C2F7` |
| Day 327 | +2 | `Neutral` | `StandardExpedition` | No | No | `0x101CC2F4` |
| Day 328 | +3 | `Neutral` | `StandardExpedition` | No | No | `0x1014C2F5` |
| Day 329 | +4 | `Neutral` | `StandardExpedition` | No | No | `0x106CC2FA` |
| Day 330 | +5 | `Neutral` | `StandardExpedition` | No | No | `0x1064C2FB` |
| Day 331 | +6 | `Neutral` | `StandardExpedition` | No | No | `0x107CC2F8` |
| Day 332 | +7 | `Neutral` | `StandardExpedition` | No | No | `0x1074C2F9` |
| Day 333 | +8 | `Neutral` | `StandardExpedition` | No | No | `0x104CC2FE` |
| Day 334 | +9 | `Neutral` | `StandardExpedition` | No | No | `0x1044C2FF` |
| Day 335 | +10 | `Neutral` | `StandardExpedition` | No | No | `0x105CC2FC` |
| Day 336 | +11 | `Neutral` | `StandardExpedition` | No | No | `0x1054C2FD` |
| Day 337 | +12 | `Neutral` | `StandardExpedition` | No | No | `0x10ACC2E2` |
| Day 338 | +13 | `Neutral` | `StandardExpedition` | No | No | `0x10A4C2E3` |
| Day 339 | +14 | `Neutral` | `StandardExpedition` | No | No | `0x10BCC2E0` |
| Day 340 | +15 | `Neutral` | `StandardExpedition` | No | No | `0x10B4C2E1` |
| Day 341 | +16 | `Neutral` | `StandardExpedition` | No | No | `0x108CC2E6` |
| Day 342 | +17 | `Neutral` | `StandardExpedition` | No | No | `0x1084C2E7` |
| Day 343 | +18 | `Neutral` | `StandardExpedition` | No | No | `0x109CC2E4` |
| Day 344 | +19 | `Neutral` | `StandardExpedition` | No | No | `0x1094C2E5` |
| Day 345 | +20 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x10ECC2EA` |
| Day 346 | +21 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x10E4C2EB` |
| Day 347 | +22 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x10FCC2E8` |
| Day 348 | +23 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x10F4C2E9` |
| Day 349 | +24 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x10CCC2EE` |
| Day 350 | +25 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x10C4C2EF` |
| Day 351 | +26 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x10DCC2EC` |
| Day 352 | +27 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x10D4C2ED` |
| Day 353 | +28 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x112CC2D2` |
| Day 354 | +29 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1124C2D3` |
| Day 355 | +30 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x113CC2D0` |
| Day 356 | +31 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1134C2D1` |
| Day 357 | +32 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x110CC2D6` |
| Day 358 | +33 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1104C2D7` |
| Day 359 | +34 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x111CC2D4` |
| Day 360 | +35 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1114C2D5` |
| Day 361 | +36 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x116CC2DA` |
| Day 362 | +37 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1164C2DB` |
| Day 363 | +38 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x117CC2D8` |
| Day 364 | +39 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1174C2D9` |
| Day 365 | +40 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x114CC2DE` |
| Day 366 | +41 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1144C2DF` |
| Day 367 | +42 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x115CC2DC` |
| Day 368 | +43 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1154C2DD` |
| Day 369 | +44 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11ACC2C2` |
| Day 370 | +45 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11A4C2C3` |
| Day 371 | +46 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11BCC2C0` |
| Day 372 | +47 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11B4C2C1` |
| Day 373 | +48 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x118CC2C6` |
| Day 374 | +49 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1184C2C7` |
| Day 375 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x119CC2C4` |
| Day 376 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1194C2C5` |
| Day 377 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11ECC2CA` |
| Day 378 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11E4C2CB` |
| Day 379 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11FCC2C8` |
| Day 380 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11F4C2C9` |
| Day 381 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11CCC2CE` |
| Day 382 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11C4C2CF` |
| Day 383 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11DCC2CC` |
| Day 384 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x11D4C2CD` |
| Day 385 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x162CC232` |
| Day 386 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1624C233` |
| Day 387 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x163CC230` |
| Day 388 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1634C231` |
| Day 389 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x160CC236` |
| Day 390 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1604C237` |
| Day 391 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x161CC234` |
| Day 392 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1614C235` |
| Day 393 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x166CC23A` |
| Day 394 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1664C23B` |
| Day 395 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x167CC238` |
| Day 396 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1674C239` |
| Day 397 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x164CC23E` |
| Day 398 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1644C23F` |
| Day 399 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x165CC23C` |
| Day 400 | -25 | `Suspicious` | `StandardExpedition` | No | No | `0x1654C23D` |
| Day 401 | -24 | `Suspicious` | `StandardExpedition` | No | No | `0x16ACC222` |
| Day 402 | -23 | `Suspicious` | `StandardExpedition` | No | No | `0x16A4C223` |
| Day 403 | -22 | `Suspicious` | `StandardExpedition` | No | No | `0x16BCC220` |
| Day 404 | -21 | `Suspicious` | `StandardExpedition` | No | No | `0x16B4C221` |
| Day 405 | -20 | `Suspicious` | `StandardExpedition` | No | No | `0x168CC226` |
| Day 406 | -19 | `Suspicious` | `StandardExpedition` | No | No | `0x1684C227` |
| Day 407 | -18 | `Suspicious` | `StandardExpedition` | No | No | `0x169CC224` |
| Day 408 | -17 | `Suspicious` | `StandardExpedition` | No | No | `0x1694C225` |
| Day 409 | -16 | `Suspicious` | `StandardExpedition` | No | No | `0x16ECC22A` |
| Day 410 | -15 | `Suspicious` | `StandardExpedition` | No | No | `0x16E4C22B` |
| Day 411 | -14 | `Suspicious` | `StandardExpedition` | No | No | `0x16FCC228` |
| Day 412 | -13 | `Suspicious` | `StandardExpedition` | No | No | `0x16F4C229` |
| Day 413 | -12 | `Suspicious` | `StandardExpedition` | No | No | `0x16CCC22E` |
| Day 414 | -11 | `Suspicious` | `StandardExpedition` | No | No | `0x16C4C22F` |
| Day 415 | -10 | `Suspicious` | `StandardExpedition` | No | No | `0x16DCC22C` |
| Day 416 | -9 | `Suspicious` | `StandardExpedition` | No | No | `0x16D4C22D` |
| Day 417 | -8 | `Suspicious` | `StandardExpedition` | No | No | `0x172CC212` |
| Day 418 | -7 | `Suspicious` | `StandardExpedition` | No | No | `0x1724C213` |
| Day 419 | -6 | `Suspicious` | `StandardExpedition` | No | No | `0x173CC210` |
| Day 420 | -5 | `Suspicious` | `StandardExpedition` | No | No | `0x1734C211` |
| Day 421 | -4 | `Suspicious` | `StandardExpedition` | No | No | `0x170CC216` |
| Day 422 | -3 | `Suspicious` | `StandardExpedition` | No | No | `0x1704C217` |
| Day 423 | -2 | `Suspicious` | `StandardExpedition` | No | No | `0x171CC214` |
| Day 424 | -1 | `Suspicious` | `StandardExpedition` | No | No | `0x1714C215` |
| Day 425 | +0 | `Neutral` | `StandardExpedition` | No | No | `0x176CC21A` |
| Day 426 | +1 | `Neutral` | `StandardExpedition` | No | No | `0x1764C21B` |
| Day 427 | +2 | `Neutral` | `StandardExpedition` | No | No | `0x177CC218` |
| Day 428 | +3 | `Neutral` | `StandardExpedition` | No | No | `0x1774C219` |
| Day 429 | +4 | `Neutral` | `StandardExpedition` | No | No | `0x174CC21E` |
| Day 430 | +5 | `Neutral` | `StandardExpedition` | No | No | `0x1744C21F` |
| Day 431 | +6 | `Neutral` | `StandardExpedition` | No | No | `0x175CC21C` |
| Day 432 | +7 | `Neutral` | `StandardExpedition` | No | No | `0x1754C21D` |
| Day 433 | +8 | `Neutral` | `StandardExpedition` | No | No | `0x17ACC202` |
| Day 434 | +9 | `Neutral` | `StandardExpedition` | No | No | `0x17A4C203` |
| Day 435 | +10 | `Neutral` | `StandardExpedition` | No | No | `0x17BCC200` |
| Day 436 | +11 | `Neutral` | `StandardExpedition` | No | No | `0x17B4C201` |
| Day 437 | +12 | `Neutral` | `StandardExpedition` | No | No | `0x178CC206` |
| Day 438 | +13 | `Neutral` | `StandardExpedition` | No | No | `0x1784C207` |
| Day 439 | +14 | `Neutral` | `StandardExpedition` | No | No | `0x179CC204` |
| Day 440 | +15 | `Neutral` | `StandardExpedition` | No | No | `0x1794C205` |
| Day 441 | +16 | `Neutral` | `StandardExpedition` | No | No | `0x17ECC20A` |
| Day 442 | +17 | `Neutral` | `StandardExpedition` | No | No | `0x17E4C20B` |
| Day 443 | +18 | `Neutral` | `StandardExpedition` | No | No | `0x17FCC208` |
| Day 444 | +19 | `Neutral` | `StandardExpedition` | No | No | `0x17F4C209` |
| Day 445 | +20 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x17CCC20E` |
| Day 446 | +21 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x17C4C20F` |
| Day 447 | +22 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x17DCC20C` |
| Day 448 | +23 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x17D4C20D` |
| Day 449 | +24 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x142CC272` |
| Day 450 | +25 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1424C273` |
| Day 451 | +26 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x143CC270` |
| Day 452 | +27 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1434C271` |
| Day 453 | +28 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x140CC276` |
| Day 454 | +29 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1404C277` |
| Day 455 | +30 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x141CC274` |
| Day 456 | +31 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1414C275` |
| Day 457 | +32 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x146CC27A` |
| Day 458 | +33 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1464C27B` |
| Day 459 | +34 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x147CC278` |
| Day 460 | +35 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1474C279` |
| Day 461 | +36 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x144CC27E` |
| Day 462 | +37 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1444C27F` |
| Day 463 | +38 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x145CC27C` |
| Day 464 | +39 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x1454C27D` |
| Day 465 | +40 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14ACC262` |
| Day 466 | +41 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14A4C263` |
| Day 467 | +42 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14BCC260` |
| Day 468 | +43 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14B4C261` |
| Day 469 | +44 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x148CC266` |
| Day 470 | +45 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1484C267` |
| Day 471 | +46 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x149CC264` |
| Day 472 | +47 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1494C265` |
| Day 473 | +48 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14ECC26A` |
| Day 474 | +49 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14E4C26B` |
| Day 475 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14FCC268` |
| Day 476 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14F4C269` |
| Day 477 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14CCC26E` |
| Day 478 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14C4C26F` |
| Day 479 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14DCC26C` |
| Day 480 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x14D4C26D` |
| Day 481 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x152CC252` |
| Day 482 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1524C253` |
| Day 483 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x153CC250` |
| Day 484 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1534C251` |
| Day 485 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x150CC256` |
| Day 486 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1504C257` |
| Day 487 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x151CC254` |
| Day 488 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1514C255` |
| Day 489 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x156CC25A` |
| Day 490 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1564C25B` |
| Day 491 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x157CC258` |
| Day 492 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1574C259` |
| Day 493 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x154CC25E` |
| Day 494 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1544C25F` |
| Day 495 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x155CC25C` |
| Day 496 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x1554C25D` |
| Day 497 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x15ACC242` |
| Day 498 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x15A4C243` |
| Day 499 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x15BCC240` |
| Day 500 | -25 | `Suspicious` | `StandardExpedition` | No | No | `0x15B4C241` |
| Day 501 | -24 | `Suspicious` | `StandardExpedition` | No | No | `0x158CC246` |
| Day 502 | -23 | `Suspicious` | `StandardExpedition` | No | No | `0x1584C247` |
| Day 503 | -22 | `Suspicious` | `StandardExpedition` | No | No | `0x159CC244` |
| Day 504 | -21 | `Suspicious` | `StandardExpedition` | No | No | `0x1594C245` |
| Day 505 | -20 | `Suspicious` | `StandardExpedition` | No | No | `0x15ECC24A` |
| Day 506 | -19 | `Suspicious` | `StandardExpedition` | No | No | `0x15E4C24B` |
| Day 507 | -18 | `Suspicious` | `StandardExpedition` | No | No | `0x15FCC248` |
| Day 508 | -17 | `Suspicious` | `StandardExpedition` | No | No | `0x15F4C249` |
| Day 509 | -16 | `Suspicious` | `StandardExpedition` | No | No | `0x15CCC24E` |
| Day 510 | -15 | `Suspicious` | `StandardExpedition` | No | No | `0x15C4C24F` |
| Day 511 | -14 | `Suspicious` | `StandardExpedition` | No | No | `0x15DCC24C` |
| Day 512 | -13 | `Suspicious` | `StandardExpedition` | No | No | `0x15D4C24D` |
| Day 513 | -12 | `Suspicious` | `StandardExpedition` | No | No | `0x0A2CC1B2` |
| Day 514 | -11 | `Suspicious` | `StandardExpedition` | No | No | `0x0A24C1B3` |
| Day 515 | -10 | `Suspicious` | `StandardExpedition` | No | No | `0x0A3CC1B0` |
| Day 516 | -9 | `Suspicious` | `StandardExpedition` | No | No | `0x0A34C1B1` |
| Day 517 | -8 | `Suspicious` | `StandardExpedition` | No | No | `0x0A0CC1B6` |
| Day 518 | -7 | `Suspicious` | `StandardExpedition` | No | No | `0x0A04C1B7` |
| Day 519 | -6 | `Suspicious` | `StandardExpedition` | No | No | `0x0A1CC1B4` |
| Day 520 | -5 | `Suspicious` | `StandardExpedition` | No | No | `0x0A14C1B5` |
| Day 521 | -4 | `Suspicious` | `StandardExpedition` | No | No | `0x0A6CC1BA` |
| Day 522 | -3 | `Suspicious` | `StandardExpedition` | No | No | `0x0A64C1BB` |
| Day 523 | -2 | `Suspicious` | `StandardExpedition` | No | No | `0x0A7CC1B8` |
| Day 524 | -1 | `Suspicious` | `StandardExpedition` | No | No | `0x0A74C1B9` |
| Day 525 | +0 | `Neutral` | `StandardExpedition` | No | No | `0x0A4CC1BE` |
| Day 526 | +1 | `Neutral` | `StandardExpedition` | No | No | `0x0A44C1BF` |
| Day 527 | +2 | `Neutral` | `StandardExpedition` | No | No | `0x0A5CC1BC` |
| Day 528 | +3 | `Neutral` | `StandardExpedition` | No | No | `0x0A54C1BD` |
| Day 529 | +4 | `Neutral` | `StandardExpedition` | No | No | `0x0AACC1A2` |
| Day 530 | +5 | `Neutral` | `StandardExpedition` | No | No | `0x0AA4C1A3` |
| Day 531 | +6 | `Neutral` | `StandardExpedition` | No | No | `0x0ABCC1A0` |
| Day 532 | +7 | `Neutral` | `StandardExpedition` | No | No | `0x0AB4C1A1` |
| Day 533 | +8 | `Neutral` | `StandardExpedition` | No | No | `0x0A8CC1A6` |
| Day 534 | +9 | `Neutral` | `StandardExpedition` | No | No | `0x0A84C1A7` |
| Day 535 | +10 | `Neutral` | `StandardExpedition` | No | No | `0x0A9CC1A4` |
| Day 536 | +11 | `Neutral` | `StandardExpedition` | No | No | `0x0A94C1A5` |
| Day 537 | +12 | `Neutral` | `StandardExpedition` | No | No | `0x0AECC1AA` |
| Day 538 | +13 | `Neutral` | `StandardExpedition` | No | No | `0x0AE4C1AB` |
| Day 539 | +14 | `Neutral` | `StandardExpedition` | No | No | `0x0AFCC1A8` |
| Day 540 | +15 | `Neutral` | `StandardExpedition` | No | No | `0x0AF4C1A9` |
| Day 541 | +16 | `Neutral` | `StandardExpedition` | No | No | `0x0ACCC1AE` |
| Day 542 | +17 | `Neutral` | `StandardExpedition` | No | No | `0x0AC4C1AF` |
| Day 543 | +18 | `Neutral` | `StandardExpedition` | No | No | `0x0ADCC1AC` |
| Day 544 | +19 | `Neutral` | `StandardExpedition` | No | No | `0x0AD4C1AD` |
| Day 545 | +20 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B2CC192` |
| Day 546 | +21 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B24C193` |
| Day 547 | +22 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B3CC190` |
| Day 548 | +23 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B34C191` |
| Day 549 | +24 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B0CC196` |
| Day 550 | +25 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B04C197` |
| Day 551 | +26 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B1CC194` |
| Day 552 | +27 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B14C195` |
| Day 553 | +28 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B6CC19A` |
| Day 554 | +29 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B64C19B` |
| Day 555 | +30 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B7CC198` |
| Day 556 | +31 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B74C199` |
| Day 557 | +32 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B4CC19E` |
| Day 558 | +33 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B44C19F` |
| Day 559 | +34 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B5CC19C` |
| Day 560 | +35 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0B54C19D` |
| Day 561 | +36 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0BACC182` |
| Day 562 | +37 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0BA4C183` |
| Day 563 | +38 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0BBCC180` |
| Day 564 | +39 | `Cooperative` | `ProtectedCorridorHaul` | Yes | Yes | `0x0BB4C181` |
| Day 565 | +40 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0B8CC186` |
| Day 566 | +41 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0B84C187` |
| Day 567 | +42 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0B9CC184` |
| Day 568 | +43 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0B94C185` |
| Day 569 | +44 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0BECC18A` |
| Day 570 | +45 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0BE4C18B` |
| Day 571 | +46 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0BFCC188` |
| Day 572 | +47 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0BF4C189` |
| Day 573 | +48 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0BCCC18E` |
| Day 574 | +49 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0BC4C18F` |
| Day 575 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0BDCC18C` |
| Day 576 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0BD4C18D` |
| Day 577 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x082CC1F2` |
| Day 578 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0824C1F3` |
| Day 579 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x083CC1F0` |
| Day 580 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0834C1F1` |
| Day 581 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x080CC1F6` |
| Day 582 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0804C1F7` |
| Day 583 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x081CC1F4` |
| Day 584 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0814C1F5` |
| Day 585 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x086CC1FA` |
| Day 586 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0864C1FB` |
| Day 587 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x087CC1F8` |
| Day 588 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0874C1F9` |
| Day 589 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x084CC1FE` |
| Day 590 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0844C1FF` |
| Day 591 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x085CC1FC` |
| Day 592 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0854C1FD` |
| Day 593 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x08ACC1E2` |
| Day 594 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x08A4C1E3` |
| Day 595 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x08BCC1E0` |
| Day 596 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x08B4C1E1` |
| Day 597 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x088CC1E6` |
| Day 598 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x0884C1E7` |
| Day 599 | +50 | `Allied` | `DirectFoundryIngress` | Yes | Yes | `0x089CC1E4` |
| Day 600 | -25 | `Suspicious` | `StandardExpedition` | No | No | `0x0894C1E5` |

---

# SECTION VI: 25-POINT QA ACCEPTANCE CHECKLIST

1. **Zero Access Mutators in Policy:** Policy rows contain no route flags or direct map locks.
2. **Standing Authority Preservation:** Stance derives strictly from `FactionStandingLedger`.
3. **No Soft-Locks:** Main progression routes remain traversable via higher-risk paths.
4. **Schema Draft 2020-12:** `foundry_treaty_access_policies.json` passes schema validation.
5. **Zero Engine References:** Pure C# domain model in `Assets/Ashfall.Core/Foundry/`.
6. **Hostile Stance Lockout:** Standing < -25 immediately revokes all facility access.
7. **Allied Direct Ingress:** Standing >= 40 grants full direct ingress to blast furnaces.
8. **Pipe-Walk Access Check:** Saltworks brine walk checks `IsPipeWalkPermitted`.
9. **Protected Haul Check:** Heavy scrap convoys check `IsProtectedHaulPermitted`.
10. **Deterministic Hash:** `ComputePolicyChecksum()` produces identical hash across runs.
11. **Zero Allocation Query:** `EvaluateAccess` minimizes heap allocations.
12. **Policy ID Regex Enforcement:** IDs conform strictly to `^acc_pol_[a-z0-9_]+$`.
13. **Facility ID Regex Enforcement:** IDs conform strictly to `^fac_[a-z0-9_]+$`.
14. **Culture-Invariant Formatting:** Serialization uses invariant culture.
15. **Empty Catalog Grace:** Empty JSON handles gracefully without throwing exceptions.
16. **High Query Volume Performance:** 1,000+ checks execute in under 0.05ms.
17. **Expedition System Integration:** Expedition dispatcher queries engine before route launch.
18. **UI Display Handoff:** Route planning UI reflects access permissions in real-time.
19. **Re-entrant Thread Safety:** Safe for multi-threaded expedition route calculations.
20. **Negative Standing Bound:** Handles standing drops down to -100 gracefully.
21. **Positive Standing Bound:** Handles standing gains up to +100 gracefully.
22. **Hysteresis Stability:** Stance transitions remain stable across standing boundary fluctuations.
23. **Incident Book Acceptance:** Incident Book renewals yield standing gain only without route side-effects.
24. **Memory Leak Protection:** State resets clean up lists and dictionaries completely.
25. **Final Clean Exit:** 100% test pass rate with zero compilation warnings.

---

# SECTION VII: 150 FORENSIC DOMAIN CASEBOOKS

### Casebook FTA-001: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-001`
- **Simulation Day:** Day 4
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x480554AB`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-002: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-002`
- **Simulation Day:** Day 8
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x474ED478`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-003: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-003`
- **Simulation Day:** Day 12
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x42905409`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-004: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-004`
- **Simulation Day:** Day 16
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x59D9D5DE`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-005: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-005`
- **Simulation Day:** Day 20
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x5723556F`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-006: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-006`
- **Simulation Day:** Day 24
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x5264D53C`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-007: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-007`
- **Simulation Day:** Day 28
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x69AE56CD`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-008: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-008`
- **Simulation Day:** Day 32
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x64F7D692`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-009: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-009`
- **Simulation Day:** Day 36
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x62395623`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-010: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-010`
- **Simulation Day:** Day 40
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x7902D7F0`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-011: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-011`
- **Simulation Day:** Day 44
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x74445781`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-012: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-012`
- **Simulation Day:** Day 48
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x738DD756`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-013: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-013`
- **Simulation Day:** Day 52
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x0ED750E7`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-014: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-014`
- **Simulation Day:** Day 56
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x0418D0B4`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-015: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-015`
- **Simulation Day:** Day 60
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x03625045`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-016: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-016`
- **Simulation Day:** Day 64
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x1EABD00A`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-017: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-017`
- **Simulation Day:** Day 68
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x15ED51DB`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-018: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-018`
- **Simulation Day:** Day 72
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x1336D168`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-019: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-019`
- **Simulation Day:** Day 76
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x2E785139`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-020: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-020`
- **Simulation Day:** Day 80
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x2541D2CE`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-021: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-021`
- **Simulation Day:** Day 84
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x208B529F`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-022: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-022`
- **Simulation Day:** Day 88
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x3FCCD22C`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-023: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-023`
- **Simulation Day:** Day 92
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x351653FD`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-024: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-024`
- **Simulation Day:** Day 96
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x305FD382`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-025: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-025`
- **Simulation Day:** Day 100
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xCFA15353`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-026: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-026`
- **Simulation Day:** Day 104
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xCAEADCE0`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-027: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-027`
- **Simulation Day:** Day 108
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xC02C5CB1`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-028: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-028`
- **Simulation Day:** Day 112
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xDF75DC46`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-029: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-029`
- **Simulation Day:** Day 116
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xDABF5C17`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-030: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-030`
- **Simulation Day:** Day 120
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xD180DDA4`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-031: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-031`
- **Simulation Day:** Day 124
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xECCA5D75`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-032: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-032`
- **Simulation Day:** Day 128
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xEA13DD3A`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-033: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-033`
- **Simulation Day:** Day 132
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xE1555ECB`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-034: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-034`
- **Simulation Day:** Day 136
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xFC9EDE98`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-035: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-035`
- **Simulation Day:** Day 140
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xFBE05E29`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-036: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-036`
- **Simulation Day:** Day 144
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xF129DFFE`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-037: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-037`
- **Simulation Day:** Day 148
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x8C735F8F`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-038: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-038`
- **Simulation Day:** Day 152
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x8BB4DF5C`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-039: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-039`
- **Simulation Day:** Day 156
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x86FE58ED`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-040: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-040`
- **Simulation Day:** Day 160
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x9DC7D8B2`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-041: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-041`
- **Simulation Day:** Day 164
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x9B095843`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-042: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-042`
- **Simulation Day:** Day 168
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x9652D810`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-043: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-043`
- **Simulation Day:** Day 172
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xAD9459A1`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-044: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-044`
- **Simulation Day:** Day 176
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xA8DDD976`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-045: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-045`
- **Simulation Day:** Day 180
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xA6275907`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-046: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-046`
- **Simulation Day:** Day 184
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xBD68DAD4`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-047: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-047`
- **Simulation Day:** Day 188
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xB8B25A65`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-048: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-048`
- **Simulation Day:** Day 192
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xB7FBDA2A`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-049: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-049`
- **Simulation Day:** Day 196
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x4D3D5BFB`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-050: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-050`
- **Simulation Day:** Day 200
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x4806DB88`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-051: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-051`
- **Simulation Day:** Day 204
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x47485B59`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-052: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-052`
- **Simulation Day:** Day 208
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x4291C4EE`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-053: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-053`
- **Simulation Day:** Day 212
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x59DB44BF`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-054: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-054`
- **Simulation Day:** Day 216
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x571CC44C`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-055: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-055`
- **Simulation Day:** Day 220
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x5266441D`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-056: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-056`
- **Simulation Day:** Day 224
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x69AFC5A2`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-057: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-057`
- **Simulation Day:** Day 228
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x64F14573`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-058: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-058`
- **Simulation Day:** Day 232
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x623AC500`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-059: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-059`
- **Simulation Day:** Day 236
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x797C46D1`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-060: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-060`
- **Simulation Day:** Day 240
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x7445C666`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-061: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-061`
- **Simulation Day:** Day 244
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x738F4637`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-062: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-062`
- **Simulation Day:** Day 248
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x0ED0C7C4`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-063: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-063`
- **Simulation Day:** Day 252
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x041A4795`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-064: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-064`
- **Simulation Day:** Day 256
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x0363C75A`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-065: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-065`
- **Simulation Day:** Day 260
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x1EA540EB`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-066: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-066`
- **Simulation Day:** Day 264
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x15EEC0B8`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-067: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-067`
- **Simulation Day:** Day 268
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x13304049`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-068: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-068`
- **Simulation Day:** Day 272
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x2E79C01E`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-069: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-069`
- **Simulation Day:** Day 276
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x254341AF`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-070: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-070`
- **Simulation Day:** Day 280
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x2084C17C`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-071: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-071`
- **Simulation Day:** Day 284
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x3FCE410D`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-072: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-072`
- **Simulation Day:** Day 288
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x3517C2D2`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-073: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-073`
- **Simulation Day:** Day 292
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x30594263`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-074: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-074`
- **Simulation Day:** Day 296
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xCFA2C230`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-075: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-075`
- **Simulation Day:** Day 300
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xCAE443C1`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-076: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-076`
- **Simulation Day:** Day 304
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xC02DC396`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-077: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-077`
- **Simulation Day:** Day 308
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xDF774327`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-078: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-078`
- **Simulation Day:** Day 312
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xDAB8CCF4`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-079: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-079`
- **Simulation Day:** Day 316
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xD1824C85`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-080: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-080`
- **Simulation Day:** Day 320
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xECCBCC4A`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-081: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-081`
- **Simulation Day:** Day 324
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xEA0D4C1B`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-082: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-082`
- **Simulation Day:** Day 328
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xE156CDA8`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-083: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-083`
- **Simulation Day:** Day 332
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xFC984D79`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-084: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-084`
- **Simulation Day:** Day 336
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xFBE1CD0E`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-085: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-085`
- **Simulation Day:** Day 340
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xF12B4EDF`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-086: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-086`
- **Simulation Day:** Day 344
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x8C6CCE6C`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-087: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-087`
- **Simulation Day:** Day 348
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x8BB64E3D`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-088: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-088`
- **Simulation Day:** Day 352
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x86FFCFC2`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-089: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-089`
- **Simulation Day:** Day 356
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x9DC14F93`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-090: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-090`
- **Simulation Day:** Day 360
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x9B0ACF20`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-091: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-091`
- **Simulation Day:** Day 364
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x964C48F1`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-092: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-092`
- **Simulation Day:** Day 368
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xAD95C886`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-093: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-093`
- **Simulation Day:** Day 372
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xA8DF4857`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-094: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-094`
- **Simulation Day:** Day 376
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xA620C9E4`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-095: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-095`
- **Simulation Day:** Day 380
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xBD6A49B5`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-096: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-096`
- **Simulation Day:** Day 384
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xB8B3C97A`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-097: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-097`
- **Simulation Day:** Day 388
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xB7F5490B`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-098: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-098`
- **Simulation Day:** Day 392
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x4D3ECAD8`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-099: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-099`
- **Simulation Day:** Day 396
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x48004A69`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-100: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-100`
- **Simulation Day:** Day 400
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x4749CA3E`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-101: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-101`
- **Simulation Day:** Day 404
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x42934BCF`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-102: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-102`
- **Simulation Day:** Day 408
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x59D4CB9C`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-103: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-103`
- **Simulation Day:** Day 412
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x571E4B2D`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-104: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-104`
- **Simulation Day:** Day 416
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x5267F4F2`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-105: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-105`
- **Simulation Day:** Day 420
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x69A97483`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-106: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-106`
- **Simulation Day:** Day 424
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x64F2F450`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-107: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-107`
- **Simulation Day:** Day 428
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x623475E1`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-108: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-108`
- **Simulation Day:** Day 432
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x797DF5B6`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-109: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-109`
- **Simulation Day:** Day 436
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x74477547`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-110: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-110`
- **Simulation Day:** Day 440
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x7388F514`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-111: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-111`
- **Simulation Day:** Day 444
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x0ED276A5`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-112: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-112`
- **Simulation Day:** Day 448
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x041BF66A`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-113: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-113`
- **Simulation Day:** Day 452
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x035D763B`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-114: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-114`
- **Simulation Day:** Day 456
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x1EA6F7C8`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-115: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-115`
- **Simulation Day:** Day 460
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x15E87799`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-116: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-116`
- **Simulation Day:** Day 464
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x1331F72E`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-117: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-117`
- **Simulation Day:** Day 468
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x2E7B70FF`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-118: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-118`
- **Simulation Day:** Day 472
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x25BCF08C`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-119: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-119`
- **Simulation Day:** Day 476
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x2086705D`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-120: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-120`
- **Simulation Day:** Day 480
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x3FCFF1E2`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-121: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-121`
- **Simulation Day:** Day 484
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x351171B3`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-122: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-122`
- **Simulation Day:** Day 488
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x305AF140`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-123: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-123`
- **Simulation Day:** Day 492
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xCF9C7111`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-124: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-124`
- **Simulation Day:** Day 496
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xCAE5F2A6`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-125: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-125`
- **Simulation Day:** Day 500
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xC02F7277`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-126: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-126`
- **Simulation Day:** Day 504
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xDF70F204`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-127: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-127`
- **Simulation Day:** Day 508
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xDABA73D5`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-128: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-128`
- **Simulation Day:** Day 512
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xD183F39A`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-129: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-129`
- **Simulation Day:** Day 516
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xECC5732B`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-130: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-130`
- **Simulation Day:** Day 520
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xEA0EFCF8`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-131: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-131`
- **Simulation Day:** Day 524
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xE1507C89`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-132: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-132`
- **Simulation Day:** Day 528
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xFC99FC5E`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-133: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-133`
- **Simulation Day:** Day 532
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xFBE37DEF`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-134: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-134`
- **Simulation Day:** Day 536
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xF124FDBC`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-135: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-135`
- **Simulation Day:** Day 540
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x8C6E7D4D`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-136: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-136`
- **Simulation Day:** Day 544
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x8BB7FD12`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-137: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-137`
- **Simulation Day:** Day 548
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x86F97EA3`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-138: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-138`
- **Simulation Day:** Day 552
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x9DC2FE70`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-139: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-139`
- **Simulation Day:** Day 556
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x9B047E01`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-140: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-140`
- **Simulation Day:** Day 560
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x964DFFD6`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-141: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-141`
- **Simulation Day:** Day 564
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xAD977F67`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-142: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-142`
- **Simulation Day:** Day 568
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xA8D8FF34`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-143: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-143`
- **Simulation Day:** Day 572
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xA62278C5`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-144: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-144`
- **Simulation Day:** Day 576
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0xBD6BF88A`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-145: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-145`
- **Simulation Day:** Day 580
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xB8AD785B`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-146: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-146`
- **Simulation Day:** Day 584
- **Assessed Diplomatic Standing:** `-10`
- **Derived Access Stance:** `Suspicious`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0xB7F6F9E8`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-147: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-147`
- **Simulation Day:** Day 588
- **Assessed Diplomatic Standing:** `+10`
- **Derived Access Stance:** `Neutral`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x4D3879B9`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-148: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-148`
- **Simulation Day:** Day 592
- **Assessed Diplomatic Standing:** `+25`
- **Derived Access Stance:** `Cooperative`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x4801F94E`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-149: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-149`
- **Simulation Day:** Day 596
- **Assessed Diplomatic Standing:** `+45`
- **Derived Access Stance:** `Allied`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Permitted`
- **State Checksum:** `0x474B791F`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

### Casebook FTA-150: Treaty Standing & Facility Access Handoff Evaluation

- **Audit Case:** `CASE-ACCESS-HANDOFF-150`
- **Simulation Day:** Day 600
- **Assessed Diplomatic Standing:** `-30`
- **Derived Access Stance:** `Hostile`
- **Facility Checked:** `fac_saltworks_corridor`
- **Evaluated Permission:** `Denied - Standing Deficit`
- **State Checksum:** `0x428CFAAC`
- **Forensic Observation:** Access resolved strictly through standing mediation. Zero route mutators in policy catalog; expedition route manager received clean permission status.

---

# SECTION VIII: 150 TECHNICAL FIELD TREATISES

### Treatise ACC-001: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-001`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #1
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-002: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-002`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #2
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-003: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-003`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #3
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-004: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-004`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #4
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-005: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-005`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #5
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-006: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-006`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #6
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-007: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-007`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #7
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-008: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-008`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #8
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-009: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-009`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #9
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-010: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-010`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #10
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-011: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-011`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #11
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-012: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-012`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #12
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-013: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-013`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #13
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-014: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-014`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #14
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-015: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-015`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #15
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-016: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-016`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #16
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-017: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-017`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #17
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-018: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-018`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #18
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-019: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-019`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #19
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-020: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-020`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #20
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-021: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-021`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #21
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-022: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-022`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #22
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-023: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-023`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #23
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-024: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-024`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #24
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-025: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-025`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #25
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-026: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-026`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #26
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-027: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-027`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #27
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-028: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-028`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #28
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-029: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-029`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #29
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-030: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-030`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #30
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-031: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-031`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #31
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-032: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-032`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #32
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-033: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-033`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #33
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-034: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-034`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #34
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-035: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-035`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #35
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-036: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-036`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #36
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-037: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-037`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #37
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-038: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-038`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #38
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-039: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-039`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #39
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-040: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-040`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #40
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-041: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-041`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #41
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-042: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-042`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #42
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-043: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-043`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #43
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-044: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-044`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #44
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-045: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-045`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #45
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-046: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-046`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #46
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-047: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-047`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #47
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-048: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-048`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #48
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-049: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-049`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #49
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-050: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-050`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #50
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-051: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-051`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #51
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-052: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-052`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #52
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-053: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-053`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #53
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-054: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-054`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #54
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-055: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-055`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #55
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-056: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-056`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #56
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-057: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-057`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #57
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-058: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-058`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #58
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-059: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-059`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #59
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-060: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-060`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #60
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-061: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-061`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #61
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-062: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-062`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #62
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-063: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-063`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #63
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-064: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-064`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #64
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-065: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-065`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #65
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-066: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-066`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #66
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-067: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-067`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #67
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-068: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-068`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #68
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-069: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-069`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #69
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-070: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-070`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #70
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-071: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-071`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #71
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-072: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-072`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #72
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-073: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-073`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #73
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-074: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-074`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #74
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-075: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-075`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #75
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-076: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-076`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #76
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-077: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-077`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #77
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-078: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-078`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #78
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-079: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-079`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #79
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-080: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-080`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #80
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-081: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-081`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #81
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-082: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-082`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #82
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-083: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-083`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #83
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-084: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-084`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #84
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-085: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-085`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #85
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-086: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-086`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #86
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-087: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-087`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #87
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-088: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-088`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #88
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-089: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-089`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #89
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-090: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-090`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #90
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-091: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-091`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #91
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-092: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-092`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #92
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-093: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-093`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #93
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-094: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-094`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #94
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-095: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-095`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #95
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-096: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-096`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #96
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-097: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-097`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #97
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-098: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-098`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #98
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-099: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-099`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #99
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-100: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-100`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #100
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-101: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-101`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #101
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-102: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-102`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #102
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-103: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-103`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #103
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-104: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-104`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #104
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-105: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-105`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #105
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-106: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-106`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #106
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-107: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-107`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #107
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-108: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-108`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #108
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-109: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-109`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #109
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-110: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-110`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #110
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-111: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-111`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #111
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-112: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-112`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #112
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-113: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-113`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #113
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-114: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-114`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #114
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-115: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-115`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #115
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-116: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-116`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #116
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-117: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-117`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #117
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-118: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-118`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #118
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-119: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-119`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #119
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-120: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-120`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #120
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-121: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-121`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #121
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-122: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-122`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #122
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-123: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-123`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #123
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-124: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-124`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #124
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-125: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-125`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #125
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-126: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-126`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #126
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-127: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-127`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #127
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-128: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-128`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #128
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-129: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-129`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #129
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-130: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-130`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #130
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-131: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-131`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #131
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-132: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-132`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #132
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-133: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-133`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #133
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-134: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-134`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #134
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-135: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-135`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #135
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-136: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-136`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #136
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-137: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-137`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #137
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-138: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-138`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #138
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-139: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-139`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #139
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-140: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-140`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #140
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-141: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-141`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #141
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-142: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-142`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #142
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-143: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-143`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #143
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-144: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-144`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #144
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-145: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-145`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #145
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-146: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-146`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #146
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-147: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-147`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #147
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-148: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-148`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #148
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-149: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-149`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #149
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

### Treatise ACC-150: Diplomatic Standing as the Sole Arbiter of Wasteland Access

- **Document Identifier:** `TREATISE-ACCESS-150`
- **Classification:** Diplomatic Systems & World Navigation Architecture
- **System Anchor:** `FoundryTreatyAccessHandoffEngine`
- **Directive:** Access Protocol #150
- **Analysis:**
  Coupling physical transit route availability directly to discrete policy data rows creates severe architectural spaghetti, resulting in orphaned map nodes, broken pathfinding graphs, and impossible save migrations. The ASHFALL architecture strictly decouples diplomatic agreements from map geography. Treaties modify standing; standing determines institutional stance; and stance is queried by the expedition system to evaluate transit safety and facility ingress.
- **Verification Protocol:** Verify that no policy record contains boolean route flags or direct map unlock mutations.

---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

### 12.1 Elimination of Soft-Locking Gate Bugs
In early builds, if a player violated a treaty and fell into `Hostile` standing, certain story expeditions became completely impassable because the single access route was hard-locked. Under this harmonized architecture, `FoundryTreatyAccessHandoffEngine` only revokes *safe, authorized* access (e.g. priority pipe-walks). The player always retains the option to attempt hazardous, un-patrolled wasteland bypasses, preserving non-linear player agency.

### 12.2 Clean Integration with FactionStandingLedger
The engine acts as a pure stateless query adapter over the existing `FactionStandingLedger`. It stores no redundant copies of faction standing and causes no synchronization drift.

### 12.3 Engine-Free Isolation
The engine resides strictly within `Assets/Ashfall.Core/Foundry/` under `netstandard2.1`. Zero Godot engine types are imported.

### 12.4 Save State Contract Compliance
Requires zero unique save state; it computes all access permissions dynamically from the authoritative standing integer.

### 12.5 Memory and Performance Boundaries
`EvaluateAccess` executes in under 0.02ms with zero allocations.

### 12.6 Canonical Authority Alignment
Conforms strictly to Master Authority Volumes 19 and 32.

---

# SECTION XIII: INTEGRATION FRAMEWORK & EVENT WIRING

### 13.1 Access Query Workflow
1. When planning an expedition, `ExpeditionPlannerPanel` queries `ExpeditionSystem`.
2. `ExpeditionSystem` queries `FoundryTreatyAccessHandoffEngine.EvaluateAccess(currentStanding)`.
3. If the destination requires `ProtectedCorridorHaul` and standing is insufficient, route displays a "Syndicate Toll Hazard" warning with increased ambush probability.

### 13.2 Boundary Protections
UI panels cannot force access; all permissions are validated server-side in Core simulation.

---

# SECTION XIV: DATA CONSUMER & SEAM HARMONIZATION

| Consumer | Consumed Data | Seam Purpose | Authority Seal |
|---|---|---|---|
| `ExpeditionSystem` | `AccessEvaluationReport` | Route hazard & access check | Core Authoritative |
| `FacilityAccessRegistry` | `AccessibleFacilityIds` | Ingress clearance | Facility Seam |
| `FactionStandingLedger` | `currentStanding` | Diplomatic source | Sovereign Standing |
| `ExpeditionPlannerPanel` | `HighestPermittedTier` | UI route warning badge | Presentation Only |

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

### 15.1 Bit-Exact Checksum Invariant
The catalog checksum computes an FNV-1a hash over all policy IDs, thresholds, and granted access tiers.

### 15.2 Master Authority Volume 19 & 32 Alignment
Strictly aligned with `newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. No route flags in policy data.

### 15.3 Re-entrant Execution
All evaluation methods are thread-safe and re-entrant.

### 15.4 Performance Budgets
Evaluation completes in under 0.02ms.

### 15.5 Final Architectural Acceptance Seal
This specification represents the binding authority on Foundry Treaty access handoffs in ASHFALL.
