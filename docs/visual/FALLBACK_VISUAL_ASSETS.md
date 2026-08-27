# ASHFALL — Active Fallback Visual Assets & Resolution Architecture

**Last Updated:** 2026-08-26
**Scope:** Canonical runtime fallback assets, programmatic fallback generators, item ID aliases, and resolution policies across `AssetRegistry` and Godot UI/World views.

---

## 1. Canonical Static Fallback Textures

The project maintains two canonical on-disk fallback texture assets, centralized under named constants to avoid magic path strings:

| Asset Name | Godot Resource Path | Relative Path | Dimensions | Code Constants | Intended Use / Consumers |
|---|---|---|---|---|---|
| **[`placeholder_survivor.png`](../../assets/sprites/Characters/placeholder_survivor.png)** | `res://assets/sprites/Characters/placeholder_survivor.png` | [`assets/sprites/Characters/placeholder_survivor.png`](../../assets/sprites/Characters/placeholder_survivor.png) · [`.import`](../../assets/sprites/Characters/placeholder_survivor.png.import) | 64×64 RGBA PNG | `AssetRegistry.FallbackSurvivorPath`<br>`AssetRegistry.FallbackSurvivorRelativePath`<br>`AshfallUiHelpers.FallbackSurvivorPath`<br>`AshfallUiHelpers.FallbackSurvivorResPath`<br>`SurvivorActorView.FallbackTexturePath` | Universal character fallback sprite texture when a survivor, NPC, or portrait asset is missing or not yet generated in the asset registry. Used by `SurvivorActorView`, `SurvivorsPanel`, and roster cards. |
| **[`icon_placeholder.png`](../../assets/ui/Icons/icon_placeholder.png)** | `res://assets/ui/Icons/icon_placeholder.png` | [`assets/ui/Icons/icon_placeholder.png`](../../assets/ui/Icons/icon_placeholder.png) · [`.import`](../../assets/ui/Icons/icon_placeholder.png.import) | 32×32 RGBA PNG | `AssetRegistry.FallbackIconPath`<br>`AssetRegistry.FallbackIconRelativePath`<br>`AshfallUiHelpers.FallbackIconPath`<br>`AshfallUiHelpers.FallbackIconResPath`<br>`ResearchAtlasPanel.DefaultDisciplineIconPath` | Universal UI icon fallback texture when an item, skill, discipline, or action icon is missing or not yet resolved. Used by `ResearchAtlasPanel`, `InventoryPanel`, and `AshfallUiHelpers`. |

---

## 2. Programmatic Procedural Fallback Generators

When no on-disk texture exists for a catalog ID, [`src/Host/AssetRegistry.cs`](../../src/Host/AssetRegistry.cs) provides deterministic procedural fallback texture generation:

1. **`AssetRegistry.MakeItemIcon(string itemId)`**
   - Generates a 64×64 `ImageTexture` with a category-coded border and tinted background based on the item category (Medical = red/green, Food = amber, Scrap = gray, Ammo = bronze, Tech = cyan).
2. **`AssetRegistry.MakeBadgeIcon(string label, Color color)`**
   - Generates compact badge textures for status icons, afflictions, discipline headers, and role tags.
3. **`AssetRegistry.MakePortrait(string name, Color color)`**
   - Generates a 96×96 silhouette portrait with initials and distinct background hue derived deterministically from the survivor's name hash.

---

## 3. Runtime Fallback Resolution Flows

### Item Resolution (`AssetRegistry.GetItem`)
```
Catalog Item ID
  │
  ├── 1. Direct match in Assets/art/ or assets/sprites/Items/ -> Return Texture2D
  │
  ├── 2. Match in ItemIdAliases map (e.g. mechanical_* -> scrap_mechanical) -> Return Texture2D
  │
  ├── 3. Procedural generation via MakeItemIcon(itemId) -> Return ImageTexture
  │
  └── 4. Static fallback -> Load AssetRegistry.FallbackIconPath ("res://assets/ui/Icons/icon_placeholder.png")
```

### Survivor & NPC Portrait Resolution (`AssetRegistry.GetSurvivorPortrait`)
```
Survivor / NPC ID
  │
  ├── 1. Direct match in Assets/sprites/Portraits/ or assets/sprites/Characters/ -> Return Texture2D
  │
  ├── 2. Procedural generation via MakePortrait(name, color) -> Return ImageTexture
  │
  └── 3. Static fallback -> Load AssetRegistry.FallbackSurvivorPath ("res://assets/sprites/Characters/placeholder_survivor.png")
```

### Location Art Resolution (`AssetRegistry.GetLocationArt`)
```
Location ID
  │
  ├── 1. Direct match in Assets/art/Locations/ -> Return Texture2D
  │
  └── 2. Default wasteland horizon texture / MakeItemIcon -> Return fallback
```

---

## 4. Item ID Aliases Table

The `AssetRegistry.ItemIdAliases` dictionary normalizes legacy / synonym catalog IDs to existing texture assets:

| Alias Key | Target Asset Key | Target Asset File | Category |
|---|---|---|---|
| `mechanical_components` | `scrap_mechanical` | [`assets/art/scrap_mechanical.png`](../../assets/art/scrap_mechanical.png) | Scrap |
| `mechanical_parts` | `scrap_mechanical` | [`assets/art/scrap_mechanical.png`](../../assets/art/scrap_mechanical.png) | Scrap |
| `9mm_ammo` | `ammo_9mm` | [`assets/art/ammo_9mm.jpg`](../../assets/art/ammo_9mm.jpg) | Ammo |
| `blood_bag` | `item_blood_bag` | [`assets/art/item_blood_bag.jpg`](../../assets/art/item_blood_bag.jpg) | Medical |

---

## 5. Verification & Smoke Gates

1. **`AssetRegistrySelfTest.Run` (`--asset-registry-selftest`)**:
   - Asserts that all 50 critical catalog IDs (items, survivors, locations, factions) resolve cleanly.
   - Asserts that `AssetRegistry.FallbackSurvivorPath` and `AssetRegistry.FallbackIconPath` load without error.
2. **`TradeThemeAndEconomyTests.AssetRegistry_FallbackTextures_ExistOnDisk` (`dotnet test`)**:
   - Asserts that `placeholder_survivor.png` and `icon_placeholder.png` exist on disk as non-empty valid PNG files.
3. **`--asset-coverage-report`**:
   - Non-gating coverage report tracking 834 total catalog entries (481 resolved [57.7%], 353 fallback [42.3%]).

---

## 6. Player-Visible Screen Fallback Audit

The table below traces how fallback art and procedural generators are intentionally consumed across active player-facing UI screens and 2D views:

| Player-Visible Screen / Surface | Consuming View / Component | Fallback Asset / Method Used | Behavior & Trigger Condition |
|---|---|---|---|
| **Research Atlas** | [`src/UI/ResearchAtlasPanel.cs`](../../src/UI/ResearchAtlasPanel.cs) | `AshfallUiHelpers.FallbackIconPath` ([`icon_placeholder.png`](../../assets/ui/Icons/icon_placeholder.png)) | Rendered for discipline sidebar filter categories (Survival, Engineering, Science, Scavenging, Combat) and for breakthrough items where specific item sprite art is pending. |
| **Shelter Interior 2D View** | [`src/World/SurvivorActorView.cs`](../../src/World/SurvivorActorView.cs) | `AssetRegistry.FallbackSurvivorPath` ([`placeholder_survivor.png`](../../assets/sprites/Characters/placeholder_survivor.png)) | Displays the standard 64×64 silhouette sprite for all active bunker dwellers roaming in 2D shelter rooms until dedicated survivor sprite sheets are bound. |
| **Survivors / Roster HUD** | [`src/UI/SurvivorsPanel.cs`](../../src/UI/SurvivorsPanel.cs) | `AssetRegistry.GetPortrait(id)` → `MakePortrait(name, color)` | Generates a deterministic initials badge or loads [`placeholder_survivor.png`](../../assets/sprites/Characters/placeholder_survivor.png) for roster survivors whose specific headshot portrait is unauthored. |
| **Survivor Detail Panel** | [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs) | `AssetRegistry.GetPortrait(id)` | Renders procedural initials badge for inspectable dweller cards when on-disk portrait is absent. |
| **Inventory & Container Overlays** | [`src/UI/InventoryPanel.cs`](../../src/UI/InventoryPanel.cs) | `AssetRegistry.GetItemIcon(itemId)` → `MakeItemIcon(itemId)` | Renders category-coded tinted square icons (Medical, Food, Scrap, Tech) or [`icon_placeholder.png`](../../assets/ui/Icons/icon_placeholder.png) for newly added items and crafting components. |
| **Economy & Barter Panels** | [`src/Economy/EconomyMarketPanel.cs`](../../src/Economy/EconomyMarketPanel.cs) & [`src/Economy/TradeScreenGodotPanel.cs`](../../src/Economy/TradeScreenGodotPanel.cs) | `AssetRegistry.GetItemIcon(itemId)` | Barter item slots display procedural category badges for unillustrated trade goods. |
| **Traveling Caravan Hub** | [`src/UI/TravelingCaravanPanel.cs`](../../src/UI/TravelingCaravanPanel.cs) | `AssetRegistry.GetPortrait(npcId)` & `GetItemIcon(itemId)` | Caravan master portrait and commodity trade goods resolve through `AssetRegistry` procedural generators. |
| **Expeditions & Sortie Deploy** | [`src/UI/ExpeditionPanel.cs`](../../src/UI/ExpeditionPanel.cs) | `AssetRegistry.GetLocationTexture(locId)` & `GetItemIcon(itemId)` | Location preview backdrop and potential loot roll icons fall back gracefully to procedural wasteland horizon and item category badges. |
| **Muster Coalition Camp** | [`src/UI/MusterPanel.cs`](../../src/UI/MusterPanel.cs) | `AssetRegistry.GetFactionEmblem(factionId)` | Renders procedural colored monogram badges for minor coalition factions and witnesses without authored heraldry. |
| **Radio Intercept Log** | [`src/UI/RadioPanel.cs`](../../src/UI/RadioPanel.cs) | `AssetRegistry.GetFactionEmblem(freqId)` | Faction broadcasts display frequency monogram badges on the tuner dial. |

---

## 7. Godot Import Settings & Intended Display Scales

Both canonical fallback textures are imported using Godot 4.7+ lossless 2D sprite presets to prevent scaling artifacts or blur in the 1920×1080 fixed-viewport presentation:

### A. Texture Import Parameters (`.import` files)

Source import settings: [`placeholder_survivor.png.import`](../../assets/sprites/Characters/placeholder_survivor.png.import) · [`icon_placeholder.png.import`](../../assets/ui/Icons/icon_placeholder.png.import)

| Parameter | `placeholder_survivor.png` | `icon_placeholder.png` | Policy Rationale |
|---|---|---|---|
| **Type** | `CompressedTexture2D` | `CompressedTexture2D` | Godot native 2D compressed resource type. |
| **UID** | `uid://c5sb84wwx1qsm` | `uid://bicjnpso2mtlu` | Stable Godot UID resource handle. |
| **`compress/mode`** | `0` (Lossless) | `0` (Lossless) | Guarantees crisp silhouette and glyph edges without JPEG/lossy compression artifacts. |
| **`mipmaps/generate`** | `false` | `false` | Disabled to avoid bilinear downsampling blur on pixel-aligned UI grids. |
| **`process/fix_alpha_border`** | `true` | `true` | Prevents dark halo fringe around transparent cutout borders. |
| **`vram_texture`** | `false` | `false` | Lightweight system RAM decompression suitable for 2D UI elements. |

### B. Intended Display Scales & Render Targets

| Asset | Native Canvas Size | Target View / Component | Transform / Layout Constraint | Effective On-Screen Size |
|---|---|---|---|---|
| **`placeholder_survivor.png`** | 64×64 px | `SurvivorActorView.cs` (2D Bunker) | `Sprite.Scale = new Vector2(0.7f, 0.7f)` | **44.8 × 44.8 px** (scaled to fit room bounds) |
| **`placeholder_survivor.png`** | 64×64 px | `SurvivorsPanel.cs` / Detail Cards | `TextureRect` with `KeepAspectCentered` | **48×48 px / 64×64 px** avatar slots |
| **`icon_placeholder.png`** | 32×32 px | `ResearchAtlasPanel.cs` (Sidebar) | `AshfallSidebar.Item` icon container | **24×24 px / 32×32 px** discipline badges |
| **`icon_placeholder.png`** | 32×32 px | `InventoryPanel.cs` (DataGrid) | `AshfallDataGrid` cell icon slot | **32×32 px / 48×48 px** item cells |
