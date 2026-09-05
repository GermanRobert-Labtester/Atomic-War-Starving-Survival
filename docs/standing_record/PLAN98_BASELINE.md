# Plan 98: Standing Record Factions Expansion — Baseline Reconnaissance Report

## 1. Executive Summary

- **Catalog Authority:** `Assets/StreamingAssets/Data/standing_record_factions.json`
- **Initial Baseline Count:** 1 faction (`faction_the_overlay`, "The Overlay")
- **Expanded Target Count:** 8 distinct, non-overlapping, politically grounded factions
- **Host Session & Consumers:**
  - `StandingRecordHostSession.cs` (`AtomicWar.GodotApp.StandingRecordHostSession`)
  - `StandingRecordEngine.cs` (`Ashfall.Core.StandingRecordEngine`)
  - `FactionIconCatalog.cs` (`Ashfall.Core.UI.FactionIconCatalog`)
  - `ContentUtilizationScanner.cs` (catalog registered as `GAMEPLAY_CONSUMED`)
  - `LocationLayoutSystemTests.cs` (previously pinned single-item assertion)

---

## 2. Initial State Analysis

Prior to Plan 98 execution, `standing_record_factions.json` declared only one entity under the `"actions"` envelope:

```json
{
  "schema_version": 1,
  "actions": [
    {
      "id": "faction_the_overlay",
      "display_name": "The Overlay",
      "alignment": "conditional",
      "home_region": "all_regions",
      "is_active": true,
      "trust": 0,
      "wants": [
        "brass_fittings",
        "sr_stencil_pot",
        "lamp_oil"
      ],
      "offers": [
        "cadastral_keys",
        "travel_correction_on_named_sites"
      ],
      "signature_quote": "The Schedule named households. The Record names ground. Ground does not argue.",
      "access_rule": "Scrape three plates without writing a lived name or a Continuity number, and Overlay labour withdraws. They do not raid. Rooms go dark of juniors. Posts stay posts.",
      "badge_asset_id": ""
    }
  ]
}
```

This single-entry baseline created a major game-design deficit: The Standing Record expansion revolves around wasteland cartography, boundary disputes, cadastral plates, and jurisdictional enforcement, yet lacked institutional actors to enforce territory, contest land claims, or barter specialized recovery services.

---

## 3. Schema & Type Audit

Inspection of `Assets/Ashfall.Core/HoldfastFactionsCatalog.cs`, `CatalogIntegrityValidator.cs`, and `LocationLayoutSystemTests.cs` established the wire contract:

| Property | C# Type | Semantics & Verification Constraints |
|---|---|---|
| `id` | `string` | Unique identifier prefixed with `faction_the_`. Must resolve in `CatalogIntegrityValidator`. |
| `display_name` | `string` | Formatted title of the organization (e.g. "The Scale", "The Compact"). |
| `alignment` | `string` | Diplomatic orientation tag: `"conditional"`, `"neutral"`, `"peaceful"`, `"allied"`, or `"hostile"`. |
| `home_region` | `string` | Geographic anchor from `WASTELAND_REGION_ATLAS.md` (`"all_regions"`, `"industrial_belt"`, `"dead_suburbs"`, `"the_cut"`, `"deep_coast"`, `"ash_flats"`). |
| `is_active` | `bool` | Whether the faction actively participates in wasteland interaction upon campaign boot (`true`). |
| `trust` | `int` | Campaign starting trust score normalized between `-50` and `+50` (neutral default: `0`). |
| `wants` | `string[]` | Array of economic and material commodities sought in trade. Must avoid unintended `IdPrefixes` collisions. |
| `offers` | `string[]` | Array of institutional privileges, survival commodities, or logistical boons provided. |
| `signature_quote` | `string` | Single concise in-world statement defining institutional ideology and worldview. |
| `access_rule` | `string` | Explicit behavioral condition required to maintain trade and passage clearance. |
| `badge_asset_id` | `string` | Emblem token; empty string `""` legally defers to `FactionIconCatalog.Resolve(id)`. |

---

## 4. Preservation Directive

The original baseline entity `faction_the_overlay` was preserved byte-for-byte in position 0, maintaining zero deviation in naming, arrays, quote, access rule, or default values.
