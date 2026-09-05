# Weather-Gate Encounter Interaction (F12)

How travel-encounter eligibility stays physically consistent with the
traversable world.

## Selection context

`TravelEncounterSelectionContext` carries everything eligibility needs:

| Field | Meaning |
|---|---|
| `Region` | region tag the encounter is tagged with |
| `DangerLevel` | current danger level |
| `Stance` | faction stance |
| `CurrentSeason` | season tag |
| `CurrentDay` | campaign day |
| `CurrentWeather` | weather kind for this selection |
| `Rng` | explicit `ISeededRng` — never a global singleton |

Weather is **passed in**, never read from a global singleton inside
`IsEncounterEligible`, so the same context always yields the same answer.

## Route↔region association contract

- Authority: `region_route_topology.json` (`region_tag` → `route_targets`).
- Fallback when the file is absent: the mapping is derived from the gate
  catalog — every route gate target belongs to every region that has at
  least one encounter tagged with that region.
- Unknown region tags **preserve existing encounter eligibility** (fail-open
  at the topology level). Weather gating itself stays strict per gate.

## Suppression semantics

> Weather suppresses a region-tagged travel encounter only when the route
> association that makes that encounter reachable is weather-blocked and no
> alternative mapped route for that region remains traversable.

Suppression is **soft**: it happens before weighted selection, and it never:

- sets a cooldown;
- marks the encounter seen;
- advances the encounter chain;
- decrements charges;
- consumes an occurrence;
- consumes RNG.

## Order of operations

1. Build the candidate list from the catalog.
2. Per candidate: cooldown → danger range → region tag → season → chain
   stage → **weather suppression** (route-aware).
3. Weight the survivors and roll `Rng` once.

Because suppression runs before weighting, a weather-blocked encounter
consumes no randomness: adding or removing suppressed encounters never
perturbs unrelated random outcomes.

## Positive-gate behaviour

When a positive gate opens (e.g. the frozen-lake crossing during sustained
deep cold), encounters tied to the newly traversable region become eligible
through the topology lookup. Opening the route never automatically forces
an encounter: eligibility only enters the candidate pool, and ordinary
filters plus the weighted roll still decide.

Creature encounters on non-gated routes are unaffected: they have no route
association, so weather suppression never touches them.

## High Scarp example

Blizzard, `enc_patrol_garrison_checkpoint`, region `high_scarp`,
mapped route `route_12` (negative Blizzard blocker):

- all `high_scarp` routes blocked, no alternative traversable ⇒ suppressed;
- cooldown unchanged, chain unchanged, no RNG consumed.

Clear weather, same encounter:

- not suppressed; eligible if cooldown/danger/season/chain filters pass.

`enc_patrol_warlord_raid` in `the_toll` during a Blizzard: its region has
no Blizzard-blocked mapped route, so it remains eligible subject to
ordinary filters. A global "Blizzard disables patrols" rule never exists.

## Black Rain

When BlackRain blocks a route mapped into a region, route-linked travel
encounters there become ineligible; unaffected regions stay eligible.
Tests use actual catalog mappings, never invented route ids.

## RNG invariants

- Eligibility checks based on deterministic state run **before** selection.
- A weather-blocked encounter never reaches the weighted roll.
- Same context ⇒ same eligibility, same weights, same roll.

## Extension checklist (new region tags)

1. Add the region to `region_route_topology.json` (or rely on the derived
   fallback) with its `route_targets`.
2. Tag encounters with the region tag in the encounter catalog.
3. If the region has weather-gated routes, the gate catalog already covers
   them — no encounter-side change.
4. Add a suppression test: every mapped route blocked ⇒ suppressed; ≥1
   traversable ⇒ eligible.
