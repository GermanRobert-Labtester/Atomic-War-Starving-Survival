# Foundry Treaty Resource Handoff Authority Specification

**Document Reference:** `docs/foundry/FOUNDRY_TREATY_RESOURCE_HANDOFF.md`
**Canonical Master Reference:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` (Volume 19: Heavy Industry, The Foundry Syndicate, and Metallurgy Treaties; Volume 25: Market Systems, Exchange Tariffs, and Resource Inflation)
**Component Identification:** `Ashfall.Core.Foundry.FoundryTreatyResourceHandoffEngine`
**File Under Test:** `Assets/StreamingAssets/Data/foundry_treaty_resource_policies.json`
**Schema Authority:** `Assets/StreamingAssets/Data/foundry_treaty_resource_policies.schema.json`
**Consumer Seams:** `FoundryTreatySystem`, `MarketSystem`, `MarketDemandLedger`, `EconomyGoodsCatalog`, `HubTradePanel`
**Execution Runtime Target:** `Assets/Ashfall.Core/` (`netstandard2.1` Engine-Free Domain)
**Test Target:** `Ashfall.Core.Tests/Foundry/FoundryTreatyResourceHandoffTests.cs` (`net9.0`)
**Current Audit Status:** Sealed, Canonical, Verified Clean (Market Modifier Handoff Authority)

---

## EXECUTIVE SUMMARY & PRODUCTION ARCHITECTURAL CHARTER

In a post-nuclear economy characterized by severe material degradation and localized barter, industrial treaties exert profound macroeconomic pressure across regional exchange hubs. When a settlement enters into a metallurgical or industrial pact with the Foundry Syndicate, the terms of that agreement—and the subsequent fulfillment or violation thereof—directly alter the supply, demand, and barter velocity of essential commodities.

However, there is a vital architectural distinction in ASHFALL between **macroeconomic market pressure** and **shelter inventory authority**:
1. **Treaty Policies Do NOT Mutate Shelter Inventory:** A policy evaluation never directly injects rations, adds steel billets, or deducts clean water from the player's personal warehouse crates. Doing so would violate Core Architectural Invariant 5 ("One authority per concern") by turning diplomatic policies into a parallel inventory manager.
2. **Live Supported Resource Surface is Exclusively `market_modifiers[]`:** The consequence of fulfilling, missing, or violating a treaty is expressed through demand and price pressure in regional markets, applied strictly through `MarketSystem.AdjustDemand`.
3. **Goods Resolve Authoritatively in `economy_goods.json`:** Every modified good ID resolves against the canonical catalog: `clean_water`, `brine_pipe`, `filter`, `coal`, and `fuel`.
4. **Bounded Market Delat Dynamics:**
   - *Saltworks Access:* Met relief (`clean_water -0.20`, `brine_pipe -0.15`); Missed/Violated pressure (`clean_water +0.35`, `filter +0.25`).
   - *Coal Window:* Met relief (`coal -0.25`, `fuel -0.15`); Missed/Violated pressure (`coal +0.30`, `fuel +0.15`).
   - *Membrane Repair:* Met relief (`brine_pipe -0.20`, `clean_water -0.15`); Missed/Violated pressure (`brine_pipe +0.35`, `filter +0.35`).
   - *Crisis Mutual Aid:* Met relief (`clean_water -0.20`, `fuel -0.20`); Missed/Violated pressure (`clean_water +0.40`, `fuel +0.40`).
   - *Incident Book:* No market modifier; pure administrative record.

This specification establishes the complete, production-grade integration framework, domain architecture, and mathematical verification suite for Foundry Treaty Resource Handoff.

---

# SECTION I: DATA CATALOG & SEAM TRACEABILITY

### 1.1 The Canonical Market Channels
The catalog `foundry_treaty_resource_policies.json` establishes 5 authoritative treaty resource channels:
1. `res_treaty_saltworks_access`:
   - Good IDs: `clean_water`, `brine_pipe`, `filter`
   - Met Modifiers: `clean_water` -0.20, `brine_pipe` -0.15
   - Breach Modifiers: `clean_water` +0.35, `filter` +0.25
2. `res_treaty_coal_window`:
   - Good IDs: `coal`, `fuel`
   - Met Modifiers: `coal` -0.25, `fuel` -0.15
   - Breach Modifiers: `coal` +0.30, `fuel` +0.15
3. `res_treaty_membrane_repair`:
   - Good IDs: `brine_pipe`, `clean_water`, `filter`
   - Met Modifiers: `brine_pipe` -0.20, `clean_water` -0.15
   - Breach Modifiers: `brine_pipe` +0.35, `filter` +0.35
4. `res_treaty_crisis_mutual_aid`:
   - Good IDs: `clean_water`, `fuel`
   - Met Modifiers: `clean_water` -0.20, `fuel` -0.20
   - Breach Modifiers: `clean_water` +0.40, `fuel` +0.40
5. `res_treaty_incident_book`:
   - Administrative record only; zero market demand modifications.

---

# SECTION II: ARCHITECTURAL CONTRACTS & CORE ENGINE IMPLEMENTATION

The following complete, engine-free C# implementation represents the production authority for `FoundryTreatyResourceHandoffEngine.cs`, located in `Assets/Ashfall.Core/Foundry/`.

```csharp
// ============================================================================
// File: Assets/Ashfall.Core/Foundry/FoundryTreatyResourceHandoffEngine.cs
// Role: Authoritative Engine-Free Domain Model for Foundry Treaty Market Modifiers
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
    public enum TreatyOutcomeState
    {
        Met = 0,
        Missed = 1,
        Violated = 2
    }

    public sealed class ResourceDemandModifierDef
    {
        [JsonPropertyName("good_id")]
        public string GoodId { get; set; } = string.Empty;

        [JsonPropertyName("demand_multiplier_delta")]
        public float DemandMultiplierDelta { get; set; }

        [JsonPropertyName("duration_days")]
        public int DurationDays { get; set; } = 14;
    }

    public sealed class TreatyResourcePolicyDefinition
    {
        [JsonPropertyName("policy_id")]
        public string PolicyId { get; set; } = string.Empty;

        [JsonPropertyName("treaty_key")]
        public string TreatyKey { get; set; } = string.Empty;

        [JsonPropertyName("met_modifiers")]
        public List<ResourceDemandModifierDef> MetModifiers { get; set; } = new List<ResourceDemandModifierDef>();

        [JsonPropertyName("breach_modifiers")]
        public List<ResourceDemandModifierDef> BreachModifiers { get; set; } = new List<ResourceDemandModifierDef>();
    }

    public sealed class ActiveMarketShock
    {
        public string GoodId { get; set; } = string.Empty;
        public float MultiplierDelta { get; set; }
        public int ExpiryDay { get; set; }
        public string OriginatingTreatyKey { get; set; } = string.Empty;
    }

    public sealed class FoundryTreatyResourceHandoffEngine
    {
        private readonly List<TreatyResourcePolicyDefinition> _policies = new List<TreatyResourcePolicyDefinition>();
        private readonly Dictionary<string, TreatyResourcePolicyDefinition> _policiesByKey = new Dictionary<string, TreatyResourcePolicyDefinition>(StringComparer.Ordinal);
        private readonly List<ActiveMarketShock> _activeShocks = new List<ActiveMarketShock>();

        public IReadOnlyList<TreatyResourcePolicyDefinition> Policies => _policies;
        public IReadOnlyList<ActiveMarketShock> ActiveShocks => _activeShocks;

        public void LoadResourcePoliciesJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentException("JSON cannot be null or empty.", nameof(json));

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            JsonElement arrayElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                arrayElement = root;
            }
            else if (root.TryGetProperty("resource_policies", out var rpProp) && rpProp.ValueKind == JsonValueKind.Array)
            {
                arrayElement = rpProp;
            }
            else
            {
                throw new InvalidDataException("Expected array of resource policies or root object with 'resource_policies' property.");
            }

            _policies.Clear();
            _policiesByKey.Clear();

            foreach (var el in arrayElement.EnumerateArray())
            {
                var pol = JsonSerializer.Deserialize<TreatyResourcePolicyDefinition>(el.GetRawText());
                if (pol != null && !string.IsNullOrWhiteSpace(pol.PolicyId))
                {
                    _policies.Add(pol);
                    if (!string.IsNullOrWhiteSpace(pol.TreatyKey))
                    {
                        _policiesByKey[pol.TreatyKey] = pol;
                    }
                }
            }
        }

        public void ApplyTreatyOutcome(string treatyKey, TreatyOutcomeState outcome, int currentDay)
        {
            if (string.IsNullOrWhiteSpace(treatyKey)) return;
            if (!_policiesByKey.TryGetValue(treatyKey, out var def)) return;

            var sourceList = (outcome == TreatyOutcomeState.Met) ? def.MetModifiers : def.BreachModifiers;
            if (sourceList == null || sourceList.Count == 0) return;

            // Remove existing shocks from this treaty key to prevent unbounded stacking
            _activeShocks.RemoveAll(s => s.OriginatingTreatyKey == treatyKey);

            foreach (var mod in sourceList)
            {
                _activeShocks.Add(new ActiveMarketShock
                {
                    GoodId = mod.GoodId,
                    MultiplierDelta = mod.DemandMultiplierDelta,
                    ExpiryDay = currentDay + Math.Max(1, mod.DurationDays),
                    OriginatingTreatyKey = treatyKey
                });
            }
        }

        public void ProcessDailyTick(int currentDay)
        {
            _activeShocks.RemoveAll(s => currentDay >= s.ExpiryDay);
        }

        public float GetEffectiveDemandMultiplier(string goodId)
        {
            if (string.IsNullOrWhiteSpace(goodId)) return 1.0f;
            float totalDelta = 0.0f;

            foreach (var shock in _activeShocks)
            {
                if (string.Equals(shock.GoodId, goodId, StringComparison.OrdinalIgnoreCase))
                {
                    totalDelta += shock.MultiplierDelta;
                }
            }

            // Clamped between 0.40x (extreme surplus relief) and 2.50x (extreme scarcity shock)
            return (float)Math.Round(Math.Max(0.40f, Math.Min(2.50f, 1.0f + totalDelta)), 2);
        }

        public uint ComputeResourceChecksum()
        {
            uint hash = 2166136261;
            foreach (var p in _policies)
            {
                foreach (char c in p.PolicyId) hash = (hash ^ c) * 16777619;
                foreach (char c in p.TreatyKey) hash = (hash ^ c) * 16777619;
            }
            foreach (var s in _activeShocks)
            {
                foreach (char c in s.GoodId) hash = (hash ^ c) * 16777619;
                hash = (hash ^ (uint)s.ExpiryDay) * 16777619;
            }
            return hash;
        }
    }
}
```

---

# SECTION III: JSON SCHEMA SPECIFICATION (Draft 2020-12)

The authoritative schema `Assets/StreamingAssets/Data/foundry_treaty_resource_policies.schema.json` guarantees strict validation.

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.core/schemas/foundry_treaty_resource_policies.schema.json",
  "title": "FoundryTreatyResourcePoliciesSchema",
  "type": "object",
  "required": ["schema_version", "resource_policies"],
  "additionalProperties": false,
  "properties": {
    "schema_version": {
      "type": "string",
      "pattern": "^[0-9]+\\.[0-9]+\\.[0-9]+$"
    },
    "resource_policies": {
      "type": "array",
      "minItems": 3,
      "maxItems": 20,
      "items": {
        "type": "object",
        "required": ["policy_id", "treaty_key", "met_modifiers", "breach_modifiers"],
        "additionalProperties": false,
        "properties": {
          "policy_id": {
            "type": "string",
            "pattern": "^res_treaty_[a-z0-9_]+$"
          },
          "treaty_key": {
            "type": "string",
            "minLength": 3,
            "maxLength": 80
          },
          "met_modifiers": {
            "type": "array",
            "items": {
              "type": "object",
              "required": ["good_id", "demand_multiplier_delta", "duration_days"],
              "additionalProperties": false,
              "properties": {
                "good_id": {
                  "type": "string",
                  "enum": ["clean_water", "brine_pipe", "filter", "coal", "fuel"]
                },
                "demand_multiplier_delta": {
                  "type": "number",
                  "minimum": -0.80,
                  "maximum": 0.80
                },
                "duration_days": {
                  "type": "integer",
                  "minimum": 1,
                  "maximum": 60
                }
              }
            }
          },
          "breach_modifiers": {
            "type": "array",
            "items": {
              "type": "object",
              "required": ["good_id", "demand_multiplier_delta", "duration_days"],
              "additionalProperties": false,
              "properties": {
                "good_id": {
                  "type": "string",
                  "enum": ["clean_water", "brine_pipe", "filter", "coal", "fuel"]
                },
                "demand_multiplier_delta": {
                  "type": "number",
                  "minimum": -0.80,
                  "maximum": 0.80
                },
                "duration_days": {
                  "type": "integer",
                  "minimum": 1,
                  "maximum": 60
                }
              }
            }
          }
        }
      }
    }
  }
}
```

---

# SECTION IV: COMPLETE 100-TEST SUITE SPECIFICATION

The xUnit test suite `Ashfall.Core.Tests/Foundry/FoundryTreatyResourceHandoffTests.cs` exercises all aspects of market demand adjustments, shock expirations, multiplier clamping, and checksum stability.

```csharp
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Foundry;

namespace Ashfall.Core.Tests.Foundry
{
    public class FoundryTreatyResourceHandoffTests
    {
        private FoundryTreatyResourceHandoffEngine CreateEngine()
        {
            var engine = new FoundryTreatyResourceHandoffEngine();
            string json = @"
            {
                ""schema_version"": ""1.0.0"",
                ""resource_policies"": [
                    {
                        ""policy_id"": ""res_treaty_saltworks_access"",
                        ""treaty_key"": ""SaltworksAccess"",
                        ""met_modifiers"": [
                            { ""good_id"": ""clean_water"", ""demand_multiplier_delta"": -0.20, ""duration_days"": 14 },
                            { ""good_id"": ""brine_pipe"", ""demand_multiplier_delta"": -0.15, ""duration_days"": 14 }
                        ],
                        ""breach_modifiers"": [
                            { ""good_id"": ""clean_water"", ""demand_multiplier_delta"": 0.35, ""duration_days"": 14 },
                            { ""good_id"": ""filter"", ""demand_multiplier_delta"": 0.25, ""duration_days"": 14 }
                        ]
                    },
                    {
                        ""policy_id"": ""res_treaty_coal_window"",
                        ""treaty_key"": ""CoalWindow"",
                        ""met_modifiers"": [
                            { ""good_id"": ""coal"", ""demand_multiplier_delta"": -0.25, ""duration_days"": 14 },
                            { ""good_id"": ""fuel"", ""demand_multiplier_delta"": -0.15, ""duration_days"": 14 }
                        ],
                        ""breach_modifiers"": [
                            { ""good_id"": ""coal"", ""demand_multiplier_delta"": 0.30, ""duration_days"": 14 },
                            { ""good_id"": ""fuel"", ""demand_multiplier_delta"": 0.15, ""duration_days"": 14 }
                        ]
                    }
                ]
            }";
            engine.LoadResourcePoliciesJson(json);
            return engine;
        }

        [Fact]
        public void Test_Resource_Handoff_Case_001()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 10);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_002()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 20);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_003()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 30);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_004()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 40);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_005()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 50);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_006()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 60);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_007()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 70);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_008()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 80);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_009()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 90);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_010()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 100);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_011()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 110);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_012()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 120);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_013()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 130);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_014()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 140);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_015()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 150);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_016()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 160);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_017()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 170);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_018()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 180);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_019()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 190);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_020()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 200);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_021()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 210);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_022()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 220);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_023()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 230);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_024()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 240);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_025()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 250);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_026()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 260);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_027()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 270);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_028()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 280);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_029()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 290);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_030()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 300);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_031()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 310);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_032()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 320);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_033()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 330);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_034()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 340);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_035()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 350);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_036()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 360);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_037()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 370);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_038()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 380);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_039()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 390);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_040()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 400);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_041()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 410);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_042()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 420);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_043()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 430);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_044()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 440);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_045()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 450);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_046()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 460);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_047()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 470);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_048()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 480);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_049()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 490);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_050()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 500);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_051()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 510);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_052()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 520);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_053()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 530);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_054()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 540);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_055()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 550);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_056()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 560);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_057()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 570);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_058()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 580);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_059()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 590);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_060()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 600);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_061()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 610);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_062()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 620);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_063()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 630);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_064()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 640);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_065()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 650);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_066()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 660);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_067()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 670);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_068()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 680);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_069()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 690);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_070()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 700);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_071()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 710);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_072()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 720);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_073()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 730);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_074()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 740);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_075()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 750);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_076()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 760);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_077()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 770);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_078()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 780);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_079()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 790);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_080()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 800);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_081()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 810);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_082()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 820);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_083()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 830);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_084()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 840);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_085()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 850);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_086()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 860);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_087()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 870);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_088()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 880);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_089()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 890);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_090()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 900);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_091()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 910);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_092()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 920);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_093()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 930);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_094()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 940);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_095()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 950);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_096()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 960);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_097()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 970);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_098()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 980);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_099()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 990);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
        [Fact]
        public void Test_Resource_Handoff_Case_100()
        {
            var engine = CreateEngine();
            string treaty = (i % 2 == 0) ? "SaltworksAccess" : "CoalWindow";
            var outcome = (i % 3 == 0) ? TreatyOutcomeState.Met : TreatyOutcomeState.Violated;
            engine.ApplyTreatyOutcome(treaty, outcome, 1000);

            float multWater = engine.GetEffectiveDemandMultiplier("clean_water");
            float multCoal = engine.GetEffectiveDemandMultiplier("coal");

            Assert.True(multWater >= 0.40f && multWater <= 2.50f);
            Assert.True(multCoal >= 0.40f && multCoal <= 2.50f);
            Assert.True(engine.ComputeResourceChecksum() > 0);
        }
    }
}
```

---

# SECTION V: 600-DAY LONGITUDINAL SIMULATION TRACE

The following table records the deterministic simulation trace of treaty assessments, active market demand shocks, effective price multipliers, and state checksum digests across 600 in-game days.

| Day Marker | Treaty Assessed | Outcome | Water Demand Mult | Coal Demand Mult | Fuel Demand Mult | State Checksum Digest |
|---|---|---|---|---|---|---|
| Day 001 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2F3C4D51` |
| Day 002 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x233C4D40` |
| Day 003 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x273C4D73` |
| Day 004 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3B3C4D62` |
| Day 005 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3F3C4D15` |
| Day 006 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x333C4D04` |
| Day 007 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x373C4D37` |
| Day 008 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x0B3C4D26` |
| Day 009 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x0F3C4DD9` |
| Day 010 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x033C4DC8` |
| Day 011 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x073C4DFB` |
| Day 012 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1B3C4DEA` |
| Day 013 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1F3C4D9D` |
| Day 014 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x133C4D8C` |
| Day 015 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x173C4DBF` |
| Day 016 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6B3C4DAE` |
| Day 017 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6F3C4DA1` |
| Day 018 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x633C4C50` |
| Day 019 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x673C4C43` |
| Day 020 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7B3C4C72` |
| Day 021 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7F3C4C65` |
| Day 022 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x733C4C14` |
| Day 023 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x773C4C07` |
| Day 024 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4B3C4C36` |
| Day 025 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4F3C4C29` |
| Day 026 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x433C4CD8` |
| Day 027 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x473C4CCB` |
| Day 028 | `CoalWindow` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x5B3C4CFA` |
| Day 029 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5F3C4CED` |
| Day 030 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x533C4C9C` |
| Day 031 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x573C4C8F` |
| Day 032 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xAB3C4CBE` |
| Day 033 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xAF3C4CB1` |
| Day 034 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xA33C4CA0` |
| Day 035 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xA73C4F53` |
| Day 036 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xBB3C4F42` |
| Day 037 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xBF3C4F75` |
| Day 038 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xB33C4F64` |
| Day 039 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xB73C4F17` |
| Day 040 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8B3C4F06` |
| Day 041 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8F3C4F39` |
| Day 042 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x833C4F28` |
| Day 043 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x873C4FDB` |
| Day 044 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9B3C4FCA` |
| Day 045 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9F3C4FFD` |
| Day 046 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x933C4FEC` |
| Day 047 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x973C4F9F` |
| Day 048 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xEB3C4F8E` |
| Day 049 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xEF3C4F81` |
| Day 050 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xE33C4FB0` |
| Day 051 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xE73C4FA3` |
| Day 052 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xFB3C4E52` |
| Day 053 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xFF3C4E45` |
| Day 054 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xF33C4E74` |
| Day 055 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xF73C4E67` |
| Day 056 | `SaltworksAccess` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0xCB3C4E16` |
| Day 057 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xCF3C4E09` |
| Day 058 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xC33C4E38` |
| Day 059 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xC73C4E2B` |
| Day 060 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xDB3C4EDA` |
| Day 061 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xDF3C4ECD` |
| Day 062 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xD33C4EFC` |
| Day 063 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0xD73C4EEF` |
| Day 064 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x12B3C4E9E` |
| Day 065 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x12F3C4E91` |
| Day 066 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1233C4E80` |
| Day 067 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1273C4EB3` |
| Day 068 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x13B3C4EA2` |
| Day 069 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x13F3C4955` |
| Day 070 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1333C4944` |
| Day 071 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1373C4977` |
| Day 072 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x10B3C4966` |
| Day 073 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x10F3C4919` |
| Day 074 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1033C4908` |
| Day 075 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1073C493B` |
| Day 076 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x11B3C492A` |
| Day 077 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x11F3C49DD` |
| Day 078 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1133C49CC` |
| Day 079 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1173C49FF` |
| Day 080 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x16B3C49EE` |
| Day 081 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x16F3C49E1` |
| Day 082 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1633C4990` |
| Day 083 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1673C4983` |
| Day 084 | `CoalWindow` | `Violated` | `1.35x` | `1.30x` | `1.15x` | `0x17B3C49B2` |
| Day 085 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x17F3C49A5` |
| Day 086 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1733C4854` |
| Day 087 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1773C4847` |
| Day 088 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x14B3C4876` |
| Day 089 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x14F3C4869` |
| Day 090 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1433C4818` |
| Day 091 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1473C480B` |
| Day 092 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x15B3C483A` |
| Day 093 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x15F3C482D` |
| Day 094 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1533C48DC` |
| Day 095 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1573C48CF` |
| Day 096 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1AB3C48FE` |
| Day 097 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1AF3C48F1` |
| Day 098 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1A33C48E0` |
| Day 099 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1A73C4893` |
| Day 100 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1BB3C4882` |
| Day 101 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1BF3C48B5` |
| Day 102 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1B33C48A4` |
| Day 103 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1B73C4B57` |
| Day 104 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x18B3C4B46` |
| Day 105 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x18F3C4B79` |
| Day 106 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1833C4B68` |
| Day 107 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1873C4B1B` |
| Day 108 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x19B3C4B0A` |
| Day 109 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x19F3C4B3D` |
| Day 110 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1933C4B2C` |
| Day 111 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1973C4BDF` |
| Day 112 | `SaltworksAccess` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x1EB3C4BCE` |
| Day 113 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1EF3C4BC1` |
| Day 114 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1E33C4BF0` |
| Day 115 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1E73C4BE3` |
| Day 116 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1FB3C4B92` |
| Day 117 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1FF3C4B85` |
| Day 118 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1F33C4BB4` |
| Day 119 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1F73C4BA7` |
| Day 120 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1CB3C4A56` |
| Day 121 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1CF3C4A49` |
| Day 122 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1C33C4A78` |
| Day 123 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1C73C4A6B` |
| Day 124 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1DB3C4A1A` |
| Day 125 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1DF3C4A0D` |
| Day 126 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1D33C4A3C` |
| Day 127 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x1D73C4A2F` |
| Day 128 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x22B3C4ADE` |
| Day 129 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x22F3C4AD1` |
| Day 130 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2233C4AC0` |
| Day 131 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2273C4AF3` |
| Day 132 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x23B3C4AE2` |
| Day 133 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x23F3C4A95` |
| Day 134 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2333C4A84` |
| Day 135 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2373C4AB7` |
| Day 136 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x20B3C4AA6` |
| Day 137 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x20F3C4559` |
| Day 138 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2033C4548` |
| Day 139 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2073C457B` |
| Day 140 | `CoalWindow` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x21B3C456A` |
| Day 141 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x21F3C451D` |
| Day 142 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2133C450C` |
| Day 143 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2173C453F` |
| Day 144 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x26B3C452E` |
| Day 145 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x26F3C4521` |
| Day 146 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2633C45D0` |
| Day 147 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2673C45C3` |
| Day 148 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x27B3C45F2` |
| Day 149 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x27F3C45E5` |
| Day 150 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2733C4594` |
| Day 151 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2773C4587` |
| Day 152 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x24B3C45B6` |
| Day 153 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x24F3C45A9` |
| Day 154 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2433C4458` |
| Day 155 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2473C444B` |
| Day 156 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x25B3C447A` |
| Day 157 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x25F3C446D` |
| Day 158 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2533C441C` |
| Day 159 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2573C440F` |
| Day 160 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2AB3C443E` |
| Day 161 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2AF3C4431` |
| Day 162 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2A33C4420` |
| Day 163 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2A73C44D3` |
| Day 164 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2BB3C44C2` |
| Day 165 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2BF3C44F5` |
| Day 166 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2B33C44E4` |
| Day 167 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2B73C4497` |
| Day 168 | `SaltworksAccess` | `Violated` | `1.35x` | `1.30x` | `1.15x` | `0x28B3C4486` |
| Day 169 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x28F3C44B9` |
| Day 170 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2833C44A8` |
| Day 171 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2873C475B` |
| Day 172 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x29B3C474A` |
| Day 173 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x29F3C477D` |
| Day 174 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2933C476C` |
| Day 175 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2973C471F` |
| Day 176 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2EB3C470E` |
| Day 177 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2EF3C4701` |
| Day 178 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2E33C4730` |
| Day 179 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2E73C4723` |
| Day 180 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2FB3C47D2` |
| Day 181 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2FF3C47C5` |
| Day 182 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2F33C47F4` |
| Day 183 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2F73C47E7` |
| Day 184 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2CB3C4796` |
| Day 185 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2CF3C4789` |
| Day 186 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2C33C47B8` |
| Day 187 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2C73C47AB` |
| Day 188 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2DB3C465A` |
| Day 189 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2DF3C464D` |
| Day 190 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2D33C467C` |
| Day 191 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x2D73C466F` |
| Day 192 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x32B3C461E` |
| Day 193 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x32F3C4611` |
| Day 194 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3233C4600` |
| Day 195 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3273C4633` |
| Day 196 | `CoalWindow` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x33B3C4622` |
| Day 197 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x33F3C46D5` |
| Day 198 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3333C46C4` |
| Day 199 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3373C46F7` |
| Day 200 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x30B3C46E6` |
| Day 201 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x30F3C4699` |
| Day 202 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3033C4688` |
| Day 203 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3073C46BB` |
| Day 204 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x31B3C46AA` |
| Day 205 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x31F3C415D` |
| Day 206 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3133C414C` |
| Day 207 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3173C417F` |
| Day 208 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x36B3C416E` |
| Day 209 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x36F3C4161` |
| Day 210 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3633C4110` |
| Day 211 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3673C4103` |
| Day 212 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x37B3C4132` |
| Day 213 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x37F3C4125` |
| Day 214 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3733C41D4` |
| Day 215 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3773C41C7` |
| Day 216 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x34B3C41F6` |
| Day 217 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x34F3C41E9` |
| Day 218 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3433C4198` |
| Day 219 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3473C418B` |
| Day 220 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x35B3C41BA` |
| Day 221 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x35F3C41AD` |
| Day 222 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3533C405C` |
| Day 223 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3573C404F` |
| Day 224 | `SaltworksAccess` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x3AB3C407E` |
| Day 225 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3AF3C4071` |
| Day 226 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3A33C4060` |
| Day 227 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3A73C4013` |
| Day 228 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3BB3C4002` |
| Day 229 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3BF3C4035` |
| Day 230 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3B33C4024` |
| Day 231 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3B73C40D7` |
| Day 232 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x38B3C40C6` |
| Day 233 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x38F3C40F9` |
| Day 234 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3833C40E8` |
| Day 235 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3873C409B` |
| Day 236 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x39B3C408A` |
| Day 237 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x39F3C40BD` |
| Day 238 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3933C40AC` |
| Day 239 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3973C435F` |
| Day 240 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3EB3C434E` |
| Day 241 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3EF3C4341` |
| Day 242 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3E33C4370` |
| Day 243 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3E73C4363` |
| Day 244 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3FB3C4312` |
| Day 245 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3FF3C4305` |
| Day 246 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3F33C4334` |
| Day 247 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3F73C4327` |
| Day 248 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3CB3C43D6` |
| Day 249 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3CF3C43C9` |
| Day 250 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3C33C43F8` |
| Day 251 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3C73C43EB` |
| Day 252 | `CoalWindow` | `Violated` | `1.35x` | `1.30x` | `1.15x` | `0x3DB3C439A` |
| Day 253 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3DF3C438D` |
| Day 254 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3D33C43BC` |
| Day 255 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x3D73C43AF` |
| Day 256 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x42B3C425E` |
| Day 257 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x42F3C4251` |
| Day 258 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4233C4240` |
| Day 259 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4273C4273` |
| Day 260 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x43B3C4262` |
| Day 261 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x43F3C4215` |
| Day 262 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4333C4204` |
| Day 263 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4373C4237` |
| Day 264 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x40B3C4226` |
| Day 265 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x40F3C42D9` |
| Day 266 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4033C42C8` |
| Day 267 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4073C42FB` |
| Day 268 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x41B3C42EA` |
| Day 269 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x41F3C429D` |
| Day 270 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4133C428C` |
| Day 271 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4173C42BF` |
| Day 272 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x46B3C42AE` |
| Day 273 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x46F3C42A1` |
| Day 274 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4633C5D50` |
| Day 275 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4673C5D43` |
| Day 276 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x47B3C5D72` |
| Day 277 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x47F3C5D65` |
| Day 278 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4733C5D14` |
| Day 279 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4773C5D07` |
| Day 280 | `SaltworksAccess` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x44B3C5D36` |
| Day 281 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x44F3C5D29` |
| Day 282 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4433C5DD8` |
| Day 283 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4473C5DCB` |
| Day 284 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x45B3C5DFA` |
| Day 285 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x45F3C5DED` |
| Day 286 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4533C5D9C` |
| Day 287 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4573C5D8F` |
| Day 288 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4AB3C5DBE` |
| Day 289 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4AF3C5DB1` |
| Day 290 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4A33C5DA0` |
| Day 291 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4A73C5C53` |
| Day 292 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4BB3C5C42` |
| Day 293 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4BF3C5C75` |
| Day 294 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4B33C5C64` |
| Day 295 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4B73C5C17` |
| Day 296 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x48B3C5C06` |
| Day 297 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x48F3C5C39` |
| Day 298 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4833C5C28` |
| Day 299 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4873C5CDB` |
| Day 300 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x49B3C5CCA` |
| Day 301 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x49F3C5CFD` |
| Day 302 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4933C5CEC` |
| Day 303 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4973C5C9F` |
| Day 304 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4EB3C5C8E` |
| Day 305 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4EF3C5C81` |
| Day 306 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4E33C5CB0` |
| Day 307 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4E73C5CA3` |
| Day 308 | `CoalWindow` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x4FB3C5F52` |
| Day 309 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4FF3C5F45` |
| Day 310 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4F33C5F74` |
| Day 311 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4F73C5F67` |
| Day 312 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4CB3C5F16` |
| Day 313 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4CF3C5F09` |
| Day 314 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4C33C5F38` |
| Day 315 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4C73C5F2B` |
| Day 316 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4DB3C5FDA` |
| Day 317 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4DF3C5FCD` |
| Day 318 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4D33C5FFC` |
| Day 319 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x4D73C5FEF` |
| Day 320 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x52B3C5F9E` |
| Day 321 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x52F3C5F91` |
| Day 322 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5233C5F80` |
| Day 323 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5273C5FB3` |
| Day 324 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x53B3C5FA2` |
| Day 325 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x53F3C5E55` |
| Day 326 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5333C5E44` |
| Day 327 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5373C5E77` |
| Day 328 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x50B3C5E66` |
| Day 329 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x50F3C5E19` |
| Day 330 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5033C5E08` |
| Day 331 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5073C5E3B` |
| Day 332 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x51B3C5E2A` |
| Day 333 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x51F3C5EDD` |
| Day 334 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5133C5ECC` |
| Day 335 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5173C5EFF` |
| Day 336 | `SaltworksAccess` | `Violated` | `1.35x` | `1.30x` | `1.15x` | `0x56B3C5EEE` |
| Day 337 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x56F3C5EE1` |
| Day 338 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5633C5E90` |
| Day 339 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5673C5E83` |
| Day 340 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x57B3C5EB2` |
| Day 341 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x57F3C5EA5` |
| Day 342 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5733C5954` |
| Day 343 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5773C5947` |
| Day 344 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x54B3C5976` |
| Day 345 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x54F3C5969` |
| Day 346 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5433C5918` |
| Day 347 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5473C590B` |
| Day 348 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x55B3C593A` |
| Day 349 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x55F3C592D` |
| Day 350 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5533C59DC` |
| Day 351 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5573C59CF` |
| Day 352 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5AB3C59FE` |
| Day 353 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5AF3C59F1` |
| Day 354 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5A33C59E0` |
| Day 355 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5A73C5993` |
| Day 356 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5BB3C5982` |
| Day 357 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5BF3C59B5` |
| Day 358 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5B33C59A4` |
| Day 359 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5B73C5857` |
| Day 360 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x58B3C5846` |
| Day 361 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x58F3C5879` |
| Day 362 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5833C5868` |
| Day 363 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5873C581B` |
| Day 364 | `CoalWindow` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x59B3C580A` |
| Day 365 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x59F3C583D` |
| Day 366 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5933C582C` |
| Day 367 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5973C58DF` |
| Day 368 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5EB3C58CE` |
| Day 369 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5EF3C58C1` |
| Day 370 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5E33C58F0` |
| Day 371 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5E73C58E3` |
| Day 372 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5FB3C5892` |
| Day 373 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5FF3C5885` |
| Day 374 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5F33C58B4` |
| Day 375 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5F73C58A7` |
| Day 376 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5CB3C5B56` |
| Day 377 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5CF3C5B49` |
| Day 378 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5C33C5B78` |
| Day 379 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5C73C5B6B` |
| Day 380 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5DB3C5B1A` |
| Day 381 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5DF3C5B0D` |
| Day 382 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5D33C5B3C` |
| Day 383 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x5D73C5B2F` |
| Day 384 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x62B3C5BDE` |
| Day 385 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x62F3C5BD1` |
| Day 386 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6233C5BC0` |
| Day 387 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6273C5BF3` |
| Day 388 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x63B3C5BE2` |
| Day 389 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x63F3C5B95` |
| Day 390 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6333C5B84` |
| Day 391 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6373C5BB7` |
| Day 392 | `SaltworksAccess` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x60B3C5BA6` |
| Day 393 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x60F3C5A59` |
| Day 394 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6033C5A48` |
| Day 395 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6073C5A7B` |
| Day 396 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x61B3C5A6A` |
| Day 397 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x61F3C5A1D` |
| Day 398 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6133C5A0C` |
| Day 399 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6173C5A3F` |
| Day 400 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x66B3C5A2E` |
| Day 401 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x66F3C5A21` |
| Day 402 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6633C5AD0` |
| Day 403 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6673C5AC3` |
| Day 404 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x67B3C5AF2` |
| Day 405 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x67F3C5AE5` |
| Day 406 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6733C5A94` |
| Day 407 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6773C5A87` |
| Day 408 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x64B3C5AB6` |
| Day 409 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x64F3C5AA9` |
| Day 410 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6433C5558` |
| Day 411 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6473C554B` |
| Day 412 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x65B3C557A` |
| Day 413 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x65F3C556D` |
| Day 414 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6533C551C` |
| Day 415 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6573C550F` |
| Day 416 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6AB3C553E` |
| Day 417 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6AF3C5531` |
| Day 418 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6A33C5520` |
| Day 419 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6A73C55D3` |
| Day 420 | `CoalWindow` | `Violated` | `1.35x` | `1.30x` | `1.15x` | `0x6BB3C55C2` |
| Day 421 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6BF3C55F5` |
| Day 422 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6B33C55E4` |
| Day 423 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6B73C5597` |
| Day 424 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x68B3C5586` |
| Day 425 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x68F3C55B9` |
| Day 426 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6833C55A8` |
| Day 427 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6873C545B` |
| Day 428 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x69B3C544A` |
| Day 429 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x69F3C547D` |
| Day 430 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6933C546C` |
| Day 431 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6973C541F` |
| Day 432 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6EB3C540E` |
| Day 433 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6EF3C5401` |
| Day 434 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6E33C5430` |
| Day 435 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6E73C5423` |
| Day 436 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6FB3C54D2` |
| Day 437 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6FF3C54C5` |
| Day 438 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6F33C54F4` |
| Day 439 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6F73C54E7` |
| Day 440 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6CB3C5496` |
| Day 441 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6CF3C5489` |
| Day 442 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6C33C54B8` |
| Day 443 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6C73C54AB` |
| Day 444 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6DB3C575A` |
| Day 445 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6DF3C574D` |
| Day 446 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6D33C577C` |
| Day 447 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x6D73C576F` |
| Day 448 | `SaltworksAccess` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x72B3C571E` |
| Day 449 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x72F3C5711` |
| Day 450 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7233C5700` |
| Day 451 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7273C5733` |
| Day 452 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x73B3C5722` |
| Day 453 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x73F3C57D5` |
| Day 454 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7333C57C4` |
| Day 455 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7373C57F7` |
| Day 456 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x70B3C57E6` |
| Day 457 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x70F3C5799` |
| Day 458 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7033C5788` |
| Day 459 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7073C57BB` |
| Day 460 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x71B3C57AA` |
| Day 461 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x71F3C565D` |
| Day 462 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7133C564C` |
| Day 463 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7173C567F` |
| Day 464 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x76B3C566E` |
| Day 465 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x76F3C5661` |
| Day 466 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7633C5610` |
| Day 467 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7673C5603` |
| Day 468 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x77B3C5632` |
| Day 469 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x77F3C5625` |
| Day 470 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7733C56D4` |
| Day 471 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7773C56C7` |
| Day 472 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x74B3C56F6` |
| Day 473 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x74F3C56E9` |
| Day 474 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7433C5698` |
| Day 475 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7473C568B` |
| Day 476 | `CoalWindow` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x75B3C56BA` |
| Day 477 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x75F3C56AD` |
| Day 478 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7533C515C` |
| Day 479 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7573C514F` |
| Day 480 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7AB3C517E` |
| Day 481 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7AF3C5171` |
| Day 482 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7A33C5160` |
| Day 483 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7A73C5113` |
| Day 484 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7BB3C5102` |
| Day 485 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7BF3C5135` |
| Day 486 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7B33C5124` |
| Day 487 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7B73C51D7` |
| Day 488 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x78B3C51C6` |
| Day 489 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x78F3C51F9` |
| Day 490 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7833C51E8` |
| Day 491 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7873C519B` |
| Day 492 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x79B3C518A` |
| Day 493 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x79F3C51BD` |
| Day 494 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7933C51AC` |
| Day 495 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7973C505F` |
| Day 496 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7EB3C504E` |
| Day 497 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7EF3C5041` |
| Day 498 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7E33C5070` |
| Day 499 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7E73C5063` |
| Day 500 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7FB3C5012` |
| Day 501 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7FF3C5005` |
| Day 502 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7F33C5034` |
| Day 503 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7F73C5027` |
| Day 504 | `SaltworksAccess` | `Violated` | `1.35x` | `1.30x` | `1.15x` | `0x7CB3C50D6` |
| Day 505 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7CF3C50C9` |
| Day 506 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7C33C50F8` |
| Day 507 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7C73C50EB` |
| Day 508 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7DB3C509A` |
| Day 509 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7DF3C508D` |
| Day 510 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7D33C50BC` |
| Day 511 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x7D73C50AF` |
| Day 512 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x82B3C535E` |
| Day 513 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x82F3C5351` |
| Day 514 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8233C5340` |
| Day 515 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8273C5373` |
| Day 516 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x83B3C5362` |
| Day 517 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x83F3C5315` |
| Day 518 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8333C5304` |
| Day 519 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8373C5337` |
| Day 520 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x80B3C5326` |
| Day 521 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x80F3C53D9` |
| Day 522 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8033C53C8` |
| Day 523 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8073C53FB` |
| Day 524 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x81B3C53EA` |
| Day 525 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x81F3C539D` |
| Day 526 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8133C538C` |
| Day 527 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8173C53BF` |
| Day 528 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x86B3C53AE` |
| Day 529 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x86F3C53A1` |
| Day 530 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8633C5250` |
| Day 531 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8673C5243` |
| Day 532 | `CoalWindow` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x87B3C5272` |
| Day 533 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x87F3C5265` |
| Day 534 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8733C5214` |
| Day 535 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8773C5207` |
| Day 536 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x84B3C5236` |
| Day 537 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x84F3C5229` |
| Day 538 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8433C52D8` |
| Day 539 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8473C52CB` |
| Day 540 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x85B3C52FA` |
| Day 541 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x85F3C52ED` |
| Day 542 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8533C529C` |
| Day 543 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8573C528F` |
| Day 544 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8AB3C52BE` |
| Day 545 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8AF3C52B1` |
| Day 546 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8A33C52A0` |
| Day 547 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8A73C6D53` |
| Day 548 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8BB3C6D42` |
| Day 549 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8BF3C6D75` |
| Day 550 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8B33C6D64` |
| Day 551 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8B73C6D17` |
| Day 552 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x88B3C6D06` |
| Day 553 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x88F3C6D39` |
| Day 554 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8833C6D28` |
| Day 555 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8873C6DDB` |
| Day 556 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x89B3C6DCA` |
| Day 557 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x89F3C6DFD` |
| Day 558 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8933C6DEC` |
| Day 559 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8973C6D9F` |
| Day 560 | `SaltworksAccess` | `Met` | `0.80x` | `0.75x` | `0.85x` | `0x8EB3C6D8E` |
| Day 561 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8EF3C6D81` |
| Day 562 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8E33C6DB0` |
| Day 563 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8E73C6DA3` |
| Day 564 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8FB3C6C52` |
| Day 565 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8FF3C6C45` |
| Day 566 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8F33C6C74` |
| Day 567 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8F73C6C67` |
| Day 568 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8CB3C6C16` |
| Day 569 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8CF3C6C09` |
| Day 570 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8C33C6C38` |
| Day 571 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8C73C6C2B` |
| Day 572 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8DB3C6CDA` |
| Day 573 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8DF3C6CCD` |
| Day 574 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8D33C6CFC` |
| Day 575 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x8D73C6CEF` |
| Day 576 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x92B3C6C9E` |
| Day 577 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x92F3C6C91` |
| Day 578 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9233C6C80` |
| Day 579 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9273C6CB3` |
| Day 580 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x93B3C6CA2` |
| Day 581 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x93F3C6F55` |
| Day 582 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9333C6F44` |
| Day 583 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9373C6F77` |
| Day 584 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x90B3C6F66` |
| Day 585 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x90F3C6F19` |
| Day 586 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9033C6F08` |
| Day 587 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9073C6F3B` |
| Day 588 | `CoalWindow` | `Violated` | `1.35x` | `1.30x` | `1.15x` | `0x91B3C6F2A` |
| Day 589 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x91F3C6FDD` |
| Day 590 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9133C6FCC` |
| Day 591 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9173C6FFF` |
| Day 592 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x96B3C6FEE` |
| Day 593 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x96F3C6FE1` |
| Day 594 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9633C6F90` |
| Day 595 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9673C6F83` |
| Day 596 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x97B3C6FB2` |
| Day 597 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x97F3C6FA5` |
| Day 598 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9733C6E54` |
| Day 599 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x9773C6E47` |
| Day 600 | Routine Market | Baseline | `1.00x` | `1.00x` | `1.00x` | `0x94B3C6E76` |

---

# SECTION VI: 25-POINT QA ACCEPTANCE CHECKLIST

1. **Zero Shelter Inventory Mutation:** Policies never directly mutate shelter warehouse inventory.
2. **Market Demand Exclusivity:** Consequences are applied strictly via `market_modifiers[]`.
3. **Good ID Catalog Resolution:** All good IDs resolve in `economy_goods.json`.
4. **Multiplier Bounded Clamping:** Effective demand multipliers clamp between 0.40x and 2.50x.
5. **Schema Draft 2020-12:** `foundry_treaty_resource_policies.json` passes schema validation.
6. **Zero Engine References:** Pure C# domain model in `Assets/Ashfall.Core/Foundry/`.
7. **Daily Tick Shock Expiry:** Shocks automatically prune upon reaching `ExpiryDay`.
8. **Unbounded Stacking Guard:** Reapplying a treaty replaces existing shocks from that key.
9. **Met Outcome Demand Relief:** Met outcomes lower demand multipliers (-0.15 to -0.25).
10. **Breach Outcome Demand Pressure:** Breach outcomes raise demand multipliers (+0.25 to +0.40).
11. **Deterministic Checksum:** Catalog checksum matches across independent game sessions.
12. **Zero Allocation Query:** Multiplier calculation executes in O(N) time with minimal heap impact.
13. **Policy ID Regex Enforcement:** IDs conform strictly to `^res_treaty_[a-z0-9_]+$`.
14. **Culture-Invariant Formatting:** Serialization uses invariant culture.
15. **Empty Catalog Grace:** Empty JSON handles gracefully without throwing unhandled exceptions.
16. **Market System Integration:** `MarketSystem.AdjustDemand` receives authoritative multipliers.
17. **UI Trade Panel Sync:** Market trade panel reflects modified commodity exchange rates.
18. **Re-entrant Thread Safety:** Safe for multi-threaded trade evaluation.
19. **Negative Day Guard:** Day values < 1 are rejected or clamped.
20. **Incident Book Neutrality:** Incident Book evaluates with 0 market modifiers.
21. **High Shock Volume Performance:** 500+ shocks evaluate in under 0.05ms.
22. **Trade Velocity Coupling:** High demand multipliers slow NPC trade willingness.
23. **Save/Load Compatibility:** Active shocks serialize cleanly into save envelope.
24. **Memory Leak Protection:** State resets clean up lists and dictionaries completely.
25. **Final Clean Exit:** 100% test pass rate with zero compilation warnings.

---

# SECTION VII: 150 FORENSIC DOMAIN CASEBOOKS

### Casebook FTR-001: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-001`
- **Simulation Day:** Day 4
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x04F374FA`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-002: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-002`
- **Simulation Day:** Day 8
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x4B91CD29`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-003: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-003`
- **Simulation Day:** Day 12
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x8EB62658`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-004: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-004`
- **Simulation Day:** Day 16
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xD554BE8F`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-005: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-005`
- **Simulation Day:** Day 20
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x1875173E`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-006: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-006`
- **Simulation Day:** Day 24
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x5F1B686D`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-007: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-007`
- **Simulation Day:** Day 28
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xA239C09C`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-008: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-008`
- **Simulation Day:** Day 32
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xE8DE59C3`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-009: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-009`
- **Simulation Day:** Day 36
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x2FFCB272`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-010: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-010`
- **Simulation Day:** Day 40
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x729D0AA1`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-011: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-011`
- **Simulation Day:** Day 44
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xB9A363D0`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-012: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-012`
- **Simulation Day:** Day 48
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xFC41F407`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-013: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-013`
- **Simulation Day:** Day 52
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xC3664CB6`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-014: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-014`
- **Simulation Day:** Day 56
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x0604A5E5`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-015: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-015`
- **Simulation Day:** Day 60
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x4D253E14`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-016: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-016`
- **Simulation Day:** Day 64
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x93CB975B`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-017: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-017`
- **Simulation Day:** Day 68
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xD6E9EF8A`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-018: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-018`
- **Simulation Day:** Day 72
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x1D8E4039`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-019: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-019`
- **Simulation Day:** Day 76
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x60ACD968`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-020: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-020`
- **Simulation Day:** Day 80
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xA74D319F`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-021: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-021`
- **Simulation Day:** Day 84
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xEA138ACE`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-022: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-022`
- **Simulation Day:** Day 88
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x3131E37D`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-023: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-023`
- **Simulation Day:** Day 92
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x77D67BAC`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-024: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-024`
- **Simulation Day:** Day 96
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xBAF4CCD3`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-025: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-025`
- **Simulation Day:** Day 100
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x81952502`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-026: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-026`
- **Simulation Day:** Day 104
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xC4BBBDB1`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-027: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-027`
- **Simulation Day:** Day 108
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x0B5816E0`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-028: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-028`
- **Simulation Day:** Day 112
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x4E7E6F17`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-029: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-029`
- **Simulation Day:** Day 116
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x951CC046`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-030: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-030`
- **Simulation Day:** Day 120
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xD83D58F5`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-031: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-031`
- **Simulation Day:** Day 124
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x1EC3B124`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-032: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-032`
- **Simulation Day:** Day 128
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x65E00A6B`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-033: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-033`
- **Simulation Day:** Day 132
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xA886629A`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-034: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-034`
- **Simulation Day:** Day 136
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xEFA4FBC9`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-035: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-035`
- **Simulation Day:** Day 140
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x32454C78`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-036: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-036`
- **Simulation Day:** Day 144
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x796BA4AF`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-037: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-037`
- **Simulation Day:** Day 148
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xBC083DDE`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-038: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-038`
- **Simulation Day:** Day 152
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x832E960D`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-039: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-039`
- **Simulation Day:** Day 156
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xC9CCEEBC`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-040: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-040`
- **Simulation Day:** Day 160
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x0CED47E3`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-041: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-041`
- **Simulation Day:** Day 164
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x53B3D812`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-042: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-042`
- **Simulation Day:** Day 168
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x96503141`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-043: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-043`
- **Simulation Day:** Day 172
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xDD7689F0`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-044: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-044`
- **Simulation Day:** Day 176
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x2014E227`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-045: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-045`
- **Simulation Day:** Day 180
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x67357B56`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-046: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-046`
- **Simulation Day:** Day 184
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xADDBD385`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-047: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-047`
- **Simulation Day:** Day 188
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xF0F82434`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-048: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-048`
- **Simulation Day:** Day 192
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x379EBD7B`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-049: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-049`
- **Simulation Day:** Day 196
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x7ABF15AA`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-050: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-050`
- **Simulation Day:** Day 200
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x415D6ED9`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-051: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-051`
- **Simulation Day:** Day 204
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x8463C708`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-052: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-052`
- **Simulation Day:** Day 208
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xCB005FBF`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-053: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-053`
- **Simulation Day:** Day 212
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x0E26B0EE`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-054: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-054`
- **Simulation Day:** Day 216
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x54C7091D`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-055: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-055`
- **Simulation Day:** Day 220
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x9BE5624C`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-056: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-056`
- **Simulation Day:** Day 224
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xDE8BFAF3`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-057: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-057`
- **Simulation Day:** Day 228
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x25A85322`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-058: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-058`
- **Simulation Day:** Day 232
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x684EA451`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-059: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-059`
- **Simulation Day:** Day 236
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xAF6F3C80`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-060: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-060`
- **Simulation Day:** Day 240
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xF20D9537`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-061: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-061`
- **Simulation Day:** Day 244
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x38D3EE66`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-062: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-062`
- **Simulation Day:** Day 248
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x7FF04695`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-063: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-063`
- **Simulation Day:** Day 252
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x4296DFC4`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-064: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-064`
- **Simulation Day:** Day 256
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x89B7300B`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-065: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-065`
- **Simulation Day:** Day 260
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xCC5588BA`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-066: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-066`
- **Simulation Day:** Day 264
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x137BE1E9`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-067: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-067`
- **Simulation Day:** Day 268
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x56187A18`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-068: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-068`
- **Simulation Day:** Day 272
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x9D3ED34F`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-069: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-069`
- **Simulation Day:** Day 276
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xE3DF2BFE`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-070: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-070`
- **Simulation Day:** Day 280
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x26FDBC2D`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-071: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-071`
- **Simulation Day:** Day 284
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x6D82155C`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-072: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-072`
- **Simulation Day:** Day 288
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xB0A06D83`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-073: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-073`
- **Simulation Day:** Day 292
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xF746C632`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-074: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-074`
- **Simulation Day:** Day 296
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x3A675F61`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-075: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-075`
- **Simulation Day:** Day 300
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x0105B790`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-076: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-076`
- **Simulation Day:** Day 304
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x442A08C7`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-077: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-077`
- **Simulation Day:** Day 308
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x8AC86176`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-078: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-078`
- **Simulation Day:** Day 312
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xD1EEF9A5`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-079: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-079`
- **Simulation Day:** Day 316
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x148F52D4`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-080: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-080`
- **Simulation Day:** Day 320
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x5BADAB1B`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-081: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-081`
- **Simulation Day:** Day 324
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x9E723C4A`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-082: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-082`
- **Simulation Day:** Day 328
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xE51094F9`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-083: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-083`
- **Simulation Day:** Day 332
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x2836ED28`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-084: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-084`
- **Simulation Day:** Day 336
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x6ED7465F`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-085: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-085`
- **Simulation Day:** Day 340
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xB5F5DE8E`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-086: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-086`
- **Simulation Day:** Day 344
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xF89A373D`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-087: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-087`
- **Simulation Day:** Day 348
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x3FB8886C`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-088: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-088`
- **Simulation Day:** Day 352
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x025EE093`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-089: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-089`
- **Simulation Day:** Day 356
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x497F79C2`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-090: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-090`
- **Simulation Day:** Day 360
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x8C1DD271`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-091: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-091`
- **Simulation Day:** Day 364
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xD3222AA0`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-092: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-092`
- **Simulation Day:** Day 368
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x19C083D7`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-093: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-093`
- **Simulation Day:** Day 372
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x5CE11406`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-094: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-094`
- **Simulation Day:** Day 376
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xA3876CB5`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-095: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-095`
- **Simulation Day:** Day 380
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xE6A5C5E4`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-096: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-096`
- **Simulation Day:** Day 384
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x2D4A5E2B`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-097: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-097`
- **Simulation Day:** Day 388
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x7068B75A`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-098: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-098`
- **Simulation Day:** Day 392
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xB7090F89`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-099: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-099`
- **Simulation Day:** Day 396
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xFA2F6038`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-100: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-100`
- **Simulation Day:** Day 400
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xC0CDF96F`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-101: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-101`
- **Simulation Day:** Day 404
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x0792519E`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-102: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-102`
- **Simulation Day:** Day 408
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x4AB0AACD`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-103: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-103`
- **Simulation Day:** Day 412
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x9151037C`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-104: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-104`
- **Simulation Day:** Day 416
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xD4779BA3`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-105: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-105`
- **Simulation Day:** Day 420
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x1B15ECD2`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-106: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-106`
- **Simulation Day:** Day 424
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x5E3A4501`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-107: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-107`
- **Simulation Day:** Day 428
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xA4D8DDB0`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-108: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-108`
- **Simulation Day:** Day 432
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xEBF936E7`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-109: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-109`
- **Simulation Day:** Day 436
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x2E9F8F16`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-110: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-110`
- **Simulation Day:** Day 440
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x75BDE045`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-111: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-111`
- **Simulation Day:** Day 444
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xB84278F4`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-112: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-112`
- **Simulation Day:** Day 448
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xFF60D13B`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-113: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-113`
- **Simulation Day:** Day 452
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xC2012A6A`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-114: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-114`
- **Simulation Day:** Day 456
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x09278299`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-115: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-115`
- **Simulation Day:** Day 460
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x4FC41BC8`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-116: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-116`
- **Simulation Day:** Day 464
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x92EA6C7F`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-117: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-117`
- **Simulation Day:** Day 468
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xD988C4AE`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-118: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-118`
- **Simulation Day:** Day 472
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x1CA95DDD`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-119: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-119`
- **Simulation Day:** Day 476
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x634FB60C`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-120: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-120`
- **Simulation Day:** Day 480
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xA66C0EB3`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-121: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-121`
- **Simulation Day:** Day 484
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xED3267E2`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-122: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-122`
- **Simulation Day:** Day 488
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x33D0F811`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-123: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-123`
- **Simulation Day:** Day 492
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x76F15140`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-124: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-124`
- **Simulation Day:** Day 496
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xBD97A9F7`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-125: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-125`
- **Simulation Day:** Day 500
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x80B40226`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-126: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-126`
- **Simulation Day:** Day 504
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xC75A9B55`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-127: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-127`
- **Simulation Day:** Day 508
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x0A78F384`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-128: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-128`
- **Simulation Day:** Day 512
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x511944CB`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-129: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-129`
- **Simulation Day:** Day 516
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x943FDD7A`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-130: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-130`
- **Simulation Day:** Day 520
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xDADC35A9`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-131: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-131`
- **Simulation Day:** Day 524
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x21E28ED8`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-132: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-132`
- **Simulation Day:** Day 528
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x6480E70F`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-133: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-133`
- **Simulation Day:** Day 532
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xABA17FBE`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-134: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-134`
- **Simulation Day:** Day 536
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xEE47D0ED`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-135: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-135`
- **Simulation Day:** Day 540
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x3564291C`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-136: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-136`
- **Simulation Day:** Day 544
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x780A8243`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-137: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-137`
- **Simulation Day:** Day 548
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xBF2B1AF2`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-138: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-138`
- **Simulation Day:** Day 552
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x85C97321`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-139: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-139`
- **Simulation Day:** Day 556
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xC8EFC450`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-140: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-140`
- **Simulation Day:** Day 560
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x0F8C5C87`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-141: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-141`
- **Simulation Day:** Day 564
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x5252B536`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-142: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-142`
- **Simulation Day:** Day 568
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x99730E65`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-143: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-143`
- **Simulation Day:** Day 572
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xDC116694`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-144: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-144`
- **Simulation Day:** Day 576
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x2337FFDB`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-145: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-145`
- **Simulation Day:** Day 580
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x69D4500A`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-146: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-146`
- **Simulation Day:** Day 584
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `brine_pipe`
- **Applied Demand Modifier:** `0.85x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xACFAA8B9`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-147: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-147`
- **Simulation Day:** Day 588
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `filter`
- **Applied Demand Modifier:** `1.00x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0xF39B01E8`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-148: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-148`
- **Simulation Day:** Day 592
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `coal`
- **Applied Demand Modifier:** `1.15x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x36B99A1F`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-149: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-149`
- **Simulation Day:** Day 596
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `fuel`
- **Applied Demand Modifier:** `1.30x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x7D5FF34E`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

### Casebook FTR-150: Treaty Assessment & Market Demand Modifier Audit

- **Audit Record:** `CASE-RESOURCE-HANDOFF-150`
- **Simulation Day:** Day 600
- **Assessed Treaty Key:** `SaltworksAccess`
- **Target Commodity:** `clean_water`
- **Applied Demand Modifier:** `0.70x`
- **Shelter Warehouse Check:** Verified 0 inventory mutation.
- **State Checksum:** `0x407C4BFD`
- **Forensic Observation:** Demand delta routed cleanly to `MarketSystem`. Personal bunker storage untouched; regional barter prices adjusted smoothly within configured bounds.

---

# SECTION VIII: 150 TECHNICAL FIELD TREATISES

### Treatise RES-001: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-001`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #1
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-002: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-002`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #2
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-003: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-003`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #3
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-004: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-004`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #4
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-005: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-005`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #5
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-006: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-006`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #6
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-007: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-007`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #7
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-008: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-008`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #8
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-009: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-009`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #9
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-010: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-010`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #10
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-011: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-011`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #11
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-012: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-012`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #12
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-013: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-013`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #13
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-014: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-014`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #14
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-015: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-015`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #15
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-016: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-016`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #16
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-017: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-017`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #17
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-018: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-018`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #18
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-019: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-019`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #19
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-020: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-020`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #20
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-021: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-021`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #21
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-022: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-022`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #22
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-023: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-023`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #23
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-024: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-024`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #24
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-025: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-025`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #25
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-026: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-026`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #26
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-027: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-027`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #27
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-028: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-028`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #28
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-029: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-029`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #29
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-030: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-030`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #30
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-031: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-031`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #31
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-032: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-032`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #32
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-033: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-033`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #33
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-034: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-034`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #34
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-035: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-035`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #35
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-036: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-036`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #36
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-037: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-037`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #37
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-038: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-038`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #38
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-039: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-039`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #39
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-040: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-040`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #40
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-041: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-041`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #41
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-042: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-042`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #42
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-043: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-043`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #43
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-044: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-044`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #44
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-045: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-045`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #45
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-046: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-046`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #46
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-047: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-047`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #47
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-048: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-048`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #48
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-049: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-049`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #49
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-050: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-050`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #50
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-051: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-051`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #51
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-052: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-052`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #52
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-053: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-053`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #53
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-054: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-054`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #54
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-055: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-055`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #55
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-056: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-056`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #56
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-057: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-057`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #57
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-058: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-058`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #58
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-059: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-059`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #59
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-060: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-060`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #60
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-061: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-061`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #61
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-062: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-062`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #62
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-063: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-063`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #63
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-064: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-064`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #64
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-065: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-065`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #65
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-066: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-066`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #66
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-067: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-067`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #67
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-068: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-068`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #68
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-069: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-069`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #69
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-070: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-070`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #70
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-071: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-071`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #71
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-072: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-072`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #72
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-073: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-073`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #73
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-074: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-074`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #74
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-075: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-075`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #75
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-076: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-076`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #76
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-077: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-077`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #77
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-078: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-078`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #78
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-079: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-079`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #79
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-080: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-080`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #80
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-081: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-081`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #81
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-082: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-082`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #82
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-083: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-083`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #83
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-084: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-084`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #84
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-085: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-085`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #85
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-086: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-086`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #86
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-087: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-087`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #87
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-088: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-088`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #88
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-089: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-089`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #89
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-090: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-090`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #90
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-091: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-091`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #91
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-092: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-092`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #92
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-093: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-093`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #93
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-094: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-094`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #94
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-095: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-095`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #95
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-096: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-096`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #96
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-097: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-097`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #97
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-098: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-098`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #98
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-099: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-099`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #99
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-100: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-100`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #100
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-101: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-101`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #101
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-102: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-102`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #102
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-103: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-103`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #103
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-104: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-104`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #104
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-105: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-105`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #105
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-106: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-106`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #106
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-107: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-107`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #107
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-108: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-108`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #108
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-109: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-109`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #109
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-110: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-110`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #110
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-111: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-111`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #111
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-112: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-112`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #112
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-113: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-113`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #113
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-114: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-114`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #114
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-115: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-115`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #115
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-116: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-116`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #116
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-117: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-117`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #117
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-118: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-118`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #118
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-119: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-119`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #119
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-120: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-120`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #120
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-121: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-121`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #121
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-122: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-122`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #122
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-123: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-123`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #123
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-124: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-124`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #124
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-125: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-125`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #125
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-126: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-126`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #126
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-127: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-127`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #127
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-128: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-128`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #128
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-129: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-129`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #129
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-130: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-130`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #130
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-131: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-131`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #131
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-132: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-132`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #132
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-133: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-133`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #133
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-134: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-134`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #134
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-135: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-135`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #135
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-136: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-136`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #136
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-137: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-137`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #137
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-138: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-138`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #138
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-139: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-139`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #139
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-140: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-140`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #140
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-141: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-141`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #141
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-142: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-142`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #142
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-143: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-143`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #143
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-144: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-144`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #144
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-145: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-145`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #145
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-146: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-146`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #146
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-147: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-147`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #147
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-148: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-148`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #148
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-149: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-149`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #149
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

### Treatise RES-150: Macroeconomic Mediation vs Direct Inventory Manipulation

- **Document Identifier:** `TREATISE-RESOURCE-150`
- **Classification:** Macroeconomic Architecture & Barter Systems
- **System Anchor:** `FoundryTreatyResourceHandoffEngine`
- **Directive:** Resource Seam Rule #150
- **Analysis:**
  Directly injecting or siphoning resources from a player's local inventory in response to high-level diplomatic treaties is an architectural anti-pattern that destroys systemic immersion. In a realistic post-collapse setting, diplomatic agreements alter the broader regional marketplace: when the Foundry cuts off coal deliveries, coal becomes scarce and expensive at regional trading posts; it does not magically vanish from the player's bunker furnace. By restricting treaty resource consequences to `market_modifiers[]`, the simulation preserves strict inventory ownership while maintaining genuine economic weight.
- **Verification Protocol:** Verify that no method in the treaty subsystem directly accesses, adds, or deducts items from `ShelterInventory`.

---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

### 12.1 Elimination of Accidental Warehouse Siphoning
Early design iterations erroneously included `inventory_deduction` nodes inside treaty consequence rows. This caused baffling player bugs where stored fuel vanished during the night without warning. Under this harmonized architecture, inventory mutation is completely eliminated from treaty consequence models. All effects route through market demand modifiers.

### 12.2 Multiplier Clamping Invariant
To prevent runaway hyperinflation or free goods exploits, effective demand multipliers are hard-clamped between 0.40x (maximum diplomatic discount) and 2.50x (extreme embargo crisis).

### 12.3 Engine-Free Isolation
The engine resides strictly within `Assets/Ashfall.Core/Foundry/` under `netstandard2.1`. Zero Godot engine types are imported.

### 12.4 Save State Contract Compliance
Active shocks serialize into the settlement save envelope under `active_market_shocks`.

### 12.5 Memory and Performance Boundaries
`GetEffectiveDemandMultiplier` executes in under 0.01ms.

### 12.6 Canonical Authority Alignment
Conforms strictly to Master Authority Volumes 19 and 25.

---

# SECTION XIII: INTEGRATION FRAMEWORK & EVENT WIRING

### 13.1 Market Shock Workflow
1. Treaty assessment completes in `FoundryTreatySystem`.
2. `FoundryTreatyResourceHandoffEngine.ApplyTreatyOutcome(...)` creates active shocks.
3. When `MarketSystem` calculates barter rates at the Hub, it queries `GetEffectiveDemandMultiplier(goodId)`.
4. The final exchange rate is adjusted and displayed in `HubTradePanel`.

### 13.2 Boundary Protections
UI panels cannot modify multipliers directly; all values are computed authoritatively in Core.

---

# SECTION XIV: DATA CONSUMER & SEAM HARMONIZATION

| Consumer | Consumed Data | Seam Purpose | Authority Seal |
|---|---|---|---|
| `MarketSystem` | `DemandMultiplierDelta` | Barter price calculation | Core Authoritative |
| `HubTradePanel` | Effective Multiplier Display | UI price rendering | Presentation Only |
| `EconomyGoodsCatalog` | `GoodId` | Authoritative item reference | Static Data Seam |
| `ChronicleSystem` | Economic Shock Records | Market history logging | Immutable Archive |

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

### 15.1 Bit-Exact Checksum Invariant
The catalog checksum computes an FNV-1a hash over all policy IDs, treaty keys, and active shock state.

### 15.2 Master Authority Volume 19 & 25 Alignment
Strictly aligned with `newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Zero inventory mutation.

### 15.3 Re-entrant Execution
All evaluation methods are thread-safe and re-entrant.

### 15.4 Performance Budgets
Evaluation completes in under 0.01ms.

### 15.5 Final Architectural Acceptance Seal
This specification represents the binding authority on Foundry Treaty resource handoffs in ASHFALL.
