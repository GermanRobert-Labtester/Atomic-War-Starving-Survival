# Final Wish Expedition Handoff Integration

**Document:** `docs/survivors/FINAL_WISH_EXPEDITION_HANDOFF.md`

---

## 1. Expedition Destination Connections (4 Wired Wishes)

Four wishes require dispatching an expedition to a canonical, reachable wasteland destination from `locations.json`:

| Wish # | Archetype | Destination ID | Node Description | Expedition Task |
|---|---|---|---|---|
| **12** | `the_courier` | `loc_settlement_cape_beacon` | Coastal outpost | Deliver the Fort Karkov waybill to the station master |
| **14** | `the_blind_preacher` | `location_ash_dune_cemetery` | Burial dunes | Place the funeral collect under the central cairn stone |
| **15** | `the_misanthrope` | `loc_settlement_cape_beacon` | Coastal headland | Escort survivor to stand before the ocean swell |
| **16** | `the_botanist` | `loc_terrace_pumphouse` | Ruined terrace | Accompany survivor to inspect surviving stone flora |
| **28** | `the_burglar` | `loc_shrine_switchback_waystation` | Mountain shrine | Return the stolen tarnished medal to the stone niche |

---

## 2. Balance & Reachability

All four destinations are within early-to-midgame expedition reach:
- `loc_terrace_pumphouse`: Immediate shelter perimeter (Low hazard, 0.5-day foot journey).
- `location_ash_dune_cemetery`: Sector 2 margin (Moderate dust hazard, 1-day journey).
- `loc_shrine_switchback_waystation`: Foothill route (Moderate hazard, 1.5-day journey).
- `loc_settlement_cape_beacon`: Coastal terminal (Settlement waypoint, safe arrival condition).

None require endgame radioactive hotspots or impenetrable locked vaults.
