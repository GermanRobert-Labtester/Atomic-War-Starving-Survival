# Plan 89 — Muster Epilogues Expansion (12 → 25 campaign-ending epilogues)

## Goal (2 lines)
Expand `muster_epilogues.json` from 12 verified epilogues to 25. The Muster
epilogue system (`EpilogueMatrix.cs` confirmed live) defines campaign-ending
prose — each ending has a key, title, and a paragraph of outcome text describing
what happened to the coalition. 12 epilogues is a good start but doesn't cover
the full branching outcome space.

## Why (P2)
- Verified: `muster_epilogues.json` has 12 entries (ending_key, title, prose).
  `EpilogueMatrix.cs` is confirmed in Core; `Main.Muster.cs` is confirmed in
  the Godot host. The existing 12 epilogues cover Muster and Verdict endings
  but miss many faction, resource, and moral-choice outcomes.
- Creates the ending-variety pillar: epilogues are the campaign's payoff —
  the player's choices across 300+ days resolve into a final paragraph that
  says what happened. 12 endings is one playthrough's worth; 25 creates
  meaningful replayability where different choices produce different endings.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/muster_epilogues.json` (expand 12 → 25 epilogues)
- Read-only: `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs` (confirm schema and
  how ending_key is selected — by flag combination? by faction standing?)

## Content grammar (per epilogue)
- `ending_key`: snake_case id (e.g. `the_open_muster`,
  `ending_verdict_the_count_is_held` — descriptive, may include expansion
  prefix like `ending_verdict_`).
- `title`: 2–6 words evoking the ending's tone.
- `prose`: 2–5 sentences of outcome text. Match the existing quality — each
  epilogue is a cold, specific, human summary of what happened. No moralizing,
  no triumphalism. The world is still broken; the question is how.
- Ending categories: Muster outcomes (rally, amnesty, corridor, blood price),
  Verdict outcomes (census, count, lease), faction outcomes (garrison,
  rebuilder, independent, foundry), resource outcomes (water, food, fuel,
  power), moral outcomes (mercy, cruelty, pragmatism, ideology), and compound
  outcomes (combinations of the above).

## Steps
1. Read `EpilogueMatrix.cs` to confirm how ending_key is selected (by flag
   combination? by faction standing? by a scoring matrix?).
2. Read the existing 12 epilogues to confirm the quality bar and the naming
   convention (Muster endings, Verdict endings).
3. Author 13 new epilogues across 5 categories:
   - Faction outcomes (4): the garrison absorbs the coalition; the coalition
     joins the Rebuilders; the coalition goes independent; the Foundry
     annexes the coalition.
   - Resource outcomes (3): the water plant is held; the grain silo is
     captured; the fuel depot burns (mutual destruction).
   - Moral outcomes (3): the mercy road (coalition spared enemies); the iron
     way (coalition eliminated rivals); the listener's thread (coalition
     chose diplomacy throughout).
   - Compound outcomes (2): the mercy road + water held (best-case); the iron
     way + fuel burned (worst-case).
   - Failure outcome (1): the shelter falls (coalition collapsed, the
     epilogue is about what the next survivor finds).
4. Each epilogue: distinct ending_key, title, and prose. Match the existing
   cold, grounded tone.
5. Cross-reference: every ending_key unique; check if ending_keys are
   referenced by flag definitions or quest outcomes (grep for ending_key
   references in other catalogs).
6. Validate: `--data-integrity-selftest` (all ids resolve).
7. xUnit: epilogue catalog loads 25 epilogues, all ending_keys unique, all
   prose non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is ending_key selection logic (step 1): confirm
how the system picks an ending before authoring — if it's flag-based, new
endings need flag combinations that can actually occur.

## Definition of Done
- `muster_epilogues.json` has 25 epilogues, all ending_keys unique, integrity
  + tests green.

## Follow-on
- Plan 96 (epilogue chronicle) — epilogue slides reference ending keys.
- Plan 74 (campaign chapters) — late chapters gate ending availability.
- Plan 66 (guilt sources) — moral choices determine which endings are
  available.
- Plan 44 (faction territory) — faction standing determines faction endings.
- Existing 15 (endgame meta) — this plan provides the ending data.
