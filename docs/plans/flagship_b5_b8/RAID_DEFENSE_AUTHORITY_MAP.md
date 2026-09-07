# Raid Defense Authority Map — Phase 0 evidence

> Required by flagship brief §10.1 before adding any new defense class.

## A. The live attack path

```
warlord/faction pressure (WarlordDoctrineSystem, faction war authorities)
  → raid selection / event generation (Plan 45 phase 2 path in src/Main.Muster.cs)
      → perimeter emplacements resolve FIRST (PerimeterDefenseSystem assault sim)
          → only raiders that BREACH continue
              → survivor combat via TacticalCombatSystem / combat catalog
                  (enforcers composed by EnemyCompositionSelector, CombatCatalog)
                      → aftermath (casualties, captives handoff, defense wear)
                          → journal / briefing reporting
```

Evidence: `src/Main.Muster.cs` Plan 45/163 comments — "perimeter emplacements,
resolve first; only raiders that breach … the raid composition from the combat
catalog (warlord enforcers…)"; "the raid never reaches survivor combat" when
repelled.

## B. Authority assignments

| Concern | Owner | File / section | B8 action |
|---|---|---|---|
| Why a raid occurs; pressure; doctrine; attacker identity | warlord/faction systems | `Assets/Ashfall.Core/Warlords/` | preserve; defense must never silently lower pressure |
| Perimeter fortification, emplacements, sectors | `PerimeterDefenseSystem` | `Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs` + `PerimeterDefenseCatalog.cs`; save `perimeter_defense` (schema_version 2, `PerimeterDefenseSaveStore`) | **extend only** — this is the Plan 67 owner |
| Tactical emplacement resolution | `PerimeterDefenseSystem.AssaultSimulationResult` (Repelled/Breached, strength attrition, rounds fired, emplacements damaged/destroyed, stealth neutralized) | same | preserve; wire aftermath wear if not already |
| Survivor combat | `TacticalCombatSystem` (+ `CombatBreachingEngine`, `CombatTypes`) | `Assets/Ashfall.Core/Combat/` | preserve; no defense-side HP writes |
| Door / visitor incidents | `AirlockSecuritySystem` | `Assets/Ashfall.Core/AirlockSecuritySystem.cs`; save `airlock_security` | do not add fortification here |
| Power availability for defenses | `PowerGridSystem` | `power_grid` | verify turret `power_draw_watts` are registered loads; brownout → sentry disabled via power state |
| Ammunition | canonical ammo items (`ammo_9x19`, `ammo_556` in `perimeter_defenses.json` `required_ammo_type`/`magazine_capacity`) | inventory authority | no "sentry ammo points" |
| Research gating | `ResearchSystem` | `research` | **to add** (see gaps) |

## C. Live defense catalog (`Assets/StreamingAssets/Data/perimeter_defenses.json`)

| defense_id | Build cost | Power W | Ammo | Notes |
|---|---|---|---|---|
| `def_sandbag_berm` | sandbags 4, scrap_wood 2 | 0 | — | passive |
| `def_razorwire_obstacle` | scrap_metal 3, electrical_wire 2 | 0 | — | passive |
| `def_tripwire_flare_line` | flare_tripwire 2, scrap_wood 1 | 0 | — | alert/warning |
| `def_reinforced_outer_gate` | scrap_metal 8, scrap_wood 4 | 0 | — | passive |
| `def_heavy_barricade` | scrap_metal 5, sandbags 3 | 0 | — | passive |
| `def_searchlight_tower` | scrap_metal 4, electrical_wire 3 | 250 | — | powered |
| `def_sentry_turret_9mm` | scrap_metal 6, electrical_wire 2 | 350 | ammo_9x19 | powered + ammo |
| `def_sentry_turret_556` | scrap_metal 10, electrical_wire 4 | 600 | ammo_556 | powered + ammo |

Sector model: `PerimeterSector` — `north`, `east`, `south`, `west`, `gate`
(canonical topology; no invented map directions). Per-sector state:
`emplacement_ids`, `alarm_armed`; per-emplacement runtime: HP, active,
destroyed, loaded ammo, jam state. Intrusion log + `assault_count` persisted.

## D. Confirmed gaps (the actual Plan 67 remainder)

1. **No research gating.** No `required_knowledge` field on any definition; zero code references to `knowledge_automated_sentry_doctrine`, `knowledge_turret_controller_blueprint`, `knowledge_fortified_chokepoints`, `knowledge_defensive_tripwire_arrays`, `knowledge_guerrilla_ambush_tactics`. Research→build gating must terminate in real build actions via `ResearchSystem.IsManualUnlocked`.
2. **`item_sentry_targeting_chip` has zero consumers** (catalog-only). Attach as build/upgrade dependency to turret emplacements through the standard transaction path.
3. **`item_iff_beacon` has zero consumers.** Classification exercise (§10.9) still required: automated-defense encounter consumer only; must not bypass living-faction raids; single-use commit rules.
4. **Power coupling audit** — confirm turret/searchlight watts are enforced against the grid at runtime (brownout → disabled sentries), and that disabled state flows into assault resolution.
5. **Raid modifier snapshot typing** — `AssaultSimulationResult` covers resolution; add the bounded pre-raid snapshot (warning lead, powered/disabled sentry counts, breach state) only where the encounter path can genuinely consume it.

## E. Forbidden effects (restated, enforced by tests to be written)

- Defense code subtracting attackers/HP outside the assault-sim/combat contract.
- Power code modifying enemies directly.
- Research unlock granting built emplacements.
- Save/reload replaying assault rounds, ammo consumption, or intrusion-log entries.
