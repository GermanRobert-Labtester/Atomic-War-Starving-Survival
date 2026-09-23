# PLAN-CAMPAIGN-EPILOGUE-TRUTH-259 — The Epilogue Builder: Inputs, Assembly & Readback

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ENDGAME-EVALUATION-TRUTH-137, PLAN-MUSTER-COALITION-TRUTH-130, PLAN-HOST-EVENT-ARCHIVE-91.
**Non-goals:** no ending resolver (Plan 137), no epilogue matrix rows (Plan 130),
no event archive (Plan 91).

## 1. Outcome
`Campaign/CampaignEpilogueEngine.cs` (**221 lines**) is reachable and
unaddressed: assembling the epilogue text from campaign facts. Plan 137 owns
the resolver, Plan 130 the matrix rows — the **assembly/readback** step (which
facts become which paragraphs, and whether a saved epilogue can be re-read) is
unowned.

| Deliverable | Detail |
|---|---|
| Input contract | the exact fact set the builder reads, with owner per fact (Plans 130/137/91) |
| Assembly rules | paragraph selection is deterministic from inputs; missing inputs produce a documented gap line, never a silent omission |
| Readback | a produced epilogue is stored and re-readable after the campaign; re-reading never re-assembles differently |
| Provenance | paragraphs trace to authored catalog rows; no generated prose |
| Save truth | produced epilogue and its inputs restore; no re-assembly on load |

## 2. Evidence
- `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs` (221 lines; unaddressed — Wave 18 audit).
- Plan 137 owns resolution; the builder consumes it.
- Plan 130 documents matrix inputs; Plan 91 can supply fact history.
- Plan 110's surfaces can render the stored epilogue.

## 3. Packages
- **CET-259A** input contract + owner table.
- **CET-259B** deterministic assembly test (paired builds).
- **CET-259C** missing-input gap-line fixture.
- **CET-259D** readback/storage test.
- **CET-259E** provenance check (authored rows only) + save round-trip.

## 4. Acceptance & verification
- Same inputs → identical epilogue; missing inputs show the gap line.
- Re-reading after reload yields the stored text; provenance resolves to authored rows.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/`.

## 5. Risks
Non-deterministic assembly → paired-build test is the guard.
Generated prose → provenance check forbids it.

---

## 6. Expanded census (2 files · 295 lines)

Scope: `Assets/Ashfall.Core/Campaign/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampaignEpilogueCatalog.cs` | 74 | Catalog | — | 0 | 0 | 0 |
| `CampaignEpilogueEngine.cs` | 221 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `campaign_epilogues.json` | object[2 keys] |
| `epilogue_chronicle.json` | object[2 keys] |
| `muster_epilogues.json` | object[2 keys] |
| `epilogue_personalization.json` | object[8 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Campaign/` |
| Test references | 6 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-CAMPAIGN-FAMILY-TRUTH-272` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CET-259A` | no name match — resolve at claim time |
| `CET-259B` | no name match — resolve at claim time |
| `CET-259C` | no name match — resolve at claim time |
| `CET-259D` | no name match — resolve at claim time |
| `CET-259E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 6. Host files: **1** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.Plans62_65.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs`, `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs`, `Ashfall.Core.Tests/Campaign/Plans62_65_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/Endgame/Plan145UnifiedEndingIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `muster` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **9** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--muster-selftest` |
| `--muster-uitest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/epilogue_chronicle.json` |
| `Assets/StreamingAssets/Data/epilogue_personalization.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (32 files, 187 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |

**Verdict:** 187 cases sit under matching regions — run those first (`Campaign`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Main.Campaign.cs` |
| `src/Main.CampaignOwners.cs` |
| `src/Main.CampaignServices.cs` |
| `src/Main.UiTests.RealCampaignJourney.cs` |
| `src/UI/EpiloguePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `epilogue_chronicle.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 7 · catalogs 4 · test regions 1 · flags 9

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CAMPAIGN-EPILOGUE-TRUTH-259
wave: 18
status: PROPOSED — foreman claim required
packages: CET-259A, CET-259B, CET-259C, CET-259D, CET-259E
claim paths:
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - src/Main.Campaign.cs  # §19 candidate host surface
  - src/Main.CampaignOwners.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/epilogue_chronicle.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
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
