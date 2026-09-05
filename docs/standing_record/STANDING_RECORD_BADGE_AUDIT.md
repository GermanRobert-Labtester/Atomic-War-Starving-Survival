# Standing Record Badge & Emblem Resolution Audit

## 1. Executive Summary

This audit verifies the asset resolution pipeline for `badge_asset_id` in `standing_record_factions.json`, ensuring zero crashes, missing-texture purples, or broken paths in both editor and exported headless builds.

---

## 2. Asset Resolution Architecture

In `Assets/Ashfall.Core/UI/FactionIconCatalog.cs`:

```csharp
public static string Resolve(string factionId)
{
    if (string.IsNullOrEmpty(factionId)) return FallbackIconPath;
    return _systemsIdsToIcon.TryGetValue(factionId, out var path) ? path : FallbackIconPath;
}
```

- **Fallback Icon Path:** `assets/ui/Icons/faction_icon_unknown.png`.
- **Policy on `badge_asset_id` in JSON:** All entries in action-style catalogs (`holdfast_factions.json`, `crossing_factions.json`, `standing_record_factions.json`) set `badge_asset_id: ""` (legal empty string).
- **Runtime Resolution:** The UI passes `faction.id` into `FactionIconCatalog.Resolve(id)`. If an explicit mapping exists in `_systemsIdsToIcon`, it loads the specialized emblem; otherwise, it seamlessly returns `FallbackIconPath`.

---

## 3. Faction Emblem Resolution Matrix

| Faction ID | Authored `badge_asset_id` | Catalog Resolution Route | Resolved Path | Visual State |
|---|---|---|---|---|
| `faction_the_overlay` | `""` | `FactionIconCatalog.Resolve("faction_the_overlay")` | `assets/ui/Icons/faction_icon_unknown.png` | Standard Fallback Emblem |
| `faction_the_scale` | `""` | `FactionIconCatalog.Resolve("faction_the_scale")` | `assets/ui/Icons/faction_icon_unknown.png` | Standard Fallback Emblem |
| `faction_the_compact` | `""` | `FactionIconCatalog.Resolve("faction_the_compact")` | `assets/ui/Icons/faction_icon_unknown.png` | Standard Fallback Emblem |
| `faction_the_underwrite` | `""` | `FactionIconCatalog.Resolve("faction_the_underwrite")` | `assets/ui/Icons/faction_icon_unknown.png` | Standard Fallback Emblem |
| `faction_the_cutters` | `""` | `FactionIconCatalog.Resolve("faction_the_cutters")` | `assets/ui/Icons/faction_icon_unknown.png` | Standard Fallback Emblem |
| `faction_the_fleet` | `""` | `FactionIconCatalog.Resolve("faction_the_fleet")` | `assets/ui/Icons/faction_icon_unknown.png` | Standard Fallback Emblem |
| `faction_the_rebuilders` | `""` | `FactionIconCatalog.Resolve("faction_the_rebuilders")` | `assets/ui/Icons/faction_icon_unknown.png` | Standard Fallback Emblem |
| `faction_the_garrison` | `""` | `FactionIconCatalog.Resolve("faction_the_garrison")` | `assets/ui/Icons/faction_icon_unknown.png` | Standard Fallback Emblem |

---

## 4. Verification & Build Safety

1. **No Invented Asset Paths:** Setting `badge_asset_id: ""` completely avoids inventing fake asset paths (e.g. `icon_scale.png`) that would trigger asset-lint or packaging errors.
2. **Exported Build Safety:** The fallback icon `assets/ui/Icons/faction_icon_unknown.png` is tracked in Git LFS, registered in Godot root `assets/`, and tested in headless PCK export pipelines.
3. **Follow-On Art Work:** Dedicated heraldic vector art can be introduced in a future asset pack by populating `_systemsIdsToIcon` or updating `badge_asset_id` once art assets are created and checked in.
