# Expansion 10 — The Silent Foundry — Phase 0 Preflight & Dependency Map

Status: implemented (Core system + host + save + trade surfaces). Re-anchored to
the Sector 4 / District 8 campaign on 2026-08-16: the expansion no longer binds
to the Valley-of-Tessarat treaty corpus (`narrative/regional_treaty_protocols.json`);
its accords are authored for the live game world in `Data/foundry_accords.json`.

## Existing anchors (reuse, do not duplicate)

| Asset | Location | Runtime consumer today |
|---|---|---|
| `exp_10_the_silent_foundry` | `narrative/jrnl_templates_cycle_d.json` (expansion_id) | journal templates via host |
| `jrnl_foundry_first_heat` | same file (stress −5, hope +5, role foundryman) | `SilentFoundryHostSession` journal triggers |
| `jrnl_foundry_strike` | same file (stress +7, hope +2, role schoolmistress) | same |
| `faction_silent_foundry` | `Data/foundry_faction.json` + `Data/foundry_accords.json` | foundry faction card, trade stance, radio |
| brine pipe / labour schedule / road iron / cluster charters | `Data/foundry_accords.json` (days 280 / 305 / 330 / 365 — all campaign-reachable) | `RegionalTreatyCatalog` via `SilentFoundryCatalogLoader.LoadAccordRatificationDays` |
| `room_bp_11_the_silent_foundry_smelter_bay` | `narrative/bunker_blueprints_codex.json` | `BunkerBlueprintCatalog` (maintenance cycle 4) |

## Dependency map (current)

```
Authored data                    Core domain                  Godot host
─────────────────────────────    ──────────────────────      ─────────────────────────
Data/foundry_accords.json      → SilentFoundryCatalogLoader → ExpansionHostSession /
Data/foundry_production.json   → SilentFoundryCatalog       SilentFoundryHostSession
Data/foundry_items.json        → ItemCatalog (host)         (BindTreaties ratification days)
Data/foundry_treaty_consequen. → SilentFoundryConsequencePo→ market demand + stance mirroring
narrative/jrnl_templates_*.json → NarrativeBatchCatalog      JournalSystem.TryAddRawEntry
narrative/bunker_blueprints_*.json→ BunkerBlueprintCatalog   maintenance cycle binding
ExpansionMasterSession (01–04,10) → ExpansionHostSession     → ExpansionsHubPanel / SaveAll
ExpansionHubSave V3 (checksum)  → ExpansionHubSaveCodec      → ExpansionHubSaveStore
JournalSystem + KnowledgeBase   → JournalSaveStore           → JournalBookUI / JournalPanel
NeedsSystem (per-survivor)      → SurvivorsHostSession       → SurvivorsPanel
```

## Insertion points (EXTEND, no new authorities)

1. Core `SilentFoundrySystem` — bounded capability (heat lifecycle, maintenance,
   quality, safety, labour disputes, accord compliance).
2. `SilentFoundryCatalog` + `foundry_production.json` / `foundry_items.json` /
   `foundry_faction.json` / `foundry_accords.json`.
3. `RegionalTreatyCatalog` — exact-faction lookup (used against the accords file).
4. `ExpansionMasterSession` — orchestrates exp_10 alongside exp 01–04.
5. `ExpansionHubSave` V3 — foundry state in the hub envelope + frozen V1/V2 migration.
6. Journal — templates stay in `NarrativeBatchCatalog`; runtime triggers via
   `JournalSystem.TryAddRawEntry(template_id, …)`.
7. Host `SilentFoundryHostSession` + `SilentFoundryPanel` + Main.cs wiring.

## Canon & alias notes

- `faction_silent_foundry` (District 8 works) ≠ `current_10_the_foundry_union`
  (currents_pamphlets.json, valley corpus). Distinct fictional factions sharing a
  foundry theme. No mapping between them; do not register one as the other's alias.
  Regression guards in tests assert the inequality.
- The valley treaty corpus (`regional_treaty_protocols.json`) is a parallel
  long-horizon narrative layer, NOT the live campaign. Per the Holdfast plan,
  Tessarat names must not surface in Sector 4 / District 8 content. The foundry
  therefore owns its accords in `Data/foundry_accords.json`, ratified on days the
  campaign can actually reach (280/305/330/365, 30-day assessment cycles).
- Data-integrity selftest walks top-level `Data/*.json`; `foundry_accords.json`
  is mechanically validated; `narrative/` remains exempt.
