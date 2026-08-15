# GAME_VISUAL_DNA

Updated: 2026-08-13. This is the prompt-level visual source of truth for the current playable build.

## Actual player presentation

- **Runtime:** UI Toolkit panels over a near-black orthographic camera clear; the current gameplay scene contains no authored 2D world or room renderer.
- **Reference scale:** 1920×1080 UI; inventory art is read as small square thumbnails and must survive reduction to 64–128 px.
- **Main menu:** the full-bleed `Assets/UI_StyleReference_01.jpg` is COMPLETE. Preserve its right-weighted ruined radio-room composition and left-side title space.
- **Destination gate:** inventory icons have a live resolver at `Resources/Art/Items/<item_id>`. Survivor, location, weather and faction images have data or placeholder folders but no current image-bearing UI destination, so they are deferred rather than generated.

## Canonical visual DNA

### Medium
Dry-gouache digital illustration with restrained charcoal/ink edges. Inventory objects read like worn product studies, not flat UI glyphs and not photographs.

### Perspective
- Inventory: single isolated object, centered, three-quarter view unless side profile is needed for silhouette; no hands.
- Documents: shallow three-quarter view with a small readable overlap; text remains abstract and illegible.
- Current environment reference: eye-level, straight-on stage framing. Do not use isometric room views unless gameplay later adopts them.
- Future portraits: chest-up, three-quarter view, eye-level; not in this production queue.

### Palette
- Dominant: near-black `#090b0c`, ash grey, cold concrete blue-grey.
- Secondary: oxidized steel, dirty canvas, muted mud brown, dull medical cream.
- Accent: rust orange and restrained terminal amber `#d3aa62` / `#f4c875`.
- Semantic UI colors remain code-driven and are not baked into inventory art.

### Shape language
Compact, repairable, asymmetric objects with exposed fasteners, reinforced corners, patched seams and simple readable silhouettes. Avoid decorative futurism.

### Material language
Corroded steel, brushed aluminum, cloudy glass, cracked rubber, worn canvas, splintered wood, brittle paper, ceramic filters, oxidized copper and repaired electronics.

### Surface condition
Used, cleaned enough to handle, field-repaired and ash-ground. Wear follows contact points and function. No random grime overlays and no graphic gore.

### Lighting
One soft top-left rim/key light with weak amber bounce; black edges remain clean for UI blending. Avoid theatrical lens effects or multiple colored lights.

### Atmosphere
Atmosphere is visible only through dust, condensation, mineral scale, soot or oxide on the object. No smoke cloud, environmental backdrop or invisible lore.

### Detail density
One primary silhouette, two or three functional details, one wear story. Fine markings must not be required for identification at thumbnail size.

### Exclusions
No isometric gameplay rooms, pixel art, glossy mobile-game rendering, neon cyberpunk, pristine consumer packaging, readable AI text, real flags or insignia, brand logos, fantasy mutations, magical glow, trophy framing, firing effects, graphic injury, or glorified violence.
