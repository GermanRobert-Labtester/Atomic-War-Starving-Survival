# PLAN-NOMADS-CARAVAN-CULTURE-82 — Moving Peoples, Camps, Trade Fairs & Custom

**Wave 7 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TRANSPORT-EXPEDITION-30, PLAN-WARLORDS-DIPLOMACY-29,
PLAN-SHELTER-POLITICS-69.
**Implementation scaffold:** [`PLAN-NOMADS-CARAVAN-CULTURE-82_APPENDIX-A_SCAFFOLD.md`](PLAN-NOMADS-CARAVAN-CULTURE-82_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-FACTION-BRANCH-TRUTH-171` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no real cultures/ethnicities; fictional peoples only; no
"exotic" framing — they are economic and political actors with agency.

## Outcome
Movement exists (`TravelingCaravanSystem`, `merchant_caravans.json`,
`caravans.json`, `caravan_trade_routes.json`, graph travel, seasonal migration
engines, visitors) but not **people who live on the move**: camps, customs,
fairs, contracts, and the politics of passage.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Camp visits | visitor/caravan hosts | host, trade, negotiate | access, goods, news |
| Trade fairs | economy + calendar | schedule, attend | price windows, contracts |
| Passage rights | territory + diplomacy | grant, tax, refuse | standing, route safety |
| Customs | narrative + relations | respect, offend, learn | trust, cultural friction |
| Seasonal routes | seasonal migration + map | predict, prepare | arrival windows, hazards |
| Hosting costs | shelter + inventory | supply camp | food/water pressure, goodwill |
| News network | rumor/information flow | share, withhold | intel, reputation |
| Contracts | mercenary/caravan | escort, deliver | income, risk |

## Evidence
- Core: `TravelingCaravanSystem`, `MerchantCaravan`/catalogs, `Visitors/VisitorIntegrationSystem.cs` (orphan), `Economy/SeasonalHumanMigrationEngine.cs` (orphan), `Economy/MigrationConsequenceEngine.cs` (orphan), `InformationFlow/RumorSystem`.
- Data: `caravans.json`, `merchant_caravans.json`, `caravan_trade_routes.json`, `waystations.json`, `outposts.json`, `rumor_hubs.json`.
- Sealed prior: Plan 34 migration (DEC-34), Plan 199 seasonal migration, Plan 58 outposts/waystations, Plan 203 rumor network (7/7).
- Contracts: one population source (registry), one rumor source, no duplicate visitors.

## Packages
- **NC-82A** camp host: arrival, duration, needs, departure; hosting costs and goodwill.
- **NC-82B** fairs: calendar windows with trade price effects and contract offers.
- **NC-82C** passage policy: toll/refuse/grant with standing and route consequences.
- **NC-82D** customs/friction: respectful/offensive interactions with authored, fictional customs.
- **NC-82E** seasonal prediction: route/arrival forecast from migration engines; expedition windows.
- **NC-82F** news exchange: rumors/intel via the rumor owner (Plan 203 briefing).
- **NC-82G** content volumes: +6 peoples, +10 camps, +10 fair events, +8 customs, +8 contracts; fictional.

## Acceptance & verification
- No duplicate visitor/population stores; arrivals deterministic per seed; hosting pressure shows in needs.
- `godot --headless --path . -- --caravan-selftest`; rumor suites; `--survivors-selftest`.

## Risks
Caravan spam → arrival windows + capacity + seasonal spacing; friendly camps can be declined.

---

## 6. Expanded census (7 files · 1,960 lines)

Scope: `Assets/Ashfall.Core/Economy/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Loader 1 · Support 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CaravanAtomicTrader.cs` | 162 | Support | — | 0 | 0 | 6 |
| `CaravanCatalogLoader.cs` | 143 | Loader | — | 0 | 0 | 0 |
| `CaravanTradeNetworkSystem.cs` | 670 | System | — | 0 | 0 | 2 |
| `CaravanTradeRouteCatalog.cs` | 68 | Catalog | — | 0 | 0 | 0 |
| `MigrationConsequenceEngine.cs` | 144 | System | — | 0 | 0 | 2 |
| `SeasonalHumanMigrationEngine.cs` | 162 | System | — | 0 | 0 | 2 |
| `VisitorIntegrationSystem.cs` | 611 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 5 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `caravan_trade_routes.json` | object[2 keys] |
| `caravans.json` | object[2 keys] |
| `merchant_caravans.json` | object[2 keys] |
| `visitor_templates.json` | object[2 keys] |
| `wasteland_trade_caravan_routes.json` | object[3 keys] |

**State surfaces:** `CaravanAtomicTrader.cs`, `CaravanTradeNetworkSystem.cs`, `MigrationConsequenceEngine.cs`, `SeasonalHumanMigrationEngine.cs`, `VisitorIntegrationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Economy/` |
| Test references | 14 name references across the test tree |
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

Domain files: 7. Other plans referencing their names: **11**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 6 |
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-ASYLUM-REFUGEES-85` | 3 |
| `PLAN-ECONOMY-LEDGER-TRUTH-96` | 3 |
| `PLAN-TRADE-TELL-TRUTH-248` | 3 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 2 |
| `PLAN-RATIONING-TRUTH-174` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `NC-82A` | no name match — resolve at claim time |
| `NC-82B` | `CaravanAtomicTrader.cs`, `CaravanTradeNetworkSystem.cs`, `CaravanTradeRouteCatalog.cs` |
| `NC-82C` | `CaravanTradeRouteCatalog.cs` |
| `NC-82D` | no name match — resolve at claim time |
| `NC-82E` | `SeasonalHumanMigrationEngine.cs`, `CaravanTradeRouteCatalog.cs`, `MigrationConsequenceEngine.cs` |
| `NC-82F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 7; intra-domain edges: **1**; isolated files:
**5**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `MigrationConsequenceEngine` | `SeasonalHumanMigrationEngine` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `SeasonalHumanMigrationEngine` | 1 |
| `CaravanAtomicTrader` | 0 |
| `CaravanCatalogLoader` | 0 |
| `CaravanTradeNetworkSystem` | 0 |
| `CaravanTradeRouteCatalog` | 0 |
| `MigrationConsequenceEngine` | 0 |
| `VisitorIntegrationSystem` | 0 |

**Class split:** hub 0 · sink 1 · source 1 · isolated 5.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 7. Host files: **3** · Test files: **10** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/HostCli.Cartography.cs`, `src/Main.AdvancedShelterSystems.cs`, `src/Main.CampaignOwners.cs` |
| Tests (`Ashfall.Core.Tests/`) | 10 | `Ashfall.Core.Tests/CollectibleMerchantSimulationTests.cs`, `Ashfall.Core.Tests/Economy/CaravanAtomicTraderTests.cs`, `Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs`, `Ashfall.Core.Tests/Economy/MigrationConsequenceEngineTests.cs`, `Ashfall.Core.Tests/Economy/SeasonalHumanMigrationEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan` |
| `caravan_trade_network` |
| `holdfast_trade` |
| `piezometer_network` |
| `route_infrastructure` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--caravan-selftest` |
| `--deep-coast-route-selftest` |
| `--holdfast-trade-save-selftest` |
| `--rumor-network-selftest` |
| `--traveling-caravan-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/muster_faction_culture.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/scavenger_expedition_route_notes.json` |
| `Assets/StreamingAssets/Data/narrative/trade_ledgers_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/wasteland_trade_caravan_routes.json` |
| `Assets/StreamingAssets/Data/piezometer_network_catalog.json` |
| `Assets/StreamingAssets/Data/pneumatic_network_catalog.json` |
| `Assets/StreamingAssets/Data/rail_network.json` |
| `Assets/StreamingAssets/Data/seasonal_events.json` |
| `Assets/StreamingAssets/Data/trade_embargoes.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (24 files, 134 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Culture` | 7 | 40 |
| `Integration` | 16 | 74 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 134 cases sit under matching regions — run those first (`Culture`, `Integration`, `NarrativeConsequence`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **20**
(7 of them panels/HUD).

| Host file |
|---|
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/CaravanSaveStore.cs` |
| `src/Host/CaravanTradeSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HoldfastTradeSaveStore.cs` |
| `src/Host/HoldfastTradeSaveStoreSelfTest.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/Host/RouteInfrastructureSaveStore.cs` |
| `src/Host/RumorNetworkHostSession.cs` |
| `src/Host/RumorNetworkSaveStore.cs` |
| `src/Host/RumorNetworkSelfTest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **5**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `caravan` | no |
| `caravan_trade_network` | no |
| `holdfast_trade` | no |
| `piezometer_network` | no |
| `route_infrastructure` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |
| `wildlife_migration` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **14**
(CODEX_ONLY 5, GAMEPLAY_CONSUMED 4, OPTIONAL 2, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `muster_faction_culture.json` | UNRESOLVED |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | CODEX_ONLY |
| `narrative/scavenger_expedition_route_notes.json` | CODEX_ONLY |
| `narrative/trade_ledgers_expansion.json` | CODEX_ONLY |
| `narrative/wasteland_trade_caravan_routes.json` | CODEX_ONLY |
| `rail_network.json` | GAMEPLAY_CONSUMED |
| `seasonal_events.json` | UNRESOLVED |
| `trade_screen_scenarios.json` | OPTIONAL |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 11
**Surface:** save sections 5 (laddered 0) · RNG streams 3 · host files 15 · catalogs 22 · test regions 3 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-NOMADS-CARAVAN-CULTURE-82
wave: 7
status: PROPOSED — foreman claim required
packages: NC-82A, NC-82B, NC-82C, NC-82D, NC-82E, NC-82F, NC-82G
claim paths:
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/CaravanSaveStore.cs  # §19 candidate host surface
  - src/Host/CaravanTradeSaveStore.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/caravan_trade_routes.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/muster_faction_culture.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Culture/
  - godot --headless --path . -- --caravan-selftest
dependencies:
  - coordinate: 11 other plan(s) name these artifacts (§12)
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
