# ASHFALL — COMPREHENSIVE UI DESIGN PLAN

### Figma MCP (Edu Pro) + Canva AI Premium
> **Goal**: Design a cohesive, high-quality UI for ASHFALL that looks like a premium survival-management game, not a prototype.
> **Current state**: 19 widget C# files exist with logic ✅ | 11 UXML stubs are empty ❌ | 43 Canva assets not generated ❌ | No polished design system ❌

---

## I. DESIGN PHILOSOPHY — THE ASHFALL UI LANGUAGE

### Core Principles
1. **Diegetic, not digital.** Every UI element should feel like it exists *in the bunker* — scratched gauges, worn labels, taped notes, improvised displays. No clean digital interfaces.
2. **Restrained palette.** Charcoal `#1A1A1A`, concrete grey `#424242`, rust brown `#795548`, dirty bone `#BCAAA4`, muted amber `#FFC107`. Cyan-green `#00BCD4` ONLY for radiation indicators.
3. **Typography as character.** Barlow Condensed — tight, efficient, military. No decorative fonts. SemiBold for headings, Regular for body, 11px minimum.
4. **Every pixel earns its place.** If a UI element doesn't convey game-critical information, it doesn't exist.
5. **Wear and damage is UI chrome.** Scratches, grime, tape marks, and chipped paint are our borders, shadows, and dividers. Not decorative — atmospheric.

### Emotional Register
The UI should feel like someone in the bunker *made it* — scrawled labels on salvaged screens, repurposed military gauges, handwritten tallies. The player should feel the weight of every decision through the interface itself.

---

## II. FIGMA MCP — DESIGN SYSTEM EXTRACTION

### Step 1: Create the ASHFALL Design System in Figma

Before generating any assets, build the design system:

```
1. Create a new Figma file: "ASHFALL — UI Design System"
2. Define color styles (named, reusable):
   - ashfall/background-primary: #1A1A1A
   - ashfall/background-panel: #2C2C2C
   - ashfall/background-modal: rgba(20,20,20,0.97)
   - ashfall/border-default: rgba(255,193,7,0.15)
   - ashfall/border-active: rgba(255,193,7,0.5)
   - ashfall/border-danger: rgba(244,67,54,0.5)
   - ashfall/text-primary: #E0E0E0
   - ashfall/text-secondary: #9E9E9E
   - ashfall/text-accent: #FFC107 (amber)
   - ashfall/text-danger: #F44336
   - ashfall/text-success: #4CAF50
   - ashfall/radiation-glow: #00BCD4 (cyan-green, sparingly)
   - ashfall/progress-fill: #FFC107
   - ashfall/progress-bg: rgba(255,193,7,0.1)

3. Define typography styles:
   - ashfall/h1: Barlow Condensed SemiBold, 28px, letter-spacing 1.2
   - ashfall/h2: Barlow Condensed SemiBold, 22px, letter-spacing 1.0
   - ashfall/h3: Barlow Condensed SemiBold, 18px, letter-spacing 0.8
   - ashfall/body: Barlow Condensed Regular, 14px, letter-spacing 0.5, line-height 1.4
   - ashfall/small: Barlow Condensed Regular, 11px, letter-spacing 0.3
   - ashfall/mono: Barlow Condensed Regular, 12px, letter-spacing 1.5 (for data/numbers)
   - ashfall/label: Barlow Condensed Regular, 10px, letter-spacing 0.8, uppercase

4. Define spacing grid:
   - ashfall/spacing-xs: 4px
   - ashfall/spacing-sm: 8px
   - ashfall/spacing-md: 12px
   - ashfall/spacing-lg: 16px
   - ashfall/spacing-xl: 24px

5. Define corner radius:
   - ashfall/radius-sm: 2px (bars, indicators)
   - ashfall/radius-md: 4px (panels, cards)
   - ashfall/radius-lg: 8px (modals)

6. Export design tokens as JSON (Figma → Tokens Studio plugin)
```

### Step 2: Extract via Figma MCP

```json
{
  "queries": [
    "Get all color styles with hex values and opacity",
    "Get all typography styles with font family, size, weight, and letter-spacing",
    "Get the panel component with border-radius, border-color, background, and padding values",
    "Get the progress bar component with fill color, background color, and height",
    "Get the button component variants (default, hover, disabled, danger) with all states",
    "Get the modal overlay component with backdrop blur and background opacity",
    "Get the tab bar component with active/inactive states and underline animation",
    "Get the scrollbar component with thumb color, track color, and width",
    "Get the badge/indicator component with size, border-radius, and color variants"
  ]
}
```

### Step 3: Convert to USS Variables

From the Figma tokens, generate USS custom properties:

```css
:root {
    /* Colors */
    --color-bg-primary: #1A1A1A;
    --color-bg-panel: #2C2C2C;
    --color-bg-modal: rgba(20, 20, 20, 0.97);
    --color-border-default: rgba(255, 193, 7, 0.15);
    --color-border-active: rgba(255, 193, 7, 0.5);
    --color-text-primary: #E0E0E0;
    --color-text-secondary: #9E9E9E;
    --color-text-accent: #FFC107;
    --color-progress-fill: #FFC107;
    --color-progress-bg: rgba(255, 193, 7, 0.1);

    /* Typography */
    --font-h1: 28px BarlowCondensed-SemiBold;
    --font-h2: 22px BarlowCondensed-SemiBold;
    --font-body: 14px BarlowCondensed-Regular;
    --font-small: 11px BarlowCondensed-Regular;

    /* Spacing */
    --space-xs: 4px;
    --space-sm: 8px;
    --space-md: 12px;
    --space-lg: 16px;
    --space-xl: 24px;
}
```

---

## III. CANVA AI PREMIUM — HIGH-QUALITY ASSET GENERATION

### Why Canva AI Premium for ASHFALL UI

Canva AI Premium provides:
- **AI texture generation** — worn metal, scratched plastic, grime overlays
- **AI icon generation** — consistent stylized icons matching the hand-painted aesthetic
- **Smart resize** — generate at exact Unity UI Toolkit dimensions
- **Brand kit** — enforce palette consistency across all assets
- **Batch export** — all assets in correct format with one click

### Canva Brand Kit Setup

Before generating anything, configure the Canva Brand Kit:
```
Primary:   #1A1A1A (Charcoal bg)
Secondary: #2C2C2C (Panel bg)
Accent:    #FFC107 (Amber)
Danger:    #F44336 (Red)
Success:   #4CAF50 (Green)
Neutral:   #9E9E9E (Grey)
Radiation: #00BCD4 (Cyan-green)
Font:      Barlow Condensed (upload if not available)
```

### Asset Generation — 4 Categories

---

#### Category 1: UI TEXTURES (Canva AI Image Generator)

These are the *materials* that give ASHFALL UI its worn, diegetic feel. Generate at high resolution, then scale down.

| # | Asset | Canva AI Prompt | Size | Usage |
|---|-------|----------------|------|-------|
| 1 | `tex_panel_steel` | "Close-up of scratched dark grey steel plate, subtle horizontal grain, small rust spots at edges, industrial metal panel texture, no text, seamless tile" | 512×512 | Panel backgrounds |
| 2 | `tex_panel_concrete` | "Aged concrete wall surface, hairline cracks, water stains near the bottom, grey-beige tone, bunker wall texture, no text, seamless tile" | 512×512 | Bunker wall elements |
| 3 | `tex_grime_overlay` | "Dirt and grime accumulation in corners and edges, dark smudge marks, dust particles, transparent background, grunge overlay" | 1024×1024 | Universal griming layer |
| 4 | `tex_scratches_overlay` | "Fine surface scratches and wear marks on dark material, micro-abrasions, scuffed texture, transparent background" | 512×512 | Panel scratch overlay |
| 5 | `tex_tape_residue` | "Duct tape residue marks on dark surface, adhesive remnants, torn tape edges, transparent background" | 256×256 | UI "sticker" elements |
| 6 | `tex_radiation_glow` | "Soft cyan-green bioluminescent glow, feathered edges, faint particle motes, transparent background, subtle radiation shimmer" | 256×256 | Radiation indicators |
| 7 | `tex_parchment_dark` | "Dark stained parchment, burned edges, uneven fiber texture, sepia-brown tones on near-black, no text, seamless" | 512×512 | Codex/journal backgrounds |
| 8 | `tex_gauge_face` | "Vintage military gauge face, scratched glass, faded markings, needle at rest, amber backlight glow, circular, no text except worn measurement marks" | 256×256 | Gauge widget backgrounds |

---

#### Category 2: UI ICONS (Canva AI Vector Generator)

Generate as SVG vectors for crisp rendering at any scale. Canva AI Premium supports SVG output.

| # | Icon Set | Count | Description |
|---|---------|-------|-------------|
| 9 | `icons_phase_dots` | 6 | Healthy (green circle), Prodromal (amber pulsing), Latent (green with "?"), Manifest (red flashing), Chronic (grey), Recovery (blue) — each 32×32 SVG |
| 10 | `icons_skulls_danger` | 6 | Danger level indicators: 1 skull through 5 skulls + empty, amber on dark bg, 24×24 SVG |
| 11 | `icons_faction_emblems` | 5 | Garrison (shield+sword), Militia (wheat+rifle), Cult (radiation trefoil+hood), Warlords (skull+coins), Scavengers (pack+rope) — 64×64 SVG |
| 12 | `icons_tactical_commands` | 5 | Hold Line (shield wall), Retreat (arrow backward), Suppressive Fire (burst), Deploy Trap (explosion ring), Decon Flush (steam burst) — 32×32 SVG |
| 13 | `icons_vehicle_mods` | 6 | Winch (hook), Armored Ram (spiked bumper), Solar Array (panel+sun), Medical Bay (red cross), Command Post (antenna), Cargo Trailer (box+axle) — 24×24 SVG |
| 14 | `icons_era_timeline` | 4 | Pre-Exchange (city skyline), Hour Zero (mushroom cloud), Black Sky (darkness+ash), Ashfall (buried ruins) — 32×32 SVG |
| 15 | `icons_status_badges` | 8 | Biohazard, Expired Clock, Checkmark, Lock, Warning Triangle, Eye (hypervigilance), Pill (dependency), Heart (compassion) — 16×16 SVG |
| 16 | `icons_relationship_lines` | 4 | Hostile (red jagged), Suspicious (orange dashed), Neutral (grey dotted), Allied (blue solid glow) — 64×8 SVG |

---

#### Category 3: WIDGET BACKGROUNDS (Canva AI Image Generator)

Full widget background compositions, generated as complete images then split into 9-slice for Unity UI Toolkit.

| # | Asset | Canva AI Prompt | Size | Widget |
|---|-------|----------------|------|--------|
| 17 | `bg_memorial_wall` | "Dark stone memorial wall, engraved name plates with scratched text, single amber candle glow, solemn underground atmosphere, hand-painted style, no readable text on the names" | 1024×768 | MemorialWallUI |
| 18 | `bg_siege_status` | "Industrial warning panel, cracked red warning stripe, scratched metal frame, amber alert lights, pulsing danger indicator, dark bunker wall behind" | 1024×48 | SiegeStatusHUD |
| 19 | `bg_vehicle_dashboard` | "Worn vehicle dashboard, scratched gauge cluster, amber backlit dials, chipped paint on metal frame, industrial aesthetic" | 800×400 | VehicleStatusPanel |
| 20 | `bg_lore_codex` | "Ancient leather-bound book cover on dark wood desk, worn spine, singed page edges, single amber lamp glow, scholarly bunker atmosphere" | 1024×768 | LoreCodexPanel |
| 21 | `bg_faction_map` | "Hand-drawn tactical map on stained paper, compass rose, faction territory markings in colored pencil, coffee-ring stains, folded crease marks" | 600×600 | FactionRelationshipMap |
| 22 | `bg_character_arc` | "Vertical timeline carved into dark wood panel, brass marker pins, etched roman numerals I-IV, warm amber light from above, handcrafted memorial aesthetic" | 400×600 | CharacterArcProgressPanel |
| 23 | `bg_keepsake_slot` | "Ornate tarnished gold frame inset into dark wood, velvet lining worn thin, single display alcove, museum-display lighting, personal shrine aesthetic" | 128×128 | KeepsakeSlotUI |

---

#### Category 4: ANIMATED ELEMENTS (Canva AI + manual CSS)

| # | Element | Canva Generation | CSS Animation |
|---|---------|-----------------|---------------|
| 24 | `anim_pulse_amber` | Generate 3-frame sprite sheet of amber dot at 100%→50%→100% opacity | `animation: pulse 1s infinite` |
| 25 | `anim_flash_red` | Generate 3-frame sprite sheet of red dot at 100%→30%→100% opacity | `animation: flash 0.5s infinite` |
| 26 | `anim_shake_icon` | Generate eye icon with 3 offset positions | `animation: shake 0.3s infinite` |
| 27 | `anim_scan_line` | Generate thin horizontal line at varying opacity | `animation: scanline 2s linear infinite` |

---

## IV. FIGMA → CANVA → UNITY PIPELINE

### Workflow Per Widget

```
FIGMA                           CANVA                          UNITY
─────                           ─────                          ─────
1. Design widget layout    →    2. Generate textures     →    3. Import .png/.svg
   (colors, spacing,              and icons via                Set Sprite type
    typography from               Canva AI Premium              Assign in Inspector
    design system)
                                                          4. Write USS stylesheet
                             5. Export assets at              referencing imported
                                correct resolutions           textures
                                (png for textures,
                                 svg for icons)         6. Wire in UXML with
                                                              matching element names
```

### Resolution Specs for Unity UI Toolkit
```
Panel backgrounds:  1024×768  → 9-slice at 16px borders → scales to any resolution
Icons:              32×32     → imported as Vector Image or Sprite at PPI=100
Textures:           512×512   → seamless tile if needed, Sprite at PPI=100
Badges:             16×16     → Sprite at PPI=100
Gaiauge faces:      256×256   → Sprite at PPI=100
```

---

## V. IMPLEMENTATION ORDER — 6 Days

### Day 1: Design System + Textures
- [ ] Figma design system created with all color/type/spacing tokens
- [ ] Canva Brand Kit configured
- [ ] 8 UI textures generated (steel, concrete, grime, scratches, tape, glow, parchment, gauge)
- [ ] Import textures into Unity, verify tiling/seamlessness

### Day 2: Icons Batch 1 (Phase 11 + Core)
- [ ] 6 phase dot icons generated
- [ ] 6 danger skull icons generated
- [ ] 8 status badge icons generated
- [ ] Import all as SVG → Vector Image in Unity
- [ ] Wire into existing Phase 11 widget C# code

### Day 3: Icons Batch 2 (Expansions 3&4)
- [ ] 5 faction emblem icons (64×64)
- [ ] 5 tactical command icons
- [ ] 6 vehicle modification icons
- [ ] 4 relationship line icons
- [ ] Import + wire into Exp 3&4 widget C# code

### Day 4: Widget Backgrounds
- [ ] 7 full widget backgrounds generated
- [ ] 9-slice setup in Unity for MemorialWallUI, LoreCodexPanel, etc.
- [ ] USS updated with background-image references

### Day 5: UXML Completion + USS Polish
- [ ] All 11 missing UXML files created with correct element names
- [ ] USS extended with all widget-specific styles
- [ ] Animation keyframes added for pulse/flash/shake/scanline
- [ ] Responsive layout testing at 1920×1080 and 1366×768

### Day 6: Integration + Inspector + Testing
- [ ] All 19 widgets wired in Gameplay.unity Inspector
- [ ] PlayMode test: all widgets visible, reactive, correctly styled
- [ ] Save/Load round-trip: widgets preserve state
- [ ] Performance check: no GC alloc from UI updates

---

## VI. QUALITY STANDARDS — THE "PREMIUM" CHECKLIST

### Visual Quality
- [ ] No flat, untextured surfaces — every panel has subtle wear, scratches, or grain
- [ ] No pure white or pure black except in specific contexts (flat black for item icons)
- [ ] Amber glow is warm, inviting — not harsh or neon
- [ ] Typography is crisp, properly kerned, no aliasing
- [ ] Icons read at a glance without labels
- [ ] Animations are subtle (150-300ms) — not distracting
- [ ] Color-coding is consistent across all widgets (red= danger, amber=warning, green=good)

### UX Quality
- [ ] Critical information visible without clicking/tabbing
- [ ] Hover states provide feedback (subtle glow, not drastic change)
- [ ] Disabled states are clearly distinguishable (opacity 0.4, not hidden)
- [ ] Modal panels have clear close buttons and backdrop click-to-close
- [ ] Scrollable content has visible scrollbar with amber thumb
- [ ] No widget overlaps or clips at 1366×768 minimum resolution

### Atmosphere Quality
- [ ] The UI feels like it belongs in a bunker, not a spaceship
- [ ] The UI feels handmade — improvised, repaired, marked up
- [ ] The UI reinforces the game's tone (cold, exhausted, human, restrained)
- [ ] Nothing breaks the fourth wall (no "gamey" elements like XP bars, achievement popups)

---

## VII. TROUBLESHOOTING

| Problem | Likely Fix |
|---------|-----------|
| Canva AI generates photorealistic instead of hand-painted | Add "hand-painted, charcoal underdrawing, gouache texture, no photorealism" to negative |
| SVG icons pixelated in Unity | Set import type to "Vector Image" — SVG vectors scale infinitely |
| USS background-image not showing | Path must be `url("project://database/Assets/...")` format |
| 9-slice borders stretching wrong | Verify slice values in Sprite Editor — inner rect must be stretchable area |
| Canva Brand Kit not applying | Re-select all generated assets → Apply Brand Kit manually |
| Animation stuttering | Use `will-change: transform` on animated elements; keep frame count ≤3 |
| Colors different between Figma and Unity | Unity uses Linear color space by default — use hex values directly in USS, avoid color variables that may be gamma-corrected |
