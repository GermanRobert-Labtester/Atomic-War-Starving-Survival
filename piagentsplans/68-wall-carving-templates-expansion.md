# Plan 68 — Wall Carving Templates Expansion (3 bands → 60 templates)

## Goal (2 lines)
Expand `wall_carving_templates.json` from 3 morale bands with few templates to 3 bands
with 60 templates (20 per band). Wall carvings are the graffiti survivors scratch into
shelter walls — tally marks, crude drawings, names, warnings, prayers. They appear based
on shelter morale and tell the story of the community through its walls.

## Why (P2)
- Verified: `wall_carving_templates.json` has 3 morale bands (`high`, `medium`, `low`)
  with template strings. The system is wired (feeds the shelter-as-character pillar,
  existing 29A) but the template count is very thin — the same carvings repeat.
- Creates the shelter-texture pillar: wall carvings are the shelter's memory — high
  morale produces hopeful carvings (tally marks, drawings of the sun, names of the
  living); low morale produces desperate carvings (warnings, prayers, names of the
  dead). 60 templates ensures the walls always have something new to say.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/wall_carving_templates.json` (expand to 60 templates)
- Read-only: confirm the wall carving system consumer — `grep -rn "wall_carv\|WallCarv"
  Assets/Ashfall.Core/` to find the loader and confirm how templates are selected per
  morale band

## Content grammar (per template)
- A single string: 1-2 sentences of carved/found text. Not dialogue — these are marks
  on a wall.
- Must fit the morale band:
  - High (60-100): hopeful, defiant, communal — tally marks of days survived, drawings
    of the sun, names of the living, plans for spring, a child's drawing.
  - Medium (30-59): neutral, tired, persistent — ration counts, duty rosters scratched
    on stone, a warning about a drafty corridor, a note about a broken pipe.
  - Low (0-29): desperate, grieving, broken — names of the dead, prayers, warnings,
    confessions scratched in the dark, a single word ("why"), a date with no name.
- Tone: cold, exhausted, human, restrained. The carvings are physical, not literary.
  Skill `ashfall-write`.

## Steps
1. Find the wall carving system consumer to confirm the schema and template selection.
2. Read the 3 existing bands and their templates to understand the format.
3. Author 20 templates per band (60 total):
   - High morale (20): "Another day survived. The tally marks are getting longer.",
     "A crude drawing of the sun — someone still remembers what it looks like.", "Names
     of the living, carved deep.", "A handprint in the dust, pressed to the wall.", "The
     children drew flowers. Someone corrected the petals.", etc.
   - Medium morale (20): "Ration count: day 47. Holding.", "Duty roster scratched in
     stone — the ink ran out weeks ago.", "The east corridor leaks. Always has.", "Someone
     tallied the ammunition. The number is smaller than last week.", "A note: 'Filter
     needs replacing. Again.'", etc.
   - Low morale (20): "Names of the dead. The list is longer than the living.", "A prayer
     scratched in the dark. The words are misspelled.", "One word: 'why'", "A date with
     no name. Someone forgot who died that day.", "The tally marks stop. Someone stopped
     counting.", etc.
4. Write each template in ASHFALL tone — physical, not literary. Show through the mark,
   not through exposition.
5. Confirm the template selection is seed-deterministic (the system should rotate
   through templates per band using ISeededRng — confirm before authoring).
6. Validate: `--data-integrity-selftest`; confirm carvings appear on the correct morale
   band in a headless boot; confirm the rotation doesn't repeat the same template.
7. xUnit: template catalog loads, 20 templates per band, selection is deterministic
   (seeded), band selection matches morale level, save round-trip preserves carved
   state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring.

## Definition of Done
- `wall_carving_templates.json` has 60 templates (20 per band × 3 bands), all in ASHFALL
  tone, selection deterministic (seeded), band matches morale, save round-trip green,
  integrity + tests green.

## Follow-on
- Existing 29A (shelter as character) — wall carvings are the shelter's voice.
- Plan 41 (shelter rooms) — carvings appear in specific rooms.
- Existing 30A (folklore) — some carvings are folkloric (prayers, rituals).
- Plan 69 (grave epitaphs) — low-morale carvings overlap with memorial text.
