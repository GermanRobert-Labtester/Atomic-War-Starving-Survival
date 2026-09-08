# ASHFALL Distress Rescue Missions — Balance, Deadlines & Travel Pacing

> **Document Status:** Authoritative Balance Analysis & Pacing Document
> **Subsystem:** Radio Distress Missions / Expedition System
> **Authority:** Flagship Integration Plan (Task 5)

---

## 1. Travel vs. Deadline Budget

A rescue mission is balanced so that a prompt expedition sortie dispatched upon signal identification arrives with a tight but achievable margin, forcing meaningful player prioritization between routine scavenging and life-saving intervention.

### Pacing Breakdown

| Mission ID | Target Destination | One-Way Ticks | Round-Trip Ticks | Trace Window (Days) | Rescue Deadline (Days) | Slack Margin (Days at 4 Ticks/Day) |
|---|---|---|---|---|---|---|
| `quest_distress_trapped_mechanic` | `loc_recovery_yard` | 6 ticks | 12 ticks | 3 | 5 | ~2.0 days |
| `quest_distress_injured_trader` | `rural_gas_station` | 3 ticks | 6 ticks | 2 | 3 | ~1.5 days |
| `quest_distress_family_shelter` | `family_bunker_backyard_shed` | 2 ticks | 4 ticks | 2 | 4 | ~3.0 days |
| `quest_distress_raider_trap` | `loc_denial_cut_substation` | 8 ticks | 16 ticks | 3 | 5 | ~1.0 days |
| `quest_distress_military_patrol` | `checkpoint_kilo_armory` | 6 ticks | 12 ticks | 3 | 5 | ~2.0 days |

---

## 2. Danger vs. Reward Curves

1. **Scavenge / Low Risk Tier (Danger 1–3):**
   - *Injured Trader* (`rural_gas_station`, Danger 3): Short travel, modest rewards (`bandage`, `battery`, `iodine_pills`, +3 Rep). Accessible in early campaign.
   - *Family Shelter* (`family_bunker_backyard_shed`, Danger 2): Closest target (2 ticks), critical survival items (`clean_water`, `canned_food`, +8 Rep). Moral urgency.
2. **Standard / Moderate Risk Tier (Danger 4–5):**
   - *Trapped Mechanic* (`loc_recovery_yard`, Danger 6): Industrial hazard, medium distance, highly prized crafting materials (`scrap_metal`, `mechanical_parts`, +5 Rep).
   - *Raider Trap* (`loc_denial_cut_substation`, Danger 4): High travel commitment, tactical deception. Tests combat preparedness and scouting caution.
3. **Hazardous / High Risk Tier (Danger 6–8):**
   - *Military Patrol* (`checkpoint_kilo_armory`, Danger 4): Elite military loot (`ammo_556`, `field_dressing_kit`, +10 Rep). High faction standing impact.

---

## 3. Propagation & Atmospheric Gating

- **Atmospheric Interference:** In `FalloutStorm`, `AshLightning`, or `EMPStorm`, signal carrier is heavily attenuated, and noise floor spikes, preventing premature signal lock unless the player is equipped with high-grade antenna arrays or directional loops.
- **Fair Warning:** Raider traps (`quest_distress_raider_trap`) exhibit unnatural consistency in loop cadence and lack subtle human distress variance, rewarding attentive operators who inspect message fragments before dispatching valuable personnel into an ambush.
