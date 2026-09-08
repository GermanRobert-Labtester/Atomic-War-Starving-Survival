# Plan 112 vector contract

## Supported vectors

| Literal | Runtime enum | Protocol | Protocol duration |
|---|---|---|---:|
| `water` | `DiseaseVector.Water` | purification | 3 days |
| `air` | `DiseaseVector.Air` | sealed vents | 2 days |
| `blood` | `DiseaseVector.Blood` | sterilised tools | 5 days |
| `spore` | `DiseaseVector.Spore` | air filtration | 4 days |

No `food`, `contact`, or `fecal_oral` literal is supported by the active
loader. Food and fecal-oral concepts are represented through the existing
water vector; contact-only concepts are not added to the contagious catalog.

## Semantics

- A vector protocol blocks exposure and autonomous spread for that vector while
  active. The protocol is camp-wide state, not a patient treatment.
- Protocol costs are owned by `DiseaseProtocolHandler`; the disease row's
  `countermeasure_item_id` does not create a new protocol.
- `spread_interval_days` schedules an attempt. `spread_radius` limits selected
  candidates from the roster supplied by the host.
- Quarantine, immunity, deterministic RNG, and source probability checks remain
  in `DiseaseSystem`.

## Compatibility

The four additions use `water`, `air`, `blood`, and `spore` exactly. No loader
fallback is relied on, and no vector enum or protocol code changed.
