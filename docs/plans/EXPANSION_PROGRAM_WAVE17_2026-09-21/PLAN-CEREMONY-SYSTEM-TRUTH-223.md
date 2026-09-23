# PLAN-CEREMONY-SYSTEM-TRUTH-223 — Ceremonies: Occasions, Order & Observance Effects

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-VERTICAL-CULTURE-04, PLAN-BELIEF-IDEOLOGY-36, PLAN-MORTUARY-MEMORIAL-TRUTH-123, PLAN-MORALE-UNREST-TRUTH-129.
**Non-goals:** no festival content (Plan 4), no doctrine (Plan 36), no death
rites (Plan 123), no mark model (Plan 129).

## 1. Outcome
`Narrative/CeremonySystem.cs` (**360 lines**) is reachable and unaddressed:
ceremonies as timed, ordered observances (oaths, remembrances, dedications)
that change how the holdfast feels. Culture and belief own content/doctrine;
the **observance runtime** — occasion trigger, participation, and effect — is
unowned, so ceremonies are either text or an unexplained mood change.

| Deliverable | Detail |
|---|---|
| Occasion model | ceremony occasions with trigger (day, event, anniversary) on the canonical clock |
| Participation | who attends from availability (Plan 101/103); attendance affects the ceremony's effect size |
| Effects | outcomes route to Plan 129/64 owners; no private mood value |
| Observance record | which ceremonies were held, when, and attendance persist as a record |
| Save truth | scheduled and held ceremonies restore; a load never re-holds or skips |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/CeremonySystem.cs` (360 lines; unaddressed — Wave 17 audit).
- Plan 4/36 own content/doctrine the ceremony enacts; Plan 123 owns death rites (a ceremony subclass).
- Plan 129/64 receive effects; Plan 101/103 supply attendance.
- Plan 138 can carry notices.

## 3. Packages
- **CST-223A** occasion model + triggers.
- **CST-223B** participation/attendance tests.
- **CST-223C** effect routing (no private mood proof).
- **CST-223D** observance record + persistence.
- **CST-223E** save round-trip; no re-hold/skip on load.

## 4. Acceptance & verification
- Triggers fire once per occasion; attendance changes effect per the table.
- Effects appear in named owners; records persist.
- Save/load preserves schedule and history.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Mood duplication → routing only; a fixture asserts no local value.
Overlap with 123 → death rites delegate to Plan 123's pipeline; the ceremony wraps it.

---

## 6. Expanded census (1 files · 360 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CeremonySystem.cs` | 360 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `CeremonySystem.cs`.

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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CST-223A` | no name match — resolve at claim time |
| `CST-223B` | no name match — resolve at claim time |
| `CST-223C` | no name match — resolve at claim time |
| `CST-223D` | no name match — resolve at claim time |
| `CST-223E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 2. Host files: **2** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Main.Plans198_201.cs`, `src/UI/CeremonyFestivalPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Integration/Plans198_201_LateGameSystemsIntegrationTests.cs`, `Ashfall.Core.Tests/LateGameCrossPlanScenarioTests.cs`, `Ashfall.Core.Tests/Narrative/CeremonySystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `ceremony` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--late-tech-mobility-selftest` |
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

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **0**.

| Catalog |
|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog filename shares a token with this domain — the authority is likely code-defined or its data lives in a broader catalog. Not a conclusion; check the owning loader.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **6** (65 files, 469 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Economy` | 41 | 329 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `Lifecycle` | 1 | 5 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 469 cases sit under matching regions — run those first (`Audio`, `Economy`, `Holdfast`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **384**
(233 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/CeremonySaveStore.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |
| `src/Host/HostCli.Plans162_165.cs` |
| `src/Host/HostCli.PlansB86_B89.cs` |
| `src/Host/PanelBindLifecycleSelfTest.cs` |
| `src/Host/Plans130To133HostSessions.cs` |
| `src/Host/Plans74To77HostSessions.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan_trade_network` | no |
| `ceremony` | no |
| `economy` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `nuclear_core_lifecycle` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **16**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 10, OPTIONAL 2, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |
| `holdfast_items.json` | GAMEPLAY_CONSUMED |
| `holdfast_locations.json` | GAMEPLAY_CONSUMED |
| `holdfast_npcs.json` | UNRESOLVED |
| `holdfast_quests.json` | GAMEPLAY_CONSUMED |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 7 (laddered 1) · RNG streams 4 · host files 16 · catalogs 10 · test regions 6 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CEREMONY-SYSTEM-TRUTH-223
wave: 17
status: PROPOSED — foreman claim required
packages: CST-223A, CST-223B, CST-223C, CST-223D, CST-223E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/CeremonySaveStore.cs  # §19 candidate host surface
  - src/Host/HoldfastTerminalPanel.cs  # §19 candidate host surface
  - caravan_trade_routes.json  # §17 catalog (verify schema + consumer)
  - economy_goods.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --expedition-panel-lifecycle
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
