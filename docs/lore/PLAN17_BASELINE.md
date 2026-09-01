# Plan 17 — Baseline Lore-Delivery Audit

**Date:** 2026-09-01
**Status:** BASELINE COMPLETE — reconnaissance finished, implementation not started.

---

## Executive Summary

ASHFALL has a substantial lore corpus — approximately **865 entries across 17 root JSON files** plus **273 narrative subfolder files** — but significant portions are **orphaned** (authored but with no runtime consumer) or **dev-only** (excellent prose locked in design documents). Plan 17's mission is to convert this latent content into player-discoverable, state-aware, provenance-classified environmental storytelling.

### Key Findings

| Category | Count | Status |
|----------|-------|--------|
| Root lore JSON files | 17 | All have `schema_version: 1` |
| Total root lore entries | ~865 | Substantial corpus |
| Narrative subfolder files | 273 | All tracked in git, all schema-versioned |
| Dev lore documents | 10 (~7,526 lines) | ~175-185 codex candidates |
| Orphaned content (no runtime consumer) | 3 sources (~204 entries) | **Critical gap** |
| Orphan item references | 11+ item IDs | **Gameplay blockers** |
| Existing lore systems | 12 | Most wired, 3 orphaned |
| Existing lore tests | ~30 tests | Archive (10), oral lore (3), narrative (260), journal (158) |
| Plan 17 implementation | 0% | No Plan17*.cs tests, no --lore-* selftest verbs |

---

## 1. Lore Systems Inventory

### 1.1 Core Systems (Assets/Ashfall.Core/)

| System | Lines | Save/Load | Host Wired | Tests | Plan 17 Role |
|--------|-------|-----------|------------|-------|--------------|
| `LocationMemorySystem` | 340 | Yes | ExpansionHostSession | Indirect | Mutation flag model (pre/after/now strata) |
| `LocationEvolutionSystem` | 133 | Yes | WorldHostSession | Indirect | Mechanical state (owner, contamination, depletion) |
| `ArchiveDeskSystem` | 196 | Yes | ArchiveDeskHostSession | 177 lines (10 tests) | Transcription pipeline |
| `ArchiveInkCatalogLoader` | 50 | No (static) | ArchiveDeskHostSession | Indirect | Ink catalog loading |
| `JournalSystem` | 323 | Yes | JournalSaveStore | 158 lines | Lore tracking core |
| `KnowledgeBase` | 129 | Yes (embedded) | via JournalSystem | Indirect | Dedup engine (ordinal-sorted keys) |
| `NarrativeEncounterSystem` | 296 | Yes | NarrativeHostSession | 260 lines | Weighted encounter selection |
| `OralLoreCatalog` | 98 | No (static) | **NOT WIRED** | 89 lines (3 tests) | **ORPHAN** — 16 songs, no host consumer |
| `DailySurvivalCatalog` | 240 | No (static) | **NOT WIRED** | 121 lines | **ORPHAN** — 4 JSON files, IFileIO violation |
| `SiteEncounterSystem` | 251 | Yes | ExpansionHostSession | 263 lines | Room-specific encounters |

### 1.2 Godot Host Systems (src/)

| System | Lines | Save/Load | Role |
|--------|-------|-----------|------|
| `JournalCodex` | 248 | No (view) | Player-facing lore browser (5 tabs: Log/Items/People/Places/Events) |
| `JournalCatalogs` | ~130 | No (static) | Codex data source (items/locations/survivors/events/verdict) |

### 1.3 System Relationships

```
LocationMemorySystem (narrative state) ──┐
                                          ├──> Location presentation
LocationEvolutionSystem (mechanical state)┘

ArchiveDeskSystem ──> JournalSystem ──> KnowledgeBase ──> JournalCodex
     (transcription)      (tracking)      (dedup)          (display)

NarrativeEncounterSystem ──> Encounter selection (weighted, filtered)

OralLoreCatalog ──> [NOT WIRED] ──> [no consumer]
DailySurvivalCatalog ──> [NOT WIRED] ──> [no consumer]
```

---

## 2. Lore Data Inventory

### 2.1 Root JSON Files (Assets/StreamingAssets/Data/)

| File | Entries | Schema | Consumer | Issues |
|------|---------|--------|----------|--------|
| `environmental_atmosphere_expansion.json` | 152 | v1 | **NONE** | **ORPHAN** — 152 atmospheric texts, no loader |
| `environmental_texts_expansion_05.json` | 36 | v1 | **NONE** | **ORPHAN** — 36 environmental texts, no loader |
| `archive_inks.json` | 3 | v1 | ArchiveInkCatalogLoader | `charcoal` item missing (2 of 3 inks uncraftable) |
| `world_history.json` | 79 | v1 | EvolvingWorldCatalog | Mixed `loc_`/`location_` prefix |
| `deep_lore_locations.json` | 10 | v1 | DeepLoreLocationCatalogLoader | `book` item missing |
| `faction_lore.json` | 23 | v1 | FactionIconCatalog/FactionStanceEngine | None |
| `deep_lore_survivor_fields.json` | 4 | v1 | SurvivorCatalog | 4 keepsake items missing |
| `holdfast_flavor.json` | 43 (3+40) | v1 | HoldfastFlavorCatalog | None |
| `journal_voice_prose.json` | 19 categories | v1 | JournalVoiceProseCatalog | None |
| `item_description_texts.json` | 184 | v1 | ItemCatalogLoader | None |
| `medical_texts.json` | 84 | v1 | MedicalWardSystem | None |
| `library_manuals.json` | 3 | v1 | LibraryStudySystem | Very small catalog |
| `epilogue_chronicle.json` | 5 | v1 | EpilogueMatrix | All placeholder art refs |
| `guilt_sources.json` | 20 | v1 | Guilt system | None |
| `confession_secrets.json` | 8 | v1 | Confession system | None |
| `final_wishes.json` | 8 | v1 | FinalWishSystem | 5 required items missing |
| `feedback_messages.json` | ~200 | v1 | UI system | Not lore (system UI) |

### 2.2 Narrative Subfolder (Assets/StreamingAssets/Data/narrative/)

**273 JSON files**, all tracked in git, all with `schema_version: 1`.

Representative samples:
- `bunker_graffiti_postings.json` — 36 entries (bunker culture)
- `cobalt_liturgies.json` — 8 entries (Cult of the Cobalt Flame)
- `bunker_children_folklore.json` — 7 entries (children's rhymes/traditions)
- `bunker_court_verdicts_codex.json` — legal proceedings
- `bunker_wiretap_transcripts.json` — surveillance logs
- `courier_dispatches_master.json` — messenger logs
- `dweller_dependency_backstories.json` — survivor backgrounds
- `engineering_logs_expansion.json` — technical records
- `eulogy_corpus_batch_1.json` — memorial texts
- 200+ more specialized narrative documents

### 2.3 Content Counts by Category

| Category | Count | Notes |
|----------|-------|-------|
| Environmental atmosphere texts | 152 | **ORPHAN** — no consumer |
| Environmental location texts | 36 | **ORPHAN** — no consumer |
| World history entries | 79 | Spans pre/post-Exchange |
| Faction lore entries | 23 | Rich ideological diversity |
| Item descriptions | 184 | Up to 14 fields per item |
| Medical condition texts | 84 | Full diagnosis→recovery pipeline |
| Archive inks | 3 | Target: 12 (Plan 17F) |
| Deep-lore locations | 10 | Exploration destinations with loot |
| Library manuals | 3 | Very small catalog |
| Oral lore songs | 16 | **ORPHAN** — no host wiring |
| Daily survival catalogs | 4 files | **ORPHAN** — no host wiring |
| Journal voice prose | 19 categories | 7-8 personality variants each |
| Holdfast flavor texts | 43 | 3 factions + 40 item descriptions |
| Guilt sources | 20 | Moral weight system |
| Confession secrets | 8 | Interpersonal drama |
| Final wishes | 8 | Multi-step quest wishes |
| Bunker graffiti postings | 36 | Plan 12B content |
| Narrative documents | 273 files | Wide variety |

---

## 3. Orphaned Content (Critical Gaps)

### 3.1 environmental_atmosphere_expansion.json — 152 entries, NO consumer

**Data shape:** `id` (prefix `atm_`), `location`, `text`, `type`, `tags[]`, `atmosphere[]`, `sense`, `time_phase`, `weather`, `author`, `condition`

**Sample entries:**
- `atm_loc_approach_thermal_plant` — location: `geothermal_plant_ruins`, type: `location_description`
- `atm_loc_arrival_flooded_subway` — location: `flooded_subway_depot`, atmosphere: `[isolation, decay, cold]`
- `atm_loc_first_sight_seed_vault` — location: `subterranean_seed_vault`, weather: `snow`

**Issue:** `ContentUtilizationScanner` maps this to `WeatherSystem`, but no code actually loads or consumes this file. **152 atmospheric texts are completely dead content.**

**Plan 17 action:** Create an `AtmosphereCatalogLoader` + `AtmosphereTextSystem` (or integrate into existing location presentation). Wire to Godot host. Add tests.

### 3.2 environmental_texts_expansion_05.json — 36 entries, NO consumer

**Data shape:** Simpler format: `id` (prefix `env_`), `location`, `text`, `type`, `tags[]`

**Sample entries:**
- `env_bunker_perimeter_sign` — location: `bunker_perimeter`, type: `warning`
- `env_scavenger_camp_note` — location: `scavenger_camp`, type: `note`
- `env_abandoned_house_diary` — location: `abandoned_house`, type: `diary`

**Issue:** Mapped to `NarrativeEncounterSystem` by scanner, but no loader exists. **36 environmental texts are dead content.**

**Plan 17 action:** Consolidate with atmosphere expansion or create separate loader. Wire to runtime.

### 3.3 OralLoreCatalog — 16 songs, NO host wiring

**Data:** `narrative/oral_lore_codex.json` + `narrative/oral_lore_batch_2.json`

**Data shape:** `lore_id`, `title`, `genre`, `tempo_bpm`, `meter`, `performance_context`, `lyrics`, `tags`

**Issue:** Core loader exists, tests exist (3 tests), but **no Godot host session** loads or presents this content. 16 musical/poetic pieces are unreachable.

**Plan 17 action:** Wire to host (radio system? archive desk? codex tab?). Add presentation layer.

### 3.4 DailySurvivalCatalog — 4 JSON files, NO host wiring + IFileIO violation

**Data:** `narrative/dweller_psychological_journals.json`, `mutated_botanical_logs.json`, `bunker_children_folklore.json`, `ration_fraud_records.json`

**Issue:** Core catalog exists, tests exist (121 lines), but **no Godot host session**. Also violates Invariant 2 (uses `System.IO.File` directly instead of `IFileIO` port).

**Plan 17 action:** Fix IFileIO violation. Wire to host. Integrate with journal/archive system.

---

## 4. Orphan Item References (Gameplay Blockers)

At least **11 item IDs** are referenced by lore/gameplay files but have no definition in any items catalog:

| Missing Item | Referenced By | Impact |
|--------------|---------------|--------|
| `charcoal` | `archive_inks.json` (2 of 3 inks) | Archive ink system uncraftable |
| `book` | `deep_lore_locations.json` loot table | Deep-lore loot unresolvable |
| `blueprint_roll` | `deep_lore_survivor_fields.json` keepsake | Survivor flavor broken |
| `radio_headset` | `deep_lore_survivor_fields.json` keepsake | Survivor flavor broken |
| `service_pistol` | `deep_lore_survivor_fields.json` keepsake | Survivor flavor broken |
| `surgical_mask` | `deep_lore_survivor_fields.json` keepsake | Survivor flavor broken |
| `scalpel` | `final_wishes.json` step requirement | Final wish uncompletable |
| `forceps` | `final_wishes.json` step requirement | Final wish uncompletable |
| `surgical_suture` | `final_wishes.json` step requirement | Final wish uncompletable |
| `dog_tags` | `final_wishes.json` step requirement | Final wish uncompletable |
| `concrete_rubble` | `final_wishes.json` step requirement | Final wish uncompletable |

**Plan 17 action:** Define missing items in `items.json` or update lore refs to existing items. Prioritize `charcoal` (blocks archive ink system).

---

## 5. Dev Lore Documents (Player-Safe Conversion Candidates)

### 5.1 Document Inventory

| File | Lines | Spoiler Risk | Player-Safe % | Codex Candidates |
|------|-------|:------------:|:-------------:|:----------------:|
| `00_OVERVIEW.md` | 143 | HIGH | ~20% | ~9 |
| `01_GAZETTEER.md` | 196 | LOW-MED | ~75% | ~9 |
| `02_THE_LIST.md` | 258 | **EXTREME** | ~10% | ~24 (gated) |
| `03_LOCATIONS.md` | 446 | MEDIUM | ~60% | ~60 |
| `04_ENCOUNTERS.md` | 317 | HIGH | ~40% | ~30 |
| `05_FACTIONS.md` | 484 | MED-HIGH | ~50% | ~22 |
| `06_REBUILDERS_AND_BLACK_OPS.md` | 448 | HIGH | ~25% | ~6 |
| `IntelBible.md` | 267 | LOW | ~60% | ~5 (+ 50 radio) |
| `ASH_FALL_CREATIVE_FRAMEWORK.md` | 2,963 | LOW-MED | ~5% | ~10-15 |
| `ASHFALL_GAME_MASTER_DOCUMENT_UPDATED.md` | 2,004 | MEDIUM | ~5% | ~0 |
| **TOTAL** | **7,526** | | | **~175-185** |

### 5.2 Highest-Value Conversion Targets

**Safest (lowest risk, highest yield):**
1. `01_GAZETTEER.md` — 75% player-safe, 9 codex entries (5 sub-regions + 4 military installations)
2. `03_LOCATIONS.md` — 60% player-safe, 60 codex entries (40 location descriptions)
3. `IntelBible.md` — 60% player-safe, 50 radio broadcasts + 5 codex fragments

**Highest spoiler risk:**
- `02_THE_LIST.md` — **EXTREME** — contains entire mystery arc (Continuity Allocation Schedule, discovery ladder, Margit Sole, Sela Renn, Day 200+ arrival). Must be day-gated and knowledge-gated. **Convert LAST.**

### 5.3 Never-Player-Facing Boundary

Across all files, dev-only content falls into clear categories:
- Code references (`hooks System_BilgePumps.cs`, line-number citations)
- Schema instructions (`Target file: locations.json`)
- Design rationale (`"That is the whole technique"`)
- Cross-file spoilers (`"See 02_THE_LIST.md"`)
- Implementation notes (DTO changes, loader warnings)
- Discovery-ladder structure (layers 1-5 with day gates)
- Spine-layer tags (`"Spine layer 2/3"`, `"Spine — critical"`)

---

## 6. Existing Test Coverage

| Test File | Tests | Coverage |
|-----------|-------|----------|
| `ArchiveDeskSystemTests.cs` | 10 | Transcription pipeline, ink consumption, cancel/refund, save/load |
| `OralLoreCatalogTests.cs` | 3 | Loads 16 entries, validates fields, queries |
| `NarrativeEncounterSystemTests.cs` | ~260 lines | Selection math, resolution, save round-trip, pending queue, determinism |
| `JournalSystemTests.cs` | ~158 lines | Discovery, dedup, tab tracking, save/load |
| `DailySurvivalCatalogTests.cs` | ~121 lines | Catalog loading, queries |
| `ExpansionHubSaveTests.cs` | ~110 lines | LocationMemorySystem save round-trip |
| `StandingRecordSystemTests.cs` | ~263 lines | SiteEncounterSystem tests |
| **Plan17*.cs tests** | **0** | **NONE — Plan 17 not implemented** |

**Selftest verbs:**
- `--evolving-world-selftest` — covers LocationEvolutionSystem, WildlifeMigrationSystem, LandmarkDegradationSystem
- `--shelter-operations-selftest` — covers ArchiveDeskSystem among other shelter systems
- **No `--lore-*`, `--environmental-*`, `--codex-*`, or `--archive-*` selftest verbs exist**

---

## 7. Gap Analysis

### 7.1 System Gaps

| Gap | Impact | Plan 17 Task |
|-----|--------|--------------|
| No atmosphere text loader/consumer | 152 texts unreachable | 17A, 17B |
| No environmental text loader | 36 texts unreachable | 17A, 17B |
| OralLoreCatalog not wired to host | 16 songs unreachable | 17T |
| DailySurvivalCatalog not wired + IFileIO violation | 4 catalogs unreachable, Invariant 2 broken | 17T |
| JournalCodex has only 4 tabs | No "Regions", "Factions", "History", "Fauna/Flora", "Technology" categories | 17M |
| No document discovery/loot system | Documents cannot be physically found in world | 17G, 17J |
| No visit-state environmental variants | Locations don't change presentation after loot/strike/restore | 17C |
| No weather-reactive atmosphere | Weather doesn't affect location text | 17D |
| Only 3 archive inks | Target is 12 | 17F |
| No world history chronology validation | Accidental date conflicts undetectable | 17K |
| No lore provenance tracking | Contradictions undetectable | 17P |
| No spoiler-gating contract | Codex could leak twists | 17O |

### 7.2 Content Gaps

| Gap | Impact | Plan 17 Task |
|-----|--------|--------------|
| 11 orphan item refs | Archive/final wishes/deep-lore broken | Fix items.json |
| Mixed `loc_`/`location_` prefix | Convention violation | 17K |
| Epilogue chronicle all placeholders | No real epilogue content | Out of scope |
| Library manuals very small (3) | Study system underpowered | Out of scope |
| No micro-stories / material evidence | Environmental storytelling lacks texture | 17E |
| No dynamic graffiti/notices | World doesn't react to events | 17R |

### 7.3 Documentation Gaps

| Missing Document | Plan 17 Task |
|------------------|--------------|
| `RUNTIME_LORE_PROVENANCE.md` | 17P |
| `LOCATION_ATMOSPHERE_COVERAGE.md` | 17A |
| `ENVIRONMENTAL_STATE_MATRIX.md` | 17C |
| `ARCHIVE_CATALOG_AUDIT.md` | 17F |
| `ARCHIVE_INK_BALANCE.md` | 17F |
| `DOCUMENT_DISCOVERY_MATRIX.md` | 17J |
| `WORLD_HISTORY_CHRONOLOGY.md` | 17K |
| `CODEX_CONVERSION_MATRIX.md` | 17L |
| `SPOILER_AND_DISCOVERY_BOUNDARIES.md` | 17O |
| `CANON_CONTRADICTION_AUDIT.md` | 17P |
| `LORE_CONTENT_UTILIZATION.md` | 17T |
| `PLAN17_REGRESSION_MATRIX.md` | 17Y |

---

## 8. Authority Map

| Question | Authoritative Owner | Notes |
|----------|---------------------|-------|
| Who owns location narrative state? | `LocationMemorySystem` | Mutation flags, three strata (pre/after/now) |
| Who owns location mechanical state? | `LocationEvolutionSystem` | Owner, contamination, depletion, threats |
| Who owns transcription? | `ArchiveDeskSystem` | Ink consumption, time cost, journal entry creation |
| Who owns codex display? | `JournalCodex` (Godot host) | 5 tabs, knowledge-gated |
| Who owns lore tracking? | `JournalSystem` | Discovery, dedup, tab tracking |
| Who owns knowledge dedup? | `KnowledgeBase` | Ordinal-sorted keys for checksum stability |
| Who owns encounter selection? | `NarrativeEncounterSystem` | Weighted, filtered by stance/danger/location |
| Who owns item lore? | `ItemCatalog` + `item_description_texts.json` | 184 items, up to 14 fields each |
| Who owns medical lore? | `MedicalWardSystem` + `medical_texts.json` | 84 conditions |
| Who owns faction lore? | `FactionIconCatalog` + `faction_lore.json` | 23 factions |
| Who owns world history? | `EvolvingWorldCatalog` + `world_history.json` | 79 entries |

---

## 9. Verification Gates (Baseline)

| Gate | Command | Baseline Status |
|------|---------|-----------------|
| Build | `dotnet build Ashfall.Core.Tests` | Baseline to be run |
| Tests | `dotnet test Ashfall.Core.Tests` | Baseline to be run |
| Data integrity | `godot --headless --path . -- --data-integrity-selftest` | Baseline to be run |
| Bridge selftest | `godot --headless --path . -- --bridge-selftest` | Baseline to be run |
| Evolving world | `godot --headless --path . -- --evolving-world-selftest` | Baseline to be run |
| Shelter operations | `godot --headless --path . -- --shelter-operations-selftest` | Baseline to be run |

---

## 10. Plan 17 Implementation Priorities

### Phase 1 — Evidence & Canon (Foundation)

1. **Fix orphan item refs** — define `charcoal`, `book`, surgical items, etc. in `items.json`
2. **Create atmosphere text loader** — wire `environmental_atmosphere_expansion.json` to runtime
3. **Create environmental text loader** — wire `environmental_texts_expansion_05.json` to runtime
4. **Wire OralLoreCatalog to host** — integrate with radio/archive/codex
5. **Fix DailySurvivalCatalog IFileIO violation** — migrate from `System.IO.File` to `IFileIO`
6. **Wire DailySurvivalCatalog to host** — integrate with journal/archive system
7. **Create location atmosphere coverage matrix** — `LOCATION_ATMOSPHERE_COVERAGE.md`
8. **Create world history chronology** — `WORLD_HISTORY_CHRONOLOGY.md`
9. **Create provenance taxonomy** — `RUNTIME_LORE_PROVENANCE.md`
10. **Create spoiler boundaries** — `SPOILER_AND_DISCOVERY_BOUNDARIES.md`

### Phase 2 — Environmental Layer (Content)

11. **Write 30+ new environmental texts** for high-exposure locations
12. **Add visit-state variants** (first visit, revisited, post-loot, post-strike, restored)
13. **Add weather-reactive variants** (8+ weather/hazard overlays)
14. **Author micro-stories** (material evidence, ordinary life)
15. **Add dynamic graffiti/notices** (event-reactive world text)

### Phase 3 — Archive Layer (Mechanics)

16. **Expand archive inks 3 → 12** — grounded material types
17. **Add 15+ discoverable documents** — diverse voices, real discovery paths
18. **Create document placement matrix** — thematic location assignment
19. **Harden transcription economy** — prevent duplicate unlocks, bound costs
20. **Integrate documents with Verdict evidence** (5+) and cipher aids (3+)
21. **Add optional archive milestones** — soft long-term purpose

### Phase 4 — Codex Layer (Presentation)

22. **Convert 40+ dev-lore entries to player-safe codex** — start with `01_GAZETTEER.md` and `03_LOCATIONS.md`
23. **Add codex categories** — Regions, Factions, History, Fauna/Flora, Technology
24. **Add 10 deep-lore location entries** — gated on discovery
25. **Wire unlock/discovery gating** — visit, faction contact, document, deep-lore site
26. **Ensure rendering/readability** — long entries, navigation, accessibility

### Phase 5 — Hardening (Quality)

27. **Dead-corpus utilization pass** — integrate or classify dev-only
28. **Canon contradiction audit** — detect accidental conflicts
29. **Localization readiness** — stable keys, pseudo-locale
30. **Save migration** — old saves retain lore/archive progress
31. **Data-integrity/continuity regression** — invalid refs fail loudly
32. **Optional landmark audio hooks** — up to 10 moments

---

## 11. Definition of Done (Plan 17)

Plan 17 is complete only when:

- [ ] Baseline lore-delivery audit exists (this document)
- [ ] Location atmosphere coverage is mapped
- [ ] Every important visitable location has a valid atmosphere path
- [ ] At least 30 new high-value environmental texts are runtime-reachable
- [ ] Important repeat locations have visit-state variants
- [ ] At least 8 weather/hazard-reactive variants exist
- [ ] Location evolution cannot produce contradictory atmosphere
- [ ] Environmental micro-stories broaden world texture beyond catastrophe
- [ ] `archive_inks.json` expands from 3 to approximately 12 valid inks
- [ ] Ink mechanics use only real runtime fields
- [ ] At least 15 new discoverable documents exist
- [ ] Every targeted document has a real discovery source
- [ ] Archive transcription consumes supported ink/time resources
- [ ] Transcription unlocks the existing JournalCodex path
- [ ] At least 5 documents integrate with Verdict evidence where compatible
- [ ] At least 3 documents integrate with cipher/decode mechanics where compatible
- [ ] `world_history.json` has a documented canonical chronology
- [ ] Accidental chronology conflicts are resolved or documented
- [ ] At least 40 player-safe codex entries are authored
- [ ] Codex categories are navigable
- [ ] At least 10 deep-lore location entries are properly gated where valid
- [ ] Developer commentary does not leak into player-facing text
- [ ] Spoiler tiers/unlock boundaries are documented
- [ ] Locked titles do not leak major spoilers
- [ ] Lore provenance is documented
- [ ] Intentional unreliable narration is distinguishable from accidental contradiction
- [ ] High-value orphan lore has been integrated or explicitly classified dev-only
- [ ] Expanded JournalCodex remains readable and accessible
- [ ] Localization/pseudo-locale checks pass where infrastructure exists
- [ ] Old saves retain lore/archive progress
- [ ] Data-integrity selftest passes
- [ ] Narrative-continuity checks pass
- [ ] `DataRuleComplianceTests` pass
- [ ] Content-utilization checks pass or intentional dev-only exclusions are documented
- [ ] Relevant builds/tests/selftests/gates pass
- [ ] No parallel lore-state authority was introduced

---

## 12. Next Steps

1. Run baseline verification gates (build, tests, data integrity)
2. Fix orphan item references (prioritize `charcoal` for archive ink system)
3. Create atmosphere text loader + consumer
4. Create environmental text loader + consumer
5. Wire OralLoreCatalog and DailySurvivalCatalog to host
6. Begin Phase 1 documentation (coverage matrix, chronology, provenance, spoiler boundaries)
7. Proceed through implementation phases in order

---

**Baseline reconnaissance complete.** Plan 17 implementation can proceed.
