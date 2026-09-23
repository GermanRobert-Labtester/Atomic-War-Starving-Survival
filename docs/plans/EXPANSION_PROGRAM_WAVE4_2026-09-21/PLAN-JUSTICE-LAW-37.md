# PLAN-JUSTICE-LAW-37 — Law Codes, Trials, Sentencing & Bounty Pursuit

**Wave:** 4 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-WARLORDS-DIPLOMACY-29, PLAN-EVENT-WIRING-21,
PLAN-REFERENCE-INTEGRITY-34.
**Expanded appendix:** [`PLAN-JUSTICE-LAW-37_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-JUSTICE-LAW-37_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's justice & law
systems (0 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no real legal systems, no modern courtroom procedures, no
graphic punishment detail.

---

## 1. Outcome

The Verdict corpus is the game's largest unfinished justice asset:
`EvidenceLedger`, `VerdictEvidenceChain`, `VerdictAccusationSystem`,
`VerdictNpcSystem`, `VerdictQuestline`s, `VerdictRadioSystem`,
`VerdictEndingEvaluator`, `ReckoningSystem`, `MachineLogSystem`, plus
`wasteland_laws.json`, `bounty_board.json`, `verdict_*` catalogs (7), and the
live `CrossingArbitrationSystem`, `FactionBountySystem` and contraband systems.
Today it reads as an investigation flavour set; this plan makes it a **working
legal layer** across settlements and factions.

Player loop: **witness/receive an accusation → gather evidence → bring it to a
forum → argue → sentence → live with the verdict or pursue the wanted**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Law codes | `wasteland_laws.json` per settlement/faction | read the local law | crimes/penalties known before acting |
| Evidence | `EvidenceLedger`, `VerdictEvidenceChain` | collect, verify, tamper | case strength, chain integrity |
| Accusation | `VerdictAccusationSystem` | accuse, defend, stay silent | standing, risk of countersuit |
| Forum | `CrossingArbitrationSystem`, court/inquest events | argue the case | ruling, precedent |
| Sentence | `ReckoningSystem` | propose/accept punishment | fine, labour, exile, amnesty |
| Pursuit | `FactionBountySystem`, `bounty_board.json` | hunt or hide | bounty, notoriety |
| Record | `MachineLogSystem`, `VerdictCensusBroadcast` | publish the verdict | faction perception, endings |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core files | `Verdict/` (15 files incl. `EvidenceLedger`, `VerdictAccusationSystem`, `VerdictNpcSystem`, `ReckoningSystem`, `VerdictEndingEvaluator`, `VerdictSave`) |
| Data | `wasteland_laws.json`, `verdict_data.json`, `verdict_items.json`, `verdict_locations.json`, `verdict_npcs.json`, `verdict_questlines.json`, `verdict_radio.json`, `bounty_board.json` |
| Sealed prior | Plan 113 (23 questlines), Plan 127 (25 corruption lines / 12 history layers), Plan 82 investigation sites, `VerdictQuestOwnershipTests` |
| Related live | `CrossingArbitrationSystem`, `FactionBountySystem`, contraband matrices, moral-choice flags |
| Save | `VerdictSave` exists; must ride the canonical registry (PLAN-SAVE-GOVERNANCE-12) |

---

## 3. Packages

### LJ-37A — Law codes and jurisdiction
- `wasteland_laws.json` maps settlement/faction → prohibited acts, severity
  bands, penalties, and jurisdiction boundaries (which forum hears what).
  The panel shows the law where the player stands.
- **Acceptance:** every settlement has a code; jurisdiction conflicts resolve
  by one priority rule; no invisible criminality.
- **Verify:** `--data-integrity-selftest` + focused verdict tests.

### LJ-37B — Evidence and accusation
- `EvidenceLedger` + `VerdictEvidenceChain` record evidence with provenance;
  tampering is possible and detectable; `VerdictAccusationSystem` opens a case
  with a visible strength band.
- **Acceptance:** evidence items resolve to `verdict_items.json`; cases are
  deterministic; a weak case can be dismissed.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/`.

### LJ-37C — Trial and arbitration
- One forum protocol shared by court cases and crossing arbitration: opening,
  evidence presentation, argument choices, ruling; NPC judges/arbitrators come
  from `VerdictNpcSystem`.
- **Acceptance:** rulings apply standing/morale/faction deltas through
  canonical owners; appeal path exists; no parallel legal state.
- **Verify:** `--communique-board-selftest` + verdict suite.

### LJ-37D — Sentencing and rehabilitation
- `ReckoningSystem` sentences: fine (canonical funds), labour (duty roster),
  exile (map/visitor owner), amnesty (standing). Rehabilitation exists as a
  path back to standing for NPCs and the player.
- **Acceptance:** sentences are time-bounded and reversible; labour uses the
  duty owner; exile keeps survivors in the roster or removes them with a
  recoverable path.
- **Verify:** duty/faction/visitor focused suites.

### LJ-37E — Bounty pursuit and extradition
- `bounty_board.json` contracts become player-acceptable jobs with pursuit,
  capture/kill choice (violence routed to combat owner), and extradition
  between settlements with jurisdiction rules.
- **Acceptance:** contracts resolve deterministically; no duplicate bounty
  ledger (`FactionBountySystem` remains canonical); notoriety affects access.
- **Verify:** bounty + patrol focused suites.

### LJ-37F — Content volumes
- +12 law codes, +20 cases, +15 NPC figures, +10 sentences, +8 bounty
  contracts; fictional and original; every row consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Justice becomes a tax on play | punishments bounded; amnesty; the player can avoid, bribe, or argue |
| Verdict corpus already partly consumed | extend existing owners; do not fork `VerdictSave` or evidence |
| Content reads as procedural drama | authored, restrained prose per tone rules |
| Punishment dead-ends a run | every sentence has an exit (serve, pay, escape, appeal) |

## 5. Verification

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/
bash scripts/run_test.sh Ashfall.Core.Tests/Factions/
godot --headless --path . -- --communique-board-selftest
godot --headless --path . -- --data-integrity-selftest
```

---

## 6. Expanded census (12 files · 1,989 lines)

Scope: `Assets/Ashfall.Core/Verdict/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Loader 2 · Save 1 · Support 5 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `JusticeSystem.cs` | 445 | System | **yes** | 0 | 0 | 2 |
| `VerdictAccusationSystem.cs` | 263 | System | **yes** | 0 | 0 | 2 |
| `VerdictCatalogLoader.cs` | 235 | Loader | — | 0 | 0 | 0 |
| `VerdictCensusBroadcast.cs` | 94 | Support | — | 0 | 0 | 0 |
| `VerdictEndingEvaluator.cs` | 72 | Support | — | 0 | 0 | 0 |
| `VerdictEvidenceChain.cs` | 63 | Support | — | 0 | 0 | 0 |
| `VerdictNpcSystem.cs` | 156 | System | — | 0 | 0 | 2 |
| `VerdictQuestCatalogLoader.cs` | 64 | Loader | — | 0 | 0 | 0 |
| `VerdictQuestMigration.cs` | 137 | Support | — | 0 | 0 | 0 |
| `VerdictRadioSystem.cs` | 115 | System | — | 0 | 0 | 2 |
| `VerdictReadout.cs` | 72 | Support | — | 0 | 0 | 0 |
| `VerdictSave.cs` | 273 | Save | — | 0 | 0 | 16 |

**Totals:** 0 banned refs · 0 empty catches · 5 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `verdict_data.json` | object[9 keys] |
| `verdict_items.json` | array[15] |
| `verdict_locations.json` | object[2 keys] |
| `verdict_npcs.json` | array[18] |
| `verdict_questlines.json` | object[2 keys] |
| `verdict_radio.json` | object[2 keys] |

**State surfaces:** `JusticeSystem.cs`, `VerdictAccusationSystem.cs`, `VerdictNpcSystem.cs`, `VerdictRadioSystem.cs`, `VerdictSave.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Verdict/` |
| Test references | 43 name references across the test tree |
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

Domain files: 14. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-INVESTIGATION-EVIDENCE-TRUTH-121` | 13 |
| `EVIDENCE` | 5 |
| `PLAN-REFERENCE-INTEGRITY-34` | 1 |
| `PLAN-ANCIENT-RUINS-VAULTS-84` | 1 |
| `PLAN-MUSTER-COALITION-TRUTH-130` | 1 |
| `PLAN-JUSTICE-SYSTEM-TRUTH-222` | 1 |
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `LJ-37A` | no name match — resolve at claim time |
| `LJ-37B` | `EvidenceLedger.cs`, `VerdictAccusationSystem.cs`, `VerdictEvidenceChain.cs` |
| `LJ-37C` | no name match — resolve at claim time |
| `LJ-37D` | no name match — resolve at claim time |
| `LJ-37E` | no name match — resolve at claim time |
| `LJ-37F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 13; intra-domain edges: **10**; isolated files:
**5**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `VerdictAccusationSystem` | `VerdictEvidenceChain` |
| `VerdictCatalogLoader` | `EvidenceLedger` |
| `VerdictEvidenceChain` | `EvidenceLedger` |
| `VerdictQuestMigration` | `VerdictSave` |
| `VerdictRadioSystem` | `VerdictCatalogLoader` |
| `VerdictSave` | `EvidenceLedger` |
| `VerdictSave` | `VerdictAccusationSystem` |
| `VerdictSave` | `VerdictNpcSystem` |
| `VerdictSave` | `VerdictQuestMigration` |
| `VerdictSave` | `VerdictRadioSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `EvidenceLedger` | 3 |
| `VerdictAccusationSystem` | 1 |
| `VerdictCatalogLoader` | 1 |
| `VerdictEvidenceChain` | 1 |
| `VerdictNpcSystem` | 1 |
| `VerdictQuestMigration` | 1 |
| `VerdictRadioSystem` | 1 |
| `VerdictSave` | 1 |
| `JusticeSystem` | 0 |
| `VerdictCensusBroadcast` | 0 |

**Class split:** hub 6 · sink 2 · source 0 · isolated 5.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 13. Host files: **11** · Test files: **25** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 11 | `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/InventoryHostSession.cs`, `src/Host/VerdictHostSession.cs`, `src/Host/VerdictSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 25 | `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs`, `Ashfall.Core.Tests/ClockPolicyTests.cs`, `Ashfall.Core.Tests/Endgame/Plan19EndingContinuityTests.cs`, `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs`, `Ashfall.Core.Tests/ExpansionAggregateCompletenessTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dose_ledger` |
| `expansion_quest` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `verdict` |
| `wasteland_justice` |

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

Events whose name shares a domain token: **12**.

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
| `OnVerdictResolved` | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` |

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

Host files (`src/`) whose names share a domain token: **40**
(16 of them panels/HUD).

| Host file |
|---|
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/JusticeSaveStore.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/PersonalQuestHostSession.cs` |
| `src/Host/PersonalQuestSaveStore.cs` |
| `src/Host/PersonalQuestSelfTest.cs` |
| `src/Host/RadioCatalogSelfTest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `dose_ledger` | yes |
| `expansion_quest` | no |
| `radio` | no |
| `radio_program_production` | no |
| `radio_station` | no |
| `verdict` | no |
| `wasteland_justice` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **24**
(CODEX_ONLY 8, GAMEPLAY_CONSUMED 13, OPTIONAL 1, UNRESOLVED 2).

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

**Verdict:** 2 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 7 (laddered 1) · RNG streams 2 · host files 14 · catalogs 22 · test regions 2 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-JUSTICE-LAW-37
wave: —
status: PROPOSED — foreman claim required
packages: LJ-37A, LJ-37B, LJ-37C, LJ-37D, LJ-37E, LJ-37F
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
  - coordinate: 7 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
