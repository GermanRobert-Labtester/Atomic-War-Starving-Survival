# Confession Secret Catalog Schema Specification

> **File:** `Assets/StreamingAssets/Data/confession_secrets.json`
> **Schema Version:** `1`
> **Runtime Deserializer:** `Assets/Ashfall.Core/Phantoms/ConfessionSecretCatalog.cs` (`ConfessionSecretCatalogJson`)

---

## 1. File Structure

```json
{
  "schema_version": 1,
  "items": [
    { ... }
  ]
}
```

---

## 2. Field Definitions

| Field Name | Type | Required | Description / Constraints | Example |
|---|---|---|---|---|
| `secret_id` | `string` | Yes | Unique snake_case identifier with `secret_` prefix. Registered as a DefinitionKey. | `"secret_nurse_missed_medication"` |
| `archetype_id` | `string` | Yes | Survivor archetype identifier (e.g. `the_nurse`, `the_cook`) or faction key for institutional secrets. | `"the_nurse"` |
| `category` | `string` | Yes | Classification vocabulary: `"npc_personal"`, `"faction_institutional"`, or `"bunker_internal"`. | `"npc_personal"` |
| `subject_id` | `string` | Yes | Target entity identifier (matches archetype, faction, or shelter structure). | `"the_nurse"` |
| `secret_title` | `string` | Yes | Brief human-readable title describing the moral failure or revelation. | `"The Substituted Ampoules"` |
| `secret_text` | `string` | Yes | Authoritative confession prose. For personal secrets, uses `{name}` placeholder. | `"{name} looks at their trembling hands..."` |
| `discovery_path` | `string` | Yes | Discovery mechanism: `"direct_confession"`, `"document"`, or `"shelter_search"`. | `"direct_confession"` |
| `discovery_source_id` | `string` | Yes | Item ID that unlocked or triggered discovery. Must resolve in `items.json`. | `"nurse_fob_watch"` |
| `gating_flag` | `string` | Yes | World flag set upon discovery/resolution. Snake_case, prefixed with `flag_`. | `"flag_secret_nurse_confessed"` |
| `forgiveness_outcome` | `string` | Yes | Narrative dialogue/prose describing listener's compassionate response. | `"'You were working forty-eight hours...'"` |
| `forgiveness_affinity` | `float` | Yes | Relationship affinity delta granted on forgiveness. Legal range: +5 to +30. | `15` |
| `forgiveness_morale` | `float` | Yes | Morale delta applied to confessor and listener. Legal range: +5 to +20. | `10` |
| `grudge_outcome` | `string` | Yes | Narrative dialogue/prose describing listener's hostile or condemning response. | `"'You killed that boy with your own hands...'"` |
| `grudge_affinity` | `float` | Yes | Relationship affinity penalty applied on grudge. Legal range: -5 to -50. | `-35` |
| `grudge_morale` | `float` | Yes | Morale penalty applied to confessor. Legal range: -5 to -30. | `-18` |
| `expose_outcome` | `string` | Yes | Resolution prose when public revelation is chosen. | `"You bring the falsified triage log..."` |
| `expose_standing_faction` | `string` | Yes | Faction ID whose standing shifts on expose. Must resolve in `factions.json`. | `"faction_independent"` |
| `expose_standing_delta` | `float` | Yes | Faction standing modification (+/-). | `-10` |
| `expose_guilt_delta` | `float` | Yes | Guilt points added to confessor's `GuiltInsomniaSystem`. | `18` |
| `blackmail_outcome` | `string` | Yes | Resolution prose when extortion is chosen. | `"You keep a copy of the altered chart..."` |
| `blackmail_resource_gain` | `string` | Yes | Description or resource token gained from extortion. | `"medical_supplies"` |
| `blackmail_hardening_delta` | `float` | Yes | Moral hardening delta applied to player/actor via `MoralBranchingSystem`. | `0.15` |
| `keep_outcome` | `string` | Yes | Resolution prose when secret is kept to build deep mutual trust. | `"You hand the altered chart back..."` |
| `keep_trust_delta` | `float` | Yes | Trust points added to confidant relationship via `SurvivorRelationsSystem`. | `25` |

---

## 3. Placeholder Semantics

- `{name}`: Formatted by string replacement or regex with the survivor's localized `displayName` at runtime.
- Case-sensitive, exactly matching `{name}` with standard curly braces.
- No other unescaped braces allowed in prose strings.

---

## 4. Integrity Constraints

1. **Prefix Enforcement:** All `secret_id` tokens must start with `secret_`.
2. **Item Resolution:** All `discovery_source_id` tokens must match a valid entry in `Assets/StreamingAssets/Data/items.json`.
3. **No Duplicate IDs:** Every `secret_id` must be globally unique across all entries.
4. **Range Enforcement:**
   - `forgiveness_affinity`: $[+5, +30]$
   - `forgiveness_morale`: $[+5, +20]$
   - `grudge_affinity`: $[-50, -5]$
   - `grudge_morale`: $[-30, -5]$
