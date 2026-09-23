# PLAN-ENDGAME-EVALUATION-TRUTH-137 — One Ending Resolver, Documented Inputs, Cross-Run Scope

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MUSTER-COALITION-TRUTH-130, PLAN-TELEMETRY-PRIVACY-58, PLAN-SAVE-MIGRATION-CORRIDOR-87.
**Implementation scaffold:** [`PLAN-ENDGAME-EVALUATION-TRUTH-137_APPENDIX-A_SCAFFOLD.md`](PLAN-ENDGAME-EVALUATION-TRUTH-137_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-CAMPAIGN-EPILOGUE-TRUTH-259` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new ending content, no second epilogue system; this plan
reconciles the resolvers that already exist.

## 1. Outcome
`Endgame/` holds eleven types, including **two** resolving paths:
`UnifiedEndingResolver.cs` and `EpilogueMatrixRuntime.cs` (Plan 130 covers the
matrix's own inputs). Also present: `CampaignOutcomeEvaluator/Snapshot`,
`CampaignCompletionHistory`, `CrossRunProfileStore`, `EpilogueChronicleBuilder/Catalog`,
and `EpilogueContextFactory`. Without a stated boundary, an ending can be
resolved twice with different inputs — the exact "two authorities" defect the
repo forbids.

| Deliverable | Detail |
|---|---|
| Resolver authority | state which type decides the ending and which others feed it; the non-deciding path becomes a projection or is retired |
| Input table | every field an ending reads, its owner, and its fallback when missing (explicit unknown, no silent default) |
| Completion history | once-per-campaign record semantics; re-evaluation on load never re-writes history |
| Cross-run profile | what persists across campaigns, where it lives, and its privacy scope (local-only, Plan 58 rules) |
| Chronicle build | `EpilogueChronicleBuilder` output is deterministic from the snapshot; same save → same chronicle |

## 2. Evidence
- `Assets/Ashfall.Core/Endgame/` file list (verified), including both `UnifiedEndingResolver.cs` and `EpilogueMatrixRuntime.cs`.
- `EndgameHeadlessDemo.cs` provides a headless verification entry.
- Plan 130 documents which persisted facts each epilogue row reads — this plan consumes that table.
- Plan 58 governs what a cross-run profile may retain locally.

## 3. Packages
- **EET-137A** resolver authority decision + projection/retirement note for the loser path.
- **EET-137B** input table (field → owner → fallback) + missing-input tests.
- **EET-137C** completion history once-only tests (save/reload, replay).
- **EET-137D** cross-run profile scope + local-only privacy check.
- **EET-137E** chronicle determinism test (paired same-save builds).

## 4. Acceptance & verification
- One resolver decides; the other path cannot produce a different ending for the same snapshot (test).
- A missing input yields the documented unknown row, not a default ending.
- Reloading a completed campaign writes no new history row.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/` + the headless demo.

## 5. Risks
Resolver reconciliation breaking crafted endings → the input table is verified against current data before any retirement.
Cross-run store scope creep → fields are enumerated and local-only; no session ids, no machine identifiers.

---

## 6. Expanded census (9 files · 1,853 lines)

Scope: `Assets/Ashfall.Core/Endgame/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Demo 1 · Support 6 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampaignOutcomeEvaluator.cs` | 323 | Support | **yes** | 0 | 0 | 0 |
| `CampaignOutcomeSnapshot.cs` | 81 | Support | — | 0 | 0 | 0 |
| `EndgameHeadlessDemo.cs` | 102 | Demo | — | 0 | 0 | 3 |
| `EndgameSystem.cs` | 323 | System | — | 0 | 0 | 2 |
| `EpilogueChronicleBuilder.cs` | 126 | Support | — | 0 | 0 | 0 |
| `EpilogueChronicleCatalog.cs` | 90 | Catalog | — | 0 | 0 | 0 |
| `EpilogueContextFactory.cs` | 61 | Support | — | 0 | 0 | 0 |
| `EpilogueMatrixRuntime.cs` | 151 | Support | **yes** | 0 | 0 | 0 |
| `UnifiedEndingResolver.cs` | 596 | Support | **yes** | 1 | 0 | 2 |

**Totals:** 1 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `campaign_epilogues.json` | object[2 keys] |
| `endings.json` | object[2 keys] |
| `epilogue_chronicle.json` | object[2 keys] |
| `muster_epilogues.json` | object[2 keys] |
| `epilogue_personalization.json` | object[8 keys] |

**State surfaces:** `EndgameHeadlessDemo.cs`, `EndgameSystem.cs`, `UnifiedEndingResolver.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Endgame/` |
| Test references | 18 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
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

## 11. Tier-2: intra-domain reference graph

Computed across 10 domain files: **18 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `UnifiedEndingResolver.cs` | 596 | 0 | 0 |
| `CampaignCompletionHistory.cs` | 511 | 0 | 1 |
| `CampaignOutcomeEvaluator.cs` | 323 | 2 | 4 |
| `EndgameSystem.cs` | 323 | 3 | 0 |
| `EpilogueMatrixRuntime.cs` | 151 | 7 | 0 |
| `EpilogueChronicleBuilder.cs` | 126 | 1 | 0 |
| `EndgameHeadlessDemo.cs` | 102 | 0 | 3 |
| `EpilogueChronicleCatalog.cs` | 90 | 0 | 1 |
| `CampaignOutcomeSnapshot.cs` | 81 | 2 | 5 |
| `EpilogueContextFactory.cs` | 61 | 3 | 4 |

**Highest-coupling files (in×2 + out):**

- `EpilogueMatrixRuntime.cs` — in 7, out 0
- `EpilogueContextFactory.cs` — in 3, out 4
- `CampaignOutcomeSnapshot.cs` — in 2, out 5
- `CampaignOutcomeEvaluator.cs` — in 2, out 4
- `EndgameSystem.cs` — in 3, out 0
- `EndgameHeadlessDemo.cs` — in 0, out 3
- `EpilogueChronicleBuilder.cs` — in 1, out 0
- `CampaignCompletionHistory.cs` — in 0, out 1

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 10. Other plans referencing their names: **5**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ACHIEVEMENTS-COMPLETION-TRUTH-76` | 2 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 2 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-FAMILY-DYNASTY-43` | 1 |
| `PLAN-DEV-TOOLING-TRUTH-75` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `EET-137A` | `UnifiedEndingResolver.cs` |
| `EET-137B` | no name match — resolve at claim time |
| `EET-137C` | `CampaignCompletionHistory.cs` |
| `EET-137D` | no name match — resolve at claim time |
| `EET-137E` | `EpilogueChronicleBuilder.cs`, `EpilogueChronicleCatalog.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 10. Host files: **7** · Test files: **15** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 7 | `src/Host/CompletionHistoryStore.cs`, `src/Host/EndgameHostSession.cs`, `src/Host/ExpansionHostSession.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Main.Endgame.cs` |
| Tests (`Ashfall.Core.Tests/`) | 15 | `Ashfall.Core.Tests/Endgame/CampaignCompletionHistoryTests.cs`, `Ashfall.Core.Tests/Endgame/CampaignOutcomeEvaluatorTests.cs`, `Ashfall.Core.Tests/Endgame/CompletionHistorySummaryTests.cs`, `Ashfall.Core.Tests/Endgame/CrossRunProfileStoreTests.cs`, `Ashfall.Core.Tests/Endgame/EndgameSystemTests.cs` |
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
| `endgame` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--ice-road-tick-demo` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--ui-snapshot-regenerate` |
| `--ui-snapshot-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnOutcomeResolved` | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **5**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/epilogue_chronicle.json` |
| `Assets/StreamingAssets/Data/epilogue_personalization.json` |
| `Assets/StreamingAssets/Data/narrative/world_history_expansion.json` |
| `Assets/StreamingAssets/Data/world_history.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (43 files, 288 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Endgame` | 11 | 101 |

**Verdict:** 288 cases sit under matching regions — run those first (`Campaign`, `Endgame`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **27**
(7 of them panels/HUD).

| Host file |
|---|
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/CompletionHistorySelfTest.cs` |
| `src/Host/CompletionHistoryStore.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/EndgameHostSession.cs` |
| `src/Host/EndgameSaveStore.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Main.Campaign.cs` |
| `src/Main.CampaignOwners.cs` |
| `src/Main.CampaignServices.cs` |
| `src/Main.Endgame.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |
| `endgame` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(CODEX_ONLY 1, GAMEPLAY_CONSUMED 2).

| Catalog | Classification |
|---|---|
| `epilogue_chronicle.json` | GAMEPLAY_CONSUMED |
| `narrative/world_history_expansion.json` | CODEX_ONLY |
| `world_history.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 3 (laddered 0) · RNG streams 0 · host files 12 · catalogs 8 · test regions 2 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ENDGAME-EVALUATION-TRUTH-137
wave: 11
status: PROPOSED — foreman claim required
packages: EET-137A, EET-137B, EET-137C, EET-137D, EET-137E
claim paths:
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - src/Host/CompletionHistorySelfTest.cs  # §19 candidate host surface
  - src/Host/CompletionHistoryStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/epilogue_chronicle.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
