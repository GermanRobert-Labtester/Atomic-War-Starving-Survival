# PLAN-JOURNEY-CONTEXT-TRUTH-156 — The Travel Context Contract: What a Journey Knows

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TRANSPORT-EXPEDITION-30, PLAN-SPATIAL-SIM-AUTHORITY-95, PLAN-DETERMINISM-CROSS-HOST-89.
**Implementation scaffold:** [`PLAN-JOURNEY-CONTEXT-TRUTH-156_APPENDIX-A_SCAFFOLD.md`](PLAN-JOURNEY-CONTEXT-TRUTH-156_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-TRAVEL-ENCOUNTER-TRUTH-177` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no travel resolution (Plan 30 owns it), no route topology
(Plan 95), no new expedition content.

## 1. Outcome
`Journeys/JourneyExecutionContext.cs` is a single file that defines what a
journey carries in its context. It is the interface between travel simulation
and everyone who reacts to travel (encounters, weather, waystations, journal).
Without a stated contract, each consumer invents its own view — the classic
source of divergence between the headless run and the live game.

| Deliverable | Detail |
|---|---|
| Context fields | documented fields (party, route, day/hour, supplies, cargo, flags) with the owner of each |
| Read-only rule | consumers read the context; only the travel owner mutates it |
| Snapshot equality | the context captured on the same seed/day is identical across headless and host runs (Plan 89 pattern) |
| Consumer table | every current consumer with the fields it reads; an undocumented read is a defect |
| Persistence | a journey saved mid-leg restores an equivalent context (fields that are derived are re-derived identically) |

## 2. Evidence
- `Assets/Ashfall.Core/Journeys/JourneyExecutionContext.cs` (whole directory; verified).
- Plan 89 supplies the cross-host comparison method this plan reuses.
- Plan 30 owns travel state; Plan 95 supplies route facts the context references.
- Plan 1 Appendix P: the file is not in the orphan set — context structs are documentation-bearing.

## 3. Packages
- **JCT-156A** field table with owners.
- **JCT-156B** consumer audit (documented reads only).
- **JCT-156C** snapshot equality test (headless vs host, same seed/day).
- **JCT-156D** read-only guard: a write attempt from a consumer fails a scan/test.
- **JCT-156E** mid-leg save/restore equivalence test.

## 4. Acceptance & verification
- Every context field has an owner; every consumer read is listed.
- Paired snapshots are byte-equal for the same seed/day.
- Mid-leg save restores an equivalent context (derived fields identical).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Journeys/` (create if absent).

## 5. Risks
Context becoming a god-object → the field table is closed; new fields need an owner row.
Consumer drift → the audit runs in CI as a scan.

---

## 6. Expanded census (1 files · 76 lines)

Scope: `Assets/Ashfall.Core/Journeys/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `JourneyExecutionContext.cs` | 76 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Journeys/` |
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

Domain files: 1. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `JCT-156A` | no name match — resolve at claim time |
| `JCT-156B` | no name match — resolve at claim time |
| `JCT-156C` | no name match — resolve at claim time |
| `JCT-156D` | no name match — resolve at claim time |
| `JCT-156E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Main.UiTests.RealCampaignJourney.cs`, `src/UI/OnboardingHintPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Journeys/EndToEndPlayerJourneyTests.cs` |
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
| `onboarding` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **12** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--onboarding-journey-selftest` |
| `--onboarding-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
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

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (190 files, 1495 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Flagship11` | 7 | 63 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `Lifecycle` | 1 | 5 |
| `Shelter` | 87 | 754 |
| `WildlifeTrapping` | 5 | 70 |

**Verdict:** 1495 cases sit under matching regions — run those first (`Campaign`, `Economy`, `Flagship11`, `Holdfast`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **379**
(232 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HostCli.Onboarding.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/OnboardingSaveStore.cs` |
| `src/Host/PanelBindLifecycleSelfTest.cs` |
| `src/Main.AdvancedShelterSystems.cs` |
| `src/Main.Anomaly.cs` |
| `src/Main.Application.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **25**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `black_market` | no |
| `campaign` | no |
| `campaign_day` | no |
| `caravan_trade_network` | no |
| `economy` | no |
| `expanded_shelter` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `nuclear_core_lifecycle` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **7**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `anomaly_hazard` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **24**
(CODEX_ONLY 5, GAMEPLAY_CONSUMED 13, OPTIONAL 2, UNRESOLVED 4).

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
| `muster_epilogues.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 25 (laddered 1) · RNG streams 7 · host files 19 · catalogs 11 · test regions 8 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-JOURNEY-CONTEXT-TRUTH-156
wave: 12
status: PROPOSED — foreman claim required
packages: JCT-156A, JCT-156B, JCT-156C, JCT-156D, JCT-156E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - caravan_trade_routes.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
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
