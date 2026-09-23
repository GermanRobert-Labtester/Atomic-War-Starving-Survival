# PLAN-PROCEDURAL-NARRATIVE-TRUTH-216 — Generated Text: Bounds, Provenance & Voice

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-NARRATIVE-GRAPH-18, PLAN-NARRATIVE-CONTINUITY-TRUTH-170, PLAN-ORIGINALITY-LICENSING-60.
**Non-goals:** no authored-graph change (Plan 18), no continuity rules (Plan 170),
no machine translation or model calls; any generation is data-compositional.

## 1. Outcome
`Narrative/ProceduralNarrativeSystem.cs` (**365 lines**) is reachable and
unaddressed: text assembled from authored fragments (names, places, deeds,
journal lines). Compositional text is exactly where provenance and tone break:
unattributed fragments, duplicated phrases, or out-of-voice output. Plan 60
owns originality/licensing, Plan 170 continuity.

| Deliverable | Detail |
|---|---|
| Composition rules | fragments drawn only from authored catalogs with recorded source ids; no invented nouns |
| Provenance | every assembled text records which fragment ids it used (debug/diagnostic surface, not stored per line forever) |
| Bounds | length/complexity limits per context; repetition suppresses within a window (game time) |
| Voice check | a fixture asserts assembled text uses only allowed grammatical shapes per context class |
| Save truth | assembled-and-shown texts are not re-rolled on load; pending compositions are deterministic from their inputs |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/ProceduralNarrativeSystem.cs` (365 lines; unaddressed — Wave 16 audit).
- Plan 60's originality rules apply to fragments; Plan 170 validates contradictions.
- Plan 138 can carry assembled notices; Plan 110 renders records.
- Plan 192's documents are authored; procedural text is the compositional sibling — boundary stated.

## 3. Packages
- **PNT-216A** composition rules + source-id table.
- **PNT-216B** provenance record + diagnostic surface test.
- **PNT-216C** bounds/repetition fixtures.
- **PNT-216D** voice/grammar check per context class.
- **PNT-216E** determinism: same inputs → same text; no re-roll on load.

## 4. Acceptance & verification
- No assembled text uses a non-catalog noun (fixture + scan).
- Same inputs produce identical text; repetition suppression works in game time.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Voice drift → the grammar fixture and provenance ids are the guards.
Fragment sprawl → only authored catalogs are admissible; a scan enforces it.

---

## 6. Expanded census (1 files · 365 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ProceduralNarrativeSystem.cs` | 365 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `ProceduralNarrativeSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 3 name references across the test tree |
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
| `PLAN-NARRATIVE-GRAPH-18` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PNT-216A` | no name match — resolve at claim time |
| `PNT-216B` | no name match — resolve at claim time |
| `PNT-216C` | no name match — resolve at claim time |
| `PNT-216D` | no name match — resolve at claim time |
| `PNT-216E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **8** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ProceduralNarrativeHostSession.cs`, `src/UI/EventsLogPanel.cs`, `src/UI/GameDashboardPanel.cs`, `src/UI/JournalPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Plan166_169ReloadReplayTests.cs`, `Ashfall.Core.Tests/Plan169ProceduralNarrativeTests.cs`, `Ashfall.Core.Tests/Quests/DynamicQuestGeneratorTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dynamic_quests` |
| `events` |
| `expansion_quest` |
| `journal` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **12** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--dashboard-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-save-selftest` |
| `--journal-selftest` |
| `--journal-uitest` |
| `--journal-weather-panel-selftest` |
| `--narrative-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--personal-quest-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **11**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnJournalTriggered` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |
| `OnQuestStarted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/contagion_events.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/desperation_events.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (97 files, 880 cases).

| Region | Files | Cases |
|---|---:|---:|
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |
| `Events` | 1 | 6 |
| `Factions` | 10 | 72 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Quests` | 4 | 25 |

**Verdict:** 880 cases sit under matching regions — run those first (`DutyRoster`, `Economy`, `Events`, `Factions`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **402**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **45**, of which versioned-ladder sections:
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
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan_trade_network` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **16**.

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **374**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 67, OPTIONAL 8, UNRESOLVED 20).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |

**Verdict:** 20 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_expelled_survivor` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 45 (laddered 1) · RNG streams 16 · host files 25 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PROCEDURAL-NARRATIVE-TRUTH-216
wave: 16
status: PROPOSED — foreman claim required
packages: PNT-216A, PNT-216B, PNT-216C, PNT-216D, PNT-216E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/contagion_events.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/
  - godot --headless --path . -- --dashboard-uitest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
