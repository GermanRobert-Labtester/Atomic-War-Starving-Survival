# Radio HUD Concept Exploration & Scorecard

## 1. Concept Overviews

### Concept 1: The Heterodyne Rack (Cold War Vacuum-Tube Receiver) [WINNER]
- **Visual Structure**: Stamped steel 19" rack bezel with brass screws and knurled aluminium tuning dials.
- **Left Column (Tuning & Diagnostics)**:
  - Analogue rotary frequency dial slider (50.0 – 150.0 MHz) with illuminated orange cursor needle.
  - S-Meter (Signal Strength 1..9, +20dB) rendered with green/amber/red arc.
  - Faction Emblem badge with live frequency lock LED.
  - Channel Presets buttons (12 faction quick-tune buttons with active frequency badges).
- **Right Column (CRT Intercept Monitor & Wiretap Log)**:
  - CRT amber scanline text monitor displaying live incoming transmission with character-by-character typewriter scroll.
  - Historical intercept scrollback buffer with timestamps (`[Day X - 14:32]`).
  - Squelch noise filter toggle & carrier lock indicator.

### Concept 2: The Scrambler Canteen (Portable Military Field Tactical Box)
- **Visual Structure**: Olive-drab canvas-wrapped field box with rubber gaskets and recessed toggle switches.
- **Layout**: Horizontal split with top LED audio spectrum bars and bottom ticker stream. Compact, tactical, but cramped for long lore transcripts at lower resolutions.

### Concept 3: The Squelch Deck (Improvised Scavenger Cassette Wiretap)
- **Visual Structure**: Asymmetrical scrap-metal enclosure with spliced audio patch cords and analog VU meters.
- **Layout**: Cluttered workbench look with magnetic tape spool animation. Highly stylized, but lower information density for strategic multi-faction tracking.

---

## 2. Concept Scorecard

| Evaluation Criteria (1-5) | Concept 1: The Heterodyne Rack | Concept 2: The Scrambler Canteen | Concept 3: The Squelch Deck |
| :--- | :---: | :---: | :---: |
| **Diegetic Atmosphere & Mood** | **5 / 5** (Immersive cold-war bunker wiretap) | **4 / 5** (Standard military tactical) | **4 / 5** (Scrap-yard workbench) |
| **Glanceability & Readability at 1366×768** | **5 / 5** (Clear 2-column split, large text area) | **4 / 5** (Horizontal squish on narrow screens) | **3 / 5** (Cluttered overlays obscure log) |
| **Information Density & Log Buffer** | **5 / 5** (Dedicated history scroll + tuning dial) | **4 / 5** (Short log history) | **4 / 5** (Limited log view) |
| **Design Token & 4px Grid Purity** | **5 / 5** (100% compliant with `Theme.cs` tokens) | **5 / 5** (Compliant) | **4 / 5** (Requires non-standard angles) |
| **Asset Reuse (12 Faction Emblems)** | **5 / 5** (Direct integration with existing 26 assets) | **4 / 5** (Smaller icon footprint) | **4 / 5** (Icons clipped) |
| **TOTAL SCORE** | **25 / 25 (WINNER)** | **21 / 25** | **19 / 25** |

---

## 3. Winner Selection Rationale

**Concept 1 (The Heterodyne Rack)** is selected for full production implementation because:
1. It delivers the grim, high-stakes atmosphere of an authentic underground surveillance station.
2. It scales perfectly from 1366×768 (laptops) to 1920×1080 and 2560×1080 (ultrawide) without layout deformation.
3. It unifies frequency tuning, signal strength analysis, and archival transcript review into a single cohesive control surface.
4. It consumes the exact 12 faction emblems generated in Stage 3/4 without requiring any visual compromises.
