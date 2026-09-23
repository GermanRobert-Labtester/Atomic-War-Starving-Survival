# PLAN-DREAM-SYSTEM-TRUTH-229 — Sleep Content, Nightmares & Waking Effects

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-SHELTER-CAPACITY-AUTHORITY-103, PLAN-PROCEDURAL-NARRATIVE-TRUTH-216, PLAN-MEMORY-DECAY-TRUTH-142.
**Non-goals:** no psychological model (Plan 64), no sleep quality (Plan 103), no
text generation (Plan 216).

## 1. Outcome
`Survivors/DreamSystem.cs` (**300 lines**) is reachable and unaddressed: dream
content assembled from a survivor's state and memory. Done carelessly it is
either random flavor or a hidden mood drain; done right it is a legible
consequence of what the survivor carries.

| Deliverable | Detail |
|---|---|
| Trigger model | nightmares/restful dreams derive from documented inputs (trauma Plan 64/230, memory Plan 142, shelter conditions Plan 103) |
| Content rules | dream text assembles from authored fragments via Plan 216's rules; no invented nouns |
| Waking effect | the dream's effect routes to Plan 64's model as a typed input; no private mood value |
| Frequency bounds | documented cadence with suppression after restorative care (Plan 144) |
| Save truth | pending/recent dream state restores; a load never re-rolls a dream |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/DreamSystem.cs` (300 lines; unaddressed — Wave 17 audit).
- Plans 64/103/142/216 are the adjacent owners; boundaries stated.
- Plan 144's care can suppress frequency per the documented rule.
- Plan 138 can carry waking notices.

## 3. Packages
- **DRM-229A** trigger table + input tests.
- **DRM-229B** content assembly via Plan 216 rules.
- **DRM-229C** waking-effect routing (no private mood proof).
- **DRM-229D** frequency/suppression fixtures.
- **DRM-229E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Dreams trace to declared inputs; effects appear only in Plan 64.
- Care suppresses frequency per rule; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Hidden mood drain → input/effect tables are fixtures.
Voice drift → content assembles only from authored fragments.

---

## 6. Expanded census (1 files · 300 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DreamSystem.cs` | 300 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `dream_templates.json` | object[2 keys] |

**State surfaces:** `DreamSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
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
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DRM-229A` | no name match — resolve at claim time |
| `DRM-229B` | no name match — resolve at claim time |
| `DRM-229C` | no name match — resolve at claim time |
| `DRM-229D` | no name match — resolve at claim time |
| `DRM-229E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **1** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.SleepNarrative.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Survivors/Plan177DreamSleepIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/dream_templates.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--narrative-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
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
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (66 files, 534 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Communication` | 3 | 17 |
| `Factions` | 10 | 72 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Quests` | 4 | 25 |

**Verdict:** 534 cases sit under matching regions — run those first (`Audio`, `Communication`, `Factions`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **174**
(11 of them panels/HUD).

| Host file |
|---|
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/Host/NarrativeContinuitySelfTest.cs` |
| `src/Host/NarrativeHostSession.cs` |
| `src/Host/NarrativeQuestlineHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **20**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `communication` | no |
| `crossing` | no |
| `death_legacy` | no |
| `deep_well` | no |
| `dynamic_quests` | no |
| `encounters` | no |
| `expansion_quest` | no |
| `factions` | no |
| `narrative` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **5**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `deep_coast` |
| `narrative` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **343**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 45, OPTIONAL 6, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 20 (laddered 0) · RNG streams 5 · host files 19 · catalogs 22 · test regions 8 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DREAM-SYSTEM-TRUTH-229
wave: 17
status: PROPOSED — foreman claim required
packages: DRM-229A, DRM-229B, DRM-229C, DRM-229D, DRM-229E
claim paths:
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/DeepCoastHostSession.cs  # §19 candidate host surface
  - src/Host/DeepWellHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/backstory_templates.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --narrative-selftest
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
