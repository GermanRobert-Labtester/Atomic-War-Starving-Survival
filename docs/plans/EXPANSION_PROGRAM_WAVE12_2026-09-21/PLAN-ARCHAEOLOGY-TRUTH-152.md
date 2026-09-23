# PLAN-ARCHAEOLOGY-TRUTH-152 — Survey, Excavation Lifecycle & Find Provenance

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ANCIENT-RUINS-VAULTS-84, PLAN-CARTOGRAPHY-LANDMARKS-70, PLAN-INVENTORY-CONSERVATION-93.
**Implementation scaffold:** [`PLAN-ARCHAEOLOGY-TRUTH-152_APPENDIX-A_SCAFFOLD.md`](PLAN-ARCHAEOLOGY-TRUTH-152_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-ANCIENT-RUINS-VAULTS-84` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no ruin content (Plan 84), no landmark discovery (Plan 70), no
new inventory store (Plan 93).

## 1. Outcome
`Archaeology/ArchaeologySystem.cs` is a single-file authority for structured
digs: the systematic layer Plan 84's ruin content sits on. Without a lifecycle,
a dig is either instant loot or an unbounded timer — and finds have no
provenance (where, how deep, what care was taken).

| Deliverable | Detail |
|---|---|
| Dig lifecycle | survey → permit/decision → excavation stages → stabilized/abandoned, with a day-based stage clock |
| Depth/strata | finds keyed to documented strata; deeper stages require more time and supplies, and hazards escalate per a table |
| Find provenance | each recovered item carries site id, stage, and condition; display reads the record |
| Conservation | recovered items enter through Plan 93's seam; site depletion is tracked so a dig cannot be re-looted |
| Abandonment | an abandoned dig leaves a documented state (open pit, partial recovery) visible to later visits |

## 2. Evidence
- `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs` (whole directory; verified).
- Plan 84 owns ruin/vault content; this plan supplies the systematic dig model.
- Plan 70 owns landmark discovery; a dig site's discovery state reads it.
- Plan 93's wrapper verifies recovered-item counts.

## 3. Packages
- **ARC-152A** lifecycle + stage clock table.
- **ARC-152B** strata/hazard table + supply consumption per stage.
- **ARC-152C** find provenance record + display-from-record test.
- **ARC-152D** depletion model + no-re-loot test; conservation check.
- **ARC-152E** abandonment state + revisit visibility test.

## 4. Acceptance & verification
- Stage transitions respect the clock and supplies; no stage skips.
- A depleted site yields nothing on revisit; conservation wrapper balances.
- Abandoned digs are visibly abandoned on revisit (state, not prose invention).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Archaeology/` (create if absent).

## 5. Risks
Loot fountain → depletion + supply costs are the constraints, both tested.
Overlap with 84 → this plan is process; 84 is places.

---

## 6. Expanded census (1 files · 317 lines)

Scope: `Assets/Ashfall.Core/Archaeology/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ArchaeologySystem.cs` | 317 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `ArchaeologySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Archaeology/` |
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

Domain files: 1. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SCIENCE-EDUCATION-38` | 1 |
| `PLAN-ANCIENT-RUINS-VAULTS-84` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `ARC-152A` | no name match — resolve at claim time |
| `ARC-152B` | no name match — resolve at claim time |
| `ARC-152C` | no name match — resolve at claim time |
| `ARC-152D` | no name match — resolve at claim time |
| `ARC-152E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Main.Plans186_189.cs`, `src/UI/ArchaeologyExcavationPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`, `Ashfall.Core.Tests/Integration/Plans186_189_CampaignContinuityTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archaeology` |
| `campaign` |
| `campaign_day` |
| `excavation` |
| `excavation_hazards` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **13** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
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
| `--real-campaign-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnExcavationChanged` | `Assets/Ashfall.Core/ExcavationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/excavation_sites.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **7** (99 files, 675 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Archaeology` | 1 | 4 |
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Excavation` | 1 | 5 |
| `Flagship11` | 7 | 63 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |

**Verdict:** 675 cases sit under matching regions — run those first (`Archaeology`, `Campaign`, `Economy`, `Excavation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **390**
(233 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/ArchaeologySaveStore.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/ExcavationHazardSaveStore.cs` |
| `src/Host/ExcavationHostSession.cs` |
| `src/Host/ExcavationSaveStore.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **11**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `archaeology` | no |
| `black_market` | no |
| `campaign` | no |
| `campaign_day` | no |
| `caravan_trade_network` | no |
| `economy` | no |
| `excavation` | no |
| `excavation_hazards` | no |
| `holdfast` | yes |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **5**.

| Stream |
|---|
| `anomaly_hazard` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **25**
(CODEX_ONLY 8, GAMEPLAY_CONSUMED 13, OPTIONAL 2, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `dive_sites.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `excavation_sites.json` | UNRESOLVED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |
| `holdfast_items.json` | GAMEPLAY_CONSUMED |
| `holdfast_locations.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 11 (laddered 1) · RNG streams 5 · host files 17 · catalogs 13 · test regions 7 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ARCHAEOLOGY-TRUTH-152
wave: 12
status: PROPOSED — foreman claim required
packages: ARC-152A, ARC-152B, ARC-152C, ARC-152D, ARC-152E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/ArchaeologySaveStore.cs  # §19 candidate host surface
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/excavation_hazard_mitigation.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Archaeology/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
