# Foundry Treaty Consequence Balance Audit

**Catalog:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`

## Severity bands

| Outcome | Standing range in catalog | Demand behavior |
|---|---:|---|
| `met` | +2 to +4 | bounded relief or no market change |
| `missed` | -5 to -6 | recoverable scarcity pressure |
| `violated` | -8 to -14 | stronger scarcity pressure and institutional memory |

The largest new penalty is Crisis Mutual Aid `-14`; it remains below the
existing stance raid threshold by itself and is not a war trigger.

## New-row audit

| Treaty | Outcome | Standing | Market deltas |
|---|---|---:|---|
| Saltworks Access | `met` | +3 | water -0.20; brine pipe -0.15 |
| Saltworks Access | `violated` | -10 | water +0.35; filter +0.25 |
| Coal Window | `met` | +3 | coal -0.25; fuel -0.15 |
| Coal Window | `missed` | -5 | coal +0.30; fuel +0.15 |
| Membrane Repair | `met` | +4 | brine pipe -0.20; water -0.15 |
| Membrane Repair | `violated` | -12 | brine pipe +0.35; filter +0.35 |
| Crisis Mutual Aid | `met` | +4 | water -0.20; fuel -0.20 |
| Crisis Mutual Aid | `violated` | -14 | water +0.40; fuel +0.40 |
| Incident Book | `met` | +2 | no market modifier |

The Incident Book is intentionally standing-only: reporting is governance,
not a fabricated commodity transfer. No row directly adds inventory,
contamination, access, or production state.

## Exploit checks

- No new repeat reward path exists; new policies are static lookups.
- No `met` row reverses a different treaty's standing or state.
- No new penalty permanently blocks water, fuel, or production.
- Market demand remains subject to `MarketSystem` clamps.
- New rows do not add randomness or alter save state shape.
