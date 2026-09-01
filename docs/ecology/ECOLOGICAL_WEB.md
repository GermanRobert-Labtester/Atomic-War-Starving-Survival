# ECOLOGICAL_WEB.md — Plan 28 Task 28Z

Explicit ownership before consequences. Node types: **[system]** live code,
**[content]** authored data/state, **[market]** MarketSystem good,
**[encounter]** expedition/combat table.

## Chain 1 — grain/seed → rodent/vermin → predator

```
crop/grain stores [system: shelter pantry state]
  → BurrowSwarm abundance ↑ (burrowers drift toward grain country: lowlands/floodplain)
  → sector pack population rises
  → trapping nuisance catch ↑ (existing densityMultiplier)
  → and predator packs follow the same graph when hungry [encounter weighting, existing]
```
- Producer: pack seed state + seasonal abundance (Plan 28 calendar).
- Bounded effect: sector population clamp (2× seed ceiling), snare density clamp [0.4, 1.5].
- Consumer: trapping + predator encounter weighting.
- Termination: swarm thins in hard cold (hunger 1.2–1.5×), population floors at 0.
- **No new population state.** The shared signal is `GetSectorPackPopulation`.

## Chain 2 — carrion → scavenger → clue

```
failed expedition / predator kill [encounter outcome, live]
  → scavenger birds feed openly (PassageFlock sector presence)
  → circling/feeding sign = opportunity clue [content: field-guide + radio wording]
  → outcome rolls: carcass salvage | predator encounter | nothing
```
- Status: scavenger *encounters* are live (expedition engine, threat_wild_beasts);
  the carrion-loot clue chain is authored event content (28AB) — **deferred to the
  expedition content pass**, zero new systems required.

## Chain 3 — coastal forage → fish run → coastal harvest & market

```
Thaw/Bloom season (Plan 19) [system]
  → CoastalRunner hunger factor 0.7–0.8 + water-bound corridor (Plan 28)
  → carp/heron population recovers toward 2× seed ceiling (existing birth rule)
  → sector pack population rises in river/estuary
  → trapping density + radio fish-run notices (Plan 28, live)
  → preserved-protein demand relief via existing scarcity chain
```
- No marine simulation: the "run" is the pack's bounded birth ceiling reached on fed water.
- Market read: `EvolvingWorldDayOwner` demand delta (live) — abundance >1.2 eases demand.

## Shared-signal map (what actually communicates today)

| Signal | Producer | Consumers |
|---|---|---|
| sector pack population | WildlifeMigrationSystem | trapping density, expedition composer |
| global population ratio | WildlifeMigrationSystem | expedition danger, market demand |
| isRabid/aggression | Live tick | expedition danger, hazard warnings |
| sector id | WildlifeMigrationSystem | corridors, notices, water filter |
| season window | WeatherSystem (Plan 19) | WildlifeSeasonalCalendar (Plan 28 reader) |
| location threats/contamination | LocationEvolutionSystem | expedition danger composer |

**Nothing duplicates population state.** The web is: one pack ledger (Core), one season
authority (weather_seasons.json), consumers reading projections.
