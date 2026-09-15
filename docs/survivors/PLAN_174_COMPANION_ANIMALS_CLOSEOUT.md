# Plan 174 — Companion Animals & Working Beasts: Closeout

**Status:** COMPLETE · **Date:** 2026-09-13 · **Batch:** `PLANS-174-177-FLAGSHIP-SURVIVOR-WORLD`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` (`companion_animals`) — registration, assignment, bond/training, feeding hierarchy, bounded role queries, sickness, death/grief; strict catalog loader |
| Data | `Assets/StreamingAssets/Data/companion_animals.json` (3 profiles: Ash Hound guard+morale, Feral Goat pack, Cotton Hare morale) |
| Data (additive) | `wildlife_ecosystem.json` + tameable `species_ash_hound` row — **species authority stays in the wildlife catalog** (Trap A avoided) |
| Host | `src/Host/CompanionSaveStore.cs`, `src/Main.Companion.cs` (daily tick, food port, grief route, treat command) |
| Save | `SaveSectionRegistry` row (`companion_animals`, owner `hunting`) + `companion_animals_save.json`; RNG stream `companion_animal` |
| Routes | Taming (`WildlifeEcosystemSystem.TryTame`) → auto-adopt via wildlife `animal_id`; guard → `DefenseSystem.ResolvePreCombatRaid(guardNightDetection)`; pack → `ExpeditionHostSession.PackCapacityProvider` (post-start, bounded); morale → `NeedsSystem.Modify` (bounded bp) |
| Tests | Core 16/16 (`Plan174CompanionAnimalTests`) · wiring 6/6 (`Plan174CompanionHostWiringTests`) · campaign harness coverage |

## Contract guarantees (§30 DoD — all satisfied)

- **Stable identity.** `companion_id` = the wildlife `DomesticAnimalState.animal_id`; names are presentation data, never identity.
- **Upkeep consumes real food.** Preferred-tag → authored-fallback → partial-feed hierarchy through the canonical inventory port; one missed meal degrades, never instantly kills (§5.8).
- **Bond/training deterministic.** Pure functions of state + profile; clamped (bond ≤ 100, training ≤ Expert); no instant max benefits (§5.6–5.7).
- **Bounded roles, routed not bypassed.** Guard fades below the health floor and scales down while hungry/sick; pack bonus never exceeds authored kg and is applied only through the expedition authority; morale support is a bounded query the morale authority applies — the system never writes survivor stats.
- **Grief is bounded and applied once.** `CompanionDied` fires once; the host route applies a bond-scaled payload within [−1500, −150] bp through `NeedsSystem.Modify` — never a hard-set morale catastrophe (Trap C avoided). Proven by the campaign harness.
- **Sickness is a feeding/veterinary concern.** `TreatSickness` consumes a canonical medical kit; malnutrition explicitly refuses kits.
- **Save/load exact; old saves adopt existing tamed animals only** — never fabricated companions (§10).

## Deferred (flagged, not silent)

`KennelUI` panel (presentation wave), kennel/stable room capacity, terrain-specific pack penalties on expedition legs, Plan 176 anomaly-dose → companion sickness handoff (Wave F cross-plan item).
