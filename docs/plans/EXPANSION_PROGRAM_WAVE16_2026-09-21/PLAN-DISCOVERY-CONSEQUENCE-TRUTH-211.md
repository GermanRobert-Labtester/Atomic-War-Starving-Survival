# PLAN-DISCOVERY-CONSEQUENCE-TRUTH-211 — What a Discovery Changes

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DISCOVERY-STATE-108, PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132, PLAN-CARTOGRAPHY-LANDMARKS-70.
**Non-goals:** no knowledge state machine (Plan 108), no consequence graph rules
(Plan 132), no landmark content (Plan 70).

## 1. Outcome
`Expeditions/DiscoveryConsequenceSystem.cs` (**390 lines**) is reachable and
unaddressed: discoveries (a site, a document, a cache) change the world —
unlocking routes, quests, trade, or warnings. Plan 108 owns knowledge state,
Plan 132 consequence rules for narrative. The **discovery→world-effect** map is
unowned, so a discovery is either cosmetic or unpredictable.

| Deliverable | Detail |
|---|---|
| Consequence table | discovery class → documented effects, each routed to an existing owner (routes Plan 95, quests Plan 18, economy Plan 96, hazards Plan 183) |
| One-shot rule | effects apply once per discovery; repeat visits do not re-apply |
| Player-visible summary | each effect has a notice via Plan 138; no silent world change |
| Precondition honesty | a consequence that requires an unmet state is deferred with a visible reason, never dropped |
| Save truth | applied consequences restore; a load never re-applies or loses a pending one |

## 2. Evidence
- `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` (390 lines; unaddressed — Wave 16 audit).
- Plan 108's state transitions are the trigger source.
- Plan 132's rules cover narrative consequences; this plan covers world/system effects — boundary stated.
- Plan 138 renders the notice.

## 3. Packages
- **DCT-211A** consequence table + owner routing.
- **DCT-211B** one-shot ledger + reload test.
- **DCT-211C** notice integration test.
- **DCT-211D** deferred-consequence fixture + visible reason.
- **DCT-211E** save round-trip; no re-apply on load.

## 4. Acceptance & verification
- Every discovery class has consequence rows or an explicit "no effect" decision.
- Effects appear once in their owners and produce a notice.
- Save/load preserves the applied ledger.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/`.

## 5. Risks
Silent world change → notices are mandatory per effect.
Effect duplication with 132 → world effects vs narrative rules; the table states which owner handles each row.

---

## 6. Expanded census (1 files · 390 lines)

Scope: `Assets/Ashfall.Core/Expeditions/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DiscoveryConsequenceSystem.cs` | 390 | System | **yes** | 0 | 1 | 2 |

**Totals:** 0 banned refs · 1 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `narrative_discovery_manifest.json` | array[243] |
| `discovery_consequences.json` | object[2 keys] |

**State surfaces:** `DiscoveryConsequenceSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Expeditions/` |
| Test references | 2 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-TRANSPORT-EXPEDITION-30` | 1 |
| `PLAN-SILENT-FAILURE-35` | 1 |
| `PLAN-EXPEDITION-FAMILY-TRUTH-269` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DCT-211A` | `DiscoveryConsequenceSystem.cs` |
| `DCT-211B` | no name match — resolve at claim time |
| `DCT-211C` | no name match — resolve at claim time |
| `DCT-211D` | `DiscoveryConsequenceSystem.cs` |
| `DCT-211E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/ExpeditionHostSession.cs`, `src/Main.Expeditions.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/BunkerCourtCatalogTests.cs`, `Ashfall.Core.Tests/Expeditions/DiscoveryConsequenceSystemTests.cs`, `Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `expedition` |
| `expedition_stealth` |
| `expeditions` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **9** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--narrative-selftest` |
| `--real-main-journey-selftest` |
| `--selftest-manifest` |
| `--test-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **10**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnExpeditionCompleted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionFailed` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionStarted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionTick` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/discovery_consequences.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (111 files, 907 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Expeditions` | 41 | 322 |
| `Factions` | 10 | 72 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Quests` | 4 | 25 |

**Verdict:** 907 cases sit under matching regions — run those first (`Audio`, `Expeditions`, `Factions`, `Foundry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **306**
(17 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BioFermentationHostSession.cs` |
| `src/Host/BlackMarketHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **40**, of which versioned-ladder sections:
**0**.

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

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **14**.

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
| `deep_coast` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **352**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 51, OPTIONAL 5, UNRESOLVED 17).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |

**Verdict:** 17 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_expelled_survivor` |
| `flag_honored_debt` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 40 (laddered 0) · RNG streams 14 · host files 26 · catalogs 22 · test regions 8 · flags 9

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DISCOVERY-CONSEQUENCE-TRUTH-211
wave: 16
status: PROPOSED — foreman claim required
packages: DCT-211A, DCT-211B, DCT-211C, DCT-211D, DCT-211E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --expedition-encounter-bridge-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
