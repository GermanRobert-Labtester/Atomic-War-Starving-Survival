# Expansion 10 — The Silent Foundry — Phase 0 Preflight & Dependency Map

Status: **DATA-ONLY** (no runtime system, no host, no save, no trigger path).

## Existing anchors (reuse, do not duplicate)

| Asset | Location | Runtime consumer today |
|---|---|---|
| `exp_10_the_silent_foundry` | `narrative/jrnl_templates_cycle_d.json` (expansion_id) | none |
| `jrnl_foundry_first_heat` | same file (stress −5, hope +5, role foundryman) | none — `NarrativeBatchCatalog` is dead loader code |
| `jrnl_foundry_strike` | same file (stress +7, hope +2, role schoolmistress) | none |
| `current_10_the_silent_foundry_guild` | `narrative/regional_treaty_protocols.json` | none (not in any faction registry) |
| treaty_05 / 10 / 12 / 16 | same treaty file (days 280 / 950 / 1500 / 3650) | `RegionalTreatyCatalog` (tests only) |
| `room_bp_11_the_silent_foundry_smelter_bay` | `narrative/bunker_blueprints_codex.json` | `BunkerBlueprintCatalog` (tests only) |

## Dependency map (current)

```
Authored data                    Core domain                  Godot host
─────────────────────────────    ──────────────────────      ─────────────────────────
narrative/jrnl_templates_*.json → NarrativeBatchCatalog      (unwired — dead)
narrative/regional_treaty_*.json→ RegionalTreatyCatalog      (tests only)
narrative/bunker_blueprints_*.json→ BunkerBlueprintCatalog   (tests only)
items.json / recipes.json        (not loaded by Godot host)
ExpansionMasterSession (01–04)  → ExpansionHostSession       → ExpansionsHubPanel / SaveAll
ExpansionHubSave V1 (checksum)  → ExpansionHubSaveCodec      → ExpansionHubSaveStore
JournalSystem + KnowledgeBase   → JournalSaveStore           → JournalBookUI / JournalPanel
NarrativeEncounterSystem        → NarrativeHostSession       → NarrativeSaveStore
MusterSystem (06)               → MusterHostSession          → MusterPanel / MusterSaveStore
SkyLayerArmorSystem (11)          Core-only (tests only)
GenerationalSuccessionEngine(12)→ ExpansionHostSession       → CenturySeedPanel
NeedsSystem (per-survivor)      → SurvivorsHostSession       → SurvivorsPanel
```

## Insertion points (EXTEND, no new authorities)

1. Core `SilentFoundrySystem` — new bounded capability (no existing system models
   multi-stage production + maintenance cycles + quality + runtime treaty compliance).
2. `SilentFoundryCatalog` + `foundry_production.json` / `foundry_items.json` /
   `foundry_faction.json` (expansion-specific JSON convention, cf. crossing_items.json).
3. `RegionalTreatyCatalog` — add exact-faction lookup (substring API kept for compat).
4. `ExpansionMasterSession` — orchestrate exp_10 alongside exp 01–04.
5. `ExpansionHubSave` V2 — foundry state in the existing hub envelope + frozen V1 migration.
6. Journal — templates stay in `NarrativeBatchCatalog` (no schema change); runtime triggers
   via `JournalSystem.TryAddRawEntry(template_id, …)`; morale via NarrativeEncounter-style
   cumulative accumulator.
7. Host `SilentFoundryHostSession` + `SilentFoundryPanel` + Main.cs wiring.

## Canon & alias notes

- `current_10_the_silent_foundry_guild` (treaty) ≠ `current_10_the_foundry_union`
  (currents_pamphlets.json). Distinct fictional factions sharing the `current_` prefix
  vocabulary. No mapping between them; do not register one as the other's alias.
- Campaign reachability: Year of Ash timeline covers days 180–360. Treaty day 280 reachable;
  950 / 1500 / 3650 NOT reachable in the current Godot campaign — treaty assessment is
  day-agnostic in Core and synthetic-day testable.
- Data-integrity selftest walks only top-level `Data/*.json`; `narrative/` is exempt.
  New top-level catalogs are mechanically validated (ids register, refs must resolve).
