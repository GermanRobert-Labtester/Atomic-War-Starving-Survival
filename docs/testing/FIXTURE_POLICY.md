# ASHFALL — Fixture Policy & Testing Invariants (Plan 27 / Wave 10)

> **Authority:** `C1_planintegration[6].md` (Continuity Wave 3 — *Ship It Intact*)\
> **Status:** ACTIVE\
> **Last Updated:** 2026-09-17\

---

## 1. Core Principles

ASHFALL tests must mean what the game means. Testing synthetic demo definitions or bypassing the campaign composition root measures the wrong game.

The testing architecture recognizes exactly **two legal categories** of test fixtures:

```
                  ┌──────────────────────────────────────────────┐
                  │           ASHFALL Test Fixtures              │
                  └──────────────────────┬───────────────────────┘
                                         │
                 ┌───────────────────────┴───────────────────────┐
                 ▼                                               ▼
   AUTHORITY-BACKED FIXTURES                       EXPLICIT SYNTHETIC FIXTURES
   • Shipped JSON catalog authority               • Isolated pure mathematical formulas
   • Production composition root & loaders        • Edge cases & boundary values
   • Pinned deterministic seeds                   • Malformed data & failure recovery
   • Used in: integration, selftests,             • Used in: unit tests, stress tests,
     save/load, journeys, content evidence          micro-benchmarks
```

---

## 2. Invariants

- **INV-27.1 — Production selftests use shipped authorities:**\
  Any selftest intended to certify the game must use the same data directory resolution (`CatalogPath.ResolveDataDir()`), catalog loaders (`ItemCatalogLoader`), composition root, and campaign owner graph as the shipped runtime.
- **INV-27.2 — Synthetic fixtures are explicit:**\
  Synthetic fixtures are permitted ONLY when explicitly constructed via dedicated fixture APIs (e.g. `InventoryHostSession.CreateForFixture()`, `SeedCatalogForTest()`, `CampaignFixture.CreateSynthetic()`) and clearly named.
- **INV-27.3 — No implicit fallback systems in selftests:**\
  A host selftest must not silently instantiate a fresh campaign-owned system (such as `DutyRosterSystem`, `SkillProgressionSystem`, etc.) when testing production behavior.
- **INV-27.4 — Behavioral assertions over presence:**\
  Tests must assert observable state transitions, reference identity, and downstream consumer consequences (Level 3–5) rather than non-null or type-presence checks (Level 0–1).

---

## 3. Category 1: Authority-Backed Fixtures

### When to Use
- Testing shipped gameplay systems and mechanics (inventory, radiation, food, crafting, combat, expeditions).
- Host selftests (`--*-selftest`) and UI test passes.
- Save/load round-trips and persistence validation.
- Real campaign journeys (first-hour, 30-day, epilogue).
- Content utilization and runtime evidence collection (`DESERIALIZED` → `REGISTERED` → `QUERIED` → `SELECTED` → `EFFECT_PRODUCED`).

### How to Construct
Always construct using the canonical loader and resolved data root:

```csharp
// Authority-backed InventoryHostSession
var invSession = InventoryHostSession.Create(dataDir, seedWhenNoSave: true);

// Authority-backed CampaignFixture
var fixture = CampaignFixture.CreateAuthorityBacked(dataDir, seed: 4242);
```

### Characteristics
- Backed by `Assets/StreamingAssets/Data/items.json`, `recipes.json`, etc.
- Uses `FileSystemIO` or Godot virtual filesystem via `CatalogPath.CreateFileIOForDataDir(dataDir)`.
- Verifies that real catalog tags, restore values, weights, and costs operate in concert.

---

## 4. Category 2: Explicit Synthetic Fixtures

### When to Use
- Narrow unit tests for isolated mathematical functions (e.g. quadratic decay, exp curve, radiation formula).
- Testing impossible states or corrupted data inputs that should never appear in valid catalogs.
- Extreme boundary value tests (e.g. integer overflow, negative weight, 0-durability instant drop).

### How to Construct
Must call unmistakable synthetic constructors:

```csharp
// Explicit synthetic fixture:
var invSession = InventoryHostSession.CreateForFixture();
// Or with custom test catalog:
var customCatalog = new ItemCatalog();
customCatalog.Register(new ItemDefinition { id = "test_item_999", stackMax = 99 });
var invSession = InventoryHostSession.CreateForFixture(catalog: customCatalog);

// Explicit synthetic campaign fixture:
var fixture = CampaignFixture.CreateSynthetic(seed: 12345);
```

### Prohibited Patterns
- Default constructors silently seeding demo data into production paths:
  ```csharp
  // PROHIBITED in production selftest:
  var session = new InventoryHostSession(); // does not load items.json
  ```
- Adding fresh campaign-owned systems inside host selftests:
  ```csharp
  // PROHIBITED in host selftest:
  var duty = new DutyRosterSystem(); // bypassing Main._dutyRoster or CampaignFixture
  ```

---

## 5. Naming Convention Guidelines

When writing new tests or fixtures, use semantic prefixes where fixture ambiguity could exist:

| Prefix | Purpose | Example |
|---|---|---|
| `AuthorityBacked_` | Verifies real shipped catalogs and systems | `AuthorityBacked_CannedFood_RestoresAuthoredHunger()` |
| `Synthetic_` | Isolates an algorithm or edge-case | `Synthetic_NeedsDecay_NegativeInput_ClampsToZero()` |
| `RealCampaign_` | Cross-system multi-step journey | `RealCampaign_FirstHour_SurvivorTreatment_Persists()` |
| `GoldenSave_` | Verifies persisted envelope against frozen baseline | `GoldenSave_EarlyCampaign_ChecksumMatches()` |

---

## 6. Enforcement & Verification

This policy is enforced continuously by:
1. `Ashfall.Core.Tests/Tooling/DataAuthorityFidelityTests.cs` — verifies that items.json properties match runtime expectations and flags divergent synthetic fixtures.
2. `Ashfall.Core.Tests/Tooling/NoFreshCampaignSystemGateTests.cs` — source gate scanning `src/Host/*SelfTest*.cs` and `src/Main.UiTests*.cs` for unauthorized direct campaign-system instantiations.
3. `Ashfall.Core.Tests/Save/GoldenSaveFixtureTests.cs` — validates early, mid, and late campaign golden save envelopes (`artifacts/golden_saves/`), aggregate checksums, and determinism digests.
4. `scripts/ci/verify-fast.sh` — gating all selftests against canonical data paths.
