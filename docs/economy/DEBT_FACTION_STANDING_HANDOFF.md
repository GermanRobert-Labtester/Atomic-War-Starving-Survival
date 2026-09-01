# Plan 40 — Faction Standing Handoff

## Standing Integration
Debt default affects faction standing through the existing `FactionWarSystem.ModifyStanding()` API.

## Standing Deltas by Consequence
| Consequence | Delta | Target |
|---|---|---|
| standing_loss_mild | -5 | creditor faction |
| standing_loss_moderate | -12 | creditor faction |
| embargo_trade | -8 | creditor faction |
| standing_loss_and_embargo | -10 | creditor faction |
| bounty_moderate | -15 | creditor faction |
| collateral_seizure | -10 | creditor faction |
| raid_severe | -20 | creditor faction |
| labor_obligation | -5 | creditor faction |
| treaty_breach | -25 | creditor faction |
| forgiveness_rare | +5 | creditor faction |

## Rules
- Standing changes apply exactly once per default (keyed by `debtorId:consequenceId`)
- No double-application from template + consequence
- Standing range: -100 to +100
- Hostile threshold: ≤-50
- Allied threshold: ≥+50
