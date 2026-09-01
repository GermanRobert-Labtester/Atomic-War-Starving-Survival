# INFESTATION_MATRIX.md — Plan 28 Task 28Q (architecture audit)

**Verdict: NO generic infestation runtime exists in the repository — and none was added.**
Plan 28's infestation work stays **data/events on current state systems** (§1.9).
An earlier parallel implementation (`EcologyCoordinator` + `ecological_infestations.json`)
was retired as a runtime island; its ten authored infestation definitions are preserved as
the Phase 4 content seed in `RETIRED_ECOLOGY_ISLAND.md`.

## Audit findings

| System searched | Infestation-relevant state found | Reusable? |
|---|---|---|
| `GreenhouseSystem` | **Yes — real crop blight state**: `plot.blight` (0–1), `OnBlightOutbreak`, `GreenhouseTreatBlight` command, `BlightTreatment` item, `blightRollCount` deterministic reseed | crop infestations = greenhouse authority (Plan 22) |
| `VentilationSystem` | air-filtration degradation, hazard cues wired to audio (Plan 07) | vent mold = ventilation + filter consumption |
| Location state | `LocationEvolutionRecord.activeThreats` (string tags, e.g. `threat_wild_beasts`, `threat_rad_squatters`), `MarkCleared`, `AddThreat` | location-level infestation = threat tag + expedition clear |
| Shelter events | existing hazard/maintenance event system | wall nests, pantry pests |
| `CatalogIntegrityRules` | Tier-2 keys `infestation_site_id`, `fungal_species_identified` (timber/milling catalogs) | content keys exist; no engine |
| Excavation | `ExcavationSystem` with deterministic event policy | nest-disturbance eligibility is an event-table add |

**Conclusion:** the common infestation contract below is *authored content structure* on
existing state — no new engine (acceptance criterion for 28Q).

## Common infestation contract (for the location/shelter packs)

```
infestation = {
  source            // which system owns it: greenhouse | ventilation | location-threat | food-spoilage
  location/shelter  // real loc_* or shelter room — never a wildlife-only space
  trigger           // environmental state (damp season, exposed stores, damaged wall)
  hazard            // debuff routed through the owning system (air quality, spoilage, threat tag)
  clue              // field-guide sign / radio line / visual text
  clear_methods     // existing items + actions only (see SHELTER_INFESTATION_CONTRACT)
  leave_option      // tolerated outcome where plausible (harvest or risk)
  terminal_state    // cleared | sealed | harvested | abandoned — every chain terminates
  recurrence        // cooldown or season gate — never infinite spam
}
```

## Location infestation pack (6 — authored against real locations on request)

Candidates bound to existing `loc_*` (all resolve in `locations.json`):
molerat nest (canyon cut), hive (orchard/cider press), mold bloom (pumphouse),
roach colony (grange stores), fungal carpet (printworks damp), rat colony (weighbridge grain).

Every infestation: ≥1 non-combat resolution (seal, smoke, trap, remove attractant, or leave/
harvest), one bounded resource opportunity, terminal state. **Deferred until the location
content pass** — the contract above is the gate; no engine was built.

## Shelter infestation pack (4) — owner mapping

| Case | Owning authority | Mechanism |
|---|---|---|
| Vent mold | `VentilationSystem` (air-filtration degradation) | degrade filter faster; cleaning action |
| Pantry weevils | food spoilage/inventory authority | bounded spoilage acceleration, inspection |
| Wall nest | shelter maintenance/wall state | structural event + repair action |
| Vermin incursion | trapping (nuisance catch) + food state | attract → trap-out cycle |

**Non-negotiable:** no direct item deletion outside inventory authority; all four persist
through the shelter save sections.
