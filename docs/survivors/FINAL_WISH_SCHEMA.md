# Final Wish Catalog Schema Contract

**Document:** `docs/survivors/FINAL_WISH_SCHEMA.md`
**Target File:** `Assets/StreamingAssets/Data/final_wishes.json`

---

## 1. Top-Level Structure

```json
{
  "schema_version": 1,
  "items": [
    ...
  ]
}
```

- `schema_version` (integer, required): Pinned to `1`.
- `items` (array of objects, required): Array of wish definition records. Exactly 30 records.

---

## 2. Wish Item Definition Fields

| Field | Type | Required | Description | Example |
|---|---|---|---|---|
| `archetype_id` | string | Yes | Unique survivor archetype ID matching `survivors.json` | `"the_hunter"` |
| `wish_type` | string | Yes | Snake_case wish category from the 10 supported types | `"teach_lesson"` |
| `wish_title` | string | Yes | Evocative, concrete 2–5 word title | `"Reading the Runs"` |
| `wish_description` | string | Yes | 1–2 sentence narrative description in survivor's voice with `{name}` placeholder | `"{name} pulls a loop..."` |
| `steps` | array | Yes | Ordered array of 2–4 executable step objects | `[ ... ]` |
| `completion_text` | string | Yes | Grounded narrative conclusion upon final step completion | `"{name} nods once..."` |
| `morale_bonus` | integer | Yes | Fixed permanent shelter morale gain | `15` |
| `buff_id` | string | Yes | Permanent buff identifier | `"their_memory_lives_on"` |

---

## 3. Step Object Schema

Each element of `steps` contains:

| Field | Type | Required | Description | Example |
|---|---|---|---|---|
| `step_id` | string | Yes | Unique snake_case identifier for this step within the wish | `"rig_hair_trigger"` |
| `description` | string | Yes | Objective description describing what must be done | `"Form and notch..."` |
| `required_items` | string[] | No | Canonical item IDs that must be gathered / possessed | `["trap_improvised_wire"]` |
| `requires_location` | string | No | Canonical location ID where expedition / action must occur | `"loc_settlement_cape_beacon"` |
| `requires_npc` | string | No | Canonical NPC ID from `npc_arcs.json` | `"npc_mara_veln"` |
| `skill_transfer` | string | No | Canonical skill ID transferred upon lesson completion | `"skill_field_dressing"` |
| `requires_patient` | boolean | No | Legacy flag indicating patient required | `true` |
