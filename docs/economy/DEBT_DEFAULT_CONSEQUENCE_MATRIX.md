# Plan 40 — Default Consequence Matrix

## 10 Consequence Records

| # | ID | Effect | Standing | Embargo | Bounty | Escalation |
|---|---|---|---|---|---|---|
| 1 | conseq_standing_loss_mild | standing_loss | -5 | — | — | — |
| 2 | conseq_standing_loss_moderate | standing_loss | -12 | — | — | → bounty_moderate |
| 3 | conseq_embargo_trade | embargo | -8 | 14d | — | — |
| 4 | conseq_standing_loss_and_embargo | standing+embargo | -10 | 10d | — | → bounty_moderate |
| 5 | conseq_bounty_moderate | bounty | -15 | — | moderate | → raid_severe |
| 6 | conseq_collateral_seizure | bounty+seizure | -10 | — | low | — |
| 7 | conseq_raid_severe | raid | -20 | — | severe | — |
| 8 | conseq_labor_obligation | labor | -5 | — | — | — |
| 9 | conseq_treaty_breach | treaty | -25 | — | — | → raid_severe |
| 10 | conseq_forgiveness_rare | forgiveness | +5 | — | — | — |

## Escalation Chains
- standing_loss_moderate → bounty_moderate → raid_severe
- standing_loss_and_embargo → bounty_moderate → raid_severe
- treaty_breach → raid_severe
- All chains are acyclic and bounded (max depth: 3)
