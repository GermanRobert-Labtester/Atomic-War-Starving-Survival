# PLAN-DATA-SCHEMA-COVERAGE-90 — Structural Schemas for the Consumed Catalogs

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DATA-AUTHORITY-14, PLAN-DATA-CONSUMER-22, PLAN-REFERENCE-INTEGRITY-34.
**Implementation scaffold:** [`PLAN-DATA-SCHEMA-COVERAGE-90_APPENDIX-A_SCAFFOLD.md`](PLAN-DATA-SCHEMA-COVERAGE-90_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-DATA-AUTHORITY-14` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no schema for every file at once, no runtime JSON-schema
engine, no second validator beside the existing integrity pipeline.

## 1. Outcome
`Assets/StreamingAssets/Data` holds **703 JSON files**. Exactly one carries a
structural schema today: `Data/mod_manifest_schema.json`. Everything else is
checked (when checked) by the integrity pipeline and by consumers. Plan 14
classifies catalog usage, Plan 22 finds consumer gaps, Plan 34 owns
**reference** integrity — none of them owns **structural** schema coverage:
field names, types, required fields, ranges, and `schema_version` consistency.

| Deliverable | Detail |
|---|---|
| Coverage triage | the GAMEPLAY_CONSUMED catalogs (Plan 14 class) ordered by blast radius; first wave gets schemas |
| Schema authoring | one schema per triaged catalog: snake_case ids, required fields, types, enums, numeric ranges |
| Validator extension | the existing integrity pipeline gains a schema stage; no parallel validator |
| Version rule | `schema_version` present and consistent with the structural shape; bump rules written down |
| Drift gate | schema ↔ catalog diff: unknown fields and missing required fields fail with per-row output |

## 2. Evidence
- 703 JSON files; `find` shows a single `*.schema.json` under `Data/` (`mod_manifest_schema.json`).
- `artifacts/content-utilization-baseline.json`: GAMEPLAY_CONSUMED 162 · CODEX_ONLY 279 · OPTIONAL 24 · UNRESOLVED 73 (Plan 14 Appendix A).
- Plan 34 Appendix A: catalog id families (`item_` 332 ids / 112 files, `loc_` 117/59, `faction_` 43/60, `quest_` 300/14).
- Skills and repo rules already require snake_case and schema-valid JSON.

## 3. Packages
- **DSC-90A** triage table: consumed catalogs ranked; first-wave schema list (bounded, e.g. 15).
- **DSC-90B** schemas: authored under `Data/schemas/` with the established naming; one PR per catalog family.
- **DSC-90C** validator stage: extend the current integrity entry point; report file + path + expected type.
- **DSC-90D** drift gate: unknown/missing field diff per catalog; catalog edits that break shape fail focused verification.
- **DSC-90E** docs: version rule + authoring example; index of covered vs pending catalogs.

## 4. Acceptance & verification
- First-wave catalogs validate; injected type/field violations fail with path-level output.
- `schema_version` rule verified on one bump scenario.
- `bash scripts/run_test.sh` on the data-integrity test region; `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 5. Risks
Schema sprawl → only consumed catalogs are triaged in this plan's scope;
CODEX_ONLY files are listed, not schema'd. False negatives → schemas declare
required fields explicitly and the drift gate catches unknown fields.

---

## 6. Expanded census (3 files · 890 lines)

Scope: `Assets/Ashfall.Core/IO/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CatalogBootValidator.cs` | 339 | Support | **yes** | 0 | 0 | 0 |
| `CatalogKeyNormalizer.cs` | 200 | Support | **yes** | 0 | 0 | 0 |
| `CatalogLoadResult.cs` | 351 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `mod_manifest_schema.json` | object[9 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/IO/` (create if absent) |
| Test references | 21 name references across the test tree |
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
| `PLAN-CATALOG-BOOT-TRUTH-148` | 3 |
| `PLAN-REFERENCE-INTEGRITY-34` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DSC-90A` | no name match — resolve at claim time |
| `DSC-90B` | `CatalogBootValidator.cs`, `CatalogKeyNormalizer.cs`, `CatalogLoadResult.cs` |
| `DSC-90C` | `CatalogBootValidator.cs` |
| `DSC-90D` | `CatalogBootValidator.cs`, `CatalogKeyNormalizer.cs`, `CatalogLoadResult.cs` |
| `DSC-90E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.Application.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/CatalogLoadContractTests.cs`, `Ashfall.Core.Tests/Data/SnakeCaseWave1Tests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

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

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--asset-coverage-report` |
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
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |
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

Host files (`src/`) whose names share a domain token: **5**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/AssetCoverageReport.cs` |
| `src/Host/AssetCoverageScanner.cs` |
| `src/Host/SaveLoadHostSession.cs` |
| `src/Host/SaveLoadUiFailureSelfTest.cs` |
| `src/UI/SaveLoadPanel.cs` |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(CODEX_ONLY 2).

| Catalog | Classification |
|---|---|
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
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 5 · catalogs 5 · test regions 0 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DATA-SCHEMA-COVERAGE-90
wave: 8
status: PROPOSED — foreman claim required
packages: DSC-90A, DSC-90B, DSC-90C, DSC-90D, DSC-90E
claim paths:
  - src/Host/AssetCoverageReport.cs  # §19 candidate host surface
  - src/Host/AssetCoverageScanner.cs  # §19 candidate host surface
  - src/Host/SaveLoadHostSession.cs  # §19 candidate host surface
  - src/Host/SaveLoadUiFailureSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/mod_manifest_schema.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/load_shed_schedule_001.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --asset-coverage-report
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
