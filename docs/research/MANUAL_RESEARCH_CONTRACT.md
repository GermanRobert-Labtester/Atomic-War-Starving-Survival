# ASHFALL Architectural Contract: Library Manuals Study vs. Research System
**Document ID:** AF-DOC-MANUAL-RESEARCH-CONTRACT
**Authority:** Core Architectural Directive — Plans 60–63 Flagship Integration
**Status:** Canonical Reference & Enforced Policy
**Last Updated:** 2026-09-05

---

## 1. Core Mission & Invariants

In ASHFALL, knowledge is both a survival resource and an archival artifact. The shelter library provides self-study facilities for survivors to master recovered literature, but self-study has a strict, bounded relationship with shelter-wide technological breakthroughs.

### Invariant 1 — Manual Study Discovers, Never Completes Tech
- **Allowed:** Reading and completing a field manual in `LibraryStudySystem` **DISCOVERS / REVEALS** knowledge nodes in the tech tree via `ResearchSystem.UnlockManual(string id)`.
- **Forbidden:** Manual study must **NEVER** call `ResearchSystem.CompleteResearch(string id)` or force-mark a multi-day technological project as finished.
- **Rationale:** A survivor reading a manual on hydroponics or radio transmission gains understanding and identifies the schematic blueprints for the shelter. However, constructing, calibrating, and operationalizing that technology in the shelter requires the dedicated research workbench pipeline (`StartResearch`, daily labor allocation, materials, and prerequisite verification).

### Invariant 2 — Journal Evidence Provenance & Deduplication
- When a manual completes, knowledge evidence is registered into `JournalSystem` using both the raw knowledge key and the canonical provenance key:
  ```text
  manual:<manual_id>:<knowledge_id>
  ```
- Unlocking the same knowledge node across multiple manuals or repeated reads is strictly idempotent in both `ResearchSystem` and `JournalSystem` without duplicate alerts or double-grants.

---

## 2. The Six Canonical Disciplines

ASHFALL organizes all field manuals and survivor skill capabilities into six core disciplines:

| Discipline | Core Focus | Skill Mapping | Manual Examples |
|---|---|---|---|
| **Survival** | Water distillation, food preservation, soil ecology, enclosed apiculture | `survival` | `manual_water_filtration`, `manual_bunker_hydroponics`, `manual_vacuum_preservation` |
| **Engineering** | Solar inverters, radiation shielding, filter repacking, solid-state electronics | `crafting` | `manual_solar_maintenance`, `manual_radiation_shielding_fabrication`, `manual_relic_reverse_engineering` |
| **Medical** | Radioprotection, trauma debridement, containment epidemiology, pharmacology | `medical` | `manual_rad_first_aid`, `manual_field_trauma_surgery`, `manual_quarantine_epidemiology` |
| **Science** | Signal analysis, tropospheric cloud seeding, ionospheric propagation, geophones | `science` | `manual_radio_signal_direction`, `manual_cloud_seeding_meteorology`, `manual_subterranean_geophone` |
| **Scavenging** | Structural vault surveying, high-yield scrap rigging, hazmat breaching drills | `scavenging` | `manual_subterranean_cartography`, `manual_salvage_mechanics`, `manual_hazmat_breaching_drills` |
| **Combat** | Improvised arms, match ballistics, fortified chokepoints, tripwire denial | `combat` | `manual_improvised_weapons`, `manual_ballistic_handloading`, `manual_fortified_chokepoints` |

---

## 3. Skill-Adjusted Comprehension Projection

Study rate is not fixed; a survivor possessing domain expertise absorbs technical material faster. However, to prevent instant-completion exploits and maintain survival tension, the comprehension rate is strictly bounded:

### Mathematical Model
```text
rate = 1.0 + 0.6 × disciplineProgress01 + 0.4 × cachedSkillBonus
rate = Clamp(rate, 0.75, 2.00)
effectiveStudyHours = studyHoursRequired / rate
```

- **Monotonicity:** An increase in reader skill XP or active discipline perks strictly increases or maintains the study rate; it never penalizes progress.
- **Floors & Ceilings:** The slowest reader studies at `0.75x` (penalty for absolute unfamiliarity under adverse conditions); the most brilliant scholar caps at `2.00x` (twice standard speed).
- **Anti-Exploit:** Zero-hour or negative-hour manuals are rejected at start time (`ActionResult.Blocked("invalid_hours")`).

---

## 4. Bidirectional Reader Availability Reservation

To prevent physical impossibility (a survivor simultaneously performing a 12-hour airlock shift and studying in the library):
1. **Duty Roster → Library Guard:**
   `LibraryStudySystem.StartStudy` queries `DutyRosterSystem.GetRoleOf(readerId)`. If non-null, study is blocked with `ActionResult.Blocked("busy")`.
2. **Library → Duty Roster Reservation:**
   `DutyRosterSystem` exposes `IsSurvivorReservedExternally`, which checks `LibraryStudySystem.IsReaderStudying(survivorId)`. While a study job is active, duty assignments are blocked with `ActionResult.Blocked("busy")`.
3. **Release:** Cancelling a study job or completing the manual immediately releases the survivor.

---

## 5. Catalog Authority & Disposition of Catalogs

### `library_manuals.json` (Gameplay Authority)
- Location: `Assets/StreamingAssets/Data/library_manuals.json`
- Target: Contains 24 canonical manuals (4 per discipline) with runtime gameplay fields (`study_hours_required`, `fatigue_per_hour`, `morale_effect`, `skill_xp_grants`, `research_unlocks`, `prerequisites`, `requires_power`) plus structured acquisition metadata (`loot_table_ids`, `expedition_reward_ids`, `trader_pool_ids`, etc.).

### `narrative/lost_tech_manuals.json` (Codex Authority)
- Location: `Assets/StreamingAssets/Data/narrative/lost_tech_manuals.json`
- Target: Preserved for narrative lore codex entries via `LostTechManualCatalog` and tested by `LostTechManualCatalogTests`.
- Disposition: Strictly separated. `library_manuals.json` governs interactive study and tech tree progression, while `lost_tech_manuals.json` provides diegetic worldbuilding documentation.

---

## 6. Save & Persistence Guarantees
- Completed manual IDs are stored as stable strings in `LibraryStudyState.completedManualIds`.
- If a save contains an unknown or historical manual ID, it is preserved during save/load roundtrips and never discarded.
- In-progress active jobs referencing deleted or missing catalog IDs log a diagnostic warning and pause rather than corrupting the save envelope.
