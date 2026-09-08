# Phantom Background Inventory

## Authority and coverage

`Assets/StreamingAssets/Data/survivors.json` is the active survivor
definition authority used by `SurvivorsHostSession`. It contains 129 survivor
definitions. `expansion_survivor_fields.json` enriches selected definitions
with an explicit `phantom_background_id`.

Before Plan 111, the active host's effective projection covered 49/129
survivor definitions with a specific phantom background (**38.0%**). The
remaining 80 definitions used `generic`.

After the authorized host bridge and data expansion, 66/129 definitions have a
specific profile (**51.2%**) and 63 remain on safe generic fallback.

| Final background | Source of reachability | Effective definitions |
|---|---|---:|
| `child_refugee` | child/orphan/refugee profession mapping and existing enrichment | 7 |
| `former_soldier` | soldier/gunner/artillery/guard/military mapping and enrichment | 13 |
| `nurse` | nurse profession and existing enrichment | 8 |
| `teacher` | teacher/professor/instructor mapping and enrichment | 2 |
| `electrician` | electrician/lineman mapping and enrichment | 4 |
| `machinist` | machinist/mechanic/technician mapping and enrichment | 8 |
| `farmer` | farmer/botanist/agronomist/grower/composter mapping | 2 |
| `engineer` | engineer profession mapping | 2 |
| `laborer` | laborer/foreman/worker mapping | 1 |
| `architect` | architect/structural engineer/builder mapping | 2 |
| `chemist` | chemist/pharmacist/radiochem mapping | 1 |
| `medic` | paramedic/surgeon/doctor/medic/orderly mapping | 5 |
| `driver` | courier/driver/navigator/convoy mapping plus `the_courier` enrichment | 1 |
| `cleric` | priest/preacher/monk/cleric/cult mapping | 1 |
| `scavenger` | scavenger/scrap/nomad mapping | 3 |
| `cook` | cook/chef mapping | 1 |
| `radio_operator` | radio/telegraph/telecomm/sonar mapping | 2 |
| `miner` | miner/mining/speleologist/cave/tunnel mapping plus enrichment | 1 |
| `librarian` | librarian/archivist/historian/bureaucrat mapping | 2 |
| `generic` | unknown or intentionally unclassified background | 63 |

The two enrichment changes are deliberately narrow:

- `the_courier`: `generic` → `driver`;
- `survivor_speleologist`: `generic` → `miner`.

They preserve enrichment precedence while making two real profiles reachable.
No survivor definition, roster ID, or save field was added.

## Substitutions

- `urban_survivor` was removed because it was not a live host output.
- `carpenter` was not present as an active phantom background authority.
- `architect` is the real roster-backed structural replacement.
- `cleric` is the normalized phantom profile for existing priest, preacher,
  monk, and cult professions.

## Host change

The only runtime bridge is in the Godot host:

1. `Main.SetupPhantom` loads the existing enrichment catalog;
2. `PhantomMemoryHostSession.BindSurvivors` receives it;
3. the existing profession fallback is extended with the final profile
   vocabulary.

Core behavior, save state, and item matching remain unchanged.
