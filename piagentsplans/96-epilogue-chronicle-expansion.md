# Plan 96 — Epilogue Chronicle Slides Expansion (5 → 20 ending slides)

## Goal (2 lines)
Expand `epilogue_chronicle.json` from 5 verified default slides to 20. The
epilogue chronicle system (`EpilogueChronicleBuilder.cs` confirmed live)
defines the ending slide sequence — each slide has an order, title, and art
asset id. 5 placeholder slides (Opening, The Bunker, What Remains, Survivors,
Final Word) is too few for a branching ending system with 25 epilogues
(Plan 89).

## Why (P2)
- Verified: `epilogue_chronicle.json` has 5 entries in `default_slides` (order,
  title, art_asset_id). `EpilogueChronicleBuilder.cs` is confirmed in Core.
  All 5 slides use placeholder art asset ids.
- Creates the ending-presentation pillar: the epilogue chronicle is the
  campaign's visual payoff — a sequence of slides showing what happened. 5
  generic slides is a placeholder; 20 slides creates a branching ending
  presentation where different epilogues (Plan 89) produce different slide
  sequences.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/epilogue_chronicle.json` (expand 5 → 20 slides)
- Read-only: `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`
  (confirm schema and how slides are selected — by ending_key? by flag
  combination?)

## Content grammar (per slide)
- `order`: integer (slide sequence position — 0, 1, 2, ...).
- `title`: 1–4 words evoking the slide's subject (Opening, The Bunker, What
  Remains, The Water Plant, The Grain Silo, The Foundry, The Muster, etc.).
- `art_asset_id`: asset id for the slide's art (existing slides use
  `epilogue_*_placeholder` — new slides should use the same placeholder
  convention until art is produced, or reference real art asset ids if they
  exist).
- Slide categories: opening slides (the exchange, the bunker, early
  survival), mid slides (factions, resources, key decisions), late slides
  (the coalition, the ending, the future).

## Steps
1. Read `EpilogueChronicleBuilder.cs` to confirm how slides are selected —
   are all `default_slides` always shown, or does the system pick a subset
   based on ending_key or flags?
2. Read the existing 5 slides to confirm the schema and the placeholder art
   convention.
3. Author 15 new slides across 3 categories:
   - Opening (3): The Exchange (the day of the war), The First Winter
     (early survival), The Shelter (the bunker as home).
   - Mid (7): The Factions (garrison, rebuilder, independent, foundry), The
     Resources (water, food, fuel, power), The Key Decisions (mercy road,
     iron way, listener's thread), The Investigations (Verdict sites), The
     Witnesses (muster testimony), The Radio (intercepted broadcasts), The
     Relics (restored artifacts).
   - Late (5): The Coalition (the muster), The Ending (the chosen epilogue),
     The Count (the Verdict census), The Future (what comes next), The Last
     Word (final reflection).
4. Each slide: distinct order, title, and art_asset_id. Use the placeholder
   convention (`epilogue_*_placeholder`) until art is produced.
5. Cross-reference: every slide order unique; every art_asset_id follows
   existing conventions; check if the system references ending_key to select
   slides (if so, slides may need ending_key associations).
6. Wire 5 slides to Plan 89 epilogues (each faction/resource/moral ending
  has a corresponding slide).
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: epilogue chronicle catalog loads 20 slides, all orders unique,
   all titles non-empty, all art_asset_ids non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is slide selection logic (step 1): confirm
whether the system shows all slides or picks a subset — if it picks by
ending_key, new slides need ending associations that the schema may not
support (check before authoring).

## Definition of Done
- `epilogue_chronicle.json` has 20 slides, all orders unique, 5 wired to
  epilogues, integrity + tests green.

## Follow-on
- Plan 89 (muster epilogues) — epilogues reference slides.
- Plan 74 (campaign chapters) — late chapters gate slide availability.
- Plan 87 (relic recipes) — relic slides show restored artifacts.
- Plan 82 (Verdict locations) — investigation slides show Verdict sites.
- Existing 15 (endgame meta) — this plan provides the ending-presentation
  data.
