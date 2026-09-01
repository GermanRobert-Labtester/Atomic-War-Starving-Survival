# Plan 40 — Ledger Debt Schema Contract

## DebtTemplate (from ledger_debt_templates.json)

| Field | Type | Required | Description |
|---|---|---|---|
| id | string | yes | Stable template ID (prefix: `debt_`) |
| creditorId | string | yes | Faction ID of the creditor |
| principalItemId | string | yes | Canonical item ID for the principal |
| principalQuantity | int | yes | Number of items lent (must be > 0) |
| termDays | int | yes | Days until forfeit (must be > 0) |
| rate | float | yes | Flat interest rate (must be >= 0) |
| forfeitDescription | string | yes | Human-readable forfeit description |
| consequenceId | string | yes | Reference to consequence record |
| displayName | string | yes | Player-facing name |
| description | string | yes | Player-facing description |

## DebtConsequence (from ledger_debt_templates.json)

| Field | Type | Required | Description |
|---|---|---|---|
| id | string | yes | Stable consequence ID (prefix: `conseq_`) |
| trigger | string | yes | When this fires: `default`, `escalation` |
| effectType | string | yes | Runtime effect type |
| targetFactionId | string | no | Faction affected (empty = creditor) |
| standingDelta | int | no | Standing change (negative = penalty) |
| embargoScope | string | no | Embargo scope: `creditor_faction` |
| embargoDurationDays | int | no | Embargo duration in days |
| bountyLevel | string | no | Bounty severity: `low`, `moderate`, `severe` |
| collateralItemId | string | no | Item to seize |
| laborDays | int | no | Compulsory labor days |
| escalationId | string | no | Next consequence in escalation chain |
| displayName | string | yes | Player-facing name |
| description | string | yes | Player-facing description |

## Supported effectType Values
- `standing_loss` — reduces faction standing
- `embargo` — suspends trade with creditor faction
- `standing_loss_and_embargo` — both
- `bounty` — posts a bounty (handoff to raid system)
- `bounty_and_seizure` — bounty + collateral seizure
- `raid` — triggers enforcement raid
- `labor_obligation` — compulsory labor assignment
- `treaty_breach` — treaty violation
- `forgiveness` — rare debt clearance

## JSON Envelope
```json
{
  "schema_version": 1,
  "templates": [...],
  "consequences": [...]
}
```
