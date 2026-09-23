# PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140 — Contracts-First Seam: Consumers Before Implementations

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-AUTONOMOUS-MACHINES-79, PLAN-INDUSTRY-AUTOMATION-45, PLAN-CORE-ONLY-REGISTRY-11.
**Implementation scaffold:** [`PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140_APPENDIX-A_SCAFFOLD.md`](PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-PORT-CONTRACT-TRUTH-157` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no machine implementation in this plan; a contract without a
named consumer is not an invitation to build one.

## 1. Outcome
`AdvancedMachinery/` contains exactly one file — `AdvancedMachineContracts.cs` —
with no plan coverage and no stated consumers. A contracts-only assembly is a
legitimate pattern (define the seam before the implementation), but it is also
how speculative architecture accumulates: unused contracts that later attract
half-built systems. This plan makes the seam **accountable**: every contract
type lists its intended realizer and its intended consumer, or is marked for
retirement.

| Deliverable | Detail |
|---|---|
| Contract inventory | every public type in the file with its members and the doc contract it states |
| Consumer map | intended realizer (which system will implement it) and intended consumer (who calls it) — each named, with a plan id |
| Realizer proof | at least one thin test double implements the contract, proving it is implementable and sufficient |
| Retirement rule | a contract with no named consumer after the review window is either retired or explicitly frozen with a reason |
| No parallel system | the contracts do not duplicate types already owned elsewhere; any overlap is reported for owner resolution |

## 2. Evidence
- `Assets/Ashfall.Core/AdvancedMachinery/AdvancedMachineContracts.cs` (the entire directory; zero plan mentions before this plan).
- Plan 79 owns autonomous machines; Plan 45 owns industry automation — the likely realizers.
- Plan 11's registry rules apply: a Core type needs a named live consumer to be considered reachable.
- Plan 1 Appendix A/B: if `AdvancedMachineContracts` types are host-unreachable, the seal package for the realizer consumes this map.

## 3. Packages
- **AMC-140A** contract inventory + member table.
- **AMC-140B** consumer map with plan ids (realizer + caller).
- **AMC-140C** test double implementing the contract + a fixture call path.
- **AMC-140D** overlap report against existing owners (no silent duplicates).
- **AMC-140E** retirement/freeze decisions for unconsumed contracts.

## 4. Acceptance & verification
- Every contract type has a consumer row or a retirement decision — no blanks.
- The test double compiles and passes a smoke call; the contract is sufficient for the stated use.
- No duplicated type signature exists beside an existing owner (report proves it).
- `bash scripts/run_test.sh Ashfall.Core.Tests/AdvancedMachinery/` (create if absent).

## 5. Risks
Speculative retention → the retirement rule has a window and an owner decision.
Premature implementation → explicitly out of scope; the realizer plan owns the build.

---

## 6. Expanded census (1 files · 101 lines)

Scope: `Assets/Ashfall.Core/AdvancedMachinery/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AdvancedMachineContracts.cs` | 101 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/AdvancedMachinery/` (create if absent) |
| Test references | 0 name references across the test tree |
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

Domain method: plan-body artifact list.
Governed artifacts: 3. Other plans referencing them: **1**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-AUTONOMOUS-MACHINES-79` | 1 |

**Governed artifacts (first 12):**

| Path |
|---|
| `AdvancedMachineContracts.cs` |
| `Assets/Ashfall.Core/AdvancedMachinery/AdvancedMachineContracts.cs` |
| `PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140_APPENDIX-A_SCAFFOLD.md` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `AMC-140A` | `AdvancedMachineContracts.cs`, `Assets/Ashfall.Core/AdvancedMachinery/AdvancedMachineContracts.cs`, `PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140_APPENDIX-A_SCAFFOLD.md` |
| `AMC-140B` | no name match — resolve at claim time |
| `AMC-140C` | `AdvancedMachineContracts.cs`, `Assets/Ashfall.Core/AdvancedMachinery/AdvancedMachineContracts.cs`, `PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140_APPENDIX-A_SCAFFOLD.md` |
| `AMC-140D` | no name match — resolve at claim time |
| `AMC-140E` | `AdvancedMachineContracts.cs`, `Assets/Ashfall.Core/AdvancedMachinery/AdvancedMachineContracts.cs`, `PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140_APPENDIX-A_SCAFFOLD.md` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **2** · Test files: **0** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/UI/MercenaryBountyBoardPanel.cs`, `src/UI/SubterraneanDebtLedgerPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 0 | — |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no test reference found — coverage risk; no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dose_ledger` |
| `mercenary_bounties` |
| `subterranean` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--communique-board-selftest` |
| `--dose-ledger-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--faction-communique-board-selftest` |
| `--journal-weather-panel-selftest` |
| `--ledger-debt-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **8**.

| Event | First declaration |
|---|---|
| `OnBountyRequested` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnBountyRequestedDetailed` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnStageAdvanced` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/dose_items.json` |
| `Assets/StreamingAssets/Data/dose_locations.json` |
| `Assets/StreamingAssets/Data/dose_quests.json` |
| `Assets/StreamingAssets/Data/dose_registers.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **6** (68 files, 498 cases).

| Region | Files | Cases |
|---|---:|---:|
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |
| `Integration` | 16 | 74 |
| `NarrativeConsequence` | 1 | 20 |
| `PlayerCommand` | 1 | 1 |
| `Quests` | 4 | 25 |

**Verdict:** 498 cases sit under matching regions — run those first (`DutyRoster`, `Economy`, `Integration`, `NarrativeConsequence`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **263**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Dose/DoseRegisterSurface.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/CollectibleEffectDispatcher.cs` |
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/DutyRosterHostSession.cs` |
| `src/Host/DutyRosterSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **21**, of which versioned-ladder sections:
**2**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan_trade_network` | no |
| `collectible_discovery` | no |
| `contractor_roster` | no |
| `deep_well` | no |
| `dose_ledger` | yes |
| `duty_roster` | no |
| `dynamic_quests` | no |
| `economy` | no |
| `expansion_quest` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `breach_obstacle_secondary_effect` |
| `deep_coast` |
| `duty_roster` |
| `economy` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **81**
(CODEX_ONLY 15, GAMEPLAY_CONSUMED 48, OPTIONAL 5, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_expelled_survivor` |
| `flag_honored_debt` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 21 (laddered 2) · RNG streams 8 · host files 22 · catalogs 22 · test regions 6 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140
wave: 11
status: PROPOSED — foreman claim required
packages: AMC-140A, AMC-140B, AMC-140C, AMC-140D, AMC-140E
claim paths:
  - src/Dose/DoseRegisterSurface.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/CollectibleEffectDispatcher.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/bounty_board.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/deep_lore_locations.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 2 versioned save ladder(s) — extend, never fork
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
