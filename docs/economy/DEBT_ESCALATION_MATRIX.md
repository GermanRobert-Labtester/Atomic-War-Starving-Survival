# Plan 40 — Escalation Matrix

## Escalation Graph
```
standing_loss_moderate ──→ bounty_moderate ──→ raid_severe
standing_loss_and_embargo ──→ bounty_moderate ──→ raid_severe
treaty_breach ──→ raid_severe
```

## Properties
- **Acyclic**: no consequence references itself
- **Bounded**: maximum chain depth is 3
- **One-shot**: each consequence fires once per contract (keyed by `debtorId:consequenceId`)
- **Deterministic**: escalation fires on the runtime's authoritative schedule

## Severity Ladder
1. **Mild**: standing_loss_mild (-5) — creditor notes the default
2. **Moderate**: standing_loss_moderate (-12) — word spreads
3. **Embargo**: embargo_trade (14d) — markets close
4. **Combined**: standing_loss_and_embargo (-10, 10d) — reputation + access
5. **Bounty**: bounty_moderate (-15) — collectors may visit
6. **Seizure**: collateral_seizure (-10) — asset forfeit
7. **Severe**: raid_severe (-20) — enforcement raid
8. **Treaty**: treaty_breach (-25) — diplomatic incident
9. **Labor**: labor_obligation (7d) — compulsory service
10. **Mercy**: forgiveness_rare (+5) — rare, contextual
