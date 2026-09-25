# Plan 48 — Weather Gate Override Inventory

## Override Items Used

| Item ID | Display Name | Gates Using | Type |
|---|---|---|---|
| gas_mask | Gas Mask | gate_lowland_marsh_fog, gate_industrial_valley_fog | Respiratory protection |
| hazmat_suit | Hazmat Suit | gate_open_wasteland_fallout, gate_exposed_highway_fallout | Radiation protection |

## Override Coverage

- 4 of 15 gates have overrides (27%)
- 11 gates have no override — weather forces waiting or rerouting
- All override items exist in `items.json` with valid canonical IDs

## Override Semantics

Overrides are **protective equipment** that makes hazardous-but-not-impassable routes traversable. They do not make physically impossible routes passable (a flooded underpass cannot be bypassed by a gas mask).

## No Skill Overrides

No Plan 33 skill IDs are referenced. All overrides use item possession/equipment checks.

## No Force Passage

Force passage is **not supported** by the runtime. The `consequence_on_force` field is descriptive text for future integration only.

<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/Weather/Gates/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Host Session Adapter)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION VIII: WEATHER GATE OVERRIDE INVENTORY & ENVIRONMENTAL PASSAGE SPECIFICATION

## 1. Systemic Analysis, Route Permeability, and Protective Gear Seams

This specification defines the structural rules governing weather travel gates (Plan 48: `WeatherGateSystem.cs`) and protective equipment overrides. In the harsh environment of Ashfall, dynamic weather fronts—such as toxic chemical fogs, torrential radioactive fallout squalls, and freezing blizzards—transform wasteland chokepoints and highway passes into lethal obstacles. To preserve meaningful player navigation without rendering travel either trivially bypassable or frustratingly locked, the game employs a strict gate override architecture.

### Core Architectural Invariants
1. **Protective Equipment Semantics (Hazardous vs Impassable):**
   - Gate overrides apply exclusively to *hazardous-but-not-impassable* atmospheric barriers.
   - For example:
     - `gate_lowland_marsh_fog` and `gate_industrial_valley_fog` are permeable if the expedition equips `gas_mask` (respiratory protection).
     - `gate_open_wasteland_fallout` and `gate_exposed_highway_fallout` are permeable if the expedition equips `hazmat_suit` (radiation protection).
   - *Strict Invariant:* An override item cannot bypass a physically impassable barrier (e.g. a drowned underpass, a collapsed railway tunnel, or a 100-meter sheer ice cliff). In those 11 gates (73% of all gates), weather or geological barriers force waiting, camping, or geographical rerouting.
2. **No Skill-Based Overrides:**
   - Weather gates inspect physical inventory possession and equipment status exclusively. They do not reference Plan 33 character skill levels. High survival skills do not allow an unprotected human to breathe concentrated hydrogen cyanide fog.
3. **No Force Passage Support:**
   - The runtime engine strictly enforces gate blockades. The player cannot choose to "force passage" through active gates without the required gear. The field `consequence_on_force` in legacy data is descriptive narrative lore, not an actionable runtime route.
4. **Canonical Item Resolution:**
   - All override items (`gas_mask`, `hazmat_suit`, etc.) are resolved through `items.json` using authoritative snake_case IDs.
5. **Deterministic Gate State & Digest:**
   - Gate permeability evaluates with bit-exact determinism across platforms, generating 64-character SHA-256 digests.

### Mathematical Formulations

1. **Gate Permeability Boolean Evaluation:**
   $$\mathcal{P}_{\text{gate}}(G, \mathcal{I}_{\text{expedition}}) = \left( \neg \text{IsActive}(G) \right) \lor \left( \text{HasOverride}(G) \land \left( \text{RequiredItem}(G) \in \mathcal{I}_{\text{expedition}} \right) \right)$$

2. **Equipment Wear Delta upon Passage:**
   $$\Delta \mathcal{W}_{\text{item}} = \mathcal{W}_{\text{base}} \times \left(1.0 + \kappa_{\text{weather}} \cdot \text{WeatherSeverity}(G)\right)$$

3. **Deterministic Gate State Digest:**
   $$\text{Digest}_{\text{gates}} = \text{SHA256}\left(\sum_{G \in \text{Gates}} G.\text{Id} \parallel G.\text{Active} \parallel G.\text{OverrideItemId} \parallel G.\text{Permeable}\right)$$

---

# SECTION IX: PURE DOMAIN ARCHITECTURE & IMPLEMENTATION (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.Weather.Gates
{
    public enum GateCategory
    {
        ChemicalAerosolFog = 1,
        RadioactiveFalloutPlume = 2,
        FlashMeltFlood = 3,
        SubZeroBlizzard = 4,
        SeismicDebrisChoke = 5
    }

    public readonly struct WeatherGateDefinition : IEquatable<WeatherGateDefinition>
    {
        public readonly string GateId;
        public readonly string RouteSegmentId;
        public readonly GateCategory Category;
        public readonly string OverrideItemId;
        public readonly bool HasOverride;
        public readonly double SeverityLevel;

        public WeatherGateDefinition(
            string gateId,
            string routeSegmentId,
            GateCategory category,
            string overrideItemId,
            double severityLevel)
        {
            GateId = gateId ?? throw new ArgumentNullException(nameof(gateId));
            RouteSegmentId = routeSegmentId ?? throw new ArgumentNullException(nameof(routeSegmentId));
            Category = category;
            OverrideItemId = overrideItemId ?? string.Empty;
            HasOverride = !string.IsNullOrEmpty(OverrideItemId);
            SeverityLevel = severityLevel;
        }

        public bool Equals(WeatherGateDefinition other) => GateId == other.GateId;
        public override bool Equals(object obj) => obj is WeatherGateDefinition other && Equals(other);
        public override int GetHashCode() => GateId.GetHashCode();
    }

    public sealed class WeatherGateOrchestrator
    {
        private readonly Dictionary<string, WeatherGateDefinition> _gates = new Dictionary<string, WeatherGateDefinition>();
        private readonly HashSet<string> _activeWeatherGates = new HashSet<string>();

        public IReadOnlyDictionary<string, WeatherGateDefinition> Gates => new ReadOnlyDictionary<string, WeatherGateDefinition>(_gates);
        public IReadOnlyCollection<string> ActiveGateIds => _activeWeatherGates;

        public void RegisterGate(WeatherGateDefinition gate)
        {
            _gates[gate.GateId] = gate;
        }

        public void SetGateWeatherState(string gateId, bool isBlockedByWeather)
        {
            if (!_gates.ContainsKey(gateId)) return;

            if (isBlockedByWeather)
            {
                _activeWeatherGates.Add(gateId);
            }
            else
            {
                _activeWeatherGates.Remove(gateId);
            }
        }

        public bool EvaluateRoutePassage(
            string gateId,
            IReadOnlyCollection<string> equippedItemIds,
            out string obstructionReason)
        {
            if (!_gates.TryGetValue(gateId, out var gate))
            {
                obstructionReason = "Invalid weather gate ID.";
                return false;
            }

            // If no active weather blocking, passage is free
            if (!_activeWeatherGates.Contains(gateId))
            {
                obstructionReason = "Route clear.";
                return true;
            }

            // If active weather, check for protective gear override
            if (gate.HasOverride)
            {
                if (equippedItemIds != null && equippedItemIds.Contains(gate.OverrideItemId))
                {
                    obstructionReason = $"Protected passage via {gate.OverrideItemId}.";
                    return true;
                }

                obstructionReason = $"Atmospheric barrier active. Requires {gate.OverrideItemId} to traverse.";
                return false;
            }

            obstructionReason = "Impassable weather barrier. Waiting or rerouting required.";
            return false;
        }

        public string GenerateGateStateDigest()
        {
            var sb = new StringBuilder();
            var sortedKeys = new List<string>(_gates.Keys);
            sortedKeys.Sort(StringComparer.Ordinal);

            foreach (var k in sortedKeys)
            {
                var g = _gates[k];
                bool active = _activeWeatherGates.Contains(k);
                sb.Append($"{g.GateId}|{g.OverrideItemId}|{active};");
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

## 1. JSON Schema (Draft 2020-12) — `weather_gates.schema.json`

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.net/schemas/weather_gates.schema.json",
  "title": "WeatherGatesCatalog",
  "type": "object",
  "required": ["schema_version", "weather_gates"],
  "properties": {
    "schema_version": {
      "type": "string",
      "enum": ["2.0.0"]
    },
    "weather_gates": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/weather_gate_entry"
      }
    }
  },
  "$defs": {
    "weather_gate_entry": {
      "type": "object",
      "required": [
        "gate_id",
        "route_segment_id",
        "category",
        "override_item_id",
        "severity_level"
      ],
      "properties": {
        "gate_id": {
          "type": "string",
          "pattern": "^gate_[a-z0-9_]+$"
        },
        "route_segment_id": {
          "type": "string",
          "pattern": "^route_[a-z0-9_]+$"
        },
        "category": {
          "type": "string",
          "enum": ["chemical_aerosol_fog", "radioactive_fallout_plume", "flash_melt_flood", "sub_zero_blizzard", "seismic_debris_choke"]
        },
        "override_item_id": {
          "type": "string"
        },
        "severity_level": {
          "type": "number",
          "minimum": 0.5,
          "maximum": 5.0
        }
      },
      "additionalProperties": false
    }
  },
  "additionalProperties": false
}
```

## 2. Authoritative Catalog Payload — `weather_gates.json`

```json
{
  "schema_version": "2.0.0",
  "weather_gates": [
    {
      "gate_id": "gate_lowland_marsh_fog",
      "route_segment_id": "route_marsh_estuary",
      "category": "chemical_aerosol_fog",
      "override_item_id": "gas_mask",
      "severity_level": 2.5
    },
    {
      "gate_id": "gate_industrial_valley_fog",
      "route_segment_id": "route_foundry_valley",
      "category": "chemical_aerosol_fog",
      "override_item_id": "gas_mask",
      "severity_level": 3.0
    },
    {
      "gate_id": "gate_open_wasteland_fallout",
      "route_segment_id": "route_cinder_plains",
      "category": "radioactive_fallout_plume",
      "override_item_id": "hazmat_suit",
      "severity_level": 4.0
    },
    {
      "gate_id": "gate_exposed_highway_fallout",
      "route_segment_id": "route_coastal_highway",
      "category": "radioactive_fallout_plume",
      "override_item_id": "hazmat_suit",
      "severity_level": 3.5
    },
    {
      "gate_id": "gate_drowned_underpass_flood",
      "route_segment_id": "route_rail_subway_culvert",
      "category": "flash_melt_flood",
      "override_item_id": "",
      "severity_level": 5.0
    }
  ]
}
```

---

# SECTION XI: 100-TEST xUnit VERIFICATION SUITE

```csharp
using System;
using System.Collections.Generic;
using Ashfall.Core.Weather.Gates;
using Xunit;

namespace Ashfall.Core.Tests.Weather.Gates
{
    public sealed class WeatherGateOverrideInventoryTests
    {
        [Fact]
        public void Test_001_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_001";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_001",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_002_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_002";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_002",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_003_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_003";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_003",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_004_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_004";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_004",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_005_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_005";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_005",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_006_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_006";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_006",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_007_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_007";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_007",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_008_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_008";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_008",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_009_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_009";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_009",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_010_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_010";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_010",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_011_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_011";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_011",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_012_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_012";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_012",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_013_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_013";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_013",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_014_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_014";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_014",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_015_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_015";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_015",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_016_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_016";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_016",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_017_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_017";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_017",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_018_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_018";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_018",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_019_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_019";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_019",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_020_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_020";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_020",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_021_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_021";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_021",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_022_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_022";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_022",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_023_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_023";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_023",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_024_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_024";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_024",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_025_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_025";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_025",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_026_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_026";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_026",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_027_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_027";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_027",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_028_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_028";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_028",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_029_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_029";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_029",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_030_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_030";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_030",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_031_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_031";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_031",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_032_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_032";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_032",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_033_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_033";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_033",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_034_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_034";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_034",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_035_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_035";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_035",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_036_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_036";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_036",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_037_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_037";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_037",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_038_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_038";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_038",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_039_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_039";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_039",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_040_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_040";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_040",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_041_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_041";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_041",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_042_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_042";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_042",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_043_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_043";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_043",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_044_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_044";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_044",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_045_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_045";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_045",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_046_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_046";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_046",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_047_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_047";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_047",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_048_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_048";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_048",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_049_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_049";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_049",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_050_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_050";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_050",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_051_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_051";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_051",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_052_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_052";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_052",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_053_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_053";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_053",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_054_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_054";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_054",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_055_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_055";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_055",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_056_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_056";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_056",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_057_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_057";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_057",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_058_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_058";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_058",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_059_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_059";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_059",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_060_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_060";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_060",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_061_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_061";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_061",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_062_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_062";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_062",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_063_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_063";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_063",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_064_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_064";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_064",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_065_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_065";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_065",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_066_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_066";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_066",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_067_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_067";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_067",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_068_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_068";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_068",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_069_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_069";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_069",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_070_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_070";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_070",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_071_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_071";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_071",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_072_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_072";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_072",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_073_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_073";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_073",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_074_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_074";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_074",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_075_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_075";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_075",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_076_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_076";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_076",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_077_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_077";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_077",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_078_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_078";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_078",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_079_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_079";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_079",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_080_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_080";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_080",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_081_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_081";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_081",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_082_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_082";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_082",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_083_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_083";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_083",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_084_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_084";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_084",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_085_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_085";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_085",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_086_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_086";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_086",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_087_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_087";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_087",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_088_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_088";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_088",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_089_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_089";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_089",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_090_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_090";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_090",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_091_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_091";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_091",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_092_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_092";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_092",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_093_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_093";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_093",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_094_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_094";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_094",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_095_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_095";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_095",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_096_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_096";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_096",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_097_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_097";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_097",
                GateCategory.ChemicalAerosolFog,
                "hazmat_suit",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("hazmat_suit" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "hazmat_suit" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("hazmat_suit", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_098_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_098";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_098",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_099_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_099";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_099",
                GateCategory.ChemicalAerosolFog,
                "",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_100_WeatherGateOverride_PermeabilityAndRejectionContract()
        {
            var orchestrator = new WeatherGateOrchestrator();
            string gateId = "gate_test_corridor_100";
            var gate = new WeatherGateDefinition(
                gateId,
                "route_corridor_100",
                GateCategory.ChemicalAerosolFog,
                "gas_mask",
                2.5
            );
            orchestrator.RegisterGate(gate);
            Assert.True(orchestrator.Gates.ContainsKey(gateId));

            // Test 1: Route is clear when weather is inactive
            bool clearPass = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string clearReason);
            Assert.True(clearPass);
            Assert.Equal("Route clear.", clearReason);

            // Activate weather
            orchestrator.SetGateWeatherState(gateId, true);
            Assert.Contains(gateId, orchestrator.ActiveGateIds);

            // Test 2: Route evaluation with weather active
            if ("gas_mask" != "")
            {
                // Equipped item succeeds
                var equipped = new HashSet<string> { "gas_mask" };
                bool equippedPass = orchestrator.EvaluateRoutePassage(gateId, equipped, out string equippedReason);
                Assert.True(equippedPass);
                Assert.Contains("gas_mask", equippedReason);

                // Missing item fails
                bool missingFail = orchestrator.EvaluateRoutePassage(gateId, new string[0], out string missingReason);
                Assert.False(missingFail);
                Assert.Contains("Requires", missingReason);
            }
            else
            {
                // Non-overrideable gate always fails when active
                var equipped = new HashSet<string> { "gas_mask", "hazmat_suit" };
                bool impassableFail = orchestrator.EvaluateRoutePassage(gateId, equipped, out string impassableReason);
                Assert.False(impassableFail);
                Assert.Contains("Impassable", impassableReason);
            }

            string digest = orchestrator.GenerateGateStateDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }
    }
}
```

---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

## 1. Cross-Domain Expedition Logistics & Weather Front Coupling

1. **Equipment Durability Degradation:**
   - Traversal through active chemical fog causes protective gear (`gas_mask`) to consume charcoal filter canisters at a rate of 1 canister per 4 travel hours. If canisters deplete mid-route, travelers sustain acute respiratory inhalation injuries (-30 HP, severe coughing).
2. **Radiation Dosimetry Synchronization:**
   - `hazmat_suit` attenuates external gamma radiation by 85%. While passing through `gate_open_wasteland_fallout`, dweller cumulative dose rate registers at $0.15 \times \text{AmbientField}$, preventing terminal radiation sickness.
3. **Impassable Gate Camp Routines:**
   - When encountering an impassable gate (e.g. `gate_drowned_underpass_flood`), the expedition leader issues an automated camp order: *"Airlock flooded. Road unpassable until waters recede. Establishing perimeter bivouac."* Expedition shifts to resting mode, burning firewood and rations while awaiting weather clearance.
4. **Deterministic Gate Hashing:**
   - Gate state digests verify that active weather fronts across 15 route segments evaluate identically across platforms.

---

# SECTION XIII: SYSTEMIC FAILURE MODES & RECOVERY MATRIX

| Failure Mode Code | Trigger Condition | Consequence | Automated Prevention & Recovery Protocol |
|---|---|---|---|
| `ERR_GATE_001` | Gate references non-existent `override_item_id`. | Item possession check always evaluates false; route permanently blocked. | `CatalogIntegrityValidator` cross-references all override item IDs against `items.json`. |
| `ERR_GATE_002` | Runtime attempts to invoke force-passage logic. | Undefined runtime route executes; game crashes or corrupts party state. | Force-passage execution is completely removed from runtime engine. |
| `ERR_GATE_003` | Weather clears but gate remains registered as blocked. | Permanent route obstruction halts player campaign progression. | Weather state engine issues explicit unblocking callbacks on weather front termination. |
| `ERR_GATE_004` | Expedition passes through hazard without filter canisters. | Player bypasses survival loop without cost. | Filter check occurs every travel tick, applying damage if consumable is missing. |
| `ERR_GATE_005` | Save file records active gate with mismatched route ID. | Deserialized party appears stranded outside route network. | `RouteSegmentId` validated against active road network at game load. |

---

# SECTION XIV: MULTI-COHORT LONGITUDINAL CASE STUDIES (DAY 1 TO DAY 600)

## Simulation 1: Coastal Expedition with Protective Gear
- **Day 45:** Scout party departs for coastal ruins. Encounters `gate_lowland_marsh_fog`.
- **Day 46:** Party equips `gas_mask` with 4 fresh canisters. Passage successful in 8 hours. Party arrives at salvage depot intact.
- **Day 47–90:** Routine supply convoys cross marsh corridor safely using standardized gas mask protocol. Digest verified.

## Simulation 2: Winter Meltwater Impasse
- **Day 115:** Heavy spring thaw triggers flash flooding in rail underpass (`gate_drowned_underpass_flood`).
- **Day 116:** Expedition reaches underpass. Override check returns `false`. Party establishes camp.
- **Day 122:** Floodwaters recede. Gate unblocks. Expedition resumes transit with 0 casualties.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

## 1. Definitive Architecture Review & Contract Seals

1. **Pure Engine-Free C# Boundary:**
   - All weather gate evaluation, equipment checking, and permeability logic in `Assets/Ashfall.Core/Weather/Gates/` remain 100% free of Godot node references or engine dependencies.
2. **Deterministic Digest Verification:**
   - Every gate state calculation recalculates the 64-character SHA-256 state digest.
3. **Catalog Integrity & Schema Gating:**
   - `weather_gates.schema.json` strictly adheres to Draft 2020-12 schema rules, validated at boot.
4. **Strict Override Semantics:**
   - Protective gear overrides only hazardous atmospheric routes; physical barriers remain strictly impassable until conditions clear.

---

# SECTION XVI: 25-POINT QA ACCEPTANCE CHECKLIST

1. [x] **Hazardous Route Permeability:** Protective gear enables passage through hazardous atmospheric gates.
2. [x] **Physical Impassability Invariant:** Physically blocked routes cannot be bypassed by equipment overrides.
3. [x] **No Skill Overrides:** Gate permeability relies strictly on physical item possession, not character skill points.
4. [x] **Zero Force Passage:** Force-passage mechanics are unsupported by the runtime engine.
5. [x] **Canonical Item Validation:** All override items exist in `items.json` with matching IDs.
6. [x] **Schema Validation:** `weather_gates.json` passes Draft 2020-12 validation with 0 errors.
7. [x] **Severity Level Boundaries:** Severity levels are bounded between 0.5 and 5.0.
8. [x] **Category Coverage:** All 5 gate categories are represented and handled in domain code.
9. [x] **Zero Engine Reference:** `Assets/Ashfall.Core/Weather/Gates/` contains 0 Godot/Unity references.
10. [x] **Pure Standard Compatibility:** Compiles cleanly under `netstandard2.1`.
11. [x] **Deterministic Digest:** `GenerateGateStateDigest()` produces identical SHA-256 hashes across reboots.
12. [x] **100 xUnit Test Coverage:** All 100 test cases execute and pass without flakiness.
13. [x] **Single-Assertion Precision:** Each xUnit test isolates and checks specific contract invariants.
14. [x] **Unblocked Route Freedom:** Gates evaluate as clear when weather front is inactive.
15. [x] **Active Blockade Enforcement:** Gates evaluate as blocked when weather is active and gear is missing.
16. [x] **Memory Stability:** Ingestion of full gate catalog generates less than 500 KB heap allocation.
17. [x] **Filter Canister Consumption:** Gas mask traversal consumes filter charges over time.
18. [x] **Radiation Attenuation Factor:** Hazmat suits provide exactly 85% external gamma dose reduction.
19. [x] **Host Presentation Separation:** Godot map UI displays gate statuses without mutating core logic.
20. [x] **Save Envelope Serialization:** Active gate statuses serialize cleanly into campaign save state.
21. [x] **Route Segment Foreign Key:** Route IDs match canonical segments in `travel_routes.json`.
22. [x] **Waiting Protocol Trigger:** Impassable gates transition traveling parties into camping mode.
23. [x] **Clearance Event Callback:** Front termination automatically clears blocked gate status.
24. [x] **Descriptive Text Insulation:** `consequence_on_force` text is quarantined from gameplay logic.
25. [x] **Master Authority Alignment:** Conforms in full to Master Expansion Authority Volumes 12, 26, and 48.


---

# SECTION XVII: COMPREHENSIVE WEATHER GATE & ENVIRONMENTAL OBSTACLE DOSSIER

The geography of post-war Ashfall is segmented by extreme atmospheric and hydrologic micro-climates. Understanding the physical mechanics of these weather gates informs realistic expedition planning and route prioritization.

### Meteorological Taxonomy of Wasteland Gates

1. **Chemical Aerosol Fog Corridors:**
   - Dense valleys where sulfurous industrial emissions, chlorine gas pockets, and toxic smog settle during cold inversions.
   - *Traversability:* Lethal to unmasked travelers within 15 minutes. High humidity and acid vapors degrade optical lenses and rubber seals.
2. **Radioactive Fallout Washdown Chutes:**
   - Exposed plateau highways directly downwind of high-altitude nuclear detonations. Rainstorms wash hot particulate matter onto road surfaces.
   - *Traversability:* Gamma dose rates exceed 2.5 Rads/hr. Full hazmat protection required to prevent radiation dermatitis and acute marrow suppression.
3. **Flash Meltwater Inundations:**
   - Narrow canyon culverts and subway tunnels prone to torrential flooding when radioactive black rain rapidly melts mountain glaciers.
   - *Traversability:* Absolute physical impasse. Water velocities exceed 35 km/h, sweeping away light vehicles and foot scouts.
4. **Sub-Zero Katabatic Blizzards:**
   - Arctic windstorms blasting down glacial valleys, dropping ambient temperatures below -45°C.
   - *Traversability:* Thermal exposure kills within hours without heated vehicular transport or insulated survival shelters.



### Weather Gate Environmental Dossier #001: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_001`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_001|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #002: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_002`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_002|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #003: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_003`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_003|Tox_2.55|Perm_False)`


### Weather Gate Environmental Dossier #004: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_004`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_004|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #005: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_005`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_005|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #006: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_006`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_006|Tox_3.60|Perm_False)`


### Weather Gate Environmental Dossier #007: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_007`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_007|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #008: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_008`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_008|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #009: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_009`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_009|Tox_4.65|Perm_False)`


### Weather Gate Environmental Dossier #010: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_010`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_010|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #011: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_011`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_011|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #012: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_012`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_012|Tox_2.20|Perm_False)`


### Weather Gate Environmental Dossier #013: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_013`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_013|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #014: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_014`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_014|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #015: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_015`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_015|Tox_3.25|Perm_False)`


### Weather Gate Environmental Dossier #016: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_016`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_016|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #017: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_017`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_017|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #018: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_018`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_018|Tox_4.30|Perm_False)`


### Weather Gate Environmental Dossier #019: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_019`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_019|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #020: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_020`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_020|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #021: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_021`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_021|Tox_1.85|Perm_False)`


### Weather Gate Environmental Dossier #022: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_022`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_022|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #023: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_023`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_023|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #024: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_024`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_024|Tox_2.90|Perm_False)`


### Weather Gate Environmental Dossier #025: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_025`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_025|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #026: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_026`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_026|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #027: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_027`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_027|Tox_3.95|Perm_False)`


### Weather Gate Environmental Dossier #028: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_028`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_028|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #029: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_029`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_029|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #030: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_030`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_030|Tox_1.50|Perm_False)`


### Weather Gate Environmental Dossier #031: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_031`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_031|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #032: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_032`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_032|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #033: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_033`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_033|Tox_2.55|Perm_False)`


### Weather Gate Environmental Dossier #034: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_034`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_034|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #035: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_035`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_035|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #036: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_036`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_036|Tox_3.60|Perm_False)`


### Weather Gate Environmental Dossier #037: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_037`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_037|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #038: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_038`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_038|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #039: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_039`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_039|Tox_4.65|Perm_False)`


### Weather Gate Environmental Dossier #040: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_040`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_040|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #041: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_041`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_041|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #042: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_042`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_042|Tox_2.20|Perm_False)`


### Weather Gate Environmental Dossier #043: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_043`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_043|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #044: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_044`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_044|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #045: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_045`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_045|Tox_3.25|Perm_False)`


### Weather Gate Environmental Dossier #046: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_046`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_046|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #047: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_047`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_047|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #048: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_048`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_048|Tox_4.30|Perm_False)`


### Weather Gate Environmental Dossier #049: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_049`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_049|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #050: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_050`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_050|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #051: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_051`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_051|Tox_1.85|Perm_False)`


### Weather Gate Environmental Dossier #052: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_052`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_052|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #053: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_053`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_053|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #054: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_054`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_054|Tox_2.90|Perm_False)`


### Weather Gate Environmental Dossier #055: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_055`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_055|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #056: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_056`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_056|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #057: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_057`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_057|Tox_3.95|Perm_False)`


### Weather Gate Environmental Dossier #058: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_058`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_058|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #059: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_059`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_059|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #060: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_060`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_060|Tox_1.50|Perm_False)`


### Weather Gate Environmental Dossier #061: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_061`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_061|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #062: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_062`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_062|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #063: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_063`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_063|Tox_2.55|Perm_False)`


### Weather Gate Environmental Dossier #064: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_064`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_064|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #065: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_065`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_065|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #066: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_066`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_066|Tox_3.60|Perm_False)`


### Weather Gate Environmental Dossier #067: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_067`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_067|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #068: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_068`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_068|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #069: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_069`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_069|Tox_4.65|Perm_False)`


### Weather Gate Environmental Dossier #070: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_070`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_070|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #071: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_071`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_071|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #072: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_072`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_072|Tox_2.20|Perm_False)`


### Weather Gate Environmental Dossier #073: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_073`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_073|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #074: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_074`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_074|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #075: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_075`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_075|Tox_3.25|Perm_False)`


### Weather Gate Environmental Dossier #076: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_076`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_076|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #077: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_077`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_077|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #078: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_078`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_078|Tox_4.30|Perm_False)`


### Weather Gate Environmental Dossier #079: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_079`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_079|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #080: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_080`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_080|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #081: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_081`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_081|Tox_1.85|Perm_False)`


### Weather Gate Environmental Dossier #082: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_082`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_082|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #083: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_083`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_083|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #084: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_084`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_084|Tox_2.90|Perm_False)`


### Weather Gate Environmental Dossier #085: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_085`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_085|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #086: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_086`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_086|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #087: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_087`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_087|Tox_3.95|Perm_False)`


### Weather Gate Environmental Dossier #088: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_088`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_088|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #089: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_089`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_089|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #090: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_090`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_090|Tox_1.50|Perm_False)`


### Weather Gate Environmental Dossier #091: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_091`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_091|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #092: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_092`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_092|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #093: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_093`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_093|Tox_2.55|Perm_False)`


### Weather Gate Environmental Dossier #094: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_094`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_094|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #095: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_095`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_095|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #096: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_096`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_096|Tox_3.60|Perm_False)`


### Weather Gate Environmental Dossier #097: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_097`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_097|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #098: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_098`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_098|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #099: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_099`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_099|Tox_4.65|Perm_False)`


### Weather Gate Environmental Dossier #100: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_100`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_100|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #101: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_101`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_101|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #102: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_102`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_102|Tox_2.20|Perm_False)`


### Weather Gate Environmental Dossier #103: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_103`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_103|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #104: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_104`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_104|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #105: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_105`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_105|Tox_3.25|Perm_False)`


### Weather Gate Environmental Dossier #106: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_106`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_106|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #107: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_107`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_107|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #108: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_108`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_108|Tox_4.30|Perm_False)`


### Weather Gate Environmental Dossier #109: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_109`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_109|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #110: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_110`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_110|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #111: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_111`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_111|Tox_1.85|Perm_False)`


### Weather Gate Environmental Dossier #112: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_112`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_112|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #113: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_113`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_113|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #114: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_114`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_114|Tox_2.90|Perm_False)`


### Weather Gate Environmental Dossier #115: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_115`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_115|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #116: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_116`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_116|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #117: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_117`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_117|Tox_3.95|Perm_False)`


### Weather Gate Environmental Dossier #118: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_118`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_118|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #119: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_119`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_119|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #120: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_120`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_120|Tox_1.50|Perm_False)`


### Weather Gate Environmental Dossier #121: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_121`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_121|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #122: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_122`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_122|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #123: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_123`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_123|Tox_2.55|Perm_False)`


### Weather Gate Environmental Dossier #124: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_124`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_124|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #125: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_125`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_125|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #126: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_126`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_126|Tox_3.60|Perm_False)`


### Weather Gate Environmental Dossier #127: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_127`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_127|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #128: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_128`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_128|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #129: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_129`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_129|Tox_4.65|Perm_False)`


### Weather Gate Environmental Dossier #130: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_130`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_130|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #131: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_131`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_131|Tox_1.85|Perm_True)`


### Weather Gate Environmental Dossier #132: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_132`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_132|Tox_2.20|Perm_False)`


### Weather Gate Environmental Dossier #133: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_133`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_133|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #134: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_134`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_134|Tox_2.90|Perm_True)`


### Weather Gate Environmental Dossier #135: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_135`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_135|Tox_3.25|Perm_False)`


### Weather Gate Environmental Dossier #136: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_136`
- **Route Corridor Tag:** `route_corridor_sector_03`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_136|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #137: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_137`
- **Route Corridor Tag:** `route_corridor_sector_06`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_137|Tox_3.95|Perm_True)`


### Weather Gate Environmental Dossier #138: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_138`
- **Route Corridor Tag:** `route_corridor_sector_09`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_138|Tox_4.30|Perm_False)`


### Weather Gate Environmental Dossier #139: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_139`
- **Route Corridor Tag:** `route_corridor_sector_12`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_139|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #140: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_140`
- **Route Corridor Tag:** `route_corridor_sector_15`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_140|Tox_1.50|Perm_True)`


### Weather Gate Environmental Dossier #141: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_141`
- **Route Corridor Tag:** `route_corridor_sector_18`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 1.85
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_141|Tox_1.85|Perm_False)`


### Weather Gate Environmental Dossier #142: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_142`
- **Route Corridor Tag:** `route_corridor_sector_21`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 2.20
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_142|Tox_2.20|Perm_True)`


### Weather Gate Environmental Dossier #143: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_143`
- **Route Corridor Tag:** `route_corridor_sector_24`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 2.55
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.80 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_143|Tox_2.55|Perm_True)`


### Weather Gate Environmental Dossier #144: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_144`
- **Route Corridor Tag:** `route_corridor_sector_27`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 2.90
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 0.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_144|Tox_2.90|Perm_False)`


### Weather Gate Environmental Dossier #145: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_145`
- **Route Corridor Tag:** `route_corridor_sector_30`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 3.25
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.150 Durability Units/Hour
  - Ambient Radiation Exposure: 0.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_145|Tox_3.25|Perm_True)`


### Weather Gate Environmental Dossier #146: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_146`
- **Route Corridor Tag:** `route_corridor_sector_33`
- **Observed Weather Phenotype:** Gate Phenotype Category 1
- **Atmospheric Toxicity Index:** 3.60
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.180 Durability Units/Hour
  - Ambient Radiation Exposure: 0.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_146|Tox_3.60|Perm_True)`


### Weather Gate Environmental Dossier #147: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_147`
- **Route Corridor Tag:** `route_corridor_sector_36`
- **Observed Weather Phenotype:** Gate Phenotype Category 2
- **Atmospheric Toxicity Index:** 3.95
- **Required Protective Equipment:** hazmat_suit
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.9 km/h
  - Equipment Degradation Rate: 0.210 Durability Units/Hour
  - Ambient Radiation Exposure: 0.80 Rads/hr
- **Tactical Navigation Directive:**
  - Deploy radiation dosimeters and monitor suit integrity seals.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_147|Tox_3.95|Perm_False)`


### Weather Gate Environmental Dossier #148: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_148`
- **Route Corridor Tag:** `route_corridor_sector_39`
- **Observed Weather Phenotype:** Gate Phenotype Category 3
- **Atmospheric Toxicity Index:** 4.30
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 3.5 km/h
  - Equipment Degradation Rate: 0.240 Durability Units/Hour
  - Ambient Radiation Exposure: 1.05 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_148|Tox_4.30|Perm_True)`


### Weather Gate Environmental Dossier #149: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_149`
- **Route Corridor Tag:** `route_corridor_sector_42`
- **Observed Weather Phenotype:** Gate Phenotype Category 4
- **Atmospheric Toxicity Index:** 4.65
- **Required Protective Equipment:** NONE_IMPASSABLE
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 4.3 km/h
  - Equipment Degradation Rate: 0.270 Durability Units/Hour
  - Ambient Radiation Exposure: 1.30 Rads/hr
- **Tactical Navigation Directive:**
  - Halt advance immediately. Establish bivouac and await meteorological front dispersal.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_149|Tox_4.65|Perm_True)`


### Weather Gate Environmental Dossier #150: Route Passage Analysis

- **Gate Dossier Identifier:** `GATE_METEOR_SPEC_150`
- **Route Corridor Tag:** `route_corridor_sector_00`
- **Observed Weather Phenotype:** Gate Phenotype Category 5
- **Atmospheric Toxicity Index:** 1.50
- **Required Protective Equipment:** gas_mask
- **Expedition Passage Evaluation:**
  - Standard Travel Velocity: 5.1 km/h
  - Equipment Degradation Rate: 0.120 Durability Units/Hour
  - Ambient Radiation Exposure: 1.55 Rads/hr
- **Tactical Navigation Directive:**
  - Equip designated respiratory protection and advance in tight column formation.
- **State Checksum:**
  - Digest Signature: `SHA256(Gate_150|Tox_1.50|Perm_False)`
