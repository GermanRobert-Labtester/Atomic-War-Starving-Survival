# PLAN-CATALOG-BOOT-TRUTH-148 — Key Normalization, Load Results & Diagnostics Contract

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DATA-SCHEMA-COVERAGE-90, PLAN-ARCHITECTURE-BOUNDARY-31, PLAN-DATA-CONSUMER-22.
**Implementation scaffold:** [`PLAN-CATALOG-BOOT-TRUTH-148_APPENDIX-A_SCAFFOLD.md`](PLAN-CATALOG-BOOT-TRUTH-148_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-DATA-SCHEMA-COVERAGE-90` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new loader framework, no data rewrites; the validators here
extend the existing boot path.

## 1. Outcome
`IO/` holds four types that define how catalogs come up: `CatalogBootValidator`,
`CatalogDiagnostics`, `CatalogKeyNormalizer`, `CatalogLoadResult`. Nothing
documents the normalizer's rules (case, separators, aliases), the result's
failure classes, or what diagnostics a boot failure must carry — so a bad
catalog can fail differently in each caller.

| Deliverable | Detail |
|---|---|
| Normalizer rules | exact transformation table (case, separators, aliases, unicode) with fixtures |
| Load-result classes | success/partial/failed with the fields each carries; no silent partial load |
| Diagnostics contract | every failure names file, record, field, and expected shape; diagnostics survive to the player-facing error path |
| Boot policy | which catalogs are required vs optional at boot; an optional failure degrades visibly, never silently |
| Interplay | schema validation (Plan 90) runs before boot acceptance; a schema failure is one of the result classes |

## 2. Evidence
- `Assets/Ashfall.Core/IO/`: the four files above (verified).
- Plan 31 Appendix A: 672 IO call sites — the boot path is one of the few IO owners, not a parallel loader.
- Plan 90 adds the schema stage this plan's result classes must carry.
- Plan 22's consumer gaps read the same catalog ids this normalizer maps.

## 3. Packages
- **CBT-148A** normalizer rule table + fixtures (case/separator/alias/unicode).
- **CBT-148B** load-result class contract + one test per class.
- **CBT-148C** diagnostics field contract + a boot-failure fixture carrying all fields.
- **CBT-148D** boot policy table (required/optional per catalog class) + degradation tests.
- **CBT-148E** integration note: schema failure is a result class, not an exception path.

## 4. Acceptance & verification
- Normalizer fixtures match the table exactly; no caller does its own casing.
- A required-catalog failure aborts with typed diagnostics; an optional failure degrades visibly.
- Partial loads never report success.
- `bash scripts/run_test.sh Ashfall.Core.Tests/IO/` (create if absent).

## 5. Risks
Normalizer becoming a second schema → it normalizes keys only; validation stays in Plan 90.
Boot strictness → required/optional is a table with an owner per catalog class.

---

## 6. Expanded census (4 files · 961 lines)

Scope: `Assets/Ashfall.Core/IO/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CatalogBootValidator.cs` | 339 | Support | **yes** | 0 | 0 | 0 |
| `CatalogDiagnostics.cs` | 71 | Support | — | 0 | 0 | 0 |
| `CatalogKeyNormalizer.cs` | 200 | Support | **yes** | 0 | 0 | 0 |
| `CatalogLoadResult.cs` | 351 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `microfluidic_diagnostic_catalog.json` | object[4 keys] |
| `geothermal_steam_vent_diagnostics.json` | array[7] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/IO/` (create if absent) |
| Test references | 24 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-DATA-SCHEMA-COVERAGE-90` | 3 |
| `PLAN-REFERENCE-INTEGRITY-34` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CBT-148A` | `CatalogKeyNormalizer.cs` |
| `CBT-148B` | `CatalogLoadResult.cs` |
| `CBT-148C` | no name match — resolve at claim time |
| `CBT-148D` | `CatalogBootValidator.cs`, `CatalogKeyNormalizer.cs`, `CatalogLoadResult.cs` |
| `CBT-148E` | `CatalogLoadResult.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **12** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 12 | `src/Host/RadioHostSession.cs`, `src/Main.Application.cs`, `src/UI/AshfallUiHelpers.cs`, `src/UI/CaravanBarterLedgerPanel.cs`, `src/UI/FactionCommuniqueBoardPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/CatalogLoadContractTests.cs`, `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs`, `Ashfall.Core.Tests/Data/SnakeCaseWave1Tests.cs`, `Ashfall.Core.Tests/District8DeepCoastTests.cs`, `Ashfall.Core.Tests/Tooling/CatchPolicyLintGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **3**; isolated: **0**.

| From | → To |
|---|---|
| `CatalogBootValidator` | `CatalogDiagnostics` |
| `CatalogKeyNormalizer` | `CatalogDiagnostics` |
| `CatalogLoadResult` | `CatalogDiagnostics` |

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

Matching flags in `HostCliRegistry.cs`: **4** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--save-load-failure-selftest` |
| `--save-load-failure-uitest` |
| `--save-load-selftest` |
| `--save-load-ui-failure-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/geothermal_steam_vent_diagnostics.json` |
| `Assets/StreamingAssets/Data/narrative/load_shed_schedule_001.json` |
| `Assets/StreamingAssets/Data/narrative/rope_break_load_assays.json` |

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

Host files (`src/`) whose names share a domain token: **4**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/SaveLoadHostSession.cs` |
| `src/Host/SaveLoadUiFailureSelfTest.cs` |
| `src/UI/SaveLoadPanel.cs` |
| `src/UI/UiNodeDiagnostics.cs` |

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

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `medical_microfluidic_diagnostics` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(CODEX_ONLY 3).

| Catalog | Classification |
|---|---|
| `narrative/geothermal_steam_vent_diagnostics.json` | CODEX_ONLY |
| `narrative/load_shed_schedule_001.json` | CODEX_ONLY |
| `narrative/rope_break_load_assays.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 0 (laddered 0) · RNG streams 1 · host files 5 · catalogs 6 · test regions 0 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CATALOG-BOOT-TRUTH-148
wave: 12
status: PROPOSED — foreman claim required
packages: CBT-148A, CBT-148B, CBT-148C, CBT-148D, CBT-148E
claim paths:
  - src/Host/SaveLoadHostSession.cs  # §19 candidate host surface
  - src/Host/SaveLoadUiFailureSelfTest.cs  # §19 candidate host surface
  - src/UI/SaveLoadPanel.cs  # §19 candidate host surface
  - src/UI/UiNodeDiagnostics.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/geothermal_steam_vent_diagnostics.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/load_shed_schedule_001.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --save-load-failure-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
