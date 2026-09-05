# Foundry Accord Schema Contract

**Authority File:** `Assets/StreamingAssets/Data/foundry_accords.json`
**C# Models:** `Assets/Ashfall.Core/Narrative/RegionalTreatyCatalog.cs`

---

## 1. Root Structure (`RegionalTreatiesFile`)

| Field | Type | Required | Description |
|---|---|---|---|
| `schema_version` | integer | Yes | File schema version (`1`). Enforced by `CatalogIntegrityValidator`. |
| `collection_id` | string | Yes | Unique identifier for catalog collection (`"foundry_district8_accords"`). |
| `treaties` | array | Yes | Array of `RegionalTreatyEntry` objects. |

---

## 2. Treaty Entry Structure (`RegionalTreatyEntry`)

| Field | Type | Required | Range / Enum | Description |
|---|---|---|---|---|
| `treaty_id` | string | Yes | Snake_case (`treaty_*`) | Unique, stable treaty identifier across campaigns and saves. |
| `ratified_day` | integer | Yes | `[1, 365]` | In-campaign day when the treaty went into effect. |
| `treaty_title` | string | Yes | Non-empty string | Formal legal title of the accord. |
| `signatory_factions` | array of strings | Yes | Count $\ge 2$ | Array of valid faction IDs that signed the accord. |
| `demarcated_territory` | string | Yes | Non-empty string | Geographical boundary, road right-of-way, or facility envelope governed by the accord. |
| `water_allocation_lpm` | float | Yes | $\ge 0.0$ | Water allocation in liters per minute (0.0 if not applicable). |
| `power_quota_kw` | float | Yes | $\ge 0.0$ | Power quota in kilowatts (0.0 if not applicable). |
| `tariff_schedule` | string | Yes | Non-empty string | Commercial duties, commodity exchange rates, or in-kind labor terms. |
| `treaty_articles` | string | Yes | Formatted string | Structured legal articles (`ARTICLE 1: ... ARTICLE 2: ... ARTICLE 3: ...`). |
| `penalties` | string | Yes | Non-empty string | Enforceable legal and diplomatic consequences upon default or breach. |
| `tags` | array of strings | Yes | Non-empty array | Normalized thematic classification tags. |

---

## 3. Invariants & Rules

1. **Zero vs Nonzero Utility Allocations:**
   - Where water or power is central to the accord (e.g. `brine_pipe` 40 lpm / 12 kW; `deep_coast_aquifer` 60 lpm / 14 kW; `garrison_grain_tithe` 50 lpm / 15 kW), realistic numbers are specified.
   - Where utilities are not part of the agreement (e.g. `the_cluster_charter` 0 lpm / 0 kW; `border_demilitarization` 0 lpm / 0 kW), values are strictly `0.0`. Fabricating utility transfers for symmetry is prohibited.

2. **Signatory Array Requirement:**
   - The array must list concrete, resolved faction IDs.
   - The wildcard `"all_factions"` is prohibited; all participants are explicitly enumerated.
