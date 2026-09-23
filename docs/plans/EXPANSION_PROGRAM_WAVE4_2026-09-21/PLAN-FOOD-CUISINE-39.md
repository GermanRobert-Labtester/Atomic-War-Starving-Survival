# PLAN-FOOD-CUISINE-39 — Kitchen, Preservation Chains, Seed Bank & the Common Table

**Wave:** 4 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-VERTICAL-BODY-INDUSTRY-05 (table/water), PLAN-ORPHAN-SEAL-01
Wave 5, PLAN-DATA-CONSUMER-22.
**Expanded appendix:** [`PLAN-FOOD-CUISINE-39_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-FOOD-CUISINE-39_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's food & cuisine
systems (4 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no real-world culinary claims, no parallel food store
(`Inventory` + `FoodPreservationSystem` remain canonical), no food-safety
realism beyond the existing disease contract.

---

## 1. Outcome

Food is the game's central pressure, and its authored surface is deep but
half-wired: `crop_strains.json`, `hydroponic_crops.json`,
`food_preservation.json`, `food_types.json`, `nutrition_profiles.json`,
`recipes_cooking.json`, plus the host-unreachable `CookingSystem`,
`FoodTypeSystem`, `CommonTableRationingEngine`, `OilseedPressingEngine` and
the dead `LyophilizationEngine`, alongside live `GreenhouseSystem`,
`KitchenNutritionSystem`, `FoodPreservationSystem`, `NutritionDiversitySystem`,
`GrainProcessingSystem`, `ApicultureSystem`, `BrineWaterSystem`.

Player loop: **grow → harvest → process → cook → preserve → share → survive the
lean season**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Kitchen & menus | `CookingSystem`, `recipes_cooking.json`, `KitchenNutritionSystem` | assign cooks, choose menu | nutrition, morale, cook skill |
| Preservation | `FoodPreservationSystem`, `FoodTypeSystem`, `LyophilizationEngine` | smoke/salt/dry/ferment/freeze | shelf life, spoilage, cold-chain |
| Crops & seed | `CropStrains`, `FarmSystem/Greenhouse`, seed bank | rotate, plant, bank seed | yield, soil, blight risk |
| Specialty production | `ApicultureSystem`, `OilseedPressingEngine`, `BrineWaterSystem`, `GrainProcessingSystem` | keep hives, press, mill, salt | honey, oil, flour, salt |
| Rationing | `CommonTableRationingEngine` | set ration tier / table rules | needs, morale, fairness |
| Food culture | `NutritionDiversitySystem`, `nutrition_profiles.json` | vary meals | diversity bonuses, comfort |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Data | `crop_strains.json`, `hydroponic_crops.json`, `food_preservation.json`, `food_types.json`, `nutrition_profiles.json`, `recipes_cooking.json`, `recipes.json` |
| Core (live) | `GreenhouseSystem`, `KitchenNutritionSystem`, `FoodPreservationSystem`, `NutritionDiversitySystem`, `GrainProcessingSystem`, `ApicultureSystem`, `BrineWaterSystem` |
| Host-unreachable | `CookingSystem` (7 tests), `FoodTypeSystem` (8), `CommonTableRationingEngine`, `OilseedPressingEngine` (5), `LyophilizationEngine` (dead, 0 refs) |
| Sealed prior | Plan 196 food-type/temp seam (8/8), Plan 136 wildlife→cooking bridge, Plan 91 greenhouse items, sanitation→disease modifier |
| Contract | spoilage authority map `docs/shelter/PLAN_196_FOOD_SPOILAGE_AUTHORITY_MAP.md` |

---

## 3. Packages

### FD-39A — Kitchen, recipes and menus
- Bind `CookingSystem` + `recipes_cooking.json` to a kitchen room and cook
  duty; menus selected per day affect nutrition and morale; cook skill reduces
  time/raises quality (already authored bands).
- **Acceptance:** no shadow food store; outputs are inventory items; recipe
  rows all resolve; cooked/well-cooked/burnt outcomes deterministic.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Kitchen/`.

### FD-39B — Preservation chains
- `FoodTypeSystem` gates preservation methods per food type + temperature
  (Plan 196 contract); `LyophilizationEngine` becomes the cold-chain branch
  with power dependency; spoilage reports are visible.
- **Acceptance:** every food type maps to allowed methods; spoilage is
  explainable; power loss accelerates loss; no duplicate spoilage timer.
- **Verify:** `bash scripts/run_test.sh` food preservation suites.

### FD-39C — Crops, seed bank and soil
- Rotation and seed banking: saved seed has quality/variety; soil depletion
  and reclamation route through `SoilReclamationProfileEngine`; blight events
  tie to ecology (PLAN-ECOLOGY-WILDLIFE-26).
- **Acceptance:** yield responds to rotation/soil/water; seed loss is
  recoverable; no infinite seed duplication.
- **Verify:** greenhouse/farming focused suites + balance sim.

### FD-39D — Specialty production
- Beehives (pollination modifier), oil pressing, grain milling, brine salt —
  each a small production chain feeding the kitchen/preservation.
- **Acceptance:** chains consume real inputs and produce real items; apiculture
  pollination affects crop yield measurably; no orphan outputs.
- **Verify:** `bash scripts/run_test.sh` apiculture/grain suites.

### FD-39E — Common table and rationing
- `CommonTableRationingEngine` sets a table policy (equal shares, priority to
  workers/children/patients); policy is a visible, changeable decision with
  morale and need consequences; starvation routes through needs.
- **Acceptance:** policy changes are explainable; no silent starvation;
  fairness perception is visible; determinism.
- **Verify:** needs + nutrition focused suites.

### FD-39F — Food culture and diversity
- Favourite meals, diversity tracking, and comfort food from
  `nutrition_profiles.json`; lean-season memory makes shortages bite harder if
  the table was rich before.
- **Acceptance:** diversity bonuses bounded; references use the morale-mark
  owner; no parallel morale stat.
- **Verify:** nutrition suite + balance sim.

### FD-39G — Content volumes
- +12 recipes, +6 preservation methods, +8 crop strains, +6 meals/comfort
  rows, +4 specialty chains; fictional; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Food system overload | one kitchen panel region, daily summary; deep options behind it |
| Preservation trivializes scarcity | power/cost/time gates; pests return in spring |
| Rationing feels unfair | explicit policy preview; fairness perception visible |
| Recipe/JSON sprawl | all rows resolve to existing items; field-consumption gate |

## 5. Verification

```bash
godot --headless --path . -- --agriculture-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Kitchen/
bash scripts/run_test.sh Ashfall.Core.Tests/Nutrition/
bash scripts/run_test.sh Ashfall.Core.Tests/Farming/
```

---

## 6. Expanded census (3 files · 960 lines)

Scope: `Assets/Ashfall.Core/Kitchen/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CookingSystem.cs` | 395 | System | **yes** | 0 | 0 | 2 |
| `FoodTypeSystem.cs` | 267 | System | **yes** | 0 | 1 | 2 |
| `CommonTableRationingEngine.cs` | 298 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 1 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `desperation_events.json` | object[2 keys] |
| `cryogenic_air_separation.json` | object[3 keys] |
| `electrostatic_filtration_catalog.json` | object[2 keys] |
| `food_preservation.json` | object[4 keys] |
| `gpr_exploration_catalog.json` | object[5 keys] |
| `recipes_cooking.json` | object[2 keys] |

**State surfaces:** `CookingSystem.cs`, `FoodTypeSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Cooking/` |
| Test references | 4 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

Domain files: 3. Other plans referencing their names: **4**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-PRESERVATION-TRUTH-118` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FD-39A` | no name match — resolve at claim time |
| `FD-39B` | no name match — resolve at claim time |
| `FD-39C` | no name match — resolve at claim time |
| `FD-39D` | no name match — resolve at claim time |
| `FD-39E` | `CommonTableRationingEngine.cs` |
| `FD-39F` | no name match — resolve at claim time |
| `FD-39G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **0** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Cooking/Plan136WildlifeCookingIntegrationTests.cs`, `Ashfall.Core.Tests/Kitchen/Plan196FoodTypeIntegrationTests.cs`, `Ashfall.Core.Tests/Kitchen/Plan22_40FoodIdentityIntegrationTests.cs`, `Ashfall.Core.Tests/Nutrition/CommonTableRationingEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `food_preservation` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **4**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/food_preservation.json` |
| `Assets/StreamingAssets/Data/food_types.json` |
| `Assets/StreamingAssets/Data/rationing_protocols.json` |
| `Assets/StreamingAssets/Data/recipes_cooking.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 5 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Cooking` | 1 | 5 |

**Verdict:** 5 cases sit under matching regions — run those first (`Cooking`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **1**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/FoodPreservationSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `food_preservation` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **0**
(none).

| Catalog | Classification |
|---|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog matches — the domain is code-authoritative or its data lives in a broader catalog.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 1 · catalogs 4 · test regions 1 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FOOD-CUISINE-39
wave: —
status: PROPOSED — foreman claim required
packages: FD-39A, FD-39B, FD-39C, FD-39D, FD-39E, FD-39F, FD-39G
claim paths:
  - src/Host/FoodPreservationSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/food_preservation.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/food_types.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Cooking/
dependencies:
  - coordinate: 4 other plan(s) name these artifacts (§12)
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
