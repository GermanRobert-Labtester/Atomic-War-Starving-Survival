# ASHFALL — Master Documentation Index

**Authoritative Engine:** Godot 4.7+ (.NET / C#) | **Status:** Migration Complete (Unity host removed)
**Total Indexed Documents:** 128 | **Last Verified:** 2026-08-29

| Status Badge | Meaning | Corpus Count |
|---|---|---|
| 🟢 `CURRENT` | Authoritative, active living documentation matching Godot architecture | 123 |
| 🟡 `HISTORICAL` | Forensic reports, phase logs, and historical postmortems (retained for record) | 1 |
| 🔵 `GENERATED` | Programmatically generated or updated catalogs (contracts, CLI reference, AI logs) | 4 |

---

## Duplicate & Near-Duplicate Audit Generations

The following documents share identical or near-identical filenames across root, `docs/`, and `deprecated_audits/`. Use the canonical location listed below:

| Filename | Copies / Locations | Canonical Location | Notes |
|---|---|---|---|
| `manifest.md` | `assets/sprites/AI_Generated/manifest.md`<br>`tools/asset_migration/legacy_tooling/AI_Generated/manifest.md` | `assets/sprites/AI_Generated/manifest.md` | Root vs docs mirror |
| `ASSET_GALLERY.md` | `docs/ui/ASSET_GALLERY.md`<br>`docs/visual/ASSET_GALLERY.md` | `docs/ui/ASSET_GALLERY.md` | Root vs docs mirror |

---

## AI Agent Fast Map (Subsystem Routing)

| Subsystem / Domain | Authoritative Specification | Key C# Location | Verification Verb |
|---|---|---|---|
| **Core Directives & Invariants** | [`AGENTS.md`](../AGENTS.md) | `Assets/Ashfall.Core/` | `dotnet test` |
| **Data Authority & Catalogs** | [`docs/data/CATALOG_REGISTRY.md`](data/CATALOG_REGISTRY.md) | `Assets/StreamingAssets/Data/` | `--data-integrity-selftest` |
| **Save Architecture & Envelopes** | [`docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`](saves/SAVE_STORE_CONTRACT_MATRIX.md) | `Assets/Ashfall.Core/Save/` | `--save-store-checksum-selftest` |
| **Host CLI & Self-Test Verbs** | [`docs/cli/HOST_CLI_COMMAND_CATALOG.md`](cli/HOST_CLI_COMMAND_CATALOG.md) | `src/Host/HostCli*.cs` | `python3 scripts/ci/run-gates.py --list` |
| **UI Panels, Theming & Modals** | [`docs/ui/README_UI_SYSTEM.md`](ui/README_UI_SYSTEM.md) | `src/UI/`, `assets/ui/` | `--scene-binding-selftest` |
| **Expansions 01–10 Specs** | [`docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md`](ASHFALL_EXPANSION_CONTEXT_ATLAS.md) | `Assets/Ashfall.Core/Expansions/` | `--expansions-completeness-selftest` |
| **Audio Pipeline & Cues** | [`docs/systems/AUDIO_SYSTEM.md`](systems/AUDIO_SYSTEM.md) | `src/Audio/` | `ashfall-audio-qa` |
| **CI Gates & Fast-Tier Runner** | [`docs/CI.md`](CI.md) | `scripts/ci/` | `python3 scripts/ci/run-gates.py` |

---

## 1. Living System Architecture & Governance (7 documents)

| Status | Document | Title / Summary |
|---|---|---|
| 🟢 `CURRENT` | [`AGENTS.md`](../AGENTS.md) | **PROJECT: ASHFALL (working title) — 2D Atomic-War Survival** — Original 2D survival-management game set after a nuclear exchange. Inspired by the survival-management genre; do **no... |
| 🟢 `CURRENT` | [`README.md`](../README.md) | **ASHFALL: Atomic War – Starving Survival** — 2D post-nuclear survival-management game. Godot 4.7 .NET (C#) is the only |
| 🟢 `CURRENT` | [`docs/architecture/ARCHITECTURE_TEST_MAP.md`](architecture/ARCHITECTURE_TEST_MAP.md) | **ASHFALL — Evidence-Derived Architecture & Verification Graph** — **Last Verified:** 2026-08-29<br> |
| 🟢 `CURRENT` | [`docs/architecture/CAMPAIGN_CALENDAR_AUTHORITY.md`](architecture/CAMPAIGN_CALENDAR_AUTHORITY.md) | **ASHFALL Campaign Calendar Authority & Time Invariants** — This document specifies the authoritative campaign calendar hierarchy, time domains, clock projections, and reconcili... |
| 🟢 `CURRENT` | [`docs/architecture/CORE_SYSTEMS_CATALOG.md`](architecture/CORE_SYSTEMS_CATALOG.md) | **ASHFALL Core Domain Subsystems & Host Seams Catalog** — **Authoritative Architecture Map** \| **Generated:** 2026-08-29 \| **Systems Documented:** 34 |
| 🟢 `CURRENT` | [`docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md`](architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md) | **ASHFALL — Triad Drift Gate & Subsystem Save Ownership** — **Date:** 2026-08-27 |
| 🟢 `CURRENT` | [`sources.md`](../sources.md) | **Atomic War: Starving Survival — Comprehensive Codebase Exploration Report** — **Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival` |

## 2. CI, Fast-Tier Gates & Verification (2 documents)

| Status | Document | Title / Summary |
|---|---|---|
| 🟢 `CURRENT` | [`docs/CI.md`](CI.md) | **ASHFALL — Continuous Integration & Verification Guide** — **Authoritative host/engine:** Godot 4.7+ (.NET / C#) (`project.godot`) |
| 🟢 `CURRENT` | [`docs/ci/GATING_VS_DIAGNOSTIC_CHECKS.md`](ci/GATING_VS_DIAGNOSTIC_CHECKS.md) | **ASHFALL — Verification Gates vs. Diagnostic-Only Checks** — **Date:** 2026-08-26 |

## 3. Save Systems & State Architecture (4 documents)

| Status | Document | Title / Summary |
|---|---|---|
| 🟢 `CURRENT` | [`docs/saves/SAVE_FUZZ_REPORT.md`](saves/SAVE_FUZZ_REPORT.md) | **ASHFALL Save System Fuzz Audit — Phase 1 (Persistence Surface)** — **Skill:** ashfall-save-fuzz · **Mode:** read-only surface map |
| 🟢 `CURRENT` | [`docs/saves/battery/ALL_BATTERY.md`](saves/battery/ALL_BATTERY.md) | **ASHFALL Save System Fuzz — Phase 2 (Round-Trip Battery)** — **Skill:** ashfall-save-fuzz · **Mode:** round-trip battery |
| 🟢 `CURRENT` | [`docs/saves/battery/EXPEDITION_BATTERY.md`](saves/battery/EXPEDITION_BATTERY.md) | **ASHFALL Save System Fuzz — Phase 2 (Round-Trip Battery)** — **Skill:** ashfall-save-fuzz · **Mode:** round-trip battery |
| 🔵 `GENERATED` | [`docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`](saves/SAVE_STORE_CONTRACT_MATRIX.md) | **ASHFALL — Save-Store Contract Matrix & Completeness Authority** — **Last Verified:** 2026-08-29<br> |

## 4. Expansions (01–10 Master Plans & Context) (25 documents)

| Status | Document | Title / Summary |
|---|---|---|
| 🟢 `CURRENT` | [`docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md`](ASHFALL_EXPANSION_CONTEXT_ATLAS.md) | **ASHFALL: DEEP EXPANSION CONTEXT & INTEGRATION ATLAS** — **Authoritative Architectural Blueprint, Connective Seams, Temporal Dynamics & Creative Design Context** |
| 🟢 `CURRENT` | [`docs/expansions/DEEP_LORE_MASTER_PLAN.md`](expansions/DEEP_LORE_MASTER_PLAN.md) | **ASHFALL — DEEP LORE & CHARACTER PROGRESSION: IMPLEMENTATION PLAN** — This expansion is **primarily data, not code**. The project already has all the narrative plumbing: |
| 🟢 `CURRENT` | [`docs/expansions/EXPANSIONS_MASTER_CATALOG.md`](expansions/EXPANSIONS_MASTER_CATALOG.md) | **ASHFALL Expansions 01–11 Master Systems & Integration Atlas** — **Authoritative Expansion Catalog** \| **Generated:** 2026-08-29 \| **Total Expansions:** 11 |
| 🟢 `CURRENT` | [`docs/expansions/EXPANSION_3_4_MASTER_PLAN.md`](expansions/EXPANSION_3_4_MASTER_PLAN.md) | **ASHFALL — EXPANSION 3 & 4: COMPREHENSIVE IMPLEMENTATION PLAN** — **Goal**: Add dynamic condition, contamination, and purity to all scavenged items. |
| 🟢 `CURRENT` | [`docs/expansions/expansion_02_the_duty_roster_creative_pack.md`](expansions/expansion_02_the_duty_roster_creative_pack.md) | **ASHFALL: THE DUTY ROSTER — Creative Pack** — **Internal id:** `expansion_the_duty_roster` |
| 🟢 `CURRENT` | [`docs/expansions/expansion_02_the_duty_roster_plan.md`](expansions/expansion_02_the_duty_roster_plan.md) | **ASHFALL — Expansion Design Bible** — **Title:** ASHFALL: THE DUTY ROSTER |
| 🟢 `CURRENT` | [`docs/expansions/expansion_03_nobodys_charter_INTEGRATION_PIPELINE.md`](expansions/expansion_03_nobodys_charter_INTEGRATION_PIPELINE.md) | **ASHFALL: NOBODY'S CHARTER — Integration & Architectural Pipeline** — Nobody's Charter is integrated exactly like the two sister packs, because it must read their flags (Appendix A of the... |
| 🟢 `CURRENT` | [`docs/expansions/expansion_03_nobodys_charter_plan.md`](expansions/expansion_03_nobodys_charter_plan.md) | **ASHFALL — Expansion Design Bible** — **Title:** ASHFALL: NOBODY'S CHARTER |
| 🟢 `CURRENT` | [`docs/expansions/expansion_03_the_standing_record_creative_pack.md`](expansions/expansion_03_the_standing_record_creative_pack.md) | **ASHFALL: THE STANDING RECORD — Creative Pack** — **Internal id:** `expansion_the_standing_record` |
| 🟢 `CURRENT` | [`docs/expansions/expansion_03_the_standing_record_plan.md`](expansions/expansion_03_the_standing_record_plan.md) | **ASHFALL — Expansion Design Bible** — **Title:** ASHFALL: THE STANDING RECORD |
| 🟢 `CURRENT` | [`docs/expansions/expansion_04_nobodys_charter_plan.md`](expansions/expansion_04_nobodys_charter_plan.md) | **ASHFALL — Expansion Design Bible** — **Title:** ASHFALL: NOBODY'S CHARTER |
| 🟢 `CURRENT` | [`docs/expansions/expansion_05_the_year_of_ash_creative_pack.md`](expansions/expansion_05_the_year_of_ash_creative_pack.md) | **ASHFALL: THE YEAR OF ASH (DAYS 180–360) — Grand Lore Bible & Master Creative Pack** — **Internal id:** `expansion_05_the_year_of_ash` |
| 🟢 `CURRENT` | [`docs/expansions/expansion_05_the_year_of_ash_plan.md`](expansions/expansion_05_the_year_of_ash_plan.md) | **ASHFALL — Master Expansion Design Bible & 10-Faction Strategic Integration Plan** — **Title:** ASHFALL: THE YEAR OF ASH (THE LONG WINTER & THE FINAL RECKONING) |
| 🟢 `CURRENT` | [`docs/expansions/expansion_06_the_muster_plan.md`](expansions/expansion_06_the_muster_plan.md) | **ASHFALL — Expansion Design Bible & Godot-Native Integration Plan** — **Title:** ASHFALL: THE MUSTER (THE FIFTEENTH CURRENT & THE VERGE RISING) |
| 🟢 `CURRENT` | [`docs/expansions/expansion_07_the_dose_IMPLEMENTATION.md`](expansions/expansion_07_the_dose_IMPLEMENTATION.md) | **ASHFALL: THE DOSE — Implementation Approach & Player Experience** — **Companion to `docs/expansions/expansion_07_the_dose_plan.md`.** This document is the build |
| 🟢 `CURRENT` | [`docs/expansions/expansion_07_the_dose_plan.md`](expansions/expansion_07_the_dose_plan.md) | **ASHFALL — Expansion Design Bible** — **Title:** ASHFALL: THE DOSE |
| 🟢 `CURRENT` | [`docs/expansions/expansion_08_the_verdict_creative_pack.md`](expansions/expansion_08_the_verdict_creative_pack.md) | **ASHFALL: THE VERDICT — MASTER CREATIVE PACK** — **Internal id:** `expansion_08_the_verdict` |
| 🟢 `CURRENT` | [`docs/expansions/expansion_08_the_verdict_plan.md`](expansions/expansion_08_the_verdict_plan.md) | **ASHFALL — Expansion Design Bible & Creative Pipeline Spec** — **Title:** ASHFALL: THE VERDICT (THE MACHINE THAT KEEPS THE COUNT) |
| 🟢 `CURRENT` | [`docs/expansions/expansion_08_verdict_INTEGRATION_MATRIX.md`](expansions/expansion_08_verdict_INTEGRATION_MATRIX.md) | **ASHFALL — Expansion 08 (The Verdict) Integration Matrix** — **Audit date:** Exchange + (this pass). **Architecture source:** live `Ashfall.Core` + `src/` reads. |
| 🟢 `CURRENT` | [`docs/expansions/expansion_09_the_black_flotilla_plan.md`](expansions/expansion_09_the_black_flotilla_plan.md) | **ASHFALL — Expansion 09: The Black Flotilla** — **Expansion 09** is the maritime expansion: coastal wreck salvage, 4-room stealth dive |
| 🟢 `CURRENT` | [`docs/expansions/expansion_10_the_silent_foundry_PHASE0.md`](expansions/expansion_10_the_silent_foundry_PHASE0.md) | **Expansion 10 — The Silent Foundry — Phase 0 Preflight & Dependency Map** — Status: implemented (Core system + host + save + trade surfaces). Re-anchored to |
| 🟢 `CURRENT` | [`docs/expansions/expansion_11_the_long_line_creative_pack.md`](expansions/expansion_11_the_long_line_creative_pack.md) | **ASHFALL — Expansion Proposal 11: THE LONG LINE** — **Proposed internal id:** `expansion_11_the_long_line` |
| 🟢 `CURRENT` | [`docs/expansions/expansion_the_holdfast_creative_pack.md`](expansions/expansion_the_holdfast_creative_pack.md) | **ASHFALL: THE HOLDFAST — Creative Pack** — **Internal id:** `expansion_the_holdfast` |
| 🟢 `CURRENT` | [`docs/expansions/expansion_the_holdfast_plan.md`](expansions/expansion_the_holdfast_plan.md) | **ASHFALL — Expansion Design Bible** — **Title:** ASHFALL: THE HOLDFAST |
| 🔵 `GENERATED` | [`prompt_assets/ASHFALL_PROMPT_CATALOG_EXPANSION.md`](../prompt_assets/ASHFALL_PROMPT_CATALOG_EXPANSION.md) | **ASHFALL — Prompt Catalog Expansion** — This file is a **continuation**, not a replacement. Two prompt libraries already exist and remain valid: |

## 5. UI, UX & Visual Systems (31 documents)

| Status | Document | Title / Summary |
|---|---|---|
| 🟢 `CURRENT` | [`docs/lore/06_REBUILDERS_AND_BLACK_OPS.md`](lore/06_REBUILDERS_AND_BLACK_OPS.md) | **The Two Unwritten Factions** — Everything in `03_LOCATIONS.md`, `04_ENCOUNTERS.md` and `05_FACTIONS.md` is |
| 🟢 `CURRENT` | [`docs/narrative/CONTINUITY_REPORT.md`](narrative/CONTINUITY_REPORT.md) | **Narrative Continuity Report — Full Corpus Audit (25 Creative-Writing Batches)** — **Scope:** Cross-reference and contradiction audit across the full narrative data-authority corpus (`Assets/Streaming... |
| 🟢 `CURRENT` | [`docs/ui/ASSET_GALLERY.md`](ui/ASSET_GALLERY.md) | **ASSET GALLERY** — This gallery lists all UI snapshots for ASHFALL: Atomic War - Starving Survival. |
| 🟢 `CURRENT` | [`docs/ui/DESIGN_SYSTEM_RULES.md`](ui/DESIGN_SYSTEM_RULES.md) | **Ashfall Design System Rules & Production Specification** — - **Theme**: Grim 2D survival-management (This War of Mine / Sheltered inspired). |
| 🟢 `CURRENT` | [`docs/ui/FACTION_VOICE_MATRIX.md`](ui/FACTION_VOICE_MATRIX.md) | **Faction Radio Voice Matrix — Ashfall Canon** — The radio is the auditory heartbeat of the Ashfall wasteland. Across 12 distinct frequencies, each faction possesses ... |
| 🟢 `CURRENT` | [`docs/ui/JOURNAL_UI_PLAN.md`](ui/JOURNAL_UI_PLAN.md) | **ASHFALL — Journal UI Plan** — Turn the existing diegetic journal (playthrough log + tutorial pages) into the |
| 🟢 `CURRENT` | [`docs/ui/PHASE13_DATA_AVAILABILITY.md`](ui/PHASE13_DATA_AVAILABILITY.md) | **Phase 13 — Data Availability Report** — **Date:** this turn. |
| 🟢 `CURRENT` | [`docs/ui/PIPELINE_REGRESSION_FIX.md`](ui/PIPELINE_REGRESSION_FIX.md) | **Pipeline Regression Fix (Phase 26 close)** — During Phase 26 close, an on-disk byte-level integrity check revealed a regression |
| 🟢 `CURRENT` | [`docs/ui/RADIO_HUD_CONCEPTS.md`](ui/RADIO_HUD_CONCEPTS.md) | **Radio HUD Concept Exploration & Scorecard** — - **Visual Structure**: Stamped steel 19" rack bezel with brass screws and knurled aluminium tuning dials. |
| 🟢 `CURRENT` | [`docs/ui/README_UI_SYSTEM.md`](ui/README_UI_SYSTEM.md) | **ASHFALL UI System - Complete Summary** — Comprehensive UI system for ASHFALL: Atomic War - Starving Survival with AI-generated backgrounds, smooth animations,... |
| 🟢 `CURRENT` | [`docs/ui/SNAPSHOT_COVERAGE.md`](ui/SNAPSHOT_COVERAGE.md) | **Snapshot Coverage — Post-Audit State** — **Generated:** Phase 26 close (2026-08-18). Refreshed after `SURFACE_GAP_REPORT.md` audit; reconciled post-Phase 28. |
| 🟢 `CURRENT` | [`docs/ui/SNAPSHOT_FIXTURE_POLICY.md`](ui/SNAPSHOT_FIXTURE_POLICY.md) | **ASHFALL — Snapshot Fixture Policy** — **Date:** this turn (Phase 14). |
| 🟢 `CURRENT` | [`docs/ui/SNAPSHOT_MANIFEST_CONSISTENCY_AUDIT_2026-08-26.md`](ui/SNAPSHOT_MANIFEST_CONSISTENCY_AUDIT_2026-08-26.md) | **Snapshot Manifest Consistency Audit — 2026-08-26** — **Date:** 2026-08-26 |
| 🟢 `CURRENT` | [`docs/ui/SNAPSHOT_REGEN_APPROVAL_2026-08-26.md`](ui/SNAPSHOT_REGEN_APPROVAL_2026-08-26.md) | **Snapshot Golden Regeneration — Approval Request (2026-08-26)** — **Pipeline:** implemented in `961df334` (`--ui-snapshot-uitest` diff gate + `--ui-snapshot-regenerate`) |
| 🟢 `CURRENT` | [`docs/ui/SURFACE_GAP_REPORT.md`](ui/SURFACE_GAP_REPORT.md) | **Surface Gap Report — Audit of every non-COVERED runtime UI surface** — **Generated:** Phase 26 close (2026-08-18) |
| 🟢 `CURRENT` | [`docs/ui/TIER3_UI_READINESS.md`](ui/TIER3_UI_READINESS.md) | **ASHFALL — Tier-3 UI Readiness Map** — **Date:** Reconciled post-Phase 28 (Historical Phase 14 baseline). |
| 🟢 `CURRENT` | [`docs/ui/UI_CORRECTION_REPORT.md`](ui/UI_CORRECTION_REPORT.md) | **ASHFALL — UI CORRECTION PASS REPORT** — **Date:** 2026-08-15 |
| 🟢 `CURRENT` | [`docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`](ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md) | **Contributor Guide — UI Node Diagnostics, Lifecycle & Leak Triage** — **Date:** 2026-08-27 |
| 🟢 `CURRENT` | [`docs/ui/UI_PANELS_MASTER_VOLUME_2.md`](ui/UI_PANELS_MASTER_VOLUME_2.md) | **ASHFALL: Atomic War - Starving Survival** — ═══════════════════════════════════════════════════════════════════════════════════════════ |
| 🟢 `CURRENT` | [`docs/ui/UI_PANELS_MASTER_VOLUME_3.md`](ui/UI_PANELS_MASTER_VOLUME_3.md) | **ASHFALL: Atomic War - Starving Survival** — ═══════════════════════════════════════════════════════════════════════════════════════════ |
| 🟢 `CURRENT` | [`docs/ui/UI_PANELS_MASTER_VOLUME_4.md`](ui/UI_PANELS_MASTER_VOLUME_4.md) | **ASHFALL: Atomic War - Starving Survival** — ═══════════════════════════════════════════════════════════════════════════════════════════ |
| 🟢 `CURRENT` | [`docs/ui/UI_PANELS_MASTER_VOLUME_5.md`](ui/UI_PANELS_MASTER_VOLUME_5.md) | **ASHFALL: Atomic War - Starving Survival** — ═══════════════════════════════════════════════════════════════════════════════════════════ |
| 🟢 `CURRENT` | [`docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md`](ui/UI_PANEL_ARCHITECTURE_GUIDE.md) | **ASHFALL Godot UI Panel Architecture & Node Binding Guide** — **Authoritative UI Contract Guide** \| **Generated:** 2026-08-29 \| **Scene-Backed Panels:** 22 |
| 🟢 `CURRENT` | [`docs/ui/UI_VISUAL_TEXT_SPEC.md`](ui/UI_VISUAL_TEXT_SPEC.md) | **ASHFALL — UI Visual Information (text spec)** — All authored in house voice: cold, exhausted, human, restrained; specificity |
| 🟢 `CURRENT` | [`docs/visual/ART_FAMILY_REFERENCE_GUIDE.md`](visual/ART_FAMILY_REFERENCE_GUIDE.md) | **ASHFALL — Art Family Reference Guide** — **Date:** Phase 16. |
| 🟢 `CURRENT` | [`docs/visual/ASSET_COVERAGE_REPORT_2026-08-26.md`](visual/ASSET_COVERAGE_REPORT_2026-08-26.md) | **ASHFALL Visual Asset Coverage Report** — 1. **Items (91.1% Coverage)**: |
| 🟢 `CURRENT` | [`docs/visual/ASSET_GALLERY.md`](visual/ASSET_GALLERY.md) | **ASHFALL — Visual Asset Gallery** — **Date:** this turn (Phase 14). |
| 🟢 `CURRENT` | [`docs/visual/ASSET_REGISTRY_RESOLUTION.md`](visual/ASSET_REGISTRY_RESOLUTION.md) | **ASHFALL AssetRegistry — Resolution Semantics** — **Source of truth:** `src/Host/AssetRegistry.cs` |
| 🟢 `CURRENT` | [`docs/visual/DIRECT_GODOT_ASSET_LOADS_AUDIT.md`](visual/DIRECT_GODOT_ASSET_LOADS_AUDIT.md) | **ASHFALL — Direct Godot Asset Loads Audit** — **Date:** 2026-08-26 |
| 🟢 `CURRENT` | [`docs/visual/FACTION_EMBLEMS_ENCYCLOPEDIA.md`](visual/FACTION_EMBLEMS_ENCYCLOPEDIA.md) | **ASHFALL: Factions of the Ashfall** — ═══════════════════════════════════════════════════════════════════════════════════════════ |
| 🔵 `GENERATED` | [`docs/ui/STITCH_GENERATED_UI_INVENTORY.md`](ui/STITCH_GENERATED_UI_INVENTORY.md) | **ASHFALL: Complete Google Stitch UI Inventory (62 Generated Screens)** — **Stitch Project Resource:** `projects/17640704459929707404` (*Ashfall - Atomic War Survival*) |

## 6. Lore, Narrative & World Design (13 documents)

| Status | Document | Title / Summary |
|---|---|---|
| 🟢 `CURRENT` | [`docs/lore/00_OVERVIEW.md`](lore/00_OVERVIEW.md) | **ASHFALL — Lore Bible** — `docs/superpowers/specs/2026-08-12-ashfall-massive-content-expansion-design.md` |
| 🟢 `CURRENT` | [`docs/lore/01_GAZETTEER.md`](lore/01_GAZETTEER.md) | **Sector 4 — Gazetteer** — `warlords_sector_4` is canon, so **Sector 4** is already the administrative |
| 🟢 `CURRENT` | [`docs/lore/02_THE_LIST.md`](lore/02_THE_LIST.md) | **The Spine — *Who Rode Out First*** — Canon establishes three facts and never connects them: |
| 🟢 `CURRENT` | [`docs/lore/03_LOCATIONS.md`](lore/03_LOCATIONS.md) | **New Locations** — 40 locations, banded by region per `01_GAZETTEER.md`. |
| 🟢 `CURRENT` | [`docs/lore/04_ENCOUNTERS.md`](lore/04_ENCOUNTERS.md) | **Encounters** — Canon already has four faction figureheads: **Colonel Voss** (`iron_garrison`), |
| 🟢 `CURRENT` | [`docs/lore/05_FACTIONS.md`](lore/05_FACTIONS.md) | **Factions — Powers and Currents** — Adding factions to a world that already has four well-drawn ones usually makes |
| 🟢 `CURRENT` | [`docs/lore/ASHFALL_GAME_MASTER_DOCUMENT_UPDATED.md`](lore/ASHFALL_GAME_MASTER_DOCUMENT_UPDATED.md) | **ASHFALL (Working Title: Atomic War - Starving Survival)** — YOU ARE AN AI SYSTEM ARCHITECT AND LEAD GAME DESIGNER REVIEWING THE 'ASHFALL' SURVIVAL GAME MASTER DOCUMENT. |
| 🟢 `CURRENT` | [`docs/lore/ASH_FALL_CREATIVE_FRAMEWORK.md`](lore/ASH_FALL_CREATIVE_FRAMEWORK.md) | **ASHFALL: ATOMIC WAR - COMPREHENSIVE CREATIVE WRITING FRAMEWORK** — This framework provides a complete system for creating all diegetic writing, lore, quests, environmental fiction, and... |
| 🟢 `CURRENT` | [`docs/lore/IntelBible.md`](lore/IntelBible.md) | **IntelBible.md — Radio Broadcast Archive** — This document contains 50 radio broadcast texts used by the RadioTunerSystem. Broadcasts are |
| 🟢 `CURRENT` | [`docs/narrative/ACCEPTANCE_eight_batches.md`](narrative/ACCEPTANCE_eight_batches.md) | **Narrative Acceptance Check — Eight Diegetic-Content Batches** — **Slice:** The eight creative-writing batches from commit `0118d212` (atmosphere, radio, journals, bureaucratic, lett... |
| 🟢 `CURRENT` | [`docs/narrative/ACCEPTANCE_moral_quests.md`](narrative/ACCEPTANCE_moral_quests.md) | **Narrative Acceptance Check — Moral-Choice Quests Expansion** — **Slice:** `moral_choice_quests_expansion.json` — 50 morale-choice quests with 200 moral/empathy-traded choices, from... |
| 🟢 `CURRENT` | [`docs/narrative/ACCEPTANCE_survivor_profiles.md`](narrative/ACCEPTANCE_survivor_profiles.md) | **Narrative Acceptance Check — Survivor Profiles Expansion** — **Slice:** `survivor_profiles_expansion.json` — 40 survivor profiles with character vignettes, from commit `94a68dc8`. |
| 🟢 `CURRENT` | [`docs/narrative/NARRATIVE_NEEDS.md`](narrative/NARRATIVE_NEEDS.md) | **Narrative Needs — Faction War Arc (Days 480–600+)** — Code requirements surfaced while authoring the six `faction_war_*.json` catalogs |

## 7. Data Authority & Subsystem Catalogs (6 documents)

| Status | Document | Title / Summary |
|---|---|---|
| 🟢 `CURRENT` | [`docs/data/CATALOG_REGISTRY.md`](data/CATALOG_REGISTRY.md) | **ASHFALL Data Authority & Master Catalog Registry** — **Authoritative Location:** `Assets/StreamingAssets/Data/` \| **Last Verified:** 2026-08-29 |
| 🟢 `CURRENT` | [`docs/data/DATA_GAP_AUDIT.md`](data/DATA_GAP_AUDIT.md) | **ASHFALL Data Gap Audit** — Counted entries in every top-level JSON catalog, identified C# consumers per catalog, |
| 🟢 `CURRENT` | [`docs/systems/AUDIO_SYSTEM.md`](systems/AUDIO_SYSTEM.md) | **ASHFALL Audio System Architecture** — The ASHFALL audio pipeline connects engine-agnostic Core domain simulation events to Godot-native audio playback thro... |
| 🟢 `CURRENT` | [`docs/systems/RESEARCH_CORE_PORT_PLAN.md`](systems/RESEARCH_CORE_PORT_PLAN.md) | **ASHFALL — Research Core Port Plan & Completion Report** — **Status:** **CLOSED — SHIPPED & VERIFIED AT PHASE 28** |
| 🟢 `CURRENT` | [`docs/systems/SKILL_PROGRESSION_CORE_PORT_PLAN.md`](systems/SKILL_PROGRESSION_CORE_PORT_PLAN.md) | **ASHFALL — Skill Progression Core Port Plan (Phase 14 design) — SHIPPED at Phase 18** — **Status:** design SHIPPED at Phase 18. Files: |
| 🟢 `CURRENT` | [`docs/systems/STANDING_RECORD_CORE_PORT_PLAN.md`](systems/STANDING_RECORD_CORE_PORT_PLAN.md) | **Standing Record Core Port Plan** — **Target:** Promote the Standing Record (Expansion 03) from read-only |

## 8. Developer Tooling, Skills & QA (6 documents)

| Status | Document | Title / Summary |
|---|---|---|
| 🟢 `CURRENT` | [`docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md`](qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md) | **Manual Smoke-Test Checklist — Audio & User Settings Recovery Behavior** — **Date:** 2026-08-27 |
| 🟢 `CURRENT` | [`docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`](qa/MANUAL_PLAYTHROUGH_CHECKLIST.md) | **Manual Playthrough Checklist — Day 1 → Day 2 Milestone** — **Environment:** Desktop Godot 4.7+ (.NET), launch via `godot --path .` or editor Play. |
| 🟢 `CURRENT` | [`docs/qa/TEST_LAYOUT_CONVENTIONS.md`](qa/TEST_LAYOUT_CONVENTIONS.md) | **ASHFALL Test Suite Layout & Conventions** — All unit, integration, simulation, and contract test files are organized by domain under `Ashfall.Core.Tests/`: |
| 🟢 `CURRENT` | [`docs/skills/ASHFALL_SKILL_REVIEW.md`](skills/ASHFALL_SKILL_REVIEW.md) | **ASHFALL Skill Review** — **Date:** 2026-08-22 |
| 🟢 `CURRENT` | [`docs/tools/TOOLING_CLASSIFICATION_AND_LIFECYCLE.md`](tools/TOOLING_CLASSIFICATION_AND_LIFECYCLE.md) | **ASHFALL — Non-Runtime Tooling Architecture, Classification, & Lifecycle** — **Date:** 2026-08-27<br> |
| 🟢 `CURRENT` | [`scripts/maintenance/README.md`](../scripts/maintenance/README.md) | **ASHFALL — Maintenance & Migration Scripts** — This directory houses historical one-off migration utilities and reusable batch-transformation tools for the ASHFALL ... |

## 9. General Project Guides & Archive Reference (34 documents)

| Status | Document | Title / Summary |
|---|---|---|
| 🟢 `CURRENT` | [`ANTIGRAVITY.md`](../ANTIGRAVITY.md) | **ASHFALL PROJECT — ANTIGRAVITY Instructions** — These five rules override anything else in this file. If a later section contradicts them, the rule below wins. |
| 🟢 `CURRENT` | [`CLAUDE.md`](../CLAUDE.md) | **CLAUDE CODE INSTRUCTIONS — ASHFALL PROJECT** — These five rules override anything else in this file. If a later section contradicts them, the rule below wins. |
| 🟢 `CURRENT` | [`CODEX.md`](../CODEX.md) | **ASHFALL PROJECT — CODEX Instructions** — These five rules override anything else in this file. If a later section contradicts them, the rule below wins. |
| 🟢 `CURRENT` | [`CRUSH.md`](../CRUSH.md) | **ASHFALL PROJECT — CRUSH Instructions** — These five rules override anything else in this file. If a later section contradicts them, the rule below wins. |
| 🟢 `CURRENT` | [`GEMINI.md`](../GEMINI.md) | **Antigravity Agent Rules — ASHFALL Project** — These rules are **always active** for every Antigravity session in this workspace. |
| 🟢 `CURRENT` | [`GOOSE.md`](../GOOSE.md) | **ASHFALL PROJECT — GOOSE Instructions** — These five rules override anything else in this file. If a later section contradicts them, the rule below wins. |
| 🟢 `CURRENT` | [`MIMOCODE.md`](../MIMOCODE.md) | **ASHFALL PROJECT — MIMOCODE Instructions** — These five rules override anything else in this file. If a later section contradicts them, the rule below wins. |
| 🟢 `CURRENT` | [`OPENSETUP.md`](../OPENSETUP.md) | **ASHFALL PROJECT — OPENSETUP Instructions** — These five rules override anything else in this file. If a later section contradicts them, the rule below wins. |
| 🟢 `CURRENT` | [`QWEN.md`](../QWEN.md) | **ASHFALL PROJECT — QWEN Instructions** — These five rules override anything else in this file. If a later section contradicts them, the rule below wins. |
| 🟢 `CURRENT` | [`VIBE.md`](../VIBE.md) | **ASHFALL PROJECT — VIBE Instructions** — These five rules override anything else in this file. If a later section contradicts them, the rule below wins. |
| 🟢 `CURRENT` | [`assets/quarantine/deprecated_sprites/README.md`](../assets/quarantine/deprecated_sprites/README.md) | **Deprecated Item Sprites Quarantine** — **Date Quarantined:** 2026-08-27 |
| 🟢 `CURRENT` | [`assets/sprites/AI_Generated/manifest.md`](../assets/sprites/AI_Generated/manifest.md) | **ASHFALL — Complete AI Game Assets Master Manifest (1,019 Assets Total)** — - **Location**: `generated_AIassets/` |
| 🟢 `CURRENT` | [`docs/AI_DISCLOSURE.md`](AI_DISCLOSURE.md) | **AI Content Disclosure — ASHFALL** — - **Code**: [e.g. "Every AI-generated function was reviewed, tested, and often rewritten. Architecture decisions, gam... |
| 🟢 `CURRENT` | [`docs/ASHFALL_CODE_INDEX.md`](ASHFALL_CODE_INDEX.md) | **ASHFALL — ENGINEERING CODE INDEX (cheap-context reference)** — Path: `home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` |
| 🟢 `CURRENT` | [`docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md`](ASHFALL_IMPLEMENTED_CANON_REGISTRY.md) | **ASHFALL: THE DEFINITIVE IMPLEMENTED-CONTENT & MECHANICS REGISTRY** — **Authoritative Forensic Knowledge Base for AI Game Mechanics & Narrative Brainstorming** |
| 🟢 `CURRENT` | [`docs/ASHFALL_MASTER_IMPLEMENTATION_PLAN.md`](ASHFALL_MASTER_IMPLEMENTATION_PLAN.md) | **ASHFALL: MASTER INTEGRATION & IMPLEMENTATION PLAN (ADVERSARIALLY HARDENED)** — **Deep Engineering Specifications, Exact Failure Mode Mitigations, Systemic Bridges & Verification Blueprints for 25 ... |
| 🟢 `CURRENT` | [`docs/CURRENT_AUTHORITY.md`](CURRENT_AUTHORITY.md) | **ASHFALL — Documentation Source-of-Truth & Authority Map** — **Date:** 2026-08-26 |
| 🟢 `CURRENT` | [`docs/ENGINE_SUPPORT_POLICY.md`](ENGINE_SUPPORT_POLICY.md) | **ASHFALL Engine Support and Source-Authority Policy** — This document defines which engine and source tree is authoritative during the Unity-to-Godot strangler migration. |
| 🟢 `CURRENT` | [`docs/GODOT_MIGRATION_STATUS.md`](GODOT_MIGRATION_STATUS.md) | **Godot Migration Status** — **Direction:** Unity → Godot (MIGRATION COMPLETE). Godot is the authoritative runtime editor and host. |
| 🟢 `CURRENT` | [`docs/HUMAN_AUTHORSHIP.md`](HUMAN_AUTHORSHIP.md) | **Human Authorship Checklist — ASHFALL** — - [ ] **Paint-over**: Every AI-generated sprite has been painted over by hand — brushstrokes, color choices, and deta... |
| 🟢 `CURRENT` | [`docs/HoldfastManualPlaytest.md`](HoldfastManualPlaytest.md) | **Holdfast Manual Playtest Checklist** — **Environment:** Desktop Godot 4.7.1+ (.NET), launch via `godot --path .` or editor Play. |
| 🟢 `CURRENT` | [`docs/HoldfastPlaytestHandoff.md`](HoldfastPlaytestHandoff.md) | **Holdfast Playtest Handoff** — **Environment:** Desktop Godot 4.7.1+ (.NET), display available (`$DISPLAY=:0`). |
| 🟢 `CURRENT` | [`docs/MORAL_CHOICE_SYSTEM.md`](MORAL_CHOICE_SYSTEM.md) | **ASHFALL: ATOMIC WAR - MORAL CHOICE SYSTEM** — *60 Quests, 8 Branching Paths, World Impact Without UI Clutter* |
| 🟢 `CURRENT` | [`docs/agents/AGENTS_SYNC_REPORT.md`](agents/AGENTS_SYNC_REPORT.md) | **ASHFALL Agent-Rulebook Synchronization Report** — **Canonical source:** `AGENTS.md`<br> |
| 🟢 `CURRENT` | [`docs/cli/HOST_CLI_COMMAND_CATALOG.md`](cli/HOST_CLI_COMMAND_CATALOG.md) | **ASHFALL — Host CLI Command Catalog** — **Last Verified:** 2026-08-29<br> |
| 🟢 `CURRENT` | [`docs/cli/HOST_TEST_EXIT_CODES.md`](cli/HOST_TEST_EXIT_CODES.md) | **ASHFALL — Host Self-Test Exit Codes & Output Protocol** — **Date:** 2026-08-27 |
| 🟢 `CURRENT` | [`mistral_plans/AGENTS.mistral-plans.md`](../mistral_plans/AGENTS.mistral-plans.md) | **PROJECT: ASHFALL (working title) — 2D Atomic-War Survival** — Original 2D survival-management game set after a nuclear exchange. Inspired by the survival-management genre; do **no... |
| 🟢 `CURRENT` | [`mistral_plans/ASH_FALL_ALPHA_0.8_DEVELOPMENT_PLAN.md`](../mistral_plans/ASH_FALL_ALPHA_0.8_DEVELOPMENT_PLAN.md) | **ASHFALL ALPHA 0.8 DEVELOPMENT PLAN** — **Milestone**: ASHFALL Alpha 0.8 = Modular host + fully connected Shelter 2D viewport + functional visual Wasteland map |
| 🟢 `CURRENT` | [`scripts/README.md`](../scripts/README.md) | **ASHFALL — Script Catalog & Lifecycle Index** — This document catalogs all developer tools, CI gates, asset pipelines, and maintenance utilities under `scripts/`, cl... |
| 🟢 `CURRENT` | [`summaries/README.md`](../summaries/README.md) | **ASHFALL: Atomic War - Starving Survival** — This folder contains the complete summary and canvas-style exported PDF reports for the forensic optimization and deb... |
| 🟢 `CURRENT` | [`tools/README.md`](../tools/README.md) | **ASHFALL — Non-Runtime Tools & Utilities Catalog** — **Date:** 2026-08-27<br> |
| 🟢 `CURRENT` | [`tools/asset_migration/legacy_tooling/AI_Generated/manifest.md`](../tools/asset_migration/legacy_tooling/AI_Generated/manifest.md) | **ASHFALL — Complete AI Game Assets Master Manifest (1,019 Assets Total)** — - **Location**: `generated_AIassets/` |
| 🔵 `GENERATED` | [`prompt_assets/prompt-optimizer-skill.md`](../prompt_assets/prompt-optimizer-skill.md) | **UNIVERSAL PROMPT OPTIMIZER — Full Skill Reference** — name: prompt-optimizer |
| 🟡 `HISTORICAL` | [`docs/ARCHIVE_INDEX.md`](ARCHIVE_INDEX.md) | **ASHFALL Historical Documentation & External Archive Index** — This repository maintains a lean, living documentation corpus in `docs/` representing active, authoritative game spec... |
