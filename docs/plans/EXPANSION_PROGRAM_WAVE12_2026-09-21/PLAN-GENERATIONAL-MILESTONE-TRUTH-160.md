# PLAN-GENERATIONAL-MILESTONE-TRUTH-160 — Second-Generation Milestones & Continuity

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-FAMILY-DYNASTY-43, PLAN-MUTATION-HEREDITY-81, PLAN-ENDGAME-EVALUATION-TRUTH-137.
**Implementation scaffold:** [`PLAN-GENERATIONAL-MILESTONE-TRUTH-160_APPENDIX-A_SCAFFOLD.md`](PLAN-GENERATIONAL-MILESTONE-TRUTH-160_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-SUCCESSION-LEGACY-TRUTH-252` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no genetics model (Plan 81), no family graph (Plan 43), no
ending resolver (Plan 137).

## 1. Outcome
`Generations/SecondGenerationMilestoneEngine.cs` is host-unreachable (Plan 1
Appendix A) and unclaimed. A holdfast that survives long enough raises a second
generation; milestones (first birth, first child reaching work age, generational
handover) are the narrative backbone of a long campaign, and they are also
inputs an ending evaluator may read (Plan 137).

| Deliverable | Detail |
|---|---|
| Milestone set | documented milestones with their triggering facts (birth, aging, role entry, death of the first generation) |
| Owner discipline | milestones read Plan 43/81 state and store a dated record; they never compute heredity or relations |
| Once-only rule | a milestone fires once per campaign; a load never re-fires one |
| Continuity | a generational handover changes role/office holders through Plan 141's ledger, not by rewriting survivors |
| Ending inputs | milestone records are exposed to Plan 137's input table as stored facts |

## 2. Evidence
- `Assets/Ashfall.Core/Generations/SecondGenerationMilestoneEngine.cs` (whole directory; verified).
- Plan 1 Appendix A/H: host-unreachable; sized small (a bounded seal).
- Plan 43 owns family state; Plan 81 owns hereditary effects.
- Plan 137's input table is where milestone records land.

## 3. Packages
- **GMT-160A** milestone table with triggering facts.
- **GMT-160B** read-only owner discipline + no-compute test.
- **GMT-160C** once-only firing tests (reload, replay).
- **GMT-160D** handover via Plan 141 ledger + no-rewrite test.
- **GMT-160E** ending-input exposure check with a Plan 137 fixture.

## 4. Acceptance & verification
- Every milestone fires once and records its day; reload does not re-fire.
- Handover changes holders only through the institutions ledger.
- Milestone records appear in Plan 137's input table and are read, not recomputed.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Generations/` (create if absent).

## 5. Risks
Recompute-on-load → stored records with once-only flags; the replay test is the guard.
Scope creep into heredity → explicitly excluded; the boundary test asserts no genetics logic lives here.

---

## 6. Expanded census (1 files · 229 lines)

Scope: `Assets/Ashfall.Core/Generations/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SecondGenerationMilestoneEngine.cs` | 229 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Generations/` |
| Test references | 1 name references across the test tree |
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
Governed artifacts: 4. Other plans referencing them: **2**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `EVIDENCE` | 1 |
| `PLAN-FAMILY-DYNASTY-43` | 1 |

**Governed artifacts (first 12):**

| Path |
|---|
| `Assets/Ashfall.Core/Generations/SecondGenerationMilestoneEngine.cs` |
| `Generations/SecondGenerationMilestoneEngine.cs` |
| `PLAN-GENERATIONAL-MILESTONE-TRUTH-160_APPENDIX-A_SCAFFOLD.md` |
| `SecondGenerationMilestoneEngine.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `GMT-160A` | `Assets/Ashfall.Core/Generations/SecondGenerationMilestoneEngine.cs`, `Generations/SecondGenerationMilestoneEngine.cs`, `PLAN-GENERATIONAL-MILESTONE-TRUTH-160_APPENDIX-A_SCAFFOLD.md` |
| `GMT-160B` | no name match — resolve at claim time |
| `GMT-160C` | no name match — resolve at claim time |
| `GMT-160D` | no name match — resolve at claim time |
| `GMT-160E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/UI/CenturySeedPanel.cs`, `src/UI/TimeCapsulePanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Generations/SecondGenerationMilestoneEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `time_capsules` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **9** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--day1-to-day2-milestone-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--time-capsule-selftest` |
| `--time-capsules-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnSpecialtyMilestone` | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/cryo_seed_ampoule_logs.json` |
| `Assets/StreamingAssets/Data/narrative/heirloom_seed_viability_reports.json` |
| `Assets/StreamingAssets/Data/narrative/pneumatic_carrier_capsule_logs.json` |
| `Assets/StreamingAssets/Data/narrative/silica_gel_seed_desiccation_audits.json` |
| `Assets/StreamingAssets/Data/narrative/trade_ledgers_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/wasteland_trade_caravan_routes.json` |
| `Assets/StreamingAssets/Data/time_capsules.json` |
| `Assets/StreamingAssets/Data/trade_embargoes.json` |
| `Assets/StreamingAssets/Data/trade_screen_scenarios.json` |
| `Assets/StreamingAssets/Data/trade_specialties.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **4** (44 files, 348 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Economy` | 41 | 329 |
| `Holdfast` | 1 | 13 |
| `Lifecycle` | 1 | 5 |
| `PlayerCommand` | 1 | 1 |

**Verdict:** 348 cases sit under matching regions — run those first (`Economy`, `Holdfast`, `Lifecycle`, `PlayerCommand`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **237**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/CaravanTradeSaveStore.cs` |
| `src/Host/GenerationalSaveStore.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HoldfastTradeSaveStore.cs` |
| `src/Host/HoldfastTradeSaveStoreSelfTest.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/PanelBindLifecycleSelfTest.cs` |
| `src/Host/TimeCapsuleHostSession.cs` |
| `src/Host/TimeCapsuleSaveStore.cs` |
| `src/Host/TimeCapsuleSelfTest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **14**, of which versioned-ladder sections:
**2**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan` | no |
| `caravan_trade_network` | no |
| `cryo_vault` | no |
| `dose_ledger` | yes |
| `economy` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `nuclear_core_lifecycle` | no |
| `pneumatic_dispatch` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **155**
(CODEX_ONLY 135, GAMEPLAY_CONSUMED 13, OPTIONAL 3, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `audio_logs_expansion_05.json` | OPTIONAL |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |
| `holdfast_items.json` | GAMEPLAY_CONSUMED |
| `holdfast_locations.json` | GAMEPLAY_CONSUMED |
| `holdfast_npcs.json` | UNRESOLVED |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 14 (laddered 2) · RNG streams 4 · host files 16 · catalogs 22 · test regions 4 · flags 9

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-GENERATIONAL-MILESTONE-TRUTH-160
wave: 12
status: PROPOSED — foreman claim required
packages: GMT-160A, GMT-160B, GMT-160C, GMT-160D, GMT-160E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/CaravanTradeSaveStore.cs  # §19 candidate host surface
  - src/Host/GenerationalSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/caravan_trade_routes.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
  - godot --headless --path . -- --day1-to-day2-milestone-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
