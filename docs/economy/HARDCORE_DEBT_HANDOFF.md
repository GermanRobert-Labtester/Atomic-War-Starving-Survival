# Hardcore Debt Handoff

## 1. Subterranean Debt Ledger Coupling

The Subterranean Debt Ledger (`SubterraneanDebtLedgerSystem`) interacts directly with `HardcoreEconomyTuning`:

### 1.1 Collateral Valuation
- Debts denominated in material collateral (e.g. water barrels, fuel drums, or ammo crates) are appraised using `GetScarcityMultiplier`.
- If a debtor defaults during a high-scarcity tier (`Critical` or `DeepWinter`), the required volume of physical goods to satisfy the claim decreases proportionately to the inflated market price.

### 1.2 Creditor Faction Preferences
- Creditors prioritize collecting collateral that aligns with their faction profile (`BuysAtPremium`).
- The Underwrite (`faction_the_underwrite`) refuses debt settlements paid in sludge or toxic tailings (`Refuses`).
