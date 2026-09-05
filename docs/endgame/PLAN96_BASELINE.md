# Plan 96 — Epilogue Chronicle Expansion: Baseline State & Reconnaissance

**Document ID:** `docs/endgame/PLAN96_BASELINE.md`
**Author:** AI Pair Programmer / Antigravity
**Target:** `epilogue_chronicle.json` (5 → 20 slides)
**Status:** Ground Truth Frozen

---

## 1. Verified Starting State

Prior to Plan 96, `Assets/StreamingAssets/Data/epilogue_chronicle.json` was an early scaffolding placeholder containing exactly 5 slide definitions under the `default_slides` key:

```json
{
  "schema_version": 1,
  "default_slides": [
    { "order": 0, "title": "Opening", "art_asset_id": "epilogue_opening_placeholder" },
    { "order": 1, "title": "The Bunker", "art_asset_id": "epilogue_bunker_placeholder" },
    { "order": 2, "title": "What Remains", "art_asset_id": "epilogue_remains_placeholder" },
    { "order": 3, "title": "Survivors", "art_asset_id": "epilogue_survivors_placeholder" },
    { "order": 4, "title": "Final Word", "art_asset_id": "epilogue_final_placeholder" }
  ]
}
```

### Baseline Properties
- **Catalog File:** `Assets/StreamingAssets/Data/epilogue_chronicle.json`
- **Schema Version:** `1`
- **Total Slide Records:** 5
- **Array Wrapper:** `"default_slides"`
- **Slide Schema:** `order` (int), `title` (string), `art_asset_id` (string)
- **Art Asset Registry Status:** 100% placeholder IDs (`epilogue_*_placeholder`). No textures or sprites currently bound.

---

## 2. Structural Deficiencies of Baseline

1. **Inadequate Outcome Granularity:** Plan 89 expanded the narrative epilogue matrix to 25 outcomes across Muster, Verdict, Factions, Resources, and Morals. Five generic slides collapsed all campaigns into identical presentation cards.
2. **Missing Campaign Themes:** No visual representation existed for the nuclear exchange, first winter survival, resource grids, casualties, radio listening networks, Verdict investigations, witness testimony, recovered relics, or future reconstruction.
3. **Misplaced Semantic Ordering:** In the 5-card baseline, "What Remains" preceded "Survivors", and "Final Word" closed immediately at order 4 without presenting the world's forward trajectory.

---

## 3. Preservation Commitment

Under Invariants 1, 5, and 6, the 5 original slide titles and their placeholder art IDs are strictly retained in the 20-slide catalog:
- `Opening` (`epilogue_opening_placeholder`)
- `The Bunker` (`epilogue_bunker_placeholder`)
- `Survivors` (`epilogue_survivors_placeholder`)
- `What Remains` (`epilogue_remains_placeholder`)
- `Final Word` (`epilogue_final_placeholder`)
