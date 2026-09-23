# PLAN-LORE-ARCHIVE-TRUTH-238 — Oral Lore Performance & the Archive Desk

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-CULTURAL-ARCHIVE-TRUTH-169, PLAN-CODEX-SURFACE-TRUTH-110, PLAN-RELATIONSHIP (Plan 43), PLAN-CEREMONY-SYSTEM-TRUTH-223.
**Non-goals:** no vault storage (Plan 169), no codex rendering (Plan 110), no
ceremony runtime (Plan 223).

## 1. Outcome
Two reachable systems are unaddressed: `OralLorePerformanceSystem.cs` (**241**)
and `ArchiveDeskSystem.cs` (**205**). Together they cover knowledge that lives
**in people** rather than on shelves: performed stories, and the desk where
residents consult or deposit records. Plan 169 owns the vault; the human side
is unowned, so oral knowledge is either lost silently or immortal.

| Deliverable | Detail |
|---|---|
| Performance model | lore held by a survivor (knowledge keys from Plan 208); a performance shares it with listeners through Plan 110's records |
| Transmission fidelity | a performance transfers the key intact or with a documented degradation (secondhand detail loss), seeded and bounded |
| Loss rule | a holder's death without transmission loses the key unless another holder exists; the loss is visible |
| Archive desk | consultations/deposits move between the vault (Plan 169) and individuals with a record of who has what |
| Save truth | holder/known-to state restores; no silent key creation or loss on load |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/OralLorePerformanceSystem.cs` (241) and `ArchiveDeskSystem.cs` (205) — both unaddressed (Wave 17 audit).
- Plan 208's knowledge-key model is the shared currency; Plan 169 the physical store.
- Plan 223 ceremonies are a performance venue — boundary stated.
- Plan 43 relations influence listening/transmission (boundary noted).

## 3. Packages
- **LAT-238A** performance model + transmission rules.
- **LAT-238B** fidelity/degradation fixtures (seeded).
- **LAT-238C** loss-on-death rule + visibility test.
- **LAT-238D** archive desk consultation/deposit records + conservation.
- **LAT-238E** save round-trip; no silent key change.

## 4. Acceptance & verification
- Transmission moves keys per the table; death without transmission loses them visibly.
- Desk movements balance against vault contents; save/load preserves holder state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Immortal knowledge → loss rule is a fixture.
Silent key duplication → conservation check across vault/people sets.

---

## 6. Expanded census (2 files · 481 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `OralLoreCatalog.cs` | 240 | Catalog | — | 0 | 0 | 0 |
| `OralLorePerformanceSystem.cs` | 241 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `OralLorePerformanceSystem.cs`.

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

Domain method: plan-body `.cs` enumeration.
Domain files: 3. Other plans referencing them: **3**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |
| `PLAN-CORE-ROOT-FAMILY-TRUTH-262` | 1 |

**Reading:** incoming edges are coordination risk.

---

## 13. Authority binding map

Symbols used: 3. Host files: **5** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/ArchiveDeskHostSession.cs`, `src/Host/OralLoreHostSession.cs`, `src/Host/OralLoreSaveStore.cs`, `src/Main.Plans155.cs`, `src/Main.ShelterBatch3.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`, `Ashfall.Core.Tests/Narrative/OralLorePlan155Tests.cs`, `Ashfall.Core.Tests/NewCatalogLoaderTests.cs`, `Ashfall.Core.Tests/OralLoreCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `OralLorePerformanceSystem` | `OralLoreCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archive_desk` |
| `black_projects_archive` |
| `grain_milling_archive` |
| `hydrogeology_archive` |
| `leatherwork_archive` |
| `oral_lore` |
| `technical_material_archive` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--performance-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/cultural_archive_tomes.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/lore_archives.json` |
| `Assets/StreamingAssets/Data/narrative/deep_lore_texts.json` |
| `Assets/StreamingAssets/Data/narrative/oral_lore_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/oral_lore_codex.json` |
| `Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json` |
| `Assets/StreamingAssets/Data/needs_performance.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (10 files, 47 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Performance` | 10 | 47 |

**Verdict:** 47 cases sit under matching regions — run those first (`Performance`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **14**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/CulturalArchiveSaveStore.cs` |
| `src/Host/GrainMillingArchiveSaveStore.cs` |
| `src/Host/HydroGeologyArchiveSaveStore.cs` |
| `src/Host/LeatherworkArchiveSaveStore.cs` |
| `src/Host/OralLoreHostSession.cs` |
| `src/Host/OralLoreSaveStore.cs` |
| `src/Host/PerformanceSelfTest.cs` |
| `src/Host/PrewarArchiveSaveStore.cs` |
| `src/Host/TechnicalMaterialArchiveSaveStore.cs` |
| `src/UI/ArchiveDeskPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `archive_desk` | no |
| `black_projects_archive` | no |
| `grain_milling_archive` | no |
| `hydrogeology_archive` | no |
| `leatherwork_archive` | no |
| `oral_lore` | no |
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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **10**
(CODEX_ONLY 4, GAMEPLAY_CONSUMED 4, OPTIONAL 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `lore_archives.json` | GAMEPLAY_CONSUMED |
| `narrative/deep_lore_texts.json` | CODEX_ONLY |
| `narrative/oral_lore_batch_2.json` | CODEX_ONLY |
| `narrative/oral_lore_codex.json` | CODEX_ONLY |
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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 7 (laddered 0) · RNG streams 0 · host files 13 · catalogs 22 · test regions 1 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-LORE-ARCHIVE-TRUTH-238
wave: 17
status: PROPOSED — foreman claim required
packages: LAT-238A, LAT-238B, LAT-238C, LAT-238D, LAT-238E
claim paths:
  - src/Host/ArchiveDeskHostSession.cs  # §19 candidate host surface
  - src/Host/BlackProjectsArchiveSaveStore.cs  # §19 candidate host surface
  - src/Host/CulturalArchiveSaveStore.cs  # §19 candidate host surface
  - src/Host/GrainMillingArchiveSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/archive_categories.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/archive_inks.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Performance/
  - godot --headless --path . -- --performance-selftest
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
