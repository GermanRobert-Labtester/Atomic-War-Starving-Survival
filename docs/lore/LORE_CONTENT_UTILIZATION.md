# Plan 17 — Lore Content Utilization

Audit of all lore content classified by runtime consumption status: gameplay consumed, codex consumed, archive consumed, dev-only, or orphaned.

## Utilization Categories

| Category | Definition | Example |
|----------|-----------|---------|
| **GAMEPLAY_CONSUMED** | Actively used by gameplay systems at runtime | Item descriptions shown in UI |
| **CODEX_CONSUMED** | Available through JournalCodex for player browsing | Location entries after visit |
| **ARCHIVE_CONSUMED** | Requires Archive Desk transcription to access | Transcribed documents |
| **EXPLORATION_CONSUMED** | Discovered through exploration/expedition | Environmental texts at locations |
| **DEV_ONLY** | Developer reference, never player-facing | Design docs, implementation notes |
| **ORPHANED** | Authored content with no runtime consumer | Dead JSON files |

## Content Utilization Matrix

### Root JSON Files (Assets/StreamingAssets/Data/)

| File | Entries | Category | Consumer | Status |
|------|---------|----------|----------|--------|
| `item_description_texts.json` | 184 | GAMEPLAY | ItemCatalog | ✅ Active |
| `medical_texts.json` | 84 | GAMEPLAY | MedicalWardSystem | ✅ Active |
| `faction_lore.json` | 23 | GAMEPLAY | FactionIconCatalog | ✅ Active |
| `world_history.json` | 79 | CODEX | EvolvingWorldCatalog | ✅ Active |
| `holdfast_flavor.json` | 43 | GAMEPLAY | HoldfastFlavorCatalog | ✅ Active |
| `journal_voice_prose.json` | 19 cats | GAMEPLAY | JournalVoiceProseCatalog | ✅ Active |
| `guilt_sources.json` | 20 | GAMEPLAY | Guilt system | ✅ Active |
| `confession_secrets.json` | 8 | GAMEPLAY | Confession system | ✅ Active |
| `final_wishes.json` | 8 | GAMEPLAY | FinalWishSystem | ✅ Active |
| `deep_lore_locations.json` | 10 | EXPLORATION | DeepLoreLocationCatalogLoader | ✅ Active |
| `deep_lore_survivor_fields.json` | 4 | GAMEPLAY | SurvivorCatalog | ✅ Active |
| `library_manuals.json` | 3 | GAMEPLAY | LibraryStudySystem | ✅ Active |
| `epilogue_chronicle.json` | 5 | GAMEPLAY | EpilogueMatrix | ⚠️ Placeholders |
| `feedback_messages.json` | ~200 | GAMEPLAY | UI system | ✅ Active (not lore) |
| `archive_inks.json` | 3 | GAMEPLAY | ArchiveInkCatalogLoader | ✅ Active |
| **`environmental_atmosphere_expansion.json`** | **152** | **ORPHAN** | **NONE** | **❌ FIXED — AtmosphereTextSystem created** |
| **`environmental_texts_expansion_05.json`** | **36** | **ORPHAN** | **NONE** | **❌ NEEDS LOADER** |

### Narrative Subfolder (Assets/StreamingAssets/Data/narrative/)

| File Category | Count | Category | Consumer | Status |
|---------------|-------|----------|----------|--------|
| Bunker culture (graffiti, postings, verdicts) | ~30 | GAMEPLAY | Various | ✅ Active |
| Faction documents (pamphlets, directives) | ~20 | EXPLORATION | Narrative systems | ✅ Active |
| Personal documents (diaries, backstories) | ~25 | ARCHIVE | Narrative systems | ✅ Active |
| Technical documents (engineering, equipment) | ~15 | ARCHIVE | Narrative systems | ✅ Active |
| Medical documents (casebooks, herbalism) | ~10 | GAMEPLAY | Medical systems | ✅ Active |
| Quest/narrative documents | ~50 | GAMEPLAY | Quest systems | ✅ Active |
| Radio/audio content | ~10 | GAMEPLAY | RadioTunerSystem | ✅ Active |
| Oral lore (songs, folklore) | ~10 | **ORPHAN** | **NONE** | **❌ OralLoreCatalog not wired** |
| Daily survival (journals, botanical) | ~4 | **ORPHAN** | **NONE** | **❌ DailySurvivalCatalog not wired** |
| Other specialized documents | ~99 | Various | Various | ✅ Active |

### Dev Lore Documents (docs/lore/)

| File | Lines | Category | Conversion Status |
|------|-------|----------|-------------------|
| `00_OVERVIEW.md` | 143 | DEV_ONLY | ~9 codex candidates (heavily redacted) |
| `01_GAZETTEER.md` | 196 | DEV_ONLY → CODEX | ~9 entries safe to convert |
| `02_THE_LIST.md` | 258 | DEV_ONLY | ~24 entries (EXTREME spoiler gating required) |
| `03_LOCATIONS.md` | 446 | DEV_ONLY → CODEX | ~60 entries (description fields player-safe) |
| `04_ENCOUNTERS.md` | 317 | DEV_ONLY → CODEX | ~30 entries (discovery-gated) |
| `05_FACTIONS.md` | 484 | DEV_ONLY → CODEX | ~22 entries (discovery-gated) |
| `06_REBUILDERS_AND_BLACK_OPS.md` | 448 | DEV_ONLY → CODEX | ~6 entries (discovery-gated) |
| `IntelBible.md` | 267 | DEV_ONLY → GAMEPLAY | 50 radio broadcasts + 5 codex fragments |
| `ASH_FALL_CREATIVE_FRAMEWORK.md` | 2,963 | DEV_ONLY | Tone reference only |
| `ASHFALL_GAME_MASTER_DOCUMENT_UPDATED.md` | 2,004 | DEV_ONLY | Developer reference only |

## Utilization Summary

| Category | Count | Percentage |
|----------|-------|:----------:|
| GAMEPLAY_CONSUMED | ~600 entries | 65% |
| CODEX_CONSUMED | ~79 entries | 9% |
| ARCHIVE_CONSUMED | ~273 files | (narrative) |
| EXPLORATION_CONSUMED | ~188 entries | 20% |
| DEV_ONLY (convertible) | ~156 candidates | — |
| DEV_ONLY (reference only) | ~5,400 lines | — |
| **ORPHANED (fixed)** | **152 entries** | **✅ AtmosphereTextSystem** |
| **ORPHANED (needs fix)** | **~56 entries** | **❌ environmental_texts + oral lore + daily survival** |

## Orphaned Content — Action Plan

### Fixed This Session

| Content | Entries | Fix Applied |
|---------|---------|-------------|
| `environmental_atmosphere_expansion.json` | 152 | ✅ AtmosphereCatalogLoader + AtmosphereTextSystem created |
| `charcoal` item (blocking archive inks) | 1 | ✅ Added to items.json |

### Remaining Orphans

| Content | Entries | Required Fix | Priority |
|---------|---------|-------------|----------|
| `environmental_texts_expansion_05.json` | 36 | Create loader + consumer | HIGH |
| OralLoreCatalog (16 songs) | 16 | Wire to host (radio/archive/codex) | MEDIUM |
| DailySurvivalCatalog (4 files) | 4 files | Fix IFileIO violation + wire to host | MEDIUM |

## Dead Content Recovery Priorities

1. **environmental_texts_expansion_05.json** — 36 location-tagged texts, simple schema, easy to wire
2. **OralLoreCatalog** — 16 songs/poems, loader exists, just needs host wiring
3. **DailySurvivalCatalog** — 4 files of diegetic content, needs IFileIO fix + host wiring

## Content That Should Stay Dev-Only

| Content | Reason |
|---------|--------|
| `ASH_FALL_CREATIVE_FRAMEWORK.md` | Writing style guide, not lore |
| `ASHFALL_GAME_MASTER_DOCUMENT_UPDATED.md` | LLM brainstorming doc, not lore |
| `00_OVERVIEW.md` (most of) | Spine thesis, mechanics discussion |
| `02_THE_LIST.md` (structure) | Discovery ladder, formula, branch outcomes |
| All code references, schema instructions | Implementation details |

## Verification

| Check | Status |
|-------|--------|
| All root JSON files have consumer | ✅ 16/17 (environmental_texts_05 needs loader) |
| Atmosphere texts wired | ✅ PASS (AtmosphereTextSystem) |
| OralLoreCatalog wired | ❌ NOT DONE |
| DailySurvivalCatalog wired | ❌ NOT DONE |
| Dev-lore conversion started | ✅ 10 docs created |
| Orphan item refs fixed | ✅ PASS (11 items added) |
