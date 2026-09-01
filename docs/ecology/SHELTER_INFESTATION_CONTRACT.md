# SHELTER_INFESTATION_CONTRACT.md — Plan 28 Task 28S

> Authored candidates for these four contracts exist in `RETIRED_ECOLOGY_ISLAND.md`
> (vent mold, pantry weevils, wall nest, vermin incursion — retired with the runtime
> island, preserved for the Phase 4 wiring task).

The four shelter infestations are **contracts on existing systems** — each names its owner,
trigger, consequence path, and terminal state. Implementation lands with the shelter content
pass; this document is the binding spec so no duplicate engine appears.

## Contract fields (every entry)

`owner system · trigger (live state) · consequence (routed through owner) · clue ·
clear methods (real items/actions) · leave/harvest option · terminal state · persistence`

## 1. Vent mold → VentilationSystem
- **Trigger:** damp window (Thaw/Black Bloom weather) + low filtration/ventilation margin.
- **Effect:** air-filtration degradation accelerates; spore exposure risk routed to the Plan 09
  disease authority (no direct health loss).
- **Clear:** filter swap, ventilation cleaning run, dry-out (fuel cost).
- **Leave:** spore exposure risk continues while damp; fungal sample possible (research hook).
- **Terminal:** cleaned / sealed / tolerated-with-risk. Cooldown before recurrence.

## 2. Pantry weevils → food spoilage authority
- **Trigger:** good grain/seed stores + warm season window (28AC chain).
- **Effect:** bounded spoilage acceleration on affected stock — never total loss.
- **Clear:** remove/inspect contaminated stock, sealed containers, colder storage.
- **Leave:** emergency protein option at a morale/quality cost; ongoing small loss.
- **Terminal:** stock treated or exhausted.

## 3. Wall nest → shelter event/maintenance
- **Trigger:** damaged wall segment + quiet season.
- **Effect:** localized hazard (noise, bites, spoilage nearby) through shelter event state.
- **Clear:** smoke/repair/traps with existing items; or seal the breach.
- **Terminal:** cleared or sealed; nest does not migrate shelters.

## 4. Vermin incursion → inventory/spoilage + trap sites
- **Trigger:** exposed food/waste state (real shelter-state hooks from 28AO).
- **Effect:** accelerated spoilage on exposed stock; nuisance catches rise (density feed).
- **Clear:** remove attractant (existing actions), set traps (existing trapping), seal gaps.
- **Terminal:** attractant removed or population trapped down.

## Rules (non-negotiable)
1. Every effect routes through the owning system's public API — no hidden inventory writes.
2. Persistence: infestation flags ride the shelter/section save of the owning system.
3. Recurrence: seasonal weighting only (28AP); no permanent spam (28BB balance gate).
4. No new engine — `ShelterInfestation` would violate §1.9; state lives with the owner.
