# PLAN-CRIME-SYNDICATES-44 — Underworld Networks, Smuggling, Heists & Enforcement

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-JUSTICE-LAW-37, PLAN-VERTICAL-BODY-INDUSTRY-05 (market
depth), PLAN-INPUT-HARDENING-25.
**Expanded appendix:** [`PLAN-CRIME-SYNDICATES-44_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-CRIME-SYNDICATES-44_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's underworld economy engines,
each mapped to its parent-plan mechanic row.
**Non-goals:** no real organized crime references, no graphic violence detail,
no second market/ledger authority.

---

## 1. Outcome

The black market is live but shallow: one discovery gate, three syndicates,
stock snapshots, pricing with an arbitrage floor, loans, heat decay — plus
`BlackMarketContrabandEngine`, `BlackMarketHeatAttentionEngine`,
`ChitPurityAssayEngine`, `LoanSharkEnforcerEngine` host-unreachable, and
`SafeCrackingSystem` in maritime. This plan grows the underworld into a **risk
economy** the player can work, exploit, or break.

Player loop: **make contact → move goods → run a score → pay or dodge the debt
→ manage heat → get raided or made**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Syndicates | `BlackMarketSystem`, `black_market_inventory.json` | deal, refuse, switch | trust, access tiers, prices |
| Contraband | `BlackMarketContrabandEngine`, contraband matrices | move goods, hide stash | inspection risk, confiscation |
| Smuggling | routes + caravans + crossing | route, bribe, forge papers | delivery, seizure, standing |
| Heists | `SafeCrackingSystem`, expedition sites | plan, crack, fence | loot, noise, pursuit |
| Debt | `LedgerDebtSystem`, `LoanSharkEnforcerEngine` | borrow, repay, default | collectors, collateral, labour |
| Forgery | `ChitPurityAssayEngine` | pass or assay chits | currency confidence, fraud loss |
| Heat & raids | `BlackMarketHeatAttentionEngine`, patrol hooks | lay low, relocate, pay off | raid, relocation, bounties |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core live | `MarketSystem`, `BlackMarketSystem` (19/19), `BlackMarketInventoryCatalog`, `FactionBountySystem`, `LedgerDebtSystem` (22/22), `DebtConsequenceDispatcher`, `CrossingArbitrationSystem` |
| Host-unreachable | `BlackMarketContrabandEngine`, `BlackMarketHeatAttentionEngine`, `ChitPurityAssayEngine`, `LoanSharkEnforcerEngine`, `SurvivorBarterSystem` |
| Data | `black_market_inventory.json`, `bounty_board.json`, `hardcore_economy_tuning.json`, `crossing_items.json`, contraband matrices (5 audit docs) |
| Sealed prior | Plan 211 black market (19/19 + host), Plan 40 debt templates (22/22), `DEC-02` settlement, `DEC-05` restock, `DEC-22/26` funds |
| Contracts | bounty escalation delegated to `FactionBountySystem`; arbitrage floor canonical+25% |

---

## 3. Packages

### CB-44A — Syndicates, trust and access
- Expand from 3 to 6 syndicates with distinct wants/refusals/currencies;
  trust gates access tiers and risk premiums; betrayal and switching have
  consequences through faction standing.
- **Acceptance:** prices/access explainable; no duplicate faction store; every
  stock row resolves to an item.
- **Verify:** `Plan211BlackMarketTests` extends.

### CB-44B — Contraband, stashing and smuggling
- `BlackMarketContrabandEngine` tracks contraband classes, stash locations, and
  inspection exposure; smuggling routes use caravans/crossing with bribe or
  forged-papers options.
- **Acceptance:** inspections are deterministic and avoidable; confiscation is
  itemized; stash state persists in one section.
- **Verify:** contraband focused suites.

### CB-44C — Heists and fencing
- Heist planning consumes intelligence (PLAN-ESPIONAGE-COUNTERINTEL-41) and
  sites; `SafeCrackingSystem` resolves the score; fencing uses black-market
  pricing with a heat cost.
- **Acceptance:** heists are once-per-site; failure escalates heat/bounty;
  loot canonical.
- **Verify:** maritime/safe-cracking tests + economy.

### CB-44D — Debt, enforcement and collection
- `LedgerDebtSystem` terms + `LoanSharkEnforcerEngine` collectors: collateral
  seizure, labour obligations, and renegotiation; default is a consequence
  chain, not a dead end.
- **Acceptance:** one debt ledger; collectors use existing consequence
  policies; recovery path exists.
- **Verify:** `LedgerDebtSystemTests` + `DebtConsequenceIntegrationTests`.

### CB-44E — Forgery and chit confidence
- `ChitPurityAssayEngine` models currency confidence: forgeries circulate,
  assays detect, confidence affects prices; the canonical `FundsLedger`
  remains the only balance.
- **Acceptance:** forgery is bounded and detectable; no second currency; fraud
  loss is explainable.
- **Verify:** funds + economy suites.

### CB-44F — Heat, attention and raids
- `BlackMarketHeatAttentionEngine` raises attention from volume, violence, and
  informants; raids are scheduled events with warning bands; relocation moves
  the operation at a cost.
- **Acceptance:** heat decays; raids recoverable; no unwinnable spiral.
- **Verify:** black-market host tests + justice suites.

### CB-44G — Content volumes
- +3 syndicates, +20 stock rows, +10 contraband classes, +8 heist sites, +10
  debt templates, +8 enforcement events; fictional; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Crime out-earns honest play | risk premiums + heat + fines; honest routes get treaties/contracts |
| Raids feel arbitrary | attention visible, warning bands, mitigation options |
| Ledger duplication | one funds ledger; enforcement uses canonical consequence policies |
| Tone | restrained prose; no glorified real-world criminal references |

## 5. Verification

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
bash scripts/run_test.sh Ashfall.Core.Tests/LedgerDebtSystemTests.cs
godot --headless --path . -- --economy-selftest
godot --headless --path . -- --contraband-selftest
```

---

## 6. Expanded census (6 files · 2,622 lines)

Scope: `Assets/Ashfall.Core/Economy/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BlackMarketContrabandEngine.cs` | 267 | System | **yes** | 0 | 0 | 0 |
| `BlackMarketHeatAttentionEngine.cs` | 289 | System | — | 0 | 0 | 4 |
| `BlackMarketInventoryCatalog.cs` | 358 | Catalog | — | 0 | 0 | 0 |
| `BlackMarketSettlementService.cs` | 373 | Support | — | 0 | 0 | 0 |
| `BlackMarketSystem.cs` | 893 | System | **yes** | 0 | 0 | 4 |
| `LoanSharkEnforcerEngine.cs` | 442 | System | — | 0 | 0 | 4 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `bunker_contraband_barter.json` | array[20] |

**State surfaces:** `BlackMarketHeatAttentionEngine.cs`, `BlackMarketSystem.cs`, `LoanSharkEnforcerEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Economy/` |
| Test references | 13 name references across the test tree |
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

Domain files: 7. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ECONOMY-LEDGER-TRUTH-96` | 7 |
| `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 7 |
| `PLAN-ORPHAN-SEAL-01` | 5 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 5 |
| `PLAN-CONTRACT-BOARD-109` | 5 |
| `PLAN-TRADE-TELL-TRUTH-248` | 5 |
| `EVIDENCE` | 4 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CB-44A` | no name match — resolve at claim time |
| `CB-44B` | `BlackMarketContrabandEngine.cs` |
| `CB-44C` | no name match — resolve at claim time |
| `CB-44D` | no name match — resolve at claim time |
| `CB-44E` | no name match — resolve at claim time |
| `CB-44F` | `BlackMarketHeatAttentionEngine.cs` |
| `CB-44G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **2**; isolated files:
**3**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `BlackMarketSettlementService` | `BlackMarketSystem` |
| `BlackMarketSystem` | `BlackMarketInventoryCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `BlackMarketInventoryCatalog` | 1 |
| `BlackMarketSystem` | 1 |
| `BlackMarketContrabandEngine` | 0 |
| `BlackMarketHeatAttentionEngine` | 0 |
| `BlackMarketSettlementService` | 0 |
| `LoanSharkEnforcerEngine` | 0 |

**Class split:** hub 1 · sink 1 · source 1 · isolated 3.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **3** · Test files: **8** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/BlackMarketHostSession.cs`, `src/UI/BlackMarketPanel.cs`, `src/UI/BlackMarketSnapshotFixture.cs` |
| Tests (`Ashfall.Core.Tests/`) | 8 | `Ashfall.Core.Tests/Economy/BlackMarketContrabandEngineTests.cs`, `Ashfall.Core.Tests/Economy/BlackMarketHeatAttentionEngineTests.cs`, `Ashfall.Core.Tests/Economy/ContrabandMarketIntegrationTests.cs`, `Ashfall.Core.Tests/Economy/LoanSharkEnforcerEngineTests.cs`, `Ashfall.Core.Tests/Economy/Plan211BlackMarketSettlementTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `black_market` |
| `black_projects_archive` |
| `contraband_stash` |
| `inventory` |
| `settlement_defenses` |
| `settlement_politics` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--black-flotilla-selftest` |
| `--contraband-selftest` |
| `--contraband-stash-selftest` |
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json` |
| `Assets/StreamingAssets/Data/narrative/personal_effects_inventory_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json` |
| `Assets/StreamingAssets/Data/wasteland_settlement_npcs.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (15 files, 114 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Inventory` | 15 | 114 |

**Verdict:** 114 cases sit under matching regions — run those first (`Inventory`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **18**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/BlackMarketHostSession.cs` |
| `src/Host/BlackMarketSaveStore.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/ContrabandSaveStore.cs` |
| `src/Host/ContrabandStashSelfTest.cs` |
| `src/Host/InventoryHostSession.cs` |
| `src/Host/InventorySaveSelfTest.cs` |
| `src/Host/InventorySaveStore.cs` |
| `src/Main.BlackMarket.cs` |
| `src/Main.Inventory.cs` |
| `src/Main.UiTests.Inventory.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **6**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `black_projects_archive` | no |
| `contraband_stash` | no |
| `inventory` | no |
| `settlement_defenses` | no |
| `settlement_politics` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **5**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `narrative/bunker_contraband_barter.json` | CODEX_ONLY |
| `narrative/personal_effects_inventory_batch_2.json` | CODEX_ONLY |
| `narrative/wasteland_settlement_gazetteer.json` | CODEX_ONLY |
| `wasteland_settlement_npcs.json` | UNRESOLVED |

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

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 6 (laddered 0) · RNG streams 3 · host files 15 · catalogs 11 · test regions 1 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CRIME-SYNDICATES-44
wave: —
status: PROPOSED — foreman claim required
packages: CB-44A, CB-44B, CB-44C, CB-44D, CB-44E, CB-44F, CB-44G
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Host/BlackMarketHostSession.cs  # §19 candidate host surface
  - src/Host/BlackMarketSaveStore.cs  # §19 candidate host surface
  - src/Host/BlackProjectsArchiveSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/black_flotilla_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/black_market_inventory.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/
  - godot --headless --path . -- --black-flotilla-selftest
dependencies:
  - coordinate: 7 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
