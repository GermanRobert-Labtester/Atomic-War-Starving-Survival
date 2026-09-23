# PLAN-BLACK-PROJECTS-TRUTH-205 — Sealed Records, Access Control & Revelation Gates

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-CULTURAL-ARCHIVE-TRUTH-169, PLAN-DOCUMENT-DISCOVERY-TRUTH-192, PLAN-ESPIONAGE-SYSTEM-TRUTH-161.
**Implementation scaffold:** [`PLAN-BLACK-PROJECTS-TRUTH-205_APPENDIX-A_SCAFFOLD.md`](PLAN-BLACK-PROJECTS-TRUTH-205_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-CULTURAL-ARCHIVE-TRUTH-169` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no vault model (Plan 169), no document discovery (Plan 192), no
field operations (Plan 161).

## 1. Outcome
`Narrative/BlackProjectsArchiveSystem.cs` (**367 lines**) is reachable and
unaddressed: records that are sealed — restricted by faction, clearance, or
consequence. Vaults (Plan 169) preserve; documents (Plan 192) are found and
read; the **sealed-record** class adds an access gate and a revelation rule,
neither stated.

| Deliverable | Detail |
|---|---|
| Seal model | sealed records with an access class (faction, clearance, key-holder) and a reason recorded |
| Access evaluation | access reads roles/standing from existing owners (Plan 141/29); no private permission list |
| Revelation gates | a sealed record opens on documented conditions (mission outcome Plan 161, relationship Plan 43, day/era) — never on a panel click alone |
| Leak consequences | unauthorized access routes to Plan 29/121 owners; the record's state reflects the leak |
| Save truth | seal and access states restore; a load never opens or re-seals silently |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/BlackProjectsArchiveSystem.cs` (367 lines; unaddressed — Wave 13/15 audit).
- Plan 169's vault is the storage layer; Plan 192's document model the read layer.
- Plan 161's operation outcomes can be revelation gates.
- Plan 141/29 own roles/standing the access rule reads.

## 3. Packages
- **BPT-205A** seal model + access class table.
- **BPT-205B** access evaluation tests per class.
- **BPT-205C** revelation gate fixtures (mission, relationship, era).
- **BPT-205D** leak routing to Plan 29/121 + state reflection test.
- **BPT-205E** save round-trip; no silent open/re-seal.

## 4. Acceptance & verification
- Access decisions match evaluated roles/standing; no private list.
- Each gate opens only on its documented condition; unauthorized read leaves a trace.
- Save/load preserves seal state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Permission duplication → access reads existing owners; a test asserts no local list.
Silent leak → leaks route and leave a visible record state.

---

## 6. Expanded census (2 files · 627 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BlackProjectsArchiveSystem.cs` | 367 | System | **yes** | 0 | 0 | 2 |
| `BlackProjectsCatalog.cs` | 260 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `BlackProjectsArchiveSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 5 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **4**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 2 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-ANCIENT-RUINS-VAULTS-84` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `BPT-205A` | no name match — resolve at claim time |
| `BPT-205B` | no name match — resolve at claim time |
| `BPT-205C` | no name match — resolve at claim time |
| `BPT-205D` | no name match — resolve at claim time |
| `BPT-205E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **3** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/BlackProjectsArchiveSaveStore.cs`, `src/Main.Plans152.cs`, `src/UI/BlackProjectsArchivePanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/BlackProjectsCatalogTests.cs`, `Ashfall.Core.Tests/Narrative/BlackProjectsArchiveTests.cs`, `Ashfall.Core.Tests/UI/BlackProjectsArchivePanelRouteTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archive_desk` |
| `black_market` |
| `black_projects_archive` |
| `grain_milling_archive` |
| `hydrogeology_archive` |
| `leatherwork_archive` |
| `route_infrastructure` |
| `technical_material_archive` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **14** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--black-flotilla-selftest` |
| `--deep-coast-route-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--real-main-journey-selftest` |

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

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/cultural_archive_tomes.json` |
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

Host files (`src/`) whose names share a domain token: **16**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/BlackMarketHostSession.cs` |
| `src/Host/BlackMarketSaveStore.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/CulturalArchiveSaveStore.cs` |
| `src/Host/GrainMillingArchiveSaveStore.cs` |
| `src/Host/HydroGeologyArchiveSaveStore.cs` |
| `src/Host/LeatherworkArchiveSaveStore.cs` |
| `src/Host/PrewarArchiveSaveStore.cs` |
| `src/Host/TechnicalMaterialArchiveSaveStore.cs` |
| `src/Main.BlackMarket.cs` |
| `src/UI/ArchiveDeskPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `archive_desk` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `grain_milling_archive` | no |
| `hydrogeology_archive` | no |
| `leatherwork_archive` | no |
| `technical_material_archive` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 1, GAMEPLAY_CONSUMED 2, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 7 (laddered 0) · RNG streams 3 · host files 16 · catalogs 10 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BLACK-PROJECTS-TRUTH-205
wave: 15
status: PROPOSED — foreman claim required
packages: BPT-205A, BPT-205B, BPT-205C, BPT-205D, BPT-205E
claim paths:
  - src/Host/ArchiveDeskHostSession.cs  # §19 candidate host surface
  - src/Host/BlackMarketHostSession.cs  # §19 candidate host surface
  - src/Host/BlackMarketSaveStore.cs  # §19 candidate host surface
  - src/Host/BlackProjectsArchiveSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/archive_categories.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/archive_inks.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --black-flotilla-selftest
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
