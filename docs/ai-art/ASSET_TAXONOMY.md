# ASSET_TAXONOMY.md

Asset families for this specific game, grounded in the actual catalogs (not a generic game's taxonomy), plus a per-family model routing table (the "Model Routing Matrix" the first-execution brief calls for).

## Families

1. **Environments** — 7 shelter rooms (Bunkhouse, Medical Bay, Hydroponics, Power Room, Decontamination, Workshop, Radio Room — `Shelter.cs`) + 47 scavenging locations (`locations.json`, establishing/key-art illustrations, not gameplay tiles — the world map is an abstract node graph) + weather overlays (16 kinds, `WeatherSystem.cs` — should be transparent VFX layers composited over an environment, not full re-renders per weather kind).
2. **Characters — survivors** — 96 named archetypes (`survivors.json`: id, displayName, profession, bio). Portraits + occasional full-body. Recurring across sessions — consistency-critical.
3. **Characters — factions/NPC/fauna archetypes** — 5 factions (GDD Ch2.3) + named one-off NPCs (`NPC_AshWidows`, `NPC_TheTollman`, `NPC_BurnedPatrol`, `NPC_TheCollector`, `NPC_FeralChildren`, `NPC_SurgeonsCaravan`, `NPC_Bandits`) + fauna (`Fauna_IrradiatedDogs`, `Fauna_AshCrows`, `Fauna_BloatedCattle`, `Fauna_RatSwarm`). Not individually recurring — lower consistency bar than survivors.
4. **Character status badges** — handled as small icon overlays composited onto ONE portrait (Hypervigilance eye, Moral Branch shield/heart, Addiction pill, Radiation phase dot, Blood type, Terminal prognosis banner), confirmed by the widget specs. **Do not** plan for per-survivor-per-state full re-renders (96 survivors × N states is untenable and isn't how the UI is built) — this family is UI icons, not character variants.
5. **Props & furniture** — crafting stations, beds, water purifier, wood stove, workbenches (`Shelter.cs`, `CraftingSystem.cs`).
6. **Inventory icons** — 419 items across 17 categories (AntiRad, Comfort, Device, Filter, Food, Fuel, Iodine, IrradiatedWater, Material, Medical, Protective, Quest, Relic, Tool, Trade, Water, Weapon — `items.json`). The single largest family by volume; zero currently have art (`audit/ASSET_MANIFEST.md`, deprecated but confirms 0 present as of its last run; `Assets/Resources` still has zero image files today).
7. **Equipment/weapons** — subset of #6 (`Weapon`, `Protective` types — hazmat suits, firearms). Functional readability over glamour, per the project's own tone.
8. **UI icons, badges & textures** — the 8 Phase 11 widgets' assets (this batch's focus) + the ~20 other unwired HUD widgets (`NeedsBar`, `DosimeterHUD`, `MapScreenUI`, `TradeScreenUI`, `WorkbenchUI`, `JournalBookUI`, `PowerGridHUD`, `RoomAssignmentHUD`, etc.). Mostly small icon/texture assets — layout itself is UXML/USS, not art.
9. **VFX overlays** — transparent sprite sheets/textures: muzzle flash, blood splatter, EMP pulse, mushroom cloud, radiation haze, toxic gas, weather particles. Shape + alpha control matter more than photoreal detail.
10. **Key art / marketing** — main menu background (already exists: `UI_StyleReference_01.jpg`), Steam capsule art, loading screens. Kept separate from gameplay-asset prompts on purpose — this register can (and should) carry more atmosphere than an inventory icon.

## Model routing matrix

| Family | Primary | Secondary | Not recommended | Why |
|---|---|---|---|---|
| Environments — shelter rooms / key-art interiors | FLUX.2 [max] for complex/reference-matched shots; FLUX.2 [pro] for simpler ones | Adobe Firefly, Nano Banana Pro | Ideogram, Runway, Kling | Multi-object interiors need compositional control and reference-image support to match the one existing key-art piece |
| Environments — 47 scavenging locations (one-off establishing shots) | FLUX.2 [pro] | Midjourney, Grok Imagine | Ideogram | Volume + painterly quality; no exact-composition need, each is a one-off |
| Characters — survivors (recurring, 96) | Nano Banana Pro | GPT Image 2, FLUX.2 [pro] | Ideogram, Runway | Same character reappears across quests/portraits — reference-image consistency beats raw painterly ceiling |
| Characters — faction/NPC/fauna (one-off) | Nano Banana 2 | Seedream 5.0 Lite, Midjourney | Ideogram | Non-recurring — the cheap/fast tier is enough |
| Props & furniture | Nano Banana 2 | FLUX.2 [pro], Seedream 5.0 Lite | Ideogram, Runway | Mid-volume, moderate detail, transparent background |
| Inventory icons (419, largest family) | Nano Banana 2 | Seedream 5.0 Lite, GPT Image 2 for hero/quest items | Ideogram, Runway, Kling, Midjourney | Price-per-image and consistent transparent-background output matter more than painterly ceiling at this volume |
| Equipment/weapons | Nano Banana 2 | GPT Image 2, Recraft (flat diagram read) | Ideogram | Readability over glamour, per project tone |
| UI icons & badges (small, flat, semantic-color) | Recraft | Nano Banana 2 | FLUX.2, Midjourney (wrong register for a flat 24px icon) | Recraft V4/V4.1 generates true scalable SVG, no tracing/cleanup — the right tool for eye/shield/heart/pill-class icons |
| UI textures & overlays (stone wall, cracked glass, vignettes) | FLUX.2 [pro] | Nano Banana 2, Adobe Firefly | Ideogram, Kling | Painterly material rendering, not vector |
| VFX sprite sheets | Nano Banana Pro | FLUX.2 [pro], Grok Imagine | Ideogram, Recraft | Shape + transparency control over photoreal detail |
| Key art / marketing / loading screens | Adobe Firefly or FLUX.2 [max] | Midjourney, Runway (if a trailer is ever made) | Ideogram (unless a literal logotype is needed) | Kept separate from gameplay prompts; this register can carry more atmosphere |
| Seedream 5.0 Pro | — | layer-precision editing / Seedance video-anchor frames, if ever needed | (default) | Verified to exist (launched 2026-07-08); nothing here currently needs its specialty — hold in reserve |
| Qwen Image | — | general exploratory alt, any family | (default) | No differentiated strength identified for this project yet |

See `IMAGE_MODEL_PROFILES.md` for how to actually prompt each model, and `prompts/pilot_batch.md` for this routing applied to 10 concrete assets.
