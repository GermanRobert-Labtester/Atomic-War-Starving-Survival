# PLAN-DOCUMENT-DISCOVERY-TRUTH-192 — Finding, Reading & Interpreting Records

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-CODEX-SURFACE-TRUTH-110, PLAN-DISCOVERY-STATE-108, PLAN-ARCHAEOLOGY-TRUTH-152.
**Implementation scaffold:** [`PLAN-DOCUMENT-DISCOVERY-TRUTH-192_APPENDIX-A_SCAFFOLD.md`](PLAN-DOCUMENT-DISCOVERY-TRUTH-192_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-CODEX-SURFACE-TRUTH-110` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no codex rendering (Plan 110), no map knowledge state (Plan 108),
no dig lifecycle (Plan 152).

## 1. Outcome
`Narrative/BureaucraticDocumentDiscoverySystem.cs` (**531 lines**) is reachable
and unaddressed: the system that lets survivors find and read records —
Bureaucratic, technical, personal. Discovery state (Plan 108) tracks places;
the codex (Plan 110) renders entries. The **document path** itself — where a
record comes from, whether it can be read now, and what reading changes — is
unstated.

| Deliverable | Detail |
|---|---|
| Document model | records with a source (site, container, trade, inheritance), a condition, and a readability requirement (language, literacy, tool) |
| Discovery hook | finding routes through the discovery owner; reading marks the codex entry revealed (Plan 110's mechanism) |
| Partial reads | a damaged record yields partial information with a visible missing portion, never silent completion |
| Interpretation | where interpretation matters, the required skill/asset is documented and its absence is visible |
| Persistence | found/unread/read states restore; a load never re-reveals or loses a document |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/BureaucraticDocumentDiscoverySystem.cs` (531 lines; unaddressed — Wave 13/15 audit).
- Plan 110's codex table is where a read document lands.
- Plan 152's digs and Plan 108's discovery are source paths.
- Plan 43's inheritance can deliver a personal record (boundary noted).

## 3. Packages
- **DDT-192A** document model + source/condition table.
- **DDT-192B** discovery→read→codex path test.
- **DDT-192C** partial-read fixtures (damage, missing page).
- **DDT-192D** interpretation requirement table + absence-visible test.
- **DDT-192E** persistence round-trip; no re-reveal on load.

## 4. Acceptance & verification
- A read document appears in Plan 110's surface; an unread one does not.
- A damaged record shows its missing portion; completion is earned.
- Save/load preserves document states exactly.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Silent completion → partial reads are explicit states.
Overlap with 108/110 → document state is its own axis; the boundary table names both.

---

## 6. Expanded census (1 files · 531 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BureaucraticDocumentCatalog.cs` | 531 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `quests_bureaucratic_morality.json` | object[2 keys] |
| `documentation_templates.json` | object[2 keys] |
| `bunker_bureaucratic_anomalies.json` | object[3 keys] |
| `bureaucratic_documents_expansion.json` | object[4 keys] |
| `documents_batch_1.json` | object[2 keys] |
| `documents_batch_2.json` | object[3 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 1 name references across the test tree |
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
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DDT-192A` | `BureaucraticDocumentCatalog.cs` |
| `DDT-192B` | no name match — resolve at claim time |
| `DDT-192C` | no name match — resolve at claim time |
| `DDT-192D` | no name match — resolve at claim time |
| `DDT-192E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 9. Host files: **4** · Test files: **1** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Journal/JournalCatalogData.cs`, `src/Journal/JournalCodex.cs`, `src/Main.Narrative.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/BureaucraticDocumentCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/narrative/bureaucratic_documents_expansion.json`, `Assets/StreamingAssets/Data/narrative/documents_batch_2.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `dynamic_quests` |
| `journal` |
| `narrative` |
| `narrative_questlines` |
| `personal_quests` |
| `procedural_narrative` |
| `quests` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--journal-save-selftest` |
| `--journal-selftest` |
| `--journal-uitest` |
| `--journal-weather-panel-selftest` |
| `--narrative-selftest` |
| `--personal-quests-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **8**.

| Event | First declaration |
|---|---|
| `OnBatchCompleted` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnCodexUnlocked` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnExtractionBatchProduced` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnJournalTriggered` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
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
| `Assets/StreamingAssets/Data/anomalies.json` |
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/codex_entries.json` |
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **7** (54 files, 529 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Codex` | 2 | 29 |
| `Communication` | 3 | 17 |
| `Factions` | 10 | 72 |
| `Foundry` | 8 | 73 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Quests` | 4 | 25 |

**Verdict:** 529 cases sit under matching regions — run those first (`Codex`, `Communication`, `Factions`, `Foundry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **202**
(23 of them panels/HUD).

| Host file |
|---|
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/CodexHostSession.cs` |
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/ContentUtilizationRuntimeCollector.cs` |
| `src/Host/ContentUtilizationSelfTest.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **19**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `collectible_discovery` | no |
| `communication` | no |
| `crossing` | no |
| `deep_well` | no |
| `dynamic_quests` | no |
| `encounters` | no |
| `expansion_quest` | no |
| `factions` | no |
| `foundry` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `cupola_foundry` |
| `deep_coast` |
| `foundry` |
| `narrative` |
| `route_engineering_mine_flail` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **350**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 48, OPTIONAL 7, UNRESOLVED 16).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `codex_entries.json` | UNRESOLVED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** 16 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_become_warlord` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 19 (laddered 0) · RNG streams 8 · host files 21 · catalogs 22 · test regions 7 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DOCUMENT-DISCOVERY-TRUTH-192
wave: 15
status: PROPOSED — foreman claim required
packages: DDT-192A, DDT-192B, DDT-192C, DDT-192D, DDT-192E
claim paths:
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/CodexHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalies.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Codex/
  - godot --headless --path . -- --journal-save-selftest
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
