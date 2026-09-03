# Plan 76 — Balance Audit (Distance / Danger / Stamina / Encounter Pressure)

Scope: the 53 authored `expeditions.json` destinations. Model = actual runtime
(`ExpeditionSystem`): an encounter roll happens **every tick hour** of the sortie
(outbound + looting + inbound); Stealth halves the per-tick chance; a standard
sortie is `2 × distanceTicks + 3` ticks; 1 tick = 0.5 h; stamina capacity 100.

```text
P(≥1 encounter, stealth round trip) = 1 − (1 − p/2) ^ (2·distance + 3)
stamina round trip (unencumbered)   = baseStaminaDrainPerHour × 0.5 × (2·distance + 3)
```

## 1. Distribution summary

| Metric | Min | Max | Mean | Read |
|---|---:|---:|---:|---|
| distanceTicks | 2 | 18 | — | near quick-runs → endgame long hauls |
| dangerLevel | 2 | 10 | — | full authored range in use |
| P(≥1 enc., stealth RT) | 0.362 | 0.998 | 0.714 | no pathological spam; scales with tier |
| stamina RT (unencumbered) | 6.6 | 78.0 | — | worst case (`location_the_dead_hand_core`, 78) fits capacity with headroom for encumbrance only on short hauls — intentional for an endgame site |

## 2. Encounter-pressure tiers (stealth round trip)

| Band | Destinations (examples) | P(≥1 encounter) |
|---|---|---|
| Low (< 0.5) | `loc_settlement_pilgrim_hearth` 0.362, `loc_settlement_tinkers_notch` 0.370, `suburban_house` 0.398, `family_bunker_backyard_shed` 0.398, `loc_grange_hall` 0.442, `loc_water_station` 0.442, `concert_hall_ruins` 0.480, `loc_settlement_brine_pans` 0.494 | starter-safe |
| Medium (0.5–0.75) | `rural_gas_station` 0.528, `loc_school_gymnasium` 0.528, `ruined_garage` 0.600, `loc_grain_silo` 0.600, `loc_conscription_office` 0.613, `old_library_cache` 0.662, `loc_garrison_checkpoint_gamma` 0.707, `abandoned_hospital` 0.722 | routine risk |
| High (0.75–0.9) | `prewar_medical_cache` 0.757, `loc_motel_verity` 0.794, `loc_recovery_yard` 0.826, `loc_diesel_tank_farm` 0.862, `hospital_pharmacy` 0.865, `loc_radio_relay_mast` 0.891, `location_flooded_subway_depot` 0.886 | committed sorties |
| Extreme (> 0.9) | `government_bunker` 0.929, `loc_ordnance_shoulder` 0.912, `location_silent_observatory` 0.987, `location_arcology_sector_4` 0.995, `location_the_dead_hand_core` 0.998 | endgame death-trips |

§45 resolved: no destination combines high per-tick chance with long distance
in a way that produces unavoidable encounter spam beyond its intended tier —
pressure grows with distance **and** tier by design, and the plan's warning
case (`distance 14 + p 0.25`) is represented only by endgame sites whose
danger is the point.

## 3. Distinctiveness (§28/§52)

Numeric near-ties exist (e.g. `ruined_garage` / `collapsed_building`, both
4/3/0.16/2.2) but the §52 dead-destination rule requires *same loot with no
unique hook* — differentiation is carried by the **loot signature** and the
`locations.json` physical identity:

| Signature | Destinations |
|---|---|
| food / water / agriculture | allotments, grange hall, apiary rows, cider press, grain silo, seed library |
| medical | prewar medical cache, hospital pharmacy, veterinary surgery, dentists' row, almshouse |
| electrical / detection | denial cut substation, electrical substation, relay mast, water station |
| mechanical / fuel | rural gas station, ruined garage, recovery yard, tank farm, weighbridge |
| ammunition / military | checkpoint kilo armory, checkpoint gamma, ordnance shoulder |
| household / broad salvage | suburban house, collapsed building, department store, motel, baths |
| administrative / knowledge | municipal archive, printworks, transit HQ, conscription office, library |
| settlement / social | tinkers notch, pilgrim hearth, brine pans, shallows |
| deep / endgame | flooded subway depot, government bunker, geo-thermal ruins, silent observatory, arcology, ministry bunker, dead hand core |

**§51 no universal best:** the numerically most attractive all-rounder
(`suburban_house`: 2 ticks, danger 2) carries a household-only signature; every
high-value signature (medical, ammunition, electrical) sits behind
higher distance and/or danger. **§50 progression:** starter band
(settlements, suburban house) → midgame (gas station, gymnasium, library,
checkpoint gamma) → endgame (depot, arcology, dead hand core) is supported by
the real numbers above.

## 4. Known pre-existing observations (documented, not repaired)

1. **`flooded_subway_depot` dual identity** — `location_flooded_subway_depot`
   (expeditions.json; 7 ticks, danger 7) vs `loc_flooded_subway_depot`
   (locations.json; travelHours 2.0, danger 4) describe the same named site
   under two ids. Old saves may reference either; renaming is a migration
   task, not a pure-data fix. Flagged for a future identity-merge pass.
2. **Prefix drift** — 33 `loc_`, 6 `location_`, 14 bare descriptive ids.
   Cosmetic; no runtime reader depends on the prefix.
3. **`Ministry of Truth Bunker` / `The Dead Hand Core`** naming register is
   lore-adjacent to real-world references; the project's own
   `DataRuleComplianceTests` gate currently accepts them. Flagged for a tone
   review pass.
