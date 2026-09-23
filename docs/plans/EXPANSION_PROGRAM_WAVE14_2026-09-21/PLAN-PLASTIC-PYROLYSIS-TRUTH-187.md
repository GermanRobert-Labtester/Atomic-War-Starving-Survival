# PLAN-PLASTIC-PYROLYSIS-TRUTH-187 — Feedstock, Yields & Emission Control

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-CHEMICAL-RECON-TRUTH-183, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Non-goals:** no industry engines (Plan 45), no plume sampling (Plan 183), no
new feedstock catalog.

## 1. Outcome
`Shelter/PlasticPyrolysisSystem.cs` (**553 lines**) is reachable and
unaddressed: turning plastic waste into fuel and chemical feedstock. Pyrolysis
is a conversion process with a yield table and an emission consequence — both
unstated today, so it is either a free fuel source or an inert building.

| Deliverable | Detail |
|---|---|
| Feedstock model | accepted plastic classes with documented yields (fuel, gas, char) and byproduct handling |
| Conversion truth | mass in ≈ mass out within documented loss; a balance test over a scripted run |
| Emissions | documented emission output that feeds Plan 183's hazard state when uncontained; containment equipment consumes upkeep |
| Equipment coupling | reactor condition via Plan 119; a worn reactor changes yield, never silently |
| Save truth | in-progress batches and reactor condition restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/PlasticPyrolysisSystem.cs` (553 lines; unaddressed — Wave 13 audit).
- Plan 183's hazard model is where uncontained emissions land.
- Plan 119 supplies reactor decay; Plan 45 owns the industrial family.
- Plan 93 verifies feedstock/output counts.

## 3. Packages
- **PPT-187A** feedstock/yield table (data-validated).
- **PPT-187B** mass-balance test + deliberate-yield-error fixture.
- **PPT-187C** emission path to Plan 183 + containment upkeep.
- **PPT-187D** reactor condition effect on yield (no silent change).
- **PPT-187E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Balance closes within tolerance; an uncontained run raises hazard state.
- Reactor condition visibly changes yield; containment consumes upkeep.
- Save/load preserves batches and condition.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Free fuel → yield table + balance + byproduct handling are all tested.
Emission invisibility → hazard state is visible through Plan 183's surface.

---

## 6. Expanded census (1 files · 553 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PlasticPyrolysisSystem.cs` | 553 | System | **yes** | 0 | 0 | 4 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `plastic_pyrolysis_catalog.json` | object[8 keys] |
| `charcoal_mound_pyrolysis_logs.json` | array[8] |

**State surfaces:** `PlasticPyrolysisSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
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
| `PLAN-INDUSTRY-AUTOMATION-45` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PPT-187A` | no name match — resolve at claim time |
| `PPT-187B` | no name match — resolve at claim time |
| `PPT-187C` | no name match — resolve at claim time |
| `PPT-187D` | no name match — resolve at claim time |
| `PPT-187E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **3** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/PlasticPyrolysisHostSession.cs`, `src/Main.Plans202_205.cs`, `src/UI/PlasticPyrolysisPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Integration/Plans202To205CampaignIntegrationTests.cs`, `Ashfall.Core.Tests/Shelter/PlasticPyrolysisEngineTests.cs` |
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
| `plastic_pyrolysis` |

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
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/narrative/ammonia_chiller_leak_logs.json` |
| `Assets/StreamingAssets/Data/narrative/armored_cockroach_hive_logs.json` |
| `Assets/StreamingAssets/Data/narrative/artesian_well_contamination_logs.json` |
| `Assets/StreamingAssets/Data/narrative/bark_tanning_vat_logs.json` |
| `Assets/StreamingAssets/Data/narrative/bone_degreasing_prep_logs.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_3.json` |
| `Assets/StreamingAssets/Data/narrative/burr_millstone_dressing_logs.json` |
| `Assets/StreamingAssets/Data/narrative/carrion_vulture_sighting_logs.json` |
| `Assets/StreamingAssets/Data/narrative/cave_aquatic_biota_logs.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **6** (109 files, 754 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Flagship11` | 7 | 63 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |

**Verdict:** 754 cases sit under matching regions — run those first (`Audio`, `Campaign`, `Economy`, `Flagship11`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **514**
(233 of them panels/HUD).

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

Matched save sections: **28**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `armored_crawlers` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `bio_fermentation` | no |
| `black_market` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **12**.

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
| `cupola_foundry` |
| `disease` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **139**
(CODEX_ONLY 116, GAMEPLAY_CONSUMED 14, OPTIONAL 3, UNRESOLVED 6).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `armored_crawler_modules.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |

**Verdict:** 6 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 28 (laddered 0) · RNG streams 12 · host files 23 · catalogs 22 · test regions 6 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PLASTIC-PYROLYSIS-TRUTH-187
wave: 14
status: PROPOSED — foreman claim required
packages: PPT-187A, PPT-187B, PPT-187C, PPT-187D, PPT-187E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/audio_logs_expansion_05.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --campaign-journey-selftest
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
