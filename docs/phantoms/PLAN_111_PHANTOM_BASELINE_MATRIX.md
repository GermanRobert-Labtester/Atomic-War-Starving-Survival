# Plan 111 Phantom Baseline Matrix

The repository already contained 11 entries when Plan 111 was reconciled.
The historical seven-entry statement was stale: four profiles had already
landed (`farmer`, `engineer`, `laborer`, and `urban_survivor`).

| Baseline entry | Trigger count | Existing memory lens | Plan 111 disposition |
|---|---:|---|---|
| `child_refugee` | 4 | childhood objects, displacement, family absence | preserved |
| `former_soldier` | 5 | military tags, medals, field supplies, duty | preserved |
| `nurse` | 4 | bedside tools, patient records, ward loss | preserved |
| `teacher` | 3 | lessons, children, classroom routine | preserved |
| `electrician` | 1 | tester, grid repair, substation loss | preserved |
| `machinist` | 2 | measurement, tolerance, workshop failure | preserved |
| `farmer` | 2 | harvest records, seed continuity, drought | preserved |
| `engineer` | 2 | systems, schedules, structural responsibility | preserved |
| `laborer` | 2 | shift signals, crew absence, industrial routine | preserved |
| `urban_survivor` | 2 | apartment key, ordinary city routine | replaced |
| `generic` | 10 | broad personal, photograph, correspondence, ordinary-object memories | preserved fallback |

## Baseline observations

- Existing motivation chances ranged from 0.20 to 0.50.
- Existing categories were the runtime taxonomy, not profession names:
  `childhood`, `photograph`, `correspondence`, `personal_item`, `military`,
  `medical`, `work_tool`, and `ordinary_object`.
- Existing prose frequently used personal failure or loss. New profiles add
  professional routine, preservation, uncertainty, institutional memory, and
  non-culpable grief.
- `urban_survivor` was not produced by the active host's background mapper or
  enrichment authority. It was replaced with roster-backed `architect`, not
  silently retained as dead content.

## Final additions

The final nine new specific profiles are:

```text
chemist
medic
driver
cleric
scavenger
cook
radio_operator
miner
librarian
```

`architect` is the roster-backed substitution for the stale
`urban_survivor`/candidate `carpenter` concept. The final catalog therefore
contains 19 specific profiles plus one `generic` fallback.
