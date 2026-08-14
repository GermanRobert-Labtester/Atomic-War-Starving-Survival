# Ashfall Design System Rules & Production Specification

## 1. Core Visual Principles
- **Theme**: Grim 2D survival-management (This War of Mine / Sheltered inspired).
- **Surface**: Muted desaturated ash greys (`#1A1A1A`, `#2C2C2C`), faded olive (`#66675F`), rust/amber accents (`#D3AA62`, `#F4C875`), blood red (`#E63333`).
- **No forbidden tropes**: Zero neon purple on dark, no unbudgeted gloss/specular bevels, no colored card borders without meaning.
- **Rhythm**: Strict 4px base grid (`Xs=4`, `Sm=8`, `Md=12`, `Lg=16`, `Xl=24`).
- **Typography Scale**: `Label=10px`, `Small=11px`, `Mono=12px`, `Body=14px`, `H3=18px`, `H2=22px`, `H1=28px`.

## 2. Token Mapping Table

| Token Identifier | Hex Code | Float RGBA | Usage |
| :--- | :--- | :--- | :--- |
| `Theme.Ink` | `#090B0C` | `(0.035, 0.043, 0.047, 1.0)` | Solid near-black background |
| `Theme.InkPanel` | `rgba(9,11,12,0.86)` | `(0.035, 0.043, 0.047, 0.86)` | Modal & Panel surface background |
| `Theme.Line` | `rgba(217,196,152,0.27)` | `(0.851, 0.769, 0.596, 0.27)` | Default border / divider |
| `Theme.LineSoft` | `rgba(217,196,152,0.14)` | `(0.851, 0.769, 0.596, 0.14)` | Subtle column divider |
| `Theme.Warm` | `#D3AA62` | `(0.827, 0.667, 0.384, 1.0)` | Primary accent & header title text |
| `Theme.Hot` | `#F4C875` | `(0.957, 0.784, 0.459, 1.0)` | Highlight / emphasis / fair barter status |
| `Theme.Pale` | `#E6E0D2` | `(0.902, 0.878, 0.824, 1.0)` | Primary readable body text |
| `Theme.Muted` | `#938F84` | `(0.576, 0.561, 0.518, 1.0)` | Secondary labels & neutral stance |
| `Theme.Dim` | `#66675F` | `(0.400, 0.404, 0.373, 1.0)` | Radio wiretap ticker & disabled controls |
| `Theme.Critical` | `#E63333` | `(0.902, 0.200, 0.200, 1.0)` | Critical warning / short offer status |
| `Theme.Entropy` | `#C97B3A` | `(0.788, 0.482, 0.227, 1.0)` | Rob stance / structural wear |
| `Theme.Lethe` | `#6EA3A8` | `(0.431, 0.639, 0.659, 1.0)` | Memory stratum / sight-gauge |

## 3. Auto-Layout & Sizing Constraints
- Root Trade Panel: `MinWidth = 560px`, `MaxHeight = 600px`.
- Header Bar: `Height = 48px`, `Padding = [12, 8, 12, 8]`.
- Faction Emblem: `40x40px` (or `64x64px` unscaled), `KeepAspectCentered`.
- Biological Offer Rows: `20x20px` icon, `Height = 28px`, `4px` gap.
- Scarcity Badges: `24x24px` (or `48x48px` unscaled).
- Price Shock Alerts: `16x16px` icon, horizontal layout with 8px separation.
- Viewport Responsiveness:
  - 1366×768: Anchored centered, scale 1.0, scrollable item lists.
  - 1920×1080: Anchored centered, scale 1.0, ample screen breathing room.
  - 2560×1080 (Ultrawide): Centered 21:9 pillarboxed layout, no horizontal stretching.

## 4. Production Asset Hygiene
- All icons and badges saved as 32-bit transparent PNGs.
- Kebab-case naming convention (`faction_icon_*.png`, `icon_bio_*.png`, `badge_scarcity_*.png`, `icon_shock_*.png`).
- 9-slice textures: `panel_bg_9slice.png` (border 16px), `header_bar_9slice.png` (border 12px x 8px).
- Maximum memory budget: 2 MB atlas size (current actual: ~25 KB).
