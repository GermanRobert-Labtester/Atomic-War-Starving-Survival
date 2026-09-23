# PLAN-INVESTIGATION-EVIDENCE-TRUTH-121 — Evidence Chain, Accusation Accuracy & Reckoning

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-JUSTICE-LAW-37, PLAN-CRIME-SYNDICATES-44, PLAN-SILENT-FAILURE-35.
**Non-goals:** no second verdict system (Plan 37 owns trial/verdict), no crime
org rewrite (Plan 44), no new evidence catalog beyond what exists.

## 1. Outcome
`Verdict/` is a full subsystem: `EvidenceLedger`, `VerdictEvidenceChain`,
`VerdictAccusationSystem` (host-unreachable), `MachineLogSystem`,
`ReckoningSystem`, `VerdictNpcSystem`, `VerdictRadioSystem`, `VerdictSave`,
`VerdictEndingEvaluator`. Plan 37 owns the verdict/trial outcome. What is
unstated is the **evidence contract**: what counts as evidence, how a chain is
built and broken, and how an accusation's accuracy follows from the chain
rather than from a hidden roll.

| Deliverable | Detail |
|---|---|
| Evidence classes | each class with its source system, persistence owner, and admissible weight band |
| Chain rules | how pieces link (who, when, where) and what breaks a link (loss, tamper, death of a witness) |
| Accusation accuracy | accuracy derives from the chain with a seeded component only where the design says so; a false accusation is possible and traceable to a missing link |
| Machine log | `MachineLogSystem` entries are a documented evidence class with a retention rule |
| Verdict hand-off | the chain's state is the input Plan 37 consumes; no verdict logic in this plan |

## 2. Evidence
- `Assets/Ashfall.Core/Verdict/`: `EvidenceLedger.cs`, `VerdictEvidenceChain.cs`, `VerdictAccusationSystem.cs`, `MachineLogSystem.cs`, `ReckoningSystem.cs`, `VerdictSave.cs` (file list verified; types re-checked per package).
- Plan 1 Appendix A/L: `VerdictAccusationSystem` host-unreachable; sized for a bounded seal.
- Plan 37 owns outcomes; Plan 44 owns organizations that produce/obstruct evidence.
- `VerdictSave.cs` already exists as the persistence seam.

## 3. Packages
- **IET-121A** evidence class table with owners and weight bands.
- **IET-121B** chain construction/break rules + focused tests per break case.
- **IET-121C** accusation accuracy derivation + false-accusation traceability test.
- **IET-121D** machine-log retention rule + class registration.
- **IET-121E** hand-off contract test to Plan 37's verdict input.

## 4. Acceptance & verification
- Breaking a chain link changes the accusation outcome in the documented direction; no hidden re-roll.
- Same seed + same chain → same accuracy.
- Machine-log retention enforced; expired entries do not silently re-enter a chain.
- `bash scripts/run_test.sh` on the verdict region.

## 5. Risks
Verdict overlap → this plan ends at the chain; Plan 37 remains the outcome owner.
Evidence inflation → classes are closed per data; new classes need a row and a weight band.

---

## 6. Expanded census (13 files · 1,932 lines)

Scope: `Assets/Ashfall.Core/Verdict/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Loader 2 · Save 1 · Support 6 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `EvidenceLedger.cs` | 112 | Support | **yes** | 0 | 0 | 2 |
| `ReckoningSystem.cs` | 276 | System | — | 0 | 0 | 2 |
| `VerdictAccusationSystem.cs` | 263 | System | **yes** | 0 | 0 | 2 |
| `VerdictCatalogLoader.cs` | 235 | Loader | — | 0 | 0 | 0 |
| `VerdictCensusBroadcast.cs` | 94 | Support | — | 0 | 0 | 0 |
| `VerdictEndingEvaluator.cs` | 72 | Support | — | 0 | 0 | 0 |
| `VerdictEvidenceChain.cs` | 63 | Support | **yes** | 0 | 0 | 0 |
| `VerdictNpcSystem.cs` | 156 | System | — | 0 | 0 | 2 |
| `VerdictQuestCatalogLoader.cs` | 64 | Loader | — | 0 | 0 | 0 |
| `VerdictQuestMigration.cs` | 137 | Support | — | 0 | 0 | 0 |
| `VerdictRadioSystem.cs` | 115 | System | — | 0 | 0 | 2 |
| `VerdictReadout.cs` | 72 | Support | — | 0 | 0 | 0 |
| `VerdictSave.cs` | 273 | Save | — | 0 | 0 | 16 |

**Totals:** 0 banned refs · 0 empty catches · 6 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `verdict_data.json` | object[9 keys] |
| `verdict_items.json` | array[15] |
| `verdict_locations.json` | object[2 keys] |
| `verdict_npcs.json` | array[18] |
| `verdict_questlines.json` | object[2 keys] |
| `verdict_radio.json` | object[2 keys] |

**State surfaces:** `EvidenceLedger.cs`, `ReckoningSystem.cs`, `VerdictAccusationSystem.cs`, `VerdictNpcSystem.cs`, `VerdictRadioSystem.cs`, `VerdictSave.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Verdict/` |
| Test references | 52 name references across the test tree |
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

## 11. Tier-2: intra-domain reference graph

Computed across 14 domain files: **31 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `ReckoningSystem.cs` | 276 | 11 | 1 |
| `VerdictSave.cs` | 273 | 2 | 13 |
| `VerdictAccusationSystem.cs` | 263 | 2 | 4 |
| `VerdictCatalogLoader.cs` | 235 | 2 | 1 |
| `MachineLogSystem.cs` | 189 | 4 | 0 |
| `VerdictNpcSystem.cs` | 156 | 2 | 0 |
| `VerdictQuestMigration.cs` | 137 | 1 | 1 |
| `VerdictRadioSystem.cs` | 115 | 2 | 3 |
| `EvidenceLedger.cs` | 112 | 4 | 0 |
| `VerdictCensusBroadcast.cs` | 94 | 0 | 0 |

**Highest-coupling files (in×2 + out):**

- `ReckoningSystem.cs` — in 11, out 1
- `VerdictSave.cs` — in 2, out 13
- `EvidenceLedger.cs` — in 4, out 0
- `MachineLogSystem.cs` — in 4, out 0
- `VerdictAccusationSystem.cs` — in 2, out 4
- `VerdictRadioSystem.cs` — in 2, out 3
- `VerdictEvidenceChain.cs` — in 1, out 4
- `VerdictCatalogLoader.cs` — in 2, out 1

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 14. Other plans referencing their names: **5**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-JUSTICE-LAW-37` | 14 |
| `EVIDENCE` | 6 |
| `PLAN-ANCIENT-RUINS-VAULTS-84` | 2 |
| `PLAN-REFERENCE-INTEGRITY-34` | 1 |
| `PLAN-MUSTER-COALITION-TRUTH-130` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `IET-121A` | `EvidenceLedger.cs`, `VerdictEvidenceChain.cs` |
| `IET-121B` | `VerdictEvidenceChain.cs` |
| `IET-121C` | `VerdictAccusationSystem.cs` |
| `IET-121D` | `MachineLogSystem.cs` |
| `IET-121E` | `VerdictAccusationSystem.cs`, `VerdictCatalogLoader.cs`, `VerdictCensusBroadcast.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 14. Host files: **11** · Test files: **24** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 11 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/InventoryHostSession.cs`, `src/Host/VerdictHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 24 | `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs`, `Ashfall.Core.Tests/ClockPolicyTests.cs`, `Ashfall.Core.Tests/Endgame/Plan19EndingContinuityTests.cs`, `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs`, `Ashfall.Core.Tests/ExpansionAggregateCompletenessTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dose_ledger` |
| `expansion_quest` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `verdict` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--census-selftest` |
| `--dose-ledger-selftest` |
| `--ledger-debt-selftest` |
| `--personal-quest-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |
| `--verdict-selftest` |
| `--verdict-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **13**.

| Event | First declaration |
|---|---|
| `OnBroadcast` | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` |
| `OnCensusUpdated` | `Assets/Ashfall.Core/CensusClaimSystem.cs` |
| `OnCulturalBroadcast` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |
| `OnQuestStarted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnReckoningCall` | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json` |
| `Assets/StreamingAssets/Data/narrative/radio_broadcast_rundowns.json` |
| `Assets/StreamingAssets/Data/narrative/radio_mysteries_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scriptbook.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scripts_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_3.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (54 files, 404 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Radio` | 47 | 354 |
| `Verdict` | 7 | 50 |

**Verdict:** 404 cases sit under matching regions — run those first (`Radio`, `Verdict`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **38**
(15 of them panels/HUD).

| Host file |
|---|
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/PersonalQuestHostSession.cs` |
| `src/Host/PersonalQuestSaveStore.cs` |
| `src/Host/PersonalQuestSelfTest.cs` |
| `src/Host/RadioCatalogSelfTest.cs` |
| `src/Host/RadioHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **6**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `dose_ledger` | yes |
| `expansion_quest` | no |
| `radio` | no |
| `radio_program_production` | no |
| `radio_station` | no |
| `verdict` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `radio` |
| `wildlife_migration` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **25**
(CODEX_ONLY 8, GAMEPLAY_CONSUMED 13, OPTIONAL 1, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `ledger_debt_templates.json` | UNRESOLVED |
| `moral_choice_quest_stubs.json` | OPTIONAL |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |
| `narrative/quest_narrative_documents.json` | CODEX_ONLY |
| `narrative/radio_broadcast_rundowns.json` | CODEX_ONLY |
| `narrative/radio_mysteries_expansion.json` | CODEX_ONLY |
| `narrative/radio_scriptbook.json` | CODEX_ONLY |
| `narrative/radio_scripts_expansion.json` | CODEX_ONLY |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 6 (laddered 1) · RNG streams 2 · host files 14 · catalogs 22 · test regions 2 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INVESTIGATION-EVIDENCE-TRUTH-121
wave: 10
status: PROPOSED — foreman claim required
packages: IET-121A, IET-121B, IET-121C, IET-121D, IET-121E
claim paths:
  - src/Host/DoseLedgerHostSession.cs  # §19 candidate host surface
  - src/Host/DoseLedgerSaveStore.cs  # §19 candidate host surface
  - src/Host/DynamicQuestSaveStore.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/dynamic_quest_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_radio_corpus.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Radio/
  - godot --headless --path . -- --census-selftest
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
