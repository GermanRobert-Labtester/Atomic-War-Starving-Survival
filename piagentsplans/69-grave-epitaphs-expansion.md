# Plan 69 — Wasteland Grave Epitaphs Expansion (8 → 30 epitaphs)

## Goal (2 lines)
Expand `wasteland_grave_epitaphs.json` from 8 verified entries to 30. Grave epitaphs are
the text on improvised graves the player finds in the wasteland — each is a 1-sentence
memorial to someone who died. They are environmental storytelling at its purest: a life
reduced to one line on a cross or a stone.

## Why (P2)
- Verified: `wasteland_grave_epitaphs.json` has 8 entries (cause, epitaph). The grave
  system feeds the `MemorialSystem` (confirmed in `Assets/Ashfall.Core/Memorial/`).
- Creates the memorial-texture pillar: graves are the most common form of environmental
  storytelling in a post-apocalyptic world. 8 epitaphs means the player sees the same
  text repeatedly; 30 ensures variety across hundreds of grave encounters.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json` (expand 8 → 30 epitaphs)
- Read-only: `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` (confirm how epitaphs are
  selected per cause of death)

## Content grammar (per epitaph)
- cause: radiation / combat / starvation / exhaustion / disease / expedition / trauma /
  exposure / suicide / infection / old_age / drowning / frostbite / poisoning / execution /
  unknown.
- epitaph: 1 sentence, 5-20 words. Grounded, human, restrained. Not a eulogy — a mark
  on a grave. Show who they were through how they died, not through praise. Skill
  `ashfall-write`.

## Steps
1. Read `MemorialSystem.cs` to confirm how epitaphs are selected per cause of death.
2. Read the 8 existing epitaphs to understand the tone and avoid duplication.
3. Author 22 new epitaphs across 15 causes (1-2 per cause, filling gaps):
   - Radiation (2): "She walked into the grey and never came back the same.", "The
     dosimeter was still ticking when they found him."
   - Combat (2): "He held the line so others could retreat.", "She never saw the shot
     that took her."
   - Starvation (2): "The ration line ended before her turn came.", "He gave his shares
     to the children. It was enough for them, not for him."
   - Exhaustion (1): "Sleep finally took her, and did not return her."
   - Disease (2): "The pathogen came from somewhere; the names are forgotten.", "He
     lasted three days after the coughing started."
   - Expedition (2): "He walked out of the holdfast and did not walk back.", "The road
     took her. It takes everyone, eventually."
   - Trauma (1): "The blast finished what the war began."
   - Exposure (2): "She fell behind the column. The cold found her before the others
     did.", "He fell asleep in the snow and simply didn't wake up."
   - Suicide (2): "He chose his own ending. No one can say it was wrong.", "She left a
     note, but the rain took the words before anyone read them."
   - Infection (1): "A scratch became a fever became a grave."
   - Old age (2): "She outlived the world, but not her own time.", "He died warm, which
     is more than most can say."
   - Drowning (1): "The ice gave way. She went under and did not come up."
   - Frostbite (1): "His fingers went first, then his feet, then the rest of him."
   - Poisoning (1): "The water looked clean. It wasn't."
   - Execution (1): "They shot him against the wall. The wall is still there."
   - Unknown (1): "No one knows who they were. The grave was already here when we
     arrived."
4. Write each epitaph in ASHFALL tone — restrained, physical, human.
5. Confirm the selection is seed-deterministic (the system should select per cause using
   ISeededRng).
6. Validate: `--data-integrity-selftest`; confirm epitaphs appear on graves matching the
   cause of death in a headless boot.
7. xUnit: epitaph catalog loads, 30 entries, selection deterministic per cause, save
   round-trip preserves grave state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring.

## Definition of Done
- `wasteland_grave_epitaphs.json` has 30 epitaphs (8 existing + 22 new), all in ASHFALL
  tone, selection deterministic per cause, save round-trip green, integrity + tests green.

## Follow-on
- `MemorialSystem` — epitaphs are the text layer for in-shelter memorials.
- Plan 49 (micro-locations) — improvised graves are a micro-location type.
- Plan 68 (wall carvings) — low-morale carvings overlap with memorial text.
- Existing 30B (mourning rites) — graves feed the mourning system.
- Plan 65 (final wishes) — some graves are for survivors whose wishes were fulfilled
  or denied.
