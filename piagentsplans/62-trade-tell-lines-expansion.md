# Plan 62 — Trade Tell Lines Expansion (4 bands → 60 tell lines)

## Goal (2 lines)
Expand `trade_tell_lines.json` from 4 trust bands with zero actual tell lines to 4 bands
with 60 tell lines (15 per band). The `TradeTellEngine` is fully implemented and reads
trust-band posture lines — but the catalog has the band structure with no line content.
The player reads the trader's posture through these lines.

## Why (P2)
- Verified: `trade_tell_lines.json` has 4 trust bands (`hostile`, `wary`, `neutral`,
  `warm`) with min/max ranges, but 0 actual tell lines. `TradeTellEngine.cs` is fully
  implemented. The system is wired but the content layer is empty.
- Creates the trade-reading pillar: the player reads the trader the way the trader reads
  the offer. Tell lines are terse, data-defined posture lines per stance × trust band,
  rotated seed-deterministically. Without them, the trade screen has no trader voice.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/trade_tell_lines.json` (add 60 tell lines, 15 per band)
- Read-only: `Assets/Ashfall.Core/Economy/TradeTellEngine.cs` (confirm the tell-line
  schema: which field holds the line text, how lines are selected per band, how the
  seed-deterministic rotation works)

## Content grammar (per tell line)
- Must fit within the existing trust-band structure (`hostile`, `wary`, `neutral`,
  `warm`).
- Tone: cold, exhausted, human, restrained (per the file's own description: "No modern
  slang, no anachronisms, no fourth-wall breaks").
- Each line is a terse posture read — what the trader's body language or tone says
  about their current stance toward the deal. Not dialogue; observation.
- Lines should reflect the trust band: hostile (suspicious, guarded, dismissive),
  wary (cautious, measuring, noncommittal), neutral (professional, flat, waiting),
  warm (relaxed, direct, almost friendly).
- Lines should vary by implied stance: the trader's posture changes based on whether
  the current offer favors them, is fair, or disadvantages them.

## Steps
1. Read `TradeTellEngine.cs` to confirm the tell-line schema, how lines are selected per
   band, and how the seed-deterministic rotation works (ISeededRng).
2. Read `trade_tell_lines.json` to confirm the 4-band structure and find where tell
   lines should be inserted (confirm the field name — is it `lines`, `tells`, `posture_lines`?).
3. Author 15 tell lines per band (60 total):
   - Hostile (15): suspicious posture, guarded stance, dismissive gesture, measuring
     look, hand near weapon, flat refusal tone, contemptuous glance, etc.
   - Wary (15): cautious tilt, noncommittal shrug, measuring the offer, half-turned,
     fingers drumming, weighing options, guarded interest, etc.
   - Neutral (15): professional stillness, flat expression, waiting for a better offer,
     hands flat on the table, steady gaze, no tells, patient, etc.
   - Warm (15): relaxed shoulders, almost a smile, direct eye contact, leaning in,
     open hands, favorable nod, almost friendly, etc.
4. Write each line in ASHFALL's tone (cold, exhausted, human, restrained). Use skill
   `ashfall-write` for voice consistency. Each line is 1 sentence, 5-15 words.
5. Confirm the seed-deterministic rotation: lines should not repeat in the same order
   every trade — the engine rotates through them per band using ISeededRng. Confirm
   the line list is ordered (the engine indexes by seed-derived position).
6. Validate: `--data-integrity-selftest`; confirm a trade screen displays tell lines
   from the correct band in a headless boot; confirm the rotation is deterministic
   (same seed → same line order).
7. xUnit: tell-line catalog loads, 15 lines per band, rotation is deterministic (seeded),
   band selection matches trust level, save round-trip preserves trade state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring. The one trap is the line-field name (step 2):
confirm where lines go in the JSON structure before authoring.

## Definition of Done
- `trade_tell_lines.json` has 60 tell lines (15 per band × 4 bands), all lines in
  ASHFALL tone, rotation deterministic (seeded), band selection matches trust level,
  save round-trip green, integrity + tests green.

## Follow-on
- Plan 61 (trade scenarios) — scenarios reference tell lines for negotiation options.
- Plan 13 (trade flow) — tell lines are the trader-voice layer of the economy.
- Plan 45 (patrols) — patrol negotiation encounters can use tell lines.
