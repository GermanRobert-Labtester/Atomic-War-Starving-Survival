# Foundry Treaty Consequence Schema Contract

**Target File:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`
**C# Models:** `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`
**Contract Version:** `schema_version = 1`

---

## 1. Root Structure (`FoundryTreatyConsequenceFile`)

| Field | Type | Required | Description |
|---|---|---|---|
| `schema_version` | integer | Yes | Catalog schema version (pinned to `1`). Required by `CatalogIntegrityValidator`. |
| `collection_id` | string | Yes | Unique identifier for catalog collection (`"foundry_treaty_consequence_policy"`). |
| `policies` | array | Yes | List of `FoundryTreatyConsequencePolicy` entries. |

---

## 2. Policy Entry Structure (`FoundryTreatyConsequencePolicy`)

| Field | Type | Required | Range / Enum | Description |
|---|---|---|---|---|
| `treaty_id` | string | Yes | Snake_case string | Treaty ID resolving against `foundry_accords.json`. |
| `faction_id` | string | Yes | Snake_case string | Signatory faction ID resolving against `foundry_accords.json` and canonical faction records. |
| `outcome` | string | Yes | `"met" \| "missed" \| "violated"` | Outcome classification triggering this consequence. |
| `standing_delta` | float | Yes | `[-20.0, +10.0]` | Faction trust adjustment applied to the signatory faction. |
| `reason` | string | Yes | Non-empty string | Institutional description of what physically and diplomatically changed. |
| `market_modifiers` | array | Yes | List of modifiers | Structured list of economic/market demand impacts. |

---

## 3. Good Modifier Structure (`FoundryGoodModifier`)

| Field | Type | Required | Range | Description |
|---|---|---|---|---|
| `good_id` | string | Yes | Valid good ID | Must resolve in `economy_goods.json` (e.g. `coal`, `fuel`, `scrap_metal`, `clean_water`, `canned_food`, `water_filter`, `item_foundry_brine_pipe`, `item_foundry_ice_anchor`). |
| `demand_delta` | float | Yes | `[-1.0, +1.0]` | Market demand multiplier shift. Negative = increased availability/lower price; Positive = scarcity/price spike. |
| `reason` | string | Yes | Non-empty string | Economic rationale for the price/demand movement. |

---

## 4. Key and Lookup Semantics

- **Primary Lookup Key:** `treaty_id + "|" + outcome`
- **Lookup Method:** `SilentFoundryConsequencePolicyCatalog.Find(string treatyId, FoundryTreatyOutcome outcome)`
- **Uniqueness Constraint:** Exactly one policy may exist for any `(treaty_id, outcome)` pair. Duplicate keys cause load errors.
- **Missing Policy Rule:** If a treaty resolves an outcome without an authored policy, `Find` returns `null` and the outcome is recorded without error (neutral fallback).
