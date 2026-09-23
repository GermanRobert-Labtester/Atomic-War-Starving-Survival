# PLAN-RADIO-RECORDING-TRUTH-258 — Recorded Broadcasts: Media, Archive & Replay

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RADIO-MEDIA-42, PLAN-RADIO-STATION-TRUTH-209, PLAN-HOST-EVENT-ARCHIVE-91.
**Non-goals:** no broadcast content (Plan 42), no station operations (Plan 209),
no event archive (Plan 91).

## 1. Outcome
`Radio/RadioRecordingSystem.cs` (**107 lines**) is reachable and unaddressed:
recording a broadcast (or intercept) onto media and replaying it later. It is
the bridge where an ephemeral broadcast becomes a persistent record — and where
privacy/provenance questions appear (whose voice, with whose consent).

| Deliverable | Detail |
|---|---|
| Media model | recording media as inventory items (Plan 93) with capacity and condition |
| Capture rules | only broadcasts the recorder can receive (Plan 209's reach) are capturable; consent/provenance recorded |
| Replay | replay is through the audio owner (Plan 97) and reads the stored record; no re-synthesis |
| Archive link | long-term retention of a recording may route to Plan 169's vault — boundary stated |
| Save truth | media contents and position restore; no duplication on load |

## 2. Evidence
- `Assets/Ashfall.Core/Radio/RadioRecordingSystem.cs` (107 lines; unaddressed — Wave 18 audit).
- Plan 209 supplies reach/station state; Plan 97 audio playback.
- Plan 91's archive holds facts, not audio; the boundary is stated.
- Plan 169 may store media long-term.

## 3. Packages
- **RRT-258A** media model + capacity/condition.
- **RRT-258B** capture/reach tests.
- **RRT-258C** replay path (no re-synthesis).
- **RRT-258D** provenance/consent record tests.
- **RRT-258E** save round-trip; no duplication on load.

## 4. Acceptance & verification
- Only receivable broadcasts are captured; replay reads the stored record.
- Media counts balance; save/load preserves contents.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/`.

## 5. Risks
Duplicate media → inventory is the item owner; the conservation test enforces it.
Replay synthesis → a fixture asserts replayed audio equals the stored record.

---

## 6. Expanded census (1 files · 107 lines)

Scope: `Assets/Ashfall.Core/Radio/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `RadioRecordingSystem.cs` | 107 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `conflict_mediation_records.json` | array[20] |

**State surfaces:** `RadioRecordingSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Radio/` |
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

Domain files: 1. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-RADIO-MEDIA-42` | 1 |
| `PLAN-RADIO-FAMILY-TRUTH-266` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RRT-258A` | no name match — resolve at claim time |
| `RRT-258B` | no name match — resolve at claim time |
| `RRT-258C` | no name match — resolve at claim time |
| `RRT-258D` | `RadioRecordingSystem.cs` |
| `RRT-258E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **14** · Test files: **3** · Data files: **9**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 14 | `src/Economy/TradeScreenGodotPanel.cs`, `src/Host/RadioCatalogSelfTest.cs`, `src/Host/RadioHostSession.cs`, `src/Muster/JournalWitnessPanel.cs`, `src/UI/ClandestineInsurgencyPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Integration/Plans60To63ThirtyDayIntegrationTests.cs`, `Ashfall.Core.Tests/Radio/RadioRecordingSystemTests.cs`, `Ashfall.Core.Tests/StartingLevelSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 9 | `Assets/StreamingAssets/Data/faction_radio_corpus.json`, `Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_codex.json`, `Assets/StreamingAssets/Data/narrative/council_meeting_minutes.json`, `Assets/StreamingAssets/Data/narrative/documents_batch_3.json`, `Assets/StreamingAssets/Data/narrative/education_session_records.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `faction_espionage` |
| `holdfast_trade` |
| `journal` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `starting_level` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **17** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--holdfast-trade-save-selftest` |
| `--journal-save-selftest` |
| `--journal-selftest` |
| `--journal-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **10**.

| Event | First declaration |
|---|---|
| `OnBatchCompleted` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnCodexUnlocked` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnConflictResolved` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnConflictStarted` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnExtractionBatchProduced` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnJournalTriggered` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnRadonLevelChanged` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/codex_entries.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/education_curriculum.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (132 files, 987 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Codex` | 2 | 29 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Education` | 2 | 11 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Radio` | 47 | 354 |

**Verdict:** 987 cases sit under matching regions — run those first (`Audio`, `Balance`, `Codex`, `Combat`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **477**
(230 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
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

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **38**, of which versioned-ladder sections:
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
| `caravan` | no |

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
| `combat` |
| `cupola_foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **144**
(CODEX_ONLY 78, GAMEPLAY_CONSUMED 45, OPTIONAL 8, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `codex_entries.json` | UNRESOLVED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_expelled_survivor` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 38 (laddered 1) · RNG streams 16 · host files 26 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RADIO-RECORDING-TRUTH-258
wave: 18
status: PROPOSED — foreman claim required
packages: RRT-258A, RRT-258B, RRT-258C, RRT-258D, RRT-258E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/bunker_graffiti_postings.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --expedition-panel-lifecycle
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
