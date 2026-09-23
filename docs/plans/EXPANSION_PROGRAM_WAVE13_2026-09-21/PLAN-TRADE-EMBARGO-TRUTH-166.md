# PLAN-TRADE-EMBARGO-TRUTH-166 — Embargo Policy, Enforcement & Smuggling Pressure

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ECONOMY-LEDGER-TRUTH-96, PLAN-WARLORDS-DIPLOMACY-29, PLAN-CRIME-SYNDICATES-44.
**Implementation scaffold:** [`PLAN-TRADE-EMBARGO-TRUTH-166_APPENDIX-A_SCAFFOLD.md`](PLAN-TRADE-EMBARGO-TRUTH-166_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-ECONOMY-LEDGER-TRUTH-96` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no ledger (Plan 96 owns arithmetic), no standing model (Plan 29),
no crime organization model (Plan 44).

## 1. Outcome
`Economy/TradeEmbargoSystem.cs` (709 lines) is reachable and unaddressed. An
embargo is a **policy with enforcement**: who is cut off, what enforcement
means for caravans and markets, and how smuggling pressure responds. Without a
contract it is either a price modifier or a switch that silently stops trade.

| Deliverable | Detail |
|---|---|
| Embargo state | target (faction/settlement), issuer, scope (goods, route), and duration on the canonical clock |
| Enforcement paths | passage refusal (Plan 30/153), market exclusion (Plan 96's order rows), and inspection outcomes |
| Smuggling pressure | demand pressure routes to Plan 44's crime systems as an input; no private crime score |
| Price effects | documented as rows in Plan 96's map; no local multiplier |
| Save truth | active embargoes restore with their issuer state; expiry on the day boundary with catch-up |

## 2. Evidence
- `Assets/Ashfall.Core/Economy/TradeEmbargoSystem.cs` (709 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 96's ledger map is where price/volume effects belong.
- Plan 29 owns faction relations that issue or receive embargoes.
- Plan 44 owns crime organizations that may profit from a blockade.

## 3. Packages
- **TET-166A** embargo state + scope table.
- **TET-166B** enforcement path tests (passage/market/inspection).
- **TET-166C** smuggling-pressure input to Plan 44 (no private score proof).
- **TET-166D** price rows handed to Plan 96 + reconciliation check.
- **TET-166E** day-boundary expiry with catch-up + save round-trip.

## 4. Acceptance & verification
- An embargo blocks exactly its declared scope and nothing more (fixture per scope).
- Effects appear in Plan 96's rows; smuggling pressure appears in Plan 44's inputs.
- Save/load expiry matches a continuous run.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`.

## 5. Risks
Over-broad switches → scope table is closed and each scope has a fixture.
Hidden price magic → effects are ledger rows, verified by reconciliation.

---

## 6. Expanded census (1 files · 709 lines)

Scope: `Assets/Ashfall.Core/Economy/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `TradeEmbargoSystem.cs` | 709 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `trade_embargoes.json` | object[4 keys] |

**State surfaces:** `TradeEmbargoSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Economy/` |
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
| `PLAN-TRADE-TELL-TRUTH-248` | 1 |
| `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `TET-166A` | `TradeEmbargoSystem.cs` |
| `TET-166B` | no name match — resolve at claim time |
| `TET-166C` | no name match — resolve at claim time |
| `TET-166D` | no name match — resolve at claim time |
| `TET-166E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **3** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/EconomyHostSession.cs`, `src/Main.Economy.cs`, `src/UI/EconomyDetailPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Economy/Plan14AEconomyIntegrationTests.cs`, `Ashfall.Core.Tests/Economy/TradeEmbargoSystemTests.cs`, `Ashfall.Core.Tests/IntegrityScratchFixture.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `economy` |
| `holdfast_trade` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--data-integrity-selftest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--holdfast-trade-save-selftest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnEmbargoAdded` | `Assets/Ashfall.Core/FactionEmbargoLedger.cs` |
| `OnEmbargoRequested` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnEmbargoRequestedDetailed` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |
| `Assets/StreamingAssets/Data/faction_war_location_overrides.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (130 files, 973 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Events` | 1 | 6 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `NarrativeConsequence` | 1 | 20 |
| `Radio` | 47 | 354 |

**Verdict:** 973 cases sit under matching regions — run those first (`Audio`, `Balance`, `Combat`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **517**
(232 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BioFermentationHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **33**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `bio_fermentation` | no |
| `black_market` | no |
| `black_projects_archive` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `cupola_foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **84**
(CODEX_ONLY 27, GAMEPLAY_CONSUMED 39, OPTIONAL 5, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_honored_debt` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 33 (laddered 1) · RNG streams 15 · host files 26 · catalogs 22 · test regions 9 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TRADE-EMBARGO-TRUTH-166
wave: 13
status: PROPOSED — foreman claim required
packages: TET-166A, TET-166B, TET-166C, TET-166D, TET-166E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/caravan_trade_routes.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/economy_goods.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --data-integrity-selftest
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
