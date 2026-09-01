# Plan 40 — Ledger Debt Templates (system exists, no data)

## Goal (2 lines)
Create `ledger_debt_templates.json` for `LedgerDebtSystem` — the system is fully implemented
and save-supported but has **no debt templates** (verified: file missing). Add 15 debt
templates and 10 default-consequence records that make shelter debt a meaningful economic
pressure with faction and reputation consequences.

## Why (P2)
- Verified: `LedgerDebtSystem.cs` exists in Core; no template catalog exists.
- Debt is the economic pressure layer: without templates, the ledger has no structured
  obligations — trade debt is ad hoc and lacks consequence chains (feeds W18 in roadmap 31).
- Creates a moral/economic loop: borrow → owe → default or repay → faction reputation shifts.

## Files to touch
- `Assets/StreamingAssets/Data/ledger_debt_templates.json` (CREATE — 15 templates + 10 consequences)
- Read-only: `Assets/Ashfall.Core/LedgerDebtSystem.cs` (confirm template schema: debt id,
  creditor faction, principal, interest rate, due day, collateral, default consequence id),
  `Assets/StreamingAssets/Data/factions.json` (creditor faction ids must resolve)
- Check loader: `grep -rn "ledger_debt\|LedgerDebt\|debt_template" Assets/Ashfall.Core/`

## Content grammar (per debt template)
- snake_case `id` with prefix `debt_` or `ledger_` (confirm accepted prefix — do not invent).
- creditor_faction: `faction_*` id from `factions.json` (TIER-2 validation).
- principal: item id + quantity owed (e.g. `item_fuel` x 20, `item_clean_water` x 50).
- interest_rate: percentage per day (compounding — confirm the system's compounding model).
- due_day: deadline tick; default triggers the consequence.
- collateral: optional item id held against the debt (seized on default).
- reputation_impact: reputation delta with the creditor faction on default.

## Content grammar (per default consequence)
- snake_case `id` with prefix `consequence_` or `event_` (confirm accepted prefix).
- trigger: default / late_payment / partial_payment.
- effects: faction reputation loss, trade embargo, bounty issued (feeds existing 14 raids),
  collateral seizure, shelter raid, debt slavery (survivor reassignment).
- escalation: some consequences escalate over time (bounty → raid → faction war).

## Steps
1. Read `LedgerDebtSystem.cs` end-to-end: confirm the debt schema, the interest-compounding
   model, the due-day logic, the default trigger, and the save DTO shape.
2. Confirm loader status; if missing, add a mechanical loader.
3. Author 15 debt templates across 5 creditor factions (3 per faction): short-term food debt,
   fuel debt, medicine debt, weapon debt, labor debt. Each with distinct principal,
   interest rate, due day, and collateral.
4. Author 10 default consequences: reputation_loss, trade_embargo, bounty_issued (feeds
   existing 14 raids), collateral_seizure, shelter_raid, debt_slavery, escalation_bounty,
   escalation_raid, escalation_faction_war, forgiveness (rare — faction absorbs the loss).
5. Cross-reference: every `faction_*` id resolves; every `item_*` id exists; every
   `consequence_*` id resolves (TIER-1/TIER-2).
6. Wire 3 debt templates into Plan 13 trade encounters — traders offer credit
   when the player can't pay upfront.
7. Validate: `--data-integrity-selftest`; confirm a borrow → accrue interest → due → default
   or repay → consequence loop works in a headless boot; save round-trip for active debts.
8. xUnit: interest compounds correctly (deterministic), due-day triggers default, reputation
   delta applies, escalation fires on schedule, save round-trip preserves debt state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — data-only if loader exists. The compounding-model question (step 1) is the one trap:
do not guess the interest formula; read it from the system first.

## Definition of Done
- `ledger_debt_templates.json` exists with 15 templates + 10 consequences, all ids resolving,
  debt loop works end-to-end, interest compounding deterministic, escalation fires on
  schedule, save round-trip green, integrity + tests green.

## Follow-on
- W18 in roadmap 31 (debt default → bounty → reputation → territory chain).
- Existing 14 (raids) — bounties generate raid encounters.
- Plan 44 (faction territory) — debt default shifts faction control.
- Existing 16C (treaties) — debt disputes escalate into treaty violations.
