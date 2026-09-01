# Plan 17 — Canon Contradiction Audit

Identifies accidental contradictions, intentional unreliable narration, and provenance conflicts across ASHFALL's lore corpus.

## Audit Scope

- `world_history.json` — 79 entries
- `faction_lore.json` — 23 factions
- `environmental_atmosphere_expansion.json` — 152 texts
- `environmental_texts_expansion_05.json` — 36 texts
- `narrative/` subfolder — 273 files
- Dev lore documents — `docs/lore/00-06`, `IntelBible.md`

## Known Issues

### 1. Mixed ID Prefix Convention

**Issue:** `world_history.json` uses both `loc_*` and `location_*` prefixes for `discovery_location_id` values.

**Examples:**
- `loc_water_treatment_plant` (standard prefix)
- `location_ministry_of_truth_bunker` (non-standard prefix, appears 3 times)

**Impact:** Both resolve to valid locations, but the mixed convention violates snake_case ID standards and could cause confusion in validation tooling.

**Resolution:** Standardize to `loc_*` prefix. Add migration alias for `location_*` → `loc_*` if needed.

**Severity:** LOW — functional but inconsistent.

### 2. Orphan Item References

**Issue:** 11+ item IDs referenced by lore/gameplay files have no definition in `items.json`.

**Affected files:**
- `archive_inks.json` — `charcoal` (blocks 2 of 3 inks)
- `deep_lore_locations.json` — `book`
- `deep_lore_survivor_fields.json` — `blueprint_roll`, `radio_headset`, `service_pistol`, `surgical_mask`
- `final_wishes.json` — `scalpel`, `forceps`, `surgical_suture`, `dog_tags`, `concrete_rubble`

**Impact:** Gameplay systems cannot fully resolve. Archive ink system uncraftable. Final wishes uncompletable.

**Resolution:** Define missing items in `items.json` with appropriate categories and descriptions.

**Severity:** HIGH — blocks gameplay.

### 3. Environmental Text Location Keys

**Issue:** `environmental_atmosphere_expansion.json` uses free-text location names (e.g., `geothermal_plant_ruins`, `flooded_subway_depot`) rather than `loc_*` IDs.

**Impact:** Cannot validate location refs through `CatalogIntegrityValidator`. Texts are keyed to conceptual locations, not the location catalog.

**Resolution:** Document this as intentional (atmosphere texts are thematic, not location-specific). Add a mapping layer if runtime integration requires `loc_*` resolution.

**Severity:** LOW — by design, but needs documentation.

### 4. Epilogue Chronicle Placeholders

**Issue:** `epilogue_chronicle.json` has 5 slides with all `art_asset_id` values ending in `_placeholder`.

**Impact:** Epilogue system has structure but no real visual content.

**Resolution:** Out of scope for Plan 17. Document as known gap.

**Severity:** MEDIUM — visible to players if epilogue triggers.

## Potential Contradictions (Requires Investigation)

### Faction Timeline Consistency

**Check:** Do faction origin stories in `faction_lore.json` align with `world_history.json` dates?

**Example to verify:**
- If a faction claims to have been "founded after the Exchange" but `world_history.json` places them pre-Exchange, that's a contradiction.
- If two factions claim to have founded the same settlement, that's a contradiction (or intentional rivalry).

**Status:** NOT YET AUDITED — requires line-by-line comparison.

### Location State Consistency

**Check:** Do environmental texts describe locations in ways that contradict `LocationEvolutionSystem` states?

**Example:**
- If a text describes "pristine, untouched shelves" but the location is marked as `lootDepletionFactor: 0.0` (fully depleted), that's a contradiction.
- If a text describes "fresh boot prints" but the location is marked as `abandoned` with no recent visits, that's a contradiction.

**Resolution:** State-aware text selection (Task 17C) must consume authoritative location state, not override it.

**Status:** NOT YET AUDITED — requires runtime integration first.

### Character Lifespan Consistency

**Check:** Do documents reference characters who would be impossibly old or dead?

**Example:**
- If a document dated "Day 300" is authored by someone who `world_history.json` says died at Day 50, that's a contradiction.
- If a child's workbook references events from "20 years ago" but the child is 8 years old, that's a contradiction.

**Resolution:** Validate document dates against character birth/death dates in survivor data.

**Status:** NOT YET AUDITED — requires character date inventory.

### Technology Chronology

**Check:** Do documents reference technology that appears before its established invention date?

**Example:**
- If a pre-Exchange document references "post-Exchange salvage tech," that's a contradiction.
- If a document from a low-tech faction references advanced electronics they shouldn't have, that's a contradiction.

**Resolution:** Establish technology timeline in `world_history.json`. Validate document tech references against it.

**Status:** NOT YET AUDITED — requires technology timeline.

## Intentional Unreliable Narration

### Classified Contradictions

Some contradictions are intentional and should be preserved:

| Type | Description | Example | Handling |
|------|-------------|---------|----------|
| Biased testimony | Eyewitness accounts with limited perspective | Survivor A says "the bridge collapsed at dawn"; Survivor B says "it was dusk" | Preserve both; note disagreement |
| Propaganda | Faction claims that may be false | Faction circular claims "we have never lost a battle" | Mark as faction propaganda; do not present as objective truth |
| Incomplete information | Documents that don't know what the player knows | Pre-Exchange document references "temporary emergency measures" that became permanent | Preserve; shows historical perspective |
| Deliberate lie | Character is lying | NPC's diary says one thing; their actions show another | Preserve; part of character depth |
| Unresolved mystery | Intentional ambiguity | Two documents give conflicting accounts of what happened at Location X | Preserve; player must decide |

### Marking Intentional Contradictions

For each intentional contradiction, document:

1. **Which sources conflict** — document IDs, faction IDs, character IDs
2. **What the contradiction is** — specific conflicting claims
3. **Why it's intentional** — biased testimony, propaganda, mystery, etc.
4. **How to present it** — show both, mark as disputed, let player decide

**Status:** NOT YET DOCUMENTED — requires provenance pass (Task 17P).

## Automated Validation

### Current State

- `CatalogIntegrityValidator` checks ID refs, ranges, uniqueness
- `DataRuleComplianceTests` checks fictional-world compliance (no real countries/wars/people)
- No automated chronology validation
- No automated contradiction detection

### Required Additions (Task 17Y)

1. **Chronology validator** — check dates in `world_history.json` are ordered
2. **Faction timeline validator** — check faction origins align with history
3. **Document date validator** — check document dates are plausible
4. **Character lifespan validator** — check document authors are alive when they write
5. **Technology timeline validator** — check tech references match invention dates
6. **Contradiction allowlist** — mark intentional contradictions as valid

## Verification

| Check | Status |
|-------|--------|
| Mixed ID prefix resolved | ❌ NOT DONE |
| Orphan item refs fixed | ❌ NOT DONE (in progress) |
| Environmental text location keys documented | ❌ NOT DONE |
| Faction timeline consistency audited | ❌ NOT DONE |
| Location state consistency audited | ❌ NOT DONE |
| Character lifespan consistency audited | ❌ NOT DONE |
| Technology chronology audited | ❌ NOT DONE |
| Intentional contradictions documented | ❌ NOT DONE |
| Automated validators created | ❌ NOT DONE |

## Next Steps

1. **Fix orphan item refs** — highest priority (blocking gameplay)
2. **Standardize ID prefixes** — migrate `location_*` → `loc_*`
3. **Document environmental text location keys** — explain intentional design
4. **Audit faction timelines** — line-by-line comparison
5. **Audit character lifespans** — validate document dates
6. **Document intentional contradictions** — provenance pass
7. **Create automated validators** — Task 17Y
