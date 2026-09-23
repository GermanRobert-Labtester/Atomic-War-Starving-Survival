# PLAN-DOC-ATLAS-CURRENCY-115 — Generated Authority Map & Superseded-Doc Detection

**Wave 9 · Kind:** GOVERNANCE · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PROGRAMME-CLOSEOUT-100, PLAN-AGENT-WORKFLOW-GOVERNANCE-59.
**Non-goals:** no production code, no doc deletion; superseded documents are
marked or archived by their owning governance process, never silently removed.

## 1. Outcome
`docs/CURRENT_AUTHORITY.md` is the domain documentation map, and the repo carries
8 expansion-programme directories plus historical rulebook archives. Doc
currency is maintained by hand today; the programme README count drift fixed in
this programme is the precedent. This plan makes the **authority map generated
and checked**: which document is current per domain, which are superseded, and
which links resolve.

| Deliverable | Detail |
|---|---|
| Map generator | scan docs for authority-marked files (headers/roles), emit a map with last-verified date and superseding links; `--check` mode |
| Supersede detection | a doc that declares a successor (or is declared superseded) is listed once, with the successor named; contradictions are reported |
| Link check | every relative link in the map resolves; broken links fail the check |
| Programme index tie-in | Plan 100's programme index is linked from the map as the plan-layer authority |
| Stale report | docs unmodified for a long window with no authority marker are listed for owner triage (not auto-retired) |

## 2. Evidence
- `docs/CURRENT_AUTHORITY.md` exists as the domain map.
- `docs/INDEX.md` is generated with `--check` (3,039 documents) — the same mechanical pattern applies here.
- `docs/archive/agent-rules/2026-09-12-pre-foreman/` is the established archive convention for superseded rules.
- Plan 100 generates the programme index this map links to; Plan 59 governs agent-facing doc rules.

## 3. Packages
- **DAC-115A** map generator + `--check`; `docs/CURRENT_AUTHORITY.md` becomes generated or map-backed (owner decision recorded).
- **DAC-115B** supersede detection with contradiction report.
- **DAC-115C** link check over map entries.
- **DAC-115D** stale-window report (list only, no action).
- **DAC-115E** cross-link to Plan 100's index + one worked example entry.

## 4. Acceptance & verification
- `--check` green; adding an authority doc without regenerating fails.
- A deliberately broken link fails with the file and target named.
- A doc with a contradictory successor declaration is reported, not resolved silently.
- `python3 scripts/ci/generate-docs-index.py --check` stays green.

## 5. Risks
Map becoming a second index → it maps authority, the docs index lists files; the cross-link states the division.
Stale list used as a delete list → report only; retirement stays with the owning governance process.

---

## 6. Expanded census (docs tree)

This plan's subject is the documentation surface itself, so the census covers
the docs tree rather than Core source.

| Metric | Value |
|---|---:|
| Root `*.md` files | 26 |
| Files under `docs/` | 2768 |
| Files under `docs/plans/` | 713 |
| Expansion-programme directories | 19 |
| Versioned generators (this programme) | 35 |
| `docs/INDEX.md` lines | 3726 |
| `docs/CURRENT_AUTHORITY.md` present | yes |

## 7. Expanded surface: what the atlas must answer

| Question | Source of truth |
|---|---|
| Which doc is current per domain | `docs/CURRENT_AUTHORITY.md` |
| Which docs are superseded | explicit successor links / archive markers |
| Which links resolve | relative-link check over map entries |
| Which plans are proposed vs claimed | programme READMEs + Plan 100's index (once built) |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Index | `python3 scripts/ci/generate-docs-index.py --check` |
| Link check | every relative link in the map resolves |
| Supersede contradictions | reported, never auto-resolved |
| Scope | map generator reads docs only; never production code |

## 9. Rollout sequence

1. Map generator over the docs tree with `--check`.
2. Supersede detection with a contradiction report.
3. Link check across map entries.
4. Stale-window report (list only, no deletion).
5. Cross-link to Plan 100's programme index.
6. Regression: `--check` plus one worked example entry.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Map entry | names exactly one current doc per domain |
| Supersede row | successor named; contradiction reported |
| Link | resolves; broken link fails the check |
| Stale report | list-only; no automatic retirement |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not create a second index.

---

## 12. Cross-plan coupling

This plan governs the documentation tree itself, so the domain proxy is the set
of document names it rules on plus the index generator.

**Governed surface (4):**

| Document |
|---|
| `docs/CURRENT_AUTHORITY.md` |
| `docs/INDEX.md` |
| `INDEX.md` |
| `generate-docs-index.py` |

Other plans referencing these documents: **53** (near-universal —
every plan cites ledger or authority docs).

**Incoming plan edges (top 8):**

| Plan | Document mentions |
|---|---:|
| `PLAN-DEBT-DRAIN-24` | 3 |
| `PLAN-DEPRECATED-TREE-RETIREMENT-94` | 2 |
| `PLAN-PROGRAMME-CLOSEOUT-100` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-LAUNCH-FACE-06` | 1 |

**Package → candidate documents (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `DAC-115A` | `CURRENT_AUTHORITY.md` |
| `DAC-115B` | no document name match — resolve at claim time |
| `DAC-115C` | no document name match — resolve at claim time |
| `DAC-115D` | no document name match — resolve at claim time |
| `DAC-115E` | `INDEX.md`, `generate-docs-index.py` |

**Reading:** this is a governance plan; its changes must land before plans that rely on the documents it renames, retires, or consolidates.

---

## 13. Authority binding map

Symbols used: 3. Host files: **8** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/UI/AquiferTreatyConcessionPanel.cs`, `src/UI/ClandestineInsurgencyPanel.cs`, `src/UI/CrossingSafeConductVouchPanel.cs`, `src/UI/InductionCupolaFurnacePanel.cs`, `src/UI/IronCenotaphMemorialPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Tooling/DocLinkValidationGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `crossing` |
| `geothermal_aquifer` |
| `memorial` |
| `regional_treaty` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--crossing-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--memorial-wall-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **10**.

| Event | First declaration |
|---|---|
| `OnSafeInspected` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |
| `OnSafeJammed` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |
| `OnSafeOpened` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |
| `OnTreatyDeliveryAccepted` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyDeliveryMissed` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyQuotaMet` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnTreatyQuotaMissed` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnTreatyStatusChanged` | `Assets/Ashfall.Core/RegionalTreatySystem.cs` |
| `OnVouchBurned` | `Assets/Ashfall.Core/VouchAccessSystem.cs` |
| `OnVouchGranted` | `Assets/Ashfall.Core/VouchAccessSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/foundry_accords.json` |
| `Assets/StreamingAssets/Data/foundry_faction.json` |
| `Assets/StreamingAssets/Data/foundry_items.json` |
| `Assets/StreamingAssets/Data/foundry_production.json` |
| `Assets/StreamingAssets/Data/foundry_treaty_consequences.json` |
| `Assets/StreamingAssets/Data/memorial_rites.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **10** (100 files, 754 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Factions` | 10 | 72 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `Lifecycle` | 1 | 5 |
| `Memorial` | 6 | 67 |
| `Production` | 3 | 12 |
| `Quests` | 4 | 25 |

**Verdict:** 754 cases sit under matching regions — run those first (`Combat`, `Economy`, `Factions`, `Foundry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **241**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/GeothermalAquiferHostSession.cs` |
| `src/Host/GeothermalAquiferSaveStore.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/MemorialSaveStore.cs` |
| `src/Host/MineClearingFlailHostSession.cs` |
| `src/Host/MineClearingFlailSaveStore.cs` |
| `src/Host/PanelBindLifecycleSelfTest.cs` |
| `src/Host/RegionalTreatyHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **22**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan_trade_network` | no |
| `crossing` | no |
| `dynamic_quests` | no |
| `economy` | no |
| `encounters` | no |
| `faction_espionage` | no |
| `factions` | no |
| `foundry` | no |
| `geothermal_aquifer` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **7**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |
| `economy` |
| `foundry` |
| `route_engineering_mine_flail` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **102**
(CODEX_ONLY 21, GAMEPLAY_CONSUMED 59, OPTIONAL 3, UNRESOLVED 19).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `cupola_foundry_catalog.json` | UNRESOLVED |

**Verdict:** 19 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_branch_iron_way_locked` |
| `flag_broke_treaty` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** governance · **Coupling (incoming plans):** 53
**Surface:** save sections 22 (laddered 1) · RNG streams 7 · host files 23 · catalogs 22 · test regions 10 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DOC-ATLAS-CURRENCY-115
wave: 9
status: PROPOSED — foreman claim required
packages: DAC-115A, DAC-115B, DAC-115C, DAC-115D, DAC-115E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/GeothermalAquiferHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/crossing_encounters.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/crossing_factions.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
  - godot --headless --path . -- --crossing-selftest
dependencies:
  - coordinate: 53 other plan(s) name these artifacts (§12)
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
| verification | **no** |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: verification.
