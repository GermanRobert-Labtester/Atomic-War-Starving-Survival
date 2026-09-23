# PLAN-RECIPE-REACHABILITY-TRUTH-125 — Every Recipe Craftable, Stations Bound, Substitutes Legal

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-CRAFT-QUALITY-TRUTH-112, PLAN-REFERENCE-INTEGRITY-34, PLAN-DATA-SCHEMA-COVERAGE-90.
**Non-goals:** no new crafting system, no balance pass (Plan 73), no
second recipe store.

## 1. Outcome
`Crafting/CraftingSystem.cs` and `Crafting/RecipeCatalogLoader.cs` exist; the
`item_` id space is 332 ids across 112 files (Plan 34 Appendix A). Nothing
verifies that a recipe's inputs exist and are obtainable, that its station is
buildable, or that substitutions are legal — so "content reachability" for
crafting is unproven even when the ids parse.

| Deliverable | Detail |
|---|---|
| Reachability table | per recipe: inputs exist, each input has a source (loot/vendor/other recipe), station exists in the build catalog, output not orphaned |
| Station binding | every station type referenced by a recipe is constructible; a recipe on an unobtainable station is reported, not silently ignored |
| Substitution rules | legal substitutes are declared per recipe (tag-based), bounded; a substitute that changes quality tier is documented |
| Cycle report | recipes that depend on each other in a cycle are listed with the break condition |
| Report-only rule | unreachable entries are reported to their content owner; this plan rewrites no data |

## 2. Evidence
- `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`, `Crafting/RecipeCatalogLoader.cs` (re-verify per package).
- Plan 34 Appendix A: `item_` 332 ids / 112 files — the id space this table walks.
- Plan 112 owns produced-goods quality; substitution quality effects reference it, not a new tier system.
- Plan 90's validator stage hosts the schema; this plan adds a reachability report, not a second validator.

## 3. Packages
- **RRT-125A** reachability table generator (recipes × inputs × sources × stations); `--check` mode.
- **RRT-125B** station constructibility check against the build catalog.
- **RRT-125C** substitution rule model + tests for legal/illegal swaps.
- **RRT-125D** dependency cycle report with break conditions.
- **RRT-125E** owner report for unreachable rows (no data edits here).

## 4. Acceptance & verification
- Table covers every loaded recipe; unknown ids appear as gaps, never dropped silently.
- A recipe with a missing input or unobtainable station is listed with its owner.
- A legal substitution crafts; an undeclared swap is refused typed.
- `bash scripts/run_test.sh` on the crafting region + table `--check`.

## 5. Risks
Scope creep into balance → reachability only; numbers untouched.
Stale build catalog knowledge → stations are read from the build catalog at check time.

---

## 6. Expanded census (3 files · 367 lines)

Scope: `Assets/Ashfall.Core/Crafting/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Loader 2 · Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PharmaRecipeCatalogLoader.cs` | 107 | Loader | — | 0 | 0 | 0 |
| `RecipeCatalogLoader.cs` | 179 | Loader | **yes** | 0 | 0 | 0 |
| `TrapRecipeIntegrity.cs` | 81 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `glassworks_recipes.json` | object[4 keys] |
| `metallurgy_recipes.json` | object[4 keys] |
| `recipes.json` | object[2 keys] |
| `relic_recipes.json` | object[2 keys] |
| `workshop_recipes.json` | object[2 keys] |
| `pharma_recipes.json` | object[2 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Crafting/` |
| Test references | 12 name references across the test tree |
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

Domain files: 5. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-CRAFT-QUALITY-TRUTH-112` | 5 |
| `PLAN-TRIO-FAMILY-TRUTH-280` | 5 |
| `EVIDENCE` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RRT-125A` | no name match — resolve at claim time |
| `RRT-125B` | `PharmaRecipeCatalogLoader.cs`, `RecipeCatalogLoader.cs` |
| `RRT-125C` | no name match — resolve at claim time |
| `RRT-125D` | no name match — resolve at claim time |
| `RRT-125E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **3** · Test files: **10** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/CraftingHostSession.cs`, `src/Host/HostCli.Plans162_165.cs` |
| Tests (`Ashfall.Core.Tests/`) | 10 | `Ashfall.Core.Tests/CampaignContinuityFlagshipTests.cs`, `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`, `Ashfall.Core.Tests/Data/RuntimeJsonBootstrapParityTests.cs`, `Ashfall.Core.Tests/Plan10RemediationTests.cs`, `Ashfall.Core.Tests/ProductionSliceTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `TrapRecipeIntegrity` | `RecipeCatalogLoader` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **0** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| — | no section key shares a token with this domain |

**Verdict:** no section key shares a token with this domain — either the authority is derived/stateless, or its persistence key is named after a different owner. Not a conclusion; verify in the owning system.

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

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnPharmaStateChanged` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/chef_recipe_development.json` |
| `Assets/StreamingAssets/Data/narrative/steam_trap_water_hammer_logs.json` |
| `Assets/StreamingAssets/Data/pharma_recipes.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **5**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/UI/PanelSceneLoader.cs` |
| `src/UI/PharmaLabPanel.cs` |
| `src/UI/PharmaLabPanelContent.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `narrative/chef_recipe_development.json` | CODEX_ONLY |
| `narrative/steam_trap_water_hammer_logs.json` | CODEX_ONLY |
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
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 5 · catalogs 6 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RECIPE-REACHABILITY-TRUTH-125
wave: 10
status: PROPOSED — foreman claim required
packages: RRT-125A, RRT-125B, RRT-125C, RRT-125D, RRT-125E
claim paths:
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/LoaderWiringSelfTest.cs  # §19 candidate host surface
  - src/UI/PanelSceneLoader.cs  # §19 candidate host surface
  - src/UI/PharmaLabPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/chef_recipe_development.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/steam_trap_water_hammer_logs.json  # §17 catalog (verify schema + consumer)
verification:
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
