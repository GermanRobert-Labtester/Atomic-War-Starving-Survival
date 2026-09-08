# Plan 135 — Narrative Source Adapter Matrix

This matrix defines the deterministic, typed mapping from heterogeneous narrative catalog schemas to the canonical `NarrativeDiscoveredRecord` model without schema collapse.

## Adapter Definitions & Schemas

| Adapter Class | Target Catalogs | Array Root Key | Record ID Field | Primary Text Field | Subtitle / Attribution Mapping |
|---|---|:---:|---|---|---|
| `ProcessLogSourceAdapter` | `boiler_feedwater_deaerator_audits.json`<br>`ragdoll_germination_assays.json`<br>`artesian_well_contamination_logs.json`<br>`slow_sand_schmutzdecke_logs.json`<br>`scavenger_expedition_route_notes.json`<br>`surface_radiation_topo_sheets.json`<br>`water_clock_orifice_silt_records.json`<br>`pot_furnace_glass_melts.json`<br>`bunker_children_folklore.json` | `items` | `id` | `prose` | Formats `timestamp_relative` and domain sensor readings into clean player meta strip |
| `BunkerGlitchSourceAdapter` | `bunker_maintenance_glitches.json` | `glitches` | `glitch_id` | `anomaly_description` + `emergency_protocol` | `affected_subsystem` + `dmitri_shift_note` |
| `BunkerBlueprintSourceAdapter` | `bunker_blueprints_codex.json` | `blueprints` | `room_id` | `purpose` + `chief_engineer_note` | `room_name` |
| `BunkerCourtSourceAdapter` | `bunker_court_verdicts_codex.json` | `cases` | `case_id` | `verdict_text` | `defendant_name` + `charge_summary` |
| `WireConfessionSourceAdapter` | `wire_confessions.json` | `confessions` | `confession_id` | `transcript` | `author_name` + `title` |
| `TradeLedgerSourceAdapter` | `bunker_trade_ledger_batch_2.json` | `transactions` | `tx_id` | `notes` | Parties `giver` -> `receiver`, goods exchanged |
| `RegionalTreatySourceAdapter` | `regional_treaty_protocols.json` | `treaties` | `treaty_id` | `transcript` | `treaty_title` + `signatory_factions` |
| `SurgeonsCasebookSourceAdapter` | `surgeons_casebook_batch_2.json` | `cases` | `case_id` | `diagnosis` + `treatment` + `outcome` | `patient` + `presenting_symptoms` |
| `DeadHandDirectiveSourceAdapter` | `dead_hand_directives.json` | `directives` | `directive_id` | `transcript` | `directive_title` + `issuing_command` |
| `CourierDispatchSourceAdapter` | `courier_dispatches_master.json` | `dispatches` | `dispatch_id` | `transcript` | `sender` -> `recipient` + `route` |
