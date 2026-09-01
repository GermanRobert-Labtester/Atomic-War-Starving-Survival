# Apiculture & Salt Product Matrices

This document specifies the expanded production yields, processing requirements, and consumer roles for Greenhouse Apiculture (`ApicultureSystem`) and Subterranean Salt Extraction (`SaltMineExtractionSystem`).

---

## 1. Apiculture Products (4 Product Roles)

| Product Item ID | Display Name | Hive Source | Output Rate | Processing Requirement | Consumer System | Survival / Economic Role |
|---|---|---|---|---|---|---|
| `item_honey_pot` | Raw Comb Honey (clay pot) | Healthy Hive (`queenVitality > 0.6`) | ~0.01 kg/pop/day (Max 5kg buffer) | Centrifugal extraction / straining | Kitchen / Canteen / Medical | Natural sweetener, morale boost (+4), calorie density, wound dressing |
| `item_beeswax_block` | Purified Beeswax Block | Hive wax combs (`waxBuffer`) | ~0.005 kg/pop/day (Max 2kg buffer) | Solar melting / cloth filtering | Workshop / Foundry / Storage | Waterproofing sealant, candle making, mold release agent, jar sealing |
| `item_raw_propolis` | Raw Propolis Resin | Hive frame scraping (`inspection`) | 0.2 kg per inspection cycle | Alcohol tincture or raw mastication | Medical Ward / Pharma Lab | Antiseptic salve, oral hygiene, throat irritation treatment |
| `item_mead_must_base` | Honey Must Fermentation Base | Strained comb washings + water | 1 batch per 2kg honey harvest | Crock fermentation vessel | Kitchen / Beverage Station | High-morale ration beverage (+8 morale), trade export good |

---

## 2. Salt Extraction Products (3 Product Roles)

| Product Item ID | Display Name | Extraction Source | Mine Processing | Output Yield | Consumer System | Survival / Economic Role |
|---|---|---|---|---|---|---|
| `item_preservation_salt` | Coarse Preservation Salt | Subterranean Halite Veins | Mechanical crushing & sieve grading | 0.60 kg / kg ore | Kitchen / Food Storage / Curing | Meat curing, vegetable brining, hide tanning, fish preservation |
| `item_trade_salt_sack` | Standard Trade Salt Sack (25kg) | Salt Mine Bulk Storage | Bagging & weigh-hut stamping | 1 sack / 25kg bulk salt | Regional Caravan / Market Trade | Universal inland currency, barter medium with agrarian settlements |
| `item_medical_saline_salt` | High-Purity Saline Salt | Recrystallized Brine Evaporator | Multi-stage wash & autoclave drying | 0.20 kg / kg brine | Medical Ward / Infirmary / Pharma | Sterile IV saline wash, burn irrigation, rehydration salts |
