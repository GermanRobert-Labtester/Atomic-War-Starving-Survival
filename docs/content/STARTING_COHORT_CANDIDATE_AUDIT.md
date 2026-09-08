# Starting Cohort Candidate Audit

## Safe selection rule

Only canonical definitions that resolve in `survivors.json`, have no active
survivor questline, and have no current evidence of a required later
recruitment or faction position may enter the flagship catalog. The loader
also rejects active questline references mechanically.

## Selected safe candidates

| ID | Canonical display | Profession | Use |
|---|---|---|---|
| `jamie_chen` | Jamie Chen | Mechanic | Repair Crew |
| `casey_garcia` | Casey Garcia | Engineer | Repair Crew |
| `hayden_reyes` | Hayden Reyes | Builder | Repair Crew |
| `taylor_morgan` | Taylor Morgan | Farmer | Growers & Stewards |
| `sage_green` | Sage Green | Botanist | Growers & Stewards |
| `reese_flores` | Reese Flores | Cook | Growers & Stewards |
| `alex_raymond` | Alex Raymond | Scout | Convoy Remnant |
| `morgan_lee` | Morgan Lee | Scavenger | Convoy Remnant |
| `drew_paterson` | Drew Paterson | Guard | Convoy Remnant |
| `jordan_kim` | Jordan Kim | Teacher | Civilian Improvisers |
| `rowan_king` | Rowan King | Storyteller | Civilian Improvisers |
| `elliot_bennett` | Elliot Bennett | Carpenter | Civilian Improvisers |

The legacy Sarah, Mikhail, and Elena records remain the Standard Holdfast
members and are not duplicated as alternate definitions.

## Excluded classes

- `the_*` named quest survivors: each has a survivor-specific
  `activeQuestlineId` and is encountered through later content.
- `aris_thorne`, `maya_lin`, `victor_vance`, and `elena_rostov`: named
  narrative entrants with active questlines.
- `survivor_*` expansion/cold-storage records: retained for their authored
  introduction paths, not day-zero generic recruitment.
- family, child, prisoner, cult, faction, military, and other special-role
  records: their presence changes an authored event premise or eligibility.

No new survivor definition is created by Plan 138.
