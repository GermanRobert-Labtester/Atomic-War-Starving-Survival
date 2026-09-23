# PLAN-RUMOR-PROPAGATION-TRUTH-120 — Rumor Origin, Distortion & Market Effects

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ECONOMY-LEDGER-TRUTH-96, PLAN-RADIO-MEDIA-42, PLAN-NARRATIVE-GRAPH-18.
**Non-goals:** no new quest system (Plan 18 owns the graph), no price rules
beyond Plan 96's map, no wall-clock-driven rumor decay.

## 1. Outcome
`Economy/EconomyMarketRumorRules.cs` exists as the market side of rumor, and
narrative already carries facts the world can learn. What is missing is the
propagation contract: where a rumor originates, how it travels, how it
distorts, who knows it, and when it expires — all of which must be
deterministic and persisted with its owner rather than recomputed per panel.

| Deliverable | Detail |
|---|---|
| Origin ledger | a rumor's source (event fact id, place, witness) recorded at creation |
| Propagation rules | travel via visits/trade/radio per documented route; arrival modifies knowledge state (Plan 108 semantics for places, a parallel record for facts) |
| Distortion | discrete distortion steps (accurate → exaggerated → inverted) with seeded selection; a rumor never invents a fact id that does not exist |
| Market effect | price/availability effect is a row in Plan 96's ledger map, not a new multiplier set |
| Expiry | rumor lifetime in game days; expiry on the day boundary with catch-up after load |

## 2. Evidence
- `Assets/Ashfall.Core/Economy/EconomyMarketRumorRules.cs` (re-verify seam per package).
- Plan 108 owns knowledge-state semantics for places; this plan adds fact-level rumor rows in the same owner family, not a second knowledge system.
- Plan 42 owns radio as a delivery medium; radio does not own rumor state.
- Plan 18 owns narrative facts; rumor references their ids and never authors text.

## 3. Packages
- **RMT-120A** origin ledger + creation hook at the event seam.
- **RMT-120B** propagation rules + per-route tests (visit, trade, radio).
- **RMT-120C** distortion ladder with seeded selection + same-seed equality test.
- **RMT-120D** market effect rows handed to Plan 96; no local multipliers.
- **RMT-120E** expiry on day boundary incl. catch-up + save round-trip.

## 4. Acceptance & verification
- Same seed + same events → identical rumor set, states, and distortion across runs.
- A rumor never references an unknown fact id (typed failure at creation).
- Expiry across save/load matches a continuous run.
- `bash scripts/run_test.sh` on the economy/narrative regions touched.

## 5. Risks
Rumor as a second narrative graph → it references fact ids only; no authored text, no graph edits.
Panel recomputation → panels read the stored rumor list; the same-seed test covers the authority.

---

## 6. Expanded census (2 files · 452 lines)

Scope: `Assets/Ashfall.Core/Economy/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `EconomyMarketRumorRules.cs` | 41 | Support | **yes** | 0 | 0 | 0 |
| `RumorSystem.cs` | 411 | System | **yes** | 0 | 1 | 2 |

**Totals:** 0 banned refs · 1 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `rumor_hubs.json` | object[2 keys] |

**State surfaces:** `RumorSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Economy/` |
| Test references | 4 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

Domain files: 2. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-SILENT-FAILURE-35` | 1 |
| `PLAN-NOMADS-CARAVAN-CULTURE-82` | 1 |
| `PLAN-ECONOMY-LEDGER-TRUTH-96` | 1 |
| `PLAN-CONTRACT-BOARD-109` | 1 |
| `PLAN-TRADE-TELL-TRUTH-248` | 1 |
| `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RMT-120A` | no name match — resolve at claim time |
| `RMT-120B` | `EconomyMarketRumorRules.cs` |
| `RMT-120C` | no name match — resolve at claim time |
| `RMT-120D` | `EconomyMarketRumorRules.cs` |
| `RMT-120E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **5** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/RadioHostSession.cs`, `src/Host/RumorNetworkHostSession.cs`, `src/Host/RumorNetworkSelfTest.cs`, `src/Main.Economy.cs`, `src/Main.RumorNetwork.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Economy/Plan212MarketRumorRulesTests.cs`, `Ashfall.Core.Tests/InformationFlow/Plan131RumorNetworkIntegrationTests.cs`, `Ashfall.Core.Tests/InformationFlow/Plan203RumorNetworkIntegrationTests.cs`, `Ashfall.Core.Tests/InformationFlow/RumorSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `black_market` |
| `caravan_trade_network` |
| `economy` |
| `piezometer_network` |
| `radio` |
| `radio_program_production` |
| `radio_station` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--economy-selftest` |
| `--economy-uitest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |
| `--real-main-journey-selftest` |
| `--rumor-network-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **8**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| `Assets/StreamingAssets/Data/radiation_economy_social.json` |
| `Assets/StreamingAssets/Data/rumor_hubs.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (41 files, 329 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Economy` | 41 | 329 |

**Verdict:** 329 cases sit under matching regions — run those first (`Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **17**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/BlackMarketHostSession.cs` |
| `src/Host/BlackMarketSaveStore.cs` |
| `src/Host/EconomyHostSession.cs` |
| `src/Host/EconomySaveStore.cs` |
| `src/Host/RumorNetworkHostSession.cs` |
| `src/Host/RumorNetworkSaveStore.cs` |
| `src/Host/RumorNetworkSelfTest.cs` |
| `src/Main.BlackMarket.cs` |
| `src/Main.Economy.cs` |
| `src/Main.RumorNetwork.cs` |
| `src/Main.UiTests.Economy.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `economy` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(GAMEPLAY_CONSUMED 2).

| Catalog | Classification |
|---|---|
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 2 (laddered 0) · RNG streams 4 · host files 16 · catalogs 10 · test regions 1 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RUMOR-PROPAGATION-TRUTH-120
wave: 10
status: PROPOSED — foreman claim required
packages: RMT-120A, RMT-120B, RMT-120C, RMT-120D, RMT-120E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Host/BlackMarketHostSession.cs  # §19 candidate host surface
  - src/Host/BlackMarketSaveStore.cs  # §19 candidate host surface
  - src/Host/EconomyHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/barter_rules.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
  - godot --headless --path . -- --economy-selftest
dependencies:
  - coordinate: 7 other plan(s) name these artifacts (§12)
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
