# Plan 138 Baseline

## Current authorities

- Canonical survivor definitions: `Assets/StreamingAssets/Data/survivors.json`
  (129 definitions on the audit date).
- Legacy starting roster and initial conditions:
  `Assets/StreamingAssets/Data/starting_survivors.json`.
- Campaign survivor existence and current condition:
  `SurvivorsHostSession` plus the canonical `survivors` save section.
- Campaign identity and persistence:
  `SaveSlotService` and the registry-keyed `campaign.json` envelope.

The legacy file contains exactly three records:

| ID | Health | Hunger | Thirst | Warmth | Morale | Lifetime dose | Acute rad | Joined |
|---|---:|---:|---:|---:|---:|---:|---|---:|
| `survivor_dr_sarah_chen` | 90 | 20 | 25 | 85 | 70 | 14 | false | 0 |
| `survivor_gunner_mikhail` | 80 | 35 | 30 | 75 | 55 | 38 | true | 0 |
| `elena_vasquez` | 95 | 15 | 20 | 90 | 65 | 8 | false | 0 |

These values remain the byte/logical parity target for
`cohort_standard_holdfast`.

## Current lifecycle finding

`StartNewGame()` previously reset in-memory sessions and destructively reset
`slot_1` before composing. This prevented stale state from leaking, but it
also erased the prior campaign, so the old campaign could not be selected
after starting a second run. `ResetAllSessionsInMemory()` itself is
non-destructive and remains so.

Plan 138 changes New Game to allocate the next deterministic free slot. The
old slot-root envelope and projections remain available for explicit restore.

## Candidate audit summary

The selected alternate members are canonical definitions with no
`activeQuestlineId`, no faction-leader role, and no expansion-only entry
requirement in the current catalog:

- repair: `jamie_chen`, `casey_garcia`, `hayden_reyes`
- growers: `taylor_morgan`, `sage_green`, `reese_flores`
- convoy: `alex_raymond`, `morgan_lee`, `drew_paterson`
- civilian: `jordan_kim`, `rowan_king`, `elliot_bennett`

Quest-bearing, named late-entry, faction, captive, child, and special
survivors remain excluded.

## Plan 134 status

The repository's Plan 134 is Dynamic Faction Territory & Supply Lines. It
does not currently provide a day-zero supply/origin selector. Plan 138
therefore adds no supply data and preserves the existing starting-supply
authority unchanged.
