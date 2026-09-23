# PLAN-CRAFT-ARCHIVE-TRUTH-208 — Technical & Trade Knowledge Archives

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-CULTURAL-ARCHIVE-TRUTH-169, PLAN-RECIPE-REACHABILITY-TRUTH-125, PLAN-CODEX-SURFACE-TRUTH-110.
**Non-goals:** no cultural vault (Plan 169), no recipe reachability (Plan 125),
no codex rendering (Plan 110).

## 1. Outcome
Two archives are reachable and unaddressed: `TechnicalMaterialArchiveSystem.cs`
(**499 lines**) and `LeatherworkArchiveSystem.cs` (**447 lines**) — the
knowledge stores that gate craft recipes (a recipe is known only if a record
survived). Culture archives (Plan 169) and recipe reachability (Plan 125) exist;
the **knowledge-gating** role of these archives is unstated, so recipes are
either always known or gated by an invisible flag.

| Deliverable | Detail |
|---|---|
| Archive model | trade/technical records with a knowledge key each (recipe id, material class); records are the gate |
| Unlocking | reading/owning a record unlocks its knowledge for the holdfast through one hook; loss re-locks only if the record is the only copy |
| Recipe coupling | Plan 125's reachability must account for knowledge-gated recipes — a craftable recipe with a missing archive row is reported |
| Copy rule | copying a record (transcription) consumes time/materials; copies preserve knowledge independently |
| Save truth | archive contents and unlocked knowledge restore; no re-lock on load |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/TechnicalMaterialArchiveSystem.cs` (499 lines) and `Narrative/LeatherworkArchiveSystem.cs` (447 lines) — both unaddressed (Wave 16 audit).
- Plan 125's table is the consumer that must know about knowledge gates.
- Plan 169's vault is the storage layer; the boundary: knowledge vs cultural record.
- Plan 110 renders what is known; this plan defines what "known" means.

## 3. Packages
- **CAT-208A** archive model + knowledge key table.
- **CAT-208B** unlock hook + single-source-of-truth tests.
- **CAT-208C** Plan 125 integration (knowledge-gated recipe rows reported).
- **CAT-208D** transcribe/copy path + independence test.
- **CAT-208E** save round-trip; no re-lock on load.

## 4. Acceptance & verification
- A knowledge key unlocks exactly once per holdfast; a copy survives source loss.
- Recipe table reports gated rows; no recipe is craftable without a key or a documented exception.
- Save/load preserves archive and knowledge state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Hidden gating → the recipe report makes every gate visible.
Duplicate knowledge stores → one hook; the independence test asserts it.

---

## 6. Expanded census (3 files · 1,041 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `LeatherworkArchiveSystem.cs` | 447 | System | **yes** | 0 | 0 | 2 |
| `TanningLeatherworkCatalog.cs` | 95 | Catalog | — | 0 | 0 | 0 |
| `TechnicalMaterialArchiveSystem.cs` | 499 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `LeatherworkArchiveSystem.cs`, `TechnicalMaterialArchiveSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 3 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CAT-208A` | `LeatherworkArchiveSystem.cs`, `TechnicalMaterialArchiveSystem.cs` |
| `CAT-208B` | no name match — resolve at claim time |
| `CAT-208C` | no name match — resolve at claim time |
| `CAT-208D` | no name match — resolve at claim time |
| `CAT-208E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **4** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/LeatherworkArchiveSaveStore.cs`, `src/Host/TechnicalMaterialArchiveSaveStore.cs`, `src/Main.Plans158.cs`, `src/Main.Plans159.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Narrative/LeatherworkArchiveTests.cs`, `Ashfall.Core.Tests/Narrative/TechnicalMaterialArchiveTests.cs`, `Ashfall.Core.Tests/TanningLeatherworkCatalogTests.cs` |
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

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archive_desk` |
| `black_projects_archive` |
| `grain_milling_archive` |
| `hydrogeology_archive` |
| `leatherwork_archive` |
| `technical_material_archive` |

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

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnCraftCompleted` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnCraftStarted` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **8**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/cultural_archive_tomes.json` |
| `Assets/StreamingAssets/Data/narrative/bark_tanning_vat_logs.json` |
| `Assets/StreamingAssets/Data/narrative/brain_tanning_hide_reports.json` |
| `Assets/StreamingAssets/Data/narrative/chrome_alum_tanning_assays.json` |
| `Assets/StreamingAssets/Data/narrative/oak_bark_tanning_pit_logs.json` |
| `Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json` |

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

Host files (`src/`) whose names share a domain token: **11**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/CulturalArchiveSaveStore.cs` |
| `src/Host/GrainMillingArchiveSaveStore.cs` |
| `src/Host/HydroGeologyArchiveSaveStore.cs` |
| `src/Host/LeatherworkArchiveSaveStore.cs` |
| `src/Host/PrewarArchiveSaveStore.cs` |
| `src/Host/TechnicalMaterialArchiveSaveStore.cs` |
| `src/UI/ArchiveDeskPanel.cs` |
| `src/UI/BlackProjectsArchivePanel.cs` |
| `src/UI/MagneticDrumArchivePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **6**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `archive_desk` | no |
| `black_projects_archive` | no |
| `grain_milling_archive` | no |
| `hydrogeology_archive` | no |
| `leatherwork_archive` | no |
| `technical_material_archive` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **7**
(CODEX_ONLY 5, GAMEPLAY_CONSUMED 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `narrative/bark_tanning_vat_logs.json` | CODEX_ONLY |
| `narrative/brain_tanning_hide_reports.json` | CODEX_ONLY |
| `narrative/chrome_alum_tanning_assays.json` | CODEX_ONLY |
| `narrative/oak_bark_tanning_pit_logs.json` | CODEX_ONLY |
| `narrative/vinyl_record_archive.json` | CODEX_ONLY |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 6 (laddered 0) · RNG streams 0 · host files 12 · catalogs 15 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CRAFT-ARCHIVE-TRUTH-208
wave: 16
status: PROPOSED — foreman claim required
packages: CAT-208A, CAT-208B, CAT-208C, CAT-208D, CAT-208E
claim paths:
  - src/Host/ArchiveDeskHostSession.cs  # §19 candidate host surface
  - src/Host/BlackProjectsArchiveSaveStore.cs  # §19 candidate host surface
  - src/Host/CulturalArchiveSaveStore.cs  # §19 candidate host surface
  - src/Host/GrainMillingArchiveSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/archive_categories.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/archive_inks.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
