# Plan 40 — Baseline Reconnaissance

## Runtime: LedgerDebtSystem.cs (301 lines)

### DebtContract Fields
| Field | Type | Purpose |
|---|---|---|
| debtorId | string | Debtor identifier |
| creditorId | string | Creditor faction (Plan 40 addition) |
| templateId | string | Template reference (Plan 40 addition) |
| principal | float | Base amount owed |
| termDays | int | Debt term length |
| rate | float | Flat interest rate |
| forfeit | string | Named good or service-days |
| readCount | int | Readings before signing |
| signed | bool | Whether ink has been applied |
| signedDay | int | Day of signing |
| daysRemaining | int | Countdown timer |
| paid | bool | Terminal: paid in full |
| forfeited | bool | Terminal: forfeit due |

### Interest Model
- **Formula**: `total = principal × (1 + rate)` — flat, never compounded
- **No partial payments**: binary paid/unpaid
- **No late payment**: goes straight to `forfeited` when `daysRemaining <= 0`

### Due-Time Semantics
- `termDays` is a duration (days after signing)
- `signedDay` records the campaign day of signing
- `daysRemaining` decrements daily via `TickDaily(day)`
- Default occurs when `daysRemaining <= 0`

### Save Integration
- `LedgerDebtSystemState` with `contracts` + `closedContracts` + `ledgerTampered`
- Present in `ExpansionHubSave` since v1
- Frozen-shape migration pattern (v1→v2→v3→v4)

### Catalog Status
- **Loader**: MISSING → created as `DebtTemplateCatalogLoader`
- **JSON**: MISSING → created as `ledger_debt_templates.json`
- **Consequence dispatch**: MISSING → created as `DebtConsequenceDispatcher`

### Existing Debt Infrastructure
- UI panel: `subterranean_debt_ledger` registered
- Questline: `quest_garrison_blood_debt` (7 stages)
- Endgame: `debtLedgersBurned` flag, `IndenturedDebtState` moral standing
- Art: 5 debt assets (evt_debt_collector, faction_dig_out_debt, item_debt_contract_copy, pay_debt, refuse_debt)
- Selftest: `LedgerDebtHeadlessDemo` (22 checks)

### Factions (20 total)
iron_garrison, ash_militia, cult_of_ash_sign, warlords_sector_4, faction_rebuilders, faction_black_ops, faction_central_garrison, faction_ash_sign, faction_scavengers, faction_hydro_barons, faction_unaligned, faction_salt_freeholders, faction_railway_guild, faction_ordnance_foundry, faction_penal_battalion, faction_ash_militia, faction_supply_corps, raiders, faction_forward_roster, faction_doctrine_archetype_*

### Items (230+ total, 22 types)
Key categories: Food (22), Medical (28), Fuel (4), Device (22), Protective (6), Material (42), Tool (16), Weapon (10), Ammo (14), Equipment (7)
