# Trade Price Authority Map

**Document Version:** 1.0.0
**Domain:** ASHFALL Economy & Barter System

---

## 1. System Authority Breakdown

| Fact / Variable | Authoritative Source | Runtime Owner | Plan 61 Role |
|---|---|---|---|
| **Base Item Value** | `economy_goods.json` | `GoodsCatalog` | Unmodified base value |
| **Scarcity & Shocks** | `hardcore_economy_tuning.json` / scenario | `HardcoreEconomyTuning` / `TradeScreenScenario` | Scenario context shocks/multipliers |
| **Biological Value** | `TradePricing.BioUnitValue` in Core | `TradePricing` (frozen) | Authoritative bio pricing formula |
| **Faction Stance & Trust** | `factions.json` / `faction_radio_corpus.json` | `FactionStanceEngine` | Contextual stance & trust parameters |
| **Debt State & Limits** | `ledger_debt_templates.json` | `TradeCreditCoordinator` | Credit eligibility and repayment boundary |
| **Settlement Default Trade** | `settlements.json` | `SettlementCatalog` | Default trade specialty & scenario mapping |
| **Transaction Valuation** | `TradeScreenViewModel.SetTable` | `TradeScreenViewModel` | Evaluates player offers vs faction asks |
| **Qualitative Labels** | `TradeWorthLabels.Format` | `TradeWorthLabels` | Formats monetary ranges (ECON-002: no raw digits) |

---

## 2. Price Composition & Arbitrage Protection

### 2.1 Pricing Equation
For any line item on the table:
$$\text{TotalValue} = \text{Quantity} \times \text{UnitPrice}$$

Where $\text{UnitPrice}$ is defined either directly in the scenario line or derived from:
$$\text{UnitPrice} = \text{BasePrice} \times \text{ScarcityMultiplier} \times \text{ShockMultiplier}$$

### 2.2 Table Evaluation Rule
$$\text{PlayerOfferValue} = \sum_{\text{items}} (\text{Quantity} \times \text{UnitPrice}) + \sum_{\text{bio}} (\text{Count} \times \text{BioUnitValue})$$
$$\text{FactionAskValue} = \sum_{\text{demands}} (\text{Quantity} \times \text{UnitPrice})$$

- If both sides are empty:
  $$\text{Fairness} = \text{EmptyTable}, \quad \text{CanConfirm} = \text{false}$$
- If $\text{PlayerOfferValue} \ge \text{FactionAskValue}$:
  $$\text{Fairness} = \text{Fair}, \quad \text{CanConfirm} = \text{willTrade}$$
- If $\text{PlayerOfferValue} < \text{FactionAskValue}$:
  $$\text{Fairness} = \text{Short}, \quad \text{CanConfirm} = \text{false}$$

Where $\text{willTrade} = (\text{Stance} == \text{Trade} \lor \text{Stance} == \text{ShareIntel})$.

### 2.3 Qualitative Worth Bands (ECON-002)
- $\le 0.0$: `"None"`
- $< 20.0$: `"Sparse"`
- $< 60.0$: `"Modest"`
- $< 150.0$: `"Substantial"`
- $\ge 150.0$: `"Generous"`

### 2.4 Biological Valuation Schedule
- `PintOfBlood`: $1 \times 25 = 25$
- `BoneMarrow`: $2 \times 25 = 50$
- `Plasma`: $3 \times 25 = 75$
- `Organ`: $4 \times 25 = 100$

### 2.5 Arbitrage Boundary Constraints
- Scenarios do not provide symmetric buy/sell prices on the same session.
- Fixed scenario inventories do not permit repeatable purchase and immediate resale within the same merchant.
- Scenario multipliers never reduce base prices below sustainable floors (minimum $1$ currency unit).
- Volume discounts in bulk dealer scenarios are counterbalanced by item availability caps and minimum demand requirements.
