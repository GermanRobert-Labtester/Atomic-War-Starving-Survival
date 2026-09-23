# PLAN-BACKSTORY-REVEAL-TRUTH-126 — Personal Histories: What Is Known, When & From Whom

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-FAMILY-DYNASTY-43, PLAN-NARRATIVE-GRAPH-18, PLAN-CODEX-SURFACE-TRUTH-110.
**Non-goals:** no new lore authoring beyond the existing record shape, no
second biography store, no panel-invented facts.

## 1. Outcome
`Survivors/BackstorySystem.cs` is host-unreachable (Plan 1 Appendix A) and
`Narrative/` carries the content catalogs (e.g. `DwellerHeirloomCatalog`,
`NarrativeDiscoveryCatalog`). Plan 43 owns the relationship web; Plan 18 owns
authored chains. The missing contract is **revelation**: which facts about a
survivor are known to the player, to other survivors, and to the holdfast —
and how a fact moves from hidden to known without the UI inventing a summary.

| Deliverable | Detail |
|---|---|
| Fact model | backstory facts keyed to a survivor with a visibility scope (player/other survivor/holdfast) |
| Revelation sources | conversation, shared crisis, letters, heirlooms — each names the existing system that triggers it |
| Truth rule | a revealed fact renders its authored record (or key); no generated paraphrase, no placeholder |
| Conflict rule | contradictory facts either coexist with sources recorded or the catalog is reported (no silent override) |
| Persistence | revealed facts restore with the survivor section; re-loading never re-reveals or loses a fact |

## 2. Evidence
- Plan 1 Appendix A/G: `BackstorySystem` host-unreachable; candidate attach points listed there.
- `Narrative/DwellerHeirloomCatalog.cs` exists as content (re-verify per package).
- Plan 18 owns authored chains that may gate revelations.
- Plan 110 owns the codex surface where facts may be re-readable.

## 3. Packages
- **BRT-126A** fact model + visibility scopes.
- **BRT-126B** revelation hooks per source system with one test each.
- **BRT-126C** truth/conflict rule tests (authored record rendered; contradiction reported).
- **BRT-126D** persistence round-trip incl. no double-reveal.
- **BRT-126E** codex re-read surface check against Plan 110's table.

## 4. Acceptance & verification
- Every fact has exactly one visibility state per scope; no fact renders without a record.
- Same seed + same triggers → same revelation order.
- Save/load mid-progress preserves revealed set exactly.
- `bash scripts/run_test.sh` on the survivors/narrative regions.

## 5. Risks
Biography authority duplication → facts are records keyed to the survivor owner; this plan adds no parallel character model.
Generated prose → rendering is record-only; the truth test forbids paraphrase.

---

## 6. Expanded census (1 files · 364 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BackstorySystem.cs` | 364 | System | **yes** | 0 | 1 | 2 |

**Totals:** 0 banned refs · 1 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `backstory_templates.json` | object[4 keys] |

**State surfaces:** `BackstorySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 1 name references across the test tree |
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

Domain method: plan-body artifact list.
Governed artifacts: 4. Other plans referencing them: **4**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `EVIDENCE` | 2 |
| `PLAN-SILENT-FAILURE-35` | 1 |
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 1 |
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `BackstorySystem.cs` |
| `Narrative/DwellerHeirloomCatalog.cs` |
| `Survivors/BackstorySystem.cs` |
| `backstory_templates.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `BRT-126A` | no name match — resolve at claim time |
| `BRT-126B` | `BackstorySystem.cs`, `Survivors/BackstorySystem.cs` |
| `BRT-126C` | no name match — resolve at claim time |
| `BRT-126D` | no name match — resolve at claim time |
| `BRT-126E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **0** · Test files: **2** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/DwellerHeirloomCatalogTests.cs`, `Ashfall.Core.Tests/Survivors/Plan174SurvivorBackstoriesIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/backstory_templates.json` |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names)

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `survivor_fate` |
| `survivor_mental_health` |
| `survivor_relations` |
| `survivor_social` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--survivor-death-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnDwellerRetired` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnSurvivorExposed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnSurvivorFate` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnSurvivorJoined` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/disaster_templates.json` |
| `Assets/StreamingAssets/Data/documentation_templates.json` |
| `Assets/StreamingAssets/Data/dream_templates.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **7** (114 files, 903 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Communication` | 3 | 17 |
| `Excavation` | 1 | 5 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |
| `NarrativeConsequence` | 1 | 20 |
| `Shelter` | 87 | 754 |

**Verdict:** 903 cases sit under matching regions — run those first (`Audio`, `Communication`, `Excavation`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **68**
(15 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ExcavationHazardSaveStore.cs` |
| `src/Host/GenerationalSaveStore.cs` |
| `src/Host/ShelterAssignmentHostSession.cs` |
| `src/Host/ShelterAtmosphereHostSession.cs` |
| `src/Host/ShelterAtmosphereSaveStore.cs` |
| `src/Host/ShelterAtmosphereSelfTest.cs` |
| `src/Host/ShelterBarterSaveStore.cs` |
| `src/Host/ShelterDecorHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **28**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `communication` | no |
| `death_legacy` | no |
| `deep_well` | no |
| `dynamic_quests` | no |
| `excavation` | no |
| `excavation_hazards` | no |
| `expanded_shelter` | no |
| `expansion_quest` | no |
| `oral_lore` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `acoustic_detection` |
| `anomaly_hazard` |
| `deep_coast` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **44**
(CODEX_ONLY 23, GAMEPLAY_CONSUMED 9, OPTIONAL 5, UNRESOLVED 7).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `dynamic_questlines.json` | GAMEPLAY_CONSUMED |
| `environmental_atmosphere_expansion.json` | GAMEPLAY_CONSUMED |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `excavation_sites.json` | UNRESOLVED |

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 28 (laddered 0) · RNG streams 4 · host files 17 · catalogs 22 · test regions 7 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BACKSTORY-REVEAL-TRUTH-126
wave: 10
status: PROPOSED — foreman claim required
packages: BRT-126A, BRT-126B, BRT-126C, BRT-126D, BRT-126E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/AnomalyHazardSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/backstory_templates.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --survivor-death-selftest
dependencies:
  - coordinate: 4 other plan(s) name these artifacts (§12)
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
