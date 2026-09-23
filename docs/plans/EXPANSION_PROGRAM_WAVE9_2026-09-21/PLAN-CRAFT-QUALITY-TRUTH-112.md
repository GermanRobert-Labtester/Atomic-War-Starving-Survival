# PLAN-CRAFT-QUALITY-TRUTH-112 — Produced-Goods Quality Tiers, Marking & Consumer Effects

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-INVENTORY-CONSERVATION-93, PLAN-FOOD-CUISINE-39.
**Non-goals:** no chit purity/contraband grading (Plan 44/96 seam), no new item
catalog, no second inventory store.

## 1. Outcome
`Crafting/CraftingSystem.cs` and the industry engines (Plan 45) produce goods,
and inventory is an owned authority (Plan 93). What is missing is **quality**:
whether a produced item's tier (crude/standard/fine) is determined by inputs,
station condition, and skill; whether the tier is marked on the item; and
whether consumers (kitchen, medicine, trade) actually read it. Without this, a
"quality" field would be decoration.

| Deliverable | Detail |
|---|---|
| Quality model | tier from documented inputs: skill, station condition, material grade — deterministic, seeded where variance exists |
| Item marking | quality rides the existing item instance (no parallel catalog); stack rules stated |
| Consumer effects | each consumer that reads quality names the effect (meal value, medical outcome, trade price within Plan 96 rules) |
| No-phantom rule | an item without a quality mark behaves as the documented default, never a hidden roll |
| Trade truth | price effect flows through the market seam, not a display-only label |

## 2. Evidence
- `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` (exists; re-verify seam per package).
- Plan 45 owns industry engines incl. `CupolaFoundryEngine`, `KilnFiringEngine` (host-unreachable, Plan 1 Appendix A).
- Plan 93 owns conservation; quality marking must not change counts.
- Plan 96 owns economy arithmetic; a price effect is a row there, not a new counter.

## 3. Packages
- **CQT-112A** quality model doc + deterministic roll rules (seed from the owning stream).
- **CQT-112B** marking on the item instance + stack behavior tests.
- **CQT-112C** consumer effect table + one test per consumer named.
- **CQT-112D** trade price row handed to Plan 96's map; no local multiplier.
- **CQT-112E** default-without-mark test + conservation replay (counts unchanged).

## 4. Acceptance & verification
- Same inputs → same tier across runs; variance only through the seeded stream.
- Consumers show the documented effect; removing the mark yields default behavior.
- Conservation test: quality marking changes no item counts.
- `bash scripts/run_test.sh` on the crafting/inventory regions.

## 5. Risks
Tier inflation → inputs documented and tested; tier distribution report generated, not tuned by hand here.
Duplicate grading systems → chit purity and contraband stay outside this plan's scope explicitly.

---

## 6. Expanded census (5 files · 892 lines)

Scope: `Assets/Ashfall.Core/Crafting/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Loader 2 · Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CraftContext.cs` | 29 | Support | — | 0 | 0 | 0 |
| `CraftingSystem.cs` | 496 | System | **yes** | 0 | 0 | 2 |
| `PharmaRecipeCatalogLoader.cs` | 107 | Loader | — | 0 | 0 | 0 |
| `RecipeCatalogLoader.cs` | 179 | Loader | **yes** | 0 | 0 | 0 |
| `TrapRecipeIntegrity.cs` | 81 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `aircraft_parts.json` | object[2 keys] |
| `glassworks_recipes.json` | object[4 keys] |
| `metallurgy_recipes.json` | object[4 keys] |
| `recipes.json` | object[2 keys] |
| `relic_recipes.json` | object[2 keys] |
| `workshop_recipes.json` | object[2 keys] |

**State surfaces:** `CraftingSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Crafting/` |
| Test references | 47 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 5. Other plans referencing them: **3**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-TRIO-FAMILY-TRUTH-280` | 5 |
| `PLAN-RECIPE-REACHABILITY-TRUTH-125` | 4 |
| `EVIDENCE` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `CQT-112A` | no name match — resolve at claim time |
| `CQT-112B` | no name match — resolve at claim time |
| `CQT-112C` | no name match — resolve at claim time |
| `CQT-112D` | no name match — resolve at claim time |
| `CQT-112E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **12** · Test files: **39** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 12 | `src/Audio/AudioEventBridge.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/CraftingHostSession.cs`, `src/Host/EquipmentConditionHostSession.cs`, `src/Host/HostCli.Difficulty.cs` |
| Tests (`Ashfall.Core.Tests/`) | 39 | `Ashfall.Core.Tests/CampaignContinuityFlagshipTests.cs`, `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`, `Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs`, `Ashfall.Core.Tests/CraftingAfflictionLoopTests.cs`, `Ashfall.Core.Tests/CraftingCommandTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **1**; isolated: **3**.

| From | → To |
|---|---|
| `TrapRecipeIntegrity` | `RecipeCatalogLoader` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `crafting` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--data-integrity-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnCraftCompleted` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnCraftStarted` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnCraftingPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnPharmaStateChanged` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **4**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/chef_recipe_development.json` |
| `Assets/StreamingAssets/Data/narrative/steam_trap_water_hammer_logs.json` |
| `Assets/StreamingAssets/Data/narrative/water_quality_test_reports_batch_2.json` |
| `Assets/StreamingAssets/Data/pharma_recipes.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 11 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Crafting` | 1 | 11 |

**Verdict:** 11 cases sit under matching regions — run those first (`Crafting`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **8**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/CraftingHostSession.cs` |
| `src/Host/CraftingSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/UI/CraftingPanel.cs` |
| `src/UI/PanelSceneLoader.cs` |
| `src/UI/PharmaLabPanel.cs` |
| `src/UI/PharmaLabPanelContent.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `crafting` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `narrative/chef_recipe_development.json` | CODEX_ONLY |
| `narrative/steam_trap_water_hammer_logs.json` | CODEX_ONLY |
| `narrative/water_quality_test_reports_batch_2.json` | CODEX_ONLY |
| `pharma_recipes.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 8 · catalogs 8 · test regions 1 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CRAFT-QUALITY-TRUTH-112
wave: 9
status: PROPOSED — foreman claim required
packages: CQT-112A, CQT-112B, CQT-112C, CQT-112D, CQT-112E
claim paths:
  - src/Host/CraftingHostSession.cs  # §19 candidate host surface
  - src/Host/CraftingSaveStore.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/LoaderWiringSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/chef_recipe_development.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/steam_trap_water_hammer_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Crafting/
  - godot --headless --path . -- --data-integrity-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
