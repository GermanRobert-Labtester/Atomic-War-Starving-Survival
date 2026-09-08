# Faction War Override Campaign Timeline

> **Timeline Range:** Day 1 through Day 600+
> **Focus:** World-state evolution during the active Faction War

---

## 1. Macro Campaign Epochs

The 20 location overrides distribute across 5 major narrative and operational epochs of the post-exchange wasteland:

```
[Day 1 ................... 100]  Epoch 1: Post-Exchange Aftermath & Ration Bureaucracy
[Day 101 ................. 200]  Epoch 2: First Shelling & Displaced Influx
[Day 200 ................. 300]  Epoch 3: Faction War Mobilization & Infrastructure Raids
[Day 300 ................. 400]  Epoch 4: Tactical Denial, Scorched Earth & Early Recovery
[Day 480 ................. 600+] Epoch 5: Late-War Signal Espionage & Year of Ash Climax
```

---

## 2. Epoch-by-Epoch Narrative Progression

### Epoch 1: Post-Exchange Aftermath & Ration Bureaucracy (Days 1–100)
- **Plaza (Days 1–50):** `loc_override_plaza_cleared` portrays orderly ration lines with civil guards and municipal tickets.
- **Almshouse (Days 1–100):** `loc_override_almshouse_pre_strike` shows crowded but intact dormitories distributing hot turnip gruel.

### Epoch 2: First Shelling & Displaced Influx (Days 101–200)
- **Almshouse (Days 101–300):** `loc_override_almshouse_post_strike` collapses under artillery bombardment; parish becomes a ruin.
- **Waystation (Days 150–280):** `loc_override_waystation_refugee` overflows with displaced civilians fleeing lowlands for the mountain ridges.

### Epoch 3: Faction War Mobilization & Infrastructure Raids (Days 200–300)
- **Grain Silo (Days 200–350):** `loc_override_silo_fortified` converted into a hardened defensive strongpoint with steel cladding.
- **Checkpoint Gamma (Days 200–250):** `loc_override_checkpoint_occupied` fortified by Garrison regulars enforcing heavy travel permits.
- **Conscription Bureau (Days 220–340):** `loc_override_conscription_burned` destroyed in anti-draft riots.
- **Crossing Granary (Days 220–260):** `loc_override_granary_burned` torched in a punitive border raid, spoiling grain stores.
- **Municipal Reservoir (Days 240–280):** `loc_override_well_contaminated` poisoned by oily chemical runoff and soot.
- **North Weighbridge (Days 250–380):** `loc_override_weighbridge_barricaded` fortified with timber cribbing and double barricades.
- **Sector 4 Switchyard (Days 260–320):** `loc_override_rail_yard_fortified` turned into a military supply transfer redoubt.
- **Iron Siding (Days 280–340):** `loc_override_village_abandoned` completely deserted as coal deliveries halt and water freezes.

### Epoch 4: Tactical Denial, Scorched Earth & Early Recovery (Days 300–400)
- **Cache Delta (Days 300–450):** `loc_override_cache_looted` breached and scavenged by heavy salvage crews.
- **Chemical Plant (Days 300–350):** `loc_override_factory_occupied` seized by militia guarding solvent distillation towers.
- **Bridge Seven (Days 310–360):** `loc_override_bridge_destroyed` dropped into the riverbed by retreating sappers.
- **Deadfall Barrier (Days 330–370):** `loc_override_roadblock_liberated` reclaimed by locals after militia withdrawal.
- **Petition Tent (Days 340–380):** `loc_override_camp_overrun` abandoned and trampled after winter food runs out.
- **Metro Station (Days 350–400):** `loc_override_station_reclaimed` reoccupied by organized survivor pump crews.
- **Burned Woodland (Days 360–400):** `loc_override_field_scorched` swept by secondary phosphorus shelling and defoliation.

### Epoch 5: Late-War Signal Espionage (Days 480–600+)
- **Understory Transmitter (Days 480–600):** `loc_override_understory_transmitter_ambient` re-crewed by signal operators monitoring military broadcasts.

---

## 3. Concurrency Density

At peak conflict (e.g. Day 345):
- `loc_override_cache_looted` (Active 300–450)
- `loc_override_bridge_destroyed` (Active 310–360)
- `loc_override_roadblock_liberated` (Active 330–370)
- `loc_override_camp_overrun` (Active 340–380)

Up to 4–6 locations exhibit simultaneous temporal overrides, creating a living, shifting front across multiple wasteland zones without performance overhead.
