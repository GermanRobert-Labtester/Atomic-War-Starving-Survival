# Plan 29 — Shelter Wear Provenance (Phase 0, §3.6–§3.9)

> Classification per Plan 29 §29C.1. **Gate: nothing in category 4 may be built until
> this map is reviewed and the §5 Core-extension test passes.** Verified 2026-09-01.

## 1. Surface classification

| Surface / domain | Classification | Authority |
|---|---|---|
| Roof armor (per grid column) | **1 — modeled** | `SkyLayerArmorSystem` (`CeilingCellArmor.currentDurability`, kinetic impacts, `RepairCell`) |
| Ceiling shielding material (per room) | **1 — modeled** | `MaterialShieldingSystem` (Wood/Dirt/Concrete/Lead) ⚠ see §2 overlap |
| Room attenuation (Day-1 roster) | **1 — modeled** | `StartingLevelSystem` room `material`+`attenuation` ⚠ see §2 overlap |
| Heating pipes | **1 — modeled** | `ShelterThermalSystem` `PipeSegment.condition`/burst |
| Ducts | **1 — modeled** | `VentilationSystem.ductIntegrity` |
| Room temperature / freeze damage | **1 — modeled** | `ShelterThermalSystem` (`isFrozen`, `freezeDamage`) |
| Insulation factor (per room) | **2 — derived-ish** | `ThermalRoomNode.insulationFactor` exists but is a static config value (0.1–2), not a wear state; renovatable surface |
| Fire structural damage | **1 — modeled (event)** | `ShelterFireHazardSystem.structuralDamage` per incident |
| Flood / moisture ingress | **2 — indirectly modeled** | `SumpFloodingSystem` flood events → `WaterTreatment.SetIncomingContamination` (no persistent surface moisture state) |
| Airlock seals | **4 — missing & meaningful** | no seal condition; glitch lore (`seal cycling`, gasket kits) supports it |
| General wiring | **4 — missing & meaningful** | no wiring wear; power grid models breakers/fuel, not cable condition; glitch lore has ground-fault/short entries |
| Bunks / furnishings | **4 — missing & meaningful** | no condition; bunks are capacity only (Assignment cap 4) |
| Interior walls / finish / paint | **3 — cosmetic** | graffiti/lore only |
| Flooring, braces, fixtures, lockers | **3 — cosmetic** | lore only (this is correct — not worth modeling) |
| Radon phase | **1 — modeled** | `YearOfAshRadonSystem` (authoritative per VentilationSystem header) |

## 2. ⚠ Shielding triple-representation (blocker for 29C)

Three parallel models of "how much fallout gets through the ceiling":

1. `StartingLevelState.rooms[].material + attenuation` (Day-1 roster, upgradable);
2. `MaterialShieldingSystem` per-room ceiling material (separate state, own save);
3. `SkyLayerArmorSystem` per-grid-column armor (Expansion 11, own save).

They do not reference each other. **Plan 29 must not add a fourth.** Before any wear or
renovation work touches shielding, Task 29C must define which of these is authoritative
per surface (recommendation: SkyLayerArmor = roof/impact authority; MaterialShielding =
interior shielding upgrades; StartingLevel attenuation = Day-1 roster projection) and
consume only that. Renovation "seal & insulate" effects should target
`ThermalRoomNode.insulationFactor` + a new seal channel — not shielding.

## 3. Candidate minimal wear channels (Plan 29 §29C.2/29C.3)

Only category-4 domains, room-scoped:

| Channel | Causes (real, existing) | Consequence sinks (existing) | Repair materials (existing) |
|---|---|---|---|
| **Seals/moisture** | weather pressure (WeatherKind), freeze/thaw cycles, flood events (SumpFlooding), age | Thermal heat loss (insulationFactor input), Ventilation radon/CO ingress, contamination events | `item_hermetic_hatch_silicone_gasket`, `scrap_metal`, `concrete_mix` |
| **Electrical fabric** | brownouts (PowerGrid), load spikes (seeded), fire incidents | PowerGrid trip chance, Ventilation blower efficiency, fire ignition risk | `copper_wire_10m_of_10m`, `scrap_electronic`, `electronic_scrap` |
| **Furnishings (bunks)** | occupancy/use (Assignment), freeze, fire | rest/morale via NeedsSystem, Thermal warmth retention | `scrap_wood`, `cloth`, `metal_pipe` |

Accrual must be **deterministic daily pressure** (no per-tile RNG), banded
(Sound/Worn/Poor/Failing per §29C.8), and persisted per (room, channel) only.

## 4. Core-extension gate (§29C.4) — assessment

A new room-wear store is *justified in principle* for the three channels above (each has
≥3 consumers: thermal, ventilation, needs/morale, power, fire) **but** Plan 29 should
first verify in Phase 5 that effects can't be expressed through existing
`ThermalRoomNode.insulationFactor` + flags. Decision deferred to Phase 5 as planned.

## 5. Repair material inventory (existing items — do NOT invent new ones)

`scrap_metal`, `scrap_wood`, `scrap_mechanical`, `scrap_electronic`, `scrap_chemical`,
`copper_wire_10m_of_10m`, `metal_pipe`, `iron_pipe`, `charcoal`, `cloth`,
`concrete_mix`, `concrete_rubble`, `water_filter`, `air_filter`, `filter_pack`,
`item_air_filter_hepa`, `item_water_filter_advanced`, `item_hermetic_hatch_silicone_gasket`,
`item_foundry_roof_armor_plate`, `item_foundry_bearing_housing`, `item_welders_glass`,
plus foundry outputs (Plan 22A) via `SilentFoundrySystem` products
(pipes/plates/brackets per its header comment).

⚠ **Narrative glitch kits do not resolve:** the 20 canonical maintenance glitches
reference ~80 `item_*` ids absent from `items.json` (e.g. `item_heavy_welding_rig`,
`item_asbestos_gasket_ring`, `item_brass_snifting_valve`…). CatalogIntegrityValidator
does not currently check `required_repair_kit` refs. Task 29B must either register these
items or remap kits to §5 materials before any glitch becomes actionable — otherwise
Plan 29 §13.3 (no runtime impossible repair) is violated.

## 6. Labor / project systems audit (§3.8)

- `DutyRosterSystem` (+ `DutyRosterCatalog`, host `DutyRosterHostSession`) — the
  assignment engine renovation should ride on (Plan 29 §29C.18). Workers → daily labor
  contribution is the established pattern (Excavation `AssignWorkers` × 5/day, Foundry
  workers, DutyRoster shifts).
- `ShelterScheduleSystem` — shift schedule flavor/scheduling.
- `ExcavationSystem` — **world-side**; its worker/progress/shoring pattern is the right
  template for renovation stages, but excavation sites are external locations.
- `SilentFoundrySystem.TreatyLabor` — existing labor-obligation pattern (quota,
  deadline, assessment) reusable for renovation labor bookkeeping if needed.

## 7. Morale/health/comfort consumers audit (§3.9)

| Consumer | Safe hook | Owner |
|---|---|---|
| Morale | `ShelterDecorSystem.GetRoomMoraleDelta` precedent → NeedsSystem.Modify per occupant | NeedsSystem (morale tick) |
| Warmth/rest | `ShelterThermalSystem.GetRoomWarmthModifier` → NeedsSystem Warmth ×24h | Thermal + Needs |
| Frostbite/affliction | `OnFrostbiteRisk(roomId, survivorId)` event | host → Medical |
| Treatment | MedicalWard / `room_clinic` power criticality | Medical systems |
| Food prep | KitchenNutritionSystem + ventilation `kitchen` valve | KitchenNutrition |
| Contamination | WaterTreatment exposure events, DecontaminationSystem | Disease/Needs |
| Social | CohortSystem / MoralChoice (out of minimal scope) | — |

Renovation effects (§29C.21) must map onto this table only — e.g. Insulated Bunks →
warmth modifier + morale delta; Proper Ward → treatment/hygiene via MedicalWard hooks;
Real Kitchen → KitchenNutrition + ventilation valve efficiency. Stacking ownership
documented later in `RENOVATION_EFFECT_PROVENANCE.md` (Phase 7+).

## 8. Verdict for Phase 5

- Categories 1–3 are consumed as-is; **no new state**.
- Category 4 is exactly the three channels of §3, subject to the §4 gate.
- The §2 shielding overlap must be resolved (documented decision) before ANY wear,
  quirk, or renovation effect references shielding — including Task 29B machine tells
  that imply shielding state.
