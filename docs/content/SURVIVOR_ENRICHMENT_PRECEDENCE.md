# Survivor Enrichment Precedence & Overlay Merging Architecture

## 1. Architectural Philosophy
In ASHFALL, narrative enrichment annotates existing survivors rather than defining new runtime entities. Because enrichment content originates from multiple authored sources (baseline expansion, deep lore vignettes, and archetype specialist catalogs), a strict, deterministic precedence hierarchy is established.

---

## 2. Precedence Tiers

### Tier 1: Baseline Authority (`expansion_survivor_fields.json`)
- **Scope:** The core 72 survivor records.
- **Fields Governed:**
  - `phantom_background_id`
  - `pre_war_profession_id`
  - `belief_profile_id`
  - `personal_keepsake_item_id`
- **Rule:** Baseline authored fields represent primary canon for survivor personal histories. Once set by Tier 1, core background fields cannot be overwritten or degraded by secondary overlays.

### Tier 2: Deep Lore Canonical Additions (`deep_lore_survivor_fields.json`)
- **Scope:** The 4 deep lore survivors (`aris_thorne`, `maya_lin`, `victor_vance`, `elena_rostov`).
- **Fields Governed:**
  - All standard survivor fields (`phantom_background_id`, `pre_war_profession_id`, `belief_profile_id`, `personal_keepsake_item_id`)
  - `philosophical_stance`
- **Rule:** Deep lore records provide the canonical baseline for survivors not present in Tier 1. They undergo duplicate detection during loading; no survivor ID in Tier 2 conflicts with Tier 1.

### Tier 3: Archetype Specialist Overlay (`antigravity_survivor_fields.json`)
- **Scope:** 11 archetype specialists (`the_veteran`, `the_surgeon`, etc.).
- **Fields Governed:**
  - `philosophical_stance` (mapped from `stance`)
  - `manifesto_law_code`
- **Merge Rules:**
  - **Additive Only:** Specialist overlays supply supplemental philosophical stances and law codes.
  - **No Overwriting:** If a baseline survivor already has a core field (`belief_profile_id`, `phantom_background_id`, etc.), the specialist overlay *cannot* overwrite it.
  - **Fill Empty:** If a baseline survivor has an empty core field, a specialist overlay may fill it.

---

## 3. Deterministic Merge Logic (`ExpansionEnrichmentCatalog.cs`)

The merge logic is implemented in `ExpansionEnrichmentCatalog.MergeSurvivorFields`:

```csharp
public void MergeSurvivorFields(ExpansionSurvivorFields fields, bool isSpecialistOverlay = false)
{
    if (fields == null || string.IsNullOrEmpty(fields.survivor_id)) return;

    if (!_survivorFields.TryGetValue(fields.survivor_id, out var existing))
    {
        _survivorFields[fields.survivor_id] = fields;
        return;
    }

    // Core fields: preserved if specialist overlay, updated only if empty
    if (!string.IsNullOrEmpty(fields.phantom_background_id))
    {
        if (string.IsNullOrEmpty(existing.phantom_background_id) || !isSpecialistOverlay)
            existing.phantom_background_id = fields.phantom_background_id;
    }

    if (!string.IsNullOrEmpty(fields.pre_war_profession_id))
    {
        if (string.IsNullOrEmpty(existing.pre_war_profession_id) || !isSpecialistOverlay)
            existing.pre_war_profession_id = fields.pre_war_profession_id;
    }

    if (!string.IsNullOrEmpty(fields.belief_profile_id))
    {
        if (string.IsNullOrEmpty(existing.belief_profile_id) || !isSpecialistOverlay)
            existing.belief_profile_id = fields.belief_profile_id;
    }

    if (!string.IsNullOrEmpty(fields.personal_keepsake_item_id))
    {
        if (string.IsNullOrEmpty(existing.personal_keepsake_item_id) || !isSpecialistOverlay)
            existing.personal_keepsake_item_id = fields.personal_keepsake_item_id;
    }

    // Supplemental fields: always enriched
    if (!string.IsNullOrEmpty(fields.philosophical_stance))
        existing.philosophical_stance = fields.philosophical_stance;

    if (!string.IsNullOrEmpty(fields.manifesto_law_code))
        existing.manifesto_law_code = fields.manifesto_law_code;
}
```

---

## 4. Invariant Compliance
- **Determinism:** Merge order is fixed in `ExpansionEnrichmentCatalogLoader.LoadCatalog`:
  1. `expansion_survivor_fields.json` (Tier 1)
  2. `deep_lore_survivor_fields.json` (Tier 2)
  3. `antigravity_survivor_fields.json` (Tier 3)
- **Zero Allocations in Game Loop:** Merging occurs strictly at catalog initialization during bootstrap. All runtime access is $O(1)$ dictionary lookup.
