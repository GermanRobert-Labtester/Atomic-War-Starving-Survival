# GAME_VISUAL_DNA.md

> Source of truth for every prompt in this system. CONFIRMED = directly supported by the repo. INFERRED = implied by several sources. PROPOSAL = a gap filled in for now, not canon. Don't silently promote a PROPOSAL to CONFIRMED.

## Game analysis (CONFIRMED)

**ASHFALL** (dev/working title; the shipped main-menu UI title is **LAST STATIC** — see Open Questions) — a 2D atomic-war survival-management sim. Unity 6 LTS, URP 2D, C#, ScriptableObject+JSON data pipeline, UI Toolkit. No runtime AI/LLM (Utility AI only).

- **Premise:** the player commands a fallout shelter under the fictional upland market town of **Tessarat**, after a nuclear exchange between a fictional central river-valley government and the upland provinces.
- **Loop:** shelter upkeep (power / air / water / heat) → survivor needs & psychology → radio/intel interception → surface scavenging expeditions → narrative crisis events → faction relations → 1 of 8 endgame paths.
- **Scale:** 419 items (17 categories), 96 survivor archetypes, 47 scavenging locations + 7 shelter rooms, 39+ narrative events, 15+ "Echoes" lore fragments, 50 radio broadcasts, 16 weather kinds, 5 factions, 8 victory paths.
- **Factions:** Central Garrison Remnants (disciplined military remnant, tactical gear), Upland Provincial Militia (agrarian, hunting gear, no medicine), Cultists of the Glow (radiation-worshipping, high-rad zones, psychological warfare), Scavenger Warlords (raiders, hatch raids), Safe Haven Communities (fragile civilian rebuilders).
- **Explicit guardrails** (`AGENTS.md`, GDD Ch1.2): no magic/fantasy, no real countries/wars/people, no glorified violence, no sci-fi mutants/lasers/aliens. Strict realism in post-nuclear physics/biology (dosimeters click, iodine protects only the thyroid, blood-type-matched transfusions, filters clog).

## Visual DNA

### Medium — CONFIRMED
2D **graphic-novel / comic-ink illustration**: heavy inked linework, cross-hatch shading, painted digital color over the linework. Confirmed by three independent sources that agree: the GDD ("2D gritty graphic-novel aesthetic... inspired by *This War of Mine* and *Darkest Dungeon*"), `prompts_for_ai.md`'s style guide, and the one finished reference piece (`UI_StyleReference_01.jpg`, the actual main-menu background art, also seen composited into the Figma mockup). Not soft-painterly, not flat-vector, not pixel art.

### Camera language
- **Interiors / key art — CONFIRMED:** eye-level to slightly elevated single-scene "stage" framing — a fixed backdrop behind diegetic UI, not an angled game-view tile. Confirmed by the actual reference art (a straight-on shelter room).
- **Characters — CONFIRMED:** 3/4 view or profile portraits; full-body sprites for in-world figures. Stated identically in `prompts_for_ai.md` and the existing (if noisy) prompt file.
- **Overworld — INFERRED:** the world map is an abstract node graph (`MapScreenUI`, `GeneratedMap`), not a walkable scene, so scavenging-location art is establishing/key-art illustration, not a rendered gameplay tile.
- ⚠️ **Contradicted, not adopted:** the existing `game_assets_prompts.md` claims environments should be "2.5D isometric perspective, emphasizing verticality." No other source supports this and it contradicts the one real reference image (flat, straight-on). Treated as a drafting error in that file — see `EXISTING_PROMPT_AUDIT.md`.

### Shape language — INFERRED
Improvised and asymmetric over clean/industrial: mismatched scavenged gear, hand-repaired objects, cluttered surfaces, patched fabric. Silhouettes read as *used*, not *designed*.

### Color — CONFIRMED two-tier system (important, non-obvious — don't collapse these into one palette)
1. **Illustrative/environmental tier** (characters, environments, props, key art): desaturated — cold ash blues, charcoal greys, rust oranges, muted mud browns — with warm amber practical-light accents (bulbs, radio dials, embers) against cool ambient/exterior light.
2. **UI semantic tier** (status/functional widgets): bright, saturated, near-Material-Design colors, pulled directly from the shipped token file `Assets/_Game/UI/Phase11/design-tokens.json` (CONFIRMED — it feeds real USS): `#4CAF50` green (safe/recovered), `#FFC107` amber (warning/detox), `#F44336` red (danger/withdrawal), `#9E9E9E` grey (neutral/numbed/fibrosis), `#2196F3` blue (recovery-state). These stay bright for at-a-glance legibility even in an otherwise desaturated game — don't mute them toward the illustrative palette.
   - A third, in-between warm sub-set lives in the same token file for narrative-flavor widgets specifically (not raw status): `phantom_motivation_sepia #b98a5e`, `phantom_breakdown_blue #6ea3a8`, `keepsake_gold #d3aa62`, `terminal_amber #f4c875`, `terminal_golden #d3aa62`, `terminal_failed #66675f`, `addiction_orange #c97b3a`, `addiction_withdrawal #e63333`, `addiction_detox #FFC107`, `addiction_recovered #4CAF50`. Use these exact values for the 8 Phase 11 widgets — don't re-derive by eye.

### Materials — INFERRED
Concrete, brick, rusted/corroded steel, cracked glass, worn canvas/fabric, damp wood, scavenged electronics with exposed wiring.

### Lighting — CONFIRMED
Deep chiaroscuro: a single warm practical light source (bare bulb, dial, ember, screen) against cool ambient or exterior light, strong cast shadows. Confirmed by GDD Ch1.2 and the reference image (bare bulbs + radio glow against a cold mushroom-cloud sky through a shattered window).

### Texture — CONFIRMED
Gritty, hand-inked, cross-hatched, worn/scratched/water-stained. Everything looks used, never new.

### Tone — CONFIRMED
"Cold, exhausted, human, restrained" — verbatim from the GDD and `AGENTS.md`, and literally the tone check in `docs/HUMAN_AUTHORSHIP.md`. Administrative, physical tragedy — not heroic, not romanticized, not gory-for-shock.

## ⚠️ Project constraint that changes what "done" means (CONFIRMED — read before generating anything)
`docs/HUMAN_AUTHORSHIP.md` and `docs/AI_DISCLOSURE.md` establish project policy: **no raw AI output ships.** Every AI-generated image from this system is a placeholder/reference/paint-over base — a human repaints, recolors, and composites before it enters the build. This doesn't change how prompts are written, but it changes the bar: these prompts need to produce good *starting points* for a human pass, not finished art. Strong silhouette clarity and consistency anchors (see `CONSISTENCY_ANCHORS.md`) matter more than surface finish, because that's what the paint-over artist works from.

## Open questions (flagged, not resolved here)
- **Naming:** docs/lore consistently say "ASHFALL (working title)." The actual shipped `Assets/_Game/UI/MainMenu/MainMenu.uxml` / `MainMenuController.cs` render the title **"LAST STATIC"** with a "© 2026 Northstar Interactive" credit. Which name is canonical for key art / a future title-card asset is a product decision — ask before generating any logo/title-card prompt.
- **"Kaliningrad":** the Figma main-menu mockup's debug footer reads "SECTOR 07 // KALININGRAD" — a real-world place name, conflicting with the "no real countries" rule and the fictional town of Tessarat used everywhere else. Likely stale placeholder text from an early exploration; flagged, not corrected here.
- **Item count drift:** the GDD header says "321 items"; `Assets/StreamingAssets/Data/items.json` currently holds 419. The catalog has grown since the doc was last regenerated (`generate_master_doc.py`) — treat `items.json` as ground truth for any full-catalog art batch.
