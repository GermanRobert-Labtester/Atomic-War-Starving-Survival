# Plan 110 — Moral Choice Gossip Line Expansion (all bands → 20 lines)

## Goal (2 lines)
Expand `moral_choice_gossip.json` so every moral band in all three line
sections (camp_chatter, npc_greeting_shifts, whisper_lines) reaches 20 lines.
The gossip system (`MoralChoiceGossipRuntime.cs` confirmed live) propagates
ambient camp lines, NPC greeting shifts, and whispered reactions keyed to the
player's moral band. Current counts range 0–10 per band — far too few for a
system meant to make the shelter feel like it notices what you do.

## Why (P2)
- Verified: `moral_choice_gossip.json` has 3 sections × 7 bands. Counts:
  camp_chatter 5–10/band, npc_greeting_shifts 3–5/band, whisper_lines 0–6/band
  (slightly_positive whisper is empty). 7 bands × 3 sections = 21 arrays; most
  are below 10 lines. A player who hears the same whisper twice will notice.
- Gossip is the ambient texture that makes the moral-choice system feel alive
  without requiring new quests. It is the cheapest high-impact content lever.
- Pure DATA work — zero new Core code. `MoralChoiceGossipCatalogLoader.cs`
  loads the arrays; `MoralChoiceGossipRuntime.cs` selects a line by moral band.

## Files to touch
- `Assets/StreamingAssets/Data/moral_choice_gossip.json` (expand all 21
  arrays to 20 lines each)
- Read-only: `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipData.cs`
  (confirm line is a plain string array)
- Read-only: `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipRuntime.cs`
  (confirm band selection and any dedup/rotation logic)

## Content grammar (per line)
- Plain string. No JSON structure per line — the arrays are string[].
- Tone must match the band: very_positive (admiration), positive (warmth),
  slightly_positive (grudging approval), neutral (indifference),
  slightly_evil (wariness), evil (condemnation), very_evil (fear/loathing).
- Lines should reference concrete survivor situations, not abstractions
  ("They gave water to the raider with the broken gun" not "They are good").
- No line may duplicate another within the same band.

## Steps
1. Read `MoralChoiceGossipData.cs` to confirm each section's band arrays are
   `List<string>` and no per-line metadata is expected.
2. Read `MoralChoiceGossipRuntime.cs` to confirm band selection (does it
   rotate, pick random, or sequential?) and whether dedup is runtime or
   data-side.
3. For each of the 21 band arrays, audit existing lines and author enough new
   lines to reach 20. The empty `whisper_lines.slightly_positive` needs 20
   from scratch.
4. Author camp_chatter lines (7 bands × 20): overheard shelter conversations
   referencing specific moral acts the player performed.
5. Author npc_greeting_shifts (7 bands × 20): how named NPCs change their
   greeting tone based on the player's moral band.
6. Author whisper_lines (7 bands × 20): the quiet, deniable reactions —
   things said when the player is not supposed to hear.
7. Cross-reference: no line duplicated within a band; no line so generic it
   could belong to any band (each line should read as belonging to its band).
8. Wire 8 new gossip lines to reference echo quest outcomes from Plan 109
   (the camp talks about the delayed consequence that just happened).
9. Wire 6 new gossip lines to reference faction reactions from Plan 100.
10. Validate: `--data-integrity-selftest` (loads cleanly; no schema break).
11. xUnit: gossip catalog loads all 21 arrays at 20 lines each, no empty
    arrays, no duplicate strings within a band.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data, string arrays only. The one trap is tone consistency (step
7): each band must read distinctly. Review against existing lines before
authoring new ones.

## Definition of Done
- `moral_choice_gossip.json` has 21 arrays (3 sections × 7 bands) each at 20
  lines, no duplicates within a band, 8 lines wired to echo outcomes, 6 to
  faction reactions, integrity + tests green.

## Follow-on
- Plan 109 (echo quests) — gossip references echo outcomes.
- Plan 100 (faction reactions) — gossip references faction standing shifts.
- Plan 95 (journal voice) — gossip lines may unlock journal entries.
- Plan 92 (faction war dialogue) — gossip complements overheard dialogue.
- Plan 89 (epilogues) — gossip band at campaign end influences ending tone.
