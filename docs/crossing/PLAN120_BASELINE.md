# Plan 120 — Crossing Factions Baseline & Preflight Audit

## 1. Executive Summary

Prior to Plan 120, `crossing_factions.json` defined exactly 3 charter-settlement factions representing the core initial political triad of Nobody's Charter:
- The Scale (`faction_the_scale`): honest weights, measurement verification, Stallrow access.
- The Underwrite (`faction_the_underwrite`): debt underwriting, risk collateral, brutal credit forfeits.
- The Compact (`faction_the_compact`): charter drafting, assembly democracy, civil record keeping.

This baseline of 3 factions was insufficient for a settlement whose central design premise is multi-party arbitration over scarce resources, contested territory, debt collection, and survival infrastructure. Plan 120 expands this catalog from 3 to exactly 8 factions.

---

## 2. Baseline Faction Records (Frozen Verbatim)

```json
{
  "schema_version": 1,
  "actions": [
    {
      "id": "faction_the_scale",
      "display_name": "The Scale",
      "alignment": "conditional",
      "home_region": "region_crossing",
      "is_active": true,
      "trust": 0,
      "wants": [
        "trade_goods"
      ],
      "offers": [
        "stallrow_trade_access",
        "verification"
      ],
      "signature_quote": "The brass scales do not lie, and the numbers have no conscience. What people infer from their poverty is not my problem.",
      "access_rule": "An agonizingly precise weigh-in is the price of doing business at Stallrow. Contest a true reading without cause, and the market stays open—but your name goes on the slate.",
      "badge_asset_id": ""
    },
    {
      "id": "faction_the_underwrite",
      "display_name": "The Underwrite",
      "alignment": "conditional",
      "home_region": "region_crossing",
      "is_active": true,
      "trust": 0,
      "wants": [
        "pledged_goods"
      ],
      "offers": [
        "seed_stock",
        "covered_loss",
        "favour_bank"
      ],
      "signature_quote": "Read it twice, under the sodium glare. I'll say it twice. After the second time, there is only the ink, and the debt it binds you to.",
      "access_rule": "Their 'help' is genuine, offered against a plainly named, brutal forfeit—a child's labor, a pound of flesh, a year of servitude. Sign, negotiate, or walk away; no hidden clause survives a second reading.",
      "badge_asset_id": ""
    },
    {
      "id": "faction_the_compact",
      "display_name": "The Compact",
      "alignment": "peaceful",
      "home_region": "region_crossing",
      "is_active": true,
      "trust": 0,
      "wants": [
        "signatories"
      ],
      "offers": [
        "charter_draft",
        "ratification"
      ],
      "signature_quote": "There is a document now, stained with ash and thumbprints. Let them argue with the paper instead of each other's throats.",
      "access_rule": "Sign the blood-flecked draft and you are on the record. Perrin will not ratify a clause he knows will break a man—but he has not yet seen every way the words can be twisted.",
      "badge_asset_id": ""
    }
  ]
}
```

---

## 3. Preflight Findings & Constraints

1. **Root Element Name:** The array is named `"actions"`. This is preserved without renaming to maintain zero-breakage compatibility with `CrossingCatalogLoader.cs` which enumerates the root object to locate array properties.
2. **Data Authority:** `Assets/StreamingAssets/Data/crossing_factions.json` and its Linux build mirror at `builds/linux/Assets/StreamingAssets/Data/crossing_factions.json` must be updated synchronously.
3. **Core Hardcoded Check:** `CrossingHeadlessDemo.cs:35` checked `session.Catalog.Factions.Count == 3`. This content-capacity constraint was updated to `>= 8` with individual resolution checks for all eight factions.
4. **ID Prefix:** All faction IDs strictly use the canonical `faction_the_<name>` prefix.
5. **No Invented Runtimes:** No new faction standing engine, territory manager, or black-market subsystem was created. All state is presented and consumed through existing crossing APIs.
