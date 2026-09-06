# ASHFALL — Micro-Location Export Parity Specification (Task F23)

## 1. Overview & Export Strategy

ASHFALL utilizes a unified Godot export pipeline across target desktop platforms (Linux/X11, Windows Desktop). Micro-locations are declared in the canonical data catalog `Assets/StreamingAssets/Data/micro_locations.json` and complemented by the localization table `assets/l10n/strings.csv`.

In Godot exported builds (`.pck` / executable archive), non-resource files such as JSON and CSV must be explicitly included in `export_presets.cfg` via the `include_filter` directive.

### Packaging Configuration (`export_presets.cfg`)

Both Linux and Windows export presets include the following filter:
```ini
include_filter="*.json, *.csv"
```
This ensures:
1. `micro_locations.json` is embedded into the exported package alongside standard catalogs (`items.json`, `locations.json`, `encounters.json`, etc.).
2. `assets/l10n/strings.csv` is embedded for runtime UI string resolution and multi-locale fallback.

---

## 2. Catalog Format & Schema Contract

`micro_locations.json` follows the canonical collection format:
```json
{
  "schema_version": 1,
  "collection_id": "micro_locations_catalog",
  "encounters": [
    {
      "id": "micro_crashed_truck",
      "title": "Crashed Supply Truck",
      "description": "A military logistics rig lies crushed in the frozen ditch...",
      "category": "Discovery",
      "baseWeight": 0.6,
      "stealthWeightMultiplier": 1.0,
      "speedWeightMultiplier": 0.8,
      "minDangerLevel": 1,
      "requiredLocationId": "",
      "choices": [
        {
          "choiceId": "search_truck_cargo",
          "text": "Search the split crate and cab for salvage.",
          "moraleDelta": 1,
          "guiltDelta": 0,
          "grantItemId": "canned_food",
          "grantItemQuantity": 2,
          "depletesOnResolve": true
        },
        {
          "choiceId": "search_truck_cab",
          "text": "Investigate the cab for documents or personal effects.",
          "moraleDelta": 0,
          "guiltDelta": 1,
          "grantItemId": "sealed_government_document",
          "grantItemQuantity": 1,
          "depletesOnResolve": true
        },
        {
          "choiceId": "ignore_truck",
          "text": "Move on. Someone already took what was worth taking.",
          "moraleDelta": 0,
          "guiltDelta": 0
        }
      ]
    }
  ]
}
```

### Invariants Enforced
1. **Schema Version**: Must be integer `1`.
2. **Catalog Root**: Root object containing `schema_version` and array `micro_locations`.
3. **No Duplicate IDs**: Every `id` within `micro_locations` is unique, and disjoint from standard `encounters.json` encounter IDs.
4. **Distance Ordering**: `min_distance >= 0`, `max_distance >= min_distance`.
5. **Deterministic Choice Outcomes**: Each choice specifies explicit deltas (`morale_delta`, `guilt_delta`, `standing_deltas`, `cost`, `grant`, `journal_entry`, `discovers_location`, `is_one_time`).

---

## 3. Cross-Reference Referential Integrity

All micro-location rewards, offerings, discoveries, and lore reference authoritative systems without runtime dangling pointers:

### 3.1 Item References (`items.json`)
Every item referenced in `cost` or `grant` payloads must exist in `Assets/StreamingAssets/Data/items.json`:
- Exemplar item keys: `canned_food`, `clean_water`, `scrap_metal`, `first_aid_kit`, `antibiotics`, `filter_cartridge`, `signal_battery`.
- Verified at compile/test time by `MicroLocationExportParityTests.ItemReferences_ResolveInItemCatalog`.

### 3.2 Map Discoveries (`locations.json` & Sector Data)
When a choice has `discovers_location`, the referenced location exists within the world cartography catalog and is unlocked on the player's expedition map upon resolution.

### 3.3 Codex / Journal Knowledge Unlocks
Choices with `journal_entry` trigger knowledge base discovery in `JournalSystem` via `TryDiscoverKnowledge(entryKey, author, day)`:
- If a journal clue key is new, the system dynamically registers it in the codex and dispatches an unlock event with the localized name.

---

## 4. Automated Verification & CI Gates

Export packaging and catalog parity are mechanically gated by xUnit and Godot self-tests:

1. **`MicroLocationExportParityTests`** (`Ashfall.Core.Tests/Expeditions/MicroLocationExportParityTests.cs`):
   - `ExportPresets_IncludeFilter_ContainsJsonAndCsv`: Validates that `export_presets.cfg` includes `*.json` and `*.csv`.
   - `CatalogStructure_MatchesSchemaContract`: Validates `schema_version == 1`, non-empty array, and minimum 25 production entries.
   - `ItemReferences_ResolveInItemCatalog`: Validates that 100% of referenced items exist in `items.json`.
   - `EncounterIds_AreUniqueAndDisjointFromEncountersCatalog`: Validates 0 ID collisions between `micro_locations.json` and `encounters.json`.

2. **Godot CI Selftests**:
   - `godot --headless --path . -- --data-integrity-selftest`: Validates JSON parse and catalog integrity for all catalogs including `micro_locations.json`.
   - `godot --headless --path . -- --content-utilization-selftest`: Validates content utilization baseline.
