# PLAN-POLITICS-SYSTEM-TRUTH-221 — Narrative Politics: Factions, Debates & Consequences

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SHELTER-POLITICS-69, PLAN-LEADERSHIP-TRUTH-173, PLAN-SOCIAL-DYNAMICS-TRUTH-214, PLAN-FACTION-BRANCH-TRUTH-171.
**Non-goals:** no policy surfaces (Plan 69), no authority/succession (Plan 173),
no group structure (Plan 214), no branch lineage (Plan 171).

## 1. Outcome
`Narrative/PoliticsSystem.cs` (**459 lines**) is reachable and unaddressed: the
**narrative-facing** political layer — debates, positions, and political
consequences attached to story moments. Plan 69 owns policy surfaces, Plan 173
authority, Plan 214 groups. Without a contract, political story moments apply
unexplained standing shifts.

| Deliverable | Detail |
|---|---|
| Position model | political positions with holders (Plan 141/43) and a stance per issue |
| Debate/scene hook | political scenes create/change positions through one hook; no panel-side mutation |
| Consequence routing | outcomes write to Plan 29 standing, Plan 173 legitimacy, Plan 129 marks — no private political score |
| Recording | political history (who argued what, when) persists and is readable in an existing surface |
| Save truth | positions and history restore; a load never re-runs a debate |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/PoliticsSystem.cs` (459 lines; unaddressed — Wave 17 audit).
- Plans 69/173/214 are the adjacent owners; boundaries stated in each.
- Plan 29/129 receive consequences.
- Plan 18 owns the story moments that trigger scenes.

## 3. Packages
- **PLT-221A** position model + holder source table.
- **PLT-221B** scene hook + no-panel-mutation audit.
- **PLT-221C** consequence routing tests (no private score proof).
- **PLT-221D** history record + read surface test.
- **PLT-221E** save round-trip; no re-run on load.

## 4. Acceptance & verification
- Positions change only through the hook; consequences appear in named owners.
- History is readable and dates from the canonical clock.
- Save/load preserves positions and history.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Hidden standing shifts → routing tests and history are the guards.
Overlap with 69/173 → this layer stores positions; policy/authority stay there.

---

## 6. Expanded census (1 files · 459 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PoliticsSystem.cs` | 459 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `PoliticsSystem.cs`.

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

Domain files: 1. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PLT-221A` | no name match — resolve at claim time |
| `PLT-221B` | no name match — resolve at claim time |
| `PLT-221C` | no name match — resolve at claim time |
| `PLT-221D` | no name match — resolve at claim time |
| `PLT-221E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 2. Host files: **3** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Main.Plans182_185.cs`, `src/UI/GameDashboardPanel.cs`, `src/UI/PoliticsUI.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Integration/Plans182_185_CampaignContinuityTests.cs`, `Ashfall.Core.Tests/Narrative/JusticeSystemTests.cs`, `Ashfall.Core.Tests/Narrative/PoliticsSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `settlement_politics` |
| `wasteland_justice` |

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
shares a domain token: **7** (124 files, 979 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Flagship11` | 7 | 63 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 979 cases sit under matching regions — run those first (`Campaign`, `Economy`, `Flagship11`, `Holdfast`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **390**
(233 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |
| `src/Host/HostCli.Plans162_165.cs` |
| `src/Host/HostCli.PlansB86_B89.cs` |
| `src/Host/JusticeSaveStore.cs` |
| `src/Host/NarrativeContinuitySelfTest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **12**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `campaign` | no |
| `campaign_day` | no |
| `caravan_trade_network` | no |
| `economy` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `narrative` | no |
| `narrative_questlines` | no |
| `procedural_narrative` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **5**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |
| `narrative` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **299**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 14, OPTIONAL 4, UNRESOLVED 2).

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

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 12 (laddered 1) · RNG streams 5 · host files 17 · catalogs 11 · test regions 7 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-POLITICS-SYSTEM-TRUTH-221
wave: 17
status: PROPOSED — foreman claim required
packages: PLT-221A, PLT-221B, PLT-221C, PLT-221D, PLT-221E
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
