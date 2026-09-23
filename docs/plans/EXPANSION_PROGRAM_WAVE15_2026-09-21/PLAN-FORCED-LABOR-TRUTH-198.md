# PLAN-FORCED-LABOR-TRUTH-198 — Labor Assignments, Dignity & Exit Paths

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-JUSTICE-LAW-37, PLAN-LABOUR-PROFESSIONS-68, PLAN-DUTY-ROSTER-TRUTH-101, PLAN-MORALE-UNREST-TRUTH-129.
**Non-goals:** no roster (Plan 101), no professions model (Plan 68), no graphic
content; the system is policy-and-consequence, not depiction.

## 1. Outcome
`Factions/ForcedLaborSystem.cs` (**401 lines**) is reachable and unaddressed:
compelled work assignments. Plan 68 owns professions, Plan 101 the roster, Plan
37 the legal basis, Plan 129 the collective consequence. The **compelled
assignment** lifecycle — who is assigned, for how long, with what effect on
them and the holdfast — is unowned, so the mechanic is either invisible or
unbounded.

| Deliverable | Detail |
|---|---|
| Assignment model | compelled workers with source (legal sentence, emergency measure, capture), duration, and task class |
| Roster integration | compelled work occupies roster slots and coverage through Plan 101's model — never a parallel assignment |
| Dignity/consequence | documented effects on the worker and the holdfast routed to Plan 129/64 owners; severity is bounded and visible |
| Exit paths | completion, commutation (Plan 37), release, or death — each with a record and notice |
| Save truth | assignments and remaining duration restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Factions/ForcedLaborSystem.cs` (401 lines; unaddressed — Wave 13/15 audit).
- Plan 101's coverage model must include compelled workers or the shelter double-counts labor.
- Plan 129 receives collective reaction; Plan 64 individual effects.
- Plan 37 owns commutation and legal basis.

## 3. Packages
- **FLT-198A** assignment model + source/duration table.
- **FLT-198B** roster integration test (no parallel assignment; coverage consistent).
- **FLT-198C** dignity/consequence routing (bounded, visible).
- **FLT-198D** exit path tests + records/notices.
- **FLT-198E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Compelled workers appear in roster coverage exactly once.
- Consequence severity stays within its bounded bands; exit paths always exist.
- Save/load preserves assignment state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/`.

## 5. Risks
Invisible labor → roster integration is the proof of visibility.
Unbounded severity → bands and exits are fixtures; the plan is administrative.

---

## 6. Expanded census (1 files · 401 lines)

Scope: `Assets/Ashfall.Core/Factions/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ForcedLaborSystem.cs` | 401 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `labor_camps.json` | object[2 keys] |

**State surfaces:** `ForcedLaborSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Factions/` |
| Test references | 2 name references across the test tree |
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
| `PLAN-FACTIONS-STATE-FAMILY-TRUTH-268` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FLT-198A` | no name match — resolve at claim time |
| `FLT-198B` | no name match — resolve at claim time |
| `FLT-198C` | no name match — resolve at claim time |
| `FLT-198D` | no name match — resolve at claim time |
| `FLT-198E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **3** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Main.Plans182_185.cs`, `src/UI/GameDashboardPanel.cs`, `src/UI/LaborUI.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Factions/ForcedLaborSystemTests.cs`, `Ashfall.Core.Tests/Integration/Plans182_185_CampaignContinuityTests.cs` |
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
| `forced_labor` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **14** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--dashboard-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--propaganda-campaign-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnLaborDisputeChanged` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnLaborObligation` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnLaborObligationCreated` | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` |
| `OnLaborObligationDetailed` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnLaborObligationReleased` | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **9**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/foundry_accords.json` |
| `Assets/StreamingAssets/Data/foundry_faction.json` |
| `Assets/StreamingAssets/Data/foundry_items.json` |
| `Assets/StreamingAssets/Data/foundry_production.json` |
| `Assets/StreamingAssets/Data/foundry_treaty_consequences.json` |
| `Assets/StreamingAssets/Data/labor_camps.json` |
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (109 files, 771 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Flagship11` | 7 | 63 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `NarrativeConsequence` | 1 | 20 |
| `Production` | 3 | 12 |

**Verdict:** 771 cases sit under matching regions — run those first (`Campaign`, `Economy`, `Flagship11`, `Foundry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **393**
(233 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/CollectibleEffectDispatcher.cs` |
| `src/Host/ForcedLaborSaveStore.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |
| `src/Host/HostCli.Plans162_165.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **15**, of which versioned-ladder sections:
**2**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `campaign` | no |
| `campaign_day` | no |
| `caravan_trade_network` | no |
| `collectible_discovery` | no |
| `dose_ledger` | yes |
| `economy` | no |
| `faction_espionage` | no |
| `forced_labor` | no |
| `foundry` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **7**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `breach_obstacle_secondary_effect` |
| `cupola_foundry` |
| `economy` |
| `foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **62**
(CODEX_ONLY 15, GAMEPLAY_CONSUMED 37, OPTIONAL 3, UNRESOLVED 7).

| Catalog | Classification |
|---|---|
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `dose_items.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_broke_treaty` |
| `flag_chosen_faction_side` |
| `flag_honored_debt` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 15 (laddered 2) · RNG streams 7 · host files 23 · catalogs 19 · test regions 8 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FORCED-LABOR-TRUTH-198
wave: 15
status: PROPOSED — foreman claim required
packages: FLT-198A, FLT-198B, FLT-198C, FLT-198D, FLT-198E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/cupola_foundry_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
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
