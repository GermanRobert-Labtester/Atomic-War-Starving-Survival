# Pilot batch — 10 assets

Validates the system end to end before scaling to the full catalog. 8 of 10 assets clear the actively-uncommitted Phase 11 HUD work (`INTEGRATION_PLAN_FOR_CURSOR.md` §III's Canva list — currently 0/12 generated); 2 demonstrate the system against the game's large catalogs (inventory, survivors) and the recurring-location case (environment).

Each entry: Master Asset Brief → primary-model prompt → universal master (model-agnostic canonical version) → one alternative. Full model matrix intentionally not generated per asset — see `PROMPT_RULES.md`.

---

## `stone_wall_bg` — Memorial Wall stone background

**Category:** UI texture · **Purpose:** `MemorialWallUI` full-screen modal background (`wall-background` element) · **Recommended generator:** FLUX.2 Pro · **Alternative:** Nano Banana 2 · **Reference recommended:** Yes (`UI_StyleReference_01.jpg`, for linework/palette) · **Aspect ratio:** 1:1, 1024×1024, opaque

### Visual brief
Dark engraved slate wall, flat and evenly lit so foreground text stays readable against it.

### FLUX.2 Pro
> Dark slate stone wall surface, faint hand-carved engraving lines scattered across it, no readable text. Evenly lit, minimal directional shadow so it sits behind UI text. 2D graphic-novel illustration, inked linework, cross-hatch texture, charcoal grey with a cold blue undertone, desaturated. Flat frontal composition, square format.

### Universal master
> Dark engraved slate stone wall texture, no legible text, flat even lighting for a UI background. Gritty 2D graphic-novel illustration, inked linework, desaturated charcoal-grey palette with a cold blue undertone. Square, 1024×1024.

### Nano Banana 2 (alt)
> Purpose: full-screen UI modal background behind memorial text entries. Subject: dark engraved slate stone wall, no legible text. Style: 2D graphic-novel illustration, inked linework, charcoal-grey desaturated palette. Lighting: flat, even — avoid strong directional shadow that would fight foreground text. Format: 1024×1024 square.

### Notes
Zero art exists for this yet (`Assets/_Game/UI/Phase11/Canva/` is empty besides `.gitkeep`) — first asset for `MemorialWallUI`'s visual polish.

---

## `cracked_glass_overlay` — Keepsake cracked-glass overlay

**Category:** VFX/UI overlay · **Purpose:** `KeepsakeSlotUI` "Lost" state overlay · **Recommended generator:** FLUX.2 Pro · **Alternative:** Nano Banana 2 · **Reference recommended:** No · **Aspect ratio:** 1:1, 128×128, transparent

### Visual brief
Spiderweb crack lines only, fully transparent elsewhere — composites directly over an item icon.

### FLUX.2 Pro
> Cracked glass overlay texture, spider-web fracture lines radiating from one impact point, fully transparent background. Thin white-grey crack highlights only — no glass pane fill, no frame. 2D graphic-novel ink linework style. Square, centered.

### Universal master
> Spiderweb crack-line pattern only, no glass fill, fully transparent background, thin inked white-grey highlights. Square, 128×128, must stay legible composited at 48×48px.

### Nano Banana 2 (alt)
> Purpose: composited overlay on an item icon to show a lost/broken keepsake. Subject: spiderweb crack pattern only, transparent background, no glass-pane fill. Style: thin inked linework matching the game's graphic-novel style. Constraint: must read clearly at 48×48px in-game — keep cracks bold, not fine detail. Format: 128×128 PNG, transparent.

### Notes
Pairs with a red grief-vignette that's opacity-driven by `KeepsakeGriefLevel` in USS, not art — don't generate that part.

---

## `vignette_sepia` / `vignette_blue` — Phantom Memory vignette pair

**Category:** VFX/UI overlay · **Purpose:** `PhantomMemoryVignette` full-screen radial gradient (Motivation vs. Breakdown trigger) · **Recommended generator:** Nano Banana Pro · **Alternative:** FLUX.2 Pro · **Reference recommended:** No (`design-tokens.json` is the color ground truth) · **Aspect ratio:** 1:1, 512×512, transparent center

### Visual brief
A pure radial gradient, no objects — demonstrates the asset-variation pattern: one brief, one changed variable.

### Nano Banana Pro — `vignette_sepia` (Motivation)
> Purpose: full-screen radial vignette overlay behind a short memory-flashback caption. Subject: soft radial gradient, warm sepia at the edges fading fully transparent at center — no objects, no texture, pure gradient. Color: sepia edge close to #b98a5e. Constraint: center 40% must stay clear so overlaid text stays readable. Format: 512×512 PNG, transparent center.

### `vignette_blue` (Breakdown) — everything invariant except:
> Color: cold blue-grey edge close to #6ea3a8 (replaces the sepia edge). Same radius, falloff, transparent center, and format.

### Universal master
> Soft radial gradient, [sepia #b98a5e / cold blue-grey #6ea3a8] at the edges fading to fully transparent at center, no objects or texture. Center 40% clear for overlaid text. 512×512, transparent center.

### FLUX.2 Pro (alt)
> Soft radial vignette gradient, warm sepia-brown edge fading to fully transparent center, no objects. Smooth painterly falloff, no linework texture. Square, 512×512.

### Notes
Near-procedural — if an in-engine USS radial gradient hits these exact colors more reliably than a generator, that's a legitimate call to make; included here mainly to demonstrate the variation-pair pattern from `PROMPT_RULES.md`.

---

## `icon_eye` — Hypervigilance eye badge

**Category:** UI icon (vector family, generated first — becomes the anchor for the rest) · **Purpose:** `HypervigilanceIndicator`, 12×12px badge · **Recommended generator:** Recraft · **Alternative:** Nano Banana 2 · **Reference recommended:** No (first of its family) · **Aspect ratio:** 1:1, 24×24, transparent, SVG

### Visual brief
Minimalist single-stroke eye icon, semantic amber, no shading.

### Recraft
> Minimalist eye icon, single stroke-weight line art, no fill gradients, symmetrical. Amber #FFC107 line on transparent background. Flat vector icon, legible at 12px. No photorealism, no shading.

### Universal master
> Flat minimalist vector eye icon, single stroke weight, symmetrical, amber #FFC107, transparent background, legible at 12px.

### Nano Banana 2 (alt, raster fallback)
> Small flat eye icon badge, amber outline, transparent background, minimal detail, reads clearly at 12px.

### Notes
First of six planned same-family icons — its stroke weight and corner rounding become the reference for `icon_shield`, `icon_heart`, `icon_pill`, `icon_hourglass`, `icon_checkmark`.

---

## `icon_shield` / `icon_heart` — Moral Branch icon pair

**Category:** UI icon (vector family) · **Purpose:** `MoralBranchDisplay` — NumbedResilience vs. BurdenedCompassion · **Recommended generator:** Recraft · **Reference recommended:** Yes, match `icon_eye`'s stroke weight once generated · **Aspect ratio:** 1:1, 32×32, transparent, SVG

### Recraft — `icon_shield` (NumbedResilience)
> Minimalist steel shield icon, single stroke-weight line art, symmetrical, grey #9E9E9E line on transparent background. Flat vector icon, legible at 16px. No shading, no gradient, same stroke weight as the eye-badge icon.

### `icon_heart` (BurdenedCompassion) — everything invariant except:
> Shape: heart instead of shield. Color: #6ea3a8 (blue-teal).

### ⚠️ Color note
`INTEGRATION_PLAN_FOR_CURSOR.md` specifies this heart icon as `#42A5F5` inline; `design-tokens.json`'s `moral_compassion` is `#6ea3a8` — a more muted teal-blue. **Use `#6ea3a8`** — it's the value that actually feeds the shipped USS. Flagged in `EXISTING_PROMPT_AUDIT.md`, not silently picked.

### Universal master
> Flat minimalist vector icon — [shield #9E9E9E / heart #6ea3a8] — single stroke weight, symmetrical, transparent background, legible at 16px, matches the eye-badge icon's stroke weight.

---

## `icon_pill` — Addiction Detox pill badge

**Category:** UI icon (vector family) · **Purpose:** `AddictionDetoxIndicator`, "Dependent" state · **Recommended generator:** Recraft · **Reference recommended:** Yes, match `icon_eye` · **Aspect ratio:** 1:1, 24×24, transparent, SVG

### Recraft
> Minimalist pill/capsule icon, single stroke-weight line art, symmetrical, orange #c97b3a line on transparent background. Flat vector icon, legible at 12px. No shading, no gradient.

### Universal master
> Flat minimalist vector pill icon, single stroke weight, symmetrical, orange #c97b3a, transparent background, legible at 12px.

### Notes
`icon_hourglass` (Managed detox, `#FFC107`) and `icon_checkmark` (Recovered, `#4CAF50`) are the same recipe — swap the symbol and color only, don't re-derive the brief.

---

## `memorial_name_plate` — Memorial name plate template

**Category:** UI prop/texture · **Purpose:** `MemorialWallUI` per-entry name plate · **Recommended generator:** FLUX.2 Pro · **Alternative:** Nano Banana 2 · **Reference recommended:** No · **Aspect ratio:** ~6.7:1, 400×60, transparent or dark background

### Visual brief
Blank engraved plate — **must stay text-free**, the game composites the survivor's name at runtime.

### FLUX.2 Pro
> Blank engraved metal name-plate template, horizontal rectangle, dark tarnished brass with a faint carved border, no text. Even flat lighting for UI use. 2D graphic-novel illustration, inked linework, desaturated. 400×60, transparent or dark background.

### Universal master
> Blank horizontal engraved brass name-plate template, faint carved border, no text, even flat lighting, desaturated inked illustration style. 400×60.

### Notes
This is exactly why it's routed to FLUX.2 Pro instead of a text-strong model: GPT Image 2's 95%+ text accuracy is a liability here, not an asset — a model eager to render text is likely to bake in placeholder lettering that then has to be removed.

---

## `geiger_counter` — Geiger Counter (inventory icon)

**Category:** Inventory icon · **Purpose:** item icon for `geiger_counter` (`items.json`: type `Device`, weight 1.0kg, trade value 42, description "Live rate meter") · **Recommended generator:** Nano Banana 2 · **Alternative:** GPT Image 2 (hero-item pass) · **Reference recommended:** No yet — becomes the `Device`-category anchor · **Aspect ratio:** 1:1, transparent

### Visual brief
Vintage analog field instrument, worn but functional — the item's whole gameplay purpose is reading it, so the dial must be legible.

### Nano Banana 2
> Purpose: inventory icon for a vintage Geiger counter, a Device-category item. Subject: olive-green metal casing, round analog needle gauge, speaker grille, small toggle switch, worn paint and scuffed corners. Style: 2D graphic-novel illustration, inked linework, chiaroscuro rim light, desaturated ash-blue and rust palette. Composition: 3/4 view, isolated. Format: transparent background, square.

### Universal master
> Vintage olive-green Geiger counter, round analog needle gauge, speaker grille, worn paint. 2D graphic-novel illustration, inked linework, desaturated ash-blue/rust palette. 3/4 view, transparent background.

### GPT Image 2 (alt, hero-item pass)
> A weathered olive-green Geiger counter for a survival game's inventory icon: round analog dial, speaker grille, worn paint at the corners. Match a gritty inked graphic-novel style with a desaturated cold palette. 3/4 view, isolated on a transparent background, no other objects.

### Notes
This is the template for the other 418 items — see `ASSET_TAXONOMY.md` for the full-catalog routing pattern. Becomes the `Device`-category anchor (pairs with `dosimeter`, which shares the same category and should read as part of the same object family).

---

## `elena_vasquez` — Survivor portrait

**Category:** Character portrait · **Purpose:** portrait card for survivor `elena_vasquez` (`survivors.json`: Paramedic; "Former field medic who ran triage in the first hours after the exchange. She doesn't talk about what she saw, but her hands never shake.") · **Recommended generator:** Nano Banana Pro · **Alternative:** FLUX.2 Pro · **Reference recommended:** Yes — this generation becomes her anchor · **Aspect ratio:** portrait (~4:5), transparent or softly blurred background

### Visual brief
Restrained, not distressed — exhaustion shown through detail (dark circles) but composure through the deliberate "steady hands" detail from her bio.

### Nano Banana Pro
> Purpose: recurring survivor portrait card, used across HUD and quest UI. Subject: Elena Vasquez, a paramedic in her late 30s, thin from rationing, steady and composed despite visible exhaustion — dark circles under her eyes, but her hands held still and sure. Wears a scavenged medical smock over practical layered clothing. Setting: plain, slightly out-of-focus shelter interior. Composition: head-and-shoulders, 3/4 view. Lighting: single warm practical light source, chiaroscuro. Style: 2D graphic-novel illustration, inked linework, desaturated ash-blue/charcoal palette. Constraint: restrained expression, not heroic or distressed. Format: portrait orientation, transparent or softly blurred background.

### Universal master
> Elena Vasquez, weary paramedic in her late 30s, composed expression, dark circles, hands held steady. Scavenged medical smock over layered clothing. 2D graphic-novel illustration, inked linework, chiaroscuro lighting, desaturated ash-blue/charcoal palette. Head-and-shoulders 3/4 view portrait.

### FLUX.2 Pro (alt)
> Elena Vasquez, weary paramedic in her late 30s, steady composed expression, dark circles under her eyes, hands held still. Scavenged medical smock over layered clothing. 2D graphic-novel illustration, inked linework, chiaroscuro lighting, desaturated ash-blue and charcoal palette. Head-and-shoulders 3/4 view portrait, shallow-blurred shelter interior background.

### Notes
Template for the other 95 survivor archetypes. She's the first pilot character with a named quest already in the codebase (`quest_elena_triage`), making her the most likely to need a second, consistent appearance soon — see `CONSISTENCY_ANCHORS.md`.

---

## `env_shelter_medical_bay` — Shelter Medical Bay

**Category:** Environment / dual-use key art · **Purpose:** establishing interior for the Medical Bay shelter room (`Shelter.cs`); doubles as loading-screen-caliber art · **Recommended generator:** FLUX.2 [max] · **Alternative:** Adobe Firefly · **Reference recommended:** Yes, mandatory (`UI_StyleReference_01.jpg`) · **Aspect ratio:** 16:9 landscape, opaque

### Visual brief
Cramped, orderly-despite-the-wear underground medical bay — must sit convincingly next to the one existing shelter piece.

### FLUX.2 Max
> A cramped underground shelter medical bay: one metal-frame cot with a thin mattress, a battered supply cabinet with its door ajar showing bandages and bottles, a wash basin, exposed overhead pipes, bare concrete walls stained with damp. A single caged bulb casts warm light across the cot; the rest of the room falls into cold shadow. A blood-transfusion kit and a clipboard rest on a side table. 2D graphic-novel illustration, inked linework, cross-hatch shading, desaturated ash-blue and charcoal palette with warm amber practical light. Eye-level, straight-on framing, empty of people, claustrophobic and orderly despite the wear.

### Universal master
> Cramped underground shelter medical bay: metal-frame cot, battered open supply cabinet with bandages, wash basin, exposed pipes, damp concrete walls. One caged bulb as the only warm light source, rest of the room in cold shadow. 2D graphic-novel illustration, inked linework, desaturated ash-blue/charcoal palette. Eye-level, straight-on, empty of people.

### Adobe Firefly (alt)
> Concept art of a shelter medical bay dug into a bunker: metal cot, battered supply cabinet with visible bandages, wash basin, exposed pipes, damp concrete walls. One caged bulb as the only warm light source, rest of the room in cold shadow. 2D graphic-novel painterly style, gritty inked texture, desaturated cold palette, claustrophobic mood, no people.

### Notes
Highest consistency priority in this batch — the first environment beyond the one that already exists, and the reference-match bar (`UI_StyleReference_01.jpg`) is the real test of whether the visual DNA spec actually holds up in a second generation.
