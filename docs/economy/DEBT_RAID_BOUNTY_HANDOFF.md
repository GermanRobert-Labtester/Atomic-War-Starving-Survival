# Plan 40 — Raid/Bounty Handoff

## Enforcement Chain
```
default → consequence → bounty → raid
```

## Bounty Integration
- `conseq_bounty_moderate` fires `OnBountyRequested` event
- Host layer receives faction ID + contract
- Existing `IronRaidersSystem.EvaluateRaidChance()` determines raid timing
- Debt is provenance, not raid state owner

## Raid Integration
- `conseq_raid_severe` fires `OnBountyRequested` with `bountyLevel=severe`
- Existing raid system owns actual raid execution
- No duplicate raid scheduling (one-shot per contract)
- Raid outcome does not automatically erase debt

## Cooldown
- Existing raid cooldown/budget applies
- Multiple defaulted debts can stack politically without encounter spam
