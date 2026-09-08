# Faction War Content Utilization Audit

> **Catalog Authority:** `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
> **Loader:** `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalogLoader.cs`
> **Consumer:** `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`
> **Self-Test Gate:** `godot --headless --path . -- --content-utilization-selftest`

---

## 1. Content Utilization Architecture

In ASHFALL, all authored JSON catalogs must be actively utilized and registered in `content_utilization_baseline.json` with status `GAMEPLAY_CONSUMED`.

### 1.1 Loader Verification
`FactionWarContentCatalogLoader.LoadLocationOverrides` parses the catalog into strongly typed records upon game initialization or catalog warm-up:
```csharp
var text = fileIO.ReadAllText(path);
var root = serializer.Deserialize<FactionWarLocationOverridesRoot>(text);
```
Both `Assets/StreamingAssets/Data/faction_war_location_overrides.json` and its Linux build mirror `builds/linux/Assets/StreamingAssets/Data/faction_war_location_overrides.json` are maintained with identical content.

---

## 2. In-Game Consumption Surfaces

Overrides loaded into `FactionWarContentCatalog` feed into the following game presentation layers:
1. **World Map Tooltips & Labels:** When hovering over or selecting a location on the overland map, `GetActiveLocationOverride` intercepts the standard location display name to show its temporal state (e.g. `Grain Silo (Hardened Outpost)`).
2. **Expedition Target Briefings:** When survivors prepare expeditions, the mission briefing displays `descriptionOverride` and `atmosphereOverride`, giving accurate visual and environmental forewarning of site status.
3. **Location Encounter Atmosphere:** Random encounters at that location adopt the overridden atmosphere string, enhancing environmental continuity.

---

## 3. Catalog Integrity & Schema Verification

`CatalogIntegrityValidator` and the Godot headless tests verify:
- `schema_version: 1` is present at the root of `faction_war_location_overrides.json`.
- All 20 items possess non-null, non-whitespace required fields.
- Zero cyclic or broken references exist across the location registry.
