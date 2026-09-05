# Plan 85 — Balance Matrix

## Fragment economy (§85E.5–85E.6)

- **32 fragments** across 12 zones (2–3 per zone), spread over **23 of 49** scavenging tables at weight 2.
- A loot roll fires per Looting tick with chance `0.5 + danger×0.05`; a fragment entry is then a ≈0.8–3% pick within its table. Expected fragments per sortie at a matching location: ≈0.05–0.2 — discovery stays an event, not a drip.
- No inventory burden (fragments are not items); no duplicate clutter (tokens resolve to nothing after registration).
- **Early cadence:** danger-2/3 destinations (farm, school, clinic, forestry, apartment, fire_station, veterinary) expose 15 fragments. **Mid:** relay_mast, tank_farm, metro_station, police_station, printworks, observatory, rail_yard, checkpoint, archive, hunting_cabin expose 14. **Late:** dead_hand_core + government_bunker gate the crater zone (3 fragments) behind ground-zero access.
- Completionist reachability: every producer is a live Plan 46 table bound to at least one authored destination — no zone requires a source the player never sees. Verified by `Catalog_EveryFragment_HasScavengingProducer` + destination table-reference gates.

## Risk / reward (§85E.7)

| Installation | Dest danger / ticks | Travel risk | Reward band |
|---|---|---|---|
| Municipal Seed Vault / Quarantine Barn | 2 / 8–12 | low | seeds, tools |
| Sealed Triage Annex / Evidence Sub-Basement | 3 / 8–9 | low | consumables / documents |
| Forestry Emergency Store | 3 / 16 | low, long walk | field kit |
| Old Medical Quarter's Annex neighbors | — | — | — |
| Underground Fuel Depot / Materials Research Sublevel / Hidden Relay Bunker 09 | 4 / 11–13 | moderate | repair, precision, comms |
| Dead-Drop Command Shelter / Electrical Maintenance Exchange | 5 / 15–21 | moderate-high | utility/electrical |
| Blacksite Armory 7 | 6 / 18 | high | comms + bounded ammo (pre-existing zone) |
| Collapsed Command Vault | 7 / 14 | highest | military comms/rations |

Higher-value sites sit behind higher danger and longer routes; the two ammunition lines are pre-existing zones (military_corridor, crater_ground_zero) and were not extended. No new zone grants ammunition, medicine jackpots, or fuel loops (single canister tier at most).

## Exploit suite status (§85E.8)

| Exploit | Status |
|---|---|
| Duplicate fragment double-counts | impossible — set registration (tested) |
| Completion/reveal re-fires on reload | impossible — edge-trigger + idempotent Discover (tested) |
| Reload before final fragment to re-roll its drop | not an exploit — rolls are seeded; identical stream replays |
| Reload to reroll site loot | not possible — site loot is stateless; each sortie is a fresh, legitimately risky roll |
| Revisit revealed installation for jackpot | bounded — ordinary loot-loop quantities per sortie |
| Sell/drop fragment to lose progress | impossible — knowledge, not possession |
| One-shot quest producer hard-lock | none used |
