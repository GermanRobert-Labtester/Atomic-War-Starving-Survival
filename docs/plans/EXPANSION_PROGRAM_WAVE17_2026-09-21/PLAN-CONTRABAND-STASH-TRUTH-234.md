# PLAN-CONTRABAND-STASH-TRUTH-234 — Hidden Caches: Concealment, Discovery & Consequence

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-CRIME-SYNDICATES-44, PLAN-ECONOMY-LEDGER-TRUTH-96, PLAN-INTERNAL-SECURITY-TRUTH-224, PLAN-INVENTORY-CONSERVATION-93.
**Non-goals:** no syndicates (Plan 44), no market legality (Plan 96/166), no
security cases (Plan 224).

## 1. Outcome
`Narrative/ContrabandStashSystem.cs` (**275 lines**) is reachable and
unaddressed: hidden caches — where contraband lives, how it is found, and what
finding it means. Plan 44 owns organizations, Plan 96 the market, Plan 224
internal security; the **cache lifecycle** is unowned, so concealment is either
absolute or a random discovery.

| Deliverable | Detail |
|---|---|
| Cache model | cache sites with concealment strength, contents (inventory items via Plan 93), and a known-to set |
| Search disclosure | search actions have documented effort vs concealment resolution; discovering a cache routes through Plan 224's case system |
| Seizure/consequence | seized items move through the inventory owner; a case records the outcome and routes standing effects to Plan 29/37 |
| Decay risks | caches can be lost to weather/vermin per Plan 118-style rules (documented, visible after discovery) |
| Save truth | cache contents/known-to restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/ContrabandStashSystem.cs` (275 lines; unaddressed — Wave 17 audit).
- Plan 224's case system is the discovery consumer; Plan 93 verifies transfers.
- Plan 44's syndicates use caches — boundary stated (this plan is the cache, not the crew).
- Plan 96/166 own legality/market effects of seized goods.

## 3. Packages
- **CST-234A** cache model + concealment table.
- **CST-234B** search resolution tests (effort vs concealment).
- **CST-234C** seizure/transfers + conservation check.
- **CST-234D** loss-risk fixtures with visible post-discovery record.
- **CST-234E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Search outcomes follow the table and route discoveries to Plan 224.
- Seizures balance in inventory; loss risks are recorded.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Absolute concealment → concealment/search table is a fixture.
Random discovery → discovery routes through the case system, never a silent seize.

---

## 6. Expanded census (4 files · 819 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BunkerContrabandCatalog.cs` | 182 | Catalog | — | 0 | 0 | 0 |
| `ContrabandBrokerCaravan.cs` | 76 | Support | — | 0 | 0 | 0 |
| `ContrabandCatalogValidator.cs` | 286 | Support | — | 0 | 0 | 0 |
| `ContrabandStashSystem.cs` | 275 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `bunker_contraband_barter.json` | array[20] |

**State surfaces:** `ContrabandStashSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 7 name references across the test tree |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 4. Other plans referencing them: **1**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 4 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `CST-234A` | no name match — resolve at claim time |
| `CST-234B` | no name match — resolve at claim time |
| `CST-234C` | no name match — resolve at claim time |
| `CST-234D` | no name match — resolve at claim time |
| `CST-234E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **3** · Test files: **3** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/ContrabandSaveStore.cs`, `src/Host/ContrabandStashSelfTest.cs`, `src/Main.Plans147.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/BunkerContrabandCatalogTests.cs`, `Ashfall.Core.Tests/Narrative/ContrabandBarterRouteTests.cs`, `Ashfall.Core.Tests/Narrative/ContrabandPlan147Tests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/black_market_inventory.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **2**; isolated: **1**.

| From | → To |
|---|---|
| `ContrabandBrokerCaravan` | `BunkerContrabandCatalog` |
| `ContrabandStashSystem` | `BunkerContrabandCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan` |
| `caravan_trade_network` |
| `contraband_stash` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **4** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--caravan-selftest` |
| `--contraband-selftest` |
| `--contraband-stash-selftest` |
| `--traveling-caravan-selftest` |

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

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_bureaucratic_anomalies.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_children_folklore.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_children_folklore_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_codex.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_herbalism_pharmacology.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_glitches.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/CaravanSaveStore.cs` |
| `src/Host/CaravanTradeSaveStore.cs` |
| `src/Host/ContrabandSaveStore.cs` |
| `src/Host/ContrabandStashSelfTest.cs` |
| `src/Host/TravelingCaravanHostSession.cs` |
| `src/UI/CaravanBarterLedgerPanel.cs` |
| `src/UI/TravelingCaravanPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `caravan` | no |
| `caravan_trade_network` | no |
| `contraband_stash` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **20**
(CODEX_ONLY 18, GAMEPLAY_CONSUMED 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `narrative/bunker_blueprints_codex.json` | CODEX_ONLY |
| `narrative/bunker_bureaucratic_anomalies.json` | CODEX_ONLY |
| `narrative/bunker_children_folklore.json` | CODEX_ONLY |
| `narrative/bunker_children_folklore_batch_2.json` | CODEX_ONLY |
| `narrative/bunker_contraband_barter.json` | CODEX_ONLY |
| `narrative/bunker_court_verdicts_batch_2.json` | CODEX_ONLY |
| `narrative/bunker_court_verdicts_codex.json` | CODEX_ONLY |
| `narrative/bunker_graffiti_postings.json` | CODEX_ONLY |

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
**Surface:** save sections 3 (laddered 0) · RNG streams 0 · host files 7 · catalogs 22 · test regions 0 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CONTRABAND-STASH-TRUTH-234
wave: 17
status: PROPOSED — foreman claim required
packages: CST-234A, CST-234B, CST-234C, CST-234D, CST-234E
claim paths:
  - src/Host/CaravanSaveStore.cs  # §19 candidate host surface
  - src/Host/CaravanTradeSaveStore.cs  # §19 candidate host surface
  - src/Host/ContrabandSaveStore.cs  # §19 candidate host surface
  - src/Host/ContrabandStashSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/bunker_graffiti_postings.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/caravan_trade_routes.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --caravan-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
