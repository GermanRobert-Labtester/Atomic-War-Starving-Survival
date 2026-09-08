# Plan 135 — Baseline Reconnaissance & Content Activation Audit

## 1. Executive Summary & Status
Plan 135 establishes an evidence-grounded, non-duplicating pipeline to activate an initial roster of 60 contextual narrative records across approximately 15–20 catalogs from ASHFALL's large `CODEX_ONLY` corpus into player-discoverable world content.

### Core Guarantees Verified
1. **Source Prose Authority**: Source prose remains strictly within `Assets/StreamingAssets/Data/narrative/`. No transcript copying into gameplay JSON.
2. **Separation of Content and Discovery**: Discovered state is tracked via stable discovery identities.
3. **No Global Random Loot Dump**: Every activated record connects to a reachable, contextual game producer (location inspections, scavenging, archives/libraries, item examinations).
4. **No Inventory Spam**: Records are non-inventory knowledge discoveries unless an existing physical document mechanic specifically applies.
5. **No Second Discovery Architecture**: Reuses the canonical `JournalSystem` / `KnowledgeBase` / `JournalCodex` / `JournalSaveStore` infrastructure.

---

## 2. Verification & Utilization Baseline (Phase 5.1)

All baseline tests and gates executed cleanly:

| Verification Suite | Result | Metrics |
|---|:---:|---|
| `dotnet test Ashfall.Core.Tests` | **PASS** | 10,045 passed, 0 failed, 0 skipped |
| `dotnet build Ashfall.csproj` | **PASS** | 0 warnings, 0 errors |
| `godot --headless -- --data-integrity-selftest` | **PASS** | 0 errors, 0 warnings across 298 catalogs |
| `godot --headless -- --content-utilization-selftest` | **PASS** | CI Content Utilization Gate: PASS |
| `godot --headless -- --real-campaign-journey-selftest` | **PASS** | Clean run through Day 4 |
| `godot --headless -- --expansions-selftest` | **PASS** | ALL EXPANSIONS GREEN (01–10) |
| `python3 scripts/ci/run-gates.py --tier fast` | **PASS** | 47 / 47 gates passed |

### Content Utilization Baseline
- **Total Catalogs Audited:** 581
- **Gameplay-Consumed:** 208
- **UI-Only:** 0
- **Codex-Only:** 279 (All located in `Assets/StreamingAssets/Data/narrative/`)
- **Optional:** 24 (whitelists, documentation, future echoes)
- **Unresolved:** 70
- **Exempted:** 24

---

## 3. Narrative Schema Census (Phase 5.2)

Audit tool `generate_narrative_census.py` analyzed all 279 JSON files in `Assets/StreamingAssets/Data/narrative/`:
- **Total Files:** 279
- **Schema Version:** 100% uniform (`schema_version: 1` on all 279 files)
- **Total Authoritative Records:** 3,592 records

### Dominant Macro-Clusters:
1. **Industrial & Technical Process Logs** (144 catalogs, 1,129 records):
   - Array root: `"items"`
   - Fields: `id`, `prose`, `timestamp_relative`, `tags`, plus subsystem-specific technical parameters.
2. **Craft & Workshop Production Logs** (24 catalogs, 180 records):
   - Array root: `"items"`
   - Fields: `id`, `log_text`, plus material/tool specifications.
3. **Audits, Casebooks & Inspection Reports** (21 catalogs, 437 records):
   - Array root: `"cases"`, `"reports"`, `"audits"`, `"logs"`
   - Fields: stable ID (`case_id`, `report_id`, `audit_id`), date/day, practitioner, notes, outcome/verdict.
4. **Transcripts, Documents & Confessions** (18 catalogs, 363 records):
   - Array root: `"documents"`, `"confessions"`
   - Fields: `doc_id`, `title`, `transcript` / `body`.
5. **Specialized Domain Catalogs** (72 catalogs, 1,483 records):
   - Blueprints, court verdicts, trade ledgers, dispatches, folklore, fauna/flora field studies, medical pathology, radiation surveys.

Full breakdown recorded in `docs/content/plan135/NARRATIVE_SCHEMA_FAMILY_CENSUS.md`.

---

## 4. Existing Loader Census (Phase 5.3)

Inspection of `Assets/Ashfall.Core/Narrative/` identified 89 specialized catalog loaders and DTOs already defined in Core, including:
- `BunkerMaintenanceCatalog` (`bunker_maintenance_glitches.json`)
- `WaterTreatmentPotableCatalog` (`slow_sand_schmutzdecke_logs.json`, `ozone_contact_tower_audits.json`)
- `SteamTurbinePowerCatalog` (`turbine_blade_erosion_reports.json`, `steam_trap_water_hammer_logs.json`)
- `SeedBankPreservationCatalog` (`ragdoll_germination_assays.json`, `silica_gel_seed_desiccation_audits.json`)
- `CharcoalPyrolysisCatalog` (`retort_wood_vinegar_audits.json`)
- `PaperMakingCatalog`, `PaperPrintingCatalog`, `CordageCableCatalog`, `SoapSaponificationCatalog`, `TimberCarpentryCatalog`, `OpticsGlassworksCatalog`, `CeramicsKilnCatalog`, `CrucibleFoundryCatalog`, `MasonryBrickworksCatalog`.

Many of these loaders deserialize the catalogs during headless tests or host sessions, but their individual records lack a unified discovery graph connecting them to player actions.

---

## 5. Existing Discovery-State Census (Phase 5.4)

Analysis of existing discovery concepts across the repository:
1. **`JournalSystem` / `KnowledgeBase` (`Assets/Ashfall.Core/Journal/`)**:
   - Stores arbitrary discovery keys (`HashSet<string>`).
   - Persisted via `JournalSave` / `KnowledgeBaseSave` into `journal_save.json`.
   - Covered by `SaveWireContract` and checksum verification.
   - Already supports typed discovery keys via `KnowledgeKeys` (`room_history_seen_`, `glitch_noted_`, `wildlife_species_caught_`, `item_seen_`, `location_visited_`, `event_fired_`).
   - Zero additional persistence engine needed.
2. **`JournalCodex` (`src/Journal/JournalCodex.cs`)**:
   - Queries `JournalSystem` to render `JournalCodexRow` (with `DisplayName`, `Meta`, `Body`, `IsLocked`).
   - Already handles section extensions like `AppendRoomHistoryRows` cleanly.
3. **`CollectibleDiscoveryState` (`Assets/Ashfall.Core/CollectibleDiscoveryState.cs`)**:
   - Specifically tracks physical inventory items from `items.json`. Not suited for environmental prose.

---

## 6. Architecture Decision (Phase 6 Decision Tree)

**Selection: Case A + B (Reuse & Extend Canonical Journal Knowledge Authority)**
- The discovery authority is `JournalSystem` via `KnowledgeBase`.
- Discovery identity follows stable pattern: `narrative_disc_<discovery_id>`.
- Discovery metadata is mapped via `Assets/StreamingAssets/Data/narrative_discovery_manifest.json`.
- A lightweight engine-agnostic `NarrativeDiscoveryCatalog` in `Ashfall.Core` loads the manifest and validates:
  - Unique `discovery_id`
  - Canonical source catalog exists in `Data/narrative/`
  - Source record resolves deterministically
  - Channel and Producer ID are valid
- Discovered records can be queried by `JournalCodex` or inspected via producer interaction.
- Idempotence is guaranteed by `KnowledgeBase.Discover(key)`: first visit discovers and notifies; revisits or reloads safely return `already_discovered` with zero repeat side-effects.
