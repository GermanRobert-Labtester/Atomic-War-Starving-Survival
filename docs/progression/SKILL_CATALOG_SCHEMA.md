# Plan 33 — Skill Catalog JSON Schema Specification

## 1. Catalog Container Specification
- **Path:** `Assets/StreamingAssets/Data/skills.json`
- **Format:** Snake_case JSON with top-level `schema_version` and collection identifier.

```json
{
  "schema_version": 1,
  "collection_id": "skills",
  "skills": [
    {
      "id": "skill_field_dressing",
      "display_name": "Field Dressing",
      "description": "Bandaging, splinting, and initial trauma stabilization.",
      "discipline_id": "medical",
      "xp_threshold": 50.0,
      "skill_bonus": 0.10,
      "is_expert_skill": false
    }
  ]
}
```

---

## 2. Field Definitions

| Property | Type | Mandatory | Description |
|---|---|---|---|
| `id` | string | Yes | Unique snake_case identifier starting with prefix `skill_`. |
| `display_name` | string | Yes | User-facing localized title. |
| `description` | string | Yes | Descriptive text explaining practical effect and fiction. |
| `discipline_id` | string | Yes | One of `medical`, `crafting`, `science`, `combat`, `scavenging`, `survival`, or empty string (for latent skills with narrative disciplines). |
| `xp_threshold` | float | Yes | XP threshold required to auto-unlock from action practice. Milestone/narrative skills use `999999.0`. |
| `skill_bonus` | float | Yes | Additive bonus percentage applied to discipline efficiency (0.00 to 0.30). |
| `is_expert_skill` | boolean | Yes | `true` if restricted to survivors whose latent expert discipline matches `discipline_id`. |

---

## 3. Validation Rules Enforced by CatalogIntegrityValidator
1. **Tier 1 Verification:** Every string ID with prefix `skill_` must resolve against the loaded skill definitions.
2. **Uniqueness:** No duplicate `id` values allowed in `skills.json`.
3. **Threshold Bounds:** `xp_threshold` must be non-negative.
4. **Bonus Range:** `skill_bonus` must be between `0.0` and `1.0`.
