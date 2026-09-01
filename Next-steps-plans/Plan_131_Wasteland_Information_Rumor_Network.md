# Plan 131 — Wasteland Information & Rumor Network

## Goal

Create a persistent information-flow system where news, rumors, and intelligence propagate between settlements, factions, and the player over time. Players learn about distant events through travelers, radio intercepts, and faction contacts, creating information asymmetry and strategic decision-making. This transforms the world from isolated locations into a connected information ecosystem.

## Why

**Repository evidence:** Zero information/rumor/intelligence systems exist. No `RumorSystem.cs`, no `intelligence.json`, no data files for information flow. `MoralChoiceGossipRuntime` handles intra-shelter gossip but doesn't propagate information between locations or factions. `TravelingCaravanSystem` (268 lines) moves goods but doesn't carry news. `WeatherStationSystem` provides forecasts but doesn't broadcast warnings to other settlements.

**What is missing:** Players have no way to learn about faction movements, location changes, economic shifts, or threats beyond their immediate vicinity. The world feels disconnected. A settlement being raided 50 miles away produces zero ripple effects.

**Why existing plans don't solve it:** Plans 50/107 (radio distress signals) handle emergency broadcasts, not ongoing information flow. Plan 73 (faction radio corpus) adds radio content but not propagation mechanics. Plan 24 (radio signals/airwaves) is audio production, not information systems. No plan addresses inter-settlement information propagation.

**Player value:** Creates strategic depth (act on intel before others), role-playing opportunities (information broker), and a living world where events have visible ripple effects.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/TravelingCaravanSystem.cs` — extend to carry rumors
- `Assets/Ashfall.Core/Factions/` — faction information networks
- `Assets/Ashfall.Core/LocationEvolutionSystem.cs` — location events generate rumors
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipRuntime.cs` — gossip precedent
- `Assets/StreamingAssets/Data/locations.json` — settlement information hubs
- `Assets/StreamingAssets/Data/faction_lore.json` — faction information networks
- NEW: `Assets/Ashfall.Core/InformationFlow/RumorSystem.cs`
- NEW: `Assets/StreamingAssets/Data/information_networks.json`

## Main Task 1 — Foundation / System Contract

1. Create `Assets/Ashfall.Core/InformationFlow/` namespace directory
2. Define `Rumor` DTO: `id`, `originLocationId`, `originDay`, `subjectType` (faction/location/event/economy), `subjectId`, `truthfulness` (0-1), `decayRate`, `propagationSpeed`, `tags`
3. Define `RumorNetwork` DTO: list of active rumors, list of information hubs (locations/factions with propagation capability)
4. Create `RumorSystem.cs` with `CaptureState/RestoreState`, seeded RNG for truthfulness/propagation
5. Define propagation rules: rumors decay over time, mutate as they spread (truthfulness decreases), blocked by distance/weather/faction hostility
6. Create `InformationHub` concept: locations/factions with `propagationRadius`, `bias` (what they emphasize/suppress), `credibility`
7. Define `RumorVariant` system: same event produces different rumors based on source bias
8. Create `IRumorSource` interface for systems that generate rumors (LocationEvolution, FactionWar, Economy, Expeditions)
9. Implement rumor aging: each tick, rumors decay, mutate, propagate to connected hubs
10. Create `RumorCatalogLoader` for static rumor templates (event → rumor generation rules)
11. Wire into `GameBootstrap` partial: `SetupInformationFlow`, `TickRumors`, `SaveRumors`
12. Add save section to `SaveSectionRegistry` for rumor state
13. Create deterministic propagation: same seed → same rumor spread pattern
14. Implement rumor access API: `GetRumorsAtLocation(locationId)`, `GetRumorsAboutFaction(factionId)`

## Main Task 2 — Implementation / Content / Propagation

1. Implement rumor generation from `LocationEvolutionSystem` events (location captured, ruined, cleared)
2. Implement rumor generation from `FactionWarSystem` events (territory change, battle, treaty)
3. Implement rumor generation from economy events (price spike, shortage, trade route established)
4. Implement rumor generation from expedition discoveries (resource found, threat encountered)
5. Create `TravelingCaravanSystem` integration: caravans carry rumors between locations
6. Create radio intercept mechanic: player can build radio receiver to intercept faction communications
7. Implement information hub behavior: settlements amplify/suppress rumors based on bias
8. Create rumor mutation: as rumors spread, details change (numbers shift, blame assigned, outcomes exaggerated)
9. Implement player reputation from rumor exposure: locations react to rumors player has heard
10. Create information broker role: player can sell rumors to factions for standing/rewards
11. Implement rumor verification: player can send expeditions to verify rumors (cost vs. benefit)
12. Create false rumor mechanic: factions deliberately spread disinformation
13. Implement rumor decay: old rumors fade, replaced by newer information
14. Add UI: "Rumor Board" panel showing current rumors at player's location
15. Create 20 starter rumor templates in `information_networks.json`

## Main Task 3 — Integration / Consequences / Validation

1. Wire rumor consumption into faction standing: acting on rumors affects faction reactions
2. Connect to quest system: rumors unlock optional quest objectives
3. Integrate with economy: rumors about shortages affect local prices
4. Connect to expedition system: rumors reveal hidden expedition destinations
5. Implement old-save compatibility: existing saves get default empty rumor state
6. Add deterministic seeding: rumor propagation uses `ISeededRng`, not `System.Random`
7. Create exploit prevention: rumors have cooldowns, can't be farmed
8. Add tests: rumor propagation determinism, save/load round-trip, decay behavior
9. Verify catalog integrity: all rumor subject IDs resolve to real locations/factions/events
10. Test edge cases: isolated locations (no rumors), faction hostility blocking information
11. Verify headless behavior: rumor system ticks correctly in `--headless` mode
12. Add data-integrity-selftest check: rumor templates validate against catalog
13. Document information flow architecture for future expansion
14. Create `--information-flow-selftest` verb for CI validation

## State / System Interaction Model

```text
Location event (raid, discovery, treaty)
├─ generates rumor at source location
│  ├─ propagates to connected locations via caravans/radio
│  │  ├─ mutates (truthfulness decays, details shift)
│  │  └─ amplified/suppressed by hub bias
│  └─ player at source location learns truth
├─ player at distant location learns mutated version
│  ├─ can act on rumor (quest unlock, trade decision)
│  │  ├─ success if rumor was accurate
│  │  └─ failure if rumor was false/mutated
│  └─ can verify rumor (expedition cost)
└─ faction spreads disinformation
   ├─ player acts on false rumor
   │  └─ faction gains advantage, player loses resources
   └─ player discovers deception
      └─ faction standing penalty, future rumors from faction distrusted
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --information-flow-selftest
```

## Risk

**MEDIUM** — Rumor propagation complexity can spiral. Must keep mutation rules simple and deterministic. Risk of information overload if too many rumors propagate simultaneously. Mitigation: cap active rumors, decay old rumors aggressively.

## Definition of Done

- `RumorSystem.cs` exists with full `CaptureState/RestoreState`
- Rumors propagate between locations via caravans and radio
- Rumors mutate and decay over time
- Player can access rumors at current location via UI
- Rumors unlock quest objectives and affect economy
- Factions can spread disinformation
- Save/load round-trip tested
- Deterministic propagation verified (same seed → same spread)
- Old saves load without error (empty rumor state)
- Data integrity selftest passes with rumor templates
- Headless mode ticks rumors correctly
- 20 starter rumor templates in data authority

## Follow-On Opportunities

- Information broker specialization (survivor skill tree)
- Encrypted communications (counter-disinformation)
- Propaganda campaigns (faction-wide disinformation operations)
- Information black markets (buy/sell verified intelligence)
- Refugee information (refugees carry rumors from fallen locations)
